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
    <link href="/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>        
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
                <div class="row">
                    <div class="col-8">
                        <h6 class="">Recuperación del Dia</h6>
                    </div>                    
                    <div class="col-4">                      
                      <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar"
                        aria-label="Buscar"/>
                    </div>                    
                </div>
            </div>
            <div class="card-body">

                <div class="form-inline justify-content-center">
                    <div class="form-group mb-2">
                        <label class="form-control-plaintext">Filtrar por Agente</label>
                    </div>
                    <div class="form-group mx-sm-3 mb-2">
                        <asp:DropDownList ID="ddlAgentesActivos" runat="server" required="required" class="form-control w-auto">
                        </asp:DropDownList>
                    </div>

                    <div class="form-group mb-2">
                        <label class="form-control-plaintext">Total Recuperado Hoy</label>
                    </div>
                    <div class="form-group mx-sm-3 mb-2">
                        <label class="col-form-label" style="font-weight:bold;" id="lblTotalRecuperadoHoy"></label>
                    </div>
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
    <!-- DATATABLES -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <%--<script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>--%>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/app/SRC/Seguimientos/SeguimientoRecuperacionDelDiaPorSupervisor.js"></script>
</body>
</html>