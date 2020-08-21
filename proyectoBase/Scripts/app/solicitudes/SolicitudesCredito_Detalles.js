var idSolicitud = 0;
var objSolicitud = [];
var resolucionHabilitada = false;
cargarInformacionSolicitud();

function cargarInformacionSolicitud() {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Detalles.aspx/CargarInformacionSolicitud",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo carga la información, contacte al administrador');
        },
        success: function (data) {

            var rowDataCliente = data.d.cliente;// Variable de informacion del cliente
            var rowDataSolicitud = data.d.solicitud;// Variable de informacion de la solicitud
            var rowDataDocumentos = data.d.documentos;// Variable de documentacion de la solicitud

            $("#btnHistorialExterno,#btnHistorialInterno").prop('disabled', false);
            var ProcesoPendiente = '/Date(-2208967200000)/';
            var IconoExito = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
            var IconoPendiente = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
            var IconoRojo = '<i class="mdi mdi mdi-close-circle-outline mdi-18px text-danger"></i>';

            objSolicitud = data.d.solicitud;
            idSolicitud = rowDataSolicitud.fiIDSolicitud;

            /* Cargar status de la solicitud */
            var statusIngreso = '';
            if (rowDataSolicitud.fdEnIngresoInicio != ProcesoPendiente) {
                statusIngreso = rowDataSolicitud.fdEnIngresoFin != ProcesoPendiente ? IconoExito : IconoPendiente;
            }

            /* Estado en tramite de la solicitud */
            var statusTramite = '';
            if (rowDataSolicitud.fdEnTramiteInicio != ProcesoPendiente) {

                if (rowDataSolicitud.fdEnTramiteFin != ProcesoPendiente) {
                    statusTramite = IconoExito;
                }
                else {
                    statusTramite = IconoPendiente;
                }
                if (rowDataSolicitud.fdEnTramiteFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                    statusTramite = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                }
            }

            /* Validar si ya se inició y terminó el proceso de analisis de la solicitud */
            var statusAnalisis = '';
            if (rowDataSolicitud.fdEnAnalisisInicio != ProcesoPendiente) {
                if (rowDataSolicitud.fdEnAnalisisFin != ProcesoPendiente) {
                    statusAnalisis = IconoExito;
                }
                else {
                    statusAnalisis = IconoPendiente;
                }
                if (rowDataSolicitud.fdEnAnalisisFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                    statusAnalisis = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                }
            }

            /* Validar si ya se envió a campo y si ya se recibió respuesta de gestoria */
            var statusCampo = '';
            if (rowDataSolicitud.fdEnvioARutaAnalista != ProcesoPendiente) {

                if (rowDataSolicitud.fdEnCampoFin != ProcesoPendiente || rowDataSolicitud.fiEstadoDeCampo == 2) {
                    statusCampo = IconoExito;
                }
                else {
                    statusCampo = IconoPendiente;
                }
                if (rowDataSolicitud.fdEnCampoFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                    statusCampo = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                }
                if (rowDataSolicitud.fiEstadoSolicitud == 5) {
                    statusCampo = IconoRojo;
                    habilitarResolucion = false;
                }
            }

            /* Validar si la solicitud está condicionada y sigue sin recibirse actualizacion del agente de ventas */
            var statusCondicionada = '';
            if (rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente) {

                if (rowDataSolicitud.fdCondificionadoFin != ProcesoPendiente) {
                    statusCondicionada = IconoExito;
                }
                else {
                    statusCondicionada = IconoPendiente;
                }
                if (rowDataSolicitud.fdCondificionadoFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                    statusCondicionada = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                }
            }

            /* Validar si la solicitud está reprogramada */
            var statusReprogramado = '';
            if (rowDataSolicitud.fdReprogramadoInicio != ProcesoPendiente) {

                if (rowDataSolicitud.fdReprogramadoFin != ProcesoPendiente) {
                    statusReprogramado = IconoExito;
                }
                else {
                    statusReprogramado = IconoPendiente;
                }
                if (rowDataSolicitud.fdReprogramadoFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                    statusReprogramado = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                }
            }

            /* Validar proceso de validacion */
            var statusValidacion = '';
            if (rowDataSolicitud.PasoFinalInicio != ProcesoPendiente) {

                if (rowDataSolicitud.PasoFinalFin != ProcesoPendiente) {
                    statusValidacion = IconoExito;
                } else {
                    statusValidacion = IconoPendiente;
                }
                if (rowDataSolicitud.PasoFinalFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                    statusValidacion = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                }
            }

            /* Validar si la solicitud ya ha sido aprobada o rechazada */
            var resolucionFinal = '';
            if (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7) {

                if (rowDataSolicitud.fiEstadoSolicitud == 7) {
                    resolucionFinal = IconoExito;
                }
                else {
                    resolucionFinal = IconoRojo;
                }
            }
            else {
                if (rowDataSolicitud.PasoFinalInicio != ProcesoPendiente) {
                    resolucionFinal = IconoPendiente;
                }
            }

            $('#tblEstadoSolicitud tbody').append('<tr>' +
                '<td class="text-center">' + statusIngreso + '</td>' +
                '<td class="text-center">' + statusTramite + '</td>' +
                '<td class="text-center">' + statusAnalisis + '</td>' +
                '<td class="text-center">' + statusCampo + '</td>' +
                '<td class="text-center">' + statusCondicionada + '</td>' +
                '<td class="text-center">' + statusReprogramado + '</td>' +
                '<td class="text-center">' + statusValidacion + '</td>' +
                '<td class="text-center">' + resolucionFinal + '</td>' +
                '</tr>');

            /* Informacion principal de la solicitud */
            $('#lblNoSolicitud').text(rowDataSolicitud.fiIDSolicitud);
            //$('#lblNoCliente').text(rowDataSolicitud.fiIDCliente);
            $('#lblNombreGestor').text(rowDataSolicitud.NombreGestor);
            var nombreCompletoCliente = rowDataCliente.clientesMaster.fcPrimerNombreCliente + ' ' + rowDataCliente.clientesMaster.fcSegundoNombreCliente + ' ' + rowDataCliente.clientesMaster.fcPrimerApellidoCliente + ' ' + rowDataCliente.clientesMaster.fcSegundoApellidoCliente;
            $('#lblNombreCliente').text(nombreCompletoCliente);
            $("#spanNombreCliente").text(nombreCompletoCliente);
            $('#lblIdentidadCliente').text(rowDataCliente.clientesMaster.fcIdentidadCliente);
            $("#spanIdentidadCliente").text(rowDataCliente.clientesMaster.fcIdentidadCliente);
            $('#lblTipoPrestamo').text(rowDataSolicitud.fcDescripcion);
            $('#lblTipoPrestamo').css('display', '');
            $('#lblTipoSolicitud').text(rowDataSolicitud.fiTipoSolicitud == 1 ? 'NUEVO' : rowDataSolicitud.fiTipoSolicitud == 2 ? 'REFINANCIAMIENTO' : rowDataSolicitud.fiTipoSolicitud == 3 ? 'RECOMPRA' : '');
            $('#lblAgenteDeVentas').text(rowDataSolicitud.fcNombreCortoVendedor);
            $('#lblAgencia').text(rowDataSolicitud.fcAgencia);
            /* Informacion personal */
            var infoPersonal = rowDataCliente.clientesMaster;
            $('#lblRtnCliente').text(infoPersonal.RTNCliente);
            $('#lblNumeroTelefono').text(infoPersonal.fcTelefonoCliente);
            $('#lblNumeroTelefono').attr('href', 'tel:+' + infoPersonal.fcTelefonoCliente.replace(' ', '').replace('-', '').replace('(', '').replace(')', ''));
            $('#lblNacionalidad').text(infoPersonal.fcDescripcionNacionalidad);
            var fechaNacimientoCliente = FechaFormato(infoPersonal.fdFechaNacimientoCliente);
            $('#lblFechaNacimientoCliente').text(fechaNacimientoCliente.split(' ')[0]);
            $('#lblCorreoCliente').text(infoPersonal.fcCorreoElectronicoCliente);
            $('#lblProfesionCliente').text(infoPersonal.fcProfesionOficioCliente);
            $('#lblSexoCliente').text(infoPersonal.fcSexoCliente == 'M' ? 'Masculino' : 'Femenino');
            $('#lblEstadoCivilCliente').text(infoPersonal.fcDescripcionEstadoCivil);
            $('#lblViviendaCliente').text(infoPersonal.fcDescripcionVivienda);
            $('#lblTiempoResidirCliente').text(infoPersonal.fiTiempoResidir > 2 ? 'Más de 2 años' : infoPersonal.fiTiempoResidir + ' años');
            /* Informacion domicilio */
            var infoDomiciliar = rowDataCliente.ClientesInformacionDomiciliar;
            $('#lblDeptoCliente').text(infoDomiciliar.fcNombreDepto);
            $('#lblMunicipioCliente').text(infoDomiciliar.fcNombreMunicipio);
            $('#lblCiudadCliente').text(infoDomiciliar.fcNombreCiudad);
            $('#lblBarrioColoniaCliente').text(infoDomiciliar.fcNombreBarrioColonia);
            $('#lblDireccionDetalladaCliente').text(infoDomiciliar.fcDireccionDetallada);
            $('#lblReferenciaDomicilioCliente').text(infoDomiciliar.fcReferenciasDireccionDetallada);
            /* Si el proceso de campo del domicilio ya se completo, mostrarlo */
            if (infoDomiciliar.fiIDInvestigacionDeCampo != 0) {
                $("#divInformaciondeCampo,#divResolucionDomicilio,#tituloCampoDomicilioModal").css('display', '');
                var ClassResultadodeCampo = infoDomiciliar.IDTipoResultado == 1 ? 'text-success' : infoDomiciliar.IDTipoResultado == 2 ? 'text-danger' : '';
                $("#lblResolucionCampoDomicilio").text('(' + infoDomiciliar.fcResultadodeCampo + ')').addClass(ClassResultadodeCampo);
                $("#lblGestorValidadorDomicilio").text(infoDomiciliar.fcGestorValidadorDomicilio);
                $("#lblResolucionDomicilio").text(infoDomiciliar.fcGestionDomicilio);
                $("#lblFechaValidacionDomicilio").text(FechaFormato(infoDomiciliar.fdFechaValidacion));
                $("#lblObservacionesCampoDomicilio").text(infoDomiciliar.fcObservacionesCampo);
                $("#lblResumenGestorDomicilio").text(infoDomiciliar.fcGestorValidadorDomicilio); // ficha de resumen
            }
            /* Informacion laboral */
            var infoLaboral = rowDataCliente.ClientesInformacionLaboral;
            $('#lblNombreTrabajoCliente').text(infoLaboral.fcNombreTrabajo);
            $('#lblIngresosMensualesCliente').text(addComasFormatoNumerico(infoLaboral.fiIngresosMensuales));
            $('#lblPuestoAsignadoCliente').text(infoLaboral.fcPuestoAsignado);
            var fechaIngresoCliente = FechaFormato(infoLaboral.fcFechaIngreso);
            $('#lblFechaIngresoCliente').text(fechaIngresoCliente.split(' ')[0]);
            $('#lblTelefonoEmpresaCliente').text(infoLaboral.fdTelefonoEmpresa);
            $('#lblTelefonoEmpresaCliente').attr('href', 'tel:+' + infoLaboral.fdTelefonoEmpresa.replace(' ', '').replace('-', '').replace('(', '').replace(')', ''));
            $('#lblExtensionRecursosHumanos').text(infoLaboral.fcExtensionRecursosHumanos);
            $('#lblExtensionCliente').text(infoLaboral.fcExtensionCliente);
            $('#lblDeptoEmpresa').text(infoLaboral.fcNombreDepto);
            $('#lblMunicipioEmpresa').text(infoLaboral.fcNombreMunicipio);
            $('#lblCiudadEmpresa').text(infoLaboral.fcNombreCiudad);
            $('#lblBarrioColoniaEmpresa').text(infoLaboral.fcNombreBarrioColonia);
            $('#lblDireccionDetalladaEmpresa').text(infoLaboral.fcDireccionDetalladaEmpresa);
            $('#lblReferenciaUbicacionEmpresa').text(infoLaboral.fcReferenciasDireccionDetallada);
            $('#lblDescripcionOtrosIngresos').text(infoLaboral.fcFuenteOtrosIngresos == '' ? 'N/A' : infoLaboral.fcFuenteOtrosIngresos);
            $('#lblValorOtrosIngresos').text(addComasFormatoNumerico(infoLaboral.fiValorOtrosIngresosMensuales));
            /* Si el proceso de campo del trabajo ya se completo, mostrarlo */
            if (infoLaboral.fiIDInvestigacionDeCampo != 0) {
                $("#divInformaciondeCampo,#divResolucionTrabajo,#tituloCampoTrabajoModal").css('display', '');
                var ClassResultadodeCampoLaboral = infoLaboral.IDTipoResultado == 1 ? 'text-success' : infoLaboral.IDTipoResultado == 2 ? 'text-danger' : '';
                $("#lblResolucionCampoTrabajo").text('(' + infoLaboral.fcResultadodeCampo + ')').addClass(ClassResultadodeCampoLaboral);
                $("#lblGestorValidadorTrabajo").text(infoLaboral.fcGestorValidadorTrabajo);
                $("#lblResolucionTrabajo").text(infoLaboral.fcGestionTrabajo);
                $("#lblFechaValidacionTrabajo").text(FechaFormato(infoLaboral.fdFechaValidacion));
                $("#lblObservacionesCampoTrabajo").text(infoLaboral.fcObservacionesCampo);
                $("#lblResumenGestorTrabajo").text(infoLaboral.fcGestorValidadorTrabajo); // ficha de resumen
            }

            /* Informacion conyugal */
            if (rowDataCliente.ClientesInformacionConyugal != null) {
                var infoConyugue = rowDataCliente.ClientesInformacionConyugal;
                $('#lblNombreConyugue').text(infoConyugue.fcNombreCompletoConyugue);
                $('#lblIdentidadConyuge').text(infoConyugue.fcIndentidadConyugue);
                var fechaNacimientoConyuge = infoConyugue.fdFechaNacimientoConyugue != '/Date(-2208967200000)/' ? FechaFormato(infoConyugue.fdFechaNacimientoConyugue) : '';
                $('#lblFechaNacimientoConygue').text(fechaNacimientoConyuge != '' ? fechaNacimientoConyuge.split(' ')[0] : '');
                $('#lblTelefonoConyugue').text(infoConyugue.fcTelefonoConyugue);
                $('#lblLugarTrabajoConyugue').text(infoConyugue.fcLugarTrabajoConyugue);
                $('#lblTelefonoTrabajoConyugue').text(infoConyugue.fcTelefonoTrabajoConyugue);
                $('#lblIngresosConyugue').text(addComasFormatoNumerico(infoConyugue.fcIngresosMensualesConyugue));
            }
            else {
                $("#titleConyugal").css('display', 'none');
                $("#divConyugueCliente").css('display', 'none');
            }
            /* Referencias personales del cliente */
            if (rowDataCliente.ClientesReferenciasPersonales != null) {
                if (rowDataCliente.ClientesReferenciasPersonales.length > 0) {
                    var tblReferencias = $('#tblReferencias tbody');
                    for (var i = 0; i < rowDataCliente.ClientesReferenciasPersonales.length; i++) {
                        var btnAgregarComentarioReferencia = '<button id="btnComentarioReferencia" data-id="' + rowDataCliente.ClientesReferenciasPersonales[i].fiIDReferencia + '" data-comment="' + rowDataCliente.ClientesReferenciasPersonales[i].fcComentarioDeptoCredito + '" data-nombreref="' + rowDataCliente.ClientesReferenciasPersonales[i].fcNombreCompletoReferencia + '" class="btn mdi mdi-comment" title="Ver observaciones del depto. de crédito"></button>';
                        var tiempoConocerRef = rowDataCliente.ClientesReferenciasPersonales[i].fiTiempoConocerReferencia <= 2 ? rowDataCliente.ClientesReferenciasPersonales[i].fiTiempoConocerReferencia + ' años' : 'Más de 2 años'
                        tblReferencias.append('<tr>' +
                            '<td class="FilaCondensada">' + rowDataCliente.ClientesReferenciasPersonales[i].fcNombreCompletoReferencia + '</td>' +
                            '<td class="FilaCondensada">' + rowDataCliente.ClientesReferenciasPersonales[i].fcLugarTrabajoReferencia + '</td>' +
                            '<td class="FilaCondensada">' + tiempoConocerRef + '</td>' +
                            '<td class="FilaCondensada">' +
                            '<a href="tel:+' + rowDataCliente.ClientesReferenciasPersonales[i].fcTelefonoReferencia.replace(' ', '').replace('-', '').replace('(', '').replace(')', '') + '" class="col-form-label">' + rowDataCliente.ClientesReferenciasPersonales[i].fcTelefonoReferencia + '</a>' +
                            '</td>' +
                            '<td class="FilaCondensada">' + rowDataCliente.ClientesReferenciasPersonales[i].fcDescripcionParentesco + '</td>' +
                            '<td class="FilaCondensada">' + btnAgregarComentarioReferencia + '</td>' +
                            '</tr>');
                    }
                }
            }
            /* Avales del cliente */
            if (rowDataCliente.Avales != null) {

                var listaAvales = rowDataCliente.Avales;
                if (listaAvales.length > 0) {
                    var tblReferencias = $('#tblAvales tbody');
                    for (var i = 0; i < listaAvales.length; i++) {
                        var estadoAval = listaAvales[i].fbAvalActivo == false ? 'Inactivo' : 'Activo';
                        var classEstadoAval = estadoAval == false ? 'text-danger' : '';
                        var info = JSON.stringify(listaAvales[i]);
                        var btnDetalleAvalModal = '<button id="btnDetalleAvalModal" data-id="' + listaAvales[i].fiIDAval + '" class="btn btn-sm ' + (classEstadoAval != '' ? 'btn-danger' : 'btn-info') + '" title="Detalles del aval">Detalles</button>';
                        var nombreCompletoAval = listaAvales[i].fcPrimerNombreAval + ' ' + listaAvales[i].fcSegundoNombreAval + ' ' + listaAvales[i].fcPrimerApellidoAval + ' ' + listaAvales[i].fcSegundoApellidoAval;

                        tblReferencias.append('<tr class="' + classEstadoAval + '">' +
                            '<td class="FilaCondensada">' + nombreCompletoAval + '</td>' +
                            '<td class="FilaCondensada">' + listaAvales[i].fcIdentidadAval + '</td>' +
                            '<td class="FilaCondensada">' +
                            '<a href="tel:+' + listaAvales[i].fcTelefonoAval.replace(' ', '').replace('-', '').replace('(', '').replace(')', '') + '" class="col-form-label ' + classEstadoAval + '">' + listaAvales[i].fcTelefonoAval + '</a>' +
                            '</td>' +
                            '<td class="FilaCondensada">' + listaAvales[i].fcNombreTrabajo + '</td>' +
                            '<td class="FilaCondensada">' + listaAvales[i].fcPuestoAsignado + '</td>' +
                            '<td class="FilaCondensada">' + addComasFormatoNumerico(listaAvales[i].fiIngresosMensuales) + '</td>' +
                            '<td class="FilaCondensada">' + estadoAval + '</td>' +
                            '<td class="FilaCondensada">' + btnDetalleAvalModal + '</td>' +
                            '</tr>');
                    }
                }
                else {
                    $('#divAval').css('display', 'none');
                }
            } else {
                $('#divAval').css('display', 'none');
            }

            /* Cargar documentación de la solicitud */
            var divDocumentacionCedula = $("#divDocumentacionCedula");
            var divDocumentacionCedulaModal = $("#divDocumentacionCedulaModal");
            var divDocumentacionDomicilio = $("#divDocumentacionDomicilio");
            var divDocumentacionDomicilioModal = $("#divDocumentacionDomicilioModal");
            var divDocumentacionLaboral = $("#divDocumentacionLaboral");
            var divDocumentacionLaboralModal = $("#divDocumentacionLaboralModal");
            var divDocumentacionFisicaModal = $("#divDocumentacionSoliFisicaModal");
            var divDocumentacionCampoDomicilio = $("#divDocumentacionCampoDomicilio");
            var divDocumentacionCampoDomicilioModal = $("#divDocumentacionCampoDomicilioModal");
            var divDocumentacionCampoTrabajo = $("#divDocumentacionCampoTrabajo");
            var divDocumentacionCampoTrabajoModal = $("#divDocumentacionCampoTrabajoModal");

            var contador = 0;
            var ruta = '';
            var img = '';
            var imgModal = '';

            for (var i = 0; i < rowDataDocumentos.length; i++) {

                ruta = rowDataDocumentos[i].URLArchivo;

                img = '<img class="img" class="img-responsive" src="' + ruta + '" title="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" alt="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" style="width: 50%; height: auto; float:left;" />'
                imgModal = '<a class="float-left" href="' + ruta + '" title="' + rowDataDocumentos[i].DescripcionTipoDocumento + '">' +
                    '<div class="img-responsive">' +
                    '<img class="img" src="' + ruta + '" alt="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" width="100" />' +
                    '</div>' +
                    '</a>'

                if (rowDataDocumentos[i].fiTipoDocumento == 1 || rowDataDocumentos[i].fiTipoDocumento == 2 || rowDataDocumentos[i].fiTipoDocumento == 18 || rowDataDocumentos[i].fiTipoDocumento == 19) {

                    if (contador < 2) {
                        divDocumentacionCedula.append('<a class="float-left" href="' + ruta + '" title="' + rowDataDocumentos[i].DescripcionTipoDocumento + '">' +
                            '<div class="img-responsive">' +
                            '<img class="img" src="' + ruta + '" alt="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" style="width: 100%; height: auto; float: left; cursor: zoom-in;" />' +
                            '</div>' +
                            '</a>');
                    }
                    divDocumentacionCedulaModal.append('<a class="float-left" href="' + ruta + '" title="' + rowDataDocumentos[i].DescripcionTipoDocumento + '">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '" alt="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" width="100" />' +
                        '</div>' +
                        '</a>');
                    contador = contador + 1;

                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 3) {

                    divDocumentacionDomicilio.append(img);
                    divDocumentacionDomicilioModal.append();

                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 5) {

                    divDocumentacionDomicilio.append(img);
                    divDocumentacionDomicilioModal.append(imgModal);

                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 4) {

                    divDocumentacionLaboral.append(img);
                    divDocumentacionLaboralModal.append(imgModal);
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 6) {

                    divDocumentacionLaboral.append(img);
                    divDocumentacionLaboralModal.append(imgModal);

                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 7) {
                    divDocumentacionLaboral.append(img);
                    divDocumentacionFisicaModal.append(imgModal);
                }

                else if (rowDataDocumentos[i].fiTipoDocumento == 8) {
                    divDocumentacionCampoDomicilio.append('<img class="img" class="img-responsive" src="' + ruta + '.jpg" title="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" alt="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" style="width: 50%; height: auto; float:left;" />');
                    divDocumentacionCampoDomicilioModal.append('<a class="float-left" href="' + ruta + '.jpg" title="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" alt="Campo - Foto de casa">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '.jpg" alt="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (rowDataDocumentos[i].fiTipoDocumento == 9) {

                    divDocumentacionCampoTrabajo.append('<img class="img" class="img-responsive" src="' + ruta + '.jpg" title="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" alt="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" style="width: 50%; height: auto; float:left;" />');
                    divDocumentacionCampoTrabajoModal.append('<a class="float-left" href="' + ruta + '.jpg" title="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" alt="Campo - Foto de trabajo">' +
                        '<div class="img-responsive">' +
                        '<img class="img" src="' + ruta + '.jpg" alt="' + rowDataDocumentos[i].DescripcionTipoDocumento + '" width="100" />' +
                        '</div>' +
                        '</a>');
                }
            }
            $(".img").imgbox({
                zoom: true,
                drag: true
            });

            /* Informacion del prestamo REQUERIDO */
            $('#lblValorPmoSugeridoSeleccionado').text(addComasFormatoNumerico(rowDataSolicitud.fdValorPmoSugeridoSeleccionado));
            $('#lblPlazoSeleccionado').text(addComasFormatoNumerico(rowDataSolicitud.fiPlazoPmoSeleccionado));
            $('#lblIngresosPrecalificado').text(addComasFormatoNumerico(rowDataSolicitud.fdIngresoPrecalificado));
            $('#lblObligacionesPrecalificado').text(addComasFormatoNumerico(rowDataSolicitud.fdObligacionesPrecalificado));
            $('#lblDisponiblePrecalificado').text(addComasFormatoNumerico(rowDataSolicitud.fdDisponiblePrecalificado));

            /* Calcular capacidad de pago mensual */
            var capacidadPago = calcularCapacidadPago(rowDataSolicitud.fiIDTipoPrestamo, rowDataSolicitud.fdObligacionesPrecalificado, rowDataSolicitud.fdIngresoPrecalificado);
            $('#lblCapacidadPagoMensual').text(addComasFormatoNumerico(capacidadPago));
            $('#lblCapacidadPagoQuincenal').text(addComasFormatoNumerico(capacidadPago / 2));

            if (rowDataSolicitud.fiIDTipoPrestamo == '101') {
                prestamoEfectivo(rowDataSolicitud.fiPlazoPmoSeleccionado, rowDataSolicitud.fdValorPmoSugeridoSeleccionado);
            }
            else if (rowDataSolicitud.fiIDTipoPrestamo == '201') {
                $('#lblValorVehiculo,#lblMontoValorVehiculo').css('display', '');
                $('#lblMontoValorVehiculo').text(addComasFormatoNumerico(rowDataSolicitud.fnValorGarantia));
                prestamoMoto(rowDataSolicitud.fnPrima, rowDataSolicitud.fnValorGarantia, rowDataSolicitud.fiPlazoPmoSeleccionado);
            }
            else if (rowDataSolicitud.fiIDTipoPrestamo == '202') {
                $('#lblValorVehiculo,#lblMontoValorVehiculo').css('display', '');
                $('#lblMontoValorVehiculo').text(addComasFormatoNumerico(rowDataSolicitud.fnValorGarantia));
                prestamoAuto(rowDataSolicitud.fnPrima, rowDataSolicitud.fnValorGarantia, rowDataSolicitud.fiPlazoPmoSeleccionado)
            }
            $('#lblMontoPrima').text(addComasFormatoNumerico(rowDataSolicitud.fnPrima));
            $('#lblEdadCliente').text(rowDataSolicitud.fiEdadCliente + ' años');

            /* Informacion del analisis */
            $("#tipoEmpresa").prop('disabled', true);
            $("#tipoPerfil").prop('disabled', true);
            $("#tipoEmpleo").prop('disabled', true);
            $("#buroActual").prop('disabled', true);
            $("#montoFinalFinanciar").prop('disabled', true);
            $("#montoFinalAprobado").prop('disabled', true);
            $("#plazoFinalAprobado").prop('disabled', true);

            /* Informacion del analisis */
            if (rowDataSolicitud.fcTipoEmpresa != '') {
                $("#tipoEmpresa").val(rowDataSolicitud.fcTipoEmpresa);
            }
            if (rowDataSolicitud.fcTipoPerfil != '') {
                $("#tipoPerfil").val(rowDataSolicitud.fcTipoPerfil);
            }
            if (rowDataSolicitud.fcTipoEmpleado != '') {
                $("#tipoEmpleo").val(rowDataSolicitud.fcTipoEmpleado);
            }
            if (rowDataSolicitud.fcBuroActual != '') {
                $("#buroActual").val(rowDataSolicitud.fcBuroActual);
            }
            if (rowDataSolicitud.fiMontoFinalSugerido != 0) {
                $("#montoFinalAprobado").val(rowDataSolicitud.fiMontoFinalSugerido);
            }
            if (rowDataSolicitud.fiMontoFinalFinanciar != 0) {
                $("#montoFinalFinanciar").val(rowDataSolicitud.fiMontoFinalFinanciar);
            }
            if (rowDataSolicitud.fiPlazoFinalAprobado != 0) {
                $("#plazoFinalAprobado").val(rowDataSolicitud.fiPlazoFinalAprobado);
            }

            /* Si se modificaron los ingresos del cliente debido a incongruencia con los comprobantes de ingreso mostrar recalculo con dicho valor */
            if (rowDataSolicitud.fnSueldoBaseReal != 0) {

                var bonosComisiones = rowDataSolicitud.fnBonosComisionesReal != 0 ? rowDataSolicitud.fnBonosComisionesReal : 0;
                var totalIngresosReales = bonosComisiones + rowDataSolicitud.fnSueldoBaseReal;
                $("#lblIngresosReales").text(addComasFormatoNumerico(totalIngresosReales));
                $('#lblObligacionesReales').text(addComasFormatoNumerico(rowDataSolicitud.fdObligacionesPrecalificado));
                $('#lblDisponibleReal').text(addComasFormatoNumerico(totalIngresosReales - rowDataSolicitud.fdObligacionesPrecalificado));
                var capacidadPagoReal = calcularCapacidadPago(rowDataSolicitud.fcDescripcion, rowDataSolicitud.fdObligacionesPrecalificado, rowDataSolicitud.fnSueldoBaseReal + bonosComisiones);
                $('#lblCapacidadPagoMensualReal').text(addComasFormatoNumerico(capacidadPagoReal));
                $('#lblCapacidadPagoQuincenalReal').text(addComasFormatoNumerico(capacidadPagoReal / 2));
                $("#divPmoSugeridoReal,#divRecalculoReal").css('display', '');
            }
            /* Si ya se determinó un monto a financiar, mostrarlo */
            if (rowDataSolicitud.fiMontoFinalFinanciar != 0) {

                /* Mostrar div del préstamo que se esocgió */
                $("#lblMontoPrestamoEscogido").text(addComasFormatoNumerico(rowDataSolicitud.fiMontoFinalFinanciar));
                $("#lblPlazoEscogido").text(rowDataSolicitud.fiPlazoFinalAprobado);
                $("#divPrestamoElegido").css('display', '');
            }
            /* Si se modificaron los ingresos del cliente y no se ha definido una resolucion para la solicitud, mostrar los PMO sugeridos */
            if (rowDataSolicitud.fiEstadoSolicitud != 4 && rowDataSolicitud.fiEstadoSolicitud != 5 && rowDataSolicitud.fiEstadoSolicitud != 7 && rowDataSolicitud.fnSueldoBaseReal != 0) {
                cargarPrestamosSugeridos(rowDataSolicitud.fnValorGarantia, rowDataSolicitud.fnPrima);
            }
        }
    });
}

