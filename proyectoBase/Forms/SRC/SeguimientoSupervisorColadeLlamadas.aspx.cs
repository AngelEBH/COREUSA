using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

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
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                string Comando = "EXEC dbo.sp_SRC_CallCenter_AgentesActivos " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario;
                sqlConexion.Open();

                using (SqlDataAdapter AdapterDDLCondiciones = new SqlDataAdapter(Comando, sqlConexion))
                {
                    DataTable dtAgentes = new DataTable();
                    AdapterDDLCondiciones.Fill(dtAgentes);
                    ddlAgentesActivos.DataSource = dtAgentes;
                    ddlAgentesActivos.DataBind();
                    ddlAgentesActivos.DataTextField = "fcNombreCorto";
                    ddlAgentesActivos.DataValueField = "fiIDUsuario";
                    ddlAgentesActivos.DataBind();
                    dtAgentes.Dispose();
                    AdapterDDLCondiciones.Dispose();
                    ddlAgentesActivos.Items.Insert(0, new ListItem("Todos los agentes", "0"));
                    ddlAgentesActivos.SelectedIndex = 0;
                }
            }
        }
        /* FIN de captura de parametros y desencriptado de cadena, realizar validaciones que se lleguen a necesitar */
    }

    [WebMethod]
    public static List<SeguimientoSupervisorColadeLlamadasViewModel> CargarRegistros(string dataCrypt, int IDAgente, int IDActividad)
    {
        List<SeguimientoSupervisorColadeLlamadasViewModel> ListadoRegistros = new List<SeguimientoSupervisorColadeLlamadasViewModel>();

        if (IDActividad == 0)
            return ListadoRegistros;

        try
        {
            /* Desencriptar parametros */
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("dbo.sp_SRC_CallCenter_ColaPorAgente", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDAgente", IDAgente);
                    sqlComando.Parameters.AddWithValue("@piIDActividad", IDActividad);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                                SegundosDuracionLlamada = ConvertFromDBVal<int>((object)reader["fiSegundos"])
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

    [WebMethod]
    public static List<ResumenAgentesViewModel> CargarResumen(string dataCrypt)
    {
        List<ResumenAgentesViewModel> ListadoRegistros = new List<ResumenAgentesViewModel>();

        try
        {
            /* Desencriptar parametros */
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("dbo.sp_SRC_CallCenter_ColaPorAgente_ResumenLlamadas", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ListadoRegistros.Add(new ResumenAgentesViewModel()
                            {
                                NombreAgente = (string)reader["fcAgente"],
                                LlamadasPorHacer = (int)reader["fiClientesPorLlamar"],
                                LlamadasHechas = (int)reader["fiClientesHechas"],
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


public class ResumenAgentesViewModel
{
    public string NombreAgente { get; set; }
    public int LlamadasPorHacer { get; set; }
    public int LlamadasHechas { get; set; }
}