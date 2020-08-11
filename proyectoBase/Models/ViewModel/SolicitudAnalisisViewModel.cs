using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class SolicitudAnalisisViewModel
    {
        public ClientesViewModel cliente { get; set; }
        public BandejaSolicitudesViewModel solicitud { get; set; }
        public List<SolicitudesDocumentosViewModel> documentos { get; set; }
    }
}