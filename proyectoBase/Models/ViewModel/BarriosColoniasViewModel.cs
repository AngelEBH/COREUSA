using System;

namespace proyectoBase.Models.ViewModel
{
    public class BarriosColoniasViewModel
    {
        // ciudad a la que pertenece el barrio/colonia
        public int? fiIDCiudad { get; set; }
        public string fcNombreCiudad { get; set; }
        public bool fbCiudadActivo { get; set; }

        // municipio al que pertenece el barrio/colonia
        public int? fiIDMunicipio { get; set; }
        public string fcNombreMunicipio { get; set; }
        public bool fbMunicipioActivo { get; set; }

        // departamento al que pertenece el barrio/colonia
        public int? fiIDDepto { get; set; }
        public string fcNombreDepto { get; set; }
        public bool fbDepartamentoActivo { get; set; }

        // informacion del barrio/colonia
        public int? fiIDBarrioColonia { get; set; }
        public string fcNombreBarrioColonia { get; set; }
        public bool fbBarrioColoniaActivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
    }
}