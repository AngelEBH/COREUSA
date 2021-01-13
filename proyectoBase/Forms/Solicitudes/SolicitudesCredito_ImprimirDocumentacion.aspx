<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_ImprimirDocumentacion.aspx.cs" Inherits="SolicitudesCredito_ImprimirDocumentacion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Imprimir documentacion</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/css/unitegallery.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.css" rel="stylesheet" />
    <link href="/CSS/SolicitudesCredito_ImprimirDocumentacion.css?v=20210106150602" rel="stylesheet" />
</head>
<body class="EstiloBody">
    <form id="frmGuardarPreSolicitud" runat="server">
        <div class="card m-0">
            <div class="card-header pb-1 pt-1">
                <div class="float-right p-1 qrCode" id="Loader" style="display: none;">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>
                <h5>Imprimir documentación solicitud de crédito No. <span id="lblIdSolicitud" class="font-weight-bold" runat="server"></span></h5>
            </div>
            <div class="card-body pt-0">
                <div class="row mb-0">
                    <div class="col-md-6">
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <label class="col-form-label">Cliente</label>
                                <asp:TextBox ID="txtNombreCliente" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6">
                                <label class="col-form-label">Identidad</label>
                                <asp:TextBox ID="txtIdentidadCliente" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6">
                                <label class="col-form-label">RTN numérico</label>
                                <asp:TextBox ID="txtRtn" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
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
                        <h6 class="">Imprimir documentos</h6>
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <button type="button" id="btnContrato" onclick="ExportToPDF('CONTRATO','divContenedorContrato','divContratoPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Contrato
                                </button>
                                <button type="button" id="btnPagare" onclick="ExportToPDF('PAGARE','divContenedorPagare','divPagarePDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Pagaré
                                </button>
                                <button type="button" id="btnCompromisoLegal" onclick="ExportToPDF('COMPROMISO_LEGAL','divContenedorCompromisoLegal','divCompromisoLegalPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Compromiso legal
                                </button>
                                <button type="button" id="btnConvenioComprayVenta" onclick="ExportToPDF('CONVENIO_DE_COMPRA_Y_VENTA_DE_VEHICULOS_PARA_FINANCIAMIENTO','divContenedorConvenioCyV','divConevionCyVPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    C. y V. de vehic. finan.
                                </button>
                                <a href="/Documentos/Recursos/INSPECCION%20DE%20VEHICULO.pdf" download="Inspección vehículo" id="btnInspeccionVehiculo" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/inspection_40px.png');">Inspección de vehículo
                                </a>
                                <button type="button" id="btnInspeccionSeguroDeVehiculo" onclick="ExportToPDF('INSPECCION_SEGURO_VEHICULO','divContenedorInspeccionSeguro','divInspeccionSeguroPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/inspection_40px.png');">
                                    Inspección seguro
                                </button>
                                <button type="button" id="btnTraspaso" onclick="ExportToPDF('TRASPASO_CLIENTE','divContenedorTraspaso','divTraspasoPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/resume_40px.png');">
                                    Traspaso cliente
                                </button>
                                <button type="button" id="btnTraspasoVendedor" onclick="ExportToPDF('TRASPASO_PROPIETARIO','divContenedorTraspasoVendedor','divTraspasoVendedorPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/resume_40px.png');">
                                    Traspaso propietario
                                </button>
                                <button type="button" id="btnRecibo" onclick="ExportToPDF('RECIBO','divContenedorRecibo','divReciboPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/receipt_40px.png');">
                                    Recibo
                                </button>
                                <button type="button" id="btnBasicoCPI" onclick="ExportToPDF('BASICO_CPI','divContenedorBasicoCPI','divBasicoCPIPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/vehicle_insurance_40px.png');">
                                    Básico + CPI
                                </button>
                                <button type="button" id="btnNotaDeEntrega" onclick="ExportToPDF('NOTA_DE_ENTREGA', 'divContenedorNotaDeEntrega', 'divNotaDeEntregaPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/resume_40px.png');">
                                    Nota de entrega
                                </button>
                                <button type="button" id="btnEnviarCorreoLiquidacion" onclick="EnviarCorreo('Liquidación', 'Liquidación', 'divCorreoLiquidacionPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/send_email_40px.png');">
                                    Correo Liquidación
                                </button>
                                <button type="button" id="btnEnviarCorreoSeguro" onclick="EnviarCorreo('Seguro', 'Seguro de garantía', 'divCorreoSeguroPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/send_email_40px.png');">
                                    Correo Seguro
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
                            <div class="col-6">
                                <label class="col-form-label">Tipo de garantía</label>
                                <asp:TextBox ID="txtTipoDeGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Tipo de vehículo</label>
                                <asp:TextBox ID="txtTipoDeVehiculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">Marca</label>
                                <asp:TextBox ID="txtMarca" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">Modelo</label>
                                <asp:TextBox ID="txtModelo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">Año</label>
                                <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">Color</label>
                                <asp:TextBox ID="txtColor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">Matrícula</label>
                                <asp:TextBox ID="txtMatricula" CssClass="form-control form-control-sm" ReadOnly="true" type="text" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">Serie Motor</label>
                                <asp:TextBox ID="txtSerieMotor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">Serie Chasis</label>
                                <asp:TextBox ID="txtSerieChasis" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">GPS</label>
                                <asp:TextBox ID="txtGPS" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <h6 class="m-0 pt-2">Características mecánicas</h6>
                        <div class="form-group row">
                            <div class="col-sm-4">
                                <label class="col-form-label">Cilindraje</label>
                                <asp:TextBox ID="txtCilindraje" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Recorrido</label>
                                <asp:TextBox ID="txtRecorrido" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Transmisión</label>
                                <asp:TextBox ID="txtTransmision" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Tipo de combustible</label>
                                <asp:TextBox ID="txtTipoDeCombustible" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Serie 1</label>
                                <asp:TextBox ID="txtSerieUno" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
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
                        <h6 class="">Fotografías de la garantía</h6>
                        <div class="form-group row">
                            <div class="col-12">
                                <!-- Div donde se muestran las imágenes de la garantía-->
                                <div class="align-self-center" id="divGaleriaGarantia" runat="server" style="display: none;">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row mr-0 ml-0 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                    <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
                </div>
            </div>
        </div>

        <!-- ********** DOCUMENTOS ********** -->

        <!-- Contrato -->
        <div id="divContenedorContrato" style="margin-top: 999px; display: none;">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divContratoPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="/Imagenes/LogoPrestadito.png" />
                        </div>
                        <div class="col-12">
                            <h6 class="text-center font-weight-bold">CONTRATO DE CRÉDITO PARA COMPRA DE VEHÍCULO</h6>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                Nostros, <b>ERICK GEOVANY MOLINA PADILLA</b>,
Casado, Ingeniero Industrial, con domicilio en la ciudad de San Pedro Sula, Departamento de Cortés, quien actúan en su condición de Representante Legal de la Sociedad Mercantil denominada
                                <b>PRESTADITO S.A. de C.V.</b> empresa domiciliada en la ciudad de San Pedro Sula, departamento de Cortes, llamada en adelante
                                <b>PRESTADITO</b> o <b>PRESTAMISTA</b>; y por otra parte el Sr(a)
                                <asp:Label runat="server" ID="lblNombre_Contrato" class="font-weight-bold"></asp:Label>,
