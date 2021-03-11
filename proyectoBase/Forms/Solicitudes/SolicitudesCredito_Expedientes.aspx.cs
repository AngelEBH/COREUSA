using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

public partial class SolicitudesCredito_Expedientes : System.Web.UI.Page
{
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

                if (lcParametros != string.Empty)
                {
                    var pcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1)).Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr") ?? "0";

                    ValidarPuesto();
                    CargarListados();
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public void ValidarPuesto()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreSeguridad.dbo.sp_InformacionUsuario", sqlConexion))
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
                            var idPuesto = sqlResultado["fiIDPuesto"].ToString();
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public void CargarListados()
    {
        try
        {
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var liParamStart = URL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? URL.Substring(liParamStart, URL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = URL.Substring(liParamStart + 1, URL.Length - (liParamStart + 1));
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
}

#region View Models

public class SolicitudesCredito_Expedientes_ViewModel
{
    public int IdSolicitud { get; set; }
    public int IdCliente { get; set; }
    public string NombreCliente { get; set; }
    public string IdentidadCliente { get; set; }
    public string RtnCliente { get; set; }
    public string Telefono { get; set; }
    public string Producto { get; set; }
    public string TipoDeSolicitud { get; set; }
    public int IdUsuarioAsignado { get; set; }
    public string UsuarioAsignado { get; set; }
    public string Agencia { get; set; }
    public int IdGestorAsignado { get; set; }
    public string GestorAsignado { get; set; }
    public int IdEstadoSolicitud { get; set; }
    public string EstadoSolicitud { get; set; }
    public int IdFondo { get; set; }
    public string Fondo { get; set; }
    public List<SolicitudesCredito_Expedientes_Documentos_Solicitud_ViewModel> Documentos { get; set; }
    public List<SolicitudesCredito_Expedientes_Solicitud_Condicion_ViewModel> Condiciones { get; set; }
    public List<SolicitudesCredito_Expedientes_Cliente_ReferenciaPersonal_ViewModel> ReferenciasPersonales { get; set; }
    public List<SolicitudesCredito_Expedientes_HistorialMantenimiento_ViewModel> HistorialMantenimientos { get; set; }

    public SolicitudesCredito_Expedientes_ViewModel()
    {
        Documentos = new List<SolicitudesCredito_Expedientes_Documentos_Solicitud_ViewModel>();
        Condiciones = new List<SolicitudesCredito_Expedientes_Solicitud_Condicion_ViewModel>();
        ReferenciasPersonales = new List<SolicitudesCredito_Expedientes_Cliente_ReferenciaPersonal_ViewModel>();
        HistorialMantenimientos = new List<SolicitudesCredito_Expedientes_HistorialMantenimiento_ViewModel>();
    }
}

public class SolicitudesCredito_Expedientes_Documentos_Solicitud_ViewModel
{
    public int IdSolicitudDocumento { get; set; }
    public int IdSolicitud { get; set; }
    public string NombreArchivo { get; set; }
    public int IdTipoDocumento { get; set; }
    public string DescripcionTipoDocumento { get; set; }
    public string Extension { get; set; }
    public string RutaArchivo { get; set; }
    public string URLArchivo { get; set; }
    public byte ArchivoActivo { get; set; }
}

public class SolicitudesCredito_Expedientes_HistorialMantenimiento_ViewModel
{
    public int IdHistorialMantenimiento { get; set; }
    public int IdSolicitud { get; set; }
    public int IdUsuario { get; set; }
    public string NombreUsuario { get; set; }
    public string AgenciaUsuario { get; set; }
    public DateTime FechaMantenimiento { get; set; }
    public string Observaciones { get; set; }
    public int EstadoMantenimiento { get; set; }
}

public class SolicitudesCredito_Expedientes_Solicitud_Condicion_ViewModel
{
    public int IdSolicitudCondicion { get; set; }
    public int IdCondicion { get; set; }
    public string Condicion { get; set; }
    public string DescripcionCondicion { get; set; }
    public int IdSolicitud { get; set; }
    public string ComentarioAdicional { get; set; }
    public bool EstadoCondicion { get; set; }
}

public class SolicitudesCredito_Expedientes_Cliente_ReferenciaPersonal_ViewModel
{
    public int IdReferencia { get; set; }
    public int IdCliente { get; set; }
    public int IdSolicitud { get; set; }
    public string NombreCompleto { get; set; }
    public string LugarTrabajo { get; set; }
    public int IdTiempoDeConocer { get; set; }
    public string TiempoDeConocer { get; set; }
    public string TelefonoReferencia { get; set; }
    public int IdParentescoReferencia { get; set; }
    public string DescripcionParentesco { get; set; }
    public bool ReferenciaActivo { get; set; }
    public string RazonInactivo { get; set; }
    public string ComentarioDeptoCredito { get; set; }
    public int AnalistaComentario { get; set; }
}

#endregion