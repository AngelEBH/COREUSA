using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.HtmlControls;

public partial class GestionCobranzaClienteDocumentos : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDCliente = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
                var lcURL = Request.Url.ToString();
                int liParamStart = lcURL.IndexOf("?");

                string lcParametros;

                if (liParamStart > 0)
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                else
                    lcParametros = string.Empty;

                if (lcParametros != string.Empty)
                {
                    var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcEncriptado = lcEncriptado.Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcIDCliente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDCliente");

                    lblCodigoCliente.Text = pcIDCliente;

                    CargarDocumentos();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
            }
        }
    }

    private void CargarDocumentos()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("SP_SRCClientesDocumentos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fcIDCliente", pcIDCliente);
                    sqlComando.CommandTimeout = 120;

                    HtmlTableRow tRowDocumento = null;
                    var url = string.Empty;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            url = (sqlResultado["fiTipoDocumento"].ToString() != "8" && sqlResultado["fiTipoDocumento"].ToString() != "9") ? sqlResultado["fcURL"].ToString() : sqlResultado["fcURL"].ToString() + ".jpg";

                            tRowDocumento = new HtmlTableRow();
                            tRowDocumento.Cells.Add(new HtmlTableCell() { InnerHtml = "<a href='" + url + "' target='imgbox'>" + sqlResultado["fcDescripcionTipoDocumento"].ToString() + "</a>" });
                            tblDocumentos.Rows.Add(tRowDocumento);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar información: " + ex.Message.ToString());
        }
    }

    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text = mensaje;
    }
}