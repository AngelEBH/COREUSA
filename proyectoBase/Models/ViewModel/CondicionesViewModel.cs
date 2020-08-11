using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class CondicionesViewModel
{
    public int IDSolicitudCondicion { get; set; }
    public int IDCondicion { get; set; }
    public int IDSolicitud { get; set; }
    public string TipoCondicion { get; set; }
    public string DescripcionCondicion { get; set; }
    public string ComentarioAdicional { get; set; }
    public bool Estado { get; set; }
}