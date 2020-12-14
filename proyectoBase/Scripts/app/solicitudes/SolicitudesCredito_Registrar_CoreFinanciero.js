COTIZADOR = null;

if (PRECALIFICADO.PermitirIngresarSolicitud == false && (PRECALIFICADO.MensajePermitirIngresarSolicitud != '' && PRECALIFICADO.MensajePermitirIngresarSolicitud != null)) {
    Swal.fire(
        {
            title: '¡Oh no!',
            text: PRECALIFICADO.MensajePermitirIngresarSolicitud,
            type: 'warning',
            showCancelButton: false,
            confirmButtonColor: "#58db83",
            confirmButtonText: "OMITIR"
        }
    )
}

var btnFinalizar = $('<button type="button" id="btnGuardarSolicitud"></button>').text('Finalizar').addClass('btn btn-info').css('display', 'none')
    .on('click', function () {

        var modelStateInformacionPrestamo = $('#frmSolicitud').parsley().isValid({ group: 'informacionPrestamo' });

        if (!modelStateInformacionPrestamo) {
            $('#frmSolicitud').parsley().validate({ group: 'informacionPrestamo', force: true });
        }

        var modelStateInformacionPersonal = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' });

        if (!modelStateInformacionPersonal) {
            $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
        }

        var modelStateInformacionDomicilio = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomicilio' });

        if (!modelStateInformacionDomicilio) {
            $('#frmSolicitud').parsley().validate({ group: 'informacionDomicilio', force: true });
        }

        var modelStateInformacionLaboral = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

        if (!modelStateInformacionLaboral) {
            $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
        }

        var modelStateInformacionConyugal = $("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal') == false ? true : $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });

        if (!modelStateInformacionConyugal) {
            $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
        }

        if (cantidadReferencias < CONSTANTES.CantidadMinimaDeReferenciasPersonales) {
            if (modelStateInformacionPrestamo == true && modelStateInformacionPersonal == true && modelStateInformacionDomicilio == true && modelStateInformacionLaboral == true && modelStateInformacionConyugal == true) {

                MensajeAdvertencia('Se requieren mínimo ' + CONSTANTES.CantidadMinimaDeReferenciasPersonales + ' referencias personales. Entre ellas 2 familiares.');
            }
        }

        if (modelStateInformacionPrestamo == true && modelStateInformacionPersonal == true && modelStateInformacionDomicilio == true && modelStateInformacionLaboral == true && modelStateInformacionConyugal == true && cantidadReferencias >= CONSTANTES.CantidadMinimaDeReferenciasPersonales /*&& PRECALIFICADO.PermitirIngresarSolicitud == true*/) {

            var solicitud = {
                IdCliente: CONSTANTES.IdCliente,
                ValorPrima: $("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''),
                ValorGlobal: $("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''),
                ValorSeleccionado: $("#txtValorFinanciar").val().replace(/,/g, '') == '' ? 0 : $("#txtValorFinanciar").val().replace(/,/g, ''),
                PlazoSeleccionado: $("#ddlPlazosDisponibles option:selected").val() == '' ? 0 : $("#ddlPlazosDisponibles option:selected").val(),
                IdOrigen: $("#ddlOrigen option:selected").val() == null ? 1 : parseInt($("#ddlOrigen option:selected").val()),
                EnIngresoInicio: ConvertirFechaJavaScriptAFechaCsharp(localStorage.getItem("EnIngresoInicio")),
                IdTipoMoneda: $("#ddlMoneda option:selected").val()
            };

            var Cliente_InformacionConyugal = {};

            var requiereInformacionConyugal = $("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal');

            if (requiereInformacionConyugal == true) {

                Cliente_InformacionConyugal = {
                    fcIndentidadConyugue: $("#txtIdentidadConyugue").val(),
                    fcNombreCompletoConyugue: $("#txtNombresConyugue").val(),
                    fdFechaNacimientoConyugue: $("#txtFechaNacimientoConyugue").val(),
                    fcTelefonoConyugue: $("#txtTelefonoConyugue").val(),
                    fcLugarTrabajoConyugue: $("#txtLugarDeTrabajoConyuge").val(),
                    fcIngresosMensualesConyugue: $("#txtIngresosMensualesConyugue").val().replace(/,/g, '') == '' ? 0 : $("#txtIngresosMensualesConyugue").val().replace(/,/g, ''),
                    fcTelefonoTrabajoConyugue: $("#txtTelefonoTrabajoConyugue").val()
                };
            }

            var cliente = {
                IdCliente: CONSTANTES.IdCliente,
                RtnCliente: $("#txtRtnCliente").val(),
                IdNacionalidad: $("#ddlNacionalidad").val(),
                ProfesionOficio: $("#txtProfesion").val(),
                Correo: $("#txtCorreoElectronico").val(),
                Sexo: $("input[name='sexoCliente']:checked").val(),
                IdEstadoCivil: $("#ddlEstadoCivil :selected").val(),
                IdVivienda: $("#ddlTipoDeVivienda :selected").val(),
                IdTiempoResidir: $("#ddlTiempoDeResidir :selected").val(),
                IdTipoCliente: $("#ddlTipoDeCliente :selected").val(),
                TelefonoCliente: $("#txtNumeroTelefono").val(),

                InformacionDomicilio: {
                    TelefonoCasa: $("#txtTelefonoCasa").val(),
                    IdDepartamento: $("#ddlDepartamentoDomicilio :selected").val(),
                    IdMunicipio: $("#ddlMunicipioDomicilio :selected").val(),
                    IdCiudadPoblado: $("#ddlCiudadPobladoDomicilio :selected").val(),
                    IdBarrioColonia: $("#ddlBarrioColoniaDomicilio :selected").val(),
                    DireccionDetallada: $("#txtDireccionDetalladaDomicilio").val(),
                    ReferenciasDireccionDetallada: $("#txtReferenciasDelDomicilio").val()
                },
                InformacionLaboral: {
                    NombreTrabajo: $("#txtNombreDelTrabajo").val(),
                    PuestoAsignado: $("#txtPuestoAsignado").val(),
                    FechaIngreso: $("#txtFechaDeIngreso").val(),
                    TelefonoEmpresa: $("#txtTelefonoEmpresa").val(),
                    ExtensionRecursosHumanos: $("#txtExtensionRecursosHumanos").val().replace(/_/g, ''),
                    ExtensionCliente: $("#txtExtensionCliente").val().replace(/_/g, ''),
                    IdDepartamento: $("#ddlDepartamentoEmpresa :selected").val(),
                    IdMunicipio: $("#ddlMunicipioEmpresa :selected").val(),
                    IdCiudadPoblado: $("#ddlCiudadPobladoEmpresa :selected").val(),
                    IdBarrioColonia: $("#ddlBarrioColoniaEmpresa :selected").val(),
                    FuenteOtrosIngresos: $("#txtFuenteDeOtrosIngresos").val(),
                    ValorOtrosIngresos: $("#txtValorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#txtValorOtrosIngresos").val().replace(/,/g, ''),
                    DireccionDetalladaEmpresa: $("#txtDireccionDetalladaEmpresa").val(),
                    ReferenciasDireccionDetallada: $("#txtReferenciasEmpresa").val()
                },
                InformacionConyugal: Cliente_InformacionConyugal,
                ListaReferenciasPersonales: listaReferenciasPersonales
            };

            var garantia = null;

            if (CONSTANTES.RequiereGarantia == 1) {

                var valorMercado = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''));
                var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''));
                var valorFinanciado = valorMercado - valorPrima;

                garantia = {
                    VIN: $("#txtVIN").val(),
                    TipoDeGarantia: $("#txtTipoDeGarantia").val(),
                    TipoDeVehiculo: $("#txtTipoDeVehiculo").val(),
                    Marca: $("#txtMarca").val(),
                    Modelo: $("#txtModelo").val(),
                    Anio: $("#txtAnio").val().replace(/,/g, '') ?? 0,
                    Color: $("#txtColor").val(),
                    Matricula: $("#txtMatricula").val(),
                    Cilindraje: $("#txtCilindraje").val(),
                    Recorrido: $("#txtRecorrido").val().replace(/,/g, '') == '' ? 0 : $("#txtRecorrido").val().replace(/,/g, ''),
                    UnidadDeDistancia: $("#ddlUnidadDeMedida").val(),
                    Transmision: $("#txtTransmision").val(),
                    TipoDeCombustible: $("#txtTipoDeCombustible").val(),
                    SerieUno: $("#txtSerieUno").val(),
                    SerieDos: $("#txtSerieDos").val(),
                    SerieMotor: $("#txtSerieMotor").val(),
                    SerieChasis: $("#txtSerieChasis").val(),
                    GPS: $("#txtGPS").val(),
                    Comentario: $("#txtComentario").val(),
                    NumeroPrestamo: '',
                    esDigitadoManualmente: true,
                    ValorMercado: valorMercado,
                    ValorPrima: valorPrima,
                    ValorFinanciado: valorFinanciado,
                    GastosDeCierre: 0,

                    IdentidadPropietario: $("#txtIdentidadPropietario").val(),
                    NombrePropietario: $("#txtNombrePropietario").val(),
                    IdNacionalidadPropietario: $("#ddlNacionalidadPropietario :selected").val(),
                    IdEstadoCivilPropietario: $("#ddlEstadoCivilPropietario :selected").val(),

                    IdentidadVendedor: $("#txtIdentidadVendedor").val(),
                    NombreVendedor: $("#txtNombreVendedor").val(),
                    IdNacionalidadVendedor: $("#ddlNacionalidadVendedor :selected").val(),
                    IdEstadoCivilVendedor: $("#ddlEstadoCivilVendedor :selected").val()
                }
            };

            $.ajax({
                type: "POST",
                url: 'SolicitudesCredito_Registrar.aspx/IngresarSolicitud',
                data: JSON.stringify({ solicitud: solicitud, cliente: cliente, precalificado: PRECALIFICADO, garantia: garantia, esClienteNuevo: CONSTANTES.EsClienteNuevo, dataCrypt: window.location.href, cotizador: COTIZADOR }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError('No se guardó el registro, contacte al administrador');
                },
                success: function (data) {

                    if (data.d.response == true) {

                        MensajeExito(data.d.message);
                        localStorage.clear();
                        resetForm($("#frmSolicitud"));
                        $($('#smartwizard')).smartWizard("reset");
                    }
                    else {
                        MensajeError(data.d.message);
                    }
                }
            });
        }
    });

