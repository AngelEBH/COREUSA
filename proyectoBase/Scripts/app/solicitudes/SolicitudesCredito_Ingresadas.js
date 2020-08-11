$(document).ready(function () {

    recargarDatatable();

    //obtener id de la solicitud clickada
    $('#datatable-bandeja tbody').on('click', 'tr', function () {
        var $this = $(this);

        var nombreCliente = tableSolicitudes.row(this).data()[1].replace('<small>', '').replace('</small>', '');
        var identidadCliente = tableSolicitudes.row(this).data()[0].replace('<small>', '').replace('</small>', '');

        //obetener la informacion del registro a inactivar
        idSolicitud_abrir = $this.closest('tr').data('id')
        $("#lblCliente").text(nombreCliente);
        $("#lblIdentidadCliente").text(identidadCliente);
        //abrir modal de inactivar
        $("#modalAbrirSolicitud").modal();
    });

    $("#añoIngreso").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });
});

//función cargar información del datatable
function recargarDatatable() {

    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Ingresadas.aspx/CargarSolicitudesIngresadas" + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo carga la información, contacte al administrador');
        },
        success: function (data) {

            // limpiar datatable
            $('#datatable-bandeja').DataTable().clear();

            // agregar rows al datatable
            if (data != null && data.d.length > 0) {

                for (var i = 0; i < data.d.length; i++) {


                    var estadoMasRelevante = '';

                    if (data.d[i].fiEstadoSolicitud == 1) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En recepción</label>';
                    }

                    if (data.d[i].fiEstadoSolicitud == 2) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En análisis</label>';
                    }

                    if (data.d[i].fdReprogramadoInicio != '/Date(-62135575200000)/' && data.d[i].fdReprogramadoFin == '/Date(-62135575200000)/') {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">Reprogramada</label>';
                    }

                    if (data.d[i].fiEstadoDeCampo == 1) {
                        estadoMasRelevante = '<label class="btn btn-sm btn-block btn-warning mb-0">En campo</label>';
                    }

                    if (data.d[i].fdCondicionadoInicio != '/Date(-62135575200000)/' && data.d[i].fdCondificionadoFin == '/Date(-62135575200000)/') {
                        estadoMasRelevante = '<button id="btnActualizar" data-id="' + data.d[i].fiIDSolicitud + '" class="btn btn-sm btn-block btn-warning mb-0">Condicionada</button>';
                    }
                    if (data.d[i].fiEstadoSolicitud == 4 || data.d[i].fiEstadoSolicitud == 5 || data.d[i].fiEstadoSolicitud == 7) {
                        estadoMasRelevante = data.d[i].fiEstadoSolicitud == 7 ? '<label class="btn btn-sm btn-block btn-success mb-0">Aprobada</label>' : '<label class="btn btn-sm btn-block btn-danger mb-0">Rechazada</label>';
                    }

                    var nombreCliente = data.d[i].fcPrimerNombreCliente + ' ' + data.d[i].fcSegundoNombreCliente + ' ' + data.d[i].fcPrimerApellidoCliente + ' ' + data.d[i].fcSegundoApellidoCliente;

                    var fechaIngreso = FechaFormatoConGuiones(data.d[i].fdFechaCreacionSolicitud);

                    var btnDetalles = '<button id="btnDetalles" data-id="' + data.d[i].fiIDSolicitud + '" class="btn btn-sm btn-block btn-info mb-0">Detalles</button>';

                    tableSolicitudes.row.add($('<tr data-id=' + data.d[i].fiIDSolicitud + '>' +
                        '<td>' + data.d[i].fcIdentidadCliente + '</td >' +
                        '<td>' + nombreCliente + '</td>' +
                        '<td>' + fechaIngreso.split(' ')[0] + '</td>' +
                        '<td>' + data.d[i].fcDescripcion + '</td>' +
                        '<td>' + btnDetalles + '</td>' +
                        '<td class="text-center">' + estadoMasRelevante + '</td>' +
                        '</tr>')[0]).draw(false);
                }
            }
        }
    });
}

