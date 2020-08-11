using System;

namespace UI.Web.Models.ViewModel
{
    public class EstadosCivilesViewModel
    {
        public int fiIDEstadoCivil { get; set; }
        public string fcDescripcionEstadoCivil { get; set; }
        public bool fbEstadoCivilActivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public Nullable<bool> fbRequiereInformacionConyugal { get; set; }
    }
}