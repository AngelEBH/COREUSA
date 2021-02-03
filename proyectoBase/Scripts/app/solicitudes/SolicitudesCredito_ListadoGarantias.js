/* Variables generales que se utilizan en casi todas las acciones */
var identidad = ''; /* Identidad del cliente seleccionado del listado de garantias de solicitudes aprobadas */
var idSolicitud = 0; /* No. solicitud seleccionado de cualquiera de las listas */
var idGarantia = 0; /* Id garantia seleccionado de cualquiera de las listas */
var marcaGarantia = 0; /* Id garantia seleccionado de cualquiera de las listas */
var modeloGarantia = 0; /* Id garantia seleccionado de cualquiera de las listas */
var anioGarantia = 0; /* Id garantia seleccionado de cualquiera de las listas */
var idSolicitudInstalacionGPS = 0; /* Id de la solicitud de instalacion de gps y revision fisica para garantia registrada de una solicitud aprobada */
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

    /* Inicializar datatable principal del listado de garantias de solicitudes */
    dtListado = $('#datatable-principal-garantias').DataTable({
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
                        '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-bars"></i></button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        (row["IdGarantia"] == 0 ? '<button type="button" class="dropdown-item" id="btnGuardar" data-toggle="modal" data-target="#modalGuardarGarantia"><i class="fas fa-plus"></i> Agregar</button>' : '') +
                        (row["IdGarantia"] == 0 ? '' : '<button type="button" class="dropdown-item" id="btnDetalles" data-toggle="modal" data-target="#modalDetallesGarantia"><i class="fas fa-tasks"></i> Detalles</button>') +
                        (row["IdGarantia"] == 0 ? '' : '<button type="button" class="dropdown-item" id="btnActualizar" data-toggle="modal" data-target="#modalActualizarGarantia"><i class="far fa-edit"></i> ' + (row["VIN"] != '' ? 'Actualizar' : 'Completar información') + '</button>') +
                        ((row["IdGarantia"] != 0 && row["VIN"] != '') ? '<button type="button" class="dropdown-item" id="btnImprimirDocumentacion" data-toggle="modal" data-target="#modalImprimirDocumentacion"><i class="far fa-file-alt"></i> Imprimir Doc.</button>' : '') +
                        ((row["IdGarantia"] != 0 && row["VIN"] != '' && row["IdAutoGPSInstalacion"] == 0) ? '<button type="button" class="dropdown-item" id="btnSolicitarGPS"><i class="fas fa-map-marker-alt"></i> Solicitar revisión física/GPS</button>' : '') +
                        ((row["IdAutoGPSInstalacion"] != 0) ? '<button type="button" class="dropdown-item" id="btnDetalleSolicitudGPS"><i class="fas fa-map-marker-alt"></i> Solicitud revisión física/GPS</button>' : '') +
                        '</div>' +
                        '</div >';
                }
            },
            {
                "data": "IdSolicitud",
                "render": function (data, type, row) {
                    return row["IdSolicitud"] + ' <br><span class="text-muted">' + moment(row["FechaCreacion"]).locale('es').format('YYYY/MM/DD hh:mm a') + '</span>';
                }
            },
            {
                "data": "Agencia",
                "render": function (data, type, row) {
                    return row["Producto"] + ' <br/><span class="text-muted">' + row["Agencia"] + ' | ' + row["UsuarioAsignado"] + '</span>'
                }
            },
            {
                "data": "PrimerNombre",
                "render": function (data, type, row) {
                    return row["PrimerNombre"] + ' ' + row["SegundoNombre"] + ' ' + row["PrimerApellido"] + ' ' + row["SegundoApellido"] + ' <br/><span class="text-muted">' + row["Identidad"] + "</span>" + (row["IdCanal"] == 3 ? ' <span class="btn btn-sm btn-info pt-0 pb-0 m-0">canex</span>' : '')
                }
            },
            {
                "data": "VIN",
                "render": function (data, type, row) {
                    return row["Marca"] + ' ' + row["Modelo"] + ' ' + row["Anio"] + ' <br/><span class="text-muted">' + 'VIN: ' + (row["VIN"] != '' ? row["VIN"] : '') + '<span>'
                }
            },
            {
                "data": "DocumentosSubidos", "className": "text-center",
                "render": function (data, type, row) {
                    return '<span class="btn btn-sm btn-secondary" onclick="MostrarDocumentosGarantia(' + row["IdGarantia"] + ')">' + row["DocumentosSubidos"] + ' <i class="fas fa-search"></i></span>'
                }
            },
            {
                "data": "IdGarantia", "className": "text-center",
                "render": function (data, type, row) {
                    return (row["IdGarantia"] != 0 ? (row["VIN"] != '' && row["VIN"].length > 4) ? iconoExito : iconoWarning : iconoPendiente);
                }
            },
            {
                "data": "EstadoRevisionFisica", "className": "text-center",
                "render": function (data, type, row) {
                    return '<i class="mdi mdi-check-circle mdi-24px p-0 text-' + row["EstadoRevisionFisicaClassName"] + '" onclick="MostrarRevisionGarantia(' + row["IdGarantia"] + ')"><label style="display:none;">' + row["EstadoRevisionFisica"] + '</label></i>';
                }
            },
            {
                "data": "EstadoSolicitudGPS", "className": "text-center",
                "render": function (data, type, row) {
                    return '<span class="badge badge-' + row["EstadoSolicitudGPSClassName"] + ' p-1" ' + (row["IdEstadoInstalacion"] == 3 ? 'onclick="MostrarInstalacionGPS(' + row["IdAutoGPSInstalacion"] + ')"' : '') + '>' + row["EstadoSolicitudGPS"] + '</span>';
                }
            }
        ],
        buttons: [
            {
                extend: 'excelHtml5',
                title: 'Garantias_' + moment(),
                autoFilter: true,
                messageTop: 'Garantías de solicitudes de crédito' + moment().format('YYYY/MM/DD'),
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8]
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
            url: "SolicitudesCredito_ListadoGarantias.aspx/CargarListadoGarantiasSinSolicitud",
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
                        '<button class="btn pt-1 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-bars"></i></button>' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<button type="button" class="dropdown-item" id="btnDetalles_SinSolicitud" data-toggle="modal" data-target="#modalDetallesGarantia_SinSolicitud"><i class="fas fa-tasks"></i> Detalles</button>' +
                        '<button type="button" class="dropdown-item" id="btnActualizar_SinSolicitud" data-toggle="modal" data-target="#modalActualizarGarantia_SinSolicitud"><i class="far fa-edit"></i> Actualizar</button>' +
                        '</div>' +
                        '</div >';
                }
            },
            {
                "data": "Vendedor",
                "render": function (data, type, row) {
                    return row["Vendedor"] + ' <br/><span class="text-muted">' + row["Agencia"] + '<span>'
                }
            },
            {
                "data": "VIN",
                "render": function (data, type, row) {
                    return row["Marca"] + ' ' + row["Modelo"] + ' ' + row["Anio"] + ' <br/><span class="text-muted">' + 'VIN: ' + (row["VIN"] != '' ? row["VIN"] : '') + '<span>'
                }
            },
            {
                "data": "TipoDeGarantia",
                "render": function (data, type, row) {
                    return row["TipoDeGarantia"] + ' <br/><span class="text-muted">' + row["TipoDeVehiculo"] + '<span>'
                }
            },
            {
                "data": "FechaCreacion",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                }
            },
            //{
            //    "data": "Comentarios",
            //    "render": function (value) {
            //        return value + value + value
            //    }
            //},
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
                dtListado.columns(6).search("").draw();
                break;
            case "1":
                dtListado.columns(6).search("estadoPendiente").draw();
                break;
            case "2":
                dtListado.columns(6).search("estadoListo").draw();
                break;
            default:
                dtListado.columns(6).search("").draw();
        }
    });

    /* Búsqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {

        if (this.value != '') {

            if (tabActivo == 'tab_Listado_Solicitudes_Garantias') {
                dtListado.columns(1).search('/' + this.value + '/').draw();
            }
            else {
                dtListado_Garantias_SinSolicitud.columns(4).search('/' + this.value + '/').draw();
            }
        }
        else {

            if (tabActivo == 'tab_Listado_Solicitudes_Garantias') {
                dtListado.columns(1).search('').draw();
            }
            else {
                dtListado_Garantias_SinSolicitud.columns(4).search('').draw();
            }
        }
    });

    /* Inicilizar filtro para rango de fechas */
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
        dtListado.draw();
    });

    /* Agregar filtros a los datatables */
    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {

        if (filtroActual == 'rangoFechas' && tabActivo == 'tab_Listado_Solicitudes_Garantias') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(data[1]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else if (filtroActual == 'rangoFechas') {

            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(data[4]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else {
            return true;
        }
    });

    /* Buscador de los datatables */
    $('#txtDatatableFilter').keyup(function () {

        if (tabActivo == 'tab_Listado_Solicitudes_Garantias') {
            dtListado.search($(this).val()).draw();
        }
        else {
            dtListado_Garantias_SinSolicitud.search($(this).val()).draw();
        }
    });

    /* Cuando se haga click en el datatable principal de garantías */
    /* setear las etiquetas de ID solicitud, nombre de cliente, marca, modelo, etc, de todos los modales a través del className */
    $("#datatable-principal-garantias tbody").on("click", "tr", function () {

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

        $(".lblVIN").text(row.VIN);
        $(".lblMarca").text(row.Marca);
        $(".lblModelo").text(row.Modelo);
        $(".lblAnio").text(row.Anio);
        $(".lblColor").text(row.Color);
        $(".lblNoSolicitudCredito").text(idSolicitud);
        $(".lblNombreCliente").text(nombreCliente);
        $("#lblEstadoRevisionFisica").removeClass('badge-success').removeClass('badge-warning').removeClass('badge-danger').addClass('badge-' + row.EstadoRevisionFisicaClassName).text(row.EstadoRevisionFisica);

        if (idSolicitudInstalacionGPS != 0) {

            $("#ddlUbicacionInstalacion_Actualizar").val(row.IDAgenciaInstalacion);
            $("#txtComentario_Actualizar").val(row.Comentario_Instalacion);
            $("#txtFechaInstalacion_Actualizar").val(new Date(parseInt(row.FechaInstalacion.substr(6, 19))).toJSON().slice(0, 19));
        }
    });

    /* Cuando se haga click en el datatable de garantías sin solicitud*/
    $("#datatable-garantiasSinSolicutd tbody").on("click", "tr", function () {

        var row = dtListado_Garantias_SinSolicitud.row(this).data();
        idSolicitud = 0;
        idGarantia = row.IdGarantia;
    });

    /* Cuando se cambie de TAB reajustar el tamaño de los datatables */
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    });
});

