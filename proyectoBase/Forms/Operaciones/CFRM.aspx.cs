using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

public partial class CFRM : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDSolicitud = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            ValidarInicioDeSesion();

            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            var lcParametros = string.Empty;

            if (liParamStart > 0)
                lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                var lcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");
                var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("S");
                pcIDUsuario = HttpContext.Current.Session["usr"].ToString();
                pcIDSesion = HttpContext.Current.Session["SID"].ToString();
                pcIDApp = HttpContext.Current.Session["IDApp"].ToString();

                lblNoSolicitudCredito.Text = pcIDSolicitud;

                if (pcIDSolicitud != "" && pcIDSolicitud != "0")
                {
                    CargarExpedienteDeLaSolicitud();
                }

            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
        }
    }

    public void CargarExpedienteDeLaSolicitud()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Expediente_ObtenerPorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /* Primer resultado: Información principal del expediente*/
                        sqlResultado.Read();

                        txtNombreCliente.Text = sqlResultado["fcNombreCliente"].ToString();
                        txtIdentidadCliente.Text = sqlResultado["fcIdentidadCliente"].ToString();
                        txtRtn.Text = sqlResultado["fcRTN"].ToString();
                        txtTelefonoCliente.Text = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                        txtProducto.Text = sqlResultado["fcProducto"].ToString();
                        txtMontoAFinanciar.Text = sqlResultado["fnMontoFinalFinanciar"].ToString();
                        txtPlazo.Text = sqlResultado["fiPlazoFinalAprobado"].ToString();
                        txtOficialDeNegocios.Text = sqlResultado["fcUsuarioAsignado"].ToString();
                        txtGestorDeCobros.Text = sqlResultado["fcGestorAsignado"].ToString();
                        txtEspecifiqueOtras.InnerText = sqlResultado["fcComentarios"].ToString();

                        /* Segundo resultado: Documentos del expediente */
                        sqlResultado.NextResult();

                        var templateDocumentosExpediente = new StringBuilder();

                        var idEstadoDocumento = 0;
                        var estadoDocumento = string.Empty;
                        var estadoDocumentoClassName = string.Empty;

                        while (sqlResultado.Read())
                        {
                            idEstadoDocumento = (int)sqlResultado["fiIDEstadoDocumento"];
                            estadoDocumento = sqlResultado["fcEstadoDocumento"].ToString();
                            estadoDocumentoClassName = idEstadoDocumento == 1 ? "success" : idEstadoDocumento == 2 ? "danger" : idEstadoDocumento == 3 ? "warning" : "secondary";

                            templateDocumentosExpediente.Append(
                            "<li class='list-group-item'>" +
                            "<div class='todo-indicator bg-" + estadoDocumentoClassName + "'></div>" +
                            "<div class='widget-content p-0'>" +
                            "<div class='widget-content-wrapper'>" +
                            "<div class='widget-content-left flex2'>" +
                            "<div class='widget-heading'>" +
                            sqlResultado["fcDescripcionDocumento"].ToString() +
                            "</div>" +
                            "</div>" +
                            "<div class='widget-content-right'>" +
                            "<div class='badge badge-" + estadoDocumentoClassName + " ml-2 font-12'>" + estadoDocumento + "</div>" +
                            "</div>" +
                            "</div>" +
                            "</div>" +
                            "</li>");
                        }

                        ulDocumentosExpediente.InnerHtml = templateDocumentosExpediente.ToString();

                        /* Listado de tipos de solicitudes */
                        sqlResultado.NextResult();
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar el expediente de la solicitud " + pcIDSolicitud + ": " + ex.Message.ToString());
        }
    }

    private void ValidarInicioDeSesion()
    {
        try
        {
            if (HttpContext.Current.Session["AccesoAutorizado"] != null)
            {
                if ((int)HttpContext.Current.Session["AccesoAutorizado"] != 1)
                    RedireccionarLogIn();
            }
            else RedireccionarLogIn();
        }
        catch (Exception ex)
        {
            MostrarMensaje("Ocurrió un error al validar la sesión: " + ex.Message.ToString());
        }
    }

    private void RedireccionarLogIn()
    {
        var lcURL = Request.Url.ToString();
        var liParamStart = lcURL.IndexOf("?");
        var lcParametros = string.Empty;

        if (liParamStart > 0)
            lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
        else
            lcParametros = string.Empty;

        Response.Write("<script>window.open('CFRM_IniciarSesion.aspx" + lcParametros + "','_self')</script>");
        Response.End();
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = 0;
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;
            liParamStart = URL.IndexOf("?");

            if (liParamStart > 0)
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            else
                lcParametros = string.Empty;

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