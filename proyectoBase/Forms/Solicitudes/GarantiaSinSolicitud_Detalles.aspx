<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GarantiaSinSolicitud_Detalles.aspx.cs" Inherits="GarantiaSinSolicitud_Detalles" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <meta name="description" content="Detalles de la garantía" />
    <title>Detalles de la garantía</title>
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/css/unitegallery.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .card {
            border: none;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }
    </style>
</head>
<body>
    <form id="frmPrincipal" runat="server">
        <div class="card m-0">
            <div class="card-header pb-1 pt-1">
                <h5>Detalles de garantía sin solicitud</h5>
            </div>
            <div class="card-body pt-0">
                <div class="row mb-0" id="divInformacionGarantia" runat="server">
                    <div class="col-lg-6">
                        <h6 class="mb-0">Características físicas</h6>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">VIN</label>
                                <asp:TextBox ID="txtVIN" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Tipo de garantía</label>
                                <asp:TextBox ID="txtTipoDeGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-6">
                                <label class="col-form-label">Tipo de vehículo</label>
                                <asp:TextBox ID="txtTipoDeVehiculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6">
                                <label class="col-form-label">Marca</label>
                                <asp:TextBox ID="txtMarca" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6">
                                <label class="col-form-label">Modelo</label>
                                <asp:TextBox ID="txtModelo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6">
                                <label class="col-form-label">Año</label>
                                <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6">
                                <label class="col-form-label">Color</label>
                                <asp:TextBox ID="txtColor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6">
                                <label class="col-form-label">Matrícula</label>
                                <asp:TextBox ID="txtMatricula" CssClass="form-control form-control-sm" ReadOnly="true" type="text" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4 col-6">
                                <label class="col-form-label">GPS</label>
                                <asp:TextBox ID="txtGPS" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6 col-6">
                                <label class="col-form-label">Serie Motor</label>
                                <asp:TextBox ID="txtSerieMotor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-6 col-6">
                                <label class="col-form-label">Serie Chasis</label>
                                <asp:TextBox ID="txtSerieChasis" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>

                        </div>
                        <h6 class="m-0 pt-2">Características mecánicas</h6>
                        <div class="form-group row">
                            <div class="col-sm-4">
                                <label class="col-form-label">Cilindraje</label>
                                <asp:TextBox ID="txtCilindraje" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Recorrido</label>
                                <asp:TextBox ID="txtRecorrido" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Transmisión</label>
                                <asp:TextBox ID="txtTransmision" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Tipo de combustible</label>
                                <asp:TextBox ID="txtTipoDeCombustible" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Serie 1</label>
                                <asp:TextBox ID="txtSerieUno" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <label class="col-form-label">Serie 2</label>
                                <asp:TextBox ID="txtSerieDos" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-12">
                                <label class="col-form-label">Comentario</label>
                                <textarea id="txtComentario" runat="server" readonly="readonly" class="form-control form-control-sm"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-6 border-left border-gray">
                        <h6>Fotografías de la garantía</h6>
                        <div class="form-group row">
                            <div class="col-12">
                                <!-- Div donde se muestran las imágenes de la garantía-->
                                <div class="align-self-center" id="divGaleriaGarantia" runat="server" style="display: none;"></div>
                            </div>
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
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tilesgrid/ug-theme-tilesgrid.js"></script>
    <script>
        $("#divGaleriaGarantia").unitegallery({
            gallery_width: 900,
            gallery_height: 600
        });
    </script>
</body>
</html>
