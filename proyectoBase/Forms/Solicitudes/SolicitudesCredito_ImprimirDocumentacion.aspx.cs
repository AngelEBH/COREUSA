using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Services;

public partial class SolicitudesCredito_ImprimirDocumentacion : System.Web.UI.Page
{
    public string pcIDUsuario = "";
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDSolicitud = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    public string DepartamentoFirma { get; set; }
    public string CiudadFirma { get; set; }
    public string DiasFirma { get; set; }
    public string MesFirma { get; set; }
    public string AnioFirma { get; set; }

    public string DiaPrimerPago { get; set; }
    public string MesPrimerPago { get; set; }
    public string AnioPrimerPago { get; set; }

    public string UrlCodigoQR { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = string.Empty;

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
                    var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcEncriptado = lcEncriptado.Replace("%2f", "/");

                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr") ?? "0";
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL") ?? "";

                    UrlCodigoQR = "http://190.92.0.76/OPS/CFRM.aspx?S=" + pcIDSolicitud;

                    var hoy = DateTime.Today;

                    DiasFirma = hoy.Day.ToString();
                    MesFirma = hoy.ToString("MMMM", new CultureInfo("es-ES"));
                    AnioFirma = hoy.Year.ToString();

                    CargarInformacion();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
            }
        }
    }

    private void CargarInformacion()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_InformacionDocumentacion", sqlConexion))
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

                            var fechaPrimerPago = (DateTime)sqlResultado["fdFechaPrimerCuota"];

                            /* Determinar fecha del primer pago */
                            MesPrimerPago = fechaPrimerPago.ToString("MMMM", new CultureInfo("es-ES"));
                            AnioPrimerPago = fechaPrimerPago.Year.ToString();
                            DiaPrimerPago = fechaPrimerPago.Day.ToString();

                            DepartamentoFirma = sqlResultado["fcDepartamentoFirma"].ToString();
                            CiudadFirma = sqlResultado["fcCiudadFirma"].ToString();

                            /* Información del cliente */
                            var numeroPrestamo = sqlResultado["fcNumeroPrestamo"].ToString();
                            var nombreCliente = sqlResultado["fcNombreCliente"].ToString();
                            var identidad = sqlResultado["fcIdentidadCliente"].ToString();
                            var RTN = sqlResultado["fcRTN"].ToString();
                            var telefonoPrimario = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                            var correoCliente = sqlResultado["fcCorreoElectronicoCliente"].ToString();
                            var profesionOficio = sqlResultado["fcProfesionOficioCliente"].ToString();
                            var estadoCivil = sqlResultado["fcDescripcionEstadoCivil"].ToString();
                            var nacionalidad = sqlResultado["fcDescripcionNacionalidad"].ToString();

                            var nombreTrabajo = sqlResultado["fcNombreTrabajo"].ToString();
                            var puestoAsignado = sqlResultado["fcPuestoAsignado"].ToString();
                            var telefonoTrabajo = sqlResultado["fcTelefonoEmpresa"].ToString();
                            var direccionTrabajo = sqlResultado["fcDireccionDetalladaEmpresa"].ToString();

                            var fechaOtorgamiento = (DateTime)sqlResultado["fdTiempoTomaDecisionFinal"];
                            var fechaVencimiento = "";

                            var oficialDeNegocios = sqlResultado["fcOficialDeNegocios"].ToString();
                            var gestorDeCobros = sqlResultado["fcGestorDeCobros"].ToString();


                            /* Direccion del cliente */
                            var departamentoResidencia = sqlResultado["fcDepartamento"].ToString();
                            var ciudadPoblado = sqlResultado["fcPoblado"].ToString();
                            var direccionCliente = sqlResultado["fcDireccionCliente"].ToString();

                            /* Información de la solicitud y prestamo */
                            var producto = sqlResultado["fcProducto"].ToString();
                            var montoTotalContrato = decimal.Parse(sqlResultado["fnValorTotalContrato"].ToString());
                            var plazoFinalAprobado = sqlResultado["fiPlazo"].ToString();
                            var valorTotalFinanciamiento = decimal.Parse(sqlResultado["fnValorTotalFinanciamiento"].ToString());
                            var tipoDePlazo = sqlResultado["fcTipoDePlazo"].ToString();
                            var tipoDePlazoSufijoAl = sqlResultado["fcTipoPlazo"].ToString();
                            var varlorGarantia = decimal.Parse(sqlResultado["fnValorGarantia"].ToString());
                            var valorPrima = decimal.Parse(sqlResultado["fnValorPrima"].ToString());
                            var valorCuotaPrestamo = decimal.Parse(sqlResultado["fnCuotaMensualPrestamo"].ToString());
                            var valorCuotaGPS = decimal.Parse(sqlResultado["fnCuotaMensualGPS"].ToString());
                            var valorCuotaSeguro = decimal.Parse(sqlResultado["fnCuotaMensualSeguro"].ToString());
                            var valorCuotaTotal = decimal.Parse(sqlResultado["fnCuotaTotal"].ToString());
                            var valorParaCompraDeVehiculo = decimal.Parse(sqlResultado["fnValorAPrestar"].ToString());
                            var valorParaCompraDeGPS = decimal.Parse(sqlResultado["fnCostoGPS"].ToString());
                            var valorParaGastosDeCierre = decimal.Parse(sqlResultado["fnGastosDeCierre"].ToString());
                            var moneda = sqlResultado["fcNombreMoneda"].ToString();
                            var monedaSimbolo = sqlResultado["fcSimboloMoneda"].ToString();
                            var monedaAbreviatura = sqlResultado["fcAbreviaturaMoneda"].ToString();
                            var tasaDeInteresSimpleMensual = decimal.Parse(sqlResultado["fnTasaMensualAplicada"].ToString());
                            var tasaDeInteresAnualAplicada = decimal.Parse(sqlResultado["fnTasaAnualAplicada"].ToString());

                            lblIdSolicitud.InnerText = pcIDSolicitud;
                            txtNombreCliente.Text = nombreCliente;
                            txtIdentidadCliente.Text = identidad;
                            txtRtn.Text = RTN;
                            txtTelefonoCliente.Text = telefonoPrimario;
                            txtProducto.Text = producto;
                            txtMontoFinalAFinanciar.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", Convert.ToDecimal(valorTotalFinanciamiento));
                            txtPlazoFinanciar.Text = plazoFinalAprobado;
                            lblTipoDePlazo.InnerText = tipoDePlazoSufijoAl;
                            txtValorCuota.Text = valorCuotaTotal.ToString("N");
                            lblTipoDePlazoCuota.InnerText = tipoDePlazoSufijoAl;

                            int requiereGarantia = (byte)sqlResultado["fiRequiereGarantia"];

                            if (requiereGarantia == 1)
                            {
                                sqlResultado.NextResult();

                                if (!sqlResultado.HasRows)
                                {
                                    string lcScript = "window.open('Garantia_Registrar.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&SID=" + pcIDSolicitud + "&IDApp=" + pcIDApp + "&IDSOL=" + pcIDSolicitud) + "','_self')";
                                    Response.Write("<script>");
                                    Response.Write(lcScript);
                                    Response.Write("</script>");
                                }
                                else
                                {
                                    /* Informacion del garantía */
                                    while (sqlResultado.Read())
                                    {
                                        var VIN = sqlResultado["fcVin"].ToString();
                                        var tipoDeGarantia = sqlResultado["fcTipoGarantia"].ToString();
                                        var tipoDeVehiculo = sqlResultado["fcTipoVehiculo"].ToString();
                                        var marca = sqlResultado["fcMarca"].ToString();
                                        var modelo = sqlResultado["fcModelo"].ToString();
                                        var anio = sqlResultado["fiAnio"].ToString();
                                        var color = sqlResultado["fcColor"].ToString();
                                        var matricula = sqlResultado["fcMatricula"].ToString();
                                        var serieMotor = sqlResultado["fcMotor"].ToString();
                                        var serieChasis = sqlResultado["fcChasis"].ToString();
                                        var GPS = sqlResultado["fcGPS"].ToString();
                                        var cilindraje = sqlResultado["fcCilindraje"].ToString();
                                        var recorridoNumerico = Convert.ToDecimal(sqlResultado["fnRecorrido"].ToString());
                                        var recorrido = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnRecorrido"].ToString())) + " " + sqlResultado["fcUnidadDeDistancia"].ToString();
                                        var transmision = sqlResultado["fcTransmision"].ToString();
                                        var tipoDeCombustible = sqlResultado["fcTipoCombustible"].ToString();
                                        var serieUno = sqlResultado["fcSerieUno"].ToString();
                                        var serieDos = sqlResultado["fcSerieDos"].ToString();
                                        var comentario = sqlResultado["fcComentario"].ToString().Trim();

                                        /* Propietario de la garantia */
                                        var nombrePropietarioGarantia = sqlResultado["fcNombrePropietarioGarantia"].ToString();
                                        var identidadPropietarioGarantia = sqlResultado["fcIdentidadPropietarioGarantia"].ToString();
                                        var nacionalidadPropietarioGarantia = sqlResultado["fcNacionalidadPropietarioGarantia"].ToString();
                                        var estadoCivilPropietarioGarantia = sqlResultado["fcEstadoCivilPropietarioGarantia"].ToString();

                                        /* Vendedor de la garantia. Puede o no ser el mismo propietario */
                                        var nombreVendedorGarantia = sqlResultado["fcNombreVendedorGarantia"].ToString();
                                        var identidadVendedorGarantia = sqlResultado["fcIdentidadVendedorGarantia"].ToString();
                                        var nacionalidadVendedorGarantia = sqlResultado["fcNacionalidadVendedorGarantia"].ToString();
                                        var estadoCivilVendedorGarantia = sqlResultado["fcEstadoCivilVendedorGarantia"].ToString();

                                        txtVIN.Text = VIN;
                                        txtTipoDeGarantia.Text = tipoDeGarantia;
                                        txtTipoDeVehiculo.Text = tipoDeVehiculo;
                                        txtMarca.Text = marca;
                                        txtModelo.Text = modelo;
                                        txtAnio.Text = anio;
                                        txtColor.Text = color;
                                        txtMatricula.Text = matricula;
                                        txtSerieMotor.Text = serieMotor;
                                        txtSerieChasis.Text = serieChasis;
                                        txtGPS.Text = GPS;
                                        txtCilindraje.Text = cilindraje;
                                        txtRecorrido.Text = recorrido;
                                        txtTransmision.Text = transmision;
                                        txtTipoDeCombustible.Text = tipoDeCombustible;
                                        txtSerieUno.Text = serieUno;
                                        txtSerieDos.Text = serieDos;
                                        txtComentario.InnerText = comentario;

                                        txtNombrePropietarioGarantia.Text = nombrePropietarioGarantia;
                                        txtIdentidadPropietarioGarantia.Text = identidadPropietarioGarantia;
                                        txtNacionalidadPropietarioGarantia.Text = nacionalidadPropietarioGarantia;
                                        txtEstadoCivilPropietarioGarantia.Text = estadoCivilPropietarioGarantia;
                                        txtNombreVendedorGarantia.Text = nombreVendedorGarantia;
                                        txtIdentidadVendedorGarantia.Text = identidadVendedorGarantia;
                                        txtNacionalidadVendedorGarantia.Text = nacionalidadVendedorGarantia;
                                        txtEstadoCivilVendedorGarantia.Text = estadoCivilVendedorGarantia;

                                        /* Contrato */
                                        lblNombre_Contrato.Text = nombreCliente;
                                        lblNacionalidad_Contrato.Text = nacionalidad;
                                        lblIdentidad_Contrato.Text = identidad;
                                        lblDireccion_Contrato.Text = direccionCliente;
                                        lblCorreo_Contrato.Text = correoCliente;

                                        lblMontoPrestamoEnPalabras_Contrato.Text = ConvertirCantidadALetras(valorTotalFinanciamiento.ToString()) + " " + moneda;
                                        lblMontoPrestamo_Contrato.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", Convert.ToDecimal(valorTotalFinanciamiento));

                                        lblMontoParaCompraVehiculoEnPalabras_Contrato.Text = ConvertirCantidadALetras(valorParaCompraDeVehiculo.ToString()) + " " + moneda;
                                        lblMontoParaCompraVehiculo_Contrato.Text = monedaSimbolo + " " + valorParaCompraDeVehiculo.ToString("N");

                                        lblMontoParaCompraGPSEnPalabras_Contrato.Text = ConvertirCantidadALetras(valorParaCompraDeGPS.ToString()) + " " + moneda;
                                        lblMontoParaCompraGPS_Contrato.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", valorParaCompraDeGPS);

                                        lblMontoGastosDeCierreEnPalabras_Contrato.Text = ConvertirCantidadALetras(valorParaGastosDeCierre.ToString()) + " " + moneda;
                                        lblMontoGastosDeCierre_Contrato.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", valorParaGastosDeCierre);

                                        lblMarca_Contrato.Text = marca;
                                        lblTipoVehiculo_Contrato.Text = tipoDeVehiculo;
                                        lblModelo_Contrato.Text = modelo;
                                        lblAnio_Contrato.Text = anio;
                                        lblColor_Contrato.Text = color;
                                        lblCilindraje_Contrato.Text = cilindraje;
                                        lblMatricula_Contrato.Text = matricula;
                                        lblVIN_Contrato.Text = VIN;
                                        lblNumeroMotor_Contrato.Text = serieMotor;
                                        lblSerieChasis_Contato.Text = serieChasis;

                                        lblTasaInteresSimpleMensual_Contrato.Text = tasaDeInteresSimpleMensual.ToString("N");
                                        lblTipoDePlazo_Contrato.Text = tipoDePlazo;
                                        lblCAT_Contrato.Text = tasaDeInteresAnualAplicada.ToString("N");
                                        lblMontoPrima_Contrato.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", Convert.ToDecimal(valorPrima));
                                        lblPlazo_Contrato.Text = plazoFinalAprobado;
                                        lblFrecuenciaPago_Contrato.Text = tipoDePlazo;

                                        lblValorCuotaPalabras_Contrato.Text = ConvertirCantidadALetras(valorCuotaPrestamo.ToString()) + " " + moneda; // pendiente
                                        lblValorCuota_Contrato.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", valorCuotaPrestamo);

                                        lblPlazoGPS_Contrato.Text = plazoFinalAprobado;
                                        lblValorCuotaGPSPalabras_Contrato.Text = ConvertirCantidadALetras(valorCuotaGPS.ToString()) + " " + moneda; // pendiente
                                        lblValorCuotaGPS_Contrato.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", valorCuotaGPS);

                                        lblPlazoSeguro_Contrato.Text = plazoFinalAprobado;
                                        lblValorCuotaSeguroPalabras_Contrato.Text = ConvertirCantidadALetras(valorCuotaSeguro.ToString()) + " " + moneda; ;
                                        lblValorCuotaSeguro_Contrato.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", valorCuotaSeguro);

                                        lblFechaPrimerCuota_Contrato.Text = DiaPrimerPago.ToString() + " de " + MesPrimerPago + " del " + AnioPrimerPago.ToString();
                                        lblPlazoPago_Contrato.Text = tipoDePlazo;

                                        lblMontoTotalPrestamoPalabras_Contrato.Text = ConvertirCantidadALetras(montoTotalContrato.ToString()) + " " + moneda; ;
                                        lblMontoTotalPrestamo_Contrato.Text = monedaSimbolo + " " + montoTotalContrato.ToString("N");

                                        lblNombreFirma_Contrato.Text = nombreCliente;
                                        lblIdentidadFirma_Contrato.Text = identidad;

                                        /* Pagare */
                                        lblMontoTitulo_Pagare.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", Convert.ToDecimal(valorTotalFinanciamiento));
                                        lblNombre_Pagare.Text = nombreCliente;
                                        lblEstadoCivil_Pagare.Text = estadoCivil;
                                        lblNacionalidad_Pagare.Text = nacionalidad;
                                        lblProfesion_Pagare.Text = profesionOficio;
                                        lblIdentidad_Pagare.Text = identidad;
                                        lblDireccion_Pagare.Text = direccionCliente;
                                        lblMontoPalabras_Pagare.Text = ConvertirCantidadALetras(valorTotalFinanciamiento.ToString()) + " " + moneda; ;
                                        lblMontoDigitos_Pagare.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", Convert.ToDecimal(valorTotalFinanciamiento));

                                        lblDiaPrimerPago_Pagare.Text = "________";
                                        lblMesPrimerPago_Pagare.Text = "________________";
                                        lblAnioPrimerPago_Pagare.Text = "________________";

                                        lblPorcentajeInteresFluctuante_Pagare.Text = tasaDeInteresSimpleMensual.ToString("N");
                                        lblInteresesMoratorios_Pagare.Text = "4.52";
                                        lblNombreFirma_Pagare.Text = nombreCliente;
                                        lblIdentidadFirma_Pagare.Text = identidad;

                                        /* Compromiso legal */
                                        lblNombreCliente_CompromisoLegal.Text = nombreCliente;
                                        lblCantidadCuotas_CompromisoLegal.Text = plazoFinalAprobado;
                                        lblValorCuotaPalabras_CompromisoLegal.Text = ConvertirCantidadALetras(valorCuotaTotal.ToString()) + " " + moneda; ;
                                        lblValorCuota_CompromisoLegal.Text = monedaSimbolo + " " + valorCuotaTotal.ToString("N");
                                        lblGarantiaUsada_CompromisoLegal.Text = /*Convert.ToDecimal(recorridoNumerico) < 1 ? "nuevo" :*/ "usado";
                                        lblIdentidadFirma_CompromisoLegal.Text = identidad;
                                        /*lblNombreFirma_CompromisoLegal.Text = nombreCliente; */

                                        /* Convenio de compra y venta de vehiculos para financiamiento a tercero */
                                        lblNombreCliente_ConvenioCyV.Text = nombreVendedorGarantia;
                                        lblNacionalidad_ConvenioCyV.Text = nacionalidadVendedorGarantia;
                                        lblEstadoCivil_ConvenioCyV.Text = estadoCivilVendedorGarantia;
                                        lblIdentidad_ConvenioCyV.Text = identidadVendedorGarantia;
                                        lblCiudadCliente_ConvenioCyV.Text = ciudadPoblado;
                                        lblMarca_ConvenioCyV.Text = marca;
                                        lblModelo_ConvenioCyV.Text = modelo;
                                        lblAnio_ConvenioCyV.Text = anio;
                                        lblSerieMotor_ConvenioCyV.Text = serieMotor;
                                        lblSerieChasis_ConvenioCyV.Text = serieChasis;
                                        lblTipoVehiculoConvenioCyV.Text = tipoDeVehiculo;
                                        lblColor_ConvenioCyV.Text = color;
                                        lblCilindraje_ConvenioCyV.Text = cilindraje;
                                        lblMatricula_ConvenioCyV.Text = matricula;
                                        lblVIN_ConvenioCyV.Text = VIN;
                                        lblNombre_ConvenioCyV.Text = nombreVendedorGarantia.ToUpper();

                                        /* Inspeccion seguro */
                                        lblNombre_InspeccionSeguro.Text = nombreCliente;
                                        lblMarca_InspeccionSeguro.Text = marca;
                                        lblModelo_InspeccionSeguro.Text = modelo;
                                        lblAnio_InspeccionSeguro.Text = anio;
                                        lblTipoDeVehiculo_InspeccionSeguro.Text = tipoDeVehiculo;
                                        lblRecorrido_InspeccionSeguro.Text = recorrido;
                                        lblMatricula_InspeccionSeguro.Text = matricula;

                                        /* Traspaso cliente */
                                        lblNombreCliente_Traspaso.Text = nombreCliente;
                                        lblIdentidad_Traspaso.Text = identidad;
                                        lblNacionalidad_Traspaso.Text = nacionalidad;
                                        /*lblDireccion_Traspaso.Text = direccionCliente;*/
                                        lblMarca_Traspaso.Text = marca;
                                        lblModelo_Traspaso.Text = modelo;
                                        lblSerieMotor_Traspaso.Text = serieMotor;
                                        lblVIN_Traspaso.Text = VIN;
                                        lblAnio_Traspaso.Text = anio;
                                        lblCilindraje_Traspaso.Text = cilindraje;
                                        lblTipoDeVehiculo_Traspaso.Text = tipoDeVehiculo;
                                        lblColor_Traspaso.Text = color;
                                        lblSerieChasis_Traspaso.Text = serieChasis;
                                        lblMatricula_Traspaso.Text = matricula;
                                        lblGarantiaUsada_Traspaso.Text = /* Convert.ToDecimal(recorridoNumerico) < 1 ? "nuevo" :*/ "usado";

                                        /* Traspaso vendedor */
                                        lblNombreCliente_TraspasoVendedor.Text = nombrePropietarioGarantia;
                                        lblIdentidad_TraspasoVendedor.Text = identidadPropietarioGarantia;
                                        lblNacionalidad_TraspasoVendedor.Text = nacionalidadPropietarioGarantia;
                                        lblMarca_TraspasoVendedor.Text = marca;
                                        lblModelo_TraspasoVendedor.Text = modelo;
                                        lblSerieMotor_TraspasoVendedor.Text = serieMotor;
                                        lblVIN_TraspasoVendedor.Text = VIN;
                                        lblAnio_TraspasoVendedor.Text = anio;
                                        lblCilindraje_TraspasoVendedor.Text = cilindraje;
                                        lblTipoDeVehiculo_TraspasoVendedor.Text = tipoDeVehiculo;
                                        lblColor_TraspasoVendedor.Text = color;
                                        lblSerieChasis_TraspasoVendedor.Text = serieChasis;
                                        lblMatricula_TraspasoVendedor.Text = matricula;
                                        lblGarantiaUsada_TraspasoVendedor.Text = /* Convert.ToDecimal(recorridoNumerico) < 1 ? "nuevo" :*/ "usado";

                                        /* Básico + CPI*/
                                        lblNombreCliente_BasicoCPI.Text = nombreCliente;
                                        lblNumeroPrestamo_BasicoCPI.Text = numeroPrestamo;

                                        /* Recibo */
                                        lblFecha_Recibo.Text = DateTime.Now.ToString("MM/dd/yyyy");
                                        lblSumaRecibidaEnPalabras_Recibo.Text = ConvertirCantidadALetras(valorParaCompraDeVehiculo.ToString());
                                        lblMarca_Recibo.Text = marca;
                                        lblModelo_Recibo.Text = modelo;
                                        lblAnio_Recibo.Text = anio;
                                        lblColor_Recibo.Text = color;
                                        lblTipo_Recibo.Text = tipoDeVehiculo;
                                        lblCilindraje_Recibo.Text = cilindraje;
                                        lblSerieMotor_Recibo.Text = serieMotor;
                                        lblVIN_Recibo.Text = VIN;
                                        lblSerieChasis_Recibo.Text = serieChasis;
                                        lblPlaca_Recibo.Text = matricula;
                                        lblNombreCliente_Recibo.Text = nombreCliente.ToUpper();
                                        lblTotalRecibido_Recibo.Text = monedaSimbolo + " " + valorParaCompraDeVehiculo.ToString("N");
                                        lblNombreVendedor_Recibo.Text = nombreVendedorGarantia;
                                        lblIdentidadVendedor_Recibo.Text = identidadVendedorGarantia;

                                        /* Correo liquidacion */
                                        lblAño_CorreoLiquidacion.Text = anio;
                                        lblPlaca_CorreoLiquidacion.Text = matricula;
                                        lblMarca_CorreoLiquidacion.Text = marca;
                                        lblModelo_CorreoLiquidacion.Text = modelo;
                                        lblTipoVehiculo_CorreoLiquidacion.Text = tipoDeVehiculo;
                                        lblColor_CorreoLiquidacion.Text = color;
                                        lblSerieMotor_CorreoLiquidacion.Text = serieMotor;
                                        lblSerieChasis_CorreoLiquidacion.Text = serieChasis;
                                        lblVIN_CorreoLiquidacion.Text = VIN;
                                        lblNombreCliente_CorreoLiquidacion.Text = nombreCliente;
                                        lblIdentidadCliente_CorreoLiquidacion.Text = identidad;
                                        lblNumeroPrestamo_CorreoLiquidacion.Text = "";
                                        lblNombreVendedor_CorreoLiquidacion.Text = nombreVendedorGarantia;
                                        lblIdentidadVendedor_CorreoLiquidacion.Text = identidadVendedorGarantia;
                                        lblValorNumero_CorreoLiquidacion.Text = monedaSimbolo + " " + valorParaCompraDeVehiculo.ToString("N");
                                        lblValorLetra_CorreoLiquidacion.Text = ConvertirCantidadALetras(valorParaCompraDeVehiculo.ToString()) + " " + moneda;

                                        /* Correo seguro */
                                        lblAño_CorreoSeguro.Text = anio;
                                        lblPlaca_CorreoSeguro.Text = matricula;
                                        lblMarca_CorreoSeguro.Text = marca;
                                        lblModelo_CorreoSeguro.Text = modelo;
                                        lblTipoVehiculo_CorreoSeguro.Text = tipoDeVehiculo;
                                        lblColor_CorreoSeguro.Text = color;
                                        lblSerieMotor_CorreoSeguro.Text = serieMotor;
                                        lblSerieChasis_CorreoSeguro.Text = serieChasis;
                                        lblVIN_CorreoSeguro.Text = VIN;
                                        lblNombreCliente_CorreoSeguro.Text = nombreCliente;
                                        lblIdentidadCliente_CorreoSeguro.Text = identidad;
                                        lblNumeroPrestamo_CorreoSeguro.Text = "";

                                        /* Nota de entrega */
                                        //lblVendedorGarantia_NotaEntrega.Text = nombreVendedorGarantia;
                                        lblNombreCliente_NotaEntrega.Text = nombreCliente;
                                        lblValorAPrestarEnPalabras_NotaEntrega.Text = ConvertirCantidadALetras(valorParaCompraDeVehiculo.ToString());
                                        lblValorAPrestar_NotaEntrega.Text = monedaSimbolo + " " + valorParaCompraDeVehiculo.ToString("N");

                                        lblEstadoGarantia_NotaEntrega.Text = /* Convert.ToDecimal(recorridoNumerico) < 1 ? "nuevo" :*/ "usado";
                                        lblMarca_NotaEntrega.Text = marca;
                                        lblModelo_NotaEntrega.Text = modelo;
                                        lblAño_NotaEntrega.Text = anio;
                                        lblTipo_NotaEntrega.Text = tipoDeVehiculo;
                                        lblChasis_NotaEntrega.Text = serieChasis;
                                        lblSerieMotor_NotaEntrega.Text = serieMotor;
                                        lblColor_NotaEntrega.Text = color;
                                        lblCilindraje_NotaEntrega.Text = cilindraje;
                                        lblPlaca_NotaEntrega.Text = matricula;
                                        lblNombreVendedorGarantia_NotaEntrega.Text = nombreVendedorGarantia;

                                        /* Expediente */
                                        lblNoSolicitudCredito_Expediente.InnerText = pcIDSolicitud;
                                        lblFechaActual_Expediente.InnerText = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
                                        lblNombreCliente_Expediente.InnerText = nombreCliente;
                                        lblIdentidadCliente_Expediente.InnerText = identidad;
                                        lblDepartamento_Expediente.InnerText = departamentoResidencia;
                                        lblDireccionCliente_Expediente.InnerText = direccionCliente;
                                        lblTelefonoCliente_Expediente.InnerText = telefonoPrimario;

                                        lblTipoDeTrabajo_Expediente.InnerText = nombreTrabajo;
                                        lblPuestoAsignado_Expediente.InnerText = puestoAsignado;
                                        lblTelefonoTrabajo_Expediente.InnerText = telefonoTrabajo;
                                        lblDirecciónTrabajo_Expediente.InnerText = direccionTrabajo;

                                        lblNoSolicitud_Expediente.InnerText = pcIDSolicitud;
                                        lblFechaOtorgamiento_Expediente.InnerText = fechaOtorgamiento.ToString("dd/MM/yyyy");
                                        lblCantidadCuotas_Expediente.InnerText = plazoFinalAprobado + " Cuotas";
                                        lblMontoOtorgado_Expediente.InnerText = valorTotalFinanciamiento.ToString("N");
                                        lblValorCuota_Expediente.InnerText = valorCuotaTotal.ToString("N");
                                        lblFechaPrimerPago_Expediente.InnerText = fechaPrimerPago.ToString("dd/MM/yyyy");
                                        lblFrecuenciaPlazo_Expediente.InnerText = tipoDePlazoSufijoAl;
                                        lblFechaVencimiento_Expediente.InnerText = fechaVencimiento;
                                        lblOficialNegocios_Expediente.InnerText = oficialDeNegocios;
                                        lblGestor_Expediente.InnerText = gestorDeCobros;

                                    }

                                    sqlResultado.NextResult();

                                    /* Fotografías de la garantía */
                                    if (!sqlResultado.HasRows)
                                    {
                                        divGaleriaGarantia.InnerHtml = "<img alt='No hay fotografías disponibles' src='/Imagenes/Imagen_no_disponible.png' data-image='/Imagenes/Imagen_no_disponible.png' data-description='No hay fotografías disponibles'/>";
                                        divContenedorInspeccionSeguro.Visible = false;
                                    }
                                    else
                                    {
                                        var imagenesGarantia = new StringBuilder();
                                        var imagenesGarantiaParaInspeccionDeSeguro = new StringBuilder();

                                        while (sqlResultado.Read())
                                        {
                                            imagenesGarantia.Append("<img alt='" + sqlResultado["fcSeccionGarantia"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcSeccionGarantia"] + "'/>");

                                            if ((bool)sqlResultado["fbApareceEnInspeccionDeSeguro"] == true)
                                            {
                                                imagenesGarantiaParaInspeccionDeSeguro.Append("<img alt='" + sqlResultado["fcSeccionGarantia"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcSeccionGarantia"] + "'/>");
                                            }
                                        }

                                        divGaleriaGarantia.InnerHtml = imagenesGarantia.ToString();
                                        divGaleriaInspeccionSeguroDeVehiculo.InnerHtml = imagenesGarantiaParaInspeccionDeSeguro.ToString();
                                    }
                                }
                                divInformacionGarantia.Visible = true;

                            } // if requiereGarantia == 1
                        } // while sqlResultado.Read()
                    } // using executeReader
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            var mensajeExtra = string.Empty;

            if (int.Parse(pcIDSolicitud) < 802)
            {
                mensajeExtra = "Esta opción está disponible a partir de la solicitud de crédito No. 802";
            }

            MostrarMensaje("Error al cargar información de la solicitud " + pcIDSolicitud + ": " + ex.Message.ToString() + " " + mensajeExtra);
            divContenedorInspeccionSeguro.Visible = false;
        }
    }

    [WebMethod]
    public static bool EnviarDocumentoPorCorreo(string asunto, string tituloGeneral, string contenidoHtml, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            var buzonCorreoUsuario = string.Empty;

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreSeguridad.dbo.sp_InformacionUsuario", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            buzonCorreoUsuario = sqlResultado["fcBuzondeCorreo"].ToString();
                        }
                    } // using reader
                } // using command
            } // using connection

            resultado = EnviarCorreo(asunto, tituloGeneral, tituloGeneral, contenidoHtml, buzonCorreoUsuario);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultado;
    }

    public static bool EnviarCorreo(string pcAsunto, string pcTituloGeneral, string pcSubtitulo, string pcContenidodelMensaje, string buzonCorreoUsuario)
    {
        var resultado = false;
        try
        {
            var pmmMensaje = new MailMessage();
            var smtpCliente = new SmtpClient();

            smtpCliente.Host = "mail.miprestadito.com";
            smtpCliente.Port = 587;
            smtpCliente.Credentials = new System.Net.NetworkCredential("systembot@miprestadito.com", "iPwf@p3q");
            smtpCliente.EnableSsl = true;

            pmmMensaje.Subject = pcAsunto;
            pmmMensaje.From = new MailAddress("systembot@miprestadito.com", "System Bot");
            pmmMensaje.To.Add("willian.diaz@miprestadito.com");
            //pmmMensaje.To.Add(buzonCorreoUsuario);
            pmmMensaje.CC.Add(buzonCorreoUsuario);
            pmmMensaje.IsBodyHtml = true;

            string htmlString = @"<!DOCTYPE html> " +
                        "<html>" +
                        "<body>" +
                        "    <div style=\"width: 500px;\">" +
                        "        <table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                        "            <tr style=\"height: 30px; background-color:#56396b; font-family: 'Microsoft Tai Le'; font-size: 14px; font-weight: bold; color: white;\">" +
                        "                <td style=\"vertical-align: central; text-align:center;\">" + pcTituloGeneral + "</td>" +
                        "            </tr>" +
                        "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        "                <td>&nbsp;</td>" +
                        "            </tr>" +
                        "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        "                <td style=\"background-color:whitesmoke; text-align:center;\">Datos del cliente y la garantía</td>" +
                        "            </tr>" +
                        "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        "                <td>&nbsp;</td>" +
                        "            </tr>" +
                        "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        "                <td style=\"vertical-align: central;\">" + pcContenidodelMensaje + "</td>" +
                        "            </tr>" +
                        "            <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                        "                <td>&nbsp;</td>" +
                        "            </tr>" +
                        "            <tr style=\"height: 20px; font-family: 'Microsoft Tai Le'; font-size: 12px; text-align:center;\">" +
                        "                <td>System Bot Prestadito</td>" +
                        "            </tr>" +
                        "        </table>" +
                        "    </div>" +
                        "</body> " +
                        "</html> ";

            pmmMensaje.Body = htmlString;

            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
            smtpCliente.Send(pmmMensaje);

            smtpCliente.Dispose();

            resultado = true;
        }
        catch (Exception ex)
        {
            ExceptionLogging.SendExcepToDB(ex);
            resultado = false;
        }

        return resultado;
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

    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text = mensaje;
    }

    #region Convertir cantidad a palabras

    public static string ConvertirCantidadALetras(string num)
    {
        string res, dec = "";
        Int64 entero;
        int decimales;
        double nro;

        try
        {
            nro = Convert.ToDouble(num);
        }
        catch
        {
            return "";
        }

        entero = Convert.ToInt64(Math.Truncate(nro));
        decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));

        dec = " CON " + PadNum(decimales) + "/100 CTVS";

        res = toText(Convert.ToDouble(entero)) + dec;
        return res;
    }

    private static string toText(double value)
    {
        string Num2Text = "";
        value = Math.Truncate(value);
        if (value == 0) Num2Text = "CERO";
        else if (value == 1) Num2Text = "UNO";
        else if (value == 2) Num2Text = "DOS";
        else if (value == 3) Num2Text = "TRES";
        else if (value == 4) Num2Text = "CUATRO";
        else if (value == 5) Num2Text = "CINCO";
        else if (value == 6) Num2Text = "SEIS";
        else if (value == 7) Num2Text = "SIETE";
        else if (value == 8) Num2Text = "OCHO";
        else if (value == 9) Num2Text = "NUEVE";
        else if (value == 10) Num2Text = "DIEZ";
        else if (value == 11) Num2Text = "ONCE";
        else if (value == 12) Num2Text = "DOCE";
        else if (value == 13) Num2Text = "TRECE";
        else if (value == 14) Num2Text = "CATORCE";
        else if (value == 15) Num2Text = "QUINCE";
        else if (value < 20) Num2Text = "DIECI" + toText(value - 10);
        else if (value == 20) Num2Text = "VEINTE";
        else if (value < 30) Num2Text = "VEINTI" + toText(value - 20);
        else if (value == 30) Num2Text = "TREINTA";
        else if (value == 40) Num2Text = "CUARENTA";
        else if (value == 50) Num2Text = "CINCUENTA";
        else if (value == 60) Num2Text = "SESENTA";
        else if (value == 70) Num2Text = "SETENTA";
        else if (value == 80) Num2Text = "OCHENTA";
        else if (value == 90) Num2Text = "NOVENTA";
        else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " Y " + toText(value % 10);
        else if (value == 100) Num2Text = "CIEN";
        else if (value < 200) Num2Text = "CIENTO " + toText(value - 100);
        else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "CIENTOS";
        else if (value == 500) Num2Text = "QUINIENTOS";
        else if (value == 700) Num2Text = "SETECIENTOS";
        else if (value == 900) Num2Text = "NOVECIENTOS";
        else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
        else if (value == 1000) Num2Text = "MIL";
        else if (value < 2000) Num2Text = "MIL " + toText(value % 1000);
        else if (value < 1000000)
        {
            Num2Text = toText(Math.Truncate(value / 1000)) + " MIL";
            if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
        }

        else if (value == 1000000) Num2Text = "UN MILLON";
        else if (value < 2000000) Num2Text = "UN MILLON " + toText(value % 1000000);
        else if (value < 1000000000000)
        {
            Num2Text = toText(Math.Truncate(value / 1000000)) + " MILLONES ";
            if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
        }

        else if (value == 1000000000000) Num2Text = "UN BILLON";
        else if (value < 2000000000000) Num2Text = "UN BILLON " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

        else
        {
            Num2Text = toText(Math.Truncate(value / 1000000000000)) + " BILLONES";
            if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
        }
        return Num2Text;

    }

    private static string PadNum(int entero)
    {
        return entero < 9 ? "0" + entero : entero.ToString();
    }
    #endregion

    #region View Models

    public class InformacionPrincipal_Cliente_Solicitud_ViewModel
    {
        public int IdSolicitud { get; set; }
        public string Identidad { get; set; }
        public string Nombre { get; set; }
        public string DireccionDomicilio { get; set; }
        public string Correo { get; set; }
        public string Nacionalidad { get; set; }
        public string EstadoCivil { get; set; }
        public Garantia_ViewModel Garantia { get; set; }
    }

    public class Garantia_ViewModel
    {
        public int IdGarantia { get; set; }
        public string VIN { get; set; }
        public string TipoDeVehiculo { get; set; }
        public string TipoDeGarantia { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Anio { get; set; }
        public string Color { get; set; }
        public string Cilindraje { get; set; }
        public decimal Recorrido { get; set; }
        public string UnidadDeDistancia { get; set; }
        public string Transmision { get; set; }
        public string Matricula { get; set; }
        public string SerieUno { get; set; }
        public string SerieDos { get; set; }
        public string SerieChasis { get; set; }
        public string SerieMotor { get; set; }
        public string GPS { get; set; }
        public string Comentario { get; set; }
    }

    public class CorreoViewModel
    {
        public string Anio { get; set; }
        public string Placa { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string TipoDeGarantia { get; set; }
        public string Color { get; set; }
        public string SerieMotor { get; set; }
        public string SerieChasis { get; set; }
        public string VIN { get; set; }

        public string NombreDelCliente { get; set; }
        public string IdentidadDeCliente { get; set; }
        public string NumeroPrestamo { get; set; }
        public string NombreVendedor { get; set; }
        public string IdentidadVendedor { get; set; }
        public decimal ValorTotal { get; set; }
    }
    #endregion
}