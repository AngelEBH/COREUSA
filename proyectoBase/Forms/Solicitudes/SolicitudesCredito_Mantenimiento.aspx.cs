using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Mantenimiento : System.Web.UI.Page
{
    public string pcIDUsuario = "";
    public string pcIDApp = "";
    public string pcSesionID = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                var lcURL = string.Empty;
                var liParamStart = 0;
                var lcParametros = string.Empty;
                var lcParametroDesencriptado = string.Empty;
                var pcEncriptado = string.Empty;

                Uri lURLDesencriptado = null;
                lcURL = Request.Url.ToString();
                liParamStart = lcURL.IndexOf("?");

                if (liParamStart > 0)
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                else
                    lcParametros = string.Empty;

                if (lcParametros != string.Empty)
                {
                    pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcSesionID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
    }

    [WebMethod]
    public static SolicitudesCredito_Mantenimiento_ViewModel CargarInformacion(int idSolicitud, string dataCrypt)
    {
        var informacionDeClienteSolicitud = new SolicitudesCredito_Mantenimiento_ViewModel();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = "1";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_SolicitudClientePorIdSolicitud", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.CommandType = CommandType.StoredProcedure;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            /****** Informacion de la solicitud ******/
                            informacionDeClienteSolicitud.IdSolicitud = (int)sqlResultado["fiIDSolicitud"];
                            informacionDeClienteSolicitud.IdCliente = (int)sqlResultado["fiIDCliente"];
                            informacionDeClienteSolicitud.IdEstadoSolicitud = (byte)sqlResultado["fiEstadoSolicitud"];
                            informacionDeClienteSolicitud.EstadoSolicitud = sqlResultado["fcEstadoSolicitud"].ToString();
                            informacionDeClienteSolicitud.Producto = sqlResultado["fcProducto"].ToString();
                            informacionDeClienteSolicitud.TipoDeSolicitud = sqlResultado["fcTipoSolicitud"].ToString();
                            informacionDeClienteSolicitud.IdUsuarioAsignado = (int)sqlResultado["fiIDUsuarioAsignado"];
                            informacionDeClienteSolicitud.UsuarioAsignado = sqlResultado["fcNombreUsuarioAsignado"].ToString();
                            informacionDeClienteSolicitud.IdGestorAsignado = (int)sqlResultado["fiIDGestor"];
                            informacionDeClienteSolicitud.GestorAsignado = sqlResultado["fcNombreGestor"].ToString();
                            informacionDeClienteSolicitud.Agencia = sqlResultado["fcNombreAgencia"].ToString();

                            /****** Documentos de la solicitud ******/
                            sqlResultado.NextResult();

                            while (sqlResultado.Read())
                            {
                                informacionDeClienteSolicitud.Documentos.Add(new SolicitudesCredito_Mantenimiento_Documentos_Solicitud_ViewModel()
                                {
                                    IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                    IdSolicitudDocumento = (int)sqlResultado["fiIDSolicitudDocs"],
                                    NombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                    Extension = sqlResultado["fcTipoArchivo"].ToString(),
                                    RutaArchivo = sqlResultado["fcRutaArchivo"].ToString(),
                                    URLArchivo = sqlResultado["fcURL"].ToString(),
                                    IdTipoDocumento = (int)sqlResultado["fiTipoDocumento"],
                                    DescripcionTipoDocumento = sqlResultado["fcDescripcionTipoDocumento"].ToString(),
                                    ArchivoActivo = (byte)sqlResultado["fiArchivoActivo"]
                                });
                            }

                            /****** Condicionamientos de la solicitud ******/
                            sqlResultado.NextResult();

                            if (sqlResultado.HasRows)
                            {
                                while (sqlResultado.Read())
                                {
                                    informacionDeClienteSolicitud.Condiciones.Add(new SolicitudesCredito_Mantenimiento_Solicitud_Condicion_ViewModel()
                                    {

                                        IdSolicitudCondicion = (int)sqlResultado["fiIDSolicitudCondicion"],
                                        IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                        IdCondicion = (int)sqlResultado["fiIDCondicion"],
                                        Condicion = sqlResultado["fcCondicion"].ToString(),
                                        DescripcionCondicion = sqlResultado["fcDescripcionCondicion"].ToString(),
                                        ComentarioAdicional = sqlResultado["fcComentarioAdicional"].ToString(),
                                        EstadoCondicion = (bool)sqlResultado["fbEstadoCondicion"]
                                    });
                                }
                            }

                            /****** Información del cliente ******/
                            sqlResultado.NextResult();
                            sqlResultado.Read();

                            informacionDeClienteSolicitud.NombreCliente = sqlResultado["fcPrimerNombreCliente"].ToString() + " " + sqlResultado["fcSegundoNombreCliente"].ToString() + " " + sqlResultado["fcPrimerApellidoCliente"].ToString() + " " + sqlResultado["fcSegundoApellidoCliente"].ToString();
                            informacionDeClienteSolicitud.IdentidadCliente = sqlResultado["fcIdentidadCliente"].ToString();
                            informacionDeClienteSolicitud.RtnCliente = sqlResultado["fcRTN"].ToString();
                            informacionDeClienteSolicitud.Telefono = sqlResultado["fcTelefonoPrimarioCliente"].ToString();

                            /****** Información laboral ******/
                            sqlResultado.NextResult();

                            /****** Informacion domicilio ******/
                            sqlResultado.NextResult();

                            /****** Informacion del conyugue ******/
                            sqlResultado.NextResult();

                            /****** Referencias de la solicitud ******/
                            sqlResultado.NextResult();

                            while (sqlResultado.Read())
                            {
                                informacionDeClienteSolicitud.ReferenciasPersonales.Add(new SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel()
                                {
                                    IdReferencia = (int)sqlResultado["fiIDReferencia"],
                                    IdCliente = (int)sqlResultado["fiIDCliente"],
                                    IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                    NombreCompleto = sqlResultado["fcNombreCompletoReferencia"].ToString(),
                                    LugarTrabajo = sqlResultado["fcLugarTrabajoReferencia"].ToString(),
                                    TiempoDeConocer = sqlResultado["fcTiempoDeConocer"].ToString(),
                                    TelefonoReferencia = sqlResultado["fcTelefonoReferencia"].ToString(),
                                    IdParentescoReferencia = (int)sqlResultado["fiIDParentescoReferencia"],
                                    DescripcionParentesco = sqlResultado["fcDescripcionParentesco"].ToString(),
                                    ReferenciaActivo = (bool)sqlResultado["fbReferenciaActivo"],
                                    RazonInactivo = sqlResultado["fcRazonInactivo"].ToString(),
                                    ComentarioDeptoCredito = sqlResultado["fcComentarioDeptoCredito"].ToString(),
                                    AnalistaComentario = (int)sqlResultado["fiAnalistaComentario"]
                                });
                            }

                            /****** Historial de mantenimientos de la solicitud ******/
                            sqlResultado.NextResult();

                            while (sqlResultado.Read())
                            {
                                informacionDeClienteSolicitud.HistorialMantenimientos.Add(new SolicitudesCredito_Mantenimiento_HistorialMantenimiento_ViewModel()
                                {
                                    IdHistorialMantenimiento = (int)sqlResultado["fiIDHistorialMantenimiento"],
                                    IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                    IdUsuario = (int)sqlResultado["fiIDUsuario"],
                                    NombreUsuario = sqlResultado["fcNombreCorto"].ToString(),
                                    AgenciaUsuario = sqlResultado["fcNombreAgencia"].ToString(),
                                    FechaMantenimiento = (DateTime)sqlResultado["fdFechaMantenimiento"],
                                    Observaciones = sqlResultado["fcObservaciones"].ToString(),
                                    EstadoMantenimiento = (int)sqlResultado["fiEstadoMantenimiento"]
                                });
                            }
                        }
                    }
                } // using command
            }// using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            informacionDeClienteSolicitud = null;
        }
        return informacionDeClienteSolicitud;
    }

    [WebMethod]
    public static bool ResolucionCampo(int idSolicitud, int resolucionCampo, string observaciones, string dataCrypt)
    {
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

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_ResolucionCampo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piResolucionInvestigacion", resolucionCampo);
                    sqlComando.Parameters.AddWithValue("@pcObservacionesDeCampo", observaciones.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (!sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                            {
                                resultado = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    [WebMethod]
    public static bool AsignarSolicitud(int idSolicitud, int idGestor, string dataCrypt)
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

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_AsignarGestor", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDGestor", idGestor);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (!sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                            {
                                resultado = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }



    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = 0;
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;
            liParamStart = URL.IndexOf("?");
            if (liParamStart > 0)
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
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
}

public class SolicitudesCredito_Mantenimiento_ViewModel
{
    public int IdSolicitud { get; set; }
    public int IdCliente { get; set; }
    public string NombreCliente { get; set; }
    public string IdentidadCliente { get; set; }
    public string RtnCliente { get; set; }
    public string Telefono { get; set; }
    public string Producto { get; set; }
    public string TipoDeSolicitud { get; set; }
    public int IdUsuarioAsignado { get; set; }
    public string UsuarioAsignado { get; set; }
    public string Agencia { get; set; }
    public int IdGestorAsignado { get; set; }
    public string GestorAsignado { get; set; }
    public int IdEstadoSolicitud { get; set; }
    public string EstadoSolicitud { get; set; }
    public List<SolicitudesCredito_Mantenimiento_Documentos_Solicitud_ViewModel> Documentos { get; set; }
    public List<SolicitudesCredito_Mantenimiento_Solicitud_Condicion_ViewModel> Condiciones { get; set; }
    public List<SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel> ReferenciasPersonales { get; set; }
    public List<SolicitudesCredito_Mantenimiento_HistorialMantenimiento_ViewModel> HistorialMantenimientos { get; set; }

    public SolicitudesCredito_Mantenimiento_ViewModel()
    {
        Documentos = new List<SolicitudesCredito_Mantenimiento_Documentos_Solicitud_ViewModel>();
        Condiciones = new List<SolicitudesCredito_Mantenimiento_Solicitud_Condicion_ViewModel>();
        ReferenciasPersonales = new List<SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel>();
        HistorialMantenimientos = new List<SolicitudesCredito_Mantenimiento_HistorialMantenimiento_ViewModel>();
    }
}

public class SolicitudesCredito_Mantenimiento_Documentos_Solicitud_ViewModel
{
    public int IdSolicitudDocumento { get; set; }
    public int IdSolicitud { get; set; }
    public string NombreArchivo { get; set; }
    public int IdTipoDocumento { get; set; }
    public string DescripcionTipoDocumento { get; set; }
    public string Extension { get; set; }
    public string RutaArchivo { get; set; }
    public string URLArchivo { get; set; }
    public byte ArchivoActivo { get; set; }
}

public class SolicitudesCredito_Mantenimiento_HistorialMantenimiento_ViewModel
{
    public int IdHistorialMantenimiento { get; set; }
    public int IdSolicitud { get; set; }
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string AgenciaUsuario { get; set; }
    public DateTime FechaMantenimiento { get; set; }
    public string Observaciones { get; set; }
    public int EstadoMantenimiento { get; set; }
}

public class SolicitudesCredito_Mantenimiento_Solicitud_Condicion_ViewModel
{
    public int IdSolicitudCondicion { get; set; }
    public int IdCondicion { get; set; }
    public string Condicion { get; set; }
    public string DescripcionCondicion { get; set; }
    public int IdSolicitud { get; set; }
    public string ComentarioAdicional { get; set; }
    public bool EstadoCondicion { get; set; }
}

public class SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel
{
    public int IdReferencia { get; set; }
    public int IdCliente { get; set; }
    public int IdSolicitud { get; set; }
    public string NombreCompleto { get; set; }
    public string LugarTrabajo { get; set; }
    public string TiempoDeConocer { get; set; }
    public string TelefonoReferencia { get; set; }
    public int IdParentescoReferencia { get; set; }
    public string DescripcionParentesco { get; set; }
    public bool ReferenciaActivo { get; set; }
    public string RazonInactivo { get; set; }
    public string ComentarioDeptoCredito { get; set; }
    public int AnalistaComentario { get; set; }
}