var filtroActual = "";
var fcfnCapitalFinanciado = 0;
var fcProductoCliente = "";
estadoMasRelevante = '';


function Prueba(IdSolicitud) {


    $("#modalDetallePickupPayments").modal();
    $.ajax({
        type: "POST",
        url: "PickUpPayments.aspx/ObtenerDetallePickUpPayments",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
          //  MensajeError("No se pudo redireccionar a ");
        },
        success: function (data) {

            $('#tblListaDetallesPickupPayments').DataTable({
                "destroy": true,
                "pageLength": 100,
                //  "aaSorting": [],
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
                dom: 'f',
                data: data.d,
                "columns": [
                    { data: 'fcCondicion' },
                    {
                        data: 'fcDescripcionCondicion',

                    },
                    { data: 'fcComentarioAdicional' },
                    { data: 'EstadoCondicion' },
                ],
                //columnDefs: [
                //    { targets: 'no-sort', orderable: false },
                //]
            });

            $("#modalCrearExpedientePrestamo").modal();
        }
    });

}


$(document).ready(function () {

    dtBandeja = $('#datatable-bandeja').DataTable({
        "pageLength": 20,
        "aaSorting": [],
        "dom": "<'row'<'col-sm-6'><'col-sm-6'T>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-6'i><'col-sm-6'p>>",
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
        "ajax": {
            type: "POST",
            url: "Prestamo_Lista.aspx/CargarPrestamos",
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
                "data": "fcIDPrestamo", "className": "text-center",
                "render": function (data, type, row) {

                    return '<div class="dropdown mo-mb-2">' +
                        '<button class="btn pt-0 pb-0 mt-0 mb-0" type="button" id="dropdownMenuButton" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" >' +
                        '<i class="fa fa-bars"></i>' +
                        '</button >' +
                        '<div class="dropdown-menu" aria-labelledby="dropdownMenuButton">' +
                        `<button type="button" class="dropdown-item" id="btnAbrirDetalles" onclick="RedirigirAccion('Prestamo_Ficha.aspx','detalles de Prestamo', '${data}' )" aria-label="Detalles"><i class="far fa-file-alt"></i> Ver detalles</button>`+
                        //'<button type="button" class="dropdown-item" id="btnDetalles" data-id="' + row["fcIDPrestamo"] + '"><i class="fas fa-tasks"></i> Detalles</button>' +
                        '</div>' +
                        '</div >';
                }
            },
            { "data": "fcNombreCliente" },
            { "data": "fcIDPrestamo" },
            { "data": "fiIDProducto" },
            { "data": "fcProducto" },
            {
                data: 'fdFechaCreacion',
                render: function (data) {
                    return `<div style="display:none;">${moment(data)}</div> <span class="label label-table label-default">${moment(data).format("DD/MMM/YYYY")}</span>`;
                }
            },
            {
                data: 'fnCapitalFinanciado',
                className:'text-right',
                render: function (data) {
                    return    accounting.formatNumber(data, 2)
                }
            },
            {
                data: 'fnSaldoActualCapital',
                className:'text-right',
                render: function (data) {
                    return    accounting.formatNumber(data, 2)
                }
            },
            {
                data: 'fcEstadoPrestamo',
                render: function (data) {

                    if (data == 'Vigente') return `<label class="btn btn-sm btn-block btn-success m-1">Vigente</label>`
                    else return `<label class="btn btn-sm btn-block btn-warning m-1">Pendiente</label>`;

                }
            },

			
			//{ "data": "fcEstadoPrestamo" }
        ]
    });

 

    /* Mostrar mensaje de actualización a los usuarios durante los próximos 2 días*/
    if (moment().isBefore('2020-11-05T23:50:00-06:00')) {

        iziToast.info({
            title: 'Info',
            message: 'Estimado usuario, ahora las opciones "Detalles" y "Actualizar condiciones" se encuentran en la primera columna con titulo "Acciones".'
        });
    }

    /* busqueda por mes de ingreso */
    $('#mesIngreso').on('change', function () {
        if (this.value != '') {
            dtBandeja.columns(5).search('/' + this.value + '/').draw();
        }
        else {
            dtBandeja.columns(5).search('').draw();
        }
    });

    $('#estadoFiltro').on('change', function () {
        
        console.log(this.value);
        if (this.value != '') {
            dtBandeja.columns(8).search(this.value).draw();
        }
        else {
            dtBandeja.columns(8).search('').draw();
        }
    });


    

    /* busqueda por año de ingreso */
    $('#añoIngreso').on('change', function () {
        dtBandeja.columns(5).search( '/'+this.value ).draw();
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
        dtBandeja.draw();
    });

    /* Agregar Filtros */
    $.fn.dataTable.ext.search.push(function (e, a, i) {
        if (filtroActual == 'rangoFechas') {
            var Desde = $("#min").datepicker("getDate"),
                Hasta = $("#max").datepicker("getDate"),
                FechaIngreso = new Date(a[5]);
            return ("Invalid Date" == Desde && "Invalid Date" == Hasta) || ("Invalid Date" == Desde && FechaIngreso <= Hasta) || ("Invalid Date" == Hasta && FechaIngreso >= Desde) || (FechaIngreso <= Hasta && FechaIngreso >= Desde);
        }
        else { return true; }
    });

    $("#añoIngreso").datepicker({
        format: "yyyy",
        viewMode: "years",
        minViewMode: "years"
    });

    /* Buscador */
    $('#txtDatatableFilter').keyup(function () {
        dtBandeja.search($(this).val()).draw();
    });

    $("#datatable-bandeja tbody").on("click", "tr", function () {
        var row = dtBandeja.row(this).data();

        fnCapitalFinanciado = row.fnCapitalFinanciado;
        fcProductoCliente = row.IdCliente;
        $("#lblCliente").text(row.fnCapitalFinanciado);
        $("#lblfcProductoCliente").text(row.IdCliente);
        $("#modalAbrirPrestamo").modal();
    });
});

