using System;
using System.Configuration;
using System.Web;
using System.Data.SqlClient;
using System.Globalization;

public partial class Gestion_ImprimirAdendum31a120 : System.Web.UI.Page
{
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDConvenio = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        TextInfo lcNombrePropio = new CultureInfo("es-US", false).TextInfo;

        /* INICIO de captura de parametros y desencriptado de cadena */
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
            lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
        else
            lcParametros = String.Empty;

        if (lcParametros != String.Empty)
        {
            lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            lcIDCliente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDCliente");
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDConvenio = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("Convenio");
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                try
                {
                    sqlConexion.Open();
                    //sp_SRC_ClientesConvenios_Imprimir(@piIDSesion Int, @piIDApp SmallInt, @piIDUsuario Int, @pcIDCliente VarChar(11), @pcIDCOnvenio VarChar(30))
                    string lcSQLInstruccion = "exec sp_SRC_ClientesConvenios_Imprimir 1," + pcIDApp + "," + pcIDUsuario + ",'" + lcIDCliente.Trim() + "','" + pcIDConvenio + "'";
                    
                    using (SqlCommand sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion))
                    {
                        using (SqlDataReader sqlResultado = sqlComando.ExecuteReader())
                        {
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
                            lblQuincenasConvenio.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiTiempoQuincenas"].ToString().Trim()));

                            DateTime lcConveirteFecha = Convert.ToDateTime(sqlResultado["fdInicioConvenio"].ToString(), CultureInfo.InvariantCulture);
                            lblInicioConvenio.Text = lcConveirteFecha.ToString("dd/MM/yyyy");

                            lcConveirteFecha = Convert.ToDateTime(sqlResultado["fdFinConvenio"].ToString(), CultureInfo.InvariantCulture);
                            lblFinConvenio.Text = lcConveirteFecha.ToString("dd/MM/yyyy");

                            lcConveirteFecha = Convert.ToDateTime(sqlResultado["fdAcuerdo"].ToString(), CultureInfo.InvariantCulture);
                            lblDiadeFirma.Text = lcConveirteFecha.ToString("dd/MM/yyyy");

                            lblCiudadAgencia.Text = sqlResultado["fcCiudad"].ToString().Trim();
                            lblDepartamentoAgencia.Text = sqlResultado["fcDepartamento"].ToString().Trim();

                            lblCodigoDeBarra.Text = "Codigo:" + sqlResultado["fcCodigoConvenio"].ToString().Trim();
                        }
                    } // using cmd
                }
                catch (Exception ex)
                {
                    lblCodigoDeBarra.Text = ex.Message;
                }
            } // using connection
        }
        /* FIN de captura de parametros y desencriptado de cadena */
    }
}