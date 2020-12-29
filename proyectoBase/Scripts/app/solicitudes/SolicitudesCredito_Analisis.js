
// #region variables globales

/* ====== Esta variable almacena el ID del estado actual de la solicitud ============== */
/* ====== Se actualiza cada vez que se cargan los detalles de la solicitud ============ */
/* ====== Se utiliza para realizar validaciones al cargar detalles de la solicitud ==== */
var ID_ESTADO_SOLICITUD = '0'; 

/* ====== Esta variable (objeto) almacena el estado actual de la solicitud ============ */
/* ====== Se actualiza cada vez que se cargan los detalles de la solicitud ============ */
/* ====== Se utiliza para realizar validaciones durante todo el analisis ============== */
var ESTADO_SOLICITUD = [];

/* ====== Esta variable almacena durante el analisis el monto final por el que se va a autorizar la solicitud ============== */
/* ====== Esta variable almacena durante el analisis el plazo final por el que se va a autorizar la solicitud ============== */
var gMontoFinal = 0;
var gPlazoFinal = 0;


var resolucionHabilitada = false;
var resolucion = false;

// #endregion

// #region Cargar detalles de la solicitud

$("#btnMasDetalles").click(function () {
    
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/CargarEstadoSolicitud",
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify({ dataCrypt: window.location.href }),
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            if (data.d != null) {

                var informacionSolicitud = data.d;

                ID_ESTADO_SOLICITUD = informacionSolicitud.IdEstadoSolicitud;

                var tablaEstatusSolicitud = $('#tblDetalleEstado tbody');

                tablaEstatusSolicitud.empty();

                /* En ingreso */
                var enIngresoInicio = ObtenerFechaFormateada(informacionSolicitud.EnIngresoInicio);
                var enIngresoFin = ObtenerFechaFormateada(informacionSolicitud.EnIngresoFin);
                var enIngresoUsuario = informacionSolicitud.UsuarioEnIngreso;

                if (ValidarFecha(informacionSolicitud.EnIngresoInicio) == null)
                {
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

                if (ValidarFecha(informacionSolicitud.EnAnalisisInicio) == null)
                {
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

                if (ValidarFecha(informacionSolicitud.EnRutaDeInvestigacionInicio) == null)
                {
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

                if (ValidarFecha(informacionSolicitud.CondicionadoInicio) == null)
                {
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

                if (ValidarFecha(informacionSolicitud.ReprogramadoInicio) == null)
                {
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

                if (ValidarFecha(informacionSolicitud.PasoFinalInicio) == null)
                {
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
                if (informacionSolicitud.ComentarioReprogramado != '')
                {
                    $("#lblUsuario_ComentarioReprogramacion").text(informacionSolicitud.UsuarioGestorAsignado);
                    $("#lblFecha_ComentarioReprogramacion").text(ObtenerFechaFormateada(informacionSolicitud.ReprogramadoInicio));
                    $("#lblComentario_Reprogramacion").text(informacionSolicitud.ComentarioReprogramado);
                    $("#liObservacionesReprogramacion").css('display', '');
                    contadorComentario++;
                }
                else
                {
                    $("#liObservacionesReprogramacion").css('display', 'none');
                }

                /* Condicionado comentario */
                if (informacionSolicitud.ComentarioCondicionado != '')
                {
                    $("#lblUsuario_ComentarioOtrosCondicionamientos").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_OtrosCondicionamientos").text(ObtenerFechaFormateada(informacionSolicitud.CondicionadoInicio));
                    $("#lblComentario_OtrosCondicionamientos").text(informacionSolicitud.ComentarioCondicionado);
                    $("#liObservaciones_OtrosCondicionamientos").css('display', '');
                    contadorComentario++;
                }
                else
                {
                    $("#liObservaciones_OtrosCondicionamientos").css('display', 'none');
                }

                /* Informacion personal comentario */
                if (informacionSolicitud.ComentarioValidacionInformacionPersonal != '')
                {
                    $("#lblUsuario_ComentarioInformacionPerosnal").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioInformacionPersonal").text(ObtenerFechaFormateada(informacionSolicitud.FechaValidacionInformacionPersonal));
                    $("#lblComentario_InformacionPersonal").text(informacionSolicitud.ComentarioValidacionInformacionPersonal);
                    $("#liObservacionesInformacionPersonal").css('display', '');
                    contadorComentario++;
                }
                else
                {
                    $("#liObservacionesInformacionPersonal").css('display', 'none');
                }

                /* Informacion laboral comentario */
                if (informacionSolicitud.ComentarioValidacionInformacionLaboral != '')
                {
                    $("#lblUsuario_ComentarioInformacionLaboral").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioInformacionLaboral").text(ObtenerFechaFormateada(informacionSolicitud.FechaValidacionInformacionLaboral));
                    $("#lblComentario_InformacionLaboral").text(informacionSolicitud.ComentarioValidacionInformacionLaboral);
                    $("#liObservacionesInformacionLaboral").css('display', '');
                    contadorComentario++;
                }
                else
                {
                    $("#liObservacionesInformacionLaboral").css('display', 'none');
                }

                /* Referencias personales comentario */
                if (informacionSolicitud.ComentarioValidacionReferenciasPersonales != '')
                {
                    $("#lblUsuario_ComentarioReferenciasPersonales").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioReferenciasPersonales").text(ObtenerFechaFormateada(informacionSolicitud.FechaValidacionReferenciasPersonales));
                    $("#lblComentario_ReferenciasPersonales").text(informacionSolicitud.ComentarioValidacionReferenciasPersonales);
                    $("#liObservacionesReferenciasPersonales").css('display', '');
                    contadorComentario++;
                }
                else
                {
                    $("#liObservacionesReferenciasPersonales").css('display', 'none');
                }

                /* Documentación comentario */
                if (informacionSolicitud.ComentarioValidacionDocumentacion != '')
                {
                    $("#lblUsuario_ComentarioDocumentacion").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioDocumentacion").text(ObtenerFechaFormateada(informacionSolicitud.FechaValidacionDocumentacion));
                    $("#lblComentario_Documentacion").text(informacionSolicitud.ComentarioValidacionDocumentacion);
                    $("#liObservacionesDocumentacion").css('display', '');
                    contadorComentario++;
                }
                else
                {
                    $("#liObservacionesDocumentacion").css('display', 'none');
                }

                /* Observaciones de crédito */
                if (informacionSolicitud.ObservacionesDeCreditos != '')
                {
                    $("#lblUsuario_ComentarioParaGestoria").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioParaGestoria").text(ObtenerFechaFormateada(informacionSolicitud.FechaEnvioARuta));
                    $("#lblComentario_ParaGestoria").text(informacionSolicitud.ObservacionesDeCreditos);
                    $("#liObservacionesParaGestoria").css('display', '');
                    contadorComentario++;
                }
                else
                {
                    $("#liObservacionesParaGestoria").css('display', 'none');
                }

                /* Observaciones de gestoria */
                if (informacionSolicitud.ObservacionesDeCampo != '')
                {
                    $("#lblUsuario_ComentarioDeGestoria").text(informacionSolicitud.UsuarioGestorAsignado);
                    $("#lblFecha_ComentarioDeGestoria").text(ObtenerFechaFormateada(informacionSolicitud.EnRutaDeInvestigacionFin));
                    $("#lblComentario_DeGestoria").text(informacionSolicitud.ObservacionesDeCampo);
                    $("#liObservacionesDeGestoria").css('display', '');
                    contadorComentario++;
                }
                else
                {
                    $("#liObservacionesDeGestoria").css('display', 'none');
                }

                /* Comentarios de la resolución */
                if (informacionSolicitud.ComentarioResolucion != '')
                {
                    $("#lblUsuario_ComentarioDeLaResolucion").text(informacionSolicitud.UsuarioAnalista);
                    $("#lblFecha_ComentarioDeLaResolucion").text(ObtenerFechaFormateada(informacionSolicitud.TiempoTomaDecisionFinal));
                    $("#lblComentario_Resolicion").text(informacionSolicitud.ComentarioResolucion);
                    $("#liComentariosDeLaResolucion").css('display', '');
                    contadorComentario++;
                }
                else
                {
                    $("#liComentariosDeLaResolucion").css('display', 'none');
                }

                if (contadorComentario > 0)
                {
                    $("#divNoHayMasDetalles").css('display', 'none');
                    $("#divLineaDeTiempo").css('display', '');
                }
                else
                {
                    $("#divNoHayMasDetalles").css('display', '');
                    $("#divLineaDeTiempo").css('display', 'none');
                }

                if (informacionSolicitud.SolicitudActiva == 0)
                {
                    $("#divSolicitudInactiva").css('display', '');
                }
                else
                {
                    $("#divSolicitudInactiva").css('display', 'none');
                }

                /* Condiciones de la solicitud */
                var tblCondiciones = $("#tblListaCondicionesDeLaSolicitud tbody");
                tblCondiciones.empty();
                tblCondiciones.append('<tr><td class="text-center" colspan="4">No hay registros disponibles...</td></tr>');

                if (informacionSolicitud.Condiciones != null) {

                    if (informacionSolicitud.Condiciones.length > 0) {

                        var condiciones = informacionSolicitud.Condiciones;
                        let templateCondiciones = '';
                        let estadoCondicion = '';

                        tblCondiciones.empty();

                        for (var i = 0; i < condiciones.length; i++) {

                            estadoCondicion = condiciones[i].EstadoCondicion != true ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-warning mb-0'>Pendiente</label>";

                            templateCondiciones += '<tr><td>' + condiciones[i].TipoCondicion + '</td><td>' + condiciones[i].DescripcionCondicion + '</td><td>' + condiciones[i].ComentarioAdicional + '</td><td>' + estadoCondicion + '</td></tr>';
                        }

                        tblCondiciones.append(templateCondiciones);

                        $("#tabListaCondicionesDeLaSolicitud").css('display', '');

                    } // if informacionSolicitud.Condiciones.length > 0
                    else {
                        $("#tabListaCondicionesDeLaSolicitud").css('display', 'none');
                    }

                } // if informacionSolicitud.Condiciones != null
                else {
                    $("#tabListaCondicionesDeLaSolicitud").css('display', 'none');
                }

                $("#modalEstadoSolicitud").modal();

            } // if data.d != null
            else {
                MensajeError('Error al cargar estado de la solicitud, contacte al administrador');
            }
        } // success
    }); // $.ajax
});

// #endregion Cargar detalles de la solicitud

// #region Administrar condiciones de la solicitud

/* Condicionar solicitud */
$("#btnCondicionarSolicitud").click(function () {

    var ddlCondiciones = $("#ddlCondiciones");

    listaCondicionamientos = [];
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/GetCondiciones",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar el lista de condiciones');
            $("#modalCondicionarSolicitud").modal();
        },
        success: function (data) {
            var listaCondiciones = data.d;
            $("#tblCondiciones tbody").empty();
            $("#txtComentarioAdicional").val('');
            ddlCondiciones.empty();
            for (var i = 0; i < listaCondiciones.length; i++) {
                ddlCondiciones.append('<option value="' + listaCondiciones[i].fiIDCondicion + '">' + listaCondiciones[i].fcCondicion + ' | ' + listaCondiciones[i].fcDescripcionCondicion + '</option>');
            }
            $("#modalCondicionarSolicitud").modal();
        }
    });
});

