const iconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">iconoExito</label></i>';
const iconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">iconoPendiente</label></i>';
const iconoRojo = '<i class="mdi mdi mdi-close-circle mdi-24px text-danger p-0"></i>';
const procesoPendiente = "/Date(-2208967200000)/";

var idCliente = 0;
var idGarantia = 0;
var idSolicitud = 0;
var idExpediente = 0;
var identidadCliente = "";
var filtroActual = "";

$(document).ready(function () {

    /* Inicializar datatable de la bandeja de solicitudes de credito */
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
                        '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" aria-label="Opciones"><i class="fa fa-bars"></i></button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        (row["PermitirAbrirAnalisis"] == true ? '<button type="button" class="dropdown-item" id="btnAbrirAnalisis" onclick="RedirigirAccion(' + "'SolicitudesCredito_Analisis.aspx'" + ',' + "'análisis de la solicitud'" + ')" aria-label="Analisis"><i class="far fa-edit"></i> Abrir análisis</button>' : '') +
                        '<button type="button" class="dropdown-item" id="btnAbrirDetalles" onclick="RedirigirAccion(' + "'SolicitudesCredito_Detalles.aspx'" + ',' + "'detalles de la solicitud'" + ')" aria-label="Detalles"><i class="far fa-file-alt"></i> Ver detalles</button>' +
                        (row["IdEstadoSolicitud"] == 7 ? '<button type="button" class="dropdown-item" id="btnImprimirDocumentacion" onclick="RedirigirAccion(' + "'SolicitudesCredito_ImprimirDocumentacion.aspx'" + ',' + "'imprimir documentación'" + ')"><i class="far fa-file-pdf"></i> Imprimir Documentos</button>' : '') +
                        '<button type="button" class="dropdown-item" id="btnExpedienteSolicitud" onclick="MostrarExpedienteSolicitudGarantia(' + row["IdSolicitud"] + ',' + row["IdGarantia"] + ')" aria-label="Expediente de la solicitud"><i class="far fa-folder"></i> Expediente de la solicitud</button>' +
                        (row["IdEstadoSolicitud"] == 7 ?
                            '<button type="button" class="dropdown-item" onclick="AbrirExpedienteFinal(' + row["IdExpediente"] + ')" aria-label="Abrir expediente final"><i class="far fa-folder"></i> ' + (row["IdExpediente"] != 0 ? 'Abrir expediente del préstamo' : 'Crear expediente del préstamo') + '</button>'
                            : ''
                        ) +
                        '</div>' +
                        '</div >';
                }
            },
            {
                "data": "IdSolicitud", "className": "td-responsive",
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
                    return row["RequiereGarantia"] == 0 ? '<span class="text-muted">No aplica</span>' : '<label class="cursor-zoom-in m-0" onclick="MostrarDocumentosGarantia(' + row["IdGarantia"] + ')">' + row["Marca"] + ' ' + row["Modelo"] + ' ' + row["Anio"] + ' <br/><span class="text-muted">' + 'VIN: ' + row["VIN"] + '</span></label>'
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

                    let Resultado = '';
                    Resultado = row["EnAnalisisInicio"] != procesoPendiente ? row["EnAnalisisFin"] != procesoPendiente ? iconoExito : iconoPendiente : '';

                    if (row["EnAnalisisInicio"] != procesoPendiente && row["EnAnalisisFin"] == procesoPendiente && (row["IdEstadoSolicitud"] == 4 || row["IdEstadoSolicitud"] == 5 || row["IdEstadoSolicitud"] == 7)) {
                        Resultado = row["IdEstadoSolicitud"] == 7 ? iconoExito : iconoRojo;
                    }
                    return Resultado;
                }
            },
            {
                "data": "EnvioARutaAnalista", "className": "text-center",
                "render": function (data, type, row) {

                    let Resultado = '';
                    if (row["EnvioARutaAnalista"] != procesoPendiente) {

                        Resultado = (row["EnCampoInicio"] != procesoPendiente) ? (row["IdEstadoDeCampo"] == 2 ? iconoExito : iconoPendiente) : '';

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

                    let Resultado = '';
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

                    let Resultado = '';
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

                    let Resultado = '';
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

                    let resolucionFinal = '<span class="btn-sm btn-block btn-warning">Pendiente</span>';
                    if (row["IdEstadoSolicitud"] == 4 || row["IdEstadoSolicitud"] == 5 || row["IdEstadoSolicitud"] == 7) {
                        resolucionFinal = row["IdEstadoSolicitud"] == 7 ? '<span class="btn btn-sm btn-block btn-success">Aprobada</span>' : '<span class="btn btn-sm btn-block btn-danger">Rechazada</span>';
                    }
                    return resolucionFinal;
                }
            },
            {
                "data": "EnIngresoFin", "visible": false, "title": 'Fecha de ingreso',
                "render": function (value) {
                    return value != procesoPendiente ? moment(value).locale('es').format('YYYY/MM/DD hh:mm A') : '';
                }
            },
            {
                "data": "FechaResolucion", "visible": false, "title": 'Fecha de resolución',
                "render": function (value) {
                    return value != procesoPendiente ? moment(value).locale('es').format('YYYY/MM/DD hh:mm A') : '';
                }
            },
            {
                "data": "ValorGarantia", "visible": false, "title": 'Valor de la garantía',
                "render": function (value) {
                    return ConvertirADecimal(value);
                }
            },
            {
                "data": "ValorPrima", "visible": false, "title": 'Valor de la prima',
                "render": function (value) {
                    return ConvertirADecimal(value);
                }
            },
            {
                "data": "ValorAPrestarGarantia", "visible": false, "title": 'Valor a prestar',
                "render": function (value) {
                    return ConvertirADecimal(value);
                }
            },
            {
                "data": "ValorAFinanciar", "visible": false, "title": 'Valor a financiar',
                "render": function (value) {
                    return ConvertirADecimal(value);
                }
            },
            { "data": "Plazo", "visible": false, "title": 'Plazo' },
            {
                "data": "TasaInteresAnual", "visible": false, "title": 'Tasa de interés anual',
                "render": function (value) {
                    return ConvertirADecimal(value);
                }
            },
            {
                "data": "TasaInteresMensual", "visible": false, "title": 'Tasa de interés mensual',
                "render": function (value) {
                    return ConvertirADecimal(value);
                }
            },
            {
                "data": "CuotaSeguro", "visible": false, "title": 'Cuota seguro',
                "render": function (value) {
                    return ConvertirADecimal(value);
                }
            },
            {
                "data": "CuotaGPS", "visible": false, "title": 'Cuota GPS',
                "render": function (value) {
                    return ConvertirADecimal(value);
                }
            },

        ],
        buttons: [
            {
                extend: 'excelHtml5',
                text: '<i class="far fa-file-excel"></i> Exportar',
                title: 'Solicitudes_de_credito_' + moment(),
                autoFilter: true,
                messageTop: 'Solicitudes de crédito ' + moment().format('YYYY/MM/DD'),
                exportOptions: {
                    columns: [1, 2, 3, 4, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23]
                }
            },
            {
                extend: 'colvis',
                text: '<i class="mdi mdi-table-column-remove"></i> Columnas',
                columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12] // columnas que pueden ocultarse y mostrarse, por indice para mejorar el tiempo de carga, por className es mas intuitivo toggle-visible-active
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Imprimir',
                autoFilter: true,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 12]
                }
            },
        ],
        columnDefs: [
            { targets: [0, 5, 6, 7, 8, 9, 10, 11], orderable: false },
            { "width": "1%", "targets": 0 }
        ]
    });

    dtBandeja.buttons().container().appendTo('#divContenedor_datatableButtons');

    /* filtrar por estado de las solicitudes */
    $("input[type=radio][name=filtros]").change(function () {

        let filtro = this.value;
        dtBandeja.columns([5, 6, 7, 8, 9, 10, 11, 12]).search("").draw();

        switch (filtro) {

            case "0":
                dtBandeja.columns([5, 6, 7, 8, 9, 10, 11, 12]).search("").draw();
                break;
            case "6": // En Recepcion
                dtBandeja.columns(6).search("iconoPendiente").columns(12).search("Pendiente").draw();
                break;
            case "7": // En análisis
                dtBandeja.columns(7).search("iconoPendiente").columns(12).search("Pendiente").draw();
                break;
            case "8": // En campo
                dtBandeja.columns(8).search("iconoPendiente").columns(12).search("Pendiente").draw();
                break;
            case "9": // Condicionada
                dtBandeja.columns(9).search("iconoPendiente").columns(12).search("Pendiente").draw();
                break;
            case "10": // Reprogramadas
                dtBandeja.columns(10).search("iconoPendiente").columns(12).search("Pendiente").draw();
                break;
            case "11": // Validación
                dtBandeja.columns(11).search("iconoPendiente").columns(12).search("Pendiente").draw();
                break;
            case "12": // Pendientes
                dtBandeja.columns(12).search("Pendiente").draw();
                break;
            case "13": // Aprobadas
                dtBandeja.columns(12).search("Aprobada").draw();
                break;
            case "14": // Rechazadas
                dtBandeja.columns(12).search("Rechazada").draw();
                break;
            default:
                dtBandeja.columns([5, 6, 7, 8, 9, 10, 11, 12]).search("").draw();
        }
    });

    /* busqueda por mes de ingreso */
    $('#ddlMesIngreso').on('change', function () {

        let filtroMes = this.value != '' ? '/' + this.value + '/' : '';
        dtBandeja.columns(13).search(filtroMes).draw();
    });

    /* busqueda por año de ingreso */
    $('#ddlAnioIngreso').on('change', function () {
        dtBandeja.columns(13).search(this.value + '/').draw();
    });

    $("#fecha-minima").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#fecha-maxima").datepicker({
        onSelect: function () {
            filtroActual = 'rangoFechas';
        },
        changeMonth: !0,
        changeYear: !0,
    });

    $("#fecha-minima, #fecha-maxima").change(function () {
        filtroActual = 'rangoFechas';
        dtBandeja.draw();
    });

    /* agregar filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {

        if (filtroActual == 'rangoFechas') {

            let Desde = $("#fecha-minima").datepicker("getDate"),
                Hasta = $("#fecha-maxima").datepicker("getDate"),
                FechaIngreso = new Date(a[13]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else return true;
    });

    $("#ddlAnioIngreso").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    $('#txtDatatableFilter').keyup(function () {
        dtBandeja.search($(this).val()).draw();
    });

    $("#datatable-bandeja tbody").on("click", "tr", function () {

        let row = dtBandeja.row(this).data();
        idCliente = row.IdCliente;
        idGarantia = row.IdGarantia;
        idSolicitud = row.IdSolicitud;
        idExpediente = row.IdExpediente;
        identidadCliente = row.IdentidadCliente;

        $(".lblNoSolicitudCredito").text(row.IdSolicitud)
        $(".lblNombreCliente").text(row.NombreCliente);
        $(".lblIdentidadCliente").text(row.IdentidadCliente);

        $(".lblProducto").text(row.Producto);
        $(".lblAgenciaYVendedorAsignado").text(row.Agencia + ' / ' + row.UsuarioAsignado);

        $(".lblMarca").text(row.Marca);
        $(".lblModelo").text(row.Modelo);
        $(".lblAnio").text(row.Anio);
        $(".lblVIN").text(row.VIN);

        $("#lblValorMercadoGarantia").text(row.Moneda + ' ' + ConvertirADecimal(row.ValorGarantia));
        $("#lblValorPrima").text(row.Moneda + ' ' + ConvertirADecimal(row.ValorPrima));
        $("#lblValorAPrestar").text(row.Moneda + ' ' + ConvertirADecimal(row.ValorAPrestarGarantia));
        $("#lblValorAFinanciar").text(row.Moneda + ' ' + ConvertirADecimal(row.ValorAFinanciar));

    });

    FiltrarSolicitudesMesActual();
});

/* Mostrar documentos de la garantía */
function MostrarDocumentosGarantia(idGarantia) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/CargarDocumentosGarantia",
        data: JSON.stringify({ idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los documentos de la garantía, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                var documentos = data.d;
                var divGaleriaGarantia = $("#divGaleriaGarantia").empty();
                var templateDocumentos = '';

                if (documentos != null) {

                    if (documentos.length > 0) {

                        for (var i = 0; i < documentos.length; i++) {

                            templateDocumentos += '<img alt="' + documentos[i].DescripcionTipoDocumento + '" src="' + documentos[i].URLArchivo + '" data-image="' + documentos[i].URLArchivo + '" data-description="' + documentos[i].DescripcionTipoDocumento + '"/>';
                        }
                    }
                }

                var imgNoHayFotografiasDisponibles = '<img alt="No hay fotografías disponibles" src="/Imagenes/Imagen_no_disponible.png" data-image="/Imagenes/Imagen_no_disponible.png" data-description="No hay fotografías disponibles"/>';
                templateDocumentos = templateDocumentos == '' ? imgNoHayFotografiasDisponibles : templateDocumentos;

                divGaleriaGarantia.append(templateDocumentos);

                $("#divGaleriaGarantia").unitegallery();

                $("#modalDocumentosDeLaGarantia").modal();
            }
            else
                MensajeError('No se pudieron cargar los documentos de la garantía, contacte al administrador.');
        }
    });
}

