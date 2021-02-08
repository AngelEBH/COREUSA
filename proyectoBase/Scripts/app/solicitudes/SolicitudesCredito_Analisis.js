// #region variables globales

/* ====== Esta variable almacena el ID del estado actual de la solicitud ============== */
/* ====== Se actualiza cada vez que se cargan los detalles de la solicitud ============ */
/* ====== Se utiliza para realizar validaciones al cargar detalles de la solicitud ==== */
var ID_ESTADO_SOLICITUD = '0';

/* ====== Esta variable (objeto) almacena el estado actual de la solicitud ============ */
/* ====== Se actualiza cada vez que se cargan los detalles de la solicitud ============ */
/* ====== Se utiliza para realizar validaciones durante todo el analisis ============== */
var ESTADO_SOLICITUD = [];

/* ====== Esta variable se utiliza para validar si el valor de una fecha es nula ============== */
var PROCESO_PENDIENTE = '/Date(-2208967200000)/';

/* ====== Esta variable almacena durante el analisis el monto final por el que se va a autorizar la solicitud ============== */
/* ====== Esta variable almacena durante el analisis el plazo final por el que se va a autorizar la solicitud ============== */
var gMontoFinal = 0;
var gPlazoFinal = 0;


var resolucionHabilitada = false;
var resolucion = false;

var datatableLanguage = {
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
}

