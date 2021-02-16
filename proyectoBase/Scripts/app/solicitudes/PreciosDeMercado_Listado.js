/* Para realizar filtros en ambas listas*/
var filtroActual = '';
var tabActivo = 'tab_listado_precios_de_mercado';

$(document).ready(function () {

    dtListadoPreciosDeMercado = $('#datatable-listado-precios-de-mercado').DataTable({
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
            url: "PreciosDeMercado_Listado.aspx/CargarListaPreciosDeMercado",
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
                "data": "IdPrecioDeMercado", "className": "text-center",
                "render": function (data, type, row) {

                    return '<div class="dropdown mo-mb-2">' +
                        '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-bars"></i></button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<button type="button" class="dropdown-item" id="btnGuardar" data-toggle="modal" data-target="#modalGuardarGarantia"><i class="fas fa-plus"></i> Agregar</button>' +
                        '<button type="button" class="dropdown-item" id="btnDetalles" data-toggle="modal" data-target="#modalDetallesGarantia"><i class="fas fa-tasks"></i> Detalles</button>' +
                        '<button type="button" class="dropdown-item" id="btnActualizar" data-toggle="modal" data-target="#modalActualizarGarantia"><i class="far fa-edit"></i> Actualizar</button>' +
                        '</div>' +
                        '</div >';
                }
            },
            { "data": "Marca" },
            { "data": "Modelo" },
            { "data": "Version" },
            { "data": "Anio", "className": "text-center" },
            {
                "data": "PrecioDeMercado", "className": "text-right",
                render: function (value) {
                    return ConvertirADecimal(value);
                }
            },
            {
                "data": "FechaInicio", "className": "text-center",
                render: function (value) {
                    return value != '/Date(-2208967200000)/' ? moment(value).locale('es').format('YYYY/MM/DD') : '';
                }
            },
            {
                "data": "FechaFin", "className": "text-center",
                "render": function (data, type, row) {
                    return row["FechaFin"] != '/Date(-2208967200000)/' ? moment(row["FechaFin"]).locale('es').format('YYYY/MM/DD') : row["FechaInicio"] != '/Date(-2208967200000)/' ? '<span class="badge badge-info p-1">Precio actual</span>' : '';
                }
            },
            {
                "data": "UltimaDevaluacion", "className": "text-right",
                render: function (value) {
                    return ConvertirADecimal(value);
                }
            },
            {
                "data": "EstadoPrecioDeMercado", "className": "text-center",
                "render": function (data, type, row) {
                    return '<span class="badge badge-' + row["EstadoPrecioDeMercadoClassName"] + ' p-1">' + row["EstadoPrecioDeMercado"] + '</span>';
                }
            },
        ],
        buttons: [
            {
                extend: 'excelHtml5',
                text: '<i class="far fa-file-excel"></i> Exportar',
                title: 'Garantias_' + moment(),
                autoFilter: true,
                messageTop: 'Precios de mercado ' + moment().format('YYYY/MM/DD'),
                exportOptions: {
                    columns: '.report-data'
                }
            },
            {
                extend: 'colvis',
                text: '<i class="mdi mdi-table-column-remove"></i> Columnas',
                columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
            },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i> Imprimir',
                autoFilter: true,
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6, 7, 8, 9]
                }
            },
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false },
            { "width": "1%", "targets": [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }
        ]
    });

    /* Inicializar datable del listado de garantias sin solicitud */
    dtListado_Historial_Devaluacion = $('#datatable-listado-devaluacion').DataTable({
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
            url: "PreciosDeMercado_Listado.aspx/CargarHistorialDevaluacionPorModelo",
            contentType: 'application/json; charset=utf-8',
            data: function (dtParms) {
                return JSON.stringify({ dataCrypt: window.location.href });
            },
            "dataSrc": function (json) {
                var return_data = json.d;

                console.log(return_data);
                return return_data;
            }
        },
        "columns": [
            {
                "data": "IdModelo", "className": "text-center",
                "render": function (value) {

                    return '<div class="dropdown mo-mb-2">' +
                        '<button class="btn pt-1 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><i class="fa fa-bars"></i></button>' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        '<button type="button" class="dropdown-item" id="btnDetalles_SinSolicitud" data-toggle="modal" data-target="#modalDetallesGarantia_SinSolicitud"><i class="fas fa-tasks"></i> Detalles</button>' +
                        '<button type="button" class="dropdown-item" id="btnActualizar_SinSolicitud" data-toggle="modal" data-target="#modalActualizarGarantia_SinSolicitud"><i class="far fa-edit"></i> Actualizar</button>' +
                        '</div>' +
                        '</div >';
                }
            },
            { "data": "Marca" },
            { "data": "Modelo" },
            { "data": "Version" },
        ],
        columnDefs: [
            { targets: [0], orderable: false }
        ]
    });

    /* Buscador de los datatables */
    $('#txtDatatableFilter').keyup(function () {

        if (tabActivo == 'tab_listado_precios_de_mercado') {
            dtListadoPreciosDeMercado.search($(this).val()).draw();
        }
        else {
            dtListado_Historial_Devaluacion.search($(this).val()).draw();
        }
    });

    /* Cuando se haga click en el datatable principal de garantías */
    /* setear las etiquetas de ID solicitud, nombre de cliente, marca, modelo, etc, de todos los modales a través del className */
    $("#datatable-listado-precios-de-mercado tbody").on("click", "tr", function () {

        var row = dtListadoPreciosDeMercado.row(this).data();
    });

    /* Cuando se haga click en el datatable de garantías sin solicitud*/
    $("#datatable-listado-devaluacion tbody").on("click", "tr", function () {

        var row = dtListado_Historial_Devaluacion.row(this).data();
    });

    /* Cuando se cambie de TAB reajustar el tamaño de los datatables */
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $.fn.dataTable.tables({ visible: true, api: true }).columns.adjust();
    });

});

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