$(document).on('click', 'button#btnActualizar', function () {
    $("#modalActualizarPrestamo").modal();
});

$('#btnActualizar').click(function (e) {

    $.ajax({
        type: "POST",
        url: "Prestamo_Lista.aspx/EncriptarParametros",
        data: JSON.stringify({ dataCrypt: window.location.href, fcfnCapitalFinanciado: fcfnCapitalFinanciado, fcProducto: fcProductoCliente }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la Prestamo, contacte al administrador');
        },
        success: function (data) {
            if (data.d != "-1") {
                window.location = "Prestamo_Ficha.aspx?" + data.d;
            }
            else {
                MensajeError('No se pudo al redireccionar a pantalla de actualización');
            }
        }
    });
});

// $(document).on('click', 'button#btnDetalles', function () {

//     $.ajax({
//         type: "POST",
//         url: "Prestamo_Lista.aspx/EncriptarParametros",
//         data: JSON.stringify({ dataCrypt: window.location.href, fcfnCapitalFinanciado: fcfnCapitalFinanciado, fcProducto: fcProductoCliente }),
//         contentType: 'application/json; charset=utf-8',
//         error: function (xhr, ajaxOptions, thrownError) {
//             MensajeError('No se pudo cargar la Prestamo, contacte al administrador');
//         },
//         success: function (data) {

//             if (data.d != "-1") {
//                 window.location = "PrestamoesCredito_RegistradaDetalles.aspx?" + data.d;
//             }
//             else {
//                 MensajeError('No se pudo al redireccionar a pantalla de detalles');
//             }
//         }
//     });
// });

$(document).ready(function () {

    debugger;
    $('.btnEditar').on('click', function () {
        var $btn = $(this);
        var data = $btn.data().json;
        alert("Entrando");
       // alert("Jedi Name : " + data.name);
    })
});


function RedirigirAccion(nombreFormulario, accion, idPrestamo) {

    $.ajax({
        type: "POST",
        url: "Prestamo_Lista.aspx/EncriptarParametros",
        data: JSON.stringify({ pcIDPrestamo: idPrestamo, identidad:'', dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError("No se pudo redireccionar a " + accion);
        },
        success: function (data) {

            data.d != "-1" ? window.location = nombreFormulario + "?" + data.d : MensajeError("No se pudo redireccionar a" + accion);
        }
    });
}

jQuery('#date-range').datepicker({
    toggleActive: true
});

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}