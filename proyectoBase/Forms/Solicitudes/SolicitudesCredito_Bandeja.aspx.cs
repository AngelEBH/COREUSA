using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Bandeja : System.Web.UI.Page
{
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1));
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + DSC.Desencriptar(pcEncriptado));

                var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "1";
                var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

                if (pcIDUsuario.Trim() == "142" || pcIDUsuario.Trim() == "1" || pcIDUsuario.Trim() == "146")
                {
                    btnAbrirAnalisis.Visible = true;
                }
            }
        }
    }

    [WebMethod]
    public static List<SolicitudesCredito_Bandeja_ViewModel> CargarSolicitudes(string dataCrypt)
    {
        var solicitudes = new List<SolicitudesCredito_Bandeja_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "1";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Bandeja", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 240;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            solicitudes.Add(new SolicitudesCredito_Bandeja_ViewModel()
                            {
                                IdSoliciud = (int)sqlResultado["fiIDSolicitud"],
                                IdProducto = (int)sqlResultado["fiIDTipoProducto"],
                                Producto = (string)sqlResultado["fcProducto"],
                                Agencia = (string)sqlResultado["fcNombreAgencia"],
                                /* Informacion del vendedor */
                                IdUsuarioAsignado = (int)sqlResultado["fiIDUsuarioVendedor"],
                                UsuarioAsignado = (string)sqlResultado["fcNombreCortoVendedor"],
                                FechaCreacionSolicitud = (DateTime)sqlResultado["fdFechaCreacionSolicitud"],
                                /* Informacion del analista */
                                IdAnalistaSolicitud = (int)sqlResultado["fiIDAnalista"],
                                AnalistaSolicitud = (string)sqlResultado["fcNombreCortoAnalista"],
                                /* Informacion cliente */
                                IdCliente = (int)sqlResultado["fiIDCliente"],
                                IdentidadCliente = (string)sqlResultado["fcIdentidadCliente"],
                                NombreCliente = sqlResultado["fcNombreCliente"].ToString(),
                                PrimerNombreCliente = (string)sqlResultado["fcPrimerNombreCliente"],
                                SegundoNombreCliente = (string)sqlResultado["fcSegundoNombreCliente"],
                                PrimerApellidoCliente = (string)sqlResultado["fcPrimerApellidoCliente"],
                                SegundoApellidoCliente = (string)sqlResultado["fcSegundoApellidoCliente"],
                                /* Bitacora */
                                EnIngresoInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdEnIngresoInicio"]),
                                EnIngresoFin = ConvertFromDBVal<DateTime>(sqlResultado["fdEnIngresoFin"]),
                                EnTramiteInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdEnColaInicio"]),
                                EnTramiteFin = ConvertFromDBVal<DateTime>(sqlResultado["fdEnColaFin"]),
                                EnAnalisisInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdEnAnalisisInicio"]),
                                EnAnalisisFin = ConvertFromDBVal<DateTime>(sqlResultado["fdEnAnalisisFin"]),
                                CondicionadoInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdCondicionadoInicio"]),
                                CondicionadoComentario = (string)sqlResultado["fcCondicionadoComentario"],
                                CondificionadoFin = ConvertFromDBVal<DateTime>(sqlResultado["fdCondificionadoFin"]),
                                /* Proceso de campo */
                                EnvioARutaAnalista = ConvertFromDBVal<DateTime>(sqlResultado["fdEnvioARutaAnalista"]),
                                IdEstadoDeCampo = (byte)sqlResultado["fiEstadoDeCampo"],
                                EnCampoInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdEnRutaDeInvestigacionInicio"]),
                                ObservacionesDeGestoria = (string)sqlResultado["fcObservacionesDeCampo"],
                                EnCampoFin = ConvertFromDBVal<DateTime>(sqlResultado["fdEnRutaDeInvestigacionFin"]),
                                ReprogramadoInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdReprogramadoInicio"]),
                                ReprogramadoComentario = (string)sqlResultado["fcReprogramadoComentario"],
                                ReprogramadoFin = ConvertFromDBVal<DateTime>(sqlResultado["fdReprogramadoFin"]),
                                PasoFinalInicio = (DateTime)sqlResultado["fdPasoFinalInicio"],
                                IdUsuarioPasoFinal = (int)sqlResultado["fiIDUsuarioPasoFinal"],
                                ComentarioPasoFinal = (string)sqlResultado["fcComentarioPasoFinal"],
                                PasoFinalFin = (DateTime)sqlResultado["fdPasoFinalFin"],
                                IdEstadoSolicitud = (byte)sqlResultado["fiEstadoSolicitud"],
                                FechaResolucion = (DateTime)sqlResultado["fdTiempoTomaDecisionFinal"]
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using SqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return solicitudes;
    }

    [WebMethod]
    public static string AbrirAnalisis(int idSolicitud, string identidad, string dataCrypt)
    {
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "1";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            /* Validar que solo el primer usuario en iniciar el analisis de la solicitud tenga acceso al analisis de la misma */
            var lcParametros = "usr=" + pcIDUsuario + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion + "&pcID=" + identidad + "&IDSOL=" + idSolicitud;

            resultado = DSC.Encriptar(lcParametros);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = "-1";
        }
        return resultado;
    }

    [WebMethod]
    public static string EncriptarParametros(int idSolicitud, string identidad, string dataCrypt)
    {
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "1";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var lcParametros = "usr=" + pcIDUsuario + "&IDApp=" + pcIDApp + "&pcID=" + identidad + "&IDSOL=" + idSolicitud;

            resultado = DSC.Encriptar(lcParametros);
        }
        catch
        {
            resultado = "-1";
        }
        return resultado;
    }

    [WebMethod]
    public static bool VerificarAnalista(string idAnalista, string dataCrypt)
    {
        var resultado = false;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "1";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            if (idAnalista == pcIDUsuario)
                resultado = true;
        }
        catch
        {
            resultado = false;
        }
        return resultado;
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var pcEncriptado = string.Empty;
            var liParamStart = URL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? URL.Substring(liParamStart, URL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = URL.Substring(liParamStart + 1, URL.Length - (liParamStart + 1));
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

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
            return default(T);
        else
            return (T)obj;
    }

    #region View Models

    public class SolicitudesCredito_Bandeja_ViewModel
    {
        public int IdSoliciud { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public DateTime FechaCreacionSolicitud { get; set; }
        public int SolicitudActiva { get; set; }
        public int IdUsuarioCreador { get; set; }
        public string UsuarioCreador { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public string UsuarioAsignado { get; set; }
        public int IdAgencia { get; set; }
        public string Agencia { get; set; }
        public int IdAnalistaSolicitud { get; set; }
        public string AnalistaSolicitud { get; set; }
        public int IdGestorAsignado { get; set; }
        public string GestorAsignado { get; set; }
        public int IdEstadoSolicitud { get; set; }
        public int IdCliente { get; set; }
        public string IdentidadCliente { get; set; }
        public bool ClienteActivo { get; set; }
        public string RazonInactivo { get; set; }
        public string NombreCliente { get; set; }
        public string PrimerNombreCliente { get; set; }
        public string SegundoNombreCliente { get; set; }
        public string PrimerApellidoCliente { get; set; }
        public string SegundoApellidoCliente { get; set; }
        public DateTime EnIngresoInicio { get; set; }
        public DateTime EnIngresoFin { get; set; }
        public DateTime EnTramiteInicio { get; set; }
        public DateTime EnTramiteFin { get; set; }
        public DateTime EnAnalisisInicio { get; set; }
        public DateTime FechaValidacionInformacionPersonal { get; set; }
        public string ComentarioValidacionInfoPersonal { get; set; }
        public DateTime FechaValidacionDocumentacion { get; set; }
        public string ComentarioValidacionDocumentacion { get; set; }
        public DateTime FechaValidacionReferenciasPersonales { get; set; }
        public string ComentarioValidacionReferenciasPersonales { get; set; }
        public DateTime FechaValidacionInformacionLaboral { get; set; }
        public string ComentarioValidacionInformacionLaboral { get; set; }
        public DateTime FechaResolucion { get; set; }
        public string ObservacionesDeCredito { get; set; }
        public string ComentarioResolucion { get; set; }
        public DateTime EnAnalisisFin { get; set; }
        public DateTime CondicionadoInicio { get; set; }
        public string CondicionadoComentario { get; set; }
        public DateTime CondificionadoFin { get; set; }
        public int IdEstadoDeCampo { get; set; }
        public DateTime EnvioARutaAnalista { get; set; }
        public DateTime EnCampoInicio { get; set; }
        public string ObservacionesDeGestoria { get; set; }
        public DateTime EnCampoFin { get; set; }
        public DateTime ReprogramadoInicio { get; set; }
        public string ReprogramadoComentario { get; set; }
        public DateTime ReprogramadoFin { get; set; }
        public DateTime PasoFinalInicio { get; set; }
        public int IdUsuarioPasoFinal { get; set; }
        public string ComentarioPasoFinal { get; set; }
        public DateTime PasoFinalFin { get; set; }
    }
    #endregion
}