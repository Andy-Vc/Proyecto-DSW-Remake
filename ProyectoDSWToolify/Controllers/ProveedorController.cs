using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Controllers
{
    //[Authorize(Roles = "A")] // SOLO ADMINISTRADOR
    public class ProveedorController : Controller
    {
        private readonly IDistritoService distritoService;
        private readonly IProveedorService proveedorService;

        public ProveedorController(IDistritoService distritoService, IProveedorService proveedorService)
        {
            this.distritoService = distritoService;
            this.proveedorService = proveedorService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var listado = await proveedorService.obtenerListadoProveedor();
            return View(listado);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var distritos = await distritoService.obtenerListaDistritos();
            ViewBag.Distritos = new SelectList(distritos, "idDistrito", "nombre");
            return View(new Proveedor());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Proveedor proveedor)
        {
            var proveedorGuardado = await proveedorService.RegistrarProveedor(proveedor);
            TempData["GoodMessage"] = $"Se registró a {proveedorGuardado.razonSocial} con código: {proveedorGuardado.idProveedor}";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Actualizar(int id = 0)
        {
            var distritos = await distritoService.obtenerListaDistritos();
            ViewBag.Distritos = new SelectList(distritos, "idDistrito", "nombre");

            var proveedor = await proveedorService.ObtenerIdProveedor(id);
            return View(proveedor);
        }

        [HttpPost]
        public async Task<IActionResult> Actualizar(Proveedor proveedor)
        {
            proveedor.distrito = new Distrito { idDistrito = proveedor.distrito.idDistrito };

            var proveedorActualizado = await proveedorService.actualizarProveedor(proveedor);

            TempData["GoodMessage"] = $"Se actualizó a {proveedorActualizado.razonSocial} con código: {proveedorActualizado.idProveedor}";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var proveedor = await proveedorService.ObtenerIdProveedor(id);
            return View(proveedor);
        }

        [HttpGet]
        public async Task<IActionResult> Desactivar(int id)
        {
            var proveedor = await proveedorService.ObtenerIdProveedor(id);
            return View(proveedor);
        }

        [HttpPost]
        [ActionName("Desactivar")]
        public async Task<IActionResult> Desactivar_Confirmar(int id)
        {
            var proveedor = await proveedorService.ObtenerIdProveedor(id);
            await proveedorService.desactivarProveedor(id);

            TempData["GoodMessage"] = $"Se desactivó a {proveedor.razonSocial} con código: {proveedor.idProveedor}";
            return RedirectToAction("Index");
        }
    }
}
