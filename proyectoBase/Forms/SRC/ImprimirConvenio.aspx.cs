using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Globalization;

public partial class Gestion_ImprimirConvenio : System.Web.UI.Page
{
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDConvenio = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        TextInfo lcNombrePropio = new CultureInfo("es-US", false).TextInfo;

        /* INICIO de captura de parametros y desencriptado de cadena */
        //DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcURL = "";
        int liParamStart = 0;
        string lcParametros = "";
        string lcEncriptado = "";
        string lcParametroDesencriptado = "";
        string lcIDCliente = "";
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
            lcIDCliente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDCliente");
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDConvenio = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("Convenio");
        }
        /* FIN de captura de parametros y desencriptado de cadena */


        String lcSQLInstruccion = "";
        String sqlConnectionString = "";
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

        try
        {
            sqlConexion.Open();
            //sp_SRC_ClientesConvenios_Imprimir(@piIDSesion Int, @piIDApp SmallInt, @piIDUsuario Int, @pcIDCliente VarChar(11), @pcIDCOnvenio VarChar(30))
            lcSQLInstruccion = "exec sp_SRC_ClientesConvenios_Imprimir 1," + pcIDApp + "," + pcIDUsuario + ",'" + lcIDCliente.Trim() + "','" + pcIDConvenio + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlResultado = sqlComando.ExecuteReader();
            sqlResultado.Read();

            lblNombreCliente.Text = sqlResultado["fcNombreSAF"].ToString().Trim();
            lblNombreCliente2.Text = lblNombreCliente.Text;
            lblNombreCliente3.Text = lblNombreCliente.Text;
            lblIdentidad.Text = sqlResultado["fcIdentidad"].ToString().Trim();

            lblTotalAdeudado.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnTotalAtrasado"].ToString().Trim()));
            lblSaldoDespuesPrimerAbono.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnSaldoDespuesdeAbono"].ToString().Trim()));
            lblPrimerPago1.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnPagoInicial"].ToString().Trim()));
            lblCuota.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaConvenio"].ToString().Trim()));
            lblFrecuenciadePago.Text = "Quincenal";

            lblDescuento.Text = sqlResultado["fnPagoInicial"].ToString().Trim();
            lblTiempodeConvenio.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiTiempoQuincenas"].ToString().Trim())) + " quincenas";
            //lblQuincenasConvenio.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiTiempoQuincenas"].ToString().Trim()));
            lblDescuento.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnDescuento"].ToString().Trim()));
            lblAbonoInicial.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnPagoInicial"].ToString().Trim()));
            lblPago2.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnPagoInicial"].ToString().Trim()));

            DateTime lcConveirteFecha = Convert.ToDateTime(sqlResultado["fdInicioConvenio"].ToString(), CultureInfo.InvariantCulture);
            lblInicioConvenio.Text = lcConveirteFecha.ToString("dd/MM/yyyy");

            lcConveirteFecha = Convert.ToDateTime(sqlResultado["fdFinConvenio"].ToString(), CultureInfo.InvariantCulture);
            lblFinConvenio.Text = lcConveirteFecha.ToString("dd/MM/yyyy");

            lcConveirteFecha = Convert.ToDateTime(sqlResultado["fdAcuerdo"].ToString(), CultureInfo.InvariantCulture);
            lblDiadeFirma.Text = lcConveirteFecha.ToString("dd/MM/yyyy");
            lblPrimerDia.Text = lcConveirteFecha.ToString("dd/MM/yyyy");

            lblCiudadAgencia.Text = sqlResultado["fcCiudad"].ToString().Trim();
            lblDepartamentoAgencia.Text = sqlResultado["fcDepartamento"].ToString().Trim();

            lblCodigoDeBarra.Text = "Codigo:" + sqlResultado["fcCodigoConvenio"].ToString().Trim();

        }
        catch (Exception ex)
        {
            lblCodigoDeBarra.Text = ex.Message;
        }
        finally
        {
            sqlConexion.Close();
            sqlConexion.Dispose();
        }

    }
}