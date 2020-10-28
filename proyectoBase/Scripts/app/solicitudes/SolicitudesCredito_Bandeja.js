var IconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">,</label></i>';
var IconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">.</label></i>';
var IconoRojo = '<i class="mdi mdi mdi-close-circle mdi-24px text-danger p-0"></i>';
var ProcesoPendiente = "/Date(-2208967200000)/";
var IDNT = "";
var IDSOL = 0;
var FiltroActual = "";

$(document).ready(function () {

    dtBandeja = $('#datatable-bandeja').DataTable({
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
        "ajax": {
            type: "POST",
            url: "SolicitudesCredito_Bandeja.aspx/CargarSolicitudes",
            contentType: 'application/json; charset=utf-8',
            data: function (dtParms) {
                return JSON.stringify({ dataCrypt: window.location.href, IDSOL: 0 });
            },
            "dataSrc": function (json) {
                var return_data = json.d;
                return return_data;
            }
        },
        "columns": [
            { "data": "fiIDSolicitud", "className": "text-center" },
            { "data": "fcAgencia" },
            {
                "data": "fcDescripcion",
                "render": function (value) {
                    return value.split(' ')[1]
                }
            },
            { "data": "fcIdentidadCliente" },
            {
                "data": "fcPrimerNombreCliente",
                "render": function (data, type, row) {
                    return row["fcPrimerNombreCliente"] + ' ' + row["fcSegundoNombreCliente"] + ' ' + row["fcPrimerApellidoCliente"] + ' ' + row["fcSegundoApellidoCliente"]
                }
            },
            {
                "data": "fdFechaCreacionSolicitud",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm:ss a');
                }
            },
            {
                "data": "fdEnIngresoInicio", "className": "text-center",
                "render": function (data, type, row) {
                    return row["fdEnIngresoInicio"] != ProcesoPendiente ? row["fdEnIngresoFin"] != ProcesoPendiente ? IconoExito : IconoPendiente : '';
                }
            },
            {
                "data": "fdEnTramiteInicio", "className": "text-center",
                "render": function (data, type, row) {
                    return row["fdEnTramiteInicio"] != ProcesoPendiente ? row["fdEnTramiteFin"] != ProcesoPendiente ? IconoExito : IconoPendiente : '';
                }
            },
            {
                "data": "fdEnAnalisisInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';
                    Resultado = row["fdEnAnalisisInicio"] != ProcesoPendiente ? row["fdEnAnalisisFin"] != ProcesoPendiente ? IconoExito : IconoPendiente : '';

                    if (row["fdEnAnalisisFin"] == ProcesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7) && row["fdEnAnalisisInicio"] != ProcesoPendiente) {
                        Resultado = row["fiEstadoSolicitud"] == 7 ? IconoExito : IconoRojo;
                    }

                    return Resultado;
                }
            },
            {
                "data": "fdEnvioARutaAnalista", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["fdEnvioARutaAnalista"] != ProcesoPendiente) {

                        Resultado = (row["fdEnCampoFin"] != ProcesoPendiente || row["fiEstadoDeCampo"] == 2) ? IconoExito : IconoPendiente;

                        if (row["fdEnCampoFin"] == ProcesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7)) {
                            Resultado = row["fiEstadoSolicitud"] == 7 ? IconoExito : IconoRojo;
                        }
                        else if (row["fiEstadoSolicitud"] == 5) {
                            Resultado = IconoRojo;
                        }
                    }

                    return Resultado;
                }
            },
            {
                "data": "fdCondicionadoInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["fdCondicionadoInicio"] != ProcesoPendiente) {

                        Resultado = row["fdCondificionadoFin"] != ProcesoPendiente ? IconoExito : IconoPendiente;

                        if (row["fdCondificionadoFin"] == ProcesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7)) {
                            Resultado = row["fiEstadoSolicitud"] == 7 ? IconoExito : IconoRojo;
                        }
                    }
                    return Resultado;
                }
            },
            {
                "data": "fdReprogramadoInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["fdReprogramadoInicio"] != ProcesoPendiente) {

                        Resultado = row["fdReprogramadoFin"] != ProcesoPendiente ? IconoExito : IconoPendiente;


                        if (row["fdReprogramadoFin"] == ProcesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7)) {
                            Resultado = row["fiEstadoSolicitud"] == 7 ? IconoExito : IconoRojo;
                        }
                    }
                    return Resultado;
                }
            },
            {
                "data": "PasoFinalInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["PasoFinalInicio"] != ProcesoPendiente) {

                        Resultado = row["PasoFinalFin"] != ProcesoPendiente ? IconoExito : IconoPendiente;


                        if (row["PasoFinalFin"] == ProcesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7)) {
                            Resultado = row["fiEstadoSolicitud"] == 7 ? IconoExito : IconoRojo;
                        }
                    }
                    return Resultado;
                }
            },
            {
                "data": "fiEstadoSolicitud", "className": "text-center",
                "render": function (data, type, row) {

                    var resolucionFinal = '<span class="btn-sm btn-block btn-warning">Pendiente</span>';
                    if (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7) {
                        resolucionFinal = row["fiEstadoSolicitud"] == 7 ? '<span class="btn btn-sm btn-block btn-success">Aprobada</span>' : '<span class="btn btn-sm btn-block btn-danger">Rechazada</span>';
                    }
                    return resolucionFinal;
                }
            },
            {
                "data": "ftTiempoTomaDecisionFinal", "visible": false, "title": 'Fecha resolución',
                "render": function (value) {

                    console.log(moment(value).locale('es').format('YYYY/MM/DD hh:mm:ss a'));
                    return value != ProcesoPendiente ? moment(value).locale('es').format('YYYY/MM/DD hh:mm:ss a') : '';
                }
            },
        ],
        buttons: [
            {
                extend: 'copy',
                text: 'Copiar'
            },
            {
                extend: 'excelHtml5',
                title: 'Solicitudes_de_credito_' + moment(),
                autoFilter: true,
                messageTop: 'Solicitudes de crédito ' + moment().format('YYYY/MM/DD'),
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 13,14]
                }
            },
            {
                extend: 'colvis',
                text: 'Columnas'
            }
        ],
        columnDefs: [
            { targets: [6, 7, 8, 9, 10, 11, 12], orderable: false },
            { "width": "1%", "targets": 0 }
        ]
    });

    dtBandeja.buttons().container().appendTo('#divContenedor_datatableButtons');

    /* Filtrar cuando se seleccione una opción */
    $("input[type=radio][name=filtros]").change(function () {
        var filtro = this.value;
        dtBandeja.columns([6, 7, 8, 9, 10, 11, 12, 13]).search("").draw();

        switch (filtro) {
            case "0":
                dtBandeja.columns([6, 7, 8, 9, 10, 11, 12, 13]).search("").draw();
                break;
            case "7":
                dtBandeja.columns(7).search(".").columns(13).search("Pendiente").draw();
                break;
            case "8":
                dtBandeja.columns(8).search(".").columns(9).search("_").columns(10).search("_").columns(13).search("Pendiente").draw();
                break;
            case "9":
                dtBandeja.columns(9).search(".").columns(13).search("Pendiente").draw();
                break;
            case "10":
                dtBandeja.columns(10).search(".").columns(13).search("Pendiente").draw();
                break;
            case "11":
                dtBandeja.columns(11).search(".").columns(13).search("Pendiente").draw();
                break;
            case "12":
                dtBandeja.columns(12).search(".").columns(13).search("Pendiente").draw();
                break;
            case "13":
                dtBandeja.columns(13).search("Pendiente").draw();
                break;
            case "14":
                dtBandeja.columns(13).search("Aprobada").draw();
                break;
            case "15":
                dtBandeja.columns(13).search("Rechazada").draw();
                break;
            default:
                dtBandeja.columns([6, 7, 8, 9, 10, 11, 12, 13]).search("").draw();
        }
    });

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            dtBandeja.columns(5).search('/' + this.value + '/').draw();
        }
        else {
            dtBandeja.columns(5).search('').draw();
        }
    });

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        dtBandeja.columns(5).search(this.value + '/').draw();
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
        dtBandeja.draw();
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
        dtBandeja.search($(this).val()).draw();
    });

    $("#datatable-bandeja tbody").on("click", "tr", function () {
        var row = dtBandeja.row(this).data(),
            IDAnalistaEncargado = row.fiIDUsuarioModifica,
            IDSolicitud = row.fiIDSolicitud;

        $("#lblCliente").text(row.fcPrimerNombreCliente + ' ' + row.fcSegundoNombreCliente + ' ' + row.fcPrimerApellidoCliente + ' ' + row.fcSegundoApellidoCliente + ' ');
        $("#lblIdentidadCliente").text(row.fcIdentidadCliente);

        if (IDAnalistaEncargado != 0 || 1 == 1) {
            $.ajax({
                type: "POST",
                url: "SolicitudesCredito_Bandeja.aspx/VerificarAnalista",
                data: JSON.stringify({ dataCrypt: window.location.href, ID: IDAnalistaEncargado }),
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
        url: "SolicitudesCredito_Bandeja.aspx/AbrirAnalisisSolicitud",
        data: JSON.stringify({ dataCrypt: window.location.href, IDSOL: IDSOL, Identidad: IDNT }),
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
        url: "SolicitudesCredito_Bandeja.aspx/EncriptarParametros",
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

    dtBandeja.columns(5).search('/' + mesActual + '/').draw();
}