var idSolicitud = 0;
var ListaMunicipios = [];
var ListaCiudades = [];
var ListaBarriosColonias = [];
var clienteID = 0;
var estadoFuncionLlenarDDL = false;

$(document).ready(function () {

    // botón finalizar condicionamiento de la solicitud
    var btnFinish = $('<button type="button" id="btnGuardarSolicitud"></button>').text('Finalizar')
        .addClass('btn btn-info')
        .css('display', 'none')
        .on('click', function () {

            $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
            var modelStateFormPersonal = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' });

            $('#frmSolicitud').parsley().validate({ group: 'informacionDomiciliar', force: true });
            var modelStateFormDomiciliar = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomiciliar' });

            $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
            var modelStateFormLaboral = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

            var modelStateFormConyugal = true;
            if ($("input[name='estadoCivil']:checked").data('info') == true) {
                $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
                modelStateFormConyugal = $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });
            }
            if (modelStateFormPersonal == true && modelStateFormDomiciliar == true && modelStateFormLaboral == true && modelStateFormConyugal == true) {

                var AvalMaster = {
                    fcIdentidadAval: $("#identidadCliente").val(),
                    fcTelefonoAval: $("#numeroTelefono").val(),
                    fiNacionalidad: $("#nacionalidad").val(),
                    fdFechaNacimientoAval: $("#fechaNacimiento").val(),
                    fcCorreoElectronicoAval: $("#correoElectronico").val(),
                    fcProfesionOficioAval: $("#profesion").val(),
                    fcSexoAval: $("input[name='sexo']:checked").val(),
                    fiIDEstadoCivil: $("input[name='estadoCivil']:checked").val(),
                    fiIDVivienda: $("#vivivenda").val(),
                    fiTiempoResidir: $("input[name='tiempoResidir']:checked").val(),
                    fcPrimerNombreAval: $("#primerNombreCliente").val(),
                    fcSegundoNombreAval: $("#SegundoNombreCliente").val(),
                    fcPrimerApellidoAval: $("#primerApellidoCliente").val(),
                    fcSegundoApellidoAval: $("#segundoApellidoCliente").val()
                };
                var AvalInformacionLaboral = {
                    fcNombreTrabajo: $("#nombreDelTrabajo").val(),
                    fiIngresosMensuales: $("#ingresosMensuales").val().replace(/,/g, ''),
                    fcPuestoAsignado: $("#puestoAsignado").val(),
                    fcFechaIngreso: $("#fechaIngreso").val(),
                    fdTelefonoEmpresa: $("#telefonoEmpresa").val(),
                    fcExtensionRecursosHumanos: $("#extensionRRHH").val(),
                    fcExtensionAval: $("#extensionCliente").val(),
                    fiCiudadEmpresa: $("#ciudadEmpresa").val(),
                    fiIDBarrioColonia: $("#barrioColoniaEmpresa").val(),
                    fcDireccionDetalladaEmpresa: $("#direccionDetalladaEmpresa").val(),
                    fcReferenciasDireccionDetallada: $("#referenciaDireccionDetalladaEmpresa").val(),
                    fcFuenteOtrosIngresos: $("#fuenteOtrosIngresos").val(),
                    fiValorOtrosIngresosMensuales: $("#valorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#fuenteOtrosIngresos").val(),
                };
                var AvalInformacionDomiciliar = {
                    fiIDCiudad: $("#ciudad").val(),
                    fiIDBarrioColonia: $("#barrioColonia").val(),
                    fcTelefonoCasa: $("#telefonoCasa").val(),
                    fcDireccionDetallada: $("#direccionDetallada").val(),
                    fcReferenciasDireccionDetallada: $("#referenciaDireccionDetallada").val()
                };
                var AvalInformacionConyugal = {
                    fcNombreCompletoConyugue: $("#nombresConyugue").val() + ' ' + $("#apellidosConyugue").val(),
                    fcIndentidadConyugue: $("#identidadConyugue").val(),
                    fdFechaNacimientoConyugue: $("#fechaNacimientoConyugue").val(),
                    fcTelefonoConyugue: $("#telefonoConyugue").val(),
                    fcLugarTrabajoConyugue: $("#lugarTrabajoConyugue").val(),
                    fcIngresosMensualesConyugue: $("#ingresoMensualesConyugue").val().replace(/,/g, ''),
                    fcTelefonoTrabajoConyugue: $("#telefonoTrabajoConyugue").val()
                };

                $.ajax({
                    type: "POST",
                    url: 'SolicitudesCredito_IngresarAval.aspx/RegistrarAval',
                    data: JSON.stringify({ avalMaster: AvalMaster, avalInformacionLaboral: AvalInformacionLaboral, avalInformacionDomiciliar: AvalInformacionDomiciliar, avalInformacionConyugal: AvalInformacionConyugal }),
                    contentType: 'application/json; charset=utf-8',
                    error: function (xhr, ajaxOptions, thrownError) {
                        MensajeError('No se guardó el registro, contacte al administrador');
                    },
                    success: function (data) {

                        if (data.d.response == true) {

                            MensajeExito(data.d.message);
                            localStorage.setItem('precalificado', null);
                            localStorage.clear();
                            clienteID = 0;
                            resetForm($("#frmSolicitud"));
                            window.location = 'SolicitudesCredito_Ingresadas.aspx';
                        }
                        else {
                            MensajeError(data.d.message);
                        }
                    }
                });
                // termina peticion ajax al server
            }
        });

    // inicalizar el Wizard
    $('#smartwizard').smartWizard({
        selected: 0,
        theme: 'default',
        transitionEffect: 'fade',
        showStepURLhash: false,
        autoAdjustHeight: false,
        toolbarSettings: {
            toolbarPosition: 'both',
            toolbarButtonPosition: 'end',
            toolbarExtraButtons: [btnFinish]
        },
        lang: {// variables del lenguaje
            next: 'Siguiente',
            previous: 'Anterior'
        }
    });

    // Cuando se muestre un step
    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {

        // si es el primer paso, deshabilitar el boton "anterior"
        if (stepPosition === 'first') {
            $("#prev-btn").addClass('disabled').css('display', 'none');
            // si es el ultimo paso, deshabilitar el boton siguiente
        } else if (stepPosition === 'final') {
            $("#next-btn").addClass('disabled');
            $("#btnGuardarSolicitud").css('display', '');

            // si no es ninguna de las anteriores, habilitar todos los botones
        } else {
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
            $("#btnGuardarSolicitud").css('display', 'none');
        }
        if (stepNumber == 4) {
            //inicializar validaciones de los formularios
            $('#frmSolicitud').parsley().reset();
        }
    });

    // habilitar boton siguiente al reiniciar steps
    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');
    });

    // Set selected theme on page refresh
    $("#theme_selector").change();

    // cuando se deja un paso (realizar validaciones)
    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

        //si no requere informacion personal, saltarse esa pestaña
        if ($("input[name='estadoCivil']:checked").data('info') == false) {
            $('#smartwizard').smartWizard("stepState", [4], "hide");
        }
        // validar solo si se quiere ir hacia el siguiente paso
        if (stepDirection == 'forward') {

            //validar informacion personal   
            if (stepNumber == 0) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' });

                if (state == false) {
                    //si no es valido, mostrar validaciones al usuario
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
                }
                return state;
            }

            //validar informacion domiciliar
            if (stepNumber == 1) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomiciliar' });

                if (state == false) {

                    //si no es valido, mostrar validaciones al usuario
                    $('#frmSolicitud').parsley().validate({ group: 'informacionDomiciliar', force: true });
                }
                return state;
            }

            //validar informacion laboral
            if (stepNumber == 2) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

                if (state == false) {
                    //si no es valido, mostrar validaciones al usuario
                    $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
                }
                return state;
            }

            //validar informacion conyugal
            if (stepNumber == 3) {

                if ($("input[name='estadoCivil']:checked").data('info') == true) {

                    var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });

                    if (state == false) {
                        //si no es valido, mostrar validaciones al usuario
                        $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
                    }
                    return state;
                }
            }

        }//termina if fowards

    });

    llenarDropDownLists();

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

    //TERMINA FORMULARIO POR PASOS/STEPS

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //enero es 0!
    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd
    }
    if (mm < 10) {
        mm = '0' + mm
    }

    today = yyyy + '-' + mm + '-' + dd;
    document.getElementById("fechaNacimiento").setAttribute("max", today);
    document.getElementById("fechaIngreso").setAttribute("max", today);
    document.getElementById("fechaNacimientoConyugue").setAttribute("max", today);
    document.getElementById("fechaNacimiento").setAttribute("max", today);
});

