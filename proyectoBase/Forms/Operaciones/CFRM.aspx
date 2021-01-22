<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CFRM.aspx.cs" Inherits="CFRM" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Expediente</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/Content/css/icons.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <style>
        html {
            background-color: rgb(255,255,255) !important;
        }
    </style>
</head>
<body>
    <form id="frmPrincipal" runat="server">
        <div class="card shadow-none m-0">
            <div class="card-header pb-1 pt-1">
                <h6>Expediente solicitud de crédito N°
                    <asp:Label ID="lblNoSolicitudCredito" CssClass="font-weight-bold" runat="server"></asp:Label>
                </h6>
            </div>
            <div class="card-body">
                <div class="row mb-0">
                    <div class="col-12">
                        <div class="form-group row mt-2 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="true">
                            <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server">Pantalla en mantenimiento</asp:Label>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Cliente</label>
                                <asp:TextBox ID="txtNombreCliente" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-12">
                                <label class="col-form-label">Identidad</label>
                                <asp:TextBox ID="txtIdentidadCliente" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">RTN numérico</label>
                                <asp:TextBox ID="txtRtn" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Teléfono</label>
                                <asp:TextBox ID="txtTelefonoCliente" type="tel" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs">
                            <li class="nav-item">
                                <a class="nav-link active" data-toggle="tab" href="#tab_Informacion_Fisica" id="tab_InformacionFisica_link" role="tab" aria-selected="false">
                                    <span class="d-block d-sm-none"><i class="fas fa-car"></i></span>
                                    <span class="d-none d-sm-block">Solicitud de crédito</span>
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-toggle="tab" href="#tab_Informacion_Mecanica" id="tab_InformacionMecanica_link" role="tab" aria-selected="false">
                                    <span class="d-block d-sm-none"><i class="fas fa-cogs"></i></span>
                                    <span class="d-none d-sm-block">Expediente</span>
                                </a>
                            </li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content" id="tabContent">

                            <div class="tab-pane active" id="tab_Informacion_Fisica" role="tabpanel">
                                <div class="row mb-0">
                                    <div class="col-12">
                                        <h6>Características físicas</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">VIN</label>
                                                <asp:TextBox ID="txtVIN" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Tipo de garantía</label>
                                                <asp:TextBox ID="txtTipoDeGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6">
                                                <label class="col-form-label">Tipo de vehículo</label>
                                                <asp:TextBox ID="txtTipoDeVehiculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-6">
                                                <label class="col-form-label">Marca</label>
                                                <asp:TextBox ID="txtMarca" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-6">
                                                <label class="col-form-label">Modelo</label>
                                                <asp:TextBox ID="txtModelo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-6">
                                                <label class="col-form-label">Año</label>
                                                <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-6">
                                                <label class="col-form-label">Color</label>
                                                <asp:TextBox ID="txtColor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-6">
                                                <label class="col-form-label">Placa</label>
                                                <asp:TextBox ID="txtMatricula" CssClass="form-control form-control-sm" ReadOnly="true" type="text" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-6">
                                                <label class="col-form-label">Serie Motor</label>
                                                <asp:TextBox ID="txtSerieMotor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-12">
                                                <label class="col-form-label">Serie Chasis</label>
                                                <asp:TextBox ID="txtSerieChasis" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-3 col-12">
                                                <label class="col-form-label">GPS</label>
                                                <asp:TextBox ID="txtGPS" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane" id="tab_Informacion_Mecanica" role="tabpanel">
                                <div class="row mb-0">
                                    <div class="col-12">
                                        <h6>Características mecánicas</h6>
                                        <div class="form-group row">
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Cilindraje</label>
                                                <asp:TextBox ID="txtCilindraje" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Recorrido</label>
                                                <asp:TextBox ID="txtRecorrido" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Transmisión</label>
                                                <asp:TextBox ID="txtTransmision" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Tipo de combustible</label>
                                                <asp:TextBox ID="txtTipoDeCombustible" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Serie 1</label>
                                                <asp:TextBox ID="txtSerieUno" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-sm-4 col-6">
                                                <label class="col-form-label">Serie 2</label>
                                                <asp:TextBox ID="txtSerieDos" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Comentario</label>
                                                <textarea id="txtComentario" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
</body>
</html>

