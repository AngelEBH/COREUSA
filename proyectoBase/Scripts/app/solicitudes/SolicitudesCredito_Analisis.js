gMontoFinal = 0;
gPlazoFinal = 0;
var objSolicitud = [];
var resolucionHabilitada = false;
var nombreCompletoClienteR = "";
cargarInformacionSolicitud();


/* Cargar la informacion de la solicitud que se va a analizar */
function cargarInformacionSolicitud() {
    
    $.ajax({

        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/CargarInformacionSolicitud",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo carga la información, contacte al administrador');
        },
        success: function (data) {

          
           console.log(data);

            /* Variable de informacion del cliente */

            var rowDataCliente = data.d.cliente;

            /* Variable de informacion de la solicitud */
            var rowDataSolicitud = data.d.solicitud;
            

            /* Variable de documentacion de la solicitud */
            var rowDataDocumentos = data.d.documentos;
            objSolicitud = data.d.solicitud;

            /* Variable para determinar si se puede dar resolucion a una solicitud dependiendo del estado de su procesamiento */
            var habilitarResolucion = true;
            $("#btnHistorialExterno,#btnHistorialInterno").prop('disabled', false);
            var ProcesoPendiente = '/Date(-2208967200000)/';
            var IconoExito = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
            var IconoPendiente = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
            var IconoRojo = '<i class="mdi mdi mdi-close-circle-outline mdi-18px text-danger"></i>';

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
                    habilitarResolucion = false;
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
                    habilitarResolucion = false;
                }

                if (rowDataSolicitud.fdEnAnalisisFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                    statusAnalisis = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                }
            }
            else {
                habilitarResolucion = false; // Si la solicitud no ha iniciado el analisis, no se puede tomar una resolucion
            }

            /* Validar si ya se envió a campo y si ya se recibió respuesta de gestoria */
            var statusCampo = '';
            if (rowDataSolicitud.fdEnvioARutaAnalista != ProcesoPendiente) {

                if (rowDataSolicitud.fdEnCampoFin != ProcesoPendiente || rowDataSolicitud.fiEstadoDeCampo == 2) {
                    statusCampo = IconoExito;
                }
                else {
                    statusCampo = IconoPendiente;
                    habilitarResolucion = false;
                }

                if (rowDataSolicitud.fdEnCampoFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                    statusCampo = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                }
                if (rowDataSolicitud.fiEstadoSolicitud == 5) {
                    statusCampo = IconoRojo;
                    habilitarResolucion = false;
                }
            }
            else {
                habilitarResolucion = false; // Si la solicitud no se ha enviado a campo, no se puede tomar una resolucion
            }

            /* Validar si la solicitud está condicionada y sigue sin recibirse actualizacion del agente de ventas */
            var statusCondicionada = '';
            if (rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente) {

                if (rowDataSolicitud.fdCondificionadoFin != ProcesoPendiente) {
                    statusCondicionada = IconoExito;
                }
                else {
                    statusCondicionada = IconoPendiente;
                    habilitarResolucion = false;
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
                    habilitarResolucion = false;
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
            else {
                habilitarResolucion = false;
            }

            /* Validar si la solicitud ya ha sido aprobada o rechazada */
            var resolucionFinal = '';
            if (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7) {

                if (rowDataSolicitud.fiEstadoSolicitud == 7) {
                    resolucionFinal = IconoExito;
                    habilitarResolucion = false;
                }
                else {
                    resolucionFinal = IconoRojo;
                    habilitarResolucion = false; // Si ya se determinó una resolucion para la solicitud, no se puede tomar una resolucion
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

            /* Verificar si la solicitud está condicionada */
            if ((rowDataSolicitud.fiEstadoSolicitud == 3)) {
                $("#btnCondicionarSolicitud, #btnCondicionarSolicitudConfirmar,#btnAgregarCondicion").prop('title', 'La solicitud ya está condicionada, esperando actualización de agente de ventas');
            }
            else if (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7) {
                var decision = rowDataSolicitud.fiEstadoSolicitud == 7 ? 'aprobada' : 'rechazada';
                $("#btnCondicionarSolicitud, #btnCondicionarSolicitudConfirmar,#btnAgregarCondicion").prop('disabled', true).prop('title', 'La solicitud ya fue ' + decision);
            }
            if (rowDataSolicitud.fbValidacionDocumentcionIdentidades != 0) {
                $("#btnValidarIdentidades").prop('disabled', true).prop('title', 'Documentación validada').addClass('text-success');
            }
            if (rowDataSolicitud.fbValidacionDocumentacionDomiciliar != 0) {
                $("#btnValidarDomiciliar").prop('disabled', true).prop('title', 'Documentación validada').addClass('text-success');
            }
            if (rowDataSolicitud.fbValidacionDocumentacionLaboral != 0) {
                $("#btnValidarLaboral").prop('disabled', true).prop('title', 'Documentación validada').addClass('text-success');
            }
            if (rowDataSolicitud.fbValidacionDocumentacionSolicitudFisica != 0) {
                $("#btnValidarSoliFisica").prop('disabled', true).prop('title', 'Documentación validada').addClass('text-success');
            }
            if (rowDataSolicitud.ftAnalisisTiempoValidarDocumentos != ProcesoPendiente) {
                $("#btnValidoDocumentacionConfirmar,#comentariosDocumentacion").prop('disabled', true);
                $("#btnValidoDocumentacionModal").removeClass('btn-warning').addClass('btn-success');
                $("#btnValidoDocumentacionConfirmar").removeClass('btn-warning').addClass('btn-primary');
                $("#btnValidoDocumentacionConfirmar").prop('title', 'La documentación ya fue validada');
                $("#btnValidoDocumentacionModal").text('Ver docs');
                $("#btnValidarSoliFisica,#btnValidarLaboral,#btnValidarDomiciliar,#btnValidarIdentidades").prop('disabled', true);
            }
            if (rowDataSolicitud.ftAnalisisTiempoValidarInformacionPersonal != ProcesoPendiente) {
                $("#btnValidoInfoPersonalModal, #btnValidoInfoPersonalConfirmar").prop('disabled', true).prop('title', 'La información personal ya fue validada').removeClass('btn-warning').addClass('btn-success');
            }
            if (rowDataSolicitud.ftAnalisisTiempoValidarInformacionLaboral != ProcesoPendiente) {
                $("#btnValidoInfoLaboralModal, #btnValidoInfoLaboralConfirmar").prop('disabled', true).prop('title', 'La información laboral ya fue validada').removeClass('btn-warning').addClass('btn-success');
            }
            if (rowDataSolicitud.ftAnalisisTiempoValidacionReferenciasPersonales != ProcesoPendiente) {
                $("#btnEliminarReferenciaConfirmar, #btnValidoReferenciasModal, #btnValidoReferenciasConfirmar,#btnEliminarReferencia,#btnReferenciaSinComunicacion,#btnComentarioReferenciaConfirmar").prop('disabled', true).prop('title', 'Las referencias personales ya fueron validadas').removeClass('btn-warning').addClass('btn-success');
            }
            if (rowDataSolicitud.fiEstadoDeCampo == 1) {
                $("#btnEnviarCampo, #btnEnviarCampoConfirmar").prop('disabled', true).prop('title', 'La solicitud ya fue enviada a campo').removeClass('btn-warning').addClass('btn-success');
            }
            else if (rowDataSolicitud.fiEstadoDeCampo == 2) {
                $("#btnEnviarCampo, #btnEnviarCampoConfirmar").prop('disabled', true).prop('title', 'El proceso de campo ya fue realizado').removeClass('btn-warning').addClass('btn-success');
            }
            else if (rowDataSolicitud.fcTipoEmpresa != '' && rowDataSolicitud.fcTipoPerfil != '' && rowDataSolicitud.fcTipoEmpleado != '' && rowDataSolicitud.fcBuroActual != '' && rowDataSolicitud.fiEstadoDeCampo == 0 && rowDataSolicitud.ftAnalisisTiempoValidarDocumentos != ProcesoPendiente && rowDataSolicitud.ftAnalisisTiempoValidarInformacionPersonal != ProcesoPendiente && rowDataSolicitud.ftAnalisisTiempoValidarInformacionLaboral != ProcesoPendiente) {
                $("#btnEnviarCampo, #btnEnviarCampoConfirmar").prop('disabled', false);
            }
            /* Informacion principal de la solicitud */
            $('#lblNoSolicitud').text(rowDataSolicitud.fiIDSolicitud);
            $('#lblNoCliente').text(rowDataSolicitud.fiIDCliente);
            $('#lblMarcaReferencia').text(rowDataSolicitud.fcMarca);
            $('#lblModeloReferencia').text(rowDataSolicitud.fcModelo);
            $('#lblVinReferencia').text(rowDataSolicitud.fcVin);
            $('#lblAnioReferencia').text(rowDataSolicitud.fiAnio); 





            //MensajeError('Mensaje '+rowDataCliente.clientesMaster.fiIDCliente);
            $('#lblNombreGestor').text(rowDataSolicitud.NombreGestor);
            var nombreCompletoCliente = rowDataCliente.clientesMaster.fcPrimerNombreCliente + ' ' + rowDataCliente.clientesMaster.fcSegundoNombreCliente + ' ' + rowDataCliente.clientesMaster.fcPrimerApellidoCliente + ' ' + rowDataCliente.clientesMaster.fcSegundoApellidoCliente;
            nombreCompletoClienteR = nombreCompletoCliente;
            $('#lblNombreCliente').text(nombreCompletoCliente);
            $('#lblResumenCliente').text(nombreCompletoCliente); // Ficha de resumen
            $("#spanNombreCliente").text(nombreCompletoCliente);
            $("#spanNombreClienteReferencia").text(nombreCompletoCliente);
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
            $('#lblCorreoCliente').attr('href', 'mailto:' + infoPersonal.fcCorreoElectronicoCliente);
            $('#lblProfesionCliente').text(infoPersonal.fcProfesionOficioCliente);
            $('#lblSexoCliente').text(infoPersonal.fcSexoCliente == 'M' ? 'Masculino' : 'Femenino');
            $('#lblEstadoCivilCliente').text(infoPersonal.fcDescripcionEstadoCivil);
            $('#lblViviendaCliente').text(infoPersonal.fcDescripcionVivienda);
            $('#lblTiempoResidirCliente').text(infoPersonal.fiTiempoResidir > 2 ? 'Más de 2 años' : infoPersonal.fiTiempoResidir + ' años');
            /* Informacion domiciliar */
            var infoDomiciliar = rowDataCliente.ClientesInformacionDomiciliar;
            $('#lblDeptoCliente').text(infoDomiciliar.fcNombreDepto);
            $('#lblMunicipioCliente').text(infoDomiciliar.fcNombreMunicipio);
            $('#lblCiudadCliente').text(infoDomiciliar.fcNombreCiudad);
            $('#lblBarrioColoniaCliente').text(infoDomiciliar.fcNombreBarrioColonia);
            $('#lblDireccionDetalladaCliente').text(infoDomiciliar.fcDireccionDetallada);
            $('#lblReferenciaDomicilioCliente').text(infoDomiciliar.fcReferenciasDireccionDetallada);
            $('#lblResumenDeptoResidencia').text(infoDomiciliar.fcNombreDepto); // Ficha de resumen
            $('#lblResumenMuniResidencia').text(infoDomiciliar.fcNombreMunicipio); // Ficha de resumen
            $('#lblResumenColResidencia').text(infoDomiciliar.fcNombreBarrioColonia); // Ficha de resumen
            $('#lblResumenTipoVivienda').text(infoPersonal.fcDescripcionVivienda); // Ficha de resumen
            $('#lblResumenTiempoResidir').text(infoPersonal.fiTiempoResidir > 2 ? 'Más de 2 años' : infoPersonal.fiTiempoResidir + ' años'); // Ficha de resumen

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
            $('#lblLugarTrabajo_Referencia').text(infoLaboral.fcNombreTrabajo);
            $('#lblResumenTrabajo').text(infoLaboral.fcNombreTrabajo); // ficha de resumen
            $('#lblIngresosMensualesCliente').text(addFormatoNumerico(infoLaboral.fiIngresosMensuales));
            $('#lblIngresosMensualesReferencia').text(addFormatoNumerico(infoLaboral.fiIngresosMensuales));
            $('#lblPuestoAsignadoCliente').text(infoLaboral.fcPuestoAsignado);
            $('#lblPuestoAsignado_Referencia').text(infoLaboral.fcPuestoAsignado);

            $('#lblResumenPuesto').text(infoLaboral.fcPuestoAsignado); // ficha de resumen
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
            $('#lblDireccionEmpresa_Referecia').text(infoLaboral.fcDireccionDetalladaEmpresa);

            $('#lblDireccionDomicilio_Referencia').text(infoLaboral.fcDireccionDetalladaEmpresa);
            $('#lblReferenciaUbicacionEmpresa').text(infoLaboral.fcReferenciasDireccionDetallada);

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
                $('#lblIngresosConyugue').text(addFormatoNumerico(infoConyugue.fcIngresosMensualesConyugue));
            }
            else {
                $("#titleConyugal").css('display', 'none');
                $("#divConyugueCliente").css('display', 'none');
            }
            /* Referencias personales del cliente */
            if (rowDataCliente.ClientesReferenciasPersonales != null) {
                if (rowDataCliente.ClientesReferenciasPersonales.length > 0) {

                    var ListaReferencias = rowDataCliente.ClientesReferenciasPersonales;

                    var tblReferencias = $('#tblReferencias tbody');
                    for (var i = 0; i < ListaReferencias.length; i++) {
                        var pencil = ListaReferencias[i].fcComentarioDeptoCredito != '' ? ListaReferencias[i].fcComentarioDeptoCredito.includes("(SIN CONFIRMAR)") ? 'text-danger':'tr-exito'   : '';

                        var ClaseBoton = ListaReferencias[i].fcComentarioDeptoCredito != '' ?
                            ListaReferencias[i].fcComentarioDeptoCredito.includes("(SIN CONFIRMAR)") ? 'btn mdi mdi-call-missed text-danger' :
                                'btn mdi mdi-check-circle-outline tr-exito' :'btn mdi mdi-pencil';

                        var btnAgregarComentarioReferencia = '<button id="btnComentarioReferencia" data-id="' + ListaReferencias[i].fiIDReferencia + '" data-comment="' + ListaReferencias[i].fcComentarioDeptoCredito + '" data-nombreref="' + ListaReferencias[i].fcNombreCompletoReferencia + '" class="' + ClaseBoton + '" title="Observaciones"></button>';
                        var tiempoConocerRef = ListaReferencias[i].fiTiempoConocerReferencia <= 2 ? ListaReferencias[i].fiTiempoConocerReferencia + ' años' : 'Más de 2 años'
                        tblReferencias.append('<tr class="' + pencil + '">' +
                            '<td class="FilaCondensada">' + ListaReferencias[i].fcNombreCompletoReferencia + '</td>' +
                            '<td class="FilaCondensada">' + ListaReferencias[i].fcLugarTrabajoReferencia + '</td>' +
                            '<td class="FilaCondensada">' + tiempoConocerRef + '</td>' +
                            '<td class="FilaCondensada">' +
                            '<a href="tel:+' + ListaReferencias[i].fcTelefonoReferencia.replace(' ', '').replace('-', '').replace('(', '').replace(')', '') + '" class="col-form-label">' + ListaReferencias[i].fcTelefonoReferencia + '</a>' +
                            '</td>' +
                            '<td class="FilaCondensada">' + ListaReferencias[i].fcDescripcionParentesco + '</td>' +
                            '<td class="FilaCondensada">' + btnAgregarComentarioReferencia + '</td>' +
                            '</tr>');
                    }
                }
            }
            /* Avales del cliente */
            if (rowDataCliente.Avales == null) {

                var listaAvales = rowDataCliente.Avales;
                if (listaAvales.length > 0) {

                    var tblReferencias = $('#tblAvales tbody');
                    for (var i = 0; i < listaAvales.length; i++) {

                        var estadoAval = listaAvales[i].fbAvalActivo == false ? 'Inactivo' : 'Activo';
                        var classEstadoAval = estadoAval == false ? 'text-danger' : '';
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
                            '<td class="FilaCondensada">' + addFormatoNumerico(listaAvales[i].fiIngresosMensuales) + '</td>' +
                            '<td class="FilaCondensada">' + estadoAval + '</td>' +
                            '<td class="FilaCondensada">' + btnDetalleAvalModal + '</td>' +
                            '</tr>');
                    }
                }
                else { $('#divAval').css('display', 'none'); }
            }
          //  else { $('#divAval').css('display', 'none'); }

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

                if (rowDataDocumentos[i].fiTipoDocumento == 1 || rowDataDocumentos[i].fiTipoDocumento == 2 || rowDataDocumentos[i].fiTipoDocumento == 18 || rowDataDocumentos[i].fiTipoDocumento == 19 || rowDataDocumentos[i].fiTipoDocumento == 24 || rowDataDocumentos[i].fiTipoDocumento == 25) {

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
                else if (rowDataDocumentos[i].fiTipoDocumento == 7 || rowDataDocumentos[i].fiTipoDocumento == 5 || rowDataDocumentos[i].fiTipoDocumento == 8 || rowDataDocumentos[i].fiTipoDocumento == 9 || rowDataDocumentos[i].fiTipoDocumento == 10
                     || rowDataDocumentos[i].fiTipoDocumento == 11 || rowDataDocumentos[i].fiTipoDocumento == 12 || rowDataDocumentos[i].fiTipoDocumento == 13 || rowDataDocumentos[i].fiTipoDocumento == 14
                     || rowDataDocumentos[i].fiTipoDocumento == 15 || rowDataDocumentos[i].fiTipoDocumento == 16 || rowDataDocumentos[i].fiTipoDocumento == 17 || rowDataDocumentos[i].fiTipoDocumento == 18) {
                    divDocumentacionLaboral.append(img);
                    divDocumentacionFisicaModal.append(imgModal);
                }


                /*else if (rowDataDocumentos[i].fiTipoDocumento == 8) {
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
                }*/
            }
            $(".img").imgbox({
                zoom: true,
                drag: true
            });

            $('#lblValorPmoSugeridoSeleccionado').text(addFormatoNumerico(rowDataSolicitud.fdValorPmoSugeridoSeleccionado));
            $('#lblPlazoSeleccionado').text(addFormatoNumerico(rowDataSolicitud.fiPlazoPmoSeleccionado));
            $('#lblIngresosPrecalificado').text(addFormatoNumerico(rowDataSolicitud.fdIngresoPrecalificado));
            $('#lblObligacionesPrecalificado').text(addFormatoNumerico(rowDataSolicitud.fdObligacionesPrecalificado));
            $('#lblDisponiblePrecalificado').text(addFormatoNumerico(rowDataSolicitud.fdDisponiblePrecalificado));

            /* Calcular capacidad de pago mensual */
            var capacidadPago = calcularCapacidadPago(rowDataSolicitud.fiIDTipoPrestamo, rowDataSolicitud.fdObligacionesPrecalificado, rowDataSolicitud.fdIngresoPrecalificado);
            $('#lblCapacidadPagoMensual').text(addFormatoNumerico(capacidadPago));
            $('#lblResumenCapacidadPagoMensual').text(addFormatoNumerico(capacidadPago)); // ficha de resumen
            var CapacidaddePagoQuincenal = (capacidadPago / 2).toFixed(2);
            $('#lblCapacidadPagoQuincenal').text(addFormatoNumerico(capacidadPago / 2));
            $('#lblResumenCapacidadPagoQuincenal').text(addFormatoNumerico(CapacidaddePagoQuincenal)); // ficha de resumen
            $('#lblResumenPrestamoSugeridoSeleccionado').text(addFormatoNumerico(rowDataSolicitud.fdValorPmoSugeridoSeleccionado)); // ficha de resumen



            //debugger;
            var tipodeCuota = 'Quincenal';
            if (rowDataSolicitud.fiIDTipoPrestamo == '101') {

                prestamoEfectivo(rowDataSolicitud.fiPlazoPmoSeleccionado, rowDataSolicitud.fdValorPmoSugeridoSeleccionado);
            }
            else if (rowDataSolicitud.fiIDTipoPrestamo == '301' || rowDataSolicitud.fiIDTipoPrestamo == '302') {

                $('#lblValorVehiculo,#lblMontoValorVehiculo').css('display', '');
                $('#lblMontoValorVehiculo').text(addFormatoNumerico(rowDataSolicitud.fnValorGarantia));

                prestamoConsumo(rowDataSolicitud.fnPrima, rowDataSolicitud.fnValorGarantia, rowDataSolicitud.fiPlazoPmoSeleccionado);
            }
            else if (rowDataSolicitud.fiIDTipoPrestamo == '201') {

                $('#lblValorVehiculo,#lblMontoValorVehiculo').css('display', '');
                $('#lblMontoValorVehiculo').text(addFormatoNumerico(rowDataSolicitud.fnValorGarantia));
                prestamoMoto(rowDataSolicitud.fnPrima, rowDataSolicitud.fnValorGarantia, rowDataSolicitud.fiPlazoPmoSeleccionado);

                $('#divPrimaManual').css('display', '');
                $('#txtMontoPrimaManual').prop('disabled', false);
            }
            else if (rowDataSolicitud.fiIDTipoPrestamo == '102' || rowDataSolicitud.fiIDTipoPrestamo == '100') {
                tipodeCuota = 'Mensual';
                $('#lblValorVehiculo,#lblMontoValorVehiculo').css('display', '');
                $('#lblMontoValorVehiculo').text(addFormatoNumerico(rowDataSolicitud.fnValorGarantia));
                prestamoAuto(rowDataSolicitud.fnPrima, rowDataSolicitud.fnValorGarantia, rowDataSolicitud.fiPlazoPmoSeleccionado)

                $('#divPrimaManual').css('display', '');
                $('#txtMontoPrimaManual').prop('disabled', false);
            }
            $('#lblMontoPrima').text(addFormatoNumerico(rowDataSolicitud.fnPrima));
            $('#lblEdadCliente').text(rowDataSolicitud.fiEdadCliente + ' años');
            $('#lblResumenEdad').text(rowDataSolicitud.fiEdadCliente + ' años'); // Ficha de resumen
            $('#lblEdadCliente_Referencia').text(rowDataSolicitud.fiEdadCliente + ' años');


            /* Validar si ya se definio un monto a financiar */
            if (rowDataSolicitud.fiMontoFinalFinanciar != 0) {
                gMontoFinal = rowDataSolicitud.fiMontoFinalFinanciar;
                $('#lblResumenMontoFinalFinanciar').text(rowDataSolicitud.fiMontoFinalFinanciar); // Ficha de resumen
            } else {
                gMontoFinal = rowDataSolicitud.fdValorPmoSugeridoSeleccionado;
            }
            if (rowDataSolicitud.fiPlazoFinalAprobado != 0) {
                gPlazoFinal = rowDataSolicitud.fiPlazoFinalAprobado;
                rowDataSolicitud.fiIDTipoPrestamo
                $('#lblResumenCuotaFinalFinanciarTitulo').text('Cuotas ' + tipodeCuota); // Ficha de resumen
                $('#lblResumenCuotaFinalFinanciar').text(rowDataSolicitud.fiPlazoFinalAprobado); // Ficha de resumen
            } else {
                gPlazoFinal = rowDataSolicitud.fiPlazoPmoSeleccionado;
            }

            /* Informacion del analisis */
            if (rowDataSolicitud.fcTipoEmpresa != '') {
                $("#tipoEmpresa").prop('disabled', true);
                $("#tipoEmpresa").val(rowDataSolicitud.fcTipoEmpresa);
                $("#lblResumenTipoEmpresa").text(rowDataSolicitud.fcTipoEmpresa); // Ficha de resumen
            }
            if (rowDataSolicitud.fcTipoPerfil != '') {
                $("#tipoPerfil").prop('disabled', true);
                $("#tipoPerfil").val(rowDataSolicitud.fcTipoPerfil);
                $("#lblResumenTipoPerfil").text(rowDataSolicitud.fcTipoPerfil); // Ficha de resumen
            }
            if (rowDataSolicitud.fcTipoEmpleado != '') {
                $("#tipoEmpleo").prop('disabled', true);
                $("#tipoEmpleo").val(rowDataSolicitud.fcTipoEmpleado);
                $("#lblResumenTipoEmpleo").text(rowDataSolicitud.fcTipoEmpleado); // Ficha de resumen
            }
            if (rowDataSolicitud.fcBuroActual != '') {
                $("#buroActual").prop('disabled', true);
                $("#buroActual").val(rowDataSolicitud.fcBuroActual);
                $("#lblResumenBuroActual").text(rowDataSolicitud.fcBuroActual); // Ficha de resumen
            }
            if (rowDataSolicitud.fiMontoFinalSugerido != 0) {
                $("#montoFinalAprobado").prop('disabled', true);
                $("#montoFinalAprobado").val(rowDataSolicitud.fiMontoFinalSugerido);
            }
            if (rowDataSolicitud.fiMontoFinalFinanciar != 0) {
                $("#montoFinalFinanciar").prop('disabled', true);
                $("#montoFinalFinanciar").val(rowDataSolicitud.fiMontoFinalFinanciar);
            }
            if (rowDataSolicitud.fiPlazoFinalAprobado != 0) {
                $("#plazoFinalAprobado").prop('disabled', true);
                $("#plazoFinalAprobado").val(rowDataSolicitud.fiPlazoFinalAprobado);
            }

            /* Si se modificaron los ingresos del cliente debido a incongruencia con los comprobantes de ingreso mostrar recalculo con dicho valor */
            if (rowDataSolicitud.fnSueldoBaseReal != 0) {

                var bonosComisiones = rowDataSolicitud.fnBonosComisionesReal != 0 ? rowDataSolicitud.fnBonosComisionesReal : 0;
                var totalIngresosReales = bonosComisiones + rowDataSolicitud.fnSueldoBaseReal;
                $("#lblIngresosReales").text(addFormatoNumerico(totalIngresosReales));
                $('#lblObligacionesReales').text(addFormatoNumerico(rowDataSolicitud.fdObligacionesPrecalificado));
                $('#lblDisponibleReal').text(addFormatoNumerico(totalIngresosReales - rowDataSolicitud.fdObligacionesPrecalificado));
                var capacidadPagoReal = calcularCapacidadPago(rowDataSolicitud.fiIDTipoPrestamo, rowDataSolicitud.fdObligacionesPrecalificado, rowDataSolicitud.fnSueldoBaseReal + bonosComisiones);
                $('#lblCapacidadPagoMensualReal').text(addFormatoNumerico(capacidadPagoReal));
                $('#lblCapacidadPagoQuincenalReal').text(addFormatoNumerico(capacidadPagoReal / 2));
                $("#divPmoSugeridoReal,#divRecalculoReal").css('display', '');
            }
            /* Si ya se determinó un monto a financiar, mostrarlo */
            if (rowDataSolicitud.fiMontoFinalFinanciar != 0) {

                /* Mostrar div del préstamo que se escogió */
                $("#lblMontoPrestamoEscogido").text(addFormatoNumerico(rowDataSolicitud.fiMontoFinalFinanciar));
                $("#lblPlazoEscogido").text(rowDataSolicitud.fiPlazoFinalAprobado);
                $("#divPrestamoElegido").css('display', '');
            }

            /* Si se modificaron los ingresos del cliente y no se ha definido una resolucion para la solicitud, mostrar los PMO sugeridos */
            if (rowDataSolicitud.fiEstadoSolicitud != 4 && rowDataSolicitud.fiEstadoSolicitud != 5 && rowDataSolicitud.fiEstadoSolicitud != 7 && rowDataSolicitud.fnSueldoBaseReal != 0) {
                cargarPrestamosSugeridos(rowDataSolicitud.fnValorGarantia, rowDataSolicitud.fnPrima);
            }
            /* Validar si ya se puede tomar una resolucion */
            rowDataSolicitud.ftAnalisisTiempoValidacionReferenciasPersonales == ProcesoPendiente ? habilitarResolucion = false : '';
            rowDataSolicitud.ftAnalisisTiempoValidarDocumentos == ProcesoPendiente ? habilitarResolucion = false : '';
            rowDataSolicitud.ftAnalisisTiempoValidarInformacionLaboral == ProcesoPendiente ? habilitarResolucion = false : '';
            rowDataSolicitud.ftAnalisisTiempoValidarInformacionPersonal == ProcesoPendiente ? habilitarResolucion = false : '';
            resolucionHabilitada = habilitarResolucion;

            if (habilitarResolucion == true) {
                $("#btnAprobar").prop('disabled', false);
                $("#btnAprobar").prop('title', 'Resolución final de la solicitud');

                $("#btnRechazar").prop('disabled', false);
                $("#btnRechazar").prop('title', 'Resolución final de la solicitud');
            } else {
                $("#btnRechazar").prop('disabled', false);
                $("#btnRechazar").prop('title', 'Resolución final de la solicitud');
            }

            if (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7) {

                var decision = rowDataSolicitud.fiEstadoSolicitud == 7 ? 'aprobada' : 'rechazada';
                $("#btnRechazar, #btnAprobar").prop('disabled', true);
                $("#btnRechazar, #btnAprobar").prop('title', 'La solicitud ya fue ' + decision);
                $("#actualizarIngresosCliente").css('display', 'none');
                $("#pencilOff").css('display', '');
                $("#btnValidarSoliFisica,#btnValidarLaboral,#btnValidarDomiciliar,#btnValidarIdentidades, #btnDigitarValoresManualmente").prop('disabled', true);
                $("#btnValidarSoliFisica,#btnValidarLaboral,#btnValidarDomiciliar,#btnValidarIdentidades, #btnDigitarValoresManualmente").prop('title', 'La solicitud ya fue ' + decision);
                $("#tipoEmpresa,#tipoPerfil,#tipoEmpleo,#buroActual").prop('disabled', true);
                $("#btnValidoDocumentacionModal").text('Ver Docs');
                $("#btnValidoDocumentacionModal").removeClass('btn-warning').addClass('btn-success');

                $(".validador").prop('disabled', true);
                $(".validador").prop('title', 'La solicitud ya fue ' + decision);
            }
            else {
                $("#pencilOff").css('display', 'none');
                $("#actualizarIngresosCliente").css('display', '');
            }

            /* Ficha de resumen*/
            $("#lblResumenVendedor").text(rowDataSolicitud.fcNombreCortoVendedor); // Ficha de resumen
            $("#lblResumenAnalista").text(rowDataSolicitud.fcNombreUsuarioModifica); // Ficha de resumen
        }
    });
    CargarInformacionClienteEquifax();
    CargarInformacionAval();
    MostrarDocumentosGarantia();
}

