using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using HotelFinalProgramacionAvanzada.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HotelFinalProgramacionAvanzada.Controllers
{
    public class HabitacionController : Controller
    {
        public HabitacionController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        readonly IUnidadTrabajo _unidadTrabajo;

        [Authorize(Roles = Utility.SD.Roles.Administrador)]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = Utility.SD.Roles.Administrador)]
        [HttpGet]
        public IActionResult Upsert(int id = 0)
        {
            HabitacionViewModel modelo =
                new HabitacionViewModel
                {
                    TiposHabitacion = _unidadTrabajo.TiposHabitacion.Listar().ToList().ConvertAll(s => new SelectListItem(s.Nombre, s.TipoHabitacionId.ToString())),
                    Hoteles = _unidadTrabajo.Hoteles.Listar().ToList().ConvertAll(s => new SelectListItem(s.Nombre, s.HotelId.ToString()))
                };

            if (id == 0)
            {
                modelo.Habitacion = new Habitacion();
                return View(modelo);
            }
            else
            {
                var h = _unidadTrabajo.Habitaciones.Buscar(id);
                if (h == null)
                {
                    return NotFound();
                }

                modelo.Habitacion = h;
                return View(modelo);
            }
        }

        [Authorize(Roles = Utility.SD.Roles.Administrador)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, HabitacionViewModel modelo)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    _unidadTrabajo.Habitaciones.Agregar(modelo.Habitacion);
                    _unidadTrabajo.Guardar();
                }
                else
                {
                    try
                    {
                        _unidadTrabajo.Habitaciones.Actualizar(modelo.Habitacion);
                        _unidadTrabajo.Guardar();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_unidadTrabajo.Habitaciones.Buscar(modelo.Habitacion.HabitacionId) == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                return Json(new { success = true, message = "La habitacion ha sido guardada." });
            }

            return Json(new { success = false, message = "Ocurrió un error guardando la habitacion." });
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new { success = true, data = _unidadTrabajo.Habitaciones.Listar(propiedades: "TipoHabitacion,Hotel")});
        }

        [Authorize(Roles = Utility.SD.Roles.Administrador)]
        [HttpDelete]
        public IActionResult Borrar(int id)
        {
            var t = _unidadTrabajo.Habitaciones.Buscar(id);
            if (t == null)
            {
                return Json(new { success = false, message = "Habitacion no borrada." });
            }
            _unidadTrabajo.Habitaciones.Remover(t);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "La habitacion ha sido borrado." });
        }
    }
}

