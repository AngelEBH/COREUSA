using System;

namespace proyectoBase.Models.ViewModel
{
    public class SolicitudesDocumentosViewModel
    {
        public int fiIDSolicitudDocs { get; set; }
        public int fiIDSolicitud { get; set; }
        public string fcNombreArchivo { get; set; }
        public string NombreAntiguo { get; set; }
        public string DescripcionTipoDocumento { get; set; }
        public string fcTipoArchivo { get; set; }
        public string fcRutaArchivo { get; set; }
        public string URLArchivo { get; set; }
        public string URLAntiguoArchivo { get; set; }
        public byte fcArchivoActivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public Nullable<int> fiTipoDocumento { get; set; }
    }
}