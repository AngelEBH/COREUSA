<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GestionCobranzaClienteDocumentos.aspx.cs" Inherits="GestionCobranzaClienteDocumentos" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Documentos</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <style>
        html {
            background-color: rgb(255,255,255) !important;
        }

        .card {
            border: none;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }

        .container {
            position: relative;
            width: 100%;
            overflow: hidden;
            padding-top: 75%; /* 4:3 Aspect Ratio */
        }

        .responsive-iframe {
            position: absolute;
            top: 0;
            left: 0;
            bottom: 0;
            right: 0;
            width: 100%;
            height: 100%;
            border: none;
        }
    </style>
</head>
<body class="EstiloBody">
    <form id="frmPrincipal" runat="server">
        <div class="card m-0">
            <div class="card-header pb-1 pt-1">
                <div class="float-right p-1" id="Loader" style="display: none;">
                    <div class="spinner-border" role="status">
                        <span class="sr-only"></span>
                    </div>
                </div>
                <h6>Documentos del cliente
                    <asp:Label ID="lblCodigoCliente" class="font-weight-bold" runat="server"></asp:Label></h6>
            </div>
            <div class="card-body">
                <div class="row" id="divInformacionGarantia" runat="server">
                    <div class="col-lg-3">
                        <div class="form-group row">
                            <div class="col-12">
                                <table id="tblDocumentos" class="table table-striped table-sm" runat="server">
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="col-lg-9 border-left border-gray">
                        <h6 class="mt-0">Vista previa</h6>
                        <div class="form-group row">
                            <div class="col-12">
                                <div class="container">
                                    <iframe class="responsive-iframe" name="imgbox" id="imgbox"></iframe>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="form-group row mr-0 ml-0 alert alert-danger" runat="server" id="PanelMensajeErrores" visible="false">
                    <asp:Label CssClass="col-sm-12 col-form-label text-danger p-0" ID="lblMensaje" Text="" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
</body>
</html>
