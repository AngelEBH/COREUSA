<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrestamosMenu.aspx.cs" Inherits="Prestamos_PrestamosMenu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMenu">
            <ContentTemplate>
<%--                <div class="DivPanelMenu">
                    <div class="DivTituloPanel">
                        <table style="height: 100%;">
                            <tr>
                                <td class="TableFormatoTitulo">Busqueda de clientes...</td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel CssClass="FormatoPanelTodoBlanco" ID="Panel1" runat="server" Style="height: 34px;">
                        <asp:TextBox CssClass="Formatotxt" ID="txtBusqueda" runat="server" Style="left: 55px; top: 5px; width: 115px;"></asp:TextBox>
                        <asp:Label CssClass="Formatolbl" ID="lblBuscar" runat="server" Style="left: 5px; top: 9px;">Buscar:</asp:Label>
                        <asp:ImageButton ID="btnBuscar" runat="server" Style="position: absolute; top: 5px; left: 187px;" ImageUrl="/Imagenes/IconoBuscarCliente24.png" OnClick="btnBuscar_Click" />
                    </asp:Panel>
                </div>

                <div class="DivPanelMenuEspaciadorTransparente"></div>--%>

                <div class="DivPanelMenu">
                    <div class="DivTituloPanel">
                        <table style="height: 100%;">
                            <tr>
                                <td class="TableFormatoTitulo">Menu Prestamos</td>
                            </tr>
                        </table>
                    </div>
                    <asp:Button CssClass="btnMenuLateral" ID="btnMisPrestamos"  runat="server" Style="background-image: url(/Imagenes/iconoBandejadeSolicitudes16.png);" Text="Mis Prestamos"  OnClick="btnMisPrestamos_Click"></asp:Button>
                    <div class="DivPanelMenuEspaciadorItems"></div>
                    <asp:Button CssClass="btnMenuLateral" ID="btnPickUpPayment" runat="server" Style="background-image: url(/Imagenes/iconoPrestamo16.png);"             Text="Pickup Payment" OnClick="btnPickUpPayment_Click"></asp:Button>
                    <div class="DivPanelMenuEspaciadorItems"></div>
                    <asp:Button CssClass="btnMenuLateral" ID="btnCrearPrestamos" runat="server" Style="background-image: url(/Imagenes/iconoDinero16.png);"              Text="Crear Prestamo" OnClick="btnCrearPrestamos_Click"></asp:Button>
                    <div class="DivPanelMenuEspaciadorItems"></div>
                </div>

                <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" Style="left: 5px; top: 75px; color: red;"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
