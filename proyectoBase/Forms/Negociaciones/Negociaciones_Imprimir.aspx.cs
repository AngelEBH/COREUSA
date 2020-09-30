using System;
using System.Web;

public partial class Negociaciones_Imprimir : System.Web.UI.Page
{
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
                string parametrosURL = lcURL.Substring((liParamStart + 1), lcURL.Length - (liParamStart + 1));
                parametrosURL = parametrosURL.Replace("%2f", "/");
                Uri Parametros = new Uri("http://localhost/web.aspx?" + parametrosURL);

                var valorVehiculo = Convert.ToDecimal(HttpUtility.ParseQueryString(Parametros.Query).Get("ValorVehiculo"));
                var prima = Convert.ToDecimal(HttpUtility.ParseQueryString(Parametros.Query).Get("Prima"));
                var montoFinanciar = Convert.ToDecimal(HttpUtility.ParseQueryString(Parametros.Query).Get("MontoFinanciar"));
                var score = Convert.ToInt32(HttpUtility.ParseQueryString(Parametros.Query).Get("Score"));
                var tasaMensual = Convert.ToDecimal(HttpUtility.ParseQueryString(Parametros.Query).Get("TasaMensual"));
                var plazo = HttpUtility.ParseQueryString(Parametros.Query).Get("Plazo");
                var cuotaPrestamo = Convert.ToDecimal(HttpUtility.ParseQueryString(Parametros.Query).Get("CuotaPrestamo"));
                var valorGPS = Convert.ToDecimal(HttpUtility.ParseQueryString(Parametros.Query).Get("ValorGPS"));
                var valorSeguro = Convert.ToDecimal(HttpUtility.ParseQueryString(Parametros.Query).Get("ValorSeguro"));
                var gastosDeCierre = Convert.ToDecimal(HttpUtility.ParseQueryString(Parametros.Query).Get("GastosDeCierre"));
                var cliente = HttpUtility.ParseQueryString(Parametros.Query).Get("Cliente") ?? "CLIENTE FINAL";
                var nombreVendedor = HttpUtility.ParseQueryString(Parametros.Query).Get("Vendedor");
                var telefonoVendedor = HttpUtility.ParseQueryString(Parametros.Query).Get("TelefonoVendedor");
                var correoVendedor = HttpUtility.ParseQueryString(Parametros.Query).Get("CorreoVendedor");

                //?ValorVehiculo=280000&Prima=120000&MontoFinanciar=160000&Score=890&TasaMensual=1.91&Plazo=12 Meses&CuotaPrestamo=6623.31&ValorGPS=197.55&ValorSeguro=694.77&GastosDeCierre=4000.00&Vendedor=Jolman Alfaro&TelefonoVendedor=9855-8952&CorreoVendedor=jolman.flores@fucku.com,Cliente=Carlos Ordoñez

                CargarResultados(nombreVendedor, telefonoVendedor, correoVendedor, valorVehiculo, prima, montoFinanciar, score, tasaMensual, plazo, cuotaPrestamo, valorGPS, valorSeguro, gastosDeCierre, cliente);

            }
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
        }
    }

    protected void CargarResultados(string vendedor, string telefonoVendedor, string correoVendedor, decimal valorVehiculo, decimal prima, decimal montoFinanciar, int score, decimal tasaMensual, string plazo, decimal cuotaPrestamo, decimal valorGPS, decimal valorSeguro, decimal gastosDeCierre, string usuarioImprime, string cliente = "CLIENTE FINAL")
    {
        try
        {
            lblCliente.Text = cliente;
            lblFechaCotizacion.Text = DateTime.Now.ToString("MM/dd/yyyy");
            lblVendedor.Text = vendedor;
            //lblTelefonoVendedor.Text = telefonoVendedor;
            //lblCorreoVendedor.Text = correoVendedor;

            lblValorVehiculo.Text = "L " + string.Format("{0:#,###0.00}", valorVehiculo);

            lblPrima.Text = "L " + string.Format("{0:#,###0.00}", prima);

            lblMontoAFinanciar.Text = "L " + string.Format("{0:#,###0.00}", montoFinanciar);

            lblScore.Text = string.Format("{0:#,###0}", score);

            lblTasaMensual.Text = string.Format("{0:#,###0.00}", tasaMensual) + "%";

            lblPlazo.Text = plazo;
            lblCuotaPrestamo.Text = "L " + string.Format("{0:#,###0.00}", cuotaPrestamo);

            lblValorGPS.Text = "L " + string.Format("{0:#,###0.00}", valorGPS);
            lblGPS.Text = respuestaLogica(valorGPS);

            lblValorSeguro.Text = "L " + string.Format("{0:#,###0.00}", valorSeguro);
            lblSeguro.Text = respuestaLogica(valorSeguro);

            lblMontoGastosDeCierre.Text = "L " + string.Format("{0:#,###0.00}", gastosDeCierre);
            lblGastosDeCierre.Text = respuestaLogica(gastosDeCierre);

            lblUsuarioImprime.Text = usuarioImprime;
        }
        catch (Exception ex)
        {
            ex.Message.ToString();
            return;
        }
    }

    private string respuestaLogica(decimal cantidad)
    {
        return cantidad > 0 ? "SI" : "NO";
    }
}