using HotelFinalProgramacionAvanzada.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio
{
    public interface IHabitacionRepositorio : IRepositorio<Habitacion>
    {
        void Actualizar(Habitacion habitacion);
    }
}
