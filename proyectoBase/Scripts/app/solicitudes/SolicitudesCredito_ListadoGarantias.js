/* Variables generales que se utilizan en casi todas las acciones */
var identidad = ''; /* Identidad del cliente seleccionado del listado de garantias de solicitudes aprobadas */
var idSolicitud = 0; /* No. solicitud seleccionado de cualquiera de las listas */
var idGarantia = 0; /* Id garantia seleccionado de cualquiera de las listas */
var marcaGarantia = 0; /* Id garantia seleccionado de cualquiera de las listas */
var modeloGarantia = 0; /* Id garantia seleccionado de cualquiera de las listas */
var anioGarantia = 0; /* Id garantia seleccionado de cualquiera de las listas */
var idSolicitudInstalacionGPS = 0; /* Id de la solicitud de instalacion de gps para garantia registrada de una solicitud aprobada */
var VIN = ''; /* VIN seleccionado del listado de garantias de solicitudes aprobadas */
var nombreCliente = ''; /* Nombre del cliente seleccionado del listado de garantias de solicitudes aprobadas */

/* Iconos de estado para el listado de garantias de solicitudes aprobadas */
var iconoExito = '<i class="mdi mdi-check-circle mdi-24px p-0 text-success"><label style="display:none;">estadoListo</label></i>';
var iconoPendiente = '<i class="mdi mdi-check-circle mdi-24px p-0 text-secondary"><label style="display:none;">estadoPendiente</label></i>';
var iconoWarning = '<i class="mdi mdi-check-circle mdi-24px p-0 text-warning"><label style="display:none;">estadoPendiente</label></i>';

/* Para realizar filtros en ambas listas*/
var filtroActual = '';
var tabActivo = 'tab_Listado_Solicitudes_Garantias';

