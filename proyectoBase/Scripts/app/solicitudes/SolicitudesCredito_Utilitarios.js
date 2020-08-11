//CALCULAR CAPACIDAD DE PAGO DEPENDIENDO EL TIPO DE PRESTAMO
function calcularCapacidadPago(tipoPrestamo, ObligacionesPrecalificado, IngresosReales) {
    var capacidadPago = 0;
    if (tipoPrestamo == '101') {
        capacidadPago = ObligacionesPrecalificado == 0 ? IngresosReales * 0.13 : (IngresosReales - ObligacionesPrecalificado) * 0.13;
    }
    else if (tipoPrestamo == '201') {
        capacidadPago = ObligacionesPrecalificado == 0 ? IngresosReales * 0.30 : (IngresosReales - ObligacionesPrecalificado) * 0.30;
    }
    else if (tipoPrestamo == '202') {
        capacidadPago = ObligacionesPrecalificado == 0 ? IngresosReales * 0.40 : (IngresosReales - ObligacionesPrecalificado) * 0.40;
    }
    return capacidadPago.toFixed(2);
}