// -- INICIA INFORMACIÓN DOMICILIAR DEL CLIENTE -- //

//habilitar ddl municipios cliente cuando se seleccione un departamento cliente
$("#departamento").change(function () {

    $(this).parsley().validate();

    //obtener id del departamento seleccionado
    var idDepto = $("#departamento").val();

    var municipioDdl = $("#municipio");
    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idDepto != '') {

        //vaciar dropdownlist de municipios
        municipioDdl.empty();

        //agregar opciones
        municipioDdl.append("<option value=''>Seleccione una opción</option>");

        //agregar munipios que pertenecen al departamento seleccionado

        var municipiosDelDepto = ListaMunicipios.filter(d => d.fiIDDepto == idDepto);

        $.each(municipiosDelDepto, function (i, iter) {
            municipioDdl.append("<option value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
        });

        //habilitar dropdownlist
        municipioDdl.attr('disabled', false);

        //reiniciar dropdonwlist de ciudades

        //vaciar dropdownlist de ciudades
        ciudadDdl.empty();

        //agregar opciones
        ciudadDdl.append("<option value=''>Seleccione un municipio</option>");

        //deshabilitar dropdownlist de ciudades
        ciudadDdl.attr('disabled', true);

        //reiniciar dropdonwlist de barrios y colonias

        //vaciar dropdownlist de barrios y colonias
        BarrioColoniaDdl.empty();

        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist de barrios y colonias
        BarrioColoniaDdl.attr('disabled', true);
    }
    else {
        //vaciar dropdownlist de municipio
        municipioDdl.empty();

        municipioDdl.append("<option value=''>Seleccione un depto.</option>");

        //deshabilitar dropdownlist de municipio
        municipioDdl.attr('disabled', true);

        //vaciar dropdownlist de ciudades
        ciudadDdl.empty();

        ciudadDdl.append("<option value=''>Seleccione un municipio</option>");

        //deshabilitar dropdownlist de ciudades
        ciudadDdl.attr('disabled', true);

        //vaciar dropdownlist de barrios y colonias
        BarrioColoniaDdl.empty();

        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist de barrios y colonias
        BarrioColoniaDdl.attr('disabled', true);
    }
});

