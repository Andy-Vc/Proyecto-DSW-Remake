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

    function getRandomColor() {
        const letters = '0123456789ABCDEF';
        let color = '#';
        for (let i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }

    // Categoría por Producto
    new Chart(document.getElementById('categoriaChart').getContext('2d'), {
        type: 'pie',
        data: {
            labels: categoriaLabels,
            datasets: [{
                label: 'Productos por Categoría',
                data: categoriaData,
                backgroundColor: categoriaLabels.map(() => getRandomColor())
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


    // Proveedor por Producto
    new Chart(document.getElementById('proveedorChart').getContext('2d'), {
        type: 'bar',
        data: {
            labels: proveedorLabels,
            datasets: [{
                label: 'Productos por Proveedor',
                data: proveedorData,
                backgroundColor: proveedorLabels.map(() => getRandomColor())
            }]
        },
        options: {
            scales: {
                y: { beginAtZero: true }
            }
        }
    });

    // Venta por Distrito
    new Chart(document.getElementById('distritoChart').getContext('2d'), {
        type: 'bar',
        data: {
            labels: distritoLabels,
            datasets: [{
                label: 'Ventas por Distrito',
                data: distritoData,
                backgroundColor: distritoLabels.map(() => getRandomColor())
            }]
        },
        options: {
            scales: {
                y: { beginAtZero: true }
            }
        }
    });

    // Venta por Mes
    new Chart(document.getElementById('mesChart').getContext('2d'), {
        type: 'line',
        data: {
            labels: mesLabels,
            datasets: [{
                label: 'Ventas por Mes',
                data: mesData,
                borderColor: '#3e95cd',
                fill: false,
                tension: 0.1
            }]
        },
        options: {
            scales: {
                y: { beginAtZero: true }
            }
        }
    });

    // Venta por Mes y Tipo de Venta (barras apiladas)
    const mesesUnicos = [...new Set(ventaTipoDataRaw.map(x => x.mes))];
    const tiposUnicos = [...new Set(ventaTipoDataRaw.map(x => x.tipoVenta))];

    const datasetsMesTipo = tiposUnicos.map(tipo => {
        return {
            label: tipo,
            data: mesesUnicos.map(mes => {
                const item = ventaTipoDataRaw.find(x => x.mes === mes && x.tipoVenta === tipo);
                return item ? item.cantidadVentas : 0;
            }),
            backgroundColor: getRandomColor()
        };
    });

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
            }
        }
    });

})();
