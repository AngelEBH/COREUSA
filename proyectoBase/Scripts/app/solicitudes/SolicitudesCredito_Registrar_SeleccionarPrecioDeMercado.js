var idMarcaSeleccionada = 0;
var marcaSeleccionada = '';
var idModeloSeleccionado = 0;
var modeloSeleccionado = '';
var idModeloAnioSeleccionado = 0;
var modeloAnioSeleccionado = 0;

/* Seleccionar precio de mercado de la garantía */
$("#btnSeleccionarPrecioDeMercado").click(function () {

    CargarMarcas(0);
});

// Cargar los modelos de la marca seleccionada
$("#ddlMarca").change(function () {

    idMarcaSeleccionada = $("#ddlMarca option:selected").val();
    marcaSeleccionada = $("#ddlMarca option:selected").text();

    CargarModelos(idMarcaSeleccionada, 0);
});

$("#ddlModelo").change(function () {

    idModeloSeleccionado = $("#ddlModelo option:selected").val(); // id del modelo seleccionado
    modeloSeleccionado = $("#ddlModelo option:selected").text();

    CargarAniosDisponiblesPorModelo(idModeloSeleccionado, 0);
});

$("#ddlAnio").change(function () {

    idModeloAnioSeleccionado = $("#ddlAnio option:selected").val(); // id del año-modelo seleccionado
    modeloAnioSeleccionado = $("#ddlAnio option:selected").text();

    CargarPrecioDeMercadoPorIdModeloAnio(idModeloAnioSeleccionado);
});

/* Seleccionar precio de mercado */
$("#btnSeleccionarPrecioDeMercadoConfirmar").click(function () {

    $("#txtValorGlobal").val($('#txtPrecioDeMercadoActual').val());
    $("#modalSeleccionarPrecioDeMercado").modal('hide');
});

/* Soliciar precio de mercado */
$("#btnSolicitarPrecioDeMercado").click(function () {

    if (idMarcaSeleccionada == 0) {
        MensajeError('No se ha seleccionado ninguna marca');
        $("#ddlMarca").focus();
        return false
    }

    if (idModeloSeleccionado == 0) {
        MensajeError('No se ha seleccionado ningún modelo');
        $("#ddlModelo").focus();
        return false
    }
    if (idModeloAnioSeleccionado == 0) {
        MensajeError('No se ha seleccionado ningún año');
        $("#ddlAnio").focus();
        return false
    }

    $(".lblMarca").text(marcaSeleccionada);
    $(".lblModelo").text(modeloSeleccionado);
    $(".lblAnio").text(modeloAnioSeleccionado);

    $("#txtPrecioDeMercadoSolicitado").val('');
    $("#txtComentarioSolicitarPrecioDeMercado").val('');

    $('#frmSolicitud').parsley().reset({ group: 'solicitarPrecioDeMercado', force: true });

    $("#modalSeleccionarPrecioDeMercado").modal('hide');
    $("#modalSolicitarPrecioDeMercado").modal();
});

$("#btnSolicitarPrecioDeMercadoConfirmar").click(function () {

    if ($('#frmSolicitud').parsley().isValid({ group: 'solicitarPrecioDeMercado', excluded: ':disabled' })) {

        $('#btnSolicitarPrecioDeMercadoConfirmar').prop('disabled', true);

        let precioSolicitado = $("#txtPrecioDeMercadoSolicitado").val().replace(/,/g, '') == '' ? 0 : $("#txtPrecioDeMercadoSolicitado").val().replace(/,/g, '');
        let comentariosParaCreditos = $("#txtComentarioSolicitarPrecioDeMercado").val();

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/SolicitarPrecioDeMercado",
            data: JSON.stringify({ idModeloAnio: idModeloAnioSeleccionado, precioSolicitado: precioSolicitado, comentarios: comentariosParaCreditos, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo solicitar el precio de mercado, contacte al administrador.");
            },
            success: function (data) {

                if (data.d.ResultadoExitoso == true) {
                    MensajeExito(data.d.MensajeResultado);

                    CargarPrecioDeMercadoPorIdModeloAnio(idModeloAnioSeleccionado);

                    $("#modalSolicitarPrecioDeMercado").modal('hide');
                    $("#modalSeleccionarPrecioDeMercado").modal();
                }
                else
                    MensajeError(data.d.MensajeResultado);

                console.log(data.d.MensajeDebug);
            },
            complete: function (data) {
                $('#btnSolicitarPrecioDeMercadoConfirmar').prop('disabled', false);
            }
        });
    }
    else
        $('#frmSolicitud').parsley().validate({ group: 'solicitarPrecioDeMercado', excluded: ':disabled', force: true });
});


