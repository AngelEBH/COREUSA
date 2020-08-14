var FiltroActual = "";
var lenguajeEspanol = {
    "sProcessing": "Cargando registros...",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sZeroRecords": "No se encontraron resultados",
    "sEmptyTable": "Ningún dato disponible en esta tabla",
    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
    "sInfoPostFix": "",
    "sSearch": "Buscar:",
    "sUrl": "",
    "sInfoThousands": ",",
    "sLoadingRecords": "Cargando solicitudes...",
    "oPaginate": {
        "sFirst": "Primero",
        "sLast": "Último",
        "sNext": "Siguiente",
        "sPrevious": "Anterior"
    },
    "oAria": {
        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
    },
    "decimal": ".",
    "thousands": ","
};

$(document).ready(function () {
    dtClientes = $('#datatable-clientes').DataTable({
        "responsive": true,
        "language": lenguajeEspanol,
        "pageLength": 10,
        "aaSorting": [],
        //"processing": true,
        "dom": "<'row'<'col-sm-6'<'toolbar'>><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        initComplete: function () {
            this.api().columns([0]).every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm w-auto"><option value="">Todos los Agentes</option></select>')
                    .appendTo($("div.toolbar").empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        },
        "ajax": {
            type: "POST",
            url: "SeguimientoJefePromesasdePago.aspx/CargarSolicitudes",
            contentType: 'application/json; charset=utf-8',
            data: function (dtParms) {
                return JSON.stringify({ dataCrypt: window.location.href });
            },
            "dataSrc": function (json) {
                var return_data = json.d;
                return return_data;
            }
        },
        "columns": [
            { "data": "NombreAgente" },
            {
                "data": "IDCliente",
                "render": function (data, type, row) {
                    return "<a href=" + row["UrlCliente"] + ">" + row["IDCliente"] + "</a>"
                }
            },
            { "data": "NombreCompletoCliente" },
            {
                "data": "Atraso",
                "className": 'text-right',
                "render": function (data, type, row) {
                    return row["Moneda"] + ' ' + addFormatoNumerico(parseFloat(row["Atraso"]).toFixed(2))
                }
            },
            { "data": "DiasMora" , "className" : "text-center"},
            {
                "data": "FechaRegistrado",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            {
                "data": "FechaPromesa",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD');
                }
            },
            { "data": "EstadoActual" }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    /* Filtrar cuando se seleccione una opción */
    $("input[type=radio][name=filtros]").change(function () {
        var filtro = this.value;
        switch (filtro) {
            case "0":
                FiltroActual = "";
                dtClientes.draw();
                break;
            case "incumplidas":
                FiltroActual = "incumplidas";
                dtClientes.draw();
                break;
            case "hoy":
                FiltroActual = "hoy";
                dtClientes.draw();
                break;
            case "futuras":
                FiltroActual = "futuras";
                dtClientes.draw();
                break;
        }
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {

            var EstadoRetornar = false;
            var hoy = moment().format('YYYY/MM/DD');
            var promesaPago = data[6];

            if (FiltroActual == "") {
                EstadoRetornar = true;
            }
            else if (FiltroActual == "incumplidas") {
                EstadoRetornar = data[7] == 'Incumplida' ? true : false;
            }
            else if (FiltroActual == "futuras" && (moment(promesaPago).isAfter(hoy))) {
                EstadoRetornar = true;
            }
            else if (FiltroActual == "hoy" && (moment(promesaPago).isSame(hoy))) {
                EstadoRetornar = true;
            }
            return EstadoRetornar;
        }
    );

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtClientes.search($(this).val()).draw();
    })

    /* Listas seleccionables */
    $(".buscadorddl").select2({
        language: {
            errorLoading: function () { return "No se pudieron cargar los resultados" },
            inputTooLong: function (e) { var n = e.input.length - e.maximum, r = "Por favor, elimine " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
            inputTooShort: function (e) { var n = e.minimum - e.input.length, r = "Por favor, introduzca " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
            loadingMore: function () { return "Cargando más resultados…" },
            maximumSelected: function (e) { var n = "Sólo puede seleccionar " + e.maximum + " elemento"; return 1 != e.maximum && (n += "s"), n },
            noResults: function () { return "No se encontraron resultados" },
            searching: function () { return "Buscando…" },
            removeAllItems: function () { return "Eliminar todos los elementos" }
        }
    });
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function addFormatoNumerico(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}