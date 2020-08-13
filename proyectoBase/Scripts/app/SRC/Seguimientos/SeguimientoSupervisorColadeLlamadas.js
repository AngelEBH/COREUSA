var lenguaje = {
    "sProcessing": "Cargando registros...",
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
    "sLoadingRecords": "Cargando solicitudes...",
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
};

var FiltroActual = "";

$(document).ready(function () {
    dtClientes = $('#datatable-clientes').DataTable({
        "responsive": true,
        "language": lenguaje,
        "pageLength": 10,
        "aaSorting": [],
        "ajax": {
            type: "POST",
            url: "SeguimientoSupervisorColadeLlamadas.aspx/CargarSolicitudes",
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
            { "data": "IDCliente" },
            { "data": "NombreCompletoCliente" },
            { "data": "TelefonoCliente" },
            { "data": "PrimerComentario" },
            { "data": "SegundoComentario" },

            {
                "data": "InicioLlamada",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            {
                "data": "FinLlamada",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            {
                "data": "SegundosDuracionLlamada",
                "render": function (value) {
                    return hhmmss(value);
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
                dtClientes.draw();
                break;
            case "porHacer":
                $(".RangoFechas").css('display', 'none');
                FiltroActual = "porHacer";
                dtClientes.draw();
                break;
            case "anteriores":
                $(".RangoFechas").css('display', '');
                FiltroActual = "anteriores";
                dtClientes.draw();
                break;
        }
    });
    $.fn.dataTable.ext.search.push(
        function (settings, data, dataIndex) {

            var EstadoRetornar = false;
            var hoy = moment().format('YYYY/MM/DD');
            var promesaPago = data[6];

            if (FiltroActual == "") {
                EstadoRetornar = true;
            }
            else if (FiltroActual == "incumplidas") {
                EstadoRetornar = data[7] == 'Incumplida' ? true : false;
            }
            else if (FiltroActual == "futuras" && (moment(promesaPago).isAfter(hoy))) {
                EstadoRetornar = true;
            }
            else if (FiltroActual == "hoy" && (moment(promesaPago).isSame(hoy))) {
                EstadoRetornar = true;
            }
            return EstadoRetornar;
        }
    );

    $("#min").datepicker({
        onSelect: function () {
            dtClientes.draw();
        },
        changeMonth: !0,
        changeYear: !0,
    });
    $("#max").datepicker({
        onSelect: function () {
            dtClientes.draw();
        },
        changeMonth: !0,
        changeYear: !0,
    });
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        var t = $("#min").datepicker("getDate"),
            l = $("#max").datepicker("getDate"),
            n = new Date(a[6]);
        return ("Invalid Date" == t && "Invalid Date" == l) || ("Invalid Date" == t && n <= l) || ("Invalid Date" == l && n >= t) || (n <= l && n >= t);
    });
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function addComasFormatoNumerico(nStr) {
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

function pad(num) {
    return ("0" + num).slice(-2);
}
function hhmmss(secs) {
    var minutes = Math.floor(secs / 60);
    secs = secs % 60;
    var hours = Math.floor(minutes / 60)
    minutes = minutes % 60;
    return `${pad(hours)}:${pad(minutes)}:${pad(secs)}`;
    // return pad(hours)+":"+pad(minutes)+":"+pad(secs); for old browsers
}