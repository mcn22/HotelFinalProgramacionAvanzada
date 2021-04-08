using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace HotelFinalProgramacionAvanzada.Models.ViewModels
{
    public class HabitacionViewModel
    {
        public HabitacionViewModel()
        {
            TiposHabitacion = new List<SelectListItem>();
            Hoteles = new List<SelectListItem>();
        }
        public Habitacion Habitacion { get; set; }
        public List<SelectListItem> TiposHabitacion { get; set; } 
        public List<SelectListItem> Hoteles { get; set; }
    }
}
