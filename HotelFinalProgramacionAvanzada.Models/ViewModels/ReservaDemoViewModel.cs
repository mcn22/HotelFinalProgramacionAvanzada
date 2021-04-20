using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace HotelFinalProgramacionAvanzada.Models.ViewModels
{
    public class ReservaDemoViewModel
    {
        public ReservaDemoViewModel()
        {
            Habitaciones = new List<SelectListItem>();
            EstadosReserva = new List<SelectListItem>();
            Usuarios = new List<SelectListItem>();
            Disponible = false;
            diasHospedaje = 0;
        }
        public Reserva Reserva { get; set; }
        public bool Disponible { get; set; }
        public int diasHospedaje { get; set;}
        public List<SelectListItem> Habitaciones { get; set; } 
        public List<SelectListItem> EstadosReserva { get; set; }
        public List<SelectListItem> Usuarios { get; set; }
    }
}
