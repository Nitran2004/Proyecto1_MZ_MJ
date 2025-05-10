namespace Proyecto1_MZ_MJ.Models
{ 
    public class Pedido
    {
        public int Id { get; set; }
        public string? UsuarioId { get; set; } // Puede ser null si no se ha registrado
        public ICollection<PedidoProducto> PedidoProductos { get; set; }

        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }

        public List<PedidoDetalle> Detalles { get; set; }

        public string Estado { get; set; } = "Preparándose"; // ← NUEVO: valor inicial por defecto


    }


}