/* Inicalizar el Wizard */
$('#smartwizard').smartWizard({
    selected: 0,
    theme: 'default',
    transitionEffect: 'fade',
    showStepURLhash: false,
    autoAdjustHeight: false,
    toolbarSettings: {
        toolbarPosition: 'both',
        toolbarButtonPosition: 'end',
        toolbarExtraButtons: [btnFinalizar]
    },
    lang: {
        next: 'Siguiente',
        previous: 'Anterior'
    }
});

$(document).ready(function () {

    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {

        /* Si es el primer paso, deshabilitar el boton "anterior" */
        if (stepPosition === 'first') {
            $("#prev-btn").addClass('disabled').css('display', 'none');
        }
        else if (stepPosition === 'final') { /* Si es el ultimo paso, deshabilitar el boton siguiente */
            $("#next-btn").addClass('disabled');
            $("#btnGuardarSolicitud").css('display', '');
        }
        else { /* Si no es ninguna de las anteriores, habilitar todos los botones */
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
            $("#btnGuardarSolicitud").css('display', 'none');
        }
        if (stepNumber == 4) {
            $('#frmSolicitud').parsley().reset({ group: 'informacionConyugal', force: true }); /* Validar información conyugal del formulario cada vez que se muestre*/
        }
    });

    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');
    });

    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

        /* Si no requere informacion personal, saltarse esa pestaña */
        if ($("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal') == false) {

            $('#smartwizard').smartWizard("stepState", [(4 + numeroPestanaInformacionGarantia)], "hide");
        }
        else if ($("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal') == true) {

            $('#smartwizard').smartWizard("stepState", [(4 + numeroPestanaInformacionGarantia)], "show");
        }

        /* Validar solo si se quiere ir hacia el siguiente paso */
        if (stepDirection == 'forward') {

            if (stepNumber == 0) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPrestamo', excluded: ':disabled' }); /* Validar pestaña informacion prestamo */

                if (state == true) {
                    GuardarRespaldoInformacionPrestamo(); /* Si es valido, guardar respaldo en el localstorage */
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPrestamo', excluded: ':disabled', force: true }); /* Si no es valido, mostrar validaciones al usuario */
                }

                var valorGlobal = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''));
                var valorFinanciar = parseFloat($("#txtValorFinanciar").val().replace(/,/g, '') == '' ? 0 : $("#txtValorFinanciar").val().replace(/,/g, ''));
                var montoOfertadoSeleccionado = parseFloat($("#ddlPrestamosDisponibles :selected").val());
                var plazoSeleccionado = parseInt($("#ddlPrestamosDisponibles option:selected").data('plazoseleccionado') == '' ? 0 : $("#ddlPrestamosDisponibles option:selected").data('plazoseleccionado'));

                if (montoOfertadoSeleccionado < valorFinanciar) {

                    state = false;
                    MensajeAdvertencia('El monto del préstamo ofertado seleccionado no puede ser menor que el valor a Financiar');
                }

                if (montoOfertadoSeleccionado > valorFinanciar) {

                    state = false;
                    MensajeAdvertencia('El monto del préstamo ofertado seleccionado no puede ser mayor que el valor a Financiar');
                }

                if (CONSTANTES.RequierePrima == 1) {

                    var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''));

                    if (valorPrima >= valorGlobal) {

                        state = false;
                        MensajeError('El valor de la prima debe ser menor que el valor de la garantía');
                    }

                    if (CONSTANTES.PorcentajePrimaMinima != 0) {

                        if (valorPrima < ((valorGlobal * CONSTANTES.PorcentajePrimaMinima) / 100)) {

                            //state = false;
                            MensajeAdvertencia('El porcentaje de prima mínimo para este producto es: ' + CONSTANTES.PorcentajePrimaMinima + '%. Aumente el valor de la prima.');
                        }
                    }
                } /* if requiere prima */

                if (CONSTANTES.MontoFinanciarMinimo != 0) {

                    if (valorFinanciar < CONSTANTES.MontoFinanciarMinimo) {

                        //state = false;
                        MensajeAdvertencia('El monto mínimo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMinimo + '.');
                    }
                }

                if (CONSTANTES.MontoFinanciarMaximo != 0) {

                    if (valorFinanciar > CONSTANTES.MontoFinanciarMaximo) {

                        //state = false;
                        MensajeAdvertencia('El monto máximo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMaximo + '.');
                    }
                }

                if (CONSTANTES.PlazoMinimo != 0) {

                    if (plazoSeleccionado < CONSTANTES.PlazoMinimo) {

                        //state = false;
                        MensajeAdvertencia('El plazo mínimo a financiar para este producto es ' + CONSTANTES.PlazoMinimo + '.');
                    }
                }

                if (CONSTANTES.PlazoMaximo != 0) {

                    if (plazoSeleccionado > CONSTANTES.PlazoMaximo) {

                        //state = false;
                        MensajeAdvertencia('El plazo máximo a financiar para este producto es ' + CONSTANTES.PlazoMaximo + '.');
                    }
                }

                if (valorFinanciar > CONSTANTES.PrestamoMaximo_Monto && CONSTANTES.PrestamoMaximo_Monto != 0) {
                    //state = false;
                    MensajeAdvertencia('El monto máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Monto + '.');
                }

                //if (plazoSeleccionado > CONSTANTES.PrestamoMaximo_Plazo && CONSTANTES.PrestamoMaximo_Plazo != null) {
                // //state = false;
                // MensajeAdvertencia('El plazo máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Plazo + '.');
                //}

                if (PRECALIFICADO.PermitirIngresarSolicitud == false && PRECALIFICADO.MensajePermitirIngresarSolicitud != null) {
                    MensajeExito(PRECALIFICADO.MensajePermitirIngresarSolicitud);
                    //state = false;
                }

                return state;
            }

            if (stepNumber == 1 && CONSTANTES.RequiereGarantia == 1 && numeroPestanaInformacionGarantia != 0) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionGarantia', excluded: ':disabled' });

                if (state == true) {
                    GuardarRespaldoInformacionGarantia();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionGarantia', force: true });
                }

                return state;
            }

            /* Validar pestaña de la informacion personal del cliente */
            if (stepNumber == (1 + numeroPestanaInformacionGarantia)) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal', excluded: ':disabled' });

                if (state == true) {
                    GuardarRespaldoInformacionPersonal();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
                }

                if (PRECALIFICADO.PermitirIngresarSolicitud == false && PRECALIFICADO.MensajePermitirIngresarSolicitud != null) {
                    MensajeAdvertencia(PRECALIFICADO.MensajePermitirIngresarSolicitud);
                    //state = false;
                }
                return state;
            }

            /* Validar pestaña de la informacion de domicilio del cliente */
            if (stepNumber == (2 + numeroPestanaInformacionGarantia)) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomicilio' });

                if (state == true) {
                    GuardarRespaldoinformacionDomicilio();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionDomicilio', force: true });
                }

                if (PRECALIFICADO.PermitirIngresarSolicitud == false && PRECALIFICADO.MensajePermitirIngresarSolicitud != null) {
                    MensajeAdvertencia(PRECALIFICADO.MensajePermitirIngresarSolicitud);
                    //state = false;
                }
                return state;
            }

            /* Validar pestaña de la informacion laboral */
            if (stepNumber == (3 + numeroPestanaInformacionGarantia)) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

                if (state == true) {
                    GuardarRespaldoInformacionLaboral();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
                }

                if (PRECALIFICADO.PermitirIngresarSolicitud == false && PRECALIFICADO.MensajePermitirIngresarSolicitud != null) {
                    MensajeAdvertencia(PRECALIFICADO.MensajePermitirIngresarSolicitud);
                    //state = false;
                }
                return state;
            }

            /* Validar pestaña de la informacion del cónyugue */
            if (stepNumber == (4 + numeroPestanaInformacionGarantia)) {

                if ($("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal') == true) {

                    var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });

                    if (state == true) {
                        GuardarRespaldoInformacionConyugal();
                    }
                    else {
                        $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
                    }

                    if (PRECALIFICADO.PermitirIngresarSolicitud == false && PRECALIFICADO.MensajePermitirIngresarSolicitud != null) {
                        MensajeAdvertencia(PRECALIFICADO.MensajePermitirIngresarSolicitud);
                        //state = false;
                    }
                    return state;
                }
            }

            /* Validar referencias personales del cliente */
            if (stepNumber == (5 + numeroPestanaInformacionGarantia)) {

                var state = false;

                /* Si la lista de referencias no es nula y contiene una o mas referencias, guardar un respaldo de estas */
                if (listaReferenciasPersonales != null) {

                    if (listaReferenciasPersonales.length > 0) {

                        GuardarRespaldoReferenciasPersonales();

                        if (listaReferenciasPersonales.length >= 4) {
                            state = true;
                        }
                        else {
                            MensajeError('La cantidad mínima de referencias es 4, entre ellos 2 familiares');
                        }
                    }
                    else {
                        MensajeError('La cantidad mínima de referencias es 4, entre ellos 2 familiares');
                    }
                }

                if (PRECALIFICADO.PermitirIngresarSolicitud == false && PRECALIFICADO.MensajePermitirIngresarSolicitud != null) {
                    MensajeAdvertencia(PRECALIFICADO.MensajePermitirIngresarSolicitud);
                    //state = false;
                }

                return state;
            }
        } /* if foward */
    });

    CargarDocumentosRequeridos();

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

    /* Verificar que no hayan respaldos anteriores de solicitudes de clientes diferentes al actual, si las identidades no coinciden, quiere decir que son clientes diferentes */
    /* Entonces se borraran los respaldos anteriores y se iniciará el proceso de ingreso como una solicitud completamento nueva */
    var respaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));

    if (respaldoInformacionPrestamo != null) {

        if (CONSTANTES.IdentidadCliente == respaldoInformacionPrestamo.txtIdentidadCliente) {
            RecuperarRespaldos();
        }
        else {
            localStorage.clear();
        }
    }

    /* Guardar hora en el que se inicia el registro de la solicitud */
    if (localStorage.getItem("EnIngresoInicio") == null || localStorage.getItem("EnIngresoInicio") == "null") {

        localStorage.setItem("EnIngresoInicio", new Date(CONSTANTES.HoraAlCargar));
    }
});

