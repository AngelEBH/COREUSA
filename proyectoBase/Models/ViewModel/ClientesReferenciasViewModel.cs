using System;

namespace proyectoBase.Models.ViewModel
{
    public class ClientesReferenciasViewModel
    {
        public int fiIDReferencia { get; set; }
        public int fiIDCliente { get; set; }
        public string fcNombreCompletoReferencia { get; set; }
        public string fcLugarTrabajoReferencia { get; set; }
        public short fiTiempoConocerReferencia { get; set; }
        public string fcTelefonoReferencia { get; set; }
        public int fiIDParentescoReferencia { get; set; }
        public string fcDescripcionParentesco { get; set; }
        public bool fbReferenciaActivo { get; set; }
        public string fcRazonInactivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public string fcComentarioDeptoCredito { get; set; }
        public Nullable<int> fiAnalistaComentario { get; set; }
    }
}