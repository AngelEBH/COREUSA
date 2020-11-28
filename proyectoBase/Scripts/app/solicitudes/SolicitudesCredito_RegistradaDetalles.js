var idSolicitud = 0;
var objSolicitud = [];
var resolucionHabilitada = false;

//abrir modal de detalles del estado del procesamiento de la solicitud
$('#tblEstadoSolicitud tbody').on('click', 'tr', function () {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_RegistradaDetalles.aspx/CargarEstadoSolicitud",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ dataCrypt: window.location.href}),
        error: function (xhr, ajaxOptions, thrownError) {

            //mostrar mensaje de error
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            if (data.d != null) {

                var rowDataSolicitud = data.d;

                var ProcesoPendiente = '/Date(-2208967200000)/';

                //limpiar tabla de detalles del estado de la solicitud
                var tablaEstatusSolicitud = $('#tblDetalleEstado tbody');
                tablaEstatusSolicitud.empty();

                var ingresoInicio = rowDataSolicitud.fdEnIngresoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnIngresoInicio) : '';
                var ingresoFin = rowDataSolicitud.fdEnIngresoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnIngresoFin) : '';
                var tiempoIngreso = ingresoInicio != '' ? ingresoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnIngresoInicio, rowDataSolicitud.fdEnIngresoFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Ingreso</td>' +
                    '<td>' + ingresoInicio + '</td>' +
                    '<td>' + ingresoFin + '</td>' +
                    '<td>' + tiempoIngreso + '</td>' +
                    '</tr>');

                var EnTramiteInicio = rowDataSolicitud.fdEnTramiteInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnTramiteInicio) : '';
                var EnTramiteFin = rowDataSolicitud.fdEnTramiteFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnTramiteFin) : '';
                var tiempoTramite = EnTramiteInicio != '' ? EnTramiteFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnTramiteInicio, rowDataSolicitud.fdEnTramiteFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Recepción</td>' +
                    '<td>' + EnTramiteInicio + '</td>' +
                    '<td>' + EnTramiteFin + '</td>' +
                    '<td>' + tiempoTramite + '</td>' +
                    '</tr>');

                var EnAnalisisInicio = rowDataSolicitud.fdEnAnalisisInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnAnalisisInicio) : '';
                var EnAnalisisFin = rowDataSolicitud.fdEnAnalisisFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnAnalisisFin) : '';
                var tiempoAnalisis = EnAnalisisInicio != '' ? EnAnalisisFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnAnalisisInicio, rowDataSolicitud.fdEnAnalisisFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Análisis</td>' +
                    '<td>' + EnAnalisisInicio + '</td>' +
                    '<td>' + EnAnalisisFin + '</td>' +
                    '<td>' + tiempoAnalisis + '</td>' +
                    '</tr>');

                var EnCampoInicio = rowDataSolicitud.fdEnvioARutaAnalista != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnvioARutaAnalista) : '';
                var EnCampoFin = rowDataSolicitud.fdEnCampoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnCampoFin) : '';
                var tiempoCampo = EnCampoInicio != '' ? EnCampoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnvioARutaAnalista, rowDataSolicitud.fdEnCampoFin) : '' : '';
                tiempoCampo == '' ? habilitarResolucion = false : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Campo</td>' +
                    '<td>' + EnCampoInicio + '</td>' +
                    '<td>' + EnCampoFin + '</td>' +
                    '<td>' + tiempoCampo + '</td>' +
                    '</tr>');

                var CondicionadoInicio = rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondicionadoInicio) : '';
                var CondificionadoFin = rowDataSolicitud.fdCondificionadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondificionadoFin) : '';
                var tiempoCondicionado = CondicionadoInicio != '' ? CondificionadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdCondicionadoInicio, rowDataSolicitud.fdCondificionadoFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Condicionado</td>' +
                    '<td>' + CondicionadoInicio + '</td>' +
                    '<td>' + CondificionadoFin + '</td>' +
                    '<td>' + tiempoCondicionado + '</td>' +
                    '</tr>');

                var ReprogramadoInicio = rowDataSolicitud.fdReprogramadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoInicio) : '';
                var ReprogramadoFin = rowDataSolicitud.fdReprogramadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoFin) : '';
                var tiempoReprogramado = ReprogramadoInicio != '' ? ReprogramadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdReprogramadoInicio, rowDataSolicitud.fdReprogramadoFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Reprogramado</td>' +
                    '<td>' + ReprogramadoInicio + '</td>' +
                    '<td>' + ReprogramadoFin + '</td>' +
                    '<td>' + tiempoReprogramado + '</td>' +
                    '</tr>');


                var ValidacionInicio = rowDataSolicitud.PasoFinalInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.PasoFinalInicio) : '';
                var ValidacionFin = rowDataSolicitud.PasoFinalFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.PasoFinalFin) : '';
                var tiempoValidacion = ValidacionInicio != '' ? ValidacionFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.PasoFinalInicio, rowDataSolicitud.PasoFinalFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Validación</td>' +
                    '<td>' + ValidacionInicio + '</td>' +
                    '<td>' + ValidacionFin + '</td>' +
                    '<td>' + tiempoValidacion + '</td>' +
                    '</tr>');

                var timer = rowDataSolicitud.fcTiempoTotalTranscurrido.split(':');
                tablaEstatusSolicitud.append('<tr>' +
                    '<td colspan="4" class="text-center">Tiempo total transcurrido: <strong>' + timer[0] + ' horas con ' + timer[1] + ' minutos y ' + timer[2] + ' segundos </strong></td>' +
                    '</tr >');

                //verificar si ya se tomó una resolución de la solicitud
                if (rowDataSolicitud.fiEstadoSolicitud == 7) {
                    $("#lblResolucion").text('Aprobado');
                    $("#lblResolucion").removeClass('text-warning');
                    $("#lblResolucion").addClass('text-success');
                }
                else if (rowDataSolicitud.fiEstadoSolicitud == 4) {
                    $("#lblResolucion").text('Rechazado por analistas');
                    $("#lblResolucion").removeClass('text-warning');
                    $("#lblResolucion").addClass('text-danger');
                }
                else if (rowDataSolicitud.fiEstadoSolicitud == 5) {
                    $("#lblResolucion").text('Rechazado por gestores');
                    $("#lblResolucion").removeClass('text-warning');
                    $("#lblResolucion").addClass('text-danger');
                }

                else if (rowDataSolicitud.fdEnTramiteFin != '/Date(-2208967200000)/') {
                    $("#lblResolucion").text('En análisis');
                }
                var contadorComentariosDetalle = 0;

                //razon reprogramado
                if (rowDataSolicitud.fcReprogramadoComentario != '') {
                    $("#lblReprogramado").text(rowDataSolicitud.fcReprogramadoComentario);
                    $("#divReprogramado").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //razon condicionado
                if (rowDataSolicitud.fcCondicionadoComentario != '') {
                    $("#lblCondicionado").text(rowDataSolicitud.fcCondicionadoComentario);
                    $("#divCondicionado").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones documentacion
                if (rowDataSolicitud.fcComentarioValidacionDocumentacion != '') {
                    $("#lblDocumentacionComentario").text(rowDataSolicitud.fcComentarioValidacionDocumentacion);
                    $("#divDocumentacionComentario").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones info personal
                if (rowDataSolicitud.fcComentarioValidacionInfoPersonal != '') {
                    $("#lblInfoPersonalComentario").text(rowDataSolicitud.fcComentarioValidacionInfoPersonal);
                    $("#divInfoPersonal").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones info laboral
                if (rowDataSolicitud.fcComentarioValidacionInfoLaboral != '') {
                    $("#lblInfoLaboralComentario").text(rowDataSolicitud.fcComentarioValidacionInfoLaboral);
                    $("#divInfoLaboral").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones referencias
                if (rowDataSolicitud.fcComentarioValidacionReferenciasPersonales != '') {
                    $("#lblReferenciasComentario").text(rowDataSolicitud.fcComentarioValidacionReferenciasPersonales);
                    $("#divReferencias").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //comentario para campo depto de gestoria
                if (rowDataSolicitud.fcObservacionesDeCredito != '') {
                    $("#lblCampoComentario").text(rowDataSolicitud.fcObservacionesDeCredito);
                    $("#divCampo").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones de gestoria
                if (rowDataSolicitud.fcObservacionesDeGestoria != '') {
                    $("#lblGestoriaComentario").text(rowDataSolicitud.fcObservacionesDeGestoria);
                    $("#divGestoria").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones de gestoria
                if (rowDataSolicitud.fcComentarioResolucion != '') {
                    $("#lblResolucionComentario").text(rowDataSolicitud.fcComentarioResolucion);
                    $("#divComentarioResolucion").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                //validar si no hay detalles que mostrar
                if (contadorComentariosDetalle == 0) {
                    $("#divNoHayMasDetalles").css('display', '');
                }
                else {
                    $("#divNoHayMasDetalles").css('display', 'none');
                }

                if (rowDataSolicitud.fiSolicitudActiva == 0) {
                    $("#divSolicitudInactiva").css('display', '');
                }

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

//modal terminar validacion de documentación---
$("#btnDocumentacionModal").click(function () {
    $("#modalDocumentacion").modal();
});

$("#btnHistorialExterno").click(function () {

    MensajeInformacion('Cargando buro externo');
    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_RegistradaDetalles.aspx/ObtenerUrlEncriptado' + qString,
        contentType: 'application/json; charset=utf-8',
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

function cargarPrestamosSugeridos(identidad, ingresos, obligaciones, codigoProducto) {

    $("#cargandoPrestamosSugeridosReales").css('display', '');

    MensajeInformacion('Cargando préstamos sugeridos');

    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_RegistradaDetalles.aspx/GetPrestamosSugeridos' + qString,
        data: JSON.stringify({ identidad: identidad, ingresos: ingresos, obligaciones: obligaciones, codigoProducto: codigoProducto }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar préstamos sugeridos');
            $("#cargandoPrestamosSugeridosReales").css('display', 'none');
        },
        success: function (data) {

            if (data.d != null) {

                var listaPmos = data.d.cotizadorProductos;

                var tablaPrestamos = $("#tblPMOSugeridosReales tbody");
                tablaPrestamos.empty();

                for (var i = 0; i < listaPmos.length; i++) {

                    var botonSeleccionarPrestamo = '<button id="btnSeleccionarPMO" data-monto="' + listaPmos[i].fnMontoOfertado + '" data-plazo="' + listaPmos[i].fiPlazo + '" data-cuota="' + listaPmos[i].fnCuotaQuincenal + '" class="btn mdi mdi-pencil mdi-24px text-info" disabled="disabled"></button>';
                    tablaPrestamos.append('<tr>'
                        + '<td>' + listaPmos[i].fnMontoOfertado + '</td>'
                        + '<td>' + listaPmos[i].fiPlazo + '</td>'
                        + '<td>' + listaPmos[i].fnCuotaQuincenal + '</td>'
                        + '</tr>');
                }
                $("#cargandoPrestamosSugeridosReales").css('display', 'none');
                $("#lbldivPrestamosSugeridosReales,#tblPMOSugeridosReales,#divPrestamosSugeridosReales").css('display', '');
            }
            else {
                $("#cargandoPrestamosSugeridosReales").css('display', 'none');
            }
        }
    });
}

//cuando se abra un modal, ocultar el scroll del BODY y deja solo el del modal (en caso de que este tenga scroll)
$(window).on('hide.bs.modal', function () {
    const body = document.body;
    const scrollY = body.style.top;
    body.style.position = '';
    body.style.top = '';
    window.scrollTo(0, parseInt(scrollY || '0') * -1);
    $("body").css('padding-right', '0');
});

//cuando se cierre un modal
$(window).on('show.bs.modal', function () {
    const scrollY = document.documentElement.style.getPropertyValue('--scroll-y');
    const body = document.body;
    body.style.position = 'fixed';
    body.style.top = `-${scrollY}`;
});

window.addEventListener('scroll', () => {
    document.documentElement.style.setProperty('--scroll-y', `${window.scrollY}px`);
});

// mensaje de error
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

// formatear fechas
function pad2(number) {
    return (number < 10 ? '0' : '') + number
}

function FechaFormato(pFecha) {
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
    var FechaFinal = dia + "/" + mes + "/" + anio + " " + hora + ":" + minutos + ":" + segundos;
    return FechaFinal;
}

function diferenciasEntreDosFechas(fechaInicio, fechaFin) {

    //calcular tiempo en ingreso
    var inicio = new Date(FechaFormatoGuiones(fechaInicio));

    var fin = new Date(FechaFormatoGuiones(fechaFin));

    var tiempoResta = (fin.getTime() - inicio.getTime()) / 1000;

    //calcular dias en ingreso
    var dias = Math.floor(tiempoResta / 86400);
    //tiempoResta = tiempoResta >= 86400 ? dias * 86400 : tiempoResta;

    //calcular horas en ingreso
    var horas = Math.floor(tiempoResta / 3600) % 24;
    //tiempoResta = tiempoResta >= 3600 ? horas * 3600 : tiempoResta;

    //calcular minutos en ingreso
    var minutos = Math.floor(tiempoResta / 60) % 60;
    //tiempoResta = tiempoResta >= 3600 ? minutos * 3600 : tiempoResta;

    //calcular segundos en ingreso
    var segundos = tiempoResta % 60;

    var diferencia = pad2(dias) + ':' + pad2(horas) + ':' + pad2(minutos) + ':' + pad2(segundos);

    return diferencia;
}

function FechaFormatoGuiones(pFecha) {
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