/* Variables generales */
var idSolicitudGPS = 0;
var idSolicitudCredito = 0;
var idGarantia = 0;
var identidad = '';
var nombreCliente = '';

/* Para realizar filtros en ambas listas*/
var filtroActual = '';
var tabActivo = 'tab_Listado_SolicitudesGPS_Pendientes';

$(document).ready(function () {

    CargarSolicitudesGPS();

    $('#ddlMes').on('change', function () {

        if (this.value != '') {

            if (tabActivo == 'tab_Listado_SolicitudesGPS_Pendientes')
                dtListado_SolicitudesGPS_Pendientes.columns(3).search('/' + this.value + '/').draw();
            else
                dtListado_SolicitudesGPS_Completadas.columns(3).search('/' + this.value + '/').draw();
        }
        else {
            if (tabActivo == 'tab_Listado_SolicitudesGPS_Pendientes')
                dtListado_SolicitudesGPS_Pendientes.columns(3).search('').draw();
            else
                dtListado_SolicitudesGPS_Completadas.columns(3).search('').draw();
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

    /* Agregar filtros a los datatables */
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

        idSolicitudGPS = row.IdSolicitudGPS;
        idSolicitudCredito = row.IdSolicitudCredito;
        idGarantia = row.IdGarantia;
        nombreCliente = row.NombreCliente;
        identidad = row.Identidad;

        if (idSolicitudGPS != 0) {

            $(".txtNombreCliente").val(nombreCliente);
            $(".txtMarca").val(row.Marca);
            $(".txtModelo").val(row.Modelo);
            $(".txtAnio").val(row.Anio);
            $(".txtRevisionesGarantia").val(row.RevisionesGarantia);
            $(".txtComentarioSolicitudGPS").val(row.ComentarioInstalacion);
        }
    });

    $("#datatable_SolicitudesGPS_Completadas tbody").on("click", "tr", function () {

        var row = dtListado_SolicitudesGPS_Completadas.row(this).data();

        idSolicitudGPS = row.IdSolicitudGPS;
        idSolicitudCredito = row.IdSolicitudCredito;
        idGarantia = row.IdGarantia;
        nombreCliente = row.NombreCliente;
        identidad = row.Identidad;

        if (idSolicitudGPS != 0) {

            $(".txtNombreCliente").val(nombreCliente);
            $(".txtMarca").val(row.Marca);
            $(".txtModelo").val(row.Modelo);
            $(".txtAnio").val(row.Anio);
            $(".txtRevisionesGarantia").val(row.RevisionesGarantia);
            $(".txtComentarioSolicitudGPS").val(row.ComentarioInstalacion);
        }
    });

    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    });
});

$(document).on('click', 'button#btnCompletarSolicitud', function () {
    $("#modalCompletarSolicitudGPS").modal();
});

$("#btnCompletarSolicitud_Confirmar").click(function (e) {
    RedirigirAccion('SolicitudesGPS_RevisionGarantia.aspx', 'Completar revisión de garantía');
});


$(document).on('click', 'button#btnInstalarGPS', function () {
    $("#modalInstalarGPS").modal();
});

$("#btnInstalarGPS_Confirmar").click(function (e) {

    RedirigirAccion('SolicitudesGPS_RegistroInstalacionGPS.aspx', 'registro de instalación de GPS');
});


$(document).on('click', 'button#btnDetallesInstalacionGPS', function () {
    $("#modalDetallesInstalacionGPS").modal();
});

$("#btnDetallesInstalacionGPS_Confirmar").click(function (e) {

    RedirigirAccion('SolicitudesGPS_DetallesInstalacionGPS.aspx', 'detalles de la instalación de GPS');
});

