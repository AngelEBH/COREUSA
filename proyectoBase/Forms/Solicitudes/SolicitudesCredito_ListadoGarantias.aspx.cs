using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class SolicitudesCredito_ListadoGarantias : System.Web.UI.Page
{
    #region Propiedades públicas

    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcNombreUsuario = "";
    public string pcBuzoCorreoUsuario = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    #endregion

    #region Page_Load, Cargar información del usuario, cargar listas para formularios

    private static string ConnectionString { get; set; }

    public SolicitudesCredito_ListadoGarantias()
    {
        ConnectionString = ConnectionString ?? DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

                CargarInformacionUsuario();
                CargarListas();
            }
        }
    }

    private void CargarInformacionUsuario()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreSeguridad.dbo.sp_InformacionUsuario", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            pcNombreUsuario = sqlResultado["fcNombreCorto"].ToString();
                            pcBuzoCorreoUsuario = sqlResultado["fcBuzondeCorreo"].ToString();
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            MostrarMensajeError("Ocurrió un error al cargar la información del usuario: " + ex.Message.ToString());
        }
    }

    private void CargarListas()
    {
        ddlUbicacionInstalacion.Items.Clear();
        ddlUbicacionInstalacion.Items.Add(new ListItem("Seleccione una opción", ""));

        ddlUbicacionInstalacion_Actualizar.Items.Clear();
        ddlUbicacionInstalacion_Actualizar.Items.Add(new ListItem("Seleccione una opción", ""));

        ddlUbicacionInstalacion_Detalle.Items.Clear();
        ddlUbicacionInstalacion_Detalle.Items.Add(new ListItem("Seleccione una opción", ""));

        try
        {
            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_UbicacionesInstalacionGPS", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            ddlUbicacionInstalacion.Items.Add(new ListItem(sqlResultado["fcNombreAgencia"].ToString(), sqlResultado["fiIDAgencia"].ToString()));
                            ddlUbicacionInstalacion_Actualizar.Items.Add(new ListItem(sqlResultado["fcNombreAgencia"].ToString(), sqlResultado["fiIDAgencia"].ToString()));
                            ddlUbicacionInstalacion_Detalle.Items.Add(new ListItem(sqlResultado["fcNombreAgencia"].ToString(), sqlResultado["fiIDAgencia"].ToString()));
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            MostrarMensajeError("Ocurrió un error al cargar parte de la información: " + ex.Message.ToString());
        }
    }

    #endregion

    #region Cargar listados

    [WebMethod]
    public static List<SolicitudesCredito_ListadoGarantias_ViewModel> CargarListado(string dataCrypt)
    {
        var listado = new List<SolicitudesCredito_ListadoGarantias_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantia_Solicitudes_Listado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listado.Add(new SolicitudesCredito_ListadoGarantias_ViewModel()
                            {
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                IdCanal = (int)sqlResultado["fiIDCanal"],
                                Agencia = sqlResultado["fcAgencia"].ToString(),
                                IdProducto = (int)sqlResultado["fiIDTipoProducto"],
                                Producto = sqlResultado["fcProducto"].ToString(),
                                FechaCreacion = (DateTime)sqlResultado["fdFechaCreacionSolicitud"],
                                IdEstadoSolicitud = (byte)sqlResultado["fiEstadoSolicitud"],
                                IdUsuarioAsignado = (int)sqlResultado["fiIDUsuarioAsignado"],
                                UsuarioAsignado = sqlResultado["fcUsuarioAsignado"].ToString(),
                                IdUsuarioCreador = (int)sqlResultado["fiIDUsuarioCrea"],
                                FechaCreado = (DateTime)sqlResultado["fdFechaCrea"],
                                SolicitudActiva = (byte)sqlResultado["fiSolicitudActiva"],
                                IdCliente = (int)sqlResultado["fiIDCliente"],
                                Identidad = sqlResultado["fcIdentidadCliente"].ToString(),
                                NombreCompleto = sqlResultado["fcNombreCliente"].ToString(),
                                IdGarantia = (int)sqlResultado["fiIDGarantia"],
                                VIN = sqlResultado["fcVIN"].ToString(),
                                Marca = sqlResultado["fcMarca"].ToString(),
                                Modelo = sqlResultado["fcModelo"].ToString(),
                                Anio = (int)sqlResultado["fiAnio"],
                                Color = sqlResultado["fcColor"].ToString(),
                                DocumentosSubidos = sqlResultado["fcDocumentos"].ToString(),
                                ValorMercadoGarantia = (decimal)sqlResultado["fnValorDelMercadoGarantia"],
                                ValorPrima = (decimal)sqlResultado["fnValorPrima"],
                                ValorAPrestarGarantia = (decimal)sqlResultado["fnValorAPrestarGarantia"],
                                ValorAFinanciar = (decimal)sqlResultado["fnValorFinanciado"],
                                Moneda = sqlResultado["fcSimboloMoneda"].ToString(),
                                /* Estado asegurado de la garantia */
                                IdEstadoAsegurado = (int)sqlResultado["fiEstadoAsegurado"],
                                IdUsuarioAseguro = (int)sqlResultado["fiIDUsuarioAseguro"],
                                UsuarioAseguro = sqlResultado["fcUsuarioAseguro"].ToString(),
                                FechaAsegurado = (DateTime)sqlResultado["fdFechaAsegurado"],
                                /* Estado revisión fisica de la garantia */
                                EstadoRevisionFisica = sqlResultado["fcEstadoRevisionFisica"].ToString(),
                                EstadoRevisionFisicaClassName = sqlResultado["fcEstadoRevisionFisicaClassName"].ToString(),
                                /* Solicitud de instalacion de GPS */
                                IdAutoGPSInstalacion = (int)sqlResultado["fiIDAutoGPSInstalacion"],
                                IDAgenciaInstalacion = (int)sqlResultado["fiIDAgenciaInstalacion"],
                                FechaInstalacion = (DateTime)sqlResultado["fdFechaInstalacion"],
                                Comentario_Instalacion = sqlResultado["fcComentarioInstalacionGPS"].ToString(),
                                IdEstadoInstalacion = (int)sqlResultado["fiStatusGPSInstalacion"],
                                EstadoActivoSolicitudGPS = (bool)sqlResultado["fiGPSInstalacionActivo"],
                                /* Estado de solicitud de instalación de GPS*/
                                EstadoSolicitudGPS = sqlResultado["fcInstalacionGPSEstatus"].ToString(),
                                EstadoSolicitudGPSClassName = sqlResultado["fcInstalacionGPSEstatusClassName"].ToString()
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return listado;
    }

    [WebMethod]
    public static List<GarantiaSinSolicitud_ViewModel> CargarListadoGarantiasSinSolicitud(string dataCrypt)
    {
        var garantiasSinSolicitud = new List<GarantiaSinSolicitud_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_SinSolicitudListado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            garantiasSinSolicitud.Add(new GarantiaSinSolicitud_ViewModel()
                            {
                                IdGarantia = (int)sqlResultado["fiIDGarantia"],
                                Vendedor = sqlResultado["fcNombreCorto"].ToString(),
                                Agencia = sqlResultado["fcAgencia"].ToString(),
                                VIN = sqlResultado["fcVin"].ToString(),
                                Marca = sqlResultado["fcMarca"].ToString(),
                                Modelo = sqlResultado["fcModelo"].ToString(),
                                Anio = (int)sqlResultado["fiAnio"],
                                TipoDeGarantia = sqlResultado["fcTipoGarantia"].ToString(),
                                TipoDeVehiculo = sqlResultado["fcTipoVehiculo"].ToString(),
                                Comentarios = sqlResultado["fcComentario"].ToString(),
                                FechaCreacion = (DateTime)sqlResultado["fdFechaCreado"],
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return garantiasSinSolicitud;
    }

    #endregion

    #region Administrar solicitudes GPS

    [WebMethod]
    public static bool GuardarSolicitudGPS(SolicitudGPS_ViewModel solicitudGPS, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_SolicitarGPS", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDCREDSolicitud", solicitudGPS.IdSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDCREDGarantia", solicitudGPS.IdGarantia);
                    sqlComando.Parameters.AddWithValue("@pcVIN", solicitudGPS.VIN);
                    sqlComando.Parameters.AddWithValue("@pdFechaInstalacion", solicitudGPS.FechaInstalacion);
                    sqlComando.Parameters.AddWithValue("@pcComentario", solicitudGPS.Comentario_Instalacion);
                    sqlComando.Parameters.AddWithValue("@piIDAgenciaInstalacion", solicitudGPS.IDAgenciaInstalacion);
                    sqlComando.Parameters.AddWithValue("@piIDInventarioGPS", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (!sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                            {
                                resultado = true;

                                var contenidoCorreo = "<table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                                "<tr><th style='text-align:left;'>Cliente:</th> <td>" + solicitudGPS.NombreCliente + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Identidad:</th> <td>" + solicitudGPS.IdentidadCliente + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Fecha de instalación:</th> <td>" + solicitudGPS.FechaInstalacion.ToString("MM/dd/yyyy hh:mm tt") + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Ubicación:</th> <td>" + solicitudGPS.AgenciaInstalacion + "</td></tr>" +
                                "<tr><th style='text-align:left;'>VIN:</th> <td>" + solicitudGPS.VIN + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Marca:</th> <td>" + solicitudGPS.Marca + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Modelo:</th> <td>" + solicitudGPS.Modelo + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Año:</th> <td>" + solicitudGPS.Anio + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Solicitado por:</th> <td>" + solicitudGPS.NombreUsuario + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Comentario:</th> <td>" + solicitudGPS.Comentario_Instalacion + "</td></tr>" +
                                "</table>";

                                EnviarCorreo("Nueva solicitud de GPS", "Nueva solicitud de GPS", "Datos", contenidoCorreo, solicitudGPS.CorreoUsuario);
                            }
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    [WebMethod]
    public static SolicitudGPS_ViewModel CargarInformacionSolicitudGPS(int idSolicitud, int idGarantia, string dataCrypt)
    {
        SolicitudGPS_ViewModel solicitudGPS = null;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_DetalleSolicitudGPS", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDGarantia", idGarantia);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            solicitudGPS = new SolicitudGPS_ViewModel()
                            {
                                IdAutoGPSInstalacion = (int)sqlResultado["fiIDAutoGPSInstalacion"],
                                IdSolicitud = (int)sqlResultado["fiIDCREDSolicitud"],
                                IdGarantia = (int)sqlResultado["fiIDCREDGarantia"],
                                VIN = sqlResultado["fcVIN"].ToString(),
                                FechaInstalacion = (DateTime)sqlResultado["fdFechaInstalacion"],
                                IDAgenciaInstalacion = (int)sqlResultado["fiIDAgenciaInstalacion"],
                                Comentario_Instalacion = sqlResultado["fcComentario"].ToString(),
                                IdEstadoInstalacion = (int)sqlResultado["fiStatusGPSInstalacion"],
                                EstadoInstalacionGPS = sqlResultado["fcStatusGPSInstalacion"].ToString(),
                                EstadoInstalacionClassName = sqlResultado["fcStatusClassName"].ToString(),
                                EstadoActivoSolicitudGPS = (bool)sqlResultado["fiGPSInstalacionActivo"]
                            };
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            solicitudGPS = null;
        }
        return solicitudGPS;
    }

    [WebMethod]
    public static bool ActualizarSolicitudGPS(SolicitudGPS_ViewModel solicitudGPS, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_ActualizarSolicitudGPS", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", solicitudGPS.IdAutoGPSInstalacion);
                    sqlComando.Parameters.AddWithValue("@piIDCREDSolicitud", solicitudGPS.IdSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDCREDGarantia", solicitudGPS.IdGarantia);
                    sqlComando.Parameters.AddWithValue("@pcVIN", solicitudGPS.VIN);
                    sqlComando.Parameters.AddWithValue("@pdFechaInstalacion", solicitudGPS.FechaInstalacion);
                    sqlComando.Parameters.AddWithValue("@pcComentario", solicitudGPS.Comentario_Instalacion);
                    sqlComando.Parameters.AddWithValue("@piIDAgenciaInstalacion", solicitudGPS.IDAgenciaInstalacion);
                    sqlComando.Parameters.AddWithValue("@piIDInventarioGPS", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (!sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                            {
                                resultado = true;

                                var contenidoCorreo = "<table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                                "<tr><th style='text-align:left;'>Cliente:</th> <td>" + solicitudGPS.NombreCliente + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Identidad:</th> <td>" + solicitudGPS.IdentidadCliente + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Fecha de instalación:</th> <td>" + solicitudGPS.FechaInstalacion.ToString("MM/dd/yyyy hh:mm tt") + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Ubicación:</th> <td>" + solicitudGPS.AgenciaInstalacion + "</td></tr>" +
                                "<tr><th style='text-align:left;'>VIN:</th> <td>" + solicitudGPS.VIN + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Marca:</th> <td>" + solicitudGPS.Marca + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Modelo:</th> <td>" + solicitudGPS.Modelo + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Año:</th> <td>" + solicitudGPS.Anio + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Solicitado por:</th> <td>" + solicitudGPS.NombreUsuario + "</td></tr>" +
                                "<tr><th style='text-align:left;'>Comentario:</th> <td>" + solicitudGPS.Comentario_Instalacion + "</td></tr>" +
                                "</table>";

                                EnviarCorreo("Actualización de solicitud de GPS", "Actualización de solicitud de GPS", "Nuevos datos", contenidoCorreo, solicitudGPS.CorreoUsuario);
                            }
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    #endregion

    #region Instalacion GPS

    [WebMethod]
    public static InstalacionGPS_ViewModel CargarInstalacionGPS(int idAutoInstalacionGPS, string dataCrypt)
    {
        InstalacionGPS_ViewModel instalacionGPS = null;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("AutoLoan.dbo.sp_AutoGPS_Instalacion_GetById", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", idAutoInstalacionGPS);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            instalacionGPS = new InstalacionGPS_ViewModel()
                            {
                                IMEI = sqlResultado["fcIMEI"].ToString(),
                                Serie = sqlResultado["fcSerie"].ToString(),
                                Modelo = sqlResultado["fcGPSModel"].ToString(),
                                Compania = sqlResultado["fcGPSCompania"].ToString(),
                                ConRelay = (bool)sqlResultado["fiConRelay"] == true ? "Con Relay" : "Sin Relay",
                                ComentarioUbicacion = sqlResultado["fcDescripcionUbicacion"].ToString(),
                                ObservacionesInstalacion = sqlResultado["fcObservacionesInstalacion"].ToString(),
                            };
                        }
                    } // using sqlResultado
                } // using sqlComando


                using (var sqlComando = new SqlCommand("AutoLoan.dbo.sp_AutoGPS_Instalacion_Fotografias_GetByIDAutoGPSInstalacion", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", idAutoInstalacionGPS);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            instalacionGPS.Fotos.Add(new FotosInstalacionGPS()
                            {
                                NombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                Extension = sqlResultado["fcExtension"].ToString(),
                                RutaArchivo = sqlResultado["fcRutaArchivo"].ToString(),
                                URLArchivo = sqlResultado["fcURL"].ToString(),
                                IdTipoDocumento = (int)sqlResultado["fiIDFotografia"],
                                DescripcionTipoDocumento = sqlResultado["fcDescripcionFotografia"].ToString(),
                                ArchivoActivo = (byte)sqlResultado["fiActivo"]
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            instalacionGPS = null;
        }
        return instalacionGPS;
    }

    #endregion

    #region Revisiones de la garantía

    [WebMethod]
    public static List<Garantia_Revision_ViewModel> CargarRevisionesGarantia(int idGarantia, string dataCrypt)
    {
        var revisionesGarantia = new List<Garantia_Revision_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_Revisiones_ListarPorIdGarantia", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDGarantia", idGarantia);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            revisionesGarantia.Add(new Garantia_Revision_ViewModel()
                            {
                                IdRevision = (int)sqlResultado["fiIDRevision"],
                                NombreRevision = sqlResultado["fcNombreRevision"].ToString(),
                                DescripcionRevision = sqlResultado["fcDescripcionRevision"].ToString(),
                                IdGarantiaRevision = (int)sqlResultado["fiIDGarantiaRevision"],
                                IdGarantia = (int)sqlResultado["fiIDGarantia"],
                                IdSolicitudGPS = (int)sqlResultado["fiIDAutoGPSInstalacion"],
                                IdEstadoRevision = (int)sqlResultado["fiEstadoRevision"],
                                EstadoRevision = sqlResultado["fcEstadoRevision"].ToString(),
                                EstadoRevisionClassName = sqlResultado["fcEstadoRevisionClassName"].ToString(),
                                Observaciones = sqlResultado["fcObservaciones"].ToString(),
                                IdUsuarioValidador = (int)sqlResultado["fiIDUsuarioValidador"],
                                UsuarioValidador = sqlResultado["fcUsuarioValidador"].ToString(),
                                FechaValidacion = (DateTime)sqlResultado["fdFechaValidacion"],
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            revisionesGarantia = null;
        }
        return revisionesGarantia;
    }

    #endregion

    [WebMethod]
    public static List<Documento_ViewModel> CargarDocumentosGarantia(int idGarantia, string dataCrypt)
    {
        return ObtenerDocumentosGarantiaPorIdGarantia(idGarantia, dataCrypt);
    }

    [WebMethod]
    public static List<Documento_ViewModel> CargarDocumentosExpedienteSolicitudGarantia(int idSolicitud, int idGarantia, string dataCrypt)
    {
        var expedienteGaratiaSolicitud = new List<Documento_ViewModel>();
        var documentosSolicitud = ObtenerDocumentosDeLaSolicitudPorIdSolicitud(idSolicitud, dataCrypt);
        var documentosGarantia = ObtenerDocumentosGarantiaPorIdGarantia(idGarantia, dataCrypt);

        documentosSolicitud.ForEach(item =>
        {
            expedienteGaratiaSolicitud.Add(item);
        });

        documentosGarantia.ForEach(item =>
        {
            expedienteGaratiaSolicitud.Add(item);
        });

        return expedienteGaratiaSolicitud;
    }

    #region Obtener documentos de la garantía y de la solicitud

    public static List<Documento_ViewModel> ObtenerDocumentosGarantiaPorIdGarantia(int idGarantia, string dataCrypt)
    {
        var documentosDeLaGarantia = new List<Documento_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_Documentos_ObtenerPorIdGarantia", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDGarantia", idGarantia);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            documentosDeLaGarantia.Add(new Documento_ViewModel()
                            {
                                NombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                Extension = sqlResultado["fcExtension"].ToString(),
                                RutaArchivo = sqlResultado["fcRutaArchivo"].ToString(),
                                URLArchivo = sqlResultado["fcURL"].ToString(),
                                IdTipoDocumento = (int)sqlResultado["fiIDSeccionGarantia"],
                                DescripcionTipoDocumento = sqlResultado["fcSeccionGarantia"].ToString(),
                                ArchivoActivo = (byte)sqlResultado["fiArchivoActivo"]
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            documentosDeLaGarantia = null;
        }
        return documentosDeLaGarantia;
    }

    public static List<Documento_ViewModel> ObtenerDocumentosDeLaSolicitudPorIdSolicitud(int idSolicitud, string dataCrypt)
    {
        var documentosDeLaSolicitud = new List<Documento_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(ConnectionString))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Documentos_ObtenerPorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            documentosDeLaSolicitud.Add(new Documento_ViewModel()
                            {
                                NombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                Extension = sqlResultado["fcTipoArchivo"].ToString(),
                                RutaArchivo = sqlResultado["fcRutaArchivo"].ToString(),
                                URLArchivo = sqlResultado["fcURL"].ToString() + ((int)sqlResultado["fiTipoDocumento"] == 8 || (int)sqlResultado["fiTipoDocumento"] == 9 ? ".jpg" : ""),
                                IdTipoDocumento = (int)sqlResultado["fiTipoDocumento"],
                                DescripcionTipoDocumento = sqlResultado["fcDescripcionTipoDocumento"].ToString(),
                                ArchivoActivo = (byte)sqlResultado["fiArchivoActivo"]
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            documentosDeLaSolicitud = null;
        }
        return documentosDeLaSolicitud;
    }

    #endregion

    #region Métodos utilitarios

    [WebMethod]
    public static string EncriptarParametros(int idSolicitud, int idGarantia, string dataCrypt)
    {
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            string lcParametros = "usr=" + pcIDUsuario + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion + "&IDSOL=" + idSolicitud + "&IDGarantia=" + idGarantia;

            resultado = DSC.Encriptar(lcParametros);
        }
        catch
        {
            resultado = "-1";
        }
        return resultado;
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

    private void MostrarMensajeError(string mensaje)
    {
        lblMensajeError.InnerText = mensaje;
    }

    public static void EnviarCorreo(string pcAsunto, string pcTituloGeneral, string pcSubtitulo, string pcContenidodelMensaje, string buzonCorreoUsuario)
    {
        try
        {
            using (var smtpCliente = new SmtpClient())
            {
                smtpCliente.Host = "mail.miprestadito.com";
                smtpCliente.Port = 587;
                smtpCliente.Credentials = new System.Net.NetworkCredential("systembot@miprestadito.com", "iPwf@p3q");
                smtpCliente.EnableSsl = true;

                using (var pmmMensaje = new MailMessage())
                {
                    pmmMensaje.Subject = pcAsunto;
                    pmmMensaje.From = new MailAddress("systembot@miprestadito.com", "System Bot");
                    //pmmMensaje.To.Add("willian.diaz@miprestadito.com");
                    pmmMensaje.To.Add("sistemas@miprestadito.com");
                    pmmMensaje.CC.Add(buzonCorreoUsuario);
                    pmmMensaje.IsBodyHtml = true;

                    string htmlString = @"<!DOCTYPE html> " +
                    "<html>" +
                    "<body>" +
                    " <div style=\"width: 500px;\">" +
                    " <table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
                    " <tr style=\"height: 30px; background-color:#56396b; font-family: 'Microsoft Tai Le'; font-size: 14px; font-weight: bold; color: white;\">" +
                    " <td style=\"vertical-align: central; text-align:center;\">" + pcTituloGeneral + "</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td>&nbsp;</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td style=\"background-color:whitesmoke; text-align:center;\">" + pcSubtitulo + "</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td>&nbsp;</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td style=\"vertical-align: central;\">" + pcContenidodelMensaje + "</td>" +
                    " </tr>" +
                    " <tr style=\"height: 24px; font-family: 'Microsoft Tai Le'; font-size: 12px; font-weight: bold;\">" +
                    " <td>&nbsp;</td>" +
                    " </tr>" +
                    " <tr style=\"height: 20px; font-family: 'Microsoft Tai Le'; font-size: 12px; text-align:center;\">" +
                    " <td>System Bot Prestadito</td>" +
                    " </tr>" +
                    " </table>" +
                    " </div>" +
                    "</body> " +
                    "</html> ";

                    pmmMensaje.Body = htmlString;

                    ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    smtpCliente.Send(pmmMensaje);
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.SendExcepToDB(ex);
        }
    }

    #endregion
}

#region View Models

/************* Listados *********************************/
public class SolicitudesCredito_ListadoGarantias_ViewModel : Garantia
{
    public int IdSolicitud { get; set; }
    public int IdCanal { get; set; }
    public string Agencia { get; set; }
    public int IdProducto { get; set; }
    public string Producto { get; set; }
    public DateTime FechaCreacion { get; set; }
    public int IdEstadoSolicitud { get; set; }
    public int IdUsuarioAsignado { get; set; }
    public string UsuarioAsignado { get; set; }
    public int IdUsuarioCreador { get; set; }
    public DateTime FechaCreado { get; set; }
    public int SolicitudActiva { get; set; }
    public int IdCliente { get; set; }
    public string Identidad { get; set; }
    public string NombreCompleto { get; set; }
    public int IdGarantia { get; set; }
    public string DocumentosSubidos { get; set; }
    public decimal ValorMercadoGarantia { get; set; }
    public decimal ValorPrima { get; set; }
    public decimal ValorAPrestarGarantia { get; set; }
    public decimal ValorAFinanciar { get; set; }
    public string Moneda { get; set; }

    /* Estado asegurado de la garantía */
    public int IdEstadoAsegurado { get; set; }
    public int IdUsuarioAseguro { get; set; }
    public string UsuarioAseguro { get; set; }
    public DateTime FechaAsegurado { get; set; }

    /* Estado de la revisión física de la garantía */
    public string EstadoRevisionFisica { get; set; }
    public string EstadoRevisionFisicaClassName { get; set; }

    /* Solicitud de instalacion de GPS */
    public int IdAutoGPSInstalacion { get; set; }
    public int IDAgenciaInstalacion { get; set; }
    public DateTime FechaInstalacion { get; set; }
    public string Comentario_Instalacion { get; set; }
    public int IdEstadoInstalacion { get; set; }
    public bool EstadoActivoSolicitudGPS { get; set; }

    /* Estado de solicitud de instalacion de GPS */
    public string EstadoSolicitudGPS { get; set; }
    public string EstadoSolicitudGPSClassName { get; set; }
}

public class GarantiaSinSolicitud_ViewModel : Garantia
{
    public int IdGarantia { get; set; }
    public string Agencia { get; set; }
    public string Vendedor { get; set; }
    public string TipoDeGarantia { get; set; }
    public string TipoDeVehiculo { get; set; }
    public string Comentarios { get; set; }
    public DateTime FechaCreacion { get; set; }
}

/************* Revisiones de la garantía ****************/
public class Garantia_Revision_ViewModel
{
    public int IdRevision { get; set; }
    public string NombreRevision { get; set; }
    public string DescripcionRevision { get; set; }

    public int IdGarantiaRevision { get; set; }
    public int IdGarantia { get; set; }
    public int IdSolicitudGPS { get; set; }

    public int IdEstadoRevision { get; set; }
    public string EstadoRevision { get; set; }
    public string EstadoRevisionClassName { get; set; }
    public string Observaciones { get; set; }

    public int IdUsuarioValidador { get; set; }
    public string UsuarioValidador { get; set; }
    public DateTime FechaValidacion { get; set; }
}

/************* Solicitudes de GPS / Revisión ************/
public class SolicitudGPS_ViewModel : Garantia
{
    public int IdAutoGPSInstalacion { get; set; }
    public int IdSolicitud { get; set; }
    public int IdGarantia { get; set; }

    public DateTime FechaInstalacion { get; set; }
    public int IDAgenciaInstalacion { get; set; }
    public string AgenciaInstalacion { get; set; }
    public string Comentario_Instalacion { get; set; }
    public int IdEstadoInstalacion { get; set; }
    public string EstadoInstalacionGPS { get; set; }
    public string EstadoInstalacionClassName { get; set; }
    public bool EstadoActivoSolicitudGPS { get; set; }

    /* Estas propiedas se utilizan como parte de la información que se envía por correo al guardar o actualizar solucitudes de GPS */
    public string NombreCliente { get; set; }
    public string IdentidadCliente { get; set; }
    public string NombreUsuario { get; set; }
    public string CorreoUsuario { get; set; }
}

public class InstalacionGPS_ViewModel
{
    public string IMEI { get; set; }
    public string Serie { get; set; }
    public string Modelo { get; set; }
    public string Compania { get; set; }
    public string ConRelay { get; set; }
    public string ComentarioUbicacion { get; set; }
    public string ObservacionesInstalacion { get; set; }
    public List<FotosInstalacionGPS> Fotos { get; set; }

    public InstalacionGPS_ViewModel()
    {
        Fotos = new List<FotosInstalacionGPS>();
    }
}

public class FotosInstalacionGPS : Documento_ViewModel
{
    public int IdAutoGPSInstalacion { get; set; }
    public int IdCREDSolicitud { get; set; }
}

/************* Documentos de la solicitud y garantia ****/
public class DocumentosExpedienteSolicitudGarantia_ViewModel
{
    public List<Documento_ViewModel> SolicitudDocumentos { get; set; }
    public List<Documento_ViewModel> GarantiaDocumentos { get; set; }

    public DocumentosExpedienteSolicitudGarantia_ViewModel()
    {
        SolicitudDocumentos = new List<Documento_ViewModel>();
        GarantiaDocumentos = new List<Documento_ViewModel>();
    }
}

/************* Entidades base ***************************/
public abstract class Garantia
{
    public string VIN { get; set; }
    public string Marca { get; set; }
    public string Modelo { get; set; }
    public int Anio { get; set; }
    public string Color { get; set; }
}

public class Documento_ViewModel
{
    public string NombreArchivo { get; set; }
    public int IdTipoDocumento { get; set; }
    public string DescripcionTipoDocumento { get; set; }
    public string Extension { get; set; }
    public string RutaArchivo { get; set; }
    public string URLArchivo { get; set; }
    public string Comentario { get; set; }
    public byte ArchivoActivo { get; set; }
    public int IdUsuarioCreador { get; set; }
    public string UsuarioCreador { get; set; }
    public DateTime FechaCreador { get; set; }
    public string HashTag { get; set; }
}

#endregion
