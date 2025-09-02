using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoDSWToolify.Models;
using ProyectoDSWToolify.Services.Contratos;
using System.Diagnostics;

namespace ProyectoDSWToolify.Controllers
{
    //[Authorize(Roles = "A")]
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
        public async Task<IActionResult> Index(int pag = 1, int proveedor = 0, int categoria = 0)
        {
            var listado = await productoService.ListaCompleta();
            var nombreProveedor = "";
            var nombreCategoria = "";

            ViewBag.Proveedores = new SelectList(await proveedorService.obtenerListadoProveedor(), "idProveedor", "razonSocial", proveedor);
            ViewBag.Categorias = new SelectList(await categoriaService.ListaCategoria(), "idCategoria", "descripcion", categoria);

            if (proveedor > 0)
            {
                listado = listado.Where(x => x.proveedor.idProveedor == proveedor).ToList();
                nombreProveedor = listado.FirstOrDefault()?.proveedor.razonSocial;
            }

            if (categoria > 0)
            {
                listado = listado.Where(x => x.categoria.idCategoria == categoria).ToList();
                nombreCategoria = listado.FirstOrDefault()?.categoria.descripcion;
            }

            if (proveedor > 0 || categoria > 0)
            {
                var mensaje = "Se filtró por ";

                if (proveedor != 0)
                    mensaje += nombreProveedor;

                if (categoria != 0)
                {
                    if (proveedor != 0)
                        mensaje += ", ";
                    mensaje += nombreCategoria;
                }

                if (listado.Count > 0)
                {
                    TempData["GoodMessage"] = mensaje;
                }
                else
                {
                    listado = await productoService.ListaCompleta();
                    TempData["ErrorMessage"] = "No hay productos para esos filtros";
                }
            }
            var paginasTotales = listado.Count;
            var paginasMax = 9;
            var numeroPag = (int)Math.Ceiling((double)paginasTotales / paginasMax);
            ViewBag.pagActual = pag;
            ViewBag.numeroPag = numeroPag;
            var skip = (pag - 1) * paginasMax;

            return View(listado.Skip(skip).Take(paginasMax));
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

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var prdEncontrado = await productoService.ObtenerIdProducto(id);
            return View(prdEncontrado);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> Delete_Confirm(int id)
        {
            var prdEnc = await productoService.ObtenerIdProducto(id);
            await productoService.DesactivarProducto(id);

            TempData["GoodMessage"] = $"Se desactivó el producto {prdEnc.descripcion} código: {prdEnc.idProducto}";
            return RedirectToAction("Index");
        }

    }
}
