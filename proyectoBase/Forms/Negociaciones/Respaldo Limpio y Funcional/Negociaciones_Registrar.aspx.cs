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
    private const string uploadDir = @"C:\inetpub\wwwroot\Documentos\Negociaciones\Temp\";
    public static NegociacionesDocumentosViewModel DocumentoNegociacion;
    public static bool ImprimirNegociacion = true;
    public static bool GuardarDocumentoNegociacion = true;
    public static bool MostrarMensajeConfirmacion = true;

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
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr") ?? "0";
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID") ?? "";

                    if (!string.IsNullOrWhiteSpace(pcID))
                    {
                        CargarPrecalificado(pcID);
                        CargarUltimaCotizacion(pcID);
                    }

                    LlenarListas();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
            }
        }

        /* Carga de documentos de la negociación */
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

                        /* Proceso de carga */
                        var data = fileUploader.Upload();

                        /* Resultado */
                        if (data["files"].Count == 1)
                            data["files"][0].Remove("file");
                        Response.Write(JsonConvert.SerializeObject(data));

                        /* Al subirse los archivos se guardan en este objeto de sesion */
                        var list = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                        /* Guardar los items del objeto de sesion en nuestra propiedad estatica llamada DocumentoNegociacion */
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
        if (lblMensaje.Text == "")
        {
            PanelMensajeErrores.Visible = false;
        }
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
            if (txtMonto.Text == "" || txtMonto.Text == "0.00")
            {
                MostrarMensaje("Debe ingresar un valor a financiar válido.");
                return;
            }

            txtValorPrima.Text = Convert.ToString(Convert.ToDecimal((Convert.ToDecimal(txtValorVehiculo.Text) - Convert.ToDecimal(txtMonto.Text)).ToString()));
        }
        if (rbFinanciamiento.Checked)
        {
            txtMonto.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal((Convert.ToDecimal(txtValorVehiculo.Text) - Convert.ToDecimal(txtValorPrima.Text)).ToString()));
        }
        if (string.IsNullOrEmpty(txtValorVehiculo.Text) || string.IsNullOrEmpty(txtValorPrima.Text))
        {
            MostrarMensaje("Debe ingresar todos los valores para calcular.");
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorVehiculo.Text, out liNumero);

        if (!lbEsNumerico)
        {
            MostrarMensaje("Ingrese valores numéricos.");
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorPrima.Text, out liNumero);

        if (!lbEsNumerico)
        {
            MostrarMensaje("Ingrese valores numéricos.");
            return;
        }

        if (Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.20)))
        {
            MostrarMensaje("Valor de la prima debe ser mayor o igual al 20%.");
            return;
        }

        if (Convert.ToDouble(txtScorePromedio.Text) < 0 || Convert.ToDouble(txtScorePromedio.Text) > 999)
        {
            MostrarMensaje("El score solo puede ser entre 0 y 999.");
            return;
        }

        if ((Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.45))) && (rbEmpeno.Checked))
        {
            MostrarMensaje("Valor de a financiar no puede ser mayor al 55%.");
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
            MostrarMensaje("Seleccione si es financiamiento o empeño.");
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
            MostrarMensaje("Ocurrió un error al realizar el cálculo de la cotización: " + ex.Message.ToString());
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
            MostrarMensaje("Ocurrió un error al cargar los plazos: " + ex.Message.ToString());
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
            MostrarMensaje("Ocurrió un error al cargar los plazos: " + ex.Message.ToString());
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
            MostrarMensaje("No se pudo cargar la nueva cotización: " + ex.Message.ToString());
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
            ExportarCotizacionPDF(NombreVendedor, TelefonoVendedor, CorreoVendedor, valorVehiculo, prima, montoFinanciar, score, tasaMensual, plazo, cuotaPrestamo, valorGPS, valorSeguro, gastosDeCierre, NombreVendedor, cliente);
        }
        catch (Exception ex)
        {
            MostrarMensaje("No se pudo imprimir la cotización: " + ex.Message.ToString());
            return;
        }
    }


    protected void ExportarCotizacionPDF(string vendedor, string telefonoVendedor, string correoVendedor, decimal valorVehiculo, decimal prima, decimal montoFinanciar, int score, decimal tasaMensual, string plazo, decimal cuotaPrestamo, decimal valorGPS, decimal valorSeguro, decimal gastosDeCierre, string usuarioImprime, string cliente = "CLIENTE FINAL")
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

            string scriptImprimir = "ExportToPDF('Cotizacion_" + DateTime.Now.ToString("yyyy_dd_M_HH_mm_ss") + "')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", scriptImprimir, true);
        }
        catch (Exception ex)
        {
            MostrarMensaje("No se pudo imprimir la cotización: " + ex.Message.ToString());
        }
    }

    #endregion


    #region NEGOCIACIÓN

    /* Guardar negociacion en la base de datos */
    protected void btnGuardarNegociacion_Click(object sender, EventArgs e)
    {
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
            MostrarMensajeNegociacion("Debe ingresar todos los valores para calcular.");
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorVehiculo.Text, out liNumero);

        if (!lbEsNumerico)
        {
            MostrarMensajeNegociacion("Ingrese valores numéricos.");
            return;
        }

        lbEsNumerico = decimal.TryParse(txtValorPrima.Text, out liNumero);

        if (!lbEsNumerico)
        {
            MostrarMensajeNegociacion("Ingrese valores numéricos.");
            return;
        }

        if (Convert.ToDouble(txtValorPrima.Text) < (Convert.ToDouble(txtValorVehiculo.Text) * (0.20)))
        {
            MostrarMensajeNegociacion("Valor de la prima debe ser mayor o igual al 20%.");
            return;
        }

        if (Convert.ToDouble(txtScorePromedio.Text) < 0 || Convert.ToDouble(txtScorePromedio.Text) > 999)
        {
            MostrarMensajeNegociacion("El score solo puede ser entre 0 y 999.");
            return;
        }

        lblPorcenajedePrima.Text = string.Format("{0:N2}", Convert.ToString(Math.Round((Convert.ToDouble(txtValorPrima.Text) * 100.00) / Convert.ToDouble(txtValorVehiculo.Text), 2))) + "%";

        if (rbFinanciamiento.Checked || rbEmpeno.Checked)
        {
            pcIDProducto = rbFinanciamiento.Checked ? "202" : "203";
        }
        else
        {
            MostrarMensajeNegociacion("Seleccione si es financiamiento o empeño.");
            return;
        }

        /* Validar entradas */
        if (ddlAutolote.SelectedValue == "0")
        {
            MostrarMensajeNegociacion("Seleccione un autolote.");
            return;
        }

        if (ddlOrigen.SelectedValue == "0")
        {
            MostrarMensajeNegociacion("Seleccione un origen.");
            return;
        }

        if (ddlPlazos.SelectedValue == "")
        {
            MostrarMensajeNegociacion("Seleccione un plazo.");
            return;
        }

        if (ddlMarca.SelectedValue == "0")
        {
            MostrarMensajeNegociacion("Seleccione una marca y modelo.");
            return;
        }

        if (ddlModelo.SelectedValue == "0")
        {
            MostrarMensajeNegociacion("Seleccione una modelo.");
            return;
        }

        if (ddlUnidadDeMedida.SelectedValue == "0")
        {
            MostrarMensajeNegociacion("Seleccione una unidad de medida para el valor recorrido.");
            return;
        }

        if (DocumentoNegociacion != null)
        {
            if (DocumentoNegociacion.NombreAntiguo == "" || DocumentoNegociacion.NombreAntiguo == null)
            {
                MostrarMensajeNegociacion("Adjunte una fotografía de la garantía.");
                return;
            }
        }

        if (!rbGastosDeCierreEfectivo.Checked && !rbGastosDeCierreFinanciados.Checked)
        {
            MostrarMensajeNegociacion("Seleccione el tipo de financiamiento");
            return;
        }

        try
        {
            // Gastos de cierre efectivo
            if (rbGastosDeCierreEfectivo.Checked)
            {
                var tasa = Convert.ToDecimal(lblEtiqueta1.Text.Replace("%", "").Replace("Tasa al", "").Trim());
                var cuotaPrestamo = Convert.ToDecimal(txtCuotaTotal1.Text.Replace(",", "").Replace("L", "").Trim());
                var servicioGPS = Convert.ToDecimal(txtServicioGPS1.Text.Replace(",", "").Replace("L", "").Trim());
                var valorSeguro = Convert.ToDecimal(txtValorSeguro1.Text.Replace(",", "").Replace("L", "").Trim());
                var gastosDeCierre = Convert.ToDecimal(txtGastosdeCierreEfectivo.Text.Replace(",", "").Replace("L", "").Trim());
                var valorFinanciar = Convert.ToDecimal(txtValorPrestamo1.Text.Replace(",", "").Replace("L", "").Trim()); ;

                GuardarNegociacion(tasa, cuotaPrestamo, servicioGPS, valorSeguro, gastosDeCierre, valorFinanciar);
            }
            // Gastos de cierre financiados
            else if (rbGastosDeCierreFinanciados.Checked)
            {
                var tasa = Convert.ToDecimal(lblEtiqueta2.Text.Replace("%", "").Replace("Tasa al", "").Trim());
                var cuotaPrestamo = Convert.ToDecimal(txtCuotaTotal2.Text.Replace(",", "").Replace("L", "").Trim());
                var servicioGPS = Convert.ToDecimal(txtServicioGPS2.Text.Replace(",", "").Replace("L", "").Trim());
                var valorSeguro = Convert.ToDecimal(txtValorSeguro2.Text.Replace(",", "").Replace("L", "").Trim());
                var gastosDeCierre = 0;
                var valorFinanciar = Convert.ToDecimal(txtValorPrestamo2.Text.Replace(",", "").Replace("L", "").Trim()); ;

                GuardarNegociacion(tasa, cuotaPrestamo, servicioGPS, valorSeguro, gastosDeCierre, valorFinanciar);
            }
        }
        catch (Exception ex)
        {
            MostrarMensajeNegociacion("Error al obtener datos de la negociación: " + ex.Message.ToString());
            return;
        }
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
                    sqlComando.CommandType = CommandType.StoredProcedure;
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
                            txtIdentidadCliente.Text = sqlResultado["fcIdentidad"].ToString();
                            txtNombreCliente.Text = sqlResultado["fcPrimerNombre"].ToString() + " " + sqlResultado["fcSegundoNombre"].ToString() + " " + sqlResultado["fcPrimerApellido"].ToString() + " " + sqlResultado["fcSegundoApellido"].ToString();
                            txtTelefonoCliente.Text = sqlResultado["fcTelefono"].ToString();
                            pcIDProducto = sqlResultado["fiIDProducto"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar precalificado " + pcID + ": " + ex.Message.ToString());
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
                    sqlComando.CommandType = CommandType.StoredProcedure;
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
            MostrarMensaje("Error al cargar información del formulario de negociación: " + ex.Message.ToString());
        }
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
            MostrarMensaje("Error al cargar los modelos de la marca seleccionada: " + ex.Message.ToString());
        }
    }


    /* Nueva negociación */
    protected void btnNuevaNegociacion_Click(object sender, EventArgs e)
    {
        string lcScript = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
        Response.Write("<script>");
        Response.Write(lcScript);
        Response.Write("</script>");
    }


    /* Primer paso para guardar la negociacion (Solicitar identidad en caso de que no venga en la URL o caso contrario, mostrar formulario de negociacion) */
    protected void btnModalGuardarNegociacion_Click(object sender, EventArgs e)
    {
        divInformacionNegociacion.Visible = true;
        string script = "PrimerPasoGuardarNegociacion()";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", script, true);

    }


    /* En caso de que la identidad no se reciba en la URL, se solicita al cliente y luego se verifica si está precalificado en este método */
    protected void btnBuscarIdentidadCliente_Click(object sender, EventArgs e)
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
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", txtBuscarIdentidad.Text.Trim());
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            PanelSolicitarIdentidad.Visible = true;
                            string script = "$('#modalSolicitarIdentidad').modal('hide'); $('#modalClienteNoPrecalificado').modal();";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", script, true);
                        }

                        while (sqlResultado.Read())
                        {
                            txtIdentidadCliente.Text = sqlResultado["fcIdentidad"].ToString();
                            txtNombreCliente.Text = sqlResultado["fcPrimerNombre"].ToString() + " " + sqlResultado["fcSegundoNombre"].ToString() + " " + sqlResultado["fcPrimerApellido"].ToString() + " " + sqlResultado["fcSegundoApellido"].ToString();
                            txtTelefonoCliente.Text = sqlResultado["fcTelefono"].ToString();
                            pcIDProducto = sqlResultado["fiIDProducto"].ToString();
                            pcID = txtBuscarIdentidad.Text.Trim();

                            string script = "$('#modalSolicitarIdentidad').modal('hide'); $('#modalGuardarNegociacion').modal();";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", script, true);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al buscar la identidad proporcionada: " + ex.Message.ToString());
        }
    }


    /* Cuando un cliente no está precalificado y el usuario de click en "Precalificar ahora" */
    protected void btnPrecalificarCliente_Click(object sender, EventArgs e)
    {
        try
        {
            /* Guardar los montos de la cotización y asociarla a la identidad ingresada */
            if (cbGuardarMontosCotizacion.Checked)
            {
                ImprimirNegociacion = false;
                GuardarDocumentoNegociacion = false;
                MostrarMensajeConfirmacion = false;
                pcIDProducto = rbFinanciamiento.Checked ? "202" : "203";
                pcID = txtBuscarIdentidad.Text.Trim();
                DocumentoNegociacion = new NegociacionesDocumentosViewModel() { fcNombreArchivo = "", fcRutaArchivo = "", NombreAntiguo = "", URLArchivo = "" };

                NombreVendedor = "";
                TelefonoVendedor = "";
                CorreoVendedor = "";

                GuardarNegociacion(0, 0, 0, 0, 0, 0);
            }

            RedirigirAPrecalificar();
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al redirigir al precalificado de clientes: " + ex.Message.ToString());
        }
    }


    /* Redirigir a pantalla de precalificar cliente */
    protected void RedirigirAPrecalificar()
    {
        try
        {
            string script = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", script, true);
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al redirigir al precalificado de clientes: " + ex.Message.ToString());
        }
    }


    /* Cuando se reciba la identidad en la URL, buscar la ultima cotización asociada a ella*/
    private void CargarUltimaCotizacion(string pcID)
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDNegociaciones_ListarPorIdentidad", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (sqlResultado.HasRows)
                        {
                            try
                            {
                                while (sqlResultado.Read())
                                {
                                    txtValorVehiculo.Text = sqlResultado["fnValorGarantia"].ToString();
                                    txtValorPrima.Text = sqlResultado["fnValorPrima"].ToString();
                                    txtScorePromedio.Text = sqlResultado["fiScore"].ToString();
                                    pcIDProducto = sqlResultado["fiIDProducto"].ToString();
                                    string plazoSeleccionado = sqlResultado["fiPlazo"].ToString() + " Meses";

                                    if (pcIDProducto == "202")
                                    {
                                        rbFinanciamiento.Checked = true;

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
                                        ddlPlazos.SelectedValue = plazoSeleccionado;
                                    }

                                    if (pcIDProducto == "203")
                                    {
                                        rbEmpeno.Checked = true;

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
                                        ddlPlazos.SelectedValue = plazoSeleccionado;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MostrarMensaje("No se pudo cargar la ultima cotización del cliente: " + pcID);
                            }
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


    /* Guardar o actualizar negociacion en la base de datos */
    private void GuardarNegociacion(decimal tasa, decimal cuotaPrestamo, decimal serivicioGPS, decimal seguro, decimal gastosDeCierre, decimal montoFinanciar)
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
                    sqlComando.CommandType = CommandType.StoredProcedure;
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
                    sqlComando.Parameters.AddWithValue("@piAnio", txtAnio.Text.Replace(",", "").Trim() == "" ? "0" : txtAnio.Text.Replace(",", "").Trim());
                    sqlComando.Parameters.AddWithValue("@pcMatricula", txtMatricula.Text.Trim());
                    sqlComando.Parameters.AddWithValue("@pcColor", txtColor.Text.Trim());
                    sqlComando.Parameters.AddWithValue("@pnRecorrido", txtRecorrido.Text.Replace(",", "").Trim() == "" ? "0" : txtRecorrido.Text.Replace(",", "").Trim());
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
                                if (!GuardarDocumentosNegociacion(DocumentoNegociacion))
                                {
                                    MostrarMensajeNegociacion("No se pudo guardar la documentación de la negociación");
                                }

                                if (!ImprimirNegociacionPDF(tasa, cuotaPrestamo, serivicioGPS, seguro, gastosDeCierre, montoFinanciar))
                                {
                                    MostrarMensajeNegociacion("No se pudo descargar el PDF de la negociación");
                                }

                                /* Mostrar mensaje de exito */
                                if (MostrarMensajeConfirmacion)
                                {
                                    NegociacionGuardadCorrectamente();
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensajeNegociacion("Error al guardar negociación: " + ex.Message.ToString());
        }
    }


    /* Descargar negociacion en PDF */
    private bool ImprimirNegociacionPDF(decimal tasa, decimal cuotaPrestamo, decimal serivicioGPS, decimal valorSeguro, decimal gastosDeCierre, decimal montoFinanciar)
    {
        if (!ImprimirNegociacion)
        {
            return true;
        }
        else
        {
            try
            {
                lblClienteNegociacion.Text = txtNombreCliente.Text;
                lblTelefonoClienteNegociacion.Text = txtTelefonoCliente.Text;
                lblIdentidadClienteNegociacion.Text = txtIdentidadCliente.Text;
                lblFechaNegociacion.Text = DateTime.Now.ToString("MM/dd/yyyy");
                lblOficialNegociosNegociacion.Text = "Oficial de negocios: " + NombreVendedor;
                lblTelefonoVendedorNegociacion.Text = "Teléfono: " + TelefonoVendedor;
                lblCorreoVendedorNegociacion.Text = "Correo: " + CorreoVendedor;

                if(TelefonoVendedor == "")
                {
                    trTelefonoVendedor.Visible = false;
                }

                lblValorVehiculoNegociacion.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(txtValorVehiculo.Text));
                lblPrimaNegociacion.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(txtValorPrima.Text));
                lblMontoFinanciarNegociacion.Text = "L " + string.Format("{0:#,###0.00}", montoFinanciar);
                lblScoreNegociacion.Text = string.Format("{0:#,###0}", txtScorePromedio.Text);
                lblTasaMensualNegociacion.Text = string.Format("{0:#,###0.00}", tasa.ToString()) + "%";
                lblPlazoNegociacion.Text = ddlPlazos.Text;
                lblCuotaDelPrestamoNegociacion.Text = "L " + string.Format("{0:#,###0.00}", cuotaPrestamo);
                lblGPSNegociacion.Text = RespuestaLogica(serivicioGPS);
                lblValorGPSNegociacion.Text = "L " + string.Format("{0:#,###0.00}", serivicioGPS);
                lblSeguroNegociacion.Text = RespuestaLogica(valorSeguro);
                lblValorSeguroNegociacion.Text = "L " + string.Format("{0:#,###0.00}", valorSeguro);
                lblGastosDeCierreNegociacion.Text = RespuestaLogica(gastosDeCierre);
                lblMontoGastosDeCierreNegociacion.Text = "L " + string.Format("{0:#,###0.00}", gastosDeCierre);
                lblMarca.Text = ddlMarca.SelectedItem.Text;
                lblModelo.Text = ddlModelo.SelectedItem.Text;
                lblAnio.Text = txtAnio.Text;
                lblMatricula.Text = txtMatricula.Text;

                lblColor.Text = txtColor.Text;
                lblCilindraje.Text = txtCilindraje.Text;
                lblUnidadDeMedidaRecorrido.Text = "Recorrido (" + ddlUnidadDeMedida.SelectedItem.Text + ")";
                lblRecorrido.Text = txtRecorrido.Text;
                lblOrigenGarantia.Text = ddlOrigen.SelectedItem.Text;
                lblVendedorGarantia.Text = txtVendedor.Text;
                lblAutolote.Text = ddlAutolote.SelectedItem.Text;
                imgVehiculoNegociacion.ImageUrl = @"http://172.20.3.140" + DocumentoNegociacion.URLArchivo;

                tblRequisitosFinanciamiento.Visible = rbFinanciamiento.Checked;
                tblRequisitosFinanciamientoConGarantia.Visible = rbEmpeno.Checked;
                //lblUsuarioImprime.Text = usuarioImprime;

                return true;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                return false;
            }
        }
    }


    private void NegociacionGuardadCorrectamente()
    {
        /* Negociacion guardada correctamente */
        pcID = "";
        txtNombreCliente.Text = "";
        txtIdentidadCliente.Text = "";
        txtTelefonoCliente.Text = "";
        ddlMarca.SelectedValue = "0";
        ddlModelo.Items.Clear();
        ddlModelo.Items.Add(new ListItem("Seleccione una marca", "0"));
        ddlModelo.Enabled = false;

        txtAnio.Text = "";
        txtMatricula.Text = "";
        txtColor.Text = "";
        txtCilindraje.Text = "";
        txtRecorrido.Text = "";
        //ddlUnidadDeMedida.SelectedValue = "0";
        //ddlOrigen.SelectedValue = "0";
        txtVendedor.Text = "";
        //ddlAutolote.SelectedValue = "0";
        DocumentoNegociacion = new NegociacionesDocumentosViewModel();
        txtDetallesGarantia.Value = "";

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

        ImprimirNegociacion = true;
        GuardarDocumentoNegociacion = true;
        MostrarMensajeConfirmacion = true;

        string script = "NegociacionGuardadaCorrectamente();";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", script, true);
    }


    /* Guardar documentos de la negociacion en su respectivo directorio */
    public static bool GuardarDocumentosNegociacion(NegociacionesDocumentosViewModel DocumentoNegociacion)
    {
        bool result;
        try
        {
            if (DocumentoNegociacion != null && GuardarDocumentoNegociacion == true)
            {
                /* Crear el nuevo directorio para los documentos de la negociacion */
                string DirectorioTemporal = @"C:\inetpub\wwwroot\Documentos\Negociaciones\Temp\";
                string NombreCarpetaDocumentos = "Negociacion_" + pcID;
                string DirectorioDocumentos = @"C:\inetpub\wwwroot\Documentos\Negociaciones\" + NombreCarpetaDocumentos + "\\";
                bool CarpetaExistente = Directory.Exists(DirectorioDocumentos);

                if (!CarpetaExistente)
                    Directory.CreateDirectory(DirectorioDocumentos);

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

    #endregion


    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text = mensaje;
    }

    /* Mensajes en el formulario de agregar negociación */
    protected void MostrarMensajeNegociacion(string mensaje)
    {
        PanelMensajeGuardarNegociacion.Visible = true;
        lblMensajeGuardarNegociacion.Visible = true;
        lblMensajeGuardarNegociacion.Text = mensaje;
    }


    private string RespuestaLogica(decimal cantidad)
    {
        return cantidad > 0 ? "SI" : "NO";
    }
}

public class NegociacionesDocumentosViewModel
{
    public string NombreAntiguo { get; set; }
    public string fcNombreArchivo { get; set; }
    public string fcRutaArchivo { get; set; }
    public string URLArchivo { get; set; }
}