/* Agregar referencia personal (abrir modal) */
$("#btnNuevaReferencia").click(function () {

    $("#txtNombreReferencia,#txtTelefonoReferencia,#ddlTiempoDeConocerReferencia, #ddlParentescos, #txtLugarTrabajoReferencia").val('');
    $('#modalAgregarReferenciaPersonal').parsley().reset();
    $("#modalAgregarReferenciaPersonal").modal("show");
});

/* Agregar referencia personal a la tabla y al listado de referencias personales */
var cantidadReferencias = 0;
var listaReferenciasPersonales = [];
$('#btnAgregarReferenciaTabla').on('click', function (e) {

    /* Validar formulario de agregar referencia personal */
    if ($('#frmSolicitud').parsley().isValid({ group: 'referenciasPersonales' })) {

        var NombreCompletoReferencia = $("#txtNombreReferencia").val();
        var TelefonoReferencia = $("#txtTelefonoReferencia").val();
        var LugarTrabajoReferencia = '-';
        var IdTiempoConocerReferencia = $("#ddlTiempoDeConocerReferencia :selected").val();
        var tiempoDeConocerReferenciaDescripcion = $("#ddlTiempoDeConocerReferencia :selected").text();
        var IdParentescoReferencia = $("#ddlParentescos :selected").val();
        var parentescoReferenciaDescripcion = $("#ddlParentescos :selected").text();
        var btnQuitarReferencia = '<button type="button" id="btnQuitarReferenciaPersonal" ' +
            'data-nombreReferencia="' + NombreCompletoReferencia + '" data-telefonoReferencia="' + TelefonoReferencia + '"' +
            'data-tiempoDeConocerReferencia="' + IdTiempoConocerReferencia + '" data-tiempoDeConocerReferenciaDescripcion="' + tiempoDeConocerReferenciaDescripcion + '" data-parentescoReferencia="' + IdParentescoReferencia + '" data-parentescoReferenciaDescripcion="' + parentescoReferenciaDescripcion + '"' +
            'class="btn btn-sm btn-danger" ><i class="far fa-trash-alt"></i> Quitar</button > ';

        /* Agregar referencia a la tabla de referencias personales */
        var row = '<tr><td>' + NombreCompletoReferencia + '</td><td>' + TelefonoReferencia + '</td><td>' + tiempoDeConocerReferenciaDescripcion + '</td><td>' + parentescoReferenciaDescripcion + '</td><td class="text-center">' + btnQuitarReferencia + '</td></tr>';
        $("#tblReferenciasPersonales tbody").append(row);

        $("#modalAgregarReferenciaPersonal").modal('hide');

        cantidadReferencias++;

        /* Objeto referencia */
        var referencia = {
            NombreCompletoReferencia: NombreCompletoReferencia,
            TelefonoReferencia: TelefonoReferencia,
            LugarTrabajoReferencia: '-',
            IdTiempoConocerReferencia: IdTiempoConocerReferencia,
            tiempoDeConocerReferenciaDescripcion: tiempoDeConocerReferenciaDescripcion,
            IdParentescoReferencia: IdParentescoReferencia,
            parentescoReferenciaDescripcion: parentescoReferenciaDescripcion
        }

        /* Agregar objeto referencia a la lista de referencias personales que se enviará al servidor */
        listaReferenciasPersonales.push(referencia);
    }
    else {
        $('#frmSolicitud').parsley().validate({ group: 'referenciasPersonales', force: true });
    }
});

