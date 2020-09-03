﻿using System;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Services;

public partial class Clientes_BandejaPrecalificados : System.Web.UI.Page
{
    private string pcIDApp = "";
    private string pcIDSesion = "1";
    private string pcIDUsuario = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        string lcURL = Request.Url.ToString();
        int liParamStart = lcURL.IndexOf("?");

        string lcParametros;
        if (liParamStart > 0)
        {
            lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
        }
        else
        {
            lcParametros = String.Empty;
        }
        if (lcParametros != String.Empty)
        {
            string lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            string lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
        }
        /* FIN de captura de parametros y desencriptado de cadena */
    }

    [WebMethod]
    public static List<Clientes_BandejaPrecalificadosViewModel> CargarLista(string dataCrypt, string pcEstado)
    {
        List<Clientes_BandejaPrecalificadosViewModel> listaRegistros = new List<Clientes_BandejaPrecalificadosViewModel>();

        /* Desencriptar parametros */
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
        string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
        string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

        using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
        {
            try
            {
                sqlConexion.Open();
                using (SqlCommand sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_Jefe_ListaClientesEstados", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario.Trim());
                    sqlComando.Parameters.AddWithValue("@piResultadoPrecalificado", pcEstado.Trim());

                    using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listaRegistros.Add(new Clientes_BandejaPrecalificadosViewModel()
                            {
                                Oficial = (string)sqlResultado["fcNombreCorto"],
                                Identidad = (string)sqlResultado["fcIdentidad"],
                                NombreCliente = (string)sqlResultado["fcNombre"],
                                Telefono = (string)sqlResultado["fcTelefono"],
                                Ingresos = (decimal)sqlResultado["fnIngresos"],
                                Moneda = "L",
                                FechaConsultado = (DateTime)sqlResultado["fdFechaPrimerConsulta"],
                                Datelle = (string)sqlResultado["fcMensaje"],
                                Imagen = (string)sqlResultado["fcImagen"],
                                Producto = (string)sqlResultado["fcProducto"]
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        } // using connection

        return listaRegistros;
    }

    [WebMethod]
    public static string EncriptarParametros(string Identidad, string dataCrypt)
    {
        string resultado;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string lcParametros = "usr=" + pcIDUsuario.Trim() +
            "&IDApp=" + pcIDApp.Trim() +
            "&SID=" + pcIDSesion.Trim() +
            "&ID=" + Identidad.Trim();
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

public class Clientes_BandejaPrecalificadosViewModel
{
    public string Oficial { get; set; }
    public string NombreCliente { get; set; }
    public string Identidad { get; set; }
    public decimal Ingresos { get; set; }
    public string Moneda { get; set; }
    public string Telefono { get; set; }
    public string Producto { get; set; }
    public DateTime FechaConsultado { get; set; }
    public string Datelle { get; set; }
    public string Imagen { get; set; }
    public int IDEstado { get; set; }
    public string Estado { get; set; }
}