﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using adminfiles;

public partial class SolicitudesCredito_Registrar : System.Web.UI.Page
{
    private string pcID = "";
    private string pcIDApp = "";
    private string pcIDSesion = "";
    private string pcIDUsuario = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    public Precalificado_ViewModel Precalificado;
    public List<TipoDocumento_ViewModel> DocumentosRequeridos;
    public SolicitudesCredito_Registrar_Constantes Constantes;
    public string jsonConstantes;
    public string jsonPrecalicado;

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        /* Captura de parámetros encriptados */
        if (!IsPostBack && type == null)
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
                lcParametros = string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                var lcParametroDesencriptado = DSC.Desencriptar(pcEncriptado);
                var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

                pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
                pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

                CargarPrecalificado();
                CargarInformacionProducto();
                ValidarClienteSolicitudesActivas();
                CargarListas();
                ObtenerInformacionCliente();
                ValidarPreSolicitud();

                HttpContext.Current.Session["ListaSolicitudesDocumentos"] = null;
                Session.Timeout = 10080;

                if (Constantes.RequiereOrigen == 1)
                {
                    CargarOrigenes();
                }

                if (Constantes.RequierePrima == 1)
                {
                    txtValorPrima.Enabled = true;
                }

                if (Precalificado.IdProducto == 202 || Precalificado.IdProducto == 203)
                {
                    lblTituloMontoPrestmo.Text = "Valor del vehículo";

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

                }

                /* Para utilizar las constantes de validaciones en el frontend */
                jsonConstantes = JsonConvert.SerializeObject(Constantes);
                jsonPrecalicado = JsonConvert.SerializeObject(Precalificado);
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
                { "uploadDir", uploadDir },
                { "maxSize", 500 }, //peso máximo de todos los archivos seleccionado en megas (MB)
                { "fileMaxSize", 10 }, //peso máximo por archivo
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

                            /* Capacidad de pago del cliente */
                            Constantes.CapacidadDePagoCliente = Precalificado.Disponible;
                            Constantes.IdentidadCliente = Precalificado.Identidad;

                            txtIdentidadCliente.Text = Precalificado.Identidad;
                            txtPrimerNombre.Text = Precalificado.PrimerNombre;
                            txtSegundoNombre.Text = Precalificado.SegundoNombre;
                            txtPrimerApellido.Text = Precalificado.PrimerApellido;
                            txtSegundoApellido.Text = Precalificado.SegundoApellido;
                            txtNumeroTelefono.Text = Precalificado.Telefono;
                            txtIngresosPrecalificados.Text = Precalificado.Ingresos.ToString();
                            txtIngresosMensuales.Text = Precalificado.Ingresos.ToString();
                            txtFechaDeNacimiento.Text = Precalificado.FechaNacimiento.ToString("yyyy-MM-dd");
                            /* Calcular edad del cliente */
                            var hoy = DateTime.Today;
                            var edad = hoy.Year - Precalificado.FechaNacimiento.Year;
                            if (Precalificado.FechaNacimiento.Date > hoy.AddYears(-edad)) edad--;

                            txtEdadDelCliente.Text = edad + " años";
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

                        /* Prestamo máximo del cliente */
                        Constantes.PrestamoMaximo_Monto = prestamoMaximoSegurido.MontoOfertado;
                        Constantes.PrestamoMaximo_Cuota = prestamoMaximoSegurido.Cuota;
                        Constantes.PrestamoMaximo_Plazo = prestamoMaximoSegurido.Plazo;
                        Constantes.PrestamoMaximo_TipoDePlazo = prestamoMaximoSegurido.TipoPlazo;
                        Constantes.TipoDePlazo = prestamoMaximoSegurido.TipoPlazo;

