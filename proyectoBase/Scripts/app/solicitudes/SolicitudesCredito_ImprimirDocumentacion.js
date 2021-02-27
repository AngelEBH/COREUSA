/***********************************************************************************************/
/************************************ Ejecutar funciones iniciales *****************************/
/***********************************************************************************************/
MostrarLoader();
CargarDocumentosParaAsegurarPendientes();



/***********************************************************************************************/
/************************************ Inicializar galerias  ************************************/
/***********************************************************************************************/
$("#divGaleriaGarantia").unitegallery({
    gallery_width: 900,
    gallery_height: 600
});

$("#divGaleriaInspeccionSeguroDeVehiculo").unitegallery({
    gallery_theme: "tilesgrid",
    tile_width: 300,
    tile_height: 194,
    grid_num_rows: 15
});

$("#divGaleriaPortadaExpediente").unitegallery({
    gallery_theme: "tilesgrid",
    tile_width: 310,
    tile_height: 190,
    grid_num_rows: 15
});

$("#divPortadaExpediente_Revision").unitegallery({
    gallery_theme: "tilesgrid",
    tile_width: 640,
    tile_height: 380,
    grid_num_rows: 15
});

$("#divContenedorInspeccionSeguro,#divContenedorPortadaExpediente").css('margin-top', '999px');
$("#divInspeccionSeguroPDF,#divContenedorInspeccionSeguro,#divPortadaExpedientePDF,#divContenedorPortadaExpediente").css('display', 'none');



/***********************************************************************************************/
/************************************ SETEAR CAMPOS GENERICOS **********************************/
/***********************************************************************************************/
$('.lblDepartamento_Firma').text(DEPARTAMENTO_FIRMA);
$('.lblCiudad_Firma').text(CIUDAD_FIRMA);
$('.lblNumeroDia_Firma').text(DIAS_FIRMA);
$('.lblMes_Firma').text(MES_FIRMA);
$('.lblAnio_Firma').text(ANIO_FIRMA);

$(".img-logo-empresa").attr('src', FONDOS_PRESTAMO.UrlLogo);
$('.lblRazonSocial').text(FONDOS_PRESTAMO.RazonSocial);
$('.lblNombreComercial').text(FONDOS_PRESTAMO.NombreComercial);
$('.lblRTNEmpresa').text(FONDOS_PRESTAMO.EmpresaRTN);
$('.lblCiudadDomicilioEmpresa').text(FONDOS_PRESTAMO.EmpresaCiudadDomiciliada);
$('.lblDepartamentoDomicilioEmpresa').text(FONDOS_PRESTAMO.EmpresaDepartamentoDomiciliada);
$('.lblTelefonoEmpresa').text(FONDOS_PRESTAMO.Telefono);
$('.lblEmailEmpresa').text(FONDOS_PRESTAMO.Email);
$('.lblConstitucionFondo').text(FONDOS_PRESTAMO.Constitucion);
$('.lblNombreRepresentanteLegal').text(FONDOS_PRESTAMO.RepresentanteLegal.NombreCompleto);
$('.lblIdentidadRepresentanteLegal').text(FONDOS_PRESTAMO.RepresentanteLegal.Identidad);
$('.lblEstadoCivilRepresentanteLegal').text(FONDOS_PRESTAMO.RepresentanteLegal.EstadoCivil);
$('.lblNacionalidadRepresentanteLegal').text(FONDOS_PRESTAMO.RepresentanteLegal.Nacionalidad);
$('.lblProfesionRepresentanteLegal').text(FONDOS_PRESTAMO.RepresentanteLegal.Prefesion);
$('.lblCiudadDomicilioRepresentanteLegal').text(FONDOS_PRESTAMO.RepresentanteLegal.CiudadDomicilio);
$('.lblDepartamentoDomicilioRepresentanteLegal').text(FONDOS_PRESTAMO.RepresentanteLegal.DepartamentoDomicilio);

OcultarLoader();



