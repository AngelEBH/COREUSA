/* Calcular capacidad de pago dependiendo */
function calcularCapacidadPago(tipoPrestamo, ObligacionesPrecalificado, IngresosReales) {

    var capacidadPago = 0;

    if (tipoPrestamo == '101')
    {
        capacidadPago = ObligacionesPrecalificado == 0 ? IngresosReales * 0.13 : (IngresosReales - ObligacionesPrecalificado) * 0.13;
    }
    else if (tipoPrestamo == '201')
    {
        capacidadPago = ObligacionesPrecalificado == 0 ? IngresosReales * 0.30 : (IngresosReales - ObligacionesPrecalificado) * 0.30;
    }
    else if (tipoPrestamo == '202')
    {
        capacidadPago = ObligacionesPrecalificado == 0 ? IngresosReales * 0.40 : (IngresosReales - ObligacionesPrecalificado) * 0.40;
    }
    return capacidadPago.toFixed(2);
}



/* Código de respaldo para mostrar sub tabla de detalles en un datatable */

$('#datatable-listado-devaluacion tbody').on('click', 'tr td:last-child', function () {

    let tr = $(this).closest("tr");
    let row = dtListado_Historial_Devaluacion.row(tr); // obtener fila clickada por el usuario
    let dataRow = row.data(); // obtener data de la fila

    console.log(dataRow);

    if (row.child.isShown()) {

        // This row is already open - close it
        row.child.hide();
        tr.removeClass('row-selected');
    }
    else if (dataRow != undefined) {

        if (dtListado_Historial_Devaluacion.row('.shown').length) {
            $('.details-control', dtListado_Historial_Devaluacion.row('.shown').node()).click();
        }

        $(".lblMarca").text(dataRow.Marca);
        $(".lblModelo").text(dataRow.Modelo);
        $(".lblVersion").text(dataRow.Version);

        tr.addClass('row-selected');

        let idTableDetailsAniosDisponibles = 'tblAniosDisponibles_' + dataRow.IdMarca + dataRow.IdModelo + dataRow.Version;

        let table = DrawDataTable_AniosDisponibles(idTableDetailsAniosDisponibles) // dibujar tabla de historial

        row.child(table).show();
        row.child().addClass("row-selected");

        tblAnios = $('#' + idTableDetailsAniosDisponibles).DataTable({
            dom: "rt<'row'<'col-sm-4'><'col-sm-8'>>",
            data: dataRow.Anios,
            language: languageDatatable,
            columns: [
                { "data": "Anio", "className": 'text-center' },
                {
                    "data": "IdModeloAnio", "className": "text-center",
                    "render": function (data, type, row) {
                        return '<button type="button" id="btnVerHistorialDePrecios_' + row["IdModeloAnio"] + '" class="btn btn-sm btn-secondary ml-1 mr-1"><i class="fas fa-history"></i> Historial de precios</button>';
                    }
                },
            ],
        }).on('click', 'tr td:last-child', function () {

            let rowAnio = tblAnios.row($(this).closest("tr")); // obtener fila clickada por el usuario
            let rowAnioData = rowAnio.data();
            let historialPrecios = rowAnioData.HistorialDevaluaciones;

            $(".lblAnio").text(rowAnioData.Anio);

            MostrarHistorialDePreciosEnModal(historialPrecios);
        });
    }
});

function DrawDataTable_AniosDisponibles(idDataTable) {

    return `<div class="">
                <div>
                    <div class="table-responsive">
                        <table class="table-bordered display compact nowrap table-sm table-hover dataTable bg-white" class="tblAniosDisponibles" id="${idDataTable}" style="width: 20%">
                            <thead>
                                <tr class="text-center">
                                    <th>Año</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                </div>
            </div>
`;
}

