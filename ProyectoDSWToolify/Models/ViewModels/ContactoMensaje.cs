namespace ProyectoDSWToolify.Models.ViewModels
{
    public class ContactoMensaje
    {
        public string? id { get; set; }
        public int idUser { get; set; }
        public string nombre { get; set; }
        public string email { get; set; }
        public string telefono { get; set; }
        public string mensaje { get; set; }
        public DateTime fechaEnvio { get; set; } = DateTime.UtcNow;
    }
}
