using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using HotelFinalProgramacionAvanzada.Utility;
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

        [Authorize(Roles = SD.Roles.Administrador)]
        public IActionResult Upsert(int? id)
        {
            if (id == null)
                return View(new EstadoReserva());
            else
            {
                var t = _unidadTrabajo.ProcedimientoAlmacenado.Buscar<EstadoReserva>(new Dictionary<string, object> { { "@EstadoReservaId", id } });
                if (t == null)
                {
                    return NotFound();
                }
                return View(t);
            }
        }

        [Authorize(Roles = SD.Roles.Administrador)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(EstadoReserva estadoReserva)
        {
            if (ModelState.IsValid)
            {
                var parametros = new Dictionary<string, object>();
                parametros.Add("@NombreEstado", estadoReserva.NombreEstado);

                if (estadoReserva.EstadoReservaId != 0)
                {
                    parametros.Add("@EstadoReservaId", estadoReserva.EstadoReservaId);
                    _unidadTrabajo.ProcedimientoAlmacenado.Ejecutar(SD.Proc_EstadoReserva_Actualizar, parametros);
                }
                else
                {
                    _unidadTrabajo.ProcedimientoAlmacenado.Ejecutar(SD.Proc_EstadoReserva_Crear, parametros);
                }
                _unidadTrabajo.Guardar();
                return Json(new { success = true, message = "El estado de la reserva  ha sido guardado." });
            }
            return Json(new { success = false, message = "Ocurrió un error guardando el estado de la reserva ." });
        }

        #region Api Methods

        [Authorize(Roles = SD.Roles.Administrador)]
        [HttpDelete]
        public IActionResult Borrar(int id)
        {
            var estadoReserva = _unidadTrabajo.ProcedimientoAlmacenado.Buscar<EstadoReserva>(new Dictionary<string, object> { { "@EstadoReservaId", id } });

            if (estadoReserva == null)
            {
                return Json(new { success = false, message = "Se ha producido un error mientras se borraba el estado de la reserva." });
            }

            _unidadTrabajo.ProcedimientoAlmacenado.Ejecutar(SD.Proc_EstadoReserva_Borrar, new Dictionary<string, object> { { "@EstadoReservaId", id } });
            _unidadTrabajo.Guardar();

            return Json(new { success = true, message = "El estado de la reserva se ha borrado permanentemente." });
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new { data = _unidadTrabajo.ProcedimientoAlmacenado.Listar<EstadoReserva>() });
        }

        #endregion
    }
}

