using System;
using System.Collections.Generic;

public class PrecalificadoViewModel
{
    public string identidad { get; set; }
    public string primerNombre { get; set; }
    public string segundoNombre { get; set; }
    public string primerApellido { get; set; }
    public string segundoApellido { get; set; }
    public string telefono { get; set; }
    public decimal obligaciones { get; set; }
    public decimal ingresos { get; set; }
    public decimal disponible { get; set; }
    public DateTime fechaNacimiento { get; set; }
    public int tipoSolicitud { get; set; }
    public int tipoProducto { get; set; }
    public string Producto { get; set; }
    public List<cotizadorProductosViewModel> cotizadorProductos { get; set; }

}
public class cotizadorProductosViewModel
{
    public int IDCotizacion { get; set; }
    public int IDProducto { get; set; }
    public string ProductoDescripcion { get; set; }
    public decimal fnMontoOfertado { get; set; }
    public int fiPlazo { get; set; }
    public string TipoCuota { get; set; }
    public decimal fnCuotaQuincenal { get; set; }
}