var select2language = {
    errorLoading: function () { return "No se pudieron cargar los resultados" },
    inputTooLong: function (e) { var n = e.input.length - e.maximum, r = "Por favor, elimine " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
    inputTooShort: function (e) { var n = e.minimum - e.input.length, r = "Por favor, introduzca " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
    loadingMore: function () { return "Cargando más resultados…" },
    maximumSelected: function (e) { var n = "Sólo puede seleccionar " + e.maximum + " elemento"; return 1 != e.maximum && (n += "s"), n },
    noResults: function () { return "No se encontraron resultados" },
    searching: function () { return "Buscando…" },
    removeAllItems: function () { return "Eliminar todos los elementos" }
}

// #endregion

// #region Document Ready

$(document).ready(function () {

    CargarDetallesDelProcesamientoDeLaSolicitud();

    /* Inicializar datatables de Referencias personales de la solicitud */
    $('#tblReferenciasPersonales').DataTable({
        "destroy": true,
        "pageLength": 10,
        "aaSorting": [],
        "responsive": true,
        "dom": "<'row'<'col-sm-12'>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        "language": datatableLanguage,
        columnDefs: [
            { targets: 'no-sort', orderable: false },
        ]
    });
});

// #endregion

// #region Cargar detalles de la solicitud

function AgregarFilaATablaDeEstatusDeLaSolicitud(descripcionProceso, procesoInicio, procesoFin, idTimerDelProceso, usuarioEncargado) {

    $('#tblDetalleEstado tbody').append('<tr>' +
        '<td class="text-center">' + descripcionProceso + '</td>' +
        '<td class="text-center">' + procesoInicio + '</td>' +
        '<td class="text-center">' + procesoFin + '</td>' +
        '<td class="text-center"><span id="' + idTimerDelProceso + '"></span></td>' +
        '<td class="text-center">' + usuarioEncargado + '</td>' +
        '</tr>');
}

/* ====== Carga el estado actual del procesamiento de la solicitud =============================== */
/* ====== Muestra el procesamiento de la solicitud en el modal "modalEstadoSolicitud" ============ */
/* ====== Además actualiza variables globales y realiza otras validaciones ======================= */
function CargarDetallesDelProcesamientoDeLaSolicitud(mostrarModalDeDetalles) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/CargarEstadoSolicitud",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ dataCrypt: window.location.href }),
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información de la solicitud de crédito, contacte al administrador');
        },
        beforeSend: function () {
            MostrarLoader();
        },
        success: function (data) {

            if (data.d != null) {

                var informacionSolicitud = data.d;
                var tablaEstatusSolicitud = $('#tblDetalleEstado tbody').empty();

                ID_ESTADO_SOLICITUD = informacionSolicitud.IdEstadoSolicitud;
                ESTADO_SOLICITUD = informacionSolicitud;

                /* En ingreso */
                var enIngresoInicio = ObtenerFechaFormateada(informacionSolicitud.EnIngresoInicio);
                var enIngresoFin = ObtenerFechaFormateada(informacionSolicitud.EnIngresoFin);
                var enIngresoUsuario = informacionSolicitud.UsuarioEnIngreso;

                if (ValidarFecha(informacionSolicitud.EnIngresoInicio) == null) {
                    enIngresoUsuario = '-';
                }

                AgregarFilaATablaDeEstatusDeLaSolicitud('Ingreso', enIngresoInicio, enIngresoFin, 'lblTiempoTranscurrido_EnIngreso', enIngresoUsuario);

                InicializarContador(informacionSolicitud.EnIngresoInicio, informacionSolicitud.EnIngresoFin, 'lblTiempoTranscurrido_EnIngreso');

                /* En tramite */
                var enColaInicio = ObtenerFechaFormateada(informacionSolicitud.EnColaInicio);
                var enColaFin = ObtenerFechaFormateada(informacionSolicitud.EnColaFin);

                AgregarFilaATablaDeEstatusDeLaSolicitud('Recepción', enColaInicio, enColaFin, 'lblTiempoTranscurrido_EnCola', 'N/A');

                InicializarContador(informacionSolicitud.EnColaInicio, informacionSolicitud.EnColaFin, 'lblTiempoTranscurrido_EnCola');

                /* En análisis */
                var enAnalisisInicio = ObtenerFechaFormateada(informacionSolicitud.EnAnalisisInicio);
                var enAnalisisFin = ObtenerFechaFormateada(informacionSolicitud.EnAnalisisFin);
                var enAnalisisUsuario = informacionSolicitud.UsuarioAnalista;

                if (ValidarFecha(informacionSolicitud.EnAnalisisInicio) == null) {
                    enAnalisisUsuario = '-';
                }

                AgregarFilaATablaDeEstatusDeLaSolicitud('Análisis', enAnalisisInicio, enAnalisisFin, 'lblTiempoTranscurrido_EnAnalisis', enAnalisisUsuario);

                InicializarContador(informacionSolicitud.EnAnalisisInicio, informacionSolicitud.EnAnalisisFin, 'lblTiempoTranscurrido_EnAnalisis');

                /* En campo */
                var enCampoInicio = ObtenerFechaFormateada(informacionSolicitud.EnRutaDeInvestigacionInicio);
                var enCampoFin = ObtenerFechaFormateada(informacionSolicitud.EnRutaDeInvestigacionFin);
                var enCampoUsuario = informacionSolicitud.UsuarioGestorAsignado;

                if (ValidarFecha(informacionSolicitud.EnRutaDeInvestigacionInicio) == null) {
                    enCampoUsuario = '-';
                }

                AgregarFilaATablaDeEstatusDeLaSolicitud('Campo', enCampoInicio, enCampoFin, 'lblTiempoTranscurrido_EnCampo', enCampoUsuario);

                InicializarContador(informacionSolicitud.EnRutaDeInvestigacionInicio, informacionSolicitud.EnRutaDeInvestigacionFin, 'lblTiempoTranscurrido_EnCampo');

                /* Condicionado */
                var condicionadoInicio = ObtenerFechaFormateada(informacionSolicitud.CondicionadoInicio);
                var condicionadoFin = ObtenerFechaFormateada(informacionSolicitud.CondicionadoFin);
                var condicionadoUsuario = informacionSolicitud.UsuarioCondicionado;

                if (ValidarFecha(informacionSolicitud.CondicionadoInicio) == null) {
                    condicionadoUsuario = '-';
                }

                AgregarFilaATablaDeEstatusDeLaSolicitud('Condicionado', condicionadoInicio, condicionadoFin, 'lblTiempoTranscurrido_Condicionado', condicionadoUsuario);

                InicializarContador(informacionSolicitud.CondicionadoInicio, informacionSolicitud.CondicionadoFin, 'lblTiempoTranscurrido_Condicionado');

                /* Reprogramado */
                var reprogramadoInicio = ObtenerFechaFormateada(informacionSolicitud.ReprogramadoInicio);
                var reprogramadoFin = ObtenerFechaFormateada(informacionSolicitud.ReprogramadoFin);
                var reprogramadoUsuario = informacionSolicitud.UsuarioGestorAsignado;

                if (ValidarFecha(informacionSolicitud.ReprogramadoInicio) == null) {
                    reprogramadoUsuario = '-';
                }

                AgregarFilaATablaDeEstatusDeLaSolicitud('Reprogramado', reprogramadoInicio, reprogramadoFin, 'lblTiempoTranscurrido_Reprogramado', reprogramadoUsuario);

                InicializarContador(informacionSolicitud.ReprogramadoInicio, informacionSolicitud.ReprogramadoFin, 'lblTiempoTranscurrido_Reprogramado');

                /* Validación */
                var validacionInicio = ObtenerFechaFormateada(informacionSolicitud.PasoFinalInicio);
                var validacionFin = ObtenerFechaFormateada(informacionSolicitud.PasoFinalFin);
                var validacionUsuario = informacionSolicitud.UsuarioPasoFinal;

                if (ValidarFecha(informacionSolicitud.PasoFinalInicio) == null) {
                    validacionUsuario = '-';
                }

                AgregarFilaATablaDeEstatusDeLaSolicitud('Validación', validacionInicio, validacionFin, 'lblTiempoTranscurrido_Validacion', (validacionUsuario == '' ? '-' : validacionUsuario));

                InicializarContador(informacionSolicitud.PasoFinalInicio, informacionSolicitud.PasoFinalFin, 'lblTiempoTranscurrido_Validacion');

                /* Tiempo total transcurrido */
                tablaEstatusSolicitud.append('<tr>' +
                    '<td colspan="5" class="text-center">Tiempo total transcurrido: <span id="lblTiempoTotal"></span></td>' +
                    '</tr >');

                InicializarContador(informacionSolicitud.EnColaInicio, informacionSolicitud.TiempoTomaDecisionFinal, 'lblTiempoTotal');

                /* Verificar estado de la solicitud */
                $("#lblEstadoSolicitud").text(informacionSolicitud.EstadoSolicitud);

                var estadoSolicitudColorClass = informacionSolicitud.IdEstadoSolicitud == "7" ? "success" : (informacionSolicitud.IdEstadoSolicitud == "5" || informacionSolicitud.IdEstadoSolicitud == "4") ? "danger" : "warning";

                $("#lblEstadoSolicitud").removeClass('text-danger').removeClass('text-warning').removeClass('text-success').addClass('text-' + estadoSolicitudColorClass);

                var contadorComentario = 0;

                /* Reprogramado comentario */
                if (informacionSolicitud.ComentarioReprogramado != '') {
                    $("#lblUsuario_ComentarioReprogramacion").text(informacionSolicitud.UsuarioGestorAsignado);
                    $("#lblFecha_ComentarioReprogramacion").text(ObtenerFechaFormateada(informacionSolicitud.ReprogramadoInicio));
                    $("#lblComentario_Reprogramacion").text(informacionSolicitud.ComentarioReprogramado);
                    $("#liObservacionesReprogramacion").css('display', '');
                    contadorComentario++;
                }
                else {
                    $("#liObservacionesReprogramacion").css('display', 'none');
                }

                /* Condicionado comentario */
                if (informacionSolicitud.ComentarioCondicionado != '') {
                    $("#lblUsuario_ComentarioOtrosCondicionamientos").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_OtrosCondicionamientos").text(ObtenerFechaFormateada(informacionSolicitud.CondicionadoInicio));
                    $("#lblComentario_OtrosCondicionamientos").text(informacionSolicitud.ComentarioCondicionado);
                    $("#liObservaciones_OtrosCondicionamientos").css('display', '');
                    contadorComentario++;
                }
                else {
                    $("#liObservaciones_OtrosCondicionamientos").css('display', 'none');
                }

                /* Informacion personal comentario */
                if (informacionSolicitud.ComentarioValidacionInformacionPersonal != '') {
                    $("#lblUsuario_ComentarioInformacionPerosnal").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioInformacionPersonal").text(ObtenerFechaFormateada(informacionSolicitud.FechaValidacionInformacionPersonal));
                    $("#lblComentario_InformacionPersonal").text(informacionSolicitud.ComentarioValidacionInformacionPersonal);
                    $("#liObservacionesInformacionPersonal").css('display', '');
                    contadorComentario++;
                }
                else {
                    $("#liObservacionesInformacionPersonal").css('display', 'none');
                }

                /* Informacion laboral comentario */
                if (informacionSolicitud.ComentarioValidacionInformacionLaboral != '') {
                    $("#lblUsuario_ComentarioInformacionLaboral").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioInformacionLaboral").text(ObtenerFechaFormateada(informacionSolicitud.FechaValidacionInformacionLaboral));
                    $("#lblComentario_InformacionLaboral").text(informacionSolicitud.ComentarioValidacionInformacionLaboral);
                    $("#liObservacionesInformacionLaboral").css('display', '');
                    contadorComentario++;
                }
                else {
                    $("#liObservacionesInformacionLaboral").css('display', 'none');
                }

                /* Referencias personales comentario */
                if (informacionSolicitud.ComentarioValidacionReferenciasPersonales != '') {
                    $("#lblUsuario_ComentarioReferenciasPersonales").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioReferenciasPersonales").text(ObtenerFechaFormateada(informacionSolicitud.FechaValidacionReferenciasPersonales));
                    $("#lblComentario_ReferenciasPersonales").text(informacionSolicitud.ComentarioValidacionReferenciasPersonales);
                    $("#liObservacionesReferenciasPersonales").css('display', '');
                    contadorComentario++;
                }
                else {
                    $("#liObservacionesReferenciasPersonales").css('display', 'none');
                }

                /* Documentación comentario */
                if (informacionSolicitud.ComentarioValidacionDocumentacion != '') {
                    $("#lblUsuario_ComentarioDocumentacion").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioDocumentacion").text(ObtenerFechaFormateada(informacionSolicitud.FechaValidacionDocumentacion));
                    $("#lblComentario_Documentacion").text(informacionSolicitud.ComentarioValidacionDocumentacion);
                    $("#liObservacionesDocumentacion").css('display', '');
                    contadorComentario++;
                }
                else {
                    $("#liObservacionesDocumentacion").css('display', 'none');
                }

                /* Observaciones de crédito */
                if (informacionSolicitud.ObservacionesDeCreditos != '') {
                    $("#lblUsuario_ComentarioParaGestoria").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioParaGestoria").text(ObtenerFechaFormateada(informacionSolicitud.FechaEnvioARuta));
                    $("#lblComentario_ParaGestoria").text(informacionSolicitud.ObservacionesDeCreditos);
                    $("#liObservacionesParaGestoria").css('display', '');
                    contadorComentario++;
                }
                else {
                    $("#liObservacionesParaGestoria").css('display', 'none');
                }

                /* Observaciones de gestoria */
                if (informacionSolicitud.ObservacionesDeCampo != '') {
                    $("#lblUsuario_ComentarioDeGestoria").text(informacionSolicitud.UsuarioGestorAsignado);
                    $("#lblFecha_ComentarioDeGestoria").text(ObtenerFechaFormateada(informacionSolicitud.EnRutaDeInvestigacionFin));
                    $("#lblComentario_DeGestoria").text(informacionSolicitud.ObservacionesDeCampo);
                    $("#liObservacionesDeGestoria").css('display', '');
                    contadorComentario++;
                }
                else {
                    $("#liObservacionesDeGestoria").css('display', 'none');
                }

                /* Comentarios de la resolución */
                if (informacionSolicitud.ComentarioResolucion != '') {
                    $("#lblUsuario_ComentarioDeLaResolucion").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioDeLaResolucion").text(ObtenerFechaFormateada(informacionSolicitud.TiempoTomaDecisionFinal));
                    $("#lblComentario_Resolicion").text(informacionSolicitud.ComentarioResolucion);
                    $("#liComentariosDeLaResolucion").css('display', '');
                    contadorComentario++;
                }
                else {
                    $("#liComentariosDeLaResolucion").css('display', 'none');
                }

                if (contadorComentario > 0) {
                    $("#divNoHayMasDetalles").css('display', 'none');
                    $("#divLineaDeTiempo").css('display', '');
                }
                else {
                    $("#divNoHayMasDetalles").css('display', '');
                    $("#divLineaDeTiempo").css('display', 'none');
                }

                if (informacionSolicitud.SolicitudActiva == 0) {
                    $("#divSolicitudInactiva").css('display', '');
                }
                else {
                    $("#divSolicitudInactiva").css('display', 'none');
                }

                /* Inicializar datatables de condiciones de la solicitud de credito */
                $('#tblListaCondicionesDeLaSolicitud').DataTable({
                    "destroy": true,
                    "pageLength": 10,
                    "aaSorting": [],
                    "responsive": true,
                    "language": datatableLanguage,
                    data: informacionSolicitud.Condiciones,
                    "columns": [
                        { "data": "Condicion" },
                        { "data": "DescripcionCondicion" },
                        { "data": "ComentarioAdicional" },
                        {
                            "data": "EstadoCondicion",
                            "render": function (value) {
                                return value != true ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-warning mb-0'>Pendiente</label>"
                            }
                        }
                    ]
                });

                /* iconos del procesamiento de la solicitud que está debajo de la información principal de la solicitud */
                var iconoRojo = "<i class='mdi mdi-close-circle-outline mdi-18px text-danger'></i>";
                var iconoExito = "<i class='mdi mdi-check-circle-outline mdi-18px text-success'></i>";
                var iconoPendiente = "<i class='mdi mdi-check-circle-outline mdi-18px text-warning'></i>";
                var iconoCancelado = "<i class='mdi mdi-check-circle-outline mdi-18px text-secondary'></i>";
                var estadoIngreso = '', estadoEnCola = '', estadoAnalisis = '', estadoCampo = '', estadoCondicionado = '', estadoReprogramado = '', estadoPasoFinal = '', estadoResolucion = '';
                var tblEstadoSolicitud = $("#tblEstadoSolicitud tbody");

                /* Estado en ingreso */
                if (ValidarFecha(informacionSolicitud.EnIngresoInicio) != null) {
                    estadoIngreso = ValidarFecha(informacionSolicitud.EnIngresoFin) != null ? iconoExito : iconoPendiente;
                }

                /* Estado en cola o recepción */
                if (ValidarFecha(informacionSolicitud.EnColaInicio) != null) {

                    estadoEnCola = ValidarFecha(informacionSolicitud.EnColaFin) != null ? iconoExito : iconoPendiente;

                    if (ValidarFecha(informacionSolicitud.EnColaFin) == null && (ID_ESTADO_SOLICITUD == "4" || ID_ESTADO_SOLICITUD == "5" || ID_ESTADO_SOLICITUD == "7")) {
                        estadoEnCola = ID_ESTADO_SOLICITUD == "7" ? iconoExito : iconoCancelado;
                    }
                }

                /* Estado en analisis */
                if (ValidarFecha(informacionSolicitud.EnAnalisisInicio) != null) {

                    estadoAnalisis = ValidarFecha(informacionSolicitud.EnAnalisisFin) != null ? iconoExito : iconoPendiente;

                    if (ValidarFecha(informacionSolicitud.EnAnalisisFin) == null && (ID_ESTADO_SOLICITUD == "4" || ID_ESTADO_SOLICITUD == "5" || ID_ESTADO_SOLICITUD == "7")) {
                        estadoAnalisis = ID_ESTADO_SOLICITUD == "7" ? iconoExito : iconoCancelado;
                    }
                    if (ID_ESTADO_SOLICITUD == "4") {
                        estadoAnalisis = iconoRojo;
                    }
                }

                /* Estado en campo */
                if (ValidarFecha(informacionSolicitud.FechaEnvioARuta) != null) {

                    estadoCampo = ValidarFecha(informacionSolicitud.EnRutaDeInvestigacionFin) != null ? iconoExito : iconoPendiente;

                    if (ValidarFecha(informacionSolicitud.EnRutaDeInvestigacionFin) == null && (ID_ESTADO_SOLICITUD == "4" || ID_ESTADO_SOLICITUD == "5" || ID_ESTADO_SOLICITUD == "7")) {
                        estadoCampo = ID_ESTADO_SOLICITUD == "7" ? iconoExito : iconoCancelado;
                    }
                    if (ID_ESTADO_SOLICITUD == "5") {
                        estadoCampo = iconoRojo;
                    }
                }

                /* Estado condicionada */
                if (ValidarFecha(informacionSolicitud.CondicionadoInicio) != null) {

                    estadoCondicionado = ValidarFecha(informacionSolicitud.CondicionadoFin) != null ? iconoExito : iconoPendiente;

                    if (ValidarFecha(informacionSolicitud.CondicionadoFin) == null && (ID_ESTADO_SOLICITUD == "4" || ID_ESTADO_SOLICITUD == "5" || ID_ESTADO_SOLICITUD == "7")) {
                        estadoCondicionado = ID_ESTADO_SOLICITUD == "7" ? iconoExito : iconoCancelado;
                    }
                }

                /* Estado Reprogramado */
                if (ValidarFecha(informacionSolicitud.ReprogramadoInicio) != null) {

                    estadoReprogramado = ValidarFecha(informacionSolicitud.ReprogramadoFin) != null ? iconoExito : iconoPendiente;

                    if (ValidarFecha(informacionSolicitud.ReprogramadoFin) == null && (ID_ESTADO_SOLICITUD == "4" || ID_ESTADO_SOLICITUD == "5" || ID_ESTADO_SOLICITUD == "7")) {
                        estadoReprogramado = ID_ESTADO_SOLICITUD == "7" ? iconoExito : iconoCancelado;
                    }
                }

                /* Estado validacion final */
                if (ValidarFecha(informacionSolicitud.PasoFinalInicio) != null) {

                    estadoPasoFinal = ValidarFecha(informacionSolicitud.PasoFinalFin) != null ? iconoExito : iconoPendiente;

                    if (ValidarFecha(informacionSolicitud.PasoFinalFin) == null && (ID_ESTADO_SOLICITUD == "4" || ID_ESTADO_SOLICITUD == "5" || ID_ESTADO_SOLICITUD == "7")) {
                        estadoPasoFinal = ID_ESTADO_SOLICITUD == "7" ? iconoExito : iconoCancelado;
                    }
                }

                if (ID_ESTADO_SOLICITUD == "4" || ID_ESTADO_SOLICITUD == "5" || ID_ESTADO_SOLICITUD == "7") {
                    estadoResolucion = ID_ESTADO_SOLICITUD == "7" ? iconoExito : iconoRojo;
                }
                else if (ValidarFecha(informacionSolicitud.PasoFinalInicio) != null) {
                    estadoResolucion = iconoPendiente;
                }

                tblEstadoSolicitud.empty().append('<tr><td> ' + estadoIngreso + ' </td><td> ' + estadoEnCola + ' </td><td> ' + estadoAnalisis + ' </td><td> ' + estadoCampo + ' </td><td> ' + estadoCondicionado + ' </td><td> ' + estadoReprogramado + ' </td><td> ' + estadoPasoFinal + ' </td><td> ' + estadoResolucion + ' </td></tr>');

                if (mostrarModalDeDetalles == true) {
                    $("#modalEstadoSolicitud").modal();
                }

            } // if data.d != null
            else {
                MensajeError('No se pudo cargar la información de la solicitud de crédito, contacte al administrador');
            }
        }, // success
        complete: function () {
            OcultarLoader()
        }
    }); // $.ajax
}

