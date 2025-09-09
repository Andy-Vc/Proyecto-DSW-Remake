using Microsoft.AspNetCore.Mvc;
using ProyectoDSWToolify.Services.Contratos;
using ProyectoDSWToolify.Models.ViewModels;
using System.Text.Json;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Models.ViewModels.ClienteVM;
using Microsoft.AspNetCore.Authorization;

namespace ProyectoDSWToolify.Controllers
{
    [Authorize(Roles = "C")]
    public class ClienteController : Controller
    {
        private readonly IClienteService _clienteService;
        private readonly IVentaService _ventaService;
        private readonly ICategoriaService _categoriaService;
        private readonly IMensajeService mensajeService;

        public ClienteController(IClienteService clienteService, IVentaService ventaService, ICategoriaService categoriaService, IMensajeService mensajeService)
        {
            _clienteService = clienteService;
            _ventaService = ventaService;
            _categoriaService = categoriaService;
            this.mensajeService = mensajeService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("C"))
            {
                return RedirectToAction("Error", "AccesoDenegado");
            }
            var resumen = await _clienteService.ObtenerResumenAsync();

            var vm = new IndexViewModel
            {
                TotalProductos = resumen.TotalProductos,
                TotalClientes = resumen.TotalClientes,
                CategoriasMasVendidas = resumen.CategoriasMasVendidas
            };

            return View(vm);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Producto(List<int> categorias = null, string orden = "asc", int pagina = 1)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("C"))
            {
                return RedirectToAction("Error", "AccesoDenegado");
            }
            categorias = categorias?.Distinct().ToList() ?? new List<int>();

            if (pagina < 1) pagina = 1;
            var todasCategorias = await _categoriaService.ListaCategoria();

            var productos = await _clienteService.ObtenerProductosAsync(categorias, orden, pagina);

            int tamañoPagina = 12;
            int totalProductos = await _clienteService.ContarProductosAsync(categorias);
            int totalPaginas = (int)System.Math.Ceiling((double)totalProductos / tamañoPagina);

            var modelo = new ProductosViewModel
            {
                Categorias = todasCategorias,
                Productos = productos,
                IdCategoriasSeleccionadas = categorias ?? new List<int>(),
                OrdenPrecio = orden,
                PaginaActual = pagina,
                TotalPaginas = totalPaginas,
                TamañoPagina = tamañoPagina
            };

            return View(modelo);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ObtenerProductoPorId(int id)
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("C"))
            {
                return RedirectToAction("Error", "AccesoDenegado");
            }
            var producto = await _clienteService.ObtenerProductoPorIdAsync(id);
            if (producto == null) return NotFound();

            string imagenUrl;

            if (string.IsNullOrEmpty(producto.imagen))
            {
                imagenUrl = Url.Content("~/assets/productos/P" + producto.id + ".jpg");
            }
            else
            {
                imagenUrl = producto.imagen;
            }

            return Json(new
            {
                id = producto.id,
                nombre = producto.nombre,
                descripcion = producto.descripcion,
                categoria = producto.categoria,
                precio = producto.precio.ToString("F2"),
                stock = producto.stock,
                imagen = imagenUrl
            });
        }
        [AllowAnonymous]
        public async Task<IActionResult> Nosotros()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("C"))
            {
                return RedirectToAction("Error", "AccesoDenegado");
            }
            var resumen = await _clienteService.ObtenerResumenAsync();

            var vm = new IndexViewModel
            {
                TotalProductos = resumen.TotalProductos,
                TotalClientes = resumen.TotalClientes,
                CategoriasMasVendidas = resumen.CategoriasMasVendidas
            };

            return View(vm);
        }
        [AllowAnonymous]
        public IActionResult Contacto()
        {
            if (User.Identity.IsAuthenticated && !User.IsInRole("C"))
            {
                return RedirectToAction("Error", "AccesoDenegado");
            }
            var model = new ContactoMensaje();
            var usuarioJson = HttpContext.Session.GetString("usuario");
            if (!string.IsNullOrEmpty(usuarioJson))
            {
                var usuario = JsonSerializer.Deserialize<Usuario>(usuarioJson);

                model.nombre = usuario.nombre;
                model.email = usuario.correo;
                model.telefono = usuario.telefono;
                model.idUser = usuario.idUsuario;
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ContactoPost()
        {
            var nombre = HttpContext.Request.Form["nombre"];
            var email = HttpContext.Request.Form["email"];
            var telefono = HttpContext.Request.Form["telefono"];
            var mensajeTexto = HttpContext.Request.Form["mensaje"];
            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(mensajeTexto))
            {
                TempData["ErrorMessage"] = "Faltan campos obligatorios.";
                return RedirectToAction("Contacto");
            }
            var usuarioJson = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                TempData["ErrorMessage"] = "Debes iniciar sesión para enviar un mensaje.";
                return RedirectToAction("Login", "UserAuth");
            }
            var usuario = JsonSerializer.Deserialize<Usuario>(usuarioJson);
            var mensaje = new ContactoMensaje
            {
                nombre = nombre,
                email = email,
                telefono = telefono,
                mensaje = mensajeTexto,
                idUser = usuario.idUsuario,
                fechaEnvio = DateTime.UtcNow
            };

            try
            {
                await mensajeService.InsertarMensajeAsync(mensaje);
                TempData["GoodMessage"] = "Mensaje enviado correctamente.";
            }
            catch (HttpRequestException ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"Error al insertar mensaje: {ex.Message}");
                TempData["ErrorMessage"] = "Ocurrió un error al enviar el mensaje. Por favor, inténtelo de nuevo.";
            }

            return RedirectToAction("Contacto");
        }
        [HttpGet]
        public async Task<IActionResult> Perfil()
        {
            var usuarioJson = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                TempData["ErrorMessage"] = "Necesitas iniciar sesión para acceder a tu perfil.";
                return RedirectToAction("Login", "UserAuth");
            }

            var usuario = JsonSerializer.Deserialize<Usuario>(usuarioJson);

            int id = usuario.idUsuario;

            var ventas = await _clienteService.ObtenerVentasClienteAsync(id);

            var viewModel = new PerfilClienteViewModel
            {
                idCliente = id,
                Nombre = $"{usuario.nombre} {usuario.apePaterno} {usuario.apeMaterno}",
                Email = usuario.correo,
                UserName = usuario.nombre,
                HistorialVentas = ventas
            };

            return View(viewModel);
        }
        public async Task<IActionResult> DescargarVentaPdf(int idCliente, int idVenta)
        {
            try
            {
                var pdfBytes = await _ventaService.DescargarVentaPdf(idCliente, idVenta);
                return File(pdfBytes, "application/pdf", $"venta_{idVenta}.pdf");
            }
            catch (Exception ex)
            {
                // Maneja error (por ejemplo, mostrar mensaje)
                return NotFound(ex.Message);
            }
        }

    }
}

