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

        $('#frmSolicitud').parsley().validate({group: 'informacionLaboral',force: true});
        var modelStateFormLaboral = $('#frmSolicitud').parsley().isValid({ group: 'informacionLaboral' });

        var modelStateFormConyugal = true;
        if ($("input[name='estadoCivil']:checked").data('info') == true) {

            $('#frmSolicitud').parsley().validate({ group: 'informacionConyugal', force: true});
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
            var ValorVehiculo = $("#tipoPrestamo").data('cod') == '101' ? '0' : $("#txtMontoPrestamo").val().replace(/,/g, '');
            var valorPrima = $("#tipoPrestamo").data('cod') == '101' ? '0' : $("#txtMontoPrima").val().replace(/,/g, '') == '' ? 0 : $("#txtMontoPrima").val().replace(/,/g, '');
            var ValorPmo = $("#txtMontoPrestamo").val().replace(/,/g, '') - valorPrima;
            var SolicitudesMaster = {
                fiIDTipoPrestamo: $("#tipoPrestamo").data('cod'),
                fcTipoSolicitud: $("#tipoSolicitud").val(),
                fiIDCliente: clienteID,
                fdValorPmoSugeridoSeleccionado: ValorPmo,
                fiPlazoPmoSeleccionado: $("#txtPlazoPrestamo").val(),
                fnValorGarantia: ValorVehiculo,
                fnPrima: valorPrima,
                IDTipoMoneda: $("#TipoMoneda option:selected").val(),
                fiIDOrigen: $("#ddlOrigen option:selected").val() == null ? 1 : parseInt($("#ddlOrigen option:selected").val())
            };
            var bitacora = {
                fdEnIngresoInicio: localStorage.getItem("EnIngresoInicio")
            };
            var ClienteMaster = {
                fiIDCliente: clienteID,
                IDTipoCliente: $("#TipodeCliente option:selected").val(),
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
                
                fiIDDepto: $("#departamentoEmpresa :selected").val(),
                fiIDMunicipio: $("#municipioEmpresa :selected").val(),
                fiIDCiudad: $("#ciudadEmpresa :selected").val(),
                fiIDBarrioColonia: $("#barrioColoniaEmpresa :selected").val(),

                fcDireccionDetalladaEmpresa: $("#direccionDetalladaEmpresa").val(),
                fcReferenciasDireccionDetallada: $("#referenciaDireccionDetalladaEmpresa").val(),
                fcFuenteOtrosIngresos: $("#fuenteOtrosIngresos").val(),
                fiValorOtrosIngresosMensuales: $("#valorOtrosIngresos").val().replace(/,/g, '') == '' ? 0 : $("#valorOtrosIngresos").val().replace(/,/g, ''),
            };
            var ClientesInformacionDomiciliar = {
                fiIDDepto: $("#departamento :selected").val(),
                fiIDMunicipio: $("#municipio :selected").val(),
                fiIDCiudad: $("#ciudad :selected").val(),
                fiIDBarrioColonia: $("#barrioColonia :selected").val(),
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
                fcIngresosMensualesConyugue: $("#ingresoMensualesConyugue").val().replace(/,/g, '') == '' ? 0 : $("#ingresoMensualesConyugue").val().replace(/,/g, ''),
                fcTelefonoTrabajoConyugue: $("#telefonoTrabajoConyugue").val()
            };
            ClientesReferencias = listaClientesReferencias;
            var qString = "?" + window.location.href.split("?")[1];
            $.ajax({
                type: "POST",
                url: 'SolicitudesCredito_Registrar.aspx/IngresarSolicitud' + qString,
                data: JSON.stringify({
                    clienteNuevo: clienteNuevo,
                    SolicitudesMaster: SolicitudesMaster,
                    ClienteMaster: ClienteMaster,
                    ClientesInformacionLaboral: ClientesInformacionLaboral,
                    ClientesInformacionDomiciliar: ClientesInformacionDomiciliar,
                    ClientesInformacionConyugal: ClientesInformacionConyugal,
                    ClientesReferencias: ClientesReferencias,
                    bitacora: bitacora
                }),
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
                    } else {
                        MensajeError(data.d.message);
                    }
                }
            });
        }
    });
/* inicalizar el formulario por pasos */
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
    lang: {
        next: 'Siguiente',
        previous: 'Anterior'
    }
});
var clienteNuevo = true;
var ListaMunicipios = [];
var ListaCiudades = [];
var ListaBarriosColonias = [];
var objPrecalificado = [];
cargarPrecalificado();
var ingresoInicio = '';
var estadoFuncionLlenarDDL = false;
var estadoFuncionRecuperarInfoCliente = false;
var estadoFuncionRecuperarRespaldos = false;

