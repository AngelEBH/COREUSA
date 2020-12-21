using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;

public partial class Solicitudes_CANEX_Detalles : System.Web.UI.Page
{
    public string pcEncriptado = "";
    public string pcIDUsuario = "";
    public string pcIDSesion = "";
    public string pcIDApp = "";

    public short IdPais = 0;
    public short IdSocio = 0;
    public short IdAgencia = 0;
    public int IdSolicitudPrestadito = 0;
    public decimal IdEstadoSolicitud = 0;
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                var idSolicitud = "0";
                var lcURL = string.Empty;
                var liParamStart = 0;
                var lcParametros = string.Empty;
                var lcParametroDesencriptado = string.Empty;
                Uri lURLDesencriptado = null;

                lcURL = Request.Url.ToString();
                liParamStart = lcURL.IndexOf("?");

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

                                    /* Verificar si la solicitud estaba en estado "enviada" y hay que pasarla a estatus "en revision" */
                                    if (estadoSolicitud == 2)
                                    {
                                        CambiarEstadoAEnRevision(idSolicitudCanex);
                                    }

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
                                        divComisionesCliente.Visible = false;
                                    else
                                        txtComisionesCliente.Text = sqlResultado["fnIngresoMensualComisiones"].ToString();

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
                                        string desicion = estadoSolicitud == estadoAprobada ? "Aprobada" : "Rechazada";
                                        title = "La solicitud ya fue " + desicion;
                                        colorClass = estadoSolicitud == estadoAprobada ? "text-success" : "text-danger";

