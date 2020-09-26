using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace proyectoBase.Forms.Negociaciones
{
    public partial class Negociaciones_Registrar : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Now.ToString("MM/dd/yyyy");
            ddlPlazos.Items.Clear();
            ddlPlazos.Items.Add("12 Meses");
            ddlPlazos.Items.Add("18 Meses");
            ddlPlazos.Items.Add("24 Meses");
            ddlPlazos.Items.Add("30 Meses");



            ddlMarca.Items.Clear();
            ddlMarca.Items.Add("Toyota");

            ddlModelo.Items.Clear();
            ddlModelo.Items.Add("Corolla");



            ddlOrigen.Items.Clear();
            ddlOrigen.Items.Add("GENERAL");


            ddlAutolote.Items.Clear();
            ddlAutolote.Items.Add("INTERNO");
        }
    }
}