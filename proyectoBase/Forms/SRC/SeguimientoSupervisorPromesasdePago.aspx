<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SeguimientoSupervisorPromesasdePago.aspx.cs" Inherits="SeguimientoSupervisorPromesasdePago" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
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
<body runat="server" class="EstiloBody-Listado-W1100px">
    <div class="card">
        <div class="card-header">
            <div class="row">
                <div class="col-8">
                    <h6>Seguimiento Promesas de Pago</h6>
                </div>
                <div class="col-4">
                    <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar" aria-label="Buscar" />
                </div>
            </div>
        </div>
        <div class="card-body">
            <div class="table-responsive p-0 mb-2">
                <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                    <label class="btn btn-danger active opcion">
                        <input id="campo" type="radio" name="filtros" value="incumplidas" />
                        Incumplidas
                    </label>
                    <label class="btn btn-success opcion">
                        <input id="recepcion" type="radio" name="filtros" value="hoy" />
                        Para Hoy
                    </label>
                    <label class="btn btn-info opcion">
                        <input id="analisis" type="radio" name="filtros" value="futuras" />
                        Futuras
                    </label>
                </div>
            </div>
            <table id="datatable-clientes" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%">
                <thead>
                    <tr>
                        <th>Agente</th>
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
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/app/SRC/Seguimientos/SeguimientoSupervisorPromesasdePago.js?v=1.1"></script>
</body>
</html>
