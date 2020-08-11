var IDNT = "";
recargarDatatable(),
    $(document).ready(function () {
        $("body").addClass("enlarged"),
            $("#datatable-bandeja tbody").on("click", "tr", function () {
                var e = $(this),
                    a = e.closest("tr").data("encargado"),
                    z = e.closest("tr").data("estado"),
                    i = tableSolicitudes.row(e).data()[4].replace("<small>", "").replace("</small>", "") + ' ',
                    t = tableSolicitudes.row(e).data()[3].replace("<small>", "").replace("</small>", "");
                if (((IDNT = t), $("#lblCliente").text(i), $("#lblIdentidadCliente").text(t), null != a)) {
                    var l = "?" + window.location.href.split("?")[1];
                    $.ajax({
                        type: "POST",
                        url: "SolicitudesCredito_Bandeja.aspx/VerificarAnalista" + l,
                        data: JSON.stringify({ ID: a }),
                        contentType: "application/json; charset=utf-8",
                        error: function (e, a, i) {
                            MensajeError("No se pudo cargar la información, contacte al administrador");
                        },
                        success: function (a) {
                            if (0 == a.d) e.closest("tr").data("analista");
                            (IDSOL = e.closest("tr").data("id")), $("#modalAbrirSolicitud").modal({ backdrop: !1 });
                        },
                    });
                } else (IDSOL = e.closest("tr").data("id")), $("#modalAbrirSolicitud").modal({ backdrop: !1 });
            }),
            $("#añoIngreso").datepicker({ format: "yyyy", viewMode: "years", minViewMode: "years" });
    });
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
}),
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
var tableSolicitudes = $("#datatable-bandeja").DataTable({
    responsive: !0,
    lengthChange: !1,
    pageLength: 50,
    ordering: !1,
    searching: !0,
    dom: "rt<'row'<'col-sm-4'i><'col-sm-8'p>>",
    language: {
        sProcessing: "Procesando...",
        sLengthMenu: "Mostrar _MENU_ registros",
        sZeroRecords: "No se encontraron resultados",
        sEmptyTable: "Ningún dato disponible en esta tabla",
        sInfo: "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
        sInfoEmpty: "Mostrando registros del 0 al 0 de un total de 0 registros",
        sInfoFiltered: "(filtrado de un total de _MAX_ registros)",
        sInfoPostFix: "",
        sSearch: "Buscar:",
        sUrl: "",
        sInfoThousands: ",",
        sLoadingRecords: "Cargando...",
        oPaginate: { sFirst: "Primero", sLast: "Último", sNext: "Siguiente", sPrevious: "Anterior" },
        oAria: { sSortAscending: ": Activar para ordenar la columna de manera ascendente", sSortDescending: ": Activar para ordenar la columna de manera descendente" },
        decimal: ".",
        thousands: ",",
    },
});
function recargarDatatable() {
    var e = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Bandeja.aspx/CargarSolicitudes" + e,
        data: JSON.stringify({ IDSOL: "0" }),
        contentType: "application/json; charset=utf-8",
        error: function (e, a, i) {
            MensajeError("No se pudo carga la información, contacte al administrador");
        },
        success: function (e) {
            if (($("#datatable-bandeja").DataTable().clear(), null != e && e.d.length > 0))
                for (var a = "/Date(-2208967200000)/", i = 0; i < e.d.length; i++) {

                    var IconoExito = '<i class="mdi mdi-check-circle mdi-24px text-success p-0"><label style="display:none;">,</label></i>';
                    var IconoPendiente = '<i class="mdi mdi-check-circle mdi-24px text-secondary p-0"><label style="display:none;">.</label></i>';
                    var IconoRojo = '<i class="mdi mdi mdi-close-circle mdi-24px text-danger p-0"></i>';

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

                    tableSolicitudes.row
                        .add(
                            $(
                                "<tr data-id=" + e.d[i].fiIDSolicitud + " data-estado=" + e.d[i].fiEstadoSolicitud + " data-analista = " + e.d[i].fcNombreUsuarioModifica + " data-encargado = " + e.d[i].fiIDUsuarioModifica + ">" +
                                "<td>" +
                                e.d[i].fiIDSolicitud+
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
function MensajeError(e) {
    iziToast.error({ title: "Error", message: e });
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
$("#identidadCliente").on("keyup", function () {
    tableSolicitudes.columns(3).search(this.value).draw();
}),
    $("#nombreCliente").on("keyup", function () {
        tableSolicitudes.columns(4).search(this.value).draw();
    }),
    $("#mesIngreso").on("change", function () {
        "" != this.value
            ? tableSolicitudes
                .columns(5)
                .search("/" + this.value + "/")
                .draw()
            : tableSolicitudes.columns(2).search("").draw();
    }),
    $("#añoIngreso").on("change", function () {
        tableSolicitudes
            .columns(5)
            .search(this.value + "/")
            .draw();
    }),
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        var t = $("#min").datepicker("getDate"),
            l = $("#max").datepicker("getDate"),
            n = new Date(a[5]);
        return ("Invalid Date" == t && "Invalid Date" == l) || ("Invalid Date" == t && n <= l) || ("Invalid Date" == l && n >= t) || (n <= l && n >= t);
    }),
    $("#min").datepicker({
        onSelect: function () {
            tableSolicitudes.draw();
        },
        changeMonth: !0,
        changeYear: !0,
    }),
    $("#max").datepicker({
        onSelect: function () {
            tableSolicitudes.draw();
        },
        changeMonth: !0,
        changeYear: !0,
    }),
    $("#min, #max").change(function () {
        tableSolicitudes.draw();
    }),
    $("input[type=radio][name=filtros]").change(function () {
        var e = this.value;
        switch ((tableSolicitudes.columns([6, 7, 8, 9, 10, 11, 12, 13]).search("").draw(), e)) {
            case "0":
                tableSolicitudes.columns([6, 7, 8, 9, 10, 11, 12, 13]).search("").draw();
                break;
            case "7":
                tableSolicitudes.columns(7).search(".").columns(13).search("Pendiente").draw();
                break;
            case "8":
                tableSolicitudes.columns(8).search(".").columns(9).search("_").columns(10).search("_").columns(13).search("Pendiente").draw();
                break;
            case "9":
                tableSolicitudes.columns(9).search(".").columns(13).search("Pendiente").draw();
                break;
            case "10":
                tableSolicitudes.columns(10).search(".").columns(13).search("Pendiente").draw();
                break;
            case "11":
                tableSolicitudes.columns(11).search(".").columns(13).search("Pendiente").draw();
                break;
            case "12":
                tableSolicitudes.columns(12).search(".").columns(13).search("Pendiente").draw();
                break;
            case "13":
                tableSolicitudes.columns(13).search("Pendiente").draw();
                break;
            case "14":
                tableSolicitudes.columns(13).search("Aprobada").draw();
                break;
            case "15":
                tableSolicitudes.columns(13).search("Rechazada").draw();
                break;
            default:
                tableSolicitudes.columns([6, 7, 8, 9, 10, 11, 12, 13]).search("").draw();
        }
    }),
    jQuery("#date-range").datepicker({ toggleActive: !0 });