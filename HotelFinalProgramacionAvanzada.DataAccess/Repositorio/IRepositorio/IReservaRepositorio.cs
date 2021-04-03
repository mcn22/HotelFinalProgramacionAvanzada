using HotelFinalProgramacionAvanzada.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio
{
    public interface IReservaRepositorio : IRepositorio<Reserva>
    {
        void Actualizar(Reserva reserva);
    }
}
