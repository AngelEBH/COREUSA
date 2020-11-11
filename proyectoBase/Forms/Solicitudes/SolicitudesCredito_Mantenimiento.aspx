<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_Mantenimiento.aspx.cs" Inherits="SolicitudesCredito_Mantenimiento" %>

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
    <form runat="server">
        <div class="card h-100">
            <div class="card-header">
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
                    <button type="button" id="btnBuscarSolicitud" class="btn btn-md btn-secondary" runat="server">
                        Buscar
                    </button>
                </div>
                <div class="row mb-0">
                    <div class="col-12">
                        <div class="form-group row">
                            <div class="col-sm-3 col-12">
                                <label class="col-form-label">Cliente</label>
                                <asp:TextBox ID="txtNombreCliente" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">Identidad</label>
                                <asp:TextBox ID="txtIdentidadCliente" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-6">
                                <label class="col-form-label">RTN numérico</label>
                                <asp:TextBox ID="txtRtn" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-12">
                                <label class="col-form-label">Teléfono</label>
                                <asp:TextBox ID="txtTelefono" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-12">
                                <label class="col-form-label">Producto</label>
                                <asp:TextBox ID="txtProducto" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-3 col-12">
                                <label class="col-form-label">Tipo de solicitud</label>
                                <asp:TextBox ID="txtTipoDeSolicitud" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 col-12">
                                <label class="col-form-label">Agencia</label>
                                <asp:TextBox ID="txtAgencia" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 col-6">
                                <label class="col-form-label">Agente asignado</label>
                                <asp:TextBox ID="txtAgenteAsignado" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-sm-2 col-6">
                                <label class="col-form-label">Gestor asignado</label>
                                <asp:TextBox ID="txtGestorAsignado" ReadOnly="true" CssClass="form-control form-control-sm" runat="server"></asp:TextBox>
                            </div>
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
                            <div class="form-group row justify-content-center">
                                <div class="col-auto mt-2 mb-2 justify-content-center">
                                    <button type="button" id="btnReiniciarResolucion" disabled="disabled" class="FormatoBotonesIconoCuadrado40 disabled" aria-disabled="true" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Reiniciar Resolución
                                    </button>
                                    <button type="button" id="btnReiniciarAnalisis" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Reiniciar Analisis
                                    </button>
                                    <button type="button" id="btnEliminarDocumento" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/delete_file_40px.png');">
                                        Eliminar Docs
                                    </button>
                                    <button type="button" id="btnReasignarSolicitud" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/exchange_40px.png');">
                                        Reasignar Solicitud
                                    </button>
                                    <button type="button" id="btnEliminarCondicion" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/delete_document_40px.png');">
                                        Eliminar Condicion
                                    </button>
                                    <button type="button" id="btnReiniciarValidacion" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Reiniciar Validación
                                    </button>
                                    <button type="button" id="btnMantenimientoRefernecias" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/crowd_40px.png');">
                                        Referencias Personales
                                    </button>
                                    <button type="button" id="btnReiniciarCampo" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Reiniciar Campo
                                    </button>
                                    <button type="button" id="btnResolucionCampo" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/motorcycle_40px.png');">
                                        Resolución Campo
                                    </button>
                                    <button type="button" id="btnReiniciarReprogramacion" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/refresh_40px.png');">
                                        Reiniciar Reprog.
                                    </button>
                                    <button type="button" id="btnReasignarGestor" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/exchange_40px.png');">
                                        Reasignar Gestor
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="form-group row">

                            <h6 class="font-weight-bold pl-3">Historial de mantenimientos</h6>

                            <div class="col-12 justify-content-center table-responsive">
                                <table class="table table-condensed table-striped" id="tblHistorialMantenimiento">
                                    <thead class="thead-light">
                                        <tr>
                                            <th>Fecha</th>
                                            <th>CC</th>
                                            <th>Usuario</th>
                                            <th>Observaciones</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class="text-center" colspan="4">No hay registros disponibles...</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Mensaje de advertencias y errores -->
                <div class="form-group row" runat="server" id="PanelMensajeErrores">
                    <asp:Label CssClass="col-sm-2 col-form-label text-danger" ID="lblMensaje" runat="server"></asp:Label>
                </div>
            </div>
        </div>

        <div id="modalReiniciarProceso" class="modal fade" role="dialog" aria-labelledby="modalReiniciarProcesoLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title mt-0" id="modalReiniciarProcesoLabel">Reiniciar Resolución</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <label class="col-form-label">¿Está seguro de reiniciar la resolución de la solicitud?</label>
                    </div>
                    <div class="modal-footer">
                        <button id="btnReiniciarSolicitud" data-dismiss="modal" class="btn btn-danger waves-effect">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>


        <div id="modalResolucionCampo" class="modal fade" role="dialog" aria-labelledby="modalResolucionCampoLabel" aria-hidden="true">
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title mt-0" id="modalResolucionCampoLabel">Resolución de investigación de campo</h5>
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
                                <textarea id="txtObservacioneResolucionCampo" runat="server" class="form-control form-control-sm" data-parsley-maxlength="300" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnResolucionCampoConfirmar" data-dismiss="modal" class="btn btn-danger waves-effect">
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
    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/moment/moment.js"></script>
    <script src="/Scripts/plugins/moment/moment-with-locales.min.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Mantenimiento.js"></script>
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
