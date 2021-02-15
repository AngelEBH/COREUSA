
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
        "processing": true,
        "dom": "<'row'<'col-sm-6'><'col-sm-6 text-right'>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        "ajax": {
            type: "POST",
            url: "SeguimientoRecuperacionDelDiaPorSupervisor.aspx/CargarRegistros",
            contentType: 'application/json; charset=utf-8',
            data: function (dtParms) {
                return JSON.stringify({ dataCrypt: window.location.href, IDAgente: $("#ddlAgentesActivos :selected").val() });
            },
            "dataSrc": function (json) {

                if (json.d == null) {
                    MensajeError('Ocurrió un error al cargar la información, contacta al administrador.');
                    return false;
                }

                var return_data = json.d;
                return return_data;
            }
        },
        "columns": [
            { "data": "NombreAgente" },
            {
                "data": "IDCliente",
                "render": function (data, type, row) {
                    return "<a class='text-info' href=" + row["UrlCliente"] + " target='_blank'>" + row["IDCliente"] + "</a>"
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
        buttons: [
            {
                extend: 'colvis',
                text: '<i class="mdi mdi-table-column-remove"></i> Columnas',
                columns: [1, 2, 3, 4, 5, 6]
            },
            {
                extend: 'excelHtml5',
                text: '<i class="far fa-file-excel"></i> Excel',
                title: 'Recuperacion_Del_Dia',
                autoFilter: true,
                messageTop: 'Recuperacion del día',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Imprimir',
                autoFilter: true,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6]
                }
            },
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
    });
});

/* Filtar por Agente */
$("#ddlAgentesActivos").change(function () {

    if ($("#ddlAgentesActivos :selected").val() != '')
        FiltrarInformacion();
});

/* Recargar DataTable */
function FiltrarInformacion() {

    if ($("#ddlAgentesActivos :selected").val() != '')
        dtClientes.ajax.reload(null, false);
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

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}
