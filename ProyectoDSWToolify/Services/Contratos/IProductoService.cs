using ProyectoDSWToolify.Models;

namespace ProyectoDSWToolify.Services.Contratos
{
    public interface IProductoService
    {
        Task<List<Producto>> ListaCompleta();
        Task<Producto> ObtenerIdProducto(int id);
        Task<Producto> RegistrarProducto(Producto producto);
        Task<Producto> ActualizarProducto(Producto producto);
        Task<int> DesactivarProducto(int id);
        Task<int> ActivarProducto(int id);
    }
}
