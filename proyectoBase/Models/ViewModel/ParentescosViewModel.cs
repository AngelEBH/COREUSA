using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class ParentescosViewModel
    {
        public int fiIDParentescos { get; set; }
        public string fcDescripcionParentesco { get; set; }
        public bool fbParentescoActivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
    }
}