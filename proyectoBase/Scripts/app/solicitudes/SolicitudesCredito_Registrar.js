var btnFinish = $('<button type="button" id="btnGuardarSolicitud"></button>').text('Finalizar')
    .addClass('btn btn-info')
    .css('display', 'none')
    .on('click', function () {
        $('#frmSolicitud').submit(function (e) { e.preventDefault() });

        $('#frmSolicitud').parsley().validate({ group: 'informacionPrestamo', force: true });
        var modelStateInformacionPrestamo = $('#frmSolicitud').parsley().isValid({ group: 'informacionPrestamo' });

        $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
        var modelStateInformacionPersonal = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' });

        $('#frmSolicitud').parsley().validate({ group: 'informacionDomiciliar', force: true });
        var modelStateInformacionDomicilio = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomiciliar' });

        $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
        var modelStateInformacionLaboral = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

        var modelStateInformacionConyugal = true;

        if ($("input[name='estadoCivil']:checked").data('info') == true) {
            $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
            modelStateInformacionConyugal = $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });
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
                fiIDTipoPrestamo: $("#tipoPrestamo").data('cod'),
                fcTipoSolicitud: $("#tipoSolicitud").val(),
                fnPrima: $("#txtPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtPrima").val().replace(/,/g, ''),
                fnValorGarantia: $("#txtValorVehiculo").val().replace(/,/g, '') == '' ? 0 : $("#txtValorVehiculo").val().replace(/,/g, ''),
                fcTipoSolicitud: $("#tipoSolicitud").val(),
                fdValorPmoSugeridoSeleccionado: $("#pmosSugeridos option:selected").val(),
                fiPlazoPmoSeleccionado: $("#pmosSugeridos option:selected").data('pmoplz'),
                fiIDOrigen: $("#ddlOrigen option:selected").val() == null ? 1 : parseInt($("#ddlOrigen option:selected").val())
            };
            var bitacora = {
                fdEnIngresoInicio: localStorage.getItem("EnIngresoInicio"),
                fdEnIngresoFin: ''
            };
            var ClienteMaster = {
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
            };
            var ClientesInformacionLaboral = {
                fcNombreTrabajo: $("#nombreDelTrabajo").val(),
                fiIngresosMensuales: $("#ingresosMensuales").val().replace(/,/g, ''),
                fcPuestoAsignado: $("#puestoAsignado").val(),
                fcFechaIngreso: $("#fechaIngreso").val(),
                fdTelefonoEmpresa: $("#telefonoEmpresa").val(),
                fcExtensionRecursosHumanos: $("#extensionRRHH").val().replace(/_/g, ''),
                fcExtensionCliente: $("#extensionCliente").val().replace(/_/g, ''),
                fiCiudadEmpresa: $("#ciudadEmpresa").val(),
                fiIDBarrioColonia: $("#barrioColoniaEmpresa :selected").val(),
                fiIDDepto: $("#departamentoEmpresa :selected").val(),
                fiIDMunicipio: $("#municipioEmpresa :selected").val(),
                fiIDCiudad: $("#ciudadEmpresa :selected").val(),
                fcDireccionDetalladaEmpresa: $("#direccionDetalladaEmpresa").val(),
                fcReferenciasDireccionDetallada: $("#referenciaDireccionDetalladaEmpresa").val(),
                fcFuenteOtrosIngresos: $("#fuenteOtrosIngresos").val(),
                fiValorOtrosIngresosMensuales: $("#valorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#valorOtrosIngresos").val().replace(/,/g, ''),
            };
            var ClientesInformacionDomiciliar = {
                fiIDCiudad: $("#ciudad").val(),
                fiIDBarrioColonia: $("#barrioColonia :selected").val(),
                fiIDDepto: $("#departamento :selected").val(),
                fiIDMunicipio: $("#municipio :selected").val(),
                fiIDCiudad: $("#ciudad :selected").val(),
                fcTelefonoCasa: $("#telefonoCasa").val(),
                fcTelefonoMovil: $("#telefonoMovil").val(),
                fcDireccionDetallada: $("#direccionDetallada").val(),
                fcReferenciasDireccionDetallada: $("#referenciaDireccionDetallada").val()
            };

            var ClientesInformacionConyugal = {};

            var requiereInformacionConyugal = $("input[name='estadoCivil']:checked").data('info');
            if (requiereInformacionConyugal == true) {
                ClientesInformacionConyugal = {
                    fcNombreCompletoConyugue: $("#nombresConyugue").val() + ' ' + $("#apellidosConyugue").val(),
                    fcIndentidadConyugue: $("#identidadConyugue").val(),
                    fdFechaNacimientoConyugue: $("#fechaNacimientoConyugue").val(),
                    fcTelefonoConyugue: $("#telefonoConyugue").val(),
                    fcLugarTrabajoConyugue: $("#lugarTrabajoConyugue").val(),
                    fcIngresosMensualesConyugue: $("#ingresoMensualesConyugue").val().replace(/,/g, '') == '' ? 0 : $("#ingresoMensualesConyugue").val().replace(/,/g, ''),
                    fcTelefonoTrabajoConyugue: $("#telefonoTrabajoConyugue").val()
                };
            }

            ClientesReferencias = listaClientesReferencias;

            var qString = "?" + window.location.href.split("?")[1];
            $.ajax({
                type: "POST",
                url: 'SolicitudesCredito_Registrar.aspx/IngresarSolicitud' + qString,
                data: JSON.stringify({ clienteNuevo: clienteNuevo, SolicitudesMaster: SolicitudesMaster, ClienteMaster: ClienteMaster, ClientesInformacionLaboral: ClientesInformacionLaboral, ClientesInformacionDomiciliar: ClientesInformacionDomiciliar, ClientesInformacionConyugal: ClientesInformacionConyugal, ClientesReferencias: ClientesReferencias, bitacora: bitacora }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError('No se guardó el registro, contacte al administrador');
                },
                success: function (data) {
                    if (data.d.response == true) {
                        MensajeExito(data.d.message);
                        localStorage.clear();
                        sessionStorage.clear();
                        clienteID = 0;
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

var clienteNuevo = true;
var ListaMunicipios = [];
var ListaCiudades = [];
var ListaBarriosColonias = [];
var objPrecalificado = [];
var ingresoInicio = '';
var estadoFuncionLlenarDDL = false;
var estadoFuncionRecuperarInfoCliente = false;
var estadoFuncionRecuperarRespaldos = false;

$(document).ready(function () {

    // Cuando se muestre un step
    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {

        // si es el primer paso, deshabilitar el boton "anterior"
        if (stepPosition === 'first') {
            $("#prev-btn").addClass('disabled').css('display', 'none');
        }
        else if (stepPosition === 'final') { // si es el ultimo paso, deshabilitar el boton siguiente
            $("#next-btn").addClass('disabled');
            $("#btnGuardarSolicitud").css('display', '');
        }
        else { // si no es ninguna de las anteriores, habilitar todos los botones
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
            $("#btnGuardarSolicitud").css('display', 'none');
        }
        if (stepNumber == 4) {
            $('#frmSolicitud').parsley().reset();//inicializar validaciones de los formularios
        }
    });

    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');
    });

    // cuando se deja un paso (realizar validaciones)
    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

        if ($("input[name='estadoCivil']:checked").data('info') == false) {
            $('#smartwizard').smartWizard("stepState", [4], "hide");//si no requere informacion personal, saltarse esa pestaña
        }
        else if ($("input[name='estadoCivil']:checked").data('info') == true) {
            $('#smartwizard').smartWizard("stepState", [4], "show");
        }

        if (stepDirection == 'forwards') {// validar solo si se quiere ir hacia el siguiente paso

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
                var DescripcionPrestamo = $("#tipoPrestamo").data('cod');

                if (parseInt(pmoSeleccionadoValidacion) < valorFinanciarValidacion) {//valor a financiar mayor al pmo sugerido
                    state = false;
                    MensajeError('Valor a financiar mayor al pmo sugerido');
                }

                if (parseInt(primaValidacion) > valorVehiculoValidacion) {//prima mayor a valor vehiculo
                    state = false;
                    MensajeError('Prima mayor a valor vehiculo');
                }

                if (DescripcionPrestamo == '201' || DescripcionPrestamo == '101') {//prima minima de 10% en caso de ser moto o efectivo
                    var financiar = DescripcionPrestamo == '201' ? parseFloat($("#txtValorVehiculo").val().replace(/,/g, '')) - primaValidacion : parseFloat($("#txtMontoPmoEfectivo").val().replace(/,/g, ''));
                    var primaMinima = (financiar * 10) / 100;
                    if (primaValidacion < primaMinima) {
                        state = false;
                        MensajeError('Prima minima de 10%');
                    }
                }

                if (DescripcionPrestamo == '201' || DescripcionPrestamo == '101') {//monto minimo a financiar en caso de ser moto o efectivo
                    var montoMinimo = 6000.00;
                    var financiar = DescripcionPrestamo == '201' ? parseFloat($("#txtValorVehiculo").val().replace(/,/g, '')) - primaValidacion : parseFloat($("#txtMontoPmoEfectivo").val().replace(/,/g, ''));
                    if (financiar < montoMinimo) {
                        state = false;
                        MensajeError('Monto minimo a financiar es 6,000');
                    }
                }

                if (DescripcionPrestamo == '101') {//si es prestamo en efectivo
                    var montoPmoEfectivo = $('#txtMontoPmoEfectivo').val().replace(/,/g, '');
                    if (montoPmoEfectivo != '') {
                        if (parseFloat(montoPmoEfectivo) > parseFloat(pmoSeleccionadoValidacion) || parseFloat(montoPmoEfectivo) == "0.00") {
                            state = false;
                        }
                    }
                    else {
                        state = false;
                    }
                }
                return state;
            }

            if (stepNumber == 1) {//validar informacion personal
                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' });
                if (state == true) {
                    guardarRespaldoInformacionPersonal();//si el formulario es valido, guardar respaldo en el localstorage
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
                }
                return state;
            }

            if (stepNumber == 2) {//validar informacion domiciliar
                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomiciliar' });
                if (state == true) {
                    guardarRespaldoInformacionDomiciliar();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionDomiciliar', force: true });
                }
                return state;
            }

            if (stepNumber == 3) {//validar informacion laboral
                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });
                if (state == true) {
                    guardarRespaldoInformacionLaboral();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
                }
                return state;
            }

            if (stepNumber == 4) {//validar informacion conyugal

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

            if (stepNumber == 5) {//validar referencias personales

                var state = false;
                if (listaClientesReferencias != null) {//si la lista de referencias no es nula y contiene una o mas referencias, guardar un respaldo de estas
                    if (listaClientesReferencias.length > 0) {
                        guardarRespaldoReferenciasPersonales();
                        if (listaClientesReferencias.length >= 4) {
                            state = true;
                        } else {
                            MensajeError('La cantidad mínima de referencias es 4, entre ellos 2 familiares');
                        }
                    }
                }
                return state;
            }
        }//termina if fowards
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
    if (localStorage.getItem("EnIngresoInicio") == null || localStorage.getItem("EnIngresoInicio") == "undefined" || localStorage.getItem("EnIngresoInicio") == undefined) {
        //obtenerFechaActual();
    }
});

