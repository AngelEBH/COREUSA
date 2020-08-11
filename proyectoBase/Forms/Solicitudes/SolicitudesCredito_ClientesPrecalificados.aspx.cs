using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;

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
            string connectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = WebUser; Password = WebUser123*;Max Pool Size=200;MultipleActiveResultSets=true";
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(connectionString);                
                SqlCommand cmd = new SqlCommand("EXEC CoreAnalitico.dbo.sp_Vendedor_ListaClientesEstados @piIDApp, @piIDUsuario, @piResultadoPrecalificado", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@piIDApp", 103);
                cmd.Parameters.AddWithValue("@piIDUsuario", 1);
                cmd.Parameters.AddWithValue("@piResultadoPrecalificado", 1);
                conn.Open();
                reader = cmd.ExecuteReader();
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
            finally
            {
                if (conn != null)
                    conn.Close();
                if (reader != null)
                    reader.Close();
            }
            return listaClientesPrecalificados;
        }

        [WebMethod]
        public static PrecalificadoViewModel GetDetallesPrecalificado(string identidad)
        {
            PrecalificadoViewModel objPrecalificado = null;
            List<cotizadorProductosViewModel> listaCotizadorProductos = new List<cotizadorProductosViewModel>();
            string connectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = WebUser; Password = WebUser123*;Max Pool Size=200;MultipleActiveResultSets=true";
            SqlConnection conn = null;
            SqlDataReader reader = null;
            try
            {
                conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand("EXEC CoreAnalitico.dbo.sp_info_ConsultaEjecutivos @piIDApp, @piIDUsuario, @pcIdentidad", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@piIDApp", 103);
                cmd.Parameters.AddWithValue("@piIDUsuario", 1);
                cmd.Parameters.AddWithValue("@pcIdentidad", identidad);
                cmd.CommandTimeout = 60;
                conn.Open();
                reader = cmd.ExecuteReader();
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
                cmd = new SqlCommand("EXEC CoreFinanciero.dbo.sp_CotizadorProductos @piIDUsuario, @piIDProducto, @pcIdentidad, @piConObligaciones, @pnIngresosBrutos, @pnIngresosDisponibles", conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@piIDUsuario", 1);
                cmd.Parameters.AddWithValue("@piIDProducto", 101);
                cmd.Parameters.AddWithValue("@pcIdentidad", objPrecalificado.identidad);
                cmd.Parameters.AddWithValue("@piConObligaciones", objPrecalificado.obligaciones == 0 ? "0" : "1");
                cmd.Parameters.AddWithValue("@pnIngresosBrutos", objPrecalificado.ingresos);
                cmd.Parameters.AddWithValue("@pnIngresosDisponibles", objPrecalificado.disponible);
                reader = cmd.ExecuteReader();
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
                objPrecalificado.cotizadorProductos = listaCotizadorProductos;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
            finally
            {
                if (conn != null)
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                if (reader != null)
                    reader.Close();
            }
            return objPrecalificado;
        }
    }
}