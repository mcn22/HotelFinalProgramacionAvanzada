using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using HotelFinalProgramacionAvanzada.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Claims;

namespace HotelFinalProgramacionAvanzada.Controllers
{
    public class ReservaController : Controller
    {
        public ReservaController(IUnidadTrabajo unidadTrabajo, UserManager<IdentityUser> userManager)
        {
            _unidadTrabajo = unidadTrabajo;
            _userManager = userManager;
        }

        private readonly UserManager<IdentityUser> _userManager;
        internal readonly IUnidadTrabajo _unidadTrabajo;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult InicioReserva()
        {
            var modelo = _unidadTrabajo.Hoteles.Listar().ToList();
            return View(modelo);
        }

        public IActionResult PreReserva(int id = 0)
        {
            ReservaViewModel modelo =
                new ReservaViewModel
                {
                    Hotel = _unidadTrabajo.Hoteles.Buscar(id),
                    TiposHabitacionList = _unidadTrabajo.TiposHabitacion.Listar().ToList(),
                    TiposHabitacionDD = _unidadTrabajo.TiposHabitacion.Listar().ToList().ConvertAll(s => new SelectListItem(s.Nombre, s.TipoHabitacionId.ToString())),
                };
            modelo.Reserva = new Reserva();
            modelo.Reserva.FechaLlegada = DateTime.Now;
            modelo.Reserva.FechaSalida = DateTime.Now.AddDays(1);
            return View(modelo);
        }

        [Authorize(Roles = Utility.SD.Roles.Cliente)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PreReserva(ReservaViewModel modelo)
        {
            if (User.Identity.IsAuthenticated)
            {               
                int idHabitacion = ValidaDisponibilidad(modelo.Reserva.FechaLlegada, modelo.Reserva.FechaSalida, modelo.Hotel.HotelId, modelo.TipoHabitacionId);
                if (idHabitacion != 0)
                {                
                    modelo.DiasHospedaje = CalculaDiasHospedaje(modelo.Reserva.FechaLlegada, modelo.Reserva.FechaSalida);
                    modelo.Reserva.CostoTotal = CalculaCosto(modelo.DiasHospedaje, modelo.TipoHabitacionId);
                    modelo.Reserva.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    modelo.Reserva.HabitacionId = idHabitacion;
                    return RedirectToAction("ConfirmarReserva",modelo.Reserva);
                }
                else
                {
                    //return RedirectToAction("PreReserva", new { id = modelo.Hotel.HotelId });
                    TempData["notificacion"] = "Error";
                    return RedirectToAction("PreReserva", new { id = modelo.Hotel.HotelId});
                }
            }
            else
            {
                return RedirectToAction("../Account/Login");
            }
        }

        [Authorize(Roles = Utility.SD.Roles.Cliente)]
        public IActionResult ConfirmarReserva(Reserva modelo)
        {
            ReservaDemoViewModel modeloDemo = new ReservaDemoViewModel();
            modeloDemo.Llegada = modelo.FechaLlegada.ToString();
            modeloDemo.Salida = modelo.FechaSalida.ToString();
            modeloDemo.Reserva = modelo;
            return View(modeloDemo);
        }

        [Authorize(Roles = Utility.SD.Roles.Cliente)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarReserva(ReservaDemoViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.Reserva.Saldo = modelo.Reserva.CostoTotal - (modelo.Reserva.CostoTotal * (decimal)0.30);
                modelo.Reserva.EstadoReservaId = _unidadTrabajo.EstadosReserva.Listar().Where(e => e.NombreEstado == Utility.SD.EstadosReserva.Adelanto)
                            .FirstOrDefault().EstadoReservaId;
                _unidadTrabajo.Reservas.Agregar(modelo.Reserva);
                _unidadTrabajo.Guardar();
                return RedirectToAction("Index");
            }
            return Json(new { success = false, message = "Ocurrió un error guardando la Reserva." });
        }

        [Authorize(Roles = ""+Utility.SD.Roles.Administrador+","+Utility.SD.Roles.Empleado + "")]
        public IActionResult CambiaEstado(int id = 0)
        {
            CambioEstadoReservaViewModel modelo =
                new CambioEstadoReservaViewModel
                {
                    TiposEstadoDD = _unidadTrabajo.EstadosReserva.Listar().ToList().ConvertAll(s => new SelectListItem(s.NombreEstado, s.EstadoReservaId.ToString()))
                };
                var reserva = _unidadTrabajo.Reservas.Buscar(id);
                if (reserva == null)
                {
                    return NotFound();
                }
                modelo.Reserva = reserva;
                return View(modelo);         
        }

        [Authorize(Roles = "" + Utility.SD.Roles.Administrador + "," + Utility.SD.Roles.Empleado + "")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CambiaEstado(int id, CambioEstadoReservaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var idSuspendida = _unidadTrabajo.EstadosReserva.Listar().Where(e => e.NombreEstado == Utility.SD.EstadosReserva.Suspendida)
                            .FirstOrDefault().EstadoReservaId;
                    if (modelo.Reserva.EstadoReservaId != idSuspendida)
                    {
                        modelo.Reserva.Saldo = 0;
                    }                  
                    _unidadTrabajo.Reservas.Actualizar(modelo.Reserva);
                    _unidadTrabajo.Guardar();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_unidadTrabajo.Reservas.Buscar(modelo.Reserva.ReservaId) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { success = true, message = "La Reserva ha sido actualizada." });
            }

            return Json(new { success = false, message = "Ocurrió un error guardando la Reserva." });
        }

