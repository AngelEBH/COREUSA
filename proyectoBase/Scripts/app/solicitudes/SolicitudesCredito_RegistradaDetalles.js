var idSolicitud = 0;
var objSolicitud = [];
var resolucionHabilitada = false;

/* modal de detalles del estado del procesamiento de la solicitud */
$('#btnMasDetalles').click(function () {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_RegistradaDetalles.aspx/CargarEstadoSolicitud",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ dataCrypt: window.location.href }),
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            if (data.d != null) {
                debugger;
                var informacionSolicitud = data.d;

                var ProcesoPendiente = '/Date(-2208967200000)/';

                /* limpiar tabla de detalles del estado de la solicitud */
                var tablaEstatusSolicitud = $('#tblDetalleEstado tbody');
                tablaEstatusSolicitud.empty();

                var enIngresoInicio = ObtenerFechaFormateada(informacionSolicitud.EnIngresoInicio);
                var enIngresoFin = ObtenerFechaFormateada(informacionSolicitud.EnIngresoFin);
                var tiempoEnIngreso = InicializarContador(informacionSolicitud.EnIngresoInicio, informacionSolicitud.EnIngresoFin,'');

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Ingreso</td>' +
                    '<td>' + enIngresoInicio + '</td>' +
                    '<td>' + enIngresoFin + '</td>' +
                    '<td>' + tiempoEnIngreso + '</td>' +
                    '</tr>');

                //var EnTramiteInicio = rowDataSolicitud.fdEnTramiteInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnTramiteInicio) : '';
                //var EnTramiteFin = rowDataSolicitud.fdEnTramiteFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnTramiteFin) : '';
                //var tiempoTramite = EnTramiteInicio != '' ? EnTramiteFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnTramiteInicio, rowDataSolicitud.fdEnTramiteFin) : '' : '';

                //tablaEstatusSolicitud.append('<tr>' +
                //    '<td>Recepción</td>' +
                //    '<td>' + EnTramiteInicio + '</td>' +
                //    '<td>' + EnTramiteFin + '</td>' +
                //    '<td>' + tiempoTramite + '</td>' +
                //    '</tr>');

                //var EnAnalisisInicio = rowDataSolicitud.fdEnAnalisisInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnAnalisisInicio) : '';
                //var EnAnalisisFin = rowDataSolicitud.fdEnAnalisisFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnAnalisisFin) : '';
                //var tiempoAnalisis = EnAnalisisInicio != '' ? EnAnalisisFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnAnalisisInicio, rowDataSolicitud.fdEnAnalisisFin) : '' : '';

                //tablaEstatusSolicitud.append('<tr>' +
                //    '<td>Análisis</td>' +
                //    '<td>' + EnAnalisisInicio + '</td>' +
                //    '<td>' + EnAnalisisFin + '</td>' +
                //    '<td>' + tiempoAnalisis + '</td>' +
                //    '</tr>');

                //var EnCampoInicio = rowDataSolicitud.fdEnvioARutaAnalista != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnvioARutaAnalista) : '';
                //var EnCampoFin = rowDataSolicitud.fdEnCampoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnCampoFin) : '';
                //var tiempoCampo = EnCampoInicio != '' ? EnCampoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnvioARutaAnalista, rowDataSolicitud.fdEnCampoFin) : '' : '';
                //tiempoCampo == '' ? habilitarResolucion = false : '';

                //tablaEstatusSolicitud.append('<tr>' +
                //    '<td>Campo</td>' +
                //    '<td>' + EnCampoInicio + '</td>' +
                //    '<td>' + EnCampoFin + '</td>' +
                //    '<td>' + tiempoCampo + '</td>' +
                //    '</tr>');

                //var CondicionadoInicio = rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondicionadoInicio) : '';
                //var CondificionadoFin = rowDataSolicitud.fdCondificionadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondificionadoFin) : '';
                //var tiempoCondicionado = CondicionadoInicio != '' ? CondificionadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdCondicionadoInicio, rowDataSolicitud.fdCondificionadoFin) : '' : '';

                //tablaEstatusSolicitud.append('<tr>' +
                //    '<td>Condicionado</td>' +
                //    '<td>' + CondicionadoInicio + '</td>' +
                //    '<td>' + CondificionadoFin + '</td>' +
                //    '<td>' + tiempoCondicionado + '</td>' +
                //    '</tr>');

                //var ReprogramadoInicio = rowDataSolicitud.fdReprogramadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoInicio) : '';
                //var ReprogramadoFin = rowDataSolicitud.fdReprogramadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoFin) : '';
                //var tiempoReprogramado = ReprogramadoInicio != '' ? ReprogramadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdReprogramadoInicio, rowDataSolicitud.fdReprogramadoFin) : '' : '';

                //tablaEstatusSolicitud.append('<tr>' +
                //    '<td>Reprogramado</td>' +
                //    '<td>' + ReprogramadoInicio + '</td>' +
                //    '<td>' + ReprogramadoFin + '</td>' +
                //    '<td>' + tiempoReprogramado + '</td>' +
                //    '</tr>');


                //var ValidacionInicio = rowDataSolicitud.PasoFinalInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.PasoFinalInicio) : '';
                //var ValidacionFin = rowDataSolicitud.PasoFinalFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.PasoFinalFin) : '';
                //var tiempoValidacion = ValidacionInicio != '' ? ValidacionFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.PasoFinalInicio, rowDataSolicitud.PasoFinalFin) : '' : '';

                //tablaEstatusSolicitud.append('<tr>' +
                //    '<td>Validación</td>' +
                //    '<td>' + ValidacionInicio + '</td>' +
                //    '<td>' + ValidacionFin + '</td>' +
                //    '<td>' + tiempoValidacion + '</td>' +
                //    '</tr>');

                //var timer = rowDataSolicitud.fcTiempoTotalTranscurrido.split(':');
                //tablaEstatusSolicitud.append('<tr>' +
                //    '<td colspan="4" class="text-center">Tiempo total transcurrido: <strong>' + timer[0] + ' horas con ' + timer[1] + ' minutos y ' + timer[2] + ' segundos </strong></td>' +
                //    '</tr >');

                ///* Verificar si ya se tomó una resolución de la solicitud */
                //if (rowDataSolicitud.fiEstadoSolicitud == 7) {
                //    $("#lblResolucion").text('Aprobado');
                //    $("#lblResolucion").removeClass('text-warning');
                //    $("#lblResolucion").addClass('text-success');
                //}
                //else if (rowDataSolicitud.fiEstadoSolicitud == 4) {
                //    $("#lblResolucion").text('Rechazado por analistas');
                //    $("#lblResolucion").removeClass('text-warning');
                //    $("#lblResolucion").addClass('text-danger');
                //}
                //else if (rowDataSolicitud.fiEstadoSolicitud == 5) {
                //    $("#lblResolucion").text('Rechazado por gestores');
                //    $("#lblResolucion").removeClass('text-warning');
                //    $("#lblResolucion").addClass('text-danger');
                //}

                //else if (rowDataSolicitud.fdEnTramiteFin != '/Date(-2208967200000)/') {
                //    $("#lblResolucion").text('En análisis');
                //}
                //var contadorComentariosDetalle = 0;

                ////razon reprogramado
                //if (rowDataSolicitud.fcReprogramadoComentario != '') {
                //    $("#lblReprogramado").text(rowDataSolicitud.fcReprogramadoComentario);
                //    $("#divReprogramado").css('display', '');
                //    contadorComentariosDetalle += 1;
                //}
                ////razon condicionado
                //if (rowDataSolicitud.fcCondicionadoComentario != '') {
                //    $("#lblCondicionado").text(rowDataSolicitud.fcCondicionadoComentario);
                //    $("#divCondicionado").css('display', '');
                //    contadorComentariosDetalle += 1;
                //}
                ////observaciones documentacion
                //if (rowDataSolicitud.fcComentarioValidacionDocumentacion != '') {
                //    $("#lblDocumentacionComentario").text(rowDataSolicitud.fcComentarioValidacionDocumentacion);
                //    $("#divDocumentacionComentario").css('display', '');
                //    contadorComentariosDetalle += 1;
                //}
                ////observaciones info personal
                //if (rowDataSolicitud.fcComentarioValidacionInfoPersonal != '') {
                //    $("#lblInfoPersonalComentario").text(rowDataSolicitud.fcComentarioValidacionInfoPersonal);
                //    $("#divInfoPersonal").css('display', '');
                //    contadorComentariosDetalle += 1;
                //}
                ////observaciones info laboral
                //if (rowDataSolicitud.fcComentarioValidacionInfoLaboral != '') {
                //    $("#lblInfoLaboralComentario").text(rowDataSolicitud.fcComentarioValidacionInfoLaboral);
                //    $("#divInfoLaboral").css('display', '');
                //    contadorComentariosDetalle += 1;
                //}
                ////observaciones referencias
                //if (rowDataSolicitud.fcComentarioValidacionReferenciasPersonales != '') {
                //    $("#lblReferenciasComentario").text(rowDataSolicitud.fcComentarioValidacionReferenciasPersonales);
                //    $("#divReferencias").css('display', '');
                //    contadorComentariosDetalle += 1;
                //}
                ////comentario para campo depto de gestoria
                //if (rowDataSolicitud.fcObservacionesDeCredito != '') {
                //    $("#lblCampoComentario").text(rowDataSolicitud.fcObservacionesDeCredito);
                //    $("#divCampo").css('display', '');
                //    contadorComentariosDetalle += 1;
                //}
                ////observaciones de gestoria
                //if (rowDataSolicitud.fcObservacionesDeGestoria != '') {
                //    $("#lblGestoriaComentario").text(rowDataSolicitud.fcObservacionesDeGestoria);
                //    $("#divGestoria").css('display', '');
                //    contadorComentariosDetalle += 1;
                //}
                ////observaciones de gestoria
                //if (rowDataSolicitud.fcComentarioResolucion != '') {
                //    $("#lblResolucionComentario").text(rowDataSolicitud.fcComentarioResolucion);
                //    $("#divComentarioResolucion").css('display', '');
                //    contadorComentariosDetalle += 1;
                //}

                ////validar si no hay detalles que mostrar
                //if (contadorComentariosDetalle == 0) {
                //    $("#divNoHayMasDetalles").css('display', '');
                //}
                //else {
                //    $("#divNoHayMasDetalles").css('display', 'none');
                //}

                //if (rowDataSolicitud.fiSolicitudActiva == 0) {
                //    $("#divSolicitudInactiva").css('display', '');
                //}

                $("#modalEstadoSolicitud").modal();
            }
            else {
                MensajeError('Error al cargar estado de la solicitud, contacte al administrador');
            }
        }
    });
});

