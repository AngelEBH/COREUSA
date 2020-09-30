var idSolicitud = 0;
var ListaMunicipios = [];
var ListaCiudades = [];
var ListaBarriosColonias = [];
var clienteID = 0;
var estadoFuncionLlenarDDL = false;
var estadoFuncionCargarInformacionCliente = false;
var estadoFuncionRecuperarRespaldos = true;

/* SMART WIZARD */
var btnFinish = $('<button id="btnActualizarSolicitud" type="button"></button>').text('Finalizar')
    .addClass('btn btn-info')
    .on('click', function () {

        var qString = "?" + window.location.href.split("?")[1];

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_ActualizarSolicitud.aspx/FinalizarCondicionamientoSolicitud' + qString,
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('No se guardó el registro, contacte al administrador');
            },
            success: function (data) {

                if (data.d == "Solicitud actualizada correctamente") {

                    MensajeExito(data.d);
                    clienteID = 0;
                    window.location = "SolicitudesCredito_Ingresadas.aspx" + qString;
                }
                else {
                    MensajeError(data.d);
                }
            }
        });
    });

var btnDetallesCondicion = $('<button id="btnDetalles" type="button" disabled></button>').text('Actualizar lista')
    .addClass('btn btn-secondary');

var btnPantallaAval = $('<button data-clt="0" id="btnPantallaAval" disabled type="button" style="display:none;"></button>').text('Registrar Aval')
    .addClass('btn btn-secondary')
    .on('click', function () {
        var ID = clienteID;

        if (ID != "0") {

            var qString = "?" + window.location.href.split("?")[1];

            $.ajax({
                type: "POST",
                url: 'SolicitudesCredito_ActualizarSolicitud.aspx/obtenerUrlEncriptado' + qString,
                data: JSON.stringify({ IDCliente: ID }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {

                    MensajeError('Error al cargar proceso de Aval');
                },
                success: function (data) {

                    var parametros = data.d;
                    var URLProd = "http://172.20.3.140/Solicitudes/Forms/Aval/Aval_CondicionamientoRegistrarAval.aspx?";
                    var URLDev = 'http://localhost:49553/Forms/Aval/Aval_CondicionamientoRegistrarAval.aspx?';
                    //window.location = "/Solicitudes/Forms/Aval/Aval_CondicionamientoRegistrarAval.aspx?" + parametros;
                    window.location = "../Aval/Aval_Registrar.aspx?" + parametros;
                }
            });
        } else {
            MensajeError('Error al cargar proceso de Aval');
        }
    });

var btnActualizarAval = $('<button data-clt="0" id="btnActualizarAval" disabled type="button" style="display:none;"></button>').text('Actualizar Aval')
    .addClass('btn btn-secondary')
    .on('click', function () {

        var ID = clienteID;
        if (ID != "0") {

            var qString = "?" + window.location.href.split("?")[1];

            $.ajax({
                type: "POST",
                url: 'SolicitudesCredito_ActualizarSolicitud.aspx/obtenerUrlEncriptado' + qString,
                data: JSON.stringify({ IDCliente: ID }),
                contentType: 'application/json; charset=utf-8',
                error: function (xhr, ajaxOptions, thrownError) {

                    MensajeError('Error al cargar proceso de Aval');
                },
                success: function (data) {

                    var parametros = data.d;
                    var URLProd = "http://172.20.3.148/Forms/Aval/Aval_CondicionamientoActualizarAval.aspx?";
                    var URLDev = 'http://localhost:49553/Forms/Aval/Aval_CondicionamientoActualizarAval.aspx?';
                    window.location = '../Aval/Aval_CondicionamientoActualizarAval.aspx?' + parametros;
                }
            });
        } else {
            MensajeError('Error al cargar proceso de Aval');
        }

    });


$('#smartwizard').smartWizard({
    selected: 0,
    theme: 'default',
    transitionEffect: 'fade',
    showStepURLhash: false,
    autoAdjustHeight: false,
    toolbarSettings: {
        toolbarPosition: 'bottom',
        toolbarButtonPosition: 'end',
        toolbarExtraButtons: [btnDetallesCondicion, btnPantallaAval, btnActualizarAval]
    },
    lang: {// variables del lenguaje
        next: 'Siguiente',
        previous: 'Anterior'
    }
});


$('#smartwizard').smartWizard("stepState", [1, 2, 3, 4, 5, 6], "hide");
/* SMART WIZARD */

LlenarListas();
ObtenerDetalles();


$(document).ready(function () {

    // Cuando se muestre un step
    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {

        // si es el primer paso, deshabilitar el boton "anterior"
        if (stepPosition === 'first') {
            $("#prev-btn").addClass('disabled').css('display', 'none');
            // si es el ultimo paso, deshabilitar el boton siguiente
        } else if (stepPosition === 'final') {
            $("#next-btn").addClass('disabled');

            // si no es ninguna de las anteriores, habilitar todos los botones
        } else {
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
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


    // cuando se deja un paso (realizar validaciones)
    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {

        //si no requere informacion personal, saltarse esa pestaña
        if ($("input[name='estadoCivil']:checked").data('info') == false) {
            $('#smartwizard').smartWizard("stepState", [4], "hide");
        }
        // validar solo si se quiere ir hacia el siguiente paso
        if (stepDirection == 'forward') {

            //aqui realizar las validaciones
            if (stepNumber == 0) {

            }

            //validar informacion personal
            if (stepNumber == 1) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPersonal' });

                if (state == false) {
                    //si no es valido, mostrar validaciones al usuario
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPersonal', force: true });
                }
                return state;
            }

            //validar informacion domiciliar
            if (stepNumber == 2) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionDomiciliar' });

                if (state == false) {

                    //si no es valido, mostrar validaciones al usuario
                    $('#frmSolicitud').parsley().validate({ group: 'informacionDomiciliar', force: true });
                }
                return state;
            }

            //validar informacion laboral
            if (stepNumber == 3) {

                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

                if (state == false) {
                    //si no es valido, mostrar validaciones al usuario
                    $('#frmSolicitud').parsley().validate({ group: 'informacionLaboral', force: true });
                }
                return state;
            }

            //validar informacion conyugal
            if (stepNumber == 4) {

                if ($("input[name='estadoCivil']:checked").data('info') == true) {

                    var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionConyugal' });

                    if (state == false) {
                        //si no es valido, mostrar validaciones al usuario
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
                        state = true;
                    }
                }
                if (cantidadReferencias > 0) {
                    state = true;
                } else {
                    MensajeError('Las referencias personales son requeridas');
                }
                return state;
            }

        }

    });


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



// llenar dinámicamente los dropdownlists del formulario de ingresar solicitud pendiente
function LlenarListas() {

    estadoFuncionLlenarDDL = false;
    $("#spinnerCargando").css('display', '');
    $("select").empty();

    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarListas" + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

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
                    "<input data-info='" + iter.fbRequiereInformacionConyugal + "' class='form-check-input informacionPersonal' type='radio' name ='estadoCivil' value='" + iter.fiIDEstadoCivil + "'>" +
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

            // llenar lista de municipios
            $("#municipio").append("<option value=''>Seleccione un depto.</option>");
            ListaMunicipios = [];

            // llenar lista de barrios y colonias
            $("#barrioColonia").append("<option value=''>Seleccione una ciudad</option>");
            ListaBarriosColonias = [];

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

            // llenar select de parentescos de referencias personales (modal)
            var parentescoRefDdl = $("#parentescoRef");

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
                    "<form action='SolicitudesCredito_ActualizarSolicitud.aspx?type=upload&doc=" + iter.IDTipoDocumento + "' method='post' enctype='multipart/form-data'>" +
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
                        url: 'SolicitudesCredito_ActualizarSolicitud.aspx?type=upload&doc=' + iter.IDTipoDocumento,
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
                });
            });

            estadoFuncionLlenarDDL = true;

            cargarInformacionSolicitud();

            if (estadoFuncionCargarInformacionCliente == true) {
                $("#spinnerCargando").css('display', 'none');
            }
        }
    });
}