mayor de edad, de nacionalidad
                                <asp:Label runat="server" ID="lblNacionalidad_Contrato" class="font-weight-bold"></asp:Label>
                                y de este domicilio,
con identidad No.
                                <asp:Label runat="server" ID="lblIdentidad_Contrato" class="font-weight-bold"></asp:Label>
                                con domicilio y dirección en
                                <asp:Label runat="server" ID="lblDireccion_Contrato"></asp:Label>,
llamado en adelante <b>EL CIENTE, PRESTATARIO y/o DEUDOR</b>, convienen celebrar el siguiente <b>CONTRATO DE CRÉDITO PARA COMPRA DE VEHICULO</b>
                                y acuerdan lo estipulado en las siguientes clausulas:
                                <b>PRIMERO: OBJETO DEL CONTRATO.- EL CLIENTE </b>declara recibir en este acto de <b>PRESTADITO</b>, un préstamo por la cantidad de
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
                                    <asp:Label runat="server" ID="lblSerieChasis_Contato"></asp:Label>
                                </b>
                            </p>
                            <p>
                                Mismo que será desembolsado por <b>PRESTADITO</b> a la persona que distribuya o sea propietario del vehículo y este ultimo deberá de entregárselo al Cliente cuando sea autorizado por <b>PRESTADITO</b>.

                                <b>- B) TASAS DE INTERES.-COSTO ANUAL TOTAL.- EL CLIENTE</b> se obliga a pagar a <b>PRESTADITO</b>, a partir de esta fecha una
tasa de interés simple del <b>
    <asp:Label runat="server" ID="lblTasaInteresSimpleMensual_Contrato">1.67</asp:Label>%
PORCIENTO MENSUAL </b>, amartizando capital mas intereses basada en la regla del 78 misma que sera pagadero
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
                                <b>Anticipo por concepto de prima de:
                                    <asp:Label runat="server" ID="lblMontoPrima_Contrato"></asp:Label>
                                    y plazo de
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
                                <b>- III)</b> El pago se realizará conforme a lo establecido en el plan de pagos, en el caso que la fecha de pago sea día feriado, entonces deberá realizarse el día hábil inmediato anterior, en las oficinas, agencias, sucursales y ventanillas de <b>PRESTADITO</b>, o en cualquier otra institución tercerizada que se designe oportunamente en virtud de convenios de cobro de cartera.
                                <b>- E) PAGO ANTICIPADO:</b>
                                En caso de pago total de la obligación antes de su vencimiento, <b>EL CLIENTE</b> deberá pagar una comisión de prepago del dos por ciento (2%) sobre el saldo adeudado, y si es un pago parcial a capital superior al diez por ciento (10%) del monto adeudado, también pagará dicha comisión calculada sobre el monto a pagar. Esta condición aplicara únicamente cuando el saldo del capital adeudado exceda cien mil dólares ($100,000.00) o su equivalente en lempiras, o los fondos sean provenientes de una institución que penalice a <b>PRESTADITO</b> por pago anticipado, cualquiera de las dos o ambas conjuntamente.
                                <b>- F) PROPIEDAD DEL VEHICULO.</b>
                                Mientras no se haya cancelado la totalidad del Préstamo, será dueño del vehículo <b>PRESTADITO</b>, el cliente será considerado como poseedor y esta posesión esta condicionada, es decir si el cliente está cumpliendo con las obligaciones contraídas en este contrato, de lo contrario <b>PRESTADITO</b> podrá a su discreción retirarlo.
                            </p>
                            <p>
                                <b>- G) OTROS GASTOS:</b>
                                <b>I-</b> Los gastos que se incurra por matricula, mantenimiento, reparación y todas las relacionadas para conservación del vehículo en perfecto estado, deberá ser pagadas por el <b>CLIENTE</b>, 
                                <b>II-</b> Los gastos que se ocasione en la recuperación del vehículo en caso de mora será de 3,000.00 Lempiras por concepto de grúa más lo generado por concepto de parqueo, Una vez recuperado el vehículo cuando se encuentre en mora, Prestadito lo tendrá en custodia un máximo de 2 meses para que el cliente se ponga al día y pague todo lo adeudo, pasado ese tiempo Prestadito podrá vender el vehículo para poder recuperar el dinero invertido.
                                <b>III-</b> En caso que un gestor se movilice a traer el dinero del pago de una o más cuotas, bien sea al domicilio o  lugar de trabajo, tendrá un costo de 250.00 Lempiras.                                
                                <b>- TERCERO: AUTORIZACIONES ESPECIALES: EL CLIENTE</b>
                                por este acto, en tanto no haya cumplido con el pago total de su obligación, autoriza a <b>PRESTADITO</b> expresamente y sin ser necesario la notificación previa para:
                                
                                <span class="page-break"></span>
                                <b>A)</b> Vender, Ceder o de cualquier otra forma traspasar, o celebrar contratos de participación, de descuentos con relación al crédito y derechos consignados en este documento o títulos valores relacionados a este mismo;
                                <b>B)</b> Autorizar a <b>PRESTADITO</b> para que en cualquier tiempo pueda acceder a la información de la Central de Riesgos de la Comisión Nacional de Bancos y Seguros u otra central de riesgo pública o privada, para gestionar y conocer la situación crediticia de <b>EL CLIENTE</b> frente a las demás instituciones del sistema financiero nacional.
                                <b>- C) EL CLIENTE</b> Autoriza de manera Irrevocable, a que <b>PRESTADITO</b> pueda entrar en su domicilio, para solo efecto de retirar el vehículo comprado con este préstamo, o que lo retire de una tercera persona sin necesidad de intervención judicial, esta cláusula solo se ejecutara en caso de mora de 1 o más cuotas vencidas y mientras no haya sido cancelado el total adeudado.
                                <b>D- El cliente</b> Autoriza que Prestadito pueda revisar el funcionamiento del GPS mientras no se haya cancelado la totalidad del prestamo.
                                
                                <b>- CUARTO: OBLIGACIONES GENERALES.- EL CLIENTE</b> durante la vigencia del presente contrato también se obliga a:
                                <b>A)</b> Permitir que <b>PRESTADITO</b> ejerza los controles que juzgue convenientes, para asegurarse que los fondos de este crédito se inviertan en los fines que se han indicado anteriormente y condiciones que se estipulan en este contrato.
                                <b>- B) DE LA GARANTIA:</b>
                                En calidad de Garantía para el Cumplimiento de la presente obligación <b>El CLIENTE</b> firmara una <b>PAGARE</b> sin protesto, así como también da en propiedad a <b>PRESTADITO</b> el vehículo comprado con el dinero objeto del presente préstamo, quedándose <b>PRESTADITO</b> con la documentación original del vehículo y el <b>CLIENTE</b> en posesión del vehículo del cual será responsable mientras se encuentre en su poder y no se haya cancelado el precio total pactado para la terminación del presente contrato.
