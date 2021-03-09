using System;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Collections.Generic;

public partial class Seguimientos_SeguimientoJefePromesasdePago : System.Web.UI.Page
{
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod]
    public static List<SeguimientoJefePromesasPagoViewModel> CargarRegistros(string dataCrypt)
    {
        var listaPromesasDePago = new List<SeguimientoJefePromesasPagoViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("dbo.sp_SRC_AdminSeguimientoPromesasdePago", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDAgente", 0);
                    sqlComando.CommandTimeout = 120;

                    using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listaPromesasDePago.Add(new SeguimientoJefePromesasPagoViewModel()
                            {
                                NombreAgente = sqlResultado["fcNombreCorto"].ToString(),
                                IDCliente = sqlResultado["fcIDCliente"].ToString(),
                                NombreCompletoCliente = sqlResultado["fcNombreCompleto"].ToString(),
                                Atraso = (decimal)sqlResultado["fnSaldoPonerAlDia"],
                                Moneda = "L",
                                DiasMora = (short)sqlResultado["fiDiasAtraso"],
                                UrlCliente = "../Gestion/GestionCobranzaCliente.aspx?" + DSC.Encriptar(sqlResultado["urlCliente"].ToString().Trim()),
                                UrlEstadodeCuenta = sqlResultado["fcURLEstadodeCuenta"].ToString(),
                                UrlImagen = sqlResultado["fcURLImagen"].ToString(),
                                FechaRegistrado = (DateTime)sqlResultado["fdGestion"],
                                FechaPromesa = (DateTime)sqlResultado["fdFechaVolveraLlamar"],
                                EstadoActual = sqlResultado["fcEstadoActual"].ToString()
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
        return listaPromesasDePago;
    }

    public static Uri DesencriptarURL(string Url)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = Url.IndexOf("?");
            var lcParametros = liParamStart > 0 ? Url.Substring(liParamStart, Url.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = Url.Substring(liParamStart + 1, Url.Length - (liParamStart + 1));
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

public class SeguimientoJefePromesasPagoViewModel
{
    public string NombreAgente { get; set; }
    public string IDCliente { get; set; }
    public string NombreCompletoCliente { get; set; }
    public decimal Atraso { get; set; }
    public string Moneda { get; set; }
    public int DiasMora { get; set; }
    public DateTime FechaRegistrado { get; set; }
    public DateTime FechaPromesa { get; set; }
    public string UrlCliente { get; set; }
    public string UrlEstadodeCuenta { get; set; }
    public string UrlImagen { get; set; }
    public string EstadoActual { get; set; }
}