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
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        var lcURL = Request.Url.ToString();
        var liParamStart = lcURL.IndexOf("?");

        string lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

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

                var Comando = "EXEC dbo.sp_SRC_CallCenter_AgentesActivos " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario;

                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(Comando, sqlConexion))
                {
                    DataTable dtAgentes = new DataTable();
                    sqlDataAdapter.Fill(dtAgentes);
                    ddlAgentesActivos.DataSource = dtAgentes;
                    ddlAgentesActivos.DataBind();
                    ddlAgentesActivos.DataTextField = "fcNombreCorto";
                    ddlAgentesActivos.DataValueField = "fiIDUsuario";
                    ddlAgentesActivos.DataBind();
                    dtAgentes.Dispose();
                    sqlDataAdapter.Dispose();
                    ddlAgentesActivos.Items.Insert(0, new ListItem("Seleccionar Agente", "0"));
                    ddlAgentesActivos.SelectedIndex = 0;
                }
            }
        }
    }

    [WebMethod]
    public static List<SeguimientoRecuperacionDelDiaPorSupervisorViewModel> CargarRegistros(string dataCrypt, int IDAgente)
    {
        var ListadoRegistros = new List<SeguimientoRecuperacionDelDiaPorSupervisorViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("dbo.sp_SRC_CallCenter_RecuperacionPorAgente", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuarioSupervisor", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDAgente", IDAgente);
                    sqlComando.CommandTimeout = 120;

                    using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            ListadoRegistros.Add(new SeguimientoRecuperacionDelDiaPorSupervisorViewModel()
                            {
                                IDUsuarioSupervisor = (int)sqlResultado["fiIDUsuarioSupervisor"],
                                NombreSupervisor = sqlResultado["fcNombreSupervisor"].ToString(),
                                IDAgente = (int)sqlResultado["fiIDUsuarioAgente"],
                                NombreAgente = sqlResultado["fcNombreAgente"].ToString(),
                                IDCliente = sqlResultado["fcIDCliente"].ToString(),
                                NombreCompletoCliente = sqlResultado["fcNombreSAF"].ToString(),
                                Descripcion = sqlResultado["fcDescripcion"].ToString(),
                                DiasAtraso = (short)sqlResultado["fiDiasAtraso"],
                                SaldoInicialPonerAlDia = (decimal)sqlResultado["fnInicialSaldoPonerAlDia"],
                                AbonosHoy = (decimal)sqlResultado["fnAbonosdeHoy"],
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
        return ListadoRegistros;
    }

    public static Uri DesencriptarURL(string Url)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;
            var liParamStart = Url.IndexOf("?");

            if (liParamStart > 0)
                lcParametros = Url.Substring(liParamStart, Url.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = Url.Substring((liParamStart + 1), Url.Length - (liParamStart + 1));
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