using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Prestamos_Prestamos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcURL = "";
        int liParamStart = 0;
        string lcParametros = "";
        string lcEncriptado = "";
        string lcParametroDesencriptado = "";
        string lcIDUsuario = "";
        string lcIDApp = "";
        string lcIDSesion = "";
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
            lcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            lcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            lcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
        }
        /* FIN de captura de parametros y desencriptado de cadena */

        if (String.IsNullOrEmpty(lcIDSesion))
        {
            lcIDSesion = "1";
        }

        Response.Write("<frameset cols=\"222px,*\" frameborder=\"0\">");
        Response.Write("  <frame src=\"PrestamosMenu.aspx?" + DSC.Encriptar("SID=" + lcIDSesion + "&Usr=" + lcIDUsuario.Trim() + "&IDApp=" + lcIDApp) + "\" NORESIZE SCROLLING=\"NO\" marginheight=\"0\" marginwidth=\"0\" >");
        Response.Write("  <frame name= \"_Clientes_Contenido\" src=\"../frmvacio.aspx\" NORESIZE >");
        Response.Write("</frameset>");
    }
}