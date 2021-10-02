
var btnFinalizar = $('<button type="button" id="btnGuardarGarantia"></button>').text('Finalizar').addClass('btn btn-info').css('display', 'none')
    .on('click', function () {

        var modelState = $('#frmGarantia').parsley().isValid();

        if (modelState || user == '11') {

            var garantia = {

                VIN: $("#txtVIN").val(),
                TipoDeGarantia: $("#ddlTipoDeGarantia").val(),
                TipoDeVehiculo: $("#txtTipoDeVehiculo").val(),
                Marca: $("#txtMarca").val(),
                Modelo: $("#txtModelo").val(),
                Anio: $("#txtAnio").val().replace(/,/g, ''),
                Color: $("#txtColor").val(),
                Matricula: $("#txtMatricula").val(),
                Cilindraje: $("#txtCilindraje").val(),
                Recorrido: $("#txtRecorrido").val().replace(/,/g, ''),
                UnidadDeDistancia: $("#ddlUnidadDeMedida").val(),
                Transmision: $("#txtTransmision").val(),
                TipoDeCombustible: $("#txtTipoDeCombustible").val(),
                SerieUno: $("#txtSerieUno").val(),
                SerieDos: $("#txtSerieDos").val(),
                SerieMotor: $("#txtSerieMotor").val(),
                SerieChasis: $("#txtSerieChasis").val(),
                GPS: $("#txtGPS").val(),
                Comentario: $("#txtComentario").val(),
                NumeroPrestamo: $("#txtNumeroPrestamo").val(),
                esDigitadoManualmente: esDigitadoManualmente,

                ValorMercado: $("#txtPrecioMercado").val().replace(/,/g, '') == '' ? 0 : $("#txtPrecioMercado").val().replace(/,/g, ''),
                ValorPrima: $("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''),
                ValorFinanciado: $("#txtValorFinanciado").val().replace(/,/g, '') == '' ? 0 : $("#txtValorFinanciado").val().replace(/,/g, ''),
                GastosDeCierre: $("#txtGastosDeCierre").val().replace(/,/g, '') == '' ? 0 : $("#txtGastosDeCierre").val().replace(/,/g, ''),
                IdentidadPropietario: $("#txtIdentidadPropietario").val(),
                NombrePropietario: $("#txtNombrePropietario").val(),
                IdNacionalidadPropietario: $("#ddlNacionalidadPropietario :selected").val() == '' ? 0 : $("#ddlNacionalidadPropietario :selected").val(),
                IdEstadoCivilPropietario: $("#ddlEstadoCivilPropietario :selected").val() == '' ? 0 : $("#ddlEstadoCivilPropietario :selected").val(),
                IdentidadVendedor: $("#txtIdentidadVendedor").val(),
                NombreVendedor: $("#txtNombreVendedor").val(),
                IdNacionalidadVendedor: $("#ddlNacionalidadVendedor :selected").val() == '' ? 0 : $("#ddlNacionalidadVendedor :selected").val(),
                IdEstadoCivilVendedor: $("#ddlEstadoCivilVendedor :selected").val() == '' ? 0 : $("#ddlEstadoCivilVendedor :selected").val(),


                IdTipoSolicitudPago: $("#ddlFormaDePagoDesembolso :selected").val() == '' ? 0 : $("#ddlFormaDePagoDesembolso :selected").val(),
                RTNVendedorGarantia: $("#txtRTNVendedor").val(),
                IdBancoDesembolso: $("#ddlBancoDesembolso :selected").val() == '' ? 0 : $("#ddlBancoDesembolso :selected").val(),
                IdTipoBancoDesembolso: $("#ddlTipoCuentaBancaria :selected").val() == '' ? 0 : $("#ddlTipoCuentaBancaria :selected").val(),
                CuentaBancoDesembolso: $("#txtCuentaBancariaDeposito").val(),

            }

            $.ajax({
                type: "POST",
                url: 'Garantia_Actualizar.aspx/ActualizarGarantia',
                data: JSON.stringify({ garantia: garantia, dataCrypt: window.location.href }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError('No se guardó el registro, contacte al administrador');
                },
                success: function (data) {

                    let resultado = data.d;

                    if (resultado.ResultadoExitoso == true)
                    {
                        window.location = "SolicitudesCredito_ListadoGarantias.aspx?" + window.location.href.split('?')[1];
                    }
                    else
                    {
                        MensajeError(resultado.MensajeResultado);
                        console.log(resultado.DebugString);
                    }
                }
            });
        }
        else {
            $('#frmGarantia').parsley().validate();
        }
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
            $("#btnGuardarGarantia").css('display', '');
        }
        else { /* Si no es ninguna de las anteriores, habilitar todos los botones */
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
            $("#btnGuardarGarantia").css('display', 'none');
        }
    });

    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');
    });

    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

        if (stepDirection == 'forward') {

            if (stepNumber == 0) {

                var state = ($('#frmGarantia').parsley().isValid() || user == '11') ? true : false;

                if (state == false) {

                    $('#frmGarantia').parsley().validate();
                }
                return state;
            }
        }
    });

    CargarDocumentosRequeridos();
});