Sin perjuicio de la <b>designación de garantías fiduciarias como ser Menaje de Hogar y demás bienes pertenecientes AL CLIENTE</b>, por lo que está terminantemente prohibido para el <b>CLIENTE</b> utilizar el vehículo para transporte público como ser taxi en todas sus modalidades incluyendo VIP,
asi como también transporte de carga o similares, también se le porhibe enajenar, vender, permutar, donar, gravar, prestar o dar en prenda el vehículo dado en propiedad, sin la autorización por escrito otorgada por <b>PRESTADITO</b>, el incumplimiento de las prohibiciones faculta a <b>PRESTADITO</b> a retirar el vehículo.
Para el menaje se formalizará el Inventario de estos, este que pasara a formar parte del presente contrato.
                                <b>C)</b> Suscribir y a mantener un seguro para vehículos en lempiras moneda de curso legal en Honduras; mientras esté vigente la deuda, por la cuantía y condiciones que señale <b>PRESTADITO</b>, con una compañía aseguradora; siendo entendido que <b>EL CLIENTE</b> deberá endosar a favor de <b>PRESTADITO</b> la respectiva póliza de seguro, o a favor de la persona natural o jurídica a cuyo nombre se traspase el presente crédito, hasta la total cancelación del saldo pendiente de pago por la deuda. <b>PRESTADITO</b> podrá pagar y cargar al préstamo las primas de seguro, si <b>EL CLIENTE</b> no lo renueva y paga a los treinta (30) días previos al vencimiento de la póliza de seguro respectiva, sin que la acción del pago o cargo sea obligatorio para <b>PRESTADITO</b>, quien no asumirá ni incurrirá en responsabilidad por no hacer el pago de las primas de seguro.
                                <b>D)</b> Mantenerse al día en el pago de los impuestos que graven a <b>EL CLIENTE</b> o al <b>VEHÍCULO</b> dado en garantía.
                                <b>- E)</b> Cuidar como buen padre de familia el vehículo dado en garantía, mientras se encuentre en su poder y no se haya cancelado el precio total pactado para la terminación del presente contrato, quedando a su cargo los riesgos de dicho bien mueble por lo que será responsable de la perdida, destrucción o deterioro que sufra aun por caso fortuito o fuerza mayor.
                                <b>- F)</b> Mantener la licencia de conducir vigente, mientras no se haya cancelado la totalidad del préstamo, en caso que <b>EL CLIENTE</b> haya solicitado excepción al momento de otorgarse el Préstamo por no poseer licencia vigente, entonces dispondrá solamente de un máximo de 40 días para presentar la Licencia de conducir a <b>PRESTADITO</b>, caso contrario <b>AUTORIZA</b> anticipadamente a <b>PRESTADITO</b> a que se le retire, en calidad de custodia, el vehiculo hasta que presente la licencia de conducir aun y cuando sus cuotas estén al día.
La excepción anterior <b>no faculta al CLIENTE</b> a conducir el vehiculo sin su respectiva Licencia emitida por la Dirección Nacional de Transito, ni a prestar a quien no tenga dicho documento, ya que <b>PRESTADITO</b> respeta las leyes hondureñas.
                                <b>- QUINTO: DE LOS DEBERES DEL CLIENTE: </b>
                                Se conviene que, desde la fecha de otorgamiento de este contrato, hasta la fecha en que se pague el total de las obligaciones pendientes con <b>PRESTADITO, EL CLIENTE</b> deberá informar siempre, por vía telefónica o escrita y a la brevedad posible, las siguientes acciones:
                                <b>1)</b> Contraiga deudas con otras instituciones financieras, no financieras, puestos de bolsa, proveedores, filiales y otros. <b>EL CLIENTE</b> aprueba libre y voluntariamente por ser válidas, todas las condiciones fijadas en este inciso, por entender que de tal manera <b>PRESTADITO</b> se asegura de la solvencia de <b>EL CLIENTE</b> y del pago del crédito otorgado.
                                <b>- SEXTO: DE LAS MODIFICACIONES DEL CONTRATO.- PRESTADITO</b> comunicará a
                                <b>EL CLIENTE: 1)</b> De manera general y sin necesidad de especificarlo individualmente, las condiciones contractuales pactadas, por cualquier medio impreso de circulación nacional, o el medio de comunicación que las partes hayan designado, en los casos de los efectos de la aplicación de la vigencia de una ley, con 30 días calendario de anticipación a la aplicación de dicho cambio;
                                <b>2)</b> Para el caso que las tasas de intereses y otros cargos sea modificada, se aplicará conforme a un factor variable que considera la tasa de interés que se concede para los depósitos a plazo, más el costo de la intermediación y sumándole un diferencial del veinte por ciento. La tasa de interés se revisará cada 3 meses. En el caso que las tasas de interés sean reguladas por el Banco Central de Honduras conforme al artículo 55 de la Ley del Sistema Financiero, se aplicará la tasa máxima permitida por dicha Institución, o la que fije y aplique <b>PRESTADITO</b>, notificándolo a <b>EL CLIENTE</b> con 15 días calendario de anticipación por lo menos por cualquiera de los medios de comunicación descritos en este contrato o los otros establecidos por la Ley, siendo entendido que cualquier ajuste resultante de la modificación de la tasa de interés será cubierto por <b>EL CLIENTE</b> quedando <b>PRESTADITO</b> autorizado para efectuar y cobrar tales ajustes y modificar la cuota quincenal o mensual del financiamiento de acuerdo al plazo que reste para la cancelación del mismo, así mismo las partes acuerdan incorporar como vinculante el principio <b>“ceteris paribus”</b>, respecto a modificaciones atinentes al contrato o los convenios incorporados.
                                <b>-SEPTIMO: RECLAMOS.-</b>Cuando se presente algún evento por el cual <b>EL CLIENTE</b> desee hacer un reclamo, se dispondrá de un plazo de 10 días hábiles para realizarlo, transcurrido éste, es entendido que caduca su derecho para reclamar y se declara vencido. Cuando sea reclamos por cuestiones de garantía deberá presentarlas al distribuidor autorizado y en caso de ser bienes usados no podrá presentar reclamos después de 30 dias de realizada la compra, es entendido que <b>PRESTADITO</b> no está obligado a resolver cuestiones de garantía puesto que solo es quien financia la compra.
                                
                                <b>- OCTAVO: DEL VENCIMIENTO ANTICIPADO DEL PLAZO DE PAGO.- </b>Además de los casos establecidos por la ley, <b>PRESTADITO</b> podrá dar por vencido el plazo establecido para el pago del préstamo concedido en este contrato, y en consecuencia exigir el pago inmediato del saldo del capital, intereses, comisiones, recargos y gastos, ya sea por la vía judicial o extra judicial, por cualquiera de los siguientes eventos:
                                <b>a)</b> Por falta de pago de una o más de las cuotas pactadas, de los intereses, o de cualquier otro cargo pendiente a favor de <b>PRESTADITO</b>;

                                
                                <b>b)</b> Por el conocimiento de la ejecución judicial iniciada por terceros, o por el mismo <b>PRESTADITO</b>, en contra de <b>EL CLIENTE</b>, originada por otros créditos;
                                <b>c)</b> Por no destinar el presente préstamo para el fin o fines para los cuales ha sido concedido;
                                <b>d)</b> Por la declaración del estado de suspensión de pagos, de quiebra o de concurso de <b>EL CLIENTE</b>, así como por su inhabilitación para el ejercicio del comercio, o por el ejercicio de acción penal en su contra o de su representante legal que derivare en sentencia de privación de libertad;
                                <b>e)</b> Por el incumplimiento o negativa por parte de <b>EL CLIENTE</b> a proporcionar la información requerida por <b>PRESTADITO</b> en forma escrita;
                                <span class="page-break"></span>
                                <b>f)</b> Por actuación fraudulenta o haber proporcionado a <b>PRESTADITO</b> información o datos falsos o incompletos para obtener el préstamo;
                                <b>g)</b> Por ser del conocimiento de <b>PRESTADITO</b>, la existencia de obligaciones de <b>EL CLIENTE</b> pendientes de pago con el Estado, en tal cantidad que a su criterio ponga en peligro la recuperación de los adeudos debido a la preferencia del Estado para obtener el pago a su favor antes que <b>PRESTADITO</b>;
                                <b>h)</b> El incumplimiento de parte de <b>EL CLIENTE</b> de cualquiera de las obligaciones contraídas en este contrato.
                                <b>i)</b> Por retirar, desconectar, adulterar, o de cualquier forma hacer que el GPS del Vehiculo no funcione correctamente.
                                
                                <b>- NOVENO: COBROS EXTRAJUDICIALES.-</b> En caso de ser necesarias las gestiones de cobranzas extrajudiciales por la mora en el pago o el vencimiento anticipado del contrato, estas se realizarán de la siguiente manera:
                                <b>1)</b> Para Mora de 1 a 180 días: alternativamente podrán ser llamadas telefónicas, correos electrónicos, mensajes por cualquier medio electrónico, visitas por gestores, cartas de cobro escritas solicitando el pago y dirigidas a las direcciones indicadas. Estas gestiones tendrán un costo de doscientos cincuenta lempiras (L250.00), cargados al estado de cuenta del préstamo otorgado, son acumulables por cada cuota vencida y serán pagados por <b>EL CLIENTE</b> en todos los casos y sin excepción;
                                <b>2)</b> Si su caso fuere trasladado a Profesionales del Derecho, cuyas gestiones iniciales podrán ser: llamadas telefónicas, envió de correos electrónicos, cartas de cobro escritas, y visitas, causaran el cobro de honorarios
