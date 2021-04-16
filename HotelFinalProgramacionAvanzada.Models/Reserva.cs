using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelFinalProgramacionAvanzada.Models
{
    public class Reserva
    {
        [Key]
        public int ReservaId { get; set; }
        //////////////////////////////////////////////

        [Required(ErrorMessage = "Debe especificar una habitación")]
        [Display(Name = "Habitacion")]
        public int HabitacionId { get; set; }

        [ForeignKey("HabitacionId")]
        public Habitacion Habitacion { get; set; }
        //////////////////////////////////////////////

        [Required(ErrorMessage = "Debe especificar un estado")]
        [Display(Name = "Estado de la reserva")]
        public int EstadoReservaId { get; set; }

        [ForeignKey("EstadoReservaId")]
        public EstadoReserva EstadoReserva { get; set; }
        //////////////////////////////////////////////

        [Required(ErrorMessage = "Debe especificar un cliente")]
        [Display(Name = "Cliente")]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public Usuario Usuario { get; set; }
        //////////////////////////////////////////////

        [Required]
        [DisplayName("Total a pagar")]
        [Range(0, 1000000)]
        public decimal CostoTotal { get; set; }
        //////////////////////////////////////////////

        [Required]
        [DisplayName("Saldo")]
        [Range(0, 1000000)]
        public decimal Saldo { get; set; }

        //////////////////////////////////////////////

        [Required]
        [DisplayName("Fecha de llegada")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FechaLlegada { get; set; }
        //////////////////////////////////////////////

        [Required]
        [DisplayName("Fecha de salida")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime FechaSalida { get; set; }
    }
}
