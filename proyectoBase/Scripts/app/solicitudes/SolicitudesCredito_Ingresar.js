if ((localStorage.getItem('precalificado') == null || localStorage.getItem('precalificado') == 'null') && localStorage.getItem('RespaldoInformacionPrestamo') == null) {
    window.location = 'SolicitudesCredito_Precalificar.aspx';
}
var clienteNuevo = true;
var ListaMunicipios = [];
var ListaCiudades = [];
var ListaBarriosColonias = [];
var objPrecalificado = JSON.parse(localStorage.getItem('precalificado'));
var ingresoInicio = '';
var estadoFuncionLlenarDDL = false;
var estadoFuncionRecuperarInfoCliente = false;
var estadoFuncionRecuperarRespaldos = false;

$(document).ready(function () {

    var btnFinish = $('<button type="button" id="btnGuardarSolicitud"></button>').text('Finalizar')
        .addClass('btn btn-info')
        .css('display', 'none')
        .on('click', function () {
            $('#frmSolicitud').submit(function (e) { e.preventDefault() });

            $('#frmSolicitud').parsley().validate({ group: 'informacionPrestamo', force: true });
            var modelStateFormPrestamo = $('#frmSolicitud').parsley().isValid({ group: 'informacionPrestamo' });

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
            if (cantidadReferencias == 0) {

                if (modelStateFormPrestamo == true && modelStateFormPersonal == true && modelStateFormDomiciliar == true && modelStateFormLaboral == true && modelStateFormConyugal == true) {
                    iziToast.warning({
                        title: 'Atención',
                        message: 'Las referencias personales son requeridas.'
                    });
                }
            }
            if (modelStateFormPrestamo == true && modelStateFormPersonal == true && modelStateFormDomiciliar == true && modelStateFormLaboral == true && modelStateFormConyugal == true && cantidadReferencias > 0) {

                var SolicitudesMaster = {
                    fiIDCliente: clienteID,
                    fiIDTipoPrestamo: $("#tipoPrestamo").val(),
                    fnPrima: $("#txtPrima").val().replace(/,/g, ''),
                    fnValorVehiculo: $("#txtValorVehiculo").val().replace(/,/g, ''),
                    fcTipoSolicitud: $("#tipoSolicitud").val(),
                    fdValorPmoSugeridoSeleccionado: $("#pmoSugeridoSeleccionado option:selected").val(),
                    fiPlazoPmoSeleccionado: $("#plazoPmoSeleccionado").val(),
                    fdIngresoPrecalificado: $("#ingresosPrecalificado").val().replace(/,/g, ''),
                    fdObligacionesPrecalificado: $("#obligacionesPrecalificado").val().replace(/,/g, ''),
                    fdDisponiblePrecalificado: $("#disponiblePrecalificado").val().replace(/,/g, '')
                };
                var bitacora = {
                    fdEnIngresoInicio: localStorage.getItem("EnIngresoInicio"),
                    fdEnIngresoFin: ''
                };
                var ClienteMaster = {
                    fiIDCliente: clienteID,
                    fcIdentidadCliente: $("#identidadCliente").val(),
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
                };
                var ClientesInformacionLaboral = {
                    fcNombreTrabajo: $("#nombreDelTrabajo").val(),
                    fiIngresosMensuales: $("#ingresosMensuales").val().replace(/,/g, ''),
                    fcPuestoAsignado: $("#puestoAsignado").val(),
                    fcFechaIngreso: $("#fechaIngreso").val(),
                    fdTelefonoEmpresa: $("#telefonoEmpresa").val(),
                    fcExtensionRecursosHumanos: $("#extensionRRHH").val(),
                    fcExtensionCliente: $("#extensionCliente").val(),
                    fiCiudadEmpresa: $("#ciudadEmpresa").val(),
                    fiIDBarrioColonia: $("#barrioColoniaEmpresa").val(),
                    fcDireccionDetalladaEmpresa: $("#direccionDetalladaEmpresa").val(),
                    fcReferenciasDireccionDetallada: $("#referenciaDireccionDetalladaEmpresa").val(),
                    fcFuenteOtrosIngresos: $("#fuenteOtrosIngresos").val(),
                    fiValorOtrosIngresosMensuales: $("#valorOtrosIngresos").val().replace(/,/g, ''),
                };
                var ClientesInformacionDomiciliar = {
                    fiIDCiudad: $("#ciudad").val(),
                    fiIDBarrioColonia: $("#barrioColonia").val(),
                    fcTelefonoCasa: $("#telefonoCasa").val(),
                    fcTelefonoMovil: $("#telefonoMovil").val(),
                    fcDireccionDetallada: $("#direccionDetallada").val(),
                    fcReferenciasDireccionDetallada: $("#referenciaDireccionDetallada").val()
                };
                var ClientesInformacionConyugal = {
                    fcNombreCompletoConyugue: $("#nombresConyugue").val() + ' ' + $("#apellidosConyugue").val(),
                    fcIndentidadConyugue: $("#identidadConyugue").val(),
                    fdFechaNacimientoConyugue: $("#fechaNacimientoConyugue").val(),
                    fcTelefonoConyugue: $("#telefonoConyugue").val(),
                    fcLugarTrabajoConyugue: $("#lugarTrabajoConyugue").val(),
                    fcIngresosMensualesConyugue: $("#ingresoMensualesConyugue").val().replace(/,/g, ''),
                    fcTelefonoTrabajoConyugue: $("#telefonoTrabajoConyugue").val()
                };
                ClientesReferencias = listaClientesReferencias;

                $.ajax({
                    type: "POST",
                    url: 'SolicitudesCredito_Ingresar.aspx/addSolicitud',
                    data: JSON.stringify({ clienteNuevo: clienteNuevo, SolicitudesMaster: SolicitudesMaster, ClienteMaster: ClienteMaster, ClientesInformacionLaboral: ClientesInformacionLaboral, ClientesInformacionDomiciliar: ClientesInformacionDomiciliar, ClientesInformacionConyugal: ClientesInformacionConyugal, ClientesReferencias: ClientesReferencias, bitacora: bitacora }),
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
                        else { MensajeError(data.d.message); }
                    }
                });
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
            $("#prev-btn").addClass('disabled').css('display', 'none');// si es el ultimo paso, deshabilitar el boton siguiente
        } else if (stepPosition === 'final') {
            $("#next-btn").addClass('disabled');
            $("#btnGuardarSolicitud").css('display', '');// si no es ninguna de las anteriores, habilitar todos los botones
        } else {
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
            $("#btnGuardarSolicitud").css('display', 'none');
        }
        if (stepNumber == 4) {//inicializar validaciones de los formularios
            $('#frmSolicitud').parsley().reset();
        }
    });    
    
    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');// habilitar boton siguiente al reiniciar steps
    });

    // cuando se deja un paso (realizar validaciones)
    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

        if ($("input[name='estadoCivil']:checked").data('info') == false) {
            $('#smartwizard').smartWizard("stepState", [4], "hide");//si no requere informacion personal, saltarse esa pestaña
        }

        // validar solo si se quiere ir hacia el siguiente paso aqui realizar las validaciones
        if (stepDirection == 'forward') {

            if (stepNumber == 0) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPrestamo', excluded: ':disabled' });// validar informacion prestamo

                if (state == true) {
                    guardarRespaldoInformacionPrestamo();//si el formulario es valido, guardar respaldo en el localstorage
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPrestamo', excluded: ':disabled', force: true });//si no es valido, mostrar validaciones al usuario
                }
                var valorVehiculoValidacion = parseFloat($("#txtValorVehiculo").val().replace(/,/g, '')).toFixed(2);
                var primaValidacion = parseFloat($("#txtPrima").val().replace(/,/g, '')).toFixed(2);
                var valorFinanciarValidacion = valorVehiculoValidacion - primaValidacion;
                var pmoSeleccionadoValidacion = parseFloat($("#pmoSugeridoSeleccionado :selected").val()).toFixed(2);
                var DescripcionPrestamo = $("#tipoPrestamo option:selected").text();

                //valor a financiar mayor al pmo sugerido
                if (parseInt(pmoSeleccionadoValidacion) < valorFinanciarValidacion) {
                    state = false;
                    MensajeError('Valor a financiar mayor al pmo sugerido');
                }
                //prima mayor a valor vehiculo
                if (parseInt(primaValidacion) > valorVehiculoValidacion) {
                    state = false;
                    MensajeError('Prima mayor a valor vehiculo');
                }
                //prima minima de 10% en caso de ser moto o efectivo
                if (DescripcionPrestamo == 'PRESTAMO VEHICULO MOTO' || DescripcionPrestamo == 'PRESTAMO EFECTIVO') {
                    var primaMinima = (valorVehiculoValidacion * 10) / 100;
                    if (primaValidacion < primaMinima) {
                        //state = false;
                        MensajeError('Prima minima de 10%');
                    }
                }
                //monto minimo a financiar en caso de ser moto o efectivo
                if (DescripcionPrestamo == 'PRESTAMO VEHICULO MOTO' || DescripcionPrestamo == 'PRESTAMO EFECTIVO') {
                    var montoMinimo = 6000.00;
                    if (valorFinanciarValidacion < montoMinimo) {
                        //state = false;
                        MensajeError('Monto minimo a financiar es 6,000');
                    }
                }
                return state;
            }

            //validar informacion personal
            if (stepNumber == 1) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' });
                if (state == true) {
                    guardarRespaldoInformacionPersonal();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
                }
                return state;
            }
            //validar informacion domiciliar
            if (stepNumber == 2) {                
                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomiciliar' });

                if (state == true) {
                    guardarRespaldoInformacionDomiciliar();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionDomiciliar', force: true });
                }
                return state;
            }
            //validar informacion laboral
            if (stepNumber == 3) {                
                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

                if (state == true) {
                    guardarRespaldoInformacionLaboral();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
                }
                return state;
            }
            //validar informacion conyugal
            if (stepNumber == 4) {
                
                if ($("input[name='estadoCivil']:checked").data('info') == true) {
                    var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });

                    if (state == true) {
                        guardarRespaldoInformacionConyugal();
                    }
                    else {
                        $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
                    }
                    return state;
                }
            }
            //validar referencias personales
            if (stepNumber == 5) {                
                var state = false;

                //si la lista de referencias no es nula y contiene una o mas referencias, guardar un respaldo de estas
                if (listaClientesReferencias != null) {
                    if (listaClientesReferencias.length > 0) {
                        guardarRespaldoReferenciasPersonales();
                        state = true;
                    }
                }
                if (cantidadReferencias > 0) { state = true; }
                else { MensajeError('Las referencias personales son requeridas'); }
                return state;
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
    if (localStorage.getItem("EnIngresoInicio") == null) {
        obtenerFechaActual();
    }
    if (localStorage.getItem('precalificado') != null && localStorage.getItem('precalificado') != 'null') {
        recuperarInformacionPrecalificado();
    }

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
$("#departamento").change(function () {

    $(this).parsley().validate();
    var idDepto = $("#departamento").val();
    var municipioDdl = $("#municipio");
    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idDepto != '') {
        municipioDdl.empty();
        municipioDdl.append("<option value=''>Seleccione una opción</option>");
        var municipiosDelDepto = ListaMunicipios.filter(d => d.fiIDDepto == idDepto);
        $.each(municipiosDelDepto, function (i, iter) {
            municipioDdl.append("<option value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
        });
        municipioDdl.attr('disabled', false);
        ciudadDdl.empty();
        ciudadDdl.append("<option value=''>Seleccione un municipio</option>");
        ciudadDdl.attr('disabled', true);
        BarrioColoniaDdl.empty();
        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
        BarrioColoniaDdl.attr('disabled', true);
    }
    else {
        municipioDdl.empty();
        municipioDdl.append("<option value=''>Seleccione un depto.</option>");
        municipioDdl.attr('disabled', true);
        ciudadDdl.empty();
        ciudadDdl.append("<option value=''>Seleccione un municipio</option>");
        ciudadDdl.attr('disabled', true);
        BarrioColoniaDdl.empty();
        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
        BarrioColoniaDdl.attr('disabled', true);
    }
});

//habilitar ddl ciudades cliente cuando se seleccione un municipio cliente
$("#municipio").change(function () {

    $(this).parsley().validate();
    var idMunicipio = $("#municipio").val();
    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idMunicipio != '') {
        ciudadDdl.empty();
        ciudadDdl.append("<option value=''>Seleccione una opción</option>");
        var ciudadesDelMunicipio = ListaCiudades.filter(d => d.fiIDMunicipio == idMunicipio);
        $.each(ciudadesDelMunicipio, function (i, iter) {
            ciudadDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
        });
        ciudadDdl.attr('disabled', false);
        BarrioColoniaDdl.empty();
        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
        BarrioColoniaDdl.attr('disabled', true);
    }
    else {
        ciudadDdl.empty();
        ciudadDdl.append("<option value=''>Seleccione un municipio</option>");
        ciudadDdl.attr('disabled', true);
        BarrioColoniaDdl.empty();
        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
        BarrioColoniaDdl.attr('disabled', true);
    }
});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
$("#ciudad").change(function () {

    $(this).parsley().validate();
    var idCiudad = $("#ciudad").val();
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idCiudad != '') {
        BarrioColoniaDdl.empty();
        BarrioColoniaDdl.append("<option value=''>Seleccione una opción</option>");
        var barriosDeLaCiudad = ListaBarriosColonias.filter(d => d.fiIDCiudad == idCiudad);
        $.each(barriosDeLaCiudad, function (i, iter) {
            BarrioColoniaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
        });
        BarrioColoniaDdl.attr('disabled', false);
    }
    else {
        BarrioColoniaDdl.empty();
        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
        BarrioColoniaDdl.attr('disabled', true);
    }
});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
$("#barrioColonia").change(function () {
    $(this).parsley().validate();
});

