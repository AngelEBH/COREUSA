<%@ Page Language="C#" AutoEventWireup="true" EnableEventValidation="false" CodeBehind="Negociaciones_Registrar.aspx.cs" Inherits="Negociaciones_Registrar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Cotizador / Negociaciones</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <style>
        html, body {
            background-color: #fff;
        }

        .card-header {
            background-color: #f8f9fa !important;
        }

        .card {
            box-shadow: none;
        }

        .nav-tabs > li > .active {
            background-color: whitesmoke !important;
        }

        .text-xs {
            font-size: .7rem !important;
        }

        .loading {
            position: fixed;
            top: 0;
            right: 0;
            bottom: 0;
            left: 0;
            background-color: transparent;
        }

        .loader {
            left: 50%;
            margin-left: -4em;
            font-size: 10px;
            border: .8em solid rgba(218, 219, 223, 1);
            border-left: .8em solid rgba(58, 166, 165, 1);
            animation: spin 1.1s infinite linear;
        }

            .loader, .loader:after {
                border-radius: 50%;
                width: 8em;
                height: 8em;
                display: block;
                position: absolute;
                top: 50%;
                margin-top: -4.05em;
            }

        @keyframes spin {
            0% {
                transform: rotate(360deg);
            }

            100% {
                transform: rotate(0deg);
            }
        }

        .modal {
            overflow-y: auto;
        }

        .divContenedorNegociacion label {
            font-size: 12px !important;
        }

        .divContenedorNegociacion table, table tr, table tr td, table tr th {
            font-size: 12px !important;
        }
    </style>
