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
    public class TipoHabitacionRepositorio : Repositorio<TipoHabitacion>, ITipoHabitacionRepositorio
    {
        public TipoHabitacionRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        readonly ApplicationDbContext _db;
         
        public void Actualizar(TipoHabitacion tipoHabitacion)
        {
            var c = _db.TiposHabitacion.FirstOrDefault(s => s.TipoHabitacionId == tipoHabitacion.TipoHabitacionId);

            if (c == null)
                return;

            c.Nombre = tipoHabitacion.Nombre;
            c.Descripcion = tipoHabitacion.Descripcion;
            c.CostoNoche = tipoHabitacion.CostoNoche;
            c.ImagenTipo = tipoHabitacion.ImagenTipo;
        }
    }
}
