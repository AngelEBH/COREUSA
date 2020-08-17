<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CompromisoLegal.aspx.cs" Inherits="CompromisoLegal" %>

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
            font-size: 22px !important;
        }

        p {
            font-size: 22px !important;
        }

        label {
            font-size: 22px !important;
        }

        .Negrita {
            font-weight: bold !important;
        }

        @font-face {
            font-family: "DejaVu Sans";
            src: url("/Content/fonts/DejaVu/DejaVuSans.ttf") format("truetype");
        }
    </style>
</head>
<body>
    <div class="container-fluid" id="GenerarConvenio">
        <div class="row">
            <div class="col-md-12">
                <div class="form-group row">
                    <div class="col-sm-12">
                        <img alt="" src="/Imagenes/logoPrestadito.png" />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12 justify-content-center">
                        <h1 class="text-center font-weight-bold">COMPROMISO LEGAL
                        </h1>
                        <br />
                    </div>
                </div>
                <div class="form-group row">
                    <div class="col-sm-12">
                        <p>
                            Yo, JOSE RAMIRO RUBIO CHAVARRIA acepto haber adquirido un préstamo en efectivo en la empresa <b>PRESTADITO S.A. de C.V.</b> financiamiento otorgado a 48 meses, para la compra
                            de un Vehículo Automotor con las siguientes características: <b>Marca</b>: BAJAJ <b>modelo:</b> PULSAR NS 125 <b>año:</b> 2020 <b>cilindraje:</b> 125 <b>color:</b> NEGRO MULTICOLOR 
                            <b>tipo: </b>MOTOCICLETA <b>motor:</b> JEYWKC38252 el cual usaré como transporte personal y quedará como garantía del financiamiento otorgado. Por lo que, durante el plazo del 
                            financiamiento del vehiculo automotor, soy el único responsable por todo acto de carácter legal o ilegal que se encuentre involucrado el automotor, liberando de cualquier responsabilidad
                            a la empresa antes mencionada.
                        </p>
                        <p>
                            Así mismo, entiendo que:
                        </p>
                        <p>
                            La garantíade dicho vehículo corresponde <b>exclusivamente</b> al distribuidor o concesionario donde fue adquirido; por lo tanto, <b>PRESTADITO S.A. de C.V.</b> no se hace responsable
                            de la garantía, la cual funciona de acuerdo a políticas y restricciones del distribuidor.
                        </p>
                        <p>
                            El seguro de daños que incluye el préstamo es válida única y exclusivamente durante el tiempo del financiamiento, siempre y cuando las cuotas estén al día.
                        </p>

                        <p>
                            Extiendo y firmo el siguiente documento en la ciudad de SAN PEDRO SULA, CORTÉS, a los 27 días del mes de febrero del año 2020.
                        </p>
                    </div>
                </div>
            </div>
        </div>

        <br />
        <br />
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
                <label style="font-size: 18px !important;" class="text-center">JOSE RAMIRO RUBIO CHAVARRIA</label>
            </div>
            <div class="col-sm-1"></div>
            <div class="col-sm-4 text-center">
                <label style="font-size: 18px !important;">No.Identidad</label>
            </div>
        </div>
    </div>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script>
        $(function () {
            window.print();
        })
    </script>
</body>
</html>
