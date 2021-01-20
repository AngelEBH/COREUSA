using System;

namespace proyectoBase.Models.ViewModel
{
    public class AccesoRolViewModel
    {
        public int acrol_Id { get; set; }
        public Nullable<int> rol_Id { get; set; }
        public Nullable<int> obj_Id { get; set; }
        public Nullable<int> acrol_UsuarioCrea { get; set; }
        public Nullable<System.DateTime> acrol_FechaCrea { get; set; }
        public Nullable<int> acrol_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> acrol_FechaModifica { get; set; }
    }
}