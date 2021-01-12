
var btnFinalizar = $('<button type="button" id="btnGuardarRevision"></button>').text('Finalizar').addClass('btn btn-info').css('display', 'none')
    .on('click', function () {

        var modelState = $('#frmPrincipal').parsley().isValid();

        if (modelState) {

            //var garantia = {

            //    VIN: $("#txtVIN").val(),
            //    TipoDeGarantia: $("#ddlTipoDeGarantia").val(),
            //    TipoDeVehiculo: $("#txtTipoDeVehiculo").val(),
            //    Marca: $("#txtMarca").val(),
            //    Modelo: $("#txtModelo").val(),
            //    Anio: $("#txtAnio").val().replace(/,/g, ''),
            //    Color: $("#txtColor").val(),
            //    Matricula: $("#txtMatricula").val(),
            //    Cilindraje: $("#txtCilindraje").val(),
            //    Recorrido: $("#txtRecorrido").val().replace(/,/g, ''),
            //    UnidadDeDistancia: $("#ddlUnidadDeMedida").val(),
            //    Transmision: $("#txtTransmision").val(),
            //    TipoDeCombustible: $("#txtTipoDeCombustible").val(),
            //    SerieUno: $("#txtSerieUno").val(),
            //    SerieDos: $("#txtSerieDos").val(),
            //    SerieMotor: $("#txtSerieMotor").val(),
            //    SerieChasis: $("#txtSerieChasis").val(),
            //    GPS: $("#txtGPS").val(),
            //    Comentario: $("#txtComentario").val(),
            //    NumeroPrestamo: $("#txtNumeroPrestamo").val(),
            //    esDigitadoManualmente: esDigitadoManualmente,

            //    ValorMercado: $("#txtPrecioMercado").val().replace(/,/g, '') == '' ? 0 : $("#txtPrecioMercado").val().replace(/,/g, ''),
            //    ValorPrima: $("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''),
            //    ValorFinanciado: $("#txtValorFinanciado").val().replace(/,/g, '') == '' ? 0 : $("#txtValorFinanciado").val().replace(/,/g, ''),
            //    GastosDeCierre: $("#txtGastosDeCierre").val().replace(/,/g, '') == '' ? 0 : $("#txtGastosDeCierre").val().replace(/,/g, ''),
            //    IdentidadPropietario: $("#txtIdentidadPropietario").val(),
            //    NombrePropietario: $("#txtNombrePropietario").val(),
            //    IdNacionalidadPropietario: $("#ddlNacionalidadPropietario :selected").val() == '' ? 0 : $("#ddlNacionalidadPropietario :selected").val(),
            //    IdEstadoCivilPropietario: $("#ddlEstadoCivilPropietario :selected").val() == '' ? 0 : $("#ddlEstadoCivilPropietario :selected").val(),
            //    IdentidadVendedor: $("#txtIdentidadVendedor").val(),
            //    NombreVendedor: $("#txtNombreVendedor").val(),
            //    IdNacionalidadVendedor: $("#ddlNacionalidadVendedor :selected").val() == '' ? 0 : $("#ddlNacionalidadVendedor :selected").val(),
            //    IdEstadoCivilVendedor: $("#ddlEstadoCivilVendedor :selected").val() == '' ? 0 : $("#ddlEstadoCivilVendedor :selected").val()
            //}

            //$.ajax({
            //    type: "POST",
            //    url: 'Garantia_Actualizar.aspx/ActualizarGarantia',
            //    data: JSON.stringify({ garantia: garantia, dataCrypt: window.location.href }),
            //    contentType: 'application/json; charset=utf-8',
            //    error: function (xhr, ajaxOptions, thrownError) {
            //        MensajeError('No se guardó el registro, contacte al administrador');
            //    },
            //    success: function (data) {

            //        let resultado = data.d;

            //        if (resultado.ResultadoExitoso == true) {

            //            window.location = "SolicitudesCredito_ListadoGarantias.aspx?" + window.location.href.split('?')[1];
            //        }
            //        else {
            //            MensajeError(resultado.MensajeResultado);
            //            console.log(resultado.DebugString);
            //        }
            //    }
            //});
        }
        else {
            $('#frmPrincipal').parsley().validate();
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
        //toolbarExtraButtons: [btnFinalizar]
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