using System;

namespace proyectoBase.Models.ViewModel
{
    public class AvalInformacionDomicilioViewModel
    {
        public int fiIDInformacionDomicilioAval { get; set; }
        public int fiIDAval { get; set; }
        public int fiIDSolicitud { get; set; }
        public string fcTelefonoCasa { get; set; }
        public string fcDireccionDetallada { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public Nullable<int> fiIDBarrioColonia { get; set; }
        public string fcReferenciasDireccionDetallada { get; set; }

        //colonia del Aval
        public string fcNombreBarrioColonia { get; set; }
        public bool fbBarrioColoniaActivo { get; set; }

        //ciudad del Aval
        public int fiIDCiudad { get; set; }
        public string fcNombreCiudad { get; set; }
        public bool fbCiudadActivo { get; set; }

        //ciudad del Aval
        public int fiIDMunicipio { get; set; }
        public string fcNombreMunicipio { get; set; }
        public bool fbMunicipioActivo { get; set; }

        //departamento del Aval
        public int fiIDDepto { get; set; }
        public string fcNombreDepto { get; set; }
        public bool fbDepartamentoActivo { get; set; }

        // proceso de campo
        public string fcLatitud { get; set; }
        public string fcLongitud { get; set; }
        public int fiIDGestorValidador { get; set; }
        public string fcGestorValidadorDomicilio { get; set; }
        public int fiIDInvestigacionDeCampo { get; set; }
        public string fcGestionDomicilio { get; set; }
        public int IDTipoResultado { get; set; }
        public string fcResultadodeCampo { get; set; }
        public DateTime fdFechaValidacion { get; set; }
        public string fcObservacionesCampo { get; set; }
        public int fiIDEstadoDeGestion { get; set; }
        public int fiEstadoDomicilio { get; set; }
    }
}