/* Cargar detalles del procesamiento de la solicitud en el modal */
$('#tblEstadoSolicitud tbody').on('click', 'tr', function () {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Detalles.aspx/CargarEstadoSolicitud",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {
            if (data.d != null) {

                var rowDataSolicitud = data.d;
                var ProcesoPendiente = '/Date(-2208967200000)/';
                var tablaEstatusSolicitud = $('#tblDetalleEstado tbody');
                tablaEstatusSolicitud.empty();//limpiar tabla de detalles del estado de la solicitud

                var ingresoInicio = rowDataSolicitud.fdEnIngresoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnIngresoInicio) : '';
                var ingresoFin = rowDataSolicitud.fdEnIngresoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnIngresoFin) : '';
                var tiempoIngreso = ingresoInicio != '' ? ingresoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnIngresoInicio, rowDataSolicitud.fdEnIngresoFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Ingreso</td>' +
                    '<td>' + ingresoInicio + '</td>' +
                    '<td>' + ingresoFin + '</td>' +
                    '<td>' + tiempoIngreso + '</td>' +
                    '</tr>');

                var EnTramiteInicio = rowDataSolicitud.fdEnTramiteInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnTramiteInicio) : '';
                var EnTramiteFin = rowDataSolicitud.fdEnTramiteFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnTramiteFin) : '';
                var tiempoTramite = EnTramiteInicio != '' ? EnTramiteFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnTramiteInicio, rowDataSolicitud.fdEnTramiteFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Recepción</td>' +
                    '<td>' + EnTramiteInicio + '</td>' +
                    '<td>' + EnTramiteFin + '</td>' +
                    '<td>' + tiempoTramite + '</td>' +
                    '</tr>');

                var EnAnalisisInicio = rowDataSolicitud.fdEnAnalisisInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnAnalisisInicio) : '';
                var EnAnalisisFin = rowDataSolicitud.fdEnAnalisisFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnAnalisisFin) : '';
                var tiempoAnalisis = EnAnalisisInicio != '' ? EnAnalisisFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnAnalisisInicio, rowDataSolicitud.fdEnAnalisisFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Análisis</td>' +
                    '<td>' + EnAnalisisInicio + '</td>' +
                    '<td>' + EnAnalisisFin + '</td>' +
                    '<td>' + tiempoAnalisis + '</td>' +
                    '</tr>');

                var EnCampoInicio = rowDataSolicitud.fdEnvioARutaAnalista != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnvioARutaAnalista) : '';
                var EnCampoFin = rowDataSolicitud.fdEnCampoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnCampoFin) : '';
                var tiempoCampo = EnCampoInicio != '' ? EnCampoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnvioARutaAnalista, rowDataSolicitud.fdEnCampoFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Campo</td>' +
                    '<td>' + EnCampoInicio + '</td>' +
                    '<td>' + EnCampoFin + '</td>' +
                    '<td>' + tiempoCampo + '</td>' +
                    '</tr>');

                var CondicionadoInicio = rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondicionadoInicio) : '';
                var CondificionadoFin = rowDataSolicitud.fdCondificionadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondificionadoFin) : '';
                var tiempoCondicionado = CondicionadoInicio != '' ? CondificionadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdCondicionadoInicio, rowDataSolicitud.fdCondificionadoFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Condicionado</td>' +
                    '<td>' + CondicionadoInicio + '</td>' +
                    '<td>' + CondificionadoFin + '</td>' +
                    '<td>' + tiempoCondicionado + '</td>' +
                    '</tr>');

                var ReprogramadoInicio = rowDataSolicitud.fdReprogramadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoInicio) : '';
                var ReprogramadoFin = rowDataSolicitud.fdReprogramadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoFin) : '';
                var tiempoReprogramado = ReprogramadoInicio != '' ? ReprogramadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdReprogramadoInicio, rowDataSolicitud.fdReprogramadoFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Reprogramado</td>' +
                    '<td>' + ReprogramadoInicio + '</td>' +
                    '<td>' + ReprogramadoFin + '</td>' +
                    '<td>' + tiempoReprogramado + '</td>' +
                    '</tr>');

                var ValidacionInicio = rowDataSolicitud.PasoFinalInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.PasoFinalInicio) : '';
                var ValidacionFin = rowDataSolicitud.PasoFinalFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.PasoFinalFin) : '';
                var tiempoValidacion = ValidacionInicio != '' ? ValidacionFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.PasoFinalInicio, rowDataSolicitud.PasoFinalFin) : '' : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Validación</td>' +
                    '<td>' + ValidacionInicio + '</td>' +
                    '<td>' + ValidacionFin + '</td>' +
                    '<td>' + tiempoValidacion + '</td>' +
                    '</tr>');

                var timer = rowDataSolicitud.fcTiempoTotalTranscurrido.split(':');
                tablaEstatusSolicitud.append('<tr>' +
                    '<td colspan="4" class="text-center">Tiempo total transcurrido: <strong>' + timer[0] + ' horas con ' + timer[1] + ' minutos y ' + timer[2] + ' segundos </strong></td>' +
                    '</tr >');

                if (rowDataSolicitud.fiEstadoSolicitud == 7) {//verificar si ya se tomó una resolución de la solicitud
                    $("#lblResolucion").text('Aprobado');
                    $("#lblResolucion").removeClass('text-warning');
                    $("#lblResolucion").addClass('text-success');
                }
                else if (rowDataSolicitud.fiEstadoSolicitud == 4) {
                    $("#lblResolucion").text('Rechazado por analistas');
                    $("#lblResolucion").removeClass('text-warning');
                    $("#lblResolucion").addClass('text-danger');
                }
                else if (rowDataSolicitud.fiEstadoSolicitud == 5) {
                    $("#lblResolucion").text('Rechazado por gestores');
                    $("#lblResolucion").removeClass('text-warning');
                    $("#lblResolucion").addClass('text-danger');
                }
                else if (rowDataSolicitud.fdEnTramiteFin != '/Date(-2208967200000)/') {
                    $("#lblResolucion").text('En análisis');
                }
                var contadorComentariosDetalle = 0;

                if (rowDataSolicitud.fcReprogramadoComentario != '') {
                    $("#lblReprogramado").text(rowDataSolicitud.fcReprogramadoComentario);//razon reprogramado
                    $("#divReprogramado").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                if (rowDataSolicitud.fcCondicionadoComentario != '') {
                    $("#lblCondicionado").text(rowDataSolicitud.fcCondicionadoComentario);//razon condicionado
                    $("#divCondicionado").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                if (rowDataSolicitud.fcComentarioValidacionDocumentacion != '') {
                    $("#lblDocumentacionComentario").text(rowDataSolicitud.fcComentarioValidacionDocumentacion);//observaciones documentacion
                    $("#divDocumentacionComentario").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                if (rowDataSolicitud.fcComentarioValidacionInfoPersonal != '') {
                    $("#lblInfoPersonalComentario").text(rowDataSolicitud.fcComentarioValidacionInfoPersonal);//observaciones info personal
                    $("#divInfoPersonal").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                if (rowDataSolicitud.fcComentarioValidacionInfoLaboral != '') {
                    $("#lblInfoLaboralComentario").text(rowDataSolicitud.fcComentarioValidacionInfoLaboral);//observaciones info laboral
                    $("#divInfoLaboral").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                if (rowDataSolicitud.fcComentarioValidacionReferenciasPersonales != '') {
                    $("#lblReferenciasComentario").text(rowDataSolicitud.fcComentarioValidacionReferenciasPersonales);//observaciones referencias
                    $("#divReferencias").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                if (rowDataSolicitud.fcObservacionesDeCredito != '') {
                    $("#lblCampoComentario").text(rowDataSolicitud.fcObservacionesDeCredito);//comentario para campo depto de gestoria
                    $("#divCampo").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                if (rowDataSolicitud.fcObservacionesDeGestoria != '') {
                    $("#lblGestoriaComentario").text(rowDataSolicitud.fcObservacionesDeGestoria);//observaciones de gestoria
                    $("#divGestoria").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                if (rowDataSolicitud.fcComentarioResolucion != '') {
                    $("#lblResolucionComentario").text(rowDataSolicitud.fcComentarioResolucion);//observaciones de gestoria
                    $("#divComentarioResolucion").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                if (contadorComentariosDetalle == 0) { $("#divNoHayMasDetalles").css('display', ''); }//validar si no hay detalles que mostrar
                else { $("#divNoHayMasDetalles").css('display', 'none'); }

                if (rowDataSolicitud.fiSolicitudActiva == 0) { $("#divSolicitudInactiva").css('display', ''); }

                $("#modalEstadoSolicitud").modal();
            }
            else {
                MensajeError('Error al cargar estado de la solicitud, contacte al administrador');
            }
        }
    });
});

$(document).on('click', 'button#btnComentarioReferencia', function () {
    $("#txtObservacionesReferencia").val($(this).data('comment')).prop('disabled', true);
    $("#lblNombreReferenciaModal").text($(this).data('nombreref'));
    $("#modalComentarioReferencia").modal();
});

/* abrir modal de documentacion */
$("#btnValidoDocumentacionModal").click(function () {
    $("#modalFinalizarValidarDocumentacion").modal();
});

$("#btnHistorialExterno").click(function () {

    MensajeInformacion('Cargando buro externo');

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Detalles.aspx/ObtenerUrlEncriptado',
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar buro externo');
        },
        success: function (data) {
            var parametros = data.d;
            window.open("http://portal.prestadito.corp/corefinanciero/Clientes/Precalificado_Analista.aspx?" + parametros, "_blank",
                "toolbar=yes, scrollbars=yes,resizable=yes," +
                "top=0,left=window.screen.availWidth/2," +
                "window.screen.availWidth/2,window.screen.availHeight");
        }
    });
});

