using System.Collections.Generic;
using UI.Web.Models.ViewModel;

namespace proyectoBase.Models.ViewModel
{
    public class DropDownListsViewModel
    {
        public List<DepartamentosViewModel> Departamentos { get; set; }
        public List<MunicipiosViewModel> Municipios { get; set; }
        public List<CiudadesViewModel> Ciudades { get; set; }
        public List<BarriosColoniasViewModel> BarriosColonias { get; set; }

        public List<EstadosCivilesViewModel> EstadosCiviles { get; set; }
        public List<NacionalidadesViewModel> Nacionalidades { get; set; }
        public List<TipoPrestamoViewModel> TipoPrestamo { get; set; }
        public List<ViviendaViewModel> Vivienda { get; set; }
    }
}