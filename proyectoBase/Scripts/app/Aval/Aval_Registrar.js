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

            $('#frmSolicitud').submit(function (e) { e.preventDefault() });

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
                    fcIdentidadAval: $("#identidadAval").val(),
                    RTNAval: $("#identidadAval").val(),
                    fcTelefonoAval: $("#numeroTelefono").val(),
                    fiNacionalidad: $("#nacionalidad :selected").val(),
                    fdFechaNacimientoAval: $("#fechaNacimiento").val(),
                    fcCorreoElectronicoAval: $("#correoElectronico").val(),
                    fcProfesionOficioAval: $("#profesion").val(),
                    fcSexoAval: $("input[name='sexo']:checked").val(),
                    fiIDEstadoCivil: $("input[name='estadoCivil']:checked").val(),
                    fiIDVivienda: $("#vivivenda").val(),
                    fiTiempoResidir: $("input[name='tiempoResidir']:checked").val(),
                    fcPrimerNombreAval: $("#primerNombreAval").val(),
                    fcSegundoNombreAval: $("#SegundoNombreAval").val(),
                    fcPrimerApellidoAval: $("#primerApellidoAval").val(),
                    fcSegundoApellidoAval: $("#segundoApellidoAval").val()
                };
                var AvalInformacionLaboral = {
                    fcNombreTrabajo: $("#nombreDelTrabajo").val(),
                    fiIngresosMensuales: $("#ingresosMensuales").val().replace(/,/g, ''),
                    fcPuestoAsignado: $("#puestoAsignado").val(),
                    fcFechaIngreso: $("#fechaIngreso").val(),
                    fdTelefonoEmpresa: $("#telefonoEmpresa").val(),
                    fcExtensionRecursosHumanos: $("#extensionRRHH").val(),
                    fcExtensionAval: $("#extensionAval").val(),
                    fiCiudadEmpresa: $("#ciudadEmpresa").val(),
                    fiIDBarrioColonia: $("#barrioColoniaEmpresaAval").val(),
                    fiIDDepto: 0 /*CODDeptoEmpresa*/,
                    fiIDMunicipio: 0 /*CODMunicipioEmpresa*/,
                    fiIDCiudad:0 /*CODCiudadEmpresa*/,
                    fcDireccionDetalladaEmpresa: $("#direccionDetalladaEmpresa").val(),
                    fcReferenciasDireccionDetallada: $("#referenciaDireccionDetalladaEmpresa").val(),
                    fcFuenteOtrosIngresos: $("#fuenteOtrosIngresos").val(),
                    fiValorOtrosIngresosMensuales: $("#valorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#valorOtrosIngresos").val().replace(/,/g, ''),
                };
                var AvalInformacionDomiciliar = {
                    fiIDCiudad:0 /*$("#ciudad").val()*/,
                    fiIDBarrioColonia: $("#barrioColoniaAval").val(),
                    fiIDDepto:0  /*CODDepto*/,
                    fiIDMunicipio: 0 /*CODMunicipio*/,
                    fiIDCiudad: 0/* CODCiudad*/,
                    fcTelefonoCasa: $("#telefonoCasa").val(),
                    fcTelefonoMovil: $("#telefonoMovil").val(),
                    fcDireccionDetallada: $("#direccionDetallada").val(),
                    fcReferenciasDireccionDetallada: $("#referenciaDireccionDetallada").val()
                };
                var AvalInformacionConyugal = {
                    fcNombreCompletoConyugue: $("#nombresConyugue").val() + ' ' + $("#apellidosConyugue").val(),
                    fcIndentidadConyugue: $("#identidadConyugue").val(),
                    fdFechaNacimientoConyugue: $("#fechaNacimientoConyugue").val(),
                    fcTelefonoConyugue: $("#telefonoConyugue").val(),
                    fcLugarTrabajoConyugue: $("#lugarTrabajoConyugue").val(),
                    fcIngresosMensualesConyugue: $("#ingresoMensualesConyugue").val().replace(/,/g, '') == '' ? 0 : $("#ingresoMensualesConyugue").val().replace(/,/g, ''),
                    fcTelefonoTrabajoConyugue: $("#telefonoTrabajoConyugue").val()
                };

                var qString = "?" + window.location.href.split("?")[1];
                $.ajax({
                    type: "POST",
                    url: 'Aval_Registrar.aspx/RegistrarAval' + qString,
                    data: JSON.stringify({ avalMaster: AvalMaster, avalInformacionLaboral: AvalInformacionLaboral, avalInformacionDomiciliar: AvalInformacionDomiciliar, avalInformacionConyugal: AvalInformacionConyugal }),
                    contentType: 'application/json; charset=utf-8',
                    error: function (xhr, ajaxOptions, thrownError) {
                        MensajeError('No se guardó el registro, contacte al administrador');
                    },
                    success: function (data) {

                        if (data.d.response == true) {

                            MensajeExito(data.d.message);
                            clienteID = 0;
                            resetForm($("#frmSolicitud"));
                           
                            window.top.location = '../Solicitudes/SolicitudesCredito_Ingresadas.aspx' + qString;
                        }
                        else {
                            MensajeError(data.d.message);
                        }
                    }
                });
            }
        });

    $('#smartwizard').smartWizard({// inicalizar el Wizard
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

        if (stepPosition === 'first') { // si es el primer paso, deshabilitar el boton "anterior"
            $("#prev-btn").addClass('disabled').css('display', 'none');
        }
        else if (stepPosition === 'final') { // si es el ultimo paso, deshabilitar el boton siguiente
            $("#next-btn").addClass('disabled');
            $("#btnGuardarSolicitud").css('display', '');
        }
        else {// si no es ninguna de las anteriores, habilitar todos los botones
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
            $("#btnGuardarSolicitud").css('display', 'none');
        }
    });

    // habilitar boton siguiente al reiniciar steps
    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');
    });

    // cuando se deja un paso (realizar validaciones)
    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

        if ($("input[name='estadoCivil']:checked").data('info') == false) {
            $('#smartwizard').smartWizard("stepState", [3], "hide");//si no requere informacion personal, saltarse esa pestaña
        }
        else if ($("input[name='estadoCivil']:checked").data('info') == true) {
            $('#smartwizard').smartWizard("stepState", [3], "show");
        }

        if (stepDirection == 'forward') {// validar solo si se quiere ir hacia el siguiente paso

            if (stepNumber == 0) {//aqui realizar las validaciones
                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' });
                if (state == false) {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });//si no es valido, mostrar validaciones al usuario
                }
                return state;
            }

            if (stepNumber == 1) {//validar informacion personal   

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomiciliar' });
                if (state == false) {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionDomiciliar', force: true });//si no es valido, mostrar validaciones al usuario
                }
                return state;
            }

            if (stepNumber == 2) {//validar informacion domiciliar

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });
                if (state == false) {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });//si no es valido, mostrar validaciones al usuario
                }
                return state;
            }

            if (stepNumber == 3) {//validar informacion laboral

                if ($("input[name='estadoCivil']:checked").data('info') == true) {

                    var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });
                    if (state == false) {//si no es valido, mostrar validaciones al usuario
                        $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
                    }
                    return state;
                }
            }
        }
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

    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
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

