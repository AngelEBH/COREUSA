using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

public partial class SolicitudesGPS_DetallesInstalacionGPS : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDGarantia = "";
    public string pcIDSolicitudGPS = "";
    public string pcIDSolicitudCredito = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        try
        {
            if (!IsPostBack && type == null)
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

                if (lcParametros != string.Empty)
                {
                    var lcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDGarantia = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDGarantia");
                    pcIDSolicitudGPS = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSolicitudGPS") ?? "0";
                    pcIDSolicitudCredito = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

                    CargarInformacion();
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
        }
    }

    private void CargarInformacion()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("AutoLoan.dbo.sp_AutoGPS_Instalacion_GetById", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", pcIDSolicitudGPS);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            Response.Write("<script>window.open('SolicitudesCredito_ListadoGarantias.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&SID=" + pcIDSesion + "&IDApp=" + pcIDApp) + "','_self')</script>");
                        }

                        while (sqlResultado.Read())
                        {
                            lblVIN.InnerText = sqlResultado["fcVIN"].ToString();
                            lblMarca.InnerText = sqlResultado["fcMarca"].ToString();
                            lblModelo.InnerText = sqlResultado["fcModelo"].ToString();
                            lblAnio.InnerText = sqlResultado["fiAnio"].ToString();
                            lblColor.InnerText = sqlResultado["fcColor"].ToString();
                            lblPlaca.InnerText = sqlResultado["fcMatricula"].ToString();

                            txtIMEI.Text = sqlResultado["fcIMEI"].ToString();
                            txtSerie.Text = sqlResultado["fcSerie"].ToString();
                            txtModeloGPS.Text = sqlResultado["fcGPSModel"].ToString();
                            txtCompania.Text = sqlResultado["fcGPSCompania"].ToString();
                            txtConRelay.Text = (bool)sqlResultado["fiConRelay"] == true ? "SI" : "NO";
                            txtUbicacion.InnerText = sqlResultado["fcDescripcionUbicacion"].ToString();
                            txtComentariosDeLaInstalacion.InnerText = sqlResultado["fcObservacionesInstalacion"].ToString();
                        }
                    } // using sqlResultado
                } // using sqlComando

                using (var sqlComando = new SqlCommand("AutoLoan.dbo.sp_AutoGPS_Instalacion_Fotografias_GetByIDAutoGPSInstalacion", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", pcIDSolicitudGPS);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            divGaleriaGarantia.InnerHtml = "<img alt='No hay fotografías disponibles' src='/Imagenes/Imagen_no_disponible.png' data-image='/Imagenes/Imagen_no_disponible.png' data-description='No hay fotografías disponibles'/>";
                        }
                        else
                        {
                            var imagenesGarantia = new StringBuilder();
                            var url = string.Empty;

                            while (sqlResultado.Read())
                            {
                                url = @"http://172.20.3.140/" + sqlResultado["fcURL"].ToString();

                                imagenesGarantia.Append("<img alt='" + sqlResultado["fcDescripcionFotografia"] + "' src='" + url + "' data-image='" + url + "' data-description='" + sqlResultado["fcDescripcionFotografia"] + "'/>");
                            }
                            divGaleriaGarantia.InnerHtml = imagenesGarantia.ToString();
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar información: " + ex.Message.ToString());
        }
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = URL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? URL.Substring(liParamStart, URL.Length - liParamStart) : string.Empty;
            var pcEncriptado = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = URL.Substring(liParamStart + 1, URL.Length - (liParamStart + 1));
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

    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text = mensaje;
    }
}