var contadorCondiciones = 0;
var listaCondicionamientos = [];
$("#btnAgregarCondicion").click(function () {

    if ($($("#frmAddCondicion")).parsley().isValid()) {
        var tblCondiciones = $("#tblCondiciones tbody");
        var condicionID = $("#ddlCondiciones :selected").val();
        var descripcionCondicion = $("#ddlCondiciones :selected").text();
        var comentarioAdicional = $("#txtComentarioAdicional").val();
        var btnQuitarCondicion = '<button data-id=' + condicionID + ' data-comentario="' + comentarioAdicional + '" id="btnQuitarCondicion" class="btn btn-sm btn-danger">Quitar</button>';
        var newRowContent = '<tr><td>' + descripcionCondicion + '</td><td>' + comentarioAdicional + '</td><td>' + btnQuitarCondicion + '</td></tr>';
        $("#tblCondiciones tbody").append(newRowContent);
        $("#txtComentarioAdicional").val('');

        listaCondicionamientos.push({
            fiIDCondicion: condicionID,
            fcComentarioAdicional: comentarioAdicional
        });
        contadorCondiciones = contadorCondiciones + 1;

    } else { $($("#frmAddCondicion")).parsley().validate(); }
});

$(document).on('click', 'button#btnQuitarCondicion', function () {

    $(this).closest('tr').remove()
    var condicion = {
        fiIDCondicion: $(this).data('id').toString(),
        fcComentarioAdicional: $(this).data('comentario')
    };
    var list = [];
    if (listaCondicionamientos.length > 0) {

        for (var i = 0; i < listaCondicionamientos.length; i++) {

            var iter = {
                fiIDCondicion: listaCondicionamientos[i].fiIDCondicion,
                fcComentarioAdicional: listaCondicionamientos[i].fcComentarioAdicional
            };
            if (JSON.stringify(iter) != JSON.stringify(condicion)) {
                list.push(iter);
            }
        }
    }
    listaCondicionamientos = list;
    contadorCondiciones -= 1;
});