//habilitar ddl municipios cliente cuando se seleccione un departamento cliente
//var CODDepto = 0;
//$("#departamento").change(function () {

//    $(this).parsley().validate();
//    var idDepto = $("#departamento option:selected").val();//obtener id del departamento seleccionado
//    CODDepto = idDepto;
//    var municipioDdl = $("#municipio");
//    var ciudadDdl = $("#ciudad");
//    var BarrioColoniaDdl = $("#barrioColonia");

//    if (idDepto != '') {

//        $("#spinnerCargando").css('display', '');
//        municipioDdl.empty();//vaciar dropdownlist de municipios
//        municipioDdl.append("<option value=''>Seleccione una opción</option>");//agregar opciones

//        $.ajax({
//            type: "POST",
//            url: "Aval_Registrar.aspx/CargarMunicipios",
//            data: JSON.stringify({ CODDepto: idDepto }),
//            contentType: 'application/json; charset=utf-8',
//            error: function (xhr, ajaxOptions, thrownError) {
//                MensajeError('Error al cargar municipios de este departamento');
//                $("#spinnerCargando").css('display', 'none');
//            },
//            success: function (data) {

//                var municipiosDelDepto = data.d;
//                $.each(municipiosDelDepto, function (i, iter) {
//                    municipioDdl.append("<option value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
//                });
//                municipioDdl.attr('disabled', false);//habilitar dropdownlist

