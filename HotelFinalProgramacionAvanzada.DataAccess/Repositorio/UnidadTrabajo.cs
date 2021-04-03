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

            //EstadosReserva = new CategoriaRepositorio(db);
            //Habitaciones = new CubiertaRepositorio(db);
            //HotelEmpleados = new LibroRepositorio(db);
            //Reservas = new UsuarioRepositorio(db);
            //TiposHabitacion = new CompaniaRepositorio(db);
            //Usuarios = new CompaniaRepositorio(db);
        }

        readonly ApplicationDbContext _db;

        public IEstadoReservaRepositorio EstadosReserva { get; private set; }
        public IHabitacionRepositorio Habitaciones { get; private set; }
        public IHotelEmpleadoRepositorio HotelEmpleados { get; private set; }
        public IReservaRepositorio Reservas { get; private set; }
        public ITipoHabitacionRepositorio TiposHabitacion { get; private set; }
        public IUsuarioRepositorio Usuarios { get; private set; }

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

