﻿using System;
using System.Web;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Web.UI.HtmlControls;

public partial class CotizadorCarros : System.Web.UI.Page
{
    private string pcIDApp = "";
    private string pcIDUsuario = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    public string NombreUsuario = "";    
    public string CorreoElectronico = "";
    public string Telefono = "";

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
            ddlGastosdeCierre.Items.Add(new ListItem("Seleccionar", ""));
            ddlGastosdeCierre.Items.Add("Financiado");
            ddlGastosdeCierre.Items.Add("Sin financiar");

            ddlProducto.Items.Add(new ListItem("Seleccionar", ""));
            ddlProducto.Items.Add("Finaciamiento");
            ddlProducto.Items.Add("Empeño");

            ddlGPS.Items.Add(new ListItem("Seleccionar", ""));
            ddlGPS.Items.Add("No");
            ddlGPS.Items.Add("Si - CPI");
            ddlGPS.Items.Add("Si - CableColor");

            ddlSeguro.Items.Add(new ListItem("Seleccionar", ""));
            ddlSeguro.Items.Add("A - Full Cover");
            ddlSeguro.Items.Add("B - Basico");
        }
    }

    protected void btnCalcular_Click(object sender, EventArgs e)
    {
        txtValorVehiculo.Text = txtValorVehiculo.Text.Replace(",", "");
        txtValorPrima.Text = txtValorPrima.Text.Replace(",", "");

        if (string.IsNullOrEmpty(txtScorePromedio.Text))
        {
            txtScorePromedio.Text = "0";
        }

        if (string.IsNullOrEmpty(ddlProducto.SelectedValue.Trim()))
        {
            PanelMensajeErrores.Visible = true;
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
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Debe ingresar todos los valores para calcular.";
            return;
        }

        decimal liNumero;
        var lbEsNumerico = decimal.TryParse(txtValorVehiculo.Text, out liNumero);

        if (!lbEsNumerico)
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Ingrese valores numéricos.";
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorPrima.Text, out liNumero);

        if (!lbEsNumerico)
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Ingrese valoes numericos.";
            return;
        }

        if (Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.20)))
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Valor de la prima debe ser mayor o igual al 20%.";
            return;
        }

        if (Convert.ToDouble(txtScorePromedio.Text) < 0 || Convert.ToDouble(txtScorePromedio.Text) > 999)
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "El score solo puede ser entre 0 y 999.";
            return;
        }

        if ((Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.45))) && lcProducto == "203")
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Valor de a financiar no puede ser mayor al 55%.";
            lblPorcenajedePrima.InnerText = "Maximo L " + string.Format("{0:N2}", Convert.ToString(Math.Round(Convert.ToDouble(txtValorVehiculo.Text) * (0.55), 2)));
            lblPorcenajedePrima.Visible = true;
            return;
        }

        if (string.IsNullOrEmpty(ddlGPS.SelectedValue.Trim()))
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Seleccione el tipo de GPS";
            return;
        }

        if (string.IsNullOrEmpty(ddlSeguro.SelectedValue.Trim()))
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Seleccione el tipo de seguro";
            return;
        }

        if (string.IsNullOrEmpty(ddlGastosdeCierre.SelectedValue.Trim()))
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = "Seleccione el metodo de pago de los gastos de cierre";
            return;
        }

        lblPorcenajedePrima.InnerText = string.Format("{0:N2}", Convert.ToString(Math.Round((Convert.ToDouble(txtValorPrima.Text) * 100.00) / Convert.ToDouble(txtValorVehiculo.Text), 2))) + "%";
        PanelMensajeErrores.Visible = false;

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
                        HtmlTableRow tRowCotizacion;
                        HtmlTableRow tRowCotizacionPDF;

                        while (sqlResultado.Read())
                        {
                            tRowCotizacion = new HtmlTableRow();
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fiIDPlazo"].ToString() });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotadelPrestamo"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotaServicioGPS"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnTotalCuota"].ToString()).ToString("N") });
                            tblCotizacionPorPlazos.Rows.Add(tRowCotizacion);

                            tRowCotizacionPDF = new HtmlTableRow();
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fiIDPlazo"].ToString() });
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotadelPrestamo"].ToString()).ToString("N") });
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()).ToString("N") });
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotaServicioGPS"].ToString()).ToString("N") });
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnTotalCuota"].ToString()).ToString("N") });

                            tblCotizacionPorPlazosPDF.Rows.Add(tRowCotizacionPDF);

                            lblFechaNegociacion.Text = DateTime.Now.ToString("MM/dd/yyyy");
                            lblOficialNegociosNegociacion.Text = "Oficial de negocios: " + sqlResultado["fcNombreCorto"].ToString();
                            lblTelefonoVendedorNegociacion.Text = "Teléfono: " + sqlResultado["fcTelefonoUsuario"].ToString();
                            lblCorreoVendedorNegociacion.Text = "Correo: " + sqlResultado["fcCorreoElectronico"].ToString();

                            if (sqlResultado["fcTelefonoUsuario"].ToString().Trim() == string.Empty)
                            {
                                trTelefonoVendedor.Visible = false;
                            }
                        }

                        lblValorVehiculo.Text = "L " + decimal.Parse(txtValorVehiculo.Text).ToString("N");
                        lblPrima.Text = "L " +  decimal.Parse(txtValorPrima.Text).ToString("N");
                        lblMontoAFinanciar.Text = "L " +  decimal.Parse(txtMonto.Text).ToString("N");
                        lblScore.Text = txtScorePromedio.Text;

                        lblGPS.Text = ddlGPS.SelectedValue;
                        lblSeguro.Text = ddlSeguro.SelectedValue;
                        lblMontoGastosDeCierre.Text = ddlGastosdeCierre.SelectedValue;
                    }
                }


                /* Inhabilitar campos de cotizacion */
                ddlProducto.Enabled = false;
                txtValorVehiculo.Enabled = false;
                txtValorPrima.Enabled = false;
                txtMonto.Enabled = false;
                txtScorePromedio.Enabled = false;
                ddlGastosdeCierre.Enabled = false;
                ddlSeguro.Enabled = false;
                ddlGPS.Enabled = false;

                lblMensaje.Visible = false;
                btnCalcular.Visible = false;
                divResultados.Visible = true;
                btnNuevoCalculo.Visible = true;
            }
        }
        catch (Exception ex)
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = ex.Message;
            return;
        }
        finally
        {
            CargarScripts();
        }
    }

    protected void rbEmpeno_CheckedChanged(object sender, EventArgs e)
    {
        txtMonto.Enabled = true;
        CargarScripts();

    }

    protected void rbFinanciamiento_CheckedChanged(object sender, EventArgs e)
    {
        txtMonto.Enabled = false;
        CargarScripts();
    }

    protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProducto.SelectedValue == "Finaciamiento")
        {
            txtMonto.Enabled = false;
            divValorDelVehiculo.Attributes.Add("class", "col-6");
            txtValorPrima.Visible = true;
            divPrima.Visible = true;
            lblMonto.Text = "Valor a financiar del vehiculo:";
        }
        else
        {
            divValorDelVehiculo.Attributes.Add("class", "col-12");
            txtMonto.Enabled = true;
            txtValorPrima.Visible = false;
            divPrima.Visible = false;
            lblMonto.Text = "Valor del empeño:";
        }

        CargarScripts();
    }

    protected void btnNuevoCalculo_Click(object sender, EventArgs e)
    {
        try
        {
            txtValorVehiculo.Text = string.Empty;
            txtValorPrima.Text = string.Empty;
            lblPorcenajedePrima.InnerText = string.Empty;
            txtMonto.Text = string.Empty;
            txtScorePromedio.Text = string.Empty;
            ddlGastosdeCierre.SelectedValue = string.Empty;
            ddlGPS.SelectedValue = string.Empty;
            ddlProducto.SelectedValue = string.Empty;
            ddlSeguro.SelectedValue = string.Empty;

            /* Habilitar campos de cotizacion */
            ddlProducto.Enabled = true;
            txtValorVehiculo.Enabled = true;
            txtValorPrima.Enabled = true;
            txtMonto.Enabled = true;
            txtScorePromedio.Enabled = true;
            ddlGastosdeCierre.Enabled = true;
            ddlSeguro.Enabled = true;
            ddlGPS.Enabled = true;

            btnCalcular.Visible = true;
            btnNuevoCalculo.Visible = false;
            divResultados.Visible = false;
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

    private string RespuestaLogica(decimal cantidad)
    {
        return cantidad > 0 ? "SI" : "NO";
    }
}