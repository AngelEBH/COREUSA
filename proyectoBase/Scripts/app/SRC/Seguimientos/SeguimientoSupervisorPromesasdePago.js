var FiltroActual = "incumplidas";
var lenguajeEspanol = {
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
};

$(document).ready(function () {
    dtClientes = $('#datatable-clientes').DataTable({
        "responsive": true,
        "language": lenguajeEspanol,
        "pageLength": 10,
        "aaSorting": [],
        //"processing": true,
        "dom": "<'row'<'col-sm-6'<'toolbar'>><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        initComplete: function () {
            this.api().columns([0]).every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm w-auto"><option value="">Todos los Agentes</option></select>')
                    .appendTo($("div.toolbar").empty())
                    .on('change', function () {
                        var val = $.fn.dataTable.util.escapeRegex(
                            $(this).val()
                        );
                        column
                            .search(val ? '^' + val + '$' : '', true, false)
                            .draw();
                    });
                column.data().unique().sort().each(function (d, j) {
                    select.append('<option value="' + d + '">' + d + '</option>')
                });
            });
        },
        "ajax": {
            type: "POST",
            url: "SeguimientoSupervisorPromesasdePago.aspx/CargarRegistros",
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
            {
                "data": "IDCliente",
                "render": function (data, type, row) {
                    return "<a href=" + row["UrlCliente"] + ">" + row["IDCliente"] + "</a>"
                }
            },
            { "data": "NombreCompletoCliente" },
            {
                "data": "Atraso",
                "className": 'text-right',
                "render": function (data, type, row) {
                    return row["Moneda"] + ' ' + addComasFormatoNumerico(parseFloat(row["Atraso"]).toFixed(2))
                }
            },
            { "data": "DiasMora", "className": "text-center" },
            {
                "data": "FechaRegistrado",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            {
                "data": "FechaPromesa",
                "render": function (value) {
                    if (value === null) return "";
                    return moment(value).locale('es').format('YYYY/MM/DD');
                }
            },
            { "data": "EstadoActual" }
        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    /* Filtrar cuando se seleccione una opción */
    $("input[type=radio][name=filtros]").change(function () {
        var filtro = this.value;
        switch (filtro) {
            case "0":
                FiltroActual = "";
                dtClientes.draw();
                break;
            case "incumplidas":
                FiltroActual = "incumplidas";
                dtClientes.draw();
                break;
            case "hoy":
                FiltroActual = "hoy";
                dtClientes.draw();
                break;
            case "futuras":
                FiltroActual = "futuras";
                dtClientes.draw();
                break;
        }
    });

    /* Agregar Filtros */
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

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtClientes.search($(this).val()).draw();
    })

    /* Listas seleccionables */
    $(".buscadorddl").select2({
        language: {
            errorLoading: function () { return "No se pudieron cargar los resultados" },
            inputTooLong: function (e) { var n = e.input.length - e.maximum, r = "Por favor, elimine " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
            inputTooShort: function (e) { var n = e.minimum - e.input.length, r = "Por favor, introduzca " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
            loadingMore: function () { return "Cargando más resultados…" },
            maximumSelected: function (e) { var n = "Sólo puede seleccionar " + e.maximum + " elemento"; return 1 != e.maximum && (n += "s"), n },
            noResults: function () { return "No se encontraron resultados" },
            searching: function () { return "Buscando…" },
            removeAllItems: function () { return "Eliminar todos los elementos" }
        }
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




var t = "";
if (e.d[i].fdEnIngresoInicio != a) {
    t = e.d[i].fdEnIngresoFin != a ? IconoExito : IconoPendiente;
}

//estado en tramite de la solicitud
var l = '';
if (e.d[i].fdEnTramiteInicio != a) {

    if (e.d[i].fdEnTramiteFin != a) {
        l = IconoExito;
    }
    else {
        l = IconoPendiente;
    }

    if (e.d[i].fdEnTramiteFin == a && (e.d[i].fiEstadoSolicitud == 4 || e.d[i].fiEstadoSolicitud == 5 || e.d[i].fiEstadoSolicitud == 7)) {
        l = e.d[i].fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
    }
}

//validar si ya se inició y terminó el proceso de analisis de la solicitud
var n = '';
if (e.d[i].fdEnAnalisisInicio != a) {

    if (e.d[i].fdEnAnalisisFin != a) {
        n = IconoExito;
    }
    else {
        n = IconoPendiente;
    }

    if (e.d[i].fdEnAnalisisFin == a && (e.d[i].fiEstadoSolicitud == 4 || e.d[i].fiEstadoSolicitud == 5 || e.d[i].fiEstadoSolicitud == 7)) {
        n = e.d[i].fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
    }
}

var s = '';
if (e.d[i].fdEnvioARutaAnalista != a) {

    if (e.d[i].fdEnCampoFin != a || e.d[i].fiEstadoDeCampo == 2) {
        s = IconoExito;
    }
    else {
        s = IconoPendiente;
    }

    if (e.d[i].fdEnCampoFin == a && (e.d[i].fiEstadoSolicitud == 4 || e.d[i].fiEstadoSolicitud == 5 || e.d[i].fiEstadoSolicitud == 7)) {
        s = e.d[i].fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
    }
    if (e.d[i].fiEstadoSolicitud == 5) {
        s = IconoRojo;
    }
}

//var s = "";
//s =
//    0 != e.d[i].fiEstadoDeCampo
//        ? 2 == e.d[i].fiEstadoDeCampo
//            ? '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">,</label></i>'
//            : '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">.</label></i>'
//        : '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"<label style="display:none;"><label style="display:none;">_</label></i>';

//validar si la solicitud está condicionada y sigue sin recibirse actualizacion del agente de ventas
var c = '';
if (e.d[i].fdCondicionadoInicio != a) {

    if (e.d[i].fdCondificionadoFin != a) {
        c = IconoExito;
    }
    else {
        c = IconoPendiente;
        habilitarResolucion = false;
    }

    if (e.d[i].fdCondificionadoFin == a && (e.d[i].fiEstadoSolicitud == 4 || e.d[i].fiEstadoSolicitud == 5 || e.d[i].fiEstadoSolicitud == 7)) {
        c = e.d[i].fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
    }
}

//var c = "";
//c =
//    e.d[i].fdCondicionadoInicio != a
//        ? e.d[i].fdCondificionadoFin != a
//            ? '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">,</label></i>'
//            : '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">.</label></i>'
//            : '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0" style="display:none;"><label style="display:none;">_</label></i>';

var d = '';
if (e.d[i].fdReprogramadoInicio != a) {

    if (e.d[i].fdReprogramadoFin != a) {
        d = IconoExito;
    }
    else {
        d = IconoPendiente;
    }

    if (e.d[i].fdReprogramadoFin == a && (e.d[i].fiEstadoSolicitud == 4 || e.d[i].fiEstadoSolicitud == 5 || e.d[i].fiEstadoSolicitud == 7)) {
        d = e.d[i].fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
    }
}

var o = '';
if (e.d[i].PasoFinalInicio != a) {

    if (e.d[i].PasoFinalFin != a) {
        o = IconoExito;
    } else {
        o = IconoPendiente;
    }

    if (e.d[i].PasoFinalFin == a && (e.d[i].fiEstadoSolicitud == 4 || e.d[i].fiEstadoSolicitud == 5 || e.d[i].fiEstadoSolicitud == 7)) {
        o = e.d[i].fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
    }
}

var resolucionFinal = '<label class="btn-sm btn-block btn-warning">Pendiente</label>';
if (e.d[i].fiEstadoSolicitud == 4 || e.d[i].fiEstadoSolicitud == 5 || e.d[i].fiEstadoSolicitud == 7) {
    resolucionFinal = e.d[i].fiEstadoSolicitud == 7 ? '<label class="btn btn-sm btn-block btn-success">Aprobada</label>' : '<label class="btn btn-sm btn-block btn-danger">Rechazada</label>';
}
var r = e.d[i].fcPrimerNombreCliente + " " + e.d[i].fcSegundoNombreCliente + " " + e.d[i].fcPrimerApellidoCliente + " " + e.d[i].fcSegundoApellidoCliente,
    u = FechaFormatoConGuiones(e.d[i].fdFechaCreacionSolicitud);