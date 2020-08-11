using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class VendedoresViewModel
    {
        //info vendedor
        public int fiIDVendedor { get; set; }
        public int fiIDAgencia { get; set; }
        public int fiIDUsuarioVendedor { get; set; }
        public bool fbEstado { get; set; }

        public string fcNombreAgencia { get; set; }
        public string fcUbicacionAgencia { get; set; }


        //info usuario del vendedor
        public int usu_Id { get; set; }
        public string usu_NombreUsuario { get; set; }
        public string usu_Nombres { get; set; }
        public string usu_Apellidos { get; set; }
        public string usu_Correos { get; set; }
        public string usu_PasswordString { get; set; }
        public bool usu_EsActivo { get; set; }
        public string usu_RazonInactivo { get; set; }
        public bool usu_EsAdministrador { get; set; }
        public Nullable<byte> usu_SesionesValidas { get; set; }

        //data de auditoria del usuario del vendedor
        public Nullable<int> usu_UsuarioCrea { get; set; }
        public Nullable<System.DateTime> usu_FechaCrea { get; set; }
        public Nullable<int> usu_UsuarioModifica { get; set; }
        public Nullable<System.DateTime> usu_FechaModifica { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public string fcNombreUsuarioModifica { get; set; }

        //roles del vendedor
        public List<RolesPorUsuarioViewModel> ListaRoles { get; set; }
    }
}