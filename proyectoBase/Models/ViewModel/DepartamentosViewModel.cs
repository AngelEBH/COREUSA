using System;

namespace UI.Web.Models.ViewModel
{
    public class DepartamentosViewModel
    {
        public int fiIDDepto { get; set; }
        public string fcNombreDepto { get; set; }
        public bool fbDepartamentoActivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public DateTime fdFechaCrea { get; set; }
        public int? fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public DateTime? fdFechaUltimaModifica { get; set; }
    }
}