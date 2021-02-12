var idSolicitud = 0;
var identidad = '';
var nombreCliente = '';

var idGarantia = 0;
var VIN = '';
var marcaGarantia = '';
var modeloGarantia = '';
var anioGarantia = '';

/* Iconos de estado para el listado de garantias de solicitudes aprobadas */
var iconoExito = '<i class="mdi mdi-check-circle mdi-24px p-0 text-success"><label style="display:none;">estadoListo</label></i>';
var iconoPendiente = '<i class="mdi mdi-check-circle mdi-24px p-0 text-secondary"><label style="display:none;">estadoPendiente</label></i>';
var iconoWarning = '<i class="mdi mdi-check-circle mdi-24px p-0 text-warning"><label style="display:none;">estadoPendiente</label></i>';

filtroActual = '';

$(document).ready(function () {

    dtListado = $('#datatable-listado-expedientes').DataTable({
        "pageLength": 15,
        "aaSorting": [],
        "dom": "<'row'<'col-sm-12'B>>" +
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
            url: "SolicitudesCredito_ListadoExpedientes.aspx/CargarListado",
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
                "data": "IdSolicitud", "className": "text-center",
                "render": function (data, type, row) {

                    return '<div class="dropdown mo-mb-2">' +
                        '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-bars"></i></button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<button type="button" class="dropdown-item" id="btnRevisarExpediente"><i class="fas fa-tasks"></i> Revisar</button>' +
                        '<button type="button" class="dropdown-item" id="btnDetallesExpediente"><i class="far fa-clipboard"></i> Detalles</button>' +
                        '<button type="button" class="dropdown-item" id="btnActualizarExpediente"><i class="far fa-edit"></i> Actualizar</button>' +
                        '</div>' +
                        '</div >';
                }
            },
            {
                "data": "IdSolicitud",
                "render": function (data, type, row) {
                    return row["IdSolicitud"] + ' <br><span class="text-muted">' + moment(row["FechaCreacion"]).locale('es').format('YYYY/MM/DD hh:mm a') + '</span>';
                }
            },
            {
                "data": "Agencia",
                "render": function (data, type, row) {
                    return row["Producto"] + ' <br/><span class="text-muted">' + row["Agencia"] + ' | ' + row["UsuarioAsignado"] + '</span>'
                }
            },
            {
                "data": "IdCliente",
                "render": function (data, type, row) {
                    return row["NombreCompleto"] + ' <br/><span class="text-muted">' + row["Identidad"] + "</span>" + (row["IdCanal"] == 3 ? ' <span class="btn btn-sm btn-info pt-0 pb-0 m-0">canex</span>' : '')
                }
            },
            {
                "data": "IdGarantia",
                "render": function (data, type, row) {
                    return row["Marca"] + ' ' + row["Modelo"] + ' ' + row["Anio"] + ' <br/><span class="text-muted">' + 'VIN: ' + row["VIN"] + '<span>'
                }
            },
            {
                "data": "CantidadDocumentosExpediente", "className": "text-center",
                "render": function (data, type, row) {
                    return '<span class="btn btn-block btn-sm btn-secondary cursor-zoom-in">' + row["CantidadDocumentosExpediente"] + ' <i class="fas fa-search pl-1"></i></span>'
                }
            },
            {
                "data": "EstadoExpedienteClassName", "className": "text-center",
                "render": function (data, type, row) {
                    return '<label class="badge badge-' + row["EstadoExpedienteClassName"] + ' p-1">' + row["EstadoExpediente"] + '</label>';
                }
            },
            {
                "data": "FechaCreacion", "className": "report-precios-de-mercado", "visible": false, "title": 'Fecha de ingreso',
                "render": function (value) {
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm a');
                }
            },
        ],
        buttons: [
            {
                extend: 'colvis',
                text: '<i class="mdi mdi-table-column-remove"></i> Columnas',
                columns: [1, 2, 3, 4, 5, 6]
            },
            {
                extend: 'excelHtml5',
                text: '<i class="far fa-file-excel"></i> Excel',
                title: 'Expedientes_Solicitudes_De_Credito_' + moment(),
                autoFilter: true,
                messageTop: 'Expedientes de solicitudes de crédito' + moment().format('YYYY/MM/DD'),
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6]
                }
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Imprimir',
                autoFilter: true,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6]
                }
            },
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false },
            { "width": "1%", "targets": [0, 1, 2, 3, 4, 5, 6] }
        ]
    });

    /* Filtrar cuando se seleccione una opción */
    $("input[type=radio][name=filtro-estado-expedientes]").change(function () {

        let filtro = this.value;
        dtListado.columns().search("").draw();

        switch (filtro) {
            case "0":
                dtListado.columns(6).search("").draw();
                break;
            case "1":
                dtListado.columns(6).search("pendiente").draw();
                break;
            case "2":
                dtListado.columns(6).search("estadoListo").draw();
                break;
            default:
                dtListado.columns(6).search("").draw();
        }
    });

    /* Búsqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {

        if (this.value != '')
            dtListado.columns(7).search('/' + this.value + '/').draw();
        else
            dtListado.columns(7).search('').draw();
    });

    /* Inicilizar filtro para rango de fechas */
    $("#min").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: true,
        changeYear: true,
    });

    $("#max").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: true,
        changeYear: true,
    });

    $("#min, #max").change(function () {
        filtroActual = 'rangoFechas';
        dtListado.draw();
    });

    /* Agregar filtros a los datatables */
    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {

        if (filtroActual == 'rangoFechas') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(data[7]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else {
            return true;
        }
    });

    /* Buscador de los datatables */
    $('#txtDatatableFilter').keyup(function () {

        dtListado.search($(this).val()).draw();
    });

    /* Cuando se haga click en el datatable principal de garantías */
    /* setear las etiquetas de ID solicitud, nombre de cliente, marca, modelo, etc, de todos los modales a través del className */
    $("#datatable-listado-expedientes tbody").on("click", "tr", function () {

        var row = dtListado.row(this).data();
        idSolicitud = row.IdSolicitud;
        idGarantia = row.IdGarantia;
        marcaGarantia = row.Marca;
        modeloGarantia = row.Modelo;
        anioGarantia = row.Anio;

        idSolicitudInstalacionGPS = row.IdAutoGPSInstalacion;
        VIN = row.VIN;
        nombreCliente = row["NombreCompleto"];
        identidad = row["Identidad"]

        $(".txtVIN").val(row.VIN);
        $(".txtMarca").val(row.Marca);
        $(".txtModelo").val(row.Modelo);
        $(".txtAnio").val(row.Anio);

        $(".lblVIN").text(row.VIN);
        $(".lblMarca").text(row.Marca);
        $(".lblModelo").text(row.Modelo);
        $(".lblAnio").text(row.Anio);
        $(".lblColor").text(row.Color);
        $("#lblValorMercadoGarantia").text(row.Moneda + ' ' + ConvertirADecimal(row.ValorMercadoGarantia));
        $("#lblValorPrima").text(row.Moneda + ' ' + ConvertirADecimal(row.ValorPrima));
        $("#lblValorAPrestar").text(row.Moneda + ' ' + ConvertirADecimal(row.ValorAPrestarGarantia));
        $("#lblValorAFinanciar").text(row.Moneda + ' ' + ConvertirADecimal(row.ValorAFinanciar));
        $(".lblNoSolicitudCredito").text(idSolicitud);
        $(".lblNombreCliente").text(nombreCliente);
        $("#lblEstadoRevisionFisica").removeClass('badge-success').removeClass('badge-warning').removeClass('badge-danger').addClass('badge-' + row.EstadoRevisionFisicaClassName).text(row.EstadoRevisionFisica);

        if (idSolicitudInstalacionGPS != 0) {

            $("#ddlUbicacionInstalacion_Actualizar").val(row.IDAgenciaInstalacion);
            $("#txtComentario_Actualizar").val(row.Comentario_Instalacion);
            $("#txtFechaInstalacion_Actualizar").val(new Date(parseInt(row.FechaInstalacion.substr(6, 19))).toJSON().slice(0, 19));
        }
    });
});

function RedirigirAccion(nombreFormulario, accion) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoExpedientes.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitud: idSolicitud, idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo redireccionar a " + accion);
        },
        success: function (data) {
            data.d != "-1" ? window.location = nombreFormulario + "?" + data.d : MensajeError("No se pudo redireccionar a" + accion);
        }
    });
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

/* Cuando de click en el boton de acciones de los datatables */
$('.table-responsive').on('show.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "inherit");
});

/* Cuando de click en el boton de acciones de los datatables */
$('.table-responsive').on('hide.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "auto");
});

jQuery("#date-range").datepicker({
    toggleActive: true
});

function ConvertirADecimal(nStr) {

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