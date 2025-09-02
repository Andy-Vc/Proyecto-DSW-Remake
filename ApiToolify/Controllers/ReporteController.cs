using ApiToolify.Data.Contratos;
using ClosedXML.Excel;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiToolify.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReporteController : ControllerBase
    {
        private readonly IReporte _dataReportes;

        public ReporteController(IReporte inyect1)
        {
            this._dataReportes = inyect1;
        }
        [HttpGet("ListarPorMesAndTipoVenta")]
        public IActionResult ListadoPorMesAndTipoVenta(DateTime? fechaInicio, DateTime? fechaFin, string? tipo)
        {
            var listado = _dataReportes.ListadoPorMesAndTipoVenta(fechaInicio, fechaFin, tipo);
            return Ok(listado);
        }
        [HttpGet("ListarProductosPorCategoria")]
        public IActionResult ListadoProductosPorCategoria(int? idCategoria = null, string? orden = null)
        {
            var listado = _dataReportes.ListarProductosPorCategoria(idCategoria, orden);
            return Ok(listado);
        }
        [HttpGet("ListadoPorMesAndTipoVentaExcel")]
        public IActionResult ListadoPorMesAndTipoVentaExcel(DateTime? fechaInicio, DateTime? fechaFin, string? tipo)
        {
            var listado = _dataReportes.ListadoPorMesAndTipoVenta(fechaInicio, fechaFin, tipo);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Ventas");

            worksheet.Cell(1, 1).Value = "ID Venta";
            worksheet.Cell(1, 2).Value = "Fecha";
            worksheet.Cell(1, 3).Value = "Cliente";
            worksheet.Cell(1, 4).Value = "Dirección";
            worksheet.Cell(1, 5).Value = "Total";
            worksheet.Cell(1, 6).Value = "Estado";
            worksheet.Cell(1, 7).Value = "Tipo Venta";

            int fila = 2;
            foreach (var venta in listado)
            {
                worksheet.Cell(fila, 1).Value = venta.idVenta;
                worksheet.Cell(fila, 2).Value = venta.fechaGenerada.ToString("yyyy-MM-dd");
                worksheet.Cell(fila, 3).Value = venta.nombresCompletos;
                worksheet.Cell(fila, 4).Value = venta.direccion;
                worksheet.Cell(fila, 5).Value = venta.total;
                worksheet.Cell(fila, 6).Value = venta.estado;
                worksheet.Cell(fila, 7).Value = venta.tipoVenta == "P" ? "Presencial" : "Remota";
                fila++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Ventas_{DateTime.Now:yyyyMMdd}.xlsx");
        }
        [HttpGet("ListadoPorMesAndTipoVentaPdf")]
        public IActionResult ListadoPorMesAndTipoVentaPdf(DateTime? fechaInicio, DateTime? fechaFin, string? tipo)
        {
            var listado = _dataReportes.ListadoPorMesAndTipoVenta(fechaInicio, fechaFin, tipo);

            using var ms = new MemoryStream();
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(document, ms);
            document.Open();

            var titulo = new Paragraph("Reporte de Ventas") { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20f };
            document.Add(titulo);

            PdfPTable table = new PdfPTable(7) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 10f, 20f, 20f, 15f, 10f, 10f, 15f });

            string[] headers = { "ID", "Fecha", "Cliente", "Dirección", "Total", "Estado", "Tipo" };
            foreach (var header in headers)
            {
                var cell = new PdfPCell(new Phrase(header))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                table.AddCell(cell);
            }

            foreach (var venta in listado)
            {
                table.AddCell(venta.idVenta.ToString());
                table.AddCell(venta.fechaGenerada.ToString("yyyy-MM-dd"));
                table.AddCell(venta.nombresCompletos ?? "");
                table.AddCell(venta.direccion ?? "");
                table.AddCell(venta.total.ToString());
                table.AddCell(venta.estado);
                table.AddCell(venta.tipoVenta == "P" ? "Presencial" : "Remota");
            }

            document.Add(table);
            document.Close();

            return File(ms.ToArray(), "application/pdf", $"Ventas_{DateTime.Now:yyyyMMdd}.pdf");
        }

        [HttpGet("ListarProductosPorCategoriaExcel")]
        public IActionResult ListarProductosPorCategoriaExcel(int? idCategoria = null, string? orden = null)
        {
            var listado = _dataReportes.ListarProductosPorCategoria(idCategoria, orden);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Productos");

            worksheet.Cell(1, 1).Value = "ID Producto";
            worksheet.Cell(1, 2).Value = "Nombre";
            worksheet.Cell(1, 3).Value = "Descripción";
            worksheet.Cell(1, 4).Value = "Proveedor";
            worksheet.Cell(1, 5).Value = "Categoría";
            worksheet.Cell(1, 6).Value = "Precio";
            worksheet.Cell(1, 7).Value = "Stock";
            worksheet.Cell(1, 8).Value = "Fecha Registro";

            int fila = 2;
            foreach (var producto in listado)
            {
                worksheet.Cell(fila, 1).Value = producto.idProducto;
                worksheet.Cell(fila, 2).Value = producto.nombre;
                worksheet.Cell(fila, 3).Value = producto.descripcion;
                worksheet.Cell(fila, 4).Value = producto.proveedor;
                worksheet.Cell(fila, 5).Value = producto.categoria;
                worksheet.Cell(fila, 6).Value = producto.precio;
                worksheet.Cell(fila, 7).Value = producto.stock;
                worksheet.Cell(fila, 8).Value = producto.fechaRegistro.ToString("yyyy-MM-dd");
                fila++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return File(content,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"Productos_{DateTime.Now:yyyyMMdd}.xlsx");
        }
        [HttpGet("ListarProductosPorCategoriaPdf")]
        public IActionResult ListarProductosPorCategoriaPdf(int? idCategoria = null, string? orden = null)
        {
            var listado = _dataReportes.ListarProductosPorCategoria(idCategoria, orden);

            using var ms = new MemoryStream();
            var document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(document, ms);
            document.Open();

            var titulo = new Paragraph("Reporte de Productos") { Alignment = Element.ALIGN_CENTER, SpacingAfter = 20f };
            document.Add(titulo);

            PdfPTable table = new PdfPTable(8) { WidthPercentage = 100 };
            table.SetWidths(new float[] { 10f, 15f, 20f, 15f, 15f, 10f, 10f, 15f });

            var headers = new[] { "ID", "Nombre", "Descripción", "Proveedor", "Categoría", "Precio", "Stock", "Fecha" };
            foreach (var h in headers)
            {
                var cell = new PdfPCell(new Phrase(h))
                {
                    BackgroundColor = BaseColor.LIGHT_GRAY,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                table.AddCell(cell);
            }

            foreach (var p in listado)
            {
                table.AddCell(p.idProducto.ToString());
                table.AddCell(p.nombre ?? "");
                table.AddCell(p.descripcion ?? "");
                table.AddCell(p.proveedor ?? "");
                table.AddCell(p.categoria ?? "");
                table.AddCell(p.precio.ToString("0.00"));
                table.AddCell(p.stock.ToString());
                table.AddCell(p.fechaRegistro.ToString("yyyy-MM-dd"));
            }

            document.Add(table);
            document.Close();

            return File(ms.ToArray(), "application/pdf", $"Productos_{DateTime.Now:yyyyMMdd}.pdf");
        }

    }
}
