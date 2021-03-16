const language = {
    "sProcessing": "Cargando información...",
    "sLengthMenu": "Mostrar _MENU_ registros",
    "sZeroRecords": "No se encontraron resultados",
    "sEmptyTable": "Ningún dato disponible en esta tabla",
    "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
    "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
    "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
    "sInfoPostFix": "",
    "sSearch": "Buscar:",
    "sUrl": "",
    "sInfoThousands": ",",
    "sLoadingRecords": "Cargando información...",
    "oPaginate": {
        "sFirst": "Primero",
        "sLast": "Último",
        "sNext": "Siguiente",
        "sPrevious": "Anterior"
    },
    "oAria": {
        "sSortAscending": ": Activar para ordenar la columna de manera ascendente",
        "sSortDescending": ": Activar para ordenar la columna de manera descendente"
    },
    "decimal": ".",
    "thousands": ","
};

var idTipoDeDocumento = 0;
var nombreTipoDeDocumento = 'Ningún documento seleccionado';
var descripcionTipoDeDocumento = '';
var EstadoNoAdjuntado = false;
var EstadoNoAplica = false;
var documentoObligatorio = false;
var cantidadMinimaDocumentos = 0;
var cantidadMaximaDocumentos = 0;
var cantidadDocumentosGuardados = 0;


$(function () {

    /*
    Buenas tardes, Amilcar, qué tal?
    Le escribía para comentarle algo que considero es de mucha importancia y realmente hubiese preferido hablarlo en persona pero en realidad no contaba con que iría de viaje. Es acerca de mi estancia en la empresa.
    En busca de crecimiento y desarrollo personal y profesional, además de otros factores de fuerza mayor, eh tomado la desición de finalizar mi relación con la empresa
    Por supuesto antes de hacerlo formal quería comentárselo por si tenía algún comentario al respecto
    */

    CargarDocumentosDelExpediente();
    CargarDocumentosGuardadosPorTipoDeDocumento(idTipoDeDocumento, nombreTipoDeDocumento, descripcionTipoDeDocumento, undefined, undefined, cantidadMinimaDocumentos, cantidadMaximaDocumentos, cantidadDocumentosGuardados, documentoObligatorio);
});

/* Cargar el listado de documentos que corresponden al expediente (esto depende del tipo de producto). Con el indicador de la cantidad de documentos que se han guardado */
function CargarDocumentosDelExpediente() {

    $.ajax({
        type: "POST",
        url: "Expedientes_Consultar.aspx/CargarDocumentosDelExpediente",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los documentos del expediente, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                $('#tblTiposDeDocumentos').DataTable({
                    "destroy": true,
                    "pageLength": 100,
                    "aaSorting": [],
                    "language": language,
                    dom: 'f',
                    data: data.d,
                    "columns": [
                        {
                            "data": "DescripcionNombreDocumento", "className": "font-12",
                            "render": function (data, type, row) {

                                let documentoObligatorio = ((row["Obligatorio"] == true && row["CantidadGuardados"] == 0) || (row["Obligatorio"] == true && row["CantidadGuardados"] < row["CantidadMinima"]) || (row["Obligatorio"] == true && row["NoAdjuntado"] == true));
                                let className = documentoObligatorio == true ? 'danger' : 'secondary';

                                return '<span title="' + row["DescripcionDetalladaDelDocumento"] + '">' +
                                    row["DescripcionNombreDocumento"] +
                                    '<small class="badge badge-' + className + ' ml-1" title="' + (documentoObligatorio == true ? 'Este documento es obligatorio. Documentos guardados: ' + row["CantidadGuardados"] + '. Cantidad minima a guardar: ' + row["CantidadMinima"] : '') + '">' +
                                    row["CantidadGuardados"] +
                                    '</small>' +
                                    (row["NoAplica"] == true || row["NoAdjuntado"] == true ? '<small class="badge badge-' + (documentoObligatorio == true ? 'danger' : 'warning') + ' float-right">' + (row["NoAplica"] == true ? "N/A" : row["NoAdjuntado"] == true ? "NO" : '') + '</small>' : '') + '</span>'
                            }
                        },
                        {
                            "data": "IdDocumento", "className": "text-center",
                            "render": function (data, type, row) {

                                return '<button class="btn btn-sm btn-secondary" onclick="CargarDocumentosGuardadosPorTipoDeDocumento(' + "'" + row["IdDocumento"] + "'" + ',' + "'" + row["DescripcionNombreDocumento"] + "'" + ',' + "'" + row["DescripcionDetalladaDelDocumento"] + "'" + ',' + row["NoAdjuntado"] + ',' + row["NoAplica"] + ',' + row["CantidadMinima"] + ',' + row["CantidadMaxima"] + ',' + row["CantidadGuardados"] + ',' + row["Obligatorio"] + ')" type="button" aria-label="Cargar ' + row["DescripcionNombreDocumento"] + ' guardados" title="Cargar ' + row["DescripcionNombreDocumento"] + ' guardados."><i class="fas fa-arrow-right"></i></button>'
                            }
                        },
                    ],
                    columnDefs: [
                        { targets: 'no-sort', orderable: false },
                    ]
                });
            }
            else
                MensajeError('Ocurrió un error al cargar los documentos del expediente, contacte al administrador.');
        }
    });
}

