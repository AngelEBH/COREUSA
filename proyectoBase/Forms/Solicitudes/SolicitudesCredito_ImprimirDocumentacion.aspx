<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_ImprimirDocumentacion.aspx.cs" Inherits="SolicitudesCredito_ImprimirDocumentacion" %>

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
    <style>
        html {
            background-color: rgb(255,255,255) !important;
        }

        .FormatoBotonesIconoCuadrado40 {
            position: absolute;
            background-color: white;
            border-style: solid;
            border-color: lightgray;
            border-width: 1px;
            margin: 0;
            white-space: normal !important;
            vertical-align: text-top;
            text-align: center;
            padding-left: 5px;
            padding-right: 5px;
            padding-top: 55px;
            padding-bottom: 5px;
            height: 95px;
            width: 80px;
            font-size: 12px;
            cursor: pointer;
            background-repeat: no-repeat;
            background-position-y: 10px;
            background-position-x: center;
            display: inline-block;
            text-wrap: normal;
            color: #808080;
        }

            .FormatoBotonesIconoCuadrado40:hover {
                background-color: #b3ecff;
                color: black;
            }

        .card {
            border: none;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }

        p {
            font-size: 15.5px !important;
            text-align: justify !important;
            text-justify: inter-word !important;
        }

        #divContratoPDF p {
            font-size: 11px !important;
            text-align: justify !important;
            text-justify: inter-word !important;
        }

        #divContratoPDF label {
            font-size: 11px !important;
        }

        .page-break {
            page-break-before: always;
        }

        .ug-thumbs-grid{
            left:50px !important;
        }
    </style>
