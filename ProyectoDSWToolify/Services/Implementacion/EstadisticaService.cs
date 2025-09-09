using Newtonsoft.Json;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Services.Implementacion
{
    public class EstadisticaService : IEstadisticaService
    {
        private readonly HttpClient _httpClient;
        public EstadisticaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<long> ContarVentasPorMesAsync(int id, string fechaMes)
        {
            var response = await _httpClient.GetAsync($"reporte/mensual/ventas/{fechaMes}?id={id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<long>(json);
        }

        public async Task<long> ContarProductosVendidosPorMesAsync(int id,string fechaMes)
        {
            var response = await _httpClient.GetAsync($"reporte/mensual/productos/{fechaMes}?id={id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<long>(json);
        }

        public async Task<long> ObtenerTotalVentasAsync(int id)
        {
            var response = await _httpClient.GetAsync($"reporte/total/ventas?id={id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<long>(json);
        }

        public async Task<long> ObtenerTotalProductosVendidosAsync(int id)
        {
            var response = await _httpClient.GetAsync($"reporte/total/productos?id={id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<long>(json);
        }

        public async Task<double> ObtenerIngresosTotalesAsync(int id)
        {
            var response = await _httpClient.GetAsync($"reporte/total/ingresos?id={id}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<double>(json);
        }
    }
}