//                ciudadDdl.empty();//vaciar dropdownlist de ciudades
//                ciudadDdl.append("<option value=''>Seleccione un municipio</option>");//agregar opciones
//                ciudadDdl.attr('disabled', true);//deshabilitar dropdownlist de ciudades

//                BarrioColoniaDdl.empty();//vaciar dropdownlist de barrios y colonias
//                BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
//                BarrioColoniaDdl.attr('disabled', true);//deshabilitar dropdownlist de barrios y colonias
//                $("#spinnerCargando").css('display', 'none');
//            }
//        });
//    }
//    else {
//        municipioDdl.empty();
//        municipioDdl.append("<option value=''>Seleccione un depto.</option>");
//        municipioDdl.attr('disabled', true);//deshabilitar dropdownlist de municipio

//        ciudadDdl.empty();//vaciar dropdownlist de ciudades
//        ciudadDdl.append("<option value=''>Seleccione un municipio</option>");
//        ciudadDdl.attr('disabled', true);//deshabilitar dropdownlist de ciudades

//        BarrioColoniaDdl.empty();//vaciar dropdownlist de barrios y colonias
//        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
//        BarrioColoniaDdl.attr('disabled', true);//deshabilitar dropdownlist de barrios y colonias
//    }
//});

//habilitar ddl ciudades cliente cuando se seleccione un municipio cliente
//var CODMunicipio = 0;
//$("#municipio").change(function () {

//    $(this).parsley().validate();
//    var idMunicipio = $("#municipio option:selected").val();//obtener id del municipio seleccionado
//    CODMunicipio = idMunicipio;
//    var ciudadDdl = $("#ciudad");
//    var BarrioColoniaDdl = $("#barrioColonia");

//    if (idMunicipio != '') {

//        $("#spinnerCargando").css('display', '');
//        ciudadDdl.empty();//vaciar dropdownlist
//        ciudadDdl.append("<option value=''>Seleccione una opción</option>");//agregar opciones

//        $.ajax({
//            type: "POST",
//            url: "Aval_Registrar.aspx/CargarPoblados",
//            data: JSON.stringify({ CODDepto: CODDepto, CODMunicipio: CODMunicipio }),
//            contentType: 'application/json; charset=utf-8',
//            error: function (xhr, ajaxOptions, thrownError) {
//                MensajeError('Error al cargar ciudades de este municipio');
//                $("#spinnerCargando").css('display', 'none');
//            },
//            success: function (data) {

//                var ciudadesDelMunicipio = data.d;
//                $.each(ciudadesDelMunicipio, function (i, iter) {
//                    ciudadDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
//                });
//                ciudadDdl.attr('disabled', false);//habilitar dropdownlist

//                BarrioColoniaDdl.empty();//vaciar dropdownlist de barrios y colonias
//                BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
//                BarrioColoniaDdl.attr('disabled', true);//deshabilitar dropdownlist de barrios y colonias
//                $("#spinnerCargando").css('display', 'none');
//            }
//        });
//    }
//    else {

//        ciudadDdl.empty();//vaciar dropdownlist de ciudades
//        ciudadDdl.append("<option value=''>Seleccione un municipio</option>");
//        ciudadDdl.attr('disabled', true);//deshabilitar dropdownlist de ciudades

//        BarrioColoniaDdl.empty();//vaciar dropdownlist de barrios y colonias
//        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
//        BarrioColoniaDdl.attr('disabled', true);//deshabilitar dropdownlist de barrios y colonias
//    }
//});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
//var CODCiudad = 0;
//$("#ciudad").change(function () {

//    var idCiudad = $("#ciudad option:selected").val();
//    CODCiudad = idCiudad;
//    $(this).parsley().validate();
//    var BarrioColoniaDdl = $("#barrioColonia");

//    if (idCiudad != '') {

//        $("#spinnerCargando").css('display', '');
//        BarrioColoniaDdl.empty();//vaciar dropdownlist
//        BarrioColoniaDdl.append("<option value=''>Seleccione una opción</option>");//agregar opciones
//        $.ajax({
//            type: "POST",
//            url: "Aval_Registrar.aspx/CargarBarrios",
//            data: JSON.stringify({ CODDepto: CODDepto, CODMunicipio: CODMunicipio, CODPoblado: CODCiudad }),
//            contentType: 'application/json; charset=utf-8',
//            error: function (xhr, ajaxOptions, thrownError) {
//                MensajeError('Error al cargar barrios de esta ciudad');
//                $("#spinnerCargando").css('display', 'none');
//            },
//            success: function (data) {