/* agregar referencias personales (abrir modal) */
$("#btnNuevaReferencia").click(function () {
    resetForm($('#addReferencia-form'));
    $('#addReferencia-form').parsley().reset();
    $("#modalAddReferencia").modal("show");
});

/* agregar referencia personal a la tabla y al listado de referencias personales */
var cantidadReferencias = 0;
var listaClientesReferencias = [];
$('#addReferencia-form').submit(function (e) {

    if ($(this).parsley().isValid()) {//validar formulario de agregar referencia personal
        e.preventDefault();
        var nombreref = $("#nombreCompletoRef").val();
        var lugarTrabajoRef = $("#lugarTrabajoRef").val();
        var tiempoConocerRefRow = parseInt($("#tiempoConocerRef").val()) <= 2 ? $("#tiempoConocerRef").val() + ' años' : 'Más de 2 años';
        var tiempoConocerRef = parseInt($("#tiempoConocerRef").val());
        var telefonoRef = $("#telefonoRef").val();
        var parentescoRef = $("#parentescoRef").val();
        var parentescoText = $("#parentescoRef :selected").text();

        tableReferencias.row.add([//agregar referencia a la tabla de referencias personales
            nombreref,
            lugarTrabajoRef,
            tiempoConocerRefRow,
            telefonoRef,
            parentescoText,
        ]).draw(false);

        $("#modalAddReferencia").modal('hide');//ocultar modal de agregar referencia

        cantidadReferencias = cantidadReferencias + 1;//incrementar variable contadora de referencias personales para validaciones

        //objeto referencia
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

        listaClientesReferencias.push(referencia);//agregar objeto referencia a la lista de referencias personales que se enviará al servidor
    }
});

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

            var ChangeInput = '<div class="fileuploader-input">' +
                '<button type="button" class="fileuploader-input-button btn-sm BotonSubirArchivo"><span>${captions.button}</span></button>' +
                '</div>';

            var divDocumentacion = $("#DivDocumentacion");
            $.each(data.d, function (i, iter) {

                var IDInput = 'Doc' + iter.IDTipoDocumento;
                divDocumentacion.append("<div class='col-sm-2'>" +
                    "<label class='form-label'>" + iter.DescripcionTipoDocumento + "</label>" +
                    "<form action='SolicitudesCredito_Registrar.aspx?type=upload&doc=" + iter.IDTipoDocumento + "' method='post' enctype='multipart/form-data'>" +
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
                        url: 'SolicitudesCredito_Registrar.aspx?type=upload&doc=' + iter.IDTipoDocumento,
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
                });
            });
        }
    });
}