//habilitar ddl municipios cliente cuando se seleccione un departamento cliente
$("#departamentoEmpresa").change(function () {

    $(this).parsley().validate();
    var idDepto = $("#departamentoEmpresa").val();
    var municipioEmpresaDdl = $("#municipioEmpresa");
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idDepto != '') {
        municipioEmpresaDdl.empty();
        municipioEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
        var municipiosDelDepto = ListaMunicipios.filter(d => d.fiIDDepto == idDepto);
        $.each(municipiosDelDepto, function (i, iter) {
            municipioEmpresaDdl.append("<option value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
        });
        municipioEmpresaDdl.attr('disabled', false);
        ciudadaEmpresaDdl.empty();
        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");
        ciudadaEmpresaDdl.attr('disabled', true);
        barrioColoniaEmpresaDdl.empty();
        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
        barrioColoniaEmpresaDdl.attr('disabled', true);
    }
    else {
        municipioEmpresaDdl.empty();
        municipioEmpresaDdl.append("<option value=''>Seleccione un depto.</option>");
        municipioEmpresaDdl.attr('disabled', true);
        ciudadaEmpresaDdl.empty();
        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");
        ciudadaEmpresaDdl.attr('disabled', true);
        barrioColoniaEmpresaDdl.empty();
        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
        barrioColoniaEmpresaDdl.attr('disabled', true);
    }
});

