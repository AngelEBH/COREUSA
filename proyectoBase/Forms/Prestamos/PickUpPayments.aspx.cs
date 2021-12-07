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


public partial class Prestamos_PickUpPayments : System.Web.UI.Page
{
    private string pcEncriptado = "";
    private string pcIDUsuario = "";
    public string pcIDApp = "";
    public string pcIDSesion = "";
    public static  string NombreCliente = "";
    public static string Identificacion = "";
    public static decimal ValorTotalPickupPayments = 0;
    public static  DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
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
            pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            pcIDSesion = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
        }

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

            lcSQLInstruccion = "exec sp_PickUpPayments_Lista " + pcIDSesion + "," + pcIDApp + "," + pcIDUsuario.Trim();
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            gvPickUpPayments.DataSource = sqlResultado;
            gvPickUpPayments.DataBind();
            sqlComando.Dispose();
            sqlConexion.Close();
        }
        catch (Exception ex)
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }


    }

    protected void gvMisPrestamos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DSCore.DataCrypt DSC = new DSCore.DataCrypt();
        int index = Convert.ToInt32(e.CommandArgument);
        DataKey dkIDSecuencia = gvPickUpPayments.DataKeys[index];
        //PanelErrores.Visible = true;
        //lblMensaje.Text = dkLlaveIdentidadCliente.Value.ToString();
        try
        {
            //string lcScript = "window.open('ImpresionFacturaAbonos.aspx?" + DSC.Encriptar(pcParametros.Trim()) + "',\"_blank\",\"toolbar=0,location=0,menubar=0,width=700,resizable=0\")";
            string lcScript = "window.open('Prestamo_Ficha.aspx?" + DSC.Encriptar("SID=" + pcIDSesion + "&usr=" + pcIDUsuario + "&Pre=" + dkIDSecuencia.Value.ToString().Trim() + "&IDApp=" + pcIDApp) + "',\"_self\",\"toolbar=0,location=0,menubar=0,width=700,resizable=0\")";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "newpage", lcScript, true);
            /*Response.Write("<script>");
            Response.Write(lcScript);
            Response.Write("</script>");*/

            //Page.Response.Redirect(Page.Request.Url.ToString(), true);

        }
        catch (Exception ex)
        {
            PanelErrores.Visible = true;
            lblMensaje.Visible = true;
            lblMensaje.Text = ex.Message;
        }
    }

    protected void btnAgregarPickUpPayment_Click(object sender, EventArgs e)
    {
        //PanelErrores.Visible = true;
        //string mensaje = "Mensaje el monto de Pick Up Payments son mayores al monto de DownPayment";
        //lblMensaje.Visible = true;
        //lblMensaje.Text = mensaje;
        PanelAgregarPickUpPayment.Visible = true;
    }
   
  

    protected void gvPickUpPayments_RowCommand(object sender, GridViewCommandEventArgs e)
    {
     
        if (e.CommandName == "Agregar")
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow selectedRow = gvPickUpPayments.Rows[index];
            string idTransaccion = selectedRow.Cells[0].Text;
            
           var lista= ObtenerDetallePickUpPayments(Convert.ToInt32(idTransaccion));
            gvPickUpPaymentsDetalle.DataSource = lista;
            gvPickUpPaymentsDetalle.DataBind();
            txtNombre_Detalle.Text = NombreCliente;
            txtIdentificación_Detalle.Text = Identificacion;
            txtMontoDownPayment_Detalle.Text = Convert.ToString(ValorTotalPickupPayments);
            PanelDetalle.Visible = true;
        }

    }

    public static List<PickUpPayments_ViewModel> ObtenerDetallePickUpPayments(int idTransaccion)
    { 
        
        string dataCrypt;
        var ListaPickUpPayments = new List<PickUpPayments_ViewModel>();
      
        try
        {
             
            //idTransaccion = 92;
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();
                using (var sqlComando = new SqlCommand("sp_PickUpPayments_ListaDetalle", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@piIDTransaccion", idTransaccion);
                    sqlComando.Parameters.AddWithValue("@piIDSesion", 1);
                    sqlComando.Parameters.AddWithValue("@piIDApp", 1);
                    sqlComando.Parameters.AddWithValue("@piIDUsuario", 1);
                    sqlComando.CommandTimeout = 120;
                    using (var sqlResultado = sqlComando.ExecuteReader())
                    {
                        var NombreCliente  = "";
                        var MontoTotal = 0;
                        while (sqlResultado.Read())
                        {
                            ListaPickUpPayments.Add(new PickUpPayments_ViewModel()
                            {
                                IdTransaccion = (int)sqlResultado["fiIDTransaccion"],
                                fnNumeroCuota = (int)sqlResultado["fnNumeroCuota"],
                                fdFecha = (DateTime)sqlResultado["fdFecha"],
                                fnValorDownPayment = (decimal)sqlResultado["fnValorDownPayment"],
                                fnSaldoCuota = (decimal)sqlResultado["fnSaldoCuota"],
                                fcNombreCliente = (string)sqlResultado["fcNombreCliente"],                            
                                Identificacion = (string)sqlResultado["fcIdentificacion"],
                                fnValorDownPaymentTotal = (decimal)sqlResultado["fnValorDownPaymentTotal"]
                            });

                        }
                    }

               
                    foreach(var item in ListaPickUpPayments)
                    {
                        NombreCliente = item.fcNombreCliente;
                        Identificacion = item.Identificacion;
                        ValorTotalPickupPayments = item.fnValorDownPaymentTotal;
                    }
               
                    
                }
            }

        }
        catch (Exception ex)
        {
            ex.Message.ToString();
           
        }
        return ListaPickUpPayments;
    }

   protected void btnRegistrar_Click(object sender, EventArgs e)
    {

        
        if (Convert.ToDouble(txtMontoDownPayment.Text) <= 0)
        {
            return;
        }


        PanelAgregarPickUpPayment.Visible = false;
        String lcSQLInstruccion = "";
        String sqlConnectionString = "";
        SqlConnection sqlConexion = null;
        SqlDataReader sqlResultado = null;
        SqlCommand sqlComando = null;
        sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

        sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
        try
        {
            var montoTotalPickUpPayment = Convert.ToDouble(txtMontoDownPayment1.Text) + Convert.ToDouble(txtMontoDownPayment2.Text) + Convert.ToDouble(txtMontoDownPayment3.Text);

            if (montoTotalPickUpPayment > Convert.ToDouble(txtMontoDownPayment.Text))
            {
                PanelErrores.Visible = true;
                string mensaje = "Mensaje el monto de Pick Up Payments son mayores al monto de DownPayment";
                lblMensaje.Visible = true;
                lblMensaje.Text = mensaje;
            }
            else
            {
                sqlConexion.Open();
                lcSQLInstruccion = "sp_PickUpPayments_Agregar ";
                sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
                sqlComando.CommandType = CommandType.StoredProcedure;
                sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                sqlComando.Parameters.AddWithValue("@pcIdentificacion", txtIdentificacion.Text.Trim());
                sqlComando.Parameters.AddWithValue("@pnValorDownPayment", txtMontoDownPayment.Text.Trim());
                sqlComando.Parameters.AddWithValue("@pdFechaVencimiento", txtFechaVencimiento.Text.Trim());
                sqlComando.Parameters.AddWithValue("@pcComentarios", txtComentarios.Text.Trim());

                sqlComando.Parameters.AddWithValue("@pnValorDownPayment1", txtMontoDownPayment1.Text.Trim());
                sqlComando.Parameters.AddWithValue("@pdFechaVencimiento1", txtFechaVencimiento1.Text.Trim());
                sqlComando.Parameters.AddWithValue("@pnValorDownPayment2", txtMontoDownPayment2.Text.Trim());
                sqlComando.Parameters.AddWithValue("@pdFechaVencimiento2", txtFechaVencimiento2.Text.Trim());
                sqlComando.Parameters.AddWithValue("@pnValorDownPayment3", txtMontoDownPayment3.Text.Trim());
                sqlComando.Parameters.AddWithValue("@pdFechaVencimiento3", txtFechaVencimiento3.Text.Trim());

                sqlComando.ExecuteNonQuery();

                PanelAgregarPickUpPayment.Visible = false;
            }
           
        }
        catch (Exception ex)
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }

    }

    protected void btnCerrar_Click(object sender, EventArgs e)
    {
        txtNombre_Detalle.Text = "";
        txtIdentificación_Detalle.Text = "";
        txtMontoDownPayment_Detalle.Text = "";
      
        PanelAgregarPickUpPayment.Visible = false;
        PanelDetalle.Visible = false;
       
    }

  
    protected void btnBuscarCliente_Click(object sender, EventArgs e)
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

            lcSQLInstruccion = "exec CoreAnalitico.dbo.sp_info_ConsultaEjecutivos " + pcIDSesion + "," + pcIDApp + ",'" + txtIdentificacion.Text.Trim() + "'";
            sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
            sqlComando.CommandType = CommandType.Text;
            sqlResultado = sqlComando.ExecuteReader();
            if (sqlResultado.Read())
            {
                txtNombreCliente.Text = sqlResultado["fcPrimerNombre"].ToString() + " " + sqlResultado["fcSegundoNombre"].ToString() + " " + sqlResultado["fcPrimerApellido"].ToString() + " " + sqlResultado["fcSegundoApellido"].ToString();
                btnRegistrar.Enabled = true;
            }
            else
            {
                btnRegistrar.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            PanelErrores.Visible = true;
            lblMensaje.Text = ex.Message;
        }
        finally
        {
            if (sqlConexion.State == ConnectionState.Open)
                sqlConexion.Close();
        }

    }

    public class PickUpPayments_ViewModel
    {
        public int IdTransaccion { get; set; }
        public int fnNumeroCuota { get; set; }
        public DateTime fdFecha { get; set; }
        public decimal fnValorDownPayment { get; set; }
        public decimal fnSaldoCuota { get; set; }
        public string fcNombreCliente { get; set; }
        public decimal fnValorDownPaymentTotal { get; set; }
        public string Identificacion { get; set; }

    }

}