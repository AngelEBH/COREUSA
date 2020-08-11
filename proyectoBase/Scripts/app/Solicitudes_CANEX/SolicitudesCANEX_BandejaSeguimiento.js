var Identidad = '';
var IDSol = 0;
$(document).ready(function () {
    var tablaSolicitudes = $('#tblSolicitudesCanex').DataTable(
        {
            lengthChange: false,
            "pageLength": 50,
            "paging": true,
            "responsive": true,
            dom: "rt<'row'<'col-sm-4'i><'col-sm-8'p>>",
            "language": {
                "sProcessing": "Cargando solicitudes...",
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
                "sLoadingRecords": "Cargando solicitudes...",
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
            "ajax":
            {
                type: "POST",
                url: "SolicitudesCANEX_BandejaSeguimiento.aspx/CargarSolicitudesCANEXSeguimiento?" + window.location.href.split("?")[1],
                contentType: 'application/json; charset=utf-8',
                data: function (d) {
                    return d;
                },
                "dataSrc": function (json) {
                    var return_data = json.d;
                    return return_data;
                }
            },
            "columns": [
                { "data": "NombreSocio" },
                { "data": "IDSolicitudCanex" },
                {
                    "data": "FechaIngresoSolicitud",
                    "render": function (value) {
                        if (value === null) return "";
                        return moment(value).lang('es').format('MMMM Do YYYY, h:mm:ss a');
                    }
                },
                { "data": "Identidad" },
                { "data": "NombreCliente" },
                { "data": "NombreProducto" },
                { "data": "ValorGlobal" },
                { "data": "ValorPrima" },
                { "data": "ValorPrestamo" },
                { "data": "DescripcionEstadoSolicitud" },
                { "data": "NombreAgencia" },
                { "data": "NombreUsuario" },
                {
                    "data": "IDSolicitudCanex",
                    "render": function (value) {

                        var btnDetalles = '<button type="button" id="btnDetalles" data-id="' + value + '" class="btn btn-sm btn-info mb-0">Detalles</button>';
                        return btnDetalles;
                    }
                }
            ]
        });

    /* busqueda por nombre del cliente */
    $('#nombreCliente').on('keyup', function () {
        tablaSolicitudes.columns(4)
            .search(this.value)
            .draw();
    });
    /* busqueda por identidad del cliente */
    $('#identidadCliente').on('keyup', function () {
        tablaSolicitudes.columns(3)
            .search(this.value)
            .draw();
    });
    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            tablaSolicitudes.columns(2)
                .search(this.value)
                .draw();
        }
        else {
            tablaSolicitudes.columns(2)
                .search('')
                .draw();
        }
    });
    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        tablaSolicitudes.columns(2)
            .search(this.value)
            .draw();
    });
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {
            var min = $('#min').datepicker('getDate');
            var max = $('#max').datepicker('getDate');
            //var startDate = new Date(data[2]);
            var startDate = moment(data[2], 'MMMM Do YYYY, h:mm:ss a');

            if (min == 'Invalid Date' && max == 'Invalid Date')
                return true;
            if (min == 'Invalid Date' && startDate <= max)
                return true;
            if (max == 'Invalid Date' && startDate >= min)
                return true;
            if (startDate <= max && startDate >= min)
                return true;
            return false;
        }
    );
    $('#min').datepicker({ onSelect: function () { tablaSolicitudes.draw(); }, changeMonth: true, changeYear: true });
    $('#max').datepicker({ onSelect: function () { tablaSolicitudes.draw(); }, changeMonth: true, changeYear: true });
    $('#min, #max').change(function () {
        tablaSolicitudes.draw();
    });

    $("#añoIngreso").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    $('#tblSolicitudesCanex tbody').on('click', 'tr', function () {
        var data = tablaSolicitudes.row(this).data();
        if (data != undefined) {
            Identidad = data.Identidad;
        }
    });

    $(document).on('click', 'button#btnDetalles', function () {
        if (Identidad != '') {
            IDSol = $(this).data('id');
            var qString = "?" + window.location.href.split("?")[1];
            $.ajax({
                type: "POST",
                url: 'SolicitudesCANEX_BandejaSeguimiento.aspx/AbrirSolicitudSeguimientoDetalles' + qString,
                data: JSON.stringify({ ID: IDSol, Identidad: Identidad }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError('Error al cargar detalles de la solicitud');
                },
                success: function (data) {
                    if (data.d != "-1") {
                        window.location = "SolicitudesCANEX_SeguimientoDetalles.aspx?" + data.d;
                    }
                    else {
                        MensajeError('No se pudo al redireccionar a pantalla de detalles');
                    }
                }
            });
        }
    });
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}