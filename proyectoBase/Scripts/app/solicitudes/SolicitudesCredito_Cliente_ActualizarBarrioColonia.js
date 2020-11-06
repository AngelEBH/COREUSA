/* Cargar municipios del departamento seleccionado del domicilio */
$("#ddlDepartamentoDomicilio").change(function () {

    $(this).parsley().validate();

    var idDepartamento = $("#ddlDepartamentoDomicilio option:selected").val();
    CargarMunicipios(idDepartamento, 'Domicilio', true, 0);
});

/* Cargar ciudades del Municipio seleccionado del domicilio */
$("#ddlMunicipioDomicilio").change(function () {

    $(this).parsley().validate();

    var idDepartamento = $("#ddlDepartamentoDomicilio option:selected").val();
    var idMunicipio = $("#ddlMunicipioDomicilio option:selected").val();
    CargarCiudadesPoblados(idDepartamento, idMunicipio, 'Domicilio', true, 0);
});

/* Cargar barrios y colonias de la ciudad/poblado seleccionada del domicilio */
$("#ddlCiudadPobladoDomicilio").change(function () {

    $(this).parsley().validate();

    var idDepartamento = $("#ddlDepartamentoDomicilio option:selected").val();
    var idMunicipio = $("#ddlMunicipioDomicilio option:selected").val();
    var idCiudadPoblado = $("#ddlCiudadPobladoDomicilio option:selected").val();
    CargarBarriosColonias(idDepartamento, idMunicipio, idCiudadPoblado, 'Domicilio', 0);
});

/* Cargar municipios del departamento seleccionado de la empresa */
$("#ddlDepartamentoEmpresa").change(function () {

    $(this).parsley().validate();

    var idDepartamentoEmpresa = $("#ddlDepartamentoEmpresa option:selected").val();
    CargarMunicipios(idDepartamentoEmpresa, 'Empresa', true, 0);
});

/* Cargar ciudades del Municipio seleccionado de la empresa */
$("#ddlMunicipioEmpresa").change(function () {

    $(this).parsley().validate();

    var idDepartamentoEmpresa = $("#ddlDepartamentoEmpresa option:selected").val();
    var idMunicipioEmpresa = $("#ddlMunicipioEmpresa option:selected").val();
    CargarCiudadesPoblados(idDepartamentoEmpresa, idMunicipioEmpresa, 'Empresa', true, 0);
});

/* Cargar barrios y colonias de la ciudad/poblado seleccionada de la empresa */
$("#ddlCiudadPobladoEmpresa").change(function () {

    $(this).parsley().validate();

    var idDepartamentoEmpresa = $("#ddlDepartamentoEmpresa option:selected").val();
    var idMunicipioEmpresa = $("#ddlMunicipioEmpresa option:selected").val();
    var idCiudadPobladoEmpresa = $("#ddlCiudadPobladoEmpresa option:selected").val();
    CargarBarriosColonias(idDepartamentoEmpresa, idMunicipioEmpresa, idCiudadPobladoEmpresa, 'Empresa', 0);
});

function CargarMunicipios(idDepartamento, tipoLista, desabilitarListasDependientes, idMunicipioSeleccionar) {

    var ddlMunicipio = $('#ddlMunicipio' + tipoLista + '');
    var ddlCiudadPoblado = $('#ddlCiudadPoblado' + tipoLista + '');
    var ddlBarrioColonia = $('#ddlBarrioColonia' + tipoLista + '');

    if (idDepartamento != '') {

        ddlMunicipio.empty();
        ddlMunicipio.append("<option value=''>Seleccione una opción</option>");

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Cliente_ActualizarBarrioColonia.aspx/CargarListaMunicipios",
            data: JSON.stringify({ idDepartamento: idDepartamento }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al cargar municipios de este departamento');
            },
            success: function (data) {

                var listaMunicipios = data.d;
                $.each(listaMunicipios, function (i, iter) {
                    ddlMunicipio.append("<option value='" + iter.IdMunicipio + "'>" + iter.NombreMunicipio + "</option>"); /* Agregar municipios del departamento seleccionado */
                });
                ddlMunicipio.attr('disabled', false);

                if (idMunicipioSeleccionar != 0) {
                    ddlMunicipio.val(idMunicipioSeleccionar);
                }

                if (desabilitarListasDependientes == true) {
                    ddlCiudadPoblado.empty();
                    ddlCiudadPoblado.append("<option value=''>Seleccione un municipio</option>");
                    ddlCiudadPoblado.attr('disabled', true);

                    ddlBarrioColonia.empty();
                    ddlBarrioColonia.append("<option value=''>Seleccione una ciudad/poblado</option>");
                    ddlBarrioColonia.attr('disabled', true);
                }
            }
        });
    }
    else {
        ddlMunicipio.empty();
        ddlMunicipio.append("<option value=''>Seleccione un depto.</option>");
        ddlMunicipio.attr('disabled', true);

        ddlCiudadPoblado.empty();
        ddlCiudadPoblado.append("<option value=''>Seleccione un municipio</option>");
        ddlCiudadPoblado.attr('disabled', true);

        ddlBarrioColonia.empty();
        ddlBarrioColonia.append("<option value=''>Seleccione una ciudad/poblado</option>");
        ddlBarrioColonia.attr('disabled', true);
    }
}

