using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class PreSolicitud_Guardar : System.Web.UI.Page
{
    public string pcID;
    public string pcIDApp;
    public string pcIDSesion;
    public string pcIDUsuario;
    public string pcIDProducto;
    public string pcIDTipoDeSolicitud;
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    public List<PreSolicitud_Guardar_DocumentosRequeridos_ViewModel> ListaDocumentosRequeridos;

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        try
        {
            if (!IsPostBack && type == null)
            {
                /* Captura de parametros y desencriptado de cadena */
                var lcURL = Request.Url.ToString();
                int liParamStart = lcURL.IndexOf("?");

                pcID = "";
                ListaDocumentosRequeridos = new List<PreSolicitud_Guardar_DocumentosRequeridos_ViewModel>();

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

                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

                    HttpContext.Current.Session["ListaDocumentosRequeridosPreSolicitud"] = null;
                    HttpContext.Current.Session["ListaDocumentosAdjuntadosPreSolicitud"] = null;
                    HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
                    Session.Timeout = 10080;

                    if (!string.IsNullOrWhiteSpace(pcID))
                    {
                        CargarPrecalificado(pcID);

                        HttpContext.Current.Session["idTipoDeSolicitud"] = pcIDTipoDeSolicitud;
                    }

                    LlenarListas();
                }
            }

            /* Guardar documentos de la solicitud */
            if (type != null || Request.HttpMethod == "POST")
            {
                Session["tipoDoc"] = Convert.ToInt32(Request.QueryString["doc"]);
                var uploadDir = @"C:\inetpub\wwwroot\Documentos\PreSolicitudes\Temp\";

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
                    case "upload":
                        /* Proceso de carga */
                        var data = fileUploader.Upload();

                        /* Resultado */
                        if (data["files"].Count == 1)
                            data["files"][0].Remove("file");
                        Response.Write(JsonConvert.SerializeObject(data));

                        /* Al subirse los archivos se guardan en este objeto de sesion general del helper fileuploader */
                        var list = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                        /* Guardar listado de documentos en una session propia de esta pantalla */
                        Session["ListaDocumentosAdjuntadosPreSolicitud"] = list;

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
            MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
        }
    }

    protected void LlenarListas()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_GeoDepartamento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", 0);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlMunicipio.Items.Clear();
                        ddlMunicipio.Items.Add(new ListItem("Seleccione un departamento", ""));
                        ddlMunicipio.Enabled = false;

                        ddlCiudadPoblado.Items.Clear();
                        ddlCiudadPoblado.Items.Add(new ListItem("Seleccione un municipio", ""));
                        ddlCiudadPoblado.Enabled = false;

                        ddlBarrioColonia.Items.Clear();
                        ddlBarrioColonia.Items.Add(new ListItem("Seleccione una ciudad", ""));
                        ddlBarrioColonia.Enabled = false;

                        ddlDepartamento.Items.Clear();
                        ddlDepartamento.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlDepartamento.Items.Add(new ListItem(sqlResultado["fcDepartamento"].ToString(), sqlResultado["fiCodDepartamento"].ToString()));
                        }
                    }
                }

                using (var sqlComando = new SqlCommand("sp_CREDPreSolicitudes_Guardar_DocumentosRequeridos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            ListaDocumentosRequeridos.Add(new PreSolicitud_Guardar_DocumentosRequeridos_ViewModel()
                            {
                                IdTipoDocumento = int.Parse(sqlResultado["fiIDTipoDocumento"].ToString()),
                                DescripcionTipoDocumento = (string)sqlResultado["fcDescripcionTipoDocumento"],
                                CantidadMaxima = int.Parse(sqlResultado["fiCantidadDocumentos"].ToString())
                            });
                        }

                        Session["ListaDocumentosRequeridosPreSolicitud"] = ListaDocumentosRequeridos;
                    }
                }

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitudes_ListadoGestores", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlGestores.Items.Clear();
                        ddlGestores.Items.Add(new ListItem("Seleccionar", ""));

                        if (sqlResultado.HasRows)
                        {
                            while (sqlResultado.Read())
                            {
                                ddlGestores.Items.Add(new ListItem(sqlResultado["fcNombreCorto"].ToString(), sqlResultado["fiIDUsuario"].ToString()));
                            }
                        }
                    }
                }

                ddlTipoInvestigacionDeCampo.Items.Add(new ListItem("Seleccionar", ""));
                ddlTipoInvestigacionDeCampo.Items.Add(new ListItem("Domicilio", "1"));
                ddlTipoInvestigacionDeCampo.Items.Add(new ListItem("Trabajo", "2"));
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar información del formulario de presolicitud: " + ex.Message.ToString());
        }
    }

    private void CargarPrecalificado(string pcID)
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            string lcScript = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
                            Response.Write("<script>");
                            Response.Write(lcScript);
                            Response.Write("</script>");
                        }

                        while (sqlResultado.Read())
                        {
                            txtIdentidadCliente.Text = sqlResultado["fcIdentidad"].ToString();
                            txtNombreCliente.Text = sqlResultado["fcPrimerNombre"].ToString() + " " + sqlResultado["fcSegundoNombre"].ToString() + " " + sqlResultado["fcPrimerApellido"].ToString() + " " + sqlResultado["fcSegundoApellido"].ToString();
                            txtTelefonoCliente.Text = sqlResultado["fcTelefono"].ToString();
                            pcIDProducto = sqlResultado["fiIDProducto"].ToString();
                            pcIDTipoDeSolicitud = sqlResultado["fiTipoSolicitudCliente"].ToString();
                        }

                        if (txtNombreCliente.Text.Trim() == string.Empty)
                        {
                            MostrarMensaje("No se puede ingresar una pre solicitud de un cliente sin nombre, contacte al administrador. ");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar precalificado " + pcID + ": " + ex.Message.ToString());
        }
    }

    public static List<PreSolicitud_Guardar_Municipios_ViewModel> CargarMunicipios(int idDepartamento)
    {
        var municipios = new List<PreSolicitud_Guardar_Municipios_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoMunicipio", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", idDepartamento);
                    sqlComando.Parameters.AddWithValue("@piMunicipio", 0);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            municipios.Add(new PreSolicitud_Guardar_Municipios_ViewModel()
                            {
                                IdDepartamento = (short)sqlResultado["fiCodDepartamento"],
                                IdMunicipio = (short)sqlResultado["fiCodMunicipio"],
                                NombreMunicipio = sqlResultado["fcMunicipio"].ToString(),
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            municipios = null;
        }
        return municipios;
    }

    public static List<PreSolicitud_Guardar_Ciudades_ViewModel> CargarCiudadesPoblados(int idDepartamento, int idMunicipio)
    {
        var ciudades = new List<PreSolicitud_Guardar_Ciudades_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoPoblado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", idDepartamento);
                    sqlComando.Parameters.AddWithValue("@piMunicipio", idMunicipio);
                    sqlComando.Parameters.AddWithValue("@piPoblado", 0);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            ciudades.Add(new PreSolicitud_Guardar_Ciudades_ViewModel()
                            {
                                IdDepartamento = (short)sqlResultado["fiCodDepartamento"],
                                IdMunicipio = (short)sqlResultado["fiCodMunicipio"],
                                IdCiudadPoblado = (short)sqlResultado["fiCodPoblado"],
                                NombreCiudadPoblado = sqlResultado["fcPoblado"].ToString(),
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            ciudades = null;
        }
        return ciudades;
    }

    public static List<PreSolicitud_Guardar_BarriosColonias_ViewModel> CargarBarriosColonias(int idDepartamento, int idMunicipio, int idCiudadPoblado)
    {
        var BarriosColonias = new List<PreSolicitud_Guardar_BarriosColonias_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoBarrios", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", idDepartamento);
                    sqlComando.Parameters.AddWithValue("@piMunicipio", idMunicipio);
                    sqlComando.Parameters.AddWithValue("@piPoblado", idCiudadPoblado);
                    sqlComando.Parameters.AddWithValue("@piBarrio", 0);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            BarriosColonias.Add(new PreSolicitud_Guardar_BarriosColonias_ViewModel()
                            {
                                IdDepartamento = (short)sqlResultado["fiCodDepartamento"],
                                IdMunicipio = (short)sqlResultado["fiCodMunicipio"],
                                IdCiudadPoblado = (short)sqlResultado["fiCodPoblado"],
                                IdBarrioColonia = (short)sqlResultado["fiCodBarrio"],
                                NombreBarrioColonia = sqlResultado["fcBarrioColonia"].ToString()
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            BarriosColonias = null;
        }
        return BarriosColonias;
    }

    [WebMethod]
    public static List<PreSolicitud_Guardar_Municipios_ViewModel> CargarListaMunicipios(int idDepartamento)
    {
        return CargarMunicipios(idDepartamento);
    }

    [WebMethod]
    public static List<PreSolicitud_Guardar_Ciudades_ViewModel> CargarListaCiudadesPoblados(int idDepartamento, int idMunicipio)
    {
        return CargarCiudadesPoblados(idDepartamento, idMunicipio);
    }

    [WebMethod]
    public static List<PreSolicitud_Guardar_BarriosColonias_ViewModel> CargarListaBarriosColonias(int idDepartamento, int idMunicipio, int idCiudadPoblado)
    {
        return CargarBarriosColonias(idDepartamento, idMunicipio, idCiudadPoblado);
    }

    [WebMethod]
    public static List<PreSolicitud_Guardar_DocumentosRequeridos_ViewModel> CargarDocumentosRequeridos()
    {
        return (List<PreSolicitud_Guardar_DocumentosRequeridos_ViewModel>)HttpContext.Current.Session["ListaDocumentosRequeridosPreSolicitud"];
    }

    [WebMethod]
    public static Resultado_ViewModel GuardarPreSolicitud(PreSolicitud_Guardar_PreSolicitud_ViewModel preSolicitud, string dataCrypt)
    {
        var resultado = new Resultado_ViewModel() { ResultadoExitoso = false };

        using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["conexionEncriptada"].ToString())))
        {
            sqlConexion.Open();

            using (var tran = sqlConexion.BeginTransaction())
            {
                try
                {
                    var urlDesencriptado = DesencriptarURL(dataCrypt);
                    var pcID = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("ID") ?? "0";
                    var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp") ?? "0";
                    var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID") ?? "0";
                    var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr") ?? "0";

                    string idPreSolicitudGuardada = "0";

                    var idTipoDeSolicitud = (string)HttpContext.Current.Session["idTipoDeSolicitud"];

                    using (var sqlComando = new SqlCommand("sp_CREDPreSolicitudes_Maestro_Guardar", sqlConexion, tran))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDPais", 1);
                        sqlComando.Parameters.AddWithValue("@piIDCanal", 1);
                        sqlComando.Parameters.AddWithValue("@piIDTipoDeUbicacion", preSolicitud.IdTipoDeUbicacion);
                        sqlComando.Parameters.AddWithValue("@piIDTipoDeSolicitud", idTipoDeSolicitud);
                        sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                        sqlComando.Parameters.AddWithValue("@pcNombreTrabajo", preSolicitud.NombreTrabajo.Trim());
                        sqlComando.Parameters.AddWithValue("@pcTelefonoAdicional", preSolicitud.TelefonoAdicional.Trim());
                        sqlComando.Parameters.AddWithValue("@pcExtensionRecursosHumanos", preSolicitud.ExtensionRecursosHumanos.Replace("_", ""));
                        sqlComando.Parameters.AddWithValue("@pcExtensionCliente", preSolicitud.ExtensionCliente.Replace("_", ""));
                        sqlComando.Parameters.AddWithValue("@piIDDepartamento", preSolicitud.IdDepartamento);
                        sqlComando.Parameters.AddWithValue("@piIDMunicipio", preSolicitud.IdMunicipio);
                        sqlComando.Parameters.AddWithValue("@piIDCiudad", preSolicitud.IdCiudadPoblado);
                        sqlComando.Parameters.AddWithValue("@piIDBarrioColonia", preSolicitud.IdBarrioColonia);
                        sqlComando.Parameters.AddWithValue("@pcDireccionDetallada", preSolicitud.DireccionDetallada.Trim());
                        sqlComando.Parameters.AddWithValue("@pcReferenciasDireccionDetallada", preSolicitud.ReferenciasDireccionDetallada.Trim());
                        sqlComando.Parameters.AddWithValue("@piIDGestorValidador", preSolicitud.IdGestorValidador);
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
                                    idPreSolicitudGuardada = sqlResultado["MensajeError"].ToString();
                                }
                                else
                                {
                                    tran.Rollback();
                                    resultado.ResultadoExitoso = false;
                                    resultado.MensajeResultado = "No se pudo guardar la pre solicitud, contacte al administrador.";
                                    resultado.DebugString = "Error en CREDPreSolicitudes_Maestro_Guardar";
                                    return resultado;
                                }
                            }
                        }
                    }

                    /* Registrar documentacion de la pre solicitud */

                    /* Lista de documentos que se va ingresar en la base de datos y se va mover al nuevo directorio */
                    var documentosPreSolicitud = new List<SolicitudesDocumentosViewModel>();

                    if (HttpContext.Current.Session["ListaDocumentosAdjuntadosPreSolicitud"] != null)
                    {
                        /* lista de documentos adjuntados por el usuario */
                        var listaDocumentos = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaDocumentosAdjuntadosPreSolicitud"];

                        if (listaDocumentos != null)
                        {
                            string NombreCarpetaDocumentos = "PreSolicitud" + idPreSolicitudGuardada;
                            string NuevoNombreDocumento = "";

                            foreach (SolicitudesDocumentosViewModel file in listaDocumentos)
                            {
                                if (File.Exists(file.fcRutaArchivo + @"\" + file.NombreAntiguo)) /* si el archivo existe, que se agregue a la lista */
                                {
                                    NuevoNombreDocumento = GenerarNombreDocumento(idPreSolicitudGuardada.ToString(), file.fiTipoDocumento.ToString());

                                    documentosPreSolicitud.Add(new SolicitudesDocumentosViewModel()
                                    {
                                        fcNombreArchivo = NuevoNombreDocumento,
                                        NombreAntiguo = file.NombreAntiguo,
                                        fcTipoArchivo = file.fcTipoArchivo,
                                        fcRutaArchivo = file.fcRutaArchivo.Replace("Temp", "") + NombreCarpetaDocumentos,
                                        URLArchivo = "/Documentos/PreSolicitudes/" + NombreCarpetaDocumentos + "/" + NuevoNombreDocumento + ".png",
                                        fiTipoDocumento = file.fiTipoDocumento
                                    });
                                }
                            }
                        }
                    }

                    /* Guardar los documentos de la pre solicitud en la base de datos*/
                    int contadorErrores = 0;
                    foreach (SolicitudesDocumentosViewModel documento in documentosPreSolicitud)
                    {
                        using (var sqlComando = new SqlCommand("sp_CREDPreSolicitudes_Documentos_Guardar", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDPreSolicitud", idPreSolicitudGuardada);
                            sqlComando.Parameters.AddWithValue("@pcNombreArchivo", documento.fcNombreArchivo);
                            sqlComando.Parameters.AddWithValue("@pcTipoArchivo", ".png");
                            sqlComando.Parameters.AddWithValue("@pcRutaArchivo", documento.fcRutaArchivo);
                            sqlComando.Parameters.AddWithValue("@pcURL", documento.URLArchivo);
                            sqlComando.Parameters.AddWithValue("@piTipoDocumento", documento.fiTipoDocumento);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                    {
                                        contadorErrores++;
                                    }
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            resultado.ResultadoExitoso = false;
                            resultado.MensajeResultado = "No se pudo registrar la documentación de la pre solicitud, contacte al administrador.";
                            resultado.DebugString = "sp_CREDPreSolicitudes_Documentos_Guardar";
                            return resultado;
                        }
                    }

                    if (!GuardarDocumentos(documentosPreSolicitud, idPreSolicitudGuardada))
                    {
                        resultado.ResultadoExitoso = false;
                        resultado.MensajeResultado = "No se pudo guardar la documentación de la pre solicitud, contacte al administrador..";
                        resultado.DebugString = "GuardarDocumentos()";
                        return resultado;
                    }

                    tran.Commit();

                    resultado.ResultadoExitoso = true;
                    resultado.MensajeResultado = "La pre solicitud se registró exitosamente";
                    resultado.DebugString = "";

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resultado.ResultadoExitoso = false;
                    resultado.MensajeResultado = "No se pudo guardar la pre solicitud, contacte al administrador.";
                    resultado.DebugString = ex.Message.ToString();
                }
            }
        }
        return resultado;
    }

    private static string GenerarNombreDocumento(string idPreSolicitud, string piIDCatalogoDocumento)
    {
        string lcBloqueFechaHora = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");

        lcBloqueFechaHora = lcBloqueFechaHora.Replace("-", "");
        lcBloqueFechaHora = lcBloqueFechaHora.Replace(":", "");
        lcBloqueFechaHora = lcBloqueFechaHora.Replace(" ", "T");

        return ("CTEPS" + idPreSolicitud.Trim() + "D" + piIDCatalogoDocumento.ToString().Trim() + "-" + lcBloqueFechaHora);
    }

    public static bool GuardarDocumentos(List<SolicitudesDocumentosViewModel> listaDocumentos, string idPreSolicitud)
    {
        bool result;
        try
        {
            if (listaDocumentos != null)
            {
                /* Crear el nuevo directorio para los documentos de la garantia */
                var directorioTemporal = @"C:\inetpub\wwwroot\Documentos\PreSolicitudes\Temp\";
                var nombreCarpetaDocumentos = "PreSolicitud" + idPreSolicitud;
                var directorioDocumentos = @"C:\inetpub\wwwroot\Documentos\PreSolicitudes\" + nombreCarpetaDocumentos + "\\";
                bool carpetaExistente = Directory.Exists(directorioDocumentos);

                if (!carpetaExistente)
                    Directory.CreateDirectory(directorioDocumentos);

                listaDocumentos.ForEach(documento =>
                {
                    string ViejoDirectorio = directorioTemporal + documento.NombreAntiguo;
                    string NuevoNombreDocumento = documento.fcNombreArchivo;
                    string NuevoDirectorio = directorioDocumentos + NuevoNombreDocumento + ".png";

                    if (File.Exists(ViejoDirectorio))
                        File.Move(ViejoDirectorio, NuevoDirectorio);
                });
            }
            result = true;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            result = false;
        }
        return result;
    }

    public static Uri DesencriptarURL(string Url)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;
            var liParamStart = Url.IndexOf("?");

            if (liParamStart > 0)
                lcParametros = Url.Substring(liParamStart, Url.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = Url.Substring((liParamStart + 1), Url.Length - (liParamStart + 1));
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

    public void MostrarMensaje(string mensaje)
    {
        lblMensajeError.Visible = true;
        lblMensajeError.InnerText += mensaje;
    }

    #region View Models

    public class PreSolicitud_Guardar_BarriosColonias_ViewModel
    {
        public int IdCiudadPoblado { get; set; }
        public int IdMunicipio { get; set; }
        public int IdDepartamento { get; set; }
        public int IdBarrioColonia { get; set; }
        public string NombreBarrioColonia { get; set; }
    }

    public class PreSolicitud_Guardar_Ciudades_ViewModel
    {
        public int IdDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public int IdCiudadPoblado { get; set; }
        public string NombreCiudadPoblado { get; set; }
    }

    public class PreSolicitud_Guardar_Municipios_ViewModel
    {
        public int IdDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public string NombreMunicipio { get; set; }
    }

    public class PreSolicitud_Guardar_DocumentosRequeridos_ViewModel
    {
        public int IdTipoDocumento { get; set; }
        public string DescripcionTipoDocumento { get; set; }
        public int CantidadMaxima { get; set; }
    }

    public class PreSolicitud_Guardar_PreSolicitud_ViewModel
    {
        public int IdPreSolicitud { get; set; }
        public int IdPais { get; set; }
        public int IdCanal { get; set; }
        public string CentroDeCosto { get; set; }
        public int IdTipoDeUbicacion { get; set; }
        public int IdTipoDeSolicitud { get; set; }
        public string Identidad { get; set; }
        public string NombreTrabajo { get; set; }
        public string TelefonoAdicional { get; set; }
        public string ExtensionRecursosHumanos { get; set; }
        public string ExtensionCliente { get; set; }
        public int IdDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public int IdCiudadPoblado { get; set; }
        public int IdBarrioColonia { get; set; }
        public string DireccionDetallada { get; set; }
        public string ReferenciasDireccionDetallada { get; set; }
        public int IdGestorValidador { get; set; }
    }

    public class PreSolicitud_Guardar_Documentos_ViewModel
    {
        public string NombreAntiguo { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string RutaArchivo { get; set; }
        public string URL { get; set; }
        public int IdTipoDocumento { get; set; }
    }

    public class PreSolicitud_Guardar_Resultado_ViewModel
    {
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string DebugString { get; set; }
    }

    public class Resultado_ViewModel
    {
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string DebugString { get; set; }
    }
    #endregion
}