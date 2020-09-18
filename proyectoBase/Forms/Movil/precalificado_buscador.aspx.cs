using System;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Text;

public partial class Creditos_precalificado : System.Web.UI.Page
{
    private string pcIDSesion = "";
    private string pcIDUsuario = "";
    private string pcIDApp = "";
    private string pcIdentidad = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        string lcURL;
        int liParamStart;
        string lcParametros;
        string lcEncriptado;
        string lcParametroDesencriptado;
        Uri lURLDesencriptado;

        lcURL = Request.Url.ToString();
        liParamStart = lcURL.IndexOf("?");

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
            lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
            lcEncriptado = lcEncriptado.Replace("%2f", "/");
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID") ?? "1";
            pcIdentidad = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");
        }
        /* FIN de captura de parametros y desencriptado de cadena */

        if (ddlCiudadResidencia.Items.Count == 0)
        {
            LlenarListas();
            ddlOrigenIngreso.Items.Insert(0, new ListItem("Seleccionar origen", ""));
            ddlOrigenIngreso.SelectedIndex = 0;
            ddlOrigenIngreso.Items.Add("Asalariado");
            ddlOrigenIngreso.Items.Add("Comerciante");
        }

        if (!string.IsNullOrEmpty(pcIdentidad))
        {
            txtNombreCliente.Visible = true;

            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_ConsultaRNP", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fcNumeroIdentidad", pcIdentidad);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        if (sqlResultado.Read())
                        {
                            txtNombreCliente.Text = sqlResultado["fcNombreCompleto"].ToString();
                            txtIdentidad.Text = pcIdentidad.Trim();
                        }
                    }
                }
            } // using connection
        } // if identidad isnullOrEmpty
    }

    protected void btnConsultarCliente_Click(object sender, EventArgs e)
    {
        int liValidarNumero;
        float liValidarDecimal;
        bool liEsNumerico;

        if (string.IsNullOrEmpty(ddlCiudadResidencia.SelectedValue))
        {
            MostrarMensaje("Seleccione una ciudad.");
            return;
        }

        if (string.IsNullOrEmpty(ddlOrigenIngreso.SelectedValue))
        {
            MostrarMensaje("Seleccione un tipo de origen de ingresos.");
            return;
        }

        /* Validamos si el ingreso es numerico */
        txtIngresos.Text = txtIngresos.Text.Replace(",", "");
        liEsNumerico = float.TryParse(txtIngresos.Text.Trim(), out liValidarDecimal);
        if (!liEsNumerico)
        {
            MostrarMensaje("Los ingresos deben ser numerico valido.");
            return;
        }

        txtIdentidad.Text = txtIdentidad.Text.Replace("-", "");
        liEsNumerico = int.TryParse(txtIdentidad.Text.Trim(), out liValidarNumero);
        if (string.IsNullOrEmpty(txtIdentidad.Text) || txtIdentidad.Text.ToString().Length != 13 || liEsNumerico)
        {
            MostrarMensaje("Ingrese una identidad valida.");
            return;
        }

        /* Validamos si el ingreso es numerico */
        liEsNumerico = float.TryParse(txtIngresos.Text.Trim(), out liValidarDecimal);
        if (!liEsNumerico)
        {
            MostrarMensaje("Los ingresos deben ser numerico valido.");
            return;
        }

        /* Validamos si el ingreso es numerico */
        txtTelefono.Text = txtTelefono.Text.Replace("-", "");
        liEsNumerico = int.TryParse(txtTelefono.Text.Trim(), out liValidarNumero);

        if (!liEsNumerico)
        {
            MostrarMensaje("Ingrese un numero de telefono valido. 1");
            return;
        }

        if (string.IsNullOrEmpty(txtTelefono.Text))
        {
            MostrarMensaje("Ingrese un numero de telefono valido. 2");
            return;
        }

        if (txtTelefono.Text.ToString().Trim().Length != 8)
        {
            MostrarMensaje("Ingrese un numero de telefono valido. 3");
            return;
        }

        /* Iniciamos todos los procesos de busqueda de datos */
        btnConsultarCliente.Enabled = false;

        /*****************************************************************************/
        /* Verificamos primero si el cliente ya fue consultado recientemente o no */
        string lcParametroEncriptado;
        string lcParametros;
        string lcScript;
        string lcIngresosPlanos = txtIngresos.Text.Replace(",", "");

        /* Realizamos la busqueda del cliente para ver si existe */
        using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
        {
            try
            {
                string lcIDProducto = "";
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CatalogoObtenerID", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@pcProducto", ddlProducto.SelectedItem.Text);

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        sqlResultado.Read();
                        lcIDProducto = sqlResultado["fiIDProducto"].ToString();
                    }
                }

                using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ClienteActualizado", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                    sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                    sqlComando.Parameters.AddWithValue("@pcIdentidad", txtIdentidad.Text.Trim());
                    sqlComando.Parameters.AddWithValue("@piIDProducto", lcIDProducto);
                    sqlComando.Parameters.AddWithValue("@pnIngresos", lcIngresosPlanos);
                    sqlComando.Parameters.AddWithValue("@pcTelefono", txtTelefono.Text.Trim());
                    sqlComando.Parameters.AddWithValue("@pcCiudadRedicencia", ddlCiudadResidencia.SelectedValue.ToString().Trim());
                    sqlComando.Parameters.AddWithValue("@pcTipoOcupacion", ddlOrigenIngreso.SelectedValue.ToString().Trim());
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        sqlResultado.Read();
                        if (sqlResultado["fiExiste"].ToString() != "0")
                        {

                            string lcPaginaWeb;
                            var lfPaginasWeb = new Funciones.FuncionesComunes();
                            lcPaginaWeb = lfPaginasWeb.UsrPaginaWeb(pcIDApp, pcIDUsuario, "3");

                            lcParametros = "usr=" + pcIDUsuario.Trim() +
                            "&IDApp=" + pcIDApp + "&ID=" + txtIdentidad.Text.Trim();

                            lcParametroEncriptado = DSC.Encriptar(lcParametros);
                            lcScript = "window.open('" + lcPaginaWeb + "?" + lcParametroEncriptado + "','_self')";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", lcScript, true);
                        }
                        else
                        {
                            string lcMaquilaCallCenter = lfValorRadioBoton(rbMaquilaoCallCenterSI);
                            string lcCasaPropia = lfValorRadioBoton(rbCasaPropiaSI);
                            string lcEsGuardiadeSeguridad = lfValorRadioBoton(rbGuardiaSI);
                            string lcAntiguedadRequerida = lfValorRadioBoton(rbAntiguedaddeNegocioSI);
                            string lcPermisodeOperacionVigente = lfValorRadioBoton(rbPermisoOperacionSI);

                            /*****************************************************************************/
                            /* Invocamos la URL para generar el archivo XML desde el recurso de CONFIAR */
                            lcParametros = "usr=" + pcIDUsuario.Trim() +
                            "&IDApp=" + pcIDApp.Trim() +
                            "&ID=" + txtIdentidad.Text.Trim() +
                            "&IDProd=" + lcIDProducto +
                            "&INS=" + lcIngresosPlanos +
                            "&TEL=" + txtTelefono.Text.Trim() +
                            "&IDCity=" + ddlCiudadResidencia.SelectedValue.ToString().Trim() +
                            "&OC=" + ddlOrigenIngreso.SelectedValue.ToString().Trim() +
                            "&MAQCC=" + lcMaquilaCallCenter.Trim() +
                            "&CP=" + lcCasaPropia.Trim() +
                            "&GS=" + lcEsGuardiadeSeguridad.Trim() +
                            "&AR=" + lcAntiguedadRequerida.Trim() +
                            "&POV=" + lcPermisodeOperacionVigente.Trim();

                            lcParametroEncriptado = DSC.Encriptar(lcParametros);
                            //lcScript = "window.open('ProcesarPrecalificado.aspx?" + lcParametroEncriptado + "','_self')";

                            CargarScripts();
                            ProcesarPrecalificado(txtIdentidad.Text.Trim(), lcParametroEncriptado);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje("Ocurrió un error, contacte al administrador.");
            }
        }
        btnConsultarCliente.Enabled = true;
    }

    private void LlenarListas()
    {
        using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
        {
            try
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CatalogoClientes", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        while (sqlResultado.Read())
                        {
                            ddlCiudadResidencia.Items.Add(sqlResultado[1].ToString().Trim());
                        }
                    }
                    ddlCiudadResidencia.Items.Insert(0, new ListItem("Seleccionar ciudad", ""));
                    ddlCiudadResidencia.SelectedIndex = 0;
                }

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CatalogoProductos", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        ddlProducto.DataSource = sqlResultado;
                        ddlProducto.DataTextField = "fcProducto";
                        ddlProducto.DataValueField = "fiIDProducto";
                        ddlProducto.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                MostrarMensaje(ex.Message);
            }
        }
    }

    protected void ddlOrigenIngreso_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlOrigenIngreso.SelectedValue == "Asalariado")
        {
            divPanelAsalariado.Visible = true;
            divPanelComerciante.Visible = false;
        }

        if (ddlOrigenIngreso.SelectedValue == "Comerciante")
        {
            divPanelAsalariado.Visible = false;
            divPanelComerciante.Visible = true;
        }

        if (ddlOrigenIngreso.SelectedValue == "")
        {
            divPanelAsalariado.Visible = false;
            divPanelComerciante.Visible = false;
        }
    }

    protected string lfValorRadioBoton(RadioButton prdBoton)
    {
        bool lbSeleccion;
        lbSeleccion = prdBoton.Checked;
        if (lbSeleccion)
        {
            return "1";
        }
        else
        {
            return "0";
        }
    }

    private void MostrarMensaje(string mensaje)
    {
        lblMensaje.Text = mensaje;
        PanelMensajeErrores.Visible = true;
    }

    private void CargarScripts()
    {
        string scriptMascarasDeEntrada =
        "<script>$('.MascaraCantidad').inputmask('decimal', { " +
        "alias: 'numeric', " +
                "groupSeparator: ',', " +
                "digits: 2, " +
                "integerDigits: 11, " +
                "digitsOptional: false, " +
                "placeholder: '0', " +
                "radixPoint: '.', " +
                "autoGroup: true, " +
                "min: 0.0, " +
            "}); " +
            "$('.MascaraNumerica').inputmask('decimal', { " +
        "alias: 'numeric', " +
                "groupSeparator: ',', " +
                "digits: 0, " +
                "integerDigits: 3," +
                "digitsOptional: false, " +
                "placeholder: '0', " +
                "radixPoint: '.', " +
                "autoGroup: true, " +
                "min: 0.0, " +
            "}); " +
        "$('.identidad').inputmask('9999999999999'); </script>";

        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "none", scriptMascarasDeEntrada, false);
    }


    protected void ProcesarPrecalificado(string identidad, string parametrosEncriptados)
    {
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_ConsultaSAF2", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@fcNumeroIdentidad", identidad);
                    sqlComando.CommandTimeout = 120;

                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        sqlResultado.Read();

                        if (sqlResultado.HasRows)
                        {
                            string lcScript = "window.open('Precalificado_ClienteInterno.aspx?" + parametrosEncriptados + "','_self')";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", lcScript, true);
                        }
                        else
                        {
                            string lcURLCoreFinanciero = "http://172.20.3.150/WSOrion/WSProcesarCliente.aspx?" + parametrosEncriptados;
                            HttpWebRequest request = WebRequest.Create(lcURLCoreFinanciero) as HttpWebRequest;
                            request.Accept = "text/xml";
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            WebHeaderCollection header = response.Headers;
                            var encoding = ASCIIEncoding.ASCII;

                            using (var reader = new System.IO.StreamReader(response.GetResponseStream(), encoding))
                            {
                                string responseText = reader.ReadToEnd();
                            }

                            string lcPaginaWeb;

                            Funciones.FuncionesComunes lfPaginasWeb = new Funciones.FuncionesComunes();

                            lcPaginaWeb = lfPaginasWeb.UsrPaginaWeb(pcIDApp, pcIDUsuario, "3");

                            string lcScript = "window.open('" + lcPaginaWeb + "?" + parametrosEncriptados + "','_self')";
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "_self", lcScript, true);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MostrarMensaje("Se ha encontrado el siguiente error: " + ex.Message);
        }
    }
}