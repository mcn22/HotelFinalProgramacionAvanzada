namespace HotelFinalProgramacionAvanzada.Controllers
{
    using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
    using HotelFinalProgramacionAvanzada.Models;
    using HotelFinalProgramacionAvanzada.Models.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Security.Claims;

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
            modelo.Reserva.FechaSalida = DateTime.Now;
            modelo.Reserva.FechaSalida.AddDays(2);
            return View(modelo);
        }        
        

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
                    return Json(new { success = false, message = "No hay disponibilidad." });
                }
            }
            else
            {
                return RedirectToAction("../Account/Login");
            }
        }


        public IActionResult ConfirmarReserva(Reserva modelo)
        {
            ReservaDemoViewModel modeloDemo = new ReservaDemoViewModel();
            modeloDemo.Llegada = modelo.FechaLlegada.ToString();
            modeloDemo.Salida = modelo.FechaSalida.ToString();
            modeloDemo.Reserva = modelo;
            return View(modeloDemo);
        }

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
                return RedirectToAction("../Reserva/Index");
            }
            return Json(new { success = false, message = "Ocurrió un error guardando la Reserva." });
        }

        //[HttpGet]
        //public IActionResult Upsert(int id = 0)
        //{
        //    ReservaViewModel modelo =
        //        new ReservaViewModel
        //        {
        //            Habitaciones = _unidadTrabajo.Habitaciones.Listar().ToList().ConvertAll(s => new SelectListItem(s.Nombre, s.HabitacionId.ToString())),
        //            EstadosReserva = _unidadTrabajo.EstadosReserva.Listar().ToList().ConvertAll(s => new SelectListItem(s.NombreEstado, s.EstadoReservaId.ToString())),
        //            Usuarios = _unidadTrabajo.Usuarios.Listar().ToList().ConvertAll(s => new SelectListItem(s.Nombre + " " + s.Apellido, s.Id.ToString()))
        //        };

        //    if (id == 0)
        //    {
        //        modelo.Reserva = new Reserva();
        //        return View(modelo);
        //    }
        //    else
        //    {
        //        var h = _unidadTrabajo.Reservas.Buscar(id);
        //        if (h == null)
        //        {
        //            return NotFound();
        //        }

        //        modelo.Reserva = h;
        //        return View(modelo);
        //    }
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Upsert(int id, ReservaViewModel modelo)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (id == 0)
        //        {
        //            _unidadTrabajo.Reservas.Agregar(modelo.Reserva);
        //            _unidadTrabajo.Guardar();
        //        }
        //        else
        //        {
        //            try
        //            {
        //                _unidadTrabajo.Reservas.Actualizar(modelo.Reserva);
        //                _unidadTrabajo.Guardar();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (_unidadTrabajo.Reservas.Buscar(modelo.Reserva.ReservaId) == null)
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //        }

        //        return Json(new { success = true, message = "La Reserva ha sido guardada." });
        //    }

        //    return Json(new { success = false, message = "Ocurrió un error guardando la Reserva." });
        //}

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new { success = true, data = _unidadTrabajo.Reservas.Listar(propiedades: "Usuario,Habitacion.Hotel") });
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
