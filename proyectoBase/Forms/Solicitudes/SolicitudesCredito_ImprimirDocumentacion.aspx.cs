using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Web;

public partial class SolicitudesCredito_ImprimirDocumentacion : System.Web.UI.Page
{
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";
    private string pcIDSolicitud = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

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
                            lblIdSolicitud.InnerText = pcIDSolicitud;
                            txtNombreCliente.Text = sqlResultado["fcNombreCliente"].ToString();
                            txtIdentidadCliente.Text = sqlResultado["fcIdentidadCliente"].ToString();
                            txtRtn.Text = sqlResultado["fcRTN"].ToString();
                            txtTelefonoCliente.Text = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                            txtProducto.Text = sqlResultado["fcProducto"].ToString();
                            txtMontoFinalAFinanciar.Text = sqlResultado["fnMontoFinalFinanciar"].ToString();
                            txtPlazoFinanciar.Text = sqlResultado["fiPlazoFinalAprobado"].ToString();
                            lblTipoDePlazo.InnerText = sqlResultado["fcTipoDePlazo"].ToString();
                            txtValorCuota.Text = sqlResultado["fiCuotaFinal"].ToString();
                            lblTipoDePlazoCuota.InnerText = sqlResultado["fcTipoDePlazo"].ToString();

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
}