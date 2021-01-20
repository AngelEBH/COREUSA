var esPrimeraCarga = true;
var filtroActual = "";
var idActividad = 1;
var lenguaje = {
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
    "buttons": {
        copyTitle: 'Copiar al portapapeles',
        copySuccess: {
            1: "Copiada una fila al portapapeles",
            _: "Copiadas %d filas al portapapeles"
        }

    },
    "decimal": ".",
    "thousands": ","
};

$(document).ready(function () {

    /* Al cargar la pagina por primera vez no es necesario que se carguen los registros de la tabla por actividades */
    //if (!esPrimeraCarga) {
    dtClientes = $('#datatable-clientes').DataTable({
        "responsive": true,
        "language": lenguaje,
        "pageLength": 10,
        "aaSorting": [],
        "processing": true,
        "dom": "<'row'<'col-sm-6'B><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        buttons: [
            {
                extend: 'copy',
                text: 'Copiar'
            },
            {
                extend: 'excelHtml5',
                title: 'Seguimiento_Cola_de_Llamadas_' + filtroActual + '' + moment()
            },
            {
                extend: 'pdf',
                title: 'Seguimiento_Cola_de_Llamadas_' + filtroActual + '' + moment()
            },
            {
                extend: 'colvis',
                text: 'Ocultar columnas',
                title: 'Seguimiento_Cola_de_Llamadas_' + filtroActual + '' + moment()
            }
        ],
        "ajax": {
            type: "POST",
            url: "SeguimientoSupervisorColadeLlamadas.aspx/CargarRegistros",
            contentType: 'application/json; charset=utf-8',
            data: function (dtParms) {
                return JSON.stringify({ dataCrypt: window.location.href, IDAgente: $("#ddlAgentesActivos :selected").val(), IDActividad: idActividad });
            },
            "dataSrc": function (json) {
                var return_data = json.d;
                return return_data;
            }
        },
        "columns": [
            { "data": "NombreAgente" },
            { "data": "IDCliente" },
            { "data": "NombreCompletoCliente" },
            { "data": "TelefonoCliente" },
            { "data": "PrimerComentario" },
            { "data": "SegundoComentario" },
            {
                "data": "InicioLlamada",
                "render": function (value) {
                    return value == '/Date(-62135575200000)/' ? '' : moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            {
                "data": "FinLlamada",
                "render": function (value) {
                    return value == '/Date(-62135575200000)/' ? '' : moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            {
                "data": "SegundosDuracionLlamada",
                "render": function (value) {
                    return value == 0 ? '' : hhmmss(value);
                }
            }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    dtClientes.buttons().container().appendTo('#datatable-buttons_wrapper .col-md-6:eq(0)');

    //} if !esPrimeraCarga

    dtResumen = $('#datatable-resumenAgentes').DataTable({
        "responsive": true,
        "language": lenguaje,
        "pageLength": 10,
        "aaSorting": [],
        "processing": true,
        "dom": "<'row'<'col-sm-6'B><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'><'col-sm-6'p>>",
        buttons: [
            {
                extend: 'copy',
                text: 'Copiar'
            },
            {
                extend: 'excelHtml5',
                title: 'Seguimiento_Cola_de_Llamadas_' + filtroActual + '' + moment()
            },
            {
                extend: 'pdf',
                title: 'Seguimiento_Cola_de_Llamadas_' + filtroActual + '' + moment()
            },
            {
                extend: 'colvis',
                text: 'Ocultar columnas',
                title: 'Seguimiento_Cola_de_Llamadas_' + filtroActual + '' + moment()
            }
        ],
        "ajax": {
            type: "POST",
            url: "SeguimientoSupervisorColadeLlamadas.aspx/CargarResumen",
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
            { "data": "NombreAgente" },
            { "data": "LlamadasPorHacer", className: "text-center" },
            { "data": "LlamadasHechas", className: "text-center" },
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    dtResumen.buttons().container().appendTo('#datatable-buttons_wrapper .col-md-6:eq(0)');



    $("input[type=radio][name=filtros]").change(function () {

        var filtro = this.value;

        switch (filtro) {

            case "hoy":
                $("#tblClientes,#divFiltros").css('display', '');
                $("#tblResumen").css('display', 'none');
                $(".RangoFechas").css('display', 'none');
                filtroActual = "hoy";
                idActividad = 1;
                FiltrarInformacion();
                break;

            case "porHacer":
                $("#tblClientes,#divFiltros").css('display', '');
                $("#tblResumen").css('display', 'none');
                $(".RangoFechas").css('display', 'none');
                filtroActual = "porHacer";
                idActividad = 2;
                FiltrarInformacion();
                break;

            case "anteriores":
                $("#tblClientes,#divFiltros").css('display', '');
                $("#tblResumen").css('display', 'none');

                $(".RangoFechas").css('display', '');
                idActividad = 3;
                filtroActual = "anteriores";
                FiltrarInformacion();
                break;

            case "resumenAgentes":
                $(".RangoFechas").css('display', '');
                $("#tblClientes,#divFiltros").css('display', 'none');
                $("#tblResumen").css('display', '');
                dtResumen.ajax.reload(null, false);
                break;
        }
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
        dtClientes.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
        if (filtroActual == 'rangoFechas') {
            var Desde = moment($("#min").datepicker("getDate")).format('YYYY/MM/DD'),
                Hasta = moment($("#max").datepicker("getDate")).format('YYYY/MM/DD'),
                FechaLlamada = moment(data[6]).format('YYYY/MM/DD');
            return (Desde == "Invalid Date" && Hasta == "Invalid Date") || (Desde == "Invalid Date" && FechaLlamada <= Hasta) || (Hasta == "Invalid Date" && FechaLlamada >= Desde) || (FechaLlamada <= Hasta && FechaLlamada >= Desde);
        }
        else { return true; }
    });

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtClientes.search($(this).val()).draw();
    })
});

$("#ddlAgentesActivos").change(function () {

    FiltrarInformacion();

});

function FiltrarInformacion() {

    if (idActividad != 0)
    {
        dtClientes.ajax.reload(null, false);
    }
    else
    {
        MensajeAdvertencia('Seleccione una actividad');
    }
}

function pad(num) {
    return ("0" + num).slice(-2);
}

function hhmmss(secs) {
    var minutes = Math.floor(secs / 60);
    secs = secs % 60;
    var hours = Math.floor(minutes / 60)
    minutes = minutes % 60;
    return `${pad(hours)}:${pad(minutes)}:${pad(secs)}`;
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