$(document).ready(function () {

    /* Inicializar datatable del listado de garantias de solicitudes aprobadas */
    dtListado = $('#datatable-listado').DataTable({
        "pageLength": 15,
        "aaSorting": [],
        "dom": "<'row'<'col-sm-12'B>>" +
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
            {
                "data": "IdGarantia", "className": "text-center",
                "render": function (data, type, row) {

                    return '<div class="dropdown mo-mb-2">' +
                        '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                        '<i class="fa fa-bars"></i>' +
                        '</button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        (row["IdGarantia"] == 0 ? '<button type="button" class="dropdown-item" id="btnGuardar"><i class="fas fa-plus"></i> Agregar</button>' : '') +
                        (row["IdGarantia"] == 0 ? '' : '<button type="button" class="dropdown-item" id="btnDetalles"><i class="fas fa-tasks"></i> Detalles</button>') +
                        (row["IdGarantia"] == 0 ? '' : '<button type="button" class="dropdown-item" id="btnActualizar"><i class="far fa-edit"></i> ' + (row["VIN"] != '' ? 'Actualizar' : 'Completar información') + '</button>') +
                        ((row["IdGarantia"] != 0 && row["VIN"] != '') ? '<button type="button" class="dropdown-item" id="btnImprimirDocumentacion"><i class="far fa-file-alt"></i> Imprimir Doc.</button>' : '') +
                        ((row["IdGarantia"] != 0 && row["VIN"] != '' && row["IdAutoGPSInstalacion"] == 0) ? '<button type="button" class="dropdown-item" id="btnSolicitarGPS"><i class="fas fa-map-marker-alt"></i> Solicitar GPS</button>' : '') +
                        ((row["IdAutoGPSInstalacion"] != 0) ? '<button type="button" class="dropdown-item" id="btnDetalleSolicitudGPS"><i class="fas fa-map-marker-alt"></i> Detalles solicitud GPS</button>' : '') +
                        '</div>' +
                        '</div >';
                }
            },
            {
                "data": "IdSolicitud",
                "render": function (data, type, row) {
                    return row["IdSolicitud"] + '<br><span class="text-muted">' + moment(row["FechaCreacion"]).locale('es').format('YYYY/MM/DD hh:mm a') + '</span>';
                }
            },
            {
                "data": "Agencia",
                "render": function (data, type, row) {
                    return row["Producto"] + '<br/><span class="text-muted">' + row["Agencia"] + ' | ' + row["UsuarioAsignado"] + '</span>'
                }
            },
            {
                "data": "PrimerNombre",
                "render": function (data, type, row) {
                    return row["PrimerNombre"] + ' ' + row["SegundoNombre"] + ' ' + row["PrimerApellido"] + ' ' + row["SegundoApellido"] + '<br/><span class="text-muted">' + row["Identidad"] + "</span>" + (row["IdCanal"] == 3 ? ' <span class="btn btn-sm btn-info pt-0 pb-0 m-0">canex</span>' : '')
                }
            },
            {
                "data": "VIN",
                "render": function (data, type, row) {
                    return row["Marca"] + ' ' + row["Modelo"] + ' ' + row["Anio"] + '<br/><span class="text-muted">' + 'VIN: ' + (row["VIN"] != '' ? row["VIN"] : '') + '<span>'
                }
            },
            { "data": "DocumentosSubidos", "className": "text-center" },
            {
                "data": "IdGarantia", "className": "text-center",
                "render": function (data, type, row) {
                    return (row["IdGarantia"] != 0 ? row["VIN"] != '' ? iconoExito : iconoWarning : iconoPendiente);
                }
            },
            {
                "data": "IdEstadoRevisionFisicaGarantia", "className": "text-center",
                "render": function (data, type, row) {
                    return '<i class="mdi mdi-check-circle mdi-24px p-0 text-' + ObtenerRevisionFisicaClassName(row["IdEstadoRevisionFisicaGarantia"]) + '" onclick="MostrarRevisionGarantia(' + row["IdGarantia"] + ')"><label style="display:none;">' + ObtenerRevisionFisicaDescripcionEstado(row["IdEstadoRevisionFisicaGarantia"]) + '</label></i>';
                }
            },
            {
                "data": "EstadoSolicitudGPS", "className": "text-center",
                "render": function (data, type, row) {
                    return '<span class="badge badge-' + row["EstadoSolicitudGPSClassName"] + ' p-1">' + row["EstadoSolicitudGPS"] + '</span>';
                }
            }
        ],
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Solicitudes_de_credito_' + moment(),
                autoFilter: true,
                messageTop: 'Garantías de solicitudes de crédito' + moment().format('YYYY/MM/DD'),
                exportOptions: {
                    columns: 'report-data'
                }
            },
            {
                extend: 'colvis',
                text: 'Ocultar columnas'
            },
            {
                extend: 'print',
                text: 'Imprimir',
                autoFilter: true,
                exportOptions: {
                    columns: 'report-data'
                }
            }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false },
            { "width": "1%", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8] }
        ]
    });

    /* Inicializar datable del listado de garantias sin solicitud */
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
            {
                "data": "IdGarantia", "className": "text-center",
                "render": function (value) {

                    return '<div class="dropdown mo-mb-2">' +
                        '<button class="btn pt-1 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                        '<i class="fa fa-bars"></i>' +
                        '</button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<button type="button" class="dropdown-item" id="btnDetalles_SinSolicitud"><i class="fas fa-tasks"></i> Detalles</button>' +
                        '<button type="button" class="dropdown-item" id="btnActualizar_SinSolicitud"><i class="far fa-edit"></i> Actualizar</button>' +
                        '</div>' +
                        '</div >';
                }
            },
            { "data": "Agencia" },
            { "data": "Vendedor" },
            { "data": "VIN" },
            { "data": "TipoDeGarantia" },
            { "data": "TipoDeVehiculo" },
            {
                "data": "FechaCreacion",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                }
            }
        ],
        columnDefs: [
            { targets: [0], orderable: false }
        ]
    });

    /* Filtrar cuando se seleccione una opción */
    $("input[type=radio][name=filtro-registro-garantia]").change(function () {

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
                dtListado.columns(7).search('/' + this.value + '/').draw();
            }
            else {
                dtListado_Garantias_SinSolicitud.columns(6).search('/' + this.value + '/').draw();
            }
        }
        else {

            if (tabActivo == 'tab_Listado_Solicitudes_Garantias') {
                dtListado.columns(7).search('').draw();
            }
            else {
                dtListado_Garantias_SinSolicitud.columns(6).search('').draw();
            }
        }
    });

    $('#añoIngreso').on('change', function () {

        if (tabActivo == 'tab_Listado_Solicitudes_Garantias') {
            dtListado.columns(7).search(this.value + '/').draw();
        }
        else {
            dtListado_Garantias_SinSolicitud.columns(6).search(this.value + '/').draw();
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
                FechaIngreso = new Date(a[7]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else if (filtroActual == 'rangoFechas') {

            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[6]);
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
        marcaGarantia = row.Marca;
        modeloGarantia = row.Modelo;
        anioGarantia = row.Anio;

        idSolicitudInstalacionGPS = row.IdAutoGPSInstalacion;
        VIN = row.VIN;
        nombreCliente = row["PrimerNombre"] + ' ' + row["SegundoNombre"] + ' ' + row["PrimerApellido"] + ' ' + row["SegundoApellido"];
        identidad = row["Identidad"]

        $(".txtVIN").val(row.VIN);
        $(".txtMarca").val(row.Marca);
        $(".txtModelo").val(row.Modelo);
        $(".txtAnio").val(row.Anio);
        $(".lblNoSolicitudCredito").text(idSolicitud);

        if (idSolicitudInstalacionGPS != 0) {

            $("#ddlUbicacionInstalacion_Actualizar").val(row.IDAgenciaInstalacion);
            $("#txtComentario_Actualizar").val(row.Comentario_Instalacion);
            $("#txtFechaInstalacion_Actualizar").val(new Date(parseInt(row.FechaInstalacion.substr(6, 19))).toJSON().slice(0, 19));
        }
    });

    $("#datatable-garantiasSinSolicutd tbody").on("click", "tr", function () {

        var row = dtListado_Garantias_SinSolicitud.row(this).data();
        idSolicitud = 0;
        idGarantia = row.IdGarantia;
    });
});

