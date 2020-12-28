<%@ Page Language="C#" AutoEventWireup="true" Inherits="CotizadorCarros" CodeFile="CotizadorCarros.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Cotizador de carros</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CotizadorMovil.css" rel="stylesheet" />
</head>
<body>
    <form id="frmCalculadora" runat="server">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMultiView">
            <ContentTemplate>
                <div class="card m-0">
                    <div class="card-body pt-0">
                        <div runat="server" style="padding-top: 1.25rem;" id="DivPanelListas" visible="true">
                            <div class="form-group form-row">
                                <div class="col-12">
                                    <label class="col-form-label">Producto</label>
                                    <asp:DropDownList ID="ddlProducto" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged" AutoPostBack="True" runat="server" CssClass="form-control form-control-sm"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col-6" runat="server" id="divValorDelVehiculo">
                                    <label class="col-form-label">Valor del vehículo</label>
                                    <asp:TextBox ID="txtValorVehiculo" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server" OnTextChanged="CalcularPrima_TextChanged" AutoPostBack="True"></asp:TextBox>
                                </div>
                                <div class="col-6" id="divPrima" runat="server" visible="true">
                                    <label class="col-form-label">Valor de la Prima <span id="lblPorcentajedePrima" runat="server" class="font-weight-bold"></span></label>
                                    <asp:TextBox ID="txtValorPrima" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server" OnTextChanged="CalcularPrima_TextChanged" AutoPostBack="True"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" runat="server" id="divMontoFinanciarVehiculo" visible="true">
                                <asp:Label runat="server" ID="lblMonto" class="col-form-label">Valor a financiar del vehiculo <span id="lblPorcentajeMonto" runat="server" class="font-weight-bold"></span></asp:Label>
                                <asp:TextBox ID="txtMonto" Enabled="false" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="col-form-label">Score Promedio</label>
                                <asp:TextBox ID="txtScorePromedio" type="tel" CssClass="form-control form-control-sm col-form-label MascaraNumerica" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="col-form-label">Gastos de cierre</label>
                                <asp:DropDownList ID="ddlGastosdeCierre" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                            </div>
                            <div class="form-group form-row">
                                <div class="col-6">
                                    <label class="col-form-label">Tipo de seguro</label>
                                    <asp:DropDownList ID="ddlSeguro" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                </div>
                                <div class="col-6">
                                    <label class="col-form-label">Lleva GPS</label>
                                    <asp:DropDownList ID="ddlGPS" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group row mb-0">
                                <div class="button-items col-sm-12">
                                    <asp:Button ID="btnCalcular" Text="Calcular" CssClass="btn btn-secondary btn-lg btn-block m-0" runat="server" OnClick="btnCalcular_Click" />
                                </div>
                            </div>
                        </div>
                        <br />

                        <!-- Resultados de la cotización -->
                        <div runat="server" id="divResultados" visible="false">

                            <!-- Nav tabs -->
                            <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs">
                                <li class="nav-item">
                                    <a class="nav-link active" data-toggle="tab" href="#tab_CotizacionPorPlazos" role="tab" aria-selected="false">
                                        <span class="d-block d-sm-none">Cotización por plazos</span>
                                        <span class="d-none d-sm-block"><small>Cotización por plazos</small></span>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" data-toggle="tab" href="#tab_DatosParaSAF" role="tab" aria-selected="false">
                                        <span class="d-block d-sm-none">Datos para SAF</span>
                                        <span class="d-none d-sm-block"><small>Datos para SAF</small></span>
                                    </a>
                                </li>
                            </ul>
                            <!-- Tab panes -->
                            <div class="tab-content" runat="server" id="tabContent" visible="true">
                                <!-- Gastos de cierre en efectivo -->
                                <div class="tab-pane active" id="tab_CotizacionPorPlazos" role="tabpanel">

                                    <div class="row justify-content-between">
                                        <div class="col-auto">
                                            <h6 class="font-weight-bold text-center">Cotización por plazos</h6>
                                        </div>
                                        <div class="col-auto">
                                            <button id="btnDescargarCotizacion" type="button" onclick="ExportToPDF('Cotizacion')" class="btn btn-lg float-right"><i class="fas fa-print"></i></button>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="table-responsive">
                                            <table class="tabla-responsiva-cotizador-por-plazos" runat="server" id="tblCotizacionPorPlazos">
                                                <%--<thead>
                                                    <tr>
                                                        <th>Plazo</th>
                                                        <th>Tasa anual (%)</th>
                                                        <th>Valor vehículo</th>
                                                        <th>Gastos de cierre</th>
                                                        <th>Costo GPS</th>
                                                        <th>Total a Financiar</th>
                                                        <th>Cuota del préstamo</th>
                                                        <th>Couta del seguro</th>
                                                        <th>Cuota del GPS</th>
                                                        <th>Cuota total</th>
                                                    </tr>
                                                </thead>--%>
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>

                                <!-- Gastos de cierre financiados -->
                                <div class="tab-pane" id="tab_DatosParaSAF" role="tabpanel">

                                    <div class="row justify-content-between">
                                        <div class="col-auto">
                                            <h6 class="font-weight-bold text-center">Datos para SAF</h6>
                                        </div>
                                    </div>

                                    <div class="form-group">
                                        <div class="table-responsive">
                                            <table class="tabla-responsiva-datos-para-saf" runat="server" id="tblDatosParaSAF">
                                                <%--<thead>
                                                    <tr>
                                                        <th>Plazo</th>
                                                        <th>Gastos Legales</th>
                                                        <th>Provisión</th>
                                                        <th>Seg. Cont.</th>
                                                        <th>Total</th>
                                                        <th>Poliza</th>
                                                    </tr>
                                                </thead>--%>
                                                <tbody>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Nueva cotización -->
                            <div class="form-group row mb-0 mr-0 ml-0 mt-3">
                                <div class="button-items col-sm-12 p-0">
                                    <asp:Button ID="btnNuevoCalculo" Text="Nuevo cálculo" CssClass="btn btn-secondary btn-block btn-lg m-0" runat="server" OnClick="btnNuevoCalculo_Click" />
                                </div>
                            </div>

                            <!-- Label para mensajes y advertencias -->
                            <div class="form-group row m-0" runat="server" id="PanelMensajeErrores">
                                <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" runat="server"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>


                <!-- PDF DE LA COTIZACIÓN (IGNORAR) -->
                <div id="divContenedor" style="margin-top: 999px; display: none;">
                    <div class="card m-0 divCotizacionPDF" runat="server" visible="true" id="divCotizacionPDF" style="display: none;">
                        <div class="card-body pt-0">
                            <div class="row">

                                <div class="col-6 m-0 p-0">
                                    <img src="/Imagenes/LogoPrestadito.png" />
                                </div>
                                <div class="col-6 m-0 p-0 justify-content-end">
                                    <table class="table table-borderless m-0" style="width: 100%">
                                        <tr>
                                            <td class="p-0 text-right">
                                                <asp:Label ID="lblOficialNegociosNegociacion" CssClass="p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                        </tr>
                                        <tr runat="server" id="trTelefonoVendedor">
                                            <td class="p-0 text-right">
                                                <asp:Label ID="lblTelefonoVendedorNegociacion" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                    <table class="table table-borderless m-0" style="width: 100%">
                                        <tr>
                                            <td class="p-0 text-right">
                                                <asp:Label ID="lblCorreoVendedorNegociacion" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                    <table class="table table-borderless m-0" style="width: 100%">
                                        <tr>
                                            <td class="p-0 text-right">
                                                <asp:Label ID="lblFechaNegociacion" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-12 mb-0">
                                    <h5 class="text-center font-weight-bold mb-1">COTIZACIÓN</h5>
                                </div>


                                <div class="col-12">
                                    <table class="table table-sm mb-0">
                                        <thead class="text-center">
                                            <tr>
                                                <th colspan="4" class="p-1 font-weight-bold">Información del financiamiento</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td class="p-1">Valor del vehiculo</td>
                                                <td class="p-1">
                                                    <asp:Label runat="server" ID="lblValorVehiculo">L 0.00</asp:Label>
                                                </td>

                                                <td class="p-1">Score</td>
                                                <td class="p-1">
                                                    <asp:Label runat="server" ID="lblScore">0</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p-1">Prima</td>
                                                <td class="p-1">
                                                    <asp:Label runat="server" ID="lblPrima">L 0.00</asp:Label>
                                                </td>

                                                <td class="p-1">GPS</td>
                                                <td class="p-1">
                                                    <asp:Label runat="server" ID="lblGPS"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="p-1">Monto a Financiar</td>
                                                <td class="p-1">
                                                    <asp:Label runat="server" ID="lblMontoAFinanciar">L 0.00</asp:Label>
                                                </td>

                                                <td class="p-1">SEGURO</td>
                                                <td class="p-1">
                                                    <asp:Label runat="server" ID="lblSeguro"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td class="p-1">GASTOS DE CIERRE</td>
                                                <td class="p-1">
                                                    <asp:Label runat="server" ID="lblMontoGastosDeCierre"></asp:Label>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-12 mt-4">
                                    <table class="table table-borderless mb-0">
                                        <thead class="text-center bg-primary text-white">
                                            <tr>
                                                <th class="p-1 font-weight-bold">Cotización por plazos</th>
                                            </tr>
                                        </thead>
                                    </table>
                                    <table class="table table-striped table-sm tabla-cotizador-pdf" runat="server" id="tblCotizacionPorPlazosPDF">
                                        <thead>
                                            <tr>
                                                <th class="pl-0 pr-0">Plazo</th>
                                                <th class="pl-0 pr-0">Tasa anual</th>
                                                <th class="pl-0 pr-0">Valorvehículo</th>
                                                <th class="pl-0 pr-0">Gastos cierre</th>
                                                <th class="pl-0 pr-0">Costo GPS</th>
                                                <th class="pl-0 pr-0">Total a Financiar</th>
                                                <th class="pl-0 pr-0">Cuota préstamo</th>
                                                <th class="pl-0 pr-0">Couta seguro</th>
                                                <th class="pl-0 pr-0">Cuota GPS</th>
                                                <th class="pl-0 pr-0">Cuota total</th>
                                            </tr>
                                        </thead>
                                    </table>
                                </div>

                                <div class="col-12 mt-2 text-center p-0">
                                    <img src="/Imagenes/Cotizador/image3.png" class="img-fluid" /><br />
                                    <label class="mt-1">Cotización valida únicamente por 5 días y está sujeta a cambios sin previo aviso por parte de Prestadito.</label>
                                </div>
                                <div class="col-6 mt-4">
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th class="p-1">REQUISITOS PARA FINANCIAMIENTO</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td class="p-1">COPIA DE CÉDULA</td>
                                            </tr>
                                            <tr>
                                                <td class="p-1">COMPROBAR INGRESOS</td>
                                            </tr>
                                            <tr>
                                                <td class="p-1">RECIBO PÚBLICO</td>
                                            </tr>
                                            <tr>
                                                <td class="p-1">CROQUIS DE VIVIENDA</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-6 mt-4">
                                    <table class="table">
                                        <thead>
                                            <tr>
                                                <th class="p-1">REQUISITOS PARA FINANCIAMIENTO CON GARANTÍA</th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td class="p-1">COPIA DE CÉDULA</td>
                                            </tr>
                                            <tr>
                                                <td class="p-1">RECIBO PÚBLICO</td>
                                            </tr>
                                            <tr>
                                                <td class="p-1">CROQUIS DE VIVIENDA</td>
                                            </tr>
                                            <tr>
                                                <td class="p-1">RTN</td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="col-12 mt-2 text-center p-0">
                                    <label class="mt-1">Para más información llama al 2540-1050</label>
                                    <h5 class="font-weight-bold">¡Porque no importa la ocasión, PRESTADITO ES LA SOLUCIÓN!</h5>
                                </div>
                            </div>
                        </div>
                        <!--<footer>
