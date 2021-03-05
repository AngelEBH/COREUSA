<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_Bandeja.aspx.cs" Inherits="SolicitudesCredito_Bandeja" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <meta name="description" content="Bandeja de solicitudes de crédito" />
    <title>Bandeja de solicitudes</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css?v=202010031105" rel="stylesheet" />
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

        #datatable-bandeja tbody tr {
            cursor: pointer;
        }

        #datatable-bandeja tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }

        div.dt-buttons {
            position: relative;
            float: left;
        }

        #divContenedor_datatableButtons div .dropdown-menu {
            overflow-y: auto;
            max-height: 300px !important;
        }

        table.datatable-cells-responsive tbody td.td-responsive {
            max-width: 200px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

            table.datatable-cells-responsive tbody td.td-responsive:hover {
                overflow: visible;
                white-space: inherit;
            }
    </style>
</head>
<body runat="server" class="EstiloBody-Listado">
    <div class="card">
        <div class="card-header">
            <div class="row justify-content-between">
                <div class="col-auto">
                    <h6>Bandeja general de solicitudes</h6>
                </div>
                <div class="col-4">
                    <input id="txtDatatableFilter" class="float-right form-control" type="text" placeholder="Buscar" aria-label="Buscar" />
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="table-responsive p-0 mb-1">
                <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                    <label class="btn btn-secondary active opcion">
                        <input id="general" type="radio" name="filtros" value="0" />
                        General
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="recepcion" type="radio" name="filtros" value="7" />
                        En Recepción
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="analisis" type="radio" name="filtros" value="8" />
                        En Analisis
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="campo" type="radio" name="filtros" value="9" />
                        En Campo
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="condicionadas" type="radio" name="filtros" value="10" />
                        Condicionadas
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="reprogramadas" type="radio" name="filtros" value="11" />
                        Reprogramadas
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="validacion" type="radio" name="filtros" value="12" />
                        Validación
                    </label>
                    <label class="btn btn-warning opcion">
                        <input id="pendientes" type="radio" name="filtros" value="13" />
                        Pendientes
                    </label>
                    <label class="btn btn-success opcion">
                        <input id="aprobadas" type="radio" name="filtros" value="14" />
                        Aprobadas
                    </label>
                    <label class="btn btn-danger opcion">
                        <input id="rechazadas" type="radio" name="filtros" value="15" />
                        Rechazadas
                    </label>
                </div>
            </div>

            <div class="row mb-0">
                <div class="col-md-12">
                    <div class="form-group row mb-1">
                        <div class="col-lg-auto col-md-auto col-sm-auto col-6 pr-0 align-self-end">
                            <div id="divContenedor_datatableButtons"></div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-3 col-6 align-self-end">
                            <label class="col-form-label">Búsqueda por Mes</label>
                            <select id="mesIngreso" class="form-control form-control-sm">
                                <option value="">Seleccionar</option>
                                <option value="01">Enero</option>
                                <option value="02" selected="selected">Febrero</option>
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
                        <div class="col-lg-4 col-md-3 col-sm-3 col-6">
                            <label class="col-form-label">Búsqueda por Fecha</label>
                            <div class="input-daterange input-group" id="date-range">
                                <input type="text" class="form-control form-control-sm" name="min" id="min" />
                                <input type="text" class="form-control form-control-sm" name="max" id="max" />
                            </div>
                        </div>
                        <div class="col-lg-2 col-md-2 col-sm-3 col-6 align-self-end">
                            <label class="col-form-label">Búsqueda por Año</label>
                            <input id="añoIngreso" class="form-control form-control-sm" type="text" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="table-responsive">
                <table id="datatable-bandeja" class="table table-bordered table-sm table-hover dataTable datatable-cells-responsive" style="width: 100%" role="grid">
                    <thead>
                        <tr>
                            <th><span class="pt-2 pb-2">Acciones</span></th>
                            <th>Solicitud de crédito</th>
                            <th>Producto / CC / Vendedor</th>
                            <th>Cliente / Identidad</th>
                            <th>Marca / Modelo / Año / VIN</th>
                            <th>Ingreso</th>
                            <th>Recepc.</th>
                            <th>Analis.</th>
                            <th>Campo</th>
                            <th>Condic.</th>
                            <th>Reprog.</th>
                            <th>Valida.</th>
                            <th>Resolución</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                    <tfoot></tfoot>
                </table>
            </div>
        </div>
    </div>

    <div id="modalAbrirSolicitud" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalAbrirSolicitudLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalAbrirSolicitudLabel">Abrir solicitud</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    Abrir solicitud de crédito <strong id="lblNoSolicitud"></strong>
                    cliente <strong id="lblCliente"></strong>
                    con identidad <strong id="lblIdentidadCliente"></strong>
                </div>
                <div class="modal-footer">
                    <button id="btnAbrirDetalles" class="btn btn-primary waves-effect waves-light" type="button">
                        Detalles
                    </button>
                    <button id="btnAbrirAnalisis" class="btn btn-primary waves-effect waves-light" type="button" runat="server" visible="false">
                        Analizar
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
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Bandeja.js?v=20201221095485"></script>
</body>
</html>
