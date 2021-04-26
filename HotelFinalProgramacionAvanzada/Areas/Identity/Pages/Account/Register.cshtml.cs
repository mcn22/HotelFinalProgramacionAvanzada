﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using HotelFinalProgramacionAvanzada.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace HotelFinalProgramacionAvanzada.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnidadTrabajo unidadTrabajo)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unidadTrabajo = unidadTrabajo;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "Este campo es requerido.")]
            [MaxLength(56, ErrorMessage = "El máximo es de 56 caracteres")]
            public string Nombre { get; set; }

            [Required(ErrorMessage = "Este campo es requerido.")]
            [MaxLength(56, ErrorMessage = "El máximo es de 56 caracteres")]
            public string Apellido { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Role { get; set; }
            public int? HotelId { get; set; }
            public IEnumerable<SelectListItem> Hoteles { get; set; }
            public IEnumerable<SelectListItem> Roles { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            Input =
                new InputModel
                {
                    Hoteles = _unidadTrabajo.Hoteles.Listar().Select(s => new SelectListItem { Text = s.Nombre, Value = s.HotelId.ToString() }),
                    Roles = _roleManager.Roles.Where(w => w.Name != SD.Roles.Cliente).Select(s => s.Name).Select(s => new SelectListItem { Text = s, Value = s })
                };
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new Usuario
                {
                    UserName = Input.Email,
                    Email = Input.Email,
                    Nombre = Input.Nombre,
                    Apellido = Input.Apellido,
                    HotelId = Input.HotelId,
                    Role = Input.Role
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await Setup.InitAsync(_userManager, _roleManager);
                    creaEstadosreservaBase();
                    creaTiposHabitacionreservaBase();
                    creaHotelesBase();

                    if (string.IsNullOrEmpty(Input.Role))
                    {
                        await _userManager.AddToRoleAsync(user, SD.Roles.Cliente);
                    }
                    else
                    {
                        var hotelEmpleado = new HotelEmpleado();
                        if (Input.Role.ToString().Equals(SD.Roles.Empleado.ToString()))
                        {
                            hotelEmpleado = new HotelEmpleado
                            {
                                UserId = user.Id,
                                HotelId = Input.HotelId
                            };
                        }
                        else
                        {
                            hotelEmpleado = new HotelEmpleado
                            {
                                UserId = user.Id,
                                HotelId = null
                            };
                        }
                        _unidadTrabajo.HotelEmpleados.Agregar(hotelEmpleado);
                        _unidadTrabajo.Guardar();
                        await _userManager.AddToRoleAsync(user, user.Role);
                    }
                    // Fin del codigo para ejecutar despues de que se tiene un usuario y rol admin

                    ///////////////////////////
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    //Evitar el envio del correo por ahora
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
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