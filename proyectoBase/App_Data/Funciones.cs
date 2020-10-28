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
using System.IO;
using System.Net;
using System.Security;
using System.Security.Principal;
using System.Security.Authentication;


/// <summary>
/// Summary description for Funciones
/// </summary>
public class Funciones
{
    public Funciones()
    {
        //
        // TODO: Add constructor logic here
        //

    }

    public class FuncionesComunes
    {
        public string FechaHoy()
        {
            string lcFechaActual = "";
            string[] lacDiadelaSemana = { "Domingo", "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado" };
            string[] lacMes = { "Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio", "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre" };

            lcFechaActual = lacDiadelaSemana[Convert.ToInt16(DateTime.Now.DayOfWeek)] + ", " + DateTime.Now.Day.ToString() + " de " +
                lacMes[Convert.ToInt16(DateTime.Now.Month) - 1] + " de " + DateTime.Now.Year.ToString();

            return lcFechaActual;
        }

        public string GenerarMD5Hash(string pcCadena)
        {
            // Calculamos el MD5 Hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(pcCadena);
            byte[] hash = md5.ComputeHash(inputBytes);

            // Convertir byte array a hex string
            StringBuilder lsBuilder = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                lsBuilder.Append(hash[i].ToString("X2"));
            }
            return lsBuilder.ToString();
        }

        public void TransferirXML(string pcIdentidadCandidato)
        {
            WebClient wcConfiar = new WebClient();

            string lcXMLEquifax = pcIdentidadCandidato.Trim() + ".xml";
            string lcUbicacionArchivoLocalXML = @"C:\ArchivosXML\" + lcXMLEquifax.Trim();
            string fcURLPrestaditoCache = @"http://www.miprestadito.com/2.0/modulos/Creditos/cache/" + lcXMLEquifax.Trim();
            string lcURLRepositorioFTP = "ftp://172.20.3.150/" + lcXMLEquifax;

            wcConfiar.DownloadFile(fcURLPrestaditoCache, lcUbicacionArchivoLocalXML);

            wcConfiar.Credentials = new NetworkCredential("usuarioFTP", "ftp123*");

            wcConfiar.UploadFile(lcURLRepositorioFTP, WebRequestMethods.Ftp.UploadFile, lcUbicacionArchivoLocalXML);
        }