profesionales según el Arancel del Profesional del Derecho vigente, y se calculará sobre el capital, intereses, recargos, cargos y seguros en mora, tal como lo establece el artículo 1432 del Código Civil.- En caso de ser perseguida la deuda por proceso Judicial, se cargaran igualmente los gastos ocasionados por costas durante dicho proceso. <b>PRESTADITO</b> podrá asignar a una empresa o Agencia de Cobranzas y/o Recuperaciones para que realice estas labores de cobro desde el día uno de atraso en el estado de cuenta lo cual es aceptado por el deudor.
                                <b>- DÉCIMO: ACCIONES JUDICIALES.- </b>
                                En caso de mora o vencimiento anticipado del contrato, dará lugar para que <b>PRESTADITO</b> ejerza las acciones judiciales correspondientes, quedando obligado el cliente a pago de gastos y honorarios que ocasione el procedimiento judicial. Así como para determinar el saldo adeudado El estado de cuenta certificado por el contador de <b>PRESTADITO</b> o de quien haya adquirido los derechos, hará fe en juicio para establecer el saldo a cargo de <b>EL CLIENTE</b> y Constituirá junto con el presente contrato título ejecutivo, sin necesidad de reconocimiento de firma ni de otro requisito previo alguno, según lo establecido en la ley del sistema financiero. En caso de ejecución de la presente obligación las partes nos sometemos a la jurisdicción y competencia de los Juzgados de San Pedro Sula, Cortés.
                                <b>.- DÉCIMO PRIMERA: MEDIOS PARA COMUNICACIONES.- EL CLIENTE y PRESTADITO</b> establecen y a la vez autorizan, que para las distintas notificaciones que se deban hacer conforme a lo estipulado por este contrato o por lo dispuesto por la ley, se harán efectivas a través de uno solo de los siguientes medios:
                                <b>A)</b> Correspondencia ordinaria escrita dirigida a las direcciones indicadas en el preámbulo de este contrato;
                                <b>B)</b> Notificación por la vía electrónica a su correo electrónico
                                <asp:Label runat="server" ID="lblCorreo_Contrato"></asp:Label>
                                <b>C)</b> Notificación mediante cualquier red sociales que pudiese pertenecer al Cliente,
                                <b>D)</b> o a las direcciones indicadas en cualquiera de los documentos suscritos con <b>“ PRESTADITO ”</b>. Cualquier cambio de dirección o número telefónico deberá notificarse fehacientemente, con una anticipación razonable a <b>PRESTADITO</b> y hasta entonces se considera efectiva.
                                <b>.-DÉCIMO SEGUNDA: DE LAS AUTORIZACIONES ESPECIALES. EL CLIENTE </b>otorga de manera expresa, voluntaria e irrevocable su consentimiento para que en caso de mora, <b>PRESTADITO</b> o sus representantes puedan ingresar a su domicilio a retirar el vehículo, y por lo tanto lo exime de toda responsabilidad que pueda incurrir según el artículo 99 de la Constitución de la Republica.
