using System;
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
    private readonly DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    public string NombreUsuario = "";
    public string CorreoElectronico = "";
    public string Telefono = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        /* Inicio de captura de parametros y desencriptado de cadena */
        var lcURL = Request.Url.ToString();
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
            var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            lcEncriptado = lcEncriptado.Replace("%2f", "/");
            var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

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
            ddlSeguro.Items.Add("B - Basico + Garantía");
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
            lblMensaje.Text = "Ingrese valores numéricos.";
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
            lblPorcentajedePrima.InnerText = "Máximo L " + string.Format("{0:N2}", Convert.ToString(Math.Round(Convert.ToDouble(txtValorVehiculo.Text) * (0.55), 2)));
            lblPorcentajedePrima.Visible = true;
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

        lblPorcentajedePrima.InnerText = string.Format("{0:N2}", Convert.ToString(Math.Round((Convert.ToDouble(txtValorPrima.Text) * 100.00) / Convert.ToDouble(txtValorVehiculo.Text), 2))) + "%";
        lblPorcentajeMonto.InnerText = string.Format("{0:N2}", Convert.ToString(100 - (Math.Round((Convert.ToDouble(txtValorPrima.Text) * 100.00) / Convert.ToDouble(txtValorVehiculo.Text), 2)))) + "%";

        PanelMensajeErrores.Visible = false;

        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                var lcSeguro = string.Empty;
                var lcGPS = "0";

                if (ddlSeguro.SelectedValue == "A - Full Cover")
                {
                    lcSeguro = "1";
                }
                if (ddlSeguro.SelectedValue == "B - Basico + Garantía")
                {
                    lcSeguro = "2";
                }

                if (ddlSeguro.SelectedValue == "C - Basico")
                {
                    lcSeguro = "3";
                }


                var lcGastosdeCierre = ddlGastosdeCierre.SelectedValue == "Financiado" ? "1" : "0";



                if (ddlGPS.SelectedValue == "Si - CPI")
                {
                    lcGPS = "1";
                }
                if (ddlGPS.SelectedValue == "Si - CableColor")
                {
                    lcGPS = "2";
                }

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
                            /* Agregar fila a la tabla que se muestra en la pestaña "Cotización por plazos" */
                            tRowCotizacion = new HtmlTableRow();
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fiIDPlazo"].ToString() });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fiInteresAnual"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnValorVehiculo"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnGastosdeCierre"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCostoGPS"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnTotalaFinanciar"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotadelPrestamo"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotaServicioGPS"].ToString()).ToString("N") });
                            tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnTotalCuota"].ToString()).ToString("N") });
                            tblCotizacionPorPlazos.Rows.Add(tRowCotizacion);

                            /* Agregar fila a la tabla que se muestra en la cotización que se imprime */
                            tRowCotizacionPDF = new HtmlTableRow();
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fiIDPlazo"].ToString() });
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fiInteresAnual"].ToString()).ToString("N") });
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnValorVehiculo"].ToString()).ToString("N") });
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnGastosdeCierre"].ToString()).ToString("N") });
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCostoGPS"].ToString()).ToString("N") });
                            tRowCotizacionPDF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnTotalaFinanciar"].ToString()).ToString("N") });
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


                        /* Llenar tabla de datos para SAF */
                        sqlResultado.NextResult();

                        HtmlTableRow tRowDatosParaSAF;

                        while (sqlResultado.Read())
                        {
                            tRowDatosParaSAF = new HtmlTableRow();
                            tRowDatosParaSAF.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fiIDPlazo"].ToString() });
                            tRowDatosParaSAF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnGastosLegales"].ToString()).ToString("N") });
                            tRowDatosParaSAF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnValorProvision"].ToString()).ToString("N") });
                            tRowDatosParaSAF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnSeguroContinental"].ToString()).ToString("N") });
                            tRowDatosParaSAF.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnTotal"].ToString()).ToString("N") });
                            tRowDatosParaSAF.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcNombrePolizadeSeguro"].ToString()});

                            tblDatosParaSAF.Rows.Add(tRowDatosParaSAF);
                        }

                        lblValorVehiculo.Text = "L " + decimal.Parse(txtValorVehiculo.Text).ToString("N");
                        lblPrima.Text = "L " + decimal.Parse(txtValorPrima.Text).ToString("N");
                        lblMontoAFinanciar.Text = "L " + decimal.Parse(txtMonto.Text).ToString("N");
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

    protected void ddlProducto_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProducto.SelectedValue == "Finaciamiento")
        {
            divValorDelVehiculo.Attributes.Add("class", "col-6");
            txtMonto.Enabled = false;
            txtValorPrima.Visible = true;
            lblPrima.Visible = true;
            lblMonto.Text = "Valor a financiar del vehiculo:";
            lblPorcentajedePrima.Visible = true;
        }
        else
        {
            divValorDelVehiculo.Attributes.Add("class", "col-12");
            txtMonto.Enabled = true;
            txtValorPrima.Visible = false;
            lblPrima.Visible = false;
            lblMonto.Text = "Valor del empeño:";
            lblPorcentajedePrima.Visible = false;
        }

        txtValorVehiculo.Focus();

        CargarScripts();
    }

    protected void CalcularPrima_TextChanged(object sender, EventArgs e)
    {
        try
        {
            string lcValorVehiculo = string.IsNullOrEmpty(txtValorVehiculo.Text) ? "0" : txtValorVehiculo.Text;
            string lcValorPrima = string.IsNullOrEmpty(txtValorPrima.Text) ? "0" : txtValorPrima.Text;
            string lcMontoaFinaciar = string.IsNullOrEmpty(txtMonto.Text) ? "0" : txtMonto.Text;

            if (ddlProducto.SelectedValue == "Finaciamiento")
            {
                txtMonto.Text = Convert.ToString(Convert.ToDecimal((Convert.ToDecimal(lcValorVehiculo) - Convert.ToDecimal(lcValorPrima)).ToString()));
                lcMontoaFinaciar = string.IsNullOrEmpty(txtMonto.Text) ? "0" : txtMonto.Text;
            }
            else
            {
                txtValorPrima.Text = Convert.ToString(Convert.ToDecimal((Convert.ToDecimal(lcValorVehiculo) - Convert.ToDecimal(lcMontoaFinaciar)).ToString()));
                lcValorPrima = string.IsNullOrEmpty(txtValorPrima.Text) ? "0" : txtValorPrima.Text;
            }


            lcMontoaFinaciar = string.IsNullOrEmpty(txtMonto.Text) ? "0" : txtMonto.Text.Replace(",", "");

            ddlSeguro.Items.Clear();
            ddlSeguro.Items.Add("");

            if (Convert.ToDecimal(lcMontoaFinaciar) > 50000)
            {
                ddlSeguro.Items.Add("A - Full Cover");
                ddlSeguro.Items.Add("B - Basico + Garantía");
            }
            else
            {
                ddlSeguro.Items.Add("A - Full Cover");
                ddlSeguro.Items.Add("C - Basico");
            }
            if (Convert.ToDouble(lcValorVehiculo) > 0)
            {
                lblPorcentajedePrima.InnerText = string.Format("{0:N2}", Convert.ToString(Math.Round((Convert.ToDouble(lcValorPrima) * 100.00) / Convert.ToDouble(lcValorVehiculo), 2))) + "%";
                lblPorcentajeMonto.InnerText = string.Format("{0:N2}", Convert.ToString(100 - (Math.Round((Convert.ToDouble(lcValorPrima) * 100.00) / Convert.ToDouble(lcValorVehiculo), 2)))) + "%";
            }

        }
        catch (Exception ex)
        {
            PanelMensajeErrores.Visible = true;
            lblMensaje.Text = ex.Message;
        }
        finally
        {
            CargarScripts();
        }
    }

    protected void ddlSeguro_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlGPS.Items.Clear();
        ddlGPS.Items.Add("");

        if (ddlSeguro.SelectedValue == "B - Basico + Garantía" || ddlSeguro.SelectedValue == "C - Basico")
        {
            ddlGPS.Items.Add("No");
            ddlGPS.Items.Add("Si - CPI");
        }
        else
        {
            ddlGPS.Items.Add("No");
            ddlGPS.Items.Add("Si - CPI");
            ddlGPS.Items.Add("Si - CableColor");
        }
        CargarScripts();
    }

    protected void btnNuevoCalculo_Click(object sender, EventArgs e)
    {
        try
        {
            txtValorVehiculo.Text = string.Empty;
            txtValorPrima.Text = string.Empty;
            lblPorcentajedePrima.InnerText = string.Empty;
            txtMonto.Text = string.Empty;
            lblPorcentajeMonto.InnerText = string.Empty;
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
}