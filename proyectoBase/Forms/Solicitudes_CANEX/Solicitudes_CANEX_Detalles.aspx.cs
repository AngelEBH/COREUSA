using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;
using System.Web.UI.HtmlControls;

public partial class Solicitudes_CANEX_Detalles : System.Web.UI.Page
{
    String pcEncriptado = "";
    string pcIDUsuario = "";
    string pcIDApp = "";
    string pcSesionID = "1";
    public short IDPais = 0;
    public short IDSocio = 0;
    public short IDAgencia = 0;
    public decimal IDEstadoSolicitud = 0;
    public int IDSolicitudPrestadito = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
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
                    IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcSesionID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    int IDPRODUCTO = 0;
                    decimal EstadoSolicitud = 0;
                    string IdentidadCLTE = string.Empty;

                    using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                    {
                        sqlConexion.Open();

                        /* Informacion de la solicitud */
                        using (SqlCommand sqlComando = new SqlCommand("sp_CANEX_Solicitud_Detalle", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcSesionID);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            using (SqlDataReader reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    IDPRODUCTO = (short)reader["fiIDProducto"];
                                    EstadoSolicitud = (decimal)reader["fiEstadoSolicitud"];
                                    IDEstadoSolicitud = EstadoSolicitud;
                                    IDPais = (short)reader["fiIDPais"];
                                    IDSocio = (short)reader["fiIDSocio"];
                                    IDAgencia = (short)reader["fiIDAgencia"];
                                    IDSolicitudPrestadito = (int)reader["fiIDSolicitudPrestadito"];
                                    string NombreLogo = IDPRODUCTO == 101 ? "iconoRecibirDinero48.png" : IDPRODUCTO == 201 ? "iconoMoto48.png" : IDPRODUCTO == 202 ? "iconoAuto48.png" : IDPRODUCTO == 301 ? "iconoConsumo48.png" : "iconoConsumo48.png";
                                    IdentidadCLTE = reader["fcIdentidad"].ToString();
                                    int IDSolicitud = (int)reader["fiIDSolicitudCANEX"];
                                    DateTime FechaNacimientoCliente = (DateTime)reader["fdNacimientoCliente"];
                                    decimal EsComisionista = (decimal)reader["fiComisionista"];

                                    /* Verificar si la solicitud estaba en estado "enviada" y hay que pasarla a estatus "en revision" */
                                    if (EstadoSolicitud == 2)
                                        CambiarEstadoAEnRevision(IDSolicitud);

                                    lblTipoPrestamo.Text = (string)reader["fcNombreProducto"];
                                    LogoPrestamo.ImageUrl = "/Imagenes/" + NombreLogo;
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
                                        lblNombreConyugue.Text = NombreCompletoConyugue.Replace(" ", " ");
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

                                    /* Habilitar/Inhabilitar botones de acciones */
                                    int EstadoRechazada = 5;
                                    int EstadoAprobada = 4;
                                    int EstadoEnRevision = 3;
                                    string Title = string.Empty;

                                    if (EstadoSolicitud == EstadoAprobada || EstadoSolicitud == EstadoRechazada)
                                    {
                                        string Desicion = EstadoSolicitud == EstadoAprobada ? "Aprobada" : "Rechazada";
                                        Title = "La solicitud ya fue " + Desicion;

                                        btnAceptarSolicitud.Disabled = true;
                                        btnRechazar.Disabled = true;
                                        btnCondicionarSolicitud.Disabled = true;
                                    }
                                    else if (IDSolicitudPrestadito != 0)
                                    {
                                        Title = "La solicitud ya fue importada";
                                        btnAceptarSolicitud.Disabled = true;
                                        btnRechazar.Disabled = true;
                                        btnCondicionarSolicitud.Disabled = true;
                                    }
                                    else if (EstadoSolicitud == EstadoEnRevision && IDSolicitudPrestadito == 0)
                                    {
                                        btnAceptarSolicitud.Disabled = false;
                                    }
                                    else if (EstadoSolicitud != EstadoRechazada && EstadoSolicitud != EstadoAprobada && IDSolicitudPrestadito == 0)
                                    {
                                        btnRechazar.Disabled = true;
                                    }

                                    btnAceptarSolicitud.Attributes.Add("title", Title);
                                    btnRechazar.Attributes.Add("title", Title);
                                    btnCondicionarSolicitud.Attributes.Add("title", Title);
                                }
                            }
                        }

