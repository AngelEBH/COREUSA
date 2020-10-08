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
using System.Web.Services;
using System.Web.UI.WebControls;

public partial class SolicitudesCredito_Registrar : System.Web.UI.Page
{
    private static string pcID = "";
    private static string pcIDApp = "";
    private static string pcIDSesion = "";
    private static string pcIDUsuario = "";
    private static DSCore.DataCrypt DSC;
    public static Precalificado_ViewModel Precalificado;
    public static SolicitudesCredito_Registrar_Constantes Constantes;
    public static List<TipoDocumento_ViewModel> DocumentosRequeridos;

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        /* Captura de parámetros encriptados */
        if (!IsPostBack)
        {
            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            DSC = new DSCore.DataCrypt();
            Precalificado = new Precalificado_ViewModel();
            Constantes = new SolicitudesCredito_Registrar_Constantes();
            DocumentosRequeridos = new List<TipoDocumento_ViewModel>();

            string lcParametros;
            if (liParamStart > 0)
                lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
            else
                lcParametros = String.Empty;

            if (lcParametros != String.Empty)
            {
                var pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

                CargarPrecalificado();
                ValidarClienteSolicitudesActivas();
                CargarListas();
                CargarOrigenes();
                ObtenerInformacionCliente();
            }
        }

        /* Guardar documentos de la solicitud */
        if (type != null || Request.HttpMethod == "POST")
        {
            Session["tipoDoc"] = Convert.ToInt32(Request.QueryString["doc"]); ;
            var uploadDir = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";

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
        else
        {
            HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
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
                            Precalificado.Identidad = sqlResultado["fcIdentidad"].ToString();
                            //Precalificado.Rtn = sqlResultado["fcRTN"].ToString();
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

                            txtIdentidadCliente.Text = Precalificado.Identidad;
                            txtRtnCliente.Text = Precalificado.Rtn;
                            txtPrimerNombre.Text = Precalificado.PrimerNombre;
                            txtSegundoNombre.Text = Precalificado.SegundoNombre;
                            txtPrimerApellido.Text = Precalificado.PrimerApellido;
                            txtSegundoApellido.Text = Precalificado.SegundoApellido;
                            txtIngresosPrecalificados.Text = Precalificado.Ingresos.ToString();
                            txtNumeroTelefono.Text = Precalificado.Telefono;
                            lblTipodeSolicitud.InnerText = Precalificado.TipoDeSolicitud;
                            lblProducto.InnerText = Precalificado.Producto;
                        }
                    }
                } // using sp consulta ejecutivos

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CotizadorProductos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", Precalificado.IdProducto);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", Precalificado.Identidad);
                    sqlComando.Parameters.AddWithValue("@piConObligaciones", Precalificado.Obligaciones > 0 ? "1" : "0");
                    sqlComando.Parameters.AddWithValue("@pnIngresosBrutos", Precalificado.Ingresos);
                    sqlComando.Parameters.AddWithValue("@pnIngresosDisponible", Precalificado.Disponible);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /* Obtener el préstamo máximo que se le puede ofertar al cliente para validaciones */
                        CotizadorProductos_ViewModel prestamoMaximoSegurido = new CotizadorProductos_ViewModel();

                        decimal montoMayor = 0;
                        int IdContador = 1;

                        while (sqlResultado.Read())
                        {
                            if (montoMayor < decimal.Parse(sqlResultado["fnMontoOfertado"].ToString()))
                            {
                                prestamoMaximoSegurido = new CotizadorProductos_ViewModel()
                                {
                                    IdCotizacion = IdContador,
                                    IdProducto = int.Parse(sqlResultado["fiIDProducto"].ToString()),
                                    Producto = sqlResultado["fcProducto"].ToString(),
                                    MontoOfertado = decimal.Parse(sqlResultado["fnMontoOfertado"].ToString()),
                                    Plazo = int.Parse(sqlResultado["fiPlazo"].ToString()),
                                    Cuota = decimal.Parse(sqlResultado["fnCuotaQuincenal"].ToString()),
                                    TipoPlazo = sqlResultado["fcTipodeCuota"].ToString()
                                };
                            }
                            montoMayor = decimal.Parse(sqlResultado["fnMontoOfertado"].ToString());
                            IdContador++;
                        }

                        Precalificado.PrestamoMaximoSugerido = prestamoMaximoSegurido;
                    }
                } // using sp cotizador productos
            }// using conexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public void ValidarClienteSolicitudesActivas()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                /* Validar si este cliente tiene solicitudes de crédito activas */
                using (var sqlComando = new SqlCommand("dbo.sp_CredSolicitud_ValidarClienteSolicitudesActivas", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", 0);
                    sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", pcID);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (int.Parse(sqlResultado["fiClienteSolicitudesActivas"].ToString()) > 0)
                            {
                                lblMensaje.InnerText = "(Este cliente ya cuenta con una solicitud de crédito activa, esperar resolución)";
                                lblMensaje.Visible = true;
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
    }

    public void CargarListas()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Guardar_LlenarListas", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /* Departamentos */
                        ddlDepartamentoDomicilio.Items.Clear();
                        ddlDepartamentoDomicilio.Items.Add(new ListItem("Seleccionar", "0"));
                        ddlDepartamentoEmpresa.Items.Clear();
                        ddlDepartamentoEmpresa.Items.Add(new ListItem("Seleccionar", "0"));
                        while (sqlResultado.Read())
                        {
                            ddlDepartamentoDomicilio.Items.Add(new ListItem(sqlResultado["fcDepartamento"].ToString(), sqlResultado["fiCodDepartamento"].ToString()));
                            ddlDepartamentoEmpresa.Items.Add(new ListItem(sqlResultado["fcDepartamento"].ToString(), sqlResultado["fiCodDepartamento"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Viviendas */
                        ddlTipoDeVivienda.Items.Clear();
                        ddlTipoDeVivienda.Items.Add(new ListItem("Seleccionar", "0"));
                        while (sqlResultado.Read())
                        {
                            ddlTipoDeVivienda.Items.Add(new ListItem(sqlResultado["fcDescripcionVivienda"].ToString(), sqlResultado["fiIDVivienda"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Estados civiles */
                        ddlEstadoCivil.Items.Clear();
                        ddlEstadoCivil.Items.Add(new ListItem("Seleccionar", "0"));
                        while (sqlResultado.Read())
                        {
                            ddlTipoDeVivienda.Items.Add(new ListItem(sqlResultado["fcDescripcionEstadoCivil"].ToString(), sqlResultado["fiIDEstadoCivil"].ToString()));
                            //fbRequiereInformacionConyugal
                        }

                        sqlResultado.NextResult();

                        /* Nacionalidades */
                        ddlNacionalidad.Items.Clear();
                        ddlNacionalidad.Items.Add(new ListItem("Seleccionar", "0"));
                        while (sqlResultado.Read())
                        {
                            ddlNacionalidad.Items.Add(new ListItem(sqlResultado["fcDescripcionNacionalidad"].ToString(), sqlResultado["fiIDNacionalidad"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Tipos de documentos */
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

                        sqlResultado.NextResult();

                        /* Parentescos */
                        ddlParentescos.Items.Clear();
                        ddlParentescos.Items.Add(new ListItem("Seleccionar", "0"));
                        while (sqlResultado.Read())
                        {
                            ddlParentescos.Items.Add(new ListItem(sqlResultado["fcDescripcionParentesco"].ToString(), sqlResultado["fiIDParentesco"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Tiempo de residir */
                        ddlTiempoDeResidir.Items.Clear();
                        ddlTiempoDeResidir.Items.Add(new ListItem("Seleccionar", "0"));
                        while (sqlResultado.Read())
                        {
                            ddlTiempoDeResidir.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeResidir"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Tiempo de conocer referencia */
                        ddlTiempoDeConocerReferencia.Items.Clear();
                        ddlTiempoDeConocerReferencia.Items.Add(new ListItem("Seleccionar", "0"));
                        while (sqlResultado.Read())
                        {
                            ddlTiempoDeConocerReferencia.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeConocer"].ToString()));
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

    public void CargarOrigenes()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredCatalogo_Origenes", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDProducto", Precalificado.IdProducto);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlOrigen.Items.Clear();
                        ddlOrigen.Items.Add(new ListItem("Seleccionar origen", "0"));

                        while (sqlResultado.Read())
                        {
                            ddlOrigen.Items.Add(new ListItem(sqlResultado["fcOrigen"].ToString(), sqlResultado["fiIDOrigen"].ToString()));
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

    public void ObtenerInformacionCliente()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_ObtenerInformacionPorIdentidad", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", pcID);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var reader = sqlComando.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return;

                        /* Clientes Maestro */
                        while (reader.Read())
                        {
                            Constantes.EsClienteNuevo = false;
                            Constantes.IdCliente = int.Parse(reader["fiIDCliente"].ToString());
                            Constantes.EstadoCliente = (bool)reader["fbClienteActivo"];
                            Constantes.RazonInactivo = reader["fcRazonInactivo"].ToString();

                            txtIdentidadCliente.Text = reader["fcIdentidadCliente"].ToString();
                            txtRtnCliente.Text = reader["fcRTN"].ToString();
                            txtPrimerNombre.Text = reader["fcPrimerNombreCliente"].ToString();
                            txtSegundoNombre.Text = reader["fcSegundoNombreCliente"].ToString();
                            txtPrimerApellido.Text = reader["fcPrimerApellidoCliente"].ToString();
                            txtSegundoApellido.Text = reader["fcSegundoApellidoCliente"].ToString();
                            //txtNumeroTelefono.Text = reader["fcTelefonoPrimarioCliente"].ToString();
                            ddlNacionalidad.SelectedValue = reader["fiNacionalidadCliente"].ToString();

                            DateTime FechaNacimientoCliente = (DateTime)reader["fdNacimientoCliente"];
                            txtFechaDeNacimiento.Text = FechaNacimientoCliente.ToString("MM/dd/yyyy");
                            
                            /* Calcular edad del cliente */
                            var hoy = DateTime.Today;
                            var edad = hoy.Year - FechaNacimientoCliente.Year;
                            if (FechaNacimientoCliente.Date > hoy.AddYears(-edad)) edad--;

                            txtEdadDelCliente.Text = edad + " años";
                            txtCorreoElectronico.Text = reader["fcCorreoElectronicoCliente"].ToString();
                            txtProfesion.Text = reader["fcProfesionOficioCliente"].ToString();

                            if (reader["fcSexoCliente"].ToString() == "F")
                            {
                                rbSexoFemenino.Checked = true;
                            }
                            else
                            {
                                rbSexoMasculino.Checked = true;
                            }
                            ddlEstadoCivil.Text = reader["fiIDEstadoCivil"].ToString();
                            ddlTipoDeVivienda.Text = reader["fiIDVivienda"].ToString();
                            ddlTiempoDeResidir.Text = reader["fiTiempoResidir"].ToString();
                        }

                        reader.NextResult();

                        /* Información de domicilio */
                        while (reader.Read())
                        {
                            txtTelefonoCasa.Text = reader["fcTelefonoCasa"].ToString();
                            txtDireccionDetalladaDomicilio.Text = reader["fcDireccionDetalladaDomicilio"].ToString();
                            txtReferenciasDelDomicilio.Value = reader["fcReferenciasDireccionDetalladaDomicilio"].ToString();
                            
                            /* Departamento */
                            ddlDepartamentoDomicilio.SelectedValue = reader["fiCodDepartamento"].ToString();

                            /* Municipio del domicilio */
                            var municipiosDeDepartamento = CargarMunicipios(int.Parse(reader["fiCodDepartamento"].ToString()));

                            ddlMunicipioDomicilio.Items.Clear();
                            ddlMunicipioDomicilio.Items.Add(new ListItem("Seleccionar", "0"));

                            municipiosDeDepartamento.ForEach(municipio =>
                            {
                                ddlMunicipioDomicilio.Items.Add(new ListItem(municipio.NombreMunicipio, municipio.IdMunicipio.ToString()));
                            });
                            ddlMunicipioDomicilio.SelectedValue = reader["fiCodMunicipio"].ToString();
                            ddlMunicipioDomicilio.Enabled = true;

                            /* Ciudad o Poblado del domicilio */
                            var ciudadesPobladosDelMunicipio = CargarCiudadesPoblados(int.Parse(reader["fiCodDepartamento"].ToString()), int.Parse(reader["fiCodMunicipio"].ToString()));

                            ddlCiudadPobladoDomicilio.Items.Clear();
                            ddlCiudadPobladoDomicilio.Items.Add(new ListItem("Seleccionar", "0"));

                            ciudadesPobladosDelMunicipio.ForEach(ciudadPoblado =>
                            {
                                ddlCiudadPobladoDomicilio.Items.Add(new ListItem(ciudadPoblado.NombreCiudadPoblado, ciudadPoblado.IdCiudadPoblado.ToString()));
                            });
                            ddlCiudadPobladoDomicilio.SelectedValue = reader["fiCodPoblado"].ToString();
                            ddlCiudadPobladoDomicilio.Enabled = true;

                            /* Barrio o colonia del domicilio */
                            var barriosColoniasDelPoblado = CargarBarriosColonias(int.Parse(reader["fiCodDepartamento"].ToString()), int.Parse(reader["fiCodMunicipio"].ToString()), int.Parse(reader["fiCodPoblado"].ToString()));

                            ddlBarrioColoniaDomicilio.Items.Clear();
                            ddlBarrioColoniaDomicilio.Items.Add(new ListItem("Seleccionar", "0"));

                            barriosColoniasDelPoblado.ForEach(barrioColonia =>
                            {
                                ddlBarrioColoniaDomicilio.Items.Add(new ListItem(barrioColonia.NombreBarrioColonia, barrioColonia.IdBarrioColonia.ToString()));
                            });
                            ddlBarrioColoniaDomicilio.SelectedValue = reader["fiCodBarrio"].ToString();
                            ddlBarrioColoniaDomicilio.Enabled = true;
                        }

                        reader.NextResult();

                        /* Información laboral */
                        while (reader.Read())
                        {
                            txtNombreDelTrabajo.Text = reader["fcNombreTrabajo"].ToString();
                            txtFechaDeIngreso.Text = DateTime.Parse(reader["fdFechaIngreso"].ToString()).ToString("MM/dd/yyyy");                            
                            txtPuestoAsignado.Text = reader["fcPuestoAsignado"].ToString();
                            txtIngresosMensuales.Text = reader["fnIngresosMensuales"].ToString();

                            txtTelefonoEmpresa.Text = reader["fcTelefonoEmpresa"].ToString();
                            txtExtensionRecursosHumanos.Text = reader["fcExtensionRecursosHumanos"].ToString();
                            txtExtensionCliente.Text = reader["fcExtensionCliente"].ToString();
                            txtFuenteDeOtrosIngresos.Text = reader["fcFuenteOtrosIngresos"].ToString();
                            txtValorOtrosIngresos.Text = reader["fnValorOtrosIngresosMensuales"].ToString();
                            txtDireccionDetalladaEmpresa.Text = reader["fcDireccionDetalladaEmpresa"].ToString();
                            txtReferenciasEmpresa.Value = reader["fcReferenciasDireccionDetalladaEmpresa"].ToString();

                            /* Departamento de la empresa */
                            ddlDepartamentoEmpresa.SelectedValue = reader["fiIDDepartamento"].ToString();

                            /* Municipio de la empresa */
                            var municipiosDeDepartamento = CargarMunicipios(int.Parse(reader["fiIDDepartamento"].ToString()));

                            ddlMunicipioEmpresa.Items.Clear();
                            ddlMunicipioEmpresa.Items.Add(new ListItem("Seleccionar", "0"));

                            municipiosDeDepartamento.ForEach(municipio =>
                            {
                                ddlMunicipioEmpresa.Items.Add(new ListItem(municipio.NombreMunicipio, municipio.IdMunicipio.ToString()));
                            });
                            ddlMunicipioEmpresa.SelectedValue = reader["fiIDMunicipio"].ToString();
                            ddlMunicipioEmpresa.Enabled = true;

                            /* Ciudad o Poblado de la empresa */
                            var ciudadesPobladosDelMunicipio = CargarCiudadesPoblados(int.Parse(reader["fiIDDepartamento"].ToString()), int.Parse(reader["fiIDMunicipio"].ToString()));

                            ddlCiudadPobladoEmpresa.Items.Clear();
                            ddlCiudadPobladoEmpresa.Items.Add(new ListItem("Seleccionar", "0"));

                            ciudadesPobladosDelMunicipio.ForEach(ciudadPoblado =>
                            {
                                ddlCiudadPobladoEmpresa.Items.Add(new ListItem(ciudadPoblado.NombreCiudadPoblado, ciudadPoblado.IdCiudadPoblado.ToString()));
                            });
                            ddlCiudadPobladoEmpresa.SelectedValue = reader["fiIDCiudad"].ToString();
                            ddlCiudadPobladoEmpresa.Enabled = true;

                            /* Barrio o colonia de la empresa */
                            var barriosColoniasDelPoblado = CargarBarriosColonias(int.Parse(reader["fiIDDepartamento"].ToString()), int.Parse(reader["fiIDMunicipio"].ToString()), int.Parse(reader["fiIDCiudad"].ToString()));

                            ddlBarrioColoniaEmpresa.Items.Clear();
                            ddlBarrioColoniaEmpresa.Items.Add(new ListItem("Seleccionar", "0"));

                            barriosColoniasDelPoblado.ForEach(barrioColonia =>
                            {
                                ddlBarrioColoniaEmpresa.Items.Add(new ListItem(barrioColonia.NombreBarrioColonia, barrioColonia.IdBarrioColonia.ToString()));
                            });
                            ddlBarrioColoniaEmpresa.SelectedValue = reader["fiIDBarrioColonia"].ToString();
                            ddlBarrioColoniaEmpresa.Enabled = true;
                        }

                        reader.NextResult();

                        /* Información del conyugue */
                        while (reader.Read())
                        {
                            txtIdentidadConyugue.Text = reader["fcIndentidadConyugue"].ToString();
                            txtNombresConyugue.Text = reader["fcNombreCompletoConyugue"].ToString();
                            txtFechaNacimientoConyugue.Text = DateTime.Parse(reader["fdFechaNacimientoConyugue"].ToString()).ToString("MM/dd/yyyy");
                            txtTelefonoConyugue.Text = reader["fcTelefonoConyugue"].ToString();
                            txtLugarDeTrabajoConyuge.Text = reader["fcLugarTrabajoConyugue"].ToString();
                            txtIngresosMensualesConyugue.Text = reader["fnIngresosMensualesConyugue"].ToString();
                            txtTelefonoTrabajoConyugue.Text = reader["fcTelefonoTrabajoConyugue"].ToString();
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
        var BarriosColonias = new List<BarriosColonias_ViewModel>();
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
                            BarriosColonias.Add(new BarriosColonias_ViewModel()
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
            BarriosColonias = null;
        }
        return BarriosColonias;
    }

    [WebMethod]
    public static List<TipoDocumento_ViewModel> CargarDocumentosRequeridos()
    {
        return DocumentosRequeridos;
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
    public static List<CotizadorProductos_ViewModel> CargarPrestamosOfertados(decimal valorProducto, decimal valorPrima)
    {
        var PrestamosOfertados = new List<CotizadorProductos_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_CredCotizador_ConPrima", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                    sqlComando.Parameters.AddWithValue("@pnValorProducto", valorProducto);
                    sqlComando.Parameters.AddWithValue("@pnPrima", valorPrima);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        int IdContador = 1;

                        while (sqlResultado.Read())
                        {
                            PrestamosOfertados.Add(new CotizadorProductos_ViewModel()
                            {
                                IdCotizacion = IdContador,
                                Producto = sqlResultado["fcProducto"].ToString(),
                                MontoOfertado = decimal.Parse(sqlResultado["fnMontoOfertado"].ToString()),
                                Plazo = int.Parse(sqlResultado["fiIDPlazo"].ToString()),
                                Cuota = decimal.Parse(sqlResultado["fnCuotaQuincenal"].ToString()),
                                TipoPlazo = sqlResultado["fcTipodeCuota"].ToString()
                            });
                            IdContador++;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return PrestamosOfertados;
    }

    [WebMethod]
    public static ResponseEntitie IngresarSolicitud(SolicitudesMasterViewModel SolicitudesMaster,
    SolicitudesBitacoraViewModel bitacora,
    ClientesMasterViewModel ClienteMaster,
    ClientesInformacionLaboralViewModel ClientesInformacionLaboral,
    ClientesInformacionDomiciliarViewModel ClientesInformacionDomiciliar,
    ClientesInformacionConyugalViewModel ClientesInformacionConyugal,
    List<ClientesReferenciasViewModel> ClientesReferencias, bool clienteNuevo)
    {
        SqlDataReader reader = null;
        SqlCommand sqlComando;
        string MensajeError;
        ResponseEntitie resultadoProceso = new ResponseEntitie();

        string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";

        using (SqlConnection sqlConexion = new SqlConnection(sqlConnectionString))
        {
            sqlConexion.Open();
            using (SqlTransaction tran = sqlConexion.BeginTransaction())
            {
                try
                {
                    string lcURL = HttpContext.Current.Request.Url.ToString();
                    Uri lURLDesencriptado = DesencriptarURL(lcURL);
                    int pcIDUsuario = Convert.ToInt32(HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr"));
                    string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    string nombreUsuario = "";
                    DateTime fechaActual = DateTime.Now;

                    int contadorErrores = 0;
                    int clienteMaster = 0;
                    if (clienteNuevo == true)
                    {
                        #region VERIFICAR DUPLICIDAD DE LA IDENTIDAD
                        int DuplicidadIdentidad = 0;
                        int DuplicidadRTN = 0;

                        using (sqlComando = new SqlCommand("dbo.sp_CredCliente_ValidarDuplicidadIdentidades", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", ClienteMaster.fcIdentidadCliente);
                            sqlComando.Parameters.AddWithValue("@fcRTN", ClienteMaster.RTNCliente);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    DuplicidadIdentidad = (int)reader["fiValidacionIdentidad"];
                                    DuplicidadRTN = (int)reader["fiValidacionRTN"];
                                }
                            }
                        }
                        if (DuplicidadIdentidad > 0 || DuplicidadRTN > 0)
                        {
                            string MensajeDuplicidad = string.Empty;

                            if (DuplicidadIdentidad > 0 && DuplicidadRTN > 0)
                                MensajeDuplicidad = "Error de duplicidad, el número de identidad y RTN ingresados ya existen";
                            else if (DuplicidadIdentidad > 0)
                                MensajeDuplicidad = "Error de duplicidad, el número de identidad ingresado ya existe";
                            else if (DuplicidadRTN > 0)
                                MensajeDuplicidad = "Error de duplicidad, el número de RTN ingresado ya existe";

                            resultadoProceso.response = false;
                            resultadoProceso.message = MensajeDuplicidad;
                            return resultadoProceso;
                        }
                        #endregion

                        #region REGISTRAR CLIENTE MASTER
                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Insert", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiTipoCliente", 1);
                            sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", ClienteMaster.fcIdentidadCliente);
                            sqlComando.Parameters.AddWithValue("@fcRTN", ClienteMaster.RTNCliente);
                            sqlComando.Parameters.AddWithValue("@fcPrimerNombreCliente", ClienteMaster.fcPrimerNombreCliente);
                            sqlComando.Parameters.AddWithValue("@fcSegundoNombreCliente", ClienteMaster.fcSegundoNombreCliente);
                            sqlComando.Parameters.AddWithValue("@fcPrimerApellidoCliente", ClienteMaster.fcPrimerApellidoCliente);
                            sqlComando.Parameters.AddWithValue("@fcSegundoApellidoCliente", ClienteMaster.fcSegundoApellidoCliente);
                            sqlComando.Parameters.AddWithValue("@fcTelefonoCliente", ClienteMaster.fcTelefonoCliente);
                            sqlComando.Parameters.AddWithValue("@fiNacionalidadCliente", ClienteMaster.fiNacionalidadCliente);
                            sqlComando.Parameters.AddWithValue("@fdFechaNacimientoCliente", ClienteMaster.fdFechaNacimientoCliente);
                            sqlComando.Parameters.AddWithValue("@fcCorreoElectronicoCliente", ClienteMaster.fcCorreoElectronicoCliente);
                            sqlComando.Parameters.AddWithValue("@fcProfesionOficioCliente", ClienteMaster.fcProfesionOficioCliente);
                            sqlComando.Parameters.AddWithValue("@fcSexoCliente", ClienteMaster.fcSexoCliente);
                            sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", ClienteMaster.fiIDEstadoCivil);
                            sqlComando.Parameters.AddWithValue("@fiIDVivienda", ClienteMaster.fiIDVivienda);
                            sqlComando.Parameters.AddWithValue("@fiTiempoResidir", ClienteMaster.fiTiempoResidir);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    MensajeError = (string)reader["MensajeError"];

                                    if (MensajeError.StartsWith("-1"))
                                        contadorErrores++;
                                    else
                                        clienteMaster = Convert.ToInt32(MensajeError);
                                }
                            }
                        }
                        if (contadorErrores > 0 || clienteMaster == 0)
                        {
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al guardar informacion personal del cliente";
                            return resultadoProceso;
                        }
                        SolicitudesMaster.fiIDCliente = clienteMaster;
                        #endregion
                    }
                    if (clienteNuevo == false)
                    {
                        #region SI EL CLIENTE NO ES NUEVO, VALIDAR QUE NO TENGA SOLICITUDES ACTIVAS
                        int ClienteSolicitudesActivas = 0;
                        using (sqlComando = new SqlCommand("dbo.sp_CredSolicitud_ValidarClienteSolicitudesActivas", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDCliente", SolicitudesMaster.fiIDCliente);
                            sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", "");
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                    ClienteSolicitudesActivas = (int)reader["fiClienteSolicitudesActivas"];
                            }
                        }
                        if (ClienteSolicitudesActivas > 0)
                        {
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Ya hay una solicitud activa de este cliente, esperar resolución";
                            return resultadoProceso;
                        }
                        #endregion
                    }

                    #region SOLICITUD

                    #region REGISTRAR SOLICITUD MAESTRO
                    int IDSolicitudMaestro = 0;
                    int fcTipoSolicitud = SolicitudesMaster.fcTipoSolicitud == "NUEVO" ? 1 : SolicitudesMaster.fcTipoSolicitud == "REFINANCIAMIENTO" ? 2 : SolicitudesMaster.fcTipoSolicitud == "RECOMPRA" ? 3 : 0;
                    using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Maestro_Insert", sqlConexion, tran))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDPais", 1);
                        sqlComando.Parameters.AddWithValue("@fiIDCanal", 1);
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", SolicitudesMaster.fiIDCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDTipoProducto", SolicitudesMaster.fiIDTipoPrestamo);
                        sqlComando.Parameters.AddWithValue("@fiTipoSolicitud", fcTipoSolicitud);
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@fnValorSeleccionado", SolicitudesMaster.fdValorPmoSugeridoSeleccionado);
                        sqlComando.Parameters.AddWithValue("@fiMoneda", 1);
                        sqlComando.Parameters.AddWithValue("@fiPlazoSeleccionado", SolicitudesMaster.fiPlazoPmoSeleccionado);
                        sqlComando.Parameters.AddWithValue("@fnValorPrima", SolicitudesMaster.fnPrima);
                        sqlComando.Parameters.AddWithValue("@fnValorGarantia", SolicitudesMaster.fnValorGarantia);
                        sqlComando.Parameters.AddWithValue("@fiIDOrigen", SolicitudesMaster.fiIDOrigen);
                        sqlComando.Parameters.AddWithValue("@fdFechaIngresoLaborarCliente", ClientesInformacionLaboral.fcFechaIngreso);
                        sqlComando.Parameters.AddWithValue("@fcCentrodeCosteAsignado", "0100");
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioAsignado", 5);
                        sqlComando.Parameters.AddWithValue("@fdEnIngresoInicio", bitacora.fdEnIngresoInicio);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                        sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                        using (reader = sqlComando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MensajeError = (string)reader["MensajeError"];

                                if (MensajeError.StartsWith("-1"))
                                    contadorErrores++;
                                else
                                    IDSolicitudMaestro = Convert.ToInt32(MensajeError);
                            }
                        }
                    }
                    if (contadorErrores > 0)
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar la solicitud, contacte al administrador";
                        return resultadoProceso;
                    }
                    #endregion

                    #region REGISTRAR DOCUMENTACIÓN DE LA SOLICITUD

                    int DocumentacionCliente = 1;
                    string NombreCarpetaDocumentos = "Solicitud" + IDSolicitudMaestro;
                    string NuevoNombreDocumento = "";

                    // lista de documentos adjuntados por el usuario
                    var listaDocumentos = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                    // Lista de documentos que se va ingresar en la base de datos y se va mover al nuevo directorio
                    List<SolicitudesDocumentosViewModel> SolicitudesDocumentos = new List<SolicitudesDocumentosViewModel>();

                    if (listaDocumentos != null)
                    {
                        // lista de bloques y la cantidad de documentos que contiene cada uno
                        var Bloques = listaDocumentos.GroupBy(TipoDocumento => TipoDocumento.fiTipoDocumento).Select(x => new { x.Key, Count = x.Count() });

                        // lista donde se guardara temporalmente los documentos adjuntados por el usuario dependiendo del tipo de documento en el iterador
                        List<SolicitudesDocumentosViewModel> DocumentosBloque = new List<SolicitudesDocumentosViewModel>();

                        foreach (var Bloque in Bloques)
                        {
                            int TipoDocumento = (int)Bloque.Key;
                            int CantidadDocumentos = Bloque.Count;

                            DocumentosBloque = listaDocumentos.Where(x => x.fiTipoDocumento == TipoDocumento).ToList();// documentos de este bloque
                            String[] NombresGenerador = Funciones.MultiNombres.GenerarNombreCredDocumento(DocumentacionCliente, IDSolicitudMaestro, TipoDocumento, CantidadDocumentos);

                            int ContadorNombre = 0;
                            foreach (SolicitudesDocumentosViewModel file in DocumentosBloque)
                            {
                                if (File.Exists(file.fcRutaArchivo + @"\" + file.NombreAntiguo))//si el archivo existe, que se agregue a la lista
                                {
                                    NuevoNombreDocumento = NombresGenerador[ContadorNombre];
                                    SolicitudesDocumentos.Add(new SolicitudesDocumentosViewModel()
                                    {
                                        fcNombreArchivo = NuevoNombreDocumento,
                                        NombreAntiguo = file.NombreAntiguo,
                                        fcTipoArchivo = file.fcTipoArchivo,
                                        fcRutaArchivo = file.fcRutaArchivo.Replace("Temp", "") + NombreCarpetaDocumentos,
                                        URLArchivo = "/Documentos/Solicitudes/" + NombreCarpetaDocumentos + "/" + NuevoNombreDocumento + ".png",
                                        fiTipoDocumento = file.fiTipoDocumento
                                    });
                                    ContadorNombre++;
                                }
                            }
                        }
                    }
                    else
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al registrar la documentación, compruebe los documentos adjuntados o vuelva a cargarlos";
                        return resultadoProceso;
                    }
                    if (SolicitudesDocumentos.Count <= 0)
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar documentación, compruebe que haya cargado los documentos correctamente o vuelva a cargarlos";
                        return resultadoProceso;
                    }
                    foreach (SolicitudesDocumentosViewModel documento in SolicitudesDocumentos)
                    {
                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Documentos_Insert", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitudMaestro);
                            sqlComando.Parameters.AddWithValue("@fcNombreArchivo", documento.fcNombreArchivo);
                            sqlComando.Parameters.AddWithValue("@fcTipoArchivo", ".png");
                            sqlComando.Parameters.AddWithValue("@fcRutaArchivo", documento.fcRutaArchivo);
                            sqlComando.Parameters.AddWithValue("@fcURL", documento.URLArchivo);
                            sqlComando.Parameters.AddWithValue("@fiTipoDocumento", documento.fiTipoDocumento);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    MensajeError = (string)reader["MensajeError"];
                                    if (MensajeError.StartsWith("-1"))
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
                    if (!FileUploader.GuardarSolicitudDocumentos(IDSolicitudMaestro, SolicitudesDocumentos))
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar la documentación de la solicitud";
                        return resultadoProceso;
                    }
                    #endregion

                    #endregion

                    //cheque
                    #region REGISTRAR CLIENTE INFORMACION LABORAL
                    using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionLaboral_Insert", sqlConexion, tran))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", SolicitudesMaster.fiIDCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitudMaestro);
                        sqlComando.Parameters.AddWithValue("@fcNombreTrabajo", ClientesInformacionLaboral.fcNombreTrabajo);
                        sqlComando.Parameters.AddWithValue("@fnIngresosMensuales", ClientesInformacionLaboral.fiIngresosMensuales);
                        sqlComando.Parameters.AddWithValue("@fcPuestoAsignado", ClientesInformacionLaboral.fcPuestoAsignado);
                        sqlComando.Parameters.AddWithValue("@fdFechaIngreso", ClientesInformacionLaboral.fcFechaIngreso);
                        sqlComando.Parameters.AddWithValue("@fcTelefonoEmpresa", ClientesInformacionLaboral.fdTelefonoEmpresa);
                        sqlComando.Parameters.AddWithValue("@fcExtensionRecursosHumanos", ClientesInformacionLaboral.fcExtensionRecursosHumanos);
                        sqlComando.Parameters.AddWithValue("@fcExtensionCliente", ClientesInformacionLaboral.fcExtensionCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDDepartamento", ClientesInformacionLaboral.fiIDDepto);
                        sqlComando.Parameters.AddWithValue("@fiIDMunicipio", ClientesInformacionLaboral.fiIDMunicipio);
                        sqlComando.Parameters.AddWithValue("@fiIDCiudad", ClientesInformacionLaboral.fiIDCiudad);
                        sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", ClientesInformacionLaboral.fiIDBarrioColonia);
                        sqlComando.Parameters.AddWithValue("@fcDireccionDetalladaEmpresa", ClientesInformacionLaboral.fcDireccionDetalladaEmpresa);
                        sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", ClientesInformacionLaboral.fcReferenciasDireccionDetallada);
                        sqlComando.Parameters.AddWithValue("@fcFuenteOtrosIngresos", ClientesInformacionLaboral.fcFuenteOtrosIngresos);
                        sqlComando.Parameters.AddWithValue("@fnValorOtrosIngresosMensuales", ClientesInformacionLaboral.fiValorOtrosIngresosMensuales);
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                        sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                        using (reader = sqlComando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MensajeError = (string)reader["MensajeError"];
                                if (MensajeError.StartsWith("-1"))
                                    contadorErrores++;
                            }
                        }
                    }
                    if (contadorErrores > 0)
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al registrar información laboral del cliente";
                        return resultadoProceso;
                    }
                    #endregion

                    //cheque
                    #region REGISTRAR INFORMACION DOMICILIO
                    using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionDomiciliar_Insert", sqlConexion, tran))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", SolicitudesMaster.fiIDCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitudMaestro);
                        sqlComando.Parameters.AddWithValue("@fcTelefonoCasa", ClientesInformacionDomiciliar.fcTelefonoCasa);
                        sqlComando.Parameters.AddWithValue("@fiIDDepartamento", ClientesInformacionDomiciliar.fiIDDepto);
                        sqlComando.Parameters.AddWithValue("@fiIDMunicipio", ClientesInformacionDomiciliar.fiIDMunicipio);
                        sqlComando.Parameters.AddWithValue("@fiIDCiudad", ClientesInformacionDomiciliar.fiIDCiudad);
                        sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", ClientesInformacionDomiciliar.fiIDBarrioColonia);
                        sqlComando.Parameters.AddWithValue("@fcDireccionDetallada", ClientesInformacionDomiciliar.fcDireccionDetallada);
                        sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", ClientesInformacionDomiciliar.fcReferenciasDireccionDetallada);
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                        sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                        using (reader = sqlComando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                MensajeError = (string)reader["MensajeError"];
                                if (MensajeError.StartsWith("-1"))
                                    contadorErrores++;
                            }
                        }
                    }
                    if (contadorErrores > 0)
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar informacion domiciliar del cliente";
                        return resultadoProceso;
                    }
                    #endregion

                    //cheque
                    #region REGISTRAR CLIENTE INFORMACION CONYUGAL
                    if (ClientesInformacionConyugal.fcIndentidadConyugue != "" && ClientesInformacionConyugal.fcIndentidadConyugue != null)
                    {
                        using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionConyugal_Insert", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDCliente", SolicitudesMaster.fiIDCliente);
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitudMaestro);
                            sqlComando.Parameters.AddWithValue("@fcNombreCompletoConyugue", ClientesInformacionConyugal.fcNombreCompletoConyugue);
                            sqlComando.Parameters.AddWithValue("@fcIndentidadConyugue", ClientesInformacionConyugal.fcIndentidadConyugue);
                            sqlComando.Parameters.AddWithValue("@fdFechaNacimientoConyugue", ClientesInformacionConyugal.fdFechaNacimientoConyugue);
                            sqlComando.Parameters.AddWithValue("@fcTelefonoConyugue", ClientesInformacionConyugal.fcTelefonoConyugue);
                            sqlComando.Parameters.AddWithValue("@fcLugarTrabajoConyugue", ClientesInformacionConyugal.fcLugarTrabajoConyugue);
                            sqlComando.Parameters.AddWithValue("@fnIngresosMensualesConyugue", ClientesInformacionConyugal.fcIngresosMensualesConyugue);
                            sqlComando.Parameters.AddWithValue("@fcTelefonoTrabajoConyugue", ClientesInformacionConyugal.fcTelefonoTrabajoConyugue);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                            using (reader = sqlComando.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    MensajeError = (string)reader["MensajeError"];
                                    if (MensajeError.StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al guardar informacion conyugal del cliente";
                            return resultadoProceso;
                        }
                    }
                    #endregion

                    //cheque
                    #region REFERENCIAS PERSONALES DEL CLIENTE
                    if (ClientesReferencias != null)
                    {
                        if (ClientesReferencias.Count != 0)
                        {
                            foreach (ClientesReferenciasViewModel clienteReferencia in ClientesReferencias)
                            {
                                using (sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Referencias_Insert", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDCliente", SolicitudesMaster.fiIDCliente);
                                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IDSolicitudMaestro);
                                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoReferencia", clienteReferencia.fcNombreCompletoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoReferencia", clienteReferencia.fcLugarTrabajoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiTiempoConocerReferencia", clienteReferencia.fiTiempoConocerReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoReferencia", clienteReferencia.fcTelefonoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDParentescoReferencia", clienteReferencia.fiIDParentescoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                                    sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                                    using (reader = sqlComando.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            MensajeError = (string)reader["MensajeError"];
                                            if (MensajeError.StartsWith("-1"))
                                                contadorErrores++;
                                        }
                                    }
                                }
                                if (contadorErrores > 0)
                                {
                                    resultadoProceso.response = false;
                                    resultadoProceso.message = "Error al guardar referencias personales del cliente";
                                    return resultadoProceso;
                                }
                            }
                        }
                    }
                    #endregion
                    
                    tran.Commit();
                    resultadoProceso.idInsertado = 0;
                    resultadoProceso.response = true;
                    resultadoProceso.message = "¡La solicitud ha sido ingresada exitosamente!";
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    ex.Message.ToString();
                    resultadoProceso.response = false;
                    resultadoProceso.message = "Error al guardar solicitud, contacte al administrador";
                    ExceptionLogging.SendExcepToDB(ex);
                }
            }
        }
        return resultadoProceso;
    }
    
    public static Uri DesencriptarURL(string Url)
    {
        Uri lURLDesencriptado = null;
        try
        {
            var lcParametros = "";
            var pcEncriptado = "";
            var liParamStart = Url.IndexOf("?");

            if (liParamStart > 0)
                lcParametros = Url.Substring(liParamStart, Url.Length - liParamStart);
            else
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                pcEncriptado = Url.Substring((liParamStart + 1), Url.Length - (liParamStart + 1));
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


/* View Models */
public class Origenes_ViewModel
{
    public int IdOrigen { get; set; }
    public string Origen { get; set; }
}

public class Precalificado_ViewModel
{
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

    public SolicitudesCredito_Registrar_Constantes()
    {
        HoraAlCargar = DateTime.Now;
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
}

public class TipoDocumento_ViewModel
{
    public int IdTipoDocumento { get; set; }
    public string DescripcionTipoDocumento { get; set; }
    public int CantidadMaximaDoucmentos { get; set; }
    public int TipoVisibilidad { get; set; }
}