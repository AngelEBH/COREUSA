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
    public string pcID = "";
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
                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

                    CargarInformacionClienteSolicitud();
                    CargarInformacionGarantia();
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
            var estadoSolicitud = string.Empty;
            var idEstadoSolicitud = string.Empty;
            var procesoPendiente = DateTime.Parse("1900-01-01 00:00:00.000");
            var iconoRojo = "<i class='mdi mdi-close-circle-outline mdi-18px text-danger'></i>";
            var iconoExito = "<i class='mdi mdi-check-circle-outline mdi-18px text-success'></i>";
            var iconoPendiente = "<i class='mdi mdi-check-circle-outline mdi-18px text-warning'></i>";
            var iconoCancelado = "<i class='mdi mdi-check-circle-outline mdi-18px text-secondary'></i>";

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

                            /* Informacion del prestamo solicitado */
                            txtValorAFinanciarSeleccionado.Text = decimal.Parse(sqlResultado["fnValorSeleccionado"].ToString()).ToString("N");
                            txtMonedaSolicitada.Text = sqlResultado["fcNombreMoneda"].ToString();
                            txtValorGarantia.Text = decimal.Parse(sqlResultado["fnValorGarantia"].ToString()).ToString("N");
                            txtValorPrima.Text = decimal.Parse(sqlResultado["fnValorPrima"].ToString()).ToString("N");
                            txtPlazoSeleccionado.Text = sqlResultado["fiPlazoSeleccionado"].ToString();
                            lblTipoDePlazo_Solicitado.InnerText = (IdProducto == "202" || IdProducto == "203" || IdProducto == "204") ? "Mensual" : "Quincenal";
                            txtOrigen.Text = sqlResultado["fcOrigen"].ToString();

                            /*** Calculo del prestamo SOLICITADO ***/
                            var montoPrestamoSolicitado = decimal.Parse(sqlResultado["fnValorGarantia"].ToString()) != 0 ? decimal.Parse(sqlResultado["fnValorGarantia"].ToString()) : decimal.Parse(sqlResultado["fnValorSeleccionado"].ToString());
                            var valorPrimaPrestamoSolicitado = decimal.Parse(sqlResultado["fnValorPrima"].ToString());
                            var plazoSeleccionado = int.Parse(sqlResultado["fiPlazoSeleccionado"].ToString());

                            var calculoPrestamoSolicitado = new SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel();

                            if (IdProducto == "101" || IdProducto == "301" || IdProducto == "201" || IdProducto == "302")
                            {
                                calculoPrestamoSolicitado = CalcularPrestamo(int.Parse(IdProducto), montoPrestamoSolicitado, valorPrimaPrestamoSolicitado, plazoSeleccionado, sqlConexion);

                                txtMontoTotalAFinanciar_Calculo.Text = calculoPrestamoSolicitado.ValorAFinanciar.ToString("N");
                                txtCuotaDelPrestamo_Calculo.Text = calculoPrestamoSolicitado.ValorCuotaPrestamo.ToString("N");
                                txtCuotaDelSeguro_Calculo.Text = calculoPrestamoSolicitado.ValorCuotaSeguroDeVehiculo.ToString("N");
                                txtCuotaGPS_Calculo.Text = calculoPrestamoSolicitado.ValorCuotaServicioGPS.ToString("N");
                                txtCuotaTotal_Calculo.Text = calculoPrestamoSolicitado.ValorCuotaNeta.ToString("N");
                                txtCostoAparatoGPS_Calculo.Text = calculoPrestamoSolicitado.CostoAparatoGPS.ToString("N");
                                txtGastosDeCierre_Calculo.Text = calculoPrestamoSolicitado.ValorGastosDeCierre.ToString("N");
                                txtTasaAnualAplicada_Calculo.Text = calculoPrestamoSolicitado.TasaAnualAplicada.ToString("N");
                                txtTasaMensualAplicada_Calculo.Text = calculoPrestamoSolicitado.TasaMensualAplicada.ToString("N");

                            }
                            else if (IdProducto == "202" || IdProducto == "203" || IdProducto == "204")
                            {
                                /* Haciendo pruebas, si el prestamo es 202, 203 o 204 no se mostrará préstamo solicitado
                                * solo se mostrará el div del monto final a financiar actual
                                * mismo que se va a extraer de la tabla CredSolicitud_InformacionPrestamo
                                */
                                divCalculoPrestamoSolicitado.Visible = false;
                            }

                            var montoFinalAFinanciar = decimal.Parse(sqlResultado["fnMontoFinalFinanciar"].ToString());

                            /*** Prestamo FINAL APROBADO ***/
                            if (montoFinalAFinanciar != 0 || IdProducto == "202" || IdProducto == "203" || IdProducto == "204")
                            {
                                var valorTotalFinalAFinanciar = 0m;
                                var valorPrimaFinal = 0m;
                                var plazoFinal = 0;
                                var calculoPrestamoFinal = new SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel();

                                if (IdProducto == "101" || IdProducto == "301" || IdProducto == "201" || IdProducto == "302")
                                {
                                    valorTotalFinalAFinanciar = montoFinalAFinanciar;
                                    valorPrimaFinal = decimal.Parse(sqlResultado["fnValorPrima"].ToString());
                                    plazoFinal = int.Parse(sqlResultado["fiPlazoFinalAprobado"].ToString());

                                    calculoPrestamoFinal = CalcularPrestamo(int.Parse(IdProducto), montoPrestamoSolicitado, valorPrimaPrestamoSolicitado, plazoSeleccionado, sqlConexion);
                                }
                                else if (IdProducto == "202" || IdProducto == "203" || IdProducto == "204")
                                {
                                    calculoPrestamoFinal = CargarPrestamoSolicitadoVehiculo(IdProducto, montoPrestamoSolicitado, valorPrimaPrestamoSolicitado, plazoSeleccionado, sqlConexion);

                                    valorTotalFinalAFinanciar = calculoPrestamoFinal.ValorAFinanciar;
                                    plazoFinal = calculoPrestamoFinal.Plazo;
                                }

                                lblEstadoDelMontoFinalAFinanciar.InnerText = idEstadoSolicitud == "7" ? "(Aprobado)" : "(No Aprobado)";
                                lblEstadoDelMontoFinalAFinanciar.Attributes.Add("class", idEstadoSolicitud == "7" ? "font-weight-bold text-success" : "font-weight-bold text-danger");
                                txtMontoTotalAFinanciar_FinalAprobado.Text = valorTotalFinalAFinanciar.ToString("N");
                                txtPlazoFinal_FinalAprobado.Text = plazoFinal.ToString();
                                lblTipoDePlazo_FinalAprobado.InnerText = (IdProducto == "202" || IdProducto == "203" || IdProducto == "204") ? "Mensual" : "Quincenal";

                                /* Culcular préstamo final a financiar */
                                txtCuotaDelPrestamo_FinalAprobado.Text = calculoPrestamoFinal.ValorCuotaPrestamo.ToString("N");
                                txtCuotaDelSeguro_FinalAprobado.Text = calculoPrestamoFinal.ValorCuotaSeguroDeVehiculo.ToString("N");
                                txtCuotaGPS_FinalAprobado.Text = calculoPrestamoFinal.ValorCuotaServicioGPS.ToString("N");
                                txtCuotaTotal_FinalAprobado.Text = calculoPrestamoFinal.ValorCuotaNeta.ToString("N");
                                txtCostoAparatoGPS_FinalAprobado.Text = calculoPrestamoFinal.CostoAparatoGPS.ToString("N");
                                txtGastosDeCierre_FinalAprobado.Text = calculoPrestamoFinal.ValorGastosDeCierre.ToString("N");
                                txtTasaAnualAplicada_FinalAprobado.Text = calculoPrestamoFinal.TasaAnualAplicada.ToString("N");
                                txtTasaMensualAplicada_FinalAprobado.Text = calculoPrestamoFinal.TasaMensualAplicada.ToString("N");

                                divPrestamoFinalAprobado.Visible = true;
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
                                txtCapacidadDePagoMensual_Recalculo.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCapacidadDePagoQuicenal_Recalculo.Text = (decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()) / 2).ToString("N");
                                divRecalculoCapacidadDePago.Visible = true;

                                divPrestamosSueridos_CapacidadDePagoReal.Visible = true;

                                if (idEstadoSolicitud != "7" && idEstadoSolicitud != "5" && idEstadoSolicitud != "4")
                                {
                                    /*** Mostrar prestamos sugeridos para la nueva capacidad de pago ***/
                                    using (var sqlComandoCotizador = new SqlCommand("CoreFinanciero.dbo.sp_CotizadorProductos", sqlConexion))
                                    {
                                        sqlComandoCotizador.CommandType = CommandType.StoredProcedure;
                                        sqlComandoCotizador.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                        sqlComandoCotizador.Parameters.AddWithValue("@piIDProducto", IdProducto);
                                        sqlComandoCotizador.Parameters.AddWithValue("@pcIdentidad", pcID);
                                        sqlComandoCotizador.Parameters.AddWithValue("@piConObligaciones", obligacionesPrecalificado == 0 ? "0" : "1");
                                        sqlComandoCotizador.Parameters.AddWithValue("@pnIngresosBrutos", ingresosReales);
                                        sqlComandoCotizador.Parameters.AddWithValue("@pnIngresosDisponible", ingresosReales - obligacionesPrecalificado);

                                        using (var sqlResultadoCotizador = sqlComandoCotizador.ExecuteReader())
                                        {
                                            if (sqlResultadoCotizador.HasRows)
                                            {
                                                divTablaNuevosPrestamosSugeridos.Visible = true;
                                            }
                                            else
                                            {
                                                divSinCapacidadDePago.Visible = true;
                                            }

                                            HtmlTableRow tRowPrestamoSugerido = null;

                                            while (sqlResultadoCotizador.Read())
                                            {
                                                tRowPrestamoSugerido = new HtmlTableRow();
                                                tRowPrestamoSugerido.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultadoCotizador["fnMontoOfertado"].ToString()).ToString("N") });
                                                tRowPrestamoSugerido.Cells.Add(new HtmlTableCell() { InnerText = sqlResultadoCotizador["fiPlazo"].ToString() + " " + sqlResultadoCotizador["fcTipodeCuota"].ToString() });
                                                tRowPrestamoSugerido.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultadoCotizador["fnCuotaQuincenal"].ToString()).ToString("N") });
                                                tblPrestamosSugeridosReales.Rows.Add(tRowPrestamoSugerido);
                                            }
                                        }
                                    }
                                }
                            }

                            txtTipoDeEmpresa.Text = sqlResultado["fcTipoEmpresa"].ToString();
                            txtTipoDePerfil.Text = sqlResultado["fcTipoPerfil"].ToString();
                            txtTipoDeEmpleo.Text = sqlResultado["fcTipoEmpleado"].ToString();
                            txtBuroActual.Text = sqlResultado["fcBuroActual"].ToString();
                            txtOrigen.Text = sqlResultado["fcOrigen"].ToString();

                            /****** Comentarios del procesamiento de la solicitud *SE HACE EN EL FRONTEND* ******/

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

                            if (documentacionDomicilio.ToString() == "")
                            {
                                divDocumentacionDomicilioModal.Visible = false;
                            }

                            if (documentacionLaboral.ToString() == "")
                            {
                                divDocumentacionLaboralModal.Visible = false;
                            }

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

                            /****** Condicionamientos de la solicitud *SE HACE EN EL FRONTEND* ******/
                            sqlResultado.NextResult();

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

                                divResolucionTrabajo.Visible = true;
                                divInformaciondeCampo.Visible = true;
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

                                divResolucionDomicilio.Visible = true;
                                divInformaciondeCampo.Visible = true;
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
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerHtml = sqlResultado["fcComentarioDeptoCredito"].ToString() != "" ? sqlResultado["fcComentarioDeptoCredito"].ToString() != "Sin comunicacion" ? "<i class='mdi mdi-check-circle-outline'></i>" : "<i class='mdi mdi-call-missed'></i>" : "<i class='mdi mdi-phone-settings'></i>" });
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerHtml = btnComentarioReferenciaPersonal });

                                colorClass = sqlResultado["fcComentarioDeptoCredito"].ToString() != "" ? sqlResultado["fcComentarioDeptoCredito"].ToString() != "Sin comunicacion" ? "tr-exito" : "text-danger" : "";

                                tRowReferencias.Attributes.Add("class", colorClass);
                                tblReferencias.Rows.Add(tRowReferencias);
                            }
                        }
                    }
                }
            }

            logo = IdProducto == "101" ? "iconoRecibirDinero48.png" : IdProducto == "201" ? "iconoMoto48.png" : (IdProducto == "202" || IdProducto == "203" || IdProducto == "204") ? "iconoAuto48.png" : (IdProducto == "301" || IdProducto == "302") ? "iconoConsumo48.png" : "iconoConsumo48.png";
            imgLogo.ImageUrl = "/Imagenes/" + logo;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public void CargarInformacionGarantia()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitud_CREDGarantia_ObtenerPorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            string lcScript = "window.open('SolicitudesCredito_ListadoGarantias.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&SID=" + pcIDSolicitud + "&IDApp=" + pcIDApp) + "','_self')";
                            Response.Write("<script>");
                            Response.Write(lcScript);
                            Response.Write("</script>");
                        }

                        while (sqlResultado.Read())
                        {
                            /* El primer resultado es información de la solicitud y el cliente */


                            /* El segundo resultado es la información de la garantía*/
                            sqlResultado.NextResult();

                            if (!sqlResultado.HasRows)
                            {
                                panelInformacionGarantia.Visible = false;
                            }
                            else
                            {
                                /* Informacion del garantía */
                                while (sqlResultado.Read())
                                {
                                    txtVIN.Text = sqlResultado["fcVin"].ToString();
                                    txtTipoDeGarantia.Text = sqlResultado["fcTipoGarantia"].ToString();
                                    txtTipoDeVehiculo.Text = sqlResultado["fcTipoVehiculo"].ToString();
                                    txtMarca.Text = sqlResultado["fcMarca"].ToString();
                                    txtModelo.Text = sqlResultado["fcModelo"].ToString();
                                    txtAnio.Text = sqlResultado["fiAnio"].ToString();
                                    txtColor.Text = sqlResultado["fcColor"].ToString();
                                    txtMatricula.Text = sqlResultado["fcMatricula"].ToString();
                                    txtSerieMotor.Text = sqlResultado["fcMotor"].ToString();
                                    txtSerieChasis.Text = sqlResultado["fcChasis"].ToString();
                                    txtGPS.Text = sqlResultado["fcGPS"].ToString();
                                    txtCilindraje.Text = sqlResultado["fcCilindraje"].ToString();
                                    txtRecorrido.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnRecorrido"].ToString())) + " " + sqlResultado["fcUnidadDeDistancia"].ToString();
                                    txtTransmision.Text = sqlResultado["fcTransmision"].ToString();
                                    txtTipoDeCombustible.Text = sqlResultado["fcTipoCombustible"].ToString();
                                    txtSerieUno.Text = sqlResultado["fcSerieUno"].ToString();
                                    txtSerieDos.Text = sqlResultado["fcSerieDos"].ToString();
                                    txtComentario.InnerText = sqlResultado["fcComentario"].ToString().Trim();

                                    txtNombrePropietarioGarantia.Text = sqlResultado["fcNombrePropietarioGarantia"].ToString();
                                    txtIdentidadPropietarioGarantia.Text = sqlResultado["fcIdentidadPropietarioGarantia"].ToString();
                                    txtNacionalidadPropietarioGarantia.Text = sqlResultado["fcNacionalidadPropietarioGarantia"].ToString();
                                    txtEstadoCivilPropietarioGarantia.Text = sqlResultado["fcEstadoCivilPropietarioGarantia"].ToString();
                                    txtNombreVendedorGarantia.Text = sqlResultado["fcNombreVendedorGarantia"].ToString();
                                    txtIdentidadVendedorGarantia.Text = sqlResultado["fcIdentidadVendedorGarantia"].ToString();
                                    txtNacionalidadVendedorGarantia.Text = sqlResultado["fcNacionalidadVendedorGarantia"].ToString();
                                    txtEstadoCivilVendedorGarantia.Text = sqlResultado["fcEstadoCivilVendedorGarantia"].ToString();
                                }

                                /* Fotografías de la garantía */
                                sqlResultado.NextResult();

                                if (!sqlResultado.HasRows)
                                {
                                    divGaleriaGarantia.InnerHtml = "<img alt='No hay fotografías disponibles' src='/Imagenes/Imagen_no_disponible.png' data-image='/Imagenes/Imagen_no_disponible.png' data-description='No hay fotografías disponibles'/>";
                                }
                                else
                                {
                                    var imagenesGarantia = new StringBuilder();

                                    while (sqlResultado.Read())
                                    {
                                        imagenesGarantia.Append("<img alt='" + sqlResultado["fcSeccionGarantia"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcSeccionGarantia"] + "'/>");
                                    }
                                    divGaleriaGarantia.InnerHtml = imagenesGarantia.ToString();
                                }
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
    }

    [WebMethod]
    public static SolicitudesCredito_RegistradaDetalles_EstadoProcesos_ViewModel CargarEstadoSolicitud(string dataCrypt)
    {
        var estadoSolicitud = new SolicitudesCredito_RegistradaDetalles_EstadoProcesos_ViewModel();

        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_CargarEstadoProcesos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            estadoSolicitud = new SolicitudesCredito_RegistradaDetalles_EstadoProcesos_ViewModel()
                            {
                                EnIngresoInicio = (DateTime)sqlResultado["fdEnIngresoInicio"],
                                EnIngresoFin = (DateTime)sqlResultado["fdEnIngresoFin"],
                                UsuarioEnIngreso = (string)sqlResultado["fcUsuarioEnIngreso"],
                                EnColaInicio = (DateTime)sqlResultado["fdEnColaInicio"],
                                EnColaFin = (DateTime)sqlResultado["fdEnColaFin"],
                                EnAnalisisInicio = (DateTime)sqlResultado["fdEnAnalisisInicio"],
                                UsuarioAnalista = (string)sqlResultado["fcUsuarioAnalista"],
                                FechaValidacionInformacionPersonal = (DateTime)sqlResultado["fdAnalisisTiempoValidarInformacionPersonal"],
                                ComentarioValidacionInformacionPersonal = (string)sqlResultado["fcComentarioValidacionInfoPersonal"],
                                FechaValidacionDocumentacion = (DateTime)sqlResultado["fdAnalisisTiempoValidarDocumentos"],
                                ComentarioValidacionDocumentacion = (string)sqlResultado["fcComentarioValidacionDocumentacion"],
                                FechaValidacionReferenciasPersonales = (DateTime)sqlResultado["fdAnalisisTiempoValidacionReferenciasPersonales"],
                                ComentarioValidacionReferenciasPersonales = (string)sqlResultado["fcComentarioValidacionReferenciasPersonales"],
                                FechaValidacionInformacionLaboral = (DateTime)sqlResultado["fdAnalisisTiempoValidacionReferenciasPersonales"],
                                ComentarioValidacionInformacionLaboral = (string)sqlResultado["fcComentarioValidacionInfoLaboral"],
                                ComentarioResolucion = (string)sqlResultado["fcComentarioResolucion"],
                                TiempoTomaDecisionFinal = (DateTime)sqlResultado["fdTiempoTomaDecisionFinal"],
                                EnAnalisisFin = (DateTime)sqlResultado["fdEnAnalisisFin"],
                                CondicionadoInicio = (DateTime)sqlResultado["fdCondicionadoInicio"],
                                ComentarioCondicionado = (string)sqlResultado["fcCondicionadoComentario"],
                                UsuarioCondicionado = (string)sqlResultado["fcUsuarioEnIngreso"],
                                CondicionadoFin = (DateTime)sqlResultado["fdCondificionadoFin"],
                                FechaEnvioARuta = (DateTime)sqlResultado["fdEnvioARutaAnalista"],
                                ObservacionesDeCreditos = (string)sqlResultado["fcObservacionesDeCredito"],
                                EnRutaDeInvestigacionInicio = (DateTime)sqlResultado["fdEnRutaDeInvestigacionInicio"],
                                UsuarioGestorAsignado = (string)sqlResultado["fcGestorAsignado"],
                                ObservacionesDeCampo = (string)sqlResultado["fcObservacionesDeCampo"],
                                EnRutaDeInvestigacionFin = (DateTime)sqlResultado["fdEnRutaDeInvestigacionFin"],
                                ReprogramadoInicio = (DateTime)sqlResultado["fdReprogramadoInicio"],
                                ComentarioReprogramado = (string)sqlResultado["fcReprogramadoComentario"],
                                ReprogramadoFin = (DateTime)sqlResultado["fdReprogramadoFin"],
                                PasoFinalInicio = (DateTime)sqlResultado["fdPasoFinalInicio"],
                                UsuarioPasoFinal = (string)sqlResultado["fcUsuarioPasoFinal"],
                                ComentarioPasoFinal = (string)sqlResultado["fcComentarioPasoFinal"],
                                PasoFinalFin = (DateTime)sqlResultado["fdPasoFinalFin"],
                                IdEstadoSolicitud = (byte)sqlResultado["fiEstadoSolicitud"],
                                EstadoSolicitud = (string)sqlResultado["fcEstadoSolicitud"],
                                SolicitudActiva = (byte)sqlResultado["fiSolicitudActiva"]
                            };

                        } // while sqlResultado.Read()
                    } // using executeReader
                } // using command

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Condiciones_ObtenerPorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            estadoSolicitud.Condiciones.Add(new SolicitudesCredito_RegistradaDetalles_Condicion_ViewModel()
                            {
                                IdSolicitudCondicion = (int)sqlResultado["fiIDSolicitudCondicion"],
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                IdCondicion = (int)sqlResultado["fiIDCondicion"],
                                TipoCondicion = sqlResultado["fcCondicion"].ToString(),
                                DescripcionCondicion = sqlResultado["fcDescripcionCondicion"].ToString(),
                                ComentarioAdicional = sqlResultado["fcComentarioAdicional"].ToString(),
                                EstadoCondicion = (bool)sqlResultado["fbEstadoCondicion"]
                            });
                        }
                    } // using executeReader()
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return estadoSolicitud;
    }

    public SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel CalcularPrestamo(int idProducto, decimal valorPrestamo, decimal valorPrima, int plazo, SqlConnection sqlConexion)
    {
        SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel resultado = null;
        try
        {
            using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_CalculoPrestamo", sqlConexion))
            {
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@piIDProducto", idProducto);
                sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", valorPrestamo);
                sqlComando.Parameters.AddWithValue("@liPlazo", plazo);
                sqlComando.Parameters.AddWithValue("@pnValorPrima", valorPrima);
                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                using (var sqlResultado = sqlComando.ExecuteReader())
                {
                    while (sqlResultado.Read())
                    {
                        resultado = new SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel()
                        {
                            ValorSeguroDeDeuda = (decimal)sqlResultado["fnSegurodeDeuda"],
                            ValorSeguroDeVehiculo = (decimal)sqlResultado["fnSegurodeVehiculo"],
                            ValorGastosDeCierre = (decimal)sqlResultado["fnGastosdeCierre"],
                            ValorAFinanciar = (decimal)sqlResultado["fnValoraFinanciar"],
                            ValorCuotaPrestamo = (decimal)sqlResultado["fnValorCuota"],
                            CostoAparatoGPS = (decimal)sqlResultado["fnCostoGPS"],
                            ValorCuotaServicioGPS = (decimal)sqlResultado["fnCuotaServicioGPS"],
                            TotalSeguroVehiculo = (decimal)sqlResultado["fnTotalSeguroVehiculo"],
                            ValorCuotaSeguroDeVehiculo = (decimal)sqlResultado["fnCuotaSegurodeVehiculo"],
                            ValorCuotaNeta = (decimal)sqlResultado["fnValorCuotaNeta"],
                            Plazo = (short)sqlResultado["fiPlazo"],
                            TipoDePlazo = (string)sqlResultado["fcTipodeCuota"]
                        };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = null;
        }
        return resultado;
    }

    public SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel CargarPrestamoSolicitadoVehiculo(string idProducto, decimal valorPrestamo, decimal valorPrima, int plazo, SqlConnection sqlConexion)
    {
        SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel resultado = null;
        try
        {
            /* Si la información */
            if (int.Parse(pcIDSolicitud) < 802 && pcIDSolicitud != "773")
            {
                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_CalculoPrestamo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDProducto", idProducto);
                    sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", valorPrestamo);
                    sqlComando.Parameters.AddWithValue("@liPlazo", plazo);
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", valorPrima);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultado = new SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel()
                            {
                                ValorSeguroDeDeuda = (decimal)sqlResultado["fnSegurodeDeuda"],
                                ValorSeguroDeVehiculo = (decimal)sqlResultado["fnSegurodeVehiculo"],
                                ValorGastosDeCierre = (decimal)sqlResultado["fnGastosdeCierre"],
                                ValorAFinanciar = (decimal)sqlResultado["fnValoraFinanciar"],
                                ValorCuotaPrestamo = (decimal)sqlResultado["fnValorCuota"],
                                CostoAparatoGPS = (decimal)sqlResultado["fnCostoGPS"],
                                ValorCuotaServicioGPS = (decimal)sqlResultado["fnCuotaServicioGPS"],
                                TotalSeguroVehiculo = (decimal)sqlResultado["fnTotalSeguroVehiculo"],
                                ValorCuotaSeguroDeVehiculo = (decimal)sqlResultado["fnCuotaSegurodeVehiculo"],
                                ValorCuotaNeta = (decimal)sqlResultado["fnValorCuotaNeta"],
                                Plazo = (short)sqlResultado["fiPlazo"],
                                TipoDePlazo = (string)sqlResultado["fcTipodeCuota"],
                                TasaAnualAplicada = (decimal)sqlResultado["fnTasaDeInteresAnual"],
                                TasaMensualAplicada = (decimal)sqlResultado["fnTasaDeInteresMensual"]
                            };
                        }
                    }
                }
            }
            else
            {
                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_InformacionPrestamo_ObtenerPorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultado = new SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel()
                            {
                                ValorSeguroDeDeuda = 0,
                                ValorSeguroDeVehiculo = (decimal)sqlResultado["fnValorTotalSeguro"],
                                ValorGastosDeCierre = (decimal)sqlResultado["fnGastosDeCierre"],
                                ValorAFinanciar = (decimal)sqlResultado["fnValorTotalFinanciamiento"],
                                ValorCuotaPrestamo = (decimal)sqlResultado["fnCuotaMensualPrestamo"],
                                CostoAparatoGPS = (decimal)sqlResultado["fnCostoGPS"],
                                ValorCuotaServicioGPS = (decimal)sqlResultado["fnCuotaMensualGPS"],
                                TotalSeguroVehiculo = (decimal)sqlResultado["fnValorTotalSeguro"],
                                ValorCuotaSeguroDeVehiculo = (decimal)sqlResultado["fnCuotaMensualSeguro"],
                                ValorCuotaNeta = (decimal)sqlResultado["fnCuotaTotal"],
                                Plazo = (int)sqlResultado["fiPlazo"],
                                TipoDePlazo = (string)sqlResultado["fcTipoDePlazo"],
                                TasaAnualAplicada = (decimal)sqlResultado["fnTasaAnualAplicada"] < 0 ? ((decimal)sqlResultado["fnTasaAnualAplicada"] * 100) : (decimal)sqlResultado["fnTasaAnualAplicada"],
                                TasaMensualAplicada = (decimal)sqlResultado["fnTasaMensualAplicada"] < 0 ? ((decimal)sqlResultado["fnTasaMensualAplicada"] * 100) : (decimal)sqlResultado["fnTasaMensualAplicada"]
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = null;
        }
        return resultado;
    }

    [WebMethod]
    public static string ObtenerUrlEncriptado(string dataCrypt)
    {
        var lURLDesencriptado = DesencriptarURL(dataCrypt);

        var pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
        var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp").ToString();
        //var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID").ToString() ?? "1";
        var pcIDSesion = "1";
        var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

        var parametrosEncriptados = DSC.Encriptar("usr=" + pcIDUsuario + "&ID=" + pcID + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion);

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

    public static int GetMonthDifference(DateTime startDate, DateTime endDate)
    {
        int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
        return Math.Abs(monthsApart);
    }

    public class SolicitudesCredito_RegistradaDetalles_Calculo_ViewModel
    {
        public decimal ValorSeguroDeDeuda { get; set; }
        public decimal ValorSeguroDeVehiculo { get; set; }
        public decimal ValorGastosDeCierre { get; set; }
        public decimal ValorAFinanciar { get; set; }
        public decimal ValorCuotaPrestamo { get; set; }
        public decimal CostoAparatoGPS { get; set; }
        public decimal ValorCuotaServicioGPS { get; set; }
        public decimal TotalSeguroVehiculo { get; set; }
        public decimal ValorCuotaSeguroDeVehiculo { get; set; }
        public decimal ValorCuotaNeta { get; set; }
        public int Plazo { get; set; }
        public string TipoDePlazo { get; set; }
        public decimal TasaAnualAplicada { get; set; }
        public decimal TasaMensualAplicada { get; set; }
    }

    public class SolicitudesCredito_RegistradaDetalles_EstadoProcesos_ViewModel
    {
        public DateTime EnIngresoInicio { get; set; }
        public DateTime EnIngresoFin { get; set; }
        public string UsuarioEnIngreso { get; set; }

        public DateTime EnColaInicio { get; set; }
        public DateTime EnColaFin { get; set; }

        public DateTime EnAnalisisInicio { get; set; }
        public string UsuarioAnalista { get; set; }

        public DateTime FechaValidacionInformacionPersonal { get; set; }
        public string ComentarioValidacionInformacionPersonal { get; set; }

        public DateTime FechaValidacionDocumentacion { get; set; }
        public string ComentarioValidacionDocumentacion { get; set; }

        public DateTime FechaValidacionReferenciasPersonales { get; set; }
        public string ComentarioValidacionReferenciasPersonales { get; set; }

        public DateTime FechaValidacionInformacionLaboral { get; set; }
        public string ComentarioValidacionInformacionLaboral { get; set; }

        public string ComentarioResolucion { get; set; }
        public DateTime TiempoTomaDecisionFinal { get; set; }
        public DateTime EnAnalisisFin { get; set; }

        public DateTime CondicionadoInicio { get; set; }
        public string ComentarioCondicionado { get; set; }
        public string UsuarioCondicionado { get; set; }
        public DateTime CondicionadoFin { get; set; }

        public DateTime FechaEnvioARuta { get; set; }
        public string ObservacionesDeCreditos { get; set; }
        public DateTime EnRutaDeInvestigacionInicio { get; set; }
        public string UsuarioGestorAsignado { get; set; }
        public string ObservacionesDeCampo { get; set; }
        public DateTime EnRutaDeInvestigacionFin { get; set; }

        public DateTime ReprogramadoInicio { get; set; }
        public string ComentarioReprogramado { get; set; }
        public DateTime ReprogramadoFin { get; set; }

        public DateTime PasoFinalInicio { get; set; }
        public string UsuarioPasoFinal { get; set; }
        public string ComentarioPasoFinal { get; set; }
        public DateTime PasoFinalFin { get; set; }

        public int IdEstadoSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }

        public int SolicitudActiva { get; set; }

        public List<SolicitudesCredito_RegistradaDetalles_Condicion_ViewModel> Condiciones { get; set; }

        public SolicitudesCredito_RegistradaDetalles_EstadoProcesos_ViewModel()
        {
            Condiciones = new List<SolicitudesCredito_RegistradaDetalles_Condicion_ViewModel>();
        }
    }

    public class SolicitudesCredito_RegistradaDetalles_Condicion_ViewModel
    {
        public int IdSolicitudCondicion { get; set; }
        public int IdCondicion { get; set; }
        public int IdSolicitud { get; set; }
        public string TipoCondicion { get; set; }
        public string DescripcionCondicion { get; set; }
        public string ComentarioAdicional { get; set; }
        public bool EstadoCondicion { get; set; }
    }
}