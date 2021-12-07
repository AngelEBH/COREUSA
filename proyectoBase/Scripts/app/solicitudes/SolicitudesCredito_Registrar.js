 COTIZADOR = null;

if (PRECALIFICADO.PermitirIngresarSolicitud == false && (PRECALIFICADO.MensajePermitirIngresarSolicitud != '' && PRECALIFICADO.MensajePermitirIngresarSolicitud != null)) {
    Swal.fire(
        {
            title: '¡Advertencia!',
            text: PRECALIFICADO.MensajePermitirIngresarSolicitud,
            type: 'warning',
            showCancelButton: false,
            confirmButtonColor: "#58db83",
            confirmButtonText: "OMITIR"
        }
    )
}
var idSolicitud = 0;
var btnFinalizar = $('<button type="button" id="btnGuardarSolicitud"></button>').text('Finalizar').addClass('btn btn-info').css('display', 'none')
    .on('click', function () {

        /*var modelStateInformacionProducto = $('#frmSolicitud').parsley().isValid({ group: 'informacionProducto' });

        if (!modelStateInformacionProducto) {
            $('#frmSolicitud').parsley().validate({ group: 'informacionProducto', force: true });
        }*/

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
            var fcTipoCuota = "";
            var TipoCuota = $("#ddlFrecuencia option:selected").val();
            var garantia = null;
            if (TipoCuota == 1) {
                fcTipoCuota = "Catorcenal";
            }

            if (TipoCuota == 3) {
                fcTipoCuota = "Mensual";
            }

            if (TipoCuota == 10) {
                fcTipoCuota = "Semanal";
            }

            var plazo = 0;
               
            if (PRECALIFICADO.IdProducto == 100) {
               plazo = $("#txtPlazoFrecuencia");
            } else {
               plazo = $("#txtPlazosDisponibles").val().replace(/,/g, '') == '' ? 0 : $("#txtPlazosDisponibles").val().replace(/,/g, '');
            }
           

            var solicitud = {
                IdCliente: CONSTANTES.IdCliente,
                ValorPrima: $("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''),
                ValorGlobal: $("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''),
                ValorSeleccionado: $("#txtValorDePrestamo").val().replace(/,/g, '') == '' ? 0 : $("#txtValorDePrestamo").val().replace(/,/g, ''),
                PlazoSeleccionado: plazo,  // $("#txtPlazosDisponibles").val().replace(/,/g, '') == '' ? 0 : $("#txtPlazosDisponibles").val().replace(/,/g, ''),  
                IdOrigen: $("#ddlOrigen option:selected").val() == null ? 1 : parseInt($("#ddlOrigen option:selected").val()),
                EnIngresoInicio: ConvertirFechaJavaScriptAFechaCsharp(localStorage.getItem("EnIngresoInicio")),
                //IdTipoMoneda: $("#ddlMoneda option:selected").val()
                FechaContrato: $("#txtIniciodeContrato").val(),
                TipoCuota: fcTipoCuota,

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
                    IdDepartamento: 0, //$("#ddlDepartamentoDomicilio :selected").val(),
                   IdMunicipio:  0, //$("#ddlMunicipioDomicilio :selected").val(),
                    IdCiudadPoblado: 0, //$("#ddlCiudadPobladoDomicilio :selected").val(),
                    IdBarrioColonia: $("#ddlBarrioColoniaDomicilio :selected").val(),
                    DireccionDetallada: $("#txtReferenciasDelDomicilio").val()  ,    //$("#txtDireccionDetalladaDomicilio").val(),
                    ReferenciasDireccionDetallada: $("#txtReferenciasDelDomicilio").val()
                },
                InformacionLaboral: {
                    NombreTrabajo: $("#txtNombreDelTrabajo").val(),
                    PuestoAsignado: $("#txtPuestoAsignado").val(),
                    FechaIngreso: $("#txtFechaDeIngreso").val(),
                    TelefonoEmpresa: $("#txtTelefonoEmpresa").val(),
                    ExtensionRecursosHumanos:'' , // $("#txtExtensionRecursosHumanos").val().replace(/_/g, ''),
                    ExtensionCliente: '',//$("#txtExtensionCliente").val().replace(/_/g, ''),
                    IdDepartamento: 0, //$("#ddlDepartamentoEmpresa :selected").val(),
                    IdMunicipio: 0,//$("#ddlMunicipioEmpresa :selected").val(),
                    IdCiudadPoblado: 0,//$("#ddlCiudadPobladoEmpresa :selected").val(),
                    IdBarrioColonia: $("#ddlBarrioColoniaEmpresa :selected").val(),
                    FuenteOtrosIngresos: $("#txtFuenteDeOtrosIngresos").val(),
                    ValorOtrosIngresos: $("#txtValorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#txtValorOtrosIngresos").val().replace(/,/g, ''),
                    DireccionDetalladaEmpresa: $("#txtReferenciasEmpresa").val(), //$("#txtDireccionDetalladaEmpresa").val(),
                    ReferenciasDireccionDetallada: $("#txtReferenciasEmpresa").val()
                },
                InformacionConyugal: Cliente_InformacionConyugal,
                ListaReferenciasPersonales: listaReferenciasPersonales
            };
           
            if (CONSTANTES.RequiereGarantia == 1) {
                

                var valorMercado = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''));
                var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''));
                var valorFinanciado = valorMercado - valorPrima;
                var ValorCollateral = parseFloat($("#txtCuotaMaxima").val().replace(/,/g, '') == '' ? 0 : $("#txtCuotaMaxima").val().replace(/,/g, ''));
                var ValorLienholder = parseFloat($("#txtLienHolder").val().replace(/,/g, '') == '' ? 0 : $("#txtLienHolder").val().replace(/,/g, '')); 
               
                
  
                   

                garantia = {
                    VIN: $("#txtVIN").val(),
                    TipoDeGarantia: $("#txtTipoDeGarantia").val(),
                    TipoDeVehiculo: $("#txtTipoDeVehiculo :selected").val(),
                    Marca: $("#txtMarca").val(),
                    Modelo: $("#txtModelo").val(),
                    Anio: $("#txtAnio").val().replace(/,/g, '') == '' ? 0 : $("#txtAnio").val().replace(/,/g, ''),
                    Color: $("#txtColor").val(),
                    Matricula: $("#txtMatricula").val(),
                    Cilindraje: $("#txtCilindraje").val(),
                    Recorrido: $("#txtRecorrido").val().replace(/,/g, '') == '' ? 0 : $("#txtRecorrido").val().replace(/,/g, ''),
                    UnidadDeDistancia: $("#ddlUnidadDeMedida").val(),
                    Transmision: $("#txtTransmision :selected").val(),
                    TipoDeCombustible: $("#txtTipoDeCombustible :selected").val(),
                    SerieUno: '',//$("#txtSerieUno").val(),
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
                    IdEstadoCivilPropietario: 0, //$("#ddlEstadoCivilPropietario :selected").val(),
                    IdentidadVendedor: $("#txtIdentidadVendedor").val(),
                    NombreVendedor: $("#txtNombreVendedor").val(),
                    IdNacionalidadVendedor: $("#ddlNacionalidadVendedor :selected").val(),
                    IdEstadoCivilVendedor: 0 //$("#ddlEstadoCivilVendedor :selected").val()
                }
            };

            COTIZADOR = {
                CuotaTotal: $("#txtValorCuota").val().replace(/,/g, ''),
                TotalAFinanciar: $("#txtValorFinanciar").val().replace(/,/g, ''),
                TasaInteresAnual: $("#txtTasaDeInteresAnual").val().replace(/,/g, ''),
                CuotaDelPrestamo: $("#txtValorCuota").val().replace(/,/g, ''),
                TotalFinanciadoConIntereses: $("#txtValorFinanciar").val().replace(/,/g, ''),
                LienHolder: ValorLienholder,
                CuotaSegurodeVehiculo: ValorCollateral,
                //TipoCuota: fcTipoCuota,
            };
           
            $.ajax({
                type: "POST",
                url: 'SolicitudesCredito_Registrar.aspx/IngresarSolicitud',
                data: JSON.stringify({ solicitud: solicitud, cliente: cliente, precalificado: PRECALIFICADO, garantia: garantia, esClienteNuevo: CONSTANTES.EsClienteNuevo, dataCrypt: window.location.href, cotizador: COTIZADOR }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    debugger;
                    MensajeError('No se guardó el registro, contacte al administrador');
                },
                success: function (data) {

                    console.log(data);
                    if (data.d.ResultadoExitoso == true) {

                        MensajeExito(data.d.MensajeResultado);
                        localStorage.clear();
                        //ConfirmarAval(data.d.IdInsertado);
                        idSolicitud = data.d.IdInsertado;
                        //idSolicitud = 80;
                        ConfirmarAval();
                        resetForm($("#frmSolicitud"));
                        $($('#smartwizard')).smartWizard("reset");
                    }
                    else {
                        MensajeError(data.d.MensajeResultado);
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


$("#ddlFrecuencia").change(function () {

    $(this).parsley().validate();

    var fcTipoCuota = "";
    var TipoCuota = $("#ddlFrecuencia option:selected").val();
    var garantia = null;
    if (TipoCuota == 1) {
        fcTipoCuota = "Catorcenal";
    }

    if (TipoCuota == 3) {
        fcTipoCuota = "Mensual";
    }

    if (TipoCuota == 10) {
        fcTipoCuota = "Semanal";
    }

    $("#TxtPlazoFrecuencialSeleccionado").text("Plazo" + "" + "(" + fcTipoCuota + ")" );
   

});

$(document).ready(function () {

    $("#TxtPlazoFrecuencialSeleccionado").text("Plazo");

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
                var valorFinanciar = parseFloat($("#txtValorDePrestamo").val().replace(/,/g, '') == '' ? 0 : $("#txtValorDePrestamo").val().replace(/,/g, ''));
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

                            state = false;
                            MensajeAdvertencia('El porcentaje de prima mínimo para este producto es: ' + CONSTANTES.PorcentajePrimaMinima + '%. Aumente el valor de la prima.');
                        }
                    }
                } /* if requiere prima */

                if (CONSTANTES.MontoFinanciarMinimo != 0) {

                    if (valorFinanciar < CONSTANTES.MontoFinanciarMinimo) {

                        state = false;
                        MensajeAdvertencia('El monto mínimo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMinimo + '.');
                    }
                }

                if (CONSTANTES.MontoFinanciarMaximo != 0) {

                    if (valorFinanciar > CONSTANTES.MontoFinanciarMaximo) {

                        state = false;
                        MensajeAdvertencia('El monto máximo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMaximo + '.');
                    }
                }

                if (CONSTANTES.PlazoMinimo != 0) {

                    if (plazoSeleccionado < CONSTANTES.PlazoMinimo) {

                        state = false;
                        MensajeAdvertencia('El plazo mínimo a financiar para este producto es ' + CONSTANTES.PlazoMinimo + '.');
                    }
                }

                if (CONSTANTES.PlazoMaximo != 0) {

                    if (plazoSeleccionado > CONSTANTES.PlazoMaximo) {

                        state = false;
                        MensajeAdvertencia('El plazo máximo a financiar para este producto es ' + CONSTANTES.PlazoMaximo + '.');
                    }
                } 

                if (valorFinanciar > CONSTANTES.PrestamoMaximo_Monto && CONSTANTES.PrestamoMaximo_Monto != 0 && (PRECALIFICADO.IdProducto != 102 && PRECALIFICADO.IdProducto != 100)) {
                    state = false;
                    MensajeAdvertencia('El monto máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Monto + '.');
                }

                //if (plazoSeleccionado > CONSTANTES.PrestamoMaximo_Plazo && CONSTANTES.PrestamoMaximo_Plazo != null) {
                // //state = false;
                // MensajeAdvertencia('El plazo máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Plazo + '.');
                //}

                if (PRECALIFICADO.PermitirIngresarSolicitud == false && PRECALIFICADO.MensajePermitirIngresarSolicitud != null) {
                    MensajeAdvertencia(PRECALIFICADO.MensajePermitirIngresarSolicitud);
                    state = false;
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
                    state = false;
                }
                return state;
            }

            /* Validar pestaña de la informacion de domicilio del cliente */
            if (stepNumber == (2 + numeroPestanaInformacionGarantia)) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomicilio', excluded: ':disabled' });

                if (state == true) {
                    GuardarRespaldoinformacionDomicilio();
                }
                else {
                 //   console.log("Entra");
                    $('#frmSolicitud').parsley().validate({ group: 'informacionDomicilio', force: true });
                }

                if (PRECALIFICADO.PermitirIngresarSolicitud == false && PRECALIFICADO.MensajePermitirIngresarSolicitud != null) {
                    MensajeAdvertencia(PRECALIFICADO.MensajePermitirIngresarSolicitud);
                    state = false;
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
                    state = false;
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
                        state = false;
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

                        if (listaReferenciasPersonales.length >= 3) {
                            state = true;
                        }
                        else {
                            MensajeError('La cantidad mínima de referencias es 4');
                        }
                    }
                    else {
                        MensajeError('La cantidad mínima de referencias es 4');
                    }
                }

                if (PRECALIFICADO.PermitirIngresarSolicitud == false && PRECALIFICADO.MensajePermitirIngresarSolicitud != null) {
                    MensajeAdvertencia(PRECALIFICADO.MensajePermitirIngresarSolicitud);
                    state = false;
                }

                return state;
            }
        } /* if foward */
    });

    CargarDocumentosRequeridos();
    CargarOrigenes(PRECALIFICADO.IdProducto, 'Origen');

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
        var IdTiempoConocerReferencia = 0; //$("#ddlTiempoDeConocerReferencia :selected").val();
        var tiempoDeConocerReferenciaDescripcion = $("#ddlTiempoDeConocerReferencia :selected").text();
        var IdParentescoReferencia = $("#ddlParentescos :selected").val();
        var parentescoReferenciaDescripcion = $("#ddlParentescos :selected").text();
        var btnQuitarReferencia = '<button type="button" id="btnQuitarReferenciaPersonal" ' +
            'data-nombreReferencia="' + NombreCompletoReferencia + '" data-telefonoReferencia="' + TelefonoReferencia + '"' +
            'data-tiempoDeConocerReferencia="' + IdTiempoConocerReferencia + '" data-tiempoDeConocerReferenciaDescripcion="' + tiempoDeConocerReferenciaDescripcion + '" data-parentescoReferencia="' + IdParentescoReferencia + '" data-parentescoReferenciaDescripcion="' + parentescoReferenciaDescripcion + '"' +
            'class="btn btn-sm btn-danger" ><i class="far fa-trash-alt"></i> Quitar</button > ';

        /* Agregar referencia a la tabla de referencias personales */
        //var row = '<tr><td>' + NombreCompletoReferencia + '</td><td>' + TelefonoReferencia + '</td><td>' + tiempoDeConocerReferenciaDescripcion + '</td><td>' + parentescoReferenciaDescripcion + '</td><td class="text-center">' + btnQuitarReferencia + '</td></tr>';
        var row = '<tr><td>' + NombreCompletoReferencia + '</td><td>' + TelefonoReferencia + '</td><td>' + parentescoReferenciaDescripcion + '</td><td class="text-center">' + btnQuitarReferencia + '</td></tr>';
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