/* Acciones para el listado de garantias de solicitudes aprobadas */
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

/* Acciones para garantias sin solicitud */
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

/* Solicitar instalacion de GPS */
var btnSolicitarGPS = '';
$(document).on('click', 'button#btnSolicitarGPS', function () {

    btnSolicitarGPS = $(this);

    $("#lblIdSolicitudSolicitarGPS").text(idSolicitud);
    $("#txtVIN_SolicitarGPS").val(VIN);
    $("#txtFechaInstalacion").val(new Date());
    $("#txtFechaInstalacion").val(moment().format().slice(0, 19));
    $("#modalSolicitarGPS").modal();
});

$("#btnSolicitarGPS_Confirmar").click(function (e) {

    if ($('#frmPrincipal').parsley().isValid({ group: 'InstalacionGPS_Guardar' })) {

        $("#btnSolicitarGPS_Confirmar").css('disabled', true);

        var solicitudGPS = {
            IdSolicitud: idSolicitud,
            IdGarantia: idGarantia,
            VIN: VIN,
            NombreCliente: nombreCliente,
            IdentidadCliente: identidad,
            Marca: marcaGarantia,
            Modelo: modeloGarantia,
            Anio: anioGarantia,
            FechaInstalacion: $("#txtFechaInstalacion").val(),
            IDAgenciaInstalacion: $("#ddlUbicacionInstalacion :selected").val(),
            AgenciaInstalacion: $("#ddlUbicacionInstalacion :selected").text(),
            Comentario_Instalacion: $("#txtComentario").val().trim(),
            NombreUsuario: NOMBRE_USUARIO,
            CorreoUsuario: CORREO_USUARIO
        }

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ListadoGarantias.aspx/GuardarSolicitudGPS",
            data: JSON.stringify({ solicitudGPS: solicitudGPS, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo guardar la solicitud de GPS, contacte al administrador.');
                $("#btnSolicitarGPS_Confirmar").css('disabled', false);
            },
            success: function (data) {

                $("#modalSolicitarGPS").modal('hide');

                data.d == true ? MensajeExito('La solicitud se guardó correctamente') : MensajeError('No se pudo guardar la solicitud de GPS, contacte al administrador.');

                $("#txtFechaInstalacion,#ddlUbicacionInstalacion,#txtComentario").val('');

                $(btnSolicitarGPS).replaceWith('<button type="button" class="dropdown-item" id="btnDetalleSolicitudGPS"><i class="fas fa-map-marker-alt"></i> Detalles solicitud GPS</button>');

                $("#btnSolicitarGPS_Confirmar").css('disabled', false);
            }
        });
    }
    else
        $('#frmPrincipal').parsley().validate({ group: 'InstalacionGPS_Guardar', force: true });
});