//                var barriosDeLaCiudad = data.d;//agregar ciudades que pertenecen al municipio seleccionado
//                $.each(barriosDeLaCiudad, function (i, iter) {
//                    BarrioColoniaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
//                });
//                BarrioColoniaDdl.attr('disabled', false);//habilitar dropdownlist
//                $("#spinnerCargando").css('display', 'none');
//            }
//        });
//    }
//    else {

//        BarrioColoniaDdl.empty();//vaciar dropdownlist
//        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
//        BarrioColoniaDdl.attr('disabled', true);//deshabilitar dropdownlist
//    }
//});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
//$("#barrioColonia").change(function () {
//    $(this).parsley().validate();
//});

//habilitar ddl municipios cliente cuando se seleccione un departamento cliente
//var CODDeptoEmpresa = 0;
//$("#departamentoEmpresa").change(function () {

//    $(this).parsley().validate();
//    var idDepto = $("#departamentoEmpresa option:selected").val();//obtener id del departamento seleccionado
//    CODDeptoEmpresa = idDepto;
//    var municipioEmpresaDdl = $("#municipioEmpresa");
//    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
//    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

//    if (idDepto != '') {

//        $("#spinnerCargando").css('display', '');
//        municipioEmpresaDdl.empty();//vaciar dropdownlist de municipios
//        municipioEmpresaDdl.append("<option value=''>Seleccione una opción</option>");//agregar opciones
//        $.ajax({
//            type: "POST",
//            url: "Aval_Registrar.aspx/CargarMunicipios",
//            data: JSON.stringify({ CODDepto: CODDeptoEmpresa }),
//            contentType: 'application/json; charset=utf-8',
//            error: function (xhr, ajaxOptions, thrownError) {
//                MensajeError('Error al cargar municipios de este departamento');
//                $("#spinnerCargando").css('display', 'none');
//            },
//            success: function (data) {

//                var municipiosDelDepto = data.d;
//                $.each(municipiosDelDepto, function (i, iter) {
//                    municipioEmpresaDdl.append("<option value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
//                });
//                municipioEmpresaDdl.attr('disabled', false);//habilitar dropdownlist

//                ciudadaEmpresaDdl.empty();//vaciar dropdownlist de ciudades
//                ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");
//                ciudadaEmpresaDdl.attr('disabled', true);//deshabilitar dropdownlist de ciudades

//                barrioColoniaEmpresaDdl.empty();//vaciar dropdownlist de barrios y colonias
//                barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
//                barrioColoniaEmpresaDdl.attr('disabled', true);//deshabilitar dropdownlist de barrios y colonias
//                $("#spinnerCargando").css('display', 'none');
//            }
//        });

//    }
//    else {
//        municipioEmpresaDdl.empty();//vaciar dropdownlist de municipio
//        municipioEmpresaDdl.append("<option value=''>Seleccione un depto.</option>");
//        municipioEmpresaDdl.attr('disabled', true);//deshabilitar dropdownlist de municipio

//        ciudadaEmpresaDdl.empty();//vaciar dropdownlist de ciudades
//        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");
//        ciudadaEmpresaDdl.attr('disabled', true);//deshabilitar dropdownlist de ciudades

//        barrioColoniaEmpresaDdl.empty();//vaciar dropdownlist de barrios y colonias
//        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
//        barrioColoniaEmpresaDdl.attr('disabled', true);//deshabilitar dropdownlist de barrios y colonias
//    }
//});

//habilitar ddl ciudades cliente cuando se seleccione un municipio cliente
//var CODMunicipioEmpresa = 0;
//$("#municipioEmpresa").change(function () {

//    $(this).parsley().validate();
//    var idMunicipio = $("#municipioEmpresa option:selected").val();//obtener id del municipio seleccionado
//    CODMunicipioEmpresa = idMunicipio;
//    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
//    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

//    if (idMunicipio != '') {

