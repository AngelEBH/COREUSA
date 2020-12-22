
var ID_ESTADO_SOLICITUD = '0';

/* Abrir modal de detalles del procesamiento de la solicitud, comentarios y condiciones */
$("#btnMasDetalles").click(function () {

    debugger;

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Detalles.aspx/CargarEstadoSolicitud",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ dataCrypt: window.location.href }),
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            debugger;

            if (data.d != null) {

                var informacionSolicitud = data.d;

                ID_ESTADO_SOLICITUD = informacionSolicitud.IdEstadoSolicitud;

                var tablaEstatusSolicitud = $('#tblDetalleEstado tbody');

                tablaEstatusSolicitud.empty();

                /* En ingreso */
                var enIngresoInicio = ObtenerFechaFormateada(informacionSolicitud.EnIngresoInicio);
                var enIngresoFin = ObtenerFechaFormateada(informacionSolicitud.EnIngresoFin);
                var enIngresoUsuario = informacionSolicitud.UsuarioEnIngreso;

                if (ValidarFecha(informacionSolicitud.EnIngresoInicio) == null) {

                    enIngresoUsuario = '-';
                }

                tablaEstatusSolicitud.append('<tr>' +
                    '<td class="text-center">Ingreso</td>' +
                    '<td class="text-center">' + enIngresoInicio + '</td>' +
                    '<td class="text-center">' + enIngresoFin + '</td>' +
                    '<td class="text-center"><span id="lblTiempoTranscurrido_EnIngreso"></span></td>' +
                    '<td class="text-center">' + enIngresoUsuario + '</td>' +
                    '</tr>');

                InicializarContador(informacionSolicitud.EnIngresoInicio, informacionSolicitud.EnIngresoFin, 'lblTiempoTranscurrido_EnIngreso');


                /* En tramite */
                var enColaInicio = ObtenerFechaFormateada(informacionSolicitud.EnColaInicio);
                var enColaFin = ObtenerFechaFormateada(informacionSolicitud.EnColaFin);

                tablaEstatusSolicitud.append('<tr>' +
                    '<td class="text-center">Recepción</td>' +
                    '<td class="text-center">' + enColaInicio + '</td>' +
                    '<td class="text-center">' + enColaFin + '</td>' +
                    '<td class="text-center"><span id="lblTiempoTranscurrido_EnCola"></span></td>' +
                    '<td class="text-center">N/A</td>' +
                    '</tr>');

                InicializarContador(informacionSolicitud.EnColaInicio, informacionSolicitud.EnColaFin, 'lblTiempoTranscurrido_EnCola');


                /* En análisis */
                var enAnalisisInicio = ObtenerFechaFormateada(informacionSolicitud.EnAnalisisInicio);
                var enAnalisisFin = ObtenerFechaFormateada(informacionSolicitud.EnAnalisisFin);
                var enAnalisisUsuario = informacionSolicitud.UsuarioAnalista;

                if (ValidarFecha(informacionSolicitud.EnAnalisisInicio) == null) {

                    enAnalisisUsuario = '-';
                }

                tablaEstatusSolicitud.append('<tr>' +
                    '<td class="text-center">Análisis</td>' +
                    '<td class="text-center">' + enAnalisisInicio + '</td>' +
                    '<td class="text-center">' + enAnalisisFin + '</td>' +
                    '<td class="text-center"><span id="lblTiempoTranscurrido_EnAnalisis"></span></td>' +
                    '<td class="text-center">' + enAnalisisUsuario + '</td>' +
                    '</tr>');

                InicializarContador(informacionSolicitud.EnAnalisisInicio, informacionSolicitud.EnAnalisisFin, 'lblTiempoTranscurrido_EnAnalisis');


                /* En campo */
                var enCampoInicio = ObtenerFechaFormateada(informacionSolicitud.EnRutaDeInvestigacionInicio);
                var enCampoFin = ObtenerFechaFormateada(informacionSolicitud.EnRutaDeInvestigacionFin);
                var enCampoUsuario = informacionSolicitud.UsuarioGestorAsignado;

                if (ValidarFecha(informacionSolicitud.EnRutaDeInvestigacionInicio) == null) {

                    enCampoUsuario = '-';
                }

                tablaEstatusSolicitud.append('<tr>' +
                    '<td class="text-center">Campo</td>' +
                    '<td class="text-center">' + enCampoInicio + '</td>' +
                    '<td class="text-center">' + enCampoFin + '</td>' +
                    '<td class="text-center"><span id="lblTiempoTranscurrido_EnCampo"></span></td>' +
                    '<td class="text-center">' + enCampoUsuario + '</td>' +
                    '</tr>');

                InicializarContador(informacionSolicitud.EnRutaDeInvestigacionInicio, informacionSolicitud.EnRutaDeInvestigacionFin, 'lblTiempoTranscurrido_EnCampo');


                /* Condicionado */
                var condicionadoInicio = ObtenerFechaFormateada(informacionSolicitud.CondicionadoInicio);
                var condicionadoFin = ObtenerFechaFormateada(informacionSolicitud.CondicionadoFin);
                var condicionadoUsuario = informacionSolicitud.UsuarioCondicionado;

                if (ValidarFecha(informacionSolicitud.CondicionadoInicio) == null) {

                    condicionadoUsuario = '-';
                }

                tablaEstatusSolicitud.append('<tr>' +
                    '<td class="text-center">Condicionado</td>' +
                    '<td class="text-center">' + condicionadoInicio + '</td>' +
                    '<td class="text-center">' + condicionadoFin + '</td>' +
                    '<td class="text-center"><span id="lblTiempoTranscurrido_Condicionado"></span></td>' +
                    '<td class="text-center">' + condicionadoUsuario + '</td>' +
                    '</tr>');

                InicializarContador(informacionSolicitud.CondicionadoInicio, informacionSolicitud.CondicionadoFin, 'lblTiempoTranscurrido_Condicionado');


                /* Reprogramado */
                var reprogramadoInicio = ObtenerFechaFormateada(informacionSolicitud.ReprogramadoInicio);
                var reprogramadoFin = ObtenerFechaFormateada(informacionSolicitud.ReprogramadoFin);
                var reprogramadoUsuario = informacionSolicitud.UsuarioGestorAsignado;

                if (ValidarFecha(informacionSolicitud.ReprogramadoInicio) == null) {

                    reprogramadoUsuario = '-';
                }

                tablaEstatusSolicitud.append('<tr>' +
                    '<td class="text-center">Reprogramado</td>' +
                    '<td class="text-center">' + reprogramadoInicio + '</td>' +
                    '<td class="text-center">' + reprogramadoFin + '</td>' +
                    '<td class="text-center"><span id="lblTiempoTranscurrido_Reprogramado"></span></td>' +
                    '<td class="text-center">' + reprogramadoUsuario + '</td>' +
                    '</tr>');

                InicializarContador(informacionSolicitud.ReprogramadoInicio, informacionSolicitud.ReprogramadoFin, 'lblTiempoTranscurrido_Reprogramado');


                /* Validación */
                var validacionInicio = ObtenerFechaFormateada(informacionSolicitud.PasoFinalInicio);
                var validacionFin = ObtenerFechaFormateada(informacionSolicitud.PasoFinalFin);
                var validacionUsuario = informacionSolicitud.UsuarioPasoFinal;

                if (ValidarFecha(informacionSolicitud.PasoFinalInicio) == null) {

                    validacionUsuario = '-';
                }

                tablaEstatusSolicitud.append('<tr>' +
                    '<td class="text-center">Validación</td>' +
                    '<td class="text-center">' + validacionInicio + '</td>' +
                    '<td class="text-center">' + validacionFin + '</td>' +
                    '<td class="text-center"><span id="lblTiempoTranscurrido_Validacion"></span></td>' +
                    '<td class="text-center">' + (validacionUsuario == '' ? '-' : validacionUsuario) + '</td>' +
                    '</tr>');

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


                /* Condiciones de la solicitud */
                var tblCondiciones = $("#tblListaSolicitudCondiciones tbody");
                tblCondiciones.empty();
                tblCondiciones.append('<tr><td class="text-center" colspan="4">No hay registros disponibles...</td></tr>');

                if (informacionSolicitud.Condiciones != null) {
                    if (informacionSolicitud.Condiciones.length > 0) {

                        var condiciones = informacionSolicitud.Condiciones;
                        let templateCondiciones = '';
                        let estadoCondicion = '';
                        //let btnAnularCondicion = '';

                        tblCondiciones.empty();

                        for (var i = 0; i < condiciones.length; i++) {

                            estadoCondicion = condiciones[i].EstadoCondicion != true ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-warning mb-0'>Pendiente</label>"

                            //btnAnularCondicion = condiciones[i].EstadoCondicion != true ? '' : '<button id="btnAnularCondicion" data-id="' + condiciones[i].IdSolicitudCondicion + '" class="btn btn-sm btn-danger mb-0" type="button" title="Anular condición"><i class="far fa-trash-alt"></i></button>';

                            templateCondiciones += '<tr><td>' + condiciones[i].TipoCondicion + '</td><td>' + condiciones[i].DescripcionCondicion + '</td><td>' + condiciones[i].ComentarioAdicional + '</td><td>' + estadoCondicion + '</td></tr>';
                        }

                        tblCondiciones.append(templateCondiciones);

                        $("#pestanaListaSolicitudCondiciones").css('display', '');
                    }
                    else {

                        $("#pestanaListaSolicitudCondiciones").css('display', 'none');
                    }

                }
                else {

                    $("#pestanaListaSolicitudCondiciones").css('display', 'none');
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

$("#btnHistorialExterno").click(function () {

    MensajeInformacion('Cargando buro externo');

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Detalles.aspx/ObtenerUrlEncriptado',
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
    /* Verificar si el proceso todavía no ha empezado */
    else if (fechaFin == '/Date(-2208967200000)/' && fechaInicio == '/Date(-2208967200000)/') {

        document.getElementById('' + identificadorEtiqueta + '').innerHTML = '-';
    }
    /* Verificar si el proceso ya empezó y finalizó */
    else if (fechaFin != '/Date(-2208967200000)/' && fechaInicio != '/Date(-2208967200000)/') {

        document.getElementById('' + identificadorEtiqueta + '').innerHTML = countdown(
            fechaInicial,
            fechaFinal,
            countdown.YEARS | countdown.MONTHS | countdown.WEEKS | countdown.DAYS | countdown.HOURS | countdown.MINUTES | countdown.SECONDS
        ).toHTML("");
    }
    /* Verificar si el proceso ya empezó y todavía no ha finalizado */
    else if (fechaInicio != '/Date(-2208967200000)/') {

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