using Newtonsoft.Json;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Services.Contratos;
using System.Net.Http.Headers;
using System.Text;

namespace ProyectoDSWToolify.Services.Implementacion
{
    public class ProductoService : IProductoService
    {
        private readonly HttpClient _httpClient;

        public ProductoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Producto>> ListaCompleta()
        {
            var listaProducto = new List<Producto>();

            var response = await _httpClient.GetAsync("producto");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                listaProducto = JsonConvert.DeserializeObject<List<Producto>>(data);
            }

            return listaProducto;
        }

        public async Task<Producto> ObtenerIdProducto(int id)
        {
            var producto = new Producto();

            var response = await _httpClient.GetAsync($"producto/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                producto = JsonConvert.DeserializeObject<Producto>(data);
            }

            return producto;
        }

        public async Task<Producto> RegistrarProducto(Producto producto)
        {
            Producto prdRegistrado = null;

            try
            {
                using (var form = new MultipartFormDataContent())
                {
                    form.Add(new StringContent(producto.nombre), "nombre");
                    form.Add(new StringContent(producto.descripcion), "descripcion");
                    form.Add(new StringContent(producto.proveedor.idProveedor.ToString()), "proveedor.idProveedor");
                    form.Add(new StringContent(producto.categoria.idCategoria.ToString()), "categoria.idCategoria");
                    form.Add(new StringContent(producto.precio.ToString()), "precio");
                    form.Add(new StringContent(producto.stock.ToString()), "stock");

                    if (producto.file != null)
                    {
                        var stream = producto.file.OpenReadStream();
                        var fileContent = new StreamContent(stream);
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(producto.file.ContentType);
                        form.Add(fileContent, "file", producto.file.FileName);
                    }

                    var response = await _httpClient.PostAsync("producto", form);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        prdRegistrado = JsonConvert.DeserializeObject<Producto>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en RegistrarProducto: " + ex.Message);
            }

            return prdRegistrado;
        }

        public async Task<Producto> ActualizarProducto(Producto producto)
        {
            Producto prdActualizado = null;

            try
            {
                using (var formData = new MultipartFormDataContent())
                {
                    formData.Add(new StringContent(producto.idProducto.ToString()), "idProducto");
                    formData.Add(new StringContent(producto.nombre), "nombre");
                    formData.Add(new StringContent(producto.descripcion), "descripcion");
                    formData.Add(new StringContent(producto.proveedor.idProveedor.ToString()), "proveedor.idProveedor");
                    formData.Add(new StringContent(producto.categoria.idCategoria.ToString()), "categoria.idCategoria");
                    formData.Add(new StringContent(producto.precio.ToString()), "precio");
                    formData.Add(new StringContent(producto.stock.ToString()), "stock");

                    if (producto.file != null && producto.file.Length > 0)
                    {
                        var fileContent = new StreamContent(producto.file.OpenReadStream());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(producto.file.ContentType);
                        formData.Add(fileContent, "file", producto.file.FileName);
                    }
                    else if (!string.IsNullOrEmpty(producto.imagen))
                    {
                        formData.Add(new StringContent(producto.imagen), "imagen");
                    }

                    var response = await _httpClient.PutAsync($"producto/{producto.idProducto}", formData);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        prdActualizado = JsonConvert.DeserializeObject<Producto>(data);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error en ActualizarProducto: " + ex.Message);
            }

            return prdActualizado;
        }

        public async Task<int> DesactivarProducto(int id)
        {
            var response = await _httpClient.DeleteAsync($"producto/{id}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(data);
            }

            return 0; 
        }

        public async Task<int> ActivarProducto(int id)
        {
            var response = await _httpClient.PostAsync($"producto/activar/{id}", null);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(data);
            }
            return 0;
        }
    }
}
