﻿var idSolicitud = 0;
var idEstadoSolicitud = 0;

$(document).ready(function () {

    $('#btnBuscarSolicitud').click(function () {

        BuscarSolicitud();

    });
});

$("#btnResolucionCampo").click(function (e) {

    $("#ddlResolucionCampo").val('');
    $("#txtObservacioneResolucionCampo").val('');
    $("#modalResolucionCampo").modal();
});

$("#btnResolucionCampoConfirmar").click(function () {

    if ($("#ddlResolucionCampo").parsley().isValid() && $("#txtObservacioneResolucionCampo").parsley().isValid()) {

        let resolucionCampo = $("#ddlResolucionCampo :selected").val();
        let observaciones = $("#txtObservacioneResolucionCampo").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/ResolucionCampo",
            data: JSON.stringify({ idSolicitud: idSolicitud, resolucionCampo: resolucionCampo, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo guardar la resolución de campo, contacte al administrador.");
            },
            success: function (data) {

                if (data.d != null) {

                    MensajeExito('La resolución de investigación de campo se guardó correctamente.');
                    $("#btnResolucionCampo").css('disabled', false);
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo guardar la resolución de campo, contacte al administrador.");
                }

                $("#modalResolucionCampo").modal('hide');
            }
        });
    }
    else {
        $("#ddlResolucionCampo").parsley().validate();
        $("#txtObservacioneResolucionCampo").parsley().validate();
    }
});


/* Asignar gestor */
$("#btnReasignarGestor").on('click', function () {

    $("#ddlGestores").val();
    $("#modalAsignarGestorSolicitud").modal();
});

$("#btnAsignarGestor_Confirmar").click(function (e) {

    if ($("#ddlGestores").parsley().isValid()) {

        let idGestor = $("#ddlGestores :selected").val();
        let observaciones = $("#txtObservacionAsignarGestor").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/AsignarGestorSolicitud",
            data: JSON.stringify({ idSolicitud: idSolicitud, idGestor: idGestor, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudor asignar el gestor, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La solicitud fue asignada correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo asignar el gestor, contacte al administrador.");
                }

                $("#modalAsignarGestorSolicitud").modal('hide');
            }
        });
    }
    else {
        $("#ddlGestores").parsley().validate();
    }
});

/* Reasignar vendedor */
$("#btnReasignarVendedor").on('click', function () {

    $("#ddlVendedores").val();
    $("#modalAsignarVendedorSolicitud").modal();
});

$("#btnAsignarVendedores_Confirmar").click(function (e) {

    if ($("#ddlVendedores").parsley().isValid()) {

        let idUsuarioAsignado = $("#ddlVendedores :selected").val();
        let observaciones = $("#txtObservacionesReasignarVendedor").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/ReasignarVendedorSolicitud",
            data: JSON.stringify({ idSolicitud: idSolicitud, idUsuarioAsignado: idUsuarioAsignado, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo asignar la solicitud, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La solicitud fue asignada correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo asignar la solicitud, contacte al administrador.");
                }

                $("#modalAsignarVendedorSolicitud").modal('hide');
            }
        });
    }
    else {
        $("#ddlVendedores").parsley().validate();
    }
});

/* Anular condiciones */
$("#btnCondiciones").on('click', function () {

    $("#modalAnularCondicion").modal();
});

/* Anular condiciones Confirmar */
var idCondicionSeleccionada = 0;
$(document).on('click', 'button#btnAnularCondicion', function () {

    $("#modalAnularCondicion").modal('hide');

    $("#txtObservacionesAnularCondicion").val('');
    idCondicionSeleccionada = $(this).data('id');

    $("#modalAnularCondicionConfirmar").modal();
});

$("#btnAnularCondicionConfirmar").click(function (e) {

    if ($("#txtObservacionesAnularCondicion").parsley().isValid() && idCondicionSeleccionada != 0) {

        let observaciones = $("#txtObservacionesAnularCondicion").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/AnularCondicion",
            data: JSON.stringify({ idSolicitud: idSolicitud, idSolicitudCondicion: idCondicionSeleccionada, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo anular la condición, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La condición fue anulada correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo anular la condición, contacte al administrador.");
                }
                $("#modalAnularCondicionConfirmar").modal('hide');
            }
        });
    }
    else {
        $("#txtObservacionesAnularCondicion").parsley().validate();
    }
});


