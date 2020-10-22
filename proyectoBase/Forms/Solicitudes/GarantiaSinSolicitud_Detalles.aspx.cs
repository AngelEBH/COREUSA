using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

public partial class GarantiaSinSolicitud_Detalles : System.Web.UI.Page
{
    public string pcIDUsuario = "";
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDGarantia = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

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
                    pcIDGarantia = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDGarantia") ?? "0";
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

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_ObtenerPorIdGarantia", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDGarantia", pcIDGarantia);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            string lcScript = "window.open('SolicitudesCredito_ListadoGarantias.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&SID=" + pcIDSesion + "&IDApp=" + pcIDApp) + "','_self')";
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
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar información: " + ex.Message.ToString());
        }
    }

    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text = mensaje;
    }
}