$("#tipoPrestamo").on('change', function () {

    if ($("#tipoPrestamo").val() != '') {

        if ($("#tipoPrestamo option:selected").val() == '101') {//si el prestamo requerido es en efectivo, no se requiere prima y el plazo es quincenal
            $(".divPrestamoVehiculo").css('display', 'none');
            $("#ddlOrigen,#titleOrigen").css('display', 'none');
            $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar,#ddlOrigen").prop('disabled', true);
            $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar").val('');
            $('#txtMontoPmoEfectivo').css('display', '');
            $('#txtMontoPmoEfectivo').prop('disabled', false);
            $(".divPrestamoEfectivo").css('display', '');

            var montoPmoEfectivo = $('#txtMontoPmoEfectivo').val().replace(/,/g, '');
            if (montoPmoEfectivo != '') {
                cargarPrestamosSugeridosPrima(montoPmoEfectivo, "0");
            } else {
                MensajeError('No se cargaron los préstamos sugeridos porque no se ha introducido un monto válido');
            }
        }
        else {//de lo contrario, si se requiere prima y el plazo es mensual
            $(".divPrestamoVehiculo").css('display', '');
            $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar,#ddlOrigen").prop('disabled', false);
            $("#ddlOrigen,#titleOrigen").css('display', '');
            var codigoProducto = $("#tipoPrestamo").data('cod');
            cargarOrigenes(codigoProducto);
        }
        $("#lblPlazoPMO").text('Plazo');
    }
    else {
        $(".divPrestamoVehiculo").css('display', 'none');
        $("#ddlOrigen,#titleOrigen").css('display', 'none');
        $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar,#ddlOrigen").prop('disabled', true);
        $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar").val('');
    }
});

/* Cargar informacion de clientes existentes, pendiente pasar esta logica al backend */
function VerificarExistenciaCliente() {

    /* verificar que no hayan respaldos ANTERIORES de solicitudes de clientes diferentes al actual, si las identidades no coinciden, quiere decir que son clientes diferentes,
    * entonces se borraran los respaldos ANTERIORES y se iniciará el proceso de ingreso como una solicitud completamento nueva
    */
    var RespaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));

    if (RespaldoInformacionPrestamo != null) {
        if (objPrecalificado.identidad == RespaldoInformacionPrestamo.identidadCliente) {
            recuperarRespaldos();
        } else {
            estadoFuncionRecuperarRespaldos = true;
            localStorage.clear(); // eliminar respaldos anteriores
            obtenerFechaActual(); // determinar la hora en la que se empieza a llenar la solicitd
        }
    } else {
        estadoFuncionRecuperarRespaldos = true;
        localStorage.clear(); // eliminar respaldos anteriores
        obtenerFechaActual(); // determinar la hora en la que se empieza a llenar la solicitud
    }
}

var clienteID = 0; // id clt

