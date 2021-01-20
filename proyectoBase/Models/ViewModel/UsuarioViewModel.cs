using System;
using System.Collections.Generic;

namespace proyectoBase.Models.ViewModel
{
    public class UsuarioViewModel
    {
        public int usu_Id { get; set; }
        public string usu_NombreUsuario { get; set; }
        public byte[] usu_Password { get; set; }
        public string usu_PasswordString { get; set; }
        public string usu_Nombres { get; set; }
        public string usu_Apellidos { get; set; }
        public string usu_Correos { get; set; }
        public bool usu_EsActivo { get; set; }
        public string usu_RazonInactivo { get; set; }
        public bool usu_EsAdministrador { get; set; }
        public Nullable<byte> usu_SesionesValidas { get; set; }
        public Nullable<int> usu_UsuarioCrea { get; set; }
        public Nullable<System.DateTime> usu_FechaCrea { get; set; }
        public Nullable<int> usu_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> usu_FechaModifica { get; set; }
        public List<RolesPorUsuarioViewModel> ListaRoles { get; set; }
    }
}