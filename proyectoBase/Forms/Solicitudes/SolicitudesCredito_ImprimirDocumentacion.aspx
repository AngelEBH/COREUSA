<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_ImprimirDocumentacion.aspx.cs" Inherits="SolicitudesCredito_ImprimirDocumentacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Imprimir documentacion</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <style>
        html, body {
            background-color: #fff;
        }

        .card {
            box-shadow: none;
        }
    </style>
</head>
<body class="EstiloBody">
    <form id="frmGuardarPreSolicitud" runat="server">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <div class="card m-0">

            <div class="card-header pb-1 pt-1">
                <!-- loader -->
                <div class="float-right p-1" id="Loader" style="display: none;">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>
                <h5>Imprimir documentación solicitud de crédito No. <span class="font-weight-bold">330</span></h5>
            </div>
            <div class="card-body pt-0">

                <div class="row mb-0">
                    <div class="col-md-6">
                        <div class="form-group row">
                            <div class="col-sm-12">
                                <label class="col-form-label">Cliente</label>
                                <asp:TextBox ID="txtNombreCliente" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="Willian Onandy Diaz Serrano" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Identidad</label>
                                <asp:TextBox ID="txtIdentidadCliente" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="0502-2000-02944" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">RTN numérico</label>
                                <asp:TextBox ID="txtRtn" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Teléfono</label>
                                <asp:TextBox ID="txtTelefonoCliente" type="tel" Enabled="false" CssClass="form-control form-control-sm col-form-label mascara-telefono" Text="9611-6376" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6 border-left border-gray">
                        <div class="form-group row">
                            <div class="col-sm-3">
                                <label class="col-form-label">Producto</label>
                                <asp:TextBox ID="txtProducto" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="Automovil Financ." runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Monto a Financiar</label>
                                <asp:TextBox ID="TextBox2" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="130,000" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Plazo</label>
                                <asp:TextBox ID="txtPlazoFinanciar" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="48 Meses" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">48 Cuotas Mensuales</label>
                                <asp:TextBox ID="txtValorCuota" type="tel" Enabled="false" CssClass="form-control form-control-sm col-form-label mascara-telefono" Text="5,600" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row mb-0">
                    <div class="col-12">
                        <h6 class="border-bottom pb-2">Información de la garantía</h6>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">VIN</label>
                                <asp:TextBox ID="txtVIN" placeholder="EJ. JH4TB2H26CC000000" CssClass="form-control form-control-sm mascara-vin" type="text" Text="4XAXH76A8AD092394" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Tipo de garantía</label>
                                <asp:TextBox ID="txtTipoDeGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" Text="AUTO" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Tipo de vehículo</label>
                                <asp:TextBox ID="txtTipoDeVehiculo" placeholder="EJ. Turismo" CssClass="form-control form-control-sm" type="text" Text="Sedan/Turismo" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-3">
                                <label class="col-form-label">Marca</label>
                                <asp:TextBox ID="txtMarca" placeholder="EJ. Honda" CssClass="form-control form-control-sm" type="text" ReadOnly="true" Text="Toyota" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Modelo</label>
                                <asp:TextBox ID="txtModelo" placeholder="EJ. Civic" CssClass="form-control form-control-sm" type="text" ReadOnly="true" Text="Corolla" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Año</label>
                                <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm mascara-enteros" type="text" ReadOnly="true" Text="2010" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Color</label>
                                <asp:TextBox ID="txtColor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" Text="Gris" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="col-form-label">Matrícula</label>
                                <asp:TextBox ID="txtMatricula" placeholder="EJ. AAA 9999" CssClass="form-control form-control-sm mascara-matricula" ReadOnly="true" type="text" Text="HAC 5798" required="required" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 border-left border-gray">
                        <div class="form-group row">
                            <div class="col-sm-4">
                                <label class="col-form-label">Cilindraje</label>
                                <asp:TextBox ID="txtCilindraje" placeholder="EJ. 1.8" CssClass="form-control form-control-sm mascara-cilindraje" type="text" ReadOnly="true" Text="1800" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Recorrido</label>
                                <asp:TextBox ID="txtRecorrido" CssClass="form-control form-control-sm mascara-cantidad" ReadOnly="true" type="text" required="required" Text="65,950" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Unidad de medida</label>
                                <asp:TextBox ID="txtUnidadDeMedida" CssClass="form-control form-control-sm" ReadOnly="true" type="text" required="required" Text="Milla" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Transmisión</label>
                                <asp:TextBox ID="txtTransmision" placeholder="EJ. Automático" CssClass="form-control form-control-sm" type="text" ReadOnly="true" Text="Automática" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Tipo de combustible</label>
                                <asp:TextBox ID="txtTipoDeCombustible" placeholder="EJ. Gasolina" CssClass="form-control form-control-sm" type="text" ReadOnly="true" Text="Gasolina" required="required" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="row mb-0 text-center text-lg-left">
                    <div class="col-lg-2 col-md-3 col-6">
                        <div class="d-block mb-4 h-100">
                            <img class="img-fluid img-thumbnail" src="http://172.20.3.140/Documentos/Solicitudes/Solicitud326/G_326_2T1KR32EX3C158977_4c5e04ed-fd6c-4a9e-b073-3689a0378c1b.png" style="min-width: 205.84px; max-width: 205.84px; min-height: 142.5px; max-height: 142.5px;" alt="" />
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-3 col-6">
                        <div class="d-block mb-4 h-100">
                            <img class="img-fluid img-thumbnail" src="http://172.20.3.140/Documentos/Solicitudes/Solicitud326/G_326_2T1KR32EX3C158977_cca1db4e-7882-4508-818d-bef4283ba54a.png" style="min-width: 205.84px; max-width: 205.84px; min-height: 142.5px; max-height: 142.5px;" alt="" />
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-3 col-6">
                        <div class="d-block mb-4 h-100">
                            <img class="img-fluid img-thumbnail" src="http://172.20.3.140/Documentos/Solicitudes/Solicitud326/G_326_2T1KR32EX3C158977_1877762a-4056-408b-8422-08d369a4b4f8.png" style="min-width: 205.84px; max-width: 205.84px; min-height: 142.5px; max-height: 142.5px;" alt="" />
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-3 col-6">
                        <div class="d-block mb-4 h-100">
                            <img class="img-fluid img-thumbnail" src="http://172.20.3.140/Documentos/Solicitudes/Solicitud326/G_326_2T1KR32EX3C158977_5910bc6e-c4f0-4bc5-a28b-7f2de4ab474d.png" style="min-width: 205.84px; max-width: 205.84px; min-height: 142.5px; max-height: 142.5px;" alt="" />
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-3 col-6">
                        <div class="d-block mb-4 h-100">
                            <img class="img-fluid img-thumbnail" src="http://172.20.3.140/Documentos/Solicitudes/Solicitud326/G_326_2T1KR32EX3C158977_eeafbd08-256d-46ea-a64b-29b54a7f3ebc.png" style="min-width: 205.84px; max-width: 205.84px; min-height: 142.5px; max-height: 142.5px;" alt="" />
                        </div>
                    </div>
                    <div class="col-lg-2 col-md-3 col-6">
                        <div class="d-block mb-4 h-100">
                            <img class="img-fluid img-thumbnail" src="http://172.20.3.140/Documentos/Solicitudes/Solicitud326/G_326_2T1KR32EX3C158977_e66f1142-e60f-4199-bf68-e9eeee4659e7.png" style="min-width: 205.84px; max-width: 205.84px; min-height: 142.5px; max-height: 142.5px;" alt="" />
                        </div>
                    </div>
                </div>

                <div class="form-group row mr-0 ml-0 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                    <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
</body>
</html>
