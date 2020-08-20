using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Configuration;

namespace proyectoBase.Forms.Solicitudes
{
    public partial class SolicitudesCredito_ClientesPrecalificados : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static List<ClientesPrecalificadosViewModel> GetClientesPrecalificados(string identidad)
        {
            List<ClientesPrecalificadosViewModel> listaClientesPrecalificados = new List<ClientesPrecalificadosViewModel>();
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            try
            {
                using (SqlConnection conn = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("EXEC CoreAnalitico.dbo.sp_Vendedor_ListaClientesEstados @piIDApp, @piIDUsuario, @piResultadoPrecalificado", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@piIDApp", 103);
                        cmd.Parameters.AddWithValue("@piIDUsuario", 1);
                        cmd.Parameters.AddWithValue("@piResultadoPrecalificado", 1);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                listaClientesPrecalificados.Add(new ClientesPrecalificadosViewModel()
                                {
                                    identidad = (string)reader["fcIdentidad"],
                                    nombres = (string)reader["fcNombre"],
                                    telefono = (string)reader["fcTelefono"],
                                    ingresos = Decimal.Parse(reader["fnIngresos"].ToString()),
                                    comentario = (string)reader["fcMensaje"]
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return listaClientesPrecalificados;
        }

        [WebMethod]
        public static PrecalificadoViewModel GetDetallesPrecalificado(string identidad)
        {
            PrecalificadoViewModel objPrecalificado = null;
            List<cotizadorProductosViewModel> listaCotizadorProductos = new List<cotizadorProductosViewModel>();
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            try
            {
                using (SqlConnection conn = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("EXEC CoreAnalitico.dbo.sp_info_ConsultaEjecutivos @piIDApp, @piIDUsuario, @pcIdentidad", conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@piIDApp", 103);
                        cmd.Parameters.AddWithValue("@piIDUsuario", 1);
                        cmd.Parameters.AddWithValue("@pcIdentidad", identidad);
                        cmd.CommandTimeout = 60;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                objPrecalificado = new PrecalificadoViewModel()
                                {
                                    identidad = (string)reader["fcIdentidad"],
                                    primerNombre = (string)reader["fcPrimerNombre"],
                                    segundoNombre = (string)reader["fcSegundoNombre"],
                                    primerApellido = (string)reader["fcPrimerApellido"],
                                    segundoApellido = (string)reader["fcSegundoApellido"],
                                    telefono = (string)reader["fcTelefono"],
                                    obligaciones = Decimal.Parse(reader["fnTotalObligaciones"].ToString()),
                                    ingresos = Decimal.Parse(reader["fnIngresos"].ToString()),
                                    disponible = Decimal.Parse(reader["fnCapacidadDisponible"].ToString()),
                                    fechaNacimiento = DateTime.Parse(reader["fdFechadeNacimiento"].ToString())
                                };
                            }
                        }
                    }

                    using (SqlCommand cmd = new SqlCommand("sp_CotizadorProductos", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@piIDUsuario", 1);
                        cmd.Parameters.AddWithValue("@piIDProducto", 101);
                        cmd.Parameters.AddWithValue("@pcIdentidad", objPrecalificado.identidad);
                        cmd.Parameters.AddWithValue("@piConObligaciones", objPrecalificado.obligaciones == 0 ? "0" : "1");
                        cmd.Parameters.AddWithValue("@pnIngresosBrutos", objPrecalificado.ingresos);
                        cmd.Parameters.AddWithValue("@pnIngresosDisponibles", objPrecalificado.disponible);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            int IDContador = 1;
                            while (reader.Read())
                            {
                                listaCotizadorProductos.Add(new cotizadorProductosViewModel()
                                {
                                    IDCotizacion = IDContador,
                                    IDProducto = (int)reader["fiIDProducto"],
                                    ProductoDescripcion = reader["fcProdDesc"].ToString(),
                                    fnMontoOfertado = decimal.Parse(reader["fnMontoOfertado"].ToString()),
                                    fiPlazo = int.Parse(reader["fiPlazo"].ToString()),
                                    fnCuotaQuincenal = decimal.Parse(reader["fnCuotaQuincenal"].ToString())
                                });
                                IDContador += 1;
                            }
                        }
                    }
                }
                objPrecalificado.cotizadorProductos = listaCotizadorProductos;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            return objPrecalificado;
        }
    }
}