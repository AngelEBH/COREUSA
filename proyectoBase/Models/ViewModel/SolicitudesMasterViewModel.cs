using System;

namespace proyectoBase.Models.ViewModel
{
    public class SolicitudesMasterViewModel
    {
        public int fiIDSolicitud { get; set; }
        public string fcNoSolicitud { get; set; }
        public int fiIDCliente { get; set; }
        public int fiIDTipoPrestamo { get; set; }
        public string fcTipoSolicitud { get; set; }
        public System.DateTime fdFechaCreacionSolicitud { get; set; }
        public string fcTipoEmpresa { get; set; }
        public string fcTipoPerfil { get; set; }
        public string fcTipoEmpleado { get; set; }
        public string fcBuroActual { get; set; }
        public Nullable<decimal> fiMontoFinalSugerido { get; set; }
        public int IDTipoMoneda { get; set; }
        public Nullable<decimal> fiMontoFinalFinanciar { get; set; }
        public Nullable<int> fiPlazoFinalAprobado { get; set; }
        public Nullable<int> fiEstadoSolicitud { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public byte fiSolicitudActiva { get; set; }
        public decimal fdValorPmoSugeridoSeleccionado { get; set; }
        public int fiPlazoPmoSeleccionado { get; set; }
        public decimal fdIngresoPrecalificado { get; set; }
        public decimal fdObligacionesPrecalificado { get; set; }
        public decimal fdDisponiblePrecalificado { get; set; }
        public Nullable<decimal> fnPrima { get; set; }
        public Nullable<decimal> fnValorGarantia { get; set; }
        public Nullable<short> fiEdadCliente { get; set; }
        public Nullable<short> fiClienteArraigoLaboralAños { get; set; }
        public Nullable<short> fiClienteArraigoLaboralMeses { get; set; }
        public Nullable<short> fiClienteArraigoLaboralDias { get; set; }
        public Nullable<decimal> fnSueldoBaseReal { get; set; }
        public Nullable<decimal> fnBonosComisionesReal { get; set; }
        public int fiIDOrigen { get; set; }
    }
}