$("#btnBuscarVIN").on('click', function () {

    BuscarVIN();
});

/* De momento no se utiliza debido a cambios solicitados */
$("#cbDigitarManualmente").on('change', function () {

    let digitarManualmente = $("#cbDigitarManualmente").prop("checked");

    $("#btnBuscarVIN,#txtBuscarVIN").prop("disabled", digitarManualmente).prop("title", digitarManualmente == true ? 'La búsqueda está desactivada' : '');

    $("#txtVIN,#txtTipoDeVehiculo,#txtMarca,#txtModelo,#txtAnio,#txtCilindraje,#txtTransmision,#txtTipoDeCombustible,#txtSerieUno").prop("readonly", !digitarManualmente);

    if (digitarManualmente == false) {

        let txtVin = $("#txtVIN").val();

        if (txtVin != '') {
            $("#txtBuscarVIN").val(txtVin);
        }
    }

    esDigitadoManualmente = digitarManualmente;
});

function BuscarVIN() {

    let idVIN = $("#txtBuscarVIN").val().trim();

    if (idVIN) {

        MostrarLoader();

        $.ajax({
            url: 'https://vpic.nhtsa.dot.gov/api/vehicles/DecodeVinValues/' + idVIN,
            type: 'Get',
            data: { format: "json" },
            success: function (data) {

                if (data.Results[0].ErrorCode != "0")
                {
                    MensajeError('Resultado de la búsqueda: ' + data.Results[0].ErrorText);
                }

                $("#txtVIN").val(data.Results[0].VIN);
                $("#txtMarca").val(data.Results[0].Make);
                $("#txtModelo").val(data.Results[0].Model);
                $("#txtTipoDeVehiculo").val(data.Results[0].VehicleType);
                $("#txtAnio").val(data.Results[0].ModelYear);
                $("#txtTipoDeVehiculo").val(data.Results[0].BodyClass);
                $("#txtCilindraje").val(data.Results[0].DisplacementL);
                $("#txtTipoDeCombustible").val(data.Results[0].FuelTypePrimary);
                $("#txtSerieUno").val(data.Results[0].Series);
                $("#txtTransmision").val(data.Results[0].TransmissionStyle);

                OcultarLoader();
            },
            error: function (data) {
                console.log('ocurrió un error: ' + data);

                OcultarLoader();
                MensajeError('No se pudo cargar la información de este VIN');
            }
        });
    }
}

/* Cargar y generar los inputs de los documentos de la garantía */
function CargarDocumentosRequeridos() {

    $.ajax({
        type: "POST",
        url: "Garantia_Actualizar.aspx/CargarDocumentosRequeridos",
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

            var Input = '<div class="bootstrap-filestyle input-group">' +
                '<span class="group-span-filestyle w-100" tabindex="0">' +
                '<label for="filestyle-1" class="btn btn-secondary btn-block">' +
                '<span class="icon-span-filestyle fas fa-folder-open">' +
                '</span>' +
                '<span class="buttonText">Subir archivos</span>' +
                '</label>' +
                '</span>' +
                '</div > ';

            var divDocumentacion = $("#DivDocumentacion");

            $.each(data.d, function (i, iter) {

                var IdInput = 'Documento' + iter.IdSeccionGarantia;

                divDocumentacion.append("<div class='col-sm-2 mt-3 pr-1 pl-1'>" +
                    "<label class='form-label mb-1'>" + iter.DescripcionSeccion + "</label>" +
                    "<form action='Garantia_Actualizar.aspx?type=upload&doc=" + iter.IdSeccionGarantia + "' method='post' enctype='multipart/form-data'>" +
                    "<input id='" + IdInput + "' type='file' name='files' data-tipo='" + iter.IdSeccionGarantia + "' />" +
                    "</form>" +
                    "</div");

                $('#' + IdInput + '').fileuploader({
                    inputNameBrackets: false,
                    changeInput: Input,
                    theme: 'dragdrop',
                    limit: 1, // Limite de archivos a subir
                    maxSize: 200, // Peso máximo de todos los archivos seleccionado en megas (MB)
                    fileMaxSize: 20, // Peso máximo de un archivo
                    extensions: ['jpg', 'png', 'jpeg'],// Extensiones/formatos permitidos
                    upload: {
                        url: 'Garantia_Actualizar.aspx?type=upload&doc=' + iter.IdSeccionGarantia,
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
                        $.post('Garantia_Actualizar.aspx?type=remove', { file: item.name });
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

function resetForm($form) {
    $form.find('input:text, input:password, input:file,input[type="date"],input[type="email"], select, textarea').val('');
    $form.find('input:radio, input:checkbox').removeAttr('checked').removeAttr('selected');
}