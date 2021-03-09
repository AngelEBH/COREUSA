<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesGPS_DetallesInstalacionGPS.aspx.cs" Inherits="SolicitudesGPS_DetallesInstalacionGPS" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <meta name="description" content="Detalles de la instalación de GPS" />
    <title>Detalles de la instalación de GPS</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/css/unitegallery.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.css" rel="stylesheet" />
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
    </style>
</head>
<body>
    <form id="frmPrincipal" runat="server" data-parsley-excluded="[disabled]">
        <div class="card m-0">
            <div class="card-header pb-1 pt-1">
                <h6>Detalles de la instalación de GPS</h6>
            </div>
            <div class="card-body">
                <div class="row mb-0">
                    <div class="col-12">
                        <div class="form-group row mt-2 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                            <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
                        </div>
                    </div>
                    <div class="col-12">
                        <div class="form-group mb-0">
                            <label class="d-block mb-0 font-weight-bold">
                                <span id="lblMarca" runat="server"></span>
                                <span id="lblModelo" runat="server"></span>
                                <span id="lblAnio" runat="server"></span>
                                <span id="lblColor" runat="server"></span>
                            </label>
                            <label class="text-muted mb-0">
                                VIN:
                                    <span id="lblVIN" runat="server"></span>
                                | Placa:
                                    <span id="lblPlaca" runat="server"></span>
                            </label>
                        </div>
                        <div class="form-group row">
                            <div class="col-sm-4 col-12">
                                <label class="col-form-label">IMEI</label>
                                <asp:TextBox ID="txtIMEI" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6 pr-1">
                                <label class="col-form-label">Serie</label>
                                <asp:TextBox ID="txtSerie" CssClass="form-control form-control-sm" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6 pl-1">
                                <label class="col-form-label">Modelo</label>
                                <asp:TextBox ID="txtModeloGPS" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6 pr-1">
                                <label class="col-form-label">Compañia</label>
                                <asp:TextBox ID="txtCompania" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6 pl-1">
                                <label class="col-form-label">Con Relay</label>
                                <asp:TextBox ID="txtConRelay" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Ubicación de la instalación</label>
                                <textarea id="txtUbicacion" runat="server" class="form-control form-control-sm" readonly="readonly" rows="3"></textarea>
                            </div>
                            <div class="col-12">
                                <label class="col-form-label">Observaciones de la instalación</label>
                                <textarea id="txtComentariosDeLaInstalacion" runat="server" class="form-control form-control-sm" readonly="readonly" rows="3"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="col-12">
                        <h6>Fotografías de la instalación</h6>
                        <div class="form-group row">
                            <div class="col-12">
                                <!-- Div donde se muestran las imágenes de la garantía-->
                                <div class="align-self-center" id="divGaleriaGarantia" runat="server" style="display: none;"></div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script>
        $("#divGaleriaGarantia").unitegallery({
            gallery_width: 900,
            gallery_height: 600
        });
    </script>
</body>
</html>
