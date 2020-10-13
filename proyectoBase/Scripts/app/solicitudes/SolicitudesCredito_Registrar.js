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

        if (cantidadReferencias < 4) {
            if (modelStateInformacionPrestamo == true && modelStateInformacionPersonal == true && modelStateInformacionDomicilio == true && modelStateInformacionLaboral == true && modelStateInformacionConyugal == true) {
                iziToast.warning({
                    title: 'Atención',
                    message: 'Se requieren mínimo 4 referencias personales. Entre ellas 2 familiares.'
                });
            }
        }

        if (modelStateInformacionPrestamo == true && modelStateInformacionPersonal == true && modelStateInformacionDomicilio == true && modelStateInformacionLaboral == true && modelStateInformacionConyugal == true && cantidadReferencias >= 4) {

            var solicitud = {
                IdCliente: CONSTANTES.IdCliente,
                ValorPrima: $("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''),
                ValorGlobal: $("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''),
                ValorSeleccionado: $("#ddlPrestamosDisponibles option:selected").val(),
                PlazoSeleccionado: $("#ddlPrestamosDisponibles option:selected").data('plazoseleccionado'),
                IdOrigen: $("#ddlOrigen option:selected").val() == null ? 1 : parseInt($("#ddlOrigen option:selected").val()),
                EnIngresoInicio: ConvertirFechaJavaScriptAFechaCsharp(localStorage.getItem("EnIngresoInicio"))
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

            $.ajax({
                type: "POST",
                url: 'SolicitudesCredito_Registrar.aspx/IngresarSolicitud',
                data: JSON.stringify({ solicitud: solicitud, cliente: cliente, precalificado: PRECALIFICADO, esClienteNuevo: CONSTANTES.EsClienteNuevo, dataCrypt: window.location.href }),
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

                var valorGlobal = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''));
                var valorFinanciar = parseFloat($("#txtValorFinanciar").val().replace(/,/g, '') == '' ? 0 : $("#txtValorFinanciar").val().replace(/,/g, ''));
                var montoOfertadoSeleccionado = parseFloat($("#ddlPrestamosDisponibles :selected").val());
                var plazoSeleccionado = parseInt($("#ddlPrestamosDisponibles option:selected").data('plazoseleccionado') == '' ? 0 : $("#ddlPrestamosDisponibles option:selected").data('plazoseleccionado'));

                if (montoOfertadoSeleccionado < valorFinanciar) {

                    state = false;
                    MensajeError('El monto del préstamo ofertado seleccionado no puede ser menor que el valor a Financiar');
                }

                if (montoOfertadoSeleccionado > valorFinanciar) {

                    state = false;
                    MensajeError('El monto del préstamo ofertado seleccionado no puede ser mayor que el valor a Financiar');
                }

                if (CONSTANTES.RequierePrima == true) {

                    var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''));

                    if (valorPrima >= valorGlobal) {

                        state = false;
                        MensajeError('El valor de la prima debe ser menor que el valor de la garantía');
                    }

                    if (CONSTANTES.PorcentajePrimaMinima != null) {

                        if (valorPrima < ((valorGlobal * CONSTANTES.PorcentajePrimaMinima) / 100)) {

                            state = false;
                            MensajeError('El porcentaje de prima mínimo para este producto es: ' + CONSTANTES.PorcentajePrimaMinima + '%. Aumente el valor de la prima.');
                        }
                    }
                } /* if requiere prima */

                if (CONSTANTES.MontoFinanciarMinimo != null) {

                    if (valorFinanciar < CONSTANTES.MontoFinanciarMinimo) {

                        state = false;
                        MensajeError('El monto mínimo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMinimo + '.');
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
                        MensajeError('El plazo mínimo a financiar para este producto es ' + CONSTANTES.PlazoMinimo + '.');
                    }
                }

                if (CONSTANTES.PlazoMaximo != null) {

                    if (plazoSeleccionado > CONSTANTES.PlazoMaximo) {

                        state = false;
                        MensajeError('El plazo máximo a financiar para este producto es ' + CONSTANTES.PlazoMaximo + '.');
                    }
                }

                if (valorFinanciar > CONSTANTES.PrestamoMaximo_Monto) {
                    state = false;
                    MensajeError('El monto máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Monto + '.');
                }

                //if (plazoSeleccionado > CONSTANTES.PrestamoMaximo_Plazo) {
                //    state = false;
                //    MensajeError('El plazo máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Plazo + '.');
                //}

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
        var LugarTrabajoReferencia = $("#txtLugarTrabajoReferencia").val();
        var IdTiempoConocerReferencia = $("#ddlTiempoDeConocerReferencia :selected").val();
        var tiempoDeConocerReferenciaDescripcion = $("#ddlTiempoDeConocerReferencia :selected").text();
        var IdParentescoReferencia = $("#ddlParentescos :selected").val();
        var parentescoReferenciaDescripcion = $("#ddlParentescos :selected").text();
        var btnQuitarReferencia = '<button type="button" id="btnQuitarReferenciaPersonal" ' +
            'data-nombreReferencia="' + NombreCompletoReferencia + '" data-telefonoReferencia="' + TelefonoReferencia + '" data-lugarTrabajoReferencia="' + LugarTrabajoReferencia + '"' +
            'data-tiempoDeConocerReferencia="' + IdTiempoConocerReferencia + '" data-tiempoDeConocerReferenciaDescripcion="' + tiempoDeConocerReferenciaDescripcion + '" data-parentescoReferencia="' + IdParentescoReferencia + '" data-parentescoReferenciaDescripcion="' + parentescoReferenciaDescripcion + '"' +
            'class="btn btn-sm btn-danger" > Quitar</button > ';

        /* Agregar referencia a la tabla de referencias personales */
        var row = '<tr><td>' + NombreCompletoReferencia + '</td><td>' + TelefonoReferencia + '</td><td>' + LugarTrabajoReferencia + '</td><td>' + tiempoDeConocerReferenciaDescripcion + '</td><td>' + parentescoReferenciaDescripcion + '</td><td class="text-center">' + btnQuitarReferencia + '</td></tr>';
        $("#tblReferenciasPersonales tbody").append(row);

        $("#modalAgregarReferenciaPersonal").modal('hide');

        cantidadReferencias++;

        /* Objeto referencia */
        var referencia = {
            NombreCompletoReferencia: NombreCompletoReferencia,
            TelefonoReferencia: TelefonoReferencia,
            LugarTrabajoReferencia: LugarTrabajoReferencia,
            IdTiempoConocerReferencia: IdTiempoConocerReferencia,
            tiempoDeConocerReferenciaDescripcion: tiempoDeConocerReferenciaDescripcion,
            IdParentescoReferencia: IdParentescoReferencia,
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
        NombreCompletoReferencia: $(this).data('nombrereferencia'),
        TelefonoReferencia: $(this).data('telefonoreferencia'),
        LugarTrabajoReferencia: $(this).data('lugartrabajoreferencia'),
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
                LugarTrabajoReferencia: listaReferenciasPersonales[i].LugarTrabajoReferencia,
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
        data: JSON.stringify({ valorProducto: valorProducto.replace(/,/g, ''), valorPrima: valorPrima.replace(/,/g, ''), dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeError('No se pudo cargar los préstamos sugeridos, contacte al administrador');
            $("#ddlPrestamosDisponibles").empty();
        },
        success: function (data) {

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

    var valorGlobal = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''));
    var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''));
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
                MensajeError('El porcentaje de prima mínimo para este producto es: ' + CONSTANTES.PorcentajePrimaMinima + '%. Aumente el valor de la prima.');
            }
        }
    } /* if requiere prima */

    if (CONSTANTES.MontoFinanciarMinimo != null) {

        if (valorFinanciar < CONSTANTES.MontoFinanciarMinimo) {

            state = false;
            MensajeError('El monto mínimo a financiar para este producto es ' + CONSTANTES.MontoFinanciarMinimo + '.');
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
        MensajeError('El monto máximo a financiar para este cliente es ' + CONSTANTES.PrestamoMaximo_Monto + '.');
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
        txtValorGlobal: $("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''),
        txtValorPrima: $("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''),
        ddlOrigen: $("#ddlOrigen :selected").val()
    }
    localStorage.setItem('RespaldoInformacionPrestamo', JSON.stringify(respaldoInformacionPrestamo));
}

function GuardarRespaldoInformacionPersonal() {

    var respaldoInformacionPersonal = {

        ddlNacionalidad: $("#ddlNacionalidad").val(),
        txtProfesion: $("#txtProfesion").val(),
        ddlEstadoCivil: $("#ddlEstadoCivil :selected").val(),
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
    if (localStorage.getItem('RespaldoInformacionPrestamo') != null) {

        var RespaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));

        $("#txtRtnCliente").val(RespaldoInformacionPrestamo.txtRtnCliente);
        $("#ddlOrigen").val(RespaldoInformacionPrestamo.ddlOrigen);
        var valorGlobal = parseFloat(RespaldoInformacionPrestamo.txtValorGlobal).toFixed(2);
        var valorPrima = parseFloat(RespaldoInformacionPrestamo.txtValorPrima).toFixed(2);
        var valorFinanciar = valorGlobal - valorPrima;
        $("#txtValorGlobal").val(valorGlobal);
        $("#txtValorPrima").val(valorPrima);
        $("#txtValorFinanciar").val(valorFinanciar);
        CargarPrestamosOfertados(RespaldoInformacionPrestamo.txtValorGlobal.toString(), RespaldoInformacionPrestamo.txtValorPrima.toString());
    }

    /* Recuperar respaldo de pestaña de informacion personal */
    if (localStorage.getItem('RespaldoInformacionPersonal') != null) {

        var respaldoInformacionPersonal = JSON.parse(localStorage.getItem('RespaldoInformacionPersonal'));

        $("#ddlNacionalidad").val(respaldoInformacionPersonal.ddlNacionalidad);
        $("#txtProfesion").val(respaldoInformacionPersonal.txtProfesion);
        $("#ddlEstadoCivil").val(respaldoInformacionPersonal.ddlEstadoCivil);
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
                    'data-nombreReferencia="' + referencia.NombreCompletoReferencia + '" data-telefonoReferencia="' + referencia.TelefonoReferencia + '" data-lugarTrabajoReferencia="' + referencia.LugarTrabajoReferencia + '"' +
                    'data-tiempoDeConocerReferencia="' + referencia.IdTiempoConocerReferencia + '" data-tiempoDeConocerReferenciaDescripcion="' + referencia.tiempoDeConocerReferenciaDescripcion + '" data-parentescoReferencia="' + referencia.IdParentescoReferencia + '" data-parentescoReferenciaDescripcion="' + referencia.parentescoReferenciaDescripcion + '"' +
                    'class="btn btn-sm btn-danger">Quitar</button > ';

                /* Agregar referencia a la tabla de referencias personales */
                var row = '<tr><td>' + referencia.NombreCompletoReferencia + '</td><td>' + referencia.TelefonoReferencia + '</td><td>' + referencia.LugarTrabajoReferencia + '</td><td>' + referencia.tiempoDeConocerReferenciaDescripcion + '</td><td>' + referencia.parentescoReferenciaDescripcion + '</td><td class="text-center">' + btnQuitarReferencia + '</td></tr>';

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

function ConvertirFechaJavaScriptAFechaCsharp(fecha) {
    var date = new Date(fecha);
    var day = date.getDate();
    var month = date.getMonth() + 1;
    var year = date.getFullYear();
    var hour = date.getHours();
    var minute = date.getMinutes();
    var second = date.getSeconds();

    return day + "/" + month + "/" + year + " " + hour + ':' + minute + ':' + second;
}