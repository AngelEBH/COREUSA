using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;

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

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        try
        {
            if (!IsPostBack && type == null)
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

                if (lcParametros != string.Empty)
                {
                    var lcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDGarantia = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDGarantia");
                    pcIDSolicitudGPS = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSolicitudGPS") ?? "0";
                    pcIDSolicitudCredito = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

                    CargarInformacion();

                    HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
                    HttpContext.Current.Session["ListaFotografiasInstalacion"] = null;
                    Session.Timeout = 10080;
                }
            }

            /* Guardar fotografías de la instalacion de GPS */
            if (type != null || Request.HttpMethod == "POST")
            {
                Session["tipoDoc"] = Convert.ToInt32(Request.QueryString["idfotografia"]);
                var directorio = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";

                var fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
                    { "limit", 1 },
                    { "title", "auto" },
                    { "uploadDir", directorio },
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
                        Session["ListaFotografiasInstalacion"] = list;

                        break;

                    case "remove":
                        string file = Request.Form["file"];

                        if (file != null)
                        {
                            file = FileUploader.FullDirectory(directorio) + file;
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
            MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
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
                            txtUbicacion.InnerText = sqlResultado["fcDescripcionUbicacion"].ToString();
                            txtComentariosDeLaInstalacion.InnerText = sqlResultado["fcObservacionesInstalacion"].ToString();
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
    public static List<FotografiaInstalacion_ViewModel> CargarListaFotografiasRequeridas(string dataCrypt)
    {
        var listaFotografiasRequeridas = new List<FotografiaInstalacion_ViewModel>();

        try
        {
            var urlDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("AutoLoan.dbo.sp_AutoGPS_Instalacion_Catalogo_Fotografias_List", sqlConexion))
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
                            listaFotografiasRequeridas.Add(new FotografiaInstalacion_ViewModel()
                            {
                                IdFotografia = (int)sqlResultado["fiIDFotografia"],
                                DescripcionFotografia = sqlResultado["fcDescripcionFotografia"].ToString()
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            listaFotografiasRequeridas = null;
        }
        return listaFotografiasRequeridas;
    }

    [WebMethod]
    public static Resultado_ViewModel RegistrarInstalacionGPS(InstalacionGPS_ViewModel instalacionGPS, string dataCrypt)
    {
        var resultado = new Resultado_ViewModel() { ResultadoExitoso = false };
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["conexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var tran = sqlConexion.BeginTransaction())
                {
                    try
                    {
                        var urlDesencriptado = DesencriptarURL(dataCrypt);
                        var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
                        var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID");
                        var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr");
                        var pcIDGarantia = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDGarantia");
                        var pcIDSolicitudGPS = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDSolicitudGPS") ?? "0";
                        var pcIDSolicitudCredito = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDSOL");

                        using (var sqlComando = new SqlCommand("AutoLoan.dbo.sp_AutoGPS_Instalacion_RegistrarInstalacion", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", pcIDSolicitudGPS);
                            sqlComando.Parameters.AddWithValue("@pcDescripcionUbicacion", instalacionGPS.DescripcionUbicacion);
                            sqlComando.Parameters.AddWithValue("@pcObservacionesInstalacion", instalacionGPS.Comentarios);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.CommandTimeout = 120;

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    var resultadoSp = sqlResultado["MensajeError"].ToString();

                                    if (resultadoSp.StartsWith("-1"))
                                    {
                                        resultado.ResultadoExitoso = false;
                                        resultado.MensajeResultado = "No se pudo registrar la información de la instalación de GPS, contacte al administrador.";
                                        resultado.MensajeDebug = resultadoSp;
                                        return resultado;
                                    }
                                }
                            }
                        }

                        /* Lista de fotografias que se va ingresar en la base de datos y se va mover al nuevo directorio */
                        var fotografiasInstalacionGPS = new List<SolicitudesDocumentosViewModel>();

                        /* Registrar documentacion de la solicitud */
                        if (HttpContext.Current.Session["ListaFotografiasInstalacion"] != null)
                        {
                            /* lista de fotografias adjuntados por el usuario */
                            var listaDeFotografias = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaFotografiasInstalacion"];

                            if (listaDeFotografias.Count < 2)
                            {
                                resultado.ResultadoExitoso = false;
                                resultado.MensajeResultado = "La fotografía del vehículo y del GPS es requerida.";
                                resultado.MensajeDebug = "docs < 2";
                                return resultado;
                            }

                            if (listaDeFotografias != null)
                            {
                                var nombreCarpetaDocumentos = "Solicitud" + pcIDSolicitudCredito;
                                var nuevoNombreDocumento = string.Empty;

                                foreach (SolicitudesDocumentosViewModel fotografia in listaDeFotografias)
                                {
                                    if (File.Exists(fotografia.fcRutaArchivo + @"\" + fotografia.NombreAntiguo)) /* si el archivo existe, que se agregue a la lista */
                                    {
                                        nuevoNombreDocumento = GenerarNombreFotografia(pcIDSolicitudCredito, fotografia.fiTipoDocumento.ToString());

                                        fotografiasInstalacionGPS.Add(new SolicitudesDocumentosViewModel()
                                        {
                                            fcNombreArchivo = nuevoNombreDocumento,
                                            NombreAntiguo = fotografia.NombreAntiguo,
                                            fcTipoArchivo = fotografia.fcTipoArchivo,
                                            fcRutaArchivo = fotografia.fcRutaArchivo.Replace("Temp", "") + nombreCarpetaDocumentos,
                                            URLArchivo = "/Documentos/Solicitudes/" + nombreCarpetaDocumentos + "/" + nuevoNombreDocumento + ".png",
                                            fiTipoDocumento = fotografia.fiTipoDocumento
                                        });
                                    } // if File.Exists
                                } // foreach lista fotografias
                            } // if lista fotografias != null
                        } // if Session["ListaFotografiasInstalacion"] != null
                        else
                        {
                            resultado.ResultadoExitoso = false;
                            resultado.MensajeResultado = "Debes adjuntar minimo la fotografía del vehículo y del GPS es requerida.";
                            resultado.MensajeDebug = "docs null";
                            return resultado;
                        }

                        /* Guardar los fotografias de la instalacion de GPS en la base de datos*/
                        int contadorErrores = 0;
                        foreach (SolicitudesDocumentosViewModel fotografia in fotografiasInstalacionGPS)
                        {
                            using (var sqlComando = new SqlCommand("AutoLoan.dbo.sp_AutoGPS_Instalacion_Fotografias_Actualizar", sqlConexion, tran))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", pcIDSolicitudGPS);
                                sqlComando.Parameters.AddWithValue("@piIDFotografia", fotografia.fiTipoDocumento);
                                sqlComando.Parameters.AddWithValue("@pcNombreArchivo", fotografia.fcNombreArchivo);
                                sqlComando.Parameters.AddWithValue("@pcExtension", ".png");
                                sqlComando.Parameters.AddWithValue("@pcRutaArchivo", fotografia.fcRutaArchivo);
                                sqlComando.Parameters.AddWithValue("@pcURL", fotografia.URLArchivo);
                                sqlComando.Parameters.AddWithValue("@pcComentario", "");
                                sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                                using (var sqlResultado = sqlComando.ExecuteReader())
                                {
                                    while (sqlResultado.Read())
                                    {
                                        if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                            contadorErrores++;
                                    }
                                }
                            }

                            if (contadorErrores > 0)
                            {
                                resultado.ResultadoExitoso = false;
                                resultado.MensajeResultado = "Ocurrió un error al guardar las fotografías de la instalacion de GPS, contacte al administrador.";
                                resultado.MensajeDebug = "Garantias_Documentos_Insert";
                                return resultado;
                            }
                        }

                        if (!GuardarDocumentosGarantia(fotografiasInstalacionGPS, pcIDSolicitudCredito))
                        {
                            resultado.ResultadoExitoso = false;
                            resultado.MensajeResultado = "Ocurrió un error al guardar las fotografías de la instalación de GPS, contacte al administrador.";
                            resultado.MensajeDebug = "GuardarDocumentosGarantia()";
                            return resultado;
                        }

                        var resultadoMoverDocumentos = MoverDocumentosGuardados(fotografiasInstalacionGPS, pcIDApp, pcIDUsuario, pcIDSolicitudCredito);

                        if (resultadoMoverDocumentos.Respuesta != "1")
                        {
                            resultado.ResultadoExitoso = false;
                            resultado.MensajeResultado = "Ocurrió un error al actualizar la documentación de la instalación del GPS, contacte al administrador..";
                            resultado.MensajeDebug = "MoverDocumentosGuardados() | " + resultadoMoverDocumentos.Mensaje;
                            return resultado;
                        }

                        tran.Commit();

                        resultado.ResultadoExitoso = true;
                        resultado.MensajeResultado = "La información de la instalación del GPS se registró correctamente";
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        resultado.ResultadoExitoso = false;
                        resultado.MensajeResultado = "No se pudo guardar la instalación de GPS, contacte al administrador.";
                        resultado.MensajeDebug = ex.Message.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultado;
    }

    #region Funciones utilitarias

    private static string GenerarNombreFotografia(string idSolicitud, string idFotografia)
    {
        var lcBloqueFechaHora = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss").Replace("-", "").Replace(":", "").Replace(" ", "T");

        return ("GPS" + idSolicitud + "D" + idFotografia + "-" + lcBloqueFechaHora).Replace("*", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");
    }

    public static bool GuardarDocumentosGarantia(List<SolicitudesDocumentosViewModel> listaFotografias, string idSolicitud)
    {
        bool resultado;
        try
        {
            if (listaFotografias != null)
            {
                /* Crear el nuevo directorio para los documentos de la garantia */
                var directorioTemporal = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";
                var nombreCarpetaDocumentos = "Solicitud" + idSolicitud;
                var directorioDocumentos = @"C:\inetpub\wwwroot\Documentos\Solicitudes\" + nombreCarpetaDocumentos + "\\";
                bool carpetaExistente = Directory.Exists(directorioDocumentos);

                if (!carpetaExistente)
                    Directory.CreateDirectory(directorioDocumentos);

                listaFotografias.ForEach(documento =>
                {
                    var viejoDirectorio = directorioTemporal + documento.NombreAntiguo;
                    var nuevoNombreDocumento = documento.fcNombreArchivo;
                    var nuevoDirectorio = directorioDocumentos + nuevoNombreDocumento + ".png";

                    if (File.Exists(viejoDirectorio))
                        File.Move(viejoDirectorio, nuevoDirectorio);
                });
            }
            resultado = true;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    public static RespuestaWebService MoverDocumentosGuardados(List<SolicitudesDocumentosViewModel> listaFotografias, string idApp, string idUsuario, string idSolicitud)
    {
        string url;
        string json;
        var resultadoWS = new RespuestaWebService();

        try
        {
            using (var client = new WebClient())
            {
                foreach (SolicitudesDocumentosViewModel archivo in listaFotografias)
                {
                    url = "http://172.20.3.140/WS/WSCoreMovilGarantias.aspx?IDApp=" + idApp + "&usr=" + idUsuario + "&file=" + archivo.fcNombreArchivo + ".png" + "&carpeta=Solicitud" + idSolicitud;

                    json = client.DownloadString(new Uri(url));

                    resultadoWS = new JavaScriptSerializer().Deserialize<RespuestaWebService>(json);

                    if (resultadoWS.Respuesta != "1")
                    {
                        resultadoWS.Mensaje += " | URL >>>> " + url;
                        return resultadoWS;
                    }
                }
            }

            Directory.Delete(@"C:\inetpub\wwwroot\Documentos\Solicitudes\Solicitud" + idSolicitud + "\\", true);
        }
        catch (Exception ex)
        {
            resultadoWS.Mensaje += " | Excepción >>>>> " + ex.Message.ToString();
        }

        return resultadoWS;
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

    #endregion

    #region View models

    public class Resultado_ViewModel
    {
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string MensajeDebug { get; set; }
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

    public class FotografiaInstalacion_ViewModel
    {
        public int IdFotografia { get; set; }
        public string DescripcionFotografia { get; set; }
    }

    public class RespuestaWebService
    {
        public string Respuesta { get; set; }
        public string Mensaje { get; set; }
    }

    #endregion
}