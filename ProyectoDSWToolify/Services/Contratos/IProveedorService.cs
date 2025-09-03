using ProyectoDSWToolify.Models;

namespace ProyectoDSWToolify.Services.Contratos
{
    public interface IProveedorService
    {
        Task<List<Proveedor>> obtenerListadoProveedor();
        Task<Proveedor> ObtenerIdProveedor(int id);
        Task<Proveedor> RegistrarProveedor(Proveedor proveedor);
        Task<Proveedor> actualizarProveedor(Proveedor proveedor);
        Task<int> desactivarProveedor(int id);
        Task<int> Activar(int id);
    }
}
