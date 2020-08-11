var idSolicitud = 0;
var objSolicitud = [];
var resolucionHabilitada = false;
cargarInformacionSolicitud();

function cargarInformacionSolicitud() {

    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_RegistradaDetalles.aspx/CargarInformacionSolicitud" + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeError('No se pudo carga la información, contacte al administrador');//mostrar mensaje de error
        },
        success: function (data) {

            //variable de informacion del cliente
            var rowDataCliente = data.d.cliente;

            //variable de informacion de la solicitud
            var rowDataSolicitud = data.d.solicitud;

            idSolicitud = rowDataSolicitud.fiIDSolicitud;

            //variable de documentacion de la solicitud
            var rowDataDocumentos = data.d.documentos;

            console.log(data.d);

            objSolicitud = data.d.solicitud;

            $("#btnHistorialExterno,#btnHistorialInterno").prop('disabled', false);

            var ProcesoPendiente = '/Date(-2208967200000)/';

            //cargar status de la solicitud
            var statusIngreso = '';
            if (rowDataSolicitud.fdEnIngresoInicio != ProcesoPendiente) {

                if (rowDataSolicitud.fdEnIngresoFin != ProcesoPendiente) {
                    statusIngreso = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
                }
                else {
                    statusIngreso = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
                }
            }

            //estado en tramite de la solicitud
            var statusTramite = '';
            if (rowDataSolicitud.fdEnTramiteInicio != ProcesoPendiente) {

                if (rowDataSolicitud.fdEnTramiteFin != ProcesoPendiente) {
                    statusTramite = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
                }
                else {
                    statusTramite = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
                }
            }

            //validar si ya se inició y terminó el proceso de analisis de la solicitud
            var statusAnalisis = '';
            if (rowDataSolicitud.fdEnAnalisisInicio != ProcesoPendiente) {

                if (rowDataSolicitud.fdEnAnalisisFin != ProcesoPendiente) {
                    statusAnalisis = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
                }
                else {
                    statusAnalisis = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
                }
            }

            //validar si ya se envió a campo y si ya se recibió respuesta de gestoria
            var statusCampo = '';
            if (rowDataSolicitud.fdEnvioARutaAnalista != ProcesoPendiente) {

                if (rowDataSolicitud.fdEnCampoFin != ProcesoPendiente || rowDataSolicitud.fiEstadoDeCampo == 2) {
                    statusCampo = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
                }
                else {
                    statusCampo = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
                }
            }

            //validar si la solicitud está condicionada y sigue sin recibirse actualizacion del agente de ventas
            var statusCondicionada = '';
            if (rowDataSolicitud.fdCondicionadoInicio != ProcesoPendiente) {

                if (rowDataSolicitud.fdCondificionadoFin != ProcesoPendiente) {
                    statusCondicionada = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
                }
                else {
                    statusCondicionada = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
                }
            }

            //validar si la solicitud está reprogramada
            var statusReprogramado = '';
            if (rowDataSolicitud.fdReprogramadoInicio != ProcesoPendiente) {

                if (rowDataSolicitud.fdReprogramadoFin != ProcesoPendiente) {
                    statusReprogramado = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
                }
                else {
                    statusReprogramado = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
                }
            }

            //validar proceso de validacion
            var statusValidacion = '';
            if (rowDataSolicitud.PasoFinalInicio != ProcesoPendiente) {

                if (rowDataSolicitud.PasoFinalFin != ProcesoPendiente) {
                    statusValidacion = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
                }
                else {
                    statusValidacion = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
                }
            }

            //validar si la solicitud ya ha sido aprobada o rechazada
            var resolucionFinal = '';
            if (rowDataSolicitud.fiEstadoSolicitud == 4 || rowDataSolicitud.fiEstadoSolicitud == 5 || rowDataSolicitud.fiEstadoSolicitud == 7) {

                if (rowDataSolicitud.fiEstadoSolicitud == 7) {
                    resolucionFinal = '<i class="mdi mdi-check-circle-outline mdi-18px text-success"></i>';
                }
                else {
                    resolucionFinal = '<i class="mdi mdi-check-circle-outline mdi-18px text-danger"></i>';
                }
            }
            else {
                if (rowDataSolicitud.PasoFinalInicio != ProcesoPendiente) {
                    resolucionFinal = '<i class="mdi mdi-check-circle-outline mdi-18px text-warning"></i>';
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

            //informacion principal de la solicitud
            $('#lblNoSolicitud').text(rowDataSolicitud.fiIDSolicitud);
            $('#lblNoCliente').text(rowDataSolicitud.fiIDCliente);
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
            //informacion personal
            var infoPersonal = rowDataCliente.clientesMaster;
            $('#lblRtnCliente').text(infoPersonal.RTNCliente != '' ? infoPersonal.RTNCliente : 'n/a');
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
            //informacion domiciliar
            var infoDomiciliar = rowDataCliente.ClientesInformacionDomiciliar;
            $('#lblDeptoCliente').text(infoDomiciliar.fcNombreDepto);
            $('#lblMunicipioCliente').text(infoDomiciliar.fcNombreMunicipio);
            $('#lblCiudadCliente').text(infoDomiciliar.fcNombreCiudad);
            $('#lblBarrioColoniaCliente').text(infoDomiciliar.fcNombreBarrioColonia);
            $('#lblDireccionDetalladaCliente').text(infoDomiciliar.fcDireccionDetallada);
            $('#lblReferenciaDomicilioCliente').text(infoDomiciliar.fcReferenciasDireccionDetallada);
            //informacion laboral
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
            //informacion conyugal
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
            //referencias personales del cliente
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
            //avales del cliente
            if (rowDataCliente.Avales != null) {
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
                            '<td class="FilaCondensada">' + addComasFormatoNumerico(listaAvales[i].fiIngresosMensuales) + '</td>' +
                            '<td class="FilaCondensada">' + estadoAval + '</td>' +
                            '<td class="FilaCondensada">' + btnDetalleAvalModal + '</td>' +
                            '</tr>');
                    }
                }
                else {
                    $('#tblAvales').css('display', 'none');
                }
            } else {
                $('#tblAvales').css('display', 'none');
            }

            //cargar documentación de la solicitud
            var divDocumentacion = $("#listaDocumentosAdjuntados");
            for (var i = 0; i < rowDataDocumentos.length; i++) {
                divDocumentacion.append("<li>" + rowDataDocumentos[i].DescripcionTipoDocumento + ' (' + rowDataDocumentos[i].fcNombreArchivo +")</li>");
            }

            //informacion del prestamo REQUERIDO
            $('#lblValorPmoSugeridoSeleccionado').text(addComasFormatoNumerico(rowDataSolicitud.fdValorPmoSugeridoSeleccionado));
            $('#lblPlazoSeleccionado').text(addComasFormatoNumerico(rowDataSolicitud.fiPlazoPmoSeleccionado));
            $('#lblIngresosPrecalificado').text(addComasFormatoNumerico(rowDataSolicitud.fdIngresoPrecalificado));
            $('#lblObligacionesPrecalificado').text(addComasFormatoNumerico(rowDataSolicitud.fdObligacionesPrecalificado));
            $('#lblDisponiblePrecalificado').text(addComasFormatoNumerico(rowDataSolicitud.fdDisponiblePrecalificado));

            //calcular capacidad de pago mensual
            var capacidadPago = calcularCapacidadPago(rowDataSolicitud.fiIDTipoPrestamo, rowDataSolicitud.fdObligacionesPrecalificado, rowDataSolicitud.fdIngresoPrecalificado);
            $('#lblCapacidadPagoMensual').text(addComasFormatoNumerico(capacidadPago));

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
            var arraigoLaboral = rowDataSolicitud.fiClienteArraigoLaboralAños + ' años ' + rowDataSolicitud.fiClienteArraigoLaboralMeses + ' meses ' + rowDataSolicitud.fiClienteArraigoLaboralDias + ' dias';
            $('#lblArraigoLaboral').text(arraigoLaboral);

            //informacion del analisis
            $("#tipoEmpresa").prop('disabled', true);
            $("#tipoPerfil").prop('disabled', true);
            $("#tipoEmpleo").prop('disabled', true);
            $("#buroActual").prop('disabled', true);
            $("#montoFinalFinanciar").prop('disabled', true);
            $("#montoFinalAprobado").prop('disabled', true);
            $("#plazoFinalAprobado").prop('disabled', true);                        

            //si se modificaron los ingresos del cliente debido a incongruencia con los comprobantes de ingreso mostrar recalculo con dicho valor
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

            //si ya se determinó un monto a financiar, mostrarlo
            if (rowDataSolicitud.fiMontoFinalFinanciar != 0) {

                //mostrar div del préstamo que se esocgió
                $("#lblMontoPrestamoEscogido").text(addComasFormatoNumerico(rowDataSolicitud.fiMontoFinalFinanciar));
                $("#lblPlazoEscogido").text(rowDataSolicitud.fiPlazoFinalAprobado);
                $("#divPrestamoElegido").css('display', '');
            }

            //si se modificaron los ingresos del cliente y no se ha definido una resolucion para la solicitud, mostrar los PMO sugeridos
            if (rowDataSolicitud.fdEnAnalisisFin == '/Date(-2208967200000)/' && rowDataSolicitud.fnSueldoBaseReal != 0) {

                var ingresosCliente = 0;
                var ingresosCliente = rowDataSolicitud.fnSueldoBaseReal == 0 ? rowDataSolicitud.fdIngresoPrecalificado : rowDataSolicitud.fnSueldoBaseReal + rowDataSolicitud.fnBonosComisionesReal;
                //cambiar
                var codigoProducto = rowDataSolicitud.fiIDTipoPrestamo;
                cargarPrestamosSugeridos(rowDataCliente.clientesMaster.fcIdentidadCliente, ingresosCliente, rowDataSolicitud.fdObligacionesPrecalificado, codigoProducto);
            }
        }
    });
}

