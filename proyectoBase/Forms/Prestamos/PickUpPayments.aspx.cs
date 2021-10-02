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


public partial class Prestamos_PickUpPayments : System.Web.UI.Page
{
    private string pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        var lcURL = Request.Url.ToString();
        var liParamStart = lcURL.IndexOf("?");

        string lcParametros;
        if (liParamStart > 0)
            lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
        else
            lcParametros = string.Empty;

        if (lcParametros != string.Empty)
        {
            pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
            var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        }

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

            lcSQLInstruccion = "exec sp_PickUpPayments_Lista " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario.Trim();
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            gvPickUpPayments.DataSource = sqlResultado;
            gvPickUpPayments.DataBind();
            sqlComando.Dispose();
            sqlConexion.Close();
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

    protected void gvMisPrestamos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        int index = Convert.ToInt32(e.CommandArgument);
        DataKey dkIDSecuencia = gvPickUpPayments.DataKeys[index];
        //PanelErrores.Visible = true;
        //lblMensaje.Text = dkLlaveIdentidadCliente.Value.ToString();
        try
        {
            //string lcScript = "window.open('ImpresionFacturaAbonos.aspx?" + DSC.Encriptar(pcParametros.Trim()) + "',\"_blank\",\"toolbar=0,location=0,menubar=0,width=700,resizable=0\")";
            string lcScript = "window.open('Prestamo_Ficha.aspx?" + DSC.Encriptar("SID=" + pcIDSesion + "&usr=" + pcIDUsuario + "&Pre=" + dkIDSecuencia.Value.ToString().Trim() + "&IDApp=" + pcIDApp) + "',\"_self\",\"toolbar=0,location=0,menubar=0,width=700,resizable=0\")";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
            /*Response.Write("<script>");
            Response.Write(lcScript);
            Response.Write("</script>");*/

            //Page.Response.Redirect(Page.Request.Url.ToString(), true);

        }
        catch (Exception ex)
        {
            PanelErrores.Visible = true;
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
        }
    }

    protected void btnAgregarPickUpPayment_Click(object sender, EventArgs e)
    {
        PanelAgregarPickUpPayment.Visible = true;
    }

    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        if(Convert.ToDouble(txtMontoDownPayment.Text)<=0)
        {
            return;
        }

        PanelAgregarPickUpPayment.Visible = false;
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
            lcSQLInstruccion = "sp_PickUpPayments_Agregar ";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
            sqlComando.Parameters.AddWithValue("@pcIdentificacion", txtIdentificacion.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pnValorDownPayment", txtMontoDownPayment.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pdFechaVencimiento", txtFechaVencimiento.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcComentarios", txtComentarios.Text.Trim());
            sqlComando.ExecuteNonQuery();

            PanelAgregarPickUpPayment.Visible = false;
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

    protected void btnCerrar_Click(object sender, EventArgs e)
    {
        PanelAgregarPickUpPayment.Visible = false;
    }

    protected void gvPickUpPayments_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void btnBuscarCliente_Click(object sender, EventArgs e)
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

            lcSQLInstruccion = "exec CoreAnalitico.dbo.sp_info_ConsultaEjecutivos " + pcIDSesion + "," + pcIDApp + ",'" + txtIdentificacion.Text.Trim() + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            if (sqlResultado.Read())
            {
                txtNombreCliente.Text = sqlResultado["fcPrimerNombre"].ToString() + " " + sqlResultado["fcSegundoNombre"].ToString() + " " + sqlResultado["fcPrimerApellido"].ToString() + " " + sqlResultado["fcSegundoApellido"].ToString();
                btnRegistrar.Enabled = true;
            }
            else
            {
                btnRegistrar.Enabled = false;
            }
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
}