$("#btnCondicionarSolicitudConfirmar").click(function () {

    var otroComentario = $("#razonCondicion").val() != '' ? $("#razonCondicion").val() : '';
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/CondicionarSolicitud",
        data: JSON.stringify({ SolicitudCondiciones: listaCondicionamientos, fcCondicionadoComentario: otroComentario, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {
            if (data.d == true) {
                $("#modalCondicionarSolicitud").modal('hide');
                //$("#btnCondicionarSolicitud, #btnCondicionarSolicitudConfirmar,#btnAgregarCondicion").prop('disabled', true);
                $("#btnCondicionarSolicitud, #btnCondicionarSolicitudConfirmar,#btnAgregarCondicion").prop('title', 'La solicitud ya está condicionada, esperando actualización de agente de ventas');
                MensajeExito('Estado de la solicitud actualizado');
                actualizarEstadoSolicitud();
            }
            else { MensajeError('Error al condicionar la solicitud, contacte al administrador'); }
        }
    });
});

// #endregion Administrar condiciones de la solicitud

// #region Administrar referencias personales

/* Actualizar comentario sobre una referencia personal radiofaro */
var comentarioActual = '';
var IDReferencia = '';
var btnReferenciaSeleccionada = '';

$(document).on('click', 'button#btnComentarioReferencia', function () {

    btnReferenciaSeleccionada = $(this);
    comentarioActual = $(this).data('comment');
    IDReferencia = $(this).data('id');
    var nombreReferencia = $(this).data('nombreref');

    $("#txtObservacionesReferencia").val(comentarioActual);
    $("#lblNombreReferenciaModal").text(nombreReferencia);

    if (comentarioActual != '' && comentarioActual != 'Sin comunicacion') {
        $("#txtObservacionesReferencia").prop('disabled', true);
        $("#btnComentarioReferenciaConfirmar,#btnReferenciaSinComunicacion").prop('disabled', true).removeClass('btn-primary').addClass('btn-secondary');
    }
    else if (comentarioActual == 'Sin comunicacion') {
        $("#txtObservacionesReferencia").prop('disabled', false);
        $("#btnReferenciaSinComunicacion").prop('disabled', true);
        $("#btnComentarioReferenciaConfirmar").prop('disabled', false).removeClass('btn-secondary').addClass('btn-primary');
    }
    else {
        $("#txtObservacionesReferencia").prop('disabled', false);
        $("#btnComentarioReferenciaConfirmar,#btnReferenciaSinComunicacion").prop('disabled', false);
    }
    $("#modalComentarioReferencia").modal();
});

