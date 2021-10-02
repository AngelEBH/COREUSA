using System;
using System.Web;

namespace proyectoBase.Forms.Aval
{
    public partial class Aval_CondicionamientoActualizarAval : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                HttpContext.Current.Session["PrecalificandoAval"] = true;

                /* CAPTURA Y DESENCRIPTADO DE PARAMETROS DE LA URL */
                DSCore.DataCrypt DSC = new DSCore.DataCrypt();
                string lcURL = "";
                int liParamStart = 0;
                string lcParametros = "";
                string lcEncriptado = "";
                string lcParametroDesencriptado = "";
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
                    int IDCliente = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("cltID"));
                    //PENDIENTE CAMBIAR POR AVAL
                    HttpContext.Current.Session["IDCliente"] = IDCliente;
                }
            }
        }
    }

}