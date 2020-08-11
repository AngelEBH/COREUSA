using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace proyectoBase.Models.ViewModel
{
    public class BuscadorPrecalificadoViewModel
    {
        public string mensaje { get; set; }
        public List<ciudades> ciudadesResidencia { get; set; }
        public List<catalogoProductos> catalogoProductos { get; set; }
    }

    public class ciudades
    {
        public string ciudad { get; set; }
    }
    
    public class catalogoProductos
    {
        public int IDProducto { get; set; }
        public string DescripcionProducto { get; set; }
    }
}