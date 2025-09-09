document.addEventListener("DOMContentLoaded", () => {
    let productos = [];
    let seleccionados = [];
    let carrito = [];

    const apiUrl = "/Vendedor/ListarProductos";
    const apiVentaUrl = "/Vendedor/GenerarVentaVendedor";

    const tablaProductos = document.getElementById("tablaProductos");
    const contenedorSeleccionados = document.getElementById("contenedorSeleccionados");
    const contenedorCarrito = document.getElementById("contenedorCarrito");
    const countSeleccionados = document.getElementById("countSeleccionados");
    const countCarrito = document.getElementById("countCarrito");
    const alertPlaceholder = document.getElementById("alert-placeholder");
    const paginacion = document.getElementById("paginacionProductos");

    const userIdElement = document.getElementById("currentUserId");
    const idUsuario = userIdElement ? parseInt(userIdElement.value, 10) : 0;
    const inputBusqueda = document.getElementById("txtBusqueda");

    const productosPorPagina = 8;
    let paginaActual = 1;
    let productosFiltrados = [];

    if (idUsuario === 0) {
        console.error("No se pudo obtener el ID del usuario.");
    }

    function mostrarMensaje(mensaje, tipo = "info") {
        if (!alertPlaceholder) return;
        alertPlaceholder.innerHTML = `
            <div class="alert alert-${tipo} alert-dismissible fade show" role="alert">
                ${mensaje}
                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
            </div>
        `;
        setTimeout(() => {
            alertPlaceholder.innerHTML = "";
        }, 5000);
    }

    async function cargarProductos() {
        try {
            const resp = await fetch(apiUrl);
            if (!resp.ok) throw new Error(`${resp.status} - ${await resp.text()}`);
            productos = await resp.json();
            productosFiltrados = productos;
            renderProductos(productosFiltrados, 1);
        } catch (err) {
            console.error(err);
            mostrarMensaje("Error al cargar productos. Intente de nuevo más tarde.", "danger");
        }
    }

    function filtrarProductos() {
        const textoBusqueda = inputBusqueda.value.toLowerCase();
        productosFiltrados = productos.filter(p =>
            p.nombre.toLowerCase().includes(textoBusqueda) ||
            p.categoria.descripcion.toLowerCase().includes(textoBusqueda) ||
            String(p.idProducto).includes(textoBusqueda)
        );
        renderProductos(productosFiltrados, 1);
    }

    function renderProductos(lista = productosFiltrados, pagina = 1) {
        paginaActual = pagina;
        const inicio = (pagina - 1) * productosPorPagina;
        const fin = inicio + productosPorPagina;
        const productosPagina = lista.slice(inicio, fin);

        tablaProductos.innerHTML = productosPagina.map(p => `
            <tr>
                <td>${p.idProducto}</td>
                <td>${p.nombre}</td>
                <td>${p.categoria.descripcion}</td>
                <td>S/. ${p.precio.toFixed(2)}</td>
                <td>${p.stock}</td>
                <td>
                    <button class="btn btn-sm bg-color-primary text-white" data-id="${p.idProducto}" ${p.stock === 0 ? 'disabled' : ''}>
                        <i class="bi bi-plus-lg"></i> Agregar
                    </button>
                </td>
            </tr>
        `).join("");

        renderPaginacion(lista.length, pagina);
    }

    function renderPaginacion(total, pagina) {
        const totalPaginas = Math.ceil(total / productosPorPagina);
        if (!paginacion || totalPaginas <= 1) {
            paginacion.innerHTML = '';
            return;
        }

        let botones = '';

        // Botón anterior
        botones += `
        <li class="page-item ${pagina === 1 ? 'disabled' : ''}">
            <a class="page-link" href="#" data-pagina="${pagina - 1}" aria-label="Anterior">
                <span aria-hidden="true">&laquo;</span>
            </a>
        </li>
    `;

        // Botones numéricos
        for (let i = 1; i <= totalPaginas; i++) {
            botones += `
            <li class="page-item ${i === pagina ? 'active' : ''}">
                <a class="page-link" href="#" data-pagina="${i}">${i}</a>
            </li>
        `;
        }

        // Botón siguiente
        botones += `
        <li class="page-item ${pagina === totalPaginas ? 'disabled' : ''}">
            <a class="page-link" href="#" data-pagina="${pagina + 1}" aria-label="Siguiente">
                <span aria-hidden="true">&raquo;</span>
            </a>
        </li>
    `;

        paginacion.innerHTML = botones;
    }


    function renderSeleccionados() {
        if (seleccionados.length === 0) {
            contenedorSeleccionados.innerHTML = `<p class="text-center text-muted py-4">No hay productos seleccionados</p>`;
        } else {
            contenedorSeleccionados.innerHTML = `
                <table class="table table-hover align-middle">
                    <thead class="table-light">
                        <tr>
                            <th>Producto</th>
                            <th>Precio</th>
                            <th>Cantidad</th>
                            <th>Subtotal</th>
                            <th>Acción</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${seleccionados.map(item => `
                            <tr>
                                <td>${item.producto.nombre}</td>
                                <td>S/. ${item.producto.precio.toFixed(2)}</td>
                                <td>
                                    <div class="btn-group btn-group-sm" role="group">
                                        <button class="btn btn-danger btn-disminuir" data-id="${item.producto.idProducto}" style="border-radius: .5rem !important;"><i class="bi bi-dash"></i></button>
                                        <span class="px-2">${item.cantidad}</span>
                                        <button class="btn btn-success btn-aumentar" data-id="${item.producto.idProducto}" style="border-radius: .5rem !important;"><i class="bi bi-plus"></i></button>
                                    </div>
                                </td>
                                <td>S/. ${item.subTotal.toFixed(2)}</td>
                                <td>
                                    <button class="btn btn-sm btn-danger btn-eliminar-seleccionado" data-id="${item.producto.idProducto}" title="Eliminar">
                                        <i class="bi bi-trash"></i>
                                    </button>
                                </td>
                            </tr>
                        `).join("")}
                    </tbody>
                </table>
                <div class="d-flex justify-content-end mt-3">
                    <button class="btn btn-success" id="btnPasarAlCarrito">
                        <i class="bi bi-cart-check"></i> Agregar a Venta
                    </button>
                </div>
            `;
        }
        countSeleccionados.textContent = `${seleccionados.length} producto${seleccionados.length !== 1 ? 's' : ''}`;
    }

    function renderCarrito() {
        if (carrito.length === 0) {
            contenedorCarrito.innerHTML = `<p class="text-center text-muted py-4">El carrito está vacío</p>`;
        } else {
            const total = carrito.reduce((acc, d) => acc + d.subTotal, 0);
            contenedorCarrito.innerHTML = `
                <table class="table table-bordered align-middle">
                    <thead class="table-light">
                        <tr>
                            <th>Producto</th>
                            <th>Cantidad</th>
                            <th>Precio</th>
                            <th>Subtotal</th>
                            <th>Acción</th>
                        </tr>
                    </thead>
                    <tbody>
                        ${carrito.map(item => `
                            <tr>
                                <td>${item.producto.nombre}</td>
                                <td>${item.cantidad}</td>
                                <td>S/. ${item.producto.precio.toFixed(2)}</td>
                                <td>S/. ${item.subTotal.toFixed(2)}</td>
                                <td>
                                    <button class="btn btn-sm btn-danger btn-eliminar-carrito" data-id="${item.producto.idProducto}" title="Eliminar">
                                        <i class="bi bi-trash"></i>
                                    </button>
                                </td>
                            </tr>
                        `).join("")}
                    </tbody>
                </table>
                <h5 class="text-end fw-bold">Total: S/. ${total.toFixed(2)}</h5>
                <div class="d-flex justify-content-end mt-3">
                    <button class="btn btn-success" id="btnFinalizarVenta">
                        <i class="bi bi-check-circle"></i> Finalizar Venta
                    </button>
                </div>
            `;
        }
        countCarrito.textContent = `${carrito.length} producto${carrito.length !== 1 ? 's' : ''}`;
    }

    function agregarSeleccionado(id) {
        const producto = productos.find(p => p.idProducto === id);
        if (!producto) return;

        if (producto.stock === 0) {
            Swal.fire({
                icon: 'info',
                title: 'Sin stock',
                text: 'Este producto no tiene stock disponible.'
            });
            return;
        }

        const existente = seleccionados.find(s => s.producto.idProducto === id);

        if (existente) {
            if (existente.cantidad < producto.stock) {
                existente.cantidad++;
                existente.subTotal = existente.cantidad * producto.precio;
            } else {
                Swal.fire({
                    icon: 'warning',
                    title: 'Stock insuficiente',
                    text: `Solo hay ${producto.stock} unidades disponibles.`
                });
            }
        } else {
            seleccionados.push({
                producto,
                cantidad: 1,
                subTotal: producto.precio
            });
        }

        renderSeleccionados();
    }

    function aumentarCantidad(id) {
        const item = seleccionados.find(d => d.producto.idProducto === id);
        if (!item) return;

        if (item.cantidad < item.producto.stock) {
            item.cantidad++;
            item.subTotal = item.cantidad * item.producto.precio;
        } else {
            Swal.fire({
                icon: 'warning',
                title: 'Stock insuficiente',
                text: `No hay más stock disponible para este producto.`
            });
        }

        renderSeleccionados();
    }

    function disminuirCantidad(id) {
        const index = seleccionados.findIndex(d => d.producto.idProducto === id);
        if (index === -1) return;

        const item = seleccionados[index];
        if (item.cantidad > 1) {
            item.cantidad--;
            item.subTotal = item.cantidad * item.producto.precio;
        } else {
            seleccionados.splice(index, 1);
        }

        renderSeleccionados();
    }

    function eliminarSeleccionado(id) {
        seleccionados = seleccionados.filter(item => item.producto.idProducto !== id);
        renderSeleccionados();
    }

    function eliminarDelCarrito(id) {
        carrito = carrito.filter(item => item.producto.idProducto !== id);
        renderCarrito();
    }

    function pasarAlCarrito() {
        if (seleccionados.length === 0) {
            Swal.fire('Aviso', 'No hay productos seleccionados para agregar a la venta.', 'warning');
            return;
        }

        seleccionados.forEach(sel => {
            const enCarrito = carrito.find(c => c.producto.idProducto === sel.producto.idProducto);
            if (enCarrito) {
                const cantidadTotal = enCarrito.cantidad + sel.cantidad;
                if (cantidadTotal <= sel.producto.stock) {
                    enCarrito.cantidad = cantidadTotal;
                    enCarrito.subTotal = enCarrito.cantidad * enCarrito.producto.precio;
                } else {
                    Swal.fire('Stock insuficiente', `No hay suficiente stock para ${sel.producto.nombre}`, 'warning');
                }
            } else {
                carrito.push({ ...sel });
            }
        });

        seleccionados = [];
        renderSeleccionados();
        renderCarrito();
    }

    async function finalizarVenta() {
        if (carrito.length === 0) {
            Swal.fire('Carrito vacío', 'Agrega productos antes de realizar la venta.', 'error');
            return;
        }

        if (idUsuario === 0) {
            Swal.fire('Error de usuario', 'No se puede procesar la venta sin un ID de usuario.', 'error');
            return;
        }

        const venta = {
            idVenta: 0,
            idUsuario,
            fecha: new Date().toISOString(),
            total: carrito.reduce((acc, d) => acc + d.subTotal, 0),
            tipoVenta: null,
            estado: null,
            detalles: carrito.map(item => ({
                idProducto: item.producto.idProducto,
                cantidad: item.cantidad,
                subTotal: item.subTotal
            }))
        };

        try {
            const resp = await fetch(apiVentaUrl, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(venta)
            });

            if (resp.ok) {
                const result = await resp.json();

                Swal.fire({
                    icon: 'success',
                    title: `Venta #${result.idVenta} realizada con éxito`,
                    text: 'Gracias por su compra',
                    timer: 4000,
                    timerProgressBar: true,
                    confirmButtonText: 'Aceptar'
                });

                carrito = [];
                renderCarrito();
                cargarProductos();
            } else {
                const error = await resp.text();
                Swal.fire('Error en la venta', `${resp.statusText}: ${error}`, 'error');
            }
        } catch (err) {
            console.error("Error en la venta:", err);
            Swal.fire('Error inesperado', 'Ocurrió un error al procesar la venta. Inténtalo de nuevo.', 'error');
        }
    }

    // Delegación de eventos
    document.addEventListener('click', e => {
        const btn = e.target.closest('button');
        if (!btn) return;

        const id = btn.dataset.id ? parseInt(btn.dataset.id, 10) : null;

        if (btn.classList.contains('btn-disminuir')) {
            disminuirCantidad(id);
        } else if (btn.classList.contains('btn-aumentar')) {
            aumentarCantidad(id);
        } else if (btn.classList.contains('btn-eliminar-seleccionado')) {
            eliminarSeleccionado(id);
        } else if (btn.classList.contains('btn-eliminar-carrito')) {
            eliminarDelCarrito(id);
        } else if (btn.textContent.trim().includes('Agregar') || btn.querySelector('i.bi-plus-lg')) {
            agregarSeleccionado(id);
        }
    });

    document.addEventListener("click", e => {
        if (e.target.id === "btnPasarAlCarrito") {
            pasarAlCarrito();
        }
        if (e.target.id === "btnFinalizarVenta") {
            finalizarVenta();
        }
    });

    inputBusqueda.addEventListener("input", filtrarProductos);

    document.addEventListener("click", e => {
        const link = e.target.closest("a[data-pagina]");
        if (link) {
            e.preventDefault();
            const nuevaPagina = parseInt(link.dataset.pagina);
            renderProductos(productosFiltrados, nuevaPagina);
        }
    });

    // Inicializar
    cargarProductos().then(() => {
        renderSeleccionados();
        renderCarrito();
    });
});
