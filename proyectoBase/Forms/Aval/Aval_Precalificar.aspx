<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Aval_Precalificar.aspx.cs" Inherits="proyectoBase.Forms.Aval.Aval_Precalificar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <link rel="shortcut icon" href="/Content/images/favicon.ico" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <title>Precalificar Aval</title>
    <!-- BOOTSTRAP -->
    <link href="../../Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/css/metismenu.min.css" rel="stylesheet" />
    <link href="../../Content/css/icons.css" rel="stylesheet" />
    <link href="../../Content/css/style.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/morris/morris.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="../../Scripts/plugins/iziToast/css/iziToast.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="../../CSS/Estilos_CSS.css" rel="stylesheet" />
    <script>
        function resizeIframe(obj) {
            obj.style.height = obj.contentWindow.document.documentElement.scrollHeight + 'px';
        }
    </script>
</head>
<body class="EstiloBody">
    <div id="wrapper">
        <form id="form1" runat="server">
            <div class="row">
                <div class="col-12">
                    <div class="card m-b-20">
                        <div class="card-body">
                            <h3>Precalificar Aval</h3>
                            <iframe src="/Forms/Precalificado/precalificado_buscador.aspx?KkzxdajCEScB5etxIfZ1xw==" frameborder="0" width="100%" scrolling="no" onload="resizeIframe(this)"></iframe>
                        </div>
                    </div>
                </div>
            </div>
        </form>
    </div>
    <script src="../../Scripts/js/jquery.min.js"></script>
    <script src="../../Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="../../Scripts/js/metisMenu.min.js"></script>
    <script src="../../Scripts/js/jquery.slimscroll.js"></script>
    <script src="../../Scripts/js/waves.min.js"></script>
    <script src="../../Scripts/plugins/jquery-sparkline/jquery.sparkline.min.js"></script>
    <script src="../../Scripts/js/app.js"></script>
    <script src="../../Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(".DropDownList").select2({
            language: {
                errorLoading: function () { return "No se pudieron cargar los resultados" },
                inputTooLong: function (e) { var n = e.input.length - e.maximum, r = "Por favor, elimine " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
                inputTooShort: function (e) { var n = e.minimum - e.input.length, r = "Por favor, introduzca " + n + " car"; return r += 1 == n ? "ácter" : "acteres" },
                loadingMore: function () { return "Cargando más resultados…" },
                maximumSelected: function (e) { var n = "Sólo puede seleccionar " + e.maximum + " elemento"; return 1 != e.maximum && (n += "s"), n },
                noResults: function () { return "No se encontraron resultados" },
                searching: function () { return "Buscando…" },
                removeAllItems: function () { return "Eliminar todos los elementos" }
            }
        });
    </script>
    <script src="../../Scripts/plugins/iziToast/js/iziToast.js"></script>
    <script src="../../Scripts/plugins/iziToast/js/iziToast.min.js"></script>
</body>
</html>