<%@ Page Language="C#" AutoEventWireup="true" Inherits="Creditos_precalificado" CodeBehind="precalificado_buscador.aspx.cs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <style>
        html, body {
            background-color: #fff;
        }

        .card-header {
            background-color: #f8f9fa !important;
        }

        h6 {
            font-size: 1rem;
        }

        .card {
            box-shadow: none;
        }

        .nav-tabs > li > .active {
            background-color: whitesmoke !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <div class="card">
            <div class="card-header">
                <h6 class="card-title text-center">Precalificado de clientes</h6>
            </div>
            <div class="card-body">

                <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
                <asp:UpdatePanel runat="server" ID="upMultiView">
                    <ContentTemplate>

                        <div runat="server" id="divParametros" visible="true">

                            <div class="form-group row">
                                <label class="col-sm-2 col-form-label">No. Identidad</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtIdentidad" CssClass="form-control form-control-sm col-form-label MascaraIdentidad" required="required" runat="server"></asp:TextBox>
                                </div>

                                <label class="col-sm-2 col-form-label" runat="server" visible="false">Nombre del cliente</label>
                                <div class="col-sm-2" runat="server" visible="false">
                                    <asp:TextBox ID="txtNombreCliente" CssClass="form-control form-control-sm col-form-label" Visible="false" required="required" runat="server"></asp:TextBox>
                                </div>
                                <label class="col-sm-2 col-form-label">Telefono movil</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtTelefono" CssClass="form-control form-control-sm col-form-label MascaraTelefono" required="required" runat="server"></asp:TextBox>
                                </div>

                                <label class="col-sm-2 col-form-label">Ingreso mensual</label>
                                <div class="col-sm-2">
                                    <asp:TextBox ID="txtIngresos" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                                </div>

                                <label class="col-sm-2 col-form-label">Ciudad residencia</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList ID="ddlCiudadResidencia" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                </div>

                                <label class="col-sm-2 col-form-label">Producto solicitado</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList ID="ddlProducto" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                </div>

                                <label class="col-sm-2 col-form-label">Origen de ingresos</label>
                                <div class="col-sm-2">
                                    <asp:DropDownList ID="ddlOrigenIngreso" runat="server" CssClass="form-control form-control-sm col-form-label" OnSelectedIndexChanged="ddlOrigenIngreso_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                                </div>
                            </div>

                            <div class="form-group row" runat="server" id="divPanelAsalariado" visible="false">
                                <label class="col-sm-2 col-form-label">¿Trabaja en sector maquila o CallCenter?</label>
                                <div class="col-sm-2">
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbMaquilaoCallCenterSI" runat="server" GroupName="vgMaquilaoCallCenter" />
                                        <label class="form-check-label">Si</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbMaquilaoCallCenterNO" runat="server" GroupName="vgMaquilaoCallCenter" />
                                        <label class="form-check-label">No</label>
                                    </div>
                                </div>
                                <label class="col-sm-2 col-form-label">¿Tiene casa propia (a su nombre, mamá, papá, esposo (a))?</label>
                                <div class="col-sm-2">
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbCasaPropiaSI" runat="server" GroupName="vgCasaPropia" />
                                        <label class="form-check-label">Si</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbCasaPropiaNO" runat="server" GroupName="vgCasaPropia" />
                                        <label class="form-check-label">No</label>
                                    </div>
                                </div>
                                <label class="col-sm-2 col-form-label">¿Trabaja como guardia de seguridad?</label>
                                <div class="col-sm-2">
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbGuardiaSI" runat="server" GroupName="vgGuardia" />
                                        <label class="form-check-label">Si</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbGuardiaNO" runat="server" GroupName="vgGuardia" />
                                        <label class="form-check-label">No</label>
                                    </div>
                                </div>
                            </div>

                            <div class="form-group row" runat="server" id="divPanelComerciante" visible="false">
                                <label class="col-sm-2 col-form-label">¿Antigüedad del negocio es >=12 Meses?</label>
                                <div class="col-sm-2">
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbAntiguedaddeNegocioSI" runat="server" GroupName="vgAntiguedadNegocio" />
                                        <label class="form-check-label">Si</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbAntiguedaddeNegocioNO" runat="server" GroupName="vgAntiguedadNegocio" />
                                        <label class="form-check-label">No</label>
                                    </div>
                                </div>
                                <label class="col-sm-2 col-form-label">¿Cuenta con permiso de operacion vigente?</label>
                                <div class="col-sm-2">
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbPermisoOperacionSI" runat="server" GroupName="vgPermisodeOperacion" />
                                        <label class="form-check-label">Si</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbPermisoOperacionNO" runat="server" GroupName="vgPermisodeOperacion" />
                                        <label class="form-check-label">No</label>
                                    </div>
                                </div>
                                <label class="col-sm-2 col-form-label">¿Tiene casa propia (a su nombre, mamá, papá, esposo (a))?</label>
                                <div class="col-sm-2">
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbCasaComercianteSI" runat="server" GroupName="vgCasaComerciante" />
                                        <label class="form-check-label">Si</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbCasaComercianteNO" runat="server" GroupName="vgCasaComerciante" />
                                        <label class="form-check-label">No</label>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="form-group row" runat="server" id="PanelMensajeErrores">
                            <asp:Label CssClass="col-sm-2 col-form-label text-danger" ID="lblMensaje" runat="server"></asp:Label>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdateProgress ID="prgLoadingStatus" runat="server" AssociatedUpdatePanelID="upMultiView">
                    <ProgressTemplate>
                        <asp:Panel runat="server" Visible="true" BackColor="Transparent" BackImageUrl="/Imagenes/fondoTransparencia.png" Style="left: 0px; top: 0px; position: absolute; height: 100%; width: 100%;">
                            <div style="align-content: center; vertical-align: central; text-align: center; height: 100%; width: 100%;">
                                <table style="width: 100%; height: 100%;">
                                    <tr>
                                        <td>
                                            <div class="spinner-border" role="status" id="divCargandoAnalisis">
                                                <span class="sr-only">Cargando</span>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </asp:Panel>
                    </ProgressTemplate>
                </asp:UpdateProgress>

                <div class="form-group row">
                    <div class="button-items col-sm-2">
                        <asp:Button ID="btnConsultarCliente" Text="Consultar" CssClass="btn btn-primary btn-lg btn-block waves-effect waves-light" runat="server" OnClick="btnConsultarCliente_Click" />
                    </div>
                </div>
            </div>
        </div>
    </form>

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
            $(".MascaraIdentidad").inputmask("9999999999999");
            $("#MascaraTelefono").inputmask("99999999");
        });
    </script>
</body>
</html>
