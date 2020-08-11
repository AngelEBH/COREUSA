using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class SolicitudesCondicionamientosViewModel
    {
        public int fiIDSolicitudCondicion { get; set; }
        public int fiIDCondicion { get; set; }
        public string fcCondicion { get; set; }
        public string fcDescripcionCondicion { get; set; }
        public int fiIDSolicitud { get; set; }
        public string fcComentarioAdicional { get; set; }
        public bool fbEstadoCondicion { get; set; }
        
    }
}