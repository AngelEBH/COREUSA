using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Bandeja : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            String pcEncriptado = "";
            string pcIDUsuario = "";
            string pcIDApp = "";
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
                HttpContext.Current.Session["pcIDUsuario"] = pcIDUsuario;
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

                if (pcIDUsuario.Trim() == "142" || pcIDUsuario.Trim() == "1")
                    btnAbrirSolicitud.Visible = true;
            }
        }
    }

    [WebMethod]
    public static List<BandejaSolicitudesViewModel> CargarSolicitudes(string dataCrypt,int IDSOL)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        List<BandejaSolicitudesViewModel> solicitudes = new List<BandejaSolicitudesViewModel>();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            //sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
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
                    fcAgencia = (string)reader["fcNombreAgencia"],
                    /* Informacion del vendedor */
                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioVendedor"],
                    fcNombreUsuarioCrea = (string)reader["fcNombreCortoVendedor"],
                    fdFechaCreacionSolicitud = (DateTime)reader["fdFechaCreacionSolicitud"],
                    /* Informacion del analista */
                    fiIDUsuarioModifica = (int)reader["fiIDAnalista"],
                    fcNombreUsuarioModifica = (string)reader["fcNombreCortoAnalista"],
                    /* Informacion cliente */
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fcIdentidadCliente = (string)reader["fcIdentidadCliente"],
                    fcPrimerNombreCliente = (string)reader["fcPrimerNombreCliente"],
                    fcSegundoNombreCliente = (string)reader["fcSegundoNombreCliente"],
                    fcPrimerApellidoCliente = (string)reader["fcPrimerApellidoCliente"],
                    fcSegundoApellidoCliente = (string)reader["fcSegundoApellidoCliente"],
                    /* Bitacora */
                    fdEnIngresoInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnIngresoInicio"]),
                    fdEnIngresoFin = ConvertFromDBVal<DateTime>((object)reader["fdEnIngresoFin"]),
                    fdEnTramiteInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnColaInicio"]),
                    fdEnTramiteFin = ConvertFromDBVal<DateTime>((object)reader["fdEnColaFin"]),
                    fdEnAnalisisInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisInicio"]),
                    fdEnAnalisisFin = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisFin"]),
                    fdCondicionadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdCondicionadoInicio"]),
                    fcCondicionadoComentario = (string)reader["fcCondicionadoComentario"],
                    fdCondificionadoFin = ConvertFromDBVal<DateTime>((object)reader["fdCondificionadoFin"]),
                    /* Proceso de campo */
                    fdEnvioARutaAnalista = ConvertFromDBVal<DateTime>((object)reader["fdEnvioARutaAnalista"]),
                    fiEstadoDeCampo = (byte)reader["fiEstadoDeCampo"],
                    fdEnCampoInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnRutaDeInvestigacionInicio"]),
                    fcObservacionesDeGestoria = (string)reader["fcObservacionesDeCampo"],
                    fdEnCampoFin = ConvertFromDBVal<DateTime>((object)reader["fdEnRutaDeInvestigacionFin"]),
                    fdReprogramadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdReprogramadoInicio"]),
                    fcReprogramadoComentario = (string)reader["fcReprogramadoComentario"],
                    fdReprogramadoFin = ConvertFromDBVal<DateTime>((object)reader["fdReprogramadoFin"]),
                    PasoFinalInicio = (DateTime)reader["fdPasoFinalInicio"],
                    IDUsuarioPasoFinal = (int)reader["fiIDUsuarioPasoFinal"],
                    ComentarioPasoFinal = (string)reader["fcComentarioPasoFinal"],
                    PasoFinalFin = (DateTime)reader["fdPasoFinalFin"],
                    fiEstadoSolicitud = (byte)reader["fiEstadoSolicitud"]
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

    [WebMethod]
    public static string AbrirAnalisisSolicitud(string dataCrypt, int IDSOL, string Identidad)
    {
        string resultado = "-1";
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        BandejaSolicitudesViewModel solicitudes = new BandejaSolicitudesViewModel();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                solicitudes = new BandejaSolicitudesViewModel()
                {
                    fiIDAnalista = (int)reader["fiIDAnalista"],
                    fcNombreCortoAnalista = (string)reader["fcNombreCortoAnalista"]
                };
            }

            //if (solicitudes.fiIDAnalista == IDUSR || solicitudes.fiIDAnalista == 0)
            if (1 == 1)
            {
                string lcParametros = "usr=" + pcIDUsuario +
                "&IDApp=" + pcIDApp +
                "&pcID=" + Identidad +
                "&IDSOL=" + IDSOL;
                resultado = DSC.Encriptar(lcParametros);
            }
            else
            {
                resultado = "-1";
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
        return resultado;
    }

    [WebMethod]
    public static string EncriptarParametros(string dataCrypt, int IDSOL, string Identidad)
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
            "&pcID=" + Identidad +
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
    public static bool VerificarAnalista(string dataCrypt, int ID)
    {
        bool resultado = false;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string lcURL = HttpContext.Current.Request.Url.ToString();
            if (ID == pcIDUsuario)
                resultado = true;
        }
        catch
        {
            resultado = false;
        }
        return resultado;
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