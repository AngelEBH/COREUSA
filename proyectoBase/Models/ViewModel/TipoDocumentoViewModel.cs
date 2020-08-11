using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class TipoDocumentoViewModel
{
    public int IDTipoDocumento { get; set; }
    public string DescripcionTipoDocumento { get; set; }
    public int CantidadMaximaDoucmentos { get; set; }
    public int TipoVisibilidad { get; set; }
}