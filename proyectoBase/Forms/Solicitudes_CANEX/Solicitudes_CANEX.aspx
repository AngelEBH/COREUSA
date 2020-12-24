<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Solicitudes_CANEX.aspx.cs" Inherits="Solicitudes_CANEX" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Solicitudes CANEX</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        #datatable-solicitudesCanex tbody tr {
            cursor: pointer;
        }

        #datatable-solicitudesCanex tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }
    </style>
</head>
<body class="EstiloBody-Listado">
    <form id="form1" runat="server">
        <div class="card">
            <div class="card-header">
                <div class="row">
                    <div class="col-8">
                        <h6 class="">Solicitudes de canalex externos</h6>
                    </div>
                    <div class="col-4">
                        <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar"
                            aria-label="Buscar" />
                    </div>
                </div>
            </div>
            <div class="card-body">

                <div class="form-inline justify-content-center">
                    <div class="form-group mb-2">
                        <label class="form-control-plaintext">Búsqueda por Mes</label>
                    </div>
                    <div class="form-group mx-sm-3 mb-2">
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

                    <div class="form-group mb-2">
                        <label class="form-control-plaintext">Búsqueda por Año</label>
                    </div>
                    <div class="form-group mx-sm-3 mb-2">
                        <input id="añoIngreso" class="form-control form-control-sm" type="text" />
                    </div>

                    <div class="form-group mb-2">
                        <label class="form-control-plaintext">Búsqueda por Fecha</label>
                    </div>
                    <div class="form-group mx-sm-3 mb-2 col-sm-2">
                        <div class="input-daterange input-group" id="date-range">
                            <input type="text" class="form-control form-control-sm" name="min" id="min" />
                            <input type="text" class="form-control form-control-sm" name="max" id="max" />
                        </div>
                    </div>
                </div>

                <div class="table-responsive">
                    <table id="datatable-solicitudesCanex" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                        <thead>
                            <tr>
                                <th>Acciones</th>
                                <th>Socio</th>
                                <th>No</th>
                                <th>Fecha de ingreso</th>
                                <th>Identidad</th>
                                <th>Cliente</th>
                                <th>Producto</th>
                                <th>Valor global</th>
                                <th>Prima</th>
                                <th>Préstamo</th>
                                <th>Agencia</th>
                                <th>Usuario</th>
                                <th>Estado</th>                                
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </form>

    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <!-- DATATABLES -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/app/Solicitudes_CANEX/Solicitudes_CANEX.js?v=20201103155625"></script>
</body>
</html>
