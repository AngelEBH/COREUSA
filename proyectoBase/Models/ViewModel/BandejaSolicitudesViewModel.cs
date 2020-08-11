using System;

namespace proyectoBase.Models.ViewModel
{
    public class BandejaSolicitudesViewModel
    {
        // informacion principal de la solicitud
        public int fiIDSolicitud { get; set; }
        public int fiIDTipoPrestamo { get; set; }
        public string fcDescripcion { get; set; }
        public int fiTipoSolicitud { get; set; }
        public int TipoNegociacion { get; set; }
        public decimal fdValorPmoSugeridoSeleccionado { get; set; }
        public int fiPlazoPmoSeleccionado { get; set; }
        public System.DateTime fdFechaCreacionSolicitud { get; set; }
        public string fcTiempoTotalTranscurrido { get; set; }
        public int fiSolicitudActiva { get; set; }

        // informacion de precalificado
        public decimal fdIngresoPrecalificado { get; set; }
        public decimal fdObligacionesPrecalificado { get; set; }
        public decimal fdDisponiblePrecalificado { get; set; }
        public Nullable<decimal> fnPrima { get; set; }
        //public Nullable<decimal> fnValorVehiculo { get; set; }
        public Nullable<decimal> fnValorGarantia { get; set; }
        public Nullable<short> fiEdadCliente { get; set; }
        public System.DateTime fdFechaIngresoArraigoLaboral { get; set; }

        // informacion del vendedor
        public int fiIDUsuarioCrea { get; set; }
        public int? fiIDUsuarioVendedor { get; set; }
        public string fcNombreCortoVendedor { get; set; }

        // informacion de la agencia donde fue registrado
        public string fcNoAgencia { get; set; }
        public string fcAgencia { get; set; }
        public string fcUbicacionAgencia { get; set; }

        // informacion del analista
        public int? fiIDAnalista { get; set; }
        public string fcNombreCortoAnalista { get; set; }

        // informacion de analisis de la solicitud
        public string fcTipoEmpresa { get; set; }
        public string fcTipoPerfil { get; set; }
        public string fcTipoEmpleado { get; set; }
        public string fcBuroActual { get; set; }
        public Nullable<decimal> fnSueldoBaseReal { get; set; }
        public Nullable<decimal> fnBonosComisionesReal { get; set; }
        public Nullable<decimal> fiMontoFinalSugerido { get; set; }
        public Nullable<decimal> fiMontoFinalFinanciar { get; set; }
        public Nullable<int> fiPlazoFinalAprobado { get; set; }
        public Nullable<int> fiEstadoSolicitud { get; set; }
        public int fiIDOrigen { get; set; }

        // informacion del cliente
        public int fiIDCliente { get; set; }
        public string fcIdentidadCliente { get; set; }
        public string fcTelefonoCliente { get; set; }
        public int fiNacionalidadCliente { get; set; }
        public System.DateTime fdFechaNacimientoCliente { get; set; }
        public string fcCorreoElectronicoCliente { get; set; }
        public string fcProfesionOficioCliente { get; set; }
        public string fcSexoCliente { get; set; }
        public int fiIDEstadoCivil { get; set; }
        public int fiIDVivienda { get; set; }
        public Nullable<short> fiTiempoResidir { get; set; }
        public bool fbClienteActivo { get; set; }
        public string fcRazonInactivo { get; set; }
        public string fcPrimerNombreCliente { get; set; }
        public string fcSegundoNombreCliente { get; set; }
        public string fcPrimerApellidoCliente { get; set; }
        public string fcSegundoApellidoCliente { get; set; }

        // auditoria
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }

        // informacion del procesamiento de la solicitud
        public int fiIDBitacora { get; set; }
        public Nullable<System.DateTime> fdEnIngresoInicio { get; set; }
        public Nullable<System.DateTime> fdEnIngresoFin { get; set; }
        public Nullable<System.DateTime> fdEnTramiteInicio { get; set; }
        public Nullable<System.DateTime> fdEnTramiteFin { get; set; }
        public Nullable<System.DateTime> fdEnAnalisisInicio { get; set; }
        public Nullable<System.DateTime> ftAnalisisTiempoValidarInformacionPersonal { get; set; }
        public string fcComentarioValidacionInfoPersonal { get; set; }
        public Nullable<System.DateTime> ftAnalisisTiempoValidarDocumentos { get; set; }
        public string fcComentarioValidacionDocumentacion { get; set; }
        public Nullable<int> fbValidacionDocumentcionIdentidades { get; set; }
        public Nullable<int> fbValidacionDocumentacionDomiciliar { get; set; }
        public Nullable<int> fbValidacionDocumentacionLaboral { get; set; }
        public Nullable<int> fbValidacionDocumentacionSolicitudFisica { get; set; }
        public Nullable<System.DateTime> ftAnalisisTiempoValidacionReferenciasPersonales { get; set; }
        public string fcComentarioValidacionReferenciasPersonales { get; set; }
        public Nullable<System.DateTime> ftAnalisisTiempoValidarInformacionLaboral { get; set; }
        public string fcComentarioValidacionInfoLaboral { get; set; }
        public Nullable<System.DateTime> ftTiempoTomaDecisionFinal { get; set; }
        public string fcObservacionesDeCredito { get; set; }
        public string fcComentarioResolucion { get; set; }
        public Nullable<System.DateTime> fdEnAnalisisFin { get; set; }
        public Nullable<System.DateTime> fdCondicionadoInicio { get; set; }
        public string fcCondicionadoComentario { get; set; }
        public Nullable<System.DateTime> fdCondificionadoFin { get; set; }

        public int fiEstadoDeCampo { get; set; }
        public Nullable<System.DateTime> fdEnvioARutaAnalista { get; set; }
        public Nullable<System.DateTime> fdEnCampoInicio { get; set; }
        public string fcObservacionesDeGestoria { get; set; }
        public Nullable<System.DateTime> fdEnCampoFin { get; set; }
        public Nullable<System.DateTime> fdReprogramadoInicio { get; set; }
        public string fcReprogramadoComentario { get; set; }
        public Nullable<System.DateTime> fdReprogramadoFin { get; set; }

        public Nullable<System.DateTime> PasoFinalInicio { get; set; }
        public int IDUsuarioPasoFinal { get; set; }
        public string ComentarioPasoFinal { get; set; }
        public Nullable<System.DateTime> PasoFinalFin { get; set; }


        // propiedades que deben ser eliminadas
        public string fcNoSolicitud { get; set; }
        public string fcTipoSolicitud { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public string fcNombreAgencia { get; set; }
        public Nullable<short> fiClienteArraigoLaboralAños { get; set; }
        public Nullable<short> fiClienteArraigoLaboralMeses { get; set; }
        public Nullable<short> fiClienteArraigoLaboralDias { get; set; }
        public string fcNoCliente { get; set; }
        //informacion vendedor que ingreso la solicitud
        public int? fiIDVendedor { get; set; }
        public int? fiIDAgencia { get; set; }
        public string usu_NombreUsuario { get; set; }
        public string usu_Nombres { get; set; }
        public string usu_Apellidos { get; set; }
        public string usu_Correos { get; set; }
        public bool? usu_EsActivo { get; set; }
        public string usu_RazonInactivo { get; set; }
        public bool? usu_EsAdministrador { get; set; }
    }
}