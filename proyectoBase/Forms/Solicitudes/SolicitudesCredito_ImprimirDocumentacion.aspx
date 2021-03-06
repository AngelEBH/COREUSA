<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_ImprimirDocumentacion.aspx.cs" Inherits="SolicitudesCredito_ImprimirDocumentacion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <meta name="description" content="Imprimir documentos de solicitudes de crédito" />
    <title>Imprimir documentacion</title>
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/css/unitegallery.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.css" rel="stylesheet" />
    <link href="/CSS/SolicitudesCredito_ImprimirDocumentacion.css?v=20210106150602" rel="stylesheet" />
    <link href="/CSS/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
</head>
<body>
    <form id="frmGuardarPreSolicitud" runat="server">

        <div class="card m-0">
            <div class="card-header pb-1 pt-1">
                <div class="float-right p-1" id="Loader" style="display: none;">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>
                <h6>Imprimir documentación solicitud de crédito N° <span id="lblIdSolicitud" class="font-weight-bold" runat="server"></span></h6>
            </div>
            <div class="card-body pt-0">
                <div class="row mb-0">
                    <div class="col-md-6">
                        <div class="form-group row">
                            <div class="col-md-6 col-12">
                                <label class="col-form-label">Cliente</label>
                                <asp:TextBox ID="txtNombreCliente" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-6 col-12">
                                <label class="col-form-label">Identidad Personal</label>
                                <asp:TextBox ID="txtIdentidadCliente" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">RTN numérico</label>
                                <asp:TextBox ID="txtRtn" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Teléfono</label>
                                <asp:TextBox ID="txtTelefonoCliente" type="tel" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Producto</label>
                                <asp:TextBox ID="txtProducto" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Monto a Financiar</label>
                                <asp:TextBox ID="txtMontoFinalAFinanciar" ReadOnly="true" CssClass="form-control form-control-sm col-form-label text-right" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Plazo <span runat="server" id="lblTipoDePlazo"></span></label>
                                <asp:TextBox ID="txtPlazoFinanciar" ReadOnly="true" CssClass="form-control form-control-sm col-form-label text-right" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Cuota <span runat="server" id="lblTipoDePlazoCuota"></span></label>
                                <asp:TextBox ID="txtValorCuota" ReadOnly="true" CssClass="form-control form-control-sm col-form-label text-right" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 border-left border-gray justify-content-center">

                        <h6>Imprimir documentos</h6>

                        <div class="form-group row mb-0">
                            <div class="col-12">
                                
                               <%-- <button type="button" id="btnContrato" onclick="ExportToPDF('CONTRATO','divContenedorContrato','divContratoPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Contrato
                                </button>--%>
                                  <button type="button" id="" onclick="ExportToPDF('CONTRATO','divContenedorContratoPrestaditoCash','divContratoPrestaditoCashPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Contrato 
                                </button>
                                <button type="button" id="btnPagare" onclick="ExportToPDF('PAGARE','divContenedorPagare','divPagarePDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png'); display: none">
                                    Pagaré
                                </button>

                               <%--  <button type="button" id="btnPlanPago" onclick="ExportToPDF('PLANPAGO','divContenedorPlanDePagos','divPlanDePagosPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Plan de Pago
                                </button>--%>

                                   <button type="button" id="btnPlanPago" onclick="ExportHtmlToPdf('#divPlanDePagosPDF','PLANPAGO')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Plan de Pago
                                </button>


                                <button type="button" id="btnCompromisoLegal" onclick="ExportToPDF('COMPROMISO_LEGAL','divContenedorCompromisoLegal','divCompromisoLegalPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');  display: none" >
                                    Compromiso legal
                                </button>
                                <button type="button" id="btnActaDeCompromiso" onclick="ExportToPDF('ACTA_DE_COMPROMISO','divContenedorActaDeCompromiso','divActaDeCompromisoPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');  display: none">
                                    Acta de compromiso
                                </button>
                                <button type="button" id="btnConvenioComprayVenta" onclick="ExportToPDF('CONVENIO_DE_COMPRA_Y_VENTA_DE_VEHICULOS_PARA_FINANCIAMIENTO','divContenedorConvenioCyV','divConevionCyVPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');  display: none">
                                    C. y V. de vehic. finan.
                                </button>
                                <a href="/Documentos/Recursos/INSPECCION%20DE%20VEHICULO.pdf" download="Inspección vehículo" id="btnInspeccionVehiculo" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/inspection_40px.png');  display: none">Inspección de vehículo
                                </a>
                                <button type="button" id="btnInspeccionSeguroDeVehiculo" onclick="ExportToPDF('INSPECCION_SEGURO_VEHICULO','divContenedorInspeccionSeguro','divInspeccionSeguroPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/inspection_40px.png');  display: none">
                                    Inspección seguro
                                </button>
                                <button type="button" id="btnTraspaso" onclick="ExportToPDF('TRASPASO_CLIENTE','divContenedorTraspaso','divTraspasoPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/resume_40px.png');  display: none">
                                    Traspaso cliente
                                </button>
                                <button type="button" id="btnTraspasoVendedor" onclick="ExportToPDF('TRASPASO_PROPIETARIO','divContenedorTraspasoVendedor','divTraspasoVendedorPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/resume_40px.png');  display: none">
                                    Traspaso propietario
                                </button>
                                <button type="button" id="btnRecibo" onclick="ExportToPDF('RECIBO','divContenedorRecibo','divReciboPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/receipt_40px.png'); display: none">
                                    Recibo
                                </button>
                                <button type="button" id="btnBasicoCPI" onclick="ExportToPDF('BASICO_CPI','divContenedorBasicoCPI','divBasicoCPIPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/vehicle_insurance_40px.png');  display: none">
                                    Básico + CPI
                                </button>
                                <button type="button" id="btnNotaDeEntrega" onclick="ExportToPDF('NOTA_DE_ENTREGA', 'divContenedorNotaDeEntrega', 'divNotaDeEntregaPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/resume_40px.png');  display: none">
                                    Nota de entrega
                                </button>
                                <button type="button" id="btnEnviarCorreoLiquidacion" onclick="EnviarCorreo('Liquidación', 'Liquidación', 'divCorreoLiquidacionPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/send_email_40px.png');  display: none">
                                    Correo Liquidación
                                </button>
                                <button type="button" id="btnEnviarCorreoSeguro" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/send_email_40px.png');  display: none">
                                    Correo Seguro
                                </button>
                                <button type="button" id="btnPortadaExpediente" onclick="ExportToPDF('PORTADA_EXPEDIENTE','divContenedorPortadaExpediente','divPortadaExpedientePDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/resume_40px.png');  display: none">
                                    Portada Expediente
                                </button>
                                <button type="button" id="btnExpediente" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/check_list_40px.png');  display: none">
                                    CHECK LIST
                                </button>
                                <button type="button" id="btnMemorandumExpediente" onclick="ExportToPDF('MEMORANDUM','divContenedorMemorandum','divMemorandumPDF')" runat="server" visible="false" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');  display: none">
                                    Memorandum
                                </button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="row mb-0" id="divInformacionGarantia" runat="server" visible="false">
                    <div class="col-12">
                        <h6 class="border-bottom pb-2">Información de la garantía</h6>
                    </div>
                    <div class="col-12">
                        <div class="row">
                            <div class="col-sm-6">

                                <h6 class="m-0 pt-2">Propietario de la garantía</h6>

                                <div class="form-group row">
                                    <div class="col-sm-6">
                                        <label class="col-form-label">Nombre</label>
                                        <asp:TextBox ID="txtNombrePropietarioGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-sm-6">
                                        <label class="col-form-label">Identidad</label>
                                        <asp:TextBox ID="txtIdentidadPropietarioGarantia" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-6">
                                        <label class="col-form-label">Nacionalidad</label>
                                        <asp:TextBox ID="txtNacionalidadPropietarioGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-6">
                                        <label class="col-form-label">Estado civil</label>
                                        <asp:TextBox ID="txtEstadoCivilPropietarioGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-6">

                                <h6 class="m-0 pt-2">Vendedor de la garantía</h6>

                                <div class="form-group row">
                                    <div class="col-6">
                                        <label class="col-form-label">Nombre</label>
                                        <asp:TextBox ID="txtNombreVendedorGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-6">
                                        <label class="col-form-label">Identidad</label>
                                        <asp:TextBox ID="txtIdentidadVendedorGarantia" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-6">
                                        <label class="col-form-label">Nacionalidad</label>
                                        <asp:TextBox ID="txtNacionalidadVendedorGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="col-6">
                                        <label class="col-form-label">Estado civil</label>
                                        <asp:TextBox ID="txtEstadoCivilVendedorGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6">

                        <h6 class="m-0">Características físicas</h6>

                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">VIN</label>
                                <asp:TextBox ID="txtVIN" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6 pr-1">
                                <label class="col-form-label">Tipo de garantía</label>
                                <asp:TextBox ID="txtTipoDeGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6 pl-1">
                                <label class="col-form-label">Tipo de vehículo</label>
                                <asp:TextBox ID="txtTipoDeVehiculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6 pr-1">
                                <label class="col-form-label">Marca</label>
                                <asp:TextBox ID="txtMarca" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6 pl-1 pr-1">
                                <label class="col-form-label">Modelo</label>
                                <asp:TextBox ID="txtModelo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6 pl-1 pr-1">
                                <label class="col-form-label">Año</label>
                                <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6 pl-1">
                                <label class="col-form-label">Color</label>
                                <asp:TextBox ID="txtColor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6 pr-1">
                                <label class="col-form-label">Matrícula</label>
                                <asp:TextBox ID="txtMatricula" CssClass="form-control form-control-sm" ReadOnly="true" type="text" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6 pl-1 pr-1">
                                <label class="col-form-label">Serie Motor</label>
                                <asp:TextBox ID="txtSerieMotor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6 pl-1 pr-1">
                                <label class="col-form-label">Serie Chasis</label>
                                <asp:TextBox ID="txtSerieChasis" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6 pl-1">
                                <label class="col-form-label">GPS</label>
                                <asp:TextBox ID="txtGPS" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <h6 class="m-0 pt-2">Características mecánicas</h6>

                        <div class="form-group row">
                            <div class="col-sm-4 pr-1">
                                <label class="col-form-label">Cilindraje</label>
                                <asp:TextBox ID="txtCilindraje" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 pl-1 pr-1">
                                <label class="col-form-label">Recorrido</label>
                                <asp:TextBox ID="txtRecorrido" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 pl-1">
                                <label class="col-form-label">Transmisión</label>
                                <asp:TextBox ID="txtTransmision" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 pr-1">
                                <label class="col-form-label">Tipo de combustible</label>
                                <asp:TextBox ID="txtTipoDeCombustible" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 pl-1 pr-1">
                                <label class="col-form-label">Serie 1</label>
                                <asp:TextBox ID="txtSerieUno" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 pl-1">
                                <label class="col-form-label">Serie 2</label>
                                <asp:TextBox ID="txtSerieDos" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-12">
                                <label class="col-form-label">Comentario</label>
                                <textarea id="txtComentario" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 border-left border-gray">
                        <h6>Fotografías de la garantía</h6>
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="align-self-center" id="divGaleriaGarantia" runat="server" style="display: none;"></div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row mr-0 ml-0 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                    <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
                </div>
            </div>
        </div>

        <!-- =============================================================================== -->
        <!-- =============================================================================== -->
        <!-- ========== HTML de los documentos que se exportan a PDF con Html2PDF ========== -->
        <!-- =============================================================================== -->
        <!-- =============================================================================== -->

        <!-- ================================== HTML Contrato =================================== -->
        <div id="divContenedorContrato" class="contenedorPDF">
            <div class="card m-0 divImprimir" id="divContratoPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h6 class="text-center font-weight-bold">CONTRATO DE CRÉDITO PARA COMPRA DE VEHÍCULO</h6>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                Nosotros, 
                                <b class="lblNombreRepresentanteLegal"></b>,
                                <span class="lblEstadoCivilRepresentanteLegal"></span>,
                                <span class="lblProfesionRepresentanteLegal"></span>,
                                con domicilio en la ciudad de
                                <span class="lblCiudadDomicilioRepresentanteLegal"></span>, 
                                Departamento de 
                                <span class="lblDepartamentoDomicilioRepresentanteLegal"></span>,
                                quien actúan en su condición de Representante Legal de la Sociedad Mercantil denominada
                                <b class="lblRazonSocial"></b>
                                empresa domiciliada en la ciudad de 
                                <span class="lblCiudadDomicilioEmpresa"></span>,
                                departamento de
                                <span class="lblDepartamentoDomicilioEmpresa"></span>,
                                llamada en adelante
                                <b class="lblNombreComercial"></b>
                                o <b>PRESTAMISTA</b>; y por otra parte el Sr(a)
                                <asp:Label runat="server" ID="lblNombre_Contrato" class="font-weight-bold"></asp:Label>,
                                mayor de edad, de nacionalidad
                                <asp:Label runat="server" ID="lblNacionalidad_Contrato" class="font-weight-bold"></asp:Label>
                                y de este domicilio, con identidad No.
                                <asp:Label runat="server" ID="lblIdentidad_Contrato" class="font-weight-bold"></asp:Label>
                                con domicilio y dirección en
                                <asp:Label runat="server" ID="lblDireccion_Contrato"></asp:Label>,
                                llamado en adelante <b>EL CIENTE, PRESTATARIO y/o DEUDOR</b>, convienen celebrar el siguiente <b>CONTRATO DE CRÉDITO PARA COMPRA DE VEHICULO</b> y acuerdan lo estipulado en las siguientes clausulas:
                                <b>PRIMERO: OBJETO DEL CONTRATO.- EL CLIENTE </b>declara recibir en este acto de 
                                <b class="lblNombreComercial"></b>,
                                un préstamo por la cantidad de
                                <asp:Label runat="server" ID="lblMontoPrestamoEnPalabras_Contrato" class="font-weight-bold"></asp:Label>
                                (<asp:Label runat="server" ID="lblMontoPrestamo_Contrato"></asp:Label>) de los cuales se destinaran
                                <asp:Label runat="server" ID="lblMontoParaCompraVehiculoEnPalabras_Contrato" class="font-weight-bold"></asp:Label>
                                (<asp:Label runat="server" ID="lblMontoParaCompraVehiculo_Contrato"></asp:Label>)
                                para compra del vehiculo y
                                <asp:Label runat="server" ID="lblMontoParaCompraGPSEnPalabras_Contrato" class="font-weight-bold"></asp:Label>
                                (<asp:Label runat="server" ID="lblMontoParaCompraGPS_Contrato"></asp:Label>)
                                para compra de modulo de monitoreo por GPS y
                                <asp:Label runat="server" ID="lblMontoGastosDeCierreEnPalabras_Contrato" class="font-weight-bold"></asp:Label>
                                (<asp:Label runat="server" ID="lblMontoGastosDeCierre_Contrato"></asp:Label>)
                                por concepto de gastos de cierre y papeleria todo en monda de curso legal en Honduras.
                                <b>-SEGUNDO: CONDICIONES DEL FINANCIAMIENTO.-</b> El préstamo se facilita bajo las siguientes condiciones:
                                <b>A) DESTINO: EL CLIENTE </b>acepta, reconoce y autoriza que la cantidad recibida en préstamo será para compra de:
                            </p>
                            <p>
                                <b>Vehiculo automotor marca
                                    <asp:Label runat="server" ID="lblMarca_Contrato"></asp:Label>
                                    tipo
                                    <asp:Label runat="server" ID="lblTipoVehiculo_Contrato"></asp:Label>
                                    modelo
                                    <asp:Label runat="server" ID="lblModelo_Contrato"></asp:Label>
                                    año
                                    <asp:Label runat="server" ID="lblAnio_Contrato"></asp:Label>
                                    color
                                    <asp:Label runat="server" ID="lblColor_Contrato"></asp:Label>
                                    con cilindraje
                                    <asp:Label runat="server" ID="lblCilindraje_Contrato"></asp:Label>
                                    y con registro de matrícula con placa numero
                                    <asp:Label runat="server" ID="lblMatricula_Contrato"></asp:Label>
                                    con VIN numero
                                    <asp:Label runat="server" ID="lblVIN_Contrato"></asp:Label>
                                    con numero de motor
                                    <asp:Label runat="server" ID="lblNumeroMotor_Contrato"></asp:Label>
                                    y numero de chasís
                                    <asp:Label runat="server" ID="lblSerieChasis_Contrato"></asp:Label>
                                </b>
                            </p>
                            <p>
                                Mismo que será desembolsado por 
                                <b class="lblNombreComercial"></b>
                                a la persona que distribuya o sea propietario del vehículo y este ultimo deberá de entregárselo al Cliente cuando sea autorizado por
                                <b class="lblNombreComercial"></b>.
                                <b>- B) TASAS DE INTERES.-COSTO ANUAL TOTAL.- EL CLIENTE</b> se obliga a pagar a
                                <b class="lblNombreComercial"></b>,
                                a partir de esta fecha una tasa de interés simple del 
                                <b>
                                    <asp:Label runat="server" ID="lblTasaInteresSimpleMensual_Contrato">1.67</asp:Label>%
                                    PORCIENTO MENSUAL
                                </b>,
                                amartizando capital mas intereses basada en la regla del 78 misma que sera pagadero
                                <asp:Label runat="server" ID="lblTipoDePlazo_Contrato"></asp:Label>
                                en moneda de curso legal en Honduras, sobre el saldo total de la deuda.
                                <b>Por la falta de pago</b> a su vencimiento de cualquiera de los abonos a capital, intereses y/o recargos, <b>EL CLIENTE</b> pagará intereses moratorios
                                del <b>4.31% PORCIENTO MENSUAL</b> sobre el saldo de capital vencido, por razón de daños y perjuicios hasta que se cancele la totalidad de la mora, sin que deba considerarse prorrogado el plazo
                                <b>.-COSTO ANUAL TOTAL:</b> El costo anual total del préstamo (CAT) es de
                                <asp:Label runat="server" ID="lblCAT_Contrato"></asp:Label>%
                                incluye el cobro y pago del capital, los intereses ordinarios, y bajo la condición que <b>EL CLIENTE</b> cumpla con sus obligaciones en las formas y plazos detallados.
                            </p>
                            <p>
                                <b>C) FORMA Y PLAZO DEL FINANCIAMENTO:</b>
                                <b>
                                    <span runat="server" id="lblValorPrimaContrato" visible="false">Anticipo por concepto de prima de:
                                        <asp:Label runat="server" ID="lblMontoPrima_Contrato"></asp:Label>,
                                    </span>
                                    El plazo de financiamiento es de
                                    <asp:Label runat="server" ID="lblPlazo_Contrato"></asp:Label>
                                    <asp:Label runat="server" ID="lblFrecuenciaPago_Contrato"></asp:Label>,
                                    valor de la cuota:
                                    <asp:Label runat="server" ID="lblValorCuotaPalabras_Contrato"></asp:Label>
                                    (<asp:Label runat="server" ID="lblValorCuota_Contrato"></asp:Label>)
                                </b>
                                más
                                <asp:Label runat="server" ID="lblPlazoGPS_Contrato"></asp:Label>
                                cuotas de Servicio de monitoreo por GPS por un valor de
                                <asp:Label runat="server" ID="lblValorCuotaGPSPalabras_Contrato"></asp:Label>
                                (<asp:Label runat="server" ID="lblValorCuotaGPS_Contrato"></asp:Label>)
                                y
                                <asp:Label runat="server" ID="lblPlazoSeguro_Contrato"></asp:Label>
                                cuotas de
                                <asp:Label runat="server" ID="lblValorCuotaSeguroPalabras_Contrato"></asp:Label>
                                (<asp:Label runat="server" ID="lblValorCuotaSeguro_Contrato"></asp:Label>)
                                por concepto de seguro de vehiculo debiendo hacer efectivo el pago de la <b>primera cuota el
                                <asp:Label runat="server" ID="lblFechaPrimerCuota_Contrato"></asp:Label></b>
                                y así sucesivamente de forma
                                <asp:Label runat="server" ID="lblPlazoPago_Contrato"></asp:Label>
                                hasta la completa cancelación de la deuda en caso de cumplir el 100% del financiamiento sin abonar a capital y sin sumar gastos por cobranza y/o moratorios o cualquiera otro gasto generado por gestiones de recuperacion o incumplimento seria de
                                <asp:Label runat="server" ID="lblMontoTotalPrestamoPalabras_Contrato"></asp:Label>
                                (<asp:Label runat="server" ID="lblMontoTotalPrestamo_Contrato"></asp:Label>)
                            </p>
                            <p>
                                <b>D) DE LA FORMA, DE LA MONEDA Y LUGAR DE PAGO:</b>
                                Los contratantes acuerdan que:
                                <b>I)</b> Los abonos se harán primero a gastos, y cargos que pudieran haberse causado, luego los intereses, y el saldo, si lo hubiera, a capital;
                                <b>- II)</b> El pago del préstamo se hará en la moneda pactada y en efectivo.
                                <b>- III)</b> El pago se realizará conforme a lo establecido en el plan de pagos, en el caso que la fecha de pago sea día feriado, entonces deberá realizarse el día hábil inmediato anterior, en las oficinas, agencias, sucursales y ventanillas de
                                <b class="lblNombreComercial"></b>,
                                o en cualquier otra institución tercerizada que se designe oportunamente en virtud de convenios de cobro de cartera.
                                <b>- E) PAGO ANTICIPADO:</b>
                                En caso de pago total de la obligación antes de su vencimiento, <b>EL CLIENTE</b> deberá pagar una comisión de prepago del dos por ciento (2%) sobre el saldo adeudado, y si es un pago parcial a capital superior al diez por ciento (10%) del monto adeudado, también pagará dicha comisión calculada sobre el monto a pagar. Esta condición aplicara únicamente cuando el saldo del capital adeudado exceda cien mil dólares ($100,000.00) o su equivalente en lempiras, o los fondos sean provenientes de una institución que penalice a
                                <b class="lblNombreComercial"></b>
                                por pago anticipado, cualquiera de las dos o ambas conjuntamente.
                                <b>- F) PROPIEDAD DEL VEHICULO.</b>
                                Mientras no se haya cancelado la totalidad del Préstamo, será dueño del vehículo
                                <b class="lblNombreComercial"></b>,
                                el cliente será considerado como poseedor y esta posesión esta condicionada, es decir si el cliente está cumpliendo con las obligaciones contraídas en este contrato, de lo contrario
                                <b class="lblNombreComercial"></b>
                                podrá a su discreción retirarlo.
                            </p>
                            <p>
                                <b>- G) OTROS GASTOS:</b>
                                <b>I-</b> Los gastos que se incurra por matricula, mantenimiento, reparación y todas las relacionadas para conservación del vehículo en perfecto estado, deberá ser pagadas por el <b>CLIENTE</b>,
                                <b>II-</b> Los gastos que se ocasione en la recuperación del vehículo en caso de mora será de 3,000.00 Lempiras por concepto de grúa más lo generado por concepto de parqueo, Una vez recuperado el vehículo cuando se encuentre en mora,
                                <b class="lblNombreComercial"></b>
                                lo tendrá en custodia un máximo de 30 días para que el cliente se ponga al día y pague todo lo adeudo, pasado ese tiempo
                                <b class="lblNombreComercial"></b>
                                podrá vender el vehículo para poder recuperar el dinero invertido.
                                <b>III-</b> En caso que un gestor se movilice a traer el dinero del pago de una o más cuotas, bien sea al domicilio o lugar de trabajo, tendrá un costo de 250.00 Lempiras.
                                <span class="page-break"></span>
                                <b>- TERCERO: AUTORIZACIONES ESPECIALES: EL CLIENTE</b>
                                por este acto, en tanto no haya cumplido con el pago total de su obligación, autoriza a
                                <b class="lblNombreComercial"></b>
                                expresamente y sin ser necesario la notificación previa para:                                
                                <b>A)</b> Vender, Ceder o de cualquier otra forma traspasar, o celebrar contratos de participación, de descuentos con relación al crédito y derechos consignados en este documento o títulos valores relacionados a este mismo;
                                <b>B)</b> Autorizar a
                                <b class="lblNombreComercial"></b>
                                para que en cualquier tiempo pueda acceder a la información de la Central de Riesgos de la Comisión Nacional de Bancos y Seguros u otra central de riesgo pública o privada, para gestionar y conocer la situación crediticia de <b>EL CLIENTE</b> frente a las demás instituciones del sistema financiero nacional.
                                <b>- C) EL CLIENTE</b> Autoriza de manera Irrevocable, a que
                                <b class="lblNombreComercial"></b>
                                pueda entrar en su domicilio, para solo efecto de retirar el vehículo comprado con este préstamo, o que lo retire de una tercera persona sin necesidad de intervención judicial, esta cláusula solo se ejecutara en caso de mora de 15 días y mientras no haya sido cancelado el total adeudado.
                                <b>D- El cliente</b> Autoriza que
                                <b class="lblNombreComercial"></b>
                                pueda revisar el funcionamiento del GPS mientras no se haya cancelado la totalidad del prestamo.
                                
                                <span runat="server" id="lblRevisionGpsAutos" visible="false">

                                    <b>EL CLIENTE</b> se obliga a venir a las instalaciones de 
                                    <b class="lblNombreComercial"></b>
                                    a revisión y mantenimiento de su GPS en cualquier momento que 
                                    <b class="lblNombreComercial"></b>
                                    lo requiera.

                                </span>

                                <b>- CUARTO: OBLIGACIONES GENERALES.- EL CLIENTE</b> durante la vigencia del presente contrato también se obliga a:
                                <b>A)</b> Permitir que
                                <b class="lblNombreComercial"></b>
                                ejerza los controles que juzgue convenientes, para asegurarse que los fondos de este crédito se inviertan en los fines que se han indicado anteriormente y condiciones que se estipulan en este contrato.
                                <b>- B) DE LA GARANTIA:</b>
                                En calidad de Garantía para el Cumplimiento de la presente obligación <b>El CLIENTE</b> firmara un <b>PAGARE</b> sin protesto, así como también da en propiedad a
                                <b class="lblNombreComercial"></b>
                                el vehículo comprado con el dinero objeto del presente préstamo, quedándose
                                <b class="lblNombreComercial"></b>
                                con la documentación original del vehículo y el <b>CLIENTE</b> en posesión del vehículo del cual será responsable mientras se encuentre en su poder y no se haya cancelado el precio total pactado para la terminación del presente contrato.
                                Sin perjuicio de la <b>designación de garantías fiduciarias como ser Menaje de Hogar y demás bienes pertenecientes AL CLIENTE</b>, por lo que está <b>terminantemente prohibido para el CLIENTE utilizar el vehículo para transporte público como ser taxi en todas sus modalidades incluyendo VIP,
                                asi como también transporte de carga o similares</b>, también se le prohibe enajenar, vender, permutar, donar, gravar, prestar o dar en prenda el vehículo dado en propiedad, sin la autorización por escrito otorgada por
                                <b class="lblNombreComercial"></b>,
                                el incumplimiento de las prohibiciones faculta a
                                <b class="lblNombreComercial"></b>
                                a retirar el vehículo.
                                Para el menaje se formalizará el Inventario de estos, este que pasara a formar parte del presente contrato.
                                <b>C)</b> Suscribir y a mantener un seguro para vehículos en lempiras moneda de curso legal en Honduras; mientras esté vigente la deuda, por la cuantía y condiciones que señale
                                <b class="lblNombreComercial"></b>,
                                con una compañía aseguradora; siendo entendido que <b>EL CLIENTE</b> deberá endosar a favor de
                                <b class="lblNombreComercial"></b>
                                la respectiva póliza de seguro, o a favor de la persona natural o jurídica a cuyo nombre se traspase el presente crédito, hasta la total cancelación del saldo pendiente de pago por la deuda.
                                <b class="lblNombreComercial"></b>
                                podrá pagar y cargar al préstamo las primas de seguro, si <b>EL CLIENTE</b> no lo renueva y paga a los treinta (30) días previos al vencimiento de la póliza de seguro respectiva, sin que la acción del pago o cargo sea obligatorio para
                                <b class="lblNombreComercial"></b>, quien no asumirá ni incurrirá en responsabilidad por no hacer el pago de las primas de seguro.
                                <b>D)</b> Mantenerse al día en el pago de los impuestos que graven a <b>EL CLIENTE</b> o al <b>VEHÍCULO</b> dado en garantía.
                                <b>- E)</b> Cuidar como buen padre de familia el vehículo dado en garantía, mientras se encuentre en su poder y no se haya cancelado el precio total pactado para la terminación del presente contrato, quedando a su cargo los riesgos de dicho bien mueble por lo que será responsable de la perdida, destrucción o deterioro que sufra aun por caso fortuito o fuerza mayor.
                                <b>- F)</b> Mantener la licencia de conducir vigente, mientras no se haya cancelado la totalidad del préstamo, en caso que <b>EL CLIENTE</b> haya solicitado excepción al momento de otorgarse el Préstamo por no poseer licencia vigente, entonces dispondrá solamente de un máximo de 30 días para presentar la Licencia de conducir a
                                <b class="lblNombreComercial"></b>,
                                caso contrario <b>AUTORIZA</b> anticipadamente a
                                <b class="lblNombreComercial"></b>
                                a que se le retire, en calidad de custodia, el vehiculo hasta que presente la licencia de conducir aun y cuando sus cuotas estén al día.
                                La excepción anterior <b>no faculta al CLIENTE</b> a conducir el vehiculo sin su respectiva Licencia emitida por la Dirección Nacional de Transito, ni a prestar a quien no tenga dicho documento, ya que
                                <b class="lblNombreComercial"></b>
                                respeta las leyes hondureñas.
                                <b>- QUINTO: DE LOS DEBERES DEL CLIENTE: </b>
                                Se conviene que, desde la fecha de otorgamiento de este contrato, hasta la fecha en que se pague el total de las obligaciones pendientes con
                                <b><span class="lblNombreComercial"></span>, EL CLIENTE</b> deberá informar siempre, por vía telefónica o escrita y a la brevedad posible, las siguientes acciones:
                                <b>1)</b> Contraiga deudas con otras instituciones financieras, no financieras, puestos de bolsa, proveedores, filiales y otros. <b>EL CLIENTE</b> aprueba libre y voluntariamente por ser válidas, todas las condiciones fijadas en este inciso, por entender que de tal manera
                                <b class="lblNombreComercial"></b>
                                se asegura de la solvencia de <b>EL CLIENTE</b> y del pago del crédito otorgado.
                                <b>- SEXTO: DE LAS MODIFICACIONES DEL CONTRATO.-
                                    <span class="lblNombreComercial"></span>
                                </b>
                                comunicará a
                                <b>EL CLIENTE: 1)</b> De manera general y sin necesidad de especificarlo individualmente, las condiciones contractuales pactadas, por cualquier medio impreso de circulación nacional, o el medio de comunicación que las partes hayan designado, en los casos de los efectos de la aplicación de la vigencia de una ley, con 30 días calendario de anticipación a la aplicación de dicho cambio;
                                <b>2)</b> Para el caso que las tasas de intereses y otros cargos sea modificada, se aplicará conforme a un factor variable que considera la tasa de interés que se concede para los depósitos a plazo, más el costo de la intermediación y sumándole un diferencial del veinte por ciento. La tasa de interés se revisará cada 3 meses. En el caso que las tasas de interés sean reguladas por el Banco Central de Honduras conforme al artículo 55 de la Ley del Sistema Financiero, se aplicará la tasa máxima permitida por dicha Institución, o la que fije y aplique
                                <b class="lblNombreComercial"></b>,
                                notificándolo a <b>EL CLIENTE</b> con 15 días calendario de anticipación por lo menos por cualquiera de los medios de comunicación descritos en este contrato o los otros establecidos por la Ley, siendo entendido que cualquier ajuste resultante de la modificación de la tasa de interés será cubierto por <b>EL CLIENTE</b> quedando
                                <b class="lblNombreComercial"></b>
                                autorizado para efectuar y cobrar tales ajustes y modificar la cuota quincenal o mensual del financiamiento de acuerdo al plazo que reste para la cancelación del mismo, así mismo las partes acuerdan incorporar como vinculante el principio <b>“ceteris paribus”</b>, respecto a modificaciones atinentes al contrato o los convenios incorporados.
                                <b>-SEPTIMO: RECLAMOS.-</b>Cuando se presente algún evento por el cual <b>EL CLIENTE</b> desee hacer un reclamo, se dispondrá de un plazo de 10 días hábiles para realizarlo, transcurrido éste, es entendido que caduca su derecho para reclamar y se declara vencido. Cuando sea reclamos por cuestiones de garantía deberá presentarlas al distribuidor autorizado y en caso de ser bienes usados no podrá presentar reclamos después de 30 dias de realizada la compra, es entendido que
                                <b class="lblNombreComercial"></b>
                                no está obligado a resolver cuestiones de garantía puesto que solo es quien financia la compra.
                                <b>- OCTAVO: DEL VENCIMIENTO ANTICIPADO DEL PLAZO DE PAGO.- </b>Además de los casos establecidos por la ley,
                                <b class="lblNombreComercial"></b>
                                podrá dar por vencido el plazo establecido para el pago del préstamo concedido en este contrato, y en consecuencia exigir el pago inmediato del saldo del capital, intereses, comisiones, recargos y gastos, ya sea por la vía judicial o extra judicial, por cualquiera de los siguientes eventos:
                                <b>a)</b> Por falta de pago de una o más de las cuotas pactadas, de los intereses, o de cualquier otro cargo pendiente a favor de
                                <b class="lblNombreComercial"></b>;
                                <b>b)</b> Por el conocimiento de la ejecución judicial iniciada por terceros, o por el mismo
                                <b class="lblNombreComercial"></b>,
                                en contra de <b>EL CLIENTE</b>, originada por otros créditos;
                                <span class="page-break"></span>
                                <b>c)</b> Por no destinar el presente préstamo para el fin o fines para los cuales ha sido concedido;
                                <b>d)</b> Por la declaración del estado de suspensión de pagos, de quiebra o de concurso de <b>EL CLIENTE</b>, así como por su inhabilitación para el ejercicio del comercio, o por el ejercicio de acción penal en su contra o de su representante legal que derivare en sentencia de privación de libertad;                                
                                <b>e)</b> Por el incumplimiento o negativa por parte de <b>EL CLIENTE</b> a proporcionar la información requerida por
                                <b class="lblNombreComercial"></b>
                                en forma escrita;                                
                                <b>f)</b> Por actuación fraudulenta o haber proporcionado a
                                <b class="lblNombreComercial"></b>
                                información o datos falsos o incompletos para obtener el préstamo;
                                <b>g)</b> Por ser del conocimiento de
                                <b class="lblNombreComercial"></b>,
                                la existencia de obligaciones de <b>EL CLIENTE</b> pendientes de pago con el Estado, en tal cantidad que a su criterio ponga en peligro la recuperación de los adeudos debido a la preferencia del Estado para obtener el pago a su favor antes que
                                <b class="lblNombreComercial"></b>;
                                <b>h)</b> El incumplimiento de parte de <b>EL CLIENTE</b> de cualquiera de las obligaciones contraídas en este contrato.
                                <b>i)</b> Por retirar, desconectar, manipular, adulterar, o de cualquier forma hacer que el GPS del Vehiculo no funcione correctamente.
                                <b>- NOVENO: COBROS EXTRAJUDICIALES.-</b> En caso de ser necesarias las gestiones de cobranzas extrajudiciales por la mora en el pago o el vencimiento anticipado del contrato, estas se realizarán de la siguiente manera:
                                <b>1)</b> Para Mora de 1 a 180 días: alternativamente podrán ser llamadas telefónicas, correos electrónicos, mensajes por cualquier medio electrónico, visitas por gestores, cartas de cobro escritas solicitando el pago y dirigidas a las direcciones indicadas. Estas gestiones tendrán un costo de doscientos cincuenta lempiras (L250.00), cargados al estado de cuenta del préstamo otorgado, son acumulables por cada cuota vencida y serán pagados por <b>EL CLIENTE</b> en todos los casos y sin excepción;
                                <b>2)</b> Si su caso fuere trasladado a Profesionales del Derecho, cuyas gestiones iniciales podrán ser: llamadas telefónicas, envió de correos electrónicos, cartas de cobro escritas, y visitas, causaran el cobro de honorarios
                                profesionales según el Arancel del Profesional del Derecho vigente, y se calculará sobre el capital, intereses, recargos, cargos y seguros en mora, tal como lo establece el artículo 1432 del Código Civil.- En caso de ser perseguida la deuda por proceso Judicial, se cargaran igualmente los gastos ocasionados por costas durante dicho proceso.
                                <b class="lblNombreComercial"></b>
                                podrá asignar a una empresa o Agencia de Cobranzas y/o Recuperaciones para que realice estas labores de cobro desde el día uno de atraso en el estado de cuenta lo cual es aceptado por el deudor.
                                <b>- DÉCIMO: ACCIONES JUDICIALES.- </b>
                                En caso de mora o vencimiento anticipado del contrato, dará lugar para que
                                <b class="lblNombreComercial"></b>
                                ejerza las acciones judiciales correspondientes, quedando obligado el cliente a pago de gastos y honorarios que ocasione el procedimiento judicial. Así como para determinar el saldo adeudado El estado de cuenta certificado por el contador de
                                <b class="lblNombreComercial"></b>
                                o de quien haya adquirido los derechos, hará fe en juicio para establecer el saldo a cargo de <b>EL CLIENTE</b> y Constituirá junto con el presente contrato título ejecutivo, sin necesidad de reconocimiento de firma ni de otro requisito previo alguno, según lo establecido en la ley del sistema financiero. En caso de ejecución de la presente obligación las partes nos sometemos a la jurisdicción y competencia de los Juzgados de San Pedro Sula, Cortés.
                                <b>.- DÉCIMO PRIMERA: MEDIOS PARA COMUNICACIONES.- EL CLIENTE y
                                    <span class="lblNombreComercial"></span>
                                </b>
                                establecen y a la vez autorizan, que para las distintas notificaciones que se deban hacer conforme a lo estipulado por este contrato o por lo dispuesto por la ley, se harán efectivas a través de uno solo de los siguientes medios:
                                <b>A)</b> Correspondencia ordinaria escrita dirigida a las direcciones indicadas en el preámbulo de este contrato;
                                <b>B)</b> Notificación por la vía electrónica a su correo electrónico
                                <asp:Label runat="server" ID="lblCorreo_Contrato"></asp:Label>
                                <b>C)</b> Notificación mediante cualquier red sociales que pudiese pertenecer al Cliente,
                                <b>D)</b> o a las direcciones indicadas en cualquiera de los documentos suscritos con
                                <b>“ <span class="lblNombreComercial"></span>”</b>.
                                Cualquier cambio de dirección o número telefónico deberá notificarse fehacientemente, con una anticipación razonable a
                                <b class="lblNombreComercial"></b>
                                y hasta entonces se considera efectiva.
                                <b>.-DÉCIMO SEGUNDA: DE LAS AUTORIZACIONES ESPECIALES. EL CLIENTE </b>otorga de manera expresa, voluntaria e irrevocable su consentimiento para que en caso de mora,
                                <b class="lblNombreComercial"></b>
                                o sus representantes puedan ingresar a su domicilio a retirar el vehículo, y por lo tanto lo exime de toda responsabilidad que pueda incurrir según el artículo 99 de la Constitución de la Republica.
                                Así como faculta a 
                                <b>“ <span class="lblNombreComercial"></span>”</b>,
                                sus distintas dependencias, así como también a su personal, que mediante visitas a su domicilio se le puedan presentar y ofrecer las diferentes propuestas de negocio, servicios, catálogos de nuevos productos; a su vez, faculta otros canales, sean estos telefónicos o electrónicos, a que se comuniquen y a que le informen en los días de semana, así como también en los días llamados vacaciones, o festivos, en los diferentes horarios abiertos, incluso fin de semana, exonerándole de cualquier perjuicio a la empresa o de ser estas visitas catalogadas como “hostigamiento”.
                                <b>- El CLIENTE:</b> autoriza de manera expresa y voluntaria que en caso de que
                                <b class="lblNombreComercial"></b>
                                retire el vehículo, pueda ser subastado al mejor postor cuando el PRESTAMO presente 60 dias de mora y el dinero recibido de la misma se abonara a la deuda, si existiera un excedente se le dará al <b>CLIENTE</b> y en caso que no cubriese el total adeudado,
                                <b class="lblNombreComercial"></b>
                                se reserva el derecho de ejercer acciones legales contra el <b>CLIENTE</b> por el pago de saldo total adeudado, que incluye capital, intereses, otros cargos o gastos que incurra por recuperacion, reparacion, impuestos u otros.
                                <b>- DÉCIMO TERCERA: PROHIBICIONES PARA EL CLIENTE:</b> Mientras este contrato no haya sido cancelado en su totalidad es se le Prohibe realizar las siguientes acciones:
                                <b>A) utilizar el vehiculo para transporte publico como ser taxi en todas sus modalidades incluyendo VIP</b>,
                                <b>B) utilzar el vehiculo como transporte de carga comercial</b>.
                                <b>-C)</b> Adulterar el GPS o mandarlo a retirar o negarse
                                <span runat="server" id="lblNegarseRevisionGpsAuto" visible="false">a la revisión o al mantenimiento programado por
                                    <b class="lblNombreComercial"></b>
                                </span>.
                                <b>D)</b> Enajenar, vender, permutar, donar, gravar, alquilar, rentar, prestar o dar en prenda el vehículo dado en propiedad, sin la autorización por escrito otorgada por
                                <b class="lblNombreComercial"></b>,
                                la inobservacias de las prohibiciones faculta a
                                <b class="lblNombreComercial"></b>
                                a dar por vencido el plazo y podra retirar el vehículo, sin intervencion JudiciaL o institucion reguladora.
                                <b>- DÉCIMO CUARTO: COMPROBACION DE HABER RECIBIDO INSTRUCCIÓN Y ORIENTACION DEL PRODUCTO Y ENTREGA DE COPIA DEL CONVENIO Y PLAN DE PAGO.- EL CLIENTE</b> por este acto acepta que previo a la celebración de este contrato, ha recibido toda la orientación y explicación necesaria sobre las condiciones del convenio, las consecuencias legales y judiciales de su incumplimiento, así como que ha recibido una copia íntegra de este documento y del plan de pagos respectivo.
                                .- Finalmente las partes declaramos que es cierto todo lo anteriormente expresado, y que por ser ello lo convenido, aceptamos libre y voluntariamente, todas estipulaciones, condiciones y cláusulas contenidas en el presente contrato de préstamo.
                                En fe de lo cual firmamos en la ciudad de
                                <span class="lblCiudad_Firma"></span>,
                                a los
                                <span class="lblNumeroDia_Firma"></span>
                                días del mes de
                                <span class="lblMes_Firma"></span>
                                del año
                                <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <div class="row mt-2">
                        <div class="col-1"></div>
                        <div class="col-5 text-center">
                            <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <asp:Label class="mt-0 d-block" runat="server" ID="lblNombreFirma_Contrato"></asp:Label>
                            <asp:Label class="mt-0 d-block" runat="server" ID="lblIdentidadFirma_Contrato"></asp:Label>
                            <label class="mt-0 d-block">EL CLIENTE</label>
                        </div>
                        <div class="col-5 text-center">
                            <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block lblNombreRepresentanteLegal"></label>
                            <label class="mt-0 d-block lblIdentidadRepresentanteLegal"></label>
                            <label class="mt-0 d-block lblNombreComercial"></label>
                        </div>
                        <div class="col-1"></div>
                    </div>
                </div>
            </div>
        </div>


        <!-- ================================== HTML Pagaré ===================================== -->
        <div id="divContenedorPagare" class="contenedorPDF">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divPagarePDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">PAGARÉ POR
                                <asp:Label runat="server" ID="lblMontoTitulo_Pagare"></asp:Label>
                            </h5>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                <b>YO,
                                    <asp:Label runat="server" ID="lblNombre_Pagare"></asp:Label>, 
                                </b>
                                mayor de edad,
                                <asp:Label runat="server" ID="lblEstadoCivil_Pagare"></asp:Label>,
                                con nacionalidad
                                <asp:Label runat="server" ID="lblNacionalidad_Pagare"></asp:Label>,
                                de profesión
                                <asp:Label runat="server" ID="lblProfesion_Pagare"></asp:Label>,
                                con tarjeta de identidad
                                <asp:Label runat="server" ID="lblIdentidad_Pagare"></asp:Label>
                                y con domicilio en
                                <asp:Label runat="server" ID="lblDireccion_Pagare"></asp:Label>,
                                actuando en condición personal, acepto que <b>DEBO y PAGARÉ</b> incondicionalmente <b>SIN PROTESTO,</b> y a la orden de
                                <b class="lblRazonSocial"></b>,
                                la cantidad de
                                <asp:Label runat="server" ID="lblMontoPalabras_Pagare"></asp:Label>
                                (<asp:Label runat="server" ID="lblMontoDigitos_Pagare"></asp:Label>).
                                Dicha cantidad será pagada el día
                                <asp:Label runat="server" ID="lblDiaPrimerPago_Pagare"></asp:Label>
                                del mes de
                                <asp:Label runat="server" ID="lblMesPrimerPago_Pagare"></asp:Label>
                                del año
                                <asp:Label runat="server" ID="lblAnioPrimerPago_Pagare"></asp:Label>,
                                en las oficinas, agencias, sucursales y ventanillas de
                                <b class="lblRazonSocial"></b>.
                                La cantidad consignada en este PAGARE devengará, a partir de esta fecha, una tasa de interés fluctuante del
                                <asp:Label runat="server" ID="lblPorcentajeInteresFluctuante_Pagare"></asp:Label>%
                                PORCIENTO MENSUAL,
                                sobre el saldo total de la deuda, a pagar mensualmente. En caso de mora, que se producirá por la falta de pago al vencimiento tanto del capital o de los intereses,
                                dará derecho a
                                <b class="lblRazonSocial"></b>
                                a exigir el pago de intereses moratorios del
                                <asp:Label runat="server" ID="lblInteresesMoratorios_Pagare"></asp:Label>%
                                PORCIENTO MENSUAL;
                                a su vez, en caso de ejecución legal de la presente obligación, me someto a la jurisdicción que establezca
                                <b class="lblRazonSocial"></b>,
                                quedando incorporadas en este documento todas las disposiciones del Código de Comercio.
                                En fe de lo cual, firmo (amos) en la ciudad de
                                <span class="lblCiudad_Firma"></span>,
                                departamento de
                                <span class="lblDepartamento_Firma"></span>,
                                a los
                                <span class="lblNumeroDia_Firma"></span>
                                días del mes de
                                <span class="lblMes_Firma"></span>
                                del año <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <!-- Firma del cliente -->
                    <div class="row">
                        <div class="col-3"></div>
                        <div class="col-6 text-center p-0 mt-3">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Firma del cliente</label>
                            <asp:Label class="mt-0 d-block" runat="server" ID="lblNombreFirma_Pagare"></asp:Label>
                            <asp:Label class="mt-0 d-block" runat="server" ID="lblIdentidadFirma_Pagare"></asp:Label>
                        </div>
                        <div class="col-3"></div>
                    </div>
                </div>
            </div>
        </div>


        <!-- ================================== HTML Compromiso Legal =========================== -->
        <div id="divContenedorCompromisoLegal" class="contenedorPDF">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divCompromisoLegalPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">COMPROMISO LEGAL</h5>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                <b>YO,
                                    <asp:Label runat="server" ID="lblNombreCliente_CompromisoLegal"></asp:Label>,
                                </b>
                                acepto haber adquirido un préstamo en efectivo con la empresa
                                <b class="lblRazonSocial"></b>,
                                financiamiento otorgado a
                                <asp:Label runat="server" ID="lblCantidadCuotas_CompromisoLegal"></asp:Label>
                                cuotas de
                                <asp:Label runat="server" ID="lblValorCuotaPalabras_CompromisoLegal"></asp:Label>
                                (<asp:Label runat="server" ID="lblValorCuota_CompromisoLegal"></asp:Label>)
                                para la compra de contado de un Vehículo Automotor,
                                el cual queda como garantía prendaria del financiamiento otorgado.
                                Por lo que, durante el plazo del financiamiento del vehículo automotor,
                                soy el único responsable por todo acto de carácter legal o ilegal que se encuentre involucrado dicho automotor,
                                liberando de cualquier responsabilidad a la empresa antes mencionada.
                            </p>
                            <p>
                                Así mismo, entiendo que:
                            </p>
                            <p>
                                La garantía de dicho vehículo corresponde exclusivamente al distribuidor o concesionario donde fue adquirido; por lo tanto,
                                <b class="lblRazonSocial"></b>
                                no se hace responsable de la garantía, la cual funciona de acuerdo a políticas y restricciones del distribuidor y/o vendedor.
                            </p>
                            <p>
                                Me doy por enterado que el vehículo es
                                <asp:Label runat="server" ID="lblGarantiaUsada_CompromisoLegal"></asp:Label>
                                y está en buen estado y se entrega tal cual esta,
                                y hago constar que lo vi, revisé y lo probé antes de comprarlo y es por eso que estoy satisfecho con dicha revisión.
                                <br />
                                El seguro de daños que incluye el préstamo es válido única y exclusivamente
                                durante el tiempo del financiamiento, siempre y cuando las cuotas estén al día.
                                <br />
                                Doy fe de lo anterior y de recibir el vehículo y para esto firmo de forma libre y espontáneamente la presente constancia
                                en la ciudad de
                                <span class="lblCiudad_Firma"></span>,
                                departamento de
                                <span class="lblDepartamento_Firma"></span>,
                                a los
                                <span class="lblNumeroDia_Firma"></span>
                                días del mes de <span class="lblMes_Firma"></span>
                                del año <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <!-- Firma del cliente -->
                    <div class="row justify-content-center">
                        <div class="col-5 text-center">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Nombre</label>
                        </div>
                        <div class="col-5 text-center">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Firma</label>
                        </div>
                        <div class="col-5 text-center">
                            <asp:Label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" ForeColor="Black" Style="border-radius: 0px;" runat="server" ID="lblIdentidadFirma_CompromisoLegal"></asp:Label>
                            <label class="mt-0 d-block">Identidad</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- ======== HTML Convenio de regulación de compra y venta de vehiculos para financiamiento a tercero =========== -->
        <div id="divContenedorConvenioCyV" class="contenedorPDF">
            <div class="card m-0 pt-4 divImprimir" runat="server" visible="true" id="divConevionCyVPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="//:0" class="img-fluid mt-0 pt-0 img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h6 class="text-center font-weight-bold">Convenio de regulación de compra y venta de vehículos para financiamiento a Tercero</h6>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                Nosotros
                                <b class="lblNombreRepresentanteLegal"></b>,
                                mayor de edad,
                                <span class="lblEstadoCivilRepresentanteLegal"></span>,
                                <span class="lblNacionalidadRepresentanteLegal"></span>,
                                <span class="lblProfesionRepresentanteLegal"></span>
                                y de este domicilio, quien actúa en condición de representante
                                Legal de la Empresa Mercantil denominada 
                                <b class="lblRazonSocial"></b>
                                <span class="lblConstitucionFondo"></span>
                                que en este convenio se denominará <b>LA EMPRESA</b> y <b>AL SEÑOR(A)</b>
                                <asp:Label runat="server" ID="lblNombreCliente_ConvenioCyV"></asp:Label>
                                mayor de edad,
                                <asp:Label runat="server" ID="lblNacionalidad_ConvenioCyV"></asp:Label>,
                                <asp:Label runat="server" ID="lblEstadoCivil_ConvenioCyV"></asp:Label>,
                                con tarjeta de identidad número
                                <asp:Label runat="server" ID="lblIdentidad_ConvenioCyV"></asp:Label>
                                y con domicilio en la ciudad de
                                <asp:Label runat="server" ID="lblCiudadCliente_ConvenioCyV"></asp:Label>,
                                quien actúa en calidad de propietario de un VEHICULO AUTOMOTOR
                                MARCA:
                                <asp:Label runat="server" ID="lblMarca_ConvenioCyV"></asp:Label>
                                MODELO:
                                <asp:Label runat="server" ID="lblModelo_ConvenioCyV"></asp:Label>
                                TIPO:
                                <asp:Label runat="server" ID="lblTipoVehiculoConvenioCyV"></asp:Label>
                                AÑO:
                                <asp:Label runat="server" ID="lblAnio_ConvenioCyV"></asp:Label>
                                COLOR:
                                <asp:Label runat="server" ID="lblColor_ConvenioCyV"></asp:Label>
                                CILINDRAJE:
                                <asp:Label runat="server" ID="lblCilindraje_ConvenioCyV"></asp:Label>
                                Y CON REGISTRO DE MATRICULA PLACA:
                                <asp:Label runat="server" ID="lblMatricula_ConvenioCyV"></asp:Label>
                                VIN:
                                <asp:Label runat="server" ID="lblVIN_ConvenioCyV"></asp:Label>
                                CHASIS:
                                <asp:Label runat="server" ID="lblSerieChasis_ConvenioCyV"></asp:Label>
                                MOTOR:
                                <asp:Label runat="server" ID="lblSerieMotor_ConvenioCyV"></asp:Label>,

                                Ambos cuentan con la capacidad y facultades suficientes para poder contratar, quienes voluntariamente con plena libertad y
                                sin presión de ninguna naturaleza manifestamos lo siguiente:
                            </p>
                            <p class="mb-2">
                                <b>Primero:</b>
                                A la firma de este documento las partes reconocen que tienen relación contractual, la cual servirá de guía al momento en que la Empresa decida otorgar crédito a terceras personas y El cliente posea el vehículo a financiar.
                            </p>
                            <p class="mb-2">
                                <b>Segundo:</b>
                                El Cliente Jura que los recursos que utiliza para la compra de vehículos provienen de Actividades Licitas.
                            </p>
                            <p class="mb-2">
                                <b>Tercero:</b>
                                La Empresa se obliga a pagar con cheques a favor de El Cliente y se hará responsable de los daños y perjuicios ocasionados en el caso que no posea fondos suficientes.
                            </p>
                            <p class="mb-2">
                                <b>Cuarto:</b>
                                El Cliente declara que los vehículos no están denunciados por haber participado en accidentes de tránsitos o en cualquier actividad ilícita.
                            </p>
                            <p class="mb-2">
                                <b>Quinto:</b>
                                La Empresa manifiesta que los recursos utilizados para la compra de los vehículos provienen de actividades licitas.
                            </p>
                            <p class="mb-2">
                                <b>Sexto:</b>
                                El cliente se obliga a entregar el vehículo a la persona que la Empresa designe.
                            </p>
                            <p class="mb-2">
                                <b>Séptimo:</b>
                                El cliente garantiza que los vehículos están en perfecto estado tanto en condiciones físicas como mecánicas para circular y reúne todas las medidas de seguridad solicitadas por las Leyes Vigentes en el País.
                            </p>
                            <p class="mb-2">
                                <b>Octavo:</b>
                                El Cliente se compromete a traspasar el vehículo a favor de la Persona que La Empresa designe.
                            </p>
                            <p class="mb-2">
                                <b>Noveno:</b>
                                El Cliente se compromete a entregar todos los documentos del vehículo a La Empresa.
                            </p>
                            <p class="mb-2">
                                <b>Décimo:</b>
                                El Cliente se obliga a indemnizar en concepto de daños y perjuicios ocasionados a La Empresa en las Clausulas anteriores.
                            </p>
                            <p class="mb-2">
                                <b>Décimo primero:</b>
                                En caso de controversia se someten al centro de Conciliación y Arbitraje de la cámara de comercio e Industrias de Cortes.
                                <br />
                                El presente convenio se firma en duplicado en la ciudad de
                                <span class="lblCiudad_Firma"></span>,
                                <span class="lblDepartamento_Firma"></span>
                                a los
                                <span class="lblNumeroDia_Firma"></span>
                                días del mes de
                                <span class="lblMes_Firma"></span>
                                del año
                                <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <!-- Firma del cliente -->
                    <div class="row justify-content-center mt-1">
                        <div class="col-5 text-center mt-1">
                            <label class="mt-3 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Por la empresa <span class="lblRazonSocial"></span></label>
                            <label class="mt-0 d-block"><span class="lblNombreRepresentanteLegal"></span></label>
                        </div>
                        <div class="col-5 text-center mt-1">
                            <label class="mt-3 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Por el cliente</label>
                            <asp:Label runat="server" ID="lblNombre_ConvenioCyV" class="mt-0 d-block"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- =========================== HTML Inspeccion del vehiculo =========================== -->
        <div id="divContenedorInspeccionSeguro" runat="server" visible="false">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divInspeccionSeguroPDF">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">INSPECCIÓN SEGURO DE VEHÍCULO</h5>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <table class="table table-bordered border-dark" style="border-width: 1px;">
                                <thead>
                                    <tr>
                                        <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Asegurado</th>
                                        <th colspan="5" class="p-0">
                                            <asp:Label runat="server" ID="lblNombre_InspeccionSeguro"></asp:Label>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Marca</th>
                                        <th class="p-0">
                                            <asp:Label runat="server" ID="lblMarca_InspeccionSeguro"></asp:Label>
                                        </th>
                                        <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Modelo</th>
                                        <th class="p-0">
                                            <asp:Label runat="server" ID="lblModelo_InspeccionSeguro"></asp:Label>
                                        </th>
                                        <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Año</th>
                                        <th class="p-0">
                                            <asp:Label runat="server" ID="lblAnio_InspeccionSeguro"></asp:Label>
                                        </th>
                                    </tr>
                                    <tr>
                                        <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Tipo</th>
                                        <th class="p-0">
                                            <asp:Label runat="server" ID="lblTipoDeVehiculo_InspeccionSeguro"></asp:Label>
                                        </th>
                                        <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Kilometraje</th>
                                        <th class="p-0">
                                            <asp:Label runat="server" ID="lblRecorrido_InspeccionSeguro"></asp:Label>
                                        </th>
                                        <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Placa</th>
                                        <th class="p-0">
                                            <asp:Label runat="server" ID="lblMatricula_InspeccionSeguro"></asp:Label>
                                        </th>
                                    </tr>
                                </thead>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 pl-0">
                            <div style="max-width: 794px !important; min-width: 794px !important; overflow-x: hidden;">
                                <div id="divGaleriaInspeccionSeguroDeVehiculo" style="width: 100% !important; max-width: 100% !important; overflow-x: hidden;" runat="server"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- =========================== HTML Recibo ============================================ -->
        <div id="divContenedorRecibo" runat="server" class="contenedorPDF">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divReciboPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 mb-5">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-6 align-self-center">
                            <h4 class="text-center font-weight-bold lblRazonSocial"></h4>
                        </div>
                        <div class="col-6">
                            <table class="table table-bordered">
                                <tbody>
                                    <tr>
                                        <th class="bg-light font-weight-bold text-center pb-0">RTN</th>
                                        <td class="p-0 text-center font-weight-bold">
                                            <span class="lblRTNEmpresa"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="bg-light font-weight-bold text-center pb-0">Fecha</th>
                                        <td class="p-0 text-center">
                                            <asp:Label runat="server" ID="lblFecha_Recibo"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="bg-light font-weight-bold text-center pb-0">Recibo</th>
                                        <td class="p-0 text-center">
                                            <asp:Label runat="server" ID="lblRecibo_Recibo"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="row mb-5">
                        <div class="col-12">
                            <table class="table table-bordered mb-0">
                                <tbody>
                                    <tr>
                                        <th class="bg-light font-weight-bold p-1" style="width: 15%">Recibí de:</th>
                                        <td class="p-1 font-weight-bold">
                                            <span class="lblRazonSocial"></span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="bg-light font-weight-bold p-1" style="width: 15%">La suma de:</th>
                                        <td class="p-1 font-weight-bold">
                                            <asp:Label runat="server" ID="lblSumaRecibidaEnPalabras_Recibo"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <th class="bg-light font-weight-bold p-1" style="width: 15%">Pago por:</th>
                                        <td class="p-1 font-weight-bold">FINANCIAMIENTO DE VEHICULO AUTOMOTOR MARCA:
                                        <asp:Label runat="server" ID="lblMarca_Recibo"></asp:Label>
                                            MODELO:
                                        <asp:Label runat="server" ID="lblModelo_Recibo"></asp:Label>
                                            AÑO:
                                        <asp:Label runat="server" ID="lblAnio_Recibo"></asp:Label>
                                            COLOR:
                                        <asp:Label runat="server" ID="lblColor_Recibo"></asp:Label>
                                            TIPO:
                                        <asp:Label runat="server" ID="lblTipo_Recibo"></asp:Label>
                                            CILINDRAJE:
                                        <asp:Label runat="server" ID="lblCilindraje_Recibo"></asp:Label>
                                            Y CON REGISTRO DE MATRICULA MOTOR:
                                        <asp:Label runat="server" ID="lblSerieMotor_Recibo"></asp:Label>
                                            VIN:
                                        <asp:Label runat="server" ID="lblVIN_Recibo"></asp:Label>
                                            CHASIS:
                                        <asp:Label runat="server" ID="lblSerieChasis_Recibo"></asp:Label>
                                            PLACA:
                                        <asp:Label runat="server" ID="lblPlaca_Recibo"></asp:Label>
                                            CLIENTE:
                                        <asp:Label runat="server" ID="lblNombreCliente_Recibo"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <table class="table table-bordered mb-5">
                                <tbody>
                                    <tr>
                                        <th class="font-weight-bold p-1 text-right" style="width: 50%">TOTAL RECIBIDO</th>
                                        <td class="p-1 font-weight-bold text-right" style="width: 50%">
                                            <asp:Label runat="server" ID="lblTotalRecibido_Recibo"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="row justify-content-center pt-5">
                        <div class="col-6">
                            <label class="text-center font-weight-bold border-top border-bottom-0 border-right-0 border-left-0 form-control border-dark pt-2">NOMBRE Y FIRMA DE RECIBIDO</label>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-6">
                            <div class="row">
                                <div class="col-3">
                                    <label>RECIBE</label>
                                </div>
                                <div class="col-9">
                                    <asp:Label runat="server" CssClass="font-weight-bold" ID="lblNombreVendedor_Recibo"></asp:Label>
                                </div>
                                <div class="col-3">
                                    <label>IDENTIDAD</label>
                                </div>
                                <div class="col-9">
                                    <asp:Label runat="server" CssClass="font-weight-bold" ID="lblIdentidadVendedor_Recibo"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- =========================== HTML Traspaso de vehiculo del cliente ================== -->
        <div id="divContenedorTraspaso" class="contenedorPDF">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divTraspasoPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">TRASPASO DE VEHÍCULO</h5>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                <b>YO,
                                    <asp:Label runat="server" ID="lblNombreCliente_Traspaso"></asp:Label>,
                                </b>
                                mayor de edad,
                                <asp:Label runat="server" ID="lblNacionalidad_Traspaso"></asp:Label>,
                                con número de identidad
                                <asp:Label runat="server" ID="lblIdentidad_Traspaso"></asp:Label>
                                y de este domicilio,
                                en mi condición de propietario por medio de este documento hago formal traspaso del vehiculo que se describe de la forma siguiente
                            </p>
                            <div class="row">
                                <div class="col-6">
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Marca:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblMarca_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Tipo:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblTipoDeVehiculo_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Motor:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblSerieMotor_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            VIN:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblVIN_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Cilindraje:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblCilindraje_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6">
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Modelo:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblModelo_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Color:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblColor_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Chasis:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblSerieChasis_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Año:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblAnio_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Matricula:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblMatricula_Traspaso"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <p>
                                El cual cedo todos los deberes y derechos que antes ejercía sobre el vehículo antes mencionado a
                                <span class="lblRazonSocial"></span>,
                                con Numero de RTN 
                                <span class="lblRTNEmpresa"></span>,
                                aceptando que el vehículo es
                                <asp:Label runat="server" ID="lblGarantiaUsada_Traspaso"></asp:Label>
                                y se encuentra de su entera satisfacción sin garantía alguna.
                                Y para seguridad y constancia de las autoridades firmo en la ciudad de
                                <span class="lblCiudad_Firma"></span>,
                                a los
                                <span class="lblNumeroDia_Firma"></span>
                                días del mes de
                                <span class="lblMes_Firma"></span>
                                del año
                                <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-1"></div>
                        <div class="col-5 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Nombre</label>
                        </div>
                        <div class="col-5 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Firma</label>
                        </div>
                        <div class="col-1"></div>
                        <div class="col-3"></div>
                        <div class="col-6 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Identidad</label>
                        </div>
                        <div class="col-3"></div>
                    </div>
                </div>
            </div>
        </div>


        <!-- =========================== HTML Traspaso de vehiculo del propietario ============== -->
        <div id="divContenedorTraspasoVendedor" class="contenedorPDF">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divTraspasoVendedorPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">TRASPASO DE VEHÍCULO</h5>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                <b>YO,
                                    <asp:Label runat="server" ID="lblNombreCliente_TraspasoVendedor"></asp:Label>,
                                </b>
                                mayor de edad,
                                <asp:Label runat="server" ID="lblNacionalidad_TraspasoVendedor"></asp:Label>,
                                con número de identidad
                                <asp:Label runat="server" ID="lblIdentidad_TraspasoVendedor"></asp:Label>
                                y de este domicilio,
                                en mi condición de propietario por medio de este documento hago formal traspaso del vehiculo que se describe de la forma siguiente
                            </p>
                            <div class="row">
                                <div class="col-6">
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Marca:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblMarca_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Tipo:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblTipoDeVehiculo_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Motor:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblSerieMotor_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            VIN:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblVIN_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Cilindraje:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblCilindraje_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-6">
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Modelo:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblModelo_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Color:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblColor_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Chasis:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblSerieChasis_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Año:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblAnio_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-4 font-weight-bold">
                                            Matricula:
                                        </div>
                                        <div class="col-sm-8">
                                            <asp:Label runat="server" ID="lblMatricula_TraspasoVendedor"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <p>
                                El cual cedo todos los deberes y derechos que antes ejercía sobre el vehículo antes mencionado a
                                <span class="lblRazonSocial"></span>,
                                con Numero de RTN
                                <span class="lblRTNEmpresa"></span>,
                                aceptando que el vehículo es
                                <asp:Label runat="server" ID="lblGarantiaUsada_TraspasoVendedor"></asp:Label>
                                y se encuentra de su entera satisfacción sin garantía alguna.
                                Y para seguridad y constancia de las autoridades firmo en la ciudad de
                                <span class="lblCiudad_Firma"></span>,
                                a los
                                <span class="lblNumeroDia_Firma"></span>
                                días del mes de
                                <span class="lblMes_Firma"></span>
                                del año
                                <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-1"></div>
                        <div class="col-5 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Nombre</label>
                        </div>
                        <div class="col-5 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Firma</label>
                        </div>
                        <div class="col-1"></div>
                        <div class="col-3"></div>
                        <div class="col-6 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Identidad</label>
                        </div>
                        <div class="col-3"></div>
                    </div>
                </div>
            </div>
        </div>


        <!-- =========================== HTML Básico + CPI ====================================== -->
        <div id="divContenedorBasicoCPI" class="contenedorPDF">
            <div class="card m-0 pt-4 divImprimir" runat="server" visible="true" id="divBasicoCPIPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 text-center">
                            <h6 class="font-weight-bold">GARANTIA SOBRE EL VEHICULO DADO EN GARANTIA DEL PRESTAMO</h6>
                            <label class="font-weight-bold">De Las Partes</label>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                Por una Parte,
                                <span class="lblRazonSocial"></span>
                                con domicilio en la ciudad de
                                <span class="lblCiudadDomicilioEmpresa"></span>,
                                departamento de
                                <span class="lblDepartamentoDomicilioEmpresa"></span>,
                                representada por el señor
                                <span class="lblNombreRepresentanteLegal"></span>,
                                <span class="lblConstitucionFondo"></span>
                                que en lo sucesivo se llamara &lt;LA COMPAÑÍA&gt;, de
                                conformidad a la SOLICITUD presentada al cliente y las CONDICIONES GENERALES de la presente.
                                Por la otra
                                <asp:Label Font-Bold="true" runat="server" ID="lblNombreCliente_BasicoCPI"></asp:Label>
                                <b>(El cliente)</b>, con las generales mencionadas en el prestamo
                                <asp:Label runat="server" Font-Bold="true" ID="lblNumeroPrestamo_BasicoCPI"></asp:Label>
                                que en lo sucesivo se denominará &lt;EL CONTRATANTE&gt; o &lt;EL ARRENDATARIO&gt;.
                            </p>
                            <p>
                                <b>FINALIDAD DEL CONVENIO:</b>
                                El presente convenio tiene por objeto cubrir los siniestros en que pudiese verse
                                involucrado El VEHICULO DADO EN GARANTIA DEL PRESTAMO, para lo cual se obliga por una parte a la
                                <b>compañía</b> o la que esta contrate, a cubrir el saldo capital hasta la fecha en que pueda existir algún siniestro,
                                y por la otra parte el contratante o cliente se obliga a pagar el servicio mediante cuotas.
                            </p>
                            <p class="text-center">
                                <b>CONDICIONES GENERALES</b>
                            </p>
                            <p>
                                <b>PRIMERA: CONSTITUCIÓN DEL CONTRATO</b>
                                El convenio de GARANTÍA SOBRE EL VEHICULO DADO EN GARANTIA DEL PRESTAMO queda
                                constituido mediante el presente contrato, el cual es firmado y se estampa la huella digital de ambas partes
                                en señal de aceptación de todas las condiciones establecidas partes.
                            </p>
                            <p>
                                <b>SEGUNDA: OBJETO DE LA GARANTÍA SOBRE EL VEHICULO DADO EN GARANTIA</b>
                                <br />
                                La GARANTÍA está destinada exclusivamente en cubrir el vehículo de los siguientes siniestros:
                            </p>
                            <ul>
                                <li>Vuelcos Accidentales o Colisiones</li>
                                <li>Incendio, Auto ignición y Rayo</li>
                                <li>Robo Total.</li>
                                <li>Huelgas y Alborotos Populares.</li>
                                <li>Responsabilidad Civil en sus Bienes (Daños a Terceros en sus Bienes, hasta Lps 50,000)</li>
                                <li>Responsabilidad Civil en sus Personas (Daños a Terceros en sus Personas, hasta Lps 50,000)</li>
                                <li>Desbordamiento de Ríos, derrumbes de carretera y otros fenómenos de la naturaleza.</li>
                            </ul>
                            <p>
                                <b>TERCERA: CONDICIONES ESPECIALES</b>
                            </p>
                            <ul>
                                <li>Los siniestros mencionados en la cláusula anterior deben ocurrir en el territorio nacional para que sea cubierto.</li>
                                <li>El vehículo debe de ser conducido por una persona debidamente acreditada, mayor de 18 años y menor a 75 años de lo contrario la garantía no cubre.</li>
                                <li>Aplica única y exclusivamente para reparar los daños ocasionados al automóvil que se encuentre en arrendamiento con
                                “<span class="lblRazonSocial"></span>”
                                o en su defecto los daños causados a un tercero hasta Lps 50,000 siempre y cuando el arrendamiento este vigente y con sus saldos al día, bajos los límites de cobertura indicados, el saldo capital adeudado a la fecha del siniestro.
                                </li>
                                <li>Para que los daños ocasionados sean cubiertos, el automóvil deberá ser conducido por El Arrendatario con licencia vigente y deberá estar al día en sus cuotas de arrendamiento,
                                caso contrario los daños ocasionados serán reparados por cuenta del arrendatario.
                                </li>
                                <li>El Arrendatario debe contar con licencia de conducir vigente.</li>
                                <li class="page-break">Solo cubre colisiones debidamente acreditas con el parte de la policía de tránsito y cuando sea
                                denuncia interpuesta sin parte de transito solo se reconoce como máximo el 50% del valor del siniestro previa inspección de ajustador.
                                </li>

                                <li class="mt-5">En caso de robo del vehículo en garantía sera necesario acreditar dicho siniestro ante entidad
                                    competente en territorio Hondureño.
                                </li>
                                <li>El arrendatario se obliga a reportar en un plazo máximo de 24 hrs el siniestro a
                                    <span class="lblRazonSocial"></span>
                                    al teléfono
                                    <span class="lblTelefonoEmpresa"></span>
                                    o vía email a
                                    <span class="lblEmailEmpresa"></span>
                                </li>
                                <li>Mantener al día el pago del préstamo y la cuota del servicio del seguro y respectivo GPS.</li>
                            </ul>
                            <p>
                                <b>CUARTA: SE EXCLUYE DE SINIESTROS</b>
                            </p>
                            <ul>
                                <li>No se cubre los accidentes o daños ocasionados al automóvil por el arrendatario, sin que tenga
                                implicación un tercero a excepción de Vuelcos Accidentales, Colisiones Accidentales, Incendio, Auto
                                ignición, Rayo, Desbordamiento de Ríos, derrumbes de carretera y otros fenómenos de la naturaleza
                                </li>
                                <li>No cubre rotura de cristales</li>
                                <li>No cubre equipo especial que pueda ser instalado</li>
                            </ul>
                            <p>
                                <b>QUINTA: PROCESO DE RECLAMO</b>
                                <br />
                                Al presentarse un reclamo el usuario deberá avocarse de manera personal al Oficial de Crédito de
                                <span class="lblRazonSocial"></span>
                            </p>
                            <ul>
                                <li>Presentar dos cotizaciones de los daños ocasionados al automóvil asegurado</li>
                                <li>Certificación del Parte de Tránsito o Certificado de Denuncia interpuesta</li>
                                <li>Fotografías a color de los daños del automóvil</li>
                                <li>Fotografías a color del automóvil una vez reparado</li>
                                <li>Los derechos de cobertura no pueden ser traspasados, cedidos, ni endosados</li>
                                <li>
                                    <span class="lblRazonSocial"></span>
                                    optara por una tercera cotización, reservándose el derecho de autorizar la reparación del automóvil asegurado en el taller de su elección.
                                </li>
                            </ul>
                            <p>
                                Una vez completada la documentación de reclamación del siniestro el proceso de pago lo realizara la empresa
                                en un plazo no mayor a 10 días de hábiles, conforme a reclamo
                            </p>
                            <p>
                                <b>SEXTA: El contratante,</b>
                                entiende y acepta que las coberturas anteriormente mencionadas están respaldas por
                                una compañía de seguros local y que exclusivamente en caso de que la compañía aseguradora local no cubra
                                el montó del siniestro debido a que este exceda el límite anual especificado en el contrato de crédito, este
                                será cubierto por una empresa extranjera tercera contratada por
                                <span class="lblRazonSocial"></span>
                                para este fin, misma que se
                                limita al valor del saldo capital al momento en que se presenta el siniestro o al valor de mercado de la garantía.
                            </p>
                            <p>
                                <b>SEPTIMA: El contratante</b>
                                acepta que las cuotas por la contratación de este servicio, sean cargadas a su plan de
                                pago del préstamo que se otorgó para la compra del vehículo dado en garantía, cuotas que serán de valor fijo, para
                                la cual se obliga a cumplir en tiempo y forma, para que la garantía le cubra los siniestros que pudiesen ocurrir.
                            </p>
                            <p>
                                El contratante declara haber entendido en toda su amplitud y alcances la presente garantía, por lo cual en fe
                                de lo acá establecido se firma y se estampa huella digital en fecha
                                <span class="lblNumeroDia_Firma"></span>
                                de
                                <span class="lblMes_Firma"></span>
                                del
                                <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-5 text-center mt-1">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Contratante o Cliente</label>
                        </div>
                        <div class="col-5 text-center mt-1">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block lblRazonSocial"></label>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- =========================== HTML Correo liquidación ================================ -->
        <div id="divContenedorCorreoLiquidacion" style="display: none;">
            <div id="divCorreoLiquidacionPDF">
                <table border="1" style="width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;">
                    <tr>
                        <th colspan='2' style='text-align: left; font-weight: bold;'><em>DATOS DEL VEHICULO</em></th>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>AÑO</th>
                        <td>
                            <asp:Label runat="server" ID="lblAño_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>PLACA</th>
                        <td>
                            <asp:Label runat="server" ID="lblPlaca_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>MARCA</th>
                        <td>
                            <asp:Label runat="server" ID="lblMarca_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>MODELO</th>
                        <td>
                            <asp:Label runat="server" ID="lblModelo_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>TIPO</th>
                        <td>
                            <asp:Label runat="server" ID="lblTipoVehiculo_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>COLOR</th>
                        <td>
                            <asp:Label runat="server" ID="lblColor_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>MOTOR</th>
                        <td>
                            <asp:Label runat="server" ID="lblSerieMotor_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>CHASIS</th>
                        <td>
                            <asp:Label runat="server" ID="lblSerieChasis_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>VIN</th>
                        <td>
                            <asp:Label runat="server" ID="lblVIN_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="2">
                            <br />
                        </th>
                    </tr>
                    <tr>
                        <th colspan='2' style='text-align: left; font-weight: bold;'><em>DATOS DEL CLIENTE</em></th>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>NOMBRE</th>
                        <td>
                            <asp:Label runat="server" ID="lblNombreCliente_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>IDENTIDAD</th>
                        <td>
                            <asp:Label runat="server" ID="lblIdentidadCliente_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'># PRESTAMO</th>
                        <td>
                            <asp:Label runat="server" ID="lblNumeroPrestamo_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th colspan="2">
                            <br />
                        </th>
                    </tr>
                    <tr>
                        <th colspan='2' style='text-align: left; font-weight: bold;'><em>DATOS DEL VENDEDOR</em></th>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>NOMBRE</th>
                        <td>
                            <asp:Label runat="server" ID="lblNombreVendedor_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>IDENTIDAD</th>
                        <td>
                            <asp:Label runat="server" ID="lblIdentidadVendedor_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>VALOR NUMERO</th>
                        <td>
                            <asp:Label runat="server" ID="lblValorNumero_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>VALOR LETRA</th>
                        <td>
                            <asp:Label runat="server" ID="lblValorLetra_CorreoLiquidacion"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>


        <!-- ================================== HTML Correo de seguro =========================== -->
        <div id="divContenedorCorreoSeguro" style="display: none;">
            <div id="divCorreoSeguroPDF">
                <table border="1" style="width: 100%; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;">
                    <tr>
                        <th colspan='2' style='text-align: center; font-weight: bold; background-color: #fefbd8'>DATOS DEL VEHICULO</th>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>AÑO</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblAño_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>PLACA</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblPlaca_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>MARCA</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblMarca_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>MODELO</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblModelo_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>TIPO</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblTipoVehiculo_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>COLOR</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblColor_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>MOTOR</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblSerieMotor_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>CHASIS</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblSerieChasis_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>VIN</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblVIN_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="1" style="width: 100%; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;">
                    <tr>
                        <th colspan='2' style='text-align: center; font-weight: bold; background-color: #fefbd8'>DATOS DEL CLIENTE</th>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>NOMBRE</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblNombreCliente_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left; padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #80ced6; width: 80px;'>IDENTIDAD</th>
                        <td style="padding: 5px; border: 1px solid darkgray; border-collapse: collapse; background-color: #d5f4e6; font-weight: normal">
                            <asp:Label runat="server" ID="lblIdentidadCliente_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>


        <!-- ================================== HTML Nota de entrega ============================ -->
        <div id="divContenedorNotaDeEntrega" class="contenedorPDF">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divNotaDeEntregaPDF" style="display: none;">
                <div class="card-body pt-0 pr-5 pl-5">
                    <div class="row">
                        <div class="col-12 m-0 p-0 text-center">
                            <img src="//:0" class="img-fluid pt-4 img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h4 class="text-center pt-2 pb-4 font-weight-bold">NOTA DE ENTREGA
                                <br />
                                POR APROBACIÓN DE CRÉDITO
                            </h4>
                        </div>
                    </div>
                    <div class="row justify-content-center">
                        <div class="col-12">
                            <p>
                                Estimado (s)
                                <asp:Label runat="server" CssClass="font-weight-bold" ID="lblPropietarioGarantia_NotaEntrega"></asp:Label>,
                                por este medio hacemos de su conocimiento que el cliente
                                <asp:Label runat="server" ID="lblNombreCliente_NotaEntrega"></asp:Label>,
                                cuenta con un crédito aprobado con
                                <span class="lblNombreComercial"></span>
                                por la cantidad de
                                <asp:Label runat="server" ID="lblValorAPrestarEnPalabras_NotaEntrega"></asp:Label>
                                (<asp:Label runat="server" ID="lblValorAPrestar_NotaEntrega" CssClass="font-weight-bold"></asp:Label>)
                                solicitamos de su parte proceder con la entrega del vehículo al cliente en mención, así mismo se le pide entregar al Oficial de
                                <span class="lblNombreComercial"></span>
                                la documentación original y completa del automóvil con las siguientes características:
                            </p>
                        </div>

                        <div class="col-6">
                            <table class="table table-sm table-bordered">
                                <tr>
                                    <th>ESTADO</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblEstadoGarantia_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>MARCA</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblMarca_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>MODELO</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblModelo_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>AÑO</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblAño_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>TIPO</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblTipo_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>CHASIS</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblChasis_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>MOTOR</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblSerieMotor_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>COLOR</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblColor_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>CILINDRAJE</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblCilindraje_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th>PLACA</th>
                                    <td>
                                        <asp:Label runat="server" ID="lblPlaca_NotaEntrega"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div class="col-12">
                            <p runat="server" id="lblParrafoFinal_NotaEntrega"></p>
                            <p>
                                Y para los fines que estime conveniente, se le extiende la presente en la ciudad de
                                <span class="lblCiudad_Firma"></span>,
                                departamento de
                                <span class="lblDepartamento_Firma"></span>,
                                a los
                                <span class="lblNumeroDia_Firma"></span>
                                días del mes de
                                <span class="lblMes_Firma"></span>
                                del año
                                <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-3"></div>
                        <div class="col-6 text-center p-0 mt-3">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">SELLO Y FIRMA</label>
                            <label class="mt-0 d-block lblRazonSocial"></label>
                        </div>
                        <div class="col-3"></div>
                    </div>
                </div>
            </div>
        </div>


        <!-- ================================== HTML Portada del expediente ===================== -->
        <div id="divContenedorPortadaExpediente" runat="server" visible="false">
            <div class="card m-0" runat="server" visible="true" id="divPortadaExpedientePDF">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-6 m-0 p-0">
                            <img src="//:0" class="img-fluid pt-4 img-logo-empresa" />
                        </div>
                        <div class="col-6 m-0 p-0 justify-content-end">
                            <table class="table table-borderless m-0" style="width: 100%">
                                <tr runat="server" id="tr1">
                                    <td class="p-0 text-right">
                                        <asp:Label ID="lblNoSolicitud_PortadaExpediente" CssClass="col-8 p-0 font-weight-bold" Text="Solicitud de crédito #1057" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="p-0 text-right">
                                        <asp:Label ID="lblOficialNegocios_PortadaExpediente" CssClass="p-0 font-weight-bold" Text="Oficial de negocios: Alejandro Flores" runat="server"></asp:Label></td>
                                </tr>
                                <tr runat="server" id="trCentroDeCostoVendedor_PortadaExpediente">
                                    <td class="p-0 text-right">
                                        <asp:Label ID="lblCentroDeCosto_PortadaExpediente" CssClass="col-8 p-0 font-weight-bold" Text="Centro de costo: Prestadito Matriz" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                            <table class="table table-borderless m-0" style="width: 100%">
                                <tr>
                                    <td class="p-0 text-right">
                                        <asp:Label ID="lblFechaActual_PortadaExpediente" CssClass="col-8 p-0 font-weight-bold" Text="02/06/2021" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-12 mb-0">
                            <h5 class="text-center font-weight-bold mb-1">Expediente</h5>
                        </div>
                        <div class="col-12">
                            <table class="table table-sm mb-0">
                                <thead class="text-center">
                                    <tr>
                                        <th colspan="6" class="p-1 font-weight-bold">Información del cliente / Garantía</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class="p-1">Cliente</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblNombreCliente_PortadaExpediente"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="p-1">Identidad</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblIdentidadCliente_PortadaExpediente"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">Marca</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblMarca_PortadaExpediente"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="p-1">Modelo</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblModelo_PortadaExpediente"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">Color</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblColor_PortadaExpediente"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="p-1">Año</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblAnio_PortadaExpediente"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">Placa</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblPlaca_PortadaExpediente"></asp:Label>
                                        </td>
                                        <td></td>
                                        <td class="p-1">VIN</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblVIN_PortadaExpediente"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">Vendedor</td>
                                        <td colspan="5" class="p-1">
                                            <asp:Label runat="server" ID="lblVendedorGarantia_PortadaExpediente"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">Dueño anterior</td>
                                        <td colspan="5" class="p-1">
                                            <asp:Label runat="server" ID="lblDuenoAnteriorGarantia_PortadaExpediente"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="6"></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 mb-0 pl-0">
                            <div id="divGaleriaPortadaExpediente" style="width: 100% !important; max-width: 100% !important; overflow-x: hidden;" runat="server"></div>
                        </div>
                        <div class="col-12 mt-0 pl-0">
                            <div id="divPortadaExpediente_Revision" style="width: 100% !important; max-width: 100% !important; overflow-x: hidden;" runat="server"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- ================================== HTML Expendiente ================================ -->
        <div id="divContenedorExpediente" class="contenedorPDF">
            <div class="card m-0 divImprimir font-12" runat="server" visible="true" id="divExpedientePDF" style="display: none;">
                <div class="card-body pt-0 pr-5 pl-5">
                    <div class="row justify-content-between">
                        <div class="col-auto">
                            <label class="font-weight-bold d-block lblRazonSocial"></label>
                            <label class="font-weight-bold d-block lblNombreComercial"></label>
                            <label class="font-weight-bold d-block">
                                Solicitud de crédito N°:
                                <b runat="server" id="lblNoSolicitudCredito_Expediente"></b>
                            </label>
                            <small class="font-weight-bold">Fecha:
                                <b runat="server" id="lblFechaActual_Expediente"></b>
                            </small>
                        </div>
                        <div class="col-auto align-content-start pr-0">
                            <div id="qr_Expediente"></div>
                        </div>
                    </div>
                    <div class="row border border-gray mb-2 mt-2">
                        <div class="col-7">
                            <div class="form-group row mb-1">
                                <div class="col-4">
                                    <b>Nombre:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblNombreCliente_Expediente" runat="server"></label>
                                </div>

                                <div class="col-4">
                                    <b>Identidad:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblIdentidadCliente_Expediente" runat="server"></label>
                                </div>

                                <div class="col-4">
                                    <b>Departamento:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblDepartamento_Expediente" runat="server"></label>
                                </div>

                                <div class="col-4">
                                    <b>Dirección:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblDireccionCliente_Expediente" runat="server"></label>
                                </div>

                                <div class="col-4">
                                    <b>Tel. Celular:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblTelefonoCliente_Expediente" runat="server"></label>
                                </div>
                            </div>
                            <u class="font-weight-bold">Datos laborales</u>
                            <div class="form-group row mb-0">
                                <div class="col-4">
                                    <b>Tipo de trabajo:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblTipoDeTrabajo_Expediente" runat="server"></label>
                                </div>
                                <div class="col-4">
                                    <b>Cargo:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblPuestoAsignado_Expediente" runat="server"></label>
                                </div>
                                <div class="col-4">
                                    <b>Teléfono:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblTelefonoTrabajo_Expediente" runat="server"></label>
                                </div>
                                <div class="col-4">
                                    <b>Dirección:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblDirecciónTrabajo_Expediente" runat="server"></label>
                                </div>
                            </div>
                        </div>
                        <div class="col-5 border-left border-gray">
                            <div class="form-group row border-bottom border-gray mb-1">
                                <u class="p-2 font-weight-bold">Datos del préstamo</u>
                            </div>
                            <div class="form-group row mb-0">
                                <div class="col-6">
                                    <b>N° Solicitud:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblNoSolicitud_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Fecha de otorgamiento:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblFechaOtorgamiento_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Plazo:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblCantidadCuotas_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Monto otorgado:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblMontoOtorgado_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Valor de la cuota:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblValorCuota_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Fecha primer pago:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblFechaPrimerPago_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Frecuencia:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblFrecuenciaPlazo_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Vencimiento</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblFechaVencimiento_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Oficial:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblOficialNegocios_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Gestor:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblGestor_Expediente" runat="server"></label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6 p-0">
                            <asp:Table ID="tblDocumentos_Expediente" CssClass="table table-condensed table-bordered" runat="server">
                                <asp:TableHeaderRow TableSection="TableHeader">
                                    <asp:TableHeaderCell>Tipo de documento</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">SI</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">NO</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">N/A</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </div>
                        <div class="col-6">
                            <u class="font-weight-bold">TIPO DE RENEGOCIACIÓN</u>
                            <asp:Table ID="tblTipoDeSolicitud_Expediente" CssClass="font-weight-bold table-borderless mt-2" runat="server"></asp:Table>
                        </div>
                        <div class="col-12 p-0">
                            <div class="form-group row">
                                <label class="col-2 pr-0">Especifique otros:</label>
                                <asp:Label runat="server" ID="lblEspecifiqueOtros_Expediente" CssClass="col-10 border-top-0 border-left-0 border-right-0 border-bottom border-dark"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row pt-2 justify-content-center">
                        <div class="col-5 text-center">
                            <label class="form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Firma de entrega</label>
                            <label class="mt-0 d-block" runat="server" id="lblNombreFirmaEntrega_Expediente"></label>
                        </div>
                        <div class="col-5 text-center">
                            <label class="form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Firma de recibe</label>
                            <label class="mt-0 d-block">Mariely Guzman</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- ================================== HTML Memorandum ================================= -->
        <div id="divContenedorMemorandum" class="contenedorPDF">
            <div class="card m-0 divImprimir" runat="server" visible="false" id="divMemorandumPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row justify-content-between">
                        <div class="col-auto m-0 p-0">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-auto align-content-start pr-0">
                            <div id="qr_Memorandum"></div>
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold border-bottom">MEMORANDUM</h5>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">

                            <table class="table table-sm table-borderless font-16">
                                <tbody>
                                    <tr>
                                        <td class="p-1 font-weight-bold">Para:</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblPara_Memorandum"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1 font-weight-bold">De:</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblDe_Memorandum"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1 font-weight-bold">Fecha:</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblFecha_Memorandum">24-02-2021</asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1 font-weight-bold">Asunto:</td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblAsunto_Memorandum"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                </tbody>
                            </table>

                            <p>
                                Por este medio, hago entrega de documentos originales del cliente:                                
                            </p>
                            <table class="table table-sm table-bordered font-16">
                                <thead>
                                    <tr>
                                        <td class="font-weight-bold">Cliente</td>
                                        <td class="font-weight-bold">Número de placa</td>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblCliente_Memorandum">Daniel Andres Coto Velasquez</asp:Label>
                                        </td>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblNumeroPlaca_Memorandum">HBF8496</asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                            <p>
                                Por este medio hago constar que he recibido los documentos arriba mencionado, mismos que quedan bajo mi custodia y responsabilidad;
                                además me comprometo a devolverlos de manera íntegra y completa.
                                Es por eso que hago constar que si los extravío, daño  o los pierdo me comprometo a conseguir en el IP o en cualquier otra institución ya que entiendo que 
                                estos documentos son garantías de préstamo y representan dinero.
                            </p>
                        </div>
                    </div>
                    <!-- Firma -->
                    <div class="row pt-5 justify-content-center">
                        <div class="col-5 text-center">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Nombre Completo</label>
                        </div>
                        <div class="col-5 text-center">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Firma</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- ================================== HTML Acta de compromiso ========================= -->
        <div id="divContenedorActaDeCompromiso" class="contenedorPDF">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divActaDeCompromisoPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">ACTA DE COMPROMISO</h5>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                Por medio de la presente yo
                                <asp:Label runat="server" ID="lblNombreCliente_ActaDeCompromiso"></asp:Label>,
                                con numero de identidad
                                <asp:Label runat="server" ID="lblIdentidadCliente_ActaDeCompromiso"></asp:Label>,
                                me comprometo a realizar y entregar el documento de LICENCIA DE CONDUCIR en los próximos 30 días, proceso que no he podido realizar debido a que la Dirección Nacional de Vialidad y Transporte (Departamento de Tránsito) se encontraba cerrada por la emergencia del COVID-19. 
                                Dicho documento me es solicitado por
                                <b class="lblNombreComercial"></b>,
                                y ________________________________
                                para realizar un trámite de financiamiento para la compra de mi vehiculo:
                                <br />
                                Marca:
                                <asp:Label runat="server" ID="lblMarca_ActaDeCompromiso"></asp:Label>,
                                Modelo:
                                <asp:Label runat="server" ID="lblModelo_ActaDeCompromiso"></asp:Label>,
                                Año:
                                <asp:Label runat="server" ID="lblAnio_ActaDeCompromiso"></asp:Label>,
                                Cilindraje:
                                <asp:Label runat="server" ID="lblCilindraje_ActaDeCompromiso"></asp:Label>,
                                Color:
                                <asp:Label runat="server" ID="lblColor_ActaDeCompromiso"></asp:Label>,
                                Motor:
                                <asp:Label runat="server" ID="lblMotor_ActaDeCompromiso"></asp:Label>,
                            </p>
                            <p>
                                <b>IMPORTANTE:</b>
                                es de mi conocimiento que de no entregar dicho documento en los próximos 30 días doy autorización a
                                <span class="lblNombreComercial"></span>
                                a recuperar la vehiculo adquirida mediante el financiamiento de ellos, y también me doy por enterado de que no se me hará entrega de placas y boleta de revisión.
                            </p>
                            <p>
                                Doy fe y constancia de que firmo este documento libre y espontáneamente
                                en la ciudad de
                                <span class="lblCiudad_Firma"></span>,
                                departamento de
                                <span class="lblDepartamento_Firma"></span>,
                                a los
                                <span class="lblNumeroDia_Firma"></span>
                                días del mes de 
                                <span class="lblMes_Firma"></span>
                                del año <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <!-- Firma -->
                    <div class="row mb-5">
                        <div class="col-12">
                            <label class="d-block mb-3">Firma: __________________________________________________</label>
                            <label class="d-block">No. ID: _________________________________________________</label>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <label class="d-block lblNombreComercial"></label>
                            <label class="d-block">C.C: ____________________________________________________</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <!-- ============== Modal de guardar expediente de la solicitud ========================= -->
        <div id="modalGuardarExpedienteSolicitud" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalGuardarExpedienteSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalGuardarExpedienteSolicitudLabel">Expediente de la solicitud de crédito</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <label class="mb-3">Documentos del expediente:</label>

                        <ul id="ulDocumentosDelExpediente" style="max-height: 50vh; overflow-x: auto;"></ul>

                        <div class="form-group mt-3">
                            <label>Especifique otras</label>
                            <textarea id="txtEspecifiqueOtras" class="form-control form-control-sm" data-parsley-maxlength="500" runat="server"></textarea>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnGuardarExpedienteSolicitud" class="btn btn-info mr-1">Guardar</button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>


        <!-- ============== Modal de enviar información para asegurar por correo electrónico ==== -->
        <div id="modalEnviarInformacionAseguradora" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalEnviarInformacionAseguradoraLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalEnviarInformacionAseguradoraLabel">Enviar información de seguro</h6>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body">

                        <div class="spinner-border text-center" role="status" id="LoaderDocumentosParaAsegurar">
                            <span class="sr-only">Cargando</span>
                        </div>
                        <div id="divDocumentosParaAsegurarPendientes" style="display: none;">

                            <h6 class="font-weight-bold">Documentos pendientes</h6>

                            <div class="mb-3" id="DivDocumentacionParaAsegurar"></div>
                        </div>
                        <div id="divParaAsegurarNoHayDocumentosPendientes">
                            <h6 class="font-weight-bold">¡No hay documentos pendientes!</h6>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnEnviarInformacionAseguradora_Confirmar" class="btn btn-primary">
                            Guardar y enviar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>
       

        
                <!-- ================================== HTML PLAN DE PAGOS ================================ -->
        <div id="divContenedorPlanDePagos" style =" font-size:15px!important;" >
            <div class="card m-0 divImprimir font-12" runat="server" visible="true" id="divPlanDePagosPDF"  style=" display:none !important;">
                <div class="card-body pt-0 pr-5 pl-5">
                     <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="//:0" class="img-fluid img-logo-empresa" />
                        </div>
                        <div class="col-12">
                            <h6 class="text-center font-weight-bold"  style ="font-size:18px!important;">PAYMENT PLAN</h6>                           
                        </div>
                    </div>

                    <div class="row justify-content-between" style ="font-size:15px!important;">
                        <div class="row" >
                          <small class="col-4">
                             <b style ="font-size:18px!important;">Loan Number:</b> <asp:Label runat="server" style ="font-size:18px!important;"  ID="lblPrestamo_PlanDePagos"></asp:Label>  
                          </small>

                          <small class="col-4">   
                               <b style ="font-size:18px!important;">First Payment:</b> <asp:Label runat="server"  style ="font-size:18px!important;" ID="lblFechaInicio_PlanDePagos"></asp:Label> 
                                  
                         </small>

                              <small class="col-4">
                              <b style ="font-size:18px!important;">Loan Amount:</b>  <asp:Label runat="server"  style ="font-size:18px!important;" ID="lblCapitalFinanciado_PlanDePagos"></asp:Label> 
                          </small>
                          

                         
                           <!-- ==================================row================================ -->

                         <small class="col-4">
                              <b style ="font-size:18px!important;">Product:</b>  <asp:Label runat="server" style ="font-size:18px!important;"  ID="lblProducto_PlanDePagos"></asp:Label> 
                         </small>


                         

                          <small class="col-4">
                               <b style ="font-size:18px!important;">Final Payment:</b> <asp:Label runat="server"  style ="font-size:18px!important;" ID="lblFechaFinal_PlanDePagos"></asp:Label>          
                          </small>
                        
                              <small class="col-4" >
                                <b style ="font-size:18px!important;">Payment Amount:</b>  <asp:Label runat="server"  style ="font-size:18px!important;" ID="lblValorCuota_PlanDePagos"></asp:Label> 
                          </small>

                        
                           <!-- ==================================row================================ -->
                           <small class="col-4">
                              <b style ="font-size:18px!important;">Customer:</b>  <asp:Label runat="server" style ="font-size:17px!important;" ID="lblNombreCliente_PlanDePagos"></asp:Label> 
                          </small>

                       

                      
                          <small class="col-4">
                              <b style ="font-size:18px!important;">Frequency:</b>  <asp:Label runat="server" style ="font-size:18px!important;" ID="lblFrecuenciaPago_PlanDePagos"></asp:Label> 
                          </small>

                            <small class="col-4">                              
                              <b style="display: none">Tasa Anual:</b>  <asp:Label runat="server" ID="lblTasaInteres_PlanDePagos" style="display: none"></asp:Label> 
                          </small>
                          <!-- ==================================row================================ -->


                

                           <small class="col-4">
                              <b style ="font-size:18px!important;">Inception Date:</b>  <asp:Label runat="server" style ="font-size:18px!important;" ID="lblInceptionDate_PlanDePagos"></asp:Label> 
                          </small>

                               
                           <small class="col-4">
                              <b style ="font-size:18px!important;">Total Payments:</b>  <asp:Label runat="server" style ="font-size:18px!important;" ID="lblTotalCuotas_PlanDePagos"></asp:Label> 
                          </small>


                        </div>
                         
                    
                     
                    </div>
                    
                   <%--  <hr />--%>
                   <%-- <div class="row border border-gray mb-2 mt-2">
                        
               </div>--%>
                    <br />
                    <div class="row">
                        <div class="col-12 p-0">
                            <asp:Table ID="tbl_PlanDePagos" CssClass="table table-condensed table-bordered ; font-16" runat="server">
                                  <asp:TableHeaderRow TableSection="TableHeader">
                                    <asp:TableHeaderCell RowSpan="2" CssClass="text-center; font-16"># PMT</asp:TableHeaderCell>
                                    <asp:TableHeaderCell RowSpan="2" CssClass="text-center; font-14;">Payment Due Date</asp:TableHeaderCell>
                                    <asp:TableHeaderCell RowSpan="2" CssClass="text-center; font-16" >Beginning Principal Balance</asp:TableHeaderCell>
                                    <asp:TableHeaderCell  ColumnSpan="4" CssClass="text-center; font-16">Payment Details</asp:TableHeaderCell>
                                  
                                     <asp:TableHeaderCell RowSpan="2" CssClass="text-center ; font-16">Total Payment</asp:TableHeaderCell>
                                     <asp:TableHeaderCell RowSpan="2" CssClass="text-center; font-16">Ending Principal Balance</asp:TableHeaderCell>
                                </asp:TableHeaderRow>

                                <asp:TableHeaderRow TableSection="TableHeader">
                                   
                                    <asp:TableHeaderCell CssClass="text-center; font-16">Principal</asp:TableHeaderCell>
                                     <asp:TableHeaderCell CssClass="text-center; font-16">Interest</asp:TableHeaderCell>
                                     <asp:TableHeaderCell CssClass="text-center; font-16">CPP 1</asp:TableHeaderCell>
                                     <asp:TableHeaderCell CssClass="text-center; font-16">CPP 2</asp:TableHeaderCell>
                                     <%--  <asp:TableHeaderCell CssClass="text-center">GPS</asp:TableHeaderCell> --%>
                                   
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </div>
                    
                      
                    </div>
                   
                </div>
            </div>
        </div>


      <!-- ================================== HTML Contrato Prestadito Cash =================================== -->
      <div id="divContenedorContratoPrestaditoCash" class="contenedorPDF">         
        <div class="card m-0 pt-4 divImprimir" runat="server" visible="true" id="divContratoPrestaditoCashPDF" style="display: none;">
           <div class="card-body pt-0">
               <div class="row">
                
                   <div class="col-12">
                       <h6 class="text-center font-weight-bold">DISCLOSURE STATEMENT, NOTE AND SECURITY AGREEMENT</h6>
                       <hr />
                   </div>
               </div>
          
              <table style="width:100% ; border:1px solid black; font-size:10px!important;" >
            <tr  style="border-right: 1px solid;" >
              <td  >Borrower(s) Name and mailing Address </td>
              <td >LENDER (NAME, ADDRESS, CITY AND STATE)</td>        
              <td >loan No:</td>          
            </tr>

            <tr>
              <td><b><asp:Label runat="server" ID="LblNombreCLiente_Cash"></asp:Label></b></td>
              <td><b><asp:Label runat="server" ID="lblNombrePropietarioGarantia_Cash"></asp:Label></b> </td>  
               <td><b><asp:Label runat="server" ID="lblPrestamo_Cash"></asp:Label></b></td>
            </tr>

            <tr>
             <td> <b><asp:Label runat="server" ID="LblDireccionCliente_Cash"></asp:Label></b> </td> 
             <td><b>17880 S. Dixie Hwy, Miami FI 33157</b></td> 
             <td>Date of Loan</td> 
            </tr>

            <tr>
             <td></td>
             <td></td>
             <td><b><asp:Label runat="server" ID="lblFechaDesembolso_Cash"></asp:Label></b></td>
            </tr>

           </table>
           <br/>
               <table style="width:100% ; border:1px solid black; font-size:10px!important; " >
                    <tr>
                      <td  >Anual Percentage Rate </td>
                      <td  >Finance Charge</td>        
                      <td  >Amount financed</td>
                      <td style=" width:10%">Collateral</td>  
                      <td  >Total of Payment</td>          
                    </tr>

                    <tr >
                      <td>The cost of Borrower's credit </td>
                      <td>the dollar amount the </td>
                      <td>the amount of credtit provided </td>
                      <td  style=" width:15%">Protection plan</td>
                      <td>the amount Borrower will have </td>   
        
                    </tr>

                <tr >
                        <td>credit as a yearly rate </td>
                        <td>credit will cost borrower </td>
                        <td>to Borrower or on Borrober' </td>
                        <td></td>
                        <td>paid after borrower has made </td>  
                </tr>
                   <tr>
                       <td></td>
                       <td></td>
                       <td>s behalf</td>
                       <td></td>
                       <td>all payments as sheduled</td>
                   </tr>

                <tr style="width:30% ; ">
                     <td><b>&nbsp; &nbsp; <asp:Label runat="server" ID="lblTasaInteres_CahsTable2"></asp:Label></b></td>
                     <td><b><asp:Label runat="server" ID="lblInteresesTotal_Cash"></asp:Label></b></td>
                     <td><b><asp:Label runat="server" ID="lblMontoFinalAFinanciarTabla_Cash"></asp:Label></b></td>                        
                     <td width:15%"><b><asp:Label runat="server" ID="lblCollateral"></asp:Label> </b></td>
                    <td><b><asp:Label runat="server" ID="blValorTotalCuota_Cash"></asp:Label></b></td>
                </tr>


               </table>
               <br/>
               <%--tabla 2--%>
               <div>
                       <div class="containerCash flexCash ">
                      <div class="leftCash">
                        <div><p  style=" font-size:11px!important;">Payment Schedule:</p></div>
                        <div class="flexCash">
                          <div class="columnCash"><p style="font-size:10px!important;"><b>Number of Payments </b></p> </div>
                          <div class="columnCash"><p style="font-size:10px!important;"><b>Amount of payments</b> </p> </div>
                          <div class="columnCash"><p style="font-size:10px!important;"><b>When Payments are Due </b> </p> </div>
                        </div>
                        <div class="flexCash" >
                          <div class="columnCash">
                           <table>
                             <tr>
                               <td  style="font-size:10px!important;">&nbsp; &nbsp; <asp:Label runat="server" ID="lblPlazoFinanciar_Cash"></asp:Label></td>          
                             </tr>
                             <tr>
                               <td  style="font-size:10px!important;">&nbsp; &nbsp;&nbsp;1</td>
                             </tr>
                           </table>
                          </div>
                          <div class="columnCash">
                               <table>
                              <tr>
                                <td style="font-size:10px!important;">$&nbsp; &nbsp;<asp:Label runat="server" ID="lblValorCuota_Cash"></asp:Label></td>                               
                              </tr>                             
                              <tr>
                                <td style="font-size:10px!important;">$&nbsp; &nbsp; <asp:Label runat="server" ID="lblValorCuota_Cash2"></asp:Label></td>
                              </tr>
             
                            </table>
                          </div>
                          <div class="columnCash">
                                <table>                                         
                              <tr><td  style="font-size:10px!important;">beginning on:  <asp:Label runat="server" ID="lblPrimerPago_Cash"></asp:Label> </td></tr>
                              <tr><td  style="font-size:10px!important;">Last payment on:<asp:Label runat="server" ID="lblFechaVencimiento_Cash"></asp:Label> </td></tr>
                              <tr><td style="font-size:10px!important;">Frecuency:<asp:Label runat="server" ID="lblFrecuanciaContrato"></asp:Label></td></tr>  
                            </table>
                          </div>
                        </div>
                        <div class="descriptionCash">
                          <p style="font-size:11px!important;">
                           See the Disclosure Statement, Note and Security Agreement for any additional
                            information about nonpayment, default, any required repayment in full before
                            the scheduled date, and prepayment refunds and penalties.
                          </p>
                        </div>
                      </div>
                      <div  style="width :50% !important" class="rightCash">
                        <div class="rowCash">
                          <p  style=" font-size:11px!important;">Security: Borrower is giving a security intereset in:</p>
                          <div>
                            <div class="controlsCash space-bottom">
                              <div class="controlCash">
                                <input type="checkbox" class="checkbox-controlCash" checked />
                                <label for=""  style=" font-size:10px!important;">Motor Vehicle</label>
                              </div>
                              <div class="controlCash">
                                <input type="checkbox" class="checkbox-controlCash"/>
                                <label for=""  style=" font-size:10px!important;">Motor Vehicle</label>
                              </div>
                            </div>
                          <p  style=" font-size:11px!important;">

                             <b>Late Charge:</b> If a payment is more than 10 days late,
                                        Borrower will be required to pay a late charge of $15.00            
                            </p>
                          </div>
                        </div>
                        <div class="rowCash">
                          <p  style=" font-size:11px!important;">Prepayment if borrower pays off early, Borrower:</p>
                          <div class="halfCash">
                            <div class="controlsCash wrap">
                              <div class="controlCash size-two">
                                <input type="checkbox" class="checkbox-controlCash" checked />
                                <label for="" style=" font-size:10px!important;">Willnot</label>
                              </div>
                              <div class="controlCash size-two">
                                <input type="checkbox" class="checkbox-controlCash" /> 
                                <label for="" style=" font-size:10px!important;">may</label>
                              </div>
                              <div class="controlCash size-two">
                                <input type="checkbox" class="checkbox-controlCash"  />
                                <label for="" style=" font-size:10px!important;">Willnot</label>
                              </div>
                              <div class="controlCash size-two">
                                <input type="checkbox" class="checkbox-controlCash" checked />
                                <label for="" style=" font-size:10px!important;">may</label>
                              </div>
                            </div>
                            <div>
                              <p  style=" font-size:10px!important;" >
                               have to pay a penalty and be entitle to a refund of party of the finance charge
                              </p>
                            </div>
                          </div>
                            <div class="rowCash">
                          <p style=" font-size:10px!important;" >Demand Feature:</p>
                        <div class="half">
                          <div class="controlsCash wrap">
                            <div class="controlCash size-two">
                              <input type="checkbox" class="checkbox-controlCash" checked />
                              <label for=""  style=" font-size:10px!important;" >Not applicate</label>
                            </div>
                            <div class="controlCash size-two">
                              <input type="checkbox" class="checkbox-controlCash" />
                              <label for=""  style=" font-size:10px!important;" >This obligation has a demand feature</label>
                            </div>
                            
                          </div>
        
                        </div>
                       </div>
                        </div>
                      </div>
                    </div>
                   </div>
               <br/>

            <%--  Tabla 3--%>
            <table style="width:100% ; border:1px solid black; font-size:12px!important;  " >

        <tr style="height: 50; border: 1px solid black" >
                <td>ITEMIZATION OF AMOUNT FINANCED</td>
                <td><b><asp:Label runat="server" ID="lblPlazoFinanciarTabla3_Cash"></asp:Label></b></td>                 
        </tr>
        <tr style="border: 1px solid black">
                <td><asp:Label runat="server" ID="lblTasaInteres_CahsTable4"></asp:Label></td>
                <td>Agreed rate of interest</td>                          
    
           </tr>
           <tr style="border: 1px solid black">
                <td>$0.00</td>
                <td>Amount given to borrower directly (check to borrower)</td>                    
           </tr>
           <tr  style="border: 1px solid black">
                <td><asp:Label runat="server" ID="lblCollatelaTabla3_Cash"></asp:Label></td>
                <td>Collateral Protection plan(If apply)</td>                    
           </tr>
           <tr  style="border: 1px solid black">
                <td>$ <asp:Label runat="server" ID="lblLienHolher_Cash"></asp:Label></td>
                <td>License/Title/ Registraction Fee / Other</td>                    
           </tr>
           <tr  style="border: 1px solid black">
                <td> <asp:Label runat="server" ID="lblInteresesTotalTabla2_Cash"></asp:Label></td> 
                <td>Finance Charge</td>                    
           </tr>
           <tr  style="border: 1px solid black">
                <td></td>
                <td>Check number</td>                    
           </tr>
   </table>
        
          <div class="row">
             <div class="col-12">                      
               <p style="font-size:12px!important;">                        
                  <u> TERMS:</u> In this Disclosure Statement, Note and Security Agreement, the word "Borrower" refers to the persons signing below as Borrower, whether one or
                   more. If more than one Borrower signs, each will be responsible, individually and together, for all promises made and for repaying the loan in full.
                   The word "Lender" refers to the Lender, whose name and address are shown above.
             
                </p>
               <p style="font-size:12px!important;"  > 
                  <u>PROMISE TO PAY:</u> In return for a loan that Borrower has received, Borrower promises to pay to the order of Lender the Total of Payments (which includes  interest charges) shown  above  in  substantially equal,  
                   consecutive  monthly installments as  shown  above  except that any appropriate adjustments will be made to the first and final payments, until the Total of Payments is fully paid. Each payment will be applied to any allowable expenses late charges and/or attorney fees/expenses and then to a combined total of the Amount Financed and interest charges. 
                   If any part of the balance remains unpaid on the final payment date, Lender, at its option, may collect interest 
                   from and after maturity upon the then unpaid Amount Financed at the maximum rate permitted by the then applicable law or, if no maximum rate is prescribed, then the rate of interest prevailing under this note. Any amount shown above as an Origination Fee has been 
                   paid by the Borrower and is considered a prepaid charge. If Borrower prepays this Note, the Origination Fee is not refundable.

                </p>
              
          </div>

              <div class="row">
                   <div class="col-12">
                  
                       <p style="font-size:13px!important;">
                          <u>PREPAYMENT:</u> 
                       </p>
                       <table>
                           <tr>
                               <td> <input type="checkbox" name="vehicle1" value="Bike"  style="width:25px;height:25px;"  checked></td>
                               <td>
                                    <ul>
                           <li>A.&nbsp;&nbsp;If this box is checked, you may prepay this loan in whole or in part at any time without penalty. However, upon partial prepayment, 
                               interest will continue to accrue. Partial prepayment and the application of a refund to the unpaid balance of the loan will not affect the 
                               amount or due date of subsequent scheduled payments on the loan, but may reduce the number of such payments. Upon prepayment in 
                               full, Borrower may receive a credit or rebate of the unearned Finance Charges according to Cuota Fija Niv / Calculo de Ints. al Inicio del 
                               Plan. Refunds of less than $1.00 will not be made. In the event of any prepayment of a loan whether or not pre-computed, the credit 
                               may collect the minimum loan Finance Charge of $45, as if earned.
                           </li>
                                        </ul>
                               </td>
                           </tr>
                           <tr>
                               <td> <input type="checkbox" name="vehicle1" value="Bike"  style="width:25px;height:25px;"></td>
                               <td><ul>
                                        <li>B.&nbsp;&nbsp;If this box is checked and this loan is primarily secured by an interest in land, Borrower will be required to pay a prepayment penalty of 
                                           two
                                          percent (2%) of the net unpaid balance (after deducting all refunds and rebates) if this loan is prepaid in full during the first three years 
                                          of the loan. A prepayment penalty will not apply if Borrower refinances or consolidates this loan with Lender or the loan is prepaid by        
                                         insurance
                                         proceeds or after acceleration after default. 
                           </li>                              
                          
                       </ul>
                               </td>
                           </tr>
                       </table>
                        
                      
                      
                       <p style="font-size:12px!important;">
                         <u> SECURITY AGREEMENT:</u>
                       </p>
                       <table>
                           <tr>
                               <td> <input type="checkbox" name="vehicle1" value="Bike"  style="width:25px;height:25px;"> </td>
                               <td>                                        
                                   <ul>
                                    <li>A.&nbsp;&nbsp; If this box is checked, this loan is unsecured.</li>
                                   </ul>
                               </td>                            
                          </tr>
                           <tr>
                               <td> <input type="checkbox" name="vehicle1" value="Bike"  style="width:25px;height:25px;"  checked> </td>
                               <td> <ul>
                                      <li>
                                            B.&nbsp;&nbsp; If this box is checked, to secure the payment and performance hereof, Borrower gives to Lender a security interest under the Uniform 
                                             Commercial Code in any property for which a description is completed below and all parts and equipment now or later added to the 
                                             property and any proceeds of the property, all of which will be called "Property". See below for additional terms applicable to this 
                                             security interest.

                                       </li>
                                    </ul>
                               </td>
                        
                        </tr>
                      
                    </table>
                   </div>
               </div>
               <div class="row">
                    <ol>
                        <li  value="1">
                            Motor vehicle/mobile Home;
                        </li>
                    </ol>
                <div class="col-12">
                       <table class="table table-sm table-bordered">
                           <tr>
                             <th>Make, No Cylinders</th>
                             <th>Year/Model</th>
                             <th>Model No.Or Name</th>
                             <th>Body Type</th>
                             <th>Identification Number</th>
                           </tr>
                           <tr>                                  
                               <td>
                                   <asp:Label runat="server" ID="lblMarca_Cash"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label runat="server" ID="LblAnio_Cash"></asp:Label>,
                               </td>
                                <td>
                                   <asp:Label runat="server" ID="lblModelo_Cash"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label runat="server" ID="Label4"></asp:Label>
                               </td>
                                <td>
                                   <asp:Label runat="server" ID="lblSerie_Cash"></asp:Label>
                               </td>
                           </tr>
                        
                       </table>
                    <ol>
                        <li value="2">
                            Other Property;
                        </li>
                    </ol>
                    <br />
                    <div class="Idletra" >
                   <table>
                       <tr>
                           <td>  <input type="checkbox" name="vehicle1" value="Bike"  style="width:25px;height:25px;"  checked>  </td>
                           <td>
                             <ul>
                        
                                <li >
                                   C.&nbsp;&nbsp;If this box is checked, Borrower's loan is secured by Deed of Trust or Mortgage of even date on real property which requires Lender's X
                                   written consent to a sale or transfer of the encumbered real property

                                </li>
                           </ul>
                           </td>
                       </tr>
                   </table>
                      <br />  <br />  <br />
                    <p style="font-size:12px!important;">
                         <u>OWNERSHIP OF PROPERTY:</u> Borrower represents that the Property is owned by Borrower free and clear of all liens and encumbrances except those of which Borrower 
                        has informed Lender in writing. Prior to any default, Borrower may keep and use the Property at Borrower's own risk, subject to the provisions of the Uniform 
                        Commercial Code. If the Property includes a motor vehicle or mobile home, Borrower will, upon request, deliver the certificate of title to the motor vehicle or 
                        mobile home to Lender.
                    </p>
                    
                    <p  style="font-size:12px!important;">
                        <u>USE OF PROPERTY:</u> Borrower will not sell, lease, encumber, or otherwise dispose of the Property without Lender's prior written consent. 
                        Borrower will keep the Property at the Borrower's 
                        address (as shown on page 1) unless Lender has granted permission in writing for the Property to be
                    </p>
                    <p style="font-size:12px!important;">
                         located elsewhere. The Property will be used only in the state in which Borrower lives unless Property is a motor vehicle, in which case it will be used outside the state
                        only in the course of Borrower's normal use of the Property. Borrower will not use or permit the use of the Property to hire or for illegal purposes.
                    </p>
                    <p style="font-size:12px!important;">
                       <u> TAXES AND FEES:</u> Borrower will pay all taxes, assessments, and other fees payable on the Property, this Disclosure Statement, Note and Security
                                Agreement, or the loan, including but not limited to any fee required by a public official to record the satisfaction of this loan, and/or the release of Lender's 
                                interest in the Property. If Borrower fails to pay S\.lCh amounts, Lender may pay such amounts for Borrower and the amounts paid by Lender will be 
                                added to the unpaid balance of the loan.
                    </p>
                 
                    <p style="font-size:12px!important;">
                       <u> INSURANCE:</u> If Borrower purchases any insurance at Lender's office, Borrower understand and acknowledges that (1) the insurance company may be affiliated with Lender, 
                        employee(s) may be an agent for the insurance company, (3) such employees(s) is not acting as the agent, broker or fiduciary for Borrower on this loan, but may be the agent of the insurance company, 
                        and (4) Lender or the insurance company may realize some benefits from the sale of that insurance. If Borrower fails to obtain or maintain any required insurance or fails to designate an agent through 
                        whom the insurance is to be obtained, Lender may purchase such required insurance for Borrower through an agent of Lender's choice, and the amounts paid by Lender will be added to the unpaid balance of the loan.
                    </p>
                    
                    <p style="font-size:12px!important;">
                       <u> FINANCING STATEMENTS:</u>   Borrower will sign all financing statements, continuation statements, security interest filing statements, and similar documents with respect to the Property at Lender's request.
                    </p>
                    
                    <p style="font-size:12px!important;">
                       <u> RETURNED CHECK FEE:</u> Lender may charge a fee of $ 20.00 if a check, negotiable order of withdrawal or share draft is returned for insufficient funds or insufficient credit.
                    </p>
                    
                    <p style="font-size:12px!important;">
                          <u> DEFERMENTS:</u> Borrower agrees to pay a deferment charge equal to interest at the agreed upon rate on the unpaid balance for one month if Lender, in its sole discretion allows Borrower to defer payment of any monthly 
                        installment prior to or on its due date without the account being considered in default. Any such deferment will extend the final payment date by one month. All other terms and conditions of the Disclosure Statement, 
                        Note and Security Agreement including the expiration dates of insurance coverage, remain unchanged by authorized deferments
                    </p>
                    
                    <p style="font-size:12px!important;">
                      <u> LOAN CHARGES:</u> If a law that applies to this loan and that sets maximum loan charges, is finally interpreted so that the interest or other loan charges collected or to be collected in connection with this loan exceed the permitted limits, 
                        then (i) any such loan charge will be reduced by the amount necessary to reduce the charge to the permitted limit, and (ii) any sums already collected from Borrower that exceeded permitted limits will be refunded to Borrower. Lender may 
                        choose to make this refund by reducing the principal owed under this loan or by making a direct payment to Borrower. If a refund reduces principal, the reduction will be treated as a partial prepayment without any prepayment charge.
                    </p>
                    <p style="font-size:12px!important;"  >
                        DEFAULT: Borrower will be in default if:
                    </p><br /> <br />
                    <ol style = "font-size:11px!important;">
                        <li>
                            Borrower does not make any scheduled payment on time;
                        </li>
                        <li>
                            Borrower is (or any other person puts Borrower) in bankruptcy, insolvency or receivership;
                        </li>
                        <li>
                            Any of Borrower's creditors attempts by legal process to take and keep any property of Borrower, including the Property securing this loan; 
                        </li>
                        <li>
                             Borrower fails to fulfill any promise made under this agreement; or
                        </li>
                        <li>
                                A default occurs under any Real Estate Mortgage or Deed of Trust which secures this loan or under any other mortgage or deed of trust on the real property.
                        </li>
                    </ol>
                
                    <p style="font-size:12px!important;">
                        Subject to Borrower's right to any notice of default, right to cure default, and any other applicable laws, if Borrower defaults, Lender may require Borrower to repay the unpaid balance of the Total of Payments plus any allowable expenses, 
                        late charges, and/or attorney fees/expenses, minus rebates of any unearned finance charges and/or refunds of unearned insurance premiums. Lender's failure to exercise or delay in exercising any of its rights when default occurs does not constitute a waiver of those or any other rights under this agreement, or the Mortgage, Deed of Trust or Deed to Secure Debt. 
                        In the case of default Lender may exercise any or all of its rights under the Mortgage, Deed of Trust or Deed to Secure Debt. If this debt is referred for collection to an attorney, Lender shall be entitled to collect all reasonable costs and expenses of collection, including, but not limited to, court costs and reasonable attorney's fees as permitted by law.
                    </p>
                    <p style="font-size:12px!important;">
                        EFFECTS OF DEFAULT: If Borrower defaults, Borrower will deliver the Property to Lender or, upon Lender's demand, assemble the Property and make it available to Lender at a reasonably convenient place. Lender may, without previous notice or demand and without legal process, peacefully enter any place where the Property is located and take possession of it. The Property may
                        be sold with notice at a private or public sale at a location chosen by Lender. At such public sale, Under may purchase the Property. The proceeds of the sale, minus the expenses of taking, removing, holding, repairing, and selling the Property including reasonable attorney's fees, and minus the cost of paying off and removing any superior liens or claims on the Property,
                        will be credited to the unpaid balance of Borrower's loan. If Borrower has left other property in the repossessed Property, Lender may hold such property temporarily for Borrower without any responsibility or liability for the property.
                    </p>
                    <p style="font-size:12px!important;">
                        Notice of the time and place of a public sale or notice of the time after which a private sale will occur is reasonable if mailed to the Borrower's address at least five days before the sale. The notice may be mailed to the Borrower's last address shown on Lender's records.
                    </p>
                    <p style="font-size:12px!important;">
                        LAW THAT APPLIES: Florida Consumer Credit Code and federal law, as applicable, govern this agreement. In no event will Borrower be required to pay interest or charges in excess of those permitted by law.
                    </p>
                    <p style="font-size:12px!important;">
                        OTHER RIGHTS: Lender may accept payments after maturity or after a default without waiving its rights with respect to any subsequent default in payment. Borrower agrees that Lender may extend time for payment after maturity without notice. The terms of this agreement can be waived or changed only in a writing signed by Lender.
                    </p>
                    <p style="font-size:12px!important;">
                        Borrower, endorsers, sureties and guarantors, to the extent permitted by law, severally waive their right to require Lender to demand payment of amounts due, to give notice of amounts that have not been paid, to receive notice of any extensions of time to pay which Lender allows to any Borrower and to require Lender to show particular diligence in bringing suit against anyone responsible for repayment of this loan, and additionally, waive benefit of homestead and exemption laws now in force or later enacted, including stay of execution and condemnation, on any property securing this loan and waive the benefit of valuation and appraisement.
                    </p>
                   
                    <p style="font-size:12px!important;">
                        If any part of the Disclosure Statement, Note and Security Agreement and, if applicable, the Mortgage or Deed of Trust and accompanying Itemization of Amount Financed and Arbitration Agreement is unenforceable, this will not make any other part unenforceable. Where the context requires, singular words may be read in the plural and plural words in the singular, and references to the masculine gender may be read to apply to the feminine gender.
                    </p>
                    <p style="font-size:12px!important;">
                      REFINANCING: The overall cost of refinancing an existing loan balance may be greater than the cost of keeping the existing loan and obtaining a second loan for any additional funds Borrower wishes to borrow.
                    </p>
                    <p style="font-size:13px!important;">
                        AUTHORIZATION TO USE CREDIT REPORT: By signing below, Borrower authorizes Lender to obtain, review and use information contained in the Borrower's credit report in order to determine whether the Borrower may qualify for products and services offered by Lender. This authorization terminates when Borrower's outstanding balance due under this Disclosure Statement, Note and Security Agreement is paid in full. Borrower may cancel such authorization at any time by writing the following: Crediflash LLC, 17880 S. Dixie Hwy, Miami FI 33157. In order to process Borrower's request, Lender must be provided Borrower's full name, address, social security number and loan number.
                    </p>
                    
                        <table>
                            <tr> <td style="width:80%;">  <p style="font-size:13px!important;">The following notice applies only if this box is checked.</p> </td>
                                <td>  <input type="checkbox" name="vehicle1" value="Bike"  style="width:25px;height:25px;">   </td>
                            </tr>
                        </table>
              

              <table style="width:100% ; border:1px solid black; font-size:12px!important;  " >
                  <tr style="border: 1px solid black" >
                      <td>
                    <p style="font-size:13px!important;"> <b> NOTICE </b></p>
                    <p style="font-size:13px!important;"><b> 
                        ANY HOLDER OF THIS CONSUMER CREDIT CONTRACT IS SUBJET TO ALL CLAIMS AND DEFENSES WHICH THE DEBTOR COULD ASSERT AGAINST THE SELLER OF GOODS OR SERVICES OBTAINED WITH THE PROCEEDS HEREOF. RECOVERY HEREUNDER BY THE DEBTOR SH NOT EXCEED AMOUNTS PAID BY THE DEBTOR HEREUNDER.
                  </b></p>
                    </td>
                 </tr>
              </table>

                   </div>
                    </div>
                 </div>
              <div>
               
              </div>

               </div>
           <br/>
               <div class="row mt-2">
                   <div class="col-1"></div>
                   <div class="col-5 text-center">
                       <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                       <asp:Label class="mt-0 d-block" runat="server" ID="Label42"></asp:Label>
                       <asp:Label class="mt-0 d-block" runat="server" ID="Label43"></asp:Label>
                     
                     
                   </div>
                   <div class="col-5 text-center">
                       <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                       <label class="mt-0 d-block lblNombreRepresentanteLegal"></label>
                       <label class="mt-0 d-block lblIdentidadRepresentanteLegal"></label>
                       <label class="mt-0 d-block">Borrower's Name</label>
                   </div>
                   <div class="col-1"></div>

                 <div class="col-1"></div>
                   <div class="col-5 text-center">
                       <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                       <asp:Label class="mt-0 d-block" runat="server" ID="Label6"></asp:Label>
                       <asp:Label class="mt-0 d-block" runat="server" ID="Label7"></asp:Label>
                     
                     
                   </div>
                   <div class="col-5 text-center">
                       <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                       <label class="mt-0 d-block lblNombreRepresentanteLegal"></label>
                       <label class="mt-0 d-block lblIdentidadRepresentanteLegal"></label>
                       <label class="mt-0 d-block">Borrower's Signature</label>
                   </div>
                   <div class="col-1"></div>
                  <%-- ///--%>
                 <div class="col-1"></div>
                   <div class="col-5 text-center">
                       <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                       <asp:Label class="mt-0 d-block" runat="server" ID="Label8"></asp:Label>
                       <asp:Label class="mt-0 d-block" runat="server" ID="Label9"></asp:Label>
                     
                     
                   </div>
                   <div class="col-5 text-center">
                       <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                       <label class="mt-0 d-block lblNombreRepresentanteLegal"></label>
                       <label class="mt-0 d-block lblIdentidadRepresentanteLegal"></label>
                       <label class="mt-0 d-block">Borrower's Name</label>
                   </div>
                   <div class="col-1"></div>
                   <div class="col-1"></div>
                   <div class="col-5 text-center">
                       <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>                           
                     
                     
                   </div>
                   <div class="col-5 text-center">
                       <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>                           
                       <label class="mt-0 d-block">Borrower's Signature</label>
                   </div>
                   <div class="col-1"></div>
                  <%-- //--%>
                 <div class="col-1"></div>
                   <div class="col-5 text-center">
                       <label></label>
                       <asp:Label class="mt-0 d-block" runat="server" ID="Label12"></asp:Label>
                       <asp:Label class="mt-0 d-block" runat="server" ID="Label13"></asp:Label>
                     
                     
                   </div>
                   <div class="col-5 text-center">
                       <asp:Label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" ForeColor="Black" Style="border-radius: 0px;" runat="server" ID="Crediflash">Crediflash LLC</asp:Label>
                                              
                   </div>
                   <div class="col-1"></div>
                   <div class="col-1"></div>
                   <div class="col-5 text-center">
                       <label ></label>                                                
                     
                   </div>
                   <div class="col-5 ">
                       <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;">By:</label>
                      
                      <%-- <label class="mt-0 d-block">CESAR ROSENTHAL</label>--%>
                        <%-- <label class="mt-0 d-block">President</label>--%>
                     <asp:Label runat="server" ID="FechaFirmaContratoCash"></asp:Label>
                            <label class="mt-0 d-block">President</label>
                            
                    
                      
                   </div>
                   <div class="col-1"></div>
               </div>
       
               <br /> <br /> <br />  <br />  <br />
               <div class:"row">
                   <div class="col-12">
                       <p style="font-size:12px!important;">
                           SECURITY INTEREST OF NONOBLIGOR: Borrower only is personally liable for payment of the loan. Nonobligor is liable and bound by all other terms, 
                           conditions, covenants, and agreements contained in this Disclosure Statement, Note and Security Agreement, including but not limited to the 
                           right and power of Lender to repossess and sell the Property securing this loan, in the event of default by Borrower in payment of this loan.

                       </p>
                   </div>
               </div>
                 <div class="row justify-content-center">
                   <div class="col-5 text-center mt-1">
                       <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                       <asp:Label runat="server" ID="LblNombreCLienteFirma_Cash"></asp:Label><br />
                       <asp:Label runat="server" ID="lblFirmaFechaContrato"></asp:Label>

                   </div>
                   <div class="col-5 text-center mt-1">
                       <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>                          
                       <label class="mt-0 d-block ">CESAR ROSENTHAL</label>
                        <label class="mt-0 d-block">President</label>
                       <asp:Label runat="server" ID="lblFirmaContrato2"></asp:Label>
                   </div>
               </div>
             
           </div>
       </div>
   </div>

    </form>
  

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tilesgrid/ug-theme-tilesgrid.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tiles/ug-theme-tiles.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/html2pdf/html2pdf.bundle.js"></script>
    <script src="/Scripts/plugins/qrcode/qrcode.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script>
        const FONDOS_PRESTAMO = <%=this.FondoPrestamoJSON %>;
        const ID_SOLICITUD_CREDITO = '<%=pcIDSolicitud%>';

        const DEPARTAMENTO_FIRMA = '<%=DepartamentoFirma%>';
        const CIUDAD_FIRMA = '<%=CiudadFirma%>';
        const DIAS_FIRMA = '<%=DiasFirma%>';
        const MES_FIRMA = '<%=MesFirma%>';
        const ANIO_FIRMA = '<%=AnioFirma%>';
        const IdProducto = '<%=IdProducto %>';

        const NombreCliente = '<%=NombreCliente %>';
        const DireccionCliente = '<%=DireccionCliente %>';

        const URL_CODIGO_QR = '<%=UrlCodigoQR%>';
        var LISTA_DOCUMENTOS_EXPEDIENTES = JSON.parse('<%=ListaDocumentosDelExpedienteJSON%>');
    </script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_ImprimirDocumentacion.js?v=20210323105125"></script>    
    <script src="/Scripts/plugins/kendo/jszip.min.js"></script>
    <script src="/Scripts/plugins/kendo/kendo.all.min.js"></script>
    <script src="/Scripts/plugins/kendo/PrintHtmlToPDF.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.2/jspdf.min.js"></script>
       <script src="https://kendo.cdn.telerik.com/2017.3.913/js/pako_deflate.min.js"></script>
   
        <script type="x/kendo-template" id="page-template">
            <div class="page-template">
                <div class="header">
                   
                </div>
                <div class="footer">
                <%--    Page #: pageNum # of #: totalPages #--%>
                </div>
            </div>
        </script>

   <style>

        /* Page Template for the exported PDF */
    .page-template {
        font-family: "DejaVu Sans", "Arial", sans-serif;
        position: absolute;
        width: 100%;
        height: 100%;
        top: 0;
        left: 0;
    }

        .page-template .header {
            position: absolute;
            top: 20px;
            left: 30px;
            right: 30px;
            border-bottom: 1px solid #888;
            color: #888;
            margin-bottom: 50px;
        }

        .page-template .footer {
            position: absolute;
            bottom: 30px;
            left: 30px;
            right: 30px;
            border-top: 1px solid #888;
            text-align: center;
            color: #888;
        }

        .page-template .watermark {
            font-weight: bold;
            font-size: 400%;
            text-align: center;
            margin-top: 30%;
            color: #aaaaaa;
            opacity: 0.1;
            transform: rotate(-35deg) scale(1.7, 1.5);
        }
       /*           */
      p {
        padding: 10px;
      }

      .flexCash {
        display: flex;
       
      }

      .containerCash {
        border: 2px solid black;
        box-sizing: border-box;
      
      }

      .leftCash {
        min-width: 60%;
        border-right: 2px solid black;
      }

      .columnCash {
        width: 33.33%;
        border-right: 2px solid black;
        border-top: 2px solid black;
        min-height: 100px;
      }

      .columnCash:last-child {
        border-right: none;
      }

      .rowCash {
        min-height: 45%;
      }

      .descriptionCash {
        min-height: 250px;
        border-top: 2px solid black;
      }

      .description p {
        max-width: 350px;
      }

      .rightCash {
        min-width: 40%;
      }

      .rightCash > div:first-child {
        border-bottom: 2px solid black !important;
      }

      .controlsCash {
        display: flex;
        justify-content: center;
      }

      .space-bottom {
        margin-bottom: 20px;
      }

      .controlsCash > div {
        display: flex;
      }

      .checkbox-controlCash {
        display: inline-block;
        -webkit-appearance: initial;
        appearance: initial;
        border: 2px solid black;
        background-color: white;
        width: 20px;
        height: 20px;
        position: relative;
      }

      .checkbox-controlCash:checked::after {
        content: "X";
        position: absolute;
        left: 50%;
        top: 50%;
        -webkit-transform: translate(-50%, -50%);
        -moz-transform: translate(-50%, -50%);
        -ms-transform: translate(-50%, -50%);
        transform: translate(-50%, -50%);
      }

      .controlCash {
        align-items: center;
        width: 30%;
        justify-content: center;
      }

      .wrap {
        flex-wrap: wrap;
      }

      .halfCash {
        display: flex;
      }

      .halfCash > div {
        width: 50%;
      }

      .size-two {
        width: 50%;
      }
    </style>

   
</body>
    
</html>
