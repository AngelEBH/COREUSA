using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SeguimientoRecuperacionDelDiaPorAgente : System.Web.UI.Page
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
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
        }
        /* FIN de captura de parametros y desencriptado de cadena, realizar validaciones que se lleguen a necesitar */
    }

    [WebMethod]
    public static List<SeguimientoRecuperacionDelDiaPorAgenteViewModel> CargarRegistros(string dataCrypt)
    {
        List<SeguimientoRecuperacionDelDiaPorAgenteViewModel> ListadoRegistros = new List<SeguimientoRecuperacionDelDiaPorAgenteViewModel>();
        try
        {
            /* Desencriptar parametros */
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_SRC_CallCenter_RecuperacionPorAgente", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuarioSupervisor", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDAgente", 0);
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListadoRegistros.Add(new SeguimientoRecuperacionDelDiaPorAgenteViewModel()
                            {
                                IDUsuarioSupervisor = (int)reader["fiIDUsuarioSupervisor"],
                                NombreSupervisor = (string)reader["fcNombreSupervisor"],
                                IDAgente = (int)reader["fiIDUsuarioAgente"],
                                NombreAgente = (string)reader["fcNombreAgente"],
                                IDCliente = (string)reader["fcIDCliente"],
                                NombreCompletoCliente = (string)reader["fcNombreSAF"],
                                Descripcion = (string)reader["fcDescripcion"],
                                DiasAtraso = (short)reader["fiDiasAtraso"],
                                SaldoInicialPonerAlDia = (decimal)reader["fnInicialSaldoPonerAlDia"],
                                AbonosHoy = (decimal)reader["fnAbonosdeHoy"],
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

public class SeguimientoRecuperacionDelDiaPorAgenteViewModel
{
    public int IDUsuarioSupervisor { get; set; }
    public string NombreSupervisor { get; set; }
    public int IDAgente { get; set; }
    public string NombreAgente { get; set; }
    public string IDCliente { get; set; }
    public string NombreCompletoCliente { get; set; }
    public string Descripcion { get; set; }
    public int DiasAtraso { get; set; }
    public decimal SaldoInicialPonerAlDia { get; set; }
    public decimal AbonosHoy { get; set; }
    public string Moneda { get; set; }
}