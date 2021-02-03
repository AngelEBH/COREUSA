using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class Solicitudes_CANEX : System.Web.UI.Page
{
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        var lcURL = Request.Url.ToString();
        var liParamStart = lcURL.IndexOf("?");
        var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

        if (lcParametros != string.Empty)
        {
            var lcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");            
            var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);

            var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");            
            /* Hacer aqui las validaciones que se lleguen a necesitar */
        }
    }

    [WebMethod]
    public static List<SolcitudesCanexViewModel> CargarSolicitudes(string dataCrypt)
    {
        var solicitudesCanex = new List<SolcitudesCanexViewModel>();
        try
        {
            /* Filtros pendientes */
            var idSocioComercial = 0;
            var idEstadoSolicitud = 0;

            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

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

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            solicitudesCanex.Add(new SolcitudesCanexViewModel()
                            {
                                NombreSocio = sqlResultado["fcNombreSocio"].ToString(),
                                NombreAgencia = sqlResultado["fcNombreAgencia"].ToString(),
                                IDSolicitudCanex = (int)sqlResultado["fiIDSolicitudCANEX"],
                                NombreUsuario = sqlResultado["fcNombreUsuario"].ToString(),
                                Identidad = sqlResultado["fcIdentidad"].ToString(),
                                NombreCliente = sqlResultado["fcNombreCliente"].ToString(),
                                NombreProducto = sqlResultado["fcNombreProducto"].ToString(),
                                ValorGlobal = (decimal)sqlResultado["fnValorGlobal"],
                                ValorPrima = (decimal)sqlResultado["fnValorPrima"],
                                ValorPrestamo = (decimal)sqlResultado["fnValorPrestamo"],
                                FechaIngresoSolicitud = (DateTime)sqlResultado["fdIngresoSolicitud"],
                                EstadoSolicitud = sqlResultado["fcEstadoSolicitud"].ToString(),
                                IDEstadoSolicitud = (decimal)sqlResultado["fiEstadoSolicitud"],
                                Moneda = "L"
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {            
            ex.Message.ToString();
        }
        return solicitudesCanex;
    }

    [WebMethod]
    public static string AbrirSolicitudDetalles(int idSolicitud, string identidad, string dataCrypt)
    {
        var resultado = string.Empty;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));

            var lcParametros = "usr=" + pcIDUsuario + "&IDApp=" + pcIDApp + "&IDSOL=" + idSolicitud + "&SID=" + pcIDSesion + "&pcID=" + identidad;
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
            var liParamStart = URL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? URL.Substring(liParamStart, URL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = URL.Substring(liParamStart + 1, URL.Length - (liParamStart + 1));
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