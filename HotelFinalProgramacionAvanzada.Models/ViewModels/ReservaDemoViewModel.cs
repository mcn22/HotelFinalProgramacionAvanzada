using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace HotelFinalProgramacionAvanzada.Models.ViewModels
{
    public class ReservaDemoViewModel
    {
        public Reserva Reserva { get; set; }
        public string Llegada { get; set; }
        public string Salida { get; set;}
    }
}
