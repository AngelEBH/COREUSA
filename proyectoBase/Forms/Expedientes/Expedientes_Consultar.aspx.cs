using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
    public string UrlCodigoQR { get; set; }

    #region Page_Load, CargarInformacionDelExpediente

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
                    UrlCodigoQR = "http://190.92.0.76/OPS/CFRM.aspx?" + DSC.Encriptar("E=" + pcIDExpediente);

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

                            /* Informacion del PDF del checklist */
                            lblNoSolicitudCredito_Expediente.InnerText = sqlResultado["fiIDSolicitud"].ToString();
                            lblNoSolicitud_Expediente.InnerText = sqlResultado["fiIDSolicitud"].ToString();
                            lblFechaActual_Expediente.InnerText = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                            lblNombreCliente_Expediente.InnerText = sqlResultado["fcNombreCliente"].ToString();
                            lblIdentidadCliente_Expediente.InnerText = sqlResultado["fcIdentidadCliente"].ToString();
                            lblDepartamento_Expediente.InnerText = sqlResultado["fcDepartamento"].ToString();
                            lblDireccionCliente_Expediente.InnerText = sqlResultado["fcDireccionCliente"].ToString();
                            lblTelefonoCliente_Expediente.InnerText = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                            lblTipoDeTrabajo_Expediente.InnerText = sqlResultado["fcNombreTrabajo"].ToString();
                            lblPuestoAsignado_Expediente.InnerText = sqlResultado["fcPuestoAsignado"].ToString();
                            lblTelefonoTrabajo_Expediente.InnerText = sqlResultado["fcTelefonoEmpresa"].ToString();
                            lblDirecciónTrabajo_Expediente.InnerText = sqlResultado["fcDireccionDetalladaEmpresa"].ToString();
                            lblOficialNegocios_Expediente.InnerText = sqlResultado["fcNombreUsuarioAsignado"].ToString();
                            lblGestor_Expediente.InnerText = sqlResultado["fcNombreGestor"].ToString();
                            lblRazonSocial.Text = sqlResultado["fcRazonSocial"].ToString().ToUpper();
                            lblNombreComercial.Text = sqlResultado["fcNombreComercial"].ToString().ToUpper();
                            var usuarioLogueado = ObtenerInformacionUsuarioPorIdUsuario(pcIDApp, pcIDUsuario, pcIDSesion);
                            lblNombreFirmaEntrega_Expediente.InnerText = usuarioLogueado.NombreCorto;
                            lblEspecifiqueOtros_Expediente.Text = sqlResultado["fcComentarios"].ToString();

                            lblFechaOtorgamiento_Expediente.InnerText = ((DateTime)sqlResultado["fdTiempoTomaDecisionFinal"]).ToString("dd/MM/yyyy");
                            lblCantidadCuotas_Expediente.InnerText = sqlResultado["fiPlazo"] + " Cuotas";
                            lblMontoOtorgado_Expediente.InnerText = DecimalToString(decimal.Parse(sqlResultado["fnValorTotalFinanciamiento"].ToString()));
                            lblValorCuota_Expediente.InnerText = DecimalToString(decimal.Parse(sqlResultado["fnCuotaTotal"].ToString()));
                            lblFechaPrimerPago_Expediente.InnerText = ((DateTime)sqlResultado["fdFechaPrimerCuota"]).ToString("dd/MM/yyyy");
                            lblFrecuenciaPlazo_Expediente.InnerText = sqlResultado["fcTipoPlazoSufijoAl"].ToString();
                            lblFechaVencimiento_Expediente.InnerText = "";

                            /* Información de la plantilla HTML del correo */
                            lblNombreCliente_Correo.Text = sqlResultado["fcNombreCliente"].ToString();
                            lblIdentidadCliente_Correo.Text = sqlResultado["fcIdentidadCliente"].ToString();
                            lblNoSolicitud_Correo.Text = sqlResultado["fiIDSolicitud"].ToString();
                            lblTipoDeSolicitud_Correo.Text = sqlResultado["fcTipoSolicitud"].ToString();
                            lblOficialNegocios_Correo.Text = sqlResultado["fcNombreUsuarioAsignado"].ToString();
                            lblCentroDeCosto_Correo.Text = sqlResultado["fcNombreAgencia"].ToString();
                            lblGestorAsignado_Correo.Text = sqlResultado["fcNombreGestor"].ToString();
                            lblMontoOtorgado_Correo.Text = DecimalToString(decimal.Parse(sqlResultado["fnValorTotalFinanciamiento"].ToString()));
                            lblMontoOtorgadoEnPalabras_Correo.Text = ConvertirCantidadALetras(sqlResultado["fnValorTotalFinanciamiento"].ToString());
                            lblPlazo_Correo.Text = sqlResultado["fiPlazo"].ToString() + " Cuotas";
                            lblFrecuencia_Correo.Text = sqlResultado["fcTipoPlazoSufijoAl"].ToString();
                            lblValorDeLaCuota_Correo.Text = DecimalToString(decimal.Parse(sqlResultado["fnCuotaTotal"].ToString()));
                            lblFechaDePrimerPago_Correo.Text = ((DateTime)sqlResultado["fdFechaPrimerCuota"]).ToString("dd/MM/yyyy");
                            lblMarca_Correo.Text = sqlResultado["fcMarca"].ToString();
                            lblModelo_Correo.Text = sqlResultado["fcModelo"].ToString();
                            lblAño_Correo.Text = sqlResultado["fiAnio"].ToString();
                            lblTipoVehiculo_Correo.Text = sqlResultado["fcTipoVehiculo"].ToString();
                            lblColor_Correo.Text = sqlResultado["fcColor"].ToString();
                            lblPlaca_Correo.Text = sqlResultado["fcMatricula"].ToString();
                            lblSerieMotor_Correo.Text = sqlResultado["fcMotor"].ToString();
                            lblSerieChasis_Correo.Text = sqlResultado["fcChasis"].ToString();
                            lblVIN_Correo.Text = sqlResultado["fcVIN"].ToString();
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

    #endregion

    #region Grupos de archivos

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
                            grupoArchivosTemplate.Append("<button type='button' " +
                                "onclick='CargarDocumentosPorGrupoDeArchivos(" + sqlResultado["fiIDGrupoDeArchivo"] + ", " + '"' + sqlResultado["fcNombre"] + '"' + ", " + '"' + sqlResultado["fcDescripcion"] + '"' + ", " + sqlResultado["fbIncluirInformacionClienteEnCorreo"].ToString().ToLower() + ", " + sqlResultado["fbIncluirInformacionSolicitudEnCorreo"].ToString().ToLower() + ", " + sqlResultado["fbIncluirInformacionPrestamoEnCorreo"].ToString().ToLower() + ", " + sqlResultado["fbIncluirInformacionGarantiaEnCorreo"].ToString().ToLower() + ")' " +
                                "class='FormatoBotonesIconoCuadrado40' style='height: 115px; width: 100px; position: relative; margin-top: 5px; margin-left: 5px; background-image: url(/Imagenes/folder_40px.png);'>" +
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

    [WebMethod]
    public static List<DocumentoDelExpediente_ViewModel> CargarDocumentosPorGrupoDeArchivos(int idGrupoDeArchivos, string dataCrypt)
    {
        var documentos = new List<DocumentoDelExpediente_ViewModel>();
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
                            documentos.Add(new DocumentoDelExpediente_ViewModel()
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
            documentos = null;
        }
        return documentos;
    }

    #endregion

    #region Administrar documentos del expediente

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

    /* Guardar documentos */
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
                                URLArchivo = "/Documentos/Expedientes/" + nombreCarpetaDestino + "/" + nuevoNombreDocumento + ".png",
                                fiTipoDocumento = item.fiTipoDocumento
                            });
                        }
                    }); // foreach documentos adjunstados
                } // if documentosAdjuntados != null
            } // if HttpContext.Current.Session["ListaDeDocumentosAGuardarPorTipoDocumento"] != null
            else
            {
                resultadoProceso.ResultadoExitoso = false;
                resultadoProceso.MensajeResultado = "No se ha adjuntado ningún documento. Asegúrate de que hayas adjuntado al menos un documento o vuelve a subirlos.";
                resultadoProceso.MensajeDebug = "lista de documentos null";
                return resultadoProceso;
            }

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
                            using (var sqlComando = new SqlCommand("sp_Expedientes_Documentos_Guardar", sqlConexion, sqlTransaction))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                sqlComando.Parameters.AddWithValue("@piIDExpediente", pcIDExpediente);
                                sqlComando.Parameters.AddWithValue("@piIDDocumento", item.fiTipoDocumento);
                                sqlComando.Parameters.AddWithValue("@piIDEstadoDocumento", 1);
                                sqlComando.Parameters.AddWithValue("@pcNombreArchivo", item.fcNombreArchivo);
                                sqlComando.Parameters.AddWithValue("@pcExtension", ".png");
                                sqlComando.Parameters.AddWithValue("@pcRutaArchivo", item.fcRutaArchivo);
                                sqlComando.Parameters.AddWithValue("@pcURL", item.URLArchivo);
                                sqlComando.Parameters.AddWithValue("@pcComentarios", "");
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

                        resultadoProceso.ResultadoExitoso = true;
                        resultadoProceso.MensajeResultado = "¡Los documentos se guardaron existosamente!";
                        resultadoProceso.MensajeDebug = "Todo ok";

                        HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
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

    #region CHECKLIST del expediente

    [WebMethod]
    public static CheckList_ViewModel ObtenerInformacionCheckListPorIdExpediente(string dataCrypt)
    {
        var checkList = new CheckList_ViewModel();
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

                using (var sqlComando = new SqlCommand("sp_Expedientes_ObtenerCheckListPorIdExpediente", sqlConexion))
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
                            checkList.Documentos.Add(new Expediente_Documento_ViewModel()
                            {
                                DescripcionNombreDocumento = sqlResultado["fcDocumento"].ToString(),
                                IdEstadoDocumento = (int)sqlResultado["fiIDEstadoDocumento"],
                            });
                        }

                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            checkList.TiposDeSolicitud.Add(new TipoDeSolicitud_ViewModel()
                            {
                                IdTipoDeSolicitud = (byte)sqlResultado["fiIDTipoSolicitud"],
                                TipoDeSolicitud = sqlResultado["fcTipoSolicitud"].ToString(),
                                Marcado = sqlResultado["fcMarcado"].ToString(),
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            checkList = null;
        }
        return checkList;
    }

    #endregion

    #region Enviar grupo de archivos por correo electrónico

    [WebMethod]
    public static Resultado_ViewModel EnviarGrupoDeArchivosPorCorreo(int idGrupoDeArchivos, string contenidoHTML, string dataCrypt)
    {
        var resultadoProceso = new Resultado_ViewModel() { ResultadoExitoso = false };
        try
        {
            var urlDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr");
            var pcIDExpediente = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("idExpediente");

            var grupoDeArchivos = string.Empty;
            var descripcionGrupoDeArchivos = string.Empty;
            var listaCC = new List<string>();
            var listaAdjuntados = new List<string>();
            var listaDestinatarios = new List<string>();

            var usuarioLogueado = ObtenerInformacionUsuarioPorIdUsuario(pcIDApp, pcIDUsuario, pcIDSesion);
            //listaDestinatarios.Add("sistemas@miprestadito.com");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                /* Obtener información del grupo de archivos */
                using (var sqlComando = new SqlCommand("sp_Expedientes_GruposDeArchivos_ObtenerPorId", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDGrupoDeArchivos", idGrupoDeArchivos);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            grupoDeArchivos = sqlResultado["fcNombre"].ToString();
                            descripcionGrupoDeArchivos = sqlResultado["fcDescripcion"].ToString();
                        }

                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            if (!(bool)sqlResultado["fbCC"])
                                listaDestinatarios.Add(sqlResultado["fcBuzonDeCorreo"].ToString());
                            else
                                listaCC.Add(sqlResultado["fcBuzonDeCorreo"].ToString());
                        }
                    }
                } // using sqlComando

                /* Obtener Lista de adjuntos */
                using (var sqlComando = new SqlCommand("sp_Expedientes_GruposDeArchivos_ObtenerDocumentosDelExpedientePorIdGrupoDeArchivos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDExpediente", pcIDExpediente);
                    sqlComando.Parameters.AddWithValue("@piIDGrupoDeArchivos", idGrupoDeArchivos);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listaAdjuntados.Add(sqlResultado["fcRutaArchivo"].ToString());
                        }
                    }
                } // using sqlComando
            } // using sqlConexion

            if (!EnviarCorreo(grupoDeArchivos, grupoDeArchivos, grupoDeArchivos, contenidoHTML, listaDestinatarios, listaCC, listaAdjuntados))
            {
                resultadoProceso.ResultadoExitoso = false;
                resultadoProceso.MensajeResultado = "Ocurrió un error al enviar la información por correo electrónico, contacta al administrador.";
                resultadoProceso.MensajeDebug = "Error al EnviarCorreo";
            }
            else
            {
                resultadoProceso.ResultadoExitoso = true;
                resultadoProceso.MensajeResultado = "¡La información se envió por correo electrónico exitosamente!";
                resultadoProceso.MensajeDebug = "Todo cheque";
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

    public static string DecimalToString(decimal valor)
    {
        return string.Format("{0:#,###0.00##}", valor);
    }

    public static bool EnviarCorreo(string pcAsunto, string pcTituloGeneral, string pcSubtitulo, string pcContenidodelMensaje, List<string> listaDestinatarios, List<string> listaCC, List<string> listaAdjuntos)
    {
        var resultado = false;
        try
        {
            using (var smtpCliente = new SmtpClient("mail.miprestadito.com", 587))
            {
                smtpCliente.Credentials = new System.Net.NetworkCredential("systembot@miprestadito.com", "iPwf@p3q");
                smtpCliente.EnableSsl = true;

                using (var pmmMensaje = new MailMessage())
                {
                    pmmMensaje.Subject = pcAsunto;
                    pmmMensaje.From = new MailAddress("systembot@miprestadito.com", "System Bot");
                    pmmMensaje.IsBodyHtml = true;
                    //pmmMensaje.To.Add("sistemas@miprestadito.com");

                    if (listaDestinatarios != null)
                    {
                        foreach (var item in listaDestinatarios)
                            pmmMensaje.To.Add(item);
                    }

                    if (listaCC != null)
                    {
                        foreach (var item in listaCC)
                            pmmMensaje.CC.Add(item);
                    }

                    if (listaAdjuntos != null)
                    {
                        foreach (var item in listaAdjuntos)
                            if (File.Exists(item))
                                pmmMensaje.Attachments.Add(new Attachment(item));
                    }

                    string htmlString = @"<!DOCTYPE html> " +
                    "<html>" +
                    "<body>" +
                    " <div style=\"width: 500px;\">" +
                    " <table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                    " <tr style=\"height: 30px; background-color:#56396b; font-family: 'Microsoft Tai Le'; font-size: 14px; font-weight: bold; color: white;\">" +
                    " <td colspan=\"2\" style=\"vertical-align: central; text-align:center;\">" + pcTituloGeneral + "</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td>&nbsp;</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td colspan=\"2\" style=\"background-color:whitesmoke; text-align:center;\">" + pcSubtitulo + "</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td>&nbsp;</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td style=\"vertical-align: central;\">" + pcContenidodelMensaje + "</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td>&nbsp;</td>" +
                    " </tr>" +
                    " <tr style=\"height: 20px; font-family: 'Microsoft Tai Le'; font-size: 12px; text-align:center;\">" +
                    " <td colspan=\"2\">System Bot Prestadito</td>" +
                    " </tr>" +
                    " </table>" +
                    " </div>" +
                    "</body> " +
                    "</html> ";

                    pmmMensaje.Body = htmlString;
                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    smtpCliente.Send(pmmMensaje);
                    resultado = true;
                    //smtpCliente.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    #endregion

    #region Convertir cantidad a palabras

    // si, otra vez..........
    public static string ConvertirCantidadALetras(string cantidad)
    {
        string resultado, centavos = string.Empty;
        Int64 entero;
        int decimales;
        double cantidadNumerica;

        try
        {
            cantidadNumerica = Convert.ToDouble(cantidad);
            entero = Convert.ToInt64(Math.Truncate(cantidadNumerica));
            decimales = Convert.ToInt32(Math.Round((cantidadNumerica - entero) * 100, 2));
            centavos = " CON " + PadNum(decimales) + "/100 CTVS";
            resultado = ToText(Convert.ToDouble(entero)) + centavos;
        }
        catch
        {
            return "";
        }

        return resultado;
    }

    private static string ToText(double value)
    {
        var Num2Text = string.Empty;
        value = Math.Truncate(value);

        if (value == 0) Num2Text = "CERO";
        else if (value == 1) Num2Text = "UNO";
        else if (value == 2) Num2Text = "DOS";
        else if (value == 3) Num2Text = "TRES";
        else if (value == 4) Num2Text = "CUATRO";
        else if (value == 5) Num2Text = "CINCO";
        else if (value == 6) Num2Text = "SEIS";
        else if (value == 7) Num2Text = "SIETE";
        else if (value == 8) Num2Text = "OCHO";
        else if (value == 9) Num2Text = "NUEVE";
        else if (value == 10) Num2Text = "DIEZ";
        else if (value == 11) Num2Text = "ONCE";
        else if (value == 12) Num2Text = "DOCE";
        else if (value == 13) Num2Text = "TRECE";
        else if (value == 14) Num2Text = "CATORCE";
        else if (value == 15) Num2Text = "QUINCE";
        else if (value < 20) Num2Text = "DIECI" + ToText(value - 10);
        else if (value == 20) Num2Text = "VEINTE";
        else if (value < 30) Num2Text = "VEINTI" + ToText(value - 20);
        else if (value == 30) Num2Text = "TREINTA";
        else if (value == 40) Num2Text = "CUARENTA";
        else if (value == 50) Num2Text = "CINCUENTA";
        else if (value == 60) Num2Text = "SESENTA";
        else if (value == 70) Num2Text = "SETENTA";
        else if (value == 80) Num2Text = "OCHENTA";
        else if (value == 90) Num2Text = "NOVENTA";
        else if (value < 100) Num2Text = ToText(Math.Truncate(value / 10) * 10) + " Y " + ToText(value % 10);
        else if (value == 100) Num2Text = "CIEN";
        else if (value < 200) Num2Text = "CIENTO " + ToText(value - 100);
        else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = ToText(Math.Truncate(value / 100)) + "CIENTOS";
        else if (value == 500) Num2Text = "QUINIENTOS";
        else if (value == 700) Num2Text = "SETECIENTOS";
        else if (value == 900) Num2Text = "NOVECIENTOS";
        else if (value < 1000) Num2Text = ToText(Math.Truncate(value / 100) * 100) + " " + ToText(value % 100);
        else if (value == 1000) Num2Text = "MIL";
        else if (value < 2000) Num2Text = "MIL " + ToText(value % 1000);
        else if (value < 1000000)
        {
            Num2Text = ToText(Math.Truncate(value / 1000)) + " MIL";
            if ((value % 1000) > 0) Num2Text = Num2Text + " " + ToText(value % 1000);
        }

        else if (value == 1000000) Num2Text = "UN MILLON";
        else if (value < 2000000) Num2Text = "UN MILLON " + ToText(value % 1000000);
        else if (value < 1000000000000)
        {
            Num2Text = ToText(Math.Truncate(value / 1000000)) + " MILLONES ";
            if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + ToText(value - Math.Truncate(value / 1000000) * 1000000);
        }

        else if (value == 1000000000000) Num2Text = "UN BILLON";
        else if (value < 2000000000000) Num2Text = "UN BILLON " + ToText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

        else
        {
            Num2Text = ToText(Math.Truncate(value / 1000000000000)) + " BILLONES";
            if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + ToText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
        }
        return Num2Text;

    }

    private static string PadNum(int entero)
    {
        return entero < 9 ? "0" + entero : entero.ToString();
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

    public class GrupoDeArchivos_Documento_ViewModel : Expediente_Documento_ViewModel
    {
        public int IdGrupoDeArchivosDocumento { get; set; } // Llave primaria relacion GruposDeArchivos - Documentos
        public int IdGrupoDeArchivos { get; set; } // Llave foranea Grupo de archivos
    }

    public class CheckList_ViewModel
    {
        public List<Expediente_Documento_ViewModel> Documentos { get; set; }
        public List<TipoDeSolicitud_ViewModel> TiposDeSolicitud { get; set; }

        public CheckList_ViewModel()
        {
            Documentos = new List<Expediente_Documento_ViewModel>();
            TiposDeSolicitud = new List<TipoDeSolicitud_ViewModel>();
        }
    }

    public class Expediente_Documento_ViewModel : Documento
    {
        public int IdExpedienteDocumento { get; set; } // Llave primaria relación Expedientes - Catalogo Documentos
        public int IdExpediente { get; set; } // Llave foranea Expedientes Maestro
        public int IdDocumento { get; set; } // Llave foranea Catalogo de documentos
    }

    public class TipoDeSolicitud_ViewModel
    {
        public int IdTipoDeSolicitud { get; set; }
        public string TipoDeSolicitud { get; set; }
        public string Marcado { get; set; }
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