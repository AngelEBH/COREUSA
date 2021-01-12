<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesGPS_RevisionGarantia.aspx.cs" Inherits="SolicitudesGPS_RevisionGarantia" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Revisión de garantía</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/Content/css/icons.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
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

        .card-footer {
            background-color: #fff
        }

        .sw-theme-default .sw-toolbar{
            background:#fff;
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
                <h6>Confirmación de garantía</h6>
            </div>
            <div class="card-body p-0">

                <div id="smartwizard" class="h-100">
                    <ul>
                        <li><a href="#step-1" class="pt-3 pb-2 font-12">Información</a></li>
                        <li><a href="#step-2" class="pt-3 pb-2 font-12">Revisiones</a></li>
                    </ul>
                    <div>
                        <!-- Información principal -->
                        <div id="step-1" class="form-section">
                            <div class="row mb-0">
                                <div class="col-12">
                                    <div class="form-group row mt-2 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                                        <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
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
                                                <span class="d-none d-sm-block">Información fisica</span>
                                            </a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" data-toggle="tab" href="#tab_Informacion_Mecanica" id="tab_InformacionMecanica_link" role="tab" aria-selected="false">
                                                <span class="d-block d-sm-none"><i class="fas fa-cogs"></i></span>
                                                <span class="d-none d-sm-block">Información mecánica</span>
                                            </a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" data-toggle="tab" href="#tab_Fotografias" id="tab_Fotografias_link" role="tab" aria-selected="false">
                                                <span class="d-block d-sm-none"><i class="far fa-images"></i></span>
                                                <span class="d-none d-sm-block">Fotografías</span>
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
                                        <div class="tab-pane" id="tab_Fotografias" role="tabpanel">
                                            <div class="row mb-0">
                                                <div class="col-12">
                                                    <h6>Fotografías de la garantía</h6>
                                                    <div class="form-group row">
                                                        <div class="col-12">
                                                            <!-- Div donde se muestran las imágenes de la garantía-->
                                                            <div class="align-self-center" id="divGaleriaGarantia" runat="server" style="display: none;">
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
                        <div id="step-2" class="form-section">
                            <h6 class="mt-1"></h6>

                            <div class="row d-flex justify-content-center container p-0 m-0">
                                <div class="col-12 p-0">
                                    <div class="card-hover-shadow-2x mb-3 card">
                                        <div class="card-header-tab card-header">
                                            <div class="card-header-title font-size-lg text-capitalize font-weight-normal"><i class="fa fa-tasks"></i>&nbsp;Revisiones de la garantía</div>
                                        </div>
                                        <div class="scroll-area-sm">
                                            <div style="position: static;" class="ps ps--active-y">
                                                <div class="ps-content">
                                                    <ul class=" list-group list-group-flush">
                                                        <li class="list-group-item">
                                                            <div class="todo-indicator bg-warning"></div>
                                                            <div class="widget-content p-0">
                                                                <div class="widget-content-wrapper">
                                                                    <div class="widget-content-left flex2">
                                                                        <div class="widget-heading">
                                                                            Confirmación de datos
                                                                            <div class="badge badge-warning ml-2">Pendiente</div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="widget-content-right">
                                                                        <button class="border-0 btn-transition btn btn-outline-success" type="button"><i class="fas fa-check"></i></button>
                                                                        <button class="border-0 btn-transition btn btn-outline-danger" type="button"><i class="fas fa-times"></i></button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li class="list-group-item">
                                                            <div class="todo-indicator bg-success"></div>
                                                            <div class="widget-content p-0">
                                                                <div class="widget-content-wrapper">
                                                                    <div class="widget-content-left">
                                                                        <div class="widget-heading">
                                                                            Revisión mecánica
                                                                            <div class="badge badge-success ml-2">Aprobado</div>
                                                                        </div>
                                                                        <%--<div class="widget-subheading">Confirmación del estado mecánico de la garantía</div>--%>
                                                                    </div>
                                                                    <div class="widget-content-right">
                                                                        <button class="border-0 btn-transition btn btn-outline-success" type="button"><i class="fas fa-check"></i></button>
                                                                        <button class="border-0 btn-transition btn btn-outline-danger" type="button"><i class="fas fa-times"></i></button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </li>
                                                        <li class="list-group-item">
                                                            <div class="todo-indicator bg-warning"></div>
                                                            <div class="widget-content p-0">
                                                                <div class="widget-content-wrapper">
                                                                    <div class="widget-content-left">
                                                                        <div class="widget-heading">
                                                                            Revisión estética
                                                                            <div class="badge badge-pill badge-warning ml-2">Pendiente</div>
                                                                        </div>
                                                                        <%--<div class="widget-subheading"> Confirmación del estado estético de la garantía </div>--%>
                                                                    </div>
                                                                    <div class="widget-content-right">
                                                                        <button class="border-0 btn-transition btn btn-outline-success" type="button"><i class="fas fa-check"></i></button>
                                                                        <button class="border-0 btn-transition btn btn-outline-danger" type="button"><i class="fas fa-times"></i></button>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </li>
                                                    </ul>
                                                </div>
                                            </div>
                                        </div>
                                        <%--<div class="d-block text-right card-footer">
                                            <button class="btn btn-info">Finalizar revisión</button>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>


        <div id="modalResultadoRevision" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalResultadoRevisionLabel" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalResultadoRevisionLabel">Resultado de <b>Revisión mecánica</b></h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row mb-1">
                            <div class="col-12 mb-2">
                                <label>Descripción de la revisión:</label>
                                <textarea id="txtDescripcionRevision" runat="server" class="form-control form-control-sm" readonly="readonly">Confirmación del estado mecánico de la garantía</textarea>
                            </div>
                            <div class="col-12 mb-2">
                                <label>Observaciones</label>
                                <textarea id="txtObservacionesResultadoRevision" runat="server" class="form-control form-control-sm" data-parsley-maxlength="300" rows="2"></textarea>
                            </div>
                            <div class="col-12 text-center">
                                <div class="button-items pb-2">
                                    <button runat="server" id="btnAceptarSolicitud" type="button" data-toggle="modal" disabled="disabled" data-target="#modalResolucionAprobar" title="" class="btn btn-success waves-effect waves-light validador">
                                        <i class="fas fa-thumbs-up"></i>
                                        Aceptar
                                    </button>
                                    <button runat="server" id="btnRechazar" type="button" data-toggle="modal" data-target="#modalResolucionRechazar" class="btn btn-danger waves-effect waves-light validador">
                                        <i class="fas fa-thumbs-down"></i>
                                        Rechazar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer pt-2 pb-2">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tilesgrid/ug-theme-tilesgrid.js"></script>
    <script>
        $("#divGaleriaGarantia").unitegallery({
            gallery_width: 900,
            gallery_height: 600
        });
    </script>
    <script src="/Scripts/app/SolicitudesGPS/SolicitudesGPS_RevisionGarantia.js?v=20201216081425"></script>
</body>
</html>

