var FiltroActual = "";
var idPreSolicitud = 0;
var identidadCliente = "";
var telefonoCliente = '';
estadoMasRelevante = '';

$(document).ready(function () {

    dtPreSolicitudes = $('#datatable-presolicitudes').DataTable({
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
            url: "PreSolicitud_Listado.aspx/CargarPreSolicitudes",
            contentType: 'application/json; charset=utf-8',
            "dataSrc": function (json) {
                var return_data = json.d;
                return return_data;
            }
        },
        "columns": [
            { "data": "IdentidadCliente" },
            { "data": "NombreCliente" },
            {
                "data": "FechaCreacion",
                "render": function (value) {
                    if (value === '/Date(-2208967200000)/') return "";
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm:ss a');
                }
            },
            { "data": "Agencia" },
            {
                "data": "GestorValidador",
                "render": function (value) {
                    return value == '' ? 'No Asingado' : value;
                }
            },
            {
                "data": "IdEstadoPreSolicitud",
                "render": function (data, type, row) {
                    return '<label class="btn btn-sm btn-block btn-' + (row['EstadoFavorable'] == 0 ? 'warning' : row['EstadoFavorable'] == 1 ? 'success' : 'danger') + ' mb-0">' + row['EstadoPreSolicitud'] + '</label>';
                }
            },
            {
                "data": "IdPreSolicitud",
                "render": function (value) {
                    return '<button id="btnDetalles" data-id="' + value + '" class="btn btn-sm btn-block btn-info mb-0">Detalles</button>';
                }
            }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            dtPreSolicitudes.columns(2)
                .search('/' + this.value + '/')
                .draw();
        }
        else {
            dtPreSolicitudes.columns(2)
                .search('')
                .draw();
        }
    });

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        dtPreSolicitudes.columns(2)
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
        dtPreSolicitudes.draw();
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

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtPreSolicitudes.search($(this).val()).draw();
    });

    $("#datatable-presolicitudes tbody").on("click", "tr", function () {

        var row = dtPreSolicitudes.row(this).data();
        idPreSolicitud = row.IdPreSolicitud;
        identidadCliente = row.IdentidadCliente;
        $("#txtNombreCliente").val(row.NombreCliente);
        $("#txtIdentidadCliente").val(identidadCliente);
    });
});

$(document).on('click', 'button#btnDetalles', function () {

    $.ajax({
        type: "POST",
        url: "PreSolicitud_Listado.aspx/DetallesPreSolicitud",
        data: JSON.stringify({ idPreSolicitud: idPreSolicitud }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información de la pre solicitud, contacte al administrador');
        },
        success: function (data) {

            if (data.d != "-1") {

                var preSolicitud = data.d;

                // estado pre solicitud
                var classEstadoPreSolicitud = preSolicitud.EstadoFavorable == 0 ? 'warning' : preSolicitud.EstadoFavorable == 1 ? 'success' : preSolicitud.EstadoFavorable == 2 ? 'danger' : 'warning';
                $("#lblEstadoPreSolicitud").text(preSolicitud.EstadoPreSolicitud);
                $("#lblEstadoPreSolicitud").removeClass('btn-danger').removeClass('btn-success').removeClass('btn-warning').addClass('btn-' + classEstadoPreSolicitud);

                $("#txtTelefonoCliente").val(preSolicitud.Telefono);
                $("#txtDepartamento").val(preSolicitud.Departamento);
                $("#txtMunicipio").val(preSolicitud.Municipio);
                $("#txtCiudadPoblado").val(preSolicitud.CiudadPoblado);
                $("#txtBarrioColonia").val(preSolicitud.BarrioColonia);
                $("#txtDireccionDetallada").val(preSolicitud.DireccionDetallada);
                $("#txtReferenciasDireccionDetallada").val(preSolicitud.ReferenciasDireccionDetallada);

                $("#txtNombreTrabajo").val(preSolicitud.NombreTrabajo);
                $("#txtTelefonoAdicional").val(preSolicitud.TelefonoAdicional);
                $("#txtExtensionRecursosHumanos").val(preSolicitud.ExtensionRecursosHumanos);
                $("#txtExtensionCliente").val(preSolicitud.ExtensionCliente);

                if (preSolicitud.IdTipoDeUbicacion == 1) {

                    $("#lblTipoDeUbicacion").text('Investigación de domicilio');
                }
                else if (preSolicitud.IdTipoDeUbicacion == 2) {
                    $("#lblTipoDeUbicacion").text('Investigación de trabajo');
                }

                // gestoria
                $("#txtGestorAsignado").val(preSolicitud.IdGestorValidador == 0 ? 'No Asignado' : preSolicitud.GestorValidador);
                $("#txtGestion").val(preSolicitud.GestionDeCampo);
                $("#txtFechaDescargadoPorGestor").val(preSolicitud.FechaDescargadoPorGestor == '/Date(-2208967200000)/' ? 'Aún no recibido' : moment(preSolicitud.FechaDescargadoPorGestor).locale('es').format('YYYY/MM/DD hh:mm:ss a'));
                $("#txtFechaValidacion").val(preSolicitud.FechaValidacion == '/Date(-2208967200000)/' ? 'Áún no validado' : moment(preSolicitud.FechaValidacion).locale('es').format('YYYY/MM/DD hh:mm:ss a'));
                $("#txtObservacionesGestoria").val(preSolicitud.ObservacionesDeCampo);

                // auditoria
                $("#txtUsuarioCreacion").val(preSolicitud.UsuarioCrea);
                $("#txtFechaCreacion").val(moment(preSolicitud.FechaCreacion).locale('es').format('YYYY/MM/DD hh:mm:ss a'));

                $("#txtUsuarioUltimaModificacion").val(preSolicitud.UsuarioUltimaMoficiacion == '' ? 'Sin Modificaciones' : preSolicitud.UsuarioUltimaMoficiacion);
                $("#txtFechaUltimaModificacion").val(preSolicitud.FechaUltimaModificacion == '/Date(-2208967200000)/' ? 'Sin Modificaciones' : moment(preSolicitud.FechaUltimaModificacion).locale('es').format('YYYY/MM/DD hh:mm:ss a'));

                $("#modalDetalles").modal();
            }
            else {
                MensajeError('No se pudo cargar los detalles de la pre solicitud, contacte al administrador.');
            }
        }
    });
});

jQuery('#date-range').datepicker({
    toggleActive: true
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}