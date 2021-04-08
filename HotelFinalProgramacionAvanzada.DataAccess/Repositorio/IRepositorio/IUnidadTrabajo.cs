using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {

        IEstadoReservaRepositorio EstadosReserva { get;}
        IHabitacionRepositorio Habitaciones { get;}
        IHotelRepositorio Hoteles { get;}
        IHotelEmpleadoRepositorio HotelEmpleados { get;}
        IReservaRepositorio Reservas { get;}
        ITipoHabitacionRepositorio TiposHabitacion { get;}
        IUsuarioRepositorio Usuarios { get;}

        void Guardar();
    }
}