//habilitar ddl ciudades cliente cuando se seleccione un municipio cliente
$("#municipio").change(function () {

    $(this).parsley().validate();

    //obtener id del municipio seleccionado
    var idMunicipio = $("#municipio").val();

    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idMunicipio != '') {

        //vaciar dropdownlist
        ciudadDdl.empty();

        //agregar opciones
        ciudadDdl.append("<option value=''>Seleccione una opción</option>");

        //agregar ciudades que pertenecen al municipio seleccionado
        var ciudadesDelMunicipio = ListaCiudades.filter(d => d.fiIDMunicipio == idMunicipio);

        $.each(ciudadesDelMunicipio, function (i, iter) {
            ciudadDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
        });

        //habilitar dropdownlist
        ciudadDdl.attr('disabled', false);

        //reiniciar dropdonwlist de barrios y colonias

        //vaciar dropdownlist de barrios y colonias
        BarrioColoniaDdl.empty();

        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist de barrios y colonias
        BarrioColoniaDdl.attr('disabled', true);
    }
    else {
        //vaciar dropdownlist de ciudades
        ciudadDdl.empty();

        ciudadDdl.append("<option value=''>Seleccione un municipio</option>");

        //deshabilitar dropdownlist de ciudades
        ciudadDdl.attr('disabled', true);

        //vaciar dropdownlist de barrios y colonias
        BarrioColoniaDdl.empty();

        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist de barrios y colonias
        BarrioColoniaDdl.attr('disabled', true);
    }
});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
$("#ciudad").change(function () {

    $(this).parsley().validate();

    //obtener id del municipio seleccionado
    var idCiudad = $("#ciudad").val();
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idCiudad != '') {
        //vaciar dropdownlist
        BarrioColoniaDdl.empty();

        //agregar opciones
        BarrioColoniaDdl.append("<option value=''>Seleccione una opción</option>");

        //agregar ciudades que pertenecen al municipio seleccionado
        var barriosDeLaCiudad = ListaBarriosColonias.filter(d => d.fiIDCiudad == idCiudad);

        $.each(barriosDeLaCiudad, function (i, iter) {
            BarrioColoniaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
        });

        //habilitar dropdownlist
        BarrioColoniaDdl.attr('disabled', false);
    }
    else {
        //vaciar dropdownlist
        BarrioColoniaDdl.empty();

        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist
        BarrioColoniaDdl.attr('disabled', true);
    }
});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
$("#barrioColonia").change(function () {
    $(this).parsley().validate();
});

