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

$(function () {

    CargarDocumentosDelExpediente();
    CargarDocumentosGuardadosPorTipoDeDocumento(0, 'Ningún documento seleccionado', '');
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

                                let documentoObligatorio = ((row["Obligatorio"] == true && row["CantidadGuardados"] == 0) || (row["Obligatorio"] == true && row["CantidadGuardados"] < row["CantidadMinima"]));
                                let className = documentoObligatorio == true ? 'danger' : 'secondary';

                                return '<span title="' + row["DescripcionDetalladaDelDocumento"] + '">' + row["DescripcionNombreDocumento"] + '<small class="badge badge-' + className + ' ml-1" title="' + (documentoObligatorio == true ? 'Este documento es obligatorio. Documentos guardados: ' + row["CantidadGuardados"] + '. Cantidad minima a guardar: ' + row["CantidadMinima"] : '') + '">' + row["CantidadGuardados"] + '</small></span>'
                            }
                        },
                        {
                            "data": "IdDocumento", "className": "text-center",
                            "render": function (data, type, row) {

                                return '<button class="btn btn-sm btn-secondary" onclick="CargarDocumentosGuardadosPorTipoDeDocumento(' + "'" + row["IdDocumento"] + "'" + ',' + "'" + row["DescripcionNombreDocumento"] + "'" + ',' + "'" + row["DescripcionDetalladaDelDocumento"] + "'" + ')" type="button" aria-label="Cargar ' + row["DescripcionNombreDocumento"] + ' guardados" title="Cargar ' + row["DescripcionNombreDocumento"] + ' guardados."><i class="fas fa-arrow-right"></i></button>'
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
function CargarDocumentosGuardadosPorTipoDeDocumento(idTipoDeDocumento, nombreTipoDocumento, descripcionTipoDeDocumento) {

    $.ajax({
        type: "POST",
        url: "Expedientes_Consultar.aspx/CargarDocumentosDelExpedientePorIdDocumento",
        data: JSON.stringify({ idTipoDocumento: idTipoDeDocumento, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los ' + nombreTipoDocumento + ' guardados, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

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
                                    '<button id="btnEliminarDocumento" data-id="' + row["IdExpedienteDocumento"] + '" class="btn btn-sm btn-danger mb-0 ml-1" type="button" title="Eliminar documento"><i class="far fa-trash-alt"></i></button>'
                            }
                        },
                    ],
                    columnDefs: [
                        { targets: 'no-sort', orderable: false },
                    ]
                });

                $("#lblTituloListadoTipoDocumento").text(nombreTipoDocumento);
                $("#lblDescripcionTipoDeDocumento").prop('title', descripcionTipoDeDocumento);
            }
            else
                MensajeError('No se pudieron cargar los documentos de tipo ' + nombreTipoDocumento + ' guardados, contacte al administrador.');
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

                if (data.d.length == 0)
                    $("#btnEnviarGrupoArchivoPorPDF,#btnGuardarGrupoArchivo").prop('disabled', true).prop('title', '');
                else
                    $("#btnEnviarGrupoArchivoPorPDF,#btnGuardarGrupoArchivo").prop('disabled', false).prop('title', '');

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