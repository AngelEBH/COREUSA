﻿$(document).ready(function () {

    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: "Solicitudes_CANEX_Detalles.aspx/CargarDocumentos" + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo carga la documentacion, contacte al administrador');//mostrar mensaje de error
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
                            '<img class="img" src="' + ruta + '" style="width: 100%; height: auto; float: left; cursor: zoom-in;" />' +
                            '</div>' +
                            '</a>');
                    }
                    divDocumentacionCedulaModal.append('<a class="float-left" href="' + ruta + '" title="Documentación identidad">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                    contador = contador + 1;
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 3) {
                    divDocumentacionDomicilio.append(
                        '<img class="img" src="' + ruta + '" title="Comprobante de domicilio" alt="Documentacion domicilio" style="width: 50%; height: auto; float:left;" />');
                    divDocumentacionDomicilioModal.append('<a class="float-left" href="' + ruta + '" title="Comprobante de domicilio">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 5) {
                    divDocumentacionDomicilio.append(
                        '<img class="img" class="img" src="' + ruta + '" title="Croquis domicilio" alt="Documentacion domicilio" style="width: 50%; height: auto; float:left;" />');
                    divDocumentacionDomicilioModal.append('<a class="float-left" href="' + ruta + '" title="Croquis domicilio" alt="Documentacion domicilio">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 4) {
                    divDocumentacionLaboral.append(
                        '<img class="img" src="' + ruta + '" title="Comprobante de ingresos" alt="Comprobante de ingresos" style="width: 50%; height: auto; float:left;"/>');
                    divDocumentacionLaboralModal.append('<a class="float-left" href="' + ruta + '" title="Comprobante de ingresos" alt="Comprobante de ingresos">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 6) {
                    divDocumentacionLaboral.append(
                        '<img class="img" src="' + ruta + '" title="Croquis empleo" alt="Croquis empleo" style="width: 50%; height: auto; float:left;"/>');
                    divDocumentacionLaboralModal.append('<a class="float-left" href="' + ruta + '" title="Croquis empleo" alt="Croquis empleo">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 7) {
                    divDocumentacionLaboral.append(
                        '<img class="img" src="' + ruta + '" title="Solicitud fisica" alt="Solicitud fisica" style="width: 50%; height: auto; float:left;"/>');
                    divDocumentacionFisicaModal.append('<a class="float-left" href="' + ruta + '" title="Solicitud fisica" alt="Solicitud fisica">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="" width="100" />' +
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
    var EstadoRechazada = 5;
    var EstadoAprobada = 4;
    var EstadoEnRevision = 3;

    if (IDEstado == EstadoAprobada || EstadoAprobada == EstadoRechazada) {

        var Desicion = IDEstado == EstadoAprobada ? 'Aprobada' : 'Rechazada';
        $("#btnAceptarSolicitud,#btnRechazar,#btnCondicionarSolicitud").prop('disabled', true);
        $("#btnAceptarSolicitud,#btnRechazar,#btnCondicionarSolicitud").prop('title','La solicitud ya fue '+ Desicion);
    }
    else if (IDSolicitudImportada != 0) {
        $("#btnAceptarSolicitud,#btnRechazar,#btnCondicionarSolicitud").prop('disabled', true);
        $("#btnAceptarSolicitud,#btnRechazar,#btnCondicionarSolicitud").prop('title', 'La solicitud ya fue importada');
    }
    else if (IDEstado == EstadoEnRevision && IDSolicitudImportada == 0) {
        $("#btnAceptarSolicitud").prop('disabled', false);
    }
    else if (IDEstado != EstadoRechazada && IDEstado != EstadoAprobada && IDSolicitudImportada == 0) {
        $("#btnRechazar").prop('disabled', true);
    }
});

$("#btnHistorialExterno").click(function () {
    MensajeInformacion('Cargando buro externo');
    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: 'Solicitudes_CANEX_Detalles.aspx/ObtenerUrlEncriptado' + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar buro externo');
        },
        success: function (data) {
            var parametros = data.d;
            window.open("http://portal.prestadito.corp/corefinanciero/Clientes/Precalificado_Analista.aspx?" + parametros, "_blank",
                "toolbar=yes, scrollbars=yes,resizable=yes," +
                "top=0,left=window.screen.availWidth/2," +
                "window.screen.availWidth/2,window.screen.availHeight");
        }
    });
});

var contadorCondiciones = 0;
var listaCondicionamientos = [];
$("#btnAgregarCondicion").click(function () {

    if ($($("#ddlCondiciones")).parsley().isValid() && $($("#txtComentarioAdicional")).parsley().isValid()) {
        var condicionID = $("#ddlCondiciones :selected").val();
        var descripcionCondicion = $("#ddlCondiciones :selected").text();
        var comentarioAdicional = $("#txtComentarioAdicional").val();
        var btnQuitarCondicion = '<button type="button" data-id=' + condicionID + ' data-comentario="' + comentarioAdicional + '" id="btnQuitarCondicion" class="btn btn-sm btn-danger">Quitar</button>';
        var newRowContent = '<tr><td>' + descripcionCondicion + '</td><td>' + comentarioAdicional + '</td><td>' + btnQuitarCondicion + '</td></tr>';
        $("#tblCondiciones tbody").append(newRowContent);
        $("#txtComentarioAdicional").val('');

        listaCondicionamientos.push({
            fiIDCondicion: condicionID,
            fcComentarioAdicional: comentarioAdicional
        });
        contadorCondiciones = contadorCondiciones + 1;

    } else { $($("#ddlCondiciones")).parsley().validate(); $($("#txtComentarioAdicional")).parsley().validate(); }
});

$(document).on('click', 'button#btnQuitarCondicion', function () {

    $(this).closest('tr').remove();
    var condicion = {
        fiIDCondicion: $(this).data('id').toString(),
        fcComentarioAdicional: $(this).data('comentario')
    };
    var list = [];
    if (listaCondicionamientos.length > 0) {

        for (var i = 0; i < listaCondicionamientos.length; i++) {

            var iter = {
                fiIDCondicion: listaCondicionamientos[i].fiIDCondicion,
                fcComentarioAdicional: listaCondicionamientos[i].fcComentarioAdicional
            };
            if (JSON.stringify(iter) != JSON.stringify(condicion)) {
                list.push(iter);
            }
        }
    }
    listaCondicionamientos = list;
    contadorCondiciones -= 1;
});

$("#btnCondicionarSolicitudConfirmar").click(function () {
    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: "Solicitudes_CANEX_Detalles.aspx/CondicionarSolicitud" + qString,
        data: JSON.stringify({ SolicitudCondiciones: listaCondicionamientos, IDPais: IDPais, IDSocio: IDSocio, IDAgencia: IDAgencia }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {
            if (data.d == true) {
                $("#modalCondicionarSolicitud").modal('hide');
                MensajeExito('Estado de la solicitud actualizado');
                $("#tblCondiciones tbody").empty();
                listaCondicionamientos = [];
                contadorCondiciones = 0;
            }
            else { MensajeError('Error al condicionar la solicitud, contacte al administrador'); }
        }
    });
});

