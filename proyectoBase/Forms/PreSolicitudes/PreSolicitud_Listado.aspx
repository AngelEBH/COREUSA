<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSolicitud_Listado.aspx.cs" Inherits="PreSolicitud_Listado" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Pre Solicitudes</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <style>
        #datatable-presolicitudes tbody tr {
            cursor: pointer !important;
        }

        #datatable-presolicitudes tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }
    </style>
</head>
<body class="EstiloBody-Listado">
    <div class="card">
        <div class="card-header">
            <div class="row">
                <div class="col-8">
                    <h6>Pre Solicitudes</h6>
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

            <table id="datatable-presolicitudes" class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid">
                <thead>
                    <tr>
                        <th>Identidad</th>
                        <th>Nombre cliente</th>
                        <th>Fecha</th>
                        <th>CC</th>
                        <th>Gestor</th>
                        <th>Estado</th>
                        <th>Acciones</th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
            </table>
        </div>
    </div>


    <!-- modal detalles de la presolicitud -->
    <div id="modalDetalles" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalDetallesLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header text-center">
                    <h6 class="modal-title w-100 mt-0" id="modalDetallesLabel" style="text-align: center">Detalles Pre Solicitud <small class="font-weight-bold">(<span id="lblTipoDeUbicacion"></span>)</small></h6>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    <!-- INFORMACION DEL CLIENTE -->
                    <div class="form-group form-row">
                        <div class="col-12">
                            <label class="col-form-label">Cliente</label>
                            <input type="text" id="txtNombreCliente" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-6">
                            <label class="col-form-label">Identidad</label>
                            <input type="text" id="txtIdentidadCliente" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-6">
                            <label class="col-form-label">Teléfono</label>
                            <input type="tel" id="txtTelefonoCliente" disabled="disabled" class="form-control form-control-sm col-form-label mascara-telefono" />
                        </div>
                    </div>
                    <div class="form-group form-row">
                        <div class="col-lg-3 col-sm-6">
                            <label class="col-form-label">Nombre del trabajo</label>
                            <input type="tel" id="txtNombreTrabajo" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-lg-3 col-sm-6">
                            <label class="col-form-label">Teléfono <span id="lblTipoNumeroAdicional">del domicilio</span></label>
                            <input type="tel" id="txtTelefonoAdicional" disabled="disabled" class="form-control form-control-sm col-form-label mascara-telefono" />
                        </div>
                        <div class="col-lg-3 col-sm-6">
                            <label class="col-form-label">Extensión RRHH</label>
                            <input type="tel" id="txtExtensionRecursosHumanos" disabled="disabled" class="form-control form-control-sm col-form-label mascara-telefono" />
                        </div>
                        <div class="col-lg-3 col-sm-6">
                            <label class="col-form-label">Extensión cliente</label>
                            <input type="tel" id="txtExtensionCliente" disabled="disabled" class="form-control form-control-sm col-form-label mascara-telefono" />
                        </div>
                    </div>
                    <div class="form-group form-row">
                        <div class="col-sm-3">
                            <label class="col-form-label">Departamento</label>
                            <input type="text" id="txtDepartamento" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-sm-3">
                            <label class="col-form-label">Municipio</label>
                            <input type="text" id="txtMunicipio" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-sm-3">
                            <label class="col-form-label">Ciudad/Poblado</label>
                            <input type="text" id="txtCiudadPoblado" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-sm-3">
                            <label class="col-form-label">Barrio/Colonia</label>
                            <input type="text" id="txtBarrioColonia" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                    </div>
                    <div class="form-group form-row">
                        <div class="col-sm-12">
                            <label class="col-form-label">Detalle dirección</label>
                            <textarea id="txtDireccionDetallada" disabled="disabled" class="form-control" rows="2"></textarea>
                        </div>
                        <div class="col-12">
                            <label class="col-form-label">Referencias del domicilio</label>
                            <textarea id="txtReferenciasDireccionDetallada" disabled="disabled" class="form-control" rows="2"></textarea>
                        </div>
                    </div>

                    <div class="form-group">
                        <h6 class="border-bottom pb-1">Información de gestoría &nbsp; <span id="lblEstadoPreSolicitud" class="btn"></span></h6>
                    </div>
                    <div class="form-group form-row">
                        <div class="col-sm-12">
                            <label class="col-form-label">Gestor asignado</label>
                            <input type="text" id="txtGestorAsignado" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-lg-6 col-12">
                            <label class="col-form-label">Gestión</label>
                            <input type="text" id="txtGestion" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-lg-3 col-6">
                            <label class="col-form-label">Fecha Recibida</label>
                            <input type="text" id="txtFechaDescargadoPorGestor" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-lg-3 col-6">
                            <label class="col-form-label">Fecha Validado</label>
                            <input type="text" id="txtFechaValidacion" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-12">
                            <label class="col-form-label">Observaciones</label>
                            <textarea id="txtObservacionesGestoria" disabled="disabled" class="form-control" rows="2"></textarea>
                        </div>
                    </div>

                    <div class="form-group">
                        <h6 class="border-bottom pb-1">Auditoría</h6>
                    </div>
                    <div class="form-group form-row">
                        <div class="col-6">
                            <label class="col-form-label">Usuario creador</label>
                            <input type="text" id="txtUsuarioCreacion" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-6">
                            <label class="col-form-label">Fecha de creación</label>
                            <input type="text" id="txtFechaCreacion" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>

                        <div class="col-6">
                            <label class="col-form-label">Usuario última modificación</label>
                            <input type="text" id="txtUsuarioUltimaModificacion" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                        <div class="col-6">
                            <label class="col-form-label">Fecha última modificación</label>
                            <input type="text" id="txtFechaUltimaModificacion" disabled="disabled" class="form-control form-control-sm col-form-label" />
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cerrar
                    </button>
                </div>
            </div>
        </div>
    </div>
    <!-- /.modal -->


    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <!-- ARCHIVOS NECESARIOS PARA LA PANTALLA -->
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
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
    <script src="/Scripts/app/Solicitudes/PreSolicitud_Listado.js?V=20201211153985"></script>
</body>
</html>