/* Expediente de la solicitud y garantia */
function MostrarExpedienteSolicitudGarantia(idSolicitud, idGarantia) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/CargarExpedienteSolicitudGarantia",
        data: JSON.stringify({ idSolicitud: idSolicitud, idGarantia: idGarantia, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los documentos de la garantía, contacte al administrador.');
        },
        success: function (data) {

            if (data.d != null) {

                /* Inicializar datatables de documentos */
                $('#tblExpedienteSolicitudGarantia').DataTable({
                    "destroy": true,
                    "pageLength": 100,
                    "aaSorting": [],
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
                    dom: 'f',
                    data: data.d,
                    "columns": [
                        { "data": "DescripcionTipoDocumento", "className": "font-12" },
                        {
                            "data": "URLArchivo", "className": "text-center",
                            "render": function (data, type, row) {
                                return '<button class="btn btn-sm btn-secondary" data-url="' + row["URLArchivo"] + '" data-descripcion="' + row["DescripcionTipoDocumento"] + '" onclick="MostrarVistaPrevia(this)" type="button" aria-label="Vista previa del documento"><i class="fas fa-search"></i></button>'
                            }
                        },
                    ],
                    columnDefs: [
                        { targets: 'no-sort', orderable: false },
                    ]
                });

                $("#divPrevisualizacionDocumento").empty();

                $("#modalDocumentosGarantiaSolicitud").modal();
            }
            else
                MensajeError('No se pudo cargar el expediente, contacte al administrador.');
        }
    });
}

