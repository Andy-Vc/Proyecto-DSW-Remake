namespace ApiToolify.Data.Contratos
{
    public interface IEstadistica
    {
        long ContarVentasPorMes(string fechaMes);
        long ContarProductosVendidosPorMes(string fechaMes);
        long ContarClientesAtendidosPorMes(string fechaMes);
        long ObtenerTotalVentas();
        long ObtenerTotalProductosVendidos();
        double ObtenerIngresosTotales();

    }
}
