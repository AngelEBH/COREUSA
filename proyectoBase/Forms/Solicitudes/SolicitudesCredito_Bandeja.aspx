<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Bandeja.aspx.cs" Inherits="SolicitudesCredito_Bandeja" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Bandeja de solicitudes</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/Content/css/bandejaSolicitudes.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        .opcion {
            cursor: pointer;
        }

        #datatable-bandeja tbody tr {
            cursor: pointer;
        }

        #datatable-bandeja tbody td {
            outline: none;
            padding:0 !important;
        }
    </style>
</head>
<body runat="server" class="EstiloBody-Listado-PRUEBA">
    <div class="card">
        <div class="card-header">
            <div class="row">
                <div class="col-8">
                    <h6>Bandeja general de solicitudes</h6>
                </div>
                <div class="col-4">
                    <input id="txtDatatableFilter" class="float-right form-control w-75" type="text" placeholder="Buscar"
                        aria-label="Buscar" />
                </div>
            </div>  
        </div>
        <div class="card-body">
            <div class="table-responsive p-0">
                <div class="btn-group btn-group-toggle col-lg-12 p-0" data-toggle="buttons">
                    <label class="btn btn-secondary active opcion">
                        <input id="general" type="radio" name="filtros" value="0" />
                        General
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="recepcion" type="radio" name="filtros" value="7" />
                        En Recepción
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="analisis" type="radio" name="filtros" value="8" />
                        En Analisis
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="campo" type="radio" name="filtros" value="9" />
                        En Campo
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="condicionadas" type="radio" name="filtros" value="10" />
                        Condicionadas
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="reprogramadas" type="radio" name="filtros" value="11" />
                        Reprogramadas
                    </label>
                    <label class="btn btn-secondary opcion">
                        <input id="validacion" type="radio" name="filtros" value="12" />
                        Validación
                    </label>
                    <label class="btn btn-warning opcion">
                        <input id="pendientes" type="radio" name="filtros" value="13" />
                        Pendientes
                    </label>
                    <label class="btn btn-success opcion">
                        <input id="aprobadas" type="radio" name="filtros" value="14" />
                        Aprobadas
                    </label>
                    <label class="btn btn-danger opcion">
                        <input id="rechazadas" type="radio" name="filtros" value="15" />
                        Rechazadas
                    </label>
                </div>
            </div>
            <br />
            <div class="form-group row">
                <label for="example-text-input" class="col-sm-2">Nombre cliente</label>
                <div class="col-sm-4">
                    <input id="nombreCliente" class="form-control form-control-sm" type="text" />
                </div>

                <label for="example-text-input" class="col-sm-2">Identidad cliente</label>
                <div class="col-sm-4">
                    <input id="identidadCliente" class="form-control form-control-sm" type="text" />
                </div>
            </div>
            <div class="form-group row">
                <label class="col-sm-2">Búsqueda por Mes</label>
                <div class="col-sm-2">
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

            <table id="datatable-bandeja" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                <thead>
                    <tr>
                        <th>Solicitud</th>
                        <th>Agencia</th>
                        <th>Prod.</th>
                        <th>Identidad</th>
                        <th>Nombre cliente</th>
                        <th>Fecha de ingreso</th>
                        <th>En ingreso</th>
                        <th>En recepción</th>
                        <th>En analisis</th>
                        <th>En Campo</th>
                        <th>Condicio.</th>
                        <th>Reprog.</th>
                        <th>Validación</th>
                        <th>Resolución</th>
                    </tr>
                </thead>
                <tbody></tbody>
                <tfoot>
                    <tr>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                        <th></th>
                    </tr>
                </tfoot>
            </table>
        </div>
    </div>

    <div id="modalAbrirSolicitud" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalAbrirSolicitudLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalAbrirSolicitudLabel">Abrir solicitud</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Desea abrir la solicitud del cliente <strong><span id="lblCliente"></span></strong>con identidad <strong><span id="lblIdentidadCliente"></span></strong>?
                </div>
                <div class="modal-footer">
                    <button id="btnDetallesSolicitud" class="btn btn-primary waves-effect waves-light">
                        Detalles
                    </button>
                    <button runat="server" type="button" id="btnAbrirSolicitud" visible="false" class="btn btn-primary waves-effect waves-light">
                        Analizar
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
    <!-- ARCHIVOS NECESARIOS PARA LA PANTALLA -->
    <script src="../../Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="../../Scripts/plugins/parsleyjs/parsley.js"></script>
    <!-- DATATABLES -->
    <script src="../../Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="../../Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="../../Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="../../Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="../../Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="../../Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="../../Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="../../Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="../../Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/app/solicitudes/SolicitudesCredito_Bandeja.js"></script>
</body>
</html>
