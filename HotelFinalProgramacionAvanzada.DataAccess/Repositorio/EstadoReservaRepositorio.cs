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
    public class EstadoReservaRepositorio : Repositorio<EstadoReserva>, IEstadoReservaRepositorio
    {
        public EstadoReservaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        readonly ApplicationDbContext _db;
         
        public void Actualizar(EstadoReserva estadoReserva)
        {
            var c = _db.EstadosReserva.FirstOrDefault(s => s.EstadoReservaId == estadoReserva.EstadoReservaId);

            if (c == null)
                return;

            c.NombreEstado = estadoReserva.NombreEstado;
        }
    }
}
