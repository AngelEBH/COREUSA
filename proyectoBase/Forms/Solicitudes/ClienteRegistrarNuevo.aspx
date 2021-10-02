<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClienteRegistrarNuevo.aspx.cs" Inherits="Solicitudes_ClienteRegistrarNuevo" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="DivPanelListas" style="width: 774px; background-color: whitesmoke;">
            <div class="DivTituloPanel">
                <table style="height: 100%;">
                    <tr>
                        <td class="TableFormatoTitulo">Busar cliente para ingreso de solicitud</td>
                    </tr>
                </table>
            </div>
            <!-- Datos generales de cliente -->
            <asp:Panel ID="Panel19" runat="server" CssClass="FormatoPanelTodoBlancoHumo">
                <asp:Panel ID="Panel2" runat="server" CssClass="FormatoPanelSubNOAB" Width="760px" Style="margin-left: 5px; margin-top: 5px;">
                    <div class="DivTituloPanel">
                        <table style="height: 100%;">
                            <tr>
                                <td class="TableFormatoTitulo">Datos del cliente</td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="Panel20" runat="server" CssClass="FormatoPanelTodoBlancoHumo" Height="132px">
                        <asp:TextBox ID="txtIdentidad"     runat="server" CssClass="FormatotxtRO" Style="left: 100px; top: 5px;   width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtFirstName"     runat="server" CssClass="Formatotxt"   Style="left: 100px; top: 31px;  width: 150px;" ></asp:TextBox>
                        <asp:TextBox ID="txtMiddleName"    runat="server" CssClass="Formatotxt"   Style="left: 400px; top: 31px;  width: 150px;" ></asp:TextBox>
                        <asp:TextBox ID="txtLastName"      runat="server" CssClass="Formatotxt"   Style="left: 100px; top: 57px;  width: 150px;" ></asp:TextBox>
                        <asp:TextBox ID="txtBirthDay"      runat="server" CssClass="Formatotxt"   Style="left: 400px; top: 57px;  width: 150px;" type="date"   ></asp:TextBox>
                        <asp:TextBox ID="txtIngresos"      runat="server" CssClass="Formatotxt"   Style="left: 100px; top: 83px;  width: 150px;" type="number" ></asp:TextBox>
                        <asp:DropDownList ID="ddlProducto" runat="server" CssClass="Formatotxt"   Style="left: 400px; top: 83px;  width: 150px;" ></asp:DropDownList>

                        <asp:Label CssClass="Formatolbl" ID="lblIdentidad"    runat="server" Style="left: 5px;   top: 9px;  ">Identification:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblPrimerNombre" runat="server" Style="left: 5px;   top: 35px; ">First name:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="Label1"          runat="server" Style="left: 305px; top: 35px; ">Middle name:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="Label2"          runat="server" Style="left: 5px;   top: 61px; ">Last name:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="Label3"          runat="server" Style="left: 305px; top: 61px; ">Birthday:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="Label4"          runat="server" Style="left: 5px;   top: 87px; ">Ingresos:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="Label5"          runat="server" Style="left: 305px; top: 87px; ">Producto:</asp:Label>
                        <asp:Button ID="btnIngresarSolicitud" runat="server" CssClass="FormatoBotonesIconoCuadrado40" Style="position: absolute; left: 674px; top: 5px; background-image: url(/Imagenes/iconoSolicitudAgregar40.png);" Text="Guardar y continuar" OnClick="btnIngresarSolicitud_Click" />
                    </asp:Panel>
                </asp:Panel>
            </asp:Panel>
            <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" Style="margin-left: 5px; margin-top: 5px; color: hotpink; font-weight: bold;"></asp:Label>
        </div>

    </form>
</body>
</html>
