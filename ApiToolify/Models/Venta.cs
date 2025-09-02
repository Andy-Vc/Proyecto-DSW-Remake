using System.ComponentModel.DataAnnotations;

namespace ProyectoDSWToolify.Models
{
    public class Venta
    {
         public int idVenta { get; set; }
        public Usuario usuario { get; set; }
        public DateTime fecha { get; set; }
        public decimal  total { get; set; }
        public string tipoVenta { get; set; }
        public string estado { get; set; }
        public List<DetalleVenta> Detalles { get; set; } = new();
    }
}
