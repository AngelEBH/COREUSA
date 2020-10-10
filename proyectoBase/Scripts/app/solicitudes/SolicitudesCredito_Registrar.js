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

        if (cantidadReferencias == 0) {
            if (modelStateInformacionPrestamo == true && modelStateInformacionPersonal == true && modelStateInformacionDomicilio == true && modelStateInformacionLaboral == true && modelStateInformacionConyugal == true) {
                iziToast.warning({
                    title: 'Atención',
                    message: 'Se requieren mínimo 4 referencias personales. Entre ellas 2 familiares.'
                });
            }
        }
        if (modelStateInformacionPrestamo == true && modelStateInformacionPersonal == true && modelStateInformacionDomicilio == true && modelStateInformacionLaboral == true && modelStateInformacionConyugal == true && cantidadReferencias > 0) {

            var SolicitudesMaster = {
                fiIDTipoPrestamo: $("#tipoPrestamo").data('cod'),
                fcTipoSolicitud: $("#tipoSolicitud").val(),
                fnPrima: $("#txtPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtPrima").val().replace(/,/g, ''),
                fnValorGarantia: $("#txtValorVehiculo").val().replace(/,/g, '') == '' ? 0 : $("#txtValorVehiculo").val().replace(/,/g, ''),
                fcTipoSolicitud: $("#tipoSolicitud").val(),
                fdValorPmoSugeridoSeleccionado: $("#pmosSugeridos option:selected").val(),
                fiPlazoPmoSeleccionado: $("#pmosSugeridos option:selected").data('plazoseleccionado'),
                fiIDOrigen: $("#ddlOrigen option:selected").val() == null ? 1 : parseInt($("#ddlOrigen option:selected").val())
            };
            var bitacora = {
                fdEnIngresoInicio: localStorage.getItem("EnIngresoInicio"),
                fdEnIngresoFin: ''
            };
            var ClienteMaster = {
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
                fiCiudadEmpresa: $("#ddlCiudadPobladoEmpresa").val(),
                IdBarrioColonia: $("#ddlBarrioColoniaEmpresa :selected").val(),
                fiIDDepto: $("#ddlDepartamentoEmpresa :selected").val(),
                IdMunicipio: $("#ddlMunicipioEmpresa :selected").val(),
                IdCiudadPoblado: $("#ddlCiudadPobladoEmpresa :selected").val(),
                fcDireccionDetalladaEmpresa: $("#direccionDetalladaEmpresa").val(),
                fcReferenciasDireccionDetallada: $("#referenciaDireccionDetalladaEmpresa").val(),
                fcFuenteOtrosIngresos: $("#fuenteOtrosIngresos").val(),
                fiValorOtrosIngresosMensuales: $("#valorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#valorOtrosIngresos").val().replace(/,/g, ''),
            };
            var ClientesinformacionDomicilio = {
                IdCiudadPoblado: $("#ciudad").val(),
                IdBarrioColonia: $("#barrioColonia :selected").val(),
                fiIDDepto: $("#departamento :selected").val(),
                IdMunicipio: $("#municipio :selected").val(),
                IdCiudadPoblado: $("#ciudad :selected").val(),
                fcTelefonoCasa: $("#telefonoCasa").val(),
                fcTelefonoMovil: $("#telefonoMovil").val(),
                fcDireccionDetallada: $("#direccionDetallada").val(),
                fcReferenciasDireccionDetallada: $("#referenciaDireccionDetallada").val()
            };

            var ClientesInformacionConyugal = {};

            var requiereInformacionConyugal = $("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal');

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

            ClientesReferencias = listaReferenciasPersonales;

            $.ajax({
                type: "POST",
                url: 'SolicitudesCredito_Registrar.aspx/IngresarSolicitud',
                data: JSON.stringify({ clienteNuevo: clienteNuevo, SolicitudesMaster: SolicitudesMaster, ClienteMaster: ClienteMaster, ClientesInformacionLaboral: ClientesInformacionLaboral, ClientesinformacionDomicilio: ClientesinformacionDomicilio, ClientesInformacionConyugal: ClientesInformacionConyugal, ClientesReferencias: ClientesReferencias, bitacora: bitacora }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError('No se guardó el registro, contacte al administrador');
                },
                success: function (data) {

                    if (data.d.response == true) {

                        MensajeExito(data.d.message);
                        localStorage.clear();
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

            $('#smartwizard').smartWizard("stepState", [4], "hide");

        }
        else if ($("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal') == true) {

            $('#smartwizard').smartWizard("stepState", [4], "show");

        }

        /* Validar solo si se quiere ir hacia el siguiente paso */
        if (stepDirection == 'forwards') {

            if (stepNumber == 0) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPrestamo', excluded: ':disabled' }); /* Validar pestaña informacion prestamo */

                if (state == true) {
                    GuardarRespaldoInformacionPrestamo(); /* Si es valido, guardar respaldo en el localstorage */
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPrestamo', excluded: ':disabled', force: true }); /* Si no es valido, mostrar validaciones al usuario */
                }

                var valorVehiculoValidacion = parseFloat($("#txtValorVehiculo").val().replace(/,/g, '')).toFixed(2);
                var primaValidacion = parseFloat($("#txtPrima").val().replace(/,/g, '')).toFixed(2);
                var valorFinanciarValidacion = valorVehiculoValidacion - primaValidacion;
                var pmoSeleccionadoValidacion = parseFloat($("#pmoSugeridoSeleccionado :selected").val()).toFixed(2);
                var DescripcionPrestamo = $("#tipoPrestamo").data('cod');

                if (parseInt(pmoSeleccionadoValidacion) < valorFinanciarValidacion) {// valor a financiar mayor al pmo sugerido
                    state = false;
                    MensajeError('Valor a financiar mayor al pmo sugerido');
                }

                if (parseInt(primaValidacion) > valorVehiculoValidacion) {// prima mayor a valor vehiculo
                    state = false;
                    MensajeError('Prima mayor a valor vehiculo');
                }

                if (DescripcionPrestamo == '201' || DescripcionPrestamo == '101') {// prima minima de 10% en caso de ser moto o efectivo
                    var financiar = DescripcionPrestamo == '201' ? parseFloat($("#txtValorVehiculo").val().replace(/,/g, '')) - primaValidacion : parseFloat($("#txtMontoPmoEfectivo").val().replace(/,/g, ''));
                    var primaMinima = (financiar * 10) / 100;
                    if (primaValidacion < primaMinima) {
                        state = false;
                        MensajeError('Prima minima de 10%');
                    }
                }

                if (DescripcionPrestamo == '201' || DescripcionPrestamo == '101') {// monto minimo a financiar en caso de ser moto o efectivo
                    var montoMinimo = 6000.00;
                    var financiar = DescripcionPrestamo == '201' ? parseFloat($("#txtValorVehiculo").val().replace(/,/g, '')) - primaValidacion : parseFloat($("#txtMontoPmoEfectivo").val().replace(/,/g, ''));
                    if (financiar < montoMinimo) {
                        state = false;
                        MensajeError('Monto minimo a financiar es 6,000');
                    }
                }

                if (DescripcionPrestamo == '101') {// si es prestamo en efectivo
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

            /* Validar pestaña de la informacion personal del cliente */
            if (stepNumber == 1) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal', excluded: ':disabled' });

                if (state == true) {
                    GuardarRespaldoInformacionPersonal();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
                }
                return state;
            }

            /* Validar pestaña de la informacion de domicilio del cliente */
            if (stepNumber == 2) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomicilio' });

                if (state == true) {
                    GuardarRespaldoinformacionDomicilio();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionDomicilio', force: true });
                }
                return state;
            }

            /* Validar pestaña de la informacion laboral */
            if (stepNumber == 3) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

                if (state == true) {
                    GuardarRespaldoInformacionLaboral();
                }
                else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
                }
                return state;
            }

            /* Validar pestaña de la informacion del cónyugue */
            if (stepNumber == 4) {

                if ($("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal') == true) {

                    var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });

                    if (state == true) {
                        GuardarRespaldoInformacionConyugal();
                    }
                    else {
                        $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
                    }
                    return state;
                }
            }

            /* Validar referencias personales del cliente */
            if (stepNumber == 5) {

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

        if (CONSTANTES.IdentidadCliente == respaldoInformacionPrestamo.IdentidadCliente) {
            RecuperarRespaldos();
        }
        else {
            localStorage.clear();
        }
    }

    /* Guardar hora en el que se inicia el registro de la solicitud */
    if (localStorage.getItem("EnIngresoInicio") == null) {

        localStorage.setItem("EnIngresoInicio", JSON.stringify(CONSTANTES.HoraAlCargar));
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

        var nombreReferencia = $("#txtNombreReferencia").val();
        var telefonoReferencia = $("#txtTelefonoReferencia").val();
        var lugarTrabajoReferencia = $("#txtLugarTrabajoReferencia").val();
        var tiempoDeConocerReferencia = $("#ddlTiempoDeConocerReferencia :selected").val();
        var tiempoDeConocerReferenciaDescripcion = $("#ddlTiempoDeConocerReferencia :selected").text();
        var parentescoReferencia = $("#ddlParentescos :selected").val();
        var parentescoReferenciaDescripcion = $("#ddlParentescos :selected").text();
        var btnQuitarReferencia = '<button type="button" id="btnQuitarReferenciaPersonal" ' +
            'data-nombreReferencia="' + nombreReferencia + '" data-telefonoReferencia="' + telefonoReferencia + '" data-lugarTrabajoReferencia="' + lugarTrabajoReferencia + '"' +
            'data-tiempoDeConocerReferencia="' + tiempoDeConocerReferencia + '" data-tiempoDeConocerReferenciaDescripcion="' + tiempoDeConocerReferenciaDescripcion + '" data-parentescoReferencia="' + parentescoReferencia + '" data-parentescoReferenciaDescripcion="' + parentescoReferenciaDescripcion + '"' +
            'class="btn btn-sm btn-danger" > Quitar</button > ';

        /* Agregar referencia a la tabla de referencias personales */
        var row = '<tr><td>' + nombreReferencia + '</td><td>' + telefonoReferencia + '</td><td>' + lugarTrabajoReferencia + '</td><td>' + tiempoDeConocerReferenciaDescripcion + '</td><td>' + parentescoReferenciaDescripcion + '</td><td>' + btnQuitarReferencia + '</td></tr>';
        $("#tblReferenciasPersonales tbody").append(row);

        $("#modalAgregarReferenciaPersonal").modal('hide');

        cantidadReferencias++;

        /* Objeto referencia */
        var referencia = {
            nombreReferencia: nombreReferencia,
            telefonoReferencia: telefonoReferencia,
            lugarTrabajoReferencia: lugarTrabajoReferencia,
            tiempoDeConocerReferencia: tiempoDeConocerReferencia,
            tiempoDeConocerReferenciaDescripcion: tiempoDeConocerReferenciaDescripcion,
            parentescoReferencia: parentescoReferencia,
            parentescoReferenciaDescripcion: parentescoReferenciaDescripcion
        }

        /* Agregar objeto referencia a la lista de referencias personales que se enviará al servidor */
        listaReferenciasPersonales.push(referencia);
    }
});


/* Quitar referencia personal de la tabla */
$(document).on('click', 'button#btnQuitarReferenciaPersonal', function () {

    $(this).closest('tr').remove();

    var referenciaPersonal = {
        nombreReferencia: $(this).data('nombrereferencia'),
        telefonoReferencia: $(this).data('telefonoreferencia'),
        lugarTrabajoReferencia: $(this).data('lugartrabajoreferencia'),
        tiempoDeConocerReferencia: $(this).data('tiempodeconocerreferencia').toString(),
        tiempoDeConocerReferenciaDescripcion: $(this).data('tiempodeconocerreferenciadescripcion'),
        parentescoReferencia: $(this).data('parentescoreferencia').toString(),
        parentescoReferenciaDescripcion: $(this).data('parentescoreferenciadescripcion'),
    };

    var list = [];

    if (listaReferenciasPersonales.length > 0) {

        for (var i = 0; i < listaReferenciasPersonales.length; i++) {

            var iter = {
                nombreReferencia: listaReferenciasPersonales[i].nombreReferencia,
                telefonoReferencia: listaReferenciasPersonales[i].telefonoReferencia,
                lugarTrabajoReferencia: listaReferenciasPersonales[i].lugarTrabajoReferencia,
                tiempoDeConocerReferencia: listaReferenciasPersonales[i].tiempoDeConocerReferencia.toString(),
                tiempoDeConocerReferenciaDescripcion: listaReferenciasPersonales[i].tiempoDeConocerReferenciaDescripcion,
                parentescoReferencia: listaReferenciasPersonales[i].parentescoReferencia.toString(),
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
                    extensions: ['jpg', 'png'],// Extensiones/formatos permitidos
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
function CargarPrestamosOfertados(valorProducto, valorPrima) {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarPrestamosOfertados",
        data: JSON.stringify({ valorProducto: valorProducto.replace(/,/g, ''), valorPrima: valorPrima.replace(/,/g, '') }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeError('No se pudo cargar los préstamos sugeridos, contacte al administrador');
            $("#ddlPrestamosDisponibles").empty();
        },
        success: function (data) {

            var ListaPrestamosOfertados = data.d;

            var ddlPrestamosDisponibles = $("#ddlPrestamosDisponibles");
            ddlPrestamosDisponibles.empty();
            ddlPrestamosDisponibles.append("<option selected value='''>Seleccione una opción</option>");
            ddlPrestamosDisponibles.append("<option value='" + ListaPrestamosOfertados[0].MontoOfertado + "' data-plazoseleccionado='" + ListaPrestamosOfertados[0].Plazo + "'>" + 'Producto: ' + ListaPrestamosOfertados[0].Producto + ' | Monto ofertado: ' + ListaPrestamosOfertados[0].MontoOfertado + ' | Plazo ' + ListaPrestamosOfertados[0].TipoPlazo + ': ' + ListaPrestamosOfertados[0].Plazo + ' | Cuota ' + ListaPrestamosOfertados[0].TipoPlazo + ': ' + ListaPrestamosOfertados[0].Cuota + "</option>");

            for (var i = 1; i < ListaPrestamosOfertados.length; i++) {

                ddlPrestamosDisponibles.append("<option value='" + ListaPrestamosOfertados[i].MontoOfertado + "' data-plazoseleccionado='" + ListaPrestamosOfertados[i].Plazo + "'>" + 'Producto: ' + ListaPrestamosOfertados[i].Producto + ' | Monto ofertado: ' + ListaPrestamosOfertados[i].MontoOfertado + ' | Plazo ' + ListaPrestamosOfertados[i].TipoPlazo + ': ' + ListaPrestamosOfertados[i].Plazo + ' | Cuota ' + ListaPrestamosOfertados[i].TipoPlazo + ': ' + ListaPrestamosOfertados[i].Cuota + "</option>");
            }
        }
    });
}


/* Cargar municipios del departamento seleccionado del domicilio */
var idDepartamento = 0;
$("#ddlDepartamentoDomicilio").change(function () {

    $(this).parsley().validate();

    idDepartamento = $("#ddlDepartamentoDomicilio option:selected").val(); // Departamento seleccionado
    var ddlMunicipioDomicilio = $("#ddlMunicipioDomicilio");
    var ddlCiudadPobladoDomicilio = $("#ddlCiudadPobladoDomicilio");
    var ddlBarrioColoniaDomicilio = $("#ddlBarrioColoniaDomicilio");

    if (idDepartamento != '') {

        ddlMunicipioDomicilio.empty();
        ddlMunicipioDomicilio.append("<option value=''>Seleccione una opción</option>");

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
                    ddlMunicipioDomicilio.append("<option value='" + iter.IdMunicipio + "'>" + iter.NombreMunicipio + "</option>"); /* Agregar municipios del departamento seleccionado */
                });
                ddlMunicipioDomicilio.attr('disabled', false);

                ddlCiudadPobladoDomicilio.empty();
                ddlCiudadPobladoDomicilio.append("<option value=''>Seleccione un municipio</option>");
                ddlCiudadPobladoDomicilio.attr('disabled', true);

                ddlBarrioColoniaDomicilio.empty();
                ddlBarrioColoniaDomicilio.append("<option value=''>Seleccione una ciudad/poblado</option>");
                ddlBarrioColoniaDomicilio.attr('disabled', true);
            }
        });
    }
    else {
        ddlMunicipioDomicilio.empty();
        ddlMunicipioDomicilio.append("<option value=''>Seleccione un depto.</option>");
        ddlMunicipioDomicilio.attr('disabled', true);

        ddlCiudadPobladoDomicilio.empty();
        ddlCiudadPobladoDomicilio.append("<option value=''>Seleccione un municipio</option>");
        ddlCiudadPobladoDomicilio.attr('disabled', true);

        ddlBarrioColoniaDomicilio.empty();
        ddlBarrioColoniaDomicilio.append("<option value=''>Seleccione una ciudad/poblado</option>");
        ddlBarrioColoniaDomicilio.attr('disabled', true);
    }
});