/***********************************************************************************************/
/************************************ Exportar a PDF *******************************************/
/***********************************************************************************************/
function ExportToPDF(nombreDelArchivo, idDivContenedor, idDivPDF) {

    $("#Loader").css('display', '');

    var opt = {
        margin: [0.3, 0.3, 0.3, 0.3], //top, left, buttom, right,
        filename: 'Solicitud_' + ID_SOLICITUD_CREDITO + '_' + nombreDelArchivo + '.pdf',
        image: { type: 'jpeg', quality: 1 },
        html2canvas: {
            dpi: 192,
            scale: 4,
            letterRendering: true,
            useCORS: false
        },
        jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' },
        pagebreak: { after: '.page-break', always: 'img' }
    };

    $("#" + idDivContenedor + ",#" + idDivPDF + "").css('display', '');
    $("body,html").css("overflow", "hidden");

    html2pdf().from(this.document.getElementById(idDivPDF)).set(opt).save().then(function () {
        $("#" + idDivContenedor + ",#" + idDivPDF + "").css('display', 'none');
        $("body,html").css("overflow", "");

        $("#Loader").css('display', 'none');
    });
}



/***********************************************************************************************/
/************************************ GENERACIÓN DE CÓDIGO QR **********************************/
/***********************************************************************************************/
$(document).ready(function () {

    InicializarCodigosQR();
});

function InicializarCodigosQR() {

    GenerarCodigoQR('qr_Expediente');
    GenerarCodigoQR('qr_Memorandum');
};

function GenerarCodigoQR(idElemento) {

    let qrcode = new QRCode(document.getElementById('' + idElemento + ''), {
        width: 85,
        height: 85
    });

    qrcode.makeCode(URL_CODIGO_QR);
}



/***********************************************************************************************/
/************************************ MANEJO DE EXPEDIENTES ************************************/
/***********************************************************************************************/
var permitirImprimirExpediente = ValidarEstadoDeDocumentosExpediente();

$("#btnExpediente").click(function () {

    if (ValidarEstadoDeDocumentosExpediente() && permitirImprimirExpediente == true) {
        ExportToPDF('Expediente', 'divContenedorExpediente', 'divExpedientePDF');
    }
    else {

        var ulDocumentosExpedientes = $("#ulDocumentosDelExpediente").empty();
        var template = '';
        var identificadorElemento = '';
        var stringData = '';

        for (var i = 0; i < LISTA_DOCUMENTOS_EXPEDIENTES.length; i++) {

            identificadorElemento = 'radio_' + LISTA_DOCUMENTOS_EXPEDIENTES[i].IdDocumento;
            stringData = ' data-iddocumento="' + LISTA_DOCUMENTOS_EXPEDIENTES[i].IdDocumento + '" ';

            template +=
                '<li>' +
                '<div class="form-group row border-bottom border-gray mb-2 mr-3">' +
                '<div class="col-sm-4 font-weight-bold pr-0">' +
                LISTA_DOCUMENTOS_EXPEDIENTES[i].DescripcionDocumento +
                '</div>' +
                '<div class="col-sm-8 pr-0">' +
                '<div class="form-check form-check-inline">' +
                '<input class="form-check-input" type="radio" name="' + identificadorElemento + '" id="radio_si_' + identificadorElemento + '" ' + stringData + ' value="1" ' + (LISTA_DOCUMENTOS_EXPEDIENTES[i].IdEstadoDocumento == 1 ? 'checked' : '') + ' onclick="ActualizarEstadoDocumentoExpediente(this)"  />' +
                '<label class="form-check-label" for="radio_si_' + identificadorElemento + '">SI</label>' +
                '</div>' +
                '<div class="form-check form-check-inline">' +
                '<input class="form-check-input" type="radio" name="' + identificadorElemento + '" id="radio_no_' + identificadorElemento + '" ' + stringData + ' value="2" ' + (LISTA_DOCUMENTOS_EXPEDIENTES[i].IdEstadoDocumento == 2 ? 'checked' : '') + ' onclick="ActualizarEstadoDocumentoExpediente(this)" />' +
                '<label class="form-check-label" for="radio_no_' + identificadorElemento + '">NO</label>' +
                '</div>' +
                '<div class="form-check form-check-inline">' +
                '<input class="form-check-input" type="radio" name="' + identificadorElemento + '" id="radio_na_' + identificadorElemento + '" ' + stringData + ' value="3" ' + (LISTA_DOCUMENTOS_EXPEDIENTES[i].IdEstadoDocumento == 3 ? 'checked' : '') + ' onclick="ActualizarEstadoDocumentoExpediente(this)" />' +
                '<label class="form-check-label" for="radio_na_' + identificadorElemento + '">N/A</label>' +
                '</div>' +
                '</div>' +
                '</div>' +
                '</li>';
        }

        ulDocumentosExpedientes.append(template);

        $("#modalGuardarExpedienteSolicitud").modal();
    }
});