CargarReferenciasDeAplicacionCredito();
function CargarReferenciasDeAplicacionCredito() {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarReferenciasDeAplicacionCredito",
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            var divReferencia = $("#BodyReferenciasDeAplicacion");

            $.each(data.d, function (i, iter) {
                divReferencia.append(
                    `<tr> 
                      <td>${iter.Nombre}</td>
                      <td>${iter.Telefono}</td>
                      <td>${iter.Relacion}</td>
                    </tr`);

            }); /* Termina .Each*/
        }
    }); /* Termina Ajax */
}

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

//debugger;
/* Cargar prestamos disponibles consultados en el cotizador */
function CalculoPrestamo(valoraFinanciar, plazo, tasadeinteres) {
   
    var frecuencia = 0;
    
    var frecuenciaseleccionada = $("#ddlFrecuencia option:selected").val();

    if (frecuenciaseleccionada==1)
    {

        frecuencia = 26;
    }

    if (frecuenciaseleccionada==3)
    {
        frecuencia = 12;
    }

    if (frecuenciaseleccionada==10)
    {
        frecuencia = 52;
    }
    debugger;
 
    if (PRECALIFICADO.IdProducto == 100) {
        var frecuenciaseleccionada = $("#ddlFrecuencia option:selected").val();
        var ResultadoPrima = $("#txtValorPrima").val();
        var lienHolder = $("#txtLienHolder").val().replace(/,/g, '') == '' ? 0 : $("#txtLienHolder").val().replace(/,/g, '');
        var valorPrima = 0;
      

    $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CalculoPrestamo",
        data: JSON.stringify({ idProducto: PRECALIFICADO.IdProducto, valorGlobal: valoraFinanciar, valorPrima: valorPrima, plazo: plazo, frecuensia: frecuenciaseleccionada, lienHolder: lienHolder, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('No se pudo realizar el cálculo del préstamo, contacte al administrador');
            },
        success: function (data) {
            
                console.log(data);
                var objCalculo = data.d;
            
                $("#txtValorDePrestamo").val(objCalculo.ValorDelPrestamo);
                $("#txtValorCuota").val(objCalculo.CuotaAuto);
                $("#txtValorFinanciar").val(objCalculo.TotalAFinanciar);
                $("#txtCuotaMaxima").val(objCalculo.CuotaSegurodeVehiculo);
                $("#txtPlazoMaximo").val(objCalculo.CuotaAuto); 
                $("#txtPrestamoMaximo").val(objCalculo.CuotaTotal);
                $("#txtPlazoFrecuencia").val(objCalculo.lnNCuotas);
                $("#txtTasaDeInteresAnual").val(objCalculo.TasaInteresAnual);
               // $("#txtValorPrima").val().replace(/,/g, ''));


                COTIZADOR = objCalculo;
            }
        });
    }
    else 
    {
    var lnPlazoAnual = (plazo/frecuencia)
    var lnTotalInteres = ((valoraFinanciar * lnPlazoAnual * tasadeinteres)/100);
    var lnTotalFinanciado = ((parseFloat(valoraFinanciar) + lnTotalInteres) / plazo);
    
    $("#txtValorCuota").val(lnTotalFinanciado.toFixed(2));
    }

    //if (PRECALIFICADO.IdProducto == 100 || PRECALIFICADO.IdProducto == 102) {

    //    var lcSeguro = '';
    //    var lcGPS = '';
    //    var lcGastosdeCierre = '';

    //    if ($("#ddlTipoDeSeguro :selected").val() == "A - Full Cover") {
    //        lcSeguro = "1";
    //    }
    //    if ($("#ddlTipoDeSeguro :selected").val() == "B - Basico + Garantía") {
    //        lcSeguro = "2";
    //    }

    //    if ($("#ddlTipoDeSeguro :selected").val() == "C - Basico") {
    //        lcSeguro = "3";
    //    }

    //    lcGastosdeCierre = $("#ddlTipoGastosDeCierre :selected").val() == "Financiado" ? "1" : "0";

    //    if ($("#ddlGps :selected").val() == "Si - CPI") {
    //        lcGPS = "1";
    //    }
    //    if ($("#ddlGps :selected").val() == "Si - CableColor") {
    //        lcGPS = "2";
    //    }
    //    if ($("#ddlGps :selected").val() == "No") {
    //        lcGPS = "0";
    //    }

    //    if (lcSeguro != '' && lcGPS != '' && lcGastosdeCierre != '') {

    //        $.ajax({
    //            type: "POST",
    //            url: "SolicitudesCredito_Registrar.aspx/CalculoPrestamoVehiculo",
    //            data: JSON.stringify(
    //                {
    //                    idProducto: PRECALIFICADO.IdProducto,
    //                    valorGlobal: valorGlobal,
    //                    valorPrima: valorPrima,
    //                    plazo: plazo,
    //                    scorePromedio: PRECALIFICADO.ScorePromedio,
    //                    tipoSeguro: lcSeguro,
    //                    tipoGps: lcGPS,
    //                    gastosDeCierreFinanciados: lcGastosdeCierre,
    //                    dataCrypt: window.location.href
    //                }),
    //            contentType: 'application/json; charset=utf-8',
    //            error: function (xhr, ajaxOptions, thrownError) {

    //                MensajeError('No se pudo realizar el cálculo del préstamo, contacte al administrador');
    //            },
    //            success: function (data) {

    //                var objCalculo = data.d;
    //                $("#txtValorDePrestamo").val(objCalculo.ValorDelPrestamo);
    //                $("#txtValorCuota").val(objCalculo.CuotaTotal);
    //                $("#txtValorFinanciar").val(objCalculo.TotalAFinanciar);
    //                $("#txtTasaDeInteresAnual").val(objCalculo.TasaInteresAnual + '%');

    //                COTIZADOR = objCalculo;
    //            }
    //        });
    //    }
    //}
    //else {

    //    $.ajax({
    //        type: "POST",
    //        url: "SolicitudesCredito_Registrar.aspx/CalculoPrestamo",
    //        data: JSON.stringify({ idProducto: PRECALIFICADO.IdProducto, valorGlobal: valorGlobal, valorPrima: valorPrima, plazo: plazo, dataCrypt: window.location.href }),
    //        contentType: 'application/json; charset=utf-8',
    //        error: function (xhr, ajaxOptions, thrownError) {

    //            MensajeError('No se pudo realizar el cálculo del préstamo, contacte al administrador');
    //        },
    //        success: function (data) {

    //            var objCalculo = data.d;

    //            $("#txtValorDePrestamo").val(objCalculo.ValorDelPrestamo);
    //            $("#txtValorCuota").val(objCalculo.CuotaTotal);
    //            $("#txtValorFinanciar").val(objCalculo.TotalAFinanciar);
    //            $("#txtTasaDeInteresAnual").val(objCalculo.TasaInteresAnual + '%');

    //            COTIZADOR = objCalculo;
    //        }
    //    });
    //}
}


