const iconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">iconoExito</label></i>';
const iconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">iconoPendiente</label></i>';
const iconoRojo = '<i class="mdi mdi mdi-close-circle mdi-24px text-danger p-0"></i>';
const procesoPendiente = "/Date(-2208967200000)/";
var identidad = "";
var idSolicitud = 0;
var filtroActual = "";

// columnas: Valor del vehiculo, valor de la prima y el valor a financiar y la tasa de interes, seguro y GPS, la marca modelo año del carro

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
                        '<button type="button" class="dropdown-item" id="btnAnalisis" data-toggle="modal" data-target="#modalAbrirAnalisis"><i class="fas fa-tasks"></i> Abrir análisis</button>' +
                        '<button type="button" class="dropdown-item" id="btnDetalles" data-toggle="modal" data-target="#modalAbrirDetalles"><i class="fas fa-tasks"></i> Ver detalles</button>' +                        
                        '<button type="button" class="dropdown-item" id="btnImprimirDocumentacion" data-toggle="modal" data-target="#modalImprimirDocumentacion"><i class="far fa-file-alt"></i> Imprimir Documentos</button>' +
                        '<button type="button" class="dropdown-item" id="btnVerExpediente" onclick="MostrarExpedienteSolicitudGarantia(' + row["IdSolicitud"] + ',' + row["IdGarantia"] + ')"><i class="far fa-file-alt"></i> Ver expediente</button>' +
                        '</div>' +
                        '</div >';
                }
            },
            {
                "data": "IdSolicitud",
                "render": function (data, type, row) {
                    return row["IdSolicitud"] + ' <br><span class="text-muted">' + moment(row["FechaCreacionSolicitud"]).locale('es').format('YYYY/MM/DD hh:mm a') + '</span>';
                }
            },
            {
                "data": "Agencia", "className": "td-responsive",
                "render": function (data, type, row) {
                    return row["Producto"] + ' <br/><span class="text-muted">' + row["Agencia"] + ' | ' + row["UsuarioAsignado"] + '</span>'
                }
            },
            {
                "data": "NombreCliente", "className": "td-responsive",
                "render": function (data, type, row) {
                    return row["NombreCliente"] + ' <br/><span class="text-muted">' + row["IdentidadCliente"] + "</span>" + (row["IdCanal"] == 3 ? ' <span class="btn btn-sm btn-info pt-0 pb-0 m-0">canex</span>' : '')
                }
            },
            {
                "data": "VIN", "className": "td-responsive",
                "render": function (data, type, row) {
                    return row["RequiereGarantia"] == 0 ? '<span class="text-muted">No aplica</span>' : row["Marca"] + ' ' + row["Modelo"] + ' ' + row["Anio"] + ' <br/><span class="text-muted">' + 'VIN: ' + row["VIN"] + '<span>'
                }
            },
            {
                "data": "EnIngresoInicio", "className": "text-center",
                "render": function (data, type, row) {
                    return row["EnIngresoInicio"] != procesoPendiente ? row["EnIngresoFin"] != procesoPendiente ? iconoExito : iconoPendiente : '';
                }
            },
            {
                "data": "EnTramiteInicio", "className": "text-center",
                "render": function (data, type, row) {
                    return row["EnTramiteInicio"] != procesoPendiente ? row["EnTramiteFin"] != procesoPendiente ? iconoExito : iconoPendiente : '';
                }
            },
            {
                "data": "EnAnalisisInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';
                    Resultado = row["EnAnalisisInicio"] != procesoPendiente ? row["EnAnalisisFin"] != procesoPendiente ? iconoExito : iconoPendiente : '';

                    if (row["EnAnalisisFin"] == procesoPendiente && (row["IdEstadoSolicitud"] == 4 || row["IdEstadoSolicitud"] == 5 || row["IdEstadoSolicitud"] == 7) && row["EnAnalisisInicio"] != procesoPendiente) {
                        Resultado = row["IdEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
                    }

                    return Resultado;
                }
            },
            {
                "data": "EnvioARutaAnalista", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["EnvioARutaAnalista"] != procesoPendiente) {

                        Resultado = (row["EnCampoInicio"] != procesoPendiente || row["IdEstadoDeCampo"] == 2) ? iconoExito : iconoPendiente;

                        if (row["EnCampoInicio"] == procesoPendiente && (row["IdEstadoSolicitud"] == 4 || row["IdEstadoSolicitud"] == 5 || row["IdEstadoSolicitud"] == 7)) {
                            Resultado = row["IdEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
                        }
                        else if (row["IdEstadoSolicitud"] == 5) {
                            Resultado = iconoRojo;
                        }
                    }

                    return Resultado;
                }
            },
            {
                "data": "CondicionadoInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["CondicionadoInicio"] != procesoPendiente) {

                        Resultado = row["CondificionadoFin"] != procesoPendiente ? iconoExito : iconoPendiente;

                        if (row["CondificionadoFin"] == procesoPendiente && (row["IdEstadoSolicitud"] == 4 || row["IdEstadoSolicitud"] == 5 || row["IdEstadoSolicitud"] == 7)) {
                            Resultado = row["IdEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
                        }
                    }
                    return Resultado;
                }
            },
            {
                "data": "ReprogramadoInicio", "className": "text-center",
                "render": function (data, type, row) {

                    var Resultado = '';

                    if (row["ReprogramadoInicio"] != procesoPendiente) {

                        Resultado = row["ReprogramadoFin"] != procesoPendiente ? iconoExito : iconoPendiente;


                        if (row["ReprogramadoFin"] == procesoPendiente && (row["IdEstadoSolicitud"] == 4 || row["IdEstadoSolicitud"] == 5 || row["IdEstadoSolicitud"] == 7)) {
                            Resultado = row["IdEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
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


                        if (row["PasoFinalFin"] == procesoPendiente && (row["IdEstadoSolicitud"] == 4 || row["IdEstadoSolicitud"] == 5 || row["IdEstadoSolicitud"] == 7)) {
                            Resultado = row["IdEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
                        }
                    }
                    return Resultado;
                }
            },
            {
                "data": "IdEstadoSolicitud", "className": "text-center",
                "render": function (data, type, row) {

                    var resolucionFinal = '<span class="btn-sm btn-block btn-warning">Pendiente</span>';
                    if (row["IdEstadoSolicitud"] == 4 || row["IdEstadoSolicitud"] == 5 || row["IdEstadoSolicitud"] == 7) {
                        resolucionFinal = row["IdEstadoSolicitud"] == 7 ? '<span class="btn btn-sm btn-block btn-success">Aprobada</span>' : '<span class="btn btn-sm btn-block btn-danger">Rechazada</span>';
                    }
                    return resolucionFinal;
                }
            },
            {
                "data": "FechaResolucion", "visible": false, "title": 'Fecha resolución',
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
            { targets: [0, 5, 6, 7, 8, 9, 10, 11, 12], orderable: false },
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
                dtBandeja.columns(7).search("iconoPendiente").columns(13).search("Pendiente").draw();
                break;
            case "8":
                dtBandeja.columns(8).search("iconoPendiente").columns(9).search("_").columns(10).search("_").columns(13).search("Pendiente").draw();
                break;
            case "9":
                dtBandeja.columns(9).search("iconoPendiente").columns(13).search("Pendiente").draw();
                break;
            case "10":
                dtBandeja.columns(10).search("iconoPendiente").columns(13).search("Pendiente").draw();
                break;
            case "11":
                dtBandeja.columns(11).search("iconoPendiente").columns(13).search("Pendiente").draw();
                break;
            case "12":
                dtBandeja.columns(12).search("iconoPendiente").columns(13).search("Pendiente").draw();
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
        idSolicitud = row.IdSoliciud;

        $("#lblCliente").text(row.PrimerNombreCliente + ' ' + row.SegundoNombreCliente + ' ' + row.PrimerApellidoCliente + ' ' + row.SegundoApellidoCliente + ' ');
        $("#lblIdentidadClienteCliente").text(row.IdentidadClienteCliente);

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
                    $("#modalAbrirSolicitud").modal({ backdrop: false });
                    idSolicitud = idSolicitud;
                    identidad = row.IdentidadClienteCliente;
                    //}
                }
            });
        }
        else {
            idSolicitud = idSolicitud;
            identidad = row.IdentidadClienteCliente;
            $("#modalAbrirSolicitud").modal({ backdrop: false });
        }
    });

    //FiltrarSolicitudesMesActual();
});


$("#btnAbrirAnalisis").click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/AbrirAnalisis",
        data: JSON.stringify({ idSolicitud: idSolicitud, IdentidadCliente: identidad, dataCrypt: window.location.href }),
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