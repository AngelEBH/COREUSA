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

public partial class Prestamos_PrestamosMenu : System.Web.UI.Page
{
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";
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
        }
        /* FIN de captura de parametros y desencriptado de cadena */
    }

    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void btnCrearPrestamos_Click(object sender, EventArgs e)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcScript = "window.open('Prestamo_Activar.aspx?" + DSC.Encriptar("SID=" + pcIDSesion + "&usr=" + pcIDUsuario + "&IDApp=" + pcIDApp) + "','_Clientes_Contenido')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
    }

    protected void btnMisPrestamos_Click(object sender, EventArgs e)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcScript = "window.open('Prestamo_Lista.aspx?" + DSC.Encriptar("SID=" + pcIDSesion + "&usr=" + pcIDUsuario + "&IDApp=" + pcIDApp) + "','_Clientes_Contenido')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
    }

    protected void btnPickUpPayment_Click(object sender, EventArgs e)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcScript = "window.open('PickUpPayments.aspx?" + DSC.Encriptar("SID=" + pcIDSesion + "&usr=" + pcIDUsuario + "&IDApp=" + pcIDApp) + "','_Clientes_Contenido')";
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
    }
}