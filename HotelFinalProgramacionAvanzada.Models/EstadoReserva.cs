using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HotelFinalProgramacionAvanzada.Models
{
    public class EstadoReserva
    {
        const string Listar = "usp_ListarEstadoReserva";
        const string Buscar = "usp_BuscarEstadoReserva";
        const string Crear = "usp_CrearEstadoReserva";
        const string Actualizar = "usp_ActualizarEstadoReserva";
        const string Borrar = "usp_BorrarEstadoReserva";

        [Key]
        public int EstadoReservaId { get; set; }

        [DisplayName("Estado de reserva")]
        [Required(ErrorMessage = "Este campo es requerido.")]
        [MaxLength(150, ErrorMessage = "El máximo es de 150 caracteres")]
        public string NombreEstado { get; set; }

    }
}
