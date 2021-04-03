using HotelFinalProgramacionAvanzada.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio
{
    public interface ITipoHabitacionRepositorio : IRepositorio<TipoHabitacion>
    {
        void Actualizar(TipoHabitacion tipoHabitacion);
    }
}