$("#btnGuardarGarantia_Confirmar").click(function (e) {

    RedirigirAccion('Garantia_Registrar.aspx', 'registro de la garantía');
});

$("#btnActualizarGarantia_Confirmar").click(function (e) {

    RedirigirAccion('Garantia_Actualizar.aspx', 'actualizar información de la garantía');
});

$("#btnDetallesGarantia_Confirmar").click(function (e) {

    MostrarContenidoEnModalFullScreen('Garantia_Detalles.aspx', 'detalles de la garantía');
});

$("#btnRegistrarGarantiaSinSolicitud").click(function (e) {

    idSolicitud = 0;

    RedirigirAccion('Garantia_Registrar.aspx', 'registro de la garantía sin solicitud');
});

/* Acciones para garantias sin solicitud */
$("#btnDetallesGarantia_SinSolicitud_Confirmar").click(function (e) {

    MostrarContenidoEnModalFullScreen('GarantiaSinSolicitud_Detalles.aspx', 'detalles de la garantía');
});

$("#btnActualizarGarantia_SinSolicitud_Confirmar").click(function (e) {

    RedirigirAccion('Garantia_Actualizar.aspx', 'actualizar información de la garantía');
});

/* Imprimir documentación */
$("#btnImprimirDocumentacion_Confirmar").click(function (e) {

    MostrarContenidoEnModalFullScreen('SolicitudesCredito_ImprimirDocumentacion.aspx', 'imprimir documentación de la solicitud');
});