/******************* Agregar items que no estén disponibles (Marcas, modelos, años del modelo seleccionado y solicitar el precio de mercado) *******************/

/******** Agregar marcas ********/
$("#btnAgregarMarca").click(function () {

    $("#txtMarcaAgregar").val('');
    $('#txtMarcaAgregar').parsley().reset();
    $("#modalSeleccionarPrecioDeMercado").modal('hide');
    $("#modalAgregarMarcaPrecioMercado").modal();
});

$("#btnAgregarMarcaCancelar").click(function () {
    $("#modalAgregarMarcaPrecioMercado").modal('hide');
    $("#modalSeleccionarPrecioDeMercado").modal();
});

$("#btnAgregarMarcaConfirmar").click(function () {

    /* Validar formulario de agregar referencia personal */
    if ($("#txtMarcaAgregar").parsley().isValid()) {

        $('#btnAgregarMarcaConfirmar').prop('disabled', true);

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/GuardarMarca",
            data: JSON.stringify({ marca: $("#txtMarcaAgregar").val(), dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo guardar la marca, contacte al administrador.");
            },
            success: function (data) {

                if (data.d.ResultadoExitoso == true) {
                    MensajeExito(data.d.MensajeResultado);

                    CargarMarcas(data.d.IdInsertado);

                    $("#modalAgregarMarcaPrecioMercado").modal('hide');
                    $("#modalSeleccionarPrecioDeMercado").modal();
                }
                else
                    MensajeError(data.d.MensajeResultado);

                console.log(data.d.MensajeDebug);
            },
            complete: function (data) {
                $('#btnAgregarMarcaConfirmar').prop('disabled', false);
            }
        });
    }
    else
        $("#txtMarcaAgregar").parsley().validate();
});

/******** Agregar Modelos ********/
$("#btnAgregarModelo").click(function () {

    $("#txtModeloAgregar,#txtVersionAgregar").val('');
    $('#frmSolicitud').parsley().reset({ group: 'agregarModelo', force: true });
    $("#modalSeleccionarPrecioDeMercado").modal('hide');
    $("#modalAgregarModelosPrecioMercado").modal();
});

$("#btnAgregarModeloCancelar").click(function () {
    $("#modalAgregarModelosPrecioMercado").modal('hide');
    $("#modalSeleccionarPrecioDeMercado").modal();
});

$("#btnAgregarModeloConfirmar").click(function () {

    /* Validar formulario de agregar referencia personal */
    if ($('#frmSolicitud').parsley().isValid({ group: 'agregarModelo', excluded: ':disabled' })) {

        $('#btnAgregarModeloConfirmar').prop('disabled', true);

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/GuardarModelo",
            data: JSON.stringify({ idMarca: idMarcaSeleccionada, modelo: $("#txtModeloAgregar").val(), version: $("#txtVersionAgregar").val(), dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo guardar el modelo, contacte al administrador.");
            },
            success: function (data) {

                if (data.d.ResultadoExitoso == true) {
                    MensajeExito(data.d.MensajeResultado);

                    CargarModelos(idMarcaSeleccionada, data.d.IdInsertado);

                    $("#modalAgregarModelosPrecioMercado").modal('hide');
                    $("#modalSeleccionarPrecioDeMercado").modal();
                }
                else
                    MensajeError(data.d.MensajeResultado);

                console.log(data.d.MensajeDebug);
            },
            complete: function (data) {
                $('#btnAgregarModeloConfirmar').prop('disabled', false);
            }
        });
    }
    else
        $('#frmSolicitud').parsley().validate({ group: 'agregarModelo', excluded: ':disabled', force: true });
});

