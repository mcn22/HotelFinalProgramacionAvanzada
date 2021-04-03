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
    public class ReservaRepositorio : Repositorio<Reserva>, IReservaRepositorio
    {
        public ReservaRepositorio(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        readonly ApplicationDbContext _db;

        public void Actualizar(Reserva reserva)
        {
            var l = _db.Reservas.FirstOrDefault(s => s.ReservaId == reserva.ReservaId);

            if (l == null)
                return;

            l.HabitacionId = reserva.HabitacionId;
            l.EstadoReservaId = reserva.EstadoReservaId;
            l.UserId = reserva.UserId;
            l.CostoTotal = reserva.CostoTotal;
            l.Saldo = reserva.Saldo;
            l.FechaLlegada = reserva.FechaLlegada;
            l.FechaSalida = reserva.FechaSalida;
        }
    }
}
