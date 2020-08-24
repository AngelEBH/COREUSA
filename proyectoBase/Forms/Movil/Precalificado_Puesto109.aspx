<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Precalificado_Puesto109.aspx.cs" Inherits="Clientes_Precalificado_Puesto109" %>

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
            height: 100%;
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

        #gvOferta tbody tr {
            cursor: pointer;
        }

        #gvOferta tbody td {
            outline: none;
            padding-top: 0 !important;
            padding-bottom: 0 !important;
        }

        .display-block {
            display: block !important;
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
                <div class="form-group row">
                    <div class="col-sm-2 text-center">
                        <asp:Label ID="lblRespuesta" ForeColor="Green" runat="server" CssClass="col-form-label font-16 font-weight-bold" Text="Pre-aprobado" />
                    </div>
                    <div class="col-sm-2">
                        Detalles:
                        <asp:Label ID="lblDetalleRespuesta" runat="server" CssClass="col-form-label font-12" />
                    </div>
                </div>

                <div class="form-group row mt-0" runat="server" id="divResolucionCreditos" visible="true">
                    <div class="col-sm-2 text-center">
                        <label class="col-form-label font-14 font-weight-bold pb-0">Resolución de créditos</label>
                    </div>
                    <div class="col-sm-2">
                        <asp:Label ID="lblResolucionCreditos" runat="server" CssClass="col-form-label font-12" Text="se deciidio mandar a comer mierda a este man" />
                    </div>
                </div>

                <ul class="nav nav-pills justify-content-center display-block" role="tablist" runat="server" id="navTabs">
                    <li class="nav-item dropdown">
                        <a class="nav-link active dropdown-toggle" data-toggle="dropdown" href="#" role="button" aria-haspopup="true" aria-expanded="false">Resultados</a>
                        <div class="dropdown-menu">
                            <a class="dropdown-item active" data-toggle="tab" href="#datosGenerales" role="tab" aria-selected="false">Datos generales</a>
                            <a class="dropdown-item" data-toggle="tab" href="#indicadoresPerfil" role="tab" aria-selected="false">Indicadores de perfil</a>
                            <a class="dropdown-item" data-toggle="tab" href="#tabOferta" role="tab" aria-selected="false" id="ddlOferta" runat="server">Oferta</a>
                            <a class="dropdown-item" data-toggle="tab" href="#ClienteCC" role="tab" aria-selected="false" id="ddlClienteCC" runat="server" visible="false">Cliente CallCenter</a>
                            <a class="dropdown-item" data-toggle="tab" href="#tabHistorialdeConsultas" role="tab" aria-selected="false" id="ddlHistorialdeConsultas" runat="server">Historial de consultas</a>
                            <a class="dropdown-item" data-toggle="tab" href="#resumenCrediticio" role="tab" aria-selected="false">Resumen crediticio</a>
                        </div>
                    </li>
                </ul>
                <div class="tab-content" runat="server" id="tabContent">
                    <!-- Datos generales -->
                    <div class="tab-pane active" id="datosGenerales" role="tabpanel">

                        <div runat="server" id="PanelCreditos">
                            <label class="col-sm-2 col-form-label text-center mt-1">Datos generales</label>

                            <div class="form-group">
                                <label class="col-form-label">Cliente</label>
                                <asp:TextBox ID="txtNombreCompleto" CssClass="form-control form-control-sm" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>

                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Identidad</label>
                                    <asp:TextBox ID="txtIdentidad" CssClass="form-control form-control-sm col-form-label" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label class="col-form-label">Teléfono</label>
                                    <asp:TextBox ID="txtTelefonoRegistrado" CssClass="form-control form-control-sm col-form-label" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Ingresos</label>
                                    <asp:TextBox ID="txtIngresosRegistrados" CssClass="form-control form-control-sm col-form-label text-right MascaraCantidad" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label class="col-form-label">Buró activo</label>
                                    <asp:TextBox ID="txtAntiguedadActiva" CssClass="form-control form-control-sm col-form-label" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-form-label">Consulta</label>
                                <asp:TextBox ID="txtInfoPrimerConsulta" CssClass="form-control form-control-sm col-form-label" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <!-- Indicadores de perfil -->
                    <div class="tab-pane" id="indicadoresPerfil" role="tabpanel">
                        <div runat="server" id="PanelCreditosDos">

                            <label class="col-sm-2 col-form-label text-center mt-1">Indicadores de perfil</label>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">SAF5</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgSAF" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">IHSS</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgIHSS" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">RNP</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgRNP" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">CallCenter</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgCallCenter" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/inconoOK.png" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">Mora Mayor</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgMoraMayor" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">Sobregiro</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgSobregiro" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">Score Bajo</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgScoreMenor" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">Saldo Castig.</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgCastigado" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">Incob. Irrec.</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgIncobrable" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" />
                                </div>
                            </div>

                            <div class="form-row">
                                <div class="col">
                                    <label class="col-form-label">Jurid.Legal</label>
                                </div>
                                <div class="col">
                                    <asp:Image ID="imgJuridico" CssClass="ImgIcono24NOAB" runat="server" Visible="false" ImageUrl="/Imagenes/iconoDetener24.png" />
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Oferta -->
                    <div class="tab-pane" id="tabOferta" runat="server" role="tabpanel">
                        <div class="form-group">
                            <label class="col-sm-2 col-form-label text-center mt-1">Oferta</label>

                            <div class="table-responsive">
                                <asp:GridView ID="gvOferta" runat="server" CssClass="table table-sm m-0 table-striped table-bordered" border="0"
                                    AutoGenerateColumns="False" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundField DataField="fcProducto" HeaderText="Producto" ReadOnly="True" />
                                        <asp:BoundField DataField="fnMontoOfertado" HeaderText="Monto ofertado" ItemStyle-CssClass="text-right" DataFormatString="{0:#,###0.00}" ReadOnly="True" />
                                        <asp:BoundField DataField="fiPlazo" HeaderText="Plazo" ItemStyle-CssClass="text-center" ReadOnly="True" />
                                        <asp:BoundField DataField="fnCuotaQuincenal" HeaderText="Cuota quincenal" ItemStyle-CssClass="text-right" DataFormatString="{0:#,###0.00}" ReadOnly="True" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!-- Cliente call center -->
                    <div class="tab-pane" id="ClienteCC" role="tabpanel" runat="server" visible="false">
                        <div class="form-group">
                            <label class="col-sm-2 col-form-label text-center mt-1">Informacion del cliente asignado en CallCenter</label>

                            <!-- Datos de Buro -->
                            <asp:Label ID="lblMensajeCallCenter" runat="server" CssClass="col-form-label">Detalle</asp:Label>
                            <asp:Label ID="lblMensajeCallCenterAgenteAsignado" runat="server" CssClass="col-form-label">Detalle</asp:Label>

                        </div>
                    </div>

                    <!-- Grid de historial de consultas hechas al cliente-->
                    <div class="tab-pane" id="tabHistorialdeConsultas" role="tabpanel" runat="server">

                        <div class="form-group">
                            <label class="col-sm-2 col-form-label text-center mt-1">Historial de consultas</label>

                            <div class="table-responsive">
                                <asp:GridView ID="gvBitacoraConsultas" runat="server" CssClass="table table-sm m-0 table-striped table-bordered" border="0"
                                    AutoGenerateColumns="False" ShowHeader="true">
                                    <Columns>
                                        <asp:BoundField HeaderText="Agencia" DataField="fcAgencia" ReadOnly="True" />
                                        <asp:BoundField HeaderText="Fecha de Consulta" DataField="fdFechaConsulta" />
                                        <asp:BoundField HeaderText="Oficial de Negocio" DataField="fcOficial" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <!-- Resumen crediticio -->
                    <div class="tab-pane" id="resumenCrediticio" role="tabpanel">
                        <div runat="server" id="Div2">
                            <label class="col-sm-2 col-form-label text-center mt-1">Resumen crediticio</label>

                            <br />

                            <div class="form-group">
                                <label class="col-form-label text-center">Endeudamiento</label>
                                <div class="progress">
                                    <div class="progress-bar progress-bar-striped progress-bar-animated" role="progressbar" style="width: 25%;" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" runat="server" id="PorcentajeEndeudamiento">25</div>
                                </div>
                            </div>

                            <div class="form-group">
                                <label class="col-form-label">Score promedio:</label>
                                <asp:Label ID="lblScorePromedio" runat="server" CssClass="col-form-label font-weight-bold" />
                            </div>

                            <label class="col-sm-2 col-form-label text-center mt-1">Obligaciones</label>

                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Banca</label>
                                    <asp:TextBox ID="txtBanca" CssClass="form-control form-control-sm col-form-label text-right MascaraCantidad" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label class="col-form-label">Tarjetas</label>
                                    <asp:TextBox ID="txtTarjetas" CssClass="form-control form-control-sm col-form-label text-right MascaraCantidad" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Comercio</label>
                                    <asp:TextBox ID="txtComercio" CssClass="form-control form-control-sm col-form-label text-right MascaraCantidad" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <label class="col-form-label">Total Obligaciones</label>
                                    <asp:TextBox ID="txtTotal" CssClass="form-control form-control-sm col-form-label text-right MascaraCantidad" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <label class="col-form-label text-center">Información de IHSS</label>

                            <asp:GridView ID="gvIHSS" runat="server" CssClass="table display compact nowrap table-condensed table-striped" AutoGenerateColumns="False" GridLines="None" ShowHeader="true">
                                <Columns>
                                    <asp:BoundField DataField="fcYear" HeaderText="Año" />
                                    <asp:BoundField DataField="fcEmpresa" HeaderText="Empresa" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="form-group row">
                    <div class="button-items col-sm-2">
                        <asp:Button ID="btnIngresarSolicitud" runat="server" CssClass="btn btn-info btn-lg btn-block waves-effect waves-light" Text="Ingresar solicitud" OnClick="btnIngresarSolicitud_Click" Visible="false" />
                    </div>
                </div>

                <div class="form-group row" runat="server" id="PanelMensajeErrores">
                    <asp:Label CssClass="col-sm-2 col-form-label text-danger" ID="lblMensaje" runat="server">Resultado sujeto a que la informacion ingresada sea real.</asp:Label>
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
            $(".identidad").inputmask("9999999999999");
        });
    </script>
</body>
</html>
