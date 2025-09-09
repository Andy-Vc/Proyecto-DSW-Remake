namespace ProyectoDSWToolify.Models.ViewModels.VendedorVM
{
    public class VendedorPerfilViewModel
    {
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string InicialNombre { get; set; }

        public long VentasMensuales { get; set; }
        public long ProductosMensuales { get; set; }
        public long ClientesMensuales { get; set; }
        public double IngresosMensuales { get; set; }

        public string FechaCompleta { get; set; }
        public string FechaCorta { get; set; }
    }
}
