using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using UI.Web.Models.ViewModel;

namespace proyectoBase.Forms.Aval
{
    public partial class Aval_Registrar : System.Web.UI.Page
    {
        private String pcEncriptado = "";
        private string pcIDSesion = "";
        private string pcIDUsuario = "";
        private string pcID = "";
        private string pcIDApp = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            string type = Request.QueryString["type"];

            if (type != null || Request.HttpMethod == "POST")
            {
                int TipoDocumento = Convert.ToInt32(Request.QueryString["doc"]);
                Session["tipoDoc"] = TipoDocumento;

                string uploadDir = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";

                FileUploader fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
                        { "limit", 1 },
                        { "title", "auto" },
                        { "uploadDir", uploadDir }
                    });

                switch (type)
                {
                    case "upload":
                        var data = fileUploader.Upload();

                        if (data["files"].Count == 1)
                            data["files"][0].Remove("file");
                        Response.Write(JsonConvert.SerializeObject(data));
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
            else
                HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;

            if (!IsPostBack)
            {
                DSCore.DataCrypt DSC = new DSCore.DataCrypt();
                string lcURL = Request.Url.ToString();
                int liParamStart = lcURL.IndexOf("?");
                string lcParametros;

                if (liParamStart > 0)
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                else
                    lcParametros = String.Empty;

                if (lcParametros != String.Empty)
                {
                    pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    string lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    string NombreCliente = String.Empty;
                    string IDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    string IDCliente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("cltID");
                    string IDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    GuardarDetallesPrecalificado();

                    using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                    {
                        sqlConexion.Open();

                        using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Listar", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDCliente", IDCliente);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUsuario);

                            using (SqlDataReader reader = sqlComando.ExecuteReader())
                            {
                                reader.Read();
                                NombreCliente = (string)reader["fcPrimerNombreCliente"] + " " + (string)reader["fcSegundoNombreCliente"] + " " + (string)reader["fcPrimerApellidoCliente"] + " " + (string)reader["fcSegundoApellidoCliente"];
                            }
                        }
                    }
                    lblIDSolicitud.Text = IDSolicitud;
                    lblNombreCliente.Text = NombreCliente;
                }
            }
        }

        public void GuardarDetallesPrecalificado()
        {
            PrecalificadoViewModel objPrecalificado = null;
            List<cotizadorProductosViewModel> listaCotizadorProductos = new List<cotizadorProductosViewModel>();
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            try
            {
                using (SqlConnection conn = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("EXEC CoreAnalitico.dbo.sp_info_ConsultaEjecutivos @piIDApp, @piIDUsuario, @pcIdentidad", conn))
                    {
                        cmd.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        cmd.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        cmd.Parameters.AddWithValue("@pcIdentidad", pcID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            reader.Read();
                            identidadAval.Text = (string)reader["fcIdentidad"];
                            identidadAval.Enabled = false;
                            primerNombreAval.Text = (string)reader["fcPrimerNombre"];
                            primerNombreAval.Enabled = false;
                            SegundoNombreAval.Text = (string)reader["fcSegundoNombre"];
                            SegundoNombreAval.Enabled = false;
                            primerApellidoAval.Text = (string)reader["fcPrimerApellido"];
                            primerApellidoAval.Enabled = false;
                            segundoApellidoAval.Text = (string)reader["fcSegundoApellido"];
                            segundoApellidoAval.Enabled = false;
                            numeroTelefono.Text = (string)reader["fcTelefono"];
                            numeroTelefono.Enabled = false;
                            objPrecalificado = new PrecalificadoViewModel()
                            {
                                identidad = (string)reader["fcIdentidad"],
                                primerNombre = (string)reader["fcPrimerNombre"],
                                segundoNombre = (string)reader["fcSegundoNombre"],
                                primerApellido = (string)reader["fcPrimerApellido"],
                                segundoApellido = (string)reader["fcSegundoApellido"],
                                telefono = (string)reader["fcTelefono"],
                                obligaciones = Decimal.Parse(reader["fnTotalObligaciones"].ToString()),
                                ingresos = Decimal.Parse(reader["fnIngresos"].ToString()),
                                disponible = Decimal.Parse(reader["fnCapacidadDisponible"].ToString()),
                                fechaNacimiento = DateTime.Parse(reader["fdFechadeNacimiento"].ToString()),
                                tipoSolicitud = (int)reader["fiTipoSolicitudCliente"],
                                tipoProducto = (int)reader["fiIDProducto"],
                                Producto = (string)reader["fcProducto"]
                            };
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("CoreFinanciero.dbo.sp_CotizadorProductos", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        cmd.Parameters.AddWithValue("@piIDProducto", objPrecalificado.tipoProducto);
                        cmd.Parameters.AddWithValue("@pcIdentidad", objPrecalificado.identidad);
                        cmd.Parameters.AddWithValue("@piConObligaciones", objPrecalificado.obligaciones == 0 ? "0" : "1");
                        cmd.Parameters.AddWithValue("@pnIngresosBrutos", objPrecalificado.ingresos);
                        cmd.Parameters.AddWithValue("@pnIngresosDisponibles", objPrecalificado.disponible);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            cotizadorProductosViewModel objCotizador = new cotizadorProductosViewModel();
                            decimal valorMasAlto = 0;
                            int IDContador = 1;

                            while (reader.Read())
                            {
                                if (valorMasAlto < decimal.Parse(reader["fnMontoOfertado"].ToString()))
                                {
                                    objCotizador = new cotizadorProductosViewModel()
                                    {
                                        IDCotizacion = IDContador,
                                        IDProducto = (int)reader["fiIDProducto"],
                                        ProductoDescripcion = reader["fcProducto"].ToString(),
                                        fnMontoOfertado = decimal.Parse(reader["fnMontoOfertado"].ToString()),
                                        fiPlazo = int.Parse(reader["fiPlazo"].ToString()),
                                        fnCuotaQuincenal = decimal.Parse(reader["fnCuotaQuincenal"].ToString())
                                    };
                                }
                                valorMasAlto = decimal.Parse(reader["fnMontoOfertado"].ToString());
                                IDContador += 1;
                            }
                            //objPrecalificado.cotizadorProductos = listaCotizadorProductos;
                            objPrecalificado.cotizadorProductos = new List<cotizadorProductosViewModel>();
                            objPrecalificado.cotizadorProductos.Add(objCotizador);
                        }
                    }                    
                }
                HttpContext.Current.Session["precalificadoDelCliente"] = objPrecalificado;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        [WebMethod]
        public static ResponseEntitie RegistrarAval(AvalMaestroViewModel avalMaster, AvalInformacionLaboralViewModel avalInformacionLaboral, AvalInformacionDomicilioViewModel avalInformacionDomiciliar, AvalInformacionConyugalViewModel avalInformacionConyugal)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando;
            String sqlConnectionString;
            string MensajeError;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            ResponseEntitie resultadoProceso = new ResponseEntitie();
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int idUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDCliente = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("cltID"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string nombreUsuario = "";
            DateTime fechaActual = DateTime.Now;
            sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (sqlConexion = new SqlConnection(sqlConnectionString))
            {
                sqlConexion.Open();
                using (SqlTransaction tran = sqlConexion.BeginTransaction())
                {
                    try
                    {
                        int contadorErrores = 0;
                        int IDAvalMaster = 0;

                        #region VERIFICAR DUPLICIDAD DE LA IDENTIDAD
                        int DuplicidadIdentidad = 0;
                        int DuplicidadRTN = 0;

                        using (sqlComando = new SqlCommand("dbo.sp_CredAval_ValidarDuplicidadIdentidades", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fcIdentidadAval", avalMaster.fcIdentidadAval);
                            sqlComando.Parameters.AddWithValue("@RTNAVAL", avalMaster.RTNAval);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DuplicidadIdentidad = (int)reader["fiValidacionIdentidad"];
                                    DuplicidadRTN = (int)reader["fiValidacionRTN"];
                                }
                            }
                        }
                        if (DuplicidadIdentidad > 0 || DuplicidadRTN > 0)
                        {
                            string MensajeDuplicidad = string.Empty;

                            if (DuplicidadIdentidad > 0 && DuplicidadRTN > 0)
                                MensajeDuplicidad = "Error de duplicidad, el número de identidad y RTN ingresados ya existen";
                            else if (DuplicidadIdentidad > 0)
                                MensajeDuplicidad = "Error de duplicidad, el número de identidad ingresado ya existe";
                            else if (DuplicidadRTN > 0)
                                MensajeDuplicidad = "Error de duplicidad, el número de RTN ingresado ya existe";

                            resultadoProceso.response = false;
                            resultadoProceso.message = MensajeDuplicidad;
                            return resultadoProceso;
                        }
                        #endregion

                        #region REGISTRAR AVAL MASTER
                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_Maestro_Crear", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@fiIDCliente", IDCliente);
                            sqlComando.Parameters.AddWithValue("@fiTipoAval", "1");
                            sqlComando.Parameters.AddWithValue("@fcIdentidadAval", avalMaster.fcIdentidadAval);
                            sqlComando.Parameters.AddWithValue("@fcRTN", avalMaster.RTNAval);
                            sqlComando.Parameters.AddWithValue("@fcPrimerNombreAval", avalMaster.fcPrimerNombreAval);
                            sqlComando.Parameters.AddWithValue("@fcSegundoNombreAval", avalMaster.fcSegundoNombreAval);
                            sqlComando.Parameters.AddWithValue("@fcPrimerApellidoAval", avalMaster.fcPrimerApellidoAval);
                            sqlComando.Parameters.AddWithValue("@fcSegundoApellidoAval", avalMaster.fcSegundoApellidoAval);
                            sqlComando.Parameters.AddWithValue("@fcTelefonoAval", avalMaster.fcTelefonoAval);
                            sqlComando.Parameters.AddWithValue("@fiNacionalidadAval", avalMaster.fiNacionalidad);
                            sqlComando.Parameters.AddWithValue("@fdFechaNacimientoAval", avalMaster.fdFechaNacimientoAval);
                            sqlComando.Parameters.AddWithValue("@fcCorreoElectronicoAval", avalMaster.fcCorreoElectronicoAval);
                            sqlComando.Parameters.AddWithValue("@fcProfesionOficioAval", avalMaster.fcProfesionOficioAval);
                            sqlComando.Parameters.AddWithValue("@fcSexoAval", avalMaster.fcSexoAval);
                            sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", avalMaster.fiIDEstadoCivil);
                            sqlComando.Parameters.AddWithValue("@fiIDVivienda", avalMaster.fiIDVivienda);
                            sqlComando.Parameters.AddWithValue("@fiTiempoResidir", avalMaster.fiTiempoResidir);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", idUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    MensajeError = (string)reader["MensajeError"];

                                    if (MensajeError.StartsWith("-1"))
                                        contadorErrores++;
                                    else
                                        IDAvalMaster = Convert.ToInt32(MensajeError);
                                }
                            }
                        }
                        if (contadorErrores > 0 || IDAvalMaster == 0)
                        {
                            tran.Rollback();
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al registrar información personal del Aval";
                            return resultadoProceso;
                        }
                        #endregion

                        #region REGISTRAR AVAL INFORMACION LABORAL
                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionLaboral_Crear", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDAval", IDAvalMaster);
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@fcNombreTrabajo", avalInformacionLaboral.fcNombreTrabajo);
                            sqlComando.Parameters.AddWithValue("@fnIngresosMensuales", avalInformacionLaboral.fiIngresosMensuales);
                            sqlComando.Parameters.AddWithValue("@fcPuestoAsignado", avalInformacionLaboral.fcPuestoAsignado);
                            sqlComando.Parameters.AddWithValue("@fdFechaIngreso", avalInformacionLaboral.fcFechaIngreso);
                            sqlComando.Parameters.AddWithValue("@fcTelefonoEmpresa", avalInformacionLaboral.fdTelefonoEmpresa);
                            sqlComando.Parameters.AddWithValue("@fcExtensionRecursosHumanos", avalInformacionLaboral.fcExtensionRecursosHumanos);
                            sqlComando.Parameters.AddWithValue("@fcExtensionAval", avalInformacionLaboral.fcExtensionAval);
                            sqlComando.Parameters.AddWithValue("@fiIDDepartamento", avalInformacionLaboral.fiIDDepto);
                            sqlComando.Parameters.AddWithValue("@fiIDMunicipio", avalInformacionLaboral.fiIDMunicipio);
                            sqlComando.Parameters.AddWithValue("@fiIDCiudad", avalInformacionLaboral.fiIDCiudad);
                            sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", avalInformacionLaboral.fiIDBarrioColonia);
                            sqlComando.Parameters.AddWithValue("@fcDireccionDetalladaEmpresa", avalInformacionLaboral.fcDireccionDetalladaEmpresa);
                            sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", avalInformacionLaboral.fcReferenciasDireccionDetallada);
                            sqlComando.Parameters.AddWithValue("@fcFuenteOtrosIngresos", avalInformacionLaboral.fcFuenteOtrosIngresos);
                            sqlComando.Parameters.AddWithValue("@fnValorOtrosIngresosMensuales", avalInformacionLaboral.fiValorOtrosIngresosMensuales);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", idUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    MensajeError = (string)reader["MensajeError"];

                                    if (MensajeError.StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            tran.Rollback();
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al registrar información laboral del Aval";
                            return resultadoProceso;
                        }
                        #endregion

                        #region REGISTRAR AVAL INFORMACION DOMICILIAR
                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionDomicilio_Crear", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDAval", IDAvalMaster);
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@fcTelefonoCasa", avalInformacionDomiciliar.fcTelefonoCasa);
                            sqlComando.Parameters.AddWithValue("@fiIDDepartamento", avalInformacionDomiciliar.fiIDDepto);
                            sqlComando.Parameters.AddWithValue("@fiIDMunicipio", avalInformacionDomiciliar.fiIDMunicipio);
                            sqlComando.Parameters.AddWithValue("@fiIDCiudad", avalInformacionDomiciliar.fiIDCiudad);
                            sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", avalInformacionDomiciliar.fiIDBarrioColonia);
                            sqlComando.Parameters.AddWithValue("@fcDireccionDetallada", avalInformacionDomiciliar.fcDireccionDetallada);
                            sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", avalInformacionDomiciliar.fcReferenciasDireccionDetallada);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", idUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    MensajeError = (string)reader["MensajeError"];

                                    if (MensajeError.StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            tran.Rollback();
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al registrar información domicilar del Aval";
                            return resultadoProceso;
                        }
                        #endregion

                        #region REGISTRAR AVAL INFORMACION CONYUGAL

                        if (avalInformacionConyugal != null)
                        {
                            if (avalInformacionConyugal.fcIndentidadConyugue != "")
                            {
                                using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionConyugal_Crear", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDAval", IDAvalMaster);
                                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoConyugue", avalInformacionConyugal.fcNombreCompletoConyugue);
                                    sqlComando.Parameters.AddWithValue("@fcIndentidadConyugue", avalInformacionConyugal.fcIndentidadConyugue);
                                    sqlComando.Parameters.AddWithValue("@fdFechaNacimientoConyugue", avalInformacionConyugal.fdFechaNacimientoConyugue);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoConyugue", avalInformacionConyugal.fcTelefonoConyugue);
                                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoConyugue", avalInformacionConyugal.fcLugarTrabajoConyugue);
                                    sqlComando.Parameters.AddWithValue("@fnIngresosMensualesConyugue", avalInformacionConyugal.fcIngresosMensualesConyugue);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoTrabajoConyugue", avalInformacionConyugal.fcTelefonoTrabajoConyugue);
                                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", idUsuario);
                                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                    sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                                    sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                                    using (reader = sqlComando.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            MensajeError = (string)reader["MensajeError"];

                                            if (MensajeError.StartsWith("-1"))
                                                contadorErrores++;
                                        }
                                    }
                                }
                                if (contadorErrores > 0)
                                {
                                    tran.Rollback();
                                    resultadoProceso.response = false;
                                    resultadoProceso.message = "Error al registrar información conyugal del Aval";
                                    return resultadoProceso;
                                }
                            }
                        }
                        #endregion

                        #region REGISTRAR DOCUMENTACIÓN DE LA SOLICITUD
                        int DocumentacionAval = 2;
                        string NombreCarpetaDocumentos = "Solicitud" + IDSOL;
                        string NuevoNombreDocumento = "";

                        // lista de documentos adjuntados por el usuario
                        var listaDocumentos = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                        // Lista de documentos que se va ingresar en la base de datos y se va mover al nuevo directorio
                        List<SolicitudesDocumentosViewModel> SolicitudesDocumentos = new List<SolicitudesDocumentosViewModel>();

                        if (listaDocumentos != null)
                        {
                            // lista de bloques y la cantidad de documentos que contiene cada uno
                            var Bloques = listaDocumentos.GroupBy(TipoDocumento => TipoDocumento.fiTipoDocumento).Select(x => new { x.Key, Count = x.Count() });

                            // lista donde se guardara temporalmente los documentos adjuntados por el usuario dependiendo del tipo de documento en el iterador
                            List<SolicitudesDocumentosViewModel> DocumentosBloque = new List<SolicitudesDocumentosViewModel>();

                            foreach (var Bloque in Bloques)
                            {
                                int TipoDocumento = (int)Bloque.Key;
                                int CantidadDocumentos = Bloque.Count;

                                DocumentosBloque = listaDocumentos.Where(x => x.fiTipoDocumento == TipoDocumento).ToList();// documentos de este bloque
                                String[] NombresGenerador = Funciones.MultiNombres.GenerarNombreCredDocumento(DocumentacionAval, IDSOL, TipoDocumento, CantidadDocumentos);

                                int ContadorNombre = 0;
                                foreach (SolicitudesDocumentosViewModel file in DocumentosBloque)
                                {
                                    if (File.Exists(file.fcRutaArchivo + @"\" + file.NombreAntiguo))//si el archivo existe, que se agregue a la lista
                                    {
                                        NuevoNombreDocumento = NombresGenerador[ContadorNombre];
                                        SolicitudesDocumentos.Add(new SolicitudesDocumentosViewModel()
                                        {
                                            fcNombreArchivo = NuevoNombreDocumento,
                                            NombreAntiguo = file.NombreAntiguo,
                                            fcTipoArchivo = file.fcTipoArchivo,
                                            fcRutaArchivo = file.fcRutaArchivo.Replace("Temp", "") + NombreCarpetaDocumentos,
                                            URLArchivo = "/Documentos/Solicitudes/" + NombreCarpetaDocumentos + "/" + NuevoNombreDocumento + ".png",
                                            fiTipoDocumento = file.fiTipoDocumento
                                        });
                                        ContadorNombre++;
                                    }
                                }
                            }
                        }
                        else
                        {
                            tran.Rollback();
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al registrar la documentación, compruebe los documentos adjuntados o vuelva a cargarlos";
                            return resultadoProceso;
                        }
                        if (SolicitudesDocumentos.Count <= 0)
                        {
                            tran.Rollback();
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al guardar documentación, compruebe que haya cargado los documentos correctamente o vuelva a cargarlos";
                            return resultadoProceso;
                        }

                        foreach (SolicitudesDocumentosViewModel documento in SolicitudesDocumentos)
                        {
                            using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitudes_Documentos_Guardar", sqlConexion, tran))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                                sqlComando.Parameters.AddWithValue("@pcNombreArchivo", documento.fcNombreArchivo);
                                sqlComando.Parameters.AddWithValue("@pcTipoArchivo", ".png");
                                sqlComando.Parameters.AddWithValue("@pcRutaArchivo", documento.fcRutaArchivo);
                                sqlComando.Parameters.AddWithValue("@pcURL", documento.URLArchivo);
                                sqlComando.Parameters.AddWithValue("@piTipoDocumento", documento.fiTipoDocumento);
                                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComando.Parameters.AddWithValue("@piIDUsuario", "1");
                                sqlComando.CommandTimeout = 120;

                                using (reader = sqlComando.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        MensajeError = (string)reader["MensajeError"];
                                        if (MensajeError.StartsWith("-1"))
                                            contadorErrores++;
                                    }
                                }
                            }
                            if (contadorErrores > 0)
                            {
                                tran.Rollback();
                                resultadoProceso.response = false;
                                resultadoProceso.message = "Error al registrar documentación de la solicitud";
                                return resultadoProceso;
                            }
                        }

                        /* Mover documentos al directorio de la solicitud */
                        if (!FileUploader.GuardarSolicitudDocumentos(IDSOL, SolicitudesDocumentos))
                        {
                            tran.Rollback();
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al guardar la documentación de la solicitud";
                            return resultadoProceso;
                        }
                        #endregion

                        #region FINALIZAR CONDICIONAMIENTO POR REGISTRO DE AVAL
                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_FinalizarCondicionamientoRegistrarAval", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    MensajeError = (string)reader["MensajeError"];
                                    if (MensajeError.StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            tran.Rollback();
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al finalizar condicion de la solicitud por registro de Aval";
                            return resultadoProceso;
                        }
                        #endregion

                        tran.Commit();
                        resultadoProceso.response = true;
                        resultadoProceso.message = "Aval registrado de forma correcta";
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                        tran.Rollback();
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar el aval";
                    }
                    finally
                    {
                        if (sqlConexion != null)
                        {
                            if (sqlConexion.State == ConnectionState.Open)
                                sqlConexion.Close();
                        }
                        if (reader != null)
                            reader.Close();
                    }
                }
            }


            return resultadoProceso;
        }

        [WebMethod]
        public static SolicitudIngresarDDLViewModel CargarListas()
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            string sqlConnectionString;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            SolicitudIngresarDDLViewModel ddls = new SolicitudIngresarDDLViewModel();
            try
            {
                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);

                #region DEPARTAMENTOS
                List<DepartamentosViewModel> viewModelDepto = new List<DepartamentosViewModel>();
                SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoDepartamento", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@piPais", 1);
                sqlComando.Parameters.AddWithValue("@piDepartamento", 0);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();
                while (reader.Read())
                {
                    viewModelDepto.Add(new DepartamentosViewModel()
                    {
                        fiIDDepto = (short)reader["fiCodDepartamento"],
                        fcNombreDepto = (string)reader["fcDepartamento"]
                    });
                }
                ddls.Departamentos = viewModelDepto;

                if (reader != null)
                    reader.Close();
                sqlComando.Dispose();
                #endregion

                #region VIVIENDAS
                List<ViviendaViewModel> viewModelVivienda = new List<ViviendaViewModel>();
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCatalogo_Vivienda_Listar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDVivienda", 0);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", "107");
                sqlComando.Parameters.AddWithValue("@piIDUsuario", "1");
                reader = sqlComando.ExecuteReader();
                while (reader.Read())
                {
                    viewModelVivienda.Add(new ViviendaViewModel()
                    {
                        fiIDVivienda = (int)reader["fiIDVivienda"],
                        fcDescripcionVivienda = (string)reader["fcDescripcionVivienda"],
                        fbViviendaActivo = (bool)reader["fbViviendaActivo"],
                        fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                        fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                        fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                        fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                    });
                }
                ddls.Vivienda = viewModelVivienda;

                if (reader != null)
                    reader.Close();
                sqlComando.Dispose();
                #endregion

                #region ESTADOS CIVILES
                List<EstadosCivilesViewModel> viewModelEstadosCiviles = new List<EstadosCivilesViewModel>();
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredCatalogo_EstadosCiviles_Listar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", 0);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", "107");
                sqlComando.Parameters.AddWithValue("@piIDUsuario", "1");
                reader = sqlComando.ExecuteReader();
                while (reader.Read())
                {
                    viewModelEstadosCiviles.Add(new EstadosCivilesViewModel()
                    {
                        fiIDEstadoCivil = (int)reader["fiIDEstadoCivil"],
                        fcDescripcionEstadoCivil = (string)reader["fcDescripcionEstadoCivil"],
                        fbEstadoCivilActivo = (bool)reader["fbEstadoCivilActivo"],
                        fbRequiereInformacionConyugal = (bool)reader["fbRequiereInformacionConyugal"],
                        fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                        fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                        fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                        fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                    });
                }
                ddls.EstadosCiviles = viewModelEstadosCiviles;

                if (reader != null)
                    reader.Close();
                sqlComando.Dispose();
                #endregion

                #region NACIONALIDADES
                List<NacionalidadesViewModel> NacionalidadesViewModel = new List<NacionalidadesViewModel>();
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCatalogo_Nacionalidades_Listar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDNacionalidad", 0);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", "107");
                sqlComando.Parameters.AddWithValue("@piIDUsuario", "1");
                reader = sqlComando.ExecuteReader();
                while (reader.Read())
                {
                    NacionalidadesViewModel.Add(new NacionalidadesViewModel()
                    {
                        fiIDNacionalidad = (int)reader["fiIDNacionalidad"],
                        fcDescripcionNacionalidad = (string)reader["fcDescripcionNacionalidad"],
                        fbNacionalidadActivo = (bool)reader["fbNacionalidadActivo"],
                        fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                        fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                        fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                        fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                    });
                }
                ddls.Nacionalidades = NacionalidadesViewModel;

                if (reader != null)
                    reader.Close();
                sqlComando.Dispose();
                #endregion

                #region TIPOS DE DOCUMENTO
                List<TipoDocumentoViewModel> TipoDocumentoViewModel = new List<TipoDocumentoViewModel>();
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredCatalogo_TipoDocumento_RegistrarAval", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                reader = sqlComando.ExecuteReader();
                while (reader.Read())
                {
                    TipoDocumentoViewModel.Add(new TipoDocumentoViewModel()
                    {
                        IDTipoDocumento = (short)reader["fiIDTipoDocumento"],
                        DescripcionTipoDocumento = (string)reader["fcDescripcionTipoDocumento"],
                        CantidadMaximaDoucmentos = (byte)reader["fiCantidadDocumentos"],
                        TipoVisibilidad = (byte)reader["fiTipodeVisibilidad"]
                    });
                }
                ddls.TipoDocumento = TipoDocumentoViewModel;

                if (reader != null)
                    reader.Close();
                sqlComando.Dispose();
                #endregion

                #region PARENTESCOS
                List<ParentescosViewModel> ParentescosViewModel = new List<ParentescosViewModel>();
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCatalogo_Parentescos_Listar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDParentesco", 0);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", "107");
                sqlComando.Parameters.AddWithValue("@piIDUsuario", "1");
                reader = sqlComando.ExecuteReader();
                while (reader.Read())
                {
                    ParentescosViewModel.Add(new ParentescosViewModel()
                    {
                        fiIDParentescos = (int)reader["fiIDParentesco"],
                        fcDescripcionParentesco = (string)reader["fcDescripcionParentesco"],
                        fbParentescoActivo = (bool)reader["fbParentescoActivo"],
                        fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                        fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                        fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                        fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                    });
                }
                ddls.Parentescos = ParentescosViewModel;

                if (reader != null)
                    reader.Close();
                sqlComando.Dispose();
                #endregion
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                if (sqlConexion != null)
                {
                    if (sqlConexion.State == ConnectionState.Open)
                        sqlConexion.Close();
                }
                if (reader != null)
                    reader.Close();
            }
            return ddls;
        }

        [WebMethod]
        public static List<MunicipiosViewModel> CargarMunicipios(int CODDepto)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            string sqlConnectionString;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            List<MunicipiosViewModel> municipios = new List<MunicipiosViewModel>();
            try
            {
                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);
                SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoMunicipio", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@piPais", 1);
                sqlComando.Parameters.AddWithValue("@piDepartamento", CODDepto);
                sqlComando.Parameters.AddWithValue("@piMunicipio", 0);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();
                while (reader.Read())
                {
                    municipios.Add(new MunicipiosViewModel()
                    {
                        fiIDDepto = (short)reader["fiCodDepartamento"],
                        fiIDMunicipio = (short)reader["fiCodMunicipio"],
                        fcNombreMunicipio = (string)reader["fcMunicipio"],
                    });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                if (sqlConexion != null)
                {
                    if (sqlConexion.State == ConnectionState.Open)
                        sqlConexion.Close();
                }
                if (reader != null)
                    reader.Close();
            }
            return municipios;
        }

        [WebMethod]
        public static List<CiudadesViewModel> CargarPoblados(int CODDepto, int CODMunicipio)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            string sqlConnectionString;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            List<CiudadesViewModel> ciudades = new List<CiudadesViewModel>();
            try
            {
                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);
                SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoPoblado", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@piPais", 1);
                sqlComando.Parameters.AddWithValue("@piDepartamento", CODDepto);
                sqlComando.Parameters.AddWithValue("@piMunicipio", CODMunicipio);
                sqlComando.Parameters.AddWithValue("@piPoblado", 0);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                while (reader.Read())
                {
                    ciudades.Add(new CiudadesViewModel()
                    {
                        fiIDDepto = (short)reader["fiCodDepartamento"],
                        fiIDMunicipio = (short)reader["fiCodMunicipio"],
                        fiIDCiudad = (short)reader["fiCodPoblado"],
                        fcNombreCiudad = (string)reader["fcPoblado"],
                    });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                if (sqlConexion != null)
                {
                    if (sqlConexion.State == ConnectionState.Open)
                        sqlConexion.Close();
                }
                if (reader != null)
                    reader.Close();
            }
            return ciudades;
        }

        [WebMethod]
        public static List<BarriosColoniasViewModel> CargarBarrios(int CODDepto, int CODMunicipio, int CODPoblado)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            string sqlConnectionString;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            List<BarriosColoniasViewModel> barrios = new List<BarriosColoniasViewModel>();
            try
            {
                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);
                SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoBarrios", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@piPais", 1);
                sqlComando.Parameters.AddWithValue("@piDepartamento", CODDepto);
                sqlComando.Parameters.AddWithValue("@piMunicipio", CODMunicipio);
                sqlComando.Parameters.AddWithValue("@piPoblado", CODPoblado);
                sqlComando.Parameters.AddWithValue("@piBarrio", 0);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                while (reader.Read())
                {
                    barrios.Add(new BarriosColoniasViewModel()
                    {
                        fiIDDepto = (short)reader["fiCodDepartamento"],
                        fiIDMunicipio = (short)reader["fiCodMunicipio"],
                        fiIDCiudad = (short)reader["fiCodPoblado"],
                        fiIDBarrioColonia = (short)reader["fiCodBarrio"],
                        fcNombreBarrioColonia = (string)reader["fcBarrioColonia"],
                    });
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                if (sqlConexion != null)
                {
                    if (sqlConexion.State == ConnectionState.Open)
                        sqlConexion.Close();
                }
                if (reader != null)
                    reader.Close();
            }
            return barrios;
        }

        public static Uri DesencriptarURL(string URL)
        {
            Uri lURLDesencriptado = null;
            try
            {
                DSCore.DataCrypt DSC = new DSCore.DataCrypt();
                int liParamStart = 0;
                string lcParametros = "";
                String pcEncriptado = "";
                liParamStart = URL.IndexOf("?");

                if (liParamStart > 0)
                    lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
                else
                    lcParametros = String.Empty;

                if (lcParametros != String.Empty)
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
}