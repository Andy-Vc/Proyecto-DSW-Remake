using ProyectoDSWToolify.Models;

namespace ProyectoDSWToolify.Services.Contratos
{
    public interface IDistritoService
    {
        Task<List<Distrito>> obtenerListaDistritos();
    }
}
