<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreciosDeMercado_Listado.aspx.cs" Inherits="PreciosDeMercado_Listado" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Precios de mercado</title>
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
    <link href="/Content/css/bootstrap4-modal-fullscreen.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .opcion {
            cursor: pointer;
        }

        .cursor-zoom-in {
            cursor: zoom-in;
        }

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
                    <h6>Precios de mercado
                        <small runat="server" id="lblMensajeError" class="text-danger"></small>
                    </h6>
                </div>
                <div class="col-4 pl-0">
                    <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar" aria-label="Buscar" />
                </div>
            </div>
        </div>
        <div class="card-body">

            <!-- Nav tabs -->
            <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs">
                <li class="nav-item">
                    <a class="nav-link active" data-toggle="tab" href="#tab_listado_precios_de_mercado" id="tab_listado_precios_de_mercado_link" role="tab" aria-selected="false">
                        <span class="d-block d-sm-none">Precios
                            <br />
                            de mercado</span>
                        <span class="d-none d-sm-block">Precios de mercado</span>
                    </a>
                </li>
                <li class="nav-item">
                    <a class="nav-link" data-toggle="tab" href="#tab_listado_devaluacion" id="tab_listado_devaluacion_link" role="tab" aria-selected="false">
                        <span class="d-block d-sm-none">Historial
                            <br />
                            de precios</span>
                        <span class="d-none d-sm-block">Historial de precios de mercado</span>
                    </a>
                </li>
            </ul>
            <!-- Tab panes -->
            <div class="tab-content" id="tabContent">
                <div class="tab-pane active" id="tab_listado_precios_de_mercado" role="tabpanel">
                    <div class="table-responsive mt-2">
                        <table id="datatable-listado-precios-de-mercado" class="table-bordered display compact nowrap table-sm table-hover dataTable" style="width: 100%" role="grid">
                            <thead>
                                <tr>
                                    <th class="no-sort">Acciones</th>
                                    <th class="report-data">Marca</th>
                                    <th class="report-data">Modelo</th>
                                    <th class="report-data">Version</th>
                                    <th class="report-data">Año</th>
                                    <th class="report-data">Precio mercado</th>
                                    <th class="report-data">Fecha inicio</th>
                                    <th class="report-data">Fecha fin</th>
                                    <th class="report-data">Última devaluación</th>
                                    <th class="report-data">Estado</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                            <tfoot></tfoot>
                        </table>
                    </div>
                </div>
                <!-- Listado garantías sin solicitudes -->
                <div class="tab-pane" id="tab_listado_devaluacion" role="tabpanel">

                    <div class="form-group mt-3 text-right">
                        <button class="btn btn-info float-right">Nuevo</button>
                    </div>

                    <div class="table-responsive mt-2">
                        <table id="datatable-listado-devaluacion" class="table-bordered nowrap display compact table-sm table-hover dataTable" style="width: 100%" role="grid">
                            <thead>
                                <tr>
                                    <%--<th><span class="mr-3">Acciones</span></th>--%>
                                    <th><span class="mr-3 not-sort">Marca</span></th>
                                    <th><span class="mr-3 not-sort">Modelo</span></th>
                                    <th><span class="mr-3 not-sort">Version</span></th>
                                    <th><span class="mr-3 not-sort">Años disponibles</span></th>
                                    <th></th>
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

    <!-- modal de documentos de la garantía -->
    <div id="modalHistorialDePrecios" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalHistorialDePreciosLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header pb-2">
                    <div class="form-group row mb-0">
                        <div class="col-12">
                            <h6 class="m-0" id="modalHistorialDePreciosLabel">Historial de precios</h6>
                        </div>
                        <div class="col-12 text-muted">
                            <span class="lblMarca"></span>
                            <span class="lblModelo"></span>
                            <span class="lblVersion"></span>
                            <span class="lblAnio"></span>
                        </div>
                    </div>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    <div class="row mb-0">
                        <div class="col-12">
                            <div class="table-responsive">
                                <table id="tblHistorialDePrecios" class="table-bordered display compact nowrap table-sm table-hover dataTable" style="width: 100%;">
                                    <thead>
                                        <tr>
                                            <td>Fecha de inicio</td>
                                            <td>Fecha de fin</td>
                                            <td>Precio de mercado</td>
                                            <td>Última devaluación</td>
                                            <td>Porcentaje de devaluación</td>
                                        </tr>
                                    </thead>
                                    <tbody></tbody>
                                </table>
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
    <script src="/Scripts/app/solicitudes/PreciosDeMercado_Listado.js?v=2021020903433562589"></script>
</body>
</html>