/* Quitar referencia personal de la tabla */
$(document).on('click', 'button#btnQuitarReferenciaPersonal', function () {

    $(this).closest('tr').remove();

    var referenciaPersonal = {
        NombreCompletoReferencia: $(this).data('nombrereferencia'),
        TelefonoReferencia: $(this).data('telefonoreferencia'),
        LugarTrabajoReferencia: '-',
        IdTiempoConocerReferencia: $(this).data('tiempodeconocerreferencia').toString(),
        tiempoDeConocerReferenciaDescripcion: $(this).data('tiempodeconocerreferenciadescripcion'),
        IdParentescoReferencia: $(this).data('parentescoreferencia').toString(),
        parentescoReferenciaDescripcion: $(this).data('parentescoreferenciadescripcion'),
    };

    var list = [];

    if (listaReferenciasPersonales.length > 0) {

        for (var i = 0; i < listaReferenciasPersonales.length; i++) {

            var iter = {
                NombreCompletoReferencia: listaReferenciasPersonales[i].NombreCompletoReferencia,
                TelefonoReferencia: listaReferenciasPersonales[i].TelefonoReferencia,
                LugarTrabajoReferencia: '-',
                IdTiempoConocerReferencia: listaReferenciasPersonales[i].IdTiempoConocerReferencia.toString(),
                tiempoDeConocerReferenciaDescripcion: listaReferenciasPersonales[i].tiempoDeConocerReferenciaDescripcion,
                IdParentescoReferencia: listaReferenciasPersonales[i].IdParentescoReferencia.toString(),
                parentescoReferenciaDescripcion: listaReferenciasPersonales[i].parentescoReferenciaDescripcion,
            };
            if (JSON.stringify(iter) != JSON.stringify(referenciaPersonal)) {
                list.push(iter);
            }
        }
    }
    listaReferenciasPersonales = list;
    cantidadReferencias -= 1;
});

/* Cargar los inputs de los documentos requeridos */
function CargarDocumentosRequeridos() {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarDocumentosRequeridos",
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            var LenguajeEspanol = {
                feedback: 'Arrastra y suelta los archivos aqui',
                feedback2: 'Arrastra y suelta los archivos aqui',
                drop: 'Arrastra y suelta los archivos aqui',
                button: 'Buscar archivos',
                confirm: 'Confirmar',
                cancel: 'Cancelar'
            }

            var Input = '<div class="bootstrap-filestyle input-group">' +
                '<span class="group-span-filestyle w-100" tabindex="0">' +
                '<label for="filestyle-1" class="btn btn-secondary btn-block">' +
                '<span class="icon-span-filestyle fas fa-folder-open">' +
                '</span>' +
                '<span class="buttonText">Subir archivos</span>' +
                '</label>' +
                '</span>' +
                '</div > ';

            var divDocumentacion = $("#DivDocumentacion");

            $.each(data.d, function (i, iter) {

                var IdInput = 'Documento' + iter.IdTipoDocumento;

                divDocumentacion.append("<div class='col-sm-2 mt-3 pr-1 pl-1'>" +
                    "<label class='form-label mb-1'>" + iter.DescripcionTipoDocumento + "</label>" +
                    "<form action='SolicitudesCredito_Registrar.aspx?type=upload&doc=" + iter.IdTipoDocumento + "' method='post' enctype='multipart/form-data'>" +
                    "<input id='" + IdInput + "' type='file' name='files' data-tipo='" + iter.IdTipoDocumento + "' />" +
                    "</form>" +
                    "</div");

                $('#' + IdInput + '').fileuploader({
                    inputNameBrackets: false,
                    changeInput: Input,
                    theme: 'dragdrop',
                    limit: iter.CantidadMaximaDoucmentos, // Limite de archivos a subir
                    maxSize: 10, // Peso máximo de todos los archivos seleccionado en megas (MB)
                    fileMaxSize: 2, // Peso máximo de un archivo
                    extensions: ['jpg', 'png', 'jpeg'],// Extensiones/formatos permitidos
                    upload: {
                        url: 'SolicitudesCredito_Registrar.aspx?type=upload&doc=' + iter.IdTipoDocumento,
                        data: null,
                        type: 'POST',
                        enctype: 'multipart/form-data',
                        start: true,
                        synchron: true,
                        beforeSend: null,
                        onSuccess: function (result, item) {
                            var data = {};
                            try {
                                data = JSON.parse(result);
                            } catch (e) {
                                data.hasWarnings = true;
                            }

                            /* Validar exito */
                            if (data.isSuccess && data.files[0]) {
                                item.name = data.files[0].name;
                                item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                            }

                            /* Validar si se produjo un error */
                            if (data.hasWarnings) {
                                for (var warning in data.warnings) {
                                    alert(data.warnings);
                                }
                                item.html.removeClass('upload-successful').addClass('upload-failed');
                                return this.onError ? this.onError(item) : null;
                            }

                            item.html.find('.fileuploader-action-remove').addClass('fileuploader-action-success');
                            setTimeout(function () {
                                item.html.find('.progress-bar2').fadeOut(400);
                            }, 400);
                        },
                        onError: function (item) {
                            var progressBar = item.html.find('.progress-bar2');

                            if (progressBar.length) {
                                progressBar.find('span').html(0 + "%");
                                progressBar.find('.fileuploader-progressbar .bar').width(0 + "%");
                                item.html.find('.progress-bar2').fadeOut(400);
                            }

                            item.upload.status != 'cancelled' && item.html.find('.fileuploader-action-retry').length == 0 ? item.html.find('.column-actions').prepend(
                                '<button type="button" class="fileuploader-action fileuploader-action-retry" title="Retry"><i class="fileuploader-icon-retry"></i></button>'
                            ) : null;
                        },
                        onProgress: function (data, item) {
                            var progressBar = item.html.find('.progress-bar2');

                            if (progressBar.length > 0) {
                                progressBar.show();
                                progressBar.find('span').html(data.percentage + "%");
                                progressBar.find('.fileuploader-progressbar .bar').width(data.percentage + "%");
                            }
                        },
                        onComplete: null,
                    },
                    onRemove: function (item) {
                        $.post('SolicitudesCredito_Registrar.aspx?type=remove', { file: item.name });
                    },
                    dialogs: {
                        alert: function (text) {
                            return iziToast.warning({
                                title: 'Atencion',
                                message: text
                            });
                        },
                        confirm: function (text, callback) {
                            confirm(text) ? callback() : null;
                        }
                    },
                    captions: $.extend(true, {}, $.fn.fileuploader.languages['es'], LenguajeEspanol)

                }); /* Termina fileUploader*/

            }); /* Termina .Each*/
        }
    }); /* Termina Ajax */
}