</head>
<body class="EstiloBody">
    <form id="frmGuardarPreSolicitud" runat="server">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <div class="card m-0">
            <div class="card-header pb-1 pt-1">
                <div class="float-right p-1" id="Loader" style="display: none;">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>
                <h5>Imprimir documentación solicitud de crédito No. <span id="lblIdSolicitud" class="font-weight-bold" runat="server">330</span></h5>
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
                            <div class="col-lg-3 col-md-6 col-sm-3">
                                <label class="col-form-label">Producto</label>
                                <asp:TextBox ID="txtProducto" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-md-6 col-sm-3">
                                <label class="col-form-label">Monto a Financiar</label>
                                <asp:TextBox ID="txtMontoFinalAFinanciar" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-md-6 col-sm-3 col-6">
                                <label class="col-form-label">Plazo <span runat="server" id="lblTipoDePlazo"></span></label>
                                <asp:TextBox ID="txtPlazoFinanciar" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-lg-3 col-md-6 col-sm-3 col-6">
                                <label class="col-form-label">48 Cuotas <span runat="server" id="lblTipoDePlazoCuota"></span></label>
                                <asp:TextBox ID="txtValorCuota" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
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
                                    Compromiso Legal
                                </button>
                                <button type="button" id="btnConvenioComprayVenta" onclick="ExportToPDF('CONVENIO_DE_COMPRA_Y_VENTA_DE_VEHICULOS_PARA_FINANCIAMIENTO','divContenedorConvenioCyV','divConevionCyVPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    C. y V. de vehic. finan.
                                </button>
                                <a href="/Documentos/Recursos/INSPECCION%20DE%20VEHICULO.pdf" download>
                                    <button type="button" id="btnInspeccionVehiculo" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/inspection_40px.png');">
                                        Inspección de vehículo
                                    </button>
                                </a>
                                <button type="button" id="btnInspeccionSeguroDeVehiculo" onclick="ExportToPDF('INSPECCION_SEGURO_VEHICULO','divContenedorInspeccionSeguro','divInspeccionSeguroPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/inspection_40px.png');">
                                    Inspección seguro
                                </button>
                                <button type="button" id="btnTraspaso" onclick="ExportToPDF('TRASPASO','divContenedorTraspaso','divTraspasoPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/resume_40px.png');">
                                    Traspaso
                                </button>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row mb-0" id="divInformacionGarantia" runat="server" visible="false">
                    <div class="col-12">
                        <h6 class="border-bottom pb-2">Información de la garantía</h6>
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
                            <div class="col-sm-3">
                                <label class="col-form-label">Cilindraje</label>
                                <asp:TextBox ID="txtCilindraje" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Recorrido</label>
                                <asp:TextBox ID="txtRecorrido" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Transmisión</label>
                                <asp:TextBox ID="txtTransmision" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Tipo de combustible</label>
                                <asp:TextBox ID="txtTipoDeCombustible" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Serie 1</label>
                                <asp:TextBox ID="txtSerieUno" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
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

        <!-- //////// DOCUMENTOS //////// -->

        <!-- CONTRATO -->
        <div id="divContenedorContrato" style="margin-top: 999px; display: none;">
            <div class="card m-0" runat="server" visible="true" id="divContratoPDF" style="display: none;">
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
                                Nostros, <b>ERICK  GEOVANY  MOLINA  PADILLA</b>,
                                Casado,  Ingeniero  Industrial,  con  domicilio  en  la  ciudad  de  San  Pedro  Sula, Departamento de Cortés, quien actúan en su condición de Representante Legal de la Sociedad Mercantil denominada
                                <b>PRESTADITO S.A. de  C.V.</b> empresa domiciliada domiciliada  en  la  ciudad  de  San  Pedro  Sula,  departamento  de  Cortes,  llamada  en  adelante
                                <b>PRESTADITO</b> o <b>PRESTAMISTA</b>; y por otra parte el Sr(a) <span id="lblNombre_Contrato" class="font-weight-bold">ESMERLYN GEOVANNY MONGE RODRIGUEZ</span>,
                                mayor de edad, de nacionalidad <span id="lblNacionalidad_Contrato" class="font-weight-bold"></span>y de este domicilio, 
                                con identidad No. <span id="lblIdentidad_Contrato" class="font-weight-bold"></span>con domicilio y dirección en <span id="lblDireccion_Contrato"></span>,
                                llamado en adelante <b>EL CIENTE, PRESTATARIO y/o DEUDAR</b>, convienen celebrar el siguiente <b>CONTRATO DE CRÉDITO PARA COMPRA DE VEHICULO</b>
                                y acuerdan lo estipulado  en  las  siguientes  clausulas:
                                <b>PRIMERO:   OBJETO  DEL  CONTRATO.-  EL  CLIENTE </b>declara  recibir  en  este  acto   de <b>PRESTADITO</b>, un préstamo por la cantidad de 
                                <span id="lblMontoPrestamoEnPalabras_Contrato" class="font-weight-bold"></span>(<span id="lblMontoPrestamo_Contrato"></span>) de los cuales se destinaran
                                <span id="lblMontoParaCompraVehiculoEnPalabras_Contrato" class="font-weight-bold"></span>para compra del vehiculo (<span id="lblMontoParaCompraVehiculo_Contrato"></span>)
                                y <span id="lblMontoParaCompraSeguroYGPSEnPalabras_Contrato" class="font-weight-bold"></span>(<span id="lblMontoParaCompraSeguroYGPS_Contrato"></span>)
                                para compra de modulo de monitoreo por GPS y <span id="lblMontoGastosDeCierre_Contrato"></span>por concepto de gastos de cierre y papeleria todo en monda de curso legal en Honduras.
                                <b>-SEGUNDO: CONDICIONES DEL FINANCIAMIENTO.-</b> El préstamo se facilita bajo las siguientes condiciones: 
                                <b>A) DESTINO: EL CLIENTE </b>acepta, reconoce y autoriza que la cantidad recibida en préstamo será para compra de: 
                            </p>
                            <p>
                                <b>Vehiculo automotor marca <span id="lblMarca_Contrato"></span>tipo <span id="lblTipoVehiculo_Contrato"></span>modelo <span id="lblModelo_Contrato"></span>
                                    año <span id="lblAnio_Contrato"></span>color <span id="lblColor_Contrato"></span>con cilindraje <span id="lblCilindraje_Contrato"></span>
                                    y con registro de matricula con placa numero <span id="lblMatricula_Contrato"></span>con VIN numero <span id="lblVIN_Contrato"></span>
                                    y numero de motor <span id="lblNumeroMotor_Contrato"></span>
                                </b>
                            </p>
                            <p>
                                Mismo que será desembolsado por <b>PRESTADITO</b> a la persona que distribuya o sea propietario del vehículo y este ultimo deberá de entregárselo al Cliente cuando sea autorizado por <b>PRESTADITO</b>. 

                                <b>- B) COMISIONES.-TASAS DE INTERES.-COSTO ANUAL TOTAL.- EL CLIENTE</b> se obliga a pagar a <b>PRESTADITO</b>, a partir de esta fecha una 
                                tasa de interés simple  del <b><span id="lblTasaInteresSimple_Contrato"></span>% PORCIENTO MENSUAL </b>,
                                amartizando capital mas intereses basada en la regla del 78 misma que sera pagadero <span id="lblTipoDePlazo_Contrato"></span>en moneda de curso legal en Honduras,
                                sobre el saldo total de la deuda.
                                <b>Por la falta de pago</b> a su vencimiento de cualquiera de los abonos a capital, intereses y/o recargos, <b>EL CLIENTE</b> pagará intereses moratorios
                                del <b>4.31% PORCIENTO MENSUAL</b> sobre el saldo de capital vencido, por razón de daños y perjuicios hasta que se cancele la totalidad de la mora, sin que deba considerarse prorrogado el plazo
                                <b>.-COSTO ANUAL TOTAL:</b> El costo anual total del préstamo (CAT) es de <span id="lblCAT_Contrato">20.04%</span> incluye el cobro
                                y pago del capital, los intereses ordinarios, y bajo la condición que <span>EL CLIENTE</span> cumpla con sus obligaciones en las formas y plazos detallados.                                
                            </p>
                            <p>
                                <b>C) FORMA  Y PLAZO DEL FINANCIAMENTO:</b>
                                <b>Anticipo por concepto de prima de: <span id="lblMontoPrima_Contrato"></span>, plazo de <span id="lblPlazo_Contrato"></span><span id="lblFrecuenciaPago_Contrato"></span>
                                    valor de la cuota: <span id="lblValorCuotaPalabras_Contrato"></span>(<span id="lblValorCuota_Contrato"></span>)
                                </b>
                                más <span id="lblPlazoGPS_Contrato"></span>cuotas de Servicio de monitoreo por GPS por un valor de <span id="lblValorCuotaGPSPalabras_Contrato"></span>y 
                                <span id="lblPlazoSeguro_Contrato"></span>cuotas de <span id="lblValorCuotaSeguroPalabras_Contrato"></span>(<span id="lblValorCuotaSeguro_Contrato"></span>) 
                                por concepto de seguro de vehiculo debiendo hacer efectivo el pago de la <b>primera cuota el <span id="lblFechaPrimerCuota_Contrato"></span></b>
                                y así sucesivamente de forma <span id="lblPlazoPago_Contrato"></span>
                                hasta la completa cancelación de la deuda en caso de cumplir el 100% del financiamiento sin abonar a capital y sin sumar gastos por cobranza y/o moratorios o cualquiera otro gasto generado por gestiones de recuperacion o incumplimento seria de
                                <span id="lblMontoPrestamo2_Contrato"></span>
                            </p>
                            <p>
                                <b>D) DE LA FORMA, DE LA MONEDA  Y LUGAR DE PAGO:</b>
                                Los contratantes acuerdan que: 
                                <b>I)</b> Los abonos se harán primero a gastos, y cargos que pudieran haberse causado, luego los intereses, y el saldo, si lo hubiera, a capital;
                                <b>- II)</b> El pago del préstamo se hará en la moneda pactada y en efectivo.
                                <b>- III)</b> El pago se realizará conforme a lo establecido en el plan de pagos, en el caso que la fecha de pago sea día feriado,  entonces deberá realizarse el día hábil inmediato anterior, en las oficinas, agencias, sucursales y ventanillas de <b>PRESTADITO</b>, o en cualquier otra institución tercerizada que se designe oportunamente en virtud de convenios de cobro de cartera. 

                                <b>- E) PAGO ANTICIPADO:</b>
                                En caso de pago total de la obligación antes de su vencimiento, <b>EL CLIENTE</b> deberá pagar una comisión de prepago del dos por  ciento (2%) sobre el saldo adeudado, y si es un pago parcial a capital superior al diez por ciento (10%) del monto adeudado, también pagará dicha comisión calculada sobre el monto a pagar. Esta condición aplicara únicamente cuando el saldo del capital adeudado exceda cien mil dólares ($100,000.00) o su equivalente en lempiras, o los fondos sean provenientes de una institución que penalice a <b>PRESTADITO</b> por pago anticipado, cualquiera de las dos o ambas conjuntamente. 
                                
                                <b>- F) PROPIEDAD DEL VEHICULO.</b>
                                Mientras no se haya cancelado la totalidad del Préstamo, será dueño del vehículo <b>PRESTADITO</b>, el cliente será considerado como poseedor y esta posesión esta condicionada, es decir si el cliente está cumpliendo con las obligaciones contraídas en este contrato, de lo contrario <b>PRESTADITO</b> podrá a su discreción retirarlo. 
                            </p>
                            <p>
                                <b>- G) OTROS GASTOS:</b>
                                Los gastos que se incurra por matricula, mantenimiento, reparación y todas las relacionadas para conservación del vehículo en perfecto estado, deberá ser pagadas por el <b>CLIENTE</b>, así como también los gastos que se ocasione en la recuperación del vehículo en caso de mora. 
                                <b>- TERCERO: AUTORIZACIONES ESPECIALES: EL CLIENTE</b>
                                por este acto, en tanto no haya cumplido con el pago total de su obligación, autoriza a <b>PRESTADITO</b> expresamente y sin ser necesario la notificación previa para: 
                                <b>A)</b> Vender, Ceder o de cualquier otra forma traspasar, o celebrar contratos de participación, de descuentos con relación al crédito y derechos consignados en este documento o títulos valores relacionados a este mismo; 
                                <b>B)</b> Autorizar a <b>PRESTADITO</b> para que en cualquier tiempo pueda acceder a la información de la Central de Riesgos de la Comisión Nacional de Bancos y Seguros u otra central de riesgo pública o privada, para gestionar y conocer la situación crediticia de <b>EL CLIENTE</b> frente a las demás instituciones del sistema financiero nacional. 
                                <b class="page-break">- C) EL CLIENTE</b> Autoriza de manera Irrevocable, a que <b>PRESTADITO</b> pueda entrar en su domicilio, para solo efecto de retirar el vehículo comprado con este préstamo, o que lo retire de una tercera persona sin necesidad de intervención judicial, esta cláusula solo se ejecutara en caso de mora de 2 o más cuotas vencidas y mientras no haya sido cancelado el total adeudado. 
                                <b>- CUARTO: OBLIGACIONES GENERALES.- EL CLIENTE</b> durante la vigencia del presente contrato también se obliga a:
                                <b>A)</b> Permitir que <b>PRESTADITO</b> ejerza los controles que juzgue convenientes, para asegurarse que los fondos de este crédito se inviertan en los fines que se han indicado anteriormente y condiciones que se estipulan en este contrato.
                                <b>- B) DE LA GARANTIA:</b>
                                En calidad de Garantía para el Cumplimiento de la presente obligación <b>El CLIENTE</b> firmara una <b>PAGARE</b>  sin protesto,  así como también da en propiedad  a <b>PRESTADITO</b>  el vehículo comprado  con el dinero objeto del presente préstamo, quedándose <b>PRESTADITO</b> con la documentación original del vehículo y el <b>CLIENTE</b> en posesión del vehículo del cual será responsable mientras se encuentre en su poder y no se haya cancelado el precio total pactado para la terminación del presente contrato. 
                                Sin perjuicio de la <b>designación de garantías fiduciarias como ser Menaje de Hogar y demás bienes pertenecientes AL CLIENTE</b>, por lo que está terminantemente prohibido para el <b>CLIENTE</b> enajenar, vender, permutar, donar, gravar, prestar o dar en prenda el vehículo dado  en  propiedad,  sin  la  autorización  por  escrito  otorgada  por  <b>PRESTADITO</b>,  el  incumplimiento  de  las  prohibiciones  faculta  a <b>PRESTADITO</b> a retirar el vehículo.
                                Para el menaje se formalizará el Inventario de estos, este que pasara a formar parte del presente contrato.
                                <b>C)</b> Suscribir y a mantener un seguro para vehículos en lempiras moneda de curso legal en Honduras; mientras esté vigente la deuda, por la cuantía y condiciones que señale <b>PRESTADITO</b>, con una compañía aseguradora; siendo entendido que <b>EL CLIENTE</b> deberá endosar a favor de <b>PRESTADITO</b> la respectiva póliza de seguro, o a favor de la persona natural o jurídica a cuyo nombre se traspase el presente crédito, hasta la total cancelación del saldo pendiente de pago por la deuda. <b>PRESTADITO</b> podrá pagar y cargar al préstamo las primas de seguro, si <b>EL CLIENTE</b> no lo renueva y paga a los treinta (30) días previos al vencimiento de la póliza de seguro respectiva, sin que la acción del pago o cargo sea obligatorio para <b>PRESTADITO</b>, quien no asumirá ni incurrirá en responsabilidad por no hacer el pago de las primas de seguro. 
                                <b>D)</b> Mantenerse al día en el pago de los impuestos que graven a <b>EL CLIENTE</b> o al <b>VEHÍCULO</b> dado en garantía.
                                <b>- E)</b> Cuidar como buen padre de familia el vehículo dado en garantía, mientras se encuentre en su poder y no se haya cancelado el precio total pactado para la terminación del presente contrato, quedando a su cargo los riesgos de dicho bien mueble por lo que será responsable de la perdida, destrucción o deterioro que sufra aun por caso fortuito o fuerza mayor. 
                                <b>- F)</b> Mantener la licencia de conducir vigente, mientras no se haya cancelado la totalidad del préstamo, en caso que <b>EL CLIENTE</b> haya solicitado excepción al momento de otorgarse el Préstamo por no  poseer  licencia  vigente,  entonces  dispondrá  solamente  de  un  máximo  de  40  días  para  presentar  la  Licencia  de  conducir  a <b>PRESTADITO</b>, caso contrario <b>AUTORIZA</b> anticipadamente a <b>PRESTADITO</b> a que se le retire, en calidad de custodia, el vehiculo hasta que presente la licencia de conducir aun y cuando sus cuotas estén al día. 
                                La excepción anterior <b>no faculta al CLIENTE</b> a conducir el vehiculo sin su respectiva Licencia emitida por la Dirección Nacional de Transito, ni a prestar a quien no tenga dicho documento, ya que <b>PRESTADITO</b> respeta las leyes hondureñas. 
                                <b>- QUINTO: DE LOS DEBERES DEL CLIENTE: </b>
                                Se conviene que, desde la fecha de otorgamiento de este contrato, hasta la fecha en que se pague el total de las obligaciones pendientes con <b>PRESTADITO, EL CLIENTE</b> deberá informar siempre, por vía telefónica o escrita y a la brevedad posible, las siguientes acciones: 
                                <b>1)</b> Contraiga deudas con otras instituciones financieras, no financieras, puestos de bolsa, proveedores, filiales y otros. <b>EL CLIENTE</b> aprueba libre y voluntariamente por ser válidas, todas las condiciones fijadas en este inciso, por entender que de tal manera <b>PRESTADITO</b> se asegura de la solvencia de <b>EL CLIENTE</b> y del pago del crédito otorgado. 
                                <b>- SEXTO: DE LAS MODIFICACIONES DEL CONTRATO.- PRESTADITO</b> comunicará a
                                <b>EL CLIENTE: 1)</b> De manera general y sin necesidad de especificarlo individualmente, las condiciones contractuales pactadas, por cualquier medio impreso de circulación nacional, o el medio de comunicación que las partes hayan designado, en los casos de los efectos de la aplicación de la vigencia de una ley, con 30 días calendario de anticipación a la aplicación de dicho cambio; 
                                <b>2)</b> Para el caso que las tasas de intereses y otros cargos sea modificada, se aplicará conforme a un factor variable que considera la tasa de interés que se concede para los depósitos a plazo, más el costo de la intermediación y sumándole un diferencial del veinte por ciento. La tasa de interés se revisará cada 3 meses. En el caso que las tasas de interés sean reguladas por el Banco Central de Honduras conforme al artículo 55 de la Ley del Sistema Financiero, se aplicará la tasa máxima permitida por dicha Institución, o la que fije y aplique <b>PRESTADITO</b>, notificándolo a <b>EL CLIENTE</b> con 15 días calendario de anticipación por lo menos por cualquiera de los medios de comunicación descritos en este contrato o los otros establecidos por la Ley, siendo entendido que cualquier ajuste resultante de la modificación de la tasa de interés será cubierto por <b>EL CLIENTE</b> quedando <b>PRESTADITO</b> autorizado para efectuar y cobrar tales ajustes y modificar la cuota quincenal o mensual del financiamiento de acuerdo al plazo que reste para la cancelación del mismo, así mismo las partes acuerdan incorporar como vinculante el principio <b>“ceteris paribus”</b>, respecto a modificaciones atinentes al contrato o los convenios incorporados. 
                                <b>-SEPTIMO: RECLAMOS.-</b>Cuando se presente algún evento por el cual <b>EL CLIENTE</b> desee hacer un reclamo, se dispondrá de un plazo de 10 días hábiles para realizarlo, transcurrido éste, es entendido que caduca su derecho para reclamar y se declara vencido. Cuando sea reclamos por cuestiones de garantía deberá presentarlas al distribuidor autorizado y en caso de ser bienes usados no podrá presentar reclamos después de 30 dias de realizada la compra, es entendido que <b>PRESTADITO</b> no está obligado a resolver cuestiones de garantía puesto que solo es quien financia la compra.
                                <b>- OCTAVO: DEL VENCIMIENTO ANTICIPADO DEL PLAZO DE PAGO.- </b>Además de los casos establecidos por la ley, <b>PRESTADITO</b> podrá dar por vencido el plazo establecido para el pago del préstamo concedido en este contrato, y en consecuencia exigir el pago inmediato del saldo del capital, intereses, comisiones, recargos y gastos, ya sea por la vía judicial o extra judicial, por cualquiera de los siguientes eventos: 
                                <b>a)</b> Por falta de pago de dos o más de las cuotas pactadas, de los intereses, o de cualquier otro cargo pendiente a favor de <b>PRESTADITO</b>;
                                <b>b)</b> Por el conocimiento de la ejecución judicial iniciada por terceros, o por el mismo <b>PRESTADITO</b>,  en contra de <b>EL CLIENTE</b>, originada por otros créditos;
                                <b>c)</b> Por no destinar el presente préstamo para el fin o fines para los cuales ha sido concedido;
                                <b class="page-break">d)</b> Por la declaración del estado de suspensión de pagos, de quiebra o de concurso de <b>EL CLIENTE</b>, así como por su inhabilitación para el ejercicio del comercio, o por el ejercicio de acción penal en su contra o de su representante legal que derivare en sentencia de privación de libertad;
                                <b>e)</b> Por el incumplimiento o negativa por parte de <b>EL CLIENTE</b> a proporcionar la información requerida por <b>PRESTADITO</b> en forma escrita; 
                                <b>f)</b> Por actuación fraudulenta o haber proporcionado a <b>PRESTADITO</b> información o datos falsos o incompletos para obtener el préstamo; 
                                <b>g)</b> Por ser del conocimiento de <b>PRESTADITO</b>, la existencia de obligaciones de <b>EL CLIENTE</b> pendientes de pago con el Estado, en tal cantidad que a su criterio ponga en peligro la recuperación de los adeudos debido a la preferencia del Estado para obtener el pago a su favor antes que <b>PRESTADITO</b>; 
                                <b>h)</b> El incumplimiento de parte de <b>EL CLIENTE</b> de cualquiera de las obligaciones contraídas en este contrato. 
                                <b>- NOVENO: COBROS EXTRAJUDICIALES.-</b> En caso de ser necesarias las gestiones de cobranzas extrajudiciales por la mora en  el  pago  o  el  vencimiento  anticipado  del  contrato,  estas  se  realizarán  de  la  siguiente  manera:
                                <b>1)</b> Para  Mora  de  1  a  180  días: alternativamente podrán ser llamadas telefónicas, correos electrónicos, mensajes por cualquier medio electrónico, visitas por gestores, cartas de cobro escritas solicitando el pago y dirigidas a las direcciones indicadas. Estas gestiones tendrán un costo de doscientos cincuenta lempiras (L250.00), cargados al estado de cuenta del préstamo otorgado, son acumulables por cada cuota vencida y serán pagados por <b>EL CLIENTE</b> en todos los casos y sin excepción;
                                <b>2)</b> Si su caso fuere trasladado a Profesionales del Derecho, cuyas gestiones iniciales podrán ser: llamadas telefónicas, envió de correos electrónicos, cartas de cobro escritas, y visitas, causaran el cobro de honorarios 
                                profesionales según el Arancel del Profesional del Derecho vigente, y se calculará sobre el capital, intereses, comisiones, cargos y seguros en mora, tal como lo establece el artículo 1432 del Código Civil.- En caso de ser perseguida la deuda por proceso Judicial, se cargaran igualmente  los  gastos  ocasionados  por  costas  durante  dicho  proceso.  <b>PRESTADITO</b>  podrá  asignar  a  una  empresa  o  Agencia  de Cobranzas y/o Recuperaciones para que realice estas labores de cobro desde el día uno de atraso en el estado de cuenta lo cual es aceptado por el deudor.
                                <b>- DÉCIMO: ACCIONES JUDICIALES.- </b>
                                En caso de mora o vencimiento anticipado del contrato, dará lugar para que <b>PRESTADITO</b> ejerza las acciones judiciales correspondientes, quedando obligado el cliente a pago de gastos y honorarios que ocasione el procedimiento judicial. Así como para determinar el saldo adeudado El estado de cuenta certificado por el contador de <b>PRESTADITO</b> o de quien haya adquirido los derechos, hará fe en juicio para establecer el saldo a cargo de <b>EL CLIENTE</b> y Constituirá junto con el presente contrato título ejecutivo, sin necesidad de reconocimiento de firma ni de otro requisito previo alguno, según lo establecido en la ley del sistema financiero. En caso de ejecución de la presente obligación las partes nos sometemos a la jurisdicción y competencia de los Juzgados  de  San  Pedro  Sula,  Cortés.
                                <b>.-  DÉCIMO  PRIMERA:  MEDIOS  PARA  COMUNICACIONES.- EL  CLIENTE  y  PRESTADITO</b> establecen y a la vez autorizan, que para las distintas notificaciones que se deban hacer conforme a lo estipulado por este contrato o por lo dispuesto por la ley, se harán efectivas a través de uno solo de los siguientes medios:
                                <b>A)</b> Correspondencia ordinaria escrita dirigida a las direcciones indicadas en el preámbulo de este contrato;
                                <b>B)</b> Notificación por la vía electrónica a su correo electrónico <span id="lblCorreo_Contrato"></span>
                                <b>C)</b> Notificación mediante cualquier red sociales que pudiese pertenecer al Cliente,
                                <b>D)</b> o a las direcciones indicadas en cualquiera de los documentos suscritos con <b>“ PRESTADITO ”</b>. Cualquier cambio de dirección o número telefónico deberá notificarse fehacientemente, con una anticipación razonable a <b>PRESTADITO</b> y hasta entonces se considera efectiva.
                                <b>.-DÉCIMO SEGUNDA: DE LAS AUTORIZACIONES ESPECIALES.  EL  CLIENTE </b>otorga  de  manera  expresa,  voluntaria  e  irrevocable  su  consentimiento  para  que  en  caso  de  mora, <b>PRESTADITO</b> o sus representantes puedan ingresar a su domicilio a retirar el vehículo, y por lo tanto lo exime de toda responsabilidad que pueda incurrir según el artículo 99 de la Constitución de la Republica.
                                Así como faculta a <b>“ PRESTADITO ”</b>, sus distintas dependencias, así como también a su personal, que mediante visitas a su domicilio se le puedan presentar y ofrecer las diferentes propuestas de negocio, servicios, catálogos de nuevos productos; a su vez, faculta otros canales, sean estos telefónicos o electrónicos, a que se comuniquen y a que le informen en los días de semana, así como también en los días llamados vacaciones, o festivos, en los diferentes horarios abiertos, incluso fin de semana, exonerándole de cualquier perjuicio a la empresa o de ser estas visitas catalogadas como “hostigamiento”.
                                <b>- El CLIENTE:</b> autoriza de manera expresa y voluntaria que en caso de que PRESTADITO retire el vehículo, pueda ser subastado al mejor postor y el dinero recibido de la misma se abonara a la deuda, si existiera un excedente se le dará al <b>CLIENTE</b> y en caso que no cubriese el total adeudado, <b>PRESTADITO</b> se reserva el derecho de ejercer acciones legales contra el <b>CLIENTE</b> por el pago de saldo total adeudado, que incluye capital, intereses y otros cargos gastos o cargos que incurra.
                                <b>- DÉCIMO TERCERA: COMPROBACION DE HABER RECIBIDO INSTRUCCIÓN Y ORIENTACION DEL PRODUCTO Y ENTREGA DE COPIA DEL CONVENIO Y PLAN DE PAGO.- EL CLIENTE</b> por este acto acepta que previo a la celebración de este contrato, ha recibido toda la orientación y explicación necesaria sobre las condiciones del convenio, las consecuencias legales y judiciales de su incumplimiento, así como que ha recibido una copia íntegra de este documento y del plan  de pagos  respectivo.
                                .-  Finalmente  las partes  declaramos  que es cierto  todo  lo anteriormente  expresado,  y que por ser ello lo convenido,  aceptamos  libre  y  voluntariamente,  todas  estipulaciones,  condiciones  y  cláusulas  contenidas  en  el  presente  contrato  de préstamo.
                                En fe de lo cual firmamos en la Ciudad de <span id="lblCiudad_Contrato">SAN PEDRO SULA</span>, a los <span id="lblDia_Contrato">21</span> días del mes de <span id="lblMes_Contrato">septiembre</span> del año <span id="lblAño_Contrato">2020</span>.
                            </p>
                        </div>
                    </div>
                    <!-- Firma del cliente -->
                    <div class="row">
                        <div class="col-1"></div>
                        <div class="col-5 text-center">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <asp:Label class="mt-0 d-block" runat="server" ID="lblNombreFirma_Contrato"></asp:Label>
                            <asp:Label class="mt-0 d-block" runat="server" ID="lblIdentidadFirma_Contrato"></asp:Label>
                            <label class="mt-0 d-block">EL CLIENTE</label>
                        </div>
                        <div class="col-5 text-center">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">ERICK GEOVANY MOLINA PADILLA</label>
                            <label class="mt-0 d-block">1301-1980-00105</label>
                            <label class="mt-0 d-block">PRESTADITO</label>
                        </div>
                        <div class="col-1"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- PAGARÉ -->
        <div id="divContenedorPagare" style="margin-top: 999px; display: none;">
            <div class="card m-0" runat="server" visible="true" id="divPagarePDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="/Imagenes/LogoPrestadito.png" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">PAGARÉ POR <asp:Label runat="server" ID="lblMontoTitulo_Pagare"></asp:Label></h5>
                            <hr />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-12">
                            <p>
                                <b>YO, <asp:Label runat="server" ID="lblNombre_Pagare"></asp:Label>,</b>
                                mayor de edad, de profesión <asp:Label runat="server" ID="lblProfesion_Pagare"></asp:Label>,
                                con tarjeta de identidad <asp:Label runat="server" ID="lblIdentidad_Pagare"></asp:Label> y con domicilio
                                en <asp:Label runat="server" ID="lblDireccion_Pagare"></asp:Label>,
                                actuando en condición personal, acepto que <b>DEBO y PAGARÉ</b> incondicionalmente <b>SIN PROTESTO,</b> y a la orden de <b>PRESTADITO S.A. de C.V.</b>,
                                la cantidad de <asp:Label runat="server" ID="lblMontoPalabras_Pagare"></asp:Label> (<asp:Label runat="server" ID="lblMontoDigitos_Pagare"></asp:Label>).
                                Dicha cantidad será pagada el día ___ del mes de ____________ del año _____, en las oficinas, agencias, sucursales y ventanillas de <b>PRESTADITO S.A. de C.V.</b>.

                                La cantidad consignada en este PAGARE devengará, a partir de esta fecha, una tasa de interés fluctuante del <asp:Label runat="server" ID="lblPorcentajeInteresFluctuante_Pagare"></asp:Label>% PORCIENTO MENSUAL,
                                sobre el saldo total de la deuda, a pagar mensualmente. En caso de mora, que se producirá por la falta de pago al vencimiento tanto del capital o de los intereses,
                                dará derecho a <b>PRESTADITO S.A. de C.V.</b> a exigir el pago de intereses moratorios del <asp:Label runat="server" ID="lblInteresesMoratorios_Pagare"></asp:Label>% PORCIENTO MENSUAL;
                                a su vez, en caso de ejecución legal de la presente obligación, me someto a la jurisdicción que establezca <b>PRESTADITO S.A. de C.V.</b>,
                                quedando incorporadas en este documento todas las disposiciones del Código de Comercio.
                                En fe de lo cual, firmo (amos) en la ciudad de SAN PEDRO SULA departamento de CORTES, a los 18 días del mes de septiembre del año 2020.
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

        <!-- COMPROMISO LEGAL -->
        <div id="divContenedorCompromisoLegal" style="margin-top: 999px; display: none;">
            <div class="card m-0" runat="server" visible="true" id="divCompromisoLegalPDF" style="display: none;">
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
                                <b>YO, <asp:Label runat="server" ID="lblNombreCliente_CompromisoLegal"></asp:Label>,</b>
                                acepto haber adquirido un préstamo en efectivo con la empresa <b>PRESTADITO S.A. de C.V.</b>,
                                financiamiento otorgado a <asp:Label runat="server" ID="lblCuotas_CompromisoLegal"></asp:Label>, para la compra de contado de un Vehículo Automotor,
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
                                Me doy por enterado que el vehículo es usado y está en buen estado y se entrega tal cual esta,
                                y hago constar que lo vi, revisé y lo probé antes de comprarlo y es por eso que estoy satisfecho con dicha revisión.
                                <br />

                                El seguro de daños que incluye el préstamo es válido única y exclusivamente
                                durante el tiempo del financiamiento, siempre y cuando las cuotas estén al día.
                                <br />

                                Doy fe de lo anterior y de recibir el vehículo y para esto firmo de forma libre y espontáneamente la presente Constancia
                                en la ciudad de San Pedro Sula, departamento de Cortes, a los 18 días del mes de septiembre del año 2020
                            </p>
                        </div>
                    </div>
                    <!-- Firma del cliente -->
                    <div class="row">
                        <div class="col-1"></div>
                        <div class="col-6 text-center mt-3">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <asp:Label class="mt-0 d-block" runat="server" ID="lblNombreFirma_CompromisoLegal"></asp:Label>
                        </div>
                        <div class="col-4 text-center mt-3">
                            <asp:Label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;" runat="server" ID="lblIdentidadFirma_CompromisoLegal"></asp:Label>
                            <label class="mt-0 d-block">No. Identidad</label>
                        </div>
                        <div class="col-1"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- CONVENIO DE REGULACION DE COMPRA Y VENTA DE VEHICULOS PARA FINANCIAMIENTO -->
        <div id="divContenedorConvenioCyV" style="margin-top: 999px; display: none;">
            <div class="card m-0 pt-4" runat="server" visible="true" id="divConevionCyVPDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
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
                                en este convenio se denominara <b>LA EMPRESA</b> y <b>AL SEÑOR</b> <asp:Label runat="server" ID="lblNombreCliente_ConvenioCyV"></asp:Label>
                                mayor de edad, <asp:Label runat="server" ID="lblNacionalidad_ConvenioCyV"></asp:Label>, <asp:Label runat="server" ID="lblEstadoCivil_ConvenioCyV"></asp:Label>,
                                con tarjeta de identidad número <asp:Label runat="server" ID="lblIdentidad_ConvenioCyV"></asp:Label> y con domicilio en la ciudad de <asp:Label runat="server" ID="lblCiudadCliente_ConvenioCyV"></asp:Label>,
                                quien actúa en calidad de propietario de un vehículo marca <asp:Label runat="server" ID="lblMarca_ConvenioCyV"></asp:Label>, modelo <asp:Label runat="server" ID="lblModelo_ConvenioCyV"></asp:Label>,
                                año <asp:Label runat="server" ID="lblAnio_ConvenioCyV"></asp:Label>, con motor: <asp:Label runat="server" ID="lblSerieMotor_ConvenioCyV"></asp:Label>,
                                y con chasis: <asp:Label runat="server" ID="lblSerieChasis_ConvenioCyV"></asp:Label> tipo <asp:Label runat="server" ID="lblTipoVehiculoConvenioCyV"></asp:Label>
                                color <asp:Label runat="server" ID="lblColor_ConvenioCyV"></asp:Label>, Placa: <asp:Label runat="server" ID="lblMatricula_ConvenioCyV"></asp:Label>,
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
                            <p class="page-break mt-5 pt-5">
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
                                El presente convenio se firma en duplicado en la Ciudad de San Pedro Sula, Cortes a los 29 días del mes de febrero del año 2020.
                            </p>
                        </div>
                    </div>
                    <!-- Firma del cliente -->
                    <div class="row mt-5 pt-5">
                        <div class="col-1"></div>
                        <div class="col-5 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Por la Empresa PRESTADITO S.A. de C.V.</label>
                            <label class="mt-0 d-block">ERICK GEOVANY MOLINA PADILLA</label>
                        </div>
                        <div class="col-5 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Por el cliente</label>
                            <asp:Label runat="server" ID="lblNombre_ConvenioCyV" class="mt-0 d-block"></asp:Label>
                        </div>
                        <div class="col-1"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- INSPECCIÓN DEL SEGURO -->
        <div id="divContenedorInspeccionSeguro">
            <div class="card m-0" runat="server" visible="true" id="divInspeccionSeguroPDF">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="/Imagenes/LogoPrestadito.png" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">INSPECCIÓN SEGURO DE VEHÍCULO</h5>
                            <hr />
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12">
                            <table class="table table-bordered border-dark" style="border-width: 1px;">
                                <tr>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Asegurado</th>
                                    <td colspan="5" class="pt-0 pb-0">Delcy Alexandra Caceres Guardado</td>
                                </tr>
                                <tr>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Marca</th>
                                    <td class="pt-0 pb-0"><asp:Label runat="server" ID="lblMarca_InspeccionSeguro"></asp:Label></td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Modelo</th>
                                    <td class="pt-0 pb-0"><asp:Label runat="server" ID="lblModelo_InspeccionSeguro"></asp:Label></td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Año</th>
                                    <td class="pt-0 pb-0"><asp:Label runat="server" ID="lblAnio_InspeccionSeguro"></asp:Label></td>
                                </tr>
                                <tr>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Tipo</th>
                                    <td class="pt-0 pb-0"><asp:Label runat="server" ID="lblTipoDeVehiculo_InspeccionSeguro"></asp:Label></td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Kilometraje</th>
                                    <td class="pt-0 pb-0"><asp:Label runat="server" ID="lblRecorrido_InspeccionSeguro"></asp:Label></td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Placa</th>
                                    <td class="pt-0 pb-0"><asp:Label runat="server" ID="lblMatricula_InspeccionSeguro"></asp:Label></td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-12 pl-0 align-items-center justify-content-center justify-items-center">
                            <div style="max-width:794px !important; min-width:794px !important;" >
                            <!-- Div donde se muestran las imágenes de la garantía-->
                            <div id="divGaleriaInspeccionSeguroDeVehiculo" runat="server">
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- TRASPASO -->
        <div id="divContenedorTraspaso" style="margin-top: 999px; display: none;">
            <div class="card m-0" runat="server" visible="true" id="divTraspasoPDF" style="display: none;">
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
                                <b>YO, <asp:Label runat="server" ID="lblNombreCliente_Traspaso"></asp:Label>,</b>
                                mayor de edad, <asp:Label runat="server" ID="lblNacionalidad_Traspaso"></asp:Label>,
                                con tarjeta de identidad <asp:Label runat="server" ID="lblIdentidad_Traspaso"></asp:Label> y con domicilio
                                en <asp:Label runat="server" ID="lblDireccion_Traspaso"></asp:Label>,
                                en mi condición de propietario, por medio de este documento hago formal el traspaso del vehiculo que se describe de la forma siguiente
                            </p>

                            <div class="row">
                                <div class="col-6">
                                    <table>
                                        <tr>
                                            <th>
                                                Marca:
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblMarca_Traspaso"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Modelo:
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblModelo_Traspaso"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Motor:
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblSerieMotor_Traspaso"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Año:
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblAnio_Traspaso"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Cilindraje:
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblCilindraje_Traspaso"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>                                    
                                </div>
                                <div class="col-6">
                                    <table>
                                        <tr>
                                            <th>
                                                Tipo:
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblTipoDeVehiculo_Traspaso"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Color:
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblColor_Traspaso"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Chasis:
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblSerieChasis_Traspaso"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>
                                                Chasis:
                                            </th>
                                            <td>
                                                <asp:Label runat="server" ID="lblMatricula_Traspaso"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>                                    
                                </div>
                            </div>
                            <p>
                                El cual cedo todos los deberes y derechos que antes ejercía sobre el vehículo antes mencionado a PRESTADITO SA, con Numero de RTN 0501-9016-811399, aceptando que el vehículo es usado y se encuentra de su entera satisfacción sin garantía alguna.
                                Y para seguridad y constancia de las autoridades firmo en la ciudad de SAN PEDRO SULA, a los 29 días del mes de febrero del año 2020.
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

    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tilesgrid/ug-theme-tilesgrid.js"></script>
    <script src="/Scripts/plugins/html2pdf/html2pdf.bundle.js"></script>
    <script>

        $("#divGaleriaGarantia").unitegallery({
            gallery_width: 900,
            gallery_height: 600
        });

        $("#divGaleriaInspeccionSeguroDeVehiculo").unitegallery({
            gallery_theme: "tilesgrid",
            tile_width: 280,
            tile_height: 250
        });

        $("#divContenedorInspeccionSeguro").css('margin-top', '999px').css('display', 'none');
        $("#divInspeccionSeguroPDF").css('display', 'none');

        function ExportToPDF(fileName, divContenedor, divPDF) {

            $("#Loader").css('display','');

            const cotizacion = this.document.getElementById(divPDF);

            var opt = {
                margin: 0.3,
                filename: fileName + '.pdf',
                image: { type: 'jpeg', quality: 1 },
                html2canvas: {
                    dpi: 192,
                    scale: 4,
                    letterRendering: true,
                    useCORS: false
                },
                jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' }
            };

            $("#" + divContenedor + ",#" + divPDF + "").css('display', '');
            $("body,html").css("overflow", "hidden");

            html2pdf().from(cotizacion).set(opt).save().then(function () {
                $("#" + divContenedor + ",#" + divPDF + "").css('display', 'none');
                $("body,html").css("overflow", "");

                $("#Loader").css('display', 'none');
            });
        }
    </script>
</body>
</html>
