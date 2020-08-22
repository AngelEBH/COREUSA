using System;
using System.Configuration;
using System.Web;
using System.Data;
using System.Data.SqlClient;

public partial class Clientes_CotizadorCarros : System.Web.UI.Page
{
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
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
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        }
        /* FIN de captura de parametros y desencriptado de cadena */

        if (ddlPlazos.Items.Count == 0)
        {
            ddlPlazos.Items.Add("12 Meses");
            ddlPlazos.Items.Add("18 Meses");
            ddlPlazos.Items.Add("24 Meses");
            ddlPlazos.Items.Add("36 Meses");
            ddlPlazos.Items.Add("48 Meses");
            ddlPlazos.Items.Add("60 Meses");
        }
    }

    protected void btnCalcular_Click(object sender, EventArgs e)
    {
        bool lbEsNumerico = false;
        decimal liNumero = 0;

        txtValorVehiculo.Text = txtValorVehiculo.Text.Replace(",", "");
        txtValorPrima.Text = txtValorPrima.Text.Replace(",", "");

        if (string.IsNullOrEmpty(txtValorVehiculo.Text) || string.IsNullOrEmpty(txtValorPrima.Text))
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Debe ingresar todos los valores para calcular.";
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorVehiculo.Text, out liNumero);

        if (!lbEsNumerico)
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Ingrese valoes numericos.";
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorPrima.Text, out liNumero);

        if (!lbEsNumerico)
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Ingrese valoes numericos.";
            return;
        }

        if (Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.10)))
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Valor de la prima debe ser mayor o igual al 10%.";
            return;
        }

        if (Convert.ToDouble(txtValorPrima.Text) >= Convert.ToDouble(txtValorVehiculo.Text))
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "La prima debe ser menor que el valor del vehiculo.";
            return;
        }

        PanelMensajeErrores.Visible = false;
        PanelCreditos.Visible = true;
        PanelCreditosDos.Visible = true;
        PanelCreditosTres.Visible = true;
        navTabs.Visible = true;
        tabContent.Visible = true;
        divParametros.Visible = false;
        divNuevoCalculo.Visible = true;

        try
        {
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                string lcMontoVehiculo = string.Format("{0:#,###0.00}", Convert.ToDecimal((Convert.ToDecimal(txtValorVehiculo.Text) - Convert.ToDecimal(txtValorPrima.Text)).ToString()));

                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredCotizadorProductos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", "202");
                    sqlComando.Parameters.AddWithValue("@pnMontoaPrestamo", lcMontoVehiculo.Trim().Replace(",", ""));
                    sqlComando.Parameters.AddWithValue("@liPlazo", ddlPlazos.Text.Substring(0, 3).Trim());
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", txtValorPrima.Text);
                    sqlComando.CommandTimeout = 120;

                    using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                    {
                        sqlResultado.Read();

                        txtValorPrestamo1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString()));
                        txtCuotaPrestamo1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensualNeta"].ToString()));
                        txtValorSeguro1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()));
                        txtServicioGPS1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaServicioGPS"].ToString()));
                        txtCuotaTotal1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensual"].ToString()));
                        txtGastosdeCierre1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnGastosdeCierre"].ToString()));

                        sqlResultado.Read();

                        txtValorPrestamo2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString()));
                        txtCuotaPrestamo2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensualNeta"].ToString()));
                        txtValorSeguro2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()));
                        txtServicioGPS2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaServicioGPS"].ToString()));
                        txtCuotaTotal2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensual"].ToString()));
                        txtGastosdeCierre2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnGastosdeCierre"].ToString()));

                        sqlResultado.Read();

                        txtValorPrestamo3.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString()));
                        txtCuotaPrestamo3.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensualNeta"].ToString()));
                        txtValorSeguro3.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()));
                        txtServicioGPS3.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaServicioGPS"].ToString()));
                        txtCuotaTotal3.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaMensual"].ToString()));
                        txtGastosdeCierre3.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnGastosdeCierre"].ToString()));
                    }
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = ex.Message;
            return;
        }
    }

    protected void btnNuevoCalculo_Click(object sender, EventArgs e)
    {
        try
        {
            txtValorVehiculo.Text = string.Empty;
            txtValorPrima.Text = string.Empty;
            ddlPlazos.SelectedIndex = 0;

            PanelCreditos.Visible = false;
            PanelCreditosDos.Visible = false;
            PanelCreditosTres.Visible = false;
            navTabs.Visible = false;
            tabContent.Visible = false;
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