using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using HotelFinalProgramacionAvanzada.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace HotelFinalProgramacionAvanzada.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<ExternalLoginModel> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public ExternalLoginModel(
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            ILogger<ExternalLoginModel> logger,
            RoleManager<IdentityRole> roleManager,
            IEmailSender emailSender,
            IUnidadTrabajo unidadTrabajo)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _unidadTrabajo = unidadTrabajo;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ProviderDisplayName { get; set; }

        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
            [Required]
            public string Nombre { get; set; }
            [Required]
            public string Apellido { get; set; }
        }

        public IActionResult OnGetAsync()
        {
            return RedirectToPage("./Login");
        }

        public IActionResult OnPost(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return new ChallengeResult(provider, properties);
        }

        public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (remoteError != null)
            {
                ErrorMessage = $"Error from external provider: {remoteError}";
                return RedirectToPage("./Login", new {ReturnUrl = returnUrl });
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor : true);
            if (result.Succeeded)
            {
                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                return LocalRedirect(returnUrl);
            }
            if (result.IsLockedOut)
            {
                return RedirectToPage("./Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ReturnUrl = returnUrl;
                ProviderDisplayName = info.ProviderDisplayName;
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    Input = new InputModel
                    {
                        Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                    };
                }
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            // Get the information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ErrorMessage = "Error loading external login information during confirmation.";
                return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
            }

            if (ModelState.IsValid)
            {

                await Setup.InitAsync(_userManager, _roleManager);
                creaEstadosreservaBase();
                creaTiposHabitacionreservaBase();
                creaHotelesBase();


                var user =
                    new Usuario
                    {
                        UserName = Input.Email,
                        Email = Input.Email,
                        Nombre = Input.Nombre,
                        Apellido = Input.Apellido
                    };

                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, SD.Roles.Cliente);

                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code },
                            protocol: Request.Scheme);

                        await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        // If account confirmation is required, we need to show the link if we don't have a real email sender
                        if (_userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return RedirectToPage("./RegisterConfirmation", new { Email = Input.Email });
                        }

                        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            ProviderDisplayName = info.ProviderDisplayName;
            ReturnUrl = returnUrl;
            return Page();
        }

        public void creaEstadosreservaBase()
        {
            _unidadTrabajo.EstadosReserva.Agregar(new EstadoReserva { NombreEstado = SD.EstadosReserva.Adelanto });
            _unidadTrabajo.EstadosReserva.Agregar(new EstadoReserva { NombreEstado = SD.EstadosReserva.Total });
            _unidadTrabajo.EstadosReserva.Agregar(new EstadoReserva { NombreEstado = SD.EstadosReserva.Suspendida });
        }

        public void creaTiposHabitacionreservaBase()
        {
            _unidadTrabajo.TiposHabitacion.Agregar(new TipoHabitacion { Nombre = SD.TiposHabitacion.Individual, Descripcion = SD.TiposHabitacion.IndividualDesc, CostoNoche = 40000, ImagenTipo = SD.TiposHabitacion.IndividualImg });
            _unidadTrabajo.TiposHabitacion.Agregar(new TipoHabitacion { Nombre = SD.TiposHabitacion.Doble, Descripcion = SD.TiposHabitacion.DobleDesc, CostoNoche = 70000, ImagenTipo = SD.TiposHabitacion.DobleImg });
            _unidadTrabajo.TiposHabitacion.Agregar(new TipoHabitacion { Nombre = SD.TiposHabitacion.DobleSuperior, Descripcion = SD.TiposHabitacion.DobleSuperiorDesc, CostoNoche = 80000, ImagenTipo = SD.TiposHabitacion.DobleSuperiorImg });
        }

        public void creaHotelesBase()
        {
            _unidadTrabajo.Hoteles.Agregar(new Hotel { Nombre = SD.Hoteles.Hotel1, Descripcion = SD.Hoteles.Hotel1Desc, UrlImagen = SD.Hoteles.Hotel1Img, Direccion = SD.Hoteles.Hotel1Direc, Ciudad = SD.Hoteles.Hotel1Ciu, Telefono = SD.Hoteles.Hotel1Tel });
            _unidadTrabajo.Hoteles.Agregar(new Hotel { Nombre = SD.Hoteles.Hotel2, Descripcion = SD.Hoteles.Hotel2Desc, UrlImagen = SD.Hoteles.Hotel2Img, Direccion = SD.Hoteles.Hotel2Direc, Ciudad = SD.Hoteles.Hotel2Ciu, Telefono = SD.Hoteles.Hotel2Tel });
            _unidadTrabajo.Hoteles.Agregar(new Hotel { Nombre = SD.Hoteles.Hotel3, Descripcion = SD.Hoteles.Hotel3Desc, UrlImagen = SD.Hoteles.Hotel3Img, Direccion = SD.Hoteles.Hotel3Direc, Ciudad = SD.Hoteles.Hotel3Ciu, Telefono = SD.Hoteles.Hotel3Tel });
        }
    }
}
