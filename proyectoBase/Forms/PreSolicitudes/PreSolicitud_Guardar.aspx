<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSolicitud_Guardar.aspx.cs" Inherits="PreSolicitud_Guardar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Nueva Pre Solicitud</title>
    <!-- BOOTSTRAP -->
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <style>
        html, body {
            background-color: #fff;
        }

        .card {
            box-shadow: none;
        }

        /* Loader */
        .loading {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background-color: transparent;
        }

        .loader {
            left: 50%;
            margin-left: -4em;
            font-size: 10px;
            border: .8em solid rgba(218, 219, 223, 1);
            border-left: .8em solid rgba(58, 166, 165, 1);
            animation: spin 1.1s infinite linear;
        }

            .loader, .loader:after {
                border-radius: 50%;
                width: 8em;
                height: 8em;
                display: block;
                position: absolute;
                top: 50%;
                margin-top: -4.05em;
            }

        @keyframes spin {
            0% {
                transform: rotate(360deg);
            }

            100% {
                transform: rotate(0deg);
            }
        }
    </style>
</head>
<body class="EstiloBody">
    <form id="frmGuardarPreSolicitud" runat="server">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <div class="card m-0">
            <div class="card-body pt-0">
                <asp:UpdatePanel runat="server" ID="upCotizador" UpdateMode="Always" ChildrenAsTriggers="True">
                    <ContentTemplate>
                        <asp:Panel ID="divInformacionDomicilio" runat="server" Visible="true">

                            <h5 class="border-bottom pb-2">Guardar nueva Pre-Solicitud</h5>

                            <!-- INFORMACION DEL CLIENTE -->
                            <div class="form-group form-row">
                                <div class="col-12">
                                    <label class="col-form-label">Cliente</label>
                                    <asp:TextBox ID="txtNombreCliente" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="col-form-label">Identidad</label>
                                    <asp:TextBox ID="txtIdentidadCliente" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-6">
                                    <label class="col-form-label">Teléfono</label>
                                    <asp:TextBox ID="txtTelefonoCliente" type="tel" Enabled="false" CssClass="form-control form-control-sm col-form-label mascara-telefono" Text="" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group form-row">
                                <div class="col-sm-3">
                                    <label class="col-form-label">Departamento</label>
                                    <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="form-control form-control-sm col-form-label" OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Municipio</label>
                                    <asp:DropDownList ID="ddlMunicipio" Enabled="false" runat="server" CssClass="form-control form-control-sm col-form-label" OnSelectedIndexChanged="ddlMunicipio_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Ciudad/Poblado</label>
                                    <asp:DropDownList ID="ddlCiudad" Enabled="false" runat="server" CssClass="form-control form-control-sm col-form-label" OnSelectedIndexChanged="ddlCiudad_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Barrio/Colonia</label>
                                    <asp:DropDownList ID="ddlBarrioColonia" Enabled="false" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group form-row">
                                <div class="col-sm-6">
                                    <label class="col-form-label">Detalle dirección</label>
                                    <asp:TextBox ID="txtDireccionDetallada" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-6">
                                    <label class="col-form-label">Teléfono casa</label>
                                    <asp:TextBox ID="txtTelefonoCasa" type="tel" CssClass="form-control form-control-sm col-form-label mascara-telefono" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-12">
                                    <label class="col-form-label">Referencias del domicilio</label>
                                    <textarea id="txtReferenciasDomicilio" runat="server" class="form-control" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2"></textarea>
                                </div>
                            </div>

                        </asp:Panel>
                        <!-- Label para mensajes y advertencias -->
                        <div class="form-group row mr-0 ml-0 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                            <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
                        </div>

                        <div class="form-group row mb-0">
                            <div class="button-items col-sm-12">
                                <asp:Button ID="btnGuardarPreSolicitud" Text="Guardar Pre-Solicitud" CssClass="btn btn-primary btn-lg waves-effect waves-light" UseSubmitBehavior="false" OnClick="btnGuardarPreSolicitud_Click" runat="server" />
                            </div>
                        </div>

                        <script type="text/javascript">

                            function CargarMascarasDeEntrada() {
                                $(".mascara-telefono").inputmask("9999-9999");
                            }

                            Sys.Application.add_load(CargarMascarasDeEntrada);
                        </script>
                        <script type="text/javascript">
                            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
                            function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }
                        </script>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="updateProgressCotizador" runat="server">
                    <ProgressTemplate>
                        <div class="loading">
                            <div class="loader"></div>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(document).ready(function () {
            CargarMascarasDeEntrada();
        });
    </script>
</body>
</html>
