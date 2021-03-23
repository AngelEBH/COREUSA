/* Seleccionar precio de mercado de la garantía */
$("#btnSeleccionarPrecioDeMercado").click(function () {

    var ddlMarca = $('#ddlMarca').empty().append("<option value=''>Seleccione una opción</option>");
    $('#ddlModelo').empty().append("<option value=''>Seleccione una marca</option>").prop('disabled', true); // reiniciar ddl marcas
    $('#ddlAnio').empty().append("<option value=''>Seleccione un modelo</option>").prop('disabled', true); // reiniciar ddl anios

    $('#btnSeleccionarPrecioDeMercadoConfirmar,#btnSolicitarPrecioDeMercado').prop('disabled', true); // inhabilitar opcion
    $('#txtPrecioDeMercadoActual').val(''); // reiniciar precio de mercado

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarMarcas",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar las marcas disponibles, contacte al administrador.');
        },
        success: function (data) {

            var listaMarcas = data.d;
            var templateMarcas = '';

            $.each(listaMarcas, function (i, iter) {
                templateMarcas += "<option value='" + iter.Id + "'>" + iter.Descripcion + "</option>";
            });
            ddlMarca.append(templateMarcas).prop('disabled', false); // llenar ddl marcas

            $("#modalSeleccionarPrecioDeMercado").modal(); // mostrar modal
        }
    });
});

// Cargar los modelos de la marca seleccionada
$("#ddlMarca").change(function () {

    var idMarca = $("#ddlMarca option:selected").val(); // id marca seleccionada
    var ddlModelo = $('#ddlModelo');
    var ddlAnio = $('#ddlAnio');

    if (idMarca != '') { // si se seleccionó una marca

        ddlModelo.empty().append("<option value=''>Seleccione una marca</option>").prop('disabled', true); // reiniciar ddl de modelos
        ddlAnio.empty().append("<option value=''>Seleccione un modelo</option>").prop('disabled', true); // reiniciar ddl de años

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarModelosPorIdMarca",
            data: JSON.stringify({ idMarca: idMarca, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudieron cargar los modelos de la marca seleccionada, contacte al administrador.');
            },
            success: function (data) {

                let listaModelos = data.d;
                let templateModelos = '';

                $.each(listaModelos, function (i, iter) {
                    templateModelos += "<option value='" + iter.Id + "'>" + iter.Descripcion + "</option>";
                });

                ddlModelo.append(templateModelos).prop('disabled', false); // Agregar modelos de la marca seleccionada al ddl de modelos
            }
        });
    }
    else {
        ddlModelo.empty().append("<option value=''>Seleccione una marca</option>").prop('disabled', true); // reiniciar ddl de modelos
        ddlAnio.empty().append("<option value=''>Seleccione un modelo</option>").prop('disabled', true); // reiniciar ddl de años

        $('#btnSeleccionarPrecioDeMercadoConfirmar,#btnSolicitarPrecioDeMercado').prop('disabled', true); // deshabilitar opciones
        $('#txtPrecioDeMercadoActual').val(''); // reiniciar precio de mercado
    }
});

$("#ddlModelo").change(function () {

    var idModelo = $("#ddlModelo option:selected").val(); // id del modelo seleccionado
    var ddlAnio = $('#ddlAnio');

    if (idModelo != '') { // si se seleccionó un modelo

        ddlAnio.empty().append("<option value=''>Seleccione un modelo</option>").prop('disabled', true); // reiniciar ddl de años

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarAniosDisponiblesPorIdModelo",
            data: JSON.stringify({ idModelo: idModelo, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudieron cargar los años disponibles del modelo seleccionado, contacte al administrador.');
            },
            success: function (data) {

                var listaAniosDisponibles = data.d;
                var templateAnios = '';

                $.each(listaAniosDisponibles, function (i, iter) {
                    templateAnios += "<option value='" + iter.Id + "'>" + iter.Descripcion + "</option>";
                });
                ddlAnio.append(templateAnios).prop('disabled', false); // agregar lista de años al ddl de años
            }
        });
    }
    else {
        ddlAnio.empty().append("<option value=''>Seleccione un modelo</option>").prop('disabled', true); // reiniciar ddl de años
        $('#btnSeleccionarPrecioDeMercadoConfirmar,#btnSolicitarPrecioDeMercado').prop('disabled', true); // deshabilitar opciones
        $('#txtPrecioDeMercadoActual').val(''); // reiniciar precios de mercado
    }
});

$("#ddlAnio").change(function () {

    var idModeloAnio = $("#ddlAnio option:selected").val(); // id del año-modelo seleccionado

    if (idModeloAnio != '') {

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarPrecioDeMercadoPorIdModeloAnio",
            data: JSON.stringify({ idModeloAnio: idModeloAnio, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar el precio de mercado actual de la garantía seleccionada, contacte al administrador.');
            },
            success: function (data) {

                if (data.d.Id != 0) {
                    $('#txtPrecioDeMercadoActual').val(data.d.Descripcion); // Setear precio de mercado actual del marca-modelo-año seleccionado
                    $('#btnSeleccionarPrecioDeMercadoConfirmar').prop('disabled', false); // habilitar opciones
                    $('#btnSolicitarPrecioDeMercado').prop('disabled', true); // habilitar opciones
                }
                else {
                    $('#txtPrecioDeMercadoActual').val('');
                    $('#btnSolicitarPrecioDeMercado').prop('disabled', false); // habilitar opciones
                    $('#btnSeleccionarPrecioDeMercadoConfirmar').prop('disabled', true); // deshabilitar opciones
                    MensajeAdvertencia("No hay ningún precio de mercado asignado a la garantía seleccionado. <br /> <b>NOTA: Recuerda que puedes solicitar los precios de mercado.</b>");
                }
            }
        });
    }
    else {
        $('#btnSeleccionarPrecioDeMercadoConfirmar,#btnSolicitarPrecioDeMercado').prop('disabled', true); // deshabilitar opciones
        $('#txtPrecioDeMercadoActual').val(''); // reiniciar precio de mercado
    }
});

$("#btnSeleccionarPrecioDeMercadoConfirmar").click(function () {

    $("#txtValorGlobal").val($('#txtPrecioDeMercadoActual').val());
    $("#modalSeleccionarPrecioDeMercado").modal('hide');
});