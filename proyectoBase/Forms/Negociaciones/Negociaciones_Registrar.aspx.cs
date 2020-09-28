using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoBase.Forms.Negociaciones
{
    public partial class Negociaciones_Registrar : System.Web.UI.Page
    {
        private string pcIDUsuario = "";
        private string pcIDApp = "";
        private string pcIDSesion = "";
        private string pcID = "";
        private DSCore.DataCrypt DSC = new DSCore.DataCrypt();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                /* INICIO de captura de parametros y desencriptado de cadena */
                string lcURL = Request.Url.ToString();
                int liParamStart = lcURL.IndexOf("?");

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
                    string lcEncriptado = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                    lcEncriptado = lcEncriptado.Replace("%2f", "/");
                    string lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
                    Uri lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
                    pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
                    pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
                    pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
                    pcID = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("ID");

                    // validar si el cliente ya está precalificado
                    CargarPrecalificado(pcID);
                    LlenarListas();
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }

        }

        private void CargarPrecalificado(string pcID)
        {
            try
            {
                using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    sqlConexion.Open();

                    using (var sqlComando = new SqlCommand("CoreAnalitico.dbo.sp_info_ConsultaEjecutivos", sqlConexion))
                    {
                        sqlComando.CommandType = System.Data.CommandType.StoredProcedure;
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
                                txtIdentidadCliente.Text = (string)sqlResultado["fcIdentidad"];
                                txtNombreCliente.Text = (string)sqlResultado["fcPrimerNombre"] + " " + (string)sqlResultado["fcSegundoNombre"] + " " + (string)sqlResultado["fcPrimerApellido"] +" " + (string)sqlResultado["fcSegundoApellido"];
                                txtTelefonoCliente.Text = (string)sqlResultado["fcTelefono"];
                                txtFecha.Text = DateTime.Now.ToString("MM/dd/yyyy");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        protected void LlenarListas()
        {
            try
            {
                using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    sqlConexion.Open();

                    using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDNegociaciones_Guardar_LlenarListas", sqlConexion))
                    {
                        sqlComando.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.CommandTimeout = 120;

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            // catalogo autolotes
                            ddlAutolote.Items.Clear();
                            while (sqlResultado.Read())
                            {
                                ddlAutolote.Items.Add(new ListItem(sqlResultado["fcAutolote"].ToString(), sqlResultado["fiIDAutolote"].ToString()));
                            }


                            // catalogo de marcas
                            ddlModelo.Items.Clear();
                            ddlModelo.Items.Add(new ListItem("Seleccione una marca", "0"));
                            ddlMarca.Items.Clear();
                            ddlMarca.Items.Add(new ListItem("Seleccionar", "0"));
                            while (sqlResultado.Read())
                            {
                                ddlMarca.Items.Add(new ListItem(sqlResultado["fcMarca"].ToString(), sqlResultado["fiIDMarca"].ToString()));
                            }

                            // catalogo de origenes de garantia
                            ddlOrigen.Items.Clear();
                            while (sqlResultado.Read())
                            {
                                ddlMarca.Items.Add(new ListItem(sqlResultado["fcOrigenGarantia"].ToString(), sqlResultado["fiIDOrigenGarantia"].ToString()));
                            }

                            // catalogo de tipos de negociacion
                            while (sqlResultado.Read())
                            {

                            }

                            // catalogo de unidad de medida
                            ddlUnidadDeMedida.Items.Clear();
                            while (sqlResultado.Read())
                            {
                                ddlMarca.Items.Add(new ListItem(sqlResultado["fcUnidadDeMedida"].ToString(), sqlResultado["fiUnidadDeMedida"].ToString()));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        // Cargar modelos de la marca seleccionada
        protected void ddlMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int idMarca = int.Parse(ddlMarca.SelectedValue);

                ddlModelo.Items.Clear();
                ddlModelo.Items.Add(new ListItem("Seleccione una marca", "0"));

                if (idMarca != 0)
                {
                    using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                    {
                        sqlConexion.Open();

                        using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_CREDNegociaciones_Guardar_ListarModelosPorIdMarca", sqlConexion))
                        {
                            sqlComando.CommandType = System.Data.CommandType.StoredProcedure;
                            sqlComando.Parameters.AddWithValue("@piIDMarca", idMarca);
                            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                            sqlComando.Parameters.AddWithValue("@pcIdentidad", pcID);
                            sqlComando.CommandTimeout = 120;

                            using (var sqlResultado = sqlComando.ExecuteReader())
                            {
                                while (sqlResultado.Read())
                                {
                                    ddlMarca.Items.Add(new ListItem(sqlResultado["fcModelo"].ToString(), sqlResultado["fiIDModelo"].ToString()));
                                }
                            }
                        }
                    }
                }
                else
                {
                    ddlModelo.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                return;
            }
        }

        protected void rbFinanciamiento_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                divPrima.Visible = !rbEmpeno.Checked;
                txtValorPrima.Visible = !rbEmpeno.Checked;
                lblPorcenajedePrima.Visible = !rbEmpeno.Checked;
                divMontoFinanciarVehiculo.Visible = rbEmpeno.Checked;
                txtMonto.Enabled = rbEmpeno.Checked;

                ddlPlazos.Items.Clear();
                ddlPlazos.Items.Add("12 Meses");
                ddlPlazos.Items.Add("18 Meses");
                ddlPlazos.Items.Add("24 Meses");
                ddlPlazos.Items.Add("36 Meses");
                ddlPlazos.Items.Add("48 Meses");
                ddlPlazos.Items.Add("60 Meses");
                //CargarScripts();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        protected void rbEmpeno_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                divPrima.Visible = !rbEmpeno.Checked;
                txtValorPrima.Visible = !rbEmpeno.Checked;
                lblPorcenajedePrima.Visible = !rbEmpeno.Checked;

                divMontoFinanciarVehiculo.Visible = rbEmpeno.Checked;
                txtMonto.Enabled = rbEmpeno.Checked;

                ddlPlazos.Items.Clear();
                ddlPlazos.Items.Add("12 Meses");
                ddlPlazos.Items.Add("18 Meses");
                ddlPlazos.Items.Add("24 Meses");
                ddlPlazos.Items.Add("30 Meses");
                //CargarScripts();
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        private void CargarScripts()
        {
            try
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
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }
    }
}