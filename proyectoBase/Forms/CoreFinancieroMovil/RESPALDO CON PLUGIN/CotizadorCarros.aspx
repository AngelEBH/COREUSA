﻿<%@ Page Language="C#" AutoEventWireup="true" Inherits="Clientes_CotizadorCarros" CodeFile="CotizadorCarros.aspx.cs" %>

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

        footer {
            position: absolute;
            bottom: 0;
            width: 100%;
            height: 2.5rem;
        }
    </style>
</head>
<body>
    <form id="Form1" runat="server">
        <asp:ScriptManager runat="server" ID="smMultiview"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="upMultiView">
            <ContentTemplate>

                <%--<iframe src="http://190.92.0.76/CoreMovil/Cotizadores/Cotizacion_24_9_2020_17_21_08.zip"></iframe>--%>


                <div class="card m-0">
                    <div class="card-body pt-0">
                        <div runat="server" style="padding-top: 1.25rem;" id="divParametros" visible="true">
                            <div class="form-group form-row">
                                <div class="col">
                                    <label class="col-form-label">Valor del automovil</label>
                                    <asp:TextBox ID="txtValorVehiculo" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="100000" runat="server"></asp:TextBox>
                                </div>
                                <div class="col" id="divPrima" runat="server" visible="true">
                                    <label class="col-form-label">Valor de la Prima</label>
                                    <asp:Label CssClass="" ID="lblPorcenajedePrima" runat="server" Text="" Visible="false" />
                                    <asp:TextBox ID="txtValorPrima" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" Text="20000" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group" runat="server" id="divMontoFinanciarVehiculo" visible="false">
                                <label class="col-form-label">Valor del vehiculo a financiar</label>
                                <asp:TextBox ID="txtMonto" Enabled="false" type="tel" CssClass="form-control form-control-sm col-form-label MascaraCantidad" required="required" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <label class="col-form-label">Score Promedio</label>
                                <asp:TextBox ID="txtScorePromedio" type="tel" CssClass="form-control form-control-sm col-form-label MascaraNumerica" required="required" Text="999" runat="server"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbFinanciamiento" runat="server" GroupName="rbTipoFinanciamiento" OnCheckedChanged="rbFinanciamiento_CheckedChanged" AutoPostBack="true" />
                                        <label class="form-check-label">Financiamiento</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <asp:RadioButton CssClass="form-check-input" ID="rbEmpeno" runat="server" GroupName="rbTipoFinanciamiento" OnCheckedChanged="rbEmpeno_CheckedChanged" AutoPostBack="true" />
                                        <label class="form-check-label">Empeño</label>
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
                        </div>

                        <br />
                        <!-- Resultados de la cotización -->
                        <div runat="server" id="PanelCreditos1" visible="false">
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
                                    <a class="nav-link" data-toggle="tab" href="#GastosDeCierreFinanciados" role="tab" aria-selected="false">
                                        <span class="d-block d-sm-none">Gastos de cierre
                                            <br />
                                            financiados</span>
                                        <span class="d-none d-sm-block"><small>Gastos de cierre financiados</small></span>
                                    </a>
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
                        </div>
                        <!-- Nueva cotización -->
                        <div class="form-group row m-0" runat="server" id="divNuevoCalculo" visible="false">
                            <div class="button-items col-sm-12 p-0">
                                <asp:Button ID="btnNuevoCalculo" Text="Nuevo cálculo" CssClass="btn btn-primary btn-lg btn-block waves-effect waves-light m-0" runat="server" OnClick="btnNuevoCalculo_Click" />
                            </div>
                        </div>
                        <!-- Label para mensajes y advertencias -->
                        <div class="form-group row m-0" runat="server" id="PanelMensajeErrores">
                            <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>



                <!-- COTIZACIÓN PDF -->
                <div class="card m-0 divCotizacionPDF" runat="server" visible="true" id="divCotizacionPDF" style="font-size: 18px !important;">
                    <div class="card-body pt-0">
                        <div class="row">
                            <div class="col-12 text-center">
                                <img src="/Imagenes/LogoPrestaditoGrande.png" />
                                <h1>COTIZACIÓN</h1>
                                <hr />
                            </div>

                            <div class="col-5">
                                <div class="form-group row mb-0">
                                    <label class="col-4">CLIENTE:</label>
                                    <asp:Label ID="lblCliente" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label>
                                </div>
                                <div class="form-group row mb-0">
                                    <label class="col-4">FECHA:</label>
                                    <asp:Label ID="lblFechaCotizacion" CssClass="col-8 p-0 font-weight-bold" Text="" runat="server"></asp:Label>
                                </div>
                            </div>
                            <div class="col-7">
                                <div class="form-group row mb-0 justify-content-end">
                                    <label class="col-3">VENDEDOR:</label>
                                    <asp:Label ID="lblVendedor" CssClass="col-6 p-0 font-weight-bold" Text="" runat="server"></asp:Label>
                                </div>
                                <div class="form-group row mb-0 justify-content-end">
                                    <label class="col-3">TELÉFONO:</label>
                                    <asp:Label ID="lblTelefonoVendedor" CssClass="col-6 p-0 font-weight-bold" Text="" runat="server"></asp:Label>
                                </div>
                                <div class="form-group row mb-0 justify-content-end">
                                    <label class="col-3">CORREO:</label>
                                    <asp:Label runat="server" ID="lblCorreoVendedor" CssClass="col-6 p-0 font-weight-bold" Text=""></asp:Label>
                                </div>
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
                                            <th class="p-1">REQUISITOS PARA FINANCIAMIENTO</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class="p-1">COPIA DE CÉDULA</td>
                                        </tr>
                                        <tr>
                                            <td class="p-1">COMPROBAR INGRESOS</td>
                                        </tr>
                                        <tr>
                                            <td class="p-1">RECIBO PÚBLICO</td>
                                        </tr>
                                        <tr>
                                            <td class="p-1">CROQUIS DE VIVIENDA</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="col-6 mt-4">
                                <table class="table">
                                    <thead>
                                        <tr>
                                            <th class="p-1">REQUISITOS PARA FINANCIAMIENTO CON GARANTÍA</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td class="p-1">COPIA DE CÉDULA</td>
                                        </tr>
                                        <tr>
                                            <td class="p-1">RECIBO PÚBLICO</td>
                                        </tr>
                                        <tr>
                                            <td class="p-1">CROQUIS DE VIVIENDA</td>
                                        </tr>
                                        <tr>
                                            <td class="p-1">RTN</td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="col-12 mt-2 text-center p-0">
                                <label class="mt-1">Para más información llama al 2540-1050</label>
                                <h3 class="font-weight-bold">¡Porque no importa la ocasion, PRESTADITO ES LA SOLUCION!</h3>
                            </div>
                        </div>
                    </div>
                </div>
                <footer>
                    <div class="row h-100">
                        <div class="col-12 justify-content-end">
                            Impreso por
                    <asp:Label runat="server" ID="lblUsuarioImprime"></asp:Label>
                        </div>
                    </div>
                </footer>

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

    <!-- jQuery -->
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(function () {
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
            $(".MascaraNumerica").inputmask("decimal", {
                alias: "numeric",
                groupSeparator: ",",
                digits: 0,
                integerDigits: 3,
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
            $(".identidad").inputmask("9999999999999");
        });
    </script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2pdf.js/0.9.2/html2pdf.bundle.js"></script>

    <script>
        // Exportar cotización a PDF
        function ExportToPDF(fileName) {
            const cotizacion = this.document.getElementById("divCotizacionPDF");
            var opt = {
                margin: 0,
                filename: fileName + '.pdf',
                image: { type: 'jpeg', quality: 0.98 },
                html2canvas: { scale: 1 },
                jsPDF: { unit: 'in', format: 'letter', orientation: 'portrait' }
            };
            html2pdf().from(cotizacion).set(opt).save();
        }
    </script>
</body>
</html>