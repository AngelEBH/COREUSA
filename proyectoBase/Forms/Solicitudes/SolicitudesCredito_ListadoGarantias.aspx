<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_ListadoGarantias.aspx.cs" Inherits="SolicitudesCredito_ListadoGarantias" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Garantías de solicitudes aprobadas</title>
    <!-- BOOTSTRAP -->
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .opcion {
            cursor: pointer;
        }

        #datatable-listado tbody tr {
            cursor: pointer;
        }

        #datatable-listado tbody td {
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
                <div class="col-8">
                    <h6>Garantias de solicitudes aprobadas</h6>
                </div>
                <div class="col-4">
                    <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar"
                        aria-label="Buscar" />
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="row mb-0">
                <div class="col-md-12">
                    <div class="form-group row">
                        <div class="col-lg-2 col-sm-3 pr-0 align-self-end">
                            <button id="btnRegistrarGarantiaSinSolicitud" class="btn btn-block btn-warning pr-0 pl-0">Registrar sin solicitud</button>
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
                        <div class="col-lg-4 col-sm-3 align-self-end">
                            <div class="table-responsive">
                                <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                                    <label class="btn btn-secondary active opcion">
                                        <input id="general" type="radio" name="filtros" value="0" />
                                        General
                                    </label>
                                    <label class="btn btn-secondary opcion">
                                        <input id="recepcion" type="radio" name="filtros" value="1" />
                                        Pendientes
                                    </label>
                                    <label class="btn btn-secondary opcion">
                                        <input id="analisis" type="radio" name="filtros" value="2" />
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
                        <table id="datatable-listado" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                            <thead>
                                <tr>
                                    <th>No</th>
                                    <th>Agencia</th>
                                    <th>Vendedor</th>
                                    <th>Prod.</th>
                                    <th>Identidad</th>
                                    <th>Nombre cliente</th>
                                    <th>Fecha de ingreso</th>
                                    <th>Estado</th>
                                    <th>Acciones</th>
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
                        <table id="datatable-garantiasSinSolicutd" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                            <thead>
                                <tr>
                                    <th>Agencia</th>
                                    <th>Vendedor</th>
                                    <th>VIN</th>
                                    <th>T. Garantia</th>
                                    <th>T. Vehiculo</th>
                                    <th>Fecha creación</th>
                                    <th>Acciones</th>
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

    <div id="modalActualizarGarantia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalActualizarGarantiaLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalActualizarGarantiaLabel">Actualizar garantía</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de actualizar la información de la garantía de esta solicitud?
                    <br />
                    Solicitud No: <span id="lblIdSolicitudActualizar"></span>. Cliente: <span id="lblNombreClienteActualizar"></span>
                </div>
                <div class="modal-footer">
                    <button id="btnActualizarGarantia" class="btn btn-primary waves-effect waves-light">
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
                    <h5 class="modal-title mt-0" id="modalGuardarGarantiaLabel">Guardar garantía</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de guardar la información de la garantía de esta solicitud?
                    <br />
                    Solicitud No: <span id="lblIdSolicitudGuardar"></span>. Cliente: <span id="lblNombreClienteGuardar"></span>
                </div>
                <div class="modal-footer">
                    <button id="btnGuardarGarantia" class="btn btn-primary waves-effect waves-light">
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
                    <h5 class="modal-title mt-0" id="modalDetallesGarantiaLabel">Detalles de la garantía</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de redirigir a los detalles de esta garantía?
                    <br />
                    Solicitud No: <span id="lblIdSolicitudDetalles"></span>. Cliente: <span id="lblNombreClienteDetalles"></span>
                </div>
                <div class="modal-footer">
                    <button id="btnDetallesGarantia" class="btn btn-primary waves-effect waves-light">
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
                    <h5 class="modal-title mt-0" id="modalDetallesGarantia_SinSolicitudLabel">Detalles de la garantía</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de redirigir a los detalles de esta garantía?
                </div>
                <div class="modal-footer">
                    <button id="btnDetallesGarantia_SinSolicitud" class="btn btn-primary waves-effect waves-light">
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
                    <h5 class="modal-title mt-0" id="modalActualizarGarantia_SinSolicitudLabel">Actualizar garantía</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de actualizar la información de la garantía de esta solicitud?
                </div>
                <div class="modal-footer">
                    <button id="btnActualizarGarantia_SinSolicitud" class="btn btn-primary waves-effect waves-light">
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
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_ListadoGarantias.js?v=20201022174525"></script>
</body>
</html>