/* Cargar ciudades del Municipio seleccionado del domicilio */
var idMunicipio = 0;
$("#ddlMunicipioDomicilio").change(function () {

    $(this).parsley().validate();

    idMunicipio = $("#ddlMunicipioDomicilio option:selected").val(); // Municipio seleccionado
    var ddlCiudadPobladoDomicilio = $("#ddlCiudadPobladoDomicilio");
    var ddlBarrioColoniaDomicilio = $("#ddlBarrioColoniaDomicilio");

    if (idMunicipio != '') {

        ddlCiudadPobladoDomicilio.empty();
        ddlCiudadPobladoDomicilio.append("<option value=''>Seleccione una opción</option>");

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
                    ddlCiudadPobladoDomicilio.append("<option value='" + iter.IdCiudadPoblado + "'>" + iter.NombreCiudadPoblado + "</option>"); // Agregar ciudades/poblados del municipio
                });
                ddlCiudadPobladoDomicilio.attr('disabled', false);

                ddlBarrioColoniaDomicilio.empty();
                ddlBarrioColoniaDomicilio.append("<option value=''>Seleccione una ciudad/poblado</option>");
                ddlBarrioColoniaDomicilio.attr('disabled', true);
            }
        });
    }
    else {
        ddlCiudadPobladoDomicilio.empty();
        ddlCiudadPobladoDomicilio.append("<option value=''>Seleccione un municipio</option>");
        ddlCiudadPobladoDomicilio.attr('disabled', true);

        ddlBarrioColoniaDomicilio.empty();
        ddlBarrioColoniaDomicilio.append("<option value=''>Seleccione una ciudad/poblado</option>");
        ddlBarrioColoniaDomicilio.attr('disabled', true);
    }
});


