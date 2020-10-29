using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

public partial class SolicitudesCredito_ImprimirDocumentacion : System.Web.UI.Page
{
    public string pcIDUsuario = "";
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDSolicitud = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    public string Departamento_Firma { get; set; }
    public string Ciudad_Firma { get; set; }
    public string Dias_Firma { get; set; }
    public string Mes_Firma { get; set; }
    public string Anio_Firma { get; set; }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
                var lcURL = Request.Url.ToString();
                int liParamStart = lcURL.IndexOf("?");

                string lcParametros;
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
                    CargarInformacion();

                    var hoy = DateTime.Today;

                    Departamento_Firma = "CORTES";
                    Ciudad_Firma = "SAN PEDRO SULA";
                    Dias_Firma = hoy.Day.ToString();
                    Mes_Firma = hoy.ToString("MMMM");
                    Anio_Firma = hoy.Year.ToString();
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
                            var nombreCliente = sqlResultado["fcNombreCliente"].ToString();
                            var identidad = sqlResultado["fcIdentidadCliente"].ToString();
                            var RTN = sqlResultado["fcRTN"].ToString();
                            var telefonoPrimario = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                            var nacionalidad = sqlResultado["fcDescripcionNacionalidad"].ToString();
                            var estadoCivil = sqlResultado["fcDescripcionEstadoCivil"].ToString();
                            var profesionOficio = sqlResultado["fcProfesionOficioCliente"].ToString();
                            var ciudadPoblado = sqlResultado["fcPoblado"].ToString();
                            var direccionCliente = sqlResultado["fcDireccionCliente"].ToString();
                            var producto = sqlResultado["fcProducto"].ToString();
                            var montoFinalFinanciar = sqlResultado["fnMontoFinalFinanciar"].ToString();
                            var plazoFinalAprobado = sqlResultado["fiPlazoFinalAprobado"].ToString();
                            var tipoDePlazo = sqlResultado["fcTipoDePlazo"].ToString();
                            var valorCuota = sqlResultado["fiCuotaFinal"].ToString();
                            var varloGarantia = sqlResultado["fnValorGarantia"].ToString();
                            var valorPrima = sqlResultado["fnValorPrima"].ToString();


