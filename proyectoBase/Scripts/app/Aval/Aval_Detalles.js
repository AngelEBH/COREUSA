CargarDetallesAval();

function CargarDetallesAval() {

    var qString = "?" + window.location.href.split("?")[1];

    $.ajax({
        type: "POST",
        url: "Aval_Detalles.aspx/DetallesAval" + qString,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeError('No se pudo carga la información, contacte al administrador');
        },
        success: function (data) {
            debugger;
            var infoPersonal = data.d.AvalMaster;
            var infoLaboral = data.d.AvalInformacionLaboral;
            var infoDomiciliar = data.d.AvalInformacionDomiciliar;
            var AvalInformacionConyugal = data.d.AvalInformacionConyugal;
            var AvalDocumentos = data.d.AvalDocumentos;

            var nombreCompletoAval = infoPersonal.fcPrimerNombreAval + ' ' + infoPersonal.fcSegundoNombreAval + ' ' + infoPersonal.fcPrimerApellidoAval + ' ' + infoPersonal.fcSegundoApellidoAval;

            $('#lblNombreAval').text(nombreCompletoAval);
            $('#lblIdentidadAval').text(infoPersonal.fcIdentidadAval);
            $('#lblRtnAval').text(infoPersonal.RTNAval);
            //informacion personal
            $('#lblNumeroTelefono').text(infoPersonal.fcTelefonoAval);
            $('#lblNumeroTelefono').attr('href', 'tel:+' + infoPersonal.fcTelefonoAval.replace(' ', '').replace('-', '').replace('(', '').replace(')', ''));
            $('#lblNacionalidad').text(infoPersonal.fcDescripcionNacionalidad);
            var fechaNacimientoAval = FechaFormato(infoPersonal.fdFechaNacimientoAval);
            $('#lblFechaNacimientoAval').text(fechaNacimientoAval.split(' ')[0]);
            $('#lblCorreoAval').text(infoPersonal.fcCorreoElectronicoAval);
            $('#lblProfesionAval').text(infoPersonal.fcProfesionOficioAval);
            $('#lblSexoAval').text(infoPersonal.fcSexoAval == 'M' ? 'Masculino' : 'Femenino');
            $('#lblEstadoCivilAval').text(infoPersonal.fcDescripcionEstadoCivil);
            $('#lblViviendaAval').text(infoPersonal.fcDescripcionVivienda);
            $('#lblTiempoResidirAval').text(infoPersonal.fiTiempoResidir > 2 ? 'Más de 2 años' : infoPersonal.fiTiempoResidir + ' años');
            //informacion domiciliar
            $('#lblDeptoAval').text(infoDomiciliar.fcNombreDepto);
            $('#lblMunicipioAval').text(infoDomiciliar.fcNombreMunicipio);
            $('#lblCiudadAval').text(infoDomiciliar.fcNombreCiudad);
            $('#lblBarrioColoniaAval').text(infoDomiciliar.fcNombreBarrioColonia);
            $('#lblDireccionDetalladaAval').text(infoDomiciliar.fcDireccionDetallada);
            $('#lblReferenciaDomicilioAval').text(infoDomiciliar.fcReferenciasDireccionDetallada);
            //informacion laboral
            $('#lblNombreTrabajoAval').text(infoLaboral.fcNombreTrabajo);
            $('#lblIngresosMensualesAval').text(addComasFormatoNumerico(infoLaboral.fiIngresosMensuales));
            $('#lblPuestoAsignadoAval').text(infoLaboral.fcPuestoAsignado);
            var fechaIngresoAval = FechaFormato(infoLaboral.fcFechaIngreso);
            $('#lblFechaIngresoAval').text(fechaIngresoAval.split(' ')[0]);
            $('#lblTelefonoEmpresaAval').text(infoLaboral.fdTelefonoEmpresa);
            $('#lblTelefonoEmpresaAval').attr('href', 'tel:+' + infoLaboral.fdTelefonoEmpresa.replace(' ', '').replace('-', '').replace('(', '').replace(')', ''));
            $('#lblExtensionRecursosHumanos').text(infoLaboral.fcExtensionRecursosHumanos);
            $('#lblExtensionAval').text(infoLaboral.fcExtensionAval);
            $('#lblDeptoEmpresa').text(infoLaboral.fcNombreDepto);
            $('#lblMunicipioEmpresa').text(infoLaboral.fcNombreMunicipio);
            $('#lblCiudadEmpresa').text(infoLaboral.fcNombreCiudad);
            $('#lblBarrioColoniaEmpresa').text(infoLaboral.fcNombreBarrioColonia);
            $('#lblDireccionDetalladaEmpresa').text(infoLaboral.fcDireccionDetalladaEmpresa);
            $('#lblReferenciaUbicacionEmpresa').text(infoLaboral.fcReferenciasDireccionDetallada);
            //informacion conyugal
            if (AvalInformacionConyugal != null) {

                var infoConyugue = AvalInformacionConyugal;
                $('#lblNombreConyugue').text(infoConyugue.fcNombreCompletoConyugue);
                $('#lblIdentidadConyuge').text(infoConyugue.fcIndentidadConyugue);
                var fechaNacimientoConyuge = infoConyugue.fdFechaNacimientoConyugue != null ? FechaFormato(infoConyugue.fdFechaNacimientoConyugue) : '';
                $('#lblFechaNacimientoConygue').text(fechaNacimientoConyuge != '' ? fechaNacimientoConyuge.split(' ')[0] : '');
                $('#lblTelefonoConyugue').text(infoConyugue.fcTelefonoConyugue);
                $('#lblLugarTrabajoConyugue').text(infoConyugue.fcLugarTrabajoConyugue);
                $('#lblTelefonoTrabajoConyugue').text(infoConyugue.fcTelefonoTrabajoConyugue);
                $('#lblIngresosConyugue').text(addComasFormatoNumerico(infoConyugue.fcIngresosMensualesConyugue));
            }
            else {
                $("#titleConyugal").css('display', 'none');
                $("#divConyugueAval").css('display', 'none');
            }

            //cargar documentación de la solicitud
            var divDocumentacionCedula = $("#divDocumentacionCedulaAval");
            var divDocumentacionCedulaModal = $("#divDocumentacionCedulaModal");
            var divDocumentacionDomicilio = $("#divDocumentacionDomicilio");
            var divDocumentacionDomicilioModal = $("#divDocumentacionDomicilioModal");
            var divDocumentacionLaboral = $("#divDocumentacionLaboral");
            var divDocumentacionLaboralModal = $("#divDocumentacionLaboralModal");

            var contador = 0;
            for (var i = 0; i < AvalDocumentos.length; i++) {

                debugger;
                var ruta = AvalDocumentos[i].URLArchivo;

                if (AvalDocumentos[i].fiTipoDocumento == 7) {
                        divDocumentacionCedula.append('<a class="float-left" href="' + ruta + '" title="Documentación identidad">' +
                            '<div class="img-responsive">' +
                            '<img src="' + ruta + '" style="width: 100%; height: auto; float: left; cursor: zoom-in;" />' +
                            '</div>' +
                            '</a>');
                    
                    divDocumentacionCedulaModal.append('<a class="float-left" href="' + ruta + '" title="Documentación identidad">' +
                        '<div class="img-responsive">' +
                        '<img src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                    contador = contador + 1;
                }
                else if (AvalDocumentos[i].fiTipoDocumento == 8) {
                    divDocumentacionDomicilio.append(
                        '<img class="img" src="' + ruta + '" title="Comprobante de domicilio" alt="Documentacion domiciliar" style="width: 50%; height: auto; float:left;" />');
                    divDocumentacionDomicilioModal.append('<a class="float-left" href="' + ruta + '" title="Comprobante de domicilio">' +
                        '<div class="img-responsive">' +
                        '<img src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (AvalDocumentos[i].fiTipoDocumento == 10) {
                    divDocumentacionDomicilio.append(
                        '<img class="img" src="' + ruta + '" title="Croquis domiciliar" alt="Documentacion domiciliar" style="width: 50%; height: auto; float:left;" />');
                    divDocumentacionDomicilioModal.append('<a class="float-left" href="' + ruta + '" title="Croquis domiciliar" alt="Documentacion domiciliar">' +
                        '<div class="img-responsive">' +
                        '<img src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                }
                else if (AvalDocumentos[i].fiTipoDocumento == 9) {
                    divDocumentacionLaboral.append(
                        '<img class="img" src="' + ruta + '" title="Comprobante de ingresos" alt="Comprobante de ingresos" style="width: 50%; height: auto; float:left;"/>');
                    divDocumentacionLaboralModal.append('<a class="float-left" href="' + ruta + '" title="Comprobante de ingresos" alt="Comprobante de ingresos">' +
                        '<div class="img-responsive">' +
                        '<img src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');

                }
                else if (AvalDocumentos[i].fiTipoDocumento == 11) {
                    divDocumentacionLaboral.append(
                        '<img class="img" src="' + ruta + '" title="Croquis empleo" alt="Croquis empleo" style="width: 50%; height: auto; float:left;"/>');
                    divDocumentacionLaboralModal.append('<a class="float-left" href="' + ruta + '" title="Croquis empleo" alt="Croquis empleo">' +
                        '<div class="img-responsive">' +
                        '<img src="' + ruta + '" alt="" width="100" />' +
                        '</div>' +
                        '</a>');
                }
            }
            $("img").imgbox({
                zoom: true,
                drag: true
            });
        }
    });
}

// mensaje de exito
function MensajeExito(mensaje) {
    iziToast.success({
        title: 'Exito',
        message: mensaje
    });
}

// mensaje de error
function MensajeError(mensaje) {
    iziToast.error({
        title: 'Error',
        message: mensaje
    });
}

// resetear campos de formularios
function resetForm($form) {
    $form.find('input:text, input:password, input:file,input[type="date"],input[type="email"], select, textarea').val('');
    $form.find('input:radio, input:checkbox')
        .prop('checked', false).removeAttr('selected');
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

function pad2(number) {
    return (number < 10 ? '0' : '') + number
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
