document.addEventListener("DOMContentLoaded", () => {

});

const botonesDetalle = document.querySelectorAll('.btn-detalle-venta');
const modalBody = document.querySelector('#detalleVentaModal .modal-body');
const modalTitle = document.querySelector('#detalleVentaModal .modal-title');

fetch("/Vendedor/DatosTotales", { cache: "no-store" })
    .then(res => res.json())
    .then(data => {
        document.getElementById("totalVentas").textContent = data.totalVentas;
        document.getElementById("totalProductos").textContent = data.totalProductosVendidos;
        document.getElementById("ingresosTotales").textContent = `S/ ${formatearMiles(data.ingresosTotales)}`;
    })
    .catch(err => console.error("Error al cargar datos totales:", err));

botonesDetalle.forEach(boton => {
    boton.addEventListener('click', function () {

        const idVenta = this.getAttribute('data-idventa');

        modalBody.innerHTML = '<p class="text-center text-muted">Cargando detalles de la venta...</p>';
        modalTitle.textContent = 'Detalle de Venta...';

        fetch(`/Vendedor/DetalleVenta?idVenta=${idVenta}`)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Error al obtener los datos de la venta.');
                }
                return response.json();
            })
            .then(data => {
                modalTitle.textContent = `Detalle de Venta #${data.idVenta}`;

                modalBody.innerHTML = '';

                const estadoTexto = data.estado === "G" ? "Generado" :
                    data.estado === "T" ? "Transportado" :
                        data.estado === "E" ? "Entregado" :
                            data.estado === "P" ? "Pendiente" :
                                data.estado === "C" ? "Cancelado" :
                                    "Desconocido";

                const estadoClase = data.estado === "G" ? "bg-warning" :
                    data.estado === "T" ? "bg-info" :
                        data.estado === "E" ? "bg-success" :
                            data.estado === "C" ? "bg-danger" :
                                "bg-secondary";

                const htmlContent = `
	<div class="mb-3">
		<p><i class="fas fa-id-card me-2 text-primary-custom"></i><strong>Documento:</strong> ${data.usuario.nroDoc}</p>
		<p><i class="fas fa-calendar-alt me-2 text-primary-custom"></i><strong>Fecha:</strong> ${new Date(data.fecha).toLocaleDateString('es-ES', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' })}</p>
		<p><i class="fas fa-money-bill-wave me-2 text-primary-custom"></i><strong>Total:</strong> S/. ${formatearMiles(data.total)}</p>
		<p><i class="fas fa-info-circle me-2 text-primary-custom"></i><strong>Estado:</strong>
			<span class="badge ${estadoClase} text-white">${estadoTexto}</span>
		</p>
	</div>
	<hr>
	<h6 class="fw-bold text-dark mb-3">
		<i class="fas fa-box-open me-2 text-success"></i> Productos
	</h6>
	<table class="table table-bordered table-striped table-hover table-sm align-middle">
		<thead class="table-light">
			<tr>
				<th>Producto</th>
				<th>Cantidad</th>
				<th>Subtotal</th>
			</tr>
		</thead>
		<tbody id="detalles-productos"></tbody>
	</table>
`;


                modalBody.innerHTML = htmlContent;

                const tbody = document.getElementById('detalles-productos');
                if (data.detalles && data.detalles.length > 0) {
                    data.detalles.forEach(det => {
                        const row = document.createElement('tr');

                        row.innerHTML = `
                                <td>${det.producto.nombre}</td>
                                <td>${det.cantidad}</td>
                                <td>S/. ${formatearMiles(det.subTotal)}</td>
                            `;
                        tbody.appendChild(row);
                    });
                } else {

                    const row = document.createElement('tr');
                    row.innerHTML = `<td colspan="3" class="text-muted text-center">No hay detalles de productos para esta venta.</td>`;
                    tbody.appendChild(row);
                }
            })
            .catch(error => {
                console.error('Error:', error);

                modalBody.innerHTML = `<p class="text-center text-danger">Ocurrió un error: ${error.message}</p>`;
            });
    });
});

function formatearMiles(numero) {
    return numero
        .toFixed(2)
        .replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}
