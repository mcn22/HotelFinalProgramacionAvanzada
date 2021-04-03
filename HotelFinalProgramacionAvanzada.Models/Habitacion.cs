﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelFinalProgramacionAvanzada.Models
{
    public class Habitacion
    {
        [Key]
        public int HabitacionId { get; set; }

        [Required(ErrorMessage = "Debe especificar un tipo de habitación")]
        [Display(Name = "Tipo de habitación")]
        public int TipoHabitacionId { get; set; }

        [ForeignKey("TipoHabitacionId")]
        public TipoHabitacion TipoHabitacion { get; set; }

        [Required(ErrorMessage = "Debe especificar un hotel")]
        [Display(Name = "Tipo de habitación")]
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }

        [DisplayName("Nombre de la habitación")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [MaxLength(10, ErrorMessage = "El máximo es de 10 caracteres")]
        public string Nombre { get; set; }

    }
}
