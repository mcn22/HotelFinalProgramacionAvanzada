using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelFinalProgramacionAvanzada.Models
{
    public class HotelEmpleado
    {
        [Key]
        public int HotelEmpleadoId { get; set; }

        [Required(ErrorMessage = "Debe especificar un empleado")]
        [Display(Name = "Empleado")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public Usuario Usuario { get; set; }

        [Required(ErrorMessage = "Debe especificar un hotel")]
        [Display(Name = "Hotel")]
        public int HotelId { get; set; }

        [ForeignKey("HotelId")]
        public Hotel Hotel { get; set; }

    }
}