/* habilitar ddl de barrios y colonias de la informacion de domicilio del cliente cuando se Seleccione una ciudad/poblado valida */
var idCiudadPoblado = 0;
$("#ddlCiudadPobladoDomicilio").change(function () {

    $(this).parsley().validate();

    idCiudadPoblado = $("#ddlCiudadPobladoDomicilio option:selected").val(); // Ciudad o poblado seleccionado
    var ddlBarrioColoniaDomicilio = $("#ddlBarrioColoniaDomicilio");

    if (idCiudadPoblado != '') {

        ddlBarrioColoniaDomicilio.empty();
        ddlBarrioColoniaDomicilio.append("<option value=''>Seleccione una opción</option>");

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
                    ddlBarrioColoniaDomicilio.append("<option value='" + iter.IdBarrioColonia + "'>" + iter.NombreBarrioColonia + "</option>"); // Agregar barrios/colonias de la ciudad seleccionada
                });
                ddlBarrioColoniaDomicilio.attr('disabled', false);
            }
        });
    }
    else {
        ddlBarrioColoniaDomicilio.empty();
        ddlBarrioColoniaDomicilio.append("<option value=''>Seleccione una ciudad/poblado</option>");
        ddlBarrioColoniaDomicilio.attr('disabled', true);
    }
});


