var FiltroActual = "0";
$(document).ready(function () {
    dtBandejaPrecalificados = $('#datatable-precalificados').DataTable({
        "responsive": true,
        "language": {
            "sProcessing": "Cargando información...",
            "sLengthMenu": "Mostrar _MENU_ registros",
            "sZeroRecords": "No se encontraron resultados",
            "sEmptyTable": "Ningún dato disponible en esta tabla",
            "sInfo": "<small>Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros</small>",
            "sInfoEmpty": "<small>Mostrando registros del 0 al 0 de un total de 0 registros</small>",
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
        },
        "pageLength": 15,
        "aaSorting": [],
        //"processing": true,
        "dom": "<'row'<'col-sm-6'><'col-sm-6'>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6 pt-0'i><'col-sm-6 pt-0'p>>",
        buttons: [
            {
                extend: 'copy',
                text: 'Copiar'
            },
            {
                extend: 'excelHtml5',
                title: 'Seguimiento_Cola_de_Llamadas_' + FiltroActual + '' + moment()
            },
            {
                extend: 'colvis',
                text: 'Columnas',
                title: 'Seguimiento_Cola_de_Llamadas_' + FiltroActual + '' + moment()
            }
        ],
        "ajax": {
            type: "POST",
            url: "BandejaPrecalificados.aspx/CargarLista",
            contentType: 'application/json; charset=utf-8',
            data: function (dtParms) {
                return JSON.stringify({ dataCrypt: window.location.href, pcEstado: "0" });
            },
            "dataSrc": function (json) {
                var return_data = json.d;
                return return_data;
            }
        },
        "columns": [
            { "data": "NombreCliente" },
            { "data": "Identidad" },
            {
                "data": "Ingresos",
                "className": 'text-right',
                "render": function (data, type, row) {
                    return row["Moneda"] + ' ' + addFormatoNumerico(parseFloat(row["Ingresos"]).toFixed(2))
                }
            },
            { "data": "Telefono" },
            { "data": "Producto" },
            {
                "data": "FechaConsultado",
                "render": function (value) {
                    return value == '/Date(-62135575200000)/' ? '' : moment(value).locale('es').format('YYYY/MM/DD h:mm:ss a');
                }
            },
            { "data": "Oficial" },
            { "data": "Oficial" }
        ],
        //columnDefs: [{
        //    className: 'dtr-control',
        //    orderable: false,
        //    targets: 0
        //}],
    });

    $("input[type=radio][name=filtros]").change(function () {
        //var filtro = this.value;
        //switch (filtro) {
        //    case "hoy":
        //        $(".RangoFechas").css('display', 'none');
        //        FiltroActual = "hoy";
        //        Actividad = 1;
        //        FiltrarInformacion();
        //        break;
        //    case "porHacer":
        //        $(".RangoFechas").css('display', 'none');
        //        FiltroActual = "porHacer";
        //        Actividad = 2;
        //        FiltrarInformacion();
        //        break;
        //    case "anteriores":
        //        $(".RangoFechas").css('display', '');
        //        Actividad = 3;
        //        FiltroActual = "anteriores";
        //        FiltrarInformacion();
        //        break;
        //}
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
        dtBandejaPrecalificados.draw();
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
});

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