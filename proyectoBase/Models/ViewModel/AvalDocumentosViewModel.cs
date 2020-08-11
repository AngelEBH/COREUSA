using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class AvalDocumentosViewModel
    {
        public int fiIDAvalDocs { get; set; }
        public int fiIDAval { get; set; }
        public string fcNombreArchivo { get; set; }
        public string fcTipoArchivo { get; set; }
        public string fcRutaArchivo { get; set; }
        public bool fcArchivoActivo { get; set; }
        public Nullable<int> fiTipoDocumento { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
    }
}