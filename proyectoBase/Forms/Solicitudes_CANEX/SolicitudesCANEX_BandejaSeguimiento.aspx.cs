using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCANEX_BandejaSeguimiento : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
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
            var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            lcEncriptado = lcEncriptado.Replace("%2f", "/");
            var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            /* Hacer aqui las validaciones que se lleguen a necesitar */
        }
    }

    [WebMethod]
    public static List<SolicitudesCANEX_BandejaSeguimientoViewModel> CargarSolicitudes(string dataCrypt)
    {
        var listadoSolicitudes = new List<SolicitudesCANEX_BandejaSeguimientoViewModel>();
        try
        {
            /* Filtros pendientes */
            var idSocioComercial = 0;
            var idEstadoSolicitud = 0;

            /* Desencriptar parametros */
            var DSC = new DSCore.DataCrypt();
            var lURLDesencriptado = DesencriptarURL(dataCrypt);

            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("dbo.sp_CANEX_Solicitudes", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSocioComercial", idSocioComercial);
                    sqlComando.Parameters.AddWithValue("@piIDEstadoSolicitud", idEstadoSolicitud);

                    using (var reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listadoSolicitudes.Add(new SolicitudesCANEX_BandejaSeguimientoViewModel()
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
        return listadoSolicitudes;
    }

    [WebMethod]
    public static string AbrirSolicitudSeguimientoDetalles(int idSolicitud, string identidad, string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
        string resultado;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string lcParametros = "usr=" + pcIDUsuario +
            "&IDApp=" + pcIDApp +
            "&IDSOL=" + idSolicitud +
            "&SID=" + pcIDSesion +
            "&pcID=" + identidad;
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
            var liParamStart = 0;
            var lcParametros = "";
            var pcEncriptado = string.Empty;
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

public class SolicitudesCANEX_BandejaSeguimientoViewModel
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