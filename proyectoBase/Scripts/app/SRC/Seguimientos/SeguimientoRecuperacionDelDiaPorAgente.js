var IDAgente = 0;

$(document).ready(function () {
    dtClientes = $('#datatable-recuperacion').DataTable({
        "responsive": true,
        "language": {
            "sProcessing": "Cargando información...",
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
        "pageLength": 10,
        "aaSorting": [],
        //"processing": true,
        "dom": "<'row'<'col-sm-6'><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
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
                    return row["Moneda"] + ' ' + addFormatoNumerico(parseFloat(row["SaldoInicialPonerAlDia"]).toFixed(2))
                }
            },
            {
                "data": "AbonosHoy",
                "className": 'text-right sum',
                "render": function (data, type, row) {
                    return row["Moneda"] + ' ' + addFormatoNumerico(parseFloat(row["AbonosHoy"]).toFixed(2))
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
                $($("#lblTotalRecuperadoHoy").empty()).html('L ' + addFormatoNumerico(parseFloat(sum).toFixed(2)));
            });
        }
    });

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtClientes.search($(this).val()).draw();
    })

    /* Buscador para listas seleccionables */
    //$(".buscadorddl").select2({
    //    language: {
    //        errorLoading: function () { return "No se pudieron cargar los resultados" },
    //        inputTooLong: function (e) { var n = e.input.length - e.maximum, r = "Por favor, elimine " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
    //        inputTooShort: function (e) { var n = e.minimum - e.input.length, r = "Por favor, introduzca " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
    //        loadingMore: function () { return "Cargando más resultados…" },
    //        maximumSelected: function (e) { var n = "Sólo puede seleccionar " + e.maximum + " elemento"; return 1 != e.maximum && (n += "s"), n },
    //        noResults: function () { return "No se encontraron resultados" },
    //        searching: function () { return "Buscando…" },
    //        removeAllItems: function () { return "Eliminar todos los elementos" }
    //    }
    //});
});

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