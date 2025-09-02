using ProyectoDSWToolify.Models.ViewModels.AdminVM;

namespace ProyectoDSWToolify.Services.Contratos
{
    public interface IGraficoService
    {
        Task<List<CategoriaXProductoViewModel>> CategoriaProducto();
        Task<List<ProveedorXProductoViewModel>> ProveedorProducto();
        Task<List<VentaXMesViewModel>> VentaPorMes();
        Task<List<VentaXTipoVentaViewModel>> VentaPorMesAndTipoVenta();
        Task<List<VentaXDistritoViewModel>> VentaPorDistrito();
    }
}
