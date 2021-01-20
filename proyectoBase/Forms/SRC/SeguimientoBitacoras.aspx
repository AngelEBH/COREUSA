<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoBitacoras.aspx.cs" Inherits="SeguimientoBitacoras" %>

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
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .opcion {
            cursor: pointer;
        }

        #datatable-clientes tbody tr {
            cursor: pointer;
        }

        #datatable-clientes tbody td {
            outline: none;
        }
    </style>
</head>
<body class="EstiloBody-Listado">
    <form runat="server">
        <div class="card">
            <div class="card-header">
                <div class="row">
                    <div class="col-8">
                        <h6>Seguimiento de bitacoras</h6>
                    </div>
                    <div class="col-4">
                        <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar" aria-label="Buscar" />
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div class="form-group form-row">
                    <div class="col">
                        <div class="form-group row m-0">
                            <div class="col-sm-3">
                                <label class="col-form-label">Filtrar por Agente</label>
                            </div>
                            <div class="col-sm-3">
                                <asp:DropDownList ID="ddlAgentesActivos" runat="server" required="required" class="form-control form-control-sm pl-0"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                <label class="col-form-label">Búsqueda por Fecha</label>
                            </div>
                            <div class="col-sm-3">
                                <input type="text" class="form-control form-control-sm" name="min" id="min" />
                            </div>
                        </div>
                    </div>
                    <div class="col align-self-end text-right" id="btnFilter-container">
                    </div>
                </div>
                <table id="datatable-clientes" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                    <thead>
                        <tr>
                            <th>Agente</th>
                            <th>ID Cliente</th>
                            <th>Nombre</th>
                            <th>Teléfono</th>
                            <th>Comentario 1</th>
                            <th>Comentario 2</th>
                            <th>Inicio Llamada</th>
                            <th>Fin Llamada</th>
                            <th>Duración</th>
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
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/app/SRC/Seguimientos/SeguimientoBitacoras.js?v=1.1"></script>
</body>
</html>
