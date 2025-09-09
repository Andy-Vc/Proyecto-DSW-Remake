using System.Text.Json;
using ProyectoDSWToolify.Models.ViewModels;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Services.Implementacion
{
    public class MensajeService : IMensajeService
    {
        private readonly HttpClient _httpClient;

        public MensajeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task EliminarMensajeAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"Mensaje/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task InsertarMensajeAsync(ContactoMensaje mensaje)
        {
            var json = JsonSerializer.Serialize(mensaje);
            Console.WriteLine("Mensaje enviado al API: " + json); 

            var response = await _httpClient.PostAsJsonAsync("Mensaje", mensaje);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException($"Error {response.StatusCode}: {error}");
            }
        }


        public async Task<List<ContactoMensaje>> ListarMensajesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ContactoMensaje>>("Mensaje");
        }

        public async Task<ContactoMensaje> ObtenerPorIdAsync(string id)
        {
            return await _httpClient.GetFromJsonAsync<ContactoMensaje>($"Mensaje/{id}");
        }
    }
}
