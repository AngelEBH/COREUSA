<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesGPS_Listado.aspx.cs" Inherits="SolicitudesGPS_Listado" %>

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
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <style>
        html {
            background-color: rgb(255,255,255) !important;
        }

        .opcion {
            cursor: pointer;
        }

        .dataTable tbody tr {
            cursor: pointer;
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
<body runat="server">
    <div class="card mb-0">
        <div class="card-header">
            <div class="row justify-content-between">
                <div class="col-sm-8 col-6">
                    <h6>Solicitudes de GPS</h6>
                </div>
                <div class="col-sm-4 col-6">
                    <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar" aria-label="Buscar" />
                </div>
                <div class="col-12">
                    <asp:Label runat="server" ID="lblMensajeError" class="text-danger"></asp:Label>
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="row mb-0">
                <div class="col-md-12">
                    <div class="form-group row">
                        <div class="col-lg-6 col-sm-6 col-12">
                            <label class="col-form-label">Búsqueda por Fecha</label>
                            <div class="input-daterange input-group" id="date-range">
                                <input type="text" class="form-control form-control-sm" name="min" id="min" />
                                <input type="text" class="form-control form-control-sm" name="max" id="max" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Nav tabs -->
            <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs">
                <li class="nav-item">
                    <a class="nav-link active" data-toggle="tab" href="#tab_Listado_SolicitudesGPS_Pendientes" id="tab_Listado_SolicitudesGPS_Pendientes_Link" role="tab" aria-selected="false">
                        <span class="d-block d-sm-none">Pendientes</span>
                        <span class="d-none d-sm-block">Solicitudes GPS Pendientes</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" data-toggle="tab" href="#tab_Listado_SolicitudesGPS_Completadas" id="tab_Listado_SolicitudesGPS_Completadas_Link" role="tab" aria-selected="false">
                        <span class="d-block d-sm-none">Completadas</span>
                        <span class="d-none d-sm-block">Solicitudes GPS completadas</span>
                    </a>
                </li>
            </ul>
            <!-- Tab panes -->
            <div class="tab-content" id="tabContent">
                <div class="tab-pane active" id="tab_Listado_SolicitudesGPS_Pendientes" role="tabpanel">
                    <table id="datatable_SolicitudesGPS_Pendientes" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Acciones</th>
                                <th>Garantía/Revisiones</th>
                                <th>Revisiones</th>
                                <th>Cliente</th>
                                <th>creado por</th>
                                <th>F. creado</th>
                                <th>F. instalación</th>
                                <th>Instalacion GPS</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                        <tfoot></tfoot>
                    </table>
                </div>
                <!-- Listado garantías sin solicitudes -->
                <div class="tab-pane" id="tab_Listado_SolicitudesGPS_Completadas" role="tabpanel">
                    <table id="datatable_SolicitudesGPS_Completadas" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                        <thead>
                            <tr>
                                <th></th>
                                <th>Acciones</th>
                                <th>Garantía/Revisiones</th>
                                <th>Revisiones</th>
                                <th>Cliente</th>
                                <th>creado por</th>
                                <th>F. creado</th>
                                <th>F. instalación</th>
                                <th>Instalacion GPS</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                        <tfoot></tfoot>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <div id="modalCompletarSolicitudGPS" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalCompletarSolicitudGPSLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalCompletarSolicitudGPSLabel">Revisar garantía</h6>
                </div>
                <div class="modal-body">
                    <div class="form-group row">
                        <div class="col-12 mb-2">
                            <label>Cliente</label>
                            <input class="txtNombreCliente form-control form-control-sm" readonly="readonly" />
                        </div>
                        <div class="col-6 mb-2 pr-0">
                            <label>Marca</label>
                            <input class="txtMarca form-control form-control-sm" readonly="readonly" />
                        </div>
                        <div class="col-6 mb-2">
                            <label>Modelo</label>
                            <input class="txtModelo form-control form-control-sm" readonly="readonly" />
                        </div>
                        <div class="col-6 mb-2 pr-0">
                            <label>Año</label>
                            <input class="txtAnio form-control form-control-sm" readonly="readonly" />
                        </div>
                        <div class="col-6 mb-2">
                            <label>Revisiones</label>
                            <input class="txtRevisionesGarantia form-control form-control-sm" readonly="readonly" value="0/3" />
                        </div>
                        <div class="col-12 mb-0">
                            <label>Comentarios de la solicitud de GPS</label>
                            <textarea class="txtComentarioSolicitudGPS form-control form-control-sm" readonly="readonly"></textarea>
                        </div>
                    </div>
                </div>
                <div class="modal-footer pt-2 pb-2">
                    <button type="button" id="btnCompletarSolicitud_Confirmar" class="btn btn-info waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <div id="modalInstalarGPS" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalInstalarGPSLabel" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header">
                    <h6 class="modal-title mt-0" id="modalInstalarGPSLabel"><b>Registro de instalación de GPS</b></h6>
                </div>
                <div class="modal-body">
                    <div class="form-group row">
                        <div class="col-12 mb-2">
                            <label>Cliente</label>
                            <input class="txtNombreCliente form-control form-control-sm" readonly="readonly" />
                        </div>
                        <div class="col-6 mb-2 pr-0">
                            <label>Marca</label>
                            <input class="txtMarca form-control form-control-sm" readonly="readonly" />
                        </div>
                        <div class="col-6 mb-2">
                            <label>Modelo</label>
                            <input class="txtModelo form-control form-control-sm" readonly="readonly" />
                        </div>
                        <div class="col-6 mb-2 pr-0">
                            <label>Año</label>
                            <input class="txtAnio form-control form-control-sm" readonly="readonly" />
                        </div>
                        <div class="col-6 mb-2">
                            <label>Revisiones</label>
                            <input class="txtRevisionesGarantia form-control form-control-sm" readonly="readonly" value="0/3" />
                        </div>
                        <div class="col-12 mb-0">
                            <label>Comentarios de la solicitud de GPS</label>
                            <textarea class="txtComentarioSolicitudGPS form-control form-control-sm" readonly="readonly"></textarea>
                        </div>
                    </div>
                </div>
                <div class="modal-footer pt-2 pb-2">
                    <button type="button" id="btnInstalarGPS_Confirmar" class="btn btn-info waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
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
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/app/SolicitudesGPS/SolicitudesGPS_Listado.js?v=20210116095835"></script>
</body>
</html>
