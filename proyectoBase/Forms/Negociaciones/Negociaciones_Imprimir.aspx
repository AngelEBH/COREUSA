<%@ Page Language="C#" AutoEventWireup="true" Inherits="Negociaciones_Imprimir" CodeFile="Negociaciones_Imprimir.aspx.cs" %>

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
        <!-- PDF -->
        <div>
            <div class="card m-0 divCotizacionPDF" runat="server" visible="true" id="divCotizacionPDF" style="font-size: 18px !important;">
                <div class="card-body pt-0">
                    <div class="row">
                        <div class="col-12 text-center">
                            <img src="/Imagenes/LogoPrestaditoGrande.png" />
                            <h2>NEGOCIACIÓN DE CRÉDITO</h2>
                            <hr />
                        </div>
                        <div class="col-12">
                            <table class="table table-borderless mb-0" style="width: 100%">
                                <tr>
                                    <td style="width: 10%" class="p-1">CLIENTE:</td>
                                    <td style="width: 70%" class="p-1">
                                        <asp:Label ID="lblCliente" CssClass="p-0 font-weight-bold" Text="Willian Onandy Diaz Serrano" runat="server"></asp:Label></td>

                                    <td style="width: 10%" class="p-1 text-right">TELÉFONO:</td>
                                    <td style="width: 10%" class="p-1 text-right">
                                        <asp:Label ID="lblTelefonoCliente" CssClass="col-8 p-0 font-weight-bold" Text="9611-6376" runat="server"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 10%" class="p-1">IDENTIDAD:</td>
                                    <td style="width: 70%" class="p-1">
                                        <asp:Label ID="Label1" CssClass="p-0 font-weight-bold" Text="0502-2000-02944" runat="server"></asp:Label></td>

                                    <td style="width: 10%" class="p-1 text-right">FECHA:</td>
                                    <td style="width: 10%" class="p-1 text-right">
                                        <asp:Label ID="lblFechaCotizacion" CssClass="col-8 p-0 font-weight-bold" Text="29/09/2020" runat="server"></asp:Label></td>
                                </tr>
                            </table>
                            <table class="table table-borderless mt-0" style="width: 100%">
                                <tr>
                                    <td style="width: 20%" class="p-1">OFICIAL DE NEGOCIOS:</td>
                                    <td style="width: 50%" class="p-1">
                                        <asp:Label ID="lblVendedor" CssClass="p-0 font-weight-bold" Text="José Flores" runat="server"></asp:Label></td>

                                    <td style="width: 20%" class="p-1">CENTRO DE COSTO:</td>
                                    <td style="width: 10%" class="p-1 text-right">
                                        <asp:Label ID="lblCentroDeCosto" CssClass="col-8 p-0 font-weight-bold" Text="MATRIZ" runat="server"></asp:Label></td>
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
                    </div>

                    <div class="row mt-3">
                        <div class="col-5">
                            <table class="table">
                                <thead class="text-center">
                                    <tr>
                                        <th colspan="2" class="p-1 font-weight-bold">VEHÍCULO</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <tr>
                                        <td class="p-1">MARCA</td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblMarca" Text="TOYOTA"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">MODELO</td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblModelo" Text="COROLLA"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">AÑO</td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblAnio" Text="2015"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">MATRICULA</td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblMatricula" Text="HAF 3874"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">COLOR</td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblColor" Text="BLANCO"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">CILINDRAJE</td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblCilindraje" Text="1,800"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">
                                            <asp:Label runat="server" ID="lblUnidadDeMedidaRecorrido" Text="MILLAJE"></asp:Label></td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblRecorrido" Text="20000"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">ORIGEN</td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblOrigenGarantia" Text="FACEBOOK"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">VENDEDOR</td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblVendedorGarantia" Text="JUAN PEREZ CASTRO"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td class="p-1">AUTOLOTE</td>
                                        <td class="p-1 text-center">
                                            <asp:Label runat="server" ID="lblAutolote" Text="INTERNO"></asp:Label></td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                        <div class="col-7">
                            <figure class="text-center">
                                <asp:Image ID="Image1" CssClass="img-fluid" runat="server" src="/Imagenes/pruebaImagen.jpg" Style="max-width: 100%; max-height: 100%;" />
                                <figcaption>Fotografía del vehículo</figcaption>
                            </figure>
                        </div>
                    </div>
                    <div class="row mt-3 align-items-end">
                        <div class="col-5">
                            <div class="col-12" runat="server" visible="true">
                                <table class="table text-center">
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
                            <div class="col-12" runat="server" visible="false">
                                <table class="table text-center">
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
                        </div>
                        <div class="col-7 text-center">
                            <label class="mt-1 form-control border-top-0 border-left-0 border-right-0 border-dark" style="border-radius: 0px;"></label>
                            <label class="mt-0 ">FIRMA DEL CLIENTE</label>
                        </div>
                    </div>
                    <div class="col-12 mt-5 text-center p-0">
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
