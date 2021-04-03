using HotelFinalProgramacionAvanzada.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelFinalProgramacionAvanzada.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EstadoReserva> EstadosReserva { get; set; }

        public DbSet<Habitacion> Habitaciones { get; set; }

        public DbSet<Hotel> Hoteles { get; set; }

        public DbSet<TipoHabitacion> TiposHabitacion { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        public DbSet<Reserva> Reservas { get; set; }

        public DbSet<HotelEmpleado> HotelEmpleados { get; set; }
    }
}
