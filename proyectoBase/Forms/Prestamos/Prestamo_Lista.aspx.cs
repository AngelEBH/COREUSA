using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

public partial class Prestamos_Prestamo_Lista : System.Web.UI.Page
{
    private string pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        var DSC = new DSCore.DataCrypt();
        var lcURL = Request.Url.ToString();
        var liParamStart = lcURL.IndexOf("?");

        string lcParametros;
        if (liParamStart > 0)
            lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
        else
            lcParametros = string.Empty;

        if (lcParametros != string.Empty)
        {
            pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
            var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        }

        String lcSQLInstruccion = "";
        String sqlConnectionString = "";
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {
            sqlConexion.Open();

            lcSQLInstruccion = "exec sp_Prestamo_Lista " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario.Trim();
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            //gvMisPrestamos.DataSource = sqlResultado;
            //gvMisPrestamos.DataBind();
            sqlComando.Dispose();
            sqlConexion.Close();
        }
        catch (Exception ex)
        {
            //PanelErrores.Visible = true;
            //lblMensaje.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }


    }

    [WebMethod]
    public static List<Prestamos_ViewModel> CargarPrestamos(string dataCrypt)
    {
        var solicitudes = new List<Prestamos_ViewModel>();
        var DSC = new DSCore.DataCrypt();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Prestamo_Lista", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            solicitudes.Add(new Prestamos_ViewModel()
                            {
                                fcNombreCliente = sqlResultado["fcNombreCliente"].ToString(),
                                 fiIDSolicitud =Convert.ToInt32(sqlResultado["fiIDSolicitud"]),
                                 fcIDPrestamo = sqlResultado["fcIDPrestamo"].ToString(),
                                 fiIDProducto = Convert.ToInt32(sqlResultado["fiIDProducto"]),
                                 fcProducto = sqlResultado["fcProducto"].ToString(),
                                 fdFechaCreacion = (DateTime)sqlResultado["fdFechaCreacion"],
                                 fnCapitalFinanciado = Convert.ToDouble(sqlResultado["fnCapitalFinanciado"]),
                                fnSaldoActualCapital = Convert.ToDouble(sqlResultado["fnSaldoActualCapital"]),
                                 fcEstadoPrestamo = sqlResultado["fcEstadoPrestamo"].ToString(),
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
        return solicitudes;
    }


    [WebMethod]
    public static string EncriptarParametros(string pcIDPrestamo, string dataCrypt, string identidad)
    {
        var DSC = new DSCore.DataCrypt();
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            string lcParametros = "usr=" + pcIDUsuario +
            "&IDApp=" + pcIDApp +
            "&SID=" + pcIDSesion +
            "&pcID=" + identidad +
            "&Pre=" + pcIDPrestamo;
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
            var DSC = new DSCore.DataCrypt();
            var liParamStart = 0;
            string lcParametros = "";
            var pcEncriptado = "";
            liParamStart = URL.IndexOf("?");
            if (liParamStart > 0)
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = URL.Substring((liParamStart + 1), URL.Length - (liParamStart + 1));
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

#region View Models

public class Prestamos_ViewModel
{
    public string fcNombreCliente { get; set; }
    public int fiIDSolicitud { get; set; }
    public string fcIDPrestamo { get; set; }
    public int fiIDProducto { get; set; }
    public string fcProducto { get; set; }
    public DateTime fdFechaCreacion { get; set; }
    public double fnCapitalFinanciado { get; set; }
    public double fnSaldoActualCapital { get; set; }
    public string fcEstadoPrestamo { get; set; }

}

#endregion