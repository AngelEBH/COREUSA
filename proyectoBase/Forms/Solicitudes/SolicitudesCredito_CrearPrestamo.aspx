<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_CrearPrestamo.aspx.cs" Inherits="Solicitudes_Forms_Solicitudes_SolicitudesCredito_CrearPrestamo" %>

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
                    <asp:Panel CssClass="FormatoPanelTodoBlanco" ID="Panel2" runat="server" Style="height: 100px; width:800px;">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h6 class="modal-title mt-0" id="modalActivarPrestamoLabel">Crear prestamo de solicitud: <asp:Label runat="server" ID="lblSolicitud" CssClass="col-form-label" Text="00" style="font-weight:bold;"></asp:Label></h6>
                        </div>
                        <div class="modal-body">
                            <div class="form-group row">
                                <div class="col-3">
                                    <label class="col-form-label">Identificacion:</label>
                                    <asp:TextBox ID="txtIdentificacion" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="col-form-label">Nombre del cliente:</label>
                                    <asp:TextBox ID="txtNombreCliente" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-2">
                                    <label class="col-form-label">ID Producto:</label>
                                    <asp:TextBox ID="txtIDProducto" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="col-form-label">Producto:</label>
                                    <asp:TextBox ID="txtProducto" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Frecuencia de pago:</label>
                                    <asp:DropDownList ID="ddlFrecuenciadePago" CssClass="form-control form-control-sm" runat="server">
                                        <asp:ListItem Selected="True" Value="1">Catorcenal</asp:ListItem>
                                        <asp:ListItem Value="3">Mensual</asp:ListItem>
                                        <asp:ListItem Value="10">Semanal</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-2">
                                    <label class="col-form-label">Plazo:</label>
                                    <asp:TextBox ID="txtPlazo" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false" Style="text-align: right;"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Capital a financiar:</label>
                                    <asp:TextBox ID="txtCapital" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false" Style="text-align: right;"></asp:TextBox>
                                </div>
                                <div class="col-2">
                                    <label class="col-form-label">Cuota:</label>
                                    <asp:TextBox ID="txtCuota" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false" Style="text-align: right;"></asp:TextBox>
                                </div>
                                <div class="col-2">
                                    <label class="col-form-label">Interes:</label>
                                    <asp:TextBox ID="txtInteres" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false" Style="text-align: right;"></asp:TextBox>
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

                <asp:Panel runat="server" ID="PanelPrestamoGenerado" CssClass="modal-dialog modal-lg" Style="position: absolute !important; left: 300px; top: 100px;" Visible="false">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h6 class="modal-title mt-0" id="modalDescuentoPrestamoLabel">Prestamo creado</h6>
                            <asp:Button ID="btnCerrar" runat="server" Text="x" CssClass="close" OnClick="btnCerrarVentana_Click" style="border:hidden; background-color:transparent;" />
                        </div>
                        <div class="modal-body">
                            <div class="form-group row">
                                <div class="col-12">
                                    <asp:label runat="server" ID="lblMensajePrestamo" class="col-form-label"></asp:label>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnCerrarVentana"    runat="server" Text="Cerrar"    CssClass="btn btn-secondary waves-effect" OnClick="btnCerrarVentana_Click" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" Style="left: 5px; top: 75px; color: red;"></asp:Label>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>