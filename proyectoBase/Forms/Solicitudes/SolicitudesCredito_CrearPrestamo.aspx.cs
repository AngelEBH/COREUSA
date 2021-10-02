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
using System.Security.Cryptography;
using System.Text;

public partial class Solicitudes_Forms_Solicitudes_SolicitudesCredito_CrearPrestamo : System.Web.UI.Page
{
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";
    private string pcIDSolicitud = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();

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
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
        }
        /* FIN de captura de parametros y desencriptado de cadena */
        lblSolicitud.Text = pcIDSolicitud;

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

            lcSQLInstruccion = "exec sp_CredSolicitud_InformacionParaPrestamo " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario.Trim() + "," + pcIDSolicitud;
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            if (sqlResultado.Read())
            {
                txtIdentificacion.Text = sqlResultado["fcIdentidadCliente"].ToString();
                txtNombreCliente.Text = sqlResultado["fcNombreCliente"].ToString();
                txtIDProducto.Text = sqlResultado["fiIDProducto"].ToString();
                txtProducto.Text = sqlResultado["fcProducto"].ToString();
                txtPlazo.Text = sqlResultado["fiPlazo"].ToString();
                txtCapital.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValorTotalFinanciamiento"].ToString()));
                txtCuota.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCuotaTotal"].ToString()));
                txtInteres.Text = string.Format("{0:#,###0.0000}", Convert.ToDecimal(sqlResultado["fnTasaAnualAplicada"].ToString())) + "%";
            }
            sqlComando.Dispose();
            sqlConexion.Close();
        }
        catch (Exception ex)
        {
            lblMensaje.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
            {
                sqlConexion.Close();
            }
            sqlConexion.Dispose();

        }

    }

    protected void btnGenerarPrestamo_Click(object sender, EventArgs e)
    {
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
            //sp_Prestamo_CrearPrestamo(@piIDUsuario Int, @piIDSolicitud Int, @piIDFrecuenciadePago TinyInt, @piIDPlazo Int)
            lcSQLInstruccion = "exec sp_Prestamo_CrearPrestamo " + pcIDUsuario.Trim() + "," + pcIDSolicitud + "," + ddlFrecuenciadePago.SelectedValue.ToString() + "," + txtPlazo.Text.Trim();
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            if (sqlResultado.Read())
            {
                lblMensajePrestamo.Text = "Se creo el prestamo " + sqlResultado["fcIDPrestamo"].ToString().Trim() + " para la solicitud " + lblSolicitud.Text.Trim() + ".";
            }
            sqlComando.Dispose();
            sqlConexion.Close();
            PanelPrestamoGenerado.Visible = true;
        }
        catch (Exception ex)
        {
            lblMensaje.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
            {
                sqlConexion.Close();
            }
            sqlConexion.Dispose();
        }
    }

    protected void btnCancelar_Click(object sender, EventArgs e)
    {
        string lcScript = "window.open('/CoreFinanciero/Solicitudes/Forms/Solicitudes/SolicitudesCredito_Bandeja.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&IDApp=107") + "','_Clientes_Contenido')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
    }

    protected void btnCerrarVentana_Click(object sender, EventArgs e)
    {
        string lcScript = "window.open('/CoreFinanciero/Solicitudes/Forms/Solicitudes/SolicitudesCredito_Bandeja.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&IDApp=107") + "','_Clientes_Contenido')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
    }
}