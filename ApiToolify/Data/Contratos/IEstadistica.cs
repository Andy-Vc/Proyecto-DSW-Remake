namespace ApiToolify.Data.Contratos
{
    public interface IEstadistica
    {
        long ContarVentasPorMes(int id,string fechaMes);
        long ContarProductosVendidosPorMes(int id,string fechaMes);
        long ObtenerTotalVentas(int id);
        long ObtenerTotalProductosVendidos(int id);
        double ObtenerIngresosTotales(int id);

    }
}
