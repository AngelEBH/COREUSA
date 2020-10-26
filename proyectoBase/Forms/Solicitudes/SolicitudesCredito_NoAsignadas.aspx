<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_NoAsignadas.aspx.cs" Inherits="SolicitudesCredito_NoAsignadas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Solicitudes no asignadas</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        #datatable-bandeja tbody tr {
            cursor: pointer;
        }

        #datatable-bandeja tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }
    </style>
</head>
<body class="EstiloBody-Listado">
    <form runat="server">
        <div class="card">
            <div class="card-header">
                <div class="row">
                    <div class="col-8">
                        <h6>Solicitudes en campo no asignadas</h6>
                    </div>
                    <div class="col-4">
                        <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar"
                            aria-label="Buscar" />
                    </div>
                </div>
            </div>
            <div class="card-body">

                <div class="row mb-0">
                    <div class="col-md-12">
                        <div class="form-group row">
                            <div class="col-lg-4 col-sm-4 align-self-end">
                                <label class="col-form-label">Búsqueda por Mes</label>
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
                            <div class="col-lg-4 col-sm-4">
                                <label class="col-form-label">Búsqueda por Año</label>
                                <input id="añoIngreso" class="form-control form-control-sm" type="text" />
                            </div>
                            <div class="col-lg-4 col-sm-4">
                                <label class="col-form-label">Búsqueda por Fecha</label>
                                <div class="input-daterange input-group" id="date-range">
                                    <input type="text" class="form-control form-control-sm" name="min" id="min" />
                                    <input type="text" class="form-control form-control-sm" name="max" id="max" />
                                </div>
                            </div>

                        </div>
                    </div>
                </div>

                <table id="datatable-listado" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                    <thead>
                        <tr>
                            <th>No</th>
                            <th>Agencia</th>
                            <th>Vendedor</th>
                            <th>Prod.</th>
                            <th>Identidad</th>
                            <th>Nombre cliente</th>
                            <th>Fecha de ingreso</th>
                            <th>Acciones</th>
                        </tr>
                    </thead>
                    <tbody></tbody>
                    <tfoot></tfoot>
                </table>
            </div>
        </div>

        <!--modal de confirmacion de actualizar una solicitud condicionada -->
        <div id="modalAsignarSolicitud" class="modal fade" role="dialog" aria-labelledby="modalAsignarSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title mt-0" id="modalAsignarSolicitudLabel">Asignar gestor de campo (Solicitud <span id="lblIdSolicitud"></span>)</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-form-label">Gestores de campo</label>
                            <asp:DropDownList ID="ddlGestores" runat="server" CssClass="form-control form-control-sm" required="required"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnAsignar_Confirmar" type="button" class="btn btn-primary waves-effect waves-light">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
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
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_NoAsignadas.js?V=202010241152"></script>
</body>
</html>