function cargarPrestamosSugeridosPrima(valorProducto, valorPrima) {

    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarPrestamosSugeridos" + qString,
        data: JSON.stringify({ valorProducto: valorProducto.replace(/,/g, ''), valorPrima: valorPrima.replace(/,/g, '') }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            $("#titlePrestamosSugeridos,#divPmosSugeridos").css('display', 'none');
            MensajeError('No se pudo cargar los préstamos sugeridos, contacte al administrador');
        },
        success: function (data) {

            var listaPrestamosSugeridos = data.d.cotizadorProductos;
            var DDLMontosSugeridos = $("#pmosSugeridos");
            DDLMontosSugeridos.empty();
            DDLMontosSugeridos.append("<option selected value='''>Seleccione una opción</option>");
            DDLMontosSugeridos.append("<option value='" + listaPrestamosSugeridos[0].fnMontoOfertado + "' data-pmoplz='" + listaPrestamosSugeridos[0].fiPlazo + "'>" + 'Producto: ' + listaPrestamosSugeridos[0].ProductoDescripcion + ' | Monto ofertado: ' + listaPrestamosSugeridos[0].fnMontoOfertado + ' | Plazo ' + listaPrestamosSugeridos[0].TipoCuota + ': ' + listaPrestamosSugeridos[0].fiPlazo + ' | Cuota ' + listaPrestamosSugeridos[0].TipoCuota + ': ' + listaPrestamosSugeridos[0].fnCuotaQuincenal + "</option>");

            for (var i = 1; i < listaPrestamosSugeridos.length; i++) {
                DDLMontosSugeridos.append("<option value='" + listaPrestamosSugeridos[i].fnMontoOfertado + "' data-pmoplz='" + listaPrestamosSugeridos[i].fiPlazo + "'>" + 'Producto: ' + listaPrestamosSugeridos[i].ProductoDescripcion + ' | Monto ofertado: ' + listaPrestamosSugeridos[i].fnMontoOfertado + ' | Plazo ' + listaPrestamosSugeridos[i].TipoCuota + ': ' + listaPrestamosSugeridos[i].fiPlazo + ' | Cuota ' + listaPrestamosSugeridos[i].TipoCuota + ': ' + listaPrestamosSugeridos[i].fnCuotaQuincenal + "</option>");

            }
            $("#titlePrestamosSugeridos,#divPmosSugeridos").css('display', '');
        }
    });
}

/* seleccionar un monto sugerido diferente, actualmente no se utiliza porque se puso un maximo de monto sugerido */
$("#pmoSugeridoSeleccionado").change(function () {
    var montoSeccionado = $(this).val();
    var cotizacionSeleccionada = listadoCotizaciones.filter(x => x.fnMontoOfertado == montoSeccionado);
    $("#plazoPmoSeleccionado").val(cotizacionSeleccionada[0].fiPlazo);
    $("#cutoaQuinceal").val(cotizacionSeleccionada[0].fnCuotaQuincenal);
});

var listadoCotizaciones = [];
function recuperarInformacionPrecalificado(objPrecalificado) {

    $("#tipoPrestamo,#btnBuscarCliente,#rtnCliente,#identidadCliente,#primerNombreCliente,#SegundoNombreCliente,#primerApellidoCliente,#segundoApellidoCliente,#ingresosPrecalificado,#ingresosMensuales,#numeroTelefono,#telefonoMovil,#fechaNacimiento").prop('disabled', true);
    $("#tipoPrestamo").val(objPrecalificado.Producto);
    $("#tipoPrestamo").data('cod', objPrecalificado.tipoProducto);
    $("#lblPlazoPMO").text('Plazo ' + objPrecalificado.cotizadorProductos[0].TipoCuota);
    $("#lblCuotaMaxima").text('Cuota ' + objPrecalificado.cotizadorProductos[0].TipoCuota);

    if ($("#tipoPrestamo").data('cod') != '101') {
        cargarOrigenes($("#tipoPrestamo").data('cod')); // cargar origenes de este tipo de producto
        $(".divPrestamoVehiculo").css('display', '');
        $('#txtValorVehiculo,#txtPrima,#txtValorFinanciar').css('display', '');
        $('#txtValorVehiculo,#txtPrima').prop('disabled', false);
    } else {
        $(".divPrestamoVehiculo").css('display', 'none');
        $('#txtValorVehiculo,#txtPrima,#txtValorFinanciar').css('display', 'none');
        $('#txtValorVehiculo,#txtPrima').prop('disabled', true);

        $('#txtMontoPmoEfectivo').css('display', '');
        $('#txtMontoPmoEfectivo').prop('disabled', false);
        $(".divPrestamoEfectivo").css('display', '');

        var montoPmoEfectivo = $('#txtMontoPmoEfectivo').val().replace(/,/g, '');
        if (montoPmoEfectivo != '') {
            cargarPrestamosSugeridosPrima(montoPmoEfectivo, "0");
        }
    }
}

