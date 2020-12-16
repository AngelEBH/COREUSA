using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;

public partial class Garantia_Actualizar : System.Web.UI.Page
{
    private string pcIDApp = "";
    private string pcIDSesion = "";
    public string pcIDUsuario = "";
    private string pcIDGarantia = "";
    private string pcIDSolicitud = "";
    public bool EsDigitadoManualmente;
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    public List<SeccionGarantia_ViewModel> Documentos_Secciones_Garantia;

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        /* Captura de parámetros encriptados */
        if (!IsPostBack && type == null)
        {
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");

            Documentos_Secciones_Garantia = new List<SeccionGarantia_ViewModel>();

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
                var pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                pcIDGarantia = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDGarantia");
                pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
                lblNoSolicitud.InnerText = pcIDSolicitud != "0" ? ("Solicitud de crédito No. " + pcIDSolicitud) : "Sin solicitud de crédito";

                HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
                HttpContext.Current.Session["ListaDocumentosGarantia_Actualizar"] = null;
                Session.Timeout = 10080;
            }
            LlenarListas();
        }

        /* Guardar documentos de la solicitud */
        if (type != null || Request.HttpMethod == "POST")
        {
            Session["tipoDoc"] = Convert.ToInt32(Request.QueryString["doc"]);
            var uploadDir = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";

            var fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
{ "limit", 1 },
{ "title", "auto" },
{ "uploadDir", uploadDir },
{ "extensions", new string[] { "jpg", "png", "jpeg"} },
{ "maxSize", 500 }, //peso máximo de todos los archivos seleccionado en megas (MB)
{ "fileMaxSize", 20 }, //peso máximo por archivo
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
                    Session["ListaDocumentosGarantia_Actualizar"] = list;

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

    public void LlenarListas()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_Actualizar_LlenarListas", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDGarantia", pcIDGarantia);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /* Primer resultado: Secciones de garantia para la documentación */
                        while (sqlResultado.Read())
                        {
                            Documentos_Secciones_Garantia.Add(new SeccionGarantia_ViewModel()
                            {
                                IdSeccionGarantia = int.Parse(sqlResultado["fiIDSeccionGarantia"].ToString()),
                                DescripcionSeccion = (string)sqlResultado["fcSeccionGarantia"]
                            });
                        }
                        Session["Documentos_Secciones_Garantia_Actualizar"] = Documentos_Secciones_Garantia;

                        /* Segundo resultado: Tipo de garantía */
                        sqlResultado.NextResult();

                        ddlTipoDeGarantia.Items.Clear();

                        while (sqlResultado.Read())
                        {
                            ddlTipoDeGarantia.Items.Add(new ListItem(sqlResultado["fcTipoDeGarantia"].ToString(), sqlResultado["fcTipoDeGarantia"].ToString()));
                        }

                        if (pcIDSolicitud == "0")
                        {
                            ddlTipoDeGarantia.Enabled = true;
                        }

                        /* Tercer resultado: Informacion de la garantía */
                        sqlResultado.NextResult();

                        var idEstadoCivilPropietario = "0";
                        var idNacionalidadPropietario = "0";

                        var idEstadoCivilVendedor = "0";
                        var idNacionalidadVendedor = "0";

                        while (sqlResultado.Read())
                        {
                            txtVIN.Text = sqlResultado["fcVIN"].ToString();
                            ddlTipoDeGarantia.SelectedValue = sqlResultado["fcTipoGarantia"].ToString();
                            txtTipoDeVehiculo.Text = sqlResultado["fcTipoVehiculo"].ToString();
                            txtMarca.Text = sqlResultado["fcMarca"].ToString();
                            txtModelo.Text = sqlResultado["fcModelo"].ToString();
                            txtAnio.Text = sqlResultado["fiAnio"].ToString();
                            txtColor.Text = sqlResultado["fcColor"].ToString();
                            txtMatricula.Text = sqlResultado["fcMatricula"].ToString();
                            txtCilindraje.Text = sqlResultado["fcCilindraje"].ToString();
                            txtRecorrido.Text = sqlResultado["fnRecorrido"].ToString();
                            ddlUnidadDeMedida.SelectedValue = sqlResultado["fcUnidadDeDistancia"].ToString();
                            txtTransmision.Text = sqlResultado["fcTransmision"].ToString();
                            txtTipoDeCombustible.Text = sqlResultado["fcTipoCombustible"].ToString();
                            txtPrecioMercado.Text = sqlResultado["fnValorGarantia"].ToString();
                            txtValorPrima.Text = sqlResultado["fnValorPrima"].ToString();
                            txtValorFinanciado.Text = sqlResultado["fnValorFinanciado"].ToString();
                            txtGastosDeCierre.Text = sqlResultado["fnGastosDeCierre"].ToString();
                            txtSerieUno.Text = sqlResultado["fcSerieUno"].ToString();
                            txtSerieMotor.Text = sqlResultado["fcMotor"].ToString();
                            txtSerieChasis.Text = sqlResultado["fcChasis"].ToString();
                            txtSerieDos.Text = sqlResultado["fcSerieDos"].ToString();
                            txtGPS.Text = sqlResultado["fcGPS"].ToString();
                            txtNumeroPrestamo.Text = sqlResultado["fcPrestamo"].ToString();
                            txtComentario.InnerText = sqlResultado["fcComentario"].ToString();
                            EsDigitadoManualmente = (bool)sqlResultado["fbDigitadoManualmente"];

                            txtIdentidadPropietario.Text = sqlResultado["fcIdentidadPropietarioGarantia"].ToString();
                            txtNombrePropietario.Text = sqlResultado["fcNombrePropietarioGarantia"].ToString();
                            idEstadoCivilPropietario = sqlResultado["fiIDEstadoCivilPropietarioGarantia"].ToString();
                            idNacionalidadPropietario = sqlResultado["fiIDNacionalidadPropietarioGarantia"].ToString();

                            txtIdentidadVendedor.Text = sqlResultado["fcIdentidadVendedorGarantia"].ToString();
                            txtNombreVendedor.Text = sqlResultado["fcNombreVendedorGarantia"].ToString();
                            idEstadoCivilVendedor = sqlResultado["fiIDEstadoCivilVendedorGarantia"].ToString();
                            idNacionalidadVendedor = sqlResultado["fiIDNacionalidadVendedorGarantia"].ToString();
                        }

                        /* Cuarto resultado: Catalogo de estados civiles */
                        sqlResultado.NextResult();

                        ddlEstadoCivilPropietario.Items.Clear();
                        ddlEstadoCivilPropietario.Items.Add(new ListItem("Seleccionar", "0"));

                        ddlEstadoCivilVendedor.Items.Clear();
                        ddlEstadoCivilVendedor.Items.Add(new ListItem("Seleccionar", "0"));

                        while (sqlResultado.Read())
                        {
                            ddlEstadoCivilPropietario.Items.Add(new ListItem(sqlResultado["fcDescripcionEstadoCivil"].ToString(), sqlResultado["fiIDEstadoCivil"].ToString()));
                            ddlEstadoCivilVendedor.Items.Add(new ListItem(sqlResultado["fcDescripcionEstadoCivil"].ToString(), sqlResultado["fiIDEstadoCivil"].ToString()));
                        }

                        ddlEstadoCivilPropietario.SelectedValue = idEstadoCivilPropietario;
                        ddlEstadoCivilVendedor.SelectedValue = idEstadoCivilVendedor;

                        /* Quinto resultado: Catalogo de nacionalidades */
                        sqlResultado.NextResult();

                        ddlNacionalidadPropietario.Items.Clear();
                        ddlNacionalidadPropietario.Items.Add(new ListItem("Seleccionar", "0"));

                        ddlNacionalidadVendedor.Items.Clear();
                        ddlNacionalidadVendedor.Items.Add(new ListItem("Seleccionar", "0"));

                        while (sqlResultado.Read())
                        {
                            ddlNacionalidadPropietario.Items.Add(new ListItem(sqlResultado["fcDescripcionNacionalidad"].ToString(), sqlResultado["fiIDNacionalidad"].ToString()));
                            ddlNacionalidadVendedor.Items.Add(new ListItem(sqlResultado["fcDescripcionNacionalidad"].ToString(), sqlResultado["fiIDNacionalidad"].ToString()));
                        }

                        ddlNacionalidadPropietario.SelectedValue = idNacionalidadPropietario;
                        ddlNacionalidadVendedor.SelectedValue = idNacionalidadVendedor;

                    } // using sqlComando.ExecuteReader()
                } // using sqlComando
            } // using sqlConexion

            ddlUnidadDeMedida.Items.Clear();
            ddlUnidadDeMedida.Items.Add(new ListItem("Kilómetros", "KM"));
            ddlUnidadDeMedida.Items.Add(new ListItem("Millas", "M"));
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    [WebMethod]
    public static List<SeccionGarantia_ViewModel> CargarDocumentosRequeridos()
    {
        return (List<SeccionGarantia_ViewModel>)HttpContext.Current.Session["Documentos_Secciones_Garantia_Actualizar"];
    }

    [WebMethod]
    public static Resultado_ViewModel ActualizarGarantia(Garantia_ViewModel garantia, string dataCrypt)
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
                    var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp") ?? "0";
                    var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID") ?? "0";
                    var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr") ?? "0";
                    var pcIDGarantia = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDGarantia") ?? "0";
                    var pcIDSolicitud = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDSol") ?? "0";

                    using (var sqlComando = new SqlCommand("sp_CREDGarantias_Actualizar", sqlConexion, tran))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDGarantia", pcIDGarantia);
                        sqlComando.Parameters.AddWithValue("@piIDCanal", 1);
                        sqlComando.Parameters.AddWithValue("@pcPrestamo", garantia.NumeroPrestamo);
                        sqlComando.Parameters.AddWithValue("@pcVin", garantia.VIN);
                        sqlComando.Parameters.AddWithValue("@pcTipoGarantia", garantia.TipoDeGarantia);
                        sqlComando.Parameters.AddWithValue("@pcTipoVehiculo", garantia.TipoDeVehiculo);
                        sqlComando.Parameters.AddWithValue("@pcMarca", garantia.Marca);
                        sqlComando.Parameters.AddWithValue("@pcModelo", garantia.Modelo);
                        sqlComando.Parameters.AddWithValue("@piAnio", garantia.Anio);
                        sqlComando.Parameters.AddWithValue("@pcColor", garantia.Color);
                        sqlComando.Parameters.AddWithValue("@pcCilindraje", garantia.Cilindraje);
                        sqlComando.Parameters.AddWithValue("@pnRecorrido", garantia.Recorrido);
                        sqlComando.Parameters.AddWithValue("@pcUnidadDeDistancia", garantia.UnidadDeDistancia);
                        sqlComando.Parameters.AddWithValue("@pcTransmision", garantia.Transmision);
                        sqlComando.Parameters.AddWithValue("@pcTipoCombustible", garantia.TipoDeCombustible);
                        sqlComando.Parameters.AddWithValue("@pcMatricula", garantia.Matricula);
                        sqlComando.Parameters.AddWithValue("@pcSerieUno", garantia.SerieUno);
                        sqlComando.Parameters.AddWithValue("@pcSerieDos", garantia.SerieDos);
                        sqlComando.Parameters.AddWithValue("@pcChasis", garantia.SerieChasis);
                        sqlComando.Parameters.AddWithValue("@pcMotor", garantia.SerieMotor);
                        sqlComando.Parameters.AddWithValue("@pcGPS", garantia.GPS);
                        sqlComando.Parameters.AddWithValue("@pnValorGarantia", garantia.ValorMercado);
                        sqlComando.Parameters.AddWithValue("@pnValorPrima", garantia.ValorPrima);
                        sqlComando.Parameters.AddWithValue("@pnValorFinanciado", garantia.ValorFinanciado);
                        sqlComando.Parameters.AddWithValue("@pnGastosDeCierre", garantia.GastosDeCierre);

                        sqlComando.Parameters.AddWithValue("@pcNombrePropietarioGarantia", garantia.NombrePropietario);
                        sqlComando.Parameters.AddWithValue("@pcIdentidadPropietarioGarantia", garantia.IdentidadPropietario);
                        sqlComando.Parameters.AddWithValue("@piIDNacionalidadPropietarioGarantia", garantia.IdNacionalidadPropietario);
                        sqlComando.Parameters.AddWithValue("@piIDEstadoCivilPropietarioGarantia", garantia.IdEstadoCivilPropietario);

                        sqlComando.Parameters.AddWithValue("@pcNombreVendedorGarantia", garantia.NombreVendedor);
                        sqlComando.Parameters.AddWithValue("@pcIdentidadVendedorGarantia", garantia.IdentidadVendedor);
                        sqlComando.Parameters.AddWithValue("@piIDNacionalidadVendedorGarantia", garantia.IdNacionalidadVendedor);
                        sqlComando.Parameters.AddWithValue("@piIDEstadoCivilVendedorGarantia", garantia.IdEstadoCivilVendedor);

                        sqlComando.Parameters.AddWithValue("@pcComentario", garantia.Comentario);
                        sqlComando.Parameters.AddWithValue("@pbDigitadoManualmente", garantia.EsDigitadoManualmente);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
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
                                    resultado.MensajeResultado = "No se pudo actualizar la información de la garantía, contacte al administrador.";
                                    resultado.DebugString = resultadoSp;

                                    if (resultadoSp.Contains("Violation of UNIQUE KEY") && pcIDSolicitud != "0")
                                    {
                                        resultado.MensajeResultado = "El VIN que intenta guardar ya está asociado a esta solicitud.";
                                    }
                                    else
                                    {
                                        resultado.MensajeResultado = "El VIN que intenta guardar ya está registrado sin solicitud.";
                                    }
                                    return resultado;
                                }
                            } // sqlResultado.Read()
                        } // sqlComando.ExecuteReader()
                    } // using sqlComando

                    /* Lista de documentos que se va ingresar en la base de datos y se va mover al nuevo directorio */
                    var garantiaDocumentos = new List<SolicitudesDocumentosViewModel>();

                    /* Registrar documentacion de la solicitud */
                    if (HttpContext.Current.Session["ListaDocumentosGarantia_Actualizar"] != null)
                    {
                        /* lista de documentos adjuntados por el usuario */
                        var listaDocumentos = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaDocumentosGarantia_Actualizar"];

                        if (listaDocumentos != null)
                        {
                            string NombreCarpetaDocumentos = "Solicitud" + pcIDSolicitud;
                            string NuevoNombreDocumento = "";

                            foreach (SolicitudesDocumentosViewModel file in listaDocumentos)
                            {
                                if (File.Exists(file.fcRutaArchivo + @"\" + file.NombreAntiguo)) /* si el archivo existe, que se agregue a la lista */
                                {
                                    NuevoNombreDocumento = GenerarNombreDocumento(pcIDSolicitud, garantia.VIN);

                                    garantiaDocumentos.Add(new SolicitudesDocumentosViewModel()
                                    {
                                        fcNombreArchivo = NuevoNombreDocumento,
                                        NombreAntiguo = file.NombreAntiguo,
                                        fcTipoArchivo = file.fcTipoArchivo,
                                        fcRutaArchivo = file.fcRutaArchivo.Replace("Temp", "") + NombreCarpetaDocumentos,
                                        URLArchivo = "/Documentos/Solicitudes/" + NombreCarpetaDocumentos + "/" + NuevoNombreDocumento + ".png",
                                        fiTipoDocumento = file.fiTipoDocumento
                                    });
                                } // if File.Exists
                            } // foreach lista documentos
                        } // if lista documentos != null
                    } // if Session["ListaDocumentosGarantia_Actualizar"] != null

                    /* Guardar los documentos de la garantia en la base de datos*/
                    int contadorErrores = 0;
                    foreach (SolicitudesDocumentosViewModel documento in garantiaDocumentos)
                    {
                        using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDGarantias_Documentos_Actualizar", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDGarantia", pcIDGarantia);
                            sqlComando.Parameters.AddWithValue("@pcNombreArchivo", documento.fcNombreArchivo);
                            sqlComando.Parameters.AddWithValue("@pcExtension", ".png");
                            sqlComando.Parameters.AddWithValue("@pcRutaArchivo", documento.fcRutaArchivo);
                            sqlComando.Parameters.AddWithValue("@pcURL", documento.URLArchivo);
                            sqlComando.Parameters.AddWithValue("@piIDSeccionGarantia", documento.fiTipoDocumento);
                            sqlComando.Parameters.AddWithValue("@pcComentario", "");
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
                            resultado.MensajeResultado = "No se pudo actualizar la informacion de la garantía, contacte al administrador.";
                            resultado.DebugString = "Garantias_Documentos_Insert";
                            return resultado;
                        }
                    }

                    if (!GuardarDocumentosGarantia(garantiaDocumentos, pcIDSolicitud))
                    {
                        resultado.ResultadoExitoso = false;
                        resultado.MensajeResultado = "No se pudo actualizar la documentación de la garantía, contacte al administrador..";
                        resultado.DebugString = "GuardarDocumentosGarantia()";
                        return resultado;
                    }

                    tran.Commit();

                    resultado.ResultadoExitoso = true;
                    resultado.MensajeResultado = "La información de la garantía se actualizó correctamente";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    resultado.ResultadoExitoso = false;
                    resultado.MensajeResultado = "No se pudo actualizar la información de la garantía, contacte al administrador.";
                    resultado.DebugString = ex.Message.ToString();
                }
            }
        }
        return resultado;
    }

    private static string GenerarNombreDocumento(string idSolicitud, string vin)
    {
        return ("G_" + idSolicitud + "_" + vin + "_" + Guid.NewGuid()).Replace("*", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");
    }

    public static bool GuardarDocumentosGarantia(List<SolicitudesDocumentosViewModel> ListaDocumentos, string idSolicitud)
    {
        bool result;
        try
        {
            if (ListaDocumentos != null)
            {
                /* Crear el nuevo directorio para los documentos de la garantia */
                string DirectorioTemporal = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";
                string NombreCarpetaDocumentos = "Solicitud" + idSolicitud;
                string DirectorioDocumentos = @"C:\inetpub\wwwroot\Documentos\Solicitudes\" + NombreCarpetaDocumentos + "\\";
                bool CarpetaExistente = Directory.Exists(DirectorioDocumentos);

                if (!CarpetaExistente)
                    Directory.CreateDirectory(DirectorioDocumentos);

                ListaDocumentos.ForEach(documento =>
                {
                    string ViejoDirectorio = DirectorioTemporal + documento.NombreAntiguo;
                    string NuevoNombreDocumento = documento.fcNombreArchivo;
                    string NuevoDirectorio = DirectorioDocumentos + NuevoNombreDocumento + ".png";

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
            {
                lcParametros = Url.Substring(liParamStart, Url.Length - liParamStart);
            }
            else
            {
                lcParametros = string.Empty;
            }

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

    #region View Models

    public class SeccionGarantia_ViewModel
    {
        public int IdSeccionGarantia { get; set; }
        public string DescripcionSeccion { get; set; }
    }

    public class Garantia_ViewModel
    {
        public int IdGarantia { get; set; }
        public string NumeroPrestamo { get; set; }
        public string VIN { get; set; }
        public string TipoDeGarantia { get; set; }
        public string TipoDeVehiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Anio { get; set; }
        public string Color { get; set; }
        public string Cilindraje { get; set; }
        public decimal Recorrido { get; set; }
        public string UnidadDeDistancia { get; set; }
        public string Transmision { get; set; }
        public string TipoDeCombustible { get; set; }
        public string Matricula { get; set; }
        public string SerieUno { get; set; }
        public string SerieDos { get; set; }
        public string SerieChasis { get; set; }
        public string SerieMotor { get; set; }
        public string GPS { get; set; }
        public string Comentario { get; set; }
        public bool EsDigitadoManualmente { get; set; }
        public decimal ValorMercado { get; set; }
        public decimal ValorPrima { get; set; }
        public decimal ValorFinanciado { get; set; }
        public decimal GastosDeCierre { get; set; }

        public string IdentidadPropietario { get; set; }
        public string NombrePropietario { get; set; }
        public int IdNacionalidadPropietario { get; set; }
        public int IdEstadoCivilPropietario { get; set; }

        public string IdentidadVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public int IdNacionalidadVendedor { get; set; }
        public int IdEstadoCivilVendedor { get; set; }
    }

    public class Garantia_Documentos_ViewModel
    {
        public string NombreAntiguo { get; set; }
        public string NombreArchivo { get; set; }
        public string Extension { get; set; }
        public string RutaArchivo { get; set; }
        public string URL { get; set; }
        public int IdSeccionGarantia { get; set; }
        public string Comentario { get; set; }
    }

    public class Resultado_ViewModel
    {
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string DebugString { get; set; }
    }

    #endregion
}