$("#btnComentarioReferenciaConfirmar").click(function () {

    if ($($("#frmObservacionReferencia")).parsley().isValid()) {

        $("#frmObservacionReferencia").submit(function (e) { e.preventDefault(); });
        comentarioActual = $('#txtObservacionesReferencia').val();

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/ComentarioReferenciaPersonal',
            data: JSON.stringify({ IDReferencia: IDReferencia, comentario: comentarioActual, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al actualizar estado de la referencia personal');
            },
            success: function (data) {
                if (data.d == true) {
                    $("#modalComentarioReferencia").modal('hide');
                    MensajeExito('Observaciones de la referencia personal actualizadas correctamente');
                    btnReferenciaSeleccionada.data('comment', comentarioActual);
                    btnReferenciaSeleccionada.closest('tr').removeClass('text-danger').addClass('tr-exito');
                    btnReferenciaSeleccionada.removeClass('mdi mdi-pencil').removeClass('mdi mdi-call-missed text-danger').addClass('mdi mdi-check-circle-outline tr-exito');
                }
                else { MensajeError('Error al actualizar observaciones de la referencia personal'); }
            }
        });
    }
    else { $($("#frmObservacionReferencia")).parsley().validate(); }
});

$("#btnReferenciaSinComunicacion").click(function () {

    comentarioActual = 'Sin comunicacion';

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ComentarioReferenciaPersonal',
        data: JSON.stringify({ IDReferencia: IDReferencia, comentario: comentarioActual, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al actualizar estado de la referencia personal');
        },
        success: function (data) {
            if (data.d == true) {
                $("#modalComentarioReferencia").modal('hide');
                MensajeExito('Estado de la referencia personal actualizado correctamente');
                btnReferenciaSeleccionada.data('comment', comentarioActual);
                btnReferenciaSeleccionada.removeClass('mdi mdi-check-circle-outline tr-exito').removeClass('mdi mdi-pencil').addClass('mdi mdi-call-missed text-danger');
                btnReferenciaSeleccionada.closest('tr').addClass('text-danger');
            }
            else { MensajeError('Error al actualizar estado de la referencia personal'); }
        }
    });
});