function MostrarHistorialDePreciosEnModal(historialPrecios) {

    $('#tblHistorialDePrecios').DataTable({
        destroy: true,
        data: historialPrecios,
        language: languageDatatable,
        columns: [
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
                "data": "PrecioDeMercado", "className": "text-right",
                render: function (value) {
                    return 'L. ' + ConvertirADecimal(value);
                }
            },
            {
                "data": "UltimaDevaluacion", "className": "text-right",
                "render": function (data, type, row) {
                    return 'L. ' + ConvertirADecimal(row["UltimaDevaluacion"]);
                }
            },
            {
                "data": "UltimaDevaluacion", "className": "text-center",
                "render": function (data, type, row) {
                    return ((row["UltimaDevaluacion"] * 100) / row["PrecioDeMercado"]).toFixed(2) + '%';
                }
            },
        ],
    });


    // resplaod



    /* Para realizar filtros en ambas listas*/
    var filtroActual = '';
    var tabActivo = 'tab_listado_precios_de_mercado';
    var languageDatatable = {
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
    }

    $(document).ready(function () {

        dtListadoPreciosDeMercado = $('#datatable-listado-precios-de-mercado').DataTable({
            "pageLength": 15,
            "aaSorting": [],
            "dom": "<'row'<'col-sm-12'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-6'i><'col-sm-6'p>>",
            "language": languageDatatable,
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
            ],
        });

        /* Inicializar datable del listado modelos */
        dtListado_Historial_Devaluacion = $('#datatable-listado-devaluacion').DataTable({
            "pageLength": 15,
            "aaSorting": [],
            "dom": "<'row'<'col-sm-12'>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-6'i><'col-sm-6'p>>",
            "language": languageDatatable,
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
                { "data": "Marca" },
                { "data": "Modelo" },
                { "data": "Version" },
                {
                    "data": "Anios", "className": "text-center details-control", // DropDownList de años disponibles
                    "render": function (data, type, row) {

                        let aniosDisponibles = row["Anios"];

                        let ddlTemplate = '<select class="form-control form-control-sm" id="ddlAnio_' + row["IdMarca"] + row["IdModelo"] + row["Version"] + '">';

                        ddlTemplate += '<option value="0">Seleccionar</option>'

                        for (var i = 0; i < aniosDisponibles.length; i++) {

                            ddlTemplate += '<option value="' + aniosDisponibles[i].IdModeloAnio + '">' + aniosDisponibles[i].Anio + '</option>';
                        }

                        ddlTemplate += '</select>';

                        return ddlTemplate;
                    }
                },
                {
                    "data": "IdModelo", "className": "text-center details-control", // Boton de ver historial de precios
                    "render": function (data, type, row) {

                        return '<button type="button" id="btnVerHistorialDePrecios_' + row["IdMarca"] + row["IdModelo"] + row["Version"] + '" class="btn btn-sm btn-secondary ml-1 mr-1"><i class="fas fa-history"></i> Historial de precios</button>';
                    }
                },
            ],
            columnDefs: [
                { targets: [0], orderable: false },
                { "width": "10%", "targets": [3, 4] }
            ],
            initComplete: function () {
                this.api().columns([0, 1, 2]).every(function () {
                    var column = this;
                    var select = $('<select class=""><option value="">Todos</option></select>')
                        .appendTo($(column.header()))
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex(
                                $(this).val()
                            );
                            column.search(val ? '^' + val + '$' : '', true, false).draw();
                        });
                    column.data().unique().sort().each(function (d, j) {
                        select.append('<option value="' + d + '">' + d + '</option>')
                    });
                });
            },
        });

        $(".filterhead").not(":eq(3),:eq(4)").each(function (i) {
            //$(".filterhead").each(function (i) {
            var select = $('<select class="form-control form-control-sm"><option value="">Todos</option></select>')
                .appendTo($(this).empty())
                .on('change', function () {
                    var term = $(this).val();
                    dtListado_Historial_Devaluacion.column(i).search(term, false, false).draw();
                });
            dtListado_Historial_Devaluacion.column(i).data().unique().sort().each(function (d, j) {
                select.append('<option value="' + d + '">' + d + '</option>')
            });
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

    /* Mostrar historial de precios de un registro */
    $('#datatable-listado-devaluacion tbody').on('click', 'tr td:last-child', function () {

        let tr = $(this).closest("tr");
        let row = dtListado_Historial_Devaluacion.row(tr); // obtener fila clickada por el usuario
        let dataRow = row.data(); // obtener data de la fila

        if (dataRow == undefined) {
            return false;
        }

        $(".lblMarca").text(dataRow.Marca);
        $(".lblModelo").text(dataRow.Modelo);
        $(".lblVersion").text(dataRow.Version);

        let ddlAnioSeleccionado = $('#ddlAnio_' + dataRow.IdMarca + dataRow.IdModelo + dataRow.Version + ' :selected');

        let idModeloAnioSeleccionado = parseInt(ddlAnioSeleccionado.val());

        if (idModeloAnioSeleccionado > 0 && idModeloAnioSeleccionado != undefined && idModeloAnioSeleccionado != null && idModeloAnioSeleccionado != '') {

            $(".lblAnio").text(ddlAnioSeleccionado.text());

            var anioModelo = dataRow.Anios.find(x => {
                return x.IdModeloAnio === idModeloAnioSeleccionado
            });

            MostrarHistorialDePreciosEnModal(anioModelo.HistorialDevaluaciones);
        }
        else {
            MensajeAdvertencia(idModeloAnioSeleccionado == 0 ? 'Primero debes seleccionar un año' : 'Año inválido');

            $('#ddlAnio_' + dataRow.IdMarca + dataRow.IdModelo + dataRow.Version).focus();
        }
    });

    function MostrarHistorialDePreciosEnModal(historialPrecios) {

        $('#tblHistorialDePrecios').DataTable({
            destroy: true,
            data: historialPrecios,
            language: languageDatatable,
            columns: [
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
                    "data": "PrecioDeMercado", "className": "text-right",
                    render: function (value) {
                        return 'L. ' + ConvertirADecimal(value);
                    }
                },
                {
                    "data": "UltimaDevaluacion", "className": "text-right",
                    "render": function (data, type, row) {
                        return 'L. ' + ConvertirADecimal(row["UltimaDevaluacion"]);
                    }
                },
                {
                    "data": "UltimaDevaluacion", "className": "text-center",
                    "render": function (data, type, row) {
                        return ((row["UltimaDevaluacion"] * 100) / row["PrecioDeMercado"]).toFixed(2) + '%';
                    }
                },
            ],
        });

        $("#modalHistorialDePrecios").modal();

    }

    function MensajeError(mensaje) {
        iziToast.error({
            title: 'Error',
            message: mensaje
        });
    }

    function MensajeAdvertencia(mensaje) {
        iziToast.warning({
            title: 'Atención',
            message: mensaje
        });
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