/* Solicitar instalacion de GPS y revisión física */
var btnSolicitarGPS = '';
$(document).on('click', 'button#btnSolicitarGPS', function () {

    btnSolicitarGPS = $(this);

    $("#txtVIN_SolicitarGPS").val(VIN);
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
                MensajeError('No se pudo guardar la solicitud de revisión física y GPS, contacte al administrador.');
                $("#btnSolicitarGPS_Confirmar").css('disabled', false);
            },
            success: function (data) {

                $("#modalSolicitarGPS").modal('hide');

                data.d == true ? MensajeExito('La solicitud se guardó correctamente') : MensajeError('No se pudo guardar la solicitud de revisión física y GPS, contacte al administrador.');

                $("#txtFechaInstalacion,#ddlUbicacionInstalacion,#txtComentario").val('');

                $(btnSolicitarGPS).replaceWith('<button type="button" class="dropdown-item" id="btnDetalleSolicitudGPS"><i class="fas fa-map-marker-alt"></i> Detalles solicitud revisión física y GPS</button>');

                $("#btnSolicitarGPS_Confirmar").css('disabled', false);
            }
        });
    }
    else
        $('#frmPrincipal').parsley().validate({ group: 'InstalacionGPS_Guardar', force: true });
});

/* Actualizar solicitud de GPS y revisión física */
var idSolicitudGPS_Actualizar = 0;
$(document).on('click', 'button#btnDetalleSolicitudGPS', function () {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/CargarInformacionSolicitudGPS",
        data: JSON.stringify({ idSolicitud: idSolicitud, idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información de la solicitud de revisión física y GPS, contacte al administrador.');
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
                    $("#btnActualizarSolicitudGPS").prop('disabled', true).removeClass('btn-primary').addClass('btn-secondary').prop('title', 'Esta solicitud de revisión física y GPS ya no puede ser actualizada debido a su estado').addClass('disabled');
                }
                else {
                    $("#btnActualizarSolicitudGPS").prop('disabled', false).removeClass('btn-secondary').addClass('btn-primary').prop('title', 'Actualizar información de esta solicitud de revisión física y GPS').removeClass('disabled');
                }

                $("#modalDetalleSolicitudGPS").modal();
            }
            else
                MensajeError('No se pudo cargar la información de la solicitud de revisión física y GPS, contacte al administrador.');
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
                MensajeError('No se pudo actualizar la solicitud de revisión física y GPS, contacte al administrador.');
            },
            success: function (data) {

                $("#modalActualizarSolicitudGPS").modal('hide');

                data.d == true ? MensajeExito('La solicitud de revisión física y GPS se actualizó correctamente') : MensajeError('No se pudo actualizar la solicitud de revisión física y GPS, contacte al administrador.');

                $("#txtFechaInstalacion_Actualizar,#ddlUbicacionInstalacion_Actualizar,#txtComentario_Actualizar").val('');
            }
        });
    }
    else
        $('#frmPrincipal').parsley().validate({ group: 'InstalacionGPS_Actualizar', force: true });
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
                var templateAcordion = '';

                for (var i = 0; i < revisionesGarantia.length; i++) {

                    templateAcordion += FormatoDetalleRevision(revisionesGarantia[i]);
                }

                $("#accordion-revisiones").empty().append(templateAcordion);

                $("#modalRevisionesGarantia").modal();
            }
            else
                MensajeError('No se pudo cargar las revisiones de la garantía, contacte al administrador.');
        }
    });
}