//        $("#spinnerCargando").css('display', '');
//        ciudadaEmpresaDdl.empty();//vaciar dropdownlist
//        ciudadaEmpresaDdl.append("<option value=''>Seleccione una opción</option>");//agregar opciones
//        $.ajax({
//            type: "POST",
//            url: "Aval_Registrar.aspx/CargarPoblados",
//            data: JSON.stringify({ CODDepto: CODDeptoEmpresa, CODMunicipio: CODMunicipioEmpresa }),
//            contentType: 'application/json; charset=utf-8',
//            error: function (xhr, ajaxOptions, thrownError) {
//                MensajeError('Error al cargar ciudades de este municipio');
//                $("#spinnerCargando").css('display', 'none');
//            },
//            success: function (data) {

//                var ciudadesDelMunicipio = data.d;//agregar ciudades que pertenecen al municipio seleccionado
//                $.each(ciudadesDelMunicipio, function (i, iter) {
//                    ciudadaEmpresaDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
//                });
//                ciudadaEmpresaDdl.attr('disabled', false);//habilitar dropdownlist
//                barrioColoniaEmpresaDdl.empty();//vaciar dropdownlist de barrios y colonias
//                barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
//                barrioColoniaEmpresaDdl.attr('disabled', true);//deshabilitar dropdownlist de barrios y colonias
//                $("#spinnerCargando").css('display', 'none');
//            }
//        });
//    }
//    else {
//        ciudadaEmpresaDdl.empty();//vaciar dropdownlist de ciudades
//        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");
//        ciudadaEmpresaDdl.attr('disabled', true);//deshabilitar dropdownlist de ciudades

//        barrioColoniaEmpresaDdl.empty();//vaciar dropdownlist de barrios y colonias
//        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
//        barrioColoniaEmpresaDdl.attr('disabled', true);//deshabilitar dropdownlist de barrios y colonias
//    }
//});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
//var CODCiudadEmpresa = 0;
//$("#ciudadEmpresa").change(function () {

//    $(this).parsley().validate();
//    var idCiudad = $("#ciudadEmpresa option:selected").val();//obtener id del municipio seleccionado
//    CODCiudadEmpresa = idCiudad;
//    var barriosEmpresaDdl = $("#barrioColoniaEmpresa");

//    if (idCiudad != '') {

//        $("#spinnerCargando").css('display', '');
//        barriosEmpresaDdl.empty();//vaciar dropdownlist
//        barriosEmpresaDdl.append("<option value=''>Seleccione una opción</option>");//agregar opciones
//        $.ajax({
//            type: "POST",
//            url: "Aval_Registrar.aspx/CargarBarrios",
//            data: JSON.stringify({ CODDepto: CODDeptoEmpresa, CODMunicipio: CODMunicipioEmpresa, CODPoblado: CODCiudadEmpresa }),
//            contentType: 'application/json; charset=utf-8',
//            error: function (xhr, ajaxOptions, thrownError) {
//                MensajeError('Error al cargar barrios de esta ciudad');
//                $("#spinnerCargando").css('display', 'none');
//            },
//            success: function (data) {

//                var barriosDeLaCiudad = data.d;//agregar ciudades que pertenecen al municipio seleccionado
//                $.each(barriosDeLaCiudad, function (i, iter) {
//                    barriosEmpresaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
//                });
//                barriosEmpresaDdl.attr('disabled', false);//habilitar dropdownlist
//                $("#spinnerCargando").css('display', 'none');
//            }
//        });
//    }
//    else {
//        barriosEmpresaDdl.empty();
//        barriosEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
//        barriosEmpresaDdl.attr('disabled', true);
//    }
//});

//habilitar ddl barrios y colonias empresa
//$("#barrioColoniaEmpresa").change(function () {
//    $(this).parsley().validate();
//});

//validar si se requiere información conyugal
$('input:radio[name="estadoCivil"]').change(function () {

    var requiereInformacionConyugal = $("input[name='estadoCivil']:checked").data('info');
    if (requiereInformacionConyugal == false) {
        $('input.infoConyugal').attr('disabled', true);
        $('#smartwizard').smartWizard("stepState", [3], "hide");//si no se requiere información conyugal, deshabilitar ese formulario
    }
    else if (requiereInformacionConyugal == true) {
        $('input.infoConyugal').attr('disabled', false);
        $('#smartwizard').smartWizard("stepState", [3], "show");
    }
});

