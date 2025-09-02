using System.ComponentModel.DataAnnotations;

namespace ProyectoDSWToolify.Models
{
    public class DetalleVenta
    {
         public int idDetalleVenta { get; set; }
        public Venta venta { get; set; }
        public Producto producto { get; set; }
        public int cantidad { get; set; }
        public decimal subTotal { get; set; }
    }
}
