using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class ClientesPrecalificadosViewModel
    {
        public string identidad { get; set; }
        public string nombres { get; set; }
        public string telefono{ get; set; }
        public decimal ingresos { get; set; }
        public string comentario { get; set; }
        public DateTime primerConsulta { get; set; }
    }
}