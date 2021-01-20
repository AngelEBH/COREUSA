using System;
using context = System.Web.HttpContext;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

/// <summary>  
/// Guardar errores en tiempo de ejecución en la base de datos para tener registro de los mismos.
/// </summary>  
public static class ExceptionLogging
{
    public static string SendExcepToDB(Exception exdb)
    {
        var DSC = new DSCore.DataCrypt();
        var resultado = "(ExceptionLogging.cs/SendExcepToDB) Paso 1: Inicio.";
        try
        {
            using (var sqlConexion = new SqlConnection(DSC.Desencriptar(ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString)))
            {
                sqlConexion.Open();

                var urlActual = context.Current.Request.Url.ToString();

                using (var sqlComando = new SqlCommand("CoreFinanciero.dbo.CapturarExcepciones", sqlConexion))
                {
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@ExceptionMsg", exdb.Message.ToString());
                    sqlComando.Parameters.AddWithValue("@ExceptionType", exdb.GetType().Name.ToString());
                    sqlComando.Parameters.AddWithValue("@ExceptionURL", urlActual);
                    sqlComando.Parameters.AddWithValue("@ExceptionSource", exdb.StackTrace.ToString());
                    sqlComando.ExecuteNonQuery();

                    resultado = "(ExceptionLogging.cs/SendExcepToDB) Paso 2: La excepción se registró correctamente.";
                }
            }
        }
        catch (Exception ex)
        {
            resultado = "(ExceptionLogging.cs/SendExcepToDB) Error al registrar la excepción: " + ex.Message.ToString();
        }
        return resultado;
    }
}