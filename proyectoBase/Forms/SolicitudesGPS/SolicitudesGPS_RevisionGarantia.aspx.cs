using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web;

public partial class SolicitudesGPS_RevisionGarantia : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDSolicitud = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");

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

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

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
                            txtNombreCliente.Text = sqlResultado["fcNombreCliente"].ToString();
                            txtIdentidadCliente.Text = sqlResultado["fcIdentidadCliente"].ToString();
                            txtRtn.Text = sqlResultado["fcRTN"].ToString();
                            txtTelefonoCliente.Text = sqlResultado["fcTelefonoPrimarioCliente"].ToString();

                            int requiereGarantia = (byte)sqlResultado["fiRequiereGarantia"];

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
                                    var recorridoNumerico = Convert.ToDecimal(sqlResultado["fnRecorrido"].ToString());
                                    var recorrido = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnRecorrido"].ToString())) + " " + sqlResultado["fcUnidadDeDistancia"].ToString();

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
                                    txtRecorrido.Text = recorrido;
                                    txtTransmision.Text = sqlResultado["fcTransmision"].ToString();
                                    txtTipoDeCombustible.Text = sqlResultado["fcTipoCombustible"].ToString();
                                    txtSerieUno.Text = sqlResultado["fcSerieUno"].ToString();
                                    txtSerieDos.Text = sqlResultado["fcSerieDos"].ToString();
                                    txtComentario.InnerText = sqlResultado["fcComentario"].ToString().Trim();
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
                            } // else !sqlResultado.HasRows
                        } // while sqlResultado.Read()
                    } // using sqlComando.ExecuteReader()
                } // using sqlComando
            } // using sqlConexion
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