// #endregion Cargar detalles de la solicitud

// #region Administrar condiciones de la solicitud

/* ====== Almacena la cantidad de nuevas condiciones que se estan agregando ================ */
/* ====== Se utiliza para validar que se agregue por lo menos una condición ================ */
var contadorNuevasCondiciones = 0;

/* ====== Arreglo que almacena las nuevas condiciones que se van a agregar ================= */
/* ====== Se actualiza al mismo tiempo que se van agregando las nuevas condiciones al DOM == */
var listaNuevasCondiciones = [];

/* Abrir modal para condicionar solicitud y ver las condiciones que ha tenido la solicitud */
$("#btnCondicionarSolicitud").click(function () {

    /* Reiniciar el listado de nuevas condiciones cada vez que se abra esta opción */
    listaNuevasCondiciones = [];
    contadorNuevasCondiciones = 0;
    $("#btnCondicionarSolicitudConfirmar").prop('disabled', true);
    $("#txtComentarioAdicional").val('');

    $("#tblNuevasCondiciones tbody").empty().append('<tr><td class="text-center" colspan="3">No hay registros disponibles...</td></tr>');

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/ObtenerCatalogoCondicionesYSolicitudCondiciones",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeError('Error al cargar el catalogo de condiciones y/o condiciones de la solicitud.');
        },
        success: function (data) {

            let catalogoCondiciones = data.d.CatalogoCondiciones;
            let ddlCondiciones = $("#ddlCondiciones");

            /* Catalogo de condiciones */
            ddlCondiciones.empty();

            ddlCondicionesTemplate = '';
            let grupoDdlCondicion = '';

            for (var i = 0; i < catalogoCondiciones.length; i++) {

                if (grupoDdlCondicion != catalogoCondiciones[i].Condicion) {
                    ddlCondicionesTemplate += ' <optgroup label="' + catalogoCondiciones[i].Condicion + '">';
                }

                ddlCondicionesTemplate += '<option value="' + catalogoCondiciones[i].IdCondicion + '">' + catalogoCondiciones[i].DescripcionCondicion + '</option>';

                if (grupoDdlCondicion != catalogoCondiciones[i].Condicion) {
                    ddlCondicionesTemplate += ' </optgroup>';
                }

                grupoDdlCondicion = catalogoCondiciones[i].Condicion;
            }

            ddlCondiciones.append(ddlCondicionesTemplate);

            ddlCondiciones.select2({ language: select2language });

            /* Inicializar datatables de condiciones */
            $('#tblListaSolicitudCondiciones').DataTable({
                "destroy": true,
                "pageLength": 10,
                "aaSorting": [],
                "responsive": true,
                "language": datatableLanguage,
                data: data.d.CondicionesDeLaSolicitud,
                "columns": [
                    { "data": "Condicion" },
                    { "data": "DescripcionCondicion" },
                    { "data": "ComentarioAdicional" },
                    {
                        "data": "EstadoCondicion",
                        "render": function (value) {
                            return value != true ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-warning mb-0'>Pendiente</label>"
                        }
                    },
                    {
                        "data": "EstadoCondicion", "className": "text-center",
                        "render": function (data, type, row) {
                            return row["EstadoCondicion"] != true ? '' : '<button id="btnAnularCondicion" data-id="' + row["IdSolicitudCondicion"] + '" class="btn btn-sm btn-block btn-danger mb-0" type="button" title="Anular condición"><i class="far fa-trash-alt"></i> Anular</button>';
                        }
                    },
                ]
            });

            $("#modalCondicionarSolicitud").modal();

        } // success
    }); // $.ajax
});

/* Agregar nueva condición al DOM y al arreglo (listaNuevasCondiciones) de nuevas condiciones que se guardarán */
$("#btnAgregarNuevaCondicion").click(function () {

    if ($($("#frmPrincipal")).parsley().isValid({ group: 'agregarCondiciones' })) {

        var tblNuevasCondiciones = $("#tblNuevasCondiciones tbody");

        /* Si es la primera condición que se agrega al DOM de la tabla de nuevas condiciones, eliminar mensaje "No hay registros disponibles" */
        if (contadorNuevasCondiciones == 0) {
            tblNuevasCondiciones.empty();
        }

        var IdCondicion = $("#ddlCondiciones :selected").val();
        var DescripcionCondicion = $("#ddlCondiciones :selected").text();
        var ComentarioAdicional = $("#txtComentarioAdicional").val();
        var btnQuitarCondicion = '<button data-id=' + IdCondicion + ' data-comentario="' + ComentarioAdicional + '" id="btnQuitarCondicion" class="btn btn-sm btn-danger"><i class="far fa-trash-alt"></i> Quitar</button>';

        tblNuevasCondiciones.append('<tr><td>' + DescripcionCondicion + '</td><td>' + ComentarioAdicional + '</td><td>' + btnQuitarCondicion + '</td></tr>');

        listaNuevasCondiciones.push({
            IdCondicion: IdCondicion,
            ComentarioAdicional: ComentarioAdicional
        });

        contadorNuevasCondiciones++;

        $("#btnCondicionarSolicitudConfirmar").prop('disabled', false);

        $("#txtComentarioAdicional").val('');
    }
    else {
        $($("#frmPrincipal")).parsley().validate({ group: 'agregarCondiciones', force: true });
    }
});

/* Quitar nueva condición del DOM de la tabla y del arreglo (listaNuevasCondiciones) de nuevas condiciones que se guardará */
$(document).on('click', 'button#btnQuitarCondicion', function () {

    let nuevaListaDeCondiciones = [];

    let condicionAQuitar = {
        IdCondicion: $(this).data('id').toString(),
        ComentarioAdicional: $(this).data('comentario')
    };

    if (listaNuevasCondiciones.length > 0) {

        let item;

        for (var i = 0; i < listaNuevasCondiciones.length; i++) {

            item = {
                IdCondicion: listaNuevasCondiciones[i].IdCondicion,
                ComentarioAdicional: listaNuevasCondiciones[i].ComentarioAdicional
            };

            if (JSON.stringify(item) != JSON.stringify(condicionAQuitar)) {
                nuevaListaDeCondiciones.push(item);
            }
        }
    }
    listaNuevasCondiciones = nuevaListaDeCondiciones;
    contadorNuevasCondiciones -= 1;

    $(this).closest('tr').remove();

    if (contadorNuevasCondiciones == 0) {
        $("#tblNuevasCondiciones tbody").append('<tr><td class="text-center" colspan="5">No hay registros disponibles...</td></tr>');
        $("#btnCondicionarSolicitudConfirmar").prop('disabled', true);
    }
});

