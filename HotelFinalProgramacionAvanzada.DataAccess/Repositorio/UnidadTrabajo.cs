using HotelFinalProgramacionAvanzada.DataAccess.Data;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinalProgramacionAvanzada.DataAccess.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        public UnidadTrabajo(ApplicationDbContext db)
        {
            _db = db;
            ProcedimientoAlmacenado = new ProcedimientoAlmacenado(_db);
            EstadosReserva = new EstadoReservaRepositorio(db);
            Habitaciones = new HabitacionRepositorio(db);
            Hoteles = new HotelRepositorio(db);
            HotelEmpleados = new HotelEmpleadoRepositorio(db);
            Reservas = new ReservaRepositorio(db);
            TiposHabitacion = new TipoHabitacionRepositorio(db);
            Usuarios = new UsuarioRepositorio(db);
        }

        readonly ApplicationDbContext _db;

        public IEstadoReservaRepositorio EstadosReserva { get; private set; }
        public IHabitacionRepositorio Habitaciones { get; private set; }
        public IHotelRepositorio Hoteles { get; private set; }
        public IHotelEmpleadoRepositorio HotelEmpleados { get; private set; }
        public IReservaRepositorio Reservas { get; private set; }
        public ITipoHabitacionRepositorio TiposHabitacion { get; private set; }
        public IUsuarioRepositorio Usuarios { get; private set; }
        public IProcedimientoAlmacenado ProcedimientoAlmacenado { get; private set; }

        public void Dispose()
        {
            _db.Dispose();
        }

        public void Guardar()
        {
            _db.SaveChanges();
        }
    }
}

