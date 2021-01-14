using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesGPS_Listado : System.Web.UI.Page
{
    #region Propiedades

    private string pcIDApp = "";
    private string pcIDSesion = "";
    private string pcIDUsuario = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = string.Empty;

                if (liParamStart > 0)
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);

                if (lcParametros != string.Empty)
                {
                    string pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    string lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensajeError("Ocurrió un error: " + ex.Message.ToString());
        }
    }

    [WebMethod]
    public static SolicitudesGPS_Listado_ViewModel CargarSolicitudesGPS(string dataCrypt)
    {
        var solicitudesGPS = new SolicitudesGPS_Listado_ViewModel();

        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("AutoLoan.dbo.sp_AutoGPS_Instalacion_ListByAssignedUser", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        var idEstadoSolicitudGPS = 0;

                        while (sqlResultado.Read())
                        {
                            idEstadoSolicitudGPS = (int)sqlResultado["fiStatusGPSInstalacion"];

                            if (idEstadoSolicitudGPS == 1 || idEstadoSolicitudGPS == 2)
                            {
                                solicitudesGPS.SolicitudesGPS_Pendientes.Add(new SolicitudGPS_ViewModel()
                                {
                                    IdSolicitudGPS = (int)sqlResultado["fiIDAutoGPSInstalacion"],
                                    IdSolicitudCredito = (int)sqlResultado["fiIDCREDSolicitud"],
                                    FechaInstalacion = (DateTime)sqlResultado["fdFechaInstalacion"],
                                    AgenciaInstalacion = sqlResultado["fcNombreAgencia"].ToString(),
                                    ComentarioInstalacion = sqlResultado["fcComentario"].ToString(),
                                    IdEstadoInstalacion = (int)sqlResultado["fiStatusGPSInstalacion"],
                                    EstadoSolicitudGPS = sqlResultado["StatusGPSName"].ToString(),
                                    EstadoSolicitudGPSClassName = sqlResultado["fcClassName"].ToString(),
                                    IdUsuarioCreador = (int)sqlResultado["fiIDUsuarioCreador"],
                                    UsuarioCreador = sqlResultado["fcNombreUsuario"].ToString(),
                                    IdUsuarioAsignado = (int)sqlResultado["fiIDUsuarioAsignado"],
                                    UsuarioAsignado = sqlResultado["fcUsuarioAsignado"].ToString(),
                                    FechaCreacion = (DateTime)sqlResultado["fdFechaCreado"],
                                    IdCliente = (int)sqlResultado["fiIDCliente"],
                                    NombreCliente = sqlResultado["fcNombreCliente"].ToString(),
                                    IdGarantia = (int)sqlResultado["fiIDCREDGarantia"],
                                    VIN = sqlResultado["fcVIN"].ToString(),
                                    Marca = sqlResultado["fcMarca"].ToString(),
                                    Modelo = sqlResultado["fcModelo"].ToString(),
                                    Anio = sqlResultado["fiAnio"].ToString(),
                                });
                            }
                            else
                            {
                                solicitudesGPS.SolicitudesGPS_Completadas.Add(new SolicitudGPS_ViewModel()
                                {
                                    IdSolicitudGPS = (int)sqlResultado["fiIDAutoGPSInstalacion"],
                                    IdSolicitudCredito = (int)sqlResultado["fiIDCREDSolicitud"],
                                    FechaInstalacion = (DateTime)sqlResultado["fdFechaInstalacion"],
                                    AgenciaInstalacion = sqlResultado["fcNombreAgencia"].ToString(),
                                    ComentarioInstalacion = sqlResultado["fcComentario"].ToString(),
                                    IdEstadoInstalacion = (int)sqlResultado["fiStatusGPSInstalacion"],
                                    EstadoSolicitudGPS = sqlResultado["StatusGPSName"].ToString(),
                                    EstadoSolicitudGPSClassName = sqlResultado["fcClassName"].ToString(),
                                    IdUsuarioCreador = (int)sqlResultado["fiIDUsuarioCreador"],
                                    UsuarioCreador = sqlResultado["fcNombreUsuario"].ToString(),
                                    IdUsuarioAsignado = (int)sqlResultado["fiIDUsuarioAsignado"],
                                    UsuarioAsignado = sqlResultado["fcUsuarioAsignado"].ToString(),
                                    FechaCreacion = (DateTime)sqlResultado["fdFechaCreado"],
                                    IdCliente = (int)sqlResultado["fiIDCliente"],
                                    NombreCliente = sqlResultado["fcNombreCliente"].ToString(),
                                    IdGarantia = (int)sqlResultado["fiIDCREDGarantia"],
                                    VIN = sqlResultado["fcVIN"].ToString(),
                                    Marca = sqlResultado["fcMarca"].ToString(),
                                    Modelo = sqlResultado["fcModelo"].ToString(),
                                    Anio = sqlResultado["fiAnio"].ToString(),
                                });
                            }
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            solicitudesGPS = null;
        }
        return solicitudesGPS;
    }

    #region Funciones utilitarias

    [WebMethod]
    public static string EncriptarParametros(int idSolicitudCredito, int idGarantia, int idSolicitudGPS, string dataCrypt)
    {
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            var lcParametros = "usr=" + pcIDUsuario +
            "&IDApp=" + pcIDApp +
            "&SID=" + pcIDSesion +
            "&IDSOL=" + idSolicitudCredito +
            "&IDGarantia=" + idGarantia +
            "&IDSolicitudGPS=" + idSolicitudGPS;

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
            var liParamStart = 0;
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;
            liParamStart = URL.IndexOf("?");

            if (liParamStart > 0)
            {
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            }
            else
            {
                lcParametros = string.Empty;
            }

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

    private void MostrarMensajeError(string mensaje)
    {
        lblMensajeError.Text = mensaje;
    }

    #endregion

    #region View Models

    public class SolicitudesGPS_Listado_ViewModel
    {
        public List<SolicitudGPS_ViewModel> SolicitudesGPS_Pendientes { get; set; }
        public List<SolicitudGPS_ViewModel> SolicitudesGPS_Completadas { get; set; }

        public SolicitudesGPS_Listado_ViewModel()
        {
            SolicitudesGPS_Pendientes = new List<SolicitudGPS_ViewModel>();
            SolicitudesGPS_Completadas = new List<SolicitudGPS_ViewModel>();
        }
    }

    public class SolicitudGPS_ViewModel
    {
        public int IdSolicitudGPS { get; set; }
        public int IdSolicitudCredito { get; set; }
        public DateTime FechaInstalacion { get; set; }
        public string AgenciaInstalacion { get; set; }
        public string ComentarioInstalacion { get; set; }
        public int IdEstadoInstalacion { get; set; }
        public string EstadoSolicitudGPS { get; set; }
        public string EstadoSolicitudGPSClassName { get; set; }

        public int IdUsuarioCreador { get; set; }
        public string UsuarioCreador { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public string UsuarioAsignado { get; set; }
        public DateTime FechaCreacion { get; set; }

        public int IdCliente { get; set; }
        public string NombreCliente { get; set; }

        public int IdGarantia { get; set; }
        public string VIN { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Anio { get; set; }
    }
    #endregion
}