</head>
<body class="EstiloBody-Listado-W1100px">
    <form runat="server" id="FrmGuardarNegociacion">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>

        <!-- UP PRINCIPAL -->
        <asp:UpdatePanel runat="server" ID="upMultiView" UpdateMode="Conditional" ChildrenAsTriggers="True">
            <ContentTemplate>
                <div class="card m-0">
                    <div class="card-body pt-0">
                        <asp:UpdatePanel runat="server" ID="upCotizador" UpdateMode="Always" ChildrenAsTriggers="True">
                            <ContentTemplate>

                                <!-- PARAMETROS DEL COTIZADOR -->
                                <asp:Panel ID="divParametros" runat="server" Style="padding-top: 1.25rem;" Visible="true">
                                    <div class="form-group form-row">
                                        <div class="col">
                                            <label class="col-form-label">Valor del automovil</label>
                                            <asp:TextBox ID="txtValorVehiculo" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" Text="" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col" id="divPrima" runat="server" visible="true">
                                            <label class="col-form-label">Valor de la Prima</label>
                                            <asp:Label CssClass="" ID="lblPorcenajedePrima" runat="server" Text="" Visible="false" />
                                            <asp:TextBox ID="txtValorPrima" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" Text="" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group" runat="server" id="divMontoFinanciarVehiculo" visible="false">
                                        <label class="col-form-label">Valor del vehiculo a financiar</label>
                                        <asp:TextBox ID="txtMonto" Enabled="false" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-form-label">Score Promedio</label>
                                        <asp:TextBox ID="txtScorePromedio" type="tel" CssClass="form-control form-control-sm col-form-label MascaraNumerica" Text="" runat="server"></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <div>
                                            <div class="form-check form-check-inline">
                                                <asp:RadioButton CssClass="form-check-input" ID="rbFinanciamiento" runat="server" GroupName="rbTipoFinanciamiento" OnCheckedChanged="rbFinanciamiento_CheckedChanged" AutoPostBack="true" />
                                                <label class="form-check-label">Financiamiento</label>
                                            </div>
                                            <div class="form-check form-check-inline">
                                                <asp:RadioButton CssClass="form-check-input" ID="rbEmpeno" runat="server" GroupName="rbTipoFinanciamiento" OnCheckedChanged="rbEmpeno_CheckedChanged" AutoPostBack="true" />
                                                <label class="form-check-label">Financiamiento con garantía</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <label class="col-form-label">Plazos disponibles</label>
                                        <asp:DropDownList ID="ddlPlazos" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                    </div>
                                    <div class="form-group row mb-0">
                                        <div class="button-items col-sm-12">
                                            <asp:Button ID="btnCalcular" Text="Calcular" CssClass="btn btn-primary btn-lg btn-block waves-effect waves-light" runat="server" OnClick="btnCalcular_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>

                                <br />
                                <!-- RESULTADOS DEL COTIZADOR -->
                                <asp:Panel ID="PanelCreditos1" runat="server" Visible="false">
                                    <!-- Nav tabs -->
                                    <ul class="nav nav-tabs nav-tabs-custom nav-justified" role="tablist" runat="server" id="navTabs">
                                        <li class="nav-item">
                                            <a class="nav-link active" data-toggle="tab" href="#GastosDeCierreEfectivo" role="tab" aria-selected="false">
                                                <span class="d-block d-sm-none">Gastos de cierre
                                                    <br />
                                                    efectivo</span>
                                                <span class="d-none d-sm-block"><small>Gastos de cierre efectivo</small></span>
                                            </a>
                                        </li>
                                        <li class="nav-item">
                                            <asp:HyperLink ID="HyperLinkGastosDeCierreFinanciados" runat="server" class="nav-link" data-toggle="tab" href="#GastosDeCierreFinanciados" role="tab" aria-selected="false">
                                                <span class="d-block d-sm-none">Gastos de cierre 
                                                    <br />
                                                    financiados</span>
                                                <span class="d-none d-sm-block"><small>Gastos de cierre financiados</small></span>
                                            </asp:HyperLink>
                                        </li>
                                    </ul>
                                    <!-- Tab panes -->
                                    <div class="tab-content" runat="server" id="tabContent" visible="true">
                                        <!-- Gastos de cierre en efectivo -->
                                        <div class="tab-pane active" id="GastosDeCierreEfectivo" role="tabpanel">
                                            <div class="form-group row">
                                                <div class="col-10 align-self-end">
                                                    <label class="col-form-label">Valor del prestamo (<span class="text-xs"><asp:Label CssClass="col-form-label p-0 font-weight-bold align-self-center" ID="lblEtiqueta1" runat="server" Text="" /></span>)</label>
                                                </div>
                                                <div class="col-2 justify-content-end">
                                                    <asp:Button ID="btnDescargarCotizacion" runat="server" OnClick="btnDescargarCotizacion_Click" CssClass="btn btn-lg float-right" Style="background-image: url(/Imagenes/export_pdf_80px.png); background-size: contain !important; background-repeat: no-repeat;" />
                                                </div>
                                                <div class="col-12">
                                                    <asp:TextBox ID="txtValorPrestamo1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-12">
                                                    <label class="col-form-label">Cuota del PMO</label>
                                                    <asp:TextBox ID="txtCuotaPrestamo1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group form-row">
                                                <div class="col">
                                                    <label class="col-form-label text-xs">Cuota del seguro</label>
                                                    <asp:TextBox ID="txtValorSeguro1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col">
                                                    <label class="col-form-label text-xs">Cuota servic. GPS</label>
                                                    <asp:TextBox ID="txtServicioGPS1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label class="col-12 col-form-label">Cuota total mensual</label>
                                                <div class="col-12">
                                                    <asp:TextBox ID="txtCuotaTotal1" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label class="col-12 col-form-label">Gastos de cierre <span class="text-xs">(Pago en efectivo)</span></label>
                                                <div class="col-12">
                                                    <asp:TextBox ID="txtGastosdeCierreEfectivo" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <!-- Gastos de cierre financiados -->
                                        <div class="tab-pane" id="GastosDeCierreFinanciados" role="tabpanel">
                                            <div class="form-group row">
                                                <div class="col-10 align-self-end">
                                                    <label class="col-form-label">Valor del prestamo (<span class="text-xs"><asp:Label CssClass="col-form-label p-0 font-weight-bold align-self-center" ID="lblEtiqueta2" runat="server" Text="" /></span>)</label>
                                                </div>
                                                <div class="col-2">
                                                    <asp:Button ID="btnDescargarCotizacion2" runat="server" OnClick="btnDescargarCotizacion2_Click" CssClass="btn btn-lg float-right" Style="background-image: url(/Imagenes/export_pdf_80px.png); background-size: contain !important; background-repeat: no-repeat;" />
                                                </div>
                                                <div class="col-12">
                                                    <asp:TextBox ID="txtValorPrestamo2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-12">
                                                    <label class="col-form-label">Cuota del PMO</label>
                                                    <asp:TextBox ID="txtCuotaPrestamo2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group form-row">
                                                <div class="col">
                                                    <label class="col-form-label text-xs">Cuota del seguro</label>
                                                    <asp:TextBox ID="txtValorSeguro2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col">
                                                    <label class="col-form-label text-xs">Cuota servic. GPS</label>
                                                    <asp:TextBox ID="txtServicioGPS2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group row">
                                                <label class="col-12 col-form-label">Cuota total mensual</label>
                                                <div class="col-12">
                                                    <asp:TextBox ID="txtCuotaTotal2" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group row" runat="server" visible="false">
                                                <label class="col-12 col-form-label">Cuota total mensual <span class="text-xs">(Gastos de cierre)</span></label>
                                                <div class="col-12">
                                                    <asp:TextBox ID="txtCuotaTotalGastosDeCierreFinanciados" CssClass="form-control form-control-sm col-form-label text-right FormatotxtMonedaRO" ReadOnly="true" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <!-- Boton para Nueva cotización -->
                                    <div class="form-group row m-0" runat="server" id="divNuevoCalculo" visible="false">
                                        <div class="button-items col-sm-12 p-0 text-center">
                                            <asp:Button ID="btnNuevaCotizacion" Text="Nueva cotización" CssClass="btn btn-primary waves-effect waves-light m-0" runat="server" OnClick="btnNuevaCotizacion_Click" />

                                            <asp:Button ID="btnModalGuardarNegociacion" Text="Guardar negociación" CssClass="btn btn-primary waves-effect waves-light m-0" runat="server" OnClick="btnModalGuardarNegociacion_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>

                                <!-- Label para mensajes y advertencias -->
                                <div class="form-group row mr-0 ml-0 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                                    <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
                                </div>

                            </ContentTemplate>
                        </asp:UpdatePanel>
                        <asp:UpdateProgress ID="updateProgressCotizador" runat="server">
                            <ProgressTemplate>
                                <div class="loading">
                                    <div class="loader"></div>
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                </div>

                <!-- MODAL DE GUARDAR NEGOCIACIÓN -->
                <div id="modalGuardarNegociacion" class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="modalResumenLabel" aria-hidden="true">
                    <div class="modal-dialog modal-lg">
                        <div class="modal-content">
                            <div class="modal-header text-center">
                                <h6 class="modal-title w-100 mt-0" id="modalResumenLabel" style="text-align: center">Guardar negociación</h6>
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            </div>
                            <div class="modal-body">
                                <asp:UpdatePanel runat="server" ID="upInformacionNegociacion" UpdateMode="Always" ChildrenAsTriggers="True">
                                    <ContentTemplate>

                                        <asp:Panel ID="divInformacionNegociacion" runat="server" Visible="false">
                                            <div class="form-group form-row">
                                                <!-- INFORMACION DEL CLIENTE -->
                                                <div class="col-12">
                                                    <label class="col-form-label">Cliente</label>
                                                    <asp:TextBox ID="txtNombreCliente" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Identidad</label>
                                                    <asp:TextBox ID="txtIdentidadCliente" Enabled="false" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-6">
                                                    <label class="col-form-label">Teléfono</label>
                                                    <asp:TextBox ID="txtTelefonoCliente" type="tel" Enabled="false" CssClass="form-control form-control-sm col-form-label telefono" Text="" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <div>
                                                    <div class="form-check form-check-inline">
                                                        <asp:RadioButton CssClass="form-check-input" ID="rbGastosDeCierreFinanciados" runat="server" GroupName="rbTipoGastosDeCierre" />
                                                        <label class="form-check-label">Gastos de cierre Financiados</label>
                                                    </div>
                                                    <div class="form-check form-check-inline">
                                                        <asp:RadioButton CssClass="form-check-input" ID="rbGastosDeCierreEfectivo" runat="server" GroupName="rbTipoGastosDeCierre" />
                                                        <label class="form-check-label">Gastos de cierre Efectivo</label>
                                                    </div>
                                                </div>
                                            </div>

                                            <!-- INFORMACION DE LA GARANTIA -->
                                            <h6 class="border-top pt-2">Información de la garantía</h6>

                                            <div class="form-group form-row">
                                                <div class="col">
                                                    <label class="col-form-label">Marca</label>
                                                    <asp:DropDownList ID="ddlMarca" runat="server" CssClass="form-control form-control-sm col-form-label" OnSelectedIndexChanged="ddlMarca_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                                </div>
                                                <div class="col" id="div2" runat="server" visible="true">
                                                    <label class="col-form-label">Modelo</label>
                                                    <asp:DropDownList ID="ddlModelo" Enabled="false" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                                </div>
                                                <div class="col-sm-4" id="div3" runat="server" visible="true">
                                                    <label class="col-form-label">Año</label>
                                                    <asp:TextBox ID="txtAnio" Enabled="true" type="tel" CssClass="form-control form-control-sm col-form-label MascaraAnio" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group form-row">
                                                <div class="col">
                                                    <label class="col-form-label">Matricula</label>
                                                    <asp:TextBox ID="txtMatricula" type="tel" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col" id="div4" runat="server" visible="true">
                                                    <label class="col-form-label">Color</label>
                                                    <asp:TextBox ID="txtColor" type="tel" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group form-row">
                                                <div class="col">
                                                    <label class="col-form-label">Cilindraje</label>
                                                    <asp:TextBox ID="txtCilindraje" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" Text="" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col" id="div5" runat="server" visible="true">
                                                    <label class="col-form-label">Recorrido</label>
                                                    <asp:TextBox ID="txtRecorrido" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" Text="" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col" id="div8" runat="server" visible="true">
                                                    <label class="col-form-label">UM</label>
                                                    <asp:DropDownList ID="ddlUnidadDeMedida" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group form-row">
                                                <div class="col">
                                                    <label class="col-form-label">Origen</label>
                                                    <asp:DropDownList ID="ddlOrigen" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                                </div>
                                                <div class="col" id="div6" runat="server" visible="true">
                                                    <label class="col-form-label">Vendedor</label>
                                                    <asp:TextBox ID="txtVendedor" type="tel" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                                </div>
                                                <div class="col-sm-4" id="div7" runat="server" visible="true">
                                                    <label class="col-form-label">Autolote</label>
                                                    <asp:DropDownList ID="ddlAutolote" runat="server" CssClass="form-control form-control-sm col-form-label"></asp:DropDownList>
                                                </div>
                                            </div>
                                            <div class="form-group form-row">
                                                <div class="col-12">
                                                    <label class="col-form-label">Fotografía</label>
                                                    <input type="file" id="fotografiaGarantia" name="files">
                                                </div>
                                                <div class="col-12">
                                                    <label class="col-form-label">Detalles</label>
                                                    <textarea id="txtDetallesGarantia" runat="server" class="form-control" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2"></textarea>
                                                </div>
                                            </div>
                                            <!-- Label para mensajes y advertencias de la negociacion -->
                                            <div class="form-group row mr-0 ml-0 alert alert-danger" runat="server" id="PanelMensajeGuardarNegociacion" visible="false">
                                                <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensajeGuardarNegociacion" runat="server" Text=""></asp:Label>
                                            </div>
                                            <div class="modal-footer">
                                                <asp:Button ID="btnGuardarNegociacion" Text="Confirmar" CssClass="btn btn-primary waves-effect waves-light" OnClick="btnGuardarNegociacion_Click" runat="server" />

                                                <button type="button" data-dismiss="modal" class="btn btn-secondary waves-effect">
                                                    Cancelar
                                                </button>
                                            </div>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <asp:UpdateProgress ID="updateProgressGuardarNegociacion" runat="server">
                                    <ProgressTemplate>
                                        <div class="loading">
                                            <div class="loader"></div>
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- /.modal -->

                <!-- Modal solicitar identidad del cliente en caso de que venga en la URL -->
                <div id="modalSolicitarIdentidad" class="modal fade bs-example-modal-center" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title mt-0">Buscar cliente</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            </div>
                            <asp:UpdatePanel runat="server" ID="upBuscarPrecalificadoIdentidadCliente">
                                <ContentTemplate>
                                    <asp:Panel ID="PanelSolicitarIdentidad" runat="server" Visible="true">
                                        <div class="modal-body">
                                            <div class="form-group form-row">
                                                <div class="col-12">
                                                    <label class="col-form-label">Identidad</label>
                                                    <asp:TextBox ID="txtBuscarIdentidad" CssClass="form-control form-control-sm col-form-label" Text="" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnBuscarIdentidadCliente" Text="Buscar" CausesValidation="false" CssClass="btn btn-primary waves-effect waves-light" OnClick="btnBuscarIdentidadCliente_Click" runat="server" />

                                            <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                                                Cancelar
                                            </button>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="updateProgressBuscarCliente" runat="server">
                                <ProgressTemplate>
                                    <div class="loading">
                                        <div class="loader"></div>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </div>
                </div>
                <!-- /.modal -->

                <!-- Modal identidad del cliente ingresado no está precalificado -->
                <div id="modalClienteNoPrecalificado" class="modal fade bs-example-modal-center" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true">
                    <div class="modal-dialog modal-dialog-centered">
                        <div class="modal-content">
                            <div class="modal-header">
                                <h5 class="modal-title mt-0">Cliente no precalificado</h5>
                                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            </div>
                            <asp:UpdatePanel runat="server" ID="upClienteNoPrecalificado">
                                <ContentTemplate>
                                    <asp:Panel ID="panelRedirigirAPrecalificado" runat="server" Visible="true">
                                        <div class="modal-body">
                                            <label class="col-form-label">Este cliente todavía no ha sido precalificado. Debe precalificarlo para continuar.</label>
                                            <div class="form-group">
                                                <div class="form-check">
                                                    <asp:CheckBox ID="cbGuardarMontosCotizacion" runat="server" CssClass="form-check-input col-form-label" />
                                                    <label class="form-check-label col-form-label" for="gridCheck">
                                                        Guardar montos de la cotización
                                                    </label>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <asp:Button ID="btnPrecalificarCliente" CausesValidation="false" Text="Precalificar ahora" CssClass="btn btn-primary waves-effect waves-light" OnClick="btnPrecalificarCliente_Click" runat="server" />

                                            <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                                                Cancelar
                                            </button>
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="updateProgressClienteNoPrecalificado" runat="server">
                                <ProgressTemplate>
                                    <div class="loading">
                                        <div class="loader"></div>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </div>
                    </div>
                </div>
                <!-- /.modal -->

                <!-- PDF DE LA COTIZACION (IGNORAR) -->
                <div id="divContenedor" style="margin-top: 999px; display: none;">
                    <asp:UpdatePanel runat="server" ID="upCotizacionPDF" UpdateMode="Always" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <div class="card m-0 divCotizacionPDF" runat="server" visible="true" id="divCotizacionPDF" style="display: none;">
                                <div class="card-body pt-0">
                                    <div class="row">
                                        <div class="col-12 text-center">
                                            <img src="/Imagenes/LogoPrestaditoMediano.png" />
                                            <h3>COTIZACIÓN</h3>
                                            <hr />
                                        </div>
                                        <div class="col-12">
                                            <table class="table table-borderless mb-0 table-condensed" style="width: 100%">
                                                <tr>
                                                    <td style="width: 10%" class="p-1">CLIENTE:</td>
                                                    <td style="width: auto" class="p-1">
                                                        <asp:Label ID="lblCliente" CssClass="p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>

                                                    <td style="width: auto" class="pt-1 pb-1 pl-0 text-right">VENDEDOR:</td>
                                                    <td style="width: auto" class="p-1 text-right">
                                                        <asp:Label ID="lblVendedor" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%" class="p-1">FECHA:</td>
                                                    <td style="width: auto" class="p-1">
                                                        <asp:Label ID="lblFechaCotizacion" CssClass="p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>

                                                    <td style="width: auto" class="pt-1 pb-1 pl-0 text-right">TELÉFONO:</td>
                                                    <td style="width: auto" class="p-1 text-right">
                                                        <asp:Label ID="lblTelefonoVendedor" CssClass="p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                            <table class="table table-borderless mb-0" style="width: 100%">
                                                <tr>
                                                    <td style="width: 80%" class="p-1"></td>
                                                    <td style="width: 10%" class="p-1 text-right">CORREO:</td>
                                                    <td style="width: 10%" class="p-1 text-right">
                                                        <asp:Label ID="lblCorreoVendedor" CssClass="p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="col-6 mt-4">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <td class="p-1">Valor del vehiculo</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblValorVehiculo">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Prima</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblPrima">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Monto a Financiar</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblMontoAFinanciar">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Score</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblScore">0</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Tasa mensual</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblTasaMensual">0.00%</asp:Label>
                                                        </td>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div>
                                        <div class="col-6 mt-4">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <td class="p-1" colspan="2">Plazo</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblPlazo"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1" colspan="2">Cuota del préstamo</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblCuotaPrestamo">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">GPS</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblGPS">NO</asp:Label></td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblValorGPS">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">SEGURO</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblSeguro">NO</asp:Label>
                                                        </td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblValorSeguro">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">GASTOS DE CIERRE</td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblGastosDeCierre">NO</asp:Label>
                                                        </td>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblMontoGastosDeCierre">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                </thead>
                                            </table>
                                        </div>
                                        <div class="col-12 mt-2 text-center p-0">
                                            <img src="/Imagenes/Cotizador/image3.png" class="img-fluid" /><br />
                                            <label class="mt-1">Cotización valida únicamente por 5 días y está sujeta a cambios sin previo aviso por parte de Prestadito.</label>
                                        </div>
                                        <div class="col-6 mt-4">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <th class="p-1 text-center">REQUISITOS PARA FINANCIAMIENTO</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td class="p-1 text-center">COPIA DE CÉDULA</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1 text-center">COMPROBAR INGRESOS</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1 text-center">RECIBO PÚBLICO</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1 text-center">CROQUIS DE VIVIENDA</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div class="col-6 mt-4">
                                            <table class="table">
                                                <thead>
                                                    <tr>
                                                        <th class="pt-1 pb-1 pl-0 pr-0 text-center">REQUISITOS PARA FINANCIAMIENTO CON GARANTÍA</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td class="p-1 text-center">COPIA DE CÉDULA</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1 text-center">RECIBO PÚBLICO</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1 text-center">CROQUIS DE VIVIENDA</td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1 text-center">RTN</td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div class="col-12 mt-2 text-center p-0">
                                            <label class="mt-1">Para más información llama al 2540-1050</label>
                                            <h5 class="font-weight-bold">¡Porque no importa la ocasión, PRESTADITO ES LA SOLUCIÓN!</h5>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <!-- PDF DE LA NEGOCIACIÓN (IGNORAR) -->
                <div id="divContenedorNegociacion" style="/*margin-top: 999px; */ display: none;">
                    <asp:UpdatePanel runat="server" ID="upNegociacionPDF" UpdateMode="Always" ChildrenAsTriggers="True">
                        <ContentTemplate>
                            <div class="card m-0 divNegociacionPDF" runat="server" visible="true" id="divNegociacionPDF" style="/*display: none; */">
                                <div class="card-body pt-0">
                                    <div class="row">
                                        <div class="col-6 m-0 p-0">
                                            <img src="/Imagenes/LogoPrestadito.png" />
                                        </div>
                                        <div class="col-6 m-0 p-0 justify-content-end">
                                            <table class="table table-borderless m-0" style="width: 100%">
                                                <tr>
                                                    <td class="p-0 text-right">
                                                        <asp:Label ID="lblOficialNegociosNegociacion" CssClass="p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                                </tr>
                                                <tr runat="server" id="trTelefonoVendedor">
                                                    <td class="p-0 text-right">
                                                        <asp:Label ID="lblTelefonoVendedorNegociacion" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                            <table class="table table-borderless m-0" style="width: 100%">
                                                <tr>
                                                    <td class="p-0 text-right">
                                                        <asp:Label ID="lblCorreoVendedorNegociacion" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </div>
                                        <div class="col-12">
                                            <h5 class="text-center font-weight-bold">NEGOCIACIÓN DE CRÉDITO</h5>
                                            <hr />
                                        </div>
                                        <div class="col-12">
                                            <table class="table table-borderless mb-0" style="width: 100%">
                                                <tr>
                                                    <td style="width: 10%" class="p-1">Cliente:</td>
                                                    <td style="width: 70%" class="p-1">
                                                        <asp:Label ID="lblClienteNegociacion" CssClass="p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>

                                                    <td style="width: 10%" class="p-1 text-right">Teléfono:</td>
                                                    <td style="width: 10%" class="p-1 text-right">
                                                        <asp:Label ID="lblTelefonoClienteNegociacion" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 10%" class="p-1">Identidad:</td>
                                                    <td style="width: 70%" class="p-1">
                                                        <asp:Label ID="lblIdentidadClienteNegociacion" CssClass="p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>

                                                    <td style="width: 10%" class="p-1 text-right">Fecha:</td>
                                                    <td style="width: 10%" class="p-1 text-right">
                                                        <asp:Label ID="lblFechaNegociacion" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label></td>
                                                </tr>
                                            </table>
                                        </div>

                                        <div class="col-12 mt-0 pt-0 mb-0 pb-0">
                                            <h6 class="text-center mb-1 pt-0 pb-0 font-weight-bold">Información financiera del vehículo</h6>
                                            <hr class="mt-0" />
                                        </div>
                                        <div class="col-6">
                                            <table class="table table-bordered">
                                                <thead class="text-center bg-primary text-white">
                                                    <tr>
                                                        <th colspan="2" class="p-1 font-weight-bold">Información del valor a financiar</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td class="p-1">Valor del vehiculo</td>
                                                        <td class="p-1 text-right">
                                                            <asp:Label runat="server" ID="lblValorVehiculoNegociacion">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Prima</td>
                                                        <td class="p-1 text-right">
                                                            <asp:Label runat="server" ID="lblPrimaNegociacion">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Monto a financiar</td>
                                                        <td class="p-1 text-right">
                                                            <asp:Label runat="server" ID="lblMontoFinanciarNegociacion">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Score</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblScoreNegociacion">0</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Plazo</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblPlazoNegociacion"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                        <div class="col-6">
                                            <table class="table table-bordered">
                                                <thead class="text-center bg-primary text-white">
                                                    <tr>
                                                        <th colspan="3" class="p-1 font-weight-bold">Información del financiamiento</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td class="p-1" colspan="2">Tasa mensual</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblTasaMensualNegociacion">0.00%</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1" colspan="2">Cuota del préstamo</td>
                                                        <td class="p-1 text-right">
                                                            <asp:Label runat="server" ID="lblCuotaDelPrestamoNegociacion">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">GPS</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblGPSNegociacion">NO</asp:Label></td>
                                                        <td class="p-1 text-right">
                                                            <asp:Label runat="server" ID="lblValorGPSNegociacion">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Seguro</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblSeguroNegociacion">NO</asp:Label>
                                                        </td>
                                                        <td class="p-1 text-right">
                                                            <asp:Label runat="server" ID="lblValorSeguroNegociacion">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Gastos de cierre</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblGastosDeCierreNegociacion">NO</asp:Label>
                                                        </td>
                                                        <td class="p-1 text-right">
                                                            <asp:Label runat="server" ID="lblMontoGastosDeCierreNegociacion">L 0.00</asp:Label>
                                                        </td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>

                                    <div class="row">
                                        <div class="col-5">
                                            <!-- Características del vehículo -->
                                            <table class="table table-bordered" style="width: 100%">
                                                <thead class="text-center bg-primary text-white">
                                                    <tr>
                                                        <th colspan="2" class="p-1 font-weight-bold">Características del vehículo</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td class="p-1">Marca</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblMarca" Text=""></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Modelo</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblModelo" Text=""></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Año</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblAnio" Text=""></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Matricula</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblMatricula" Text=""></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Color</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblColor" Text=""></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Cilindraje</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblCilindraje" Text=""></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">
                                                            <asp:Label runat="server" ID="lblUnidadDeMedidaRecorrido" Text="Recorrido"></asp:Label></td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblRecorrido" Text=""></asp:Label></td>
                                                    </tr>
                                                    <tr style="display: none;">
                                                        <td class="p-1">Origen</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblOrigenGarantia" Text=""></asp:Label></td>
                                                    </tr>
                                                </tbody>
                                            </table>
                                            <table class="table table-bordered" style="width: 100%">
                                                <thead class="text-center bg-primary text-white">
                                                    <tr>
                                                        <th colspan="2" class="p-1 font-weight-bold">Información del vendedor</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                                    <tr>
                                                        <td class="p-1">Vendedor</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblVendedorGarantia" Text=""></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td class="p-1">Autolote</td>
                                                        <td class="p-1 text-center">
                                                            <asp:Label runat="server" ID="lblAutolote" Text=""></asp:Label></td>
                                                    </tr>
                                                </tbody>
                                            </table>

                                            <!-- Tablas requisitos para financimiento -->
                                            <div class="col-12 p-0" runat="server" visible="false" id="tblRequisitosFinanciamiento">
                                                <table class="table table-bordered text-center pl-0" style="width: 100%">
                                                    <thead class="text-center bg-primary text-white">
                                                        <tr>
                                                            <th class="p-1">Requisitos para financiamiento</th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td class="p-1">Copia de cédula</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="p-1">Comprobar ingresos</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="p-1">Recibo público</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="p-1">Croquis de vivienda</td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>

                                            <!-- Tablas requisitos para financiamiento con garantia -->
                                            <div class="col-12 p-0" runat="server" visible="false" id="tblRequisitosFinanciamientoConGarantia">
                                                <table class="table table-bordered text-center pl-0" style="width: 100%">
                                                    <thead class="text-center bg-primary text-white">
                                                        <tr>
                                                            <th class="pt-1 pb-1 pl-0 pr-0 text-center"><small>Requisitos para financiamiento con garantía</small></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr>
                                                            <td class="p-1">Copia de cédula</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="p-1">Recibo público</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="p-1">Croquis de vivienda</td>
                                                        </tr>
                                                        <tr>
                                                            <td class="p-1">RTN</td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                        <!-- Fotografía del vehiculo -->
                                        <div class="col-7">
                                            <figure class="text-center">
                                                <asp:Image ID="imgVehiculoNegociacion" CssClass="img-fluid" runat="server" Style="max-width: 100%; max-height: 100%;" />
                                                <figcaption>Fotografía del vehículo</figcaption>
                                            </figure>

                                            <!-- Firma del cliente -->
                                            <div class="col-12 align-self-end text-center mt-5">
                                                <label class="mt-5 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                                                <label class="mt-0 ">Firma del cliente</label>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-12 text-center p-0 mt-3">
                                        <label class="">Para más información llama al 2540-1050</label>
                                        <h6 class="font-weight-bold mt-1">¡Porque no importa la ocasión, PRESTADITO ES LA SOLUCIÓN!</h6>
                                    </div>
                                </div>
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>

                <script src="/Scripts/js/jquery.min.js"></script>
                <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
                <script src="/Scripts/app/Negociaciones/Negociaciones_Registrar.js"></script>
                <script type="text/javascript">
                    Sys.Application.add_load(CargarMascarasDeEntrada);
                    Sys.Application.add_load(InicializarCargaDeArchivos);
                </script>
                <script type="text/javascript">
                    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
                    function BeginRequestHandler(sender, args) { var oControl = args.get_postBackElement(); oControl.disabled = true; }
                </script>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdateProgress ID="updateProgress" runat="server">
            <ProgressTemplate>
                <div class="loading">
                    <div class="loader"></div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
    </form>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script src="/Scripts/plugins/html2pdf/html2pdf.bundle.js"></script>
    <script>
        $(document).ready(function () {

            /* Guardar en el frontend identidad del cliente que viene en la URL */
            identidadCliente = <%= "'" + pcID.ToString() + "'" %>;
        });
    </script>
</body>
</html>
