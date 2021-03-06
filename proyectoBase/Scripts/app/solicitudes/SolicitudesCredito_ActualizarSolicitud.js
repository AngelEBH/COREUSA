var objSeccion = '';
COTIZADOR = null;


$('#smartwizard').smartWizard({
    selected: 0,
    theme: 'default',
    transitionEffect: 'fade',
    showStepURLhash: false,
    autoAdjustHeight: false,
    toolbarSettings: {
        toolbarPosition: 'bottom',
        toolbarButtonPosition: 'end'
    },
    lang: {
        next: 'Siguiente',
        previous: 'Anterior'
    }
});

$(document).ready(function () {

    /* Inicializar mascaras de entrada */
    $(".mascara-cantidad").inputmask("decimal", {
        alias: 'numeric',
        groupSeparator: ',',
        digits: 2,
        integerDigits: 11,
        digitsOptional: false,
        placeholder: '0',
        radixPoint: ".",
        autoGroup: true,
        min: 0.00
    });
    $(".mascara-telefono").inputmask("9999-9999");
    $(".mascara-extension").inputmask("999999");
    $(".mascara-identidad").inputmask("9999999999999");
    $(".mascara-rtn").inputmask("99999999999999");
    $(".mascara-enteros").inputmask("decimal", {
        alias: "numeric",
        groupSeparator: ",",
        digits: 0,
        integerDigits: 4,
        digitsOptional: false,
        placeholder: "0",
        radixPoint: ".",
        autoGroup: true,
        min: 0.0,
    });
    $(".mascara-plazos").inputmask("decimal", {
        alias: "numeric",
        groupSeparator: ",",
        digits: 0,
        integerDigits: 2,
        digitsOptional: false,
        placeholder: "0",
        radixPoint: ".",
        autoGroup: true,
        min: 0.0,
    });

    /* Cuando se muestre un step */
    
    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {

        /* si es el primer paso, deshabilitar el boton "anterior" */
        if (stepPosition === 'first') {
            $("#prev-btn").addClass('disabled').css('display', 'none');
        }
        else if (stepPosition === 'final') { /* si es el ultimo paso, deshabilitar el boton siguiente */
            $("#next-btn").addClass('disabled');
        }
        else { /* si no es ninguna de las anteriores, habilitar todos los botones */
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
        }
    });

    /* Habilitar boton siguiente al reiniciar steps */
    $("#smartwizard").on("endReset", function () {
        $("#next-btn").removeClass('disabled');
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
});

/* Cargar los inputs de los documentos requeridos */
function CargarDocumentosRequeridos() {
    console.log("entrabdo");
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarDocumentosRequeridos",
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

            divDocumentacion.empty();

            $.each(data.d, function (i, iter) {

                var IdInput = 'Documento' + iter.IdTipoDocumento;

                divDocumentacion.append("<div class='col-sm-2 mt-3 pr-1 pl-1'>" +
                    "<label class='form-label mb-1'>" + iter.DescripcionTipoDocumento + "</label>" +
                    "<form action='SolicitudesCredito_ActualizarSolicitud.aspx?type=upload&doc=" + iter.IdTipoDocumento + "' method='post' enctype='multipart/form-data'>" +
                    "<input id='" + IdInput + "' type='file' name='files' data-tipo='" + iter.IdTipoDocumento + "' />" +
                    "</form>" +
                    "</div");

                $('#' + IdInput + '').fileuploader({
                    inputNameBrackets: false,
                    changeInput: Input,
                    theme: 'dragdrop',
                    limit: iter.CantidadMaximaDoucmentos, // Limite de archivos a subir
                    maxSize: 500, // Peso máximo de todos los archivos seleccionado en megas (MB)
                    fileMaxSize: 10, // Peso máximo de un archivo
                    extensions: ['jpg', 'png'],// Extensiones/formatos permitidos
                    upload: {
                        url: 'SolicitudesCredito_ActualizarSolicitud.aspx?type=upload&doc=' + iter.IdTipoDocumento,
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
                        $.post('SolicitudesCredito_ActualizarSolicitud.aspx?type=remove', { file: item.name });
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


/* Validar listas desplegables */
$("select").on('change', function () {
    $(this).parsley().validate();
});
// 
$("#ddlBarrioColoniaDomicilio").change(function () {

    $(this).parsley().validate();

    var DetalleCodigoPostal = $("#ddlBarrioColoniaDomicilio option:selected").text(); 
    $("#txtReferenciasDelDomicilio").val(DetalleCodigoPostal.trim());

});


/* Manejo de referencias personales */
$("#btnAgregarReferencia").click(function () {

    $("#txtNombreReferencia,#txtTelefonoReferencia,#ddlTiempoDeConocerReferencia, #ddlParentescos, #txtLugarTrabajoReferencia").val('');
    $('#modalAgregarReferenciaPersonal').parsley().reset();
    $("#modalAgregarReferenciaPersonal").modal();
});

$("#btnAgregarReferenciaConfirmar").click(function () {

    /* Validar formulario de agregar referencia personal */
    if ($('#frmSolicitud').parsley().isValid({ group: 'referenciasPersonales' })) {

        let NombreCompletoReferencia = $("#txtNombreReferencia").val();
        let TelefonoReferencia = $("#txtTelefonoReferencia").val();
        let LugarTrabajoReferencia = $("#txtLugarTrabajoReferencia").val();
        let IdTiempoConocerReferencia = $("#ddlTiempoDeConocerReferencia :selected").val();
        let IdParentescoReferencia = $("#ddlParentescos :selected").val();

        /* Objeto referencia */
        var referenciaPersonal = {
            IdCliente: ID_CLIENTE,
            IdSolicitud: ID_SOLICITUD,
            NombreCompleto: NombreCompletoReferencia,
            TelefonoReferencia: TelefonoReferencia,
            LugarTrabajo: LugarTrabajoReferencia,
            IdTiempoDeConocer: IdTiempoConocerReferencia,
            IdParentescoReferencia: IdParentescoReferencia,
        }

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ActualizarSolicitud.aspx/RegistrarReferenciaPersonal",
            data: JSON.stringify({ referenciaPersonal: referenciaPersonal, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo agregar la referencia personal, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La referencia personal se agregó correctamente.');
                    CargarReferenciasPersonales();
                }
                else {
                    MensajeError("No se pudo agregar la referencia personal, contacte al administrador.");
                }

                $("#modalAgregarReferenciaPersonal").modal('hide');
            }
        });
    }
    else {
        $('#frmSolicitud').parsley().validate({ group: 'referenciasPersonales', force: true });
    }

});

/* Eliminar referencia personal */
var idReferenciaPersonalSeleccionada = 0;
$(document).on('click', 'button#btnEliminarReferencia', function () {

    $("#modalReferenciasPersonales").modal('hide');

    idReferenciaPersonalSeleccionada = $(this).data('id');

    $("#modalEliminarReferenciaPersonal").modal();
});

$("#btnEliminarReferenciaPersonalConfirmar").click(function (e) {

    if (idReferenciaPersonalSeleccionada != 0) {

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ActualizarSolicitud.aspx/EliminarReferenciaPersonal",
            data: JSON.stringify({ idReferenciaPersonal: idReferenciaPersonalSeleccionada, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo eliminar la referencia personal, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La referencia personal fue eliminada correctamente.');
                    CargarReferenciasPersonales();
                }
                else {
                    MensajeError("No se pudo eliminar la referencia personal, contacte al administrador.");
                }
                $("#modalEliminarReferenciaPersonal").modal('hide');
            }
        });
    }
    else {
        MensajeAdvertencia('Primero debes seleccionar una referencia personal.');
    }
});


/* Actualizar referencia personal */
$(document).on('click', 'button#btnEditarReferencia', function () {

    $("#modalReferenciasPersonales").modal('hide');

    idReferenciaPersonalSeleccionada = $(this).data('id');

    $("#txtNombreReferenciaPersonal_Editar").val($(this).data('nombre'));
    $("#txtTelefonoReferenciaPersonal_Editar").val($(this).data('telefono'));
    $("#ddlTiempoDeConocerReferencia_Editar").val($(this).data('idtiempodeconocer'));
    $("#ddlParentescos_Editar").val($(this).data('idparentesco'));
    $("#txtLugarDeTrabajoReferencia_Editar").val($(this).data('trabajo'));

    $("#modalEditarReferenciaPersonal").modal();
});

$("#btnEditarReferenciaConfirmar").click(function (e) {

    /* Validar formulario de agregar referencia personal */
    if ($('#frmSolicitud').parsley().isValid({ group: 'referenciasPersonalesEditar' })) {

        let NombreCompletoReferencia = $("#txtNombreReferenciaPersonal_Editar").val();
        let TelefonoReferencia = $("#txtTelefonoReferenciaPersonal_Editar").val();
        let LugarTrabajoReferencia = $("#txtLugarDeTrabajoReferencia_Editar").val();
        let IdTiempoConocerReferencia = $("#ddlTiempoDeConocerReferencia_Editar :selected").val();
        let IdParentescoReferencia = $("#ddlParentescos_Editar :selected").val();

        /* Objeto referencia */
        var referenciaPersonal = {
            IdReferencia: idReferenciaPersonalSeleccionada,
            NombreCompleto: NombreCompletoReferencia,
            TelefonoReferencia: TelefonoReferencia,
            LugarTrabajo: LugarTrabajoReferencia,
            IdTiempoDeConocer: IdTiempoConocerReferencia,
            IdParentescoReferencia: IdParentescoReferencia
        }

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ActualizarSolicitud.aspx/ActualizarReferenciaPersonal",
            data: JSON.stringify({ referenciaPersonal: referenciaPersonal, dataCrypt: window.location.href }),
            contentType: "application/json; charset=utf-8",
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError("No se pudo editar la referencia personal, contacte al administrador.");
            },
            success: function (data) {

                if (data.d == true) {

                    MensajeExito('La referencia personal se editó correctamente.');
                    CargarReferenciasPersonales();
                }
                else {
                    MensajeError("No se pudo editar la referencia personal, contacte al administrador.");
                }

                $("#modalEditarReferenciaPersonal").modal('hide');
            }
        });
    }
    else {
        $('#frmSolicitud').parsley().validate({ group: 'referenciasPersonalesEditar', force: true });
    }
});