$(document).on('click', 'button#btnComentarioReferencia', function () {

    $("#txtObservacionesReferencia").val($(this).data('comment')).prop('disabled', true);
    $("#lblNombreReferenciaModal").text($(this).data('nombreref'));
    $("#modalComentarioReferencia").modal();
});

$("#btnHistorialExterno").click(function () {

    MensajeInformacion('Cargando buro externo');

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_RegistradaDetalles.aspx/ObtenerUrlEncriptado',
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ dataCrypt: window.location.href }),
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeError('Error al cargar buro externo');
        },
        success: function (data) {

            var parametros = data.d;
            window.open("http://portal.prestadito.corp/corefinanciero/Clientes/Precalificado_Analista.aspx?" + parametros, "_blank",
                "toolbar=yes, scrollbars=yes,resizable=yes," +
                "top=0,left=window.screen.availWidth/2," +
                "window.screen.availWidth/2,window.screen.availHeight");
        }
    });
});

/* cuando se abra un modal, ocultar el scroll del BODY y deja solo el del modal (en caso de que este tenga scroll) */
$(window).on('hide.bs.modal', function () {
    const body = document.body;
    const scrollY = body.style.top;
    body.style.position = '';
    body.style.top = '';
    window.scrollTo(0, parseInt(scrollY || '0') * -1);
    $("body").css('padding-right', '0');
});

