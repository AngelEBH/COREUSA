var IconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">,</label></i>';
var IconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">.</label></i>';
var IconoRojo = '<i class="mdi mdi mdi-close-circle mdi-24px text-danger p-0"></i>';
var a = "/Date(-2208967200000)/";

var IDNT = "";
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
    dtBandeja = $('#datatable-bandeja').DataTable({
        "responsive": true,
        "language": lenguajeEspanol,
        "pageLength": 10,
        "aaSorting": [],
        //"processing": true,
        "dom": "<'row'<'col-sm-6'l><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
        initComplete: function () {
            this.api().columns().every(function () {
                var column = this;
                var select = $('<select class="form-control form-control-sm"><option value="">Filtrar</option></select>')
                    .appendTo($(column.footer()))
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
            { "data": "fiIDSolicitud" },
            { "data": "fcAgencia" },
            { "data": "fcDescripcion" },
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
                    return moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            {
                "data": "fdEnIngresoInicio",
                "render": function (data, type, row) {
                    return row["fdEnIngresoInicio"].fdEnIngresoFin != a ? IconoExito : IconoPendiente;
                }
            },

            {
                "data": "fdEnTramiteInicio",
                "render": function (data, type, row) {
                    var l = "";
                    if (row["fdEnTramiteFin"] != a) {
                        l = IconoExito;
                    }
                    else {
                        l = IconoPendiente;
                    }

                    if (row["fdEnTramiteFin"] == a && (row["fiEstadoSolicitud"] == 4 || row["fiEstadoSolicitud"] == 5 || row["fiEstadoSolicitud"] == 7)) {
                        l = row["fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                    }

                    return row["fdEnIngresoInicio"].fdEnIngresoFin != a ? IconoExito : IconoPendiente;
                }
            },

        ],
        columnDefs: [
            { targets: 'no-sort', orderable: false }
        ]
    });

    /* Filtrar cuando se seleccione una opción */
    //$("input[type=radio][name=filtros]").change(function () {
    //    var filtro = this.value;
    //    switch (filtro) {
    //        case "0":
    //            FiltroActual = "";
    //            dtBandeja.draw();
    //            break;
    //        case "incumplidas":
    //            FiltroActual = "incumplidas";
    //            dtBandeja.draw();
    //            break;
    //        case "hoy":
    //            FiltroActual = "hoy";
    //            dtBandeja.draw();
    //            break;
    //        case "futuras":
    //            FiltroActual = "futuras";
    //            dtBandeja.draw();
    //            break;
    //    }
    //});

    /* Agregar Filtros */
    //$.fn.dataTable.ext.search.push(
    //    function (settings, data, dataIndex) {

    //        var EstadoRetornar = false;
    //        var hoy = moment().format('YYYY/MM/DD');
    //        var promesaPago = data[6];

    //        if (FiltroActual == "") {
    //            EstadoRetornar = true;
    //        }
    //        else if (FiltroActual == "incumplidas") {
    //            EstadoRetornar = data[7] == 'Incumplida' ? true : false;
    //        }
    //        else if (FiltroActual == "futuras" && (moment(promesaPago).isAfter(hoy))) {
    //            EstadoRetornar = true;
    //        }
    //        else if (FiltroActual == "hoy" && (moment(promesaPago).isSame(hoy))) {
    //            EstadoRetornar = true;
    //        }
    //        return EstadoRetornar;
    //    }
    //);

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtBandeja.search($(this).val()).draw();
    });

    $("#añoIngreso").datepicker({ format: "yyyy", viewMode: "years", minViewMode: "years" });

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

function addFormatoNumerico(nStr) {
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

var IDSOL = 0;
$("#btnAbrirSolicitud").click(function (e) {
    var a = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/AbrirAnalisisSolicitud" + a,
        data: JSON.stringify({ IDSOL: IDSOL, Identidad: IDNT }),
        contentType: "application/json; charset=utf-8",
        error: function (e, a, i) {
            MensajeError("No se pudo cargar la solicitud, contacte al administrador");
        },
        success: function (e) {
            "-1" != e.d ? (window.location = "SolicitudesCredito_Analisis.aspx?" + e.d) : MensajeError("Esta solicitud ya está siendo analizada por otro usuario");
        },
    });
});

$("#btnDetallesSolicitud").click(function (e) {
        var a = "?" + window.location.href.split("?")[1];
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Bandeja.aspx/EncriptarParametros" + a,
            data: JSON.stringify({ IDSOL: IDSOL, Identidad: IDNT }),
            contentType: "application/json; charset=utf-8",
            error: function (e, a, i) {
                MensajeError("No se pudo cargar la solicitud, contacte al administrador");
            },
            success: function (e) {
                "-1" != e.d ? (window.location = "SolicitudesCredito_Detalles.aspx?" + e.d) : MensajeError("No se pudo al redireccionar a pantalla de detalles");
            },
        });
    });

