<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SeguimientoSupervisorColadeLlamadas.aspx.cs" Inherits="SeguimientoSupervisorColadeLlamadas" %>

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
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
</head>
<body runat="server" class="EstiloBody-Listado">
    <form runat="server">
        <div class="card">
            <div class="card-header">
                <h6>Seguimiento Cola de Llamadas</h6>
            </div>
            <div class="card-body">
                <div class="table-responsive p-0">
                    <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                        <label class="btn btn-success">
                            <input id="recepcion" type="radio" name="filtros" value="hoy" />
                            <a href="#" class="text-white">Llamadas de Hoy</a>
                        </label>
                        <label class="btn btn-danger">
                            <input id="campo" type="radio" name="filtros" value="porHacer" />
                            <a href="#" class="text-white">LLamadas Por Hacer</a>
                        </label>
                        <label class="btn btn-info active">
                            <input id="analisis" type="radio" name="filtros" value="anteriores" />
                            <a href="#" class="text-white">Llamadas Días Anteriores</a>
                        </label>
                    </div>
                </div>
                <br />
                <div class="form-group row">
                    <label class="col-sm-2 col-form-label pr-0">Filtrar por Agente</label>
                    <div class="col-sm-3 pl-0">
                        <asp:DropDownList ID="ddlAgentesActivos" runat="server" required="required" class="form-control form-control-sm pl-0">
                        </asp:DropDownList>
                    </div>
                    <label class="col-sm-2 col-form-label RangoFechas" style="display: none;">Búsqueda por Fecha</label>
                    <div class="col-sm-3 RangoFechas" style="display: none;">
                        <div class="input-daterange input-group" id="date-range">
                            <input type="text" class="form-control form-control-sm" name="min" id="min" />
                            <input type="text" class="form-control form-control-sm" name="max" id="max" />
                        </div>
                    </div>
                </div>
                <table id="datatable-clientes" class="display compact table-striped table-bordered nowrap table-condensed" style="width: 100%">
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
    <script src="/Scripts/app/SRC/Seguimientos/SeguimientoSupervisorColadeLlamadas.js"></script>
</body>
</html>
