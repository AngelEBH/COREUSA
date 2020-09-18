using System;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public partial class Clientes_Precalificado_Puesto109 : System.Web.UI.Page
{
    private string pcIDUsuario = "";
    private string pcID = "";
    private string pcIDApp = "";
    private string pcIDIngresos = "";
    private string pcIDSesion = "1";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private String pcPasoenCurso = "";
    private String pcBuscarDatos = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (pcBuscarDatos == "0" && !IsPostBack)
        {
            pcBuscarDatos = "1";

            // variables
            string lcTipodeProducto = "";
            string lcObligaciones = "";
            string lcTipoCliente = "1";
            string lcCapacidadDisponible = "0.00";
            string lcEsPrimerConsultor = "0";

            /* INICIO de captura de parametros y desencriptado de cadena */
            string lcURL = "";
            int liParamStart = 0;
            string lcParametros = "";
            string lcEncriptado = "";
            string lcParametroDesencriptado = "";
            Uri lURLDesencriptado = null;

            lcURL = Request.Url.ToString();
            liParamStart = lcURL.IndexOf("?");

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
                lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                lcEncriptado = lcEncriptado.Replace("%", "");
                lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            }
            /* FIN de captura de parametros y desencriptado de cadena */

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                try
                {
                    sqlConexion.Open();
                    pcPasoenCurso = "Paso 1";

                    using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                        sqlComando.CommandTimeout = 120;

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            sqlResultado.Read();
                            if (!sqlResultado.HasRows)
                            {
                                tabOferta.Visible = false;
                                ddlOferta.Visible = false;
                                ClienteCC.Visible = false;
                                ddlClienteCC.Visible = false;
                                lblRespuesta.Text = "--Sin Informacion--";
                                lblDetalleRespuesta.Text = "No se encuentra informción alguna de la identidad proporcionada.";
                            }
                            pcPasoenCurso = "Paso 2";

                            txtIdentidad.Text = sqlResultado["fcIdentidad"].ToString();
                            txtNombreCompleto.Text = sqlResultado["fcNombre"].ToString();
                            txtTelefonoRegistrado.Text = sqlResultado["fcTelefono"].ToString();
                            txtIngresosRegistrados.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnIngresos"].ToString()));
                            lblDetalleRespuesta.Text = sqlResultado["fcMensaje"].ToString();
                            lcObligaciones = sqlResultado["fnTotalObligaciones"].ToString();
                            lcCapacidadDisponible = sqlResultado["fnCapacidadDisponible"].ToString();
                            lcEsPrimerConsultor = sqlResultado["fiEsPrimerConsultor"].ToString();
                            pcIDIngresos = sqlResultado["fnIngresos"].ToString();
                            pcIDIngresos = pcIDIngresos.Replace(",", "");

                            if (sqlResultado["fcObservacionesCreditos"].ToString().Trim().Length > 0)
                            {
                                divResolucionCreditos.Visible = true;
                                lblResolucionCreditos.Text = sqlResultado["fcObservacionesCreditos"].ToString();
                                lblDetalleRespuesta.Text = "Aprobado por parte del area de crédito.";
                                tabOferta.Visible = false;
                                ddlOferta.Visible = false;
                            }

                            if (lcEsPrimerConsultor == "0")
                                txtInfoPrimerConsulta.InnerText = "Este cliente ya fue consultado por " + sqlResultado["fcOficialAgencia"].ToString().Trim() + " en agencia " + sqlResultado["fcCentrodeCosto"].ToString().Trim() + " - " + sqlResultado["fcNombreAgencia"].ToString().Trim();
                            else
                                txtInfoPrimerConsulta.InnerText = "Eres el primero en consultar este cliente. Consultado el " + sqlResultado["fdFechaPrimerConsulta"].ToString().Trim();

                            if (lcObligaciones == "0.00")
                                lcTipoCliente = "0";

                            if (sqlResultado["fiEstadoActualPrecalificado"].ToString() == "2")
                            {
                                lblRespuesta.Text = "-- No Aplica --";
                                lblRespuesta.ForeColor = System.Drawing.Color.Red;
                                lblDetalleRespuesta.ForeColor = System.Drawing.Color.Red;
                                tabOferta.Visible = false;
                                ddlOferta.Visible = false;
                                if (sqlResultado["fiIDUsuarioCreditos"].ToString() != "0")
                                {
                                    lblDetalleRespuesta.Text = "Rechazado de parte del area de crédito.";
                                }
                            }

                            if (sqlResultado["fiEstadoActualPrecalificado"].ToString() == "3")
                            {
                                lblRespuesta.Text = "-- Sujeto a Analisis --";
                                lblRespuesta.ForeColor = System.Drawing.Color.Goldenrod;
                                lblDetalleRespuesta.ForeColor = System.Drawing.Color.Goldenrod;
                                tabOferta.Visible = false;
                                ddlOferta.Visible = false;
                            }

                            if (sqlResultado["fiEstadoActualPrecalificado"].ToString() == "1" && lcEsPrimerConsultor == "1")
                            {
                                divIngresarSolicitud.Visible = true;
                            }

                            pcPasoenCurso = "Paso 3";

                            if (sqlResultado["fiSAF"].ToString() == "1")
                            {
                                imgSAF.Visible = true;
                            }

                            if (sqlResultado["fiIHSS"].ToString() == "1")
                            {
                                imgIHSS.Visible = true;
                            }

                            if (sqlResultado["fiCNE"].ToString() == "1")
                            {
                                imgRNP.Visible = true;
                            }

                            if (sqlResultado["fiCallCenter"].ToString() == "1")
                            {
                                imgCallCenter.Visible = true;
                                ClienteCC.Visible = true;
                                ddlClienteCC.Visible = true;
                                tabOferta.Visible = false;
                                ddlOferta.Visible = false;
                                lblMensajeCallCenter.Text = "Cliente está asignado a la base de CallCenter";
                                lblMensajeCallCenterAgenteAsignado.Text = "Agente asginado: " + sqlResultado["fcAgenteCallCenter"].ToString().Trim() + ".";
                            }

                            if (sqlResultado["fiScoreBajo"].ToString() == "1")
                            {
                                imgScoreMenor.Visible = true;
                            }

                            if (sqlResultado["fiIncobrable"].ToString() == "1")
                            {
                                imgIncobrable.Visible = true;
                            }
                            if (sqlResultado["fiIrrecuperable"].ToString() == "1")
                            {
                                imgCallCenter.Visible = true;
                            }
                            if (sqlResultado["fiJuridicoLegal"].ToString() == "1")
                            {
                                imgJuridico.Visible = true;
                            }
                            if (sqlResultado["fiSaldosCastigados"].ToString() == "1")
                            {
                                imgCastigado.Visible = true;
                            }

                            if (sqlResultado["fiMoraMayor"].ToString() == "1")
                            {
                                imgMoraMayor.Visible = true;
                            }

                            if (sqlResultado["fiSobregiro"].ToString() == "1")
                            {
                                imgSobregiro.Visible = true;
                            }

                            lcTipodeProducto = sqlResultado["fiIDProducto"].ToString();

                        } // using reader
                    } // using command

                    CargarAntiguedadActiva(sqlConexion);

                    using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CotizadorProductos", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@piIDProducto", lcTipodeProducto);
                        sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID.Trim());
                        sqlComando.Parameters.AddWithValue("@piConObligaciones", lcTipoCliente.Trim());
                        sqlComando.Parameters.AddWithValue("@pnIngresosBrutos", pcIDIngresos.Trim());
                        sqlComando.Parameters.AddWithValue("@pnIngresosDisponible", lcCapacidadDisponible.Trim());

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            gvOferta.DataSource = sqlResultado;
                            gvOferta.DataBind();
                        }
                    }

                    /*********************************************************************/
                    /* Inicia bloque de compatibilidad de datos de analistas de credito */
                    using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaAnalistas", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            sqlResultado.Read();

                            txtBanca.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaBanca"].ToString()));
                            txtComercio.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaComercio"].ToString()));
                            txtTarjetas.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaTarjeta"].ToString()));
                            txtTotal.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnTotalObligaciones"].ToString()));
                            lblScorePromedio.Text = sqlResultado["fiScorePromedio"].ToString();

                            CargarEndeudamiento(sqlResultado["fiEndeudamiento"].ToString());
                        }

                    }

                    CargarIHSS(sqlConexion);
                    /*********************************************************************/
                }
                catch (Exception ex)
                {
                    lblMensaje.Text = pcPasoenCurso + " - " + ex.Message;
                    lblMensaje.ForeColor = System.Drawing.Color.Red;
                }
            }

            tabHistorialdeConsultas.Visible = false;
            ddlHistorialdeConsultas.Visible = false;
        }
    }

    protected void btnConsultarCliente_Click(object sender, EventArgs e)
    {
        string lcScript = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_Clientes_Contenido')";
        Response.Write("<script>");
        Response.Write(lcScript);
        Response.Write("</script>");
    }

    private void CargarAntiguedadActiva(SqlConnection pSqlConnection)
    {
        using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_UltimoCreditoActivo", pSqlConnection))
        {
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
            sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);

            using (var sqlResultado = sqlComando.ExecuteReader())
            {
                if (sqlResultado.Read())
                {
                    txtAntiguedadActiva.Text = GetMonthDifference(Convert.ToDateTime(sqlResultado["gaveDate"].ToString()), DateTime.Now).ToString() + " Meses";
                }
            }
            if (string.IsNullOrEmpty(txtAntiguedadActiva.Text.Trim()))
                txtAntiguedadActiva.Text = "Sin buro activo.";
        }
    }

    public static int GetMonthDifference(DateTime startDate, DateTime endDate)
    {
        int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
        return Math.Abs(monthsApart);
    }

    private void CargarEndeudamiento(string pcEndeudamiento)
    {
        PorcentajeEndeudamiento.Attributes.Add("aria-valuenow", pcEndeudamiento);
        string classColor = "bg-success";

        if (((Convert.ToDouble(pcEndeudamiento) * 100) > 30) && ((Convert.ToDouble(pcEndeudamiento) * 100) < 70))
            classColor = "bg-warning";

        if ((Convert.ToDouble(pcEndeudamiento) * 100) >= 70)
            classColor = "bg-danger";

        PorcentajeEndeudamiento.Attributes.Add("text", pcEndeudamiento + "%");
        PorcentajeEndeudamiento.Attributes.Add("class", "progress-bar progress-bar-striped progress-bar-animated " + classColor);
    }

    private void CargarIHSS(SqlConnection pSqlConnection)
    {
        using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_Info_TrabajosIHSS", pSqlConnection))
        {
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@piIDUSuario", pcIDUsuario);
            sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);

            using (var sqlResultado = sqlComando.ExecuteReader())
            {
                gvIHSS.DataSource = sqlResultado;
                gvIHSS.DataBind();
            }
        }
    }

    protected void btnIngresarSolicitud_Click(object sender, EventArgs e)
    {
        string lcParametroEncriptar = "SID=" + pcIDSesion + "&IDApp=" + pcIDApp + "&usr=" + pcIDUsuario + "&ID=" + txtIdentidad.Text.Trim();
        string lcScript = "window.open('../Solicitudes/Forms/Solicitudes/SolicitudesCredito_Registrar.aspx?" + DSC.Encriptar(lcParametroEncriptar) + "','_Clientes_Contenido')";
        Response.Write("<script>");
        Response.Write(lcScript);
        Response.Write("</script>");
    }
}