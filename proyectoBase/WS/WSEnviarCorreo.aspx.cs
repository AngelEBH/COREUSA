using System;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

public partial class WSEnviarCorreo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        var resultadoJSON = string.Empty;

        try
        {
            var nombreCompleto = HttpUtility.ParseQueryString(Request.Url.Query).Get("nombre_completo");
            var telefono = HttpUtility.ParseQueryString(Request.Url.Query).Get("telefono");
            var correo = HttpUtility.ParseQueryString(Request.Url.Query).Get("correo");
            var asunto = HttpUtility.ParseQueryString(Request.Url.Query).Get("asunto");
            var mensaje = HttpUtility.ParseQueryString(Request.Url.Query).Get("mensaje");
            var tituloGeneral = HttpUtility.ParseQueryString(Request.Url.Query).Get("titulo_general");
            var subtitulo = HttpUtility.ParseQueryString(Request.Url.Query).Get("subtitulo");
            var correoDestinatario = HttpUtility.ParseQueryString(Request.Url.Query).Get("correo_destinatario");

            if (ValidarParametros(nombreCompleto, telefono, correo, asunto, mensaje, tituloGeneral, subtitulo, correoDestinatario))
            {
                using (var smtpCliente = new SmtpClient())
                {
                    smtpCliente.Host = "mail.miprestadito.com";
                    smtpCliente.Port = 587;
                    smtpCliente.Credentials = new System.Net.NetworkCredential("systembot@miprestadito.com", "iPwf@p3q");
                    smtpCliente.EnableSsl = true;

                    using (var pmmMensaje = new MailMessage())
                    {
                        pmmMensaje.Subject = asunto;
                        pmmMensaje.From = new MailAddress("systembot@miprestadito.com", "System Bot");
                        pmmMensaje.To.Add(correoDestinatario);
                        //pmmMensaje.To.Add("sac@miprestadito.com");
                        pmmMensaje.IsBodyHtml = true;

                        var contenidoCorreo = "<table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                                    "<tr><th style='text-align:left;'>full name:</th> <td>" + nombreCompleto + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>phone number:</th> <td>" + telefono + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Email address:</th> <td>" + correo + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Message:</th> <td>" + mensaje + "</td></tr>" +
                                    "</table>";

                        string htmlString = @"<!DOCTYPE html> " +
                        "<html>" +
                        "<body>" +
                        " <div style=\"width: 500px;\">" +
                        " <table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                        " <tr style=\"height: 30px; background-color:#56396b; font-family: 'Microsoft Tai Le'; font-size: 14px; font-weight: bold; color: white;\">" +
                        " <td style=\"vertical-align: central; text-align:center;\">" + tituloGeneral + "</td>" +
                        " </tr>" +
                        " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        " <td>&nbsp;</td>" +
                        " </tr>" +
                        " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        " <td style=\"background-color:whitesmoke; text-align:center;\">" + subtitulo + "</td>" +
                        " </tr>" +
                        " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        " <td>&nbsp;</td>" +
                        " </tr>" +
                        " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        " <td style=\"vertical-align: central;\">" + contenidoCorreo + "</td>" +
                        " </tr>" +
                        " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        " <td>&nbsp;</td>" +
                        " </tr>" +
                        " <tr style=\"height: 20px; font-family: 'Microsoft Tai Le'; font-size: 12px; text-align:center;\">" +
                        " <td>System Bot Prestadito</td>" +
                        " </tr>" +
                        " </table>" +
                        " </div>" +
                        "</body> " +
                        "</html> ";

                        pmmMensaje.Body = htmlString;

                        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                        smtpCliente.Send(pmmMensaje);

                        resultadoJSON = "{\"Respuesta\":\"1\",\"Mensaje\":\"Enviado existosamente.\"}";
                        Response.ContentType = "application/json; charset=utf-8";
                        Response.Write(resultadoJSON);
                    }
                }
            }
            else
            {
                resultadoJSON = "{\"Respuesta\":\"0\",\"Mensaje\":\"" + "Parametros invalidos" + "\"}";
                Response.ContentType = "application/json; charset=utf-8";
                Response.Write(resultadoJSON);
            }
        }
        catch (Exception ex)
        {
            resultadoJSON = "{\"Respuesta\":\"0\",\"Mensaje\":\"" + ex.Message.Trim() + "\"}";
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write(resultadoJSON);
        }
    }

    private bool ValidarParametros(string nombreCompleto, string telefono, string correo, string asunto, string mensaje, string tituloGeneral, string subtitulo, string correoDestinatario)
    {
        if (nombreCompleto == "" || nombreCompleto == null ||
            telefono == "" || telefono == null ||
            correo == "" || correo == null ||
            asunto == "" || asunto == null ||
            mensaje == "" || mensaje == null ||
            tituloGeneral == "" || tituloGeneral == null ||
            subtitulo == "" || subtitulo == null ||
            correoDestinatario == "" || correoDestinatario == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}