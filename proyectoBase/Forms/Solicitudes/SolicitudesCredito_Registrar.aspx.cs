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
using UI.Web.Models.ViewModel;

public partial class SolicitudesCredito_Registrar : System.Web.UI.Page
{
    private String pcEncriptado = "";
    private string pcIDUsuario = "";
    private string pcID = "";
    private string pcIDApp = "";
    public DateTime FechaCarga;

    protected void Page_Load(object sender, EventArgs e)
    {
        string type = Request.QueryString["type"];
        if (type != null || Request.HttpMethod == "POST")
        {
            int TipoDocumento = Convert.ToInt32(Request.QueryString["doc"]);
            Session["tipoDoc"] = TipoDocumento;
            string uploadDir = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";

            FileUploader fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
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
            HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;

        if (!IsPostBack)
        {
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            string lcURL = Request.Url.ToString();
            int liParamStart = lcURL.IndexOf("?");

            string lcParametros;
            if (liParamStart > 0)
                lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
            else
                lcParametros = String.Empty;

            if (lcParametros != String.Empty)
            {
                pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                string lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                GuardarDetallesPrecalificado(pcID);
            }
            FechaCarga = DateTime.Now;
        }
    }

    public void GuardarDetallesPrecalificado(string identidad)
    {
        PrecalificadoViewModel objPrecalificado = null;
        List<cotizadorProductosViewModel> listaCotizadorProductos = new List<cotizadorProductosViewModel>();
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            using (SqlConnection conn = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    cmd.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    cmd.Parameters.AddWithValue("@pcIdentidad", identidad);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            identidadCliente.Text = (string)reader["fcIdentidad"];
                            identidadCliente.Enabled = false;
                            primerNombreCliente.Text = (string)reader["fcPrimerNombre"];
                            primerNombreCliente.Enabled = false;
                            SegundoNombreCliente.Text = (string)reader["fcSegundoNombre"];
                            SegundoNombreCliente.Enabled = false;
                            primerApellidoCliente.Text = (string)reader["fcPrimerApellido"];
                            primerApellidoCliente.Enabled = false;
                            segundoApellidoCliente.Text = (string)reader["fcSegundoApellido"];
                            segundoApellidoCliente.Enabled = false;
                            ingresosPrecalificado.Text = Decimal.Parse(reader["fnIngresos"].ToString()).ToString();
                            ingresosPrecalificado.Enabled = false;
                            numeroTelefono.Text = (string)reader["fcTelefono"];
                            numeroTelefono.Enabled = false;
                            //rtnCliente.Text = (string)reader["fcRTN"];
                            //if((string)reader["fcRTN"] != "")
                            //    rtnCliente.Enabled = false;

                            int tipoDeSolicitud = (int)reader["fiTipoSolicitudCliente"];
                            tipoSolicitud.Text = tipoDeSolicitud == 1 ? "NUEVO" : tipoDeSolicitud == 2 ? "REFINANCIAMIENTO" : tipoDeSolicitud == 3 ? "RECOMPRA" : "";

                            objPrecalificado = new PrecalificadoViewModel()
                            {
                                identidad = (string)reader["fcIdentidad"],
                                primerNombre = (string)reader["fcPrimerNombre"],
                                segundoNombre = (string)reader["fcSegundoNombre"],
                                primerApellido = (string)reader["fcPrimerApellido"],
                                segundoApellido = (string)reader["fcSegundoApellido"],
                                telefono = (string)reader["fcTelefono"],
                                obligaciones = Decimal.Parse(reader["fnTotalObligaciones"].ToString()),
                                ingresos = Decimal.Parse(reader["fnIngresos"].ToString()),
                                disponible = Decimal.Parse(reader["fnCapacidadDisponible"].ToString()),
                                fechaNacimiento = DateTime.Parse(reader["fdFechadeNacimiento"].ToString()),
                                tipoSolicitud = (int)reader["fiTipoSolicitudCliente"],
                                tipoProducto = (int)reader["fiIDProducto"],
                                Producto = (string)reader["fcProducto"]
                            };
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("sp_CotizadorProductos", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    cmd.Parameters.AddWithValue("@piIDProducto", objPrecalificado.tipoProducto);
                    cmd.Parameters.AddWithValue("@pcIdentidad", objPrecalificado.identidad);
                    cmd.Parameters.AddWithValue("@piConObligaciones", objPrecalificado.obligaciones == 0 ? "0" : "1");
                    cmd.Parameters.AddWithValue("@pnIngresosBrutos", objPrecalificado.ingresos);
                    cmd.Parameters.AddWithValue("@pnIngresosDisponible", objPrecalificado.disponible);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        decimal valorMasAlto = 0;
                        int IDContador = 1;
                        cotizadorProductosViewModel objCotizador = new cotizadorProductosViewModel();

                        while (reader.Read())
                        {
                            if (valorMasAlto < decimal.Parse(reader["fnMontoOfertado"].ToString()))
                            {
                                objCotizador = new cotizadorProductosViewModel()
                                {
                                    IDCotizacion = IDContador,
                                    IDProducto = (int)reader["fiIDProducto"],
                                    ProductoDescripcion = reader["fcProducto"].ToString(),
                                    fnMontoOfertado = decimal.Parse(reader["fnMontoOfertado"].ToString()),
                                    fiPlazo = int.Parse(reader["fiPlazo"].ToString()),
                                    fnCuotaQuincenal = decimal.Parse(reader["fnCuotaQuincenal"].ToString())
                                };
                            }
                            valorMasAlto = decimal.Parse(reader["fnMontoOfertado"].ToString());
                            IDContador += 1;

                            objPrecalificado.cotizadorProductos = new List<cotizadorProductosViewModel>();
                            objPrecalificado.cotizadorProductos.Add(objCotizador);
                            HttpContext.Current.Session["precalificadoDelCliente"] = objPrecalificado;
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

    [WebMethod]
    public static PrecalificadoViewModel GetDetallesPrecalificado()
    {
        PrecalificadoViewModel objPrecalificado = null;

        if (HttpContext.Current.Session["precalificadoDelCliente"] != null)
            objPrecalificado = (PrecalificadoViewModel)HttpContext.Current.Session["precalificadoDelCliente"];

        return objPrecalificado;
    }

    [WebMethod]
    public static ClientesViewModel ObtenerInformacionCliente(string dataCrypt)
    {
        ClientesViewModel objCliente = new ClientesViewModel();
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string IDSOL = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDSOL");
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            string identidad = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_Maestro_ObtenerInformacion", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", identidad);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        if (!reader.HasRows)
                            return objCliente;
                        while (reader.Read())
                        {
                            objCliente.clientesMaster = new ClientesMasterViewModel()
                            {
                                fiIDCliente = (int)reader["fiIDCliente"],
                                fcIdentidadCliente = (string)reader["fcIdentidadCliente"],
                                RTNCliente = (string)reader["fcRTN"],
                                fcPrimerNombreCliente = (string)reader["fcPrimerNombreCliente"],
                                fcSegundoNombreCliente = (string)reader["fcSegundoNombreCliente"],
                                fcPrimerApellidoCliente = (string)reader["fcPrimerApellidoCliente"],
                                fcSegundoApellidoCliente = (string)reader["fcSegundoApellidoCliente"],
                                fcTelefonoCliente = (string)reader["fcTelefonoPrimarioCliente"],
                                fiNacionalidadCliente = (int)reader["fiNacionalidadCliente"],
                                fcDescripcionNacionalidad = (string)reader["fcDescripcionNacionalidad"],
                                fbNacionalidadActivo = (bool)reader["fbNacionalidadActivo"],
                                fdFechaNacimientoCliente = (DateTime)reader["fdFechaNacimientoCliente"],
                                fcCorreoElectronicoCliente = (string)reader["fcCorreoElectronicoCliente"],
                                fcProfesionOficioCliente = (string)reader["fcProfesionOficioCliente"],
                                fcSexoCliente = (string)reader["fcSexoCliente"],
                                fiIDEstadoCivil = (int)reader["fiIDEstadoCivil"],
                                fcDescripcionEstadoCivil = (string)reader["fcDescripcionEstadoCivil"],
                                fiIDVivienda = (int)reader["fiIDVivienda"],
                                fcDescripcionVivienda = (string)reader["fcDescripcionVivienda"],
                                fiTiempoResidir = (short)reader["fiTiempoResidir"],
                                fbClienteActivo = (bool)reader["fbClienteActivo"],
                                fcRazonInactivo = (string)reader["fcRazonInactivo"],
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                            };
                        }
                    }
                }

                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCliente_Laboral_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objCliente.ClientesInformacionLaboral = new ClientesInformacionLaboralViewModel()
                            {
                                fiIDCliente = (int)reader["fiIDCliente"],
                                fiIDInformacionLaboral = (int)reader["fiIDInformacionLaboral"],
                                fcNombreTrabajo = (string)reader["fcNombreTrabajo"],
                                fiIngresosMensuales = (decimal)reader["fnIngresosMensuales"],
                                fcPuestoAsignado = (string)reader["fcPuestoAsignado"],
                                fcFechaIngreso = (DateTime)reader["fdFechaIngreso"],
                                fdTelefonoEmpresa = (string)reader["fcTelefonoEmpresa"],
                                fcExtensionRecursosHumanos = (string)reader["fcExtensionRecursosHumanos"],
                                fcExtensionCliente = (string)reader["fcExtensionCliente"],
                                fcDireccionDetalladaEmpresa = (string)reader["fcDireccionDetalladaEmpresa"],
                                fcReferenciasDireccionDetallada = (string)reader["fcReferenciasDireccionDetalladaEmpresa"],
                                fcFuenteOtrosIngresos = (string)reader["fcFuenteOtrosIngresos"],
                                fiValorOtrosIngresosMensuales = (decimal)reader["fnValorOtrosIngresosMensuales"],
                                fiIDBarrioColonia = (int)reader["fiIDBarrioColonia"],
                                fcNombreBarrioColonia = (string)reader["fcBarrio"],
                                fiIDCiudad = (int)reader["fiIDCiudad"],
                                fcNombreCiudad = (string)reader["fcPoblado"],
                                fiIDMunicipio = (int)reader["fiIDMunicipio"],
                                fcNombreMunicipio = (string)reader["fcMunicipio"],
                                fiIDDepto = (int)reader["fiIDDepartamento"],
                                fcNombreDepto = (string)reader["fcDepartamento"],
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                            };
                        }
                    }
                }

                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionConyugal_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSOlicitud", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objCliente.ClientesInformacionConyugal = new ClientesInformacionConyugalViewModel()
                            {
                                fiIDInformacionConyugal = (int)reader["fiIDInformacionConyugal"],
                                fiIDCliente = (int)reader["fiIDCliente"],
                                fcNombreCompletoConyugue = (string)reader["fcNombreCompletoConyugue"],
                                fcIndentidadConyugue = (string)reader["fcIndentidadConyugue"],
                                fdFechaNacimientoConyugue = (DateTime)reader["fdFechaNacimientoConyugue"],
                                fcTelefonoConyugue = (string)reader["fcTelefonoConyugue"],
                                fcLugarTrabajoConyugue = (string)reader["fcLugarTrabajoConyugue"],
                                fcIngresosMensualesConyugue = (decimal)reader["fnIngresosMensualesConyugue"],
                                fcTelefonoTrabajoConyugue = (string)reader["fcTelefonoTrabajoConyugue"],
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                            };
                        }
                    }
                }

                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionDomiciliar_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDCliente", objCliente.clientesMaster.fiIDCliente);
                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objCliente.ClientesInformacionDomiciliar = new ClientesInformacionDomiciliarViewModel()
                            {
                                fiIDInformacionDomicilio = (int)reader["fiIDInformacionDomicilio"],
                                fiIDCliente = (int)reader["fiIDCliente"],
                                fcTelefonoCasa = (string)reader["fcTelefonoCasa"],
                                fcDireccionDetallada = (string)reader["fcDireccionDetalladaDomicilio"],
                                fcReferenciasDireccionDetallada = (string)reader["fcReferenciasDireccionDetalladaDomicilio"],
                                fiIDBarrioColonia = (short)reader["fiCodBarrio"],
                                fcNombreBarrioColonia = (string)reader["fcBarrio"],
                                fiIDCiudad = (short)reader["fiCodPoblado"],
                                fcNombreCiudad = (string)reader["fcPoblado"],
                                fiIDMunicipio = (short)reader["fiCodMunicipio"],
                                fcNombreMunicipio = (string)reader["fcMunicipio"],
                                fiIDDepto = (short)reader["fiCodDepartamento"],
                                fcNombreDepto = (string)reader["fcDepartamento"],
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                            };
                        }
                    }
                } // using command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return objCliente;
    }

    [WebMethod]
    public static SolicitudIngresarDDLViewModel CargarListas(string dataCrypt)
    {
        SolicitudIngresarDDLViewModel ddls = new SolicitudIngresarDDLViewModel();
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                List<DepartamentosViewModel> viewModelDepto = new List<DepartamentosViewModel>();
                using (SqlCommand sqlComando = new SqlCommand("sp_GeoDepartamento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", 0);
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            viewModelDepto.Add(new DepartamentosViewModel()
                            {
                                fiIDDepto = (short)reader["fiCodDepartamento"],
                                fcNombreDepto = (string)reader["fcDepartamento"]
                            });
                        }
                    }
                    ddls.Departamentos = viewModelDepto;
                }

                List<ViviendaViewModel> viewModelVivienda = new List<ViviendaViewModel>();
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCatalogo_Vivienda_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDVivienda", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            viewModelVivienda.Add(new ViviendaViewModel()
                            {
                                fiIDVivienda = (int)reader["fiIDVivienda"],
                                fcDescripcionVivienda = (string)reader["fcDescripcionVivienda"],
                                fbViviendaActivo = (bool)reader["fbViviendaActivo"],
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                            });
                        }
                    }
                    ddls.Vivienda = viewModelVivienda;
                }

                List<EstadosCivilesViewModel> viewModelEstadosCiviles = new List<EstadosCivilesViewModel>();
                using (SqlCommand sqlComando = new SqlCommand("sp_CredCatalogo_EstadosCiviles_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            viewModelEstadosCiviles.Add(new EstadosCivilesViewModel()
                            {
                                fiIDEstadoCivil = (int)reader["fiIDEstadoCivil"],
                                fcDescripcionEstadoCivil = (string)reader["fcDescripcionEstadoCivil"],
                                fbEstadoCivilActivo = (bool)reader["fbEstadoCivilActivo"],
                                fbRequiereInformacionConyugal = (bool)reader["fbRequiereInformacionConyugal"],
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                            });
                        }
                    }
                    ddls.EstadosCiviles = viewModelEstadosCiviles;
                }

                List<NacionalidadesViewModel> NacionalidadesViewModel = new List<NacionalidadesViewModel>();
                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCatalogo_Nacionalidades_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDNacionalidad", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            NacionalidadesViewModel.Add(new NacionalidadesViewModel()
                            {
                                fiIDNacionalidad = (int)reader["fiIDNacionalidad"],
                                fcDescripcionNacionalidad = (string)reader["fcDescripcionNacionalidad"],
                                fbNacionalidadActivo = (bool)reader["fbNacionalidadActivo"],
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                            });
                        }
                    }
                    ddls.Nacionalidades = NacionalidadesViewModel;
                }

                List<TipoDocumentoViewModel> TipoDocumentoViewModel = new List<TipoDocumentoViewModel>();

                using (SqlCommand sqlComando = new SqlCommand("sp_CredCatalogo_TipoDocumento_RegistrarSolicitudListar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TipoDocumentoViewModel.Add(new TipoDocumentoViewModel()
                            {
                                IDTipoDocumento = (short)reader["fiIDTipoDocumento"],
                                DescripcionTipoDocumento = (string)reader["fcDescripcionTipoDocumento"],
                                CantidadMaximaDoucmentos = (byte)reader["fiCantidadDocumentos"],
                                TipoVisibilidad = (byte)reader["fiTipodeVisibilidad"]
                            });
                        }
                    }
                    ddls.TipoDocumento = TipoDocumentoViewModel;
                }

                List<ParentescosViewModel> ParentescosViewModel = new List<ParentescosViewModel>();
                using (SqlCommand sqlComando = new SqlCommand("sp_CREDCatalogo_Parentescos_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiIDParentesco", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ParentescosViewModel.Add(new ParentescosViewModel()
                            {
                                fiIDParentescos = (int)reader["fiIDParentesco"],
                                fcDescripcionParentesco = (string)reader["fcDescripcionParentesco"],
                                fbParentescoActivo = (bool)reader["fbParentescoActivo"],
                                fiIDUsuarioCrea = (int)reader["fiIDUsuarioCrea"],
                                fdFechaCrea = (DateTime)reader["fdFechaCrea"],
                                fiIDUsuarioModifica = (int)reader["fiIDUsuarioModifica"],
                                fdFechaUltimaModifica = (DateTime)reader["fdFechaUltimaModifica"]
                            });
                        }
                    }
                    ddls.Parentescos = ParentescosViewModel;
                }

                List<MonedasViewModel> TiposdeMoneda = new List<MonedasViewModel>();
                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredCatalogo_Monedas_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiMoneda", 0);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TiposdeMoneda.Add(new MonedasViewModel()
                            {
                                IDTipoMoneda = (short)reader["fiMoneda"],
                                TipoMoneda = (string)reader["fcNombreMoneda"]
                            });
                        }
                    }
                }
                ddls.Monedas = TiposdeMoneda;

                List<TipoClienteViewModel> TiposdeCliente = new List<TipoClienteViewModel>();
                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredCatalogo_TipoCliente_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fiTipoCliente", 0);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TiposdeCliente.Add(new TipoClienteViewModel()
                            {
                                IDTipoCliente = (short)reader["fiTipoCliente"],
                                TipoCliente = (string)reader["fcTipoCliente"]
                            });
                        }
                    }
                    ddls.TipoCliente = TiposdeCliente;
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return ddls;
    }

    [WebMethod]
    public static List<MunicipiosViewModel> CargarMunicipios(int CODDepto)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        List<MunicipiosViewModel> municipios = new List<MunicipiosViewModel>();
        try
        {
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_GeoMunicipio", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", CODDepto);
                    sqlComando.Parameters.AddWithValue("@piMunicipio", 0);
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            municipios.Add(new MunicipiosViewModel()
                            {
                                fiIDDepto = (short)reader["fiCodDepartamento"],
                                fiIDMunicipio = (short)reader["fiCodMunicipio"],
                                fcNombreMunicipio = (string)reader["fcMunicipio"],
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
        return municipios;
    }

    [WebMethod]
    public static List<CiudadesViewModel> CargarPoblados(int CODDepto, int CODMunicipio)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        List<CiudadesViewModel> ciudades = new List<CiudadesViewModel>();
        try
        {
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_GeoPoblado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", CODDepto);
                    sqlComando.Parameters.AddWithValue("@piMunicipio", CODMunicipio);
                    sqlComando.Parameters.AddWithValue("@piPoblado", 0);
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ciudades.Add(new CiudadesViewModel()
                            {
                                fiIDDepto = (short)reader["fiCodDepartamento"],
                                fiIDMunicipio = (short)reader["fiCodMunicipio"],
                                fiIDCiudad = (short)reader["fiCodPoblado"],
                                fcNombreCiudad = (string)reader["fcPoblado"],
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
        return ciudades;
    }

    [WebMethod]
    public static List<BarriosColoniasViewModel> CargarBarrios(int CODDepto, int CODMunicipio, int CODPoblado)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        List<BarriosColoniasViewModel> Barrios = new List<BarriosColoniasViewModel>();
        try
        {
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_GeoBarrios", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", CODDepto);
                    sqlComando.Parameters.AddWithValue("@piMunicipio", CODMunicipio);
                    sqlComando.Parameters.AddWithValue("@piPoblado", CODPoblado);
                    sqlComando.Parameters.AddWithValue("@piBarrio", 0);
                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Barrios.Add(new BarriosColoniasViewModel()
                            {
                                fiIDDepto = (short)reader["fiCodDepartamento"],
                                fiIDMunicipio = (short)reader["fiCodMunicipio"],
                                fiIDCiudad = (short)reader["fiCodPoblado"],
                                fiIDBarrioColonia = (short)reader["fiCodBarrio"],
                                fcNombreBarrioColonia = (string)reader["fcBarrioColonia"],
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
        return Barrios;
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
        String sqlConnectionString;
        string MensajeError;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        ResponseEntitie resultadoProceso = new ResponseEntitie();

        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
        using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString)))
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
                    string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
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
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
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
                            sqlComando.Parameters.AddWithValue("@fiTipoCliente", ClienteMaster.IDTipoCliente);
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
                            tran.Rollback();
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
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
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

                    #region REGISTRAR SOLICITUDES MASTER
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
                        sqlComando.Parameters.AddWithValue("@fiMoneda", SolicitudesMaster.IDTipoMoneda);
                        sqlComando.Parameters.AddWithValue("@fiPlazoSeleccionado", SolicitudesMaster.fiPlazoPmoSeleccionado);
                        sqlComando.Parameters.AddWithValue("@fnValorPrima", SolicitudesMaster.fnPrima);
                        sqlComando.Parameters.AddWithValue("@fnValorGarantia", SolicitudesMaster.fnValorGarantia);
                        sqlComando.Parameters.AddWithValue("@fiIDOrigen", SolicitudesMaster.fiIDOrigen);
                        sqlComando.Parameters.AddWithValue("@fdFechaIngresoLaborarCliente", ClientesInformacionLaboral.fcFechaIngreso);
                        sqlComando.Parameters.AddWithValue("@fcCentrodeCosteAsignado", "");
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioAsignado", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@fdEnIngresoInicio", bitacora.fdEnIngresoInicio);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
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
                        tran.Rollback();
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
                        tran.Rollback();
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al registrar información laboral del cliente";
                        return resultadoProceso;
                    }
                    #endregion

                    #region REGISTRAR AVAL INFORMACION DOMICILIAR
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
                        tran.Rollback();
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar informacion domiciliar del cliente";
                        return resultadoProceso;
                    }
                    #endregion

                    #region REGISTRAR CLIENTE INFORMACION CONYUGAL
                    if (ClientesInformacionConyugal.fcIndentidadConyugue != "")
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
                            tran.Rollback();
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al guardar informacion conyugal del cliente";
                            return resultadoProceso;
                        }
                    }
                    #endregion

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
                    ex.Message.ToString();
                    tran.Rollback();
                    resultadoProceso.response = false;
                    resultadoProceso.message = "Error al guardar solicitud, contacte al administrador";
                    ExceptionLogging.SendExcepToDB(ex);
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
        return resultadoProceso;
    }

    [WebMethod]
    public static List<OrigenesViewModel> CargarOrigenes(int COD, string dataCrypt)
    {
        List<OrigenesViewModel> origenes = new List<OrigenesViewModel>();
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CredCatalogo_Origenes", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDProducto", COD);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            origenes.Add(new OrigenesViewModel()
                            {
                                fiIDOrigen = Convert.ToInt16(reader["fiIDOrigen"]),
                                fcOrigen = (string)reader["fcOrigen"]
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
        return origenes;
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

    [WebMethod]
    public static CalculoPrestamoViewModel CalculoPrestamo(int TipoProducto, decimal MontoFinanciar, decimal PlazoFinanciar, decimal ValorPrima, string dataCrypt)
    {
        CalculoPrestamoViewModel objCalculo = null;
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        try
        {
            Uri lURLDesencriptado = DesencriptarURL(dataCrypt);
            string pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            string pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            string pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");

            using (SqlConnection sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (SqlCommand sqlComando = new SqlCommand("sp_CredSolicitud_CalculoPrestamo", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDProducto", TipoProducto);
                    sqlComando.Parameters.AddWithValue("@pnMontoPrestamo", MontoFinanciar);
                    sqlComando.Parameters.AddWithValue("@liPlazo", PlazoFinanciar);
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", ValorPrima);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (SqlDataReader reader = sqlComando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            objCalculo = new CalculoPrestamoViewModel()
                            {
                                SegurodeDeuda = (decimal)reader["fnSegurodeDeuda"],
                                SegurodeVehiculo = (decimal)reader["fnSegurodeVehiculo"],
                                GastosdeCierre = (decimal)reader["fnGastosdeCierre"],
                                ValoraFinanciar = (decimal)reader["fnValoraFinanciar"],
                                CuotaQuincenal = (decimal)reader["fnCuotaQuincenal"],
                                CuotaMensual = (decimal)reader["fnCuotaMensual"],
                                CuotaServicioGPS = (decimal)reader["fnCuotaServicioGPS"],
                                CuotaSegurodeVehiculo = (decimal)reader["fnCuotaSegurodeVehiculo"],
                                CuotaMensualNeta = (decimal)reader["fnCuotaMensualNeta"],
                                TotalSeguroVehiculo = (decimal)reader["fnTotalSeguroVehiculo"]
                            };
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return objCalculo;
    }
}

public class OrigenesViewModel
{
    public int fiIDOrigen { get; set; }
    public string fcOrigen { get; set; }
}