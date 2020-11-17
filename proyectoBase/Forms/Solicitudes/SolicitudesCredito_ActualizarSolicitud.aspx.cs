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
    private string IdSolicitud = "";
    private string pcIDUsuario = "";
    private string pcIDSesion = "";
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

                            /****** Documentos de la solicitud ******/
                            sqlResultado.NextResult();

                            /****** Condicionamientos de la solicitud ******/
                            sqlResultado.NextResult();

                            if (sqlResultado.HasRows)
                            {
                                HtmlTableRow tRowCondicion;
                                var btnFinalizarCondicion = string.Empty;
                                var lblEstadoCondicion = string.Empty;

                                /* Llenar table de referencias */
                                while (sqlResultado.Read())
                                {
                                    btnFinalizarCondicion = "<button id='btnFinalizarCondicion' data-id='" + sqlResultado["fiIDCondicion"].ToString() + "' class='btn btn-sm btn-warning mb-0' type='button' title='Finalizar condicion'><i class='far fa-check-circle'></i> Finalizar</button>";
                                    lblEstadoCondicion = (bool)sqlResultado["fbEstadoCondicion"] == true ? "<label class='btn btn-sm btn-warning mb-0'>Pendiente<label>" : "<label class='btn btn-sm btn-success mb-0'>Completada<label>";

                                    tRowCondicion = new HtmlTableRow();
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcCondicion"].ToString() });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcDescripcionCondicion"].ToString() });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fcComentarioAdicional"].ToString() });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerHtml = lblEstadoCondicion });
                                    tRowCondicion.Cells.Add(new HtmlTableCell() { InnerHtml = btnFinalizarCondicion });
                                    tblCondiciones.Rows.Add(tRowCondicion);
                                }
                            }

                            /****** Información del cliente ******/
                            sqlResultado.NextResult();
                            sqlResultado.Read();

                            txtIdentidadCliente.Text = sqlResultado["fcIdentidadCliente"].ToString();
                            txtRtnCliente.Text = sqlResultado["fcRTN"].ToString();
                            txtPrimerNombre.Text = sqlResultado["fcPrimerNombreCliente"].ToString();
                            txtSegundoNombre.Text = sqlResultado["fcSegundoNombreCliente"].ToString();
                            txtPrimerApellido.Text = sqlResultado["fcPrimerApellidoCliente"].ToString();
                            txtSegundoApellido.Text = sqlResultado["fcSegundoApellidoCliente"].ToString();
                            ddlNacionalidad.SelectedValue = sqlResultado["fiNacionalidadCliente"].ToString();
                            txtCorreoElectronico.Text = sqlResultado["fcCorreoElectronicoCliente"].ToString();
                            txtProfesion.Text = sqlResultado["fcProfesionOficioCliente"].ToString();

                            if (sqlResultado["fcSexoCliente"].ToString() == "F")
                                rbSexoFemenino.Checked = true;
                            else
                                rbSexoMasculino.Checked = true;

                            ddlEstadoCivil.Text = sqlResultado["fiIDEstadoCivil"].ToString();
                            ddlTipoDeVivienda.Text = sqlResultado["fiIDVivienda"].ToString();
                            ddlTiempoDeResidir.Text = sqlResultado["fiTiempoResidir"].ToString();

                            /****** Información laboral ******/
                            sqlResultado.NextResult();

                            while (sqlResultado.Read())
                            {
                                txtNombreDelTrabajo.Text = sqlResultado["fcNombreTrabajo"].ToString();
                                txtFechaDeIngreso.Text = DateTime.Parse(sqlResultado["fdFechaIngreso"].ToString()).ToString("MM/dd/yyyy");
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
    public static bool ActualizarCondicionamiento(int idCondicionSolicitud, string seccionFormulario, string objSeccion, int idCliente, string dataCrypt)
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
                case "Correccion Informacion de la Solicitud":
                    var objSolicitudesMaster = json_serializer.Deserialize<SolicitudesMasterViewModel>(objSeccion);
                    resultadoActualizacion = ActualizarSolicitudMaster(objSolicitudesMaster, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Correccion Informacion Personal":
                    var objInfoPersonal = json_serializer.Deserialize<ClientesMasterViewModel>(objSeccion);
                    resultadoActualizacion = ActualizarInformacionPersonal(objInfoPersonal, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Correccion Informacion Domiciliar":
                    var objInforDomiciliar = json_serializer.Deserialize<ClientesInformacionDomiciliarViewModel>(objSeccion);
                    resultadoActualizacion = ActualizarInformacionDomiciliar(objInforDomiciliar, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Correccion Informacion Laboral":
                    var objClientesInformacionLaboral = json_serializer.Deserialize<ClientesInformacionLaboralViewModel>(objSeccion);
                    resultadoActualizacion = ActualizarInformacionLaboral(objClientesInformacionLaboral, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Correccion Informacion Conyugal":
                    var ClientesInformacionConyugal = json_serializer.Deserialize<ClientesInformacionConyugalViewModel>(objSeccion);
                    resultadoActualizacion = ActualizarInformacionConyugal(ClientesInformacionConyugal, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Correccion Referencias":
                    var clientesReferencias = json_serializer.Deserialize<List<ClientesReferenciasViewModel>>(objSeccion);
                    resultadoActualizacion = ActualizarReferenciasPersonales(clientesReferencias, idCliente, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
                    break;

                case "Cambio de Referencias":
                    var clientesCambioReferencias = json_serializer.Deserialize<List<ClientesReferenciasViewModel>>(objSeccion);
                    resultadoActualizacion = ActualizarReferenciasPersonales(clientesCambioReferencias, idCliente, idSolicitud, pcIDSesion, pcIDUsuario, pcIDApp);
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
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitudCondicion", idCondicionSolicitud);

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

    public static string ActualizarSolicitudMaster(SolicitudesMasterViewModel SolicitudesMaster, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
    {
        var resultado = string.Empty;

        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CREDSolicitud_Maestro_Update", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@fiIDTipoPrestamo", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fcTipoSolicitud", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fcTipoEmpresa", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fcTipoPerfil", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fcTipoEmpleado", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fcBuroActual", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fiMontoFinalSugerido", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fiMontoFinalFinanciar", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fiPlazoFinalAprobado", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fiSolicitudActiva", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fdValorPmoSugeridoSeleccionado", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fiPlazoPmoSeleccionado", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fdIngresoPrecalificado", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fdObligacionesPrecalificado", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fdDisponiblePrecalificado", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fnPrima", SolicitudesMaster.fnPrima);
                    sqlComando.Parameters.AddWithValue("@fnValorGarantia", SolicitudesMaster.fnValorGarantia);
                    sqlComando.Parameters.AddWithValue("@fiEdadCliente", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fiClienteArraigoLaboralAños", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fiClienteArraigoLaboralMeses", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@fiClienteArraigoLaboralDias", DBNull.Value);
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

    public static string ActualizarInformacionPersonal(ClientesMasterViewModel clienteMaster, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
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
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", clienteMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", clienteMaster.fcIdentidadCliente);
                    sqlComando.Parameters.AddWithValue("@fcRTN", clienteMaster.RTNCliente);
                    sqlComando.Parameters.AddWithValue("@fcPrimerNombreCliente", clienteMaster.fcPrimerNombreCliente);
                    sqlComando.Parameters.AddWithValue("@fcSegundoNombreCliente", clienteMaster.fcSegundoNombreCliente);
                    sqlComando.Parameters.AddWithValue("@fcPrimerApellidoCliente", clienteMaster.fcPrimerApellidoCliente);
                    sqlComando.Parameters.AddWithValue("@fcSegundoApellidoCliente", clienteMaster.fcSegundoApellidoCliente);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoCliente", clienteMaster.fcTelefonoCliente);
                    sqlComando.Parameters.AddWithValue("@fiNacionalidadCliente", clienteMaster.fiNacionalidadCliente);
                    sqlComando.Parameters.AddWithValue("@fdFechaNacimientoCliente", clienteMaster.fdFechaNacimientoCliente);
                    sqlComando.Parameters.AddWithValue("@fcCorreoElectronicoCliente", clienteMaster.fcCorreoElectronicoCliente);
                    sqlComando.Parameters.AddWithValue("@fcProfesionOficioCliente", clienteMaster.fcProfesionOficioCliente);
                    sqlComando.Parameters.AddWithValue("@fcSexoCliente", clienteMaster.fcSexoCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", clienteMaster.fiIDEstadoCivil);
                    sqlComando.Parameters.AddWithValue("@fiIDVivienda", clienteMaster.fiIDVivienda);
                    sqlComando.Parameters.AddWithValue("@fiTiempoResidir", clienteMaster.fiTiempoResidir);
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

    public static string ActualizarInformacionDomiciliar(ClientesInformacionDomiciliarViewModel ClientesInformacionDomiciliar, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
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
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", ClientesInformacionDomiciliar.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDDepartamento", ClientesInformacionDomiciliar.fiIDDepto);
                    sqlComando.Parameters.AddWithValue("@fiIDMunicipio", ClientesInformacionDomiciliar.fiIDMunicipio);
                    sqlComando.Parameters.AddWithValue("@fiIDCiudad", ClientesInformacionDomiciliar.fiIDCiudad);
                    sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", ClientesInformacionDomiciliar.fiIDBarrioColonia);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoCasa", ClientesInformacionDomiciliar.fcTelefonoCasa);
                    sqlComando.Parameters.AddWithValue("@fcDireccionDetallada", ClientesInformacionDomiciliar.fcDireccionDetallada);
                    sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", ClientesInformacionDomiciliar.fcReferenciasDireccionDetallada);
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

    public static string ActualizarInformacionLaboral(ClientesInformacionLaboralViewModel ClientesInformacionLaboral, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
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
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", ClientesInformacionLaboral.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fcNombreTrabajo", ClientesInformacionLaboral.fcNombreTrabajo);
                    sqlComando.Parameters.AddWithValue("@fnIngresosMensuales", ClientesInformacionLaboral.fiIngresosMensuales);
                    sqlComando.Parameters.AddWithValue("@fcPuestoAsignado", ClientesInformacionLaboral.fcPuestoAsignado);
                    sqlComando.Parameters.AddWithValue("@fdFechaIngreso", ClientesInformacionLaboral.fcFechaIngreso);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoEmpresa", ClientesInformacionLaboral.fdTelefonoEmpresa);
                    sqlComando.Parameters.AddWithValue("@fiIDDepartamento", ClientesInformacionLaboral.fiIDDepto);
                    sqlComando.Parameters.AddWithValue("@fiIDMunicipio", ClientesInformacionLaboral.fiIDMunicipio);
                    sqlComando.Parameters.AddWithValue("@fiIDCiudad", ClientesInformacionLaboral.fiIDCiudad);
                    sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", ClientesInformacionLaboral.fiIDBarrioColonia);
                    sqlComando.Parameters.AddWithValue("@fcDireccionDetalladaEmpresa", ClientesInformacionLaboral.fcDireccionDetalladaEmpresa);
                    sqlComando.Parameters.AddWithValue("@fcFuenteOtrosIngresos", ClientesInformacionLaboral.fcFuenteOtrosIngresos);
                    sqlComando.Parameters.AddWithValue("@fnValorOtrosIngresosMensuales", ClientesInformacionLaboral.fiValorOtrosIngresosMensuales);
                    sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", ClientesInformacionLaboral.fcReferenciasDireccionDetallada);
                    sqlComando.Parameters.AddWithValue("@fcExtensionRecursosHumanos", ClientesInformacionLaboral.fcExtensionRecursosHumanos);
                    sqlComando.Parameters.AddWithValue("@fcExtensionCliente", ClientesInformacionLaboral.fcExtensionCliente);
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

    public static string ActualizarInformacionConyugal(ClientesInformacionConyugalViewModel ClientesInformacionConyugal, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
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
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", ClientesInformacionConyugal.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoConyugue", ClientesInformacionConyugal.fcNombreCompletoConyugue);
                    sqlComando.Parameters.AddWithValue("@fcIndentidadConyugue", ClientesInformacionConyugal.fcIndentidadConyugue);
                    sqlComando.Parameters.AddWithValue("@fdFechaNacimientoConyugue", ClientesInformacionConyugal.fdFechaNacimientoConyugue);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoConyugue", ClientesInformacionConyugal.fcTelefonoConyugue);
                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoConyugue", ClientesInformacionConyugal.fcLugarTrabajoConyugue);
                    sqlComando.Parameters.AddWithValue("@fnIngresosMensualesConyugue", ClientesInformacionConyugal.fcIngresosMensualesConyugue);
                    sqlComando.Parameters.AddWithValue("@fcTelefonoTrabajoConyugue", ClientesInformacionConyugal.fcTelefonoTrabajoConyugue);
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

    public static string ActualizarReferenciasPersonales(List<ClientesReferenciasViewModel> ClientesReferencias, int idCliente, string idSolicitud, string pcIDSesion, string pcIDUsuario, string pcIDApp)
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
                        var referenciasExistentes = new List<ClientesReferenciasViewModel>();

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
                                    referenciasExistentes.Add(new ClientesReferenciasViewModel()
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

                        /* Identificar las nuevas referencias personales */
                        var referenciasInsertar = new List<ClientesReferenciasViewModel>();

                        foreach (ClientesReferenciasViewModel item in ClientesReferencias)
                        {
                            if (!referenciasExistentes.Select(x => x.fiIDReferencia).ToList().Contains(item.fiIDReferencia))
                            {
                                referenciasInsertar.Add(new ClientesReferenciasViewModel()
                                {
                                    fcNombreCompletoReferencia = item.fcNombreCompletoReferencia,
                                    fcLugarTrabajoReferencia = item.fcLugarTrabajoReferencia,
                                    fiTiempoConocerReferencia = item.fiTiempoConocerReferencia,
                                    fcTelefonoReferencia = item.fcTelefonoReferencia,
                                    fiIDParentescoReferencia = item.fiIDParentescoReferencia,
                                });
                            }
                        };

                        /* Guardar nuevas referencias personales */
                        if (referenciasInsertar.Count > 0)
                        {
                            foreach (ClientesReferenciasViewModel referencia in referenciasInsertar)
                            {
                                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Insert", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDCliente", idCliente);
                                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitud);
                                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoReferencia", referencia.fcNombreCompletoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoReferencia", referencia.fcLugarTrabajoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiTiempoConocerReferencia", referencia.fiTiempoConocerReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoReferencia", referencia.fcTelefonoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDParentescoReferencia", referencia.fiIDParentescoReferencia);
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
                        var referenciasInactivar = new List<ClientesReferenciasViewModel>();

                        foreach (ClientesReferenciasViewModel item in referenciasExistentes)
                        {
                            /* si la nueva lista de referencias personales no contiene alguna referencia de la lista vieja se debe eliminar dicha referencia */
                            if (!ClientesReferencias.Select(x => x.fiIDReferencia).ToList().Contains(item.fiIDReferencia))
                            {
                                referenciasInactivar.Add(new ClientesReferenciasViewModel()
                                {
                                    fiIDReferencia = item.fiIDReferencia,
                                });
                            }
                        };

                        if (referenciasInactivar.Count > 0)
                        {
                            foreach (ClientesReferenciasViewModel referencia in referenciasInactivar)
                            {
                                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Eliminar", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", referencia.fiIDReferencia);
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
                        var referenciasEditar = new List<ClientesReferenciasViewModel>();

                        foreach (ClientesReferenciasViewModel item in referenciasExistentes)
                        {
                            /* si la nueva lista de referencias contiene una referencia de la vieja, actualizar su informacion */
                            if (ClientesReferencias.Select(x => x.fiIDReferencia).ToList().Contains(item.fiIDReferencia))
                            {
                                referenciasEditar.Add(new ClientesReferenciasViewModel()
                                {
                                    fiIDReferencia = item.fiIDReferencia,
                                    fcNombreCompletoReferencia = item.fcNombreCompletoReferencia,
                                    fcLugarTrabajoReferencia = item.fcLugarTrabajoReferencia,
                                    fiTiempoConocerReferencia = item.fiTiempoConocerReferencia,
                                    fcTelefonoReferencia = item.fcTelefonoReferencia,
                                    fiIDParentescoReferencia = item.fiIDParentescoReferencia,
                                });
                            }
                        }

                        if (referenciasEditar.Count > 0)
                        {
                            foreach (ClientesReferenciasViewModel referencia in referenciasEditar)
                            {
                                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Update", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDReferencia", referencia.fiIDReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoReferencia", referencia.fcNombreCompletoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoReferencia", referencia.fcLugarTrabajoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiTiempoConocerReferencia", referencia.fiTiempoConocerReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoReferencia", referencia.fcTelefonoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDParentescoReferencia", referencia.fiIDParentescoReferencia);
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
    public static List<CondicionesViewModel> DetallesCondicion(string dataCrypt)
    {
        var condicionesSolicitud = new List<CondicionesViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_SolicitudCondiciones_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitud);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            condicionesSolicitud.Add(new CondicionesViewModel()
                            {
                                IDSolicitudCondicion = (int)sqlResultado["fiIDSolicitudCondicion"],
                                IDCondicion = (int)sqlResultado["fiIDCondicion"],
                                IDSolicitud = (int)sqlResultado["fiIDSolicitud"],
                                TipoCondicion = sqlResultado["fcCondicion"].ToString(),
                                DescripcionCondicion = sqlResultado["fcDescripcionCondicion"].ToString(),
                                ComentarioAdicional = sqlResultado["fcComentarioAdicional"].ToString(),
                                Estado = (bool)sqlResultado["fbEstadoCondicion"]
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
        return condicionesSolicitud;
    }

    [WebMethod]
    public static string ObtenerUrlEncriptado(int idCliente, string dataCrypt)
    {
        Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
        var idUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
        var idSolicitud = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
        var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

        return DSC.Encriptar("usr=" + idUsuario + "&IDSOL=" + idSolicitud + "&cltID=" + idCliente + "&IDApp=" + pcIDApp + "&SID=" + pcIDSesion);
    }

    public static Uri DesencriptarURL(string URL)
    {
        Uri lURLDesencriptado = null;
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
                lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return lURLDesencriptado;
    }

    #region View Models

    public class CondicionesViewModel
    {
        public int IDSolicitudCondicion { get; set; }
        public int IDCondicion { get; set; }
        public int IDSolicitud { get; set; }
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

    public class Precalificado_ViewModel
    {
        public string IdClienteSAF { get; set; }
        public string TipoDeClienteSAF { get; set; }
        public bool PermitirIngresarSolicitud { get; set; }
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
        public CotizadorProductos_ViewModel PrestamoMaximoSugerido { get; set; }
    }

    public class CotizadorProductos_ViewModel
    {
        public int IdCotizacion { get; set; }
        public int IdProducto { get; set; }
        public string Producto { get; set; }
        public decimal MontoOfertado { get; set; }
        public int Plazo { get; set; }
        public string TipoPlazo { get; set; }
        public decimal Cuota { get; set; }
    }
    public class CalculoPrestamo_ViewModel
    {
        public decimal SegurodeDeuda { get; set; }
        public decimal GastosdeCierre { get; set; }
        public decimal ValoraFinanciar { get; set; }
        public decimal CuotaQuincenal { get; set; }
        public decimal CuotaMensual { get; set; }
        public decimal SegurodeVehiculo { get; set; }
        public decimal CostoGPS { get; set; }
        public decimal CuotaServicioGPS { get; set; }
        public decimal TotalSeguroVehiculo { get; set; }
        public decimal CuotaSegurodeVehiculo { get; set; }
        public decimal CuotaMensualNeta { get; set; }
        public string TipoCuota { get; set; }
    }

    public class SolicitudesCredito_Registrar_Constantes
    {
        public DateTime HoraAlCargar { get; set; }
        public bool EsClienteNuevo { get; set; }
        public int IdCliente { get; set; }
        public decimal PrestamoMaximo_Monto { get; set; }
        public decimal PrestamoMaximo_Cuota { get; set; }
        public int PrestamoMaximo_Plazo { get; set; }
        public string PrestamoMaximo_TipoDePlazo { get; set; }
        public bool EstadoCliente { get; internal set; }
        public string RazonInactivo { get; internal set; }
        public string TipoDePlazo { get; set; }
        public string IdentidadCliente { get; set; }

        /* Parametros de logica de negocio que dependen del tipo de producto */
        public decimal CapacidadDePagoCliente { get; set; }
        public decimal? PorcentajePrimaMinima { get; set; }
        public decimal? MontoFinanciarMinimo { get; set; }
        public decimal? MontoFinanciarMaximo { get; set; }
        public int CantidadMinimaDeReferenciasPersonales { get; set; }

        public int RequiereGarantia { get; set; }
        public string TipoDeGarantiaRequerida { get; set; }
        public int RequiereGPS { get; set; }
        public int RequierePrima { get; set; }
        public int RequiereOrigen { get; set; }

        public int IdTipoDePlazo { get; set; }
        public int PlazoMinimo { get; set; }
        public int PlazoMaximo { get; set; }

        public int? PlazoMaximoCliente { get; set; }
        public decimal? MontoFinanciarMaximoCliente { get; set; }

        public SolicitudesCredito_Registrar_Constantes()
        {
            HoraAlCargar = DateTime.Now;
            EsClienteNuevo = true;
            IdCliente = 0;
            CantidadMinimaDeReferenciasPersonales = 4;
        }
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
        public string IndentidadConyugue { get; set; }
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

    public class Solicitud_Maestro_ViewModel
    {
        public int IdSolicitud { get; set; }
        public int IdCliente { get; set; }
        public int IdProducto { get; set; }
        public int IdTipoDeSolicitud { get; set; }
        public int IdTipoMoneda { get; set; }
        public decimal ValorSeleccionado { get; set; }
        public int PlazoSeleccionado { get; set; }
        public decimal ValorPrima { get; set; }
        public decimal ValorGlobal { get; set; }
        public int IdOrigen { get; set; }
        public DateTime EnIngresoInicio { get; set; }
    }

    public class Garantia_ViewModel
    {
        public int IdGarantia { get; set; }
        public string NumeroPrestamo { get; set; }
        public string VIN { get; set; }
        public string TipoDeGarantia { get; set; }
        public string TipoDeVehiculo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Anio { get; set; }
        public string Color { get; set; }
        public string Cilindraje { get; set; }
        public decimal Recorrido { get; set; }
        public string UnidadDeDistancia { get; set; }
        public string Transmision { get; set; }
        public string TipoDeCombustible { get; set; }
        public string Matricula { get; set; }
        public string SerieUno { get; set; }
        public string SerieDos { get; set; }
        public string SerieChasis { get; set; }
        public string SerieMotor { get; set; }
        public string GPS { get; set; }
        public string Comentario { get; set; }
        public bool EsDigitadoManualmente { get; set; }
        public decimal ValorMercado { get; set; }
        public decimal ValorPrima { get; set; }
        public decimal ValorFinanciado { get; set; }
        public decimal GastosDeCierre { get; set; }
    }

    #endregion
}