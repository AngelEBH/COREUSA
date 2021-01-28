<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesGPS_RegistroInstalacionGPS.aspx.cs" Inherits="SolicitudesGPS_RegistroInstalacionGPS" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Instalación de GPS</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />    
    <link href="/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
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

        .sw-theme-default .sw-toolbar {
            background: #fff;
        }

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
<body>
    <form id="frmPrincipal" runat="server">
        <div class="card m-0">
            <div class="card-header pb-1 pt-1">
                <h6>Instalación de GPS</h6>
            </div>
            <div class="card-body p-0">
                <div id="smartwizard" class="h-100">
                    <ul>
                        <li><a href="#step-1" class="pt-3 pb-2 font-12">Información principal</a></li>
                        <li><a href="#step-2" class="pt-3 pb-2 font-12">Registro de instalación GPS</a></li>
                    </ul>
                    <div>
                        <div id="step-1" class="form-section">
                            <div class="row mb-0">
                                <div class="col-12">
                                    <div class="form-group row mt-2 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                                        <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
                                    </div>
                                    <!-- Nav tabs -->
                                    <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs">
                                        <li class="nav-item">
                                            <a class="nav-link active" data-toggle="tab" href="#tab_Informacion_GPS" role="tab" aria-selected="false">
                                                <span class="d-block d-sm-none"><i class="fas fa-map-marker-alt"></i></span>
                                                <span class="d-none d-sm-block">Información del GPS</span>
                                            </a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" data-toggle="tab" href="#tab_Informacion_Garantia" role="tab" aria-selected="false">
                                                <span class="d-block d-sm-none"><i class="fas fa-car"></i></span>
                                                <span class="d-none d-sm-block">Información de la garantía</span>
                                            </a>
                                        </li>
                                    </ul>
                                    <!-- Tab panes -->
                                    <div class="tab-content" id="tabContent">
                                        <div class="tab-pane active" id="tab_Informacion_GPS" role="tabpanel">
                                            <div class="row mb-0">
                                                <div class="col-12">
                                                    <h6>Información del GPS asingnado</h6>
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
                                                </div>
                                            </div>
                                        </div>
                                        <div class="tab-pane" id="tab_Informacion_Garantia" role="tabpanel">
                                            <div class="row mb-0">
                                                <div class="col-12">
                                                    <h6>Información de la garantía</h6>
                                                    <div class="form-group row">
                                                        <div class="col-12">
                                                            <label class="col-form-label">VIN</label>
                                                            <asp:TextBox ID="txtVIN" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-6 pr-1">
                                                            <label class="col-form-label">Tipo de garantía</label>
                                                            <asp:TextBox ID="txtTipoDeGarantia" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-6 pl-1">
                                                            <label class="col-form-label">Tipo de vehículo</label>
                                                            <asp:TextBox ID="txtTipoDeVehiculo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3 col-6 pr-1">
                                                            <label class="col-form-label">Marca</label>
                                                            <asp:TextBox ID="txtMarca" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3 col-6 pl-1">
                                                            <label class="col-form-label">Modelo</label>
                                                            <asp:TextBox ID="txtModelo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3 col-6 pr-1">
                                                            <label class="col-form-label">Año</label>
                                                            <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3 col-6 pl-1">
                                                            <label class="col-form-label">Color</label>
                                                            <asp:TextBox ID="txtColor" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                                        </div>
                                                        <div class="col-sm-3 col-12">
                                                            <label class="col-form-label">Placa</label>
                                                            <asp:TextBox ID="txtMatricula" CssClass="form-control form-control-sm" ReadOnly="true" type="text" runat="server"></asp:TextBox>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Fotografías de la instalación -->
                        <div id="step-2" class="form-section">
                            <div class="form-group row m-0 border-bottom border-gray mb-3">
                                <div class="col-12 p-0">
                                    <label class="mt-1">Detalles de la instalación</label>
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="col-12">
                                    <label class="col-form-label">Ubicación de la instalación</label>
                                    <textarea id="txtUbicacion" runat="server" class="form-control form-control-sm"></textarea>
                                </div>
                                <div class="col-12">
                                    <label class="col-form-label">Comentarios de la instalación</label>
                                    <textarea id="txtComentariosDeLaInstalacion" runat="server" class="form-control form-control-sm"></textarea>
                                </div>
                            </div>

                            <div class="form-group row m-0 border-bottom border-gray mb-3">
                                <div class="col-12 p-0">
                                    <label class="mt-1">Fotografías de la instalación</label>
                                </div>
                            </div>

                            <!-- Div donde se generan dinamicamente los inputs para las fotografías -->
                            <div id="DivDocumentacion">
                                <%--<div class="form-group">
                                    <label>Foto vehículo</label>
                                    <input type="file" class="filestyle" data-buttonname="btn-secondary" id="filestyle-0" tabindex="-1" style="position: absolute; clip: rect(0px, 0px, 0px, 0px);"/>
                                    <div class="bootstrap-filestyle input-group">
                                        <input type="text" class="form-control " placeholder="" disabled=""/>
                                        <span class="group-span-filestyle input-group-append" tabindex="0">
                                            <label for="filestyle-0" class="btn btn-secondary">
                                                <span class="icon-span-filestyle fas fa-folder-open"></span>
                                                <span class="buttonText">Subir archivo</span>
                                            </label>
                                        </span>
                                    </div>
                                </div>--%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="loading" id="divLoader" style="display: none;">
            <div class="loader"></div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script src="/Scripts/app/SolicitudesGPS/SolicitudesGPS_RegistroInstalacionGPS.js?v=20210116115525"></script>
</body>
</html>
