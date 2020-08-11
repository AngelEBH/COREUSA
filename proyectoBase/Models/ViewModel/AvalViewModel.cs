using System.Collections.Generic;

namespace proyectoBase.Models.ViewModel
{
    //ESTE VIEW MODEL ES PARA LA PANTALLA "AVAL_DETALLES.ASPX"
    public class AvalViewModel
    {
        public AvalMasterViewModel AvalMaster { get; set; }
        public AvalInformacionConyugalViewModel AvalInformacionConyugal { get; set; }
        public AvalInformacionDomiciliarViewModel AvalInformacionDomiciliar { get; set; }
        public AvalInformacionLaboralViewModel AvalInformacionLaboral { get; set; }
        public List<SolicitudesDocumentosViewModel> AvalDocumentos { get; set; }
    }
}