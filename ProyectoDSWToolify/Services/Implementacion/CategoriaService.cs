using Newtonsoft.Json;
using System.Net.Http;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Services.Implementacion
{
    public class CategoriaService : ICategoriaService
    {
        private readonly HttpClient _httpClient;

        public CategoriaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Categoria>> ListaCategoria()
        {
            var response = await _httpClient.GetAsync("Categoria");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Categoria>>(json);
        }
    }
}