        public string UsrPaginaWeb(string pcIDApp, string piIDUsuario, string piIDModulo)
        {
            String lcPaginaUsuario = "";
            String lcSQLInstruccion = "";
            String sqlConnectionString = "";
            SqlConnection sqlConexion = null;
            SqlCommand sqlComando = null;
            SqlDataReader slqReader = null;

            DSCore.DataCrypt DSC = new DSCore.DataCrypt();

            sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;

            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));
            try
            {
                sqlConexion.Open();
                lcSQLInstruccion = "exec Coreseguridad.dbo.sp_OpcionesWeb " + pcIDApp.Trim() + "," + piIDUsuario.Trim() + "," + piIDModulo.Trim();
                sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
                sqlComando.CommandType = CommandType.Text;
                slqReader = sqlComando.ExecuteReader();
                slqReader.Read();

                lcPaginaUsuario = slqReader["fcPaginaWeb"].ToString().Trim();

                sqlComando.Dispose();
                sqlConexion.Close();
            }
            catch (Exception ex)
            {
                lcPaginaUsuario = "";
            }
			finally
			{
				sqlConexion.Close();
			}

            return lcPaginaUsuario;
        }


        public Boolean ValReturnar(string pcValor)
        {
            Boolean lbRespuesta=false;

            if (pcValor == "1")
                lbRespuesta = true;

            return lbRespuesta;
        }

    }

    public class Convertidores
    {
        public Boolean CaracteraLogico(string pcValor)
        {
            Boolean lbRespuesta = false;

            if (pcValor == "1")
                lbRespuesta = true;

            return lbRespuesta;
        }
    }

    public string NombreMesCorto(string pcMes)
    {
        string fcMesCorto = "";

        if (pcMes == "1")
            fcMesCorto = "Ene";
        if (pcMes == "2")
            fcMesCorto = "Feb";
        if (pcMes == "3")
            fcMesCorto = "Mar";
        if (pcMes == "4")
            fcMesCorto = "Abr";
        if (pcMes == "5")
            fcMesCorto = "May";
        if (pcMes == "6")
            fcMesCorto = "Jun";
        if (pcMes == "7")
            fcMesCorto = "Jul";
        if (pcMes == "8")
            fcMesCorto = "Ago";
        if (pcMes == "9")
            fcMesCorto = "Sep";
        if (pcMes == "10")
            fcMesCorto = "Oct";
        if (pcMes == "11")
            fcMesCorto = "Nov";
        if (pcMes == "12")
            fcMesCorto = "Dic";
        return fcMesCorto;
    }

    public class FuncionesdeSesion
    {
        public string[] IniciarSesion(string piIDApp, string pcUsuario, string pcPassword, string pcIPUsuario)
        {
            DSCore.DataCrypt DSC = new DSCore.DataCrypt();
            String lcSQLInstruccion = "";
            String sqlConnectionString = "";
            SqlConnection sqlConexion = null;
            SqlDataReader sqlResultado = null;
            SqlCommand sqlComando = null;

            string[] lcAcceso = new string[3];

            sqlConnectionString = ConfigurationManager.ConnectionStrings["ConexionEncriptada"].ConnectionString;
            sqlConexion = new SqlConnection(DSC.Desencriptar(sqlConnectionString));

            try
            {
                sqlConexion.Open();

                //lcSQLInstruccion = "exec CoreSeguridad.dbo.sp_MasterLogin " + piIDApp + ",'" + pcUsuario.Trim() + "','" + pcPassword.Trim() + "','" + pcIPUsuario + "'";
                lcSQLInstruccion = "exec CoreSeguridad.dbo.sp_MasterLogin " + piIDApp + ",'" + pcUsuario.Trim() + "','" + pcPassword.Trim() + "'";
                sqlComando = new SqlCommand(lcSQLInstruccion, sqlConexion);
                sqlComando.CommandType = CommandType.Text;
                sqlResultado = sqlComando.ExecuteReader();
                sqlResultado.Read();

                lcAcceso[0] = sqlResultado["fiIDUsuario"].ToString().Trim();
                lcAcceso[1] = sqlResultado["fcMensaje"].ToString().Trim();
                lcAcceso[2] = sqlResultado["fiIDSesion"].ToString().Trim();

                sqlComando.Dispose();
            }
            catch (Exception ex)
            {
                lcAcceso[0] = "-1";
                lcAcceso[1] = ex.Message;
                lcAcceso[2] = "2";
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return lcAcceso;
        }

        public string FinalizarSesion(string piIDApp, int piIDSesion, string pcIDUsuario)
        {

            return " ";
        }

    }

    public class MultiNombres
    {
        public static string[] GenerarNombreCredDocumento(int piTipoDocumento, int piSolicitud, int piIDCatalogoDocumento, int piBloque)
        {
            string lcPrimerBloque = "";
            string lcBloqueFechaHora = "";
            string[] lcRespuesta;

            lcRespuesta = new string[piBloque];

            if (piTipoDocumento == 1)
                lcPrimerBloque = "CTE";
            else
                lcPrimerBloque = "AVAL";

            for (int x = 0; x < piBloque; x++)
            {
                lcBloqueFechaHora = System.DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
                lcBloqueFechaHora = lcBloqueFechaHora.Replace("-", "");
                lcBloqueFechaHora = lcBloqueFechaHora.Replace(":", "");
                lcBloqueFechaHora = lcBloqueFechaHora.Replace(" ", "T");
                lcRespuesta[x] = lcPrimerBloque + "S" + piSolicitud.ToString().Trim() + "D" + piIDCatalogoDocumento.ToString().Trim() + "-" + lcBloqueFechaHora + "-" + x.ToString().Trim();
            }

            //lcRespuesta = lcPrimerBloque;

            return lcRespuesta;
        }
    }
}