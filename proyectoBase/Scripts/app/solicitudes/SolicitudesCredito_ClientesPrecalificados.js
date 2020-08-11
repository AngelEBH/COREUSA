$(document).ready(function () {

    recargarDatatable();    

});

//abrir modal de ingresar solicitud
var identidadCliente = 0;
$(document).on('click', 'button#btnIngresarSolicitud', function () {

    identidadCliente = $(this).data('id');
    var nombresCliente = $(this).data('nombre');
    $("#lblNombreCliente").text(nombresCliente);
    $("#lblIdentidadCliente").text(identidadCliente);
    $("#modalActualizarSolicitud").modal();
});

//abrir pantalla de ingresar la solicitud
$('#btnIngresarSolictudConfirmar').click(function (e) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ClientesPrecalificados.aspx/GetDetallesPrecalificado",
        data: JSON.stringify({ identidad: identidadCliente }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo carga la información, contacte al administrador');
        },
        success: function (data) {
            if (data.d != null) {
                sessionStorage.clear();
                localStorage.clear();
                localStorage.setItem("precalificado", JSON.stringify(data.d));
                window.location = "SolicitudesCredito_Ingresar.aspx";
            }
            else {
                MensajeError('No se pudo carga la información, contacte al administrador');
            }
        }
    });

    
});

//función cargar información del datatable
function recargarDatatable() {
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ClientesPrecalificados.aspx/GetClientesPrecalificados",
        data: JSON.stringify({ identidad: '' }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo carga la información, contacte al administrador');
        },
        success: function (data) {

            $('#datatable-bandeja').DataTable().clear();

            // agregar rows al datatable
            if (data != null && data.d.length > 0) {

                for (var i = 0; i < data.d.length; i++) {

                    var botonIngresarSolicitud = '<button id="btnIngresarSolicitud" data-id="' + data.d[i].identidad + '" data-nombre="' + data.d[i].nombres + '" type="button" class="btn btn-primary btn-block waves-effect waves-light">Ingresar solicitud</button>';

                    tableSolicitudes.row.add($('<tr>' +
                        '<td>' + data.d[i].identidad + '</td >' +
                        '<td>' + data.d[i].nombres + '</td>' +
                        '<td>' + data.d[i].telefono + '</td>' +
                        '<td>' + addComasFormatoNumerico(data.d[i].ingresos) + '</td>' +
                        '<td>' + data.d[i].comentario + '</td>' +
                        '<td>' + botonIngresarSolicitud + '</td>' +
                        '</tr>')[0]).draw(false);
                }
            }
        }
    });
}

// inicializar datatables
var tableSolicitudes = $('#datatable-bandeja').DataTable({
    responsive: true,
    lengthChange: false,
    pageLength: 50,
    "ordering": false,
    searching: true,
    dom: "brt<'row'<'col-sm-4'i><'col-sm-8'p>>",
    buttons: [{
        extend: 'excel',
        text: 'Excel',
        titleAttr: 'Exportar a Excel'
    },
    {
        extend: 'pdf',
        text: 'PDF',
        titleAttr: 'Exportar a PDF'
    },
    {
        extend: 'colvis',
        text: 'Columnas visibles',
        titleAttr: 'Columnas visibles'
    }
    ],
    "language": {
        "sProcessing": "Procesando...",
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
        "sLoadingRecords": "Cargando...",
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
});

// FILTROS //

// busqueda por nombre del cliente
$('#nombreCliente').on('keyup', function () {
    tableSolicitudes.columns(1)
        .search(this.value)
        .draw();
});

// busqueda por identidad del cliente
$('#identidadCliente').on('keyup', function () {
    tableSolicitudes.columns(0)
        .search(this.value)
        .draw();
});

//incluir Utilities.js
$.getScript("../../Scripts/app/Utilities.js")
    .done(function (script, textStatus) {
    })
    .fail(function (jqxhr, settings, exception) {
        console.log('error al cargar utilities');
    });