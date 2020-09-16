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
    </style>
</head>
<body>
    <form id="Form1" runat="server">
        <div class="card">
            <div class="card-header">
                <h6 class="card-title text-center">Cotizador de Prestadito Automovil</h6>
            </div>
            <div class="card-body pt-0">
                <div runat="server" style="padding-top: 1.25rem;" id="divParametros" visible="true">
                    <div class="form-group form-row">
                        <div class="col">
                            <label class="col-form-label">Valor del automovil</label>
                            <asp:TextBox ID="txtValorVehiculo" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                        </div>
                        <div class="col" id="divPrima" runat="server" visible="true">
                            <label class="col-form-label">Valor de la Prima</label>
                            <asp:Label CssClass="" ID="lblPorcenajedePrima" runat="server" Text="" />
                            <asp:TextBox ID="txtValorPrima" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>

                        </div>
                    </div>

                    <div class="form-group">
                        <label class="col-form-label">Valor del vehiculo a financiar</label>
                        <asp:TextBox ID="txtMonto" Enabled="false" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <label class="col-form-label">Score Promedio</label>
                        <asp:TextBox ID="txtScorePromedio" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
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
                        <div class="col-sm-12 text-center">
                            <asp:Label CssClass="col-form-label p-0" ID="lblEtiqueta1" runat="server" Text="" />
                        </div>
                    </div>

                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Valor del prestamo</label>
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
    </form>

    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(document).ready(function () {
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
            $(".identidad").inputmask("9999999999999");
        });
    </script>
</body>
</html>