/******** Agregar año de un modelo ********/
$("#btnAgregarModeloAnio").click(function () {

    $("#ddlAnioAgregarAnioModelo").val('').parsley().reset();
    $('#btnAgregarAnioModeloConfirmar').prop('disabled', true);

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarCatalogoDeAnios",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar el catálogo de años, contacte al administrador.');
        },
        success: function (data) {

            var ddlAnioAgregarAnioModelo = $("#ddlAnioAgregarAnioModelo").empty().append("<option value=''>Seleccione una opción</option>");
            var listaAnios = data.d;
            var templateAnios = '';

            $.each(listaAnios, function (i, iter) {
                templateAnios += "<option value='" + iter.Id + "'>" + iter.Descripcion + "</option>";
            });
            ddlAnioAgregarAnioModelo.append(templateAnios).prop('disabled', false);

            $('#btnAgregarAnioModeloConfirmar').prop('disabled', false);
            $("#modalSeleccionarPrecioDeMercado").modal('hide');
            $("#modalAgregarAnioModeloPrecioMercado").modal();
        }
    });
});

$("#btnAgregarAnioModeloCancelar").click(function () {
    $("#modalAgregarAnioModeloPrecioMercado").modal('hide');
    $("#modalSeleccionarPrecioDeMercado").modal();
});

$("#btnAgregarAnioModeloConfirmar").click(function () {

    /* Validar formulario de agregar referencia personal */
    if ($("#ddlAnioAgregarAnioModelo").parsley().isValid()) {

        $('#btnAgregarAnioModeloConfirmar').prop('disabled', true);

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/GuardarModeloAnio",
            data: JSON.stringify({ idModelo: idModeloSeleccionado, idAnio: $("#ddlAnioAgregarAnioModelo option:selected").val(), dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo guardar el año para el modelo seleccionado, contacte al administrador.");
            },
            success: function (data) {

                if (data.d.ResultadoExitoso == true) {
                    MensajeExito(data.d.MensajeResultado);

                    CargarAniosDisponiblesPorModelo(idModeloSeleccionado, data.d.IdInsertado);

                    $("#modalAgregarAnioModeloPrecioMercado").modal('hide');
                    $("#modalSeleccionarPrecioDeMercado").modal();
                }
                else
                    MensajeError(data.d.MensajeResultado);

                console.log(data.d.MensajeDebug);
            },
            complete: function (data) {
                $('#btnAgregarAnioModeloConfirmar').prop('disabled', false);
            }
        });
    }
    else
        $("#ddlAnioAgregarAnioModelo").parsley().validate();
});

/******************* F U N C I O N E S   U T I L I T A R I A S *******************/

function CargarMarcas(idMarcaSeleccionar) {

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
                templateMarcas += "<option value='" + iter.Id + "' " + (iter.Id == idMarcaSeleccionar ? 'selected' : '') + ">" + iter.Descripcion + "</option>";
            });
            ddlMarca.append(templateMarcas).prop('disabled', false); // llenar ddl marcas

            idMarcaSeleccionada = idMarcaSeleccionar;
            marcaSeleccionada = $("#ddlMarca option:selected").text();

            $(".lblMarca").text(marcaSeleccionada);

            CargarModelos(idMarcaSeleccionar, 0);

            $("#modalSeleccionarPrecioDeMercado").modal(); // mostrar modal
        }
    });
}

function CargarModelos(idMarca, idModeloSeleccionar) {

    var ddlModelo = $('#ddlModelo');
    var ddlAnio = $('#ddlAnio');

    if (idMarca != '' && idMarca != 0) { // si se seleccionó una marca

        ddlModelo.empty().append("<option value=''>Seleccione una opción</option>").prop('disabled', true); // reiniciar ddl de modelos
        ddlAnio.empty().append("<option value=''>Seleccione un modelo</option>").prop('disabled', true); // reiniciar ddl de años

        $("#btnAgregarModelo").prop('disabled', false);

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
                    templateModelos += "<option value='" + iter.Id + "' " + (iter.Id == idModeloSeleccionar ? 'selected' : '') + ">" + iter.Descripcion + "</option>";
                });

                ddlModelo.append(templateModelos).prop('disabled', false); // Agregar modelos de la marca seleccionada al ddl de modelos

                idModeloSeleccionado = idModeloSeleccionar;
                modeloSeleccionado = $("#ddlModelo option:selected").text();

                $(".lblModelo").text(modeloSeleccionado);

                CargarAniosDisponiblesPorModelo(idModeloSeleccionado, 0);
            }
        });
    }
    else {
        ddlModelo.empty().append("<option value=''>Seleccione una marca</option>").prop('disabled', true); // reiniciar ddl de modelos
        ddlAnio.empty().append("<option value=''>Seleccione un modelo</option>").prop('disabled', true); // reiniciar ddl de años

        $('#btnSeleccionarPrecioDeMercadoConfirmar,#btnSolicitarPrecioDeMercado').prop('disabled', true); // deshabilitar opciones
        $('#txtPrecioDeMercadoActual').val(''); // reiniciar precio de mercado

        $("#btnAgregarModelo,#btnAgregarModeloAnio").prop('disabled', true);
    }
}

