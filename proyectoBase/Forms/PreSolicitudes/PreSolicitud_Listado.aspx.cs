using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class PreSolicitud_Listado : System.Web.UI.Page
{
    private static string pcIDUsuario = "";
    private static string pcIDApp = "";
    private static string pcIDSesion = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
                var lcURL = Request.Url.ToString();
                int liParamStart = lcURL.IndexOf("?");

                string lcParametros;
                if (liParamStart > 0)
                {
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                }
                else
                {
                    lcParametros = String.Empty;
                }

                if (lcParametros != String.Empty)
                {
                    var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcEncriptado = lcEncriptado.Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr") ?? "0";
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                }
            }
            catch (Exception ex)
            {
                //MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
            }
        }
    }

    [WebMethod]
    public static string EncriptarParametros(int IDSOL, string dataCrypt, string Identidad)
    {
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string lcParametros = "usr=" + pcIDUsuario +
            "&IDApp=" + pcIDApp +
            "&SID=" + pcIDSesion +
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
    public static List<PreSolicitud_Listado_ViewModel> CargarPreSolicitudes()
    {
        var preSolicitudes = new List<PreSolicitud_Listado_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDPreSolicitudes_Maestro_Listado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            preSolicitudes.Add(new PreSolicitud_Listado_ViewModel()
                            {
                                IdPreSolicitud = int.Parse(reader["fiIDPreSolicitud"].ToString()),
                                IdPais = int.Parse(reader["fiIDPais"].ToString()),
                                IdCanal = int.Parse(reader["fiIDCanal"].ToString()),
                                CentroDeCosto = reader["fcCentrodeCosto"].ToString(),
                                Agencia = reader["fcNombreAgencia"].ToString(),
                                IdentidadCliente = reader["fcIdentidad"].ToString(),
                                NombreCliente = reader["fcNombreCliente"].ToString(),
                                IdGestorValidador = int.Parse(reader["fiIDGestorValidador"].ToString()),
                                GestorValidador = reader["fcGestorValidador"].ToString(),

                                IdUsuarioCra = int.Parse(reader["fiIDUsuarioCrea"].ToString()),
                                UsuarioCrea = reader["fcUsuarioCrea"].ToString(),
                                FechaCreacion = DateTime.Parse(reader["fdFechaCrea"].ToString()),

                                IdUsuarioUltimaModificacion = int.Parse(reader["fiIDUsuarioModificacion"].ToString()),
                                UsuarioUltimaMoficiacion = reader["fcUsuarioModifica"].ToString(),
                                FechaUltimaModificacion = DateTime.Parse(reader["fdFechaUltimaModificacion"].ToString()),

                                IdEstadoPreSolicitud = int.Parse(reader["fiEstadoPreSolicitud"].ToString()),
                                EstadoPreSolicitud = reader["fcEstadoPreSolicitud"].ToString(),
                                EstadoFavorable = byte.Parse(reader["fiFavorable"].ToString())
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
        return preSolicitudes;
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
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
}

public class PreSolicitud_Listado_ViewModel
{
    public int IdPreSolicitud { get; set; }
    public int IdPais { get; set; }
    public int IdCanal { get; set; }
    public string CentroDeCosto { get; set; }
    public string Agencia { get; set; }
    public string IdentidadCliente { get; set; }
    public string NombreCliente { get; set; }
    public int? IdGestorValidador { get; set; }
    public string GestorValidador { get; set; }
    public int IdUsuarioCra { get; set; }
    public string UsuarioCrea { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public int IdUsuarioUltimaModificacion { get; set; }
    public string UsuarioUltimaMoficiacion { get; set; }
    public DateTime? FechaUltimaModificacion { get; set; }
    public int IdEstadoPreSolicitud { get; set; }
    public string EstadoPreSolicitud { get; set; }
    public int EstadoFavorable { get; set; }
}

public class PreSolicitud_Detalles_ViewModel
{
    public int IdPreSolicitud { get; set; }
    public int IdPais { get; set; }
    public int IdCanal { get; set; }
    public string CentroDeCosto { get; set; }
    public string Agencia { get; set; }
    public string IdentidadCliente { get; set; }
    public string NombreCliente { get; set; }
    public int? IdGestorValidador { get; set; }
    public string GestorValidador { get; set; }
    public int IdUsuarioCra { get; set; }
    public string UsuarioCrea { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public int IdUsuarioUltimaModificacion { get; set; }
    public string UsuarioUltimaMoficiacion { get; set; }
    public DateTime? FechaUltimaModificacion { get; set; }
    public int IdEstadoPreSolicitud { get; set; }
    public string EstadoPreSolicitud { get; set; }
    public int EstadoFavorable { get; set; }
}
