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
using System.Web.UI.WebControls;

public partial class SolicitudesCredito_ListadoGarantias : System.Web.UI.Page
{
    private string pcIDApp = "";
    private string pcIDSesion = "";
    private string pcIDUsuario = "";
    public string pcNombreUsuario = "";
    public string pcBuzoCorreoUsuario = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var lcURL = Request.Url.ToString();
            int liParamStart = lcURL.IndexOf("?");
            string lcParametros;

            if (liParamStart > 0)
                lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
            else
                lcParametros = String.Empty;

            if (lcParametros != String.Empty)
            {
                string pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                CargarInformacionUsuario();
                CargarListas();
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
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            pcNombreUsuario = sqlResultado["fcNombreCorto"].ToString();
                            pcBuzoCorreoUsuario = sqlResultado["fcBuzondeCorreo"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensajeError("Ocurrió un error al cargar la información del usuario: " + ex.Message.ToString());
        }
    }

    private void CargarListas()
    {
        ddlUbicacionInstalacion.Items.Clear();
        ddlUbicacionInstalacion.Items.Add(new ListItem("Seleccione una opción", ""));

        ddlUbicacionInstalacion_Actualizar.Items.Clear();
        ddlUbicacionInstalacion_Actualizar.Items.Add(new ListItem("Seleccione una opción", ""));

        ddlUbicacionInstalacion_Detalle.Items.Clear();
        ddlUbicacionInstalacion_Detalle.Items.Add(new ListItem("Seleccione una opción", ""));

        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_UbicacionesInstalacionGPS", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);                    
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            ddlUbicacionInstalacion.Items.Add(new ListItem(sqlResultado["fcNombreAgencia"].ToString(), sqlResultado["fiIDAgencia"].ToString()));
                            ddlUbicacionInstalacion_Actualizar.Items.Add(new ListItem(sqlResultado["fcNombreAgencia"].ToString(), sqlResultado["fiIDAgencia"].ToString()));
                            ddlUbicacionInstalacion_Detalle.Items.Add(new ListItem(sqlResultado["fcNombreAgencia"].ToString(), sqlResultado["fiIDAgencia"].ToString()));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensajeError("Ocurrió un error al cargar parte de la información: " + ex.Message.ToString());
        }
    }

    [WebMethod]
    public static List<SolicitudesCredito_ListadoGarantias_ViewModel> CargarListado(string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
        var listado = new List<SolicitudesCredito_ListadoGarantias_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantia_Solicitudes_Listado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listado.Add(new SolicitudesCredito_ListadoGarantias_ViewModel()
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
                                IdCliente = (int)sqlResultado["fiIDCliente"],
                                Identidad = sqlResultado["fcIdentidadCliente"].ToString(),
                                PrimerNombre = sqlResultado["fcPrimerNombreCliente"].ToString(),
                                SegundoNombre = sqlResultado["fcSegundoNombreCliente"].ToString(),
                                PrimerApellido = sqlResultado["fcPrimerApellidoCliente"].ToString(),
                                SegundoApellido = sqlResultado["fcSegundoApellidoCliente"].ToString(),
                                IdGarantia = (int)sqlResultado["fiIDGarantia"],
                                VIN = sqlResultado["fcVIN"].ToString(),
                                Marca = sqlResultado["fcMarca"].ToString(),
                                Modelo = sqlResultado["fcModelo"].ToString(),
                                Anio = sqlResultado["fiAnio"].ToString(),
                                /* Solicitud de instalacion de GPS */
                                IdAutoGPSInstalacion = (int)sqlResultado["fiIDAutoGPSInstalacion"],
                                IDAgenciaInstalacion = (int)sqlResultado["fiIDAgenciaInstalacion"],
                                FechaInstalacion = (DateTime)sqlResultado["fdFechaInstalacion"],
                                Comentario_Instalacion = sqlResultado["fcComentarioInstalacionGPS"].ToString(),
                                IdEstadoInstalacion = (int)sqlResultado["fiStatusGPSInstalacion"],
                                EstadoActivoSolicitudGPS = (bool)sqlResultado["fiGPSInstalacionActivo"],
                            });
                        }
                    } // using reader
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return listado;
    }

    [WebMethod]
    public static List<GarantiaSinSolicitud_ViewModel> CargarListadoGarantiasSinGarantia(string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
        var listado = new List<GarantiaSinSolicitud_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_SinSolicitudListado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listado.Add(new GarantiaSinSolicitud_ViewModel()
                            {
                                IdGarantia = (int)reader["fiIDGarantia"],
                                Vendedor = reader["fcNombreCorto"].ToString(),
                                Agencia = reader["fcAgencia"].ToString(),
                                VIN = reader["fcVin"].ToString(),
                                TipoDeGarantia = reader["fcTipoGarantia"].ToString(),
                                TipoDeVehiculo = reader["fcTipoVehiculo"].ToString(),
                                FechaCreacion = (DateTime)reader["fdFechaCreado"],
                            });
                        }
                    } // using reader
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return listado;
    }

    [WebMethod]
    public static bool GuardarSolicitudGPS(SolicitudGPS_ViewModel solicitudGPS, string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_SolicitarGPS", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDCREDSolicitud", solicitudGPS.IdSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDCREDGarantia", solicitudGPS.IdGarantia);
                    sqlComando.Parameters.AddWithValue("@pcVIN", solicitudGPS.VIN);
                    sqlComando.Parameters.AddWithValue("@pdFechaInstalacion", solicitudGPS.FechaInstalacion);
                    sqlComando.Parameters.AddWithValue("@pcComentario", solicitudGPS.Comentario_Instalacion);
                    sqlComando.Parameters.AddWithValue("@piIDAgenciaInstalacion", solicitudGPS.IDAgenciaInstalacion);
                    sqlComando.Parameters.AddWithValue("@piIDInventarioGPS", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            var resultadoSP = sqlResultado["MensajeError"].ToString();

                            if (!resultadoSP.StartsWith("-1"))
                            {
                                resultado = true;

                                var contenidoCorreo = "<table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                                    "<tr><th style='text-align:left;'>Cliente:</th> <td>" + solicitudGPS.NombreCliente + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Identidad:</th> <td>" + solicitudGPS.IdentidadCliente + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>VIN:</th> <td>" + solicitudGPS.VIN + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Fecha de instalación:</th> <td>" + solicitudGPS.FechaInstalacion.ToString("MM/dd/yyyy hh:mm tt") + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Ubicación:</th> <td>" + solicitudGPS.AgenciaInstalacion + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>VIN:</th> <td>" + solicitudGPS.VIN + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Marca:</th> <td>" + solicitudGPS.Marca + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Modelo:</th> <td>" + solicitudGPS.Modelo + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Año:</th> <td>" + solicitudGPS.Anio + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Solicitado por:</th> <td>" + solicitudGPS.NombreUsuario + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Comentario:</th> <td>" + solicitudGPS.Comentario_Instalacion + "</td></tr>" +
                                    "</table>";

                                EnviarCorreo("Nueva solicitud de GPS", "Nueva solicitud de GPS", "Datos", contenidoCorreo, solicitudGPS.CorreoUsuario);
                            }
                        }
                    } // using reader
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    [WebMethod]
    public static SolicitudGPS_ViewModel CargarInformacionSolicitudGPS(int idSolicitud, int idGarantia, string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
        SolicitudGPS_ViewModel solicitudGPS = null;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_DetalleSolicitudGPS", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDGarantia", idGarantia);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            solicitudGPS = new SolicitudGPS_ViewModel()
                            {
                                IdAutoGPSInstalacion = (int)sqlResultado["fiIDAutoGPSInstalacion"],
                                IdSolicitud = (int)sqlResultado["fiIDCREDSolicitud"],
                                IdGarantia = (int)sqlResultado["fiIDCREDGarantia"],
                                VIN = sqlResultado["fcVIN"].ToString(),
                                FechaInstalacion = (DateTime)sqlResultado["fdFechaInstalacion"],
                                IDAgenciaInstalacion = (int)sqlResultado["fiIDAgenciaInstalacion"],
                                Comentario_Instalacion = sqlResultado["fcComentario"].ToString(),
                                IdEstadoInstalacion = (int)sqlResultado["fiStatusGPSInstalacion"],
                                EstadoInstalacionGPS = sqlResultado["fcStatusGPSInstalacion"].ToString(),
                                EstadoInstalacionClassName = sqlResultado["fcStatusClassName"].ToString(),
                                EstadoActivoSolicitudGPS = (bool)sqlResultado["fiGPSInstalacionActivo"]
                            };
                        }
                    } // using reader
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            solicitudGPS = null;
        }
        return solicitudGPS;
    }

    [WebMethod]
    public static bool ActualizarSolicitudGPS(SolicitudGPS_ViewModel solicitudGPS, string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_ActualizarSolicitudGPS", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;

                    sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", solicitudGPS.IdAutoGPSInstalacion);
                    sqlComando.Parameters.AddWithValue("@piIDCREDSolicitud", solicitudGPS.IdSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDCREDGarantia", solicitudGPS.IdGarantia);
                    sqlComando.Parameters.AddWithValue("@pcVIN", solicitudGPS.VIN);
                    sqlComando.Parameters.AddWithValue("@pdFechaInstalacion", solicitudGPS.FechaInstalacion);
                    sqlComando.Parameters.AddWithValue("@pcComentario", solicitudGPS.Comentario_Instalacion);
                    sqlComando.Parameters.AddWithValue("@piIDAgenciaInstalacion", solicitudGPS.IDAgenciaInstalacion);
                    sqlComando.Parameters.AddWithValue("@piIDInventarioGPS", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            var resultadoSP = sqlResultado["MensajeError"].ToString();

                            if (!resultadoSP.StartsWith("-1"))
                            {
                                resultado = true;

                                var contenidoCorreo = "<table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                                    "<tr><th style='text-align:left;'>Cliente:</th> <td>" + solicitudGPS.NombreCliente + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Identidad:</th> <td>" + solicitudGPS.IdentidadCliente + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>VIN:</th> <td>" + solicitudGPS.VIN + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Fecha de instalación:</th> <td>" + solicitudGPS.FechaInstalacion.ToString("MM/dd/yyyy hh:mm tt") + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Ubicación:</th> <td>" + solicitudGPS.AgenciaInstalacion + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>VIN:</th> <td>" + solicitudGPS.VIN + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Marca:</th> <td>" + solicitudGPS.Marca + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Modelo:</th> <td>" + solicitudGPS.Modelo + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Año:</th> <td>" + solicitudGPS.Anio + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Solicitado por:</th> <td>" + solicitudGPS.NombreUsuario + "</td></tr>" +
                                    "<tr><th style='text-align:left;'>Comentario:</th> <td>" + solicitudGPS.Comentario_Instalacion + "</td></tr>"+
                                    "</table>";

                                EnviarCorreo("Actualización de solicitud de GPS", "Actualización de solicitud de GPS","Nuevos datos", contenidoCorreo, solicitudGPS.CorreoUsuario);
                            }
                        }
                    } // using reader
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    [WebMethod]
    public static string EncriptarParametros(int idSolicitud, int idGarantia, string dataCrypt)
    {
        string resultado;
        var DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string lcParametros = "usr=" + pcIDUsuario +
            "&IDApp=" + pcIDApp +
            "&SID=" + pcIDSesion +
            "&IDSOL=" + idSolicitud +
            "&IDGarantia=" + idGarantia;
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
            var DSC = new DSCore.DataCrypt();
            int liParamStart = 0;
            string lcParametros = "";
            string pcEncriptado = "";
            liParamStart = URL.IndexOf("?");
            if (liParamStart > 0)
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            else
                lcParametros = String.Empty;

            if (lcParametros != String.Empty)
            {
                pcEncriptado = URL.Substring((liParamStart + 1), URL.Length - (liParamStart + 1));
                string lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
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
        var pmmMensaje = new MailMessage();
        var smtpCliente = new SmtpClient();

        smtpCliente.Host = "mail.miprestadito.com";
        smtpCliente.Port = 587;
        smtpCliente.Credentials = new System.Net.NetworkCredential("systembot@miprestadito.com", "iPwf@p3q");
        smtpCliente.EnableSsl = true;

        pmmMensaje.Subject = pcAsunto;
        pmmMensaje.From = new MailAddress("systembot@miprestadito.com", "System Bot");
        pmmMensaje.To.Add("sistemas@miprestadito.com");
        pmmMensaje.CC.Add(buzonCorreoUsuario);
        pmmMensaje.IsBodyHtml = true;

        string htmlString = @"<!DOCTYPE html> " +
                    "<html>" +
                    "<body>" +
                    "    <div style=\"width: 500px;\">" +
                    "        <table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                    "            <tr style=\"height: 30px; background-color:#56396b; font-family: 'Microsoft Tai Le'; font-size: 14px; font-weight: bold; color: white;\">" +
                    "                <td style=\"vertical-align: central; text-align:center;\">" + pcTituloGeneral + "</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    "                <td>&nbsp;</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    "                <td style=\"background-color:whitesmoke; text-align:center;\">" + pcSubtitulo + "</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    "                <td>&nbsp;</td>" +
                    "            </tr>" +
                    "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    "                <td style=\"vertical-align: central;\">" + pcContenidodelMensaje + "</td>" +
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
}

public class SolicitudesCredito_ListadoGarantias_ViewModel
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
    public int IdCliente { get; set; }
    public string Identidad { get; set; }
    public string PrimerNombre { get; set; }
    public string SegundoNombre { get; set; }
    public string PrimerApellido { get; set; }
    public string SegundoApellido { get; set; }
    public int IdGarantia { get; set; }
    public string VIN { get; set; }
    /* Estas propiedas se utilizan como parte de la información que se envía por correo al guardar o actualizar solucitudes de GPS */
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Anio { get; set; }

    /* Solicitud de instalacion de GPS */
    public int IdAutoGPSInstalacion { get; set; }
    public int IDAgenciaInstalacion { get; set; }
    public DateTime FechaInstalacion { get; set; }
    public string Comentario_Instalacion { get; set; }
    public int IdEstadoInstalacion { get; set; }
    public bool EstadoActivoSolicitudGPS { get; set; }
}

public class SolicitudGPS_ViewModel
{
    public int IdAutoGPSInstalacion { get; set; }
    public int IdSolicitud { get; set; }
    public int IdGarantia { get; set; }
    public string VIN { get; set; }
    public DateTime FechaInstalacion { get; set; }
    public int IDAgenciaInstalacion { get; set; }
    public string AgenciaInstalacion { get; set; }
    public string Comentario_Instalacion { get; set; }
    public int IdEstadoInstalacion { get; set; }
    public string EstadoInstalacionGPS { get; set; }
    public string EstadoInstalacionClassName { get; set; }
    public bool EstadoActivoSolicitudGPS { get; set; }

    /* Estas propiedas se utilizan como parte de la información que se envía por correo al guardar o actualizar solucitudes de GPS */
    public string NombreCliente { get; set; }
    public string IdentidadCliente { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public string Anio { get; set; }
    public string NombreUsuario { get; set; }
    public string CorreoUsuario { get; set; }
}

public class GarantiaSinSolicitud_ViewModel
{
    public int IdGarantia { get; set; }
    public string Agencia { get; set; }
    public string Vendedor { get; set; }
    public string TipoDeGarantia { get; set; }
    public string TipoDeVehiculo { get; set; }
    public string VIN { get; set; }
    public DateTime FechaCreacion { get; set; }
}