$("#btnGuardarExpedienteSolicitud").click(function () {

    var modelStateIsValid = true;

    if (!ValidarEstadoDeDocumentosExpediente()) {
        MensajeError('La lista de verificación está incompleta.');
        modelStateIsValid = false;
    }

    if (modelStateIsValid) {

        var especifiqueOtrosDocumentos = $("#txtEspecifiqueOtras").val();

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_ImprimirDocumentacion.aspx/GuardarExpediente',
            data: JSON.stringify({ documentosExpediente: LISTA_DOCUMENTOS_EXPEDIENTES, especifiqueOtros: especifiqueOtrosDocumentos, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo guardar el expediente de la solicitud, contacte al administrador');
            },
            beforeSend: function () {
                MostrarLoader();
            },
            success: function (data) {

                if (data.d.ResultadoExitoso == true) {

                    var tblDocumentos_Expediente = $("#tblDocumentos_Expediente tbody").empty();
                    let template = '';

                    for (var i = 0; i < LISTA_DOCUMENTOS_EXPEDIENTES.length; i++) {

                        template += '<tr>' +
                            '<td>' + LISTA_DOCUMENTOS_EXPEDIENTES[i].DescripcionDocumento + '</td>' +
                            '<td class="text-center">' + (LISTA_DOCUMENTOS_EXPEDIENTES[i].IdEstadoDocumento == '1' ? 'X' : '') + '</td>' +
                            '<td class="text-center">' + (LISTA_DOCUMENTOS_EXPEDIENTES[i].IdEstadoDocumento == '2' ? 'X' : '') + '</td>' +
                            '<td class="text-center">' + (LISTA_DOCUMENTOS_EXPEDIENTES[i].IdEstadoDocumento == '3' ? 'X' : '') + '</td>' +
                            '</tr>';
                    }

                    tblDocumentos_Expediente.append(template);

                    $("#lblEspecifiqueOtros_Expediente").text(especifiqueOtrosDocumentos);

                    permitirImprimirExpediente = true;
                    $("#modalGuardarExpedienteSolicitud").modal('hide');
                    ExportToPDF('Expediente', 'divContenedorExpediente', 'divExpedientePDF');
                }
                else {
                    MensajeError(data.d.MensajeResultado);
                    console.log(data.d.MensajeDebug);
                }
            },
            complete: function () {
                OcultarLoader();
            }
        });
    }
});

function ActualizarEstadoDocumentoExpediente(elemento) {

    let idDocumento = $(elemento).data('iddocumento');
    let idEstado = $(elemento).val();

    for (var i = 0; i < LISTA_DOCUMENTOS_EXPEDIENTES.length; i++) {

        if (LISTA_DOCUMENTOS_EXPEDIENTES[i].IdDocumento == idDocumento) {
            LISTA_DOCUMENTOS_EXPEDIENTES[i].IdEstadoDocumento = idEstado;
            break;
        }
    }
}