// -- INICIA INFORMACIÓN UBICACIÓN EMPRESA CLIENTE -- //

//habilitar ddl municipios cliente cuando se seleccione un departamento cliente
$("#departamentoEmpresa").change(function () {

    $(this).parsley().validate();

    //obtener id del departamento seleccionado
    var idDepto = $("#departamentoEmpresa").val();

    var municipioEmpresaDdl = $("#municipioEmpresa");
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idDepto != '') {
        //vaciar dropdownlist de municipios
        municipioEmpresaDdl.empty();

        //agregar opciones
        municipioEmpresaDdl.append("<option value=''>Seleccione una opción</option>");

        //agregar munipios que pertenecen al departamento seleccionado

        var municipiosDelDepto = ListaMunicipios.filter(d => d.fiIDDepto == idDepto);

        $.each(municipiosDelDepto, function (i, iter) {
            municipioEmpresaDdl.append("<option value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
        });

        //habilitar dropdownlist
        municipioEmpresaDdl.attr('disabled', false);

        //reiniciar dropdonwlist de ciudades

        //vaciar dropdownlist de ciudades
        ciudadaEmpresaDdl.empty();

        //agregar opciones
        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");

        //deshabilitar dropdownlist de ciudades
        ciudadaEmpresaDdl.attr('disabled', true);

        //reiniciar dropdonwlist de barrios y colonias

        //vaciar dropdownlist de barrios y colonias
        barrioColoniaEmpresaDdl.empty();

        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist de barrios y colonias
        barrioColoniaEmpresaDdl.attr('disabled', true);

    }
    else {
        //vaciar dropdownlist de municipio
        municipioEmpresaDdl.empty();

        municipioEmpresaDdl.append("<option value=''>Seleccione un depto.</option>");

        //deshabilitar dropdownlist de municipio
        municipioEmpresaDdl.attr('disabled', true);

        //vaciar dropdownlist de ciudades
        ciudadaEmpresaDdl.empty();

        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");

        //deshabilitar dropdownlist de ciudades
        ciudadaEmpresaDdl.attr('disabled', true);

        //vaciar dropdownlist de barrios y colonias
        barrioColoniaEmpresaDdl.empty();

        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist de barrios y colonias
        barrioColoniaEmpresaDdl.attr('disabled', true);
    }
});

