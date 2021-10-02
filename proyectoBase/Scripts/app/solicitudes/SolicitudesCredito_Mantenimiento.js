var idSolicitud = 0;
var idCliente = 0;
var idEstadoSolicitud = 0;

/* Cambiar resultado de campo */
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

    $("#cbReiniciarCampoDomicilio").prop("checked", false);
    $("#cbReiniciarCampoTrabajo").prop("checked", false);
    $("#txtObservacionesReiniciarCampo").val('');
    $("#modalReiniciarCampo").modal();
});

$("#btnReiniciarInvestigacionDeCampo").click(function (e) {

    if ($("#txtObservacionesReiniciarCampo").parsley().isValid()) {

        if ($("#cbReiniciarCampoDomicilio").prop("checked") == true || $("#cbReiniciarCampoTrabajo").prop("checked") == true) {
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


/* Cambiar tasa del préstamo */
$("#btnCambiarTasa").on('click', function () {

    $("#txtObservacionesCambiarTasa").val('');
    $("#modalCambiarTasa").modal();
});

$("#btnCambiarTasaConfirmar").click(function (e) {

    if ($("#txtNuevaTasa").parsley().isValid() && $("#txtObservacionesCambiarTasa").parsley().isValid())
    {
        let TasaNueva = $("#txtNuevaTasa").val();
        let observaciones = $("#txtObservacionesCambiarTasa").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/CambiarTasaPrestamo",
            data: JSON.stringify({ idSolicitud: idSolicitud, Tasa: TasaNueva, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError)
            {
                /*MensajeError("E0001: No se pudo cambiar la tasa, contacte al administrador.");*/
                MensajeError(ajaxOptions);
            },
            success: function (data) {

                if (data.d == true) {
                    MensajeExito('La tasa se cambió correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("E0002: No se pudo cambiar la tasa, contacte al administrador.");
                }

                $("#modalCambiarTasa").modal('hide');
            }
        });

    }
    else {
        $("#txtNuevaTasa").parsley().validate();
        $("#txtObservacionesCambiarFondos").parsley().validate();
    }
});

/* Cambiar fondos del préstamo */
$("#btnCambiarFondos").on('click', function () {

    $("#txtObservacionesCambiarFondos").val('');
    $("#modalCambiarFondos").modal();
});

$("#btnCambiarFondosConfirmar").click(function (e) {

    if ($("#ddlFondos").parsley().isValid() && $("#txtObservacionesCambiarFondos").parsley().isValid()) {

        if ($("#ddlFondos :selected").val()) {

            let idNuevoFondo = $("#ddlFondos :selected").val();
            let observaciones = $("#txtObservacionesCambiarFondos").val();

            $.ajax({
                type: "POST",
                url: "SolicitudesCredito_Mantenimiento.aspx/CambiarFondosPrestamo",
                data: JSON.stringify({ idSolicitud: idSolicitud, idFondo: idNuevoFondo, observaciones: observaciones, dataCrypt: window.location.href }),
                contentType: "application/json; charset=utf-8",
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError("No se pudo cambiar el origen de los fondos, contacte al administrador.");
                },
                success: function (data) {

                    if (data.d == true) {
                        MensajeExito('El origen de los fondos se cambió correctamente.');
                        BuscarSolicitud();
                    }
                    else {
                        MensajeError("No se pudo cambiar el origen de los fondos, contacte al administrador.");
                    }

                    $("#modalCambiarFondos").modal('hide');
                }
            });
        }
        else
            MensajeError('La solicitud ya tiene esta resolución...');
    }
    else {
        $("#ddlFondos").parsley().validate();
        $("#txtObservacionesCambiarFondos").parsley().validate();
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


/* Referencias personales */
$("#btnReferenciasPersonales").on('click', function () {

    $("#modalReferenciasPersonales").modal();
});

/* Manejo de referencias personales */
$("#btnAgregarReferencia").click(function () {

    $("#txtNombreReferencia,#txtTelefonoReferencia,#ddlTiempoDeConocerReferencia, #ddlParentescos, #txtLugarTrabajoReferencia, #txtObservacionesNuevaReferencia").val('');
    $('#modalAgregarReferenciaPersonal').parsley().reset();
    $("#modalReferenciasPersonales").modal('hide');
    $("#modalAgregarReferenciaPersonal").modal();
});

$("#btnAgregarReferenciaConfirmar").click(function () {

    /* Validar formulario de agregar referencia personal */
    if ($('#frmPrincipal').parsley().isValid({ group: 'referenciasPersonales' })) {

        var NombreCompletoReferencia = $("#txtNombreReferencia").val();
        var TelefonoReferencia = $("#txtTelefonoReferencia").val();
        var LugarTrabajoReferencia = $("#txtLugarTrabajoReferencia").val();
        var IdTiempoConocerReferencia = $("#ddlTiempoDeConocerReferencia :selected").val();
        var IdParentescoReferencia = $("#ddlParentescos :selected").val();

        /* Objeto referencia */
        var referenciaPersonal = {
            NombreCompleto: NombreCompletoReferencia,
            TelefonoReferencia: TelefonoReferencia,
            LugarTrabajo: LugarTrabajoReferencia,
            IdTiempoDeConocer: IdTiempoConocerReferencia,
            IdParentescoReferencia: IdParentescoReferencia,
        }

        var observaciones = $("#txtObservacionesNuevaReferencia").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/RegistrarReferenciaPersonal",
            data: JSON.stringify({ idSolicitud: idSolicitud, idCliente: idCliente, referenciaPersonal: referenciaPersonal, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo agregar la referencia personal, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La referencia personal se agregó correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo agregar la referencia personal, contacte al administrador.");
                }

                $("#modalAgregarReferenciaPersonal").modal('hide');
            }
        });
    }
    else {
        $('#frmPrincipal').parsley().validate({ group: 'referenciasPersonales', force: true });
    }

});

/* Eliminar referencia personal */
var idReferenciaPersonalSeleccionada = 0;
$(document).on('click', 'button#btnEliminarReferencia', function () {

    $("#modalReferenciasPersonales").modal('hide');

    $("#txtObservacionesEliminarReferenciaPersonal").val('');
    idReferenciaPersonalSeleccionada = $(this).data('id');

    $("#modalEliminarReferenciaPersonal").modal();
});

$("#btnEliminarReferenciaPersonalConfirmar").click(function (e) {

    if ($("#txtObservacionesEliminarReferenciaPersonal").parsley().isValid() && idReferenciaPersonalSeleccionada != 0) {

        let observaciones = $("#txtObservacionesEliminarReferenciaPersonal").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/EliminarReferenciaPersonal",
            data: JSON.stringify({ idSolicitud: idSolicitud, idCliente: idCliente, idReferenciaPersonal: idReferenciaPersonalSeleccionada, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo eliminar la referencia personal, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La referencia personal fue eliminada correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo eliminar la referencia personal, contacte al administrador.");
                }
                $("#modalEliminarReferenciaPersonal").modal('hide');
            }
        });
    }
    else {
        $("#txtObservacionesEliminarReferenciaPersonal").parsley().validate();
    }
});

/* Actualizar referencia personal */
$(document).on('click', 'button#btnEditarReferencia', function () {

    $("#modalReferenciasPersonales").modal('hide');

    idReferenciaPersonalSeleccionada = $(this).data('id');

    $("#txtNombreReferenciaPersonal_Editar").val($(this).data('nombre'));
    $("#txtTelefonoReferenciaPersonal_Editar").val($(this).data('telefono'));
    $("#ddlTiempoDeConocerReferencia_Editar").val($(this).data('idtiempodeconocer'));
    $("#ddlParentesco_Editar").val($(this).data('idparentesco'));
    $("#txtLugarDeTrabajoReferencia_Editar").val($(this).data('trabajo'));

    $("#txtObservacionesEditarReferenciaPersonal").val('');

    $("#modalEditarReferenciaPersonal").modal();
});

$("#btnEditarReferenciaConfirmar").click(function (e) {

    /* Validar formulario de agregar referencia personal */
    if ($('#frmPrincipal').parsley().isValid({ group: 'referenciasPersonalesEditar' })) {

        var NombreCompletoReferencia = $("#txtNombreReferenciaPersonal_Editar").val();
        var TelefonoReferencia = $("#txtTelefonoReferenciaPersonal_Editar").val();
        var LugarTrabajoReferencia = $("#txtLugarDeTrabajoReferencia_Editar").val();
        var IdTiempoConocerReferencia = $("#ddlTiempoDeConocerReferencia_Editar :selected").val();
        var IdParentescoReferencia = $("#ddlParentesco_Editar :selected").val();

        /* Objeto referencia */
        var referenciaPersonal = {
            IdReferencia: idReferenciaPersonalSeleccionada,
            NombreCompleto: NombreCompletoReferencia,
            TelefonoReferencia: TelefonoReferencia,
            LugarTrabajo: LugarTrabajoReferencia,
            IdTiempoDeConocer: IdTiempoConocerReferencia,
            IdParentescoReferencia: IdParentescoReferencia
        }

        var observaciones = $("#txtObservacionesEditarReferenciaPersonal").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/ActualizarReferenciaPersonal",
            data: JSON.stringify({ idSolicitud: idSolicitud, idCliente: idCliente, referenciaPersonal: referenciaPersonal, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo editar la referencia personal, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La referencia personal se editó correctamente.');
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo editar la referencia personal, contacte al administrador.");
                }

                $("#modalEditarReferenciaPersonal").modal('hide');
            }
        });
    }
    else {
        $('#frmPrincipal').parsley().validate({ group: 'referenciasPersonalesEditar', force: true });
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

                $("#divInformacionSolicitud").css('display', 'none');
                MensajeError("No se pudo realizar la búsqueda, contacte al administrador.");
            },
            success: function (data) {

                if (data.d != null) {

                    var resultado = data.d;

                    idCliente = resultado.IdCliente;

                    if (idCliente == 0) {
                        MensajeError('No se encontraron resultados');
                        return false;
                    }

                    $('#lblIdSolicitud').text('No. ' + idSolicitud);
                    $('#lblIdSolicitud').css('display', '');
                    $("#lblNombreCliente").text(resultado.NombreCliente);
                    $("#lblIdentidadCliente").text(resultado.IdentidadCliente);
                    $("#lblRtn").text(resultado.RtnCliente);
                    $("#lblTelefono").text(resultado.Telefono);
                    $("#lblProducto").text(resultado.Producto);
                    $("#lblTipoDeSolicitud").text(resultado.TipoDeSolicitud);
                    $("#lblAgenciaYVendedorAsignado").text(resultado.Agencia + ' | ' + resultado.UsuarioAsignado);
                    $("#lblGestorAsignado").text(resultado.GestorAsignado);
                    $("#lblFondoActual").text(resultado.Fondo);
                    $("#lblTasaActual").text(resultado.Tasa);
                    $("#ddlFondos").val(resultado.IdFondo);

                    var datatableLenguage = {
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

                    /* Inicializar datatables de condiciones */
                    $('#tblCondiciones').DataTable({
                        "destroy": true,
                        "pageLength": 10,
                        "aaSorting": [],
                        "responsive": true,
                        "language": datatableLenguage,
                        data: resultado.Condiciones,
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
                                    return row["EstadoCondicion"] != true ? '' : '<button id="btnAnularCondicion" data-id="' + row["IdSolicitudCondicion"] + '" class="btn btn-sm btn-danger mb-0" type="button" title="Anular condición"><i class="far fa-trash-alt"></i></button>';
                                }
                            },
                        ]
                    });

                    /* Inicializar datatables de documentos */
                    $('#tblDocumenacionSolicitud').DataTable({
                        "destroy": true,
                        "pageLength": 10,
                        "aaSorting": [],
                        "responsive": true,
                        "language": datatableLenguage,
                        data: resultado.Documentos,
                        "columns": [
                            { "data": "DescripcionTipoDocumento" },
                            { "data": "NombreArchivo" },
                            {
                                "data": "URLArchivo", "className": "text-center",
                                "render": function (data, type, row) {
                                    return '<button class="btn btn-sm btn-secondary" data-url="' + row["URLArchivo"] + (row["IdTipoDocumento"] == 8 || row["IdTipoDocumento"] == 9 ? '.jpg' : '') + '" data-descripcion="' + row["DescripcionTipoDocumento"] + '" onclick="MostrarVistaPrevia(this)" type="button">Abrir <i class="fas fa-search pl-1"></i></button>'
                                }
                            },
                            {
                                "data": "IdSolicitudDocumento", "className": "text-center",
                                "render": function (value) {
                                    return '<button id="btnEliminarDocumento" data-id="' + value + '" class="btn btn-sm btn-danger mb-0" type="button" title="Eliminar documento"><i class="far fa-trash-alt"></i></button>'
                                }
                            },
                        ],
                        columnDefs: [
                            { targets: 'no-sort', orderable: false },
                        ]
                    });

                    /* Inicializar datatables de Referencias personales de la solicitud */
                    $('#tblReferenciasPersonales').DataTable({
                        "destroy": true,
                        "pageLength": 10,
                        "aaSorting": [],
                        "responsive": true,
                        "dom": "<'row'<'col-sm-12'>>" +
                            "<'row'<'col-sm-12'tr>>" +
                            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
                        "language": datatableLenguage,
                        data: resultado.ReferenciasPersonales,
                        "columns": [
                            { "data": "NombreCompleto" },
                            { "data": "LugarTrabajo", "className": "text-center" },
                            { "data": "TelefonoReferencia" },
                            { "data": "TiempoDeConocer" },
                            { "data": "DescripcionParentesco" },
                            {
                                "data": "IdReferencia", "className": "text-center",
                                "render": function (data, type, row) {
                                    return '<button id="btnEliminarReferencia" data-id="' + row["IdReferencia"] + '" class="btn btn-sm btn-danger mb-0" type="button" title="Eliminar referencia personal"><i class="far fa-trash-alt"></i></button> ' +
                                        '<button id="btnEditarReferencia" data-id="' + row["IdReferencia"] + '" data-nombre="' + row["NombreCompleto"] + '" data-trabajo="' + row["LugarTrabajo"] + '" data-telefono="' + row["TelefonoReferencia"] + '" data-idtiempodeconocer="' + row["IdTiempoDeConocer"] + '" data-idparentesco="' + row["IdParentescoReferencia"] + '" class="btn btn-sm btn-info mb-0" type="button" title="Editar referencia personal"><i class="far fa-edit"></i></button>';
                                }
                            },
                        ],
                        columnDefs: [
                            { targets: 'no-sort', orderable: false },
                        ]
                    });

                    /* Resolución de la solicitud */
                    $("#ddlCatalogoResoluciones").val(resultado.IdEstadoSolicitud);
                    idEstadoSolicitud = resultado.IdEstadoSolicitud;

                    /* Dtatable Historial de mantenimiento */
                    $('#tblHistorialMantenimiento').DataTable({
                        "destroy": true,
                        "pageLength": 10,
                        "aaSorting": [],
                        "responsive": true,
                        "language": datatableLenguage,
                        data: resultado.HistorialMantenimientos,
                        "columns": [
                            {
                                "data": "FechaMantenimiento",
                                "render": function (value) {
                                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm a')
                                }
                            },
                            { "data": "AgenciaUsuario" },
                            { "data": "NombreUsuario" },
                            { "data": "Observaciones" }
                        ],
                        columnDefs: [
                            { targets: 'no-sort', orderable: false },
                        ]
                    });

                    $("#divInformacionSolicitud").css('display', '');

                } // if data.d != null
                else {
                    $("#divInformacionSolicitud").css('display', 'none');
                    MensajeError('No se pudo cargar la información, contacte al administrador.');
                }
            },// success
            complete: function () {
                OcultarLoader();
                $('#btnBuscarSolicitud').prop('disabled', false);
            }
        }); // ajax
    } // if txtNoSolicitud != ''
    else {
        $("#txtNoSolicitud").focus();
    }
}

function MostrarVistaPrevia(btnVistaPrevia) {

    let urlImagen = $(btnVistaPrevia).data('url');
    let descripcion = $(btnVistaPrevia).data('descripcion');
    let imgTemplate = '<img alt="' + descripcion + '" src="' + urlImagen + '" data-image="' + urlImagen + '" data-description="' + descripcion + '"/>';


    $("#lblTipoDocumentoVistaPrevia").text(descripcion);
    $("#divImgVistaPrevia").empty().append(imgTemplate).unitegallery();

    $("#modalDocumentacionSolicitud").modal('hide');
    $("#modalVistaPreviaDocumento").modal();
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