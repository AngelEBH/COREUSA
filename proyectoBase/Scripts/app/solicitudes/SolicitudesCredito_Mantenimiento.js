var idSolicitud = 0;

$(document).ready(function () {

    $('#btnBuscarSolicitud').click(function () {

        BuscarSolicitud();

    }); // btnBuscarSolicitud click
});

$(document).on('click', 'button#btnAsignar', function () {

    $("#lblIdSolicitud").text(idSolicitud);
    $("#modalAsignarSolicitud").modal();
});

$("#btnResolucionCampo").click(function (e) {

    $("#modalResolucionCampo").modal();

});

$("#btnResolucionCampoConfirmar").click(function () {

    if ($("#ddlResolucionCampo :selected").val() != '') {

        let resolucionCampo = $("#ddlResolucionCampo :selected").val();
        let observaciones = $("#txtObservacioneResolucionCampo").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/ResolucionCampo",
            data: JSON.stringify({ idSolicitud: idSolicitud, resolucionCampo: resolucionCampo, observaciones: observaciones, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo guardar la resolución, contacte al administrador.");
            },
            success: function (data) {

                if (data.d != null) {

                    MensajeExito('La resolución de investigación de campo se guardó correctamente.');
                    $("#btnResolucionCampo").css('disabled', false);
                    BuscarSolicitud();
                }
                else {
                    MensajeError("No se pudo guardar la resolución, contacte al administrador.");
                }
            }
        });
    }
    else {
        $("#ddlResolucionCampo").focus();
    }
});


function BuscarSolicitud() {

    if ($('#txtNoSolicitud').val() != '') {

        $('#btnBuscarSolicitud').prop('disabled', true);

        idSolicitud = $('#txtNoSolicitud').val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Mantenimiento.aspx/CargarInformacion",
            data: JSON.stringify({ idSolicitud: idSolicitud, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {

                $("#btnBuscarSolicitud").prop('disabled', false);
                $("#divInformacionSolicitud").css('display', 'none');
                MensajeError("No se pudo realizar la búsqueda, contacte al administrador.");
            },
            success: function (data) {

                if (data.d != null) {

                    var resultado = data.d;

                    $('#lblIdSolicitud').text('No. ' + idSolicitud);
                    $('#lblIdSolicitud').css('display', '');
                    $("#txtNombreCliente").val(resultado.NombreCliente);
                    $("#txtIdentidadCliente").val(resultado.IdentidadCliente);
                    $("#txtRtn").val(resultado.RtnCliente);
                    $("#txtTelefono").val(resultado.Telefono);
                    $("#txtProducto").val(resultado.Producto);
                    $("#txtTipoDeSolicitud").val(resultado.TipoDeSolicitud);
                    $("#txtAgencia").val(resultado.Agencia);
                    $("#txtAgenteAsignado").val(resultado.UsuarioAsignado);
                    $("#txtGestorAsignado").val(resultado.GestorAsignado);

                    var tblHistorialMantenimiento = $("#tblHistorialMantenimiento tbody");
                    tblHistorialMantenimiento.empty();
                    tblHistorialMantenimiento.append('<tr><td class="text-center" colspan="4">No hay registros disponibles...</td></tr>');

                    /* Historial de mantenimiento */
                    if (resultado.HistorialMantenimientos != null) {
                        if (resultado.HistorialMantenimientos.length > 0) {

                            tblHistorialMantenimiento.empty();

                            var historialMantenimiento = resultado.HistorialMantenimientos;

                            var template = '';

                            $.each(historialMantenimiento, function (i, iter) {

                                template += '<tr><td>' + moment(iter.FechaMantenimiento).locale('es').format('YYYY/MM/DD hh:mm:ss a') + '</td><td>' + iter.AgenciaUsuario + '</td><td>' + iter.NombreUsuario + '</td><td>' + iter.Observaciones + '</td></tr>';
                            });

                            tblHistorialMantenimiento.append(template);
                        }
                    }

                    $("#divInformacionSolicitud").css('display', '');

                } // if data.d != null
                else {
                    $("#divInformacionSolicitud").css('display', 'none');
                    MensajeError('No se pudo cargar la información, contacte al administrador.');
                }
                $('#btnBuscarSolicitud').prop('disabled', false);

            }// success
        }); // ajax
    } // if txtNoSolicitud != ''
    else {
        $("#txtNoSolicitud").focus();
    }
}

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function MensajeExito(mensaje) {
    iziToast.success({
        title: 'Exito',
        message: mensaje
    });
}