Así como faculta a <b>“ PRESTADITO ”</b>, sus distintas dependencias, así como también a su personal, que mediante visitas a su domicilio se le puedan presentar y ofrecer las diferentes propuestas de negocio, servicios, catálogos de nuevos productos; a su vez, faculta otros canales, sean estos telefónicos o electrónicos, a que se comuniquen y a que le informen en los días de semana, así como también en los días llamados vacaciones, o festivos, en los diferentes horarios abiertos, incluso fin de semana, exonerándole de cualquier perjuicio a la empresa o de ser estas visitas catalogadas como “hostigamiento”.
                                <b>- El CLIENTE:</b> autoriza de manera expresa y voluntaria que en caso de que PRESTADITO retire el vehículo, pueda ser subastado al mejor postor cuando el PRESTAMO presente 60 dias de mora y el dinero recibido de la misma se abonara a la deuda, si existiera un excedente se le dará al <b>CLIENTE</b> y en caso que no cubriese el total adeudado, <b>PRESTADITO</b> se reserva el derecho de ejercer acciones legales contra el <b>CLIENTE</b> por el pago de saldo total adeudado, que incluye capital, intereses, otros cargos o gastos que incurra por recuperacion, reparacion, impuestos u otros.

                                <b>- DÉCIMO TERCERA: PROHIBICIONES PARA EL CLIENTE:</b> Mientras este contrato no haya sido cancelado en su totalidad es se le Prohibe realizar las siguientes acciones:
                                <b>A)</b> utilizar el vehiculo para transporte publico como ser taxi en todas sus modalidades incluyendo VIP, 
                                <b>B)</b> utilzar el vehiculo como transporte de carga comercial.
                                <b>-C)</b>, Adulterar el GPS o mandarlo a retirar o negarse.
                                <b>D)</b> Enajenar, vender, permutar, donar, gravar, alquilar, rentar, prestar o dar en prenda el vehículo dado en propiedad, sin la autorización por escrito otorgada por PRESTADITO, la inobservacias de las prohibiciones faculta a PRESTADITO a dar por vencido el plazo y podra retirar el vehículo, sin intervencion JudiciaL o institucion reguladora.
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
                    <!-- Firma del cliente -->
                    <div class="row mt-5">
                        <div class="col-1"></div>
                        <div class="col-5 text-center">
                            <label class="mt-3 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <asp:Label class="mt-0 d-block" runat="server" ID="lblNombreFirma_Contrato"></asp:Label>
                            <asp:Label class="mt-0 d-block" runat="server" ID="lblIdentidadFirma_Contrato"></asp:Label>
                            <label class="mt-0 d-block">EL CLIENTE</label>
                        </div>
                        <div class="col-5 text-center">
                            <label class="mt-3 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">ERICK GEOVANY MOLINA PADILLA</label>
                            <label class="mt-0 d-block">1301-1980-00105</label>
                            <label class="mt-0 d-block">PRESTADITO</label>
                        </div>
                        <div class="col-1"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Pagaré -->
        <div id="divContenedorPagare" style="margin-top: 999px; display: none;">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divPagarePDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="/Imagenes/LogoPrestadito.png" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">PAGARÉ POR
                                <asp:Label runat="server" ID="lblMontoTitulo_Pagare"></asp:Label></h5>
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
actuando en condición personal, acepto que <b>DEBO y PAGARÉ</b> incondicionalmente <b>SIN PROTESTO,</b> y a la orden de <b>PRESTADITO S.A. de C.V.</b>,
la cantidad de
                                <asp:Label runat="server" ID="lblMontoPalabras_Pagare"></asp:Label>
                                (<asp:Label runat="server" ID="lblMontoDigitos_Pagare"></asp:Label>).
Dicha cantidad será pagada el día
                                <asp:Label runat="server" ID="lblDiaPrimerPago_Pagare"></asp:Label>
                                del mes de
                                <asp:Label runat="server" ID="lblMesPrimerPago_Pagare"></asp:Label>
                                del año
                                <asp:Label runat="server" ID="lblAnioPrimerPago_Pagare"></asp:Label>,
en las oficinas, agencias, sucursales y ventanillas de <b>PRESTADITO S.A. de C.V.</b>.

La cantidad consignada en este PAGARE devengará, a partir de esta fecha, una tasa de interés fluctuante del
                                <asp:Label runat="server" ID="lblPorcentajeInteresFluctuante_Pagare"></asp:Label>%
PORCIENTO MENSUAL,
sobre el saldo total de la deuda, a pagar mensualmente. En caso de mora, que se producirá por la falta de pago al vencimiento tanto del capital o de los intereses,
dará derecho a <b>PRESTADITO S.A. de C.V.</b> a exigir el pago de intereses moratorios del
                                <asp:Label runat="server" ID="lblInteresesMoratorios_Pagare"></asp:Label>%
PORCIENTO MENSUAL;
a su vez, en caso de ejecución legal de la presente obligación, me someto a la jurisdicción que establezca <b>PRESTADITO S.A. de C.V.</b>,
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

        <!-- Compromiso Legal -->
        <div id="divContenedorCompromisoLegal" style="margin-top: 999px; display: none;">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divCompromisoLegalPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="/Imagenes/LogoPrestadito.png" />
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
                                    <asp:Label runat="server" ID="lblNombreCliente_CompromisoLegal"></asp:Label>,</b>
                                acepto haber adquirido un préstamo en efectivo con la empresa <b>PRESTADITO S.A. de C.V.</b>,
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
                                La garantía de dicho vehículo corresponde exclusivamente al distribuidor o concesionario donde fue adquirido; por lo tanto, <b>PRESTADITO S.A. de C.V.</b>
                                no se hace responsable de la garantía, la cual funciona de acuerdo a políticas y restricciones del distribuidor
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
a los <span class="lblNumeroDia_Firma"></span>
                                días del mes de <span class="lblMes_Firma"></span>
                                del año <span class="lblAnio_Firma"></span>.
                            </p>
                        </div>
                    </div>
                    <!-- Firma del cliente -->
                    <div class="row mt-5 justify-content-center">
                        <div class="col-5 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Nombre</label>
                        </div>
                        <div class="col-5 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Firma</label>
                        </div>
                        <div class="col-5 text-center mt-5">
                            <asp:Label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" ForeColor="Black" Style="border-radius: 0px;" runat="server" ID="lblIdentidadFirma_CompromisoLegal"></asp:Label>
                            <label class="mt-0 d-block">Identidad</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Convenio de regulación de compra y venta de vehiculos para financiamiento a tercero -->
        <div id="divContenedorConvenioCyV" style="margin-top: 999px; display: none;">
            <div class="card m-0 pt-4 divImprimir" runat="server" visible="true" id="divConevionCyVPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="/Imagenes/LogoPrestadito.png" class="mt-0 pt-0" />
                        </div>
                        <div class="col-12">
                            <h6 class="text-center font-weight-bold">Convenio de regulación de compra y venta de vehículos para financiamiento a Tercero</h6>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                Nosotros ERICK GEOVANY MOLINA PADILLA, mayor de edad, casado, hondureño, Ingeniero Industrial y de este domicilio, quien actúa en condición de representante
