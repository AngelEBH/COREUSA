<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_Mantenimiento.aspx.cs" Inherits="SolicitudesCredito_Mantenimiento" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Mantenimiento de solicitud</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        h6 {
            font-size: 1rem;
        }

        .card {
            box-shadow: none;
        }

        .nav-tabs > li > .active {
            background-color: whitesmoke !important;
        }
    </style>
</head>
<body class="EstiloBody" style="height: 100vh;">
    <form id="frmCalculadora" runat="server">
        <div class="card">
            <div class="card-header">
                <h6 class="card-title">Mantenimiento de solicitud</h6>
            </div>
            <div class="card-body">
                <div>
                    <div class="form-group form-row">
                        <div class="col-md-2">
                            <label class="col-form-label">No. Solicitud</label>
                            <asp:TextBox ID="txtNoSolicitud" type="tel" CssClass="form-control form-control-sm col-form-label" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-2">
                            <label class="col-form-label">Identidad</label>
                            <asp:TextBox ID="txtIdentidadCliente" type="tel" CssClass="form-control form-control-sm col-form-label" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label class="col-form-label">Nombre</label>
                            <asp:TextBox ID="txtNombreCliente" type="tel" CssClass="form-control form-control-sm col-form-label" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <!-- Nav tabs -->
                <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" id="navTabs">
                    <li class="nav-item">
                        <a class="nav-link active" data-toggle="tab" href="#tabInformacionSolicitud" role="tab" aria-selected="false">
                            <span class="d-block d-sm-none">Solicitud</span>
                            <span class="d-none d-sm-block">Información Solicitud</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-toggle="tab" href="#tabInformacionCliente" role="tab" aria-selected="false">
                            <span class="d-block d-sm-none">Cliente</span>
                            <span class="d-none d-sm-block">InformacionCliente</span>
                        </a>
                    </li>
                </ul>
                <!-- Tab panes -->
                <div class="tab-content" runat="server">

                    <!-- PESTAÑANA PRINCIPAL DE INFORMACION DE LA SOLICITUD  -->
                    <div class="tab-pane active mb-1" id="tabInformacionSolicitud" role="tabpanel">
                    </div>

                    <!-- PESTAÑA PRINCIPAL DE INFORMACION DEL CLIENTE-->
                    <div class="tab-pane" id="tabInformacionCliente" role="tabpanel">
                    </div>
                </div>

                <div class="form-group row" runat="server" id="PanelMensajeErrores">
                    <asp:Label CssClass="col-sm-2 col-form-label text-danger" ID="lblMensaje" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </form>

    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(document).ready(function () {
            $(".MascaraCantidad").inputmask("decimal", {
                alias: "numeric",
                groupSeparator: ",",
                digits: 2,
                integerDigits: 11,
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
            $(".identidad").inputmask("9999999999999");
        });
    </script>
</body>
</html>