/* Definir el tab que está mirando el usuario */
$("#tab_listado_precios_de_mercado_link").on("click", function () {
    tabActivo = 'tab_listado_precios_de_mercado';
});

/* Definir el tab que está mirando el usuario */
$("#tab_listado_devaluacion_link").on("click", function () {
    tabActivo = 'tab_listado_devaluacion';
});

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


// Para mostrar detalles en el datatables de devaluaciones

$('#datatable-listado-devaluacion tbody').on('click', 'tr td:not(:first-child)', function () {

    let tr = $(this).closest("tr");
    let row = dtListado_Historial_Devaluacion.row(tr);

    if (row.child.isShown()) {

        // This row is already open - close it
        row.child.hide();
        tr.removeClass('row-selected');
    }
    else {

        let dataUser = row.data();
        tr.addClass('row-selected');

        let table = DrawDataTable_Anios(TipoTabla + dataUser.fiIDUsuario)

        row.child(table).show();
        row.child().addClass("row-selected");
        $.ajax({
            url: UrlPage + '/LoadDataAvancePorCartera',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ ID: dataUser.fiIDUsuario, CrypData: window.location.href, TipoTablaRecuperacion: TipoTabla }),
            dataType: "json",
            type: 'post',

            success: function (data) {

                $('#TableCartera-' + TipoTabla + dataUser.fiIDUsuario).DataTable({
                    dom: "rt<'row'<'col-sm-4'><'col-sm-8'>>",
                    data: data.d,
                    columns: RenderColumnsDataTable_Recuperacion(false)
                });

            },
            error: function (data) {
                console.log(data);
            },
        });
    }
});

function DrawDataTable_Anios(IdUsuario) {


    return `<div class="">
                <div>
                    <div class="table-responsive">
                        <table class="table-bordered display compact nowrap table-condensed table-hover bg-white" id="TableCartera-${IdUsuario}" style="width: 100%">
                            <thead>
                                <tr class="text-center">
                                    <th rowspan="2"></th>
                                    <th rowspan="2" >Nombre</th>
                                    <th colspan="2" >RECUPERACION</th>
                                    <th colspan="4" >CUOTA</th>
                                    <th colspan="2" >REQUERIDO</th>
                                    <th colspan="2" >PROYECCION</th>
                                </tr>
                                <tr>
                                    <th>HOY</th>
                                    <th>Acumulada</th>
                                    <th>Diaria</th>
                                    <th>Acumulada</th>
                                    <th>Global</th>
                                    <th>% Cobr</th>
                                    <th>Global</th>
                                    <th>% Efic</th>
                                    <th>Total</th>
                                    <th> %</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
`;
}