/* Validar que se seleccione un barrio/colonia válido */
$("#ddlBarrioColoniaDomicilio").change(function () {
    $(this).parsley().validate();
});


/* habilitar ddl de municipios de la informacion laboral del cliente cuando se seleccione un departamento valido */
var idDepartamentoEmpresa = 0;
$("#ddlDepartamentoEmpresa").change(function () {

    $(this).parsley().validate();

    idDepartamentoEmpresa = $("#ddlDepartamentoEmpresa option:selected").val(); // Departamento seleccionado

    var ddlMunicipioEmpresa = $("#ddlMunicipioEmpresa");
    var ddlCiudadPobladoEmpresa = $("#ddlCiudadPobladoEmpresa");
    var ddlBarrioColoniaEmpresa = $("#ddlBarrioColoniaEmpresa");

    if (idDepartamentoEmpresa != '') {

        ddlMunicipioEmpresa.empty();
        ddlMunicipioEmpresa.append("<option value=''>Seleccione una opción</option>");

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarListaMunicipios",
            data: JSON.stringify({ idDepartamento: idDepartamentoEmpresa }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al cargar municipios de este departamento');
            },
            success: function (data) {

                var listaMunicipios = data.d;

                $.each(listaMunicipios, function (i, iter) {
                    ddlMunicipioEmpresa.append("<option value='" + iter.IdMunicipio + "'>" + iter.NombreMunicipio + "</option>"); // Agregar los municipios del departamento seleccionado
                });
                ddlMunicipioEmpresa.attr('disabled', false);
                ddlCiudadPobladoEmpresa.empty();
                ddlCiudadPobladoEmpresa.append("<option value=''>Seleccione un municipio</option>");
                ddlCiudadPobladoEmpresa.attr('disabled', true);

                ddlBarrioColoniaEmpresa.empty();
                ddlBarrioColoniaEmpresa.append("<option value=''>Seleccione una ciudad/poblado</option>");
                ddlBarrioColoniaEmpresa.attr('disabled', true);
            }
        });
    }
    else {
        ddlMunicipioEmpresa.empty();
        ddlMunicipioEmpresa.append("<option value=''>Seleccione un depto.</option>");
        ddlMunicipioEmpresa.attr('disabled', true);

        ddlCiudadPobladoEmpresa.empty();
        ddlCiudadPobladoEmpresa.append("<option value=''>Seleccione un municipio</option>");
        ddlCiudadPobladoEmpresa.attr('disabled', true);

        ddlBarrioColoniaEmpresa.empty();
        ddlBarrioColoniaEmpresa.append("<option value=''>Seleccione una ciudad/poblado</option>");
        ddlBarrioColoniaEmpresa.attr('disabled', true);
    }
});


