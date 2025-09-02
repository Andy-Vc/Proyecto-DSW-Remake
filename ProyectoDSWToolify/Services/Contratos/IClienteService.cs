using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Models.ViewModels;
using ProyectoDSWToolify.Models.ViewModels.ClienteVM;

namespace ProyectoDSWToolify.Services.Contratos
{
    public interface IClienteService
    {
        Task<List<Producto>> ObtenerProductosAsync(List<int> categorias, string orden, int pagina);
        Task<ProductoDetallesViewModel> ObtenerProductoPorIdAsync(int id);
        Task<List<Venta>> ObtenerVentasClienteAsync(int idCliente);
        Task<IndexViewModel> ObtenerResumenAsync();
        Task<int> ContarProductosAsync(List<int> categorias);

    }
}
