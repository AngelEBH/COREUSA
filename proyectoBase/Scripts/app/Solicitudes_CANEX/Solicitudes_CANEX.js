var identidad = '';
var idSolicitud = 0;
var filtroActual = "";

$(document).ready(function () {

    var tablaSolicitudes = $('#datatable-solicitudesCanex').DataTable(
        {
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
            "ajax":
            {
                type: "POST",
                url: "Solicitudes_CANEX.aspx/CargarSolicitudes",
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
                {
                    "data": "IDSolicitudCanex",
                    "render": function (value) {

                        return '<div class="dropdown mo-mb-2">' +
                            '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                            '<i class="fa fa-bars"></i>' +
                            '</button >' +
                            '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                            '<button type="button" class="dropdown-item" id="btnDetalles" data-id="' + value + '"><i class="fas fa-tasks"></i> Detalles</button>' +
                            '</div>' +
                            '</div >';
                    }
                },
                { "data": "NombreSocio" },
                { "data": "IDSolicitudCanex" },
                {
                    "data": "FechaIngresoSolicitud",
                    "render": function (value) {
                        if (value === null) return "";
                        return moment(value).locale('es').format('YYYY/MM/DD hh:mm:ss a');
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
                    "data": "EstadoSolicitud",
                    "render": function (data, type, row) {                        
                        return '<label class="btn btn-sm btn-block mb-0 btn-' + GetEstadoClass(row["IDEstadoSolicitud"]) + '">' + row["EstadoSolicitud"] + '</label>'
                    }
                }
            ],
            columnDefs: [
                { targets: [0], orderable: false }
            ]
        });

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            tablaSolicitudes.columns(2).search('/' + this.value + '/').draw();
        }
        else {
            tablaSolicitudes.columns(2).search('').draw();
        }
    });

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        tablaSolicitudes.columns(2).search(this.value + '/').draw();
    });

    $("#min").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#max").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#min, #max").change(function () {
        filtroActual = 'rangoFechas';
        tablaSolicitudes.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        if (filtroActual == 'rangoFechas') {
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

    $('#datatable-solicitudesCanex tbody').on('click', 'tr', function () {
        var data = tablaSolicitudes.row(this).data();
        if (data != undefined) {
            identidad = data.Identidad;
        }
    });

    $(document).on('click', 'button#btnDetalles', function () {

        if (identidad != '') {

            idSolicitud = $(this).data('id');
            $.ajax({
                type: "POST",
                url: 'Solicitudes_CANEX.aspx/AbrirSolicitudDetalles',
                data: JSON.stringify({ idSolicitud: idSolicitud, identidad: identidad, dataCrypt: window.location.href }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError('Error al cargar detalles de la solicitud');
                },
                success: function (data) {
                    if (data.d != "-1") {
                        window.location = "Solicitudes_CANEX_Detalles.aspx?" + data.d;
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