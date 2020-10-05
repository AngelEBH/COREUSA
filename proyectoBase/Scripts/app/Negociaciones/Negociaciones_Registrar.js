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