using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_RegistradaDetalles : System.Web.UI.Page
{
    private String pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
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
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                else
                    lcParametros = String.Empty;

                if (lcParametros != String.Empty)
                {
                    pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    CargarInformacionSolicitud(Convert.ToInt32(pcIDUsuario), IDSOL, pcIDApp);
                }
                else
                {
                    string lcScript = "window.open('SolicitudesCredito_Ingresadas.aspx?" + pcEncriptado + "','_self')";
                    Response.Write("<script>");
                    Response.Write(lcScript);
                    Response.Write("</script>");
                }
            }
            catch
            {
                string lcScript = "window.open('SolicitudesCredito_Ingresadas.aspx?" + pcEncriptado + "','_self')";
                Response.Write("<script>");
                Response.Write(lcScript);
                Response.Write("</script>");
            }
        }
    }

    public bool CargarInformacionSolicitud(int IDUsuario, int IDSolicitud, string pcIDApp)
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
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUsuario);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();

            int IDPRODUCTO = 0;
            while (reader.Read())
            {
                solicitudes = new BandejaSolicitudesViewModel()
                {
                    fiIDAnalista = (int)reader["fiIDAnalista"],
                    fcNombreCortoAnalista = (string)reader["fcNombreCortoAnalista"]
                };
                IDPRODUCTO = (int)reader["fiIDTipoProducto"];
                lblArraigoLaboral.Text = (string)reader["fcClienteArraigoLaboral"].ToString();
            }
            string NombreLogo = IDPRODUCTO == 101 ? "iconoRecibirDinero48.png" : IDPRODUCTO == 201 ? "iconoMoto48.png" : IDPRODUCTO == 202 ? "iconoAuto48.png" : IDPRODUCTO == 301 ? "iconoConsumo48.png" : "iconoConsumo48.png";
            LogoPrestamo.ImageUrl = "http://172.20.3.140/Imagenes/" + NombreLogo;           
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
        string sqlConnectionString;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        SolicitudAnalisisViewModel ObjSolicitud = new SolicitudAnalisisViewModel();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            #region SOLICITUD MAESTRO
            BandejaSolicitudesViewModel SolicitudMaestro = new BandejaSolicitudesViewModel();
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
                    PasoFinalFin = (DateTime)reader["fdPasoFinalFin"]
                };
            }
            sqlComando.Dispose();
            if (reader != null)
                reader.Close();

            ObjSolicitud.solicitud = SolicitudMaestro;
            #endregion

            #region DOCUMENTOS DE LA SOLICITUD
            List<SolicitudesDocumentosViewModel> ListadoDocumentos = new List<SolicitudesDocumentosViewModel>();
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ObtenerSolicitudDocumentos", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                ListadoDocumentos.Add(new SolicitudesDocumentosViewModel()
                {
                    fiIDSolicitud = (int)reader["fiIDSolicitud"],
                    fiIDSolicitudDocs = (int)reader["fiIDSolicitudDocs"],
                    fcNombreArchivo = (string)reader["fcNombreArchivo"],
                    fcTipoArchivo = (string)reader["fcTipoArchivo"],
                    fcRutaArchivo = (string)reader["fcRutaArchivo"],
                    URLArchivo = (string)reader["fcURL"],
                    fcArchivoActivo = (byte)reader["fiArchivoActivo"],
                    fiTipoDocumento = (int)reader["fiTipoDocumento"],
                    DescripcionTipoDocumento = (string )reader["fcDescripcionTipoDocumento"]
                });
            }
            sqlComando.Dispose();
            if (reader != null)
                reader.Close();
            ObjSolicitud.documentos = ListadoDocumentos;
            #endregion

            #region INFORMACION DEL CLIENTE
            ClientesViewModel objCliente = new ClientesViewModel();

            #region CLIENTES MASTER
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", ObjSolicitud.solicitud.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"],
                    // proceso de campo
                    fcLatitud = (string)reader["fcLatitud"],
                    fcLongitud = (string)reader["fcLongitud"],
                    fiIDGestorValidador = (int)reader["fiIDGestorValidador"],
                    fcGestorValidadorTrabajo = (string)reader["fcGestorValidadorTrabajo"],
                    fiIDInvestigacionDeCampo = (byte)reader["fiIDInvestigacionDeCampo"],
                    fcGestionTrabajo = (string)reader["fcGestionTrabajo"],
                    IDTipoResultado = (byte)reader["fiTipodeResultado"],
                    fcResultadodeCampo = (string)reader["fcResultadodeCampo"],
                    fdFechaValidacion = (DateTime)reader["fdFechaValidacion"],
                    fcObservacionesCampo = (string)reader["fcObservacionesCampo"],
                    fiIDEstadoDeGestion = (byte)reader["fiIDEstadoDeGestion"],
                    fiEstadoLaboral = (byte)reader["fiEstadoLaboral"]
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
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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

            #region CLIENTE INFORMACION DE DOMICILIO
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionDomiciliar_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"],
                    // proceso de campo
                    fcLatitud = (string)reader["fcLatitud"],
                    fcLongitud = (string)reader["fcLongitud"],
                    fiIDGestorValidador = (int)reader["fiIDGestorValidador"],
                    fcGestorValidadorDomicilio = (string)reader["fcGestorValidadorDomicilio"],
                    fiIDInvestigacionDeCampo = (byte)reader["fiIDInvestigacionDeCampo"],
                    fcGestionDomicilio = (string)reader["fcGestionDomicilio"],
                    IDTipoResultado = (byte)reader["fiTipodeResultado"],
                    fcResultadodeCampo = (string)reader["fcResultadodeCampo"],
                    fdFechaValidacion = (DateTime)reader["fdFechaValidacion"],
                    fcObservacionesCampo = (string)reader["fcObservacionesCampo"],
                    fiIDEstadoDeGestion = (byte)reader["fiIDEstadoDeGestion"],
                    fiEstadoDomicilio = (byte)reader["fiEstadoDomicilio"]
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
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
    public static BandejaSolicitudesViewModel CargarEstadoSolicitud()
    {
        BandejaSolicitudesViewModel objEstadoSolicitud = new BandejaSolicitudesViewModel();
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        string sqlConnectionString;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            #region OBTENER INFORMACION DEL ESTADO DE LA SOLICITUD
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_SolicitudEstadoProcesamiento", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                objEstadoSolicitud = new BandejaSolicitudesViewModel()
                {
                    fiIDSolicitud = (int)reader["fiIDSolicitud"],
                    fiEstadoSolicitud = (byte)reader["fiEstadoSolicitud"],
                    fiSolicitudActiva = (byte)reader["fiSolicitudActiva"],
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
                    fcTiempoTotalTranscurrido = (string)reader["fcTiempoTotalTranscurrido"],
                };
            }
            sqlComando.Dispose();
            if (reader != null)
                reader.Close();
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
        return objEstadoSolicitud;
    }

    [WebMethod]
    public static CalculoPrestamoViewModel CalculoPrestamo(decimal MontoFinanciar, decimal PlazoFinanciar, decimal ValorPrima)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        CalculoPrestamoViewModel objCalculo = null;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            DateTime fechaActual = DateTime.Now;
            string MensajeError = String.Empty;
            int IDPRODUCTO = 0;

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
                IDPRODUCTO = (int)reader["fiIDTipoProducto"];

            sqlComando.Dispose();
            if (reader != null)
                reader.Close();

            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_CalculoPrestamo", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@piIDProducto", IDPRODUCTO);
            sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", MontoFinanciar);
            sqlComando.Parameters.AddWithValue("@liPlazo", PlazoFinanciar);
            sqlComando.Parameters.AddWithValue("@pnValorPrima", ValorPrima);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            reader = sqlComando.ExecuteReader();

            while (reader.Read())
            {
                objCalculo = new CalculoPrestamoViewModel()
                {
                    SegurodeDeuda = (decimal)reader["fnSegurodeDeuda"],
                    SegurodeVehiculo = (decimal)reader["fnSegurodeVehiculo"],
                    GastosdeCierre = (decimal)reader["fnGastosdeCierre"],
                    ValoraFinanciar = (decimal)reader["fnValoraFinanciar"],
                    CuotaQuincenal = (decimal)reader["fnCuotaQuincenal"],
                    CuotaMensual = (decimal)reader["fnCuotaMensual"],
                    CuotaServicioGPS = (decimal)reader["fnCuotaServicioGPS"],
                    CuotaSegurodeVehiculo = (decimal)reader["fnCuotaSegurodeVehiculo"],
                    CuotaMensualNeta = (decimal)reader["fnCuotaMensualNeta"],
                    TotalSeguroVehiculo = (decimal)reader["fnTotalSeguroVehiculo"],
                    TipoCuota = (string)reader["fcTipodeCuota"]
                };
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
        return objCalculo;
    }

    [WebMethod]
    public static string ObtenerUrlEncriptado()
    {
        string lcURL = HttpContext.Current.Request.Url.ToString();
        Uri lURLDesencriptado = DesencriptarURL(lcURL);
        int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
        string pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
        string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string parametrosEncriptados = DSC.Encriptar("usr=" + pcIDUsuario + "&ID=" + pcID + "&IDApp=" + pcIDApp);
        return parametrosEncriptados;
    }

    [WebMethod]
    public static PrecalificadoViewModel GetPrestamosSugeridos(decimal ingresos, decimal obligaciones, string codigoProducto)
    {
        PrecalificadoViewModel objPrecalificado = new PrecalificadoViewModel();
        List<cotizadorProductosViewModel> listaCotizadorProductos = new List<cotizadorProductosViewModel>();
        string lcURL = HttpContext.Current.Request.Url.ToString();
        Uri lURLDesencriptado = DesencriptarURL(lcURL);
        string identidad = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
        int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString());

        string connectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = WebUser; Password = WebUser123*;Max Pool Size=200;MultipleActiveResultSets=true";
        SqlConnection conn = null;
        SqlDataReader reader = null;
        try
        {
            conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("EXEC CoreFinanciero.dbo.sp_CotizadorProductos @piIDUsuario, @piIDProducto, @pcIdentidad, @piConObligaciones, @pnIngresosBrutos, @pnIngresosDisponibles", conn);
            cmd.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
            cmd.Parameters.AddWithValue("@piIDProducto", codigoProducto);
            cmd.Parameters.AddWithValue("@pcIdentidad", identidad);
            cmd.Parameters.AddWithValue("@piConObligaciones", obligaciones == 0 ? "0" : "1");
            cmd.Parameters.AddWithValue("@pnIngresosBrutos", ingresos);
            cmd.Parameters.AddWithValue("@pnIngresosDisponibles", ingresos - obligaciones);
            conn.Open();
            reader = cmd.ExecuteReader();
            int IDContador = 1;
            while (reader.Read())
            {
                listaCotizadorProductos.Add(new cotizadorProductosViewModel()
                {
                    IDCotizacion = IDContador,
                    IDProducto = (int)reader["fiIDProducto"],
                    ProductoDescripcion = reader["fcProducto"].ToString(),
                    fnMontoOfertado = decimal.Parse(reader["fnMontoOfertado"].ToString()),
                    fiPlazo = int.Parse(reader["fiPlazo"].ToString()),
                    fnCuotaQuincenal = decimal.Parse(reader["fnCuotaQuincenal"].ToString()),
                    TipoCuota = (string)reader["fcTipodeCuota"]
                });
                IDContador += 1;
            }
            objPrecalificado.cotizadorProductos = listaCotizadorProductos;
            cmd.Dispose();
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
            {
                reader.Close();
            }
        }
        return objPrecalificado;
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