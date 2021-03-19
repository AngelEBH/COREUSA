<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Expedientes_Mantenimiento.aspx.cs" Inherits="Expedientes_Mantenimiento" %>

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
    <link href="/Scripts/plugins/sweet-alert2/sweetalert2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/buttons.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datatables/responsive.bootstrap4.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
</head>
<body class="EstiloBody-Listado">
    <form id="frmPrincipal" runat="server" data-parsley-excluded="[disabled]">
        <div class="card h-100">
            <div class="card-header">
                <div class="float-right p-1" id="Loader" style="display: none;">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>
                <h6 class="card-title font-weight-bold">Mantenimiento de expedientes</h6>
            </div>
            <div class="card-body">

                <!-- Nav tabs -->
                <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs">
                    <li class="nav-item">
                        <a class="nav-link active" data-toggle="tab" href="#tab_Catalogo_Documentos" role="tab" aria-selected="true">
                            <span class="d-block d-sm-none">Documentos</span>
                            <span class="d-none d-sm-block">Documentos</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-toggle="tab" href="#tab_Catalogo_TiposDeDocumentos" role="tab" aria-selected="false">
                            <span class="d-block d-sm-none">Tipos de
                                <br />
                                documentos</span>
                            <span class="d-none d-sm-block">Tipos de documentos</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" data-toggle="tab" href="#tab_GruposDeDocumentos" role="tab" aria-selected="false">
                            <span class="d-block d-sm-none">Grupos de
                                <br />
                                archivos</span>
                            <span class="d-none d-sm-block">Grupos de archivos</span>
                        </a>
                    </li>
                </ul>
                <div class="tab-content" id="tabContent">
                    <!-- Mantenimiento catalogo de documentos -->
                    <div class="tab-pane active" id="tab_Catalogo_Documentos" role="tabpanel">

                        <button type="button" class="btn btn-info mt-3">
                            <i class="fa fa-plus"></i>
                            Nuevo Documento
                        </button>
                        <div class="table-responsive mt-2">
                            <table id="datatable-catalogo-documentos" class="table-bordered display compact nowrap table-sm table-hover dataTable cursor-pointer" style="width: 100%" role="grid">
                                <thead>
                                    <tr>
                                        <th class="report-data">Documento</th>
                                        <th class="report-data">Tipo de documento</th>
                                        <th class="no-sort text-center">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%--<tr>
                                        <td>Poliza de seguros</td>
                                        <td>De Garantía</td>
                                        <td class="text-center">
                                            <button type="button" class="btn btn-sm btn-secondary m-0" title="Detalles">
                                                <i class="far fa-list-alt"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-info m-0" title="Editar">
                                                <i class="far fa-edit"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-danger m-0" title="Inactivar">
                                                <i class="far fa-trash-alt"></i>
                                            </button>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="3" class="text-center">Cargando información...</td>
                                    </tr>
                                </tbody>
                                <tfoot></tfoot>
                            </table>
                        </div>

                    </div>
                    <!-- Mantenimiento de tipos de documentos -->
                    <div class="tab-pane" id="tab_Catalogo_TiposDeDocumentos" role="tabpanel">

                        <button type="button" class="btn btn-info mt-3">
                            <i class="fa fa-plus"></i>
                            Nuevo Tipo de Documento
                        </button>
                        <div class="table-responsive mt-2">
                            <table id="datatable-catalogoTiposDeDocumentos" class="table-bordered display compact nowrap table-sm table-hover dataTable cursor-pointer" style="width: 100%" role="grid">
                                <thead>
                                    <tr>
                                        <th class="report-data">Tipo de documento</th>
                                        <th class="no-sort text-center">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%--<tr>
                                        <td>Documentos del cliente</td>
                                        <td class="text-center">
                                            <button type="button" class="btn btn-sm btn-secondary m-0" title="Detalles">
                                                <i class="far fa-list-alt"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-info m-0" title="Editar">
                                                <i class="far fa-edit"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-danger m-0" title="Inactivar">
                                                <i class="far fa-trash-alt"></i>
                                            </button>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="2" class="text-center">Cargando información...</td>
                                    </tr>
                                </tbody>
                                <tfoot></tfoot>
                            </table>
                        </div>

                    </div>
                    <!-- Mantenimiento de grupos de archivos -->
                    <div class="tab-pane" id="tab_GruposDeDocumentos" role="tabpanel">

                        <button type="button" class="btn btn-info mt-3">
                            <i class="fa fa-plus"></i>
                            Nuevo Grupo de Archivos
                        </button>
                        <div class="table-responsive mt-2">
                            <table id="datatable-catalogoGruposDeArchivos" class="table-bordered display compact nowrap table-sm table-hover dataTable cursor-pointer" style="width: 100%" role="grid">
                                <thead>
                                    <tr>
                                        <th class="report-data">Descripción</th>
                                        <th class="no-sort text-center">Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <%--<tr>
                                        <td>Documentos para asegurar</td>
                                        <td class="text-center">
                                            <button type="button" class="btn btn-sm btn-secondary m-0" title="Detalles">
                                                <i class="far fa-list-alt"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-info m-0" title="Editar">
                                                <i class="far fa-edit"></i>
                                            </button>
                                            <button type="button" class="btn btn-sm btn-danger m-0" title="Inactivar">
                                                <i class="far fa-trash-alt"></i>
                                            </button>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td colspan="2" class="text-center">Cargando información...</td>
                                    </tr>
                                </tbody>
                                <tfoot></tfoot>
                            </table>
                        </div>

                    </div>
                </div>

                <!-- Mensaje de advertencias y errores -->
                <div class="form-group row mb-0" runat="server" id="PanelMensajeErrores">
                    <asp:Label CssClass="col-sm-2 col-form-label text-danger" ID="lblMensaje" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </form>
    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script src="/Scripts/plugins/sweet-alert2/sweetalert2.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <!-- datatable js -->
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <!-- Responsive -->
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/datatables/responsive.bootstrap4.min.js"></script>
    <script src="/Scripts/app/Expedientes/Expedientes_Consultar.js?20210319102925"></script>
</body>
</html>