var IDCondicionAval = 0;

function ObtenerDetalles() {

    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_ActualizarSolicitud.aspx/DetallesCondicion' + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar detalles del condicionamiento');
        },
        success: function (data) {

            if (data.d != null) {

                var mostrarInfoPersonal = false;
                var mostrarInfoDomicilio = false;
                var mostrarInfoLaboral = false;
                var mostrarInfoConyugal = false;
                var mostrarReferencias = false;
                var mostrarDocumentacion = false;
                var mostrarBtnRegistrarModal = false;
                var mostrarBtnActualizarModal = false;

                var tablaCondiciones = $("#tblCondiciones tbody");

                tablaCondiciones.empty();

                if (data.d.length > 0) {

                    var listaCondiciones = data.d;

                    for (var i = 0; i < listaCondiciones.length; i++) {

                        var btnFinalizarCondicion = listaCondiciones[i].Estado == true ? '<button type="button" id="btnTerminarCondicion" data-id=' + listaCondiciones[i].IDSolicitudCondicion + ' data-tipo="' + listaCondiciones[i].TipoCondicion + '" data-seccion="' + listaCondiciones[i].DescripcionCondicion + '" class="btn btn-lg mdi mdi-check mdi-24px text-warning" title="Finalizar"></button>' : '<button type="button" class="btn mdi mdi-check mdi-24px text-info" title="Finalizado"></button>';

                        var row = '<tr class="noSpace"><td>' + listaCondiciones[i].TipoCondicion + '</td><td>' + listaCondiciones[i].DescripcionCondicion + '</td><td>' + listaCondiciones[i].ComentarioAdicional + '</td><td>' + btnFinalizarCondicion + '</td></tr>';

                        tablaCondiciones.append(row);

                        if (listaCondiciones[i].TipoCondicion == "Documentacion" && listaCondiciones[i].Estado == true) {
                            mostrarDocumentacion = true;
                        }

                        switch (listaCondiciones[i].DescripcionCondicion) {

                            case "Correccion Informacion Personal":
                                if (listaCondiciones[i].Estado == true) {
                                    mostrarInfoPersonal = true;
                                }
                                break;
                            case "Correccion Informacion Domiciliar":
                                if (listaCondiciones[i].Estado == true) {
                                    mostrarInfoDomicilio = true;
                                }
                                break;
                            case "Correccion Informacion Laboral":
                                if (listaCondiciones[i].Estado == true) {
                                    mostrarInfoLaboral = true;
                                }
                                break;
                            case "Correccion Informacion Conyugal":
                                if (listaCondiciones[i].Estado == true) {
                                    mostrarInfoConyugal = true;
                                }
                                break;
                            case "Correccion Referencias":
                                if (listaCondiciones[i].Estado == true) {
                                    mostrarReferencias = true;
                                }
                                break;
                            case "Registrar Aval":
                                if (listaCondiciones[i].Estado == true) {
                                    mostrarBtnRegistrarModal = true;
                                    IDCondicionAval = listaCondiciones[i].IDSolicitudCondicion;
                                    localStorage.setItem('CondAval', IDCondicionAval);
                                }
                                break;
                        }
                    }
                    if (mostrarInfoPersonal == true) {
                        $('#smartwizard').smartWizard("stepState", [1], "show");
                        $("#pestanaInfPersonal").css('display', '');
                        $(".informacionPersonal").prop('disabled', false).prop('readonly', false);
                    } else {
                        $('#smartwizard').smartWizard("stepState", [1], "hide");
                        $("#pestanaInfPersonal").css('display', 'none');
                        $(".informacionPersonal").prop('disabled', true).prop('readonly', true);
                    }

                    if (mostrarInfoDomicilio == true) {
                        $('#smartwizard').smartWizard("stepState", [2], "show");
                        $("#pestanaInfDomicilio").css('display', '');
                        $(".informacionDomiciliar").prop('disabled', false).prop('readonly', false);
                    }
                    else {
                        $('#smartwizard').smartWizard("stepState", [2], "hide");
                        $("#pestanaInfDomicilio").css('display', 'none');
                        $(".informacionDomiciliar").prop('disabled', true).prop('readonly', true);
                    }

                    if (mostrarInfoLaboral == true) {
                        $('#smartwizard').smartWizard("stepState", [3], "show");
                        $("#pestanaInfLaboral").css('display', '');
                        $(".informacionLaboral").prop('disabled', false).prop('readonly', false);
                    }
                    else {
                        $('#smartwizard').smartWizard("stepState", [3], "hide");
                        $("#pestanaInfLaboral").css('display', 'none');
                        $(".informacionLaboral").prop('disabled', true).prop('readonly', true);
                    }

                    if (mostrarInfoConyugal == true) {
                        $('#smartwizard').smartWizard("stepState", [4], "show");
                        $("#pestanaInfConyugal").css('display', '');
                        $(".infoConyugal").prop('disabled', false).prop('readonly', false);
                    }
                    else {
                        $('#smartwizard').smartWizard("stepState", [4], "hide");
                        $("#pestanaInfConyugal").css('display', 'none');
                        $(".infoConyugal").prop('disabled', true).prop('readonly', true);
                    }
                    //?TY+5GZ6AGYKv916x7KwGLrXp/NxR5c+2Ep2TMiMqXqS44VIeCzbQp4/8qDTcXz5eAJeEdorJv+D1j+t83YBgZw==
                    debugger;
                    if (mostrarReferencias == true) {
                        $('#smartwizard').smartWizard("stepState", [5], "show");
                        $("#pestanaReferencias").css('display', '');
                        $("#btnNuevaReferencia").prop('disabled', false).prop('readonly', false);
                    }
                    else {
                        $('#smartwizard').smartWizard("stepState", [5], "hide");
                        $("#pestanaReferencias").css('display', 'none');
                        $("#btnNuevaReferencia").prop('disabled', true).prop('readonly', true);
                    }

                    if (mostrarDocumentacion == true) {
                        $('#smartwizard').smartWizard("stepState", [6], "show");
                        $("#pestanaDocumentacion").css('display', '');
                    }
                    else {
                        $('#smartwizard').smartWizard("stepState", [6], "hide");
                        $("#pestanaDocumentacion").css('display', 'none');
                    }

                    if (mostrarBtnRegistrarModal == true) {
                        $("#btnPantallaAval").prop('disabled', false).css('display', '');
                    }
                    else {
                        $("#btnPantallaAval").prop('disabled', false).css('display', 'none');
                    }

                }
                else {
                    ("#tblCondiciones").css('display', 'none');
                    MensajeInformacion('No se encontraron condicionamientos pendientes en esta solicitud');
                }
            }
            $("#btnDetalles").prop('disabled', false);
        }
    });

}

