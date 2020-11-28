using proyectoBase.Models.ViewModel;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;

public partial class SolicitudesCredito_RegistradaDetalles : System.Web.UI.Page
{
    public string pcID { get; set; }
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
                            txtPlazoSeleccionado.Text = decimal.Parse(sqlResultado["fiPlazoSeleccionado"].ToString()).ToString("N");
                            /*lblTipoDePlazo_FinalAprobado.InnerText = decimal.Parse(sqlResultado["AquiPonerDinamicamenteElTipoDePlazo"].ToString()).ToString("N"); */
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

                            var montoFinalAFinanciar = decimal.Parse(sqlResultado["fnMontoFinalFinanciar"].ToString());

                            /*** Prestamo Final aprobado ***/
                            if (montoFinalAFinanciar != 0)
                            {
                                lblEstadoDelMontoFinalAFinanciar.InnerText = idEstadoSolicitud == "7" ? "Aprobado" : "No Aprobado";
                                lblEstadoDelMontoFinalAFinanciar.Attributes.Add("class", idEstadoSolicitud == "7" ? "font-weight-bold text-success" : "font-weight-bold text-danger");

                                txtMontoTotalAFinanciar_FinalAprobado.Text = decimal.Parse(sqlResultado["fnMontoFinalFinanciar"].ToString()).ToString("N");
                                txtPlazoFinal_FinalAprobado.Text = decimal.Parse(sqlResultado["fiPlazoFinalAprobado"].ToString()).ToString("N");

                                /*lblTipoDePlazo_FinalAprobado.InnerText = decimal.Parse(sqlResultado["AquiPonerDinamicamenteElTipoDePlazo"].ToString()).ToString("N"); */
                                lblTipoDePlazo_FinalAprobado.InnerText = IdProducto == "202" ? "Mensual" : "Quincenal";

                                /* Culcular préstamo final a financiar *PENDIENTE* */
                                txtCuotaDelPrestamo_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCuotaDelSeguro_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCuotaGPS_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCuotaTotal_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtCostoAparatoGPS_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");
                                txtGastosDeCierre_FinalAprobado.Text = decimal.Parse(sqlResultado["fnDisponiblePrecalificado"].ToString()).ToString("N");

                                divPrestamoFinalAprobado.Visible = true;
                            }
                            else
                            {
                                divPrestamoFinalAprobado.Visible = false;
                            }

                            var sueldoBaseReal = decimal.Parse(sqlResultado["fnSueldoBaseReal"].ToString());

                            /*** Recalculo capacidad de pago cuando los ingresos son diferentes ***/
                            if (sueldoBaseReal != 0 || 1 == 1)
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