                                        btnAceptarSolicitud.Disabled = true;
                                        btnRechazar.Disabled = true;
                                        btnCondicionarSolicitud.Disabled = true;
                                    }
                                    else if (IdSolicitudPrestadito != 0)
                                    {
                                        title = "La solicitud ya fue importada";
                                        btnAceptarSolicitud.Disabled = true;
                                        btnRechazar.Disabled = true;
                                        btnCondicionarSolicitud.Disabled = true;
                                        colorClass = "text-success";
                                    }
                                    else if (estadoSolicitud == estadoEnRevision && IdSolicitudPrestadito == 0)
                                    {
                                        btnAceptarSolicitud.Disabled = false;
                                        colorClass = "text-warning";
                                    }
                                    else if (estadoSolicitud != estadoRechazada && estadoSolicitud != estadoAprobada && IdSolicitudPrestadito == 0)
                                    {
                                        btnRechazar.Disabled = false;
                                        colorClass = "text-warning";
                                    }

                                    btnAceptarSolicitud.Attributes.Add("title", title);
                                    btnRechazar.Attributes.Add("title", title);
                                    btnCondicionarSolicitud.Attributes.Add("title", title);

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
                                    pestanaListaSolicitudCondiciones.Style.Add("display", "");

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
                                }
                            }
                        }

                        /* Catalogo de condiciones */
                        using (var sqlComando = new SqlCommand("sp_CANEX_Catalogo_Condiciones", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                            using (var AdapterDDLCondiciones = new SqlDataAdapter(sqlComando))
                            {
                                var dtCondiciones = new DataTable();

                                AdapterDDLCondiciones.Fill(dtCondiciones);
                                ddlCondiciones.DataSource = dtCondiciones;
                                ddlCondiciones.DataBind();

                                ddlCondiciones.DataTextField = "fcDescripcion";
                                ddlCondiciones.DataValueField = "fiID";
                                ddlCondiciones.DataBind();
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

    private void CambiarEstadoAEnRevision(int idSolicitudCanex)
    {
        const int estadoEnRevision = 3;
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CANEX_Solicitud_CambiarEstado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitudCanex);
                    sqlComando.Parameters.AddWithValue("@piIDPais", IdPais);
                    sqlComando.Parameters.AddWithValue("@piIDSocio", IdSocio);
                    sqlComando.Parameters.AddWithValue("@piIDAgencia", IdAgencia);
                    sqlComando.Parameters.AddWithValue("@IDEstadoNuevo", estadoEnRevision);
                    sqlComando.ExecuteReader();
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    [WebMethod]
    public static bool CondicionarSolicitud(List<SolicitudesCondicionamientosViewModel> solicitudCondiciones, int idPais, int idSocio, int idAgencia, string dataCrypt)
    {
        var resultadoProceso = false;

        using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
        {
            sqlConexion.Open();

            using (var transaccion = sqlConexion.BeginTransaction("insercionSolicitudCondiciones"))
            {
                try
                {
                    var lURLDesencriptado = DesencriptarURL(dataCrypt);
                    var idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
                    var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

                    foreach (SolicitudesCondicionamientosViewModel item in solicitudCondiciones)
                    {
                        using (var sqlComandoList = new SqlCommand("sp_CANEX_Solicitud_Condicionar", sqlConexion, transaccion))
                        {
                            sqlComandoList.CommandType = CommandType.StoredProcedure;
                            sqlComandoList.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComandoList.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComandoList.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComandoList.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                            sqlComandoList.Parameters.AddWithValue("@piIDPais", idPais);
                            sqlComandoList.Parameters.AddWithValue("@piIDSocio", idSocio);
                            sqlComandoList.Parameters.AddWithValue("@piIDAgencia", idAgencia);
                            sqlComandoList.Parameters.AddWithValue("@IDCondicion", item.fiIDCondicion);
                            sqlComandoList.Parameters.AddWithValue("@Observaciones", item.fcComentarioAdicional);

                            using (var readerList = sqlComandoList.ExecuteReader())
                            {
                                while (readerList.Read())
                                {
                                    if ((long)readerList["RESULT"] == 1)
                                    {
                                        resultadoProceso = true;
                                    }
                                    else
                                    {
                                        resultadoProceso = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }

                    if (resultadoProceso == true)
                        transaccion.Commit();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    ex.Message.ToString();
                }
            }// using transaction
        } // using connection
        return resultadoProceso;
    }

    [WebMethod]
    public static int RechazarSolicitud(int idPais, int idSocio, int idAgencia, string comentario, string dataCrypt)
    {
        int resultadoProceso = 0;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            long mensajeError = 0;

            const int estadoRechazado = 5;
            int resolucion = estadoRechazado;

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CANEX_SolicitudResolucion", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDPais", idPais);
                    sqlComando.Parameters.AddWithValue("@piIDSocio", idSocio);
                    sqlComando.Parameters.AddWithValue("@piIDAgencia", idAgencia);
                    sqlComando.Parameters.AddWithValue("@IDEstadoNuevo", resolucion);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitudPrestadito", "0");
                    sqlComando.Parameters.AddWithValue("@pcComentario", comentario.Trim());

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                            mensajeError = (long)sqlResultado["RESULT"];

                        if (mensajeError == 1)
                            resultadoProceso = Convert.ToInt32(mensajeError);
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
    public static ResponseEntitie ImportarSolicitud(int idPais, int idSocio, int idAgencia, string comentario, string dataCrypt)
    {
        var resultadoProceso = new ResponseEntitie();
        var ListadoDocumentosCANEX = new List<SolicitudesDocumentosViewModel>();

        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            var resultadoSp = false;
            var mensajeResultado = string.Empty;
            var mensajeError = string.Empty;
            var idSolicitudPrestadito = 0;
            var contadorErrores = 0;

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CANEX_Solicitud_ImportarSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitudCANEX", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDPais", idPais);
                    sqlComando.Parameters.AddWithValue("@piIDSocio", idSocio);
                    sqlComando.Parameters.AddWithValue("@piIDAgencia", idAgencia);
                    sqlComando.Parameters.AddWithValue("@pcComentario", comentario.Trim());

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultadoProceso.idInsertado = (int)sqlResultado["fiIDSolicitud"];
                            resultadoProceso.message = "MensajeResultado:" + (string)sqlResultado["fcMensajeResultado"] + " | fcMensajeError" + (string)sqlResultado["fcMensajeError"]; ;
                            resultadoProceso.response = (bool)sqlResultado["fbTipoResultado"];

                            resultadoSp = (bool)sqlResultado["fbTipoResultado"];
                            mensajeResultado = (string)sqlResultado["fcMensajeResultado"];
                            mensajeError = (string)sqlResultado["fcMensajeError"];
                            idSolicitudPrestadito = (int)sqlResultado["fiIDSolicitud"];
                        }
                    }
                }

                /* si se importó todo correctamente, guardar la documentacion */
                if (resultadoSp == true)
                {
                    using (var sqlComando = new SqlCommand("sp_CANEX_Solicitud_Documentos", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            var nombreSocio = string.Empty;
                            var nombreImagen = string.Empty;

                            while (sqlResultado.Read())
                            {
                                nombreSocio = sqlResultado["fcNombreSocio"].ToString();
                                nombreImagen = sqlResultado["fcNombreImagen"].ToString();

                                /* Lista de documentos canex, que se deben mover a la carpeta de documentos de solicitudes */
                                ListadoDocumentosCANEX.Add(new SolicitudesDocumentosViewModel()
                                {
                                    fiIDSolicitudDocs = (short)sqlResultado["fiIDImagen"],
                                    NombreAntiguo = (string)sqlResultado["fcNombreImagen"],
                                    URLAntiguoArchivo = "http://canex.miprestadito.com/documentos/" + nombreSocio + "/SOL_" + idSolicitud + "/" + nombreImagen,
                                    fiTipoDocumento = (short)sqlResultado["fiIDImagen"]
                                });
                            }
                        }
                    }// using cmd

                    int documentacionCliente = 1;
                    int documentacionAval = 2;
                    int tipoDocumentacion = 0;

                    int[] idsDocumentosAval = new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 20, 21 };
                    int[] idsDocumentosCliente = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 18, 19 };

                    if (idsDocumentosAval.Contains(documentacionAval))
                    {
                        tipoDocumentacion = documentacionAval;
                    }
                    else
                    {
                        tipoDocumentacion = documentacionCliente;
                    }

                    string nombreCarpetaDocumentos = "Solicitud" + idSolicitudPrestadito;
                    string nuevoNombreDocumento = "";

                    /* lista de documentos de la solicitud de canex */
                    var listaDocumentos = ListadoDocumentosCANEX;

                    /* Lista de documentos que se va registrar en la base de datos de credito y se va mover al nuevo directorio */
                    var solicitudesDocumentos = new List<SolicitudesDocumentosViewModel>();


                    if (listaDocumentos != null)
                    {
                        /* lista de bloques y la cantidad de documentos que contiene cada uno */
                        var bloques = listaDocumentos.GroupBy(TipoDocumento => TipoDocumento.fiTipoDocumento).Select(x => new { x.Key, Count = x.Count() });

                        /* lista donde se guardara temporalmente los documentos dependiendo del tipo de documento en el iterador */
                        var documentosBloque = new List<SolicitudesDocumentosViewModel>();

                        var nombreCarpetaDocumentosCANEX = "Solicitud" + idSolicitudPrestadito;
                        var directorioDocumentosSolicitudCANEX = @"C:\inetpub\wwwroot\Documentos\Solicitudes\" + nombreCarpetaDocumentosCANEX + "\\";

                        foreach (var bloque in bloques)
                        {
                            int tipoDocumento = (int)bloque.Key;
                            int cantidadDocumentos = bloque.Count;

                            documentosBloque = listaDocumentos.Where(x => x.fiTipoDocumento == tipoDocumento).ToList();// documentos de este bloque
                            string[] nombresGenerador = Funciones.MultiNombres.GenerarNombreCredDocumento(documentacionCliente, idSolicitudPrestadito, tipoDocumento, cantidadDocumentos);

                            int contadorNombre = 0;

                            foreach (SolicitudesDocumentosViewModel file in documentosBloque)
                            {
                                nuevoNombreDocumento = nombresGenerador[contadorNombre];

                                solicitudesDocumentos.Add(new SolicitudesDocumentosViewModel()
                                {
                                    fcNombreArchivo = nuevoNombreDocumento,
                                    NombreAntiguo = file.NombreAntiguo,
                                    fcRutaArchivo = directorioDocumentosSolicitudCANEX,
                                    URLArchivo = "/Documentos/Solicitudes/" + nombreCarpetaDocumentos + "/" + nuevoNombreDocumento + ".png",
                                    URLAntiguoArchivo = file.URLAntiguoArchivo,
                                    fiTipoDocumento = file.fiTipoDocumento
                                });
                                contadorNombre++;
                            }
                        }
                    }
                    else
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al registrar la documentación";
                        return resultadoProceso;
                    }
                    if (solicitudesDocumentos.Count <= 0)
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar documentación, compruebe que los documentos se hayan cargado correctamente";
                        return resultadoProceso;
                    }

                    foreach (SolicitudesDocumentosViewModel documento in solicitudesDocumentos)
                    {
                        using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Documentos_Guardar", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitudPrestadito);
                            sqlComando.Parameters.AddWithValue("@pcNombreArchivo", documento.fcNombreArchivo);
                            sqlComando.Parameters.AddWithValue("@pcTipoArchivo", ".png");
                            sqlComando.Parameters.AddWithValue("@pcRutaArchivo", documento.fcRutaArchivo);
                            sqlComando.Parameters.AddWithValue("@pcURL", documento.URLArchivo);
                            sqlComando.Parameters.AddWithValue("@piTipoDocumento", documento.fiTipoDocumento);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al registrar documentación de la solicitud";
                            return resultadoProceso;
                        }
                    }
                    /* Mover documentos al directorio de la solicitud */
                    if (!ImportarDocumentosCANEX(idSolicitudPrestadito, solicitudesDocumentos))
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar la documentación de la solicitud";
                        return resultadoProceso;
                    }
                } // if SP == true
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static string ObtenerUrlEncriptado(string dataCrypt)
    {
        var lURLDesencriptado = DesencriptarURL(dataCrypt);
        var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
        var pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
        var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp").ToString();

        var parametrosEncriptados = DSC.Encriptar("usr=" + pcIDUsuario + "&ID=" + pcID + "&IDApp=" + pcIDApp);
        return parametrosEncriptados;
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = 0;
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;
            liParamStart = URL.IndexOf("?");

            if (liParamStart > 0)
            {
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            }
            else
            {
                lcParametros = string.Empty;
            }

            if (lcParametros != string.Empty)
            {
                pcEncriptado = URL.Substring((liParamStart + 1), URL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return lURLDesencriptado;
    }

    /* Descargar y guardar los documentos de la solicitud en su respectiva carpeta de documentos */
    public static bool ImportarDocumentosCANEX(int idSolicitud, List<SolicitudesDocumentosViewModel> listaDocumentos)
    {
        bool result;
        try
        {
            var client = new WebClient();
            var md5ArchivoDescargado = MD5.Create();

            if (listaDocumentos != null)
            {
                /* Crear el nuevo directorio para los documentos de la solicitud  */
                var nombreCarpetaDocumentos = "Solicitud" + idSolicitud;
                var directorioDocumentosSolicitud = @"C:\inetpub\wwwroot\Documentos\Solicitudes\" + nombreCarpetaDocumentos + "\\";
                var carpetaExistente = Directory.Exists(directorioDocumentosSolicitud);

                if (!carpetaExistente)
                    Directory.CreateDirectory(directorioDocumentosSolicitud);

                foreach (SolicitudesDocumentosViewModel Documento in listaDocumentos)
                {
                    var viejoDirectorio = Documento.URLAntiguoArchivo;
                    var nuevoNombreDocumento = Documento.fcNombreArchivo;
                    var nuevoDirectorio = directorioDocumentosSolicitud + nuevoNombreDocumento + ".png";

                    if (File.Exists(nuevoDirectorio))
                        File.Delete(nuevoDirectorio);

                    if (!File.Exists(nuevoDirectorio))
                    {
                        var lcURL = Documento.URLAntiguoArchivo;

                        client.DownloadFile(new Uri(lcURL), nuevoDirectorio);
                        client.Dispose();
                        client = null;
                        client = new WebClient();
                    }
                }
            }
            result = true;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            result = false;
        }
        return result;
    }

    public static int GetMonthDifference(DateTime startDate, DateTime endDate)
    {
        int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
        return Math.Abs(monthsApart);
    }
}