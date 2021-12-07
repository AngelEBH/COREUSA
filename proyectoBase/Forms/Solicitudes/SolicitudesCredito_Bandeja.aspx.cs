using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;

public partial class SolicitudesCredito_Bandeja : System.Web.UI.Page
{
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    public int FechaDesembolso = 0;
    public static int CantidadDias = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
    }

    [WebMethod]
    public static List<SolicitudesCredito_Bandeja_ViewModel> CargarSolicitudes(string dataCrypt)
    {
        var solicitudes = new List<SolicitudesCredito_Bandeja_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            //var fechaDesembolsoDePrestamo = 0;

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Bandeja", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 240;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            //var EstadoSolicitud = (byte)sqlResultado["fiEstadoSolicitud"];
                            //var fechaDesembolsoDePrestamo = (DateTime)sqlResultado["fcFechaDesembolso"];
                            //var fechaPasoFinal = (DateTime)sqlResultado["fdPasoFinalFin"];
                            //var fechaHoy = DateTime.Now;

                            //if (EstadoSolicitud == 7 || EstadoSolicitud == 4)
                            //{
                            //     CantidadDias = (fechaPasoFinal - fechaDesembolsoDePrestamo ).Days;
                            //}
                            //else {
                            //     CantidadDias = (fechaHoy - fechaDesembolsoDePrestamo).Days;
                            //}

                            var fiTiempoIncompleto = (int)sqlResultado["fiTiempoIncompleto"];


                            solicitudes.Add(new SolicitudesCredito_Bandeja_ViewModel()
                            {
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                FechaCreacionSolicitud = (DateTime)sqlResultado["fdFechaCreacionSolicitud"],
                                IdProducto = (int)sqlResultado["fiIDTipoProducto"],
                                Producto = (string)sqlResultado["fcProducto"],
                                RequiereGarantia = (byte)sqlResultado["fiRequiereGarantia"],
                                /* Centro de costo */
                                Agencia = (string)sqlResultado["fcNombreAgencia"],
                                /* Informacion del vendedor */
                                IdUsuarioAsignado = (int)sqlResultado["fiIDUsuarioAsignado"],
                                UsuarioAsignado = (string)sqlResultado["fcUsuarioAsignado"],
                                /* Informacion cliente */
                                IdCliente = (int)sqlResultado["fiIDCliente"],
                                IdentidadCliente = (string)sqlResultado["fcIdentidadCliente"],
                                NombreCliente = sqlResultado["fcNombreCliente"].ToString(),
                                IdGarantia = (int)sqlResultado["fiIDGarantia"],
                                VIN = sqlResultado["fcVIN"].ToString(),
                                Marca = sqlResultado["fcMarca"].ToString(),
                                Modelo = sqlResultado["fcModelo"].ToString(),
                                Anio = (int)sqlResultado["fiAnio"],
                                /* Informacion del analista */
                                IdAnalistaSolicitud = (int)sqlResultado["fiIDAnalista"],
                                AnalistaSolicitud = (string)sqlResultado["fcNombreCortoAnalista"],
                                /* Bitacora */
                                EnIngresoInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdEnIngresoInicio"]),
                                EnIngresoFin = ConvertFromDBVal<DateTime>(sqlResultado["fdEnIngresoFin"]),
                                EnTramiteInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdEnColaInicio"]),
                                EnTramiteFin = ConvertFromDBVal<DateTime>(sqlResultado["fdEnColaFin"]),
                                EnAnalisisInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdEnAnalisisInicio"]),
                                EnAnalisisFin = ConvertFromDBVal<DateTime>(sqlResultado["fdEnAnalisisFin"]),
                                CondicionadoInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdCondicionadoInicio"]),
                                CondificionadoFin = ConvertFromDBVal<DateTime>(sqlResultado["fdCondificionadoFin"]),
                                /* Proceso de campo */
                                EnvioARutaAnalista = ConvertFromDBVal<DateTime>(sqlResultado["fdEnvioARutaAnalista"]),
                                IdEstadoDeCampo = (byte)sqlResultado["fiEstadoDeCampo"],
                                EnCampoInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdEnRutaDeInvestigacionInicio"]),
                                ObservacionesDeGestoria = (string)sqlResultado["fcObservacionesDeCampo"],
                                EnCampoFin = ConvertFromDBVal<DateTime>(sqlResultado["fdEnRutaDeInvestigacionFin"]),
                                ReprogramadoInicio = ConvertFromDBVal<DateTime>(sqlResultado["fdReprogramadoInicio"]),
                                ReprogramadoComentario = (string)sqlResultado["fcReprogramadoComentario"],
                                ReprogramadoFin = ConvertFromDBVal<DateTime>(sqlResultado["fdReprogramadoFin"]),
                                PasoFinalInicio = (DateTime)sqlResultado["fdPasoFinalInicio"],
                                IdUsuarioPasoFinal = (int)sqlResultado["fiIDUsuarioPasoFinal"],
                                ComentarioPasoFinal = (string)sqlResultado["fcComentarioPasoFinal"],
                                PasoFinalFin = (DateTime)sqlResultado["fdPasoFinalFin"],
                                IdEstadoSolicitud = (byte)sqlResultado["fiEstadoSolicitud"],
                                FechaResolucion = (DateTime)sqlResultado["fdTiempoTomaDecisionFinal"],
                                ValorGarantia = (decimal)sqlResultado["fnValorGarantia"],
                                ValorPrima = (decimal)sqlResultado["fnValorPrima"],
                                ValorAPrestarGarantia = (decimal)sqlResultado["fnValorAPrestar"],
                                ValorAFinanciar = (decimal)sqlResultado["fnValorTotalFinanciamiento"],
                                Moneda = sqlResultado["fcSimboloMoneda"].ToString(),
                                Plazo = (int)sqlResultado["fiPlazo"],
                                TasaInteresAnual = (decimal)sqlResultado["fnTasaAnualAplicada"],
                                TasaInteresMensual = (decimal)sqlResultado["fnTasaMensualAplicada"],
                                CuotaSeguro = (decimal)sqlResultado["fnCuotaMensualSeguro"],
                                CuotaGPS = (decimal)sqlResultado["fnCuotaMensualGPS"],
                                IdExpediente = (int)sqlResultado["fiIDExpediente"],
                               
                               // FechaDesembolso = fiTiempoIncompleto,
                                ConteoIncompleto = fiTiempoIncompleto,
                                PermitirAbrirAnalisis = pcIDUsuario.Trim() == "27" || pcIDUsuario.Trim() == "1" || pcIDUsuario.Trim() == "28",
                            });
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using SqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

      



        return solicitudes;
    }


    [WebMethod]
    public static List<ClientesReferenciasViewModel> ListaReferenciaCliente(int IdSolicitud, int IdCliente, string dataCrypt)
    {
        var Referencia = new List<ClientesReferenciasViewModel>();
        Referencia.Clear();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
          
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", IdCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IdSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;
                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                      
                        while (sqlResultado.Read())
                        {
                            Referencia.Add(new ClientesReferenciasViewModel()
                            {
                                fiIDReferencia = (int)sqlResultado["fiIDReferencia"],
                                fiIDCliente = (int)sqlResultado["fiIDCliente"],
                                fcNombreCompletoReferencia = (string)sqlResultado["fcNombreCompletoReferencia"],
                                fcLugarTrabajoReferencia = (string)sqlResultado["fcLugarTrabajoReferencia"],
                                fiTiempoConocerReferencia = (short)sqlResultado["fiTiempoConocerReferencia"],
                                fcTelefonoReferencia = (string)sqlResultado["fcTelefonoReferencia"],
                                fiIDParentescoReferencia = (int)sqlResultado["fiIDParentescoReferencia"],
                                fcDescripcionParentesco = (string)sqlResultado["fcDescripcionParentesco"],
                                fbReferenciaActivo = (bool)sqlResultado["fbReferenciaActivo"],
                                fcRazonInactivo = (string)sqlResultado["fcRazonInactivo"],
                                fiIDUsuarioCrea = (int)sqlResultado["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)sqlResultado["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)sqlResultado["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)sqlResultado["fdFechaUltimaModifica"],
                                fcComentarioDeptoCredito = (string)sqlResultado["fcComentarioDeptoCredito"],
                                fiAnalistaComentario = (int)sqlResultado["fiAnalistaComentario"]
                            
                            });
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ex.Message.ToString();         
        }
        return Referencia;

    }

    [WebMethod]
    public static List<DetalleCondicion_ViewModel> ListaCondiciones(int IdSolicitud, string dataCrypt)
    {
        var condiciones = new List<DetalleCondicion_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            int IDPRODUCTO = 0;
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                using (var sqlComando = new SqlCommand("sp_CREDSolicitud_SolicitudCondiciones_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IdSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;
                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        //HtmlTableRow tRowSolicitudCondiciones = null;
                        string EstadoCondicion = String.Empty;
                        int contadorCondiciones = 1;
                        while (sqlResultado.Read())
                        {
                            condiciones.Add(new DetalleCondicion_ViewModel()
                            {
                                fcCondicion = sqlResultado["fcCondicion"].ToString(),
                                fcDescripcionCondicion = sqlResultado["fcDescripcionCondicion"].ToString(),
                                fcComentarioAdicional = sqlResultado["fcComentarioAdicional"].ToString(),
                                EstadoCondicion = (bool)sqlResultado["fbEstadoCondicion"] != true ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-danger mb-0'>Pendiente</label>"

                        });
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            //throw;
        }
        return condiciones;
       
    }
    [WebMethod]
    public static string ValidarToken(string token,  string dataCrypt)
    {
        var idTokenMensaje = "";
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreSeguridad.dbo.sp_Token_Aplicar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;                   
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcIP", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@pcToken", token);
                    sqlComando.Parameters.AddWithValue("@pcCodigoAplicacion", "S001");
                  
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                          //  if( !sqlResultado["fcMensaje"].ToString().StartsWith("1"))
                                idTokenMensaje = sqlResultado["fcMensaje"].ToString();
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return idTokenMensaje;
    }
    #region Obtener documentos de la garantía y de la solicitud

    [WebMethod]
    public static List<Documento_ViewModel> CargarDocumentosGarantia(int idGarantia, string dataCrypt)
    {
        return ObtenerDocumentosGarantiaPorIdGarantia(idGarantia, dataCrypt);
    }
    [WebMethod]
    public static bool ComentarioReferenciaPersonal(int IDReferencia, string comentario, string dataCrypt)
    {
        bool resultadoProceso = false;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Comentario", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", IDReferencia);
                    sqlComando.Parameters.AddWithValue("@fcComentarioDeptoCredito", comentario);
                    sqlComando.Parameters.AddWithValue("@fiAnalistaComentario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            if (!reader["MensajeError"].ToString().StartsWith("-1"))
                                resultadoProceso = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static bool EliminarReferenciaPersonal(int IDReferencia, string dataCrypt)
    {
        bool resultadoProceso = false;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            //string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string pcIDSesion = "1";

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("dbo.sp_CREDCliente_Referencias_Eliminar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", IDReferencia);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            if (!reader["MensajeError"].ToString().StartsWith("-1"))
                                resultadoProceso = true;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }
    [WebMethod]
    public static List<Documento_ViewModel> CargarExpedienteSolicitudGarantia(int idSolicitud, int idGarantia, string dataCrypt)
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

    public static List<Documento_ViewModel> ObtenerDocumentosGarantiaPorIdGarantia(int idGarantia, string dataCrypt)
    {
        var documentosDeLaGarantia = new List<Documento_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
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

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
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
                                IdSolicitudDocumento = (int)sqlResultado["fiIDSolicitudDocs"],
                                NombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                Extension = sqlResultado["fcTipoArchivo"].ToString(),
                                RutaArchivo = sqlResultado["fcRutaArchivo"].ToString(),
                                URLArchivo = sqlResultado["fcURL"].ToString() + ((int)sqlResultado["fiTipoDocumento"] == 8 || (int)sqlResultado["fiTipoDocumento"] == 9 ? ".png" : ""),
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

    [WebMethod]
    public static bool EliminarDocumento(int idSolicitud, int idSolicitudDocumento,  string dataCrypt)
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

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_EliminarDocumento_token", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitudDocumento", idSolicitudDocumento);
                    //sqlComando.Parameters.AddWithValue("@pcObservaciones", observaciones.Trim());
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


    #region Crear expediente del prestamo

    [WebMethod]
    public static string CrearExpedientePrestamo(int idSolicitud, string comentarios, string dataCrypt)
    {
        var idExpedienteCreado = "-1";
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Expedientes_Maestro_Guardar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@pcComentarios", comentarios.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (!sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                idExpedienteCreado = sqlResultado["MensajeError"].ToString();
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return idExpedienteCreado;
    }

    #endregion

    #region Metodos utilitarios

    [WebMethod]
    public static string EncriptarParametros(int idSolicitud, int idCliente, int idGarantia, string identidad, string idExpediente, string dataCrypt)
    {
        string resultado;
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "1";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var lcParametros = "usr=" + pcIDUsuario + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion + "&pcID=" + identidad + "&IDSOL=" + idSolicitud + "&IDGarantia=" + idGarantia + "&IdCredCliente=" + idCliente + "&idExpediente=" + idExpediente;
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

    public static T ConvertFromDBVal<T>(object obj)
    {
        if (obj == null || obj == DBNull.Value)
            return default(T);
        else
            return (T)obj;
    }
    #endregion

    #region View Models

    public class SolicitudesCredito_Bandeja_ViewModel
    {
        public int IdSolicitud { get; set; }
        public DateTime FechaCreacionSolicitud { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public int RequiereGarantia { get; set; }
        public int IdAgencia { get; set; }
        public string Agencia { get; set; }
        public int IdUsuarioAsignado { get; set; }
        public string UsuarioAsignado { get; set; }
        public int IdCliente { get; set; }
        public string IdentidadCliente { get; set; }
        public string NombreCliente { get; set; }
        public bool ClienteActivo { get; set; }
        public string RazonInactivo { get; set; }
        public int IdGarantia { get; set; }
        public string VIN { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }

        public int SolicitudActiva { get; set; }
        public int IdUsuarioCreador { get; set; }
        public string UsuarioCreador { get; set; }
        public int IdAnalistaSolicitud { get; set; }
        public string AnalistaSolicitud { get; set; }
        public int IdGestorAsignado { get; set; }
        public string GestorAsignado { get; set; }
        public int IdEstadoSolicitud { get; set; }

        public DateTime EnIngresoInicio { get; set; }
        public DateTime EnIngresoFin { get; set; }
        public DateTime EnTramiteInicio { get; set; }
        public DateTime EnTramiteFin { get; set; }
        public DateTime EnAnalisisInicio { get; set; }
        public DateTime FechaValidacionInformacionPersonal { get; set; }
        public string ComentarioValidacionInfoPersonal { get; set; }
        public DateTime FechaValidacionDocumentacion { get; set; }
        public string ComentarioValidacionDocumentacion { get; set; }
        public DateTime FechaValidacionReferenciasPersonales { get; set; }
        public string ComentarioValidacionReferenciasPersonales { get; set; }
        public DateTime FechaValidacionInformacionLaboral { get; set; }
        public string ComentarioValidacionInformacionLaboral { get; set; }
        public DateTime FechaResolucion { get; set; }
        public string ObservacionesDeCredito { get; set; }
        public string ComentarioResolucion { get; set; }
        public DateTime EnAnalisisFin { get; set; }
        public DateTime CondicionadoInicio { get; set; }
        public DateTime CondificionadoFin { get; set; }
        public int IdEstadoDeCampo { get; set; }
        public DateTime EnvioARutaAnalista { get; set; }
        public DateTime EnCampoInicio { get; set; }
        public string ObservacionesDeGestoria { get; set; }
        public DateTime EnCampoFin { get; set; }
        public DateTime ReprogramadoInicio { get; set; }
        public string ReprogramadoComentario { get; set; }
        public DateTime ReprogramadoFin { get; set; }
        public DateTime PasoFinalInicio { get; set; }
        public int IdUsuarioPasoFinal { get; set; }
        public string ComentarioPasoFinal { get; set; }
        public DateTime PasoFinalFin { get; set; }

        /* Información del préstamo */
        public decimal ValorGarantia { get; set; }
        public decimal ValorPrima { get; set; }
        public decimal ValorAPrestarGarantia { get; set; }
        public decimal ValorAFinanciar { get; set; }
        public string Moneda { get; set; }
        public int Plazo { get; set; }
        public decimal TasaInteresAnual { get; set; }
        public decimal TasaInteresMensual { get; set; }
        public decimal CuotaSeguro { get; set; }
        public decimal CuotaGPS { get; set; }
        public int IdExpediente { get; set; }
        public bool PermitirAbrirAnalisis { get; set; }
        public int FechaDesembolso { get; set; }
        public int ConteoIncompleto { get; set; }
    }

    public class Documento_ViewModel
    {
        public int IdSolicitudDocumento { get; set; }
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


    public class DetalleCondicion_ViewModel
    {
        public string fcCondicion { get; set; }       
        public string fcDescripcionCondicion { get; set; }
        public string fcComentarioAdicional { get; set; }
        public string EstadoCondicion { get; set; }

    }

    public class ClientesReferenciasViewModel
    {
        public int fiIDReferencia { get; set; }
        public int fiIDCliente { get; set; }
        public string fcNombreCompletoReferencia { get; set; }
        public string fcLugarTrabajoReferencia { get; set; }
        public short fiTiempoConocerReferencia { get; set; }
        public string fcTelefonoReferencia { get; set; }
        public int fiIDParentescoReferencia { get; set; }
        public string fcDescripcionParentesco { get; set; }
        public bool fbReferenciaActivo { get; set; }
        public string fcRazonInactivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public string fcComentarioDeptoCredito { get; set; }
        public Nullable<int> fiAnalistaComentario { get; set; }
    }

    #endregion
}