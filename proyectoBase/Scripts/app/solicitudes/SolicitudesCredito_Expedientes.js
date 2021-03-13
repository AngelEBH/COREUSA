
$(function () {

    CargarDocumentosDelExpediente();
});

/* Cargar el listado de documentos que corresponden al expediente (esto depende del tipo de producto). Con el indicador de la cantidad de documentos que se han guardado */
function CargarDocumentosDelExpediente() {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Expedientes.aspx/CargarDocumentosDelExpediente",
        data: JSON.stringify({ idExpediente: ID_EXPEDIENTE, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los documentos del expediente, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                /* Inicializar datatables de documentos */
                $('#tblTiposDeDocumentos').DataTable({
                    "destroy": true,
                    "pageLength": 100,
                    "aaSorting": [],
                    "language": {
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
                    },
                    dom: 'f',
                    data: data.d,
                    "columns": [
                        {
                            "data": "DescripcionNombreDocumento", "className": "font-12",
                            "render": function (data, type, row) {
                                return '<span class="' + ((row["Obligatorio"] == true && row["CantidadGuardados"] == 0) || (row["Obligatorio"] == true && row["CantidadGuardados"] < row["CantidadMinima"]) ? 'text-danger' : '') + '">' + row["DescripcionNombreDocumento"] + '<small class="badge badge-secondary ml-1">' + row["CantidadGuardados"] + '</small></span>'
                            }
                        },
                        {
                            "data": "IdDocumento", "className": "text-center",
                            "render": function (data, type, row) {

                                return '<button class="btn btn-sm btn-secondary" data-idtipodocumento="' + row["IdDocumento"] + '" data-descripcion="' + row["DescripcionNombreDocumento"] + '" data-descripciondetallada="' + row["DescripcionDetalladaDelDocumento"] + '" onclick="CargarDocumentosGuardadosPorTipoDeDocumento(this)" type="button" aria-label="Cargar lista de documentos guardados"><i class="fas fa-arrow-right"></i></button>'
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

function CargarDocumentosGuardadosPorTipoDeDocumento(btnCargarDocumentos) {

    let idTipoDeDocumento = $(btnCargarDocumentos).data('idtipodocumento');
    let nombreTipoDeDocumento = $(btnCargarDocumentos).data('descripcion');
    let descripcionTipoDeDocumento = $(btnCargarDocumentos).data('descripciondetallada');


}

/* Cargar documentos del expedientes agrupado por el grupo de archivos seleccionado */
function CargarDocumentosPorGrupoDeArchivos(idGrupoDeArchivos, nombreGrupoDeArchivos, descripcionGrupoDeArchivos) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Expedientes.aspx/CargarDocumentosPorGrupoDeArchivos",
        data: JSON.stringify({ idExpediente: ID_EXPEDIENTE, idGrupoDeArchivos: idGrupoDeArchivos, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los documentos del grupo de archivos, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                $("#btnEnviarGrupoArchivoPorPDF,#btnGuardarGrupoArchivo").prop('disabled', false).prop('title', '');

                /* Inicializar datatables de documentos */
                $('#tblDocumentosDelGrupoDeArchivos').DataTable({
                    "destroy": true,
                    "pageLength": 100,
                    "aaSorting": [],
                    "language": {
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
                    },
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

                                return row["IdExpedienteDocumento"] != 0 ? '<button class="btn btn-sm btn-secondary" data-url="' + row["URL"] + '" data-descripcion="' + row["DescripcionNombreDocumento"] + '" onclick="MostrarVistaPrevia(this)" type="button" aria-label="Vista previa del documento"><i class="fas fa-search"></i></button>' : '<span class="badge badge-danger">Pendiente</span>'
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
        url: "SolicitudesCredito_Expedientes.aspx/EnviarDocumentoPorCorreo",
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
function MostrarVistaPrevia(btnVistaPrevia) {

    let urlImagen = $(btnVistaPrevia).data('url');
    let descripcion = $(btnVistaPrevia).data('descripcion');
    let imgTemplate = '<img alt="' + descripcion + '" src="' + urlImagen + '" data-image="' + urlImagen + '" data-description="' + descripcion + '"/>';

    $("#divPrevisualizacionDocumento").empty().append(imgTemplate).unitegallery();
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