using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HotelFinalProgramacionAvanzada.Models.ViewModels
{
    public class CambioEstadoReservaViewModel
    {
        public CambioEstadoReservaViewModel()
        {
            TiposEstadoDD = new List<SelectListItem>();
        }
        public Reserva Reserva { get; set; }
        public List<SelectListItem> TiposEstadoDD { get; set; }
    }
}