        public IActionResult Detalle(int id = 0)
        {            
            var reserva = _unidadTrabajo.Reservas.Buscar(id);
            DetalleReservaViewModel modelo =
                new DetalleReservaViewModel
                {
                    Estado = _unidadTrabajo.EstadosReserva.Buscar(reserva.EstadoReservaId).NombreEstado
                };

            if (reserva == null)
            {
                return NotFound();
            }
            modelo.Reserva = reserva;
            return View(modelo);
        }

        [HttpGet]
        public IActionResult Listar()
        {
            if (User.IsInRole(Utility.SD.Roles.Cliente))
            {
                return Json(new { success = true, data = _unidadTrabajo.Reservas.Listar(propiedades: "Habitacion.TipoHabitacion,Habitacion,Habitacion.Hotel").
                    Where(u => u.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier)) });
            }
            else if (User.IsInRole(Utility.SD.Roles.Empleado)){
                var hotel = _unidadTrabajo.HotelEmpleados.Buscar(u => u.UserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
                return Json(new { success = true, data = _unidadTrabajo.Reservas.Listar(propiedades: "Usuario,Habitacion,EstadoReserva").Where(h => h.Habitacion.HotelId == hotel.HotelId).ToList()
                
                });
        }
            else {
                return Json(new { success = true, data = _unidadTrabajo.Reservas.Listar(propiedades: "Usuario,Habitacion,EstadoReserva") });
            }
        }

        [HttpDelete]
        public IActionResult Borrar(int id)
        {
            var t = _unidadTrabajo.Reservas.Buscar(id);
            if (t == null)
            {
                return Json(new { success = false, message = "Reserva no borrada." });
            }
            _unidadTrabajo.Reservas.Remover(t);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "La Reserva ha sido borrado." });
        }

        private int CalculaDiasHospedaje(DateTime llegada, DateTime salida)
        {
            return Convert.ToInt32((salida - llegada).TotalDays);
        }

        private int CalculaCosto(int dias, int tipoHabitacionId)
        {
            var habitacion = _unidadTrabajo.TiposHabitacion.Buscar(tipoHabitacionId);
            int costo = dias * Convert.ToInt32(habitacion.CostoNoche);
            return costo;
        }

        public int ValidaDisponibilidad(DateTime llegada, DateTime salida, int hotelId, int tipoHabitacionId)
        {
            int resultado = 0;
            try
            {
                var habitaciones = _unidadTrabajo.Habitaciones.Listar().Where(k => k.HotelId == hotelId && k.TipoHabitacionId == tipoHabitacionId).ToList();
                var estadoCancelado = _unidadTrabajo.EstadosReserva.Listar().Where(e => e.NombreEstado == Utility.SD.EstadosReserva.Suspendida).FirstOrDefault();
                foreach (var habitacion in habitaciones)
                {
                    var reservas = _unidadTrabajo.Reservas.Listar().Where(r => r.Habitacion.HabitacionId == habitacion.HabitacionId && 
                    r.EstadoReservaId != estadoCancelado.EstadoReservaId).ToList();
                    if (reservas.Count == 0)
                    {
                        resultado = habitacion.HabitacionId;
                        break;
                    }
                    else
                    {
                        foreach (var reserva in reservas)
                        {

                            if (llegada >= Convert.ToDateTime(reserva.FechaLlegada) && llegada <= Convert.ToDateTime(reserva.FechaSalida) ||
                                salida >= Convert.ToDateTime(reserva.FechaLlegada) && salida <= Convert.ToDateTime(reserva.FechaSalida))
                            {
                                resultado = 0;
                                break;
                            }
                            else
                            {
                                resultado = habitacion.HabitacionId;
                            }

                        }
                    }
                }
            }
            catch (Exception)
            {
                resultado = 0;
            }
            return resultado;
        }
    }
}


