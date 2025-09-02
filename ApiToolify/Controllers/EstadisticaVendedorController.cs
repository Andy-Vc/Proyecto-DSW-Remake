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
        public IActionResult contarVentasMesActual(string fechaMes)
        {
            return Ok(reporteData.ContarVentasPorMes(fechaMes));
        }

        [HttpGet]
        [Route("mensual/productos/{fechaMes}")]
        public IActionResult contarProductosVendidosMesActual(string fechaMes)
        {
            return Ok(reporteData.ContarProductosVendidosPorMes(fechaMes));
        }

        [HttpGet]
        [Route("mensual/clientes/{fechaMes}")]
        public IActionResult contarClientesAtendidosMesActual(string fechaMes)
        {
            return Ok(reporteData.ContarClientesAtendidosPorMes(fechaMes));
        }

        [HttpGet]
        [Route("total/ventas")]
        public IActionResult obtenerTotalVentas()
        {
            return Ok(reporteData.ObtenerTotalVentas());
        }

        [HttpGet]
        [Route("total/productos")]
        public IActionResult obtenerTotalProductosVendidos()
        {
            return Ok(reporteData.ObtenerTotalProductosVendidos());
        }

        [HttpGet]
        [Route("total/ingresos")]
        public IActionResult obtenerIngresosTotales()
        {
            return Ok(reporteData.ObtenerIngresosTotales());
        }
    }
}