function MostrarVistaPrevia(btnVistaPrevia) {

    let urlImagen = $(btnVistaPrevia).data('url');
    let descripcion = $(btnVistaPrevia).data('descripcion');
    let imgTemplate = '<img alt="' + descripcion + '" src="' + urlImagen + '" data-image="' + urlImagen + '" data-description="' + descripcion + '"/>';

    $("#divPrevisualizacionDocumento").empty().append(imgTemplate).unitegallery();
}

/* Expediente final */
function AbrirExpedienteFinal(idExpediente) {

    if (idExpediente != 0)
        RedirigirAccion('SolicitudesCredito_Expedientes.aspx', 'expediente del préstamo');
    else
        $("#modalCrearExpedientePrestamo").modal();
}

$("#btnCrearExpediente_Confirmar").click(function () {

    $("#btnCrearExpediente_Confirmar").prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/CrearExpedientePrestamo",
        data: JSON.stringify({ idSolicitud: idSolicitud, comentarios: $("#txtComentariosExpediente").val(), dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Ocurrió un error al crear el expediente del préstamo, contacte al administrador');
        },
        success: function (data) {

            if (data.d != "-1") {
                idExpediente = data.d;
                RedirigirAccion('SolicitudesCredito_Expedientes.aspx', 'expediente del préstamo');
            }
            else
                MensajeError('Ocurrió un error al crear el expediente del préstamo, contacte al administrador');
        },
        complete: function () {
            $("#btnCrearExpediente_Confirmar").prop('disabled', false);
        }
    });

});

/* Funciones utilitarias */
function RedirigirAccion(nombreFormulario, accion) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitud: idSolicitud, idCliente: idCliente, idGarantia: idGarantia, identidad: identidadCliente, idExpediente: idExpediente, dataCrypt: window.location.href }),
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

function FiltrarSolicitudesMesActual() {
    $("#ddlMesIngreso").val(moment().format("MM"));
    dtBandeja.columns(13).search('/' + moment().format("MM") + '/').draw();
}

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

/* Cuando de click en el boton de acciones de los datatables */
$('.table-responsive').on('show.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "inherit");
});

/* Cuando de click en el boton de acciones de los datatables */
$('.table-responsive').on('hide.bs.dropdown', function () {
    $('.table-responsive').css("overflow", "auto");
});