//finalizar condicion
var prestamoSeleccionado = [];
var idCondicion = 0;
var button = [];
var seccionFormulario = '';

$(document).on('click', 'button#btnTerminarCondicion', function () {

    idCondicion = $(this).data('id');
    button = $(this);
    seccionFormulario = $(this).data('tipo') != 'Documentacion' ? $(this).data('seccion') : 'Documentacion';
    $("#modalDetails").modal('hide');
    $("#modalFinalizarCondicionLista").modal();
});

$("#btnDetalles").click(function () {
    ObtenerDetalles();
});

//confirmar actualizacion de los ingresos del cliente
$("#btnTerminarCondicionFinalizar").click(function () {

    var objSeccion = {};

    debugger;
    switch (seccionFormulario) {
        case "Correccion Informacion de la Solicitud":
            objSeccion = {
                fiIDSolicitud: idSolicitud,
                fiIDCliente: clienteID,
                fnPrima: $("#txtPrima").val().replace(/,/g, ''),
                fnValorGarantia: $("#txtValorVehiculo").val().replace(/,/g, '')
            }
            break;
        case "Correccion Informacion Personal":
            objSeccion = {
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
            }
            break;
        case "Correccion Informacion Domiciliar":
            objSeccion = {
                fiIDCliente: clienteID,
                fiIDDepto: $("#departamento :selected").val(),
                fiIDMunicipio: $("#municipio :selected").val(),
                fiIDCiudad: $("#ciudad :selected").val(),
                fiIDBarrioColonia: $("#barrioColonia :selected").val(),
                
                fcTelefonoCasa: $("#telefonoCasa").val(),
                fcTelefonoMovil: $("#telefonoMovil").val(),
                fcDireccionDetallada: $("#direccionDetallada").val(),
                fcReferenciasDireccionDetallada: $("#referenciaDireccionDetallada").val()
            }
            break;
        case "Correccion Informacion Laboral":
            objSeccion = {
                fiIDCliente: clienteID,
                fcNombreTrabajo: $("#nombreDelTrabajo").val(),
                fiIngresosMensuales: $("#ingresosMensuales").val().replace(/,/g, ''),
                fcPuestoAsignado: $("#puestoAsignado").val(),
                fcFechaIngreso: $("#fechaIngreso").val(),
                fdTelefonoEmpresa: $("#telefonoEmpresa").val(),
                fcExtensionRecursosHumanos: $("#extensionRRHH").val().replace(/_/g, ''),
                fcExtensionCliente: $("#extensionCliente").val().replace(/_/g, ''),
                fiIDDepto: $("#departamentoEmpresa :selected").val(),
                fiIDMunicipio: $("#municipioEmpresa :selected").val(),
                fiIDCiudad: $("#ciudadEmpresa :selected").val(),
                fiIDBarrioColonia: $("#barrioColoniaEmpresa :selected").val(),
                fcDireccionDetalladaEmpresa: $("#direccionDetalladaEmpresa").val(),
                fcReferenciasDireccionDetallada: $("#referenciaDireccionDetalladaEmpresa").val(),
                fcFuenteOtrosIngresos: $("#fuenteOtrosIngresos").val(),
                fiValorOtrosIngresosMensuales: $("#valorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#valorOtrosIngresos").val().replace(/,/g, '')
            }
            break;
        case "Correccion Informacion Conyugal":
            objSeccion = {
                fiIDCliente: clienteID,
                fcNombreCompletoConyugue: $("#nombresConyugue").val() + ' ' + $("#apellidosConyugue").val(),
                fcIndentidadConyugue: $("#identidadConyugue").val(),
                fdFechaNacimientoConyugue: $("#fechaNacimientoConyugue").val(),
                fcTelefonoConyugue: $("#telefonoConyugue").val(),
                fcLugarTrabajoConyugue: $("#lugarTrabajoConyugue").val(),
                fcIngresosMensualesConyugue: $("#ingresoMensualesConyugue").val().replace(/,/g, ''),
                fcTelefonoTrabajoConyugue: $("#telefonoTrabajoConyugue").val()
            }
            break;
        case "Correccion Referencias":
            objSeccion = listaClientesReferencias;
            break;
        case "Documentacion":
            objSeccion = {};
            break;
    }
    objSeccion = JSON.stringify(objSeccion);

    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_ActualizarSolicitud.aspx/ActualizarCondicionamiento' + qString,
        data: JSON.stringify({ ID: idCondicion, seccionFormulario: seccionFormulario, objSeccion: objSeccion }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al finalizar condicionamiento');
        },
        success: function (data) {

            if (data.d == true) {

                ObtenerDetalles();

                $("#modalFinalizarCondicionLista,#modalDetails").modal('hide');
                MensajeExito('Condición finalizada correctamente');
                button.removeClass('text-warning').addClass('text-info');
                button.on('click', function () { return false; });
            }
            else {
                MensajeError('Error al finalizar condicionamiento');
                button.addClass('text-warning');
            }
        }
    });
});

