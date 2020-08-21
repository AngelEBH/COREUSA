using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_Detalles : System.Web.UI.Page
{
    private String pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";

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
                string IDSOL = "0";
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
                    IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    //pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcIDSesion = "1";
                    bool AccesoAlAnalisis = CargarInformacion(IDSOL);
                }
                else
                {
                    string lcScript = "window.open('SolicitudesCredito_Bandeja.aspx?" + pcEncriptado + "','_self')";
                    Response.Write("<script>");
                    Response.Write(lcScript);
                    Response.Write("</script>");
                }
            }
            catch
            {
                string lcScript = "window.open('SolicitudesCredito_Bandeja.aspx?" + pcEncriptado + "','_self')";
                Response.Write("<script>");
                Response.Write(lcScript);
                Response.Write("</script>");
            }
        }
    }

    public bool CargarInformacion(string IDSolicitud)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        BandejaSolicitudesViewModel solicitudes = new BandejaSolicitudesViewModel();
        int idProducto = 0;
        bool resultado;
        try
        {
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            solicitudes = new BandejaSolicitudesViewModel()
                            {
                                fiIDAnalista = (int)reader["fiIDAnalista"],
                                fcNombreCortoAnalista = (string)reader["fcNombreCortoAnalista"]
                            };
                            idProducto = (int)reader["fiIDTipoProducto"];
                            lblArraigoLaboral.Text = (string)reader["fcClienteArraigoLaboral"].ToString();
                        }
                    }
                }
            } // using connection

            string NombreLogo = idProducto == 101 ? "iconoRecibirDinero48.png" : idProducto == 201 ? "iconoMoto48.png" : idProducto == 202 ? "iconoAuto48.png" : idProducto == 301 ? "iconoConsumo48.png" : "iconoConsumo48.png";
            LogoPrestamo.ImageUrl = "http://172.20.3.140/Imagenes/" + NombreLogo;

            if (solicitudes.fiIDAnalista == Convert.ToInt32(pcIDUsuario))
                resultado = true;
            else
                resultado = false;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    [WebMethod]
    public static SolicitudAnalisisViewModel CargarInformacionSolicitud(string dataCrypt)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        SolicitudAnalisisViewModel ObjSolicitud = new SolicitudAnalisisViewModel();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                /* Informacion de la solicitud */
                BandejaSolicitudesViewModel SolicitudMaestro = new BandejaSolicitudesViewModel();
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SolicitudMaestro = new BandejaSolicitudesViewModel()
                            {
                                fiIDSolicitud = (int)reader["fiIDSolicitud"],
                                fiIDTipoPrestamo = (int)reader["fiIDTipoProducto"],
                                fcDescripcion = (string)reader["fcProducto"],
                                fiTipoSolicitud = (short)reader["fiTipoSolicitud"],
                                TipoNegociacion = (short)reader["fiTipoNegociacion"],
                                // Informacion del precalificado
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
                                // Informacion del vendedor
                                fiIDUsuarioVendedor = (int)reader["fiIDUsuarioVendedor"],
                                fcNombreCortoVendedor = (string)reader["fcNombreCortoVendedor"],
                                fdFechaCreacionSolicitud = (DateTime)reader["fdFechaCreacionSolicitud"],
                                // Informacion del analista
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
                                // Informacion cliente
                                fiIDCliente = (int)reader["fiIDCliente"],
                                fcNoAgencia = (string)reader["fcCentrodeCosto"],
                                fcAgencia = (string)reader["fcNombreAgencia"],
                                // Bitacora
                                fdEnIngresoInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnIngresoInicio"]),
                                fdEnIngresoFin = ConvertFromDBVal<DateTime>((object)reader["fdEnIngresoFin"]),
                                fdEnTramiteInicio = ConvertFromDBVal<DateTime>((object)reader["fdEnColaInicio"]),
                                fdEnTramiteFin = ConvertFromDBVal<DateTime>((object)reader["fdEnColaFin"]),
                                // Todo el proceso de analisis
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
                                // Todo el proceso de analisis
                                fdCondicionadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdCondicionadoInicio"]),
                                fcCondicionadoComentario = (string)reader["fcCondicionadoComentario"],
                                fdCondificionadoFin = ConvertFromDBVal<DateTime>((object)reader["fdCondificionadoFin"]),
                                fiIDOrigen = (short)reader["fiIDOrigen"],
                                // Proceso de campo
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
                                // Informacion del gestor
                                IDGestor = (int)reader["fiIDGestor"],
                                NombreGestor = (string)reader["fcNombreGestor"]
                            };
                        }
                    }
                    ObjSolicitud.solicitud = SolicitudMaestro;
                }

                /* Documentos de la solicitud */
                List<SolicitudesDocumentosViewModel> ListadoDocumentos = new List<SolicitudesDocumentosViewModel>();
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_ObtenerSolicitudDocumentos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                    }
                    ObjSolicitud.documentos = ListadoDocumentos;
                }

                /* Informacion del cliente */
                ClientesViewModel objCliente = new ClientesViewModel();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_Maestro_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", ObjSolicitud.solicitud.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                    }
                }

                /* Informacion Laboral*/
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_Laboral_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                                // Proceso de campo
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
                    }
                }

                /* Informacion del conyugue */
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_InformacionConyugal_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSOlicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                    }
                }

                /* Informacion del domicilio */
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_InformacionDomiciliar_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                                // Proceso de campo
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
                    }
                }

                /* Referencias de la solicitud */
                objCliente.ClientesReferenciasPersonales = new List<ClientesReferenciasViewModel>();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                    }
                }
                ObjSolicitud.cliente = objCliente;

            }// using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return ObjSolicitud;
    }

    [WebMethod]
    public static BandejaSolicitudesViewModel CargarEstadoSolicitud(string dataCrypt)
    {
        BandejaSolicitudesViewModel objEstadoSolicitud = new BandejaSolicitudesViewModel();
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CredSolicitud_SolicitudEstadoProcesamiento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                                // Todo el proceso de analisis
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
                                // Todo el proceso de analisis
                                fdCondicionadoInicio = ConvertFromDBVal<DateTime>((object)reader["fdCondicionadoInicio"]),
                                fcCondicionadoComentario = (string)reader["fcCondicionadoComentario"],
                                fdCondificionadoFin = ConvertFromDBVal<DateTime>((object)reader["fdCondificionadoFin"]),
                                // Proceso de campo
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
                                fcTiempoTotalTranscurrido = (string)reader["fcTiempoTotalTranscurrido"]
                            };
                        }
                    } // using reader
                } // using command
            }// using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return objEstadoSolicitud;
    }

    [WebMethod]
    public static string ObtenerUrlEncriptado(string dataCrypt)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();

        Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
        string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
        string pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
        string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp").ToString();
        //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID").ToString();
        string pcIDSesion = "1";

        string parametrosEncriptados = DSC.Encriptar("usr=" + pcIDUsuario + "&ID=" + pcID + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion);
        return parametrosEncriptados;
    }

    [WebMethod]
    public static PrecalificadoViewModel GetPrestamosSugeridos(decimal ValorProducto, decimal ValorPrima, string dataCrypt)
    {
        PrecalificadoViewModel objPrecalificado = new PrecalificadoViewModel();
        List<cotizadorProductosViewModel> listaCotizadorProductos = new List<cotizadorProductosViewModel>();
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string identidad = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr").ToString();

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand cmd = new SqlCommand("sp_CredCotizador_ConPrima", sqlConexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pcIdentidad", identidad);
                    cmd.Parameters.AddWithValue("@pnValorProducto", ValorProducto);
                    cmd.Parameters.AddWithValue("@pnPrima", ValorPrima);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
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
                    }
                } // using command
            } // using connection

            objPrecalificado.cotizadorProductos = listaCotizadorProductos;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return objPrecalificado;
    }

    [WebMethod]
    public static CalculoPrestamoViewModel CalculoPrestamo(decimal MontoFinanciar, decimal PlazoFinanciar, decimal ValorPrima, string dataCrypt)
    {
        CalculoPrestamoViewModel objCalculo = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";
            int IDPRODUCTO = 0;

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            IDPRODUCTO = (int)reader["fiIDTipoProducto"];
                    }
                }

                using (SqlCommand sqlComando = new SqlCommand("sp_CredSolicitud_CalculoPrestamo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDProducto", IDPRODUCTO);
                    sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", MontoFinanciar);
                    sqlComando.Parameters.AddWithValue("@liPlazo", PlazoFinanciar);
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", ValorPrima);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return objCalculo;
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