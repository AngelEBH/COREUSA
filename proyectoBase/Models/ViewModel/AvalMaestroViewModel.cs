using System;

namespace proyectoBase.Models.ViewModel
{
    public class AvalMaestroViewModel
    {
        public int fiIDSolicitud { get; set; }
        public int TipoAval { get; set; }
        public int fiIDCliente { get; set; }
        public int fiIDAval { get; set; }
        public string fcIdentidadAval { get; set; }
        public string RTNAval { get; set; }
        public string fcTelefonoAval { get; set; }
        public int fiNacionalidad { get; set; }
        public System.DateTime fdFechaNacimientoAval { get; set; }
        public string fcCorreoElectronicoAval { get; set; }
        public string fcProfesionOficioAval { get; set; }
        public string fcSexoAval { get; set; }
        public int fiIDEstadoCivil { get; set; }
        public int fiIDVivienda { get; set; }
        public Nullable<int> fiTiempoResidir { get; set; }
        public bool fbAvalActivo { get; set; }
        public string fcRazonInactivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public string fcPrimerNombreAval { get; set; }
        public string fcSegundoNombreAval { get; set; }
        public string fcPrimerApellidoAval { get; set; }
        public string fcSegundoApellidoAval { get; set; }

        //nacionalidad del Aval
        public string fcDescripcionNacionalidad { get; set; }
        public bool fbNacionalidadActivo { get; set; }

        public string fcDescripcionEstadoCivil { get; set; }
        public bool fbEstadoCivilActivo { get; set; }

        public string fcDescripcionVivienda { get; set; }
        public bool fbViviendaActivo { get; set; }
        public int fiTipoAval { get; set; }
    }
}