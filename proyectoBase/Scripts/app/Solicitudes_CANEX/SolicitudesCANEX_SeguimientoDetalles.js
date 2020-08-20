$(document).ready(function () {

    /* Cargar documentos de la solicitud  */
    $.ajax({
        type: "POST",
        url: "Solicitudes_CANEX_Detalles.aspx/CargarDocumentos",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la documentacion, contacte al administrador');
        },
        success: function (data) {

            var rowDataDocumentos = data.d;
            var divDocumentacionCedula = $("#divDocumentacionCedula");
            var divDocumentacionCedulaModal = $("#divDocumentacionCedulaModal");
            var divDocumentacionDomicilio = $("#divDocumentacionDomicilio");
            var divDocumentacionDomicilioModal = $("#divDocumentacionDomicilioModal");
            var divDocumentacionLaboral = $("#divDocumentacionLaboral");
            var divDocumentacionLaboralModal = $("#divDocumentacionLaboralModal");
            var divDocumentacionFisicaModal = $("#divDocumentacionSoliFisicaModal");
            var contador = 0;

            for (var i = 0; i < rowDataDocumentos.length; i++) {

                var ruta = rowDataDocumentos[i].URLArchivo;

                if (rowDataDocumentos[i].fiTipoDocumento == 1 || rowDataDocumentos[i].fiTipoDocumento == 2 || rowDataDocumentos[i].fiTipoDocumento == 18 || rowDataDocumentos[i].fiTipoDocumento == 19) {
                    if (contador < 2) {
                        divDocumentacionCedula.append('<a class="float-left" href="' + ruta + '" title="Documentación identidad">' +
                            '<div class="img-responsive">' +
                            '<img class="img" src="' + ruta + '" alt="Documentación identidad" style="width: 100%; height: auto; float: left; cursor: zoom-in;" />' +
                            '</div>' +
                            '</a>');
                    }
                    divDocumentacionCedulaModal.append('<a class="float-left" href="' + ruta + '" title="Documentación identidad">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="Documentación identidad" width="100" />' +
                        '</div>' +
                        '</a>');
                    contador = contador + 1;
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 3) {
                    divDocumentacionDomicilio.append(
                        '<img class="img" src="' + ruta + '" title="Comprobante de domicilio" alt="Documentacion domicilio" style="width: 50%; height: auto; float:left;" />');
                    divDocumentacionDomicilioModal.append('<a class="float-left" href="' + ruta + '" title="Comprobante de domicilio">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="Comprobante de domicilio" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 5) {
                    divDocumentacionDomicilio.append(
                        '<img class="img" class="img" src="' + ruta + '" title="Croquis domicilio" alt="Documentacion domicilio" style="width: 50%; height: auto; float:left;" />');
                    divDocumentacionDomicilioModal.append('<a class="float-left" href="' + ruta + '" title="Croquis domicilio">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="Documentacion domicilio" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 4) {
                    divDocumentacionLaboral.append(
                        '<img class="img" src="' + ruta + '" title="Comprobante de ingresos" alt="Comprobante de ingresos" style="width: 50%; height: auto; float:left;"/>');
                    divDocumentacionLaboralModal.append('<a class="float-left" href="' + ruta + '" title="Comprobante de ingresos">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="Comprobante de ingresos" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 6) {
                    divDocumentacionLaboral.append(
                        '<img class="img" src="' + ruta + '" title="Croquis empleo" alt="Croquis empleo" style="width: 50%; height: auto; float:left;"/>');
                    divDocumentacionLaboralModal.append('<a class="float-left" href="' + ruta + '" title="Croquis empleo">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 7) {
                    divDocumentacionLaboral.append(
                        '<img class="img" src="' + ruta + '" title="Solicitud fisica" alt="Solicitud fisica" style="width: 50%; height: auto; float:left;"/>');
                    divDocumentacionFisicaModal.append('<a class="float-left" href="' + ruta + '" title="Solicitud fisica">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="Solicitud fisica" width="100" />' +
                        '</div>' +
                        '</a>');
                }
            }
            $(".img").imgbox({
                zoom: true,
                drag: true
            });
            $("#divCargandoAnalisis").css('display', 'none');
            $("#LogoPrestamo").css('display', '');
        }
    });
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}