using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using static proyectoBase.Forms.Aval.Aval_Registrar;

namespace UI.Web.Models.ViewModel
{
    public class SolicitudIngresarDDLViewModel
    {
        public List<DepartamentosViewModel> Departamentos { get; set; }
        public List<MunicipiosViewModel> Municipios { get; set; }
        public List<CiudadesViewModel> Ciudades { get; set; }
        public List<BarriosColoniasViewModel> BarriosColonias { get; set; }

        public List<CodigoPostalViewModal> CodigoPostal { get; set; }

        public List<EstadosCivilesViewModel> EstadosCiviles { get; set; }
        public List<NacionalidadesViewModel> Nacionalidades { get; set; }
        public List<TipoPrestamoViewModel> TipoPrestamo { get; set; }
        public List<ViviendaViewModel> Vivienda { get; set; }
        public List<ParentescosViewModel> Parentescos { get; set; }
        public List<TipoDocumentoViewModel> TipoDocumento { get; set; }
    }
}