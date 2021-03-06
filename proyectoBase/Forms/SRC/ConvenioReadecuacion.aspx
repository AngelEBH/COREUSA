<%@ Page Language="C#" AutoEventWireup="true" Inherits="ConvenioReadecuacion" CodeFile="ConvenioReadecuacion.aspx.cs" %>

<!DOCTYPE html>
<html lang="es" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/sweet-alert2/sweetalert2.min.css" rel="stylesheet" />
    <style type="text/css">
        html * {
            font-family: "DejaVu Sans", "Arial", sans-serif !important;
            font-size: 14px !important;
        }

        p {
            font-size: 14px !important;
        }

        label {
            font-size: 14px !important;
        }

        .Negrita {
            font-weight: bold !important;
        }

        div {
            font-family: "DejaVu Sans", "Arial", sans-serif;
            font-size: 12px;
        }

        @font-face {
            font-family: "DejaVu Sans";
            src: url("/Content/fonts/DejaVu/DejaVuSans.ttf") format("truetype");
        }

        @font-face {
            font-family: "DejaVu Sans";
            font-weight: bold;
            src: url("/Content/fonts/DejaVu/DejaVuSans-Bold.ttf") format("truetype");
        }
    </style>
</head>
<body>
    <div class="container-fluid" id="GenerarConvenio">
        <div class="row">
            <div class="col-sm-12 mb-3">
                <img alt="" src="/Imagenes/logoPrestadito.png" />
            </div>
            <div class="col-sm-12 justify-content-center mb-3">
                <h5 class="text-center">ADENDUM
                    <asp:Label runat="server" ID="lblNoAdendum" Text="" /><br />
                    POR READECUACION
                </h5>
            </div>
            <div class="col-md-12">
                <div class="form-group row">
                    <div class="col-sm-12">
                        <p>
                            El señor(a)
                            <asp:Label CssClass="font-weight-bold" runat="server" ID="lblNombreCliente" Text="" />
                            con número de identidad
                            <asp:Label runat="server" CssClass="font-weight-bold" ID="lblIdentidadCliente" Text="" />
                            y el acreedor
                            <span class="font-weight-bold">PRESTADITO S.A. de C.V.</span>
                            acuerdan hacer la readecuación de su préstamo No.
                            <asp:Label CssClass="font-weight-bold" runat="server" ID="lblNoPrestamo" Text="" />
                            a un nuevo préstamo el cual tendrá las siguientes condiciones:
                        </p>
                    </div>

                    <label class="col-sm-5">Abono inicial</label>
                    <asp:Label CssClass="col-sm-5" runat="server" ID="lblAbonoInicial" Style="font-weight: bold; font-size: 12px;" Text="" />

                    <label class="col-sm-5" runat="server" id="lblTipoCuotaTabla">Cuota quincenal</label>
                    <asp:Label CssClass="col-sm-5" runat="server" ID="lblCantidadCuotas" Style="font-weight: bold; font-size: 12px;" Text="" />

                    <label class="col-sm-5">Plazo readecuación</label>
                    <asp:Label class="col-sm-5" runat="server" ID="lblPlazoReadecuacion" Style="font-weight: bold; font-size: 12px;" Text="" />

                    <label class="col-sm-5">Monto a readecuar</label>
                    <asp:Label class="col-sm-5" runat="server" ID="lblMontoReadecuar" Style="font-weight: bold; font-size: 12px;" Text="" />

                    <label class="col-sm-5">Fecha inicial contrato</label>
                    <asp:Label class="col-sm-5" runat="server" ID="lblFechaInicialContrato" Style="font-weight: bold; font-size: 12px;" Text="" />

                    <label class="col-sm-5">Fecha final contrato</label>
                    <asp:Label class="col-sm-5" runat="server" ID="lblFechaFinalContrato" Style="font-weight: bold; font-size: 12px;" Text="" />
                </div>
            </div>
            <div class="col-md-12">
                <div class="form-group row">
                    <div class="col-sm-12">
                        <p>
                            El presente adendum pasa a formar parte del convenio de crédito, modifica el plazo, cantidad a pagar y cantidad de cuotas.
                        </p>
                        <p>
                            El cliente empezará a pagar su primera cuota
                            <asp:Label runat="server" ID="lblTipodeCuota" Text="quincenal" />
                            a partir de la fecha
                            <asp:Label runat="server" ID="lblFechaPrimeraCuota" Text="" />
                            y finalizará su última cuota el
                            <asp:Label runat="server" ID="lblFechaUltimaCuota" Text="" />
                        </p>
                        <p>
                            El cliente se da por enterado y acepta cada una de las modificaciones que en este documento se estipulan, en fe de lo cual se firma
                            en la ciudad de
                            <asp:Label runat="server" ID="lblCiudad" Text="" />,
                            Departamento de
                            <asp:Label runat="server" ID="lblDepartamento" Text="" />
                            a los
                            <asp:Label runat="server" ID="lblDias" Text="" />
                            dias del mes de
                            <asp:Label runat="server" ID="lblMes" Text="" />
                            del año
                            <asp:Label runat="server" ID="lblAnio" Text="" />.
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-md-12">
                <div class="form-group row">
                    <div class="col-sm-12">
                        <br />
                        <div class="form-group row justify-content-center">
                            <div class="col-sm-4">
                                <div class="border border-dark"></div>
                            </div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-4">
                                <div class="border border-dark"></div>
                            </div>
                            <div class="col-sm-4 text-center">
                                <label class="text-center">Firma del cliente</label>
                            </div>
                            <div class="col-sm-1"></div>
                            <div class="col-sm-4 text-center">
                                <label>Firma Prestadito</label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/kendo/jszip.min.js"></script>
    <script src="/Scripts/plugins/kendo/kendo.all.min.js"></script>
    <script src="/Scripts/plugins/sweet-alert2/sweetalert2.min.js"></script>
    <script>
        function ExportHtmlToPdf(element, docName) {

            var r = $.Deferred();

            kendo.drawing.drawDOM(element,
                {
                    forcePageBreak: ".page-break", // add this class to each element where you want manual page break
                    paperSize: "letter",
                    margin: { top: "1.5cm", bottom: "1cm", right: "0.5cm", left: "0.5cm" }
                })
                .then(function (group) {
                    kendo.drawing.pdf.saveAs(group, docName + ".pdf")
                });
            return r;
        }

        var CerrarVentana = function () {
            Swal.fire(
                {
                    title: 'Descarga Exitosa!',
                    text: 'El archivo se guardó en la carpeta de descargas!',
                    type: 'success',
                    showCancelButton: false,
                    confirmButtonColor: "#58db83",
                    confirmButtonText: "OK"
                }
            ).then(function () {
                //window.close();
            });
        };

        $(document).ready(function () {
            var NombrePDF = '<%= this.lcNombreArchivoPDF %>';
            ExportHtmlToPdf('#GenerarConvenio', NombrePDF).done(CerrarVentana());
        });
    </script>
</body>
</html>
