<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SeguimientoRecuperacionDelDiaPorSupervisor.aspx.cs" Inherits="SeguimientoRecuperacionDelDiaPorSupervisor" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .opcion {
            cursor: pointer;
        }
        .active {
            /*border-style: solid !important;
            border-width: 1px !important;
            border-color: black !important;*/
        }
        #datatable-recuperacion tbody tr {
            cursor: pointer;
        }
        #datatable-recuperacion tbody td {
            outline: none;
        }
    </style>
</head>
<body class="EstiloBody-Listado-W1100px">
    <form runat="server">
        <div class="card">
            <div class="card-header">
                <h6>Recuperación del Dia</h6>
            </div>
            <div class="card-body">
                <div class="form-group row justify-content-center">
                    <label class="col-sm-2 col-form-label pr-0">Filtrar por Agente</label>
                    <div class="col-sm-3 pl-0">
                        <asp:DropDownList ID="ddlAgentesActivos" runat="server" required="required" class="form-control form-control-sm pl-0">
                        </asp:DropDownList>
                    </div>

                    <label class="col-sm-2 col-form-label">Total Recuperado Hoy</label>
                    <label class="col-sm-3 col-form-label" id="lblTotalRecuperadoHoy">L 20,000.00</label>
                </div>
                <table id="datatable-recuperacion" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                    <thead>
                        <tr>
                            <th>Agente</th>
                            <th>ID Cliente</th>
                            <th>Nombre</th>
                            <th>Descripción</th>
                            <th>Días de Atraso</th>
                            <th>Saldo Inicial</th>
                            <th>Abonos de Hoy</th>
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
    <script src="/Scripts/js/metisMenu.min.js"></script>
    <script src="/Scripts/js/jquery.slimscroll.js"></script>
    <script src="/Scripts/js/waves.min.js"></script>
    <script src="/Scripts/plugins/jquery-sparkline/jquery.sparkline.min.js"></script>
    <script src="/Scripts/js/app.js"></script>
    <!-- DATATABLES -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/app/SRC/Seguimientos/SeguimientoRecuperacionDelDiaPorSupervisor.js"></script>
</body>
</html>