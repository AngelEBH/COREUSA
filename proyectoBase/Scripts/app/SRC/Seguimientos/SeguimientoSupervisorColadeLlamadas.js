var FiltroActual = "";
var Actividad = 1;

$(document).ready(function () {
    dtClientes = $('#datatable-clientes').DataTable({
        "responsive": true,
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
        "pageLength": 10,
        "aaSorting": [],
        "processing": true,
        "dom": "<'row'<'col-sm-6'><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        "ajax": {
            type: "POST",
            url: "SeguimientoSupervisorColadeLlamadas.aspx/CargarRegistros",
            contentType: 'application/json; charset=utf-8',
            data: function (dtParms) {
                return JSON.stringify({ dataCrypt: window.location.href, IDAgente: $("#ddlAgentesActivos :selected").val(), IDActividad: Actividad });
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

    $("input[type=radio][name=filtros]").change(function () {
        var filtro = this.value;
        switch (filtro) {
            case "hoy":
                $(".RangoFechas").css('display', 'none');
                FiltroActual = "hoy";
                Actividad = 1;
                FiltrarInformacion();
                break;
            case "porHacer":
                $(".RangoFechas").css('display', 'none');
                FiltroActual = "porHacer";
                Actividad = 2;
                FiltrarInformacion();
                break;
            case "anteriores":
                $(".RangoFechas").css('display', '');
                Actividad = 3;
                FiltroActual = "anteriores";
                FiltrarInformacion();
                break;
        }
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
        dtClientes.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
        if (FiltroActual == 'rangoFechas') {
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
    if ($("#ddlAgentesActivos :selected").val() != '0') {
        FiltrarInformacion();
    }
});

function FiltrarInformacion() {

    if ($("#ddlAgentesActivos :selected").val() != '0') {
        dtClientes.ajax.reload(null, false);
    }
    else {
        MensajeAdvertencia('Seleccione un agente');
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