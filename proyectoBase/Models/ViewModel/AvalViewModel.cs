using System.Collections.Generic;

namespace proyectoBase.Models.ViewModel
{
    public class AvalViewModel
    {
        public AvalMaestroViewModel AvalMaster { get; set; }
        public AvalInformacionConyugalViewModel AvalInformacionConyugal { get; set; }
        public AvalInformacionDomicilioViewModel AvalInformacionDomiciliar { get; set; }
        public AvalInformacionLaboralViewModel AvalInformacionLaboral { get; set; }
        public List<SolicitudesDocumentosViewModel> AvalDocumentos { get; set; }
    }
}