$(document).ready(function () {

    // Cuando se muestre un step
    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {

        if (stepPosition === 'first') { // si es el primer paso, deshabilitar el boton "anterior"
            $("#prev-btn").addClass('disabled').css('display', 'none');
        } else if (stepPosition === 'final') { // si es el ultimo paso, deshabilitar el boton siguiente
            $("#next-btn").addClass('disabled');
            $("#btnGuardarSolicitud").css('display', '');
        } else { // si no es ninguna de las anteriores, habilitar todos los botones
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
            $("#btnGuardarSolicitud").css('display', 'none');
        }
        if (stepNumber == 4) {
            $('#frmSolicitud').parsley().reset();
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
        if (stepDirection == 'forward') { // validar solo si se quiere ir hacia el siguiente paso

            if (stepNumber == 0) { // validar informacion prestamo
                var state = $('#frmSolicitud').parsley().isValid({ group: 'informacionPrestamo', excluded: ':disabled' });
                if (state == true) {
                    guardarRespaldoInformacionPrestamo(); //si el formulario es valido, guardar respaldo en el localstorage
                } else {
                    $('#frmSolicitud').parsley().validate({ group: 'informacionPrestamo', excluded: ':disabled', force: true }); //si no es valido, mostrar validaciones al usuario
                }
                var TipoPrestamo = $("#tipoPrestamo").data('cod');
                var ValorPrestamo = parseFloat($("#txtMontoPrestamo").val().replace(/,/g, '')).toFixed(2);
                var MontoPrima = parseFloat($("#txtMontoPrima").val().replace(/,/g, '')).toFixed(2);
                var MontoFinanciar = ValorPrestamo - MontoPrima;
                if (TipoPrestamo != '101') {
                    if (parseInt(MontoPrima) > ValorPrestamo) {
                        MensajeError('Prima mayor a valor del vehiculo');
                    }
                    if (TipoPrestamo == '201') { //prima minima de 10% en caso de ser moto o efectivo
                        var primaMinima = (ValorPrestamo * 10) / 100;
                        if (MontoPrima < primaMinima) {
                            MensajeError('Prima minima de 10%');
                        }
                    }
                }
                if (TipoPrestamo == '201' || TipoPrestamo == '101') { //monto minimo a financiar en caso de ser moto o efectivo
                    if (MontoFinanciar < 6000.00) {
                        MensajeError('Monto minimo a financiar es 6,000');
                    }
                }
                return state;
            }

            if (stepNumber == 1) { //validar informacion personal
                var state = $('#frmSolicitud').parsley().isValid({
                    group: 'informacionPersonal'
                });
                if (state == true) {
                    guardarRespaldoInformacionPersonal(); //si el formulario es valido, guardar respaldo en el localstorage
                } else {
                    $('#frmSolicitud').parsley().validate({
                        group: 'informacionPersonal',
                        force: true
                    });
                }
                return state;
            }

            if (stepNumber == 2) { //validar informacion domiciliar
                var state = $('#frmSolicitud').parsley().isValid({
                    group: 'informacionDomiciliar'
                });
                if (state == true) {
                    guardarRespaldoInformacionDomiciliar();
                } else {
                    $('#frmSolicitud').parsley().validate({
                        group: 'informacionDomiciliar',
                        force: true
                    });
                }
                return state;
            }

            if (stepNumber == 3) { //validar informacion laboral
                var state = $('#frmSolicitud').parsley().isValid({
                    group: 'informacionLaboral'
                });
                if (state == true) {
                    guardarRespaldoInformacionLaboral();
                } else {
                    $('#frmSolicitud').parsley().validate({
                        group: 'informacionLaboral',
                        force: true
                    });
                }
                return state;
            }

            if (stepNumber == 4) { //validar informacion conyugal
                if ($("input[name='estadoCivil']:checked").data('info') == true) {
                    var state = $('#frmSolicitud').parsley().isValid({
                        group: 'informacionConyugal'
                    });
                    if (state == true) {
                        guardarRespaldoInformacionConyugal();
                    } else {
                        $('#frmSolicitud').parsley().validate({
                            group: 'informacionConyugal',
                            force: true
                        });
                    }
                    return state;
                }
            }

            if (stepNumber == 5) { //validar referencias personales
                var state = true;
                if (listaClientesReferencias != null) { //si la lista de referencias no es nula y contiene una o mas referencias, guardar un respaldo de estas
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
        } //termina if fowards
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
    if (localStorage.getItem("EnIngresoInicio") == null || localStorage.getItem("EnIngresoInicio") == "undefined" || localStorage.getItem("EnIngresoInicio") == undefined) {
        obtenerFechaActual();
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

/* agregar referencias personales (abrir modal) */
$("#btnNuevaReferencia").click(function () {
    resetForm($('#addReferencia-form'));
    $('#addReferencia-form').parsley().reset();
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
        $("#modalAddReferencia").modal('hide');
        cantidadReferencias = cantidadReferencias + 1;//incrementar variable contadora de referencias personales para validaciones
        var referencia = {//objeto referencia
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

/* inicializar datatable de referencias personales del cliente */
var tableReferencias = $('#datatable-buttons').DataTable({
    "searching": false,
    "lengthChange": false,
    "pageLength": 4,
    "paging": true,
    "responsive": false,
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

function LlenarListas() {
    estadoFuncionLlenarDDL = false;
    $("#spinnerCargando").css('display', '');
    $("select").empty();
    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarListas" + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            var monedasDdl = $("#TipoMoneda");
            monedasDdl.append("<option value=''>Seleccionar</option>");
            $.each(data.d.Monedas, function (i, iter) {
                monedasDdl.append("<option value='" + iter.IDTipoMoneda + "'>" + iter.TipoMoneda + "</option>");
            });

            var tipoClienteDdl = $("#TipodeCliente");
            tipoClienteDdl.append("<option value=''>Seleccionar</option>");
            $.each(data.d.TipoCliente, function (i, iter) {
                tipoClienteDdl.append("<option value='" + iter.IDTipoCliente + "'>" + iter.TipoCliente + "</option>");
            });

            var nacionalidadDdl = $("#nacionalidad");
            nacionalidadDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Nacionalidades, function (i, iter) {
                nacionalidadDdl.append("<option value='" + iter.fiIDNacionalidad + "'>" + iter.fcDescripcionNacionalidad + "</option>");
            });

            var divEstadoCivil = $("#divEstadoCivil");
            $.each(data.d.EstadosCiviles, function (i, iter) {
                divEstadoCivil.append("<div class='form-check form-check-inline'>" +
                    "<input data-info='" + iter.fbRequiereInformacionConyugal + "' class='form-check-input' type='radio' name ='estadoCivil' value='" + iter.fiIDEstadoCivil + "'>" +
                    "<label class='form-check-label'>" + iter.fcDescripcionEstadoCivil + "</label>" +
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

            $("#municipio").append("<option value=''>Seleccione un depto.</option>");// llenar lista de municipios 
            ListaMunicipios = [];

            $("#ciudad").append("<option value=''>Seleccione un municipio</option>");// llenar lista de ciudades
            ListaCiudades = [];

            $("#barrioColonia").append("<option value=''>Seleccione una ciudad</option>");// llenar lista de barrios y colonias 
            ListaBarriosColonias = [];
            
            var departamentoEmpresaDdl = $("#departamentoEmpresa"); // llenar lista de departamentos de cliente informacion laboral
            departamentoEmpresaDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Departamentos, function (i, iter) {
                departamentoEmpresaDdl.append("<option value='" + iter.fiIDDepto + "'>" + iter.fcNombreDepto + "</option>");
            });

            $("#municipioEmpresa").append("<option value=''>Seleccione un depto</option>");// llenar lista de municipios de cliente informacion laboral
            $("#ciudadEmpresa").append("<option value=''>Seleccione un municipio</option>"); // llenar lista de ciudades de cliente informacion laboral
            $("#barrioColoniaEmpresa").append("<option value=''>Seleccione una ciudad</option>"); // llenar lista de barrios y colonias de cliente informacion laboral            

            var parentescoRefDdl = $("#parentescoRef");
            parentescoRefDdl.append("<option value=''>Seleccione una opción</option>");
            $.each(data.d.Parentescos, function (i, iter) {
                parentescoRefDdl.append("<option value='" + iter.fiIDParentescos + "'>" + iter.fcDescripcionParentesco + "</option>");// llenar listado de referencias personales en el modal de agregar referencia personal del cliente
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

            /* TERMINA CARGA DE LISTADOS */
            estadoFuncionLlenarDDL = true;
            VerificarExistenciaCliente();
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
        url: "SolicitudesCredito_Registrar.aspx/CargarOrigenes",
        data: JSON.stringify({ COD: COD }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar catalogo de orígenes');
            if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
                $("#spinnerCargando").css('display', 'none');
            }
            $("#ddlOrigen").prop('disabled', true);
        },
        success: function (data) {

            if (data.d != null) {
                var origenesDdl = $("#ddlOrigen");
                origenesDdl.empty();
                origenesDdl.append("<option value=''>Seleccione una opción</option>");
                var listaOrigenes = data.d;
                $.each(listaOrigenes, function (i, iter) {
                    origenesDdl.append("<option value='" + iter.fiIDOrigen + "'>" + iter.fcOrigen + "</option>"); // llenar listado de origenes en informacion del prestamo
                });
                $("#spinnerCargando").css('display', 'none');
                $("#ddlOrigen,#titleOrigen").css('display', '');
                $("#ddlOrigen").prop('disabled', false);
            }
            else {
                if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
                    $("#spinnerCargando").css('display', 'none');
                }
            }
        }
    });
}

function VerificarExistenciaCliente() {

    estadoFuncionRecuperarInfoCliente = false;
    $("#spinnerCargando").css('display', '');
    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/ObtenerInformacionCliente" + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al verificar existencia del cliente');
            estadoFuncionCargarInformacionCliente = true;
            if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
                $("#spinnerCargando").css('display', 'none');
            }
        },
        success: function (data) {
            if (data.d.clientesMaster != null) {
                $("#spinnerCargando").css('display', '');
                var infoCompletaCliente = data.d;
                cargarInformacionCompletaDelCliente(infoCompletaCliente); // invocar metodo que cargara la informacion del cliente
            }
            else {
                estadoFuncionCargarInformacionCliente = true;
                if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
                    $("#spinnerCargando").css('display', 'none');
                }
            }
        }
    });
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

//ID CLTE
var clienteID = 0;
function cargarPrecalificado() {
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/GetDetallesPrecalificado",
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar información del precalificado');
        },
        success: function (data) {
            objPrecalificado = data.d;
            LlenarListas();

            /* Informacion del precalificado */
            $("#tipoPrestamo,#btnBuscarCliente,#rtnCliente,#identidadCliente,#primerNombreCliente,#SegundoNombreCliente,#primerApellidoCliente,#segundoApellidoCliente,#ingresosPrecalificado,#ingresosMensuales,#numeroTelefono,#telefonoMovil,#fechaNacimiento").prop('disabled', true);
            $("#tipoPrestamo").val(objPrecalificado.Producto);
            $("#tipoPrestamo").data('cod', objPrecalificado.tipoProducto);
            $("#lblPlazoPMO").text('Plazo ' + objPrecalificado.cotizadorProductos[0].TipoCuota);
            $("#lblCuotaMaxima").text('Cuota ' + objPrecalificado.cotizadorProductos[0].TipoCuota);

            if ($("#tipoPrestamo").data('cod') != '101') { //si no es prestamo en efectivo
                cargarOrigenes($("#tipoPrestamo").data('cod')); //cargar origenes
            }
            $("#ingresosMensuales").val(objPrecalificado.ingresos);
            $("#telefonoMovil").val(objPrecalificado.telefono);
            $("#fechaNacimiento").val(dateFormat(objPrecalificado.fechaNacimiento));
            var FechaNac = new Date(parseInt(objPrecalificado.fechaNacimiento.replace("/Date(", "").replace(")/", ""), 10));
            var today = new Date();
            var edad = Math.floor((today - FechaNac) / (365.25 * 24 * 60 * 60 * 1000));
            $('#edadCliente').val(edad + ' años');
            $('#edadCliente').prop('disabled', true);

            // mostrar monto a maximo sugerido del precalificado
            var listadoCotizaciones = objPrecalificado.cotizadorProductos;
            var DDLMontosSugeridos = $("#pmoSugeridoSeleccionado");
            DDLMontosSugeridos.empty();
            DDLMontosSugeridos.append("<option selected value='" + listadoCotizaciones[0].fnMontoOfertado + "'>" + listadoCotizaciones[0].fnMontoOfertado + "</option>");
            $("#plazoPmoSeleccionado").val(listadoCotizaciones[0].fiPlazo);
            $("#cutoaQuinceal").val(listadoCotizaciones[0].fnCuotaQuincenal);
            for (var i = 1; i < listadoCotizaciones.length; i++) {
                DDLMontosSugeridos.append("<option value='" + listadoCotizaciones[i].fnMontoOfertado + "'>" + listadoCotizaciones[i].fnMontoOfertado + "</option>");
            }
        }
    });
}

//funcion para llenar el formulario con la informacion de un cliente existente
function cargarInformacionCompletaDelCliente(informacionCliente) {

    $("#spinnerCargando").css('display', '');
    estadoFuncionRecuperarInfoCliente = false;
    rowData = informacionCliente;
    clienteNuevo = false;
    clienteID = rowData.clientesMaster.fiIDCliente;

    $(".buscardorddl").select2("destroy");
    $("#nacionalidad").val(rowData.clientesMaster.fiNacionalidadCliente);
    $("#fechaNacimiento").val(dateFormat(rowData.clientesMaster.fdFechaNacimientoCliente));
    var FechaNac = new Date(parseInt(objPrecalificado.fechaNacimiento.replace("/Date(", "").replace(")/", ""), 10));
    var today = new Date();
    var edad = Math.floor((today - FechaNac) / (365.25 * 24 * 60 * 60 * 1000));
    $('#edadCliente').val(edad + ' años');
    $("#edadCliente").attr('readonly', true);
    $("#correoElectronico").val(rowData.clientesMaster.fcCorreoElectronicoCliente);
    $("#rtnCliente").val(rowData.clientesMaster.RTNCliente);
    $("#profesion").val(rowData.clientesMaster.fcProfesionOficioCliente);
    $("input[name=sexo][value=" + rowData.clientesMaster.fcSexoCliente + "]").prop('checked', true);
    $("input[name=sexo]").prop('disabled', true);
    $("input[name=estadoCivil][value=" + rowData.clientesMaster.fiIDEstadoCivil + "]").prop('checked', true);
    $("#vivivenda").val(rowData.clientesMaster.fiIDVivienda);
    $("input[name=tiempoResidir][value=" + rowData.clientesMaster.fiTiempoResidir + "]").prop('checked', true);
    $("#nacionalidad,#fechaNacimiento,#profesion").attr('disabled', true);
    $("#departamento").val(rowData.ClientesInformacionDomiciliar.fiIDDepto);
    $('#departamento').select2().trigger('change');

    var qString = "?" + window.location.href.split("?")[1];
    //cargar municipios
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarMunicipios" + qString,
        data: JSON.stringify({ CODDepto: rowData.ClientesInformacionDomiciliar.fiIDDepto }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar municipios de este departamento');
        },
        success: function (data) {
            var municipiosDelDepto = data.d;
            var municipioClienteDdl = $("#municipio");
            municipioClienteDdl.empty();
            $.each(municipiosDelDepto, function (i, iter) {
                municipioClienteDdl.append("<option " + (iter.fiIDMunicipio == rowData.ClientesInformacionDomiciliar.fiIDMunicipio ? 'selected' : '') + " value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
            });
            municipioClienteDdl.attr('disabled', false);
        }
    });
    //cargar ciudades
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarPoblados" + qString,
        data: JSON.stringify({ CODDepto: rowData.ClientesInformacionDomiciliar.fiIDDepto, CODMunicipio: rowData.ClientesInformacionDomiciliar.fiIDMunicipio }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar ciudades de este municipio');
        },
        success: function (data) {
            var ciudadesDelMunicipio = data.d;
            var ciudadDdl = $("#ciudad");
            ciudadDdl.empty();
            $.each(ciudadesDelMunicipio, function (i, iter) {
                ciudadDdl.append("<option " + (iter.fiIDCiudad == rowData.ClientesInformacionDomiciliar.fiIDCiudad ? 'selected' : '') + " value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
            });
            ciudadDdl.attr('disabled', false);
        }
    });
    //cargar Barrio
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarBarrios" + qString,
        data: JSON.stringify({ CODDepto: rowData.ClientesInformacionDomiciliar.fiIDDepto, CODMunicipio: rowData.ClientesInformacionDomiciliar.fiIDMunicipio, CODPoblado: rowData.ClientesInformacionDomiciliar.fiIDCiudad }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar ciudades de este municipio');
        },
        success: function (data) {
            var barriosDeLaCiudad = data.d;
            var BarrioColoniaDdl = $("#barrioColonia");
            BarrioColoniaDdl.empty();
            $.each(barriosDeLaCiudad, function (i, iter) {
                BarrioColoniaDdl.append("<option " + (iter.fiIDBarrioColonia == rowData.ClientesInformacionDomiciliar.fiIDBarrioColonia ? 'selected' : '') + " value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
            });
            BarrioColoniaDdl.attr('disabled', false);
        }
    });
    $("#telefonoCasa").val(rowData.ClientesInformacionDomiciliar.fcTelefonoCasa);
    $("#telefonoMovil").val(rowData.clientesMaster.fcTelefonoCliente);
    $("#direccionDetallada").val(rowData.ClientesInformacionDomiciliar.fcDireccionDetallada);
    $("#referenciaDireccionDetallada").val(rowData.ClientesInformacionDomiciliar.fcReferenciasDireccionDetallada);


    // INFORMACION LABORAL
    $("#nombreDelTrabajo").val(rowData.ClientesInformacionLaboral.fcNombreTrabajo);
    $("#ingresosMensuales").val(rowData.ClientesInformacionLaboral.fiIngresosMensuales);
    $("#puestoAsignado").val(rowData.ClientesInformacionLaboral.fcPuestoAsignado);
    $("#fechaIngreso").val(dateFormat(rowData.ClientesInformacionLaboral.fcFechaIngreso));
    $("#telefonoEmpresa").val(rowData.ClientesInformacionLaboral.fdTelefonoEmpresa);
    $("#extensionRRHH").val(rowData.ClientesInformacionLaboral.fcExtensionRecursosHumanos);
    $("#extensionCliente").val(rowData.ClientesInformacionLaboral.fcExtensionCliente);
    $("#ingresosMensuales").val(rowData.ClientesInformacionLaboral.fiIngresosMensuales);
    $("#puestoAsignado").val(rowData.ClientesInformacionLaboral.fcPuestoAsignado);
    // informacion laboral
    $("#departamentoEmpresa").val(rowData.ClientesInformacionLaboral.fiIDDepto);
    $('#departamentoEmpresa').select2().trigger('change');
    //cargar municipios
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarMunicipios" + qString,
        data: JSON.stringify({ CODDepto: rowData.ClientesInformacionLaboral.fiIDDepto }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar municipios de este departamento');
        },
        success: function (data) {
            var municipiosDelDeptoEmpresa = data.d;
            var municipioEmpresaClienteDdl = $("#municipioEmpresa");
            municipioEmpresaClienteDdl.empty();
            $.each(municipiosDelDeptoEmpresa, function (i, iter) {
                municipioEmpresaClienteDdl.append("<option " + (iter.fiIDMunicipio == rowData.ClientesInformacionLaboral.fiIDMunicipio ? 'selected' : '') + " value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
            });
            municipioEmpresaClienteDdl.attr('disabled', false);
        }
    });
    //cargar ciudades
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarPoblados" + qString,
        data: JSON.stringify({ CODDepto: rowData.ClientesInformacionLaboral.fiIDDepto, CODMunicipio: rowData.ClientesInformacionLaboral.fiIDMunicipio }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar ciudades de este municipio');
        },
        success: function (data) {
            var ciudadesDelMunicipioEmpresa = data.d;
            var ciudadEmpresaClienteDdl = $("#ciudadEmpresa");
            ciudadEmpresaClienteDdl.empty();
            $.each(ciudadesDelMunicipioEmpresa, function (i, iter) {
                ciudadEmpresaClienteDdl.append("<option " + (iter.fiIDCiudad == rowData.ClientesInformacionLaboral.fiIDCiudad ? 'selected' : '') + " value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
            });
            ciudadEmpresaClienteDdl.attr('disabled', false);
        }
    });
    //cargar Barrio
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/CargarBarrios" + qString,
        data: JSON.stringify({ CODDepto: rowData.ClientesInformacionLaboral.fiIDDepto, CODMunicipio: rowData.ClientesInformacionLaboral.fiIDMunicipio, CODPoblado: rowData.ClientesInformacionLaboral.fiIDCiudad }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar ciudades de este municipio');
        },
        success: function (data) {
            var barriosDeLaCiudadEmpresa = data.d;
            var BarrioColoniaEmpresaClienteDdl = $("#barrioColoniaEmpresa");
            BarrioColoniaEmpresaClienteDdl.empty();
            $.each(barriosDeLaCiudadEmpresa, function (i, iter) {
                BarrioColoniaEmpresaClienteDdl.append("<option " + (iter.fiIDBarrioColonia == rowData.ClientesInformacionLaboral.fiIDBarrioColonia ? 'selected' : '') + " value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
            });
            BarrioColoniaEmpresaClienteDdl.attr('disabled', false);
        }
    });
    $("#direccionDetalladaEmpresa").val(rowData.ClientesInformacionLaboral.fcDireccionDetalladaEmpresa);
    $("#referenciaDireccionDetalladaEmpresa").val(rowData.ClientesInformacionLaboral.fcReferenciasDireccionDetallada);
    $("#fuenteOtrosIngresos").val(rowData.ClientesInformacionLaboral.fcFuenteOtrosIngresos);
    $("#valorOtrosIngresos").val(rowData.ClientesInformacionLaboral.fiValorOtrosIngresosMensuales);

    //INFORMACION CONYUGAL
    if (rowData.ClientesInformacionConyugal != null) {

        $("#nombresConyugue").val('');
        if (rowData.ClientesInformacionConyugal.fcNombreCompletoConyugue != null) {
            $("#nombresConyugue").val(rowData.ClientesInformacionConyugal.fcNombreCompletoConyugue.toString().split(' ').slice(0, -1).join(' '));
        }
        $("#apellidosConyugue").val('');
        if (rowData.ClientesInformacionConyugal.fcNombreCompletoConyugue != null) {
            $("#apellidosConyugue").val(rowData.ClientesInformacionConyugal.fcNombreCompletoConyugue.toString().split(' ').slice(-1).join(' '));
        }
        $("#identidadConyugue").val(rowData.ClientesInformacionConyugal.fcIndentidadConyugue);
        $("#fechaNacimientoConyugue").val(dateFormat(rowData.ClientesInformacionConyugal.fdFechaNacimientoConyugue));
        $("#telefonoConyugue").val(rowData.ClientesInformacionConyugal.fcTelefonoConyugue);
        $("#lugarTrabajoConyugue").val(rowData.ClientesInformacionConyugal.fcLugarTrabajoConyugue);
        $("#ingresoMensualesConyugue").val(rowData.ClientesInformacionConyugal.fcIngresosMensualesConyugue);
        $("#telefonoTrabajoConyugue").val(FechaFormato(rowData.ClientesInformacionConyugal.fcTelefonoTrabajoConyugue));
    }
    //REFERENCIAS PERSONALES
    if (listaClientesReferencias.length == 0) {
        $('#datatable-buttons').DataTable().clear();
        listaClientesReferencias = [];
    }
    estadoFuncionRecuperarInfoCliente = true;
    if (estadoFuncionLlenarDDL == true && estadoFuncionRecuperarRespaldos == true) {
        $("#spinnerCargando").css('display', 'none');
    }

    /* verificar que no hayan respaldos ANTERIORES de solicitudes de clientes diferentes al actual, si las identidades no coinciden, quiere decir que son clientes diferentes, 
     * entonces se borraran los respaldos ANTERIORES y se iniciará el proceso de ingreso como una solicitud completamento nueva
    */
    var RespaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));

    if (RespaldoInformacionPrestamo != null) {
        if (objPrecalificado.identidad != RespaldoInformacionPrestamo.identidadCliente) {// si los respaldos que hay son de otro cliente, reemplazarlos por el actual del que se cargó la información
            guardarRespaldoInformacionPrestamo();
            guardarRespaldoInformacionPersonal();
            guardarRespaldoInformacionDomiciliar();
            guardarRespaldoInformacionLaboral();
            guardarRespaldoInformacionConyugal();
            guardarRespaldoReferenciasPersonales();
        }
        else {
            estadoFuncionRecuperarRespaldos = true;
            //localStorage.clear(); // eliminar respaldos anteriores
            //obtenerFechaActual(); // determinar la hora en la que se empieza a llenar la solicitd
        }
    } else {
        estadoFuncionRecuperarRespaldos = true;
        //localStorage.clear(); // eliminar respaldos anteriores
        //obtenerFechaActual(); // determinar la hora en la que se empieza a llenar la solicitud
    }
    //guardar

    $("#spinnerCargando").css('display', 'none');
    MensajeInformacion('La información de este cliente se cargó correctamente');
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