//función cargar información de la solicitud
function cargarInformacionSolicitud() {

    estadoFuncionCargarInformacionCliente = false;

    $("#spinnerCargando").css('display', '');

    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarInformacionSolicitud" + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeError('No se pudo carga la información, contacte al administrador');
        },
        success: function (data) {

            cargarInformacionCompletaDelCliente(data.d.cliente);

            //variable de informacion de la solicitud
            var rowDataSolicitud = data.d.solicitud;

            $("#txtOtrasCondiciones").val(rowDataSolicitud.fcCondicionadoComentario);

            idSolicitud = rowDataSolicitud.fiIDSolicitud;

            objSolicitud = data.d.solicitud;


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

            estadoFuncionCargarInformacionCliente = true;

            if (estadoFuncionLlenarDDL == true) {
                $("#spinnerCargando").css('display', 'none');
            }
        }
    });
}


//modal observaciones del depto de credito, razones por las que fue condicionada la solicitud
$("#btnObservacionesCredito").click(function () {

    $("#modalObservacionesCredito").modal();

});

//funcion para llenar el formulario con la informacion de un cliente existente
function cargarInformacionCompletaDelCliente(informacionCliente) {

    estadoFuncionRecuperarInfoCliente = false;

    $("#spinnerCargando").css('display', '');

    //información del cliente
    rowData = informacionCliente;

    clienteID = rowData.clientesMaster.fiIDCliente;

    //llenar el campo con la info del cliente y dejarlo como solo lectura
    $("#primerNombreCliente").val(rowData.clientesMaster.fcPrimerNombreCliente);
    $("#primerNombreCliente").attr('readonly', true);

    //llenar el campo con la info del cliente y dejarlo como solo lectura
    $("#SegundoNombreCliente").val(rowData.clientesMaster.fcSegundoNombreCliente);
    $("#SegundoNombreCliente").attr('readonly', true);

    //llenar el campo con la info del cliente y dejarlo como solo lectura
    $("#primerApellidoCliente").val(rowData.clientesMaster.fcPrimerApellidoCliente);
    $("#primerApellidoCliente").attr('readonly', true);

    //llenar el campo con la info del cliente y dejarlo como solo lectura
    $("#segundoApellidoCliente").val(rowData.clientesMaster.fcSegundoApellidoCliente);
    $("#segundoApellidoCliente").attr('readonly', true);

    //llenar el campo con la info del cliente y dejarlo como solo lectura
    $("#identidadCliente").val(rowData.clientesMaster.fcIdentidadCliente);
    $("#identidadCliente").attr('readonly', true);

    $("#rtnCliente").val(rowData.clientesMaster.RTNCliente);
    $("#rtnCliente").attr('readonly', true);

    //llenar el campo con la info del cliente y dejarlo como solo lectura
    $("#numeroTelefono").val(rowData.clientesMaster.fcTelefonoCliente);
    $("#numeroTelefono").attr('readonly', true);

    $(".buscardorddl").select2("destroy");

    //llenar el campo con la info del cliente y dejarlo como solo lectura
    $("#nacionalidad").val(rowData.clientesMaster.fiNacionalidadCliente);
    $("#nacionalidad").attr('disabled', true);

    //llenar el campo fecha de nacimiento del cliente
    $("#fechaNacimiento").val(dateFormat(rowData.clientesMaster.fdFechaNacimientoCliente));
    $("#fechaNacimiento").attr('readonly', true);

    $('#edadCliente').val(rowData.clientesMaster.fiEdadCliente);
    $("#edadCliente").attr('readonly', true);

    //llenar correo electronico
    $("#correoElectronico").val(rowData.clientesMaster.fcCorreoElectronicoCliente);
    $("#correoElectronico").attr('readonly', true);

    $("#profesion").val(rowData.clientesMaster.fcProfesionOficioCliente);
    $("#profesion").attr('readonly', true);

    //$("#sexo").val(rowData.clientesMaster.fcSexoCliente);
    $("input[name=sexo][value=" + rowData.clientesMaster.fcSexoCliente + "]").prop('checked', true);
    $("input[name=sexo]").attr('disabled', true);

    //$("#estadoCivil").val(rowData.clientesMaster.fiIDEstadoCivil);
    $("input[name=estadoCivil][value=" + rowData.clientesMaster.fiIDEstadoCivil + "]").prop('checked', true);
    //$("input[name=estadoCivil]").attr('disabled', true);

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

    //informacion domiciliar
    var infoDomiciliar = rowData.ClientesInformacionDomiciliar;


    //DROPDOWNLISTS DE INFORMACION DOMICILIAR DEL CLIENTE
    $('#departamento').val(infoDomiciliar.fiIDDepto);
    $('#departamento').select2().trigger('change');

    var qString = "?" + window.location.href.split("?")[1];
    //cargar municipios
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarMunicipios" + qString,
        data: JSON.stringify({ CODDepto: infoDomiciliar.fiIDDepto }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar municipios de este departamento');
        },
        success: function (data) {

            var municipiosDelDepto = data.d;

            var municipioClienteDdl = $("#municipio");

            municipioClienteDdl.empty();

            $.each(municipiosDelDepto, function (i, iter) {
                municipioClienteDdl.append("<option " + (iter.fiIDMunicipio == infoDomiciliar.fiIDMunicipio ? 'selected' : '') + " value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
            });
            municipioClienteDdl.attr('disabled', false);
        }
    });

    //cargar ciudades
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarPoblados" + qString,
        data: JSON.stringify({ CODDepto: infoDomiciliar.fiIDDepto, CODMunicipio: infoDomiciliar.fiIDMunicipio }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar ciudades de este municipio');
        },
        success: function (data) {

            var ciudadesDelMunicipio = data.d;

            var ciudadDdl = $("#ciudad");

            ciudadDdl.empty();

            $.each(ciudadesDelMunicipio, function (i, iter) {
                ciudadDdl.append("<option " + (iter.fiIDCiudad == infoDomiciliar.fiIDCiudad ? 'selected' : '') + " value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
            });
            ciudadDdl.attr('disabled', false);
        }
    });

    //cargar Barrio
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarBarrios" + qString,
        data: JSON.stringify({ CODDepto: infoDomiciliar.fiIDDepto, CODMunicipio: infoDomiciliar.fiIDMunicipio, CODPoblado: infoDomiciliar.fiIDCiudad }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar ciudades de este municipio');
        },
        success: function (data) {

            var barriosDeLaCiudad = data.d;

            var BarrioColoniaDdl = $("#barrioColonia");

            BarrioColoniaDdl.empty();

            $.each(barriosDeLaCiudad, function (i, iter) {
                BarrioColoniaDdl.append("<option " + (iter.fiIDBarrioColonia == infoDomiciliar.fiIDBarrioColonia ? 'selected' : '') + " value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
            });
            BarrioColoniaDdl.attr('disabled', false);
        }
    });

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

    //DROPDOWNLISTS DE INFORMACION UBICACION DE LA EMPRESA DONDE LABORA EL CLIENTE

    var infoLaboral = rowData.ClientesInformacionLaboral;
    
    $('#departamentoEmpresa').val(infoLaboral.fiIDDepto);
    $('#departamentoEmpresa').select2().trigger('change');
    //cargar municipios
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarMunicipios" + qString,
        data: JSON.stringify({ CODDepto: infoLaboral.fiIDDepto }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar municipios de este departamento');
        },
        success: function (data) {

            var municipiosDelDeptoEmpresa = data.d;

            var municipioEmpresaClienteDdl = $("#municipioEmpresa");

            municipioEmpresaClienteDdl.empty();

            $.each(municipiosDelDeptoEmpresa, function (i, iter) {
                municipioEmpresaClienteDdl.append("<option " + (iter.fiIDMunicipio == infoDomiciliar.fiIDMunicipio ? 'selected' : '') + " value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
            });
            municipioEmpresaClienteDdl.attr('disabled', false);
        }
    });

    //cargar ciudades
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarPoblados" + qString,
        data: JSON.stringify({ CODDepto: infoLaboral.fiIDDepto, CODMunicipio: infoLaboral.fiIDMunicipio }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar ciudades de este municipio');
        },
        success: function (data) {

            debugger;

            var ciudadesDelMunicipioEmpresa = data.d;

            var ciudadEmpresaClienteDdl = $("#ciudadEmpresa");

            ciudadEmpresaClienteDdl.empty();

            $.each(ciudadesDelMunicipioEmpresa, function (i, iter) {
                ciudadEmpresaClienteDdl.append("<option " + (iter.fiIDCiudad == infoLaboral.fiIDCiudad ? 'selected' : '') + " value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
            });
            ciudadEmpresaClienteDdl.attr('disabled', false);
        }
    });

    //cargar Barrio
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarBarrios" + qString,
        data: JSON.stringify({ CODDepto: infoLaboral.fiIDDepto, CODMunicipio: infoLaboral.fiIDMunicipio, CODPoblado: infoLaboral.fiIDCiudad }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar ciudades de este municipio');
        },
        success: function (data) {

            debugger;

            var barriosDeLaCiudadEmpresa = data.d;

            var BarrioColoniaEmpresaClienteDdl = $("#barrioColoniaEmpresa");

            BarrioColoniaEmpresaClienteDdl.empty();

            $.each(barriosDeLaCiudadEmpresa, function (i, iter) {
                BarrioColoniaEmpresaClienteDdl.append("<option " + (iter.fiIDBarrioColonia == infoLaboral.fiIDBarrioColonia ? 'selected' : '') + " value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
            });
            BarrioColoniaEmpresaClienteDdl.attr('disabled', false);
        }
    });

    $("#direccionDetalladaEmpresa").val(rowData.ClientesInformacionLaboral.fcDireccionDetalladaEmpresa);
    $("#direccionDetalladaEmpresa").attr('readonly', true);

    $("#referenciaDireccionDetalladaEmpresa").val(rowData.ClientesInformacionLaboral.fcReferenciasDireccionDetallada);
    $("#referenciaDireccionDetalladaEmpresa").attr('readonly', true);

    $("#fuenteOtrosIngresos").val(rowData.ClientesInformacionLaboral.fcFuenteOtrosIngresos);
    $("#fuenteOtrosIngresos").attr('readonly', true);

    $("#valorOtrosIngresos").val(rowData.ClientesInformacionLaboral.fiValorOtrosIngresosMensuales);
    $("#valorOtrosIngresos").attr('readonly', true);

    var requiereInformacionConyugal = $("#estadoCivil option:selected").data('info');

    if (requiereInformacionConyugal == false) {
        $('input.infoConyugal').attr('disabled', true);
    }
    else if (requiereInformacionConyugal == true) {
        $('input.infoConyugal').attr('disabled', false);
    }

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

    if (rowData.ClientesReferenciasPersonales != null) {

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
    }

    estadoFuncionRecuperarInfoCliente = true;
    if (estadoFuncionLlenarDDL == true) {
        $("#spinnerCargando").css('display', 'none');
    }

    $("#btnNuevaReferencia").prop('disabled', true);

    $("#spinnerCargando").css('display', 'none');

}


