﻿using HotelFinalProgramacionAvanzada.DataAccess.Data;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using HotelFinalProgramacionAvanzada.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelFinalProgramacionAvanzada.Controllers
{

    public class UsuarioController : Controller
    {
        public UsuarioController(ApplicationDbContext db)
        {
            _db = db;
        }

        readonly ApplicationDbContext _db;

        public IActionResult Index()
        {
            return View();
        }

        #region Api Methods

        //[HttpPost]
        //public IActionResult LockUnlock([FromBody] string id)
        //{
        //    var usuario = _db.Usuarios.FirstOrDefault(s => s.Id == id);

        //    if (usuario == null)
        //    {
        //        return Json(new { success = false, message = "Se ha producido un error mientras se bloqueaba/desbloqueaba." });
        //    }

        //    if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
        //    {
        //        usuario.LockoutEnd = null;
        //    }
        //    else
        //    {
        //        usuario.LockoutEnd = DateTime.Now.AddYears(1000);
        //    }

        //    _db.SaveChanges();
        //    return Json(new { success = true, message = "El usuario se ha bloqueado/desbloqueado." });
        //}

        [HttpGet]
        public IActionResult Listar()
        {
            var usuarios = _db.HotelEmpleados.Include(i => i.Usuario).Include(h => h.Hotel).ToList();
            var userRoles = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            usuarios.ForEach
                (
                    usuario =>
                    {
                        var roleId = userRoles.FirstOrDefault(s => s.UserId == usuario.Usuario.Id).RoleId;
                            usuario.Usuario.Role = roles.FirstOrDefault(s => s.Id == roleId).Name;
                        if (usuario.Hotel == null)
                        {
                            usuario.Hotel = new Hotel { Nombre = string.Empty};
                        }
                    }
                );

            return Json(new { data = usuarios });
        }
        #endregion
    }
}
