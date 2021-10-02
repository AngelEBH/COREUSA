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
using System.Net;
using System.IO;


public partial class Solicitudes_ClienteRegistrarNuevo : System.Web.UI.Page
{

    private string pcIDUsuario = "";
    private string pcID = "";
    private string pcIDApp = "";
    private string pcIDIngresos = "";
    private string pcIDSesion = "1";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private String lcSQLInstruccion = "";
    private String sqlConnectionString = "";
    private String pcPasoenCurso = "";
    private String pcBuscarDatos = "0";

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
            lcEncriptado = lcEncriptado.Replace("%2f", "");
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
        }
        /* FIN de captura de parametros y desencriptado de cadena */

        if (String.IsNullOrEmpty(pcIDSesion))
        {
            pcIDSesion = "1";
        }
        pcIDApp = "107";
        txtIdentidad.Text = pcID;

        SqlConnection sqlConexion = null;
        SqlCommand sqlComando = null;
        SqlDataReader sqlReader = null;

        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {
            sqlConexion.Open();

            lcSQLInstruccion = "CoreAnalitico.dbo.sp_CatalogoProductos";

            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlReader = sqlComando.ExecuteReader();
            while (sqlReader.Read())
            {
                ddlProducto.Items.Add(new ListItem(sqlReader["fcProducto"].ToString(), sqlReader["fiIDProducto"].ToString()));
            }

        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
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

    protected void btnIngresarSolicitud_Click(object sender, EventArgs e)
    {

        SqlConnection sqlConexion = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {

            sqlConexion.Open();

            lcSQLInstruccion = "CoreAnalitico.dbo.sp_Equifax_AltaCliente";

            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;

            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
            sqlComando.Parameters.AddWithValue("@pnIngresos", txtIngresos.Text.Replace(",",""));
            sqlComando.Parameters.AddWithValue("@piOcupacion", 1);
            sqlComando.Parameters.AddWithValue("@pcSubjectSSN", pcID);
            sqlComando.Parameters.AddWithValue("@pcFirstName", txtFirstName.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcMiddleName", txtMiddleName.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcLastName", txtLastName.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcStreetNumber", "");
            sqlComando.Parameters.AddWithValue("@pcStreetName", "");
            sqlComando.Parameters.AddWithValue("@pcCity", "");
            sqlComando.Parameters.AddWithValue("@pcState", "FL");
            sqlComando.Parameters.AddWithValue("@pcPostalCode", "");
            sqlComando.Parameters.AddWithValue("@pcBirthDate", txtBirthDay.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcTelefono", "");
            sqlComando.ExecuteNonQuery();

            string lcParametroEncriptar = "";
            lcParametroEncriptar = "SID=" + pcIDSesion + "&IDApp=" + pcIDApp + "&usr=" + pcIDUsuario + "&ID=" + txtIdentidad.Text.Trim();

            //string lcScript = "window.open('resumen.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_CuerpoPrincipal')";
            string lcScript = "window.open('../Solicitudes/Forms/Solicitudes/SolicitudesCredito_Registrar.aspx?" + DSC.Encriptar(lcParametroEncriptar) + "','_Clientes_Contenido')";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
        }
        catch (Exception ex)
        {
            lblMensaje.Visible = true;
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
}