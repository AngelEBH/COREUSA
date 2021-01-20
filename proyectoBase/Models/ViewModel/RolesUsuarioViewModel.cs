using System;

namespace proyectoBase.Models.ViewModel
{
    public class RolesUsuarioViewModel
    {
        public int rolu_Id { get; set; }
        public Nullable<int> rol_Id { get; set; }
        public Nullable<int> usu_Id { get; set; }
        public Nullable<int> rolu_UsuarioCrea { get; set; }
        public Nullable<System.DateTime> rolu_FechaCrea { get; set; }
        public Nullable<int> rolu_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> rolu_FechaModifica { get; set; }
    }
}