using Proyecto1_MZ_MJ.Models;
namespace Proyecto1_MZ_MJ.Models.DTOs

{
    public class PedidoConUbicacionDTO
    {
        public List<int> ProductosIdsSeleccionados { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }
    }
}
