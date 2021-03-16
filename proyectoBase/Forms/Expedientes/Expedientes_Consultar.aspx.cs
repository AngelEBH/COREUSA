using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;

public partial class Expedientes_Consultar : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDExpediente = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    #region Page_Load, CargarInformacionDelExpediente, CargarGruposDeArchivos

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var type = Request.QueryString["type"];

            if (!IsPostBack && type == null)
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
                    pcIDExpediente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("idExpediente");

                    CargarInformacionDelExpediente();
                    CargarGruposDeArchivos();

                    HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
                    HttpContext.Current.Session["ListaDeDocumentosAGuardarPorTipoDocumento"] = null;
                    Session.Timeout = 10080;
                }
            }

            /* Guardar documentos de la solicitud */
            if (type != null || Request.HttpMethod == "POST")
            {
                Session["tipoDoc"] = Convert.ToInt32(Request.QueryString["IdTipoDocumento"]);
                var uploadDir = @"C:\inetpub\wwwroot\Documentos\Expedientes\Temp\";

                var fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
                    { "limit", 1 },
                    { "title", "auto" },
                    { "uploadDir", uploadDir },
                    { "extensions", new string[] { "jpg", "png", "jpeg"} },
                    { "maxSize", 500 }, /* Peso máximo de todos los archivos seleccionado en megas (MB) */
                    { "fileMaxSize", 20 }, /* Peso máximo por archivo */
                });

                switch (type)
                {
                    case "upload": /* Guardar achivo en carpeta temporal y guardar la informacion del mismo en una variable de sesion */

                        var data = fileUploader.Upload();

                        if (data["files"].Count == 1)
                            data["files"][0].Remove("file");
                        Response.Write(JsonConvert.SerializeObject(data));

                        /* Al subirse los archivos se guardan en este objeto de sesion general del helper fileuploader */
                        var list = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                        /* Guardar listado de documentos en una session propia de esta pantalla */
                        Session["ListaDeDocumentosAGuardarPorTipoDocumento"] = list;
                        break;

                    case "remove":
                        string file = Request.Form["file"];

                        if (file != null)
                        {
                            file = FileUploader.FullDirectory(uploadDir) + file;
                            if (File.Exists(file))
                                File.Delete(file);
                        }
                        break;
                }
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public void CargarInformacionDelExpediente()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_Maestro_ObtenerPorIdExpediente", sqlConexion))
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
                            lblEstadoExpediente.Text = sqlResultado["fcEstadoExpediente"].ToString();
                            lblEstadoExpediente.Attributes.Add("class", "btn btn-" + sqlResultado["fcEstadoExpedienteClassName"]);

                            if ((byte)sqlResultado["fiActivo"] == 0)
                                lblExpedienteInactivo.Visible = true;
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

    #endregion

    #region CargarDocumentosDelExpediente, CargarDocumentosDelExpedientePorIdDocumento, CargarDocumentosPorGrupoDeArchivos, ElimiarDocumentoExpediente, CambiarEstadoDocumentosPorIdDocumento

    [WebMethod]
    public static List<DocumentoDelExpediente_ViewModel> CargarDocumentosDelExpediente(string dataCrypt)
    {
        var documentosDelExpediente = new List<DocumentoDelExpediente_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDExpediente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("idExpediente");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_ObtenerDocumentosDelExpedientePorIdExpediente", sqlConexion))
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
                            documentosDelExpediente.Add(new DocumentoDelExpediente_ViewModel()
                            {
                                IdDocumento = (int)sqlResultado["fiIDDocumento"],
                                DescripcionNombreDocumento = sqlResultado["fcDocumento"].ToString(),
                                DescripcionDetalladaDelDocumento = sqlResultado["fcDescripcionDetallada"].ToString(),
                                Obligatorio = (bool)sqlResultado["fbObligatorio"],
                                CantidadMinima = (int)sqlResultado["fiCantidadMinima"],
                                CantidadMaxima = (int)sqlResultado["fiCantidadMaxima"],
                                CantidadGuardados = (int)sqlResultado["fiDocumentosGuardados"],
                                NoAdjuntado = (bool)sqlResultado["fbNoAdjuntado"],
                                NoAplica = (bool)sqlResultado["fbNoAplica"]
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            documentosDelExpediente = null;
        }
        return documentosDelExpediente;
    }

    [WebMethod]
    public static List<Expediente_Documento_ViewModel> CargarDocumentosDelExpedientePorIdDocumento(int idTipoDocumento, string dataCrypt)
    {
        var listaDocumentos = new List<Expediente_Documento_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDExpediente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("idExpediente");

            if (idTipoDocumento == 0)
                return listaDocumentos;

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_Documentos_ObtenerPorTipoDeDocumento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDExpediente", pcIDExpediente);
                    sqlComando.Parameters.AddWithValue("@piIDTipoDeDocumento", idTipoDocumento);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listaDocumentos.Add(new Expediente_Documento_ViewModel()
                            {
                                IdExpedienteDocumento = (int)sqlResultado["fiIDExpedienteDocumento"],
                                IdExpediente = (int)sqlResultado["fiIDExpediente"],
                                IdDocumento = (int)sqlResultado["fiIDDocumento"],
                                DescripcionNombreDocumento = sqlResultado["fcDocumento"].ToString(),
                                DescripcionDetalladaDelDocumento = sqlResultado["fcDescripcionDetallada"].ToString(),
                                IdEstadoDocumento = (int)sqlResultado["fiIDEstadoDocumento"],
                                NombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                //NombreArchivo = "CTES1177D1-20211212T041219-0",
                                Extension = sqlResultado["fcExtension"].ToString(),
                                Ruta = sqlResultado["fcRutaArchivo"].ToString(),
                                URL = sqlResultado["fcURL"].ToString(),
                                //URL = "/Documentos/Solicitudes/Solicitud1177/CTES1177D1-20211212T041219-0.png",
                                Comentarios = sqlResultado["fcComentario"].ToString(),
                                IdUsuarioCreador = (int)sqlResultado["fiIDUsuarioCreador"],
                                UsuarioCreador = sqlResultado["fcUsuarioCreador"].ToString(),
                                FechaCreacion = (DateTime)sqlResultado["fdFechaCreado"]
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

    [WebMethod]
    public static List<GrupoDeArchivos_Documento_ViewModel> CargarDocumentosPorGrupoDeArchivos(int idGrupoDeArchivos, string dataCrypt)
    {
        var documentos = new List<GrupoDeArchivos_Documento_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDExpediente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("idExpediente");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_Documentos_ObtenerPorGrupoDeArchivo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDExpediente", pcIDExpediente);
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

    [WebMethod]
    public static bool ElimiarDocumentoExpediente(int idDocumentoExpediente, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_Documentos_EliminarDocumento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDExpedienteDocumento", idDocumentoExpediente);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (!sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                            {
                                resultado = true;
                            }
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    [WebMethod]
    public static Resultado_ViewModel CambiarEstadoDocumentosPorIdDocumento(int idTipoDeDocumento, int idEstadoDocumento, string dataCrypt)
    {
        var resultado = new Resultado_ViewModel();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDExpediente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("idExpediente");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_Documentos_CambiarEstadoDocumentosPorTipoDeDocumento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDExpediente", pcIDExpediente);
                    sqlComando.Parameters.AddWithValue("@piIDDocumento", idTipoDeDocumento);
                    sqlComando.Parameters.AddWithValue("@piIDEstadoDocumento", idEstadoDocumento);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultado.ResultadoExitoso = sqlResultado["MensajeError"].ToString().StartsWith("-1") ? false : true;
                            resultado.MensajeResultado = sqlResultado["MensajeResultado"].ToString();
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado.ResultadoExitoso = false;
            resultado.MensajeResultado = "Ocurrió un error al cambiar el estado del documento, contacte al administrador.";
        }
        return resultado;
    }

    #endregion

    #region Guardar Documentos

    [WebMethod]
    public static bool ReiniciarListaDeDocumentosAGuardarPorTipoDocumento(bool reiniciarListaDeDocumentosAGuardarPorTipoDocumento)
    {
        try
        {
            if (reiniciarListaDeDocumentosAGuardarPorTipoDocumento)
            {
                HttpContext.Current.Session["ListaDeDocumentosAGuardarPorTipoDocumento"] = null;
                HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
            }

            return true;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            return false;
        }
    }

    [WebMethod]
    public static Resultado_ViewModel GuardarDocumentos(string dataCrypt)
    {
        var resultadoProceso = new Resultado_ViewModel() { ResultadoExitoso = false };
        try
        {
            var urlDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr");
            var pcIDExpediente = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("idExpediente");
            var documentosAGuardarEnBBDD = new List<SolicitudesDocumentosViewModel>();

            /* Validar si se adjuntaron Documentos. Si se adjuntaron, guardarlos en la base de datos y moverlos al nuevo directorio respectivo */
            if (HttpContext.Current.Session["ListaDeDocumentosAGuardarPorTipoDocumento"] != null)
            {
                var documentosAdjuntados = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaDeDocumentosAGuardarPorTipoDocumento"];

                if (documentosAdjuntados != null)
                {
                    var nombreCarpetaDestino = "Expediente" + pcIDExpediente;
                    var nuevoNombreDocumento = string.Empty;

                    documentosAdjuntados.ForEach(item =>
                    {
                        /* si el archivo existe, que se agregue a la lista de los documentos que se van a guardar */
                        if (File.Exists(item.fcRutaArchivo + "\\" + item.NombreAntiguo))
                        {
                            nuevoNombreDocumento = GenerarNombre(pcIDExpediente, item.fiTipoDocumento.ToString());

                            documentosAGuardarEnBBDD.Add(new SolicitudesDocumentosViewModel()
                            {
                                fcNombreArchivo = nuevoNombreDocumento,
                                NombreAntiguo = item.NombreAntiguo,
                                fcTipoArchivo = item.fcTipoArchivo,
                                fcRutaArchivo = item.fcRutaArchivo.Replace("Temp", "") + nombreCarpetaDestino,
                                URLArchivo = "/Documentos/Solicitudes/" + nombreCarpetaDestino + "/" + nuevoNombreDocumento + ".png",
                                fiTipoDocumento = item.fiTipoDocumento
                            });
                        }
                    }); // foreach documentos adjunstados
                } // if documentosAdjuntados != null
            } // if HttpContext.Current.Session["ListaDeDocumentosAGuardarPorTipoDocumento"] != null


            if (documentosAGuardarEnBBDD.Count > 0)
            {
                /* Si se adjuntaron documentos para asegurar, guardarlos en la base de datos */
                using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    sqlConexion.Open();

                    using (SqlTransaction sqlTransaction = sqlConexion.BeginTransaction())
                    {
                        foreach (var item in documentosAGuardarEnBBDD)
                        {
                            using (var sqlComando = new SqlCommand(guardarDocumentoSP, sqlConexion, sqlTransaction))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                sqlComando.CommandTimeout = 120;

                                using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                                {
                                    while (sqlResultado.Read())
                                    {
                                        if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                        {
                                            resultadoProceso.ResultadoExitoso = false;
                                            resultadoProceso.MensajeResultado = "Ocurrió un error al registrar el documento" + item.fcNombreArchivo + "., contacte al administrador.";
                                            resultadoProceso.MensajeDebug = sqlResultado["MensajeError"].ToString();
                                            sqlTransaction.Rollback();
                                            return resultadoProceso;
                                        }
                                    }
                                } // using sqlResultado
                            } // using sqlComando
                        } // ForEach documentos que se van a guardar en BBDD


                        /* Mover al directorio de la solicitud los documentos para asegurar adjuntados por el usuario que se guardaron en la base de datos */
                        if (!MoverDocumentos(pcIDExpediente, documentosAGuardarEnBBDD))
                        {
                            resultadoProceso.ResultadoExitoso = false;
                            resultadoProceso.MensajeResultado = "Ocurrió un error al guadar los documentos para asegurar, contacte al administrador.";
                            resultadoProceso.MensajeDebug = "Error al mover los documentos al nuevo directorio";
                            sqlTransaction.Rollback();

                            return resultadoProceso;
                        }

                        sqlTransaction.Commit();
                        HttpContext.Current.Session["ListaDeDocumentosAGuardarPorTipoDocumento"] = null;
                    } // using sqlTransaction
                } // using sqlConexion
            }
        }
        catch (Exception ex)
        {
            resultadoProceso.ResultadoExitoso = false;
            resultadoProceso.MensajeResultado = "Ocurrió un error al enviar la información, contacte al administrador.";
            resultadoProceso.MensajeDebug = ex.Message.ToString();
        }
        return resultadoProceso;
    }

    #endregion

    #region Metodos Utilitarios

    public static bool MoverDocumentos(string idLlavePrimaria, List<SolicitudesDocumentosViewModel> listaDocumentos)
    {
        bool resultado;
        try
        {
            if (listaDocumentos != null)
            {
                /* Definir el nuevo directorio al que se van a mover los documentos */
                var directorioTemporal = @"C:\inetpub\wwwroot\Documentos\Expedientes\Temp\";
                var nombreCarpetaDocumentos = "Expediente" + idLlavePrimaria;
                var directorioDestino = @"C:\inetpub\wwwroot\Documentos\Expedientes\" + nombreCarpetaDocumentos + "\\";

                if (!Directory.Exists(directorioDestino))
                    Directory.CreateDirectory(directorioDestino);

                foreach (SolicitudesDocumentosViewModel documento in listaDocumentos)
                {
                    string viejoDirectorio = directorioTemporal + documento.NombreAntiguo;
                    string nuevoNombreDocumento = documento.fcNombreArchivo;
                    string nuevoDirectorio = directorioDestino + nuevoNombreDocumento + ".png";

                    if (File.Exists(viejoDirectorio))
                        File.Move(viejoDirectorio, nuevoDirectorio);
                }
            }
            HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
            resultado = true;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    private static string GenerarNombre(string idLlavePrimaria, string idTipoDocumento)
    {
        return ("EXP" + idLlavePrimaria + "D" + idTipoDocumento + "_" + Guid.NewGuid()).Replace("*", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");
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

    public class GrupoDeArchivos_Documento_ViewModel : Expediente_Documento_ViewModel
    {
        public int IdGrupoDeArchivosDocumento { get; set; } // Llave primaria relacion GruposDeArchivos - Documentos
        public int IdGrupoDeArchivos { get; set; } // Llave foranea Grupo de archivos
    }

    public class Expediente_Documento_ViewModel : Documento
    {
        public int IdExpedienteDocumento { get; set; } // Llave primaria relación Expedientes - Catalogo Documentos
        public int IdExpediente { get; set; } // Llave foranea Expedientes Maestro
        public int IdDocumento { get; set; } // Llave foranea Catalogo de documentos
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

    public class DocumentoDelExpediente_ViewModel
    {
        public int IdDocumento { get; set; }
        public string DescripcionNombreDocumento { get; set; }
        public string DescripcionDetalladaDelDocumento { get; set; }
        public bool Obligatorio { get; set; }
        public int CantidadMinima { get; set; }
        public int CantidadMaxima { get; set; }
        public int CantidadGuardados { get; set; }

        public bool NoAdjuntado { get; set; }
        public bool NoAplica { get; set; }
    }

    #endregion
}