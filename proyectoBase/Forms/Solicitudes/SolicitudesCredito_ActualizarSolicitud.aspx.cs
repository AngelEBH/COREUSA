using adminfiles;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class SolicitudesCredito_ActualizarSolicitud : System.Web.UI.Page
{
    #region Propiedades

    private string pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDSesion = "";
    public string IdSolicitud = "";
    public string IdCliente = "";
    private string pcIDApp = "";
    public string pcID = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    public SolicitudesCredito_ActualizarSolicitud_Precalificado_ViewModel Precalificado;
    public List<CoreFinanciero_TipoDocumento_ViewModel> DocumentosRequeridos = new List<CoreFinanciero_TipoDocumento_ViewModel>();
    public string jsonPrecalicado;
    public int IdProducto { get; set; }

    #endregion

    #region Page_Load, CargarListas, CargarInformacion, CargarPrecalificado

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        if (!IsPostBack && type == null)
        {
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            string lcParametros;

            Precalificado = new SolicitudesCredito_ActualizarSolicitud_Precalificado_ViewModel();

            if (liParamStart > 0)
                lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));

                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                IdSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

                HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
                Session.Timeout = 10080;

                CargarListas();
                CargarInformacion();
                CargarPrecalificado();
                jsonPrecalicado = JsonConvert.SerializeObject(Precalificado);
            }
            else
            {
                string lcScript = "window.open('SolicitudesCredito_Ingresadas.aspx?" + pcEncriptado + "','_self')";
                Response.Write("<script>");
                Response.Write(lcScript);
                Response.Write("</script>");
            }
        }

        /* Validar si es un postback para guardar documentos */
        if (type != null || Request.HttpMethod == "POST")
        {
            var uploadDir = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";
            var tipoDocumento = Convert.ToInt32(Request.QueryString["doc"]);
            Session["tipoDoc"] = tipoDocumento;

            var fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
{ "limit", 1 },
{ "title", "auto" },
{ "uploadDir", uploadDir }
});

            switch (type)
            {
                case "upload":
                    var data = fileUploader.Upload();

                    if (data["files"].Count == 1)
                        data["files"][0].Remove("file");
                    Response.Write(JsonConvert.SerializeObject(data));
                    break;

                case "remove":
                    string file = Request.Form["file"];
                    if (file != null)
                    {
                        file = FileUploader.FullDirectory(uploadDir) + file;
                        if (File.Exists(file))
                            File.Delete(file);
                    }
                    break;
            }
            Response.End();
        }
    }

    public void CargarListas()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitud_Guardar_LlenarListas", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /**** Departamentos ****/
                        ddlDepartamentoDomicilio.Items.Clear();
                        ddlDepartamentoDomicilio.Items.Add(new ListItem("Seleccionar", ""));
                        ddlDepartamentoEmpresa.Items.Clear();
                        ddlDepartamentoEmpresa.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlDepartamentoDomicilio.Items.Add(new ListItem(sqlResultado["fcDepartamento"].ToString(), sqlResultado["fiCodDepartamento"].ToString()));
                            ddlDepartamentoEmpresa.Items.Add(new ListItem(sqlResultado["fcDepartamento"].ToString(), sqlResultado["fiCodDepartamento"].ToString()));
                        }

                        ddlMunicipioDomicilio.Items.Clear();
                        ddlMunicipioDomicilio.Items.Add(new ListItem("Seleccione un departamento", ""));
                        ddlMunicipioEmpresa.Items.Clear();
                        ddlMunicipioEmpresa.Items.Add(new ListItem("Seleccione un departamento", ""));

                        ddlCiudadPobladoDomicilio.Items.Clear();
                        ddlCiudadPobladoDomicilio.Items.Add(new ListItem("Seleccione un municipio", ""));
                        ddlCiudadPobladoEmpresa.Items.Clear();
                        ddlCiudadPobladoEmpresa.Items.Add(new ListItem("Seleccione un municipio", ""));

                        ddlBarrioColoniaDomicilio.Items.Clear();
                        ddlBarrioColoniaDomicilio.Items.Add(new ListItem("Seleccione una ciudad/poblado", ""));
                        ddlBarrioColoniaEmpresa.Items.Clear();
                        ddlBarrioColoniaEmpresa.Items.Add(new ListItem("Seleccione una ciudad/poblado", ""));

                        /**** Viviendas ****/
                        sqlResultado.NextResult();

                        ddlTipoDeVivienda.Items.Clear();
                        ddlTipoDeVivienda.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlTipoDeVivienda.Items.Add(new ListItem(sqlResultado["fcDescripcionVivienda"].ToString(), sqlResultado["fiIDVivienda"].ToString()));
                        }

                        /**** Estados civiles ****/
                        sqlResultado.NextResult();

                        ddlEstadoCivil.Items.Clear();
                        ddlEstadoCivil.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            var estadoCivil = new ListItem(sqlResultado["fcDescripcionEstadoCivil"].ToString(), sqlResultado["fiIDEstadoCivil"].ToString());
                            estadoCivil.Attributes.Add("data-requiereinformacionconyugal", bool.Parse(sqlResultado["fbRequiereInformacionConyugal"].ToString()).ToString().ToLower());
                            ddlEstadoCivil.Items.Add(estadoCivil);
                        }

                        /**** Nacionalidades ****/
                        sqlResultado.NextResult();

                        ddlNacionalidad.Items.Clear();
                        ddlNacionalidad.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlNacionalidad.Items.Add(new ListItem(sqlResultado["fcDescripcionNacionalidad"].ToString(), sqlResultado["fiIDNacionalidad"].ToString()));
                        }

                        /**** Tipos de documentos ****/
                        sqlResultado.NextResult();

                        while (sqlResultado.Read())
                        {
                            DocumentosRequeridos.Add(new CoreFinanciero_TipoDocumento_ViewModel()
                            {
                                IdTipoDocumento = (short)sqlResultado["fiIDTipoDocumento"],
                                DescripcionTipoDocumento = sqlResultado["fcDescripcionTipoDocumento"].ToString(),
                                CantidadMaximaDoucmentos = (byte)sqlResultado["fiCantidadDocumentos"],
                                TipoVisibilidad = (byte)sqlResultado["fiTipodeVisibilidad"]
                            });
                        }
                        Session["DocumentosRequeridos"] = DocumentosRequeridos;

                        /**** Parentescos ****/
                        sqlResultado.NextResult();

                        ddlParentescos.Items.Clear();
                        ddlParentescos.Items.Add(new ListItem("Seleccionar", ""));

                        ddlParentescos_Editar.Items.Clear();
                        ddlParentescos_Editar.Items.Add(new ListItem("Seleccionar", ""));

                        while (sqlResultado.Read())
                        {
                            ddlParentescos.Items.Add(new ListItem(sqlResultado["fcDescripcionParentesco"].ToString(), sqlResultado["fiIDParentesco"].ToString()));
                            ddlParentescos_Editar.Items.Add(new ListItem(sqlResultado["fcDescripcionParentesco"].ToString(), sqlResultado["fiIDParentesco"].ToString()));
                        }

                        /**** Tiempo de residir ****/
                        sqlResultado.NextResult();

                        ddlTiempoDeResidir.Items.Clear();
                        ddlTiempoDeResidir.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlTiempoDeResidir.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeResidir"].ToString()));
                        }

                        /**** Tiempo de conocer referencia ****/
                        sqlResultado.NextResult();

                        ddlTiempoDeConocerReferencia.Items.Clear();
                        ddlTiempoDeConocerReferencia.Items.Add(new ListItem("Seleccionar", ""));

                        ddlTiempoDeConocerReferencia_Editar.Items.Clear();
                        ddlTiempoDeConocerReferencia_Editar.Items.Add(new ListItem("Seleccionar", ""));

                        while (sqlResultado.Read())
                        {
                            ddlTiempoDeConocerReferencia.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeConocer"].ToString()));
                            ddlTiempoDeConocerReferencia_Editar.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeConocer"].ToString()));
                        }

                        /**** Moneda ****/
                        sqlResultado.NextResult();

                        /*
                        ddlMoneda.Items.Clear();
                        ddlMoneda.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                        ddlMoneda.Items.Add(new ListItem(sqlResultado["fcNombreMoneda"].ToString(), sqlResultado["fiMoneda"].ToString()));
                        }
                        */

                        /**** Tipo de cliente ****/
                        sqlResultado.NextResult();

                        ddlTipoDeCliente.Items.Clear();
                        ddlTipoDeCliente.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlTipoDeCliente.Items.Add(new ListItem(sqlResultado["fcTipoCliente"].ToString(), sqlResultado["fiTipoCliente"].ToString()));
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

    public void CargarInformacion()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_SolicitudClientePorIdSolicitud", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", IdSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.CommandType = CommandType.StoredProcedure;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            /****** Informacion de la solicitud ******/
                            IdCliente = sqlResultado["fiIDCliente"].ToString();

                            IdProducto = (int)sqlResultado["fiIDProducto"];

                            lblTipoDePlazo.InnerText = "quincenal";

                            switch (IdProducto)
                            {
                                case 202:
                                case 204:

                                    lblTituloMontoPrestmo.Text = "Valor del vehiculo";
                                    divCotizadorAutos.Visible = true;
                                    ddlTipoGastosDeCierre.Enabled = true;
                                    ddlTipoDeSeguro.Enabled = true;
                                    ddlGps.Enabled = true;
                                    txtValorPrima.Enabled = true;

                                    ddlTipoGastosDeCierre.Items.Add(new ListItem("Seleccionar", ""));
                                    ddlTipoGastosDeCierre.Items.Add("Financiado");
                                    ddlTipoGastosDeCierre.Items.Add("Sin financiar");

                                    ddlGps.Items.Add(new ListItem("Seleccionar", ""));
                                    ddlGps.Items.Add("No");
                                    ddlGps.Items.Add("Si - CPI");
                                    ddlGps.Items.Add("Si - CableColor");

                                    ddlTipoDeSeguro.Items.Add(new ListItem("Seleccionar", ""));
                                    ddlTipoDeSeguro.Items.Add("A - Full Cover");
                                    ddlTipoDeSeguro.Items.Add("B - Basico + Garantía");
                                    ddlTipoDeSeguro.Items.Add("C - Basico");

                                    lblTipoDePlazo.InnerText = "mensual";
                                    break;

                                case 203:

                                    lblTituloMontoPrestmo.Text = "Valor del vehiculo";
                                    lblTituloPrima.InnerText = "Valor del empeño";
                                    txtValorPrima.Enabled = true;

                                    divCotizadorAutos.Visible = true;
                                    ddlTipoGastosDeCierre.Enabled = true;
                                    ddlTipoDeSeguro.Enabled = true;
                                    ddlGps.Enabled = true;

                                    ddlTipoGastosDeCierre.Items.Add(new ListItem("Seleccionar", ""));
                                    ddlTipoGastosDeCierre.Items.Add("Financiado");
                                    ddlTipoGastosDeCierre.Items.Add("Sin financiar");

                                    ddlGps.Items.Add(new ListItem("Seleccionar", ""));
                                    ddlGps.Items.Add("No");
                                    ddlGps.Items.Add("Si - CPI");
                                    ddlGps.Items.Add("Si - CableColor");

                                    ddlTipoDeSeguro.Items.Add(new ListItem("Seleccionar", ""));
                                    ddlTipoDeSeguro.Items.Add("A - Full Cover");
                                    ddlTipoDeSeguro.Items.Add("B - Basico + Garantía");
                                    ddlTipoDeSeguro.Items.Add("C - Basico");

                                    lblTipoDePlazo.InnerText = "mensual";

                                    break;

                                case 101:
                                    lblTituloMontoPrestmo.Text = "Valor de efectivo solicitado";
                                    txtValorPrima.Enabled = false;
                                    break;

                                case 201:
                                    lblTituloMontoPrestmo.Text = "Valor de la moto";
                                    txtValorPrima.Enabled = true;
                                    break;

                                case 301:
                                case 302:
                                    lblTituloMontoPrestmo.Text = "Valor del artículo";
                                    txtValorPrima.Enabled = true;
                                    break;
                            }


                            /****** Documentos de la solicitud ******/
                            sqlResultado.NextResult();

                            /****** Condicionamientos de la solicitud ******/
                            sqlResultado.NextResult();

                            if (sqlResultado.HasRows)
                            {
                                var listaCondiciones = new List<int>();

                                var listaCondicionesDeDocumentacion = new int[] { 1, 2, 3, 4, 5, 6 };

                                HtmlTableRow tRowCondicion;

                                HtmlTableRow tRowCondicionAcciones;

                                var btnFinalizarCondicion = string.Empty;
                                var lblEstadoCondicion = string.Empty;
                                bool estadoCondicion;

                                /* Llenar table de referencias */
                                while (sqlResultado.Read())
                                {
                                    estadoCondicion = (bool)sqlResultado["fbEstadoCondicion"];
                                    btnFinalizarCondicion = (bool)sqlResultado["fbEstadoCondicion"] == true ? "<button id='btnFinalizarCondicion' data-id='" + sqlResultado["fiIDSolicitudCondicion"].ToString() + "' data-idtipocondicion='" + sqlResultado["fiIDCondicion"].ToString() + "' class='btn btn-sm btn-warning mb-0' type='button' title='Finalizar condicion'>Finalizar</button>" : "";
                                    lblEstadoCondicion = (bool)sqlResultado["fbEstadoCondicion"] == true ? "<label class='btn btn-sm btn-warning mb-0'>Pendiente<label>" : "<label class='btn btn-sm btn-success mb-0'>Completada<label>";

                                    tRowCondicion = new HtmlTableRow();
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcCondicion"].ToString() });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcDescripcionCondicion"].ToString() });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcComentarioAdicional"].ToString() });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerHtml = lblEstadoCondicion });

                                    tblCondiciones.Rows.Add(tRowCondicion);

                                    tRowCondicionAcciones = new HtmlTableRow();
                                    tRowCondicionAcciones.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcCondicion"].ToString() });
                                    tRowCondicionAcciones.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcDescripcionCondicion"].ToString() });
                                    tRowCondicionAcciones.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcComentarioAdicional"].ToString() });
                                    tRowCondicionAcciones.Cells.Add(new HtmlTableCell() { InnerHtml = lblEstadoCondicion });
                                    tRowCondicionAcciones.Cells.Add(new HtmlTableCell() { InnerHtml = btnFinalizarCondicion });

                                    listaCondiciones.Add((int)sqlResultado["fiIDCondicion"]);


                                    /* validar si hay condiciones de documentación */
                                    if (listaCondicionesDeDocumentacion.Contains((int)sqlResultado["fiIDCondicion"]) && estadoCondicion == true)
                                    {
                                        liDocumentacion.Visible = true;
                                        tblCondicionesDocumentacion.Rows.Add(tRowCondicionAcciones);
                                    }
                                    /* validar si hay condiciones de referencias personales */
                                    else if ((sqlResultado["fiIDCondicion"].ToString() == "8" || sqlResultado["fiIDCondicion"].ToString() == "14") && estadoCondicion == true)
                                    {
                                        liReferenciasPersonales.Visible = true;
                                        tblCondicionesReferenciasPersonales.Rows.Add(tRowCondicionAcciones);
                                    }
                                    /* condiciones de la información de la solicitud */
                                    else if (sqlResultado["fiIDCondicion"].ToString() == "9" && estadoCondicion == true)
                                    {
                                        liInformacionPrestamoSolicitado.Visible = true;
                                        tblCondicionesInformacionSolicitud.Rows.Add(tRowCondicionAcciones);
                                    }
                                    /* condiciones de informacion personal */
                                    else if (sqlResultado["fiIDCondicion"].ToString() == "10" && estadoCondicion == true)
                                    {
                                        liInformacionPersonal.Visible = true;
                                        tblCondicionesInformacionPersonal.Rows.Add(tRowCondicionAcciones);
                                    }
                                    /* condiciones de la información del domicilio */
                                    else if (sqlResultado["fiIDCondicion"].ToString() == "11" && estadoCondicion == true)
                                    {
                                        liInformacionDomicilio.Visible = true;
                                        tblCondicionesDomicilio.Rows.Add(tRowCondicionAcciones);
                                    }
                                    /* condiciones de la informacion laboral */
                                    else if (sqlResultado["fiIDCondicion"].ToString() == "12" && estadoCondicion == true)
                                    {
                                        liInformacionLaboral.Visible = true;
                                        tblCondicionesLaboral.Rows.Add(tRowCondicionAcciones);
                                    }
                                    /* condiciones de la informacion conyugal */
                                    else if (sqlResultado["fiIDCondicion"].ToString() == "13" && estadoCondicion == true)
                                    {
                                        liInformacionConyugal.Visible = true;
                                        tblCondicionesInformacionConyugal.Rows.Add(tRowCondicionAcciones);
                                    }
                                }
                            }

                            /****** Información del cliente ******/
                            sqlResultado.NextResult();
                            sqlResultado.Read();

                            var fechaNacimiento = DateTime.Parse(sqlResultado["fdFechaNacimientoCliente"].ToString());
                            var hoy = DateTime.Today;
                            var edad = hoy.Year - fechaNacimiento.Year;
                            if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;


                            pcID = sqlResultado["fcIdentidadCliente"].ToString();

                            txtIdentidadCliente.Text = sqlResultado["fcIdentidadCliente"].ToString();
                            txtRtnCliente.Text = sqlResultado["fcRTN"].ToString();
                            txtPrimerNombre.Text = sqlResultado["fcPrimerNombreCliente"].ToString();
                            txtSegundoNombre.Text = sqlResultado["fcSegundoNombreCliente"].ToString();
                            txtPrimerApellido.Text = sqlResultado["fcPrimerApellidoCliente"].ToString();
                            txtSegundoApellido.Text = sqlResultado["fcSegundoApellidoCliente"].ToString();
                            ddlNacionalidad.SelectedValue = sqlResultado["fiNacionalidadCliente"].ToString();
                            ddlTipoDeCliente.SelectedValue = sqlResultado["fiTipoCliente"].ToString();
                            txtCorreoElectronico.Text = sqlResultado["fcCorreoElectronicoCliente"].ToString();
                            txtNumeroTelefono.Text = sqlResultado["fcTelefonoPrimarioCliente"].ToString();
                            txtFechaDeNacimiento.Text = fechaNacimiento.ToString("yyyy-MM-dd");

                            txtEdadDelCliente.Text = edad + " años";
                            if (sqlResultado["fcSexoCliente"].ToString() == "F")
                                rbSexoFemenino.Checked = true;
                            else
                                rbSexoMasculino.Checked = true;

                            ddlEstadoCivil.Text = sqlResultado["fiIDEstadoCivil"].ToString();
                            txtProfesion.Text = sqlResultado["fcProfesionOficioCliente"].ToString();

                            ddlTipoDeVivienda.Text = sqlResultado["fiIDVivienda"].ToString();
                            ddlTiempoDeResidir.Text = sqlResultado["fiTiempoResidir"].ToString();

                            /****** Información laboral ******/
                            sqlResultado.NextResult();

                            while (sqlResultado.Read())
                            {
                                txtNombreDelTrabajo.Text = sqlResultado["fcNombreTrabajo"].ToString();
                                txtFechaDeIngreso.Text = DateTime.Parse(sqlResultado["fdFechaIngreso"].ToString()).ToString("yyyy-MM-dd");
                                txtPuestoAsignado.Text = sqlResultado["fcPuestoAsignado"].ToString();
                                txtIngresosMensuales.Text = sqlResultado["fnIngresosMensuales"].ToString();
                                txtTelefonoEmpresa.Text = sqlResultado["fcTelefonoEmpresa"].ToString();
                                txtExtensionRecursosHumanos.Text = sqlResultado["fcExtensionRecursosHumanos"].ToString();
                                txtExtensionCliente.Text = sqlResultado["fcExtensionCliente"].ToString();
                                txtFuenteDeOtrosIngresos.Text = sqlResultado["fcFuenteOtrosIngresos"].ToString();
                                txtValorOtrosIngresos.Text = sqlResultado["fnValorOtrosIngresosMensuales"].ToString();
                                txtDireccionDetalladaEmpresa.Text = sqlResultado["fcDireccionDetalladaEmpresa"].ToString();
                                txtReferenciasEmpresa.Value = sqlResultado["fcReferenciasDireccionDetalladaEmpresa"].ToString();

                                /* Departamento de la empresa */
                                ddlDepartamentoEmpresa.SelectedValue = sqlResultado["fiIDDepartamento"].ToString();

                                /* Municipio de la empresa */
                                var municipiosDeDepartamento = CargarMunicipios(int.Parse(sqlResultado["fiIDDepartamento"].ToString()));

                                ddlMunicipioEmpresa.Items.Clear();
                                ddlMunicipioEmpresa.Items.Add(new ListItem("Seleccionar", ""));

                                municipiosDeDepartamento.ForEach(municipio =>
                                {
                                    ddlMunicipioEmpresa.Items.Add(new ListItem(municipio.NombreMunicipio, municipio.IdMunicipio.ToString()));
                                });
                                ddlMunicipioEmpresa.SelectedValue = sqlResultado["fiIDMunicipio"].ToString();
                                ddlMunicipioEmpresa.Enabled = true;

                                /* Ciudad o Poblado de la empresa */
                                var ciudadesPobladosDelMunicipio = CargarCiudadesPoblados(int.Parse(sqlResultado["fiIDDepartamento"].ToString()), int.Parse(sqlResultado["fiIDMunicipio"].ToString()));

                                ddlCiudadPobladoEmpresa.Items.Clear();
                                ddlCiudadPobladoEmpresa.Items.Add(new ListItem("Seleccionar", ""));

                                ciudadesPobladosDelMunicipio.ForEach(ciudadPoblado =>
                                {
                                    ddlCiudadPobladoEmpresa.Items.Add(new ListItem(ciudadPoblado.NombreCiudadPoblado, ciudadPoblado.IdCiudadPoblado.ToString()));
                                });
                                ddlCiudadPobladoEmpresa.SelectedValue = sqlResultado["fiIDCiudad"].ToString();
                                ddlCiudadPobladoEmpresa.Enabled = true;

                                /* Barrio o colonia de la empresa */
                                var barriosColoniasDelPoblado = CargarBarriosColonias(int.Parse(sqlResultado["fiIDDepartamento"].ToString()), int.Parse(sqlResultado["fiIDMunicipio"].ToString()), int.Parse(sqlResultado["fiIDCiudad"].ToString()));

                                ddlBarrioColoniaEmpresa.Items.Clear();
                                ddlBarrioColoniaEmpresa.Items.Add(new ListItem("Seleccionar", ""));

                                barriosColoniasDelPoblado.ForEach(barrioColonia =>
                                {
                                    ddlBarrioColoniaEmpresa.Items.Add(new ListItem(barrioColonia.NombreBarrioColonia, barrioColonia.IdBarrioColonia.ToString()));
                                });
                                ddlBarrioColoniaEmpresa.SelectedValue = sqlResultado["fiIDBarrioColonia"].ToString();
                                ddlBarrioColoniaEmpresa.Enabled = true;
                            }

                            /****** Informacion domicilio ******/
                            sqlResultado.NextResult();

                            while (sqlResultado.Read())
                            {
                                txtTelefonoCasa.Text = sqlResultado["fcTelefonoCasa"].ToString();
                                txtDireccionDetalladaDomicilio.Text = sqlResultado["fcDireccionDetalladaDomicilio"].ToString();
                                txtReferenciasDelDomicilio.Value = sqlResultado["fcReferenciasDireccionDetalladaDomicilio"].ToString();

                                /* Departamento */
                                ddlDepartamentoDomicilio.SelectedValue = sqlResultado["fiCodDepartamento"].ToString();

                                /* Municipio del domicilio */
                                var municipiosDeDepartamento = CargarMunicipios(int.Parse(sqlResultado["fiCodDepartamento"].ToString()));

                                ddlMunicipioDomicilio.Items.Clear();
                                ddlMunicipioDomicilio.Items.Add(new ListItem("Seleccionar", ""));

                                municipiosDeDepartamento.ForEach(municipio =>
                                {
                                    ddlMunicipioDomicilio.Items.Add(new ListItem(municipio.NombreMunicipio, municipio.IdMunicipio.ToString()));
                                });
                                ddlMunicipioDomicilio.SelectedValue = sqlResultado["fiCodMunicipio"].ToString();
                                ddlMunicipioDomicilio.Enabled = true;

                                /* Ciudad o Poblado del domicilio */
                                var ciudadesPobladosDelMunicipio = CargarCiudadesPoblados(int.Parse(sqlResultado["fiCodDepartamento"].ToString()), int.Parse(sqlResultado["fiCodMunicipio"].ToString()));

                                ddlCiudadPobladoDomicilio.Items.Clear();
                                ddlCiudadPobladoDomicilio.Items.Add(new ListItem("Seleccionar", ""));

                                ciudadesPobladosDelMunicipio.ForEach(ciudadPoblado =>
                                {
                                    ddlCiudadPobladoDomicilio.Items.Add(new ListItem(ciudadPoblado.NombreCiudadPoblado, ciudadPoblado.IdCiudadPoblado.ToString()));
                                });
                                ddlCiudadPobladoDomicilio.SelectedValue = sqlResultado["fiCodPoblado"].ToString();
                                ddlCiudadPobladoDomicilio.Enabled = true;

                                /* Barrio o colonia del domicilio */
                                var barriosColoniasDelPoblado = CargarBarriosColonias(int.Parse(sqlResultado["fiCodDepartamento"].ToString()), int.Parse(sqlResultado["fiCodMunicipio"].ToString()), int.Parse(sqlResultado["fiCodPoblado"].ToString()));

                                ddlBarrioColoniaDomicilio.Items.Clear();
                                ddlBarrioColoniaDomicilio.Items.Add(new ListItem("Seleccionar", ""));

                                barriosColoniasDelPoblado.ForEach(barrioColonia =>
                                {
                                    ddlBarrioColoniaDomicilio.Items.Add(new ListItem(barrioColonia.NombreBarrioColonia, barrioColonia.IdBarrioColonia.ToString()));
                                });
                                ddlBarrioColoniaDomicilio.SelectedValue = sqlResultado["fiCodBarrio"].ToString();
                                ddlBarrioColoniaDomicilio.Enabled = true;
                            }

                            /****** Informacion del conyugue ******/
                            sqlResultado.NextResult();

                            if (sqlResultado.HasRows)
                            {
                                while (sqlResultado.Read())
                                {
                                    txtIdentidadConyugue.Text = sqlResultado["fcIndentidadConyugue"].ToString();
                                    txtNombresConyugue.Text = sqlResultado["fcNombreCompletoConyugue"].ToString();
                                    txtFechaNacimientoConyugue.Text = DateTime.Parse(sqlResultado["fdFechaNacimientoConyugue"].ToString()).ToString("yyyy-MM-dd");
                                    txtTelefonoConyugue.Text = sqlResultado["fcTelefonoConyugue"].ToString();
                                    txtLugarDeTrabajoConyuge.Text = sqlResultado["fcLugarTrabajoConyugue"].ToString();
                                    txtIngresosMensualesConyugue.Text = sqlResultado["fnIngresosMensualesConyugue"].ToString();
                                    txtTelefonoTrabajoConyugue.Text = sqlResultado["fcTelefonoTrabajoConyugue"].ToString();
                                }
                            }
                            else
                            {
                                //liInformacionConyugal.Visible = false;
                            }

                            /****** Referencias de la solicitud ******/
                            sqlResultado.NextResult();

                            if (sqlResultado.HasRows)
                            {
                                HtmlTableRow tRowReferencias;
                                var btnEditarReferencia = string.Empty;
                                var btnEliminarReferencia = string.Empty;

                                /* Llenar table de referencias */
                                while (sqlResultado.Read())
                                {
                                    btnEliminarReferencia = "<button id='btnEliminarReferencia' data-id='" + sqlResultado["fiIDReferencia"].ToString() + "' class='btn btn-sm btn-danger mb-0 align-self-center' type='button' title='Eliminar referencia personal'><i class='far fa-trash-alt'></i></button>";
                                    btnEditarReferencia = "<button id='btnEditarReferencia' data-id='" + sqlResultado["fiIDReferencia"].ToString() + "' data-nombre='" + sqlResultado["fcNombreCompletoReferencia"].ToString() + "' data-trabajo='" + sqlResultado["fcLugarTrabajoReferencia"].ToString() + "' data-telefono='" + sqlResultado["fcTelefonoReferencia"].ToString() + "' data-idtiempodeconocer='" + sqlResultado["fiTiempoConocerReferencia"].ToString() + "' data-idparentesco='" + sqlResultado["fiIDParentescoReferencia"] + "' class='btn btn-sm btn-info mb-0 align-self-center' type='button' title='Editar referencia personal'><i class='far fa-edit'></i></button>";

                                    tRowReferencias = new HtmlTableRow();
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcNombreCompletoReferencia"].ToString() });
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcTelefonoReferencia"].ToString() });
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcLugarTrabajoReferencia"].ToString() });
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcTiempoDeConocer"].ToString() });
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcDescripcionParentesco"].ToString() });
                                    tRowReferencias.Cells.Add(new HtmlTableCell() { InnerHtml = btnEditarReferencia + btnEliminarReferencia });
                                    tblReferenciasPersonales.Rows.Add(tRowReferencias);
                                }
                            }

                            /****** Historial de mantenimientos de la solicitud ******/
                            sqlResultado.NextResult();
                        }
                    }
                } // using command
            }// using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public void CargarPrecalificado()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                /* Cargar precalificado del cliente */
                using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /* Si no está precalificado, retornar a pantalla de precalificacion */
                        if (!sqlResultado.HasRows)
                        {
                            string lcScript = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
                            Response.Write("<script>");
                            Response.Write(lcScript);
                            Response.Write("</script>");
                        }

                        while (sqlResultado.Read())
                        {
                            Precalificado.IdClienteSAF = sqlResultado["fcIDCliente"].ToString();
                            Precalificado.Identidad = sqlResultado["fcIdentidad"].ToString();
                            Precalificado.PrimerNombre = sqlResultado["fcPrimerNombre"].ToString();
                            Precalificado.SegundoNombre = sqlResultado["fcSegundoNombre"].ToString();
                            Precalificado.PrimerApellido = sqlResultado["fcPrimerApellido"].ToString();
                            Precalificado.SegundoApellido = sqlResultado["fcSegundoApellido"].ToString();
                            Precalificado.Telefono = sqlResultado["fcTelefono"].ToString();
                            Precalificado.Ingresos = decimal.Parse(sqlResultado["fnIngresos"].ToString());
                            Precalificado.Obligaciones = decimal.Parse(sqlResultado["fnTotalObligaciones"].ToString());
                            Precalificado.Disponible = decimal.Parse(sqlResultado["fnCapacidadDisponible"].ToString());
                            Precalificado.FechaNacimiento = DateTime.Parse(sqlResultado["fdFechadeNacimiento"].ToString());
                            Precalificado.IdTipoDeSolicitud = (byte)sqlResultado["fiTipoSolicitudCliente"];
                            Precalificado.TipoDeSolicitud = sqlResultado["fcTipoSolicitud"].ToString();
                            Precalificado.IdProducto = int.Parse(sqlResultado["fiIDProducto"].ToString());
                            Precalificado.Producto = sqlResultado["fcProducto"].ToString();
                            Precalificado.ScorePromedio = sqlResultado["fiScorePromedio"].ToString();
                        }
                    }
                } // using sp consulta ejecutivos
            }// using conexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    #endregion

    #region Cargar Municipios, poblados y barrios

    public static List<Municipios_ViewModel> CargarMunicipios(int idDepartamento)
    {
        var municipios = new List<Municipios_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoMunicipio", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", idDepartamento);
                    sqlComando.Parameters.AddWithValue("@piMunicipio", 0);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            municipios.Add(new Municipios_ViewModel()
                            {
                                IdDepartamento = (short)sqlResultado["fiCodDepartamento"],
                                IdMunicipio = (short)sqlResultado["fiCodMunicipio"],
                                NombreMunicipio = sqlResultado["fcMunicipio"].ToString(),
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            municipios = null;
        }
        return municipios;
    }

    public static List<Ciudades_ViewModel> CargarCiudadesPoblados(int idDepartamento, int idMunicipio)
    {
        var ciudades = new List<Ciudades_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoPoblado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", idDepartamento);
                    sqlComando.Parameters.AddWithValue("@piMunicipio", idMunicipio);
                    sqlComando.Parameters.AddWithValue("@piPoblado", 0);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            ciudades.Add(new Ciudades_ViewModel()
                            {
                                IdDepartamento = (short)sqlResultado["fiCodDepartamento"],
                                IdMunicipio = (short)sqlResultado["fiCodMunicipio"],
                                IdCiudadPoblado = (short)sqlResultado["fiCodPoblado"],
                                NombreCiudadPoblado = sqlResultado["fcPoblado"].ToString(),
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            ciudades = null;
        }
        return ciudades;
    }

    public static List<BarriosColonias_ViewModel> CargarBarriosColonias(int idDepartamento, int idMunicipio, int idCiudadPoblado)
    {
        var barriosColonias = new List<BarriosColonias_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_GeoBarrios", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", idDepartamento);
                    sqlComando.Parameters.AddWithValue("@piMunicipio", idMunicipio);
                    sqlComando.Parameters.AddWithValue("@piPoblado", idCiudadPoblado);
                    sqlComando.Parameters.AddWithValue("@piBarrio", 0);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            barriosColonias.Add(new BarriosColonias_ViewModel()
                            {
                                IdDepartamento = (short)sqlResultado["fiCodDepartamento"],
                                IdMunicipio = (short)sqlResultado["fiCodMunicipio"],
                                IdCiudadPoblado = (short)sqlResultado["fiCodPoblado"],
                                IdBarrioColonia = (short)sqlResultado["fiCodBarrio"],
                                NombreBarrioColonia = sqlResultado["fcBarrioColonia"].ToString()
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            barriosColonias = null;
        }
        return barriosColonias;
    }

    #endregion

    #region WebMethods Cargar Municipios, poblados, colonias y documentos requeridos

    [WebMethod]
    public static List<CoreFinanciero_TipoDocumento_ViewModel> CargarDocumentosRequeridos()
    {
        return (List<CoreFinanciero_TipoDocumento_ViewModel>)HttpContext.Current.Session["DocumentosRequeridos"];
    }

    [WebMethod]
    public static List<Municipios_ViewModel> CargarListaMunicipios(int idDepartamento)
    {
        return CargarMunicipios(idDepartamento);
    }

    [WebMethod]
    public static List<Ciudades_ViewModel> CargarListaCiudadesPoblados(int idDepartamento, int idMunicipio)
    {
        return CargarCiudadesPoblados(idDepartamento, idMunicipio);
    }

    [WebMethod]
    public static List<BarriosColonias_ViewModel> CargarListaBarriosColonias(int idDepartamento, int idMunicipio, int idCiudadPoblado)
    {
        return CargarBarriosColonias(idDepartamento, idMunicipio, idCiudadPoblado);
    }

    #endregion

    #region Actualizar condiciones

    [WebMethod]
    public static bool ActualizarCondicionamiento(int idSolicitudCondicion, int idCliente, int idTipoDeCondicion, string objSeccion, string dataCrypt, SolicitudesCredito_Actualizar_CalculoPrestamo_ViewModel cotizador)
    {
        bool resultadoProceso = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var mensajeError = string.Empty;

            var resultadoActualizacion = string.Empty;
            var json_serializer = new JavaScriptSerializer();

            var listaCondicionesDeDocumentacion = new int[] { 1, 2, 3, 4, 5, 6 };

            /* validar si hay condiciones de documentación */
            if (listaCondicionesDeDocumentacion.Contains(idTipoDeCondicion))
            {
                resultadoActualizacion = ActualizarDocumentacion(idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
            }

            /* validar si hay condiciones de referencias personales */
            if (idTipoDeCondicion == 8 || idTipoDeCondicion == 14)
            {
                //var referenciasPersonales = json_serializer.Deserialize<List<Cliente_ReferenciaPersonal_ViewModel>>(objSeccion);
                //resultadoActualizacion = ActualizarReferenciasPersonales(referenciasPersonales, idCliente, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                resultadoActualizacion = idSolicitudCondicion.ToString();
            }

            /* condiciones de la información de la solicitud */
            if (idTipoDeCondicion == 9)
            {
                var informacionSolicitud = json_serializer.Deserialize<SolicitudesCredito_Solicitud_Maestro_ViewModel>(objSeccion);
                resultadoActualizacion = ActualizarInformacionSolicitud(informacionSolicitud, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp, cotizador);
            }


            /* condiciones de informacion personal */
            if (idTipoDeCondicion == 10)
            {
                var informacionPersonal = json_serializer.Deserialize<Cliente_ViewModel>(objSeccion);
                resultadoActualizacion = ActualizarInformacionPersonal(informacionPersonal, pcIDSesion, pcIDUsuario, pcIDApp);
            }

            /* condiciones de la información del domicilio */
            if (idTipoDeCondicion == 11)
            {
                var informacionDomicilio = json_serializer.Deserialize<Cliente_InformacionDomicilio_ViewModel>(objSeccion);
                resultadoActualizacion = ActualizarInformacionDomicilio(informacionDomicilio, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
            }

            /* condiciones de la informacion laboral */
            if (idTipoDeCondicion == 12)
            {
                var informacionLaboral = json_serializer.Deserialize<Cliente_InformacionLaboral_ViewModel>(objSeccion);
                resultadoActualizacion = ActualizarInformacionLaboral(informacionLaboral, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
            }

            /* condiciones de la informacion conyugal */
            if (idTipoDeCondicion == 13)
            {
                var informacionConyugue = json_serializer.Deserialize<Cliente_InformacionConyugal_ViewModel>(objSeccion);
                resultadoActualizacion = ActualizarInformacionConyugal(informacionConyugue, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
            }

            if (resultadoActualizacion.StartsWith("-1") || resultadoActualizacion == string.Empty || resultadoActualizacion == "0")
            {
                return false;
            }

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Condiciones_Actualizar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitudCondicion", idSolicitudCondicion);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (!sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                            {
                                resultadoProceso = true;
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
        return resultadoProceso;
    }

    public static string ActualizarInformacionPersonal(Cliente_ViewModel cliente, string pcIDSesion, string pcIDUsuario, string pcIDApp)
    {
        var resultado = string.Empty;

        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDCliente_Maestro_Actualizar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDCliente", cliente.IdCliente);
                    sqlComando.Parameters.AddWithValue("@pcIdentidadCliente", cliente.IdentidadCliente);
                    sqlComando.Parameters.AddWithValue("@pcRTN", cliente.RtnCliente);
                    sqlComando.Parameters.AddWithValue("@pcPrimerNombreCliente", cliente.PrimerNombre);
                    sqlComando.Parameters.AddWithValue("@pcSegundoNombreCliente", cliente.SegundoNombre);
                    sqlComando.Parameters.AddWithValue("@pcPrimerApellidoCliente", cliente.PrimerApellido);
                    sqlComando.Parameters.AddWithValue("@pcSegundoApellidoCliente", cliente.SegundoApellido);
                    sqlComando.Parameters.AddWithValue("@pcTelefonoCliente", cliente.TelefonoCliente);
                    sqlComando.Parameters.AddWithValue("@piNacionalidadCliente", cliente.IdNacionalidad);
                    sqlComando.Parameters.AddWithValue("@pdFechaNacimientoCliente", cliente.FechaNacimiento);
                    sqlComando.Parameters.AddWithValue("@pcCorreoElectronicoCliente", cliente.Correo);
                    sqlComando.Parameters.AddWithValue("@pcProfesionOficioCliente", cliente.ProfesionOficio);
                    sqlComando.Parameters.AddWithValue("@pcSexoCliente", cliente.Sexo);
                    sqlComando.Parameters.AddWithValue("@piIDEstadoCivil", cliente.IdEstadoCivil);
                    sqlComando.Parameters.AddWithValue("@piIDVivienda", cliente.IdVivienda);
                    sqlComando.Parameters.AddWithValue("@piTiempoResidir", cliente.IdTiempoResidir);
                    sqlComando.Parameters.AddWithValue("@pbClienteActivo", true);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultado = sqlResultado["MensajeError"].ToString();
                        }
                    }
                }
            }
            /* Pendiente verficar si el nuevo estado civil del cliente requiere información conyugal */
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultado;
    }

    public static string ActualizarInformacionDomicilio(Cliente_InformacionDomicilio_ViewModel clienteInformacionDomicilio, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
    {
        var resultado = string.Empty;

        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDCliente_InformacionDomicilio_Actualizar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDCliente", clienteInformacionDomicilio.IdCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDDepartamento", clienteInformacionDomicilio.IdDepartamento);
                    sqlComando.Parameters.AddWithValue("@piIDMunicipio", clienteInformacionDomicilio.IdMunicipio);
                    sqlComando.Parameters.AddWithValue("@piIDCiudad", clienteInformacionDomicilio.IdCiudadPoblado);
                    sqlComando.Parameters.AddWithValue("@piIDBarrioColonia", clienteInformacionDomicilio.IdBarrioColonia);
                    sqlComando.Parameters.AddWithValue("@pcTelefonoCasa", clienteInformacionDomicilio.TelefonoCasa);
                    sqlComando.Parameters.AddWithValue("@pcDireccionDetallada", clienteInformacionDomicilio.DireccionDetallada);
                    sqlComando.Parameters.AddWithValue("@pcReferenciasDireccionDetallada", clienteInformacionDomicilio.ReferenciasDireccionDetallada);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultado = sqlResultado["MensajeError"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultado;
    }

    public static string ActualizarInformacionLaboral(Cliente_InformacionLaboral_ViewModel clienteInformacionLaboral, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
    {
        var resultado = string.Empty;
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDCliente_InformacionLaboral_Actualizar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDCliente", clienteInformacionLaboral.IdCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@pcNombreTrabajo", clienteInformacionLaboral.NombreTrabajo);
                    sqlComando.Parameters.AddWithValue("@pnIngresosMensuales", clienteInformacionLaboral.IngresosMensuales);
                    sqlComando.Parameters.AddWithValue("@pcPuestoAsignado", clienteInformacionLaboral.PuestoAsignado);
                    sqlComando.Parameters.AddWithValue("@pdFechaIngreso", clienteInformacionLaboral.FechaIngreso);
                    sqlComando.Parameters.AddWithValue("@pcTelefonoEmpresa", clienteInformacionLaboral.TelefonoEmpresa);
                    sqlComando.Parameters.AddWithValue("@pcExtensionCliente", clienteInformacionLaboral.ExtensionCliente);
                    sqlComando.Parameters.AddWithValue("@pcExtensionRecursosHumanos", clienteInformacionLaboral.ExtensionRecursosHumanos);
                    sqlComando.Parameters.AddWithValue("@piIDDepartamento", clienteInformacionLaboral.IdDepartamento);
                    sqlComando.Parameters.AddWithValue("@piIDMunicipio", clienteInformacionLaboral.IdMunicipio);
                    sqlComando.Parameters.AddWithValue("@piIDCiudad", clienteInformacionLaboral.IdCiudadPoblado);
                    sqlComando.Parameters.AddWithValue("@piIDBarrioColonia", clienteInformacionLaboral.IdBarrioColonia);
                    sqlComando.Parameters.AddWithValue("@pcDireccionDetalladaEmpresa", clienteInformacionLaboral.DireccionDetalladaEmpresa);
                    sqlComando.Parameters.AddWithValue("@pcReferenciasDireccionDetallada", clienteInformacionLaboral.ReferenciasDireccionDetallada);
                    sqlComando.Parameters.AddWithValue("@pcFuenteOtrosIngresos", clienteInformacionLaboral.FuenteOtrosIngresos);
                    sqlComando.Parameters.AddWithValue("@pnValorOtrosIngresosMensuales", clienteInformacionLaboral.ValorOtrosIngresos);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultado = sqlResultado["MensajeError"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultado;
    }

    public static string ActualizarInformacionConyugal(Cliente_InformacionConyugal_ViewModel informacionConyugal, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
    {
        var resultado = string.Empty;
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDCliente_InformacionConyugal_Actualizar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDCliente", informacionConyugal.IdCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@pcNombreCompletoConyugue", informacionConyugal.NombreCompletoConyugue);
                    sqlComando.Parameters.AddWithValue("@pcIndentidadConyugue", informacionConyugal.IdentidadConyugue);
                    sqlComando.Parameters.AddWithValue("@pdFechaNacimientoConyugue", informacionConyugal.FechaNacimientoConyugue);
                    sqlComando.Parameters.AddWithValue("@pcTelefonoConyugue", informacionConyugal.TelefonoConyugue);
                    sqlComando.Parameters.AddWithValue("@pcLugarTrabajoConyugue", informacionConyugal.LugarTrabajoConyugue);
                    sqlComando.Parameters.AddWithValue("@pnIngresosMensualesConyugue", informacionConyugal.IngresosMensualesConyugue);
                    sqlComando.Parameters.AddWithValue("@pcTelefonoTrabajoConyugue", informacionConyugal.TelefonoTrabajoConyugue);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultado = sqlResultado["MensajeError"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultado;
    }

    public static string ActualizarDocumentacion(string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
    {
        var resultado = string.Empty;
        try
        {
            var documentacionCliente = 1;
            var nombreCarpetaDocumentos = "Solicitud" + idSolicitud;
            var nuevoNombreDocumento = string.Empty;

            /* lista de documentos adjuntados por el usuario */
            var listaDocumentos = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

            /* Lista de documentos que se va ingresar en la base de datos y se va mover al nuevo directorio */
            var solicitudDocumentos = new List<SolicitudesDocumentosViewModel>();

            if (listaDocumentos != null)
            {
                /* lista de bloques y la cantidad de documentos que contiene cada uno */
                var bloques = listaDocumentos.GroupBy(TipoDocumento => TipoDocumento.fiTipoDocumento).Select(x => new { x.Key, Count = x.Count() });

                /* lista donde se guardara temporalmente los documentos adjuntados por el usuario dependiendo del tipo de documento en el iterador */
                var documentosBloque = new List<SolicitudesDocumentosViewModel>();

                foreach (var bloque in bloques)
                {
                    var tipoDocumento = (int)bloque.Key;
                    var cantidadDocumentos = bloque.Count;

                    documentosBloque = listaDocumentos.Where(x => x.fiTipoDocumento == tipoDocumento).ToList(); // documentos de este bloque
                    var nombresGenerador = Funciones.MultiNombres.GenerarNombreCredDocumento(documentacionCliente, Convert.ToInt32(idSolicitud), tipoDocumento, cantidadDocumentos);

                    var contadorNombre = 0;
                    foreach (SolicitudesDocumentosViewModel file in documentosBloque)
                    {
                        if (File.Exists(file.fcRutaArchivo + @"\" + file.NombreAntiguo))//si el archivo existe, que se agregue a la lista
                        {
                            nuevoNombreDocumento = nombresGenerador[contadorNombre];
                            solicitudDocumentos.Add(new SolicitudesDocumentosViewModel()
                            {
                                fcNombreArchivo = nuevoNombreDocumento,
                                NombreAntiguo = file.NombreAntiguo,
                                fcTipoArchivo = file.fcTipoArchivo,
                                fcRutaArchivo = file.fcRutaArchivo.Replace("Temp", "") + nombreCarpetaDocumentos,
                                URLArchivo = "/Documentos/Solicitudes/" + nombreCarpetaDocumentos + "/" + nuevoNombreDocumento + ".png",
                                fiTipoDocumento = file.fiTipoDocumento
                            });
                            contadorNombre++;
                        }
                    }
                }
            }
            else
            {
                return "-1";
            }

            if (solicitudDocumentos.Count <= 0)
            {
                return "-1";
            }

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var tran = sqlConexion.BeginTransaction())
                {
                    var contadorErrores = 0;

                    foreach (SolicitudesDocumentosViewModel documento in solicitudDocumentos)
                    {
                        using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Documentos_Guardar", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                            sqlComando.Parameters.AddWithValue("@pcNombreArchivo", documento.fcNombreArchivo);
                            sqlComando.Parameters.AddWithValue("@pcTipoArchivo", ".png");
                            sqlComando.Parameters.AddWithValue("@pcRutaArchivo", documento.fcRutaArchivo);
                            sqlComando.Parameters.AddWithValue("@pcURL", documento.URLArchivo);
                            sqlComando.Parameters.AddWithValue("@piTipoDocumento", documento.fiTipoDocumento);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.CommandTimeout = 120;

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    resultado = sqlResultado["MensajeError"].ToString();

                                    if (resultado.StartsWith("-1"))
                                    {
                                        contadorErrores++;
                                    }
                                }
                            }
                        }
                    }
                    /* Mover documentos al directorio de la solicitud */
                    if (!FileUploader.GuardarSolicitudDocumentos(Convert.ToInt32(idSolicitud), solicitudDocumentos))
                    {
                        contadorErrores++;
                    }

                    /* Verificar resultado del proceso */
                    if (contadorErrores == 0)
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                        resultado = "-1";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            resultado = "-1" + ex.Message.ToString();
        }
        return resultado;
    }

    public static string ActualizarInformacionSolicitud(SolicitudesCredito_Solicitud_Maestro_ViewModel informacionSolicitud, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp, SolicitudesCredito_Actualizar_CalculoPrestamo_ViewModel cotizador)
    {
        var resultado = string.Empty;
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Maestro_Actualizar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@pnValorSeleccionado", informacionSolicitud.ValorSeleccionado);
                    sqlComando.Parameters.AddWithValue("@piPlazoSeleccionado", informacionSolicitud.PlazoSeleccionado);
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", informacionSolicitud.ValorPrima);
                    sqlComando.Parameters.AddWithValue("@pnValorGarantia", informacionSolicitud.ValorGarantia);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            resultado = sqlResultado["MensajeError"].ToString();
                        }
                    }
                }

                /* Guardar informacion del cotizador para imprimir documentos... */
                if (informacionSolicitud.IdProducto == 202 || informacionSolicitud.IdProducto == 203 || informacionSolicitud.IdProducto == 204 || informacionSolicitud.IdProducto == 201)
                {
                    var fechaPrimerPago = ObtenerFechaPrimerPago(idSolicitud);

                    using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_InformacionPrestamo_Actualizar", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDCanal", 1);
                        sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                        sqlComando.Parameters.AddWithValue("@pcNumeroPrestamo", "No disponible");
                        sqlComando.Parameters.AddWithValue("@pdFechaPrimerCuota", fechaPrimerPago);
                        sqlComando.Parameters.AddWithValue("@pnValorTotalFinanciamiento", cotizador.TotalAFinanciar);
                        sqlComando.Parameters.AddWithValue("@pnValorAPrestar", informacionSolicitud.ValorGarantia - informacionSolicitud.ValorPrima);
                        sqlComando.Parameters.AddWithValue("@pnCostoGPS", cotizador.CostoGPS);
                        sqlComando.Parameters.AddWithValue("@pnValorTotalSeguro", cotizador.TotalSeguroVehiculo);
                        sqlComando.Parameters.AddWithValue("@pnGastosDeCierre", cotizador.GastosdeCierre);
                        sqlComando.Parameters.AddWithValue("@pnTasaMensualAplicada", cotizador.TasaInteresAnual / 12);
                        sqlComando.Parameters.AddWithValue("@pnTasaAnualAplicada", cotizador.TasaInteresAnual);
                        sqlComando.Parameters.AddWithValue("@piPlazo", informacionSolicitud.PlazoSeleccionado);
                        sqlComando.Parameters.AddWithValue("@pcTipoDePlazo", "Meses");
                        sqlComando.Parameters.AddWithValue("@pnCuotaMensualPrestamo", cotizador.CuotaDelPrestamo);
                        sqlComando.Parameters.AddWithValue("@pnCuotaMensualGPS", cotizador.CuotaServicioGPS);
                        sqlComando.Parameters.AddWithValue("@pnCuotaMensualSeguro", cotizador.CuotaSegurodeVehiculo);
                        sqlComando.Parameters.AddWithValue("@pnCuotaTotal", cotizador.CuotaTotal);
                        sqlComando.Parameters.AddWithValue("@pnValorTotalContrato", cotizador.TotalFinanciadoConIntereses);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.CommandTimeout = 120;

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            while (sqlResultado.Read())
                            {
                                resultado = sqlResultado["MensajeError"].ToString();
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
        return resultado;
    }

    #endregion

    #region Administracion de referencias personales

    [WebMethod]
    public static List<Cliente_ReferenciaPersonal_ViewModel> ListadoReferenciasPersonalesPorIdSolicitud(string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
        var listadoReferenciasPersonales = new List<Cliente_ReferenciaPersonal_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");


            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_ObtenerPorIdSolicitud", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            listadoReferenciasPersonales.Add(new Cliente_ReferenciaPersonal_ViewModel()
                            {
                                IdReferencia = (int)sqlResultado["fiIDReferencia"],
                                IdCliente = (int)sqlResultado["fiIDCliente"],
                                IdSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                NombreCompleto = sqlResultado["fcNombreCompletoReferencia"].ToString(),
                                LugarTrabajo = sqlResultado["fcLugarTrabajoReferencia"].ToString(),
                                IdTiempoDeConocer = (short)sqlResultado["fiTiempoConocerReferencia"],
                                TiempoDeConocer = sqlResultado["fcTiempoDeConocer"].ToString(),
                                TelefonoReferencia = sqlResultado["fcTelefonoReferencia"].ToString(),
                                IdParentescoReferencia = (int)sqlResultado["fiIDParentescoReferencia"],
                                DescripcionParentesco = sqlResultado["fcDescripcionParentesco"].ToString(),
                                ReferenciaActivo = (bool)sqlResultado["fbReferenciaActivo"],
                                RazonInactivo = sqlResultado["fcRazonInactivo"].ToString(),
                            });
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            listadoReferenciasPersonales = null;
        }
        return listadoReferenciasPersonales;
    }

    [WebMethod]
    public static bool RegistrarReferenciaPersonal(Cliente_ReferenciaPersonal_ViewModel referenciaPersonal, string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
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

                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Guardar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDCliente", referenciaPersonal.IdCliente);
                    sqlComando.Parameters.AddWithValue("@piIDSolicitud", referenciaPersonal.IdSolicitud);
                    sqlComando.Parameters.AddWithValue("@pcNombreCompletoReferencia", referenciaPersonal.NombreCompleto.Trim());
                    sqlComando.Parameters.AddWithValue("@pcLugarTrabajoReferencia", referenciaPersonal.LugarTrabajo.Trim());
                    sqlComando.Parameters.AddWithValue("@piTiempoConocerReferencia", referenciaPersonal.IdTiempoDeConocer);
                    sqlComando.Parameters.AddWithValue("@pcTelefonoReferencia", referenciaPersonal.TelefonoReferencia.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDParentescoReferencia", referenciaPersonal.IdParentescoReferencia);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (!sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                            {
                                resultado = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    [WebMethod]
    public static bool EliminarReferenciaPersonal(int idReferenciaPersonal, string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
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

                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Eliminar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", idReferenciaPersonal);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (!sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                            {
                                resultado = true;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    [WebMethod]
    public static bool ActualizarReferenciaPersonal(Cliente_ReferenciaPersonal_ViewModel referenciaPersonal, string dataCrypt)
    {
        var DSC = new DSCore.DataCrypt();
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

                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Actualizar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDReferenciaPersonal", referenciaPersonal.IdReferencia);
                    sqlComando.Parameters.AddWithValue("@pcNombreCompletoReferencia", referenciaPersonal.NombreCompleto.Trim());
                    sqlComando.Parameters.AddWithValue("@pcLugarTrabajoReferencia", referenciaPersonal.LugarTrabajo.Trim());
                    sqlComando.Parameters.AddWithValue("@piTiempoConocerReferencia", referenciaPersonal.IdTiempoDeConocer);
                    sqlComando.Parameters.AddWithValue("@pcTelefonoReferencia", referenciaPersonal.TelefonoReferencia.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDParentescoReferencia", referenciaPersonal.IdParentescoReferencia);
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
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
        }
        return resultado;
    }

    #endregion

    #region Calculo de prestamo

    [WebMethod]
    public static SolicitudesCredito_Actualizar_CalculoPrestamo_ViewModel CalculoPrestamo(int idProducto, decimal valorGlobal, decimal valorPrima, int plazo, string dataCrypt)
    {
        var calculo = new SolicitudesCredito_Actualizar_CalculoPrestamo_ViewModel();
        try
        {
            var urlDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
            var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CredSolicitud_CalculoPrestamo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDProducto", idProducto);
                    sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", valorGlobal);
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", valorPrima);
                    sqlComando.Parameters.AddWithValue("@liPlazo", plazo);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            calculo = new SolicitudesCredito_Actualizar_CalculoPrestamo_ViewModel()
                            {
                                SegurodeDeuda = decimal.Parse(sqlResultado["fnSegurodeDeuda"].ToString()),
                                TotalSeguroVehiculo = (idProducto == 202 || idProducto == 203 || idProducto == 204) ? decimal.Parse(sqlResultado["fnTotalSeguroVehiculo"].ToString()) : decimal.Parse(sqlResultado["fnSegurodeVehiculo"].ToString()),
                                CuotaSegurodeVehiculo = decimal.Parse(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()),
                                GastosdeCierre = decimal.Parse(sqlResultado["fnGastosdeCierre"].ToString()),
                                TotalAFinanciar = decimal.Parse(sqlResultado["fnValoraFinanciar"].ToString()),
                                CuotaDelPrestamo = (idProducto == 202 || idProducto == 203 || idProducto == 204) ? decimal.Parse(sqlResultado["fnCuotaMensual"].ToString()) : (decimal.Parse(sqlResultado["fnCuotaQuincenal"].ToString()) - decimal.Parse(sqlResultado["fnCuotaSegurodeVehiculo"].ToString())),
                                CuotaTotal = (idProducto == 202 || idProducto == 203 || idProducto == 204) ? decimal.Parse(sqlResultado["fnCuotaMensualNeta"].ToString()) : decimal.Parse(sqlResultado["fnCuotaQuincenal"].ToString()),
                                CuotaServicioGPS = decimal.Parse(sqlResultado["fnCuotaServicioGPS"].ToString()),
                                TipoCuota = (idProducto == 202 || idProducto == 203 || idProducto == 204) ? "Meses" : "Quincenas",
                                ValorDelPrestamo = valorGlobal - valorPrima,
                                TasaInteresAnual = decimal.Parse(sqlResultado["fnTasaDeInteresAnual"].ToString()),
                            };
                        } // using sqlResultado.Read()
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            calculo = null;
        }
        return calculo;
    }

    [WebMethod]
    public static SolicitudesCredito_Actualizar_CalculoPrestamo_ViewModel CalculoPrestamoVehiculo(int idProducto, decimal valorGlobal, decimal valorPrima, int plazo, string scorePromedio, int tipoSeguro, int tipoGps, int gastosDeCierreFinanciados, string dataCrypt)
    {
        var calculo = new SolicitudesCredito_Actualizar_CalculoPrestamo_ViewModel();
        try
        {
            var urlDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
            var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CredCotizadorProductos_Vehiculos", sqlConexion))
                {
                    var montoPrestamo = (idProducto == 203) ? valorPrima : valorGlobal - valorPrima;
                    valorPrima = (idProducto == 203) ? valorGlobal - valorPrima : valorPrima;

                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", idProducto);
                    sqlComando.Parameters.AddWithValue("@pnMontoaPrestamo", montoPrestamo);
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", valorPrima);
                    sqlComando.Parameters.AddWithValue("@piScorePromedio", scorePromedio);
                    sqlComando.Parameters.AddWithValue("@piTipodeSeguro", tipoSeguro);
                    sqlComando.Parameters.AddWithValue("@piTipodeGPS", tipoGps);
                    sqlComando.Parameters.AddWithValue("@piFinanciandoGastosdeCierre", gastosDeCierreFinanciados);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (sqlResultado["fiIDPlazo"].ToString() == plazo.ToString())
                            {
                                calculo = new SolicitudesCredito_Actualizar_CalculoPrestamo_ViewModel()
                                {
                                    IdOrden = (int)sqlResultado["fiOrden"],
                                    Plazo = (byte)sqlResultado["fiIDPlazo"],
                                    TasaInteresAnual = decimal.Parse(sqlResultado["fiInteresAnual"].ToString()),
                                    ValorGarantia = decimal.Parse(sqlResultado["fnValorVehiculo"].ToString()),
                                    GastosdeCierre = decimal.Parse(sqlResultado["fnGastosdeCierre"].ToString()),
                                    CostoGPS = decimal.Parse(sqlResultado["fnCostoGPS"].ToString()),
                                    TotalAFinanciar = decimal.Parse(sqlResultado["fnTotalaFinanciar"].ToString()),
                                    TotalIntereses = decimal.Parse(sqlResultado["fnTotalIntereses"].ToString()),
                                    TotalFinanciadoConIntereses = decimal.Parse(sqlResultado["fnTotalFinanciado"].ToString()),
                                    CuotaDelPrestamo = decimal.Parse(sqlResultado["fnCuotadelPrestamo"].ToString()),
                                    CuotaSegurodeVehiculo = decimal.Parse(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()),
                                    CuotaServicioGPS = decimal.Parse(sqlResultado["fnCuotaServicioGPS"].ToString()),
                                    CuotaTotal = decimal.Parse(sqlResultado["fnTotalCuota"].ToString()),
                                    TipoCuota = "Mensual",
                                    ValorDelPrestamo = montoPrestamo
                                };
                            }
                        } // using sqlResultado.Read()
                    }// using sqlcomand.ExecuteReader()
                }// using command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            calculo = null;
        }
        return calculo;
    }

    #endregion

    #region Funciones utilitarias

    [WebMethod]
    public static string ObtenerUrlEncriptado(int idCliente, string dataCrypt)
    {
        var lUrlDesencriptado = DesencriptarURL(dataCrypt);
        var idUsuario = HttpUtility.ParseQueryString(lUrlDesencriptado.Query).Get("usr");
        var idSolicitud = HttpUtility.ParseQueryString(lUrlDesencriptado.Query).Get("IDSOL");
        var pcIDApp = HttpUtility.ParseQueryString(lUrlDesencriptado.Query).Get("IDApp");
        var pcIDSesion = HttpUtility.ParseQueryString(lUrlDesencriptado.Query).Get("SID") ?? "0";

        return DSC.Encriptar("usr=" + idUsuario + "&IDSOL=" + idSolicitud + "&cltID=" + idCliente + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion);
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lUrlDesencriptado = null;
        try
        {
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;
            var liParamStart = URL.IndexOf("?");

            if (liParamStart > 0)
                lcParametros = URL.Substring(liParamStart, URL.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = URL.Substring((liParamStart + 1), URL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                lUrlDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return lUrlDesencriptado;
    }

    private static DateTime ObtenerFechaPrimerPago(string idProducto)
    {
        DateTime fechaDelPrimerPago;

        var hoy = DateTime.Now;
        int diaPrimerPago = hoy.Day;
        int mesPrimerPago = hoy.Month;
        int anioPrimerPago = hoy.Year;

        /* ================================================== */
        /* ================= Quincenalmente  ================ */
        /* ======= Del 06 al 20 = 30 del mismo mes ========== */
        /* ======= Del 21 al 05 = proximo 15 ================ */
        /* ================================================== */
        if (idProducto == "101" || idProducto == "201" || idProducto == "301" || idProducto == "302")
        {
            if (hoy.Day >= 6 && hoy.Day <= 20)
            {
                var ultimoDiaDelMes = new DateTime(hoy.Year, hoy.Month, DateTime.DaysInMonth(hoy.Year, hoy.Month)).Day; // ultimo dia del mes
                diaPrimerPago = ultimoDiaDelMes > 30 ? 30 : ultimoDiaDelMes;
            }
            else if (hoy.Day >= 21 || hoy.Day <= 5)
            {
                if (hoy.Day > 5)
                {
                    mesPrimerPago = hoy.AddMonths(1).Month;
                    anioPrimerPago = hoy.AddMonths(1).Year;
                }

                diaPrimerPago = 15;
            }
        }
        /* ================================================== */
        /* ================== Mensualmente ================== */
        /* ========= Del 06 - 20 = prox. 30 mensual ========= */
        /* ========= Del 21 - 05 = prox. 15 mensual ========= */
        /* ================================================== */
        else if (idProducto == "202" || idProducto == "203" || idProducto == "204")
        {
            if (hoy.Day >= 6 && hoy.Day <= 20)
            {
                anioPrimerPago = hoy.AddMonths(1).Year;
                mesPrimerPago = hoy.AddMonths(1).Month;
                diaPrimerPago = 15;
            }
            else if (hoy.Day >= 21 || hoy.Day <= 5)
            {
                if (hoy.Day > 5)
                {
                    mesPrimerPago = hoy.AddMonths(1).Month;
                    anioPrimerPago = hoy.AddMonths(1).Year;
                }

                var ultimoDiaDelMes = new DateTime(anioPrimerPago, mesPrimerPago, DateTime.DaysInMonth(anioPrimerPago, mesPrimerPago)).Day; // ultimo dia del mes
                diaPrimerPago = ultimoDiaDelMes > 30 ? 30 : ultimoDiaDelMes;
            }
        }

        fechaDelPrimerPago = new DateTime(anioPrimerPago, mesPrimerPago, diaPrimerPago);

        return fechaDelPrimerPago;
    }

    #endregion

    #region View Models

    public class SolicitudesCredito_ActualizarSolicitud_Precalificado_ViewModel
    {
        public string IdClienteSAF { get; set; }
        public string TipoDeClienteSAF { get; set; }
        public bool PermitirIngresarSolicitud { get; set; }
        public string MensajePermitirIngresarSolicitud { get; set; }
        public string Identidad { get; set; }
        public string Rtn { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Telefono { get; set; }
        public decimal Obligaciones { get; set; }
        public decimal Ingresos { get; set; }
        public decimal Disponible { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int IdTipoDeSolicitud { get; set; }
        public string TipoDeSolicitud { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public string ScorePromedio { get; set; }
    }

    public class SolicitudesCredito_Actualizar_CalculoPrestamo_ViewModel
    {
        public int IdOrden { get; set; }
        public int Plazo { get; set; }
        public decimal TasaInteresAnual { get; set; }
        public decimal ValorGarantia { get; set; }
        public decimal TotalSeguroVehiculo { get; set; }
        public decimal GastosdeCierre { get; set; }
        public decimal CostoGPS { get; set; }
        public decimal TotalAFinanciar { get; set; }
        public decimal TotalIntereses { get; set; }
        public decimal TotalFinanciadoConIntereses { get; set; }
        public decimal CuotaDelPrestamo { get; set; }
        public decimal CuotaSegurodeVehiculo { get; set; }
        public decimal CuotaServicioGPS { get; set; }
        public decimal CuotaTotal { get; set; }
        public string TipoCuota { get; set; }
        public decimal SegurodeDeuda { get; set; }
        public decimal ValorDelPrestamo { get; set; }
    }

    public class SolicitudesCredito_Solicitud_Maestro_ViewModel
    {
        public int IdSolicitud { get; set; }
        public int IdCliente { get; set; }
        public decimal ValorSeleccionado { get; set; }
        public int PlazoSeleccionado { get; set; }
        public decimal ValorPrima { get; set; }
        public decimal ValorGarantia { get; set; }
        public int IdMoneda { get; set; }
        public int IdProducto { get; set; }
    }


    public class SolicitudesCredito_ActualizarSolicitud_Cliente_ViewModel
    {
        public int IdSolicitud { get; set; }
        public int IdCliente { get; set; }
        public Cliente_ViewModel Cliente { get; set; }
        public List<Condicion_ViewModel> Condiciones { get; set; }

        public SolicitudesCredito_ActualizarSolicitud_Cliente_ViewModel()
        {
            Condiciones = new List<Condicion_ViewModel>();
            Cliente = new Cliente_ViewModel();
        }
    }

    public class Condicion_ViewModel
    {
        public int IdSolicitudCondicion { get; set; }
        public int IdCondicion { get; set; }
        public int IdSolicitud { get; set; }
        public string TipoCondicion { get; set; }
        public string DescripcionCondicion { get; set; }
        public string ComentarioAdicional { get; set; }
        public bool Estado { get; set; }
    }

    public class Origenes_ViewModel
    {
        public int IdOrigen { get; set; }
        public string Origen { get; set; }
    }

    public class BarriosColonias_ViewModel
    {
        public int IdCiudadPoblado { get; set; }
        public int IdMunicipio { get; set; }
        public int IdDepartamento { get; set; }
        public int IdBarrioColonia { get; set; }
        public string NombreBarrioColonia { get; set; }
    }

    public class Ciudades_ViewModel
    {
        public int IdDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public int IdCiudadPoblado { get; set; }
        public string NombreCiudadPoblado { get; set; }
    }

    public class Municipios_ViewModel
    {
        public int IdDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public string NombreMunicipio { get; set; }
    }

    public class CoreFinanciero_TipoDocumento_ViewModel
    {
        public int IdTipoDocumento { get; set; }
        public string DescripcionTipoDocumento { get; set; }
        public int CantidadMaximaDoucmentos { get; set; }
        public int TipoVisibilidad { get; set; }
    }

    public class Cliente_ViewModel
    {
        /* Clientes maestro */
        public int IdCliente { get; set; }
        public int IdTipoCliente { get; set; }
        public string IdentidadCliente { get; set; }
        public string RtnCliente { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string TelefonoCliente { get; set; }
        public int IdNacionalidad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Correo { get; set; }
        public string ProfesionOficio { get; set; }
        public string Sexo { get; set; }
        public int IdEstadoCivil { get; set; }
        public int IdVivienda { get; set; }
        public int IdTiempoResidir { get; set; }
        public bool ClienteActivo { get; set; }
        public string RazonInactivo { get; set; }

        public Cliente_InformacionDomicilio_ViewModel InformacionDomicilio { get; set; }
        public Cliente_InformacionLaboral_ViewModel InformacionLaboral { get; set; }
        public Cliente_InformacionConyugal_ViewModel InformacionConyugal { get; set; }
        public List<Cliente_ReferenciaPersonal_ViewModel> ListaReferenciasPersonales { get; set; }
    }

    public class Cliente_InformacionDomicilio_ViewModel
    {
        public int IdInformacionDomicilio { get; set; }
        public int IdCliente { get; set; }
        public int IdSolicitud { get; set; }
        public string TelefonoCasa { get; set; }
        public int IdDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public int IdCiudadPoblado { get; set; }
        public int IdBarrioColonia { get; set; }
        public string DireccionDetallada { get; set; }
        public string ReferenciasDireccionDetallada { get; set; }
    }

    public class Cliente_InformacionLaboral_ViewModel
    {
        public int IdInformacionLaboral { get; set; }
        public int IdCliente { get; set; }
        public int IdSolicitud { get; set; }
        public string NombreTrabajo { get; set; }
        public decimal IngresosMensuales { get; set; }
        public string PuestoAsignado { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string TelefonoEmpresa { get; set; }
        public string ExtensionRecursosHumanos { get; set; }
        public string ExtensionCliente { get; set; }
        public string FuenteOtrosIngresos { get; set; }
        public decimal? ValorOtrosIngresos { get; set; }
        public int IdDepartamento { get; set; }
        public int IdMunicipio { get; set; }
        public int IdCiudadPoblado { get; set; }
        public int IdBarrioColonia { get; set; }
        public string DireccionDetalladaEmpresa { get; set; }
        public string ReferenciasDireccionDetallada { get; set; }
    }

    public class Cliente_InformacionConyugal_ViewModel
    {
        public int IdInformacionConyugal { get; set; }
        public int IdCliente { get; set; }
        public int IdSolicitud { get; set; }
        public string IdentidadConyugue { get; set; }
        public string NombreCompletoConyugue { get; set; }
        public string TelefonoConyugue { get; set; }
        public DateTime FechaNacimientoConyugue { get; set; }
        public string LugarTrabajoConyugue { get; set; }
        public string TelefonoTrabajoConyugue { get; set; }
        public decimal IngresosMensualesConyugue { get; set; }
    }

    public class Cliente_ReferenciaPersonal_ViewModel
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

}