/* Abrir modal confimar finalizacion de condicionamiento */
var idSolicitudCondicion = 0;
var idTipoDeCondicion = 0;
var objSeccion = {};
$(document).on('click', 'button#btnFinalizarCondicion', function () {
    debugger;
    idSolicitudCondicion = $(this).data('id');
    idTipoDeCondicion = $(this).data('idtipocondicion');
    objSeccion = {};

    $('.modal').modal('hide');
    $("#modalFinalizarCondicion_Confirmar").modal();

 });

/* Finalizar condicionamientos */
$("#btnFinalizarCondicion_Confirmar").click(function () {

    var listaCondicionesDeDocumentacion = [1, 2, 3, 4, 5, 6];

    var cotizador = {};

    /* Documentacion */
    if (jQuery.inArray(idTipoDeCondicion, listaCondicionesDeDocumentacion) >= 0) {

        objSeccion = {};

    }

    /* Referencias personales */
    else if (idTipoDeCondicion == '8' || idTipoDeCondicion == '14') {

        objSeccion = {};
    }
    /* Informacion de la solicitud */
    else if (idTipoDeCondicion == '9') {
        debugger;
        if ($('#frmSolicitud').parsley().isValid({ group: 'informacionPrestamo' })) {

            objSeccion = {
                IdSolicitud: ID_SOLICITUD,
                IdCliente: ID_CLIENTE,
                IdProducto: ID_PRODUCTO,
                ValorSeleccionado: $("#txtValorDePrestamo").val().replace(/,/g, '') == '' ? 0 : $("#txtValorDePrestamo").val().replace(/,/g, ''),
                PlazoSeleccionado: $("#txtPlazoSeleccionado").val().replace(/,/g, '') == '' ? 0 : $("#txtPlazoSeleccionado").val().replace(/,/g, ''),
                ValorGarantia: $("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''),
                ValorPrima: $("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''),
                CuotaDelPrestamo: $("#txtValorCuota").val().replace(/,/g, ''),
                TotalFinanciadoConIntereses: $("#txtValorFinanciar").val().replace(/,/g, ''),
                TasaInteresAnual: $("#txtTasaDeInteresAnual").val().replace(/,/g, ''),

            };

            cotizador = COTIZADOR;
        }
        else {
            $('#frmSolicitud').parsley().validate({ group: 'informacionPrestamo', force: true });
            $("#modalFinalizarCondicion_InfoSolicitud").modal('hide');
            return false;
        }
    }

    /* Informacion personal */

    else if (idTipoDeCondicion == '10') {
       
        if ($('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' })) {

            objSeccion = {
                IdCliente: ID_CLIENTE,
                IdTipoCliente: $("#ddlTipoDeCliente :selected").val(),
                IdentidadCliente: $("#txtIdentidadCliente").val(),
                RtnCliente: $("#txtIdentidadCliente").val(),
                PrimerNombre: $("#txtPrimerNombre").val(),
                SegundoNombre: $("#txtSegundoNombre").val(),
                PrimerApellido: $("#txtPrimerApellido").val(),
                SegundoApellido: $("#txtSegundoApellido").val(),
                TelefonoCliente: $("#txtNumeroTelefono").val(),
                IdNacionalidad: $("#ddlNacionalidad :selected").val(),
                FechaNacimiento: $("#txtFechaDeNacimiento").val(),
                Correo: $("#txtCorreoElectronico").val(),
                ProfesionOficio: $("#txtProfesion").val(),
                Sexo: $("input[name='sexoCliente']:checked").val(),
                IdEstadoCivil: $("#ddlEstadoCivil :selected").val(),
                IdVivienda: $("#ddlTipoDeVivienda :selected").val(),
                IdTiempoResidir: $("#ddlTiempoDeResidir :selected").val(),
                DocumentoIDPersonal: $("#dllDoctoIdPersonal :selected").val(),
                fcDescripcionDoctosFiscal: $("#txtNoIdFiscal").val(),
                IDDocumentoFiscal: $("#dllDocumentoIDFiscal :selected").val(),
                IdOrigenEtnico: $("#dllOrigenEtnico :selected").val(),
                IdPaisNacimiento: $("#dllPaisNacimiento :selected").val()

            }
        }
        else {
            $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
            $("#modalFinalizarCondicion_InfoPersonal").modal('hide');
            return false;
        }
    }
    /* Informacion del domicilio */
    else if (idTipoDeCondicion == '11') {
       
        if ($('#frmSolicitud').parsley().isValid({ group: 'informacionDomicilio' })) {

            objSeccion = {
                IdCliente: ID_CLIENTE,
                IdSolicitud: ID_SOLICITUD,
                TelefonoCasa: $("#txtTelefonoCasa").val(),
                IdDepartamento: 0,/*$("#ddlDepartamentoDomicilio :selected").val(),*/
                IdMunicipio: 0,/* $("#ddlMunicipioDomicilio :selected").val(),*/
                IdCiudadPoblado: 0,/*$("#ddlCiudadPobladoDomicilio :selected").val(),*/
                IdBarrioColonia: $("#ddlBarrioColoniaDomicilio :selected").val(),
                //DireccionDetallada: $("#txtDireccionDetalladaDomicilio").val(),
                DireccionDetallada: $("#txtReferenciasDelDomicilio").val(),
                ReferenciasDireccionDetallada: $("#txtReferenciasDelDomicilio").val()
            }

        }
        else {
            $('#frmSolicitud').parsley().validate({ group: 'informacionDomicilio', force: true });
            $("#modalFinalizarCondicion_InfoDomicilio").modal('hide');
            return false;
        }
    }
    /* Informacion laboral */
    else if (idTipoDeCondicion == '12') {

        if ($('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' })) {

            objSeccion = {
                IdCliente: ID_CLIENTE,
                IdSolicitud: ID_SOLICITUD,
                NombreTrabajo: $("#txtNombreDelTrabajo").val(),
                IngresosMensuales: $("#txtIngresosMensuales").val().replace(/,/g, ''),
                PuestoAsignado: $("#txtPuestoAsignado").val(),
                FechaIngreso: $("#txtFechaDeIngreso").val(),
                TelefonoEmpresa: $("#txtTelefonoEmpresa").val(),
                ExtensionRecursosHumanos: $("#txtExtensionRecursosHumanos").val(),
                ExtensionCliente: $("#txtExtensionCliente").val(),
                IdDepartamento: 0, /*$("#ddlDepartamentoEmpresa :selected").val(),*/
                IdMunicipio: 0,/*$("#ddlMunicipioEmpresa :selected").val(),*/
                IdCiudadPoblado: 0, /*$("#ddlCiudadPobladoEmpresa :selected").val(),*/
                IdBarrioColonia: $("#ddlBarrioColoniaEmpresa :selected").val(),
                DireccionDetalladaEmpresa: $("#txtDireccionDetalladaEmpresa").val(),
                ReferenciasDireccionDetallada: $("#txtDireccionDetalladaEmpresa").val(),
                FuenteOtrosIngresos: $("#txtFuenteDeOtrosIngresos").val(),
                ValorOtrosIngresos: $("#txtValorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#txtValorOtrosIngresos").val().replace(/,/g, '')
            }
        }
        else {
            $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
            $("#modalFinalizarCondicion_InfoLaboral").modal('hide');
            return false;
        }

    }
    /* Informacion conyugal */
    else if (idTipoDeCondicion == '13') {

        if ($('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' })) {

            objSeccion = {
                IdCliente: ID_CLIENTE,
                IdSolicitud: ID_SOLICITUD,
                IdentidadConyugue: $("#txtIdentidadConyugue").val(),
                NombreCompletoConyugue: $("#txtNombresConyugue").val(),
                TelefonoConyugue: $("#txtTelefonoConyugue").val(),
                FechaNacimientoConyugue: $("#txtFechaNacimientoConyugue").val(),
                LugarTrabajoConyugue: $("#txtLugarDeTrabajoConyuge").val(),
                IngresosMensualesConyugue: $("#txtIngresosMensualesConyugue").val().replace(/,/g, ''),
                TelefonoTrabajoConyugue: $("#txtTelefonoTrabajoConyugue").val()
            }
        }
        else {
            $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true });
            $("#modalFinalizarCondicion_InfoConyugal").modal('hide');
            return false;
        }
    }

 

    objSeccion = JSON.stringify(objSeccion);

    FinalizarSeccionCondicionamientos(idSolicitudCondicion, idTipoDeCondicion, objSeccion, cotizador);

});

