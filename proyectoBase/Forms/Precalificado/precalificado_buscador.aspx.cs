using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Creditos_precalificado : System.Web.UI.Page
{
    private string pcIDSesion = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIdentidad = "";
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
            lcEncriptado = lcEncriptado.Replace("%2f", "/");
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            pcIdentidad = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
        }
        /* FIN de captura de parametros y desencriptado de cadena */

        if (String.IsNullOrEmpty(pcIDSesion))
        {
            pcIDSesion = "1";
        }

        if (ddlCiudadResidencia.Items.Count == 0)
        {
            LlenarListas();
            ddlOrigenIngreso.Items.Add("");
            ddlOrigenIngreso.Items.Add("Asalariado");
            ddlOrigenIngreso.Items.Add("Comerciante");
        }

        if (!String.IsNullOrEmpty(pcIdentidad))
        {
            txtNombreCliente.Visible = true;
           

            SqlConnection sqlConexion = null;
            SqlDataReader sqlResultado = null;
            SqlCommand sqlComando = null;
            sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

            sqlConexion.Open();

            lcSQLInstruccion = "exec CoreFinanciero.dbo.sp_ConsultaRNP '" + pcIdentidad + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();

            if (sqlResultado.Read())
            {
                txtNombreCliente.Text = sqlResultado["fcNombreCompleto"].ToString();
                txtIdentidad.Text = pcIdentidad.Trim();
            }
            sqlConexion.Close();
            sqlConexion.Dispose();
        }
    }


    protected void btnConsultarCliente_Click(object sender, EventArgs e)
    {

        Funciones.FuncionesComunes fnFunciones = new Funciones.FuncionesComunes();
        int liValidarNumero = 0;
        float liValidarDecimal = 0;
        bool liEsNumerico = false;

        //lblMensaje.Text = ddlProducto.SelectedItem.ToString();

        if (string.IsNullOrEmpty(ddlCiudadResidencia.SelectedValue))
        {
            lblMensaje.Text = "Seleccione una ciudad.";
            return;
        }

        if (string.IsNullOrEmpty(ddlOrigenIngreso.SelectedValue))
        {
            lblMensaje.Text = "Seleccione un tipo de origen de ingresos.";
            return;
        }

        /* Validamos si el ingreso es numerico */
        liEsNumerico = float.TryParse(txtIngresos.Text.Trim(), out liValidarDecimal);
        if (!liEsNumerico)
        {
            lblMensaje.Text = "Los ingresos deben ser numerico valido.";
            return;
        }

        txtIdentidad.Text = txtIdentidad.Text.Replace("-", "");
        liEsNumerico = int.TryParse(txtIdentidad.Text.Trim(), out liValidarNumero);
        if (string.IsNullOrEmpty(txtIdentidad.Text) || txtIdentidad.Text.ToString().Length != 13 || liEsNumerico)
        {
            lblMensaje.Text = "Ingrese una identidad valida.";
            return;
        }

        /* Validamos si el ingreso es numerico */
        liEsNumerico = float.TryParse(txtIngresos.Text.Trim(), out liValidarDecimal);
        if (!liEsNumerico)
        {
            lblMensaje.Text = "Los ingresos deben ser numerico valido.";
            return;
        }

        /* Validamos si el ingreso es numerico */
        txtTelefono.Text = txtTelefono.Text.Replace("-", "");
        liEsNumerico = int.TryParse(txtTelefono.Text.Trim(), out liValidarNumero);
        /*if (!liEsNumerico || string.IsNullOrEmpty(txtTelefono.Text) || txtTelefono.Text.ToString().Length != 8)
        {
            lblMensaje.Text = "Ingrese un numero de telefono valido.";
            return;
        }*/

        if (!liEsNumerico)
        {
            lblMensaje.Text = "Ingrese un numero de telefono valido. 1";
            return;
        }

        if (string.IsNullOrEmpty(txtTelefono.Text))
        {
            lblMensaje.Text = "Ingrese un numero de telefono valido. 2";
            return;
        }

        if (txtTelefono.Text.ToString().Trim().Length != 8)
        {
            lblMensaje.Text = "Ingrese un numero de telefono valido. 3";
            return;
        }
        //PanelBuscador.Visible = true;
        //PanelDatosCliente.Visible = false;

        /* Iniciamos todos los procesos de busqueda de datos */
        btnConsultarCliente.Enabled = false;
        btnConsultarCliente.ForeColor = System.Drawing.Color.Gray;
        btnConsultarCliente.BackColor = System.Drawing.Color.LightGray;

        /*****************************************************************************/
        /*  Verificamos primero si el cliente ya fue consultado recientemente o no   */
        string lcParametroEncriptado = "";
        string lcParametros = "";
        string lcScript = "";
        string lcIngresosPlanos = txtIngresos.Text.Replace(",", "");
        /* Realizamos la busqueda del cliente para ver si existe */
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

        //@pcCiudadRedicencia VarChar(30), @pcTipoOcupacion VarChar(20)
        sqlConexion.Open();

        string lcIDProducto = "";

        lcSQLInstruccion = "exec CoreFinanciero.dbo.sp_CatalogoObtenerID '" + ddlProducto.SelectedItem + "'";
        sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
        sqlComando.CommandType = CommandType.Text;
        sqlResultado = sqlComando.ExecuteReader();

        sqlResultado.Read();
        lcIDProducto = sqlResultado["fiIDProducto"].ToString();

        lcParametros = pcIDSesion + "," + pcIDApp + "," + pcIDUsuario + ",'" + txtIdentidad.Text.Trim() + "'," + lcIDProducto + "," + lcIngresosPlanos + ",'" + txtTelefono.Text.Trim() + "','" +
        ddlCiudadResidencia.SelectedValue.ToString().Trim() + "','" + ddlOrigenIngreso.SelectedValue.ToString().Trim() + "'";


        lcSQLInstruccion = "exec CoreAnalitico.dbo.sp_info_ClienteActualizado " + lcParametros;
        sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
        sqlComando.CommandType = CommandType.Text;
        sqlResultado = sqlComando.ExecuteReader();

        sqlResultado.Read();

        if (sqlResultado["fiExiste"].ToString() != "0")
        {

            string lcPaginaWeb;
            Funciones.FuncionesComunes lfPaginasWeb = new Funciones.FuncionesComunes();
            lcPaginaWeb = lfPaginasWeb.UsrPaginaWeb(pcIDApp, pcIDUsuario, "3");

            lcParametros = "usr=" + pcIDUsuario.Trim() +
                "&IDApp=" + pcIDApp + "&ID=" + txtIdentidad.Text.Trim();

            lcParametroEncriptado = DSC.Encriptar(lcParametros);
            lcScript = "window.open('" + lcPaginaWeb + "?" + lcParametroEncriptado + "','_self')";
        }
        else
        {
            string lcMaquilaCallCenter = lfValorRadioBoton(rbMaquilaoCallCenterSI);
            string lcCasaPropia = lfValorRadioBoton(rbCasaPropiaSI);
            string lcEsGuardiadeSeguridad = lfValorRadioBoton(rbGuardiaSI);
            string lcAntiguedadRequerida = lfValorRadioBoton(rbAntiguedaddeNegocioSI);
            string lcPermisodeOperacionVigente = lfValorRadioBoton(rbPermisoOperacionSI);

            /*****************************************************************************/
            /*  Invocamos la URL para generar el archivo XML desde el recurso de CONFIAR */
            lcParametros = "usr=" + pcIDUsuario.Trim() +
                "&IDApp="+ pcIDApp.Trim() +
                "&ID=" + txtIdentidad.Text.Trim() +
                "&IDProd=" + lcIDProducto +
                "&INS=" + lcIngresosPlanos +
                "&TEL=" + txtTelefono.Text.Trim() +
                "&IDCity=" + ddlCiudadResidencia.SelectedValue.ToString().Trim() +
                "&OC=" + ddlOrigenIngreso.SelectedValue.ToString().Trim() +
                "&MAQCC=" + lcMaquilaCallCenter.Trim() +
                "&CP=" + lcCasaPropia.Trim() +
                "&GS=" + lcEsGuardiadeSeguridad.Trim() +
                "&AR=" + lcAntiguedadRequerida.Trim() +
                "&POV=" + lcPermisodeOperacionVigente.Trim();

            lcParametroEncriptado = DSC.Encriptar(lcParametros);
            lcScript = "window.open('ProcesarPrecalificado.aspx?" + lcParametroEncriptado + "','_self')";
        }
        //Response.Write("<script>");
        Response.Write(lcScript);
        //Response.Write("</script>");
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
    }

    private void LlenarListas()
    {
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {
            sqlConexion.Open();

            lcSQLInstruccion = "exec CoreFinanciero.dbo.sp_CatalogoClientes";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            ddlCiudadResidencia.Items.Add("");
            while (sqlResultado.Read())
            {
                ddlCiudadResidencia.Items.Add(sqlResultado[1].ToString().Trim());
            }

            lcSQLInstruccion = "exec CoreFinanciero.dbo.sp_CatalogoProductos";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            ddlProducto.DataSource = sqlResultado;
            ddlProducto.DataTextField = "fcProducto";
            ddlProducto.DataValueField = "fiIDProducto";
            ddlProducto.DataBind();

            sqlConexion.Close();
            sqlConexion.Dispose();
        }
        catch(Exception ex)
        {
            lblMensaje.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }
    }

    protected void ddlOrigenIngreso_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrigenIngreso.SelectedValue == "Asalariado")
        {
            PanelAsalariado.Visible = true;
            PanelComerciante.Visible = false;
        }

        if (ddlOrigenIngreso.SelectedValue == "Comerciante")
        {
            PanelAsalariado.Visible = false;
            PanelComerciante.Visible = true;
        }

        if (ddlOrigenIngreso.SelectedValue == "")
        {
            PanelAsalariado.Visible = false;
            PanelComerciante.Visible = false;
        }
    }

    protected string lfValorRadioBoton(RadioButton prdBoton)
    {
        bool lbSeleccion;
        lbSeleccion = prdBoton.Checked;
        if (lbSeleccion)
        {
            return "1";
        }
        else
        {
            return "0";
        }
    }
}