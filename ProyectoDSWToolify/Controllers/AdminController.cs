using System.Security.Claims;
using Azure;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Models.ViewModels;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Controllers
{
    //[Authorize(Roles = "A")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly ICategoriaService categoriaService;

        public AdminController(IAdminService adminService, ICategoriaService categoriaService)
        {
            this.adminService = adminService;
            this.categoriaService = categoriaService;
        }

        public IActionResult Index()
        {
            DateTime fechaActual = DateTime.Now;
            var nombre = User.FindFirst(ClaimTypes.Name)?.Value;
            var apellido = User.FindFirst("Apellido")?.Value;
            var rol = User.FindFirst(ClaimTypes.Role)?.Value;
            var diaSemanaNombre = fechaActual.ToString("dddd", new System.Globalization.CultureInfo("es-ES"));


            List<String> mensaje = new List<string>()
                {
                    $"Bienvenido {nombre} ¿Que deseas hacer {diaSemanaNombre}?",
                    $"Hola {nombre} {apellido} ¿Como te encuentras este {diaSemanaNombre}?",
                    $"Sr. {apellido}, bienvenido a Toolify.",
                    $"Feliz ¡{diaSemanaNombre}! {nombre} "
                };
            string mensajeAleatorio = obtenerMensajeAleatorio(mensaje);

            TempData["Mensaje"] = mensajeAleatorio;
            return View();
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

        [HttpGet]
        public async Task<IActionResult> ReporteVenta(DateTime? fechaInicio, DateTime? fechaFin, string? tipo, int pag = 1)
        {
            // Obtener listado de ventas filtradas desde el servicio (que llama la API)
            var listado = await adminService.ListadoVentaFechaAndTipoVenta(fechaInicio, fechaFin, tipo);

            if (!listado.Any())
            {
                listado = await adminService.ListadoVentaFechaAndTipoVenta(null, null, null);
            }

            // Exportar a Excel (descarga directamente el archivo)
            if (Request.Query.ContainsKey("export") && Request.Query["export"] == "excel")
            {
                var fileContent = await adminService.DescargarVentasExcel(fechaInicio, fechaFin, tipo);
                return File(fileContent, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    $"ReporteVentas_{DateTime.Now:yyyyMMdd}.xlsx");
            }

            // Exportar a PDF (descarga directamente el archivo)
            if (Request.Query.ContainsKey("export") && Request.Query["export"] == "pdf")
            {
                var fileContent = await adminService.DescargarVentasPdf(fechaInicio, fechaFin, tipo);
                return File(fileContent, "application/pdf", $"ReporteVentas_{DateTime.Now:yyyyMMdd}.pdf");
            }

            // Paginación local para la vista
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


    }
}
