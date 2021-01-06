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
    public static List<PreSolicitud_ViewModel> CargarPreSolicitudes()
    {
        var listadoDePreSolicitudes = new List<PreSolicitud_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDPreSolicitudes_Maestro_Listado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listadoDePreSolicitudes.Add(new PreSolicitud_ViewModel()
                            {
                                IdPreSolicitud = int.Parse(sqlResultado["fiIDPreSolicitud"].ToString()),
                                IdPais = int.Parse(sqlResultado["fiIDPais"].ToString()),
                                IdCanal = int.Parse(sqlResultado["fiIDCanal"].ToString()),
                                CentroDeCosto = sqlResultado["fcCentrodeCosto"].ToString(),
                                Agencia = sqlResultado["fcNombreAgencia"].ToString(),
                                IdentidadCliente = sqlResultado["fcIdentidad"].ToString(),
                                NombreCliente = sqlResultado["fcNombreCliente"].ToString(),
                                IdGestorValidador = int.Parse(sqlResultado["fiIDGestorValidador"].ToString()),
                                GestorValidador = sqlResultado["fcGestorValidador"].ToString(),
                                IdUsuarioCrea = int.Parse(sqlResultado["fiIDUsuarioCreador"].ToString()),
                                UsuarioCrea = sqlResultado["fcUsuarioCrea"].ToString(),
                                FechaCreacion = DateTime.Parse(sqlResultado["fdFechaCreado"].ToString()),
                                IdUsuarioUltimaModificacion = int.Parse(sqlResultado["fiIDUsuarioUltimaModificacion"].ToString()),
                                UsuarioUltimaMoficiacion = sqlResultado["fcUsuarioModifica"].ToString(),
                                FechaUltimaModificacion = DateTime.Parse(sqlResultado["fdFechaUltimaModificacion"].ToString()),
                                IdEstadoPreSolicitud = int.Parse(sqlResultado["fiEstadoPreSolicitud"].ToString()),
                                EstadoPreSolicitud = sqlResultado["fcEstadoPreSolicitud"].ToString(),
                                EstadoFavorable = byte.Parse(sqlResultado["fiFavorable"].ToString())
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return listadoDePreSolicitudes;
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

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            preSolicitud = new PreSolicitud_Detalles_ViewModel()
                            {
                                IdPreSolicitud = int.Parse(sqlResultado["fiIDPreSolicitud"].ToString()),
                                IdPais = int.Parse(sqlResultado["fiIDPais"].ToString()),
                                IdCanal = int.Parse(sqlResultado["fiIDCanal"].ToString()),
                                IdTipoDeUbicacion = int.Parse(sqlResultado["fiIDTipoDeUbicacion"].ToString()),
                                CentroDeCosto = sqlResultado["fcCentrodeCosto"].ToString(),
                                Agencia = sqlResultado["fcNombreAgencia"].ToString(),
                                IdentidadCliente = sqlResultado["fcIdentidad"].ToString(),
                                NombreCliente = sqlResultado["fcNombreCliente"].ToString(),
                                Telefono = sqlResultado["fcTelefono"].ToString(),
                                NombreTrabajo = sqlResultado["fcNombreTrabajo"].ToString(),
                                TelefonoAdicional = sqlResultado["fcTelefonoAdicional"].ToString(),
                                ExtensionRecursosHumanos = sqlResultado["fcExtensionRecursosHumanos"].ToString(),
                                ExtensionCliente = sqlResultado["fcExtensionCliente"].ToString(),
                                IdDepartamento = int.Parse(sqlResultado["fiIDDepartamento"].ToString()),
                                Departamento = sqlResultado["fcDepartamento"].ToString(),
                                IdMunicipio = int.Parse(sqlResultado["fiIDMunicipio"].ToString()),
                                Municipio = sqlResultado["fcMunicipio"].ToString(),
                                IdCiudadPoblado = int.Parse(sqlResultado["fiIDCiudad"].ToString()),
                                CiudadPoblado = sqlResultado["fcPoblado"].ToString(),
                                IdBarrioColonia = int.Parse(sqlResultado["fiIDBarrioColonia"].ToString()),
                                BarrioColonia = sqlResultado["fcBarrio"].ToString(),
                                DireccionDetallada = sqlResultado["fcDireccionDetallada"].ToString(),
                                ReferenciasDireccionDetallada = sqlResultado["fcReferenciasDireccionDetallada"].ToString(),
                                // usuario crea
                                IdUsuarioCrea = int.Parse(sqlResultado["fiIDUsuarioCreador"].ToString()),
                                UsuarioCrea = sqlResultado["fcUsuarioCreador"].ToString(),
                                FechaCreacion = DateTime.Parse(sqlResultado["fdFechaCreado"].ToString()),
                                // usuario modifica
                                IdUsuarioUltimaModificacion = int.Parse(sqlResultado["fiIDUsuarioUltimaModificacion"].ToString()),
                                UsuarioUltimaMoficiacion = sqlResultado["fcUsuarioUltimaModificacion"].ToString(),
                                FechaUltimaModificacion = DateTime.Parse(sqlResultado["fdFechaUltimaModificacion"].ToString()),
                                // gestor
                                IdGestorValidador = int.Parse(sqlResultado["fiIDGestorValidador"].ToString()),
                                GestorValidador = sqlResultado["fcGestorValidador"].ToString(),
                                // validacion de gestoria
                                Latitud = sqlResultado["fcLatitud"].ToString(),
                                Longitud = sqlResultado["fcLongitud"].ToString(),
                                IdInvestigacionDeCampo = byte.Parse(sqlResultado["fiIDInvestigacionDeCampo"].ToString()),
                                TipoResultadoDeCampo = int.Parse(sqlResultado["fiTipodeResultado"].ToString()), // para poner clase text danger, success, warining, etc
                                GestionDeCampo = sqlResultado["fcGestion"].ToString(), // Del Catalogo de investigaciones
                                FechaValidacion = (DateTime)sqlResultado["fdFechaValidacion"],
                                FechaDescargadoPorGestor = (DateTime)sqlResultado["fdFechaDescargadaPorGestor"],
                                ObservacionesDeCampo = sqlResultado["fcObservacionesCampo"].ToString(),
                                IdEstadoDeGestion = byte.Parse(sqlResultado["fiIDEstadoDeGestion"].ToString()), // para cuestiones internas de la bbdd
                                IdEstadoPreSolicitud = byte.Parse(sqlResultado["fiEstadoPreSolicitud"].ToString()), // id estado pre solicitud
                                EstadoPreSolicitud = sqlResultado["fcEstadoPreSolicitud"].ToString(), // del catalogo de estados de presolucitud
                                EstadoFavorable = byte.Parse(sqlResultado["fiFavorable"].ToString()) // para poner clase text danger, success, warining, etc
                            };
                        }

                        /****** Documentos de la solicitud ******/
                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            preSolicitud.Documentos.Add(new PreSolicitudDocumentosSolicitud_ViewModel()
                            {
                                IdPreSolicitudDocumento = (int)sqlResultado["fiIDPreSolicitudDocumento"],
                                IdPreSolicitud = (int)sqlResultado["fiIDPreSolicitud"],
                                NombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                Extension = sqlResultado["fcTipoArchivo"].ToString(),
                                RutaArchivo = sqlResultado["fcRutaArchivo"].ToString(),
                                URLArchivo = sqlResultado["fcURL"].ToString(),
                                IdTipoDocumento = (int)sqlResultado["fiTipoDocumento"],
                                DescripcionTipoDocumento = sqlResultado["fcDescripcionTipoDocumento"].ToString(),
                                ArchivoActivo = (byte)sqlResultado["fiArchivoActivo"]
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

public class PreSolicitud_ViewModel
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
    public int IdUsuarioCrea { get; set; }
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
    public int IdUsuarioCrea { get; set; }
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
    public List<PreSolicitudDocumentosSolicitud_ViewModel> Documentos { get; set; }

    public PreSolicitud_Detalles_ViewModel()
    {
        Documentos = new List<PreSolicitudDocumentosSolicitud_ViewModel>();
    }
}


public class PreSolicitudDocumentosSolicitud_ViewModel
{
    public int IdPreSolicitudDocumento { get; set; }
    public int IdPreSolicitud { get; set; }
    public string NombreArchivo { get; set; }
    public int IdTipoDocumento { get; set; }
    public string DescripcionTipoDocumento { get; set; }
    public string Extension { get; set; }
    public string RutaArchivo { get; set; }
    public string URLArchivo { get; set; }
    public byte ArchivoActivo { get; set; }
}