using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public partial class SolicitudesGPS_RegistroInstalacionGPS : System.Web.UI.Page
{
    #region Propiedades publicas

    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDGarantia = "";
    public string pcIDSolicitudGPS = "";
    public string pcIDSolicitudCredito = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    public List<FotografiaInstalacion_ViewModel> ListaFotografiasInstalacion;

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        if (!IsPostBack && type == null)
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

    #region Funciones utilitarias

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

    #endregion

    #region View models

    public class Resultado_ViewModel
    {
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string MensajeDebug { get; set; }
    }

    public class FotografiaInstalacion_ViewModel
    {
        public int IdFotografia { get; set; }
        public string DescripcionFotografia { get; set; }
    }

    public class InstalacionGPS_ViewModel
    {
        public string DescripcionUbicacion { get; set; }
        public string Comentarios { get; set; }
        public List<InstalacionGPS_Fotografia_ViewModel> Fotografias { get; set; }
    }

    public class InstalacionGPS_Fotografia_ViewModel
    {
        public int IdFotografia { get; set; }
        public string NombreAntiguo { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string RutaArchivo { get; set; }
        public string URL { get; set; }
        public string Comentario { get; set; }
    }

    #endregion
}