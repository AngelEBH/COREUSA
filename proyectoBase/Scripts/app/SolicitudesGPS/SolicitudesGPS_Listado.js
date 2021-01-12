/* Variables generales */
var idSolicitudInstalacionGPS = 0; /* Id de la solicitud de instalacion de gps para garantia registrada de una solicitud aprobada */
var idSolicitudCredito = 0; /* No. solicitud seleccionado de cualquiera de las listas */
var identidad = ''; /* Identidad del cliente seleccionado del listado */
var nombreCliente = ''; /* Nombre del cliente seleccionado del listado */

/* Iconos de estado para el listado */
var iconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">estadoListo</label></i>';
var iconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">estadoPendiente</label></i>';
var iconoWarning = '<i class="mdi mdi-check-circle mdi-24px text-warning p-0"><label style="display:none;">estadoPendiente</label></i>';

/* Para realizar filtros en ambas listas*/
var filtroActual = '';
var tabActivo = 'tab_Listado_SolicitudesGPS_Pendientes';

$(document).ready(function () {

    CargarSolicitudesGPS();

    $('#ddlMes').on('change', function () {

        if (this.value != '') {

            if (tabActivo == 'tab_Listado_SolicitudesGPS_Pendientes') {
                dtListado_SolicitudesGPS_Pendientes.columns(3).search('/' + this.value + '/').draw();
            }
            else {
                dtListado_SolicitudesGPS_Completadas.columns(3).search('/' + this.value + '/').draw();
            }
        }
        else {

            if (tabActivo == 'tab_Listado_SolicitudesGPS_Pendientes') {
                dtListado_SolicitudesGPS_Pendientes.columns(3).search('').draw();
            }
            else {
                dtListado_SolicitudesGPS_Completadas.columns(3).search('').draw();
            }
        }
    });

    $('#añoIngreso').on('change', function () {

        if (tabActivo == 'tab_Listado_SolicitudesGPS_Pendientes')
            dtListado_SolicitudesGPS_Pendientes.columns(3).search(this.value + '/').draw();
        else
            dtListado_SolicitudesGPS_Completadas.columns(3).search(this.value + '/').draw();
    });

    $("#min").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: true,
        changeYear: true,
    });

    $("#max").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: true,
        changeYear: true,
    });

    $("#min, #max").change(function () {
        filtroActual = 'rangoFechas';
        dtListado_SolicitudesGPS_Pendientes.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {

        if (filtroActual == 'rangoFechas' && tabActivo == 'tab_Listado_SolicitudesGPS_Pendientes') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[3]);

            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else if (filtroActual == 'rangoFechas') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[3]);

            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else return true;

    });

    $("#añoIngreso").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {

        if (tabActivo == 'tab_Listado_SolicitudesGPS_Pendientes')
            dtListado_SolicitudesGPS_Pendientes.search($(this).val()).draw();
        else
            dtListado_SolicitudesGPS_Completadas.search($(this).val()).draw();
    });

    $("#datatable_SolicitudesGPS_Pendientes tbody").on("click", "tr", function () {

        var row = dtListado_SolicitudesGPS_Pendientes.row(this).data();

        idSolicitudInstalacionGPS = row.IdSolicitudGPS;
        idSolicitudCredito = row.IdSolicitudCredito;
        nombreCliente = row.NombreCliente;
        identidad = row.Identidad;

        if (idSolicitudInstalacionGPS != 0) {

            $(".lblNombreCliente").val(nombreCliente);
            $(".lblMarca").val(row.Marca);
            $(".lblModelo").val(row.Modelo);
            $(".lblAnio").val(row.Anio);
        }
    });

    $("#datatable_SolicitudesGPS_Completadas tbody").on("click", "tr", function () {

        var row = dtListado_SolicitudesGPS_Completadas.row(this).data();

        idSolicitudInstalacionGPS = row.IdSolicitudGPS;
        idSolicitudCredito = row.IdSolicitudCredito;
        nombreCliente = row.NombreCliente;
        identidad = row.Identidad;

        if (idSolicitudInstalacionGPS != 0) {

            $(".lblNombreCliente").val(nombreCliente);
            $(".lblMarca").val(row.Marca);
            $(".lblModelo").val(row.Modelo);
            $(".lblAnio").val(row.Anio);
        }
    });

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    });
});

$(document).on('click', 'button#btnCompletarSolicitud', function () {
    $("#lblNombreCliente").text(nombreCliente);
    $("#modalCompletarSolicitudGPS").modal();
});

$("#btnCompletarSolicitud_Confirmar").click(function (e) {

    RedirigirAccion('SolicitudesCredito_ImprimirDocumentacion.aspx', 'imprimir documentación de la solicitud');
});

function RedirigirAccion(nombreFormulario, accion) {

    $.ajax({
        type: "POST",
        url: "SolicitudesGPS_Listado.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitudCredito: idSolicitudCredito, idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo redireccionar a " + accion);
        },
        success: function (data) {
            data.d != "-1" ? window.location = nombreFormulario + "?" + data.d : MensajeError("No se pudo redireccionar a" + accion);
        }
    });
}

