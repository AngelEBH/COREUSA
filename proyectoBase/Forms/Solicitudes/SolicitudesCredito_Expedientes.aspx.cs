using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Expedientes : System.Web.UI.Page
{
    public string pcID = "";
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDSolicitudCredito = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    public int pcIDExpediente { get; set; }

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

                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDSolicitudCredito = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

                    pcIDExpediente = 1;

                    CargarInformacionClienteSolicitud();
                    CargarGruposDeArchivos();
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

    }

    public void CargarInformacionClienteSolicitud()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_ObtenerPorIdExpediente", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDExpediente", pcIDExpediente);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            lblNoSolicitudCredito.InnerText = sqlResultado["fiIDSolicitud"].ToString();
                            lblProducto.Text = sqlResultado["fcProducto"].ToString();
                            lblTipoDeSolicitud.Text = sqlResultado["fcTipoSolicitud"].ToString();
                            lblAgenciaYVendedorAsignado.Text = sqlResultado["fcNombreAgencia"].ToString() + " / " + sqlResultado["fcNombreUsuarioAsignado"].ToString();
                            lblGestorAsignado.Text = sqlResultado["fcNombreGestor"].ToString();

                            lblNombreCliente.Text = sqlResultado["fcNombreCliente"].ToString();
                            lblIdentidadCliente.Text = sqlResultado["fcIdentidadCliente"].ToString();
                            txtRTNCliente.Text = sqlResultado["fcRTN"].ToString();
                            txtTelefonoCliente.Text = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                        }
                    } // using sqlComando
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public void CargarGruposDeArchivos()
    {
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
                        var grupoArchivosTemplate = new StringBuilder();

                        while (sqlResultado.Read())
                        {
                            grupoArchivosTemplate.Append("<button type='button' onclick='CargarDocumentosPorGrupoDeArchivos(" + sqlResultado["fiIDGrupoDeArchivo"] + ", " + '"' + sqlResultado["fcNombre"] + '"' + ", " + '"' + sqlResultado["fcDescripcion"] + '"' + ")' class='FormatoBotonesIconoCuadrado40' style='height: 115px; width: 100px; position: relative; margin-top: 5px; margin-left: 5px; background-image: url(/Imagenes/folder_40px.png);'>" +
                                sqlResultado["fcNombre"].ToString() +
                            "</button>");
                        }

                        divGruposDeArchivos.InnerHtml = grupoArchivosTemplate.ToString();
                    } // using sqlComando
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    #region Obtener documentos de la garantía y de la solicitud

    [WebMethod]
    public static List<GrupoDeArchivos_Documento_ViewModel> CargarDocumentosPorGrupoDeArchivos(int idExpediente, int idGrupoDeArchivos, string dataCrypt)
    {
        var documentos = new List<GrupoDeArchivos_Documento_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_Documentos_ObtenerPorGrupoDeArchivo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDExpediente", idExpediente);
                    sqlComando.Parameters.AddWithValue("@piIDGrupoDeArchivo", idGrupoDeArchivos);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            documentos.Add(new GrupoDeArchivos_Documento_ViewModel()
                            {
                                IdGrupoDeArchivosDocumento = (int)sqlResultado["fiIDGrupoDeArchivosDocumento"],
                                IdGrupoDeArchivos = (int)sqlResultado["fiIDGrupoDeArchivos"],
                                IdDocumento = (int)sqlResultado["fiIDDocumento"],
                                IdExpediente = (int)sqlResultado["fiIDExpediente"],
                                IdExpedienteDocumento = (int)sqlResultado["fiIDExpedienteDocumento"],

                                DescripcionNombreDocumento = sqlResultado["fcDocumento"].ToString(),
                                DescripcionDetalladaDelDocumento = sqlResultado["fcDescripcionDetallada"].ToString(),
                                NombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                Extension = sqlResultado["fcExtension"].ToString(),
                                Ruta = sqlResultado["fcRutaArchivo"].ToString(),
                                URL = sqlResultado["fcURL"].ToString(),
                                Comentarios = sqlResultado["fcComentario"].ToString(),
                                IdEstadoDocumento = (int)sqlResultado["fiIDEstadoDocumento"],
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            documentos = null;
        }
        return documentos;
    }

    #endregion

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

    #region View Models

    public class GrupoDeArchivos_Documento_ViewModel : Documento
    {
        public int IdGrupoDeArchivosDocumento { get; set; } // Llave primaria relacion GruposDeArchivos - Documentos
        public int IdGrupoDeArchivos { get; set; } // Llave foranea Grupo de archivos
        public int IdDocumento { get; set; } // Llave foranea Catalogo de documentos
        public int IdExpediente { get; set; } // Llave foranea Expedientes Maestro
        public int IdExpedienteDocumento { get; set; }  // Llave primaria relación Expedientes - Catalogo Documentos
    }

    public abstract class Documento
    {
        public string DescripcionNombreDocumento { get; set; }
        public string DescripcionDetalladaDelDocumento { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string Ruta { get; set; }
        public string URL { get; set; }
        public string Comentarios { get; set; }
        public int IdEstadoDocumento { get; set; }
        public int IdUsuarioCreador { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
    #endregion
}