function CargarInformacionClienteEquifax(){
   // alert("prueba");
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/CargarInformacionClienteEquifax",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo carga la información, contacte al administrador');
        },
        success: function (data){
        //console.log(data);
        //console.log(data.d.fcDescricpcionOrigenEtnicoORacial);

           // alert("prueba");
           $("#lblDocumentoFiscal").text(data.d.fcDescripcionDoctosFiscal);
           $("#lblNIdFiscal").text(data.d.fcNoIdFiscal );
           $("#spanDocumentoPersonal").text(data.d.fcNombreDoctosIdPersonal);
           $("#lblOrigenEtnico").text(data.d.fcDescricpcionOrigenEtnicoORacial);

        }
    });
}


function MostrarDocumentosGarantia() {
  //  var idGarantia = 0;
   
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/CargarDocumentosGarantia",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: "application/json; charset=utf-8",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudieron cargar los documentos de la garantía, contacte al administrador.');
        },
        success: function (data) {
           
            if (data.d != null) {

                var documentos = data.d;
                var divGaleriaGarantia = $("#divGaleriaGarantia").empty();
                var templateDocumentos = '';

                if (documentos != null) {

                    if (documentos.length > 0) {

                        for (var i = 0; i < documentos.length; i++) {

                            templateDocumentos += '<img alt="' + documentos[i].DescripcionTipoDocumento + '" src="' + documentos[i].URLArchivo + '" data-image="' + documentos[i].URLArchivo + '" data-description="' + documentos[i].DescripcionTipoDocumento + '"/>';
                        }
                    }
                }

                var imgNoHayFotografiasDisponibles = '<img alt="No hay fotografías disponibles" src="/Imagenes/Imagen_no_disponible.png" data-image="/Imagenes/Imagen_no_disponible.png" data-description="No hay fotografías disponibles"/>';
                templateDocumentos = templateDocumentos == '' ? imgNoHayFotografiasDisponibles : templateDocumentos;

                divGaleriaGarantia.append(templateDocumentos);

                $("#divGaleriaGarantia").unitegallery();

                //$("#modalDocumentosDeLaGarantia").modal();
            }
            else
                MensajeError('No se pudieron cargar los documentos de la garantía, contacte al administrador.');
        }
    });
}

