using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PreSolicitud_Guardar : System.Web.UI.Page
{
    private static string pcIDUsuario = "";
    private static string pcIDApp = "";
    private static string pcIDSesion = "";
    public static string pcID = "";
    private static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                /* Captura de parametros y desencriptado de cadena */
                var lcURL = Request.Url.ToString();
                int liParamStart = lcURL.IndexOf("?");
                pcID = "";

                string lcParametros;
                if (liParamStart > 0)
                {
                    lcParametros = lcURL.Substring(liParamStart, lcURL.Length - liParamStart);
                }
                else
                {
                    lcParametros = String.Empty;
                }

                if (lcParametros != String.Empty)
                {
                    var lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcEncriptado = lcEncriptado.Replace("%2f", "/");
                    var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr") ?? "0";
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp") ?? "0";
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "0";
                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID") ?? "";

                    if (!string.IsNullOrWhiteSpace(pcID))
                    {
                        CargarPrecalificado(pcID);
                    }

                    LlenarListas();
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error al cargar la información: " + ex.Message.ToString());
            }
        }
    }

    /* Llenar listas desplegables */
    protected void LlenarListas()
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("sp_GeoDepartamento", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piPais", 1);
                    sqlComando.Parameters.AddWithValue("@piDepartamento", 0);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        // catalogo de marcas
                        ddlMunicipio.Items.Clear();
                        ddlMunicipio.Items.Add(new ListItem("Seleccione un departamento", "0"));
                        ddlMunicipio.Enabled = false;

                        ddlCiudad.Items.Clear();
                        ddlCiudad.Items.Add(new ListItem("Seleccione un municipio", "0"));
                        ddlCiudad.Enabled = false;

                        ddlBarrioColonia.Items.Clear();
                        ddlBarrioColonia.Items.Add(new ListItem("Seleccione una ciudad", "0"));
                        ddlBarrioColonia.Enabled = false;

                        ddlDepartamento.Items.Clear();
                        ddlDepartamento.Items.Add(new ListItem("Seleccionar", "0"));
                        while (sqlResultado.Read())
                        {
                            ddlDepartamento.Items.Add(new ListItem(sqlResultado["fcDepartamento"].ToString(), sqlResultado["fiCodDepartamento"].ToString()));
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar información del formulario de negociación: " + ex.Message.ToString());
        }
    }

    /* Cargar informacion del precalificado del cliente o redirigirlo a precalificar si todavia no lo esta */
    private void CargarPrecalificado(string pcID)
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (!sqlResultado.HasRows)
                        {
                            string lcScript = "window.open('precalificado_buscador.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
                            Response.Write("<script>");
                            Response.Write(lcScript);
                            Response.Write("</script>");
                        }

                        while (sqlResultado.Read())
                        {
                            txtIdentidadCliente.Text = sqlResultado["fcIdentidad"].ToString();
                            txtNombreCliente.Text = sqlResultado["fcPrimerNombre"].ToString() + " " + sqlResultado["fcSegundoNombre"].ToString() + " " + sqlResultado["fcPrimerApellido"].ToString() + " " + sqlResultado["fcSegundoApellido"].ToString();
                            txtTelefonoCliente.Text = sqlResultado["fcTelefono"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar precalificado " + pcID + ": " + ex.Message.ToString());
        }
    }

    protected void MostrarMensaje(string mensaje)
    {
        PanelMensajeErrores.Visible = true;
        lblMensaje.Visible = true;
        lblMensaje.Text = mensaje;
    }

    protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var idDepartamento = ddlDepartamento.SelectedValue;

            if (idDepartamento != "0" && !string.IsNullOrWhiteSpace(idDepartamento))
            {
                using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    sqlConexion.Open();

                    using (var sqlComando = new SqlCommand("sp_GeoMunicipio", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piPais", 1);
                        sqlComando.Parameters.AddWithValue("@piDepartamento", idDepartamento);
                        sqlComando.Parameters.AddWithValue("@piMunicipio", 0);
                        sqlComando.CommandTimeout = 120;

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            ddlMunicipio.Items.Clear();
                            ddlMunicipio.Items.Add(new ListItem("Seleccionar", "0"));
                            ddlMunicipio.Enabled = true;

                            ddlCiudad.Items.Clear();
                            ddlCiudad.Items.Add(new ListItem("Seleccione un municipio", "0"));
                            ddlCiudad.Enabled = false;

                            ddlBarrioColonia.Items.Clear();
                            ddlBarrioColonia.Items.Add(new ListItem("Seleccione una ciudad", "0"));
                            ddlBarrioColonia.Enabled = false;

                            while (sqlResultado.Read())
                            {
                                ddlMunicipio.Items.Add(new ListItem(sqlResultado["fcMunicipio"].ToString(), sqlResultado["fiCodMunicipio"].ToString()));
                            }
                            ddlMunicipio.Focus();
                        }
                    }
                }
            }
            else
            {
                ddlMunicipio.Items.Clear();
                ddlMunicipio.Items.Add(new ListItem("Seleccione un departamento", "0"));
                ddlMunicipio.Enabled = false;

                ddlCiudad.Items.Clear();
                ddlCiudad.Items.Add(new ListItem("Seleccione un municipio", "0"));
                ddlCiudad.Enabled = false;

                ddlBarrioColonia.Items.Clear();
                ddlBarrioColonia.Items.Add(new ListItem("Seleccione una ciudad", "0"));
                ddlBarrioColonia.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar municipios del departamento seleccionado: " + ex.Message.ToString());
        }
    }

    protected void ddlMunicipio_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var idDepartamento = ddlDepartamento.SelectedValue;
            var idMunicipio = ddlMunicipio.SelectedValue;

            if ((idMunicipio != "0" && !string.IsNullOrWhiteSpace(idMunicipio)) && (idDepartamento != "0" && !string.IsNullOrWhiteSpace(idDepartamento)))
            {
                using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    sqlConexion.Open();

                    using (var sqlComando = new SqlCommand("sp_GeoPoblado", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piPais", 1);
                        sqlComando.Parameters.AddWithValue("@piDepartamento", idDepartamento);
                        sqlComando.Parameters.AddWithValue("@piMunicipio", idMunicipio);
                        sqlComando.Parameters.AddWithValue("@piPoblado", 0);
                        sqlComando.CommandTimeout = 120;

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            ddlCiudad.Items.Clear();
                            ddlCiudad.Items.Add(new ListItem("Seleccionar", "0"));
                            ddlCiudad.Enabled = true;

                            ddlBarrioColonia.Items.Clear();
                            ddlBarrioColonia.Items.Add(new ListItem("Seleccione una ciudad", "0"));
                            ddlBarrioColonia.Enabled = false;

                            while (sqlResultado.Read())
                            {
                                ddlCiudad.Items.Add(new ListItem(sqlResultado["fcPoblado"].ToString(), sqlResultado["fiCodPoblado"].ToString()));
                            }
                            ddlCiudad.Focus();
                        }
                    }
                }
            }
            else
            {
                ddlCiudad.Items.Clear();
                ddlCiudad.Items.Add(new ListItem("Seleccione un municipio", "0"));
                ddlCiudad.Enabled = false;

                ddlBarrioColonia.Items.Clear();
                ddlBarrioColonia.Items.Add(new ListItem("Seleccione una ciudad", "0"));
                ddlBarrioColonia.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar ciudades/poblado del municipio seleccionado: " + ex.Message.ToString());
        }
    }

    protected void ddlCiudad_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            var idDepartamento = ddlDepartamento.SelectedValue;
            var idMunicipio = ddlMunicipio.SelectedValue;
            var idCiudad = ddlCiudad.SelectedValue;

            if ((idCiudad != "0" && !string.IsNullOrWhiteSpace(idCiudad)) && (idMunicipio != "0" && !string.IsNullOrWhiteSpace(idMunicipio)) && (idDepartamento != "0" && !string.IsNullOrWhiteSpace(idDepartamento)))
            {
                using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    sqlConexion.Open();

                    using (var sqlComando = new SqlCommand("sp_GeoBarrios", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piPais", 1);
                        sqlComando.Parameters.AddWithValue("@piDepartamento", idDepartamento);
                        sqlComando.Parameters.AddWithValue("@piMunicipio", idMunicipio);
                        sqlComando.Parameters.AddWithValue("@piPoblado", idCiudad);
                        sqlComando.Parameters.AddWithValue("@piBarrio", 0);
                        sqlComando.CommandTimeout = 120;

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            ddlBarrioColonia.Items.Clear();
                            ddlBarrioColonia.Items.Add(new ListItem("Seleccionar", "0"));
                            ddlBarrioColonia.Enabled = true;

                            while (sqlResultado.Read())
                            {
                                ddlBarrioColonia.Items.Add(new ListItem(sqlResultado["fcBarrioColonia"].ToString(), sqlResultado["fiCodBarrio"].ToString()));
                            }
                            ddlBarrioColonia.Focus();
                        }
                    }
                }
            }
            else
            {
                ddlBarrioColonia.Items.Clear();
                ddlBarrioColonia.Items.Add(new ListItem("Seleccione una ciudad", "0"));
                ddlBarrioColonia.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Error al cargar barrios/colonias de la ciudad/poblado seleccionado: " + ex.Message.ToString());
        }
    }

    protected void btnGuardarPreSolicitud_Click(object sender, EventArgs e)
    {
        if (Validaciones())
        {
            try
            {
                using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    sqlConexion.Open();

                    using (var sqlComando = new SqlCommand("sp_CREDPreSolicitudes_Maestro_Guardar", sqlConexion))
                    {
                        sqlComando.CommandType = CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDPais", 1);
                        sqlComando.Parameters.AddWithValue("@piIDCanal", 1);
                        sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                        sqlComando.Parameters.AddWithValue("@pcTelefonoCasa", txtTelefonoCasa.Text.Trim().Replace("_","").Replace("-",""));
                        sqlComando.Parameters.AddWithValue("@piIDDepartamento", ddlDepartamento.SelectedValue);
                        sqlComando.Parameters.AddWithValue("@piIDMunicipio", ddlMunicipio.SelectedValue);
                        sqlComando.Parameters.AddWithValue("@piIDCiudad", ddlCiudad.SelectedValue);
                        sqlComando.Parameters.AddWithValue("@piIDBarrioColonia", ddlBarrioColonia.SelectedValue);
                        sqlComando.Parameters.AddWithValue("@pcDireccionDetallada", txtDireccionDetallada.Text.Trim());
                        sqlComando.Parameters.AddWithValue("@pcReferenciasDireccionDetallada", txtReferenciasDomicilio.Value.Trim());
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
                                    string lcScript = "window.open('PreSolicitud_Listado.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario) + "','_self')";
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", lcScript, true);
                                }
                                else
                                {
                                    MostrarMensaje("No se pudo guardar la pre-solicitud: " + sqlResultado["MensajeError"].ToString());
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Error al cargar precalificado " + pcID + ": " + ex.Message.ToString());
            }
        }
    }

    private bool Validaciones()
    {
        try
        {
            var idDepartamento = ddlDepartamento.SelectedValue;
            var idMunicipio = ddlMunicipio.SelectedValue;
            var idCiudad = ddlCiudad.SelectedValue;
            var idBarrio = ddlBarrioColonia.SelectedValue;

            if (string.IsNullOrEmpty(txtIdentidadCliente.Text) || (txtIdentidadCliente.Text.Length < 13))
            {
                MostrarMensaje("El número de identidad del cliente no es válido.");
                return false;
            }

            if (string.IsNullOrEmpty(txtTelefonoCliente.Text))
            {
                MostrarMensaje("El número de teléfono del cliente no es válido.");
                return false;
            }

            if (idDepartamento == "0" && string.IsNullOrWhiteSpace(idDepartamento))
            {
                MostrarMensaje("Seleccione un departamento.");
                return false;
            }

            if (idMunicipio == "0" && string.IsNullOrWhiteSpace(idMunicipio))
            {
                MostrarMensaje("Seleccione un municipio.");
                return false;
            }

            if (idCiudad == "0" && string.IsNullOrWhiteSpace(idCiudad))
            {
                MostrarMensaje("Seleccione una ciudad.");
                return false;
            }

            if (idBarrio == "0" && string.IsNullOrWhiteSpace(idBarrio))
            {
                MostrarMensaje("Seleccione un barrio/colonia.");
                return false;
            }

            if (string.IsNullOrEmpty(txtDireccionDetallada.Text) || (txtIdentidadCliente.Text.Length < 10))
            {
                MostrarMensaje("La dirección detallada debe tener 10 o más caracteres.");
                return false;
            }

            if (string.IsNullOrEmpty(txtReferenciasDomicilio.Value) || (txtReferenciasDomicilio.Value.Length < 15))
            {
                MostrarMensaje("La dirección detallada debe tener 15 o más caracteres.");
                return false;
            }

            return true;
        }
        catch (Exception ex)
        {
            MostrarMensaje("Ocurrió un error al validar la información: " + ex.Message.ToString());
            return false;
        }
    }
}