/* Finalizar condicionamientos de una seccion del formulario */
function FinalizarSeccionCondicionamientos(idSolicitudCondicion, idTipoDeCondicion, objSeccion, cotizador) {
    debugger;
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_ActualizarSolicitud.aspx/ActualizarCondicionamiento',
        data: JSON.stringify({ idSolicitudCondicion: idSolicitudCondicion, idCliente: ID_CLIENTE, idTipoDeCondicion: idTipoDeCondicion, objSeccion: objSeccion, dataCrypt: window.location.href, cotizador: cotizador }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al finalizar condicionamiento');
        },
        success: function (data) {

            if (data.d == true) {

                $("#modalFinalizarCondicion_Confirmar").modal('hide');

                RecargarInformacion();
            }
            else {
                MensajeError('Error al finalizar condicionamiento');
            }
        }
    });
}



function CargarReferenciasPersonales() {

    MostrarLoader();

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/ListadoReferenciasPersonalesPorIdSolicitud",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Ocurrió un error al actualizar el listado de referencias personales, contacte al administrador.');
            OcultarLoader();
        },
        success: function (data) {

            /* Referencias personales de la solicitud */
            var tblReferenciasPersonales = $("#tblReferenciasPersonales tbody");
            tblReferenciasPersonales.empty();
            tblReferenciasPersonales.append('<tr><td class="text-center" colspan="6">No hay registros disponibles...</td></tr>');

            if (data.d != null) {

                if (data.d.length > 0) {

                    var referenciasPersonales = data.d;
                    let templateReferenciasPersonales = '';
                    let btnEliminarReferencia = '';
                    let btnEditarReferencia = '';

                    tblReferenciasPersonales.empty();

                    tblReferenciasPersonales.append('<tr><th>nombre completo</th><th>Telefono</th><th>Lugar de trabajo</th><th>Tiempo de conocer</th><th>Parentesco</th><th>Acciones</th></tr>');

                    for (var i = 0; i < referenciasPersonales.length; i++) {

                        btnEliminarReferencia = '<button id="btnEliminarReferencia" data-id="' + referenciasPersonales[i].IdReferencia + '" class="btn btn-sm btn-danger mb-0" type="button" title="Eliminar referencia personal"><i class="far fa-trash-alt"></i></button>';
                        btnEditarReferencia = '<button id="btnEditarReferencia" data-id="' + referenciasPersonales[i].IdReferencia + '" data-nombre="' + referenciasPersonales[i].NombreCompleto + '" data-trabajo="' + referenciasPersonales[i].LugarTrabajo + '" data-telefono="' + referenciasPersonales[i].TelefonoReferencia + '" data-idtiempodeconocer="' + referenciasPersonales[i].IdTiempoDeConocer + '" data-idparentesco="' + referenciasPersonales[i].IdParentescoReferencia + '" class="btn btn-sm btn-info mb-0" type="button" title="Editar referencia personal"><i class="far fa-edit"></i></button>';

                        templateReferenciasPersonales += '<tr><td class="pt-0 pb-0">' + referenciasPersonales[i].NombreCompleto + '</td><td class="pt-0 pb-0 text-center">' + referenciasPersonales[i].TelefonoReferencia + '</td><td class="pt-0 pb-0">' + referenciasPersonales[i].LugarTrabajo + '</td><td class="pt-0 pb-0 text-center">' + referenciasPersonales[i].TiempoDeConocer + '</td><td class="pt-0 pb-0">' + referenciasPersonales[i].DescripcionParentesco + '</td><td class="text-center pt-0 pb-0">' + btnEliminarReferencia + btnEditarReferencia + '</td></tr>';
                    }

                    tblReferenciasPersonales.append(templateReferenciasPersonales);
                }
            }
            else {
                MensajeError('Ocurrió un error al actualizar el listado de referencias personales, contacte al administrador.');
            }

            OcultarLoader();

        }// success
    }); // ajax
}

