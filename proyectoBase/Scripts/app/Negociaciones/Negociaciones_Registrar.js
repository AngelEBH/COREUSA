$(document).ready(function () {
    CargarMascarasDeEntrada();
    InicializarCargaDeArchivos();
});

/* Exportar COTIZACIÓN a PDF */
function ExportToPDF(fileName) {

    const cotizacion = this.document.getElementById("divCotizacionPDF");
    var opt = {
        margin: 0.3,
        filename: fileName + '.pdf',
        image: { type: 'jpeg', quality: 0.98 },
        html2canvas: { scale: 1 },
        jsPDF: { unit: 'in', format: 'A4', orientation: 'portrait' }
    };

    $("#divContenedor,#divCotizacionPDF").css('display', '');
    $("body,html").css("overflow", "hidden");

    html2pdf().from(cotizacion).set(opt).save().then(function () {
        $("#divContenedor,#divCotizacionPDF").css('display', 'none');
        $("body,html").css("overflow", "");
    });
}

/* Exportar NEGOCIACIÓN a PDF */
function ExportarNegociacionAPDF(fileName) {

    const negociacion = this.document.getElementById("divContenedorNegociacion");
    var opt = {
        margin: 0.3,
        filename: fileName + '.pdf',
        image: { type: 'jpeg', quality: 1 },
        html2canvas: { scale: 2, logging: true, dpi: 300, letterRendering: true, useCORS: true },
        jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
    };

    $("#divContenedorNegociacion,#divNegociacionPDF").css('display', '');
    $("body,html").css("overflow", "hidden");

    html2pdf().from(negociacion).set(opt).save().then(function () {
        $("#divContenedorNegociacion,#divNegociacionPDF").css('display', 'none');
        $("body,html").css("overflow", "");
    });
}

function CargarMascarasDeEntrada() {

    $(".MascaraCantidad").inputmask("decimal", {
        alias: 'numeric',
        groupSeparator: ',',
        digits: 2,
        integerDigits: 11,
        digitsOptional: false,
        placeholder: '0',
        radixPoint: ".",
        autoGroup: true,
        min: 0.00
    });

    $(".MascaraNumerica").inputmask("decimal", {
        alias: "numeric",
        groupSeparator: ",",
        digits: 0,
        integerDigits: 3,
        digitsOptional: false,
        placeholder: "0",
        radixPoint: ".",
        autoGroup: true,
        min: 0.0,
    });

    $(".MascaraAnio").inputmask("decimal", {
        alias: "numeric",
        groupSeparator: ",",
        digits: 0,
        integerDigits: 4,
        digitsOptional: false,
        placeholder: "0",
        radixPoint: ".",
        autoGroup: true,
        min: 0.0,
    });
}

function PrimerPasoGuardarNegociacion() {

    /* Si la identidad no viene en la url, entonces solicitarla al usuario para luego verificar si ese cliente ya ha sido precalificado */
    if (identidadCliente == '' || identidadCliente == null) {

        /* si la identidad no venia en la URL, solicitarla al usuario y verificar si está precalificado */
        $("#txtBuscarIdentidad").val('').prop('disabled', false);
        $("#modalSolicitarIdentidad").modal();
    }
    else {
        /* si la identidad ya venia en la URL y está precalificado, ingresar informacion de la garantia */
        $("#txtBuscarIdentidad").prop('disabled', false);
        $("#modalGuardarNegociacion").modal();
    }
}

function NegociacionGuardadaCorrectamente() {

    /* Cuando la negociacion se guarde correctamente */
    $('#modalGuardarNegociacion').modal('hide');
    MensajeExito('Negociacion guardada correctamente');
    identidadCliente = '';

    ExportarNegociacionAPDF('Negociacion_' + getFormattedTime());
}

function InicializarCargaDeArchivos() {
    $('#fotografiaGarantia').fileuploader({
        inputNameBrackets: false,
        theme: 'dragdrop',
        limit: 1, // limite de archivos a subir
        maxSize: 10, // peso máximo de todos los archivos seleccionado en megas (MB)
        fileMaxSize: 2, // peso máximo de un archivo
        extensions: ['jpg', 'png'],// extensiones/formatos permitidos
        upload: {
            url: 'Negociaciones_Registrar.aspx?type=upload',
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

                /* validar exito */
                if (data.isSuccess && data.files[0]) {
                    item.name = data.files[0].name;
                    item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                }

                /* validar si se produjo un error */
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
            $.post('Negociaciones_Registrar.aspx?type=remove', { file: item.name });
        },
        dialogs: {

            /* Alertas */
            alert: function (text) {
                return iziToast.warning({
                    title: 'Atencion',
                    message: text
                });
            },

            /* Confirmaciones */
            confirm: function (text, callback) {
                confirm(text) ? callback() : null;
            }
        },
        captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], {
            feedback: 'Arrastra y suelta los archivos aqui',
            feedback2: 'Arrastra y suelta los archivos aqui',
            drop: 'Arrastra y suelta los archivos aqui',
            button: 'Buscar archivos',
            confirm: 'Confirmar',
            cancel: 'Cancelar'
        })
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

function getFormattedTime() {
    var today = new Date();
    var y = today.getFullYear();
    // JavaScript months are 0-based.
    var m = today.getMonth() + 1;
    var d = today.getDate();
    var h = today.getHours();
    var mi = today.getMinutes();
    var s = today.getSeconds();
    return y + "-" + m + "-" + d + "-" + h + "-" + mi + "-" + s;
}