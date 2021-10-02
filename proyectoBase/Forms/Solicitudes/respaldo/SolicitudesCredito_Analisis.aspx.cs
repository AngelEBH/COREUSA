using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;

public partial class SolicitudesCredito_Analisis : System.Web.UI.Page
{
    private String pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";
    private int IdSolicitud = 0;
    //public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

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
                IdSolicitud = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                //pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                pcIDSesion = "1";

                bool AccesoAlAnalisis = ValidarAnalista(IdSolicitud);

                //if (AccesoAlAnalisis == false) {
                //string lcScript = "window.open('SolicitudesCredito_Bandeja.aspx?" + pcEncriptado + "','_self')";
                //Response.Write("<script>");
                //Response.Write(lcScript);
                //Response.Write("</script>");
                //}
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
    public static void MensajeExterno(string lcMensaje)
    {
        string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // Write the string array to a new file named "WriteLines.txt".
        using (StreamWriter outputFile = new StreamWriter(@"C:\Temp\WriteLines.txt"))
        {
                outputFile.WriteLine(lcMensaje);
        }
    }

    public bool ValidarAnalista(int IDSolicitud)
    {

        DSCore.DataCrypt DSC = new DSCore.DataCrypt();

        bool resultado = true;
        int IDPRODUCTO = 0;       
        int IDAPPLICATIONFORM = 0; 
        BandejaSolicitudesViewModel solicitudes = new BandejaSolicitudesViewModel();
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
                              IDPRODUCTO = (int)reader["fiIDTipoProducto"];
                              IDAPPLICATIONFORM = (int)reader["fiIDForm"];
                             
                            //  btnLinkDocumentos.Text = idForm;
                            //lblArraigoLaboral.Text = (string)reader["fcClienteArraigoLaboral"].ToString();

                            /* Llenar ficha de resumen */
                            //lblResumenCliente.Text = ((string)reader["fcPrimerNombreCliente"] + " " + (string)reader["fcSegundoNombreCliente"] + " " + (string)reader["fcPrimerApellidoCliente"] + " " + (string)reader["fcSegundoApellidoCliente"]).Replace(" ","");
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
                    }
                }

                //btnLinkDocumentos.NavigateUrl = "http://crediflash.prestadito.corp/DealerApplication/ViewMainApplicationForm?id=" +  IDAPPLICATIONFORM;

                
                string NombreLogo = IDPRODUCTO == 101 ? "iconoAuto48.png" : IDPRODUCTO == 201 ? "iconoRecibirDinero48.png" : (IDPRODUCTO == 100|| IDPRODUCTO == 102) ? "iconoAuto48.png" : IDPRODUCTO == 301 ? "iconoAuto48.png" : "iconoAuto48.png";
                LogoPrestamo.ImageUrl = "http://172.20.3.148/Imagenes/" + NombreLogo;

                if (solicitudes.fiIDAnalista == Convert.ToInt32(pcIDUsuario))
                {
                    resultado = true;
                }
                else if (solicitudes.fiIDAnalista == 0)
                {
                    /* Si la solicitud no está siendo analizada, asignar solicitud al usuario actual */
                    using (SqlCommand cmd = new SqlCommand("sp_CredSolicitud_Analizar", sqlConexion))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitud);
                        cmd.Parameters.AddWithValue("@fiIDAnalista", pcIDUsuario);
                        cmd.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                        cmd.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        cmd.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        using (SqlDataReader readerAnalisis = cmd.ExecuteReader())
                        {
                            readerAnalisis.Read();

                            if (readerAnalisis["MensajeError"].ToString().StartsWith("-1"))
                                resultado = false;
                            else
                                resultado = true;
                        }
                    }
                }
                else
                    resultado = false;

                /* Verficar si la solicitud tiene condicionamientos pendientes */
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_SolicitudCondiciones_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IdSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            pestanaListaSolicitudCondiciones.Style.Add("display", "");

                            HtmlTableRow tRowSolicitudCondiciones = null;
                            string EstadoCondicion = String.Empty;
                            int contadorCondiciones = 1;
                            while (reader.Read())
                            {
                                EstadoCondicion = (bool)reader["fbEstadoCondicion"] != true ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-danger mb-0'>Pendiente</label>";
                                tRowSolicitudCondiciones = new HtmlTableRow();
                                tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcCondicion"].ToString() });
                                tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcDescripcionCondicion"].ToString() });
                                tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcComentarioAdicional"].ToString() });
                                tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerHtml = EstadoCondicion });
                                tblListaSolicitudCondiciones.Rows.Add(tRowSolicitudCondiciones);
                                contadorCondiciones++;
                            }
                        }
                    }
                }

            }
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
        string lcIDCliente="";
        string lcPasoOperativo="";
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        SolicitudAnalisisViewModel ObjSolicitud = new SolicitudAnalisisViewModel();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            ////string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                BandejaSolicitudesViewModel SolicitudMaestro = new BandejaSolicitudesViewModel();

                /* Información de la solicitud */
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
                            lcIDCliente = reader["fiIDCliente"].ToString();
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
                                NombreGestor = (string)reader["fcNombreGestor"],
                              
                               
                            };

                            
                  
                        }
                    }
                    ObjSolicitud.solicitud = SolicitudMaestro;
                }

                lcPasoOperativo="ObtenerDocumentos";

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
lcPasoOperativo="InformacionCliente";
                /* Informacion del cliente */
                ClientesViewModel objCliente = new ClientesViewModel();

                /* Cliente Maestro */
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_Maestro_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    //lblNoSolicitud.Text="Cliente:"+lcIDCliente;
                    //sqlComando.Parameters.AddWithValue("@fiIDCliente", ObjSolicitud.solicitud.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", lcIDCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        if (reader.Read())
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
                    }
                }

                lcPasoOperativo="ListaLaboral";

                /* Informacion laboral */
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
                                fiIDBarrioColonia = Convert.ToInt32(reader["fiIDBarrioColonia"]),
                                fcNombreBarrioColonia = (string)reader["fcBarrio"],
                                //ciudad de la empresa
                                fiIDCiudad =  Convert.ToInt32(reader["fiIDCiudad"]),
                                fcNombreCiudad = (string)reader["fcPoblado"],
                                //municipio de la empresa
                                fiIDMunicipio =  Convert.ToInt32(reader["fiIDMunicipio"]),
                                fcNombreMunicipio = (string)reader["fcMunicipio"],
                                //departamento de la empresa
                                fiIDDepto =  Convert.ToInt32(reader["fiIDDepartamento"]),
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
                                //data de auditoria
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]

                            };
                        }
                    }
                }
