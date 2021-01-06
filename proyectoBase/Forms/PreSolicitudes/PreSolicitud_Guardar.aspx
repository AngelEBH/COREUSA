<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSolicitud_Guardar.aspx.cs" Inherits="PreSolicitud_Guardar" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Nueva Pre Solicitud</title>
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/CSS/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
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
<body>
    <form id="frmPreSolicitud" runat="server" class="" action="#" data-parsley-excluded="[disabled]">
        <div class="card m-0">
            <div class="card-header pb-1 pt-1">
                <!-- loader -->
                <div class="float-right p-1" id="Loader" style="display: none;">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>
                <h6>Guardar nueva Pre Solicitud <small><span runat="server" id="lblMensajeError" class="text-danger" visible="false"></span></small></h6>
            </div>
            <div class="card-body">

                <div id="smartwizard" class="h-100">
                    <ul>
                        <li><a href="#step-1" class="pt-3 pb-2 font-12">Informacion del cliente</a></li>
                        <li><a href="#step-2" class="pt-3 pb-2 font-12">Documentación</a></li>
                    </ul>
                    <div>
                        <!-- Información principal -->
                        <div id="step-1" class="form-section">

                            <!-- loader -->
                            <div class="float-right" id="spinnerCargando" runat="server" visible="false">
                                <div class="spinner-border" role="status">
                                    <span class="sr-only"></span>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col-12">
                                    <label class="col-form-label">Cliente</label>
                                    <asp:TextBox ID="txtNombreCliente" ReadOnly="true" required="required" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
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
                                <div class="col-md-4 col-sm-4 col-6">
                                    <label class="col-form-label">Tipo de investigación</label>
                                    <asp:DropDownList ID="ddlTipoInvestigacionDeCampo" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-errors-container="#error-ddlTipoInvestigacionDeCampo"></asp:DropDownList>
                                    <div id="error-ddlTipoInvestigacionDeCampo"></div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-6">
                                    <label class="col-form-label">Gestor de campo</label>
                                    <asp:DropDownList ID="ddlGestores" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-errors-container="#error-ddlGestores"></asp:DropDownList>
                                    <div id="error-ddlGestores"></div>
                                </div>
                                <div class="col-md-4 col-sm-4 col-12">
                                    <label class="col-form-label">Nombre del trabajo</label>
                                    <asp:TextBox ID="txtNombreTrabajo" type="tel" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-4 col-sm-4 col-12">
                                    <label class="col-form-label">Teléfono <span class="font-weight-bold" id="lblTipoDeNumeroDeTelefono"></span></label>
                                    <asp:TextBox ID="txtTelefonoAdicional" type="tel" CssClass="form-control form-control-sm col-form-label mascara-telefono" Text="" runat="server" required="required"></asp:TextBox>
                                </div>
                                <div class="col-md-4 col-sm-4 col-6">
                                    <label class="col-form-label">Extensión cliente</label>
                                    <asp:TextBox ID="txtExtensionCliente" type="tel" CssClass="form-control form-control-sm col-form-label mascara-extension" Text="" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-md-4 col-sm-4 col-6">
                                    <label class="col-form-label">Extensión RRHH</label>
                                    <asp:TextBox ID="txtExtensionRecursosHumanos" type="tel" CssClass="form-control form-control-sm col-form-label mascara-extension" Text="" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col-md-3 col-sm-6">
                                    <label class="col-form-label">Departamento</label>
                                    <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-ddlDepartamento"></asp:DropDownList>
                                    <div id="error-ddlDepartamento"></div>
                                </div>
                                <div class="col-md-3 col-sm-6">
                                    <label class="col-form-label">Municipio</label>
                                    <asp:DropDownList ID="ddlMunicipio" Enabled="false" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-ddlMunicipio"></asp:DropDownList>
                                    <div id="error-ddlMunicipio"></div>
                                </div>
                                <div class="col-md-3 col-sm-6">
                                    <label class="col-form-label">Ciudad/Poblado</label>
                                    <asp:DropDownList ID="ddlCiudadPoblado" Enabled="false" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-ddlCiudadPoblado"></asp:DropDownList>
                                    <div id="error-ddlCiudadPoblado"></div>
                                </div>
                                <div class="col-md-3 col-sm-6">
                                    <label class="col-form-label">Barrio/Colonia</label>
                                    <asp:DropDownList ID="ddlBarrioColonia" Enabled="false" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-ddlBarrioColonia"></asp:DropDownList>
                                    <div id="error-ddlBarrioColonia"></div>
                                </div>
                            </div>
                            <div class="form-group form-row">
                                <div class="col-sm-12">
                                    <label class="col-form-label">Dirección detallada</label>
                                    <textarea id="txtDireccionDetallada" runat="server" class="form-control" data-parsley-maxlength="256" data-parsley-minlength="15" rows="2" required="required"></textarea>
                                </div>
                                <div class="col-12">
                                    <label class="col-form-label">Referencias de la dirección</label>
                                    <textarea id="txtReferenciasDireccionDetallada" runat="server" class="form-control" data-parsley-maxlength="256" data-parsley-minlength="15" rows="2" required="required"></textarea>
                                </div>
                            </div>
                        </div>
                        <!-- Documentación -->
                        <div id="step-2" class="form-section">
                            <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-12 p-0">
                                    <h6 class="mt-1">Documentación <small class="text-info">(Estimado usuario, recuerda subir toda la documentación hasta que ya vayas a guardar la pre solicitud)</small></h6>
                                </div>
                            </div>
                            <!-- Div donde se generan dinamicamente los inputs para la documentación -->
                            <div class="row pr-1 pl-1 text-center" id="DivDocumentacion">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(document).ready(function () {

            $(".mascara-telefono").inputmask("9999-9999");
            $(".mascara-extension").inputmask("999999");
        });
    </script>
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/app/solicitudes/PreSolicitud_Guardar.js?v=20201126132785"></script>
</body>
</html>
