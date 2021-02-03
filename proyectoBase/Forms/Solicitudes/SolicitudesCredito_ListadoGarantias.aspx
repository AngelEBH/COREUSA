<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_ListadoGarantias.aspx.cs" Inherits="SolicitudesCredito_ListadoGarantias" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Garantías de solicitudes aprobadas</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/css/unitegallery.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.css" rel="stylesheet" />
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <link href="/Content/css/bootstrap4-modal-fullscreen.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .opcion {
            cursor: pointer;
        }

        .cursor-zoom-in {cursor: zoom-in;}

        .dataTable tbody tr {
            cursor: pointer;
        }

        .dataTable tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }

        div.dt-buttons {
            position: relative;
            float: left;
        }

        .nav-tabs > li > .active {
            background-color: whitesmoke !important;
        }
    </style>
</head>
<body runat="server" class="EstiloBody-Listado">

    <div class="card">
        <div class="card-header">
            <div class="row">
                <div class="col-8 pr-0">
                    <h6>Garantías de solicitudes crediticias
                        <small runat="server" id="lblMensajeError" class="text-danger"></small>
                    </h6>
                </div>
                <div class="col-4 pl-0">
                    <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar" aria-label="Buscar" />
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="row mb-0">
                <div class="col-md-12">
                    <div class="form-group row">
                        <div class="col-lg-2 col-sm-3 pr-0 align-self-end">
                            <button type="button" id="btnRegistrarGarantiaSinSolicitud" class="btn btn-block btn-warning pr-0 pl-0">Registrar sin solicitud</button>
                        </div>
                        <div class="col-lg-3 col-sm-3 align-self-end">
                            <label class="col-form-label">Búsqueda por Mes</label>
                            <select id="mesIngreso" class="form-control form-control-sm">
                                <option value="" selected="selected">Seleccionar</option>
                                <option value="01">Enero</option>
                                <option value="02">Febrero</option>
                                <option value="03">Marzo</option>
                                <option value="04">Abril</option>
                                <option value="05">Mayo</option>
                                <option value="06">Junio</option>
                                <option value="07">Julio</option>
                                <option value="08">Agosto</option>
                                <option value="09">Septiembre</option>
                                <option value="10">Octubre</option>
                                <option value="11">Noviembre</option>
                                <option value="12">Diciembre</option>
                            </select>
                        </div>
                        <div class="col-lg-3 col-sm-3">
                            <label class="col-form-label">Búsqueda por Fecha</label>
                            <div class="input-daterange input-group" id="date-range">
                                <input type="text" class="form-control form-control-sm" name="min" id="min" />
                                <input type="text" class="form-control form-control-sm" name="max" id="max" />
                            </div>
                        </div>
                        <div class="col-lg-4 col-sm-3 align-self-end mt-2">
                            <div class="table-responsive">
                                <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                                    <label class="btn btn-secondary active opcion">
                                        <input id="general" type="radio" name="filtro-registro-garantia" value="0" />
                                        General
                                    </label>
                                    <label class="btn btn-secondary opcion">
                                        <input id="recepcion" type="radio" name="filtro-registro-garantia" value="1" />
                                        Pendientes
                                    </label>
                                    <label class="btn btn-secondary opcion">
                                        <input id="analisis" type="radio" name="filtro-registro-garantia" value="2" />
                                        Guardadas
                                    </label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Nav tabs -->
            <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs">
                <li class="nav-item">
                    <a class="nav-link active" data-toggle="tab" href="#tab_Listado_Solicitudes_Garantias" id="tab_Listado_Solicitudes_Garantias_link" role="tab" aria-selected="false">
                        <span class="d-block d-sm-none">Garantías
                            <br />
                            solicitudes aprobadas</span>
                        <span class="d-none d-sm-block">Garantías de solicitudes aprobadas</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" data-toggle="tab" href="#tab_Listado_Garantias_SinSolicitud" id="tab_Listado_Garantias_SinSolicitud_link" role="tab" aria-selected="false">
                        <span class="d-block d-sm-none">Garantías
                            <br />
                            sin solicitud</span>
                        <span class="d-none d-sm-block">Garantías sin solicitud</span>
                    </a>
                </li>
            </ul>
            <!-- Tab panes -->
            <div class="tab-content" id="tabContent">
                <!-- Listado de solicitudes aprobadas que requieren garantías -->
                <div class="tab-pane active" id="tab_Listado_Solicitudes_Garantias" role="tabpanel">
                    <div class="table-responsive mt-2">
                        <table id="datatable-principal-garantias" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                            <thead>
                                <tr>
                                    <th class="no-sort">Acciones</th>
                                    <th class="report-data">Solicitud N°<br />
                                        Fecha de ingreso</th>
                                    <th class="report-data">Producto / CC / Vendedor</th>
                                    <th class="report-data">Cliente / Identidad</th>
                                    <th class="report-data">Marca / Modelo / Año / VIN</th>
                                    <th class="no-sort">Doc.</th>
                                    <th class="no-sort">Registro<br />
                                        Garantia</th>
                                    <th class="no-sort">Revisión<br />
                                        Física</th>
                                    <th class="no-sort estado-instalacion-gps">Instalación<br />
                                        GPS</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                            <tfoot></tfoot>
                        </table>
                    </div>
                </div>
                <!-- Listado garantías sin solicitudes -->
                <div class="tab-pane" id="tab_Listado_Garantias_SinSolicitud" role="tabpanel">
                    <div class="table-responsive mt-2">
                        <table id="datatable-garantiasSinSolicutd" class="table-bordered nowrap display compact table-condensed table-hover dataTable" style="width: 100%" role="grid">
                            <thead>
                                <tr>
                                    <th>Acciones</th>
                                    <th>Vendedor / CC</th>
                                    <th>Marca / Modelo / Año / VIN</th>
                                    <th>Tipo de Garantia / Tipo de vehículo</th>
                                    <th>Fecha de creación</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                            <tfoot></tfoot>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>


    <div id="modalPantallaCompleta" class="modal fade modal-fullscreen" tabindex="-1" role="dialog" aria-labelledby="modalPantallaCompletaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-body p-0">

                    <iframe class="w-100 h-100" id="iframe-fullscreen" src="Garantia_Detalles.aspx?hFuYL/3BFCvrmaPQ+Jn7raBWiZZzDdR1rCp9U4CzMiHir1Y3dsMSqh4guMZqUYhpV7PvRlbKYRqtyPc/mQeXow=="></iframe>

                </div>
                <div class="modal-footer pt-2 pb-2">
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                        Cerrar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="modalActualizarGarantia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalActualizarGarantiaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalActualizarGarantiaLabel">Actualizar garantía | Solicitud <span class="lblNoSolicitudCredito"></span></h6>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de actualizar la información de la garantía de esta solicitud?
                    <br />
                    <br />
                    Cliente: <span class="font-weight-bold lblNombreCliente"></span>
                </div>
                <div class="modal-footer">
                    <button type="button" id="btnActualizarGarantia_Confirmar" class="btn btn-primary waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="modalGuardarGarantia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalGuardarGarantiaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalGuardarGarantiaLabel">Guardar garantía | Solicitud <span class="lblNoSolicitudCredito"></span></h6>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de guardar la información de la garantía de esta solicitud?
                    <br />
                    <br />
                    Cliente: <span class="font-weight-bold lblNombreCliente"></span>
                </div>
                <div class="modal-footer">
                    <button type="button" id="btnGuardarGarantia_Confirmar" class="btn btn-primary waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="modalDetallesGarantia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDetallesGarantiaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalDetallesGarantiaLabel">Detalles de la garantía | Solicitud <span class="lblNoSolicitudCredito"></span></h6>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de redirigir a los detalles de esta garantía?
                    <br />
                    <br />
                    Cliente: <span class="font-weight-bold lblNombreCliente"></span>
                </div>
                <div class="modal-footer pt-2 pb-2">
                    <button type="button" id="btnDetallesGarantia_Confirmar" class="btn btn-primary waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="modalDetallesGarantia_SinSolicitud" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDetallesGarantia_SinSolicitudLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalDetallesGarantia_SinSolicitudLabel">Detalles de la garantía</h6>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de redirigir a los detalles de esta garantía?
                </div>
                <div class="modal-footer pt-2 pb-2">
                    <button type="button" id="btnDetallesGarantia_SinSolicitud_Confirmar" class="btn btn-primary waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="modalActualizarGarantia_SinSolicitud" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalActualizarGarantia_SinSolicitudLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalActualizarGarantia_SinSolicitudLabel">Actualizar garantía</h6>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de actualizar la información de la garantía de esta solicitud?
                </div>
                <div class="modal-footer pt-2 pb-2">
                    <button type="button" id="btnActualizarGarantia_SinSolicitud_Confirmar" class="btn btn-primary waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="modalImprimirDocumentacion" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalImprimirDocumentacionLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalImprimirDocumentacionLabel">Imprimir documentación | Solicitud <span class="lblNoSolicitudCredito"></span></h6>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de redirigir a la impresión de documentación?
                    <br />
                    <br />
                    Cliente: <span class="font-weight-bold lblNombreCliente"></span>
                </div>
                <div class="modal-footer pt-2 pb-2">
                    <button type="button" id="btnImprimirDocumentacion_Confirmar" class="btn btn-primary waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>


    <form runat="server" id="frmPrincipal" data-parsley-excluded="[disabled]">

        <div id="modalSolicitarGPS" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalSolicitarGPSLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalSolicitarGPSLabel">Solicitar Revisión/GPS | Solicitud <span class="lblNoSolicitudCredito"></span></h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-form-label">VIN</label>
                            <input id="txtVIN_SolicitarGPS" type="text" class="form-control form-control-sm col-form-label" data-parsley-group="InstalacionGPS_Guardar" readonly="readonly" />
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <div class="form-group">
                                    <label class="col-form-label">Ubicación</label>
                                    <asp:DropDownList runat="server" ID="ddlUbicacionInstalacion" class="form-control form-control-sm col-form-label" data-parsley-group="InstalacionGPS_Guardar" required="required"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="form-group">
                                    <label class="col-form-label">Fecha de instalación</label>
                                    <input type="datetime-local" class="form-control form-control-sm" name="txtFechaInstalacion" id="txtFechaInstalacion" data-parsley-group="InstalacionGPS_Guardar" />
                                </div>
                            </div>
                            <div class="col-12">
                                <div class="form-group">
                                    <label class="col-form-label">Comentario</label>
                                    <textarea id="txtComentario" runat="server" class="form-control form-control-sm" data-parsley-group="InstalacionGPS_Guardar" data-parsley-maxlength="300" rows="2"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer pt-2 pb-2">
                        <button type="button" id="btnSolicitarGPS_Confirmar" class="btn btn-primary waves-effect waves-light">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalDetalleSolicitudGPS" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDetalleSolicitudGPSLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalDetalleSolicitudGPSLabel">Detalles de la solicitud Revisión/GPS | Solicitud <span id="lblIdSolicitudDetalleSolicitudGPS"></span></h6>
                        <span id="lblEstadoSolicitudGPS_Detalle" class="btn btn-sm btn-warning float-right">Pendiente</span>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-form-label">VIN</label>
                            <input id="txtVIN_SolicitarGPS_Detalle" type="text" class="form-control form-control-sm col-form-label" data-parsley-group="InstalacionGPS_Detalle" readonly="readonly" />
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <div class="form-group">
                                    <label class="col-form-label">Ubicación</label>
                                    <asp:DropDownList runat="server" ID="ddlUbicacionInstalacion_Detalle" ReadOnly="True" class="form-control form-control-sm col-form-label" data-parsley-group="InstalacionGPS_Detalle" required="required"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="form-group">
                                    <label class="col-form-label">Fecha de instalación</label>
                                    <input type="datetime-local" class="form-control form-control-sm" name="txtFechaInstalacion_Detalle" id="txtFechaInstalacion_Detalle" readonly="readonly" data-parsley-group="InstalacionGPS_Detalle" />
                                </div>
                            </div>
                            <div class="col-12">
                                <div class="form-group">
                                    <label class="col-form-label">Comentario</label>
                                    <textarea id="txtComentario_Detalle" runat="server" class="form-control form-control-sm" readonly="readonly" data-parsley-group="InstalacionGPS_Detalle" data-parsley-maxlength="300" rows="2"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer pt-2 pb-2">
                        <button type="button" id="btnActualizarSolicitudGPS" class="btn btn-primary waves-effect waves-light">
                            Actualizar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalActualizarSolicitudGPS" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalActualizarSolicitudGPSLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalActualizarSolicitudGPSLabel">Actualizar solicitud Revisión/GPS | Solicitud <span id="lblIdSolicitudActualizarSolicitudGPS"></span></h6>
                        <span id="lblEstadoSolicitudGPS_Actualizar" class="btn btn-sm btn-warning float-right">Pendiente</span>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-form-label">VIN</label>
                            <input id="txtVIN_SolicitarGPS_Actualizar" type="text" class="form-control form-control-sm col-form-label" data-parsley-group="InstalacionGPS_Actualizar" readonly="readonly" />
                        </div>
                        <div class="row">
                            <div class="col-6">
                                <div class="form-group">
                                    <label class="col-form-label">Ubicación</label>
                                    <asp:DropDownList runat="server" ID="ddlUbicacionInstalacion_Actualizar" class="form-control form-control-sm col-form-label" data-parsley-group="InstalacionGPS_Actualizar" required="required"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-6">
                                <div class="form-group">
                                    <label class="col-form-label">Fecha de instalación</label>
                                    <input type="datetime-local" class="form-control form-control-sm" name="txtFechaInstalacion_Actualizar" id="txtFechaInstalacion_Actualizar" data-parsley-group="InstalacionGPS_Actualizar" />
                                </div>
                            </div>
                            <div class="col-12">
                                <div class="form-group">
                                    <label class="col-form-label">Comentario</label>
                                    <textarea id="txtComentario_Actualizar" runat="server" class="form-control form-control-sm" data-parsley-group="InstalacionGPS_Actualizar" data-parsley-maxlength="300" rows="2"></textarea>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer pt-2 pb-2">
                        <button type="button" id="btnActualizarSolicitudGPS_Confirmar" class="btn btn-primary waves-effect waves-light">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalRevisionesGarantia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalDetalleSolicitudGPSLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalRevisionesGarantiaLabel">Revisión física de la garantía | Solicitud de crédito N° <b class="lblNoSolicitudCredito"></b></h6>
                        <div id="lblEstadoRevisionFisica" class="badge p-2 float-right"></div>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-sm-3">
                                <label class="col-form-label">VIN</label>
                                <asp:TextBox ID="txtVIN" CssClass="form-control form-control-sm txtVIN" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Marca</label>
                                <asp:TextBox ID="txtMarca" CssClass="form-control form-control-sm txtMarca" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Modelo</label>
                                <asp:TextBox ID="txtModelo" CssClass="form-control form-control-sm txtModelo" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Año</label>
                                <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm txtAnio" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <h6 class="border-top border-gray pt-2">Listado de revisiones</h6>
                        <div id="accordion-revisiones"></div>
                    </div>
                    <div class="modal-footer pt-2 pb-2">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal detalles de la instalación de GPS -->
        <div id="modalDetallesInstalacionGPS" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalDetallesInstalacionGPSLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalDetallesInstalacionGPSLabel">Detalles de la instalación GPS | Solicitud de crédito N° <b class="lblNoSolicitudCredito"></b></h6>
                            </div>
                            <div class="col-12 text-muted">
                                Marca:
                                <span class="lblMarca"></span>
                                / Modelo:
                                <span class="lblModelo"></span>
                                / Año:
                                <span class="lblAnio"></span>
                                / Color:
                                <span class="lblColor"></span>
                                / VIN:
                                <span class="lblVIN"></span>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="row mb-0">
                            <div class="col-12">
                                <div class="form-group row">
                                    <div class="col-12">
                                        <label class="col-form-label d-block mb-0 pb-0">Información del GPS</label>
                                        <label class="col-form-label pt-0">
                                            IMEI: <span id="lblIMEI" class="font-weight-bold"></span>/
                                            Serie: <span id="lblSerie" class="font-weight-bold"></span>/
                                            Modelo: <span id="lblModelo" class="font-weight-bold"></span>/
                                            Compañía: <span id="lblCompania" class="font-weight-bold"></span>/
                                            <span id="lblRelay" class="font-weight-bold"></span>
                                        </label>
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
                                        <!-- Div donde se muestran las imágenes de la instalacion de GPS -->
                                        <div class="align-self-center" id="divFotosInstalacionGPS" runat="server" style="display: none;"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- modal de documentos de la garantía -->
        <div id="modalDocumentosDeLaGarantia" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalDocumentosDeLaGarantiaLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalDocumentosDeLaGarantiaLabel">Fotografías de la garantía | Solicitud de crédito N° <b class="lblNoSolicitudCredito"></b></h6>
                            </div>
                            <div class="col-12 text-muted">
                                Marca:
                                <span class="lblMarca"></span>
                                / Modelo:
                                <span class="lblModelo"></span>
                                / Año:
                                <span class="lblAnio"></span>
                                / Color:
                                <span class="lblColor"></span>
                                / VIN:
                                <span class="lblVIN"></span>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="row mb-0">
                            <div class="col-12">
                                <div class="form-group row">
                                    <div class="col-12">
                                        <!-- Div donde se muestran las fotografías de la garantía -->
                                        <div class="align-self-center" id="divGaleriaGarantia" runat="server" style="display: none;"></div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <!-- datatable js -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <!-- Buttons -->
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="/Scripts/plugins/datatables/pdfmake.min.js"></script>
    <script src="/Scripts/plugins/datatables/vfs_fonts.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.print.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.colVis.min.js"></script>
    <!-- Responsive -->
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/datatables/responsive.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/tilesgrid/ug-theme-tilesgrid.js"></script>
    <script>
        const NOMBRE_USUARIO = '<%=pcNombreUsuario %>';
        const CORREO_USUARIO = '<%=pcBuzoCorreoUsuario%>';
    </script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_ListadoGarantias.js?v=20210203031652"></script>
</body>
</html>
