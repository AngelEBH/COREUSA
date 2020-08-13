<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SeguimientoSupervisorPromesasdePago.aspx.cs" Inherits="SeguimientoSupervisorPromesasdePago" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <!-- BOOTSTRAP -->
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
</head>
<body runat="server" class="EstiloBody-Listado-W1100px">
    <div class="card">
        <div class="card-header">
            <h6>Seguimiento Promesas de Pago</h6>
        </div>
        <div class="card-body">
            <div class="table-responsive p-0">
                <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                    <label class="btn btn-secondary active">
                        <input id="general" type="radio" name="filtros" value="0" />
                        General
                    </label>
                    <label class="btn btn-danger">
                        <input id="campo" type="radio" name="filtros" value="incumplidas" />
                        <a href="#" class="text-white">Incumplidas</a>
                    </label>
                    <label class="btn btn-success">
                        <input id="recepcion" type="radio" name="filtros" value="hoy" />
                        <a href="#" class="text-white">Para Hoy</a>
                    </label>
                    <label class="btn btn-info">
                        <input id="analisis" type="radio" name="filtros" value="futuras" />
                        <a href="#" class="text-white">Futuras</a>
                    </label>

                </div>
            </div>
            <br />
            <table id="datatable-clientes" class="display compact table-striped table-bordered nowrap table-condensed" style="width: 100%">
                <thead>
                    <tr>
                        <th>Agente &nbsp;</th>
                        <th>ID Cliente</th>
                        <th>Nombre</th>
                        <th>Atraso</th>
                        <th>Dias Mora</th>
                        <th>Registrado</th>
                        <th>Promesa</th>
                        <th>Estado</th>
                    </tr>
                </thead>
                <tbody></tbody>
                <tfoot></tfoot>
            </table>
        </div>
    </div>
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
    <script src="/Scripts/app/SRC/Seguimientos/SeguimientoSupervisorPromesasdePago.js"></script>
</body>
</html>