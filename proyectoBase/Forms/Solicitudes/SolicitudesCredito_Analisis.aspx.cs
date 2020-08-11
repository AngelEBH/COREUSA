using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Analisis : System.Web.UI.Page
{
    private String pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";

    protected void Page_Load(object sender, EventArgs e)
    {
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
                bool AccesoAlAnalisis = ValidarAnalista(Convert.ToInt32(pcIDUsuario), IDSOL, pcIDApp);

                //if (AccesoAlAnalisis == false)
                if (1 == 2)
                {
                    string lcScript = "window.open('SolicitudesCredito_Bandeja.aspx?" + pcEncriptado + "','_self')";
                    Response.Write("<script>");
                    Response.Write(lcScript);
                    Response.Write("</script>");
                }
                //CargarSolicitud(IDSOL);
            }
            else
            {
                string lcScript = "window.open('SolicitudesCredito_Bandeja.aspx?" + pcEncriptado + "','_self')";
                Response.Write("<script>");
                Response.Write(lcScript);
                Response.Write("</script>");
            }
        }
    }

    public bool ValidarAnalista(int IDUsuario, int IDSolicitud, string pcIDApp)
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

                //Llenar ficha de resumen
                //lblResumenCliente.Text = ((string)reader["fcPrimerNombreCliente"] + " " + (string)reader["fcSegundoNombreCliente"] + " " + (string)reader["fcPrimerApellidoCliente"] + " " + (string)reader["fcSegundoApellidoCliente"]).Replace("  ","");
                //lblResumenEdad.Text = ((short)reader["fiEdadCliente"]).ToString() + " Años";
                //lblResumenTrabajo.Text = (string)"N/A";
                //lblResumenPuesto.Text = (string)"N/A";
                //lblResumenCapacidadPagoMensual.Text = (string)"N/A";
                //lblResumenCapacidadPagoQuincenal.Text = (string)"N/A";

                //lblResumenDeptoResidencia.Text = (string)"N/A";
                //lblResumenMuniResidencia.Text = (string)"N/A";
                //lblResumenColResidencia.Text = (string)"N/A";
                //lblResumenTipoVivienda.Text = (string)"N/A";
                //lblResumenTiempoResidir.Text = (string)"N/A";

                //lblResumenValorGarantia.Text = (string)"N/A";
                //lblResumenValorPrima.Text = (string)"N/A";
                //lblResumenValorFinanciar.Text = (string)"N/A";
                //lblResumenCuota.Text = (string)"N/A";

                //lblResumenTipoEmpresa.Text = (string)"N/A";
                //lblResumenTipoPerfil.Text = (string)"N/A";
                //lblResumenTipoEmpleo.Text = (string)"N/A";
                //lblResumenBuroActual.Text = (string)"N/A";

                //lblResumenVendedor.Text = (string)"N/A";
                //lblResumenAnalista.Text = (string)"N/A";
                //lblResumenGestor.Text = (string)"N/A";

            }
            string NombreLogo = IDPRODUCTO == 101 ? "iconoRecibirDinero48.png" : IDPRODUCTO == 201 ? "iconoMoto48.png" : IDPRODUCTO == 202 ? "iconoAuto48.png" : IDPRODUCTO == 301 ? "iconoConsumo48.png" : "iconoConsumo48.png";
            LogoPrestamo.ImageUrl = "http://172.20.3.140/Imagenes/" + NombreLogo;

            if (solicitudes.fiIDAnalista == IDUsuario)
                resultado = true;
            else if (solicitudes.fiIDAnalista == 0)
            {
                /* SI LA SOLICITUD NO ESTA SIENDO ANALIZADA, ASIGNAR SOLICITUD AL USUARIO */
                SqlCommand cmd = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_Analizar", sqlConexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitud);
                cmd.Parameters.AddWithValue("@fiIDAnalista", IDUsuario);
                cmd.Parameters.AddWithValue("@piIDSesion", 1);
                cmd.Parameters.AddWithValue("@piIDApp", pcIDApp);
                cmd.Parameters.AddWithValue("@piIDUsuario", IDUsuario);
                SqlDataReader readerAnalisis = cmd.ExecuteReader();
                
                string MensajeErrorAnalisis = String.Empty;
                while (readerAnalisis.Read())
                    MensajeErrorAnalisis = (string)readerAnalisis["MensajeError"];

                resultado = true;
                if (MensajeErrorAnalisis.StartsWith("-1"))
                    resultado = false;
            }
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
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);

            #region OBTENER TODA LA INFORMACION DE LA SOLICITUD            

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
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
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
                    DescripcionTipoDocumento = (string)reader["fcDescripcionTipoDocumento"]
                });
            }
            sqlComando.Dispose();
            if (reader != null)
                reader.Close();

            ObjSolicitud.documentos = ListadoDocumentos;
            #endregion

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
                    fcTelefonoCliente = (string)reader["fcTelefonoPrimarioCliente"],
                    //nacionalidad del cliente
                    fiNacionalidadCliente = (int)reader["fiNacionalidadCliente"],
                    fcDescripcionNacionalidad = (string)reader["fcDescripcionNacionalidad"],
                    fbNacionalidadActivo = (bool)reader["fbNacionalidadActivo"],
                    fdFechaNacimientoCliente = (DateTime)reader["fdFechaNacimientoCliente"],
                    fcCorreoElectronicoCliente = (string)reader["fcCorreoElectronicoCliente"],
                    fcProfesionOficioCliente = (string)reader["fcProfesionOficioCliente"],
                    fcSexoCliente = (string)reader["fcSexoCliente"],
                    //estado civil del cliente
                    fiIDEstadoCivil = (int)reader["fiIDEstadoCivil"],
                    fcDescripcionEstadoCivil = (string)reader["fcDescripcionEstadoCivil"],
                    fbEstadoCivilActivo = (bool)reader["fbEstadoCivilActivo"],
                    //informacion de la vivienda del cliente
                    fiIDVivienda = (int)reader["fiIDVivienda"],
                    fcDescripcionVivienda = (string)reader["fcDescripcionVivienda"],
                    fbViviendaActivo = (bool)reader["fbViviendaActivo"],
                    fiTiempoResidir = (short)reader["fiTiempoResidir"],
                    fbClienteActivo = (bool)reader["fbClienteActivo"],
                    fcRazonInactivo = (string)reader["fcRazonInactivo"],
                    //nombres del cliente
                    fcPrimerNombreCliente = (string)reader["fcPrimerNombreCliente"],
                    fcSegundoNombreCliente = (string)reader["fcSegundoNombreCliente"],
                    fcPrimerApellidoCliente = (string)reader["fcPrimerApellidoCliente"],
                    fcSegundoApellidoCliente = (string)reader["fcSegundoApellidoCliente"],
                    //data de auditoria
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

            string MensajeError = String.Empty;
            while (reader.Read())
            {
                objCliente.ClientesInformacionLaboral = new ClientesInformacionLaboralViewModel()
                {
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fiIDInformacionLaboral = (int)reader["fiIDInformacionLaboral"],
                    fcNombreTrabajo = (string)reader["fcNombreTrabajo"],
                    fiIngresosMensuales = (decimal)reader["fnIngresosMensuales"],
                    //nacionalidad del cliente
                    fcPuestoAsignado = (string)reader["fcPuestoAsignado"],
                    fcFechaIngreso = (DateTime)reader["fdFechaIngreso"],
                    fdTelefonoEmpresa = (string)reader["fcTelefonoEmpresa"],
                    fcExtensionRecursosHumanos = (string)reader["fcExtensionRecursosHumanos"],
                    fcExtensionCliente = (string)reader["fcExtensionCliente"],
                    fcDireccionDetalladaEmpresa = (string)reader["fcDireccionDetalladaEmpresa"],
                    fcReferenciasDireccionDetallada = (string)reader["fcReferenciasDireccionDetalladaEmpresa"],
                    fcFuenteOtrosIngresos = (string)reader["fcFuenteOtrosIngresos"],
                    fiValorOtrosIngresosMensuales = (decimal)reader["fnValorOtrosIngresosMensuales"],
                    //barrio de la empresa
                    fiIDBarrioColonia = (int)reader["fiIDBarrioColonia"],
                    fcNombreBarrioColonia = (string)reader["fcBarrio"],
                    //ciudad de la empresa
                    fiIDCiudad = (int)reader["fiIDCiudad"],
                    fcNombreCiudad = (string)reader["fcPoblado"],
                    //municipio de la empresa
                    fiIDMunicipio = (int)reader["fiIDMunicipio"],
                    fcNombreMunicipio = (string)reader["fcMunicipio"],
                    //departamento de la empresa
                    fiIDDepto = (int)reader["fiIDDepartamento"],
                    fcNombreDepto = (string)reader["fcDepartamento"],
                    //data de auditoria
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
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            MensajeError = String.Empty;
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
                    //data de auditoria
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

            #region CLIENTE INFORMACION DEL DOMICILIO
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionDomiciliar_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            MensajeError = String.Empty;
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
                    //barrio del cliente
                    fiIDBarrioColonia = (short)reader["fiCodBarrio"],
                    fcNombreBarrioColonia = (string)reader["fcBarrio"],
                    //ciudad del cliente
                    fiIDCiudad = (short)reader["fiCodPoblado"],
                    fcNombreCiudad = (string)reader["fcPoblado"],
                    //municipio del cliente
                    fiIDMunicipio = (short)reader["fiCodMunicipio"],
                    fcNombreMunicipio = (string)reader["fcMunicipio"],
                    //departamento del cliente
                    fiIDDepto = (short)reader["fiCodDepartamento"],
                    fcNombreDepto = (string)reader["fcDepartamento"],
                    //data de auditoria
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
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            MensajeError = String.Empty;
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

            #region OBTENER AVALES DEL CLIENTE
            objCliente.Avales = new List<ClienteAvalesViewModel>();
            sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_Maestro_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
            sqlComando.Parameters.AddWithValue("@fiIDAval", 0);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            reader = sqlComando.ExecuteReader();

            MensajeError = String.Empty;
            while (reader.Read())
            {
                objCliente.Avales.Add(new ClienteAvalesViewModel()
                {
                    fiIDAval = (int)reader["fiIDAval"],
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fiIDSolicitud = (int)reader["fiIDSolicitud"],
                    fiTipoAval = (int)reader["fiTipoAval"],
                    fcIdentidadAval = (string)reader["fcIdentidadAval"],
                    RTNAval = (string)reader["fcRTNAval"],
                    fcPrimerNombreAval = (string)reader["fcPrimerNombreAval"],
                    fcSegundoNombreAval = (string)reader["fcSegundoNombreAval"],
                    fcPrimerApellidoAval = (string)reader["fcPrimerApellidoAval"],
                    fcSegundoApellidoAval = (string)reader["fcSegundoApellidoAval"],
                    fcTelefonoAval = (string)reader["fcTelefonoPrimarioAval"],
                    fdFechaNacimientoAval = (DateTime)reader["fdFechaNacimientoAval"],
                    fcCorreoElectronicoAval = (string)reader["fcCorreoElectronicoAval"],
                    fcProfesionOficioAval = (string)reader["fcProfesionOficioAval"],
                    fcSexoAval = (string)reader["fcSexoAval"],
                    fbAvalActivo = (bool)reader["fbAvalActivo"],
                    fcRazonInactivo = (string)reader["fcRazonInactivo"],
                    fcNombreTrabajo = (string)reader["fcNombreTrabajo"],
                    fdTelefonoEmpresa = (string)reader["fcTelefonoEmpresa"],
                    fcExtensionRecursosHumanos = (string)reader["fcExtensionRecursosHumanos"],
                    fcExtensionAval = (string)reader["fcExtensionAval"],
                    fiIngresosMensuales = (decimal)reader["fiIngresosMensuales"],
                    fcPuestoAsignado = (string)reader["fcPuestoAsignado"],
                    fcFechaIngreso = (DateTime)reader["fdFechaIngresoAval"],
                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]

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
    public static bool ValidacionesAnalisis(string validacion, string observacion)
    {
        bool resultadoProceso = false;
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            DateTime fechaActual = DateTime.Now;
            string MensajeError = String.Empty;

            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ValidacionesAnalisis", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@fcValidacion", validacion);
            sqlComando.Parameters.AddWithValue("@fcComentario", observacion);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
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
    public static bool EnviarACampo(string fcObservacionesDeCredito)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        bool resultadoProceso = false;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            DateTime fechaActual = DateTime.Now;
            string MensajeError = String.Empty;
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_EnviarACampo", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@fcObservacionesDeCredito", fcObservacionesDeCredito);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
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
    public static int SolicitudResolucion(SolicitudesBitacoraViewModel objBitacora, SolicitudesMasterViewModel objSolicitud)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        int resultadoProceso = 0;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            DateTime fechaActual = DateTime.Now;
            string MensajeError = String.Empty;

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolictud_Resolucion", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@fiEstadoSolicitud", objSolicitud.fiEstadoSolicitud);

            if (objSolicitud.fiMontoFinalSugerido != null)
                sqlComando.Parameters.AddWithValue("@fnMontoFinalSugerido", (decimal)objSolicitud.fiMontoFinalSugerido);
            else
                sqlComando.Parameters.AddWithValue("@fnMontoFinalSugerido", DBNull.Value);

            if (objSolicitud.fiMontoFinalSugerido != null)
                sqlComando.Parameters.AddWithValue("@fnMontoFinalFinanciar", (decimal)objSolicitud.fiMontoFinalFinanciar);
            else
                sqlComando.Parameters.AddWithValue("@fnMontoFinalFinanciar", DBNull.Value);

            if (objSolicitud.fiPlazoFinalAprobado != null)
                sqlComando.Parameters.AddWithValue("@fiPlazoFinalAprobado", objSolicitud.fiPlazoFinalAprobado);
            else
                sqlComando.Parameters.AddWithValue("@fiPlazoFinalAprobado", DBNull.Value);

            sqlComando.Parameters.AddWithValue("@fcComentarioResolucion", objBitacora.fcComentarioResolucion);
            sqlComando.Parameters.AddWithValue("@fiIDUsuarioPasoFinal", IDUSR);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();

            while (reader.Read())
                MensajeError = (string)reader["MensajeError"];

            if (!MensajeError.StartsWith("-1"))
                resultadoProceso = Convert.ToInt32(MensajeError);
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
    public static BandejaSolicitudesViewModel CargarEstadoSolicitud()
    {
        BandejaSolicitudesViewModel objEstadoSolicitud = new BandejaSolicitudesViewModel();
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_SolicitudEstadoProcesamiento", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
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

                    // informacion de perfil
                    fcTipoEmpresa = (string)reader["fcTipoEmpresa"],
                    fcTipoPerfil = (string)reader["fcTipoPerfil"],
                    fcTipoEmpleado = (string)reader["fcTipoEmpleado"],
                    fcBuroActual = (string)reader["fcBuroActual"]
                };
            }
            sqlComando.Dispose();
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
    public static string ObtenerUrlEncriptado()
    {
        string lcURL = HttpContext.Current.Request.Url.ToString();
        Uri lURLDesencriptado = DesencriptarURL(lcURL);
        int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
        string pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
        string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string parametrosEncriptados = DSC.Encriptar("usr=" + IDUSR + "&ID=" + pcID + "&IDApp=" + pcIDApp);
        return parametrosEncriptados;
    }

    [WebMethod]
    public static string EncriptarParametroDetallesAval(int parametro)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcURL = HttpContext.Current.Request.Url.ToString();
        Uri lURLDesencriptado = DesencriptarURL(lcURL);
        int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
        int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
        string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        string parametrosEncriptados = DSC.Encriptar("usr=" + IDUSR + "&IDAval=" + parametro + "&IDSOL=" + IDSOL + "&IDApp=" + pcIDApp);
        return parametrosEncriptados;
    }

    [WebMethod]
    public static bool ActualizarIngresosCliente(decimal sueldoBaseReal, decimal bonosComisionesReal)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        bool resultadoProceso = false;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            DateTime fechaActual = DateTime.Now;
            string MensajeError = String.Empty;

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ActualizarIngresosClienteSolicitud", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@fnSueldoBaseReal", sueldoBaseReal);
            sqlComando.Parameters.AddWithValue("@fnBonosComisionesReal", bonosComisionesReal);
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
    public static bool GuardarInformacionAnalisis(string tipoEmpresa, string tipoPerfil, string tipoEmpleo, string buroActual)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        bool resultadoProceso = false;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            DateTime fechaActual = DateTime.Now;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();

            string sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            
            SqlCommand sqlComando = new SqlCommand("dbo.sp_CredSolicitud_GuardarInformaciondePerfil", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@fcTipoEmpresa", tipoEmpresa);
            sqlComando.Parameters.AddWithValue("@fcTipoPerfil", tipoPerfil);
            sqlComando.Parameters.AddWithValue("@fcTipoEmpleado", tipoEmpleo);
            sqlComando.Parameters.AddWithValue("@fcBuroActual", buroActual);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();            
            reader = sqlComando.ExecuteReader();

            while (reader.Read())
                if (!reader["MensajeError"].ToString().StartsWith("-1"))
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

    [WebMethod] // este
    public static CalculoPrestamoViewModel CalculoPrestamo(decimal ValorPrestamo, decimal ValorPrima, decimal CantidadPlazos)
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
            sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", ValorPrestamo);
            sqlComando.Parameters.AddWithValue("@liPlazo", CantidadPlazos);
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
    public static ClienteAvalesViewModel DetallesAval(int IDAval)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        ClienteAvalesViewModel AvalInfo = null;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string MensajeError = String.Empty;

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredAval_InformacionPrincipal", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDAval", IDAval);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            while (reader.Read())
            {
                AvalInfo = new ClienteAvalesViewModel()
                {
                    fiIDAval = (int)reader["fiIDAval"],
                    fiIDCliente = (int)reader["fiIDCliente"],
                    fcIdentidadAval = (string)reader["fcIdentidadAval"],
                    RTNAval = (string)reader["fcRTNAval"],
                    fcPrimerNombreAval = (string)reader["fcPrimerNombreAval"],
                    fcSegundoNombreAval = (string)reader["fcSegundoNombreAval"],
                    fcPrimerApellidoAval = (string)reader["fcPrimerApellidoAval"],
                    fcSegundoApellidoAval = (string)reader["fcSegundoApellidoAval"],
                    fcTelefonoAval = (string)reader["fcTelefonoPrimarioAval"],
                    fdFechaNacimientoAval = (DateTime)reader["fdFechaNacimientoAval"],
                    fcCorreoElectronicoAval = (string)reader["fcCorreoElectronicoAval"],
                    fcProfesionOficioAval = (string)reader["fcProfesionOficioAval"],
                    fcSexoAval = (string)reader["fcSexoAval"],
                    fbAvalActivo = (bool)reader["fbAvalActivo"],
                    fcRazonInactivo = (string)reader["fcRazonInactivo"],
                    fiTipoAval = (int)reader["fiTipoAval"],
                    fcNombreTrabajo = (string)reader["fcNombreTrabajo"],
                    fdTelefonoEmpresa = (string)reader["fcTelefonoEmpresa"],
                    fcExtensionRecursosHumanos = (string)reader["fcExtensionRecursosHumanos"],
                    fcExtensionAval = (string)reader["fcExtensionAval"],
                    fiIngresosMensuales = (decimal)reader["fiIngresosMensuales"],
                    fcPuestoAsignado = (string)reader["fcPuestoAsignado"],
                    fcFechaIngreso = (DateTime)reader["fdFechaIngresoAval"],
                    fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                    fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                    fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                    fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"],
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
        return AvalInfo;
    }

    [WebMethod]
    public static PrecalificadoViewModel GetPrestamosSugeridos(decimal ValorProducto, decimal ValorPrima)
    {
        PrecalificadoViewModel objPrecalificado = new PrecalificadoViewModel();
        List<cotizadorProductosViewModel> listaCotizadorProductos = new List<cotizadorProductosViewModel>();
        string connectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = WebUser; Password = WebUser123*;Max Pool Size=200;MultipleActiveResultSets=true";
        SqlConnection conn = null;
        SqlDataReader reader = null;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            string identidad = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
            int IDUSR = int.Parse(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr").ToString());

            conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("dbo.sp_CredCotizador_ConPrima", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@pcIdentidad", identidad);
            cmd.Parameters.AddWithValue("@pnValorProducto", ValorProducto);
            cmd.Parameters.AddWithValue("@pnPrima", ValorPrima);
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
        return objPrecalificado;
    }

    [WebMethod]
    public static bool ActualizarPlazoMontoFinanciar(decimal ValorGlobal, decimal ValorPrima, int CantidadPlazos)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        bool resultadoProceso = false;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            DateTime fechaActual = DateTime.Now;
            string MensajeError = String.Empty;

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ActualizarPlazoMontoFinanciar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@fnValorGlobal", ValorGlobal);
            sqlComando.Parameters.AddWithValue("@fnValorPrima", ValorPrima);
            sqlComando.Parameters.AddWithValue("@fiCantidadPlazos", CantidadPlazos);
            sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", pcIDUsuario);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
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
    public static bool ComentarioReferenciaPersonal(int IDReferencia, string comentario)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        bool resultadoProceso = false;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            DateTime fechaActual = DateTime.Now;
            string MensajeError = String.Empty;

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Referencias_Comentario", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDReferencia", IDReferencia);
            sqlComando.Parameters.AddWithValue("@fcComentarioDeptoCredito", comentario);
            sqlComando.Parameters.AddWithValue("@fiAnalistaComentario", IDUSR);
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
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
    public static bool EliminarReferenciaPersonal(int IDReferencia)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        bool resultadoProceso = false;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int pcIDSesion = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("dbo.sp_CREDCliente_Referencias_Eliminar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDReferencia", IDReferencia);
            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();

            while (reader.Read())
                if (!reader["MensajeError"].ToString().StartsWith("-1"))
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

    [WebMethod] // este
    public static List<SolicitudesCondicionamientosViewModel> GetCondiciones()
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        List<SolicitudesCondicionamientosViewModel> ListaCondiciones = new List<SolicitudesCondicionamientosViewModel>();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCatalogo_Condiciones_Listar", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDCondicion", "0");
            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();

            while (reader.Read())
            {
                ListaCondiciones.Add(new SolicitudesCondicionamientosViewModel()
                {
                    fiIDCondicion = (int)reader["fiIDCondicion"],
                    fcCondicion = (string)reader["fcCondicion"],
                    fcDescripcionCondicion = (string)reader["fcDescripcionCondicion"]
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
        return ListaCondiciones;
    }

    [WebMethod]
    public static bool CondicionarSolicitud(List<SolicitudesCondicionamientosViewModel> SolicitudCondiciones, string fcCondicionadoComentario)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader reader = null;
        SqlDataReader readerList = null;
        bool resultadoProceso = false;
        SqlTransaction transaccion = null;
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            DateTime fechaActual = DateTime.Now;
            
            //sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            //sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            sqlConexion = new SqlConnection(sqlConnectionString);
            sqlConexion.Open();
            transaccion = sqlConexion.BeginTransaction("insercionSolicitudCondiciones");
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_CondicionarSolicitudBitacora", sqlConexion, transaccion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);

            if (fcCondicionadoComentario == null)
                sqlComando.Parameters.AddWithValue("@fcCondicionadoComentario", "");
            else
                sqlComando.Parameters.AddWithValue("@fcCondicionadoComentario", fcCondicionadoComentario);

            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);            
            reader = sqlComando.ExecuteReader();
            string MensajeError = String.Empty;

            while (reader.Read())
            {
                if (reader["MensajeError"].ToString().StartsWith("-1"))
                    return false;
            }

            if (reader != null)
                reader.Close();

            foreach (SolicitudesCondicionamientosViewModel item in SolicitudCondiciones)
            {
                SqlCommand sqlComandoList;
                using (sqlComandoList = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Condicionamientos_Insert", sqlConexion, transaccion))
                {
                    sqlComandoList.CommandType = CommandType.StoredProcedure;
                    sqlComandoList.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComandoList.Parameters.AddWithValue("@fiIDCondicion", item.fiIDCondicion);
                    sqlComandoList.Parameters.AddWithValue("@fcComentarioAdicional", item.fcComentarioAdicional);
                    sqlComandoList.Parameters.AddWithValue("@piIDSesion", "1");
                    sqlComandoList.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComandoList.Parameters.AddWithValue("@piIDUsuario", IDUSR);
                    sqlComandoList.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComandoList.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                    using (readerList = sqlComandoList.ExecuteReader())
                    {
                        while (readerList.Read())
                            MensajeError = (string)readerList["MensajeError"];

                        if (!MensajeError.StartsWith("-1"))
                            resultadoProceso = true;
                        else
                        {
                            resultadoProceso = false;
                            break;
                        }
                    }
                }
            }
            if (SolicitudCondiciones.Count == 0)
                resultadoProceso = true;

            if (readerList != null)
                readerList.Close();

            if (resultadoProceso == true)
                transaccion.Commit();
        }
        catch (Exception ex)
        {
            transaccion.Rollback();
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