/* Confirmar condicionar la solicitud, guardar todas las nuuevas condiciones y cambiar el estado de la solicitud */
$("#btnCondicionarSolicitudConfirmar").click(function () {

    if (contadorNuevasCondiciones >= 1 && listaNuevasCondiciones.length >= 1) {
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/CondicionarSolicitud",
            data: JSON.stringify({ listaCondiciones: listaNuevasCondiciones, observacionesOtrasCondiciones: $("#razonCondicion").val(), dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {

                if (data.d == true) {
                    $("#modalCondicionarSolicitud").modal('hide');

                    MensajeExito('Estado de la solicitud actualizado. Actualizando información de la solicitud...');
                    CargarDetallesDelProcesamientoDeLaSolicitud(false);
                }
                else {
                    MensajeError('Error al condicionar la solicitud, contacte al administrador');
                }
            } // success
        }); // $.ajax
    } // if contadorNuevasCondiciones >= 1 && listaNuevasCondiciones.length >= 1
    else {
        MensajeError('Debes agregar por lo menos una condición');
    }
});

/* Anular o cancelar condiciones de la solicitud. Estas condiciones son las que se muestran dinamicamente en el modal de condicionar solicitud (modalCondicionarSolicitud) */
$(document).on('click', 'button#btnAnularCondicion', function () {

    var buttonAnularCondicion = $(this);

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/AnularCondicionDeLaSolicitud',
        data: JSON.stringify({ idSolicitudCondicion: buttonAnularCondicion.data('id'), dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al anular la condición, contacte al administrador.');
        },
        success: function (data) {

            if (data.d == true) {
                buttonAnularCondicion.replaceWith('<label class="btn btn-sm btn-success mb-0"><i class="far fa-check-circle"></i> Anulada</label>');

                MensajeExito('La condición se anuló correctamente. Actualizando información de la solicitud...');
                CargarDetallesDelProcesamientoDeLaSolicitud();
            }
            else {
                MensajeError('Error al anular la condición, contacte al administrador.');
            }
        }
    });
});

// #endregion Administrar condiciones de la solicitud

// #region Administrar referencias personales

/* Actualizar comentario sobre una referencia personal radiofaro */
var idReferenciaPersonalSeleccionada = '';
var btnReferenciaPersonalSeleccionada = '';


/* Actualizar comentario de la referencia */
$(document).on('click', 'button#btnComentarioReferencia', function () {

    btnReferenciaPersonalSeleccionada = $(this);
    idReferenciaPersonalSeleccionada = btnReferenciaPersonalSeleccionada.data('id');

    $("#txtNombreReferenciaModal").val(btnReferenciaPersonalSeleccionada.data('nombrereferencia'));
    $("#txtAnalistaDeCredito").val(btnReferenciaPersonalSeleccionada.data('analista'));
    $("#txtFechaDeAnalisis").val(btnReferenciaPersonalSeleccionada.data('fechaanalisis'));
    $("#txtObservacionesReferencia").val(btnReferenciaPersonalSeleccionada.data('observaciones'));

    if (btnReferenciaPersonalSeleccionada.data('sincomunicacion') == true)
        $("#cbSinComunicacion").prop('checked', true);
    else
        $("#cbSinComunicacion").prop('checked', false);

    $("#modalObservacionesReferenciaPersonal").modal();
});

$("#btnActualizarObservacionReferencia").click(function () {

    if ($($("#txtObservacionesReferencia")).parsley().isValid()) {

        DeshabilitarElementoPorId('btnActualizarObservacionReferencia');

        let observacionesReferenciaPersonal = $('#txtObservacionesReferencia').val();

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/ActualizarObservacionesReferenciaPersonal',
            data: JSON.stringify({ idReferencia: idReferenciaPersonalSeleccionada, observaciones: observacionesReferenciaPersonal, sinComunicacion: $("#cbSinComunicacion").prop('checked'), dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al actualizar observaciones de la referencia personal, contacte al administrador.');
            },
            success: function (data) {

                if (data.d == true) {
                    MensajeExito('Las observaciones/comentarios se actualizaron correctamente. Actualizando listado...');
                    CargarReferenciasPersonales();
                }
                else
                    MensajeError('Error al actualizar observaciones de la referencia personal');

                $("#modalObservacionesReferenciaPersonal").modal('hide');
            },
            complete: function (data) {
                HabilitarElementoPorId('btnActualizarObservacionReferencia');
            }
        });
    }
    else
        $($("#txtObservacionesReferencia")).parsley().validate();
});


/* Agregar referencia personal */
$("#btnAgregarReferencia").click(function () {

    $("#txtNombreReferencia,#txtTelefonoReferencia,#ddlTiempoDeConocerReferencia, #ddlParentescos, #txtLugarTrabajoReferencia, #txtObservacionesNuevaReferencia").val('');
    $('#modalAgregarReferenciaPersonal').parsley().reset();
    $("#modalAgregarReferenciaPersonal").modal();
});

$("#btnAgregarReferenciaConfirmar").click(function () {

    /* Validar formulario de agregar referencia personal */
    if ($($('#frmPrincipal')).parsley().isValid({ group: 'referenciasPersonales' })) {

        DeshabilitarElementoPorId('btnAgregarReferenciaConfirmar');

        var referenciaPersonal = {
            IdCliente: ID_CLIENTE,
            NombreCompleto: $("#txtNombreReferencia").val(),
            TelefonoReferencia: $("#txtTelefonoReferencia").val(),
            LugarTrabajo: $("#txtLugarTrabajoReferencia").val(),
            IdTiempoDeConocer: $("#ddlTiempoDeConocerReferencia :selected").val(),
            IdParentescoReferencia: $("#ddlParentescos :selected").val(),
        }

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/RegistrarReferenciaPersonal",
            data: JSON.stringify({ referenciaPersonal: referenciaPersonal, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo agregar la referencia personal, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {
                    MensajeExito('La referencia personal se agregó correctamente.');
                    CargarReferenciasPersonales();
                }
                else
                    MensajeError("No se pudo agregar la referencia personal, contacte al administrador.");

                $("#modalAgregarReferenciaPersonal").modal('hide');
            },
            complete: function (data) {
                HabilitarElementoPorId('btnAgregarReferenciaConfirmar');
            }
        });
    }
    else
        $($("#frmPrincipal")).parsley().validate({ group: 'referenciasPersonales', force: true });
});


/* Actualizar referencia personal */
function AbrirModalActualizarReferenciaPersonal(btnReferenciaPersonal) {

    let referenciaPersonal = $(btnReferenciaPersonal);

    idReferenciaPersonalSeleccionada = referenciaPersonal.data('id');

    $("#txtNombreReferenciaPersonal_Editar").val(referenciaPersonal.data('nombrereferencia'));
    $("#txtTelefonoReferenciaPersonal_Editar").val(referenciaPersonal.data('telefono'));
    $("#ddlTiempoDeConocerReferencia_Editar").val(referenciaPersonal.data('idtiempodeconocer'));
    $("#ddlParentesco_Editar").val(referenciaPersonal.data('idparentesco'));
    $("#txtLugarDeTrabajoReferencia_Editar").val(referenciaPersonal.data('lugardetrabajo'));
    $("#modalEditarReferenciaPersonal").modal();
};

$("#btnEditarReferenciaConfirmar").click(function (e) {

    /* Validar formulario de actualizar referencia personal */
    if ($('#frmPrincipal').parsley().isValid({ group: 'referenciasPersonalesEditar' })) {

        DeshabilitarElementoPorId('btnEditarReferenciaConfirmar');

        var referenciaPersonal = {
            IdReferencia: idReferenciaPersonalSeleccionada,
            NombreCompleto: $("#txtNombreReferenciaPersonal_Editar").val(),
            TelefonoReferencia: $("#txtTelefonoReferenciaPersonal_Editar").val(),
            LugarTrabajo: $("#txtLugarDeTrabajoReferencia_Editar").val(),
            IdTiempoDeConocer: $("#ddlTiempoDeConocerReferencia_Editar :selected").val(),
            IdParentescoReferencia: $("#ddlParentesco_Editar :selected").val()
        }

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ActualizarReferenciaPersonal",
            data: JSON.stringify({ referenciaPersonal: referenciaPersonal, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo editar la referencia personal, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {
                    MensajeExito('La referencia personal se editó correctamente.');
                    CargarReferenciasPersonales();
                }
                else
                    MensajeError("No se pudo editar la referencia personal, contacte al administrador.");

                $("#modalEditarReferenciaPersonal").modal('hide');
            },
            complete: function (data) {
                HabilitarElementoPorId('btnEditarReferenciaConfirmar');
            }
        });
    }
    else
        $('#frmPrincipal').parsley().validate({ group: 'referenciasPersonalesEditar', force: true });
});


/* Eliminar referencia personal */
function AbrirModalEliminarReferenciaPersonal(idReferenciaPersonal) {

    idReferenciaPersonalSeleccionada = idReferenciaPersonal;
    $("#modalEliminarReferencia").modal('show');
}

$("#btnEliminarReferenciaConfirmar").click(function () {

    DeshabilitarElementoPorId('btnEliminarReferenciaConfirmar');

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/EliminarReferenciaPersonal',
        data: JSON.stringify({ idReferenciaPersonal: idReferenciaPersonalSeleccionada, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al eliminar la referencia personal, contacte al administrador.');
        },
        success: function (data) {

            if (data.d == true) {
                MensajeExito('La referencia personal se eliminó correctamente');
                CargarReferenciasPersonales();
            }
            else
                MensajeError('Error al eliminar la referencia personal, contacte al administrador.');

            $("#modalEliminarReferencia").modal('hide');
        },
        complete: function (data) {
            HabilitarElementoPorId('btnEliminarReferenciaConfirmar');
        }
    });
});


