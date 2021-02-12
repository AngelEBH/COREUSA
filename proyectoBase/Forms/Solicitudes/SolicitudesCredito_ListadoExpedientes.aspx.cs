using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_ListadoExpedientes : System.Web.UI.Page
{
    #region Propiedades públicas

    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcNombreUsuario = "";
    public string pcBuzoCorreoUsuario = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    #endregion

    #region Page_Load, Cargar información del usuario

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

                CargarInformacionUsuario();
            }
        }
    }

    private void CargarInformacionUsuario()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreSeguridad.dbo.sp_InformacionUsuario", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            pcNombreUsuario = sqlResultado["fcNombreCorto"].ToString();
                            pcBuzoCorreoUsuario = sqlResultado["fcBuzondeCorreo"].ToString();
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            MostrarMensajeError("Ocurrió un error al cargar la información del usuario: " + ex.Message.ToString());
        }
    }

    #endregion

    [WebMethod]
    public static List<SolicitudesCredito_Expediente_ViewModel> CargarListado(string dataCrypt)
    {
        var listado = new List<SolicitudesCredito_Expediente_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Expedientes_Listado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listado.Add(new SolicitudesCredito_Expediente_ViewModel()
                            {
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                IdCanal = (int)sqlResultado["fiIDCanal"],
                                Agencia = sqlResultado["fcAgencia"].ToString(),
                                IdProducto = (int)sqlResultado["fiIDTipoProducto"],
                                Producto = sqlResultado["fcProducto"].ToString(),
                                FechaCreacion = (DateTime)sqlResultado["fdFechaCreacionSolicitud"],
                                IdEstadoSolicitud = (byte)sqlResultado["fiEstadoSolicitud"],
                                IdUsuarioAsignado = (int)sqlResultado["fiIDUsuarioAsignado"],
                                UsuarioAsignado = sqlResultado["fcUsuarioAsignado"].ToString(),
                                IdUsuarioCreador = (int)sqlResultado["fiIDUsuarioCrea"],
                                FechaCreado = (DateTime)sqlResultado["fdFechaCrea"],
                                SolicitudActiva = (byte)sqlResultado["fiSolicitudActiva"],

                                /* Cliente */
                                IdCliente = (int)sqlResultado["fiIDCliente"],
                                Identidad = sqlResultado["fcIdentidadCliente"].ToString(),
                                NombreCompleto = sqlResultado["fcNombreCliente"].ToString(),

                                /* Expediente */
                                IdExpediente = (int)sqlResultado["fiIDExpediente"],
                                CantidadDocumentosExpediente = sqlResultado["fcDocumentosExpediente"].ToString(),
                                ComentariosExpediente = sqlResultado["fcComentarios"].ToString(),
                                IdEstadoExpediente = (int)sqlResultado["fiIDEstadoExpediente"],
                                EstadoExpediente = sqlResultado["fcEstadoExpediente"].ToString(),
                                EstadoExpedienteClassName = sqlResultado["fcEstadoExpedienteClassName"].ToString(),
                                IdUsuarioCreadorExpediente = (int)sqlResultado["fiIDUsuarioCreadorExpediente"],
                                FechaCreadoExpediente = (DateTime)sqlResultado["fdFechaCreacionExpediente"],
                                IdUsuarioUltimaModificacionExpediente = (int)sqlResultado["fiIDUsuarioUltimaModificacionExpediente"],
                                FechaUltimaModificacionExpediente = (DateTime)sqlResultado["fdFechaUltimaModificacionExpediente"],

                                /* Garantia */
                                IdGarantia = (int)sqlResultado["fiIDGarantia"],
                                VIN = sqlResultado["fcVIN"].ToString(),
                                Marca = sqlResultado["fcMarca"].ToString(),
                                Modelo = sqlResultado["fcModelo"].ToString(),
                                Anio = sqlResultado["fiAnio"].ToString(),
                                Color = sqlResultado["fcColor"].ToString(),
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return listado;
    }

    #region Métodos utilitarios

    [WebMethod]
    public static string EncriptarParametros(int idSolicitud, int idGarantia, string dataCrypt)
    {
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            string lcParametros = "usr=" + pcIDUsuario + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion + "&IDSOL=" + idSolicitud + "&IDGarantia=" + idGarantia;

            resultado = DSC.Encriptar(lcParametros);
        }
        catch
        {
            resultado = "-1";
        }
        return resultado;
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = URL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? URL.Substring(liParamStart, URL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = URL.Substring(liParamStart + 1, URL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);

                lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return lURLDesencriptado;
    }

    private void MostrarMensajeError(string mensaje)
    {
        lblMensajeError.InnerText = mensaje;
    }

    public static void EnviarCorreo(string pcAsunto, string pcTituloGeneral, string pcSubtitulo, string pcContenidodelMensaje, string buzonCorreoUsuario)
    {
        try
        {
            using (var smtpCliente = new SmtpClient())
            {
                smtpCliente.Host = "mail.miprestadito.com";
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential("systembot@miprestadito.com", "iPwf@p3q");
                smtpCliente.EnableSsl = true;

                using (var pmmMensaje = new MailMessage())
                {
                    pmmMensaje.Subject = pcAsunto;
                    pmmMensaje.From = new MailAddress("systembot@miprestadito.com", "System Bot");
                    pmmMensaje.To.Add("sistemas@miprestadito.com");
                    pmmMensaje.CC.Add(buzonCorreoUsuario);
                    pmmMensaje.IsBodyHtml = true;

                    string htmlString = @"<!DOCTYPE html> " +
                    "<html>" +
                    "<body>" +
                    " <div style=\"width: 500px;\">" +
                    " <table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                    " <tr style=\"height: 30px; background-color:#56396b; font-family: 'Microsoft Tai Le'; font-size: 14px; font-weight: bold; color: white;\">" +
                    " <td style=\"vertical-align: central; text-align:center;\">" + pcTituloGeneral + "</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td>&nbsp;</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td style=\"background-color:whitesmoke; text-align:center;\">" + pcSubtitulo + "</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td>&nbsp;</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td style=\"vertical-align: central;\">" + pcContenidodelMensaje + "</td>" +
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
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.SendExcepToDB(ex);
        }
    }

    #endregion

    #region View Models

    public class SolicitudesCredito_Expediente_ViewModel
    {
        public int IdSolicitud { get; set; }
        public int IdCanal { get; set; }
        public string Agencia { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdEstadoSolicitud { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public string UsuarioAsignado { get; set; }
        public int IdUsuarioCreador { get; set; }
        public DateTime FechaCreado { get; set; }
        public int SolicitudActiva { get; set; }

        /* Cliente */
        public int IdCliente { get; set; }
        public string Identidad { get; set; }
        public string NombreCompleto { get; set; }

        /* Expediente */
        public int IdExpediente { get; set; }
        public string CantidadDocumentosExpediente { get; set; }
        public string ComentariosExpediente { get; set; }
        public int IdEstadoExpediente { get; set; }
        public string EstadoExpediente { get; set; }
        public string EstadoExpedienteClassName { get; set; }
        public int IdUsuarioCreadorExpediente { get; set; }
        public DateTime FechaCreadoExpediente { get; set; }
        public int IdUsuarioUltimaModificacionExpediente { get; set; }
        public DateTime FechaUltimaModificacionExpediente { get; set; }

        /* Garantia */
        public int IdGarantia { get; set; }
        public string VIN { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Anio { get; set; }
        public string Color { get; set; }
    }

    #endregion
}