/* habilitar ddl de ciudades de la informacion laboral del cliente cuando se seleccione un municipio valido */
var idMunicipioEmpresa = 0;
$("#ddlMunicipioEmpresa").change(function () {

    $(this).parsley().validate();

    idMunicipioEmpresa = $("#ddlMunicipioEmpresa option:selected").val();// Municipio seleccionado
    var ddlCiudadPobladoEmpresa = $("#ddlCiudadPobladoEmpresa");
    var ddlBarrioColoniaEmpresa = $("#ddlBarrioColoniaEmpresa");

    if (idMunicipioEmpresa != '') {

        ddlCiudadPobladoEmpresa.empty();
        ddlCiudadPobladoEmpresa.append("<option value=''>Seleccione una opción</option>");

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarListaCiudadesPoblados",
            data: JSON.stringify({ idDepartamento: idDepartamentoEmpresa, idMunicipio: idMunicipioEmpresa }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar ciudades de este municipio');
            },
            success: function (data) {

                var listaMunicipios = data.d;

                $.each(listaMunicipios, function (i, iter) {
                    ddlCiudadPobladoEmpresa.append("<option value='" + iter.IdCiudadPoblado + "'>" + iter.NombreCiudadPoblado + "</option>"); // Agregar ciudades del municipio seleccionado
                });
                ddlCiudadPobladoEmpresa.attr('disabled', false);

                ddlBarrioColoniaEmpresa.empty();
                ddlBarrioColoniaEmpresa.append("<option value=''>Seleccione una ciudad/poblado</option>");
                ddlBarrioColoniaEmpresa.attr('disabled', true);
            }
        });
    }
    else {
        ddlCiudadPobladoEmpresa.empty();
        ddlCiudadPobladoEmpresa.append("<option value=''>Seleccione un municipio</option>");
        ddlCiudadPobladoEmpresa.attr('disabled', true);

        ddlBarrioColoniaEmpresa.empty();
        ddlBarrioColoniaEmpresa.append("<option value=''>Seleccione una ciudad/poblado</option>");
        ddlBarrioColoniaEmpresa.attr('disabled', true);
    }
});


