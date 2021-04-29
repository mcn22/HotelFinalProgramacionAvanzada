using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelFinalProgramacionAvanzada.Models.ViewModels
{
    public class ReservaViewModel
    {
        public ReservaViewModel()
        {
            TiposHabitacionList = new List<TipoHabitacion>();
            Hotel = new Hotel();
            Reserva = new Reserva();
            Disponible = false;
            DiasHospedaje = 0;
            TiposHabitacionDD = new List<SelectListItem>();
            TipoHabitacionId = 0;
        }
        public Reserva Reserva { get; set; }
        public bool Disponible { get; set; }
        public int DiasHospedaje { get; set;}

        [DisplayName("Tipo de habitacion")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public int TipoHabitacionId { get; set; }
        public List<TipoHabitacion> TiposHabitacionList { get; set; } 
        public Hotel Hotel { get; set; }
        public List<SelectListItem> TiposHabitacionDD { get; set; }
    }
}
