using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Proyecto1_MZ_MJ.Models
{
    public class Pago
    {
        public int PagoId { get; set; }

        [Required(ErrorMessage = "El campo Método de Pago es obligatorio.")]
        public string? MetodoPago { get; set; }

        [Required(ErrorMessage = "El campo Fecha del Pago es obligatorio.")]
        public DateTime FechaPago { get; set; }

        [Required(ErrorMessage = "El campo Monto Pagado es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El campo Monto Pagado debe ser un valor positivo.")]
        public double MontoPagado { get; set; }

        [Required(ErrorMessage = "El campo Pagado es obligatorio.")]
        public bool Pagado { get; set; }

        // Relación con la habitación (y por ende, los nombres de los huéspedes)
        public int HabitacionId { get; set; }
        [ForeignKey("HabitacionId")]
        public Habitacion? Habitacion { get; set; }
    }
}