                                if (estadoSolicitud != "7" && estadoSolicitud != "5" && estadoSolicitud != "4" || 1 == 1)
                                {
                                    /*** Mostrar prestamos sugeridos para la nueva capacidad de pago ***/
                                    using (var sqlComandoCotizador = new SqlCommand("CoreFinanciero.dbo.sp_CotizadorProductos", sqlConexion))
                                    {
                                        sqlComandoCotizador.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                        sqlComandoCotizador.Parameters.AddWithValue("@piIDProducto", IdProducto);
                                        sqlComandoCotizador.Parameters.AddWithValue("@pcIdentidad", pcID);
                                        sqlComandoCotizador.Parameters.AddWithValue("@piConObligaciones", obligacionesPrecalificado == 0 ? "0" : "1");
                                        sqlComandoCotizador.Parameters.AddWithValue("@pnIngresosBrutos", ingresosReales);
                                        sqlComandoCotizador.Parameters.AddWithValue("@pnIngresosDisponible", ingresosReales - obligacionesPrecalificado);
                                        sqlComandoCotizador.CommandType = CommandType.StoredProcedure;

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


                            /****** Comentarios del procesamiento de la solicitud  ******/

                            lblEstadoSolicitud.InnerText = estadoSolicitud;

                            lblEstadoSolicitud.Attributes.Add("class", idEstadoSolicitud == "7" ? "text-center font-weight-bold text-success" : (idEstadoSolicitud == "5" || idEstadoSolicitud == "4") ? "text-center font-weight-bold text-danger" : "text-center font-weight-bold text-warning");

                            var contadorComentario = 0;

                            if (sqlResultado["fcReprogramadoComentario"].ToString() != string.Empty)
                            {
                                liObservacionesReprogramacion.Visible = true;
                                lblUsuario_ComentarioReprogramacion.InnerText = sqlResultado["fcNombreGestor"].ToString();
                                lblFecha_ComentarioReprogramacion.InnerText = DateTime.Parse(sqlResultado["fdReprogramadoInicio"].ToString()).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                lblComentario_Reprogramacion.InnerText = sqlResultado["fcReprogramadoComentario"].ToString();
                                contadorComentario++;
                            }

                            if (sqlResultado["fcCondicionadoComentario"].ToString() != string.Empty)
                            {
                                liObservaciones_OtrosCondicionamientos.Visible = true;
                                lblUsuario_ComentarioOtrosCondicionamientos.InnerText = sqlResultado["fcNombreCortoAnalista"].ToString();
                                lblFecha_OtrosCondicionamientos.InnerText = DateTime.Parse(sqlResultado["fdCondicionadoInicio"].ToString()).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                lblComentario_OtrosCondicionamientos.InnerText = sqlResultado["fcCondicionadoComentario"].ToString();
                                contadorComentario++;
                            }

                            if (sqlResultado["fcComentarioValidacionInfoPersonal"].ToString() != string.Empty)
                            {
                                liObservacionesInformacionPersonal.Visible = true;
                                lblUsuario_ComentarioInformacionPerosnal.InnerText = sqlResultado["fcNombreCortoAnalista"].ToString();
                                lblFecha_ComentarioInformacionPersonal.InnerText = DateTime.Parse(sqlResultado["fdAnalisisTiempoValidarInformacionPersonal"].ToString()).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                lblComentario_InformacionPersonal.InnerText = sqlResultado["fcComentarioValidacionInfoPersonal"].ToString();
                                contadorComentario++;
                            }

                            if (sqlResultado["fcComentarioValidacionInfoLaboral"].ToString() != string.Empty)
                            {
                                liObservacionesInformacionLaboral.Visible = true;
                                lblUsuario_ComentarioInformacionLaboral.InnerText = sqlResultado["fcNombreCortoAnalista"].ToString();
                                lblFecha_ComentarioInformacionLaboral.InnerText = DateTime.Parse(sqlResultado["fdAnalisisTiempoValidarInformacionLaboral"].ToString()).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                lblComentario_InformacionLaboral.InnerText = sqlResultado["fcComentarioValidacionInfoLaboral"].ToString();
                                contadorComentario++;
                            }

                            if (sqlResultado["fcComentarioValidacionReferenciasPersonales"].ToString() != string.Empty)
                            {
                                liObservacionesReferenciasPersonales.Visible = true;
                                lblUsuario_ComentarioReferenciasPersonales.InnerText = sqlResultado["fcNombreCortoAnalista"].ToString();
                                lblFecha_ComentarioReferenciasPersonales.InnerText = DateTime.Parse(sqlResultado["fdAnalisisTiempoValidacionReferenciasPersonales"].ToString()).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                lblComentario_ReferenciasPersonales.InnerText = sqlResultado["fcComentarioValidacionReferenciasPersonales"].ToString();
                                contadorComentario++;
                            }

                            if (sqlResultado["fcComentarioValidacionDocumentacion"].ToString() != string.Empty)
                            {
                                liObservacionesDocumentacion.Visible = true;
                                lblUsuario_ComentarioDocumentacion.InnerText = sqlResultado["fcNombreCortoAnalista"].ToString();
                                lblFecha_ComentarioDocumentacion.InnerText = DateTime.Parse(sqlResultado["fdAnalisisTiempoValidarDocumentos"].ToString()).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                lblComentario_Documentacion.InnerText = sqlResultado["fcComentarioValidacionDocumentacion"].ToString();
                                contadorComentario++;
                            }

                            if (sqlResultado["fcObservacionesDeCredito"].ToString() != string.Empty)
                            {
                                liObservacionesParaGestoria.Visible = true;
                                lblUsuario_ComentarioParaGestoria.InnerText = sqlResultado["fcNombreCortoAnalista"].ToString();
                                lblFecha_ComentarioParaGestoria.InnerText = DateTime.Parse(sqlResultado["fdEnvioARutaAnalista"].ToString()).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                lblComentario_ParaGestoria.InnerText = sqlResultado["fcObservacionesDeCredito"].ToString();
                                contadorComentario++;
                            }

                            if (sqlResultado["fcObservacionesDeCampo"].ToString() != string.Empty)
                            {
                                liObservacionesDeGestoria.Visible = true;
                                lblUsuario_ComentarioDeGestoria.InnerText = sqlResultado["fcNombreCortoAnalista"].ToString();
                                lblFecha_ComentarioDeGestoria.InnerText = DateTime.Parse(sqlResultado["fdEnRutaDeInvestigacionFin"].ToString()).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                lblComentario_DeGestoria.InnerText = sqlResultado["fcObservacionesDeCampo"].ToString();
                                contadorComentario++;
                            }

                            if (sqlResultado["fcComentarioResolucion"].ToString() != string.Empty)
                            {
                                liComentariosDeLaResolucion.Visible = true;
                                lblUsuario_ComentarioDeLaResolucion.InnerText = sqlResultado["fcNombreCortoAnalista"].ToString();
                                lblFecha_ComentarioDeLaResolucion.InnerText = DateTime.Parse(sqlResultado["fdTiempoTomaDecisionFinal"].ToString()).ToString("dddd, dd MMMM yyyy hh:mm tt");
                                lblComentario_Resolicion.InnerText = sqlResultado["fcComentarioResolucion"].ToString();
                                contadorComentario++;
                            }

                            if (contadorComentario > 0)
                            {
                                divNoHayMasDetalles.Visible = false;
                                divLineaDeTiempo.Visible = true;
                            }


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
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerHtml = sqlResultado["fcComentarioDeptoCredito"].ToString() != "Sin comunicacion" ? "<i class='mdi mdi-check-circle-outline'></i>" : "<i class='mdi mdi-call-missed'></i>" });
                                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerHtml = btnComentarioReferenciaPersonal });

                                colorClass = sqlResultado["fcComentarioDeptoCredito"].ToString() != "Sin comunicacion" ? "tr-exito" : "text-danger";

                                tRowReferencias.Attributes.Add("class", colorClass);
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
    public static BandejaSolicitudesViewModel CargarEstadoSolicitud(string dataCrypt)
    {
        var objEstadoSolicitud = new BandejaSolicitudesViewModel();

        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));
            var pcIDSesion = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID"));
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var pcIDSolicitud = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_SolicitudEstadoProcesamiento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", pcIDSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
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
                                fdEnIngresoInicio = ConvertFromDBVal<DateTime>(reader["fdEnIngresoInicio"]),
                                fdEnIngresoFin = ConvertFromDBVal<DateTime>(reader["fdEnIngresoFin"]),
                                fdEnTramiteInicio = ConvertFromDBVal<DateTime>(reader["fdEnColaInicio"]),
                                fdEnTramiteFin = ConvertFromDBVal<DateTime>(reader["fdEnColaFin"]),
                                /* Todo el proceso de analisis */
                                fdEnAnalisisInicio = ConvertFromDBVal<DateTime>(reader["fdEnAnalisisInicio"]),
                                ftAnalisisTiempoValidarInformacionPersonal = ConvertFromDBVal<DateTime>(reader["fdAnalisisTiempoValidarInformacionPersonal"]),
                                fcComentarioValidacionInfoPersonal = (string)reader["fcComentarioValidacionInfoPersonal"],
                                ftAnalisisTiempoValidarDocumentos = ConvertFromDBVal<DateTime>(reader["fdAnalisisTiempoValidarDocumentos"]),
                                fcComentarioValidacionDocumentacion = (string)reader["fcComentarioValidacionDocumentacion"],
                                fbValidacionDocumentcionIdentidades = (byte)reader["fbValidacionDocumentcionIdentidades"],
                                fbValidacionDocumentacionDomiciliar = (byte)reader["fbValidacionDocumentacionDomiciliar"],
                                fbValidacionDocumentacionLaboral = (byte)reader["fbValidacionDocumentacionLaboral"],
                                fbValidacionDocumentacionSolicitudFisica = (byte)reader["fbValidacionDocumentacionSolicitudFisica"],
                                ftAnalisisTiempoValidacionReferenciasPersonales = ConvertFromDBVal<DateTime>(reader["fdAnalisisTiempoValidacionReferenciasPersonales"]),
                                fcComentarioValidacionReferenciasPersonales = (string)reader["fcComentarioValidacionReferenciasPersonales"],
                                ftAnalisisTiempoValidarInformacionLaboral = ConvertFromDBVal<DateTime>(reader["fdAnalisisTiempoValidarInformacionLaboral"]),
                                fcComentarioValidacionInfoLaboral = (string)reader["fcComentarioValidacionInfoLaboral"],
                                ftTiempoTomaDecisionFinal = ConvertFromDBVal<DateTime>(reader["fdTiempoTomaDecisionFinal"]),
                                fcObservacionesDeCredito = (string)reader["fcObservacionesDeCredito"],
                                fcComentarioResolucion = (string)reader["fcComentarioResolucion"],
                                fdEnAnalisisFin = ConvertFromDBVal<DateTime>(reader["fdEnAnalisisFin"]),
                                /* Todo el proceso de analisis */
                                fdCondicionadoInicio = ConvertFromDBVal<DateTime>(reader["fdCondicionadoInicio"]),
                                fcCondicionadoComentario = (string)reader["fcCondicionadoComentario"],
                                fdCondificionadoFin = ConvertFromDBVal<DateTime>(reader["fdCondificionadoFin"]),
                                /* Proceso de campo */
                                fdEnvioARutaAnalista = ConvertFromDBVal<DateTime>(reader["fdEnvioARutaAnalista"]),
                                fiEstadoDeCampo = (byte)reader["fiEstadoDeCampo"],
                                fdEnCampoInicio = ConvertFromDBVal<DateTime>(reader["fdEnRutaDeInvestigacionInicio"]),
                                fcObservacionesDeGestoria = (string)reader["fcObservacionesDeCampo"],
                                fdEnCampoFin = ConvertFromDBVal<DateTime>(reader["fdEnRutaDeInvestigacionFin"]),
                                fdReprogramadoInicio = ConvertFromDBVal<DateTime>(reader["fdReprogramadoInicio"]),
                                fcReprogramadoComentario = (string)reader["fcReprogramadoComentario"],
                                fdReprogramadoFin = ConvertFromDBVal<DateTime>(reader["fdReprogramadoFin"]),
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