function RecargarInformacion() {
    Swal.fire(
        {
            title: 'Condición finalizada correctamente',
            text: 'La condición se finalizó exitosamente, la información se va a recargar al presionar "OK"',
            type: 'success',
            showCancelButton: false,
            confirmButtonColor: "#58db83",
            confirmButtonText: "OK"
        }
    ).then(function () {
        location.reload();
    });
}

$('#txtValorFinanciar').blur(function () {
    debugger;
    var valorGlobal = parseFloat($("#txtValorGlobal").val().replace(/,/g, '') == '' ? 0 : $("#txtValorGlobal").val().replace(/,/g, ''));
    var valorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtValorPrima").val().replace(/,/g, ''));
    var plazo = parseInt($("#txtPlazoSeleccionado").val().replace(/,/g, '') == '' ? 0 : $("#txtPlazoSeleccionado").val().replace(/,/g, '')); 
    var valorFinanciar = parseFloat($("#txtValorFinanciar").val().replace(/,/g, ''));
   
    var InteresAnual = parseFloat($("#txtTasaDeInteresAnual").val().replace(/,/g, ''));
    if (valorFinanciar > 0 && plazo > 0) {
        CalculoPrestamo(valorFinanciar.toString(), plazo.toString(), InteresAnual.toString());
    }
});


