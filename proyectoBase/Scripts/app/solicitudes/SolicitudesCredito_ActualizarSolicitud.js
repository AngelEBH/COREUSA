var idSolicitud = 0;
var idCliente = 0;

var btnFinalizar = $('<button id="btnActualizarSolicitud" type="button">Finalizar</button>').addClass('btn btn-info')
    .on('click', function () {

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_ActualizarSolicitud.aspx/FinalizarCondicionamientoSolicitud' + qString,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify({ dataCrypt: window.location.href }),
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('No se guardó el registro, contacte al administrador');
            },
            success: function (data) {

                if (data.d == "Solicitud actualizada correctamente") {
                    window.location = "SolicitudesCredito_Ingresadas.aspx" + qString;
                }
                else {
                    MensajeError(data.d);
                }
            }
        });
    });

$(document).ready(function () {

    $('#smartwizard').smartWizard({
        selected: 0,
        theme: 'default',
        transitionEffect: 'fade',
        showStepURLhash: false,
        autoAdjustHeight: false,
        toolbarSettings: {
            toolbarPosition: 'bottom',
            toolbarButtonPosition: 'end',
            toolbarExtraButtons: [btnFinalizar]
        },
        lang: {
            next: 'Siguiente',
            previous: 'Anterior'
        }
    });

    /* Cuando se muestre un step */
    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {

        if (stepPosition === 'first') { /* si es el primer paso, deshabilitar el boton "anterior" */
            $("#prev-btn").addClass('disabled').css('display', 'none');
        }
        else if (stepPosition === 'final') { /* si es el ultimo paso, deshabilitar el boton siguiente */
            $("#next-btn").addClass('disabled');
        }
        else { /* si no es ninguna de las anteriores, habilitar todos los botones */
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
        }
        if (stepNumber == 4) { /* Inicializar validaciones de los formularios */
            $('#frmSolicitud').parsley().reset();
        }
    });

    /* Habilitar boton siguiente al reiniciar steps */
    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');
    });

    /* Cuando se deja un paso (realizar validaciones) */
    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

        if ($("#ddlEstadoCivil :selected").data('info') == false) { /* si no requere informacion personal, saltarse esa pestaña */
            $('#smartwizard').smartWizard("stepState", [4], "hide");
        }

        /* validar solo si se quiere ir hacia el siguiente paso */
        if (stepDirection == 'forward') {

            if (stepNumber == 0) {

            }

            if (stepNumber == 1) { /* validar informacion personal */

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' });

                if (state == false) { /* si no es valido, mostrar validaciones al usuario */

                    $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
                }
                return state;
            }

            if (stepNumber == 2) { /* validar informacion domiciliar */

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomiciliar' });

                if (state == false) {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionDomiciliar', force: true });
                }
                return state;
            }

            if (stepNumber == 3) { /* validar informacion laboral */

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

                if (state == false) {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
                }
                return state;
            }

            if (stepNumber == 4) { /* validar informacion conyugal */

                if ($("input[name='estadoCivil']:checked").data('info') == true) {

                    var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });

                    if (state == false) {
                        $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
                    }
                    return state;
                }
            }

            if (stepNumber == 5) { /* validar referencias personales */

                var state = false;

                if (listaClientesReferencias != null) {
                    if (listaClientesReferencias.length > 0) {
                        state = true;
                    }
                }
                if (cantidadReferencias > 0) {
                    state = true;
                } else {
                    MensajeError('Las referencias personales son requeridas');
                }
                return state;
            }

        }
    });


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

//finalizar condicion
var prestamoSeleccionado = [];
var idCondicion = 0;
var button = [];
var seccionFormulario = '';

$(document).on('click', 'button#btnTerminarCondicion', function () {

    idCondicion = $(this).data('id');
    button = $(this);
    seccionFormulario = $(this).data('tipo') != 'Documentacion' ? $(this).data('seccion') : 'Documentacion';
    $("#modalDetails").modal('hide');
    $("#modalFinalizarCondicionLista").modal();
});