//habilitar ddl ciudades cliente cuando se seleccione un municipio cliente
$("#municipioEmpresa").change(function () {

    $(this).parsley().validate();

    //obtener id del municipio seleccionado
    var idMunicipio = $("#municipioEmpresa").val();
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idMunicipio != '') {
        //vaciar dropdownlist
        ciudadaEmpresaDdl.empty();

        //agregar opciones
        ciudadaEmpresaDdl.append("<option value=''>Seleccione una opción</option>");

        //agregar ciudades que pertenecen al municipio seleccionado
        var ciudadesDelMunicipio = ListaCiudades.filter(d => d.fiIDMunicipio == idMunicipio);

        $.each(ciudadesDelMunicipio, function (i, iter) {
            ciudadaEmpresaDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
        });

        //habilitar dropdownlist
        ciudadaEmpresaDdl.attr('disabled', false);

        //reiniciar dropdonwlist de barrios y colonias

        //vaciar dropdownlist de barrios y colonias
        barrioColoniaEmpresaDdl.empty();

        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist de barrios y colonias
        barrioColoniaEmpresaDdl.attr('disabled', true);
    }
    else {
        //vaciar dropdownlist de ciudades
        ciudadaEmpresaDdl.empty();

        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");

        //deshabilitar dropdownlist de ciudades
        ciudadaEmpresaDdl.attr('disabled', true);

        //vaciar dropdownlist de barrios y colonias
        barrioColoniaEmpresaDdl.empty();

        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist de barrios y colonias
        barrioColoniaEmpresaDdl.attr('disabled', true);
    }
});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
$("#ciudadEmpresa").change(function () {

    $(this).parsley().validate();

    //obtener id del municipio seleccionado
    var idCiudad = $("#ciudadEmpresa").val();
    var barriosEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idCiudad != '') {
        //vaciar dropdownlist
        barriosEmpresaDdl.empty();

        //agregar opciones
        barriosEmpresaDdl.append("<option value=''>Seleccione una opción</option>");

        //agregar ciudades que pertenecen al municipio seleccionado
        var barriosDeLaCiudad = ListaBarriosColonias.filter(d => d.fiIDCiudad == idCiudad);

        $.each(barriosDeLaCiudad, function (i, iter) {
            barriosEmpresaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
        });

        //habilitar dropdownlist
        barriosEmpresaDdl.attr('disabled', false);
    }
    else {
        //vaciar dropdownlist
        barriosEmpresaDdl.empty();

        barriosEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist
        barriosEmpresaDdl.attr('disabled', true);
    }
});

//habilitar ddl barrios y colonias empresa
$("#barrioColoniaEmpresa").change(function () {
    $(this).parsley().validate();
});

//validar si se requiere información conyugal
$('input:radio[name="estadoCivil"]').change(function () {
    var requiereInformacionConyugal = $("input[name='estadoCivil']:checked").data('info');
    //var requiereInformacionConyugal = this.data('info');

    //si no se requiere información conyugal, deshabilitar ese formulario
    if (requiereInformacionConyugal == false) {
        $('input.infoConyugal').attr('disabled', true);
        $('#smartwizard').smartWizard("stepState", [4], "hide");
    }
    else if (requiereInformacionConyugal == true) {
        $('input.infoConyugal').attr('disabled', false);
        $('#smartwizard').smartWizard("stepState", [4], "show");
    }
});