                        /* Referencias del cliente */
                        using (SqlCommand sqlComando = new SqlCommand("sp_CANEX_Solicitud_Referencias", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcSesionID);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            using (SqlDataReader reader = sqlComando.ExecuteReader())
                            {
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
                            }
                        }
                        /* Informacion del precalificado */
                        using (SqlCommand sqlComando = new SqlCommand("EXEC CoreAnalitico.dbo.sp_info_ConsultaEjecutivos @piIDApp, @piIDUsuario, @pcIdentidad", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.Text;
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcIdentidad", IdentidadCLTE);
                            using (SqlDataReader reader = sqlComando.ExecuteReader())
                            {
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
                            }
                        }
                        /* Verficar si la solicitud tiene condicionamientos pendientes */
                        using (SqlCommand sqlComando = new SqlCommand("EXEC sp_CANEX_Solicitud_Condiciones @piIDSesion, @piIDApp, @piIDUsuario, @piIDSolicitud", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.Text;
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcSesionID);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                            using (SqlDataReader reader = sqlComando.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    pestanaListaSolicitudCondiciones.Style.Add("display", "");

                                    HtmlTableRow tRowSolicitudCondiciones = null;
                                    string EstadoCondicion = String.Empty;
                                    int contadorCondiciones = 1;
                                    while (reader.Read())
                                    {
                                        EstadoCondicion = (short)reader["fiEstadoCondicion"] != 0 ? "<label class='btn btn-sm btn-block btn-success mb-0'>Completado</label>" : "<label class='btn btn-sm btn-block btn-danger mb-0'>Pendiente</label>";
                                        tRowSolicitudCondiciones = new HtmlTableRow();
                                        tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcCategoriaCondicion"].ToString() });
                                        tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcDescripcionCondicion"].ToString() });
                                        tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerText = reader["fcObservaciones"].ToString() });
                                        tRowSolicitudCondiciones.Cells.Add(new HtmlTableCell() { InnerHtml = EstadoCondicion });
                                        tblListaSolicitudCondiciones.Rows.Add(tRowSolicitudCondiciones);
                                        contadorCondiciones++;
                                    }
                                }
                            }
                        }
                        /* Catalogo de condiciones */
                        string Comando = "EXEC sp_CANEX_Catalogo_Condiciones " + pcSesionID + "," + pcIDApp + "," + pcIDUsuario;
                        using (SqlDataAdapter AdapterDDLCondiciones = new SqlDataAdapter(Comando, sqlConexion))
                        {
                            DataTable dtCondiciones = new DataTable();
                            AdapterDDLCondiciones.Fill(dtCondiciones);
                            ddlCondiciones.DataSource = dtCondiciones;
                            ddlCondiciones.DataBind();
                            ddlCondiciones.DataTextField = "fcDescripcion";
                            ddlCondiciones.DataValueField = "fiID";
                            ddlCondiciones.DataBind();
                            dtCondiciones.Dispose();
                            AdapterDDLCondiciones.Dispose();
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

    private void CambiarEstadoAEnRevision(int IDSolicitud)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        const int EstadoEnRevision = 3;
        try
        {
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                string lcSQLInstruccion = "exec CoreFinanciero.dbo.sp_CANEX_Solicitud_CambiarEstado '" + pcSesionID + "', '" + pcIDApp + "','" + pcIDUsuario + "', '" + IDSolicitud + "', '" + IDPais + "', '" + IDSocio + "', '" + IDAgencia + "', '" + EstadoEnRevision + "'";
                using (SqlCommand sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion))
                {
                    sqlComando.CommandType = CommandType.Text;
                    sqlComando.ExecuteReader();
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    [WebMethod]
    public static List<SolicitudesDocumentosViewModel> CargarDocumentos(string dataCrypt)
    {
        List<SolicitudesDocumentosViewModel> ListadoDocumentos = new List<SolicitudesDocumentosViewModel>();
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int pcIDSesion = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                using (SqlCommand sqlComando = new SqlCommand("sp_CANEX_Solicitud_Documentos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
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

                            ListadoDocumentos.Add(new SolicitudesDocumentosViewModel()
                            {
                                fiIDSolicitudDocs = (short)reader["fiIDImagen"],
                                fcNombreArchivo = (string)reader["fcNombreImagen"],
                                URLArchivo = "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + IDSOL + "/" + fcNombreImagen,
                                fiTipoDocumento = (short)reader["fiIDImagen"]
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
        return ListadoDocumentos;
    }

    [WebMethod]
    public static bool CondicionarSolicitud(List<SolicitudesCondicionamientosViewModel> SolicitudCondiciones, int IDPais, int IDSocio, int IDAgencia, string dataCrypt)
    {
        bool resultadoProceso = false;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();

        using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
        {
            sqlConexion.Open();

            using (SqlTransaction transaccion = sqlConexion.BeginTransaction("insercionSolicitudCondiciones"))
            {
                try
                {
                    Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
                    int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                    int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
                    string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    long MensajeError = 0;

                    foreach (SolicitudesCondicionamientosViewModel item in SolicitudCondiciones)
                    {
                        using (SqlCommand sqlComandoList = new SqlCommand("sp_CANEX_Solicitud_Condicionar", sqlConexion, transaccion))
                        {
                            sqlComandoList.CommandType = CommandType.StoredProcedure;
                            sqlComandoList.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComandoList.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComandoList.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComandoList.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
                            sqlComandoList.Parameters.AddWithValue("@piIDPais", IDPais);
                            sqlComandoList.Parameters.AddWithValue("@piIDSocio", IDSocio);
                            sqlComandoList.Parameters.AddWithValue("@piIDAgencia", IDAgencia);
                            sqlComandoList.Parameters.AddWithValue("@IDCondicion", item.fiIDCondicion);
                            sqlComandoList.Parameters.AddWithValue("@Observaciones", item.fcComentarioAdicional);

                            using (SqlDataReader readerList = sqlComandoList.ExecuteReader())
                            {
                                while (readerList.Read())
                                    MensajeError = (long)readerList["RESULT"];

                                if (MensajeError == 1)
                                    resultadoProceso = true;
                                else
                                {
                                    resultadoProceso = false;
                                    break;
                                }
                            }
                        } // using cmdList
                    }

                    if (resultadoProceso == true)
                        transaccion.Commit();
                }
                catch (Exception ex)
                {
                    transaccion.Rollback();
                    ex.Message.ToString();
                }

            }// using transaction

        } // using connection

        return resultadoProceso;
    }

    [WebMethod]
    public static int SolicitudResolucion(int IDEstado, int IDPais, int IDSocio, int IDAgencia, string Comentario, string dataCrypt)
    {
        int resultadoProceso = 0;
        try
        {
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int pcIDSesion = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID"));
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            long MensajeError = 0;

            const int EstadoAprobado = 4;
            const int EstadoRechazado = 5;
            int Resolucion = IDEstado == 1 ? EstadoAprobado : EstadoRechazado;

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                string lcSQLInstruccion = "exec sp_CANEX_Solicitud_CambiarEstado '" + pcIDSesion + "', '" + pcIDApp + "','" + pcIDUsuario + "', '" + IDSOL + "', '" + IDPais + "', '" + IDSocio + "', '" + IDAgencia + "', '" + Resolucion + "'";
                using (SqlCommand sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion))
                {
                    sqlComando.CommandType = CommandType.Text;
                    sqlConexion.Open();
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                            MensajeError = (long)reader["RESULT"];

                        if (MensajeError == 1)
                            resultadoProceso = Convert.ToInt32(MensajeError);
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
    public static ResponseEntitie ImportarSolicitud(int IDPais, int IDSocio, int IDAgencia, string dataCrypt)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        ResponseEntitie resultadoProceso = new ResponseEntitie();
        List<SolicitudesDocumentosViewModel> ListadoDocumentosCANEX = new List<SolicitudesDocumentosViewModel>();

        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
            int pcIDSesion = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID"));
            int IDSOL = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL"));
            int pcIDApp = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp"));

            bool ResultadoSp = false;
            string fcMensajeResultado = string.Empty;
            string fcMensajeError = string.Empty;
            int IDSolicitudPrestadito = 0;
            int contadorErrores = 0;

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                string lcSQLInstruccion = "exec dbo.sp_CANEX_Solicitud_ImportarSolicitud " + pcIDSesion + ", " + pcIDApp + "," + pcIDUsuario + ", " + IDSOL + ", " + IDPais + ", " + IDSocio + ", " + IDAgencia;

                using (SqlCommand sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion))
                {
                    sqlComando.CommandType = CommandType.Text;
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ResultadoSp = (bool)reader["fbTipoResultado"];
                            fcMensajeResultado = (string)reader["fcMensajeResultado"];
                            fcMensajeError = (string)reader["fcMensajeError"];
                            IDSolicitudPrestadito = (int)reader["fiIDSolicitud"];
                        }
                    }
                }

                /* si se importó todo correctamente, guardar la documentacion */
                if (ResultadoSp == true)
                {
                    #region REGISTRAR DOCUMENTACIÓN DE LA SOLICITUD
                    using (SqlCommand sqlComando = new SqlCommand("sp_CANEX_Solicitud_Documentos", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDSolicitud", IDSOL);
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

                                /* Lista de documentos canex, que se deben mover a la carpeta de documentos de solicitudes */
                                ListadoDocumentosCANEX.Add(new SolicitudesDocumentosViewModel()
                                {
                                    fiIDSolicitudDocs = (short)reader["fiIDImagen"],
                                    NombreAntiguo = (string)reader["fcNombreImagen"],
                                    URLAntiguoArchivo = "http://canex.miprestadito.com/documentos/" + fcNombreSocio + "/SOL_" + IDSOL + "/" + fcNombreImagen,
                                    fiTipoDocumento = (short)reader["fiIDImagen"]
                                });
                            }
                        }
                    }// using cm

                    int DocumentacionCliente = 1;
                    int DocumentacionAval = 2;
                    int TipoDocumentacion = 0;

                    int[] IDSDocumentosAval = new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 20, 21 };
                    int[] IDSDocumentosCliente = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 18, 19 };

                    if (IDSDocumentosAval.Contains(DocumentacionAval))
                    {
                        TipoDocumentacion = DocumentacionAval;
                    }
                    else
                    {
                        TipoDocumentacion = DocumentacionCliente;
                    }

                    string NombreCarpetaDocumentos = "Solicitud" + IDSolicitudPrestadito;
                    string NuevoNombreDocumento = "";

                    /* lista de documentos de la solicitud de canex */
                    var listaDocumentos = ListadoDocumentosCANEX;

                    /* Lista de documentos que se va registrar en la base de datos de credito y se va mover al nuevo directorio */
                    List<SolicitudesDocumentosViewModel> SolicitudesDocumentos = new List<SolicitudesDocumentosViewModel>();


                    if (listaDocumentos != null)
                    {
                        /* lista de bloques y la cantidad de documentos que contiene cada uno */
                        var Bloques = listaDocumentos.GroupBy(TipoDocumento => TipoDocumento.fiTipoDocumento).Select(x => new { x.Key, Count = x.Count() });

                        /* lista donde se guardara temporalmente los documentos dependiendo del tipo de documento en el iterador */
                        List<SolicitudesDocumentosViewModel> DocumentosBloque = new List<SolicitudesDocumentosViewModel>();

                        string NombreCarpetaDocumentosCANEX = "Solicitud" + IDSolicitudPrestadito;
                        string DirectorioDocumentosSolicitudCANEX = @"C:\inetpub\wwwroot\Documentos\Solicitudes\" + NombreCarpetaDocumentosCANEX + "\\";

                        foreach (var Bloque in Bloques)
                        {
                            int TipoDocumento = (int)Bloque.Key;
                            int CantidadDocumentos = Bloque.Count;

                            DocumentosBloque = listaDocumentos.Where(x => x.fiTipoDocumento == TipoDocumento).ToList();// documentos de este bloque
                            String[] NombresGenerador = Funciones.MultiNombres.GenerarNombreCredDocumento(DocumentacionCliente, IDSolicitudPrestadito, TipoDocumento, CantidadDocumentos);

                            int ContadorNombre = 0;
                            foreach (SolicitudesDocumentosViewModel file in DocumentosBloque)
                            {
                                NuevoNombreDocumento = NombresGenerador[ContadorNombre];
                                SolicitudesDocumentos.Add(new SolicitudesDocumentosViewModel()
                                {
                                    fcNombreArchivo = NuevoNombreDocumento,
                                    NombreAntiguo = file.NombreAntiguo,
                                    fcRutaArchivo = DirectorioDocumentosSolicitudCANEX,
                                    URLArchivo = "/Documentos/Solicitudes/" + NombreCarpetaDocumentos + "/" + NuevoNombreDocumento + ".png",
                                    URLAntiguoArchivo = file.URLAntiguoArchivo,
                                    fiTipoDocumento = file.fiTipoDocumento
                                });
                                ContadorNombre++;
                            }
                        }
                    }
                    else
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al registrar la documentación";
                        return resultadoProceso;
                    }
                    if (SolicitudesDocumentos.Count <= 0)
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar documentación, compruebe que los documentos se hayan cargado correctamente";
                        return resultadoProceso;
                    }
                    foreach (SolicitudesDocumentosViewModel documento in SolicitudesDocumentos)
                    {
                        using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Documentos_Insert", sqlConexion))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitudPrestadito);
                            sqlComando.Parameters.AddWithValue("@fcNombreArchivo", documento.fcNombreArchivo);
                            sqlComando.Parameters.AddWithValue("@fcTipoArchivo", ".png");
                            sqlComando.Parameters.AddWithValue("@fcRutaArchivo", documento.fcRutaArchivo);
                            sqlComando.Parameters.AddWithValue("@fcURL", documento.URLArchivo);
                            sqlComando.Parameters.AddWithValue("@fiTipoDocumento", documento.fiTipoDocumento);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", string.Empty);
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);
                            using (SqlDataReader reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    if (reader["MensajeError"].ToString().StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al registrar documentación de la solicitud";
                            return resultadoProceso;
                        }
                    }
                    /* Mover documentos al directorio de la solicitud */
                    if (!ImportarDocumentosCANEX(IDSolicitudPrestadito, SolicitudesDocumentos))
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar la documentación de la solicitud";
                        return resultadoProceso;
                    }

                    #endregion

                } // if SP == true

            } // using connection

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultadoProceso;
    }

    [WebMethod]
    public static string ObtenerUrlEncriptado(string dataCrypt)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
        int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
        string pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("pcID").ToString();
        string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp").ToString();

        string parametrosEncriptados = DSC.Encriptar("usr=" + pcIDUsuario + "&ID=" + pcID + "&IDApp=" + pcIDApp);
        return parametrosEncriptados;
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

    /* Descargar y guardar los documentos de la solicitud en su respectiva carpeta de documentos */
    public static bool ImportarDocumentosCANEX(int IDSolicitud, List<SolicitudesDocumentosViewModel> ListaDocumentos)
    {
        bool result;
        try
        {
            WebClient client = new WebClient();
            MD5 md5ArchivoDescargado = MD5.Create();

            if (ListaDocumentos != null)
            {
                /* CREAR EL NUEVO DIRECTORIO PARA LOS DOCUMENTOS DE LA SOLICITUD */
                string NombreCarpetaDocumentos = "Solicitud" + IDSolicitud;
                string DirectorioDocumentosSolicitud = @"C:\inetpub\wwwroot\Documentos\Solicitudes\" + NombreCarpetaDocumentos + "\\";
                bool CarpetaExistente = System.IO.Directory.Exists(DirectorioDocumentosSolicitud);

                if (!CarpetaExistente)
                    System.IO.Directory.CreateDirectory(DirectorioDocumentosSolicitud);

                foreach (SolicitudesDocumentosViewModel Documento in ListaDocumentos)
                {
                    string ViejoDirectorio = Documento.URLAntiguoArchivo;
                    string NuevoNombreDocumento = Documento.fcNombreArchivo;
                    string NuevoDirectorio = DirectorioDocumentosSolicitud + NuevoNombreDocumento + ".png";

                    if (File.Exists(NuevoDirectorio))
                        File.Delete(NuevoDirectorio);

                    if (!System.IO.File.Exists(NuevoDirectorio))
                    {
                        string lcURL = Documento.URLAntiguoArchivo;

                        client.DownloadFile(new Uri(lcURL), NuevoDirectorio);
                        client.Dispose();
                        client = null;
                        client = new WebClient();
                    }
                }
            }
            result = true;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            result = false;
        }
        return result;
    }
}