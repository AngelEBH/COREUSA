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
    public InformacionPrincipal_Cliente_Solicitud_ViewModel InformacionPrincipal { get; set; }
    public string JsonInformacionPrincipal { get; set; }

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
                    lcParametros = String.Empty;
                }

                if (lcParametros != String.Empty)
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
                            var producto = sqlResultado["fcProducto"].ToString();
                            var montoFinalFinanciar = sqlResultado["fnMontoFinalFinanciar"].ToString();
                            var plazoFinalAprobado = sqlResultado["fiPlazoFinalAprobado"].ToString();
                            var tipoDePlazo = sqlResultado["fcTipoDePlazo"].ToString();
                            var valorCuota = sqlResultado["fiCuotaFinal"].ToString();

                            lblIdSolicitud.InnerText = pcIDSolicitud;
                            txtNombreCliente.Text = nombreCliente;
                            txtIdentidadCliente.Text = identidad;
                            txtRtn.Text = RTN;
                            txtTelefonoCliente.Text = telefonoPrimario;
                            txtProducto.Text = producto;
                            txtMontoFinalAFinanciar.Text = montoFinalFinanciar;
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

                                        /* Pagaré */
                                        lblNombreFirma_Pagare.Text = nombreCliente;
                                        lblIdentidad_Traspaso.Text = identidad;
                                        lblNacionalidad_Traspaso.Text = nacionalidad;
                                        lblDireccion_Traspaso.Text = "no definido";
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