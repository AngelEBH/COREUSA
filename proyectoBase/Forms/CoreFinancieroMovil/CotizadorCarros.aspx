<%@ Page Language="C#" AutoEventWireup="true" Inherits="Clientes_CotizadorCarros" CodeFile="CotizadorCarros.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <style>
        html, body {
            background-color: #fff;
        }

        .card-header {
            background-color: #f8f9fa !important;
        }

        h6 {
            font-size: 1rem;
        }

        .card {
            box-shadow: none;
        }

        .nav-tabs > li > .active {
            background-color: whitesmoke !important;
        }

        .text-xs {
            font-size: .7rem !important;
        }

        .loading {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background-color: transparent;
        }

        .loader {
            left: 50%;
            margin-left: -4em;
            font-size: 10px;
            border: .8em solid rgba(218, 219, 223, 1);
            border-left: .8em solid rgba(58, 166, 165, 1);
            animation: spin 1.1s infinite linear;
        }

            .loader, .loader:after {
                border-radius: 50%;
                width: 8em;
                height: 8em;
                display: block;
                position: absolute;
                top: 50%;
                margin-top: -4.05em;
            }

        @keyframes spin {
            0% {
                transform: rotate(360deg);
            }

            100% {
                transform: rotate(0deg);
            }
        }

        .page-template {
            font-family: "DejaVu Sans", "Arial", sans-serif;
            position: absolute;
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
        }

            .page-template .header {
                font-family: "DejaVu Sans";
                position: absolute;
                top: 20px;
                left: 30px;
                right: 30px;
                border-bottom: 1px solid #888;
                color: #888;
                margin-bottom: 50px;
                text-align: center;
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

        @font-face {
            font-family: "DejaVu Sans";
            src: url("/Content/fonts/DejaVu/DejaVuSans.ttf") format("truetype");
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMultiView">
            <ContentTemplate>
                <div class="card m-0">
                    <div class="card-body pt-0">
                        <div runat="server" style="padding-top: 1.25rem;" id="divParametros" visible="true">
                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Valor del automovil</label>
                                    <asp:TextBox ID="txtValorVehiculo" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="100000" runat="server"></asp:TextBox>
                                </div>
                                <div class="col" id="divPrima" runat="server" visible="true">
                                    <label class="col-form-label">Valor de la Prima</label>
                                    <asp:Label CssClass="" ID="lblPorcenajedePrima" runat="server" Text="" />
                                    <asp:TextBox ID="txtValorPrima" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="20000" runat="server"></asp:TextBox>

                                </div>
                            </div>
                            <div class="form-group" runat="server" id="divMontoFinanciarVehiculo" visible="false">
                                <label class="col-form-label">Valor del vehiculo a financiar</label>
                                <asp:TextBox ID="txtMonto" Enabled="false" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="col-form-label">Score Promedio</label>
                                <asp:TextBox ID="txtScorePromedio" type="tel" CssClass="form-control form-control-sm col-form-label MascaraNumerica" required="required" Text="999" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbFinanciamiento" runat="server" GroupName="rbTipoFinanciamiento" OnCheckedChanged="rbFinanciamiento_CheckedChanged" AutoPostBack="true" />
                                        <label class="form-check-label">Financiamiento</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbEmpeno" runat="server" GroupName="rbTipoFinanciamiento" OnCheckedChanged="rbEmpeno_CheckedChanged" AutoPostBack="true" />
                                        <label class="form-check-label">Empeño</label>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="col-form-label">Plazos disponibles</label>
                                <asp:DropDownList ID="ddlPlazos" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                            </div>
                            <div class="form-group row">
                                <div class="button-items col-sm-2">
                                    <asp:Button ID="btnCalcular" Text="Calcular" CssClass="btn btn-primary btn-lg btn-block waves-effect waves-light" runat="server" OnClick="btnCalcular_Click" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <div runat="server" id="PanelCreditos1" visible="false">
                            <div class="form-group row">
                                <div class="col-10 align-self-end">
                                    <label class="col-form-label">Valor del prestamo (<span class="text-xs"><asp:Label CssClass="col-form-label p-0 font-weight-bold align-self-center" ID="lblEtiqueta1" runat="server" Text="" /></span>)</label>
                                </div>

                                <div class="col-2">
                                    <asp:Button ID="btnDescargarCotizacion" runat="server" OnClientClick="ExportHtmlToPdf('#PanelCreditos1','Cotización_202009181655','Cotización de vehiculo')" CssClass="btn btn-lg float-right" Style="background-image: url(/Imagenes/export_pdf_80px.png); background-size: contain !important; background-repeat:no-repeat;" />
                                    <%--<button id="btnDescargarCotizacion" type="button" onclick="ExportHtmlToPdf('#PanelCreditos1','Cotización_202009181655','Cotización de vehiculo')" class="btn btn-lg float-right" style="background-image: url(/Imagenes/export_pdf_80px.png); background-size: contain !important; background-repeat:no-repeat;"></button>--%>
                                </div>
                                
                                
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtValorPrestamo1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label text-xs">Cuota del PMO</label>
                                    <asp:TextBox ID="txtCuotaPrestamo1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label class="col-form-label text-xs">Cuota del seguro</label>
                                    <asp:TextBox ID="txtValorSeguro1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label class="col-form-label text-xs">Cuota servic. GPS</label>
                                    <asp:TextBox ID="txtServicioGPS1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 col-form-label">Cuota total mensual</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtCuotaTotal1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 col-form-label">Cuota total mensual <span class="text-xs">(Gastos de cierre)</span></label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtCuotaTotalGC1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-2 col-form-label">Gastos de cierre <span class="text-xs">(Pago en efectivo)</span></label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtGastosdeCierre1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group row m-0" runat="server" id="divNuevoCalculo" visible="false">
                            <div class="button-items col-sm-2 p-0">
                                <asp:Button ID="btnNuevoCalculo" Text="Nuevo cálculo" CssClass="btn btn-primary btn-lg btn-block waves-effect waves-light m-0" runat="server" OnClick="btnNuevoCalculo_Click" />
                            </div>
                        </div>
                        <div class="form-group row m-0" runat="server" id="PanelMensajeErrores">
                            <asp:Label CssClass="col-sm-2 col-form-label text-danger" ID="lblMensaje" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="updateProgress" runat="server">
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
    <script src="/Scripts/plugins/kendo/jszip.min.js"></script>
    <script src="/Scripts/plugins/kendo/kendo.all.min.js"></script>
    <script src="/Scripts/plugins/kendo/PrintHtmlToPDF.js"></script>
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
                integerDigits: 3, // cantidad de digitos permitidos
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
            $(".identidad").inputmask("9999999999999");
        });
    </script>
    <script type="x/kendo-template" id="page-template">
        <div class="page-template">
            <div class="header">
                <%--<img src="http://crediflash.prestadito.corp/documentos/base/logo_prestadito.png" style="width:150px;" />--%>
                <img src="/Imagenes/LogoPrestaditoMediano.png" style="width:150px; float:left;" />
                <h3 id="titleTemplate">Cotización de vehículo</h3>
            </div>
            <div class="footer">
                Pagina #: pageNum # de #: totalPages #
            </div>
        </div>
    </script>
</body>
</html>
