var filtroActual = "";

$(document).ready(function () {

    dtClientes = $('#datatable-clientes').DataTable({
        "responsive": true,
        "language": {
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
        },
        "pageLength": 10,
        "aaSorting": [],
        "dom": "<'row'<'col-sm-6'<'toolbar'>><'col-sm-6'fT>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        initComplete: function () {
            this.api().columns([0]).every(function () {
                var column = this;
                var select = $('<select class=""><option value="">Todos los Agentes</option></select>')
                    .appendTo($("div.toolbar").empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        column.search(val ? '^' + val + '$' : '', true, false).draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        },
        "ajax": {
            type: "POST",
            url: "SeguimientoColadeLlamadas.aspx/CargarSolicitudes",
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
            { "data": "NombreAgente", "orderable": false },
            {
                "data": "IDCliente",
                "render": function (data, type, row) {
                    return "<a href=" + row["UrlCliente"] + ">" + row["IDCliente"] + "</a>"
                }
            },
            { "data": "NombreCompletoCliente" },
            {
                "data": "Atraso",
                "render": function (data, type, row) {
                    return row["Moneda"] + ' ' + row["Atraso"]
                }
            },
            { "data": "DiasMora", "className": "text-center" },
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

    $("input[type=radio][name=filtros]").change(function () {

        var filtro = this.value;

        switch (filtro) {

            case "0":
                filtroActual = "";
                dtClientes.draw();
                break;

            case "incumplidas":
                filtroActual = "incumplidas";
                dtClientes.draw();
                break;

            case "hoy":
                filtroActual = "hoy";
                dtClientes.draw();
                break;

            case "futuras":
                filtroActual = "futuras";
                dtClientes.draw();
                break;
        }
    });
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {

            var estadoRetornar = false;
            var hoy = moment().format('YYYY/MM/DD');
            var promesaPago = data[6];

            if (filtroActual == "") {
                estadoRetornar = true;
            }
            else if (filtroActual == "incumplidas") {
                estadoRetornar = data[7] == 'Incumplida' ? true : false;
            }
            else if (filtroActual == "futuras" && (moment(promesaPago).isAfter(hoy))) {
                estadoRetornar = true;
            }
            else if (filtroActual == "hoy" && (moment(promesaPago).isSame(hoy))) {
                estadoRetornar = true;
            }
            return estadoRetornar;
        }
    );
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function addComasFormatoNumerico(nStr) {
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