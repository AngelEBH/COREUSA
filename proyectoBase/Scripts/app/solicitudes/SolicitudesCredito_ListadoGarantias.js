var iconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">estadoListo</label></i>';
var iconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">estadoPendiente</label></i>';
var identidad = '';
var idSolicitud = 0;
var idGarantia = 0;
var filtroActual = '';
var nombreCliente = '';
var tabActivo = 'tab_Listado_Solicitudes_Garantias';

$(document).ready(function () {

    dtListado = $('#datatable-listado').DataTable({
        "pageLength": 15,
        "aaSorting": [],
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
        "ajax": {
            type: "POST",
            url: "SolicitudesCredito_ListadoGarantias.aspx/CargarListado",
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
            { "data": "IdSolicitud", "className": "text-center" },
            { "data": "Agencia" },
            { "data": "UsuarioAsignado" },
            {
                "data": "Producto",
                "render": function (value) {
                    return value.split(' ')[1]
                }
            },
            { "data": "Identidad" },
            {
                "data": "PrimerNombre",
                "render": function (data, type, row) {
                    return row["PrimerNombre"] + ' ' + row["SegundoNombre"] + ' ' + row["PrimerApellido"] + ' ' + row["SegundoApellido"] + ' ' + (row["IdCanal"] == 3 ? '<span class="btn btn-sm btn-info pt-0 pb-0 m-0">canex</span>' : '')
                }
            },
            {
                "data": "FechaCreacion",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm:ss a');
                }
            },
            {
                "data": "IdGarantia", "className": "text-center",
                "render": function (data, type, row) {

                    return row["IdGarantia"] != 0 ? iconoExito : iconoPendiente;
                }
            },
            {
                "data": "IdGarantia", "className": "text-center",
                "render": function (value) {

                    return '<div class="dropdown mo-mb-2">' +
                        '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                        '<i class="fa fa-bars"></i>' +
                        '</button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        (value == 0 ? '<button type="button" class="dropdown-item" id="btnGuardar"><i class="fas fa-plus"></i> Agregar</button>' : '') +
                        (value == 0 ? '' : '<button type="button" class="dropdown-item" id="btnDetalles"><i class="fas fa-tasks"></i> Detalles</button>') +
                        (value == 0 ? '' : '<button type="button" class="dropdown-item" id="btnActualizar"><i class="far fa-edit"></i> Actualizar</button>') +
                        (value == 0 ? '' : '<button type="button" class="dropdown-item" id="btnImprimirDocumentacion"><i class="far fa-file-alt"></i> Imprimir Doc.</button>') +
                        '</div>' +
                        '</div >';
                }
            }
        ],
        columnDefs: [
            { targets: [7, 8], orderable: false },
            { "width": "1%", "targets": 0 }
        ]
    });

    dtListado_Garantias_SinSolicitud = $('#datatable-garantiasSinSolicutd').DataTable({
        "pageLength": 15,
        "aaSorting": [],
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
        "ajax": {
            type: "POST",
            url: "SolicitudesCredito_ListadoGarantias.aspx/CargarListadoGarantiasSinGarantia",
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
            { "data": "Agencia" },
            { "data": "Vendedor" },
            { "data": "VIN" },
            { "data": "TipoDeGarantia" },
            { "data": "TipoDeVehiculo" },
            {
                "data": "FechaCreacion",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm:ss a');
                }
            },
            {
                "data": "IdGarantia", "className": "text-center",
                "render": function (value) {

                    return '<div class="dropdown mo-mb-2">' +
                        '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                        '<i class="fa fa-bars"></i>' +
                        '</button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<button type="button" class="dropdown-item" id="btnDetalles_SinSolicitud"><i class="fas fa-tasks"></i> Detalles</button>' +
                        '<button type="button" class="dropdown-item" id="btnActualizar_SinSolicitud"><i class="far fa-edit"></i> Actualizar</button>' +
                        '</div>' +
                        '</div >';
                }
            }
        ],
        columnDefs: [
            { targets: [6], orderable: false }
        ]
    });

    /* Filtrar cuando se seleccione una opción */
    $("input[type=radio][name=filtros]").change(function () {

        let filtro = this.value;
        dtListado.columns().search("").draw();

        switch (filtro) {
            case "0":
                dtListado.columns(7).search("").draw();
                break;
            case "1":
                dtListado.columns(7).search("estadoPendiente").draw();
                break;
            case "2":
                dtListado.columns(7).search("estadoListo").draw();
                break;
            default:
                dtListado.columns(7).search("").draw();
        }
    });

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {

        if (this.value != '') {

            if (tabActivo == 'tab_Listado_Solicitudes_Garantias') {
                dtListado.columns(6).search('/' + this.value + '/').draw();
            }
            else {
                dtListado_Garantias_SinSolicitud.columns(5).search('/' + this.value + '/').draw();
            }
        }
        else {

            if (tabActivo == 'tab_Listado_Solicitudes_Garantias') {
                dtListado.columns(6).search('').draw();
            }
            else {
                dtListado_Garantias_SinSolicitud.columns(5).search('').draw();
            }
        }
    });

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {

        if (tabActivo == 'tab_Listado_Solicitudes_Garantias') {
            dtListado.columns(6).search(this.value + '/').draw();
        }
        else {
            dtListado_Garantias_SinSolicitud.columns(5).search(this.value + '/').draw();
        }
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
        dtListado.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        if (filtroActual == 'rangoFechas' && tabActivo == 'tab_Listado_Solicitudes_Garantias') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[6]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else if (filtroActual == 'rangoFechas') {

            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[5]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else {
            return true;
        }
    });

    $("#añoIngreso").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        if (tabActivo == 'tab_Listado_Solicitudes_Garantias') {
            dtListado.search($(this).val()).draw();
        }
        else {
            dtListado_Garantias_SinSolicitud.search($(this).val()).draw();
        }
    });

    $("#datatable-listado tbody").on("click", "tr", function () {

        var row = dtListado.row(this).data();
        idSolicitud = row.IdSolicitud;
        idGarantia = row.IdGarantia;
        nombreCliente = row["PrimerNombre"] + ' ' + row["SegundoNombre"] + ' ' + row["PrimerApellido"] + ' ' + row["SegundoApellido"];
    });

    $("#datatable-garantiasSinSolicutd tbody").on("click", "tr", function () {

        var row = dtListado_Garantias_SinSolicitud.row(this).data();
        idSolicitud = 0;
        idGarantia = row.IdGarantia;
    });
});