$("#btnRechazarConfirmar").click(function () {

    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: "Solicitudes_CANEX_Detalles.aspx/SolicitudResolucion" + qString,
        data: JSON.stringify({ IDEstado: 0, IDPais: IDPais, IDSocio: IDSocio, IDAgencia: IDAgencia }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al actualizar estado de la solicitud, contacte al administrador');
        },
        success: function (data) {
            if (data.d != 0) {
                $("#btnAceptarSolicitud, #btnRechazar, #btnCondicionarSolicitud").prop('disabled', true);
                $("#btnAceptarSolicitud, #btnRechazar, #btnCondicionarSolicitud").prop('title', 'La solicitud ya fue rechazada');
                $("#modalResolucionRechazar").modal('hide');
                MensajeExito('¡Solicitud rechazada correctamente!');
            }
            else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
        }
    });
});

$("#btnAceptarSolicitudConfirmar").click(function () {

    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: "Solicitudes_CANEX_Detalles.aspx/ImportarSolicitud" + qString,
        data: JSON.stringify({ IDPais: IDPais, IDSocio: IDSocio, IDAgencia: IDAgencia }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al actualizar estado de la solicitud, contacte al administrador');
        },
        success: function (data) {
            if (data.d != 0) {
                $("#btnAceptarSolicitud, #btnRechazar, #btnCondicionarSolicitud").prop('disabled', true);
                $("#btnAceptarSolicitud, #btnRechazar, #btnCondicionarSolicitud").prop('title', 'La solicitud ya fue rechazada');
                $("#modalResolucionAprobar").modal('hide');
                MensajeExito('¡Solicitud exportada a bandeja de crédito correctamente!');
            }
            else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
        }
    });
});

function MensajeInformacion(mensaje) {
    iziToast.info({
        title: 'Info',
        message: mensaje
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