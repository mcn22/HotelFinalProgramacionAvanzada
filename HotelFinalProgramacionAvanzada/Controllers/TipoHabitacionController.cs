using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelFinalProgramacionAvanzada.Controllers
{
    public class TipoHabitacionController : Controller
    {
        public TipoHabitacionController(IUnidadTrabajo unidadTrabajo)
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
            if (id == 0)
                return View(new TipoHabitacion());
            else
            {
                var t = _unidadTrabajo.TiposHabitacion.Buscar(id);
                if (t == null)
                {
                    return NotFound();
                }
                return View(t);
            }
        }

        [Authorize(Roles = Utility.SD.Roles.Administrador)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(int id, TipoHabitacion tipoHabitacion)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    _unidadTrabajo.TiposHabitacion.Agregar(tipoHabitacion);
                    _unidadTrabajo.Guardar();
                }
                else
                {
                    try
                    {
                        _unidadTrabajo.TiposHabitacion.Actualizar(tipoHabitacion);
                        _unidadTrabajo.Guardar();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_unidadTrabajo.TiposHabitacion.Buscar(tipoHabitacion.TipoHabitacionId) == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return Json(new { success = true, message = "El tipo de habitacion  ha sido guardado." });
            }
            return Json(new { success = false, message = "Ocurrió un error guardando el tipo de habitacion ." });
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new { success = true, data = _unidadTrabajo.TiposHabitacion.Listar()});
        }

        [Authorize(Roles = Utility.SD.Roles.Administrador)]
        [HttpDelete]
        public IActionResult Borrar(int id)
        {
            var t = _unidadTrabajo.TiposHabitacion.Buscar(id);
            if (t == null)
            {
                return Json(new { success = false, message = "Tipo de habitacion  no borrado." });
            }
            _unidadTrabajo.TiposHabitacion.Remover(t);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "El tipo de habitacion ha sido borrado." });
        }
    }
}
