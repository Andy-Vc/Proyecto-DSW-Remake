using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProyectoDSWToolify.Models
{
    public class Producto
    {
        public int idProducto { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public Proveedor proveedor { get; set; }
        public Categoria categoria { get; set; }
        public decimal precio { get; set; }
        public int stock { get; set; }
        public string? imagen { get; set; }
        public DateTime fechaRegistro { get; set; }
        public bool estado { get; set; }

        [NotMapped]
        public IFormFile? file { get; set; }


    }
}
