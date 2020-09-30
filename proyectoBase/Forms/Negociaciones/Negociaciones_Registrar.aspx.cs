using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Negociaciones_Registrar : System.Web.UI.Page
{
    private static string pcIDUsuario = "";
    private static string pcIDApp = "";
    private static string pcIDSesion = "";
    public static string pcID = "";
    public static string pcIDProducto = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    public static NegociacionesDocumentosViewModel DocumentoNegociacion;
    private const string uploadDir = @"C:\inetpub\wwwroot\Documentos\Negociaciones\Temp\";
    public static bool FinanciarGastosDeCierre = true;


    // Propiedades de CotizadorCarros.cs
    private static string NombreVendedor = "";
    private static string TelefonoVendedor = "";
    private static string CorreoVendedor = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string type = Request.QueryString["type"];

        if (!IsPostBack && (type == "" || type == null))
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
                var lcURL = Request.Url.ToString();
                int liParamStart = lcURL.IndexOf("?");
                pcID = "";

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
                    var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcEncriptado = lcEncriptado.Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID") ?? "";

                    if (!string.IsNullOrWhiteSpace(pcID))
                    {
                        CargarPrecalificado(pcID);
                        CargarUltimaNegociacion(pcID);
                    }

                    LlenarListas();
                }
            }
            catch (Exception ex)
            {
                lblMensaje.Visible = true;
                lblMensaje.Text = ex.Message;
            }
        }

        /* Carga de documentos */
        if (Request.HttpMethod == "POST" && type != null)
        {
            try
            {
                FileUploader fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
                { "limit", 1 },
                { "title", "auto" },
                { "uploadDir", uploadDir },
                { "extensions", new string[] { "jpg", "png"} },
                });

                /* No es necesario para este proceso pero el metodo de subida de archivos lo requiere */
                Session["tipoDoc"] = 0;

                switch (type)
                {
                    case "upload":

                        // Proceso de carga
                        var data = fileUploader.Upload();

                        // Resultado
                        if (data["files"].Count == 1)
                            data["files"][0].Remove("file");
                        Response.Write(JsonConvert.SerializeObject(data));

                        // Al subirse los archivos se guardan en este objeto de sesion
                        var list = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                        // Guardar los items del objeto de sesion en nuestra propiedad estatica llamada DocumentoNegociacion
                        list.ForEach(val =>
                        {
                            DocumentoNegociacion = new NegociacionesDocumentosViewModel()
                            {
                                NombreAntiguo = val.NombreAntiguo,
                                fcNombreArchivo = val.fcNombreArchivo,
                                fcRutaArchivo = val.fcRutaArchivo,
                                URLArchivo = val.URLArchivo
                            };
                        });
                        break;

                    case "remove":
                        string file = Request.Form["file"];

                        if (file != null)
                        {
                            file = FileUploader.FullDirectory(uploadDir) + file;

                            if (File.Exists(file))
                                File.Delete(file);
                        }
                        break;
                }
                Response.End();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
    }

    private void CargarUltimaNegociacion(string pcID)
    {
        //throw new NotImplementedException();
    }

    #region COTIZACIÓN

    /* Calcular cotización */
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
            }
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
            return;
        }
    }

    /* Si se empeño o financiamiento con garantía */
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
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
        }

    }

    /* Si se selecciona financiamiento */
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
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
        }
    }

    /* Nueva cotización */
    protected void btnNuevaCotizacion_Click(object sender, EventArgs e)
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
        string cliente = txtNombreCliente.Text == "" ? "CLIENTE FINAL" : txtNombreCliente.Text;

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
        string cliente = txtNombreCliente.Text == "" ? "CLIENTE FINAL" : txtNombreCliente.Text;

        MandarAImprimirCotizacion(valorVehiculo, prima, montoFinanciar, score, tasa, plazo, cuotaPrestamo, servicioGPS, valorSeguro, gastosCierre, cliente);
    }

    protected void MandarAImprimirCotizacion(decimal valorVehiculo, decimal prima, decimal montoFinanciar, int score, decimal tasaMensual, string plazo, decimal cuotaPrestamo, decimal valorGPS, decimal valorSeguro, decimal gastosDeCierre, string cliente = "CLIENTE FINAL")
    {
        try
        {
            ExportarPDF(NombreVendedor, TelefonoVendedor, CorreoVendedor, valorVehiculo, prima, montoFinanciar, score, tasaMensual, plazo, cuotaPrestamo, valorGPS, valorSeguro, gastosDeCierre, NombreVendedor, cliente);
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = "No se pudo imprimir la cotización: " + ex.Message;
            return;
        }
    }

    protected void ExportarPDF(string vendedor, string telefonoVendedor, string correoVendedor, decimal valorVehiculo, decimal prima, decimal montoFinanciar, int score, decimal tasaMensual, string plazo, decimal cuotaPrestamo, decimal valorGPS, decimal valorSeguro, decimal gastosDeCierre, string usuarioImprime, string cliente = "CLIENTE FINAL")
    {
        try
        {
            lblCliente.Text = cliente;
            lblFechaCotizacion.Text = DateTime.Now.ToString("MM/dd/yyyy");
            lblVendedor.Text = vendedor;
            lblTelefonoVendedor.Text = telefonoVendedor;
            lblCorreoVendedor.Text = correoVendedor;

            lblValorVehiculo.Text = "L " + string.Format("{0:#,###0.00}", valorVehiculo);

            lblPrima.Text = "L " + string.Format("{0:#,###0.00}", prima);

            lblMontoAFinanciar.Text = "L " + string.Format("{0:#,###0.00}", montoFinanciar);

            lblScore.Text = string.Format("{0:#,###0}", score);

            lblTasaMensual.Text = string.Format("{0:#,###0.00}", tasaMensual) + "%";

            lblPlazo.Text = plazo;
            lblCuotaPrestamo.Text = "L " + string.Format("{0:#,###0.00}", cuotaPrestamo);

            lblValorGPS.Text = "L " + string.Format("{0:#,###0.00}", valorGPS);
            lblGPS.Text = RespuestaLogica(valorGPS);

            lblValorSeguro.Text = "L " + string.Format("{0:#,###0.00}", valorSeguro);
            lblSeguro.Text = RespuestaLogica(valorSeguro);

            lblMontoGastosDeCierre.Text = "L " + string.Format("{0:#,###0.00}", gastosDeCierre);
            lblGastosDeCierre.Text = RespuestaLogica(gastosDeCierre);

            //lblUsuarioImprime.Text = "Impreso por "+ usuarioImprime;

            string scriptImprimir = "ExportToPDF('Cotizacion_+" + DateTime.Now.ToString("yyyy_dd_M_HH_mm_ss") + "')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", scriptImprimir, true);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            return;
        }
    }

    private string RespuestaLogica(decimal cantidad)
    {
        return cantidad > 0 ? "SI" : "NO";
    }

    #endregion

    #region NEGOCIACIÓN

    /* Guardar negociacion en la base de datos */
    protected void btnGuardarNegociacion_Click(object sender, EventArgs e)
    {
        btnGuardarNegociacion.Enabled = false;
        txtValorVehiculo.Text = txtValorVehiculo.Text.Replace(",", "");
        txtValorPrima.Text = txtValorPrima.Text.Replace(",", "");

        bool lbEsNumerico = false;
        decimal liNumero = 0;

        if (String.IsNullOrEmpty(txtScorePromedio.Text))
        {
            txtScorePromedio.Text = "0";
        }

        txtMonto.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal((Convert.ToDecimal(txtValorVehiculo.Text) - Convert.ToDecimal(txtValorPrima.Text)).ToString()));

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

        lblPorcenajedePrima.Text = string.Format("{0:N2}", Convert.ToString(Math.Round((Convert.ToDouble(txtValorPrima.Text) * 100.00) / Convert.ToDouble(txtValorVehiculo.Text), 2))) + "%";

        if (rbFinanciamiento.Checked || rbEmpeno.Checked)
        {
            pcIDProducto = rbFinanciamiento.Checked ? "202" : "203";
        }
        else
        {
            lblMensaje.Text = "Seleccione si es financiamiento o empeño.";
            return;
        }

        /* Validar entradas */
        if (ddlAutolote.SelectedValue == "0")
        {
            lblMensaje.Text = "Seleccione un autolote.";
            return;
        }

        if (ddlOrigen.SelectedValue == "0")
        {
            lblMensaje.Text = "Seleccione un origen.";
            return;
        }

        if (ddlPlazos.SelectedValue == "")
        {
            lblMensaje.Text = "Seleccione un plazo.";
            return;
        }

        if (ddlMarca.SelectedValue == "0")
        {
            lblMensaje.Text = "Seleccione una marca y modelo.";
            return;
        }

        if (ddlModelo.SelectedValue == "0")
        {
            lblMensaje.Text = "Seleccione una modelo.";
            return;
        }

        if (ddlUnidadDeMedida.SelectedValue == "0")
        {
            lblMensaje.Text = "Seleccione una unidad de medida para el valor recorrido.";
            return;
        }

        if (DocumentoNegociacion.NombreAntiguo == "" || DocumentoNegociacion.NombreAntiguo == null)
        {
            lblMensaje.Text = "Adjunte una fotografía de la garantía.";
            return;
        }

        try
        {
            // Gastos de cierre efectivo
            if (FinanciarGastosDeCierre == false)
            {
                var tasa = Convert.ToDecimal(lblEtiqueta1.Text.Replace("%", "").Replace("Tasa al", "").Trim());
                var cuotaPrestamo = Convert.ToDecimal(txtCuotaTotal1.Text.Replace(",", "").Replace("L", "").Trim());
                var servicioGPS = Convert.ToDecimal(txtServicioGPS1.Text.Replace(",", "").Replace("L", "").Trim());
                var valorSeguro = Convert.ToDecimal(txtValorSeguro1.Text.Replace(",", "").Replace("L", "").Trim());
                var gastosDeCierre = Convert.ToDecimal(txtGastosdeCierreEfectivo.Text.Replace(",", "").Replace("L", "").Trim());
                var valorFinanciar = Convert.ToDecimal(txtValorPrestamo1.Text.Replace(",", "").Replace("L", "").Trim()); ;

                GuardarNegociacion(tasa, cuotaPrestamo, servicioGPS, valorSeguro, gastosDeCierre, valorFinanciar, NombreVendedor, TelefonoVendedor, CorreoVendedor);
            }

            // Gastos de cierre financiados
            if (FinanciarGastosDeCierre == true)
            {
                var tasa = Convert.ToDecimal(lblEtiqueta2.Text.Replace("%", "").Replace("Tasa al", "").Trim());
                var cuotaPrestamo = Convert.ToDecimal(txtCuotaTotal2.Text.Replace(",", "").Replace("L", "").Trim());
                var servicioGPS = Convert.ToDecimal(txtServicioGPS2.Text.Replace(",", "").Replace("L", "").Trim());
                var valorSeguro = Convert.ToDecimal(txtValorSeguro2.Text.Replace(",", "").Replace("L", "").Trim());
                var gastosDeCierre = 0;
                var valorFinanciar = Convert.ToDecimal(txtValorPrestamo2.Text.Replace(",", "").Replace("L", "").Trim()); ;

                GuardarNegociacion(tasa, cuotaPrestamo, servicioGPS, valorSeguro, gastosDeCierre, valorFinanciar, NombreVendedor, TelefonoVendedor, CorreoVendedor);
            }
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
            return;
        }
    }

    /* Guardar o actualizar negociacion en la base de datos */
    private void GuardarNegociacion(decimal tasa, decimal cuotaPrestamo, decimal serivicioGPS, decimal seguro, decimal gastosDeCierre, decimal montoFinanciar, string nombreVendedor, string telefonoVendedor, string correoVendedor)
    {
        try
        {
            /* Cambiarle el nombre al documento */
            DocumentoNegociacion.fcNombreArchivo = GenerarNombreDocumento();
            DocumentoNegociacion.URLArchivo = "/Documentos/Negociaciones/Negociacion_" + pcID + "/" + DocumentoNegociacion.fcNombreArchivo + ".png";

            using (var context = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["conexionEncriptada"].ToString())))
            {
                context.Open();

                using (var sqlComando = new SqlCommand("sp_CREDNegociaciones_Maestro_Guardar", context))
                {
                    sqlComando.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDCanal", 1);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", pcIDProducto);
                    sqlComando.Parameters.AddWithValue("@piIDTipoNegociacion", 1);
                    sqlComando.Parameters.AddWithValue("@pnValorGarantia", txtValorVehiculo.Text);
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", txtValorPrima.Text);
                    sqlComando.Parameters.AddWithValue("@pnMontoFinanciar", montoFinanciar);
                    sqlComando.Parameters.AddWithValue("@piIDMoneda", 1);
                    sqlComando.Parameters.AddWithValue("@piScore", txtScorePromedio.Text.Trim());
                    sqlComando.Parameters.AddWithValue("@pnTasa", tasa);
                    sqlComando.Parameters.AddWithValue("@piIDTipoDeTasa", 1);
                    sqlComando.Parameters.AddWithValue("@piPlazo", ddlPlazos.Text.Substring(0, 3).Trim());
                    sqlComando.Parameters.AddWithValue("@pnCuotaDelPrestamo", cuotaPrestamo);
                    sqlComando.Parameters.AddWithValue("@pnServicioGPS", serivicioGPS);
                    sqlComando.Parameters.AddWithValue("@pnSeguro", seguro);
                    sqlComando.Parameters.AddWithValue("@pnGastosDeCierre", gastosDeCierre);
                    sqlComando.Parameters.AddWithValue("@piIDOrigenGarantia", ddlOrigen.SelectedValue);
                    sqlComando.Parameters.AddWithValue("@pcNombreVendedorGarantia", txtVendedor.Text.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDAutolote", ddlAutolote.SelectedValue);
                    sqlComando.Parameters.AddWithValue("@piIDMarca", ddlMarca.SelectedValue);
                    sqlComando.Parameters.AddWithValue("@piIDModelo", ddlModelo.SelectedValue);
                    sqlComando.Parameters.AddWithValue("@piAnio", txtAnio.Text.Replace(",", "").Trim());
                    sqlComando.Parameters.AddWithValue("@pcMatricula", txtMatricula.Text.Trim());
                    sqlComando.Parameters.AddWithValue("@pcColor", txtColor.Text.Trim());
                    sqlComando.Parameters.AddWithValue("@pnRecorrido", txtRecorrido.Text.Replace(",", "").Trim());
                    sqlComando.Parameters.AddWithValue("@piUnidadDeDistancia", ddlUnidadDeMedida.SelectedValue);
                    sqlComando.Parameters.AddWithValue("@pcDetalleGarantia", txtDetallesGarantia.Value.Trim());
                    sqlComando.Parameters.AddWithValue("@pcNombreFotografia", DocumentoNegociacion.fcNombreArchivo);
                    sqlComando.Parameters.AddWithValue("@pcRutaFotografia", DocumentoNegociacion.fcRutaArchivo);
                    sqlComando.Parameters.AddWithValue("@pcURLFotografia", DocumentoNegociacion.URLArchivo);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            string idNegociacionGuardada = sqlResultado["MensajeError"].ToString();
                            if (!idNegociacionGuardada.StartsWith("-1"))
                            {
                                btnGuardarNegociacion.Visible = false;
                                btnNuevaCotizacion.Visible = true;
                                GuardarDocumentosNegociacion(DocumentoNegociacion);
                                DescargarPDF(idNegociacionGuardada);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            btnGuardarNegociacion.Enabled = true;
            ex.Message.ToString();
        }
    }

    /* Descargar negociacion en PDF */
    private void DescargarPDF(string idNegociacion)
    {
        //throw new NotImplementedException();
    }

    /* Cargar informacion del precalificado del cliente o redirigirlo a precalificar si todavia no lo esta */
    private void CargarPrecalificado(string pcID)
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion))
                {
                    sqlComando.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            string lcScript = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
                            Response.Write("<script>");
                            Response.Write(lcScript);
                            Response.Write("</script>");
                        }

                        while (sqlResultado.Read())
                        {
                            txtIdentidadCliente.Text = (string)sqlResultado["fcIdentidad"];
                            txtNombreCliente.Text = (string)sqlResultado["fcPrimerNombre"] + " " + (string)sqlResultado["fcSegundoNombre"] + " " + (string)sqlResultado["fcPrimerApellido"] + " " + (string)sqlResultado["fcSegundoApellido"];
                            txtTelefonoCliente.Text = (string)sqlResultado["fcTelefono"];
                            pcIDProducto = (string)sqlResultado["fiIDProducto"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return;
        }
    }

    /* Llenar listas desplegables */
    protected void LlenarListas()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDNegociaciones_Guardar_LlenarListas", sqlConexion))
                {
                    sqlComando.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        // catalogo autolotes
                        ddlAutolote.Items.Clear();
                        while (sqlResultado.Read())
                        {
                            ddlAutolote.Items.Add(new ListItem(sqlResultado["fcAutolote"].ToString(), sqlResultado["fiIDAutolote"].ToString()));
                        }

                        sqlResultado.NextResult();

                        // catalogo de marcas
                        ddlModelo.Items.Clear();
                        ddlModelo.Items.Add(new ListItem("Seleccione una marca", "0"));
                        ddlMarca.Items.Clear();
                        ddlMarca.Items.Add(new ListItem("Seleccionar", "0"));
                        while (sqlResultado.Read())
                        {
                            ddlMarca.Items.Add(new ListItem(sqlResultado["fcMarca"].ToString(), sqlResultado["fiIDMarca"].ToString()));
                        }

                        sqlResultado.NextResult();

                        // catalogo de origenes de garantia
                        ddlOrigen.Items.Clear();
                        while (sqlResultado.Read())
                        {
                            ddlOrigen.Items.Add(new ListItem(sqlResultado["fcOrigenGarantia"].ToString(), sqlResultado["fiIDOrigenGarantia"].ToString()));
                        }

                        sqlResultado.NextResult();

                        // catalogo de tipos de negociacion
                        while (sqlResultado.Read())
                        {

                        }

                        sqlResultado.NextResult();

                        // catalogo de unidad de medida
                        ddlUnidadDeMedida.Items.Clear();
                        while (sqlResultado.Read())
                        {
                            ddlUnidadDeMedida.Items.Add(new ListItem(sqlResultado["fcUnidadDeMedida"].ToString(), sqlResultado["fiUnidadDeMedida"].ToString()));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return;
        }
    }

    /* Guardar documentos de la negociacion en su respectivo directorio */
    public static bool GuardarDocumentosNegociacion(NegociacionesDocumentosViewModel DocumentoNegociacion)
    {
        bool result;
        try
        {
            if (DocumentoNegociacion != null)
            {
                /* Crear el nuevo directorio para los documentos de la negociacion */
                string DirectorioTemporal = @"C:\inetpub\wwwroot\Documentos\Negociaciones\Temp\";
                string NombreCarpetaDocumentos = "Negociacion_" + pcID;
                string DirectorioDocumentos = @"C:\inetpub\wwwroot\Documentos\Negociaciones\" + NombreCarpetaDocumentos + "\\";
                bool CarpetaExistente = System.IO.Directory.Exists(DirectorioDocumentos);

                if (!CarpetaExistente)
                    System.IO.Directory.CreateDirectory(DirectorioDocumentos);

                string ViejoDirectorio = DirectorioTemporal + DocumentoNegociacion.NombreAntiguo;
                string NuevoNombreDocumento = DocumentoNegociacion.fcNombreArchivo;
                string NuevoDirectorio = DirectorioDocumentos + NuevoNombreDocumento + ".png";

                if (File.Exists(ViejoDirectorio))
                    File.Move(ViejoDirectorio, NuevoDirectorio);
            }
            result = true;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            result = false;
        }
        return result;
    }

    /* Generar nombre de documentos */
    public static string GenerarNombreDocumento()
    {
        string lcBloqueFechaHora;
        string lcRespuesta;
        lcBloqueFechaHora = System.DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
        lcBloqueFechaHora = lcBloqueFechaHora.Replace("-", "");
        lcBloqueFechaHora = lcBloqueFechaHora.Replace(":", "");
        lcBloqueFechaHora = lcBloqueFechaHora.Replace(" ", "T");
        lcRespuesta = "N" + "-" + lcBloqueFechaHora;
        return lcRespuesta;
    }

    /* Cargar modelos de la marca seleccionada */
    protected void ddlMarca_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int idMarca = int.Parse(ddlMarca.SelectedValue);

            ddlModelo.Items.Clear();
            ddlModelo.Items.Add(new ListItem("Seleccione una marca", "0"));

            if (idMarca != 0)
            {
                using (var context = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    context.Open();

                    using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDNegociaciones_Guardar_ListarModelosPorIdMarca", context))
                    {
                        sqlComando.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDMarca", idMarca);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                        sqlComando.CommandTimeout = 120;

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            while (sqlResultado.Read())
                            {
                                ddlModelo.Items.Add(new ListItem(sqlResultado["fcModelo"].ToString(), sqlResultado["fiIDModelo"].ToString()));
                            }
                            ddlModelo.Enabled = true;
                        }
                    }
                }
            }
            else
            {
                ddlModelo.SelectedValue = "0";
                ddlModelo.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            return;
        }
    }

    protected void btnNuevaNegociacion_Click(object sender, EventArgs e)
    {
        string lcScript = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
        Response.Write("<script>");
        Response.Write(lcScript);
        Response.Write("</script>");
    }

    /* Primer paso para guardar la negociacion */
    protected void btnModalGuardarNegociacion_Click(object sender, EventArgs e)
    {
        divInformacionNegociacion.Visible = true;
        string script = "PrimerPasoGuardarNegociacion()";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", script, true);

    }

    protected void btnBuscarIdentidadCliente_Click(object sender, EventArgs e)
    {
        try
        {
            pcID = txtBuscarIdentidad.Text.Trim();

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion))
                {
                    sqlComando.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            PanelSolicitarIdentidad.Visible = true;
                            string script = "$('#modalSolicitarIdentidad').modal('hide'); $('#modalGuardarNegociacion').modal();";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", script, true);
                        }

                        while (sqlResultado.Read())
                        {
                            txtIdentidadCliente.Text = (string)sqlResultado["fcIdentidad"];
                            txtNombreCliente.Text = (string)sqlResultado["fcPrimerNombre"] + " " + (string)sqlResultado["fcSegundoNombre"] + " " + (string)sqlResultado["fcPrimerApellido"] + " " + (string)sqlResultado["fcSegundoApellido"];
                            txtTelefonoCliente.Text = (string)sqlResultado["fcTelefono"];
                            pcIDProducto = (string)sqlResultado["fiIDProducto"].ToString();

                            string script = "$('#modalSolicitarIdentidad').modal('hide'); $('#modalGuardarNegociacion').modal();";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", script, true);

                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    protected void btnPrecalificarCliente_Click(object sender, EventArgs e)
    {
        try
        {
            /* Guardar negociacion hasta este momento */
            // ...

            string script = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", script, true);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    // si no encutro la solucion a eso, ocultar y mostrar la solicitud de la identidad
    #endregion
}

public class NegociacionesDocumentosViewModel
{
    public string NombreAntiguo { get; set; }
    public string fcNombreArchivo { get; set; }
    public string fcRutaArchivo { get; set; }
    public string URLArchivo { get; set; }
}