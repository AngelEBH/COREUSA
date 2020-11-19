<%@ Page Language="C#" AutoEventWireup="true" Inherits="Clientes_CotizadorCarros" CodeBehind="CotizadorCarros.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css" />
    <style>
        html, body {
            background-color: #fff;
        }

        .card-header {
            background-color: #f8f9fa !important;
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

        footer {
            position: absolute;
            bottom: 0;
            width: 100%;
            height: 2.5rem;
        }
    </style>
</head>
<body>
    <form id="frmCalculadora" runat="server" style="width: 100%; height: 100%; position: absolute; left: 0; top: 0;">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMultiView">
            <ContentTemplate>


                <div class="card m-0">
                    <div class="card-body pt-0">
                        <div runat="server" style="padding-top: 1.25rem;" id="divParametros" visible="true">
                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Valor del automovil</label>
                                    <asp:TextBox ID="TextBox1" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="100000" runat="server"></asp:TextBox>
                                </div>
                                <div class="col" id="divPrima" runat="server" visible="true">
                                    <label class="col-form-label">Valor de la Prima</label>
                                    <asp:Label CssClass="" ID="Label1" runat="server" Text="" Visible="false" />
                                    <asp:TextBox ID="TextBox2" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="20000" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" runat="server" id="divMontoFinanciarVehiculo" visible="false">
                                <label class="col-form-label">Valor del vehiculo a financiar</label>
                                <asp:TextBox ID="TextBox3" Enabled="false" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="col-form-label">Score Promedio</label>
                                <asp:TextBox ID="TextBox4" type="tel" CssClass="form-control form-control-sm col-form-label MascaraNumerica" required="required" Text="999" runat="server"></asp:TextBox>
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
                            <div class="form-group row mb-0">
                                <div class="button-items col-sm-12">
                                    <asp:Button ID="Button1" Text="Calcular" CssClass="btn btn-primary btn-lg btn-block waves-effect waves-light" runat="server" OnClick="btnCalcular_Click" />
                                </div>
                            </div>
                        </div>
                        <br />
                        <!-- Resultados de la cotización -->
                        <div runat="server" id="PanelCreditos1" visible="false">
                            <!-- Nav tabs -->
                            <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs">
                                <li class="nav-item">
                                    <a class="nav-link active" data-toggle="tab" href="#GastosDeCierreEfectivo" role="tab" aria-selected="false">
                                        <span class="d-block d-sm-none">Gastos de cierre
                                            <br />
                                            efectivo</span>
                                        <span class="d-none d-sm-block"><small>Gastos de cierre efectivo</small></span>
                                    </a>
                                </li>
                                <li class="nav-item">
                                    <a class="nav-link" data-toggle="tab" href="#GastosDeCierreFinanciados" role="tab" aria-selected="false">
                                        <span class="d-block d-sm-none">Gastos de cierre
                                            <br />
                                            financiados</span>
                                        <span class="d-none d-sm-block"><small>Gastos de cierre financiados</small></span>
                                    </a>
                                </li>
                            </ul>
                            <!-- Tab panes -->
                            <div class="tab-content" runat="server" id="tabContent" visible="true">
                                <!-- Gastos de cierre en efectivo -->
                                <div class="tab-pane active" id="GastosDeCierreEfectivo" role="tabpanel">
                                    <div class="form-group row">
                                        <div class="col-10 align-self-end">
                                            <label class="col-form-label">Valor del prestamo (<span class="text-xs"><asp:Label CssClass="col-form-label p-0 font-weight-bold align-self-center" ID="lblEtiqueta1" runat="server" Text="" /></span>)</label>
                                        </div>
                                        <div class="col-2 justify-content-end">
                                            <asp:Button ID="btnDescargarCotizacion" runat="server" OnClick="btnDescargarCotizacion_Click" CssClass="btn btn-lg float-right" Style="background-image: url(/Imagenes/export_pdf_80px.png); background-size: contain !important; background-repeat: no-repeat;" />
                                        </div>
                                        <div class="col-12">
                                            <asp:TextBox ID="txtValorPrestamo1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-12">
                                            <label class="col-form-label">Cuota del PMO</label>
                                            <asp:TextBox ID="txtCuotaPrestamo1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group form-row">
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
                                        <label class="col-12 col-form-label">Cuota total mensual</label>
                                        <div class="col-12">
                                            <asp:TextBox ID="txtCuotaTotal1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label class="col-12 col-form-label">Gastos de cierre <span class="text-xs">(Pago en efectivo)</span></label>
                                        <div class="col-12">
                                            <asp:TextBox ID="txtGastosdeCierreEfectivo" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <!-- Gastos de cierre financiados -->
                                <div class="tab-pane" id="GastosDeCierreFinanciados" role="tabpanel">
                                    <div class="form-group row">
                                        <div class="col-10 align-self-end">
                                            <label class="col-form-label">Valor del prestamo (<span class="text-xs"><asp:Label CssClass="col-form-label p-0 font-weight-bold align-self-center" ID="lblEtiqueta2" runat="server" Text="" /></span>)</label>
                                        </div>
                                        <div class="col-2">
                                            <asp:Button ID="btnDescargarCotizacion2" runat="server" OnClick="btnDescargarCotizacion2_Click" CssClass="btn btn-lg float-right" Style="background-image: url(/Imagenes/export_pdf_80px.png); background-size: contain !important; background-repeat: no-repeat;" />
                                        </div>
                                        <div class="col-12">
                                            <asp:TextBox ID="txtValorPrestamo2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-12">
                                            <label class="col-form-label">Cuota del PMO</label>
                                            <asp:TextBox ID="txtCuotaPrestamo2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group form-row">
                                        <div class="col">
                                            <label class="col-form-label text-xs">Cuota del seguro</label>
                                            <asp:TextBox ID="txtValorSeguro2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col">
                                            <label class="col-form-label text-xs">Cuota servic. GPS</label>
                                            <asp:TextBox ID="txtServicioGPS2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label class="col-12 col-form-label">Cuota total mensual</label>
                                        <div class="col-12">
                                            <asp:TextBox ID="txtCuotaTotal2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row" runat="server" visible="false">
                                        <label class="col-12 col-form-label">Cuota total mensual <span class="text-xs">(Gastos de cierre)</span></label>
                                        <div class="col-12">
                                            <asp:TextBox ID="txtCuotaTotalGastosDeCierreFinanciados" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Nueva cotización -->
                        <div class="form-group row m-0" runat="server" id="divNuevoCalculo" visible="false">
                            <div class="button-items col-sm-12 p-0">
                                <asp:Button ID="btnNuevoCalculo" Text="Nuevo cálculo" CssClass="btn btn-primary btn-lg btn-block waves-effect waves-light m-0" runat="server" OnClick="btnNuevoCalculo_Click" />
                            </div>
                        </div>
                        <!-- Label para mensajes y advertencias -->
                        <div class="form-group row m-0" runat="server" id="PanelMensajeErrores">
                            <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="Label2" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>

                <div class="DivPanelListas" style="width: 700px;">
                    <div class="DivTituloPanel">
                        <table style="height: 100%;">
                            <tr>
                                <td class="TableFormatoTitulo">Cotizador de Prestadito Automovil</td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel20" runat="server" CssClass="FormatoPanelTodoBlanco" Height="140px" Style="background-color: whitesmoke;">

                        <asp:DropDownList ID="ddlProducto" runat="server" CssClass="FormatoDDL" Style="left: 190px; top: 5px; width: 120px;" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        <asp:TextBox ID="txtValorVehiculo" runat="server" CssClass="FormatotxtMoneda" input="tel" Style="left: 190px; top: 31px; width: 80px;"></asp:TextBox>
                        <asp:TextBox ID="txtValorPrima" runat="server" CssClass="FormatotxtMoneda" input="tel" Style="left: 190px; top: 57px; width: 80px;"></asp:TextBox>
                        <asp:TextBox ID="txtMonto" runat="server" CssClass="FormatotxtMonedaRO" input="tel" Style="left: 190px; top: 83px; width: 80px;" Enabled="false"></asp:TextBox>
                        <asp:TextBox ID="txtScorePromedio" runat="server" CssClass="FormatotxtMoneda" input="tel" Style="left: 190px; top: 109px; width: 80px;"></asp:TextBox>

                        <asp:Label CssClass="Formatolbl" ID="lblProducto" runat="server" Style="left: 15px; top: 9px;" Text="Producto:" />
                        <asp:Label CssClass="Formatolbl" ID="lblValor" runat="server" Style="left: 15px; top: 35px;" Text="Valor de vehiculo:" />
                        <asp:Label CssClass="Formatolbl" ID="lblPrima" runat="server" Style="left: 15px; top: 61px;" Text="Valor de la prima:" />
                        <asp:Label CssClass="Formatolbl" ID="lblPorcenajedePrima" runat="server" Style="left: 290px; top: 87px; width: 50px;" />
                        <asp:Label CssClass="Formatolbl" ID="lblMonto" runat="server" Style="left: 15px; top: 87px;" Text="Valor a financiar del vehiculo:" />
                        <asp:Label CssClass="Formatolbl" ID="lblScorePromedio" runat="server" Style="left: 15px; top: 113px;" Text="Score promedio:" />

                        <asp:DropDownList ID="ddlGastosdeCierre" runat="server" CssClass="FormatoDDL" Style="left: 430px; top: 5px; width: 150px;"></asp:DropDownList>
                        <asp:DropDownList ID="ddlSeguro" runat="server" CssClass="FormatoDDL" Style="left: 430px; top: 31px; width: 150px;"></asp:DropDownList>
                        <asp:DropDownList ID="ddlGPS" runat="server" CssClass="FormatoDDL" Style="left: 430px; top: 57px; width: 150px;"></asp:DropDownList>

                        <asp:Label CssClass="Formatolbl" ID="lblGastosdeCierre" runat="server" Style="left: 330px; top: 9px;" Text="Gastos de cierre:" />
                        <asp:Label CssClass="Formatolbl" ID="lblSeguro" runat="server" Style="left: 330px; top: 35px;" Text="Tipo de seguro:" />
                        <asp:Label CssClass="Formatolbl" ID="lblGPS" runat="server" Style="left: 330px; top: 61px;" Text="Lleva GPS:" />



                        <asp:Button CssClass="FormatoBotonesIconoArriba" ID="btnCalcular" runat="server" Text="Calcular" Style="background-image: url('/Imagenes/iconoCotizador24.png'); left: 590px; top: 5px;" OnClick="btnCalcular_Click" />



                    </asp:Panel>
                </div>

                <asp:Panel CssClass="FormatoPanelNOAB" ID="PanelCotizadorResultado" runat="server" Style="margin-top: 5px; width: 700px; background-color: whitesmoke;" Visible="false">
                    <div class="DivTituloPanel">
                        <table style="height: 100%;">
                            <tr>
                                <td class="TableFormatoTitulo">Cotizacion por plazos</td>
                            </tr>
                        </table>
                    </div>
                    <asp:GridView ID="gvCotizador" runat="server" CssClass="GridViewFormatoGeneral" HeaderStyle-CssClass="GridViewCabecera" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False" ShowHeader="True" Style="word-break: break-all;" RowStyle-Height="24px">
                        <Columns>
                            <asp:BoundField DataField="fiIDPlazo" HeaderText="Plazo" ItemStyle-Width="50px" ItemStyle-HorizontalAlign="Center" />
                            <asp:BoundField DataField="fnCuotadelPrestamo" HeaderText="Cuota Prestamo" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                            <asp:BoundField DataField="fnCuotaSegurodeVehiculo" HeaderText="Cuota Seguro" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                            <asp:BoundField DataField="fnCuotaServicioGPS" HeaderText="Cuota GPS" ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                            <asp:BoundField DataField="fnTotalCuota" HeaderText="Total Cuota." ItemStyle-Width="90px" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                        </Columns>
                        <HeaderStyle CssClass="GridViewCabecera" />
                        <RowStyle CssClass="GridViewFilas" />
                    </asp:GridView>
                </asp:Panel>

                <asp:Panel ID="PanelErrores" runat="server" CssClass="FormatoPanelAlertaRojo" Height="50px" Width="250px" Visible="false">
                    <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" Style="color: white; margin: 5px; margin-left: 55px;"></asp:Label>
                </asp:Panel>
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
</body>
</html>
