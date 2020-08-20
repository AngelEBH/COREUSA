using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class Solicitudes_CANEX : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
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
            string lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            lcEncriptado = lcEncriptado.Replace("%2f", "/");
            string lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            /* Hacer aqui las validaciones que se lleguen a necesitar */
        }
    }

    [WebMethod]
    public static List<SolcitudesCanexViewModel> CargarSolicitudes(string dataCrypt)
    {
        List<SolcitudesCanexViewModel> ListadoSolicitudes = new List<SolcitudesCanexViewModel>();
        try
        {
            /* Filtros pendientes */
            int IDSocioComercial = 0; int IDEstadoSolicitud = 0;

            /* Desencriptar parametros */
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);

            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("dbo.sp_CANEX_Solicitudes", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSocioComercial", IDSocioComercial);
                    sqlComando.Parameters.AddWithValue("@piIDEstadoSolicitud", IDEstadoSolicitud);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var x = reader["fiEstadoSolicitud"].GetType();
                            ListadoSolicitudes.Add(new SolcitudesCanexViewModel()
                            {
                                NombreSocio = (string)reader["fcNombreSocio"],
                                NombreAgencia = (string)reader["fcNombreAgencia"],
                                IDSolicitudCanex = (int)reader["fiIDSolicitudCANEX"],
                                NombreUsuario = (string)reader["fcNombreUsuario"],
                                Identidad = (string)reader["fcIdentidad"],
                                NombreCliente = (string)reader["fcNombreCliente"],
                                NombreProducto = (string)reader["fcNombreProducto"],
                                ValorGlobal = (decimal)reader["fnValorGlobal"],
                                ValorPrima = (decimal)reader["fnValorPrima"],
                                ValorPrestamo = (decimal)reader["fnValorPrestamo"],
                                FechaIngresoSolicitud = (DateTime)reader["fdIngresoSolicitud"],
                                EstadoSolicitud = (string)reader["fcEstadoSolicitud"],
                                IDEstadoSolicitud = (decimal)reader["fiEstadoSolicitud"],
                                Moneda = "L"
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
        return ListadoSolicitudes;
    }

    [WebMethod]
    public static string AbrirSolicitudDetalles(int ID, string Identidad, string dataCrypt)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            DateTime fechaActual = DateTime.Now;

            string lcParametros = "usr=" + pcIDUsuario +
            "&IDApp=" + pcIDApp +
            "&IDSOL=" + ID +
            "&SID=" + pcIDSesion +
            "&pcID=" + Identidad;
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
}

public class SolcitudesCanexViewModel
{
    public string NombreSocio { get; set; }
    public string NombreAgencia { get; set; }
    public int IDSolicitudCanex { get; set; }
    public string Identidad { get; set; }
    public string NombreCliente { get; set; }
    public string NombreProducto { get; set; }
    public decimal ValorGlobal { get; set; }
    public decimal ValorPrima { get; set; }
    public decimal ValorPrestamo { get; set; }
    public string Moneda { get; set; }
    public DateTime FechaIngresoSolicitud { get; set; }
    public string NombreUsuario { get; set; }
    public string EstadoSolicitud { get; set; }
    public decimal IDEstadoSolicitud { get; set; }
}