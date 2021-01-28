
var btnFinalizar = $('<button type="button" id="btnGuardarInstalacionGPS"></button>').text('Finalizar').addClass('btn btn-info').css('display', 'none')
    .on('click', function () {

        var modelState = $('#frmPrincipal').parsley().isValid();

        if (modelState) {

            var instalacionGPS = {
                DescripcionUbicacion: $("#txtUbicacion").val(),
                Comentarios: $("#txtComentariosDeLaInstalacion").val()
            }

            $.ajax({
                type: "POST",
                url: 'SolicitudesGPS_RegistroInstalacionGPS.aspx/CargarListaFotografiasRequeridas',
                data: JSON.stringify({ instalacionGPS: instalacionGPS, dataCrypt: window.location.href }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError('No se pudo conectar al servidor, contacte al administrador');
                },
                success: function (data) {

                    let resultado = data.d;

                    if (resultado.ResultadoExitoso == true) {

                        window.location = "SolicitudesGPS_Listado.aspx?" + window.location.href.split('?')[1];
                    }
                    else {
                        MensajeError(resultado.MensajeResultado);
                        console.log(resultado.MensajeDebug);
                    }
                }
            });
        }
        else
            $('#frmPrincipal').parsley().validate();
    });

/* Inicalizar el Wizard */
$('#smartwizard').smartWizard({
    selected: 0,
    theme: 'default',
    transitionEffect: 'fade',
    showStepURLhash: false,
    autoAdjustHeight: false,
    toolbarSettings: {
        toolbarPosition: 'both',
        toolbarButtonPosition: 'end',
        toolbarExtraButtons: [btnFinalizar]
    },
    lang: {
        next: 'Siguiente',
        previous: 'Anterior'
    }
});

$(document).ready(function () {

    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {

        /* Si es el primer paso, deshabilitar el boton "anterior" */
        if (stepPosition === 'first') {
            $("#prev-btn").addClass('disabled').css('display', 'none');
        }
        else if (stepPosition === 'final') { /* Si es el ultimo paso, deshabilitar el boton siguiente */
            $("#next-btn").addClass('disabled');
            $("#btnGuardarInstalacionGPS").css('display', '');
        }
        else { /* Si no es ninguna de las anteriores, habilitar todos los botones */
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
            $("#btnGuardarInstalacionGPS").css('display', 'none');
        }
    });

    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');
    });

    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

        if (stepDirection == 'forward') {

            if (stepNumber == 0) {

                var state = ($('#frmPrincipal').parsley().isValid()) ? true : false;

                if (state == false) {

                    $('#frmPrincipal').parsley().validate();
                }
                return state;
            }
        }
    });

    CargarDocumentosRequeridos();
});