/* cuando se cierre un modal */
$(window).on('show.bs.modal', function () {
    const scrollY = document.documentElement.style.getPropertyValue('--scroll-y');
    const body = document.body;
    body.style.position = 'fixed';
    body.style.top = `-${scrollY}`;
});

window.addEventListener('scroll', () => {
    document.documentElement.style.setProperty('--scroll-y', `${window.scrollY}px`);
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function MensajeInformacion(mensaje) {
    iziToast.info({
        title: 'Info',
        message: mensaje
    });
}

function ValidarFecha(fecha) {

    return fecha == '/Date(-2208967200000)/' ? null : fecha;
}

countdown.setLabels(
    ' <small>ms</small>| <small>seg</small>| <small>min</small>| <small>h</small>| <small>d</small>| <small>sem</small>| <small>mes</small>| <small>año</small>| <small>dec</small>| <small>sig</small>| <small>mil</small>',
    ' <small>ms</small>| <small>seg</small>| <small>min</small>| <small>h</small>| <small>d</small>| <small>sem</small>| <small>mes</small>| <small>año</small>| <small>dec</small>| <small>sig</small>| <small>mil</small>',
    ':',
    ':',
    'ahora',
    function (n) { return n.toString(); });

InicializarContador(new Date(), new Date(), 'lblProducto');

function InicializarContador(fechaInicio, fechaFin, identificadorEtiqueta) {
    debugger;
    var fechaInicial = moment(new Date(parseInt(fechaInicio.substr(6, 19)))).format().slice(0, 19);
    var fechaFinal = moment(new Date(parseInt(fechaFin.substr(6, 19)))).format().slice(0, 19);

    return countdown(
        fechaInicio,
        //function (ts) {

        //    document.getElementById('' + identificadorEtiqueta + '').innerHTML = ts.toHTML("strong"); // callback de fecha de inicio
        //},
        fechaFin,
        countdown.YEARS | countdown.MONTHS | countdown.WEEKS | countdown.DAYS | countdown.HOURS | countdown.MINUTES | countdown.SECONDS
    );
}


function ObtenerFechaFormateada(fecha)
{
    return fecha == '/Date(-2208967200000)/' ? 'En curso' : moment(fecha).locale('es').format('YYYY/MM/DD hh:mm:ss a');
}

//InicializarContador('2014-01-01T23:28:56.782Z', '2015-01-01T23:28:56.782Z', 'lblProducto')