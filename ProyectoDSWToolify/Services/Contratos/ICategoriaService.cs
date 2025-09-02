using ProyectoDSWToolify.Models;

namespace ProyectoDSWToolify.Services.Contratos
{
    public interface ICategoriaService
    {
        Task<List<Categoria>> ListaCategoria();
    }
}
