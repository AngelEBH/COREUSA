using System;

namespace proyectoBase.Models.ViewModel
{
    public class AgenciasViewModel
    {
        public int fiIDAgencia { get; set; }
        public string fcNombreAgencia { get; set; }
        public string fcUbicacionAgencia { get; set; }
        public bool fbEstado { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
    }
}