const iconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">,</label></i>';
const iconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">.</label></i>';
const iconoRojo = '<i class="mdi mdi mdi-close-circle mdi-24px text-danger p-0"></i>';
const procesoPendiente = "/Date(-2208967200000)/";
var identidad = "";
var idSolicitud = 0;
var filtroActual = "";

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
                return JSON.stringify({ dataCrypt: window.location.href, idSolicitud: 0 });
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
                    return moment(value).locale('es').format('YYYY/MM/DD hh:mm A');
                }
            },
            {
                "data": "fdEnIngresoInicio", "className": "text-center",
                "render": function (data, type, row) {
                    return row["fdEnIngresoInicio"] != procesoPendiente ? row["fdEnIngresoFin"] != procesoPendiente ? iconoExito : iconoPendiente : '';
                }
            },
            {
                "data": "fdEnTramiteInicio", "className": "text-center",
                "render": function (data, type, row) {
                    return row["fdEnTramiteInicio"] != procesoPendiente ? row["fdEnTramiteFin"] != procesoPendiente ? iconoExito : iconoPendiente : '';
                }
            },
            {
                "data": "fdEnAnalisisInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';
                    Resultado = row["fdEnAnalisisInicio"] != procesoPendiente ? row["fdEnAnalisisFin"] != procesoPendiente ? iconoExito : iconoPendiente : '';

                    if (row["fdEnAnalisisFin"] == procesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7) && row["fdEnAnalisisInicio"] != procesoPendiente) {
                        Resultado = row["fiEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
                    }

                    return Resultado;
                }
            },
            {
                "data": "fdEnvioARutaAnalista", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["fdEnvioARutaAnalista"] != procesoPendiente) {

                        Resultado = (row["fdEnCampoFin"] != procesoPendiente || row["fiEstadoDeCampo"] == 2) ? iconoExito : iconoPendiente;

                        if (row["fdEnCampoFin"] == procesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7)) {
                            Resultado = row["fiEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
                        }
                        else if (row["fiEstadoSolicitud"] == 5) {
                            Resultado = iconoRojo;
                        }
                    }

                    return Resultado;
                }
            },
            {
                "data": "fdCondicionadoInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["fdCondicionadoInicio"] != procesoPendiente) {

                        Resultado = row["fdCondificionadoFin"] != procesoPendiente ? iconoExito : iconoPendiente;

                        if (row["fdCondificionadoFin"] == procesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7)) {
                            Resultado = row["fiEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
                        }
                    }
                    return Resultado;
                }
            },
            {
                "data": "fdReprogramadoInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["fdReprogramadoInicio"] != procesoPendiente) {

                        Resultado = row["fdReprogramadoFin"] != procesoPendiente ? iconoExito : iconoPendiente;


                        if (row["fdReprogramadoFin"] == procesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7)) {
                            Resultado = row["fiEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
                        }
                    }
                    return Resultado;
                }
            },
            {
                "data": "PasoFinalInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["PasoFinalInicio"] != procesoPendiente) {

                        Resultado = row["PasoFinalFin"] != procesoPendiente ? iconoExito : iconoPendiente;


                        if (row["PasoFinalFin"] == procesoPendiente && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7)) {
                            Resultado = row["fiEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
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

                    return value != procesoPendiente ? moment(value).locale('es').format('YYYY/MM/DD hh:mm A') : '';
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
                    columns: [0, 1, 2, 3, 4, 5, 13, 14]
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

        if (this.value != '')
            dtBandeja.columns(5).search('/' + this.value + '/').draw();
        else
            dtBandeja.columns(5).search('').draw();
    });

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        dtBandeja.columns(5).search(this.value + '/').draw();
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
        dtBandeja.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {

        if (filtroActual == 'rangoFechas') {

            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[5]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else return true;
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

        var row = dtBandeja.row(this).data();
        let idAnalistaSolicitud = row.fiIDUsuarioModifica;
        idSolicitud = row.fiIDSolicitud;

        $("#lblCliente").text(row.fcPrimerNombreCliente + ' ' + row.fcSegundoNombreCliente + ' ' + row.fcPrimerApellidoCliente + ' ' + row.fcSegundoApellidoCliente + ' ');
        $("#lblIdentidadCliente").text(row.fcIdentidadCliente);

        if (idAnalistaSolicitud != 0 || 1 == 1) {

            $.ajax({
                type: "POST",
                url: "SolicitudesCredito_Bandeja.aspx/VerificarAnalista",
                data: JSON.stringify({ dataCrypt: window.location.href, idAnalista: idAnalistaSolicitud }),
                contentType: "application/json; charset=utf-8",
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError("No se pudo cargar la información, contacte al administrador");
                },
                success: function (data) {

                    //if (data.d == true) {
                    $("#modalAbrirSolicitud").modal({ backdrop: !1 });
                    idSolicitud = idSolicitud;
                    identidad = row.fcIdentidadCliente;
                    //}
                }
            });
        }
        else {
            idSolicitud = idSolicitud;
            identidad = row.fcIdentidadCliente;
            $("#modalAbrirSolicitud").modal({ backdrop: !1 });
        }
    });

    FiltrarSolicitudesMesActual();
});


$("#btnAbrirAnalisis").click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/AbrirAnalisis",
        data: JSON.stringify({ idSolicitud: idSolicitud, Identidad: identidad, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo cargar la solicitud, contacte al administrador");
        },
        success: function (data) {
            data.d != "-1" ? window.location = "SolicitudesCredito_Analisis.aspx?" + data.d : MensajeError("Esta solicitud ya está siendo analizada por otro usuario");
        }
    });
});

$("#btnAbrirDetalles").click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitud: idSolicitud, identidad: identidad, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo cargar la solicitud, contacte al administrador");
        },
        success: function (data) {
            data.d != "-1" ? window.location = "SolicitudesCredito_Detalles.aspx?" + data.d : MensajeError("No se pudo al redireccionar a pantalla de detalles");
        }
    });
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function FiltrarSolicitudesMesActual() {

    dtBandeja.columns(5).search('/' + moment().format("MM") + '/').draw();
}