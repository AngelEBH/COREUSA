var objSeccion = '';

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

/* Validar listas desplegables */
$("select").on('change', function () {
    $(this).parsley().validate();
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
                    BuscarSolicitud();
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
                    BuscarSolicitud();
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
                    BuscarSolicitud();
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

    idSolicitudCondicion = $(this).data('id');
    idTipoDeCondicion = $(this).data('idtipocondicion');
    objSeccion = {};

    $('.modal').modal('hide');
    $("#modalFinalizarCondicion_Confirmar").modal();

});

/* Finalizar condicionamientos */
$("#btnFinalizarCondicion_Confirmar").click(function () {
    debugger;

    var listaCondicionesDeDocumentacion = [1, 2, 3, 4, 5, 6];

    /* Documentacion */
    if (jQuery.inArray(idTipoDeCondicion, listaCondicionesDeDocumentacion) >= 0) {

        objSeccion = {};

    }
    /* Referencias personales */
    else if (idTipoDeCondicion == '8' || idTipoDeCondicion == '14') {

        objSeccion = {};

        //if (listaClientesReferencias != null) {
        //    if (listaClientesReferencias.length > 0) {

        //        objSeccion = listaClientesReferencias;

        //    }
        //    else {
        //        MensajeError('La cantidad minima de referencias personales es: 4. Entre ellas 2 familiares.');
        //        $("#modalFinalizarCondicion_ReferenciasPersonales").modal('hide');
        //        return false;
        //    }
        //}
        //else {
        //    MensajeError('No se detectó ningún cambio en la referencias personalels.');
        //    $("#modalFinalizarCondicion_ReferenciasPersonales").modal('hide');
        //    return false;
        //}
    }
    /* Informacion de la solicitud */
    else if (idTipoDeCondicion == '9') {

        /*objSeccion = {
            fiIDSolicitud: ID_SOLICITUD,
            IdCliente: ID_CLIENTE,
            fnPrima: $("#txtPrima").val().replace(/,/g, ''),
            fnValorGarantia: $("#txtValorVehiculo").val().replace(/,/g, '')
        }*/

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
                IdTiempoResidir: $("#ddlTiempoDeResidir :selected").val()                
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
                IdDepartamento: $("#ddlDepartamentoDomicilio :selected").val(),
                IdMunicipio: $("#ddlMunicipioDomicilio :selected").val(),
                IdCiudadPoblado: $("#ddlCiudadPobladoDomicilio :selected").val(),
                IdBarrioColonia: $("#ddlBarrioColoniaDomicilio :selected").val(),
                DireccionDetallada: $("#txtDireccionDetalladaDomicilio").val(),
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
                IdDepartamento: $("#ddlDepartamentoEmpresa :selected").val(),
                IdMunicipio: $("#ddlMunicipioEmpresa :selected").val(),
                IdCiudadPoblado: $("#ddlCiudadPobladoEmpresa :selected").val(),
                IdBarrioColonia: $("#ddlBarrioColoniaEmpresa :selected").val(),
                DireccionDetalladaEmpresa: $("#txtDireccionDetalladaEmpresa").val(),
                ReferenciasDireccionDetallada: $("#txtReferenciasEmpresa").val(),
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

    FinalizarSeccionCondicionamientos(idSolicitudCondicion, idTipoDeCondicion, objSeccion);

});

/* Finalizar condicionamientos de una seccion del formulario */
function FinalizarSeccionCondicionamientos(idSolicitudCondicion, idTipoDeCondicion, objSeccion) {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_ActualizarSolicitud.aspx/ActualizarCondicionamiento',
        data: JSON.stringify({ idSolicitudCondicion: idSolicitudCondicion, idCliente: ID_CLIENTE, idTipoDeCondicion: idTipoDeCondicion, objSeccion: objSeccion, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al finalizar condicionamiento');
        },
        success: function (data) {

            if (data.d == true) {

                CargarInformacionCondiciones();

                $("#modalFinalizarCondicion").modal('hide');

                MensajeExito('Condición finalizada correctamente');
            }
            else {
                MensajeError('Error al finalizar condicionamiento');
            }
        }
    });
}

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