/* Cargar prestamos disponibles consultados en el cotizador */
function CalculoPrestamo(valoraFinanciar, plazo, tasadeinteres) {
    debugger;
    var frecuencia = 0;
    var frecuenciaseleccionada = $("#ddlFrecuencia option:selected").val();

    if (frecuenciaseleccionada == 1) {
        frecuencia = 26;
    }

    if (frecuenciaseleccionada == 3) {
        frecuencia = 12;
    }

    if (frecuenciaseleccionada == 10) {
        frecuencia = 52;
    }
    var lnPlazoAnual = (plazo / frecuencia)
    var lnTotalInteres = ((valoraFinanciar * lnPlazoAnual * tasadeinteres) / 100);
    var lnTotalFinanciado = ((parseFloat(valoraFinanciar) + lnTotalInteres) / plazo);

    $("#txtValorCuota").val(lnTotalFinanciado.toFixed(2));


  //      $.ajax({
  //          type: "POST",
  //          url: "SolicitudesCredito_ActualizarSolicitud.aspx/CalculoPrestamo",
  //          data: JSON.stringify({ idProducto: PRECALIFICADO.IdProducto, valorGlobal: valorGlobal, valorPrima: valorPrima, plazo: plazo, dataCrypt: window.location.href }),
  //          contentType: 'application/json; charset=utf-8',
  //          error: function (xhr, ajaxOptions, thrownError) {

  //              MensajeError('No se pudo realizar el cálculo del préstamo, contacte al administrador');
  //          },
  //          success: function (data) {

  //              var objCalculo = data.d;
  //              $("#txtValorDePrestamo").val(objCalculo.ValorDelPrestamo);
  //              $("#txtValorCuota").val(objCalculo.CuotaTotal);
  //              $("#txtValorFinanciar").val(objCalculo.TotalAFinanciar);
  //              $("#txtTasaDeInteresAnual").val(objCalculo.TasaInteresAnual + '%');

  //              COTIZADOR = objCalculo;
  //          }
  //      });
 
}

