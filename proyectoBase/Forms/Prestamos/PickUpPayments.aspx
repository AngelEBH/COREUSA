<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PickUpPayments.aspx.cs" Inherits="Prestamos_PickUpPayments" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Mis solicitudes</title>
    <!-- BOOTSTRAP -->
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .dataTable tbody tr {
            cursor: pointer;
        }

        .dataTable tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }
    </style>
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css" />
</head>
<body runat="server" class="EstiloBody-Listado">
    <form runat="server">
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <asp:UpdatePanel runat="server" ID="upMenu">
                <ContentTemplate>
    <div class="card DivPanelListas" style="width:auto; max-width:1000px; ">
        <div class="card-header">
            <div class="row">
                <div class="col-8">
                    <h6>Pick Up Payments</h6>
                </div>
                <div class="col-4">
                    <!--<input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar"
                        aria-label="Buscar" />-->
                    <asp:Button ID="btnAgregarPickUpPayment" runat="server" Text="Agregar Pick Up Payment" class="btn btn-secondary waves-effect waves-light" OnClick="btnAgregarPickUpPayment_Click" />
                </div>
            </div>
        </div>

        <div class="card-body">
            <div class="DivPanelListas" style="width:auto; height: calc(100vh - 100px); ">
                <div class="DivPanelGridScrollFull" style="width:100%; height: calc(100vh - 102px);">
                    <asp:GridView ID="gvPickUpPayments" runat="server" CssClass="GridViewFormatoGeneral" HeaderStyle-CssClass="GridViewCabecera" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False"  ShowHeader="True" style="word-break:break-all;" RowStyle-Height="24px" HeaderStyle-Height="24px">
                        <Columns>
                            <asp:BoundField DataField="fiIDTransaccion"    ItemStyle-Width="50px"  ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="ID"              DataFormatString="{0:#,###0}"/>
                            <asp:BoundField DataField="fcNombreCliente"    ItemStyle-Width="300px" ItemStyle-HorizontalAlign="Left"   HeaderStyle-HorizontalAlign="Left"   HeaderText="Nombre del cliente"/>
                            <asp:BoundField DataField="fdInicio"           ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Fec.Creado"      DataFormatString="{0:dd/MM/yyyy hh:mm:ss tt}" />
                            <asp:BoundField DataField="fdFechaVencimiento" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Fec.Vence"       DataFormatString="{0:dd/MM/yyyy}" />
                            <asp:BoundField DataField="fnValorDownPayment" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Right"  HeaderText="DownPayment."    DataFormatString="{0:#,###0.00}"/>
                            <asp:BoundField DataField="fnValorPagado"      ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Right"  HeaderText="Valor pagado"    DataFormatString="{0:#,###0.00}"/>
                            <asp:BoundField DataField="fnSaldoActual"      ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  HeaderStyle-HorizontalAlign="Right"  HeaderText="Saldo pendiente" DataFormatString="{0:#,###0.00}"/>
                            <asp:BoundField DataField="fcMensaje"          ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Estado"/>
                        </Columns>
                        <HeaderStyle CssClass="GridViewCabecera" />
                        <RowStyle CssClass="GridViewFilas" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>


    <asp:Panel runat="server" ID="PanelAgregarPickUpPayment" CssClass="modal-dialog modal-lg" Style="position: absolute !important; left: 100px; top: 100px;" Visible="false">
        <div class="modal-content">
            <div class="modal-header">
                <h6 class="modal-title mt-0" id="modalActivarPrestamoLabel">Registrar Pick Up Payment</h6>
                <asp:Button ID="btnCerrarX" runat="server" Text="x" CssClass="close" OnClick="btnCerrar_Click" style="border:hidden; background-color:transparent;" />
            </div>
            <div class="modal-body">
                <div class="form-group row">
                    <div class="col-3">
                        <label class="col-form-label">Identificación:</label>
                        <asp:TextBox ID="txtIdentificacion" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="true"></asp:TextBox>
                        <asp:Button ID="btnBuscarCliente" runat="server" Text="Buscar" CssClass="btn btn-secondary waves-effect" OnClick="btnBuscarCliente_Click" />
                    </div>
                    <div class="col-6">
                        <label class="col-form-label">Nombre:</label>
                        <asp:TextBox ID="txtNombreCliente" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                    </div>
                    <div class="col-3">
                        <label class="col-form-label">Fecha de vencimiento:</label>
                        <asp:TextBox ID="txtFechaVencimiento" CssClass="form-control form-control-sm" type="date" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-3">
                        <label class="col-form-label">Monto DownPayment</label>
                        <asp:TextBox ID="txtMontoDownPayment" CssClass="form-control form-control-sm" type="number" runat="server" Enabled="true" step="0.01" ></asp:TextBox>
                    </div>
                    <div class="col-9">
                        <label class="col-form-label">Comentarios:</label>
                        <asp:TextBox ID="txtComentarios" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                    </div>

                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnRegistrar" runat="server" Text="Activar" CssClass="btn btn-secondary waves-effect" OnClick="btnRegistrar_Click" Enabled="false" />
                <asp:Button ID="btnCerrar"    runat="server" Text="Cerrar"  CssClass="btn btn-secondary waves-effect" OnClick="btnCerrar_Click" />
            </div>
        </div>
    </asp:Panel>
    <asp:Panel ID="PanelErrores" runat="server" CssClass="FormatoPanelAlertaRojo" Height="50px" Width="350px" Visible="false" >
        <asp:Label CssClass="Formatolbl" ID="lblMensaje" runat="server" style="color:white; margin:5px;"></asp:Label>
    </asp:Panel>
    </ContentTemplate>
    </asp:UpdatePanel>


    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <!-- datatable js -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <!-- Buttons -->
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="/Scripts/plugins/datatables/pdfmake.min.js"></script>
    <script src="/Scripts/plugins/datatables/vfs_fonts.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.print.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.colVis.min.js"></script>
    <!-- Responsive -->
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/datatables/responsive.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/app/Prestamos/Prestamos_Lista.js?V=20201103144650"></script>

    </form>
</body>
</html>
