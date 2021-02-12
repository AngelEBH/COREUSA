<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_ListadoExpedientes.aspx.cs" Inherits="SolicitudesCredito_ListadoExpedientes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Expedientes</title>
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
    </style>
</head>
<body runat="server" class="EstiloBody-Listado">

    <div class="card">
        <div class="card-header">
            <div class="row">
                <div class="col-8 pr-0">
                    <h6>Expedientes de solicitudes de crédito
                        <small runat="server" id="lblMensajeError" class="text-danger"></small>
                    </h6>
                </div>
                <div class="col-4 pl-0">
                    <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar" aria-label="Buscar" />
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="row">
                <div class="col-4">
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
                <div class="col-4">
                    <label class="col-form-label">Búsqueda por Fecha</label>
                    <div class="input-daterange input-group" id="date-range">
                        <input type="text" class="form-control form-control-sm" name="min" id="min" />
                        <input type="text" class="form-control form-control-sm" name="max" id="max" />
                    </div>
                </div>
                <div class="col-4 align-self-end mt-2">
                    <div class="table-responsive">
                        <div class="btn-group btn-block btn-group-toggle" data-toggle="buttons">
                            <label class="btn btn-secondary active opcion">
                                <input type="radio" name="filtro-estado-expedientes" value="0" />
                                General
                            </label>
                            <label class="btn btn-secondary opcion">
                                <input type="radio" name="filtro-estado-expedientes" value="1" />
                                Pendientes
                            </label>
                            <label class="btn btn-secondary opcion">
                                <input type="radio" name="filtro-estado-expedientes" value="2" />
                                Revisadas
                            </label>
                        </div>
                    </div>
                </div>

                <div class="col-12 mt-3">
                    <table id="datatable-listado-expedientes" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                        <thead>
                            <tr>
                                <th class="no-sort">Acciones</th>
                                <th class="report-data">Solicitud N°<br />
                                    Fecha de ingreso</th>
                                <th class="report-data">Producto / CC / Vendedor</th>
                                <th class="report-data">Cliente / Identidad</th>
                                <th class="report-data">Marca / Modelo / Año / VIN</th>
                                <th class="report-data no-sort">Docs. <br /> Expediente</th>
                                <th class="report-data">Estado<br />
                                    Expediente</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                        <tfoot></tfoot>
                    </table>
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
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_ListadoExpedientes.js?v=20210212100652"></script>
</body>
</html>
