<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Solicitudes_CANEX.aspx.cs" Inherits="Solicitudes_CANEX" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Solicitudes CANEX</title>
    <!-- BOOTSTRAP -->
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
</head>
<body class="EstiloBody-Listado">
    <form id="form1" runat="server">
        <div class="card">
            <div class="card-header">
                <h4>Solicitudes CANEX</h4>
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <label class="col-sm-2">Nombre cliente</label>
                    <div class="col-sm-4">
                        <input id="nombreCliente" class="form-control form-control-sm" type="text" />
                    </div>
                    <label class="col-sm-2">Identidad cliente</label>
                    <div class="col-sm-4">
                        <input id="identidadCliente" class="form-control form-control-sm" type="text" />
                    </div>
                </div>
                <div class="form-group row">
                    <label class="col-sm-2">Búsqueda por Mes</label>
                    <div class="col-sm-2">
                        <select id="mesIngreso" class="form-control form-control-sm">
                            <option value="" selected="selected">Seleccionar</option>
                            <option value="enero">Enero</option>
                            <option value="febrero">Febrero</option>
                            <option value="marzo">Marzo</option>
                            <option value="abril">Abril</option>
                            <option value="mayo">Mayo</option>
                            <option value="junio">Junio</option>
                            <option value="julio">Julio</option>
                            <option value="agosto">Agosto</option>
                            <option value="septiembre">Septiembre</option>
                            <option value="octubre">Octubre</option>
                            <option value="noviembre">Noviembre</option>
                            <option value="diciembre">Diciembre</option>
                        </select>
                    </div>
                    <label class="col-sm-2 col-form-label">Búsqueda por Año</label>
                    <div class="col-sm-1">
                        <input id="añoIngreso" class="form-control form-control-sm" type="text" />
                    </div>
                    <label class="col-sm-2 col-form-label">Búsqueda por Fecha</label>
                    <div class="col-sm-3">
                        <div class="input-daterange input-group" id="date-range">
                            <input type="text" class="form-control form-control-sm" name="min" id="min" />
                            <input type="text" class="form-control form-control-sm" name="max" id="max" />
                        </div>
                    </div>
                </div>
                <table id="tblSolicitudesCanex" class="display compact table-striped table-bordered nowrap table-condensed" style="width: 100%">
                    <thead>
                        <tr>
                            <th>Socio</th>
                            <th>No. Solicitud</th>
                            <th>Fecha de ingreso</th>
                            <th>Identidad</th>
                            <th>Nombre cliente</th>
                            <th>Producto</th>
                            <th>Valor global</th>
                            <th>Prima</th>
                            <th>Préstamo</th>                            
                            <th>Agencia</th>
                            <th>Usuario</th>
                            <th></th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
        </div>
    </form>
    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/js/metisMenu.min.js"></script>
    <script src="/Scripts/js/jquery.slimscroll.js"></script>
    <script src="/Scripts/js/waves.min.js"></script>
    <script src="/Scripts/plugins/jquery-sparkline/jquery.sparkline.min.js"></script>
    <script src="/Scripts/js/app.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <!-- DATATABLES -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/app/Solicitudes_CANEX/Solicitudes_CANEX.js"></script>
</body>
</html>