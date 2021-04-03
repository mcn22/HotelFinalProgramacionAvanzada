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
    public class HabitacionRepositorio : Repositorio<Habitacion>, IHabitacionRepositorio
    {
        public HabitacionRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        readonly ApplicationDbContext _db;

        public void Actualizar(Habitacion habitacion)
        {
            var l = _db.Habitaciones.FirstOrDefault(s => s.HabitacionId == habitacion.HabitacionId);

            if (l == null)
                return;

            l.TipoHabitacionId = habitacion.TipoHabitacionId;
            l.HotelId = habitacion.HotelId;
            l.Nombre = habitacion.Nombre;
        }
    }
}
