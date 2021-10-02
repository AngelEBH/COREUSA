<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClienteparaSolicitud.aspx.cs" Inherits="Clientes_ClienteparaSolicitud" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upClientes">
            <ContentTemplate>
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
                                        <td class="TableFormatoTitulo">Datos generales</td>
                                    </tr>
                                </table>
                            </div>
                            <asp:Panel ID="Panel20" runat="server" CssClass="FormatoPanelTodoBlancoHumo" Height="150px">
                                <asp:TextBox ID="txtIdentidad" runat="server" CssClass="FormatotxtRO" Style="left: 115px; top: 5px; width: 100px;"></asp:TextBox>
                                <asp:TextBox ID="txtNombreCompleto" runat="server" CssClass="FormatotxtRO" Style="left: 270px; top: 5px; width: 210px;" ReadOnly="True"></asp:TextBox>
                               <%-- <asp:TextBox ID="txtTelefonoRegistrado" runat="server" CssClass="FormatotxtRO" Style="left: 75px; top: 31px; width: 200px;" ReadOnly="True"></asp:TextBox>--%>
                                <asp:TextBox ID="txtIngresosRegistrados" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 115px; top: 31px; width: 120px;" ReadOnly="True"></asp:TextBox>
                                <%--<asp:TextBox ID="txtAntiguedadActiva" runat="server" CssClass="FormatotxtRO" Style="left: 535px; top: 31px; width: 120px; text-align: center; background-color: white;" ReadOnly="True"></asp:TextBox>--%>
                                <asp:TextBox ID="txtInfoPrimerConsulta" runat="server" CssClass="FormatotxtRO" Style="left: 115px; top: 57px; width: 530px; height: 40px; overflow: hidden;" ReadOnly="True" TextMode="MultiLine"></asp:TextBox>
                                
                             <%--   <asp:TextBox ID="TextOrigenEtnico" runat="server" CssClass="FormatotxtRO" Style="left: 120px; top: 115px; width: 100px;"></asp:TextBox>                                
                                <asp:TextBox ID="txtNoIdentificacion" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 535px; top: 115px; width: 120px;" ReadOnly="True"></asp:TextBox>
                                <asp:TextBox ID="txtPaisNacimiento" runat="server" CssClass="FormatotxtRO" Style="left: 120px; top: 150px; width: 100px;"></asp:TextBox>
                                <asp:TextBox ID="txtDocumentacionIdentificacion" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 535px; top: 150px; width: 120px;" ReadOnly="True"></asp:TextBox>
                                <asp:TextBox ID="txtSocialSecurity" runat="server" CssClass="FormatotxtMonedaRO" Style="left: 535px; top: 180px; width: 120px;" ReadOnly="True"></asp:TextBox>--%>
                               
                                
                                <asp:Label CssClass="Formatolbl" ID="lblIdentidad" runat="server" Style="left: 5px; top: 9px;">No de Id Personal:</asp:Label>
                               <%-- <asp:Label CssClass="Formatolbl" ID="lblTelefonoRegistrado" runat="server" Style="left: 5px; top: 35px;">Telefono:</asp:Label>--%>
                                <asp:Label CssClass="Formatolbl" ID="lblIngresos" runat="server" Style="left: 5px; top: 35px;">Ingresos:</asp:Label>
                                <%--<asp:Label CssClass="Formatolbl" ID="lblAntiguedadActiva" runat="server" Style="left: 460px; top: 35px;">Buro Activo:</asp:Label>--%>
                                <asp:Label CssClass="Formatolbl" ID="lblInfoPrimerConsulta" runat="server" Style="left: 5px; top: 61px;">Consulta:</asp:Label>

                                <%-- <asp:Label CssClass="Formatolbl" ID="LblOrigenEtnico" runat="server" Style="left: 5px; top: 120px;">Origen Etnico:</asp:Label>
                                <asp:Label CssClass="Formatolbl" ID="lblNdeIdentificacion" runat="server" Style="left: 380px; top: 120px;">No de Identificacion:</asp:Label>
                                <asp:Label CssClass="Formatolbl" ID="lblPaisNacimiento" runat="server" Style="left: 5px; top: 150px;">Pais de Nacimiento:</asp:Label>
                                <asp:Label CssClass="Formatolbl" ID="lblDocumentacionIdentificacion" runat="server" Style="left: 380px; top: 150px;">Documento Identificacion:</asp:Label>
                                <asp:Label CssClass="Formatolbl" ID="lblSocialSecurity" runat="server" Style="left: 380px; top: 180px;">Social Segurity:</asp:Label>--%>



                                <asp:ImageButton ID="btnBuscar" runat="server" Style="position: absolute; top: 5px; left: 245px;" ImageUrl="/Imagenes/IconoBuscarCliente24.png" OnClick="btnBuscar_Click" />
                                <asp:Button ID="btnIngresarSolicitud" runat="server" CssClass="FormatoBotonesIconoCuadrado40" Style="position: absolute; left: 674px; top: 5px; background-image: url(/Imagenes/iconoSolicitudAgregar40.png);" Text="Ingresar solicitud" OnClick="btnIngresarSolicitud_Click" Visible="true" />


                            </asp:Panel>
                        </asp:Panel>

                        <!-- Datos SAF, IHSS, CNE, CallCenter -->
                        <asp:Panel ID="PanelInidicadores" runat="server" CssClass="FormatoPanelSubNOAB" Height="74px" Width="760px" Style="margin-left: 5px; margin-top: 5px;" Visible="false">
                            <div class="DivTituloPanel">
                                <table style="height: 100%;">
                                    <tr>
                                        <td class="TableFormatoTitulo">Indicadores de perfil</td>
                                    </tr>
                                </table>
                            </div>
                            <table class="TableFormatoPrecalificado" style="height: 50px;">
                            <tr style="height:50%;">
                                <td style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblSAF5"           runat="server" ForeColor="Silver" Text="SAF5" /></td>
                                <td style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblIHSS"           runat="server" ForeColor="Silver" Text="IHSS" /></td>
                                <td style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblRNP"            runat="server" ForeColor="Silver" Text="RNP" /></td>
                                <td style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblCallCenter"     runat="server" ForeColor="Silver" Text="CallCenter" /></td>
                                <td style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblMoraMayor"      runat="server" ForeColor="Silver" Text="Mora Mayor" /></td>
                                <td style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblSobregiro"      runat="server" ForeColor="Silver" Text="Sobregiro" /></td>
                                <td style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblScoreBajo"      runat="server" ForeColor="Silver" Text="Score Bajo" /></td>
                                <td style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblSaldosCastig"   runat="server" ForeColor="Silver" Text="Saldo Castig." /></td>
                                <td style="font-size:12px; width:76px; border-right:solid; border-right-width:1px; border-color:silver;"><asp:Label ID="lblIncobIrrecup"   runat="server" ForeColor="Silver" Text="Incob. Irrec." /></td>
                                <td style="font-size:12px; "><asp:Label ID="lblJuridicoLegal" runat="server" ForeColor="Silver" Text="Jurid.Legal" /></td>
                            </tr>
                            <tr>
                                <td style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgSAF"            CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" /></td>
                                <td style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgIHSS"           CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" /></td>
                                <td style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgRNP"            CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" /></td>
                                <td style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgCallCenter"     CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" /></td>
                                <td style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgMoraMayor"      CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></td>
                                <td style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgSobregiro"      CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></td>
                                <td style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgScoreMenor"     CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></td>
                                <td style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgCastigado"      CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></td>
                                <td style="border-right:solid; border-right-width:1px; border-color:silver;"><asp:Image ID="imgIncobrable"     CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></td>
                                <td><asp:Image ID="imgJuridico"   CssClass="ImgIcono24NOAB" runat="server"  Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" /></td>
                            </tr>
                            </table>

                        </asp:Panel>
                    </asp:Panel>

                    <!-- Resumen de credito -->

                    <asp:Panel ID="PanelAnalisis" runat="server" CssClass="FormatoPanelSubNOAB" Height="110px" Width="760px" Style="margin-left: 5px; margin-top: 5px;" Visible="false">
                        <div class="DivTituloPanel">
                            <table style="height: 100%;">
                                <tr>
                                    <td class="TableFormatoTitulo">Estado del cliente</td>
                                </tr>
                            </table>
                        </div>
                        <!-- Datos de Buro -->
                        <asp:Label ID="lblRespuesta" runat="server" CssClass="FormatolblScore" Style="margin-left: 5px;" ForeColor="#00CC66" Text="-- Pre-aprobado --" /><br />
                        <asp:Label ID="lblDetalleRespuesta" runat="server" CssClass="Formatolbl" Style="margin-left: 5px;" ForeColor="#00CC66" Font-Size="24px" Width="630" />

                        <asp:Image ID="imgAlegre" runat="server" Style="position: absolute; left: 640px; top: 30px; height: 64px; width: 64px;" ImageUrl="/Imagenes/EmoticonAlegre64.png" Visible="true" />
                        <asp:Image ID="imgTriste" runat="server" Style="position: absolute; left: 640px; top: 30px; height: 64px; width: 64px;" ImageUrl="/Imagenes/EmoticonTriste64.png" Visible="false" />
                        <asp:Image ID="imgAnalisis" runat="server" Style="position: absolute; left: 640px; top: 30px; height: 64px; width: 64px;" ImageUrl="/Imagenes/EmoticonAnalisis64.png" Visible="false" />
                    </asp:Panel>

                    <!-- Resolucion del area de credito -->
                    <asp:Panel ID="PanelResolucionCreditos" runat="server" CssClass="FormatoPanelSubNOAB" Height="80px" Width="760px" Style="margin-left: 5px; margin-top: 5px;" Visible="false">
                        <div class="DivTituloPanel">
                            <table style="height: 100%;">
                                <tr>
                                    <td class="TableFormatoTitulo">Resolución de creditos</td>
                                </tr>
                            </table>
                        </div>
                        <asp:Label ID="lblResolucionCreditos" runat="server" CssClass="Formatolbl" Style="margin: 10px;" />
                    </asp:Panel>


                    <!-- Resumen de credito -->
                    <asp:Panel ID="PanelOferta" runat="server" CssClass="FormatoPanelSubNOAB" Height="230px" Width="760px" Style="left: 5px; margin-top: 5px;" Visible="false">
                        <div class="DivTituloPanel">
                            <table style="height: 100%;">
                                <tr>
                                    <td class="TableFormatoTitulo">Oferta
                                    </td>
                                </tr>
                            </table>
                            <div class="DivTituloGrid">
                                <table class="TableFormatoTituloGrid">
                                    <tr>
                                        <td style="width: 200px;">Oferta</td>
                                        <td style="width: 130px;">Monto OfertadoOferta</td>
                                        <td style="width: 130px;">Plazo QuincenasMonto Ofertado</td>
                                        <td style="width: 130px;">Cuota QuincenalPlazo Quincenas</td>
                                        <td>Cuota Quincenal</td>
                                    </tr>
                                </table>
                            </div>
                            <asp:GridView ID="gvOferta" runat="server" CssClass="GridViewFormatoGeneral"
                                RowStyle-CssClass="GridViewFilas"
                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False" Width="640">
                                <Columns>
                                    <asp:BoundField DataField="fcProducto" ReadOnly="True" ItemStyle-Width="250px" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="fnMontoOfertado" DataFormatString="{0:#,###0.00}" ReadOnly="True" ItemStyle-Width="130px" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="fiPlazo" ReadOnly="True" ItemStyle-Width="130px" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="fnCuotaQuincenal" DataFormatString="{0:#,###0.00}" ReadOnly="True" ItemStyle-Width="130px" ItemStyle-Wrap="false" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>
                    <!-- Si el cliente es de CallCenter -->
                    <asp:Panel ID="PanelCallCenter" runat="server" CssClass="FormatoPanelSubNOAB" Height="110px" Width="760px" Style="left: 5px; margin-top: 5px;" Visible="false">
                        <div class="DivTituloPanel">
                            <table style="height: 100%;">
                                <tr>
                                    <td class="TableFormatoTitulo">Informacion del cliente asignado en CallCenter</td>
                                </tr>
                            </table>
                        </div>
                        <!-- Datos de Buro -->
                        <asp:Label ID="lblMensajeCallCenter" runat="server" CssClass="Formatolbl" ForeColor="#00CC66" Font-Size="24px" Style="margin-left: 10px; margin-top: 10px;">Detalle</asp:Label>
                        <asp:Label ID="lblMensajeCallCenterAgenteAsignado" runat="server" CssClass="Formatolbl" ForeColor="#00CC66" Font-Size="24px" Style="margin-left: 10px; margin-top: 45px;">Detalle</asp:Label>
                    </asp:Panel>

                    <!-- Grid de historial de consultas hechas al cliente-->
                    <asp:Panel ID="PanelHistorialdeConsultas" runat="server" CssClass="FormatoPanelSubNOAB" Height="165px" Width="760px" Style="left: 5px; margin-top: 5px;" Visible="false">
                        <div class="DivTituloPanel">
                            <table style="height: 100%;">
                                <tr>
                                    <td class="TableFormatoTitulo">Historial de consultas
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="DivTituloGrid">
                            <table class="TableFormatoTituloGrid">
                                <tr>
                                    <td style="width: 200px;">AgenciaOficial de Negocio</td>
                                    <td style="width: 200px;">Fecha de ConsultaAgencia</td>
                                    <td>Oficial de NegocioFecha de Consulta</td>
                                </tr>
                            </table>
                        </div>
                        <div class="DivPanelGridScrollAuto" style="height: 117px;">
                            <asp:GridView ID="gvBitacoraConsultas" runat="server" CssClass="GridViewFormatoGeneral"
                                RowStyle-CssClass="GridViewFilas"
                                AutoGenerateColumns="False" CellPadding="4" ForeColor="#333333" GridLines="None" ShowHeader="False">
                                <Columns>
                                    <asp:BoundField DataField="fcAgencia" ReadOnly="True" ItemStyle-Width="200px" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="fdFechaConsulta" ReadOnly="True" ItemStyle-Width="200px" ItemStyle-Wrap="false" />
                                    <asp:BoundField DataField="fcOficial" ReadOnly="True" ItemStyle-Width="200px" ItemStyle-Wrap="false" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </asp:Panel>

                    <!--Componentes de botones y etiqueta de mensajes de error.-->
                    <asp:Panel ID="Panel1" runat="server" CssClass="FormatoPanelSubNOAB" Height="24px" Width="760px" Style="left: 5px; margin-top: 5px; background-color: lightgray">
                        <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" Style="margin-left: 5px; margin-top: 5px; color: hotpink; font-weight: bold;">Resultado sujeto a que la informacion ingresada sea real.</asp:Label>
                    </asp:Panel>
                    <div class="DivPanelMenuEspaciadorTransparente"></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="prgLoadingStatus" runat="server" AssociatedUpdatePanelID="upClientes">
            <ProgressTemplate>
                <asp:Panel runat="server" Visible="true" BackColor="Transparent" BackImageUrl="/Imagenes/fondoTransparencia.png" Style="left: 0px; top: 0px; position: absolute; height: 100%; width: 100%;">
                    <div style="align-content: center; vertical-align: central; text-align: center; height: 100%; width: 100%;">
                        <table style="width: 100%; height: 100%;">
                            <tr>
                                <td>
                                    <asp:Image ID="imgCargando" runat="server" ImageUrl="/Imagenes/tenor.gif" Style="background-color: transparent; align-content: center; vertical-align: central; margin-top: auto;" />
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

