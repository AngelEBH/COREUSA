using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class RolesPorUsuarioViewModel
    {
        public int usu_Id { get; set; }
        public string usu_NombreUsuario { get; set; }
        public byte[] usu_Password { get; set; }
        public string usu_Nombres { get; set; }
        public string usu_Apellidos { get; set; }
        public string usu_Correos { get; set; }
        public bool usu_EsActivo { get; set; }
        public string usu_RazonInactivo { get; set; }
        public bool usu_EsAdministrador { get; set; }
        public Nullable<byte> usu_SesionesValidas { get; set; }
        public int rolu_Id { get; set; }
        public Nullable<int> rol_Id { get; set; }
        public Nullable<int> rolu_UsuarioCrea { get; set; }
        public Nullable<System.DateTime> rolu_FechaCrea { get; set; }
        public Nullable<int> rolu_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> rolu_FechaModifica { get; set; }
        public string rol_Descripcion { get; set; }
        public Nullable<int> rol_UsuarioCrea { get; set; }
        public Nullable<System.DateTime> rol_FechaCrea { get; set; }
        public Nullable<int> rol_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> rol_FechaModifica { get; set; }
        public bool rol_Estado { get; set; }
    }
}