/* Eliminar documentos */
$("#btnSolicitudDocumentos").on('click', function () {

    $("#modalDocumentacionSolicitud").modal();
});

/* Eliminar documentos Confirmar */
var idDocumentoSeleccionado = 0;
$(document).on('click', 'button#btnEliminarDocumento', function () {

    $("#modalDocumentacionSolicitud").modal('hide');

    $("#txtObservacionesEliminarDocumento").val('');
    idDocumentoSeleccionado = $(this).data('id');

    $("#modalEliminarDocumentoSolicitud").modal();
});

$("#btnEliminarDocumentoConfirmar").click(function (e) {

    if ($("#txtObservacionesEliminarDocumento").parsley().isValid() && idDocumentoSeleccionado != 0) {

        let observaciones = $("#txtObservacionesEliminarDocumento").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/EliminarDocumento",
            data: JSON.stringify({ idSolicitud: idSolicitud, idSolicitudDocumento: idDocumentoSeleccionado, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo eliminar el documento, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('El documento fue eliminado correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se eliminar el documento, contacte al administrador.");
                }
                $("#modalEliminarDocumentoSolicitud").modal('hide');
            }
        });
    }
    else {
        $("#txtObservacionesEliminarDocumento").parsley().validate();
    }
});

/* Referencias personales */
$("#btnReferenciasPersonales").on('click', function () {

    $("#modalReferenciasPersonales").modal();
});


/* Cambiar resolucion */
$("#btnReiniciarResolucion").on('click', function () {

    $("#modalCambiarResolucion").modal();
});


$("#btnCambiarResolucionConfirmar").click(function (e) {

    if ($("#ddlCatalogoResoluciones").parsley().isValid() && $("#txtObservacionesCambiarResolucionSolicitud").parsley().isValid()) {

        if ($("#ddlCatalogoResoluciones :selected").val() != idEstadoSolicitud) {

            let idNuevaResolucion = $("#ddlCatalogoResoluciones :selected").val();
            let observaciones = $("#txtObservacionesCambiarResolucionSolicitud").val();

            $.ajax({
                type: "POST",
                url: "SolicitudesCredito_Mantenimiento.aspx/CambiarResolucionSolicitud",
                data: JSON.stringify({ idSolicitud: idSolicitud, idNuevaResolucion: idNuevaResolucion, observaciones: observaciones, dataCrypt: window.location.href }),
                contentType: "application/json; charset=utf-8",
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError("No se pudo cambiar la resolución de la solicitud, contacte al administrador.");
                },
                success: function (data) {

                    if (data.d == true) {

                        MensajeExito('La solicitud fue asignada correctamente.');
                        BuscarSolicitud();
                    }
                    else {
                        MensajeError("No se pudo cambiar la resolucion de la solicitud, contacte al administrador.");
                    }

                    $("#modalCambiarResolucion").modal('hide');
                }
            });

        }
        else {
            MensajeError('La solicitud ya tiene esta resolución...');
        }
    }
    else {
        $("#ddlCatalogoResoluciones").parsley().validate();
        $("#txtObservacionesCambiarResolucionSolicitud").parsley().validate();
    }
});


/* Reiniciar investigacion de campo */
$("#btnReiniciarCampo").on('click', function () {

    $("#cbReiniciarCampoDomicilio").prop("checked",false);
    $("#cbReiniciarCampoTrabajo").prop("checked", false);
    $("#txtObservacionesReiniciarCampo").val('');
    $("#modalReiniciarCampo").modal();
});


$("#btnReiniciarInvestigacionDeCampo").click(function (e) {

    if ($("#txtObservacionesReiniciarCampo").parsley().isValid()) {

        if ($("#cbReiniciarCampoDomicilio").prop("checked") == true || $("#cbReiniciarCampoTrabajo").prop("checked") == true)
        {
            let reiniciarInvestigacionDomicilio = $("#cbReiniciarCampoDomicilio").prop("checked");
            let reiniciarInvestigacionTrabajo = $("#cbReiniciarCampoTrabajo").prop("checked");
            let observaciones = $("#txtObservacionesReiniciarCampo").val();

            $.ajax({
                type: "POST",
                url: "SolicitudesCredito_Mantenimiento.aspx/ReiniciarCampo",
                data: JSON.stringify({ idSolicitud: idSolicitud, reiniciarInvestigacionDomicilio: reiniciarInvestigacionDomicilio, reiniciarInvestigacionTrabajo: reiniciarInvestigacionTrabajo, observaciones: observaciones, dataCrypt: window.location.href }),
                contentType: "application/json; charset=utf-8",
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError("No se pudo reiniciar la investigación de campo, contacte al administrador.");
                },
                success: function (data) {

                    if (data.d == true) {

                        MensajeExito('La investigación de campo se reinció correctamente.');
                        BuscarSolicitud();
                    }
                    else {
                        MensajeError("No se pudo reiniciar la investigación de campo, contacte al administrador.");
                    }

                    $("#modalReiniciarCampo").modal('hide');
                }
            });

        }
        else {
            MensajeError('Debes seleccionar por lo menos una opción');
        }
    }
    else {
        $("#txtObservacionesReiniciarCampo").parsley().validate();
    }
});


