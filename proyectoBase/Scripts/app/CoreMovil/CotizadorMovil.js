$(function () {
    $(".mascara-cantidad").inputmask("decimal", {
        alias: "numeric",
        groupSeparator: ",",
        digits: 2,
        integerDigits: 11,
        digitsOptional: false,
        placeholder: "0",
        radixPoint: ".",
        autoGroup: true,
        min: 0.0,
    });
    $(".mascara-numerica").inputmask("decimal", {
        alias: "numeric",
        groupSeparator: ",",
        digits: 0,
        integerDigits: 3,
        digitsOptional: false,
        placeholder: "0",
        radixPoint: ".",
        autoGroup: true,
        min: 0.0,
    });
    $(".mascara-identidad").inputmask("9999999999999");
});

$("#ddlProducto").on('change', function () {

    if ($("#ddlProducto :selected").val() == "Finaciamiento") {

        $("#divValorDelVehiculo").removeClass('col-12').addClass('col-6');
        $("#txtMonto").prop('disabled', true);
        $("#txtValorPrima").css('display', '');
        $("#lblPrima").css('display', '');
        $("#lblMonto").text("Valor a financiar del vehiculo");
        $("#lblPorcentajedePrima").css('display', '');
    }
    else {

        $("#divValorDelVehiculo").removeClass('col-6').addClass('col-12');
        $("#txtMonto").prop('disabled', false);
        $("#txtValorPrima").css('display', 'none');
        $("#lblPrima").css('display', 'none');
        $("#lblMonto").text("Valor del empeño");
        $("#lblPorcentajedePrima").css('display', 'none');
    }
});


function CalcularPrima() {

    var lcValorVehiculo = parseFloat($("#txtValorVehiculo").val().replace(/,/g, '') != '' ? $("#txtValorVehiculo").val().replace(/,/g, '') : 0);
    var lcValorPrima = parseFloat($("#txtValorPrima").val().replace(/,/g, '') != '' ? $("#txtValorPrima").val().replace(/,/g, '') : 0);
    var lcMontoaFinaciar = parseFloat($("#txtMonto").val().replace(/,/g, '') != '' ? $("#txtMonto").val().replace(/,/g, '') : 0);

    if ($("#ddlProducto :selected").val() == "Finaciamiento")
    {
        $("#txtMonto").val(lcValorVehiculo - lcValorPrima);
        lcMontoaFinaciar = $("#txtMonto").val().replace(/,/g, '') != '' ? $("#txtMonto").val().replace(/,/g, '') : 0;
    }
    else
    {
        $("#txtValorPrima").val(lcValorVehiculo - lcMontoaFinaciar);
        lcValorPrima = $("#txtValorPrima") ? "0" : txtValorPrima.Text;
    }

    lcMontoaFinaciar = $("#txtMonto").val().replace(/,/g, '') != '' ? $("#txtMonto").val().replace(/,/g, '') : 0;

    var ddlSeguro = $("#ddlSeguro");
    ddlSeguro.empty;
    ddlSeguro.add('<option value="">Seleccionar</option>');

    if (Convert.ToDecimal(lcMontoaFinaciar) > 50000) {

        ddlSeguro.add('<option value="A - Full Cover">A - Full Cover</option>');
        ddlSeguro.add('<option value="B - Basico + Garantía">B - Basico + Garantía</option>');
    }
    else {
        ddlSeguro.add('<option value="A - Full Cover">A - Full Cover</option>');
        ddlSeguro.add('<option value="C - Basico">C - Basico</option>');
    }

    if (lcValorVehiculo > 0) {
        $("#lblPorcentajedePrima").text();
        lblPorcentajedePrima.InnerText = string.Format("{0:N2}", Convert.ToString(Math.Round((Convert.ToDouble(lcValorPrima) * 100.00) / Convert.ToDouble(lcValorVehiculo), 2))) + "%";
        lblPorcentajeMonto.InnerText = string.Format("{0:N2}", Convert.ToString(100 - (Math.Round((Convert.ToDouble(lcValorPrima) * 100.00) / Convert.ToDouble(lcValorVehiculo), 2)))) + "%";
    }
}

function IniciarDatatables() {

    $('#tblDatosParaSAF').DataTable({
        responsive: true,
        dom: ''
    });
}

/* Exportar cotización a PDF */
function ExportToPDF(fileName) {

    const cotizacion = this.document.getElementById("divCotizacionPDF");

    var opt = {
        margin: 0.3,
        filename: fileName + '.pdf',
        image: { type: 'jpeg', quality: 1 },
        html2canvas: {
            dpi: 192,
            scale: 4,
            letterRendering: true,
            useCORS: false
        },
        jsPDF: { unit: 'in', format: 'a4', orientation: 'portrait' },
        pagebreak: { after: '.page-break', always: 'img' }
    };

    $("#divContenedor,#divCotizacionPDF").css('display', '');
    $("body,html").css("overflow", "hidden");

    html2pdf().from(cotizacion).set(opt).save().then(function () {
        $("#divContenedor,#divCotizacionPDF").css('display', 'none');
        $("body,html").css("overflow", "");
    });
}