Legal de la Empresa Mercantil denominada “PRESTADITO S.A DE C.V. con Registro Tributario Nacional 05019016811399, Empresa Constituida mediante
escritura pública Instrumento número 86 autorizada en esta Ciudad el día 18 de Diciembre del año 2,015 por el Notario Efraín Antonio Gutiérrez Ardon e inscrita
bajo número de MATRICULA 96565 del Registro Mercantil del Centro Asociado de la Cámara de Comercio e Industria de Cortes,
en este convenio se denominara <b>LA EMPRESA</b> y <b>AL SEÑOR(A)</b>
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
                            <p>
                                <b>Primero:</b>
                                A la firma de este documento las partes reconocen que tienen relación contractual, la cual servirá de guía al momento en que la Empresa decida otorgar crédito a terceras personas y El cliente posea el vehículo a financiar.
                            </p>
                            <p>
                                <b>Segundo:</b>
                                El Cliente Jura que los recursos que utiliza para la compra de vehículos provienen de Actividades Licitas.
                            </p>
                            <p>
                                <b>Tercero:</b>
                                La Empresa se obliga a pagar con cheques a favor de El Cliente y se hará responsable de los daños y perjuicios ocasionados en el caso que no posea fondos suficientes.
                            </p>
                            <p>
                                <b>Cuarto:</b>
                                El Cliente declara que los vehículos no están denunciados por haber participado en accidentes de tránsitos o en cualquier actividad ilícita.
                            </p>
                            <p>
                                <b>Quinto:</b>
                                La Empresa manifiesta que los recursos utilizados para la compra de los vehículos provienen de actividades licitas.
                            </p>
                            <p>
                                <b>Sexto:</b>
                                El cliente se obliga a entregar el vehículo a la persona que la Empresa designe.
                            </p>
                            <p>
                                <b>Séptimo:</b>
                                El cliente garantiza que los vehículos están en perfecto estado tanto en condiciones físicas como mecánicas para circular y reúne todas las medidas de seguridad solicitadas por las Leyes Vigentes en el País.
                            </p>
                            <p>
                                <b>Octavo:</b>
                                El Cliente se compromete a traspasar el vehículo a favor de la Persona que La Empresa designe.
                            </p>
                            <p>
                                <b>Noveno:</b>
                                El Cliente se compromete a entregar todos los documentos del vehículo a La Empresa.
                            </p>
                            <p>
                                <b>Décimo:</b>
                                El Cliente se obliga a indemnizar en concepto de daños y perjuicios ocasionados a La Empresa en las Clausulas anteriores.
                            </p>
                            <p>
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
                    <div class="row justify-content-center mt-1 pt-1">
                        <div class="col-5 text-center mt-1">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Por la empresa PRESTADITO S.A. de C.V.</label>
                            <label class="mt-0 d-block">ERICK GEOVANY MOLINA PADILLA</label>
                        </div>
                        <div class="col-5 text-center mt-1">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Por el cliente</label>
                            <asp:Label runat="server" ID="lblNombre_ConvenioCyV" class="mt-0 d-block"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Inspeccion del vehiculo -->
        <div id="divContenedorInspeccionSeguro" runat="server">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divInspeccionSeguroPDF">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 p-0 text-center mb-3">
                            <div class="float-left"><img src="/Imagenes/LogoPrestadito.png" /></div>
                            <div class="float-right" id="qrCode_InspeccionSeguro"></div>
                            <div class="pt-5"><h5 class="text-center pl-70px font-weight-bold">INSPECCIÓN SEGURO DE VEHÍCULO</h5></div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <table class="table table-bordered border-dark" style="border-width: 1px;">
                                <tr>
                                    <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Asegurado</th>
                                    <td colspan="5" class="p-0">
                                        <asp:Label runat="server" ID="lblNombre_InspeccionSeguro"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Marca</th>
                                    <td class="p-0">
                                        <asp:Label runat="server" ID="lblMarca_InspeccionSeguro"></asp:Label>
                                    </td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Modelo</th>
                                    <td class="p-0">
                                        <asp:Label runat="server" ID="lblModelo_InspeccionSeguro"></asp:Label>
                                    </td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Año</th>
                                    <td class="p-0">
                                        <asp:Label runat="server" ID="lblAnio_InspeccionSeguro"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Tipo</th>
                                    <td class="p-0">
                                        <asp:Label runat="server" ID="lblTipoDeVehiculo_InspeccionSeguro"></asp:Label>
                                    </td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Kilometraje</th>
                                    <td class="p-0">
                                        <asp:Label runat="server" ID="lblRecorrido_InspeccionSeguro"></asp:Label>
                                    </td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0 pr-0">Placa</th>
                                    <td class="p-0">
                                        <asp:Label runat="server" ID="lblMatricula_InspeccionSeguro"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 pl-0">
                            <div style="max-width: 794px !important; min-width: 794px !important; overflow-x: hidden;">
                                <div id="divGaleriaInspeccionSeguroDeVehiculo" style="width: 100% !important; max-width: 100% !important; overflow-x: hidden;" runat="server">
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Recibo -->
        <div id="divContenedorRecibo" runat="server" style="margin-top: 999px; display: none;">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divReciboPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 mb-5">
                            <img src="/Imagenes/LogoPrestadito.png" />
                        </div>
                        <div class="col-6 align-self-center">
                            <h4 class="text-center font-weight-bold">PRESTADITO S.A DE C.V</h4>
                        </div>
                        <div class="col-6">
                            <table class="table table-bordered" style="border-width: 1px !important;">
                                <tr>
                                    <th class="bg-light font-weight-bold text-center pb-0">RTN</th>
                                    <td class="p-0 text-center font-weight-bold">
                                        <asp:Label runat="server" ID="lblRTNEmpresa_Recibo">05019016811399</asp:Label></td>
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
                            </table>
                        </div>
                    </div>
                    <div class="row mb-5">
                        <div class="col-12">
                            <table class="table table-bordered mb-0">
                                <tr>
                                    <th class="bg-light font-weight-bold p-1" style="width: 15%">Recibí de:</th>
                                    <td class="p-1 font-weight-bold">
                                        <asp:Label runat="server" ID="lblNombreEmpresa_Recibo">PRESTADITO SA DE CV</asp:Label>
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
                                    <td class="p-1 font-weight-bold">
                                        <asp:Label runat="server" ID="lblConceptoPago_Recibo">FINANCIAMIENTO DE VEHICULO AUTOMOTOR MARCA:
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
                                        </asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <table class="table table-bordered mb-5">
                                <tr>
                                    <th class="font-weight-bold p-1 text-right" style="width: 50%">TOTAL RECIBIDO</th>
                                    <td class="p-1 font-weight-bold text-right" style="width: 50%">
                                        <asp:Label runat="server" ID="lblTotalRecibido_Recibo"></asp:Label>
                                    </td>
                                </tr>
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

        <!-- Traspaso de vehiculo del cliente -->
        <div id="divContenedorTraspaso" style="margin-top: 999px; display: none;">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divTraspasoPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="/Imagenes/LogoPrestadito.png" />
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
                                    <asp:Label runat="server" ID="lblNombreCliente_Traspaso"></asp:Label>,</b>
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
                                El cual cedo todos los deberes y derechos que antes ejercía sobre el vehículo antes mencionado a PRESTADITO SA, con Numero de RTN 0501-9016-811399, aceptando que el vehículo es
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
                    <!-- Firma del cliente -->
                    <div class="row mt-5">
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

        <!-- Traspaso de vehiculo del propietario -->
        <div id="divContenedorTraspasoVendedor" style="margin-top: 999px; display: none;">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divTraspasoVendedorPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="/Imagenes/LogoPrestadito.png" />
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
                                    <asp:Label runat="server" ID="lblNombreCliente_TraspasoVendedor"></asp:Label>,</b>
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
                                El cual cedo todos los deberes y derechos que antes ejercía sobre el vehículo antes mencionado a PRESTADITO SA, con Numero de RTN 0501-9016-811399, aceptando que el vehículo es
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
                    <!-- Firma del cliente -->
                    <div class="row mt-5">
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

        <!-- Básico + CPI -->
        <div id="divContenedorBasicoCPI" style="margin-top: 999px; display: none;">
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
                                Por una Parte, PRESTADITO. DE C.V. con domicilio en la ciudad de San Pedro Sula, Cortes, Honduras, C.A.
