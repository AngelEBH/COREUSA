<%@ Page Language="C#" AutoEventWireup="true" Inherits="Clientes_CotizadorCarros" Codebehind="CotizadorCarros.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css"/>
</head>
<body>
    <form id="frmCalculadora" runat="server" style="width: 100%; height:100%; position: absolute; left:0; top:0;">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMultiView">
            <ContentTemplate>

        <div class="DivPanelListas" style="width:700px;">
            <div class="DivTituloPanel">
                <table style="height:100%;"><tr><td class="TableFormatoTitulo">Cotizador de Prestadito Automovil</td></tr></table>
            </div>
            <asp:Panel ID="Panel20" runat="server" CssClass="FormatoPanelTodoBlanco" Height="140px" style="background-color:whitesmoke;">

                <asp:DropDownList ID="ddlProducto" runat="server" CssClass="FormatoDDL"                     style="left: 190px; top: 5px;   width: 120px;" OnSelectedIndexChanged="ddlProducto_SelectedIndexChanged" AutoPostBack="True"  ></asp:DropDownList>
                <asp:TextBox ID="txtValorVehiculo" runat="server" CssClass="FormatotxtMoneda"   input="tel" style="left: 190px; top: 31px;  width: 80px; " ></asp:TextBox>
                <asp:TextBox ID="txtValorPrima"    runat="server" CssClass="FormatotxtMoneda"   input="tel" style="left: 190px; top: 57px;  width: 80px; " ></asp:TextBox>
                <asp:TextBox ID="txtMonto"         runat="server" CssClass="FormatotxtMonedaRO" input="tel" style="left: 190px; top: 83px;  width: 80px; " Enabled="false"></asp:TextBox>
                <asp:TextBox ID="txtScorePromedio" runat="server" CssClass="FormatotxtMoneda"   input="tel" style="left: 190px; top: 109px; width: 80px; " ></asp:TextBox>

                <asp:Label CssClass="Formatolbl" ID="lblProducto"         runat="server" style="left:15px;  top:9px;   " Text="Producto:" />
                <asp:Label CssClass="Formatolbl" ID="lblValor"            runat="server" style="left:15px;  top:35px;  " Text="Valor de vehiculo:" />
                <asp:Label CssClass="Formatolbl" ID="lblPrima"            runat="server" style="left:15px;  top:61px;  " Text="Valor de la prima:"/>
                <asp:Label CssClass="Formatolbl" ID="lblPorcenajedePrima" runat="server" style="left:290px; top:87px;  width:50px;"/>
                <asp:Label CssClass="Formatolbl" ID="lblMonto"            runat="server" style="left:15px;  top:87px;  " Text="Valor a financiar del vehiculo:"/>
                <asp:Label CssClass="Formatolbl" ID="lblScorePromedio"    runat="server" style="left:15px;  top:113px; " Text="Score promedio:"/>

                <asp:DropDownList ID="ddlGastosdeCierre" runat="server" CssClass="FormatoDDL" style="left: 430px; top: 5px;  width: 150px;" ></asp:DropDownList>
                <asp:DropDownList ID="ddlSeguro"         runat="server" CssClass="FormatoDDL" style="left: 430px; top: 31px; width: 150px;" ></asp:DropDownList>
                <asp:DropDownList ID="ddlGPS"            runat="server" CssClass="FormatoDDL" style="left: 430px; top: 57px; width: 150px;" ></asp:DropDownList>
                
                <asp:Label CssClass="Formatolbl" ID="lblGastosdeCierre" runat="server" style="left:330px;  top:9px;  " Text="Gastos de cierre:" />
                <asp:Label CssClass="Formatolbl" ID="lblSeguro"         runat="server" style="left:330px;  top:35px; " Text="Tipo de seguro:" />
                <asp:Label CssClass="Formatolbl" ID="lblGPS"            runat="server" style="left:330px;  top:61px; " Text="Lleva GPS:" />



                <asp:Button CssClass="FormatoBotonesIconoArriba" ID="btnCalcular" runat="server" Text="Calcular"  style="background-image:url('/Imagenes/iconoCotizador24.png'); left: 590px; top: 5px; " OnClick="btnCalcular_Click" />



            </asp:Panel>
        </div>

        <asp:Panel CssClass="FormatoPanelNOAB"  ID ="PanelCotizadorResultado" runat="server" style="margin-top:5px;width:700px; background-color:whitesmoke;" Visible="false">
            <div class="DivTituloPanel">
                <table style="height:100%;"><tr><td class="TableFormatoTitulo">Cotizacion por plazos</td></tr></table>
            </div>
            <asp:GridView ID="gvCotizador" runat="server" CssClass="GridViewFormatoGeneral" HeaderStyle-CssClass="GridViewCabecera" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False" ShowHeader="True" style="word-break:break-all;" RowStyle-Height="24px" >
                <Columns>
                    <asp:BoundField DataField="fiIDPlazo"               HeaderText="Plazo"          ItemStyle-Width="50px"  ItemStyle-HorizontalAlign="Center" />
                    <asp:BoundField DataField="fnCuotadelPrestamo"      HeaderText="Cuota Prestamo" ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                    <asp:BoundField DataField="fnCuotaSegurodeVehiculo" HeaderText="Cuota Seguro"   ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                    <asp:BoundField DataField="fnCuotaServicioGPS"      HeaderText="Cuota GPS"      ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                    <asp:BoundField DataField="fnTotalCuota"            HeaderText="Total Cuota."   ItemStyle-Width="90px"  ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" DataFormatString="{0:#,###0.00}" />
                </Columns>
                <HeaderStyle CssClass="GridViewCabecera" />
                <RowStyle CssClass="GridViewFilas" />
            </asp:GridView>
        </asp:Panel>

        <asp:Panel ID="PanelErrores" runat="server" CssClass="FormatoPanelAlertaRojo" Height="50px" Width="250px" Visible="false" >
            <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" style="color:white; margin:5px; margin-left:55px; "></asp:Label>
        </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
            <asp:UpdateProgress ID="prgLoadingStatus" runat="server" AssociatedUpdatePanelID="upMultiView">
            <ProgressTemplate>
                <asp:Panel runat="server" Visible="true" BackColor="Transparent" BackImageUrl="/Imagenes/fondoTransparencia.png" Style="left: 0px; top: 0px; position: absolute; height: 100%; width: 100%;">
                    <div style="align-content: center; vertical-align: central; text-align: center; height: 100%; width: 100%;">
                        <table style="width: 100%; height: 100%;">
                            <tr>
                                <td>
                                    <asp:Image ID="imgCargando" runat="server" ImageUrl="/Imagenes/Procesando.gif" Style="background-color: transparent; align-content: center; vertical-align: central; margin-top: auto;" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </asp:Panel>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>
