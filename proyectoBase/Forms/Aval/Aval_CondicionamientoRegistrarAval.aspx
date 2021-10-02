<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Aval_CondicionamientoRegistrarAval.aspx.cs" Inherits="Aval_CondicionamientoRegistrarAval" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Aval</title>
    <!-- BOOTSTRAP -->
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css?v=202010031053" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <style>
        html {
            background-color: #d9d9d9;
        }
    </style>
    <script>
        function resizeIframe(obj) {
            obj.style.height = obj.contentWindow.document.documentElement.scrollHeight + 'px';
        }
    </script>
</head>
<body style="border: solid; border-width: 1px; border-color: lightgray; margin-bottom: 0px; padding-bottom: 0px;">

    <form id="form1" runat="server">
        <div class="row">
            <div class="col-12">
                <div class="card" style="margin-bottom: 0px; padding-bottom: 0px;">
                    <div class="card-body" style="margin-bottom: 0px; padding-bottom: 0px;">
                        <h4 class="text-center">Registrar aval para el cliente:&nbsp;<asp:Label ID="lblNombreCliente" runat="server"></asp:Label> | Solicitud: &nbsp;<asp:Label ID="lblIDSolicitud" runat="server"></asp:Label></h4>
                        <iframe src="../Precalificado/precalificado_buscador.aspx?wl3VBR6+Gyrpgt20vUhH0HCyFLJKHEkhqPQb/l1KZlg=" frameborder="0" width="100%" scrolling="no" onload="resizeIframe(this)"></iframe>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
</body>
</html>
