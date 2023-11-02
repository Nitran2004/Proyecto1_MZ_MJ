namespace Proyecto1_MZ_MJ.Models
{
    public class Comentario
    {
        public int ComentarioId { get; set; }
        public string? Texto { get; set; }
        // Otros atributos relacionados con los comentarios
        //public string HabitacionTipo
        //{
          //  get { return Habitacion.Tipo; }
        //}
        public int HabitacionId { get; set; }
        public Habitacion? Habitacion { get; set; }

    }
}