lcPasoOperativo="Domicilio";
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
                        if (reader.Read())
                        {
                            lcPasoOperativo = reader["fiCodBarrio"].ToString();

                            objCliente.ClientesInformacionDomiciliar = new ClientesInformacionDomiciliarViewModel()
                            {
                                fiIDInformacionDomicilio = (int)reader["fiIDInformacionDomicilio"],
                                fiIDCliente = (int)reader["fiIDCliente"],
                                fcTelefonoCasa = (string)reader["fcTelefonoCasa"],
                                fcDireccionDetallada = (string)reader["fcDireccionDetalladaDomicilio"],
                                fcReferenciasDireccionDetallada = (string)reader["fcReferenciasDireccionDetalladaDomicilio"],
                                //barrio del cliente
                                fiIDBarrioColonia =  Convert.ToInt32(reader["fiCodBarrio"]),
                                fcNombreBarrioColonia = (string)reader["fcBarrio"],
                                //ciudad del cliente
                                fiIDCiudad =  Convert.ToInt32(reader["fiCodPoblado"]),
                                fcNombreCiudad = (string)reader["fcPoblado"],
                                //municipio del cliente
                                fiIDMunicipio =  Convert.ToInt32(reader["fiCodMunicipio"]),
                                fcNombreMunicipio = (string)reader["fcMunicipio"],
                                //departamento del cliente
                                fiIDDepto =  Convert.ToInt32(reader["fiCodDepartamento"]),
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
                    }
                }