// llenar dinámicamente los dropdownlists del formulario de ingresar solicitud
function llenarDropDownLists() {

    estadoFuncionLlenarDDL = false;
    $("#spinnerCargando").css('display', '');
    $("select").empty();

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/getDDLS",
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            // llenar select de tipos de prestamos
            var tipoPrestamoDdl = $("#tipoPrestamo");
            tipoPrestamoDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.TipoPrestamo, function (i, iter) {
                tipoPrestamoDdl.append("<option value='" + iter.fiIDTipoPrestamo + "'>" + iter.fcDescripcion + "</option>");
            });

            // llenar select de nacionalidades
            var nacionalidadDdl = $("#nacionalidad");
            nacionalidadDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Nacionalidades, function (i, iter) {
                nacionalidadDdl.append("<option value='" + iter.fiIDNacionalidad + "'>" + iter.fcDescripcionNacionalidad + "</option>");
            });

            // llenar select de estados civiles
            var divEstadoCivil = $("#divEstadoCivil");

            $.each(data.d.EstadosCiviles, function (i, iter) {
                divEstadoCivil.append("<div class='form-check form-check-inline'>" +
                    "<input data-info='" + iter.fbRequiereInformacionConyugal + "' class='form-check-input' type='radio' name ='estadoCivil' value='" + iter.fiIDEstadoCivil + "' required data-parsley-errors-container='#error-estadoCivil' data-parsley-group='informacionPersonal'>" +
                    "<label class='form-check-label'>" + iter.fcDescripcionEstadoCivil + "</label>" +
                    "</div>");
            });

            // llenar select de tiempo de conocer referencia personal
            var tiempoConocerRefDdl = $("#tiempoConocerRef");
            tiempoConocerRefDdl.append("<option value='Menos de un año'>-1 año</option>");
            tiempoConocerRefDdl.append("<option value='1'>1 año</option>");
            tiempoConocerRefDdl.append("<option value='2'>2 años</option>");
            tiempoConocerRefDdl.append("<option value='3'>+2 años</option>");

            // llenar select de vivivenda
            var viviendaDdl = $("#vivivenda");
            viviendaDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Vivienda, function (i, iter) {
                viviendaDdl.append("<option value='" + iter.fiIDVivienda + "'>" + iter.fcDescripcionVivienda + "</option>");
            });

            // llenar select de departamentos
            var departamentoDdl = $("#departamento");
            departamentoDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Departamentos, function (i, iter) {
                departamentoDdl.append("<option value='" + iter.fiIDDepto + "'>" + iter.fcNombreDepto + "</option>");
            });

            // llenar lista de municipios //aqui
            $("#municipio").append("<option value=''>Seleccione un depto.</option>");
            ListaMunicipios = [];
            $.each(data.d.Municipios, function (i, iter) {
                ListaMunicipios.push(iter);
            });

            // llenar lista de ciudad
            $("#ciudad").append("<option value=''>Seleccione un municipio</option>");
            ListaCiudades = [];
            $.each(data.d.Ciudades, function (i, iter) {
                ListaCiudades.push(iter);
            });

            // llenar lista de barrios y colonias 
            $("#barrioColonia").append("<option value=''>Seleccione una ciudad</option>");
            ListaBarriosColonias = [];
            $.each(data.d.BarriosColonias, function (i, iter) {
                ListaBarriosColonias.push(iter);
            });

            // -- Información de la empresa 

            // llenar select de departamento Empresa 
            var departamentoEmpresaDdl = $("#departamentoEmpresa");
            departamentoEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Departamentos, function (i, iter) {
                departamentoEmpresaDdl.append("<option value='" + iter.fiIDDepto + "'>" + iter.fcNombreDepto + "</option>");
            });

            // llenar select de municipios de la empresa 
            $("#municipioEmpresa").append("<option value=''>Seleccione un depto</option>");

            // llenar select de ciudadEmpresa 
            $("#ciudadEmpresa").append("<option value=''>Seleccione un municipio</option>");

            // llenar select de barrios y colonias de la empresa
            $("#barrioColoniaEmpresa").append("<option value=''>Seleccione una ciudad</option>");

            // **TERMINAN** CARGA DE DROPDOWNLIST
            estadoFuncionLlenarDDL = true;
            $("#spinnerCargando").css('display', 'none');
        }
    });
}

$("#nacionalidad,#vivivenda").on('change', function () {
    $($(this)).parsley().validate();
});

//incluir Utilities.js
$.getScript("../../Scripts/app/Utilities.js")
    .done(function (script, textStatus) {
    })
    .fail(function (jqxhr, settings, exception) {
        console.log('error al cargar utilities');
    });