function CargarCiudadesPoblados(idDepartamento, idMunicipio, tipoLista, desabilitarListasDependientes, idCiudadPobladoSeleccionar) {

    var ddlCiudadPoblado = $('#ddlCiudadPoblado' + tipoLista + '');
    var ddlBarrioColonia = $('#ddlBarrioColonia' + tipoLista + '');

    if (idMunicipio != '') {

        ddlCiudadPoblado.empty();
        ddlCiudadPoblado.append("<option value=''>Seleccione una opción</option>");

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Cliente_ActualizarBarrioColonia.aspx/CargarListaCiudadesPoblados",
            data: JSON.stringify({ idDepartamento: idDepartamento, idMunicipio: idMunicipio }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar ciudades de este municipio');
            },
            success: function (data) {

                var listaCiudades = data.d;

                $.each(listaCiudades, function (i, iter) {
                    ddlCiudadPoblado.append("<option value='" + iter.IdCiudadPoblado + "'>" + iter.NombreCiudadPoblado + "</option>"); // Agregar ciudades/poblados del municipio
                });
                ddlCiudadPoblado.attr('disabled', false);

                if (idCiudadPobladoSeleccionar != 0) {
                    ddlCiudadPoblado.val(idCiudadPobladoSeleccionar);
                }

                if (desabilitarListasDependientes == true) {
                    ddlBarrioColonia.empty();
                    ddlBarrioColonia.append("<option value=''>Seleccione una ciudad/poblado</option>");
                    ddlBarrioColonia.attr('disabled', true);
                }
            }
        });
    }
    else {
        ddlCiudadPoblado.empty();
        ddlCiudadPoblado.append("<option value=''>Seleccione un municipio</option>");
        ddlCiudadPoblado.attr('disabled', true);

        ddlBarrioColonia.empty();
        ddlBarrioColonia.append("<option value=''>Seleccione una ciudad/poblado</option>");
        ddlBarrioColonia.attr('disabled', true);
    }
}

function CargarBarriosColonias(idDepartamento, idMunicipio, idCiudadPoblado, tipoLista, idBarrioColoniaSeleccionar) {

    var ddlBarrioColonia = $('#ddlBarrioColonia' + tipoLista + '');

    if (idCiudadPoblado != '') {

        ddlBarrioColonia.empty();
        ddlBarrioColonia.append("<option value=''>Seleccione una opción</option>");

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Cliente_ActualizarBarrioColonia.aspx/CargarListaBarriosColonias",
            data: JSON.stringify({ idDepartamento: idDepartamento, idMunicipio: idMunicipio, idCiudadPoblado: idCiudadPoblado }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar los barrios y colonias de esta ciudad/poblado');
            },
            success: function (data) {

                var listaBarriosColonias = data.d;

                $.each(listaBarriosColonias, function (i, iter) {
                    ddlBarrioColonia.append("<option value='" + iter.IdBarrioColonia + "'>" + iter.NombreBarrioColonia + "</option>"); // Agregar barrios/colonias de la ciudad seleccionada
                });
                ddlBarrioColonia.attr('disabled', false);

                if (idBarrioColoniaSeleccionar != 0) {
                    ddlBarrioColonia.val(idBarrioColoniaSeleccionar);
                }
            }
        });
    }
    else {
        ddlBarrioColonia.empty();
        ddlBarrioColonia.append("<option value=''>Seleccione una ciudad/poblado</option>");
        ddlBarrioColonia.attr('disabled', true);
    }
}

function MensajeExito(mensaje) {
    iziToast.success({
        title: 'Exito',
        message: mensaje
    });
}

function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

function MensajeInformacion(mensaje) {
    iziToast.info({
        title: 'Info',
        message: mensaje
    });
}

function ConvertirFechaJavaScriptAFechaCsharp(fecha) {
    var date = new Date(fecha);
    var milliseconds = date.getTime();
    var dt = new Date(parseInt(milliseconds));
    var fechaConvertida = `${
        dt.getFullYear().toString().padStart(4, '0')}-${
        (dt.getMonth() + 1).toString().padStart(2, '0')}-${
        dt.getDate().toString().padStart(2, '0')} ${
        dt.getHours().toString().padStart(2, '0')}:${
        dt.getMinutes().toString().padStart(2, '0')}:${
        dt.getSeconds().toString().padStart(2, '0')}`;

    return fechaConvertida
}