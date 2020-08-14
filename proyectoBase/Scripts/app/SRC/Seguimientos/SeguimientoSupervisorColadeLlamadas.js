var FiltroActual = "";
var Actividad = 1;
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
        "processing": true,
        "dom": "<'row'<'col-sm-6'><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
    });

    $("input[type=radio][name=filtros]").change(function () {
        var filtro = this.value;
        switch (filtro) {
            case "hoy":
                $(".RangoFechas").css('display', 'none');
                FiltroActual = "hoy";
                Actividad = 1;
                FiltrarInformacion(Actividad);
                break;
            case "porHacer":
                $(".RangoFechas").css('display', 'none');
                FiltroActual = "porHacer";
                Actividad = 2;
                FiltrarInformacion(Actividad);
                break;
            case "anteriores":
                $(".RangoFechas").css('display', '');
                Actividad = 3;
                FiltroActual = "anteriores";
                FiltrarInformacion(Actividad);
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
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        if (FiltroActual == 'rangoFechas') {
            var t = $("#min").datepicker("getDate"),
                l = $("#max").datepicker("getDate"),
                n = new Date(a[6]);
            return ("Invalid Date" == t && "Invalid Date" == l) || ("Invalid Date" == t && n <= l) || ("Invalid Date" == l && n >= t) || (n <= l && n >= t);
        }
        else { return true; }
    });

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

$("#ddlAgentesActivos").change(function () {
    if ($("#ddlAgentesActivos :selected").val() != '') {
        FiltrarInformacion(Actividad);
    }
});

function FiltrarInformacion(Actividad) {

    if ($("#ddlAgentesActivos :selected").val() != '') {

        $('#datatable-clientes').DataTable().clear().draw();

        $.ajax({
            type: "POST",
            url: "SeguimientoSupervisorColadeLlamadas.aspx/CargarSolicitudes",
            data: JSON.stringify({ dataCrypt: window.location.href, IDAgente: $("#ddlAgentesActivos :selected").val(), IDActividad: Actividad }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información');
            },
            success: function (data) {

                var Listado = data.d;
                var InicioLlamada = ''
                var FinLlamada = ''
                var duracion = '';
                var DatatableColaLlamadas = $('#datatable-clientes').dataTable();

                for (var i = 0; i < Listado.length; i++) {

                    InicioLlamada = Listado[i].InicioLlamada == '/Date(-62135575200000)/' ? '' : moment(Listado[i].InicioLlamada).locale('es').format('YYYY/MM/DD h:mm:ss a');
                    FinLlamada = Listado[i].FinLlamada == '/Date(-62135575200000)/' ? '' : moment(Listado[i].FinLlamada).locale('es').format('YYYY/MM/DD h:mm:ss a');
                    duracion = Listado[i].SegundosDuracionLlamada == 0 ? '' : hhmmss(Listado[i].SegundosDuracionLlamada);

                    DatatableColaLlamadas.fnAddData([
                        Listado[i].NombreAgente,
                        Listado[i].IDCliente,
                        Listado[i].NombreCompletoCliente,
                        Listado[i].TelefonoCliente,
                        Listado[i].PrimerComentario,
                        Listado[i].SegundoComentario,
                        InicioLlamada,
                        FinLlamada,
                        duracion
                    ], false);
                }
                DatatableColaLlamadas.fnDraw();

                MensajeInformacion('La información se cargó correctamente');
            }
        });
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

function MensajeAdvertencia(mensaje) {
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

function MensajeInformacion(mensaje) {
    iziToast.info({
        title: 'Info',
        message: mensaje
    });
}