representada por el señor ERICK GEOVANI MOLINA PADILLA, facultad otorgada mediante instrumento
número 86 ante notario Efraín Antonio Gutiérrez Ardon, que en lo sucesivo se llamara &lt;LA COMPAÑÍA&gt;, de
conformidad a la SOLICITUD presentada al cliente y las CONDICIONES GENERALES de la presente.
Por la otra
                                <asp:Label Font-Bold="true" runat="server" ID="lblNombreCliente_BasicoCPI"></asp:Label>
                                <b>(El cliente)</b>, con las generales mencionadas en el prestamo
                                <asp:Label runat="server" Font-Bold="true" ID="lblNumeroPrestamo_BasicoCPI"></asp:Label>
                                que en lo sucesivo se denominará &lt;EL CONTRATANTE&gt;.
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
                            <ol type="a">
                                <li>Vuelcos Accidentales o Colisiones</li>
                                <li>Incendio, Auto ignición y Rayo</li>
                                <li>Robo Total.</li>
                                <li>Huelgas y Alborotos Populares.</li>
                                <li>Responsabilidad Civil en sus Bienes (Daños a Terceros en sus Bienes, hasta Lps 50,000)</li>
                                <li>Responsabilidad Civil en sus Personas (Daños a Terceros en sus Personas, hasta Lps 50,000)</li>
                                <li>Desbordamiento de Ríos, derrumbes de carretera y otros fenómenos de la naturaleza.</li>
                            </ol>
                            <p>
                                <b>TERCERA: CONDICIONES ESPECIALES</b>
                            </p>
                            <ol type="a">
                                <li>Los siniestros mencionados en la cláusula anterior deben ocurrir en el territorio nacional para que sea cubierto.</li>
                                <li>El vehículo debe de ser conducido por una persona debidamente acreditada, mayor de 18 años y menor a 75 años de lo contrario la garantía no cubre.</li>
                                <li>Aplica única y exclusivamente para reparar los daños ocasionados al automóvil que se encuentre en arrendamiento con Compañía Financiera “PRESTADITO S.A.” o en su defecto los daños causados a un
tercero hasta Lps 50,000 siempre y cuando el arrendamiento este vigente y con sus saldos al día, bajos los límites de cobertura indicados, el saldo capital adeudado a la fecha del siniestro.
                                </li>
                                <li>Para que los daños ocasionados sean cubiertos, el automóvil deberá ser conducido por El Arrendatario, esposa e hijos con licencia vigente y deberá estar al día en sus cuotas de arrendamiento,
caso contrario los daños ocasionados serán reparados por cuenta del arrendatario.
                                </li>
                                <li>El Arrendatario debe contar con licencia de conducir vigente.</li>
                                <li>Solo cubre colisiones debidamente acreditas con el parte de la policía de tránsito y cuando sea
denuncia interpuesta sin parte de transito solo se reconoce como máximo el 50% del valor del siniestro previa inspección de ajustador.
                                </li>
                                <li>En caso de robo del vehículo en garantía será necesario acreditar dicho siniestro ante entidad
competente en territorio Hondureño.
                                </li>
                                <li>El arrendatario se obliga a reportar en un plazo máximo de 24 hrs el siniestro a Prestadito SA al
teléfono 2540 1050 o vía email a sac@miprestadito.com
                                </li>
                                <li>Mantener al día el pago del préstamo y la cuota del servicio del seguro y respectivo GPS.</li>
                            </ol>
                            <span class="page-break"></span>
                            <p class="mt-5">
                                <b>CUARTA: SE EXCLUYE DE SINIESTROS</b>
                            </p>
                            <ol type="a">
                                <li>No se cubre los accidentes o daños ocasionados al automóvil por el arrendatario, sin que tenga
implicación un tercero a excepción de Vuelcos Accidentales, Colisiones Accidentales, Incendio, Auto
ignición, Rayo,Desbordamiento de Ríos, derrumbes de carretera y otros fenómenos de la naturaleza
                                </li>
                                <li>No cubre rotura de cristales</li>
                                <li>No cubre equipo especial que pueda ser instalado</li>
                            </ol>
                            <p>
                                <b>QUINTA: PROCESO DE RECLAMO</b>
                                <br />
                                Al presentarse un reclamo el usuario deberá avocarse de manera personal al Oficial de Crédito de PRESTADITO S.A.
                            </p>
                            <ol type="a">
                                <li>Presentar dos cotizaciones de los daños ocasionados al automóvil asegurado</li>
                                <li>Certificación del Parte de Tránsito o Certificado de Denuncia interpuesta</li>
                                <li>Fotografías a color de los daños del automóvil</li>
                                <li>Fotografías a color del automóvil una vez reparado</li>
                                <li>Los derechos de cobertura no pueden ser traspasados, cedidos, ni endosados</li>
                                <li>PRESTADITO S.A. optara por una tercera cotización, reservándose el derecho de autorizar la reparación del automóvil asegurado en el taller de su elección.</li>
                            </ol>
                            <p>
                                Una vez completada la documentación de reclamación del siniestro el proceso de pago lo realizara la empresa
en un plazo no mayor a 10 días de hábiles, conforme a reclamo
                            </p>
                            <p>
                                <b>SEXTA: El contratante,</b>
                                entiende y acepta que las coberturas anteriormente mencionadas están respaldas por
