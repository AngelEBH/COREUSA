using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;

public partial class ConvenioReadecuacion : System.Web.UI.Page
{
    public string lcNombreArchivoPDF = "";
    public static DSCore.DataCrypt DSC = new DSCore.DataCrypt();

    protected void Page_Load(object sender, EventArgs e)
    {
        var lcURL = Request.Url.ToString();
        var liParamStart = lcURL.IndexOf("?");
        var lcParametros = liParamStart > 0 ? lcURL.Substring(liParamStart, lcURL.Length - liParamStart) : string.Empty;

        if (lcParametros != string.Empty)
        {
            var lcEncriptado = lcURL.Substring(liParamStart + 1, lcURL.Length - (liParamStart + 1));
            var lcParametroDesencriptado = DSC.Desencriptar(lcEncriptado);
            var lURLDesencriptado = new Uri("http://localhost/web.aspx?" + lcParametroDesencriptado);

            var pcIDApp = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDApp");
            var pcIDUsuario = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("usr");
            var lcIDCliente = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("IDCliente");
            var pcIDConvenio = HttpUtility.ParseQueryString(lURLDesencriptado.Query).Get("Convenio");

            try
            {
                using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    sqlConexion.Open();

                    using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.sp_SRC_ClientesReadecuaciones_Consultar", sqlConexion))
                    {
                        sqlComando.CommandType = System.Data.CommandType.StoredProcedure;
                        sqlComando.Parameters.AddWithValue("@piIDSesion", "1");
                        sqlComando.Parameters.AddWithValue("@piIDApp", pcIDApp);
                        sqlComando.Parameters.AddWithValue("@piIDUsuario", pcIDUsuario);
                        sqlComando.Parameters.AddWithValue("@pcCodigoReadecuacion", pcIDConvenio);
                        sqlComando.CommandTimeout = 120;

                        using (var sqlResultado = sqlComando.ExecuteReader())
                        {
                            if (sqlResultado.Read())
                            {
                                lblNoAdendum.Text = sqlResultado["fcCodigoReadecuacion"].ToString().Trim();
                                lblNombreCliente.Text = sqlResultado["fcNombreSAF"].ToString().Trim();
                                lblIdentidadCliente.Text = sqlResultado["fcIdentidad"].ToString().Trim();
                                lblNoPrestamo.Text = sqlResultado["fcPrestamo"].ToString().Trim();
                                lcNombreArchivoPDF = lblNombreCliente.Text.Replace(" ", "_") + "_" + sqlResultado["fcIDCliente"].ToString().Trim();
                                lblAbonoInicial.Text = "L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnPagoInicial"].ToString().Trim()));

                                var TipodeCuota = sqlResultado["fcTipodeCuota"].ToString().Trim();
                                lblTipoCuotaTabla.InnerText = "Cuota " + TipodeCuota;

                                lblCantidadCuotas.Text = "L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnValorCuota"].ToString().Trim()));
                                lblPlazoReadecuacion.Text = string.Format("{0:#,###0}", Convert.ToDecimal(sqlResultado["fiPlazoQuincenas"].ToString().Trim())) + " " + TipodeCuota;
                                lblMontoReadecuar.Text = "L. " + string.Format("{0:#,###0.00}", Convert.ToDecimal(sqlResultado["fnTotalReadecuacion"].ToString().Trim()));

                                var lcConvertirFecha = (DateTime)sqlResultado["fdInicioAcuerdo"];
                                lblFechaInicialContrato.Text = lcConvertirFecha.ToString("dd/MM/yyyy");

                                lcConvertirFecha = (DateTime)sqlResultado["fdFinAcuerdo"];
                                lblFechaFinalContrato.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
                                lblTipodeCuota.Text = TipodeCuota;

                                lcConvertirFecha = (DateTime)sqlResultado["fdInicioAcuerdo"];
                                lblFechaPrimeraCuota.Text = lcConvertirFecha.ToString("dd/MM/yyyy");
                                lcConvertirFecha = (DateTime)sqlResultado["fdFinAcuerdo"];
                                lblFechaUltimaCuota.Text = lcConvertirFecha.ToString("dd/MM/yyyy");

                                lblCiudad.Text = sqlResultado["fcCiudad"].ToString().Trim();
                                lblDepartamento.Text = sqlResultado["fcDepartamento"].ToString().Trim();
                                lblDias.Text = sqlResultado["fcDiasFirma"].ToString().Trim();
                                lblMes.Text = sqlResultado["fcMesFimar"].ToString().Trim();
                                lblAnio.Text = sqlResultado["lblAnioFirma"].ToString().Trim();
                            }
                        } // using sqlResultado
                    } // using sqlComando
                } // using sqlConexion                
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                lblNoAdendum.Text = ex.Message;
            }
        } // if lcParametros != string.empty
    } // page load
}