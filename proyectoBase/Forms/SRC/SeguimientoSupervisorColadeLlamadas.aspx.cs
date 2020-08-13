using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SeguimientoSupervisorColadeLlamadas : System.Web.UI.Page
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
            //string Comando = "EXEC dbo.sp_SRC_CallCenter_AgentesActivos " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario;
            string Comando = "EXEC dbo.sp_SRC_CallCenter_AgentesActivos " + 1 + "," + 101 + "," + 87;
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
        }
        /* FIN de captura de parametros y desencriptado de cadena, realizar validaciones que se lleguen a necesitar */
    }

    [WebMethod]
    public static List<SeguimientoSupervisorColadeLlamadasViewModel> CargarSolicitudes(string dataCrypt, int IDAgente, int IDActividad)
    {
        List<SeguimientoSupervisorColadeLlamadasViewModel> ListadoRegistros = new List<SeguimientoSupervisorColadeLlamadasViewModel>();
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
            SqlCommand sqlComando = new SqlCommand("dbo.sp_SRC_CallCenter_ColaPorAgente", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", 87);
            sqlComando.Parameters.AddWithValue("@piIDAgente", IDAgente);
            sqlComando.Parameters.AddWithValue("@piIDActividad", IDActividad);
            sqlConexion.Open();
            SqlDataReader reader = sqlComando.ExecuteReader();

            while (reader.Read())
            {
                ListadoRegistros.Add(new SeguimientoSupervisorColadeLlamadasViewModel()
                {
                    IDAgente = (int)reader["fiIDUsuario"],
                    NombreAgente = (string)reader["fcNombreCorto"],
                    IDCliente = (string)reader["fcIDCliente"],
                    NombreCompletoCliente = (string)reader["fcNombreSAF"],
                    TelefonoCliente = (string)reader["fcTelefono"],
                    PrimerComentario = (string)reader["fcComentario1"],
                    SegundoComentario = (string)reader["fcComentario2"],
                    InicioLlamada = ConvertFromDBVal<DateTime>((object)reader["fdInicioLlamada"]),
                    FinLlamada = ConvertFromDBVal<DateTime>((object)reader["fdFinLlamada"]),
                    SegundosDuracionLlamada = (int)reader["fiSegundos"]
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

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
            return default(T);
        else
            return (T)obj;
    }
}

public class SeguimientoSupervisorColadeLlamadasViewModel
{
    public int IDAgente { get; set; }
    public string NombreAgente { get; set; }
    public string IDCliente { get; set; }
    public string NombreCompletoCliente { get; set; }
    public string TelefonoCliente { get; set; }
    public string PrimerComentario { get; set; }
    public string SegundoComentario { get; set; }
    public DateTime InicioLlamada { get; set; }
    public DateTime FinLlamada { get; set; }
    public int SegundosDuracionLlamada { get; set; }
}