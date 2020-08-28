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
<body class="EstiloBody" style="max-width: 1100px !important;">
    <form id="frmCalculadora" runat="server">
        <div class="card h-100">
            <div class="card-header">
                <h6 class="card-title">Mantenimiento de solicitud</h6>
            </div>
            <div class="card-body">


                <div class="form-inline">
                    <div class="form-group mb-0">
                        <label class="form-control-plaintext">No. Solicitud</label>
                    </div>
                    <div class="form-group mx-sm-3 mb-0">
                        <asp:TextBox ID="txtNoSolicitud" type="tel" CssClass="form-control mask-numeros text-left" Enabled="true" runat="server"></asp:TextBox>
                    </div>
                    <button type="button" id="btnConsultar" class="btn btn-md btn-secondary" runat="server">
                        Buscar
                    </button>
                </div>


                <div class="form-row">

                    <div class="col-auto">
                        <label class="col-form-label">Identidad</label>
                        <asp:TextBox ID="txtIdentidadCliente" type="tel" CssClass="form-control form-control-sm" Enabled="false" runat="server"></asp:TextBox>
                    </div>

                    <div class="col">
                        <label class="col-form-label">Nombre</label>
                        <asp:TextBox ID="txtNombreCliente" type="tel" CssClass="form-control form-control-sm col-form-label" Enabled="false" runat="server"></asp:TextBox>
                    </div>

                </div>

                <!-- Nav tabs -->
                <ul class="nav nav-tabs nav-tabs-custom nav-justified mt-1" role="tablist" id="navTabs">
                    <li class="nav-item">
                        <a class="nav-link active" data-toggle="tab" href="#tabOpcionesCreditos" role="tab" aria-selected="false">
                            <span class="d-block d-sm-none">Opciones</span>
                            <span class="d-none d-sm-block">Información de la Solicitud</span>
                        </a>
                    </li>
                </ul>
                <div class="tab-content">
                    <div class="tab-pane active bg-light" id="tabOpcionesCreditos" role="tabpanel">

                        <div class="form-group row justify-content-center">
                            <div class="col-auto m-3">
                                <button type="button" id="btnReiniciarResolucion" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/reiniciarConEngranaje.png');">
                                    Reiniciar Resolución
                                </button>
                                <button type="button" id="btnReiniciarAnalisis" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/reiniciarAnalisis.png');">
                                    Reiniciar Analisis
                                </button>
                                <button type="button" id="btnEliminarDocumento" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/deleteFile.png');">
                                    Eliminar Docs
                                </button>
                                <button type="button" id="btnReasignarSolicitud" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/reasignarPersona.png');">
                                    Reasignar Solicitud
                                </button>
                                <button type="button" id="btnEliminarCondicion" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/eliminarCondicion.png');">
                                    Eliminar Condicion
                                </button>
                                <button type="button" id="btnReiniciarValidacion" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/reiniciarValidacion.png');">
                                    Reiniciar Validación
                                </button>
                                <button type="button" id="btnReiniciarCampo" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/campo.png');">
                                    Reiniciar Campo
                                </button>
                                <button type="button" id="btnReiniciarReprogramacion" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/reiniciarReprogramacion.png');">
                                    Reiniciar Reprog.
                                </button>
                                <button type="button" id="btnReasignarGestor" class="FormatoBotonesIconoCuadrado40" style="position: relative; margin-top: 5px; margin-left: 5px; background-image: url('/Imagenes/reasignarPersona.png');">
                                    Reasignar Gestor
                                </button>
                            </div>
                        </div>
                    </div>
                    <div class="form-group row justify-content-center">
                        <h6 class="col-12 text-center">Historial de mantenimientos</h6>
                        <br />

                        <div class="col-12">
                            <table class="table table-striped">
                                <tr>
                                    <th>Fecha</th>
                                    <th>CC</th>
                                    <th>Usuario</th>
                                    <th>Observaciones</th>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <!-- Mensaje de advertencias y errores -->
                <div class="form-group row" runat="server" id="PanelMensajeErrores">
                    <asp:Label CssClass="col-sm-2 col-form-label text-danger" ID="lblMensaje" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </form>


    <!-- modal reiniciar proceso -->
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

    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(document).ready(function () {
            $(".MascaraCantidad").inputmask("decimal", {
                alias: "numeric",
                groupSeparator: ",",
                digits: 2,
                integerDigits: 11,
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
            $(".mask-numeros").inputmask("numeric");
            $(".identidad").inputmask("9999999999999");
        });
    </script>
</body>
</html>