//function RecargarInformacion() {

// MostrarLoader();

// $.ajax({
// type: "POST",
// url: "SolicitudesCredito_ActualizarSolicitud.aspx/ObtenerInformacionClienteSolicitudPorIdSolicitud",
// data: JSON.stringify({ dataCrypt: window.location.href }),
// contentType: "application/json; charset=utf-8",
// error: function (xhr, ajaxOptions, thrownError) {
// MensajeError('Ocurrió un error al recargar la información, contacte al administrador.');
// OcultarLoader();
// },
// success: function (data) {


// if (data.d != null) {

// var resultado = data.d;

// /* información de pestaña información personal */
// $("#txtIdentidadCliente").val(respaldoInformacionPersonal.txtProfesion);
// $("#txtRtnCliente").val(respaldoInformacionPersonal.txtProfesion);
// $("#txtPrimerNombre").val(respaldoInformacionPersonal.txtProfesion);
// $("#txtSegundoNombre").val(respaldoInformacionPersonal.txtProfesion);
// $("#txtPrimerApellido").val(respaldoInformacionPersonal.txtProfesion);
// $("#txtSegundoApellido").val(respaldoInformacionPersonal.txtProfesion);
// $("#ddlNacionalidad").val(respaldoInformacionPersonal.ddlNacionalidad);
// $("#ddlTipoDeCliente").val(respaldoInformacionPersonal.ddlEstadoCivil);
// $("#txtFechaDeNacimiento").val(respaldoInformacionPersonal.txtProfesion);
// $("#txtEdadDelCliente").val(respaldoInformacionPersonal.txtProfesion);
// $("input[name=sexoCliente][value=" + respaldoInformacionPersonal.sexoCliente + "]").prop('checked', true);
// $("#txtCorreoElectronico").val(respaldoInformacionPersonal.txtCorreoElectronico);
// $("#txtNumeroTelefono").val(respaldoInformacionPersonal.txtCorreoElectronico);
// $("#ddlTipoDeVivienda").val(respaldoInformacionPersonal.ddlTipoDeVivienda);
// $("#ddlTiempoDeResidir").val(respaldoInformacionPersonal.ddlTiempoDeResidir);
// $("#ddlEstadoCivil").val(respaldoInformacionPersonal.ddlEstadoCivil);
// $("#txtProfesion").val(respaldoInformacionPersonal.txtProfesion);