//abrir modal de actualizar solicitud condicionada
var idSolicitudActualizar = 0;
$(document).on('click', 'button#btnActualizar', function () {

    idSolicitudActualizar = $(this).data('id');
    $("#modalActualizarSolicitud").modal();
});

//abrir pantalla de actualizar informacion de la solicitud
$('#btnActualizar').click(function (e) {

    var qString = "?" + window.location.href.split("?")[1];

    var IDSOL = idSolicitudActualizar;

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Ingresadas.aspx/EncriptarParametros" + qString,
        data: JSON.stringify({ IDSOL: IDSOL }),
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

    var qString = "?" + window.location.href.split("?")[1];

    var IDSOL = $(this).data('id');

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Ingresadas.aspx/EncriptarParametros" + qString,
        data: JSON.stringify({ IDSOL: IDSOL }),
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

var tableSolicitudes = $('#datatable-bandeja').DataTable({
    responsive: true,
    lengthChange: false,
    pageLength: 50,
    "ordering": false,
    searching: true,
    dom: "rt<'row'<'col-sm-4'i><'col-sm-8'p>>",
    "language": {
        "sProcessing": "Procesando...",
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
        "sLoadingRecords": "Cargando...",
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
    }
});

/* FILTROS */

// busqueda por nombre del cliente
$('#nombreCliente').on('keyup', function () {
    tableSolicitudes.columns(1)
        .search(this.value)
        .draw();
});

// busqueda por identidad del cliente
$('#identidadCliente').on('keyup', function () {
    tableSolicitudes.columns(0)
        .search(this.value)
        .draw();
});

// busqueda por mes de ingreso
$('#mesIngreso').on('change', function () {
    if (this.value != '') {
        tableSolicitudes.columns(2)
            .search('/' + this.value + '/')
            .draw();
    }
    else {
        tableSolicitudes.columns(2)
            .search('')
            .draw();
    }
});

// busqueda por año de ingreso
$('#añoIngreso').on('change', function () {
    tableSolicitudes.columns(2)
        .search(this.value + '/')
        .draw();
});

$.fn.dataTable.ext.search.push(
    function (settings, data, dataIndex) {
        var min = $('#min').datepicker('getDate');
        var max = $('#max').datepicker('getDate');
        var startDate = new Date(data[2]);
        if (min == 'Invalid Date' && max == 'Invalid Date')
            return true;
        if (min == 'Invalid Date' && startDate <= max)
            return true;
        if (max == 'Invalid Date' && startDate >= min)
            return true;
        if (startDate <= max && startDate >= min)
            return true;
        return false;
    }
);

$('#min').datepicker({ onSelect: function () { tableSolicitudes.draw(); }, changeMonth: true, changeYear: true });
$('#max').datepicker({ onSelect: function () { tableSolicitudes.draw(); }, changeMonth: true, changeYear: true });

$('#min, #max').change(function () {
    tableSolicitudes.draw();
});

jQuery('#date-range').datepicker({
    toggleActive: true
});

function FechaFormatoConGuiones(pFecha) {
    if (!pFecha)
        return "Sin modificaciones";
    var fechaString = pFecha.substr(6, 19);
    var fechaActual = new Date(parseInt(fechaString));
    var mes = pad2(fechaActual.getMonth() + 1);
    var dia = pad2(fechaActual.getDate());
    var anio = fechaActual.getFullYear();
    var hora = pad2(fechaActual.getHours());
    var minutos = pad2(fechaActual.getMinutes());
    var segundos = pad2(fechaActual.getSeconds().toString());
    var FechaFinal = anio + "/" + mes + "/" + dia + " " + hora + ":" + minutos + ":" + segundos;
    return FechaFinal;
}

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function pad2(number) {
    return (number < 10 ? '0' : '') + number
}