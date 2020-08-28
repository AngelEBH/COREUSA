<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_ListaMantenimiento.aspx.cs" Inherits="SolicitudesCredito_ListaMantenimiento" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Solicitudes de crédito - Lista de mantenimiento</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>

        #datatable-listaMantenimiento tbody tr {
            cursor: pointer;
        }

        #datatable-listaMantenimiento tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }
    </style>
</head>
<body class="EstiloBody-Listado-W1100px">
    <div class="card">
        <div class="card-header">
            <div class="row">
                <div class="col-8">
                    <h6>Lista de mantenimiento</h6>
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
                    <label class="form-control-plaintext">Búsqueda por número de solicitud</label>
                </div>
                <div class="form-group mx-sm-3 mb-2">
                    <input id="NoSolicitud" class="form-control form-control-sm" type="text" />
                </div>
            </div>

            <table id="datatable-listaMantenimiento" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                <thead>
                    <tr>
                        <th>No.</th>
                        <th>Identidad</th>
                        <th>Nombre cliente</th>
                        <th>Fecha</th>
                        <th>Tipo de prestamo</th>
                        <th>Estado</th>
                        <th></th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
    </div>

    <!--modal de confirmacion de actualizar una solicitud condicionada -->
    <div id="modalActualizarSolicitud" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalActualizarSolicitudLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalActualizarSolicitudLabel">Actualizar solicitud de crédito</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    <label>¿Realizar mantenimiento de esta solicitud?</label>
                </div>
                <div class="modal-footer">
                    <button id="btnAbrirMantenimiento" class="btn btn-primary waves-effect waves-light">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
    </div>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    
    <!-- datatable js -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <!-- Responsive datatable -->
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/datatables/responsive.bootstrap4.min.js"></script>

    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/app/SolicitudesCredito/SolicitudesCredito_ListaMantenimiento.js"></script>
</body>
</html>