$('#txtValorVehiculo').blur(function () {

    var DescripcionPrestamo = $("#tipoPrestamo").data('cod');
    var prestamoSeleccionado = parseFloat($("#pmoSugeridoSeleccionado :selected").val()).toFixed(2);
    var valorVehiculo = $('#txtValorVehiculo').val().replace(/,/g, '');
    var prima = $('#txtPrima').val().replace(/,/g, '');

    /* si el tipo de prestamo es 201, calcular la prima minima la cual es el 10% del valor del vehiculo */
    if (DescripcionPrestamo == '201') {
        var primaMinima = (valorVehiculo * 10) / 100;
        prima = primaMinima;
        $('#txtPrima').val(prima);
    }

    var totalAFinanciar = valorVehiculo - prima;// calcular total a financiar
    $('#txtValorFinanciar').val(totalAFinanciar);

    var mensajeErrorFinanciar = '';
    if (totalAFinanciar < 6000.00 && (DescripcionPrestamo == '201' || DescripcionPrestamo == '101')) { // validar que el valor minimo a financiar para efectivo y motos
        mensajeErrorFinanciar = 'El valor mínimo a financiar es 6,000.00';
    }

    if (totalAFinanciar > prestamoSeleccionado) { // si el total a financiar es mayor que el PMO MAXIMO SUGERIDO, mostrar validacion al usuario
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

    if (mensajeErrorFinanciar == '' && $('#txtPrima').val() != '') { // si no hay errores, cargar nuevas ofertas de prestamos
        cargarPrestamosSugeridosPrima($("#txtValorVehiculo").val(), $('#txtPrima').val());
    }
});

/* calcular monto a financiar y validar que no sea mayor que el prestamo sugerido seleccionado */

$('#txtPrima').blur(function () {
    var mensajeError = '';
    var CodProducto = $("#tipoPrestamo").data('cod');

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

    if (parseFloat(prima) >= parseFloat(valorVehiculo)) { // validar que la prima no sea mayor que el valor del vehiculo
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

    if (totalAFinanciar < 6000.00 && (CodProducto == '201' || DescripcionPrestamo == '101')) { // validar monto minimo a financiar para motos y efectivo
        var financiar = DescripcionPrestamo == '201' ? parseFloat($("#txtValorVehiculo").val().replace(/,/g, '')) : parseFloat($("#txtMontoPmoEfectivo").val().replace(/,/g, ''));
        mensajeErrorFinanciar = 'El valor mínimo a financiar es 6,000.00';
    }

    if (totalAFinanciar > prestamoSeleccionado) { // si el total a financiar es mayor que el PMO SUGERIDO MAXIMO, mostrar validacion al usuario
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

    if ($("#txtValorVehiculo").val() != '' && mensajeErrorFinanciar == '') {// si no hay errores, cargar nuevas ofertas de prestamos
        cargarPrestamosSugeridosPrima($("#txtValorVehiculo").val(), $('#txtPrima').val());
    }
});

$('#txtMontoPmoEfectivo').blur(function () {

    var prestamoSeleccionado = parseFloat($("#pmoSugeridoSeleccionado :selected").val()).toFixed(2);
    var montoEfectivo = $('#txtMontoPmoEfectivo').val().replace(/,/g, '');
    var mensajeErrorFinanciar = '';

    if (parseFloat(montoEfectivo) > prestamoSeleccionado) {
        mensajeErrorFinanciar = 'El monto no puede ser mayor que el PMO sugerido.';
    }
    if (parseFloat(montoEfectivo) < 6000) {
        mensajeErrorFinanciar = 'El valor minimo a financiar es 6,000.';
    }
    if (mensajeErrorFinanciar != '') {
        $('#txtMontoPmoEfectivo').addClass('parsley-error');
        $('#txtMontoPmoEfectivo').addClass('text-danger');
        MensajeError(mensajeErrorFinanciar);
    } else {
        $('#txtMontoPmoEfectivo').removeClass('parsley-error');
        $('#txtMontoPmoEfectivo').removeClass('text-danger');
    }

    if (mensajeErrorFinanciar == '' && $('#txtMontoPmoEfectivo').val().replace(/,/g, '') != '') { // si no hay errores, cargar nuevas ofertas de prestamos
        cargarPrestamosSugeridosPrima($("#txtMontoPmoEfectivo").val(), "0.00");
    } else {
        MensajeError('No se cargaron los prestamos ofertados');
    }
});

var ListaMunicipiosDomicilioCargada = false;
var ListaCiudadesDomicilioCargada = false;
var ListaBarriossDomicilioCargada = false;

/* habilitar ddl de municipios de la informacion de domicilio del cliente cuando se seleccione un departamento cliente valido */
var CODDepto = 0;
$("#departamento").change(function () {

    $(this).parsley().validate();
    CODDepto = $("#departamento option:selected").val(); // Departamento seleccionado
    var municipioDdl = $("#municipio");
    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (CODDepto != '' && CODDepto != 0) {

        $("#spinnerCargando").css('display', '');
        municipioDdl.empty();
        municipioDdl.append("<option value=''>Seleccione una opción</option>");
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarMunicipios",
            data: JSON.stringify({ CODDepto: CODDepto }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al cargar municipios de este departamento');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {

                var municipiosDelDepto = data.d;
                $.each(municipiosDelDepto, function (i, iter) {
                    municipioDdl.append("<option value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>"); // Agregar municipios del departamento seleccionado
                });
                municipioDdl.attr('disabled', false);

                ciudadDdl.empty();
                ciudadDdl.append("<option value=''>Seleccione un municipio</option>");
                ciudadDdl.attr('disabled', true);

                BarrioColoniaDdl.empty();
                BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
                BarrioColoniaDdl.attr('disabled', true);
                $("#spinnerCargando").css('display', 'none');
            }
        });
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

/* habilitar ddl ciudades de la informacion de domicilio del cliente cuando se seleccione un municipio valido */
var CODMunicipio = 0;
$("#municipio").change(function () {

    $(this).parsley().validate();
    CODMunicipio = $("#municipio option:selected").val(); // Municipio seleccionado
    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (CODMunicipio != '' && CODMunicipio != 0) {

        $("#spinnerCargando").css('display', '');
        ciudadDdl.empty();
        ciudadDdl.append("<option value=''>Seleccione una opción</option>");
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarPoblados",
            data: JSON.stringify({ CODDepto: CODDepto, CODMunicipio: CODMunicipio }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al cargar ciudades de este municipio');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {
                var ciudadesDelMunicipio = data.d;
                $.each(ciudadesDelMunicipio, function (i, iter) {
                    ciudadDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>"); // Agregar ciudades/poblados del municipio
                });
                ciudadDdl.attr('disabled', false);

                BarrioColoniaDdl.empty();
                BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
                BarrioColoniaDdl.attr('disabled', true);
                $("#spinnerCargando").css('display', 'none');
            }
        });
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

/* habilitar ddl de barrios y colonias de la informacion de domicilio del cliente cuando se seleccione una ciudad valida */
var CODCiudad = 0;
$("#ciudad").change(function () {

    CODCiudad = $("#ciudad option:selected").val(); // Ciudad seleccionada
    $(this).parsley().validate();
    var BarrioColoniaDdl = $("#barrioColonia");

    if (CODCiudad != '' && CODCiudad != 0) {

        $("#spinnerCargando").css('display', '');
        BarrioColoniaDdl.empty();
        BarrioColoniaDdl.append("<option value=''>Seleccione una opción</option>");
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarBarrios",
            data: JSON.stringify({ CODDepto: CODDepto, CODMunicipio: CODMunicipio, CODPoblado: CODCiudad }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al cargar barrios de esta ciudad');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {

                var barriosDeLaCiudad = data.d;
                $.each(barriosDeLaCiudad, function (i, iter) {
                    BarrioColoniaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>"); // Agregar barrios/colonias de la ciudad seleccionada
                });
                BarrioColoniaDdl.attr('disabled', false);
                $("#spinnerCargando").css('display', 'none');
            }
        });
    }
    else {
        BarrioColoniaDdl.empty();
        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
        BarrioColoniaDdl.attr('disabled', true);
    }
});

/* validar que se haya seleccionado un barrio valido o mostrar la validacion al usuario */
$("#barrioColonia").change(function () {
    $(this).parsley().validate();
});

/* habilitar ddl de municipios de la informacion laboral del cliente cuando se seleccione un departamento valido */
var CODDeptoEmpresa = 0;
$("#departamentoEmpresa").change(function () {

    $(this).parsley().validate();
    CODDeptoEmpresa = $("#departamentoEmpresa option:selected").val(); // Departamento seleccionado
    var municipioEmpresaDdl = $("#municipioEmpresa");
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (CODDeptoEmpresa != '' && CODDeptoEmpresa != 0) {

        $("#spinnerCargando").css('display', '');
        municipioEmpresaDdl.empty();
        municipioEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarMunicipios",
            data: JSON.stringify({ CODDepto: CODDeptoEmpresa }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al cargar municipios de este departamento');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {
                var municipiosDelDepto = data.d;
                $.each(municipiosDelDepto, function (i, iter) {
                    municipioEmpresaDdl.append("<option value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>"); // Agregar los municipios del departamento seleccionado
                });
                municipioEmpresaDdl.attr('disabled', false);
                ciudadaEmpresaDdl.empty();
                ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");
                ciudadaEmpresaDdl.attr('disabled', true);

                barrioColoniaEmpresaDdl.empty();
                barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
                barrioColoniaEmpresaDdl.attr('disabled', true);
                $("#spinnerCargando").css('display', 'none');
            }
        });
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

/* habilitar ddl de ciudades de la informacion laboral del cliente cuando se seleccione un municipio valido */
var CODMunicipioEmpresa = 0;
$("#municipioEmpresa").change(function () {

    $(this).parsley().validate();
    CODMunicipioEmpresa = $("#municipioEmpresa option:selected").val();// Municipio seleccionado
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (CODMunicipioEmpresa != '' && CODMunicipioEmpresa != 0) {

        $("#spinnerCargando").css('display', '');
        ciudadaEmpresaDdl.empty();
        ciudadaEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarPoblados",
            data: JSON.stringify({ CODDepto: CODDeptoEmpresa, CODMunicipio: CODMunicipioEmpresa }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al cargar ciudades de este municipio');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {
                var ciudadesDelMunicipio = data.d;
                $.each(ciudadesDelMunicipio, function (i, iter) {
                    ciudadaEmpresaDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>"); // Agregar ciudades del municipio seleccionado
                });
                ciudadaEmpresaDdl.attr('disabled', false);

                barrioColoniaEmpresaDdl.empty();
                barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
                barrioColoniaEmpresaDdl.attr('disabled', true);
                $("#spinnerCargando").css('display', 'none');
            }
        });
    }
    else {
        ciudadaEmpresaDdl.empty();
        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");
        ciudadaEmpresaDdl.attr('disabled', true);

        barrioColoniaEmpresaDdl.empty();
        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
        barrioColoniaEmpresaDdl.attr('disabled', true);
    }
});

/* habilitar ddl de barrios y colonias de la informacion laboral del cliente cuando se seleccione una ciudad valida */
var CODCiudadEmpresa = 0;
$("#ciudadEmpresa").change(function () {

    $(this).parsley().validate();
    CODCiudadEmpresa = $("#ciudadEmpresa option:selected").val();
    var barriosEmpresaDdl = $("#barrioColoniaEmpresa");

    if (CODCiudadEmpresa != '' && CODCiudadEmpresa != 0) {

        $("#spinnerCargando").css('display', '');
        barriosEmpresaDdl.empty();
        barriosEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarBarrios",
            data: JSON.stringify({ CODDepto: CODDeptoEmpresa, CODMunicipio: CODMunicipioEmpresa, CODPoblado: CODCiudadEmpresa }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al cargar barrios de esta ciudad');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {
                var barriosDeLaCiudad = data.d;
                $.each(barriosDeLaCiudad, function (i, iter) {
                    barriosEmpresaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>"); // Agregar barrios/colonias de la ciudad seleccionada
                });
                barriosEmpresaDdl.attr('disabled', false);
                $("#spinnerCargando").css('display', 'none');
            }
        });
    }
    else {
        barriosEmpresaDdl.empty();
        barriosEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
        barriosEmpresaDdl.attr('disabled', true);
    }
});