lcPasoOperativo="Referencia";
                /* Referencias personales */
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
lcPasoOperativo="Avales";
                /* Avales de la solicitud */
                objCliente.Avales = new List<ClienteAvalesViewModel>();
                using (SqlCommand sqlComando = new SqlCommand("sp_CredAval_Maestro_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDAval", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                    }
                }
                ObjSolicitud.cliente = objCliente;
            }
        }
        catch (Exception ex)
        {

                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

            ex.Message.ToString();
            MensajeExterno(lcPasoOperativo+" / " +st.ToString()+"-" + frame.ToString() + " // "+ line.ToString() + " / " +ex.Message.ToString());
            //objCliente.clientesMaster.fcIdentidadCliente=ex.Message.ToString();
        }
        return ObjSolicitud;
    }

    [WebMethod]
    public static bool ValidacionesAnalisis(string validacion, string observacion, string dataCrypt)
    {
        bool resultadoProceso = false;
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

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_ValidacionesAnalisis", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@fcValidacion", validacion);
                    sqlComando.Parameters.AddWithValue("@fcComentario", observacion);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            if (!reader["MensajeError"].ToString().StartsWith("-1"))
                                resultadoProceso = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static bool EnviarACampo(string fcObservacionesDeCredito, string dataCrypt)
    {
        bool resultadoProceso = false;
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

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_EnviarACampo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@fcObservacionesDeCredito", fcObservacionesDeCredito);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            if (!reader["MensajeError"].ToString().StartsWith("-1"))
                                resultadoProceso = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static int SolicitudResolucion(SolicitudesBitacoraViewModel objBitacora, SolicitudesMasterViewModel objSolicitud, string dataCrypt)
    {
        int resultadoProceso = 0;
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

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolictud_Resolucion", sqlConexion))
                {
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
                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioPasoFinal", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        string MensajeError = string.Empty;
                        while (reader.Read())
                            MensajeError = (string)reader["MensajeError"];

                        if (!MensajeError.StartsWith("-1"))
                            resultadoProceso = Convert.ToInt32(MensajeError);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
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
                    }
                }
            }
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
        string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
        string pcIDSesion = "1";

        string parametrosEncriptados = DSC.Encriptar("usr=" + pcIDUsuario + "&ID=" + pcID + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion);
        return parametrosEncriptados;
    }

    [WebMethod]
    public static string EncriptarParametroDetallesAval(int parametro, string dataCrypt)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();

        Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
        string IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
        string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
        string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
        string pcIDSesion = "1";

        string parametrosEncriptados = DSC.Encriptar("usr=" + pcIDUsuario + "&IDAval=" + parametro + "&IDSOL=" + IDSOL + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion);
        return parametrosEncriptados;
    }

    [WebMethod]
    public static bool ActualizarIngresosCliente(decimal sueldoBaseReal, decimal bonosComisionesReal, string dataCrypt)
    {
        bool resultadoProceso = false;
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

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_ActualizarIngresosClienteSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@fnSueldoBaseReal", sueldoBaseReal);
                    sqlComando.Parameters.AddWithValue("@fnBonosComisionesReal", bonosComisionesReal);
                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            if (!reader["MensajeError"].ToString().StartsWith("-1"))
                                resultadoProceso = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static bool GuardarInformacionAnalisis(string tipoEmpresa, string tipoPerfil, string tipoEmpleo, string buroActual, string dataCrypt)
    {
        bool resultadoProceso = false;
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

                using (SqlCommand sqlComando = new SqlCommand("sp_CredSolicitud_GuardarInformaciondePerfil", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@fcTipoEmpresa", tipoEmpresa);
                    sqlComando.Parameters.AddWithValue("@fcTipoPerfil", tipoPerfil);
                    sqlComando.Parameters.AddWithValue("@fcTipoEmpleado", tipoEmpleo);
                    sqlComando.Parameters.AddWithValue("@fcBuroActual", buroActual);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            if (!reader["MensajeError"].ToString().StartsWith("-1"))
                                resultadoProceso = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static CalculoPrestamoViewModel CalculoPrestamo(decimal ValorPrestamo, decimal ValorPrima, decimal CantidadPlazos, string dataCrypt)
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
            int idProducto = 0;

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            idProducto = (int)reader["fiIDTipoProducto"];
                    }
                }

                using (SqlCommand sqlComando = new SqlCommand("sp_CredSolicitud_CalculoPrestamoCallBack", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDProducto", idProducto);
                    sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", ValorPrestamo);
                    sqlComando.Parameters.AddWithValue("@liPlazo", CantidadPlazos);
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", ValorPrima);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);

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
                }
            }
        }
        catch (Exception ex)
        {
            objCalculo = objCalculo = new CalculoPrestamoViewModel();
            ex.Message.ToString();
        }
        return objCalculo;
    }

    [WebMethod]
    public static CalculoPrestamoViewModel CalculoPrestamoVehiculo(decimal ValorPrestamo, decimal ValorPrima, decimal CantidadPlazos, string dataCrypt)
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
            int idProducto = 0;

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            idProducto = (int)reader["fiIDTipoProducto"];
                    }
                }

                if (int.Parse(IDSOL) < 802 && IDSOL != "773" && IDSOL != "694" && IDSOL != "323")
                {
                    using (var sqlComando = new SqlCommand("sp_CredSolicitud_CalculoPrestamo", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDProducto", idProducto);
                        sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", ValorPrestamo);
                        sqlComando.Parameters.AddWithValue("@liPlazo", CantidadPlazos);
                        sqlComando.Parameters.AddWithValue("@pnValorPrima", ValorPrima);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                        using (var reader = sqlComando.ExecuteReader())
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
                    }
                }
                else
                {
                    using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitudes_InformacionPrestamo_ObtenerPorIdSolicitud", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                        using (SqlDataReader reader = sqlComando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                objCalculo = new CalculoPrestamoViewModel()
                                {
                                    SegurodeDeuda = 0,
                                    SegurodeVehiculo = (decimal)reader["fnValorTotalSeguro"],
                                    GastosdeCierre = (decimal)reader["fnGastosDeCierre"],
                                    ValoraFinanciar = (decimal)reader["fnValorTotalFinanciamiento"],
                                    CuotaQuincenal = (decimal)reader["fnCuotaTotal"],
                                    CuotaMensual = (decimal)reader["fnCuotaTotal"],
                                    CuotaServicioGPS = (decimal)reader["fnCuotaMensualGPS"],
                                    CuotaSegurodeVehiculo = (decimal)reader["fnCuotaMensualSeguro"],
                                    CuotaMensualNeta = (decimal)reader["fnCuotaTotal"],
                                    TotalSeguroVehiculo = (decimal)reader["fnValorTotalSeguro"],
                                    TipoCuota = (string)reader["fcTipoDePlazo"]
                                };
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return objCalculo;
    }

    [WebMethod]
    public static ClienteAvalesViewModel DetallesAval(int IDAval, string dataCrypt)
    {
        ClienteAvalesViewModel AvalInfo = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CredAval_InformacionPrincipal", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDAval", IDAval);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return AvalInfo;
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
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand cmd = new SqlCommand("sp_CredCotizador_ConPrima", sqlConexion))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@pcIdentidad", identidad);
                    cmd.Parameters.AddWithValue("@pnValorProducto", ValorProducto);
                    cmd.Parameters.AddWithValue("@pnPrima", ValorPrima);

                    int IDContador = 1;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listaCotizadorProductos.Add(new cotizadorProductosViewModel()
                            {
                                IDCotizacion = IDContador,
                                IDProducto = (short)reader["fiIDProducto"],
                                ProductoDescripcion = reader["fcProducto"].ToString(),
                                fnMontoOfertado = decimal.Parse(reader["fnMontoOfertado"].ToString()),
                                fiPlazo = int.Parse(reader["fiIDPlazo"].ToString()),
                                fnCuotaQuincenal = decimal.Parse(reader["fnCuotaQuincenal"].ToString()),
                                TipoCuota = (string)reader["fcTipodeCuota"]
                            });
                            IDContador += 1;
                        }
                    }
                }
                objPrecalificado.cotizadorProductos = listaCotizadorProductos;
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return objPrecalificado;
    }

    [WebMethod]
    public static bool ActualizarPlazoMontoFinanciar(decimal ValorGlobal, decimal ValorPrima, int CantidadPlazos, string dataCrypt)
    {
        bool resultadoProceso = false;
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

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_ActualizarPlazoMontoFinanciar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@fnValorGlobal", ValorGlobal);
                    sqlComando.Parameters.AddWithValue("@fnValorPrima", ValorPrima);
                    sqlComando.Parameters.AddWithValue("@fiCantidadPlazos", CantidadPlazos);
                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            if (!reader["MensajeError"].ToString().StartsWith("-1"))
                                resultadoProceso = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static bool ComentarioReferenciaPersonal(int IDReferencia, string comentario, string dataCrypt)
    {
        bool resultadoProceso = false;
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
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Comentario", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", IDReferencia);
                    sqlComando.Parameters.AddWithValue("@fcComentarioDeptoCredito", comentario);
                    sqlComando.Parameters.AddWithValue("@fiAnalistaComentario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            if (!reader["MensajeError"].ToString().StartsWith("-1"))
                                resultadoProceso = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static bool EliminarReferenciaPersonal(int IDReferencia, string dataCrypt)
    {
        bool resultadoProceso = false;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("dbo.sp_CREDCliente_Referencias_Eliminar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", IDReferencia);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            if (!reader["MensajeError"].ToString().StartsWith("-1"))
                                resultadoProceso = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static List<SolicitudesCondicionamientosViewModel> GetCondiciones(string dataCrypt)
    {
        List<SolicitudesCondicionamientosViewModel> ListaCondiciones = new List<SolicitudesCondicionamientosViewModel>();
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
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCatalogo_Condiciones_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCondicion", "0");
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
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
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return ListaCondiciones;
    }

    [WebMethod]
    public static bool CondicionarSolicitud(List<SolicitudesCondicionamientosViewModel> SolicitudCondiciones, string fcCondicionadoComentario, string dataCrypt)
    {
        bool resultadoProceso = false;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            ////string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                using (SqlTransaction transaccion = sqlConexion.BeginTransaction("insercionSolicitudCondiciones"))
                {
                    try
                    {
                        using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_CondicionarSolicitudBitacora", sqlConexion, transaccion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                            if (fcCondicionadoComentario == null)
                                sqlComando.Parameters.AddWithValue("@fcCondicionadoComentario", "");
                            else
                                sqlComando.Parameters.AddWithValue("@fcCondicionadoComentario", fcCondicionadoComentario);

                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                            using (SqlDataReader reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader["MensajeError"].ToString().StartsWith("-1"))
                                        return false;
                                }
                            }
                        }

                        foreach (SolicitudesCondicionamientosViewModel item in SolicitudCondiciones)
                        {
                            using (SqlCommand sqlComandoList = new SqlCommand("sp_CREDSolicitud_Condicionamientos_Insert", sqlConexion, transaccion))
                            {
                                sqlComandoList.CommandType = CommandType.StoredProcedure;
                                sqlComandoList.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                                sqlComandoList.Parameters.AddWithValue("@fiIDCondicion", item.fiIDCondicion);
                                sqlComandoList.Parameters.AddWithValue("@fcComentarioAdicional", item.fcComentarioAdicional);
                                sqlComandoList.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                sqlComandoList.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComandoList.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                sqlComandoList.Parameters.AddWithValue("@pcUserNameCreated", "");
                                sqlComandoList.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);
                                using (SqlDataReader readerList = sqlComandoList.ExecuteReader())
                                {
                                    readerList.Read();

                                    if (!readerList["MensajeError"].ToString().StartsWith("-1"))
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

                        if (resultadoProceso == true)
                            transaccion.Commit();
                    }
                    catch (Exception ex)
                    {
                        ex.Message.ToString();
                        transaccion.Rollback();
                    }
                } // using transaction
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
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
/* View Models */
/*
    public class ClientesInformacionDomiciliarViewModel
    {
        public int fiIDInformacionDomicilio { get; set; }
        public int fiIDCliente { get; set; }
        public string fcTelefonoCasa { get; set; }
        public string fcDireccionDetallada { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public string fiIDBarrioColonia { get; set; }
        public string fcReferenciasDireccionDetallada { get; set; }

        //colonia del cliente
        public string fcNombreBarrioColonia { get; set; }
        public bool fbBarrioColoniaActivo { get; set; }

        //ciudad del cliente
        public string fiIDCiudad { get; set; }
        public string fcNombreCiudad { get; set; }
        public bool fbCiudadActivo { get; set; }

        //ciudad del cliente
        public string fiIDMunicipio { get; set; }
        public string fcNombreMunicipio { get; set; }
        public bool fbMunicipioActivo { get; set; }

        //departamento del cliente
        public string fiIDDepto { get; set; }
        public string fcNombreDepto { get; set; }
        public bool fbDepartamentoActivo { get; set; }
        public string fcLatitud { get; set; }
        public string fcLongitud { get; set; }
        public int fiIDGestorValidador  { get; set; }
        public string fcGestorValidadorDomicilio { get; set; }
        public int fiIDInvestigacionDeCampo { get; set; }
        public string fcGestionDomicilio { get; set; }
        public string fcResultadodeCampo { get; set; }
        public DateTime fdFechaValidacion { get; set; }
        public string fcObservacionesCampo { get; set; }
        public string fiIDEstadoDeGestion { get; set; }
        public string fiEstadoLaboral { get; set; }
        public byte IDTipoResultado { get; set; }
        public byte fiEstadoDomicilio  { get; set; }

    }

    public class ClientesInformacionLaboralViewModel
    {
        public int fiIDInformacionLaboral { get; set; }
        public int fiIDCliente { get; set; }
        public string fcNombreTrabajo { get; set; }
        public decimal fiIngresosMensuales { get; set; }
        public string fcPuestoAsignado { get; set; }
        public System.DateTime fcFechaIngreso { get; set; }
        public string fdTelefonoEmpresa { get; set; }
        public string fcExtensionRecursosHumanos { get; set; }
        public string fcExtensionCliente { get; set; }
        public string fcDireccionDetalladaEmpresa { get; set; }
        public string fcFuenteOtrosIngresos { get; set; }
        public Nullable<decimal> fiValorOtrosIngresosMensuales { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public string fiIDBarrioColonia { get; set; }
        public string fcReferenciasDireccionDetallada { get; set; }

        //colonia del cliente
        public string fcNombreBarrioColonia { get; set; }
        public bool fbBarrioColoniaActivo { get; set; }

        //ciudad del cliente
        public string fiIDCiudad { get; set; }
        public string fcNombreCiudad { get; set; }
        public bool fbCiudadActivo { get; set; }

        //ciudad del cliente
        public string fiIDMunicipio { get; set; }
        public string fcNombreMunicipio { get; set; }
        public bool fbMunicipioActivo { get; set; }

        //departamento del cliente
        public string fiIDDepto { get; set; }
        public string fcNombreDepto { get; set; }
        public bool fbDepartamentoActivo { get; set; }

        public string fcLatitud { get; set; }
        public string fcLongitud { get; set; }
        public int fiIDGestorValidador  { get; set; }
        public string fcGestorValidadorTrabajo{ get; set; }
        public int fiIDInvestigacionDeCampo { get; set; }
        public string fcGestionTrabajo { get; set; }
        public string fcResultadodeCampo { get; set; }
        public DateTime fdFechaValidacion { get; set; }
        public string fcObservacionesCampo { get; set; }
        public byte fiIDEstadoDeGestion { get; set; }
        public byte fiEstadoLaboral { get; set; }
        public byte IDTipoResultado { get; set; }
        public byte fiEstadoDomicilio  { get; set; }
    }
*/
}