                        txtPrestamoMaximo.Text = Constantes.PrestamoMaximo_Monto.ToString();
                        txtPlazoMaximo.Text = Constantes.PrestamoMaximo_Plazo.ToString();
                        txtCuotaMaxima.Text = Constantes.PrestamoMaximo_Cuota.ToString();
                        lblTituloPlazoMaximo.Text = "Plazo " + Constantes.PrestamoMaximo_TipoDePlazo;
                        lblTituloCuotaMaxima.Text = "Cuota " + Constantes.PrestamoMaximo_TipoDePlazo;
                        lblTituloPlazo.Text = "Plazo " + Constantes.PrestamoMaximo_TipoDePlazo;
                    }
                } // using sp cotizador productos

                /* Verificar el tipo de cliente cuando se trate de renoviaciones y refinanciamiento para validar que solo se ingresen clientes excelentes y muy buenos*/
                if (Precalificado.IdClienteSAF != "")
                {
                    using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.Sp_Creditos_ClienteClasificacion", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@IDCliente", Precalificado.IdClienteSAF);

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            while (sqlResultado.Read())
                            {
                                Precalificado.TipoDeClienteSAF = sqlResultado["fcClasificacionCliente"].ToString();
                            }

                            if (Precalificado.TipoDeClienteSAF == "A - Excelente" || Precalificado.TipoDeClienteSAF == "B - Muy Bueno")
                            {
                                Precalificado.PermitirIngresarSolicitud = true;
                            }
                            else
                            {
                                Precalificado.PermitirIngresarSolicitud = false;
                                lblMensaje.InnerText = "(Esta solicitud no puede ser ingresada debido a la clasificación del cliente: " + Precalificado.TipoDeClienteSAF + ". Solo se permite A - Excelente y B - Muy Bueno)";
                                lblMensaje.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    Precalificado.PermitirIngresarSolicitud = true;
                }
            }// using conexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    public void CargarInformacionProducto()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_Catalogo_Productos_Listar", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", Precalificado.IdProducto);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            Constantes.RequierePrima = (byte)sqlResultado["fiRequierePrima"];
                            Constantes.PorcentajePrimaMinima = (decimal)sqlResultado["fnPorcentajePrimaMinima"];
                            Constantes.RequiereGarantia = (byte)sqlResultado["fiRequiereGarantia"];
                            Constantes.TipoDeGarantiaRequerida = sqlResultado["fcTipoDeGarantia"].ToString();
                            Constantes.RequiereOrigen = (byte)sqlResultado["fiRequiereOrigen"];
                            Constantes.RequiereGPS = (byte)sqlResultado["fiRequiereGPS"];
                            Constantes.MontoFinanciarMinimo = (decimal)sqlResultado["fnMontoFinanciarMinimo"];
                            Constantes.MontoFinanciarMaximo = (decimal)sqlResultado["fnMontoFinanciarMaximo"];
                            Constantes.IdTipoDePlazo = (int)sqlResultado["fiIDTipoDePlazo"];
                            Constantes.TipoDePlazo = sqlResultado["fcTipoDePlazo"].ToString();
                            Constantes.PlazoMinimo = (int)sqlResultado["fiPlazoMinimo"];
                            Constantes.PlazoMaximo = (int)sqlResultado["fiPlazoMaximo"];
                            Constantes.CantidadMinimaDeReferenciasPersonales = 4;
                        }
                    }
                }
            }
            /* Si el producto requiere prima, mostrar */
            if (Constantes.RequiereGarantia == 1)
            {
                step_garantia.Visible = true;
                step_garantia_titulo.Visible = true;
            }
            else
            {
                step_garantia.Visible = false;
                step_garantia_titulo.Visible = false;
            }

            txtTipoDeGarantia.Text = Constantes.TipoDeGarantiaRequerida;
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

    public void ValidarPreSolicitud()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                /* Validar si este cliente tiene solicitudes de crédito activas */
                using (var sqlComando = new SqlCommand("dbo.sp_CREDPreSolicitudes_Maestro_ObtenerPorIdentidad", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (sqlResultado.HasRows)
                        {
                            sqlResultado.Read();

                            var idTipoInvestigacion = sqlResultado["fiIDTipoDeUbicacion"].ToString(); // 1 = Domicilio, 2 = Trabajo
                            var estadoPreSolicitud = sqlResultado["fiEstadoPreSolicitud"].ToString();

                            switch (estadoPreSolicitud)
                            {
                                case "1":
                                case "2":
                                    Precalificado.PermitirIngresarSolicitud = false;
                                    Precalificado.MensajePermitirIngresarSolicitud += "Esta solicitud no puede ser ingresada porque hay una Pre Solicitud pendiente asociada a este cliente, comunicarse con el departamento de gestoría.";
                                    lblMensaje.InnerText = "(Esta solicitud no puede ser ingresada porque hay una Pre Solicitud pendiente asociada a este cliente, comunicarse con el departamento de gestoría)";
                                    lblMensaje.Visible = true;
                                    break;

                                case "3":
                                    break;

                                case "4":
                                    Precalificado.PermitirIngresarSolicitud = false;
                                    Precalificado.MensajePermitirIngresarSolicitud += "Esta solicitud no puede ser ingresada porque hay una Pre Solicitud rechazada asociada a este cliente, comunicarse con el departamento de gestoría.";
                                    lblMensaje.InnerText = "(Esta solicitud no puede ser ingresada porque hay una Pre Solicitud rechazada asociada a este cliente, comunicarse con el departamento de gestoría)";
                                    lblMensaje.Visible = true;
                                    break;

                                default:

                                    Precalificado.PermitirIngresarSolicitud = false;
                                    Precalificado.MensajePermitirIngresarSolicitud += "Esta solicitud no puede ser ingresada porque hay una Pre Solicitud con estado indefinido asociada a este cliente, contacte con el administrador.";
                                    lblMensaje.InnerText = "(Esta solicitud no puede ser ingresada porque hay una Pre Solicitud con estado indefinido asociada a este cliente, contacte con el administrador)";
                                    lblMensaje.Visible = true;
                                    break;
                            }

                            if (idTipoInvestigacion == "1" && estadoPreSolicitud == "3") // Investigacion de domicilio y que haya sido aprobada
                            {
                                divInformacionPreSolicitud_Domicilio.Visible = true;

                                txtTelefonoCasa.Text = sqlResultado["fcTelefonoAdicional"].ToString();
                                txtTelefonoCasa.ReadOnly = true;

                                txtDireccionDetalladaDomicilio.Text = sqlResultado["fcDireccionDetallada"].ToString();
                                txtDireccionDetalladaDomicilio.ReadOnly = true;

                                txtReferenciasDelDomicilio.Value = sqlResultado["fcReferenciasDireccionDetallada"].ToString();
                                txtReferenciasDelDomicilio.Disabled = true;

                                /* Departamento */
                                ddlDepartamentoDomicilio.SelectedValue = sqlResultado["fiIDDepartamento"].ToString();
                                ddlDepartamentoDomicilio.Enabled = false;

                                /* Municipio del domicilio */
                                var municipiosDeDepartamento = CargarMunicipios(int.Parse(sqlResultado["fiIDDepartamento"].ToString()));

                                ddlMunicipioDomicilio.Items.Clear();
                                ddlMunicipioDomicilio.Items.Add(new ListItem("Seleccionar", ""));

                                municipiosDeDepartamento.ForEach(municipio =>
                                {
                                    ddlMunicipioDomicilio.Items.Add(new ListItem(municipio.NombreMunicipio, municipio.IdMunicipio.ToString()));
                                });
                                ddlMunicipioDomicilio.SelectedValue = sqlResultado["fiIDMunicipio"].ToString();
                                ddlMunicipioDomicilio.Enabled = false;

                                /* Ciudad o Poblado del domicilio */
                                var ciudadesPobladosDelMunicipio = CargarCiudadesPoblados(int.Parse(sqlResultado["fiIDDepartamento"].ToString()), int.Parse(sqlResultado["fiIDMunicipio"].ToString()));

                                ddlCiudadPobladoDomicilio.Items.Clear();
                                ddlCiudadPobladoDomicilio.Items.Add(new ListItem("Seleccionar", ""));

                                ciudadesPobladosDelMunicipio.ForEach(ciudadPoblado =>
                                {
                                    ddlCiudadPobladoDomicilio.Items.Add(new ListItem(ciudadPoblado.NombreCiudadPoblado, ciudadPoblado.IdCiudadPoblado.ToString()));
                                });
                                ddlCiudadPobladoDomicilio.SelectedValue = sqlResultado["fiIDCiudad"].ToString();
                                ddlCiudadPobladoDomicilio.Enabled = false;

                                /* Barrio o colonia del domicilio */
                                var barriosColoniasDelPoblado = CargarBarriosColonias(int.Parse(sqlResultado["fiIDDepartamento"].ToString()), int.Parse(sqlResultado["fiIDMunicipio"].ToString()), int.Parse(sqlResultado["fiIDCiudad"].ToString()));

                                ddlBarrioColoniaDomicilio.Items.Clear();
                                ddlBarrioColoniaDomicilio.Items.Add(new ListItem("Seleccionar", ""));

                                barriosColoniasDelPoblado.ForEach(barrioColonia =>
                                {
                                    ddlBarrioColoniaDomicilio.Items.Add(new ListItem(barrioColonia.NombreBarrioColonia, barrioColonia.IdBarrioColonia.ToString()));
                                });
                                ddlBarrioColoniaDomicilio.SelectedValue = sqlResultado["fiIDBarrioColonia"].ToString();
                                ddlBarrioColoniaDomicilio.Enabled = false;
                            }
                            else if (idTipoInvestigacion == "2" && estadoPreSolicitud == "3") // Investigacion de trabajo y que haya sido aprobada
                            {
                                divInformacionPreSolicitud_Trabajo.Visible = true;

                                txtNombreDelTrabajo.Text = sqlResultado["fcNombreTrabajo"].ToString();
                                txtNombreDelTrabajo.ReadOnly = true;

                                txtTelefonoEmpresa.Text = sqlResultado["fcTelefonoAdicional"].ToString();
                                txtTelefonoEmpresa.ReadOnly = true;

                                txtExtensionRecursosHumanos.Text = sqlResultado["fcExtensionRecursosHumanos"].ToString();
                                txtExtensionRecursosHumanos.ReadOnly = true;

                                txtExtensionCliente.Text = sqlResultado["fcExtensionCliente"].ToString();
                                txtExtensionCliente.ReadOnly = true;

                                txtDireccionDetalladaEmpresa.Text = sqlResultado["fcDireccionDetallada"].ToString();
                                txtDireccionDetalladaEmpresa.ReadOnly = true;

                                txtReferenciasEmpresa.Value = sqlResultado["fcReferenciasDireccionDetallada"].ToString();
                                txtReferenciasEmpresa.Disabled = true;

                                /* Departamento */
                                ddlDepartamentoEmpresa.SelectedValue = sqlResultado["fiIDDepartamento"].ToString();
                                ddlDepartamentoEmpresa.Enabled = false;

                                /* Municipio */
                                var municipiosDeDepartamento = CargarMunicipios(int.Parse(sqlResultado["fiIDDepartamento"].ToString()));

                                ddlMunicipioEmpresa.Items.Clear();
                                ddlMunicipioEmpresa.Items.Add(new ListItem("Seleccionar", ""));

                                municipiosDeDepartamento.ForEach(municipio =>
                                {
                                    ddlMunicipioEmpresa.Items.Add(new ListItem(municipio.NombreMunicipio, municipio.IdMunicipio.ToString()));
                                });
                                ddlMunicipioEmpresa.SelectedValue = sqlResultado["fiIDMunicipio"].ToString();
                                ddlMunicipioEmpresa.Enabled = false;

                                /* Ciudad o Poblado */
                                var ciudadesPobladosDelMunicipio = CargarCiudadesPoblados(int.Parse(sqlResultado["fiIDDepartamento"].ToString()), int.Parse(sqlResultado["fiIDMunicipio"].ToString()));

                                ddlCiudadPobladoEmpresa.Items.Clear();
                                ddlCiudadPobladoEmpresa.Items.Add(new ListItem("Seleccionar", ""));

                                ciudadesPobladosDelMunicipio.ForEach(ciudadPoblado =>
                                {
                                    ddlCiudadPobladoEmpresa.Items.Add(new ListItem(ciudadPoblado.NombreCiudadPoblado, ciudadPoblado.IdCiudadPoblado.ToString()));
                                });
                                ddlCiudadPobladoEmpresa.SelectedValue = sqlResultado["fiIDCiudad"].ToString();
                                ddlCiudadPobladoEmpresa.Enabled = false;

                                /* Barrio o colonia */
                                var barriosColoniasDelPoblado = CargarBarriosColonias(int.Parse(sqlResultado["fiIDDepartamento"].ToString()), int.Parse(sqlResultado["fiIDMunicipio"].ToString()), int.Parse(sqlResultado["fiIDCiudad"].ToString()));

                                ddlBarrioColoniaEmpresa.Items.Clear();
                                ddlBarrioColoniaEmpresa.Items.Add(new ListItem("Seleccionar", ""));

                                barriosColoniasDelPoblado.ForEach(barrioColonia =>
                                {
                                    ddlBarrioColoniaEmpresa.Items.Add(new ListItem(barrioColonia.NombreBarrioColonia, barrioColonia.IdBarrioColonia.ToString()));
                                });
                                ddlBarrioColoniaEmpresa.SelectedValue = sqlResultado["fiIDBarrioColonia"].ToString();
                                ddlBarrioColoniaEmpresa.Enabled = false;
                            }
                        } // if sqlResultado.HasRows                      
                    } // sqlComando.ExecuteReader()
                } // using command
            } // using connection
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

                        sqlResultado.NextResult();

                        /* Viviendas */
                        ddlTipoDeVivienda.Items.Clear();
                        ddlTipoDeVivienda.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlTipoDeVivienda.Items.Add(new ListItem(sqlResultado["fcDescripcionVivienda"].ToString(), sqlResultado["fiIDVivienda"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Estados civiles */
                        ddlEstadoCivil.Items.Clear();
                        ddlEstadoCivil.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            var estadoCivil = new ListItem(sqlResultado["fcDescripcionEstadoCivil"].ToString(), sqlResultado["fiIDEstadoCivil"].ToString());
                            estadoCivil.Attributes.Add("data-requiereinformacionconyugal", bool.Parse(sqlResultado["fbRequiereInformacionConyugal"].ToString()).ToString().ToLower());
                            ddlEstadoCivil.Items.Add(estadoCivil);
                        }

                        sqlResultado.NextResult();

                        /* Nacionalidades */
                        ddlNacionalidad.Items.Clear();
                        ddlNacionalidad.Items.Add(new ListItem("Seleccionar", ""));
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
                        Session["DocumentosRequeridos"] = DocumentosRequeridos;

                        sqlResultado.NextResult();

                        /* Parentescos */
                        ddlParentescos.Items.Clear();
                        ddlParentescos.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlParentescos.Items.Add(new ListItem(sqlResultado["fcDescripcionParentesco"].ToString(), sqlResultado["fiIDParentesco"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Tiempo de residir */
                        ddlTiempoDeResidir.Items.Clear();
                        ddlTiempoDeResidir.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlTiempoDeResidir.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeResidir"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Tiempo de conocer referencia */
                        ddlTiempoDeConocerReferencia.Items.Clear();
                        ddlTiempoDeConocerReferencia.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlTiempoDeConocerReferencia.Items.Add(new ListItem(sqlResultado["fcDescripcion"].ToString(), sqlResultado["fiIDTiempoDeConocer"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Moneda */
                        ddlMoneda.Items.Clear();
                        ddlMoneda.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlMoneda.Items.Add(new ListItem(sqlResultado["fcNombreMoneda"].ToString(), sqlResultado["fiMoneda"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Tipo de cliente */
                        ddlTipoDeCliente.Items.Clear();
                        ddlTipoDeCliente.Items.Add(new ListItem("Seleccionar", ""));
                        while (sqlResultado.Read())
                        {
                            ddlTipoDeCliente.Items.Add(new ListItem(sqlResultado["fcTipoCliente"].ToString(), sqlResultado["fiTipoCliente"].ToString()));
                        }
                    }
                }

                ddlUnidadDeMedida.Items.Clear();
                ddlUnidadDeMedida.Items.Add(new ListItem("Kilómetros", "KM"));
                ddlUnidadDeMedida.Items.Add(new ListItem("Millas", "M"));
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
                        ddlOrigen.Items.Add(new ListItem("Seleccionar origen", ""));

                        while (sqlResultado.Read())
                        {
                            ddlOrigen.Items.Add(new ListItem(sqlResultado["fcOrigen"].ToString(), sqlResultado["fiIDOrigen"].ToString()));
                        }
                        ddlOrigen.Enabled = true;
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
                            ddlNacionalidad.SelectedValue = reader["fiNacionalidadCliente"].ToString();
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
                            ddlMunicipioDomicilio.Items.Add(new ListItem("Seleccionar", ""));

                            municipiosDeDepartamento.ForEach(municipio =>
                            {
                                ddlMunicipioDomicilio.Items.Add(new ListItem(municipio.NombreMunicipio, municipio.IdMunicipio.ToString()));
                            });
                            ddlMunicipioDomicilio.SelectedValue = reader["fiCodMunicipio"].ToString();
                            ddlMunicipioDomicilio.Enabled = true;

                            /* Ciudad o Poblado del domicilio */
                            var ciudadesPobladosDelMunicipio = CargarCiudadesPoblados(int.Parse(reader["fiCodDepartamento"].ToString()), int.Parse(reader["fiCodMunicipio"].ToString()));

                            ddlCiudadPobladoDomicilio.Items.Clear();
                            ddlCiudadPobladoDomicilio.Items.Add(new ListItem("Seleccionar", ""));

                            ciudadesPobladosDelMunicipio.ForEach(ciudadPoblado =>
                            {
                                ddlCiudadPobladoDomicilio.Items.Add(new ListItem(ciudadPoblado.NombreCiudadPoblado, ciudadPoblado.IdCiudadPoblado.ToString()));
                            });
                            ddlCiudadPobladoDomicilio.SelectedValue = reader["fiCodPoblado"].ToString();
                            ddlCiudadPobladoDomicilio.Enabled = true;

                            /* Barrio o colonia del domicilio */
                            var barriosColoniasDelPoblado = CargarBarriosColonias(int.Parse(reader["fiCodDepartamento"].ToString()), int.Parse(reader["fiCodMunicipio"].ToString()), int.Parse(reader["fiCodPoblado"].ToString()));

                            ddlBarrioColoniaDomicilio.Items.Clear();
                            ddlBarrioColoniaDomicilio.Items.Add(new ListItem("Seleccionar", ""));

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
                            ddlMunicipioEmpresa.Items.Add(new ListItem("Seleccionar", ""));

                            municipiosDeDepartamento.ForEach(municipio =>
                            {
                                ddlMunicipioEmpresa.Items.Add(new ListItem(municipio.NombreMunicipio, municipio.IdMunicipio.ToString()));
                            });
                            ddlMunicipioEmpresa.SelectedValue = reader["fiIDMunicipio"].ToString();
                            ddlMunicipioEmpresa.Enabled = true;

                            /* Ciudad o Poblado de la empresa */
                            var ciudadesPobladosDelMunicipio = CargarCiudadesPoblados(int.Parse(reader["fiIDDepartamento"].ToString()), int.Parse(reader["fiIDMunicipio"].ToString()));

                            ddlCiudadPobladoEmpresa.Items.Clear();
                            ddlCiudadPobladoEmpresa.Items.Add(new ListItem("Seleccionar", ""));

                            ciudadesPobladosDelMunicipio.ForEach(ciudadPoblado =>
                            {
                                ddlCiudadPobladoEmpresa.Items.Add(new ListItem(ciudadPoblado.NombreCiudadPoblado, ciudadPoblado.IdCiudadPoblado.ToString()));
                            });
                            ddlCiudadPobladoEmpresa.SelectedValue = reader["fiIDCiudad"].ToString();
                            ddlCiudadPobladoEmpresa.Enabled = true;

                            /* Barrio o colonia de la empresa */
                            var barriosColoniasDelPoblado = CargarBarriosColonias(int.Parse(reader["fiIDDepartamento"].ToString()), int.Parse(reader["fiIDMunicipio"].ToString()), int.Parse(reader["fiIDCiudad"].ToString()));

                            ddlBarrioColoniaEmpresa.Items.Clear();
                            ddlBarrioColoniaEmpresa.Items.Add(new ListItem("Seleccionar", ""));

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
                            txtFechaNacimientoConyugue.Text = DateTime.Parse(reader["fdFechaNacimientoConyugue"].ToString()).ToString("yyyy-MM-dd");
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
    public static CalculoPrestamo_ViewModel CalculoPrestamo(int idProducto, decimal valorGlobal, decimal valorPrima, int plazo, string dataCrypt)
    {
        var calculo = new CalculoPrestamo_ViewModel();
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
                            calculo = new CalculoPrestamo_ViewModel()
                            {
                                SegurodeDeuda = decimal.Parse(sqlResultado["fnSegurodeDeuda"].ToString()),
                                SegurodeVehiculo = decimal.Parse(sqlResultado["fnSegurodeVehiculo"].ToString()),
                                GastosdeCierre = decimal.Parse(sqlResultado["fnGastosdeCierre"].ToString()),
                                ValoraFinanciar = decimal.Parse(sqlResultado["fnValoraFinanciar"].ToString()),
                                CuotaQuincenal = decimal.Parse(sqlResultado["fnCuotaQuincenal"].ToString()),
                                CuotaMensual = decimal.Parse(sqlResultado["fnCuotaMensual"].ToString()),
                                CuotaServicioGPS = decimal.Parse(sqlResultado["fnCuotaServicioGPS"].ToString()),
                                CuotaSegurodeVehiculo = decimal.Parse(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()),
                                CuotaMensualNeta = decimal.Parse(sqlResultado["fnCuotaMensualNeta"].ToString()),
                                TotalSeguroVehiculo = decimal.Parse(sqlResultado["fnTotalSeguroVehiculo"].ToString())
                            };
                        }
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
    public static CalculoPrestamo_ViewModel CalculoPrestamoVehiculo(int idProducto, decimal valorGlobal, decimal valorPrima, int plazo, string scorePromedio, int tipoSeguro, int tipoGps, int gastosDeCierreFinanciados, string dataCrypt)
    {
        var calculo = new CalculoPrestamo_ViewModel();
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
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", idProducto);
                    sqlComando.Parameters.AddWithValue("@pnMontoaPrestamo", (valorGlobal - valorPrima));
                    sqlComando.Parameters.AddWithValue("@pnValorPrima", valorPrima);
                    sqlComando.Parameters.AddWithValue("@piScorePromedio", scorePromedio);
                    sqlComando.Parameters.AddWithValue("@piTipodeSeguro", tipoSeguro);
                    sqlComando.Parameters.AddWithValue("@piTipodeGPS", tipoGps);
                    sqlComando.Parameters.AddWithValue("@piFinanciandoGastosdeCierre", gastosDeCierreFinanciados);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            //tRowCotizacion = new HtmlTableRow();
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = sqlResultado["fiIDPlazo"].ToString() });
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fiInteresAnual"].ToString()).ToString("N") });
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnValorVehiculo"].ToString()).ToString("N") });
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnGastosdeCierre"].ToString()).ToString("N") });
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCostoGPS"].ToString()).ToString("N") });
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnTotalaFinanciar"].ToString()).ToString("N") });
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotadelPrestamo"].ToString()).ToString("N") });
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotaSegurodeVehiculo"].ToString()).ToString("N") });
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnCuotaServicioGPS"].ToString()).ToString("N") });
                            //tRowCotizacion.Cells.Add(new HtmlTableCell() { InnerText = decimal.Parse(sqlResultado["fnTotalCuota"].ToString()).ToString("N") });
                            //tblCotizacionPorPlazos.Rows.Add(tRowCotizacion);


                            if (sqlResultado["fiIDPlazo"].ToString() == plazo.ToString())
                            {
                                calculo = new CalculoPrestamo_ViewModel()
                                {
                                    ValoraFinanciar = decimal.Parse(sqlResultado["fnTotalaFinanciar"].ToString()),
                                    CuotaMensualNeta = decimal.Parse(sqlResultado["fnTotalCuota"].ToString())
                                };
                            }

                        }
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
    public static ResponseEntitie IngresarSolicitud(Solicitud_Maestro_ViewModel solicitud, Cliente_ViewModel cliente, Precalificado_ViewModel precalificado, Garantia_ViewModel garantia, bool esClienteNuevo, string dataCrypt)
    {
        var resultadoProceso = new ResponseEntitie();
        var mensajeError = string.Empty;

        using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
        {
            sqlConexion.Open();

            using (SqlTransaction tran = sqlConexion.BeginTransaction())
            {
                try
                {
                    var urlDesencriptado = DesencriptarURL(dataCrypt);
                    var pcID = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("ID");
                    var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
                    var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID") ?? "0";
                    var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr");
                    var nombreUsuario = string.Empty;
                    var fechaActual = DateTime.Now;
                    int contadorErrores = 0;
                    int idClienteInsertado = 0;

                    if (esClienteNuevo == true)
                    {
                        /* Verificar duplicidad de identidad y RTN */
                        int duplicidadIdentidad = 0;
                        int duplicidadRTN = 0;

                        using (var sqlComando = new SqlCommand("sp_CredCliente_ValidarDuplicidadIdentidades", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", precalificado.Identidad);
                            sqlComando.Parameters.AddWithValue("@fcRTN", cliente.RtnCliente);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    duplicidadIdentidad = (int)sqlResultado["fiValidacionIdentidad"];
                                    duplicidadRTN = (int)sqlResultado["fiValidacionRTN"];
                                }
                            }
                        }
                        if (duplicidadIdentidad > 0 || duplicidadRTN > 0)
                        {
                            string mensajeDuplicidad = string.Empty;

                            if (duplicidadIdentidad > 0 && duplicidadRTN > 0)
                            {
                                mensajeDuplicidad = "Error de duplicidad, el número de identidad y RTN ingresados ya existen";
                            }
                            else if (duplicidadIdentidad > 0)
                            {
                                mensajeDuplicidad = "Error de duplicidad, el número de identidad ingresado ya existe";
                            }
                            else if (duplicidadRTN > 0)
                            {
                                mensajeDuplicidad = "Error de duplicidad, el número de RTN ingresado ya existe";
                            }

                            resultadoProceso.response = false;
                            resultadoProceso.message = mensajeDuplicidad;
                            return resultadoProceso;
                        }

                        /* Registrar cliente maestro */
                        using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Maestro_Insert", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiTipoCliente", cliente.IdTipoCliente);
                            sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", precalificado.Identidad);
                            sqlComando.Parameters.AddWithValue("@fcRTN", cliente.RtnCliente);
                            sqlComando.Parameters.AddWithValue("@fcPrimerNombreCliente", precalificado.PrimerNombre);
                            sqlComando.Parameters.AddWithValue("@fcSegundoNombreCliente", precalificado.SegundoNombre);
                            sqlComando.Parameters.AddWithValue("@fcPrimerApellidoCliente", precalificado.PrimerApellido);
                            sqlComando.Parameters.AddWithValue("@fcSegundoApellidoCliente", precalificado.SegundoApellido);
                            sqlComando.Parameters.AddWithValue("@fcTelefonoCliente", cliente.TelefonoCliente);
                            sqlComando.Parameters.AddWithValue("@fiNacionalidadCliente", cliente.IdNacionalidad);
                            sqlComando.Parameters.AddWithValue("@fdFechaNacimientoCliente", precalificado.FechaNacimiento);
                            sqlComando.Parameters.AddWithValue("@fcCorreoElectronicoCliente", cliente.Correo);
                            sqlComando.Parameters.AddWithValue("@fcProfesionOficioCliente", cliente.ProfesionOficio);
                            sqlComando.Parameters.AddWithValue("@fcSexoCliente", cliente.Sexo);
                            sqlComando.Parameters.AddWithValue("@fiIDEstadoCivil", cliente.IdEstadoCivil);
                            sqlComando.Parameters.AddWithValue("@fiIDVivienda", cliente.IdVivienda);
                            sqlComando.Parameters.AddWithValue("@fiTiempoResidir", cliente.IdTiempoResidir);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    mensajeError = sqlResultado["MensajeError"].ToString();

                                    if (mensajeError.StartsWith("-1"))
                                        contadorErrores++;
                                    else
                                        idClienteInsertado = Convert.ToInt32(mensajeError);
                                }
                            }
                        }
                        if (contadorErrores > 0 || idClienteInsertado == 0)
                        {
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Error al guardar informacion personal del cliente";
                            return resultadoProceso;
                        }
                        solicitud.IdCliente = idClienteInsertado;
                    }

                    if (esClienteNuevo == false)
                    {
                        /* Si el cliente no es nuevo, validar que no tenga solicitudes de crédito activas */
                        int solicitudesActivas = 0;

                        using (var sqlComando = new SqlCommand("sp_CredSolicitud_ValidarClienteSolicitudesActivas", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                            sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", "");
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    solicitudesActivas = (int)sqlResultado["fiClienteSolicitudesActivas"];
                                }
                            }
                        }
                        if (solicitudesActivas > 0)
                        {
                            resultadoProceso.response = false;
                            resultadoProceso.message = "Hay una solicitud de crédito activa de este cliente, esperar resolución.";
                            return resultadoProceso;
                        }
                    }

                    /* Registrar cliente maestro */
                    int IdSolicitudInsertada = 0;
                    using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDSolicitud_Maestro_Insert", sqlConexion, tran))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDPais", 1);
                        sqlComando.Parameters.AddWithValue("@fiIDCanal", 1);
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDTipoProducto", precalificado.IdProducto);
                        sqlComando.Parameters.AddWithValue("@fiTipoSolicitud", precalificado.IdTipoDeSolicitud);
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@fnValorSeleccionado", solicitud.ValorSeleccionado);
                        sqlComando.Parameters.AddWithValue("@fiMoneda", solicitud.IdTipoMoneda);
                        sqlComando.Parameters.AddWithValue("@fiPlazoSeleccionado", solicitud.PlazoSeleccionado);
                        sqlComando.Parameters.AddWithValue("@fnValorPrima", solicitud.ValorPrima);
                        sqlComando.Parameters.AddWithValue("@fnValorGarantia", solicitud.ValorPrima == 0 ? 0 : solicitud.ValorGlobal);
                        sqlComando.Parameters.AddWithValue("@fiIDOrigen", solicitud.IdOrigen);
                        sqlComando.Parameters.AddWithValue("@fdFechaIngresoLaborarCliente", cliente.InformacionLaboral.FechaIngreso);
                        sqlComando.Parameters.AddWithValue("@fcCentrodeCosteAsignado", "");
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioAsignado", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@fdEnIngresoInicio", solicitud.EnIngresoInicio);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                        sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            while (sqlResultado.Read())
                            {
                                mensajeError = sqlResultado["MensajeError"].ToString();

                                if (mensajeError.StartsWith("-1"))
                                    contadorErrores++;
                                else
                                    IdSolicitudInsertada = Convert.ToInt32(mensajeError);
                            }
                        }
                    }
                    if (contadorErrores > 0)
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar la solicitud, contacte al administrador";
                        return resultadoProceso;
                    }

                    /* Registrar documentacion de la solicitud */
                    int DocumentacionCliente = 1;
                    string NombreCarpetaDocumentos = "Solicitud" + IdSolicitudInsertada;
                    string NuevoNombreDocumento = "";

                    /* lista de documentos adjuntados por el usuario */
                    var listaDocumentos = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                    /* Lista de documentos que se va ingresar en la base de datos y se va mover al nuevo directorio */
                    List<SolicitudesDocumentosViewModel> solicitudesDocumentos = new List<SolicitudesDocumentosViewModel>();

                    if (listaDocumentos != null)
                    {
                        /* lista de bloques y la cantidad de documentos que contiene cada uno */
                        var Bloques = listaDocumentos.GroupBy(TipoDocumento => TipoDocumento.fiTipoDocumento).Select(x => new { x.Key, Count = x.Count() });

                        /* lista donde se guardara temporalmente los documentos adjuntados por el usuario dependiendo del tipo de documento en el iterador */
                        List<SolicitudesDocumentosViewModel> DocumentosBloque = new List<SolicitudesDocumentosViewModel>();

                        foreach (var Bloque in Bloques)
                        {
                            int TipoDocumento = (int)Bloque.Key;
                            int CantidadDocumentos = Bloque.Count;

                            DocumentosBloque = listaDocumentos.Where(x => x.fiTipoDocumento == TipoDocumento).ToList();// documentos de este bloque
                            String[] NombresGenerador = Funciones.MultiNombres.GenerarNombreCredDocumento(DocumentacionCliente, IdSolicitudInsertada, TipoDocumento, CantidadDocumentos);

                            int ContadorNombre = 0;
                            foreach (SolicitudesDocumentosViewModel file in DocumentosBloque)
                            {
                                if (File.Exists(file.fcRutaArchivo + @"\" + file.NombreAntiguo)) /* si el archivo existe, que se agregue a la lista */
                                {
                                    NuevoNombreDocumento = NombresGenerador[ContadorNombre];
                                    solicitudesDocumentos.Add(new SolicitudesDocumentosViewModel()
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
                    if (solicitudesDocumentos.Count <= 0)
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar documentación, compruebe que haya cargado los documentos correctamente o vuelva a cargarlos";
                        return resultadoProceso;
                    }

                    foreach (SolicitudesDocumentosViewModel documento in solicitudesDocumentos)
                    {
                        using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Documentos_Guardar", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", IdSolicitudInsertada);
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
                                    mensajeError = sqlResultado["MensajeError"].ToString();
                                    if (mensajeError.StartsWith("-1"))
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
                    if (!FileUploader.GuardarSolicitudDocumentos(IdSolicitudInsertada, solicitudesDocumentos))
                    {
                        resultadoProceso.response = false;
                        resultadoProceso.message = "Error al guardar la documentación de la solicitud";
                        return resultadoProceso;
                    }

                    /* Registrar informacion laboral del cliente */
                    using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionLaboral_Insert", sqlConexion, tran))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IdSolicitudInsertada);
                        sqlComando.Parameters.AddWithValue("@fcNombreTrabajo", cliente.InformacionLaboral.NombreTrabajo);
                        sqlComando.Parameters.AddWithValue("@fnIngresosMensuales", precalificado.Ingresos);
                        sqlComando.Parameters.AddWithValue("@fcPuestoAsignado", cliente.InformacionLaboral.PuestoAsignado);
                        sqlComando.Parameters.AddWithValue("@fdFechaIngreso", cliente.InformacionLaboral.FechaIngreso);
                        sqlComando.Parameters.AddWithValue("@fcTelefonoEmpresa", cliente.InformacionLaboral.TelefonoEmpresa);
                        sqlComando.Parameters.AddWithValue("@fcExtensionRecursosHumanos", cliente.InformacionLaboral.ExtensionRecursosHumanos);
                        sqlComando.Parameters.AddWithValue("@fcExtensionCliente", cliente.InformacionLaboral.ExtensionCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDDepartamento", cliente.InformacionLaboral.IdDepartamento);
                        sqlComando.Parameters.AddWithValue("@fiIDMunicipio", cliente.InformacionLaboral.IdMunicipio);
                        sqlComando.Parameters.AddWithValue("@fiIDCiudad", cliente.InformacionLaboral.IdCiudadPoblado);
                        sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", cliente.InformacionLaboral.IdBarrioColonia);
                        sqlComando.Parameters.AddWithValue("@fcDireccionDetalladaEmpresa", cliente.InformacionLaboral.DireccionDetalladaEmpresa);
                        sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", cliente.InformacionLaboral.ReferenciasDireccionDetallada);
                        sqlComando.Parameters.AddWithValue("@fcFuenteOtrosIngresos", cliente.InformacionLaboral.FuenteOtrosIngresos);
                        sqlComando.Parameters.AddWithValue("@fnValorOtrosIngresosMensuales", cliente.InformacionLaboral.ValorOtrosIngresos);
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                        sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            while (sqlResultado.Read())
                            {
                                mensajeError = sqlResultado["MensajeError"].ToString();
                                if (mensajeError.StartsWith("-1"))
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

                    /* Registrar información del domicilio del cliente */
                    using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionDomiciliar_Insert", sqlConexion, tran))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IdSolicitudInsertada);
                        sqlComando.Parameters.AddWithValue("@fcTelefonoCasa", cliente.InformacionDomicilio.TelefonoCasa);
                        sqlComando.Parameters.AddWithValue("@fiIDDepartamento", cliente.InformacionDomicilio.IdDepartamento);
                        sqlComando.Parameters.AddWithValue("@fiIDMunicipio", cliente.InformacionDomicilio.IdMunicipio);
                        sqlComando.Parameters.AddWithValue("@fiIDCiudad", cliente.InformacionDomicilio.IdCiudadPoblado);
                        sqlComando.Parameters.AddWithValue("@fiIDBarrioColonia", cliente.InformacionDomicilio.IdBarrioColonia);
                        sqlComando.Parameters.AddWithValue("@fcDireccionDetallada", cliente.InformacionDomicilio.DireccionDetallada);
                        sqlComando.Parameters.AddWithValue("@fcReferenciasDireccionDetallada", cliente.InformacionDomicilio.ReferenciasDireccionDetallada);
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                        sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            while (sqlResultado.Read())
                            {
                                mensajeError = sqlResultado["MensajeError"].ToString();
                                if (mensajeError.StartsWith("-1"))
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

                    /* Registrar información conyugal del cliente */
                    if (cliente.InformacionConyugal.IndentidadConyugue != null)
                    {
                        using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_InformacionConyugal_Insert", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IdSolicitudInsertada);
                            sqlComando.Parameters.AddWithValue("@fcNombreCompletoConyugue", cliente.InformacionConyugal.NombreCompletoConyugue);
                            sqlComando.Parameters.AddWithValue("@fcIndentidadConyugue", cliente.InformacionConyugal.IndentidadConyugue);
                            sqlComando.Parameters.AddWithValue("@fdFechaNacimientoConyugue", cliente.InformacionConyugal.FechaNacimientoConyugue);
                            sqlComando.Parameters.AddWithValue("@fcTelefonoConyugue", cliente.InformacionConyugal.TelefonoConyugue);
                            sqlComando.Parameters.AddWithValue("@fcLugarTrabajoConyugue", cliente.InformacionConyugal.LugarTrabajoConyugue);
                            sqlComando.Parameters.AddWithValue("@fnIngresosMensualesConyugue", cliente.InformacionConyugal.IngresosMensualesConyugue);
                            sqlComando.Parameters.AddWithValue("@fcTelefonoTrabajoConyugue", cliente.InformacionConyugal.TelefonoConyugue);
                            sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                            sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);
                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    mensajeError = (string)sqlResultado["MensajeError"];
                                    if (mensajeError.StartsWith("-1"))
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

                    /* Guardar referencias personales del cliente */
                    if (cliente.ListaReferenciasPersonales != null)
                    {
                        if (cliente.ListaReferenciasPersonales.Count != 0)
                        {
                            foreach (var referenciaPersonal in cliente.ListaReferenciasPersonales)
                            {
                                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDCliente_Referencias_Insert", sqlConexion, tran))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", IdSolicitudInsertada);
                                    sqlComando.Parameters.AddWithValue("@fcNombreCompletoReferencia", referenciaPersonal.NombreCompletoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcLugarTrabajoReferencia", referenciaPersonal.LugarTrabajoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiTiempoConocerReferencia", referenciaPersonal.IdTiempoConocerReferencia);
                                    sqlComando.Parameters.AddWithValue("@fcTelefonoReferencia", referenciaPersonal.TelefonoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDParentescoReferencia", referenciaPersonal.IdParentescoReferencia);
                                    sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                                    sqlComando.Parameters.AddWithValue("@pcUserNameCreated", nombreUsuario);
                                    sqlComando.Parameters.AddWithValue("@pdDateCreated", fechaActual);

                                    using (var sqlResultado = sqlComando.ExecuteReader())
                                    {
                                        while (sqlResultado.Read())
                                        {
                                            mensajeError = sqlResultado["MensajeError"].ToString();

                                            if (mensajeError.StartsWith("-1"))
                                            {
                                                contadorErrores++;
                                            }
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

                    /* Si se requere garantía */
                    if (garantia != null)
                    {
                        using (var sqlComando = new SqlCommand("sp_CREDGarantias_Guardar", sqlConexion, tran))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDCanal", 1);
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", IdSolicitudInsertada);
                            sqlComando.Parameters.AddWithValue("@pcPrestamo", garantia.NumeroPrestamo);
                            sqlComando.Parameters.AddWithValue("@pcVin", garantia.VIN);
                            sqlComando.Parameters.AddWithValue("@pcTipoVehiculo", garantia.TipoDeVehiculo);
                            sqlComando.Parameters.AddWithValue("@pcTipoGarantia", garantia.TipoDeGarantia);
                            sqlComando.Parameters.AddWithValue("@pcMarca", garantia.Marca);
                            sqlComando.Parameters.AddWithValue("@pcModelo", garantia.Modelo);
                            sqlComando.Parameters.AddWithValue("@piAnio", garantia.Anio);
                            sqlComando.Parameters.AddWithValue("@pcColor", garantia.Color);
                            sqlComando.Parameters.AddWithValue("@pcCilindraje", garantia.Cilindraje);
                            sqlComando.Parameters.AddWithValue("@pnRecorrido", garantia.Recorrido);
                            sqlComando.Parameters.AddWithValue("@pcUnidadDeDistancia", garantia.UnidadDeDistancia);
                            sqlComando.Parameters.AddWithValue("@pcTransmision", garantia.Transmision);
                            sqlComando.Parameters.AddWithValue("@pcTipoCombustible", garantia.TipoDeCombustible);
                            sqlComando.Parameters.AddWithValue("@pcMatricula", garantia.Matricula);
                            sqlComando.Parameters.AddWithValue("@pcSerieUno", garantia.SerieUno);
                            sqlComando.Parameters.AddWithValue("@pcSerieDos", garantia.SerieDos);
                            sqlComando.Parameters.AddWithValue("@pcChasis", garantia.SerieChasis);
                            sqlComando.Parameters.AddWithValue("@pcMotor", garantia.SerieMotor);
                            sqlComando.Parameters.AddWithValue("@pcGPS", garantia.GPS);
                            sqlComando.Parameters.AddWithValue("@pnValorGarantia", solicitud.ValorGlobal);
                            sqlComando.Parameters.AddWithValue("@pnValorPrima", solicitud.ValorPrima);
                            sqlComando.Parameters.AddWithValue("@pnValorFinanciado", solicitud.ValorSeleccionado);
                            sqlComando.Parameters.AddWithValue("@pnGastosDeCierre", 0);
                            sqlComando.Parameters.AddWithValue("@pcComentario", garantia.Comentario);
                            sqlComando.Parameters.AddWithValue("@pbDigitadoManualmente", garantia.EsDigitadoManualmente);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.CommandTimeout = 120;

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    var resultadoSp = sqlResultado["MensajeError"].ToString();

                                    if (!resultadoSp.StartsWith("-1"))
                                    {
                                        mensajeError = sqlResultado["MensajeError"].ToString();
                                        if (mensajeError.StartsWith("-1"))
                                            contadorErrores++;
                                    }
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            resultadoProceso.response = false;
                            resultadoProceso.message = "No se pudo guardar la información de la garantía, contacte al administrador";
                            return resultadoProceso;
                        }
                    }

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
            var lcParametros = string.Empty;
            var pcEncriptado = string.Empty;
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


    #region View Models
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
        public CotizadorProductos_ViewModel PrestamoMaximoSugerido { get; set; }


        public string ScorePromedio { get; set; }
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