//abrir modal de detalles del estado del procesamiento de la solicitud
$('#tblEstadoSolicitud tbody').on('click', 'tr', function () {

    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: "SolicitudesCredito_RegistradaDetalles.aspx/CargarEstadoSolicitud" + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            //mostrar mensaje de error
            MensajeError('No se pudo cargar la información, contacte al administrador');
        },
        success: function (data) {

            if (data.d != null) {

                var rowDataSolicitud = data.d;

                var ProcesoPendiente = '/Date(-2208967200000)/';

                //limpiar tabla de detalles del estado de la solicitud
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

                //verificar si ya se tomó una resolución de la solicitud
                if (rowDataSolicitud.fiEstadoSolicitud == 7) {
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

                //razon reprogramado
                if (rowDataSolicitud.fcReprogramadoComentario != '') {
                    $("#lblReprogramado").text(rowDataSolicitud.fcReprogramadoComentario);
                    $("#divReprogramado").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //razon condicionado
                if (rowDataSolicitud.fcCondicionadoComentario != '') {
                    $("#lblCondicionado").text(rowDataSolicitud.fcCondicionadoComentario);
                    $("#divCondicionado").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones documentacion
                if (rowDataSolicitud.fcComentarioValidacionDocumentacion != '') {
                    $("#lblDocumentacionComentario").text(rowDataSolicitud.fcComentarioValidacionDocumentacion);
                    $("#divDocumentacionComentario").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones info personal
                if (rowDataSolicitud.fcComentarioValidacionInfoPersonal != '') {
                    $("#lblInfoPersonalComentario").text(rowDataSolicitud.fcComentarioValidacionInfoPersonal);
                    $("#divInfoPersonal").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones info laboral
                if (rowDataSolicitud.fcComentarioValidacionInfoLaboral != '') {
                    $("#lblInfoLaboralComentario").text(rowDataSolicitud.fcComentarioValidacionInfoLaboral);
                    $("#divInfoLaboral").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones referencias
                if (rowDataSolicitud.fcComentarioValidacionReferenciasPersonales != '') {
                    $("#lblReferenciasComentario").text(rowDataSolicitud.fcComentarioValidacionReferenciasPersonales);
                    $("#divReferencias").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //comentario para campo depto de gestoria
                if (rowDataSolicitud.fcObservacionesDeCredito != '') {
                    $("#lblCampoComentario").text(rowDataSolicitud.fcObservacionesDeCredito);
                    $("#divCampo").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones de gestoria
                if (rowDataSolicitud.fcObservacionesDeGestoria != '') {
                    $("#lblGestoriaComentario").text(rowDataSolicitud.fcObservacionesDeGestoria);
                    $("#divGestoria").css('display', '');
                    contadorComentariosDetalle += 1;
                }
                //observaciones de gestoria
                if (rowDataSolicitud.fcComentarioResolucion != '') {
                    $("#lblResolucionComentario").text(rowDataSolicitud.fcComentarioResolucion);
                    $("#divComentarioResolucion").css('display', '');
                    contadorComentariosDetalle += 1;
                }

                //validar si no hay detalles que mostrar
                if (contadorComentariosDetalle == 0) {
                    $("#divNoHayMasDetalles").css('display', '');
                }
                else {
                    $("#divNoHayMasDetalles").css('display', 'none');
                }

                if (rowDataSolicitud.fiSolicitudActiva == 0) {
                    $("#divSolicitudInactiva").css('display', '');
                }

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

//modal terminar validacion de documentación---
$("#btnDocumentacionModal").click(function () {
    $("#modalDocumentacion").modal();
});

$("#btnHistorialExterno").click(function () {

    MensajeInformacion('Cargando buro externo');
    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Analisis.aspx/ObtenerUrlEncriptado' + qString,
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

function prestamoEfectivo(plazoQuincenal, prestamoAprobado) {

    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Detalles.aspx/CalculoPrestamo' + qString,
        data: JSON.stringify({ MontoFinanciar: prestamoAprobado, PlazoFinanciar: plazoQuincenal, ValorPrima: '0' }),
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

    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Detalles.aspx/CalculoPrestamo' + qString,
        data: JSON.stringify({ MontoFinanciar: valorDeLaMoto, PlazoFinanciar: plazoQuincenal, ValorPrima: ValorPrima }),
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
            else { MensajeError('Error al realizar calculo del préstamo');}
        }
    });
}

function prestamoAuto(ValorPrima, valorDelAuto, plazoMensual) {

    var qString = "?" + window.location.href.split("?")[1];
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_Detalles.aspx/CalculoPrestamo' + qString,
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

function cargarPrestamosSugeridos(identidad, ingresos, obligaciones, codigoProducto) {

    $("#cargandoPrestamosSugeridosReales").css('display', '');

    MensajeInformacion('Cargando préstamos sugeridos');
    $.ajax({
        type: "POST",
        url: 'SolicitudesCredito_RegistradaDetalles.aspx/GetPrestamosSugeridos',
        data: JSON.stringify({ identidad: identidad, ingresos: ingresos, obligaciones: obligaciones, codigoProducto: codigoProducto }),
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

                    var botonSeleccionarPrestamo = '<button id="btnSeleccionarPMO" data-monto="' + listaPmos[i].fnMontoOfertado + '" data-plazo="' + listaPmos[i].fiPlazo + '" data-cuota="' + listaPmos[i].fnCuotaQuincenal + '" class="btn mdi mdi-pencil mdi-24px text-info" disabled="disabled"></button>';
                    tablaPrestamos.append('<tr>'
                        + '<td>' + listaPmos[i].fnMontoOfertado + '</td>'
                        + '<td>' + listaPmos[i].fiPlazo + '</td>'
                        + '<td>' + listaPmos[i].fnCuotaQuincenal + '</td>'
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

//cuando se abra un modal, ocultar el scroll del BODY y deja solo el del modal (en caso de que este tenga scroll)
$(window).on('hide.bs.modal', function () {
    const body = document.body;
    const scrollY = body.style.top;
    body.style.position = '';
    body.style.top = '';
    window.scrollTo(0, parseInt(scrollY || '0') * -1);
    $("body").css('padding-right', '0');
});

//cuando se cierre un modal
$(window).on('show.bs.modal', function () {
    const scrollY = document.documentElement.style.getPropertyValue('--scroll-y');
    const body = document.body;
    body.style.position = 'fixed';
    body.style.top = `-${scrollY}`;
});

window.addEventListener('scroll', () => {
    document.documentElement.style.setProperty('--scroll-y', `${window.scrollY}px`);
});

// mensaje de error
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

// formatear fechas
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

function diferenciasEntreDosFechas(fechaInicio, fechaFin) {

    //calcular tiempo en ingreso
    var inicio = new Date(FechaFormatoGuiones(fechaInicio));

    var fin = new Date(FechaFormatoGuiones(fechaFin));

    var tiempoResta = (fin.getTime() - inicio.getTime()) / 1000;

    //calcular dias en ingreso
    var dias = Math.floor(tiempoResta / 86400);
    //tiempoResta = tiempoResta >= 86400 ? dias * 86400 : tiempoResta;

    //calcular horas en ingreso
    var horas = Math.floor(tiempoResta / 3600) % 24;
    //tiempoResta = tiempoResta >= 3600 ? horas * 3600 : tiempoResta;

    //calcular minutos en ingreso
    var minutos = Math.floor(tiempoResta / 60) % 60;
    //tiempoResta = tiempoResta >= 3600 ? minutos * 3600 : tiempoResta;

    //calcular segundos en ingreso
    var segundos = tiempoResta % 60;

    var diferencia = pad2(dias) + ':' + pad2(horas) + ':' + pad2(minutos) + ':' + pad2(segundos);

    return diferencia;
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