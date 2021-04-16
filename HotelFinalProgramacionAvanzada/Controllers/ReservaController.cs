using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using HotelFinalProgramacionAvanzada.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HotelFinalProgramacionAvanzada.Controllers
{
    public class ReservaController : Controller
    {
        public ReservaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        readonly IUnidadTrabajo _unidadTrabajo;

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int id = 0)
        {
            ReservaViewModel modelo =
                new ReservaViewModel
                {
                    Habitaciones = _unidadTrabajo.Habitaciones.Listar().ToList().ConvertAll(s => new SelectListItem(s.Nombre, s.HabitacionId.ToString())),
                    EstadosReserva = _unidadTrabajo.EstadosReserva.Listar().ToList().ConvertAll(s => new SelectListItem(s.NombreEstado, s.EstadoReservaId.ToString())),
                    Usuarios = _unidadTrabajo.Usuarios.Listar().ToList().ConvertAll(s => new SelectListItem(s.Nombre + " " + s.Apellido, s.Id.ToString()))
                };

            if (id == 0)
            {
                modelo.Reserva = new Reserva();
                return View(modelo);
            }
            else
            {
                var h = _unidadTrabajo.Reservas.Buscar(id);
                if (h == null)
                {
                    return NotFound();
                }

                modelo.Reserva = h;
                return View(modelo);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, ReservaViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    _unidadTrabajo.Reservas.Agregar(modelo.Reserva);
                    _unidadTrabajo.Guardar();
                }
                else
                {
                    try
                    {
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
                }

                return Json(new { success = true, message = "La Reserva ha sido guardada." });
            }

            return Json(new { success = false, message = "Ocurrió un error guardando la Reserva." });
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new { success = true, data = _unidadTrabajo.Reservas.Listar(propiedades: "Usuario,Habitacion.Hotel")});
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
    }
}