/* Cargar lista origenes para el producto */
$("#ddlProducto").change(function () {

    $(this).parsley().validate();

    var idProducto = $("#ddlProducto option:selected").val();
    
    if (idProducto == 101)
    {
        var TasaAnual = $("#txtTasaDeInteresAnual");
        TasaAnual.Text="30.00";
    }
   // CargarOrigenes(idProducto, 'Origen');
    //MensajeError(idProducto);
});

$("#ddlBarrioColoniaDomicilio").change(function () {

    $(this).parsley().validate();

    var id = $(this).val();
    console.log(id);
  
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


    if (PRECALIFICADO.IdProducto == 100) {
        $('#txtValorDePrestamo').blur(function () {
            var valorGlobal = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''));
            var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''));
            var plazo = parseInt($("#txtPlazosDisponibles").val().replace(/,/g, '') == '' ? 0 : $("#txtPlazosDisponibles").val().replace(/,/g, ''));          
            var valorFinanciar = parseFloat($("#txtValorDePrestamo").val().replace(/,/g, ''));
            
            var state = true;
            var InteresAnual = parseFloat($("#txtTasaDeInteresAnual").val().replace(/,/g, ''));
            if (CONSTANTES.RequierePrima == 1) {

                if (valorPrima >= valorGlobal) {

                    state = false;
                    MensajeError('El valor de la prima debe ser menor que el valor de la garantía');
                }

                if (CONSTANTES.PorcentajePrimaMinima != 0) {

                    if (valorPrima < ((valorGlobal * CONSTANTES.PorcentajePrimaMinima) / 100)) {

                        state = false;
                        MensajeAdvertencia('El porcentaje de prima mínimo para este producto es: ' + CONSTANTES.PorcentajePrimaMinima + '%. Aumente el valor de la prima.');
                    }
                }
            } /* if requiere prima */

            if (CONSTANTES.MontoFinanciarMinimo != 0) {

                if (valorFinanciar < CONSTANTES.MontoFinanciarMinimo) {

                    state = false;
                    MensajeAdvertencia('El monto mínimo a financiar es ' + CONSTANTES.MontoFinanciarMinimo + '.');
                }
            }

            if (CONSTANTES.MontoFinanciarMaximo != 0) {

                if (valorFinanciar > CONSTANTES.MontoFinanciarMaximo) {

                    state = false;
                    MensajeAdvertencia('El monto máximo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMaximo + '.');
                }
            }

            if (valorFinanciar > CONSTANTES.PrestamoMaximo_Monto && CONSTANTES.PrestamoMaximo_Monto != 0) {

                state = false;
                MensajeAdvertencia('El monto máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Monto + '.');
            }
            //debugger;
           
            if (valorFinanciar > 0 && plazo > 0) {
                    CalculoPrestamo(valorFinanciar.toString(), plazo.toString(), 0);
                }
            
            
        });
        $('#txtLienHolder').blur(function () {       
           
            var plazo = parseInt($("#txtPlazosDisponibles").val().replace(/,/g, '') == '' ? 0 : $("#txtPlazosDisponibles").val().replace(/,/g, ''));
            var valorFinanciar = parseFloat($("#txtValorDePrestamo").val().replace(/,/g, ''));
            if (valorFinanciar > 0 && plazo > 0) {
                CalculoPrestamo(valorFinanciar.toString(), plazo.toString(), 0);
            }

        });

    } else {
    $('#txtValorFinanciar').blur(function () {

    var valorGlobal = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''));
    var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''));
    var plazo = parseInt($("#txtPlazosDisponibles").val().replace(/,/g, '') == '' ? 0 : $("#txtPlazosDisponibles").val().replace(/,/g, ''));
    var valorFinanciar = parseFloat($("#txtValorFinanciar").val().replace(/,/g, ''));
    var state = true;
    var InteresAnual = parseFloat($("#txtTasaDeInteresAnual").val().replace(/,/g, ''));

    if (CONSTANTES.RequierePrima == 1) {

        if (valorPrima >= valorGlobal) {

            state = false;
            MensajeError('El valor de la prima debe ser menor que el valor de la garantía');
        }

        if (CONSTANTES.PorcentajePrimaMinima != 0) {

            if (valorPrima < ((valorGlobal * CONSTANTES.PorcentajePrimaMinima) / 100)) {

                state = false;
                MensajeAdvertencia('El porcentaje de prima mínimo para este producto es: ' + CONSTANTES.PorcentajePrimaMinima + '%. Aumente el valor de la prima.');
            }
        }
    } /* if requiere prima */

    if (CONSTANTES.MontoFinanciarMinimo != 0) {

        if (valorFinanciar < CONSTANTES.MontoFinanciarMinimo) {

            state = false;
            MensajeAdvertencia('El monto mínimo a financiar es ' + CONSTANTES.MontoFinanciarMinimo + '.');
        }
    }

    if (CONSTANTES.MontoFinanciarMaximo != 0) {

        if (valorFinanciar > CONSTANTES.MontoFinanciarMaximo) {

            state = false;
            MensajeAdvertencia('El monto máximo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMaximo + '.');
        }
    }

    if (valorFinanciar > CONSTANTES.PrestamoMaximo_Monto && CONSTANTES.PrestamoMaximo_Monto != 0) {

        state = false;
        MensajeAdvertencia('El monto máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Monto + '.');
    }
    //debugger;
    if (PRECALIFICADO.IdProducto == 100) {
        if (valorFinanciar > 0 && plazo > 0) {
            CalculoPrestamo(valorFinanciar.toString(), plazo.toString(), 0);
        }
    } else {
    if (valorFinanciar > 0 && plazo > 0) {
        CalculoPrestamo(valorFinanciar.toString(), plazo.toString(),InteresAnual.toString());
        }
    }
    });
}