//habilitar ddl ciudades cliente cuando se seleccione un municipio cliente
$("#municipioEmpresa").change(function () {

    $(this).parsley().validate();
    var idMunicipio = $("#municipioEmpresa").val();
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idMunicipio != '') {
        ciudadaEmpresaDdl.empty();
        ciudadaEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
        var ciudadesDelMunicipio = ListaCiudades.filter(d => d.fiIDMunicipio == idMunicipio);
        $.each(ciudadesDelMunicipio, function (i, iter) {
            ciudadaEmpresaDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
        });
        ciudadaEmpresaDdl.attr('disabled', false);
        barrioColoniaEmpresaDdl.empty();
        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
        barrioColoniaEmpresaDdl.attr('disabled', true);    }
    else {
        ciudadaEmpresaDdl.empty();
        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");
        ciudadaEmpresaDdl.attr('disabled', true);
        barrioColoniaEmpresaDdl.empty();
        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
        barrioColoniaEmpresaDdl.attr('disabled', true);
    }
});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
$("#ciudadEmpresa").change(function () {

    $(this).parsley().validate();
    var idCiudad = $("#ciudadEmpresa").val();
    var barriosEmpresaDdl = $("#barrioColoniaEmpresa");
    if (idCiudad != '') {
        barriosEmpresaDdl.empty();
        barriosEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
        var barriosDeLaCiudad = ListaBarriosColonias.filter(d => d.fiIDCiudad == idCiudad);
        $.each(barriosDeLaCiudad, function (i, iter) {
            barriosEmpresaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
        });
        barriosEmpresaDdl.attr('disabled', false);    }
    else {
        barriosEmpresaDdl.empty();
        barriosEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
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

    if (requiereInformacionConyugal == false) {
        $('input.infoConyugal').attr('disabled', true);
        $('#smartwizard').smartWizard("stepState", [4], "hide");//si no se requiere información conyugal, deshabilitar ese formulario
    }
    else if (requiereInformacionConyugal == true) {
        $('input.infoConyugal').attr('disabled', false); 
        $('#smartwizard').smartWizard("stepState", [4], "show");
    }
});

// agregar referencias personales (abrir modal)
$("#btnNuevaReferencia").click(function () {

    resetForm($('#addReferencia-form'));
    $('#addReferencia-form').parsley().reset();
});

// agregar referencias personales (submit)
var cantidadReferencias = 0;
var listaClientesReferencias = [];
$('#addReferencia-form').submit(function (e) {

    //validar formulario de agregar referencia personal
    if ($(this).parsley().isValid()) {
        e.preventDefault();
        var nombreref = $("#nombreCompletoRef").val();
        var lugarTrabajoRef = $("#lugarTrabajoRef").val();
        var tiempoConocerRefRow = parseInt($("#tiempoConocerRef").val()) <= 2 ? $("#tiempoConocerRef").val() + ' años' : 'Más de 2 años';
        var tiempoConocerRef = parseInt($("#tiempoConocerRef").val());
        var telefonoRef = $("#telefonoRef").val();
        var parentescoRef = $("#parentescoRef").val();
        var parentescoText = $("#parentescoRef :selected").text();
        tableReferencias.row.add([
            nombreref,
            lugarTrabajoRef,
            tiempoConocerRefRow,
            telefonoRef,
            parentescoText,
        ]).draw(false);

        $("#modalAddReferencia").modal('hide');
        cantidadReferencias = cantidadReferencias + 1;
        var referencia = {
            fiIDReferencia: 0,
            fiIDCliente: 0,
            fcNombreCompletoReferencia: nombreref,
            fcLugarTrabajoReferencia: lugarTrabajoRef,
            fiTiempoConocerReferencia: tiempoConocerRef,
            fcTelefonoReferencia: telefonoRef,
            fiIDParentescoReferencia: parentescoRef,
            fcDescripcionParentesco: parentescoText
        }
        listaClientesReferencias.push(referencia);
    }
});

// inicializar datatables
var tableReferencias = $('#datatable-buttons').DataTable({
    "searching": false,
    "lengthChange": false,
    "pageLength": 4,
    "paging": true,
    "responsive": true,
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

function llenarDropDownLists() {
    
    estadoFuncionLlenarDDL = false;
    $("#spinnerCargando").css('display', '');
    $("select").empty();

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Ingresar.aspx/getDDLS",
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {
            $("#tipoSolicitud").append("<option value=''>Seleccione una opción</option>");
            $("#tipoSolicitud").append("<option value='NUEVO' selected>NUEVO</option>");

            if (localStorage.getItem('RespaldoInformacionPrestamo') != null) {
                var respaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));

                if (respaldoInformacionPrestamo.clienteID != 0) {
                    var tipoSolicitudDdl = $("#tipoSolicitud");
                    tipoSolicitudDdl.empty();
                    tipoSolicitudDdl.append("<option value=''>Seleccione una opción</option>");
                    tipoSolicitudDdl.append("<option " + (respaldoInformacionPrestamo.tipoSolicitud == 'REFINANCIAMIENTO' ? 'selected' : '') + " value='REFINANCIAMIENTO'>REFINANCIAMIENTO</option>");
                    tipoSolicitudDdl.append("<option " + (respaldoInformacionPrestamo.tipoSolicitud == 'RENOVACION' ? 'selected' : '') + " value='RENOVACION' selected>RENOVACION</option>");
                    tipoSolicitudDdl.attr('tipoSolicitud', false);
                    tipoSolicitudDdl.attr('tipoSolicitud', false);
                }
            }
            var tipoPrestamoDdl = $("#tipoPrestamo");
            tipoPrestamoDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.TipoPrestamo, function (i, iter) {
                tipoPrestamoDdl.append("<option value='" + iter.fiIDTipoPrestamo + "'>" + iter.fcDescripcion + "</option>");
            });

            var nacionalidadDdl = $("#nacionalidad");
            nacionalidadDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Nacionalidades, function (i, iter) {
                nacionalidadDdl.append("<option value='" + iter.fiIDNacionalidad + "'>" + iter.fcDescripcionNacionalidad + "</option>");
            });

            var divEstadoCivil = $("#divEstadoCivil");

            $.each(data.d.EstadosCiviles, function (i, iter) {
                divEstadoCivil.append("<div class='form-check form-check-inline'>" +
                        "<input data-info='"+ iter.fbRequiereInformacionConyugal+"' class='form-check-input' type='radio' name ='estadoCivil' value='"+ iter.fiIDEstadoCivil +"'>" +
                        "<label class='form-check-label'>" + iter.fcDescripcionEstadoCivil+ "</label>"+
                    "</div>");
            });

            var tiempoConocerRefDdl = $("#tiempoConocerRef");
            tiempoConocerRefDdl.append("<option value='Menos de un año'>-1 año</option>");
            tiempoConocerRefDdl.append("<option value='1'>1 año</option>");
            tiempoConocerRefDdl.append("<option value='2'>2 años</option>");
            tiempoConocerRefDdl.append("<option value='3'>+2 años</option>");

            var viviendaDdl = $("#vivivenda");
            viviendaDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Vivienda, function (i, iter) {
                viviendaDdl.append("<option value='" + iter.fiIDVivienda + "'>" + iter.fcDescripcionVivienda + "</option>");
            });

            var departamentoDdl = $("#departamento");
            departamentoDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Departamentos, function (i, iter) {
                departamentoDdl.append("<option value='" + iter.fiIDDepto + "'>" + iter.fcNombreDepto + "</option>");
            });

            $("#municipio").append("<option value=''>Seleccione un depto.</option>");
            ListaMunicipios = [];
            $.each(data.d.Municipios, function (i, iter) {
                ListaMunicipios.push(iter);
            });

            var respaldoInformacionDomiciliar = [];
            if (localStorage.getItem('RespaldoInformacionDomiciliar') != null) {

                respaldoInformacionDomiciliar = JSON.parse(localStorage.getItem('RespaldoInformacionDomiciliar'));
                var municipioDdl = $("#municipio");
                municipioDdl.empty();
                municipioDdl.append("<option value=''>Seleccione una opción</option>");
                var municipiosDelDepto = ListaMunicipios.filter(d => d.fiIDDepto == respaldoInformacionDomiciliar.departamento);
                $.each(municipiosDelDepto, function (i, iter) {
                    municipioDdl.append("<option " + (iter.fiIDMunicipio == respaldoInformacionDomiciliar.municipio ? "selected" : "") + " value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
                });

                municipioDdl.attr('disabled', false);
                municipioDdl.attr('readonly', false);
            }

            $("#ciudad").append("<option value=''>Seleccione un municipio</option>");
            ListaCiudades = [];
            $.each(data.d.Ciudades, function (i, iter) {
                ListaCiudades.push(iter);
            });

            if (localStorage.getItem('RespaldoInformacionDomiciliar') != null) {

                var ciudadDdl = $("#ciudad");
                ciudadDdl.empty();
                ciudadDdl.append("<option value=''>Seleccione una opción</option>");
                var ciudadesDelMunicipio = ListaCiudades.filter(d => d.fiIDMunicipio == respaldoInformacionDomiciliar.municipio);
                $.each(ciudadesDelMunicipio, function (i, iter) {
                    ciudadDdl.append("<option  " + (iter.fiIDCiudad == respaldoInformacionDomiciliar.ciudad ? "selected" : "") + " value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
                });
                ciudadDdl.attr('disabled', false);
                ciudadDdl.attr('readonly', false);
            }

            $("#barrioColonia").append("<option value=''>Seleccione una ciudad</option>");
            ListaBarriosColonias = [];
            $.each(data.d.BarriosColonias, function (i, iter) {
                ListaBarriosColonias.push(iter);
            });

            if (localStorage.getItem('RespaldoInformacionDomiciliar') != null) {

                var barrioColoniaDdl = $("#barrioColonia");
                barrioColoniaDdl.empty();
                barrioColoniaDdl.append("<option value=''>Seleccione una opción</option>");
                var barriosDeLaCiudad = ListaBarriosColonias.filter(d => d.fiIDCiudad == respaldoInformacionDomiciliar.ciudad);
                $.each(barriosDeLaCiudad, function (i, iter) {
                    barrioColoniaDdl.append("<option " + (iter.fiIDBarrioColonia == respaldoInformacionDomiciliar.barrioColonia ? "selected" : "") + " value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
                });

                barrioColoniaDdl.attr('disabled', false);
                barrioColoniaDdl.attr('readonly', false);
            }
            var departamentoEmpresaDdl = $("#departamentoEmpresa");
            departamentoEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Departamentos, function (i, iter) {
                departamentoEmpresaDdl.append("<option value='" + iter.fiIDDepto + "'>" + iter.fcNombreDepto + "</option>");
            });

            $("#municipioEmpresa").append("<option value=''>Seleccione un depto</option>");
            var respaldoInformacionLaboral = [];
            if (localStorage.getItem('RespaldoInformacionLaboral') != null) {

                respaldoInformacionLaboral = JSON.parse(localStorage.getItem('RespaldoInformacionLaboral'));
                var municipioEmpresaDdl = $("#municipioEmpresa");
                municipioEmpresaDdl.empty();
                municipioEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
                var municipiosDelDepto = ListaMunicipios.filter(d => d.fiIDDepto == respaldoInformacionLaboral.departamentoEmpresa);
                $.each(municipiosDelDepto, function (i, iter) {
                    municipioEmpresaDdl.append("<option " + (iter.fiIDMunicipio == respaldoInformacionLaboral.municipioEmpresa ? "selected" : "") + " value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
                });
                municipioEmpresaDdl.attr('disabled', false);
                municipioEmpresaDdl.attr('readonly', false);
            }
            $("#ciudadEmpresa").append("<option value=''>Seleccione un municipio</option>");

            if (localStorage.getItem('RespaldoInformacionLaboral') != null) {

                var ciudadEmpresaDdl = $("#ciudadEmpresa");
                ciudadEmpresaDdl.empty();
                ciudadEmpresaDdl.append("<option value=''>Seleccione una opción</option>");

                var ciudadesDelMunicipio = ListaCiudades.filter(d => d.fiIDMunicipio == respaldoInformacionLaboral.municipioEmpresa);
                $.each(ciudadesDelMunicipio, function (i, iter) {
                    ciudadEmpresaDdl.append("<option  " + (iter.fiIDCiudad == respaldoInformacionLaboral.ciudadEmpresa ? "selected" : "") + " value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
                });
                ciudadEmpresaDdl.attr('disabled', false);
                ciudadEmpresaDdl.attr('readonly', false);
            }

            $("#barrioColoniaEmpresa").append("<option value=''>Seleccione una ciudad</option>");
            if (localStorage.getItem('RespaldoInformacionLaboral') != null) {

                var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");
                barrioColoniaEmpresaDdl.empty();
                barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
                var barriosDeLaCiudad = ListaBarriosColonias.filter(d => d.fiIDCiudad == respaldoInformacionLaboral.ciudadEmpresa);
                $.each(barriosDeLaCiudad, function (i, iter) {
                    barrioColoniaEmpresaDdl.append("<option " + (iter.fiIDBarrioColonia == respaldoInformacionLaboral.barrioColoniaEmpresa ? "selected" : "") + " value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
                });

                barrioColoniaEmpresaDdl.attr('disabled', false);
                barrioColoniaEmpresaDdl.attr('readonly', false);
            }

            var parentescoRefDdl = $("#parentescoRef");
            parentescoRefDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Parentescos, function (i, iter) {
                parentescoRefDdl.append("<option value='" + iter.fiIDParentescos + "'>" + iter.fcDescripcionParentesco + "</option>");
            });
            estadoFuncionLlenarDDL = true;
            
            VerificarExistenciaCliente();
            recuperarRespaldos();
            if (estadoFuncionRecuperarInfoCliente == true && estadoFuncionRecuperarRespaldos == true) {
                $("#spinnerCargando").css('display', 'none');
            }
        }
    });
}