function CargarInformacionAval(){
    // alert("prueba");
     $.ajax({
         type: "POST",
         url: "SolicitudesCredito_Analisis.aspx/CargarDatosAval",
         data: JSON.stringify({ dataCrypt: window.location.href }),
         contentType: 'application/json; charset=utf-8',
         error: function (xhr, ajaxOptions, thrownError) {
             debugger;
             MensajeError('No se pudo carga la información, contacte al administrador');
         },
         success: function (data){
             
             var ListaAval = data.d;
         // console.log(data);
         //console.log(ListaAval.d.fcTelefonoAval);
         var tblReferencias = $('#tblAvales tbody');
         if (data.length > 0) {
             console.log(Entrando);
         }
         for (var i = 0; i < ListaAval.length; i++) {
           
            var estadoAval = ListaAval[i].fbAvalActivo == false ? 'Inactivo' : 'Activo';
            var classEstadoAval = estadoAval == false ? 'text-danger' : '';
            var btnDetalleAvalModal = '<button id="btnDetalleAvalModal" data-id="' + ListaAval[i].fiIDAval + '" class="btn btn-sm ' + (classEstadoAval != '' ? 'btn-danger' : 'btn-info') + '" title="Detalles del aval">Detalles</button>';
            var nombreCompletoAval = ListaAval[i].fcPrimerNombreAval + ' ' + ListaAval[i].fcSegundoNombreAval + ' ' + ListaAval[i].fcPrimerApellidoAval + ' ' + ListaAval[i].fcSegundoApellidoAval;
            tblReferencias.append('<tr class="' + classEstadoAval + '">' +
                            '<td class="FilaCondensada">' + nombreCompletoAval + '</td>' +
                            '<td class="FilaCondensada">' + ListaAval[i].fcIdentidadAval + '</td>' +
                            '<td class="FilaCondensada">' +
                            '<a href="tel:+' + ListaAval[i].fcTelefonoAval.replace(' ', '').replace('-', '').replace('(', '').replace(')', '') + '" class="col-form-label ' + classEstadoAval + '">' + ListaAval[i].fcTelefonoAval + '</a>' +
                            '</td>' +
                            '<td class="FilaCondensada">' + ListaAval[i].fcNombreTrabajo + '</td>' +
                            '<td class="FilaCondensada">' + ListaAval[i].fcPuestoAsignado + '</td>' +
                            '<td class="FilaCondensada">' + addFormatoNumerico(ListaAval[i].fiIngresosMensuales) + '</td>' +
                            '<td class="FilaCondensada">' + estadoAval + '</td>' +
                            '<td class="FilaCondensada">' + btnDetalleAvalModal + '</td>' +
                            '</tr>');
         }
 
         }
     });
 }


/* Validaciones de analisis */
$("#btnValidoInfoPersonalModal").click(function () {
    /* Verificar si la informacion personal ya fue validada antes */
    if (objSolicitud.ftAnalisisTiempoValidarInformacionPersonal == '/Date(-2208967200000)/') {
        $("#modalFinalizarValidarPersonal").modal();
    }
});