/* habilitar ddl de barrios y colonias de la informacion laboral del cliente cuando se Seleccione una ciudad/poblado valida */
var idCiudadPobladoEmpresa = 0;
$("#ddlCiudadPobladoEmpresa").change(function () {

    $(this).parsley().validate();

    idCiudadPobladoEmpresa = $("#ddlCiudadPobladoEmpresa option:selected").val();
    var ddlBarrioColoniaEmpresa = $("#ddlBarrioColoniaEmpresa");

    if (idCiudadPobladoEmpresa != '') {

        ddlBarrioColoniaEmpresa.empty();
        ddlBarrioColoniaEmpresa.append("<option value=''>Seleccione una opción</option>");

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarListaBarriosColonias",
            data: JSON.stringify({ idDepartamento: idDepartamentoEmpresa, idMunicipio: idMunicipioEmpresa, idCiudadPoblado: idCiudadPobladoEmpresa }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar barrios de esta ciudad');
            },
            success: function (data) {

                var listaBarriosColonias = data.d;

                $.each(listaBarriosColonias, function (i, iter) {
                    ddlBarrioColoniaEmpresa.append("<option value='" + iter.IdBarrioColonia + "'>" + iter.NombreBarrioColonia + "</option>"); // Agregar barrios/colonias de la ciudad seleccionada
                });
                ddlBarrioColoniaEmpresa.attr('disabled', false);
            }
        });
    }
    else {
        ddlBarrioColoniaEmpresa.empty();
        ddlBarrioColoniaEmpresa.append("<option value=''>Seleccione una ciudad/poblado</option>");
        ddlBarrioColoniaEmpresa.attr('disabled', true);
    }
});


