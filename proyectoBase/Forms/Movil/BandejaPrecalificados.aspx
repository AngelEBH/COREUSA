<%@ Page Language="C#" AutoEventWireup="true" Inherits="Clientes_BandejaPrecalificados" Codebehind="BandejaPrecalificados.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css"/>
    <title></title>
    <script type="text/javascript" src='/Scripts/js/jquery.min.js' > </script>  
    <script type="text/javascript">
        $.expr[":"].containsNoCase = function (el, i, m) { var search = m[3];  if (!search) return false;  return eval("/" + search + "/i").test($(el).text()); };

        $(document).ready(
            function () {
                $('#txtBuscador').keyup(function () {
                    if ($('#txtBuscador').val().length > 1) {
                        $('#gvPrecalificado tr').hide();
                        $('#gvPrecalificado tr:first').show();
                        $('#gvPrecalificado tr td:containsNoCase(\'' + $('#txtBuscador').val() + '\')').parent().show();
                        document.getElementById('txtRegistros').value = $('tr').filter(function () { return $(this).css('display') !== 'none'; }).length + ' registros';
                        document.getElementById('gvPrecalificado').style.visibility = "visible";
                    }
                    else if ($('#txtBuscador').val().length == 0) { resetSearchValue(); }

                    if ($('#gvPrecalificado tr:visible').length == 1) {
                        $('.norecords').remove();
                        document.getElementById('gvPrecalificado').style.visibility = "hidden";
                        document.getElementById('txtRegistros').value = 'Sin registros';
                    }
                }
                );
                $('#txtBuscador').keyup(function (event) { if (event.keyCode == 27) { resetSearchValue(); } });
            });  
  
        function resetSearchValue() {
            $('#txtBuscador').val('');
            $('#gvPrecalificado tr').show();
            $('.norecords').remove();
            $('#txtBuscador').focus();
            document.getElementById('gvPrecalificado').style.visibility = "visible";
            document.getElementById('txtRegistros').value = '';
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upLista">
            <ContentTemplate>

                <div class="DivPanelListas" style="width:1112px; height: calc(100vh - 5px);">
                    <div class="DivTituloPanelXGrande">
                        <table style="height:100%;"><tr><td class="TableFormatoTituloXGrande" style="width:20px;"><asp:Image ID="imgIcono" runat="server" /></td><td class="TableFormatoTituloXGrande"><asp:Label runat="server" style="margin-left:5px;" ID="lblTituloVentana"/></td></tr></table>
                        QUe</div>
                    <div>
                        <asp:Panel ID="PanelOpciones" runat="server" CssClass="FormatoPanelViewNOAB" Height="32px" Style="margin-left: 5px; margin-top: 5px; background-color:gray;  width: fit-content;">
                            <asp:Button ID="btnTodos"      runat="server" CssClass="FormatoBotonesIconoFiltros40" Style="background-image: url('/Imagenes/iconoPersonas24.png'); border-bottom-left-radius: 2px; border-top-left-radius: 2px;" Text="Todos" OnClick="btnFiltros_Click" CommandName="0" CommandArgument="0" />
                            <asp:Button ID="btnAprobados"  runat="server" CssClass="FormatoBotonesIconoFiltros40" Style="background-image: url('/Imagenes/iconoOk24.png');"      Text="Pre-aprobados" OnClick="btnFiltros_Click" CommandName="1" CommandArgument="1" />
                            <asp:Button ID="btnAnalisis"   runat="server" CssClass="FormatoBotonesIconoFiltros40" Style="background-image: url('/Imagenes/iconoAlerta24.png');"  Text="En analisis"   OnClick="btnFiltros_Click" CommandName="3" CommandArgument="3" />
                            <asp:Button ID="btnRechazados" runat="server" CssClass="FormatoBotonesIconoFiltros40" Style="background-image: url('/Imagenes/iconoDetener24.png');  border-bottom-right-radius: 2px; border-top-right-radius: 2px;"  Text="Rechazados"    OnClick="btnFiltros_Click" CommandName="2" CommandArgument="2"  />
                        </asp:Panel>
                    </div>

                    <div class="FormatoPanelSubNOAB" style="width:1100px; margin:5px;">
                        <div class="DivTituloGrid">
                            <table class="TableFormatoTituloGrid">
                                <tr>
                                    <td style="width:10px; ">&nbsp</td>
                                    <td style="width:100px;">Oficial</td>
                                    <td style="width:230px;">Nombre/Identidad</td>
                                    <td style="width:200px;">Ingresos/Telefono</td>
                                    <td style="width:150px;">Producto/Consultado</td>
                                    <td>Detalle</td>
                                    <td style="width:70px; ">Acciones</td>
                                    <td style="width:20px; ">&nbsp</td>
                                </tr>
                            </table>
                        </div>
                        <div class="DivPanelGridScrollFull" style="width:100%; height: calc(100vh - 119px); ">
                            <asp:GridView ID="gvPrecalificado" runat="server" CssClass="GridViewFormatoGeneral" HeaderStyle-CssClass="GridViewCabecera" RowStyle-CssClass="GridViewFilasNeutral" RowStyle-Height="46px" AutoGenerateColumns="False"  ShowHeader="False" style="vertical-align:middle;" OnRowCommand="gvPrecalificado_RowCommand" DataKeyNames="fcIdentidad">
                                <Columns>
                                    <asp:ImageField DataImageUrlField="fcImagen"      ItemStyle-Width="24px" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle"/>
                                    <asp:BoundField DataField="fcIdentidad"           ItemStyle-Width="100px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna1"            ItemStyle-Width="100px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna2"            ItemStyle-Width="230px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna3"            ItemStyle-Width="150px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna4"            ItemStyle-Width="230px" HtmlEncode="False" />
                                    <asp:BoundField DataField="fcColumna5"            HtmlEncode="False" />
                                    <asp:ButtonField ButtonType="Image" ImageUrl="/Imagenes/iconoSolicitudesLista30.png" ItemStyle-Width="35px" CommandName="Ver"      Text="Ver detalle del precalificado" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle"/>
                                </Columns>
                                <HeaderStyle CssClass="GridViewCabecera" />
                                <RowStyle CssClass="GridViewFilasBitacoras" />
                            </asp:GridView>
                        </div>
                    </div>
                </div>
                <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" style="left:5px; color:red;"></asp:Label>    
                <asp:Panel ID="PanelErrores" runat="server" CssClass="FormatoPanelContenedorBordes" Height="50px" Width="200px" Visible="false" style="left:calc(100vw - 300px); position:absolute; top:10px; background-color:darkred; border-color:red;">
                    <asp:Label CssClass="Formatolbl" ID="lblMensajeError" runat="server" style="color:white; margin:5px;"></asp:Label>
                </asp:Panel>

                <input type="text" id="txtBuscador"  name="txtBuscador"  class="Formatotxt" maxlength="50" style="top: 51px; width: 150px; left:780px;" /> 
                <input type="text" id="txtRegistros" name="txtRegistros" class="Formatotxt" maxlength="20" style="top: 52px; width: 120px; left:945px; border:hidden; background-color:transparent;" disabled="disabled" /> 
                
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="prgLoadingStatus" runat="server" AssociatedUpdatePanelID="upLista">
            <ProgressTemplate>
                <asp:Panel ID="PanelCargando" runat="server" CssClass="FormatoPanelView" Height="40px" Width="64px" Style="left: 1030px; top: 42px; background-color:transparent;">
                    <asp:Image ID="imgCargando" runat="server" ImageUrl="/Imagenes/gifCargandoHorizontal30x60.gif" Style="position:absolute; left:0px; top:0px;"/>
                </asp:Panel>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
</body>
</html>