function ValidarEstadoDeDocumentosExpediente() {

    for (var i = 0; i < LISTA_DOCUMENTOS_EXPEDIENTES.length; i++) {
        if (LISTA_DOCUMENTOS_EXPEDIENTES[i].IdEstadoDocumento == 0)
            return false;
    }
    return true;
}



/***********************************************************************************************/
/************************************ ENVIAR INFORMACION PARA ASEGURAR *************************/
/***********************************************************************************************/
function CargarDocumentosParaAsegurarPendientes() {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ImprimirDocumentacion.aspx/ObtenerDocumentosParaAsegurar",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Ocurrió un error al validar los documentos para asegurar la garantía, contacte al administrador');
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

            var formatoInputFile = '<div class="form-group mb-0">' +
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

            var divDocumentacion = $("#DivDocumentacionParaAsegurar");

            $.each(data.d, function (i, iter) {

                if (iter.IdEstadoDocumento == 0) {

                    $("#divDocumentosParaAsegurarPendientes").css('display', '');

                    var idInput = 'Documento' + iter.IdDocumento;

                    divDocumentacion.append(
                        '<form action="SolicitudesCredito_ImprimirDocumentacion.aspx?type=upload&IdDocumentoAsegurar=' + iter.IdDocumento + ' method="post" enctype="multipart/form-data">' +
                        '<label class="mb-1 mt-2">' + iter.Descripcion + '</label>' +
                        '<input type="file" class="filestyle" data-buttonname="btn-secondary" id="' + idInput + '" name="files" data-tipo="' + iter.IdDocumento + '"/>' +
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
                            url: 'SolicitudesCredito_ImprimirDocumentacion.aspx?type=upload&IdDocumentoAsegurar=' + iter.IdDocumento,
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
                            $.post('SolicitudesCredito_ImprimirDocumentacion.aspx?type=remove', { file: item.name });
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

                }

            }); /* Termina .Each*/

            $("#LoaderDocumentosParaAsegurar").css('display', 'none');
        }
    }); /* Termina Ajax */
}

$("#btnEnviarInformacionAseguradora_Confirmar").click(function () {

    debugger;

    $("#btnEnviarInformacionAseguradora_Confirmar").prop('disabled',true);

    let contenidoHtml = $('#divCorreoSeguroPDF').html();

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ImprimirDocumentacion.aspx/EnviarInformacionParaAsegurar",
        data: JSON.stringify({ contenidoHtml: contenidoHtml, VIN: $("#txtVIN").val(), dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo enviar la información para asegurar, contacte al administrador.');
        },
        success: function (data) {

            data.d.ResultadoExitoso == true ? MensajeExito(data.d.MensajeResultado) : MensajeError(data.d.MensajeResultado);

            console.log(data.d.MensajeDebug);

            $("#modalEnviarInformacionAseguradora").modal('hide');
        },
        complete: function () {
            $("#btnEnviarInformacionAseguradora_Confirmar").prop('disabled', false);
        }
    });

});

/***********************************************************************************************/
/************************************ FUNCIONES UTILITARIAS ************************************/
/***********************************************************************************************/
function EnviarCorreo(asunto, tituloGeneral, idContenidoHtml) {

    let contenidoHtml = $('#' + idContenidoHtml + '').html();

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ImprimirDocumentacion.aspx/EnviarDocumentoPorCorreo",
        data: JSON.stringify({ asunto: asunto, tituloGeneral: tituloGeneral, contenidoHtml: contenidoHtml, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo enviar el correo, contacte al administrador.');
        },
        success: function (data) {
            data.d == true ? MensajeExito('El correo se envió correctamente') : MensajeError('No se pudo enviar el correo, contacte al administrador.');
        }
    });
}

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function MensajeExito(mensaje) {
    iziToast.success({
        title: 'Éxito',
        message: mensaje
    });
}

function MostrarLoader() {
    $("#Loader").css('display', '');
}

function OcultarLoader() {
    $("#Loader").css('display', 'none');
}