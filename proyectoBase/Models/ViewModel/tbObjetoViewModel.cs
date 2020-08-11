using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class tbObjetoViewModel
    {
        public int obj_Id { get; set; }
        public string obj_Pantalla { get; set; }
        public string obj_Referencia { get; set; }
        public string obj_SubMenu { get; set; }
        public Nullable<int> obj_UsuarioCrea { get; set; }
        public Nullable<System.DateTime> obj_FechaCrea { get; set; }
        public Nullable<int> obj_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> obj_FechaModifica { get; set; }
    }
}