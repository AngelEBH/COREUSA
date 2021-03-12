using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Services;

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
            var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var lcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");
                var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                pcIDApp = HttpContext.Current.Session["IDApp"].ToString();
                pcIDSesion = HttpContext.Current.Session["SID"].ToString();
                pcIDUsuario = HttpContext.Current.Session["usr"].ToString();
                pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("S");
                lblNoSolicitudCredito.Text = pcIDSolicitud;

                if (pcIDSolicitud != "" && pcIDSolicitud != "0")
                    CargarExpedienteDeLaSolicitud();
                else
                    MostrarMensaje("Parámetros inválidos.");
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
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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

                        var idUsuarioCreadorExpediente = sqlResultado["fiIDUsuarioCreador"].ToString();
                        var idEstadoExpediente = (int)sqlResultado["fiIDEstadoExpediente"];
                        var usuarioCreador = sqlResultado["fcUsuarioCreador"].ToString();
                        var fechaCreacion = (DateTime)sqlResultado["fdFechaCreacion"];

                        var idUsuarioUltimaModificacion = (int)sqlResultado["fiIDUsuarioUltimaModificacion"];
                        var usuarioUltimaModificacion = sqlResultado["fcUsuarioUltimaModificacion"].ToString();
                        var fechaUltimaModificacion = (DateTime)sqlResultado["fdFechaUltimaModificacion"];

                        switch (idEstadoExpediente)
                        {
                            case 1: // Estado "Creado" solo el usuario creador podrá pasarlo a "En tránsito"
                                divCambiarEstadoExpediente.Visible = idUsuarioCreadorExpediente == pcIDUsuario;
                                break;

                            case 2: // Estado "En Transito" solo el usuario 211 podrá cambiar el estado a estado 3 "Recibido Mariely Guzman" *
                            case 3: /* Si está en estado "Recibido Mariely Guzman" cualquier usuario puede cambiar el estado a "Entregado Abogado" */
                            case 4: /* Si está en estado "Entregado Abogado" cualquier usuario puede cambiar el estado a "Recibido Abogado" */
                                if (pcIDUsuario == "211" || pcIDUsuario == "77")
                                    divCambiarEstadoExpediente.Visible = true;
                                else
                                    divCambiarEstadoExpediente.Visible = false;
                                break;
                            case 5: /* Si está en estado "Recibido Abogado" solo el usuario 89 puede cambiar el estado a "Entregado Archivo"*/
                            case 6: /* Si está en estado "Entregado Archivo" solo el usuario 89 puede cambiar el estado a "Archivado"*/
                                divCambiarEstadoExpediente.Visible = pcIDUsuario == "89";
                                break;
                            default:

                                divCambiarEstadoExpediente.Visible = false;
                                break;
                        }

                        txtEspecifiqueOtras.InnerText = sqlResultado["fcComentarios"].ToString();
                        divEstadoExpediente.InnerText = "Estado actual: " + sqlResultado["fcEstadoExpediente"].ToString();
                        divEstadoExpediente.Attributes.Add("class", "badge badge-" + sqlResultado["fcEstadoExpedienteClassName"].ToString() + " font-12");

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
                            sqlResultado["fcDocumento"].ToString() +
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

                        /* Tercer resultado: Listado de tipos de solicitudes */
                        sqlResultado.NextResult();

                        /* Cuarto resultado: Siguiente estado del expediente */
                        sqlResultado.NextResult();
                        sqlResultado.Read();

                        var idSiguienteEstadoExpediente = (int)sqlResultado["fiIDEstadoExpedienteSiguiente"];

                        if (idSiguienteEstadoExpediente > 0)
                        {
                            btnCambiarEstadoExpediente.Visible = true;
                            btnCambiarEstadoExpediente.Attributes.Add("data-idsiguienteestado", idSiguienteEstadoExpediente.ToString());
                            btnCambiarEstadoExpediente.Attributes.Add("class", "btn btn-" + sqlResultado["fcClassNameSiguiente"].ToString());
                            lblSiguienteEstadoExpediente.InnerText = sqlResultado["fcEstadoExpedienteSiguiente"].ToString();
                        }
                        else
                        {
                            btnCambiarEstadoExpediente.Visible = false;
                            btnCambiarEstadoExpediente.Disabled = true;
                        }

                        /* Quinto resultado: Listar historial de movimientos de un expediente */
                        sqlResultado.NextResult();

                        var templateHistorial = new StringBuilder();

                        templateHistorial.Append("<li>" +
                            "<span class='text-muted'>" + usuarioCreador + " - " + fechaCreacion.ToString("MM/dd/yyyy hh:mm tt") + "</span>" +
                            "<br />" +
                            "<span>Expediente creado</span>" +
                        "</li>");

                        if (idUsuarioUltimaModificacion != 0)
                        {
                            templateHistorial.Append("<li class='mt-2'>" +
                            "<span class='text-muted'>" + usuarioUltimaModificacion + " - " + fechaUltimaModificacion.ToString("MM/dd/yyyy hh:mm tt") + "</span>" +
                            "<br />" +
                            "<span>Última modificación</span>" +
                        "</li>");
                        }

                        while (sqlResultado.Read())
                        {
                            templateHistorial.Append("<li class='mt-2'>" +
                                "<span class='text-muted'>" + sqlResultado["fcNombreCorto"].ToString() + " - " + DateTime.Parse(sqlResultado["fdFecha"].ToString()).ToString("MM/dd/yyyy hh:mm tt") + "</span>" +
                                "<br />" +
                                "<span>" + sqlResultado["fcComentarios"].ToString() + "</span>" +
                                "</li>");
                        }

                        ulHistorialExpediente.InnerHtml = templateHistorial.ToString();
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar el expediente de la solicitud " + pcIDSolicitud + ": " + ex.Message.ToString());
        }
    }

    [WebMethod]
    public static Resultado_ViewModel CambiarEstadoExpediente(int idEstado, string dataCrypt)
    {
        var resultado = new Resultado_ViewModel() { SesionValida = true, ResultadoExitoso = true };
        try
        {

            if (HttpContext.Current.Session["AccesoAutorizado"] != null)
            {
                if ((int)HttpContext.Current.Session["AccesoAutorizado"] != 1)
                {
                    resultado.ResultadoExitoso = false;
                    resultado.SesionValida = false;
                    resultado.MensajeResultado = "La sesión caducó, vuelve a iniciar sesión.";
                    return resultado;
                }
            }
            else
            {
                resultado.ResultadoExitoso = false;
                resultado.SesionValida = false;
                resultado.MensajeResultado = "La sesión caducó, vuelve a iniciar sesión.";
                return resultado;
            }

            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpContext.Current.Session["IDApp"].ToString();
            var pcIDSesion = HttpContext.Current.Session["SID"].ToString();
            var pcIDUsuario = HttpContext.Current.Session["usr"].ToString();
            var pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("S");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Expediente_CambiarEstadoExpediente", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDEstadoExpediente", idEstado);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        sqlResultado.Read();

                        if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                        {
                            resultado.ResultadoExitoso = false;
                            resultado.MensajeResultado = "No se pudo cambiar el estado del expediente: " + sqlResultado["MensajeError"].ToString() + ", contacte al administrador.";
                        }
                    } // using sqlComando.ExecuteReader()
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            resultado.ResultadoExitoso = false;
            resultado.MensajeResultado = "No se pudo cambiar el estado del expediente: " + ex.Message.ToString() + ", contacte al administrador.";
        }
        return resultado;
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
        var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

        Response.Write("<script>window.open('CFRM_IniciarSesion.aspx" + lcParametros + "','_self')</script>");
        Response.End();
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

    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text = mensaje;
    }

    public class Resultado_ViewModel
    {
        public bool ResultadoExitoso { get; set; }
        public bool SesionValida { get; set; }
        public string MensajeResultado { get; set; }
    }
}