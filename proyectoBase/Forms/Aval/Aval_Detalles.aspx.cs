using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

namespace proyectoBase.Forms.Aval
{
    public partial class Aval_Detalles : System.Web.UI.Page
    {
        private string pcIDApp = "";
        private string pcIDSesion = "";
        private string pcIDUsuario = "";
        public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
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
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

                    var idAval = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDAval");
                    var idUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    var idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");                    
                    var nombreCliente = string.Empty;
                    var idCliente = 0;

                    using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                    {
                        sqlConexion.Open();

                        using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitud);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    idCliente = (int)sqlResultado["fiIDCliente"];
                                }
                            }
                        }

                        using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Listar", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDCliente", idCliente);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    nombreCliente = (string)sqlResultado["fcPrimerNombreCliente"] + " " + (string)sqlResultado["fcSegundoNombreCliente"] + " " + (string)sqlResultado["fcPrimerApellidoCliente"] + " " + (string)sqlResultado["fcSegundoApellidoCliente"];
                                }
                            }
                        }
                    }
                    lblIDSolicitud.Text = idSolicitud;
                    lblNombreCliente.Text = nombreCliente;
                }
            }
        }

        [WebMethod]
        public static AvalViewModel DetallesAval()
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            SqlCommand sqlComando;
            String sqlConnectionString;
            AvalViewModel Aval = new AvalViewModel();
            try
            {
                string lcURL = HttpContext.Current.Request.Url.ToString();
                Uri lURLDesencriptado = DesencriptarURL(lcURL);
                int IDAval = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDAval"));
                int idUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp").ToString();
                string nombreUsuario = String.Empty;
                //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
                sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";

                using (sqlConexion = new SqlConnection(sqlConnectionString))
                {
                    sqlConexion.Open();

                    #region OBTENER AVAL MASTER
                    using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_Maestro_Listar", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDAval", IDAval);
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", 0);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                        using (reader = sqlComando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Aval.AvalMaster = new AvalMasterViewModel()
                                {
                                    fiIDAval = (int)reader["fiIDAval"],
                                    fiIDSolicitud = (int)reader["fiIDSolicitud"],
                                    fiIDCliente = (int)reader["fiIDCliente"],
                                    TipoAval = (int)reader["fiTipoAval"],
                                    fcIdentidadAval = (string)reader["fcIdentidadAval"],
                                    RTNAval = (string)reader["fcRTNAval"],
                                    fcPrimerNombreAval = (string)reader["fcPrimerNombreAval"],
                                    fcSegundoNombreAval = (string)reader["fcSegundoNombreAval"],
                                    fcPrimerApellidoAval = (string)reader["fcPrimerApellidoAval"],
                                    fcSegundoApellidoAval = (string)reader["fcSegundoApellidoAval"],
                                    fcTelefonoAval = (string)reader["fcTelefonoPrimarioAval"],
                                    fiNacionalidad = (int)reader["fiNacionalidadAval"],
                                    fcDescripcionNacionalidad = (string)reader["fcDescripcionNacionalidad"],
                                    fdFechaNacimientoAval = (DateTime)reader["fdFechaNacimientoAval"],
                                    fcCorreoElectronicoAval = (string)reader["fcCorreoElectronicoAval"],
                                    fcProfesionOficioAval = (string)reader["fcProfesionOficioAval"],
                                    fcSexoAval = (string)reader["fcSexoAval"],
                                    fiIDEstadoCivil = (int)reader["fiIDEstadoCivil"],
                                    fcDescripcionEstadoCivil = (string)reader["fcDescripcionEstadoCivil"],
                                    fiIDVivienda = (int)reader["fiIDVivienda"],
                                    fcDescripcionVivienda = (string)reader["fcDescripcionVivienda"],
                                    fiTiempoResidir = (short)reader["fiTiempoResidir"],
                                    fbAvalActivo = (bool)reader["fbAvalActivo"],
                                    fcRazonInactivo = (string)reader["fcRazonInactivo"],
                                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                                };
                            }
                        }
                    }
                    #endregion

                    #region OBTENER AVAL INFORMACION LABORAL
                    using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionLaboral_Listar", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDAval", Aval.AvalMaster.fiIDAval);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                        using (reader = sqlComando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Aval.AvalInformacionLaboral = new AvalInformacionLaboralViewModel()
                                {
                                    fiIDInformacionLaboralAval = (int)reader["fiIDInformacionLaboralAval"],
                                    fiIDSolicitud = (int)reader["fiIDSolicitud"],
                                    fiIDAval = (int)reader["fiIDAval"],
                                    fcNombreTrabajo = (string)reader["fcNombreTrabajo"],
                                    fiIngresosMensuales = (decimal)reader["fnIngresosMensuales"],
                                    fcPuestoAsignado = (string)reader["fcPuestoAsignado"],
                                    fcFechaIngreso = (DateTime)reader["fdFechaIngresoAval"],
                                    fdTelefonoEmpresa = (string)reader["fcTelefonoEmpresa"],
                                    fcExtensionRecursosHumanos = (string)reader["fcExtensionRecursosHumanos"],
                                    fcExtensionAval = (string)reader["fcExtensionAval"],
                                    fiIDDepto = (int)reader["fiIDDepartamento"],
                                    fcNombreDepto = (string)reader["fcDepartamento"],
                                    fiIDMunicipio = (int)reader["fiIDMunicipio"],
                                    fcNombreMunicipio = (string)reader["fcMunicipio"],
                                    fiIDCiudad = (int)reader["fiIDCiudad"],
                                    fcNombreCiudad = (string)reader["fcPoblado"],
                                    fiIDBarrioColonia = (int)reader["fiIDBarrioColonia"],
                                    fcNombreBarrioColonia = (string)reader["fcBarrio"],
                                    fcDireccionDetalladaEmpresa = (string)reader["fcDireccionDetalladaEmpresaAval"],
                                    fcReferenciasDireccionDetallada = (string)reader["fcReferenciasDireccionDetalladaAval"],
                                    fcFuenteOtrosIngresos = (string)reader["fcFuenteOtrosIngresos"],
                                    fiValorOtrosIngresosMensuales = (decimal)reader["fnValorOtrosIngresosMensuales"],
                                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                                };
                            }
                        }
                    }
                    #endregion

                    #region OBTENER AVAL INFORMACION DOMICILIAR
                    using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionDomicilio_Listar", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDAval", Aval.AvalMaster.fiIDAval);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                        using (reader = sqlComando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int fiIDInformacionDomicilioAval = (int)reader["fiIDInformacionDomicilioAval"];
                                int fiIDAval = (int)reader["fiIDAval"];
                                int fiIDSolicitud = (int)reader["fiIDSolicitud"];
                                short fiIDDepto = (short)reader["fiCodDepartamento"];
                                string fcNombreDepto = (string)reader["fcDepartamento"];
                                int fiIDMunicipio = (short)reader["fiCodMunicipio"];
                                string fcNombreMunicipio = (string)reader["fcMunicipio"];
                                int fiIDCiudad = (short)reader["fiCodPoblado"];
                                string fcNombreCiudad = (string)reader["fcPoblado"];
                                int fiIDBarrioColonia = (short)reader["fiCodBarrio"];
                                string fcNombreBarrioColonia = (string)reader["fcBarrio"];
                                string fcTelefonoCasa = (string)reader["fcTelefonoCasaAval"];
                                string fcDireccionDetallada = (string)reader["fcDireccionDetalladaAval"];
                                string fcReferenciasDireccionDetallada = (string)reader["fcReferenciasDireccionDetalladaAval"];
                                int fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"];
                                DateTime fdFechaCrea = (DateTime)reader["fdFechaCrea"];
                                int fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"];
                                DateTime fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"];

                                Aval.AvalInformacionDomiciliar = new AvalInformacionDomiciliarViewModel()
                                {
                                    fiIDInformacionDomicilioAval = (int)reader["fiIDInformacionDomicilioAval"],
                                    fiIDAval = (int)reader["fiIDAval"],
                                    fiIDSolicitud = (int)reader["fiIDSolicitud"],
                                    fiIDDepto = (short)reader["fiCodDepartamento"],
                                    fcNombreDepto = (string)reader["fcDepartamento"],
                                    fiIDMunicipio = (short)reader["fiCodMunicipio"],
                                    fcNombreMunicipio = (string)reader["fcMunicipio"],
                                    fiIDCiudad = (short)reader["fiCodPoblado"],
                                    fcNombreCiudad = (string)reader["fcPoblado"],
                                    fiIDBarrioColonia = (short)reader["fiCodBarrio"],
                                    fcNombreBarrioColonia = (string)reader["fcBarrio"],
                                    fcTelefonoCasa = (string)reader["fcTelefonoCasaAval"],
                                    fcDireccionDetallada = (string)reader["fcDireccionDetalladaAval"],
                                    fcReferenciasDireccionDetallada = (string)reader["fcReferenciasDireccionDetalladaAval"],
                                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                                };
                            }
                        }
                    }
                    #endregion

                    #region OBTENER AVAL INFORMACION CONYUGAL
                    using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionConyugal_Listar", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDAval", Aval.AvalMaster.fiIDAval);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                        using (reader = sqlComando.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Aval.AvalInformacionConyugal = new AvalInformacionConyugalViewModel()
                                    {
                                        fiIDInformacionConyugalAval = (int)reader["fiIDInformacionConyugalAval"],
                                        fiIDAval = (int)reader["fiIDAval"],
                                        fcNombreCompletoConyugue = (string)reader["fcNombreCompletoConyugue"],
                                        fcIndentidadConyugue = (string)reader["fcIndentidadConyugue"],
                                        fdFechaNacimientoConyugue = (DateTime)reader["fdFechaNacimientoConyugue"],
                                        fcTelefonoConyugue = (string)reader["fcTelefonoConyugue"],
                                        fcLugarTrabajoConyugue = (string)reader["fcLugarTrabajoConyugue"],
                                        fcIngresosMensualesConyugue = (decimal)reader["fnIngresosMensualesConyugue"],
                                        fcTelefonoTrabajoConyugue = (string)reader["fcTelefonoTrabajoConyugue"],
                                        fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                        fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                        fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                        fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]

                                    };
                                }
                            }
                        }
                    }
                    #endregion

                    #region OBTENER DOCUMENTACIÓN DEL AVAL
                    using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ObtenerSolicitudDocumentos", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDSolicitud", Aval.AvalMaster.fiIDSolicitud);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);
                        using (reader = sqlComando.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                Aval.AvalDocumentos = new List<SolicitudesDocumentosViewModel>();
                                while (reader.Read())
                                {
                                    Aval.AvalDocumentos.Add(new SolicitudesDocumentosViewModel()
                                    {
                                        fiIDSolicitudDocs = (int)reader["fiIDSolicitudDocs"],
                                        fcNombreArchivo = (string)reader["fcNombreArchivo"],
                                        fcTipoArchivo = (string)reader["fcTipoArchivo"],
                                        fcRutaArchivo = (string)reader["fcRutaArchivo"],
                                        URLArchivo = (string)reader["fcURL"],
                                        fcArchivoActivo = (byte)reader["fiArchivoActivo"],
                                        fiTipoDocumento = (int)reader["fiTipoDocumento"]
                                    });
                                }
                            }
                        }
                    }
                    #endregion
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
            return Aval;
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