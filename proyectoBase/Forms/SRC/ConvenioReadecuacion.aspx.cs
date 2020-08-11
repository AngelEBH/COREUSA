using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class ConvenioReadecuacion : System.Web.UI.Page
{
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDConvenio = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        string lcIDCliente = "";
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
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
            string lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            lcIDCliente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDCliente");
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDConvenio = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("Convenio");
        }

        SqlDataReader sqlResultado = null;
        string sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
        SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        string lcSQLInstruccion = "";
        string lcNombreArchivoPDF = "";

        try
        {
            sqlConexion.Open();
            lcSQLInstruccion = "exec CoreFinanciero.dbo.sp_SRC_ClientesReadecuaciones_Consultar 1," + pcIDApp + "," + pcIDUsuario + ",'" + pcIDConvenio + "'";
            SqlCommand sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlResultado = sqlComando.ExecuteReader();

            if (sqlResultado.Read())
            {

                lblNoAdendum.Text = sqlResultado["fcCodigoReadecuacion"].ToString().Trim();
                lblNombreCliente.Text = sqlResultado["fcNombreSAF"].ToString().Trim();
                lblIdentidadCliente.Text = sqlResultado["fcIdentidad"].ToString().Trim();
                lblNoPrestamo.Text = sqlResultado["fcPrestamo"].ToString().Trim();
                lcNombreArchivoPDF = lblNombreCliente.Text.Replace(" ", "_") + "_" + sqlResultado["fcIDCliente"].ToString().Trim();

                lblAbonoInicial.Text = "L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnPagoInicial"].ToString().Trim()));
                string TipodeCuota = sqlResultado["fcTipodeCuota"].ToString().Trim();
                lblTipoCuotaTabla.InnerText = "Cuota " + TipodeCuota;
                lblCantidadCuotas.Text = "L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValorCuota"].ToString().Trim()));
                lblPlazoReadecuacion.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiPlazoQuincenas"].ToString().Trim())) + " " + TipodeCuota;
                lblMontoReadecuar.Text = "L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnTotalReadecuacion"].ToString().Trim()));
                DateTime lcConvertirFecha = Convert.ToDateTime(sqlResultado["fdInicioAcuerdo"].ToString(), CultureInfo.InvariantCulture);
                lblFechaInicialContrato.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
                lcConvertirFecha = Convert.ToDateTime(sqlResultado["fdFinAcuerdo"].ToString(), CultureInfo.InvariantCulture);
                lblFechaFinalContrato.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
                lblTipodeCuota.Text = TipodeCuota;

                lcConvertirFecha = Convert.ToDateTime(sqlResultado["fdInicioAcuerdo"].ToString(), CultureInfo.InvariantCulture);
                lblFechaPrimeraCuota.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
                lcConvertirFecha = Convert.ToDateTime(sqlResultado["fdFinAcuerdo"].ToString(), CultureInfo.InvariantCulture);
                lblFechaUltimaCuota.Text = lcConvertirFecha.ToString("dd/MM/yyyy");

                lblCiudad.Text = sqlResultado["fcCiudad"].ToString().Trim();
                lblDepartamento.Text = sqlResultado["fcDepartamento"].ToString().Trim();
                lblDias.Text = sqlResultado["fcDiasFirma"].ToString().Trim();
                lblMes.Text = sqlResultado["fcMesFimar"].ToString().Trim();
                lblAnio.Text = sqlResultado["lblAnioFirma"].ToString().Trim();

                /*ExportHtmlToPdf('#GenerarConvenio', 'ConvenioRecaudacion').done(CerrarVentana());*/

            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            lblNoAdendum.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion != null)
            {
                if (sqlConexion.State == ConnectionState.Open)
                    sqlConexion.Close();
            }
            if (sqlResultado != null)
                sqlResultado.Close();
        }
    }
}

 