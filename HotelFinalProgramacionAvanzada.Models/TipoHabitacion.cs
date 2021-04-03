using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelFinalProgramacionAvanzada.Models
{
    public class TipoHabitacion
    {
        [Key]
        public int TipoHabitacionId { get; set; }

        [DisplayName("Nombre del tipo de habitacion")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [MaxLength(128, ErrorMessage = "El máximo es de 128 caracteres")]
        public string Nombre { get; set; }

        [DisplayName("Descripción")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        public string Descripcion { get; set; }

        [Required]
        [DisplayName("Costo por noche")]
        [Range(0, 1000000)]
        public decimal CostoNoche { get; set; }

        [Required]
        [StringLength(128)]
        public string ImagenTipo { get; set; }
    }
}
