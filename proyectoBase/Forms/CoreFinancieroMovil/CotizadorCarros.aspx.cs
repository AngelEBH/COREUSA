using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Clientes_CotizadorCarros : System.Web.UI.Page
{
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcID = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private String lcSQLInstruccion = "";
    private String sqlConnectionString = "";

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
            lcEncriptado = lcEncriptado.Replace("%", "");
            lcEncriptado = lcEncriptado.Replace("%", "");
            lcEncriptado = lcEncriptado.Replace("%", "");
            lcEncriptado = lcEncriptado.Replace("%", "");
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        }

        pcIDUsuario = "1";
        pcIDApp = "1";
    }

    protected void btnCalcular_Click(object sender, EventArgs e)
    {
        txtValorVehiculo.Text = txtValorVehiculo.Text.Replace(",", "");
        txtValorPrima.Text = txtValorPrima.Text.Replace(",", "");

        bool lbEsNumerico = false;
        decimal liNumero = 0;
        string lcProducto = "202";
        if (String.IsNullOrEmpty(txtScorePromedio.Text))
        {
            txtScorePromedio.Text = "0";
        }

        if (rbEmpeno.Checked)
        {
            txtValorPrima.Text = Convert.ToString(Convert.ToDecimal((Convert.ToDecimal(txtValorVehiculo.Text) - Convert.ToDecimal(txtMonto.Text)).ToString()));
        }
        if (rbFinanciamiento.Checked)
        {
            txtMonto.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal((Convert.ToDecimal(txtValorVehiculo.Text) - Convert.ToDecimal(txtValorPrima.Text)).ToString()));
        }
        if (string.IsNullOrEmpty(txtValorVehiculo.Text) || string.IsNullOrEmpty(txtValorPrima.Text))
        {
            lblMensaje.Text = "Debe ingresar todos los valores para calcular.";
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorVehiculo.Text, out liNumero);

        if (!lbEsNumerico)
        {
            lblMensaje.Text = "Ingrese valoes numericos.";
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorPrima.Text, out liNumero);

        if (!lbEsNumerico)
        {
            lblMensaje.Text = "Ingrese valoes numericos.";
            return;
        }

        if (Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.20)))
        {
            lblMensaje.Text = "Valor de la prima debe ser mayor o igual al 20%.";
            return;
        }

        if (Convert.ToDouble(txtScorePromedio.Text) < 0 || Convert.ToDouble(txtScorePromedio.Text) > 999)
        {
            lblMensaje.Text = "El score solo puede ser entre 0 y 999.";
            return;
        }

        if ((Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.45))) && (rbEmpeno.Checked))
        {
            lblMensaje.Text = "Valor de a financiar no puede ser mayor al 55%.";
            lblPorcenajedePrima.Text = "Maximo L " + string.Format("{0:N2}", Convert.ToString(Math.Round(Convert.ToDouble(txtValorVehiculo.Text) * (0.55), 2)));
            lblPorcenajedePrima.Visible = true;
            return;
        }

        lblPorcenajedePrima.Text = string.Format("{0:N2}", Convert.ToString(Math.Round((Convert.ToDouble(txtValorPrima.Text) * 100.00) / Convert.ToDouble(txtValorVehiculo.Text), 2))) + "%";

        if (rbFinanciamiento.Checked || rbEmpeno.Checked)
        {
            lcProducto = rbFinanciamiento.Checked ? "202" : "203";
        }
        else
        {
            lblMensaje.Text = "Seleccione si es financiamiento o empeño.";
            return;
        }

        PanelCreditos1.Visible = true;


        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;

        try
        {
            PanelCreditos1.Visible = true;
            divParametros.Visible = false;
            divNuevoCalculo.Visible = true;

            sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            sqlConexion.Open();

            string lcParametrosSP = "";
            string lcMontoVehiculo = txtMonto.Text;

            lcParametrosSP = pcIDApp + "," + pcIDUsuario + "," + lcProducto + "," + lcMontoVehiculo.Trim().Replace(",", "") + "," + ddlPlazos.Text.Substring(0, 3).Trim() + "," + txtValorPrima.Text + "," + txtScorePromedio.Text.Trim();
            lcSQLInstruccion = "exec CoreFinanciero.dbo.sp_CredCotizadorProductos_ConScore " + lcParametrosSP;
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlComando.CommandTimeout = 120;
            sqlResultado = sqlComando.ExecuteReader();
            sqlResultado.Read();

            txtValorPrestamo1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString()));
            txtCuotaPrestamo1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensualNeta"].ToString()));
            txtValorSeguro1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()));
            txtServicioGPS1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaServicioGPS"].ToString()));
            txtCuotaTotal1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensual"].ToString()));
            txtCuotaTotalGC1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensualConGastosdeCierre"].ToString()));
            txtGastosdeCierre1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnGastosdeCierre"].ToString()));
            lblEtiqueta1.Text = "Tasa al " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnPorcentajeTasadeInteresAnual"].ToString())) + "%";

            sqlConexion.Close();
            sqlConexion.Dispose();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = ex.Message;
            return;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }
    }

    protected void rbEmpeno_CheckedChanged(object sender, EventArgs e)
    {
        divPrima.Visible = !rbEmpeno.Checked;
        txtValorPrima.Visible = !rbEmpeno.Checked;
        lblPorcenajedePrima.Visible = !rbEmpeno.Checked;
        ddlPlazos.Items.Clear();
        ddlPlazos.Items.Add("12 Meses");
        ddlPlazos.Items.Add("18 Meses");
        ddlPlazos.Items.Add("24 Meses");
        ddlPlazos.Items.Add("30 Meses");
        txtMonto.Enabled = true;

    }

    protected void rbFinanciamiento_CheckedChanged(object sender, EventArgs e)
    {
        divPrima.Visible = !rbEmpeno.Checked;
        txtValorPrima.Visible = !rbEmpeno.Checked;
        lblPorcenajedePrima.Visible = !rbEmpeno.Checked;
        ddlPlazos.Items.Clear();
        ddlPlazos.Items.Add("12 Meses");
        ddlPlazos.Items.Add("18 Meses");
        ddlPlazos.Items.Add("24 Meses");
        ddlPlazos.Items.Add("36 Meses");
        ddlPlazos.Items.Add("48 Meses");
        ddlPlazos.Items.Add("60 Meses");
        txtMonto.Enabled = false;
    }

    protected void btnNuevoCalculo_Click(object sender, EventArgs e)
    {
        try
        {
            txtValorVehiculo.Text = string.Empty;
            txtValorPrima.Text = string.Empty;
            txtMonto.Text = string.Empty;
            txtScorePromedio.Text = string.Empty;
            rbEmpeno.Checked = false;
            rbFinanciamiento.Checked = false;
            ddlPlazos.Items.Clear();

            PanelCreditos1.Visible = false;
            divNuevoCalculo.Visible = false;
            divParametros.Visible = true;
        }
        catch (Exception ex)
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = ex.Message;
        }
    }
}