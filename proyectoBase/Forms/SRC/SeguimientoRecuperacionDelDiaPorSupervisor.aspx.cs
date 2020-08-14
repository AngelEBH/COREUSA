using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class SeguimientoRecuperacionDelDiaPorSupervisor : System.Web.UI.Page
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

            /* Agentes Activos */
            string sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string Comando = "EXEC dbo.sp_SRC_CallCenter_AgentesActivos " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario;
            //string Comando = "EXEC dbo.sp_SRC_CallCenter_AgentesActivos " + 1 + "," + 101 + "," + 87;
            SqlDataAdapter AdapterDDLCondiciones = new SqlDataAdapter(Comando, sqlConexion);
            DataTable dtAgentes = new DataTable();
            sqlConexion.Open();
            AdapterDDLCondiciones.Fill(dtAgentes);
            ddlAgentesActivos.DataSource = dtAgentes;
            ddlAgentesActivos.DataBind();
            ddlAgentesActivos.DataTextField = "fcNombreCorto";
            ddlAgentesActivos.DataValueField = "fiIDUsuario";
            ddlAgentesActivos.DataBind();
            dtAgentes.Dispose();
            AdapterDDLCondiciones.Dispose();
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
            ddlAgentesActivos.Items.Insert(0, new ListItem("Seleccionar Agente", "0"));
            ddlAgentesActivos.SelectedIndex = 0;
        }
        /* FIN de captura de parametros y desencriptado de cadena, realizar validaciones que se lleguen a necesitar */
    }

    [WebMethod]
    public static List<SeguimientoRecuperacionDelDiaPorSupervisorViewModel> CargarRegistros(string dataCrypt, int IDAgente)
    {
        List<SeguimientoRecuperacionDelDiaPorSupervisorViewModel> ListadoRegistros = new List<SeguimientoRecuperacionDelDiaPorSupervisorViewModel>();
        try
        {
            /* Desencriptar parametros */
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            SqlCommand sqlComando = new SqlCommand("dbo.sp_SRC_CallCenter_RecuperacionPorAgente", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuarioSupervisor", pcIDUsuario);
            sqlComando.Parameters.AddWithValue("@piIDAgente", IDAgente);

            sqlConexion.Open();
            SqlDataReader reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                ListadoRegistros.Add(new SeguimientoRecuperacionDelDiaPorSupervisorViewModel()
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

public class SeguimientoRecuperacionDelDiaPorSupervisorViewModel
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