/* validar que se haya seleccionado un barrio de la informacion laboral del cliente valido o mostrar la validacion al usuario */
$("#ddlBarrioColoniaEmpresa").change(function () {
    $(this).parsley().validate();
});


/* validar si el estado civil seleccionado requiere información conyugal */
$("#ddlEstadoCivil").change(function () {

    var requiereInformacionConyugal = $("#ddlEstadoCivil :selected").data('requiereinformacionconyugal');

    if (requiereInformacionConyugal == false) {

        $('input.infoConyugal').attr('disabled', true);
        $('#smartwizard').smartWizard("stepState", [4], "hide");// Si no se requiere información conyugal, deshabilitar ese formulario
    }
    else if (requiereInformacionConyugal == true) {

        $('input.infoConyugal').attr('disabled', false);
        $('#smartwizard').smartWizard("stepState", [4], "show");// Si se requiere información conyugal, habilitar ese formulario
    }
});


$("select").on('change', function () {

    $(this).parsley().validate();
});


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
                CargarPrestamosOfertados(montoPmoEfectivo, "0");
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
        CargarPrestamosOfertados($("#txtValorVehiculo").val(), $('#txtPrima').val());
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
        CargarPrestamosOfertados($("#txtValorVehiculo").val(), $('#txtPrima').val());
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
        CargarPrestamosOfertados($("#txtMontoPmoEfectivo").val(), "0.00");
    } else {
        MensajeError('No se cargaron los prestamos ofertados');
    }
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

function GuardarRespaldoInformacionPrestamo() {

    var respaldoInformacionPrestamo = {

        txtRtnCliente: $("#txtRtnCliente").val(),
        txtValorGlobal: $("#txtValorGlobal").val().replace(/,/g, ''),
        txtValorPrima: $("#txtValorPrima").val().replace(/,/g, ''),
        ddlOrigen: $("#ddlOrigen :selected").val()
    }
    localStorage.setItem('RespaldoInformacionPrestamo', JSON.stringify(respaldoInformacionPrestamo));
}

