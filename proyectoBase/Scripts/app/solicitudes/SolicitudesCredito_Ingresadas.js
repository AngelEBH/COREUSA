var filtroActual = "";
var idSolicitud = 0;
var identidadCliente = "";
estadoMasRelevante = '';

$(document).ready(function () {

    dtBandeja = $('#datatable-bandeja').DataTable({
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
            {
                "data": "IdSolicitud", "className": "text-center",
                "render": function (data, type, row) {

                    return '<div class="dropdown mo-mb-2">' +
                        '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                        '<i class="fa fa-bars"></i>' +
                        '</button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<button type="button" class="dropdown-item" id="btnDetalles" data-id="' + row["IdSolicitud"] + '"><i class="fas fa-tasks"></i> Detalles</button>' +
                        ((row["CondicionadoInicio"] != null && row["CondicionadoFin"] == null && row["IdEstadoSolicitud"] != 4 && row["IdEstadoSolicitud"] != 5 && row["IdEstadoSolicitud"] != 7) ? '<button type="button" class="dropdown-item" id="btnActualizar" data-id="' + row["IdSolicitud"] + '"><i class="far fa-edit"></i> Actualizar condiciones</button>' : '') +
                        '</div>' +
                        '</div >';
                }
            },
            { "data": "IdSolicitud" },
            { "data": "Agencia" },
            { "data": "Producto" },
            { "data": "Identidad" },
            { "data": "NombreCliente" },
            {
                "data": "FechaCreacion",
                "render": function (value) {
                    if (value === '/Date(-62135575200000)/') return "";
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm:ss a');
                }
            },
            {
                "data": "IdEstadoSolicitud",
                "render": function (data, type, row) {

                    if (row["IdEstadoSolicitud"] == 1) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En recepción</label>';
                    }

                    if (row["IdEstadoSolicitud"] == 2) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En análisis</label>';
                    }

                    if (row["ReprogramadoInicio"] != null && row["ReprogramadoFin"] == null) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">Reprogramada</label>';
                    }

                    if (row["EstadoDeCampo"] == 1) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En campo</label>';
                    }

                    if (row["IdEstadoSolicitud"] == 6) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En Validación</label>';
                    }

                    if (row["CondicionadoInicio"] != null && row["CondicionadoFin"] == null) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">Condicionada</label>';
                    }

                    if (row["IdEstadoSolicitud"] == 4 || row["IdEstadoSolicitud"] == 5 || row["IdEstadoSolicitud"] == 7) {
                        estadoMasRelevante = row["IdEstadoSolicitud"] == 7 ? '<label class="btn btn-sm btn-block btn-success mb-0">Aprobada</label>' : '<label class="btn btn-sm btn-block btn-danger mb-0">Rechazada</label>';
                    }

                    return estadoMasRelevante;
                }
            }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    /* Mostrar mensaje de actualización a los usuarios durante los próximos 2 días*/
    if (moment().isBefore('2020-11-05T23:50:00-06:00')) {

        iziToast.info({
            title: 'Info',
            message: 'Estimado usuario, ahora las opciones "Detalles" y "Actualizar condiciones" se encuentran en la primera columna con titulo "Acciones".'
        });
    }

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            dtBandeja.columns(6).search('/' + this.value + '/').draw();
        }
        else {
            dtBandeja.columns(6).search('').draw();
        }
    });

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        dtBandeja.columns(6).search(this.value + '/').draw();
    });

    $("#min").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#max").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#min, #max").change(function () {
        filtroActual = 'rangoFechas';
        dtBandeja.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        if (filtroActual == 'rangoFechas') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[6]);
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

        idSolicitud = row.IdSolicitud;
        identidadCliente = row.IdCliente;
        $("#lblCliente").text(row.NombreCliente);
        $("#lblIdentidadCliente").text(row.IdCliente);
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
        data: JSON.stringify({ dataCrypt: window.location.href, idSolicitud: idSolicitud, identidad: identidadCliente }),
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
        data: JSON.stringify({ dataCrypt: window.location.href, idSolicitud: idSolicitud, identidad: identidadCliente }),
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