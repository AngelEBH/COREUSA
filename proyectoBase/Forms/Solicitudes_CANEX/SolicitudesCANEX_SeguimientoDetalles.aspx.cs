using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

public partial class SolicitudesCANEX_SeguimientoDetalles : System.Web.UI.Page
{
    string pcEncriptado = "";
    string pcIDUsuario = "";
    string pcIDApp = "";
    string pcIDSesion = "";

    public short IdPais = 0;
    public short IdSocio = 0;
    public short IdAgencia = 0;
    public decimal IdEstadoSolicitud = 0;
    public int IdSolicitudPrestadito = 0;
    public DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                var idSolicitud = "0";
                var lcURL = "";
                var liParamStart = 0;
                var lcParametros = "";
                var lcParametroDesencriptado = "";
                Uri lURLDesencriptado = null;

                lcURL = Request.Url.ToString();
                liParamStart = lcURL.IndexOf("?");


                if (liParamStart > 0)
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                else
                    lcParametros = string.Empty;

                if (lcParametros != string.Empty)
                {
                    pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

                    var idProducto = 0;
                    var estadoSolicitud = 0m;
                    var identidadCliente = string.Empty;

                    using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                    {
                        sqlConexion.Open();

                        /* Informacion de la solicitud */
                        using (var sqlComando = new SqlCommand("sp_CANEX_Solicitud_Detalle", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    idProducto = (short)sqlResultado["fiIDProducto"];
                                    estadoSolicitud = (decimal)sqlResultado["fiEstadoSolicitud"];
                                    IdEstadoSolicitud = estadoSolicitud;
                                    IdPais = (short)sqlResultado["fiIDPais"];
                                    IdSocio = (short)sqlResultado["fiIDSocio"];
                                    IdAgencia = (short)sqlResultado["fiIDAgencia"];
                                    IdSolicitudPrestadito = (int)sqlResultado["fiIDSolicitudPrestadito"];
                                    identidadCliente = sqlResultado["fcIdentidad"].ToString();

                                    var logo = idProducto == 101 ? "iconoRecibirDinero48.png" : idProducto == 201 ? "iconoMoto48.png" : idProducto == 202 ? "iconoAuto48.png" : idProducto == 301 ? "iconoConsumo48.png" : "iconoConsumo48.png";
                                    var idSolicitudCanex = (int)sqlResultado["fiIDSolicitudCANEX"];
                                    var fechaNacimientoCliente = (DateTime)sqlResultado["fdNacimientoCliente"];
                                    var esComisionista = (decimal)sqlResultado["fiComisionista"];

                                    /* Información principal */
                                    imgLogo.ImageUrl = "/Imagenes/" + logo;
                                    lblProducto.Text = sqlResultado["fcNombreProducto"].ToString();
                                    lblNombreCliente.Text = sqlResultado["fcNombreCliente"].ToString();
                                    lblIdentidadCliente.Text = identidadCliente;
                                    lblNoSolicitud.Text = sqlResultado["fiIDSolicitudCANEX"].ToString();
                                    lblNoCliente.Text = sqlResultado["fiIDCliente"].ToString();
                                    lblTipoSolicitud.Text = "Solicitud CANEX";
                                    lblAgenteDeVentas.Text = sqlResultado["fcNombreUsuario"].ToString();
                                    lblAgencia.Text = sqlResultado["fcNombreAgencia"].ToString();
                                    lblEstadoSolicitud.Text = sqlResultado["fcEstadoSolicitud"].ToString();
                                    lblEstadoSolicitudModal.Text = sqlResultado["fcEstadoSolicitud"].ToString();

                                    /* Información personal */
                                    txtRTNCliente.Text = sqlResultado["fcRTN"].ToString();
                                    txtTelefonoCliente.Text = sqlResultado["fcTelefonoPrimario"].ToString();
                                    txtNumeroTelefonoAlternativo.Text = sqlResultado["fcTelefonoAlternativo"].ToString();

                                    /* Calcular edad del cliente */
                                    var hoy = DateTime.Today;
                                    var edad = hoy.Year - fechaNacimientoCliente.Year;

                                    if (fechaNacimientoCliente.Date > hoy.AddYears(-edad)) edad--;
                                    txtFechaNacimientoCliente.Text = fechaNacimientoCliente.ToString("MM/dd/yyyy");

                                    txtEdadCliente.Text = edad.ToString() + " " + "años";
                                    txtNacionalidad.Text = sqlResultado["fcNacionalidadCliente"].ToString();
                                    txtProfesionCliente.Text = sqlResultado["fcProfesionuOficio"].ToString();
                                    txtSexoCliente.Text = sqlResultado["fcSexoCliente"].ToString();
                                    txtEstadoCivilCliente.Text = sqlResultado["fcEstadoCivilCliente"].ToString();

                                    /* Información del domicilio */
                                    txtVivienda.Text = sqlResultado["fcTipoResidenciaCliente"].ToString();
                                    txtTiempoDeResidir.Text = sqlResultado["fcTiempoDeResidir"].ToString();
                                    txtDepartamentoDomicilio.Text = sqlResultado["fcDepartamentoDomicilio"].ToString();
                                    txtMunicipioDomicilio.Text = sqlResultado["fcMunicipioDomicilio"].ToString();
                                    txtCiudadPobladoDomicilio.Text = sqlResultado["fcCiudadDomicilio"].ToString();
                                    txtBarrioColoniaDomicilio.Text = sqlResultado["fcBarrioColoniaDomicilio"].ToString();
                                    txtDireccionDetalladaDomicilio.InnerText = sqlResultado["fcDireccionDetallada"].ToString();
                                    txtReferenciasDomicilio.InnerText = sqlResultado["fcDireccionReferencias"].ToString();

                                    /* Información conyugal */
                                    string nombreCompletoConyugue = sqlResultado["fcPrimerNombreConyugue"].ToString() + sqlResultado["fcSegundoNombreConyugue"].ToString() + sqlResultado["fcPrimerApellidoConyugue"].ToString() + sqlResultado["fcSegundoApellidoConyugue"].ToString();

                                    if (string.IsNullOrEmpty(sqlResultado["fcPrimerNombreConyugue"].ToString()))
                                    {
                                        divPanelInformacionConyugal.Visible = false;
                                    }
                                    else
                                    {
                                        var fechaNacimientoConyugue = (DateTime)sqlResultado["fdFechaNacimientoConyugue"];

                                        txtNombreDelConyugue.Text = nombreCompletoConyugue.Replace("  ", " ");
                                        txtIdentidadConyugue.Text = sqlResultado["fcIdentidadConyugue"].ToString();
                                        txtFechaNacimientoConyugue.Text = fechaNacimientoConyugue.ToString("MM/dd/yyyy");
                                        txtTelefonoConyugue.Text = sqlResultado["fcTelefonoConyugue"].ToString();
                                        txtProfesionOficioConyugue.Text = sqlResultado["fcProfesionuOficioConyugue"].ToString();
                                        txtOcupacionConyugue.Text = sqlResultado["fcOcupacionConyugue"].ToString();
                                        txtLugarTrabajoConyugue.Text = sqlResultado["fcNombreEmpresaConyugue"].ToString();
                                        txtPuestoAsignadoConyugue.Text = sqlResultado["fcPuestoConyugue"].ToString();
                                    }

                                    /* Información laboral */
                                    txtNombreTrabajoCliente.Text = sqlResultado["fcNombreEmpresa"].ToString();
                                    txtPuestoAsignado.Text = sqlResultado["fcPuesto"].ToString();
                                    txtIngresosMensuales.Text = sqlResultado["fnIngresoMensualBase"].ToString();

                                    if (esComisionista == 0)
                                    {
                                        divComisionesCliente.Visible = false;
                                    }
                                    else
                                    {
                                        txtComisionesCliente.Text = sqlResultado["fnIngresoMensualComisiones"].ToString();
                                    }

                                    int arraigoLaboralMeses = GetMonthDifference(hoy, DateTime.Parse(sqlResultado["fdFechaIngresoEmpresa"].ToString()));

                                    txtFechaIngreso.Text = DateTime.Parse(sqlResultado["fdFechaIngresoEmpresa"].ToString()).ToString("MM/dd/yyyy");
                                    txtArraigoLaboral.Text = arraigoLaboralMeses.ToString() + " meses";
                                    txtTelefonoEmpresa.Text = sqlResultado["fcTelefonoEmpresa"].ToString();
                                    txtExtensionCliente.Text = sqlResultado["fcExtension"].ToString();
                                    txtExtensionRecursosHumanos.Text = sqlResultado["fcExtensionRRHH"].ToString();
                                    txtDepartamentoEmpresa.Text = sqlResultado["fcDepartamentoEmpresa"].ToString();
                                    txtMunicipioEmpresa.Text = sqlResultado["fcMunicipioEmpresa"].ToString();
                                    txtCiudadPobladoEmpresa.Text = sqlResultado["fcCiudadEmpresa"].ToString();
                                    txtBarrioColoniaEmpresa.Text = sqlResultado["fcBarrioColoniaEmpresa"].ToString();
                                    txtDireccionDetalladaEmpresa.InnerText = sqlResultado["fcDireccionDetalladaEmpresa"].ToString();
                                    txtReferenciaDetalladaEmpresa.InnerText = sqlResultado["fcDireccionReferenciasEmpresa"].ToString();
                                    txtFuenteDeOtrosIngresos.Text = sqlResultado["fcDescripcionOtrosIngresos"].ToString();
                                    txtValorDeOtrosIngresos.Text = decimal.Parse(sqlResultado["fnOtrosIngresos"].ToString()).ToString("N");

                                    /* Informacion del préstamo requerido */
                                    txtValorGlobal.Text = ((decimal)sqlResultado["fnValorGlobal"]).ToString("N");
                                    txtValorPrima.Text = ((decimal)sqlResultado["fnValorPrima"]).ToString("N");
                                    lblPlazoTitulo.InnerText = "Plazo " + sqlResultado["fcTipodeCuota"].ToString();
                                    txtPlazoSeleccionado.Text = sqlResultado["fcPlazo"].ToString();
                                    txtMontoAFinanciar.Text = ((decimal)sqlResultado["fnValorPrestamo"]).ToString("N");

                                    /* Habilitar/Inhabilitar botones de acciones */
                                    var estadoRechazada = 5;
                                    var estadoAprobada = 4;
                                    var estadoEnRevision = 3;
                                    var title = string.Empty;
                                    var colorClass = string.Empty;

                                    if (estadoSolicitud == estadoAprobada || estadoSolicitud == estadoRechazada)
                                    {
                                        colorClass = estadoSolicitud == estadoAprobada ? "text-success" : "text-danger";

                                    }
                                    else if (IdSolicitudPrestadito != 0)
                                    {
                                        colorClass = "text-success";
                                    }
                                    else if (estadoSolicitud == estadoEnRevision && IdSolicitudPrestadito == 0)
                                    {
                                        colorClass = "text-warning";
                                    }
                                    else if (estadoSolicitud != estadoRechazada && estadoSolicitud != estadoAprobada && IdSolicitudPrestadito == 0)
                                    {
                                        colorClass = "text-warning";
                                    }

                                    var claseCss = "col-form-label font-16 font-weight-bold " + colorClass;
                                    lblEstadoSolicitud.Attributes.Add("class", colorClass);
                                    lblEstadoSolicitudModal.Attributes.Add("class", claseCss);

                                    /* Comentarios acerca de la resolucion de la solicitud */
                                    var nombreAnalistaResolucion = sqlResultado["fcNombreAnalistaResolucion"].ToString();
                                    var comentarioResolucion = sqlResultado["fcComentarioResulucion"].ToString();

                                    if (!string.IsNullOrEmpty(comentarioResolucion))
                                    {
                                        btnDetallesResolucion.Visible = true;
                                        lblNombreAnalista.Text = nombreAnalistaResolucion;
                                        lblDetalleEstado.InnerText = comentarioResolucion;
                                    }
                                }
                            }
                        }

                        /* Referencias del cliente */
                        using (var sqlComando = new SqlCommand("sp_CANEX_Solicitud_Referencias", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                /* Llenar table de referencias */
                                HtmlTableRow tRowReferencias = null;
                                while (sqlResultado.Read())
                                {
                                    tRowReferencias = new HtmlTableRow();
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcNombre"].ToString() });
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcTrabajo"].ToString() });
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcTiempoConocerlo"].ToString() });
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcTelefono"].ToString() });
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcParentesco"].ToString() });
                                    tblReferencias.Rows.Add(tRowReferencias);
                                }
                            }
                        }

                        /* Informacion del precalificado */
                        using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcIdentidad", identidadCliente);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    var ObligacionesPrecalificado = (decimal)sqlResultado["fnTotalObligaciones"];
                                    var IngresosReales = (decimal)sqlResultado["fnIngresos"];
                                    var CapacidadPagoMensual = decimal.Parse(sqlResultado["fnCapacidadDisponible"].ToString());

                                    txtIngresosPrecalificado.Text = IngresosReales.ToString("N");
                                    txtObligacionesPrecalificado.Text = ObligacionesPrecalificado.ToString("N");
                                    txtDisponiblePrecalificado.Text = CapacidadPagoMensual.ToString("N");
                                    txtCapacidadDePagoMensual.Text = CapacidadPagoMensual.ToString("N");
                                    txtCapacidadDePagoQuincenal.Text = (CapacidadPagoMensual / 2).ToString("N");
                                }
                            }
                        }

                        /* Verficar si la solicitud tiene condicionamientos pendientes */
                        using (var sqlComando = new SqlCommand("sp_CANEX_Solicitud_Condiciones", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                if (sqlResultado.HasRows)
                                {
                                    HtmlTableRow tRowSolicitudCondiciones = null;
                                    var EstadoCondicion = string.Empty;
                                    var contadorCondiciones = 1;
                                    while (sqlResultado.Read())
                                    {
                                        EstadoCondicion = (short)sqlResultado["fiEstadoCondicion"] != 0 ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-danger mb-0'>Pendiente</label>";
                                        tRowSolicitudCondiciones = new HtmlTableRow();
                                        tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcCategoriaCondicion"].ToString() });
                                        tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcDescripcionCondicion"].ToString() });
                                        tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcObservaciones"].ToString() });
                                        tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerHtml = EstadoCondicion });
                                        tblListaSolicitudCondiciones.Rows.Add(tRowSolicitudCondiciones);
                                        contadorCondiciones++;
                                    }
                                    btnListaCondiciones.Visible = true;
                                }
                            }
                        }

                        /* Documentos de la solicitud */
                        using (var sqlComando = new SqlCommand("sp_CANEX_Solicitud_Documentos", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                var fcNombreSocio = string.Empty;
                                var fcNombreImagen = string.Empty;

                                var documentacionIdentidad = new StringBuilder();
                                var documentacionDomicilio = new StringBuilder();
                                var documentacionLaboral = new StringBuilder();
                                var documentacionSolicitudFisica = new StringBuilder();
                                var documentacionCampoDomicilio = new StringBuilder();
                                var documentacionCampoTrabajo = new StringBuilder();
                                var documentacionOtros = new StringBuilder();
                                var idTipoDocumento = string.Empty;

                                while (sqlResultado.Read())
                                {
                                    idTipoDocumento = sqlResultado["fiIDImagen"].ToString();
                                    fcNombreSocio = sqlResultado["fcNombreSocio"].ToString();
                                    fcNombreImagen = sqlResultado["fcNombreImagen"].ToString();

                                    switch (idTipoDocumento)
                                    {
                                        case "1":
                                        case "2":
                                        case "18":
                                        case "19":
                                            documentacionIdentidad.Append("<img alt='" + sqlResultado["fcNombreImagen"] + "' src='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + "' data-image='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + "' data-description='" + sqlResultado["fcNombreImagen"] + "' data-identificador='" + sqlResultado["fcNombreImagen"] + "'/>");
                                            break;
                                        case "3":
                                        case "5":
                                            documentacionDomicilio.Append("<img alt='" + sqlResultado["fcNombreImagen"] + "' src='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + "' data-image='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + "' data-description='" + sqlResultado["fcNombreImagen"] + "' data-identificador='" + sqlResultado["fcNombreImagen"] + "'/>");
                                            break;
                                        case "4":
                                        case "6":
                                            documentacionLaboral.Append("<img alt='" + sqlResultado["fcNombreImagen"] + "' src='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + "' data-image='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + "' data-description='" + sqlResultado["fcNombreImagen"] + "' data-identificador='" + sqlResultado["fcNombreImagen"] + "'/>");
                                            break;
                                        case "7":
                                            documentacionSolicitudFisica.Append("<img alt='" + sqlResultado["fcNombreImagen"] + "' src='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + "' data-image='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + "' data-description='" + sqlResultado["fcNombreImagen"] + "' data-identificador='" + sqlResultado["fcNombreImagen"] + "'/>");
                                            break;
                                        case "8":
                                            documentacionCampoDomicilio.Append("<img alt='" + sqlResultado["fcNombreImagen"] + "' src='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + ".jpg" + "' data-image='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + ".jpg" + "' data-description='" + sqlResultado["fcNombreImagen"] + "' data-identificador='" + sqlResultado["fcNombreImagen"] + "'/>");
                                            break;
                                        case "9":
                                            documentacionCampoTrabajo.Append("<img alt='" + sqlResultado["fcNombreImagen"] + "' src='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + ".jpg" + "' data-image='" + "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen + ".jpg" + "' data-description='" + sqlResultado["fcNombreImagen"] + "' data-identificador='" + sqlResultado["fcNombreImagen"] + "'/>");
                                            break;
                                        default:
                                            break;

                                    }
                                }

                                divDocumentacionCedula.InnerHtml = documentacionIdentidad.ToString();
                                divDocumentacionCedulaModal.InnerHtml = documentacionIdentidad.ToString();
                                divDocumentacionDomicilio.InnerHtml = documentacionDomicilio.ToString();
                                divDocumentacionDomicilioModal.InnerHtml = documentacionDomicilio.ToString();
                                divDocumentacionLaboral.InnerHtml = documentacionLaboral.ToString();
                                divDocumentacionLaboral.InnerHtml += documentacionSolicitudFisica.ToString();
                                divDocumentacionLaboralModal.InnerHtml = documentacionLaboral.ToString();
                                divDocumentacionSoliFisicaModal.InnerHtml = documentacionSolicitudFisica.ToString();
                            }
                        }// using command documentos
                    } // using connection
                } // if parametros != string.empty
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
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

    public static int GetMonthDifference(DateTime startDate, DateTime endDate)
    {
        int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
        return Math.Abs(monthsApart);
    }
}