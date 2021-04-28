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
    public class EstadoReservaController : Controller
    {
        public EstadoReservaController(IUnidadTrabajo unidadTrabajo)
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
                return View(new EstadoReserva());
            else
            {
                var t = _unidadTrabajo.EstadosReserva.Buscar(id);
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
        public IActionResult Upsert(int id, EstadoReserva EstadoReserva)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    _unidadTrabajo.EstadosReserva.Agregar(EstadoReserva);
                    _unidadTrabajo.Guardar();
                }
                else
                {
                    try
                    {
                        _unidadTrabajo.EstadosReserva.Actualizar(EstadoReserva);
                        _unidadTrabajo.Guardar();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (_unidadTrabajo.EstadosReserva.Buscar(EstadoReserva.EstadoReservaId) == null)
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return Json(new { success = true, message = "El estado de la reserva  ha sido guardado." });
            }
            return Json(new { success = false, message = "Ocurrió un error guardando el estado de la reserva ." });
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new { success = true, data = _unidadTrabajo.EstadosReserva.Listar() });
        }

        [Authorize(Roles = Utility.SD.Roles.Administrador)]
        [HttpDelete]
        public IActionResult Borrar(int id)
        {
            var t = _unidadTrabajo.EstadosReserva.Buscar(id);
            if (t == null)
            {
                return Json(new { success = false, message = "Estado de la reserva no borrado." });
            }
            _unidadTrabajo.EstadosReserva.Remover(t);
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "El estado de la reserva ha sido borrado." });
        }
    }
}

