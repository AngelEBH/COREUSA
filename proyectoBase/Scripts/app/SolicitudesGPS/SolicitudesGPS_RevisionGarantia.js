
InicializarWizard();

$(function () {

    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {

        if (stepPosition === 'first') { /* Si es el primer paso, deshabilitar el boton "anterior" */
            $("#prev-btn").addClass('disabled').css('display', 'none');
        }
        else if (stepPosition === 'final') { /* Si es el ultimo paso, deshabilitar el boton siguiente */
            $("#next-btn").addClass('disabled');
            $("#btnGuardarRevision").css('display', '');
        }
        else { /* Si no es ninguna de las anteriores, habilitar todos los botones */
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
            $("#btnGuardarRevision").css('display', 'none');
        }
    });

    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');
    });
});

var btnResultadoRevision = '';
var idRevision = 0;

$(document).on('click', 'button#btnResultadoRevision', function () {

    btnResultadoRevision = $(this);
    idRevision = btnResultadoRevision.data('id');

    $("#lblRevision").text(btnResultadoRevision.data('revision'));
    $("#lblDescripcionRevision").text(btnResultadoRevision.data('descripcion'));
    $("#txtObservacionesResultadoRevision").val(btnResultadoRevision.data('observaciones'));
    $("#modalActualizarRevision").modal();
});

$("#btnRechazarRevisionConfirmar").click(function () {

    if ($('#txtObservacionesResultadoRevision').parsley().isValid()) {

        ActualizarResultadoRevision(idRevision, 2, $("#txtObservacionesResultadoRevision").val());
        $('#todo-indicator-revision-' + idRevision + ',#badge-revision-' + idRevision).removeClass('bg-warning').removeClass('bg-success').addClass('bg-danger');
        $('#badge-revision-' + idRevision).text('Rechazado');
        btnResultadoRevision.data('observaciones', $("#txtObservacionesResultadoRevision").val());

        $("#modalActualizarRevision").modal('hide');
    }
    else
        $('#txtObservacionesResultadoRevision').parsley().validate({ force: true });
});

$("#btnAprobarRevisionConfirmar").click(function () {

    if ($('#txtObservacionesResultadoRevision').parsley().isValid()) {

        ActualizarResultadoRevision(idRevision, 1, $("#txtObservacionesResultadoRevision").val());
        $('#todo-indicator-revision-' + idRevision + ',#badge-revision-' + idRevision).removeClass('bg-warning').removeClass('bg-danger').addClass('bg-success');
        $('#badge-revision-' + idRevision).text('Aprobado');
        btnResultadoRevision.data('observaciones', $("#txtObservacionesResultadoRevision").val());

        $("#modalActualizarRevision").modal('hide');
    }
    else
        $('#txtObservacionesResultadoRevision').parsley().validate({ force: true });
});


distanciaRecorridaGarantia = '';
unidadDeDistanciaGarantia = '';
$("#btnActualizarMillaje").click(function () {

    $("#txtDistanciaRecorrida").val(distanciaRecorridaGarantia);
    $("#ddlUnidadDeMedida").val(unidadDeDistanciaGarantia);
    $("#modalActualizarMillaje").modal();
});

$("#btnActualizarMillajeConfirmar").click(function () {

    if ($('#frmPrincipal').parsley().isValid({ group: 'actualizarMillaje' })) {

        distanciaRecorridaGarantia = $("#txtDistanciaRecorrida").val().replace(/,/g, '');
        unidadDeDistanciaGarantia = $("#ddlUnidadDeMedida :selected").val();

        $('#todo-indicator-actualizar-millaje,#bg-actualizar-millaje').removeClass('bg-danger').removeClass('bg-warning').addClass('bg-success');
        $('#bg-actualizar-millaje').text('Actualizado');
        $("#modalActualizarMillaje").modal('hide');
    }
    else
        $('#frmPrincipal').parsley().validate({ group: 'actualizarMillaje', force: true });
});

$("#btnConfirmarYEnviar").click(function () {

    var modelStateIsValid = true;

    if (!ValidarTodasLasRevisiones()) {
        MensajeError('Hay revisiones pendientes. Debe completarlas todas.');
        modelStateIsValid = false;
    }

    if (distanciaRecorridaGarantia == '' || unidadDeDistanciaGarantia == '') {
        MensajeError('El millaje todavía no ha sido actualizado.');
        modelStateIsValid = false;
    }

    if (modelStateIsValid) {

        $.ajax({
            type: "POST",
            url: 'SolicitudesGPS_RevisionGarantia.aspx/FinalizarRevisionGarantia',
            data: JSON.stringify({ revisionesGarantia: REVISIONES_GARANTIA, recorrido: distanciaRecorridaGarantia, unidadDeDistancia: unidadDeDistanciaGarantia, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo finalizar la revisión de garanía, contacte al administrador');
            },
            beforeSend: function () {
                MostrarLoader();
            },
            success: function (data) {

                if (data.d.ResultadoExitoso == true)
                    window.location = "SolicitudesGPS_Listado.aspx?" + window.location.href.split('?')[1];
                else
                    MensajeError(data.d.MensajeResultado);
            },
            complete: function () {
                OcultarLoader();
            }
        });
    }
});

function InicializarWizard() {

    $('#smartwizard').smartWizard({
        selected: 0,
        theme: 'default',
        transitionEffect: 'fade',
        showStepURLhash: false,
        autoAdjustHeight: false,
        toolbarSettings: {
            toolbarPosition: 'both',
            toolbarButtonPosition: 'end'
        },
        lang: {
            next: 'Siguiente',
            previous: 'Anterior'
        }
    });
}

function ActualizarResultadoRevision(idRevision, idEstadoRevision, observaciones) {

    let estadoRevision = idEstadoRevision == 1 ? 'Aprobado' : 'Rechazado';

    for (var i = 0; i < REVISIONES_GARANTIA.length; i++) {

        if (REVISIONES_GARANTIA[i].IdRevision == idRevision) {

            REVISIONES_GARANTIA[i].IdEstadoRevision = idEstadoRevision;
            REVISIONES_GARANTIA[i].EstadoRevision = estadoRevision;
            REVISIONES_GARANTIA[i].Observaciones = observaciones;
            break;
        }
    }
}

function ValidarTodasLasRevisiones() {

    /* Validar que a todas las revisiones se les haya determinado un resultado */
    for (var i = 0; i < REVISIONES_GARANTIA.length; i++) {

        if (REVISIONES_GARANTIA[i].IdEstadoRevision == 0)
            return false;
    }
    return true;
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


function MostrarLoader() {
    $("#divLoader").css('display','');
}

function OcultarLoader() {
    $("#divLoader").css('display', 'none');
}