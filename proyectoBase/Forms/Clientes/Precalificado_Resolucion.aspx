<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Precalificado_Resolucion.aspx.cs" Inherits="Clientes_Precalificado_Resolucion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div class="DivPanelMenuEspaciadorTop"></div>
        <div class="DivPanelListas" style="width:1000px;">
            <div class="DivTituloPanel">
                <table style="height:100%;">
                    <tr>
                        <td class="TableFormatoTitulo">
                            Resultados...
                        </td>
                    </tr>
                </table>
            </div>
            <asp:Panel ID="Panel19" runat="server" CssClass="FormatoPanelTodoBlanco" Height="202px">
            <!-- Datos generales de cliente -->
                <asp:Panel ID="Panel2" runat="server" CssClass="FormatoPanelSubNOAB" Height="116px" Width="718px" style="left:5px; top:5px;">
                    <div class="DivTituloPanelGrande">
                        <table style="height:100%;"><tr><td class="TableFormatoTituloGrande">Datos Generales</td></tr></table>
                    </div>
                    <asp:Panel ID="Panel20" runat="server" CssClass="FormatoPanelTodoBlanco" Height="86px" style="background-color:whitesmoke;">
                        <asp:TextBox ID="txtIdentidad"              runat="server" CssClass="FormatotxtRO"       style="left: 75px;  top: 5px;  width: 100px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtNombreCompleto"         runat="server" CssClass="FormatotxtRO"       style="left: 190px; top: 5px;  width: 240px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtTelefonoRegistrado"     runat="server" CssClass="FormatotxtRO"       style="left: 75px;  top: 31px; width: 80px;"  ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtIngresos"               runat="server" CssClass="FormatotxtMonedaRO" style="left: 535px; top: 5px;  width: 80px;"  ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtObligaciones"           runat="server" CssClass="FormatotxtMonedaRO" style="left: 535px; top: 31px; width: 80px;"  ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtDisponible"             runat="server" CssClass="FormatotxtMonedaRO" style="left: 535px; top: 57px; width: 80px;"  ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtActividadeconomica"     runat="server" CssClass="FormatotxtRO"       style="left: 280px; top: 31px; width: 150px;" ReadOnly="True"></asp:TextBox>
                        <asp:TextBox ID="txtResultadoPrecalificado" runat="server" CssClass="FormatotxtRO"       style="left: 160px; top: 57px; width: 270px;" ReadOnly="True"></asp:TextBox>

                        <asp:Label CssClass="Formatolbl" ID="lblIdentidad"          runat="server" style="left:5px;   top:9px;  ">Identidad:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblTelefonoRegistrado" runat="server" style="left:5px;   top:35px; ">Telefono:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblIngresos"           runat="server" style="left:450px; top:9px;  ">Ingresos:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblObligaciones"       runat="server" style="left:450px; top:35px; ">Obligaciones:</asp:Label>                            
                        <asp:Label CssClass="Formatolbl" ID="lblDisponible"    runat="server" style="left:450px; top:61px; ">Disponible:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblActividad"          runat="server" style="left:190px; top:35px; ">Actividad:</asp:Label>
                        <asp:Label CssClass="Formatolbl" ID="lblReglaAplicada"      runat="server" style="left:5px;   top:61px; ">Resultado pre-calificado:</asp:Label>
                    
                    </asp:Panel>
                </asp:Panel>

                <!-- Datos SAF, IHSS, CNE, CallCenter -->
                <asp:Panel ID="Panel3" runat="server" CssClass="FormatoPanelSubNOAB" Height="74px" Width="988px" style="left:5px; top:127px; position:absolute; vertical-align:middle;">
                    <div class="DivTituloPanel">
                        <table style="height:100%;"><tr><td class="TableFormatoTituloGrande">Indicadores de perfil</td></tr></table>
                    </div>
                    <asp:Table runat="server" ID="Table1" class="TableFormatoPrecalificado" style="height:50px; background-color:whitesmoke;">
                        <asp:TableRow runat="server" Height="50%">
                            <asp:TableCell runat="server" id="idA1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblSAF5"           runat="server" ForeColor="Silver" Text="SAF5" /></asp:TableCell><asp:TableCell runat="server" id="idB1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblIHSS"           runat="server" ForeColor="Silver" Text="IHSS" /></asp:TableCell><asp:TableCell runat="server" id="idC1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblRNP"            runat="server" ForeColor="Silver" Text="RNP" /></asp:TableCell><asp:TableCell runat="server" id="idD1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblCallCenter"     runat="server" ForeColor="Silver" Text="CallCenter" /></asp:TableCell><asp:TableCell runat="server" id="idE1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblComunicaciones" runat="server" ForeColor="Silver" Text="Com.Mora" /></asp:TableCell><asp:TableCell runat="server" id="idF1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblMoraMayor"      runat="server" ForeColor="Silver" Text="Mora Mayor" /></asp:TableCell><asp:TableCell runat="server" id="idG1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblMoraMenor"      runat="server" ForeColor="Silver" Text="Mora Menor" /></asp:TableCell><asp:TableCell runat="server" id="idH1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblMoraHistorica"  runat="server" ForeColor="Silver" Text="Mora Hist." /></asp:TableCell><asp:TableCell runat="server" id="idI1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblSobregiro"      runat="server" ForeColor="Silver" Text="Sobregiro" /></asp:TableCell><asp:TableCell runat="server" id="idJ1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblScoreBajo"      runat="server" ForeColor="Silver" Text="Score Bajo" /></asp:TableCell><asp:TableCell runat="server" id="idK1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblSaldosCastig"   runat="server" ForeColor="Silver" Text="Saldo Castig." /></asp:TableCell><asp:TableCell runat="server" id="idL1" style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblIncobIrrecup"   runat="server" ForeColor="Silver" Text="Incob. Irrec." /></asp:TableCell><asp:TableCell runat="server" id="idM1" style="font-size:12px; "><asp:Label ID="lblJuridicoLegal" runat="server" ForeColor="Silver" Text="Jurid.Legal" /></asp:TableCell></asp:TableRow><asp:TableRow>
                            <asp:TableCell runat="server" id="idA2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgSAF"            CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idB2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgIHSS"           CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idC2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgRNP"            CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idD2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgCallCenter"     CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idE2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgComunicaciones" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoAlerta24.png"  /></asp:TableCell>
                            <asp:TableCell runat="server" id="idF2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgMoraMayor"      CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idG2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgMoraMenor"      CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idH2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgMoraHistorica"  CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idI2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgSobregiro"      CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idJ2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgScoreMenor"     CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idK2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgCastigado"      CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idL2" style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgIncobrable"     CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></asp:TableCell>
                            <asp:TableCell runat="server" id="idM2" ><asp:Image ID="imgJuridico"   CssClass="ImgIcono24NOAB" runat="server"  Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></asp:TableCell>
						</asp:TableRow>
					</asp:Table>
                </asp:Panel>


                <!-- Resumen de IHSS -->
                <asp:Panel ID="PanelDetalleIHSS" runat="server" CssClass="FormatoPanelSubNOAB" Height="116px" Width="263px" style="left:730px; top:5px; position:absolute; vertical-align:middle;">
                    <div class="DivTituloPanelGrande">
                        <table style="height:100%;">
                            <tr>
                                <td class="TableFormatoTituloGrande">
                                    Información de IHSS </td></tr></table></div><div class="DivPanelMenuEspaciadorPaneles"></div>
                    <asp:GridView ID="gvIHSS" runat="server" CssClass="GridViewFormatoGeneral" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False">
                        <Columns>
                            <asp:BoundField DataField="fcYear"    ReadOnly="True" ItemStyle-Width="40px"  ItemStyle-Wrap="false"/>
                            <asp:BoundField DataField="fcEmpresa" ReadOnly="True" ItemStyle-Width="200px" ItemStyle-Wrap="false"/>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </asp:Panel>


            <asp:Panel ID="Panel18" runat="server" CssClass="FormatoPanelSubNOAB" Width="988px" style="left:5px; margin-top:5px;">
                <div class="DivTituloPanelGrande">
                    <table style="height:100%;"><tr><td class="TableFormatoTituloGrande">Resolución</td></tr></table></div><asp:Panel ID="Panel1" runat="server" CssClass="FormatoPanelTodoBlanco" Width="986px" style="left:0px; height:100px;  background-color:whitesmoke;">

                    <asp:RadioButton ID="rbAprobado"  runat="server" CssClass="Formatolbl" GroupName="rbgPrecalificado" style="left:100px; top:5px; font-weight:bold; font-size:14px; color:green;" Text="Cliente aprobado" OnCheckedChanged="rbAprobado_CheckedChanged"/>
                    <asp:RadioButton ID="rbRechazado" runat="server" CssClass="Formatolbl" GroupName="rbgPrecalificado" style="left:300px; top:5px; font-weight:bold; font-size:14px; color:red;" Text="Cliente rechazado" OnCheckedChanged="rbRechazado_CheckedChanged"/>

                    <asp:TextBox ID="txtObservaciones" runat="server" CssClass="Formatotxt" MaxLength="100" style="left: 130px; top: 31px; width: 545px; height: 40px; " TextMode="MultiLine"></asp:TextBox><asp:Label  ID="lblObservaciones" runat="server" CssClass="Formatolbl" style="left:35px;  top:44px;">Observaciones:</asp:Label><asp:Button ID="imgbtnGuardar"  runat="server" CssClass="FormatoBotonesAzul" OnClick="imgbtnGuardar_Click"  style="background-image: url('/Imagenes/iconoGuardar24.png');  top: 15px; left: 705px;" Text="Guardar" />
                    <asp:Button ID="imgbtnCancelar" runat="server" CssClass="FormatoBotonesAzul" OnClick="imgbtnCancelar_Click" style="background-image: url('/Imagenes/iconoCancelar24.png'); top: 50px; left: 705px;" Text="Cancelar" />

                </asp:Panel>                
            </asp:Panel>
            <div class="DivPanelMenuEspaciadorPaneles"></div>
        </div>

        <asp:Panel ID="PanelErrores" runat="server" CssClass="FormatoPanelContenedorBordes" Height="50px" Width="200px" Visible="false" style="left:calc(100vw - 300px); position:absolute; top:10px; background-color:darkred; border-color:red;">
            <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" style="color:white; margin:5px;">
            </asp:Label></asp:Panel></form></body></html>