/* Cargar prestamos disponibles consultados en el cotizador */
function CalculoPrestamo(valorGlobal, valorPrima, plazo) {

    if (PRECALIFICADO.IdProducto == 202 || PRECALIFICADO.IdProducto == 203) {

        var lcSeguro = '';
        var lcGPS = '';
        var lcGastosdeCierre = '';

        if ($("#ddlTipoDeSeguro :selected").val() == "A - Full Cover") {
            lcSeguro = "1";
        }
        if ($("#ddlTipoDeSeguro :selected").val() == "B - Basico + Garantía") {
            lcSeguro = "2";
        }

        if ($("#ddlTipoDeSeguro :selected").val() == "C - Basico") {
            lcSeguro = "3";
        }

        lcGastosdeCierre = $("#ddlTipoGastosDeCierre :selected").val() == "Financiado" ? "1" : "0";

        if ($("#ddlGps :selected").val() == "Si - CPI") {
            lcGPS = "1";
        }
        if ($("#ddlGps :selected").val() == "Si - CableColor") {
            lcGPS = "2";
        }
        if ($("#ddlGps :selected").val() == "No") {
            lcGPS = "0";
        }

        if (lcSeguro != '' && lcGPS != '' && lcGastosdeCierre != '') {

            $.ajax({
                type: "POST",
                url: "SolicitudesCredito_Registrar.aspx/CalculoPrestamoVehiculo",
                data: JSON.stringify(
                    {
                        idProducto: PRECALIFICADO.IdProducto,
                        valorGlobal: valorGlobal,
                        valorPrima: valorPrima,
                        plazo: plazo,
                        scorePromedio: PRECALIFICADO.ScorePromedio,
                        tipoSeguro: lcSeguro,
                        tipoGps: lcGPS,
                        gastosDeCierreFinanciados: lcGastosdeCierre,
                        dataCrypt: window.location.href
                    }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {

                    MensajeError('No se pudo realizar el cálculo del préstamo, contacte al administrador');
                },
                success: function (data) {

                    var objCalculo = data.d;

                    $("#txtValorCuota").val(objCalculo.CuotaTotal);
                    $("#txtValorFinanciar").val(objCalculo.TotalAFinanciar);

                    COTIZADOR = objCalculo;
                }
            });
        }
    }
    else {

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CalculoPrestamo",
            data: JSON.stringify({ idProducto: PRECALIFICADO.IdProducto, valorGlobal: valorGlobal, valorPrima: valorPrima, plazo: plazo, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('No se pudo realizar el cálculo del préstamo, contacte al administrador');
            },
            success: function (data) {

                var objCalculo = data.d;

                $("#txtValorCuota").val(objCalculo.CuotaTotal);
                $("#txtValorFinanciar").val(objCalculo.TotalAFinanciar);

                COTIZADOR = objCalculo;
            }
        });
    }
}

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

/* Validar si el estado civil seleccionado requiere información conyugal */
$("#ddlEstadoCivil").change(function () {

    var requiereInformacionConyugal = $("#ddlEstadoCivil :selected").data('requiereinformacionconyugal');

    if (requiereInformacionConyugal == false) {

        $('input.infoConyugal').attr('disabled', true);
        $('#smartwizard').smartWizard("stepState", [(4 + numeroPestanaInformacionGarantia)], "hide");// Si no se requiere información conyugal, deshabilitar ese formulario
    }
    else if (requiereInformacionConyugal == true) {

        $('input.infoConyugal').attr('disabled', false);
        $('#smartwizard').smartWizard("stepState", [(4 + numeroPestanaInformacionGarantia)], "show");// Si se requiere información conyugal, habilitar ese formulario
    }
});

$("select").on('change', function () {
    $(this).parsley().validate();
});

