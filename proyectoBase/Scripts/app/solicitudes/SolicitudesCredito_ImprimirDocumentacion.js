MostrarLoader();

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

$('.lblDepartamento_Firma').text('<%=DepartamentoFirma%>');
$('.lblCiudad_Firma').text('<%=CiudadFirma%>');
$('.lblNumeroDia_Firma').text('<%=DiasFirma%>');
$('.lblMes_Firma').text('<%=MesFirma%>');
$('.lblAnio_Firma').text('<%=AnioFirma%>');



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