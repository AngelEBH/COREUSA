using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class CFRM_IniciarSesion : System.Web.UI.Page
{
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                // ....
            }
        }
        catch (Exception ex)
        {
            MostrarMensajeError("Ocurrió un error: " + ex.Message.ToString());
        }
    }

    [WebMethod]
    public static ResultadoIniciarSesion_ViewModel IniciarSesion(string usuario, string password, string dataCrypt)
    {
        var resultado = new ResultadoIniciarSesion_ViewModel() { AccesoAutorizado = 0 };
        try
        {
            var generarMD5 = new Funciones.FuncionesComunes();
            var idUsuario = "";
            var idSesion = "";
            var idApp = "108";

            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDSolicitud = lURLDesencriptado != null ? HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("S") : "";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreSeguridad.dbo.sp_MasterLogin", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", idApp);
                    sqlComando.Parameters.AddWithValue("@pcUsuarioDominio", usuario.Trim());
                    sqlComando.Parameters.AddWithValue("@pcPassword", generarMD5.GenerarMD5Hash(password.Trim()));
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultado.AccesoAutorizado = (int)sqlResultado["fiAccesoAutorizado"];
                            resultado.Mensaje = sqlResultado["fcMensaje"].ToString();
                            idUsuario = sqlResultado["fiIDUsuario"].ToString();
                            idSesion = sqlResultado["fiIDSesion"].ToString();
                        }
                    }

                    if (resultado.Mensaje == "Usuario no tiene acceso a esta aplicacion")
                        resultado.AccesoAutorizado = 1;

                    if (resultado.AccesoAutorizado == 1)
                    {
                        HttpContext.Current.Session["AccesoAutorizado"] = resultado.AccesoAutorizado;
                        HttpContext.Current.Session["usr"] = idUsuario;
                        HttpContext.Current.Session["SID"] = idSesion;
                        HttpContext.Current.Session["IDApp"] = idApp;

                        resultado.UrlRedireccion = "CFRM.aspx?" + DSC.Encriptar("S=" + pcIDSolicitud);
                    }
                    else
                    {
                        HttpContext.Current.Session["AccesoAutorizado"] = null;
                        HttpContext.Current.Session["usr"] = null;
                        HttpContext.Current.Session["SID"] = null;
                        HttpContext.Current.Session["IDApp"] = null;

                        resultado.UrlRedireccion = "";
                    }
                } // using sqlCommand
            } // using sqlConnection
        }
        catch (Exception ex)
        {
            resultado.AccesoAutorizado = 0;
            resultado.Mensaje = "Ocurrió un error al inciar sesión: " + ex.Message.ToString();
            resultado.UrlRedireccion = "";
        }
        return resultado;
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = URL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? URL.Substring(liParamStart, URL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = URL.Substring(liParamStart + 1, URL.Length - (liParamStart + 1));
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

    private void MostrarMensajeError(string mensaje)
    {
        divMensajeError.Visible = true;
        lblMensajeError.Text = mensaje;
    }

    public class ResultadoIniciarSesion_ViewModel
    {
        public int AccesoAutorizado { get; set; }
        public string Mensaje { get; set; }
        public string UrlRedireccion { get; set; }
    }
}