function CargarReferenciasPersonales() {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ListadoReferenciasPersonalesPorIdSolicitud',
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar las referencias personales de la solicitud, contacte al administrador.');
        },
        success: function (data) {

            var listaReferenciasPersonales = data.d;
            var tblReferenciasPersonales = $("#tblReferenciasPersonales tbody").empty().append('<tr><td class="text-center" colspan="7">No hay registros disponibles...</td></tr>');

            if (listaReferenciasPersonales != null) {

                if (listaReferenciasPersonales.length > 0) {

                    let templateReferenciasPersonales = '';
                    let btnComentarioReferenciaPersonal, btnEliminarReferencia, btnActualizarReferencia = '';
                    let estado, colorClass, stringDatas = '';
                    tblReferenciasPersonales.empty();

                    for (var i = 0; i < listaReferenciasPersonales.length; i++) {

                        stringDatas = 'data-id="' + listaReferenciasPersonales[i].IdReferencia + '" data-nombrereferencia="' + listaReferenciasPersonales[i].NombreCompleto + '" data-telefono="' + listaReferenciasPersonales[i].TelefonoReferencia + '" data-idtiempodeconocer="' + listaReferenciasPersonales[i].IdTiempoDeConocer + '" data-idparentesco="' + listaReferenciasPersonales[i].IdParentescoReferencia + '" data-lugardetrabajo="' + listaReferenciasPersonales[i].LugarTrabajo + '" data-observaciones="' + listaReferenciasPersonales[i].ComentarioDeptoCredito + '" data-analista="' + listaReferenciasPersonales[i].AnalistaComentario + '" data-sincomunicacion="' + listaReferenciasPersonales[i].SinComunicacion + '" data-fechaanalisis="' + (listaReferenciasPersonales[i].FechaAnalisis == PROCESO_PENDIENTE ? (listaReferenciasPersonales[i].AnalistaComentario != '' ? 'Fecha no disponible' : '') : moment(listaReferenciasPersonales[i].FechaAnalisis).format('YYYY/MM/DD hh:mm A')) + '"';

                        btnComentarioReferenciaPersonal = '<button type="button" id="btnComentarioReferencia" ' + stringDatas + ' class="btn btn-sm btn-info far fa-comments" title="Ver observaciones del departamento de crédito"></button>';
                        btnActualizarReferencia = '<button id="btnActualizarReferencia" ' + stringDatas + ' onclick="AbrirModalActualizarReferenciaPersonal(this)" class="btn btn-sm btn-info far fa-edit" type="button" title="Editar"></button>';
                        btnEliminarReferencia = '<button id="btnEliminarReferencia" ' + stringDatas + ' onclick="AbrirModalEliminarReferenciaPersonal(' + listaReferenciasPersonales[i].IdReferencia + ')" class="btn btn-sm btn-danger far fa-trash-alt" type="button" title="Eliminar"></button>';
                        colorClass = listaReferenciasPersonales[i].ComentarioDeptoCredito != '' ? listaReferenciasPersonales[i].SinComunicacion != true ? 'tr-exito' : 'text-danger' : '';
                        estado = listaReferenciasPersonales[i].ComentarioDeptoCredito != '' ? listaReferenciasPersonales[i].SinComunicacion != true ? '<i class="far fa-check-circle" title="Validación realizada"></i>' : '<i class="fas fa-phone-slash" title="Sin comunicación"></i>' : '<i class="fas fa-phone" title="Validación pendiente"></i>';

                        templateReferenciasPersonales += '<tr class=' + colorClass + '><td>' + listaReferenciasPersonales[i].NombreCompleto + '</td><td>' + listaReferenciasPersonales[i].LugarTrabajo + '</td><td>' + listaReferenciasPersonales[i].TiempoDeConocer + '</td><td>' + listaReferenciasPersonales[i].TelefonoReferencia + '</td><td>' + listaReferenciasPersonales[i].DescripcionParentesco + '</td><td class="text-center">' + estado + '</td><td class="text-center">' + btnComentarioReferenciaPersonal + ' ' + btnActualizarReferencia + ' ' + btnEliminarReferencia + '</td></tr>';
                    }
                    tblReferenciasPersonales.append(templateReferenciasPersonales);

                    /* Inicializar datatables de Referencias personales de la solicitud */
                    $('#tblReferenciasPersonales').DataTable({
                        "destroy": true,
                        "pageLength": 10,
                        "aaSorting": [],
                        "responsive": true,
                        "dom": "<'row'<'col-sm-12'>>" +
                            "<'row'<'col-sm-12'tr>>" +
                            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
                        "language": datatableLanguage,
                        columnDefs: [
                            { targets: 'no-sort', orderable: false },
                        ]
                    });
                }
            }
        }
    });
}

// #endregion

// #region validaciones de analisis

var descripcionTipoDeValidacion = '';
var btnValidacion = '';

/* Validaciones de analisis, información del cliente y la solicitud */
function ValidacionDeAnalisis(element) {

    btnValidacion = $(element);
    descripcionTipoDeValidacion = btnValidacion.data('validacion');

    $("#txtComentarioValidacionDeAnalisis").val('');

    $($("#frmPrincipal")).parsley().reset({ group: 'validacionDeAnalisis' });

    $("#modalValidacionDeAnalisis").modal();
}

/* Validaciones de la documentación de la solicitud */
function ValidacionDeDocumentacion(element) {

    btnValidacion = $(element);
    descripcionTipoDeValidacion = btnValidacion.data('validacion');

    let documentosPendientesDeValidar = 0;

    if (ESTADO_SOLICITUD.DocumentacionIdentidadesValidada != 1)
        documentosPendientesDeValidar++;

    if (ESTADO_SOLICITUD.DocumentacionDomicilioValidada != 1)
        documentosPendientesDeValidar++;

    if (ESTADO_SOLICITUD.DocumentacionLaboralValidada != 1)
        documentosPendientesDeValidar++;

    if (ESTADO_SOLICITUD.DocumentacionSolicitudFisicaValidada != 1)
        documentosPendientesDeValidar++;

    /* Si unicamente falta validar un tipo de documentación mostrar modal y solicitar comentario */
    if (documentosPendientesDeValidar == 1) {

        $("#txtComentarioValidacionDeAnalisis").val('');

        $($("#frmPrincipal")).parsley().reset({ group: 'validacionDeAnalisis' });

        $("#modalDocumentacion").modal('hide');
        $("#modalValidacionDeAnalisis").modal();
    }
    else
        RealizarValidacion();
}

$("#btnValidacionDeAnalisisConfirmar").click(function () {

    if ($($('#frmPrincipal')).parsley().isValid({ group: 'validacionDeAnalisis' }))
        RealizarValidacion();
    else
        $($("#frmPrincipal")).parsley().validate({ group: 'validacionDeAnalisis', force: true });
});

function RealizarValidacion() {

    btnValidacion.prop('disabled', true);
    DeshabilitarElementoPorId('btnValidacionDeAnalisisConfirmar');

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ValidacionesDeAnalisis',
        data: JSON.stringify({ tipoDeValidacion: descripcionTipoDeValidacion, comentario: $("#txtComentarioValidacionDeAnalisis").val(), dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar validación, contacte al administrador.');
        },
        success: function (data) {

            if (data.d == true) {

                MensajeExito('La validación se realizó correctamente.');
                CargarDetallesDelProcesamientoDeLaSolicitud();
            }
            else
                MensajeError('Error al realizar validación, contacte al administrador.');

            $("#modalValidacionDeAnalisis").modal('hide');
        },
        complete: function (data) {
            btnValidacion.prop('disabled', false);
            HabilitarElementoPorId('btnValidacionDeAnalisisConfirmar');
        }
    });
}

// #endregion

// #region Enviar a solicitud a investigación campo

$("#btnEnviarACampo").click(function () {

    if (ESTADO_SOLICITUD.FechaEnvioARuta == PROCESO_PENDIENTE) {
        $("#modalEnviarACampo").modal();
    }
});

$("#btnEnviarACampoConfirmar").click(function () {

    if ($($("#txtComentariosParaGestoria")).parsley().isValid()) {

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/EnviarACampo",
            data: JSON.stringify({ observacionesDeCredito: $("#txtComentariosParaGestoria").val(), dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo enviar a investigación de campo, contacte al administrador');
            },
            success: function (data) {

                if (data.d != false) {

                    $("#btnEnviarACampo, #btnEnviarACampoConfirmar").prop('disabled', true).removeClass('btn-warning').addClass('btn-success').prop('title', 'La solicitud ya fue enviada a investigación de campo');

                    CargarDetallesDelProcesamientoDeLaSolicitud();

                    $("#modalEnviarACampo").modal('hide');

                    MensajeExito('La solicitud se envió a investigación de campo correctamente.');
                }
                else
                    MensajeError('Error al enviar la solicitud a investigación de campo, contacte al administrador');
            }
        });
    }
    else
        $($("#txtComentariosParaGestoria")).parsley().validate();
});

// #endregion

//#region Funciones de analisis

function CargarBuroExterno() {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ObtenerUrlEncriptado',
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar buro externo');
        },
        success: function (data) {
            window.open("http://portal.prestadito.corp/corefinanciero/Clientes/Precalificado_Analista.aspx?" + data.d, "_blank", "toolbar=yes, scrollbars=yes, resizable=yes, top=0, left=window.screen.availWidth/2, window.screen.availWidth/2,window.screen.availHeight");
        }
    });
}

function ValidarInformacionDePerfil() {

    if ($("#ddlTipoDeEmpresa option:selected").val() != '' && $("#ddlTipoDePerfil option:selected").val() != '' && $("#ddlTipoDeEmpleo option:selected").val() != '' && $("#ddlBuroActual option:selected").val() != '')
        $("#modalActualizarInformacionPerfil").modal();
}

