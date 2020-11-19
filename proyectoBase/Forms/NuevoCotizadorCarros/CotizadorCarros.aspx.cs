using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

public partial class Clientes_CotizadorCarros : System.Web.UI.Page
{
    private string pcIDApp = "";
    private string pcIDUsuario = "";
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
            lcParametros = string.Empty;
        }
        if (lcParametros != string.Empty)
        {
            string lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            lcEncriptado = lcEncriptado.Replace("%2f", "/");
            string lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);            
            Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        }
        /* FIN de captura de parametros y desencriptado de cadena */

        if (ddlProducto.Items.Count == 0)
        {
            ddlGastosdeCierre.Items.Add("");
            ddlGastosdeCierre.Items.Add("Financiado");
            ddlGastosdeCierre.Items.Add("Sin financiar");

            ddlProducto.Items.Add("");
            ddlProducto.Items.Add("Finaciamiento");
            ddlProducto.Items.Add("Empeño");

            ddlGPS.Items.Add("");
            ddlGPS.Items.Add("No");
            ddlGPS.Items.Add("Si - CPI");
            ddlGPS.Items.Add("Si - CableColor");

            ddlSeguro.Items.Add("");
            ddlSeguro.Items.Add("A - Full Cover");
            ddlSeguro.Items.Add("B - Basico");
        }
    }

    protected void btnCalcular_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtScorePromedio.Text))
        {
            txtScorePromedio.Text = "0";
        }

        if (string.IsNullOrEmpty(ddlProducto.SelectedValue.Trim()))
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "Seleccione un producto";
            return;
        }

        var lcProducto = ddlProducto.SelectedValue == "Finaciamiento" ? "202" : "203";


        if (lcProducto == "203")
        {
            txtValorPrima.Text = Convert.ToString(Convert.ToDecimal((Convert.ToDecimal(txtValorVehiculo.Text) - Convert.ToDecimal(txtMonto.Text)).ToString()));
        }
        if (lcProducto == "202")
        {
            txtMonto.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal((Convert.ToDecimal(txtValorVehiculo.Text) - Convert.ToDecimal(txtValorPrima.Text)).ToString()));
        }

        if (string.IsNullOrEmpty(txtValorVehiculo.Text) || string.IsNullOrEmpty(txtValorPrima.Text))
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "Debe ingresar todos los valores para calcular.";
            return;
        }

        decimal liNumero;
        var lbEsNumerico = decimal.TryParse(txtValorVehiculo.Text, out liNumero);

        if (!lbEsNumerico)
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "Ingrese valoes numericos.";
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorPrima.Text, out liNumero);

        if (!lbEsNumerico)
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "Ingrese valoes numericos.";
            return;
        }

        if (Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.20)))
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "Valor de la prima debe ser mayor o igual al 20%.";
            return;
        }

        if (Convert.ToDouble(txtScorePromedio.Text) < 0 || Convert.ToDouble(txtScorePromedio.Text) > 999)
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "El score solo puede ser entre 0 y 999.";
            return;
        }

        if ((Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.45))) && lcProducto == "203")
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "Valor de a financiar no puede ser mayor al 55%.";
            lblPorcenajedePrima.Text = "Maximo L " + string.Format("{0:N2}", Convert.ToString(Math.Round(Convert.ToDouble(txtValorVehiculo.Text) * (0.55), 2)));
            lblPorcenajedePrima.Visible = true;
            return;
        }

        if (string.IsNullOrEmpty(ddlGPS.SelectedValue.Trim()))
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "Seleccione el tipo de GPS";
            return;
        }

        if (string.IsNullOrEmpty(ddlSeguro.SelectedValue.Trim()))
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "Seleccione el tipo de seguro";
            return;
        }

        if (string.IsNullOrEmpty(ddlGastosdeCierre.SelectedValue.Trim()))
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = "Seleccione el metodo de pago de los gastos de cierre";
            return;
        }

        lblPorcenajedePrima.Text = string.Format("{0:N2}", Convert.ToString(Math.Round((Convert.ToDouble(txtValorPrima.Text) * 100.00) / Convert.ToDouble(txtValorVehiculo.Text), 2))) + "%";
        PanelErrores.Visible = false;
        PanelCotizadorResultado.Visible = true;

        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                var lcSeguro = ddlSeguro.SelectedValue == "A - Full Cover" ? "1" : "2";

                var lcGPS = "0";

                if (ddlGPS.SelectedValue == "Si - CPI")
                {
                    lcGPS = "1";
                }
                if (ddlGPS.SelectedValue == "Si - CableColor")
                {
                    lcGPS = "2";
                }

                var lcGastosdeCierre = ddlGastosdeCierre.SelectedValue == "Financiado" ? "1" : "0";

                using (var sqlComando = new SqlCommand("sp_CredCotizadorProductos_Vehiculos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", lcProducto);
                    sqlComando.Parameters.AddWithValue("@pnMontoaPrestamo", txtMonto.Text.Trim().Replace(",", ""));
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", txtValorPrima.Text.Trim().Replace(",", ""));
                    sqlComando.Parameters.AddWithValue("@piScorePromedio", txtScorePromedio.Text.Trim());
                    sqlComando.Parameters.AddWithValue("@piTipodeSeguro", lcSeguro);
                    sqlComando.Parameters.AddWithValue("@piTipodeGPS", lcGPS);
                    sqlComando.Parameters.AddWithValue("@piFinanciandoGastosdeCierre", lcGastosdeCierre);
                    sqlComando.CommandTimeout = 120;

                    using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                    {
                        gvCotizador.DataSource = sqlResultado;
                        gvCotizador.DataBind();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = ex.Message;
            return;
        }
    }

    protected void rbEmpeno_CheckedChanged(object sender, EventArgs e)
    {
        txtMonto.Enabled = true;

    }

    protected void rbFinanciamiento_CheckedChanged(object sender, EventArgs e)
    {
        txtMonto.Enabled = false;
    }

    protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProducto.SelectedValue == "Finaciamiento")
        {
            txtMonto.Enabled = false;
            txtValorPrima.Visible = true;
            lblPrima.Visible = true;
            lblMonto.Text = "Valor a financiar del vehiculo:";
        }
        else
        {
            txtMonto.Enabled = true;
            txtValorPrima.Visible = false;
            lblPrima.Visible = false;
            lblMonto.Text = "Valor del empeño:";
        }
    }
}