<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CFRM_IniciarSesion.aspx.cs" Inherits="CFRM_IniciarSesion" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Iniciar sesión</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/icons.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <style>
        html {
            background-color: rgb(255,255,255) !important;
        }
    </style>
</head>
<body runat="server">
    <form id="frmPrincipal" runat="server">
        <div class="card mb-0 shadow-none">
            <div class="card-body">
                <h3 class="text-center m-0">
                    <a href="index.html" class="logo logo-admin">
                        <img src="/Imagenes/LogoPrestadito.png" height="50" alt="logo" />
                    </a>
                </h3>
                <div class="p-3">
                    <h4 class="font-18 m-b-5 text-center">Iniciar sesión</h4>
                    <p class="text-center">Registrar procesamiento de expedientes.</p>

                    <div class="alert alert-danger mb-0 text-center" role="alert" runat="server" id="divMensajeError" style="display: none;">
                        <asp:Label ID="lblMensajeError" runat="server"></asp:Label>
                    </div>

                    <div class="form-horizontal m-t-20">
                        <div class="form-group">
                            <label for="txtUsuario">Usuario</label>
                            <asp:TextBox ID="txtUsuario" CssClass="form-control" required="required" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <label for="txtClave">Contraseña</label>
                            <asp:TextBox ID="txtClave" type="password" CssClass="form-control" required="required" runat="server"></asp:TextBox>
                        </div>
                        <div class="form-group row m-t-20">
                            <div class="col-6">
                                <div class="custom-control custom-checkbox">
                                    <input type="checkbox" class="custom-control-input" id="cbRecuerdame" checked="checked" />
                                    <label class="custom-control-label" for="cbRecuerdame">Recuérdame!</label>
                                </div>
                            </div>
                            <div class="col-6 text-right">
                                <button id="btnIniciarSesion" onclick="IniciarSesion()" class="btn btn-primary w-md waves-effect waves-light" type="button">Iniciar sesión</button>
                            </div>
                        </div>
                        <div class="form-group m-t-10 mb-0 row">
                            <div class="col-12 m-t-20">
                                <a href="#"><i class="mdi mdi-lock"></i>&nbsp;Olvidé mi contraseña</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script>

        $(function () {
            CheckRememberMe();
        });

        function IniciarSesion() {

            if ($($("#frmPrincipal")).parsley().isValid()) {

                $.ajax({
                    type: "POST",
                    url: 'CFRM_IniciarSesion.aspx/IniciarSesion',
                    data: JSON.stringify({ usuario: $("#txtUsuario").val(), password: $("#txtClave").val(), dataCrypt: window.location.href }),
                    contentType: 'application/json; charset=utf-8',
                    error: function (xhr, ajaxOptions, thrownError) {
                        MensajeError('No se pudo conectar al servidor, contacte al administrador');
                    },
                    beforeSend: function () {
                        $("#btnIniciarSesion").prop('disabled', true);
                    },
                    success: function (data) {

                        if (data.d.AccesoAutorizado == 1) {
                            if ($('#cbRecuerdame').is(':checked')) {
                                SetCookie('usr', $("#txtUsuario").val(), 30);
                                SetCookie('clv', $("#txtClave").val(), 30);
                            }
                            else {
                                DeleteCookie('usr');
                                DeleteCookie('clv');
                            }
                            window.location = data.d.UrlRedireccion;
                        }
                        else
                            MostrarMensajeError(data.d.Mensaje);
                    },
                    complete: function () {
                        $("#btnIniciarSesion").prop('disabled', false);
                    }
                });
            }
            else
                $($("#frmPrincipal")).parsley().validate();

        }

        function MostrarMensajeError(mensaje) {
            $("#divMensajeError").css('display', '');
            $("#lblMensajeError").text(mensaje);
        }


        /* Recordar contraseña Vanilla JS*/
        function SetCookie(cname, cvalue, exdays) {

            var d = new Date();
            d.setTime(d.getTime() + (exdays * 24 * 60 * 60 * 1000));

            var expires = "expires=" + d.toUTCString();
            document.cookie = cname + "=" + cvalue + ";" + expires + ";path=/";
        }

        function DeleteCookie(cname) {

            document.cookie = cname + "=;expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;";
        }

        function GetCookie(cname) {

            var name = cname + "=";
            var ca = document.cookie.split(';');
            for (var i = 0; i < ca.length; i++) {
                var c = ca[i];
                while (c.charAt(0) == ' ') {
                    c = c.substring(1);
                }
                if (c.indexOf(name) == 0) {
                    return c.substring(name.length, c.length);
                }
            }
            return "";
        }

        function CheckRememberMe() {

            var user = GetCookie("usr");
            var pass = GetCookie("clv");

            if (user != "" && user != null && pass != "" && pass != null) {

                $("#txtUsuario").val(user);
                $("#txtClave").val(pass);
            }
        }

    </script>
</body>
</html>