$("#btnValidoInfoPersonalConfirmar").click(function () {

    if ($($("#comentariosInfoPersonal")).parsley().isValid()) {

        var observacion = $("#comentariosInfoPersonal").val();
        var validacion = 'InformacionPersonal';
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
            data: JSON.stringify({ validacion: validacion, observacion: observacion, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != false) {
                    $("#modalFinalizarValidarPersonal").modal('hide');
                    $("#btnValidoInfoPersonalModal, #btnValidoInfoPersonalConfirmar").prop('disabled', true);
                    $("#btnValidoInfoPersonalModal, #btnValidoInfoPersonalConfirmar").removeClass('btn-warning').addClass('btn-success');
                    $("#btnValidoInfoPersonalModal, #btnValidoInfoPersonalConfirmar").prop('title', 'La información personal ya fue validada');
                    MensajeExito('Estado de la solicitud actualizado');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('E001 Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    } else { $($("#comentariosInfoPersonal")).parsley().validate(); }
});

$("#btnValidoInfoLaboralModal").click(function () {
    /* Verificar si la informacion laboral ya fue validada antes */
    if (objSolicitud.ftAnalisisTiempoValidarInformacionLaboral == '/Date(-2208967200000)/') {
        $("#modalFinalizarValidarLaboral").modal();
    }
});

$("#btnValidoInfoLaboralConfirmar").click(function () {

    if ($($("#comentariosInfoLaboral")).parsley().isValid()) {

        var observacion = $("#comentariosInfoLaboral").val();
        var validacion = 'InformacionLaboral';
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
            data: JSON.stringify({ validacion: validacion, observacion: observacion, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != false) {
                    $("#modalFinalizarValidarLaboral").modal('hide');
                    $("#btnValidoInfoLaboralModal, #btnValidoInfoLaboralConfirmar").prop('disabled', true);
                    $("#btnValidoInfoLaboralModal, #btnValidoInfoLaboralConfirmar").removeClass('btn-warning').addClass('btn-success');
                    $("#btnValidoInfoLaboralModal, #btnValidoInfoLaboralConfirmar").prop('title', 'La información laboral ya fue validada');
                    MensajeExito('Estado de la solicitud actualizado');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('E002 Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    }
    else { $($("#comentariosInfoLaboral")).parsley().validate(); }
});

$("#btnValidoReferenciasModal").click(function () {
    if (objSolicitud.ftAnalisisTiempoValidacionReferenciasPersonales == '/Date(-2208967200000)/') {
        $("#modalFinalizarValidarReferencias").modal();
    }
});

$("#btnValidoReferenciasConfirmar").click(function () {

    if ($($("#comentarioReferenciasPersonales")).parsley().isValid()) {

        var observacion = $("#comentarioReferenciasPersonales").val();
        var validacion = 'Referencias';

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
            data: JSON.stringify({ validacion: validacion, observacion: observacion, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != 0) {
                    $("#btnValidoReferenciasModal, #btnValidoReferenciasConfirmar").prop('disabled', true);
                    $("#btnValidoReferenciasModal, #btnValidoReferenciasConfirmar").removeClass('btn-warning').addClass('btn-success');
                    $("#btnValidoReferenciasModal, #btnValidoReferenciasConfirmar").prop('title', 'Las referencias personales ya fueron validadas');
                    $("#modalFinalizarValidarReferencias").modal('hide');
                    MensajeExito('Estado de la solicitud actualizado');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('E003 Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    } else { $($("#comentarioReferenciasPersonales")).parsley().validate(); }
});

$("#btnValidoDocumentacionModal").click(function () {
    $("#modalFinalizarValidarDocumentacion").modal();
});

$("#btnValidoDocumentacionConfirmar").click(function () {

    if ($($("#comentariosDocumentacion")).parsley().isValid() && objSolicitud.ftAnalisisTiempoValidarDocumentos == '/Date(-2208967200000)/') {

        var observacion = $("#comentariosDocumentacion").val();
        var validacion = 'Documentacion';

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
            data: JSON.stringify({ validacion: validacion, observacion: observacion, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != 0) {
                    $("#btnValidoDocumentacionModal").removeClass('btn-warning').addClass('btn-success');
                    $("#btnValidoDocumentacionConfirmar").removeClass('btn-warning').addClass('btn-primary');
                    $("#btnValidoDocumentacionConfirmar").prop('title', 'La documentación ya fue validada');
                    $("#btnValidoDocumentacionModal").text('Ver docs');
                    $("#btnValidoDocumentacionConfirmar,#btnValidarSoliFisica,#btnValidarLaboral,#btnValidarDomiciliar,#btnValidarIdentidades").prop('disabled', true);
                    $("#btnValidarSoliFisica,#btnValidarLaboral,#btnValidarDomiciliar,#btnValidarIdentidades").prop('title', 'La documentación ya fue validada');
                    $("#modalFinalizarValidarDocumentacion").modal('hide');
                    MensajeExito('Estado de la solicitud actualizado');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('E004 Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    } else { $($("#comentariosDocumentacion")).parsley().validate(); }
});

var tipoDoc = 0;
$(document).on('click', 'button#btnValidarIdentidades,#btnValidarDomiciliar,#btnValidarLaboral,#btnValidarSoliFisica', function () {

    tipoDoc = $(this).data('id');
    $("#modalFinalizarValidarDocumentacion").modal('hide');
    $("#modalValidarTipoDocs").modal();
});

$("#btnValidarTipoDocConfirmar").click(function () {

    var btn = 0;
    var validacion = '';

    switch (tipoDoc) {
        case 1:
            validacion = 'ValidacionDocumentcionIdentidades';
            btn = 1;
            break;
        case 2:
            validacion = 'ValidacionDocumentacionDomicilio';
            btn = 2;
            break;
        case 3:
            validacion = 'ValidacionDocumentacionLaboral';
            btn = 3;
            break;
        case 4:
            validacion = 'ValidacionDocumentacionSolicitudFisica';
            btn = 4;
            break;
        default:
            MensajeError('Error al validar tipo de documentación');
    }

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/ValidacionesAnalisis",
        data: JSON.stringify({ validacion: validacion, observacion: '', dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo realizar la validacion, contacte al administrador');
            $("#modalValidarTipoDocs").modal('hide');
            $("#modalFinalizarValidarDocumentacion").modal();
        },
        success: function (data) {
            if (data.d != 0) {
                $("#modalValidarTipoDocs").modal('hide');
                $("#modalFinalizarValidarDocumentacion").modal();

                switch (btn) {
                    case 1:
                        $("#btnValidarIdentidades").prop('disabled', true);
                        $("#btnValidarIdentidades").addClass('text-success');
                        $("#btnValidarIdentidades").prop('title', 'Documentación validada');
                        break;
                    case 2:
                        $("#btnValidarDomiciliar").prop('disabled', true);
                        $("#btnValidarDomiciliar").addClass('text-success');
                        $("#btnValidarDomiciliar").prop('title', 'Documentación validada');
                        break;
                    case 3:
                        $("#btnValidarLaboral").prop('disabled', true);
                        $("#btnValidarLaboral").addClass('text-success');
                        $("#btnValidarLaboral").prop('title', 'Documentación validada');
                        break;
                    case 4:
                        $("#btnValidarSoliFisica").prop('disabled', true);
                        $("#btnValidarSoliFisica").addClass('text-success');
                        $("#btnValidarSoliFisica").prop('title', 'Documentación validada');
                        break;
                }
                MensajeExito('Estado de la solicitud actualizado');
            }
            else {
                MensajeError('E005 Error al actualizar estado de la solicitud, contacte al administrador');
                $("#modalValidarTipoDocs").modal('hide');
                $("#modalFinalizarValidarDocumentacion").modal();
            }
        }
    });
});

/* Actualizar ingresos del cliente */
$("#actualizarIngresosCliente").click(function () {
    $("#txtIngresosReales,#txtBonosComisiones").val('');
    $("#modalActualizarIngresos").modal();
});

$("#formActualizarIngresos").submit(function (e) {

    e.preventDefault();
    if ($($("#formActualizarIngresos")).parsley().isValid()) {

        var sueldoBaseReal = $("#txtIngresosReales").val().replace(/,/g, '');
        var bonosComisionesReal = $("#txtBonosComisiones").val().replace(/,/g, '');
        var obligaciones = objSolicitud.fdObligacionesPrecalificado;
        var codigoProducto = objSolicitud.fiIDTipoPrestamo;

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/ActualizarIngresosCliente',
            data: JSON.stringify({ sueldoBaseReal: sueldoBaseReal, bonosComisionesReal: bonosComisionesReal, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al actualizar ingresos');
            },
            success: function (data) {
                if (data.d == true) {

                    $('#lblIngresosMensualesCliente').text('');
                    var ingresos = parseFloat(sueldoBaseReal) + parseFloat(bonosComisionesReal);
                    $('#lblIngresosMensualesCliente').text(addFormatoNumerico(ingresos));                  

                    var totalIngresosReales = ingresos;
                   
                    $("#lblIngresosReales").text(addFormatoNumerico(totalIngresosReales));
                    $('#lblObligacionesReales').text(addFormatoNumerico(obligaciones));
                    $('#lblDisponibleReal').text(addFormatoNumerico(totalIngresosReales - obligaciones));
                    var capacidadPagoReal = calcularCapacidadPago(codigoProducto, obligaciones, totalIngresosReales);
                    $('#lblCapacidadPagoMensualReal').text(addFormatoNumerico(capacidadPagoReal));
                    $('#lblCapacidadPagoQuincenalReal').text(addFormatoNumerico(capacidadPagoReal / 2));
                    $("#divRecalculoReal").css('display', '');
                    MensajeExito('Ingresos actualizados correctamente');
                    cargarPrestamosSugeridos(objSolicitud.fnValorGarantia, objSolicitud.fnPrima);
                    $("#modalActualizarIngresos").modal('hide');
                }
                else {
                    MensajeError('Error al actualizar ingresos');
                }
            }
        });
    } else { $($("#formActualizarIngresos")).parsley().validate(); }
});


/* Cuando la nueva capacidad de pago es insuficiente */
$("#btnRechazarIncapPagoConfirmar").click(function () {

    var objBitacora = {
        fcComentarioResolucion: 'Rechazado por incapcidad de pago'
    };
    var solicitud = {
        fiEstadoSolicitud: 4
    };

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/SolicitudResolucion",
        data: JSON.stringify({ objBitacora: objBitacora, objSolicitud: solicitud, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Ocurrió un error al determinar resolucion de la solicitud, contacte al administrador');
        },
        success: function (data) {
            if (data.d != 0) {

                /* Informacion del analisis */
                $("#tipoEmpresa").prop('disabled', true);
                $("#tipoEmpresa").val('');
                $("#tipoPerfil").prop('disabled', true);
                $("#tipoPerfil").val('');
                $("#tipoEmpleo").prop('disabled', true);
                $("#tipoEmpleo").val('');
                $("#buroActual").prop('disabled', true);
                $("#buroActual").val('');
                $("#montoFinalAprobado").prop('disabled', true);
                $("#montoFinalAprobado").val('');
                $("#montoFinalFinanciar").prop('disabled', true);
                $("#montoFinalFinanciar").val('');
                $("#plazoFinalAprobado").prop('disabled', true);
                $("#plazoFinalAprobado").val('');
                $("#modalRechazar,#modalRechazarPorIncapcidadPago").modal('hide');
                $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud,#btnDigitarMontoManualmente,#btnDigitarValoresManualmente,#btnRechazarIncapacidadPagoModal,#btnRechazarIncapPagoConfirmar").prop('disabled', true);
                $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud,#btnDigitarMontoManualmente,#btnDigitarValoresManualmente,#btnRechazarIncapacidadPagoModal,#btnRechazarIncapPagoConfirmar").prop('title', 'La solicitud ya fue rechazada');
                $("#actualizarIngresosCliente").css('display', 'none');
                $("#pencilOff").css('display', '');
                MensajeExito('¡Solicitud rechazada correctamente!');
                actualizarEstadoSolicitud();
            }
            else { MensajeError('E006 Error al actualizar estado de la solicitud, contacte al administrador'); }
        }
    });
});

$("#btnDigitarMontoManualmente,#btnDigitarValoresManualmente").click(function () {
    $("#txtMontoManual,#txtPlazoManual,#txtMontoPrimaManual").val('');
    $("#ModalDigitarMontosManualmente").modal();
});

// -- DEBUGGEANDO ESTE
$('#txtValorGlobalManual,#txtValorPrimaManual,#txtValorPlazoManual').blur(function () {
    debugger;
    /* Calcular Cuota y valor a Financiar */
    var valorGlobal = $("#txtValorGlobalManual").val().replace(/,/g, '') == '' ? 0 : $('#txtValorGlobalManual').val().replace(/,/g, '');
    valorGlobal = parseFloat(valorGlobal);

    var valorPrima = $("#txtValorPrimaManual").val().replace(/,/g, '') == '' ? 0 : $('#txtValorPrimaManual').val().replace(/,/g, '');
    valorPrima = parseFloat(valorPrima);

    var cantidadPlazos = $("#txtValorPlazoManual").val() == '' ? 0 : $('#txtValorPlazoManual').val();
    cantidadPlazos = parseInt(cantidadPlazos);

    var montoFinanciar = valorGlobal - valorPrima;

    if (valorGlobal > 0 && valorPrima >= 0 && valorGlobal > valorPrima && cantidadPlazos > 0 && montoFinanciar > 0) {

        $('#divCalculandoCuotaManual').css('display', '');
        debugger;
        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/CalculoPrestamo',
            data: JSON.stringify({ ValorPrestamo: valorGlobal, ValorPrima: valorPrima, CantidadPlazos: cantidadPlazos, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al calcular cuota');
                $('#divCalculandoCuotaManual').css('display', 'none');
                $('#btnActualizarMontoManualmente').prop('disabled', false);
            },
            success: function (data) {
                if (data.d != null) {
                    $('#lblTituloCantidadCuotaManual').text(cantidadPlazos + ' Cuotas ' + data.d.TipoCuota);
                    $('#txtValorCuotaManual').val(data.d.CuotaQuincenal);
                    if (data.d.CuotaMensualNeta != 0) {
                        $('#txtValorCuotaManual').val(data.d.CuotaMensualNeta);
                    }
                    $("#txtMontoaFinanciarManual").val(data.d.ValoraFinanciar);
                    $('#divMostrarCalculoCuotaManual').css('display', '');
                    montoFinanciarCalculado = data.d.ValoraFinanciar;
                }
                else {
                    MensajeError('No se pudo calcular la cuota');
                }
                $('#divCalculandoCuotaManual').css('display', 'none');
                $('#btnActualizarMontoManualmente').prop('disabled', false);
            }
        });
    }
});

/* DEBUGUEAR ESTE */
var montoFinanciarCalculado = 0;
$("#btnActualizarMontoManualmente").click(function () {
    var valorGlobal = $("#txtValorGlobalManual").val().replace(/,/g, '') == '' ? 0 : $('#txtValorGlobalManual').val().replace(/,/g, '');
    valorGlobal = parseFloat(valorGlobal);

    var valorPrima = $("#txtValorPrimaManual").val().replace(/,/g, '') == '' ? 0 : $('#txtValorPrimaManual').val().replace(/,/g, '');
    valorPrima = parseFloat(valorPrima);

    var cantidadPlazos = $("#txtValorPlazoManual").val() == '' ? 0 : $('#txtValorPlazoManual').val();
    cantidadPlazos = parseInt(cantidadPlazos);

    if (valorGlobal > 0 && valorPrima >= 0 && valorGlobal > valorPrima && cantidadPlazos > 0 && montoFinanciarCalculado > 0) {

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/ActualizarPlazoMontoFinanciar',
            data: JSON.stringify({ ValorGlobal: valorGlobal, ValorPrima: valorPrima, CantidadPlazos: cantidadPlazos, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al actualizar valores del préstamo');
            },
            success: function (data) {
                if (data.d == true) {
                    $("#lblMontoPrestamoEscogido").text(addFormatoNumerico(montoFinanciarCalculado));
                    $("#lblPlazoEscogido").text(cantidadPlazos);
                    gMontoFinal = montoFinanciarCalculado;
                    gPlazoFinal = cantidadPlazos;
                    $("#divPrestamoElegido").css('display', '');
                    $("#modalMontoFinanciar,#ModalDigitarMontosManualmente").modal('hide');
                    MensajeExito('Monto a financiar actualizado correctamente');

                    VALORGARANTIA_CARGADA_EN_PRESTAMOSSUGERIDOS = valorGlobal;
                    VALORPRIMA_CARGADA_EN_PRESTAMOSSUGERIDOS = valorPrima;
                }
                else { MensajeError('Error al actualizar ingresos'); }
            }
        });
    }
    else {
        $($("#txtMontoManual")).parsley().validate();
        $($("#txtPlazoManual")).parsley().validate();
    }
});

/* Actualizar el monto y plazo a Financiar de la lista de prestamos sugeridos */
// DEBUGGEAR ESTE
var prestamoSeleccionado = [];
$(document).on('click', 'button#btnSeleccionarPMO', function () {
    prestamoSeleccionado = {
        fnMontoOfertado: $(this).data('monto'),
        fiPlazo: $(this).data('plazo'),
        fnCuotaQuincenal: $(this).data('cuota'),
    }
    $("#modalMontoFinanciar").modal();
});

// DEBUGGEAR ESTE
$("#btnConfirmarPrestamoAprobado").click(function () {

    var pmo = prestamoSeleccionado;
    var valorGlobal = pmo.fnMontoOfertado;
    var cantidadPlazos = pmo.fiPlazo;

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ActualizarPlazoMontoFinanciar',
        data: JSON.stringify({ ValorGlobal: VALORGARANTIA_CARGADA_EN_PRESTAMOSSUGERIDOS, ValorPrima: VALORPRIMA_CARGADA_EN_PRESTAMOSSUGERIDOS, CantidadPlazos: cantidadPlazos, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al actualizar ingresos');
        },
        success: function (data) {
            if (data.d == true) {
                $("#lblMontoPrestamoEscogido").text(valorGlobal);
                $("#lblPlazoEscogido").text(cantidadPlazos);
                gMontoFinal = valorGlobal;
                gPlazoFinal = cantidadPlazos;
                $("#divPrestamoElegido").css('display', '');
                $("#modalMontoFinanciar").modal('hide');
                MensajeExito('Monto a financiar actualizado correctamente');
            }
            else { MensajeError('Error al actualizar ingresos'); }
        }
    });
});

/* Condicionar solicitud */
$("#btnCondicionarSolicitud").click(function () {

    var ddlCondiciones = $("#ddlCondiciones");

    listaCondicionamientos = [];
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/GetCondiciones",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar el lista de condiciones');
            $("#modalCondicionarSolicitud").modal();
        },
        success: function (data) {
            var listaCondiciones = data.d;
            $("#tblCondiciones tbody").empty();
            $("#txtComentarioAdicional").val('');
            ddlCondiciones.empty();
            for (var i = 0; i < listaCondiciones.length; i++) {
                ddlCondiciones.append('<option value="' + listaCondiciones[i].fiIDCondicion + '">' + listaCondiciones[i].fcCondicion + ' | ' + listaCondiciones[i].fcDescripcionCondicion + '</option>');
            }
            $("#modalCondicionarSolicitud").modal();
        }
    });
});

var contadorCondiciones = 0;
var listaCondicionamientos = [];
$("#btnAgregarCondicion").click(function () {

    if ($($("#frmAddCondicion")).parsley().isValid()) {
        var tblCondiciones = $("#tblCondiciones tbody");
        var condicionID = $("#ddlCondiciones :selected").val();
        var descripcionCondicion = $("#ddlCondiciones :selected").text();
        var comentarioAdicional = $("#txtComentarioAdicional").val();
        var btnQuitarCondicion = '<button data-id=' + condicionID + ' data-comentario="' + comentarioAdicional + '" id="btnQuitarCondicion" class="btn btn-sm btn-danger">Quitar</button>';
        var newRowContent = '<tr><td>' + descripcionCondicion + '</td><td>' + comentarioAdicional + '</td><td>' + btnQuitarCondicion + '</td></tr>';
        $("#tblCondiciones tbody").append(newRowContent);
        $("#txtComentarioAdicional").val('');

        listaCondicionamientos.push({
            fiIDCondicion: condicionID,
            fcComentarioAdicional: comentarioAdicional
        });
        contadorCondiciones = contadorCondiciones + 1;

    } else { $($("#frmAddCondicion")).parsley().validate(); }
});

$(document).on('click', 'button#btnQuitarCondicion', function () {

    $(this).closest('tr').remove()
    var condicion = {
        fiIDCondicion: $(this).data('id').toString(),
        fcComentarioAdicional: $(this).data('comentario')
    };
    var list = [];
    if (listaCondicionamientos.length > 0) {

        for (var i = 0; i < listaCondicionamientos.length; i++) {

            var iter = {
                fiIDCondicion: listaCondicionamientos[i].fiIDCondicion,
                fcComentarioAdicional: listaCondicionamientos[i].fcComentarioAdicional
            };
            if (JSON.stringify(iter) != JSON.stringify(condicion)) {
                list.push(iter);
            }
        }
    }
    listaCondicionamientos = list;
    contadorCondiciones -= 1;
});

$("#btnCondicionarSolicitudConfirmar").click(function () {

    var otroComentario = $("#razonCondicion").val() != '' ? $("#razonCondicion").val() : '';
    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/CondicionarSolicitud",
        data: JSON.stringify({ SolicitudCondiciones: listaCondicionamientos, fcCondicionadoComentario: otroComentario, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {
            if (data.d == true) {
                $("#modalCondicionarSolicitud").modal('hide');
                //$("#btnCondicionarSolicitud, #btnCondicionarSolicitudConfirmar,#btnAgregarCondicion").prop('disabled', true);
                $("#btnCondicionarSolicitud, #btnCondicionarSolicitudConfirmar,#btnAgregarCondicion").prop('title', 'La solicitud ya está condicionada, esperando actualización de agente de ventas');
                MensajeExito('Estado de la solicitud actualizado');
                actualizarEstadoSolicitud();
            }
            else { MensajeError('E008 Error al condicionar la solicitud, contacte al administrador'); }
        }
    });
});

/* Guardar Informacion del Perfil */
$("#tipoEmpresa, #tipoPerfil, #tipoEmpleo, #buroActual").change(function () {

    var tipoEmpresa = $("#tipoEmpresa :selected").val();
    var tipoPerfil = $("#tipoPerfil :selected").val();
    var tipoEmpleo = $("#tipoEmpleo :selected").val();
    var buroActual = $("#buroActual :selected").val();

    if (tipoEmpresa != '' && tipoPerfil != '' && tipoEmpleo != '' && buroActual != '') {

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/GuardarInformacionAnalisis",
            data: JSON.stringify({ tipoEmpresa: tipoEmpresa, tipoPerfil: tipoPerfil, tipoEmpleo: tipoEmpleo, buroActual: buroActual, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo guardar la información de perfil, contacte al administrador');
            },
            success: function (data) {
                if (data.d != false) {
                    MensajeExito('Informacion de perfil guardada correctamente');
                    actualizarEstadoSolicitud();
                }
                MensajeError('No se pudo guardar la información de perfil, contacte al administrador');
            }
        });

    }
});

/* Enviar solicitud a campo */
$("#btnEnviarCampo").click(function () {
    /* Verificar si la solicitud ya fue enivada a campo antes */
    if (objSolicitud.fiEstadoDeCampo == 0) {
        $("#modalEnviarCampo").modal();
    }
});

$("#btnEnviarCampoConfirmar").click(function () {

    if ($($("#comentarioAdicional")).parsley().isValid()) {

        var fcObservacionesDeCredito = $("#comentarioAdicional").val();
        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/EnviarACampo",
            data: JSON.stringify({ fcObservacionesDeCredito: fcObservacionesDeCredito, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('No se pudo cargar la información, contacte al administrador');
            },
            success: function (data) {
                if (data.d != false) {
                    $("#modalEnviarCampo").modal('hide');
                    $("#btnEnviarCampo, #btnEnviarCampoConfirmar").prop('disabled', true);
                    $("#btnEnviarCampo, #btnEnviarCampoConfirmar").removeClass('btn-warning').addClass('btn-success');
                    $("#btnEnviarCampo, #btnEnviarCampoConfirmar").prop('title', 'La solicitud ya fue enviada a campo');
                    MensajeExito('La solicitud ha sido enviada a campo correctamente');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('Error al enviar la solicitud a campo, contacte al administrador'); }
            }
        });
    } else { $($("#comentarioAdicional")).parsley().validate(); }
});

/* Resolucion de la solicitud */
var resolucion = false;
$("#btnRechazar").click(function () {
    resolucion = false;
    $($("#comentarioRechazar")).parsley().reset();
    $("#comentarioRechazar").val('');
    $("#modalRechazar").modal();
});

$("#btnConfirmarRechazar").click(function () {

    if ($($("#comentarioRechazar")).parsley().isValid()) {

        var objBitacora = {
            fcComentarioResolucion: $("#comentarioRechazar").val()
        };
        var solicitud = {
            fiEstadoSolicitud: 4
        };

        $.ajax({
            type: "POST",
            url: "SolicitudesCredito_Analisis.aspx/SolicitudResolucion",
            data: JSON.stringify({ objBitacora: objBitacora, objSolicitud: solicitud, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Ocurrió un error, contacte al administrador');
            },
            success: function (data) {

                if (data.d != 0) {
                    /* Informacion del analisis */
                    $("#tipoEmpresa").prop('disabled', true);
                    $("#tipoEmpresa").val('');
                    $("#tipoPerfil").prop('disabled', true);
                    $("#tipoPerfil").val('');
                    $("#tipoEmpleo").prop('disabled', true);
                    $("#tipoEmpleo").val('');
                    $("#buroActual").prop('disabled', true);
                    $("#buroActual").val('');
                    $("#montoFinalAprobado").prop('disabled', true);
                    $("#montoFinalAprobado").val('');
                    $("#montoFinalFinanciar").prop('disabled', true);
                    $("#montoFinalFinanciar").val('');
                    $("#plazoFinalAprobado").prop('disabled', true);
                    $("#plazoFinalAprobado").val('');
                    $("#modalRechazar").modal('hide');
                    $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud").prop('disabled', true);
                    $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud").prop('title', 'La solicitud ya fue rechazada');
                    $("#actualizarIngresosCliente").css('display', 'none');
                    $("#pencilOff").css('display', '');
                    MensajeExito('¡Solicitud rechazada correctamente!');
                    actualizarEstadoSolicitud();
                }
                else { MensajeError('E009 Error al actualizar estado de la solicitud, contacte al administrador'); }
            }
        });
    }
    else { $($("#comentarioRechazar")).parsley().validate(); }
});

$("#btnAprobar").click(function () {
    resolucion = true;
    $($("#comentarioAprobar")).parsley().reset();
    $("#comentarioAprobar").val('');
    $("#modalAprobar").modal();
});

$("#btnConfirmarAprobar").click(function () {

    if ($($("#comentarioAprobar")).parsley().isValid()) {

        if (resolucionHabilitada == false) {
            MensajeError('Todavía no se puede determinar una resolución para esta solicitud, tiene procesos sin concluir');
        }
        else if (resolucionHabilitada == true) {

            if ($($("#frmInfoAnalisis")).parsley().isValid()) {

                var objBitacora = {
                    fcComentarioResolucion: $("#comentarioAprobar").val()
                };
                var solicitud = {
                    fiEstadoSolicitud: 7,
                    fiMontoFinalSugerido: gMontoFinal,
                    fiMontoFinalFinanciar: gMontoFinal,
                    fiPlazoFinalAprobado: gPlazoFinal
                };

                $.ajax({
                    type: "POST",
                    url: "SolicitudesCredito_Analisis.aspx/SolicitudResolucion",
                    data: JSON.stringify({ objBitacora: objBitacora, objSolicitud: solicitud, dataCrypt: window.location.href }),
                    contentType: 'application/json; charset=utf-8',
                    error: function (xhr, ajaxOptions, thrownError) {
                        MensajeError('Ocurrió un error, contacte al administrador');
                    },
                    success: function (data) {

                        if (data.d != 0) {
                            /* Informacion del analisis */
                            $("#tipoEmpresa").prop('disabled', true);
                            $("#tipoPerfil").prop('disabled', true);
                            $("#tipoEmpleo").prop('disabled', true);
                            $("#buroActual").prop('disabled', true);
                            $("#montoFinalAprobado").prop('disabled', true);
                            $("#montoFinalFinanciar").prop('disabled', true);
                            $("#plazoFinalAprobado").prop('disabled', true);
                            $("#modalAprobar").modal('hide');
                            $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud").prop('disabled', true);
                            $("#modalAprobar, #btnRechazar, #btnCondicionarSolicitud").attr('title', 'La solicitud ya fue aprobada');
                            $("#actualizarIngresosCliente").css('display', 'none');
                            $("#pencilOff").css('display', '');
                            MensajeExito('¡Solicitud aprobada correctamente!');
                            actualizarEstadoSolicitud();
                        }
                        else { MensajeError('E010 Error al actualizar estado de la solicitud, contacte al administrador'); }
                    }
                });
            }
            else {
                MensajeError('Información de análisis incompleta');
                $($("#frmInfoAnalisis")).parsley().validate();
            }
        }
    }
    else { $($("#comentarioAprobar")).parsley().validate(); }
});

/* Cargar detalles del procesamiento de la solicitud en el modal */
$('#tblEstadoSolicitud tbody').on('click', 'tr', function () {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/CargarEstadoSolicitud",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {
            if (data.d != null) {

                var habilitarResolucion = true;
                var rowDataSolicitud = data.d;
                var ProcesoPendiente = '/Date(-2208967200000)/';

                /* Limpiar tabla de detalles del estado de la solicitud */
                var tablaEstatusSolicitud = $('#tblDetalleEstado tbody');
                tablaEstatusSolicitud.empty();

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
                rowDataSolicitud.fdEnAnalisisInicio == ProcesoPendiente ? habilitarResolucion = false : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Análisis</td>' +
                    '<td>' + EnAnalisisInicio + '</td>' +
                    '<td>' + EnAnalisisFin + '</td>' +
                    '<td>' + tiempoAnalisis + '</td>' +
                    '</tr>');

                var EnCampoInicio = rowDataSolicitud.fdEnvioARutaAnalista != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnvioARutaAnalista) : '';
                var EnCampoFin = rowDataSolicitud.fdEnCampoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnCampoFin) : '';
                var tiempoCampo = EnCampoInicio != '' ? EnCampoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnvioARutaAnalista, rowDataSolicitud.fdEnCampoFin) : '' : '';
                tiempoCampo == '' ? habilitarResolucion = false : '';

                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Campo</td>' +
                    '<td>' + EnCampoInicio + '</td>' +
                    '<td>' + EnCampoFin + '</td>' +
                    '<td>' + tiempoCampo + '</td>' +
                    '</tr>');

                var CondicionadoInicio = rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondicionadoInicio) : '';
                var CondificionadoFin = rowDataSolicitud.fdCondificionadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondificionadoFin) : '';
                var tiempoCondicionado = CondicionadoInicio != '' ? CondificionadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdCondicionadoInicio, rowDataSolicitud.fdCondificionadoFin) : '' : '';

                if (rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente) {

                    if (rowDataSolicitud.fdCondificionadoFin == ProcesoPendiente) {
                        habilitarResolucion = false;
                    }
                }
                tablaEstatusSolicitud.append('<tr>' +
                    '<td>Condicionado</td>' +
                    '<td>' + CondicionadoInicio + '</td>' +
                    '<td>' + CondificionadoFin + '</td>' +
                    '<td>' + tiempoCondicionado + '</td>' +
                    '</tr>');

                var ReprogramadoInicio = rowDataSolicitud.fdReprogramadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoInicio) : '';
                var ReprogramadoFin = rowDataSolicitud.fdReprogramadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoFin) : '';
                var tiempoReprogramado = ReprogramadoInicio != '' ? ReprogramadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdReprogramadoInicio, rowDataSolicitud.fdReprogramadoFin) : '' : '';

                if (rowDataSolicitud.fdReprogramadoInicio != ProcesoPendiente) {

                    if (rowDataSolicitud.fdReprogramadoFin == ProcesoPendiente) {
                        habilitarResolucion = false;
                    }
                }
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

                /* Verificar si ya se tomó una resolución de la solicitud */
                if (rowDataSolicitud.fiEstadoSolicitud == 7) {
                    $("#lblResolucion").text('Aprobado');
                    $("#lblResolucion").removeClass('text-warning');
                    $("#lblResolucion").addClass('text-success');
                    habilitarResolucion = false;
                }
                else if (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5) {
                    $("#lblResolucion").text('Rechazado');
                    $("#lblResolucion").removeClass('text-warning');
                    $("#lblResolucion").addClass('text-danger');
                    habilitarResolucion = false;
                }
                else if (rowDataSolicitud.fdEnTramiteFin != ProcesoPendiente) {
                    $("#lblResolucion").text('En análisis');
                }
                var contadorComentariosDetalle = 0;

                /* Razon reprogramado */
                if (rowDataSolicitud.fcReprogramadoComentario != '') {
                    $("#lblReprogramado").text(rowDataSolicitud.fcReprogramadoComentario);
                    $("#divReprogramado").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                /* Razon condicionado */
                if (rowDataSolicitud.fcCondicionadoComentario != '') {
                    $("#lblCondicionado").text(rowDataSolicitud.fcCondicionadoComentario);
                    $("#divCondicionado").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                /* Observaciones documentacion */
                if (rowDataSolicitud.fcComentarioValidacionDocumentacion != '') {
                    $("#lblDocumentacionComentario").text(rowDataSolicitud.fcComentarioValidacionDocumentacion);
                    $("#divDocumentacionComentario").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                /* Observaciones info personal */
                if (rowDataSolicitud.fcComentarioValidacionInfoPersonal != '') {
                    $("#lblInfoPersonalComentario").text(rowDataSolicitud.fcComentarioValidacionInfoPersonal);
                    $("#divInfoPersonal").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                /* Observaciones info laboral */
                if (rowDataSolicitud.fcComentarioValidacionInfoLaboral != '') {
                    $("#lblInfoLaboralComentario").text(rowDataSolicitud.fcComentarioValidacionInfoLaboral);
                    $("#divInfoLaboral").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                /* Observaciones referencias */
                if (rowDataSolicitud.fcComentarioValidacionReferenciasPersonales != '') {
                    $("#lblReferenciasComentario").text(rowDataSolicitud.fcComentarioValidacionReferenciasPersonales);
                    $("#divReferencias").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                /* Comentario para campo depto de gestoria */
                if (rowDataSolicitud.fcObservacionesDeCredito != '') {
                    $("#lblCampoComentario").text(rowDataSolicitud.fcObservacionesDeCredito);
                    $("#divCampo").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                /* Observaciones de gestoria */
                if (rowDataSolicitud.fcObservacionesDeGestoria != '') {
                    $("#lblGestoriaComentario").text(rowDataSolicitud.fcObservacionesDeGestoria);
                    $("#divGestoria").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                /* Observaciones de gestoria */
                if (rowDataSolicitud.fcComentarioResolucion != '') {
                    $("#lblResolucionComentario").text(rowDataSolicitud.fcComentarioResolucion);
                    $("#divComentarioResolucion").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                /* Validar si no hay detalles que mostrar */
                if (contadorComentariosDetalle == 0) {
                    $("#divNoHayMasDetalles").css('display', '');
                }
                else {
                    $("#divNoHayMasDetalles").css('display', 'none');
                }
                $("#modalEstadoSolicitud").modal();

                /* Validar si ya se puede tomar una resolucion */
                resolucionHabilitada = habilitarResolucion;
                $("#btnRechazar").prop('disabled', false);
                $("#btnRechazar").prop('title', 'Resolución final de la solicitud');

                if (habilitarResolucion == true) {
                    $("#btnAprobar").prop('disabled', false);
                    $("#btnAprobar").prop('title', 'Resolución final de la solicitud');
                    $("#actualizarIngresosCliente").css('display', 'none');
                    $("#pencilOff").css('display', '');
                }
                if (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7) {
                    var decision = rowDataSolicitud.fiEstadoSolicitud == 7 ? 'aprobada' : 'rechazada';
                    $("#btnRechazar, #btnAprobar").prop('disabled', true);
                    $("#btnRechazar, #btnAprobar").prop('title', 'La solicitud ya fue ' + decision);
                    $("#actualizarIngresosCliente").css('display', 'none');
                    $("#pencilOff").css('display', '');
                }
            }
            else { MensajeError('Error al cargar estado de la solicitud, contacte al administrador'); }
        }
    });
});

/* Actualizar comentario sobre una referencia personal radiofaro */
var comentarioActual = '';
var IDReferencia = '';
var btnReferenciaSeleccionada = '';

$(document).on('click', 'button#btnComentarioReferencia', function () {
    //debugger;
    btnReferenciaSeleccionada = $(this);
    comentarioActual = $(this).data('comment');
    IDReferencia = $(this).data('id');
    var nombreReferencia = $(this).data('nombreref');
    var nombreReferencia2 = nombreReferencia;

    $("#txtObservacionesReferencia").val(comentarioActual);
    var InformacionDetalleReferencia = "Muy Buen Dia Sr" + " " + nombreReferencia2 + " " + "Le Saludamos de parte de Crediflash" + "\n" + "El Motivo de mi llamada es porque el señor " + nombreCompletoClienteR  + " lo puso como su referencia,"
        + "\n" + "Solo queremos validar unos datos , tiene un minuto de su tiempo que me pueda brindar." + "\n"
        +  "1) Usted conoce al señor(a) " + nombreCompletoClienteR + "\n" + "2)  Me podría confirmar a que se dedica el señor(a) " + nombreCompletoClienteR + "\n"
        + "3) Usted sabe donde reside actualmente el señor(a) " + nombreCompletoClienteR + "\n" + " Gracias por atender mi llamada" + "\n" + " Pase un excelente Dia "

  
    $("#txtDetalleReferencia").text(InformacionDetalleReferencia);



    $("#lblNombreReferenciaModal").text(nombreReferencia);

    if (comentarioActual != '' && comentarioActual != 'Sin comunicacion') {
        //$("#txtObservacionesReferencia").prop('disabled', true);
       // $("#btnComentarioReferenciaConfirmar,#btnReferenciaSinComunicacion").prop('disabled', true).removeClass('btn-primary').addClass('btn-secondary');
    }
    else if (comentarioActual == 'Sin comunicacion') {
        $("#txtObservacionesReferencia").prop('disabled', false);
        $("#btnReferenciaSinComunicacion").prop('disabled', true);
        $("#btnComentarioReferenciaConfirmar").prop('disabled', false).removeClass('btn-secondary').addClass('btn-primary');
    }
    else {
        $("#txtObservacionesReferencia").prop('disabled', false);
        $("#btnComentarioReferenciaConfirmar,#btnReferenciaSinComunicacion").prop('disabled', false);
    }
    $("#modalComentarioReferencia").modal();
});

$("#btnComentarioReferenciaConfirmar").click(function () {

   

    if ($($("#frmObservacionReferencia")).parsley().isValid()) {

        $("#frmObservacionReferencia").submit(function (e) { e.preventDefault(); });


        comentarioActual = "(CONFIRMAR) " + $('#txtObservacionesReferencia').val();

        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/ComentarioReferenciaPersonal',
            data: JSON.stringify({ IDReferencia: IDReferencia, comentario: comentarioActual, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al actualizar estado de la referencia personal');
            },
            success: function (data) {
                if (data.d == true) {
                    $("#modalComentarioReferencia").modal('hide');
                    MensajeExito('Observaciones de la referencia personal actualizadas correctamente');
                    btnReferenciaSeleccionada.data('comment', comentarioActual);
                    btnReferenciaSeleccionada.closest('tr').removeClass('text-danger').addClass('tr-exito');
                    btnReferenciaSeleccionada.removeClass('mdi mdi-pencil').removeClass('mdi mdi-call-missed text-danger').addClass('mdi mdi-check-circle-outline tr-exito');
                }
                else { MensajeError('Error al actualizar observaciones de la referencia personal'); }
            }
        });
    }
    else { $($("#frmObservacionReferencia")).parsley().validate(); }
});

$("#btnReferenciaSinComunicacion").click(function () {

    //comentarioActual = 'Sin comunicacion';
     comentarioActual ="(SIN CONFIRMAR) " +  $('#txtObservacionesReferencia').val();

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ComentarioReferenciaPersonal',
        data: JSON.stringify({ IDReferencia: IDReferencia, comentario: comentarioActual, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al actualizar estado de la referencia personal');
        },
        success: function (data) {
            if (data.d == true) {
                $("#modalComentarioReferencia").modal('hide');
                MensajeExito('Estado de la referencia personal actualizado correctamente');
                btnReferenciaSeleccionada.data('comment', comentarioActual);
                btnReferenciaSeleccionada.removeClass('mdi mdi-check-circle-outline tr-exito').removeClass('mdi mdi-pencil').addClass('mdi mdi-call-missed text-danger');
                btnReferenciaSeleccionada.closest('tr').addClass('text-danger');
            }
            else { MensajeError('Error al actualizar estado de la referencia personal'); }
        }
    });
});

$("#btnEliminarReferencia").click(function () {
    $("#modalComentarioReferencia").modal('hide');
    $("#modalEliminarReferencia").modal('show');
});

$("#btnEliminarReferenciaConfirmar").click(function () {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/EliminarReferenciaPersonal',
        data: JSON.stringify({ IDReferencia: IDReferencia, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al eliminar referencia personal');
        },
        success: function (data) {
            if (data.d == true) {
                $(btnReferenciaSeleccionada).closest('tr').remove();
                $("#modalEliminarReferencia").modal('hide');
                MensajeExito('La referencia personal ha sido eliminada correctamente');
            }
            else { MensajeError('Error al eliminar referencia personal'); }
        }
    });
});

