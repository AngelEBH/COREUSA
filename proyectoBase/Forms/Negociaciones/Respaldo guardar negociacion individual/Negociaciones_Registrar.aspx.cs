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
using System.Web.UI.WebControls;

public partial class Negociaciones_Registrar : System.Web.UI.Page
{
    private static string pcIDUsuario = "";
    private static string pcIDApp = "";
    private static string pcIDSesion = "";
    private static string pcID = "";
    private static string pcIDProducto = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    public static NegociacionesDocumentosViewModel DocumentoNegociacion;
    private const string uploadDir = @"C:\inetpub\wwwroot\Documentos\Negociaciones\Temp\";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
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
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");

                    CargarPrecalificado(pcID);
                    LlenarListas();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        /* Carga de documentos */
        string type = Request.QueryString["type"];
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

                Session["tipoDoc"] = 0;

                switch (type)
                {
                    case "upload":

                        // upload process
                        var data = fileUploader.Upload();

                        // response
                        if (data["files"].Count == 1)
                            data["files"][0].Remove("file");
                        Response.Write(JsonConvert.SerializeObject(data));


                        var list = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

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

        if (rbFinanciamiento.Checked || rbEmpeno.Checked)
        {
            pcIDProducto = rbFinanciamiento.Checked ? "202" : "203";
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
                    sqlComando.Parameters.AddWithValue("@piIDProducto", pcIDProducto);
                    sqlComando.Parameters.AddWithValue("@pnMontoaPrestamo", lcMontoVehiculo.Trim().Replace(",", ""));
                    sqlComando.Parameters.AddWithValue("@liPlazo", ddlPlazos.Text.Substring(0, 3).Trim());
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", txtValorPrima.Text);
                    sqlComando.Parameters.AddWithValue("@piScorePromedio", txtScorePromedio.Text.Trim());
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        sqlResultado.Read();

                        // informacion del vendedor
                        string nombreVendedor = sqlResultado["fcNombreCorto"].ToString();
                        string telefonoVendedor = sqlResultado["fcTelefonoMovil"].ToString();
                        string correoVendedor = sqlResultado["fcBuzondeCorreo"].ToString();

                        // Gastos de cierre efectivo
                        if (rbGastosDeCierreEfectivo.Checked)
                        {
                            txtMonto.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString()));

                            var tasa = Convert.ToDecimal(sqlResultado["fnPorcentajeTasadeInteresAnual"].ToString());
                            var cuotaPrestamo = Convert.ToDecimal(sqlResultado["fnCuotaMensual"].ToString());
                            var servicioGPS = Convert.ToDecimal(sqlResultado["fnCuotaServicioGPS"].ToString());
                            var valorSeguro = Convert.ToDecimal(sqlResultado["fnCuotaSegurodeVehiculo"].ToString());
                            var gastosDeCierre = Convert.ToDecimal(sqlResultado["fnGastosdeCierre"].ToString());
                            var valorFinanciar = Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString());

                            GuardarNegociacion(tasa, cuotaPrestamo, servicioGPS, valorSeguro, gastosDeCierre, valorFinanciar, nombreVendedor, telefonoVendedor, correoVendedor);
                        }

                        sqlResultado.Read();

                        // Gastos de cierre financiados
                        if (rbGastosDeCierreFinanciados.Checked)
                        {
                            txtMonto.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString()));

                            var tasa = Convert.ToDecimal(sqlResultado["fnPorcentajeTasadeInteresAnual"].ToString());
                            var cuotaPrestamo = Convert.ToDecimal(sqlResultado["fnCuotaMensual"].ToString());
                            var servicioGPS = Convert.ToDecimal(sqlResultado["fnCuotaServicioGPS"].ToString());
                            var valorSeguro = Convert.ToDecimal(sqlResultado["fnCuotaSegurodeVehiculo"].ToString());
                            var gastosDeCierre = Convert.ToDecimal(sqlResultado["fnGastosdeCierre"].ToString());
                            var valorFinanciar = Convert.ToDecimal(sqlResultado["fnValoraFinanciar"].ToString());

                            GuardarNegociacion(tasa, cuotaPrestamo, servicioGPS, valorSeguro, gastosDeCierre, valorFinanciar, nombreVendedor, telefonoVendedor, correoVendedor);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
            return;
        }
    }

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
                                btnNuevaNegociacion.Visible = true;
                                GuardarSolicitudDocumentos(idNegociacionGuardada, DocumentoNegociacion);
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
                            pcIDProducto = (string)sqlResultado["fiIDProducto"];
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
    public static bool GuardarSolicitudDocumentos(string IDNegociacion, NegociacionesDocumentosViewModel DocumentoNegociacion)
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

    /* Si se empeño */
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

    protected void btnNuevaNegociacion_Click(object sender, EventArgs e)
    {
        string lcScript = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
        Response.Write("<script>");
        Response.Write(lcScript);
        Response.Write("</script>");
    }
}

public class NegociacionesDocumentosViewModel
{
    public string NombreAntiguo { get; set; }
    public string fcNombreArchivo { get; set; }
    public string fcRutaArchivo { get; set; }
    public string URLArchivo { get; set; }
}