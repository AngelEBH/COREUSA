using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Globalization;

public partial class Prestamos_Prestamo_Ficha : System.Web.UI.Page
{
    private string pcIDSesion = "";
    private string pcIDApp = "";
    private string pcIDUsuario = "";
    private string pcIDPrestamo = "";
    private string pcParametros = "";
    private DSCore.DataCrypt DSC = new DSCore.DataCrypt();
    private String lcSQLInstruccion = "";
    private String sqlConnectionString = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        /* INICIO de captura de parametros y desencriptado de cadena */
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        string lcURL = "";
        int liParamStart = 0;
        string lcParametros = "";
        string lcEncriptado = "";
        string lcParametroDesencriptado = "";
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
            pcIDPrestamo = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("Pre");
            /*SID=1&IDApp=103&usr=1&Pre=210500000025*/
            /*t3+YMGlCBK9wBonzu6eIVgE0NEpomM3PNcHNDZl2scFgOunZ1TBkaLzysEFzvhEe*/
        }
        /* FIN de captura de parametros y desencriptado de cadena */



         RefrescarPrestamo(pcIDPrestamo);

     
    }

    protected void btnActivarPrestamo_Click(object sender, EventArgs e)
    {
        if (lblProducto.Text.Trim() == "Auto Loan")
        {
            PanelDescuento.Visible = true;
        }
        else
        {
            PanelActivarPrestamo.Visible = true;

            String lcSQLInstruccion = "";
        String sqlConnectionString = "";
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

        sqlConexion.Open();
        try
        {
            PanelDescuento.Visible = false;
            PanelActivarPrestamo.Visible = true;
            

            /***********************************************************************************/
             /*Gridview de datos del desembolso ************************************************/
            lcSQLInstruccion = "exec sp_Prestamo_Desembolso_Previo '" + pcIDPrestamo + "'";
            //lcSQLInstruccion = "exec sp_Prestamo_Desembolso_Previo '" + pcIDPrestamo + "',1,15000";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            gvDesembolsoDetalle.DataSource = sqlResultado;
            gvDesembolsoDetalle.DataBind();
        }
        catch(Exception ex)
        {
            lblMensaje.Text = ex.Message;
            lblMensaje.Text = lcSQLInstruccion;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }
        }
    }

    protected void btnActivar_Click(object sender, EventArgs e)
    {
        string lcIP = ObtenerIP();
        String lcSQLInstruccion = "";
        String sqlConnectionString = "";
        SqlConnection sqlConexion = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

        try
        {
            sqlConexion.Open();
            lcSQLInstruccion = "sp_Prestamo_ActivarPrestamo ";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.StoredProcedure;
            sqlComando.Parameters.AddWithValue("@piIDSesion", pcIDSesion);
            sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
            sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
            sqlComando.Parameters.AddWithValue("@pcIDPrestamo", pcIDPrestamo);
            sqlComando.Parameters.AddWithValue("@pcIP", lcIP.Trim());
            sqlComando.Parameters.AddWithValue("@pdFechaInicioContrato", txtFechadelContrato.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pdFechaPrimerPago", txtFechaPrimerPago.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcComentarios", txtComentariosDesembolso.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcTipoSolicituddePago", txtFormadePago.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcNombreBeneficiario", txtBeneficiario.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcBeneficiarioDocumento1", txtIdentidadBeneficiario.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcBeneficiarioDocumento2", txtRTNBeneficiario.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcInformacionBancaria1", txtBancoaDepositarBeneficiario.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcInformacionBancaria2", txtCuentaBancariaBeneficiario.Text.Trim());
            sqlComando.Parameters.AddWithValue("@pcInformacionBancaria3", txtTipodeCuentaBancariaBeneficiario.Text.Trim());

            if (lblProducto.Text.Trim() == "Auto Loan")
            {
                sqlComando.Parameters.AddWithValue("@piOrigenVehiculo", ddlOrigendelVehiculo.SelectedValue.ToString());
                sqlComando.Parameters.AddWithValue("@pnValorVehiculo", txtValordelVehiculo.Text.Trim());
            }
            sqlComando.ExecuteNonQuery();
            PanelActivarPrestamo.Visible = false;
             RefrescarPrestamo(pcIDPrestamo);
        }
        catch (Exception ex)
        {
            /*PanelErrores.Visible = true;*/
            lblMensaje.Text = ex.Message;
            //lblMensaje.Text = lcSQLInstruccion;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }
    }

    protected void btnCerrar_Click(object sender, EventArgs e)
    {
        PanelActivarPrestamo.Visible = false;
    }

    private string FechaNula(string pcFechaaVerificar)
    {
        string lcFecha = "";
        DateTime lcConvertirFecha = new DateTime();

        if (pcFechaaVerificar.Trim() != "")
        {
            lcConvertirFecha = Convert.ToDateTime(pcFechaaVerificar, CultureInfo.InvariantCulture);
            lcFecha = lcConvertirFecha.ToString("dd/MM/yyyy");
        }
        else
        {
            lcFecha = "";
        }

        return lcFecha;
    }

    private string ObtenerIP()
    {
        string ipAdd;

        ipAdd = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (string.IsNullOrEmpty(ipAdd))
        {
            ipAdd = Request.ServerVariables["REMOTE_ADDR"];
        }

        return ipAdd;
    }


    protected void btnContinuarDescuento_Click(object sender, EventArgs e)
    {

        String lcSQLInstruccion = "";
        String sqlConnectionString = "";
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

        sqlConexion.Open();
        try
        {
            PanelDescuento.Visible = false;
            PanelActivarPrestamo.Visible = true;
            

            /***********************************************************************************/
             /*Gridview de datos del desembolso ************************************************/
            lcSQLInstruccion = "exec sp_Prestamo_Desembolso_Previo '" + pcIDPrestamo + "'," + ddlOrigendelVehiculo.SelectedValue.ToString() + "," + txtValordelVehiculo.Text.Trim();
            //lcSQLInstruccion = "exec sp_Prestamo_Desembolso_Previo '" + pcIDPrestamo + "',1,15000";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            gvDesembolsoDetalle.DataSource = sqlResultado;
            gvDesembolsoDetalle.DataBind();
        }
        catch(Exception ex)
        {
            lblMensaje.Text = ex.Message;
            lblMensaje.Text = lcSQLInstruccion;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }
    }

    protected void btnCerrarDescuento_Click(object sender, EventArgs e)
    {
        PanelDescuento.Visible = false;
    }

 protected void btnSolicitudPrestamo_Click(object sender, EventArgs e)
    {
        /*usr=1&IDApp=107&SID=1&pcID=0506199100025&IDSOL=1473&IDGarantia=2400&IdCredCliente=1449&idExpediente=0*/
        /*usr=1&IDApp=107&SID=1&pcID=1803198700137&IDSOL=1475&IDGarantia=2402&IdCredCliente=1451&idExpediente=0*/
         DSCore.DataCrypt DSC = new DSCore.DataCrypt();
         String lcCadenaParametro = "";
         lcCadenaParametro = "usr=" + pcIDUsuario.Trim() + "&IDApp=107&pcID=" + txtIdentidadCliente.Text.Trim()+ "&IDSOL="+ txtIDSolicitud.Text.Trim();
         String lcScript = "";
         lcScript = "window.open('../Solicitudes/Forms/Solicitudes/SolicitudesCredito_Detalles.aspx?" + DSC.Encriptar(lcCadenaParametro) + "','postwindow','toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=300')";
         ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newwindowFinder", lcScript, true);
    }


//  protected void btnDocumentoSolicitudPrestamo_Click(object sender, EventArgs e)
//     {
//         /*usr=1&IDApp=107&SID=1&pcID=0506199100025&IDSOL=1473&IDGarantia=2400&IdCredCliente=1449&idExpediente=0*/
//         /*usr=1&IDApp=107&SID=1&pcID=1803198700137&IDSOL=1475&IDGarantia=2402&IdCredCliente=1451&idExpediente=0*/
//          DSCore.DataCrypt DSC = new DSCore.DataCrypt();
//          String lcCadenaParametro = "";
//          lcCadenaParametro = "usr=" + pcIDUsuario.Trim() + "&IDApp=107&pcID=" + txtIdentidadCliente.Text.Trim()+ "&IDSOL="+ txtIDSolicitud.Text.Trim();
//          String lcScript = "";
//          lcScript = "window.open('../Solicitudes/Forms/Solicitudes/SolicitudesCredito_Detalles.aspx?" + DSC.Encriptar(lcCadenaParametro) + "','postwindow','toolbar=0,scrollbars=0,location=0,statusbar=0,menubar=0,resizable=0,width=300')";
//          ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newwindowFinder", lcScript, true);

        
//     }


    



     private void RefrescarPrestamo(string pcIDPrestamo)
    {
        String lcSQLInstruccion = "";
        String sqlConnectionString = "";
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
        string lcFrecuenciadePago = "";

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {
            sqlConexion.Open();
            lcSQLInstruccion = "exec sp_Prestamo_InformacionCliente " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario.Trim() + ",'" + pcIDPrestamo + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            if (sqlResultado.Read())
            {
                /* Datos del cliente */
                txtIdentidadCliente.Text = "XXXXXXXX" + sqlResultado["fcIdentidadCliente"].ToString().Substring(sqlResultado["fcIdentidadCliente"].ToString().Length - 4, 4);
                txtNombreCliente.Text = sqlResultado["fcNombreCliente"].ToString();
                txtProducto.Text = sqlResultado["fcProducto"].ToString();

                /* Informacion del financiamiento */
                imgLogo.ImageUrl = sqlResultado["fcImagenProducto"].ToString();
                lblProducto.Text = sqlResultado["fcProducto"].ToString();
                txtIDSolicitud.Text = sqlResultado["fiIDSolicitud"].ToString();
                txtIDPrestamo.Text = sqlResultado["fcIDPrestamo"].ToString();
                txtEstadodelPrestamo.Text = sqlResultado["fcEstadodelPrestamo"].ToString();
                txtFechaCreacion.Text = FechaNula(sqlResultado["fdFechaCreacion"].ToString());
                txtFechaColocacion.Text = FechaNula(sqlResultado["fdFechaDesembolso"].ToString());
                txtFechaInicioPrestamo.Text = FechaNula(sqlResultado["fdFechaInicio"].ToString());
                txtFechaFinPrestamo.Text = FechaNula(sqlResultado["fdFechaVencimiento"].ToString());
                txtSaldoActual.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnSaldoActualCapital"].ToString()));
                txtFrecuencia.Text = sqlResultado["fcFrecuenciadePago"].ToString();
                txtPlazo.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiPlazo"].ToString()));
                txtMontoFinanciado.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnCapitalFinanciado"].ToString()));
                txtValorCuota.Text = string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValorCuota"].ToString()));
                txtCuotasPagadas.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiCuotasPagadas"].ToString()));
                txtCuotasAtrasadas.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiCuotasAtrasadas"].ToString()));
                txtDiasAtraso.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiDiasAtraso"].ToString()));
                txtTasadeInteres.Text = string.Format("{0:#,###0.0000}", Convert.ToDecimal(sqlResultado["fiTasadeInteres"].ToString())) + " %";
                txtVINVehiculo.Text = sqlResultado["fcVINVehiculo"].ToString();
                txtVINVehiculo.Text = sqlResultado["fcVINVehiculo"].ToString();
                txtFrecuenciadePago.Text = sqlResultado["fcFrecuenciadePago"].ToString();

                if (sqlResultado["fiIDProducto"].ToString() == "102")
                {
                    PanelDealer.Visible = true;
                }

                if (txtEstadodelPrestamo.Text == "Pendiente")
                {
                    btnActivarPrestamo.Visible = true;
                }
                else
                {
                    btnActivarPrestamo.Visible = false;
                }

            }
            /* Gridview de PlandePago */
            //sp_Prestamo_PlandePago_ConsultarPorPrestamo
            /*lcSQLInstruccion = "exec sp_Catalogo_FrecuenciadePago " + sqlResultado["fiIDProducto"].ToString();
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            ddlFrecuencia.Items.Clear();

            while (sqlResultado.Read())
            {
                ddlFrecuencia.Items.Add(new ListItem(sqlResultado["fcTipodePlazo"].ToString(), sqlResultado["fiIDTipoDePlazo"].ToString()));
                if (sqlResultado["fcTipodePlazo"].ToString().Trim()==lcFrecuenciadePago.Trim())
                {
                    lcFrecuenciadePago = sqlResultado["fiIDTipoDePlazo"].ToString();
                }
            }
            ddlFrecuencia.SelectedValue = lcFrecuenciadePago;*/

            /* Gridview de PlandePago */
            //sp_Prestamo_PlandePago_ConsultarPorPrestamo

            lcSQLInstruccion = "exec sp_Prestamo_PlandePago_ConsultarPorPrestamo '" + pcIDPrestamo + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            gvPlandePago.DataSource = sqlResultado;
            gvPlandePago.DataBind();

            /***********************************************************************************/
            /* Gridview de estado de cuenta ****************************************************/

            lcSQLInstruccion = "exec sp_Prestamo_PlandePago_ConsultarAvancePorPrestamo '" + pcIDPrestamo + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            gvPlandePagoAvance.DataSource = sqlResultado;
            gvPlandePagoAvance.DataBind();

            /***********************************************************************************/
            /* Gridview de estado de cuenta ****************************************************/
            lcSQLInstruccion = "exec sp_Prestamo_EstadodeCuenta_ConsultarPorPrestamo '" + pcIDPrestamo + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            gvEstadodeCuenta.DataSource = sqlResultado;
            gvEstadodeCuenta.DataBind();


            /***********************************************************************************/
            /* Gridview de desembolso del prestamo *********************************************/
            lcSQLInstruccion = "exec sp_Prestamo_Desembolso_Consultar '" + pcIDPrestamo + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            gvPartidaDesembolso.DataSource = sqlResultado;
            gvPartidaDesembolso.DataBind();


            /***********************************************************************************/
            /* Traemos la informacion del tipo de desembolso ***********************************/
            lcSQLInstruccion = "exec sp_Prestamo_Desembolso_ConsultaTipo '" + pcIDPrestamo + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            if (sqlResultado.Read())
            {
                /* Datos del cliente */
                txtIdentidadBeneficiario.Text = sqlResultado["fcIdentidadVendedorGarantia"].ToString();
                txtRTNBeneficiario.Text = sqlResultado["fcRTNVendedorGarantia"].ToString();
                txtBeneficiario.Text = sqlResultado["fcNombreVendedorGarantia"].ToString();
                txtFormadePago.Text = sqlResultado["fcTipoSolicituddePago"].ToString();
                txtBancoaDepositarBeneficiario.Text = sqlResultado["fcNombreBanco"].ToString();
                txtCuentaBancariaBeneficiario.Text = sqlResultado["fcCuentaBancoDesembolso"].ToString();
                txtTipodeCuentaBancariaBeneficiario.Text = sqlResultado["fcTipoCuentaBancoDesembolso"].ToString();
            }
            
            sqlComando.Dispose();
            sqlConexion.Close();

        }
        catch (Exception ex)
        {
            /*PanelErrores.Visible = true;*/
            lblMensaje.Text = ex.Message;
            //lblMensaje.Text = lcSQLInstruccion;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }
    }









}