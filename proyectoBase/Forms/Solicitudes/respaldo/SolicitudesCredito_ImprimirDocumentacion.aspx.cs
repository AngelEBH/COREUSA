using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class SolicitudesCredito_ImprimirDocumentacion : System.Web.UI.Page
{
    #region Propiedades publicas

    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDSolicitud = "";
   
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    public string DiasFirma { get; set; }
    public string MesFirma { get; set; }
    public string AnioFirma { get; set; }
    public string CiudadFirma { get; set; }
    public string DepartamentoFirma { get; set; }

    public string DiaPrimerPago { get; set; }
    public string MesPrimerPago { get; set; }
    public string AnioPrimerPago { get; set; }

    public string FondoPrestamoJSON { get; set; }

    public string UrlCodigoQR { get; set; }
    public string ListaDocumentosDelExpedienteJSON { get; set; }
    public string Frecuencia = "";

    #endregion

    #region Page Load

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            var type = Request.QueryString["type"];

            if (!IsPostBack && type == null)
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

                if (lcParametros != string.Empty)
                {
                    var lcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
                    UrlCodigoQR = "http://190.92.0.76/OPS/CFRM.aspx?" + DSC.Encriptar("S=" + pcIDSolicitud);

                    var hoy = DateTime.Today;
                    DiasFirma = hoy.Day.ToString();
                    MesFirma = hoy.ToString("MMMM", new CultureInfo("es-ES"));
                    AnioFirma = hoy.Year.ToString();

                    CargarInformacion();
                    CargarExpedienteDeLaSolicitud();
                    CargarPlanDePagos();

                    HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
                    HttpContext.Current.Session["ListaDocumentosParaAsegurar"] = null;
                    Session.Timeout = 10080;

                    divContenedorPortadaExpediente.Visible = true;
                    divContenedorInspeccionSeguro.Visible = true;
                }
            }

            /* Guardar documentos de la solicitud */
            if (type != null || Request.HttpMethod == "POST")
            {
                Session["tipoDoc"] = Convert.ToInt32(Request.QueryString["IdDocumentoAsegurar"]);
                var uploadDir = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";

                var fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
                    { "limit", 1 },
                    { "title", "auto" },
                    { "uploadDir", uploadDir },
                    { "extensions", new string[] { "jpg", "png", "jpeg"} },
                    { "maxSize", 500 }, /* Peso máximo de todos los archivos seleccionado en megas (MB) */
                    { "fileMaxSize", 20 }, /* Peso máximo por archivo */
                    });

                switch (type)
                {
                    case "upload": /* Guardar achivo en carpeta temporal y guardar la informacion del mismo en una variable de sesion */

                        var data = fileUploader.Upload();

                        if (data["files"].Count == 1)
                            data["files"][0].Remove("file");
                        Response.Write(JsonConvert.SerializeObject(data));

                        /* Al subirse los archivos se guardan en este objeto de sesion general del helper fileuploader */
                        var list = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                        /* Guardar listado de documentos en una session propia de esta pantalla */
                        Session["ListaDocumentosParaAsegurar"] = list;

                        break;

                    case "remove":
                        string file = Request.Form["file"];

                        if (file != null)
                        {
                            file = FileUploader.FullDirectory(uploadDir) + file;
                            if (File.Exists(file))
                                File.Delete(file);
                        }
                        break;
                }
                Response.End();
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
        }
    }

    #endregion

    #region Cargar información de la solicitud y expediente de la solicitud

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
                            Response.Write("<script>window.open('SolicitudesCredito_ListadoGarantias.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&SID=" + pcIDSolicitud + "&IDApp=" + pcIDApp) + "','_self')</script>");

                        while (sqlResultado.Read())
                        {
                            var fechaPrimerPago = (DateTime)sqlResultado["fdFechaPrimerCuota"];
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

                            if (sqlResultado["fdTiempoTomaDecisionFinal"] == DBNull.Value)
                            {
                                MostrarMensaje("No puede imprimir los documentos de esta solicitud antes de que esta sea aprobada.");
                            }

                            var fechaOtorgamiento = (DateTime)sqlResultado["fdTiempoTomaDecisionFinal"];
                            var oficialDeNegocios = sqlResultado["fcOficialDeNegocios"].ToString();
                            var oficialDeNegociosCentroDeCosto = sqlResultado["fcCentroDeCostoOFicialDeNegocios"].ToString();
                            var gestorDeCobros = sqlResultado["fcGestorDeCobros"].ToString();
                            var departamentoResidencia = sqlResultado["fcDepartamento"].ToString();
                            var ciudadPoblado = sqlResultado["fcPoblado"].ToString();
                            var direccionCliente = sqlResultado["fcDireccionCliente"].ToString();

                            /* Información de la solicitud y prestamo */
                            var idProducto = sqlResultado["fiIDProducto"].ToString();
                            var producto = sqlResultado["fcProducto"].ToString();
                            var montoTotalContrato = decimal.Parse(sqlResultado["fnValorTotalContrato"].ToString());
                            var plazoFinalAprobado = sqlResultado["fiPlazo"].ToString();
                            var valorTotalFinanciamiento = decimal.Parse(sqlResultado["fnValorTotalFinanciamiento"].ToString());
                            var tipoDePlazo = sqlResultado["fcTipoDePlazo"].ToString();
                            var tipoDePlazoSufijoAl = sqlResultado["fcTipoPlazoSufijoAl"].ToString();
                            var tipoDePlazoSufijoMente = sqlResultado["fcTipoPlazoSufijoMente"].ToString();
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
                            var CuotalTotal = decimal.Parse(sqlResultado["CuotaTotal"].ToString());
                            var RazonSocial = sqlResultado["fcRazonSocial"].ToString().ToUpper();
                            var fcFrecuencia = sqlResultado["Frecuensia"].ToString();

                            Frecuencia = fcFrecuencia;

                           /* Información de los fondos del préstamo y el representante legal del mismo */
                           var fondosPrestamo = new Fondo_RepresentanteLegal_ViewModel()
                            {
                                //IdFondo = (int)sqlResultado["fiIDFondo"],
                                //RazonSocial = sqlResultado["fcRazonSocial"].ToString().ToUpper(),
                                //NombreComercial = sqlResultado["fcNombreComercial"].ToString().ToUpper(),
                                //EmpresaRTN = sqlResultado["fcRTNEmpresa"].ToString(),
                                //EmpresaCiudadDomiciliada = sqlResultado["fcCiudadDomiciliada"].ToString(),
                                //EmpresaDepartamentoDomiciliada = sqlResultado["fcDepartamentoDomiciliada"].ToString(),
                                //Telefono = sqlResultado["fcTelefono"].ToString(),
                                //Email = sqlResultado["fcEmail"].ToString(),
                                //Constitucion = sqlResultado["fcConstitucionFondo"].ToString(),
                                UrlLogo = sqlResultado["fcUrlLogo"].ToString(),

                                RepresentanteLegal = new RepresentanteLegal_ViewModel()
                                {
                                    //IdRepresentanteLegal = (int)sqlResultado["fiIDFondo"],
                                    //NombreCompleto = sqlResultado["fcNombreRepresentanteLegal"].ToString().ToUpper(),
                                    //Identidad = sqlResultado["fcIdentidadRepresentanteLegal"].ToString(),
                                    //EstadoCivil = sqlResultado["fcEstadoCivilRepresentanteLegal"].ToString(),
                                    //Nacionalidad = sqlResultado["fcNacionalidadRepresentanteLegal"].ToString(),
                                    //Prefesion = sqlResultado["fcProfesionRepresentanteLegal"].ToString(),
                                    //CiudadDomicilio = sqlResultado["fcCiudadDomicilioRepresentanteLegal"].ToString(),
                                    //DepartamentoDomicilio = sqlResultado["fcDepartamentoDomicilioRepresentanteLegal"].ToString()
                                }
                            };

                            FondoPrestamoJSON = JsonConvert.SerializeObject(fondosPrestamo);

                            lblIdSolicitud.InnerText = pcIDSolicitud;
                            txtNombreCliente.Text = nombreCliente;
                            //lblNombreCliente.InnerText = nombreCliente;
                            txtIdentidadCliente.Text = identidad;
                            txtRtn.Text = RTN;
                            txtTelefonoCliente.Text = telefonoPrimario;
                            txtProducto.Text = producto;

                            txtMontoFinalAFinanciar.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", Convert.ToDecimal(valorTotalFinanciamiento));
                            txtPlazoFinanciar.Text = plazoFinalAprobado;
                            lblTipoDePlazo.InnerText = tipoDePlazoSufijoAl;
                            txtValorCuota.Text = DecimalToString(valorCuotaTotal);
                            lblTipoDePlazoCuota.InnerText = tipoDePlazoSufijoAl;

                            if ((byte)sqlResultado["fiRequiereGarantia"] == 1)
                            {
                                /* Informacion del garantía */
                                sqlResultado.NextResult();

                                if (!sqlResultado.HasRows)
                                    Response.Write("<script>window.open('Garantia_Registrar.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&SID=" + pcIDSolicitud + "&IDApp=" + pcIDApp + "&IDSOL=" + pcIDSolicitud) + "','_self')</script>");

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

                                    if (sqlResultado["fiEstadoAsegurado"].ToString() != "0")
                                    {
                                        btnEnviarCorreoSeguro.Visible = true;
                                    }

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
                                    lblMontoParaCompraVehiculo_Contrato.Text = monedaSimbolo + " " + DecimalToString(valorParaCompraDeVehiculo);
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
                                    lblSerieChasis_Contrato.Text = serieChasis;
                                    lblTasaInteresSimpleMensual_Contrato.Text = DecimalToString(tasaDeInteresSimpleMensual < 1 ? tasaDeInteresSimpleMensual * 100 : tasaDeInteresSimpleMensual);
                                    lblTipoDePlazo_Contrato.Text = tipoDePlazoSufijoMente;
                                    lblCAT_Contrato.Text = DecimalToString((tasaDeInteresAnualAplicada < 1 ? tasaDeInteresAnualAplicada * 100 : tasaDeInteresAnualAplicada));
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
                                    lblPlazoPago_Contrato.Text = tipoDePlazoSufijoAl;
                                    lblMontoTotalPrestamoPalabras_Contrato.Text = ConvertirCantidadALetras(montoTotalContrato.ToString()) + " " + moneda; ;
                                    lblMontoTotalPrestamo_Contrato.Text = monedaSimbolo + " " + DecimalToString(montoTotalContrato);
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
                                    lblPorcentajeInteresFluctuante_Pagare.Text = DecimalToString(tasaDeInteresSimpleMensual < 1 ? tasaDeInteresSimpleMensual * 100 : tasaDeInteresSimpleMensual);
                                    lblInteresesMoratorios_Pagare.Text = "4.52";
                                    lblNombreFirma_Pagare.Text = nombreCliente;
                                    lblIdentidadFirma_Pagare.Text = identidad;

                                    /* Compromiso legal */
                                    lblNombreCliente_CompromisoLegal.Text = nombreCliente;
                                    lblCantidadCuotas_CompromisoLegal.Text = plazoFinalAprobado;
                                    lblValorCuotaPalabras_CompromisoLegal.Text = ConvertirCantidadALetras(valorCuotaTotal.ToString()) + " " + moneda; ;
                                    lblValorCuota_CompromisoLegal.Text = monedaSimbolo + " " + DecimalToString(valorCuotaTotal);
                                    lblGarantiaUsada_CompromisoLegal.Text = idProducto == "201" ? "NUEVO" : "USADO";
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
                                    lblGarantiaUsada_Traspaso.Text = idProducto == "201" ? "NUEVO" : "USADO";

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
                                    lblGarantiaUsada_TraspasoVendedor.Text = idProducto == "201" ? "NUEVO" : "USADO";

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
                                    lblTotalRecibido_Recibo.Text = monedaSimbolo + " " + DecimalToString(valorParaCompraDeVehiculo);
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
                                    lblValorNumero_CorreoLiquidacion.Text = monedaSimbolo + " " + DecimalToString(valorParaCompraDeVehiculo);
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

                                    /* Nota de entrega */
                                    lblPropietarioGarantia_NotaEntrega.Text = nombrePropietarioGarantia;
                                    lblNombreCliente_NotaEntrega.Text = nombreCliente;
                                    lblValorAPrestarEnPalabras_NotaEntrega.Text = ConvertirCantidadALetras(valorParaCompraDeVehiculo.ToString());
                                    lblValorAPrestar_NotaEntrega.Text = monedaSimbolo + " " + DecimalToString(valorParaCompraDeVehiculo);
                                    lblEstadoGarantia_NotaEntrega.Text = idProducto == "201" ? "NUEVO" : "USADO";
                                    lblMarca_NotaEntrega.Text = marca;
                                    lblModelo_NotaEntrega.Text = modelo;
                                    lblAño_NotaEntrega.Text = anio;
                                    lblTipo_NotaEntrega.Text = tipoDeVehiculo;
                                    lblChasis_NotaEntrega.Text = serieChasis;
                                    lblSerieMotor_NotaEntrega.Text = serieMotor;
                                    lblColor_NotaEntrega.Text = color;
                                    lblCilindraje_NotaEntrega.Text = cilindraje;
                                    lblPlaca_NotaEntrega.Text = matricula;

                                    /* Portada del expediente */
                                    lblNoSolicitud_PortadaExpediente.Text = "Solicitud de crédito #" + pcIDSolicitud;
                                    lblOficialNegocios_PortadaExpediente.Text = "Oficial de negocios: " + oficialDeNegocios;
                                    lblCentroDeCosto_PortadaExpediente.Text = "Centro de costo: " + oficialDeNegociosCentroDeCosto;
                                    lblFechaActual_PortadaExpediente.Text = DateTime.Now.ToString("MM/dd/yyyy");
                                    lblNombreCliente_PortadaExpediente.Text = nombreCliente;
                                    lblIdentidadCliente_PortadaExpediente.Text = identidad;
                                    lblMarca_PortadaExpediente.Text = marca;
                                    lblModelo_PortadaExpediente.Text = modelo;
                                    lblColor_PortadaExpediente.Text = color;
                                    lblAnio_PortadaExpediente.Text = anio;
                                    lblPlaca_PortadaExpediente.Text = matricula;
                                    lblVIN_PortadaExpediente.Text = VIN;
                                    lblVendedorGarantia_PortadaExpediente.Text = nombreVendedorGarantia + " - " + identidadVendedorGarantia;
                                    lblDuenoAnteriorGarantia_PortadaExpediente.Text = nombrePropietarioGarantia + " - " + identidadPropietarioGarantia;

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
                                    lblMontoOtorgado_Expediente.InnerText = DecimalToString(valorTotalFinanciamiento);
                                    lblValorCuota_Expediente.InnerText = DecimalToString(valorCuotaTotal);
                                    lblFechaPrimerPago_Expediente.InnerText = fechaPrimerPago.ToString("dd/MM/yyyy");
                                    lblFrecuenciaPlazo_Expediente.InnerText = tipoDePlazoSufijoAl;
                                    lblFechaVencimiento_Expediente.InnerText = "";
                                    lblOficialNegocios_Expediente.InnerText = oficialDeNegocios;
                                    lblGestor_Expediente.InnerText = gestorDeCobros;

                                    /* Memorandum */
                                    var usuarioLogueado = ObtenerInformacionUsuarioLogueado(pcIDApp, pcIDUsuario, pcIDSesion);

                                    lblNombreFirmaEntrega_Expediente.InnerText = usuarioLogueado.NombreCorto;
                                    lblPara_Memorandum.Text = "Marco Lara";
                                    lblDe_Memorandum.Text = usuarioLogueado.NombreCorto;
                                    lblFecha_Memorandum.Text = DateTime.Now.ToString("dd-MM-yyyy");
                                    lblAsunto_Memorandum.Text = "Entrega de documentos originales de vehículos";
                                    lblCliente_Memorandum.Text = nombreCliente;
                                    lblNumeroPlaca_Memorandum.Text = matricula;

                                    /* Acta de compromiso */
                                    lblNombreCliente_ActaDeCompromiso.Text = nombreCliente;
                                    lblIdentidadCliente_ActaDeCompromiso.Text = identidad;
                                    lblMarca_ActaDeCompromiso.Text = marca;
                                    lblModelo_ActaDeCompromiso.Text = modelo;
                                    lblAnio_ActaDeCompromiso.Text = anio;
                                    lblCilindraje_ActaDeCompromiso.Text = cilindraje;
                                    lblColor_ActaDeCompromiso.Text = color;
                                    lblMotor_ActaDeCompromiso.Text = serieMotor;
                                    //lblMontoFinalAFinanciar_Cash.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", Convert.ToDecimal(valorTotalFinanciamiento));
                                    

                                    //Prestadito Cash Contrato

                                    LblNombreCLiente_Cash.Text = nombreCliente;
                                    LblNombreCLienteFirma_Cash.Text = nombreCliente;
                                    LblDireccionCliente_Cash.Text = direccionCliente;
                                    LblAnio_Cash.Text = anio;
                                    lblMarca_Cash.Text = marca;
                                    lblModelo_Cash.Text = modelo;
                                    lblSerie_Cash.Text = serieChasis;
                                    lblNombrePropietarioGarantia_Cash.Text = RazonSocial;
                                    lblNombrePropietarioGarantiaFirma_Cash.Text = nombrePropietarioGarantia;
                                    //  lblNumeroPrestamo_Cash.Text = numeroPrestamo;


                                    blValorTotalCuota_Cash.Text = monedaSimbolo + " "  + CuotalTotal.ToString("n");
                                    //blValorTotalCuota_Cash.Text = MotontoTotalCash.ToString("n");
                                    lblValorCuota_Cash.Text = DecimalToString(valorCuotaTotal);
                                    lblValorCuota_Cash2.Text = DecimalToString(valorCuotaTotal);
                                    lblPlazoFinanciar_Cash.Text = plazoFinalAprobado;
                                    lblPlazoFinanciarTabla3_Cash.Text = monedaSimbolo + " " +  CuotalTotal.ToString("n");

                                    //lblMontoFinalAFinanciarTabla2_Cash.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", Convert.ToDecimal(valorTotalFinanciamiento));
                                    lblMontoFinalAFinanciarTabla_Cash.Text = monedaSimbolo + " " + string.Format("{0:#,###0.00}", Convert.ToDecimal(valorTotalFinanciamiento));
                                  //  lblPrimerPago_Cash.Text = fechaPrimerPago.ToString("dd/MMM/yyyy");



                                    /**** DIFERENCIAS ENTRE CONTRAROS SEGÚN EL TIPO DE PRODUCTO ***********/
                                    var parraforFinalNotaEntrega = "En virtud de lo anterior se le emite esta <b>NOTA DE ENTREGA</b> y al mismo tiempo ratificamos nuestro compromiso de hacer el pago correspondiente en efectivo, en cheque o en transferencia";

                                    if (idProducto == "202" || idProducto == "203" || idProducto == "204") // (Automovil Financiamiento, empeño, crediEmpeño)
                                    {
                                        lblValorPrimaContrato.Visible = false;

                                        var parrafoFinalProductoAutoNotaEntrega = " al señor: " + nombreVendedorGarantia + " en un máximo de diez (10) días hábiles mismos que serán para inscribir dicho vehículo en el Instituto de la Propiedad Mercantil a favor de " + fondosPrestamo.NombreComercial + ", salvo que la documentación entregada no se encuentre completa y/o no pueda ser inscrito en el IP.";
                                        parraforFinalNotaEntrega += parrafoFinalProductoAutoNotaEntrega;
                                        lblRevisionGpsAutos.Visible = true;
                                        lblNegarseRevisionGpsAuto.Visible = true;
                                    }
                                    else// if (idProducto == "201")
                                    {
                                        lblValorPrimaContrato.Visible = true;
                                        var parrafoFinalProductoMotosNotaEntrega = ". Se le pide a la concesionaria entregar una COPIA de factura al cliente y retener una llave original para ser entregada a Prestadito.";
                                        parraforFinalNotaEntrega += parrafoFinalProductoMotosNotaEntrega;
                                    }

                                    lblParrafoFinal_NotaEntrega.InnerHtml = parraforFinalNotaEntrega;
                                }

                                /* Fotografías de la garantía */
                                sqlResultado.NextResult();

                                if (!sqlResultado.HasRows)
                                {
                                    divContenedorInspeccionSeguro.Visible = false;
                                }

                                var imgNoHayFotografiasDisponibles = "<img alt='No hay fotografías disponibles' src='/Imagenes/Imagen_no_disponible.png' data-image='/Imagenes/Imagen_no_disponible.png' data-description='No hay fotografías disponibles'/>";

                                divGaleriaGarantia.InnerHtml = imgNoHayFotografiasDisponibles;
                                divGaleriaInspeccionSeguroDeVehiculo.InnerHtml = imgNoHayFotografiasDisponibles;
                                divGaleriaPortadaExpediente.InnerHtml = imgNoHayFotografiasDisponibles;
                                divPortadaExpediente_Revision.InnerHtml = imgNoHayFotografiasDisponibles;

                                var imagenesGarantia = new StringBuilder();
                                var imagenesGarantiaParaInspeccionDeSeguro = new StringBuilder();
                                var imagenesGarantiaParaPortadaExpediente = new StringBuilder();
                                var imagenesGarantiaParaPortadaExpedienteRevision = new StringBuilder();

                                while (sqlResultado.Read())
                                {
                                    imagenesGarantia.Append("<img alt='" + sqlResultado["fcSeccionGarantia"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcSeccionGarantia"] + "'/>");

                                    if ((bool)sqlResultado["fbApareceEnInspeccionDeSeguro"] == true)
                                        imagenesGarantiaParaInspeccionDeSeguro.Append("<img alt='" + sqlResultado["fcSeccionGarantia"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcSeccionGarantia"] + "'/>");

                                    if ((int)sqlResultado["fiIDSeccionGarantia"] == 3 || (int)sqlResultado["fiIDSeccionGarantia"] == 4)
                                        imagenesGarantiaParaPortadaExpediente.Append("<img alt='" + sqlResultado["fcSeccionGarantia"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcSeccionGarantia"] + "'/>");

                                    if ((int)sqlResultado["fiIDSeccionGarantia"] == 9)
                                        imagenesGarantiaParaPortadaExpedienteRevision.Append("<img alt='" + sqlResultado["fcSeccionGarantia"] + "' src='" + sqlResultado["fcURL"] + "' data-image='" + sqlResultado["fcURL"] + "' data-description='" + sqlResultado["fcSeccionGarantia"] + "'/>");
                                }

                                if (imagenesGarantia.ToString() != "")
                                    divGaleriaGarantia.InnerHtml = imagenesGarantia.ToString();

                                if (imagenesGarantiaParaInspeccionDeSeguro.ToString() != "")
                                    divGaleriaInspeccionSeguroDeVehiculo.InnerHtml = imagenesGarantiaParaInspeccionDeSeguro.ToString();

                                if (imagenesGarantiaParaPortadaExpediente.ToString() != "")
                                    divGaleriaPortadaExpediente.InnerHtml = imagenesGarantiaParaPortadaExpediente.ToString();
                                else
                                    divGaleriaPortadaExpediente.InnerHtml += imgNoHayFotografiasDisponibles;


                                if (imagenesGarantiaParaPortadaExpedienteRevision.ToString() != "")
                                    divPortadaExpediente_Revision.InnerHtml = imagenesGarantiaParaPortadaExpedienteRevision.ToString();

                                divInformacionGarantia.Visible = true;

                            } // if requiereGarantia == 1
                        } // while sqlResultado.Read()
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            var mensajeExtra = string.Empty;

            //if (int.Parse(pcIDSolicitud) < 802)
            //    mensajeExtra = ". Esta opción está disponible a partir de la solicitud de crédito No. 802.";
           // mensajeExtra = ex.InnerException.Message.ToString();
            MostrarMensaje("Error al cargar información de la solicitud " + pcIDSolicitud + ": " + ex.Message.ToString() + mensajeExtra);
            divInspeccionSeguroPDF.Visible = false;
            divPortadaExpedientePDF.Visible = false;
        }
    }

    private void CargarExpedienteDeLaSolicitud()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Expediente_ObtenerPorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /* Primer resultado: Información principal del expediente*/
                        while (sqlResultado.Read())
                        {
                            lblEspecifiqueOtros_Expediente.Text = sqlResultado["fcComentarios"].ToString();
                            txtEspecifiqueOtras.InnerText = sqlResultado["fcComentarios"].ToString();

                            if ((int)sqlResultado["fiIDEstadoExpediente"] != 1 && (int)sqlResultado["fiIDEstadoExpediente"] != 2 && pcIDUsuario == "211") /* ID usuario Mariely Guzman*/
                            {
                                divMemorandumPDF.Visible = true;
                                btnMemorandumExpediente.Visible = true;
                            }

                            var usuarioCreador = ObtenerInformacionUsuarioLogueado(pcIDApp, sqlResultado["fiIDUsuarioCreador"].ToString(), pcIDSesion);

                            lblNombreFirmaEntrega_Expediente.InnerText = usuarioCreador.NombreCorto;
                        }

                        /* Segundo resultado: Documentos del expediente */
                        sqlResultado.NextResult();

                        var listaDocumentosExpediente = new List<Expediente_Documento_ViewModel>();

                        TableRow tRowDocumentoExpediente = null;

                        while (sqlResultado.Read())
                        {
                            tRowDocumentoExpediente = new TableRow();
                            tRowDocumentoExpediente.Cells.Add(new TableCell() { Text = sqlResultado["fcDocumento"].ToString(), CssClass= "mt-0 mb-0 pt-0 pb-0" });
                            tRowDocumentoExpediente.Cells.Add(new TableCell() { Text = sqlResultado["fiIDEstadoDocumento"].ToString() == "1" ? "X" : "", CssClass = "mt-0 mb-0 pt-0 pb-0", HorizontalAlign = HorizontalAlign.Center });
                            tRowDocumentoExpediente.Cells.Add(new TableCell() { Text = sqlResultado["fiIDEstadoDocumento"].ToString() == "2" ? "X" : "", CssClass = "mt-0 mb-0 pt-0 pb-0", HorizontalAlign = HorizontalAlign.Center });
                            tRowDocumentoExpediente.Cells.Add(new TableCell() { Text = sqlResultado["fiIDEstadoDocumento"].ToString() == "3" ? "X" : "", CssClass = "mt-0 mb-0 pt-0 pb-0", HorizontalAlign = HorizontalAlign.Center });
                            tblDocumentos_Expediente.Rows.Add(tRowDocumentoExpediente);

                            listaDocumentosExpediente.Add(new Expediente_Documento_ViewModel()
                            {
                                IdDocumento = (int)sqlResultado["fiIDDocumento"],
                                DescripcionDocumento = sqlResultado["fcDocumento"].ToString(),
                                IdTipoDocumento = (int)sqlResultado["fiIDTipoDocumento"],
                                IdEstadoDocumento = (int)sqlResultado["fiIDEstadoDocumento"],
                                EstadoDocumento = sqlResultado["fcEstadoDocumento"].ToString()
                            });
                        }

                        ListaDocumentosDelExpedienteJSON = JsonConvert.SerializeObject(listaDocumentosExpediente);

                        /* Listado de tipos de solicitudes */
                        sqlResultado.NextResult();

                        TableRow tRowTiposDeSolicitud = null;

                        while (sqlResultado.Read())
                        {
                            tRowTiposDeSolicitud = new TableRow();
                            tRowTiposDeSolicitud.Cells.Add(new TableCell() { Text = sqlResultado["fcTipoSolicitud"].ToString() });
                            tRowTiposDeSolicitud.Cells.Add(new TableCell() { Text = "(" + sqlResultado["fcMarcado"].ToString() + ")" });
                            tblTipoDeSolicitud_Expediente.Rows.Add(tRowTiposDeSolicitud);
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar el expediente de la solicitud " + pcIDSolicitud + ": " + ex.Message.ToString());
        }
    }

    private void CargarPlanDePagos()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Prestamo_PlandePago_ConsultarPorSolicitud ", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;                 
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /* Primer resultado: Información principal*/
                        while (sqlResultado.Read())
                        {
                            lblPrestamo_PlanDePagos.Text = sqlResultado["fcIDPrestamo"].ToString();
                            lblNombreCliente_PlanDePagos.Text = sqlResultado["fcNombreCliente"].ToString();
                            lblFechaInicio_PlanDePagos.Text = Convert.ToDateTime(sqlResultado["fdFechaPrimerPago"].ToString()).ToString("dd/MMM/yyyy");
                            lblFechaFinal_PlanDePagos.Text = Convert.ToDateTime(sqlResultado["fdFechaVencimiento"].ToString()).ToString("dd/MMM/yyyy");
                            lblTasaInteres_PlanDePagos.Text = Convert.ToDecimal(sqlResultado["fiTasadeInteres"].ToString()).ToString("n") + " %";
                            lblCapitalFinanciado_PlanDePagos.Text = "$ " +Convert.ToDecimal(sqlResultado["fnCapitalFinanciado"].ToString()).ToString("n");

                            
                            lblInceptionDate_PlanDePagos.Text = Convert.ToDateTime(sqlResultado["fdFechaDesembolso"].ToString()).ToString("dd/MMM/yyyy");
                            lblProducto_PlanDePagos.Text = sqlResultado["fcProducto"].ToString();
                           // lblFrecuenciaPago_PlanDePagos.Text = sqlResultado["fcFrecuenciadePago"].ToString();


                            lblValorCuota_PlanDePagos.Text = "$ " + Convert.ToDecimal(sqlResultado["fnValorCuota"].ToString()).ToString("n");
                            lblTotalCuotas_PlanDePagos.Text = sqlResultado["fiPlazo"].ToString();
                            //cash
                            lblPrestamo_Cash.Text = sqlResultado["fcIDPrestamo"].ToString();
                            lblTasaInteres_CahsTable2.Text = Convert.ToDecimal(sqlResultado["fiTasadeInteres"].ToString()).ToString("n") + " %";
                            lblTasaInteres_CahsTable4.Text = Convert.ToDecimal(sqlResultado["fiTasadeInteres"].ToString()).ToString("n") + " %";
                            lblFechaVencimiento_Cash.Text = Convert.ToDateTime(sqlResultado["fdFechaVencimiento"].ToString()).ToString("dd/MMM/yyyy");
                            lblFechaDesembolso_Cash.Text = Convert.ToDateTime(sqlResultado["fdFechaDesembolso"].ToString()).ToString("dd/MMM/yyyy");
                            lblPrimerPago_Cash.Text = Convert.ToDateTime(sqlResultado["fdFechaPrimerPago"].ToString()).ToString("dd/MMM/yyyy");



                        }

                        /* Segundo resultado: listado plan de pagos */
                        sqlResultado.NextResult();

                 

                        TableRow tRowPlanDePagos = null;
                        var InteresTotal = 0m;
                        while (sqlResultado.Read())
                        {
                            tRowPlanDePagos = new TableRow();
                            tRowPlanDePagos.Cells.Add(new TableCell() { Text = sqlResultado["fiCuota"].ToString(), CssClass = "mt-0 mb-0 pt-0 pb-0" });
                            tRowPlanDePagos.Cells.Add(new TableCell() { Text = Convert.ToDateTime(sqlResultado["fdFechadeCuota"].ToString()).ToString("dd/MMM/yyyy"), CssClass = "mt-0 mb-0 pt-0 pb-0" });
                            tRowPlanDePagos.Cells.Add(new TableCell() { Text = "$" + Convert.ToDecimal(sqlResultado["fnCapitalAnterior"].ToString()).ToString("n") , CssClass = "mt-0 mb-0 pt-0 pb-0" ,HorizontalAlign = HorizontalAlign.Right } );
                            tRowPlanDePagos.Cells.Add(new TableCell() { Text = "$" +Convert.ToDecimal(sqlResultado["fnCapitalPactado"].ToString()).ToString("n"), CssClass = "mt-0 mb-0 pt-0 pb-0", HorizontalAlign = HorizontalAlign.Right });
                            tRowPlanDePagos.Cells.Add(new TableCell() { Text = "$" + Convert.ToDecimal(sqlResultado["fnInteresPactado"].ToString()).ToString("n"), CssClass = "mt-0 mb-0 pt-0 pb-0", HorizontalAlign = HorizontalAlign.Right });
                            tRowPlanDePagos.Cells.Add(new TableCell() { Text = "$" + Convert.ToDecimal(sqlResultado["fnSeguro1"].ToString()).ToString("n"), CssClass = "mt-0 mb-0 pt-0 pb-0", HorizontalAlign = HorizontalAlign.Right });
                            tRowPlanDePagos.Cells.Add(new TableCell() { Text = "$" + Convert.ToDecimal(sqlResultado["fnSeguro2"].ToString()).ToString("n"), CssClass = "mt-0 mb-0 pt-0 pb-0", HorizontalAlign = HorizontalAlign.Right });                           

                            tRowPlanDePagos.Cells.Add(new TableCell() { Text = "$" + Convert.ToDecimal(sqlResultado["fnTotalCuota"].ToString()).ToString("n"), CssClass = "mt-0 mb-0 pt-0 pb-0", HorizontalAlign = HorizontalAlign.Right });
                            tRowPlanDePagos.Cells.Add(new TableCell() { Text = "$" + Convert.ToDecimal(sqlResultado["fnCapitalBalanceFinal"].ToString()).ToString("n"), CssClass = "mt-0 mb-0 pt-0 pb-0", HorizontalAlign = HorizontalAlign.Right });

                            tbl_PlanDePagos.Rows.Add(tRowPlanDePagos);

                            InteresTotal +=  Convert.ToDecimal(sqlResultado["fnInteresPactado"]);
                            lblCollateral.Text = "$" + Convert.ToDecimal(sqlResultado["fnSeguro1"].ToString()).ToString("n");
                            lblCollatelaTabla3_Cash.Text = "$" + Convert.ToDecimal(sqlResultado["fnSeguro1"].ToString()).ToString("n");

                        }

                        lblInteresesTotal_Cash.Text = "$" + " " +  InteresTotal.ToString("n");
                        lblInteresesTotalTabla2_Cash.Text = "$" + " " + InteresTotal.ToString("n");
                        lblFrecuenciaPago_PlanDePagos.Text = Frecuencia;





                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar el plan de pagos de la solicitud " + pcIDSolicitud + ": " + ex.Message.ToString());
        }
    }


    #endregion

    #region Guardar expediente de la solicitud

    [WebMethod]
    public static Resultado_ViewModel GuardarExpediente(List<Expediente_Documento_ViewModel> documentosExpediente, string especifiqueOtros, string dataCrypt)
    {
        var resultado = new Resultado_ViewModel() { ResultadoExitoso = true };
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlTransaccion = sqlConexion.BeginTransaction("GuardarExpediente"))
                {
                    try
                    {
                        var idExpediente = string.Empty;

                        using (var sqlComando = new SqlCommand("sp_Expedientes_Maestro_Guardar", sqlConexion, sqlTransaccion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitud);
                            sqlComando.Parameters.AddWithValue("@pcComentarios", especifiqueOtros.Trim());
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.CommandTimeout = 120;

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    idExpediente = sqlResultado["MensajeError"].ToString();

                                    if (idExpediente.StartsWith("-1"))
                                    {
                                        sqlTransaccion.Rollback();
                                        resultado.ResultadoExitoso = false;
                                        resultado.MensajeResultado = "Error al guardar información principal del expediente, contacte al administrador";
                                        resultado.MensajeDebug = idExpediente;
                                        return resultado;
                                    }
                                }
                            } // using sqlResultado
                        } // using sqlComando

                        foreach (Expediente_Documento_ViewModel item in documentosExpediente)
                        {
                            using (var sqlComandoList = new SqlCommand("sp_Expedientes_Documentos_Guardar", sqlConexion, sqlTransaccion))
                            {
                                sqlComandoList.CommandType = CommandType.StoredProcedure;
                                sqlComandoList.Parameters.AddWithValue("@piIDExpediente", idExpediente);
                                sqlComandoList.Parameters.AddWithValue("@piIDDocumento", item.IdDocumento);
                                sqlComandoList.Parameters.AddWithValue("@piIDEstadoDocumento", item.IdEstadoDocumento);
                                sqlComandoList.Parameters.AddWithValue("@pcNombreArchivo", "");
                                sqlComandoList.Parameters.AddWithValue("@pcExtension", "");
                                sqlComandoList.Parameters.AddWithValue("@pcRutaArchivo", "");
                                sqlComandoList.Parameters.AddWithValue("@pcURL", "");
                                sqlComandoList.Parameters.AddWithValue("@pcComentarios", "");
                                sqlComandoList.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                sqlComandoList.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComandoList.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                sqlComandoList.CommandTimeout = 120;

                                using (var sqlResultado = sqlComandoList.ExecuteReader())
                                {
                                    while (sqlResultado.Read())
                                    {
                                        if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                        {
                                            sqlTransaccion.Rollback();
                                            resultado.ResultadoExitoso = false;
                                            resultado.MensajeResultado = "Error al guardar información del documento " + item.DescripcionDocumento + " del expediente, contacte al administrador.";
                                            resultado.MensajeDebug = sqlResultado["MensajeError"].ToString();
                                            return resultado;
                                        }
                                    }
                                } // using sqlResultado
                            } // using sqlComandoList
                        } // foreach documentos del expediente

                        if (resultado.ResultadoExitoso != false)
                            sqlTransaccion.Commit();
                    }
                    catch (Exception ex)
                    {
                        sqlTransaccion.Rollback();
                        resultado.ResultadoExitoso = false;
                        resultado.MensajeResultado = "Error al guardar el expediente de la solicitud, contacte al administrador";
                        resultado.MensajeDebug = ex.Message.ToString();
                    }
                } // using sqlTransaccion
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            resultado.ResultadoExitoso = false;
            resultado.MensajeResultado = "Error al guardar el expediente de la solicitud, contacte al administrador";
            resultado.MensajeDebug = ex.Message.ToString();
        }
        return resultado;
    }

    #endregion

    #region Proceso para asegurar vehiculo

    [WebMethod]
    public static List<TipoDocumento_ViewModel> ObtenerDocumentosParaAsegurar(bool reiniciarListaDocumentosAdjuntados, string dataCrypt)
    {
        try
        {
            var urlDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr") ?? "0";
            var pcIDSolicitud = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDSol");

            if (reiniciarListaDocumentosAdjuntados)
            {
                HttpContext.Current.Session["ListaDocumentosParaAsegurar"] = null;
                HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
            }

            return ObtenerDocumentosParaAsegurarPorIdSolicitud(pcIDApp, pcIDSesion, pcIDUsuario, pcIDSolicitud);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            return null;
        }
    }

    [WebMethod]
    public static Resultado_ViewModel EnviarInformacionParaAsegurar(string contenidoHtml, string VIN, string dataCrypt)
    {
        var resultadoProceso = new Resultado_ViewModel() { ResultadoExitoso = false };
        try
        {
            var urlDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr") ?? "0";
            var pcIDGarantia = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDGarantia");
            var pcIDSolicitud = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDSol");
            var usuarioLogueado = ObtenerInformacionUsuarioLogueado(pcIDApp, pcIDUsuario, pcIDSesion);

            var documentosParaAsegurarGuardarEnBbdd = new List<SolicitudesDocumentosViewModel>();

            /* Validar si se adjuntaron Documentos. Si se adjuntaron, guardarlos y moverlos al directorio de la solicitud crediticia */
            if (HttpContext.Current.Session["ListaDocumentosParaAsegurar"] != null)
            {
                var documentosParaAsegurarAdjuntados = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaDocumentosParaAsegurar"];

                if (documentosParaAsegurarAdjuntados != null)
                {
                    var nombreCarpetaDestino = "Solicitud" + pcIDSolicitud;
                    var nuevoNombreDocumento = string.Empty;

                    documentosParaAsegurarAdjuntados.ForEach(item =>
                    {
                        /* si el archivo existe, que se agregue a la lista */
                        if (File.Exists(item.fcRutaArchivo + "\\" + item.NombreAntiguo))
                        {
                            nuevoNombreDocumento = GenerarNombreDocumentoGarantia(pcIDSolicitud, VIN);

                            documentosParaAsegurarGuardarEnBbdd.Add(new SolicitudesDocumentosViewModel()
                            {
                                fcNombreArchivo = nuevoNombreDocumento,
                                NombreAntiguo = item.NombreAntiguo,
                                fcTipoArchivo = item.fcTipoArchivo,
                                fcRutaArchivo = item.fcRutaArchivo.Replace("Temp", "") + nombreCarpetaDestino,
                                URLArchivo = "/Documentos/Solicitudes/" + nombreCarpetaDestino + "/" + nuevoNombreDocumento + ".png",
                                fiTipoDocumento = item.fiTipoDocumento
                            });
                        }
                    }); // foreach documentos adjunstados
                } // if documentosParaAsegurarAdjuntados != null
            } // if HttpContext.Current.Session["ListaDocumentosParaAsegurar"] != null

            if (documentosParaAsegurarGuardarEnBbdd.Count > 0)
            {
                /* Si se adjuntaron documentos para asegurar, guardarlos en la base de datos */
                using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    sqlConexion.Open();

                    using (SqlTransaction sqlTransaction = sqlConexion.BeginTransaction())
                    {
                        var guardarDocumentoSP = string.Empty;
                        var llavePrimaria = 0;
                        var nombreParametroIdLlavePrimaria = string.Empty;
                        var nombreParametroExtensionArchivo = string.Empty;
                        var nombreParametroIdTipoDocumento = string.Empty;

                        foreach (var item in documentosParaAsegurarGuardarEnBbdd)
                        {
                            /* Validar si es un documento de la garantía o de la solicitud */
                            /* Si el documento a guardar tiene ID 9 quiere decir que es el documento "Boleta de revisión" de la garantía por lo que se guardará en la tabla de documentos de la garantía */
                            /* En caso contrario, quiere decir que es un documento de la solicitud y se guardará en la tabla de documentos de la solicitud */
                            if (item.fiTipoDocumento == 9)
                            {
                                guardarDocumentoSP = "sp_CREDGarantias_Documentos_Actualizar";
                                llavePrimaria = int.Parse(pcIDGarantia);
                                nombreParametroIdLlavePrimaria = "@piIDGarantia";
                                nombreParametroExtensionArchivo = "@pcExtension";
                                nombreParametroIdTipoDocumento = "@piIDSeccionGarantia";
                            }
                            else
                            {
                                guardarDocumentoSP = "sp_CREDSolicitudes_Documentos_Guardar";
                                llavePrimaria = int.Parse(pcIDSolicitud);
                                nombreParametroIdLlavePrimaria = "@piIDSolicitud";
                                nombreParametroExtensionArchivo = "@pcTipoArchivo";
                                nombreParametroIdTipoDocumento = "@piTipoDocumento";
                            }

                            using (var sqlComando = new SqlCommand(guardarDocumentoSP, sqlConexion, sqlTransaction))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                sqlComando.Parameters.AddWithValue(nombreParametroIdLlavePrimaria, llavePrimaria);
                                sqlComando.Parameters.AddWithValue("@pcNombreArchivo", item.fcNombreArchivo);
                                sqlComando.Parameters.AddWithValue(nombreParametroExtensionArchivo, ".png");
                                sqlComando.Parameters.AddWithValue("@pcRutaArchivo", item.fcRutaArchivo);
                                sqlComando.Parameters.AddWithValue("@pcURL", item.URLArchivo);
                                sqlComando.Parameters.AddWithValue(nombreParametroIdTipoDocumento, item.fiTipoDocumento);

                                if (item.fiTipoDocumento == 9)
                                    sqlComando.Parameters.AddWithValue("@pcComentario", "");

                                sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                sqlComando.CommandTimeout = 120;

                                using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                                {
                                    while (sqlResultado.Read())
                                    {
                                        if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                        {
                                            resultadoProceso.ResultadoExitoso = false;
                                            resultadoProceso.MensajeResultado = "Ocurrió un error al registrar el documento" + item.fcNombreArchivo + "., contacte al administrador.";
                                            resultadoProceso.MensajeDebug = sqlResultado["MensajeError"].ToString();
                                            sqlTransaction.Rollback();

                                            return resultadoProceso;
                                        }
                                    }
                                } // using sqlResultado
                            } // using sqlComando
                        } // ForEach documentos que se van a guardar en BBDD


                        /* Mover al directorio de la solicitud los documentos para asegurar adjuntados por el usuario que se guardaron en la base de datos */
                        if (!FileUploader.GuardarSolicitudDocumentos(int.Parse(pcIDSolicitud), documentosParaAsegurarGuardarEnBbdd))
                        {
                            resultadoProceso.ResultadoExitoso = false;
                            resultadoProceso.MensajeResultado = "Ocurrió un error al guadar los documentos para asegurar, contacte al administrador.";
                            resultadoProceso.MensajeDebug = "Error al mover los documentos al nuevo directorio";
                            sqlTransaction.Rollback();

                            return resultadoProceso;
                        }

                        sqlTransaction.Commit();
                        HttpContext.Current.Session["ListaDocumentosParaAsegurar"] = null;
                    } // using sqlTransaction
                } // using sqlConexion
            }

            /* Validar si hay documentos para asegurar pendientes. En caso de que hayan, retornar mensaje de error al usuario */
            var documentosParaAsegurar = ObtenerDocumentosParaAsegurarPorIdSolicitud(pcIDApp, pcIDSesion, pcIDUsuario, pcIDSolicitud);

            if (documentosParaAsegurar.Any(x => x.IdEstadoDocumento == 0))
            {
                var documentosPendientes = new StringBuilder();

                foreach (var item in documentosParaAsegurar.Where(x => x.IdEstadoDocumento == 0))
                {
                    documentosPendientes.Append(item.Descripcion + ", ");
                }
                resultadoProceso.ResultadoExitoso = false;
                resultadoProceso.MensajeResultado = "Los siguientes documentos están pendientes: " + documentosPendientes.ToString() + ". Adjuntalos todos para poder continuar.";
                resultadoProceso.MensajeDebug = "Hay documentos pendientes";
                return resultadoProceso;
            }

            /* Cambiar estado asegurado de la garantía a "Asegurado" */
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_CambiarEstadoAsegurado", sqlConexion))
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
                            if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                            {
                                resultadoProceso.ResultadoExitoso = false;
                                resultadoProceso.MensajeResultado = "Ocurrió un error al cambiar el estado de la garantía a asegurado. vuelva a intentarlo o contacte al administrador.";
                                resultadoProceso.MensajeDebug = sqlResultado["MensajeError"].ToString();
                                return resultadoProceso;
                            }
                        }
                    }
                }
            }

            var listaDirectoriosAttachments = documentosParaAsegurar.Where(x => x.IdEstadoDocumento != 0).Select(x => x.Ruta).ToList();

            /** Enviar por correo electrónico la información para asegurar un vehículo **/
            if (!EnviarCorreo("Seguro", "Seguro de garantía", "", contenidoHtml, usuarioLogueado.BuzonDeCorreo, listaDirectoriosAttachments))
            {
                resultadoProceso.ResultadoExitoso = false;
                resultadoProceso.MensajeResultado = "El proceso se realizó con exito pero ocurrió un error al enviar el correo electrónico, vuelva a intentarlo o contacte al administrador.";
                resultadoProceso.MensajeDebug = "Error al EnviarCorreo";
            }
            else
            {
                resultadoProceso.ResultadoExitoso = true;
                resultadoProceso.MensajeResultado = "¡La información para asegurar se envió exitosamente!";
                resultadoProceso.MensajeDebug = "Todo cheque";
            }
        }
        catch (Exception ex)
        {
            resultadoProceso.ResultadoExitoso = false;
            resultadoProceso.MensajeResultado = "Ocurrió un error al enviar la información, contacte al administrador.";
            resultadoProceso.MensajeDebug = ex.Message.ToString();
        }
        return resultadoProceso;
    }

    public static List<TipoDocumento_ViewModel> ObtenerDocumentosParaAsegurarPorIdSolicitud(string pcIDApp, string pcIDSesion, string pcIDUsuario, string pcIDSolicitud)
    {
        var documentosParaAsegurar = new List<TipoDocumento_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString.ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Documentos_ObtenerDocumentosParaAsegurar", sqlConexion))
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
                            documentosParaAsegurar.Add(new TipoDocumento_ViewModel()
                            {
                                IdDocumento = (short)sqlResultado["fiIDTipoDocumento"],
                                Descripcion = sqlResultado["fcDescripcionTipoDocumento"].ToString(),
                                IdEstadoDocumento = (int)sqlResultado["fiIDEstadoDocumento"],
                                Url = sqlResultado["fcURL"].ToString(),
                                Ruta = sqlResultado["fcRutaArchivo"].ToString()
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            documentosParaAsegurar = null;
        }
        return documentosParaAsegurar;
    }

    #endregion

    #region Funciones utilitarias

    [WebMethod]
    public static bool EnviarDocumentoPorCorreo(string asunto, string tituloGeneral, string contenidoHtml, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var usuarioLogueado = ObtenerInformacionUsuarioLogueado(pcIDApp, pcIDUsuario, pcIDSesion);

            resultado = EnviarCorreo(asunto, tituloGeneral, tituloGeneral, contenidoHtml, usuarioLogueado.BuzonDeCorreo);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultado;
    }

    public static bool EnviarCorreo(string pcAsunto, string pcTituloGeneral, string pcSubtitulo, string pcContenidodelMensaje, string buzonCorreoUsuario, List<string> directoriosAttachments = null)
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
            //pmmMensaje.To.Add("edwar.madrid@miprestadito.com");
            //pmmMensaje.CC.Add("amilcar.sauceda@miprestadito.com");
            //pmmMensaje.To.Add(buzonCorreoUsuario);
            pmmMensaje.To.Add("willian.diaz@miprestadito.com");
            pmmMensaje.CC.Add(buzonCorreoUsuario);

            pmmMensaje.IsBodyHtml = true;

            string htmlString = @"<!DOCTYPE html> " +
            "<html>" +
            "<body>" +
            " <div style=\"width: 500px;\">" +
            " <table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
            " <tr style=\"height: 30px; background-color:#56396b; font-family: 'Microsoft Tai Le'; font-size: 14px; font-weight: bold; color: white;\">" +
            " <td style=\"vertical-align: central; text-align:center;\">" + pcTituloGeneral + "</td>" +
            " </tr>" +
            " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
            " <td>&nbsp;</td>" +
            " </tr>" +
            " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
            " <td style=\"background-color:whitesmoke; text-align:center;\">Datos del cliente y la garantía</td>" +
            " </tr>" +
            " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
            " <td>&nbsp;</td>" +
            " </tr>" +
            " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
            " <td style=\"vertical-align: central;\">" + pcContenidodelMensaje + "</td>" +
            " </tr>" +
            " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
            " <td>&nbsp;</td>" +
            " </tr>" +
            " <tr style=\"height: 20px; font-family: 'Microsoft Tai Le'; font-size: 12px; text-align:center;\">" +
            " <td>System Bot Prestadito</td>" +
            " </tr>" +
            " </table>" +
            " </div>" +
            "</body> " +
            "</html> ";

            pmmMensaje.Body = htmlString;

            ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

            /* Validar si se agregó algún attachments */
            if (directoriosAttachments != null)
            {
                foreach (var item in directoriosAttachments)
                {
                    if (File.Exists(item))
                        pmmMensaje.Attachments.Add(new Attachment(item));
                }
            }

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

    public static string DecimalToString(decimal valor)
    {
        return string.Format("{0:#,###0.00##}", valor);
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = URL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? URL.Substring(liParamStart, URL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = URL.Substring(liParamStart + 1, URL.Length - (liParamStart + 1));
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

    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text += mensaje + " ";
    }

    public static InformacionUsuario_ViewModel ObtenerInformacionUsuarioLogueado(string pcIDApp, string pcIDUsuario, string pcIDSesion)
    {
        var usuarioLogueado = new InformacionUsuario_ViewModel();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreSeguridad.dbo.sp_InformacionUsuario", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            usuarioLogueado.NombreCorto = sqlResultado["fcNombreCorto"].ToString();
                            usuarioLogueado.CentroDeCosto = sqlResultado["fcCentroDeCosto"].ToString();
                            usuarioLogueado.NombreAgencia = sqlResultado["fcNombreAgencia"].ToString();
                            usuarioLogueado.BuzonDeCorreo = sqlResultado["fcBuzondeCorreo"].ToString();
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            usuarioLogueado = null;
        }
        return usuarioLogueado;
    }

    private static string GenerarNombreDocumentoGarantia(string idSolicitud, string vin)
    {
        return ("G_" + idSolicitud + "_" + vin + "_" + Guid.NewGuid()).Replace("*", "").Replace("/", "").Replace("\\", "").Replace(":", "").Replace("?", "").Replace("<", "").Replace(">", "").Replace("|", "");
    }

    #region Convertir cantidad a palabras

    public static string ConvertirCantidadALetras(string cantidad)
    {
        string resultado, centavos = string.Empty;
        Int64 entero;
        int decimales;
        double cantidadNumerica;

        try
        {
            cantidadNumerica = Convert.ToDouble(cantidad);
            entero = Convert.ToInt64(Math.Truncate(cantidadNumerica));
            decimales = Convert.ToInt32(Math.Round((cantidadNumerica - entero) * 100, 2));
            centavos = " CON " + PadNum(decimales) + "/100 CTVS";
            resultado = ToText(Convert.ToDouble(entero)) + centavos;
        }
        catch
        {
            return "";
        }

        return resultado;
    }

    private static string ToText(double value)
    {
        var Num2Text = string.Empty;
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
        else if (value < 20) Num2Text = "DIECI" + ToText(value - 10);
        else if (value == 20) Num2Text = "VEINTE";
        else if (value < 30) Num2Text = "VEINTI" + ToText(value - 20);
        else if (value == 30) Num2Text = "TREINTA";
        else if (value == 40) Num2Text = "CUARENTA";
        else if (value == 50) Num2Text = "CINCUENTA";
        else if (value == 60) Num2Text = "SESENTA";
        else if (value == 70) Num2Text = "SETENTA";
        else if (value == 80) Num2Text = "OCHENTA";
        else if (value == 90) Num2Text = "NOVENTA";
        else if (value < 100) Num2Text = ToText(Math.Truncate(value / 10) * 10) + " Y " + ToText(value % 10);
        else if (value == 100) Num2Text = "CIEN";
        else if (value < 200) Num2Text = "CIENTO " + ToText(value - 100);
        else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = ToText(Math.Truncate(value / 100)) + "CIENTOS";
        else if (value == 500) Num2Text = "QUINIENTOS";
        else if (value == 700) Num2Text = "SETECIENTOS";
        else if (value == 900) Num2Text = "NOVECIENTOS";
        else if (value < 1000) Num2Text = ToText(Math.Truncate(value / 100) * 100) + " " + ToText(value % 100);
        else if (value == 1000) Num2Text = "MIL";
        else if (value < 2000) Num2Text = "MIL " + ToText(value % 1000);
        else if (value < 1000000)
        {
            Num2Text = ToText(Math.Truncate(value / 1000)) + " MIL";
            if ((value % 1000) > 0) Num2Text = Num2Text + " " + ToText(value % 1000);
        }

        else if (value == 1000000) Num2Text = "UN MILLON";
        else if (value < 2000000) Num2Text = "UN MILLON " + ToText(value % 1000000);
        else if (value < 1000000000000)
        {
            Num2Text = ToText(Math.Truncate(value / 1000000)) + " MILLONES ";
            if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + ToText(value - Math.Truncate(value / 1000000) * 1000000);
        }

        else if (value == 1000000000000) Num2Text = "UN BILLON";
        else if (value < 2000000000000) Num2Text = "UN BILLON " + ToText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

        else
        {
            Num2Text = ToText(Math.Truncate(value / 1000000000000)) + " BILLONES";
            if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + ToText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
        }
        return Num2Text;

    }

    private static string PadNum(int entero)
    {
        return entero < 9 ? "0" + entero : entero.ToString();
    }
    #endregion

    #endregion

    #region View Models

    public class Resultado_ViewModel
    {
        public int IdInsertado { get; set; }
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string MensajeDebug { get; set; }
    }

    public class Expediente_Documento_ViewModel
    {
        public int IdDocumento { get; set; }
        public string DescripcionDocumento { get; set; }
        public int IdTipoDocumento { get; set; }
        public int IdEstadoDocumento { get; set; }
        public string EstadoDocumento { get; set; }
    }

    public class Fondo_RepresentanteLegal_ViewModel
    {
        public int IdFondo { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
        public string EmpresaRTN { get; set; }
        public string EmpresaCiudadDomiciliada { get; set; }
        public string EmpresaDepartamentoDomiciliada { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Constitucion { get; set; }
        public string UrlLogo { get; set; }
        public RepresentanteLegal_ViewModel RepresentanteLegal { get; set; }
    }

    public class RepresentanteLegal_ViewModel
    {
        public int IdRepresentanteLegal { get; set; }
        public string NombreCompleto { get; set; }
        public string Identidad { get; set; }
        public string EstadoCivil { get; set; }
        public string Nacionalidad { get; set; }
        public string Prefesion { get; set; }
        public string CiudadDomicilio { get; set; }
        public string DepartamentoDomicilio { get; set; }
    }

    public class InformacionUsuario_ViewModel
    {
        public int IdUsuario { get; set; }
        public string NombreCorto { get; set; }
        public string CentroDeCosto { get; set; }
        public string NombreAgencia { get; set; }
        public string BuzonDeCorreo { get; set; }
    }

    public class TipoDocumento_ViewModel
    {
        public int IdDocumento { get; set; }
        public string Descripcion { get; set; }
        public int IdEstadoDocumento { get; set; }
        public string Url { get; set; }
        public string Ruta { get; set; }
    }

    #endregion
}