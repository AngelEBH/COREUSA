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
using Newtonsoft.Json;
using proyectoBase.Models.ViewModel;
using adminfiles;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net.Mail;

public partial class SolicitudesCredito_Registrar : System.Web.UI.Page
{
    #region Propiedades

    private string pcID = ""; /* Identidad del cliente */
    private string pcIDApp = ""; /* Id de la aplicación */
    private string pcIDSesion = ""; /* Id de la sesion */
    private string pcIDUsuario = ""; /* Id del usuario */
    public string jsonConstantes; /* String para almacenar en formato JSON algunos parametros de validacion que se enviaran al frontend cuando cargue la pagina */
    public string jsonPrecalicado; /* String para almacenar en formato JSON la información del precalificado del cliente para validaciones que se enviaran al frontend cuando cargue la pagina */
    public Precalificado_ViewModel Precalificado;
    public List<TipoDocumento_ViewModel> DocumentosRequeridos; /* Lista de documentos requeridos para ingresar la solicitud de crédito. Se usa para generar dinamicamente los inputs de los documentos en el front */
    public SolicitudesCredito_Registrar_Constantes Constantes;
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    #endregion

    #region Page Load, CargarPrecalificado, CargarInformacionProducto, ValidarClienteSolicitudesActivas, ValidarPreSolicitud, CargarListas, CargarOrigenes, ObtenerInformacionCliente