function llenarDropDownLists() {

    estadoFuncionLlenarDDL = false;
    $("#spinnerCargando").css('display', '');
    $("select").empty();

    $.ajax({
        type: "POST",
        url: "Aval_Registrar.aspx/CargarListas",
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            var nacionalidadDdl = $("#nacionalidad");
            nacionalidadDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Nacionalidades, function (i, iter) {
                nacionalidadDdl.append("<option value='" + iter.fiIDNacionalidad + "'>" + iter.fcDescripcionNacionalidad + "</option>");
            });

            var CodigoPostalDdl = $("#barrioColoniaEmpresaAval");
            nacionalidadDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.CodigoPostal, function (i, iter) {
                CodigoPostalDdl.append("<option value='" + iter.fiCodBarrio + "'>" + iter.DirrecionCompleta + "</option>");
            });

            var CodigoPostalDdlColonia = $("#barrioColoniaAval");
            nacionalidadDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.CodigoPostal, function (i, iter) {
                CodigoPostalDdlColonia .append("<option value='" + iter.fiCodBarrio + "'>" + iter.DirrecionCompleta + "</option>");
            });

            var divEstadoCivil = $("#divEstadoCivil");// llenar select de estados civiles

            $.each(data.d.EstadosCiviles, function (i, iter) {
                divEstadoCivil.append("<div class='form-check form-check-inline'>" +
                    "<input data-info='" + iter.fbRequiereInformacionConyugal + "' class='form-check-input' type='radio' name ='estadoCivil' value='" + iter.fiIDEstadoCivil + "'>" +
                    "<label class='form-check-label'>" + iter.fcDescripcionEstadoCivil + "</label>" +
                    "</div>");
            });

            var tiempoConocerRefDdl = $("#tiempoConocerRef");// llenar select de tiempo de conocer referencia personal
            tiempoConocerRefDdl.append("<option value='Menos de un año'>-1 año</option>");
            tiempoConocerRefDdl.append("<option value='1'>1 año</option>");
            tiempoConocerRefDdl.append("<option value='2'>2 años</option>");
            tiempoConocerRefDdl.append("<option value='3'>+2 años</option>");

            var viviendaDdl = $("#vivivenda");// llenar select de vivivenda
            viviendaDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Vivienda, function (i, iter) {
                viviendaDdl.append("<option value='" + iter.fiIDVivienda + "'>" + iter.fcDescripcionVivienda + "</option>");
            });

            //var departamentoDdl = $("#departamento");// llenar select de departamentos
            //departamentoDdl.append("<option value=''>Seleccione una opción</option>");
            //$.each(data.d.Departamentos, function (i, iter) {
            //    departamentoDdl.append("<option value='" + iter.fiIDDepto + "'>" + iter.fcNombreDepto + "</option>");
            //});

            //$("#municipio").append("<option value=''>Seleccione un depto.</option>");// llenar lista de municipios
            //$("#ciudad").append("<option value=''>Seleccione un municipio</option>");// llenar lista de ciudad
            //$("#barrioColonia").append("<option value=''>Seleccione una ciudad</option>");// llenar lista de barrios y colonias

            //var departamentoEmpresaDdl = $("#departamentoEmpresa");// llenar select de departamento Empresa //aqui
            //departamentoEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
            //$.each(data.d.Departamentos, function (i, iter) {
            //    departamentoEmpresaDdl.append("<option value='" + iter.fiIDDepto + "'>" + iter.fcNombreDepto + "</option>");
            //});
            //$("#municipioEmpresa").append("<option value=''>Seleccione un depto</option>");// llenar select de municipios de la empresa            
            //$("#ciudadEmpresa").append("<option value=''>Seleccione un municipio</option>");// llenar select de ciudadEmpresa            
            //$("#barrioColoniaEmpresa").append("<option value=''>Seleccione una ciudad</option>");// llenar select de barrios y colonias de la empresa

            var parentescoRefDdl = $("#parentescoRef");// llenar select de parentescos de referencias personales (modal)
            parentescoRefDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Parentescos, function (i, iter) {
                parentescoRefDdl.append("<option value='" + iter.fiIDParentescos + "'>" + iter.fcDescripcionParentesco + "</option>");
            });

            var LenguajeEspanol = {
                feedback: 'Arrastra y suelta los archivos aqui',
                feedback2: 'Arrastra y suelta los archivos aqui',
                drop: 'Arrastra y suelta los archivos aqui',
                button: 'Buscar archivos',
                confirm: 'Confirmar',
                cancel: 'Cancelar'
            }

            var ChangeInput = '<div class="fileuploader-input">' +
                '<button type="button" class="fileuploader-input-button btn-sm BotonSubirArchivo"><span>${captions.button}</span></button>' +
                '</div>';

            var divDocumentacion = $("#DivDocumentacion");
            $.each(data.d.TipoDocumento, function (i, iter) {

                var IDInput = 'Doc' + iter.IDTipoDocumento;
                divDocumentacion.append("<div class='col-sm-2'>" +
                    "<label class='form-label'>" + iter.DescripcionTipoDocumento + "</label>" +
                    "<form action='Aval_Registrar.aspx?type=upload&doc=" + iter.IDTipoDocumento + "' method='post' enctype='multipart/form-data'>" +
                    "<input id='" + IDInput + "' type='file' name='files' data-tipo='" + iter.IDTipoDocumento + "' />" +
                    "</form>" +
                    "</div");

                $('#' + IDInput + '').fileuploader({
                    inputNameBrackets: false,
                    changeInput: ChangeInput,
                    theme: 'dragdrop',
                    limit: iter.CantidadMaximaDoucmentos, //limite de archivos a subir
                    maxSize: 10, //peso máximo de todos los archivos seleccionado en megas (MB)
                    fileMaxSize: 2, //peso máximo de un archivo
                    extensions: ['jpg', 'png'],// extensiones/formatos permitidos

                    upload: {
                        url: 'Aval_Registrar.aspx?type=upload&doc=' + iter.IDTipoDocumento,
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
                            // validar exito
                            if (data.isSuccess && data.files[0]) {
                                item.name = data.files[0].name;
                                item.html.find('.column-title > div:first-child').text(data.files[0].name).attr('title', data.files[0].name);
                            }
                            // validar si se produjo un error
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
                        $.post('Aval_Registrar.aspx?type=remove', { file: item.name });
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
                });
            });

            estadoFuncionLlenarDDL = true;
            $("#spinnerCargando").css('display', 'none');

        }
    });
}

