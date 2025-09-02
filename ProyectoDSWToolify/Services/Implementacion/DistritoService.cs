using Newtonsoft.Json;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Services.Implementacion
{
    public class DistritoService : IDistritoService
    {
        private readonly HttpClient _httpClient;

        public DistritoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Distrito>> obtenerListaDistritos()
        {
            var response = await _httpClient.GetAsync("Distrito");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Distrito>>(json);
        }
    }
}
