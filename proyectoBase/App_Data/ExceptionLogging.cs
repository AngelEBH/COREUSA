using System;
using context = System.Web.HttpContext;
using System.Data.SqlClient;
using System.Data;
/// <summary>  
/// Summary description for ExceptionLogging  
/// article by Vithal Wadje  

/// </summary>  
public static class ExceptionLogging
{
    public static string SendExcepToDB(Exception exdb)
    {
        string resultado = "(ExceptionLogging.cs/SendExcepToDB) INICIO";
        try
        {
            string sqlConnectionString = "Data Source=172.20.3.150;Initial Catalog = CoreFinanciero; User ID = SA; Password = Password2009;Max Pool Size=200;MultipleActiveResultSets=true";
            SqlConnection sqlConexion = new SqlConnection(sqlConnectionString);
            string exepurl = context.Current.Request.Url.ToString();
            SqlCommand com = new SqlCommand("CoreFinanciero.dbo.CapturarExcepciones", sqlConexion);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@ExceptionMsg", exdb.Message.ToString());
            com.Parameters.AddWithValue("@ExceptionType", exdb.GetType().Name.ToString());
            com.Parameters.AddWithValue("@ExceptionURL", exepurl);
            com.Parameters.AddWithValue("@ExceptionSource", exdb.StackTrace.ToString());
            sqlConexion.Open();
            com.ExecuteNonQuery();
            resultado = "(ExceptionLogging.cs/SendExcepToDB)LA EXCEPCION SE REGISTRÓ EN LA BD";
        }
        catch (Exception ex)
        {
            resultado = "(ExceptionLogging.cs/SendExcepToDB)ERROR AL REGISTRAR LA EXCEPCION EN LA BD" + ex.Message.ToString();
        }
        return resultado;
    }
}