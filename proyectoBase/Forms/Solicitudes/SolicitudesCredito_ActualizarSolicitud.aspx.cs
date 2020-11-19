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
    private string pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcIDSesion = "";
    public string IdSolicitud = "";
    public string IdCliente = "";
    private string pcIDApp = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    public List<TipoDocumento_ViewModel> DocumentosRequeridos = new List<TipoDocumento_ViewModel>();

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        if (!IsPostBack)
        {
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            string lcParametros;

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
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

                HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
                Session.Timeout = 10080;

                CargarListas();
                CargarInformacion();
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
                            DocumentosRequeridos.Add(new TipoDocumento_ViewModel()
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
                        while (sqlResultado.Read())
                        {
                            ddlParentescos.Items.Add(new ListItem(sqlResultado["fcDescripcionParentesco"].ToString(), sqlResultado["fiIDParentesco"].ToString()));
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
                        while (sqlResultado.Read())
                        {
                            ddlTiempoDeConocerReferencia.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeConocer"].ToString()));
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

                            /****** Documentos de la solicitud ******/
                            sqlResultado.NextResult();

                            /****** Condicionamientos de la solicitud ******/
                            sqlResultado.NextResult();

                            if (sqlResultado.HasRows)
                            {
                                var listaCondiciones = new List<int>();

                                var listaCondicionesDeDocumentacion = new int[] { 1, 2, 3, 4, 5, 6 };

                                HtmlTableRow tRowCondicion;
                                var btnFinalizarCondicion = string.Empty;
                                var lblEstadoCondicion = string.Empty;

                                /* Llenar table de referencias */
                                while (sqlResultado.Read())
                                {
                                    btnFinalizarCondicion = "<button id='btnFinalizarCondicion' data-id='" + sqlResultado["fiIDSolicitudCondicion"].ToString() + "' class='btn btn-sm btn-warning mb-0' type='button' title='Finalizar condicion'><i class='far fa-check-circle'></i> Finalizar</button>";
                                    lblEstadoCondicion = (bool)sqlResultado["fbEstadoCondicion"] == true ? "<label class='btn btn-sm btn-warning mb-0'>Pendiente<label>" : "<label class='btn btn-sm btn-success mb-0'>Completada<label>";

                                    tRowCondicion = new HtmlTableRow();
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcCondicion"].ToString() });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcDescripcionCondicion"].ToString() });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcComentarioAdicional"].ToString() });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerHtml = lblEstadoCondicion });
                                    //tRowCondicion.Cells.Add(new HtmlTableCell() { InnerHtml = btnFinalizarCondicion });
                                    tblCondiciones.Rows.Add(tRowCondicion);

                                    listaCondiciones.Add((int)sqlResultado["fiIDCondicion"]);




                                }

                                ValidarFormularioCondiciones(listaCondiciones);
                            }

                            /****** Información del cliente ******/
                            sqlResultado.NextResult();
                            sqlResultado.Read();

                            var fechaNacimiento = DateTime.Parse(sqlResultado["fdFechaNacimientoCliente"].ToString());
                            var hoy = DateTime.Today;
                            var edad = hoy.Year - fechaNacimiento.Year;
                            if (fechaNacimiento.Date > hoy.AddYears(-edad)) edad--;

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
                                liInformacionConyugal.Visible = false;
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

    [WebMethod]
    public static List<TipoDocumento_ViewModel> CargarDocumentosRequeridos()
    {
        return (List<TipoDocumento_ViewModel>)HttpContext.Current.Session["DocumentosRequeridos"];
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

    [WebMethod]
    public static bool ActualizarCondicionamiento(int idCondicion, int idCliente, string seccionFormulario, string objSeccion, string dataCrypt)
    {
        bool resultadoProceso = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var mensajeError = string.Empty;

            var resultadoActualizacion = string.Empty;
            var json_serializer = new JavaScriptSerializer();

            switch (seccionFormulario)
            {
                //case "Correccion Informacion de la Solicitud":
                // var objSolicitudesMaster = json_serializer.Deserialize<SolicitudesMasterViewModel>(objSeccion);
                // resultadoActualizacion = ActualizarSolicitudMaster(objSolicitudesMaster, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                // break;

                case "Correccion Informacion Personal":
                    var informacionPersonal = json_serializer.Deserialize<Cliente_ViewModel>(objSeccion);
                    resultadoActualizacion = ActualizarInformacionPersonal(informacionPersonal, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Correccion Informacion Domiciliar":
                    var informacionDomicilio = json_serializer.Deserialize<Cliente_InformacionDomicilio_ViewModel>(objSeccion);
                    resultadoActualizacion = ActualizarInformacionDomicilio(informacionDomicilio, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Correccion Informacion Laboral":
                    var informacionLaboral = json_serializer.Deserialize<Cliente_InformacionLaboral_ViewModel>(objSeccion);
                    resultadoActualizacion = ActualizarInformacionLaboral(informacionLaboral, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Correccion Informacion Conyugal":
                    var informacionConyugue = json_serializer.Deserialize<Cliente_InformacionConyugal_ViewModel>(objSeccion);
                    resultadoActualizacion = ActualizarInformacionConyugal(informacionConyugue, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Correccion Referencias":
                    var referenciasPersonales = json_serializer.Deserialize<List<Cliente_ReferenciaPersonal_ViewModel>>(objSeccion);
                    resultadoActualizacion = ActualizarReferenciasPersonales(referenciasPersonales, idCliente, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Cambio de Referencias":
                    var CambioReferenciasPersonales = json_serializer.Deserialize<List<Cliente_ReferenciaPersonal_ViewModel>>(objSeccion);
                    resultadoActualizacion = ActualizarReferenciasPersonales(CambioReferenciasPersonales, idCliente, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Documentacion":
                    resultadoActualizacion = ActualizarDocumentacion(idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;
            }

            if (resultadoActualizacion.StartsWith("-1") || resultadoActualizacion == string.Empty || resultadoActualizacion == "0")
            {
                return false;
            }

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredSolicitud_ActualizarCondicion", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitudCondicion", idCondicion);

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

                using (var sqlComando = new SqlCommand("sp_CREDCliente_Maestro_Update", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", cliente.IdCliente);
                    sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", cliente.IdentidadCliente);
                    sqlComando.Parameters.AddWithValue("@fcRTN", cliente.RtnCliente);
                    sqlComando.Parameters.AddWithValue("@fcPrimerNombreCliente", cliente.PrimerNombre);
                    sqlComando.Parameters.AddWithValue("@fcSegundoNombreCliente", cliente.SegundoNombre);
                    sqlComando.Parameters.AddWithValue("@fcPrimerApellidoCliente", cliente.PrimerApellido);
                    sqlComando.Parameters.AddWithValue("@fcSegundoApellidoCliente", cliente.SegundoApellido);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoCliente", cliente.TelefonoCliente);
                    sqlComando.Parameters.AddWithValue("@fiNacionalidadCliente", cliente.IdNacionalidad);
                    sqlComando.Parameters.AddWithValue("@fdFechaNacimientoCliente", cliente.FechaNacimiento);
                    sqlComando.Parameters.AddWithValue("@fcCorreoElectronicoCliente", cliente.Correo);
                    sqlComando.Parameters.AddWithValue("@fcProfesionOficioCliente", cliente.ProfesionOficio);
                    sqlComando.Parameters.AddWithValue("@fcSexoCliente", cliente.Sexo);
                    sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", cliente.IdEstadoCivil);
                    sqlComando.Parameters.AddWithValue("@fiIDVivienda", cliente.IdVivienda);
                    sqlComando.Parameters.AddWithValue("@fiTiempoResidir", cliente.IdTiempoResidir);
                    sqlComando.Parameters.AddWithValue("@fbClienteActivo", true);
                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

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

                using (var sqlComando = new SqlCommand("sp_CREDCliente_InformacionDomicilio_Update", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", clienteInformacionDomicilio.IdCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDDepartamento", clienteInformacionDomicilio.IdDepartamento);
                    sqlComando.Parameters.AddWithValue("@fiIDMunicipio", clienteInformacionDomicilio.IdMunicipio);
                    sqlComando.Parameters.AddWithValue("@fiIDCiudad", clienteInformacionDomicilio.IdCiudadPoblado);
                    sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", clienteInformacionDomicilio.IdBarrioColonia);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoCasa", clienteInformacionDomicilio.TelefonoCasa);
                    sqlComando.Parameters.AddWithValue("@fcDireccionDetallada", clienteInformacionDomicilio.DireccionDetallada);
                    sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", clienteInformacionDomicilio.ReferenciasDireccionDetallada);
                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        resultado = sqlResultado["MensajeError"].ToString();
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

                using (var sqlComando = new SqlCommand("sp_CREDCliente_InformacionLaboral_Update", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", clienteInformacionLaboral.IdCliente);
                    sqlComando.Parameters.AddWithValue("@fcNombreTrabajo", clienteInformacionLaboral.NombreTrabajo);
                    sqlComando.Parameters.AddWithValue("@fnIngresosMensuales", clienteInformacionLaboral.IngresosMensuales);
                    sqlComando.Parameters.AddWithValue("@fcPuestoAsignado", clienteInformacionLaboral.PuestoAsignado);
                    sqlComando.Parameters.AddWithValue("@fdFechaIngreso", clienteInformacionLaboral.FechaIngreso);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoEmpresa", clienteInformacionLaboral.TelefonoEmpresa);
                    sqlComando.Parameters.AddWithValue("@fiIDDepartamento", clienteInformacionLaboral.IdDepartamento);
                    sqlComando.Parameters.AddWithValue("@fiIDMunicipio", clienteInformacionLaboral.IdMunicipio);
                    sqlComando.Parameters.AddWithValue("@fiIDCiudad", clienteInformacionLaboral.IdCiudadPoblado);
                    sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", clienteInformacionLaboral.IdBarrioColonia);
                    sqlComando.Parameters.AddWithValue("@fcDireccionDetalladaEmpresa", clienteInformacionLaboral.DireccionDetalladaEmpresa);
                    sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", clienteInformacionLaboral.ReferenciasDireccionDetallada);
                    sqlComando.Parameters.AddWithValue("@fcFuenteOtrosIngresos", clienteInformacionLaboral.FuenteOtrosIngresos);
                    sqlComando.Parameters.AddWithValue("@fnValorOtrosIngresosMensuales", clienteInformacionLaboral.ValorOtrosIngresos);
                    sqlComando.Parameters.AddWithValue("@fcExtensionRecursosHumanos", clienteInformacionLaboral.ExtensionRecursosHumanos);
                    sqlComando.Parameters.AddWithValue("@fcExtensionCliente", clienteInformacionLaboral.ExtensionCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        resultado = sqlResultado["MensajeError"].ToString();
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

                using (var sqlComando = new SqlCommand("sp_CREDCliente_InformacionConyugal_Update", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", informacionConyugal.IdCliente);
                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoConyugue", informacionConyugal.NombreCompletoConyugue);
                    sqlComando.Parameters.AddWithValue("@fcIndentidadConyugue", informacionConyugal.IdentidadConyugue);
                    sqlComando.Parameters.AddWithValue("@fdFechaNacimientoConyugue", informacionConyugal.FechaNacimientoConyugue);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoConyugue", informacionConyugal.TelefonoConyugue);
                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoConyugue", informacionConyugal.LugarTrabajoConyugue);
                    sqlComando.Parameters.AddWithValue("@fnIngresosMensualesConyugue", informacionConyugal.IngresosMensualesConyugue);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoTrabajoConyugue", informacionConyugal.TelefonoTrabajoConyugue);
                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        resultado = sqlResultado["ConexionEncriptada"].ToString();
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

    public static string ActualizarReferenciasPersonales(List<Cliente_ReferenciaPersonal_ViewModel> referenciasPersonales, int idCliente, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
    {
        var resultado = string.Empty;
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var tran = sqlConexion.BeginTransaction())
                {
                    try
                    {
                        /* Obtener referencias personales existentes del cliente */
                        var referenciasExistentes = new List<Cliente_ReferenciaPersonal_ViewModel>();

                        using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Listar", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDCliente", idCliente);
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitud);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    referenciasExistentes.Add(new Cliente_ReferenciaPersonal_ViewModel()
                                    {
                                        IdReferencia = (int)sqlResultado["fiIDReferencia"],
                                        IdCliente = (int)sqlResultado["fiIDCliente"],
                                        NombreCompletoReferencia = (string)sqlResultado["fcNombreCompletoReferencia"],
                                    });
                                }
                            }
                        }

                        /* Identificar las nuevas referencias personales */
                        var referenciasInsertar = new List<Cliente_ReferenciaPersonal_ViewModel>();

                        foreach (Cliente_ReferenciaPersonal_ViewModel item in referenciasPersonales)
                        {
                            if (!referenciasExistentes.Select(x => x.IdReferencia).ToList().Contains(item.IdReferencia))
                            {
                                referenciasInsertar.Add(new Cliente_ReferenciaPersonal_ViewModel()
                                {
                                    NombreCompletoReferencia = item.NombreCompletoReferencia,
                                    LugarTrabajoReferencia = item.LugarTrabajoReferencia,
                                    IdTiempoConocerReferencia = item.IdTiempoConocerReferencia,
                                    TelefonoReferencia = item.TelefonoReferencia,
                                    IdParentescoReferencia = item.IdParentescoReferencia,
                                });
                            }
                        };

                        /* Guardar nuevas referencias personales */
                        if (referenciasInsertar.Count > 0)
                        {
                            foreach (Cliente_ReferenciaPersonal_ViewModel referencia in referenciasInsertar)
                            {
                                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Insert", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDCliente", idCliente);
                                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitud);
                                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoReferencia", referencia.NombreCompletoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoReferencia", referencia.LugarTrabajoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiTiempoConocerReferencia", referencia.IdTiempoConocerReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoReferencia", referencia.TelefonoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDParentescoReferencia", referencia.IdParentescoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                                    using (var sqlResultado = sqlComando.ExecuteReader())
                                    {
                                        while (sqlResultado.Read())
                                        {
                                            if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                            {
                                                return "-1";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        /* Identificar las referencias personales que se van a eliminar */
                        var referenciasInactivar = new List<Cliente_ReferenciaPersonal_ViewModel>();

                        foreach (Cliente_ReferenciaPersonal_ViewModel item in referenciasExistentes)
                        {
                            /* si la nueva lista de referencias personales no contiene alguna referencia de la lista vieja se debe eliminar dicha referencia */
                            if (!referenciasPersonales.Select(x => x.IdReferencia).ToList().Contains(item.IdReferencia))
                            {
                                referenciasInactivar.Add(new Cliente_ReferenciaPersonal_ViewModel()
                                {
                                    IdReferencia = item.IdReferencia,
                                });
                            }
                        };

                        if (referenciasInactivar.Count > 0)
                        {
                            foreach (Cliente_ReferenciaPersonal_ViewModel referencia in referenciasInactivar)
                            {
                                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Eliminar", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", referencia.IdReferencia);
                                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                    using (var sqlResultado = sqlComando.ExecuteReader())
                                    {
                                        while (sqlResultado.Read())
                                        {
                                            if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                            {
                                                return "-1";
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        /* Identificar las referencias personales que se van a editar */
                        var referenciasEditar = new List<Cliente_ReferenciaPersonal_ViewModel>();

                        foreach (Cliente_ReferenciaPersonal_ViewModel item in referenciasExistentes)
                        {
                            /* si la nueva lista de referencias contiene una referencia de la vieja, actualizar su informacion */
                            if (referenciasPersonales.Select(x => x.IdReferencia).ToList().Contains(item.IdReferencia))
                            {
                                referenciasEditar.Add(new Cliente_ReferenciaPersonal_ViewModel()
                                {
                                    IdReferencia = item.IdReferencia,
                                    NombreCompletoReferencia = item.NombreCompletoReferencia,
                                    LugarTrabajoReferencia = item.LugarTrabajoReferencia,
                                    IdTiempoConocerReferencia = item.IdTiempoConocerReferencia,
                                    TelefonoReferencia = item.TelefonoReferencia,
                                    IdParentescoReferencia = item.IdParentescoReferencia,
                                });
                            }
                        }

                        if (referenciasEditar.Count > 0)
                        {
                            foreach (Cliente_ReferenciaPersonal_ViewModel referencia in referenciasEditar)
                            {
                                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Update", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", referencia.IdParentescoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoReferencia", referencia.NombreCompletoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoReferencia", referencia.LugarTrabajoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiTiempoConocerReferencia", referencia.IdTiempoConocerReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoReferencia", referencia.TelefonoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDParentescoReferencia", referencia.IdParentescoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioModifica", pcIDUsuario);
                                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                                    sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                                    using (var sqlResultado = sqlComando.ExecuteReader())
                                    {
                                        while (sqlResultado.Read())
                                        {
                                            if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                            {
                                                return "-1";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        ex.Message.ToString();
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
                        using (var sqlComando = new SqlCommand("sp_CREDSolicitud_Documentos_Insert", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitud);
                            sqlComando.Parameters.AddWithValue("@fcNombreArchivo", documento.fcNombreArchivo);
                            sqlComando.Parameters.AddWithValue("@fcTipoArchivo", ".png");
                            sqlComando.Parameters.AddWithValue("@fcRutaArchivo", documento.fcRutaArchivo);
                            sqlComando.Parameters.AddWithValue("@fcURL", documento.URLArchivo);
                            sqlComando.Parameters.AddWithValue("@fiTipoDocumento", documento.fiTipoDocumento);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", "");
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", DateTime.Now);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
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

    [WebMethod]
    public static string ObtenerUrlEncriptado(int idCliente, string dataCrypt)
    {
        var lUrlDesencriptado = DesencriptarURL(dataCrypt);
        var idUsuario = HttpUtility.ParseQueryString(lUrlDesencriptado.Query).Get("usr");
        var idSolicitud = HttpUtility.ParseQueryString(lUrlDesencriptado.Query).Get("IDSOL");
        var pcIDApp = HttpUtility.ParseQueryString(lUrlDesencriptado.Query).Get("IDApp");
        var pcIDSesion = HttpUtility.ParseQueryString(lUrlDesencriptado.Query).Get("SID");

        return DSC.Encriptar("usr=" + idUsuario + "&IDSOL=" + idSolicitud + "&cltID=" + idCliente + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion);
    }

    public void ValidarFormularioCondiciones(List<int> listaCondiciones)
    {
        var listaCondicionesDeDocumentacion = new int[] { 1, 2, 3, 4, 5, 6 };

        /* validar si hay condiciones de documentación */
        if (listaCondiciones.Intersect(listaCondicionesDeDocumentacion).Any())
        {
            liDocumentacion.Visible = true;
        }

        /* validar si hay condiciones de referencias personales */
        if (listaCondiciones.Contains(8) || listaCondiciones.Contains(14))
        {
            liReferenciasPersonales.Visible = true;
        }

        /* condiciones de la información de la solicitud
        if (listaCondiciones.Contains(9))
        {
        liInformacionDeLaSolicitud.Visible = true;
        }
        */

        /* condiciones de informacion personal */
        if (listaCondiciones.Contains(10))
        {
            liInformacionPersonal.Visible = true;
        }

        /* condiciones de la información del domicilio */
        if (listaCondiciones.Contains(11))
        {
            liInformacionDomicilio.Visible = true;
        }

        /* condiciones de la informacion laboral */
        if (listaCondiciones.Contains(12))
        {
            liInformacionLaboral.Visible = true;
        }

        /* condiciones de la informacion conyugal */
        if (listaCondiciones.Contains(13))
        {
            liInformacionConyugal.Visible = true;
        }
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

    #region View Models

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

    public class TipoDocumento_ViewModel
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
        public string IdentidadConyugue { get; set; }
        public string NombreCompletoConyugue { get; set; }
        public string TelefonoTrabajoConyugue { get; set; }
        public DateTime FechaNacimientoConyugue { get; set; }
        public string TelefonoConyugue { get; set; }
        public string LugarTrabajoConyugue { get; set; }
        public decimal IngresosMensualesConyugue { get; set; }
    }

    public class Cliente_ReferenciaPersonal_ViewModel
    {
        public int IdReferencia { get; set; }
        public int IdCliente { get; set; }
        public string NombreCompletoReferencia { get; set; }
        public string TelefonoReferencia { get; set; }
        public string LugarTrabajoReferencia { get; set; }
        public short IdTiempoConocerReferencia { get; set; }
        public int IdParentescoReferencia { get; set; }
    }
    #endregion
}