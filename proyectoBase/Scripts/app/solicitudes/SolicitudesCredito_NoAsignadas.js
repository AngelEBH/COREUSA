var idSolicitud = 0;
var filtroActual = '';

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
            "thousands": ","
        },
        "ajax": {
            type: "POST",
            url: "SolicitudesCredito_NoAsignadas.aspx/CargarListado",
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
            { "data": "UsuarioAsignado" },
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
                    return row["PrimerNombre"] + ' ' + row["SegundoNombre"] + ' ' + row["PrimerApellido"] + ' ' + row["SegundoApellido"] + ' ' + (row["IdCanal"] == 3 ? '<span class="btn btn-sm btn-info pt-0 pb-0 m-0">canex</span>' : '')
                }
            },
            {
                "data": "FechaCreacion",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm:ss a');
                }
            },
            {
                "data": "IdSolicitud", "className": "text-center",
                "render": function (value) {

                    return '<button type="button" class="btn btn-sm btn-info pt-1 pb-1 mt-0 mb-0" data-id="'+ value +'" id="btnAsignar"><i class="fas fa-tasks"></i> Asignar</button>';
                }
            }
        ],
        columnDefs: [
            { targets: [7], orderable: false },
            { "width": "1%", "targets": 0 }
        ]
    });

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            dtListado.columns(6).search('/' + this.value + '/').draw();
        }
        else {
            dtListado.columns(6).search('').draw();
        }
    });

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        dtListado.columns(6).search(this.value + '/').draw();
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
                FechaIngreso = new Date(a[6]);
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
    });

    $("#ddlGestores").select2({
        language: {
            errorLoading: function () { return "No se pudieron cargar los resultados" },
            inputTooLong: function (e) { var n = e.input.length - e.maximum, r = "Por favor, elimine " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
            inputTooShort: function (e) { var n = e.minimum - e.input.length, r = "Por favor, introduzca " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
            loadingMore: function () { return "Cargando más resultados…" },
            maximumSelected: function (e) { var n = "Sólo puede seleccionar " + e.maximum + " elemento"; return 1 != e.maximum && (n += "s"), n },
            noResults: function () { return "No se encontraron resultados" },
            searching: function () { return "Buscando…" },
            removeAllItems: function () { return "Eliminar todos los elementos" }
        }
    });
});

$(document).on('click', 'button#btnAsignar', function () {

    $("#lblIdSolicitud").text(idSolicitud);
    $("#modalAsignarSolicitud").modal();
});

$("#btnAsignar_Confirmar").click(function (e) {

    if ($("#ddlGestores :selected").val() != '') {

        let idGestor = $("#ddlGestores :selected").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_NoAsignadas.aspx/AsignarSolicitud",
            data: JSON.stringify({ idSolicitud: idSolicitud, idGestor: idGestor, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se asignar la solicitud, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La solicitud fue asignada correctamente.');
                }
                else {
                    MensajeError("No se asignar la solicitud, contacte al administrador.");
                }

                $("#modalAsignarSolicitud").modal('hide');
            }
        });
    }
    else {
        $("#ddlGestores").focus();
    }
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

function MensajeExito(mensaje) {
    iziToast.success({
        title: 'Exito',
        message: mensaje
    });
}