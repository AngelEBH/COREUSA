using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;

public partial class SolicitudesCredito_RegistradaDetalles : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string IdProducto = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDSolicitud = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var lcURL = string.Empty;
            var liParamStart = 0;
            var lcParametros = string.Empty;
            var lcEncriptado = string.Empty;
            var lcParametroDesencriptado = string.Empty;
            Uri lURLDesencriptado = null;

            try
            {
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
                    lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcIDSesion = "1";

                    CargarInformacionClienteSolicitud();
                }
                else
                {
                    string lcScript = "window.open('SolicitudesCredito_Bandeja.aspx?" + lcEncriptado + "','_self')";
                    Response.Write("<script>");
                    Response.Write(lcScript);
                    Response.Write("</script>");
                }
            }
            catch
            {
                string lcScript = "window.open('SolicitudesCredito_Bandeja.aspx?" + lcEncriptado + "','_self')";
                Response.Write("<script>");
                Response.Write(lcScript);
                Response.Write("</script>");
            }
        }
    }

    public void CargarInformacionClienteSolicitud()
    {
        try
        {
            var logo = string.Empty;
            var idEstadoSolicitud = string.Empty;
            var estadoSolicitud = string.Empty;
            var iconoRojo = "<i class='mdi mdi-close-circle-outline mdi-18px text-danger'></i>";
            var iconoExito = "<i class='mdi mdi-check-circle-outline mdi-18px text-success'></i>";
            var iconoPendiente = "<i class='mdi mdi-check-circle-outline mdi-18px text-warning'></i>";
            var iconoCancelado = "<i class='mdi mdi-check-circle-outline mdi-18px text-secondary'></i>";
            var procesoPendiente = DateTime.Parse("1900-01-01 00:00:00.000");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_SolicitudClientePorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            /****** Informacion de la solicitud ******/
                            idEstadoSolicitud = sqlResultado["fiEstadoSolicitud"].ToString();
                            estadoSolicitud = sqlResultado["fcEstadoSolicitud"].ToString();
                            var fechaEnInvestigacionInicio = (DateTime)sqlResultado["fdEnRutaDeInvestigacionInicio"];

                            /* Estado del procesamiento */
                            var estadoIngreso = string.Empty;
                            var estadoEnCola = string.Empty;
                            var estadoAnalisis = string.Empty;
                            var estadoCampo = string.Empty;
                            var estadoCondicionado = string.Empty;
                            var estadoReprogramado = string.Empty;
                            var estadoPasoFinal = string.Empty;
                            var estadoResolucion = string.Empty;

                            if (DateTime.Parse(sqlResultado["fdEnIngresoInicio"].ToString()) != procesoPendiente)
                            {
                                estadoIngreso = DateTime.Parse(sqlResultado["fdEnIngresoFin"].ToString()) != procesoPendiente ? iconoExito : iconoPendiente;
                            }

                            if (DateTime.Parse(sqlResultado["fdEnColaInicio"].ToString()) != procesoPendiente)
                            {
                                estadoEnCola = DateTime.Parse(sqlResultado["fdEnColaFin"].ToString()) != procesoPendiente ? iconoExito : iconoPendiente;

                                if (DateTime.Parse(sqlResultado["fdEnColaFin"].ToString()) == procesoPendiente && (idEstadoSolicitud == "4" || idEstadoSolicitud == "5" || idEstadoSolicitud == "7"))
                                {
                                    estadoEnCola = idEstadoSolicitud == "7" ? iconoExito : iconoCancelado;
                                }
                            }

                            if (DateTime.Parse(sqlResultado["fdEnAnalisisInicio"].ToString()) != procesoPendiente)
                            {
                                estadoAnalisis = DateTime.Parse(sqlResultado["fdEnAnalisisFin"].ToString()) != procesoPendiente ? iconoExito : iconoPendiente;

                                if (DateTime.Parse(sqlResultado["fdEnAnalisisFin"].ToString()) == procesoPendiente && (idEstadoSolicitud == "4" || idEstadoSolicitud == "5" || idEstadoSolicitud == "7"))
                                {
                                    estadoAnalisis = idEstadoSolicitud == "7" ? iconoExito : iconoCancelado;
                                }

                                if (idEstadoSolicitud == "4")
                                {
                                    estadoAnalisis = iconoRojo;
                                }
                            }

                            if (DateTime.Parse(sqlResultado["fdEnvioARutaAnalista"].ToString()) != procesoPendiente)
                            {
                                estadoCampo = DateTime.Parse(sqlResultado["fdEnRutaDeInvestigacionFin"].ToString()) != procesoPendiente ? iconoExito : iconoPendiente;

                                if (DateTime.Parse(sqlResultado["fdEnRutaDeInvestigacionFin"].ToString()) == procesoPendiente && (idEstadoSolicitud == "4" || idEstadoSolicitud == "5" || idEstadoSolicitud == "7"))
                                {
                                    estadoCampo = idEstadoSolicitud == "7" ? iconoExito : iconoCancelado;
                                }

                                if (idEstadoSolicitud == "5")
                                {
                                    estadoCampo = iconoRojo;
                                }
                            }

                            if (DateTime.Parse(sqlResultado["fdCondicionadoInicio"].ToString()) != procesoPendiente)
                            {
                                estadoCondicionado = DateTime.Parse(sqlResultado["fdCondificionadoFin"].ToString()) != procesoPendiente ? iconoExito : iconoPendiente;

                                if (DateTime.Parse(sqlResultado["fdCondificionadoFin"].ToString()) == procesoPendiente && (idEstadoSolicitud == "4" || idEstadoSolicitud == "5" || idEstadoSolicitud == "7"))
                                {
                                    estadoCondicionado = idEstadoSolicitud == "7" ? iconoExito : iconoCancelado;
                                }
                            }

                            if (DateTime.Parse(sqlResultado["fdReprogramadoInicio"].ToString()) != procesoPendiente)
                            {
                                estadoReprogramado = DateTime.Parse(sqlResultado["fdReprogramadoFin"].ToString()) != procesoPendiente ? iconoExito : iconoPendiente;

                                if (DateTime.Parse(sqlResultado["fdReprogramadoFin"].ToString()) == procesoPendiente && (idEstadoSolicitud == "4" || idEstadoSolicitud == "5" || idEstadoSolicitud == "7"))
                                {
                                    estadoReprogramado = idEstadoSolicitud == "7" ? iconoExito : iconoCancelado;
                                }
                            }

                            if (DateTime.Parse(sqlResultado["fdPasoFinalInicio"].ToString()) != procesoPendiente)
                            {
                                estadoPasoFinal = DateTime.Parse(sqlResultado["fdPasoFinalFin"].ToString()) != procesoPendiente ? iconoExito : iconoPendiente;

                                if (DateTime.Parse(sqlResultado["fdPasoFinalFin"].ToString()) == procesoPendiente && (idEstadoSolicitud == "4" || idEstadoSolicitud == "5" || idEstadoSolicitud == "7"))
                                {
                                    estadoPasoFinal = idEstadoSolicitud == "7" ? iconoExito : iconoCancelado;
                                }
                            }

                            if (idEstadoSolicitud == "4" || idEstadoSolicitud == "5" || idEstadoSolicitud == "7")
                            {
                                estadoResolucion = idEstadoSolicitud == "7" ? iconoExito : iconoRojo;
                            }
                            else if (DateTime.Parse(sqlResultado["fdPasoFinalInicio"].ToString()) != procesoPendiente)
                            {
                                estadoResolucion = iconoPendiente;
                            }

                            HtmlTableRow tRowEstadoProcesamiento = null;
                            tRowEstadoProcesamiento = new HtmlTableRow();
                            tRowEstadoProcesamiento.Cells.Add(new HtmlTableCell() { InnerHtml = estadoIngreso });
                            tRowEstadoProcesamiento.Cells.Add(new HtmlTableCell() { InnerHtml = estadoEnCola });
                            tRowEstadoProcesamiento.Cells.Add(new HtmlTableCell() { InnerHtml = estadoAnalisis });
                            tRowEstadoProcesamiento.Cells.Add(new HtmlTableCell() { InnerHtml = estadoCampo });
                            tRowEstadoProcesamiento.Cells.Add(new HtmlTableCell() { InnerHtml = estadoCondicionado });
                            tRowEstadoProcesamiento.Cells.Add(new HtmlTableCell() { InnerHtml = estadoReprogramado });
                            tRowEstadoProcesamiento.Cells.Add(new HtmlTableCell() { InnerHtml = estadoPasoFinal });
                            tRowEstadoProcesamiento.Cells.Add(new HtmlTableCell() { InnerHtml = estadoResolucion });
                            tblEstadoSolicitud.Rows.Add(tRowEstadoProcesamiento);

                            IdProducto = sqlResultado["fiIDProducto"].ToString();
                            lblProducto.Text = sqlResultado["fcProducto"].ToString();
                            lblNoSolicitud.Text = sqlResultado["fiIDSolicitud"].ToString();
                            lblTipoSolicitud.Text = sqlResultado["fcTipoSolicitud"].ToString();
                            lblAgenteDeVentas.Text = sqlResultado["fcNombreUsuarioAsignado"].ToString();
                            lblAgencia.Text = sqlResultado["fcNombreAgencia"].ToString();
                            lblNombreGestor.Text = sqlResultado["fcNombreGestor"].ToString();

                            /* Información del precalificado */
                            txtIngresosPrecalificado.Text = decimal.Parse(sqlResultado["fnIngresoPrecalificado"].ToString()).ToString("N");
                            txtObligacionesPrecalificado.Text = decimal.Parse(sqlResultado["fnObligacionesPrecalificado"].ToString()).ToString("N");
                            txtDisponiblePrecalificado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                            txtCapacidadDePagoMensual.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                            txtCapacidadDePagoQuincenal.Text = (decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()) / 2).ToString("N");

                            txtValorAFinanciarSeleccionado.Text = decimal.Parse(sqlResultado["fnValorSeleccionado"].ToString()).ToString("N");
                            txtMonedaSolicitada.Text = sqlResultado["fcNombreMoneda"].ToString();
                            txtValorGarantia.Text = decimal.Parse(sqlResultado["fnValorGarantia"].ToString()).ToString("N");
                            txtValorPrima.Text = decimal.Parse(sqlResultado["fnValorPrima"].ToString()).ToString("N");
                            txtPlazoSeleccionado.Text = decimal.Parse(sqlResultado["fiPlazoSeleccionado"].ToString()).ToString("N");
                            //lblTipoDePlazo_Solicitado.InnerText = sqlResultado["TipoDePlazo"].ToString();
                            lblTipoDePlazo_Solicitado.InnerText = IdProducto == "202" ? "Mensual" : "Quincenal";
                            txtOrigen.Text = sqlResultado["fcOrigen"].ToString();

                            /*** Calculo del prestamo solicitado ***/
                            txtMontoTotalAFinanciar_Calculo.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                            txtCuotaDelPrestamo_Calculo.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                            txtCuotaDelSeguro_Calculo.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                            txtCuotaGPS_Calculo.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                            txtCuotaTotal_Calculo.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                            txtCostoAparatoGPS_Calculo.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                            txtGastosDeCierre_Calculo.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");

                            /*** Prestamo Final aprobado ***/
                            if (idEstadoSolicitud == "7" || sqlResultado["fnMontoFinalFinanciar"].ToString() != string.Empty)// el id estado 7 es aprobado
                            {
                                divPrestamoFinalAprobado.Visible = true;

                                txtMontoTotalAFinanciar_FinalAprobado.Text = decimal.Parse(sqlResultado["fnMontoFinalFinanciar"].ToString()).ToString("N");
                                txtPlazoFinal_FinalAprobado.Text = decimal.Parse(sqlResultado["fiPlazoFinalAprobado"].ToString()).ToString("N");
                                //lblTipoDePlazo_FinalAprobado.InnerText = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                lblTipoDePlazo_FinalAprobado.InnerText = IdProducto == "202" ? "Mensual" : "Quincenal";
                                txtCuotaDelPrestamo_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCuotaDelSeguro_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCuotaGPS_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCuotaTotal_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCostoAparatoGPS_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtGastosDeCierre_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                            }
                            else
                            {
                                divPrestamoFinalAprobado.Visible = false;
                            }

                            var sueldoBaseReal = decimal.Parse(sqlResultado["fnSueldoBaseReal"].ToString());

                            /*** Recalculo capacidad de pago cuando los ingresos son diferentes ***/
                            if (sueldoBaseReal != 0)
                            {
                                
                                var bonosComisiones = decimal.Parse(sqlResultado["fnBonosComisionesReal"].ToString());
                                var ingresosReales = sueldoBaseReal + bonosComisiones;
                                var obligacionesPrecalificado = decimal.Parse(sqlResultado["fnObligacionesPrecalificado"].ToString());


                                txtIngresos_Recalculo.Text = ingresosReales.ToString("N");
                                txtObligaciones_Recalculo.Text = obligacionesPrecalificado.ToString("N");
                                txtDisponible_Recalculo.Text = (ingresosReales - obligacionesPrecalificado).ToString("N");
                                txtCapacidadDePagoMensual.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCapacidadDePagoQuincenal.Text = (decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()) / 2).ToString("N");

                                divRecalculoCapacidadDePago.Visible = true;

                            }


                            txtTipoDeEmpresa.Text = sqlResultado["fcTipoEmpresa"].ToString();
                            txtTipoDePerfil.Text = sqlResultado["fcTipoPerfil"].ToString();
                            txtTipoDeEmpleo.Text = sqlResultado["fcTipoEmpleado"].ToString();
                            txtBuroActual.Text = sqlResultado["fcBuroActual"].ToString();
                            txtOrigen.Text = sqlResultado["fcOrigen"].ToString();

                            /****** Documentos de la solicitud ******/
                            sqlResultado.NextResult();
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
                                idTipoDocumento = sqlResultado["fiTipoDocumento"].ToString();

                                switch (idTipoDocumento)
                                {
                                    case "1":
                                    case "2":
                                    case "18":
                                    case "19":
                                        documentacionIdentidad.Append("<img alt='" + sqlResultado["fcDescripcionTipoDocumento"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcDescripcionTipoDocumento"] + "' data-identificador='" + sqlResultado["fcDescripcionTipoDocumento"] + "'/>");
                                        break;
                                    case "3":
                                    case "5":
                                        documentacionDomicilio.Append("<img alt='" + sqlResultado["fcDescripcionTipoDocumento"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcDescripcionTipoDocumento"] + "' data-identificador='" + sqlResultado["fcDescripcionTipoDocumento"] + "'/>");
                                        break;
                                    case "4":
                                    case "6":
                                        documentacionLaboral.Append("<img alt='" + sqlResultado["fcDescripcionTipoDocumento"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcDescripcionTipoDocumento"] + "' data-identificador='" + sqlResultado["fcDescripcionTipoDocumento"] + "'/>");
                                        break;
                                    case "7":
                                        documentacionSolicitudFisica.Append("<img alt='" + sqlResultado["fcDescripcionTipoDocumento"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcDescripcionTipoDocumento"] + "' data-identificador='" + sqlResultado["fcDescripcionTipoDocumento"] + "'/>");
                                        break;
                                    case "8":
                                        documentacionCampoDomicilio.Append("<img alt='" + sqlResultado["fcDescripcionTipoDocumento"] + "' src='" + sqlResultado["fcURL"] + ".jpg" + "' data-image='" + sqlResultado["fcURL"] + ".jpg" + "' data-description='" + sqlResultado["fcDescripcionTipoDocumento"] + "' data-identificador='" + sqlResultado["fcDescripcionTipoDocumento"] + "'/>");
                                        break;
                                    case "9":
                                        documentacionCampoTrabajo.Append("<img alt='" + sqlResultado["fcDescripcionTipoDocumento"] + "' src='" + sqlResultado["fcURL"] + ".jpg" + "' data-image='" + sqlResultado["fcURL"] + ".jpg" + "' data-description='" + sqlResultado["fcDescripcionTipoDocumento"] + "' data-identificador='" + sqlResultado["fcDescripcionTipoDocumento"] + "'/>");
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

                            if (documentacionCampoDomicilio.ToString() != "")
                            {
                                divDocumentacionCampoDomicilio.InnerHtml = documentacionCampoDomicilio.ToString();
                                divDocumentacionCampoDomicilioModal.InnerHtml = documentacionCampoDomicilio.ToString();
                                divDocumentacionCampoDomicilio.Visible = true;
                                divContenedorCampoDomicilioModal.Visible = true;
                            }

                            if (documentacionCampoTrabajo.ToString() != "")
                            {
                                divDocumentacionCampoTrabajo.InnerHtml = documentacionCampoTrabajo.ToString();
                                divDocumentacionCampoTrabajoModal.InnerHtml = documentacionCampoTrabajo.ToString();
                                divDocumentacionCampoTrabajo.Visible = true;
                                divContenedorCampoTrabajoModal.Visible = true;
                            }

                            /****** Condicionamientos de la solicitud ******/
                            sqlResultado.NextResult();

                            if (sqlResultado.HasRows)
                            {
                                pestanaListaSolicitudCondiciones.Style.Add("display", "");

                                HtmlTableRow tRowSolicitudCondiciones = null;
                                var EstadoCondicion = string.Empty;
                                while (sqlResultado.Read())
                                {
                                    EstadoCondicion = (bool)sqlResultado["fbEstadoCondicion"] == false ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-danger mb-0'>Pendiente</label>";
                                    tRowSolicitudCondiciones = new HtmlTableRow();
                                    tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcCondicion"].ToString() });
                                    tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcDescripcionCondicion"].ToString() });
                                    tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcComentarioAdicional"].ToString() });
                                    tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerHtml = EstadoCondicion });
                                    tblListaSolicitudCondiciones.Rows.Add(tRowSolicitudCondiciones);
                                }
                            }

                            /****** Información del cliente ******/
                            sqlResultado.NextResult();
                            sqlResultado.Read();

                            var fechaNacimientoCliente = (DateTime)sqlResultado["fdFechaNacimientoCliente"];

                            /* Calcular edad del cliente */
                            var hoy = DateTime.Today;
                            var edad = hoy.Year - fechaNacimientoCliente.Year;

                            if (fechaNacimientoCliente.Date > hoy.AddYears(-edad)) 
                                edad--;
                            
                            lblNombreCliente.Text = sqlResultado["fcPrimerNombreCliente"].ToString() + " " + sqlResultado["fcSegundoNombreCliente"].ToString() + " " + sqlResultado["fcPrimerApellidoCliente"].ToString() + " " + sqlResultado["fcSegundoApellidoCliente"].ToString();
                            lblIdentidadCliente.Text = sqlResultado["fcIdentidadCliente"].ToString();
                            txtRTNCliente.Text = sqlResultado["fcRTN"].ToString();
                            txtTelefonoCliente.Text = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                            txtNacionalidad.Text = sqlResultado["fcDescripcionNacionalidad"].ToString();
                            txtFechaNacimientoCliente.Text = fechaNacimientoCliente.ToString("MM/dd/yyyy");
                            txtEdadCliente.Text = edad.ToString() + " " + "años";
                            txtCorreoCliente.Text = sqlResultado["fcCorreoElectronicoCliente"].ToString();
                            txtProfesionCliente.Text = sqlResultado["fcProfesionOficioCliente"].ToString();
                            txtSexoCliente.Text = sqlResultado["fcSexoCliente"].ToString() == "M" ? "Masculino" : "Femenino";
                            txtEstadoCivilCliente.Text = sqlResultado["fcDescripcionEstadoCivil"].ToString();

                            txtTipoDeVivienda.Text = sqlResultado["fcDescripcionViviEnda"].ToString();
                            txtTiempoDeResidir.Text = sqlResultado["fcTiempoDeResidir"].ToString();

                            /****** Información laboral ******/
                            sqlResultado.NextResult();
                            sqlResultado.Read();

                            int arraigoLaboralMeses = GetMonthDifference(hoy, DateTime.Parse(sqlResultado["fdFechaIngreso"].ToString()));


                            txtNombreTrabajoCliente.Text = sqlResultado["fcNombreTrabajo"].ToString();
                            txtPuestoAsignado.Text = sqlResultado["fcPuestoAsignado"].ToString();
                            txtIngresosMensuales.Text = decimal.Parse(sqlResultado["fnIngresosMensuales"].ToString()).ToString("N");
                            txtFechaIngreso.Text = DateTime.Parse(sqlResultado["fdFechaIngreso"].ToString()).ToString("MM/dd/yyyy");
                            txtArraigoLaboral.Text = arraigoLaboralMeses.ToString() + " meses";
                            txtTelefonoEmpresa.Text = sqlResultado["fcTelefonoEmpresa"].ToString();
                            txtExtensionCliente.Text = sqlResultado["fcExtensionCliente"].ToString();
                            txtExtensionRecursosHumanos.Text = sqlResultado["fcExtensionRecursosHumanos"].ToString();
                            txtDepartamentoEmpresa.Text = sqlResultado["fcDepartamento"].ToString();
                            txtMunicipioEmpresa.Text = sqlResultado["fcMunicipio"].ToString();
                            txtCiudadPobladoEmpresa.Text = sqlResultado["fcPoblado"].ToString();
                            txtBarrioColoniaEmpresa.Text = sqlResultado["fcBarrio"].ToString();
                            txtDireccionDetalladaEmpresa.InnerText = sqlResultado["fcDireccionDetalladaEmpresa"].ToString();
                            txtReferenciaDetalladaEmpresa.InnerText = sqlResultado["fcReferenciasDireccionDetalladaEmpresa"].ToString();
                            txtFuenteDeOtrosIngresos.Text = sqlResultado["fcFuenteOtrosIngresos"].ToString();
                            txtValorDeOtrosIngresos.Text = decimal.Parse(sqlResultado["fnValorOtrosIngresosMensuales"].ToString()).ToString("N");

                            if (sqlResultado["fiIDInvestigacionDeCampo"].ToString() != "0")
                            {
                                lblResolucionCampoTrabajo.InnerText = sqlResultado["fcResultadodeCampo"].ToString();
                                txtGestorValidador_Trabajo.Text = sqlResultado["fcGestorValidadorTrabajo"].ToString();
                                txtResultadoInvestigacion_Trabajo.Text = sqlResultado["fcGestionTrabajo"].ToString();
                                txtFechaRecibida_Trabajo.Text = fechaEnInvestigacionInicio.ToString("MM/dd/yyyy hh:mm tt");
                                txtFechaRendida_Trabajo.Text = DateTime.Parse(sqlResultado["fdFechaValidacion"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
                                txtObservacionesDeCampo_Trabajo.InnerText = sqlResultado["fcObservacionesCampo"].ToString();
                            }

                            /****** Informacion domicilio ******/
                            sqlResultado.NextResult();
                            sqlResultado.Read();
                            
                            txtDepartamentoDomicilio.Text = sqlResultado["fcDepartamento"].ToString();
                            txtMunicipioDomicilio.Text = sqlResultado["fcMunicipio"].ToString();
                            txtCiudadPobladoDomicilio.Text = sqlResultado["fcPoblado"].ToString();
                            txtBarrioColoniaDomicilio.Text = sqlResultado["fcBarrio"].ToString();
                            txtDireccionDetalladaDomicilio.InnerText = sqlResultado["fcDireccionDetalladaDomicilio"].ToString();
                            txtReferenciasDomicilio.InnerText = sqlResultado["fcReferenciasDireccionDetalladaDomicilio"].ToString();

                            if (sqlResultado["fiIDInvestigacionDeCampo"].ToString() != "0")
                            {
                                lblResolucionCampoDomicilio.InnerText = sqlResultado["fcResultadodeCampo"].ToString();
                                txtGestorValidador_Domicilio.Text = sqlResultado["fcGestorValidadorDomicilio"].ToString();
                                txtResultadoInvestigacionCampo_Domicilio.Text = sqlResultado["fcGestionDomicilio"].ToString();
                                txtFechaRecibida_Domicilio.Text = fechaEnInvestigacionInicio.ToString("MM/dd/yyyy hh:mm tt");
                                txtFechaValidacion_Domicilio.Text = DateTime.Parse(sqlResultado["fdFechaValidacion"].ToString()).ToString("MM/dd/yyyy hh:mm tt");
                                txtObservacionesDeCampo_Domicilio.InnerText = sqlResultado["fcObservacionesCampo"].ToString();
                            }

                            /****** Informacion del conyugue ******/
                            sqlResultado.NextResult();
                            sqlResultado.Read();
                            if (!sqlResultado.HasRows)
                            {
                                divPanelInformacionConyugal.Visible = false;
                            }
                            else
                            {
                                txtNombreDelConyugue.Text = sqlResultado["fcNombreCompletoConyugue"].ToString();
                                txtIdentidadConyugue.Text = sqlResultado["fcIndentidadConyugue"].ToString();
                                txtTelefonoConyugue.Text = sqlResultado["fcTelefonoConyugue"].ToString();
                                txtFechaNacimientoConyugue.Text = DateTime.Parse(sqlResultado["fdFechaNacimientoConyugue"].ToString()).ToString("MM/dd/yyyy");
                                txtLugarDeTrabajoConyugue.Text = sqlResultado["fcLugarTrabajoConyugue"].ToString();
                                txtTelefonoTrabajoConyugue.Text = sqlResultado["fcTelefonoConyugue"].ToString();
                                txtIngresosMensualesConyugue.Text = decimal.Parse(sqlResultado["fnIngresosMensualesConyugue"].ToString()).ToString("N");
                            }

                            /****** Referencias de la solicitud ******/
                            sqlResultado.NextResult();

                            HtmlTableRow tRowReferencias = null;
                            var btnComentarioReferenciaPersonal = string.Empty;
                            var colorClass = string.Empty;

                            while (sqlResultado.Read())
                            {
                                btnComentarioReferenciaPersonal = "<button type='button' id='btnComentarioReferencia' data-id='" + sqlResultado["fiIDReferencia"].ToString() + "' data-comment='" + sqlResultado["fcComentarioDeptoCredito"].ToString() + "' data-nombreref='" + sqlResultado["fcNombreCompletoReferencia"].ToString() + "' class='btn mdi mdi-comment' title='Ver observaciones del depto. de crédito'></button>";

                                tRowReferencias = new HtmlTableRow();
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcNombreCompletoReferencia"].ToString() });
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcLugarTrabajoReferencia"].ToString() });
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcTiempoDeConocer"].ToString() });
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcTelefonoReferencia"].ToString() });
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcDescripcionParentesco"].ToString() });
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerHtml = sqlResultado["fcComentarioDeptoCredito"].ToString() != "Sin comunicacion" ? "<i class='mdi mdi-check-circle-outline'></i>" : "<i class='mdi mdi-call-missed'></i>" });
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerHtml = btnComentarioReferenciaPersonal });

                                colorClass = sqlResultado["fcComentarioDeptoCredito"].ToString() != "Sin comunicacion" ? "tr-exito" : "text-danger";

                                tRowReferencias.Attributes.Add("class",colorClass);
                                tblReferencias.Rows.Add(tRowReferencias);
                            }
                        }
                    }
                }
            }

            logo = IdProducto == "101" ? "iconoRecibirDinero48.png" : IdProducto == "201" ? "iconoMoto48.png" : IdProducto == "202" ? "iconoAuto48.png" : IdProducto == "301" ? "iconoConsumo48.png" : "iconoConsumo48.png";
            imgLogo.ImageUrl = "/Imagenes/" + logo;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    [WebMethod]
    public static SolicitudAnalisisViewModel CargarInformacionSolicitud()
    {
        var DSC = new DSCore.DataCrypt();
        var ObjSolicitud = new SolicitudAnalisisViewModel();
        try
        {
            var lcURL = HttpContext.Current.Request.Url.ToString();
            var lURLDesencriptado = DesencriptarURL(lcURL);
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            var pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                #region SOLICITUD MAESTRO
                var SolicitudMaestro = new BandejaSolicitudesViewModel();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
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
                    }
                }
                ObjSolicitud.solicitud = SolicitudMaestro;
                #endregion

                #region DOCUMENTOS DE LA SOLICITUD
                var ListadoDocumentos = new List<SolicitudesDocumentosViewModel>();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ObtenerSolicitudDocumentos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
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
                        ObjSolicitud.documentos = ListadoDocumentos;
                    }
                }
                #endregion

                var objCliente = new ClientesViewModel();

                #region CLIENTES MASTER
                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", ObjSolicitud.solicitud.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
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
                #endregion

                #region CLIENTE INFORMACION LABORAL
                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Laboral_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
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
                #endregion

                #region CLIENTE INFORMACION CONYUGAL
                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionConyugal_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSOlicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
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
                #endregion

                #region CLIENTE INFORMACION DE DOMICILIO
                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionDomiciliar_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
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
                #endregion

                #region CLIENTE REFERENCIAS PERSONALES
                objCliente.ClientesReferenciasPersonales = new List<ClientesReferenciasViewModel>();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Referencias_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
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
                #endregion

                ObjSolicitud.cliente = objCliente;
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return ObjSolicitud;
    }

    [WebMethod]
    public static BandejaSolicitudesViewModel CargarEstadoSolicitud()
    {
        var objEstadoSolicitud = new BandejaSolicitudesViewModel();
        var DSC = new DSCore.DataCrypt();
        try
        {
            var lcURL = HttpContext.Current.Request.Url.ToString();
            var lURLDesencriptado = DesencriptarURL(lcURL);
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            var pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_SolicitudEstadoProcesamiento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
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
        var lURLDesencriptado = DesencriptarURL(dataCrypt);
        var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
        var pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
        var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp").ToString();
        var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID").ToString() ?? "1";

        var parametrosEncriptados = DSC.Encriptar("usr=" + pcIDUsuario + "&ID=" + pcID + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion);

        return parametrosEncriptados;
    }

    [WebMethod]
    public static PrecalificadoViewModel GetPrestamosSugeridos(decimal ingresos, decimal obligaciones, string codigoProducto)
    {
        var DSC = new DSCore.DataCrypt();
        var objPrecalificado = new PrecalificadoViewModel();
        var listaCotizadorProductos = new List<cotizadorProductosViewModel>();
        var lcURL = HttpContext.Current.Request.Url.ToString();
        var lURLDesencriptado = DesencriptarURL(lcURL);
        var identidad = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
        var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr").ToString();

        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("EXEC CoreFinanciero.dbo.sp_CotizadorProductos @piIDUsuario, @piIDProducto, @pcIdentidad, @piConObligaciones, @pnIngresosBrutos, @pnIngresosDisponibles", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", codigoProducto);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", identidad);
                    sqlComando.Parameters.AddWithValue("@piConObligaciones", obligaciones == 0 ? "0" : "1");
                    sqlComando.Parameters.AddWithValue("@pnIngresosBrutos", ingresos);
                    sqlComando.Parameters.AddWithValue("@pnIngresosDisponibles", ingresos - obligaciones);

                    using (var reader = sqlComando.ExecuteReader())
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
                }
            }
            objPrecalificado.cotizadorProductos = listaCotizadorProductos;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return objPrecalificado;
    }

    [WebMethod]
    public static CalculoPrestamoViewModel CalculoPrestamo(decimal MontoFinanciar, decimal PlazoFinanciar, decimal ValorPrima)
    {
        CalculoPrestamoViewModel objCalculo = null;
        var DSC = new DSCore.DataCrypt();
        try
        {
            var lcURL = HttpContext.Current.Request.Url.ToString();
            var lURLDesencriptado = DesencriptarURL(lcURL);
            var IDUSR = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var fechaActual = DateTime.Now;
            var MensajeError = string.Empty;
            var IDPRODUCTO = 0;

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_ListarSolicitudesCredito", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);

                    using (var reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            IDPRODUCTO = (int)reader["fiIDTipoProducto"];
                    }
                }

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_CalculoPrestamo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDProducto", IDPRODUCTO);
                    sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", MontoFinanciar);
                    sqlComando.Parameters.AddWithValue("@liPlazo", PlazoFinanciar);
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", ValorPrima);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);

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
            var DSC = new DSCore.DataCrypt();
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

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
            return default(T);
        else
            return (T)obj;
    }

    public static int GetMonthDifference(DateTime startDate, DateTime endDate)
    {
        int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
        return Math.Abs(monthsApart);
    }
}