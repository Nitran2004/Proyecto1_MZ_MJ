using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace Proyecto1_MZ_MJ.Models
{
    public class Habitacion
    {
        public int HabitacionId { get; set; }

        [Required(ErrorMessage = "El campo NumHabitacion es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El campo NumHabitacion debe ser un valor positivo.")]
        public double NumHabitacion { get; set; }

        [Required(ErrorMessage = "El campo Capacidad es obligatorio.")]
        public int Capacidad { get; set; }

        [Required(ErrorMessage = "El campo PrecioPorNoche es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El campo PrecioPorNoche debe ser un valor positivo.")]
        public double PrecioPorNoche { get; set; }

        [Required(ErrorMessage = "El campo Descripcion es obligatorio.")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo Disponible es obligatorio.")]
        public bool Disponible { get; set; }

        [Required(ErrorMessage = "El campo Tipo es obligatorio.")]
        public string? Tipo { get; set; }

        [Required(ErrorMessage = "El campo Vistas es obligatorio.")]
        public string? Vistas { get; set; }

        public List<Comentario>? Comentarios { get; set; } // Agrega el using para el espacio de nombres donde se define Comentario

        

        

        // Otros atributos relacionados con la habitación, como comodidades, tamaño, etc.

        // Constructor y métodos adicionales si es necesario
    }
}