// /* Informacion domicilio */
// $("#ddlDepartamentoDomicilio").val(respaldoinformacionDomicilio.ddlDepartamentoDomicilio);
// CargarMunicipios(respaldoinformacionDomicilio.ddlDepartamentoDomicilio, 'Domicilio', false, respaldoinformacionDomicilio.ddlMunicipioDomicilio);
// CargarCiudadesPoblados(respaldoinformacionDomicilio.ddlDepartamentoDomicilio, respaldoinformacionDomicilio.ddlMunicipioDomicilio, 'Domicilio', false, respaldoinformacionDomicilio.ddlCiudadPobladoDomicilio);
// CargarBarriosColonias(respaldoinformacionDomicilio.ddlDepartamentoDomicilio, respaldoinformacionDomicilio.ddlMunicipioDomicilio, respaldoinformacionDomicilio.ddlCiudadPobladoDomicilio, 'Domicilio', respaldoinformacionDomicilio.ddlBarrioColoniaDomicilio);
// $("#txtTelefonoCasa").val(respaldoinformacionDomicilio.txtTelefonoCasa);
// $("#txtDireccionDetalladaDomicilio").val(respaldoinformacionDomicilio.txtDireccionDetalladaDomicilio);
// $("#txtReferenciasDelDomicilio").val(respaldoinformacionDomicilio.txtReferenciasDelDomicilio);

// /* Informacion laboral */
// $("#txtNombreDelTrabajo").val(respaldoInformacionLaboral.txtNombreDelTrabajo);
// $("#txtFechaDeIngreso").val(respaldoInformacionLaboral.txtFechaDeIngreso);
// $("#txtPuestoAsignado").val(respaldoInformacionLaboral.txtPuestoAsignado);
// $("#txtIngresosMensuales").val(respaldoInformacionLaboral.txtPuestoAsignado);
// $("#txtTelefonoEmpresa").val(respaldoInformacionLaboral.txtTelefonoEmpresa);
// $("#txtExtensionRecursosHumanos").val(respaldoInformacionLaboral.txtExtensionRecursosHumanos);
// $("#txtExtensionCliente").val(respaldoInformacionLaboral.txtExtensionCliente);
// $("#txtFuenteDeOtrosIngresos").val(respaldoInformacionLaboral.txtFuenteDeOtrosIngresos);
// $("#txtValorOtrosIngresos").val(respaldoInformacionLaboral.txtValorOtrosIngresos);
// $("#ddlDepartamentoEmpresa").val(respaldoInformacionLaboral.ddlDepartamentoEmpresa);
// CargarMunicipios(respaldoInformacionLaboral.ddlDepartamentoEmpresa, 'Empresa', false, respaldoInformacionLaboral.ddlMunicipioEmpresa);
// CargarCiudadesPoblados(respaldoInformacionLaboral.ddlDepartamentoEmpresa, respaldoInformacionLaboral.ddlMunicipioEmpresa, 'Empresa', false, respaldoInformacionLaboral.ddlCiudadPobladoEmpresa);
// CargarBarriosColonias(respaldoInformacionLaboral.ddlDepartamentoEmpresa, respaldoInformacionLaboral.ddlMunicipioEmpresa, respaldoInformacionLaboral.ddlCiudadPobladoEmpresa, 'Empresa', respaldoInformacionLaboral.ddlBarrioColoniaEmpresa);
// $("#txtDireccionDetalladaEmpresa").val(respaldoInformacionLaboral.txtDireccionDetalladaEmpresa);
// $("#txtReferenciasEmpresa").val(respaldoInformacionLaboral.txtReferenciasEmpresa);

