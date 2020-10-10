var btnFinalizar = $('<button type="button" id="btnGuardarSolicitud"></button>').text('Finalizar').addClass('btn btn-info').css('display', 'none')
    .on('click', function () {

        debugger;

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
                fnPrima: $("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''),
                fnValorGarantia: $("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''),
                fdValorPmoSugeridoSeleccionado: $("#ddlPrestamosDisponibles option:selected").val(),
                fiPlazoPmoSeleccionado: $("#ddlPrestamosDisponibles option:selected").data('plazoseleccionado'),
                fiIDOrigen: $("#ddlOrigen option:selected").val() == null ? 1 : parseInt($("#ddlOrigen option:selected").val())
            };

            var bitacora = {
                fdEnIngresoInicio: localStorage.getItem("EnIngresoInicio")
            };

            var ClienteMaster = {
                RTNCliente: $("#txtRtnCliente").val(),
                fiNacionalidadCliente: $("#ddlNacionalidad").val(),
                fcProfesionOficioCliente: $("#txtProfesion").val(),
                fcCorreoElectronicoCliente: $("#txtCorreoElectronico").val(),                
                fcSexoCliente: $("input[name='sexoCliente']:checked").val(),
                fiIDEstadoCivil: $("#ddlEstadoCivil :selected").val(),
                fiIDVivienda: $("#ddlTipoDeVivienda :selected").val(),
                fiTiempoResidir: $("#ddlTiempoDeResidir :selected").val()
            };

            var ClienteInformacionDomicilio = {
                fiIDDepto: $("#ddlDepartamentoDomicilio :selected").val(),
                fiIDMunicipio: $("#ddlMunicipioDomicilio :selected").val(),
                fiIDBarrioColonia: $("#ddlBarrioColoniaDomicilio :selected").val(),
                fiIDCiudad: $("#ddlCiudadPobladoDomicilio :selected").val(),
                fcTelefonoCasa: $("#txtTelefonoCasa").val(),
                fcDireccionDetallada: $("#txtDireccionDetalladaDomicilio").val(),
                fcReferenciasDireccionDetallada: $("#txtReferenciasDelDomicilio").val()
            };

            var ClientesInformacionLaboral = {
                fcNombreTrabajo: $("#txtNombreDelTrabajo").val(),
                fcFechaIngreso: $("#txtFechaDeIngreso").val(),
                fcPuestoAsignado: $("#txtPuestoAsignado").val(),                
                fdTelefonoEmpresa: $("#txtTelefonoEmpresa").val(),
                fcExtensionRecursosHumanos: $("#txtExtensionRecursosHumanos").val().replace(/_/g, ''),
                fcExtensionCliente: $("#txtExtensionCliente").val().replace(/_/g, ''),                
                fiIDBarrioColonia: $("#ddlBarrioColoniaEmpresa :selected").val(),
                fiIDDepto: $("#ddlDepartamentoEmpresa :selected").val(),
                fiIDMunicipio: $("#ddlMunicipioEmpresa :selected").val(),
                fiIDCiudad: $("#ddlCiudadPobladoEmpresa :selected").val(),
                fcFuenteOtrosIngresos: $("#txtFuenteDeOtrosIngresos").val(),
                fiValorOtrosIngresosMensuales: $("#txtValorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#txtValorOtrosIngresos").val().replace(/,/g, ''),
                fcDireccionDetalladaEmpresa: $("#txtDireccionDetalladaEmpresa").val(),
                fcReferenciasDireccionDetallada: $("#txtReferenciasEmpresa").val()
            };           

            var ClientesInformacionConyugal = {};

            var requiereInformacionConyugal = $("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal');

            if (requiereInformacionConyugal == true) {

                ClientesInformacionConyugal = {
                    fcIndentidadConyugue: $("#txtIdentidadConyugue").val(),
                    fcNombreCompletoConyugue: $("#txtNombresConyugue").val(),
                    
                    fdFechaNacimientoConyugue: $("#txtFechaNacimientoConyugue").val(),
                    fcTelefonoConyugue: $("#txtTelefonoConyugue").val(),
                    fcLugarTrabajoConyugue: $("#txtLugarDeTrabajoConyuge").val(),
                    fcIngresosMensualesConyugue: $("#txtIngresosMensualesConyugue").val().replace(/,/g, '') == '' ? 0 : $("#txtIngresosMensualesConyugue").val().replace(/,/g, ''),
                    fcTelefonoTrabajoConyugue: $("#txtTelefonoTrabajoConyugue").val()
                };
            }

            ClientesReferencias = listaReferenciasPersonales;

            $.ajax({
                type: "POST",
                url: 'SolicitudesCredito_Registrar.aspx/IngresarSolicitud',
                data: JSON.stringify({ SolicitudesMaster: SolicitudesMaster, ClienteMaster: ClienteMaster, ClientesInformacionLaboral: ClientesInformacionLaboral, ClienteInformacionDomicilio: ClienteInformacionDomicilio, ClientesInformacionConyugal: ClientesInformacionConyugal, ClientesReferencias: ClientesReferencias, bitacora: bitacora }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError('No se guardó el registro, contacte al administrador');
                },
                success: function (data) {

                    debugger;

                    if (data.d.response == true) {

                        //MensajeExito(data.d.message);
                        //localStorage.clear();

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

            $('#smartwizard').smartWizard("stepState", [4], "hide");

        }
        else if ($("#ddlEstadoCivil option:selected").data('requiereinformacionconyugal') == true) {

            $('#smartwizard').smartWizard("stepState", [4], "show");

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

                var valorGlobal = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, '')).toFixed(2);
                var valorFinanciar = parseFloat($("#txtValorFinanciar").val().replace(/,/g, '') == '' ? 0 : $("#txtValorFinanciar").val().replace(/,/g, '')).toFixed(2);
                var montoOfertadoSeleccionado = parseFloat($("#ddlPrestamosDisponibles :selected").val()).toFixed(2);
                var plazoSeleccionado = parseInt($("#ddlPrestamosDisponibles option:selected").data('plazoseleccionado') == '' ? 0 : $("#ddlPrestamosDisponibles option:selected").data('plazoseleccionado'));

                if (montoOfertadoSeleccionado < valorFinanciar) {

                    state = false;
                    MensajeError('El monto del préstamo ofertado seleccionado no puede ser menor que el valor a Financiar');
                }

                if (CONSTANTES.RequierePrima == true) {

                    var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, '')).toFixed(2);

                    if (valorPrima >= valorGlobal) {

                        state = false;
                        MensajeError('El valor de la prima debe ser menor que el valor de la garantía');
                    }

                    if (CONSTANTES.PorcentajePrimaMinima != null) {

                        if (valorPrima < ((valorGlobal * CONSTANTES.PorcentajePrimaMinima) / 100)) {

                            state = false;
                            MensajeError('El porcentaje de prima mínimo es: ' + CONSTANTES.PorcentajePrimaMinima + '%. Aumente el valor de la prima.');
                        }
                    }
                } /* if requiere prima */

                if (CONSTANTES.MontoFinanciarMinimo != null) {

                    if (valorFinanciar < CONSTANTES.MontoFinanciarMinimo) {

                        state = false;
                        MensajeError('El monto mínimo a financiar es ' + CONSTANTES.MontoFinanciarMinimo + '.');
                    }
                }

                if (CONSTANTES.MontoFinanciarMaximo != null) {

                    if (valorFinanciar > CONSTANTES.MontoFinanciarMaximo) {

                        state = false;
                        MensajeError('El monto máximo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMaximo + '.');
                    }
                }

                if (CONSTANTES.PlazoMinimo != null) {

                    if (plazoSeleccionado < CONSTANTES.PlazoMinimo) {

                        state = false;
                        MensajeError('El plazo mínimo a para este producto es ' + CONSTANTES.PlazoMinimo + '.');
                    }
                }

                if (CONSTANTES.PlazoMaximo != null) {

                    if (plazoSeleccionado > CONSTANTES.PlazoMaximo) {

                        state = false;
                        MensajeError('El plazo máximo a para este producto es ' + CONSTANTES.PlazoMaximo + '.');
                    }
                }

                if (valorFinanciar > CONSTANTES.PrestamoMaximo_Monto) {
                    state = false;
                    MensajeError('El monto máximo a para este cliente es ' + CONSTANTES.PlazoMaximo + '.');
                }

                if (plazoSeleccionado > CONSTANTES.PrestamoMaximo_Plazo) {
                    state = false;
                    MensajeError('El plazo máximo a para este cliente es ' + CONSTANTES.PrestamoMaximo_Plazo + '.');
                }

                return state;
            }

            /* Validar pestaña de la informacion personal del cliente */
            if (stepNumber == 1) {
                debugger;

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
                    else {
                        MensajeError('La cantidad mínima de referencias es 4, entre ellos 2 familiares');
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

    debugger;
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
    if (localStorage.getItem("EnIngresoInicio") == null) {

        var dt = new Date(CONSTANTES.HoraAlCargar);
        localStorage.setItem("EnIngresoInicio", JSON.stringify(dt));
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
    debugger;
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

            debugger;

            var ListaPrestamosOfertados = data.d;

            var ddlPrestamosDisponibles = $("#ddlPrestamosDisponibles");
            ddlPrestamosDisponibles.empty();
            ddlPrestamosDisponibles.append("<option selected value=''>Seleccione una opción</option>");

            for (var i = 0; i < ListaPrestamosOfertados.length; i++) {

                ddlPrestamosDisponibles.append("<option value='" + ListaPrestamosOfertados[i].MontoOfertado + "' data-plazoseleccionado='" + ListaPrestamosOfertados[i].Plazo + "'>" + 'Producto: ' + ListaPrestamosOfertados[i].Producto + ' | Monto ofertado: ' + ListaPrestamosOfertados[i].MontoOfertado + ' | Plazo ' + ListaPrestamosOfertados[i].TipoPlazo + ': ' + ListaPrestamosOfertados[i].Plazo + ' | Cuota ' + ListaPrestamosOfertados[i].TipoPlazo + ': ' + ListaPrestamosOfertados[i].Cuota + "</option>");
            }
        }
    });
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


$('#txtValorGlobal,#txtValorPrima').blur(function () {

    debugger;
    var valorGlobal = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, '')).toFixed(2);    
    var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, '')).toFixed(2);
    var valorFinanciar = valorGlobal - valorPrima;
    $("#txtValorFinanciar").val(valorFinanciar);
    var state = true;

    if (CONSTANTES.RequierePrima == true) {

        if (valorPrima >= valorGlobal) {

            state = false;
            MensajeError('El valor de la prima debe ser menor que el valor de la garantía');
        }

        if (CONSTANTES.PorcentajePrimaMinima != null) {

            if (valorPrima < ((valorGlobal * CONSTANTES.PorcentajePrimaMinima) / 100)) {

                state = false;
                MensajeError('El porcentaje de prima mínimo es: ' + CONSTANTES.PorcentajePrimaMinima + '%. Aumente el valor de la prima.');
            }
        }
    } /* if requiere prima */

    if (CONSTANTES.MontoFinanciarMinimo != null) {

        if (valorFinanciar < CONSTANTES.MontoFinanciarMinimo) {

            state = false;
            MensajeError('El monto mínimo a financiar es ' + CONSTANTES.MontoFinanciarMinimo + '.');
        }
    }

    if (CONSTANTES.MontoFinanciarMaximo != null) {

        if (valorFinanciar > CONSTANTES.MontoFinanciarMaximo) {

            state = false;
            MensajeError('El monto máximo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMaximo + '.');
        }
    }

    if (valorFinanciar > CONSTANTES.PrestamoMaximo_Monto) {
        state = false;
        MensajeError('El monto máximo a para este cliente es ' + CONSTANTES.PlazoMaximo + '.');
    }

    if (state == true) {
        CargarPrestamosOfertados(valorGlobal.toString(), valorPrima.toString());
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

function GuardarRespaldoReferenciasPersonales() {

    localStorage.setItem('RespaldoReferenciasPersonales', JSON.stringify(listaReferenciasPersonales));
}

function RecuperarRespaldos() {

    $(".buscardorddl").select2("destroy");

    /* Recuperar información de pestaña información del préstamo */
    //if (localStorage.getItem('RespaldoInformacionPrestamo') != null) {

    //    var RespaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));
    //    $("#txtPrima").val(RespaldoInformacionPrestamo.prima);
    //    $("#txtValorVehiculo").val(RespaldoInformacionPrestamo.valorVehiculo);
    //    $("#txtValorFinanciar").val(RespaldoInformacionPrestamo.valorVehiculo - RespaldoInformacionPrestamo.prima);
    //    $("#txtMontoPmoEfectivo").val(RespaldoInformacionPrestamo.montoPmoEfectivo);
    //    var prestamoSeleccionadoMax = parseFloat($("#pmoSugeridoSeleccionado :selected").val()).toFixed(2);
    //    if ($("#tipoPrestamo").data('cod') == '101') { // si el prestamo requerido es en efectivo, no se requiere prima y el plazo es quincenal

    //        $(".divPrestamoVehiculo").css('display', 'none');
    //        $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar,#ddlOrigen").prop('disabled', true);
    //        $("#txtPrima,#txtValorVehiculo,#txtValorFinanciar").val('');
    //        $("#ddlOrigen,#titleOrigen").css('display', 'none');

    //        $('#txtMontoPmoEfectivo').css('display', '');
    //        $('#txtMontoPmoEfectivo').prop('disabled', false);
    //        $(".divPrestamoEfectivo").css('display', '');

    //        var montoPmoEfectivo = $('#txtMontoPmoEfectivo').val().replace(/,/g, '');

    //        if (montoPmoEfectivo != '') {
    //            if (parseFloat(montoPmoEfectivo) <= parseFloat(prestamoSeleccionadoMax)) {
    //                CargarPrestamosOfertados(montoPmoEfectivo, "0"); // cargar oferas de prestamos
    //            }
    //            else { MensajeError('No se cargaron los préstamos sugeridos porque no se ha introducido un monto válido'); }

    //        } else { MensajeError('No se cargaron los préstamos sugeridos porque no se ha introducido un monto válido'); }
    //    }
    //    else { // de lo contrario, si se requiere prima y el plazo es mensual

    //        $("#ddlOrigen").prop('disabled', false);
    //        $("#ddlOrigen,#titleOrigen").css('display', '');
    //        cargarOrigenes($("#tipoPrestamo").data('cod')); // cargar origenes del producto actual
    //        $("#lblPlazoPMO").text('Plazo');
    //        $(".divPrestamoVehiculo").css('display', '');
    //        $("#txtPrima,#txtValorVehiculo").prop('disabled', false);
    //        var prima = parseFloat($('#txtPrima').val().replace(/,/g, '') == '' ? 0 : $('#txtPrima').val().replace(/,/g, ''));
    //        var valorVehiculo = parseFloat($('#txtValorVehiculo').val().replace(/,/g, '') == '' ? 0 : $('#txtValorVehiculo').val().replace(/,/g, ''));
    //        var totalAFinanciar = valorVehiculo - prima;
    //        $('#txtValorFinanciar').val(totalAFinanciar);
    //        if (totalAFinanciar > prestamoSeleccionadoMax || valorVehiculo <= 0) {
    //            $('#txtValorFinanciar').addClass('parsley-error');
    //            $('#txtValorFinanciar').addClass('text-danger');
    //            $('#error-valorFinanciar').css('display', '');
    //        } else {
    //            $('#txtValorFinanciar').removeClass('parsley-error');
    //            $('#txtValorFinanciar').removeClass('text-danger');
    //            $('#error-valorFinanciar').css('display', 'none');
    //        }
    //        var estadoPrimaMinima = true;
    //        if ($("#tipoPrestamo").data('cod') == '201') {

    //            if (prima < (valorVehiculo * 0.10)) { estadoPrimaMinima = false; }
    //        }
    //        if (totalAFinanciar <= prestamoSeleccionadoMax && valorVehiculo >= 0 && valorVehiculo > prima && estadoPrimaMinima == true) {
    //            CargarPrestamosOfertados($('#txtValorVehiculo').val(), $('#txtPrima').val());
    //        }
    //        else { MensajeError('No se cargaron los préstamos sugeridos porque los valores ingresados no son válidos'); }
    //    }
    //}

    /* Recuperar respaldo de pestaña de informacion personal */
    if (localStorage.getItem('RespaldoInformacionPersonal') != null) {

        var respaldoInformacionPersonal = JSON.parse(localStorage.getItem('RespaldoInformacionPersonal'));

        $("#ddlNacionalidad").val(respaldoInformacionPersonal.ddlNacionalidad);
        $("#txtProfesion").val(respaldoInformacionPersonal.txtProfesion);
        $("#ddlEstadoCivil").val(respaldoInformacionPersonal.ddlEstadoCivil);
        $("#txtFechaDeNacimiento").val(respaldoInformacionPersonal.txtFechaDeNacimiento);
        $("#txtEdadDelCliente").val(respaldoInformacionPersonal.txtEdadDelCliente);
        $("input[name=sexoCliente][value=" + respaldoInformacionPersonal.sexoCliente + "]").prop('checked', true);
        $("#txtCorreoElectronico").val(respaldoInformacionPersonal.txtCorreoElectronico);
        $("#ddlTipoDeVivienda").val(respaldoInformacionPersonal.ddlTipoDeVivienda);
        $("#ddlTiempoDeResidir").val(respaldoInformacionPersonal.ddlTiempoDeResidir);
    }

    /* Recuperar resplado de pestaña de informacion domiciliar */
    if (localStorage.getItem('RespaldoinformacionDomicilio') != null) {

        var respaldoinformacionDomicilio = JSON.parse(localStorage.getItem('RespaldoinformacionDomicilio'));

        $("#ddlDepartamentoDomicilio").val(respaldoinformacionDomicilio.ddlDepartamentoDomicilio);
        CargarMunicipios(respaldoinformacionDomicilio.ddlDepartamentoDomicilio, 'Domicilio', false, respaldoinformacionDomicilio.ddlMunicipioDomicilio);
        CargarCiudadesPoblados(respaldoinformacionDomicilio.ddlDepartamentoDomicilio, respaldoinformacionDomicilio.ddlMunicipioDomicilio, 'Domicilio', false, respaldoinformacionDomicilio.ddlCiudadPobladoDomicilio);
        CargarBarriosColonias(respaldoinformacionDomicilio.ddlDepartamentoDomicilio, respaldoinformacionDomicilio.ddlMunicipioDomicilio, respaldoinformacionDomicilio.ddlCiudadPobladoDomicilio, 'Domicilio', respaldoinformacionDomicilio.ddlBarrioColoniaDomicilio);
        $("#txtTelefonoCasa").val(respaldoinformacionDomicilio.txtTelefonoCasa);
        $("#txtDireccionDetalladaDomicilio").val(respaldoinformacionDomicilio.txtDireccionDetalladaDomicilio);
        $("#txtReferenciasDelDomicilio").val(respaldoinformacionDomicilio.txtReferenciasDelDomicilio);
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
                    'data-nombreReferencia="' + referencia.nombreReferencia + '" data-telefonoReferencia="' + referencia.telefonoReferencia + '" data-lugarTrabajoReferencia="' + referencia.lugarTrabajoReferencia + '"' +
                    'data-tiempoDeConocerReferencia="' + referencia.tiempoDeConocerReferencia + '" data-tiempoDeConocerReferenciaDescripcion="' + referencia.tiempoDeConocerReferenciaDescripcion + '" data-parentescoReferencia="' + referencia.parentescoReferencia + '" data-parentescoReferenciaDescripcion="' + referencia.parentescoReferenciaDescripcion + '"' +
                    'class="btn btn-sm btn-danger">Quitar</button > ';

                /* Agregar referencia a la tabla de referencias personales */
                var row = '<tr><td>' + referencia.nombreReferencia + '</td><td>' + referencia.telefonoReferencia + '</td><td>' + referencia.lugarTrabajoReferencia + '</td><td>' + referencia.tiempoDeConocerReferenciaDescripcion + '</td><td>' + referencia.parentescoReferenciaDescripcion + '</td><td>' + btnQuitarReferencia + '</td></tr>';

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
}