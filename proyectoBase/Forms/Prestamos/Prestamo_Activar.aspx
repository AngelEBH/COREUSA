<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prestamo_Activar.aspx.cs" Inherits="Prestamos_Prestamo_Activar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crear prestamo</title>
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css" />
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/CSS/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/sweet-alert2/sweetalert2.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" /></head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMenu">
            <ContentTemplate>
                <div class="DivPanelLista" style="width:600px;">
                    <div class="DivTituloPanel">
                        <table style="height: 100%;">
                            <tr>
                                <td class="TableFormatoTitulo">Crear prestamo</td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel CssClass="FormatoPanelTodoBlanco" ID="Panel1" runat="server" Style="height: 34px; width:600px;">
                        <asp:TextBox CssClass="FormatotxtMoneda" ID="txtBusqueda" runat="server" Style="left: 65px; top: 5px; width: 115px;"></asp:TextBox>
                        <asp:Label CssClass="Formatolbl" ID="lblBuscar" runat="server" Style="left: 5px; top: 9px;">Solicitud:</asp:Label>
                        <asp:ImageButton ID="btnBuscar" runat="server" Style="position: absolute; top: 5px; left: 197px;" ImageUrl="/Imagenes/IconoBuscarCliente24.png" OnClick="btnBuscar_Click" />
                    </asp:Panel>

                    <asp:Panel CssClass="FormatoPanelTodoBlanco" ID="Panel2" runat="server" Style="height: 100px; width:600px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h6 class="modal-title mt-0" id="modalActivarPrestamoLabel">Informacion del desembolso</h6>
                        </div>
                        <div class="modal-body pb-0">
                            <asp:GridView ID="gvDesembolsoDetalle" runat="server" CssClass="GridViewFormatoGeneralLista" HeaderStyle-CssClass="GridViewCabecera" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False" ShowHeader="True" Style="word-break: break-all;" RowStyle-Height="24px">
                                <Columns>
                                    <asp:BoundField DataField="fcCuentaContable"    HeaderText="Cuenta contable" HeaderStyle-HorizontalAlign="Left"   ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="fcDescripcionCuenta" HeaderText="Descripcion"     HeaderStyle-HorizontalAlign="Left"   ItemStyle-Width="250px" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="fnDebe"              HeaderText="Debe"            HeaderStyle-HorizontalAlign="Right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                    <asp:BoundField DataField="fnHaber"             HeaderText="Haber"           HeaderStyle-HorizontalAlign="Right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                    <asp:BoundField DataField="fcCentrodeCosto"     HeaderText="C.C."            HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="60px"  ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="modal-body">
                            <div class="form-group row">
                                <div class="col-12">
                                    <label class="col-form-label">Comentarios:</label>
                                    <asp:TextBox ID="txtComentariosDesembolso" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Identidad:</label>
                                    <asp:TextBox ID="txtIdentidadBeneficiario" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">RTN:</label>
                                    <asp:TextBox ID="txtRTNBeneficiario" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="col-form-label">Nombre beneficiario:</label>
                                    <asp:TextBox ID="txtBeneficiario" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Forma de pago:</label>
                                    <asp:TextBox ID="txtFormadePago" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Banco a depositar:</label>
                                    <asp:TextBox ID="txtBancoaDepositarBeneficiario" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Cuenta de banco:</label>
                                    <asp:TextBox ID="txtCuentaBancariaBeneficiario" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Tipo de cuenta:</label>
                                    <asp:TextBox ID="txtTipodeCuentaBancariaBeneficiario" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnGenerarPrestamo" runat="server" Text="Generar prestamo" CssClass="btn btn-secondary waves-effect" OnClick="btnGenerarPrestamo_Click" />
                            <asp:Button ID="btnCancelar"  runat="server" Text="Cancelar"  CssClass="btn btn-secondary waves-effect" OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                    </asp:Panel>
                </div>

                <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" Style="left: 5px; top: 75px; color: red;"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