var CODDepto = 0;
$("#departamento").change(function () {

    $(this).parsley().validate();

    //obtener id del departamento seleccionado
    var idDepto = $("#departamento option:selected").val();
    CODDepto = idDepto;

    var municipioDdl = $("#municipio");
    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idDepto != '') {

        $("#spinnerCargando").css('display', '');

        //vaciar dropdownlist de municipios
        municipioDdl.empty();

        //agregar opciones
        municipioDdl.append("<option value=''>Seleccione una opción</option>");

        var qString = "?" + window.location.href.split("?")[1];

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarMunicipios" + qString,
            data: JSON.stringify({ CODDepto: idDepto }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar municipios de este departamento');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {

                var municipiosDelDepto = data.d;

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

                $("#spinnerCargando").css('display', 'none');
            }
        });
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


var CODMunicipio = 0;
$("#municipio").change(function () {

    $(this).parsley().validate();

    //obtener id del municipio seleccionado
    var idMunicipio = $("#municipio option:selected").val();
    CODMunicipio = idMunicipio;

    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idMunicipio != '') {

        $("#spinnerCargando").css('display', '');

        //vaciar dropdownlist
        ciudadDdl.empty();

        //agregar opciones
        ciudadDdl.append("<option value=''>Seleccione una opción</option>");

        var qString = "?" + window.location.href.split("?")[1];

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarPoblados" + qString,
            data: JSON.stringify({ CODDepto: CODDepto, CODMunicipio: CODMunicipio }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar ciudades de este municipio');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {

                var ciudadesDelMunicipio = data.d;

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

                $("#spinnerCargando").css('display', 'none');
            }
        });
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


