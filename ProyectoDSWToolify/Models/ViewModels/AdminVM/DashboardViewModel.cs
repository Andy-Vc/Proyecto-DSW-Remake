namespace ProyectoDSWToolify.Models.ViewModels.AdminVM
{
    public class DashboardViewModel
    {
        public string MensajeBienvenida { get; set; }
        public List<CategoriaXProductoViewModel> CategoriaXProducto { get; set; }
        public List<ProveedorXProductoViewModel> ProveedorXProducto { get; set; }
        public List<VentaXDistritoViewModel> VentaXDistrito { get; set; }
        public List<VentaXMesViewModel> VentaXMes { get; set; }
        public List<VentaXTipoVentaViewModel> VentaXTipoVenta { get; set; }
        public int TotalProductos { get; set; }
        public int TotalCategorias { get; set; }
        public int TotalProveedores { get; set; }
        public int TotalVentas { get; set; }

    }

}