//confirmar actualizacion de los ingresos del cliente
$("#btnTerminarCondicionFinalizar").click(function () {

    var objSeccion = {};

    switch (seccionFormulario) {
        case "Correccion Informacion de la Solicitud":
            objSeccion = {
                fiIDSolicitud: idSolicitud,
                fiIDCliente: clienteID,
                fnPrima: $("#txtPrima").val().replace(/,/g, ''),
                fnValorGarantia: $("#txtValorVehiculo").val().replace(/,/g, '')
            }
            break;
        case "Correccion Informacion Personal":
            objSeccion = {
                fiIDCliente: clienteID,
                fcIdentidadCliente: $("#identidadCliente").val(),
                RTNCliente: $("#rtnCliente").val(),
                fcTelefonoCliente: $("#numeroTelefono").val(),
                fiNacionalidadCliente: $("#nacionalidad").val(),
                fdFechaNacimientoCliente: $("#fechaNacimiento").val(),
                fcCorreoElectronicoCliente: $("#correoElectronico").val(),
                fcProfesionOficioCliente: $("#profesion").val(),
                fcSexoCliente: $("input[name='sexo']:checked").val(),
                fiIDEstadoCivil: $("input[name='estadoCivil']:checked").val(),
                fiIDVivienda: $("#vivivenda").val(),
                fiTiempoResidir: $("input[name='tiempoResidir']:checked").val(),
                fcPrimerNombreCliente: $("#primerNombreCliente").val(),
                fcSegundoNombreCliente: $("#SegundoNombreCliente").val(),
                fcPrimerApellidoCliente: $("#primerApellidoCliente").val(),
                fcSegundoApellidoCliente: $("#segundoApellidoCliente").val()
            }
            break;
        case "Correccion Informacion Domiciliar":
            objSeccion = {
                fiIDCliente: clienteID,
                fiIDDepto: $("#departamento :selected").val(),
                fiIDMunicipio: $("#municipio :selected").val(),
                fiIDCiudad: $("#ciudad :selected").val(),
                fiIDBarrioColonia: $("#barrioColonia :selected").val(),

                fcTelefonoCasa: $("#telefonoCasa").val(),
                fcTelefonoMovil: $("#telefonoMovil").val(),
                fcDireccionDetallada: $("#direccionDetallada").val(),
                fcReferenciasDireccionDetallada: $("#referenciaDireccionDetallada").val()
            }
            break;
        case "Correccion Informacion Laboral":
            objSeccion = {
                fiIDCliente: clienteID,
                fcNombreTrabajo: $("#nombreDelTrabajo").val(),
                fiIngresosMensuales: $("#ingresosMensuales").val().replace(/,/g, ''),
                fcPuestoAsignado: $("#puestoAsignado").val(),
                fcFechaIngreso: $("#fechaIngreso").val(),
                fdTelefonoEmpresa: $("#telefonoEmpresa").val(),
                fcExtensionRecursosHumanos: $("#extensionRRHH").val(),
                fcExtensionCliente: $("#extensionCliente").val(),
                fiIDDepto: $("#departamentoEmpresa :selected").val(),
                fiIDMunicipio: $("#municipioEmpresa :selected").val(),
                fiIDCiudad: $("#ciudadEmpresa :selected").val(),
                fiIDBarrioColonia: $("#barrioColoniaEmpresa :selected").val(),
                fcDireccionDetalladaEmpresa: $("#direccionDetalladaEmpresa").val(),
                fcReferenciasDireccionDetallada: $("#referenciaDireccionDetalladaEmpresa").val(),
                fcFuenteOtrosIngresos: $("#fuenteOtrosIngresos").val(),
                fiValorOtrosIngresosMensuales: $("#valorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#valorOtrosIngresos").val().replace(/,/g, '')
            }
            break;
        case "Correccion Informacion Conyugal":
            objSeccion = {
                fiIDCliente: clienteID,
                fcNombreCompletoConyugue: $("#nombresConyugue").val() + ' ' + $("#apellidosConyugue").val(),
                fcIndentidadConyugue: $("#identidadConyugue").val(),
                fdFechaNacimientoConyugue: $("#fechaNacimientoConyugue").val(),
                fcTelefonoConyugue: $("#telefonoConyugue").val(),
                fcLugarTrabajoConyugue: $("#lugarTrabajoConyugue").val(),
                fcIngresosMensualesConyugue: $("#ingresoMensualesConyugue").val().replace(/,/g, ''),
                fcTelefonoTrabajoConyugue: $("#telefonoTrabajoConyugue").val()
            }
            break;
        case "Correccion Referencias":
            objSeccion = listaClientesReferencias;
            break;
        case "Documentacion":
            objSeccion = {};
            break;
    }
    objSeccion = JSON.stringify(objSeccion);

    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_ActualizarSolicitud.aspx/ActualizarCondicionamiento' + qString,
        data: JSON.stringify({ ID: idCondicion, seccionFormulario: seccionFormulario, objSeccion: objSeccion, idCliente: clienteID }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al finalizar condicionamiento');
        },
        success: function (data) {

            if (data.d == true) {

                ObtenerDetalles();

                $("#modalFinalizarCondicionLista,#modalDetails").modal('hide');
                MensajeExito('Condición finalizada correctamente');
                button.removeClass('text-warning').addClass('text-info');
                button.on('click', function () { return false; });
            }
            else {
                MensajeError('Error al finalizar condicionamiento');
                button.addClass('text-warning');
            }
        }
    });
});

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

function resetForm($form) {
    $form.find('input:text, input:password, input:file,input[type="date"],input[type="email"], select, textarea').val('');
    $form.find('input:radio, input:checkbox').prop('checked', false).removeAttr('selected');
}