function CargarDocumentosRequeridos() {

    $.ajax({
        type: "POST",
        url: "SolicitudesGPS_RegistroInstalacionGPS.aspx/CargarListaFotografiasRequeridas",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar el listado de documentos requeridos, contacte al administrador');
        },
        success: function (data) {

            var LenguajeEspanol = {
                feedback: 'Arrastra y suelta los archivos aqui',
                feedback2: 'Arrastra y suelta los archivos aqui',
                drop: 'Arrastra y suelta los archivos aqui',
                button: 'Buscar archivos',
                confirm: 'Confirmar',
                cancel: 'Cancelar'
            }

            var formatoInputFile = '<div class="form-group">' +
                '<input type="file" class="filestyle" data-buttonname="btn-secondary" id="filestyle-0" tabindex="-1" style="position: absolute; clip: rect(0px, 0px, 0px, 0px);"/>' +
                '<div class="bootstrap-filestyle input-group">' +
                '<input type="text" class="form-control " placeholder="" disabled=""/>' +
                '<span class="group-span-filestyle input-group-append" tabindex="0">' +
                '<label for="filestyle-0" class="btn btn-secondary">' +
                '<span class="icon-span-filestyle fas fa-folder-open"></span>' +
                '<span class="buttonText">Subir archivo</span>' +
                '</label>' +
                '</span>' +
                '</div>' +
                '</div>';

            var divDocumentacion = $("#DivDocumentacion");

            $.each(data.d, function (i, iter) {

                var idInput = 'Documento' + iter.IdFotografia;

                divDocumentacion.append(
                    '<form action="SolicitudesGPS_RegistroInstalacionGPS.aspx?type=upload&idfotografia=' + iter.IdFotografia + ' method="post" enctype="multipart/form-data">' +                    
                    '<label class="font-weight-bold">' + iter.DescripcionFotografia + '</label>' +
                    '<input type="file" class="filestyle" data-buttonname="btn-secondary" id="' + idInput + '" name="files" data-tipo="' + iter.IdFotografia + '"/>' +                    
                    '</form>');

                $('#' + idInput + '').fileuploader({
                    inputNameBrackets: false,
                    changeInput: formatoInputFile,
                    theme: 'dragdrop',
                    limit: 1, // Limite de archivos a subir
                    maxSize: 200, // Peso máximo de todos los archivos seleccionado en megas (MB)
                    fileMaxSize: 20, // Peso máximo de un archivo
                    extensions: ['jpg', 'png', 'jpeg'],// Extensiones/formatos permitidos
                    upload: {
                        url: 'SolicitudesGPS_RegistroInstalacionGPS.aspx?type=upload&idfotografia=' + iter.IdFotografia,
                        data: null,
                        type: 'POST',
                        enctype: 'multipart/form-data',
                        start: true,
                        synchron: true,
                        beforeSend: null,
                        onSuccess: function (result, item) {
                            var data = {};
                            try {
                                data = JSON.parse(result);
                            } catch (e) {
                                data.hasWarnings = true;
                            }

                            /* Validar exito */
                            if (data.isSuccess && data.files[0]) {
                                item.name = data.files[0].name;
                                item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                            }

                            /* Validar si se produjo un error */
                            if (data.hasWarnings) {
                                for (var warning in data.warnings) {
                                    alert(data.warnings);
                                }
                                item.html.removeClass('upload-successful').addClass('upload-failed');
                                return this.onError ? this.onError(item) : null;
                            }

                            item.html.find('.fileuploader-action-remove').addClass('fileuploader-action-success');
                            setTimeout(function () {
                                item.html.find('.progress-bar2').fadeOut(400);
                            }, 400);
                        },
                        onError: function (item) {
                            var progressBar = item.html.find('.progress-bar2');

                            if (progressBar.length) {
                                progressBar.find('span').html(0 + "%");
                                progressBar.find('.fileuploader-progressbar .bar').width(0 + "%");
                                item.html.find('.progress-bar2').fadeOut(400);
                            }

                            item.upload.status != 'cancelled' && item.html.find('.fileuploader-action-retry').length == 0 ? item.html.find('.column-actions').prepend(
                                '<button type="button" class="fileuploader-action fileuploader-action-retry" title="Retry"><i class="fileuploader-icon-retry"></i></button>'
                            ) : null;
                        },
                        onProgress: function (data, item) {
                            var progressBar = item.html.find('.progress-bar2');

                            if (progressBar.length > 0) {
                                progressBar.show();
                                progressBar.find('span').html(data.percentage + "%");
                                progressBar.find('.fileuploader-progressbar .bar').width(data.percentage + "%");
                            }
                        },
                        onComplete: null,
                    },
                    onRemove: function (item) {
                        $.post('SolicitudesGPS_RegistroInstalacionGPS.aspx?type=remove', { file: item.name });
                    },
                    dialogs: {
                        alert: function (text) {
                            return iziToast.warning({
                                title: 'Atencion',
                                message: text
                            });
                        },
                        confirm: function (text, callback) {
                            confirm(text) ? callback() : null;
                        }
                    },
                    captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], LenguajeEspanol)

                }); /* Termina fileUploader*/

            }); /* Termina .Each*/
        }
    }); /* Termina Ajax */
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

function MensajeInformacion(mensaje) {
    iziToast.info({
        title: 'Info',
        message: mensaje
    });
}

function MostrarLoader() {

    $("#Loader").css('display', '');
}

function OcultarLoader() {

    $("#Loader").css('display', 'none');
}