//habilitar ddl municipios cliente cuando se seleccione un departamento cliente
var CODDepto = 0;
$("#departamento").change(function () {

    $(this).parsley().validate();
    var idDepto = $("#departamento option:selected").val();
    CODDepto = idDepto;
    var municipioDdl = $("#municipio");
    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idDepto != '') {
        $("#spinnerCargando").css('display', '');
        municipioDdl.empty();
        municipioDdl.append("<option value=''>Seleccione una opción</option>");
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Registrar.aspx/CargarMunicipios",
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
    } else {
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
var CODMunicipio = 0;
$("#municipio").change(function () {

    $(this).parsley().validate();
    var idMunicipio = $("#municipio option:selected").val();
    CODMunicipio = idMunicipio;
    var ciudadDdl = $("#ciudad");
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idMunicipio != '') {
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
                    ciudadDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
                });
                ciudadDdl.attr('disabled', false);
                BarrioColoniaDdl.empty();
                BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
                BarrioColoniaDdl.attr('disabled', true);
                $("#spinnerCargando").css('display', 'none');
            }
        });
    } else {
        ciudadDdl.empty();
        ciudadDdl.append("<option value=''>Seleccione un municipio</option>");
        ciudadDdl.attr('disabled', true);
        BarrioColoniaDdl.empty();
        BarrioColoniaDdl.append("<option value=''>Seleccione una ciudad</option>");
        BarrioColoniaDdl.attr('disabled', true);
    }
});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
var CODCiudad = 0;
$("#ciudad").change(function () {

    var idCiudad = $("#ciudad option:selected").val();
    CODCiudad = idCiudad;
    $(this).parsley().validate();
    var BarrioColoniaDdl = $("#barrioColonia");

    if (idCiudad != '') {

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
                    BarrioColoniaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
                });
                BarrioColoniaDdl.attr('disabled', false);
                $("#spinnerCargando").css('display', 'none');
            }
        });
    } else {
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
var CODDeptoEmpresa = 0;
$("#departamentoEmpresa").change(function () {

    $(this).parsley().validate();
    var idDepto = $("#departamentoEmpresa option:selected").val();
    CODDeptoEmpresa = idDepto;
    var municipioEmpresaDdl = $("#municipioEmpresa");
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idDepto != '') {

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
                    municipioEmpresaDdl.append("<option value='" + iter.fiIDMunicipio + "'>" + iter.fcNombreMunicipio + "</option>");
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
    } else {
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
var CODMunicipioEmpresa = 0;
$("#municipioEmpresa").change(function () {

    $(this).parsley().validate();
    var idMunicipio = $("#municipioEmpresa option:selected").val();
    CODMunicipioEmpresa = idMunicipio;
    var ciudadaEmpresaDdl = $("#ciudadEmpresa");
    var barrioColoniaEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idMunicipio != '') {

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
                    ciudadaEmpresaDdl.append("<option value='" + iter.fiIDCiudad + "'>" + iter.fcNombreCiudad + "</option>");
                });
                ciudadaEmpresaDdl.attr('disabled', false);
                barrioColoniaEmpresaDdl.empty();
                barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
                barrioColoniaEmpresaDdl.attr('disabled', true);
                $("#spinnerCargando").css('display', 'none');
            }
        });
    } else {
        ciudadaEmpresaDdl.empty();
        ciudadaEmpresaDdl.append("<option value=''>Seleccione un municipio</option>");
        ciudadaEmpresaDdl.attr('disabled', true);
        barrioColoniaEmpresaDdl.empty();
        barrioColoniaEmpresaDdl.append("<option value=''>Seleccione una ciudad</option>");
        barrioColoniaEmpresaDdl.attr('disabled', true);
    }
});

