using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Net;
using System.IO;

public partial class Clientes_ClienteparaSolicitud : System.Web.UI.Page
{

    private string pcIDUsuario = "";
    private string pcID = "";
    private string pcIDApp = "";
    private string pcIDIngresos = "";
    private string pcIDSesion = "1";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private String lcSQLInstruccion = "";
    private String sqlConnectionString = "";
    private String pcPasoenCurso = "";
    private String pcBuscarDatos = "0";

    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcURL = "";
        int liParamStart = 0;
        string lcParametros = "";
        string lcEncriptado = "";
        string lcParametroDesencriptado = "";
        Uri lURLDesencriptado = null;
        HyperLink hlLink = new HyperLink();
        LinkButton btnLink = new LinkButton();


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
            lcEncriptado = lcEncriptado.Replace("%2f", "/");
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
        }
        /* FIN de captura de parametros y desencriptado de cadena */

        if (String.IsNullOrEmpty(pcIDSesion))
        {
            pcIDSesion = "1";
        }
    }

    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        pcID = txtIdentidad.Text.Trim();

        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

        try
        {
            string lcParametrosSP = "";
            sqlConexion.Open();
            pcPasoenCurso = "Paso 1";
            /* sp_InfoEquifaxPuesto110 1,'1313198600042',30000 */
            lcParametrosSP = pcIDApp + "," + pcIDUsuario + ",'" + pcID + "'";
            lcSQLInstruccion = "exec CoreAnalitico.dbo.sp_info_ConsultaEjecutivos " + lcParametrosSP;
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlComando.CommandTimeout = 120;
            sqlResultado = sqlComando.ExecuteReader();
            sqlResultado.Read();

            if (!sqlResultado.HasRows)
            {
                string lcScript = "window.open('ClienteRegistrarNuevo.aspx?" + DSC.Encriptar("SID=" + pcIDSesion + "&IDApp=" + pcIDApp + "&usr=" + pcIDUsuario + "&ID=" + txtIdentidad.Text.Trim()) + "','_Clientes_Contenido')";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
            }

            pcPasoenCurso = "Paso 2";
            string lcObligaciones = "";
            string lcTipoCliente = "1";
            string lcCapacidadDisponible = "0.00";
            string lcEsPrimerConsultor = "0";

            txtIdentidad.Text = sqlResultado["fcIdentidad"].ToString();
            txtNombreCompleto.Text = sqlResultado["fcNombre"].ToString();
            //txtTelefonoRegistrado.Text = sqlResultado["fcTelefono"].ToString();
            txtIngresosRegistrados.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnIngresos"].ToString()));
            lblDetalleRespuesta.Text = sqlResultado["fcMensaje"].ToString();
            lcObligaciones = sqlResultado["fnTotalObligaciones"].ToString();
            lcCapacidadDisponible = sqlResultado["fnCapacidadDisponible"].ToString();
            lcEsPrimerConsultor = sqlResultado["fiEsPrimerConsultor"].ToString();
            pcIDIngresos = sqlResultado["fnIngresos"].ToString();
            pcIDIngresos = pcIDIngresos.Replace(",", "");

            if (sqlResultado["fcObservacionesCreditos"].ToString().Trim().Length > 0)
            {
                PanelResolucionCreditos.Visible = true;
                lblResolucionCreditos.Text = sqlResultado["fcObservacionesCreditos"].ToString();
                lblDetalleRespuesta.Text = "Aprobado por parte del area de crédito.";
                PanelOferta.Visible = false;
            }

            if (lcEsPrimerConsultor == "0")
                txtInfoPrimerConsulta.Text = "Este cliente ya fue consultado por " + sqlResultado["fcOficialAgencia"].ToString().Trim() + " en agencia " + sqlResultado["fcCentrodeCosto"].ToString().Trim() + " - " + sqlResultado["fcNombreAgencia"].ToString().Trim() + " (" + sqlResultado["fdFechaPrimerConsulta"].ToString().Trim() + ")";
            else
                txtInfoPrimerConsulta.Text = "Eres el primero en consultar este cliente. Consultado el " + sqlResultado["fdFechaPrimerConsulta"].ToString().Trim();

            if (lcObligaciones == "0.00")
                lcTipoCliente = "0";

            if (sqlResultado["fiEstadoActualPrecalificado"].ToString() == "2")
            {
                imgTriste.Visible = true;
                lblRespuesta.Text = "-- No Aplica --";
                lblRespuesta.ForeColor = System.Drawing.Color.Red;
                lblDetalleRespuesta.ForeColor = System.Drawing.Color.Red;
                PanelOferta.Visible = false;
                if (sqlResultado["fiIDUsuarioCreditos"].ToString() != "0")
                {
                    lblDetalleRespuesta.Text = "Rechazado de parte del area de crédito.";
                }
            }

            if (sqlResultado["fiEstadoActualPrecalificado"].ToString() == "3")
            {
                imgAnalisis.Visible = true;
                lblRespuesta.Text = "-- Sujeto a Analisis --";
                lblRespuesta.ForeColor = System.Drawing.Color.Goldenrod;
                lblDetalleRespuesta.ForeColor = System.Drawing.Color.Goldenrod;
                PanelOferta.Visible = false;
            }

            if (sqlResultado["fiIngresarSolicitud"].ToString() == "1")
            {
                btnIngresarSolicitud.Visible = true;
            }

            pcPasoenCurso = "Paso 3";

            if (sqlResultado["fiSAF"].ToString() == "1")
            {
                lblSAF5.ForeColor = System.Drawing.Color.Black;
                imgSAF.Visible = true;
            }

            if (sqlResultado["fiIHSS"].ToString() == "1")
            {
                lblIHSS.ForeColor = System.Drawing.Color.Black;
                imgIHSS.Visible = true;
            }

            if (sqlResultado["fiCNE"].ToString() == "1")
            {
                lblRNP.ForeColor = System.Drawing.Color.Black;
                imgRNP.Visible = true;
            }

            if (sqlResultado["fiCallCenter"].ToString() == "1")
            {
                lblCallCenter.ForeColor = System.Drawing.Color.Black;
                imgCallCenter.Visible = true;
                PanelCallCenter.Visible = true;
                PanelOferta.Visible = false;
                PanelAnalisis.Visible = false;
                lblMensajeCallCenter.Text = "Cliente esta asignado a la base de CallCenter";
                lblMensajeCallCenterAgenteAsignado.Text = "Agente asginado: " + sqlResultado["fcAgenteCallCenter"].ToString().Trim() + ".";
            }

            if (sqlResultado["fiScoreBajo"].ToString() == "1")
            {
                lblScoreBajo.ForeColor = System.Drawing.Color.Black;
                imgScoreMenor.Visible = true;
            }

            if (sqlResultado["fiIncobrable"].ToString() == "1")
            {
                lblIncobIrrecup.ForeColor = System.Drawing.Color.Black;
                imgIncobrable.Visible = true;
            }
            if (sqlResultado["fiIrrecuperable"].ToString() == "1")
            {
                lblIncobIrrecup.ForeColor = System.Drawing.Color.Black;
                imgCallCenter.Visible = true;
            }
            if (sqlResultado["fiJuridicoLegal"].ToString() == "1")
            {
                lblJuridicoLegal.ForeColor = System.Drawing.Color.Black;
                imgJuridico.Visible = true;
            }
            if (sqlResultado["fiSaldosCastigados"].ToString() == "1")
            {
                lblSaldosCastig.ForeColor = System.Drawing.Color.Black;
                imgCastigado.Visible = true;
            }

            if (sqlResultado["fiMoraMayor"].ToString() == "1")
            {
                lblMoraMayor.ForeColor = System.Drawing.Color.Black;
                imgMoraMayor.Visible = true;
            }

            if (sqlResultado["fiSobregiro"].ToString() == "1")
            {
                lblSobregiro.ForeColor = System.Drawing.Color.Black;
                imgSobregiro.Visible = true;
            }

            //CargarAntiguedadActiva(sqlConexion);
            // string lcTipodeProducto = "";
            // lcTipodeProducto = sqlResultado["fiIDProducto"].ToString();

            // lcParametrosSP = pcIDUsuario.Trim() + "," + lcTipodeProducto + ",'" + pcID.Trim() + "'," + lcTipoCliente.Trim() + "," + pcIDIngresos.Trim() + "," + lcCapacidadDisponible.Trim();
            // lcSQLInstruccion = "exec CoreFinanciero.dbo.sp_CotizadorProductos " + lcParametrosSP;
            // sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            // sqlResultado = sqlComando.ExecuteReader();

            // gvOferta.DataSource = sqlResultado;
            // gvOferta.DataBind();

            sqlComando.Dispose();
            sqlConexion.Close();
            //lblMensaje.Text = lcSQLInstruccion;

        }
        catch (Exception ex)
        {
            lblMensaje.Text = pcPasoenCurso + " - " + ex.Message;
            lblMensaje.ForeColor = System.Drawing.Color.Red;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }

        //PanelOferta.Visible = false;
        PanelHistorialdeConsultas.Visible = false;
        PanelOferta.Visible = false;
        PanelAnalisis.Visible = true;
        PanelInidicadores.Visible = false;
        //btnIngresarSolicitud.Visible = true;
    }

    // ANGEL
    //private void CargarAntiguedadActiva(SqlConnection pSqlConnection)
    //{

    //    string lcSQLInstruccion = "exec CoreAnalitico.dbo.sp_info_UltimoCreditoActivo " + pcIDUsuario + ",'" + pcID + "'";
    //    SqlCommand sqlComando = new SqlCommand(lcSQLInstruccion, pSqlConnection);
    //    sqlComando.CommandType = CommandType.Text;
    //    SqlDataReader sqlResultado = sqlComando.ExecuteReader();

    //    if (sqlResultado.Read())
    //    {
    //        txtAntiguedadActiva.Text = GetMonthDifference(Convert.ToDateTime(sqlResultado["gaveDate"].ToString()), DateTime.Now).ToString() + " Meses";
    //    }

    //    if (string.IsNullOrEmpty(txtAntiguedadActiva.Text.Trim()))
    //        txtAntiguedadActiva.Text = "Sin buro activo.";
    //}

    public static int GetMonthDifference(DateTime startDate, DateTime endDate)
    {
        int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
        return Math.Abs(monthsApart);
    }

    protected void btnIngresarSolicitud_Click(object sender, EventArgs e)
    {
        try
        {
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            string lcParametroEncriptar = "";
            lcParametroEncriptar = "SID=" + pcIDSesion + "&IDApp=" + pcIDApp + "&usr=" + pcIDUsuario + "&ID=" + txtIdentidad.Text.Trim();

            //string lcScript = "window.open('resumen.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_CuerpoPrincipal')";
            string lcScript = "window.open('../Solicitudes/Forms/Solicitudes/SolicitudesCredito_Registrar.aspx?" + DSC.Encriptar(lcParametroEncriptar) + "','_Clientes_Contenido')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
        }
        catch(Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
        }
    }
}