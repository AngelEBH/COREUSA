using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;

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
        try
        {
            //sqlConexion.Open();
            //string lcSQLInstruccion = "exec dbo.NombreDelProcedimiento 1," + pcIDApp + "," + pcIDUsuario + ",'" + lcIDCliente.Trim() + "','" + pcIDConvenio + "'";
            //SqlCommand sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            //sqlResultado = sqlComando.ExecuteReader();
            //sqlResultado.Read();

            //lblNoAdendum.Text = sqlResultado["fiNoConvenio"].ToString().Trim();
            //lblNombreCliente.Text = sqlResultado["fcNombreCliente"].ToString().Trim();
            //lblIdentidadCliente.Text = sqlResultado["fcIdentidadCliente"].ToString().Trim();
            //lblNoPrestamo.Text = sqlResultado["fiNoPrestamo"].ToString().Trim();

            //lblAbonoInicial.Text = "Lps. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnAbonoInicial"].ToString().Trim()));
            //string TipodeCuota = sqlResultado["fcTipodeCuota"].ToString().Trim();
            //lblTipoCuotaTabla.InnerText = "Cuota " + lblTipoCuotaTabla;
            //lblCantidadCuotas.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCatidadCuotas"].ToString().Trim())) + " " + TipodeCuota;
            //lblPlazoReadecuacion.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fiPlazoReadecuacion"].ToString().Trim())) + " " + TipodeCuota;
            //lblMontoReadecuar.Text = "Lps. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnMontoReadecuar"].ToString().Trim()));

            //DateTime lcConvertirFecha = Convert.ToDateTime(sqlResultado["fdInicioConvenio"].ToString(), CultureInfo.InvariantCulture);
            //lblFechaInicialContrato.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
            //lcConvertirFecha = Convert.ToDateTime(sqlResultado["fdFinConvenio"].ToString(), CultureInfo.InvariantCulture);
            //lblFechaFinalContrato.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
            //lblTipodeCuota.Text = TipodeCuota;

            //lcConvertirFecha = Convert.ToDateTime(sqlResultado["fdFechaPrimeraCuota"].ToString(), CultureInfo.InvariantCulture);
            //lblFechaPrimeraCuota.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
            //lcConvertirFecha = Convert.ToDateTime(sqlResultado["fdFechaUltimaCuota"].ToString(), CultureInfo.InvariantCulture);
            //lblFechaUltimaCuota.Text = lcConvertirFecha.ToString("dd/MM/yyyy");

            //lblCiudad.Text = sqlResultado["fcCiudad"].ToString().Trim();
            //lblDepartamento.Text = sqlResultado["fcDepartamento"].ToString().Trim();
            //lblDias.Text = sqlResultado["fcDiasFirma"].ToString().Trim();
            //lblMes.Text = sqlResultado["fcMesFimar"].ToString().Trim();
            //lblAnio.Text = sqlResultado["lblAnioFirma"].ToString().Trim();
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
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