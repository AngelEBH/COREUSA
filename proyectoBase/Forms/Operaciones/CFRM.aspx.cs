using System;
using System.Web;

public partial class CFRM : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDSolicitud = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = string.Empty;

                if (liParamStart > 0)
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                else
                    lcParametros = string.Empty;

                if (lcParametros != string.Empty)
                {
                    var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("S");

                    ValidarInicioDeSesion();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
            }
        }
    }

    private void ValidarInicioDeSesion()
    {
        try
        {
            if (HttpContext.Current.Session["AccesoAutorizado"] != null)
            {
                if ((int)HttpContext.Current.Session["AccesoAutorizado"] != 1)
                    RedireccionarLogIn();
            }
            else RedireccionarLogIn();
        }
        catch (Exception ex)
        {
            MostrarMensaje("Ocurrió un error al validar la sesión: " + ex.Message.ToString());
        }
    }

    private void RedireccionarLogIn()
    {
        var lcURL = Request.Url.ToString();
        var liParamStart = lcURL.IndexOf("?");
        var lcParametros = string.Empty;

        if (liParamStart > 0)
            lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
        else
            lcParametros = string.Empty;

        Response.Write("<script>window.open('CFRM_IniciarSesion.aspx?" + lcParametros + "','_self')</script>");
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = 0;
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;
            liParamStart = URL.IndexOf("?");

            if (liParamStart > 0)
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = URL.Substring(liParamStart + 1, URL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return lURLDesencriptado;
    }

    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text = mensaje;
    }
}