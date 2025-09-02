using ProyectoDSWToolify.Models.ViewModels;

namespace ProyectoDSWToolify.Services.Contratos
{
    public interface IAdminService
    {
        Task<List<ListadoVentaFechaAndTipoVentaDTO>> ListadoVentaFechaAndTipoVenta(
            DateTime? fechaInicio, DateTime? fechaFin, string? tipo = null);
        Task<List<ListarProductosPorCategoriaDTO>> ListarProductosPorCategoria(
          int? idCategoria, string? orden);
        Task<byte[]> DescargarVentasExcel(DateTime? fechaInicio, DateTime? fechaFin, string? tipo);
        Task<byte[]> DescargarVentasPdf(DateTime? fechaInicio, DateTime? fechaFin, string? tipo);
        Task<byte[]> DescargarProductosExcel(int? idCategoria, string? orden);
        Task<byte[]> DescargarProductosPdf(int? idCategoria, string? orden);

    }
}