                            lblIdSolicitud.InnerText = pcIDSolicitud;
                            txtNombreCliente.Text = nombreCliente;
                            txtIdentidadCliente.Text = identidad;
                            txtRtn.Text = RTN;
                            txtTelefonoCliente.Text = telefonoPrimario;
                            txtProducto.Text = producto;
                            txtMontoFinalAFinanciar.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(montoFinalFinanciar));
                            txtPlazoFinanciar.Text = plazoFinalAprobado;
                            lblTipoDePlazo.InnerText = tipoDePlazo;
                            txtValorCuota.Text = valorCuota;
                            lblTipoDePlazoCuota.InnerText = tipoDePlazo;

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
                                        var tipoDeGarantia = sqlResultado["fcVin"].ToString();
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

                                        /* Contrato */
                                        lblNombre_Contrato.Text = nombreCliente;
                                        lblNacionalidad_Contrato.Text = nacionalidad;
                                        lblIdentidad_Contrato.Text = identidad;
                                        lblDireccion_Contrato.Text = direccionCliente;
                                        lblMontoPrestamoEnPalabras_Contrato.Text = ConvertirCantidadALetras(montoFinalFinanciar);
                                        lblMontoPrestamo_Contrato.Text = "L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(montoFinalFinanciar));
                                        lblMontoParaCompraVehiculoEnPalabras_Contrato.Text = ConvertirCantidadALetras(montoFinalFinanciar);
                                        lblMontoParaCompraVehiculo_Contrato.Text = montoFinalFinanciar;
                                        lblMontoParaCompraSeguroYGPSEnPalabras_Contrato.Text = ConvertirCantidadALetras("0");
                                        lblMontoParaCompraSeguroYGPS_Contrato.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal("0"));
                                        lblMontoGastosDeCierre_Contrato.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal("0"));
                                        lblMarca_Contrato.Text = marca;
                                        lblTipoVehiculo_Contrato.Text = tipoDeVehiculo;
                                        lblModelo_Contrato.Text = modelo;
                                        lblAnio_Contrato.Text = anio;
                                        lblColor_Contrato.Text = color;
                                        lblCilindraje_Contrato.Text = cilindraje;
                                        lblMatricula_Contrato.Text = matricula;
                                        lblVIN_Contrato.Text = VIN;
                                        lblNumeroMotor_Contrato.Text = serieMotor;
                                        lblTasaInteresSimple_Contrato.Text = "1.67";
                                        lblTipoDePlazo_Contrato.Text = tipoDePlazo;
                                        lblCAT_Contrato.Text = "20.04";
                                        lblMontoPrima_Contrato.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(valorPrima));
                                        lblPlazo_Contrato.Text = plazoFinalAprobado;
                                        lblFrecuenciaPago_Contrato.Text = tipoDePlazo;
                                        lblValorCuotaPalabras_Contrato.Text = ConvertirCantidadALetras("0"); // pendiente
                                        lblValorCuota_Contrato.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal("0"));

                                        /* Pagare */
                                        lblMontoTitulo_Pagare.Text = "L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(montoFinalFinanciar));
                                        lblNombre_Pagare.Text = nombreCliente;
                                        lblProfesion_Pagare.Text = profesionOficio;
                                        lblIdentidad_Pagare.Text = identidad;
                                        lblDireccion_Pagare.Text = direccionCliente;
                                        lblMontoPalabras_Pagare.Text = ConvertirCantidadALetras(montoFinalFinanciar);
                                        lblMontoDigitos_Pagare.Text = "L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(montoFinalFinanciar));
                                        lblPorcentajeInteresFluctuante_Pagare.Text = "1.91";
                                        lblInteresesMoratorios_Pagare.Text = "4.52";
                                        lblNombreFirma_Pagare.Text = nombreCliente;
                                        lblIdentidadFirma_Pagare.Text = identidad;

                                        /* Compromiso legal */
                                        lblNombreCliente_CompromisoLegal.Text = nombreCliente;
                                        lblCantidadCuotas_CompromisoLegal.Text = plazoFinalAprobado;
                                        lblCuota_CompromisoLegal.Text = valorCuota;
                                        lblGarantiaUsada_CompromisoLegal.Text = Convert.ToDecimal(recorridoNumerico) < 1 ? "nuevo" : "usado";
                                        lblIdentidadFirma_CompromisoLegal.Text = identidad;
                                        lblNombreFirma_CompromisoLegal.Text = nombreCliente;

                                        /* Convenio de compra y venta de vehiculos para financiamiento a tercero */
                                        lblNombreCliente_ConvenioCyV.Text = nombreCliente;
                                        lblNacionalidad_ConvenioCyV.Text = nacionalidad;
                                        lblEstadoCivil_ConvenioCyV.Text = estadoCivil;
                                        lblIdentidad_ConvenioCyV.Text = identidad;
                                        lblCiudadCliente_ConvenioCyV.Text = ciudadPoblado;
                                        lblMarca_ConvenioCyV.Text = marca;
                                        lblModelo_ConvenioCyV.Text = modelo;
                                        lblAnio_ConvenioCyV.Text = anio;
                                        lblSerieMotor_ConvenioCyV.Text = serieMotor;
                                        lblSerieChasis_ConvenioCyV.Text = serieChasis;
                                        lblTipoVehiculoConvenioCyV.Text = tipoDeVehiculo;
                                        lblColor_ConvenioCyV.Text = color;
                                        lblMatricula_ConvenioCyV.Text = matricula;
                                        lblNombre_ConvenioCyV.Text = nombreCliente.ToUpper();

                                        /* Inspeccion seguro */
                                        lblNombre_InspeccionSeguro.Text = nombreCliente;
                                        lblMarca_InspeccionSeguro.Text = marca;
                                        lblModelo_InspeccionSeguro.Text = modelo;
                                        lblAnio_InspeccionSeguro.Text = anio;
                                        lblTipoDeVehiculo_InspeccionSeguro.Text = tipoDeVehiculo;
                                        lblRecorrido_InspeccionSeguro.Text = recorrido;
                                        lblMatricula_InspeccionSeguro.Text = matricula;

                                        /* Traspaso */
                                        lblNombreCliente_Traspaso.Text = nombreCliente;
                                        lblIdentidad_Traspaso.Text = identidad;
                                        lblNacionalidad_Traspaso.Text = nacionalidad;
                                        lblDireccion_Traspaso.Text = direccionCliente;
                                        lblMarca_Traspaso.Text = marca;
                                        lblModelo_Traspaso.Text = modelo;
                                        lblSerieMotor_Traspaso.Text = serieMotor;
                                        lblAnio_Traspaso.Text = anio;
                                        lblCilindraje_Traspaso.Text = cilindraje;
                                        lblTipoDeVehiculo_Traspaso.Text = tipoDeVehiculo;
                                        lblColor_Traspaso.Text = color;
                                        lblSerieChasis_Traspaso.Text = serieChasis;
                                        lblMatricula_Traspaso.Text = matricula;
                                        lblGarantiaUsada_Traspaso.Text = Convert.ToDecimal(recorridoNumerico) < 1 ? "nuevo" : "usado";
                                    }

                                    sqlResultado.NextResult();

                                    /* Fotografías de la garantía */
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
                                        divGaleriaInspeccionSeguroDeVehiculo.InnerHtml = imagenesGarantia.ToString();
                                    }
                                }
                                divInformacionGarantia.Visible = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar información de la solicitud " + pcIDSolicitud + ": " + ex.Message.ToString());
        }
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
}