var CODCiudad = 0;
$("#ciudad").change(function () {

    var idCiudad = $("#ciudad option:selected").val();
    CODCiudad = idCiudad;

    $(this).parsley().validate();

    var BarrioColoniaDdl = $("#barrioColonia");

    if (idCiudad != '') {

        $("#spinnerCargando").css('display', '');

        //vaciar dropdownlist
        BarrioColoniaDdl.empty();

        //agregar opciones
        BarrioColoniaDdl.append("<option value=''>Seleccione una opción</option>");

        var qString = "?" + window.location.href.split("?")[1];

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarBarrios" + qString,
            data: JSON.stringify({ CODDepto: CODDepto, CODMunicipio: CODMunicipio, CODPoblado: CODCiudad }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar barrios de esta ciudad');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {

                //agregar ciudades que pertenecen al municipio seleccionado
                var barriosDeLaCiudad = data.d;

                $.each(barriosDeLaCiudad, function (i, iter) {
                    BarrioColoniaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
                });

                //habilitar dropdownlist
                BarrioColoniaDdl.attr('disabled', false);

                $("#spinnerCargando").css('display', 'none');
            }
        });
    }
    else {
        //vaciar dropdownlist
        BarrioColoniaDdl.empty();

        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist
        BarrioColoniaDdl.attr('disabled', true);
    }
});


