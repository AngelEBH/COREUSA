using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Services;

public partial class SolicitudesGPS_RevisionGarantia : System.Web.UI.Page
{
    #region Propiedades

    public string pcIDApp = "";
    public string pcIDSesion = "";
    public string pcIDUsuario = "";
    public string pcIDGarantia = "";
    public string pcIDSolicitudGPS = "";
    public string pcIDSolicitudCredito = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    public List<Garantia_Revision_ViewModel> RevisionesDeLaGarantia = new List<Garantia_Revision_ViewModel>();
    public string RevisionesDeLaGarantiaJSON = ""; // Serializar a formato JSON el listado de revisiones de la garantia para pasarlo a variable JS en el .aspx

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
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
                    pcIDGarantia = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDGarantia");
                    pcIDSolicitudGPS = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSolicitudGPS") ?? "0";
                    pcIDSolicitudCredito = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

                    /* Cagrar información de la garantía y el listado de revisiones de la garantía */
                    CargarInformacion();

                    /* Serializar a formato JSON el listado de revisiones de la garantia para pasarlo a variable JS en el .aspx */
                    RevisionesDeLaGarantiaJSON = JsonConvert.SerializeObject(RevisionesDeLaGarantia);
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
            }
        }
    }

    private void CargarInformacion()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitud_CREDGarantia_ObtenerPorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", pcIDSolicitudCredito);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            Response.Write("<script>");
                            Response.Write("window.open('SolicitudesCredito_ListadoGarantias.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&SID=" + pcIDSesion + "&IDApp=" + pcIDApp) + "','_self')");
                            Response.Write("</script>");
                        }

                        while (sqlResultado.Read())
                        {
                            txtNombreCliente.Text = sqlResultado["fcNombreCliente"].ToString();
                            txtIdentidadCliente.Text = sqlResultado["fcIdentidadCliente"].ToString();
                            txtRtn.Text = sqlResultado["fcRTN"].ToString();
                            txtTelefonoCliente.Text = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                            int requiereGarantia = (byte)sqlResultado["fiRequiereGarantia"];

                            sqlResultado.NextResult();

                            if (!sqlResultado.HasRows)
                            {
                                Response.Write("<script>");
                                Response.Write("window.open('Garantia_Registrar.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&SID=" + pcIDSesion + "&IDApp=" + pcIDApp + "&IDSOL=" + pcIDSolicitudCredito) + "','_self')");
                                Response.Write("</script>");
                            }
                            else
                            {
                                /* Informacion del garantía */
                                while (sqlResultado.Read())
                                {
                                    var recorridoNumerico = Convert.ToDecimal(sqlResultado["fnRecorrido"].ToString());
                                    var recorrido = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnRecorrido"].ToString())) + " " + sqlResultado["fcUnidadDeDistancia"].ToString();

                                    txtVIN.Text = sqlResultado["fcVin"].ToString();
                                    txtTipoDeGarantia.Text = sqlResultado["fcTipoGarantia"].ToString();
                                    txtTipoDeVehiculo.Text = sqlResultado["fcTipoVehiculo"].ToString();
                                    txtMarca.Text = sqlResultado["fcMarca"].ToString();
                                    txtModelo.Text = sqlResultado["fcModelo"].ToString();
                                    txtAnio.Text = sqlResultado["fiAnio"].ToString();
                                    txtColor.Text = sqlResultado["fcColor"].ToString();
                                    txtMatricula.Text = sqlResultado["fcMatricula"].ToString();
                                    txtSerieMotor.Text = sqlResultado["fcMotor"].ToString();
                                    txtSerieChasis.Text = sqlResultado["fcChasis"].ToString();
                                    txtGPS.Text = sqlResultado["fcGPS"].ToString();
                                    txtCilindraje.Text = sqlResultado["fcCilindraje"].ToString();
                                    txtRecorrido.Text = recorrido;
                                    txtTransmision.Text = sqlResultado["fcTransmision"].ToString();
                                    txtTipoDeCombustible.Text = sqlResultado["fcTipoCombustible"].ToString();
                                    txtSerieUno.Text = sqlResultado["fcSerieUno"].ToString();
                                    txtSerieDos.Text = sqlResultado["fcSerieDos"].ToString();
                                    txtComentario.InnerText = sqlResultado["fcComentario"].ToString().Trim();
                                    lblMarca.InnerText = sqlResultado["fcMarca"].ToString();
                                    lblModelo.InnerText = sqlResultado["fcModelo"].ToString();
                                    lblAnio.InnerText = sqlResultado["fiAnio"].ToString();
                                    lblColor.InnerText = sqlResultado["fcColor"].ToString();
                                }

                                /* Fotografías de la garantía */
                                sqlResultado.NextResult();

                                if (!sqlResultado.HasRows)
                                {
                                    divGaleriaGarantia.InnerHtml = "<img alt='No hay fotografías disponibles' src='/Imagenes/Imagen_no_disponible.png' data-image='/Imagenes/Imagen_no_disponible.png' data-description='No hay fotografías disponibles'/>";
                                }
                                else
                                {
                                    var imagenesGarantia = new StringBuilder();
                                    var url = string.Empty;

                                    while (sqlResultado.Read())
                                    {
                                        url = @"http://172.20.3.140/" + sqlResultado["fcURL"].ToString();

                                        imagenesGarantia.Append("<img alt='" + sqlResultado["fcSeccionGarantia"] + "' src='" + url + "' data-image='" + url + "' data-description='" + sqlResultado["fcSeccionGarantia"] + "'/>");
                                    }
                                    divGaleriaGarantia.InnerHtml = imagenesGarantia.ToString();
                                }
                            } // else !sqlResultado.HasRows
                        } // while sqlResultado.Read()
                    } // using sqlComando.ExecuteReader()
                } // using sqlComando

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_Revisiones_ListarPorIdGarantia", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDGarantia", pcIDGarantia);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        var templateRevisiones = new StringBuilder();
                        var idEstadoRevision = 0;
                        var estadoRevision = string.Empty;
                        var estadoRevisionClassName = string.Empty;
                        var dataString = string.Empty;
                        var btnResultadoRevision = string.Empty;

                        while (sqlResultado.Read())
                        {
                            idEstadoRevision = (int)sqlResultado["fiIDEstadoRevision"];
                            estadoRevision = sqlResultado["fcEstadoRevision"].ToString();
                            estadoRevisionClassName = sqlResultado["fcEstadoRevisionClassName"].ToString();
                            dataString = "data-id='" + sqlResultado["fiIDRevision"].ToString() + "' data-revision='" + sqlResultado["fcNombreRevision"] + "' data-descripcion='" + sqlResultado["fcDescripcionRevision"] + "' data-observaciones='" + sqlResultado["fcObservaciones"] + "' ";

                            btnResultadoRevision = "<button id='btnResultadoRevision' class='border-0 btn-transition btn btn-outline-warning' " + dataString + " type='button'><i class='fas fa-edit'></i></button>";

                            templateRevisiones.Append(
                            "<li class='list-group-item'>" +
                            "<div class='todo-indicator bg-" + estadoRevisionClassName + "' id='todo-indicator-revision-" + sqlResultado["fiIDRevision"] + "'></div>" +
                            "<div class='widget-content p-0'>" +
                            "<div class='widget-content-wrapper'>" +
                            "<div class='widget-content-left flex2'>" +
                            "<div class='widget-heading'>" +
                            sqlResultado["fcNombreRevision"].ToString() +
                            "<div class='badge badge-" + estadoRevisionClassName + " ml-2' id='badge-revision-" + sqlResultado["fiIDRevision"] + "'>" + estadoRevision + "</div>" +
                            "</div>" +
                            "</div>" +
                            "<div class='widget-content-right'>" +
                            btnResultadoRevision +
                            "</div>" +
                            "</div>" +
                            "</div>" +
                            "</li>");

                            RevisionesDeLaGarantia.Add(new Garantia_Revision_ViewModel()
                            {
                                IdGarantiaRevision = (int)sqlResultado["fiIDGarantiaRevision"],
                                IdGarantia = int.Parse(pcIDGarantia),
                                IdRevision = (int)sqlResultado["fiIDRevision"],
                                Revision = sqlResultado["fcNombreRevision"].ToString(),
                                IdSolicitudGPS = int.Parse(pcIDSolicitudCredito),
                                IdEstadoRevision = (int)sqlResultado["fiEstadoRevision"],
                                EstadoRevision = estadoRevision,
                                Observaciones = sqlResultado["fcObservaciones"].ToString()
                            });
                        }

                        divRevisionesDeLaGarantia.InnerHtml += templateRevisiones.ToString();

                    } // using sqlComando.ExecuteReader()
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar información: " + ex.Message.ToString());
        }
    }

    [WebMethod]
    public static Resultado_ViewModel FinalizarRevisionGarantia(List<Garantia_Revision_ViewModel> revisionesGarantia, decimal recorrido, string unidadDeDistancia, string dataCrypt)
    {
        var resultado = new Resultado_ViewModel() { ResultadoExitoso = true };
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDGarantia = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDGarantia");
            var pcIDSolicitudGPS = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSolicitudGPS") ?? "0";
            var pcIDSolicitudCredito = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlTransaccion = sqlConexion.BeginTransaction("revisionGarantia"))
                {
                    try
                    {
                        foreach (Garantia_Revision_ViewModel item in revisionesGarantia)
                        {
                            using (var sqlComando = new SqlCommand("sp_CREDGarantias_Revisiones_Guardar", sqlConexion, sqlTransaccion))
                            {
                                sqlComando.CommandType = CommandType.StoredProcedure;
                                sqlComando.Parameters.AddWithValue("@piIDGarantia", pcIDGarantia);
                                sqlComando.Parameters.AddWithValue("@piIDRevision", item.IdRevision);
                                sqlComando.Parameters.AddWithValue("@piIDAutoGPSInstalacion", pcIDSolicitudGPS);
                                sqlComando.Parameters.AddWithValue("@piEstadoRevision", item.IdEstadoRevision);
                                sqlComando.Parameters.AddWithValue("@pcObservaciones", item.Observaciones.Trim());
                                sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                sqlComando.CommandTimeout = 120;

                                using (var sqlResultado = sqlComando.ExecuteReader())
                                {
                                    sqlResultado.Read();

                                    if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                    {
                                        sqlTransaccion.Rollback();
                                        resultado.ResultadoExitoso = false;
                                        resultado.MensajeResultado = "Error al guardar resultado de la revisión: " + item.Revision + ", contacte al aministrador.";
                                        resultado.MensajeDebug = "_Revisiones_Guardar" + sqlResultado["MensajeError"].ToString();
                                        return resultado;
                                    }
                                }
                            }
                        }

                        using (var sqlComando = new SqlCommand("sp_CREDGarantias_Revisiones_ActualizarMillaje", sqlConexion, sqlTransaccion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDGarantia", pcIDGarantia);
                            sqlComando.Parameters.AddWithValue("@pnRecorrido", recorrido);
                            sqlComando.Parameters.AddWithValue("@pcUnidadDeDistancia", unidadDeDistancia);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.CommandTimeout = 120;

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                sqlResultado.Read();

                                if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                {
                                    sqlTransaccion.Rollback();
                                    resultado.ResultadoExitoso = false;
                                    resultado.MensajeResultado = "Error al actualizar millaje de la garantía, contacte al aministrador.";
                                    resultado.MensajeDebug = "_Revisiones_ActualizarMillaje" + sqlResultado["MensajeError"].ToString();
                                    return resultado;
                                }
                            }
                        }

                        if (resultado.ResultadoExitoso == true)
                        {
                            sqlTransaccion.Commit();
                            resultado.MensajeResultado = "Las revisiones se guardaron exitosamente";
                            resultado.MensajeDebug = "";

                            EnviarCorreoYSMSRevisionDeGarantia(pcIDSolicitudCredito, revisionesGarantia, recorrido, unidadDeDistancia, pcIDUsuario, pcIDApp, pcIDSesion);
                        }
                    }
                    catch (Exception ex)
                    {
                        sqlTransaccion.Rollback();

                        resultado.ResultadoExitoso = false;
                        resultado.MensajeResultado = "Error al guardar resultado de la revisión: " + ex.Message.ToString() + ". Contacte al aministrador.";
                        resultado.MensajeDebug = "_Revisiones_Guardar";
                    }
                } // using sqlTransacciomn
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado.ResultadoExitoso = false;
            resultado.MensajeResultado = "Error al guardar resultado de la revisión: " + ex.Message.ToString() + ". Contacte al aministrador.";
            resultado.MensajeDebug = "_Revisiones_Guardar";
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

    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text = mensaje;
    }

    public static void EnviarCorreoYSMSRevisionDeGarantia(string idSolicitudCredito, List<Garantia_Revision_ViewModel> revisionesGarantia, decimal recorrido, string unidadDeDistancia, string idUsuario, string idApp, string idSesion)
    {
        try
        {
            var usuarioVendedor = string.Empty;
            var correoUsuarioVendedor = string.Empty;
            var telefonoUsuarioVendedor = string.Empty;
            var usuarioValidador = string.Empty;
            var correoUsuarioValidador = string.Empty;
            var VIN = string.Empty;
            var marca = string.Empty;
            var modelo = string.Empty;
            var anio = string.Empty;
            var color = string.Empty;
            var nombreCliente = string.Empty;
            var identidadCliente = string.Empty;
            var contenidoCorreo = new StringBuilder();

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDGarantias_Revisiones_ObtenerDatosCorreo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitudCredito", idSolicitudCredito);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", idSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", idApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", idUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /* Información del vendedor de la solicitud y el usuario que validó la garantía */
                        while (sqlResultado.Read())
                        {
                            usuarioVendedor = sqlResultado["fcUsuarioVendedor"].ToString();
                            correoUsuarioVendedor = sqlResultado["fcCorreoUsuarioVendedor"].ToString();
                            telefonoUsuarioVendedor = sqlResultado["fcTelefonoUsuarioVendedor"].ToString();
                            usuarioValidador = sqlResultado["fcUsuarioValidador"].ToString();
                            correoUsuarioValidador = sqlResultado["fcCorreoUsuarioValidador"].ToString();
                        }

                        /* Información de la garantía */
                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            VIN = sqlResultado["fcVin"].ToString();
                            marca = sqlResultado["fcMarca"].ToString();
                            modelo = sqlResultado["fcModelo"].ToString();
                            anio = sqlResultado["fiAnio"].ToString();
                            color = sqlResultado["fcColor"].ToString();
                        }

                        /* Información del cliente */
                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            nombreCliente = sqlResultado["fcNombreCliente"].ToString();
                            identidadCliente = sqlResultado["fcIdentidadCliente"].ToString();
                        }

                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion

            /* COMENTADO PORQUE DE MOMENTO NO SE REQUIERE QUE SE ENVÍE EL CORREO */

            /* Información basico y datos del cliente */
            //contenidoCorreo.Append("<table border='1' style='width: 600px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;'>" +
            // "<tr><th colspan='2' style='text-align: left; font-weight: bold;'><b>INFORMACIÓN BÁSICA</b></th></tr>" +
            // "<tr><th style='text-align: left;'>Vendedor</th><td>" + usuarioVendedor + "</td></tr>" +
            // "<tr><th style='text-align: left;'>Validado por</th><td>" + usuarioValidador + "</td></tr>" +
            // "<tr><th style='text-align: left;'>VIN</th><td>" + VIN + "</td></tr>" +
            // "<tr><th style='text-align: left;'>Marca</th><td>" + marca + "</td></tr><tr>" +
            // "<th style='text-align: left;'>Modelo</th><td>" + modelo + "</td></tr>" +
            // "<tr><th style='text-align: left;'>Año</th><td>" + anio + "</td></tr>" +
            // "<tr><th style='text-align: left;'>Color</th><td>" + color + "</td></tr>" +
            // "<tr><th colspan='2'><br /></th></tr>" +
            // "<tr><th colspan='2' style='text-align: left; font-weight: bold;'><b>DATOS DEL CLIENTE</b></th></tr>" +
            // "<tr><th style='text-align: left;'>Nombre</th><td>" + nombreCliente + "</td></tr>" +
            // "<tr><th style='text-align: left;'>Identidad</th><td>" + identidadCliente + "</td></tr>" +
            // "</table>" +
            // "<br />" +
            // /* Inicio de la tabla de revisiones de la garantia y millaje actualizado */
            // "<table border='1' style='width: 600px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;'>" +
            // "<thead><tr><th colspan='2' style='text-align:center; font-weight:bold'>REVISIONES REALIZADAS</th></tr></thead>" +
            // "<tr><th colspan='2' style='text-align: left; font-weight: bold;'><b>Actualización de millaje</b></th></tr>" +
            // "<tr><th style='text-align: left;'>Nuevo millaje</th><td>" + recorrido.ToString("N") + "</td></tr>" +
            // "<tr><th style='text-align: left;'>Unidad de medida</th><td>" + unidadDeDistancia + "</td></tr>");

            /* Revisiones realizadas */
            //foreach (var item in revisionesGarantia)
            //{
            // contenidoCorreo.Append(
            // "<tr><th colspan='2' style='text-align: left; font-weight: bold;'><b>" + item.Revision + "</b></th></tr>" +
            // "<tr><th style='text-align: left;'>Estado</th><td>" + item.EstadoRevision + "</td></tr>" +
            // "<tr><th style='text-align: left;'>Observaciones</th><td>" + item.Observaciones + "</td></tr>");
            //}

            //contenidoCorreo.Append("</table>");

            //resultado = EnviarCorreo("Revisión de garantía | Solicitud de crédito " + idSolicitudCredito, contenidoCorreo.ToString(), correoUsuarioVendedor);

            /* ENIVAR SMS AL TELEFONO DEL VENDEDOR*/
            if (telefonoUsuarioVendedor != "")
            {
                string resultadoRevisionGarantia = revisionesGarantia.Where(x => x.IdEstadoRevision == 2).Any() ? "rechazada" : "aprobada";
                string mensajeResultado = "La garantía de la solicitud de crédito " + idSolicitudCredito + " ha sido " + resultadoRevisionGarantia + " por " + usuarioValidador;
                EnviarSMS(telefonoUsuarioVendedor, mensajeResultado);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public static bool EnviarCorreo(string pcTituloGeneral, string pcContenidodelMensaje, string buzonCorreoUsuario)
    {
        var resultado = false;
        try
        {
            var pmmMensaje = new MailMessage();
            var smtpCliente = new SmtpClient();

            smtpCliente.Host = "mail.miprestadito.com";
            smtpCliente.Port = 587;
            smtpCliente.Credentials = new System.Net.NetworkCredential("systembot@miprestadito.com", "iPwf@p3q");
            smtpCliente.EnableSsl = true;

            pmmMensaje.Subject = "Revisión de garantía";
            pmmMensaje.From = new MailAddress("systembot@miprestadito.com", "System Bot");
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
            " <td style=\"background-color:whitesmoke; text-align:center;\">Resultados de la revisión</td>" +
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
            smtpCliente.Dispose();

            resultado = true;
        }
        catch (Exception ex)
        {
            ExceptionLogging.SendExcepToDB(ex);
            resultado = false;
        }

        return resultado;
    }

    public static void EnviarSMS(string numeroReceptor, string mensajeDeTexto)
    {
        string url;
        try
        {
            using (var client = new WebClient())
            {
                numeroReceptor = numeroReceptor == "" ? "96116376" : numeroReceptor;

                url = "http://172.20.3.177/default/en_US/send.html?u=admin&p=admin&l=1&n=" + numeroReceptor.Replace("-", "") + "&m=" + mensajeDeTexto;
                client.DownloadString(new Uri(url));
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public class Garantia_Revision_ViewModel
    {
        public int IdGarantiaRevision { get; set; }
        public int IdGarantia { get; set; }
        public int IdRevision { get; set; }
        public string Revision { get; set; }
        public int IdSolicitudGPS { get; set; }
        public int IdEstadoRevision { get; set; }
        public string EstadoRevision { get; set; }
        public string Observaciones { get; set; }
    }

    public class Resultado_ViewModel
    {
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string MensajeDebug { get; set; }
    }
}