function cargarOrigenes(COD) {

    $("#spinnerCargando").css('display', '');
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Ingresar.aspx/CargarOrigenes",
        data: JSON.stringify({ COD: COD }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar catalogo de orígenes');
            if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
                $("#spinnerCargando").css('display', 'none');
            }
        },
        success: function (data) {
            if (data.d != null) {
                var origenesDdl = $("#ddlOrigen");
                origenesDdl.empty();
                origenesDdl.append("<option value=''>Seleccione una opción</option>");
                var listaOrigenes = data.d;
                $.each(listaOrigenes, function (i, iter) {
                    origenesDdl.append("<option value='" + iter.fiIDOrigen + "'>" + iter.fcOrigen + "</option>");
                });
                $("#spinnerCargando").css('display', 'none');
            }
            else {
                if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
                    $("#spinnerCargando").css('display', 'none');
                }
            }
        }
    });
}

//función llenar datatable de clientes para ser seleccionados
var listadoClientesGlobal = [];
var infoCompletaCliente = {};

function VerificarExistenciaCliente() {

    estadoFuncionRecuperarInfoCliente = false;
    $("#spinnerCargando").css('display', '');

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Ingresar.aspx/ObtenerInformacionCliente",
        data: JSON.stringify({ identidad: objPrecalificado.identidad }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al verificar existencia del cliente');
            estadoFuncionCargarInformacionCliente = true;
            if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
                $("#spinnerCargando").css('display', 'none');
            }
        },
        success: function (data) {
            if (data.d != null) {
                $("#spinnerCargando").css('display', '');

                var clientePrecalificadoExistente = false;
                infoCompletaCliente = data.d;
                clientePrecalificadoExistente = true;
                cargarInformacionCompletaDelCliente(infoCompletaCliente);
            }
            else {
                estadoFuncionCargarInformacionCliente = true;
                if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
                    $("#spinnerCargando").css('display', 'none');
                }
            }            
        }
    });
}

//ID CLTE
var clienteID = 0;

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

function MensajeInformacion(mensaje) {
    iziToast.info({
        title: 'Info',
        message: mensaje
    });
}

function pad2(number) {
    return (number < 10 ? '0' : '') + number
}

function FechaFormato(pFecha) {    
    if (!pFecha)
        return "Sin modificaciones";
    var fechaString = pFecha.substr(6, 19);
    var fechaActual = new Date(parseInt(fechaString));
    var mes = fechaActual.getMonth() + 1;
    var dia = pad2(fechaActual.getDate());
    var anio = fechaActual.getFullYear();
    var hora = pad2(fechaActual.getHours());
    var minutos = pad2(fechaActual.getMinutes());
    var segundos = pad2(fechaActual.getSeconds().toString());
    var FechaFinal = dia + "/" + mes + "/" + anio + " " + hora + ":" + minutos + ":" + segundos;
    return FechaFinal;
}

function dateFormat(jsondate) {
    var result = FechaFormato(jsondate);
    var fechaNormal = result.split(' ')[0];
    var dateVal = new Date();
    var numeroMes = parseInt(fechaNormal.substring(3, 5).replace(/\//g, ''));
    var mes = numeroMes < 10 ? '0' + fechaNormal.substring(3, 4) : fechaNormal.substring(3, 5);
    var anio = numeroMes < 10 ? fechaNormal.substring(5, 9) : fechaNormal.substring(6, 10);
    var dia = fechaNormal.substring(0, 2);
    dateVal = anio + "-" + mes + "-" + dia;
    return dateVal;
}

function obtenerFechaActual() {
    var dt = new Date();
    var fechaActual = `${
        dt.getFullYear().toString().padStart(4, '0')}-${
        (dt.getMonth() + 1).toString().padStart(2, '0')}-${
        dt.getDate().toString().padStart(2, '0')} ${
        dt.getHours().toString().padStart(2, '0')}:${
        dt.getMinutes().toString().padStart(2, '0')}:${
        dt.getSeconds().toString().padStart(2, '0')}`;
    ingresoInicio = fechaActual;
    localStorage.setItem("EnIngresoInicio", ingresoInicio);    
}

$("#culminarSesion").click(function () {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Ingresar.aspx/CerrarSesion',
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Ocurrió un error, contacte al administrador');
        },
        success: function (data) {
            if (data.d == true) {
                window.location = "../Cuentas/Login.aspx";
            }
            else {
                MensajeError('Error al cerrar sesión, intenta realizar esta acción en otra pantalla del sistema');
            }
        }
    });
});

function validarSesion() {
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Ingresar.aspx/validarSesion',
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Ocurrió un error, contacte al administrador');
        },
        success: function (data) {
            if (data.d == false) {
                window.location = "../Cuentas/Login.aspx";
            }
        }
    });
}

function guardarRespaldoInformacionPrestamo() {

    var RespaldoInformacionPrestamo = {
        clienteID: clienteID,
        tipoPrestamo: $("#tipoPrestamo").val(),
        tipoSolicitud: $("#tipoSolicitud").val(),
        primerNombreCliente: $("#primerNombreCliente").val(),
        SegundoNombreCliente: $("#SegundoNombreCliente").val(),
        primerApellidoCliente: $("#primerApellidoCliente").val(),
        segundoApellidoCliente: $("#segundoApellidoCliente").val(),
        identidadCliente: $("#identidadCliente").val(),
        pmoSugeridoSeleccionado: $("#pmoSugeridoSeleccionado :selected").val(),
        plazoPmoSeleccionado: $("#plazoPmoSeleccionado").val(),
        ingresosPrecalificado: $("#ingresosPrecalificado").val().replace(/,/g, ''),
        obligacionesPrecalificado: $("#obligacionesPrecalificado").val().replace(/,/g, ''),
        disponiblePrecalificado: $("#disponiblePrecalificado").val().replace(/,/g, ''),
        prima: $("#txtPrima").val().replace(/,/g, ''),
        valorVehiculo: $("#txtValorVehiculo").val().replace(/,/g, '')
    }
    localStorage.setItem('RespaldoInformacionPrestamo', JSON.stringify(RespaldoInformacionPrestamo));
}

function guardarRespaldoInformacionPersonal() {
    var respaldoInformacionPersonal = {
        clienteID: clienteID,
        numeroTelefono: $("#numeroTelefono").val(),
        nacionalidad: $("#nacionalidad").val(),
        fechaNacimiento: $("#fechaNacimiento").val(),
        correoElectronico: $("#correoElectronico").val(),
        profesion: $("#profesion").val(),
        sexo: $("input[name='sexo']:checked").val(),
        estadoCivil: $("input[name='estadoCivil']:checked").val(),
        vivivenda: $("#vivivenda").val(),
        tiempoResidir: $("input[name='tiempoResidir']:checked").val()
    }
    localStorage.setItem('RespaldoInformacionPersonal', JSON.stringify(respaldoInformacionPersonal));
}

function guardarRespaldoInformacionDomiciliar() {
    var respaldoInformacionDomiciliar = {
        departamento: $("#departamento").val(),
        municipio: $("#municipio").val(),
        ciudad: $("#ciudad").val(),
        barrioColonia: $("#barrioColonia").val(),
        telefonoCasa: $("#telefonoCasa").val(),
        telefonoMovil: $("#telefonoMovil").val(),
        direccionDetallada: $("#direccionDetallada").val(),
        referenciaDireccionDetallada: $("#referenciaDireccionDetallada").val()
    }
    localStorage.setItem('RespaldoInformacionDomiciliar', JSON.stringify(respaldoInformacionDomiciliar));
}

