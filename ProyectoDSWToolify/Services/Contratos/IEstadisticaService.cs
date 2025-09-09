namespace ProyectoDSWToolify.Services.Contratos
{
    public interface IEstadisticaService
    {
        Task<long> ContarVentasPorMesAsync(int id,string fechaMes);
        Task<long> ContarProductosVendidosPorMesAsync(int id,string fechaMes);
        Task<long> ObtenerTotalVentasAsync(int id);
        Task<long> ObtenerTotalProductosVendidosAsync(int id);
        Task<double> ObtenerIngresosTotalesAsync(int id);
    }
}
