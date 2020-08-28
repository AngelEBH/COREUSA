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

                if (lcParametros != String.Empty && 2 == 1)
                {
                    pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                    lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcSesionID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");

                    //using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                    //{
                    //    sqlConexion.Open();

                    //    /* Informacion completa de la solicitud y del cliente */
                    //    using (SqlCommand sqlComando = new SqlCommand("sp_CREDSolicitud_InformacionCompletaClienteSolicitud", sqlConexion))
                    //    {
                    //        sqlComando.CommandType = CommandType.StoredProcedure;
                    //        sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    //        sqlComando.Parameters.AddWithValue("@piIDSesion", pcSesionID);
                    //        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    //        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    //        using (SqlDataReader reader = sqlComando.ExecuteReader())
                    //        {
                    //            while (reader.Read())
                    //            {
                    //                lblTipoPrestamo.Text = (string)reader["fcNombreProducto"];
                    //                LogoPrestamo.ImageUrl = "http://172.20.3.140/Imagenes/" + NombreLogo;
                    //                spanNombreCliente.Text = reader["fcNombreCliente"].ToString();
                    //                spanIdentidadCliente.Text = IdentidadCLTE;
                    //                lblNoSolicitud.Text = reader["fiIDSolicitudCANEX"].ToString();
                    //                lblNoCliente.Text = reader["fiIDCliente"].ToString();
                    //                lblTipoSolicitud.Text = "Solicitud CANEX";
                    //                lblAgenteDeVentas.Text = reader["fcNombreUsuario"].ToString();
                    //                lblAgencia.Text = reader["fcNombreAgencia"].ToString();
                    //                lblEstadoSolicitud.Text = reader["fcEstadoSolicitud"].ToString();
                    //                lblEstadoSolicitudModal.Text = reader["fcEstadoSolicitud"].ToString();
                    //                lblRtnCliente.Text = reader["fcRTN"].ToString();
                    //                lblNumeroTelefono.NavigateUrl = "tel:" + reader["fcTelefonoPrimario"].ToString();
                    //                lblNumeroTelefono.Text = reader["fcTelefonoPrimario"].ToString();
                    //                lblNumeroTelefonoAlternativo.NavigateUrl = "tel:" + reader["fcTelefonoAlternativo"].ToString();
                    //                lblNumeroTelefonoAlternativo.Text = reader["fcTelefonoAlternativo"].ToString();
                    //                lblNacionalidad.Text = reader["fcNacionalidadCliente"].ToString();
                    //                /* Calcular edad del cliente */
                    //                var hoy = DateTime.Today;
                    //                var edad = hoy.Year - FechaNacimientoCliente.Year;
                    //                if (FechaNacimientoCliente.Date > hoy.AddYears(-edad)) edad--;
                    //                lblFechaNacimientoCliente.Text = FechaNacimientoCliente.ToString("MM/dd/yyyy");
                    //                lblEdadCliente.Text = edad.ToString();
                    //                lblProfesionCliente.Text = reader["fcProfesionuOficio"].ToString();
                    //                lblSexoCliente.Text = reader["fcSexoCliente"].ToString();
                    //                lblEstadoCivilCliente.Text = reader["fcEstadoCivilCliente"].ToString();
                    //                lblViviendaCliente.Text = reader["fcTipoResidenciaCliente"].ToString();
                    //                lblTiempoResidirCliente.Text = "n/a";
                    //                lblDeptoCliente.Text = reader["fcDepartamentoDomicilio"].ToString();
                    //                lblMunicipioCliente.Text = reader["fcMunicipioDomicilio"].ToString();
                    //                lblCiudadCliente.Text = reader["fcCiudadDomicilio"].ToString();
                    //                lblBarrioColoniaCliente.Text = reader["fcBarrioColoniaDomicilio"].ToString();
                    //                lblDireccionDetalladaCliente.Text = reader["fcDireccionDetallada"].ToString();
                    //                lblReferenciaDomicilioCliente.Text = reader["fcDireccionReferencias"].ToString();
                    //                String NombreCompletoConyugue = reader["fcPrimerNombreConyugue"].ToString() + reader["fcSegundoNombreConyugue"].ToString() + reader["fcPrimerApellidoConyugue"].ToString() + reader["fcSegundoApellidoConyugue"].ToString();
                    //                if (string.IsNullOrEmpty(reader["fcPrimerNombreConyugue"].ToString()))
                    //                {
                    //                    divConyugueCliente.Visible = false;
                    //                }
                    //                else
                    //                {
                    //                    lblNombreConyugue.Text = NombreCompletoConyugue.Replace(" ", " ");
                    //                    DateTime FechaNacimientoConyugue = (DateTime)reader["fdNacimientoCliente"];
                    //                    lblFechaNacimientoConygue.Text = FechaNacimientoConyugue.ToString("MM/dd/yyyy");
                    //                    lblTelefonoConyugue.NavigateUrl = "tel:" + reader["fcTelefonoConyugue"].ToString();
                    //                    lblTelefonoConyugue.Text = reader["fcTelefonoConyugue"].ToString();
                    //                    lblOcupacionConyugue.Text = reader["fcOcupacionConyugue"].ToString();
                    //                    lblProfesionOficioConyugue.Text = reader["fcProfesionuOficioConyugue"].ToString();
                    //                    lblPuestoAsignadoConyugue.Text = reader["fcPuestoConyugue"].ToString();
                    //                    lblLugarTrabajoConyugue.Text = reader["fcNombreEmpresaConyugue"].ToString();
                    //                }
                    //                lblNombreTrabajoCliente.Text = reader["fcNombreEmpresa"].ToString();
                    //                lblIngresosMensualesCliente.Text = reader["fnIngresoMensualBase"].ToString();
                    //                if (EsComisionista == 0)
                    //                {
                    //                    lblComisionesClienteTitulo.Visible = false;
                    //                    lblComisionesCliente.Visible = false;
                    //                }
                    //                else
                    //                    lblComisionesCliente.Text = reader["fnIngresoMensualComisiones"].ToString();
                    //                lblPuestoAsignadoCliente.Text = reader["fcPuesto"].ToString();
                    //                lblTelefonoEmpresaCliente.NavigateUrl = "tel:" + reader["fcTelefonoEmpresa"].ToString();
                    //                lblTelefonoEmpresaCliente.Text = reader["fcTelefonoEmpresa"].ToString();
                    //                lblExtension.Text = reader["fcExtension"].ToString();
                    //                lblDeptoEmpresa.Text = reader["fcDepartamentoEmpresa"].ToString();
                    //                lblMunicipioEmpresa.Text = reader["fcMunicipioEmpresa"].ToString();
                    //                lblCiudadEmpresa.Text = reader["fcCiudadEmpresa"].ToString();
                    //                lblBarrioColoniaEmpresa.Text = reader["fcBarrioColoniaEmpresa"].ToString();
                    //                lblDireccionDetalladaEmpresa.Text = reader["fcDireccionDetalladaEmpresa"].ToString();
                    //                lblReferenciaUbicacionEmpresa.Text = reader["fcDireccionReferenciasEmpresa"].ToString();
                    //                /* Informacion del préstamo requerido */
                    //                lblValorGlobal.Text = ((decimal)reader["fnValorGlobal"]).ToString("N");
                    //                lblValorPrima.Text = ((decimal)reader["fnValorPrima"]).ToString("N");
                    //                lblPlazoTitulo.Text = "Plazo " + reader["fcTipodeCuota"].ToString();
                    //                lblPlazo.Text = reader["fcPlazo"].ToString();
                    //                lblMontoFinanciar.Text = ((decimal)reader["fnValorPrestamo"]).ToString("N");


                    //                /* Habilitar/Inhabilitar botones de acciones */
                    //                int EstadoRechazada = 5;
                    //                int EstadoAprobada = 4;
                    //                int EstadoEnRevision = 3;
                    //                string colorClass = string.Empty;

                    //                if (EstadoSolicitud == EstadoAprobada || EstadoSolicitud == EstadoRechazada)
                    //                {
                    //                    colorClass = EstadoSolicitud == EstadoAprobada ? "text-success" : "text-danger";

                    //                }
                    //                else if (IDSolicitudPrestadito != 0)
                    //                {
                    //                    colorClass = "text-success";
                    //                }
                    //                else if (EstadoSolicitud == EstadoEnRevision && IDSolicitudPrestadito == 0)
                    //                {
                    //                    colorClass = "text-warning";
                    //                }
                    //                else if (EstadoSolicitud != EstadoRechazada && EstadoSolicitud != EstadoAprobada && IDSolicitudPrestadito == 0)
                    //                {
                    //                    colorClass = "text-warning";
                    //                }

                    //                string claseCss = "col-form-label font-16 font-weight-bold " + colorClass;
                    //                lblEstadoSolicitud.Attributes.Add("class", colorClass);
                    //                lblEstadoSolicitudModal.Attributes.Add("class", claseCss);

                    //                /* Comentarios acerca de la resolucion de la solicitud */
                    //                string nombreAnalistaResolucion = reader["fcNombreAnalistaResolucion"].ToString();
                    //                string comentarioResolucion = reader["fcComentarioResulucion"].ToString();

                    //                if (!string.IsNullOrEmpty(comentarioResolucion))
                    //                {
                    //                    btnDetallesResolucion.Visible = true;
                    //                    lblNombreAnalista.Text = nombreAnalistaResolucion;
                    //                    lblDetalleEstado.InnerText = comentarioResolucion;
                    //                }
                    //            }
                    //        }
                    //    }

                    //    /* Referencias del cliente */
                    //    using (SqlCommand sqlComando = new SqlCommand("sp_CANEX_Solicitud_Referencias", sqlConexion))
                    //    {
                    //        sqlComando.CommandType = CommandType.StoredProcedure;
                    //        sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    //        sqlComando.Parameters.AddWithValue("@piIDSesion", pcSesionID);
                    //        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    //        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    //        using (SqlDataReader reader = sqlComando.ExecuteReader())
                    //        {
                    //            /* Llenar table de referencias */
                    //            HtmlTableRow tRowReferencias = null;
                    //            while (reader.Read())
                    //            {
                    //                tRowReferencias = new HtmlTableRow();
                    //                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcNombre"].ToString() });
                    //                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcTrabajo"].ToString() });
                    //                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcTiempoConocerlo"].ToString() });
                    //                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcTelefono"].ToString() });
                    //                tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcParentesco"].ToString() });
                    //                tblReferencias.Rows.Add(tRowReferencias);
                    //            }
                    //        }
                    //    }

                    //    /* Informacion del precalificado */
                    //    using (SqlCommand sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion))
                    //    {
                    //        sqlComando.CommandType = CommandType.StoredProcedure;
                    //        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    //        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    //        sqlComando.Parameters.AddWithValue("@pcIdentidad", IdentidadCLTE);

                    //        using (SqlDataReader reader = sqlComando.ExecuteReader())
                    //        {
                    //            while (reader.Read())
                    //            {
                    //                decimal ObligacionesPrecalificado = (decimal)reader["fnTotalObligaciones"];
                    //                decimal IngresosReales = (decimal)reader["fnIngresos"];
                    //                decimal CapacidadPagoMensual = 0;
                    //                if (IDPRODUCTO == 101)
                    //                {
                    //                    CapacidadPagoMensual = ObligacionesPrecalificado == 0 ? (IngresosReales * 13) / 100 : ((IngresosReales - ObligacionesPrecalificado) * 13) / 100;
                    //                }
                    //                else if (IDPRODUCTO == 201)
                    //                {
                    //                    CapacidadPagoMensual = ObligacionesPrecalificado == 0 ? (IngresosReales * 30) / 100 : ((IngresosReales - ObligacionesPrecalificado) * 30) / 100;
                    //                }
                    //                else if (IDPRODUCTO == 202)
                    //                {
                    //                    CapacidadPagoMensual = ObligacionesPrecalificado == 0 ? (IngresosReales * 40) / 100 : ((IngresosReales - ObligacionesPrecalificado) * 40) / 100;
                    //                }
                    //                lblIngresosPrecalificado.Text = IngresosReales.ToString("N");
                    //                lblObligacionesPrecalificado.Text = ObligacionesPrecalificado.ToString("N");
                    //                lblDisponiblePrecalificado.Text = CapacidadPagoMensual.ToString("N");
                    //                lblCapacidadPagoMensual.Text = CapacidadPagoMensual.ToString("N");
                    //                lblCapacidadPagoQuincenal.Text = (CapacidadPagoMensual / 2).ToString("N");
                    //            }
                    //        }
                    //    }

                    //    /* Verficar si la solicitud tiene condicionamientos pendientes */
                    //    using (SqlCommand sqlComando = new SqlCommand("sp_CANEX_Solicitud_Condiciones", sqlConexion))
                    //    {
                    //        sqlComando.CommandType = CommandType.StoredProcedure;
                    //        sqlComando.Parameters.AddWithValue("@piIDSesion", pcSesionID);
                    //        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    //        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    //        sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);

                    //        using (SqlDataReader reader = sqlComando.ExecuteReader())
                    //        {
                    //            if (reader.HasRows)
                    //            {
                    //                HtmlTableRow tRowSolicitudCondiciones = null;
                    //                string EstadoCondicion = String.Empty;
                    //                int contadorCondiciones = 1;
                    //                while (reader.Read())
                    //                {
                    //                    EstadoCondicion = (short)reader["fiEstadoCondicion"] != 0 ? "Completado" : "Pendiente";
                    //                    tRowSolicitudCondiciones = new HtmlTableRow();
                    //                    tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcCategoriaCondicion"].ToString() });
                    //                    tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcDescripcionCondicion"].ToString() });
                    //                    tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcObservaciones"].ToString() });
                    //                    tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = EstadoCondicion });
                    //                    tblListaSolicitudCondiciones.Rows.Add(tRowSolicitudCondiciones);
                    //                    tblListaSolicitudCondiciones.Rows[contadorCondiciones].Cells[3].Attributes.Add("class", EstadoCondicion == "Pendiente" ? "text-warning" : "text-success");
                    //                    contadorCondiciones++;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
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