$("#nacionalidad,#vivivenda").on('change', function () {
    $($(this)).parsley().validate();
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
function resetForm($form) {
    $form.find('input:text, input:password, input:file,input[type="date"],input[type="email"], select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .prop('checked', false).removeAttr('selected');
}
function dateFormat(jsondate) {

    var result = FechaFormato(jsondate);
    var fechaNormal = result.split(' ')[0];
    var dateVal = new Date();
    var numeroMes = parseInt(fechaNormal.substring(3, 5).replace(/\//g, ''));
    var mes = fechaNormal.substring(3, 5);
    var anio = fechaNormal.substring(6, 10);
    var dia = fechaNormal.substring(0, 2);
    dateVal = anio + "-" + mes + "-" + dia;
    return dateVal;
}
function FechaFormato(pFecha) {
    if (!pFecha)
        return "Sin modificaciones";
    var fechaString = pFecha.substr(6, 19);
    var fechaActual = new Date(parseInt(fechaString));
    var mes = pad2(fechaActual.getMonth() + 1);
    var dia = pad2(fechaActual.getDate());
    var anio = fechaActual.getFullYear();
    var hora = pad2(fechaActual.getHours());
    var minutos = pad2(fechaActual.getMinutes());
    var segundos = pad2(fechaActual.getSeconds().toString());
    var FechaFinal = dia + "/" + mes + "/" + anio + " " + hora + ":" + minutos + ":" + segundos;
    return FechaFinal;
}
function pad2(number) {
    return (number < 10 ? '0' : '') + number
}
function FechaFormatoGuiones(pFecha) {
    if (!pFecha)
        return "Sin modificaciones";
    var fechaString = pFecha.substr(6, 19);
    var fechaActual = new Date(parseInt(fechaString));
    var mes = pad2(fechaActual.getMonth() + 1);
    var dia = pad2(fechaActual.getDate());
    var anio = fechaActual.getFullYear();
    var hora = pad2(fechaActual.getHours());
    var minutos = pad2(fechaActual.getMinutes());
    var segundos = pad2(fechaActual.getSeconds().toString());
    var FechaFinal = anio + "/" + mes + "/" + dia + " " + hora + ":" + minutos + ":" + segundos;
    return FechaFinal;
}