<div class="row h-100">
<div class="col-12 justify-content-end">
<asp:Label runat="server" ID="lblUsuarioImprime" CssClass="font-weight-bold"></asp:Label>
</div>
</div>
</footer>-->
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="prgLoadingStatus" runat="server" AssociatedUpdatePanelID="upMultiView">
            <ProgressTemplate>
                <div class="loading">
                    <div class="loader"></div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>

    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(function () {
            $(".MascaraCantidad").inputmask("decimal", {
                alias: "numeric",
                groupSeparator: ",",
                digits: 2,
                integerDigits: 11,
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
            $(".MascaraNumerica").inputmask("decimal", {
                alias: "numeric",
                groupSeparator: ",",
                digits: 0,
                integerDigits: 3,
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
            $(".identidad").inputmask("9999999999999");
        });
    </script>
    <script src="/Scripts/plugins/html2pdf/html2pdf.bundle.js"></script>
    <script>
        // Exportar cotización a PDF
        function ExportToPDF(fileName) {

            const cotizacion = this.document.getElementById("divCotizacionPDF");
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
                jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' },
                pagebreak: { after: '.page-break', always: 'img' }
            };

            $("#divContenedor,#divCotizacionPDF").css('display', '');
            $("body,html").css("overflow", "hidden");

            html2pdf().from(cotizacion).set(opt).save().then(function () {
                $("#divContenedor,#divCotizacionPDF").css('display', 'none');
                $("body,html").css("overflow", "");
            });
        }
    </script>
</body>
</html>
