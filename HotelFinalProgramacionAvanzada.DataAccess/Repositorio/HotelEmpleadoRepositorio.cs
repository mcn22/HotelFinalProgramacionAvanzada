using HotelFinalProgramacionAvanzada.DataAccess.Data;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HotelFinalProgramacionAvanzada.DataAccess.Repositorio
{
    public class HotelEmpleadoRepositorio : Repositorio<HotelEmpleado>, IHotelEmpleadoRepositorio
    {
        public HotelEmpleadoRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        readonly ApplicationDbContext _db;

        public void Actualizar(HotelEmpleado hotelEmpleado)
        {
            var l = _db.HotelEmpleados.FirstOrDefault(s => s.HotelEmpleadoId == hotelEmpleado.HotelEmpleadoId);

            if (l == null)
                return;

            l.UserId = hotelEmpleado.UserId;
            l.HotelId = hotelEmpleado.HotelId;
        }
    }
}