function ActualizarInformacionDePerfil() {

    let ddlTipoDeEmpresa = $("#ddlTipoDeEmpresa option:selected").val();
    let ddlTipoDePerfil = $("#ddlTipoDePerfil option:selected").val();
    let ddlTipoDeEmpleo = $("#ddlTipoDeEmpleo option:selected").val();
    let ddlBuroActual = $("#ddlBuroActual option:selected").val();

    if (ddlTipoDeEmpresa != '' && ddlTipoDePerfil != '' && ddlTipoDeEmpleo != '' && ddlBuroActual != '') {

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ActualizarInformacionPerfil",
            data: JSON.stringify({ tipoEmpresa: ddlTipoDeEmpresa, tipoPerfil: ddlTipoDePerfil, tipoEmpleo: ddlTipoDeEmpleo, buroActual: ddlBuroActual, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo guardar la información de perfil, contacte al administrador');
            },
            success: function (data) {

                if (data.d != false) {

                    $("#ddlTipoDeEmpresa,#ddlTipoDePerfil,#ddlTipoDeEmpleo,#ddlBuroActual").prop('disabled', true);
                    $("#modalActualizarInformacionPerfil").modal('hide');
                    MensajeExito('La información de perfil se guardó correctamente');
                    CargarDetallesDelProcesamientoDeLaSolicitud();
                }
                else
                    MensajeError('No se pudo guardar la información de perfil, contacte al administrador');
            }
        });
    }
}

/* Actualizar ingresos del cliente */
$("#actualizarIngresosCliente").click(function () {
    $("#txtIngresosReales,#txtBonosComisiones").val('');
    $("#modalActualizarIngresos").modal();
});

$("#formActualizarIngresos").submit(function (e) {

    e.preventDefault();
    if ($($("#formActualizarIngresos")).parsley().isValid()) {

        var sueldoBaseReal = $("#txtIngresosReales").val().replace(/,/g, '');
        var bonosComisionesReal = $("#txtBonosComisiones").val().replace(/,/g, '');
        var obligaciones = ESTADO_SOLICITUD.fdObligacionesPrecalificado;
        var codigoProducto = ESTADO_SOLICITUD.fiIDTipoPrestamo;

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/ActualizarIngresosCliente',
            data: JSON.stringify({ sueldoBaseReal: sueldoBaseReal, bonosComisionesReal: bonosComisionesReal, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al actualizar ingresos');
            },
            success: function (data) {
                if (data.d == true) {

                    $('#lblIngresosMensualesCliente').text('');
                    var ingresos = parseFloat(sueldoBaseReal) + parseFloat(bonosComisionesReal);
                    $('#lblIngresosMensualesCliente').text(ConvertirAFormatoNumerico(ingresos));
                    var totalIngresosReales = ingresos;
                    $("#lblIngresosReales").text(ConvertirAFormatoNumerico(totalIngresosReales));
                    $('#lblObligacionesReales').text(ConvertirAFormatoNumerico(obligaciones));
                    $('#lblDisponibleReal').text(ConvertirAFormatoNumerico(totalIngresosReales - obligaciones));
                    var capacidadPagoReal = calcularCapacidadPago(codigoProducto, obligaciones, totalIngresosReales);
                    $('#lblCapacidadPagoMensualReal').text(ConvertirAFormatoNumerico(capacidadPagoReal));
                    $('#lblCapacidadPagoQuincenalReal').text(ConvertirAFormatoNumerico(capacidadPagoReal / 2));
                    $("#divRecalculoReal").css('display', '');
                    MensajeExito('Ingresos actualizados correctamente');
                    cargarPrestamosSugeridos(ESTADO_SOLICITUD.fnValorGarantia, ESTADO_SOLICITUD.fnPrima);
                    $("#modalActualizarIngresos").modal('hide');
                }
                else {
                    MensajeError('Error al actualizar ingresos');
                }
            }
        });
    } else { $($("#formActualizarIngresos")).parsley().validate(); }
});

$("#btnDigitarMontoManualmente,#btnDigitarValoresManualmente").click(function () {
    $("#txtMontoManual,#txtPlazoManual,#txtMontoPrimaManual").val('');
    $("#ModalDigitarMontosManualmente").modal();
});

// -- DEBUGGEANDO ESTE
$('#txtValorGlobalManual,#txtValorPrimaManual,#txtValorPlazoManual').blur(function () {

    /* Calcular Cuota y valor a Financiar */
    var valorGlobal = $("#txtValorGlobalManual").val().replace(/,/g, '') == '' ? 0 : $('#txtValorGlobalManual').val().replace(/,/g, '');
    valorGlobal = parseFloat(valorGlobal);

    var valorPrima = $("#txtValorPrimaManual").val().replace(/,/g, '') == '' ? 0 : $('#txtValorPrimaManual').val().replace(/,/g, '');
    valorPrima = parseFloat(valorPrima);

    var cantidadPlazos = $("#txtValorPlazoManual").val() == '' ? 0 : $('#txtValorPlazoManual').val();
    cantidadPlazos = parseInt(cantidadPlazos);

    var montoFinanciar = valorGlobal - valorPrima;

    if (valorGlobal > 0 && valorPrima >= 0 && valorGlobal > valorPrima && cantidadPlazos > 0 && montoFinanciar > 0) {

        $('#divCalculandoCuotaManual').css('display', '');

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/CalculoPrestamo',
            data: JSON.stringify({ ValorPrestamo: valorGlobal, ValorPrima: valorPrima, CantidadPlazos: cantidadPlazos, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al calcular cuota');
                $('#divCalculandoCuotaManual').css('display', 'none');
                $('#btnActualizarMontoManualmente').prop('disabled', false);
            },
            success: function (data) {
                if (data.d != null) {
                    $('#lblTituloCantidadCuotaManual').text(cantidadPlazos + ' Cuotas ' + data.d.TipoCuota);
                    $('#txtValorCuotaManual').val(data.d.CuotaQuincenal);
                    if (data.d.CuotaMensualNeta != 0) {
                        $('#txtValorCuotaManual').val(data.d.CuotaMensualNeta);
                    }
                    $("#txtMontoaFinanciarManual").val(data.d.ValoraFinanciar);
                    $('#divMostrarCalculoCuotaManual').css('display', '');
                    montoFinanciarCalculado = data.d.ValoraFinanciar;
                }
                else {
                    MensajeError('No se pudo calcular la cuota');
                }
                $('#divCalculandoCuotaManual').css('display', 'none');
                $('#btnActualizarMontoManualmente').prop('disabled', false);
            }
        });
    }
});

/* DEBUGUEAR ESTE */
var montoFinanciarCalculado = 0;
$("#btnActualizarMontoManualmente").click(function () {
    var valorGlobal = $("#txtValorGlobalManual").val().replace(/,/g, '') == '' ? 0 : $('#txtValorGlobalManual').val().replace(/,/g, '');
    valorGlobal = parseFloat(valorGlobal);

    var valorPrima = $("#txtValorPrimaManual").val().replace(/,/g, '') == '' ? 0 : $('#txtValorPrimaManual').val().replace(/,/g, '');
    valorPrima = parseFloat(valorPrima);

    var cantidadPlazos = $("#txtValorPlazoManual").val() == '' ? 0 : $('#txtValorPlazoManual').val();
    cantidadPlazos = parseInt(cantidadPlazos);

    if (valorGlobal > 0 && valorPrima >= 0 && valorGlobal > valorPrima && cantidadPlazos > 0 && montoFinanciarCalculado > 0) {

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/ActualizarPlazoMontoFinanciar',
            data: JSON.stringify({ ValorGlobal: valorGlobal, ValorPrima: valorPrima, CantidadPlazos: cantidadPlazos, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al actualizar valores del préstamo');
            },
            success: function (data) {
                if (data.d == true) {
                    $("#lblMontoPrestamoEscogido").text(ConvertirAFormatoNumerico(montoFinanciarCalculado));
                    $("#lblPlazoEscogido").text(cantidadPlazos);
                    gMontoFinal = montoFinanciarCalculado;
                    gPlazoFinal = cantidadPlazos;
                    $("#divPrestamoElegido").css('display', '');
                    $("#modalMontoFinanciar,#ModalDigitarMontosManualmente").modal('hide');
                    MensajeExito('Monto a financiar actualizado correctamente');

                    VALORGARANTIA_CARGADA_EN_PRESTAMOSSUGERIDOS = valorGlobal;
                    VALORPRIMA_CARGADA_EN_PRESTAMOSSUGERIDOS = valorPrima;
                }
                else { MensajeError('Error al actualizar ingresos'); }
            }
        });
    }
    else {
        $($("#txtMontoManual")).parsley().validate();
        $($("#txtPlazoManual")).parsley().validate();
    }
});

/* Actualizar el monto y plazo a Financiar de la lista de prestamos sugeridos */
// DEBUGGEAR ESTE
var prestamoSeleccionado = [];
$(document).on('click', 'button#btnSeleccionarPMO', function () {
    prestamoSeleccionado = {
        fnMontoOfertado: $(this).data('monto'),
        fiPlazo: $(this).data('plazo'),
        fnCuotaQuincenal: $(this).data('cuota'),
    }
    $("#modalMontoFinanciar").modal();
});

// DEBUGGEAR ESTE
$("#btnConfirmarPrestamoAprobado").click(function () {

    var pmo = prestamoSeleccionado;
    var valorGlobal = pmo.fnMontoOfertado;
    var cantidadPlazos = pmo.fiPlazo;

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ActualizarPlazoMontoFinanciar',
        data: JSON.stringify({ ValorGlobal: VALORGARANTIA_CARGADA_EN_PRESTAMOSSUGERIDOS, ValorPrima: VALORPRIMA_CARGADA_EN_PRESTAMOSSUGERIDOS, CantidadPlazos: cantidadPlazos, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al actualizar ingresos');
        },
        success: function (data) {
            if (data.d == true) {
                $("#lblMontoPrestamoEscogido").text(valorGlobal);
                $("#lblPlazoEscogido").text(cantidadPlazos);
                gMontoFinal = valorGlobal;
                gPlazoFinal = cantidadPlazos;
                $("#divPrestamoElegido").css('display', '');
                $("#modalMontoFinanciar").modal('hide');
                MensajeExito('Monto a financiar actualizado correctamente');
            }
            else { MensajeError('Error al actualizar ingresos'); }
        }
    });
});

//#endregion Funciones de analisis