function CargarSolicitudesGPS() {

    $.ajax({
        type: "POST",
        url: "SolicitudesGPS_Listado.aspx/CargarSolicitudesGPS",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Ocurrió un error al cargar las solicitudes GPS, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                var solicitudesGPS_Pendientes = data.d.SolicitudesGPS_Pendientes;
                var solicitudesGPS_Completadas = data.d.SolicitudesGPS_Completadas;

                dtListado_SolicitudesGPS_Pendientes = $('#datatable_SolicitudesGPS_Pendientes').DataTable({
                    "pageLength": 15,
                    "aaSorting": [],
                    "responsive": true,
                    "dom": "<'row'<'col-sm-12'>>" +
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
                    data: solicitudesGPS_Pendientes,
                    "columns": [
                        { defaultContent: '' },
                        {
                            "data": "IdGarantia", "className": "text-center",
                            "render": function (data, type, row) {

                                return '<div class="dropdown mo-mb-0">' +
                                    '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                                    '<i class="fa fa-bars"></i>' +
                                    '</button >' +
                                    '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                                    '<button id="btnCompletarSolicitud" data-id="' + row["IdSolicitudGPS"] + '" type="button" class="dropdown-item"><i class="far fa-edit"></i> Completar</button>'
                                //(row["IdGarantia"] == 0 ? '<button type="button" class="dropdown-item" id="btnGuardar"><i class="fas fa-plus"></i> Agregar</button>' : '') +
                                '</div>' +
                                    '</div >';
                            }
                        },
                        { "data": "NombreCliente" },
                        {
                            "data": "FechaInstalacion",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                            }
                        },
                        { "data": "UsuarioCreador" },
                        {
                            "data": "FechaCreacion",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                            }
                        },
                        { "data": "Marca" },
                        { "data": "Modelo" },
                        { "data": "Anio", "className": "text-center" },
                        { "data": "AgenciaInstalacion" },
                        { "data": "ComentarioInstalacion" },
                        {
                            "data": "EstadoSolicitudGPS", "className": "text-center",
                            "render": function (data, type, row) {
                                return '<span class="badge badge-' + row["EstadoSolicitudGPSClassName"] + ' p-1">' + row["EstadoSolicitudGPS"] + '</span>';
                            }
                        },
                        {
                            "data": "IdGarantia", "className": "text-center",
                            "render": function (data, type, row) {
                                return (row["IdGarantia"] != 0 ? row["VIN"] != '' ? iconoExito : iconoWarning : iconoPendiente);
                            }
                        }
                    ],
                    columnDefs: [
                        { targets: [0, 1, 11], orderable: false, "width": "0%" }
                    ]
                });

                dtListado_SolicitudesGPS_Completadas = $('#datatable_SolicitudesGPS_Completadas').DataTable({
                    "pageLength": 15,
                    "aaSorting": [],
                    "responsive": true,
                    "dom": "<'row'<'col-sm-12'>>" +
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
                    data: solicitudesGPS_Completadas,
                    "columns": [
                        { defaultContent: '' },
                        {
                            "data": "IdGarantia", "className": "text-center",
                            "render": function (data, type, row) {

                                return '<div class="dropdown mo-mb-0">' +
                                    '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                                    '<i class="fa fa-bars"></i>' +
                                    '</button >' +
                                    '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                                    '<button id="btnCompletarSolicitud" data-id="' + row["IdSolicitudGPS"] + '" type="button" class="dropdown-item"><i class="far fa-edit"></i> Completar</button>'
                                //(row["IdGarantia"] == 0 ? '<button type="button" class="dropdown-item" id="btnGuardar"><i class="fas fa-plus"></i> Agregar</button>' : '') +
                                '</div>' +
                                    '</div >';
                            }
                        },
                        { "data": "NombreCliente" },
                        {
                            "data": "FechaInstalacion",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                            }
                        },
                        { "data": "UsuarioCreador" },
                        {
                            "data": "FechaCreacion",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                            }
                        },
                        { "data": "Marca" },
                        { "data": "Modelo" },
                        { "data": "Anio", "className": "text-center" },
                        { "data": "AgenciaInstalacion" },
                        { "data": "ComentarioInstalacion" },
                        {
                            "data": "EstadoSolicitudGPS", "className": "text-center",
                            "render": function (data, type, row) {
                                return '<span class="badge badge-' + row["EstadoSolicitudGPSClassName"] + ' p-1">' + row["EstadoSolicitudGPS"] + '</span>';
                            }
                        },
                        {
                            "data": "IdGarantia", "className": "text-center",
                            "render": function (data, type, row) {
                                return (row["IdGarantia"] != 0 ? row["VIN"] != '' ? iconoExito : iconoWarning : iconoPendiente);
                            }
                        }
                    ],
                    columnDefs: [
                        { targets: [0, 1, 11], orderable: false, "width": "0%" }
                    ]
                });
            }
            else
                MensajeError('Ocurrió un error al cargar las solicitudes GPS, contacte al administrador.');
        }
    });
}

$("#tab_Listado_SolicitudesGPS_Pendientes").on("click", function () {
    tabActivo = 'tab_Listado_SolicitudesGPS_Pendientes';
});

$("#tab_Listado_SolicitudesGPS_Completadas_Link").on("click", function () {
    tabActivo = 'tab_Listado_SolicitudesGPS_Completadas';
});

jQuery("#date-range").datepicker({
    toggleActive: !0
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function MensajeExito(mensaje) {
    iziToast.success({
        title: 'Éxito',
        message: mensaje
    });
}