/* Reiniciar reprogramación */
$("#btnReiniciarReprogramacion").on('click', function () {

    $("#txtObservacionesReiniciarReprogramacion").val('');
    $("#modalReiniciarReprogramacion").modal();
});


$("#btnReiniciarReprogramacionConfirmar").click(function (e) {

    if ($("#txtObservacionesReiniciarReprogramacion").parsley().isValid()) {

        let observaciones = $("#txtObservacionesReiniciarReprogramacion").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/ReiniciarReprogramacion",
            data: JSON.stringify({ idSolicitud: idSolicitud, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo reiniciar la reprogramacion, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La reprogramación se reinció correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo reiniciar la reprogramacion, contacte al administrador.");
                }

                $("#modalReiniciarReprogramacion").modal('hide');
            }
        });        
    }
    else {
        $("#txtObservacionesReiniciarReprogramacion").parsley().validate();
    }
});


/* Reiniciar validacion */
$("#btnReiniciarValidacion").on('click', function () {

    $("#txtObservacionesReiniciarReprogramacion").val('');
    $("#modalReiniciarValidacion").modal();
});


$("#btnReiniciarValidacionConfirmar").click(function (e) {

    if ($("#txtObservacionesReiniciarValidacion").parsley().isValid()) {

        let observaciones = $("#txtObservacionesReiniciarValidacion").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/ReiniciarValidacion",
            data: JSON.stringify({ idSolicitud: idSolicitud, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo reiniciar la validación, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La validación se reinció correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo reiniciar la validación, contacte al administrador.");
                }

                $("#modalReiniciarValidacion").modal('hide');
            }
        });
    }
    else {
        $("#txtObservacionesReiniciarValidacion").parsley().validate();
    }
});


/* Reiniciar analisis */
$("#btnReiniciarAnalisis").on('click', function () {

    $("#cbValidacionInformacionPersonal,#cbValidacionInformacionLaboral,#cbValidacionReferenciasPersonales,#cbValidacionDocumentos").prop("checked", false);
    $("#txtObservacionesReiniciarAnalisis").val('');
    $("#modalReiniciarAnalisis").modal();
});


$("#btnReiniciarAnalisisConfirmar").click(function (e) {

    if ($("#txtObservacionesReiniciarAnalisis").parsley().isValid()) {

        if ($("#cbValidacionInformacionLaboral").prop("checked") == true || $("#cbValidacionInformacionPersonal").prop("checked") == true || $("#cbValidacionReferenciasPersonales").prop("checked") == true || $("#cbValidacionDocumentos").prop("checked") == true) {
            let reiniciarInfoPersonal = $("#cbValidacionInformacionPersonal").prop("checked");
            let reiniciarInfoLaboral = $("#cbValidacionInformacionLaboral").prop("checked");
            let reiniciarReferencias = $("#cbValidacionReferenciasPersonales").prop("checked");
            let reiniciarDocumentacion = $("#cbValidacionDocumentos").prop("checked");
            let observaciones = $("#txtObservacionesReiniciarAnalisis").val();

            $.ajax({
                type: "POST",
                url: "SolicitudesCredito_Mantenimiento.aspx/ReiniciarAnalisis",
                data: JSON.stringify({ idSolicitud: idSolicitud, reiniciarInfoPersonal: reiniciarInfoPersonal, reiniciarInfoLaboral: reiniciarInfoLaboral, reiniciarReferencias: reiniciarReferencias, reiniciarDocumentacion: reiniciarDocumentacion, observaciones: observaciones, dataCrypt: window.location.href }),
                contentType: "application/json; charset=utf-8",
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError("No se pudo reiniciar el análisis, contacte al administrador.");
                },
                success: function (data) {

                    if (data.d == true) {

                        MensajeExito('El análisis se reinció correctamente.');
                        BuscarSolicitud();
                    }
                    else {
                        MensajeError("No se pudo reiniciar el análisis, contacte al administrador.");
                    }

                    $("#modalReiniciarAnalisis").modal('hide');
                }
            });

        }
        else {
            MensajeError('Debes seleccionar por lo menos una opción');
        }
    }
    else {
        $("#txtObservacionesReiniciarAnalisis").parsley().validate();
    }
});