$(document).on('click', 'button#btnActualizar', function () {

    $("#lblIdSolicitudActualizar").text(idSolicitud);
    $("#lblNombreClienteActualizar").text(nombreCliente);
    $("#modalActualizarGarantia").modal();
});

$(document).on('click', 'button#btnGuardar', function () {

    $("#lblIdSolicitudGuardar").text(idSolicitud);
    $("#lblNombreClienteGuardar").text(nombreCliente);
    $("#modalGuardarGarantia").modal();
});

$(document).on('click', 'button#btnDetalles', function () {

    $("#lblIdSolicitudDetalles").text(idSolicitud);
    $("#lblNombreClienteDetalles").text(nombreCliente);
    $("#modalDetallesGarantia").modal();
});

$("#btnGuardarGarantia_Confirmar").click(function (e) {

    RedirigirAccion('Garantia_Registrar.aspx', 'registro de la garantía');
});

$("#btnActualizarGarantia_Confirmar").click(function (e) {

    RedirigirAccion('Garantia_Actualizar.aspx', 'actualizar información de la garantía');
});

$("#btnDetallesGarantia_Confirmar").click(function (e) {

    RedirigirAccion('Garantia_Detalles.aspx', 'detalles de la garantía');
});

$("#btnRegistrarGarantiaSinSolicitud").click(function (e) {

    idSolicitud = 0;

    RedirigirAccion('Garantia_Registrar.aspx', 'registro de la garantía sin solicitud');
});

/* Sin solicitud */
$(document).on('click', 'button#btnDetalles_SinSolicitud', function () {

    $("#modalDetallesGarantia_SinSolicitud").modal();
});

$(document).on('click', 'button#btnActualizar_SinSolicitud', function () {

    $("#modalActualizarGarantia_SinSolicitud").modal();
});

$("#btnDetallesGarantia_SinSolicitud_Confirmar").click(function (e) {

    RedirigirAccion('GarantiaSinSolicitud_Detalles.aspx', 'detalles de la garantía');
});

$("#btnActualizarGarantia_SinSolicitud_Confirmar").click(function (e) {

    RedirigirAccion('Garantia_Actualizar.aspx', 'actualizar información de la garantía');
});

/* Imprimir documentación */
$(document).on('click', 'button#btnImprimirDocumentacion', function () {

    $("#lblIdSolicitudImprimirDocumentacion").text(idSolicitud);
    $("#lblNombreClienteImprimirDocumentacion").text(nombreCliente);
    $("#modalImprimirDocumentacion").modal();
});

$("#btnImprimirDocumentacion_Confirmar").click(function (e) {

    RedirigirAccion('SolicitudesCredito_ImprimirDocumentacion.aspx', 'imprimir documentación de la solicitud');
});

function RedirigirAccion(nombreFormulario, accion) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitud: idSolicitud, idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo redireccionar a " + accion);
        },
        success: function (data) {
            data.d != "-1" ? window.location = nombreFormulario + "?" + data.d : MensajeError("No se pudo redireccionar a" + accion);
        }
    });
}

$("#tab_Listado_Solicitudes_Garantias_link").on("click", function () {
    tabActivo = 'tab_Listado_Solicitudes_Garantias';
});

$("#tab_Listado_Garantias_SinSolicitud_link").on("click", function () {
    tabActivo = 'tab_Listado_Garantias_SinSolicitud';
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