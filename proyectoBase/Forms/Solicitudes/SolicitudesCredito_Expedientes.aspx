<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Expedientes.aspx.cs" Inherits="SolicitudesCredito_Expedientes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <meta name="description" content="Expediente del préstamo" />
    <title>Expediente del préstamo</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/css/unitegallery.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
    <link href="/CSS/Estilos.css" rel="stylesheet" />
    <style>
        h6 {
            font-size: 1rem;
        }

        .card {
            box-shadow: none;
        }
    </style>
</head>
<body>
    <form id="frmPrincipal" runat="server" data-parsley-excluded="[disabled]">
        <div class="card h-100">
            <div class="card-header">
                <div class="float-right p-1" id="Loader" style="display: none;">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>
                <h6 class="card-title font-weight-bold">Expediente del Préstamo | Solicitud de crédito 1080</h6>
            </div>
            <div class="card-body">
                <div class="row mt-3 mb-0">
                    <div class="col-sm-6">
                        <div class="table-responsive">
                            <table class="table table-sm" style="margin-bottom: 0px;">
                                <tbody>
                                    <tr>
                                        <td><span class="font-weight-bold">Información del cliente</span></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Cliente</span></td>
                                        <td><span class="label label-table label-success" id="lblNombreCliente">Willian Onandy Diaz Serrano</span></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Identidad</span></td>
                                        <td id="lblIdentidadCliente">050220002944</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">RTN numérico</span></td>
                                        <td id="lblRtn">05022000029448</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Teléfono</span></td>
                                        <td id="lblTelefono">96116376</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="col-sm-6">
                        <div class="table-responsive">
                            <table class="table table-sm" style="margin-bottom: 0px;">
                                <tbody>
                                    <tr>
                                        <td><span class="font-weight-bold">Información de la solicitud</span></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Producto</span></td>
                                        <td id="lblProducto">Prestadito Automovil Empeño</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Tipo de solicitud</span></td>
                                        <td id="lblTipoDeSolicitud">Nueva</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">CC / Vendedor</span></td>
                                        <td id="lblAgenciaYVendedorAsignado">Prestadito Matriz / XXXXX YYYY</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Gestor asignado</span></td>
                                        <td id="lblGestorAsignado">XXXXX YYYY</td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>

                <div class="row justify-content-center bg-light border ">
                    <div class="col-auto mt-2 mb-2">
                        <button type="button" data-toggle="modal" data-target="#modalGrupoDeDocumentos" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/folder_40px.png');">
                            Archivo 1
                        </button>
                        <button type="button" data-toggle="modal" data-target="#modalGrupoDeDocumentos" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/folder_40px.png');">
                            Archivo 2
                        </button>
                        <button type="button" data-toggle="modal" data-target="#modalGrupoDeDocumentos" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/folder_40px.png');">
                            Archivo 3
                        </button>
                        <button type="button" data-toggle="modal" data-target="#modalGrupoDeDocumentos" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/folder_40px.png');">
                            Archivo 4
                        </button>
                        <button type="button" data-toggle="modal" data-target="#modalGrupoDeDocumentos" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/folder_40px.png');">
                            Archivo 5
                        </button>
                        <button type="button" data-toggle="modal" data-target="#modalGrupoDeDocumentos" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/folder_40px.png');">
                            Archivo 6
                        </button>
                    </div>
                </div>

                <h6 class="font-weight-bold">Documentos del expediente</h6>
                <div class="row">
                    <div class="col-lg-4 col-md-6 pt-3 border">
                        <label class="font-weight-bold font-14">Tipo de documento</label>
                        <table class="table table-sm table-borderless table-hover cursor-pointer" id="tblTiposDeDocumentos" style="width: 100%">
                            <thead class="thead-light">
                                <tr>
                                    <th>Documento</th>
                                    <th class="text-center">Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>Comprobante de ingresos <small class="badge badge-secondary">2</small></td>
                                    <td class="text-center">
                                        <button type="button" class="btn btn-sm btn-secondary">
                                            <i class="fas fa-arrow-right"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Comprobante de domicilio <small class="badge badge-secondary">3</small></td>
                                    <td class="text-center">
                                        <button type="button" class="btn btn-sm btn-secondary">
                                            <i class="fas fa-arrow-right"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Solicitud de póliza de seguro <small class="badge badge-secondary">3</small></td>
                                    <td class="text-center">
                                        <button type="button" class="btn btn-sm btn-secondary">
                                            <i class="fas fa-arrow-right"></i>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="col-lg-4 col-md-6 pt-3 border">
                        
                        <div class="row justify-content-between">
                            <div class="col-auto">
                                <label class="font-14">Tipo de documento: <b>Solicitud de póliza</b></label>
                            </div>
                            <div class="col-auto">
                                <button type="button" class="btn btn-sm btn-info">
                                    <i class="fas fa-plus"></i>
                                    Agregar Nuevo
                                </button>
                            </div>
                        </div>


                        <table class="table table-sm table-borderless table-hover cursor-pointer" id="tblDocumentos" style="width: 100%">
                            <thead class="thead-light">
                                <tr>
                                    <th>Nombre del archivo</th>
                                    <th class="text-center">Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td>S1080CTE_202103111058</td>
                                    <td class="text-center">
                                        <button type="button" class="btn btn-sm btn-secondary">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>S1080CTE_202103111058</td>
                                    <td class="text-center">
                                        <button type="button" class="btn btn-sm btn-secondary">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>S1080CTE_202103111058</td>
                                    <td class="text-center">
                                        <button type="button" class="btn btn-sm btn-secondary">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>S1080CTE_202103111058</td>
                                    <td class="text-center">
                                        <button type="button" class="btn btn-sm btn-secondary">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </td>
                                </tr>
                                <tr>
                                    <td>S1080CTE_202103111058</td>
                                    <td class="text-center">
                                        <button type="button" class="btn btn-sm btn-secondary">
                                            <i class="fas fa-search"></i>
                                        </button>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                    <div class="col-lg-4 col-md-12 pt-3 border">
                        <label class="font-weight-bold font-14">Previsualización del documento</label>
                    </div>
                </div>

                <!-- Mensaje de advertencias y errores -->
                <div class="form-group row mb-0" runat="server" id="PanelMensajeErrores">
                    <asp:Label CssClass="col-sm-2 col-form-label text-danger" ID="lblMensaje" runat="server"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Modal Mostrar Gupo De Documentos -->
        <div id="modalGrupoDeDocumentos" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalGrupoDeDocumentosLabel" aria-hidden="true">
            <div class="modal-dialog modal-xl">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalGrupoDeDocumentosLabel">Documentos de <b>Archivo 1</b></h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="row mt-3">
                            <div class="col-sm-4 border-right border-top pt-3">
                                <div class="table-responsive border-right border p-2" style="max-height: 50vh; overflow: auto;">
                                    <table id="tblExpedienteSolicitudGarantia" class="table table-sm table-hover cursor-pointer" style="width: 100%">
                                        <thead>
                                            <tr>
                                                <th>Documento</th>
                                                <th class="text-center no-sort"></th>
                                            </tr>
                                        </thead>
                                        <tbody></tbody>
                                    </table>
                                </div>
                            </div>
                            <div class="col-sm-8 border-top pt-3">
                                <h6 class="font-weight-bold mt-0">Previsualización</h6>

                                <div class="align-self-center" id="divPrevisualizacionDocumento" runat="server" style="display: none;"></div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnGuardarBloqueArchivo" class="btn btn-primary">
                            <i class="far fa-file-pdf"></i>
                            Guardar PDF
                        </button>
                        <button type="button" id="btnEnviarBloqueArchivoPorPDF" class="btn btn-primary">
                            <i class="mdi mdi-email-outline"></i>
                            Enviar por E-Mail
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

    </form>
    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <!-- datatable js -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <!-- Responsive -->
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/datatables/responsive.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/js/unitegallery.min.js"></script>
    <script src="/Scripts/plugins/unitegallery/themes/default/ug-theme-default.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Mantenimiento.js?20210203010352"></script>

    <script>
        $(document).ready(function () {
            $(".mascara-enteros").inputmask("decimal", {
                alias: "numeric",
                groupSeparator: "",
                digits: 0,
                integerDigits: 13,
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
        });
    </script>
</body>
</html>
