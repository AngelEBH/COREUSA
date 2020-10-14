var IconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">,</label></i>';
var IconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">.</label></i>';
var IconoRojo = '<i class="mdi mdi mdi-close-circle mdi-24px text-danger p-0"></i>';
var ProcesoPendiente = "/Date(-2208967200000)/";
var IDNT = "";
var IDSOL = 0;
var FiltroActual = "";

$(document).ready(function () {

    dtListado = $('#datatable-listado').DataTable({
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
            "thousands": ",",
            buttons: {
                copyTitle: 'Copiado al portapapeles',
                copySuccess: {
                    _: '%d Lineas copiadas',
                    1: '1 linea copiada'
                }
            }
        },
        buttons: [
            {
                extend: 'copy',
                text: 'Copiar'
            },
            {
                extend: 'excelHtml5',
                title: 'Solicitudes_de_credito_' + moment(),
                autoFilter: true,
                messageTop: 'Solicitudes de crédito ' + moment().format('YYYY/MM/DD')//,
                //exportOptions: {
                //    columns: [0, 1, 2, 3, 4, 5, 13]
                //}
            },
            {
                extend: 'colvis',
                text: 'Ocultar columnas'
            }
        ],
        columnDefs: [
            //{ targets: [6,7,8,9,10,11,12], orderable: false },
            { "width": "1%", "targets": 0 }
        ]
    });

    /* Filtrar cuando se seleccione una opción */
    $("input[type=radio][name=filtros]").change(function () {
        //var filtro = this.value;
        //dtListado.columns().search("").draw();

        //switch (filtro) {
        //    case "0":
        //        dtListado.columns([6, 7, 8, 9, 10, 11, 12, 13]).search("").draw();
        //        break;
        //    case "7":
        //        dtListado.columns(7).search(".").columns(13).search("Pendiente").draw();
        //        break;
        //    case "8":
        //        dtListado.columns(8).search(".").columns(9).search("_").columns(10).search("_").columns(13).search("Pendiente").draw();
        //        break;
        //    case "9":
        //        dtListado.columns(9).search(".").columns(13).search("Pendiente").draw();
        //        break;
        //    case "10":
        //        dtListado.columns(10).search(".").columns(13).search("Pendiente").draw();
        //        break;
        //    case "11":
        //        dtListado.columns(11).search(".").columns(13).search("Pendiente").draw();
        //        break;
        //    case "12":
        //        dtListado.columns(12).search(".").columns(13).search("Pendiente").draw();
        //        break;
        //    case "13":
        //        dtListado.columns(13).search("Pendiente").draw();
        //        break;
        //    case "14":
        //        dtListado.columns(13).search("Aprobada").draw();
        //        break;
        //    case "15":
        //        dtListado.columns(13).search("Rechazada").draw();
        //        break;
        //    default:
        //        dtListado.columns([6, 7, 8, 9, 10, 11, 12, 13]).search("").draw();
        //}
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
        dtListado.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        if (FiltroActual == 'rangoFechas') {
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
        var row = dtListado.row(this).data(),
            IDAnalistaEncargado = row.fiIDUsuarioModifica,
            IDSolicitud = row.fiIDSolicitud;

        $("#lblCliente").text(row.fcPrimerNombreCliente + ' ' + row.fcSegundoNombreCliente + ' ' + row.fcPrimerApellidoCliente + ' ' + row.fcSegundoApellidoCliente + ' ');
        $("#lblIdentidadCliente").text(row.fcIdentidadCliente);        

        if (IDAnalistaEncargado != 0 || 1 == 1) {
            $.ajax({
                type: "POST",
                url: "SolicitudesCredito_ListadoGarantias.aspx/VerificarAnalista",
                data: JSON.stringify({ dataCrypt: window.location.href,  ID: IDAnalistaEncargado }),
                contentType: "application/json; charset=utf-8",
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError("No se pudo cargar la información, contacte al administrador");
                },
                success: function (data) {
                    //if (data.d == true) {
                        $("#modalAbrirSolicitud").modal({ backdrop: !1 });
                        IDSOL = IDSolicitud;
                        IDNT = row.fcIdentidadCliente;
                    //}
                }
            });
        }
        else {
            IDSOL = IDSolicitud;
            IDNT = row.fcIdentidadCliente;
            $("#modalAbrirSolicitud").modal({ backdrop: !1 });
        }
    });

    FiltrarSolicitudesMesActual();
});


$("#btnAbrirSolicitud").click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/AbrirAnalisisSolicitud",
        data: JSON.stringify({ dataCrypt: window.location.href,  IDSOL: IDSOL, Identidad: IDNT }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo cargar la solicitud, contacte al administrador");
        },
        success: function (data) {
            data.d != "-1" ? window.location = "SolicitudesCredito_Analisis.aspx?" + data.d : MensajeError("Esta solicitud ya está siendo analizada por otro usuario");
        }
    });
});

$("#btnDetallesSolicitud").click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ListadoGarantias.aspx/EncriptarParametros",
        data: JSON.stringify({ dataCrypt: window.location.href, IDSOL: IDSOL, Identidad: IDNT }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo cargar la solicitud, contacte al administrador");
        },
        success: function (data) {
            data.d != "-1" ? window.location = "SolicitudesCredito_Detalles.aspx?" + data.d : MensajeError("No se pudo al redireccionar a pantalla de detalles");
        },
    });
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

function FiltrarSolicitudesMesActual() {

    var mesActual = moment().format("MM");

    dtListado.columns(5)
        .search('/' + mesActual + '/')
        .draw();
}