// /* Informacion conyugal */
// if (informacionConyugal != null) {

// var respaldoInformacionConyugal = JSON.parse(localStorage.getItem('RespaldoInformacionConyugal'));

// $("#txtIdentidadConyugue").val(respaldoInformacionConyugal.txtIdentidadConyugue);
// $("#txtNombresConyugue").val(respaldoInformacionConyugal.txtNombresConyugue);
// $("#txtFechaNacimientoConyugue").val(respaldoInformacionConyugal.txtFechaNacimientoConyugue);
// $("#txtTelefonoConyugue").val(respaldoInformacionConyugal.txtTelefonoConyugue);
// $("#txtLugarDeTrabajoConyuge").val(respaldoInformacionConyugal.txtLugarDeTrabajoConyuge);
// $("#txtIngresosMensualesConyugue").val(respaldoInformacionConyugal.txtIngresosMensualesConyugue);
// $("#txtTelefonoTrabajoConyugue").val(respaldoInformacionConyugal.txtTelefonoTrabajoConyugue);
// }
// else if ($("#ddlEstadoCivil :selected").data('requiereinformacionconyugal') == false) {

// $(".infoConyugal").prop('disabled', true);
// }



// /* Referencias personales de la solicitud */
// var tblReferenciasPersonales = $("#tblReferenciasPersonales tbody");
// tblReferenciasPersonales.empty();
// tblReferenciasPersonales.append('<tr><td class="text-center" colspan="6">No hay registros disponibles...</td></tr>');

// if (data.d != null) {

// if (data.d.length > 0) {

// var referenciasPersonales = resultado.Cliente.ListaReferenciasPersonales;
// let templateReferenciasPersonales = '';
// let btnEliminarReferencia = '';
// let btnEditarReferencia = '';

// tblReferenciasPersonales.empty();

// tblReferenciasPersonales.append('<tr><th>nombre completo</th><th>Telefono</th><th>Lugar de trabajo</th><th>Tiempo de conocer</th><th>Parentesco</th><th>Acciones</th></tr>');

// for (var i = 0; i < referenciasPersonales.length; i++) {

// btnEliminarReferencia = '<button id="btnEliminarReferencia" data-id="' + referenciasPersonales[i].IdReferencia + '" class="btn btn-sm btn-danger mb-0" type="button" title="Eliminar referencia personal"><i class="far fa-trash-alt"></i></button>';
// btnEditarReferencia = '<button id="btnEditarReferencia" data-id="' + referenciasPersonales[i].IdReferencia + '" data-nombre="' + referenciasPersonales[i].NombreCompleto + '" data-trabajo="' + referenciasPersonales[i].LugarTrabajo + '" data-telefono="' + referenciasPersonales[i].TelefonoReferencia + '" data-idtiempodeconocer="' + referenciasPersonales[i].IdTiempoDeConocer + '" data-idparentesco="' + referenciasPersonales[i].IdParentescoReferencia + '" class="btn btn-sm btn-info mb-0" type="button" title="Editar referencia personal"><i class="far fa-edit"></i></button>';

// templateReferenciasPersonales += '<tr><td class="pt-0 pb-0">' + referenciasPersonales[i].NombreCompleto + '</td><td class="pt-0 pb-0 text-center">' + referenciasPersonales[i].TelefonoReferencia + '</td><td class="pt-0 pb-0">' + referenciasPersonales[i].LugarTrabajo + '</td><td class="pt-0 pb-0 text-center">' + referenciasPersonales[i].TiempoDeConocer + '</td><td class="pt-0 pb-0">' + referenciasPersonales[i].DescripcionParentesco + '</td><td class="text-center pt-0 pb-0">' + btnEliminarReferencia + btnEditarReferencia + '</td></tr>';
// }

// tblReferenciasPersonales.append(templateReferenciasPersonales);
// }
// }

// }
// else {
// MensajeError('Ocurrió un error al recargar la información, contacte al administrador.');
// }

// OcultarLoader();

// }// success
// }); // ajax
//}

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

function ResetForm($form) {
    $form.find('input:text, input:password, input:file,input[type="date"],input[type="email"], select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .removeAttr('checked').removeAttr('selected');
}

function MostrarLoader() {
    $("#divLoader").css('display', '');
}

function OcultarLoader() {
    $("#divLoader").css('display', 'none');
}