/**********************************************************************************************************************/
/******** Cargar documentos guardados del expedientes que sean del tipo de documento recibido como parametro **********/
/**********************************************************************************************************************/
function CargarDocumentosGuardadosPorTipoDeDocumento(idDocumento, Documento, descripcionDocumento, noAdjuntado, noAplica, documentosMinimos, documentosMaximos, documentosGuardados, obligatorio) {

    idTipoDeDocumento = idDocumento;
    nombreTipoDeDocumento = Documento;
    descripcionTipoDeDocumento = descripcionDocumento;
    EstadoNoAdjuntado = noAdjuntado;
    EstadoNoAplica = noAplica;
    documentoObligatorio = obligatorio;
    cantidadMinimaDocumentos = documentosMinimos;
    cantidadMaximaDocumentos = documentosMaximos;
    cantidadDocumentosGuardados = documentosGuardados;

    $.ajax({
        type: "POST",
        url: "Expedientes_Consultar.aspx/CargarDocumentosDelExpedientePorIdDocumento",
        data: JSON.stringify({ idTipoDocumento: idTipoDeDocumento, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los ' + nombreTipoDeDocumento + ' guardados, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                if (data.d.length > 0) {
                    MostrarVistaPrevia(data.d[0].URL, data.d[0].DescripcionNombreDocumento, 'divPrevisualizacionDocumento_TipoDeDocumento');
                }
                else
                    $("#divPrevisualizacionDocumento_TipoDeDocumento").empty();

                /* Inicializar datatables de documentos */
                $('#tblListadoTipoDocumento').DataTable({
                    "destroy": true,
                    "pageLength": 100,
                    "aaSorting": [],
                    "language": language,
                    dom: 'f',
                    data: data.d,
                    "columns": [
                        { "data": "NombreArchivo", "className": "font-12" },
                        {
                            "data": "URL", "className": "text-center",
                            "render": function (data, type, row) {

                                return '<button class="btn btn-sm btn-secondary" onclick="MostrarVistaPrevia(' + "'" + row["URL"] + "'" + ',' + "'" + row["DescripcionNombreDocumento"] + "'" + ',' + "'divPrevisualizacionDocumento_TipoDeDocumento'" + ')" type="button"><i class="fas fa-search"></i></button>' +
                                    '<a href="' + row["URL"] + '" download="' + row["DescripcionNombreDocumento"] + "_" + row["NombreArchivo"] + '" class="btn btn-sm btn-secondary mb-0 ml-1" title="Descargar documento"><i class="fas fa-download"></i></a>' +
                                    '<button onclick="EliminarDocumentoExpediente(' + row["IdExpedienteDocumento"] + ')" class="btn btn-sm btn-danger mb-0 ml-1" type="button" title="Eliminar documento"><i class="far fa-trash-alt"></i></button>'
                            }
                        },
                    ],
                    columnDefs: [
                        { targets: 'no-sort', orderable: false },
                    ]
                });

                $("#lblTituloListadoTipoDocumento").text(nombreTipoDeDocumento);
                $("#lblDescripcionTipoDeDocumento").prop('title', descripcionTipoDeDocumento);

                $("#btnAgrearNuevoTipoDocumento").prop('disabled', idTipoDeDocumento == 0 ? true : false);

                if (EstadoNoAdjuntado == true && idTipoDeDocumento != 0) // si ya está marcado como no adjunto no mostrar el boton
                    $("#btnCambiarEstadoANoAdjuntado").prop('disabled', true).css('display', 'none');
                else if (EstadoNoAdjuntado == false && idTipoDeDocumento != 0 && data.d.length == 0) // si no está marcado como no y el tipo de documento es diferente a cero y no hay ningun documento del tipo de documento seleccionado, mostrar el boton
                    $("#btnCambiarEstadoANoAdjuntado").prop('disabled', false).css('display', '');
                else if (idTipoDeDocumento != 0) // si el tipo de documento es diferente a cero, no mostrar el boton
                    $("#btnCambiarEstadoANoAdjuntado").prop('disabled', true).css('display', 'none');
                else if (idTipoDeDocumento == 0 && EstadoNoAdjuntado != undefined) // si el tipo de documento es igual a cero y el estado ha sido definido, mostrar el boton
                    $("#btnCambiarEstadoANoAdjuntado").prop('disabled', false).css('display', '');


                if (EstadoNoAplica == true && idTipoDeDocumento != 0)
                    $("#btnCambiarEstadoANoAplica").prop('disabled', true).css('display', 'none');
                else if (EstadoNoAplica == false && idTipoDeDocumento != 0 && data.d.length == 0)
                    $("#btnCambiarEstadoANoAplica").prop('disabled', false).css('display', '');
                else if (idTipoDeDocumento != 0)
                    $("#btnCambiarEstadoANoAplica").prop('disabled', true).css('display', 'none');
                else if (idTipoDeDocumento == 0 && EstadoNoAplica != undefined)
                    $("#btnCambiarEstadoANoAplica").prop('disabled', false).css('display', '');
            }
            else
                MensajeError('No se pudieron cargar los documentos de tipo ' + nombreTipoDeDocumento + ' guardados, contacte al administrador.');
        }
    });

}

