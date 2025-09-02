using System.ComponentModel.DataAnnotations;

namespace ProyectoDSWToolify.Models
{
    public class Proveedor
    {
        public int idProveedor { get; set; }
        public string? ruc { get; set; }
        public string? razonSocial { get; set; }
        public string? telefono { get; set; }
        public string? direccion { get; set; }
        public Distrito? distrito { get; set; }
        public DateTime? fechaRegistro { get; set; }
        public bool? estado { get; set; }
    }
}
