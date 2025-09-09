using System.Security.Claims;
using Azure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Models.ViewModels;
using ProyectoDSWToolify.Models.ViewModels.AdminVM;
using ProyectoDSWToolify.Services.Contratos;
using ProyectoDSWToolify.Services.Implementacion;

namespace ProyectoDSWToolify.Controllers
{
    //[Authorize(Roles = "A")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly ICategoriaService categoriaService;
        private readonly IGraficoService graficoService;
        private readonly IProveedorService proveedorService;
        private readonly IProductoService productoService;
        private readonly IMensajeService mensajeService;
        public AdminController(IAdminService adminService, ICategoriaService categoriaService, IGraficoService graficoService, IProveedorService proveedorService, IProductoService productoService, IMensajeService mensajeService)
        {
            this.adminService = adminService;
            this.categoriaService = categoriaService;
            this.graficoService = graficoService;
            this.proveedorService = proveedorService;
            this.productoService = productoService;
            this.mensajeService = mensajeService;
        }

        public async Task<IActionResult> Dashboard()
        {
            DateTime fechaActual = DateTime.Now;
            var nombre = User.FindFirst(ClaimTypes.Name)?.Value;
            var apellido = User.FindFirst("Apellido")?.Value;
            var diaSemanaNombre = fechaActual.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));

            List<string> mensajes = new List<string>()
        {
            $"Bienvenido {nombre} ¿Qué deseas hacer este {diaSemanaNombre}?",
            $"Hola {nombre} {apellido}, ¿Cómo te encuentras este {diaSemanaNombre}?",
            $"Sr. {apellido}, bienvenido a Toolify.",
            $"Feliz ¡{diaSemanaNombre}! {nombre}"
        };
            string mensajeAleatorio = obtenerMensajeAleatorio(mensajes);

            var categorias = await graficoService.CategoriaProducto();
            var proveedores = await graficoService.ProveedorProducto();
            var ventasDistrito = await graficoService.VentaPorDistrito();
            var ventasMes = await graficoService.VentaPorMes();
            var ventasTipo = await graficoService.VentaPorMesAndTipoVenta();
            var cantidadCategorias = await categoriaService.ListaCategoria();
            var cantidadVentas = await adminService.ListadoVentaFechaAndTipoVenta(null, null, null);
            var cantidadProductos = await productoService.ListaCompleta();
            var cantidadProveedor = await proveedorService.obtenerListadoProveedor();

            var model = new DashboardViewModel
            {
                MensajeBienvenida = mensajeAleatorio,
                CategoriaXProducto = categorias,
                ProveedorXProducto = proveedores,
                VentaXDistrito = ventasDistrito,
                VentaXMes = ventasMes,
                VentaXTipoVenta = ventasTipo,
                TotalCategorias = cantidadCategorias.Count(),
                TotalProductos = cantidadProductos.Count(),
                TotalProveedores = cantidadProveedor.Count(),
                TotalVentas = cantidadVentas.Count()
            };

            return View(model);
        }
        public string obtenerMensajeAleatorio(List<string> mensaje)
        {
            if (mensaje == null || mensaje.Count == 0)
            {
                return "No hay mensajes disponibles.";
            }

            Random rdm = new Random();
            int indice = rdm.Next(0, mensaje.Count);

            return mensaje[indice];
        }
        public async Task<IActionResult> Categoria()
        {
            var listado = await categoriaService.ListaCategoria();
            return View(listado);
        }
        public async Task<IActionResult> ReporteVenta(DateTime? fechaInicio, DateTime? fechaFin, string? tipo, int pag = 1)
        {
            var listado = await adminService.ListadoVentaFechaAndTipoVenta(fechaInicio, fechaFin, tipo);

            if (!listado.Any())
            {
                listado = await adminService.ListadoVentaFechaAndTipoVenta(null, null, null);
            }

            if (Request.Query.ContainsKey("export") && Request.Query["export"] == "excel")
            {
                var fileContent = await adminService.DescargarVentasExcel(fechaInicio, fechaFin, tipo);
                return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"ReporteVentas_{DateTime.Now:yyyyMMdd}.xlsx");
            }

            if (Request.Query.ContainsKey("export") && Request.Query["export"] == "pdf")
            {
                var fileContent = await adminService.DescargarVentasPdf(fechaInicio, fechaFin, tipo);
                return File(fileContent, "application/pdf", $"ReporteVentas_{DateTime.Now:yyyyMMdd}.pdf");
            }

            int paginasMax = 10;
            int paginasTotales = listado.Count;
            int numeroPag = (int)Math.Ceiling((double)paginasTotales / paginasMax);
            ViewBag.pagActual = pag;
            ViewBag.numeroPag = numeroPag;
            var skip = (pag - 1) * paginasMax;

            return View(listado.Skip(skip).Take(paginasMax));
        }
        public async Task<IActionResult> ReporteProducto(int? idCategoria, string? orden, int pag = 1)
        {
            if (idCategoria == 0)
                idCategoria = null;

            var categorias = await categoriaService.ListaCategoria();
            ViewBag.Categorias = new SelectList(categorias, "idCategoria", "descripcion", idCategoria);

            var productos = await adminService.ListarProductosPorCategoria(idCategoria, orden);

            if (!productos.Any())
            {
                productos = await adminService.ListarProductosPorCategoria(null, null);
            }

            if (Request.Query.ContainsKey("export") && Request.Query["export"] == "excel")
            {
                var fileContent = await adminService.DescargarProductosExcel(idCategoria, orden);
                return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"ReporteProductos_{DateTime.Now:yyyyMMdd}.xlsx");
            }

            if (Request.Query.ContainsKey("export") && Request.Query["export"] == "pdf")
            {
                var fileContent = await adminService.DescargarProductosPdf(idCategoria, orden);
                return File(fileContent, "application/pdf", $"ReporteProductos_{DateTime.Now:yyyyMMdd}.pdf");
            }

            int paginasMax = 10;
            int paginasTotales = productos.Count;
            int numeroPag = (int)Math.Ceiling((double)paginasTotales / paginasMax);
            ViewBag.pagActual = pag;
            ViewBag.numeroPag = numeroPag;
            var skip = (pag - 1) * paginasMax;

            return View(productos.Skip(skip).Take(paginasMax));
        }
        public async Task<IActionResult> MensajeContacto()
        {
            var list = await mensajeService.ListarMensajesAsync();
            return View(list);
        }
        [HttpPost]
        public async Task<IActionResult> EliminarMensaje(string id)
        {
            await mensajeService.EliminarMensajeAsync(id);
            TempData["GoodMessage"] = "Mensaje eliminado correctamente.";
            return RedirectToAction("MensajeContacto");
        }

    }
}