    protected void Page_Load(object sender, EventArgs e)
    {
        var type = Request.QueryString["type"];

        /* Captura de parámetros encriptados */
        if (!IsPostBack && type == null)
        {
            Precalificado = new Precalificado_ViewModel();
            Constantes = new SolicitudesCredito_Registrar_Constantes();
            DocumentosRequeridos = new List<TipoDocumento_ViewModel>();

            var lcURL = Request.Url.ToString();
            var liParamStart = lcURL.IndexOf("?");
            var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

            if (lcParametros != string.Empty)
            {
                var pcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1));
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
                    CargarOrigenes();

                if (Constantes.RequierePrima == 1)
                    txtValorPrima.Enabled = true;

                switch (Precalificado.IdProducto)
                {
                    case 202:
                    case 204:
                        lblTituloMontoPrestmo.Text = "Valor del vehiculo";
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

                        btnSeleccionarPrecioDeMercado.Visible = true;
                        txtValorGlobal.ReadOnly = true;
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

                        btnSeleccionarPrecioDeMercado.Visible = true;
                        txtValorGlobal.ReadOnly = true;
                        break;

                    case 101:
                        lblTituloMontoPrestmo.Text = "Cantidad efectivo solicitado";
                        break;

                    case 201:
                        lblTituloMontoPrestmo.Text = "Valor de la moto";
                        break;

                    case 301:
                    case 302:
                        lblTituloMontoPrestmo.Text = "Precio del producto";
                        break;
                }

                /* Para utilizar las constantes de validaciones en el frontend */
                jsonConstantes = JsonConvert.SerializeObject(Constantes);
                jsonPrecalicado = JsonConvert.SerializeObject(Precalificado);
            }
        }

        /* Guardar documentos de la solicitud que son enviados a través de peticiones AJAX */
        if (type != null || Request.HttpMethod == "POST")
        {
            Session["tipoDoc"] = Convert.ToInt32(Request.QueryString["doc"]);
            var uploadDir = @"C:\inetpub\wwwroot\Documentos\Solicitudes\Temp\";

            var fileUploader = new FileUploader("files", new Dictionary<string, dynamic>() {
{ "limit", 1 },
{ "title", "auto" },
{ "uploadDir", uploadDir },
{ "extensions", new string[] { "jpg", "png", "jpeg"} },
{ "maxSize", 500 }, //peso máximo de todos los archivos seleccionado en megas (MB)
{ "fileMaxSize", 10 }, //peso máximo por archivo
});

            switch (type)
            {
                case "upload":
                    var data = fileUploader.Upload(); /* Subir archivo al servidor y guardarlo en carpeta Temp*/

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

                using (var sqlComando = CrearSqlComando("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion)) /* Cargar precalificado del cliente */
                {
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        /* Si no está precalificado, retornar a pantalla de precalificacion */
                        if (!sqlResultado.HasRows)
                            Response.Write("<script>window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')</script>");

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
                            lblScoreCliente.InnerText = Precalificado.ScorePromedio;
                        }
                    }
                } // using sp consulta ejecutivos

                /* Obtener el préstamo máximo que se le puede ofertar al cliente para validaciones */
                using (var sqlComando = CrearSqlComando("sp_CotizadorProductos", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", Precalificado.IdProducto);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", Precalificado.Identidad);
                    sqlComando.Parameters.AddWithValue("@piConObligaciones", Precalificado.Obligaciones > 0 ? "1" : "0");
                    sqlComando.Parameters.AddWithValue("@pnIngresosBrutos", Precalificado.Ingresos);
                    sqlComando.Parameters.AddWithValue("@pnIngresosDisponible", Precalificado.Disponible);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        var prestamoMaximoSegurido = new CotizadorProductos_ViewModel();
                        var montoMayor = 0m;
                        var idContador = 1;

                        while (sqlResultado.Read())
                        {
                            if (montoMayor < decimal.Parse(sqlResultado["fnMontoOfertado"].ToString()))
                            {
                                prestamoMaximoSegurido = new CotizadorProductos_ViewModel()
                                {
                                    IdCotizacion = idContador,
                                    IdProducto = int.Parse(sqlResultado["fiIDProducto"].ToString()),
                                    Producto = sqlResultado["fcProducto"].ToString(),
                                    MontoOfertado = decimal.Parse(sqlResultado["fnMontoOfertado"].ToString()),
                                    Plazo = int.Parse(sqlResultado["fiPlazo"].ToString()),
                                    Cuota = decimal.Parse(sqlResultado["fnCuotaQuincenal"].ToString()),
                                    TipoPlazo = sqlResultado["fcTipodeCuota"].ToString()
                                };
                            }
                            montoMayor = decimal.Parse(sqlResultado["fnMontoOfertado"].ToString());
                            idContador++;
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
                        //lblTituloPlazo.Text = "Plazo " + Constantes.PrestamoMaximo_TipoDePlazo;
                    }
                } // using sp cotizador productos

                /* Verificar el tipo de cliente cuando se trate de renoviaciones y refinanciamiento para validar que solo se ingresen clientes excelentes y muy buenos*/
                if (Precalificado.IdClienteSAF != "")
                {
                    using (var sqlComando = CrearSqlComando("Sp_Creditos_ClienteClasificacion", sqlConexion))
                    {
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@IDCliente", Precalificado.IdClienteSAF);

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            while (sqlResultado.Read())
                                Precalificado.TipoDeClienteSAF = sqlResultado["fcClasificacionCliente"].ToString();

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
                } // if Precalificado.IdClienteSAF != ""
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

                using (var sqlComando = CrearSqlComando("sp_Catalogo_Productos_Listar", sqlConexion))
                {
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

                using (var sqlComando = CrearSqlComando("sp_Catalogo_Productos_ObtenerPlazosPorIdProducto", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@piIDProducto", Precalificado.IdProducto);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlPlazosDisponibles.Items.Clear();

                        while (sqlResultado.Read())
                            ddlPlazosDisponibles.Items.Add(new ListItem(sqlResultado["fcPlazo"].ToString(), sqlResultado["fiPlazo"].ToString()));
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
                using (var sqlComando = CrearSqlComando("dbo.sp_CredSolicitud_ValidarClienteSolicitudesActivas", sqlConexion))
                {
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
                                Precalificado.PermitirIngresarSolicitud = false;
                                Precalificado.MensajePermitirIngresarSolicitud = "Este cliente ya cuenta con una solicitud de crédito activa, esperar resolución";
                                lblMensaje.InnerText = "(Este cliente ya cuenta con una solicitud de crédito activa, esperar resolución)";
                                lblMensaje.Visible = true;
                            }
                        }
                    }
                } // using sqlComando
            } // using connection
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

                /* Validar si existe una pre solicitud de este cliente y el estado de la misma */
                using (var sqlComando = CrearSqlComando("dbo.sp_CREDPreSolicitudes_Maestro_ObtenerPorIdentidad", sqlConexion))
                {
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
                            var estadoPreSolicitud = sqlResultado["fiEstadoPreSolicitud"].ToString(); // ver CREDPreSolicitudes_Catalogo_Estados

                            switch (estadoPreSolicitud)
                            {
                                case "1": // Pendiente
                                case "2": // Recibida
                                    Precalificado.PermitirIngresarSolicitud = false;
                                    Precalificado.MensajePermitirIngresarSolicitud += "Esta solicitud no puede ser ingresada porque hay una Pre Solicitud pendiente asociada a este cliente, comunicarse con el departamento de gestoría.";
                                    lblMensaje.InnerText = "(Esta solicitud no puede ser ingresada porque hay una Pre Solicitud pendiente asociada a este cliente, comunicarse con el departamento de gestoría)";
                                    lblMensaje.Visible = true;
                                    break;

                                case "3": // Aprobada
                                case "5": // Solicitud ingresada
                                    break;

                                case "4": // rechazada
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

                using (var sqlComando = CrearSqlComando("sp_CREDSolicitud_Guardar_LlenarListas", sqlConexion))
                {
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

                        ddlEstadoCivilPropietario.Items.Clear();
                        ddlEstadoCivilPropietario.Items.Add(new ListItem("Seleccionar", "0"));

                        ddlEstadoCivilVendedor.Items.Clear();
                        ddlEstadoCivilVendedor.Items.Add(new ListItem("Seleccionar", "0"));

                        while (sqlResultado.Read())
                        {
                            var estadoCivil = new ListItem(sqlResultado["fcDescripcionEstadoCivil"].ToString(), sqlResultado["fiIDEstadoCivil"].ToString());
                            estadoCivil.Attributes.Add("data-requiereinformacionconyugal", bool.Parse(sqlResultado["fbRequiereInformacionConyugal"].ToString()).ToString().ToLower());

                            ddlEstadoCivil.Items.Add(estadoCivil);

                            ddlEstadoCivilPropietario.Items.Add(new ListItem(sqlResultado["fcDescripcionEstadoCivil"].ToString(), sqlResultado["fiIDEstadoCivil"].ToString()));
                            ddlEstadoCivilVendedor.Items.Add(new ListItem(sqlResultado["fcDescripcionEstadoCivil"].ToString(), sqlResultado["fiIDEstadoCivil"].ToString()));
                        }

                        sqlResultado.NextResult();

                        /* Nacionalidades */
                        ddlNacionalidad.Items.Clear();
                        ddlNacionalidad.Items.Add(new ListItem("Seleccionar", ""));

                        ddlNacionalidadPropietario.Items.Clear();
                        ddlNacionalidadPropietario.Items.Add(new ListItem("Seleccionar", "0"));

                        ddlNacionalidadVendedor.Items.Clear();
                        ddlNacionalidadVendedor.Items.Add(new ListItem("Seleccionar", "0"));

                        while (sqlResultado.Read())
                        {
                            ddlNacionalidad.Items.Add(new ListItem(sqlResultado["fcDescripcionNacionalidad"].ToString(), sqlResultado["fiIDNacionalidad"].ToString()));
                            ddlNacionalidadPropietario.Items.Add(new ListItem(sqlResultado["fcDescripcionNacionalidad"].ToString(), sqlResultado["fiIDNacionalidad"].ToString()));
                            ddlNacionalidadVendedor.Items.Add(new ListItem(sqlResultado["fcDescripcionNacionalidad"].ToString(), sqlResultado["fiIDNacionalidad"].ToString()));
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

                        /* Moneda */
                        sqlResultado.NextResult();

                        //ddlMoneda.Items.Clear();
                        //ddlMoneda.Items.Add(new ListItem("Seleccionar", ""));
                        //while (sqlResultado.Read())
                        //{
                        // ddlMoneda.Items.Add(new ListItem(sqlResultado["fcNombreMoneda"].ToString(), sqlResultado["fiMoneda"].ToString()));
                        //}

                        /* Tipo de cliente */
                        sqlResultado.NextResult();

                        //ddlTipoDeCliente.Items.Clear();
                        //ddlTipoDeCliente.Items.Add(new ListItem("Seleccionar", ""));
                        //while (sqlResultado.Read())
                        //{
                        // ddlTipoDeCliente.Items.Add(new ListItem(sqlResultado["fcTipoCliente"].ToString(), sqlResultado["fiTipoCliente"].ToString()));
                        //}
                    }
                }

                ddlUnidadDeMedida.Items.Clear();
                ddlUnidadDeMedida.Items.Add(new ListItem("Kilómetros", "KM"));
                ddlUnidadDeMedida.Items.Add(new ListItem("Millas", "M"));

                ddlMarca.Items.Add(new ListItem("Seleccionar", ""));
                ddlModelo.Items.Add(new ListItem("Seleccione una marca", ""));
                ddlAnio.Items.Add(new ListItem("Seleccione un modelo", ""));
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

                using (var sqlComando = CrearSqlComando("sp_CredCatalogo_Origenes", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDProducto", Precalificado.IdProducto);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlOrigen.Items.Clear();
                        ddlOrigen.Items.Add(new ListItem("Seleccionar origen", ""));

                        while (sqlResultado.Read())
                            ddlOrigen.Items.Add(new ListItem(sqlResultado["fcOrigen"].ToString(), sqlResultado["fiIDOrigen"].ToString()));

                        ddlOrigen.Enabled = true;
                    }
                } // using comando
            } // using connection
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

                using (var sqlComando = CrearSqlComando("sp_CREDCliente_ObtenerInformacionPorIdentidad", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@fcIdentidadCliente", pcID);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                            return;

                        /* Clientes Maestro */
                        while (sqlResultado.Read())
                        {
                            Constantes.EsClienteNuevo = false;
                            Constantes.IdCliente = int.Parse(sqlResultado["fiIDCliente"].ToString());
                            Constantes.EstadoCliente = (bool)sqlResultado["fbClienteActivo"];
                            Constantes.RazonInactivo = sqlResultado["fcRazonInactivo"].ToString();

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

                            ddlEstadoCivil.SelectedValue = sqlResultado["fiIDEstadoCivil"].ToString();
                            //ddlTipoDeCliente.SelectedValue = sqlResultado["fiTipoCliente"].ToString();
                            ddlTipoDeVivienda.SelectedValue = sqlResultado["fiIDVivienda"].ToString();
                            ddlTiempoDeResidir.SelectedValue = sqlResultado["fiTiempoResidir"].ToString();
                        }

                        /* Información de domicilio */
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

                        /* Información laboral */
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

                        /* Información del conyugue */
                        sqlResultado.NextResult();

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
                    } // using command.ExecuteReader()
                } // using new command
            } // using connection
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    #endregion

    #region Cargar municipios, poblados, barrios y colonias

    public static List<Municipios_ViewModel> CargarMunicipios(int idDepartamento)
    {
        var municipios = new List<Municipios_ViewModel>();
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_GeoMunicipio", sqlConexion))
                {
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
                } // using sqlComando
            } // using sqlConexion
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

                using (var sqlComando = CrearSqlComando("sp_GeoPoblado", sqlConexion))
                {
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
                } // using sqlComando
            } // using sqlConexion
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

                using (var sqlComando = CrearSqlComando("sp_GeoBarrios", sqlConexion))
                {
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
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            BarriosColonias = null;
        }
        return BarriosColonias;
    }

    #endregion

    #region WebMethods que se llaman desde el frontend para cargar municipios, poblados, colonias y los documentos requeridos

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

    #endregion

    #region C A L C U L O S D E P R E S T A M O S por producto

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

                using (var sqlComando = CrearSqlComando("sp_CredSolicitud_CalculoPrestamo", sqlConexion))
                {
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
                        }
                    } // using sqlResultado.ExecuteReader()
                } // using sqlComando
            } // using sqlConexion
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

                using (var sqlComando = CrearSqlComando("sp_CredCotizadorProductos_Vehiculos", sqlConexion))
                {
                    valorPrima = (idProducto == 203) ? valorGlobal - valorPrima : valorPrima;
                    var montoPrestamo = (idProducto == 203) ? valorGlobal - valorPrima : valorGlobal - valorPrima;

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
                                calculo = new CalculoPrestamo_ViewModel()
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
                                    TipoCuota = "Meses",
                                    ValorDelPrestamo = montoPrestamo
                                };
                            }
                        }
                    } // using command.ExecuteReader()
                } // using command
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

    #region I N G R E S A R S O L I C I T U D D E C R E D I T O

    [WebMethod]
    public static Resultado_ViewModel IngresarSolicitud(Solicitud_Maestro_ViewModel solicitud, Cliente_ViewModel cliente, Precalificado_ViewModel precalificado, Garantia_ViewModel garantia, bool esClienteNuevo, string dataCrypt, CalculoPrestamo_ViewModel cotizador)
    {
        var resultadoProceso = new Resultado_ViewModel();
        var mensajeError = string.Empty;

        using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
        {
            sqlConexion.Open();

            using (SqlTransaction sqlTransaction = sqlConexion.BeginTransaction())
            {
                try
                {
                    var urlDesencriptado = DesencriptarURL(dataCrypt);
                    var pcID = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("ID");
                    var pcIDApp = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("IDApp");
                    var pcIDSesion = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("SID") ?? "0";
                    var pcIDUsuario = HttpUtility.ParseQueryString(urlDesencriptado.Query).Get("usr");

                    var fechaActual = DateTime.Now;
                    var nombreUsuario = string.Empty;
                    var contadorErrores = 0;
                    var idClienteInsertado = 0;

                    /* si es cliente nuevo, validar que no haya duplicidad de identidad y RTN */
                    if (esClienteNuevo == true)
                    {
                        var duplicidadIdentidad = 0;
                        var duplicidadRTN = 0;

                        using (var sqlComando = new SqlCommand("sp_CredCliente_ValidarDuplicidadIdentidades", sqlConexion, sqlTransaction))
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
                            var mensajeDuplicidad = string.Empty;

                            if (duplicidadIdentidad > 0 && duplicidadRTN > 0)
                                mensajeDuplicidad = "Error de duplicidad, el número de identidad y RTN ingresados ya existen";
                            else if (duplicidadIdentidad > 0)
                                mensajeDuplicidad = "Error de duplicidad, el número de identidad ingresado ya existe";
                            else if (duplicidadRTN > 0)
                                mensajeDuplicidad = "Error de duplicidad, el número de RTN ingresado ya existe";

                            resultadoProceso.ResultadoExitoso = false;
                            resultadoProceso.MensajeResultado = mensajeDuplicidad;
                            return resultadoProceso;
                        }

                        /* Registrar cliente maestro */
                        using (var sqlComando = new SqlCommand("sp_CREDCliente_Maestro_Insert", sqlConexion, sqlTransaction))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiTipoCliente", 1);
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
                            resultadoProceso.ResultadoExitoso = false;
                            resultadoProceso.MensajeResultado = "Error al guardar informacion personal del cliente";
                            return resultadoProceso;
                        }
                        solicitud.IdCliente = idClienteInsertado;
                    }

                    if (esClienteNuevo == false) /* Si el cliente no es nuevo, validar que no tenga solicitudes de crédito activas */
                    {
                        int solicitudesActivas = 0;

                        using (var sqlComando = new SqlCommand("sp_CredSolicitud_ValidarClienteSolicitudesActivas", sqlConexion, sqlTransaction))
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
                                    solicitudesActivas = (int)sqlResultado["fiClienteSolicitudesActivas"];
                            }
                        }
                        if (solicitudesActivas > 0)
                        {
                            resultadoProceso.ResultadoExitoso = false;
                            resultadoProceso.MensajeResultado = "Hay una solicitud de crédito activa de este cliente, esperar resolución.";
                            return resultadoProceso;
                        }
                    }

                    /****** Obtener todos los documentos que se van a guardar asociadas a la solicitud de credito. ******/
                    /****** Se obtienen antes de guardar la solicitud para poder obtener los documentos de la pre solicitud antes de que esta se actualice a estado 5 (SOLICITUD INGRESADA) ******/

                    /* Obtener documentos adjuntados por el usuario en el formulario del ingreso de la solicitud */
                    var listaDeDocumentosAdjuntadosYPreSolicitud = (List<SolicitudesDocumentosViewModel>)HttpContext.Current.Session["ListaSolicitudesDocumentos"];

                    /* Documentos de la pre solicitud en caso de que exista una */
                    var listaDeDocumentosPreSolicitud = new List<SolicitudesDocumentosViewModel>();
                    var idPreSolicitud = 0;
                    var idEstadoPreSolicitud = 0;

                    if (listaDeDocumentosAdjuntadosYPreSolicitud != null) /* Si no se adjuntaron documentos en el formulario de ingreso, mostrar mensaje de error porque es requerido por lo menos un documento */
                    {
                        /* Obtener documentos de la PRE SOLICITUD en caso de que exista una */
                        using (var sqlComando = new SqlCommand("sp_CREDPreSolicitudes_Maestro_ObtenerPorIdentidad", sqlConexion, sqlTransaction))
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
                                    /****** Primer resultado: Información de la pre solicitud ******/

                                    while (sqlResultado.Read())
                                    {
                                        idPreSolicitud = (int)sqlResultado["fiIDPreSolicitud"];
                                        idEstadoPreSolicitud = (byte)sqlResultado["fiEstadoPreSolicitud"];
                                    }

                                    if (idEstadoPreSolicitud == 3) // Si la pre solicitud corresponde a este cliente y está en estado APROBADA (mirar CREDPreSolicitudes_Catalogo_Estados)
                                    {
                                        /****** Segundo resultado: Documentos de la pre solicitud ******/
                                        sqlResultado.NextResult();

                                        while (sqlResultado.Read())
                                        {
                                            listaDeDocumentosPreSolicitud.Add(new SolicitudesDocumentosViewModel()
                                            {
                                                fcNombreArchivo = sqlResultado["fcNombreArchivo"].ToString(),
                                                NombreAntiguo = sqlResultado["fcNombreArchivo"].ToString(),
                                                fcTipoArchivo = sqlResultado["fcTipoArchivo"].ToString(),
                                                fcRutaArchivo = sqlResultado["fcRutaArchivo"].ToString(),
                                                URLArchivo = sqlResultado["fcURL"].ToString(),
                                                fiTipoDocumento = (int)sqlResultado["fiTipoDocumento"]
                                            });

                                            listaDeDocumentosAdjuntadosYPreSolicitud.Add(new SolicitudesDocumentosViewModel()
                                            {
                                                fcNombreArchivo = "",
                                                NombreAntiguo = sqlResultado["fcNombreArchivo"].ToString(),
                                                fcTipoArchivo = sqlResultado["fcTipoArchivo"].ToString(),
                                                fcRutaArchivo = sqlResultado["fcRutaArchivo"].ToString(),
                                                URLArchivo = sqlResultado["fcURL"].ToString(),
                                                fiTipoDocumento = (int)sqlResultado["fiTipoDocumento"]
                                            });
                                        }
                                    }
                                } // if sqlResultado.HasRows
                            } // using sqlComando.ExecuteReader()
                        } // using sqlComando sp_CREDPreSolicitudes_Maestro_ObtenerPorIdentidad
                    } // if listaDeDocumentosAdjuntadosYPreSolicitud != null
                    else
                    {
                        resultadoProceso.ResultadoExitoso = false;
                        resultadoProceso.MensajeResultado = "Error al registrar la documentación, compruebe los documentos adjuntados o vuelva a cargarlos.";
                        return resultadoProceso;
                    }

                    if (listaDeDocumentosAdjuntadosYPreSolicitud.Count <= 0) // si no hay ningun documento de la solicitud, mostrar mensaje de error.
                    {
                        resultadoProceso.ResultadoExitoso = false;
                        resultadoProceso.MensajeResultado = "Error al guardar documentación, compruebe que haya cargado los documentos correctamente o vuelva a cargarlos";
                        return resultadoProceso;
                    }


                    /* Registrar solicitud maestro */
                    int idSolicitudInsertada = 0;

                    using (var sqlComando = new SqlCommand("sp_CREDSolicitud_Maestro_Insert", sqlConexion, sqlTransaction))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDPais", 1);
                        sqlComando.Parameters.AddWithValue("@fiIDCanal", 1);
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDTipoProducto", precalificado.IdProducto);
                        sqlComando.Parameters.AddWithValue("@fiTipoSolicitud", precalificado.IdTipoDeSolicitud);
                        sqlComando.Parameters.AddWithValue("@fiIDUsuarioCrea", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@fnValorSeleccionado", solicitud.ValorSeleccionado);
                        sqlComando.Parameters.AddWithValue("@fiMoneda", 1);
                        sqlComando.Parameters.AddWithValue("@fiPlazoSeleccionado", solicitud.PlazoSeleccionado);
                        sqlComando.Parameters.AddWithValue("@fnValorPrima", precalificado.IdProducto == 203 ? 0 : solicitud.ValorPrima);
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
                                    idSolicitudInsertada = Convert.ToInt32(mensajeError);
                            }
                        }
                    }

                    if (contadorErrores > 0)
                    {
                        resultadoProceso.ResultadoExitoso = false;
                        resultadoProceso.MensajeResultado = "Error al guardar la solicitud, contacte al administrador";
                        return resultadoProceso;
                    }

                    /* Guardar informacion del cotizador para imprimir documentos... si,esto va a fallar también */
                    if (precalificado.IdProducto == 202 || precalificado.IdProducto == 203 || precalificado.IdProducto == 204 || precalificado.IdProducto == 201)
                    {
                        var fechaPrimerPago = ObtenerFechaPrimerPago(precalificado.IdProducto.ToString());
                        decimal totalAFinanciar = cotizador.TotalAFinanciar;
                        //decimal valorAPrestar = garantia.ValorMercado - garantia.ValorPrima;
                        decimal valorAPrestar = precalificado.IdProducto != 203 ? garantia.ValorMercado - garantia.ValorPrima : garantia.ValorPrima;
                        decimal tasaInteresAnual = (precalificado.IdProducto == 202 || precalificado.IdProducto == 203 || precalificado.IdProducto == 204) ? cotizador.TasaInteresAnual : ObtenerTasaInteresAnualPorIdProducto(precalificado.IdProducto);
                        decimal tasaInteresMensual = tasaInteresAnual / 12;
                        decimal totalAFinanciarConIntereses = (precalificado.IdProducto == 202 || precalificado.IdProducto == 203 || precalificado.IdProducto == 204) ? cotizador.TotalFinanciadoConIntereses : CalcularTotalAFinanciarConIntereses(cotizador.TotalAFinanciar, solicitud.PlazoSeleccionado, tasaInteresAnual, precalificado.IdProducto);

                        using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_InformacionPrestamo_Guardar", sqlConexion, sqlTransaction))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDCanal", 1);
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitudInsertada);
                            sqlComando.Parameters.AddWithValue("@pcNumeroPrestamo", "No disponible");
                            sqlComando.Parameters.AddWithValue("@pdFechaPrimerCuota", fechaPrimerPago);
                            sqlComando.Parameters.AddWithValue("@pnValorTotalFinanciamiento", totalAFinanciar);
                            sqlComando.Parameters.AddWithValue("@pnValorAPrestar", valorAPrestar);
                            sqlComando.Parameters.AddWithValue("@pnCostoGPS", cotizador.CostoGPS);
                            sqlComando.Parameters.AddWithValue("@pnValorTotalSeguro", cotizador.TotalSeguroVehiculo);
                            sqlComando.Parameters.AddWithValue("@pnGastosDeCierre", cotizador.GastosdeCierre);
                            sqlComando.Parameters.AddWithValue("@pnTasaMensualAplicada", tasaInteresMensual);
                            sqlComando.Parameters.AddWithValue("@pnTasaAnualAplicada", tasaInteresAnual);
                            sqlComando.Parameters.AddWithValue("@piPlazo", solicitud.PlazoSeleccionado);
                            sqlComando.Parameters.AddWithValue("@pcTipoDePlazo", cotizador.TipoCuota);
                            sqlComando.Parameters.AddWithValue("@pnCuotaMensualPrestamo", cotizador.CuotaDelPrestamo);
                            sqlComando.Parameters.AddWithValue("@pnCuotaMensualGPS", cotizador.CuotaServicioGPS);
                            sqlComando.Parameters.AddWithValue("@pnCuotaMensualSeguro", cotizador.CuotaSegurodeVehiculo);
                            sqlComando.Parameters.AddWithValue("@pnCuotaTotal", cotizador.CuotaTotal);
                            sqlComando.Parameters.AddWithValue("@pnValorTotalContrato", totalAFinanciarConIntereses);
                            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.CommandTimeout = 120;

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }

                        if (contadorErrores > 0)
                        {
                            resultadoProceso.ResultadoExitoso = false;
                            resultadoProceso.MensajeResultado = "Error al guardar informacion de la solicitud, contacte al administrador";
                            return resultadoProceso;
                        }
                    }



                    #region Guardar los documentos de la solicitud (Adjuntados en el formulario de ingreso de solicitud)

                    /* Lista de documentos de la solicitud que van a guardar en la base de datos y se van a mover al nuevo directorio correspondiente de la solicitud */
                    var listaDeDocumentosAdjuntadosPreSolicitudRenombrados = RenombrarListaDocumentos(idSolicitudInsertada, "Solicitud" + idSolicitudInsertada, listaDeDocumentosAdjuntadosYPreSolicitud);

                    /* Guardar los registros de los documentos en la base de datos */
                    foreach (SolicitudesDocumentosViewModel documento in listaDeDocumentosAdjuntadosPreSolicitudRenombrados)
                    {
                        using (var sqlComando = new SqlCommand("sp_CREDSolicitudes_Documentos_Guardar", sqlConexion, sqlTransaction))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitudInsertada);
                            sqlComando.Parameters.AddWithValue("@pcNombreArchivo", documento.fcNombreArchivo);
                            sqlComando.Parameters.AddWithValue("@pcTipoArchivo", (documento.fiTipoDocumento == 8 || documento.fiTipoDocumento == 9) ? "jpg" : ".png");
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
                                    if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            resultadoProceso.ResultadoExitoso = false;
                            resultadoProceso.MensajeResultado = "Error al registrar documentación de la solicitud";
                            return resultadoProceso;
                        }
                    }

                    /* Mover documentos adjuntados renombrados al nuevo directorio de la solicitud */
                    if (!FileUploader.GuardarSolicitudDocumentos(idSolicitudInsertada, listaDeDocumentosAdjuntadosPreSolicitudRenombrados))
                    {
                        resultadoProceso.ResultadoExitoso = false;
                        resultadoProceso.MensajeResultado = "Error al guardar la documentación de la solicitud";
                        return resultadoProceso;
                    }

                    /* Mover documentos adjuntados renombrados al nuevo directorio de la solicitud */
                    if (!MoverDocumentosPreSolicitud(listaDeDocumentosAdjuntadosPreSolicitudRenombrados, listaDeDocumentosPreSolicitud, "Solicitud" + idSolicitudInsertada))
                    {
                        resultadoProceso.ResultadoExitoso = false;
                        resultadoProceso.MensajeResultado = "Error al importar los documentos de la pre solicitud, contacte al administrador.";
                        return resultadoProceso;
                    }

                    #endregion

                    /* Registrar informacion laboral del cliente */
                    using (var sqlComando = new SqlCommand("sp_CREDCliente_InformacionLaboral_Insert", sqlConexion, sqlTransaction))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitudInsertada);
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
                                if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                    contadorErrores++;
                            }
                        }
                    }
                    if (contadorErrores > 0)
                    {
                        resultadoProceso.ResultadoExitoso = false;
                        resultadoProceso.MensajeResultado = "Error al registrar información laboral del cliente";
                        return resultadoProceso;
                    }

                    /* Registrar información del domicilio del cliente */
                    using (var sqlComando = new SqlCommand("sp_CREDCliente_InformacionDomiciliar_Insert", sqlConexion, sqlTransaction))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                        sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitudInsertada);
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
                                if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                    contadorErrores++;
                            }
                        }
                    }
                    if (contadorErrores > 0)
                    {
                        resultadoProceso.ResultadoExitoso = false;
                        resultadoProceso.MensajeResultado = "Error al guardar informacion domiciliar del cliente";
                        return resultadoProceso;
                    }

                    /* Registrar información conyugal del cliente */
                    if (cliente.InformacionConyugal.IndentidadConyugue != null)
                    {
                        using (var sqlComando = new SqlCommand("sp_CREDCliente_InformacionConyugal_Insert", sqlConexion, sqlTransaction))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                            sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitudInsertada);
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
                                    if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            resultadoProceso.ResultadoExitoso = false;
                            resultadoProceso.MensajeResultado = "Error al guardar informacion conyugal del cliente";
                            return resultadoProceso;
                        }
                    }

                    /* Guardar referencias personales del cliente */
                    if (cliente.ListaReferenciasPersonales != null)
                    {
                        if (cliente.ListaReferenciasPersonales.Count != 0)
                        {
                            foreach (Cliente_ReferenciaPersonal_ViewModel referenciaPersonal in cliente.ListaReferenciasPersonales)
                            {
                                using (var sqlComando = new SqlCommand("sp_CREDCliente_Referencias_Insert", sqlConexion, sqlTransaction))
                                {
                                    sqlComando.CommandType = CommandType.StoredProcedure;
                                    sqlComando.Parameters.AddWithValue("@fiIDCliente", solicitud.IdCliente);
                                    sqlComando.Parameters.AddWithValue("@fiIDSolicitud", idSolicitudInsertada);
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
                                            if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                                contadorErrores++;
                                        }
                                    }
                                }
                                if (contadorErrores > 0)
                                {
                                    resultadoProceso.ResultadoExitoso = false;
                                    resultadoProceso.MensajeResultado = "Error al guardar referencias personales del cliente";
                                    return resultadoProceso;
                                }
                            }
                        }
                    }

                    /* Si se requere garantía */
                    if (garantia != null)
                    {
                        using (var sqlComando = new SqlCommand("sp_CREDGarantias_Guardar", sqlConexion, sqlTransaction))
                        {
                            sqlComando.CommandType = CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDCanal", 1);
                            sqlComando.Parameters.AddWithValue("@piIDSolicitud", idSolicitudInsertada);
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
                            sqlComando.Parameters.AddWithValue("@pnValorGarantia", garantia.ValorMercado);
                            sqlComando.Parameters.AddWithValue("@pnValorPrima", garantia.ValorPrima);
                            sqlComando.Parameters.AddWithValue("@pnValorFinanciado", garantia.ValorFinanciado);
                            sqlComando.Parameters.AddWithValue("@pnGastosDeCierre", 0);
                            sqlComando.Parameters.AddWithValue("@pcNombrePropietarioGarantia", garantia.NombrePropietario);
                            sqlComando.Parameters.AddWithValue("@pcIdentidadPropietarioGarantia", garantia.IdentidadPropietario);
                            sqlComando.Parameters.AddWithValue("@piIDNacionalidadPropietarioGarantia", garantia.IdNacionalidadPropietario);
                            sqlComando.Parameters.AddWithValue("@piIDEstadoCivilPropietarioGarantia", garantia.IdEstadoCivilPropietario);
                            sqlComando.Parameters.AddWithValue("@pcNombreVendedorGarantia", garantia.NombreVendedor);
                            sqlComando.Parameters.AddWithValue("@pcIdentidadVendedorGarantia", garantia.IdentidadVendedor);
                            sqlComando.Parameters.AddWithValue("@piIDNacionalidadVendedorGarantia", garantia.IdNacionalidadVendedor);
                            sqlComando.Parameters.AddWithValue("@piIDEstadoCivilVendedorGarantia", garantia.IdEstadoCivilVendedor);
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
                                    if (sqlResultado["MensajeError"].ToString().StartsWith("-1"))
                                        contadorErrores++;
                                }
                            }
                        }
                        if (contadorErrores > 0)
                        {
                            resultadoProceso.ResultadoExitoso = false;
                            resultadoProceso.MensajeResultado = "No se pudo guardar la información de la garantía, contacte al administrador";
                            return resultadoProceso;
                        }
                    }

                    sqlTransaction.Commit();
                    resultadoProceso.IdInsertado = 0;
                    resultadoProceso.ResultadoExitoso = true;
                    resultadoProceso.MensajeResultado = "¡La solicitud ha sido ingresada exitosamente!";
                }
                catch (Exception ex)
                {
                    sqlTransaction.Rollback();
                    ex.Message.ToString();
                    resultadoProceso.ResultadoExitoso = false;
                    resultadoProceso.MensajeResultado = "Error al guardar solicitud, contacte al administrador";
                }
            }
        }
        return resultadoProceso;
    }

    #endregion

    #region FUNCIONES VARIAS - UTILITARIAS

    public static SqlCommand CrearSqlComando(string nombreSP, SqlConnection sqlConexion)
    {
        return new SqlCommand(nombreSP, sqlConexion) { CommandType = CommandType.StoredProcedure, CommandTimeout = 120 };
    }

    private static decimal CalcularTotalAFinanciarConIntereses(decimal totalAFinanciar, int plazoSeleccionado, decimal tasaInteresAnual, int idProducto)
    {
        decimal interesAnual = tasaInteresAnual > 1 ? (totalAFinanciar * tasaInteresAnual) / 100 : totalAFinanciar * tasaInteresAnual;

        var tipoDePlazo = (idProducto == 202 || idProducto == 203 || idProducto == 204) ? 12 : 24; /* Determinar si el plazo es mensual o quincenal para calcular el interés anual */

        var interesesTotal = interesAnual * (plazoSeleccionado / tipoDePlazo);

        return totalAFinanciar + interesesTotal;
    }

    private static decimal ObtenerTasaInteresAnualPorIdProducto(int idProducto)
    {
        decimal tasaInteresAnual = 0;
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CatalogoProductos", sqlConexion))
                {
                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            if (idProducto == (int)sqlResultado["fiIDProducto"])
                                tasaInteresAnual = (decimal)sqlResultado["fnTasadeInteres"];

                        } // while sqlResultado.Read()
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }

        return tasaInteresAnual;
    }

    private static DateTime ObtenerFechaPrimerPago(string idProducto)
    {
        DateTime fechaDelPrimerPago;

        var hoy = DateTime.Now;
        int diaPrimerPago = hoy.Day;
        int mesPrimerPago = hoy.Month;
        int anioPrimerPago = hoy.Year;

        /* ================================================== */
        /* ================= Quincenalmente ================ */
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
        /* ======================================================================= */
        /* ================== Mensualmente ======================================= */
        /* ========= Del 06 - 20 = 15 del siguiente mes ========================== */
        /* ========= Del 21 - 05 = 30 del mes en curso o del proximo mes ========= */
        /* ======================================================================= */
        else if (idProducto == "202" || idProducto == "203" || idProducto == "204")
        {
            if (hoy.Day >= 21 || hoy.Day <= 5)
            {
                if (hoy.Day >= 21)
                {
                    mesPrimerPago = hoy.AddMonths(1).Month;
                    anioPrimerPago = hoy.AddMonths(1).Year;
                }
                var ultimoDiaDelMes = new DateTime(anioPrimerPago, mesPrimerPago, DateTime.DaysInMonth(anioPrimerPago, mesPrimerPago)).Day; // ultimo dia del mes
                diaPrimerPago = ultimoDiaDelMes > 30 ? 30 : ultimoDiaDelMes;

            }
            else if (hoy.Day >= 6 || hoy.Day <= 20)
            {
                mesPrimerPago = hoy.AddMonths(1).Month;
                anioPrimerPago = hoy.AddMonths(1).Year;
                diaPrimerPago = 15;
            }
        }
        fechaDelPrimerPago = new DateTime(anioPrimerPago, mesPrimerPago, diaPrimerPago);

        return fechaDelPrimerPago;
    }

    public static List<SolicitudesDocumentosViewModel> RenombrarListaDocumentos(int idMaestro, string nombreCarpetaDestino, List<SolicitudesDocumentosViewModel> listaDocumentos)
    {
        var listaDeDocumentosDeLaSolicitud = new List<SolicitudesDocumentosViewModel>();

        var documentacionCliente = 1;
        var nuevoNombreDocumento = string.Empty;
        var rutaDocumento = string.Empty;

        /* lista de bloques y la cantidad de documentos que contiene cada uno */
        var listaDeBloquesDeTiposDeDocumentos = listaDocumentos.GroupBy(documento => documento.fiTipoDocumento).Select(x => new { x.Key, Count = x.Count() });

        /* lista donde se guardara temporalmente los documentos adjuntados por el usuario dependiendo del tipo de documento en el iterador */
        var documentosBloque = new List<SolicitudesDocumentosViewModel>();

        /* renombrar los documentos y determinar nuevo directorio */
        foreach (var bloqueDeDocumentos in listaDeBloquesDeTiposDeDocumentos)
        {
            int tipoDocumento = (int)bloqueDeDocumentos.Key;
            int cantidadDocumentos = bloqueDeDocumentos.Count;

            documentosBloque = listaDocumentos.Where(x => x.fiTipoDocumento == tipoDocumento).ToList();// documentos de este bloque
            var nombresGeneradosParaBloqueActual = Funciones.MultiNombres.GenerarNombreCredDocumento(documentacionCliente, idMaestro, tipoDocumento, cantidadDocumentos);

            int contadorNombre = 0;
            foreach (SolicitudesDocumentosViewModel documento in documentosBloque)
            {
                if (documento.fcRutaArchivo.Contains(".jpg") || documento.fcRutaArchivo.Contains(".png") || documento.fcRutaArchivo.Contains(".jpeg"))
                {
                    rutaDocumento = documento.fcRutaArchivo;
                }
                else
                {
                    rutaDocumento = documento.fcRutaArchivo + @"\" + ((documento.NombreAntiguo.Contains(".jpg") || documento.NombreAntiguo.Contains(".png") || documento.NombreAntiguo.Contains(".jpeg")) ? documento.NombreAntiguo : documento.NombreAntiguo + (documento.fcTipoArchivo.Contains(".") ? documento.fcTipoArchivo : "." + documento.fcTipoArchivo));
                }

                if (File.Exists(rutaDocumento)) /* si el archivo existe, que se agregue a la lista */
                {
                    nuevoNombreDocumento = nombresGeneradosParaBloqueActual[contadorNombre];
                    listaDeDocumentosDeLaSolicitud.Add(new SolicitudesDocumentosViewModel()
                    {
                        fcNombreArchivo = nuevoNombreDocumento,
                        NombreAntiguo = documento.NombreAntiguo,
                        fcTipoArchivo = documento.fcTipoArchivo,
                        fcRutaArchivo = @"C:\inetpub\wwwroot\Documentos\Solicitudes\" + nombreCarpetaDestino + (documento.fiTipoDocumento == 8 || documento.fiTipoDocumento == 9 ? (@"\" + nuevoNombreDocumento + "." + documento.fcTipoArchivo) : ""),
                        URLArchivo = "/Documentos/Solicitudes/" + nombreCarpetaDestino + "/" + nuevoNombreDocumento + (documento.fiTipoDocumento == 8 || documento.fiTipoDocumento == 9 ? "" : ".png"),
                        fiTipoDocumento = documento.fiTipoDocumento
                    });
                    contadorNombre++;
                }
            }
        }
        return listaDeDocumentosDeLaSolicitud;
    }

    public static bool MoverDocumentosPreSolicitud(List<SolicitudesDocumentosViewModel> ListaDocumentosRenombrados, List<SolicitudesDocumentosViewModel> ListaDocumentosPreSolicitud, string nombreCarpetaDestino)
    {
        bool resultado;
        try
        {
            if (ListaDocumentosPreSolicitud != null)
            {
                if (ListaDocumentosPreSolicitud.Count > 0)
                {
                    var directorioViejo = string.Empty;
                    var directorioDestino = @"C:\inetpub\wwwroot\Documentos\Solicitudes\" + nombreCarpetaDestino + "\\";
                    var nuevoNombreDocumento = string.Empty;
                    var nuevoDirectorioDestinoCompleto = string.Empty;

                    if (!Directory.Exists(directorioDestino))
                        Directory.CreateDirectory(directorioDestino);

                    foreach (SolicitudesDocumentosViewModel documentoPreSolicitud in ListaDocumentosPreSolicitud)
                    {
                        directorioViejo = (documentoPreSolicitud.fiTipoDocumento == 8 || documentoPreSolicitud.fiTipoDocumento == 9) ? documentoPreSolicitud.fcRutaArchivo : documentoPreSolicitud.fcRutaArchivo + @"\" + documentoPreSolicitud.fcNombreArchivo + documentoPreSolicitud.fcTipoArchivo;
                        nuevoNombreDocumento = ListaDocumentosRenombrados.Where(x => x.NombreAntiguo == documentoPreSolicitud.fcNombreArchivo).Select(x => x.fcNombreArchivo).FirstOrDefault();
                        nuevoDirectorioDestinoCompleto = directorioDestino + nuevoNombreDocumento + ((documentoPreSolicitud.fiTipoDocumento == 8 || documentoPreSolicitud.fiTipoDocumento == 9) ? ".jpg" : ".png");

                        if (File.Exists(directorioViejo))
                            File.Copy(directorioViejo, nuevoDirectorioDestinoCompleto);
                    }
                }
            }
            resultado = true;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            resultado = false;
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

    #endregion

    #region ENVIAR CORREO de solicitud de cambio de Score

    [WebMethod]
    public static bool EnviarSolicitudCambioScore(string identidadCliente, string nombreCliente, string scoreActual, string nuevoScore, string comentarioAdicional, string dataCrypt)
    {
        var resultado = false;
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            var nombreUsuario = string.Empty;
            var buzonCorreoUsuario = string.Empty;

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("CoreSeguridad.dbo.sp_InformacionUsuario", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            buzonCorreoUsuario = sqlResultado["fcBuzondeCorreo"].ToString();
                            nombreUsuario = sqlResultado["fcNombreCorto"].ToString();
                        }
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion

            var contenidoCorreo = "<table style=\"width: 500px; border-collapse: collapse; border-width: 0; border-style: none; border-spacing: 0; padding: 0;\">" +
            "<tr><th style='text-align:left;'>Identidad:</th> <td>" + identidadCliente + "</td></tr>" +
            "<tr><th style='text-align:left;'>Nombre:</th> <td>" + nombreCliente + "</td></tr>" +
            "<tr><th style='text-align:left;'>Score actual:</th> <td>" + scoreActual + "</td></tr>" +
            "<tr><th style='text-align:left;'>Score solicitado:</th> <td>" + nuevoScore + "</td></tr>" +
            "<tr><th style='text-align:left;'>Solicitado por:</th> <td>" + nombreUsuario + "</td></tr>" +
            "<tr><th style='text-align:left;'>Comentario:</th> <td>" + comentarioAdicional + "</td></tr>" +
            "</table>";

            resultado = EnviarCorreo("Solicitud de cambio de SCORE", "Cambio de score", "Datos de la solicitud", contenidoCorreo, buzonCorreoUsuario);
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
        return resultado;
    }

    public static bool EnviarCorreo(string pcAsunto, string pcTituloGeneral, string pcSubtitulo, string pcContenidodelMensaje, string buzonCorreoUsuario)
    {
        var resultado = false;
        try
        {
            using (var smtpCliente = new SmtpClient("mail.miprestadito.com", 587))
            {
                smtpCliente.Credentials = new System.Net.NetworkCredential("systembot@miprestadito.com", "iPwf@p3q");
                smtpCliente.EnableSsl = true;

                using (var pmmMensaje = new MailMessage())
                {
                    pmmMensaje.Subject = pcAsunto;
                    pmmMensaje.From = new MailAddress("systembot@miprestadito.com", "System Bot");
                    pmmMensaje.To.Add("sistemas@miprestadito.com");
                    pmmMensaje.CC.Add(buzonCorreoUsuario);
                    pmmMensaje.CC.Add("edwin.aguilar@miprestadito.com");
                    pmmMensaje.CC.Add("keyla.hernandez@miprestadito.com");
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
                    " <td style=\"background-color:whitesmoke; text-align:center;\">" + pcSubtitulo + "</td>" +
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
                    resultado = true;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionLogging.SendExcepToDB(ex);
            resultado = false;
        }

        return resultado;
    }
    #endregion

    #region P R E C I O S D E M E R C A D O

    [WebMethod]
    public static List<EntidadGenerica_ViewModel> CargarMarcas(string dataCrypt)
    {
        var marcas = new List<EntidadGenerica_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Catalogo_Marcas_Listar", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDMarca", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            marcas.Add(new EntidadGenerica_ViewModel()
                            {
                                Id = (int)sqlResultado["fiIDMarca"],
                                Descripcion = sqlResultado["fcMarca"].ToString(),
                            });
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            marcas = null;
        }
        return marcas;
    }

    [WebMethod]
    public static List<EntidadGenerica_ViewModel> CargarModelosPorIdMarca(string idMarca, string dataCrypt)
    {
        var marcas = new List<EntidadGenerica_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Catalogo_Modelos_ObtenerPorIdMarca", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDMarca", idMarca);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            marcas.Add(new EntidadGenerica_ViewModel()
                            {
                                Id = (int)sqlResultado["fiIDModelo"],
                                Descripcion = sqlResultado["fcModelo"].ToString() + " " + sqlResultado["fcVersion"].ToString(),
                            });
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            marcas = null;
        }
        return marcas;
    }

    [WebMethod]
    public static List<EntidadGenerica_ViewModel> CargarCatalogoDeAnios(string dataCrypt)
    {
        var marcas = new List<EntidadGenerica_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Catalogo_Anios_Listar", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDAnio", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            marcas.Add(new EntidadGenerica_ViewModel()
                            {
                                Id = (int)sqlResultado["fiIDAnio"],
                                Descripcion = sqlResultado["fiAnio"].ToString(),
                            });
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            marcas = null;
        }
        return marcas;
    }

    [WebMethod]
    public static List<EntidadGenerica_ViewModel> CargarAniosDisponiblesPorIdModelo(string idModelo, string dataCrypt)
    {
        var marcas = new List<EntidadGenerica_ViewModel>();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Catalogo_Modelos_Anios_ObtenerPorIdModelo", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDModelo", idModelo);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            marcas.Add(new EntidadGenerica_ViewModel()
                            {
                                Id = (int)sqlResultado["fiIDModeloAnio"],
                                Descripcion = sqlResultado["fiAnio"].ToString(),
                            });
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            marcas = null;
        }
        return marcas;
    }

    [WebMethod]
    public static PrecioDeMercado_ViewModel CargarPrecioDeMercadoPorIdModeloAnio(string idModeloAnio, string dataCrypt)
    {
        var precioDeMercadoActual = new PrecioDeMercado_ViewModel();
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ToString())))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Maestro_ObtenerPrecioActualPorIdModeloAnio", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDModeloAnio", idModeloAnio);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            precioDeMercadoActual = new PrecioDeMercado_ViewModel()
                            {
                                IdPrecioDeMercado = (int)sqlResultado["fiIDPrecioDeMercado"],
                                PrecioDeMercado = (decimal)sqlResultado["fnPrecioDeMercado"],
                                IdEstado = (int)sqlResultado["fiIDEstadoPrecioDeMercado"],
                                Estado = sqlResultado["fcEstadoPrecioDeMercado"].ToString(),
                                EstadoClassName = sqlResultado["fcEstadoPrecioDeMercadoClassName"].ToString()
                            };
                        }
                    }
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            precioDeMercadoActual = null;
        }
        return precioDeMercadoActual;
    }

    [WebMethod]
    public static Resultado_ViewModel GuardarMarca(string marca, string dataCrypt)
    {
        var Resultado = new Resultado_ViewModel() { ResultadoExitoso = false };
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Catalogo_Marcas_Guardar", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@pcMarca", marca.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            var resultadoSP = sqlResultado["MensajeError"].ToString();

                            if (!resultadoSP.StartsWith("-1"))
                            {
                                Resultado.ResultadoExitoso = true;
                                Resultado.MensajeResultado = "La marca se registró existosamente";
                                Resultado.IdInsertado = int.Parse(resultadoSP);
                            }
                            else
                            {
                                Resultado.ResultadoExitoso = false;
                                Resultado.MensajeResultado = "No se pudo guardar la marca, contacte al administrador.";
                                Resultado.MensajeDebug = resultadoSP;

                                if (resultadoSP.Contains("Violation of UNIQUE KEY"))
                                    Resultado.MensajeResultado = "La marca que intenta guardar ya ha sido registrada. Si la marca ha sido INACTIVADA, cambie el estado a ACTIVA. Si cree que se trata de un error, contacte al administrador.";
                            }
                        } // using sqlResultado.Read()
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            Resultado.ResultadoExitoso = false;
            Resultado.MensajeResultado = "Ocurrió un error al guardar la marca, contacte al administrador.";
            Resultado.MensajeDebug = ex.Message.ToString();
        }
        return Resultado;
    }

    [WebMethod]
    public static Resultado_ViewModel GuardarModelo(int idMarca, string modelo, string version, string dataCrypt)
    {
        var Resultado = new Resultado_ViewModel() { ResultadoExitoso = false };
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Catalogo_Modelos_Guardar", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDMarca", idMarca);
                    sqlComando.Parameters.AddWithValue("@pcModelo", modelo.Trim());
                    sqlComando.Parameters.AddWithValue("@pcVersion", version.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            var resultadoSP = sqlResultado["MensajeError"].ToString();

                            if (!resultadoSP.StartsWith("-1"))
                            {
                                Resultado.ResultadoExitoso = true;
                                Resultado.MensajeResultado = "El modelo se registró existosamente";
                                Resultado.IdInsertado = int.Parse(resultadoSP);
                            }
                            else
                            {
                                Resultado.ResultadoExitoso = false;
                                Resultado.MensajeResultado = "No se pudo guardar el modelo, contacte al administrador.";
                                Resultado.MensajeDebug = resultadoSP;

                                if (resultadoSP.Contains("Violation of UNIQUE KEY"))
                                    Resultado.MensajeResultado = "El modelo que intenta guardar ya ha sido registrado. Si el modelo ha sido INACTIVADA, cambie el estado a ACTIVA. Si cree que se trata de un error, contacte al administrador.";
                            }
                        } // using sqlResultado.Read()
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            Resultado.ResultadoExitoso = false;
            Resultado.MensajeResultado = "Ocurrió un error al guardar el modelo, contacte al administrador.";
            Resultado.MensajeDebug = ex.Message.ToString();
        }
        return Resultado;
    }

    [WebMethod]
    public static Resultado_ViewModel GuardarModeloAnio(int idModelo, int idAnio, string dataCrypt)
    {
        var Resultado = new Resultado_ViewModel() { ResultadoExitoso = false };
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Catalogo_Modelos_Anios_Guardar", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDModelo", idModelo);
                    sqlComando.Parameters.AddWithValue("@piIDAnio", idAnio);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            var resultadoSP = sqlResultado["MensajeError"].ToString();

                            if (!resultadoSP.StartsWith("-1"))
                            {
                                Resultado.ResultadoExitoso = true;
                                Resultado.MensajeResultado = "El año del modelo seleccionado se registró existosamente";
                                Resultado.IdInsertado = int.Parse(resultadoSP);
                            }
                            else
                            {
                                Resultado.ResultadoExitoso = false;
                                Resultado.MensajeResultado = "No se pudo guardar el año del modelo seleccionado, contacte al administrador.";
                                Resultado.MensajeDebug = resultadoSP;

                                if (resultadoSP.Contains("Violation of UNIQUE KEY"))
                                    Resultado.MensajeResultado = "El año del modelo seleccionado que intenta guardar ya ha sido registrado. Si el año del modelo seleccionado ha sido INACTIVADO, cambie el estado a ACTIVO. Si cree que se trata de un error, contacte al administrador.";
                            }
                        } // using sqlResultado.Read()
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            Resultado.ResultadoExitoso = false;
            Resultado.MensajeResultado = "Ocurrió un error al guardar el año del modelo seleccionado, contacte al administrador.";
            Resultado.MensajeDebug = ex.Message.ToString();
        }
        return Resultado;
    }

    [WebMethod]
    public static Resultado_ViewModel SolicitarPrecioDeMercado(int idModeloAnio, decimal precioSolicitado, string comentarios, string dataCrypt)
    {
        var Resultado = new Resultado_ViewModel() { ResultadoExitoso = false };
        try
        {
            var lURLDesencriptado = DesencriptarURL(dataCrypt);
            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = CrearSqlComando("sp_CREDPreciosDeMercado_Maestro_Guardar", sqlConexion))
                {
                    sqlComando.Parameters.AddWithValue("@piIDModeloAnio", idModeloAnio);
                    sqlComando.Parameters.AddWithValue("@pnPrecioDeMercado", precioSolicitado);
                    sqlComando.Parameters.AddWithValue("@piIDUsuarioSolicitante", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcComentariosSolicitante", comentarios);
                    sqlComando.Parameters.AddWithValue("@piIDUsuarioAprobador", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@pcComentariosAprobador", DBNull.Value);
                    sqlComando.Parameters.AddWithValue("@piIDEstadoPrecioDeMercado", 0);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            var resultadoSP = sqlResultado["lcResultadoProceso"].ToString();

                            if (!resultadoSP.StartsWith("-1"))
                            {
                                Resultado.ResultadoExitoso = true;
                                Resultado.IdInsertado = int.Parse(resultadoSP);
                            }
                            else
                            {
                                Resultado.ResultadoExitoso = false;
                                Resultado.MensajeDebug = resultadoSP;
                            }

                            Resultado.MensajeResultado = sqlResultado["lcMensajeValidaciones"].ToString();

                        } // using sqlResultado.Read()
                    } // using sqlResultado
                } // using sqlComando
            } // using sqlConexion
        }
        catch (Exception ex)
        {
            Resultado.ResultadoExitoso = false;
            Resultado.MensajeResultado = "Ocurrió un error al solicitar el precio de mercado, contacte al administrador.";
            Resultado.MensajeDebug = ex.Message.ToString();
        }
        return Resultado;
    }

    #endregion

    #region V I E W M O D E L S

    public class Resultado_ViewModel
    {
        public int IdInsertado { get; set; }
        public bool ResultadoExitoso { get; set; }
        public string MensajeResultado { get; set; }
        public string MensajeDebug { get; set; }
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
        public string IdentidadPropietario { get; set; }
        public string NombrePropietario { get; set; }
        public int IdNacionalidadPropietario { get; set; }
        public int IdEstadoCivilPropietario { get; set; }
        public string IdentidadVendedor { get; set; }
        public string NombreVendedor { get; set; }
        public int IdNacionalidadVendedor { get; set; }
        public int IdEstadoCivilVendedor { get; set; }
    }

    public class EntidadGenerica_ViewModel
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class PrecioDeMercado_ViewModel
    {
        public int IdPrecioDeMercado { get; set; }
        public int IdModeloAnio { get; set; }
        public decimal PrecioDeMercado { get; set; }
        public int IdEstado { get; set; }
        public string Estado { get; set; }
        public string EstadoClassName { get; set; }
    }

    #endregion
}