/***************************************************************************************************************/
/************** Cargar documentos del expedientes agrupado por el grupo de archivos seleccionado ***************/
/***************************************************************************************************************/
function CargarDocumentosPorGrupoDeArchivos(idGrupoDeArchivos, nombreGrupoDeArchivos, descripcionGrupoDeArchivos) {

    $.ajax({
        type: "POST",
        url: "Expedientes_Consultar.aspx/CargarDocumentosPorGrupoDeArchivos",
        data: JSON.stringify({ idGrupoDeArchivos: idGrupoDeArchivos, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los documentos del grupo de archivos, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                if (data.d.length == 0) {
                    $("#btnEnviarGrupoArchivoPorPDF,#btnGuardarGrupoArchivo").prop('disabled', true).prop('title', '');
                }
                else {
                    $("#btnEnviarGrupoArchivoPorPDF,#btnGuardarGrupoArchivo").prop('disabled', false).prop('title', '');
                }

                /* Inicializar datatables de documentos */
                $('#tblDocumentosDelGrupoDeArchivos').DataTable({
                    "destroy": true,
                    "pageLength": 100,
                    "aaSorting": [],
                    "language": language,
                    dom: 'f',
                    data: data.d,
                    "columns": [
                        { "data": "DescripcionNombreDocumento", "className": "font-12" },
                        {
                            "data": "URL", "className": "text-center",
                            "render": function (data, type, row) {

                                if (row["IdExpedienteDocumento"] == 0) {
                                    $("#btnEnviarGrupoArchivoPorPDF,#btnGuardarGrupoArchivo").prop('disabled', true).prop('title', 'Grupo de archivos incompleto.');
                                }

                                return row["IdExpedienteDocumento"] != 0 ? '<button class="btn btn-sm btn-secondary" onclick="MostrarVistaPrevia(' + "'" + row["URL"] + "'" + ',' + "'" + row["DescripcionNombreDocumento"] + "'" + ',' + "'divPrevisualizacionDocumento_GrupoDeArchivos'" + ')" type="button" aria-label="Vista previa del documento"><i class="fas fa-search"></i></button>' : '<span class="badge badge-danger">Pendiente</span>'
                            }
                        },
                    ],
                    columnDefs: [
                        { targets: 'no-sort', orderable: false },
                    ]
                });

                $("#lblNombreGrupoDeArchivos").text(nombreGrupoDeArchivos);
                $("#lblDescripcionDetalladaGrupoDeArchivos").text(descripcionGrupoDeArchivos);

                $("#divPrevisualizacionDocumento").empty();
                $("#modalGrupoDeDocumentos").modal();
            }
            else
                MensajeError('No se pudo cargar el expediente, contacte al administrador.');
        }
    });
}

