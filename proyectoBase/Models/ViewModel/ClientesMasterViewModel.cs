using System;

namespace proyectoBase.Models.ViewModel
{
    public class ClientesMasterViewModel
    {
        public int fiIDCliente { get; set; }
        public int IDTipoCliente { get; set; }
        public string fcNoCliente { get; set; }
        public string fcIdentidadCliente { get; set; }
        public string RTNCliente { get; set; }
        public string fcTelefonoCliente { get; set; }
        public int fiNacionalidadCliente { get; set; }
        public System.DateTime fdFechaNacimientoCliente { get; set; }
        public string fcCorreoElectronicoCliente { get; set; }
        public string fcProfesionOficioCliente { get; set; }
        public string fcSexoCliente { get; set; }
        public int fiIDEstadoCivil { get; set; }
        public int fiIDVivienda { get; set; }
        public Nullable<int> fiTiempoResidir { get; set; }
        public bool fbClienteActivo { get; set; }
        public string fcRazonInactivo { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
        public string fcPrimerNombreCliente { get; set; }
        public string fcSegundoNombreCliente { get; set; }
        public string fcPrimerApellidoCliente { get; set; }
        public string fcSegundoApellidoCliente { get; set; }

        // datos
        public string DocumentosFiscal { get; set; }
        public int IdFiscal { get; set; }
        public string DocumentoIdPersonal { get; set; }

        // nacionalidad del cliente
        public string fcDescripcionNacionalidad { get; set; }
        public bool fbNacionalidadActivo { get; set; }

        public string fcDescripcionEstadoCivil { get; set; }
        public bool fbEstadoCivilActivo { get; set; }

        public string fcDescripcionVivienda { get; set; }
        public bool fbViviendaActivo { get; set; }

        // Datos Tabla EquifaxCliente

        public string fcDescripcionDoctosFiscal { get; set; }
        public string fcDescricpcionOrigenEtnicoORacial { get; set; }
        public string fcNombreDoctosIdPersonal { get; set; }
        public string fcNombrePaisNacimiento { get; set; }
        public string fcNoIdFiscal { get; set; }
        public int fiIDProducto { get; set; }
    }
}