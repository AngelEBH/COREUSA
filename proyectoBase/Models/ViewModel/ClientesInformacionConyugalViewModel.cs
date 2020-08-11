using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class ClientesInformacionConyugalViewModel
    {
        public int fiIDInformacionConyugal { get; set; }
        public int fiIDCliente { get; set; }
        public string fcNombreCompletoConyugue { get; set; }
        public string fcIndentidadConyugue { get; set; }
        public Nullable<System.DateTime> fdFechaNacimientoConyugue { get; set; }
        public string fcTelefonoConyugue { get; set; }
        public string fcLugarTrabajoConyugue { get; set; }
        public Nullable<decimal> fcIngresosMensualesConyugue { get; set; }
        public string fcTelefonoTrabajoConyugue { get; set; }
        public int fiIDUsuarioCrea { get; set; }
        public string fcNombreUsuarioCrea { get; set; }
        public System.DateTime fdFechaCrea { get; set; }
        public Nullable<int> fiIDUsuarioModifica { get; set; }
        public string fcNombreUsuarioModifica { get; set; }
        public Nullable<System.DateTime> fdFechaUltimaModifica { get; set; }
    }
}