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

        .card {
            border: none;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }

        .nav-tabs > li > .active {
            background-color: whitesmoke !important;
        }

        .btn-actions-pane-right {
            margin-left: auto;
            white-space: nowrap
        }

        .scroll-area-sm {
            height: 50vh;
            overflow-x: hidden
        }

        .todo-indicator {
            position: absolute;
            width: 4px;
            height: 60%;
            border-radius: 0.3rem;
            left: 0.625rem;
            top: 20%;
            opacity: .6;
            transition: opacity .2s
        }

        .widget-content .widget-content-wrapper {
            display: flex;
            flex: 1;
            position: relative;
            align-items: center
        }

        .widget-content .widget-content-right.widget-content-actions {
            visibility: hidden;
            opacity: 0;
            transition: opacity .2s
        }

        .widget-content .widget-content-right {
            margin-left: auto
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
                        <div class="form-group row mt-2 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                            <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server">asdasdasdasdasd</asp:Label>
                        </div>
                        <div class="form-group mt-2 text-center">
                            <button class="btn btn-info" type="button">Cambiar estado a <span class="font-weight-bold">ENTREGADO</span></button>
                        </div>
                        <!-- Nav tabs -->
                        <ul class="nav nav-tabs nav-justified" role="tablist" runat="server" id="navTabs">
                            <li class="nav-item">
                                <a class="nav-link active" data-toggle="tab" href="#tab_Cliente" role="tab" aria-selected="false">
                                    <span class="d-block d-sm-none"><i class="fas fa-user"></i></span>
                                    <span class="d-none d-sm-block">Cliente</span>
                                </a>
                            </li>
                            <li class="nav-item">
                                <a class="nav-link" data-toggle="tab" href="#tab_Expediente" role="tab" aria-selected="false">
                                    <span class="d-block d-sm-none"><i class="fas fa-folder"></i></span>
                                    <span class="d-none d-sm-block">Expediente</span>
                                </a>
                            </li>
                        </ul>
                        <!-- Tab panes -->
                        <div class="tab-content" id="tabContent">
                            <div class="tab-pane active" id="tab_Cliente" role="tabpanel">
                                <div class="row mb-0">
                                    <div class="col-12">
                                        <h6>Información del cliente/solicitud</h6>
                                        <div class="form-group row">
                                            <div class="col-12">
                                                <label class="col-form-label">Cliente</label>
                                                <asp:TextBox ID="txtNombreCliente" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Identidad</label>
                                                <asp:TextBox ID="txtIdentidadCliente" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6 pr-1">
                                                <label class="col-form-label">RTN numérico</label>
                                                <asp:TextBox ID="txtRtn" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6 pl-1">
                                                <label class="col-form-label">Teléfono</label>
                                                <asp:TextBox ID="txtTelefonoCliente" type="tel" ReadOnly="true" CssClass="form-control form-control-sm col-form-label" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-12">
                                                <label class="col-form-label">Producto</label>
                                                <asp:TextBox ID="txtProducto" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6 pr-1">
                                                <label class="col-form-label">Monto a financiar</label>
                                                <asp:TextBox ID="txtMontoAFinanciar" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6 pl-1">
                                                <label class="col-form-label">Plazo</label>
                                                <asp:TextBox ID="txtPlazo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6 pr-1">
                                                <label class="col-form-label">Oficial de negocios</label>
                                                <asp:TextBox ID="txtOficialDeNegocios" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                            <div class="col-6 pl-1">
                                                <label class="col-form-label">Gestor de cobros</label>
                                                <asp:TextBox ID="txtGestorDeCobros" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="tab-pane" id="tab_Expediente" role="tabpanel">
                                <div class="row mb-0">
                                    <div class="col-12">
                                        <h6>Expediente de la solicitud</h6>
                                        <div class="scroll-area-sm border border-gray">
                                            <div style="position: static;" class="ps ps--active-y">
                                                <div class="ps-content">
                                                    <ul class=" list-group list-group-flush" runat="server" id="ulDocumentosExpediente"></ul>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-12 mt-3">
                                            <div class="form-group row">
                                                <label>Especifique otras</label>
                                                <textarea id="txtEspecifiqueOtras" class="form-control form-control-sm" readonly="readonly" rows="3" runat="server"></textarea>
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
