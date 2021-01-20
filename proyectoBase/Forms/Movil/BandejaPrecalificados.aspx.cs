using System;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Services;

public partial class Clientes_BandejaPrecalificados : System.Web.UI.Page
{
    private string pcIDApp = "";
    private string pcIDSesion = "";
    private string pcIDUsuario = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        var lcURL = Request.Url.ToString();
        int liParamStart = lcURL.IndexOf("?");

        string lcParametros;

        if (liParamStart > 0)
            lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
        else
            lcParametros = string.Empty;

        if (lcParametros != string.Empty)
        {
            var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
        }
    }

    [WebMethod]
    public static List<Clientes_BandejaPrecalificadosViewModel> CargarLista(string dataCrypt, string pcEstado)
    {
        var listaRegistros = new List<Clientes_BandejaPrecalificadosViewModel>();

        var lURLDesencriptado = DesencriptarURL(dataCrypt);
        var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

        using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
        {
            try
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_Jefe_ListaClientesEstados", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario.Trim());
                    sqlComando.Parameters.AddWithValue("@piResultadoPrecalificado", pcEstado.Trim());
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
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
        }

        return listaRegistros;
    }

    [WebMethod]
    public static string EncriptarParametros(string Identidad, string dataCrypt)
    {
        var resultado = string.Empty;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

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