$("#barrioColonia").change(function () {
    $(this).parsley().validate();
});


var CODDeptoEmpresa = 0;
$("#departamentoEmpresa").change(function () {

    $(this).parsley().validate();

    //obtener id del departamento seleccionado
    var idDepto = $("#departamentoEmpresa option:selected").val();
    CODDeptoEmpresa = idDepto;

    var municipioEmpresaDdl = $("#municipioEmpresa");
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idDepto != '') {

        $("#spinnerCargando").css('display', '');

        //vaciar dropdownlist de municipios
        municipioEmpresaDdl.empty();

        //agregar opciones
        municipioEmpresaDdl.append("<option value=''>Seleccione una opción</option>");

        var qString = "?" + window.location.href.split("?")[1];

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarMunicipios" + qString,
            data: JSON.stringify({ CODDepto: CODDeptoEmpresa }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar municipios de este departamento');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {

                var municipiosDelDepto = data.d;

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

                $("#spinnerCargando").css('display', 'none');
            }
        });

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


var CODMunicipioEmpresa = 0;
$("#municipioEmpresa").change(function () {

    $(this).parsley().validate();

    //obtener id del municipio seleccionado
    var idMunicipio = $("#municipioEmpresa option:selected").val();
    CODMunicipioEmpresa = idMunicipio;
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idMunicipio != '') {

        $("#spinnerCargando").css('display', '');

        //vaciar dropdownlist
        ciudadaEmpresaDdl.empty();

        //agregar opciones
        ciudadaEmpresaDdl.append("<option value=''>Seleccione una opción</option>");

        var qString = "?" + window.location.href.split("?")[1];

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarPoblados" + qString,
            data: JSON.stringify({ CODDepto: CODDeptoEmpresa, CODMunicipio: CODMunicipioEmpresa }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar ciudades de este municipio');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {

                //agregar ciudades que pertenecen al municipio seleccionado
                var ciudadesDelMunicipio = data.d;

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

                $("#spinnerCargando").css('display', 'none');
            }
        });
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


