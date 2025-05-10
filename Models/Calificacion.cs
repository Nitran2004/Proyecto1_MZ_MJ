// En Models/Calificacion.cs
using System;
using System.ComponentModel.DataAnnotations;

namespace Proyecto1_MZ_MJ.Models
{
    public class Calificacion
    {
        public int Id { get; set; }

        [Required]
        public int PedidoId { get; set; }

        public Pedido Pedido { get; set; }

        [Required]
        [Range(1, 5)]
        [Display(Name = "Calificación")]
        public int Valor { get; set; }

        [Display(Name = "Comentario (opcional)")]
        public string Comentario { get; set; }

        public DateTime Fecha { get; set; }
    }
}