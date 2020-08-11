<%@ Page Language="C#" AutoEventWireup="true" CodeFile="precalificado_buscador.aspx.cs" Inherits="Creditos_precalificado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMultiView">
            <ContentTemplate>
                <asp:Panel ID="PanelDatosCliente" runat="server" CssClass="FormatoPanelNOAB" Height="276px" Width="700px">
                    <div class="DivTituloPanel">
                        <table style="height: 100%;">
                            <tr>
                                <td class="TableFormatoTitulo">Precalificado de clientes
                                </td>
                            </tr>
                        </table>
                    </div>

                    <asp:TextBox ID="txtIdentidad"     runat="server" CssClass="Formatotxt" Style="left: 130px; top: 31px; width: 100px;" ValidationExpression="\d+"/>
                    <asp:TextBox ID="txtNombreCliente" runat="server" CssClass="Formatotxt" Style="left: 244px; top: 31px; width: 286px;" Visible="false"/>
                    <asp:TextBox ID="txtTelefono"      runat="server" CssClass="Formatotxt" Style="left: 130px; top: 57px; width: 100px;" />
                    <asp:TextBox ID="txtIngresos"      runat="server" CssClass="Formatotxt" Style="left: 430px; top: 57px; width: 100px;" ValidationExpression="\d+"/>

                    <asp:Label CssClass="Formatolbl" ID="lblIdentidad" runat="server" Style="left: 10px; top: 35px;">No. Identidad:</asp:Label>
                    <asp:Label CssClass="Formatolbl" ID="lblTelefono"  runat="server" Style="left: 10px; top: 61px;">Telefono movil:</asp:Label>
                    <asp:Label CssClass="Formatolbl" ID="lblIngresos"  runat="server" Style="left: 300px; top: 61px;">Ingreso mensual:</asp:Label>

                    <asp:DropDownList ID="ddlCiudadResidencia" runat="server" CssClass="FormatoDDL" Style="left: 130px; top: 83px; width: 150px;"></asp:DropDownList>
                    <asp:DropDownList ID="ddlProducto" runat="server" CssClass="FormatoDDL" Style="left: 430px; top: 83px; width: 200px;" />
                    <asp:DropDownList ID="ddlOrigenIngreso" runat="server" CssClass="FormatoDDL" Style="left: 130px; top: 109px; width: 150px;" OnSelectedIndexChanged="ddlOrigenIngreso_SelectedIndexChanged"  AutoPostBack="True" />

                    <asp:Label CssClass="Formatolbl" ID="lblCiudadResidencia" runat="server" Style="left: 10px; top: 87px;">Ciudad residencia:</asp:Label>
                    <asp:Label CssClass="Formatolbl" ID="lblProducto" runat="server" Style="left: 300px; top: 87px;">Producto solicitado:</asp:Label>
                    <asp:Label CssClass="Formatolbl" ID="lblTipodeIngreso" runat="server" Style="left: 10px; top: 113px;">Origen de ingresos:</asp:Label>


                    <asp:Panel runat="server" CssClass="FormatoPanelContenedor" ID="PanelAsalariado" Visible="false" style="top: 145px; left:5px; width:500px; height:80px;">
                        <asp:Label CssClass="Formatolbl" ID="lblClienteTrabajoMaquila"     runat="server" Style="left: 10px;  top: 5px; ">Trabaja en sector maquila o CallCenter?</asp:Label>
                        <asp:RadioButton CssClass="Formatolbl" ID="rbMaquilaoCallCenterSI" runat="server" Style="left: 350px; top: 5px;" Text="Si" GroupName="vgMaquilaoCallCenter" />
                        <asp:RadioButton CssClass="Formatolbl" ID="rbMaquilaoCallCenterNO" runat="server" Style="left: 410px; top: 5px;" Text="No" GroupName="vgMaquilaoCallCenter" />

                        <asp:Label CssClass="Formatolbl" ID="lblCasaPropia"      runat="server" Style="left: 10px;  top: 31px;">Tiene casa propia(a su nombre, mamá, papá, esposo (a))?</asp:Label>
                        <asp:RadioButton CssClass="Formatolbl" ID="rbCasaPropiaSI" runat="server" Style="left: 350px; top: 31px;" Text="Si" GroupName="vgCasaPropia" />
                        <asp:RadioButton CssClass="Formatolbl" ID="rbCasaPropiaNO" runat="server" Style="left: 410px; top: 31px;" Text="No" GroupName="vgCasaPropia" />

                        <asp:Label CssClass="Formatolbl" ID="lblGuardiadeSeguridad" runat="server" Style="left: 10px;  top: 57px;">Trabaja como guardia de seguridad?</asp:Label>
                        <asp:RadioButton CssClass="Formatolbl" ID="rbGuardiaSI" runat="server" Style="left: 350px; top: 57px;" Text="Si" GroupName="vgGuardia" />
                        <asp:RadioButton CssClass="Formatolbl" ID="rbGuardiaNO" runat="server" Style="left: 410px; top: 57px;" Text="No" GroupName="vgGuardia" />
                    </asp:Panel>


                    <asp:Panel runat="server" CssClass="FormatoPanelContenedor" ID="PanelComerciante" Visible="false" style="top: 145px; left:5px; width:500px; height:80px;">

                        <asp:Label CssClass="Formatolbl" ID="lblAntiguedadNegocio"     runat="server" Style="left: 10px;  top: 5px; ">Antigüedad del negocio es >=12 Meses?</asp:Label>
                        <asp:RadioButton CssClass="Formatolbl" ID="rbAntiguedaddeNegocioSI" runat="server" Style="left: 350px; top: 5px;" Text="Si" GroupName="vgAntiguedadNegocio" />
                        <asp:RadioButton CssClass="Formatolbl" ID="rbAntiguedaddeNegocioNO" runat="server" Style="left: 410px; top: 5px;" Text="No" GroupName="vgAntiguedadNegocio" />

                        <asp:Label CssClass="Formatolbl" ID="lblPermisodeOperacion"      runat="server" Style="left: 10px;  top: 31px;">Cuenta con permiso de operacion vigente?</asp:Label>
                        <asp:RadioButton CssClass="Formatolbl" ID="rbPermisoOperacionSI" runat="server" Style="left: 350px; top: 31px;" Text="Si" GroupName="vgPermisodeOperacion" />
                        <asp:RadioButton CssClass="Formatolbl" ID="rbPermisoOperacionNO" runat="server" Style="left: 410px; top: 31px;" Text="No" GroupName="vgPermisodeOperacion" />

                        <asp:Label CssClass="Formatolbl" ID="lblCasaPropiaComerciante" runat="server" Style="left: 10px;  top: 57px;">Tiene casa propia(a su nombre, mamá, papá, esposo (a))?</asp:Label>
                        <asp:RadioButton CssClass="Formatolbl" ID="rbCasaComercianteSI"    runat="server" Style="left: 350px; top: 57px;" Text="Si" GroupName="vgCasaComerciante" />
                        <asp:RadioButton CssClass="Formatolbl" ID="rbCasaComercianteNO"    runat="server" Style="left: 410px; top: 57px;" Text="No" GroupName="vgCasaComerciante" />

                    </asp:Panel>


                </asp:Panel>
                <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" Style="margin-left: 5px; color: red;"></asp:Label>

            </ContentTemplate>
        </asp:UpdatePanel>
        
        <asp:Button ID="btnConsultarCliente" runat="server" CssClass="FormatoBotonesAzulBuscador" Text="Consultar" Style="left: 520px; top: 239px;" OnClick="btnConsultarCliente_Click" />
    </form>
</body>
</html>
