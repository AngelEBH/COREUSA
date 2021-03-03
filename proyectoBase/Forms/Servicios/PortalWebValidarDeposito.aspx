<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalWebValidarDeposito.aspx.cs" Inherits="PortalWebValidarDeposito" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Validar depósito</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
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
    </style>
</head>
<body class="EstiloBody">
    <form id="frmPrincipal" runat="server">
        <div class="card m-0">
            <div class="card-header pb-1 pt-1">
                <div class="float-right p-1" id="Loader" style="display: none;">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>
                <h6>Documentos del cliente&nbsp;<asp:Label ID="lblCodigoCliente" class="font-weight-bold" runat="server"></asp:Label></h6>
            </div>
            <div class="card-body">
                <div class="form-group row mr-0 ml-0 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                    <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
                </div>

                <div class="row mt-3">
                    <div class="col-6">
                        <div class="table-responsive">
                            <table class="table table-sm" style="margin-bottom: 0px;">
                                <tbody>
                                    <tr>
                                        <td><span class="font-weight-bold">Información del cliente</span></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Cliente</span></td>
                                        <td><span class="label label-table label-success lblNombreCliente"></span></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Identidad</span></td>
                                        <td class="lblIdentidadCliente"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="col-6">
                        <div class="table-responsive">
                            <table class="table table-sm" style="margin-bottom: 0px;">
                                <tbody>
                                    <tr>
                                        <td colspan="3"><span class="font-weight-bold">Información de la solicitud</span></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Producto</span></td>
                                        <td class="lblProducto"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">CC / Vendedor</span></td>
                                        <td class="lblAgenciaYVendedorAsignado"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="row mt-3">
                    <div class="col-sm-4 border-right border-top pt-3">
                        <div class="table-responsive border-right border p-2" style="max-height: 50vh; overflow: auto;">
                            <table id="tblExpedienteSolicitudGarantia" class="table table-sm table-hover cursor-pointer" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Documento</th>
                                        <%--<th>Nombre</th>--%>
                                        <th class="text-center no-sort"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="col-sm-8 border-top pt-3">
                        <h6 class="font-weight-bold mt-0">Previsualización</h6>

                        <div class="align-self-center" id="divPrevisualizacionDocumento" runat="server" style="display: none;"></div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tilesgrid/ug-theme-tilesgrid.js"></script>
</body>
</html>
