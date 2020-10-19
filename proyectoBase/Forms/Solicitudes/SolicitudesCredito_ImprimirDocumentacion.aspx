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
            /*color: white;*/
            /*padding: 5px 5px;*/
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
                /*color: white;*/
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

        .page-break {
            page-break-before: always;
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
                                <button type="button" id="btnReiniciarResolucion" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Contrato
                                </button>
                                <button type="button" id="btnReiniciarAnalisis" onclick="ExportToPDF('PAGARE','divContenedorPagare','divPagarePDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Pagaré
                                </button>
                                <button type="button" id="btnEliminarDocumento" onclick="ExportToPDF('COMPROMISO_LEGAL','divContenedorCompromisoLegal','divCompromisoLegalPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    Compromiso Legal
                                </button>
                                <button type="button" id="btnReasignarSolicitud" onclick="ExportToPDF('CONVENIO_DE_COMPRA_Y_VENTA_DE_VEHICULOS_PARA_FINANCIAMIENTO','divContenedorConvenioCyV','divConevionCyVPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/document_40px.png');">
                                    C. y V. de vehic. finan.
                                </button>
                                <a href="/Documentos/Recursos/INSPECCION%20DE%20VEHICULO.pdf" download>
                                    <button type="button" id="btnEliminarCondicion" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/inspection_40px.png');">
                                        Inspección de vehículo
                                    </button>
                                </a>
                                <button type="button" id="btnReiniciarValidacion" onclick="ExportToPDF('INSPECCION_SEGURO_VEHICULO','divContenedorInspeccionSeguro','divInspeccionSeguroPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/inspection_40px.png');">
                                    Inspección seguro
                                </button>
                                <button type="button" id="btnReiniciarCampo" onclick="ExportToPDF('TRASPASO','divContenedorTraspaso','divTraspasoPDF')" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 3px; margin-left: 5px; background-image: url('/Imagenes/resume_40px.png');">
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
                        <h6 class="m-0">Características mecánicas</h6>
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

        <!-- Divs de documentacion que se van a imprimir -->

        <!-- CONTRATO -->

        <!-- PAGARÉ -->
        <div id="divContenedorPagare" style="margin-top: 999px; display: none;">
            <div class="card m-0" runat="server" visible="true" id="divPagarePDF" style="display: none;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 m-0 p-0">
                            <img src="/Imagenes/LogoPrestadito.png" />
                        </div>
                        <div class="col-12">
                            <h5 class="text-center font-weight-bold">PAGARÉ POR <span id="lblMontoPagare">L. 396,468.78</span></h5>
                            <hr />
                        </div>
                    </div>

                    <div class="row">

                        <div class="col-12">
                            <p>
                                <b>YO, <span id="lblNombreClientePagare">MAYENSI CAROLINA POSADAS HENRIQUEZ</span>,</b>
                                mayor de edad, de profesión <span id="lblProfesionPagare">ING. INDUSTRIAL</span>,
                                con tarjeta de identidad <span id="lblIdentidadPagare">0410199700010</span> y con domicilio
                                en <span id="lblDireccionPagare">BARRIO PLAN DE LIMO,FLORIDA, COPA, UNA CUADRA ATRAS DE MERCADO MUNICIPAL, CALLE QUE VA AL CEMENTERIO MUNICIPAL, CASA DE ESQUINA COLOR CAFE, </span>,
                                actuando en condición personal, acepto que <b>DEBO y PAGARÉ</b> incondicionalmente <b>SIN PROTESTO,</b> y a la orden de <b>PRESTADITO S.A. de C.V.</b>,
                                la cantidad de <span id="lblMontoPalabrasPagare">TRESCIENTOS NOVENTA Y SEIS MIL CUATROCIENTOS SESENTA Y OCHO CON 78/100.</span> <span id="lblMontoDigitosPagare">(L.396,468.78). </span>
                                Dicha cantidad será pagada el día ___ del mes de ____________ del año _____, en las oficinas, agencias, sucursales y ventanillas de <b>PRESTADITO S.A. de C.V.</b>.

                                La cantidad consignada en este PAGARE devengará, a partir de esta fecha, una tasa de interés fluctuante del <span id="lblPorcentajeInteresPagare">1.91</span>% PORCIENTO MENSUAL,
                                sobre el saldo total de la deuda, a pagar mensualmente. En caso de mora, que se producirá por la falta de pago al vencimiento tanto del capital o de los intereses,
                                dará derecho a <b>PRESTADITO S.A. de C.V.</b> a exigir el pago de intereses moratorios del <span id="lblInteresesMoratoriosPagare">4.52%</span> PORCIENTO MENSUAL; 
                                a su vez, en caso de ejecución legal de la presente obligación, me someto a la jurisdicción que establezca <b>PRESTADITO S.A. de C.V.</b>,
                                quedando incorporadas en este documento todas las disposiciones del Código de Comercio. 
                                En fe de lo cual, firmo (amos)  en la ciudad de SAN PEDRO SULA departamento de CORTES, a los 18 días del mes de septiembre del año 2020.
                            </p>
                        </div>
                    </div>
                    <!-- Firma del cliente -->
                    <div class="row">
                        <div class="col-3"></div>
                        <div class="col-6 text-center p-0 mt-3">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Firma del cliente</label>
                            <label class="mt-0 d-block">MAYENSI CAROLINA POSADAS HENRIQUEZ</label>
                            <label class="mt-0 d-block">0410199700010</label>
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
                                <b>YO, <span id="lblNombreClienteCompromisoLegal">MAYENSI CAROLINA POSADAS HENRIQUEZ</span>,</b>
                                acepto haber adquirido un préstamo en efectivo con la empresa <b>PRESTADITO S.A. de C.V.</b>,
                                financiamiento otorgado a <span id="lblCuotasCompromisoLegal">60 cuotas (16,305.41)</span>, para la compra de contado de un Vehículo Automotor,
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
                            <label class="mt-0 d-block">MAYENSI CAROLINA POSADAS HENRIQUEZ</label>
                        </div>
                        <div class="col-4 text-center mt-3">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;">0410199700010</label>
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
                            <h6 class="text-center font-weight-bold">Convenio de regulación de  compra y venta de vehículos para financiamiento a Tercero</h6>
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
                                en este convenio se denominara <b>LA EMPRESA</b> y  <b>AL SEÑOR</b> <span id="lblNombreClienteConvenioCyV">JUAN PEDRO SUAREZ BERTIZ</span>
                                mayor de edad, <span id="lblNacionalidadConvenioCyV">hondureño</span>, <span id="lblEstadoCivilConvenioCyV">casado</span>,
                                con tarjeta de identidad número <span id="lblIdentidadConvenioCyV">1301-1980-00105</span> y con domicilio en la ciudad de <span id="lblCiudadClienteConvenioCyV">SAN PEDRO SULA</span>,
                                quien actúa en calidad de propietario de un vehículo marca <span id="lblMarcaConvenioCyV">HYUNDAI</span>, modelo <span id="lblModeloConvenioCyV">ELANTRA</span>, 
                                año <span id="lblAnioConvenioCyV">2017</span>, con motor: <span id="lblSerieMotorConvenioCyV">G4NH-GU734772</span>,
                                y con chasis: <span id="lblSerieChasis">KMHD74LF3HU094217</span> tipo <span id="lblTipoVehiculoConvenioCyV">TURISMO</span>
                                color <span id="lblColorConvenioCyV">PLATINO</span>, Placa: <span id="lblMatriculaConvenioCyV">PDA 6766</span>,
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
                                El cliente garantiza que los vehículos están en perfecto estado tanto en condiciones físicas como mecánicas para  circular y reúne todas las medidas de seguridad solicitadas por las Leyes Vigentes en el País. 
                            </p>
                            <p class="page-break mt-5 pt-5">
                                <b>Octavo:</b>
                                El Cliente se compromete a traspasar el vehículo a favor de la Persona que La Empresa designe. 
                            </p>
                            <p>
                                <b>Noveno:</b>
                                El Cliente se compromete a entregar  todos los documentos del vehículo a La Empresa. 
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
                            <label class="mt-0 d-block" id="lblNombreConvenioCyV">JUAN PEDRO SUAREZ BERTIZ</label>
                        </div>
                        <div class="col-1"></div>
                    </div>
                </div>
            </div>
        </div>

        <!-- INSPECCIÓN DEL SEGURO -->
        <div id="divContenedorInspeccionSeguro" style="margin-top: 999px; display: none;">
            <div class="card m-0" runat="server" visible="true" id="divInspeccionSeguroPDF" style="display: none;">
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
                                    <td class="pt-0 pb-0">JEEP</td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Modelo</th>
                                    <td class="pt-0 pb-0">PATRIOT</td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Año</th>
                                    <td class="pt-0 pb-0">2015</td>
                                </tr>
                                <tr>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Tipo</th>
                                    <td class="pt-0 pb-0">CAMIONETA</td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Kilometraje</th>
                                    <td class="pt-0 pb-0">59,290.00 M</td>
                                    <th class="bg-light font-weight-bold pt-0 pb-0">Placa</th>
                                    <td class="pt-0 pb-0">HAZ3205</td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6 pl-5">
                            <!-- Div donde se muestran las imágenes de la garantía-->
                            <div class="align-self-center text-center" id="divGaleriaInspeccionSeguroDeVehiculo" runat="server" style="display: none;">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- TRASPASO -->
        <div id="divContenedorTraspaso" style="/*margin-top: 999px; display: none; */">
            <div class="card m-0" runat="server" visible="true" id="divTraspasoPDF" style="/*display: none; */">
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
                                <b>YO, <span id="lblNombreClienteTraspaso">MAYENSI CAROLINA POSADAS HENRIQUEZ</span>,</b>
                                mayor de edad, <span id="lblNacionalidadTraspaso">hondureña</span>,
                                con tarjeta de identidad <span id="lblIdentidadTraspaso">0410199700010</span> y con domicilio
                                en <span id="lblDireccionTraspaso">BARRIO PLAN DE LIMO,FLORIDA, COPA, UNA CUADRA ATRAS DE MERCADO MUNICIPAL, CALLE QUE VA AL CEMENTERIO MUNICIPAL, CASA DE ESQUINA COLOR CAFE, </span>,
                                en mi condición de propietario, por medio de este documento hago formal el traspaso del vehiculo que se describe de la forma siguiente
                            </p>

                            <div class="row">
                                <div class="col-6">
                                    <div class="form-group row">
                                        <label class="col-sm-6">Marca:</label>
                                        <label class="col-sm-6">JEEP</label>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-6">Modelo:</label>
                                        <label class="col-sm-6">PATRIOT</label>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-6">Motor:</label>
                                        <label class="col-sm-6">BFD270814</label>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-6">Año:</label>
                                        <label class="col-sm-6">2015</label>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-6">Cilindraje:</label>
                                        <label class="col-sm-6">2.4</label>
                                    </div>
                                </div>
                                <div class="col-6">

                                    <div class="form-group row">
                                        <label class="col-sm-6">Tipo:</label>
                                        <label class="col-sm-6">CAMNIONETA</label>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-6">Color:</label>
                                        <label class="col-sm-6">NEGRO</label>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-6">Chasis:</label>
                                        <label class="col-sm-6">1C4NJRFB7FD270814</label>
                                    </div>

                                    <div class="form-group row">
                                        <label class="col-sm-6">Matricula:</label>
                                        <label class="col-sm-6">HAZ3205</label>
                                    </div>
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
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Nombre</label>
                        </div>
                        <div class="col-5 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 d-block">Firma</label>
                        </div>
                        <div class="col-1"></div>


                        <div class="col-3"></div>
                        <div class="col-6 text-center mt-5">
                            <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
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
            tile_height: 250,
        });


        //gallery_theme: "grid"

        function ExportToPDF(fileName, divContenedor, divPDF) {

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
            });
        }
    </script>
</body>
</html>
