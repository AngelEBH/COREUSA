using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public partial class PortalWebValidarDeposito : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDCliente = "";
    public string pcDocumento = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

                if (lcParametros != string.Empty)
                {
                    var lcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDCliente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDCliente");
                    pcDocumento = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("COD");


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
                    var url = string.Empty;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            url = sqlResultado["fcURL"].ToString() + ((sqlResultado["fiTipoDocumento"].ToString() != "8" && sqlResultado["fiTipoDocumento"].ToString() != "9") ? "" : ".jpg");
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