function guardarRespaldoInformacionLaboral() {
    var respaldoInformacionLaboral = {
        nombreDelTrabajo: $("#nombreDelTrabajo").val(),
        ingresosMensuales: $("#ingresosMensuales").val().replace(/,/g, ''),
        puestoAsignado: $("#puestoAsignado").val(),
        fechaIngreso: $("#fechaIngreso").val(),
        telefonoEmpresa: $("#telefonoEmpresa").val(),
        extensionRRHH: $("#extensionRRHH").val(),
        extensionCliente: $("#extensionCliente").val(),
        departamentoEmpresa: $("#departamentoEmpresa").val(),
        municipioEmpresa: $("#municipioEmpresa").val(),
        ciudadEmpresa: $("#ciudadEmpresa").val(),
        barrioColoniaEmpresa: $("#barrioColoniaEmpresa").val(),
        direccionDetalladaEmpresa: $("#direccionDetalladaEmpresa").val(),
        referenciaDireccionDetalladaEmpresa: $("#referenciaDireccionDetalladaEmpresa").val(),
        fuenteOtrosIngresos: $("#fuenteOtrosIngresos").val(),
        valorOtrosIngresos: $("#valorOtrosIngresos").val().replace(/,/g, '')
    }
    localStorage.setItem('RespaldoInformacionLaboral', JSON.stringify(respaldoInformacionLaboral));
}

function guardarRespaldoInformacionConyugal() {
    var respaldoInformacionConyugal = {
        nombresConyugue: $("#nombresConyugue").val(),
        apellidosConyugue: $("#apellidosConyugue").val(),
        identidadConyugue: $("#identidadConyugue").val(),
        fechaNacimientoConyugue: $("#fechaNacimientoConyugue").val(),
        telefonoConyugue: $("#telefonoConyugue").val(),
        lugarTrabajoConyugue: $("#lugarTrabajoConyugue").val(),
        ingresoMensualesConyugue: $("#ingresoMensualesConyugue").val().replace(/,/g, ''),
        telefonoTrabajoConyugue: $("#telefonoTrabajoConyugue").val()
    }
    localStorage.setItem('RespaldoInformacionConyugal', JSON.stringify(respaldoInformacionConyugal));
}

function guardarRespaldoReferenciasPersonales() {
    localStorage.setItem('RespaldoReferenciasPersonales', JSON.stringify(listaClientesReferencias));
}

function recuperarRespaldos() {

    $("#spinnerCargando").css('display', '');
    estadoFuncionRecuperarRespaldos = false;
    if (localStorage.getItem('RespaldoInformacionPrestamo') != null) {

        var RespaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));
        clienteID = RespaldoInformacionPrestamo.clienteID;
        $("#tipoPrestamo").val(RespaldoInformacionPrestamo.tipoPrestamo);
        $("#tipoSolicitud").val(RespaldoInformacionPrestamo.tipoSolicitud);
        $("#primerNombreCliente").val(RespaldoInformacionPrestamo.primerNombreCliente);
        $("#SegundoNombreCliente").val(RespaldoInformacionPrestamo.SegundoNombreCliente);
        $("#primerApellidoCliente").val(RespaldoInformacionPrestamo.primerApellidoCliente);
        $("#segundoApellidoCliente").val(RespaldoInformacionPrestamo.segundoApellidoCliente);
        $("#identidadCliente").val(RespaldoInformacionPrestamo.identidadCliente);
        $("#pmoSugeridoSeleccionado").val(RespaldoInformacionPrestamo.pmoSugeridoSeleccionado);
        $("#plazoPmoSeleccionado").val(RespaldoInformacionPrestamo.plazoPmoSeleccionado);
        $("#ingresosPrecalificado").val(RespaldoInformacionPrestamo.ingresosPrecalificado);
        $("#obligacionesPrecalificado").val(RespaldoInformacionPrestamo.obligacionesPrecalificado);
        $("#disponiblePrecalificado").val(RespaldoInformacionPrestamo.disponiblePrecalificado);
        $("#txtPrima").val(RespaldoInformacionPrestamo.prima);
        $("#txtValorVehiculo").val(RespaldoInformacionPrestamo.valorVehiculo);
        $("#txtValorFinanciar").val(RespaldoInformacionPrestamo.valorVehiculo - RespaldoInformacionPrestamo.prima);
        if ($("#tipoPrestamo option:selected").text() == 'PRESTAMO EFECTIVO') {
            $("#lblPlazoPMO").text('Plazo (quincenal)');
            $(".divPrestamoVehiculo").css('display', 'none');
            $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar,#ddlOrigen").prop('disabled', true);
            $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar").val('');
            $("#ddlOrigen,#titleOrigen").css('display', 'none');
        }
        else if ($("#tipoPrestamo option:selected").val() != '') {

            $("#ddlOrigen").prop('disabled', false);
            $("#ddlOrigen,#titleOrigen").css('display', '');
            switch ($("#tipoPrestamo option:selected").text()) {

                case "PRESTAMO EFECTIVO":
                    codigoProducto = '101';
                    break;
                case "PRESTAMO VEHICULO MOTO":
                    codigoProducto = '201';
                    break;
                case "PRESTAMO VEHICULO AUTO":
                    codigoProducto = '202';
                    break;
                case "PRESTAMO PRODUCTO":
                    codigoProducto = '301';
                    break;

            }
            cargarOrigenes(codigoProducto);

            $("#lblPlazoPMO").text('Plazo (mensual)');
            $(".divPrestamoVehiculo").css('display', '');
            $("#txtPrima,#txtValorVehiculo").prop('disabled', false);
            var prima = $('#txtPrima').val().replace(/,/g, '');
            var valorVehiculo = $('#txtValorVehiculo').val().replace(/,/g, '');
            var totalAFinanciar = valorVehiculo - prima;
            $('#txtValorFinanciar').val(totalAFinanciar);
            if (totalAFinanciar > parseFloat($("#pmoSugeridoSeleccionado :selected").val()).toFixed(2) || valorVehiculo <= 0) {
                $('#txtValorFinanciar').addClass('parsley-error');
                $('#txtValorFinanciar').addClass('text-danger');
                $('#error-valorFinanciar').css('display', '');
            } else {
                $('#txtValorFinanciar').removeClass('parsley-error');
                $('#txtValorFinanciar').removeClass('text-danger');
                $('#error-valorFinanciar').css('display', 'none');
            }
        }
        if (clienteID != 0) {
            $("#primerNombreCliente").prop('disabled', true);
            $("#SegundoNombreCliente").prop('disabled', true);
            $("#primerApellidoCliente").prop('disabled', true);
            $("#segundoApellidoCliente").prop('disabled', true);
            $("#identidadCliente").prop('disabled', true);
        }
    }
    if (localStorage.getItem('RespaldoInformacionPersonal') != null) {
        var respaldoInformacionPersonal = JSON.parse(localStorage.getItem('RespaldoInformacionPersonal'));

        clienteID = respaldoInformacionPersonal.clienteID;
        $("#numeroTelefono").val(respaldoInformacionPersonal.numeroTelefono);
        $("#nacionalidad").val(respaldoInformacionPersonal.nacionalidad);
        $("#fechaNacimiento").val(respaldoInformacionPersonal.fechaNacimiento);
        $("#correoElectronico").val(respaldoInformacionPersonal.correoElectronico);
        $("#profesion").val(respaldoInformacionPersonal.profesion);
        $("input[name=sexo][value=" + respaldoInformacionPersonal.sexo + "]").prop('checked', true);
        $("input[name=estadoCivil][value=" + respaldoInformacionPersonal.estadoCivil + "]").prop('checked', true);
        $("#vivivenda").val(respaldoInformacionPersonal.vivivenda);
        $("input[name=tiempoResidir][value=" + respaldoInformacionPersonal.tiempoResidir + "]").prop('checked', true);

        if (clienteID != 0) {
            $("#numeroTelefono").prop('disabled', true);
            $("#nacionalidad").prop('disabled', true);
            $("#fechaNacimiento").prop('disabled', true);
            $("#correoElectronico").prop('disabled', true);
            $("#profesion").prop('disabled', true);
            $("input[name=sexo]").prop('disabled', true);
            $("input[name=estadoCivil]").prop('disabled', true);
            $("#vivivenda").prop('disabled', true);
            $("input[name=tiempoResidir]").prop('disabled', true);
        }
    }
    if (localStorage.getItem('RespaldoInformacionDomiciliar') != null) {
        var respaldoInformacionDomiciliar = JSON.parse(localStorage.getItem('RespaldoInformacionDomiciliar'));
        $("#departamento").val(respaldoInformacionDomiciliar.departamento);
        $("#municipio").val(respaldoInformacionDomiciliar.municipio);
        $("#ciudad").val(respaldoInformacionDomiciliar.ciudad);
        $("#barrioColonia").val(respaldoInformacionDomiciliar.barrioColonia);
        $("#telefonoCasa").val(respaldoInformacionDomiciliar.telefonoCasa);
        $("#telefonoMovil").val(respaldoInformacionDomiciliar.telefonoMovil);
        $("#direccionDetallada").val(respaldoInformacionDomiciliar.direccionDetallada);
        $("#referenciaDireccionDetallada").val(respaldoInformacionDomiciliar.referenciaDireccionDetallada);
        //validar si es un cliente existente
        if (clienteID != 0) {
            $("#departamento").prop('disabled', true);
            $("#municipio").prop('disabled', true);
            $("#ciudad").prop('disabled', true);
            $("#barrioColonia").prop('disabled', true);
            $("#telefonoCasa").prop('disabled', true);
            $("#telefonoMovil").prop('disabled', true);
            $("#direccionDetallada").prop('disabled', true);
            $("#referenciaDireccionDetallada").prop('disabled', true);
            $("#btnAgregarColonia").prop('disabled', true);
        }
    }
    if (localStorage.getItem('RespaldoInformacionLaboral') != null) {
        var respaldoInformacionLaboral = JSON.parse(localStorage.getItem('RespaldoInformacionLaboral'));
        $("#nombreDelTrabajo").val(respaldoInformacionLaboral.nombreDelTrabajo);
        $("#ingresosMensuales").val(respaldoInformacionLaboral.ingresosMensuales);
        $("#puestoAsignado").val(respaldoInformacionLaboral.puestoAsignado);
        $("#fechaIngreso").val(respaldoInformacionLaboral.fechaIngreso);
        $("#telefonoEmpresa").val(respaldoInformacionLaboral.telefonoEmpresa);
        $("#extensionRRHH").val(respaldoInformacionLaboral.extensionRRHH);
        $("#extensionCliente").val(respaldoInformacionLaboral.extensionCliente);
        $("#departamentoEmpresa").val(respaldoInformacionLaboral.departamentoEmpresa);
        $("#municipioEmpresa").val(respaldoInformacionLaboral.municipioEmpresa);
        $("#ciudadEmpresa").val(respaldoInformacionLaboral.ciudadEmpresa);
        $("#barrioColoniaEmpresa").val(respaldoInformacionLaboral.barrioColoniaEmpresa);
        $("#direccionDetalladaEmpresa").val(respaldoInformacionLaboral.direccionDetalladaEmpresa);
        $("#referenciaDireccionDetalladaEmpresa").val(respaldoInformacionLaboral.referenciaDireccionDetalladaEmpresa);
        $("#fuenteOtrosIngresos").val(respaldoInformacionLaboral.fuenteOtrosIngresos);
        $("#valorOtrosIngresos").val(respaldoInformacionLaboral.valorOtrosIngresos);

        if (clienteID != 0) {
            $("#nombreDelTrabajo").prop('disabled', true);
            $("#ingresosMensuales").prop('disabled', true);
            $("#puestoAsignado").prop('disabled', true);
            $("#fechaIngreso").prop('disabled', true);
            $("#telefonoEmpresa").prop('disabled', true);
            $("#extensionRRHH").prop('disabled', true);
            $("#extensionCliente").prop('disabled', true);
            $("#departamentoEmpresa").prop('disabled', true);
            $("#municipioEmpresa").prop('disabled', true);
            $("#ciudadEmpresa").prop('disabled', true);
            $("#barrioColoniaEmpresa").prop('disabled', true);
            $("#direccionDetalladaEmpresa").prop('disabled', true);
            $("#referenciaDireccionDetalladaEmpresa").prop('disabled', true);
            $("#fuenteOtrosIngresos").prop('disabled', true);
            $("#valorOtrosIngresos").prop('disabled', true);
            $("#btnAgregarColoniaEmpresa").prop('disabled', true);
        }
    }
    if (localStorage.getItem('RespaldoInformacionConyugal') != null) {
        var respaldoInformacionConyugal = JSON.parse(localStorage.getItem('RespaldoInformacionConyugal'));
        $("#nombresConyugue").val(respaldoInformacionConyugal.nombresConyugue);
        $("#apellidosConyugue").val(respaldoInformacionConyugal.apellidosConyugue);
        $("#identidadConyugue").val(respaldoInformacionConyugal.identidadConyugue);
        $("#fechaNacimientoConyugue").val(respaldoInformacionConyugal.fechaNacimientoConyugue);
        $("#telefonoConyugue").val(respaldoInformacionConyugal.telefonoConyugue);
        $("#lugarTrabajoConyugue").val(respaldoInformacionConyugal.lugarTrabajoConyugue);
        $("#ingresoMensualesConyugue").val(respaldoInformacionConyugal.ingresoMensualesConyugue);
        $("#telefonoTrabajoConyugue").val(respaldoInformacionConyugal.telefonoTrabajoConyugue);

        if (clienteID != 0) {
            $("#nombresConyugue").prop('disabled', true);
            $("#apellidosConyugue").prop('disabled', true);
            $("#identidadConyugue").prop('disabled', true);
            $("#fechaNacimientoConyugue").prop('disabled', true);
            $("#telefonoConyugue").prop('disabled', true);
            $("#lugarTrabajoConyugue").prop('disabled', true);
            $("#ingresoMensualesConyugue").prop('disabled', true);
            $("#telefonoTrabajoConyugue").prop('disabled', true);
        }
    }
    else if ($("input[name=estadoCivil]:checked").data('info') == false) {
        $("#nombresConyugue").prop('disabled', true);
        $("#apellidosConyugue").prop('disabled', true);
        $("#identidadConyugue").prop('disabled', true);
        $("#fechaNacimientoConyugue").prop('disabled', true);
        $("#telefonoConyugue").prop('disabled', true);
        $("#lugarTrabajoConyugue").prop('disabled', true);
        $("#ingresoMensualesConyugue").prop('disabled', true);
        $("#telefonoTrabajoConyugue").prop('disabled', true);
    }

    if (localStorage.getItem('RespaldoReferenciasPersonales') != null) {
        RespaldolistaClientesReferencias = JSON.parse(localStorage.getItem('RespaldoReferenciasPersonales'));

        $('#datatable-buttons').DataTable().clear();
        listaClientesReferencias = [];
        if (RespaldolistaClientesReferencias.length > 0) {
            for (var i = 0; i < RespaldolistaClientesReferencias.length; i++) {
                $('#datatable-buttons').dataTable().fnAddData([
                    RespaldolistaClientesReferencias[i].fcNombreCompletoReferencia,
                    RespaldolistaClientesReferencias[i].fcLugarTrabajoReferencia,
                    RespaldolistaClientesReferencias[i].fiTiempoConocerReferencia <= 2 ? RespaldolistaClientesReferencias[i].fiTiempoConocerReferencia + ' años' : 'Más de 2 años',
                    RespaldolistaClientesReferencias[i].fcTelefonoReferencia,
                    RespaldolistaClientesReferencias[i].fcDescripcionParentesco,
                ]);
                cantidadReferencias += 1;
                RespaldolistaClientesReferencias[i].fdFechaCrea = '';
                RespaldolistaClientesReferencias[i].fdFechaUltimaModifica = '';
                listaClientesReferencias.push(RespaldolistaClientesReferencias[i]);
            }
            $("#btnNuevaReferencia").prop('disabled', true);
        }
    }
    estadoFuncionRecuperarRespaldos = true
    if (estadoFuncionRecuperarInfoCliente == true && estadoFuncionLlenarDDL == true) {
        $("#spinnerCargando").css('display', 'none');
    }
}

