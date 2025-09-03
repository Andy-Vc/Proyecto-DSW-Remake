(function () {
    const {
        categoriaLabels,
        categoriaData,
        proveedorLabels,
        proveedorData,
        distritoLabels,
        distritoData,
        mesLabels,
        mesData,
        ventaTipoDataRaw
    } = window.dashboardData;

    // Paleta de colores mejorada y armoniosa para el dashboard

    const categoriaColors = [
        '#1f497d', // azul oscuro fuerte
        '#4f81bd', // azul medio
        '#9bbb59', // verde oliva claro
        '#c0504d', // rojo ladrillo (para acentos)
        '#f79646', // naranja cálido
        '#8064a2', // púrpura suave
        '#4bacc6', // azul aqua
        '#f2c80f'  // amarillo mostaza
    ];

    const proveedorColors = [
        '#2f5597', // azul oscuro
        '#3d85c6', // azul claro
        '#6aa84f', // verde oliva
        '#38761d', // verde oscuro
        '#e69138', // naranja quemado
        '#6d9eeb'  // azul pastel
    ];

    const distritoColors = [
        '#a9d18e', // verde pastel
        '#ffd966', // amarillo pastel
        '#f4b084', // naranja pastel
        '#9dc3e6', // azul pastel
        '#c5a5c5', // lavanda pastel
        '#d9d9d9'  // gris claro neutro
    ];

    const tipoVentaColors = {
        'P': '#2e75b6', // Presencial - azul saturado
        'R': '#a9d18e'  // Remota - naranja suave
    };

    // Gráfico de Categorías (pie)
    new Chart(document.getElementById('categoriaChart').getContext('2d'), {
        type: 'pie',
        data: {
            labels: categoriaLabels,
            datasets: [{
                label: 'Productos por Categoría',
                data: categoriaData,
                backgroundColor: categoriaLabels.map((_, i) => categoriaColors[i % categoriaColors.length])
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right',
                    labels: {
                        boxWidth: 20,
                        padding: 20
                    }
                }
            }
        }
    });

    // Gráfico de Proveedores (bar)
    new Chart(document.getElementById('proveedorChart').getContext('2d'), {
        type: 'bar',
        data: {
            labels: proveedorLabels,
            datasets: [{
                label: 'Productos por Proveedor',
                data: proveedorData,
                backgroundColor: proveedorLabels.map((_, i) => proveedorColors[i % proveedorColors.length])
            }]
        },
        options: {
            scales: {
                y: { beginAtZero: true }
            }
        }
    });

    // Gráfico de Distritos (bar)
    new Chart(document.getElementById('distritoChart').getContext('2d'), {
        type: 'bar',
        data: {
            labels: distritoLabels,
            datasets: [{
                label: 'Ventas por Distrito',
                data: distritoData,
                backgroundColor: distritoLabels.map((_, i) => distritoColors[i % distritoColors.length])
            }]
        },
        options: {
            scales: {
                y: { beginAtZero: true }
            }
        }
    });

    // Gráfico de Ventas por Mes (line)
    new Chart(document.getElementById('mesChart').getContext('2d'), {
        type: 'line',
        data: {
            labels: mesLabels,
            datasets: [{
                label: 'Ventas por Mes',
                data: mesData,
                borderColor: '#1f497d', // azul oscuro fuerte
                backgroundColor: 'rgba(31, 73, 125, 0.3)',
                fill: true,
                tension: 0.1
            }]
        },
        options: {
            scales: {
                y: { beginAtZero: true }
            }
        }
    });

    // Gráfico Ventas por Mes y Tipo de Venta (barras apiladas)
    const mesesUnicos = [...new Set(ventaTipoDataRaw.map(x => x.mes))];
    const tiposUnicos = [...new Set(ventaTipoDataRaw.map(x => x.tipoVenta))];

    const datasetsMesTipo = tiposUnicos.map(tipo => ({
        label: tipo === 'P' ? 'Presencial' : (tipo === 'R' ? 'Remota' : tipo),
        data: mesesUnicos.map(mes => {
            const item = ventaTipoDataRaw.find(x => x.mes === mes && x.tipoVenta === tipo);
            return item ? item.cantidadVentas : 0;
        }),
        backgroundColor: tipoVentaColors[tipo] || '#999999'
    }));

    new Chart(document.getElementById('mesTipoVentaChart').getContext('2d'), {
        type: 'bar',
        data: {
            labels: mesesUnicos,
            datasets: datasetsMesTipo
        },
        options: {
            responsive: true,
            scales: {
                x: { stacked: true },
                y: { stacked: true, beginAtZero: true }
            },
            plugins: {
                legend: {
                    position: 'top'
                }
            }
        }
    });

})();
