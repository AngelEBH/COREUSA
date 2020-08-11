using proyectoBase.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UI.Web.Models.ViewModel
{
    public class CiudadesViewModel
    {
        //municipio al que pertenece el barrio/colonia
        public string fcNombreMunicipio { get; set; }
        public bool fbMunicipioActivo { get; set; }

        //departamento al que pertenece el barrio/colonia
        public int? fiIDDepto { get; set; }
        public string fcNombreDepto { get; set; }
        public bool fbDepartamentoActivo { get; set; }

        //informacion de la ciudad
        public int fiIDCiudad { get; set; }
        public int fiIDMunicipio { get; set; }
        public string fcCodigoPostal { get; set; }
        public string fcNombreCiudad { get; set; }
        public bool fbCiudadActivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
    }
}