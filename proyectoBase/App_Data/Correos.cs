using System.Net.Mail;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Summary description for Correos
/// </summary>
public class Correos
{
    public string lcAsunto = "";
    public string lcTituloGeneral = "";
    public string lcSubtitulo = "";
    public string lcContenidodelMensaje = "";
    private MailMessage pmmMensaje = new MailMessage();
    private SmtpClient smtpCliente = new SmtpClient();

    public Correos()
    {
        smtpCliente.Host = "mail.miprestadito.com";
        smtpCliente.Port = 587;
        smtpCliente.Credentials = new System.Net.NetworkCredential("systembot@miprestadito.com", "iPwf@p3q");
        smtpCliente.EnableSsl = true;
        pmmMensaje.From = new MailAddress("systembot@miprestadito.com","System Bot");
        pmmMensaje.IsBodyHtml = true;
    }

    public void EnviarCorreo()
    {
        pmmMensaje.Subject = lcAsunto;

        string htmlString = @"<!DOCTYPE html> " +
                    "<html>" +
                    "<body>" +
                    "    <div style=\"width: 500px;\">" +
                    "        <table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                    "            <tr style=\"height: 30px; background-color:#56396b; font-family: 'Microsoft Tai Le'; font-size: 14px; font-weight: bold; color: white;\">" +
                    "                <td style=\"vertical-align: central; text-align:center;\">" + lcTituloGeneral + "</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    "                <td>&nbsp;</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    "                <td style=\"background-color:whitesmoke; text-align:center;\">" + lcSubtitulo + "</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    "                <td>&nbsp;</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    "                <td style=\"vertical-align: central; text-align:center;\">" + lcContenidodelMensaje + "</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    "                <td>&nbsp;</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 20px; font-family: 'Microsoft Tai Le'; font-size: 12px; text-align:center;\">" +
                    "                <td>System Bot Prestadito</td>" +
                    "            </tr>" +
                    "        </table>" +
                    "    </div>" +
                    "</body> " +
                    "</html> ";

        pmmMensaje.Body = htmlString;

        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
        smtpCliente.Send(pmmMensaje);
    }

    public void AgregarDestinatario(string pcCorreo)
    {
        pmmMensaje.To.Add(pcCorreo);
    }
}