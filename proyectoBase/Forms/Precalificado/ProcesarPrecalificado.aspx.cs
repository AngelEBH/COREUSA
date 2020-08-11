using System;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Net;

public partial class Clientes_ProcesarPrecalificado : System.Web.UI.Page
{

    private string pcIDUsuario = "";
    private string pcID = "";
    private string pcIDApp = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private String lcSQLInstruccion = "";
    private String sqlConnectionString = "";
    private String pcEncriptado = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcURL = "";
        int liParamStart = 0;
        string lcParametros = "";
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
            pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            //lcEncriptado = Request.Params["x"].ToString();
            lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        }
        /* FIN de captura de parametros y desencriptado de cadena */
    }

    protected void Page_LoadComplete(object sender, EventArgs e)
    {
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {

            string lcParametrosSP = "";
            sqlConexion.Open();
            lcParametrosSP = "'" + pcID + "'";
            lcSQLInstruccion = "Exec CoreFinanciero.dbo.sp_ConsultaSAF2 " + lcParametrosSP;
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlComando.CommandTimeout = 120;
            sqlResultado = sqlComando.ExecuteReader();
            sqlResultado.Read();

            if (sqlResultado.HasRows)
            {
                string lcScript = "window.open('Precalificado_ClienteInterno.aspx?" + pcEncriptado + "','_self')";
                Response.Write("<script>");
                Response.Write(lcScript);
                Response.Write("</script>");
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            else
            {
                string lcURLCoreFinanciero = "http://172.20.3.150/WSOrion/WSProcesarCliente.aspx?" + pcEncriptado;
                HttpWebRequest request = WebRequest.Create(lcURLCoreFinanciero) as HttpWebRequest;
                request.Accept = "text/xml";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                WebHeaderCollection header = response.Headers;
                var encoding = ASCIIEncoding.ASCII;

                using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                {
                    string responseText = reader.ReadToEnd();
                }
                //Response.Write("Cargando informacion...");

                string lcPaginaWeb;

                Funciones.FuncionesComunes lfPaginasWeb = new Funciones.FuncionesComunes();

                lcPaginaWeb = lfPaginasWeb.UsrPaginaWeb(pcIDApp, pcIDUsuario, "3");

                string lcScript = "window.open('" + lcPaginaWeb + "?" + pcEncriptado + "','_self')";
                Response.Write("<script>");
                Response.Write(lcScript);
                Response.Write("</script>");
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
        }
        catch (Exception ex)
        {
            Response.Write("Se ha encontrado el siguiente error: " + ex.Message);
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }
    }

}