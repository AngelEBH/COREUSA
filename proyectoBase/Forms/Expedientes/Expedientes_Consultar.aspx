<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Expedientes_Consultar.aspx.cs" Inherits="Expedientes_Consultar" %>

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
    <link href="/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
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
                <div class="row justify-content-around">
                    <div class="col-sm-10">
                        <h6 class="card-title font-weight-bold">Expediente del Préstamo | Solicitud de crédito
                            <span runat="server" id="lblNoSolicitudCredito"></span>
                            <span class="badge badge-danger" id="lblExpedienteInactivo" runat="server" visible="false">EXPEDIENTE INACTIVO</span>
                        </h6>
                    </div>
                    <div class="col-sm-2 text-right">
                        <asp:Label runat="server" ID="lblEstadoExpediente"></asp:Label>
                    </div>
                </div>
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
                                        <td>
                                            <asp:Label class="label label-table label-success" ID="lblNombreCliente" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Identidad</span></td>
                                        <td>
                                            <asp:Label ID="lblIdentidadCliente" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">RTN numérico</span></td>
                                        <td>
                                            <asp:Label ID="txtRTNCliente" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Teléfono</span></td>
                                        <td>
                                            <asp:Label ID="txtTelefonoCliente" runat="server"></asp:Label>
                                        </td>
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
                                        <td>
                                            <asp:Label ID="lblProducto" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Tipo de solicitud</span></td>
                                        <td>
                                            <asp:Label ID="lblTipoDeSolicitud" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">CC / Vendedor</span></td>
                                        <td>
                                            <asp:Label ID="lblAgenciaYVendedorAsignado" runat="server"></asp:Label>
                                        </td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Gestor asignado</span></td>
                                        <td>
                                            <asp:Label ID="lblGestorAsignado" runat="server"></asp:Label>
                                        </td>
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

                <h6 class="font-weight-bold">Grupos de archivos</h6>

                <div class="row justify-content-center bg-light border ">
                    <div class="col-auto mt-2 mb-2" runat="server" id="divGruposDeArchivos"></div>
                </div>

                <h6 class="font-weight-bold">Documentos del expediente</h6>

                <div class="row">
                    <div class="col-lg-3 col-md-6 pt-3 border">
                        <label class="font-weight-bold font-14">Lista de documentos del expediente</label>
                        <button type="button" id="btnGenerarCheckList" class="btn btn-sm p-0 float-right" title="Descargar Check List">
                            <i class="fas fa-download font-14"></i>
                        </button>
                        <div class="table-responsive" style="max-height: 50vh; overflow: auto;">

                            <table class="table table-sm table-borderless table-hover cursor-pointer" id="tblTiposDeDocumentos" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th>Documento</th>
                                        <th class="text-center">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody></tbody>
                            </table>
                        </div>
                    </div>
                    <div class="col-lg-4 col-md-6 pt-3 border">
                        <div class="row justify-content-between">
                            <div class="col-auto pr-0">
                                <label class="font-14 font-weight-bold text-muted" id="lblTituloListadoTipoDocumento">
                                    Ningún documento seleccionado
                                    <i class="far fa-question-circle" id="lblDescripcionTipoDeDocumento" title=""></i>
                                </label>
                            </div>
                            <div class="col-auto pl-0 text-right">
                                <button type="button" id="btnCambiarEstadoANoAdjuntado" class="btn btn-sm btn-danger btnCambiarEstadoDocumento" data-idestado="2" disabled="disabled">
                                    NO
                                </button>
                                <button type="button" id="btnCambiarEstadoANoAplica" class="btn btn-sm btn-danger btnCambiarEstadoDocumento" data-idestado="3" disabled="disabled">
                                    N/A
                                </button>
                                <button type="button" id="btnAgrearNuevoTipoDocumento" class="btn btn-sm btn-info">
                                    <i class="fas fa-plus"></i>
                                    Agregar Nuevo
                                </button>
                            </div>
                        </div>
                        <table class="table table-sm table-borderless table-hover cursor-pointer" id="tblListadoTipoDocumento" style="width: 100%">
                            <thead>
                                <tr>
                                    <th>Nombre del archivo</th>
                                    <th class="text-center">Acciones</th>
                                </tr>
                            </thead>
                            <tbody></tbody>
                        </table>
                    </div>
                    <div class="col-lg-5 col-md-12 pt-3 border">

                        <label class="font-weight-bold font-14">Previsualización del documento</label>
                        <div class="align-self-center" id="divPrevisualizacionDocumento_TipoDeDocumento" style="display: none;"></div>
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
                        <h6 class="modal-title mt-0 font-weight-bold" id="modalGrupoDeDocumentosLabel"><span id="lblNombreGrupoDeArchivos"></span></h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12">
                                <div class="alert alert-info bg-info text-white pt-1 pb-1" role="alert">
                                    <i class="fas fa-exclamation-circle text-white"></i>
                                    <span id="lblDescripcionDetalladaGrupoDeArchivos"></span>
                                </div>
                            </div>
                            <div class="col-sm-4 border-right border-top pt-3">
                                <div class="table-responsive border-right border p-2" style="max-height: 50vh; overflow: auto;">
                                    <table id="tblDocumentosDelGrupoDeArchivos" class="table table-sm table-hover cursor-pointer" style="width: 100%">
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

                                <div class="align-self-center" id="divPrevisualizacionDocumento_GrupoDeArchivos" runat="server" style="display: none;"></div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnGuardarGrupoArchivo" class="btn btn-secondary">
                            <i class="far fa-file-pdf"></i>
                            Guardar PDF
                        </button>
                        <button type="button" id="btnEnviarGrupoArchivoPorPDF" class="btn btn-secondary">
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

        <!-- ============== Modal Guardar Documentos De Un Determinado Tipo De Documento ==== -->
        <div id="modalGuardarDocumentos" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalGuardarDocumentosLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalGuardarDocumentosLabel">Guardar Documentos
                                    (<span id="lblTituloTipoDocumento" class="m-0 font-weight-bold text-muted"></span>)
                                </h6>
                            </div>
                        </div>
                    </div>
                    <div class="modal-body">
                        <div class="row">
                            <div class="col-12 text-muted font-12 mb-2">
                                <span id="lblDocumentoObligatorio" class="text-danger">Documento obligatorio</span>
                                / Mínimo:
                                <span id="lblCantidadMinimaDocumentos">3</span>
                                / Máximo:
                                <span id="lblCantidadMaximaDocumentos">3</span>
                                / Guardados:
                                <span id="lblCantidadDocumentosGuardados">0</span>
                            </div>
                            <div class="col-12">
                                <div class="alert alert-info bg-info text-white pt-1 pb-1" role="alert">
                                    <i class="fas fa-exclamation-circle text-white"></i><b>Descripción del documento:</b>
                                    <span id="lblDescripcionTipoDocumento"></span>
                                </div>
                            </div>
                            <div class="col-12">
                                <div id="divFormularioDocumentos"></div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnGuardarDocumentos_Confirmar" class="btn btn-primary">
                            Guardar documentos
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-danger">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- ================================== HTML Expendiente ================================ -->
        <div id="divContenedorExpediente" class="contenedorPDFx">
            <div class="card m-0 divImprimir font-12" runat="server" visible="true" id="divExpedientePDF" style="display: ;">
                <div class="card-body pt-0 pr-5 pl-5">
                    <div class="row justify-content-between">
                        <div class="col-auto">
                            <asp:Label runat="server" ID="lblRazonSocial" class="font-weight-bold d-block"></asp:Label>
                            <asp:Label runat="server" ID="lblNombreComercial" class="font-weight-bold d-block"></asp:Label>
                            <label class="font-weight-bold d-block">
                                Solicitud de crédito N°:
                                <b runat="server" id="lblNoSolicitudCredito_Expediente"></b>
                            </label>
                            <small class="font-weight-bold">Fecha:
                                <b runat="server" id="lblFechaActual_Expediente"></b>
                            </small>
                        </div>
                        <div class="col-auto align-content-start pr-0">
                            <div id="qr_Expediente"></div>
                        </div>
                    </div>
                    <div class="row border border-gray mb-2 mt-2">
                        <div class="col-7">
                            <div class="form-group row mb-1">
                                <div class="col-4">
                                    <b>Nombre:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblNombreCliente_Expediente" runat="server"></label>
                                </div>

                                <div class="col-4">
                                    <b>Identidad:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblIdentidadCliente_Expediente" runat="server"></label>
                                </div>

                                <div class="col-4">
                                    <b>Departamento:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblDepartamento_Expediente" runat="server"></label>
                                </div>

                                <div class="col-4">
                                    <b>Dirección:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblDireccionCliente_Expediente" runat="server"></label>
                                </div>

                                <div class="col-4">
                                    <b>Tel. Celular:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblTelefonoCliente_Expediente" runat="server"></label>
                                </div>
                            </div>
                            <u class="font-weight-bold">Datos laborales</u>
                            <div class="form-group row mb-0">
                                <div class="col-4">
                                    <b>Tipo de trabajo:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblTipoDeTrabajo_Expediente" runat="server"></label>
                                </div>
                                <div class="col-4">
                                    <b>Cargo:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblPuestoAsignado_Expediente" runat="server"></label>
                                </div>
                                <div class="col-4">
                                    <b>Teléfono:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblTelefonoTrabajo_Expediente" runat="server"></label>
                                </div>
                                <div class="col-4">
                                    <b>Dirección:</b>
                                </div>
                                <div class="col-8">
                                    <label class="mb-0" id="lblDirecciónTrabajo_Expediente" runat="server"></label>
                                </div>
                            </div>
                        </div>
                        <div class="col-5 border-left border-gray">
                            <div class="form-group row border-bottom border-gray mb-1">
                                <u class="p-2 font-weight-bold">Datos del préstamo</u>
                            </div>
                            <div class="form-group row mb-0">
                                <div class="col-6">
                                    <b>N° Solicitud:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblNoSolicitud_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Fecha de otorgamiento:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblFechaOtorgamiento_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Plazo:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblCantidadCuotas_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Monto otorgado:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblMontoOtorgado_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Valor de la cuota:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblValorCuota_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Fecha primer pago:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblFechaPrimerPago_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Frecuencia:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblFrecuenciaPlazo_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Vencimiento</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblFechaVencimiento_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Oficial:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblOficialNegocios_Expediente" runat="server"></label>
                                </div>
                                <div class="col-6">
                                    <b>Gestor:</b>
                                </div>
                                <div class="col-6">
                                    <label class="mb-0" id="lblGestor_Expediente" runat="server"></label>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-6 p-0">
                            <asp:Table ID="tblDocumentos_Expediente" CssClass="table table-sm table-condensed table-bordered" runat="server">
                                <asp:TableHeaderRow TableSection="TableHeader">
                                    <asp:TableHeaderCell>Tipo de documento</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">SI</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">NO</asp:TableHeaderCell>
                                    <asp:TableHeaderCell CssClass="text-center">N/A</asp:TableHeaderCell>
                                </asp:TableHeaderRow>
                            </asp:Table>
                        </div>
                        <div class="col-6">
                            <u class="font-weight-bold">TIPO DE RENEGOCIACIÓN</u>
                            <asp:Table ID="tblTipoDeSolicitud_Expediente" CssClass="font-weight-bold table-borderless mt-2" runat="server"></asp:Table>
                        </div>
                        <div class="col-12 p-0">
                            <div class="form-group row">
                                <label class="col-2 pr-0">Especifique otros:</label>
                                <asp:Label runat="server" ID="lblEspecifiqueOtros_Expediente" CssClass="col-10 border-top-0 border-left-0 border-right-0 border-bottom border-dark"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="row pt-5 justify-content-center">
                        <div class="col-5 text-center">
                            <label class="form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Firma de entrega</label>
                            <label class="mt-0 d-block" runat="server" id="lblNombreFirmaEntrega_Expediente"></label>
                        </div>
                        <div class="col-5 text-center">
                            <label class="form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px; border-width: 1px; border-color: black;"></label>
                            <label class="mt-0 d-block">Firma de recibe</label>
                            <label class="mt-0 d-block">Mariely Guzman</label>
                        </div>
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
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script src="/Scripts/app/Expedientes/Expedientes_Consultar.js?20210315081528"></script>
</body>
</html>
