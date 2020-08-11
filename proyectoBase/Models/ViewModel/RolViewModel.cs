using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class RolViewModel
    {
        public int rol_Id { get; set; }
        public string rol_Descripcion { get; set; }
        public Nullable<int> rol_UsuarioCrea { get; set; }
        public Nullable<System.DateTime> rol_FechaCrea { get; set; }
        public Nullable<int> rol_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> rol_FechaModifica { get; set; }
        public bool rol_Estado { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public List<PantallasPorRolViewModel> ListaPantallas { get; set; }
    }

}