una compañía de seguros local y que exclusivamente en caso de que la compañía aseguradora local no cubra
el montó del siniestro debido a que este exceda el límite anual especificado en el contrato de crédito, este
será cubierto por una empresa extranjera tercera contratada por Prestadito SA para este fin, misma que se
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
                    <!-- Firma del cliente -->
                    <div class="row justify-content-center mt-1 pt-1">
                        <div class="col-5 text-center mt-1">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Contratante o Cliente</label>
                        </div>
                        <div class="col-5 text-center mt-1">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Prestadito S.A.</label>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Correo liquidación -->
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

        <!-- Correo seguro -->
        <div id="divContenedorCorreoSeguro" style="display: none;">
            <div id="divCorreoSeguroPDF">
                <table border="1" style="width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;">
                    <tr>
                        <th colspan='2' style='text-align: left; font-weight: bold;'><em>DATOS DEL VEHICULO</em></th>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>AÑO</th>
                        <td>
                            <asp:Label runat="server" ID="lblAño_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>PLACA</th>
                        <td>
                            <asp:Label runat="server" ID="lblPlaca_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>MARCA</th>
                        <td>
                            <asp:Label runat="server" ID="lblMarca_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>MODELO</th>
                        <td>
                            <asp:Label runat="server" ID="lblModelo_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>TIPO</th>
                        <td>
                            <asp:Label runat="server" ID="lblTipoVehiculo_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>COLOR</th>
                        <td>
                            <asp:Label runat="server" ID="lblColor_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>MOTOR</th>
                        <td>
                            <asp:Label runat="server" ID="lblSerieMotor_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>CHASIS</th>
                        <td>
                            <asp:Label runat="server" ID="lblSerieChasis_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>VIN</th>
                        <td>
                            <asp:Label runat="server" ID="lblVIN_CorreoSeguro"></asp:Label>
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
                            <asp:Label runat="server" ID="lblNombreCliente_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'>IDENTIDAD</th>
                        <td>
                            <asp:Label runat="server" ID="lblIdentidadCliente_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th style='text-align: left;'># PRESTAMO</th>
                        <td>
                            <asp:Label runat="server" ID="lblNumeroPrestamo_CorreoSeguro"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <!-- Nota de entrega -->
        <div id="divContenedorNotaDeEntrega" style="margin-top: 999px; display: none;">
            <div class="card m-0 divImprimir" runat="server" visible="true" id="divNotaDeEntregaPDF" style="display: none;">
                <div class="card-body pt-0 pr-5 pl-5">
                    <div class="row">
                        <div class="col-12 m-0 p-0 text-center">
                            <img src="/Imagenes/LogoPrestadito.png" class="pt-4" />
                        </div>
                        <div class="col-12">
                            <h4 class="text-center pt-2 pb-4"><b>NOTA DE ENTREGA
                                <br />
                                POR APROBACIÓN DE CRÉDITO</b></h4>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <p>
                                Estimado (s)
                                <b>
                                    <asp:Label runat="server" ID="lblPropietarioGarantia_NotaEntrega"></asp:Label>,
                                </b>
                                por este medio hacemos de su conocimiento que el cliente
                                <asp:Label runat="server" ID="lblNombreCliente_NotaEntrega"></asp:Label>,
                                cuenta con un crédito aprobado con PRESTADITO por la cantidad de
                                <asp:Label runat="server" ID="lblValorAPrestarEnPalabras_NotaEntrega"></asp:Label>
                                (<asp:Label runat="server" ID="lblValorAPrestar_NotaEntrega" CssClass="font-weight-bold"></asp:Label>)
                                solicitamos de su parte proceder con la entrega del vehículo al cliente en mención, así mismo se le pide entregar al Oficial de Prestadito la documentación
                                original y completa del automóvil con las siguientes características:
                            </p>
                        </div>
                        <div class="col-2"></div>
                        <div class="col-auto">
                            <table class="table table-sm table-bordered" style="width: 400px;">
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
                                        <asp:Label runat="server" ID="lblPlaca_NotaEntrega"></asp:Label></td>
                                </tr>
                            </table>
                        </div>
                        <div class="col-3"></div>
                        <div class="col-12">
                            <p>
                                En virtud de lo anterior se le emite esta <b>NOTA DE ENTREGA</b> y al mismo tiempo ratificamos nuestro 
                                compromiso de hacer el pago correspondiente en efectivo o en Cheque al señor:
                                <asp:Label runat="server" ID="lblNombreVendedorGarantia_NotaEntrega"></asp:Label>
                                en cinco (5) días hábiles mismos que serán para inscribir dicho vehículo en el Instituto de la Propiedad Mercantil a favor de Prestadito,
                                salvo que la documentación entregada no se encuentre completa y/o no pueda ser inscrito en el IP.
                            </p>
                            <p>
                                Y para los fines que estime conveniente, se le extiende la presente en la ciudad de 
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
                            <label class="mt-0 d-block">SELLO Y FIRMA</label>
                            <label class="mt-0 d-block">PRESTADITO S.A. DE C.V.</label>
                        </div>
                        <div class="col-3"></div>
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
    <script src="/Scripts/plugins/html2pdf/html2pdf.bundle.js"></script>
    <script src="/Scripts/plugins/qrcode/qrcode.js"></script>
    <script src="/Scripts/plugins/qrious/qrious.min.js"></script>
    <script>

        const idSolicitud = '<%=pcIDSolicitud%>';

        /* Incializar galerias */
        $("#divGaleriaGarantia").unitegallery({
            gallery_width: 900,
            gallery_height: 600
        });

        $("#divGaleriaInspeccionSeguroDeVehiculo").unitegallery({
            gallery_theme: "tilesgrid",
            tile_width: 300,
            tile_height: 194,
            grid_num_rows: 15
        });

        $("#divContenedorInspeccionSeguro").css('margin-top', '999px').css('display', 'none');
        $("#divInspeccionSeguroPDF").css('display', 'none');

        /* Información de los documentos */
        $('.lblDepartamento_Firma').text('<%=DepartamentoFirma%>');
        $('.lblCiudad_Firma').text('<%=CiudadFirma%>');
        $('.lblNumeroDia_Firma').text('<%=DiasFirma%>');
        $('.lblMes_Firma').text('<%=MesFirma%>');
        $('.lblAnio_Firma').text('<%=AnioFirma%>');


        $(document).ready(function () {

            InicializarCodigosQR();
        });

        function InicializarCodigosQR() {

            GenerarCodigoQR('qrCode_InspeccionSeguro');
        };

        /* Generar QR */
        function GenerarCodigoQR(idElemento) {

            let textQr = '<%=UrlCodigoQR%>';

            let qrcode = new QRCode(document.getElementById('' + idElemento + ''), {
                width: 70,
                height: 70
            });

            qrcode.makeCode(textQr);
        }

        function ExportToPDF(fileName, divContenedor, divPDF) {

            $("#Loader").css('display', '');

            const cotizacion = this.document.getElementById(divPDF);

            var opt = {
                margin: 0.3,
                filename: 'Solicitud_' + idSolicitud + '_' + fileName + '.pdf',
                image: { type: 'jpeg', quality: 1 },
                html2canvas: {
                    dpi: 192,
                    scale: 4,
                    letterRendering: true,
                    useCORS: false
                },
                jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' },
                pagebreak: { after: '.page-break', always: 'img' }
            };

            $("#" + divContenedor + ",#" + divPDF + "").css('display', '');
            $("body,html").css("overflow", "hidden");

            html2pdf().from(cotizacion).set(opt).save().then(function () {
                $("#" + divContenedor + ",#" + divPDF + "").css('display', 'none');
                $("body,html").css("overflow", "");

                $("#Loader").css('display', 'none');
            });
        }

        function EnviarCorreo(asunto, tituloGeneral, idContenidoHtml) {

            let contenidoHtml = $('#' + idContenidoHtml + '').html();

            $.ajax({
                type: "POST",
                url: "SolicitudesCredito_ImprimirDocumentacion.aspx/EnviarDocumentoPorCorreo",
                data: JSON.stringify({ asunto: asunto, tituloGeneral: tituloGeneral, contenidoHtml: contenidoHtml, dataCrypt: window.location.href }),
                contentType: "application/json; charset=utf-8",
                error: function (xhr, ajaxOptions, thrownError) {
                    MensajeError('No se pudo enviar el correo, contacte al administrador.');
                },
                success: function (data) {

                    data.d == true ? MensajeExito('El correo se envió correctamente') : MensajeError('No se pudo enviar el correo, contacte al administrador.');

                }
            });
        }

        function MensajeError(mensaje) {
            iziToast.error({
                title: 'Error',
                message: mensaje
            });
        }

        function MensajeExito(mensaje) {
            iziToast.success({
                title: 'Éxito',
                message: mensaje
            });
        }

        // prueba nuevo qr

        var qr;
        (function () {
            qr = new QRious({
                element: document.getElementById('qrCode_InspeccionSeguro'),
                size: 200,
                value: '<%=UrlCodigoQR%>'
            });
        })();

        function generateQRCode() {
            var qrtext = '<%=UrlCodigoQR%>';
            qr.set({
                foreground: 'black',
                size: 200,
                value: qrtext
            });
        }

    </script>
</body>
</html>

