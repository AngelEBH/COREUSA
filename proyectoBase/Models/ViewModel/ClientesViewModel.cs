﻿using System.Collections.Generic;

namespace proyectoBase.Models.ViewModel
{
    public class ClientesViewModel
    {
        public ClientesMasterViewModel clientesMaster { get; set; }
        public ClientesInformacionConyugalViewModel ClientesInformacionConyugal { get; set; }
        public ClientesInformacionDomicilioViewModel ClientesInformacionDomiciliar { get; set; }
        public ClientesInformacionLaboralViewModel ClientesInformacionLaboral { get; set; }
        public List<ClientesReferenciasViewModel> ClientesReferenciasPersonales { get; set; }

        //AVALES DEL CLIENTE (SOLO PUEDE TENER 1 ACTIVO)
        public List<ClienteAvalesViewModel> Avales { get; set; }
    }
}