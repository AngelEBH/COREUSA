var FiltroActual = "";
var idSolicitud = 0;
var identidadCliente = "";
estadoMasRelevante = '';

$(document).ready(function () {

    dtBandeja = $('#datatable-bandeja').DataTable({
        //"responsive": true,
        "pageLength": 20,
        "aaSorting": [],
        "dom": "<'row'<'col-sm-6'><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
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
        "ajax": {
            type: "POST",
            url: "SolicitudesCredito_Ingresadas.aspx/CargarSolicitudes",
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
            { "data": "fcIdentidadCliente" },
            {
                "data": "fcPrimerNombreCliente",
                "render": function (data, type, row) {
                    return row["fcPrimerNombreCliente"] + ' ' + row["fcSegundoNombreCliente"] + ' ' + row["fcPrimerApellidoCliente"] + ' ' + row["fcSegundoApellidoCliente"]
                }
            },
            {
                "data": "fdFechaCreacionSolicitud",
                "render": function (value) {
                    if (value === '/Date(-62135575200000)/') return "";
                    return moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            { "data": "fcDescripcion" },
            {
                "data": "fiIDSolicitud",
                "render": function (value) {

                    return '<button id="btnDetalles" data-id="' + value + '" class="btn btn-sm btn-block btn-info mb-0">Detalles</button>';
                }
            },
            {
                "data": "fiEstadoSolicitud",
                "render": function (data, type, row) {

                    if (row["fiEstadoSolicitud"] == 1) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En recepción</label>';
                    }

                    if (row["fiEstadoSolicitud"] == 2) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En análisis</label>';
                    }

                    if (row["fdReprogramadoInicio"] != '/Date(-62135575200000)/' && row["fdReprogramadoFin"] == '/Date(-62135575200000)/') {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">Reprogramada</label>';
                    }

                    if (row["fiEstadoDeCampo"] == 1) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En campo</label>';
                    }

                    if (row["fdCondicionadoInicio"] != '/Date(-62135575200000)/' && row["fdCondificionadoFin"] == '/Date(-62135575200000)/') {
                        estadoMasRelevante = '<button id="btnActualizar" data-id="' + row["fiIDSolicitud"] + '" class="btn btn-sm btn-block btn-warning mb-0">Condicionada</button>';
                    }
                    if (row["fiEstadoSolicitud" == 4] || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7) {
                        estadoMasRelevante = row["fiEstadoSolicitud"] == 7 ? '<label class="btn btn-sm btn-block btn-success mb-0">Aprobada</label>' : '<label class="btn btn-sm btn-block btn-danger mb-0">Rechazada</label>';
                    }

                    return estadoMasRelevante;
                }
            }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            dtBandeja.columns(2)
                .search('/' + this.value + '/')
                .draw();
        }
        else {
            dtBandeja.columns(2)
                .search('')
                .draw();
        }
    });

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        dtBandeja.columns(2)
            .search(this.value + '/')
            .draw();
    });

    $("#min").datepicker({
        onSelect: function () {
            FiltroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#max").datepicker({
        onSelect: function () {
            FiltroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#min, #max").change(function () {
        FiltroActual = 'rangoFechas';
        dtBandeja.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        if (FiltroActual == 'rangoFechas') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[2]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else { return true; }
    });

    $("#añoIngreso").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtBandeja.search($(this).val()).draw();
    });

    $("#datatable-bandeja tbody").on("click", "tr", function () {
        var row = dtBandeja.row(this).data();

        idSolicitud = row.fiIDSolicitud;
        identidadCliente = row.fcIdentidadCliente;
        $("#lblCliente").text(row.fcPrimerNombreCliente + ' ' + row.fcSegundoNombreCliente + ' ' + row.fcPrimerApellidoCliente + ' ' + row.fcSegundoApellidoCliente + ' ');
        $("#lblIdentidadCliente").text(row.fcIdentidadCliente);
        $("#modalAbrirSolicitud").modal();
    });
});

$(document).on('click', 'button#btnActualizar', function () {
    $("#modalActualizarSolicitud").modal();
});

$('#btnActualizar').click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Ingresadas.aspx/EncriptarParametros",
        data: JSON.stringify({ dataCrypt: window.location.href, IDSOL: idSolicitud, Identidad: identidadCliente }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la solicitud, contacte al administrador');
        },
        success: function (data) {

            if (data.d != "-1") {
                window.location = "SolicitudesCredito_ActualizarSolicitud.aspx?" + data.d;
            }
            else {
                MensajeError('No se pudo al redireccionar a pantalla de actualización');
            }
        }
    });

});

$(document).on('click', 'button#btnDetalles', function () {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Ingresadas.aspx/EncriptarParametros",
        data: JSON.stringify({ dataCrypt: window.location.href, IDSOL: idSolicitud, Identidad: identidadCliente }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la solicitud, contacte al administrador');
        },
        success: function (data) {

            if (data.d != "-1") {
                window.location = "SolicitudesCredito_RegistradaDetalles.aspx?" + data.d;
            }
            else {
                MensajeError('No se pudo al redireccionar a pantalla de detalles');
            }
        }
    });
});

jQuery('#date-range').datepicker({
    toggleActive: true
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}