/* Visualizar informacion de aval del cliente */
var avalID = 0;
$(document).on('click', 'button#btnDetalleAvalModal', function () {
    

    var IDAval = $(this).data('id');
    avalID = $(this).data('id');

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/DetallesAval',
        data: JSON.stringify({ IDAval: IDAval, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar detalles del aval');
        },
        success: function (data) {

            if (data.d != null) {
                var info = data.d;
                avalID = info.fiIDAval;
                $("#lblIdentidadAval").html(info.fcIdentidadAval);
                $("#lblFechaCreacionAval").html(FechaFormato(info.fdFechaCrea));
                $("#lblEstadoAval").html(info.fbAvalActivo == true ? 'Activo' : 'Inactivo');
                var classEstado = info.fbAvalActivo == true ? 'text-info' : 'text-danger';
                $("#lblEstadoAval").addClass(classEstado);
                var nombreCompletoAval = info.fcPrimerNombreAval + ' ' + info.fcSegundoNombreAval + ' ' + info.fcPrimerApellidoAval + ' ' + info.fcSegundoApellidoAval;
                $("#txtNombreCompletoAval").val(nombreCompletoAval);
                $("#txtTelefonoAval").val(info.fcTelefonoAval);
                $("#txtRTNAval").val(info.RTNAval);
                var fechaNacAval = FechaFormato(info.fdFechaNacimientoAval);
                $("#txtFechaNacimientoAval").val(fechaNacAval.split(' ')[0]);
                $("#txtSexoAval").val(info.fcSexoAval == 'M' ? 'Masculino' : 'Femenino');
                $("#txtCorreoAval").val(info.fcCorreoElectronicoAval);
                $("#txtLugarTrabajoAval").val(info.fcNombreTrabajo);
                var fechaIngresoAval = FechaFormato(info.fcFechaIngreso);
                $("#txtFechaIngresoAval").val(fechaIngresoAval.split(' ')[0]);
                $("#txtPuestoAsignadoAval").val(info.fcPuestoAsignado);
                $("#txtIngresosMensualesAval").val(addFormatoNumerico(info.fiIngresosMensuales));
                $("#txtTelEmpresaAval").val(info.fdTelefonoEmpresa);
                $("#txtExtensionRRHHAval").val(info.fcExtensionRecursosHumanos);
                $("#txtExtensionAval").val(info.fcExtensionAval);
                $("#modalDetallesAval").modal();
            } else {
                MensajeError('Error al cargar detalles del aval');
            }
        }
    });
});

