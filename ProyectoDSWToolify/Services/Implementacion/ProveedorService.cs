using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Services.Contratos;
using Newtonsoft.Json;
using System.Text;

namespace ProyectoDSWToolify.Services.Implementacion
{
    public class ProveedorService : IProveedorService
    {
        private readonly HttpClient _httpClient;

        public ProveedorService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Proveedor>> obtenerListadoProveedor()
        {
            var listaProveedor = new List<Proveedor>();
            var response = await _httpClient.GetAsync("proveedor");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                listaProveedor = JsonConvert.DeserializeObject<List<Proveedor>>(data);
            }

            return listaProveedor;
        }

        public async Task<Proveedor> ObtenerIdProveedor(int id)
        {
            var proveedor = new Proveedor();
            var response = await _httpClient.GetAsync($"proveedor/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                proveedor = JsonConvert.DeserializeObject<Proveedor>(data);
            }

            return proveedor;
        }

        public async Task<Proveedor> RegistrarProveedor(Proveedor proveedor)
        {
            Proveedor proveedorGuardado = null;

            var contenidoJson = new StringContent(
                JsonConvert.SerializeObject(proveedor),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync("proveedor", contenidoJson);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                proveedorGuardado = JsonConvert.DeserializeObject<Proveedor>(data);
            }

            return proveedorGuardado;
        }

        public async Task<Proveedor> actualizarProveedor(Proveedor proveedor)
        {
            Proveedor proveedorGuardado = null;

            var contenidoJson = new StringContent(
                JsonConvert.SerializeObject(proveedor),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PutAsync($"proveedor/{proveedor.idProveedor}", contenidoJson);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                proveedorGuardado = JsonConvert.DeserializeObject<Proveedor>(data);
            }

            return proveedorGuardado;
        }

        public async Task<int> desactivarProveedor(int id)
        {
            var response = await _httpClient.DeleteAsync($"proveedor/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(data);
            }

            return 0; 
        }

        public async Task<int> Activar(int id)
        {
            var response = await _httpClient.PostAsync($"proveedor/activar/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(data);
            }
            return 0;
        }

    }
}