function CargarAniosDisponiblesPorModelo(idModelo, idModeloAnioSeleccionar) {

    var ddlAnio = $('#ddlAnio');

    if (idModelo != '' && idModelo != 0) { // si se seleccionó un modelo

        ddlAnio.empty().append("<option value=''>Seleccione una opción</option>").prop('disabled', true); // reiniciar ddl de años

        $("#btnAgregarModeloAnio").prop('disabled', false);

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
                    templateAnios += "<option value='" + iter.Id + "' " + (iter.Id == idModeloAnioSeleccionar ? 'selected' : '') + ">" + iter.Descripcion + "</option>";
                });
                ddlAnio.append(templateAnios).prop('disabled', false); // agregar lista de años al ddl de años

                idModeloAnioSeleccionado = idModeloAnioSeleccionar;
                modeloAnioSeleccionado = $("#ddlAnio option:selected").text();

                $(".lblAnio").text(modeloSeleccionado);

                CargarPrecioDeMercadoPorIdModeloAnio(idModeloAnioSeleccionar);
            }
        });
    }
    else {
        ddlAnio.empty().append("<option value=''>Seleccione un modelo</option>").prop('disabled', true); // reiniciar ddl de años
        $('#btnSeleccionarPrecioDeMercadoConfirmar,#btnSolicitarPrecioDeMercado').prop('disabled', true); // deshabilitar opciones
        $('#txtPrecioDeMercadoActual').val(''); // reiniciar precios de mercado

        $("#btnAgregarModeloAnio").prop('disabled', true);
    }
}

function CargarPrecioDeMercadoPorIdModeloAnio(idModeloAnio) {

    if (idModeloAnio != '' && idModeloAnio != 0) {

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarPrecioDeMercadoPorIdModeloAnio",
            data: JSON.stringify({ idModeloAnio: idModeloAnio, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar el precio de mercado actual de la garantía seleccionada, contacte al administrador.');
            },
            success: function (data) {

                if (data.d.IdPrecioDeMercado != 0) {

                    debugger;


                    if (data.d.IdEstado == 1) {
                        $('#txtPrecioDeMercadoActual').val(data.d.PrecioDeMercado); // Setear precio de mercado actual del marca-modelo-año seleccionado
                        $('#btnSeleccionarPrecioDeMercadoConfirmar').prop('disabled', false).prop('title',''); // habilitar opciones
                        $('#btnSolicitarPrecioDeMercado').prop('disabled', true).prop('title', ''); // habilitar opciones
                    }
                    else if (data.d.IdEstado == 0) {

                        $('#txtPrecioDeMercadoActual').val(data.d.PrecioDeMercado);
                        $('#btnSolicitarPrecioDeMercado,#btnSeleccionarPrecioDeMercadoConfirmar').prop('disabled', true).prop('title','Este precio de mercado todavía no ha sido aprobado, espere la resolución del departamento de crédito.'); // habilitar opciones
                        MensajeAdvertencia('Este precio de mercado todavía no ha sido aprobado, espere la resolución del departamento de crédito.')
                    }
                }
                else {
                    $('#txtPrecioDeMercadoActual').val('');
                    $('#btnSolicitarPrecioDeMercado').prop('disabled', false).prop('title', ''); // habilitar opciones
                    $('#btnSeleccionarPrecioDeMercadoConfirmar').prop('disabled', true).prop('title', ''); // deshabilitar opciones
                    MensajeAdvertencia("No hay ningún precio de mercado asignado a la garantía seleccionada. <br /> <b>NOTA: Recuerda que puedes solicitar los precios de mercado.</b>");
                }

                idModeloAnioSeleccionado = idModeloAnio;
                modeloAnioSeleccionado = $("#ddlAnio option:selected").text();
            }
        });
    }
    else {
        $('#btnSeleccionarPrecioDeMercadoConfirmar,#btnSolicitarPrecioDeMercado').prop('disabled', true); // deshabilitar opciones
        $('#txtPrecioDeMercadoActual').val(''); // reiniciar precio de mercado
    }

}