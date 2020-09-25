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

    private static string NombreVendedor = "";
    private static string TelefonoVendedor = "";
    private static string CorreoVendedor = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        try
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
                lcEncriptado = lcEncriptado.Replace("%2f", "/");
                string lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            }
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
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

                        // informacion del vendedor 
                        NombreVendedor = sqlResultado["fcNombreCorto"].ToString();
                        TelefonoVendedor = sqlResultado["fcTelefonoMovil"].ToString();
                        CorreoVendedor = sqlResultado["fcBuzondeCorreo"].ToString();

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
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
            return;
        }
    }

    protected void rbEmpeno_CheckedChanged(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
        }

    }

    protected void rbFinanciamiento_CheckedChanged(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
        }
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
        try
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
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
        }
    }

    /* Cotización con gastos de cierre en efectivo */
    protected void btnDescargarCotizacion_Click(object sender, EventArgs e)
    {
        decimal valorVehiculo = Convert.ToDecimal(txtValorVehiculo.Text.Replace(",", "").Replace("L", "").Trim());
        decimal prima = Convert.ToDecimal(txtValorPrima.Text.Replace(",", "").Replace("L", ",").Trim() ?? "0");
        decimal montoFinanciar = Convert.ToDecimal(txtValorPrestamo1.Text.Replace(",", "").Replace("L", "").Trim());
        int score = Convert.ToInt32(txtScorePromedio.Text.Replace(",", ""));
        decimal tasa = Convert.ToDecimal(lblEtiqueta1.Text.Replace("%", "").Replace("Tasa al", "").Trim());
        string plazo = ddlPlazos.Text;
        decimal cuotaPrestamo = Convert.ToDecimal(txtCuotaTotal1.Text.Replace(",", "").Replace("L", "").Trim());
        decimal servicioGPS = Convert.ToDecimal(txtServicioGPS1.Text.Replace(",", "").Replace("L", "").Trim());
        decimal valorSeguro = Convert.ToDecimal(txtValorSeguro1.Text.Replace(",", "").Replace("L", "").Trim());
        decimal gastosCierreEfectivo = Convert.ToDecimal(txtGastosdeCierreEfectivo.Text.Replace(",", "").Replace("L", "").Trim());
        string cliente = "CLIENTE FINAL";

        MandarAImprimirCotizacion(valorVehiculo, prima, montoFinanciar, score, tasa, plazo, cuotaPrestamo, servicioGPS, valorSeguro, gastosCierreEfectivo, cliente);
    }

    /* Cotización con gastos de cierre financiados */
    protected void btnDescargarCotizacion2_Click(object sender, EventArgs e)
    {
        decimal valorVehiculo = Convert.ToDecimal(txtValorVehiculo.Text.Replace(",", "").Replace("L", "").Trim());
        decimal prima = Convert.ToDecimal(txtValorPrima.Text.Replace(",", "").Replace("L", ",").Trim() ?? "0");
        decimal montoFinanciar = Convert.ToDecimal(txtValorPrestamo2.Text.Replace(",", "").Replace("L", "").Trim());
        int score = Convert.ToInt32(txtScorePromedio.Text.Replace(",", ""));
        decimal tasa = Convert.ToDecimal(lblEtiqueta2.Text.Replace("%", "").Replace("Tasa al", "").Trim());
        string plazo = ddlPlazos.Text;

        decimal cuotaPrestamo = Convert.ToDecimal(txtCuotaTotal2.Text.Replace(",", "").Replace("L", "").Trim());
        decimal servicioGPS = Convert.ToDecimal(txtServicioGPS2.Text.Replace(",", "").Replace("L", "").Trim());
        decimal valorSeguro = Convert.ToDecimal(txtValorSeguro2.Text.Replace(",", "").Replace("L", "").Trim());

        decimal gastosCierre = 0;
        string cliente = "CLIENTE FINAL";

        MandarAImprimirCotizacion(valorVehiculo, prima, montoFinanciar, score, tasa, plazo, cuotaPrestamo, servicioGPS, valorSeguro, gastosCierre, cliente);
    }

    protected void MandarAImprimirCotizacion(decimal valorVehiculo, decimal prima, decimal montoFinanciar, int score, decimal tasaMensual, string plazo, decimal cuotaPrestamo, decimal valorGPS, decimal valorSeguro, decimal gastosDeCierre, string cliente = "CLIENTE FINAL")
    {
        try
        {
            string parametros = "ValorVehiculo=" + valorVehiculo + "&" +
            "Prima=" + prima + "&" +
            "MontoFinanciar=" + montoFinanciar + "&" +
            "Score=" + score + "&" +
            "TasaMensual=" + tasaMensual + "&" +
            "Plazo=" + plazo + "&" +
            "CuotaPrestamo=" + cuotaPrestamo + "&" +
            "ValorGPS=" + valorGPS + "&" +
            "ValorSeguro=" + valorSeguro + "&" +
            "GastosDeCierre=" + gastosDeCierre + "&" +
            "Cliente=" + cliente + "&" +
            "Vendedor=" + NombreVendedor + "&" +
            "TelefonoVendedor=" + TelefonoVendedor + "&" +
            "CorreoVendedor=" + CorreoVendedor + "&";

            string scriptRedireccionar = "window.open('ImprimirCotizacion.aspx?" + parametros + "','_self')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", scriptRedireccionar, true);

            //window.open('ImprimirCotizacion.aspx?ValorVehiculo=100000.00&Prima=20000.00&MontoFinanciar=82468.78&Score=970&TasaMensual=26.00&Plazo=36 Meses&CuotaPrestamo=4553.04&ValorGPS=197.50&ValorSeguro=277.92&GastosDeCierre=2800.00&Cliente=CLIENTE FINAL&Vendedor=Edwin Aguilar&TelefonoVendedor=&CorreoVendedor=edwin.aguilar@miprestadito.com&', '_self')
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = "No se pudo imprimir la cotización: " + ex.Message;
            return;
        }
    }
}