function CargarOrigenes(idProducto,tipoLista) {
    //PRECALIFICADO.IdProducto
    var ddlOrigen = $('#ddlOrigen');

    if (idProducto != '') {
        //ddlOrigen.empty();
        //ddlOrigen.append("<option value=''>Seleccione una opción</option>");
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarListaOrigenes",
            data: JSON.stringify({ idProducto : idProducto }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al cargar catalogo de origenes');
            },
            success: function (data) {
                var listaOrigenes = data.d;

                $.each(listaOrigenes, function (i, iter) 
                {
                    ddlOrigen.append("<option value='" + iter.fiIDOrigen + "'>" + iter.fcOrigen + "</option>"); /* Agregar origenes seleccionado */
                });

                ddlOrigen.attr('disabled', false);
            }
        });
    }
    else {
        ddlOrigen.empty();
        ddlOrigen.append("<option value=''>Seleccione origen.</option>");
        ddlOrigen.attr('disabled', true);
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
        txtPlazosDisponibles: $("#txtPlazosDisponibles option:selected").val() == '' ? 0 : $("#txtPlazosDisponibles option:selected").val(),
        //ddlMoneda: $("#ddlMoneda :selected").val(),
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
        //ddlTipoDeCliente: $("#ddlTipoDeCliente :selected").val(),
        sexoCliente: $("input[name='sexoCliente']:checked").val(),
        txtCorreoElectronico: $("#txtCorreoElectronico").val(),
    }
    localStorage.setItem('RespaldoInformacionPersonal', JSON.stringify(respaldoInformacionPersonal));
}

