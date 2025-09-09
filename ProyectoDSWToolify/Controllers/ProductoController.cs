using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Services.Contratos;

namespace ProyectoDSWToolify.Controllers
{
    [Authorize(Roles = "A")]
    public class ProductoController : Controller
    {
        private readonly IProveedorService proveedorService;
        private readonly ICategoriaService categoriaService;
        private readonly IProductoService productoService;

        public ProductoController(IProveedorService proveedorService, ICategoriaService categoriaService, IProductoService productoService)
        {
            this.proveedorService = proveedorService;
            this.categoriaService = categoriaService;
            this.productoService = productoService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int pag = 1, int proveedor = 0, int categoria = 0, string searchQuery = "")
        {
            ViewBag.Proveedores = new SelectList(await proveedorService.obtenerListadoProveedor(), "idProveedor", "razonSocial", proveedor);
            ViewBag.Categorias = new SelectList(await categoriaService.ListaCategoria(), "idCategoria", "descripcion", categoria);

            var listado = await productoService.ListaCompleta();
            if (proveedor > 0)
            {
                listado = listado.Where(x => x.proveedor.idProveedor == proveedor).ToList();
            }

            if (categoria > 0)
            {
                listado = listado.Where(x => x.categoria.idCategoria == categoria).ToList();
            }

            if (!string.IsNullOrEmpty(searchQuery))
            {
                listado = listado.Where(x => x.nombre.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)).ToList();
            }


            if (listado.Count == 0 && (proveedor > 0 || categoria > 0 || !string.IsNullOrEmpty(searchQuery)))
            {
                TempData["ErrorMessage"] = "No hay productos para esos filtros.";
            }

            var totalRegistros = listado.Count;
            var registrosPorPagina = 9;
            var totalPaginas = (int)Math.Ceiling((double)totalRegistros / registrosPorPagina);

            ViewBag.TotalPages = totalPaginas;
            ViewBag.Page = pag;
            ViewBag.ProveedorId = proveedor;
            ViewBag.CategoriaId = categoria;
            ViewBag.CurrentSearchQuery = searchQuery;

            var productosPaginados = listado.Skip((pag - 1) * registrosPorPagina).Take(registrosPorPagina);

            return View(productosPaginados);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categorias = new SelectList(await categoriaService.ListaCategoria(), "idCategoria", "descripcion");
            ViewBag.Proveedores = new SelectList(await proveedorService.obtenerListadoProveedor(), "idProveedor", "razonSocial");
            return View(new Producto());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Producto producto)
        {
            var productoRegistrado = await productoService.RegistrarProducto(producto);
            TempData["GoodMessage"] = $"Se registró el producto {productoRegistrado.descripcion} código: {productoRegistrado.idProducto}";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var prdEncontrado = await productoService.ObtenerIdProducto(id);
            ViewBag.Categorias = new SelectList(await categoriaService.ListaCategoria(), "idCategoria", "descripcion", prdEncontrado.categoria.idCategoria);
            ViewBag.Proveedores = new SelectList(await proveedorService.obtenerListadoProveedor(), "idProveedor", "razonSocial", prdEncontrado.proveedor.idProveedor);
            return View(prdEncontrado);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Producto producto)
        {
            var prdActualizado = await productoService.ActualizarProducto(producto);
            TempData["GoodMessage"] = $"Se actualizó el producto {prdActualizado.descripcion} código: {prdActualizado.idProducto}";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var prdEncontrado = await productoService.ObtenerIdProducto(id);
            return View(prdEncontrado);
        }

        public async Task<IActionResult> ToggleEstado(int id)
        {
            var producto = await productoService.ObtenerIdProducto(id);
            if (producto == null)
                return NotFound();

            int resultado = 0;

            if ((bool)producto.estado)
            {
                resultado = await productoService.DesactivarProducto(id);
                if (resultado > 0)
                    TempData["GoodMessage"] = "Producto desactivado correctamente.";
                else
                    TempData["ErrorMessage"] = "No se pudo desactivar el producto.";
            }
            else
            {
                resultado = await productoService.ActivarProducto(id);
                if (resultado > 0)
                    TempData["GoodMessage"] = "Producto activado correctamente.";
                else
                    TempData["ErrorMessage"] = "No se pudo activar el producto.";
            }

            if (resultado > 0)
                return RedirectToAction("Index");

            TempData["ErrorMessage"] = "No se pudo cambiar el estado";
            return RedirectToAction("Index");
        }
    }
}
