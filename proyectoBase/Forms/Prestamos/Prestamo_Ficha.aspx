<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Prestamo_Ficha.aspx.cs" Inherits="Prestamos_Prestamo_Ficha" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Ingresar solicitud de crédito</title>
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
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" >
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMenu" style="background-color:#d9d9d9;">
            <ContentTemplate>
                <div class="EstiloBody-Listado" style="max-width: 1200px;">
                    <div class="card-header">
                        <div class="row justify-content-between align-items-end">
                            <div class="col-lg-auto col-md-auto col-sm-auto col-auto">
                                <div class="form-inline p-0">
                                    <div class="spinner-border" role="status" id="divCargandoAnalisis" style="display: none;">
                                        <span class="sr-only">Cargando</span>
                                    </div>
                                    <asp:Image runat="server" ID="imgLogo" class="align-self-center d-none d-sm-block d-sm-none d-md-block d-md-none d-lg-block" alt="Logo del Producto" Style="display: none; padding-right:8px;" />
                                    <asp:Label runat="server" ID="lblProducto" CssClass="h6 font-weight-bold align-self-end"></asp:Label>
                                    <asp:Label runat="server" ID="lblMensaje" CssClass="h6 font-weight-bold align-self-end"></asp:Label>
                                </div>
                            </div>
                            <div class="col-lg-auto col-md-auto col-sm-auto col-auto">
                                <div class="form-inline">
                                    <div class="button-items pb-2">
                                        <asp:Button ID="btnActivarPrestamo" runat="server" Text="Activar Prestamo" class="btn btn-secondary waves-effect waves-light" OnClick="btnActivarPrestamo_Click" />
                                        <asp:Button ID="btnSolicitudPrestamo" runat="server" Text="Ver Solicitud" class="btn btn-secondary waves-effect waves-light" OnClick="btnSolicitudPrestamo_Click" />
                                        <!-- <button id="btnDocumentacionModal" type="button" data-toggle="modal" data-target="#modalDocumentacion" class="btn btn-secondary waves-effect waves-light">
                                            <i class="far fa-file-alt"></i>
                                            Documentos</button> -->
                                        <button id="btnMasDetalles" type="button" class="btn btn-secondary waves-effect waves-light">
                                            <i class="fas fa-info-circle"></i>
                                            Más detalles</button>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div>
                        <div class="panel-body">
                            <div class="row mb-0" id="divInformacionPersonal" runat="server">
                                <div class="col-lg-5 col-md-5 border-left border-gray">
                                    <div class="col-md-12">
                                        <h6 class="font-weight-bold">Información del cliente</h6>
                                        <div class="form-group row">
                                            <div class="col-4">
                                                <label class="col-form-label">Identificación</label>
                                                <asp:TextBox ID="txtIdentidadCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" ></asp:TextBox>
                                            </div>
                                            <div class="col-8">
                                                <label class="col-form-label">Nombre</label>
                                                <asp:TextBox ID="txtNombreCliente" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Producto</label>
                                                <asp:TextBox ID="txtProducto" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-4">
                                                <label class="col-form-label">Solicitud</label>
                                                <asp:TextBox ID="txtIDSolicitud" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">VIN:</label>
                                                <asp:TextBox ID="txtVINVehiculo" CssClass="form-control form-control-sm" type="text" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                            <div class="col-8">
                                                <label class="col-form-label">Propietario:</label>
                                                <asp:TextBox ID="txtPropietarioGarantia" CssClass="form-control form-control-sm" type="text" runat="server" ReadOnly="true"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-lg-6 col-md-6 border-left border-gray">
                                    <div class="col-md-14">
                                        <h6 class="font-weight-bold">Información del financiamiento</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <div class="form-group row">
                                                    <div class="col-4">
                                                        <label class="col-form-label">No. Prestamo</label>
                                                        <asp:TextBox ID="txtIDPrestamo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Estado</label>
                                                        <asp:TextBox ID="txtEstadodelPrestamo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Tasa de interes</label>
                                                        <asp:TextBox ID="txtTasadeInteres" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" Style="text-align: right;"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Fecha de creación</label>
                                                        <asp:TextBox ID="txtFechaCreacion" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Fecha de contrato</label>
                                                        <asp:TextBox ID="txtFechaColocacion" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Inicio de prestamo</label>
                                                        <asp:TextBox ID="txtFechaInicioPrestamo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Fin de prestamo</label>
                                                        <asp:TextBox ID="txtFechaFinPrestamo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Capital financiado</label>
                                                        <asp:TextBox ID="txtMontoFinanciado" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" Style="text-align: right;"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Saldo actual</label>
                                                        <asp:TextBox ID="txtSaldoActual" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" Style="text-align: right;"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Valor cuota</label>
                                                        <asp:TextBox ID="txtValorCuota" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" Style="text-align: right;"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Frecuencia</label>
                                                        <asp:TextBox ID="txtFrecuencia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" Style="text-align: right;"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Plazo</label>
                                                        <asp:TextBox ID="txtPlazo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" Style="text-align: right;"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Cuotas pagadas</label>
                                                        <asp:TextBox ID="txtCuotasPagadas" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" Style="text-align: right;"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Cuotas atrasadas</label>
                                                        <asp:TextBox ID="txtCuotasAtrasadas" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" Style="text-align: right;"></asp:TextBox>
                                                    </div>
                                                    <div class="col-4">
                                                        <label class="col-form-label">Dias de atraso</label>
                                                        <asp:TextBox ID="txtDiasAtraso" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server" Style="text-align: right;"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        <div class="col-lg-auto col-md-auto col-sm-auto col-auto">
                            <div class="form-inline">
                                <div class="button-items pb-2">
                                    <button id="btnPlandePago"       type="button" class="btn btn-secondary waves-effect waves-light" onclick="TabControl1()"><i class="far fa-file-alt"></i>
                                        Plan de Pago</button>
                                    <button id="btnPlandePagoAvance" type="button" class="btn btn-secondary waves-effect waves-light" onclick="TabControl2()"><i class="far fa-file-alt"></i>
                                        Plan de Pago - Avance</button>
                                    <button id="btnEstaodeCuenta"    type="button" class="btn btn-secondary waves-effect waves-light" onclick="TabControl3()"><i class="far fa-file-alt"></i>
                                        Estado de cuenta</button>
                                    <button id="btnDesembolso"       type="button" class="btn btn-secondary waves-effect waves-light" onclick="TabControl4()"><i class="far fa-file-alt"></i>
                                        Desembolso</button>
                                </div>
                            </div>
                        </div>

                            <div class="mb-0" id="TabPlandePago" runat="server" visible="true">
                                <h6 class="font-weight-bold" style="margin-left:20px;">Plan de Pago</h6>
                                <div id="divPlandePago" class="DivPanelExploracion" runat="server" style="margin-left:5px; margin-right:5px;">
                                    <asp:GridView ID="gvPlandePago" runat="server" CssClass="GridViewFormatoGeneralLista" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="GridViewCabecera32" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False" ShowHeader="True" Style="word-break: break-all;" RowStyle-Height="24px">
                                        <Columns>
                                            <asp:BoundField DataField="fiCuota"               HeaderText="No.Cuota"      HeaderStyle-CssClass="text-center" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="fdFechadeCuota"        HeaderText="Fecha"         HeaderStyle-CssClass="text-center" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="fnCapitalAnterior"     HeaderText="Cap.Inicial"   HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnCapitalPactado"      HeaderText="Cap.Pact."     HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnInteresPactado"      HeaderText="Int.Pact."     HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnSeguro1"             HeaderText="Cuota Seg.1"   HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnSeguro2"             HeaderText="Cuota Seg.2"   HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnServicio1"           HeaderText="Cuota GPS"     HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnTotalCuota"          HeaderText="Total Cuota"   HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnCapitalBalanceFinal" HeaderText="Balance Final" HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="mb-0" id="TabPlandePagoAvance" runat="server" style="display:none;">
                                <h6 class="font-weight-bold" style="margin-left:20px;">Plan de Pago - Avance</h6>
                                <div id="div2" class="DivPanelExploracion" runat="server" style="margin-left:5px; margin-right:5px;">
                                    <asp:GridView ID="gvPlandePagoAvance" runat="server" CssClass="GridViewFormatoGeneralLista" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="GridViewCabecera32" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False" ShowHeader="True" Style="word-break: break-all;" RowStyle-Height="24px">
                                        <Columns>
                                            <asp:BoundField DataField="fiCuota"          HeaderText="No.Cuota"       HeaderStyle-CssClass="text-center" ItemStyle-Width="50px"  ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="fdFechadeCuota"   HeaderText="Fecha"          HeaderStyle-CssClass="text-center" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy}" />
                                            <asp:BoundField DataField="fnCapitalPactado" HeaderText="Cap.Pact."      HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnInteresPactado" HeaderText="Int.Pact."      HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnSaldoCapital"   HeaderText="Saldo Capital"  HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnSaldoInteres"   HeaderText="Saldo Interes"  HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnSaldoSeguro1"   HeaderText="Saldo Seguro 1" HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnSaldoSeguro2"   HeaderText="Saldo Seguro 2" HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnSaldoServicio1" HeaderText="Saldo GPS"      HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnTotalCuota"     HeaderText="Total Cuota"    HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>

                            <div class="mb-0" id="TabEstadodeCuenta" runat="server" style="display:none;">
                                <h6 class="font-weight-bold" style="margin-left:20px;">Estado de Cuenta</h6>
                                <div id="div3" class="DivPanelExploracion" runat="server" style="margin-left:5px; margin-right:5px;">
                                    <asp:GridView ID="gvEstadodeCuenta" runat="server" CssClass="GridViewFormatoGeneralLista" HeaderStyle-VerticalAlign="Middle" HeaderStyle-CssClass="GridViewCabecera32" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False" ShowHeader="True" Style="word-break: break-all;" RowStyle-Height="24px">
                                        <Columns>
                                            <asp:BoundField DataField="fiIDTransaccion"    HeaderText="ID Tran."     HeaderStyle-CssClass="text-center" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="fdFechaTransaccion" HeaderText="Fecha"        HeaderStyle-CssClass="text-center" ItemStyle-Width="200px" ItemStyle-HorizontalAlign="Center" DataFormatString="{0:dd/MM/yyyy hh:mm:ss}" />
                                            <asp:BoundField DataField="fcTipoOperacion"    HeaderText="ID Tran."     HeaderStyle-CssClass="text-center" ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="fnCapital"          HeaderText="Capital"      HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnInteres"          HeaderText="Interes"      HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnInteresMoratorio" HeaderText="Int.Mora"     HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnSeguro1"          HeaderText="Seg.1"        HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnSeguro2"          HeaderText="Seg.2"        HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnServicio1"        HeaderText="GPS"          HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnRecargos"         HeaderText="Recargos"     HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnOtrosCagos"       HeaderText="Otros Cargos" HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                            <asp:BoundField DataField="fnTotalTransaccion" HeaderText="Total"        HeaderStyle-CssClass="text-right"  ItemStyle-Width="100px" ItemStyle-HorizontalAlign="Right"  DataFormatString="{0:#,###0.00}"/>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="mb-0" id="TabDesembolso" runat="server" style="display:none;">
                                <h6 class="font-weight-bold" style="margin-left:20px;">Partida de desembolso</h6>
                                <div id="div4" class="DivPanelExploracion" runat="server" style="margin-left:5px; margin-right:5px;">
                                    <div class="modal-body pb-0">
                                        <asp:GridView ID="gvPartidaDesembolso" runat="server" CssClass="GridViewFormatoGeneralLista" HeaderStyle-CssClass="GridViewCabecera" RowStyle-CssClass="GridViewFilas" AutoGenerateColumns="False" ShowHeader="True" Style="word-break: break-all;" RowStyle-Height="24px">
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
                                        <div class="col-12">
                                            <label class="col-form-label">Comentarios</label>
                                            <asp:TextBox ID="txtComentarioDesembolsado" CssClass="form-control form-control-sm" type="text" runat="server" ReadOnly="true"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Solicitar datos del origen del carro -->
                <asp:Panel runat="server" ID="PanelDescuento" CssClass="modal-dialog modal-lg" Style="position: absolute !important; left: 300px; top: 100px;" Visible="false">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h6 class="modal-title mt-0" id="modalDescuentoPrestamoLabel">Origen del vehiculo</h6>
                            <asp:Button ID="Button1" runat="server" Text="x" CssClass="close" OnClick="btnContinuarDescuento_Click" style="border:hidden; background-color:transparent;" />
                        </div>
                        <div class="modal-body">
                            <div class="form-group row">
                                <div class="col-8">
                                    <label class="col-form-label">Origen del vehiculo</label>
                                    <asp:DropDownList ID="ddlOrigendelVehiculo" CssClass="form-control form-control-sm" runat="server">
                                        <asp:ListItem Selected="True" Value="1">Inventario Dealer</asp:ListItem>
                                        <asp:ListItem Value="2">Inventario Propio(CrediFlash)</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-4">
                                    <label class="col-form-label">Valor del vehiculo:</label>
                                    <asp:TextBox ID="txtValordelVehiculo" CssClass="form-control form-control-sm" type="number" runat="server" step="0.01"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnContinuarDescuento" runat="server" Text="Continuar" CssClass="btn btn-secondary waves-effect" OnClick="btnContinuarDescuento_Click" />
                            <asp:Button ID="btnCerrarDescuento"    runat="server" Text="Cerrar"    CssClass="btn btn-secondary waves-effect" OnClick="btnCerrarDescuento_Click" />
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="PanelActivarPrestamo" CssClass="modal-dialog modal-lg" Style="position: absolute !important; left: 100px; top: 100px;" Visible="false">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h6 class="modal-title mt-0" id="modalActivarPrestamoLabel">Informacion del desembolso</h6>
                            <asp:Button ID="btnCerrarX" runat="server" Text="x" CssClass="close" OnClick="btnCerrar_Click" style="border:hidden; background-color:transparent;" />
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
                                <div class="col-3">
                                    <label class="col-form-label">Fecha contrato:</label>
                                    <asp:TextBox ID="txtFechadelContrato" CssClass="form-control form-control-sm" type="date" runat="server" ></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Frecuencia de pago:</label>
                                    <!--<asp:DropDownList ID="ddlFrecuencia" CssClass="form-control form-control-sm" type="text" runat="server"></asp:DropDownList>-->
                                    <asp:TextBox ID="txtFrecuenciadePago" CssClass="form-control form-control-sm" Enabled="false" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Primer pago:</label>
                                    <asp:TextBox ID="txtFechaPrimerPago" CssClass="form-control form-control-sm" type="date" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-12">
                                    <label class="col-form-label">Comentarios:</label>
                                    <asp:TextBox ID="txtComentariosDesembolso" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                </div>
                                <asp:Panel runat="server" ID="PanelDealer" Visible="false" CssClass="form-group row" style="margin-left: 0px; margin-right:0px;">
                                <div class="col-3">
                                    <label class="col-form-label">Identificación:</label>
                                    <asp:TextBox ID="txtIdentidadBeneficiario" CssClass="form-control form-control-sm" type="text" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Documento secundario:</label>
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
                                </asp:Panel>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <asp:Button ID="btnActivar" runat="server" Text="Activar" CssClass="btn btn-secondary waves-effect" OnClick="btnActivar_Click" />
                            <asp:Button ID="btnCerrar"  runat="server" Text="Cerrar"  CssClass="btn btn-secondary waves-effect" OnClick="btnCerrar_Click" />
                        </div>
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>

        <script>
            function TabControl1() {
                var x = document.getElementById("TabPlandePago");
                x.style.display = "block";
                x = document.getElementById("TabPlandePagoAvance");
                x.style.display = "none";
                x = document.getElementById("TabEstadodeCuenta");
                x.style.display = "none";
                x = document.getElementById("TabDesembolso");
                x.style.display = "none";
            }
            function TabControl2() {
                var x = document.getElementById("TabPlandePago");
                x.style.display = "none";
                x = document.getElementById("TabPlandePagoAvance");
                x.style.display = "block";
                x = document.getElementById("TabEstadodeCuenta");
                x.style.display = "none";
                x = document.getElementById("TabDesembolso");
                x.style.display = "none";
            }
            function TabControl3() {
                var x = document.getElementById("TabPlandePago");
                x.style.display = "none";
                x = document.getElementById("TabPlandePagoAvance");
                x.style.display = "none";
                x = document.getElementById("TabEstadodeCuenta");
                x.style.display = "block";
                x = document.getElementById("TabDesembolso");
                x.style.display = "none";
            }
            function TabControl4() {
                var x = document.getElementById("TabPlandePago");
                x.style.display = "none";
                x = document.getElementById("TabPlandePagoAvance");
                x.style.display = "none";
                x = document.getElementById("TabEstadodeCuenta");
                x.style.display = "none";
                x = document.getElementById("TabDesembolso");
                x.style.display = "block";

            }
        </script>
        <script src="/Recursos/assets/js/form-plugins.demo.min.js"></script>
    </form>
</body>
</html>
