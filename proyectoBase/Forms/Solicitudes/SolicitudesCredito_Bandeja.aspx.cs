using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Bandeja : System.Web.UI.Page
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
                HttpContext.Current.Session["pcIDUsuario"] = pcIDUsuario;
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

                if (pcIDUsuario.Trim() == "142" || pcIDUsuario.Trim() == "1")
                    btnAbrirSolicitud.Visible = true;
            }
        }
    }

    [WebMethod]
    public static List<BandejaSolicitudesViewModel> CargarSolicitudes(int IDSOL)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        List<BandejaSolicitudesViewModel> solicitudes = new List<BandejaSolicitudesViewModel>();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            int IDUSR = GetUSRID(lcURL);
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
            sqlComando.Parameters.AddWithValue("@piIDApp", 107);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
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
                    // informacion del vendedor
                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioVendedor"],
                    fcNombreUsuarioCrea = (string)reader["fcNombreCortoVendedor"],
                    fdFechaCreacionSolicitud = (DateTime)reader["fdFechaCreacionSolicitud"],
                    // informacion del analista
                    fiIDUsuarioModifica = (int)reader["fiIDAnalista"],
                    fcNombreUsuarioModifica = (string)reader["fcNombreCortoAnalista"],
                    //informacion cliente
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fcIdentidadCliente = (string)reader["fcIdentidadCliente"],
                    fcPrimerNombreCliente = (string)reader["fcPrimerNombreCliente"],
                    fcSegundoNombreCliente = (string)reader["fcSegundoNombreCliente"],
                    fcPrimerApellidoCliente = (string)reader["fcPrimerApellidoCliente"],
                    fcSegundoApellidoCliente = (string)reader["fcSegundoApellidoCliente"],
                    //bitacora
                    fdEnIngresoInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnIngresoInicio"]),
                    fdEnIngresoFin = ConvertFromDBVal<DateTime>((object)reader["fdEnIngresoFin"]),
                    fdEnTramiteInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnColaInicio"]),
                    fdEnTramiteFin = ConvertFromDBVal<DateTime>((object)reader["fdEnColaFin"]),
                    fdEnAnalisisInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisInicio"]),
                    fdEnAnalisisFin = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisFin"]),
                    fdCondicionadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdCondicionadoInicio"]),
                    fcCondicionadoComentario = (string)reader["fcCondicionadoComentario"],
                    fdCondificionadoFin = ConvertFromDBVal<DateTime>((object)reader["fdCondificionadoFin"]),
                    //proceso de campo
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
    public static string AbrirAnalisisSolicitud(int IDSOL, string Identidad)
    {
        string resultado = "-1";
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        BandejaSolicitudesViewModel solicitudes = new BandejaSolicitudesViewModel();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            int IDUSR = GetUSRID(lcURL);
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            #region VERIFICAR SI LA SOLICITUD YA ESTABA SIENDO ANALISADA ANTES
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
            sqlComando.Parameters.AddWithValue("@piIDApp", 107);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
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
                //string lcParametros = "usr=" + IDUSR +
                // //"&IDApp=" + pcIDApp +
                // "&IDApp=107"+
                // "&IDSOL=" + IDSOL;

                string lcParametros = "usr=" + IDUSR +
                "&IDApp=107" +
                "&pcID=" + Identidad +
                "&IDSOL=" + IDSOL;
                resultado = DSC.Encriptar(lcParametros);
            }
            else
            {
                resultado = "-1";
            }

            #endregion
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
    public static string EncriptarParametros(int IDSOL, string Identidad)
    {
        string resultado = "-1";
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            int IDUSR = GetUSRID(lcURL);
            string lcParametros = "usr=" + IDUSR +
            //"&IDApp=" + pcIDApp +
            "&IDApp=107" +
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
    public static bool VerificarAnalista(int ID)
    {
        bool resultado = false;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            int idUsuario = GetUSRID(lcURL);
            if (ID == idUsuario)
                resultado = true;
        }
        catch
        {
            resultado = false;
        }
        return resultado;
    }

    public static int GetUSRID(string URL)
    {
        int ID = 0;
        try
        {
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            int liParamStart = 0;
            string lcParametros = "";
            string lcParametroDesencriptado = "";
            Uri lURLDesencriptado = null;
            String pcEncriptado = "";
            liParamStart = URL.IndexOf("?");

            if (liParamStart > 0)
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            else
                lcParametros = String.Empty;

            if (lcParametros != String.Empty)
            {
                pcEncriptado = URL.Substring((liParamStart + 1), URL.Length - (liParamStart + 1));
                lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                ID = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            ID = 0;
        }
        return ID;
    }

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
            return default(T);
        else
            return (T)obj;
    }
}