using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;

public partial class SolicitudesCANEX_SeguimientoDetalles : System.Web.UI.Page
{
    String pcEncriptado = "";
    string pcIDUsuario = "";
    string pcIDApp = "";
    string pcSesionID = "1";
    public short IDPais = 0;
    public short IDSocio = 0;
    public short IDAgencia = 0;
    public decimal IDEstadoSolicitud = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            SqlConnection sqlConexion = null;
            SqlDataReader reader = null;
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            try
            {
                string lcURL = "";
                int liParamStart = 0;
                string lcParametros = "";
                string lcParametroDesencriptado = "";
                Uri lURLDesencriptado = null;
                int IDSOL = 0;
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
                    IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcSesionID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    int IDPRODUCTO = 0;
                    decimal EstadoSolicitud = 0;

                    string sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
                    sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

                    /* Informacion de la solicitud */
                    SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CANEX_Solicitud_Detalle", sqlConexion);
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcSesionID);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlConexion.Open(); // Abrir conexion
                    reader = sqlComando.ExecuteReader();

                    while (reader.Read())
                    {
                        IDPRODUCTO = (short)reader["fiIDProducto"];
                        EstadoSolicitud = (decimal)reader["fiEstadoSolicitud"];
                        IDEstadoSolicitud = EstadoSolicitud;
                        IDPais = (short)reader["fiIDPais"];
                        IDSocio = (short)reader["fiIDSocio"];
                        IDAgencia = (short)reader["fiIDAgencia"];
                        string NombreLogo = IDPRODUCTO == 101 ? "iconoRecibirDinero48.png" : IDPRODUCTO == 201 ? "iconoMoto48.png" : IDPRODUCTO == 202 ? "iconoAuto48.png" : IDPRODUCTO == 301 ? "iconoConsumo48.png" : "iconoConsumo48.png";
                        string IdentidadCLTE = reader["fcIdentidad"].ToString();
                        int IDSolicitud = (int)reader["fiIDSolicitudCANEX"];
                        DateTime FechaNacimientoCliente = (DateTime)reader["fdNacimientoCliente"];
                        decimal EsComisionista = (decimal)reader["fiComisionista"];

                        lblTipoPrestamo.Text = (string)reader["fcNombreProducto"];
                        LogoPrestamo.ImageUrl = "http://172.20.3.140/Imagenes/" + NombreLogo;
                        spanNombreCliente.Text = reader["fcNombreCliente"].ToString();
                        spanIdentidadCliente.Text = IdentidadCLTE;
                        lblNoSolicitud.Text = reader["fiIDSolicitudCANEX"].ToString();
                        lblNoCliente.Text = reader["fiIDCliente"].ToString();
                        lblTipoSolicitud.Text = "Solicitud CANEX";
                        lblAgenteDeVentas.Text = reader["fcNombreUsuario"].ToString();
                        lblAgencia.Text = reader["fcNombreAgencia"].ToString();
                        lblEstadoSolicitud.Text = reader["fcEstadoSolicitud"].ToString();
                        lblRtnCliente.Text = reader["fcRTN"].ToString();
                        lblNumeroTelefono.NavigateUrl = "tel:" + reader["fcTelefonoPrimario"].ToString();
                        lblNumeroTelefono.Text = reader["fcTelefonoPrimario"].ToString();
                        lblNumeroTelefonoAlternativo.NavigateUrl = "tel:" + reader["fcTelefonoAlternativo"].ToString();
                        lblNumeroTelefonoAlternativo.Text = reader["fcTelefonoAlternativo"].ToString();
                        lblNacionalidad.Text = reader["fcNacionalidadCliente"].ToString();
                        /* Calcular edad del cliente */
                        var hoy = DateTime.Today;
                        var edad = hoy.Year - FechaNacimientoCliente.Year;
                        if (FechaNacimientoCliente.Date > hoy.AddYears(-edad)) edad--;
                        lblFechaNacimientoCliente.Text = FechaNacimientoCliente.ToString("MM/dd/yyyy");
                        lblEdadCliente.Text = edad.ToString();
                        lblProfesionCliente.Text = reader["fcProfesionuOficio"].ToString();
                        lblSexoCliente.Text = reader["fcSexoCliente"].ToString();
                        lblEstadoCivilCliente.Text = reader["fcEstadoCivilCliente"].ToString();
                        lblViviendaCliente.Text = reader["fcTipoResidenciaCliente"].ToString();
                        lblTiempoResidirCliente.Text = "n/a";
                        lblDeptoCliente.Text = reader["fcDepartamentoDomicilio"].ToString();
                        lblMunicipioCliente.Text = reader["fcMunicipioDomicilio"].ToString();
                        lblCiudadCliente.Text = reader["fcCiudadDomicilio"].ToString();
                        lblBarrioColoniaCliente.Text = reader["fcBarrioColoniaDomicilio"].ToString();
                        lblDireccionDetalladaCliente.Text = reader["fcDireccionDetallada"].ToString();
                        lblReferenciaDomicilioCliente.Text = reader["fcDireccionReferencias"].ToString();
                        String NombreCompletoConyugue = reader["fcPrimerNombreConyugue"].ToString() + reader["fcSegundoNombreConyugue"].ToString() + reader["fcPrimerApellidoConyugue"].ToString() + reader["fcSegundoApellidoConyugue"].ToString();
                        if (string.IsNullOrEmpty(reader["fcPrimerNombreConyugue"].ToString()))
                        {
                            divConyugueCliente.Visible = false;
                        }
                        else
                        {
                            lblNombreConyugue.Text = NombreCompletoConyugue.Replace("  ", " ");
                            DateTime FechaNacimientoConyugue = (DateTime)reader["fdNacimientoCliente"];
                            lblFechaNacimientoConygue.Text = FechaNacimientoConyugue.ToString("MM/dd/yyyy");
                            lblTelefonoConyugue.NavigateUrl = "tel:" + reader["fcTelefonoConyugue"].ToString();
                            lblTelefonoConyugue.Text = reader["fcTelefonoConyugue"].ToString();
                            lblOcupacionConyugue.Text = reader["fcOcupacionConyugue"].ToString();
                            lblProfesionOficioConyugue.Text = reader["fcProfesionuOficioConyugue"].ToString();
                            lblPuestoAsignadoConyugue.Text = reader["fcPuestoConyugue"].ToString();
                            lblLugarTrabajoConyugue.Text = reader["fcNombreEmpresaConyugue"].ToString();
                        }
                        lblNombreTrabajoCliente.Text = reader["fcNombreEmpresa"].ToString();
                        lblIngresosMensualesCliente.Text = reader["fnIngresoMensualBase"].ToString();
                        if (EsComisionista == 0)
                        {
                            lblComisionesClienteTitulo.Visible = false;
                            lblComisionesCliente.Visible = false;
                        }
                        else
                            lblComisionesCliente.Text = reader["fnIngresoMensualComisiones"].ToString();
                        lblPuestoAsignadoCliente.Text = reader["fcPuesto"].ToString();
                        lblTelefonoEmpresaCliente.NavigateUrl = "tel:" + reader["fcTelefonoEmpresa"].ToString();
                        lblTelefonoEmpresaCliente.Text = reader["fcTelefonoEmpresa"].ToString();
                        lblExtension.Text = reader["fcExtension"].ToString();
                        lblDeptoEmpresa.Text = reader["fcDepartamentoEmpresa"].ToString();
                        lblMunicipioEmpresa.Text = reader["fcMunicipioEmpresa"].ToString();
                        lblCiudadEmpresa.Text = reader["fcCiudadEmpresa"].ToString();
                        lblBarrioColoniaEmpresa.Text = reader["fcBarrioColoniaEmpresa"].ToString();
                        lblDireccionDetalladaEmpresa.Text = reader["fcDireccionDetalladaEmpresa"].ToString();
                        lblReferenciaUbicacionEmpresa.Text = reader["fcDireccionReferenciasEmpresa"].ToString();
                        /* Informacion del préstamo requerido */
                        lblValorGlobal.Text = ((decimal)reader["fnValorGlobal"]).ToString("N");
                        lblValorPrima.Text = ((decimal)reader["fnValorPrima"]).ToString("N");
                        lblPlazoTitulo.Text = "Plazo " + reader["fcTipodeCuota"].ToString();
                        lblPlazo.Text = reader["fcPlazo"].ToString();
                        lblMontoFinanciar.Text = ((decimal)reader["fnValorPrestamo"]).ToString("N");
                        if (reader != null)
                            reader.Close();

                        /* Referencias del cliente */
                        sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CANEX_Solicitud_Referencias", sqlConexion);
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcSesionID);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        reader = sqlComando.ExecuteReader();
                        /* Llenar table de referencias */
                        HtmlTableRow tRowReferencias = null;
                        while (reader.Read())
                        {
                            tRowReferencias = new HtmlTableRow();
                            tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcNombre"].ToString() });
                            tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcTrabajo"].ToString() });
                            tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcTiempoConocerlo"].ToString() });
                            tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcTelefono"].ToString() });
                            tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = reader["fcParentesco"].ToString() });
                            tblReferencias.Rows.Add(tRowReferencias);
                        }
                        if (reader != null)
                            reader.Close();

                        /* Informacion del precalificado */
                        sqlComando = new SqlCommand("EXEC CoreAnalitico.dbo.sp_info_ConsultaEjecutivos @piIDApp, @piIDUsuario, @pcIdentidad", sqlConexion);
                        sqlComando.CommandType = CommandType.Text;
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcIdentidad", IdentidadCLTE);
                        reader = sqlComando.ExecuteReader(); ;
                        while (reader.Read())
                        {
                            decimal ObligacionesPrecalificado = (decimal)reader["fnTotalObligaciones"];
                            decimal IngresosReales = (decimal)reader["fnIngresos"];
                            decimal CapacidadPagoMensual = 0;
                            if (IDPRODUCTO == 101)
                            {
                                CapacidadPagoMensual = ObligacionesPrecalificado == 0 ? (IngresosReales * 13) / 100 : ((IngresosReales - ObligacionesPrecalificado) * 13) / 100;
                            }
                            else if (IDPRODUCTO == 201)
                            {
                                CapacidadPagoMensual = ObligacionesPrecalificado == 0 ? (IngresosReales * 30) / 100 : ((IngresosReales - ObligacionesPrecalificado) * 30) / 100;
                            }
                            else if (IDPRODUCTO == 202)
                            {
                                CapacidadPagoMensual = ObligacionesPrecalificado == 0 ? (IngresosReales * 40) / 100 : ((IngresosReales - ObligacionesPrecalificado) * 40) / 100;
                            }
                            lblIngresosPrecalificado.Text = IngresosReales.ToString("N");
                            lblObligacionesPrecalificado.Text = ObligacionesPrecalificado.ToString("N");
                            lblDisponiblePrecalificado.Text = CapacidadPagoMensual.ToString("N");
                            lblCapacidadPagoMensual.Text = CapacidadPagoMensual.ToString("N");
                            lblCapacidadPagoQuincenal.Text = (CapacidadPagoMensual / 2).ToString("N");
                        }

                        if (reader != null)
                            reader.Close();

                        /* Verficar si la solicitud tiene condicionamientos pendientes */
                        sqlComando = new SqlCommand("EXEC CoreFinanciero.dbo.sp_CANEX_Solicitud_Condiciones @piIDSesion, @piIDApp, @piIDUsuario, @piIDSolicitud", sqlConexion);
                        sqlComando.CommandType = CommandType.Text;
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcSesionID);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                        reader = sqlComando.ExecuteReader(); ;
                        if (reader.HasRows)
                        {
                            HtmlTableRow tRowSolicitudCondiciones = null;
                            string EstadoCondicion = String.Empty;
                            int contadorCondiciones = 1;
                            while (reader.Read())
                            {
                                EstadoCondicion = (short)reader["fiEstadoCondicion"] != 0 ? "Completado" : "Pendiente";
                                tRowSolicitudCondiciones = new HtmlTableRow();
                                tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcCategoriaCondicion"].ToString() });
                                tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcDescripcionCondicion"].ToString() });
                                tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcObservaciones"].ToString() });
                                tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = EstadoCondicion });
                                tblListaSolicitudCondiciones.Rows.Add(tRowSolicitudCondiciones);
                                tblListaSolicitudCondiciones.Rows[contadorCondiciones].Cells[3].Attributes.Add("class", EstadoCondicion == "Pendiente" ? "text-warning" : "text-success");
                                contadorCondiciones++;
                            }
                            btnListaCondiciones.Visible = true;
                        }
                        if (reader != null)
                            reader.Close();
                    }
                    sqlComando.Dispose();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                if (sqlConexion != null)
                {
                    if (sqlConexion.State == ConnectionState.Open)
                        sqlConexion.Close();
                }
                if (reader != null)
                    reader.Close();
            }
        }
    }

    [WebMethod]
    public static List<SolicitudesDocumentosViewModel> CargarDocumentos()
    {
        List<SolicitudesDocumentosViewModel> ListadoDocumentos = new List<SolicitudesDocumentosViewModel>();
        SqlDataReader reader = null;
        SqlConnection sqlConexion = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            string lcURL = HttpContext.Current.Request.Url.ToString();
            Uri lURLDesencriptado = DesencriptarURL(lcURL);
            int IDUSR = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDSesion = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            string sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CANEX_Solicitud_Documentos", sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", IDUSR);
            sqlConexion.Open();
            reader = sqlComando.ExecuteReader();
            string fcNombreSocio = "";
            string fcNombreImagen = "";
            while (reader.Read())
            {
                fcNombreSocio = reader["fcNombreSocio"].ToString();
                fcNombreImagen = reader["fcNombreImagen"].ToString();

                ListadoDocumentos.Add(new SolicitudesDocumentosViewModel()
                {
                    fiIDSolicitudDocs = (short)reader["fiIDImagen"],
                    fcNombreArchivo = (string)reader["fcNombreImagen"],
                    URLArchivo = "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + IDSOL + "/" + fcNombreImagen,
                    fiTipoDocumento = (short)reader["fiIDImagen"]
                });
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        finally
        {
            if (sqlConexion != null)
            {
                if (sqlConexion.State == ConnectionState.Open)
                    sqlConexion.Close();
            }
            if (reader != null)
                reader.Close();
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