/* calculos de los prestamos */
function prestamoEfectivo(plazoQuincenal, prestamoAprobado) {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Detalles.aspx/CalculoPrestamo',
        data: JSON.stringify({ MontoFinanciar: prestamoAprobado, PlazoFinanciar: plazoQuincenal, ValorPrima: '0', dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar calculo del préstamo');
        },
        success: function (data) {
            var objCalculo = data.d;
            if (objCalculo != null) {
                $("#lblMontoFinanciarEfectivo").text(addComasFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblMontoCuotaEfectivo").text(addComasFormatoNumerico(objCalculo.CuotaQuincenal));
                $("#lblTituloCuotaEfectivo").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota);
                /* Mostrar div del calculo del prestamo efectivo */
                $("#lblPrima").css('display', 'none');
                $("#lblMontoPrima").css('display', 'none');
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                $("#divPrestamoEfectivo").css('display', '');
            }
            else { MensajeError('Error al realizar calculo del préstamo'); }
        }
    });
}

function prestamoMoto(ValorPrima, valorDeLaMoto, plazoQuincenal) {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Detalles.aspx/CalculoPrestamo',
        data: JSON.stringify({ MontoFinanciar: valorDeLaMoto, PlazoFinanciar: plazoQuincenal, ValorPrima: ValorPrima, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar calculo del préstamo');
        },
        success: function (data) {

            var objCalculo = data.d;
            if (objCalculo != null) {
                $("#lblMontoFinanciarMoto").text(addComasFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblTituloCuotaMoto").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota);
                $("#lblMontoCuotaMoto").text(addComasFormatoNumerico(objCalculo.CuotaQuincenal));

                /* Mostrar div del calculo del prestamo moto */
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                $("#divPrestamoMoto").css('display', '');
            }
            else { MensajeError('Error al realizar calculo del préstamo'); }
        }
    });
}

