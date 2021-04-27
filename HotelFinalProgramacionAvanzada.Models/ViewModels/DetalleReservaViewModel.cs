using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HotelFinalProgramacionAvanzada.Models.ViewModels
{
    public class DetalleReservaViewModel
    {
        public DetalleReservaViewModel()
        {
        }
        public Reserva Reserva { get; set; }
        public string Estado { get; set; }
    }
}