/***********************************************************************************************/
/*************************** ELIMINAR DOCUMENTO DEL EXPEDIENTE *********************************/
/***********************************************************************************************/
function EliminarDocumentoExpediente(idDocumentoExpediente) {

    $.ajax({
        type: "POST",
        url: "Expedientes_Consultar.aspx/ElimiarDocumentoExpediente",
        data: JSON.stringify({ idDocumentoExpediente: idDocumentoExpediente, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo eliminar el documento, contacte al administrador.');
        },
        success: function (data) {

            if (data.d == true) {

                MensajeExito('¡El documento se eliminó exitosamente!');
                CargarDocumentosDelExpediente();
                CargarDocumentosGuardadosPorTipoDeDocumento(idTipoDeDocumento, nombreTipoDeDocumento, descripcionTipoDeDocumento, EstadoNoAdjuntado, EstadoNoAplica, cantidadMinimaDocumentos, cantidadMaximaDocumentos, cantidadDocumentosGuardados, documentoObligatorio);
            }
            else {
                MensajeError('Ocurrió un error al eliminar el documento, contacte al administrador.');
            }
        }
    });
}

/***********************************************************************************************/
/*************** CAMBIAR EL ESTADO DE UN TIPO DE DOCUMENTOS EN EL EXPEDIENTE *******************/
/***********************************************************************************************/
$("#btnCambiarEstadoANoAdjuntado,#btnCambiarEstadoANoAplica").click(function () {

    if (idTipoDeDocumento != 0) {

        let idNuevoEstadoDocumento = $(this).data('idestado');

        $.ajax({
            type: "POST",
            url: "Expedientes_Consultar.aspx/CambiarEstadoDocumentosPorIdDocumento",
            data: JSON.stringify({ idTipoDeDocumento: idTipoDeDocumento, idEstadoDocumento: idNuevoEstadoDocumento, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cambiar el estado del documento, contacte al administrador.');
            },
            success: function (data) {

                if (idNuevoEstadoDocumento === 2)
                    EstadoNoAdjuntado = true;
                else
                    EstadoNoAdjuntado = false;

                if (idNuevoEstadoDocumento === 3)
                    EstadoNoAplica = true;
                else
                    EstadoNoAplica = false;

                if (data.d.ResultadoExitoso == true) {

                    MensajeExito(data.d.MensajeResultado);
                    CargarDocumentosDelExpediente();
                    CargarDocumentosGuardadosPorTipoDeDocumento(idTipoDeDocumento, nombreTipoDeDocumento, descripcionTipoDeDocumento, EstadoNoAdjuntado, EstadoNoAplica, cantidadMinimaDocumentos, cantidadMaximaDocumentos, cantidadDocumentosGuardados, documentoObligatorio);
                }
                else {
                    MensajeError(data.d.MensajeResultado);
                }
            }
        });
    }
    else
        MensajeError('Primero debes seleccionar un tipo de documento.');
});

/***********************************************************************************************/
/*************** AGREGAR NUEVO DOCUMENTO DEL TIPO DE DOCUMENTO SELECCIONADO ********************/
/***********************************************************************************************/
$("#btnAgrearNuevoTipoDocumento").click(function () {

    if (idTipoDeDocumento == 0) {
        return false;
    }
    
    $("#divFormularioDocumentos").empty();
    $("#lblTituloTipoDocumento").text(nombreTipoDeDocumento);
    $("#lblDescripcionTipoDocumento").text(descripcionTipoDeDocumento);
    $("#lblDocumentoObligatorio").text(documentoObligatorio == true ? 'Documento obligatorio' : 'Documento opcional').removeClass('text-danger').addClass(documentoObligatorio == true ? 'text-danger' : '');
    $("#lblCantidadMinimaDocumentos").text(cantidadMinimaDocumentos);
    $("#lblCantidadMaximaDocumentos").text(cantidadMaximaDocumentos);
    $("#lblCantidadDocumentosGuardados").text(cantidadDocumentosGuardados);

    $.ajax({
        type: "POST",
        url: "Expedientes_Consultar.aspx/ReiniciarListaDocumentosAdjuntados",
        data: JSON.stringify({ reiniciarListaDocumentosAdjuntados: true }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Ocurrió un error al cargar la información del documento seleccionado, contacte al administrador.');
        },
        success: function (data) {

            if (data.d == false) {
                MensajeError('Ocurrió un error al cargar la información del documento seleccionado, contacte al administrador.');
                return false;
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

            var divDocumentacion = $("#divFormularioDocumentos");

            var idInput = 'Documento_' + idTipoDeDocumento;

            divDocumentacion.append(
                '<form action="Expedientes_Consultar.aspx?type=upload&IdTipoDocumento=' + idTipoDeDocumento + '" method="post" enctype="multipart/form-data">' +
                '<input type="file" class="filestyle" data-buttonname="btn-secondary" id="' + idInput + '" name="files" data-tipo="' + idTipoDeDocumento + '"/>' +
                '</form>');

            $('#' + idInput + '').fileuploader({
                inputNameBrackets: false,
                changeInput: formatoInputFile,
                theme: 'dragdrop',
                limit: cantidadMaximaDocumentos, // Limite de archivos a subir
                maxSize: 500, // Peso máximo de todos los archivos seleccionado en megas (MB)
                fileMaxSize: 50, // Peso máximo de un archivo
                extensions: ['jpg', 'png', 'jpeg'],// Extensiones/formatos permitidos
                upload: {
                    url: 'Expedientes_Consultar.aspx?type=upload&IdTipoDocumento=' + idTipoDeDocumento,
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
                    $.post('Expedientes_Consultar.aspx?type=remove', { file: item.name });
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
                captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], {
                    feedback: 'Arrastra y suelta los archivos aqui',
                    feedback2: 'Arrastra y suelta los archivos aqui',
                    drop: 'Arrastra y suelta los archivos aqui',
                    button: 'Buscar archivos',
                    confirm: 'Confirmar',
                    cancel: 'Cancelar',
                }),
            }); /* Termina fileUploader*/

            $("#modalGuardarDocumentos").modal({ backdrop: 'static' });
        }
    }); /* Termina Ajax */
});

/***********************************************************************************************/
/************************************ FUNCIONES UTILITARIAS ************************************/
/***********************************************************************************************/
function EnviarCorreo(asunto, tituloGeneral, idContenidoHtml) {

    let contenidoHtml = $('#' + idContenidoHtml + '').html();

    $.ajax({
        type: "POST",
        url: "Expedientes_Consultar.aspx/EnviarDocumentoPorCorreo",
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

/* Mostrar vista previa del documento */
function MostrarVistaPrevia(url, descripcion, idDivPrevisualizacion) {

    let imgTemplate = '<img alt="' + descripcion + '" src="' + url + '" data-image="' + url + '" data-description="' + descripcion + '"/>';

    $("#" + idDivPrevisualizacion + "").empty().append(imgTemplate).unitegallery();
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