function RenderColumnsDataTable_Recuperacion(Hasbottom, IsAdmin, UrlPage) {

    let columnsData = [
        {
            "data": null,
            visible: Hasbottom,
            // className: "details-Agent",
            "render": function (data) {
                if (data.fiIDUsuario <= 0) return "";

                //var urlDetalle = IsAdmin ? `${UrlPage}/AbrirDetallesupervisor` : `${UrlPage}/AbrirDetalleAgente`;
                //var urlPageToSend = IsAdmin ? "DetailSupervisor.aspx?" : "DetailAgent.aspx?";

                //var  btnDetalles = `<button type="button" onclick="ViewAgentDetails(${data.fiIDUsuario},'${urlDetalle}', '${urlPageToSend}')" class="btn btn-sm btn-success mr-1"><i class="fa fa-user"></i></button>`;
                //btnDetalles += IsAdmin? '<button type="button" onclick="ViewSupervisorAgents(' + data.fiIDUsuario + ')" class="btn btn-sm btn-primary"><i class="fa fa-list"></i></button>':"";

                var urlDetalleAgente = `${UrlPage}/AbrirDetalleAgente`;
                var urlPageToSend = "DetailAgent.aspx?";

                var btnDetalles = IsAdmin ? '<button type="button" onclick="ViewSupervisorAgents(' + data.fiIDUsuario + ')" class="btn btn-sm btn-primary"><i class="fa fa-list"></i></button>' : `<button type="button" onclick="ViewAgentDetails(${data.fiIDUsuario},'${urlDetalleAgente}', '${urlPageToSend}')" class="btn btn-sm btn-success mr-1"><i class="fa fa-user"></i></button>`;

                return btnDetalles;
            }
        },
        { "data": "fcNombreCorto" },
        {
            data: null,
            className: 'text-right',
            render: function (data) {
                return ConvertirDecimal(data.fnRecuperado_Hoy);
            }
        },
        {
            data: null,
            className: 'text-right',
            render: function (data) {
                return ConvertirDecimal(data.fnRecuperado_Acumulado);
            }
        },

        {
            data: null,
            className: 'text-right',
            render: function (data) {
                return ConvertirDecimal(data.fnCuota_Hoy);
            }
        },
        {
            data: null,
            className: 'text-right',
            render: function (data) {
                return ConvertirDecimal(data.fnCuota_Acumulado);
            }
        },
        {
            data: null,
            className: 'text-right',
            render: function (data) {
                return ConvertirDecimal(data.fnCuota_Global);
            }
        },
        {
            data: null,
            className: 'text-right',
            render: function (data) {
                var className = data.fnPorcentajeCobranza < 80 ? "text-danger" : "text-success";
                return `<span class="${className} text-bold">${ConvertirDecimal(data.fnPorcentajeCobranza)} %</span>`;
            }
        },
        {
            data: null,
            className: 'text-right',
            render: function (data) {
                return ConvertirDecimal(data.fnRequerido_Global);
            }
        },


        {
            data: null,
            className: 'text-right',
            render: function (data) {
                var className = data.fnPorcentajeEficiencia < 80 ? "text-danger" : "text-success";
                return `<span class="${className} text-bold">${ConvertirDecimal(data.fnPorcentajeEficiencia)} %</span>`;

            }
        },


        {
            data: null,
            className: 'text-right',
            render: function (data) {
                return ConvertirDecimal(data.fnProyeccion);
            }
        },
        {
            data: null,
            className: 'text-right',
            render: function (data) {
                var className = data.fnPorcentajeProyeccion < 80 ? "text-danger" : "text-success";
                return `<span class="${className} text-bold">${ConvertirDecimal(data.fnPorcentajeProyeccion)} %</span>`;


            }
        },
    ];

    return columnsData;
}