var lenguaje = {
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
var IDAgente = 0;

$(document).ready(function () {
    dtClientes = $('#datatable-recuperacion').DataTable({
        "responsive": true,
        "language": lenguaje,
        "pageLength": 10,
        "aaSorting": [],
        "processing": true,
        "ajax": {
            type: "POST",
            url: "SeguimientoRecuperacionDelDiaPorAgente.aspx/CargarRegistros",
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
                    return "<a class='text-info' href=" + row["UrlCliente"] + ">" + row["IDCliente"] + "</a>"
                }
            },
            { "data": "NombreCompletoCliente" },
            { "data": "Descripcion" },
            { "data": "DiasAtraso", "className": "text-center" },
            {
                "data": "SaldoInicialPonerAlDia",
                "className": 'text-right',
                "render": function (data, type, row) {
                    return row["Moneda"] + ' ' + addComasFormatoNumerico(parseFloat(row["SaldoInicialPonerAlDia"]).toFixed(2))
                }
            },
            {
                "data": "AbonosHoy",
                "className": 'text-right sum',
                "render": function (data, type, row) {
                    return row["Moneda"] + ' ' + addComasFormatoNumerico(parseFloat(row["AbonosHoy"]).toFixed(2))
                }
            }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ],
        "footerCallback": function (row, data, start, end, display) {
            var api = this.api();
            api.columns('.sum').every(function () {
                var sum = this
                    .data()
                    .reduce(function (a, b) {
                        var x = parseFloat(a) || 0;
                        var y = parseFloat(b) || 0;
                        return x + y;
                    }, 0);
                $($("#lblTotalRecuperadoHoy").empty()).html('L ' + addComasFormatoNumerico(parseFloat(sum).toFixed(2)));
            });
        }
    });
});

$("#ddlAgentesActivos").change(function () {
    if ($("#ddlAgentesActivos :selected").val() != '') {
        FiltrarInformacion();
    }
});

function FiltrarInformacion() {
    if ($("#ddlAgentesActivos :selected").val() != '') {
        dtClientes.ajax.reload(null, false);
    }
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