using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Controllers
{
    [Authorize(Roles = "A")]
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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 10)
        {
            var listado = await proveedorService.obtenerListadoProveedor();

            var totalItems = listado.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1) page = 1;
            if (page > totalPages) page = totalPages;

            var itemsPaginados = listado
                                 .Skip((page - 1) * pageSize)
                                 .Take(pageSize)
                                 .ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;

            return View(itemsPaginados);
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
            try
            {
                var proveedorGuardado = await proveedorService.RegistrarProveedor(proveedor);

                TempData["GoodMessage"] = $"Se registró a {proveedorGuardado.razonSocial} con código: {proveedorGuardado.idProveedor}";
                return RedirectToAction("Index");
            }
            catch (ApplicationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                var distritos = await distritoService.obtenerListaDistritos();
                ViewBag.Distritos = new SelectList(distritos, "idDistrito", "nombre");
                return View(proveedor);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error inesperado al registrar el proveedor.";
                var distritos = await distritoService.obtenerListaDistritos();
                ViewBag.Distritos = new SelectList(distritos, "idDistrito", "nombre");
                return View(proveedor);
            }
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
        public async Task<IActionResult> ToggleEstado(int id)
        {
            var proveedor = await proveedorService.ObtenerIdProveedor(id);
            if (proveedor == null)
                return NotFound();

            int resultado = 0;

            if ((bool)proveedor.estado)
            {
                resultado = await proveedorService.desactivarProveedor(id);
                if (resultado > 0)
                    TempData["GoodMessage"] = "Proveedor desactivado correctamente.";
                else
                    TempData["ErrorMessage"] = "No se pudo desactivar el proveedor.";
            }
            else
            {
                resultado = await proveedorService.Activar(id);
                if (resultado > 0)
                    TempData["GoodMessage"] = "Proveedor activado correctamente.";
                else
                    TempData["ErrorMessage"] = "No se pudo activar el proveedor.";
            }

            if (resultado > 0)
                return RedirectToAction("Index");

            TempData["ErrorMessage"] = "No se pudo cambiar el estado";
            return RedirectToAction("Index");
        }

    }
}
