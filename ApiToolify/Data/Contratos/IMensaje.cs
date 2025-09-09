using ApiToolify.Models.DTO;

namespace ApiToolify.Data.Contratos
{
    public interface IMensaje
    {
        Task<List<ContactoMensaje>> ListarMensajesAsync();
        Task<ContactoMensaje> ObtenerPorIdAsync(string id);
        Task InsertarMensajeAsync(ContactoMensaje mensaje);
        Task EliminarMensajeAsync(string id);
    }
}
