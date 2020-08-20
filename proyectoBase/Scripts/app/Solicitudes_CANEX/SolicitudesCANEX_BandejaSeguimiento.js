var Identidad = '';
var IDSol = 0;
var FiltroActual = "";

$(document).ready(function () {
    var tablaSolicitudes = $('#tblSolicitudesCanex').DataTable(
        {
            //"responsive": true,
            "pageLength": 20,
            "aaSorting": [],
            //"processing": true,
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
            "ajax":
            {
                type: "POST",
                url: "SolicitudesCANEX_BandejaSeguimiento.aspx/CargarSolicitudesCANEXSeguimiento",
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
                { "data": "NombreSocio" },
                { "data": "IDSolicitudCanex" },
                {
                    "data": "FechaIngresoSolicitud", "orderable": false,
                    "render": function (value) {
                        if (value === null) return "";
                        return moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                    }
                },
                { "data": "Identidad" },
                { "data": "NombreCliente" },
                { "data": "NombreProducto" },
                {
                    "data": "ValorGlobal",
                    "className": 'text-right sum',
                    "render": function (data, type, row) {
                        return row["Moneda"] + ' ' + addFormatoNumerico(parseFloat(row["ValorGlobal"]).toFixed(2))
                    }
                },
                {
                    "data": "ValorPrima",
                    "className": 'text-right sum',
                    "render": function (data, type, row) {
                        return row["Moneda"] + ' ' + addFormatoNumerico(parseFloat(row["ValorPrima"]).toFixed(2))
                    }
                },
                {
                    "data": "ValorPrestamo",
                    "className": 'text-right sum',
                    "render": function (data, type, row) {
                        return row["Moneda"] + ' ' + addFormatoNumerico(parseFloat(row["ValorPrestamo"]).toFixed(2))
                    }
                },
                { "data": "NombreAgencia" },
                { "data": "NombreUsuario" },
                {
                    "data": "DescripcionEstadoSolicitud",
                    "render": function (data, type, row) {
                        return '<label class="btn btn-sm btn-block mb-0 btn-' + GetEstadoClass(row["IDEstadoSolicitud"]) + '">' + row["DescripcionEstadoSolicitud"] + '</label>'
                    }
                },
                {
                    "data": "IDSolicitudCanex",
                    "render": function (value) {

                        var btnDetalles = '<button type="button" id="btnDetalles" data-id="' + value + '" class="btn btn-sm btn-info mb-0">Detalles</button>';
                        return btnDetalles;
                    }
                }
            ],
            "columnDefs": [
                { "orderable": false, "targets": 0 }
            ]
        });

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            tablaSolicitudes.columns(2)
                .search('/' + this.value + '/')
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
            .search(this.value + '/')
            .draw();
    });

    $("#min").datepicker({
        onSelect: function () {
            FiltroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#max").datepicker({
        onSelect: function () {
            FiltroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#min, #max").change(function () {
        FiltroActual = 'rangoFechas';
        tablaSolicitudes.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        if (FiltroActual == 'rangoFechas') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[2]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else { return true; }
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
            $.ajax({
                type: "POST",
                url: 'SolicitudesCANEX_BandejaSeguimiento.aspx/AbrirSolicitudSeguimientoDetalles',
                data: JSON.stringify({ ID: IDSol, Identidad: Identidad, dataCrypt: window.location.href }),
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

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        tablaSolicitudes.search($(this).val()).draw();
    })
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function addFormatoNumerico(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

function GetEstadoClass(IDEstado) {
    switch (IDEstado) {
        case 1:
            return "primary"
            break;
        case 2:
            return "info"
            break;
        case 3:
        case 7:
        case 6:
            return "warning";
            break;
        case 4:
            return "success";
            break;
        case 5:
            return "danger";
            break;
        default:
            return "secondary";
            break;
    }
}