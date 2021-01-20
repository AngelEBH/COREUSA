<%@ Page Language="C#" AutoEventWireup="true" Inherits="Clientes_BandejaPrecalificados" CodeFile="BandejaPrecalificados.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <style>
        html {
            background-color: rgb(255,255,255) !important;
        }

        .card {
            border: none;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }

        .opcion {
            cursor: pointer;
        }

        #datatable-precalificados tbody tr {
            cursor: pointer;
        }

        #datatable-precalificados tbody td {
            outline: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="card mb-0">
            <div class="card-body">
                <div class="form-group">
                    <input id="txtDatatableFilter" class="form-control form-control-sm" type="text" placeholder="Buscar" aria-label="Buscar" />
                </div>
                <div class="table-responsive">
                    <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                        <label class="btn btn-secondary active opcion">
                            <input id="general" type="radio" name="filtros" value="0" />
                            <small>Todos</small>
                        </label>
                        <label class="btn btn-secondary opcion">
                            <input id="preAprobados" type="radio" name="filtros" value="1" />
                            <small>Pre-Aprobados</small>
                        </label>
                        <label class="btn btn-secondary opcion">
                            <input id="rechazados" type="radio" name="filtros" value="2" />
                            <small>Rechazados</small>
                        </label>
                        <label class="btn btn-secondary opcion">
                            <input id="analisis" type="radio" name="filtros" value="3" />
                            <small>Analisis</small>
                        </label>

                    </div>
                </div>
                <table id="datatable-precalificados" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                    <thead>
                        <tr>
                            <th></th>
                            <th>Cliente</th>
                            <th>Identidad</th>
                            <th>Ingresos</th>
                            <th>Telefono</th>
                            <th>Producto</th>
                            <th>Consultado</th>
                            <th>Oficial</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                    <tfoot></tfoot>
                </table>
            </div>
        </div>
    </form>

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <!-- datatable js -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <!-- Responsive -->
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/datatables/responsive.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/app/CoreMovil/Clientes_BandejaPrecalificados.js?20210120123485"></script>
</body>
</html>

