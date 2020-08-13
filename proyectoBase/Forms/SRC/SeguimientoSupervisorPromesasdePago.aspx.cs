using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SeguimientoSupervisorPromesasdePago : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();

        string lcURL = Request.Url.ToString();
        int liParamStart = lcURL.IndexOf("?");

        string lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : String.Empty;

        if (lcParametros != String.Empty)
        {
            string lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            string lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            string lcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string lcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string lcIDEstado = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDEstado");
        }
        /* FIN de captura de parametros y desencriptado de cadena, realizar validaciones que se lleguen a necesitar */
    }

    [WebMethod]
    public static List<SeguimientoSupervisorPromesasPagoViewModel> CargarSolicitudes(string dataCrypt)
    {
        List<SeguimientoSupervisorPromesasPagoViewModel> ListadoRegistros = new List<SeguimientoSupervisorPromesasPagoViewModel>();
        try
        {
            /* Desencriptar parametros */
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            string sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            SqlCommand sqlComando = new SqlCommand("dbo.sp_SRC_AdminSeguimientoPromesasdePago", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@piIDApp", 101);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", 87);
            sqlComando.Parameters.AddWithValue("@piIDAgente", 0);

            sqlConexion.Open();
            SqlDataReader reader = sqlComando.ExecuteReader();

            while (reader.Read())
            {
                ListadoRegistros.Add(new SeguimientoSupervisorPromesasPagoViewModel()
                {
                    NombreAgente = (string)reader["fcNombreCorto"],
                    IDCliente = (string)reader["fcIDCliente"],
                    NombreCompletoCliente = (string)reader["fcNombreCompleto"],
                    Atraso = (decimal)reader["fnSaldoPonerAlDia"],
                    Moneda = "L",
                    DiasMora = (short)reader["fiDiasAtraso"],
                    UrlCliente = "../Gestion/GestionCobranzaCliente.aspx?" + DSC.Encriptar(reader["urlCliente"].ToString().Trim()),
                    UrlEstadodeCuenta = (string)reader["fcURLEstadodeCuenta"],
                    UrlImagen = (string)reader["fcURLImagen"],
                    FechaRegistrado = (DateTime)reader["fdGestion"],
                    FechaPromesa = (DateTime)reader["fdFechaVolveraLlamar"],
                    EstadoActual = (string)reader["fcEstadoActual"]
                });
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return ListadoRegistros;
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

public class SeguimientoSupervisorPromesasPagoViewModel
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