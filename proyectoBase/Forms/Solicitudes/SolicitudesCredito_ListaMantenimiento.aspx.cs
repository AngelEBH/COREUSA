using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_ListaMantenimiento : System.Web.UI.Page
{
    private String pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";

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
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
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
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
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
    public static List<ListaMantenimientoViewModel> CargarSolicitudes(string dataCrypt)
    {
        List<ListaMantenimientoViewModel> solicitudes = new List<ListaMantenimientoViewModel>();
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            solicitudes.Add(new ListaMantenimientoViewModel()
                            {
                                IDSolicitud = (int)reader["fiIDSolicitud"],
                                IDTipoPrestamo = (int)reader["fiIDTipoProducto"],
                                DescripcionTipoPrestamo = (string)reader["fcProducto"],
                                IdentidadCliente = (string)reader["fcIdentidadCliente"],
                                NombreCliente = (string)reader["fcPrimerNombreCliente"] + " " + (string)reader["fcSegundoNombreCliente"] + " " + (string)reader["fcPrimerApellidoCliente"] + " " + (string)reader["fcSegundoApellidoCliente"],
                                FechaIngreso = (DateTime)reader["fdFechaCreacionSolicitud"],                                
                                IDEstado = (byte)reader["fiEstadoSolicitud"],
                                DescripcionEstado = (string)reader["fcEstadoSolicitud"],
                                EstadoDeCampo = (byte)reader["fiEstadoDeCampo"]
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

public class ListaMantenimientoViewModel
{
    public int IDSolicitud { get; set; }
    public string IdentidadCliente { get; set; }
    public  string NombreCliente { get; set; }
    public int IDTipoPrestamo { get; set; }
    public string DescripcionTipoPrestamo { get; set; }
    public DateTime FechaIngreso { get; set; }
    public int EstadoDeCampo { get; set; }
    public int IDEstado { get; set; }
    public string DescripcionEstado { get; set; }
}