function GuardarRespaldoinformacionDomicilio() {

    var respaldoinformacionDomicilio = {

       
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
        var plazo = parseInt(RespaldoInformacionPrestamo.txtPlazosDisponibles);

        $("#txtValorFinanciar").val(valorGlobal);
        $("#txtValorPrima").val(valorPrima);
        $("#txtPlazosDisponibles").val(plazo);


        /* Parametros de cotizador de vehiculos */
        $("#ddlTipoGastosDeCierre").val(RespaldoInformacionPrestamo.ddlTipoGastosDeCierre);
        $("#ddlTipoDeSeguro").val(RespaldoInformacionPrestamo.ddlTipoDeSeguro);
        $("#ddlGps").val(RespaldoInformacionPrestamo.ddlGps);

        //if (valorGlobal > 0 && plazo > 0) {
        //    CalculoPrestamo(valorGlobal.toString(), plazo.toString(), $("#txtTasaDeInteresAnual").val().replace(/,/g, ''));
        //}

        if (PRECALIFICADO.IdProducto == 100) {
            if (valorFinanciar > 0 && plazo > 0) {
                CalculoPrestamo(valorFinanciar.toString(), plazo.toString(), 0);
            }
        } else {
            if (valorFinanciar > 0 && plazo > 0) {
                CalculoPrestamo(valorFinanciar.toString(), plazo.toString(), InteresAnual.toString());
            }
        }
    }

    /* Recuperar respaldo de pestaña de informacion personal */
    if (localStorage.getItem('RespaldoInformacionPersonal') != null) {

        var respaldoInformacionPersonal = JSON.parse(localStorage.getItem('RespaldoInformacionPersonal'));

        $("#ddlNacionalidad").val(respaldoInformacionPersonal.ddlNacionalidad);
        $("#txtProfesion").val(respaldoInformacionPersonal.txtProfesion);
        $("#ddlEstadoCivil").val(respaldoInformacionPersonal.ddlEstadoCivil);
        //$("#ddlTipoDeCliente").val(respaldoInformacionPersonal.ddlTipoDeCliente);
        $("input[name=sexoCliente][value=" + respaldoInformacionPersonal.sexoCliente + "]").prop('checked', true);
        $("#txtCorreoElectronico").val(respaldoInformacionPersonal.txtCorreoElectronico);
    }

    /* Recuperar resplado de pestaña de informacion de domicilio */
    if (localStorage.getItem('RespaldoinformacionDomicilio') != null) {

        var respaldoinformacionDomicilio = JSON.parse(localStorage.getItem('RespaldoinformacionDomicilio'));

        $("#ddlBarrioColoniaEmpresa").val(respaldoinformacionDomicilio.ddlBarrioColoniaEmpresa);
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
        $("#ddlBarrioColoniaDomicilio").val(respaldoInformacionLaboral.ddlBarrioColoniaDomicilio);

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
                    /*'data-tiempoDeConocerReferencia="' + referencia.IdTiempoConocerReferencia + '" data-tiempoDeConocerReferenciaDescripcion="' + referencia.tiempoDeConocerReferenciaDescripcion + */'" data-parentescoReferencia="' + referencia.IdParentescoReferencia + '" data-parentescoReferenciaDescripcion="' + referencia.parentescoReferenciaDescripcion + '"' +
                    'class="btn btn-sm btn-danger"><i class="far fa-trash-alt"></i> Quitar</button > ';

                /* Agregar referencia a la tabla de referencias personales */
                //var row = '<tr><td>' + referencia.NombreCompletoReferencia + '</td><td>' + referencia.TelefonoReferencia + '</td><td>' + referencia.tiempoDeConocerReferenciaDescripcion + '</td><td>' + referencia.parentescoReferenciaDescripcion + '</td><td class="text-center">' + btnQuitarReferencia + '</td></tr>';
                var row = '<tr><td>' + referencia.NombreCompletoReferencia + '</td><td>' + referencia.TelefonoReferencia + '</td><td>' + referencia.parentescoReferenciaDescripcion + '</td><td class="text-center">' + btnQuitarReferencia + '</td></tr>';

                tablaReferenciasPersonales.append(row);

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

function ConfirmarAval() {
    $("#modalConfirmarAval").modal("show");  
    
    $("#btnAgregarAvalModelConfirmar").click(function () {       

        RedirigirAccion('../Aval/Aval_Registrar.aspx', 'Modulo de aval');

        //idSolicitud = 80;
        /*
        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Registrar.aspx/ConfirmarAval',
            data: JSON.stringify({idSolicitud: idSolicitud}),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                debugger;
                MensajeError('Error al confirmar aval');
            },
            success: function (data) {
                debugger;
                console.log(data);
                //if (data.d.ResultadoExitoso == true) {

                //    MensajeExito(data.d.MensajeResultado);
                //    //localStorage.clear();
                //    //ConfirmarAval(data.d.IdInsertado);
                //    //resetForm($("#frmSolicitud"));
                //    ///$($('#smartwizard')).smartWizard("reset");
                //}
                //else {
                //    MensajeError(data.d.MensajeResultado);
                //}
            }*/
       // })


        //window.location.href = 'Aval_Registrar.aspx';
    });
    $("#btnAgregarAvalModelCancelar").click(function () {
       
        $("#modalConfirmarAval").modal('hide');
    });

}

function RedirigirAccion(nombreFormulario, accion) {
    
   // idSolicitud = 80; 
    //var identidad = CONSTANTES.IdCliente;
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/EncriptarParametros",
        data: JSON.stringify({ idSolicitud: idSolicitud, dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            debugger;
            MensajeError("No se pudo redireccionar a " + accion);
        },
        success: function (data) {

            data.d != "-1" ? window.location = nombreFormulario + "?" + data.d : MensajeError("No se pudo redireccionar a" + accion);
        }
    });
}

/* Solicitar cambio de score... */
$("#btnSolicitarCambioScore").click(function () {

    if ($('#frmSolicitud').parsley().isValid({ group: 'cambiarScore' })) {

        $("#btnSolicitarCambioScore").prop('disabled', true);

        let nuevoScore = $("#txtNuevoScore").val() == '' ? 0 : $("#txtNuevoScore").val();
        let comentarioAdicional = $("#txtCambiarScoreComentarioAdicional").val();
        let nombreCliente = PRECALIFICADO.PrimerNombre + ' ' + PRECALIFICADO.SegundoNombre + ' ' + PRECALIFICADO.PrimerApellido + ' ' + PRECALIFICADO.SegundoApellido

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/EnviarSolicitudCambioScore",
            data: JSON.stringify({ identidadCliente: PRECALIFICADO.Identidad, nombreCliente: nombreCliente, scoreActual: PRECALIFICADO.ScorePromedio, nuevoScore: nuevoScore, comentarioAdicional: comentarioAdicional, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                $("#btnSolicitarCambioScore").prop('disabled', false);
                MensajeError('No se pudo enviar el correo, contacte al administrador.');
            },
            success: function (data) {

                data.d == true ? MensajeExito('Solicitud de cambio de score enviada por correo exitosamente!') : MensajeError('No se pudo enviar el correo, contacte al administrador.');

                $("#txtNuevoScore,#txtCambiarScoreComentarioAdicional").val('');
                $("#btnSolicitarCambioScore").prop('disabled', false);
            }
        });

        MensajeInformacion('Enviando solicitud por correo...');
        $("#modalCambiarScore").modal('hide');
    }
    else {
        $('#frmSolicitud').parsley().validate({ group: 'cambiarScore', force: true });
    }
});