/* Actualizar solicitud de GPS */
var idSolicitudGPS_Actualizar = 0;
$(document).on('click', 'button#btnDetalleSolicitudGPS', function () {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/CargarInformacionSolicitudGPS",
        data: JSON.stringify({ idSolicitud: idSolicitud, idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información de la solicitud de GPS, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                var solicitudGPS = data.d;

                idSolicitudGPS_Actualizar = solicitudGPS.IdAutoGPSInstalacion;
                $("#lblEstadoSolicitudGPS_Actualizar,#lblEstadoSolicitudGPS_Detalle").text(solicitudGPS.EstadoInstalacionGPS);
                $("#lblEstadoSolicitudGPS_Actualizar,#lblEstadoSolicitudGPS_Detalle").removeClass('btn-info').removeClass('btn-warning').removeClass('btn-success').addClass('btn-' + solicitudGPS.EstadoInstalacionClassName);
                $("#ddlUbicacionInstalacion_Actualizar, #ddlUbicacionInstalacion_Detalle").val(solicitudGPS.IDAgenciaInstalacion);
                $("#txtComentario_Actualizar,#txtComentario_Detalle").val(solicitudGPS.Comentario_Instalacion);
                $("#txtFechaInstalacion_Actualizar,#txtFechaInstalacion_Detalle").val(moment(new Date(parseInt(solicitudGPS.FechaInstalacion.substr(6, 19)))).format().slice(0, 19));
                $("#lblIdSolicitudActualizarSolicitudGPS, #lblIdSolicitudDetalleSolicitudGPS").text(solicitudGPS.IdSolicitud);
                $("#txtVIN_SolicitarGPS_Actualizar,#txtVIN_SolicitarGPS_Detalle").val(solicitudGPS.VIN);

                if (solicitudGPS.IdEstadoInstalacion != 1) {
                    $("#btnActualizarSolicitudGPS").prop('disabled', true).removeClass('btn-primary').addClass('btn-secondary').prop('title', 'Esta solicitud de GPS ya no puede ser actualizada debido a su estado').addClass('disabled');
                }
                else {
                    $("#btnActualizarSolicitudGPS").prop('disabled', false).removeClass('btn-secondary').addClass('btn-primary').prop('title', 'Actualizar información de esta solicitud de GPS').removeClass('disabled');
                }

                $("#modalDetalleSolicitudGPS").modal();
            }
            else
                MensajeError('No se pudo cargar la información de la solicitud de GPS, contacte al administrador.');
        }
    });
});

$("#btnActualizarSolicitudGPS").click(function (e) {

    $("#modalDetalleSolicitudGPS").modal('hide');
    $("#modalActualizarSolicitudGPS").modal();
});

$("#btnActualizarSolicitudGPS_Confirmar").click(function (e) {

    if ($('#frmPrincipal').parsley().isValid({ group: 'InstalacionGPS_Actualizar' })) {

        var solicitudGPS = {
            IdAutoGPSInstalacion: idSolicitudGPS_Actualizar,
            IdSolicitud: idSolicitud,
            IdGarantia: idGarantia,
            NombreCliente: nombreCliente,
            IdentidadCliente: identidad,
            Marca: marcaGarantia,
            Modelo: modeloGarantia,
            Anio: anioGarantia,
            VIN: VIN,
            FechaInstalacion: $("#txtFechaInstalacion_Actualizar").val(),
            IDAgenciaInstalacion: $("#ddlUbicacionInstalacion_Actualizar :selected").val(),
            AgenciaInstalacion: $("#ddlUbicacionInstalacion_Actualizar :selected").text(),
            Comentario_Instalacion: $("#txtComentario_Actualizar").val().trim(),
            NombreUsuario: NOMBRE_USUARIO,
            CorreoUsuario: CORREO_USUARIO
        }

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ListadoGarantias.aspx/ActualizarSolicitudGPS",
            data: JSON.stringify({ solicitudGPS: solicitudGPS, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo actualizar la solicitud de GPS, contacte al administrador.');
            },
            success: function (data) {

                $("#modalActualizarSolicitudGPS").modal('hide');

                data.d == true ? MensajeExito('La solicitud de GPS se actualizó correctamente') : MensajeError('No se pudo actualizar la solicitud de GPS, contacte al administrador.');

                $("#txtFechaInstalacion_Actualizar,#ddlUbicacionInstalacion_Actualizar,#txtComentario_Actualizar").val('');
            }
        });
    }
    else
        $('#frmPrincipal').parsley().validate({ group: 'InstalacionGPS_Actualizar', force: true });
});