function RedirigirAccion(nombreFormulario, accion) {

    $.ajax({
        type: "POST",
        url: "SolicitudesGPS_Listado.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitudCredito: idSolicitudCredito, idGarantia: idGarantia, idSolicitudGPS: idSolicitudGPS, dataCrypt: window.location.href }),
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
                        { defaultContent: '' }, // boton + de expandir detalles
                        {
                            "data": "IdGarantia", "className": "text-center", // boton de acciones
                            "render": function (data, type, row) {

                                return '<div class="dropdown mo-mb-0">' +
                                    '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                                    '<i class="fa fa-bars"></i>' +
                                    '</button >' +
                                    '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                                    '<button id="btnCompletarSolicitud" data-id="' + row["IdSolicitudGPS"] + '" type="button" class="dropdown-item"><i class="fa fa-tasks"></i> ' + (row["CantidadRevisionesCompletadas"] == row["CantidadRevisiones"] ? 'Actualizar revisiones' : 'Revisar garantía') + '</button>' +
                                    (row["CantidadRevisionesCompletadas"] == row["CantidadRevisiones"] && row["IdEstadoInstalacion"] != 3 ? '<button id="btnInstalarGPS" data-id="' + row["IdSolicitudGPS"] + '" type="button" class="dropdown-item"><i class="fas fa-map-marker-alt"></i> Instalar GPS</button>' : '') +
                                    (row["IdEstadoInstalacion"] == 3 ? '<button id="btnDetallesInstalacionGPS" data-id="' + row["IdSolicitudGPS"] + '" type="button" class="dropdown-item"><i class="fas fa-map-marker-alt"></i> Detalles instalación GPS</button>' : '') +
                                    '</div>' +
                                    '</div >';
                            }
                        },
                        {
                            "data": "Marca", // Garantia
                            "render": function (data, type, row) {
                                return row["Marca"] + ' ' + row["Modelo"] + ' <small>' + row["Anio"] + '</small> <span class="badge badge-info p-1">' + row["RevisionesGarantia"] + '</span>';
                            }
                        },
                        { "data": "RevisionesGarantia", "className": "text-center", }, // revisiones
                        { "data": "NombreCliente" },
                        { "data": "UsuarioCreador" },
                        {
                            "data": "FechaCreacion",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                            }
                        },
                        {
                            "data": "FechaInstalacion",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                            }
                        },
                        {
                            "data": "EstadoSolicitudGPS", "className": "text-center",
                            "render": function (data, type, row) {
                                return '<span class="badge badge-' + row["EstadoSolicitudGPSClassName"] + ' p-1">' + row["EstadoSolicitudGPS"] + '</span>';
                            }
                        },
                    ],
                    columnDefs: [
                        { targets: [0, 1, 8], orderable: false, "width": "0%" }
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
                        { defaultContent: '' }, // boton + de expandir detalles
                        {
                            "data": "IdGarantia", "className": "text-center", // boton de acciones
                            "render": function (data, type, row) {

                                return '<div class="dropdown mo-mb-0">' +
                                    '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                                    '<i class="fa fa-bars"></i>' +
                                    '</button >' +
                                    '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                                    '<button id="btnCompletarSolicitud" data-id="' + row["IdSolicitudGPS"] + '" type="button" class="dropdown-item"><i class="fa fa-tasks"></i> ' + (row["CantidadRevisionesCompletadas"] == row["CantidadRevisiones"] ? 'Actualizar revisiones' : 'Revisar garantía') + '</button>' +
                                    (row["CantidadRevisionesCompletadas"] == row["CantidadRevisiones"] && row["IdEstadoInstalacion"] != 3 ? '<button id="btnInstalarGPS" data-id="' + row["IdSolicitudGPS"] + '" type="button" class="dropdown-item"><i class="fas fa-map-marker-alt"></i> Instalar GPS</button>' : '') +
                                    (row["IdEstadoInstalacion"] == 3 ? '<button id="btnDetallesInstalacionGPS" data-id="' + row["IdSolicitudGPS"] + '" type="button" class="dropdown-item"><i class="fas fa-map-marker-alt"></i> Detalles instalación GPS</button>' : '') +
                                    '</div>' +
                                    '</div >';
                            }
                        },
                        {
                            "data": "Marca", // Garantia
                            "render": function (data, type, row) {
                                return row["Marca"] + ' ' + row["Modelo"] + ' <small>' + row["Anio"] + '</small> <span class="badge badge-info p-1">' + row["RevisionesGarantia"] + '</span>';
                            }
                        },
                        { "data": "RevisionesGarantia", "className": "text-center", }, // revisiones
                        { "data": "NombreCliente" },
                        { "data": "UsuarioCreador" },
                        {
                            "data": "FechaCreacion",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                            }
                        },
                        {
                            "data": "FechaInstalacion",
                            "render": function (value) {
                                if (value === null) return "";
                                return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                            }
                        },
                        {
                            "data": "EstadoSolicitudGPS", "className": "text-center",
                            "render": function (data, type, row) {
                                return '<span class="badge badge-' + row["EstadoSolicitudGPSClassName"] + ' p-1">' + row["EstadoSolicitudGPS"] + '</span>';
                            }
                        },
                    ],
                    columnDefs: [
                        { targets: [0, 1, 8], orderable: false, "width": "0%" }
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