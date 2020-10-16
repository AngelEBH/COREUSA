using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_ListadoGarantias : System.Web.UI.Page
{
    private string pcIDApp = "";
    private string pcIDSesion = "";
    private string pcIDUsuario = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var DSC = new DSCore.DataCrypt();
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
            }
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

                    using (var reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listado.Add(new SolicitudesCredito_ListadoGarantias_ViewModel()
                            {
                                IdSolicitud = (int)reader["fiIDSolicitud"],                                
                                Agencia = (string)reader["fcAgencia"],
                                IdProducto = (int)reader["fiIDTipoProducto"],
                                Producto = (string)reader["fcProducto"],
                                FechaCreacion = (DateTime)reader["fdFechaCreacionSolicitud"],
                                IdEstadoSolicitud = (byte) reader["fiEstadoSolicitud"],
                                IdUsuarioAsignado = (int)reader["fiIDUsuarioAsignado"],
                                IdUsuarioCreador = (int)reader["fiIDUsuarioCrea"],
                                FechaCreado = (DateTime)reader["fdFechaCrea"],
                                SolicitudActiva = (byte)reader["fiSolicitudActiva"],
                                IdCliente = (int)reader["fiIDCliente"],
                                Identidad = (string)reader["fcIdentidadCliente"],
                                PrimerNombre = (string)reader["fcPrimerNombreCliente"],
                                SegundoNombre = (string)reader["fcSegundoNombreCliente"],
                                PrimerApellido = (string)reader["fcPrimerApellidoCliente"],
                                SegundoApellido = (string)reader["fcSegundoApellidoCliente"],
                                IdGarantia = (int)reader["fiIDGarantia"],
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
    public static string EncriptarParametros(int idSolicitud, string dataCrypt)
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
            "&IDSOL=" + idSolicitud;
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
}

public class SolicitudesCredito_ListadoGarantias_ViewModel
{
    public int IdSolicitud { get; set; }
    public string Agencia { get; set; }
    public int IdProducto { get; set; }
    public string Producto { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int IdEstadoSolicitud { get; set; }
    public int IdUsuarioAsignado { get; set; }
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
}