using ApiToolify.Data.Contratos;
using ApiToolify.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiToolify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajeController : ControllerBase
    {
        private readonly IMensaje _repository;

        public MensajeController(IMensaje repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> EnviarMensaje([FromBody] ContactoMensaje mensaje)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _repository.InsertarMensajeAsync(mensaje);
            return Ok(new { message = "Mensaje recibido correctamente" });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMensajes()
        {
            var mensajes = await _repository.ListarMensajesAsync();
            return Ok(mensajes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObtenerMensajePorId(string id)
        {
            var mensaje = await _repository.ObtenerPorIdAsync(id);
            if (mensaje == null)
                return NotFound();

            return Ok(mensaje);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarMensaje(string id)
        {
            await _repository.EliminarMensajeAsync(id);
            return Ok(new { message = "Mensaje eliminado correctamente" });
        }

    }
}
