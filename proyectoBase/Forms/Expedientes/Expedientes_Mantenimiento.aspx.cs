using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class Expedientes_Mantenimiento : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

                if (lcParametros != string.Empty)
                {
                    var pcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    #endregion

    #region Cargar Listas

    [WebMethod]
    public static List<ListadosMantenimientoExpediente_ViewModel> CargarListadosMantenimiento(string dataCrypt)
    {
        var listadosMantenimiento = new List<ListadosMantenimientoExpediente_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            listadosMantenimiento = null;
        }
        return listadosMantenimiento;
    }

    // Obtener Catalogo de Documentos
    public static List<EntidadGenerica_ViewModel> ObtenerCatalogoDocumentos(string pcIDSesion, string pcIDApp, string pcIDUsuario)
    {
        var listaDocumentos = new List<EntidadGenerica_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_Catalogo_Documentos_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listaDocumentos.Add(new EntidadGenerica_ViewModel()
                            {
                                Id = (int)sqlResultado["fiIDDocumento"],
                                Descripcion = sqlResultado["fcDocumento"].ToString(),
                                DescripcionDetallada = sqlResultado["fcTipoDocumento"].ToString()
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            listaDocumentos = null;
        }
        return listaDocumentos;
    }

    // Obtener Catalogo de Tipos de Documentos
    public static List<EntidadGenerica_ViewModel> ObtenerCatalogoTiposDeDocumentos(string pcIDSesion, string pcIDApp, string pcIDUsuario)
    {
        var listaTiposDeDocumentos = new List<EntidadGenerica_ViewModel>();
        try
        {

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_Catalogo_TiposDeDocumentos_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listaTiposDeDocumentos.Add(new EntidadGenerica_ViewModel()
                            {
                                Id = (int)sqlResultado["fiIDTipoDocumento"],
                                Descripcion = sqlResultado["fcTipoDocumento"].ToString(),
                                DescripcionDetallada = sqlResultado["fcClassName"].ToString()
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            listaTiposDeDocumentos = null;
        }
        return listaTiposDeDocumentos;
    }

    // Obtener Catalogo Grupos De Archivos
    public static List<EntidadGenerica_ViewModel> ObtenerCatalogoGruposDeArchivos(string pcIDSesion, string pcIDApp, string pcIDUsuario)
    {
        var listaTiposDeDocumentos = new List<EntidadGenerica_ViewModel>();
        try
        {

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_GruposDeArchivos_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listaTiposDeDocumentos.Add(new EntidadGenerica_ViewModel()
                            {
                                Id = (int)sqlResultado["fiIDGrupoDeArchivo"],
                                Descripcion = sqlResultado["fcNombre"].ToString(),
                                DescripcionDetallada = sqlResultado["fcDescripcion"].ToString()
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            listaTiposDeDocumentos = null;
        }
        return listaTiposDeDocumentos;
    }

    #endregion

    #region Metodos Utilitarios

    public static InformacionUsuario_ViewModel ObtenerInformacionUsuarioPorIdUsuario(string pcIDApp, string pcIDUsuario, string pcIDSesion)
    {
        var usuarioLogueado = new InformacionUsuario_ViewModel();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreSeguridad.dbo.sp_InformacionUsuario", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            usuarioLogueado.NombreCorto = sqlResultado["fcNombreCorto"].ToString();
                            usuarioLogueado.CentroDeCosto = sqlResultado["fcCentroDeCosto"].ToString();
                            usuarioLogueado.NombreAgencia = sqlResultado["fcNombreAgencia"].ToString();
                            usuarioLogueado.BuzonDeCorreo = sqlResultado["fcBuzondeCorreo"].ToString();
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            usuarioLogueado = null;
        }
        return usuarioLogueado;
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

    #endregion

    #region View Models

    public class Resultado_ViewModel
    {
        public int IdInsertado { get; set; }
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string MensajeDebug { get; set; }
    }

    public class InformacionUsuario_ViewModel
    {
        public int IdUsuario { get; set; }
        public string NombreCorto { get; set; }
        public string CentroDeCosto { get; set; }
        public string NombreAgencia { get; set; }
        public string BuzonDeCorreo { get; set; }
    }

    public class ListadosMantenimientoExpediente_ViewModel
    {
        public List<EntidadGenerica_ViewModel> CatalogoDocumentos { get; set; }
        public List<EntidadGenerica_ViewModel> CatalogoTiposDeDocumentos { get; set; }
        public List<EntidadGenerica_ViewModel> GruposDeArchivos { get; set; }

        public ListadosMantenimientoExpediente_ViewModel()
        {
            CatalogoDocumentos = new List<EntidadGenerica_ViewModel>();
            CatalogoTiposDeDocumentos = new List<EntidadGenerica_ViewModel>();
            GruposDeArchivos = new List<EntidadGenerica_ViewModel>();
        }
    }

    public class EntidadGenerica_ViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionDetallada { get; set; }
    }

    public class TipoDeDocumento_ViewModel
    {
        public int IdTipoDeDocumento { get; set; }
        public string TipoDeDocumento { get; set; }
        public string ClassName { get; set; }
        public int Activo { get; set; }
        public string RazonInactivo { get; set; }
        public int IdUsuarioCreador { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioUltimaModificacion { get; set; }
        public string UsuarioUltimaModificacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
    }

    public class Documento_ViewModel
    {
        public int IdDocumento { get; set; }
        public string Descripcion { get; set; }
        public string DescripcionDetallada { get; set; }
        public int IdTipoDeDocumento { get; set; }
        public TipoDeDocumento_ViewModel TipoDeDocumento { get; set; }
        public int Activo { get; set; }
        public string RazonInactivo { get; set; }
        public int IdUsuarioCreador { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioUltimaModificacion { get; set; }
        public string UsuarioUltimaModificacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
    }

    public class GrupoDeArchivos_ViewModel
    {
        public int IdGrupoDeArchivos { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool IncluirInformacionClienteEnCorreo { get; set; }
        public bool IncluirInformacionSolicitudEnCorreo { get; set; }
        public bool IncluirInformacionPrestamoEnCorreo { get; set; }
        public bool InluirInformacionGarantiaEnPrestamo { get; set; }
        public int IdUsuarioCreador { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int IdUsuarioUltimaModificacion { get; set; }
        public string UsuarioUltimaModificacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
        public List<Documento_ViewModel> Documentos { get; set; }
    }

    #endregion
}