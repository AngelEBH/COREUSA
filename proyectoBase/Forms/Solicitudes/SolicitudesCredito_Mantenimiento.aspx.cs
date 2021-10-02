using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class SolicitudesCredito_Mantenimiento : System.Web.UI.Page
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
                Uri lURLDesencriptado = null;
                var lcURL = Request.Url.ToString();
                var liParamStart = lcURL.IndexOf("?");
                var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

                if (lcParametros != string.Empty)
                {
                    var pcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1));
                    var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);

                    lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr") ?? "0";
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

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

                            switch (idPuesto)
                            {
                                case "113":
                                    btnReiniciarResolucion.Visible = false;
                                    btnReiniciarAnalisis.Visible = false;
                                    btnSolicitudDocumentos.Visible = false;
                                    btnReasignarVendedor.Visible = false;
                                    btnCondiciones.Visible = false;
                                    btnReiniciarValidacion.Visible = false;
                                    btnReiniciarCampo.Visible = false;
                                    btnResolucionCampo.Visible = false;
                                    btnReiniciarReprogramacion.Visible = false;
                                    btnReasignarGestor.Visible = false;
                                    break;

                                case "104": // jefe de créditos
                                    //btnReasignarGestor.Visible = false;
                                    btnReiniciarReprogramacion.Visible = false;
                                    btnResolucionCampo.Visible = false;
                                    //btnReiniciarCampo.Visible = false;
                                    break;

                                case "103":
                                    btnReiniciarResolucion.Visible = false;
                                    btnReiniciarAnalisis.Visible = false;
                                    btnReasignarVendedor.Visible = false;
                                    btnCondiciones.Visible = false;
                                    btnReiniciarValidacion.Visible = false;
                                    btnReferenciasPersonales.Visible = false;
                                    break;

                                case "101":

                                    break;
                                default:
                                    btnReiniciarResolucion.Visible = false;
                                    btnReiniciarAnalisis.Visible = false;
                                    btnSolicitudDocumentos.Visible = false;
                                    btnReasignarVendedor.Visible = false;
                                    btnCondiciones.Visible = false;
                                    btnReiniciarValidacion.Visible = false;
                                    btnReiniciarCampo.Visible = false;
                                    btnResolucionCampo.Visible = false;
                                    btnReiniciarReprogramacion.Visible = false;
                                    btnReasignarGestor.Visible = false;
                                    btnReferenciasPersonales.Visible = false;
                                    break;
                            } // switch (idPuesto)
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
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitudes_ListadoGestores", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlGestores.Items.Clear();
                        ddlGestores.Items.Add(new ListItem("Seleccionar", ""));

                        if (sqlResultado.HasRows)
                        {
                            while (sqlResultado.Read())
                            {
                                ddlGestores.Items.Add(new ListItem(sqlResultado["fcNombreCorto"].ToString(), sqlResultado["fiIDUsuario"].ToString()));
                            }
                        }
                    }
                } // using sqlComando listado gestores

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_ListadoVendedores", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlVendedores.Items.Clear();
                        ddlVendedores.Items.Add(new ListItem("Seleccionar", ""));

                        if (sqlResultado.HasRows)
                        {
                            while (sqlResultado.Read())
                            {
                                ddlVendedores.Items.Add(new ListItem(sqlResultado["fcNombreCorto"].ToString(), sqlResultado["fiIDUsuario"].ToString()));
                            }
                        }
                    }
                } // using sqlComando listado de vendedores

                using (var sqlComando = new SqlCommand("sp_CredCatalogo_SolicitudEstados", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlCatalogoResoluciones.Items.Clear();
                        ddlCatalogoResoluciones.Items.Add(new ListItem("Seleccionar", ""));

                        if (sqlResultado.HasRows)
                        {
                            while (sqlResultado.Read())
                            {
                                ddlCatalogoResoluciones.Items.Add(new ListItem(sqlResultado["fcEstadoSolicitud"].ToString(), sqlResultado["fiIDEstadoSolicitud"].ToString()));
                            }
                        }
                    }
                } // using sqlComando catalogo estados solicitudes

                using (var sqlComando = new SqlCommand("sp_CREDCatalogo_Parentescos_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDParentesco", 0);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlParentescos.Items.Clear();
                        ddlParentescos.Items.Add(new ListItem("Seleccionar", ""));

                        ddlParentesco_Editar.Items.Clear();
                        ddlParentesco_Editar.Items.Add(new ListItem("Seleccionar", ""));

                        if (sqlResultado.HasRows)
                        {
                            while (sqlResultado.Read())
                            {
                                ddlParentescos.Items.Add(new ListItem(sqlResultado["fcDescripcionParentesco"].ToString(), sqlResultado["fiIDParentesco"].ToString()));
                                ddlParentesco_Editar.Items.Add(new ListItem(sqlResultado["fcDescripcionParentesco"].ToString(), sqlResultado["fiIDParentesco"].ToString()));
                            }
                        }
                    }
                } // using sqlComando catalogo parentescos

                using (var sqlComando = new SqlCommand("sp_CREDCatalogo_TiempoDeConocer_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlTiempoDeConocerReferencia.Items.Clear();
                        ddlTiempoDeConocerReferencia.Items.Add(new ListItem("Seleccionar", ""));

                        ddlTiempoDeConocerReferencia_Editar.Items.Clear();
                        ddlTiempoDeConocerReferencia_Editar.Items.Add(new ListItem("Seleccionar", ""));

                        if (sqlResultado.HasRows)
                        {
                            while (sqlResultado.Read())
                            {
                                ddlTiempoDeConocerReferencia.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeConocer"].ToString()));
                                ddlTiempoDeConocerReferencia_Editar.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeConocer"].ToString()));
                            }
                        }
                    }
                } // using sqlComando catalogo tiempo de conocer

                using (var sqlComando = new SqlCommand("sp_Catalogo_Fondos_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlFondos.Items.Clear();

                        if (sqlResultado.HasRows)
                        {
                            while (sqlResultado.Read())
                            {
                                ddlFondos.Items.Add(new ListItem(sqlResultado["fcRazonSocial"].ToString().ToUpper(), sqlResultado["fiIDFondo"].ToString()));
                            }
                        }
                    }
                } // using sqlComando catalogo tiempo de conocer
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    [WebMethod]
    public static SolicitudesCredito_Mantenimiento_ViewModel CargarInformacion(int idSolicitud, string dataCrypt)
    {
        var informacionDeClienteSolicitud = new SolicitudesCredito_Mantenimiento_ViewModel();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = "1";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_SolicitudClientePorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /****** Informacion de la solicitud ******/
                        while (sqlResultado.Read())
                        {
                            informacionDeClienteSolicitud.IdSolicitud = (int)sqlResultado["fiIDSolicitud"];
                            informacionDeClienteSolicitud.IdCliente = (int)sqlResultado["fiIDCliente"];
                            informacionDeClienteSolicitud.IdEstadoSolicitud = (byte)sqlResultado["fiEstadoSolicitud"];
                            informacionDeClienteSolicitud.EstadoSolicitud = sqlResultado["fcEstadoSolicitud"].ToString();
                            informacionDeClienteSolicitud.Producto = sqlResultado["fcProducto"].ToString();
                            informacionDeClienteSolicitud.TipoDeSolicitud = sqlResultado["fcTipoSolicitud"].ToString();
                            informacionDeClienteSolicitud.IdUsuarioAsignado = (int)sqlResultado["fiIDUsuarioAsignado"];
                            informacionDeClienteSolicitud.UsuarioAsignado = sqlResultado["fcNombreUsuarioAsignado"].ToString();
                            informacionDeClienteSolicitud.IdGestorAsignado = (int)sqlResultado["fiIDGestor"];
                            informacionDeClienteSolicitud.GestorAsignado = sqlResultado["fcNombreGestor"].ToString();
                            informacionDeClienteSolicitud.Agencia = sqlResultado["fcNombreAgencia"].ToString();
                            informacionDeClienteSolicitud.IdFondo = (int)sqlResultado["fiIDFondo"];
                            informacionDeClienteSolicitud.Tasa = sqlResultado["fnTasaAnualAplicada"].ToString();
                            informacionDeClienteSolicitud.Fondo = sqlResultado["fcRazonSocial"].ToString().ToUpper();
                        }

                        /****** Documentos de la solicitud ******/
                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            informacionDeClienteSolicitud.Documentos.Add(new SolicitudesCredito_Mantenimiento_Documentos_Solicitud_ViewModel()
                            {
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                IdSolicitudDocumento = (int)sqlResultado["fiIDSolicitudDocs"],
                                NombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                Extension = sqlResultado["fcTipoArchivo"].ToString(),
                                RutaArchivo = sqlResultado["fcRutaArchivo"].ToString(),
                                URLArchivo = sqlResultado["fcURL"].ToString(),
                                IdTipoDocumento = (int)sqlResultado["fiTipoDocumento"],
                                DescripcionTipoDocumento = sqlResultado["fcDescripcionTipoDocumento"].ToString(),
                                ArchivoActivo = (byte)sqlResultado["fiArchivoActivo"]
                            });
                        }

                        /****** Condicionamientos de la solicitud ******/
                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            informacionDeClienteSolicitud.Condiciones.Add(new SolicitudesCredito_Mantenimiento_Solicitud_Condicion_ViewModel()
                            {

                                IdSolicitudCondicion = (int)sqlResultado["fiIDSolicitudCondicion"],
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                IdCondicion = (int)sqlResultado["fiIDCondicion"],
                                Condicion = sqlResultado["fcCondicion"].ToString(),
                                DescripcionCondicion = sqlResultado["fcDescripcionCondicion"].ToString(),
                                ComentarioAdicional = sqlResultado["fcComentarioAdicional"].ToString(),
                                EstadoCondicion = (bool)sqlResultado["fbEstadoCondicion"]
                            });
                        }

                        /****** Información del cliente ******/
                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            informacionDeClienteSolicitud.NombreCliente = sqlResultado["fcPrimerNombreCliente"].ToString() + " " + sqlResultado["fcSegundoNombreCliente"].ToString() + " " + sqlResultado["fcPrimerApellidoCliente"].ToString() + " " + sqlResultado["fcSegundoApellidoCliente"].ToString();
                            informacionDeClienteSolicitud.IdentidadCliente = sqlResultado["fcIdentidadCliente"].ToString();
                            informacionDeClienteSolicitud.RtnCliente = sqlResultado["fcRTN"].ToString();
                            informacionDeClienteSolicitud.Telefono = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                        }

                        /****** Información laboral ******/
                        sqlResultado.NextResult();

                        /****** Informacion domicilio ******/
                        sqlResultado.NextResult();

                        /****** Informacion del conyugue ******/
                        sqlResultado.NextResult();

                        /****** Referencias de la solicitud ******/
                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            informacionDeClienteSolicitud.ReferenciasPersonales.Add(new SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel()
                            {
                                IdReferencia = (int)sqlResultado["fiIDReferencia"],
                                IdCliente = (int)sqlResultado["fiIDCliente"],
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                NombreCompleto = sqlResultado["fcNombreCompletoReferencia"].ToString(),
                                LugarTrabajo = sqlResultado["fcLugarTrabajoReferencia"].ToString(),
                                TiempoDeConocer = sqlResultado["fcTiempoDeConocer"].ToString(),
                                IdTiempoDeConocer = (short)sqlResultado["fiTiempoConocerReferencia"],
                                TelefonoReferencia = sqlResultado["fcTelefonoReferencia"].ToString(),
                                IdParentescoReferencia = (int)sqlResultado["fiIDParentescoReferencia"],
                                DescripcionParentesco = sqlResultado["fcDescripcionParentesco"].ToString(),
                                ReferenciaActivo = (bool)sqlResultado["fbReferenciaActivo"],
                                RazonInactivo = sqlResultado["fcRazonInactivo"].ToString(),
                                ComentarioDeptoCredito = sqlResultado["fcComentarioDeptoCredito"].ToString(),
                                AnalistaComentario = (int)sqlResultado["fiAnalistaComentario"]
                            });
                        }

                        /****** Historial de mantenimientos de la solicitud ******/
                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            informacionDeClienteSolicitud.HistorialMantenimientos.Add(new SolicitudesCredito_Mantenimiento_HistorialMantenimiento_ViewModel()
                            {
                                IdHistorialMantenimiento = (int)sqlResultado["fiIDHistorialMantenimiento"],
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                IdUsuario = (int)sqlResultado["fiIDUsuario"],
                                NombreUsuario = sqlResultado["fcNombreCorto"].ToString(),
                                AgenciaUsuario = sqlResultado["fcNombreAgencia"].ToString(),
                                FechaMantenimiento = (DateTime)sqlResultado["fdFechaMantenimiento"],
                                Observaciones = sqlResultado["fcObservaciones"].ToString(),
                                EstadoMantenimiento = (int)sqlResultado["fiEstadoMantenimiento"]
                            });
                        }

                    } // using sqlResultado
                } // using sqlComando
            }// using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            informacionDeClienteSolicitud = null;
        }
        return informacionDeClienteSolicitud;
    }

    [WebMethod]
    public static bool ResolucionCampo(int idSolicitud, int resolucionCampo, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_ResolucionCampo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piResolucionInvestigacion", resolucionCampo);
                    sqlComando.Parameters.AddWithValue("@pcObservacionesDeCampo", observaciones.Trim());
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
    public static bool AsignarGestorSolicitud(int idSolicitud, int idGestor, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_ReasignarGestor", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDGestor", idGestor);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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
    public static bool ReasignarVendedorSolicitud(int idSolicitud, int idUsuarioAsignado, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_ReasignarSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDUsuarioAsignado", idUsuarioAsignado);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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
    public static bool AnularCondicion(int idSolicitud, int idSolicitudCondicion, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_AnularCondicion", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitudCondicion", idSolicitudCondicion);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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
    public static bool EliminarDocumento(int idSolicitud, int idSolicitudDocumento, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_EliminarDocumento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitudDocumento", idSolicitudDocumento);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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
    public static bool CambiarResolucionSolicitud(int idSolicitud, int idNuevaResolucion, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_CambiarResolucion", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piNuevaResolucion", idNuevaResolucion);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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
    public static bool ReiniciarCampo(int idSolicitud, bool reiniciarInvestigacionDomicilio, bool reiniciarInvestigacionTrabajo, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_ReiniciarCampo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piReiniciarDomicilio", reiniciarInvestigacionDomicilio);
                    sqlComando.Parameters.AddWithValue("@piReiniciarTrabajo", reiniciarInvestigacionTrabajo);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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
    public static bool ReiniciarReprogramacion(int idSolicitud, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_ReiniciarReprogramacion", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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
    public static bool ReiniciarValidacion(int idSolicitud, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_ReiniciarPasoFinal", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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
    public static bool ReiniciarAnalisis(int idSolicitud, bool reiniciarInfoPersonal, bool reiniciarInfoLaboral, bool reiniciarReferencias, bool reiniciarDocumentacion, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_ReiniciarAnalisis", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piReiniciarInformacionPersonal", reiniciarInfoPersonal);
                    sqlComando.Parameters.AddWithValue("@piReiniciarInformacionLaboral", reiniciarInfoLaboral);
                    sqlComando.Parameters.AddWithValue("@piReiniciarReferenciasPersonales", reiniciarReferencias);
                    sqlComando.Parameters.AddWithValue("@piReiniciarDocumentacion", reiniciarDocumentacion);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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
    public static bool RegistrarReferenciaPersonal(int idSolicitud, int idCliente, SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel referenciaPersonal, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_AgregarReferencia", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDCliente", idCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@pcNombreCompletoReferencia", referenciaPersonal.NombreCompleto.Trim());
                    sqlComando.Parameters.AddWithValue("@pcLugarTrabajoReferencia", referenciaPersonal.LugarTrabajo.Trim());
                    sqlComando.Parameters.AddWithValue("@piTiempoConocerReferencia", referenciaPersonal.IdTiempoDeConocer);
                    sqlComando.Parameters.AddWithValue("@pcTelefonoReferencia", referenciaPersonal.TelefonoReferencia.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDParentescoReferencia", referenciaPersonal.IdParentescoReferencia);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones);
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
    public static bool EliminarReferenciaPersonal(int idSolicitud, int idCliente, int idReferenciaPersonal, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_EliminarReferencia", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDCliente", idCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDReferenciaPersonal", idReferenciaPersonal);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones);
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
    public static bool ActualizarReferenciaPersonal(int idSolicitud, int idCliente, SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel referenciaPersonal, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_ActualizarReferencia", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDReferenciaPersonal", referenciaPersonal.IdReferencia);
                    sqlComando.Parameters.AddWithValue("@piIDCliente", idCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@pcNombreCompletoReferencia", referenciaPersonal.NombreCompleto.Trim());
                    sqlComando.Parameters.AddWithValue("@pcLugarTrabajoReferencia", referenciaPersonal.LugarTrabajo.Trim());
                    sqlComando.Parameters.AddWithValue("@piTiempoConocerReferencia", referenciaPersonal.IdTiempoDeConocer);
                    sqlComando.Parameters.AddWithValue("@pcTelefonoReferencia", referenciaPersonal.TelefonoReferencia.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDParentescoReferencia", referenciaPersonal.IdParentescoReferencia);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones);
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
    public static bool CambiarFondosPrestamo(int idSolicitud, int idFondo, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_CambiarFondos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDFondo", idFondo);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones);
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
    public static bool CambiarTasaPrestamo(int idSolicitud, string Tasa, string observaciones, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Mantenimiento_CambiarTasa", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@pnTasa", Tasa);
                    sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones);
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

public class SolicitudesCredito_Mantenimiento_ViewModel
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
    public string Tasa { get; set; }
    public string Fondo { get; set; }
    public List<SolicitudesCredito_Mantenimiento_Documentos_Solicitud_ViewModel> Documentos { get; set; }
    public List<SolicitudesCredito_Mantenimiento_Solicitud_Condicion_ViewModel> Condiciones { get; set; }
    public List<SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel> ReferenciasPersonales { get; set; }
    public List<SolicitudesCredito_Mantenimiento_HistorialMantenimiento_ViewModel> HistorialMantenimientos { get; set; }

    public SolicitudesCredito_Mantenimiento_ViewModel()
    {
        Documentos = new List<SolicitudesCredito_Mantenimiento_Documentos_Solicitud_ViewModel>();
        Condiciones = new List<SolicitudesCredito_Mantenimiento_Solicitud_Condicion_ViewModel>();
        ReferenciasPersonales = new List<SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel>();
        HistorialMantenimientos = new List<SolicitudesCredito_Mantenimiento_HistorialMantenimiento_ViewModel>();
    }
}

public class SolicitudesCredito_Mantenimiento_Documentos_Solicitud_ViewModel
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

public class SolicitudesCredito_Mantenimiento_HistorialMantenimiento_ViewModel
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

public class SolicitudesCredito_Mantenimiento_Solicitud_Condicion_ViewModel
{
    public int IdSolicitudCondicion { get; set; }
    public int IdCondicion { get; set; }
    public string Condicion { get; set; }
    public string DescripcionCondicion { get; set; }
    public int IdSolicitud { get; set; }
    public string ComentarioAdicional { get; set; }
    public bool EstadoCondicion { get; set; }
}

public class SolicitudesCredito_Mantenimiento_Cliente_ReferenciaPersonal_ViewModel
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