using ApiToolify.Data.Contratos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiToolify.Controllers
{
    [Route("api/reporte")]
    [ApiController]
    public class EstadisticaVendedorController : ControllerBase
    {
        private readonly IEstadistica reporteData;

        public EstadisticaVendedorController(IEstadistica repo)
        {
            reporteData = repo;
        }

        [HttpGet]
        [Route("mensual/ventas/{fechaMes}")]
        public IActionResult contarVentasMesActual(int id, string fechaMes)
        {
            return Ok(reporteData.ContarVentasPorMes(id,fechaMes));
        }

        [HttpGet]
        [Route("mensual/productos/{fechaMes}")]
        public IActionResult contarProductosVendidosMesActual(int id, string fechaMes)
        {
            return Ok(reporteData.ContarProductosVendidosPorMes(id,fechaMes));
        }

        [HttpGet]
        [Route("total/ventas")]
        public IActionResult obtenerTotalVentas(int id)
        {
            return Ok(reporteData.ObtenerTotalVentas(id));
        }

        [HttpGet]
        [Route("total/productos")]
        public IActionResult obtenerTotalProductosVendidos(int id)
        {
            return Ok(reporteData.ObtenerTotalProductosVendidos(id));
        }

        [HttpGet]
        [Route("total/ingresos")]
        public IActionResult obtenerIngresosTotales(int id)
        {
            return Ok(reporteData.ObtenerIngresosTotales(id));
        }
    }
}
