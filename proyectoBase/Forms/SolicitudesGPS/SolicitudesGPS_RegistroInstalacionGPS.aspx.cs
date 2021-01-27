using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesGPS_RegistroInstalacionGPS : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            try
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

                if (lcParametros != string.Empty)
                {
                    var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcEncriptado = lcEncriptado.Replace("%2f", "/");
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
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
            }
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
                            txtIMEI.Text = sqlResultado["fcIMEI"].ToString();
                            txtSerie.Text = sqlResultado["fcSerie"].ToString();
                            txtModeloGPS.Text = sqlResultado["fcGPSModel"].ToString();
                            txtCompania.Text = sqlResultado["fcGPSCompania"].ToString();
                            txtConRelay.Text = (bool)sqlResultado["fiConRelay"] == true ? "SI" : "NO";

                            txtVIN.Text = sqlResultado["fcVIN"].ToString();
                            txtTipoDeGarantia.Text = sqlResultado["fcTipoGarantia"].ToString();
                            txtTipoDeVehiculo.Text = sqlResultado["fcTipoVehiculo"].ToString();
                            txtMarca.Text = sqlResultado["fcMarca"].ToString();
                            txtModelo.Text = sqlResultado["fcModelo"].ToString();
                            txtAnio.Text = sqlResultado["fiAnio"].ToString();
                            txtColor.Text = sqlResultado["fcColor"].ToString();
                            txtMatricula.Text = sqlResultado["fcMatricula"].ToString();
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

    [WebMethod]
    public static Resultado_ViewModel FinalizarRevisionGarantia(List<Garantia_Revision_ViewModel> revisionesGarantia, decimal recorrido, string unidadDeDistancia, string dataCrypt)
    {
        var resultado = new Resultado_ViewModel() { ResultadoExitoso = true };
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDGarantia = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDGarantia");
            var pcIDSolicitudGPS = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSolicitudGPS") ?? "0";
            var pcIDSolicitudCredito = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlTransaccion = sqlConexion.BeginTransaction("revisionGarantia"))
                {
                    try
                    {
                        foreach (Garantia_Revision_ViewModel item in revisionesGarantia)
                        {
                            using (var sqlComando = new SqlCommand("sp_CREDGarantias_Revisiones_Guardar", sqlConexion, sqlTransaccion))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                sqlComando.Parameters.AddWithValue("@piIDGarantia", pcIDGarantia);
                                sqlComando.Parameters.AddWithValue("@piIDRevision", item.IdRevision);
                                sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", pcIDSolicitudGPS);
                                sqlComando.Parameters.AddWithValue("@piEstadoRevision", item.IdEstadoRevision);
                                sqlComando.Parameters.AddWithValue("@pcObservaciones", item.Observaciones.Trim());
                                sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                                using (var sqlResultado = sqlComando.ExecuteReader())
                                {
                                    sqlResultado.Read();

                                    if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                    {
                                        sqlTransaccion.Rollback();

                                        resultado.ResultadoExitoso = false;
                                        resultado.MensajeResultado = "Error al guardar resultado de la revisión: " + item.Revision + ", contacte al aministrador.";
                                        resultado.MensajeDebug = "_Revisiones_Guardar";
                                        return resultado;
                                    }
                                }
                            }
                        }

                        using (var sqlComando = new SqlCommand("sp_CREDGarantias_Revisiones_ActualizarMillaje", sqlConexion, sqlTransaccion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDGarantia", pcIDGarantia);
                            sqlComando.Parameters.AddWithValue("@pnRecorrido", recorrido);
                            sqlComando.Parameters.AddWithValue("@pcUnidadDeDistancia", unidadDeDistancia);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                sqlResultado.Read();

                                if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                {
                                    sqlTransaccion.Rollback();

                                    resultado.ResultadoExitoso = false;
                                    resultado.MensajeResultado = "Error al actualizar millaje de la garantía, contacte al aministrador.";
                                    resultado.MensajeDebug = "_Revisiones_ActualizarMillaje";
                                    return resultado;
                                }
                            }
                        }

                        if (resultado.ResultadoExitoso == true)
                        {
                            sqlTransaccion.Commit();

                            resultado.MensajeResultado = "Las revisiones se guardaron exitosamente";
                            resultado.MensajeDebug = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        sqlTransaccion.Rollback();

                        resultado.ResultadoExitoso = false;
                        resultado.MensajeResultado = "Error al guardar resultado de la revisión: " + ex.Message.ToString() + ". Contacte al aministrador.";
                        resultado.MensajeDebug = "_Revisiones_Guardar";
                    }
                } // using sqlTransacciomn
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado.ResultadoExitoso = false;
            resultado.MensajeResultado = "Error al guardar resultado de la revisión: " + ex.Message.ToString() + ". Contacte al aministrador.";
            resultado.MensajeDebug = "_Revisiones_Guardar";
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

    public class Garantia_Revision_ViewModel
    {
        public int IdGarantiaRevision { get; set; }
        public int IdGarantia { get; set; }
        public int IdRevision { get; set; }
        public string Revision { get; set; }
        public int IdSolicitudGPS { get; set; }
        public int IdEstadoRevision { get; set; }
        public string EstadoRevision { get; set; }
        public string Observaciones { get; set; }
    }

    public class Resultado_ViewModel
    {
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string MensajeDebug { get; set; }
    }
}