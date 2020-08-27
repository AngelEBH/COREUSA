var idSolicitud = 0;
var classEstado = '';

$(document).ready(function () {

    dtListaMantenimiento = $('#datatable-listaMantenimiento').DataTable({
        "responsive": true,
        "pageLength": 20,
        "aaSorting": [],
        "dom": "<'row'<'col-sm-6'><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
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
        "ajax": {
            type: "POST",
            url: "SolicitudesCredito_ListaMantenimiento.aspx/CargarSolicitudes",
            contentType: 'application/json; charset=utf-8',
            data: function (dtParms) {
                return JSON.stringify({ dataCrypt: window.location.href });
            },
            "dataSrc": function (json) {
                var return_data = json.d;
                return return_data;
            }
        },
        "columns": [
            { "data": "IDSolicitud" },
            { "data": "IdentidadCliente" },
            { "data": "NombreCliente" },
            {
                "data": "FechaIngreso",
                "render": function (value) { return moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a'); }
            },
            { "data": "DescripcionTipoPrestamo" },
            {
                "data": "DescripcionEstado",
                "render": function (data, type, row) {

                    if (row["IDEstado"] == 1) {
                        classEstado = 'warning';
                    }
                    else if (row["IDEstado"] == 2) {
                        classEstado = 'warning';
                    }
                    else if (row["IDEstado"] == 3) {
                        classEstado = 'warning';
                    }
                    else if (row["EstadoDeCampo"] == 1) {
                        classEstado = 'warning';
                    }
                    if (row["IDEstado" == 4] || row["IDEstado"] == 5 || row["IDEstado"] == 7) {
                        classEstado = row["IDEstado"] == 7 ? 'success' : 'danger';
                    }

                    return '<label class="btn btn-sm btn-block m-0 btn-' + classEstado + '">' + row["DescripcionEstado"] + '</label>';
                }
            },
            {
                "data": "IDSolicitud",
                "render": function (value) {
                    return '<button id="btnMantenimiento" data-id="' + value + '" class="btn btn-sm btn-block btn-info mb-0">Mantenimiento</button>';
                }
            }
        ]
    });

    /* busqueda por numero de solicitud */
    $('#NoSolicitud').keyup(function () {
        dtListaMantenimiento.columns(0)
            .search(this.value)
            .draw();
    });

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtListaMantenimiento.search($(this).val()).draw();
    });
});

$(document).on('click', 'button#btnMantenimiento', function () {
    $("#modalActualizarSolicitud").modal();
    idSolicitud = $(this).data('id');
});

$('#btnAbrirMantenimiento').click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListaMantenimiento.aspx/EncriptarParametros",
        data: JSON.stringify({ dataCrypt: window.location.href, IDSOL: idSolicitud }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la solicitud, contacte al administrador');
        },
        success: function (data) {

            if (data.d != "-1") {
                window.location = "SolicitudesCredito_Mantenimiento.aspx?" + data.d;
            }
            else {
                MensajeError('No se pudo al redireccionar a pantalla de mantenimiento');
            }
        }
    });

});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}