//cargar informacion del precalificado
var listadoCotizaciones = [];
function recuperarInformacionPrecalificado() {
    if (localStorage.getItem('precalificado') != null && localStorage.getItem('precalificado') != 'null') {
        $("#obligacionesPrecalificado,#btnBuscarCliente,#identidadCliente,#primerNombreCliente,#SegundoNombreCliente,#primerApellidoCliente,#segundoApellidoCliente,#ingresosPrecalificado,#disponiblePrecalificado,#ingresosMensuales,#numeroTelefono,#telefonoMovil,#fechaNacimiento").prop('disabled', true);

        $("#identidadCliente").val(objPrecalificado.identidad);
        $("#primerNombreCliente").val(objPrecalificado.primerNombre);
        $("#SegundoNombreCliente").val(objPrecalificado.segundoNombre);
        $("#primerApellidoCliente").val(objPrecalificado.primerApellido);
        $("#segundoApellidoCliente").val(objPrecalificado.segundoApellido);
        $("#ingresosPrecalificado").val(objPrecalificado.ingresos);
        var obligaciones = 0;
        if (objPrecalificado.obligaciones == "" && objPrecalificado.obligaciones == null && objPrecalificado.obligaciones == '' && objPrecalificado.obligaciones == undefined) {
            obligaciones = objPrecalificado.obligaciones;
        }
        $("#obligacionesPrecalificado").val(obligaciones);
        $("#disponiblePrecalificado").val(objPrecalificado.disponible);
        $("#ingresosMensuales").val(objPrecalificado.ingresos);
        $("#numeroTelefono").val(objPrecalificado.telefono);
        $("#telefonoMovil").val(objPrecalificado.telefono);
        $("#fechaNacimiento").val(dateFormat(objPrecalificado.fechaNacimiento));
        var FechaNac = new Date(parseInt(objPrecalificado.fechaNacimiento.replace("/Date(", "").replace(")/", ""), 10));        
        var today = new Date();
        var edad = Math.floor((today - FechaNac) / (365.25 * 24 * 60 * 60 * 1000));
        $('#edadCliente').val(edad + ' años');
        //llenar lista de montos sugeridos
        listadoCotizaciones = objPrecalificado.cotizadorProductos;
        var DDLMontosSugeridos = $("#pmoSugeridoSeleccionado");
        DDLMontosSugeridos.empty();
        DDLMontosSugeridos.append("<option selected value='" + listadoCotizaciones[0].fnMontoOfertado + "'>" + listadoCotizaciones[0].fnMontoOfertado + "</option>");
        $("#plazoPmoSeleccionado").val(listadoCotizaciones[0].fiPlazo);
        $("#cutoaQuinceal").val(listadoCotizaciones[0].fnCuotaQuincenal);
        for (var i = 1; i < listadoCotizaciones; i++) {
            DDLMontosSugeridos.append("<option value='" + listadoCotizaciones[i].fnMontoOfertado + "'>" + listadoCotizaciones[i].fnMontoOfertado + "</option>");
        }
    }
}