function FormatoDetalleRevision(revision) {

    return '<div class="card bg-light mb-1">' +
        '<div class="p-3" id="heading' + revision.IdRevision + '"> ' +
        '<div class="row justify-content-between">' +
        '<div class="col-auto">' +
        '<a href="#collapse' + revision.IdRevision + '" class="text-dark collapsed" data-toggle="collapse" aria-expanded="false" aria-controls="collapse' + revision.IdRevision + '">' +
        '<h6 class="m-0 font-14 font-weight-bold">' + revision.NombreRevision + '</h6>' +
        '</a>' +
        '</div>' +
        '<div class="col-auto">' +
        '<div class="badge badge-' + revision.EstadoRevisionClassName + ' p-2 float-right">' + revision.EstadoRevision + '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '<div id="collapse' + revision.IdRevision + '" class="collapse" aria-labelledby="heading' + revision.IdRevision + '" data-parent="#accordion-revisiones" style="">' +
        '<div class="card-body p-0">' +
        '<div class="form-group row">' +
        '<div class="col-sm-12">' +
        '<label class="col-form-label">Descripción de la revisión</label>' +
        '<textarea class="form-control form-control-sm" type="text" readonly="readonly" required="required">' + revision.DescripcionRevision + '</textarea>' +
        '</div>' +
        '<div class="col-sm-6">' +
        '<label class="col-form-label">Usuario validador</label>' +
        '<input class="form-control form-control-sm" type="text" readonly="readonly" value="' + revision.UsuarioValidador + '" required="required"/>' +
        '</div>' +
        '<div class="col-sm-6">' +
        '<label class="col-form-label">Fecha validación</label>' +
        '<input class="form-control form-control-sm" type="text" readonly="readonly" value="' + (revision.IdEstadoRevision != 0 ? moment(revision.FechaValidacion).locale('es').format('YYYY/MM/DD hh:mm a') : revision.EstadoRevision) + '" required="required"/>' +
        '</div>' +
        '<div class="col-sm-12">' +
        '<label class="col-form-label">Comentarios de la revisión</label>' +
        '<textarea class="form-control form-control-sm" type="text" readonly="readonly" required="required">' + revision.Observaciones + '</textarea>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>' +
        '</div>';
}

