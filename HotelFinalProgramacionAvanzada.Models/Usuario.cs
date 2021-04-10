using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HotelFinalProgramacionAvanzada.Models
{
    public class Usuario : IdentityUser
    {

        [Required(ErrorMessage = "Este campo es requerido.")]
        [MaxLength(56, ErrorMessage = "El máximo es de 56 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es requerido.")]
        [MaxLength(56, ErrorMessage = "El máximo es de 56 caracteres")]
        public string Apellido{ get; set; }

        [NotMapped]
        public string Role { get; set; }

        public int? HotelId { get; set; }

        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }
    }
}
