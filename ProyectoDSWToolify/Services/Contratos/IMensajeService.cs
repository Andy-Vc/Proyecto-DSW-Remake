using ProyectoDSWToolify.Models.ViewModels;

namespace ProyectoDSWToolify.Services.Contratos
{
    public interface IMensajeService
    {
        Task<List<ContactoMensaje>> ListarMensajesAsync();
        Task<ContactoMensaje> ObtenerPorIdAsync(string id);
        Task InsertarMensajeAsync(ContactoMensaje mensaje);
        Task EliminarMensajeAsync(string id);
    }
}
