var idEstado = "0";

$(document).ready(function () {

    dtBandejaPrecalificados = $('#datatable-precalificados').DataTable({
        "responsive": true,
        "language": {
            "sProcessing": "Cargando información...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "<small>Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros</small>",
            "sInfoEmpty": "<small>Mostrando registros del 0 al 0 de un total de 0 registros</small>",
            "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
            "sInfoPostFix": "",
            "sSearch": "Buscar:",
            "sUrl": "",
            "sInfoThousands": ",",
            "sLoadingRecords": "Cargando información...",
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
        "pageLength": 15,
        "processing": true,
        "aaSorting": [],
        "dom": "<'row'<'col-sm-6'><'col-sm-6'>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6 pt-0'i><'col-sm-6 pt-0'p>>",
        "ajax": {
            type: "POST",
            url: "BandejaPrecalificados.aspx/CargarLista",
            contentType: 'application/json; charset=utf-8',
            data: function (dtParms) {
                return JSON.stringify({ dataCrypt: window.location.href, pcEstado: idEstado });
            },
            "dataSrc": function (json) {
                var return_data = json.d;
                return return_data;
            }
        },
        "columns": [
            { defaultContent: '' },
            { "data": "NombreCliente" },
            { "data": "Identidad" },
            {
                "data": "Ingresos",
                "className": 'text-right',
                "render": function (data, type, row) {
                    return row["Moneda"] + ' ' + addFormatoNumerico(parseFloat(row["Ingresos"]).toFixed(2))
                }
            },
            { "data": "Telefono" },
            { "data": "Producto" },
            {
                "data": "FechaConsultado",
                "render": function (value) {
                    return value == '/Date(-62135575200000)/' ? '' : moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            { "data": "Oficial" },
            { "data": "Oficial" }
        ]
    });

    $("input[type=radio][name=filtros]").change(function () {

        idEstado = this.value;
        FiltrarPorEstado();
    });

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtBandejaPrecalificados.search($(this).val()).draw();
    });
});


function FiltrarPorEstado() {

    dtBandejaPrecalificados.ajax.reload(null, false);
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