var CODCiudadEmpresa = 0;
$("#ciudadEmpresa").change(function () {

    $(this).parsley().validate();

    //obtener id del municipio seleccionado
    var idCiudad = $("#ciudadEmpresa option:selected").val();
    CODCiudadEmpresa = idCiudad;
    var barriosEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idCiudad != '') {

        $("#spinnerCargando").css('display', '');

        //vaciar dropdownlist
        barriosEmpresaDdl.empty();

        //agregar opciones
        barriosEmpresaDdl.append("<option value=''>Seleccione una opción</option>");

        var qString = "?" + window.location.href.split("?")[1];

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_ActualizarSolicitud.aspx/CargarBarrios" + qString,
            data: JSON.stringify({ CODDepto: CODDeptoEmpresa, CODMunicipio: CODMunicipioEmpresa, CODPoblado: CODCiudadEmpresa }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeError('Error al cargar barrios de esta ciudad');
                $("#spinnerCargando").css('display', 'none');
            },
            success: function (data) {

                //agregar ciudades que pertenecen al municipio seleccionado
                var barriosDeLaCiudad = data.d;

                $.each(barriosDeLaCiudad, function (i, iter) {
                    barriosEmpresaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
                });

                //habilitar dropdownlist
                barriosEmpresaDdl.attr('disabled', false);
                $("#spinnerCargando").css('display', 'none');
            }
        });
    }
    else {
        //vaciar dropdownlist
        barriosEmpresaDdl.empty();

        barriosEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");

        //deshabilitar dropdownlist
        barriosEmpresaDdl.attr('disabled', true);
    }
});


$("#barrioColoniaEmpresa").change(function () {
    $(this).parsley().validate();
});


$('input:radio[name="estadoCivil"]').change(function () {

    var requiereInformacionConyugal = $("input[name='estadoCivil']:checked").data('info');

    //si no se requiere información conyugal, deshabilitar ese formulario
    if (requiereInformacionConyugal == false) {
        $('input.infoConyugal').attr('disabled', true);
        $('#smartwizard').smartWizard("stepState", [4], "hide");
    }
    else if (requiereInformacionConyugal == true) {
        $('input.infoConyugal').attr('disabled', false);
        $('#smartwizard').smartWizard("stepState", [4], "show");
        $("#pestanaInfConyugal").css('display', '');
    }
});


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


// agregar referencias personales (abrir modal)
$("#btnNuevaReferencia").click(function () {

    // vaciar todos los inputs del formulario
    resetForm($('#addReferencia-form'));

    //inicializar validaciones de los formularios
    $('#addReferencia-form').parsley().reset();

});


// agregar referencias personales (submit)
var cantidadReferencias = 0;
var listaClientesReferencias = [];
$('#addReferencia-form').submit(function (e) {

    //validar formulario de agregar referencia personal
    if ($(this).parsley().isValid()) {

        //evitar postback del formulario
        e.preventDefault();

        //obtener el valor ingresado en la cajas de texto
        var nombreref = $("#nombreCompletoRef").val();
        var lugarTrabajoRef = $("#lugarTrabajoRef").val();
        var tiempoConocerRefRow = parseInt($("#tiempoConocerRef").val()) <= 2 ? $("#tiempoConocerRef").val() + ' años' : 'Más de 2 años';
        var tiempoConocerRef = parseInt($("#tiempoConocerRef").val());
        var telefonoRef = $("#telefonoRef").val();
        var parentescoRef = $("#parentescoRef").val();
        var parentescoText = $("#parentescoRef :selected").text();

        //agregar referencia a la tabla de referencias personales
        tableReferencias.row.add([
            nombreref,
            lugarTrabajoRef,
            tiempoConocerRefRow,
            telefonoRef,
            parentescoText,
        ]).draw(false);

        //ocultar modal de agregar referencia
        $("#modalAddReferencia").modal('hide');

        //incrementar variable contadora de referencias personales para validaciones
        cantidadReferencias = cantidadReferencias + 1;

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

        //agregar objeto referencia a la lista de referencias personales que se enviará al servidor
        listaClientesReferencias.push(referencia);

    }
});

function pad2(number) {
    return (number < 10 ? '0' : '') + number
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
function resetForm($form) {
    $form.find('input:text, input:password, input:file,input[type="date"],input[type="email"], select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .prop('checked', false).removeAttr('selected');
}