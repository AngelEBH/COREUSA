using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using UI.Web.Models.ViewModel;

namespace proyectoBase.Forms.Aval
{
    public partial class Aval_Actualizar : System.Web.UI.Page
    {
        private String pcEncriptado = "";
        private string pcIDUsuario = "";
        private string pcIDApp = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            // Guardar documentos
            string type = Request.QueryString["type"];

            if (type != null || Request.HttpMethod == "POST")
            {
                int tipoDocumento = 0;
                tipoDocumento = Convert.ToInt32(Request.QueryString["doc"]);
                Session["tipoDoc"] = tipoDocumento;

                string uploadDir = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";

                FileUploader fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
            { "limit", 1 },
                { "title", "auto" },
                { "uploadDir", uploadDir }
            });

                switch (type)
                {
                    // cargar file
                    case "upload":

                        var data = fileUploader.Upload();

                        if (data["files"].Count == 1)
                            data["files"][0].Remove("file");
                        Response.Write(JsonConvert.SerializeObject(data));
                        break;

                    // remover file
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
                string lcURL = "";
                int liParamStart = 0;
                string lcParametros = "";
                string lcParametroDesencriptado = "";
                Uri lURLDesencriptado = null;
                int IDSOL = 0;

                lcURL = Request.Url.ToString();
                liParamStart = lcURL.IndexOf("?");

                if (liParamStart > 0)
                {
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                }
                else
                {
                    lcParametros = String.Empty;
                }
                if (lcParametros != String.Empty)
                {
                    pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    bool AccesoSolicitud = ValidarVendedor(Convert.ToInt32(pcIDUsuario), IDSOL);

                    if (AccesoSolicitud == false)
                    {
                        string lcScript = "window.open('SolicitudesCredito_Ingresadas.aspx?" + pcEncriptado + "','_self')";
                        Response.Write("<script>");
                        Response.Write(lcScript);
                        Response.Write("</script>");
                    }
                }
                else
                {
                    string lcScript = "window.open('SolicitudesCredito_Ingresadas.aspx?" + pcEncriptado + "','_self')";
                    Response.Write("<script>");
                    Response.Write(lcScript);
                    Response.Write("</script>");
                }
            }

        }

        public bool ValidarVendedor(int IDUsuario, int IDSolicitud)
        {
            bool resultado = true;

            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            BandejaSolicitudesViewModel solicitudes = new BandejaSolicitudesViewModel();
            try
            {

                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);

                #region VERIFICAR EL ACCESO DEL USUARIO AL ANALISIS DE LA SOLICITUD

                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitud);
                sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUsuario);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                while (reader.Read())
                {
                    solicitudes = new BandejaSolicitudesViewModel()
                    {
                        fiIDUsuarioVendedor = (int)reader["fiIDUsuarioVendedor"]
                    };
                }

                if (solicitudes.fiIDUsuarioVendedor == IDUsuario)
                    resultado = true;
                else
                    resultado = false;

                #endregion
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                resultado = false;
            }
            finally
            {
                if (sqlConexion != null)
                {
                    if (sqlConexion.State == ConnectionState.Open)
                        sqlConexion.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return resultado;
        }

        [WebMethod]
        public static bool ActualizarCondicionamientoAval(int ID, string seccionFormulario, string objSeccion)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            bool resultadoProceso = false;
            try
            {
                Aval_Actualizar obj = new Aval_Actualizar();
                string resultadoActualizacion = String.Empty;
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();

                switch (seccionFormulario)
                {
                    case "Correccion Informacion Personal Aval":
                        AvalMaestroViewModel objInfoPersonal = json_serializer.Deserialize<AvalMaestroViewModel>(objSeccion);
                        resultadoActualizacion = obj.actualizarInformacionPersonal(objInfoPersonal);
                        break;
                    case "Correccion Informacion Domiciliar Aval":
                        AvalInformacionDomicilioViewModel objInforDomiciliar = json_serializer.Deserialize<AvalInformacionDomicilioViewModel>(objSeccion);
                        resultadoActualizacion = obj.actualizarInformacionDomiciliar(objInforDomiciliar);
                        break;
                    case "Correccion Informacion Laboral Aval":
                        AvalInformacionLaboralViewModel objAvalInformacionLaboral = json_serializer.Deserialize<AvalInformacionLaboralViewModel>(objSeccion);
                        resultadoActualizacion = obj.actualizarInformacionLaboral(objAvalInformacionLaboral);
                        break;
                    case "Correccion Informacion Conyugal Aval":
                        AvalInformacionConyugalViewModel AvalInformacionConyugal = json_serializer.Deserialize<AvalInformacionConyugalViewModel>(objSeccion);
                        resultadoActualizacion = obj.actualizarInformacionConyugal(AvalInformacionConyugal);
                        break;
                    case "Documentacion Aval":
                        resultadoActualizacion = obj.ActualizarDocumentacion();
                        break;
                }

                if (resultadoActualizacion.StartsWith("-1") || resultadoActualizacion == String.Empty || resultadoActualizacion == "0")
                {
                    resultadoProceso = false;
                    return resultadoProceso;
                }

                string lcURL = HttpContext.Current.Request.Url.ToString();
                Uri lURLDesencriptado = DesencriptarURL(lcURL);
                int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                DateTime fechaActual = DateTime.Now;

                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_ActualizarCondicion", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDSolicitudCondicion", ID);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", "107");
                sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                string MensajeError = String.Empty;
                while (reader.Read())
                {
                    MensajeError = (string)reader["MensajeError"];
                }
                if (!MensajeError.StartsWith("-1"))
                    resultadoProceso = true;
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
                {
                    reader.Close();
                }
            }
            return resultadoProceso;
        }

        public string actualizarInformacionPersonal(AvalMaestroViewModel AvalMaster)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            string MensajeError = String.Empty;

            try
            {
                string lcURL = HttpContext.Current.Request.Url.ToString();
                Uri lURLDesencriptado = DesencriptarURL(lcURL);
                int idUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                int IDAval = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDAval"));
                int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
                string nombreUsuario = "";
                DateTime fechaActual = DateTime.Now;

                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";

                sqlConexion = new SqlConnection(sqlConnectionString);
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_Maestro_Actualizar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDAval", IDAval);
                sqlComando.Parameters.AddWithValue("@fiTipoAval", 1);
                sqlComando.Parameters.AddWithValue("@fcIdentidadAval", AvalMaster.fcIdentidadAval);
                sqlComando.Parameters.AddWithValue("@fcRTNAval", AvalMaster.RTNAval);
                sqlComando.Parameters.AddWithValue("@fcPrimerNombreAval", AvalMaster.fcPrimerNombreAval);
                sqlComando.Parameters.AddWithValue("@fcSegundoNombreAval", AvalMaster.fcSegundoNombreAval);
                sqlComando.Parameters.AddWithValue("@fcPrimerApellidoAval", AvalMaster.fcPrimerApellidoAval);
                sqlComando.Parameters.AddWithValue("@fcSegundoApellidoAval", AvalMaster.fcSegundoApellidoAval);
                sqlComando.Parameters.AddWithValue("@fcTelefonoAval", AvalMaster.fcTelefonoAval);
                sqlComando.Parameters.AddWithValue("@fiNacionalidadAval", AvalMaster.fiNacionalidad);
                sqlComando.Parameters.AddWithValue("@fdFechaNacimientoAval", AvalMaster.fdFechaNacimientoAval);
                sqlComando.Parameters.AddWithValue("@fcCorreoElectronicoAval", AvalMaster.fcCorreoElectronicoAval);
                sqlComando.Parameters.AddWithValue("@fcProfesionOficioAval", AvalMaster.fcProfesionOficioAval);
                sqlComando.Parameters.AddWithValue("@fcSexoAval", AvalMaster.fcSexoAval);
                sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", AvalMaster.fiIDEstadoCivil);
                sqlComando.Parameters.AddWithValue("@fiIDVivienda", AvalMaster.fiIDVivienda);
                sqlComando.Parameters.AddWithValue("@fiTiempoResidir", AvalMaster.fiTiempoResidir);
                sqlComando.Parameters.AddWithValue("@fbAvalActivo", true);
                sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", idUsuario);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();
                // iterar resultado del procedimiento almacenado
                while (reader.Read())
                {
                    MensajeError = (string)reader["MensajeError"];
                }
                /* PENDIENTE VERIFICAR SI EL NUEVO ESTADO CIVIL DEL AVAL REQUIERE INFORMACION CONYUGAL, EN CASO DE QUE NO, BORRAR EL REGISTRO*/
                //AvalMaster.fiIDEstadoCivil
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
                {
                    reader.Close();
                }
            }
            return MensajeError;
        }

        public string actualizarInformacionDomiciliar(AvalInformacionDomicilioViewModel AvalInformacionDomiciliar)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            string MensajeError = String.Empty;

            try
            {
                string lcURL = HttpContext.Current.Request.Url.ToString();
                Uri lURLDesencriptado = DesencriptarURL(lcURL);
                int idUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                int IDAval = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDAval"));
                int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
                string nombreUsuario = "";
                DateTime fechaActual = DateTime.Now;

                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionDomicilio_Actualizar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDAval", IDAval);
                sqlComando.Parameters.AddWithValue("@fiIDDepartamento", AvalInformacionDomiciliar.fiIDDepto);
                sqlComando.Parameters.AddWithValue("@fiIDMunicipio", AvalInformacionDomiciliar.fiIDMunicipio);
                sqlComando.Parameters.AddWithValue("@fiIDCiudad", AvalInformacionDomiciliar.fiIDCiudad);
                sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", AvalInformacionDomiciliar.fiIDBarrioColonia);
                sqlComando.Parameters.AddWithValue("@fcTelefonoCasa", AvalInformacionDomiciliar.fcTelefonoCasa);
                sqlComando.Parameters.AddWithValue("@fcDireccionDetallada", AvalInformacionDomiciliar.fcDireccionDetallada);
                sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", AvalInformacionDomiciliar.fcReferenciasDireccionDetallada);
                sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", idUsuario);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();
                // iterar resultado del procedimiento almacenado
                while (reader.Read())
                {
                    MensajeError = (string)reader["MensajeError"];
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
                {
                    reader.Close();
                }
            }
            return MensajeError;
        }

        public string actualizarInformacionLaboral(AvalInformacionLaboralViewModel AvalInformacionLaboral)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            string MensajeError = String.Empty;

            try
            {
                string lcURL = HttpContext.Current.Request.Url.ToString();
                Uri lURLDesencriptado = DesencriptarURL(lcURL);
                int idUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                int IDAval = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDAval"));
                int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
                string nombreUsuario = "";
                DateTime fechaActual = DateTime.Now;

                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);

                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionLaboral_Actualizar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDAval", IDAval);
                sqlComando.Parameters.AddWithValue("@fcNombreTrabajo", AvalInformacionLaboral.fcNombreTrabajo);
                sqlComando.Parameters.AddWithValue("@fnIngresosMensuales", AvalInformacionLaboral.fiIngresosMensuales);
                sqlComando.Parameters.AddWithValue("@fcPuestoAsignado", AvalInformacionLaboral.fcPuestoAsignado);
                sqlComando.Parameters.AddWithValue("@fdFechaIngreso", AvalInformacionLaboral.fcFechaIngreso);
                sqlComando.Parameters.AddWithValue("@fcTelefonoEmpresa", AvalInformacionLaboral.fdTelefonoEmpresa);
                sqlComando.Parameters.AddWithValue("@fcExtensionRecursosHumanos", AvalInformacionLaboral.fcExtensionRecursosHumanos);
                sqlComando.Parameters.AddWithValue("@fcExtensionAval", AvalInformacionLaboral.fcExtensionAval);
                sqlComando.Parameters.AddWithValue("@fiIDDepartamento", AvalInformacionLaboral.fiIDDepto);
                sqlComando.Parameters.AddWithValue("@fiIDMunicipio", AvalInformacionLaboral.fiIDMunicipio);
                sqlComando.Parameters.AddWithValue("@fiIDCiudad", AvalInformacionLaboral.fiIDCiudad);
                sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", AvalInformacionLaboral.fiIDBarrioColonia);
                sqlComando.Parameters.AddWithValue("@fcDireccionDetalladaEmpresa", AvalInformacionLaboral.fcDireccionDetalladaEmpresa);
                sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", AvalInformacionLaboral.fcReferenciasDireccionDetallada);
                sqlComando.Parameters.AddWithValue("@fcFuenteOtrosIngresos", AvalInformacionLaboral.fcFuenteOtrosIngresos);
                sqlComando.Parameters.AddWithValue("@fnValorOtrosIngresosMensuales", AvalInformacionLaboral.fiValorOtrosIngresosMensuales);
                sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", idUsuario);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                while (reader.Read())
                {
                    MensajeError = (string)reader["MensajeError"];
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
                {
                    reader.Close();
                }
            }
            return MensajeError;
        }

        public string actualizarInformacionConyugal(AvalInformacionConyugalViewModel AvalInformacionConyugal)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            string MensajeError = String.Empty;

            try
            {
                string lcURL = HttpContext.Current.Request.Url.ToString();
                Uri lURLDesencriptado = DesencriptarURL(lcURL);
                int idUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                int IDAval = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDAval"));
                int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
                string nombreUsuario = "";
                DateTime fechaActual = DateTime.Now;

                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";

                sqlConexion = new SqlConnection(sqlConnectionString);
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionConyugal_Actualizar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                //agregar valor a los parametros del PA
                sqlComando.Parameters.AddWithValue("@fiIDAval", IDAval);
                sqlComando.Parameters.AddWithValue("@fcNombreCompletoConyugue", AvalInformacionConyugal.fcNombreCompletoConyugue);
                sqlComando.Parameters.AddWithValue("@fcIndentidadConyugue", AvalInformacionConyugal.fcIndentidadConyugue);
                sqlComando.Parameters.AddWithValue("@fdFechaNacimientoConyugue", AvalInformacionConyugal.fdFechaNacimientoConyugue);
                sqlComando.Parameters.AddWithValue("@fcTelefonoConyugue", AvalInformacionConyugal.fcTelefonoConyugue);
                sqlComando.Parameters.AddWithValue("@fcLugarTrabajoConyugue", AvalInformacionConyugal.fcLugarTrabajoConyugue);
                sqlComando.Parameters.AddWithValue("@fnIngresosMensualesConyugue", AvalInformacionConyugal.fcIngresosMensualesConyugue);
                sqlComando.Parameters.AddWithValue("@fcTelefonoTrabajoConyugue", AvalInformacionConyugal.fcTelefonoTrabajoConyugue);
                sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", idUsuario);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                sqlComando.Parameters.AddWithValue("@piIDUsuario", "1");
                sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();
                // iterar resultado del procedimiento almacenado
                while (reader.Read())
                {
                    MensajeError = (string)reader["MensajeError"];
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
                {
                    reader.Close();
                }
            }
            return MensajeError;
        }

        public string ActualizarDocumentacion()
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            string MensajeError = String.Empty;
            string resultadoProceso = String.Empty;
            try
            {
                DateTime fechaActual = DateTime.Now;
                string lcURL = HttpContext.Current.Request.Url.ToString();
                Uri lURLDesencriptado = DesencriptarURL(lcURL);
                int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));

                List<SolicitudesDocumentosViewModel> SolicitudesDocumentos = new List<SolicitudesDocumentosViewModel>();

                var listaDocumentos = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                int DocumentacionCliente = 1;
                int IDOrigen = 1;

                string NombreCarpetaDocumentos = "Solicitud" + IDSOL;


                if (listaDocumentos != null)
                {
                    foreach (SolicitudesDocumentosViewModel file in listaDocumentos)
                    {
                        //si el archivo existe, que se agregue a la lista
                        if (File.Exists(file.fcRutaArchivo + @"\" + file.NombreAntiguo))
                        {
                            int tipoDocumento = (int)file.fiTipoDocumento;

                            string NuevoNombreDocumento = GenerarNombreArchivo(IDSOL, DocumentacionCliente, tipoDocumento, IDOrigen);

                            SolicitudesDocumentos.Add(new SolicitudesDocumentosViewModel()
                            {
                                fcNombreArchivo = NuevoNombreDocumento,
                                NombreAntiguo = file.NombreAntiguo,
                                fcTipoArchivo = file.fcTipoArchivo,
                                fcRutaArchivo = file.fcRutaArchivo.Replace("Temp", "") + NombreCarpetaDocumentos,
                                //URLArchivo = "/Documentos/Solicitudes/" + NombreCarpetaDocumentos + "/" + NuevoNombreDocumento + file.fcTipoArchivo,
                                URLArchivo = "/Documentos/Solicitudes/" + NombreCarpetaDocumentos + "/" + NuevoNombreDocumento + ".png",
                                fiTipoDocumento = file.fiTipoDocumento
                            });
                        }
                    }
                }
                else
                    return "-1";

                if (SolicitudesDocumentos.Count <= 0)
                    return "-1";

                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";

                using (sqlConexion = new SqlConnection(sqlConnectionString))
                {
                    sqlConexion.Open();

                    using (SqlTransaction tran = sqlConexion.BeginTransaction())
                    {
                        int contadorErrores = 0;
                        foreach (SolicitudesDocumentosViewModel documento in SolicitudesDocumentos)
                        {
                            using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Documentos_Insert", sqlConexion, tran))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                //agregar valor a los parametros del PA
                                sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                                sqlComando.Parameters.AddWithValue("@fcNombreArchivo", documento.fcNombreArchivo);
                                //sqlComando.Parameters.AddWithValue("@fcTipoArchivo", documento.fcTipoArchivo);
                                sqlComando.Parameters.AddWithValue("@fcTipoArchivo", ".png");
                                sqlComando.Parameters.AddWithValue("@fcRutaArchivo", documento.fcRutaArchivo);
                                sqlComando.Parameters.AddWithValue("@fcURL", documento.URLArchivo);
                                sqlComando.Parameters.AddWithValue("@fiTipoDocumento", documento.fiTipoDocumento);
                                sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", IDUSR);
                                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
                                sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                                sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                                using (reader = sqlComando.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        MensajeError = (string)reader["MensajeError"];
                                        if (MensajeError.StartsWith("-1"))
                                        {
                                            contadorErrores++;
                                        }
                                    }
                                }
                            }
                        }

                        /* Mover documentos al directorio de la solicitud */

                        bool GuardarDocumentos = FileUploader.GuardarSolicitudDocumentos(IDSOL, SolicitudesDocumentos);

                        if (!GuardarDocumentos)
                        {
                            contadorErrores++;
                        }

                        //verificar resultado del proceso
                        if (contadorErrores == 0)
                        {
                            tran.Commit();
                        }
                        else
                        {
                            tran.Rollback();
                            resultadoProceso = "-1";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                resultadoProceso = "-1" + ex.Message.ToString();
            }
            finally
            {
                if (sqlConexion != null)
                {
                    if (sqlConexion.State == ConnectionState.Open)
                        sqlConexion.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
            }
            return MensajeError;
        }

        [WebMethod]
        public static List<CondicionesViewModel> DetallesCondicion()
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            List<CondicionesViewModel> condicionesSolicitud = new List<CondicionesViewModel>();

            try
            {
                string lcURL = HttpContext.Current.Request.Url.ToString();

                Uri lURLDesencriptado = DesencriptarURL(lcURL);

                int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));

                int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));

                int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));

                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);

                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_SolicitudCondiciones_Listar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;

                sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                string MensajeError = String.Empty;
                while (reader.Read())
                {
                    condicionesSolicitud.Add(new CondicionesViewModel()
                    {
                        IDSolicitudCondicion = (int)reader["fiIDSolicitudCondicion"],
                        IDCondicion = (int)reader["fiIDCondicion"],
                        IDSolicitud = (int)reader["fiIDSolicitud"],
                        TipoCondicion = (string)reader["fcCondicion"],
                        DescripcionCondicion = (string)reader["fcDescripcionCondicion"],
                        ComentarioAdicional = (string)reader["fcComentarioAdicional"],
                        Estado = (bool)reader["fbEstadoCondicion"]
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
                {
                    reader.Close();
                }
            }
            return condicionesSolicitud;
        }

        [WebMethod]
        public static SolicitudIngresarDDLViewModel CargarListas()
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
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

                //configurar comando SQL
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoDepartamento", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                //agregar valor a los parametros del PA
                sqlComando.Parameters.AddWithValue("@piPais", 1);
                sqlComando.Parameters.AddWithValue("@piDepartamento", 0);
                //ejecutar comando
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                //verificar resultado del procedimiento
                string MensajeError = String.Empty;

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

                MensajeError = String.Empty;

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

                //configurar comando SQL
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCatalogo_EstadosCiviles_Listar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                //agregar valor a los parametros del PA
                sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", 0);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", "107");
                sqlComando.Parameters.AddWithValue("@piIDUsuario", "1");
                //ejecutar comando
                reader = sqlComando.ExecuteReader();

                MensajeError = String.Empty;

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

                MensajeError = String.Empty;

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

                #region PRODUCTOS

                List<TipoPrestamoViewModel> TipoPrestamoViewModel = new List<TipoPrestamoViewModel>();

                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCatalogo_TipoProducto_Listar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;

                sqlComando.Parameters.AddWithValue("@fiIDProducto", 0);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", "107");
                sqlComando.Parameters.AddWithValue("@piIDUsuario", "1");

                reader = sqlComando.ExecuteReader();

                //verificar resultado del procedimiento
                MensajeError = String.Empty;

                while (reader.Read())
                {
                    TipoPrestamoViewModel.Add(new TipoPrestamoViewModel()
                    {
                        fiIDTipoPrestamo = (int)reader["fiIDProducto"],
                        fcDescripcion = (string)reader["fcProducto"]
                    });
                }

                ddls.TipoPrestamo = TipoPrestamoViewModel;

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

                MensajeError = String.Empty;

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
                {
                    reader.Close();
                }
            }
            return ddls;
        }

        [WebMethod]
        public static List<MunicipiosViewModel> CargarMunicipios(int CODDepto)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            List<MunicipiosViewModel> municipios = new List<MunicipiosViewModel>();
            try
            {
                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);

                #region OBTENER MUNICIPIOS

                //configurar comando SQL
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoMunicipio", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                //agregar valor a los parametros del PA
                sqlComando.Parameters.AddWithValue("@piPais", 1);
                sqlComando.Parameters.AddWithValue("@piDepartamento", CODDepto);
                sqlComando.Parameters.AddWithValue("@piMunicipio", 0);
                //ejecutar comando
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                //verificar resultado del procedimiento
                string MensajeError = String.Empty;

                while (reader.Read())
                {
                    municipios.Add(new MunicipiosViewModel()
                    {
                        fiIDDepto = (short)reader["fiCodDepartamento"],
                        fiIDMunicipio = (short)reader["fiCodMunicipio"],
                        fcNombreMunicipio = (string)reader["fcMunicipio"],
                    });
                }

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
                {
                    reader.Close();
                }
            }
            return municipios;
        }

        [WebMethod]
        public static List<CiudadesViewModel> CargarPoblados(int CODDepto, int CODMunicipio)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            List<CiudadesViewModel> ciudades = new List<CiudadesViewModel>();
            try
            {
                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);

                #region OBTENER CIUDADES/POBLADOS

                //configurar comando SQL
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoPoblado", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                //agregar valor a los parametros del PA
                sqlComando.Parameters.AddWithValue("@piPais", 1);
                sqlComando.Parameters.AddWithValue("@piDepartamento", CODDepto);
                sqlComando.Parameters.AddWithValue("@piMunicipio", CODMunicipio);
                sqlComando.Parameters.AddWithValue("@piPoblado", 0);
                //ejecutar comando
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                //verificar resultado del procedimiento
                string MensajeError = String.Empty;

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
                {
                    reader.Close();
                }
            }
            return ciudades;
        }

        [WebMethod]
        public static List<BarriosColoniasViewModel> CargarBarrios(int CODDepto, int CODMunicipio, int CODPoblado)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando = null;
            String sqlConnectionString = String.Empty;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            List<BarriosColoniasViewModel> barrios = new List<BarriosColoniasViewModel>();
            try
            {
                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);

                #region OBTENER BARRIOS/COLONIAS

                //configurar comando SQL
                sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoBarrios", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                //agregar valor a los parametros del PA
                sqlComando.Parameters.AddWithValue("@piPais", 1);
                sqlComando.Parameters.AddWithValue("@piDepartamento", CODDepto);
                sqlComando.Parameters.AddWithValue("@piMunicipio", CODMunicipio);
                sqlComando.Parameters.AddWithValue("@piPoblado", CODPoblado);
                sqlComando.Parameters.AddWithValue("@piBarrio", 0);
                //ejecutar comando
                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();

                //verificar resultado del procedimiento
                string MensajeError = String.Empty;

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
                {
                    reader.Close();
                }
            }
            return barrios;
        }

        public static Uri DesencriptarURL(string URL)
        {
            string lcParametroDesencriptado = "";
            Uri lURLDesencriptado = null;
            try
            {
                DSCore.DataCrypt DSC = new DSCore.DataCrypt();
                int liParamStart = 0;
                string lcParametros = "";
                String pcEncriptado = "";

                liParamStart = URL.IndexOf("?");

                if (liParamStart > 0)
                {
                    lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
                }
                else
                {
                    lcParametros = String.Empty;
                }
                if (lcParametros != String.Empty)
                {
                    pcEncriptado = URL.Substring((liParamStart + 1), URL.Length - (liParamStart + 1));
                    lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return lURLDesencriptado;
        }

        public static T ConvertFromDBVal<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T);
            }
            else
            {
                return (T)obj;
            }
        }

        private static string GenerarNombreArchivo(int IDSolicitud, int TipoDocumentacion, int TipoDocumento, int IDOrigen)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            string nombre = String.Empty;

            try
            {
                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);

                SqlCommand sqlComando = new SqlCommand("SELECT dbo.fn_CredGenerarNombreArchivo (" + TipoDocumentacion + "," + IDSolicitud + "," + TipoDocumento + "," + IDOrigen + ") AS fcNombreArchivo", sqlConexion);
                sqlComando.CommandType = CommandType.Text;

                sqlConexion.Open();
                reader = sqlComando.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    nombre = reader.GetString(0);
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
                {
                    reader.Close();
                }
            }
            return nombre;
        }

    }
}