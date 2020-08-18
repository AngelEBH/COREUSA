using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Ingresadas : System.Web.UI.Page
{
    private String pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            string lcURL = Request.Url.ToString();
            int liParamStart = lcURL.IndexOf("?");

            string lcParametros;
            if (liParamStart > 0)
                lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
            else
                lcParametros = String.Empty;

            if (lcParametros != String.Empty)
            {
                pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                string lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            }
        }
    }

    [WebMethod]
    public static string EncriptarParametros(int IDSOL, string dataCrypt)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string lcParametros = "usr=" + pcIDUsuario +
            "&IDApp=" + pcIDApp +
            "&SID=" + pcIDSesion +
            "&IDSOL=" + IDSOL;
            resultado = DSC.Encriptar(lcParametros);
        }
        catch
        {
            resultado = "-1";
        }
        return resultado;
    }

    [WebMethod]
    public static List<BandejaSolicitudesViewModel> CargarSolicitudes(string dataCrypt)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        string sqlConnectionString;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        List<BandejaSolicitudesViewModel> solicitudes = new List<BandejaSolicitudesViewModel>();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string MensajeError = String.Empty;
            sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            SqlCommand sqlComando = new SqlCommand("dbo.sp_CREDSolicitud_ListarSolicitudesPorUsuario", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                solicitudes.Add(new BandejaSolicitudesViewModel()
                {
                    fiIDSolicitud = (int)reader["fiIDSolicitud"],
                    fiIDTipoPrestamo = (int)reader["fiIDTipoProducto"],
                    fcDescripcion = (string)reader["fcProducto"],
                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                    fdFechaCreacionSolicitud = (DateTime)reader["fdFechaCreacionSolicitud"],
                    //informacion cliente
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fcIdentidadCliente = (string)reader["fcIdentidadCliente"],
                    fcPrimerNombreCliente = (string)reader["fcPrimerNombreCliente"],
                    fcSegundoNombreCliente = (string)reader["fcSegundoNombreCliente"],
                    fcPrimerApellidoCliente = (string)reader["fcPrimerApellidoCliente"],
                    fcSegundoApellidoCliente = (string)reader["fcSegundoApellidoCliente"],
                    //bitacora
                    fdEnTramiteInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnColaInicio"]),
                    fdEnTramiteFin = ConvertFromDBVal<DateTime>((object)reader["fdEnColaFin"]),
                    fdEnAnalisisInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisInicio"]),
                    fdEnAnalisisFin = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisFin"]),
                    fdCondicionadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdCondicionadoInicio"]),
                    fdCondificionadoFin = ConvertFromDBVal<DateTime>((object)reader["fdCondificionadoFin"]),
                    //proceso de campo
                    fdEnCampoInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnRutaDeInvestigacionInicio"]),
                    fdEnCampoFin = ConvertFromDBVal<DateTime>((object)reader["fdEnRutaDeInvestigacionFin"]),
                    fdReprogramadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdReprogramadoInicio"]),
                    fdReprogramadoFin = ConvertFromDBVal<DateTime>((object)reader["fdReprogramadoFin"]),
                    fiEstadoSolicitud = ConvertFromDBVal<byte>((object)reader["fiEstadoSolicitud"]),
                });
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            if (sqlConexion != null)
            {
                if (sqlConexion.State == ConnectionState.Open)
                    sqlConexion.Close();
            }
            if (reader != null)
                reader.Close();
        }
        return solicitudes;
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            int liParamStart = 0;
            string lcParametros = "";
            String pcEncriptado = "";
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

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
            return default(T);
        else
            return (T)obj;
    }
}