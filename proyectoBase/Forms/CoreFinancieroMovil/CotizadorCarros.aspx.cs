using System;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

public partial class Clientes_CotizadorCarros : System.Web.UI.Page
{
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcID = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        string lcURL = Request.Url.ToString();
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
            string lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            lcEncriptado = lcEncriptado.Replace("%", "");
            lcEncriptado = lcEncriptado.Replace("3d", "=");
            string lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        }
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
            lblMensaje.Text = "Ingrese valores numéricos.";
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorPrima.Text, out liNumero);

        if (!lbEsNumerico)
        {
            lblMensaje.Text = "Ingrese valores numéricos.";
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
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                string lcMontoVehiculo = txtMonto.Text;

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredCotizadorProductos_ConScore", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", lcProducto);
                    sqlComando.Parameters.AddWithValue("@pnMontoaPrestamo", lcMontoVehiculo.Trim().Replace(",", ""));
                    sqlComando.Parameters.AddWithValue("@liPlazo", ddlPlazos.Text.Substring(0, 3).Trim());
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", txtValorPrima.Text);
                    sqlComando.Parameters.AddWithValue("@piScorePromedio", txtScorePromedio.Text.Trim());
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        // Gastos de cierre efectivo
                        sqlResultado.Read();

                        lblEtiqueta1.Text = "Tasa al " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnPorcentajeTasadeInteresAnual"].ToString())) + "%";
                        txtValorPrestamo1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString()));
                        txtCuotaPrestamo1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensualNeta"].ToString()));
                        txtValorSeguro1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()));
                        txtServicioGPS1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaServicioGPS"].ToString()));
                        txtCuotaTotal1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensual"].ToString()));
                        txtGastosdeCierreEfectivo.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnGastosdeCierre"].ToString()));

                        // Gastos de cierre financiados                        
                        sqlResultado.Read();

                        lblEtiqueta2.Text = "Tasa al " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnPorcentajeTasadeInteresAnual"].ToString())) + "%";
                        txtValorPrestamo2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString()));
                        txtCuotaPrestamo2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensualNeta"].ToString()));
                        txtValorSeguro2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()));
                        txtServicioGPS2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaServicioGPS"].ToString()));
                        txtCuotaTotal2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensual"].ToString()));
                        txtCuotaTotalGastosDeCierreFinanciados.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensualConGastosdeCierre"].ToString()));

                    }
                }
                lblMensaje.Visible = false;

                PanelCreditos1.Visible = true;
                divParametros.Visible = false;
                divNuevoCalculo.Visible = true;

                CargarScripts();
            }
        }
        catch (Exception ex)
        {
            lblMensaje.Text = ex.Message;
            return;
        }
    }

    protected void rbEmpeno_CheckedChanged(object sender, EventArgs e)
    {
        divPrima.Visible = !rbEmpeno.Checked;
        txtValorPrima.Visible = !rbEmpeno.Checked;
        lblPorcenajedePrima.Visible = !rbEmpeno.Checked;

        divMontoFinanciarVehiculo.Visible = rbEmpeno.Checked;
        txtMonto.Enabled = rbEmpeno.Checked;

        ddlPlazos.Items.Clear();
        ddlPlazos.Items.Add("12 Meses");
        ddlPlazos.Items.Add("18 Meses");
        ddlPlazos.Items.Add("24 Meses");
        ddlPlazos.Items.Add("30 Meses");
        CargarScripts();

    }

    protected void rbFinanciamiento_CheckedChanged(object sender, EventArgs e)
    {
        divPrima.Visible = !rbEmpeno.Checked;
        txtValorPrima.Visible = !rbEmpeno.Checked;
        lblPorcenajedePrima.Visible = !rbEmpeno.Checked;
        divMontoFinanciarVehiculo.Visible = rbEmpeno.Checked;
        txtMonto.Enabled = rbEmpeno.Checked;

        ddlPlazos.Items.Clear();
        ddlPlazos.Items.Add("12 Meses");
        ddlPlazos.Items.Add("18 Meses");
        ddlPlazos.Items.Add("24 Meses");
        ddlPlazos.Items.Add("36 Meses");
        ddlPlazos.Items.Add("48 Meses");
        ddlPlazos.Items.Add("60 Meses");
        CargarScripts();
    }

    protected void btnNuevoCalculo_Click(object sender, EventArgs e)
    {
        try
        {
            txtValorVehiculo.Text = string.Empty;
            txtValorPrima.Text = string.Empty;
            lblPorcenajedePrima.Visible = false;
            txtMonto.Text = string.Empty;
            txtScorePromedio.Text = string.Empty;
            rbEmpeno.Checked = false;
            rbFinanciamiento.Checked = false;
            ddlPlazos.Items.Clear();

            PanelCreditos1.Visible = false;
            divNuevoCalculo.Visible = false;
            divParametros.Visible = true;
            CargarScripts();
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
        }
    }

    private void CargarScripts()
    {
        string scriptMascarasDeEntrada =
        "<script>$('.MascaraCantidad').inputmask('decimal', { " +
        "alias: 'numeric', " +
                "groupSeparator: ',', " +
                "digits: 2, " +
                "integerDigits: 11, " +
                "digitsOptional: false, " +
                "placeholder: '0', " +
                "radixPoint: '.', " +
                "autoGroup: true, " +
                "min: 0.0, " +
            "}); " +
            "$('.MascaraNumerica').inputmask('decimal', { " +
        "alias: 'numeric', " +
                "groupSeparator: ',', " +
                "digits: 0, " +
                "integerDigits: 3," +
                "digitsOptional: false, " +
                "placeholder: '0', " +
                "radixPoint: '.', " +
                "autoGroup: true, " +
                "min: 0.0, " +
            "}); " +
        "$('.identidad').inputmask('9999999999999'); </script>";

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", scriptMascarasDeEntrada, false);
    }
}