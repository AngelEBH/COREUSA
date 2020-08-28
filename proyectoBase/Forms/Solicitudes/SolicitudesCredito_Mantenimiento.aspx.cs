using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;

public partial class SolicitudesCredito_Mantenimiento : System.Web.UI.Page
{
    string pcIDUsuario = "";
    string pcIDApp = "";
    string pcSesionID = "1";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            try
            {
                string lcURL = string.Empty;
                int liParamStart = 0;
                string lcParametros = string.Empty;
                string lcParametroDesencriptado = string.Empty;
                string pcEncriptado = string.Empty;
                Uri lURLDesencriptado = null;
                string idSolicitud = "0";
                lcURL = Request.Url.ToString();
                liParamStart = lcURL.IndexOf("?");

                if (liParamStart > 0)
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                else
                    lcParametros = String.Empty;

                if (lcParametros != String.Empty)
                {
                    pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcSesionID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

                    using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                    {
                        sqlConexion.Open();

                        /* Informacion completa de la solicitud y del cliente */
                        using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_InformacionCompletaClienteSolicitud", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", 64);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
                            sqlComando.Parameters.AddWithValue("@piIDApp", 1);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", 1);

                            using (SqlDataReader reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    var x = reader["fiIDSolicitud"].GetType();
                                }

                                reader.NextResult(); // Documentos de la solicitud

                                List<string> Lista = new List<string>();
                                while (reader.Read())
                                {
                                    Lista.Add(reader["fcNombreArchivo"].ToString()); ;
                                }

                                reader.NextResult(); // Condicionamientos de la solicitud

                                List<string> Condiciones = new List<string>();
                                while (reader.Read())
                                {
                                    Condiciones.Add(reader["fcDescripcionCondicion"].ToString()); ;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
    }

    [WebMethod]
    public static List<MantenimientoDocumentosViewModel> CargarDocumentos(string dataCrypt)
    {
        List<MantenimientoDocumentosViewModel> ListadoDocumentos = new List<MantenimientoDocumentosViewModel>();
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("dbo.sp_CANEX_Solicitud_Documentos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        string fcNombreSocio = "";
                        string fcNombreImagen = "";

                        while (reader.Read())
                        {
                            fcNombreSocio = reader["fcNombreSocio"].ToString();
                            fcNombreImagen = reader["fcNombreImagen"].ToString();

                            //ListadoDocumentos.Add(new MantenimientoDocumentosViewModel()
                            //{
                            //    fiIDSolicitudDocs = (short)reader["fiIDImagen"],
                            //    fcNombreArchivo = (string)reader["fcNombreImagen"],
                            //    URLArchivo = "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + idSolicitud + "/" + fcNombreImagen,
                            //    fiTipoDocumento = (short)reader["fiIDImagen"]
                            //});
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return ListadoDocumentos;
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            int liParamStart = 0;
            string lcParametros = "";
            String pcEncriptado = "";
            liParamStart = URL.IndexOf("?");
            if (liParamStart > 0)
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            else
                lcParametros = String.Empty;

            if (lcParametros != String.Empty)
            {
                pcEncriptado = URL.Substring((liParamStart + 1), URL.Length - (liParamStart + 1));
                string lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return lURLDesencriptado;
    }
}

public class MantenimientoDocumentosViewModel
{
    public int IDSolicitudDocs { get; set; }
    public int IDSolicitud { get; set; }
    public string NombreArchivo { get; set; }
    public int IDTipoDocumento { get; set; }
    public string DescripcionTipoDocumento { get; set; }
    public string Extension { get; set; }
    public string RutaArchivo { get; set; }
    public string URLArchivo { get; set; }
    public byte ArchivoActivo { get; set; }
}