$("#btnMasDetallesAval").click(function () {

    if (avalID != 0) {
        $.ajax({
            type: "POST",
            url: 'SolicitudesCredito_Analisis.aspx/EncriptarParametroDetallesAval',
            data: JSON.stringify({ parametro: avalID, dataCrypt: window.location.href }),
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeError('Error al redireccionar a pantalla de detalles del aval');
            },
            success: function (data) {
                if (data.d != null) {
                    var parametros = data.d;
                    //window.open('../Aval/Aval_Detalles.aspx?', "toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=400,height=400" );
                    window.open("../Aval/Aval_Detalles.aspx?" + parametros  , "_blank", "toolbar=yes,scrollbars=yes,resizable=yes,top=500,left=500,width=700,height=600");
                }
                else { MensajeError('Error al redireccionar a pantalla de detalles del aval'); }
            }
        });
    }
    else { MensajeError('Error al cargar detalles del aval'); }
});

/* Cargar buro externo */
$("#btnHistorialExterno").click(function () {

    MensajeInformacion('Cargando buro externo');
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ObtenerUrlEncriptado',
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

/* Calculos de los prestamos */
function prestamoEfectivo(plazoQuincenal, prestamoAprobado) {
//debugger;
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/CalculoPrestamo',
        data: JSON.stringify({ ValorPrestamo: prestamoAprobado, ValorPrima: 0.00, CantidadPlazos: plazoQuincenal, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            //debugger;
            MensajeError('Error al realizar calculo del préstamo');
            gMontoFinal = 0;
            gPlazoFinal = 0;
        },
        success: function (data) {
           // debugger;
            var objCalculo = data.d;
            console.log(objCalculo);
            if (objCalculo != null) {

                /* Variables globales */
                gMontoFinal = objCalculo.ValoraFinanciar;
                gPlazoFinal = plazoQuincenal;
                $("#lblMontoFinanciarEfectivo").text(addFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblMontoCuotaEfectivo").text(addFormatoNumerico(objCalculo.CuotaQuincenal));
                $("#lblTituloCuotaEfectivo").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota);

                /* Mostrar div del calculo del prestamo efectivo */
                $("#lblPrima").css('display', 'none');
                $("#lblMontoPrima").css('display', 'none');
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                $("#divPrestamoEfectivo").css('display', '');

                //$('#lblResumenValorGarantia').text(addFormatoNumerico(rowDataSolicitud.fdValorPmoSugeridoSeleccionado)); // Ficha de resumen
                //$('#lblResumenValorPrima').text(addFormatoNumerico(rowDataSolicitud.fdValorPmoSugeridoSeleccionado)); // Ficha de resumen
                $('#lblResumenValorFinanciar').text(addFormatoNumerico(objCalculo.ValoraFinanciar)); // ficha de resumen
                $('#lblResumenCuota').text(addFormatoNumerico(objCalculo.CuotaQuincenal)); // ficha de resumen
                $("#lblResumenCuotaTitulo").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota); // ficha de resumen
            } else {
                MensajeError('Error al realizar calculo del préstamo');
                gMontoFinal = 0;
                gPlazoFinal = 0;
            }
        }
    });
}

