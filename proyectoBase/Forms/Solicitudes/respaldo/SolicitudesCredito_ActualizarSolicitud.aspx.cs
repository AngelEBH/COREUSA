﻿using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Helpers;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.WebControls;
using UI.Web.Models.ViewModel;

public partial class SolicitudesCredito_ActualizarSolicitud : System.Web.UI.Page
{
    private String pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string type = Request.QueryString["type"];
        if (type != null || Request.HttpMethod == "POST")
        {
            int tipoDocumento = Convert.ToInt32(Request.QueryString["doc"]);
            Session["tipoDoc"] = tipoDocumento;
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
                    if (file != null) {
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
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                bool AccesoSolicitud = ValidarVendedor(Convert.ToInt32(pcIDUsuario), IDSOL, Convert.ToInt32(pcIDApp));
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

    public bool ValidarVendedor(int IDUsuario, int IDSolicitud, int IDApp)
    {
        bool resultado = true;
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        BandejaSolicitudesViewModel solicitudes = new BandejaSolicitudesViewModel();
        try
        {
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitud);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
            sqlComando.Parameters.AddWithValue("@piIDApp", IDApp);
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
                reader.Close();
        }
        return resultado;
    }

    [WebMethod]
    public static SolicitudAnalisisViewModel CargarInformacionSolicitud()
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        SolicitudAnalisisViewModel ObjSolicitud = new SolicitudAnalisisViewModel();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            #region SOLICITUD MAESTRO
            BandejaSolicitudesViewModel SolicitudMaestro = new BandejaSolicitudesViewModel();
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                SolicitudMaestro = new BandejaSolicitudesViewModel()
                {
                    fiIDSolicitud = (int)reader["fiIDSolicitud"],
                    fiIDTipoPrestamo = (int)reader["fiIDTipoProducto"],
                    fcDescripcion = (string)reader["fcProducto"],
                    fiTipoSolicitud = (short)reader["fiTipoSolicitud"],
                    TipoNegociacion = (short)reader["fiTipoNegociacion"],
                    // informacion del precalificado
                    fdValorPmoSugeridoSeleccionado = (decimal)reader["fnValorSeleccionado"],
                    fiPlazoPmoSeleccionado = (int)reader["fiPlazoSeleccionado"],
                    fdIngresoPrecalificado = (decimal)reader["fnIngresoPrecalificado"],
                    fdObligacionesPrecalificado = (decimal)reader["fnObligacionesPrecalificado"],
                    fdDisponiblePrecalificado = (decimal)reader["fnDisponiblePrecalificado"],
                    fnPrima = (decimal)reader["fnValorPrima"],
                    fnValorGarantia = (decimal)reader["fnValorGarantia"],
                    fiEdadCliente = (short)reader["fiEdadCliente"],
                    fnSueldoBaseReal = (decimal)reader["fnSueldoBaseReal"],
                    fnBonosComisionesReal = (decimal)reader["fnBonosComisionesReal"],
                    // informacion del vendedor
                    fiIDUsuarioVendedor = (int)reader["fiIDUsuarioVendedor"],
                    fcNombreCortoVendedor = (string)reader["fcNombreCortoVendedor"],
                    fdFechaCreacionSolicitud = (DateTime)reader["fdFechaCreacionSolicitud"],
                    // informacion del analista
                    fiIDUsuarioModifica = (int)reader["fiIDAnalista"],
                    fcNombreUsuarioModifica = (string)reader["fcNombreCortoAnalista"],
                    fcTipoEmpresa = (string)reader["fcTipoEmpresa"],
                    fcTipoPerfil = (string)reader["fcTipoPerfil"],
                    fcTipoEmpleado = (string)reader["fcTipoEmpleado"],
                    fcBuroActual = (string)reader["fcBuroActual"],
                    fiMontoFinalSugerido = (decimal)reader["fnMontoFinalSugerido"],
                    fiMontoFinalFinanciar = (decimal)reader["fnMontoFinalFinanciar"],
                    fiPlazoFinalAprobado = (int)reader["fiPlazoFinalAprobado"],
                    fiEstadoSolicitud = (byte)reader["fiEstadoSolicitud"],
                    fiSolicitudActiva = (byte)reader["fiSolicitudActiva"],
                    //informacion cliente
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fcNoAgencia = (string)reader["fcCentrodeCosto"],
                    fcAgencia = (string)reader["fcNombreAgencia"],
                    //bitacora
                    fdEnIngresoInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnIngresoInicio"]),
                    fdEnIngresoFin = ConvertFromDBVal<DateTime>((object)reader["fdEnIngresoFin"]),
                    fdEnTramiteInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnColaInicio"]),
                    fdEnTramiteFin = ConvertFromDBVal<DateTime>((object)reader["fdEnColaFin"]),
                    //todo el proceso de analisis
                    fdEnAnalisisInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisInicio"]),
                    ftAnalisisTiempoValidarInformacionPersonal = ConvertFromDBVal<DateTime>((object)reader["fdAnalisisTiempoValidarInformacionPersonal"]),
                    fcComentarioValidacionInfoPersonal = (string)reader["fcComentarioValidacionInfoPersonal"],
                    ftAnalisisTiempoValidarDocumentos = ConvertFromDBVal<DateTime>((object)reader["fdAnalisisTiempoValidarDocumentos"]),
                    fcComentarioValidacionDocumentacion = (string)reader["fcComentarioValidacionDocumentacion"],
                    fbValidacionDocumentcionIdentidades = (byte)reader["fbValidacionDocumentcionIdentidades"],
                    fbValidacionDocumentacionDomiciliar = (byte)reader["fbValidacionDocumentacionDomiciliar"],
                    fbValidacionDocumentacionLaboral = (byte)reader["fbValidacionDocumentacionLaboral"],
                    fbValidacionDocumentacionSolicitudFisica = (byte)reader["fbValidacionDocumentacionSolicitudFisica"],
                    ftAnalisisTiempoValidacionReferenciasPersonales = ConvertFromDBVal<DateTime>((object)reader["fdAnalisisTiempoValidacionReferenciasPersonales"]),
                    fcComentarioValidacionReferenciasPersonales = (string)reader["fcComentarioValidacionReferenciasPersonales"],
                    ftAnalisisTiempoValidarInformacionLaboral = ConvertFromDBVal<DateTime>((object)reader["fdAnalisisTiempoValidarInformacionLaboral"]),
                    fcComentarioValidacionInfoLaboral = (string)reader["fcComentarioValidacionInfoLaboral"],
                    ftTiempoTomaDecisionFinal = ConvertFromDBVal<DateTime>((object)reader["fdTiempoTomaDecisionFinal"]),
                    fcObservacionesDeCredito = (string)reader["fcObservacionesDeCredito"],
                    fcComentarioResolucion = (string)reader["fcComentarioResolucion"],
                    fdEnAnalisisFin = ConvertFromDBVal<DateTime>((object)reader["fdEnAnalisisFin"]),
                    //todo el proceso de analisis
                    fdCondicionadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdCondicionadoInicio"]),
                    fcCondicionadoComentario = (string)reader["fcCondicionadoComentario"],
                    fdCondificionadoFin = ConvertFromDBVal<DateTime>((object)reader["fdCondificionadoFin"]),
                    fiIDOrigen = (short)reader["fiIDOrigen"],
                    //proceso de campo
                    fdEnvioARutaAnalista = ConvertFromDBVal<DateTime>((object)reader["fdEnvioARutaAnalista"]),
                    fiEstadoDeCampo = (byte)reader["fiEstadoDeCampo"],
                    fdEnCampoInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnRutaDeInvestigacionInicio"]),
                    fcObservacionesDeGestoria = (string)reader["fcObservacionesDeCampo"],
                    fdEnCampoFin = ConvertFromDBVal<DateTime>((object)reader["fdEnRutaDeInvestigacionFin"]),
                    fdReprogramadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdReprogramadoInicio"]),
                    fcReprogramadoComentario = (string)reader["fcReprogramadoComentario"],
                    fdReprogramadoFin = ConvertFromDBVal<DateTime>((object)reader["fdReprogramadoFin"]),
                    PasoFinalInicio = (DateTime)reader["fdPasoFinalInicio"],
                    IDUsuarioPasoFinal = (int)reader["fiIDUsuarioPasoFinal"],
                    ComentarioPasoFinal = (string)reader["fcComentarioPasoFinal"],
                    PasoFinalFin = (DateTime)reader["fdPasoFinalFin"],
                };
            }
            sqlComando.Dispose();
            reader.Close();
            ObjSolicitud.solicitud = SolicitudMaestro;
            #endregion