// #region Calculos

function prestamoEfectivo(plazoQuincenal, prestamoAprobado) {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/CalculoPrestamo',
        data: JSON.stringify({ ValorPrestamo: prestamoAprobado, ValorPrima: '0', CantidadPlazos: plazoQuincenal, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar calculo del préstamo');
            gMontoFinal = 0;
            gPlazoFinal = 0;
        },
        success: function (data) {

            var objCalculo = data.d;
            if (objCalculo != null) {

                /* Variables globales */
                gMontoFinal = objCalculo.ValoraFinanciar;
                gPlazoFinal = plazoQuincenal;
                $("#lblMontoFinanciarEfectivo").text(ConvertirAFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblMontoCuotaEfectivo").text(ConvertirAFormatoNumerico(objCalculo.CuotaQuincenal));
                $("#lblTituloCuotaEfectivo").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota);

                /* Mostrar div del calculo del prestamo efectivo */
                $("#lblPrima").css('display', 'none');
                $("#lblMontoPrima").css('display', 'none');
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                $("#divPrestamoEfectivo").css('display', '');

                //$('#lblResumenValorGarantia').text(ConvertirAFormatoNumerico(rowDataSolicitud.fdValorPmoSugeridoSeleccionado)); // Ficha de resumen
                //$('#lblResumenValorPrima').text(ConvertirAFormatoNumerico(rowDataSolicitud.fdValorPmoSugeridoSeleccionado)); // Ficha de resumen
                $('#lblResumenValorFinanciar').text(ConvertirAFormatoNumerico(objCalculo.ValoraFinanciar)); // ficha de resumen
                $('#lblResumenCuota').text(ConvertirAFormatoNumerico(objCalculo.CuotaQuincenal)); // ficha de resumen
                $("#lblResumenCuotaTitulo").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota); // ficha de resumen
            } else {
                MensajeError('Error al realizar calculo del préstamo');
                gMontoFinal = 0;
                gPlazoFinal = 0;
            }
        }
    });
}

function prestamoMoto(ValorPrima, valorDeLaMoto, plazoQuincenal) {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/CalculoPrestamo',
        data: JSON.stringify({ ValorPrestamo: valorDeLaMoto, ValorPrima: ValorPrima, CantidadPlazos: plazoQuincenal, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar calculo del préstamo');
            gMontoFinal = 0;
            gPlazoFinal = 0;
        },
        success: function (data) {

            var objCalculo = data.d;
            if (objCalculo != null) {

                /* variables globales */
                gMontoFinal = objCalculo.ValoraFinanciar;
                gPlazoFinal = plazoQuincenal;
                $("#lblMontoFinanciarMoto").text(ConvertirAFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblTituloCuotaMoto").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota);
                $("#lblMontoCuotaMoto").text(ConvertirAFormatoNumerico(objCalculo.CuotaQuincenal));

                /* Mostrar div del calculo del prestamo moto */
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                $("#divPrestamoMoto").css('display', '');

                $("#lblResumenValorGarantiaTitulo,#lblResumenValorGarantia, #lblResumenValorPrimaTitulo,#lblResumenValorPrima").css('display', '');
                $('#lblResumenValorGarantia').text(ConvertirAFormatoNumerico(valorDeLaMoto)); // Ficha de resumen
                $('#lblResumenValorPrima').text(ConvertirAFormatoNumerico(ValorPrima)); // Ficha de resumen
                $('#lblResumenValorFinanciar').text(ConvertirAFormatoNumerico(objCalculo.ValoraFinanciar)); // Ficha de resumen
                $('#lblResumenCuota').text(ConvertirAFormatoNumerico(objCalculo.CuotaQuincenal)); // Ficha de resumen
                $("#lblResumenCuotaTitulo").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota); //Ficha de resumen
            } else {
                MensajeError('Error al realizar calculo del préstamo');
                gMontoFinal = 0;
                gPlazoFinal = 0;
            }
        }
    });
}

function prestamoAuto(ValorPrima, valorDelAuto, plazoMensual) {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/CalculoPrestamo',
        data: JSON.stringify({ ValorPrestamo: valorDelAuto, ValorPrima: ValorPrima, CantidadPlazos: plazoMensual, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar calculo del préstamo');
            gMontoFinal = 0;
            gPlazoFinal = 0;
        },
        success: function (data) {

            var objCalculo = data.d;
            if (objCalculo != null) {
                /* variables globales */
                gMontoFinal = objCalculo.ValoraFinanciar;
                gPlazoFinal = plazoMensual;
                $("#lblMontoFinanciarAuto").text(ConvertirAFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblMontoCuotaTotalAuto").text(ConvertirAFormatoNumerico(objCalculo.CuotaMensualNeta));
                $("#lblTituloCuotaAuto").text(plazoMensual + ' Cuotas ' + objCalculo.TipoCuota);
                /* Mostrar div del calculo del prestamo auto */
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                $("#divPrestamoAuto").css('display', '');

                $("#lblResumenValorGarantiaTitulo,#lblResumenValorGarantia, #lblResumenValorPrimaTitulo,#lblResumenValorPrima").css('display', '');
                $('#lblResumenValorGarantia').text(ConvertirAFormatoNumerico(valorDelAuto)); // ficha de resumen
                $('#lblResumenValorPrima').text(ConvertirAFormatoNumerico(ValorPrima)); // ficha de resumen
                $('#lblResumenValorFinanciar').text(ConvertirAFormatoNumerico(objCalculo.ValoraFinanciar)); // ficha de resumen
                $('#lblResumenCuota').text(ConvertirAFormatoNumerico(objCalculo.CuotaMensualNeta)); // ficha de resumen
                $("#lblResumenCuotaTitulo").text(plazoMensual + ' Cuotas ' + objCalculo.TipoCuota); // ficha de resumen

            } else {
                MensajeError('Error al realizar calculo del préstamo');
                gMontoFinal = 0;
                gPlazoFinal = 0;
            }
        }
    });
}

var VALORPRIMA_CARGADA_EN_PRESTAMOSSUGERIDOS = 0;
var VALORGARANTIA_CARGADA_EN_PRESTAMOSSUGERIDOS = 0;

function cargarPrestamosSugeridos(ValorProducto, ValorPrima) {

    $("#cargandoPrestamosSugeridosReales").css('display', '');
    var tablaPrestamos = $("#tblPMOSugeridosReales tbody");
    tablaPrestamos.empty();
    MensajeInformacion('Cargando préstamos sugeridos');

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/GetPrestamosSugeridos',
        data: JSON.stringify({ ValorProducto: ValorProducto, ValorPrima: ValorPrima, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar préstamos sugeridos');
            $("#cargandoPrestamosSugeridosReales").css('display', 'none');
        },
        success: function (data) {
            if (data.d != null) {
                var objPrestamos = data.d;
                if (objPrestamos.cotizadorProductos.length > 0) {
                    VALORPRIMA_CARGADA_EN_PRESTAMOSSUGERIDOS = ValorPrima;
                    VALORGARANTIA_CARGADA_EN_PRESTAMOSSUGERIDOS = ValorProducto;

                    var listaPmos = data.d.cotizadorProductos;
                    for (var i = 0; i < listaPmos.length; i++) {
                        var botonSeleccionarPrestamo = '<button id="btnSeleccionarPMO" data-monto="' + listaPmos[i].fnMontoOfertado + '" data-plazo="' + listaPmos[i].fiPlazo + '" data-cuota="' + listaPmos[i].fnCuotaQuincenal + '" class="btn mdi mdi-pencil mdi-24px text-info"></button>';
                        tablaPrestamos.append('<tr>'
                            + '<td class="FilaCondensada">' + listaPmos[i].fnMontoOfertado + '</td>'
                            + '<td class="FilaCondensada">' + listaPmos[i].fiPlazo + '</td>'
                            + '<td class="FilaCondensada">' + listaPmos[i].fnCuotaQuincenal + '</td>'
                            + '<td class="FilaCondensada">' + botonSeleccionarPrestamo + '</td>'
                            + '</tr>');
                    }
                    $("#divSinCapacidadPago").css('display', 'none');
                    $("#btnDigitarMontoManualmente, #btnRechazarIncapacidadPagoModal").prop('disabled', true);
                    $("#cargandoPrestamosSugeridosReales").css('display', 'none');
                    $("#lbldivPrestamosSugeridosReales,#tblPMOSugeridosReales,#divPrestamosSugeridosReales").css('display', '');
                }
                else {
                    $("#cargandoPrestamosSugeridosReales").css('display', 'none');
                    $("#btnDigitarMontoManualmente, #btnRechazarIncapacidadPagoModal").prop('disabled', false);
                    $("#tblPMOSugeridosReales,#lbldivPrestamosSugeridosReales,#divPrestamosSugeridosReales").css('display', 'none');
                    $("#divSinCapacidadPago").css('display', '');
                }
            } else { MensajeError('Error al cargar préstamos sugeridos'); }
        }
    });
}

// #endregion Calculos


// #region Aprobar solicitud

$("#btnAprobar").click(function () {
    resolucion = true;
    $($("#comentarioAprobar")).parsley().reset();
    $("#comentarioAprobar").val('');
    $("#modalAprobar").modal();
});