/* validar que se haya seleccionado un barrio de la informacion laboral del cliente valido o mostrar la validacion al usuario */
$("#barrioColoniaEmpresa").change(function () {
    $(this).parsley().validate();
});

/* validar si el estado civil seleccionado requiere información conyugal */
$('input[type=radio][name=estadoCivil]').change(function () {

    var requiereInformacionConyugal = $("input[name='estadoCivil']:checked").data('info');

    if (requiereInformacionConyugal == false) {
        $('input.infoConyugal').attr('disabled', true);
        $('#smartwizard').smartWizard("stepState", [4], "hide");// si no se requiere información conyugal, deshabilitar ese formulario
    }
    else if (requiereInformacionConyugal == true) {
        $('input.infoConyugal').attr('disabled', false);
        $('#smartwizard').smartWizard("stepState", [4], "show");// si se requiere información conyugal, habilitar ese formulario
    }
});

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

function guardarRespaldoInformacionPrestamo() {
    var RespaldoInformacionPrestamo = {
        clienteID: clienteID,
        tipoPrestamo: $("#tipoPrestamo").val(),
        tipoSolicitud: $("#tipoSolicitud").val(),
        identidadCliente: $("#identidadCliente").val(),
        RTNCliente: $("#RTNCliente").val(),
        pmoSugeridoSeleccionado: $("#pmoSugeridoSeleccionado :selected").val(),
        plazoPmoSeleccionado: $("#plazoPmoSeleccionado option:selected").val(),
        prima: $("#txtPrima").val().replace(/,/g, ''),
        valorVehiculo: $("#txtValorVehiculo").val().replace(/,/g, ''),
        montoPmoEfectivo: $("#txtMontoPmoEfectivo").val().replace(/,/g, '')
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
        RTNCliente: $("#rtnCliente").val(),
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

    if (localStorage.getItem('RespaldoInformacionPrestamo') != null) { // recuperar respaldos del paso "informacion del prestamo"

        var RespaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));
        clienteID = RespaldoInformacionPrestamo.clienteID;
        $("#txtPrima").val(RespaldoInformacionPrestamo.prima);
        $("#txtValorVehiculo").val(RespaldoInformacionPrestamo.valorVehiculo);
        $("#txtValorFinanciar").val(RespaldoInformacionPrestamo.valorVehiculo - RespaldoInformacionPrestamo.prima);
        $("#txtMontoPmoEfectivo").val(RespaldoInformacionPrestamo.montoPmoEfectivo);
        var prestamoSeleccionadoMax = parseFloat($("#pmoSugeridoSeleccionado :selected").val()).toFixed(2);
        if ($("#tipoPrestamo").data('cod') == '101') { // si el prestamo requerido es en efectivo, no se requiere prima y el plazo es quincenal

            $(".divPrestamoVehiculo").css('display', 'none');
            $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar,#ddlOrigen").prop('disabled', true);
            $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar").val('');
            $("#ddlOrigen,#titleOrigen").css('display', 'none');

            $('#txtMontoPmoEfectivo').css('display', '');
            $('#txtMontoPmoEfectivo').prop('disabled', false);
            $(".divPrestamoEfectivo").css('display', '');

            var montoPmoEfectivo = $('#txtMontoPmoEfectivo').val().replace(/,/g, '');

            if (montoPmoEfectivo != '') {
                if (parseFloat(montoPmoEfectivo) <= parseFloat(prestamoSeleccionadoMax)) {
                    cargarPrestamosSugeridosPrima(montoPmoEfectivo, "0"); // cargar oferas de prestamos
                }
                else { MensajeError('No se cargaron los préstamos sugeridos porque no se ha introducido un monto válido'); }

            } else { MensajeError('No se cargaron los préstamos sugeridos porque no se ha introducido un monto válido'); }
        }
        else { // de lo contrario, si se requiere prima y el plazo es mensual

            $("#ddlOrigen").prop('disabled', false);
            $("#ddlOrigen,#titleOrigen").css('display', '');
            cargarOrigenes($("#tipoPrestamo").data('cod')); // cargar origenes del producto actual
            $("#lblPlazoPMO").text('Plazo');
            $(".divPrestamoVehiculo").css('display', '');
            $("#txtPrima,#txtValorVehiculo").prop('disabled', false);
            var prima = parseFloat($('#txtPrima').val().replace(/,/g, '') == '' ? 0 : $('#txtPrima').val().replace(/,/g, ''));
            var valorVehiculo = parseFloat($('#txtValorVehiculo').val().replace(/,/g, '') == '' ? 0 : $('#txtValorVehiculo').val().replace(/,/g, ''));
            var totalAFinanciar = valorVehiculo - prima;
            $('#txtValorFinanciar').val(totalAFinanciar);
            if (totalAFinanciar > prestamoSeleccionadoMax || valorVehiculo <= 0) {
                $('#txtValorFinanciar').addClass('parsley-error');
                $('#txtValorFinanciar').addClass('text-danger');
                $('#error-valorFinanciar').css('display', '');
            } else {
                $('#txtValorFinanciar').removeClass('parsley-error');
                $('#txtValorFinanciar').removeClass('text-danger');
                $('#error-valorFinanciar').css('display', 'none');
            }
            var estadoPrimaMinima = true;
            if ($("#tipoPrestamo").data('cod') == '201') {

                if (prima < (valorVehiculo * 0.10)) { estadoPrimaMinima = false; }
            }
            if (totalAFinanciar <= prestamoSeleccionadoMax && valorVehiculo >= 0 && valorVehiculo > prima && estadoPrimaMinima == true) {
                cargarPrestamosSugeridosPrima($('#txtValorVehiculo').val(), $('#txtPrima').val());
            }
            else { MensajeError('No se cargaron los préstamos sugeridos porque los valores ingresados no son válidos'); }
        }
    }

    if (localStorage.getItem('RespaldoInformacionPersonal') != null) { // recuperar informacion del paso "informacion personal"

        var respaldoInformacionPersonal = JSON.parse(localStorage.getItem('RespaldoInformacionPersonal'));
        clienteID = respaldoInformacionPersonal.clienteID;
        $("#nacionalidad").val(respaldoInformacionPersonal.nacionalidad);
        $("#fechaNacimiento").val(respaldoInformacionPersonal.fechaNacimiento);
        $("#correoElectronico").val(respaldoInformacionPersonal.correoElectronico);
        $("#rtnCliente").val(respaldoInformacionPersonal.RTNCliente);
        $("#profesion").val(respaldoInformacionPersonal.profesion);
        $("input[name=sexo][value=" + respaldoInformacionPersonal.sexo + "]").prop('checked', true);
        $("input[name=estadoCivil][value=" + respaldoInformacionPersonal.estadoCivil + "]").prop('checked', true);
        $("#vivivenda").val(respaldoInformacionPersonal.vivivenda);
        $("input[name=tiempoResidir][value=" + respaldoInformacionPersonal.tiempoResidir + "]").prop('checked', true);
    }

    if (localStorage.getItem('RespaldoInformacionDomiciliar') != null) { // recuperar informacion del paso "informacion domiciliar"
        var respaldoInformacionDomiciliar = JSON.parse(localStorage.getItem('RespaldoInformacionDomiciliar'));
        //$("#departamento").val(respaldoInformacionDomiciliar.departamento);
        //$("#municipio").val(respaldoInformacionDomiciliar.municipio);
        //$("#ciudad").val(respaldoInformacionDomiciliar.ciudad);
        //$("#barrioColonia").val(respaldoInformacionDomiciliar.barrioColonia);
        $("#telefonoCasa").val(respaldoInformacionDomiciliar.telefonoCasa);
        $("#telefonoMovil").val(respaldoInformacionDomiciliar.telefonoMovil);
        $("#direccionDetallada").val(respaldoInformacionDomiciliar.direccionDetallada);
        $("#referenciaDireccionDetallada").val(respaldoInformacionDomiciliar.referenciaDireccionDetallada);
    }

    if (localStorage.getItem('RespaldoInformacionLaboral') != null) { // recuperar informacion del paso "informacion laboral"
        var respaldoInformacionLaboral = JSON.parse(localStorage.getItem('RespaldoInformacionLaboral'));
        $("#nombreDelTrabajo").val(respaldoInformacionLaboral.nombreDelTrabajo);
        $("#ingresosMensuales").val(respaldoInformacionLaboral.ingresosMensuales);
        $("#puestoAsignado").val(respaldoInformacionLaboral.puestoAsignado);
        $("#fechaIngreso").val(respaldoInformacionLaboral.fechaIngreso);
        $("#telefonoEmpresa").val(respaldoInformacionLaboral.telefonoEmpresa);
        $("#extensionRRHH").val(respaldoInformacionLaboral.extensionRRHH);
        $("#extensionCliente").val(respaldoInformacionLaboral.extensionCliente);
        //$("#departamentoEmpresa").val(respaldoInformacionLaboral.departamentoEmpresa);
        //$("#municipioEmpresa").val(respaldoInformacionLaboral.municipioEmpresa);
        //$("#ciudadEmpresa").val(respaldoInformacionLaboral.ciudadEmpresa);
        //$("#barrioColoniaEmpresa").val(respaldoInformacionLaboral.barrioColoniaEmpresa);
        $("#direccionDetalladaEmpresa").val(respaldoInformacionLaboral.direccionDetalladaEmpresa);
        $("#referenciaDireccionDetalladaEmpresa").val(respaldoInformacionLaboral.referenciaDireccionDetalladaEmpresa);
        $("#fuenteOtrosIngresos").val(respaldoInformacionLaboral.fuenteOtrosIngresos);
        $("#valorOtrosIngresos").val(respaldoInformacionLaboral.valorOtrosIngresos);
    }

    if (localStorage.getItem('RespaldoInformacionConyugal') != null) { // recuperar respaldos de la pestana "Informacion conyugal"
        var respaldoInformacionConyugal = JSON.parse(localStorage.getItem('RespaldoInformacionConyugal'));
        $("#nombresConyugue").val(respaldoInformacionConyugal.nombresConyugue);
        $("#apellidosConyugue").val(respaldoInformacionConyugal.apellidosConyugue);
        $("#identidadConyugue").val(respaldoInformacionConyugal.identidadConyugue);
        $("#fechaNacimientoConyugue").val(respaldoInformacionConyugal.fechaNacimientoConyugue);
        $("#telefonoConyugue").val(respaldoInformacionConyugal.telefonoConyugue);
        $("#lugarTrabajoConyugue").val(respaldoInformacionConyugal.lugarTrabajoConyugue);
        $("#ingresoMensualesConyugue").val(respaldoInformacionConyugal.ingresoMensualesConyugue);
        $("#telefonoTrabajoConyugue").val(respaldoInformacionConyugal.telefonoTrabajoConyugue);
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

    if (localStorage.getItem('RespaldoReferenciasPersonales') != null) { // recuperar respaldo de pestana "Referencias personales"

        RespaldolistaClientesReferencias = JSON.parse(localStorage.getItem('RespaldoReferenciasPersonales'));
        $('#datatable-buttons').DataTable().clear();
        listaClientesReferencias = [];
        if (RespaldolistaClientesReferencias.length > 0) {
            for (var i = 0; i < RespaldolistaClientesReferencias.length; i++) {

                $('#datatable-buttons').dataTable().fnAddData([ // agregar las referencias personales al datatabel
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
        }
    }
    estadoFuncionRecuperarRespaldos = true
    if (estadoFuncionRecuperarInfoCliente == true && estadoFuncionLlenarDDL == true) {
        $("#spinnerCargando").css('display', 'none');
    }
}