//habilitar ddl barrios y colonias cliente cuando se seleccione una ciudad cliente
var CODCiudadEmpresa = 0;
$("#ciudadEmpresa").change(function () {

    $(this).parsley().validate();
    var idCiudad = $("#ciudadEmpresa option:selected").val();
    CODCiudadEmpresa = idCiudad;
    var barriosEmpresaDdl = $("#barrioColoniaEmpresa");

    if (idCiudad != '') {

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
                    barriosEmpresaDdl.append("<option value='" + iter.fiIDBarrioColonia + "'>" + iter.fcNombreBarrioColonia + "</option>");
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
//habilitar ddl barrios y colonias empresa
$("#barrioColoniaEmpresa").change(function () {
    $(this).parsley().validate();
});

//validar si se requiere información conyugal,si no se requiere información conyugal, deshabilitar ese paso del formulario
$('input:radio[name="estadoCivil"]').change(function () {

    var requiereInformacionConyugal = $("input[name='estadoCivil']:checked").data('info');
    if (requiereInformacionConyugal == false) {
        $('input.infoConyugal').attr('disabled', true);
        $('#smartwizard').smartWizard("stepState", [4], "hide");
    }
    else if (requiereInformacionConyugal == true) {
        $('input.infoConyugal').attr('disabled', false);
        $('#smartwizard').smartWizard("stepState", [4], "show");
    }
});

$('#txtMontoPrestamo,#txtMontoPrima,#txtPlazoPrestamo').blur(function () {
    var TipoPrestamo = $("#tipoPrestamo").data('cod');
    var MontoPrestamo = $('#txtMontoPrestamo').val().replace(/,/g, '') == '' ? 0 : $('#txtMontoPrestamo').val().replace(/,/g, '');
    var MontoPrima = TipoPrestamo == '101' ? 0 : $('#txtMontoPrima').val().replace(/,/g, '') == '' ? 0 : $('#txtMontoPrima').val().replace(/,/g, '');
    var Plazo = $('#txtPlazoPrestamo').val().replace(/,/g, '') == '' ? 0 : $('#txtPlazoPrestamo').val().replace(/,/g, '');
    var MontoFinanciar = MontoPrestamo - MontoPrima;
    $('#txtMontoFinanciar').val(MontoFinanciar);
    var ContadorErroresCalculoCuota = 0;

    /* validar monto minimo a financiar para motos y efectivo */
    if (MontoFinanciar < 6000.00 && (TipoPrestamo == '201' || TipoPrestamo == '101')) {
        MensajeError('El valor mínimo a financiar es 6,000.00');
        ContadorErroresCalculoCuota += 1;
    }

    /* si es prestamo moto, validar que la prima minima sea 10% */
    if (TipoPrestamo == '201') {
        var primaMinima = (MontoPrestamo * 10) / 100;
        MontoPrima = MontoPrima > primaMinima ? MontoPrima : primaMinima;
        $('#txtMontoPrima').val(MontoPrima);
        MontoFinanciar = MontoPrestamo - MontoPrima;
        $('#txtMontoFinanciar').val(MontoFinanciar);
    }

    if (MontoPrestamo > 0 && Plazo > 0 && MontoFinanciar > 0) {
        CalculoPrestamo(TipoPrestamo, MontoFinanciar, MontoPrima, Plazo);
    }
});

function CalculoPrestamo(TipoPrducto, MontoFinanciar, MontoPrima, PlazoPrestamo) {

    if (TipoPrducto != '' && MontoFinanciar > 0 && PlazoPrestamo > 0) {
        $('#spinnerCargando').css('display', '');
        var qString = "?" + window.location.href.split("?")[1];
        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Registrar.aspx/CalculoPrestamo' + qString,
            data: JSON.stringify({ TipoProducto: TipoPrducto, MontoFinanciar: MontoFinanciar, PlazoFinanciar: PlazoPrestamo, ValorPrima: MontoPrima }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al calcular cuota');
                $('#spinnerCargando').css('display', 'none');
            },
            success: function (data) {
                if (data.d != null) {

                    var CuotaCalculada = TipoPrducto == 202 ? data.d.CuotaMensual : data.d.CuotaQuincenal;
                    $('#txtCuotaCalculada').val(CuotaCalculada);
                } else {
                    MensajeError('No se pudo calcular la cuota');
                }
                $('#spinnerCargando').css('display', 'none');
            }
        });
    } else {
        MensajeError('Los valores ingresados no son válidos para el calculo de la cuota');
    }
}

function guardarRespaldoInformacionPrestamo() {
    debugger;
    var RespaldoInformacionPrestamo = {
        clienteID: clienteID,
        tipoPrestamo: $("#tipoPrestamo").val(),
        tipoSolicitud: $("#tipoSolicitud").val(),
        identidadCliente: $("#identidadCliente").val(),
        RTNCliente: $("#rtnCliente").val(),
        MontoPrestamo: $("#txtMontoPrestamo").val().replace(/,/g, ''),
        PlazoPrestamo: $("#txtPlazoPrestamo").val(),
        prima: $("#txtMontoPrima").val().replace(/,/g, '') == '' ? '0.00' : $("#txtMontoPrima").val().replace(/,/g, ''),
        valorVehiculo: $("#txtMontoPrestamo").val().replace(/,/g, '')
    }
    localStorage.setItem('RespaldoInformacionPrestamo', JSON.stringify(RespaldoInformacionPrestamo));
}

function guardarRespaldoInformacionPersonal() {

    var respaldoInformacionPersonal = {
        clienteID: clienteID,
        numeroTelefono: $("#numeroTelefono").val(),
        nacionalidad: $("#nacionalidad :selected").val(),
        TipodeCliente: $("#TipodeCliente :selected").val(),
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
        departamento: $("#departamento :selected").val(),
        municipio: $("#municipio :selected").val(),
        ciudad: $("#ciudad :selected").val(),
        barrioColonia: $("#barrioColonia :selected").val(),
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
        departamentoEmpresa: $("#departamentoEmpresa :selected").val(),
        municipioEmpresa: $("#municipioEmpresa :selected").val(),
        ciudadEmpresa: $("#ciudadEmpresa :selected").val(),
        barrioColoniaEmpresa: $("#barrioColoniaEmpresa :selected").val(),
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
        debugger;
        var RespaldoInformacionPrestamo = JSON.parse(localStorage.getItem('RespaldoInformacionPrestamo'));
        clienteID = RespaldoInformacionPrestamo.clienteID;
        var ValoraFinanciar = RespaldoInformacionPrestamo.MontoPrestamo - RespaldoInformacionPrestamo.prima;
        var PlazoPrestamo = RespaldoInformacionPrestamo.PlazoPrestamo;
        var PrimaPrestamo = RespaldoInformacionPrestamo.prima;
        PrimaPrestamo != '' ? PrimaPrestamo != 0 ? $("#txtMontoPrima").val(PrimaPrestamo) : '' : '';
        $("#txtMontoPrestamo").val(RespaldoInformacionPrestamo.MontoPrestamo);
        $("#txtPlazoPrestamo").val(RespaldoInformacionPrestamo.PlazoPrestamo);

        $("#txtMontoFinanciar").val(ValoraFinanciar);

        if ($("#tipoPrestamo").data('cod') == '101') {

            // si el prestamo requerido es en efectivo, no se requiere prima y el plazo es quincenal
            $("#txtMontoPrima,#ddlOrigen").prop('disabled', true);
            $("#ddlOrigen,#titleOrigen").css('display', 'none');
            var montoPmoEfectivo = $('#txtMontoPrestamo').val().replace(/,/g, '');
            if (montoPmoEfectivo != '') {
                CalculoPrestamo($("#tipoPrestamo").data('cod'), ValoraFinanciar, 0, parseInt(PlazoPrestamo))
            }
        }
        else {
            $("#ddlOrigen").prop('disabled', false);
            $("#ddlOrigen,#titleOrigen").css('display', '');
            cargarOrigenes($("#tipoPrestamo").data('cod'));

            $("#txtMontoPrima").prop('disabled', false);
            var prima = $('#txtMontoPrima').val().replace(/,/g, '');
            var valorVehiculo = $('#txtMontoPrestamo').val().replace(/,/g, '');
            var totalAFinanciar = valorVehiculo - prima;
            $('#txtMontoFinanciar').val(totalAFinanciar);

            if (valorVehiculo >= 0 && valorVehiculo >= prima) {
                CalculoPrestamo($("#tipoPrestamo").data('cod'), ValoraFinanciar, PrimaPrestamo, PlazoPrestamo)
            }
        }
    }

    //recuperar informacion del paso "informacion personal"
    if (localStorage.getItem('RespaldoInformacionPersonal') != null) {
        var respaldoInformacionPersonal = JSON.parse(localStorage.getItem('RespaldoInformacionPersonal'));
        clienteID = respaldoInformacionPersonal.clienteID;
        $("#nacionalidad").val(respaldoInformacionPersonal.nacionalidad);
        $("#TipodeCliente").val(respaldoInformacionPersonal.TipodeCliente);
        $("#fechaNacimiento").val(respaldoInformacionPersonal.fechaNacimiento);
        $("#correoElectronico").val(respaldoInformacionPersonal.correoElectronico);
        $("#rtnCliente").val(respaldoInformacionPersonal.RTNCliente);
        $("#profesion").val(respaldoInformacionPersonal.profesion);
        $("input[name=sexo][value=" + respaldoInformacionPersonal.sexo + "]").prop('checked', true);
        $("input[name=estadoCivil][value=" + respaldoInformacionPersonal.estadoCivil + "]").prop('checked', true);
        $("#vivivenda").val(respaldoInformacionPersonal.vivivenda);
        $("input[name=tiempoResidir][value=" + respaldoInformacionPersonal.tiempoResidir + "]").prop('checked', true);
    }

    //recuperar informacion del paso "informacion domiciliar"
    if (localStorage.getItem('RespaldoInformacionDomiciliar') != null) {
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

    //recuperar informacion del paso "informacion laboral"
    if (localStorage.getItem('RespaldoInformacionLaboral') != null) {
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
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Registrar.aspx/GetFecha",
        data: JSON.stringify({}),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
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
        },
        success: function (data) {
            var milli = data.d.replace(/\/Date\((-?\d+)\)\//, '$1');
            var dt = new Date(parseInt(milli));
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
    });
}