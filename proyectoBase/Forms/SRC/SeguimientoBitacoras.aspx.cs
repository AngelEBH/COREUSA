﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class SeguimientoBitacoras : System.Web.UI.Page
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
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            /* Agentes activos */
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                string Comando = "EXEC dbo.sp_SRC_CallCenter_AgentesActivos " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario;

                using (var sqlDataAdapter = new SqlDataAdapter(Comando, sqlConexion))
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
            } // using sqlConexion
        } // if lcParametros != string.empty
    }

    [WebMethod]
    public static List<SeguimientoBitacorasViewModel> CargarRegistros(string dataCrypt, int IDAgente, int IDActividad)
    {
        var ListadoRegistros = new List<SeguimientoBitacorasViewModel>();

        if (IDAgente == 0)
            return ListadoRegistros;

        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
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
                            ListadoRegistros.Add(new SeguimientoBitacorasViewModel()
                            {
                                IDAgente = (int)sqlResultado["fiIDUsuario"],
                                NombreAgente = (string)sqlResultado["fcNombreCorto"],
                                IDCliente = (string)sqlResultado["fcIDCliente"],
                                NombreCompletoCliente = (string)sqlResultado["fcNombreSAF"],
                                TelefonoCliente = (string)sqlResultado["fcTelefono"],
                                PrimerComentario = (string)sqlResultado["fcComentario1"],
                                SegundoComentario = (string)sqlResultado["fcComentario2"],
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

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
            return default(T);
        else
            return (T)obj;
    }
}

public class SeguimientoBitacorasViewModel
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