function prestamoAuto(ValorPrima, valorDelAuto, plazoMensual) {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Detalles.aspx/CalculoPrestamo',
        data: JSON.stringify({ MontoFinanciar: valorDelAuto, PlazoFinanciar: plazoMensual, ValorPrima: ValorPrima }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar calculo del préstamo');
        },
        success: function (data) {

            var objCalculo = data.d;
            if (objCalculo != null) {
                $("#lblMontoFinanciarAuto").text(addComasFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblMontoCuotaTotalAuto").text(addComasFormatoNumerico(objCalculo.CuotaMensualNeta));
                $("#lblTituloCuotaAuto").text(plazoMensual + ' Cuotas ' + objCalculo.TipoCuota);
                /* Mostrar div del calculo del prestamo auto */
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                $("#divPrestamoAuto").css('display', '');
            }
            else { MensajeError('Error al realizar calculo del préstamo'); }
        }
    });
}

function cargarPrestamosSugeridos(ValorProducto, ValorPrima) {
    $("#cargandoPrestamosSugeridosReales").css('display', '');

    MensajeInformacion('Cargando préstamos sugeridos');
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Detalles.aspx/GetPrestamosSugeridos',
        data: JSON.stringify({ ValorProducto: ValorProducto, ValorPrima: ValorPrima, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar préstamos sugeridos');
            $("#cargandoPrestamosSugeridosReales").css('display', 'none');
        },
        success: function (data) {

            if (data.d != null) {

                var listaPmos = data.d.cotizadorProductos;

                var tablaPrestamos = $("#tblPMOSugeridosReales tbody");
                tablaPrestamos.empty();

                for (var i = 0; i < listaPmos.length; i++) {

                    tablaPrestamos.append('<tr>'
                        + '<td class="FilaCondensada">' + listaPmos[i].fnMontoOfertado + '</td>'
                        + '<td class="FilaCondensada">' + listaPmos[i].fiPlazo + '</td>'
                        + '<td class="FilaCondensada">' + listaPmos[i].fnCuotaQuincenal + '</td>'
                        + '</tr>');
                }
                $("#cargandoPrestamosSugeridosReales").css('display', 'none');
                $("#lbldivPrestamosSugeridosReales,#tblPMOSugeridosReales,#divPrestamosSugeridosReales").css('display', '');
            }
            else {
                $("#cargandoPrestamosSugeridosReales").css('display', 'none');
            }
        }
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

function diferenciasEntreDosFechas(fechaInicio, fechaFin) {

    var inicio = new Date(FechaFormatoGuiones(fechaInicio));
    var fin = new Date(FechaFormatoGuiones(fechaFin));
    var tiempoResta = (fin.getTime() - inicio.getTime()) / 1000;
    /* Calcular dias */
    var dias = Math.floor(tiempoResta / 86400);
    //tiempoResta = tiempoResta >= 86400 ? dias * 86400 : tiempoResta;

    /* Calcular horas */
    var horas = Math.floor(tiempoResta / 3600) % 24;
    //tiempoResta = tiempoResta >= 3600 ? horas * 3600 : tiempoResta;

    /* Calcular minutos */
    var minutos = Math.floor(tiempoResta / 60) % 60;
    //tiempoResta = tiempoResta >= 3600 ? minutos * 3600 : tiempoResta;

    /* Calcular segundos */
    var segundos = tiempoResta % 60;
    var diferencia = pad2(dias) + ':' + pad2(horas) + ':' + pad2(minutos) + ':' + pad2(segundos);
    return diferencia;

    //var now = moment(fechaInicio);
    //var then = moment(fechaFin);

    //var ms = moment(now, "DD/MM/YYYY HH:mm:ss").diff(moment(then, "DD/MM/YYYY HH:mm:ss"));
    //var d = moment.duration(ms);
    //var s = Math.floor(d.asHours()) + moment.utc(ms).format(":mm:ss");
}

function addComasFormatoNumerico(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

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

$(window).on('hide.bs.modal', function () {
    /* cuando se abra un modal, ocultar el scroll del BODY y deja solo el del modal (en caso de que este tenga scroll) */
    const body = document.body;
    const scrollY = body.style.top;
    body.style.position = '';
    body.style.top = '';
    window.scrollTo(0, parseInt(scrollY || '0') * -1);
    $("body").css('padding-right', '0');
});

$(window).on('show.bs.modal', function () {
    /* Cuando se cierre el modal */
    const scrollY = document.documentElement.style.getPropertyValue('--scroll-y');
    const body = document.body;
    body.style.position = 'fixed';
    body.style.top = `-${scrollY}`;
    $("body").css('padding-right', '0');
});

window.addEventListener('scroll', () => {
    document.documentElement.style.setProperty('--scroll-y', `${window.scrollY}px`);
});