function prestamoMoto(ValorPrima, valorDeLaMoto, plazoQuincenal) {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/CalculoPrestamo',
        data: JSON.stringify({ ValorPrestamo: valorDeLaMoto, ValorPrima: ValorPrima, CantidadPlazos: plazoQuincenal, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar calculo del préstamo');
            gMontoFinal = 0;
            gPlazoFinal = 0;
        },
        success: function (data) {

            var objCalculo = data.d;
            if (objCalculo != null) {

                /* variables globales */
                gMontoFinal = objCalculo.ValoraFinanciar;
                gPlazoFinal = plazoQuincenal;
                $("#lblMontoFinanciarMoto").text(addFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblTituloCuotaMoto").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota);
                $("#lblMontoCuotaMoto").text(addFormatoNumerico(objCalculo.CuotaQuincenal));

                /* Mostrar div del calculo del prestamo moto */
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                $("#divPrestamoMoto").css('display', '');

                $("#lblResumenValorGarantiaTitulo,#lblResumenValorGarantia, #lblResumenValorPrimaTitulo,#lblResumenValorPrima").css('display', '');
                $('#lblResumenValorGarantia').text(addFormatoNumerico(valorDeLaMoto)); // Ficha de resumen
                $('#lblResumenValorPrima').text(addFormatoNumerico(ValorPrima)); // Ficha de resumen
                $('#lblResumenValorFinanciar').text(addFormatoNumerico(objCalculo.ValoraFinanciar)); // Ficha de resumen
                $('#lblResumenCuota').text(addFormatoNumerico(objCalculo.CuotaQuincenal)); // Ficha de resumen
                $("#lblResumenCuotaTitulo").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota); //Ficha de resumen
            } else {
                MensajeError('Error al realizar calculo del préstamo');
                gMontoFinal = 0;
                gPlazoFinal = 0;
            }
        }
    });
}

function prestamoConsumo(ValorPrima, valorDelArticulo, plazoQuincenal) {

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/CalculoPrestamo',
        data: JSON.stringify({ ValorPrestamo: valorDelArticulo, ValorPrima: ValorPrima, CantidadPlazos: plazoQuincenal, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar calculo del préstamo');
            gMontoFinal = 0;
            gPlazoFinal = 0;
        },
        success: function (data) {

            var objCalculo = data.d;
         
            if (objCalculo != null) {

                /* variables globales */
                gMontoFinal = objCalculo.ValoraFinanciar;
                gPlazoFinal = plazoQuincenal;
                $("#lblMontoFinanciarMoto").text(addFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblTituloCuotaMoto").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota);
                $("#lblMontoCuotaMoto").text(addFormatoNumerico(objCalculo.CuotaQuincenal));

                /* Mostrar div del calculo del prestamo moto */
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                $("#divPrestamoMoto").css('display', '');

                $("#lblResumenValorGarantiaTitulo,#lblResumenValorGarantia, #lblResumenValorPrimaTitulo,#lblResumenValorPrima").css('display', '');
                $('#lblResumenValorGarantia').text(addFormatoNumerico(valorDelArticulo)); // Ficha de resumen
                $('#lblResumenValorPrima').text(addFormatoNumerico(ValorPrima)); // Ficha de resumen
                $('#lblResumenValorFinanciar').text(addFormatoNumerico(objCalculo.ValoraFinanciar)); // Ficha de resumen
                $('#lblResumenCuota').text(addFormatoNumerico(objCalculo.CuotaQuincenal)); // Ficha de resumen
                $("#lblResumenCuotaTitulo").text(plazoQuincenal + ' Cuotas ' + objCalculo.TipoCuota); //Ficha de resumen
            } else {
                MensajeError('Error al realizar calculo del préstamo');
                gMontoFinal = 0;
                gPlazoFinal = 0;
            }
        }
    });
}

function prestamoAuto(ValorPrima, valorDelAuto, plazoMensual) {
    //debugger;
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/CalculoPrestamoVehiculo',
        data: JSON.stringify({ ValorPrestamo: valorDelAuto, ValorPrima: ValorPrima, CantidadPlazos: plazoMensual, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al realizar calculo del préstamo');
            gMontoFinal = 0;
            gPlazoFinal = 0;
        },
        success: function (data) {
            //console.log(objCalculo);
            var objCalculo = data.d;
            if (objCalculo != null) {
                /* variables globales */
                gMontoFinal = objCalculo.ValoraFinanciar;
                gPlazoFinal = plazoMensual;
                $("#lblMontoFinanciarAuto").text(addFormatoNumerico(objCalculo.ValoraFinanciar));
                $("#lblMontoCuotaTotalAuto").text(addFormatoNumerico(objCalculo.CuotaMensualNeta));
                $("#lblTituloCuotaAuto").text(plazoMensual + ' Cuotas ' + objCalculo.TipoCuota);
                /* Mostrar div del calculo del prestamo auto */
                $("#divCargando,#divCargandoAnalisis").css('display', 'none');
                $("#LogoPrestamo").css('display', '');
                //$("#divPrestamoAuto").css('display', '');

                $("#lblResumenValorGarantiaTitulo,#lblResumenValorGarantia, #lblResumenValorPrimaTitulo,#lblResumenValorPrima").css('display', '');
                $('#lblResumenValorGarantia').text(addFormatoNumerico(valorDelAuto)); // ficha de resumen
                $('#lblResumenValorPrima').text(addFormatoNumerico(ValorPrima)); // ficha de resumen
                $('#lblResumenValorFinanciar').text(addFormatoNumerico(objCalculo.ValoraFinanciar)); // ficha de resumen
                $('#lblResumenCuota').text(addFormatoNumerico(objCalculo.CuotaMensualNeta)); // ficha de resumen
                $("#lblResumenCuotaTitulo").text(plazoMensual + ' Cuotas ' + objCalculo.TipoCuota); // ficha de resumen

            } else {
                MensajeError('Error al realizar calculo del préstamo');
                gMontoFinal = 0;
                gPlazoFinal = 0;
            }
        }
    });
}

var VALORPRIMA_CARGADA_EN_PRESTAMOSSUGERIDOS = 0;
var VALORGARANTIA_CARGADA_EN_PRESTAMOSSUGERIDOS = 0;
function cargarPrestamosSugeridos(ValorProducto, ValorPrima) {

    $("#cargandoPrestamosSugeridosReales").css('display', '');
    var tablaPrestamos = $("#tblPMOSugeridosReales tbody");
    tablaPrestamos.empty();
    MensajeInformacion('Cargando préstamos sugeridos');

    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/GetPrestamosSugeridos',
        data: JSON.stringify({ ValorProducto: ValorProducto, ValorPrima: ValorPrima, dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('Error al cargar préstamos sugeridos');
            $("#cargandoPrestamosSugeridosReales").css('display', 'none');
        },
        success: function (data) {
            if (data.d != null) {
                var objPrestamos = data.d;
                if (objPrestamos.cotizadorProductos.length > 0) {
                    VALORPRIMA_CARGADA_EN_PRESTAMOSSUGERIDOS = ValorPrima;
                    VALORGARANTIA_CARGADA_EN_PRESTAMOSSUGERIDOS = ValorProducto;

                    var listaPmos = data.d.cotizadorProductos;
                    for (var i = 0; i < listaPmos.length; i++) {
                        var botonSeleccionarPrestamo = '<button id="btnSeleccionarPMO" data-monto="' + listaPmos[i].fnMontoOfertado + '" data-plazo="' + listaPmos[i].fiPlazo + '" data-cuota="' + listaPmos[i].fnCuotaQuincenal + '" class="btn mdi mdi-pencil mdi-24px text-info"></button>';
                        tablaPrestamos.append('<tr>'
                            + '<td class="FilaCondensada">' + listaPmos[i].fnMontoOfertado + '</td>'
                            + '<td class="FilaCondensada">' + listaPmos[i].fiPlazo + '</td>'
                            + '<td class="FilaCondensada">' + listaPmos[i].fnCuotaQuincenal + '</td>'
                            + '<td class="FilaCondensada">' + botonSeleccionarPrestamo + '</td>'
                            + '</tr>');
                    }
                    $("#divSinCapacidadPago").css('display', 'none');
                    $("#btnDigitarMontoManualmente, #btnRechazarIncapacidadPagoModal").prop('disabled', true);
                    $("#cargandoPrestamosSugeridosReales").css('display', 'none');
                    $("#lbldivPrestamosSugeridosReales,#tblPMOSugeridosReales,#divPrestamosSugeridosReales").css('display', '');
                }
                else {
                    $("#cargandoPrestamosSugeridosReales").css('display', 'none');
                    $("#btnDigitarMontoManualmente, #btnRechazarIncapacidadPagoModal").prop('disabled', false);
                    $("#tblPMOSugeridosReales,#lbldivPrestamosSugeridosReales,#divPrestamosSugeridosReales").css('display', 'none');
                    $("#divSinCapacidadPago").css('display', '');
                }
            } else { MensajeError('Error al cargar préstamos sugeridos'); }
        }
    });
}

