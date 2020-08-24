using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.Adapters;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;


public partial class Clientes_BandejaPrecalificados : System.Web.UI.Page
{
    private string pcIDApp = "";
    private string pcIDSesion = "1";
    private string pcCadenaBusqueda = "";
    private string pcIDUsuario = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        string lcURL = "";
        int liParamStart = 0;
        string lcParametros = "";
        string lcEncriptado = "";
        string lcParametroDesencriptado = "";
        //string lcIDUsuario = "";
        string lcEstado = "";
        Uri lURLDesencriptado = null;
        HyperLink hlLink = new HyperLink();
        LinkButton btnLink = new LinkButton();

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
            //lcEncriptado = Request.Params["x"].ToString();
            lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);
            pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("SID");
        }
        /* FIN de captura de parametros y desencriptado de cadena */

        if (!IsPostBack)
        {
            lblTituloVentana.Text = "Bandeja de clientes precalificados";
            CargarLista("0");
            Botones("0");
            //gvPrecalificado.Columns[7].Visible = false;
        }
        //CargarScripts();
        //txtBuscador.Attributes.Add("onkeyup", "setTimeout('__doPostBack(\'" + txtBuscador.ClientID.Replace("_", "$") + "\',\'\')', 0);");
    }

    protected void gvPrecalificado_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        int index = Convert.ToInt32(e.CommandArgument);
        DataKey dkLlaveIdentidadCliente = gvPrecalificado.DataKeys[index];
        try
        {
            string lcScript = "";

            switch (e.CommandName)
            {
                case "Ver":
                    lcScript = "window.open('Precalificado_Analista.aspx?" + DSC.Encriptar("usr=" + pcIDUsuario + "&ID=" + dkLlaveIdentidadCliente.Value.ToString().Trim() + "&IDApp=108") + "','_Clientes_Contenido')";
                    break;
            }

            //PanelErrores.Visible = true;
            //lblMensaje.Text = dkLlaveIdentidadCliente.Value.ToString();

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
        }
        catch (Exception ex)
        {
            lblMensaje.Text = ex.Message;
        }
    }




    protected void btnFiltros_Click(object sender, EventArgs e)
    {
        Button ce = (Button)sender;

        switch (ce.CommandArgument)
        {
            case "0":
                lblTituloVentana.Text = "Bandeja de clientes precalificados";
                //gvPrecalificado.Columns[7].Visible = false;
                break;
            case "1":
                lblTituloVentana.Text = "Clientes pre-aprobados";
                //gvPrecalificado.Columns[7].Visible = false;
                break;
            case "2":
                lblTituloVentana.Text = "Clientes rechazados";
                //gvPrecalificado.Columns[7].Visible = false;
                break;
            case "3":
                lblTituloVentana.Text = "Clientes en analisis";
                //gvPrecalificado.Columns[7].Visible = false;
                break;
        }

        CargarLista(ce.CommandArgument.ToString());
        Botones(ce.CommandArgument.ToString());

    }

    protected void CargarLista(string pcEstado)
    {
        String lcSQLInstruccion = "";
        String sqlConnectionString = "";
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {
            sqlConexion.Open();
            lcSQLInstruccion = "exec CoreAnalitico.dbo.sp_Jefe_ListaClientesEstados " + pcIDApp + "," + pcIDUsuario.Trim() + "," + pcEstado.Trim();
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();


            //gvPrecalificado.DataSource = sqlResultado;
            //gvPrecalificado.DataBind();


            DataTable dfClientesPrecalificado = new DataTable();
            string lcIdentidad = "";
            string lcColumna1 = "";
            string lcColumna2 = "";
            string lcColumna3 = "";
            string lcColumna4 = "";
            string lcColumna5 = "";
            string lcImagen = "";
            dfClientesPrecalificado.Columns.Add("fcIdentidad", typeof(String));
            dfClientesPrecalificado.Columns.Add("fcColumna1", typeof(String));
            dfClientesPrecalificado.Columns.Add("fcColumna2", typeof(String));
            dfClientesPrecalificado.Columns.Add("fcColumna3", typeof(String));
            dfClientesPrecalificado.Columns.Add("fcColumna4", typeof(String));
            dfClientesPrecalificado.Columns.Add("fcColumna5", typeof(String));
            dfClientesPrecalificado.Columns.Add("fcImagen", typeof(String));

            while (sqlResultado.Read())
            {
                lcIdentidad = sqlResultado["fcIdentidad"].ToString();
                lcColumna1 = sqlResultado["fcNombreCorto"].ToString();
                lcColumna2 = "<span class=\"GridViewSpanNegro\">" + sqlResultado["fcNombre"].ToString() + "</span><br/><span class=\"GridViewSpan\"> ID: " + sqlResultado["fcIdentidad"].ToString() + "</span>";
                lcColumna3 = "<span class=\"GridViewSpanNegro\">Ingresos:</span> L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnIngresos"].ToString())) + "<br/><span class=\"GridViewSpan\">Telefono: " + sqlResultado["fcTelefono"].ToString() + "</span>";
                lcColumna4 = "<span class=\"GridViewSpanNegro\">" + sqlResultado["fcProducto"].ToString() + "</span><br/><span class=\"GridViewSpan\">Consultado: " + string.Format("{0:yyyy/MM/dd hh:mm:ss tt}", Convert.ToDateTime(sqlResultado["fdFechaPrimerConsulta"].ToString())) + "</span>";
                lcColumna5 = sqlResultado["fcMensaje"].ToString();
                lcImagen = sqlResultado["fcImagen"].ToString();
                dfClientesPrecalificado.Rows.Add(new object[] { lcIdentidad, lcColumna1, lcColumna2, lcColumna3, lcColumna4, lcColumna5, lcImagen });
            }

            gvPrecalificado.DataSource = dfClientesPrecalificado;
            gvPrecalificado.DataBind();

            gvPrecalificado.Columns[1].Visible = false;

            sqlComando.Dispose();
            sqlConexion.Close();
        }
        catch (Exception ex)
        {
            MensajeError(ex.Message, true);
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();

            MensajeError("", false);
        }

        lblTituloVentana.Text = lblTituloVentana.Text + " (" + string.Format("{0:#,###0}", Convert.ToDecimal(gvPrecalificado.Rows.Count.ToString())) + " clientes)";
        CargarScripts();
    }

    private void MensajeError(string pcMensaje, bool blVisible)
    {
        PanelErrores.Visible = blVisible;
        lblMensajeError.Text = pcMensaje;
    }


    private void Botones(string pcBoton)
    {
        string lcColorSeleccionado = "#656565";
        btnTodos.Style.Remove("background-color");
        btnAprobados.Style.Remove("background-color");
        btnAnalisis.Style.Remove("background-color");
        btnRechazados.Style.Remove("background-color");
        /*btnTodos.Text = "Todos";
        btnAprobados.Text = "Pre-aprobados";
        btnRechazados.Text = "Rechazados";
        btnAnalisis.Text = "En analisis";*/

        switch (pcBoton)
        {
            case "0":
                btnTodos.Style.Add("background-color", lcColorSeleccionado);
                //btnTodos.Text = "Todos(" + gvPrecalificado.Rows.Count.ToString().Trim() + ")";
                break;
            case "1":
                btnAprobados.Style.Add("background-color", lcColorSeleccionado);
                //btnAprobados.Text = "Pre-aprobados(" + gvPrecalificado.Rows.Count.ToString().Trim() + ")";
                break;
            case "2":
                btnRechazados.Style.Add("background-color", lcColorSeleccionado);
                //btnRechazados.Text = "Rechazdos(" + gvPrecalificado.Rows.Count.ToString().Trim() + ")";
                break;
            case "3":
                btnAnalisis.Style.Add("background-color", lcColorSeleccionado);
                //btnAnalisis.Text = "En analisis(" + gvPrecalificado.Rows.Count.ToString().Trim() + ")";
                break;
        }
    }

    private void CargarScripts()
    {
        string lcScriptEjecucion = "";
        lcScriptEjecucion = "$.expr[\":\"].containsNoCase = function (el, i, m) { var search = m[3];  if (!search) return false;  return eval(\"/\" + search + \"/i\").test($(el).text()); }; " +
            "$(document).ready( " +
            "function () { " +
            "$('#txtBuscador').keyup(function () { " +
            "if ($('#txtBuscador').val().length > 1) { " +
            "	$('#gvPrecalificado tr').hide(); " +
            "	$('#gvPrecalificado tr:first').show(); " +
            "	$('#gvPrecalificado tr td:containsNoCase(\\'' + $('#txtBuscador').val() + '\\')').parent().show(); " +
            "	document.getElementById('txtRegistros').value = $('tr').filter(function () { return $(this).css('display') !== 'none'; }).length + ' registros'; " +
            "	document.getElementById('gvPrecalificado').style.visibility = \"visible\"; " +
            "} " +
            "else if ($('#txtBuscador').val().length == 0) { resetSearchValue(); } " +
            "if ($('#gvPrecalificado tr:visible').length == 1) { " +
            "	$('.norecords').remove(); " +
            "	document.getElementById('gvPrecalificado').style.visibility = \"hidden\"; " +
            "	document.getElementById('txtRegistros').value = 'Sin registros'; " +
            "} " +
            "} " +
            "); " +
            "$('#txtBuscador').keyup(function (event) { if (event.keyCode == 27) { resetSearchValue(); } }); " +
            "});   " +
            "function resetSearchValue() { " +
            "	$('#txtBuscador').val(''); " +
            "	$('#gvPrecalificado tr').show(); " +
            "	$('.norecords').remove(); " +
            "	$('#txtBuscador').focus(); " +
            "	document.getElementById('gvPrecalificado').style.visibility = \"visible\"; " +
            "	document.getElementById('txtRegistros').value = ''; " +
            "}";
        /*
        StringBuilder sbConstructor = new StringBuilder();

        sbConstructor.Append(lcScriptEjecucion);
        ScriptManager.RegisterStartupScript(this, GetType(), "script", sbConstructor.ToString(), false);
        */
        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScriptEjecucion, true);

    }
}