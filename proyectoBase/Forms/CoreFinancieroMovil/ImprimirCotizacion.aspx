<%@ Page Language="C#" AutoEventWireup="true" Inherits="ImprimirCotizacion" CodeFile="ImprimirCotizacion.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title></title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <style>
        html, body {
            background-color: #fff;
        }

        .card {
            box-shadow: none;
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
        <!-- PDF COTIZACIÓN-->
        <div>
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
        </div>
        <footer>
            <div class="row h-100">
                <div class="col-12 justify-content-end">
                    Impreso por
                    <asp:Label runat="server" ID="lblUsuarioImprime"></asp:Label>
                </div>
            </div>
        </footer>
    </form>
    <script>
    <!-- Imprimir cuando se haya terminado de cargar -->
    window.onload = function () {
        window.print();
    };

    window.onafterprint = function () {
        //window.close();
        window.history.go(-1);
    }
    </script>
</body>
</html>
