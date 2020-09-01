<%@ Page Language="C#" AutoEventWireup="True" CodeFile="GestionCobranzaClientesOpcionesFinanciamiento.aspx.cs" Inherits="Gestion_GestionCobranzaClientesOpcionesFinanciamiento" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css" />
    <title></title>
    <script type="text/javascript">
        function codeAddress() {
            window.resizeTo(920, 610);
            window.menubar = no;
            window.document.menubar = no;
        }
        window.onload = codeAddress;
    </script>
    <style>
        input::-webkit-outer-spin-button,
        input::-webkit-inner-spin-button {
            -webkit-appearance: none;
            margin: 0;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="smConvenios"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upClientes">
            <ContentTemplate>
                <asp:Panel ID="PanelDatos" runat="server" CssClass="FormatoPanelNOAB" Height="540px" Width="900px" style="padding-bottom: 10px;">
                    <div class="DivTituloPanel">
                        <table style="height: 100%;">
                            <tr>
                                <td class="TableFormatoTitulo">
                                    <asp:Label CssClass="FormatolblNOAB" ID="lblTitulo" runat="server" Text="" />
                            </tr>
                        </table>
                    </div>
                    <asp:Panel CssClass="FormatoPanelTodoBlancoHumo" ID="PanelDatosCliente" runat="server" Width="898px" Height="36px">
                        <asp:TextBox ID="txtIDCliente" runat="server" CssClass="FormatotxtRO" Style="left: 80px; top: 5px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtNombreCliente" runat="server" CssClass="FormatotxtRO" Style="left: 260px; top: 5px; width: 290px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtProducto" runat="server" CssClass="FormatotxtRO" Style="left: 640px; top: 5px; width: 240px;" ReadOnly="True"></asp:TextBox>


                        <asp:Label CssClass="Formatolbl" ID="lblIDCliente" runat="server" Style="left: 10px; top: 9px;">ID Cliente:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblNombreCliente" runat="server" Style="left: 210px; top: 9px;">Cliente:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblProducto" runat="server" Style="left: 580px; top: 9px;">Producto:</asp:Label>
                    </asp:Panel>
                    <asp:Panel CssClass="FormatoPanelTodoBlancoHumo" ID="PanelDetalleCredito" runat="server" Width="898px" Height="200px">
                        <asp:TextBox ID="txtMontoOriginal" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 125px; top: 5px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtPlazoOtorgado" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 125px; top: 31px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtValorCuota" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 125px; top: 57px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtCuotasPagadas" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 125px; top: 83px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFechaInicioCredito" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 125px; top: 109px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFechaFinCredito" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 125px; top: 135px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtDiasAtraso" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 125px; top: 161px; width: 100px;" ReadOnly="True"></asp:TextBox>

                        <asp:Label CssClass="Formatolbl" ID="lblMontoOtorgado" runat="server" Style="left: 15px; top: 9px;">Monto otorgado:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblPlazo" runat="server" Style="left: 15px; top: 35px;">Plazo otorgado:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="Label1" runat="server" Style="left: 15px; top: 61px;">Valor de la cuota:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblCuotasPagadas" runat="server" Style="left: 15px; top: 87px;">Cuotas pagadas:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblFechaInicio" runat="server" Style="left: 15px; top: 113px;">Inicio de credito:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblFechaFin" runat="server" Style="left: 15px; top: 139px;">Final de credito:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblDiasAtraso" runat="server" Style="left: 15px; top: 165px;">Dias en atraso:</asp:Label>

                        <asp:TextBox ID="txtSaldoCapital" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 425px; top: 5px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtCapitalVencido" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 425px; top: 31px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtInteresVencido" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 425px; top: 57px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtInteresMora" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 425px; top: 83px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtSaldoSeguros" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 425px; top: 109px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtSaldosVarios" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 425px; top: 135px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtCuotasAtrasadas" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 425px; top: 161px; width: 100px;" ReadOnly="True"></asp:TextBox>

                        <asp:Label CssClass="Formatolbl" ID="lblSaldoCapital" runat="server" Style="left: 280px; top: 9px;">Saldo capital:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblCapitalVencido" runat="server" Style="left: 280px; top: 35px;">Capital vencido:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblInteresOrdinario" runat="server" Style="left: 280px; top: 61px;">Interes ordinario:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblInteresMoratorio" runat="server" Style="left: 280px; top: 87px;">Interes moratorio:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblSaldoSeguros" runat="server" Style="left: 280px; top: 113px;">Seguros:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblRecargos" runat="server" Style="left: 280px; top: 139px;">Gastos por cobranza:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblCuotasAtrasadas" runat="server" Style="left: 280px; top: 165px;">Cuotas atrasadas:</asp:Label>





                        <asp:TextBox ID="txtTotalAtrasado" runat="server" CssClass="FormatotxtMonedaROBold" Style="left: 710px; top: 5px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtTotalAdeudado" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 710px; top: 31px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtCuotasPendientes" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 710px; top: 57px; width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtTasa" runat="server" ReadOnly="True" Visible="false"></asp:TextBox>

                        <asp:Label CssClass="FormatolblBold" ID="lblMontoPonerAlDia" runat="server" Style="left: 565px; top: 9px;">Monto ponerse al dia:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblMontoLiquidar" runat="server" Style="left: 565px; top: 35px;">Monto para liquidar:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblCuotasPendientes" runat="server" Style="left: 565px; top: 61px;">Cuotas pendientes:</asp:Label>

                        <asp:Panel ID="PanelDatosReadecuacion" runat="server" CssClass="FormatoPanel" Height="68px" Width="275px" Style="top: 100px; left: 565px;">

                            <asp:TextBox ID="txtAbonoInicial" input="tel" type="number" step="0" runat="server" CssClass="FormatotxtMoneda" Style="left: 110px; top: 25px; width: 70px;" ValidationExpression="\d+"></asp:TextBox>

                            <asp:Label CssClass="Formatolbl" ID="lblAbonoInicial" runat="server" Style="left: 15px; top: 29px;" Text="Abono inicial:" />

                            <asp:Button ID="btnCalcularConvenio" runat="server" CssClass="FormatoBotonesIconoArribaCuadrado60" Style="left: 210px; top: 5px; width: 60px; height: 60px; background-image: url(/Imagenes/iconoCotizador24.png);" Text="Calcular" OnClick="btnCalcularConvenio_Click" />

                        </asp:Panel>

                    </asp:Panel>

                    <asp:Panel CssClass="FormatoPanelTodoBlancoHumo" ID="Panel1" runat="server" Width="898px" Height="270px">
                        <div class="DivTituloGrid" style="height: 80px; font-weight: bold;">
                            <table class="TableFormatoTituloGrid" style="color: black;">
                                <tr>
                                    <td style="width: 290px; background-color: antiquewhite;">Opcion A</td>
                                    <td style="width: 290px; background-color: aquamarine;">Opcion B</td>
                                    <td style="width: 290px; background-color: lightblue;">Opcion C</td>
                                </tr>
                                <tr>
                                    <td style="width: 290px; background-color: antiquewhite;">Readeucacion de Ley</td>
                                    <td style="width: 290px; background-color: aquamarine;">Mini Pmo x Interes Atraso</td>
                                    <td style="width: 290px; background-color: lightblue;">Mini Pmo x Interes Atraso</td>
                                </tr>
                                <tr>
                                    <td style="width: 290px; background-color: antiquewhite;">Capitalizando Interes Atrasado</td>
                                    <td style="width: 290px; background-color: aquamarine;">Int. Atraso + Sldo Capital</td>
                                    <td style="width: 290px; background-color: lightblue;">Int. Atraso + Sldo Capital</td>
                                </tr>
                                <tr>
                                    <td style="width: 290px; background-color: antiquewhite;">Misma cuota / Plazo extenso</td>
                                    <td style="width: 290px; background-color: aquamarine;">Cuota mayor / Plazo pendiente</td>
                                    <td style="width: 290px; background-color: lightblue;">Misma cuota / Plazo poco largo</td>
                                </tr>
                            </table>
                        </div>
                        <asp:GridView ID="gvOpciones" runat="server" CssClass="GridViewFormatoGeneralLista" HeaderStyle-CssClass="GridViewCabecera" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False" ShowHeader="False" RowStyle-Height="24px">
                            <Columns>
                                <asp:BoundField DataField="fcDatosOpcionA" ItemStyle-Width="191px" ItemStyle-BackColor="antiquewhite" />
                                <asp:BoundField DataField="fnValoresOpcionA" ItemStyle-Width="90px" ItemStyle-BackColor="antiquewhite" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                                <asp:BoundField DataField="fcDatosOpcionB" ItemStyle-Width="190px" ItemStyle-BackColor="aquamarine" />
                                <asp:BoundField DataField="fnValoresOpcionB" ItemStyle-Width="90px" ItemStyle-BackColor="aquamarine" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                                <asp:BoundField DataField="fcDatosOpcionC" ItemStyle-Width="190px" ItemStyle-BackColor="lightblue" />
                                <asp:BoundField DataField="fnValoresOpcionC" ItemStyle-Width="90px" ItemStyle-BackColor="lightblue" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                            </Columns>
                            <HeaderStyle CssClass="GridViewCabecera" />
                            <RowStyle CssClass="GridViewFilas" />

                        </asp:GridView>

                        <div style="padding-top:5px;">
                            <table class="TableFormatoTituloGrid" style="color: black;">
                                <tr>
                                    <td style="width: 290px; background-color: antiquewhite;">
                                        <asp:Button ID="btnOpcionAGeneraraPDF" runat="server" CssClass="FormatoBotonesIconoArribaCuadrado60" Style="left: 100px; width: 105px; height: 80px; background-image: url(/Imagenes/iconoImprimir24.png);" Text="Descargar acuerdo Opción A" OnClick="btnOpcionAGeneraraPDF_Click" />
                                    </td>
                                    <td style="width: 290px; background-color: aquamarine;">
                                        <asp:Button ID="btnOpcionBGeneraraPDF" runat="server" CssClass="FormatoBotonesIconoArribaCuadrado60" Style="left: 395px; width: 105px; height: 80px; background-image: url(/Imagenes/iconoImprimir24.png);" Text="Descargar acuerdo Opción B" OnClick="btnOpcionBGeneraraPDF_Click" />
                                    </td>
                                    <td style="width: 290px; background-color: lightblue;">
                                        <asp:Button ID="btnOpcionCGeneraraPDF" runat="server" CssClass="FormatoBotonesIconoArribaCuadrado60" Style="left: 695px; width: 105px; height: 80px; background-image: url(/Imagenes/iconoImprimir24.png);" Text="Descargar acuerdo Opción C" OnClick="btnOpcionCGeneraraPDF_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>


                    </asp:Panel>

                </asp:Panel>
                <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" Style="left: 5px; color: red;"></asp:Label>

                <asp:Panel ID="PanelErrores" runat="server" CssClass="FormatoPanelAlertaRojo" Height="50px" Width="350px" Visible="false">
                    <asp:Label CssClass="Formatolbl" ID="lblMensajes" runat="server" Style="color: white; margin: 5px; margin-left: 55px;"></asp:Label>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
