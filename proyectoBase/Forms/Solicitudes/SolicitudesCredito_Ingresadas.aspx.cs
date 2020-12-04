using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Ingresadas : System.Web.UI.Page
{
    private string pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var DSC = new DSCore.DataCrypt();
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");

            string lcParametros;
            if (liParamStart > 0)
                lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            }
        }
    }

    [WebMethod]
    public static string EncriptarParametros(int idSolicitud, string dataCrypt, string identidad)
    {
        var DSC = new DSCore.DataCrypt();
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string lcParametros = "usr=" + pcIDUsuario +
            "&IDApp=" + pcIDApp +
            "&SID=" + pcIDSesion +
            "&pcID=" + identidad +
            "&IDSOL=" + idSolicitud;
            resultado = DSC.Encriptar(lcParametros);
        }
        catch
        {
            resultado = "-1";
        }
        return resultado;
    }

    [WebMethod]
    public static List<SolicitudCredito_ViewModel> CargarSolicitudes(string dataCrypt)
    {        
        var solicitudes = new List<SolicitudCredito_ViewModel>();
        var DSC = new DSCore.DataCrypt();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            pcIDUsuario = "3";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Ingresadas", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            solicitudes.Add(new SolicitudCredito_ViewModel()
                            {
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                Agencia = sqlResultado["fcAgencia"].ToString(),
                                Producto = sqlResultado["fcProducto"].ToString(),
                                IdCliente = (int)sqlResultado["fiIDCliente"],
                                Identidad = sqlResultado["fcIdentidadCliente"].ToString(),
                                NombreCliente = sqlResultado["fcPrimerNombreCliente"].ToString() + " " + sqlResultado["fcSegundoNombreCliente"].ToString() + " " + sqlResultado["fcPrimerApellidoCliente"].ToString() + " " + sqlResultado["fcSegundoApellidoCliente"].ToString(),
                                FechaCreacion = (DateTime)sqlResultado["fdFechaCreacionSolicitud"],
                                IdEstadoSolicitud = (byte)sqlResultado["fiEstadoSolicitud"],
                                IdUsuarioAsignado = (int)sqlResultado["fiIDUsuarioAsignado"],
                                UsuarioAsignado = sqlResultado["fcNombreCorto"].ToString(),
                                ReprogramadoInicio = (DateTime?)(DBNull.Value == sqlResultado["fdReprogramadoInicio"] ? null : sqlResultado["fdReprogramadoInicio"]),
                                ReprogramadoFin = (DateTime?)(DBNull.Value == sqlResultado["fdReprogramadoFin"] ? null : sqlResultado["fdReprogramadoFin"]),
                                EstadoDeCampo = (byte)sqlResultado["fiEstadoDeCampo"],
                                CondicionadoInicio = (DateTime?)(DBNull.Value == sqlResultado["fdCondicionadoInicio"] ? null : sqlResultado["fdCondicionadoInicio"]),
                                CondicionadoFin = (DateTime?)(DBNull.Value == sqlResultado["fdCondificionadoFin"] ? null : sqlResultado["fdCondificionadoFin"])
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return solicitudes;
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var DSC = new DSCore.DataCrypt();
            var liParamStart = 0;
            string lcParametros = "";
            var pcEncriptado = "";
            liParamStart = URL.IndexOf("?");
            if (liParamStart > 0)
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = URL.Substring((liParamStart + 1), URL.Length - (liParamStart + 1));
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
}

#region View Models

public class SolicitudCredito_ViewModel
{
    public int IdSolicitud { get; set; }
    public string Agencia { get; set; }
    public string Producto { get; set; }
    public int IdCliente { get; set; }
    public string Identidad { get; set; }
    public string NombreCliente { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int IdEstadoSolicitud { get; set; }
    public int IdUsuarioAsignado { get; set; }
    public string UsuarioAsignado { get; set; }
    public DateTime? ReprogramadoInicio { get; set; }
    public DateTime? ReprogramadoFin { get; set; }
    public int EstadoDeCampo { get; set; }
    public DateTime? CondicionadoInicio { get; set; }
    public DateTime? CondicionadoFin { get; set; }
}

#endregion