function recargarDatatable() {
    var e = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/CargarSolicitudes" + e,
        data: JSON.stringify({ dataCrypt: window.location.href, IDSOL: "0" }),
        contentType: "application/json; charset=utf-8",
        error: function (e, a, i) {
            MensajeError("No se pudo carga la información, contacte al administrador");
        },
        success: function (e) {
            if (($("#datatable-bandeja").DataTable().clear(), null != e && e.d.length > 0))
                for (var a = "/Date(-2208967200000)/", i = 0; i < e.d.length; i++) {

                    

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

                    dtBandeja.row
                        .add(
                            $(
                                "<tr data-id=" + e.d[i].fiIDSolicitud + " data-estado=" + e.d[i].fiEstadoSolicitud + " data-analista = " + e.d[i].fcNombreUsuarioModifica + " data-encargado = " + e.d[i].fiIDUsuarioModifica + ">" +
                                "<td>" +
                                e.d[i].fiIDSolicitud +
                                "</td ><td>" +
                                e.d[i].fcAgencia +
                                "</td ><td>" +
                                e.d[i].fcDescripcion.split(' ')[1] +
                                "</td ><td>" +
                                e.d[i].fcIdentidadCliente +
                                "</td ><td>" +
                                r +
                                "</td><td>" +
                                u +
                                '</td><td class="text-center">' +
                                t +
                                '</td><td class="text-center">' +
                                l +
                                '</td><td class="text-center">' +
                                n +
                                '</td><td class="text-center">' +
                                s +
                                '</td><td class="text-center">' +
                                c +
                                '</td><td class="text-center">' +
                                d +
                                '</td><td class="text-center">' +
                                o +
                                '</td><td class="text-center">' +
                                resolucionFinal +
                                "</td></tr>"
                            )[0]
                        )
                        .draw(!1);
                }
        },
    });
}

function pad2(e) {
    return (e < 10 ? "0" : "") + e;
}
function FechaFormatoConGuiones(e) {
    if (!e) return "Sin modificaciones";
    var a = e.substr(6, 19),
        i = new Date(parseInt(a)),
        t = pad2(i.getMonth() + 1),
        l = pad2(i.getDate());
    return i.getFullYear() + "/" + t + "/" + l + " " + pad2(i.getHours()) + ":" + pad2(i.getMinutes()) + ":" + pad2(i.getSeconds().toString());
}

$("#mesIngreso").on("change", function () {
    "" != this.value
        ? dtBandeja
            .columns(5)
            .search("/" + this.value + "/")
            .draw()
        : dtBandeja.columns(2).search("").draw();
});

$("#añoIngreso").on("change", function () {
    dtBandeja
        .columns(5)
        .search(this.value + "/")
        .draw();
});

$.fn.dataTable.ext.search.push(function (e, a, i) {
    var t = $("#min").datepicker("getDate"),
        l = $("#max").datepicker("getDate"),
        n = new Date(a[5]);
    return ("Invalid Date" == t && "Invalid Date" == l) || ("Invalid Date" == t && n <= l) || ("Invalid Date" == l && n >= t) || (n <= l && n >= t);
});

$("#min").datepicker({
    onSelect: function () {
        dtBandeja.draw();
    },
    changeMonth: !0,
    changeYear: !0,
});

$("#max").datepicker({
    onSelect: function () {
        dtBandeja.draw();
    },
    changeMonth: !0,
    changeYear: !0,
});

$("#min, #max").change(function () {
    dtBandeja.draw();
});

$("input[type=radio][name=filtros]").change(function () {
    var e = this.value;
    switch ((dtBandeja.columns([6, 7, 8, 9, 10, 11, 12, 13]).search("").draw(), e)) {
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
jQuery("#date-range").datepicker({ toggleActive: !0 });