using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProyectoDSWToolify.Data.Contratos;
using ProyectoDSWToolify.Models;

namespace ApiToolify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoria categoriarepo;

        public CategoriaController(ICategoria categoriarepo)
        {
            this.categoriarepo = categoriarepo;
        }

        [HttpGet]
        public IActionResult ListaCategoria() {
            return Ok(categoriarepo.listCategoriasCliente());
        }
    }
}