/* Otros */
$("#tab_Listado_Solicitudes_Garantias_link").on("click", function () {
    tabActivo = 'tab_Listado_Solicitudes_Garantias';
});

$("#tab_Listado_Garantias_SinSolicitud_link").on("click", function () {
    tabActivo = 'tab_Listado_Garantias_SinSolicitud';
});

jQuery("#date-range").datepicker({
    toggleActive: !0
});

function MostrarRevisionGarantia(idGarantia) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/CargarRevisionesGarantia",
        data: JSON.stringify({ idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar las revisiones de la garantía, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                var revisionesGarantia = data.d;

                $("#modalRevisionesGarantia").modal();
            }
            else
                MensajeError('No se pudo cargar las revisiones de la garantía, contacte al administrador.');
        }
    });
}



function FormatoDetalleRevision(revision) {


    var acordion = '<div class="bg-light mb-1">' +
    '<div class="p-3" id="headingOne"> '+
    '<div class="row justify-content-between">' +
    '<div class="col-auto">' +
        '<a href="#' + revision.NombreRevision + '" class="text-dark collapsed" data-toggle="collapse" aria-expanded="false" aria-controls="' + revision.NombreRevision+'">' +
        '<h6 class="m-0 font-14 font-weight-bold">' + revision.NombreRevision +'</h6>' +
    '</a>' +
    '</div>' +
    '<div class="col-auto">' +
        '<div class="badge badge-' + revision.EstadoRevisionClassName +' p-2 float-right">' + revision.EstadoRevision + '</div>' +
    '</div>' +
    '</div>' +
    '</div>' +
        '<div id="' + revision.NombreRevision + '" class="collapse" aria-labelledby="' + revision.NombreRevision +'" data-parent="#accordion-revisiones" style="">' +
    '<div class="card-body p-0">' +
    '<div class="form-group row">' +
    '<div class="col-sm-12">' +
    '<label class="col-form-label">Descripción de la revisión</label>' +
        '<textarea class="form-control form-control-sm" type="text" readonly="readonly" required="required">' + revision.DescripcionRevision+'</textarea>' +
    '</div>' +
    '<div class="col-sm-6">' +
    '<label class="col-form-label">Usuario validador</label>' +
        '<input class="form-control form-control-sm" type="text" readonly="readonly" value="' + revision.UsuarioValidador+'" required="required"/>' +
    '</div>' +
    '<div class="col-sm-6">' +
    '<label class="col-form-label">Fecha validación</label>' +
    '<input class="form-control form-control-sm" type="text" readonly="readonly" value="'+revision.UsuarioValidador+'" required="required"/>' +
    '</div>' +
    '<div class="col-sm-12">' +
    '<label class="col-form-label">Comentarios de la revisión</label>' +
        '<textarea class="form-control form-control-sm" type="text" readonly="readonly" required="required">' + revision.Observaciones+'</textarea>' +
    '</div>' +
    '</div>' +
                                    '</div>'+
                                '</div>'+
                            '</div>';


    debugger;
    return 'Descripción: ' + revision.DescripcionRevision + '<br>' +
        'Usuario validador: ' + revision.UsuarioValidador + '<br>' +
        'Comentarios/Observaciones: ' + revision.Observaciones + '<br>' +
        'Fecha de validación: ' + (revision.IdEstadoRevision != 0 ? moment(revision.IdEstadoRevision).locale('es').format('YYYY/MM/DD hh:mm a') : 'PENDIENTE');
}

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

function ObtenerRevisionFisicaClassName(idEstadoRevisionFisica) {
    return idEstadoRevisionFisica == 0 ? 'secondary' : idEstadoRevisionFisica == 1 ? 'success' : idEstadoRevisionFisica == 2 ? 'danger' : 'secondary';
}

function ObtenerRevisionFisicaDescripcionEstado(idEstadoRevisionFisica) {
    return idEstadoRevisionFisica == 0 ? 'revision pediente' : idEstadoRevisionFisica == 1 ? 'revision aprobada' : idEstadoRevisionFisica == 2 ? 'revision rechazada' : 'revision pendiente';
}

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

$('.table-responsive').on('show.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "inherit");
});

$('.table-responsive').on('hide.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "auto");
})