function GuardarRespaldoInformacionPersonal() {

    var respaldoInformacionPersonal = {

        ddlNacionalidad: $("#ddlNacionalidad").val(),
        txtProfesion: $("#txtProfesion").val(),
        ddlEstadoCivil: $("#ddlEstadoCivil :selected").val(),
        txtFechaDeNacimiento: $("#txtFechaDeNacimiento").val(),
        txtEdadDelCliente: $("#txtEdadDelCliente").val(),
        sexoCliente: $("input[name='sexoCliente']:checked").val(),        
        txtCorreoElectronico: $("#txtCorreoElectronico").val(),
        ddlTipoDeVivienda: $("#ddlTipoDeVivienda :selected").val(),
        ddlTiempoDeResidir: $("#ddlTiempoDeResidir :selected").val()
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
        txtReferenciasDelDomicilio: $("#txtReferenciasDelDomicilio").val()
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
        txtReferenciasEmpresa: $("#txtReferenciasEmpresa").val(),
        
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

function GuardarRespaldoReferenciasPersonales() {

    localStorage.setItem('RespaldoReferenciasPersonales', JSON.stringify(listaReferenciasPersonales));
}

function RecuperarRespaldos() {

    if (localStorage.getItem('RespaldoInformacionPrestamo') != null) {

        var RespaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));
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
                    CargarPrestamosOfertados(montoPmoEfectivo, "0"); // cargar oferas de prestamos
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
                CargarPrestamosOfertados($('#txtValorVehiculo').val(), $('#txtPrima').val());
            }
            else { MensajeError('No se cargaron los préstamos sugeridos porque los valores ingresados no son válidos'); }
        }
    }

    if (localStorage.getItem('RespaldoInformacionPersonal') != null) { // recuperar informacion del paso "informacion personal"

        var respaldoInformacionPersonal = JSON.parse(localStorage.getItem('RespaldoInformacionPersonal'));

        $("#ddlNacionalidad").val(respaldoInformacionPersonal.ddlNacionalidad);
        $("#txtProfesion").val(respaldoInformacionPersonal.txtProfesion);
        $("#ddlEstadoCivil").val(respaldoInformacionPersonal.ddlEstadoCivil);
        $("#txtFechaDeNacimiento").val(respaldoInformacionPersonal.txtFechaDeNacimiento);
        $("#txtEdadDelCliente").val(respaldoInformacionPersonal.txtEdadDelCliente);
        
        $("input[name=sexoCliente][value=" + respaldoInformacionPersonal.sexoCliente + "]").prop('checked', true);
        $("#txtCorreoElectronico").val(respaldoInformacionPersonal.txtCorreoElectronico);
        $("#txtNumeroTelefono").val(respaldoInformacionPersonal.txtNumeroTelefono);

        $("#ddlTipoDeVivienda").val(respaldoInformacionPersonal.ddlTipoDeVivienda);
        $("#ddlTiempoDeResidir").val(respaldoInformacionPersonal.ddlTiempoDeResidir);
    }

    if (localStorage.getItem('RespaldoinformacionDomicilio') != null) { // recuperar informacion del paso "informacion domiciliar"
        var respaldoinformacionDomicilio = JSON.parse(localStorage.getItem('RespaldoinformacionDomicilio'));
        //$("#departamento").val(respaldoinformacionDomicilio.departamento);
        //$("#municipio").val(respaldoinformacionDomicilio.municipio);
        //$("#ciudad").val(respaldoinformacionDomicilio.ciudad);
        //$("#barrioColonia").val(respaldoinformacionDomicilio.barrioColonia);
        $("#telefonoCasa").val(respaldoinformacionDomicilio.telefonoCasa);
        $("#telefonoMovil").val(respaldoinformacionDomicilio.telefonoMovil);
        $("#direccionDetallada").val(respaldoinformacionDomicilio.direccionDetallada);
        $("#referenciaDireccionDetallada").val(respaldoinformacionDomicilio.referenciaDireccionDetallada);
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
        //$("#ddlDepartamentoEmpresa").val(respaldoInformacionLaboral.departamentoEmpresa);
        //$("#ddlMunicipioEmpresa").val(respaldoInformacionLaboral.municipioEmpresa);
        //$("#ddlCiudadPobladoEmpresa").val(respaldoInformacionLaboral.ciudadEmpresa);
        //$("#ddlBarrioColoniaEmpresa").val(respaldoInformacionLaboral.barrioColoniaEmpresa);
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

        RespaldolistaReferenciasPersonales = JSON.parse(localStorage.getItem('RespaldoReferenciasPersonales'));
        $('#datatable-buttons').DataTable().clear();
        listaReferenciasPersonales = [];
        if (RespaldolistaReferenciasPersonales.length > 0) {
            for (var i = 0; i < RespaldolistaReferenciasPersonales.length; i++) {

                $('#datatable-buttons').dataTable().fnAddData([ // agregar las referencias personales al datatabel
                    RespaldolistaReferenciasPersonales[i].fcNombreCompletoReferencia,
                    RespaldolistaReferenciasPersonales[i].fcLugarTrabajoReferencia,
                    RespaldolistaReferenciasPersonales[i].fiTiempoConocerReferencia <= 2 ? RespaldolistaReferenciasPersonales[i].fiTiempoConocerReferencia + ' años' : 'Más de 2 años',
                    RespaldolistaReferenciasPersonales[i].fcTelefonoReferencia,
                    RespaldolistaReferenciasPersonales[i].fcDescripcionParentesco,
                ]);
                cantidadReferencias += 1;
                RespaldolistaReferenciasPersonales[i].fdFechaCrea = '';
                RespaldolistaReferenciasPersonales[i].fdFechaUltimaModifica = '';
                listaReferenciasPersonales.push(RespaldolistaReferenciasPersonales[i]);
            }
        }
    }
}