$('#txtValorGlobal,#txtValorPrima,#ddlPlazosDisponibles,#ddlTipoGastosDeCierre,#ddlTipoDeSeguro,#ddlGps').blur(function () {

    var valorGlobal = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''));
    var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''));
    var plazo = parseInt($("#ddlPlazosDisponibles option:selected").val() == '' ? 0 : $("#ddlPlazosDisponibles option:selected").val());
    var valorFinanciar = valorGlobal - valorPrima;
    var state = true;

    if (CONSTANTES.RequierePrima == 1) {

        if (valorPrima >= valorGlobal) {

            state = false;
            MensajeError('El valor de la prima debe ser menor que el valor de la garantía');
        }

        if (CONSTANTES.PorcentajePrimaMinima != 0) {

            if (valorPrima < ((valorGlobal * CONSTANTES.PorcentajePrimaMinima) / 100)) {

                //state = false;
                MensajeAdvertencia('El porcentaje de prima mínimo para este producto es: ' + CONSTANTES.PorcentajePrimaMinima + '%. Aumente el valor de la prima.');
            }
        }
    } /* if requiere prima */

    if (CONSTANTES.MontoFinanciarMinimo != 0) {

        if (valorFinanciar < CONSTANTES.MontoFinanciarMinimo) {

            //state = false;
            MensajeAdvertencia('El monto mínimo a financiar es ' + CONSTANTES.MontoFinanciarMinimo + '.');
        }
    }

    if (CONSTANTES.MontoFinanciarMaximo != 0) {

        if (valorFinanciar > CONSTANTES.MontoFinanciarMaximo) {

            //state = false;
            MensajeAdvertencia('El monto máximo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMaximo + '.');
        }
    }

    if (valorFinanciar > CONSTANTES.PrestamoMaximo_Monto && CONSTANTES.PrestamoMaximo_Monto != 0) {
        //state = false;
        MensajeAdvertencia('El monto máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Monto + '.');
    }

    if (valorGlobal > 0 && plazo > 0 && state == true) {
        CalculoPrestamo(valorGlobal.toString(), valorPrima.toString(), plazo.toString());
    }
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
            url: "SolicitudesCredito_Registrar.aspx/CargarListaMunicipios",
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
            url: "SolicitudesCredito_Registrar.aspx/CargarListaCiudadesPoblados",
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
            url: "SolicitudesCredito_Registrar.aspx/CargarListaBarriosColonias",
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

function resetForm($form) {
    $form.find('input:text, input:password, input:file,input[type="date"],input[type="email"], select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .removeAttr('checked').removeAttr('selected');
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

function GuardarRespaldoInformacionPrestamo() {

    var respaldoInformacionPrestamo = {
        txtIdentidadCliente: $("#txtIdentidadCliente").val(),
        txtRtnCliente: $("#txtRtnCliente").val(),
        txtValorGlobal: $("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''),
        txtValorPrima: $("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''),
        ddlPlazosDisponibles: $("#ddlPlazosDisponibles option:selected").val() == '' ? 0 : $("#ddlPlazosDisponibles option:selected").val(),
        ddlMoneda: $("#ddlMoneda :selected").val(),
        ddlOrigen: $("#ddlOrigen :selected").val(),

        /* Parametros de cotizador de vehiculos */
        ddlTipoGastosDeCierre: $("#ddlTipoGastosDeCierre :selected").val(),
        ddlTipoDeSeguro: $("#ddlTipoDeSeguro :selected").val(),
        ddlGps: $("#ddlGps :selected").val()
    }
    localStorage.setItem('RespaldoInformacionPrestamo', JSON.stringify(respaldoInformacionPrestamo));
}

function GuardarRespaldoInformacionPersonal() {

    var respaldoInformacionPersonal = {

        ddlNacionalidad: $("#ddlNacionalidad").val(),
        txtProfesion: $("#txtProfesion").val(),
        ddlEstadoCivil: $("#ddlEstadoCivil :selected").val(),
        ddlTipoDeCliente: $("#ddlTipoDeCliente :selected").val(),
        sexoCliente: $("input[name='sexoCliente']:checked").val(),
        txtCorreoElectronico: $("#txtCorreoElectronico").val(),
    }
    localStorage.setItem('RespaldoInformacionPersonal', JSON.stringify(respaldoInformacionPersonal));
}

function GuardarRespaldoinformacionDomicilio() {

    var respaldoinformacionDomicilio = {

        ddlDepartamentoDomicilio: $("#ddlDepartamentoDomicilio :selected").val(),
        ddlMunicipioDomicilio: $("#ddlMunicipioDomicilio :selected").val(),
        ddlCiudadPobladoDomicilio: $("#ddlCiudadPobladoDomicilio :selected").val(),
        ddlBarrioColoniaDomicilio: $("#ddlBarrioColoniaDomicilio :selected").val(),
        txtTelefonoCasa: $("#txtTelefonoCasa").val(),
        txtDireccionDetalladaDomicilio: $("#txtDireccionDetalladaDomicilio").val(),
        txtReferenciasDelDomicilio: $("#txtReferenciasDelDomicilio").val(),

        ddlTipoDeVivienda: $("#ddlTipoDeVivienda :selected").val(),
        ddlTiempoDeResidir: $("#ddlTiempoDeResidir :selected").val()
    }
    localStorage.setItem('RespaldoinformacionDomicilio', JSON.stringify(respaldoinformacionDomicilio));
}

function GuardarRespaldoInformacionLaboral() {

    var respaldoInformacionLaboral = {

        txtNombreDelTrabajo: $("#txtNombreDelTrabajo").val(),
        txtFechaDeIngreso: $("#txtFechaDeIngreso").val(),
        txtPuestoAsignado: $("#txtPuestoAsignado").val(),
        txtTelefonoEmpresa: $("#txtTelefonoEmpresa").val(),
        txtExtensionRecursosHumanos: $("#txtExtensionRecursosHumanos").val(),
        txtExtensionCliente: $("#txtExtensionCliente").val(),
        txtFuenteDeOtrosIngresos: $("#txtFuenteDeOtrosIngresos").val(),
        txtValorOtrosIngresos: $("#txtValorOtrosIngresos").val().replace(/,/g, ''),
        ddlDepartamentoEmpresa: $("#ddlDepartamentoEmpresa :selected").val(),
        ddlMunicipioEmpresa: $("#ddlMunicipioEmpresa :selected").val(),
        ddlCiudadPobladoEmpresa: $("#ddlCiudadPobladoEmpresa :selected").val(),
        ddlBarrioColoniaEmpresa: $("#ddlBarrioColoniaEmpresa :selected").val(),
        txtDireccionDetalladaEmpresa: $("#txtDireccionDetalladaEmpresa").val(),
        txtReferenciasEmpresa: $("#txtReferenciasEmpresa").val()
    }
    localStorage.setItem('RespaldoInformacionLaboral', JSON.stringify(respaldoInformacionLaboral));
}

function GuardarRespaldoInformacionConyugal() {

    var respaldoInformacionConyugal = {
        txtIdentidadConyugue: $("#txtIdentidadConyugue").val(),
        txtNombresConyugue: $("#txtNombresConyugue").val(),
        txtFechaNacimientoConyugue: $("#txtFechaNacimientoConyugue").val(),
        txtTelefonoConyugue: $("#txtTelefonoConyugue").val(),
        txtLugarDeTrabajoConyuge: $("#txtLugarDeTrabajoConyuge").val(),
        txtIngresosMensualesConyugue: $("#txtIngresosMensualesConyugue").val().replace(/,/g, ''),
        txtTelefonoTrabajoConyugue: $("#txtTelefonoTrabajoConyugue").val()
    }
    localStorage.setItem('RespaldoInformacionConyugal', JSON.stringify(respaldoInformacionConyugal));
}

function GuardarRespaldoInformacionGarantia() {

    var respaldoInformacionGarantia = {
        txtVIN: $("#txtVIN").val(),
        txtTipoDeGarantia: $("#txtTipoDeGarantia").val(),
        txtTipoDeVehiculo: $("#txtTipoDeVehiculo").val(),
        txtMarca: $("#txtMarca").val(),
        txtModelo: $("#txtModelo").val(),
        txtAnio: $("#txtAnio").val().replace(/,/g, ''),
        txtColor: $("#txtColor").val(),
        txtMatricula: $("#txtMatricula").val(),
        txtCilindraje: $("#txtCilindraje").val().replace(/,/g, ''),
        txtRecorrido: $("#txtRecorrido").val().replace(/,/g, ''),
        ddlUnidadDeMedida: $("#ddlUnidadDeMedida :selected").val(),
        txtTransmision: $("#txtTransmision").val(),
        txtTipoDeCombustible: $("#txtTipoDeCombustible").val(),
        txtSerieUno: $("#txtSerieUno").val(),
        txtSerieMotor: $("#txtSerieMotor").val(),
        txtSerieChasis: $("#txtSerieChasis").val(),
        txtSerieDos: $("#txtSerieDos").val(),
        txtGPS: $("#txtGPS").val(),
        txtComentario: $("#txtComentario").val(),

        /* Informacion propietario de la garantia */
        txtIdentidadPropietario: $("#txtIdentidadPropietario").val(),
        txtNombrePropietario: $("#txtNombrePropietario").val(),
        ddlNacionalidadPropietario: $("#ddlNacionalidadPropietario :selected").val(),
        ddlEstadoCivilPropietario: $("#ddlEstadoCivilPropietario :selected").val(),

        /* Informacion vendedor de la garantia */
        txtIdentidadVendedor: $("#txtIdentidadVendedor").val(),
        txtNombreVendedor: $("#txtNombreVendedor").val(),
        ddlNacionalidadVendedor: $("#ddlNacionalidadVendedor :selected").val(),
        ddlEstadoCivilVendedor: $("#ddlEstadoCivilVendedor :selected").val()
    }
    localStorage.setItem('RespaldoInformacionGarantia', JSON.stringify(respaldoInformacionGarantia));
}

function GuardarRespaldoReferenciasPersonales() {

    localStorage.setItem('RespaldoReferenciasPersonales', JSON.stringify(listaReferenciasPersonales));
}

function RecuperarRespaldos() {

    //MostrarLoader();

    $(".buscardorddl").select2("destroy");

    /* Recuperar información de pestaña información del préstamo */
    if (localStorage.getItem('RespaldoInformacionPrestamo') != null) {

        var RespaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));

        $("#txtRtnCliente").val(RespaldoInformacionPrestamo.txtRtnCliente);
        $("#ddlOrigen").val(RespaldoInformacionPrestamo.ddlOrigen);
        var valorGlobal = parseFloat(RespaldoInformacionPrestamo.txtValorGlobal);
        var valorPrima = parseFloat(RespaldoInformacionPrestamo.txtValorPrima);
        var plazo = parseInt(RespaldoInformacionPrestamo.ddlPlazosDisponibles);

        $("#txtValorGlobal").val(valorGlobal);
        $("#txtValorPrima").val(valorPrima);
        $("#ddlPlazosDisponibles").val(plazo);
        $("#ddlMoneda").val(RespaldoInformacionPrestamo.ddlMoneda);

        /* Parametros de cotizador de vehiculos */
        $("#ddlTipoGastosDeCierre").val(RespaldoInformacionPrestamo.ddlTipoGastosDeCierre);
        $("#ddlTipoDeSeguro").val(RespaldoInformacionPrestamo.ddlTipoDeSeguro);
        $("#ddlGps").val(RespaldoInformacionPrestamo.ddlGps);

        if (valorGlobal > 0 && plazo > 0) {
            CalculoPrestamo(valorGlobal.toString(), valorPrima.toString(), plazo.toString());
        }
    }

    /* Recuperar respaldo de pestaña de informacion personal */
    if (localStorage.getItem('RespaldoInformacionPersonal') != null) {

        var respaldoInformacionPersonal = JSON.parse(localStorage.getItem('RespaldoInformacionPersonal'));

        $("#ddlNacionalidad").val(respaldoInformacionPersonal.ddlNacionalidad);
        $("#txtProfesion").val(respaldoInformacionPersonal.txtProfesion);
        $("#ddlEstadoCivil").val(respaldoInformacionPersonal.ddlEstadoCivil);
        $("#ddlTipoDeCliente").val(respaldoInformacionPersonal.ddlTipoDeCliente);
        $("input[name=sexoCliente][value=" + respaldoInformacionPersonal.sexoCliente + "]").prop('checked', true);
        $("#txtCorreoElectronico").val(respaldoInformacionPersonal.txtCorreoElectronico);
    }

    /* Recuperar resplado de pestaña de informacion de domicilio */
    if (localStorage.getItem('RespaldoinformacionDomicilio') != null) {

        var respaldoinformacionDomicilio = JSON.parse(localStorage.getItem('RespaldoinformacionDomicilio'));

        $("#ddlDepartamentoDomicilio").val(respaldoinformacionDomicilio.ddlDepartamentoDomicilio);
        CargarMunicipios(respaldoinformacionDomicilio.ddlDepartamentoDomicilio, 'Domicilio', false, respaldoinformacionDomicilio.ddlMunicipioDomicilio);
        CargarCiudadesPoblados(respaldoinformacionDomicilio.ddlDepartamentoDomicilio, respaldoinformacionDomicilio.ddlMunicipioDomicilio, 'Domicilio', false, respaldoinformacionDomicilio.ddlCiudadPobladoDomicilio);
        CargarBarriosColonias(respaldoinformacionDomicilio.ddlDepartamentoDomicilio, respaldoinformacionDomicilio.ddlMunicipioDomicilio, respaldoinformacionDomicilio.ddlCiudadPobladoDomicilio, 'Domicilio', respaldoinformacionDomicilio.ddlBarrioColoniaDomicilio);
        $("#txtTelefonoCasa").val(respaldoinformacionDomicilio.txtTelefonoCasa);
        $("#txtDireccionDetalladaDomicilio").val(respaldoinformacionDomicilio.txtDireccionDetalladaDomicilio);
        $("#txtReferenciasDelDomicilio").val(respaldoinformacionDomicilio.txtReferenciasDelDomicilio);

        $("#ddlTipoDeVivienda").val(respaldoinformacionDomicilio.ddlTipoDeVivienda);
        $("#ddlTiempoDeResidir").val(respaldoinformacionDomicilio.ddlTiempoDeResidir);
    }

    /* Recuperar informacion de pestaña de informacion laboral */
    if (localStorage.getItem('RespaldoInformacionLaboral') != null) {

        var respaldoInformacionLaboral = JSON.parse(localStorage.getItem('RespaldoInformacionLaboral'));

        $("#txtNombreDelTrabajo").val(respaldoInformacionLaboral.txtNombreDelTrabajo);
        $("#txtFechaDeIngreso").val(respaldoInformacionLaboral.txtFechaDeIngreso);
        $("#txtPuestoAsignado").val(respaldoInformacionLaboral.txtPuestoAsignado);
        $("#txtTelefonoEmpresa").val(respaldoInformacionLaboral.txtTelefonoEmpresa);
        $("#txtExtensionRecursosHumanos").val(respaldoInformacionLaboral.txtExtensionRecursosHumanos);
        $("#txtExtensionCliente").val(respaldoInformacionLaboral.txtExtensionCliente);
        $("#txtFuenteDeOtrosIngresos").val(respaldoInformacionLaboral.txtFuenteDeOtrosIngresos);
        $("#txtValorOtrosIngresos").val(respaldoInformacionLaboral.txtValorOtrosIngresos);
        $("#ddlDepartamentoEmpresa").val(respaldoInformacionLaboral.ddlDepartamentoEmpresa);
        CargarMunicipios(respaldoInformacionLaboral.ddlDepartamentoEmpresa, 'Empresa', false, respaldoInformacionLaboral.ddlMunicipioEmpresa);
        CargarCiudadesPoblados(respaldoInformacionLaboral.ddlDepartamentoEmpresa, respaldoInformacionLaboral.ddlMunicipioEmpresa, 'Empresa', false, respaldoInformacionLaboral.ddlCiudadPobladoEmpresa);
        CargarBarriosColonias(respaldoInformacionLaboral.ddlDepartamentoEmpresa, respaldoInformacionLaboral.ddlMunicipioEmpresa, respaldoInformacionLaboral.ddlCiudadPobladoEmpresa, 'Empresa', respaldoInformacionLaboral.ddlBarrioColoniaEmpresa);
        $("#txtDireccionDetalladaEmpresa").val(respaldoInformacionLaboral.txtDireccionDetalladaEmpresa);
        $("#txtReferenciasEmpresa").val(respaldoInformacionLaboral.txtReferenciasEmpresa);
    }

    /* Recuperar informacion de pestaña de informacion conyugal */
    if (localStorage.getItem('RespaldoInformacionConyugal') != null) {

        var respaldoInformacionConyugal = JSON.parse(localStorage.getItem('RespaldoInformacionConyugal'));

        $("#txtIdentidadConyugue").val(respaldoInformacionConyugal.txtIdentidadConyugue);
        $("#txtNombresConyugue").val(respaldoInformacionConyugal.txtNombresConyugue);
        $("#txtFechaNacimientoConyugue").val(respaldoInformacionConyugal.txtFechaNacimientoConyugue);
        $("#txtTelefonoConyugue").val(respaldoInformacionConyugal.txtTelefonoConyugue);
        $("#txtLugarDeTrabajoConyuge").val(respaldoInformacionConyugal.txtLugarDeTrabajoConyuge);
        $("#txtIngresosMensualesConyugue").val(respaldoInformacionConyugal.txtIngresosMensualesConyugue);
        $("#txtTelefonoTrabajoConyugue").val(respaldoInformacionConyugal.txtTelefonoTrabajoConyugue);
    }
    else if ($("#ddlEstadoCivil :selected").data('requiereinformacionconyugal') == false) {

        $(".infoConyugal").prop('disabled', true);
    }

    /* Recuperar informacion de pestaña de informacion de la garantia */
    if (localStorage.getItem('RespaldoInformacionGarantia') != null) {

        var respaldoInformacionGarantia = JSON.parse(localStorage.getItem('RespaldoInformacionGarantia'));

        $("#txtVIN").val(respaldoInformacionGarantia.txtVIN);
        $("#txtTipoDeGarantia").val(respaldoInformacionGarantia.txtTipoDeGarantia);
        $("#txtTipoDeVehiculo").val(respaldoInformacionGarantia.txtTipoDeVehiculo);
        $("#txtMarca").val(respaldoInformacionGarantia.txtMarca);
        $("#txtModelo").val(respaldoInformacionGarantia.txtModelo);
        $("#txtAnio").val(respaldoInformacionGarantia.txtAnio);
        $("#txtColor").val(respaldoInformacionGarantia.txtColor);
        $("#txtMatricula").val(respaldoInformacionGarantia.txtMatricula);
        $("#txtCilindraje").val(respaldoInformacionGarantia.txtCilindraje);
        $("#txtRecorrido").val(respaldoInformacionGarantia.txtRecorrido);
        $("#ddlUnidadDeMedida").val(respaldoInformacionGarantia.ddlUnidadDeMedida);
        $("#txtTransmision").val(respaldoInformacionGarantia.txtTransmision);
        $("#txtTipoDeCombustible").val(respaldoInformacionGarantia.txtTipoDeCombustible);
        $("#txtSerieUno").val(respaldoInformacionGarantia.txtSerieUno);
        $("#txtSerieMotor").val(respaldoInformacionGarantia.txtSerieMotor);
        $("#txtSerieChasis").val(respaldoInformacionGarantia.txtSerieChasis);
        $("#txtSerieDos").val(respaldoInformacionGarantia.txtSerieDos);
        $("#txtGPS").val(respaldoInformacionGarantia.txtGPS);
        $("#txtComentario").val(respaldoInformacionGarantia.txtComentario);

        /* Informacion propietario de la garantia */
        $("#txtIdentidadPropietario").val(respaldoInformacionGarantia.txtIdentidadPropietario);
        $("#txtNombrePropietario").val(respaldoInformacionGarantia.txtNombrePropietario);
        $("#ddlNacionalidadPropietario").val(respaldoInformacionGarantia.ddlNacionalidadPropietario);
        $("#ddlEstadoCivilPropietario").val(respaldoInformacionGarantia.ddlEstadoCivilPropietario);

        /* Informacion vendedor de la garantia */
        $("#txtIdentidadVendedor").val(respaldoInformacionGarantia.txtIdentidadVendedor);
        $("#txtNombreVendedor").val(respaldoInformacionGarantia.txtNombreVendedor);
        $("#ddlNacionalidadVendedor").val(respaldoInformacionGarantia.ddlNacionalidadVendedor);
        $("#ddlEstadoCivilVendedor").val(respaldoInformacionGarantia.ddlEstadoCivilVendedor);
    }

    /* Recuperar respaldo de pestaña de referencias personales del cliente */
    if (localStorage.getItem('RespaldoReferenciasPersonales') != null) {

        RespaldolistaReferenciasPersonales = JSON.parse(localStorage.getItem('RespaldoReferenciasPersonales'));

        listaReferenciasPersonales = [];

        var btnQuitarReferencia = '';
        var referencia;
        var tablaReferenciasPersonales = $("#tblReferenciasPersonales tbody");

        if (RespaldolistaReferenciasPersonales.length > 0) {

            for (var i = 0; i < RespaldolistaReferenciasPersonales.length; i++) {

                referencia = RespaldolistaReferenciasPersonales[i];

                btnQuitarReferencia = '<button type="button" id="btnQuitarReferenciaPersonal" ' +
                    'data-nombreReferencia="' + referencia.NombreCompletoReferencia + '" data-telefonoReferencia="' + referencia.TelefonoReferencia + '" ' +
                    'data-tiempoDeConocerReferencia="' + referencia.IdTiempoConocerReferencia + '" data-tiempoDeConocerReferenciaDescripcion="' + referencia.tiempoDeConocerReferenciaDescripcion + '" data-parentescoReferencia="' + referencia.IdParentescoReferencia + '" data-parentescoReferenciaDescripcion="' + referencia.parentescoReferenciaDescripcion + '"' +
                    'class="btn btn-sm btn-danger"><i class="far fa-trash-alt"></i> Quitar</button > ';

                /* Agregar referencia a la tabla de referencias personales */
                var row = '<tr><td>' + referencia.NombreCompletoReferencia + '</td><td>' + referencia.TelefonoReferencia + '</td><td>' + referencia.tiempoDeConocerReferenciaDescripcion + '</td><td>' + referencia.parentescoReferenciaDescripcion + '</td><td class="text-center">' + btnQuitarReferencia + '</td></tr>';

                tablaReferenciasPersonales.append(row);

                console.log('Referencia localstorage');
                console.log(referencia);

                listaReferenciasPersonales.push(referencia);
                cantidadReferencias++;
            }
        }
    }

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

    //OcultarLoader();
}

$("#btnBuscarVIN").click(function () {

    BuscarVIN();
});

function BuscarVIN() {

    let idVIN = $("#txtBuscarVIN").val().trim();

    if (idVIN) {

        MostrarLoader();

        $.ajax({
            url: 'https://vpic.nhtsa.dot.gov/api/vehicles/DecodeVinValues/' + idVIN,
            type: 'Get',
            data: { format: "json" },
            success: function (data) {

                if (data.Results[0].ErrorCode != "0") {
                    MensajeError('Resultado de la búsqueda: ' + data.Results[0].ErrorText);
                }

                $("#txtVIN").val(data.Results[0].VIN);
                $("#txtMarca").val(data.Results[0].Make);
                $("#txtModelo").val(data.Results[0].Model);
                $("#txtTipoDeVehiculo").val(data.Results[0].VehicleType);
                $("#txtAnio").val(data.Results[0].ModelYear);
                $("#txtTipoDeVehiculo").val(data.Results[0].BodyClass);
                $("#txtCilindraje").val(data.Results[0].DisplacementL);
                $("#txtTipoDeCombustible").val(data.Results[0].FuelTypePrimary);
                $("#txtSerieUno").val(data.Results[0].Series);
                $("#txtTransmision").val(data.Results[0].TransmissionStyle);

                OcultarLoader();
            },
            error: function (data) {
                console.log('ocurrió un error: ' + data);

                OcultarLoader();
                MensajeError('No se pudo cargar la información de este VIN');
            }
        });
    }
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

function MostrarLoader() {

    $("#Loader").css('display', '');
}

function OcultarLoader() {

    $("#Loader").css('display', 'none');
}