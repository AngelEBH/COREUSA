using System;

namespace UI.Web.Models.ViewModel
{
    public class TipoPrestamoViewModel
    {
        public int fiIDTipoPrestamo { get; set; }
        public string fcDescripcion { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
    }
}