$("#btnEliminarReferencia").click(function () {
    $("#modalComentarioReferencia").modal('hide');
    $("#modalEliminarReferencia").modal('show');
});

$("#btnEliminarReferenciaConfirmar").click(function () {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/EliminarReferenciaPersonal',
        data: JSON.stringify({ IDReferencia: IDReferencia, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al eliminar referencia personal');
        },
        success: function (data) {
            if (data.d == true) {
                $(btnReferenciaSeleccionada).closest('tr').remove();
                $("#modalEliminarReferencia").modal('hide');
                MensajeExito('La referencia personal ha sido eliminada correctamente');
            }
            else { MensajeError('Error al eliminar referencia personal'); }
        }
    });
});

// #endregion

// #region validaciones de analisis

$("#btnValidoInfoPersonalModal").click(function () {
    /* Verificar si la informacion personal ya fue validada antes */
    if (ESTADO_SOLICITUD.ftAnalisisTiempoValidarInformacionPersonal == '/Date(-2208967200000)/') {
        $("#modalFinalizarValidarPersonal").modal();
    }
});

$("#btnValidoInfoPersonalConfirmar").click(function () {

    if ($($("#comentariosInfoPersonal")).parsley().isValid()) {

        var observacion = $("#comentariosInfoPersonal").val();
        var validacion = 'InformacionPersonal';
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
            data: JSON.stringify({ validacion: validacion, observacion: observacion, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != false) {
                    $("#modalFinalizarValidarPersonal").modal('hide');
                    $("#btnValidoInfoPersonalModal, #btnValidoInfoPersonalConfirmar").prop('disabled', true);
                    $("#btnValidoInfoPersonalModal, #btnValidoInfoPersonalConfirmar").removeClass('btn-warning').addClass('btn-success');
                    $("#btnValidoInfoPersonalModal, #btnValidoInfoPersonalConfirmar").prop('title', 'La información personal ya fue validada');
                    MensajeExito('Estado de la solicitud actualizado');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    } else { $($("#comentariosInfoPersonal")).parsley().validate(); }
});

$("#btnValidoInfoLaboralModal").click(function () {
    /* Verificar si la informacion laboral ya fue validada antes */
    if (ESTADO_SOLICITUD.ftAnalisisTiempoValidarInformacionLaboral == '/Date(-2208967200000)/') {
        $("#modalFinalizarValidarLaboral").modal();
    }
});

$("#btnValidoInfoLaboralConfirmar").click(function () {

    if ($($("#comentariosInfoLaboral")).parsley().isValid()) {

        var observacion = $("#comentariosInfoLaboral").val();
        var validacion = 'InformacionLaboral';
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
            data: JSON.stringify({ validacion: validacion, observacion: observacion, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != false) {
                    $("#modalFinalizarValidarLaboral").modal('hide');
                    $("#btnValidoInfoLaboralModal, #btnValidoInfoLaboralConfirmar").prop('disabled', true);
                    $("#btnValidoInfoLaboralModal, #btnValidoInfoLaboralConfirmar").removeClass('btn-warning').addClass('btn-success');
                    $("#btnValidoInfoLaboralModal, #btnValidoInfoLaboralConfirmar").prop('title', 'La información laboral ya fue validada');
                    MensajeExito('Estado de la solicitud actualizado');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    }
    else { $($("#comentariosInfoLaboral")).parsley().validate(); }
});

$("#btnValidoReferenciasModal").click(function () {
    if (ESTADO_SOLICITUD.ftAnalisisTiempoValidacionReferenciasPersonales == '/Date(-2208967200000)/') {
        $("#modalFinalizarValidarReferencias").modal();
    }
});

$("#btnValidoReferenciasConfirmar").click(function () {

    if ($($("#comentarioReferenciasPersonales")).parsley().isValid()) {

        var observacion = $("#comentarioReferenciasPersonales").val();
        var validacion = 'Referencias';

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
            data: JSON.stringify({ validacion: validacion, observacion: observacion, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != 0) {
                    $("#btnValidoReferenciasModal, #btnValidoReferenciasConfirmar").prop('disabled', true);
                    $("#btnValidoReferenciasModal, #btnValidoReferenciasConfirmar").removeClass('btn-warning').addClass('btn-success');
                    $("#btnValidoReferenciasModal, #btnValidoReferenciasConfirmar").prop('title', 'Las referencias personales ya fueron validadas');
                    $("#modalFinalizarValidarReferencias").modal('hide');
                    MensajeExito('Estado de la solicitud actualizado');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    } else { $($("#comentarioReferenciasPersonales")).parsley().validate(); }
});

$("#btnValidoDocumentacionModal").click(function () {
    $("#modalFinalizarValidarDocumentacion").modal();
});

$("#btnValidoDocumentacionConfirmar").click(function () {

    if ($($("#comentariosDocumentacion")).parsley().isValid() && ESTADO_SOLICITUD.ftAnalisisTiempoValidarDocumentos == '/Date(-2208967200000)/') {

        var observacion = $("#comentariosDocumentacion").val();
        var validacion = 'Documentacion';

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
            data: JSON.stringify({ validacion: validacion, observacion: observacion, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != 0) {
                    $("#btnValidoDocumentacionModal").removeClass('btn-warning').addClass('btn-success');
                    $("#btnValidoDocumentacionConfirmar").removeClass('btn-warning').addClass('btn-primary');
                    $("#btnValidoDocumentacionConfirmar").prop('title', 'La documentación ya fue validada');
                    $("#btnValidoDocumentacionModal").text('Ver docs');
                    $("#btnValidoDocumentacionConfirmar,#btnValidarSoliFisica,#btnValidarLaboral,#btnValidarDomiciliar,#btnValidarIdentidades").prop('disabled', true);
                    $("#btnValidarSoliFisica,#btnValidarLaboral,#btnValidarDomiciliar,#btnValidarIdentidades").prop('title', 'La documentación ya fue validada');
                    $("#modalFinalizarValidarDocumentacion").modal('hide');
                    MensajeExito('Estado de la solicitud actualizado');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    } else { $($("#comentariosDocumentacion")).parsley().validate(); }
});

var tipoDoc = 0;
$(document).on('click', 'button#btnValidarIdentidades,#btnValidarDomiciliar,#btnValidarLaboral,#btnValidarSoliFisica', function () {

    tipoDoc = $(this).data('id');
    $("#modalFinalizarValidarDocumentacion").modal('hide');
    $("#modalValidarTipoDocs").modal();
});

$("#btnValidarTipoDocConfirmar").click(function () {

    var btn = 0;
    var validacion = '';

    switch (tipoDoc) {
        case 1:
            validacion = 'ValidacionDocumentcionIdentidades';
            btn = 1;
            break;
        case 2:
            validacion = 'ValidacionDocumentacionDomicilio';
            btn = 2;
            break;
        case 3:
            validacion = 'ValidacionDocumentacionLaboral';
            btn = 3;
            break;
        case 4:
            validacion = 'ValidacionDocumentacionSolicitudFisica';
            btn = 4;
            break;
        default:
            MensajeError('Error al validar tipo de documentación');
    }

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
        data: JSON.stringify({ validacion: validacion, observacion: '', dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo realizar la validacion, contacte al administrador');
            $("#modalValidarTipoDocs").modal('hide');
            $("#modalFinalizarValidarDocumentacion").modal();
        },
        success: function (data) {
            if (data.d != 0) {
                $("#modalValidarTipoDocs").modal('hide');
                $("#modalFinalizarValidarDocumentacion").modal();

                switch (btn) {
                    case 1:
                        $("#btnValidarIdentidades").prop('disabled', true);
                        $("#btnValidarIdentidades").addClass('text-success');
                        $("#btnValidarIdentidades").prop('title', 'Documentación validada');
                        break;
                    case 2:
                        $("#btnValidarDomiciliar").prop('disabled', true);
                        $("#btnValidarDomiciliar").addClass('text-success');
                        $("#btnValidarDomiciliar").prop('title', 'Documentación validada');
                        break;
                    case 3:
                        $("#btnValidarLaboral").prop('disabled', true);
                        $("#btnValidarLaboral").addClass('text-success');
                        $("#btnValidarLaboral").prop('title', 'Documentación validada');
                        break;
                    case 4:
                        $("#btnValidarSoliFisica").prop('disabled', true);
                        $("#btnValidarSoliFisica").addClass('text-success');
                        $("#btnValidarSoliFisica").prop('title', 'Documentación validada');
                        break;
                }
                MensajeExito('Estado de la solicitud actualizado');
            }
            else {
                MensajeError('Error al actualizar estado de la solicitud, contacte al administrador');
                $("#modalValidarTipoDocs").modal('hide');
                $("#modalFinalizarValidarDocumentacion").modal();
            }
        }
    });
});

// #endregion

//#region Funciones de analisis

/* Cargar buro externo */
$("#btnHistorialExterno").click(function () {

    MensajeInformacion('Cargando buro externo');
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ObtenerUrlEncriptado',
        data: JSON.stringify({ dataCrypt: window.location.href }),
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

/* Guardar Informacion del Perfil */
$("#tipoEmpresa, #tipoPerfil, #tipoEmpleo, #buroActual").change(function () {

    var tipoEmpresa = $("#tipoEmpresa :selected").val();
    var tipoPerfil = $("#tipoPerfil :selected").val();
    var tipoEmpleo = $("#tipoEmpleo :selected").val();
    var buroActual = $("#buroActual :selected").val();

    if (tipoEmpresa != '' && tipoPerfil != '' && tipoEmpleo != '' && buroActual != '') {

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/GuardarInformacionAnalisis",
            data: JSON.stringify({ tipoEmpresa: tipoEmpresa, tipoPerfil: tipoPerfil, tipoEmpleo: tipoEmpleo, buroActual: buroActual, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo guardar la información de perfil, contacte al administrador');
            },
            success: function (data) {
                if (data.d != false) {
                    MensajeExito('Informacion de perfil guardada correctamente');
                    actualizarEstadoSolicitud();
                }
                MensajeError('No se pudo guardar la información de perfil, contacte al administrador');
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

// #region Enviar a campo

/* Enviar solicitud a campo */
$("#btnEnviarCampo").click(function () {
    /* Verificar si la solicitud ya fue enivada a campo antes */
    if (ESTADO_SOLICITUD.fiEstadoDeCampo == 0) {
        $("#modalEnviarCampo").modal();
    }
});

$("#btnEnviarCampoConfirmar").click(function () {

    if ($($("#comentarioAdicional")).parsley().isValid()) {

        var fcObservacionesDeCredito = $("#comentarioAdicional").val();
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/EnviarACampo",
            data: JSON.stringify({ fcObservacionesDeCredito: fcObservacionesDeCredito, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != false) {
                    $("#modalEnviarCampo").modal('hide');
                    $("#btnEnviarCampo, #btnEnviarCampoConfirmar").prop('disabled', true);
                    $("#btnEnviarCampo, #btnEnviarCampoConfirmar").removeClass('btn-warning').addClass('btn-success');
                    $("#btnEnviarCampo, #btnEnviarCampoConfirmar").prop('title', 'La solicitud ya fue enviada a campo');
                    MensajeExito('La solicitud ha sido enviada a campo correctamente');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('Error al enviar la solicitud a campo, contacte al administrador'); }
            }
        });
    } else { $($("#comentarioAdicional")).parsley().validate(); }
});

// #endregion

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
    else { $($("#comentarioRechazar")).parsley().validate(); }
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

//#endregion Funciones utilitarias

// #region Otras funciones

$(window).on('hide.bs.modal', function () {
    /* cuando se abra un modal, ocultar el scroll del BODY y deja solo el del modal (en caso de que este tenga scroll) */
    const body = document.body;
    const scrollY = body.style.top;
    body.style.position = '';
    body.style.top = '';
    window.scrollTo(0, parseInt(scrollY || '0') * -1);
    $("body").css('padding-right', '0');
});

$(window).on('show.bs.modal', function () {
    /* Cuando se cierre el modal */
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
