using System;

namespace proyectoBase.Models.ViewModel
{
    public class ClienteAvalesViewModel
    {
        public int fiIDAval { get; set; }
        public int fiIDCliente { get; set; }
        public int fiIDSolicitud { get; set; }
        public string fcIdentidadAval { get; set; }
        public string RTNAval { get; set; }
        public string fcPrimerNombreAval { get; set; }
        public string fcSegundoNombreAval { get; set; }
        public string fcPrimerApellidoAval { get; set; }
        public string fcSegundoApellidoAval { get; set; }
        public string fcTelefonoAval { get; set; }
        public DateTime fdFechaNacimientoAval { get; set; }
        public string fcCorreoElectronicoAval { get; set; }
        public string fcProfesionOficioAval { get; set; }
        public string fcSexoAval { get; set; }
        public bool fbAvalActivo { get; set; }
        public string fcRazonInactivo { get; set; }
        public int fiTipoAval { get; set; }
        public string fcNombreTrabajo { get; set; }
        public string fdTelefonoEmpresa { get; set; }
        public string fcExtensionRecursosHumanos { get; set; }
        public string fcExtensionAval { get; set; }
        public decimal fiIngresosMensuales { get; set; }
        public string fcPuestoAsignado { get; set; }
        public DateTime fcFechaIngreso { get; set; }
        //auditoria
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public DateTime fdFechaCrea { get; set; }
        public int fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public DateTime fdFechaUltimaModifica { get; set; }
    }
}