$("#btnConfirmarAprobar").click(function () {

    if ($($("#comentarioAprobar")).parsley().isValid()) {

        if (resolucionHabilitada == false) {
            MensajeError('Todavía no se puede determinar una resolución para esta solicitud, tiene procesos sin concluir');
        }
        else if (resolucionHabilitada == true) {

            if ($($("#frmInfoAnalisis")).parsley().isValid()) {

                var objBitacora = {
                    fcComentarioResolucion: $("#comentarioAprobar").val()
                };
                var solicitud = {
                    fiEstadoSolicitud: 7,
                    fiMontoFinalSugerido: gMontoFinal,
                    fiMontoFinalFinanciar: gMontoFinal,
                    fiPlazoFinalAprobado: gPlazoFinal
                };

                $.ajax({
                    type: "POST",
                    url: "SolicitudesCredito_Analisis.aspx/SolicitudResolucion",
                    data: JSON.stringify({ objBitacora: objBitacora, ESTADO_SOLICITUD: solicitud, dataCrypt: window.location.href }),
                    contentType: 'application/json; charset=utf-8',
                    error: function (xhr, ajaxOptions, thrownError) {
                        MensajeError('Ocurrió un error, contacte al administrador');
                    },
                    success: function (data) {

                        if (data.d != 0) {
                            /* Informacion del analisis */
                            $("#tipoEmpresa").prop('disabled', true);
                            $("#tipoPerfil").prop('disabled', true);
                            $("#tipoEmpleo").prop('disabled', true);
                            $("#buroActual").prop('disabled', true);
                            $("#montoFinalAprobado").prop('disabled', true);
                            $("#montoFinalFinanciar").prop('disabled', true);
                            $("#plazoFinalAprobado").prop('disabled', true);
                            $("#modalAprobar").modal('hide');
                            $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud").prop('disabled', true);
                            $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud").attr('title', 'La solicitud ya fue aprobada');
                            $("#actualizarIngresosCliente").css('display', 'none');
                            $("#pencilOff").css('display', '');
                            MensajeExito('¡Solicitud aprobada correctamente!');
                            actualizarEstadoSolicitud();
                        }
                        else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
                    }
                });
            }
            else {
                MensajeError('Información de análisis incompleta');
                $($("#frmInfoAnalisis")).parsley().validate();
            }
        }
    }
    else { $($("#comentarioAprobar")).parsley().validate(); }
});

// #endregion Aprobar solicitud


// #region Rechazar solicitud

$("#btnRechazar").click(function () {
    resolucion = false;
    $($("#comentarioRechazar")).parsley().reset();
    $("#comentarioRechazar").val('');
    $("#modalRechazar").modal();
});

$("#btnConfirmarRechazar").click(function () {

    if ($($("#comentarioRechazar")).parsley().isValid()) {

        var objBitacora = {
            fcComentarioResolucion: $("#comentarioRechazar").val()
        };
        var solicitud = {
            fiEstadoSolicitud: 4
        };

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/SolicitudResolucion",
            data: JSON.stringify({ objBitacora: objBitacora, ESTADO_SOLICITUD: solicitud, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Ocurrió un error, contacte al administrador');
            },
            success: function (data) {

                if (data.d != 0) {
                    /* Informacion del analisis */
                    $("#tipoEmpresa").prop('disabled', true);
                    $("#tipoEmpresa").val('');
                    $("#tipoPerfil").prop('disabled', true);
                    $("#tipoPerfil").val('');
                    $("#tipoEmpleo").prop('disabled', true);
                    $("#tipoEmpleo").val('');
                    $("#buroActual").prop('disabled', true);
                    $("#buroActual").val('');
                    $("#montoFinalAprobado").prop('disabled', true);
                    $("#montoFinalAprobado").val('');
                    $("#montoFinalFinanciar").prop('disabled', true);
                    $("#montoFinalFinanciar").val('');
                    $("#plazoFinalAprobado").prop('disabled', true);
                    $("#plazoFinalAprobado").val('');
                    $("#modalRechazar").modal('hide');
                    $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud").prop('disabled', true);
                    $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud").prop('title', 'La solicitud ya fue rechazada');
                    $("#actualizarIngresosCliente").css('display', 'none');
                    $("#pencilOff").css('display', '');
                    MensajeExito('¡Solicitud rechazada correctamente!');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    }
    else {
        $($("#comentarioRechazar")).parsley().validate();
    }
});

/* Cuando la nueva capacidad de pago es insuficiente */
$("#btnRechazarIncapPagoConfirmar").click(function () {

    var objBitacora = {
        fcComentarioResolucion: 'Rechazado por incapcidad de pago'
    };
    var solicitud = {
        fiEstadoSolicitud: 4
    };

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/SolicitudResolucion",
        data: JSON.stringify({ objBitacora: objBitacora, ESTADO_SOLICITUD: solicitud, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Ocurrió un error al determinar resolucion de la solicitud, contacte al administrador');
        },
        success: function (data) {
            if (data.d != 0) {

                /* Informacion del analisis */
                $("#tipoEmpresa").prop('disabled', true);
                $("#tipoEmpresa").val('');
                $("#tipoPerfil").prop('disabled', true);
                $("#tipoPerfil").val('');
                $("#tipoEmpleo").prop('disabled', true);
                $("#tipoEmpleo").val('');
                $("#buroActual").prop('disabled', true);
                $("#buroActual").val('');
                $("#montoFinalAprobado").prop('disabled', true);
                $("#montoFinalAprobado").val('');
                $("#montoFinalFinanciar").prop('disabled', true);
                $("#montoFinalFinanciar").val('');
                $("#plazoFinalAprobado").prop('disabled', true);
                $("#plazoFinalAprobado").val('');
                $("#modalRechazar,#modalRechazarPorIncapcidadPago").modal('hide');
                $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud,#btnDigitarMontoManualmente,#btnDigitarValoresManualmente,#btnRechazarIncapacidadPagoModal,#btnRechazarIncapPagoConfirmar").prop('disabled', true);
                $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud,#btnDigitarMontoManualmente,#btnDigitarValoresManualmente,#btnRechazarIncapacidadPagoModal,#btnRechazarIncapPagoConfirmar").prop('title', 'La solicitud ya fue rechazada');
                $("#actualizarIngresosCliente").css('display', 'none');
                $("#pencilOff").css('display', '');
                MensajeExito('¡Solicitud rechazada correctamente!');
                actualizarEstadoSolicitud();
            }
            else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
        }
    });
});

// #endregion Rechazar solicitud


//#region Funciones utilitarias

countdown.setLabels(
    'ms | seg | min | hr | d | sem | mes |año | dec | sig | mil ',
    'ms | seg | min | hr | d | sem | mes |año | dec | sig | mil ',
    ' y ',
    ' : ',
    'ahora',
    function (n) { return n.toString(); });

function InicializarContador(fechaInicio, fechaFin, identificadorEtiqueta) {

    var fechaInicial = fechaInicio == '/Date(-2208967200000)/' ? null : new Date(parseInt(fechaInicio.substr(6, 19)));

    var fechaFinal = fechaFin == '/Date(-2208967200000)/' ? null : new Date(parseInt(fechaFin.substr(6, 19)));

    /* Verificar si ya se determinó una resolución para la solicitud y el el proceso se quedó pendiente y */
    if (fechaInicio != '/Date(-2208967200000)/' && fechaFin == '/Date(-2208967200000)/' && (ID_ESTADO_SOLICITUD == '7' || ID_ESTADO_SOLICITUD == '5' || ID_ESTADO_SOLICITUD == '4')) {

        document.getElementById('' + identificadorEtiqueta + '').innerHTML = '-';
    }
    else if (fechaFin == '/Date(-2208967200000)/' && fechaInicio == '/Date(-2208967200000)/') { /* Verificar si el proceso todavía no ha empezado */

        document.getElementById('' + identificadorEtiqueta + '').innerHTML = '-';
    }
    else if (fechaFin != '/Date(-2208967200000)/' && fechaInicio != '/Date(-2208967200000)/') { /* Verificar si el proceso ya empezó y finalizó */

        document.getElementById('' + identificadorEtiqueta + '').innerHTML = countdown(
            fechaInicial,
            fechaFinal,
            countdown.YEARS | countdown.MONTHS | countdown.WEEKS | countdown.DAYS | countdown.HOURS | countdown.MINUTES | countdown.SECONDS
        ).toHTML("");
    }
    else if (fechaInicio != '/Date(-2208967200000)/') { /* Verificar si el proceso ya empezó y todavía no ha finalizado */

        countdown(
            fechaInicial,
            function (ts) {

                document.getElementById('' + identificadorEtiqueta + '').innerHTML = ts.toHTML("");
            },
            fechaFinal,
            countdown.YEARS | countdown.MONTHS | countdown.WEEKS | countdown.DAYS | countdown.HOURS | countdown.MINUTES | countdown.SECONDS
        );
    }
}

function ObtenerFechaFormateada(fecha) {

    return fecha == '/Date(-2208967200000)/' ? '-' : moment(fecha).locale('es').format('YYYY/MM/DD hh:mm:ss A');
}

function ValidarFecha(fecha) {

    return fecha == '/Date(-2208967200000)/' ? null : fecha;
}

function MensajeInformacion(mensaje) {
    iziToast.info({
        title: 'Info',
        message: mensaje
    });
}

function MensajeExito(mensaje) {
    iziToast.success({
        title: 'Exito',
        message: mensaje
    });
}

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function ConvertirAFormatoNumerico(nStr) {

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

function DeshabilitarElementoPorId(idElemento) {
    $('#' + idElemento + '').prop('disabled', true);
};

function HabilitarElementoPorId(idElemento) {
    $('#' + idElemento + '').prop('disabled', false);
};

function MostrarLoader() {
    $("#imgLogo").css('display', 'none');
    $("#divCargandoAnalisis").css('display', '');
}

function OcultarLoader() {
    $("#divCargandoAnalisis").css('display', 'none');
    $("#imgLogo").css('display', '');
}

//#endregion Funciones utilitarias


// #region Otras funciones

$(window).on('hide.bs.modal', function () { /* cuando se abra un modal, ocultar el scroll del BODY y deja solo el del modal (en caso de que este tenga scroll) */

    const body = document.body;
    const scrollY = body.style.top;
    body.style.position = '';
    body.style.top = '';
    window.scrollTo(0, parseInt(scrollY || '0') * -1);
    $("body").css('padding-right', '0');
});

$(window).on('show.bs.modal', function () { /* Cuando se cierre el modal */

    const scrollY = document.documentElement.style.getPropertyValue('--scroll-y');
    const body = document.body;
    body.style.position = 'fixed';
    body.style.top = `-${scrollY}`;
    $("body").css('padding-right', '0');
});

window.addEventListener('scroll', () => {
    document.documentElement.style.setProperty('--scroll-y', `${window.scrollY}px`);
});

// #endregion Otras funciones
