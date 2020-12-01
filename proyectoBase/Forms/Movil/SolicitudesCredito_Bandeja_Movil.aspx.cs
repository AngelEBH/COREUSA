using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Bandeja_Movil : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            var lcParametros = string.Empty;

            if (liParamStart > 0)
            {
                lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
            }
            else
            {
                lcParametros = string.Empty;
            }

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));                
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            }
        }
    }

    [WebMethod]
    public static string EncriptarParametros(int idSolicitud, string dataCrypt)
    {
        string resultado;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);            
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            var lcParametros = "usr=" + pcIDUsuario +
            "&IDApp=" + pcIDApp +
            "&SID=" + pcIDSesion +
            "&IDSOL=" + idSolicitud;

            resultado = DSC.Encriptar(lcParametros);
        }
        catch
        {
            resultado = "-1";
        }
        return resultado;
    }

    [WebMethod]
    public static List<SolicitudesCredito_Bandeja_Movil_ViewModel> CargarSolicitudes(string dataCrypt)
    {
        var listaSolicitudes = new List<SolicitudesCredito_Bandeja_Movil_ViewModel>();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitud_ListarSolicitudesPorUsuario", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaSolicitudes.Add(new SolicitudesCredito_Bandeja_Movil_ViewModel()
                            {
                                fiIDSolicitud = (int)reader["fiIDSolicitud"],
                                fiIDTipoPrestamo = (int)reader["fiIDTipoProducto"],
                                fcDescripcion = (string)reader["fcProducto"],
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCreacionSolicitud = (DateTime)reader["fdFechaCreacionSolicitud"],
                                //informacion cliente
                                fiIDCliente = (int)reader["fiIDCliente"],
                                fcIdentidadCliente = (string)reader["fcIdentidadCliente"],
                                fcPrimerNombreCliente = (string)reader["fcPrimerNombreCliente"],
                                fcSegundoNombreCliente = (string)reader["fcSegundoNombreCliente"],
                                fcPrimerApellidoCliente = (string)reader["fcPrimerApellidoCliente"],
                                fcSegundoApellidoCliente = (string)reader["fcSegundoApellidoCliente"],
                                //bitacora
                                fdEnTramiteInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnColaInicio"]),
                                fdEnTramiteFin = ConvertFromDBVal<DateTime>((object)reader["fdEnColaFin"]),
                                fdEnAnalisisInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisInicio"]),
                                fdEnAnalisisFin = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisFin"]),
                                fdCondicionadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdCondicionadoInicio"]),
                                fdCondificionadoFin = ConvertFromDBVal<DateTime>((object)reader["fdCondificionadoFin"]),
                                //proceso de campo
                                fdEnCampoInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnRutaDeInvestigacionInicio"]),
                                fdEnCampoFin = ConvertFromDBVal<DateTime>((object)reader["fdEnRutaDeInvestigacionFin"]),
                                fdReprogramadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdReprogramadoInicio"]),
                                fdReprogramadoFin = ConvertFromDBVal<DateTime>((object)reader["fdReprogramadoFin"]),
                                fiEstadoSolicitud = ConvertFromDBVal<byte>((object)reader["fiEstadoSolicitud"]),
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
        return listaSolicitudes;
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = URL.IndexOf("?");
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;

            if (liParamStart > 0)
            {
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            }
            else
            {
                lcParametros = string.Empty;
            }

            if (lcParametros != string.Empty)
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

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
            return default(T);
        else
            return (T)obj;
    }
}

public class SolicitudesCredito_Bandeja_Movil_ViewModel
{
    // informacion principal de la solicitud
    public int fiIDSolicitud { get; set; }
    public int fiIDTipoPrestamo { get; set; }
    public string fcDescripcion { get; set; }
    public decimal fdValorPmoSugeridoSeleccionado { get; set; }
    public int fiPlazoPmoSeleccionado { get; set; }
    public System.DateTime fdFechaCreacionSolicitud { get; set; }
    public int fiSolicitudActiva { get; set; }

    public int fiIDUsuarioCrea { get; set; }

    public Nullable<int> fiEstadoSolicitud { get; set; }

    // informacion del cliente
    public int fiIDCliente { get; set; }
    public string fcIdentidadCliente { get; set; }
    public string fcTelefonoCliente { get; set; }
    public System.DateTime fdFechaNacimientoCliente { get; set; }
    public string fcCorreoElectronicoCliente { get; set; }
    public string fcProfesionOficioCliente { get; set; }
    public string fcSexoCliente { get; set; }
    public int fiIDEstadoCivil { get; set; }
    public int fiIDVivienda { get; set; }
    public Nullable<short> fiTiempoResidir { get; set; }
    public bool fbClienteActivo { get; set; }
    public string fcRazonInactivo { get; set; }
    public string fcPrimerNombreCliente { get; set; }
    public string fcSegundoNombreCliente { get; set; }
    public string fcPrimerApellidoCliente { get; set; }
    public string fcSegundoApellidoCliente { get; set; }

    // auditoria
    public System.DateTime fdFechaCrea { get; set; }

    // informacion del procesamiento de la solicitud
    public Nullable<System.DateTime> fdEnTramiteInicio { get; set; }
    public Nullable<System.DateTime> fdEnTramiteFin { get; set; }
    public Nullable<System.DateTime> fdEnAnalisisInicio { get; set; }
    public Nullable<System.DateTime> fdEnAnalisisFin { get; set; }
    public Nullable<System.DateTime> fdCondicionadoInicio { get; set; }
    public Nullable<System.DateTime> fdCondificionadoFin { get; set; }

    public int fiEstadoDeCampo { get; set; }
    public Nullable<System.DateTime> fdEnvioARutaAnalista { get; set; }
    public Nullable<System.DateTime> fdEnCampoInicio { get; set; }
    public Nullable<System.DateTime> fdEnCampoFin { get; set; }
    public Nullable<System.DateTime> fdReprogramadoInicio { get; set; }
    public Nullable<System.DateTime> fdReprogramadoFin { get; set; }
}