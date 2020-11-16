$("#btnHistorialExterno").click(function () {

    MensajeInformacion('Cargando buro externo');

    $.ajax({
        type: "POST",
        url: 'Solicitudes_CANEX_Detalles.aspx/ObtenerUrlEncriptado',
        data: JSON.stringify({ dataCrypt: window.location.href }),
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
        var btnQuitarCondicion = '<button type="button" data-id=' + condicionID + ' data-comentario="' + comentarioAdicional + '" id="btnQuitarCondicion" data-condicion="' + descripcionCondicion + '" class="btn btn-sm btn-danger">Quitar</button>';
        var newRowContent = '<tr><td>' + descripcionCondicion + '</td><td>' + comentarioAdicional + '</td><td>' + btnQuitarCondicion + '</td></tr>';

        $("#tblCondiciones tbody").append(newRowContent);
        $("#txtComentarioAdicional").val('');

        listaCondicionamientos.push({
            fiIDCondicion: condicionID,
            fcComentarioAdicional: comentarioAdicional,
            fcCondicion: descripcionCondicion
        });
        contadorCondiciones = contadorCondiciones + 1;

    } else { $($("#ddlCondiciones")).parsley().validate(); $($("#txtComentarioAdicional")).parsley().validate(); }
});

$(document).on('click', 'button#btnQuitarCondicion', function () {

    $(this).closest('tr').remove();

    var condicion = {
        fiIDCondicion: $(this).data('id').toString(),
        fcComentarioAdicional: $(this).data('comentario'),
        fcCondicion: $(this).data('condicion')
    };

    var list = [];
    if (listaCondicionamientos.length > 0) {

        for (var i = 0; i < listaCondicionamientos.length; i++) {

            var iter = {
                fiIDCondicion: listaCondicionamientos[i].fiIDCondicion,
                fcComentarioAdicional: listaCondicionamientos[i].fcComentarioAdicional,
                fcCondicion: listaCondicionamientos[i].fcCondicion
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

    $.ajax({
        type: "POST",
        url: "Solicitudes_CANEX_Detalles.aspx/CondicionarSolicitud",
        data: JSON.stringify({ SolicitudCondiciones: listaCondicionamientos, idPais: idPais, idSocio: idSocio, idAgencia: idAgencia, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            if (data.d == true) {

                $("#modalCondicionarSolicitud").modal('hide');
                MensajeExito('Estado de la solicitud actualizado');
                $("#tblCondiciones tbody").empty();

                var tblListaSolicitudCondiciones = $("#tblListaSolicitudCondiciones tbody");

                /* Actualizar Tabla de Listado de Condicionamientos */
                var condicion = '';
                for (var i = 0; i < listaCondicionamientos.length; i++) {

                    condicion = listaCondicionamientos[i];

                    tblListaSolicitudCondiciones.append('<tr><td><label class="btn btn-sm btn-block btn-info mb-0">Nuevo</label></td><td>' + condicion.fcCondicion + '</td><td>' + condicion.fcComentarioAdicional + '</td><td><label class="btn btn-sm btn-block btn-danger mb-0">Pendiente</label></td></tr>');
                }

                listaCondicionamientos = [];
                contadorCondiciones = 0;
                $("#pestanaListaSolicitudCondiciones").css('display', '');
            }
            else { MensajeError('Error al condicionar la solicitud, contacte al administrador'); }
        }
    });
});

$("#btnRechazarConfirmar").click(function () {

    if ($($("#txtComentarioRechazar")).parsley().isValid()) {

        $.ajax({
            type: "POST",
            url: "Solicitudes_CANEX_Detalles.aspx/RechazarSolicitud",
            data: JSON.stringify({ idPais: idPais, idSocio: idSocio, idAgencia: idAgencia, comentario: $("#txtComentarioRechazar").val().trim(), dataCrypt: window.location.href }),
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
    }
    else {
        $($("#txtComentarioRechazar")).parsley().validate();
    }
});

$("#btnAceptarSolicitudConfirmar").click(function () {

    if ($($("#txtComentarioAceptar")).parsley().isValid()) {
        $.ajax({
            type: "POST",
            url: "Solicitudes_CANEX_Detalles.aspx/ImportarSolicitud",
            data: JSON.stringify({ idPais: idPais, idSocio: idSocio, idAgencia: idAgencia, comentario: $("#txtComentarioAceptar").val().trim(), dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al actualizar estado de la solicitud, contacte al administrador');
            },
            success: function (data) {

                console.log(data.d);

                if (data.d.response === true) {

                    $("#btnAceptarSolicitud, #btnRechazar, #btnCondicionarSolicitud").prop('disabled', true);
                    $("#btnAceptarSolicitud, #btnRechazar, #btnCondicionarSolicitud").prop('title', 'La solicitud ya fue rechazada');

                    $("#modalResolucionAprobar").modal('hide');

                    MensajeExito('¡Solicitud exportada a bandeja de crédito correctamente!');
                }
                else { MensajeError('Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    }
    else {
        $($("#txtComentarioAceptar")).parsley().validate();
    }
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