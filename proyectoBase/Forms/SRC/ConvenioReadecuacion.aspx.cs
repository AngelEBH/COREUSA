using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

public partial class ConvenioReadecuacion : System.Web.UI.Page
{
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDConvenio = "";
    public string lcNombreArchivoPDF = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcIDCliente = "";
        string lcParametros;

        string lcURL = Request.Url.ToString();
        int liParamStart = lcURL.IndexOf("?");

        if (liParamStart > 0)
            lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
        else
            lcParametros = String.Empty;

        if (lcParametros != String.Empty)
        {
            string lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            string lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            lcIDCliente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDCliente");
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDConvenio = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("Convenio");
            string lcSQLInstruccion = "exec CoreFinanciero.dbo.sp_SRC_ClientesReadecuaciones_Consultar 1," + pcIDApp + "," + pcIDUsuario + ",'" + pcIDConvenio + "'";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                try
                {
                    sqlConexion.Open();
                    using (SqlCommand sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion))
                    {
                        using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                        {
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
                                DateTime lcConvertirFecha = (DateTime)sqlResultado["fdInicioAcuerdo"];
                                lblFechaInicialContrato.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
                                lcConvertirFecha = (DateTime)sqlResultado["fdFinAcuerdo"];
                                lblFechaFinalContrato.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
                                lblTipodeCuota.Text = TipodeCuota;

                                lcConvertirFecha = (DateTime)sqlResultado["fdInicioAcuerdo"];
                                lblFechaPrimeraCuota.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
                                lcConvertirFecha = (DateTime)sqlResultado["fdFinAcuerdo"];
                                lblFechaUltimaCuota.Text = lcConvertirFecha.ToString("dd/MM/yyyy");

                                lblCiudad.Text = sqlResultado["fcCiudad"].ToString().Trim();
                                lblDepartamento.Text = sqlResultado["fcDepartamento"].ToString().Trim();
                                lblDias.Text = sqlResultado["fcDiasFirma"].ToString().Trim();
                                lblMes.Text = sqlResultado["fcMesFimar"].ToString().Trim();
                                lblAnio.Text = sqlResultado["lblAnioFirma"].ToString().Trim();
                            }
                        } // using reader
                    } // using command
                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                    lblNoAdendum.Text = ex.Message;
                }
            } // using connection
        }
    } // page load
}