$('#txtValorVehiculo').blur(function () {
    var mensajeError = '';
    var DescripcionPrestamo = $("#tipoPrestamo option:selected").text();
    var prestamoSeleccionado = parseFloat($("#pmoSugeridoSeleccionado :selected").val()).toFixed(2);
    var valorVehiculo = $('#txtValorVehiculo').val().replace(/,/g, '');
    var prima = $('#txtPrima').val().replace(/,/g, '');

    /* CALCULAR PRIMA MINIMA SI EL TIPO DE PRESTAMO ES MOTO, VALIDAR QUE LA PRIMA MINIMA SEA EL 10% DEL VALOR DEL VEHICULO */
    if (DescripcionPrestamo == 'PRESTAMO VEHICULO MOTO') {
        var primaMinima = (valorVehiculo * 10) / 100;
        prima = primaMinima;
        $('#txtPrima').val(prima);
    }
    /* CALCULAR EL TOTAL A FINANCIAR */
    var totalAFinanciar = valorVehiculo - prima;
    $('#txtValorFinanciar').val(totalAFinanciar);

    var mensajeErrorFinanciar = '';
    /* VALIDAR EL VALOR MINIMO A FINANCIAR*/
    if (totalAFinanciar < 6000.00 && (DescripcionPrestamo == 'PRESTAMO VEHICULO MOTO' || DescripcionPrestamo == 'PRESTAMO EFECTIVO')) {
        mensajeErrorFinanciar = 'El valor mínimo a financiar es 6,000.00';
    }
    /* SI EL TOTAL A FINANCIAR ES MAYOR QUE EL PMO SUGERIDO, MOSTRAR MENSAJE DE VALIDACION */
    if (totalAFinanciar > prestamoSeleccionado) {
        mensajeErrorFinanciar = 'Total a financiar mayor que PMO sugerido, aumente el valor de la prima.';        
    }
    if (mensajeErrorFinanciar != '') {
        $('#txtValorFinanciar').addClass('parsley-error');
        $('#txtValorFinanciar').addClass('text-danger');
        MensajeError(mensajeErrorFinanciar);

    } else {
        $('#txtValorFinanciar').removeClass('parsley-error');
        $('#txtValorFinanciar').removeClass('text-danger');
    }
});

// calcular monto a financiar y validar que no sea mayor que el prestamo sugerido seleccionado
$('#txtPrima').blur(function () {
    /* OBTENER VARIABLES */
    var mensajeError = '';
    var DescripcionPrestamo = $("#tipoPrestamo option:selected").text();

    var prestamoSeleccionado = parseFloat($("#pmoSugeridoSeleccionado :selected").val()).toFixed(2);
    if (isNaN(prestamoSeleccionado)) {
        MensajeError('Error al obtener el valor del préstamo sugerido o se insertó un tipo de dato incorrecto');
    }
    var valorVehiculo = $('#txtValorVehiculo').val().replace(/,/g, '');
    if (isNaN(valorVehiculo)) {
        MensajeError('Error al obtener el valor del vehiculo o se insertó un tipo de dato incorrecto');
    }
    var prima = $('#txtPrima').val().replace(/,/g, '');
    if (isNaN(prima)) {
        MensajeError('Error al obtener el valor de la prima o se insertó un tipo de dato incorrecto');
    }

    /* VALIDAR QUE LA PRIMA NO SEA MAYOR QUE EL VALOR DEL VEHICULO */
    if (parseFloat(prima) >= parseFloat(valorVehiculo)) {
        mensajeError = 'Prima mayor o igual que el valor del vehiculo.';
        $('#txtPrima').addClass('parsley-error');
        $('#error-prima').addClass('text-danger');
        MensajeError(mensajeError);

    } else {
        $('#txtPrima').removeClass('parsley-error');
        $('#error-prima').removeClass('text-danger');
        $('#error-prima').css('display', 'none');
    }
    var totalAFinanciar = valorVehiculo - prima;
    $('#txtValorFinanciar').val(totalAFinanciar);

    var mensajeErrorFinanciar = '';
    /*VALIDAR MONTO MINIMO A FINANCIAR */
    if (totalAFinanciar < 6000.00 && (DescripcionPrestamo == 'PRESTAMO VEHICULO MOTO' || DescripcionPrestamo == 'PRESTAMO EFECTIVO')) {
        mensajeErrorFinanciar = 'El valor mínimo a financiar es 6,000.00';
    }
    /* SI EL TOTAL A FINANCIAR ES MAYOR QUE EL PMO SUGERIDO, MOSTRAR MENSAJE DE VALIDACION */
    if (totalAFinanciar > prestamoSeleccionado) {
        mensajeErrorFinanciar = 'Total a financiar mayor que PMO sugerido, aumente el valor de la prima.';
    } 
    if (mensajeErrorFinanciar != '') {
        $('#txtValorFinanciar').addClass('parsley-error');
        $('#txtValorFinanciar').addClass('text-danger');
        MensajeError(mensajeErrorFinanciar);

    } else {
        $('#txtValorFinanciar').removeClass('parsley-error');
        $('#txtValorFinanciar').removeClass('text-danger');
    }
});

