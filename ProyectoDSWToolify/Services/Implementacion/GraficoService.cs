using System.Net.Http.Json;
using ProyectoDSWToolify.Models.ViewModels.AdminVM;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Services.Implementacion
{
    public class GraficoService : IGraficoService
    {
        private readonly HttpClient _httpClient;

        public GraficoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoriaXProductoViewModel>> CategoriaProducto()
        {
            var resultado = await _httpClient.GetFromJsonAsync<List<CategoriaXProductoViewModel>>("Grafico/CategoriaProducto");
            return resultado ?? new List<CategoriaXProductoViewModel>();
        }

        public async Task<List<ProveedorXProductoViewModel>> ProveedorProducto()
        {
            var resultado = await _httpClient.GetFromJsonAsync<List<ProveedorXProductoViewModel>>("Grafico/ProveedorProducto");
            return resultado ?? new List<ProveedorXProductoViewModel>();
        }

        public async Task<List<VentaXDistritoViewModel>> VentaPorDistrito()
        {
            var resultado = await _httpClient.GetFromJsonAsync<List<VentaXDistritoViewModel>>("Grafico/VentaPorDistrito");
            return resultado ?? new List<VentaXDistritoViewModel>();
        }

        public async Task<List<VentaXMesViewModel>> VentaPorMes()
        {
            var resultado = await _httpClient.GetFromJsonAsync<List<VentaXMesViewModel>>("Grafico/VentaPorMes");
            return resultado ?? new List<VentaXMesViewModel>();
        }

        public async Task<List<VentaXTipoVentaViewModel>> VentaPorMesAndTipoVenta()
        {
            var resultado = await _httpClient.GetFromJsonAsync<List<VentaXTipoVentaViewModel>>("Grafico/VentaPorMesAndTipoVenta");
            return resultado ?? new List<VentaXTipoVentaViewModel>();
        }
    }
}
