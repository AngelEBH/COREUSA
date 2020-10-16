var iconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">,</label></i>';
var iconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">.</label></i>';
var identidad = '';
var idSolicitud = 0;
var filtroActual = '';
var nombreCliente = '';

$(document).ready(function () {

    dtListado = $('#datatable-listado').DataTable({
        "pageLength": 15,
        "aaSorting": [],
        "dom": "<'row'<'col-sm-12'>>" +
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
            "thousands": ",",
            buttons: {
                copyTitle: 'Copiado al portapapeles',
                copySuccess: {
                    _: '%d Lineas copiadas',
                    1: '1 linea copiada'
                }
            }
        },
        "ajax": {
            type: "POST",
            url: "SolicitudesCredito_ListadoGarantias.aspx/CargarListado",
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
            { "data": "IdSolicitud", "className": "text-center" },
            { "data": "Agencia" },
            {
                "data": "Producto",
                "render": function (value) {
                    return value.split(' ')[1]
                }
            },
            { "data": "Identidad" },
            {
                "data": "PrimerNombre",
                "render": function (data, type, row) {
                    return row["PrimerNombre"] + ' ' + row["SegundoNombre"] + ' ' + row["PrimerApellido"] + ' ' + row["SegundoApellido"]
                }
            },
            {
                "data": "FechaCreacion",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            {
                "data": "IdGarantia", "className": "text-center",
                "render": function (data, type, row) {

                    return row["IdGarantia"] != 0 ? iconoExito : iconoPendiente;
                }
            },
            {
                "data": "IdGarantia", "className": "text-center",
                "render": function (value) {

                    return value == 0 ? '<button id="btnGuardar" data-id="' + value + '" class="btn btn-sm btn-block btn-info mb-0">Registrar</button>' :
                        '<button id="btnActualizar" data-id="' + value + '" class="btn btn-sm btn-block btn-info mb-0">Actualizar</button>';
                }
            }
        ],
        columnDefs: [
            { targets: 6, orderable: false },
            { "width": "1%", "targets": 0 }
        ]
    });

    /* Filtrar cuando se seleccione una opción */
    $("input[type=radio][name=filtros]").change(function () {

        let filtro = this.value;
        dtListado.columns().search("").draw();

        switch (filtro) {
            case "0":
                dtListado.columns(7).search("").draw();
                break;
            case "1":
                dtListado.columns(7).search("Guardar").draw();
                break;
            case "2":
                dtListado.columns(7).search("Actualizar").draw();
                break;
            default:
                dtListado.columns(7).search("").draw();
        }
    });

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            dtListado.columns(5)
                .search('/' + this.value + '/')
                .draw();
        }
        else {
            dtListado.columns(5)
                .search('')
                .draw();
        }
    });

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        dtListado.columns(5)
            .search(this.value + '/')
            .draw();
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
        dtListado.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        if (filtroActual == 'rangoFechas') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[5]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else { return true; }
    });

    $("#añoIngreso").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtListado.search($(this).val()).draw();
    });

    $("#datatable-listado tbody").on("click", "tr", function () {

        var row = dtListado.row(this).data();
        idSolicitud = row.IdSolicitud;
        nombreCliente = row["PrimerNombre"] + ' ' + row["SegundoNombre"] + ' ' + row["PrimerApellido"] + ' ' + row["SegundoApellido"];
    });
});

$(document).on('click', 'button#btnActualizar', function () {

    $("#lblIdSolicitudActualizar").text(idSolicitud);
    $("#lblNombreClienteActualizar").text(nombreCliente);
    $("#modalActualizarGarantia").modal();
});

$(document).on('click', 'button#btnGuardar', function () {

    $("#lblIdSolicitudGuardar").text(idSolicitud);
    $("#lblNombreClienteGuardar").text(nombreCliente);
    $("#modalGuardarGarantia").modal();
});

$("#btnGuardarGarantia").click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitud: idSolicitud, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo redireccionar al registro de la garantía");
        },
        success: function (data) {
            data.d != "-1" ? window.location = "Garantia_Registrar.aspx?" + data.d : MensajeError("No se pudo redireccionar al registro de la garantía");
        }
    });
});


$("#btnRegistrarGarantiaSinSolicitud").click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitud: 0, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo redireccionar al registro de la garantía");
        },
        success: function (data) {
            data.d != "-1" ? window.location = "Garantia_Registrar.aspx?" + data.d : MensajeError("No se pudo redireccionar al registro de la garantía");
        }
    });
});

$("#btnActualizarGarantia").click(function (e) {

    iziToast.info({
        title: 'Atención',
        message: 'Esta función sigue en desarrollo.'
    });

    //$.ajax({
    //    type: "POST",
    //    url: "SolicitudesCredito_ListadoGarantias.aspx/EncriptarParametros",
    //    data: JSON.stringify({ idSolicitud: idSolicitud, dataCrypt: window.location.href}),
    //    contentType: "application/json; charset=utf-8",
    //    error: function (xhr, ajaxOptions, thrownError) {
    //        MensajeError("No se pudo cargar la solicitud, contacte al administrador");
    //    },
    //    success: function (data) {
    //        data.d != "-1" ? window.location = "Garantia_Registrar.aspx?" + data.d : MensajeError("No se pudo redireccionar al registro de la garantía");
    //    },
    //});
});

jQuery("#date-range").datepicker({
    toggleActive: !0
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}