using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Services.Contratos;
using Newtonsoft.Json;
using ProyectoDSWToolify.Models.ViewModels.AdminVM;

namespace ProyectoDSWToolify.Services.Implementacion
{
    public class AdminService : IAdminService
    {
        private readonly HttpClient _httpClient;

        public AdminService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ListadoVentaFechaAndTipoVentaDTO>> ListadoVentaFechaAndTipoVenta(DateTime? fechaInicio, DateTime? fechaFin, string? tipo = null)
        {
            var queryParams = new List<string>();

            if (fechaInicio.HasValue)
                queryParams.Add($"fechaInicio={fechaInicio.Value:yyyy-MM-dd}");

            if (fechaFin.HasValue)
                queryParams.Add($"fechaFin={fechaFin.Value:yyyy-MM-dd}");

            if (!string.IsNullOrEmpty(tipo))
                queryParams.Add($"tipo={tipo}");

            var queryString = string.Join("&", queryParams);
            var url = "Reporte/ListarPorMesAndTipoVenta";
            if (queryParams.Count > 0)
                url += "?" + queryString;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ListadoVentaFechaAndTipoVentaDTO>>(json);
        }


        public async Task<List<ListarProductosPorCategoriaDTO>> ListarProductosPorCategoria(int? idCategoria, string? orden)
        {
            var queryParams = new List<string>();

            if (idCategoria.HasValue)
                queryParams.Add($"idCategoria={idCategoria.Value}");

            if (!string.IsNullOrEmpty(orden))
                queryParams.Add($"orden={orden}");

            var queryString = string.Join("&", queryParams);
            var url = "Reporte/ListarProductosPorCategoria";

            if (queryParams.Count > 0)
                url += "?" + queryString;

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ListarProductosPorCategoriaDTO>>(json);
        }


        public async Task<byte[]> DescargarVentasExcel(DateTime? fechaInicio, DateTime? fechaFin, string? tipo)
        {
            var url = $"Reporte/ListadoPorMesAndTipoVentaExcel?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}";
            if (!string.IsNullOrEmpty(tipo))
                url += $"&tipo={tipo}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> DescargarVentasPdf(DateTime? fechaInicio, DateTime? fechaFin, string? tipo)
        {
            var url = $"Reporte/ListadoPorMesAndTipoVentaPdf?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}";
            if (!string.IsNullOrEmpty(tipo))
                url += $"&tipo={tipo}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> DescargarProductosExcel(int? idCategoria, string? orden)
        {
            var url = $"Reporte/ListarProductosPorCategoriaExcel";
            var hasQuery = false;

            if (idCategoria != null)
            {
                url += $"?idCategoria={idCategoria}";
                hasQuery = true;
            }

            if (!string.IsNullOrEmpty(orden))
            {
                url += hasQuery ? $"&orden={orden}" : $"?orden={orden}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }

        public async Task<byte[]> DescargarProductosPdf(int? idCategoria, string? orden)
        {
            var url = $"Reporte/ListarProductosPorCategoriaPdf";
            var hasQuery = false;

            if (idCategoria != null)
            {
                url += $"?idCategoria={idCategoria}";
                hasQuery = true;
            }

            if (!string.IsNullOrEmpty(orden))
            {
                url += hasQuery ? $"&orden={orden}" : $"?orden={orden}";
            }

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsByteArrayAsync();
        }
    }
}