function MostrarInstalacionGPS(idAutoInstalacionGPS) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/CargarInstalacionGPS",
        data: JSON.stringify({ idAutoInstalacionGPS: idAutoInstalacionGPS, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar el registro de instalación GPS, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                var resultado = data.d;

                $("#lblIMEI").text(data.d.IMEI);
                $("#lblSerie").text(data.d.Serie);
                $("#lblModelo").text(data.d.Modelo);
                $("#lblCompania").text(data.d.Compania);
                $("#lblRelay").text(data.d.ConRelay);
                $("#txtUbicacion").val(data.d.ComentarioUbicacion);
                $("#txtComentariosDeLaInstalacion").val(data.d.ObservacionesInstalacion);

                /* Fotos de la instalacion */
                var divFotosInstalacionGPS = $("#divFotosInstalacionGPS").empty();
                var templateDocumentos = '';
                var documentos = resultado.Fotos;

                if (resultado.Fotos != null) {

                    if (resultado.Fotos.length > 0) {

                        for (var i = 0; i < documentos.length; i++) {

                            templateDocumentos += '<img alt="' + documentos[i].DescripcionTipoDocumento + '" src="' + documentos[i].URLArchivo + '" data-image="' + documentos[i].URLArchivo + '" data-description="' + documentos[i].DescripcionTipoDocumento + '"/>';
                        }
                    }
                }

                var imgNoHayFotografiasDisponibles = '<img alt="No hay fotografías disponibles" src="/Imagenes/Imagen_no_disponible.png" data-image="/Imagenes/Imagen_no_disponible.png" data-description="No hay fotografías disponibles"/>';
                templateDocumentos = templateDocumentos == '' ? imgNoHayFotografiasDisponibles : templateDocumentos;

                divFotosInstalacionGPS.append(templateDocumentos);

                $("#divFotosInstalacionGPS").unitegallery({
                    gallery_theme: "tilesgrid",
                    tile_width: 170,
                    tile_height: 120,
                    lightbox_type: "compact",
                    grid_num_rows: 15,
                    tile_enable_textpanel: true,
                    tile_textpanel_title_text_align: "center"
                });


                $("#modalDetallesInstalacionGPS").modal();
            }
            else
                MensajeError('No se pudo cargar el registro de instalación GPS, contacte al administrador.');
        }
    });
}

function MostrarDocumentosGarantia(idGarantia) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/CargarDocumentosGarantia",
        data: JSON.stringify({ idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los documentos de la garantía, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                var documentos = data.d;
                var divGaleriaGarantia = $("#divGaleriaGarantia").empty();
                var templateDocumentos = '';


                if (documentos != null) {

                    if (documentos.length > 0) {

                        for (var i = 0; i < documentos.length; i++) {

                            templateDocumentos += '<img alt="' + documentos[i].DescripcionTipoDocumento + '" src="' + documentos[i].URLArchivo + '" data-image="' + documentos[i].URLArchivo + '" data-description="' + documentos[i].DescripcionTipoDocumento + '"/>';
                        }
                    }
                }

                var imgNoHayFotografiasDisponibles = '<img alt="No hay fotografías disponibles" src="/Imagenes/Imagen_no_disponible.png" data-image="/Imagenes/Imagen_no_disponible.png" data-description="No hay fotografías disponibles"/>';
                templateDocumentos = templateDocumentos == '' ? imgNoHayFotografiasDisponibles : templateDocumentos;

                divGaleriaGarantia.append(templateDocumentos);

                $("#divGaleriaGarantia").unitegallery();

                $("#modalDocumentosDeLaGarantia").modal();
            }
            else
                MensajeError('No se pudieron cargar los documentos de la garantía, contacte al administrador.');
        }
    });
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


function MostrarContenidoEnModalFullScreen(nombreFormulario, accion) {


    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitud: idSolicitud, idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo redireccionar a " + accion);
        },
        success: function (data) {

            $("#iframe-fullscreen").prop('src', '#');
            $(".modal.fade.show").modal("hide");
            $("#iframe-fullscreen").prop('src', nombreFormulario + "?" + data.d);
            $('body').css('overflow', 'hidden');
            $(".modal.modal-fullscreen .modal-body").css('overflow', 'hidden');
            $("#modalPantallaCompleta").modal();
        }
    });
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

/* Definir el tab que está mirando el usuario */
$("#tab_Listado_Solicitudes_Garantias_link").on("click", function () {
    tabActivo = 'tab_Listado_Solicitudes_Garantias';
});

/* Definir el tab que está mirando el usuario */
$("#tab_Listado_Garantias_SinSolicitud_link").on("click", function () {
    tabActivo = 'tab_Listado_Garantias_SinSolicitud';
});

/* Cuando de click en el boton de acciones de los datatables */
$('.table-responsive').on('show.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "inherit");
});

/* Cuando de click en el boton de acciones de los datatables */
$('.table-responsive').on('hide.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "auto");
});

jQuery("#date-range").datepicker({
    toggleActive: true
});