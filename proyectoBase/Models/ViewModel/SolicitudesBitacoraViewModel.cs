using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class SolicitudesBitacoraViewModel
    {
        public int fiIDBitacora { get; set; }
        public int fiIDSolicitud { get; set; }
        public string fcTiempoTotalTranscurrido { get; set; }
        public Nullable<System.DateTime> fdEnIngresoInicio { get; set; }
        public Nullable<System.DateTime> fdEnIngresoFin { get; set; }
        public Nullable<System.DateTime> fdEnTramiteInicio { get; set; }
        public Nullable<System.DateTime> fdEnTramiteFin { get; set; }
        public Nullable<System.DateTime> fdEnAnalisisInicio { get; set; }
        public Nullable<System.DateTime> ftAnalisisTiempoValidarInformacionPersonal { get; set; }
        public Nullable<System.DateTime> ftAnalisisTiempoValidarDocumentos { get; set; }
        public Nullable<System.DateTime> ftAnalisisTiempoValidacionReferenciasPersonales { get; set; }
        public Nullable<System.DateTime> ftTiempoTomaDecisionFinal { get; set; }
        public Nullable<System.DateTime> fdEnAnalisisFin { get; set; }
        public Nullable<bool> fbSolicitudAprobada { get; set; }
        public Nullable<System.DateTime> fdCondicionadoInicio { get; set; }
        public string fcCondicionadoComentario { get; set; }
        public Nullable<System.DateTime> fdCondificionadoFin { get; set; }
        public Nullable<System.DateTime> fdEnCampoInicio { get; set; }
        public Nullable<System.DateTime> fdEnCampoFin { get; set; }
        public Nullable<System.DateTime> fdReprogramadoInicio { get; set; }
        public string fcReprogramadoComentario { get; set; }
        public Nullable<System.DateTime> fdReprogramadoFin { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public Nullable<System.DateTime> ftAnalisisTiempoValidarInformacionLaboral { get; set; }
        public string fcComentarioValidacionInfoPersonal { get; set; }
        public string fcComentarioValidacionDocumentacion { get; set; }
        public string fcComentarioValidacionReferenciasPersonales { get; set; }
        public string fcComentarioValidacionInfoLaboral { get; set; }
        public string fcObservacionesDeCredito { get; set; }
        public string fcObservacionesDeGestoria { get; set; }
        public string fcComentarioResolucion { get; set; }
        public Nullable<bool> fbValidacionDocumentcionIdentidades { get; set; }
        public Nullable<bool> fbValidacionDocumentacionDomiciliar { get; set; }
        public Nullable<bool> fbValidacionDocumentacionLaboral { get; set; }
        public Nullable<bool> fbValidacionDocumentacionSolicitudFisica { get; set; }
    }
}