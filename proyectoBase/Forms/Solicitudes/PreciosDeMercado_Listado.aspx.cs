using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;

public partial class PreciosDeMercado_Listado : System.Web.UI.Page
{
    #region Propiedades públicas

    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    #endregion

    #region Page_Load, Cargar información del usuario, cargar listas para formularios

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
                    var pcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1));
                    var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensajeError("Ocurrió un error al cargar, contacte al administrador. (" + ex.Message.ToString() + ")");
        }
    }

    #endregion

    [WebMethod]
    public static List<PrecioDeMercado_ViewModel> CargarListaPreciosDeMercado(string dataCrypt)
    {
        var listado = new List<PrecioDeMercado_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_ListarPreciosDeMercadoActuales", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listado.Add(new PrecioDeMercado_ViewModel()
                            {
                                IdPrecioDeMercado = (int)sqlResultado["fiIDPrecioDeMercado"],
                                IdModeloAnio = (int)sqlResultado["fiIDModeloAnio"],
                                IdMarca = (int)sqlResultado["fiIDMarca"],
                                Marca = sqlResultado["fcMarca"].ToString(),
                                IdModelo = (int)sqlResultado["fiIDModelo"],
                                Modelo = sqlResultado["fcModelo"].ToString(),
                                Version = sqlResultado["fcVersion"].ToString(),
                                IdAnio = (int)sqlResultado["fiIDAnio"],
                                Anio = (int)sqlResultado["fiAnio"],
                                PrecioDeMercado = (decimal)sqlResultado["fnPrecioDeMercado"],
                                UltimaDevaluacion = (decimal)sqlResultado["fnUltimaDevaluacion"],
                                IdUsuarioSolicitante = (int)sqlResultado["fiIDUsuarioSolicitante"],
                                UsuarioSolicitante = sqlResultado["fcUsuarioSolicitante"].ToString(),
                                ComentariosSolicitante = sqlResultado["fcComentariosSolicitante"].ToString(),
                                IdUsuarioAprobador = (int)sqlResultado["fiIDUsuarioAprobador"],
                                UsuarioAprobador = sqlResultado["fcUsuarioAprobador"].ToString(),
                                ComentariosAprobador = sqlResultado["fcComentariosAprobador"].ToString(),
                                FechaInicio = (DateTime)sqlResultado["fdFechaInicio"],
                                FechaFin = (DateTime)sqlResultado["fdFechaFin"],
                                IdEstadoPrecioDeMercado = (int)sqlResultado["fiIDEstadoPrecioDeMercado"],
                                EstadoPrecioDeMercado = sqlResultado["fcEstadoPrecioDeMercado"].ToString(),
                                EstadoPrecioDeMercadoClassName = sqlResultado["fcEstadoPrecioDeMercadoClassName"].ToString(),
                                IdUsuarioCreador = (int)sqlResultado["fiIDUsuarioCreador"],
                                UsuarioCreador = sqlResultado["fcUsuarioCreador"].ToString(),
                                FechaCreado = (DateTime)sqlResultado["fdFechaCreacion"],
                                IdUsuarioUltimaModificacion = (int)sqlResultado["fiIDUsuarioUltimaModificacion"],
                                UsuarioUltimaModificacion = sqlResultado["fcUsuarioUltimaModificacion"].ToString(),
                                FechaUltimaModificacion = (DateTime)sqlResultado["fdFechaUltimaModificacion"],
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
    public static List<HistorialDevaluacionPorModelo_ViewModel> CargarHistorialDevaluacionPorModelo(string dataCrypt)
    {
        var listado = new List<HistorialDevaluacionPorModelo_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_ListarModelos", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listado.Add(new HistorialDevaluacionPorModelo_ViewModel()
                            {
                                IdMarca = (int)sqlResultado["fiIDMarca"],
                                Marca = sqlResultado["fcMarca"].ToString(),
                                IdModelo = (int)sqlResultado["fiIDModelo"],
                                Modelo = sqlResultado["fcModelo"].ToString(),
                                Version = sqlResultado["fcVersion"].ToString(),
                                Anios = ObtenerAniosPorIdModelo((int)sqlResultado["fiIDModelo"], pcIDSesion, pcIDApp, pcIDUsuario)
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
    public static List<PrecioDeMercado_ViewModel> CargarSolicitudesDePreciosDeMercado(string dataCrypt)
    {
        var listado = new List<PrecioDeMercado_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_ListarSolicitudesDePreciosDeMercado", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listado.Add(new PrecioDeMercado_ViewModel()
                            {
                                IdPrecioDeMercado = (int)sqlResultado["fiIDPrecioDeMercado"],
                                IdModeloAnio = (int)sqlResultado["fiIDModeloAnio"],
                                IdMarca = (int)sqlResultado["fiIDMarca"],
                                Marca = sqlResultado["fcMarca"].ToString(),
                                IdModelo = (int)sqlResultado["fiIDModelo"],
                                Modelo = sqlResultado["fcModelo"].ToString(),
                                Version = sqlResultado["fcVersion"].ToString(),
                                IdAnio = (int)sqlResultado["fiIDAnio"],
                                Anio = (int)sqlResultado["fiAnio"],
                                PrecioDeMercado = (decimal)sqlResultado["fnPrecioDeMercado"],
                                IdUsuarioSolicitante = (int)sqlResultado["fiIDUsuarioSolicitante"],
                                UsuarioSolicitante = sqlResultado["fcUsuarioSolicitante"].ToString(),
                                ComentariosSolicitante = sqlResultado["fcComentariosSolicitante"].ToString(),
                                IdUsuarioAprobador = (int)sqlResultado["fiIDUsuarioAprobador"],
                                UsuarioAprobador = sqlResultado["fcUsuarioAprobador"].ToString(),
                                ComentariosAprobador = sqlResultado["fcComentariosAprobador"].ToString(),
                                //FechaInicio = (DateTime)sqlResultado["fdFechaInicio"],
                                //FechaFin = (DateTime)sqlResultado["fdFechaFin"],
                                IdEstadoPrecioDeMercado = (int)sqlResultado["fiIDEstadoPrecioDeMercado"],
                                EstadoPrecioDeMercado = sqlResultado["fcEstadoPrecioDeMercado"].ToString(),
                                EstadoPrecioDeMercadoClassName = sqlResultado["fcEstadoPrecioDeMercadoClassName"].ToString(),
                                IdUsuarioCreador = (int)sqlResultado["fiIDUsuarioCreador"],
                                UsuarioCreador = sqlResultado["fcUsuarioCreador"].ToString(),
                                FechaCreado = (DateTime)sqlResultado["fdFechaCreacion"],
                                IdUsuarioUltimaModificacion = (int)sqlResultado["fiIDUsuarioUltimaModificacion"],
                                UsuarioUltimaModificacion = sqlResultado["fcUsuarioUltimaModificacion"].ToString(),
                                FechaUltimaModificacion = (DateTime)sqlResultado["fdFechaUltimaModificacion"],
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

    public static List<AnioViewModel> ObtenerAniosPorIdModelo(int idModelo, string pcIDSesion, string pcIDApp, string pcIDUsuario)
    {
        var listado = new List<AnioViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Catalogo_Modelos_Anios_ObtenerPorIdModelo", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDModelo", idModelo);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);                    

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listado.Add(new AnioViewModel()
                            {
                                IdModeloAnio = (int)sqlResultado["fiIDModeloAnio"],
                                IdAnio = (int)sqlResultado["fiIDAnio"],
                                Anio = (int)sqlResultado["fiAnio"],
                                HistorialDevaluaciones = ObtenerHistorialDevaluacionesPorIdModeloAnio((int)sqlResultado["fiIDModeloAnio"], pcIDSesion, pcIDApp, pcIDUsuario),
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

    public static List<Devaluacion_ViewModel> ObtenerHistorialDevaluacionesPorIdModeloAnio(int idModeloAnio, string pcIDSesion, string pcIDApp, string pcIDUsuario)
    {
        var listado = new List<Devaluacion_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_ObtenerDevaluacionPorIdModeloAnio", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDModeloAnio", idModeloAnio);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listado.Add(new Devaluacion_ViewModel()
                            {
                                IdPrecioMercado = (int)sqlResultado["fiIDPrecioDeMercado"],
                                FechaInicio = (DateTime)sqlResultado["fdFechaInicio"],
                                FechaFin = (DateTime)sqlResultado["fdFechaFin"],
                                PrecioDeMercado = (decimal)sqlResultado["fnPrecioDeMercado"],
                                UltimaDevaluacion = (decimal)sqlResultado["fnUltimaDevaluacion"],
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
    public static bool CargarSolicitudesDePreciosDeMercado(int idPrecioDeMercado, int idModeloAnio, decimal precio, string comentarios, int idEstado, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_ResolucionSolicitudPrecioDeMercado", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDPrecioDeMercado", idPrecioDeMercado);
                    sqlComando.Parameters.AddWithValue("@piIDModeloAnio", idModeloAnio);
                    sqlComando.Parameters.AddWithValue("@pnPrecioDeMercado", precio);
                    sqlComando.Parameters.AddWithValue("@piIDUsuarioAprobador", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcComentariosAprobador", comentarios);
                    sqlComando.Parameters.AddWithValue("@piIDEstadoPrecioDeMercado", idEstado);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                            if (!sqlResultado["lcResultadoProceso"].ToString().StartsWith("-1"))
                                resultado = true;

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
        lblMensajeError.InnerText += mensaje;
    }

    public static SqlCommand CrearSqlComando(string nombreSP, SqlConnection sqlConexion)
    {
        return new SqlCommand(nombreSP, sqlConexion) { CommandType = CommandType.StoredProcedure, CommandTimeout = 120 };
    }

    #endregion

    #region View Models

    /* Para el listado de precios de mercado */
    public class PrecioDeMercado_ViewModel
    {
        public int IdPrecioDeMercado { get; set; }
        public int IdModeloAnio { get; set; }
        public int IdMarca { get; set; }
        public string Marca { get; set; }
        public int IdModelo { get; set; }
        public string Modelo { get; set; }
        public string Version { get; set; }
        public int IdAnio { get; set; }
        public int Anio { get; set; }

        public decimal PrecioDeMercado { get; set; }
        public decimal UltimaDevaluacion { get; set; }

        public int IdUsuarioSolicitante { get; set; }
        public string UsuarioSolicitante { get; set; }
        public string ComentariosSolicitante { get; set; }

        public int IdUsuarioAprobador { get; set; }
        public string UsuarioAprobador { get; set; }
        public string ComentariosAprobador { get; set; }

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public int IdEstadoPrecioDeMercado { get; set; }
        public string EstadoPrecioDeMercado { get; set; }
        public string EstadoPrecioDeMercadoClassName { get; set; }

        public int IdUsuarioCreador { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime FechaCreado { get; set; }

        public int IdUsuarioUltimaModificacion { get; set; }
        public string UsuarioUltimaModificacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
    }

    /* Para el listado de historial de devaluaciones */
    public class HistorialDevaluacionPorModelo_ViewModel
    {
        public int IdMarca { get; set; }
        public string Marca { get; set; }
        public int IdModelo { get; set; }
        public string Modelo { get; set; }
        public string Version { get; set; }
        public List<AnioViewModel> Anios { get; set; }
        public HistorialDevaluacionPorModelo_ViewModel()
        {
            Anios = new List<AnioViewModel>();
        }
    }

    public class AnioViewModel
    {
        public int IdModeloAnio { get; set; }
        public int IdAnio { get; set; }
        public int Anio { get; set; }
        public List<Devaluacion_ViewModel> HistorialDevaluaciones { get; set; }
        public AnioViewModel()
        {
            HistorialDevaluaciones = new List<Devaluacion_ViewModel>();
        }
    }

    public class Devaluacion_ViewModel
    {
        public int IdPrecioMercado { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal PrecioDeMercado { get; set; }
        public decimal UltimaDevaluacion { get; set; }
    }


    public class SolicitudPrecioDeMercado_ViewModel
    {
        public int IdPrecioDeMercado { get; set; }
        public int IdModeloAnio { get; set; }
        public int IdMarca { get; set; }
        public string Marca { get; set; }
        public int IdModelo { get; set; }
        public string Modelo { get; set; }
        public string Version { get; set; }
        public int IdAnio { get; set; }
        public int Anio { get; set; }
        public decimal PrecioSolicitado { get; set; }
        public int IdUsuarioSolicitante { get; set; }
        public string UsuarioSolicitante { get; set; }
        public DateTime ComentariosSolicitante { get; set; }

        public int IdUsuarioValidador { get; set; }
        public string UsuarioValidador { get; set; }
        public string ComentariosValidador { get; set; }

        public int IdEstadoPrecioDeMercado { get; set; }
        public string EstadoPrecioDeMercado { get; set; }
        public string EstadoPrecioDeMercadoClassName { get; set; }


        public int IdUsuarioCreador { get; set; }
        public string UsuarioCreador { get; set; }
        public DateTime FechaCreado { get; set; }
        public int IdUsuarioUltimaModificacion { get; set; }
        public string UsuarioUltimaModificacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }

    }

    #endregion
}