            #region INFORMACION DEL CLIENTE
            ClientesViewModel objCliente = new ClientesViewModel();

            #region CLIENTES MASTER
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", ObjSolicitud.solicitud.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                objCliente.clientesMaster = new ClientesMasterViewModel()
                {
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fcIdentidadCliente = (string)reader["fcIdentidadCliente"],
                    RTNCliente = (string)reader["fcRTN"],
                    fcPrimerNombreCliente = (string)reader["fcPrimerNombreCliente"],
                    fcSegundoNombreCliente = (string)reader["fcSegundoNombreCliente"],
                    fcPrimerApellidoCliente = (string)reader["fcPrimerApellidoCliente"],
                    fcSegundoApellidoCliente = (string)reader["fcSegundoApellidoCliente"],
                    fcTelefonoCliente = (string)reader["fcTelefonoPrimarioCliente"],
                    fiNacionalidadCliente = (int)reader["fiNacionalidadCliente"],
                    fcDescripcionNacionalidad = (string)reader["fcDescripcionNacionalidad"],
                    fbNacionalidadActivo = (bool)reader["fbNacionalidadActivo"],
                    fdFechaNacimientoCliente = (DateTime)reader["fdFechaNacimientoCliente"],
                    fcCorreoElectronicoCliente = (string)reader["fcCorreoElectronicoCliente"],
                    fcProfesionOficioCliente = (string)reader["fcProfesionOficioCliente"],
                    fcSexoCliente = (string)reader["fcSexoCliente"],
                    fiIDEstadoCivil = (int)reader["fiIDEstadoCivil"],
                    fcDescripcionEstadoCivil = (string)reader["fcDescripcionEstadoCivil"],
                    fbEstadoCivilActivo = (bool)reader["fbEstadoCivilActivo"],
                    fiIDVivienda = (int)reader["fiIDVivienda"],
                    fcDescripcionVivienda = (string)reader["fcDescripcionVivienda"],
                    fbViviendaActivo = (bool)reader["fbViviendaActivo"],
                    fiTiempoResidir = (short)reader["fiTiempoResidir"],
                    fbClienteActivo = (bool)reader["fbClienteActivo"],
                    fcRazonInactivo = (string)reader["fcRazonInactivo"],
                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                };
            }
            if (reader != null)
                reader.Close();
            sqlComando.Dispose();
            #endregion

            #region CLIENTE INFORMACION LABORAL
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Laboral_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                objCliente.ClientesInformacionLaboral = new ClientesInformacionLaboralViewModel()
                {
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fiIDInformacionLaboral = (int)reader["fiIDInformacionLaboral"],
                    fcNombreTrabajo = (string)reader["fcNombreTrabajo"],
                    fiIngresosMensuales = (decimal)reader["fnIngresosMensuales"],
                    fcPuestoAsignado = (string)reader["fcPuestoAsignado"],
                    fcFechaIngreso = (DateTime)reader["fdFechaIngreso"],
                    fdTelefonoEmpresa = (string)reader["fcTelefonoEmpresa"],
                    fcExtensionRecursosHumanos = (string)reader["fcExtensionRecursosHumanos"],
                    fcExtensionCliente = (string)reader["fcExtensionCliente"],
                    fcDireccionDetalladaEmpresa = (string)reader["fcDireccionDetalladaEmpresa"],
                    fcReferenciasDireccionDetallada = (string)reader["fcReferenciasDireccionDetalladaEmpresa"],
                    fcFuenteOtrosIngresos = (string)reader["fcFuenteOtrosIngresos"],
                    fiValorOtrosIngresosMensuales = (decimal)reader["fnValorOtrosIngresosMensuales"],
                    fiIDBarrioColonia = (int)reader["fiIDBarrioColonia"],
                    fcNombreBarrioColonia = (string)reader["fcBarrio"],
                    fiIDCiudad = (int)reader["fiIDCiudad"],
                    fcNombreCiudad = (string)reader["fcPoblado"],
                    fiIDMunicipio = (int)reader["fiIDMunicipio"],
                    fcNombreMunicipio = (string)reader["fcMunicipio"],
                    fiIDDepto = (int)reader["fiIDDepartamento"],
                    fcNombreDepto = (string)reader["fcDepartamento"],
                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                };
            }
            if (reader != null)
                reader.Close();
            sqlComando.Dispose();
            #endregion

            #region CLIENTE INFORMACION CONYUGAL
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionConyugal_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fiIDSOlicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                objCliente.ClientesInformacionConyugal = new ClientesInformacionConyugalViewModel()
                {
                    fiIDInformacionConyugal = (int)reader["fiIDInformacionConyugal"],
                    fiIDCliente = (int)reader["fiIDCliente"],
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
            if (reader != null)
                reader.Close();
            sqlComando.Dispose();
            #endregion

            #region CLIENTE INFORMACION DOMICILIAR
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionDomiciliar_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                objCliente.ClientesInformacionDomiciliar = new ClientesInformacionDomiciliarViewModel()
                {
                    fiIDInformacionDomicilio = (int)reader["fiIDInformacionDomicilio"],
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fcTelefonoCasa = (string)reader["fcTelefonoCasa"],
                    fcDireccionDetallada = (string)reader["fcDireccionDetalladaDomicilio"],
                    fcReferenciasDireccionDetallada = (string)reader["fcReferenciasDireccionDetalladaDomicilio"],
                    fiIDBarrioColonia = (short)reader["fiCodBarrio"],
                    fcNombreBarrioColonia = (string)reader["fcBarrio"],
                    fiIDCiudad = (short)reader["fiCodPoblado"],
                    fcNombreCiudad = (string)reader["fcPoblado"],
                    fiIDMunicipio = (short)reader["fiCodMunicipio"],
                    fcNombreMunicipio = (string)reader["fcMunicipio"],
                    fiIDDepto = (short)reader["fiCodDepartamento"],
                    fcNombreDepto = (string)reader["fcDepartamento"],
                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                };
            }
            if (reader != null)
                reader.Close();
            sqlComando.Dispose();
            #endregion

            #region CLIENTE REFERENCIAS PERSONALES
            objCliente.ClientesReferenciasPersonales = new List<ClientesReferenciasViewModel>();
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Referencias_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                objCliente.ClientesReferenciasPersonales.Add(new ClientesReferenciasViewModel()
                {
                    fiIDReferencia = (int)reader["fiIDReferencia"],
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fcNombreCompletoReferencia = (string)reader["fcNombreCompletoReferencia"],
                    fcLugarTrabajoReferencia = (string)reader["fcLugarTrabajoReferencia"],
                    fiTiempoConocerReferencia = (short)reader["fiTiempoConocerReferencia"],
                    fcTelefonoReferencia = (string)reader["fcTelefonoReferencia"],
                    fiIDParentescoReferencia = (int)reader["fiIDParentescoReferencia"],
                    fcDescripcionParentesco = (string)reader["fcDescripcionParentesco"],
                    fbReferenciaActivo = (bool)reader["fbReferenciaActivo"],
                    fcRazonInactivo = (string)reader["fcRazonInactivo"],
                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"],
                    fcComentarioDeptoCredito = (string)reader["fcComentarioDeptoCredito"],
                    fiAnalistaComentario = (int)reader["fiAnalistaComentario"]
                });
            }
            if (reader != null)
                reader.Close();
            sqlComando.Dispose();
            #endregion

            ObjSolicitud.cliente = objCliente;
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
        return ObjSolicitud;
    }

    [WebMethod]
    public static bool ActualizarCondicionamiento(int ID, string seccionFormulario, string objSeccion, int clienteID)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        bool resultadoProceso = false;
        try
        {
            SolicitudesCredito_ActualizarSolicitud obj = new SolicitudesCredito_ActualizarSolicitud();
            string resultadoActualizacion = String.Empty;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            switch (seccionFormulario)
            {
                case "Correccion Informacion de la Solicitud":
                    SolicitudesMasterViewModel objSolicitudesMaster = json_serializer.Deserialize<SolicitudesMasterViewModel>(objSeccion);
                    resultadoActualizacion = obj.ActualizarSolicitudMaster(objSolicitudesMaster);
                    break;
                case "Correccion Informacion Personal":
                    ClientesMasterViewModel objInfoPersonal = json_serializer.Deserialize<ClientesMasterViewModel>(objSeccion);
                    resultadoActualizacion = obj.ActualizarInformacionPersonal(objInfoPersonal);
                    break;
                case "Correccion Informacion Domiciliar":
                    ClientesInformacionDomiciliarViewModel objInforDomiciliar = json_serializer.Deserialize<ClientesInformacionDomiciliarViewModel>(objSeccion);
                    resultadoActualizacion = obj.ActualizarInformacionDomiciliar(objInforDomiciliar);
                    break;
                case "Correccion Informacion Laboral":
                    ClientesInformacionLaboralViewModel objClientesInformacionLaboral = json_serializer.Deserialize<ClientesInformacionLaboralViewModel>(objSeccion);
                    resultadoActualizacion = obj.ActualizarInformacionLaboral(objClientesInformacionLaboral);
                    break;
                case "Correccion Informacion Conyugal":
                    ClientesInformacionConyugalViewModel ClientesInformacionConyugal = json_serializer.Deserialize<ClientesInformacionConyugalViewModel>(objSeccion);
                    resultadoActualizacion = obj.ActualizarInformacionConyugal(ClientesInformacionConyugal);
                    break;
                case "Correccion Referencias":
                    List<ClientesReferenciasViewModel> clientesReferencias = json_serializer.Deserialize<List<ClientesReferenciasViewModel>>(objSeccion);
                    resultadoActualizacion = obj.ActualizarInformacionReferenciasPersonales(clientesReferencias, clienteID);
                    break;
                case "Cambio de Referencias":
                    List<ClientesReferenciasViewModel> clientesCambioReferencias = json_serializer.Deserialize<List<ClientesReferenciasViewModel>>(objSeccion);
                    resultadoActualizacion = obj.ActualizarInformacionReferenciasPersonales(clientesCambioReferencias, clienteID);
                    break;
                case "Documentacion":
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
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string MensajeError = String.Empty;
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_ActualizarCondicion", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitudCondicion", ID);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
                MensajeError = (string)reader["MensajeError"];
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
                reader.Close();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static string FinalizarCondicionamientoSolicitud()
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        string resultadoProceso = String.Empty;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_SolicitudCondiciones_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            int contadorSolicitudesPendientes = 0;
            while (reader.Read())
            {
                bool estado = (bool)reader["fbEstadoCondicion"];
                if (estado == true)
                    contadorSolicitudesPendientes++;
            }
            if (contadorSolicitudesPendientes > 0)
                return "No se puede finalizar el condicionamiento de la solicitud porque tiene condiciones pendientes, revise los detalles.";

            if (reader != null)
                reader.Close();
            sqlComando.Dispose();

            //quitar condicionamiento de la solicitud
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_FinalizarCondicionamiento", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            reader = sqlComando.ExecuteReader();
            string MensajeError = String.Empty;
            
            while (reader.Read())
                MensajeError = (string)reader["MensajeError"];

            if (!MensajeError.StartsWith("-1"))
                resultadoProceso = "Solicitud actualizada correctamente";
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
        return resultadoProceso;
    }

    public string ActualizarSolicitudMaster(SolicitudesMasterViewModel SolicitudesMaster)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        string MensajeError = String.Empty;
        try
        {
            DateTime fechaActual = Helpers.DatetimeNow();
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Maestro_Update", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@fiIDTipoPrestamo", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fcTipoSolicitud", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fcTipoEmpresa", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fcTipoPerfil", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fcTipoEmpleado", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fcBuroActual", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fiMontoFinalSugerido", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fiMontoFinalFinanciar", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fiPlazoFinalAprobado", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fiSolicitudActiva", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fdValorPmoSugeridoSeleccionado", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fiPlazoPmoSeleccionado", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fdIngresoPrecalificado", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fdObligacionesPrecalificado", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fdDisponiblePrecalificado", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fnPrima", SolicitudesMaster.fnPrima);
            sqlComando.Parameters.AddWithValue("@fnValorGarantia", SolicitudesMaster.fnValorGarantia);
            sqlComando.Parameters.AddWithValue("@fiEdadCliente", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fiClienteArraigoLaboralAños", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fiClienteArraigoLaboralMeses", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@fiClienteArraigoLaboralDias", DBNull.Value);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
                MensajeError = (string)reader["MensajeError"];
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
        return MensajeError;
    }

    public string ActualizarInformacionPersonal(ClientesMasterViewModel clienteMaster)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        string MensajeError = String.Empty;
        try
        {
            /* PENDIENTE VALIDAR SI LA NUEVA IDENTIDAD NO ESTÉ DUPLICADA*/
            DateTime fechaActual = Helpers.DatetimeNow();
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Update", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", clienteMaster.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", clienteMaster.fcIdentidadCliente);
            sqlComando.Parameters.AddWithValue("@fcRTN", clienteMaster.RTNCliente);
            sqlComando.Parameters.AddWithValue("@fcPrimerNombreCliente", clienteMaster.fcPrimerNombreCliente);
            sqlComando.Parameters.AddWithValue("@fcSegundoNombreCliente", clienteMaster.fcSegundoNombreCliente);
            sqlComando.Parameters.AddWithValue("@fcPrimerApellidoCliente", clienteMaster.fcPrimerApellidoCliente);
            sqlComando.Parameters.AddWithValue("@fcSegundoApellidoCliente", clienteMaster.fcSegundoApellidoCliente);
            sqlComando.Parameters.AddWithValue("@fcTelefonoCliente", clienteMaster.fcTelefonoCliente);
            sqlComando.Parameters.AddWithValue("@fiNacionalidadCliente", clienteMaster.fiNacionalidadCliente);
            sqlComando.Parameters.AddWithValue("@fdFechaNacimientoCliente", clienteMaster.fdFechaNacimientoCliente);
            sqlComando.Parameters.AddWithValue("@fcCorreoElectronicoCliente", clienteMaster.fcCorreoElectronicoCliente);
            sqlComando.Parameters.AddWithValue("@fcProfesionOficioCliente", clienteMaster.fcProfesionOficioCliente);
            sqlComando.Parameters.AddWithValue("@fcSexoCliente", clienteMaster.fcSexoCliente);
            sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", clienteMaster.fiIDEstadoCivil);
            sqlComando.Parameters.AddWithValue("@fiIDVivienda", clienteMaster.fiIDVivienda);
            sqlComando.Parameters.AddWithValue("@fiTiempoResidir", clienteMaster.fiTiempoResidir);
            sqlComando.Parameters.AddWithValue("@fbClienteActivo", true);
            sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", IDUSR);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            
            while (reader.Read())
                MensajeError = (string)reader["MensajeError"];
            /* PENDIENTE VERIFICAR SI EL NUEVO ESTADO CIVIL DEL CLIENTE REQUIERE INFORMACION CONYUGAL, EN CASO DE QUE NO, BORRAR EL REGISTRO*/
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
        return MensajeError;
    }

    public string ActualizarInformacionDomiciliar(ClientesInformacionDomiciliarViewModel ClientesInformacionDomiciliar)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        string MensajeError = String.Empty;
        try
        {
            DateTime fechaActual = Helpers.DatetimeNow();
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionDomicilio_Update", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", ClientesInformacionDomiciliar.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fiIDDepartamento", ClientesInformacionDomiciliar.fiIDDepto);
            sqlComando.Parameters.AddWithValue("@fiIDMunicipio", ClientesInformacionDomiciliar.fiIDMunicipio);
            sqlComando.Parameters.AddWithValue("@fiIDCiudad", ClientesInformacionDomiciliar.fiIDCiudad);
            sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", ClientesInformacionDomiciliar.fiIDBarrioColonia);
            sqlComando.Parameters.AddWithValue("@fcTelefonoCasa", ClientesInformacionDomiciliar.fcTelefonoCasa);
            sqlComando.Parameters.AddWithValue("@fcDireccionDetallada", ClientesInformacionDomiciliar.fcDireccionDetallada);
            sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", ClientesInformacionDomiciliar.fcReferenciasDireccionDetallada);
            sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", IDUSR);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            
            while (reader.Read())
                MensajeError = (string)reader["MensajeError"];
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
        return MensajeError;
    }

    public string ActualizarInformacionLaboral(ClientesInformacionLaboralViewModel ClientesInformacionLaboral)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        string MensajeError = String.Empty;
        try
        {
            DateTime fechaActual = Helpers.DatetimeNow();
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionLaboral_Update", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", ClientesInformacionLaboral.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fcNombreTrabajo", ClientesInformacionLaboral.fcNombreTrabajo);
            sqlComando.Parameters.AddWithValue("@fnIngresosMensuales", ClientesInformacionLaboral.fiIngresosMensuales);
            sqlComando.Parameters.AddWithValue("@fcPuestoAsignado", ClientesInformacionLaboral.fcPuestoAsignado);
            sqlComando.Parameters.AddWithValue("@fdFechaIngreso", ClientesInformacionLaboral.fcFechaIngreso);
            sqlComando.Parameters.AddWithValue("@fcTelefonoEmpresa", ClientesInformacionLaboral.fdTelefonoEmpresa);
            sqlComando.Parameters.AddWithValue("@fiIDDepartamento", ClientesInformacionLaboral.fiIDDepto);
            sqlComando.Parameters.AddWithValue("@fiIDMunicipio", ClientesInformacionLaboral.fiIDMunicipio);
            sqlComando.Parameters.AddWithValue("@fiIDCiudad", ClientesInformacionLaboral.fiIDCiudad);
            sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", ClientesInformacionLaboral.fiIDBarrioColonia);
            sqlComando.Parameters.AddWithValue("@fcDireccionDetalladaEmpresa", ClientesInformacionLaboral.fcDireccionDetalladaEmpresa);
            sqlComando.Parameters.AddWithValue("@fcFuenteOtrosIngresos", ClientesInformacionLaboral.fcFuenteOtrosIngresos);
            sqlComando.Parameters.AddWithValue("@fnValorOtrosIngresosMensuales", ClientesInformacionLaboral.fiValorOtrosIngresosMensuales);
            sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", ClientesInformacionLaboral.fcReferenciasDireccionDetallada);
            sqlComando.Parameters.AddWithValue("@fcExtensionRecursosHumanos", ClientesInformacionLaboral.fcExtensionRecursosHumanos);
            sqlComando.Parameters.AddWithValue("@fcExtensionCliente", ClientesInformacionLaboral.fcExtensionCliente);
            sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", IDUSR);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
                MensajeError = (string)reader["MensajeError"];
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
        return MensajeError;
    }

    public string ActualizarInformacionConyugal(ClientesInformacionConyugalViewModel ClientesInformacionConyugal)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        string MensajeError = String.Empty;
        try
        {
            DateTime fechaActual = Helpers.DatetimeNow();
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionConyugal_Update", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", ClientesInformacionConyugal.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fcNombreCompletoConyugue", ClientesInformacionConyugal.fcNombreCompletoConyugue);
            sqlComando.Parameters.AddWithValue("@fcIndentidadConyugue", ClientesInformacionConyugal.fcIndentidadConyugue);
            sqlComando.Parameters.AddWithValue("@fdFechaNacimientoConyugue", ClientesInformacionConyugal.fdFechaNacimientoConyugue);
            sqlComando.Parameters.AddWithValue("@fcTelefonoConyugue", ClientesInformacionConyugal.fcTelefonoConyugue);
            sqlComando.Parameters.AddWithValue("@fcLugarTrabajoConyugue", ClientesInformacionConyugal.fcLugarTrabajoConyugue);
            sqlComando.Parameters.AddWithValue("@fnIngresosMensualesConyugue", ClientesInformacionConyugal.fcIngresosMensualesConyugue);
            sqlComando.Parameters.AddWithValue("@fcTelefonoTrabajoConyugue", ClientesInformacionConyugal.fcTelefonoTrabajoConyugue);
            sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", IDUSR);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            
            while (reader.Read())
                MensajeError = (string)reader["MensajeError"];
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
        return MensajeError;
    }

    public string ActualizarInformacionReferenciasPersonales(List<ClientesReferenciasViewModel> ClientesReferencias, int clienteID)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        SqlCommand sqlComando = null;
        String sqlConnectionString = String.Empty;
        string MensajeError = String.Empty;
        int IDCliente = 0;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            DateTime fechaActual = DateTime.Now;
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";

            using (sqlConexion = new SqlConnection(sqlConnectionString))
            {
                sqlConexion.Open();
                using (SqlTransaction tran = sqlConexion.BeginTransaction())
                {
                    try
                    {
                        #region OBTENER REFERENCIAS PERSONALES EXISTENTES DEL CLIENTE
                        List<ClientesReferenciasViewModel> referenciasExistentes = new List<ClientesReferenciasViewModel>();
                        sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Referencias_Listar", sqlConexion, tran);
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", ClientesReferencias[0].fiIDCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
                        using (reader = sqlComando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                referenciasExistentes.Add(new ClientesReferenciasViewModel()
                                {
                                    fiIDReferencia = (int)reader["fiIDReferencia"],
                                    fiIDCliente = (int)reader["fiIDCliente"],
                                    fcNombreCompletoReferencia = (string)reader["fcNombreCompletoReferencia"],
                                    fcLugarTrabajoReferencia = (string)reader["fcLugarTrabajoReferencia"],
                                    fiTiempoConocerReferencia = (short)reader["fiTiempoConocerReferencia"],
                                    fcTelefonoReferencia = (string)reader["fcTelefonoReferencia"],
                                    fiIDParentescoReferencia = (int)reader["fiIDParentescoReferencia"],
                                    fcDescripcionParentesco = (string)reader["fcDescripcionParentesco"],
                                    fbReferenciaActivo = (bool)reader["fbReferenciaActivo"],
                                    fcRazonInactivo = (string)reader["fcRazonInactivo"],
                                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"],
                                    fcComentarioDeptoCredito = (string)reader["fcComentarioDeptoCredito"],
                                    fiAnalistaComentario = (int)reader["fiAnalistaComentario"]

                                });
                            }
                        }
                        IDCliente = clienteID;
                        #endregion

                        #region INSERTAR NUEVAS REFERENCIAS
                        List<ClientesReferenciasViewModel> referenciasInsertar = new List<ClientesReferenciasViewModel>();
                        foreach (ClientesReferenciasViewModel item in ClientesReferencias)
                        {
                            if (!referenciasExistentes.Select(x => x.fiIDReferencia).ToList().Contains(item.fiIDReferencia))
                            {
                                referenciasInsertar.Add(new ClientesReferenciasViewModel()
                                {
                                    fcNombreCompletoReferencia = item.fcNombreCompletoReferencia,
                                    fcLugarTrabajoReferencia = item.fcLugarTrabajoReferencia,
                                    fiTiempoConocerReferencia = item.fiTiempoConocerReferencia,
                                    fcTelefonoReferencia = item.fcTelefonoReferencia,
                                    fiIDParentescoReferencia = item.fiIDParentescoReferencia,
                                });
                            }
                        };
                        if (referenciasInsertar.Count > 0)
                        {
                            foreach (ClientesReferenciasViewModel referencia in referenciasInsertar)
                            {
                                using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Referencias_Insert", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDCliente", IDCliente);
                                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoReferencia", referencia.fcNombreCompletoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoReferencia", referencia.fcLugarTrabajoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiTiempoConocerReferencia", referencia.fiTiempoConocerReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoReferencia", referencia.fcTelefonoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDParentescoReferencia", referencia.fiIDParentescoReferencia);
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
                                                return "-1";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region REFERENCIAS PERSONALES QUE SE VAN A ELIMINAR/INACTIVAR
                        List<ClientesReferenciasViewModel> referenciasInactivar = new List<ClientesReferenciasViewModel>();

                        foreach (ClientesReferenciasViewModel item in referenciasExistentes)
                        {
                            //si la nueva lista de referencias no contiene una referencia de la lista vieja de referencias quiere decir que esa referencia se debe eliminar
                            if (!ClientesReferencias.Select(x => x.fiIDReferencia).ToList().Contains(item.fiIDReferencia))
                            {
                                referenciasInactivar.Add(new ClientesReferenciasViewModel()
                                {
                                    fiIDReferencia = item.fiIDReferencia,
                                });
                            }
                        };
                        if (referenciasInactivar.Count > 0)
                        {
                            foreach (ClientesReferenciasViewModel referencia in referenciasInactivar)
                            {
                                using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Referencias_Eliminar", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", referencia.fiIDReferencia);
                                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                    sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
                                    using (reader = sqlComando.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            MensajeError = (string)reader["MensajeError"];
                                            if (MensajeError.StartsWith("-1"))
                                                return "-1";
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        #region REFERENCIAS PERSONALES QUE SE VAN A EDITAR
                        List<ClientesReferenciasViewModel> referenciasEditar = new List<ClientesReferenciasViewModel>();
                        foreach (ClientesReferenciasViewModel item in referenciasExistentes)
                        {
                            //si la nueva lista de referencias contiene una referencia de la vieja, actualizar su informacion
                            if (ClientesReferencias.Select(x => x.fiIDReferencia).ToList().Contains(item.fiIDReferencia))
                            {
                                referenciasEditar.Add(new ClientesReferenciasViewModel()
                                {
                                    fiIDReferencia = item.fiIDReferencia,
                                    fcNombreCompletoReferencia = item.fcNombreCompletoReferencia,
                                    fcLugarTrabajoReferencia = item.fcLugarTrabajoReferencia,
                                    fiTiempoConocerReferencia = item.fiTiempoConocerReferencia,
                                    fcTelefonoReferencia = item.fcTelefonoReferencia,
                                    fiIDParentescoReferencia = item.fiIDParentescoReferencia,
                                });
                            }
                        }
                        if (referenciasEditar.Count > 0)
                        {
                            foreach (ClientesReferenciasViewModel referencia in referenciasEditar)
                            {
                                using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Referencias_Update", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", referencia.fiIDReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoReferencia", referencia.fcNombreCompletoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoReferencia", referencia.fcLugarTrabajoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiTiempoConocerReferencia", referencia.fiTiempoConocerReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoReferencia", referencia.fcTelefonoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDParentescoReferencia", referencia.fiIDParentescoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", IDUSR);
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
                                                return "-1";
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        MensajeError = "-1";
                        tran.Rollback();
                        ex.Message.ToString();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MensajeError = "-1";
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
        return MensajeError;
    }

    public string ActualizarDocumentacion()
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        string MensajeError = string.Empty;
        string resultadoProceso = String.Empty;
        try
        {
            DateTime fechaActual = Helpers.DatetimeNow();
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));


            int DocumentacionCliente = 1;
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
                    String[] NombresGenerador = Funciones.MultiNombres.GenerarNombreCredDocumento(DocumentacionCliente, IDSOL, TipoDocumento, CantidadDocumentos);

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
                return "-1";

            if (SolicitudesDocumentos.Count <= 0)
                return "-1";

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            using (sqlConexion = new SqlConnection(sqlConnectionString))
            {
                sqlConexion.Open();
                using (SqlTransaction tran = sqlConexion.BeginTransaction())
                {
                    int contadorErrores = 0;
                    foreach (SolicitudesDocumentosViewModel documento in SolicitudesDocumentos)
                    {
                        SqlCommand sqlComando;
                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Documentos_Insert", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@fcNombreArchivo", documento.fcNombreArchivo);
                            sqlComando.Parameters.AddWithValue("@fcTipoArchivo", ".png");
                            sqlComando.Parameters.AddWithValue("@fcRutaArchivo", documento.fcRutaArchivo);
                            sqlComando.Parameters.AddWithValue("@fcURL", documento.URLArchivo);
                            sqlComando.Parameters.AddWithValue("@fiTipoDocumento", documento.fiTipoDocumento);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
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
                    }
                    /* Mover documentos al directorio de la solicitud */
                    if (!FileUploader.GuardarSolicitudDocumentos(IDSOL, SolicitudesDocumentos))
                        contadorErrores++;

                    //verificar resultado del proceso
                    if (contadorErrores == 0)
                        tran.Commit();
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
                reader.Close();
        }
        return MensajeError;
    }

    [WebMethod]
    public static List<CondicionesViewModel_prueba> DetallesCondicion()
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        List<CondicionesViewModel_prueba> condicionesSolicitud = new List<CondicionesViewModel_prueba>();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_SolicitudCondiciones_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                condicionesSolicitud.Add(new CondicionesViewModel_prueba()
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
                reader.Close();
        }
        return condicionesSolicitud;
    }

    [WebMethod]
    public static SolicitudIngresarDDLViewModel CargarListas()
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        SolicitudIngresarDDLViewModel ddls = new SolicitudIngresarDDLViewModel();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
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
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredCatalogo_TipoDocumento_RegistrarSolicitudListar", sqlConexion);
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
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
    public static string obtenerUrlEncriptado(int IDCliente)
    {
        string lcURL = HttpContext.Current.Request.Url.ToString();
        Uri lURLDesencriptado = DesencriptarURL(lcURL);
        int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
        int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
        string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string parametrosEncriptados = DSC.Encriptar("usr=" + IDUSR + "&IDSOL=" + IDSOL + "&cltID=" + IDCliente + "&IDApp=" + pcIDApp);        
        return parametrosEncriptados;
    }

    [WebMethod]
    public static List<MunicipiosViewModel> CargarMunicipios(int CODDepto)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        List<MunicipiosViewModel> municipios = new List<MunicipiosViewModel>();
        try
        {
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
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
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        List<CiudadesViewModel> ciudades = new List<CiudadesViewModel>();
        try
        {
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
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
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        List<BarriosColoniasViewModel> Barrios = new List<BarriosColoniasViewModel>();
        try
        {
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
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
                Barrios.Add(new BarriosColoniasViewModel()
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
        return Barrios;
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

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
            return default(T);
        else
            return (T)obj;
    }
}

public class CondicionesViewModel_prueba
{
    public int IDSolicitudCondicion { get; set; }
    public int IDCondicion { get; set; }
    public int IDSolicitud { get; set; }
    public string TipoCondicion { get; set; }
    public string DescripcionCondicion { get; set; }
    public string ComentarioAdicional { get; set; }
    public bool Estado { get; set; }
}