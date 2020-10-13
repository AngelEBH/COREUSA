using adminfiles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Garantia_Registrar : System.Web.UI.Page
{
    private string pcIDApp = "";
    private string pcIDSesion = "";
    private string pcIDUsuario = "";
    private string pcIDSolicitud = "";
    private static DSCore.DataCrypt DSC;
    public List<SeccionGarantia_ViewModel> Documentos_Secciones_Garantia;

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        /* Captura de parámetros encriptados */
        if (!IsPostBack && type == null)
        {
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            DSC = new DSCore.DataCrypt();
            Documentos_Secciones_Garantia = new List<SeccionGarantia_ViewModel>();

            string lcParametros;
            if (liParamStart > 0)
                lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
            else
                lcParametros = String.Empty;

            if (lcParametros != String.Empty)
            {
                var pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);                
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                pcIDSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSol") ?? "0";
            }

            LlenarListas();
        }

        /* Guardar documentos de la solicitud */
        if (type != null || Request.HttpMethod == "POST")
        {
            Session["tipoDoc"] = Convert.ToInt32(Request.QueryString["doc"]); ;
            var uploadDir = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";

            var fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
                { "limit", 1 },
                { "title", "auto" },
                { "uploadDir", uploadDir }
            });

            switch (type)
            {
                case "upload":
                    var data = fileUploader.Upload();

                    if (data["files"].Count == 1)
                        data["files"][0].Remove("file");
                    Response.Write(JsonConvert.SerializeObject(data));
                    break;

                case "remove":
                    string file = Request.Form["file"];

                    if (file != null)
                    {
                        file = FileUploader.FullDirectory(uploadDir) + file;
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                    break;
            }
            Response.End();
        }
        else
        {
            HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
        }
    }

    public void LlenarListas()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDNegociaciones_Guardar_LlenarListas", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            Documentos_Secciones_Garantia.Add(new SeccionGarantia_ViewModel() 
                            {
                                IdSeccionGarantia = (int)sqlResultado["fiIDSeccionGarantia"],
                                DescripcionSeccion = (string)sqlResultado["fcSeccionGarantia"]
                            });
                        }
                        Session["Documentos_Secciones_Garantia"] = Documentos_Secciones_Garantia;
                    }
                }
            }

            ddlUnidadDeMedida.Items.Clear();
            ddlUnidadDeMedida.Items.Add(new ListItem("Kilómetros", "KM"));
            ddlUnidadDeMedida.Items.Add(new ListItem("Millas", "M"));

            ddlTipoDeGarantia.Items.Clear();
            ddlTipoDeGarantia.Items.Add(new ListItem("AUTO", "AUTO"));
            ddlTipoDeGarantia.Items.Add(new ListItem("MOTO", "MOTO"));
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public class SeccionGarantia_ViewModel
    {
        public int IdSeccionGarantia { get; set; }
        public string DescripcionSeccion { get; set; }
    }
}