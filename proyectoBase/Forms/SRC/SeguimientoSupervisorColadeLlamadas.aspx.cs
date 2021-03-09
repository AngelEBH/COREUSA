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
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        var lcURL = Request.Url.ToString();
        var liParamStart = lcURL.IndexOf("?");
        var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

        if (lcParametros != string.Empty)
        {
            var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            /* Agentes activos */
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("dbo.sp_SRC_CallCenter_AgentesActivos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlAgentesActivos.Items.Clear();
                        ddlAgentesActivos.Items.Add(new ListItem("Seleccionar agente", "0"));

                        while (sqlResultado.Read())
                        {
                            ddlAgentesActivos.Items.Add(new ListItem(sqlResultado["fcNombreCorto"].ToString(), sqlResultado["fiIDUsuario"].ToString()));
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
    }

    [WebMethod]
    public static List<SeguimientoSupervisorColadeLlamadasViewModel> CargarRegistros(string dataCrypt, int IDAgente, int IDActividad)
    {
        var ListadoRegistros = new List<SeguimientoSupervisorColadeLlamadasViewModel>();

        if (IDActividad == 0)
            return ListadoRegistros;

        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("dbo.sp_SRC_CallCenter_ColaPorAgente", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDAgente", IDAgente);
                    sqlComando.Parameters.AddWithValue("@piIDActividad", IDActividad);
                    sqlComando.CommandTimeout = 120;

                    using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            ListadoRegistros.Add(new SeguimientoSupervisorColadeLlamadasViewModel()
                            {
                                IDAgente = (int)sqlResultado["fiIDUsuario"],
                                NombreAgente = sqlResultado["fcNombreCorto"].ToString(),
                                IDCliente = sqlResultado["fcIDCliente"].ToString(),
                                NombreCompletoCliente = sqlResultado["fcNombreSAF"].ToString(),
                                TelefonoCliente = sqlResultado["fcTelefono"].ToString(),
                                PrimerComentario = sqlResultado["fcComentario1"].ToString(),
                                SegundoComentario = sqlResultado["fcComentario2"].ToString(),
                                InicioLlamada = ConvertFromDBVal<DateTime>((object)sqlResultado["fdInicioLlamada"]),
                                FinLlamada = ConvertFromDBVal<DateTime>((object)sqlResultado["fdFinLlamada"]),
                                SegundosDuracionLlamada = ConvertFromDBVal<int>((object)sqlResultado["fiSegundos"])
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
        return ListadoRegistros;
    }

    [WebMethod]
    public static List<ResumenAgentesViewModel> CargarResumen(string dataCrypt)
    {
        var ListadoResumen = new List<ResumenAgentesViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("dbo.sp_SRC_CallCenter_ColaPorAgente_ResumenLlamadas", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            ListadoResumen.Add(new ResumenAgentesViewModel()
                            {
                                NombreAgente = sqlResultado["fcAgente"].ToString(),
                                LlamadasPorHacer = (int)sqlResultado["fiClientesPorLlamar"],
                                LlamadasHechas = (int)sqlResultado["fiClientesHechas"],
                            });
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return ListadoResumen;
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