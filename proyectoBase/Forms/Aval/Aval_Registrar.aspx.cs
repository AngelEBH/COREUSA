using adminfiles;
using Newtonsoft.Json;
//using proyectoBase.Models.ViewModel;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using UI.Web.Models.ViewModel;
using System.Configuration;
using static DSCore;

namespace proyectoBase.Forms.Aval
{
    public partial class Aval_Registrar : System.Web.UI.Page
    {
        private String pcEncriptado = "";
        private string pcIDSesion = "";
        private string pcIDUsuario = "";
        private string pcID = "";
        private string pcIDApp = "";
        public static int IDCondicionActual = 0;
        public static int FiIDCondicionActual = 0;
        public int IdSolicitudCondicion { get; set; }

        //public static Precalificado_ViewModel Precalificado;
        public List<TipoDocumento_ViewModel> DocumentosRequeridos;
        private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

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

                    var pcEncriptadoAc = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1));
                    var lcParametroDesencriptadoAC = DSC.Desencriptar(pcEncriptado);
                    var lURLDesencriptadoAC = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);




                    string NombreCliente = String.Empty;
                    int IDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                    int IDCliente = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("cltID"));
                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    string IDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
                    int IDCondicion = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDCOND"));
                    IDCondicionActual = IDCondicion;

                    GuardarDetallesPrecalificado(pcID, IDUsuario);
                   // CargarListas();
                    SqlConnection sqlConexion = null;
                    string sqlConnectionString = "Data Source=172.20.3.152;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                    sqlConexion = new SqlConnection(sqlConnectionString);
                    SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Listar", sqlConexion);
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", IDCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUsuario);
                    sqlConexion.Open();
                    SqlDataReader reader = sqlComando.ExecuteReader();

                    while (reader.Read())
                    {
                        NombreCliente = (string)reader["fcPrimerNombreCliente"] + " " + (string)reader["fcSegundoNombreCliente"] + " " + (string)reader["fcPrimerApellidoCliente"] + " " + (string)reader["fcSegundoApellidoCliente"];
                    }

                    if (sqlConexion != null)
                    {
                        if (sqlConexion.State == ConnectionState.Open)
                            sqlConexion.Close();
                    }
                    if (reader != null)
                        reader.Close();

                    lblIDSolicitud.Text = IDSolicitud;
                    lblNombreCliente.Text = NombreCliente;
                }
            }
        }

        public void GuardarDetallesPrecalificado(string identidad, int pcIDUsuario)
        {
            PrecalificadoViewModel objPrecalificado = null;
            List<cotizadorProductosViewModel> listaCotizadorProductos = new List<cotizadorProductosViewModel>();
            string connectionString = "Data Source=172.20.3.152;Initial Catalog = CoreFinanciero; User ID = WebUser; Password = WebUser123*;Max Pool Size=200;MultipleActiveResultSets=true";
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("EXEC CoreAnalitico.dbo.sp_info_ConsultaEjecutivos @piIDApp, @piIDUsuario, @pcIdentidad", conn);
                cmd.Parameters.AddWithValue("@piIDApp", 103);
                cmd.Parameters.AddWithValue("@piIDUsuario", 1);
                cmd.Parameters.AddWithValue("@pcIdentidad", identidad);
                conn.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
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

                cmd = new SqlCommand("EXEC CoreFinanciero.dbo.sp_CotizadorProductos @piIDUsuario, @piIDProducto, @pcIdentidad, @piConObligaciones, @pnIngresosBrutos, @pnIngresosDisponibles", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                cmd.Parameters.AddWithValue("@piIDProducto", objPrecalificado.tipoProducto);
                cmd.Parameters.AddWithValue("@pcIdentidad", objPrecalificado.identidad);
                cmd.Parameters.AddWithValue("@piConObligaciones", objPrecalificado.obligaciones == 0 ? "0" : "1");
                cmd.Parameters.AddWithValue("@pnIngresosBrutos", objPrecalificado.ingresos);
                cmd.Parameters.AddWithValue("@pnIngresosDisponibles", objPrecalificado.disponible);
                reader = cmd.ExecuteReader();
                //cambios nuevos
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

                HttpContext.Current.Session["precalificadoDelCliente"] = objPrecalificado;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                if (reader != null)
                    reader.Close();
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
            sqlConnectionString = "Data Source=172.20.3.152;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
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
                            sqlComando.Parameters.AddWithValue("@pcIdentidad", avalMaster.fcIdentidadAval);
                            sqlComando.Parameters.AddWithValue("@pcRTN", avalMaster.fcIdentidadAval);
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
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@piIDCliente", IDCliente);
                            sqlComando.Parameters.AddWithValue("@piIDTipoAval", "1");
                            sqlComando.Parameters.AddWithValue("@pcIdentidadAval", avalMaster.fcIdentidadAval);
                            sqlComando.Parameters.AddWithValue("@pcRTN", avalMaster.RTNAval);
                            sqlComando.Parameters.AddWithValue("@pcPrimerNombreAval", avalMaster.fcPrimerNombreAval);
                            sqlComando.Parameters.AddWithValue("@pcSegundoNombreAval", avalMaster.fcSegundoNombreAval);
                            sqlComando.Parameters.AddWithValue("@pcPrimerApellidoAval", avalMaster.fcPrimerApellidoAval);
                            sqlComando.Parameters.AddWithValue("@pcSegundoApellidoAval", avalMaster.fcSegundoApellidoAval);
                            sqlComando.Parameters.AddWithValue("@pcTelefonoAval", avalMaster.fcTelefonoAval);
                            sqlComando.Parameters.AddWithValue("@piIDNacionalidadAval", avalMaster.fiNacionalidad);
                            sqlComando.Parameters.AddWithValue("@pdFechaNacimientoAval", avalMaster.fdFechaNacimientoAval);
                            sqlComando.Parameters.AddWithValue("@pcCorreoElectronicoAval", avalMaster.fcCorreoElectronicoAval);
                            sqlComando.Parameters.AddWithValue("@pcProfesionOficioAval", avalMaster.fcProfesionOficioAval);
                            sqlComando.Parameters.AddWithValue("@pcSexoAval", avalMaster.fcSexoAval);
                            sqlComando.Parameters.AddWithValue("@piIDEstadoCivil", avalMaster.fiIDEstadoCivil);
                            sqlComando.Parameters.AddWithValue("@piIDVivienda", avalMaster.fiIDVivienda);
                            sqlComando.Parameters.AddWithValue("@piTiempoResidir", avalMaster.fiTiempoResidir);
                            //sqlComando.Parameters.AddWithValue("@piIDUsuarioCrea", idUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                            //sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            //sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
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

                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CodigoPostal_ListarDetalle", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piPoblado", avalInformacionLaboral.fiIDBarrioColonia);
                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    avalInformacionLaboral.fiIDCiudad = (int)sqlResultado["fiCodPoblado"];
                                    avalInformacionLaboral.fiIDMunicipio = (int)sqlResultado["fiCodMunicipio"];
                                    avalInformacionLaboral.fiIDDepto = (short)sqlResultado["fiCodDepartamento"];
                                }

                            }

                        }


                            using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionLaboral_Crear", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDAval", IDAvalMaster);
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@pcNombreTrabajo", avalInformacionLaboral.fcNombreTrabajo);
                            sqlComando.Parameters.AddWithValue("@pnIngresosMensuales", avalInformacionLaboral.fiIngresosMensuales);
                            sqlComando.Parameters.AddWithValue("@pcPuestoAsignado", avalInformacionLaboral.fcPuestoAsignado);
                            sqlComando.Parameters.AddWithValue("@pdFechaIngreso", avalInformacionLaboral.fcFechaIngreso);
                            sqlComando.Parameters.AddWithValue("@pcTelefonoEmpresa", avalInformacionLaboral.fdTelefonoEmpresa);
                            sqlComando.Parameters.AddWithValue("@pcExtensionRecursosHumanos", avalInformacionLaboral.fcExtensionRecursosHumanos);
                            sqlComando.Parameters.AddWithValue("@pcExtensionAval", avalInformacionLaboral.fcExtensionAval);
                            sqlComando.Parameters.AddWithValue("@piIDPais", 1);
                            sqlComando.Parameters.AddWithValue("@piIDDepartamento", avalInformacionLaboral.fiIDDepto);
                            sqlComando.Parameters.AddWithValue("@piIDMunicipio", avalInformacionLaboral.fiIDMunicipio);
                            sqlComando.Parameters.AddWithValue("@piIDCiudad", avalInformacionLaboral.fiIDCiudad);
                            sqlComando.Parameters.AddWithValue("@piIDBarrioColonia", avalInformacionLaboral.fiIDBarrioColonia);
                            sqlComando.Parameters.AddWithValue("@pcDireccionDetalladaEmpresa", avalInformacionLaboral.fcDireccionDetalladaEmpresa);
                            sqlComando.Parameters.AddWithValue("@pcReferenciasDireccionDetalladaEmpresa", avalInformacionLaboral.fcReferenciasDireccionDetallada);
                            sqlComando.Parameters.AddWithValue("@pcFuenteOtrosIngresos", avalInformacionLaboral.fcFuenteOtrosIngresos);
                            sqlComando.Parameters.AddWithValue("@pnValorOtrosIngresosMensuales", avalInformacionLaboral.fiValorOtrosIngresosMensuales);
                            //sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", idUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                            //sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            //sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
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

                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CodigoPostal_ListarDetalle", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piPoblado", avalInformacionDomiciliar.fiIDBarrioColonia);
                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    avalInformacionDomiciliar.fiIDCiudad = (int)sqlResultado["fiCodPoblado"];
                                    avalInformacionDomiciliar.fiIDMunicipio = (int)sqlResultado["fiCodMunicipio"];
                                    avalInformacionDomiciliar.fiIDDepto = (short)sqlResultado["fiCodDepartamento"];
                                }

                            }

                        }
                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionDomicilio_Crear", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDAval", IDAvalMaster);
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@piIDPais", 1);
                            sqlComando.Parameters.AddWithValue("@pcTelefonoCasa", avalInformacionDomiciliar.fcTelefonoCasa);
                            sqlComando.Parameters.AddWithValue("@piIDDepartamento", avalInformacionDomiciliar.fiIDDepto);
                            sqlComando.Parameters.AddWithValue("@piIDMunicipio", avalInformacionDomiciliar.fiIDMunicipio);
                            sqlComando.Parameters.AddWithValue("@piIDCiudad", avalInformacionDomiciliar.fiIDCiudad);
                            sqlComando.Parameters.AddWithValue("@piIDBarrioColonia", avalInformacionDomiciliar.fiIDBarrioColonia);
                            sqlComando.Parameters.AddWithValue("@pcDireccionDetallada", avalInformacionDomiciliar.fcDireccionDetallada);
                            sqlComando.Parameters.AddWithValue("@pcReferenciasDireccionDetallada", avalInformacionDomiciliar.fcReferenciasDireccionDetallada);
                            //sqlComando.Parameters.AddWithValue("@piIDUsuarioCrea", idUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                            //sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            //sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
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
                                    sqlComando.Parameters.AddWithValue("@piIDAval", IDAvalMaster);
                                    sqlComando.Parameters.AddWithValue("@pcNombreCompletoConyugue", avalInformacionConyugal.fcNombreCompletoConyugue);
                                    sqlComando.Parameters.AddWithValue("@pcIndentidadConyugue", avalInformacionConyugal.fcIndentidadConyugue);
                                    sqlComando.Parameters.AddWithValue("@pdFechaNacimientoConyugue", avalInformacionConyugal.fdFechaNacimientoConyugue);
                                    sqlComando.Parameters.AddWithValue("@pcTelefonoConyugue", avalInformacionConyugal.fcTelefonoConyugue);
                                    sqlComando.Parameters.AddWithValue("@pcLugarTrabajoConyugue", avalInformacionConyugal.fcLugarTrabajoConyugue);
                                    sqlComando.Parameters.AddWithValue("@pnIngresosMensualesConyugue", avalInformacionConyugal.fcIngresosMensualesConyugue);
                                    sqlComando.Parameters.AddWithValue("@pcTelefonoTrabajoConyugue", avalInformacionConyugal.fcTelefonoTrabajoConyugue);
                                    sqlComando.Parameters.AddWithValue("@piIDUsuarioCrea", idUsuario);
                                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                    sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                                   // sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                                   ///* sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual)*/;
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
                            using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Documentos_Insert", sqlConexion, tran))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                                sqlComando.Parameters.AddWithValue("@fcNombreArchivo", documento.fcNombreArchivo);
                                sqlComando.Parameters.AddWithValue("@fcTipoArchivo", ".png");
                                sqlComando.Parameters.AddWithValue("@fcRutaArchivo", documento.fcRutaArchivo);
                                sqlComando.Parameters.AddWithValue("@fcURL", documento.URLArchivo);
                                sqlComando.Parameters.AddWithValue("@fiTipoDocumento", documento.fiTipoDocumento);
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
                        /*  Condicionamiento Aval   */
                        if (IDCondicionActual == 7)
                        {
                            int fIDCondicion = 0;

                            //using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDAval_CondicionesAvalSolicitud", sqlConexion, tran))
                            //{
                            //    sqlComando.CommandType = CommandType.StoredProcedure;
                            //    sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                            //    using (var sqlResultado = sqlComando.ExecuteReader())
                            //    {
                            //        while (sqlResultado.Read())
                            //        {
                            //            fIDCondicion = (int)sqlResultado["fiIDSolicitudCondicion"];
                                      
                            //        }

                            //    }
                            //    FiIDCondicionActual = fIDCondicion;
                            //}
                     


                            using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitudes_Condiciones_Actualizar", sqlConexion, tran))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComando.Parameters.AddWithValue("@piIDSolicitudCondicion", 7);                            
                                sqlComando.Parameters.AddWithValue("@piIDSolicitud ", IDSOL);
                                sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
                                sqlComando.Parameters.AddWithValue("@piIDUsuario", 1);
                              

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
                        }
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
        public static  SolicitudIngresarDDLViewModel CargarListas()
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
                sqlConnectionString = "Data Source=172.20.3.152;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
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
                sqlComando.Parameters.AddWithValue("@piIDVivienda", 0);
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
                sqlComando.Parameters.AddWithValue("@piIDNacionalidad", 0);
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

                //codigo postal 
                #region  CODIGO POSTAL


                List<CodigoPostalViewModal> CodigoPostalViewModel = new List<CodigoPostalViewModal>();
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CodigoPostal", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                //sqlComando.Parameters.AddWithValue("@fiIDParentesco", 0);
                //sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                //sqlComando.Parameters.AddWithValue("@piIDApp", "107");
                //sqlComando.Parameters.AddWithValue("@piIDUsuario", "1");
                reader = sqlComando.ExecuteReader();
                while (reader.Read())
                {
                    CodigoPostalViewModel.Add(new CodigoPostalViewModal()
                    {
                        fiCodBarrio = (int)reader["fiCodBarrio"],
                        DirrecionCompleta = (string)reader["DirrecionCompleta"]
                       
                    });
                }
                ddls.CodigoPostal = CodigoPostalViewModel;

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
        /*    Actualizacion  Angel       */

 


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
                sqlConnectionString = "Data Source=172.20.3.152;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
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
                sqlConnectionString = "Data Source=172.20.3.152;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
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
                sqlConnectionString = "Data Source=172.20.3.152;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
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



        public class TipoDocumento_ViewModel
        {
            public int IdTipoDocumento { get; set; }
            public string DescripcionTipoDocumento { get; set; }
            public int CantidadMaximaDoucmentos { get; set; }
            public int TipoVisibilidad { get; set; }
        }
        public class AvalMaestroViewModel
        {
            public int fiIDSolicitud { get; set; }
            public int TipoAval { get; set; }
            public int fiIDCliente { get; set; }
            public int fiIDAval { get; set; }
            public string fcIdentidadAval { get; set; }
            public string RTNAval { get; set; }
            public string fcTelefonoAval { get; set; }
            public int fiNacionalidad { get; set; }
            public System.DateTime fdFechaNacimientoAval { get; set; }
            public string fcCorreoElectronicoAval { get; set; }
            public string fcProfesionOficioAval { get; set; }
            public string fcSexoAval { get; set; }
            public int fiIDEstadoCivil { get; set; }
            public int fiIDVivienda { get; set; }
            public Nullable<int> fiTiempoResidir { get; set; }
            public bool fbAvalActivo { get; set; }
            public string fcRazonInactivo { get; set; }
            public int fiIDUsuarioCrea { get; set; }
            public string fcNombreUsuarioCrea { get; set; }
            public System.DateTime fdFechaCrea { get; set; }
            public Nullable<int> fiIDUsuarioModifica { get; set; }
            public string fcNombreUsuarioModifica { get; set; }
            public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
            public string fcPrimerNombreAval { get; set; }
            public string fcSegundoNombreAval { get; set; }
            public string fcPrimerApellidoAval { get; set; }
            public string fcSegundoApellidoAval { get; set; }

            //nacionalidad del Aval
            public string fcDescripcionNacionalidad { get; set; }
            public bool fbNacionalidadActivo { get; set; }

            public string fcDescripcionEstadoCivil { get; set; }
            public bool fbEstadoCivilActivo { get; set; }

            public string fcDescripcionVivienda { get; set; }
            public bool fbViviendaActivo { get; set; }
        }


        public class AvalInformacionLaboralViewModel
        {
            public int fiIDInformacionLaboralAval { get; set; }
            public int fiIDAval { get; set; }
            public int fiIDSolicitud { get; set; }
            public string fcNombreTrabajo { get; set; }
            public decimal fiIngresosMensuales { get; set; }
            public string fcPuestoAsignado { get; set; }
            public System.DateTime fcFechaIngreso { get; set; }
            public string fdTelefonoEmpresa { get; set; }
            public string fcExtensionRecursosHumanos { get; set; }
            public string fcExtensionAval { get; set; }
            public string fcDireccionDetalladaEmpresa { get; set; }
            public string fcFuenteOtrosIngresos { get; set; }
            public Nullable<decimal> fiValorOtrosIngresosMensuales { get; set; }
            public int fiIDUsuarioCrea { get; set; }
            public string fcNombreUsuarioCrea { get; set; }
            public System.DateTime fdFechaCrea { get; set; }
            public Nullable<int> fiIDUsuarioModifica { get; set; }
            public string fcNombreUsuarioModifica { get; set; }
            public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
            public Nullable<int> fiIDBarrioColonia { get; set; }
            public string fcReferenciasDireccionDetallada { get; set; }

            //colonia del Aval
            public string fcNombreBarrioColonia { get; set; }
            public bool fbBarrioColoniaActivo { get; set; }

            //ciudad del Aval
            public int fiIDCiudad { get; set; }
            public string fcNombreCiudad { get; set; }
            public bool fbCiudadActivo { get; set; }

            //ciudad del Aval
            public int fiIDMunicipio { get; set; }
            public string fcNombreMunicipio { get; set; }
            public bool fbMunicipioActivo { get; set; }

            //departamento del Aval
            public int fiIDDepto { get; set; }
            public string fcNombreDepto { get; set; }
            public bool fbDepartamentoActivo { get; set; }

            // proceso de campo
            public string fcLatitud { get; set; }
            public string fcLongitud { get; set; }
            public int fiIDGestorValidador { get; set; }
            public string fcGestorValidadorDomicilio { get; set; }
            public int fiIDInvestigacionDeCampo { get; set; }
            public string fcGestionDomicilio { get; set; }
            public int IDTipoResultado { get; set; }
            public string fcResultadodeCampo { get; set; }
            public DateTime fdFechaValidacion { get; set; }
            public string fcObservacionesCampo { get; set; }
            public int fiIDEstadoDeGestion { get; set; }
            public int fiEstadoDomicilio { get; set; }
        }

        public class AvalInformacionDomicilioViewModel
        {
            public int fiIDInformacionDomicilioAval { get; set; }
            public int fiIDAval { get; set; }
            public int fiIDSolicitud { get; set; }
            public string fcTelefonoCasa { get; set; }
            public string fcDireccionDetallada { get; set; }
            public int fiIDUsuarioCrea { get; set; }
            public string fcNombreUsuarioCrea { get; set; }
            public System.DateTime fdFechaCrea { get; set; }
            public Nullable<int> fiIDUsuarioModifica { get; set; }
            public string fcNombreUsuarioModifica { get; set; }
            public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
            public Nullable<int> fiIDBarrioColonia { get; set; }
            public string fcReferenciasDireccionDetallada { get; set; }

            //colonia del Aval
            public string fcNombreBarrioColonia { get; set; }
            public bool fbBarrioColoniaActivo { get; set; }

            //ciudad del Aval
            public int fiIDCiudad { get; set; }
            public string fcNombreCiudad { get; set; }
            public bool fbCiudadActivo { get; set; }

            //ciudad del Aval
            public int fiIDMunicipio { get; set; }
            public string fcNombreMunicipio { get; set; }
            public bool fbMunicipioActivo { get; set; }

            //departamento del Aval
            public int fiIDDepto { get; set; }
            public string fcNombreDepto { get; set; }
            public bool fbDepartamentoActivo { get; set; }

            // proceso de campo
            public string fcLatitud { get; set; }
            public string fcLongitud { get; set; }
            public int fiIDGestorValidador { get; set; }
            public string fcGestorValidadorDomicilio { get; set; }
            public int fiIDInvestigacionDeCampo { get; set; }
            public string fcGestionDomicilio { get; set; }
            public int IDTipoResultado { get; set; }
            public string fcResultadodeCampo { get; set; }
            public DateTime fdFechaValidacion { get; set; }
            public string fcObservacionesCampo { get; set; }
            public int fiIDEstadoDeGestion { get; set; }
            public int fiEstadoDomicilio { get; set; }
        }

        public class AvalInformacionConyugalViewModel
        {
            public int fiIDInformacionConyugalAval { get; set; }
            public int fiIDAval { get; set; }
            public string fcNombreCompletoConyugue { get; set; }
            public string fcIndentidadConyugue { get; set; }
            public Nullable<System.DateTime> fdFechaNacimientoConyugue { get; set; }
            public string fcTelefonoConyugue { get; set; }
            public string fcLugarTrabajoConyugue { get; set; }
            public Nullable<decimal> fcIngresosMensualesConyugue { get; set; }
            public string fcTelefonoTrabajoConyugue { get; set; }
            public int fiIDUsuarioCrea { get; set; }
            public string fcNombreUsuarioCrea { get; set; }
            public System.DateTime fdFechaCrea { get; set; }
            public Nullable<int> fiIDUsuarioModifica { get; set; }
            public string fcNombreUsuarioModifica { get; set; }
            public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        }

        public class CodigoPostalViewModal
        {
            public int fiCodBarrio { get; set; }
            public string DirrecionCompleta { get; set; }
        }

        public class SolicitudIngresarDDLViewModel
        {
            public List<DepartamentosViewModel> Departamentos { get; set; }
            public List<MunicipiosViewModel> Municipios { get; set; }
            public List<CiudadesViewModel> Ciudades { get; set; }
            public List<BarriosColoniasViewModel> BarriosColonias { get; set; }

            public List<CodigoPostalViewModal> CodigoPostal { get; set; }

            public List<EstadosCivilesViewModel> EstadosCiviles { get; set; }
            public List<NacionalidadesViewModel> Nacionalidades { get; set; }
            public List<TipoPrestamoViewModel> TipoPrestamo { get; set; }
            public List<ViviendaViewModel> Vivienda { get; set; }
            public List<ParentescosViewModel> Parentescos { get; set; }
            public List<TipoDocumentoViewModel> TipoDocumento { get; set; }
        }
    }
}