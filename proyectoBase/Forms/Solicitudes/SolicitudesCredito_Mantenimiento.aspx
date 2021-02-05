<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Mantenimiento.aspx.cs" Inherits="SolicitudesCredito_Mantenimiento" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Mantenimiento de solicitud</title>
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
                <h6 class="card-title font-weight-bold">Mantenimiento de solicitud de crédito <span id="lblIdSolicitud" style="display: none;">No. 0</span></h6>
            </div>
            <div class="card-body">
                <div class="form-inline" id="divParametrosBusqueda">
                    <div class="form-group mb-0">
                        <label class="form-control-plaintext font-weight-bold">No. Solicitud</label>
                    </div>
                    <div class="form-group mx-sm-3 mb-0">
                        <asp:TextBox ID="txtNoSolicitud" type="tel" CssClass="form-control mascara-enteros text-left" Enabled="true" runat="server"></asp:TextBox>
                    </div>
                    <button type="button" id="btnBuscarSolicitud" onclick="BuscarSolicitud()" class="btn btn-md btn-secondary" runat="server">
                        Buscar
                    </button>
                </div>
                <div class="row mt-3 mb-0">
                    <div class="col-6">
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
                                        <td><span class="label label-table label-success" id="lblNombreCliente"></span></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Identidad</span></td>
                                        <td id="lblIdentidadCliente"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">RTN numérico</span></td>
                                        <td id="lblRtn"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Teléfono</span></td>
                                        <td id="lblTelefono"></td>
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
                    <div class="col-6">
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
                                        <td id="lblProducto"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Tipo de solicitud</span></td>
                                        <td id="lblTipoDeSolicitud"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">CC / Vendedor</span></td>
                                        <td id="lblAgenciaYVendedorAsignado"></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td><span class="font-weight-bold text-muted">Gestor asignado</span></td>
                                        <td id="lblGestorAsignado"></td>
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

                <div id="divInformacionSolicitud" style="display: none;">
                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs nav-tabs-custom nav-justified mt-1" role="tablist" id="navTabs">
                        <li class="nav-item">
                            <a class="nav-link active" data-toggle="tab" href="#tabOpcionesCreditos" role="tab" aria-selected="false">
                                <span class="d-block d-sm-none">Opciones</span>
                                <span class="d-none d-sm-block">Opciones de mantenimiento</span>
                            </a>
                        </li>
                    </ul>
                    <div class="tab-content">
                        <div class="tab-pane active bg-light" id="tabOpcionesCreditos" role="tabpanel">
                            <div class="form-group row justify-content-center mb-0">
                                <div class="col-auto mt-2 mb-2 justify-content-center">
                                    <button type="button" id="btnReiniciarResolucion" runat="server" class="FormatoBotonesIconoCuadrado40 disabled" aria-disabled="true" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Cambiar Resolución
                                    </button>
                                    <button type="button" id="btnCambiarFondos" runat="server" class="FormatoBotonesIconoCuadrado40" aria-disabled="true" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/money_bag_40px.png');">
                                        Cambiar Fondos
                                    </button>
                                    <button type="button" id="btnReiniciarAnalisis" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Reiniciar Analisis
                                    </button>
                                    <button type="button" id="btnSolicitudDocumentos" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/delete_file_40px.png');">
                                        Eliminar Docs
                                    </button>
                                    <button type="button" id="btnReasignarVendedor" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/exchange_40px.png');">
                                        Reasignar Solicitud
                                    </button>
                                    <button type="button" id="btnCondiciones" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/delete_document_40px.png');">
                                        Anular Condicion
                                    </button>
                                    <button type="button" id="btnReiniciarValidacion" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Reiniciar Validación
                                    </button>
                                    <button type="button" id="btnReferenciasPersonales" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/crowd_40px.png');">
                                        Referencias Personales
                                    </button>
                                    <button type="button" id="btnReiniciarCampo" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Reiniciar Campo
                                    </button>
                                    <button type="button" id="btnResolucionCampo" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/motorcycle_40px.png');">
                                        Resolución Campo
                                    </button>
                                    <button type="button" id="btnReiniciarReprogramacion" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Reiniciar Reprog.
                                    </button>
                                    <button type="button" id="btnReasignarGestor" runat="server" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/exchange_40px.png');">
                                        Reasignar Gestor
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row mb-0">

                            <h6 class="font-weight-bold pl-3">Historial de mantenimientos</h6>

                            <div class="col-12 justify-content-center table-responsive">
                                <table class="table table-sm table-bordered table-hover cursor-pointer" id="tblHistorialMantenimiento" style="width: 100%">
                                    <thead class="thead-light">
                                        <tr>
                                            <th>Fecha</th>
                                            <th>CC</th>
                                            <th>Usuario</th>
                                            <th class="no-sort">Observaciones</th>
                                        </tr>
                                    </thead>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Mensaje de advertencias y errores -->
                <div class="form-group row mb-0" runat="server" id="PanelMensajeErrores">
                    <asp:Label CssClass="col-sm-2 col-form-label text-danger" ID="lblMensaje" runat="server"></asp:Label>
                </div>
            </div>
        </div>

        <!-- Fondos del préstamo -->
        <div id="modalCambiarFondos" class="modal fade" role="dialog" aria-labelledby="modalCambiarFondosLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pb-2">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="modalCambiarFondosLabel">Cambiar Fondos</h6>
                            </div>
                            <div class="col-12 text-muted">
                                Fondo actual:
                                <span class="text-muted" id="lblFondoActual"></span>
                            </div>
                        </div>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <label class="col-form-label font-weight-bold">Fondos disponibles</label>
                            </div>
                            <div class="col-12">
                                <asp:DropDownList ID="ddlFondos" runat="server" CssClass="form-control form-control-sm" required="required"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesCambiarFondos" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="500" data-parsley-minlength="15" rows="3"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnCambiarFondosConfirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Resolucion de la solicitud -->
        <div id="modalCambiarResolucion" class="modal fade" role="dialog" aria-labelledby="modalCambiarResolucionLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalCambiarResolucionLabel">Cambiar resolución de la solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row mb-0">
                            <div class="col-auto">
                                <label class="col-form-label font-weight-bold">Resolución</label>
                            </div>
                            <div class="col-auto">
                                <asp:DropDownList ID="ddlCatalogoResoluciones" runat="server" CssClass="form-control form-control-sm" required="required"></asp:DropDownList>
                            </div>
                        </div>

                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesCambiarResolucionSolicitud" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnCambiarResolucionConfirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Resultados de campo -->
        <div id="modalResolucionCampo" class="modal fade" role="dialog" aria-labelledby="modalResolucionCampoLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalResolucionCampoLabel">Resolución de investigación de campo</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row mb-0">
                            <div class="col-auto">
                                <label class="col-form-label font-weight-bold">Resolución</label>
                            </div>
                            <div class="col-auto">
                                <asp:DropDownList ID="ddlResolucionCampo" runat="server" CssClass="form-control form-control-sm" required="required">
                                    <asp:ListItem Text="Seleccione una opción" Value="" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Autorizar" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Rechazar" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacioneResolucionCampo" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" id="btnResolucionCampoConfirmar" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalReiniciarCampo" class="modal fade" role="dialog" aria-labelledby="modalReiniciarCampoLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalReiniciarCampoLabel">Reiniciar investigación de campo</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="cbReiniciarCampoDomicilio" />
                            <label class="form-check-label" for="cbReiniciarCampoDomicilio">
                                Investigación del domicilio
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="cbReiniciarCampoTrabajo" />
                            <label class="form-check-label" for="cbReiniciarCampoTrabajo">
                                Investigación del trabajo
                            </label>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesReiniciarCampo" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnReiniciarInvestigacionDeCampo" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Asignaciones -->
        <div id="modalAsignarGestorSolicitud" class="modal fade" role="dialog" aria-labelledby="modalAsignarGestorSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalAsignarGestorSolicitudLabel">Asignar gestor de campo</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-form-label">Gestores de campo</label>
                            <asp:DropDownList ID="ddlGestores" runat="server" CssClass="form-control form-control-sm" required="required"></asp:DropDownList>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionAsignarGestor" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnAsignarGestor_Confirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAsignarVendedorSolicitud" class="modal fade" role="dialog" aria-labelledby="modalAsignarVendedorSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalAsignarVendedorSolicitudLabel">Reasignar solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <label class="col-form-label">Vendedores</label>
                            <asp:DropDownList ID="ddlVendedores" runat="server" CssClass="form-control form-control-sm" required="required"></asp:DropDownList>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesReasignarVendedor" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnAsignarVendedores_Confirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Condiciones -->
        <div id="modalAnularCondicion" class="modal fade" role="dialog" aria-labelledby="modalAnularCondicionLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalAnularCondicionLabel">Condiciones de la solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="table-responsive">
                            <table id="tblCondiciones" class="table table-sm table-bordered table-hover cursor-pointer w-100">
                                <thead>
                                    <tr>
                                        <th>Tipo Condición</th>
                                        <th>Descripción</th>
                                        <th>Comentario Adicional</th>
                                        <th>Estado</th>
                                        <th>Acciones</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAnularCondicionConfirmar" class="modal fade" role="dialog" aria-labelledby="modalAnularCondicionConfirmarLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalAnularCondicionConfirmarLabel">Anular condición</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesAnularCondicion" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnAnularCondicionConfirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary" data-toggle="modal" data-target="#modalAnularCondicion">
                            <i class="far fa-arrow-alt-circle-left"></i>
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Documentos -->
        <div id="modalDocumentacionSolicitud" class="modal fade" role="dialog" aria-labelledby="modalDocumentacionSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalDocumentacionSolicitudLabel">Documentación de la solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="table-responsive">
                            <table id="tblDocumenacionSolicitud" class="table table-sm table-bordered table-hover cursor-pointer" style="width: 100%">
                                <thead>
                                    <tr>
                                        <th colspan="2" class="text-center text-muted">Información del documento</th>
                                        <th colspan="2" class="text-center text-muted">Acciones</th>
                                    </tr>
                                    <tr>
                                        <th>Tipo de documento</th>
                                        <th>Nombre</th>
                                        <th class="text-center no-sort">Vista previa</th>
                                        <th class="text-center no-sort">Eliminar</th>
                                    </tr>
                                </thead>
                                <tbody>
                                </tbody>
                            </table>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEliminarDocumentoSolicitud" class="modal fade" role="dialog" aria-labelledby="modalEliminarDocumentoSolicitudLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalEliminarDocumentoSolicitudLabel">Eliminar documento</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesEliminarDocumento" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnEliminarDocumentoConfirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary" data-toggle="modal" data-target="#modalDocumentacionSolicitud">
                            <i class="far fa-arrow-alt-circle-left"></i>
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalVistaPreviaDocumento" class="modal fade" role="dialog" aria-labelledby="modalVistaPreviaDocumentoLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <div class="form-group row mb-0">
                            <div class="col-12">
                                <h6 class="m-0" id="lblTipoDocumentoVistaPrevia"></h6>
                            </div>
                            <div class="col-12 text-muted">
                                <span class="text-muted" id="modalVistaPreviaDocumentoLabel">Vista previa del documento</span>
                            </div>
                        </div>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <div id="divImgVistaPrevia"></div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary" data-toggle="modal" data-target="#modalDocumentacionSolicitud">
                            <i class="far fa-arrow-alt-circle-left"></i>
                            Volver a los documentos
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Reiniciar procesos -->
        <div id="modalReiniciarReprogramacion" class="modal fade" role="dialog" aria-labelledby="modalReiniciarReprogramacionLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalReiniciarReprogramacionLabel">Reiniciar reprogramación</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesReiniciarReprogramacion" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnReiniciarReprogramacionConfirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalReiniciarValidacion" class="modal fade" role="dialog" aria-labelledby="modalReiniciarValidacionLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalReiniciarValidacionLabel">Reiniciar validación</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesReiniciarValidacion" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnReiniciarValidacionConfirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalReiniciarAnalisis" class="modal fade" role="dialog" aria-labelledby="modalReiniciarAnalisisLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalReiniciarAnalisisLabel">Reiniciar análisis de la solicitud</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="cbValidacionInformacionPersonal" />
                            <label class="form-check-label">
                                Validación de información personal
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="cbValidacionInformacionLaboral" />
                            <label class="form-check-label">
                                Validación de información laboral
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="cbValidacionReferenciasPersonales" />
                            <label class="form-check-label">
                                Validacion de referencias personales
                            </label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="cbValidacionDocumentos" />
                            <label class="form-check-label">
                                Validación de documentación
                            </label>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesReiniciarAnalisis" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="500" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnReiniciarAnalisisConfirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!-- Referencias personales -->
        <div id="modalReferenciasPersonales" class="modal fade" role="dialog" aria-labelledby="modalReferenciasPersonalesLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalReferenciasPersonalesLabel">Referencias personales</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <button type="button" id="btnAgregarReferencia" class="btn btn-info mb-1 float-right">
                                <i class="fas fa-plus"></i>
                                Agregar nueva
                            </button>
                            <div class="table-responsive">
                                <table id="tblReferenciasPersonales" class="table table-sm table-bordered table-hover cursor-pointer" style="width: 100%">
                                    <thead>
                                        <tr>
                                            <th>Nombre completo</th>
                                            <th class="no-sort">Lugar de trabajo</th>
                                            <th>Telefono</th>
                                            <th>Tiempo C.</th>
                                            <th>Parentesco</th>
                                            <th class="no-sort">Acciones</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalAgregarReferenciaPersonal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalAgregarReferenciaPersonalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header pb-1 pt-1">
                        <h6 class="modal-title" id="modalAgregarReferenciaPersonalLabel">Agregar referencia personal</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Nombre completo</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtNombreReferencia" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="referenciasPersonales" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Telefono</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtTelefonoReferencia" CssClass="form-control form-control-sm mascara-telefono" type="text" required="required" data-parsley-group="referenciasPersonales" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Tiempo de conocer</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlTiempoDeConocerReferencia" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonales" data-parsley-errors-container="#error-ddlTiempoDeConocerReferencia"></asp:DropDownList>
                                <div id="error-ddlTiempoDeConocerReferencia"></div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Parentesco</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlParentescos" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonales" data-parsley-errors-container="#error-ddlParentescos"></asp:DropDownList>
                                <div id="error-ddlParentescos"></div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Lugar de trabajo</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtLugarTrabajoReferencia" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="referenciasPersonales" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesNuevaReferencia" runat="server" class="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonales" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer pt-2 pb-2">
                        <button id="btnAgregarReferenciaConfirmar" type="button" class="btn btn-primary mr-1">
                            Agregar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary" data-toggle="modal" data-target="#modalReferenciasPersonales">
                            <i class="far fa-arrow-alt-circle-left"></i>
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEditarReferenciaPersonal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalEditarReferenciaPersonalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header pb-1 pt-1">
                        <h6 class="modal-title" id="modalEditarReferenciaPersonalLabel">Editar referencia personal</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Nombre completo</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtNombreReferenciaPersonal_Editar" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="referenciasPersonalesEditar" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Telefono</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtTelefonoReferenciaPersonal_Editar" CssClass="form-control form-control-sm mascara-telefono" type="text" required="required" data-parsley-group="referenciasPersonalesEditar" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Tiempo de conocer</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlTiempoDeConocerReferencia_Editar" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonalesEditar" data-parsley-errors-container="#error-ddlTiempoDeConocerReferencia_Editar"></asp:DropDownList>
                                <div id="error-ddlTiempoDeConocerReferencia_Editar"></div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Parentesco</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlParentesco_Editar" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonalesEditar" data-parsley-errors-container="#error-ddlParentesco_Editar"></asp:DropDownList>
                                <div id="error-ddlParentesco_Editar"></div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Lugar de trabajo</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtLugarDeTrabajoReferencia_Editar" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="referenciasPersonalesEditar" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesEditarReferenciaPersonal" runat="server" class="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonalesEditar" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer pt-2 pb-2">
                        <button id="btnEditarReferenciaConfirmar" type="button" class="btn btn-primary mr-1">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary" data-toggle="modal" data-target="#modalReferenciasPersonales">
                            <i class="far fa-arrow-alt-circle-left"></i>
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEliminarReferenciaPersonal" class="modal fade" role="dialog" aria-labelledby="modalEliminarReferenciaPersonalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalEliminarReferenciaPersonalLabel">Eliminar referencia personal</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesEliminarReferenciaPersonal" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnEliminarReferenciaPersonalConfirmar" type="button" class="btn btn-danger">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary" data-toggle="modal" data-target="#modalReferenciasPersonales">
                            <i class="far fa-arrow-alt-circle-left"></i>
                            Cancelar
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