//funcion para llenar el formulario con la informacion de un cliente existente
function cargarInformacionCompletaDelCliente(informacionCliente) {

    estadoFuncionRecuperarInfoCliente = false;
    $("#spinnerCargando").css('display', '');
    rowData = informacionCliente;
    clienteNuevo = false;
    clienteID = rowData.clientesMaster.fiIDCliente;
    $("#primerNombreCliente").val(rowData.clientesMaster.fcPrimerNombreCliente);
    $("#primerNombreCliente").attr('readonly', true);
    $("#SegundoNombreCliente").val(rowData.clientesMaster.fcSegundoNombreCliente);
    $("#SegundoNombreCliente").attr('readonly', true);
    $("#primerApellidoCliente").val(rowData.clientesMaster.fcPrimerApellidoCliente);
    $("#primerApellidoCliente").attr('readonly', true);
    $("#segundoApellidoCliente").val(rowData.clientesMaster.fcSegundoApellidoCliente);
    $("#segundoApellidoCliente").attr('readonly', true);
    $("#identidadCliente").val(rowData.clientesMaster.fcIdentidadCliente);
    $("#identidadCliente").attr('readonly', true);
    $("#numeroTelefono").val(rowData.clientesMaster.fcTelefonoCliente);
    $("#numeroTelefono").attr('readonly', true);
    $(".buscardorddl").select2("destroy");
    $("#nacionalidad").val(rowData.clientesMaster.fiNacionalidadCliente);
    $("#nacionalidad").attr('disabled', true);
    $("#fechaNacimiento").val(dateFormat(rowData.clientesMaster.fdFechaNacimientoCliente));
    $("#fechaNacimiento").attr('readonly', true);    
    var FechaNac = new Date(parseInt(objPrecalificado.fechaNacimiento.replace("/Date(", "").replace(")/", ""), 10));
    var today = new Date();
    var edad = Math.floor((today - FechaNac) / (365.25 * 24 * 60 * 60 * 1000));
    $('#edadCliente').val(edad + ' años');
    $("#edadCliente").attr('readonly', true);
    $("#correoElectronico").val(rowData.clientesMaster.fcCorreoElectronicoCliente);
    $("#correoElectronico").attr('readonly', true);
    $("#profesion").val(rowData.clientesMaster.fcProfesionOficioCliente);
    $("#profesion").attr('readonly', true);
    $("input[name=sexo][value=" + rowData.clientesMaster.fcSexoCliente + "]").prop('checked', true);
    $("input[name=sexo]").attr('disabled', true);
    $("input[name=estadoCivil][value=" + rowData.clientesMaster.fiIDEstadoCivil + "]").prop('checked', true);
    $("input[name=estadoCivil]").attr('disabled', true);
    $("#vivivenda").val(rowData.clientesMaster.fiIDVivienda);
    $("#vivivenda").attr('disabled', true);    
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
    $("input[name=tiempoResidir][value=" + rowData.clientesMaster.fiTiempoResidir + "]").prop('checked', true);
    $("input[name=tiempoResidir]").attr('disabled', true);
    // INFORMACION DOMICILIAR
    $("#departamento").empty();
    $("#departamento").append("<option selected value='" + rowData.ClientesInformacionDomiciliar.fiIDDepto + "'>" + rowData.ClientesInformacionDomiciliar.fcNombreDepto + "</option>");
    $("#departamento").attr('readonly', true);
    $("#municipio").empty();
    $("#municipio").append("<option selected value='" + rowData.ClientesInformacionDomiciliar.fiIDMunicipio + "'>" + rowData.ClientesInformacionDomiciliar.fcNombreMunicipio + "</option>");
    $("#municipio").attr('readonly', true);
    $("#ciudad").empty();
    $("#ciudad").append("<option selected value='" + rowData.ClientesInformacionDomiciliar.fiIDCiudad + "'>" + rowData.ClientesInformacionDomiciliar.fcNombreCiudad + "</option>");
    $("#ciudad").attr('readonly', true);
    $("#barrioColonia").empty();
    $("#barrioColonia").append("<option selected value='" + rowData.ClientesInformacionDomiciliar.fiIDBarrioColonia + "'>" + rowData.ClientesInformacionDomiciliar.fcNombreBarrioColonia + "</option>");
    $("#barrioColonia").attr('readonly', true);
    $("#telefonoCasa").val(rowData.ClientesInformacionDomiciliar.fcTelefonoCasa);
    $("#telefonoCasa").attr('readonly', true);
    $("#telefonoMovil").val(rowData.clientesMaster.fcTelefonoCliente);
    $("#telefonoMovil").attr('readonly', true);
    $("#direccionDetallada").val(rowData.ClientesInformacionDomiciliar.fcDireccionDetallada);
    $("#direccionDetallada").attr('readonly', true);
    $("#referenciaDireccionDetallada").val(rowData.ClientesInformacionDomiciliar.fcReferenciasDireccionDetallada);
    $("#referenciaDireccionDetallada").attr('readonly', true);
    // INFORMACION LABORAL
    $("#nombreDelTrabajo").val(rowData.ClientesInformacionLaboral.fcNombreTrabajo);
    $("#nombreDelTrabajo").attr('readonly', true);
    $("#ingresosMensuales").val(rowData.ClientesInformacionLaboral.fiIngresosMensuales);
    $("#ingresosMensuales").attr('readonly', true);
    $("#puestoAsignado").val(rowData.ClientesInformacionLaboral.fcPuestoAsignado);
    $("#puestoAsignado").attr('readonly', true);
    $("#fechaIngreso").val(dateFormat(rowData.ClientesInformacionLaboral.fcFechaIngreso));
    $("#fechaIngreso").attr('readonly', true);
    $("#telefonoEmpresa").val(rowData.ClientesInformacionLaboral.fdTelefonoEmpresa);
    $("#telefonoEmpresa").attr('readonly', true);
    $("#extensionRRHH").val(rowData.ClientesInformacionLaboral.fcExtensionRecursosHumanos);
    $("#extensionRRHH").attr('readonly', true);
    $("#extensionCliente").val(rowData.ClientesInformacionLaboral.fcExtensionCliente);
    $("#extensionCliente").attr('readonly', true);
    $("#ingresosMensuales").val(rowData.ClientesInformacionLaboral.fiIngresosMensuales);
    $("#ingresosMensuales").attr('readonly', true);
    $("#puestoAsignado").val(rowData.ClientesInformacionLaboral.fcPuestoAsignado);
    $("#puestoAsignado").attr('readonly', true);
    $("#departamentoEmpresa").empty();
    $("#departamentoEmpresa").append("<option selected value='" + rowData.ClientesInformacionLaboral.fiIDDepto + "'>" + rowData.ClientesInformacionLaboral.fcNombreDepto + "</option>");
    $("#departamentoEmpresa").attr('readonly', true);
    $("#municipioEmpresa").empty();
    $("#municipioEmpresa").append("<option selected value='" + rowData.ClientesInformacionLaboral.fiIDMunicipio + "'>" + rowData.ClientesInformacionLaboral.fcNombreMunicipio + "</option>");
    $("#municipioEmpresa").attr('readonly', true);
    $("#ciudadEmpresa").empty();
    $("#ciudadEmpresa").append("<option selected value='" + rowData.ClientesInformacionLaboral.fiIDCiudad + "'>" + rowData.ClientesInformacionLaboral.fcNombreCiudad + "</option>");
    $("#ciudadEmpresa").attr('readonly', true);
    $("#barrioColoniaEmpresa").empty();
    $("#barrioColoniaEmpresa").append("<option selected value='" + rowData.ClientesInformacionLaboral.fiIDBarrioColonia + "'>" + rowData.ClientesInformacionLaboral.fcNombreBarrioColonia + "</option>");
    $("#barrioColoniaEmpresa").attr('readonly', true);
    $("#direccionDetalladaEmpresa").val(rowData.ClientesInformacionLaboral.fcDireccionDetalladaEmpresa);
    $("#direccionDetalladaEmpresa").attr('readonly', true);
    $("#referenciaDireccionDetalladaEmpresa").val(rowData.ClientesInformacionLaboral.fcReferenciasDireccionDetallada);
    $("#referenciaDireccionDetalladaEmpresa").attr('readonly', true);
    $("#fuenteOtrosIngresos").val(rowData.ClientesInformacionLaboral.fcFuenteOtrosIngresos);
    $("#fuenteOtrosIngresos").attr('readonly', true);
    $("#valorOtrosIngresos").val(rowData.ClientesInformacionLaboral.fiValorOtrosIngresosMensuales);
    $("#valorOtrosIngresos").attr('readonly', true);
    //INFORMACION CONYUGAL
    if (rowData.ClientesInformacionConyugal != null) {
        $("#nombresConyugue").val('');
        if (rowData.ClientesInformacionConyugal.fcNombreCompletoConyugue != null) {
            $("#nombresConyugue").val(rowData.ClientesInformacionConyugal.fcNombreCompletoConyugue.toString().split(' ').slice(0, -1).join(' '));
        }
        $("#nombresConyugue").attr('readonly', true);
        $("#apellidosConyugue").val('');
        if (rowData.ClientesInformacionConyugal.fcNombreCompletoConyugue != null) {
            $("#apellidosConyugue").val(rowData.ClientesInformacionConyugal.fcNombreCompletoConyugue.toString().split(' ').slice(-1).join(' '));
        }
        $("#apellidosConyugue").attr('readonly', true);
        $("#identidadConyugue").val(rowData.ClientesInformacionConyugal.fcIndentidadConyugue);
        $("#identidadConyugue").attr('readonly', true);
        $("#fechaNacimientoConyugue").val(dateFormat(rowData.ClientesInformacionConyugal.fdFechaNacimientoConyugue));
        $("#fechaNacimientoConyugue").attr('readonly', true);
        $("#telefonoConyugue").val(rowData.ClientesInformacionConyugal.fcTelefonoConyugue);
        $("#telefonoConyugue").attr('readonly', true);
        $("#lugarTrabajoConyugue").val(rowData.ClientesInformacionConyugal.fcLugarTrabajoConyugue);
        $("#lugarTrabajoConyugue").attr('readonly', true);
        $("#ingresoMensualesConyugue").val(rowData.ClientesInformacionConyugal.fcIngresosMensualesConyugue);
        $("#ingresoMensualesConyugue").attr('readonly', true);
        $("#telefonoTrabajoConyugue").val(FechaFormato(rowData.ClientesInformacionConyugal.fcTelefonoTrabajoConyugue));
        $("#telefonoTrabajoConyugue").attr('readonly', true);
    }
    $('#datatable-buttons').DataTable().clear();
    listaClientesReferencias = [];
    if (rowData.ClientesReferenciasPersonales.length > 0) {
        for (var i = 0; i < rowData.ClientesReferenciasPersonales.length; i++) {
            $('#datatable-buttons').dataTable().fnAddData([
                rowData.ClientesReferenciasPersonales[i].fcNombreCompletoReferencia,
                rowData.ClientesReferenciasPersonales[i].fcLugarTrabajoReferencia,
                rowData.ClientesReferenciasPersonales[i].fiTiempoConocerReferencia <= 2 ? rowData.ClientesReferenciasPersonales[i].fiTiempoConocerReferencia + ' años' : 'Más de 2 años',
                rowData.ClientesReferenciasPersonales[i].fcTelefonoReferencia,
                rowData.ClientesReferenciasPersonales[i].fcDescripcionParentesco,
            ]);
            cantidadReferencias += 1;
            rowData.ClientesReferenciasPersonales[i].fdFechaCrea = '';
            rowData.ClientesReferenciasPersonales[i].fdFechaUltimaModifica = '';
            listaClientesReferencias.push(rowData.ClientesReferenciasPersonales[i]);
        }
    }
    estadoFuncionRecuperarInfoCliente = true;
    if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
        $("#spinnerCargando").css('display', 'none');
    }
    $("#btnNuevaReferencia").prop('disabled', true);
    guardarRespaldoInformacionPrestamo();
    guardarRespaldoInformacionPersonal();
    guardarRespaldoInformacionDomiciliar();
    guardarRespaldoInformacionLaboral();
    guardarRespaldoInformacionConyugal();
    guardarRespaldoReferenciasPersonales();
    $("#spinnerCargando").css('display', 'none');
    MensajeInformacion('La información de este cliente se cargó correctamente');
}

$("#AbrirmodalReiniciarSolicitud").click(function () {
    $("#modalReiniciarSolicitud").modal();
});
$("#btnReiniciarSolicitud").click(function () {
    resetForm($("#frmSolicitud"));
    $('#frmSolicitud').parsley().reset();
    $('#datatable-buttons').DataTable().clear().draw();
    $("#frmSolicitud :input").prop('disabled', false);
    $("#frmSolicitud :input").prop('readonly', false);
    // llenar select de tipo de solicitud
    $("#tipoSolicitud").empty();
    $("#tipoSolicitud").append("<option value=''>Seleccione una opción</option>");
    $("#tipoSolicitud").append("<option value='NUEVO' selected>NUEVO</option>");
    clienteID = 0;
    cantidadReferencias = 0;
    listaClientesReferencias = [];
    localStorage.clear();
    localStorage.setItem('precalificado', null);
    localStorage.clear();
    window.location = 'SolicitudesCredito_Precalificar.aspx';
});