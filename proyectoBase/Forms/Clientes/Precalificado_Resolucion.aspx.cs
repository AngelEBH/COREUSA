using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class Clientes_Precalificado_Resolucion : System.Web.UI.Page
{

    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcID = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private String lcSQLInstruccion = "";
    private String sqlConnectionString = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcURL = "";
        int liParamStart = 0;
        string lcParametros = "";
        string lcEncriptado = "";
        string lcParametroDesencriptado = "";
        Uri lURLDesencriptado = null;
        HyperLink hlLink = new HyperLink();
        LinkButton btnLink = new LinkButton();

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
            //lcEncriptado = Request.Params["x"].ToString();
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        }
        /* FIN de captura de parametros y desencriptado de cadena */


        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        try
        {

            sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            sqlConexion.Open();

            /* Cargamos formato para el */

            string lcParametrosSP = "";
            lcParametrosSP = "107," + pcIDUsuario + ",'" + pcID + "'";
            lcSQLInstruccion = "exec CoreAnalitico.dbo.sp_info_ConsultaAnalistas " + lcParametrosSP;
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlComando.CommandTimeout = 120;
            sqlResultado = sqlComando.ExecuteReader();
            sqlResultado.Read();

            txtIdentidad.Text = sqlResultado[0].ToString();
            txtNombreCompleto.Text = sqlResultado["fcNombre"].ToString();
            txtTelefonoRegistrado.Text = sqlResultado["fcTelefono"].ToString();
            txtActividadeconomica.Text = sqlResultado["fcOcupacion"].ToString();

            txtIngresos.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnIngresos"].ToString()));
            txtObligaciones.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnTotalObligaciones"].ToString()));
            txtDisponible.Text = "L " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCapacidadDisponible"].ToString()));

            txtResultadoPrecalificado.Text = sqlResultado["fcMensaje"].ToString();

            /* Estados de CNE, SAF, IHSS y CallCenter*/
            if (sqlResultado["fiSAF"].ToString() == "1")
            {
                idA1.BackColor = System.Drawing.Color.White;
                idA2.BackColor = System.Drawing.Color.White;
                lblSAF5.ForeColor = System.Drawing.Color.Black;
                imgSAF.Visible = true;
            }

            if (sqlResultado["fiIHSS"].ToString() == "1")
            {
                idB1.BackColor = System.Drawing.Color.White;
                idB2.BackColor = System.Drawing.Color.White;
                lblIHSS.ForeColor = System.Drawing.Color.Black;
                imgIHSS.Visible = true;
            }

            if (sqlResultado["fiCNE"].ToString() == "1")
            {
                idC1.BackColor = System.Drawing.Color.White;
                idC2.BackColor = System.Drawing.Color.White;
                lblRNP.ForeColor = System.Drawing.Color.Black;
                imgRNP.Visible = true;
            }

            if (sqlResultado["fiCallCenter"].ToString() == "1")
            {
                idD1.BackColor = System.Drawing.Color.White;
                idD2.BackColor = System.Drawing.Color.White;
                lblCallCenter.ForeColor = System.Drawing.Color.Black;
                imgCallCenter.Visible = true;
            }

            if (sqlResultado["fiMoraComunicaciones"].ToString() == "1")
            {
                idE1.BackColor = System.Drawing.Color.White;
                idE2.BackColor = System.Drawing.Color.White;
                lblComunicaciones.ForeColor = System.Drawing.Color.Red;
                imgComunicaciones.Visible = true;
            }

            if (sqlResultado["fiMoraMayor"].ToString() == "1")
            {
                idF1.BackColor = System.Drawing.Color.White;
                idF2.BackColor = System.Drawing.Color.White;
                lblMoraMayor.ForeColor = System.Drawing.Color.Red;
                imgMoraMayor.Visible = true;
            }

            if (sqlResultado["fiMoraMenor"].ToString() == "1")
            {
                idG1.BackColor = System.Drawing.Color.White;
                idG2.BackColor = System.Drawing.Color.White;
                lblMoraMenor.ForeColor = System.Drawing.Color.Red;
                imgMoraMenor.Visible = true;
            }
            if (sqlResultado["fiMoraHistorica"].ToString() == "1")
            {
                idH1.BackColor = System.Drawing.Color.White;
                idH2.BackColor = System.Drawing.Color.White;
                lblMoraHistorica.ForeColor = System.Drawing.Color.Red;
                imgMoraHistorica.Visible = true;
            }

            if (sqlResultado["fiSobregiro"].ToString() == "1")
            {
                idI1.BackColor = System.Drawing.Color.White;
                idI2.BackColor = System.Drawing.Color.White;
                lblSobregiro.ForeColor = System.Drawing.Color.Red;
                imgSobregiro.Visible = true;
            }

            if (sqlResultado["fiScoreBajo"].ToString() == "1")
            {
                idJ1.BackColor = System.Drawing.Color.White;
                idJ2.BackColor = System.Drawing.Color.White;
                lblScoreBajo.ForeColor = System.Drawing.Color.Red;
                imgScoreMenor.Visible = true;
            }

            if (sqlResultado["fiSaldosCastigados"].ToString() == "1")
            {
                idK1.BackColor = System.Drawing.Color.White;
                idK2.BackColor = System.Drawing.Color.White;
                lblSaldosCastig.ForeColor = System.Drawing.Color.Red;
                imgCastigado.Visible = true;
            }

            if (sqlResultado["fiIncobrable"].ToString() == "1")
            {
                idL1.BackColor = System.Drawing.Color.White;
                idL2.BackColor = System.Drawing.Color.White;
                lblIncobIrrecup.ForeColor = System.Drawing.Color.Red;
                imgIncobrable.Visible = true;
            }

            if (sqlResultado["fiIrrecuperable"].ToString() == "1")
            {
                idL1.BackColor = System.Drawing.Color.White;
                idL2.BackColor = System.Drawing.Color.White;
                lblIncobIrrecup.ForeColor = System.Drawing.Color.Red;
                imgCallCenter.Visible = true;
            }

            if (sqlResultado["fiJuridicoLegal"].ToString() == "1")
            {
                idM1.BackColor = System.Drawing.Color.White;
                idM2.BackColor = System.Drawing.Color.White;
                lblJuridicoLegal.ForeColor = System.Drawing.Color.Red;
                imgJuridico.Visible = true;
            }

            CargarIHSS(sqlConexion);


        }
        catch (Exception ex)
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }

    }

    protected void imgbtnGuardar_Click(object sender, EventArgs e)
    {

        SqlConnection sqlConexion = null;
        SqlCommand sqlComando = null;
        SqlDataReader sqlResultado = null;
        try
        {
            string lcCorreoDestino = "";
            sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            sqlConexion.Open();
            string lcTipoSeleccion = "2";

            if (rbAprobado.Checked)
                lcTipoSeleccion = "1";

            //sp_RegistrarAnalisis(@piIDApp TinyInt, @piIDUsuario int, @pcIdentidad VarChar(13), @piResolucion TinyInt, @pcObservaciones VarChar(100))

            lcSQLInstruccion = "exec CoreAnalitico.dbo.sp_RegistrarAnalisis " + pcIDApp + "," + pcIDUsuario + ",'" + pcID + "'," + lcTipoSeleccion + ",'" + txtObservaciones.Text.Trim() + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            sqlResultado.Read();

            lcCorreoDestino = sqlResultado["fcCorreoDestinatario"].ToString().Trim();

            sqlComando.Dispose();
            sqlConexion.Close();

            Correos objCorreos = new Correos();
            objCorreos.lcAsunto = "Cambio de estado de precalificado";
            objCorreos.lcTituloGeneral = "Notificación de cambio de estado";
            objCorreos.lcSubtitulo = "Se le ha modificado el estado de precalificado al cliente: " + txtNombreCompleto.Text.Trim();

            if (lcTipoSeleccion == "1")
            {
                objCorreos.lcContenidodelMensaje = "Nuevo estado: Pre-aprobado";
            }

            if (lcTipoSeleccion == "2")
            {
                objCorreos.lcContenidodelMensaje = "Nuevo estado: Rechazado - " + txtObservaciones.Text;
            }

            objCorreos.AgregarDestinatario(lcCorreoDestino);
            objCorreos.AgregarDestinatario("edwin.aguilar@miprestadito.com");
            objCorreos.EnviarCorreo();

            string lcScript = "window.open('ClientesEstados.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&S=3&T=Clientes_Pre-aprobados") + "','_Clientes_Contenido')";
            Response.Write("<script>");
            Response.Write(lcScript);
            Response.Write("</script>");

        }
        catch(Exception ex)
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }
    }

    protected void imgbtnCancelar_Click(object sender, EventArgs e)
    {
        string lcScript = "window.open('Precalificado_Analista.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&ID=" + txtIdentidad.Text.Trim() + "&IDApp=107") + "','_Clientes_Contenido')";
        Response.Write("<script>");
        Response.Write(lcScript);
        Response.Write("</script>");
    }

    private void CargarIHSS(SqlConnection pSqlConnection)
    {

        string lcSQLInstruccion = "exec CoreAnalitico.dbo.sp_Info_TrabajosIHSS " + pcIDUsuario + ",'" + pcID + "'";
        SqlCommand sqlComando = new SqlCommand(lcSQLInstruccion, pSqlConnection);
        sqlComando.CommandType = CommandType.Text;
        SqlDataReader sqlResultado = sqlComando.ExecuteReader();
        gvIHSS.DataSource = sqlResultado;
        gvIHSS.DataBind();

    }

    protected void rbAprobado_CheckedChanged(object sender, EventArgs e)
    {
        imgbtnGuardar.Enabled = true;
    }

    protected void rbRechazado_CheckedChanged(object sender, EventArgs e)
    {
        imgbtnGuardar.Enabled = true;
    }
}