/* Cargar nuevo estado de la solicitud despues de realizarse algun proceso */
function actualizarEstadoSolicitud() {

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_Analisis.aspx/CargarEstadoSolicitud",
        data: JSON.stringify({ dataCrypt: window.location.href }),
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {
            if (data.d != null) {

                var rowDataSolicitud = data.d;
                var habilitarResolucion = true;
                var ProcesoPendiente = '/Date(-2208967200000)/';
                var IconoExito = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
                var IconoPendiente = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
                var IconoRojo = '<i class="mdi mdi mdi-close-circle-outline mdi-18px text-danger"></i>';

                var statusIngreso = '';
                if (rowDataSolicitud.fdEnIngresoInicio != ProcesoPendiente) {
                    statusIngreso = rowDataSolicitud.fdEnIngresoFin != ProcesoPendiente ? IconoExito : IconoPendiente;
                }
                var statusTramite = '';
                if (rowDataSolicitud.fdEnTramiteInicio != ProcesoPendiente) {

                    statusTramite = rowDataSolicitud.fdEnTramiteFin != ProcesoPendiente ? IconoExito : IconoPendiente;
                    if (rowDataSolicitud.fdEnTramiteFin == ProcesoPendiente && rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7) {
                        statusTramite = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                    }
                }
                var statusAnalisis = '';
                if (rowDataSolicitud.fdEnAnalisisInicio != ProcesoPendiente) {

                    if (rowDataSolicitud.fdEnAnalisisFin != ProcesoPendiente) {
                        statusAnalisis = IconoExito;
                    }
                    else {
                        statusAnalisis = IconoPendiente;
                        habilitarResolucion = false;
                    }
                    if (rowDataSolicitud.fdEnAnalisisFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                        statusAnalisis = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                    }
                }
                else { habilitarResolucion = false; }

                var statusCampo = '';
                if (rowDataSolicitud.fdEnvioARutaAnalista != ProcesoPendiente || rowDataSolicitud.fiEstadoDeCampo != 0) {

                    if (rowDataSolicitud.fdEnCampoFin != ProcesoPendiente || rowDataSolicitud.fiEstadoDeCampo == 2) {
                        statusCampo = IconoExito;
                    }
                    else {
                        statusCampo = IconoPendiente;
                        habilitarResolucion = false;
                    }
                    if (rowDataSolicitud.fdEnCampoFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                        statusCampo = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                    }
                    if (rowDataSolicitud.fiEstadoSolicitud == 5) {
                        statusCampo = IconoRojo;
                        habilitarResolucion = false;
                    }
                }
                else { habilitarResolucion = false; }

                var statusCondicionada = '';
                if (rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente) {

                    if (rowDataSolicitud.fdCondificionadoFin != ProcesoPendiente) {
                        statusCondicionada = IconoExito;
                    }
                    else {
                        statusCondicionada = IconoPendiente;
                        habilitarResolucion = false;
                    }
                    if (rowDataSolicitud.fdCondificionadoFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                        statusCondicionada = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                    }
                }

                var statusReprogramado = '';
                if (rowDataSolicitud.fdReprogramadoInicio != ProcesoPendiente) {

                    if (rowDataSolicitud.fdReprogramadoFin != ProcesoPendiente) {
                        statusReprogramado = IconoExito;
                    }
                    else {
                        statusReprogramado = IconoPendiente;
                        habilitarResolucion = false;
                    }
                    if (rowDataSolicitud.fdReprogramadoFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                        statusReprogramado = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                    }
                }

                var statusPasoFinal = '';
                if (rowDataSolicitud.PasoFinalInicio != ProcesoPendiente) {

                    if (rowDataSolicitud.PasoFinalFin != ProcesoPendiente) {
                        statusPasoFinal = IconoExito;
                    } else {
                        statusPasoFinal = IconoPendiente;
                    }

                    if (rowDataSolicitud.PasoFinalFin == ProcesoPendiente && (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7)) {
                        statusPasoFinal = rowDataSolicitud.fiEstadoSolicitud == 7 ? IconoExito : IconoRojo;
                    }
                }
                var resolucionFinal = '';
                if (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7) {

                    if (rowDataSolicitud.fiEstadoSolicitud == 7) {
                        resolucionFinal = IconoExito;
                        habilitarResolucion = false;
                    } else {
                        resolucionFinal = IconoRojo;
                        habilitarResolucion = false;
                    }
                }
                else {
                    resolucionFinal = rowDataSolicitud.PasoFinalInicio != ProcesoPendiente ? IconoPendiente : '';
                }
                $('#tblEstadoSolicitud tbody').empty();
                $('#tblEstadoSolicitud tbody').append('<tr>' +
                    '<td class="text-center">' + statusIngreso + '</td>' +
                    '<td class="text-center">' + statusTramite + '</td>' +
                    '<td class="text-center">' + statusAnalisis + '</td>' +
                    '<td class="text-center">' + statusCampo + '</td>' +
                    '<td class="text-center">' + statusCondicionada + '</td>' +
                    '<td class="text-center">' + statusReprogramado + '</td>' +
                    '<td class="text-center">' + statusPasoFinal + '</td>' +
                    '<td class="text-center">' + resolucionFinal + '</td>' +
                    '</tr>');

                /* Calcular tiempos */
                //var ingresoInicio = rowDataSolicitud.fdEnIngresoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnIngresoInicio) : '';
                //var ingresoFin = rowDataSolicitud.fdEnIngresoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnIngresoFin) : '';
                //var tiempoIngreso = ingresoInicio != '' ? ingresoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnIngresoInicio, rowDataSolicitud.fdEnIngresoFin) : '' : '';

                //var EnTramiteInicio = rowDataSolicitud.fdEnTramiteInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnTramiteInicio) : '';
                //var EnTramiteFin = rowDataSolicitud.fdEnTramiteFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnTramiteFin) : '';
                //var tiempoTramite = EnTramiteInicio != '' ? EnTramiteFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnTramiteInicio, rowDataSolicitud.fdEnTramiteFin) : '' : '';

                //var EnAnalisisInicio = rowDataSolicitud.fdEnAnalisisInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnAnalisisInicio) : '';
                //var EnAnalisisFin = rowDataSolicitud.fdEnAnalisisFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnAnalisisFin) : '';
                //var tiempoAnalisis = EnAnalisisInicio != '' ? EnAnalisisFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnAnalisisInicio, rowDataSolicitud.fdEnAnalisisFin) : '' : '';

                //var EnCampoInicio = rowDataSolicitud.fdEnvioARutaAnalista != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnvioARutaAnalista) : '';
                //var EnCampoFin = rowDataSolicitud.fdEnCampoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdEnCampoFin) : '';
                //var tiempoCampo = EnCampoInicio != '' ? EnCampoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdEnvioARutaAnalista, rowDataSolicitud.fdEnCampoFin) : '' : '';

                //var CondicionadoInicio = rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondicionadoInicio) : '';
                //var CondificionadoFin = rowDataSolicitud.fdCondificionadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdCondificionadoFin) : '';
                //var tiempoCondicionado = CondicionadoInicio != '' ? CondificionadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdCondicionadoInicio, rowDataSolicitud.fdCondificionadoFin) : '' : '';

                //var ReprogramadoInicio = rowDataSolicitud.fdReprogramadoInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoInicio) : '';
                //var ReprogramadoFin = rowDataSolicitud.fdReprogramadoFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.fdReprogramadoFin) : '';
                //var tiempoReprogramado = ReprogramadoInicio != '' ? ReprogramadoFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.fdReprogramadoInicio, rowDataSolicitud.fdReprogramadoFin) : '' : '';

                //var ValidacionInicio = rowDataSolicitud.PasoFinalInicio != ProcesoPendiente ? FechaFormato(rowDataSolicitud.PasoFinalInicio) : '';
                //var ValidacionFin = rowDataSolicitud.PasoFinalFin != ProcesoPendiente ? FechaFormato(rowDataSolicitud.PasoFinalFin) : '';
                //var tiempoValidacion = ValidacionInicio != '' ? ValidacionFin != '' ? diferenciasEntreDosFechas(rowDataSolicitud.PasoFinalInicio, rowDataSolicitud.PasoFinalFin) : '' : '';
                //var timer = rowDataSolicitud.fcTiempoTotalTranscurrido.split(':');
                //$('#tblEstadoSolicitud tbody').append('<tr>' +
                // '<td class="text-center">' + tiempoIngreso + '</td>' +
                // '<td class="text-center">' + tiempoTramite + '</td>' +
                // '<td class="text-center">' + tiempoAnalisis + '</td>' +
                // '<td class="text-center">' + tiempoCampo + '</td>' +
                // '<td class="text-center">' + tiempoCondicionado + '</td>' +
                // '<td class="text-center">' + tiempoReprogramado + '</td>' +
                // '<td class="text-center">' + tiempoValidacion + '</td>' +
                // '<td class="text-center">' + timer + '</td>' +
                // '</tr>');

                /* Validar si ya se puede tomar una resolucion */
                if (rowDataSolicitud.ftAnalisisTiempoValidacionReferenciasPersonales != ProcesoPendiente) {
                    $("#btnEliminarReferenciaConfirmar, #btnValidoReferenciasModal, #btnValidoReferenciasConfirmar,#btnEliminarReferencia,#btnReferenciaSinComunicacion,#btnComentarioReferenciaConfirmar").prop('disabled', true).prop('title', 'Las referencias personales ya fueron validadas').removeClass('btn-warning').addClass('btn-success');
                }

                rowDataSolicitud.ftAnalisisTiempoValidacionReferenciasPersonales == ProcesoPendiente ? habilitarResolucion = false : '';
                rowDataSolicitud.ftAnalisisTiempoValidarDocumentos == ProcesoPendiente ? habilitarResolucion = false : '';
                rowDataSolicitud.ftAnalisisTiempoValidarInformacionLaboral == ProcesoPendiente ? habilitarResolucion = false : '';
                rowDataSolicitud.ftAnalisisTiempoValidarInformacionPersonal == ProcesoPendiente ? habilitarResolucion = false : '';
                resolucionHabilitada = habilitarResolucion;

                if (rowDataSolicitud.fcTipoEmpresa != '' && rowDataSolicitud.fcTipoPerfil != '' && rowDataSolicitud.fcTipoEmpleado != '' && rowDataSolicitud.fcBuroActual != '' && rowDataSolicitud.fiEstadoDeCampo == 0 && rowDataSolicitud.ftAnalisisTiempoValidarDocumentos != ProcesoPendiente && rowDataSolicitud.ftAnalisisTiempoValidarInformacionPersonal != ProcesoPendiente && rowDataSolicitud.ftAnalisisTiempoValidarInformacionLaboral != ProcesoPendiente) {
                    $("#btnEnviarCampo, #btnEnviarCampoConfirmar").prop('disabled', false);
                }

                if (habilitarResolucion == true) {
                    $("#btnAprobar").prop('disabled', false);
                    $("#btnAprobar").prop('title', 'Resolución final de la solicitud');
                } else {
                    $("#btnRechazar").prop('disabled', false);
                    $("#btnRechazar").prop('title', 'Resolución final de la solicitud');
                }

                if (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7) {
                    var decision = rowDataSolicitud.fiEstadoSolicitud == 7 ? 'aprobada' : 'rechazada';
                    $("#btnRechazar, #btnAprobar").prop('disabled', true);
                    $("#btnRechazar, #btnAprobar").prop('title', 'La solicitud ya fue ' + decision);
                    $("#actualizarIngresosCliente,#divPrestamosSugeridosReales").css('display', 'none');
                    $("#pencilOff").css('display', '');
                }
            }
            else {
                MensajeError('E011 Error al actualizar estado de la solicitud, contacte al administrador');
            }
        }
    });
}

/* Funciones varias */
function MensajeInformacion(mensaje) {
    iziToast.info({
        title: 'Info',
        message: mensaje
    });
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

function addFormatoNumerico(nStr) {
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