using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class PreSolicitud_Listado : System.Web.UI.Page
{
    private static string pcIDUsuario = "";
    private static string pcIDApp = "";
    private static string pcIDSesion = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
                var lcURL = Request.Url.ToString();
                int liParamStart = lcURL.IndexOf("?");

                string lcParametros;
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
                    var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcEncriptado = lcEncriptado.Replace("%2f", "/");

                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr") ?? "0";
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
    }

    [WebMethod]
    public static List<PreSolicitud_Listado_ViewModel> CargarPreSolicitudes()
    {
        var preSolicitudes = new List<PreSolicitud_Listado_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDPreSolicitudes_Maestro_Listado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            preSolicitudes.Add(new PreSolicitud_Listado_ViewModel()
                            {
                                IdPreSolicitud = int.Parse(reader["fiIDPreSolicitud"].ToString()),
                                IdPais = int.Parse(reader["fiIDPais"].ToString()),
                                IdCanal = int.Parse(reader["fiIDCanal"].ToString()),
                                CentroDeCosto = reader["fcCentrodeCosto"].ToString(),
                                Agencia = reader["fcNombreAgencia"].ToString(),
                                IdentidadCliente = reader["fcIdentidad"].ToString(),
                                NombreCliente = reader["fcNombreCliente"].ToString(),
                                IdGestorValidador = int.Parse(reader["fiIDGestorValidador"].ToString()),
                                GestorValidador = reader["fcGestorValidador"].ToString(),

                                IdUsuarioCra = int.Parse(reader["fiIDUsuarioCreador"].ToString()),
                                UsuarioCrea = reader["fcUsuarioCrea"].ToString(),
                                FechaCreacion = DateTime.Parse(reader["fdFechaCreado"].ToString()),

                                IdUsuarioUltimaModificacion = int.Parse(reader["fiIDUsuarioUltimaModificacion"].ToString()),
                                UsuarioUltimaMoficiacion = reader["fcUsuarioModifica"].ToString(),
                                FechaUltimaModificacion = DateTime.Parse(reader["fdFechaUltimaModificacion"].ToString()),

                                IdEstadoPreSolicitud = int.Parse(reader["fiEstadoPreSolicitud"].ToString()),
                                EstadoPreSolicitud = reader["fcEstadoPreSolicitud"].ToString(),
                                EstadoFavorable = byte.Parse(reader["fiFavorable"].ToString())
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
        return preSolicitudes;
    }

    [WebMethod]
    public static PreSolicitud_Detalles_ViewModel DetallesPreSolicitud(int idPreSolicitud)
    {
        var preSolicitud = new PreSolicitud_Detalles_ViewModel();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDPreSolicitudes_Maestro_Detalles", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDPreSolicitud", idPreSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            preSolicitud = new PreSolicitud_Detalles_ViewModel()
                            {
                                IdPreSolicitud = int.Parse(reader["fiIDPreSolicitud"].ToString()),
                                IdPais = int.Parse(reader["fiIDPais"].ToString()),
                                IdCanal = int.Parse(reader["fiIDCanal"].ToString()),
                                IdTipoDeUbicacion = int.Parse(reader["fiIDTipoDeUbicacion"].ToString()),
                                CentroDeCosto = reader["fcCentrodeCosto"].ToString(),
                                Agencia = reader["fcNombreAgencia"].ToString(),
                                IdentidadCliente = reader["fcIdentidad"].ToString(),
                                NombreCliente = reader["fcNombreCliente"].ToString(),
                                Telefono = reader["fcTelefono"].ToString(),
                                NombreTrabajo = reader["fcNombreTrabajo"].ToString(),
                                TelefonoAdicional = reader["fcTelefonoAdicional"].ToString(),
                                ExtensionRecursosHumanos = reader["fcExtensionRecursosHumanos"].ToString(),
                                ExtensionCliente = reader["fcExtensionCliente"].ToString(),



                                IdDepartamento = int.Parse(reader["fiIDDepartamento"].ToString()),
                                Departamento = reader["fcDepartamento"].ToString(),
                                IdMunicipio = int.Parse(reader["fiIDMunicipio"].ToString()),
                                Municipio = reader["fcMunicipio"].ToString(),
                                IdCiudadPoblado = int.Parse(reader["fiIDCiudad"].ToString()),
                                CiudadPoblado = reader["fcPoblado"].ToString(),
                                IdBarrioColonia = int.Parse(reader["fiIDBarrioColonia"].ToString()),
                                BarrioColonia = reader["fcBarrio"].ToString(),
                                DireccionDetallada = reader["fcDireccionDetallada"].ToString(),
                                ReferenciasDireccionDetallada = reader["fcReferenciasDireccionDetallada"].ToString(),

                                // usuario crea
                                IdUsuarioCra = int.Parse(reader["fiIDUsuarioCreador"].ToString()),
                                UsuarioCrea = reader["fcUsuarioCreador"].ToString(),
                                FechaCreacion = DateTime.Parse(reader["fdFechaCreado"].ToString()),
                                // usuario modifica
                                IdUsuarioUltimaModificacion = int.Parse(reader["fiIDUsuarioUltimaModificacion"].ToString()),
                                UsuarioUltimaMoficiacion = reader["fcUsuarioUltimaModificacion"].ToString(),
                                FechaUltimaModificacion = DateTime.Parse(reader["fdFechaUltimaModificacion"].ToString()),

                                // gestor
                                IdGestorValidador = int.Parse(reader["fiIDGestorValidador"].ToString()),
                                GestorValidador = reader["fcGestorValidador"].ToString(),
                                // validacion de gestoria
                                Latitud = reader["fcLatitud"].ToString(),
                                Longitud = reader["fcLongitud"].ToString(),
                                IdInvestigacionDeCampo = byte.Parse(reader["fiIDInvestigacionDeCampo"].ToString()),

                                TipoResultadoDeCampo = int.Parse(reader["fiTipodeResultado"].ToString()), // para poner clase text danger, success, warining, etc
                                GestionDeCampo = reader["fcGestion"].ToString(), // Del Catalogo de investigaciones
                                FechaValidacion = (DateTime)reader["fdFechaValidacion"],
                                FechaDescargadoPorGestor = (DateTime)reader["fdFechaDescargadaPorGestor"],
                                ObservacionesDeCampo = reader["fcObservacionesCampo"].ToString(),

                                IdEstadoDeGestion = byte.Parse(reader["fiIDEstadoDeGestion"].ToString()), // para cuestiones internas de la bbdd
                                IdEstadoPreSolicitud = byte.Parse(reader["fiEstadoPreSolicitud"].ToString()), // id estado pre solicitud
                                EstadoPreSolicitud = reader["fcEstadoPreSolicitud"].ToString(), // del catalogo de estados de presolucitud
                                EstadoFavorable = byte.Parse(reader["fiFavorable"].ToString()) // para poner clase text danger, success, warining, etc
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return preSolicitud;
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
}

public class PreSolicitud_Listado_ViewModel
{
    public int IdPreSolicitud { get; set; }
    public int IdPais { get; set; }
    public int IdCanal { get; set; }
    public string CentroDeCosto { get; set; }
    public string Agencia { get; set; }
    public string IdentidadCliente { get; set; }
    public string NombreCliente { get; set; }
    public int? IdGestorValidador { get; set; }
    public string GestorValidador { get; set; }
    public int IdUsuarioCra { get; set; }
    public string UsuarioCrea { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public int IdUsuarioUltimaModificacion { get; set; }
    public string UsuarioUltimaMoficiacion { get; set; }
    public DateTime? FechaUltimaModificacion { get; set; }
    public int IdEstadoPreSolicitud { get; set; }
    public string EstadoPreSolicitud { get; set; }
    public int EstadoFavorable { get; set; }
}

public class PreSolicitud_Detalles_ViewModel
{
    public int IdPreSolicitud { get; set; }
    public int IdPais { get; set; }
    public int IdCanal { get; set; }
    public int IdTipoDeSolicitud { get; set; }
    public int IdTipoDeUbicacion { get; set; }
    public string CentroDeCosto { get; set; }
    public string Agencia { get; set; }
    public string IdentidadCliente { get; set; }
    public string NombreCliente { get; set; }
    public string Telefono { get; set; }
    public string TelefonoAdicional { get; set; }
    public string NombreTrabajo { get; set; }
    public string ExtensionRecursosHumanos { get; set; }
    public string ExtensionCliente { get; set; }
    public int IdDepartamento { get; set; }
    public string Departamento { get; set; }
    public int IdMunicipio { get; set; }
    public string Municipio { get; set; }
    public int IdCiudadPoblado { get; set; }
    public string CiudadPoblado { get; set; }
    public int IdBarrioColonia { get; set; }
    public string BarrioColonia { get; set; }
    public string DireccionDetallada { get; set; }
    public string ReferenciasDireccionDetallada { get; set; }
    // usuario crea
    public int IdUsuarioCra { get; set; }
    public string UsuarioCrea { get; set; }
    public DateTime? FechaCreacion { get; set; }
    // usuario ultima modificacion
    public int IdUsuarioUltimaModificacion { get; set; }
    public string UsuarioUltimaMoficiacion { get; set; }
    public DateTime? FechaUltimaModificacion { get; set; }
    // gestor
    public int? IdGestorValidador { get; set; }
    public string GestorValidador { get; set; }
    // validacion de gestoria
    public string Latitud { get; set; }
    public string Longitud { get; set; }
    public int IdInvestigacionDeCampo { get; set; }
    public int TipoResultadoDeCampo { get; set; }
    public string ResultadoDeCampo { get; set; }
    public string GestionDeCampo { get; set; }
    public DateTime FechaDescargadoPorGestor { get; set; }
    public DateTime FechaValidacion { get; set; }
    public string ObservacionesDeCampo { get; set; }
    public int IdEstadoDeGestion { get; set; }
    public int IdEstadoPreSolicitud { get; set; }
    public string EstadoPreSolicitud { get; set; }
    public int EstadoFavorable { get; set; }
}
