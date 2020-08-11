<%@ Page Language="C#" AutoEventWireup="true" Inherits="Gestion_ImprimirAdendum31a120" Codebehind="ImprimirAdendum31a120.aspx.cs" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="/CSS/Estilos.css"/>
    <link rel="stylesheet" type="text/css" href="/CSS/Imprimir.css"/>
    <style type="text/css" media="print">
        @page { margin-top: 0; margin-bottom: 0;  margin-left: 40px; margin-right: 40px;}
        @font-face {font-family: BCTFB;src: url(BARCODETFB.ttf);}
    </style>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <script type="text/javascript">
        function codeAddress() {
            window.print();
        }
        window.onload = codeAddress;
    </script>
</head>
<body>
    <form id="form1" runat="server" style="background-color:white;">
            <asp:Panel CssClass="FormatoPanelContenedorNOAB"  ID ="Panel1" runat="server" style="width:880px; ">
                <div style="margin-top:30px; margin-left:5px; margin-right:5px;">
                    <table>
                        <tr>
                            <td rowspan="4">
                                <img alt="" src="/Imagenes/logoPrestadito.png" />
                            </td>
                            <td class="ClaseTDDerechaGrande">Prestadito S.A. de C.V.</td>
                        </tr>
                        <tr>
                            <td class="ClaseTDDerecha">Barrio Rio de Piedras</td>
                        </tr>
                        <tr>
                            <td class="ClaseTDDerecha">24 avenida 4ta calle S.O.</td>
                        </tr>
                        <tr>
                            <td class="ClaseTDDerecha">Telefono 2504-3434</td>
                        </tr>
                    </table>
                    <table>
                        <tr><td>&nbsp;</td></tr>
                        <tr><td class="ClaseTDC" style="font-size:20px;">ADENDUM</td></tr>
                        <tr><td style="font-size:10px;">&nbsp;</td></tr>
                    </table>
                </div>
                <div style="margin-left:5px; margin-right:5px;">
                    <table>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ"><p>El deudor <asp:label runat="server" CssClass="EquitetaBold" ID="lblNombreCliente" Text=""/> &nbsp;con número de identidad: <asp:label runat="server" CssClass="EquitetaBold" ID="lblIdentidad" Text=""/> y el acreedor <span class="ParBold">PRESTADITO S.A. de C.V.</span> suscribimos, a través de este documento, un "Convenio de pago" para alargamiento del plazo que establece lo siguiente:</p></td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ"><p><span class="ParBold">Primero: </span> En virtud que el(a) señor(a).: <asp:label runat="server" CssClass="EquitetaBold" ID="lblNombreCliente2" Text=""/> , no cuenta actualmente con los recursos económicos y la capacidad de pago que disponía cuando se solicitó el préstamo y con el ánimo de honrar la deuda, se solicita un convenio de pago que le permita congelar la cuenta de forma parcial y poner la cuenta al día a través de cuotas, comprometiéndome en este acto a un primer pago de: <asp:label runat="server" CssClass="EquitetaBold" ID="lblPrimerPago1" Text=""/> &nbsp;Lempiras.</p></td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ"><p><span class="ParBold">Segundo: </span> Analizando el atraso y la solicitud antes expuesta por El(a) Señor(a): <asp:label runat="server" CssClass="EquitetaBold" ID="lblNombreCliente3" Text=""/> , la Empresa PRESTADITO S.A de C.V. excepcionalmente, se ve en la necesidad de acceder en lo que el(a) deudor(a) ha solicitado y acepta la propuesta.</p></td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ"><p>Constando en este acto que se recibirá la cantidad de <asp:label runat="server" CssClass="EquitetaBold" ID="lblPago2" Text=""/> &nbsp;como pago inicial para realizar el convenio, y consta que la condición para que no surta sus efectos será: <span class="ParBold">la falta de pago de una sola cuota, la parte deudora se compromete a </span>no tener ningún atraso en este convenio, caso contrario , PRESTADITO S.A de C.V. , se reserva el derecho de reclamar todo lo adeudado por la vía legal correspondiente y los pagos realizados objeto de este convenio, se aplicaran como pagos normales, sin condonación de intereses.</p></td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                    </table>
                </div>
                <div style="margin-left:5px; margin-right:5px;">
                    <table>
                        <tr><td class="ClaseTDC"><asp:Label runat="server" ID="lblCosto" style="left:  260px; top:108px; ">Desglose del Convenio</asp:Label>                        </td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <table style="width:100%;">
                                    <tr>
                                        <td class="ClaseTDDerechaBold" style="vertical-align:bottom;" >Total atrasado:</td>
                                        <td class="ClaseTDBUR" style="vertical-align:bottom; height:24px; width:150px;"><asp:Label runat="server" ID="lblTotalAdeudado" style="left:  260px; top:108px; " Text=""/></td>
                                        <td style="width:50px;">&nbsp;</td>
                                        <td class="ClaseTDDerechaBold" style="vertical-align:bottom;">Abono inicial:</td>
                                        <td class="ClaseTDBUR" style="width:150px;"><asp:Label runat="server" ID="lblDescuento" style="left:  260px; top:108px; " Text=""/></td>
                                        <td style="width:100px;">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ClaseTDDerechaBold" style="vertical-align:bottom;">Saldo después 1er abono:</td>
                                        <td class="ClaseTDBUR" style="vertical-align:bottom; height:24px; width:150px;"><asp:Label runat="server" ID="lblSaldoDespuesPrimerAbono" style="left:  260px; top:108px; " Text=""/></td>
                                        <td style="width:50px;">&nbsp;</td>
                                        <td class="ClaseTDDerechaBold" style="vertical-align:bottom;">Tiempo de convenio:</td>
                                        <td class="ClaseTDBUR" style="vertical-align:bottom; width:150px;"><asp:Label runat="server" ID="lblTiempodeConvenio" style="left:  260px; top:108px; " Text=""/></td>
                                        <td style="width:100px;">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ClaseTDDerechaBold" style="vertical-align:bottom; height: 24px">Cuota:</td>
                                        <td class="ClaseTDBUR" style="vertical-align:bottom; height:24px; width:150px; height: 19px;"><asp:Label runat="server" ID="lblCuota" style="left:  260px; top:108px; " Text=""/></td>
                                        <td style="width:50px; height: 19px;">&nbsp;</td>
                                        <td class="ClaseTDDerechaBold" style="vertical-align:bottom; height: 19px">Frecuencia de pago:</td>
                                        <td class="ClaseTDBUR" style="vertical-align:bottom; width:150px; height: 19px;"><asp:Label runat="server" ID="lblFrecuenciadePago" style="left:  260px; top:108px; " Text=""/></td>
                                        <td style="width:100px; height: 19px;">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ClaseTDDerechaBold" style="vertical-align:bottom;">Fecha inicial del convenio:</td>
                                        <td class="ClaseTDBUR" style="vertical-align:bottom; height:24px; width:150px;"><asp:Label runat="server" ID="lblInicioConvenio" style="left:  260px; top:108px; " Text=""/></td>
                                        <td style="width:50px;">&nbsp;</td>
                                        <td class="ClaseTDDerechaBold" style="vertical-align:bottom;">Fecha final del convenio:</td>
                                        <td class="ClaseTDBUR" style="vertical-align:bottom; width:150px;"><asp:Label runat="server" ID="lblFinConvenio" style="left:  260px; top:108px; " Text=""/></td>
                                        <td style="width:100px;">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div style="margin-left:5px; margin-right:5px;">
                    <table>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ"><p>Me doy por enterado y entendido que el tiempo que  dure este convenio  se extenderá el plazo de mí contrato  la cantidad  de  <asp:label runat="server" ID="lblQuincenasConvenio" Text=""/> Quincenas tomando en cuenta la última cuota especificada en mi contrato y plan de pagos.</p></td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ"><p>El presente convenio se establece y firma para constancia, el día: <asp:label runat="server" ID="lblDiadeFirma" Text=""/> en la ciudad de <asp:label runat="server" ID="lblCiudadAgencia" Text=""/>, Departamento de <asp:label runat="server" ID="lblDepartamentoAgencia" Text=""/>.</p></td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                    </table>
                </div>
                <div style="margin-left:5px; margin-right:5px;">
                    <table>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">
                                <table style="width:100%;">
                                    <tr>
                                        <td style="width:100px; height: 16px;"></td>
                                        <td class=".ClaseTDBU" >_______________________________________________</td>
                                        <td style="width:50px; height: 16px;"></td>
                                        <td class=".ClaseTDBU" >_______________________________________________</td>
                                        <td style="width:100px; height: 16px;"></td>
                                    </tr>
                                    <tr>
                                        <td style="width:100px;">&nbsp;</td>
                                        <td class=".ClaseTDBU" style="width:200px; text-align: center;">Firma del cliente</td>
                                        <td style="width:50px;">&nbsp;</td>
                                        <td class=".ClaseTDBU" style="width:200px; text-align: center;">Prestadito</td>
                                        <td style="width:100px;">&nbsp;</td>
                                    </tr>
                                </table>
                        </td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ">&nbsp;</td></tr>
                        <tr><td class="ClaseTDJ"><asp:label runat="server" ID="lblCodigoDeBarra" Text="Codigo: 2019120001"/> </td></tr>
                    </table>
                </div>
            </asp:Panel>
    </form>
</body>
</html>
