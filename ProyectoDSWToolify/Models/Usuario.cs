using System.ComponentModel.DataAnnotations;

namespace ProyectoDSWToolify.Models
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string nombre { get; set; }
        public string apeMaterno { get; set; }
        public string apePaterno { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public string nroDoc { get; set; }
        public string direccion { get; set; }
        public Distrito distrito { get; set; }
        public string telefono { get; set; }
        public Rol? rol { get; set; }
        public DateTime fechaRegistro { get; set; }


    }
}
