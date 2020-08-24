using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public partial class Aval_CondicionamientoRegistrarAval : System.Web.UI.Page
{
    private string pcIDSesion = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();

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
                string NombreCliente = String.Empty;
                lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                int IDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                string IDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
                int IDCliente = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("cltID"));
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

                SqlConnection sqlConexion = null;
                string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
                sqlConexion = new SqlConnection(sqlConnectionString);
                SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Listar", sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@fiIDCliente", IDCliente);
                sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUsuario);
                sqlConexion.Open();
                SqlDataReader reader = sqlComando.ExecuteReader();
                
                while (reader.Read())
                {
                    NombreCliente = (string)reader["fcPrimerNombreCliente"] + " " + (string)reader["fcSegundoNombreCliente"] + " " + (string)reader["fcPrimerApellidoCliente"] + " " + (string)reader["fcSegundoApellidoCliente"];
                }

                if (sqlConexion != null)
                {
                    if (sqlConexion.State == ConnectionState.Open)
                        sqlConexion.Close();
                }
                if (reader != null)
                {
                    reader.Close();
                }
                lblIDSolicitud.Text = IDSolicitud;
                lblNombreCliente.Text = NombreCliente;
            }
        }

    }
}