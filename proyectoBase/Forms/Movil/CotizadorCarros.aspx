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
    </style>
</head>
<body>
    <form id="frmCalculadora" runat="server">
        <div class="card">
            <div class="card-header">
                <h6 class="card-title text-center">Cotizador de Prestadito Automovil</h6>
            </div>
            <div class="card-body">
                <div class="form-group row" runat="server" id="divNuevoCalculo" visible="false">
                    <div class="button-items col-sm-2">
                        <asp:Button ID="btnNuevoCalculo" Text="Nuevo cálculo" CssClass="btn btn-primary btn-lg btn-block waves-effect waves-light m-0" runat="server" OnClick="btnNuevoCalculo_Click" />
                    </div>
                </div>
                <div runat="server" id="divParametros" visible="true">
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Valor del automovil</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="txtValorVehiculo" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                        </div>

                        <label class="col-sm-2 col-form-label">Valor de la Prima</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="txtValorPrima" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Plazos disponibles</label>
                        <div class="col-sm-2">
                            <asp:DropDownList ID="ddlPlazos" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="button-items col-sm-2">
                            <asp:Button ID="btnCalcular" Text="Calcular" CssClass="btn btn-primary btn-lg btn-block waves-effect waves-light" runat="server" OnClick="btnCalcular_Click" />
                        </div>
                    </div>
                </div>

                <br />

                <!-- Nav tabs -->
                <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs" visible="false">
                    <li class="nav-item">
                        <a class="nav-link active" data-toggle="tab" href="#tasa19" role="tab" aria-selected="false">
                            <span class="d-block d-sm-none">Tasa 19%</span>
                            <span class="d-none d-sm-block"><small>Tasa al 19% con score mayor o igual a 800</small></span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-toggle="tab" href="#tasa26" role="tab" aria-selected="false">
                            <span class="d-block d-sm-none">Tasa 26%</span>
                            <span class="d-none d-sm-block"><small>Tasa al 26% con score entre 700 y 799</small></span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-toggle="tab" href="#tasa32" role="tab" aria-selected="false">
                            <span class="d-block d-sm-none">Tasa 32%</span>
                            <span class="d-none d-sm-block"><small>Tasa al 32% con score entre 600 y 699</small></span>
                        </a>
                    </li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content" runat="server" id="tabContent" visible="false">
                    <div class="tab-pane p-3 active" id="tasa19" role="tabpanel">
                        <div class="form-group row" runat="server" id="PanelCreditos" visible="false">

                            <label class="col-sm-2 col-form-label">Valor del prestamo</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtValorPrestamo1" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota del prestamo</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtCuotaPrestamo1" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota del seguro</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtValorSeguro1" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota Servicio GPS</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtServicioGPS1" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota total mensual</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtCuotaTotal1" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Gastos de Cierre(Pago en efectivo)</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtGastosdeCierre1" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                <small>Tasa al 19% con score mayor o igual a 800</small>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane p-3" id="tasa26" role="tabpanel">
                        <div class="form-group row" runat="server" id="PanelCreditosDos" visible="false">

                            <label class="col-sm-2 col-form-label">Valor del prestamo</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtValorPrestamo2" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota del prestamo</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtCuotaPrestamo2" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota del seguro</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtValorSeguro2" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota Servicio GPS</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtServicioGPS2" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota total mensual</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtCuotaTotal2" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Gastos de Cierre(Pago en efectivo)</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtGastosdeCierre2" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                <small>Tasa al 26% con score entre 700 y 799</small>
                            </div>

                        </div>
                    </div>
                    <div class="tab-pane p-3" id="tasa32" role="tabpanel">
                        <div class="form-group row" runat="server" id="PanelCreditosTres" visible="false">

                            <label class="col-sm-2 col-form-label">Valor del prestamo</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtValorPrestamo3" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota del prestamo</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtCuotaPrestamo3" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota del seguro</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtValorSeguro3" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota Servicio GPS</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtServicioGPS3" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Cuota total mensual</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtCuotaTotal3" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>

                            <label class="col-sm-2 col-form-label">Gastos de Cierre(Pago en efectivo)</label>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtGastosdeCierre3" CssClass="form-control form-control-sm col-form-label FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                <small>Tasa al 32% con score entre 600 y 699</small>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="form-group row" runat="server" id="PanelMensajeErrores">
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
