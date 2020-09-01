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
using System.Globalization;


public partial class Gestion_GestionCobranzaClientesOpcionesFinanciamiento : System.Web.UI.Page
{
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcCategoria = "";
    private decimal pnTotalAtrasado = 0;
    private decimal pnTotalAdeudado = 0;
    private decimal pnCuotaActual = 0;

    private String lcSQLInstruccion = "";
    private String sqlConnectionString = "";
    private SqlConnection sqlConexion = null;
    private SqlDataReader sqlResultado = null;
    private SqlCommand sqlComando = null;

    protected void Page_Load(object sender, EventArgs e)
    {
        TextInfo lcNombrePropio = new CultureInfo("es-US", false).TextInfo;

        /* INICIO de captura de parametros y desencriptado de cadena */
        //DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcURL = "";
        int liParamStart = 0;
        string lcParametros = "";
        string lcEncriptado = "";
        string lcParametroDesencriptado = "";
        string lcIDCliente = "";
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
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            lcIDCliente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDCliente");
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcCategoria = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("CAT");
        }
        /* FIN de captura de parametros y desencriptado de cadena */

        lblTitulo.Text = "Calculadora de opciones de readecuación de deuda";

        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

        try
        {
            sqlConexion.Open();

            lcSQLInstruccion = "exec sp_SRCClienteDetalle '" + lcIDCliente.Trim() + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlResultado = sqlComando.ExecuteReader();
            sqlResultado.Read();
            txtIDCliente.Text = sqlResultado["fcIDCliente"].ToString().Trim();
            txtNombreCliente.Text = sqlResultado["fcNombreSAF"].ToString().Trim();

            lcSQLInstruccion = "exec sp_SRCClientesConvenios_COVID19 '" + lcIDCliente.Trim() + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlResultado = sqlComando.ExecuteReader();
            sqlResultado.Read();

            txtProducto.Text = sqlResultado["fcProducto"].ToString().Trim();
            txtTasa.Text = sqlResultado["fnTasa"].ToString().Trim();
            txtMontoOriginal.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCapitalOtorgado"].ToString().Trim()));
            txtPlazoOtorgado.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiCuotasPrestamo"].ToString().Trim())) + " Cuotas";
            txtValorCuota.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValorCuota"].ToString().Trim()));
            //txtNuevaCuotaQuincenal.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValorCuota"].ToString().Trim()));
            txtCuotasPagadas.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiCuotasPagadas"].ToString().Trim())) + " Cuotas";
            txtFechaInicioCredito.Text = Convert.ToDateTime(sqlResultado["fdFechaApertura"]).ToString("dd/MM/yyyy");
            txtFechaFinCredito.Text = Convert.ToDateTime(sqlResultado["fdFechaVencimiento"]).ToString("dd/MM/yyyy");
            txtDiasAtraso.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiDiasAtraso"].ToString().Trim()));

            txtSaldoCapital.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnSaldoActual"].ToString().Trim()));
            txtCapitalVencido.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCapitalVencido"].ToString().Trim()));
            txtInteresVencido.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnInteresVencido"].ToString().Trim()));
            txtInteresMora.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnInteresMora"].ToString().Trim()));
            txtSaldoSeguros.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnSaldoSeguros"].ToString().Trim()));
            txtSaldosVarios.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnSaldosVarios"].ToString().Trim()));
            txtCuotasAtrasadas.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiCuotasVencidas"].ToString().Trim())) + " Cuotas";

            txtTotalAtrasado.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnTotalAtrasado"].ToString().Trim()));
            txtTotalAdeudado.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnTotalAdeudado"].ToString().Trim()));
            txtCuotasPendientes.Text = string.Format("{0:#,###0}", (Convert.ToDecimal(sqlResultado["fiCuotasPrestamo"].ToString().Trim())) - Convert.ToDecimal(sqlResultado["fiCuotasPagadas"].ToString().Trim()));
            pnTotalAdeudado = Convert.ToDecimal(sqlResultado["fnTotalAdeudado"].ToString().Trim());
            pnTotalAtrasado = Convert.ToDecimal(sqlResultado["fnTotalAtrasado"].ToString().Trim());
            pnCuotaActual = Convert.ToDecimal(sqlResultado["fnValorCuota"].ToString().Trim());


            if (String.IsNullOrEmpty(txtAbonoInicial.Text.Trim()))
            {
                txtAbonoInicial.Text = "0";
            }

            lcSQLInstruccion = "exec sp_SRCClientesConvenios_MultiOpciones '" + lcIDCliente.Trim() + "'," + txtAbonoInicial.Text.Trim();
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlResultado = sqlComando.ExecuteReader();
            gvOpciones.DataSource = sqlResultado;
            gvOpciones.DataBind();


        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
        }

    }

    private void MostrarError(string pcError)
    {
        PanelErrores.Visible = true;
        lblMensajes.Visible = true;
        lblMensajes.Text = pcError;
    }

    protected void btnCalcularConvenio_Click(object sender, EventArgs e)
    {
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {
            sqlConexion.Open();
            lcSQLInstruccion = "exec sp_SRCClientesConvenios_MultiOpciones '" + txtIDCliente.Text.Trim() + "'," + txtAbonoInicial.Text.Trim();
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlResultado = sqlComando.ExecuteReader();
            gvOpciones.DataSource = sqlResultado;
            gvOpciones.DataBind();
        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
        }
    }

    protected void btnOpcionAGeneraraPDF_Click(object sender, EventArgs e)
    {

    }

    protected void btnOpcionBGeneraraPDF_Click(object sender, EventArgs e)
    {

    }

    protected void btnOpcionCGeneraraPDF_Click(object sender, EventArgs e)
    {

    }

    private void GenerarPDF(string nuevaCuota, string nuevoPlazo, string valorReadecuar)
    {
        //String lcScript = "";
        String lcSQLInstruccion = "";
        String sqlConnectionString = "";
        SqlConnection sqlConexion = null;
        string lcIDReadeucacion = "";

        string lcAbonoInicial = txtAbonoInicial.Text.Trim();
        lcAbonoInicial = lcAbonoInicial.Replace("L", "");
        lcAbonoInicial = lcAbonoInicial.Replace(",", "");

        string lcCuotaQuincenal = nuevaCuota;
        lcCuotaQuincenal = lcCuotaQuincenal.Replace("L", "");
        lcCuotaQuincenal = lcCuotaQuincenal.Replace(",", "");

        string lcPlazo = nuevoPlazo;
        lcPlazo = lcPlazo.Replace(" cuotas", "");
        lcPlazo = lcPlazo.Replace(",", "");

        string lcMonto = valorReadecuar;
        lcMonto = lcMonto.Replace("L", "");
        lcMonto = lcMonto.Replace(",", "");

        string lcTotalAdeudado = txtTotalAdeudado.Text;
        lcTotalAdeudado = lcTotalAdeudado.Replace("L ", "");
        lcTotalAdeudado = lcTotalAdeudado.Replace(",", "");

        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {
            sqlConexion.Open();
            lcSQLInstruccion = "exec sp_SRC_ClientesReadecuaciones_Registrar 1," + pcIDApp + "," + pcIDUsuario + ",'" + txtIDCliente.Text.Trim() + "'," + lcTotalAdeudado + "," + lcMonto + "," + lcAbonoInicial + "," + lcCuotaQuincenal + "," + lcPlazo;

            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlResultado = sqlComando.ExecuteReader();
            sqlResultado.Read();

            lcIDReadeucacion = sqlResultado["fcCodigoReadecuacion"].ToString();

            sqlComando.Dispose();
            sqlConexion.Close();

            String lcScript = "window.open('ConvenioReadecuacion.aspx?" + DSC.Encriptar("IDApp=" + pcIDApp + "&IDCliente=" + txtIDCliente.Text.Trim() + "&Convenio=" + lcIDReadeucacion + "&usr=" + pcIDUsuario + "&pcIDApp=" + pcIDApp) + "','postwindow','toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=300')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newwindow", lcScript, true);
        }
        catch (Exception ex)
        {
            MostrarError(ex.Message);
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
        }
    }
}