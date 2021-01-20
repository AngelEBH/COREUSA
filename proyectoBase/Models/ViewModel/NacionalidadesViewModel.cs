using System;

namespace UI.Web.Models.ViewModel
{
    public class NacionalidadesViewModel
    {
        public int fiIDNacionalidad { get; set; }
        public string fcDescripcionNacionalidad { get; set; }
        public bool fbNacionalidadActivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
    }
}