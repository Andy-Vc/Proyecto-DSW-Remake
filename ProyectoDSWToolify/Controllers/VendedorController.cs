using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Models.ViewModels.ClienteVM;
using ProyectoDSWToolify.Models.ViewModels.VendedorVM;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Controllers
{
    [Authorize(Roles = "V")]
    public class VendedorController : Controller
    {
        private readonly IVendedorService _vendedorService;
        private readonly IVentaService _ventaService;
        private readonly IEstadisticaService _reporteService;

        public VendedorController(IVendedorService vendedorService, IVentaService ventaService, IEstadisticaService reporteService)
        {
            _vendedorService = vendedorService;
            _ventaService = ventaService;
            _reporteService = reporteService;
        }

        #region
        public async Task<IActionResult> DescargarVentaPdf(int idUsuario, int idVenta)
        {
            try
            {
                var pdfBytes = await _ventaService.DescargarVentaPdf(idUsuario, idVenta);
                return File(pdfBytes, "application/pdf", $"venta_{idVenta}.pdf");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DatosTotales()
        {
            var usuarioJson = HttpContext.Session.GetString("usuario");

            var usuario = System.Text.Json.JsonSerializer.Deserialize<Usuario>(usuarioJson);
            int idUsuario = usuario.idUsuario;

            var metricas = new
            {
                totalProductosVendidos = await _reporteService.ObtenerTotalProductosVendidosAsync(idUsuario),
                totalVentas = await _reporteService.ObtenerTotalVentasAsync(idUsuario),
                ingresosTotales = await _reporteService.ObtenerIngresosTotalesAsync(idUsuario)
            };

            return Json(metricas);
        }

        public async Task<IActionResult> DetalleVenta(int idVenta)
        {
            try
            {
                var venta = await _vendedorService.ObtenerVentaPorId(idVenta);
                return Json(venta);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        public async Task<IActionResult> ListarProductos()
        {
            try
            {
                var productos = await _vendedorService.ListProductosVendedorAsync();
                return Json(productos);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        [HttpPost]
        public async Task<IActionResult> GenerarVentaVendedor([FromBody] VentaViewModel v)
        {
            try
            {
                var venta = await _ventaService.GenerarVentaVendedor(v);
                return Json(venta);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
        #endregion
        public async Task<IActionResult> Index()
        {
            var usuarioJson = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                TempData["ErrorMessage"] = "Necesitas iniciar sesión para acceder a tu perfil.";
                return RedirectToAction("Login", "UserAuth");
            }

            var usuario = System.Text.Json.JsonSerializer.Deserialize<Usuario>(usuarioJson);

            var fechaActual = DateTime.Now.ToString("yyyy-MM");

            var viewModel = new VendedorPerfilViewModel
            {
                Nombre = usuario.nombre,
                Correo = usuario.correo,
                InicialNombre = usuario.nombre.Substring(0, 1).ToUpper(),
                VentasMensuales = await _reporteService.ContarVentasPorMesAsync(usuario.idUsuario,fechaActual),
                ProductosMensuales = await _reporteService.ContarProductosVendidosPorMesAsync(usuario.idUsuario,fechaActual),
                IngresosMensuales = await _reporteService.ObtenerIngresosTotalesAsync(usuario.idUsuario),

                FechaCompleta = DateTime.Now.ToString("dddd, dd MMMM yyyy", new System.Globalization.CultureInfo("es-ES")),
                FechaCorta = DateTime.Now.ToString("dd/MM/yyyy")
            };

            return View(viewModel);
        }

        public IActionResult Ventas()
        {
            var usuarioJson = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                TempData["ErrorMessage"] = "Necesitas iniciar sesión para acceder a tu perfil.";
                return RedirectToAction("Login", "UserAuth");
            }

            var usuario = System.Text.Json.JsonSerializer.Deserialize<Usuario>(usuarioJson);

            int id = usuario.idUsuario;

            ViewBag.IdUsuario = id;
            return View();
        }

        [HttpGet]
        public async Task <IActionResult> Historial()
        {
            var usuarioJson = HttpContext.Session.GetString("usuario");
            if (string.IsNullOrEmpty(usuarioJson))
            {
                TempData["ErrorMessage"] = "Necesitas iniciar sesión para acceder a tu perfil.";
                return RedirectToAction("Login", "UserAuth");
            }

            var usuario = System.Text.Json.JsonSerializer.Deserialize<Usuario>(usuarioJson);

            int id = usuario.idUsuario;

            var ventas = await _vendedorService.ObtenerLstVentasAsync(id);

            var listado = new ListadoHistorialViewModel
            {
                idVendedor = id,
                Nombre = $"{usuario.nombre} {usuario.apePaterno} {usuario.apeMaterno}",
                UserName = usuario.nombre,
                HistorialVentas = ventas
            };

            return View(listado);
        }
    }
}