function BuscarSolicitud() {

    if ($('#txtNoSolicitud').val() != '') {

        MostrarLoader();

        $('#btnBuscarSolicitud').prop('disabled', true);

        idSolicitud = $('#txtNoSolicitud').val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/CargarInformacion",
            data: JSON.stringify({ idSolicitud: idSolicitud, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {

                $("#btnBuscarSolicitud").prop('disabled', false);
                $("#divInformacionSolicitud").css('display', 'none');
                MensajeError("No se pudo realizar la búsqueda, contacte al administrador.");
                OcultarLoader();
            },
            success: function (data) {

                if (data.d != null) {

                    var resultado = data.d;

                    $('#lblIdSolicitud').text('No. ' + idSolicitud);
                    $('#lblIdSolicitud').css('display', '');
                    $("#txtNombreCliente").val(resultado.NombreCliente);
                    $("#txtIdentidadCliente").val(resultado.IdentidadCliente);
                    $("#txtRtn").val(resultado.RtnCliente);
                    $("#txtTelefono").val(resultado.Telefono);
                    $("#txtProducto").val(resultado.Producto);
                    $("#txtTipoDeSolicitud").val(resultado.TipoDeSolicitud);
                    $("#txtAgencia").val(resultado.Agencia);
                    $("#txtAgenteAsignado").val(resultado.UsuarioAsignado);
                    $("#txtGestorAsignado").val(resultado.GestorAsignado);

                    /* Condiciones de la solicitud */
                    var tblCondiciones = $("#tblCondiciones tbody");
                    tblCondiciones.empty();
                    tblCondiciones.append('<tr><td class="text-center" colspan="5">No hay registros disponibles...</td></tr>');

                    if (resultado.Condiciones != null) {
                        if (resultado.Condiciones.length > 0) {

                            var condiciones = resultado.Condiciones;
                            let templateCondiciones = '';
                            let estadoCondicion = '';
                            let btnAnularCondicion = '';

                            tblCondiciones.empty();

                            for (var i = 0; i < condiciones.length; i++) {

                                estadoCondicion = condiciones[i].EstadoCondicion != true ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-warning mb-0'>Pendiente</label>"

                                btnAnularCondicion = condiciones[i].EstadoCondicion != true ? '' : '<button id="btnAnularCondicion" data-id="' + condiciones[i].IdSolicitudCondicion + '" class="btn btn-sm btn-danger mb-0" type="button" title="Anular condición"><i class="far fa-trash-alt"></i></button>';

                                templateCondiciones += '<tr><td>' + condiciones[i].Condicion + '</td><td>' + condiciones[i].DescripcionCondicion + '</td><td>' + condiciones[i].ComentarioAdicional + '</td><td>' + estadoCondicion + '</td><td class="text-center">' + btnAnularCondicion + '</td></tr>';
                            }

                            tblCondiciones.append(templateCondiciones);
                        }
                    }


                    /* Documentación de la solicitud */
                    var tblDocumentos = $("#tblDocumenacionSolicitud tbody");
                    tblDocumentos.empty();
                    tblDocumentos.append('<tr><td class="text-center" colspan="3">No hay registros disponibles...</td></tr>');
                    let btnEliminarDocumento = '';

                    if (resultado.Documentos != null) {
                        if (resultado.Documentos.length > 0) {

                            var documentos = resultado.Documentos;
                            let templateDocumentos = '';

                            tblDocumentos.empty();

                            for (var i = 0; i < documentos.length; i++) {

                                btnEliminarDocumento = '<button id="btnEliminarDocumento" data-id="' + documentos[i].IdSolicitudDocumento + '" class="btn btn-sm btn-danger mb-0" type="button" title="Eliminar documento"><i class="far fa-trash-alt"></i></button>';

                                templateDocumentos += '<tr><td>' + documentos[i].DescripcionTipoDocumento + '</td><td>' + documentos[i].NombreArchivo + '</td><td><a href="' + documentos[i].URLArchivo + '" target="_blank"> Abrir</a></td><td class="text-center">' + btnEliminarDocumento + '</td></tr>';
                            }

                            tblDocumentos.append(templateDocumentos);
                        }
                    }

                    /* Referencias personales de la solicitud */
                    var tblReferenciasPersonales = $("#tblReferneciasPersonales tbody");
                    tblReferenciasPersonales.empty();
                    tblReferenciasPersonales.append('<tr><td class="text-center" colspan="6">No hay registros disponibles...</td></tr>');

                    if (resultado.ReferenciasPersonales != null) {
                        if (resultado.ReferenciasPersonales.length > 0) {

                            var referenciasPersonales = resultado.ReferenciasPersonales;
                            let templateReferenciasPersonales = '';
                            let btnEliminarReferencia = '';
                            let btnEditarReferencia = '';

                            tblReferenciasPersonales.empty();

                            for (var i = 0; i < referenciasPersonales.length; i++) {

                                btnEliminarReferencia = '<button id="btnEliminarReferencia" disabled="disabled" data-id="' + referenciasPersonales[i].IdReferencia + '" class="btn btn-sm btn-danger mb-0" type="button" title="Opción no disponible"><i class="far fa-trash-alt"></i></button>';
                                btnEditarReferencia = '<button id="btnEditarReferencia" disabled="disabled" data-id="' + referenciasPersonales[i].IdReferencia + '" class="btn btn-sm btn-info mb-0" type="button" title="Opción no disponible"><i class="far fa-edit"></i></button>';

                                templateReferenciasPersonales += '<tr><td><small>' + referenciasPersonales[i].NombreCompleto + '</small></td><td><small>' + referenciasPersonales[i].LugarTrabajo + '</small></td><td><small>' + referenciasPersonales[i].TelefonoReferencia + '</small></td><td><small>' + referenciasPersonales[i].TiempoDeConocer + '</small></td><td><small>' + referenciasPersonales[i].DescripcionParentesco + '</small></td><td class="text-center">' + btnEliminarReferencia + btnEditarReferencia + '</td></tr>';
                            }

                            tblReferenciasPersonales.append(templateReferenciasPersonales);
                        }
                    }

                    /* Resolución de la solicitud */
                    $("#ddlCatalogoResoluciones").val(resultado.IdEstadoSolicitud);
                    idEstadoSolicitud = resultado.IdEstadoSolicitud;

                    /* Historial de mantenimiento */
                    var tblHistorialMantenimiento = $("#tblHistorialMantenimiento tbody");
                    tblHistorialMantenimiento.empty();
                    tblHistorialMantenimiento.append('<tr><td class="text-center" colspan="4">No hay registros disponibles...</td></tr>');

                    if (resultado.HistorialMantenimientos != null) {
                        if (resultado.HistorialMantenimientos.length > 0) {

                            tblHistorialMantenimiento.empty();

                            var historialMantenimiento = resultado.HistorialMantenimientos;

                            var template = '';

                            $.each(historialMantenimiento, function (i, iter) {

                                template += '<tr><td>' + moment(iter.FechaMantenimiento).locale('es').format('YYYY/MM/DD hh:mm:ss a') + '</td><td>' + iter.AgenciaUsuario + '</td><td>' + iter.NombreUsuario + '</td><td>' + iter.Observaciones + '</td></tr>';
                            });

                            tblHistorialMantenimiento.append(template);
                        }
                    }

                    $("#divInformacionSolicitud").css('display', '');

                } // if data.d != null
                else {
                    $("#divInformacionSolicitud").css('display', 'none');
                    MensajeError('No se pudo cargar la información, contacte al administrador.');
                }
                $('#btnBuscarSolicitud').prop('disabled', false);
                OcultarLoader();

            }// success
        }); // ajax
    } // if txtNoSolicitud != ''
    else {
        $("#txtNoSolicitud").focus();
    }
}




function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function MensajeExito(mensaje) {
    iziToast.success({
        title: 'Exito',
        message: mensaje
    });
}

function MostrarLoader() {
    $("#Loader").css('display', '');
}

function OcultarLoader() {
    $("#Loader").css('display', 'none');
}