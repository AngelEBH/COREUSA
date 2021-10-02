<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Garantia_Actualizar.aspx.cs" Inherits="Garantia_Actualizar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <meta name="description" content="Actualizar garantía" />
    <title>Actualizar información de la garantía</title>
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/CSS/Content/css/icons.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/CSS/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <style>
        .card {
            border: none;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }
    </style>
</head>
<body>
    <form runat="server" id="frmGarantia" class="" action="#" data-parsley-excluded="[disabled]">
        <div class="card mb-0">
            <div class="card-header pb-1 pt-1">
                <div class="row justify-content-between">
                    <div class="col-auto">
                        <h6>Actualizar garantía: <span runat="server" id="lblNoSolicitud"></span><small><span runat="server" id="lblMensaje" class="text-danger" visible="false"></span></small></h6>
                    </div>
                    <div class="col-1 align-self-center">
                        <!-- loader -->
                        <div id="Loader" class="float-right" runat="server" style="display: none;">
                            <div class="spinner-border" role="status">
                                <span class="sr-only"></span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <div id="smartwizard" class="h-100">
                    <ul>
                        <li><a href="#step-1" class="pt-3 pb-2 font-12">Características</a></li>
                        <li><a href="#step-2" class="pt-3 pb-2 font-12">Documentación</a></li>
                    </ul>
                    <div>
                        <!-- Información principal -->
                        <div id="step-1" class="form-section">
                            <div class="row m-0 border-bottom border-gray pb-1">
                                <div class="col-sm-auto p-0">
                                    <label class="col-form-label">VIN</label>
                                </div>
                                <div class="col-sm-4 pr-0">
                                    <asp:TextBox ID="txtBuscarVIN" CssClass="form-control form-control-sm mascara-vin" placeholder="EJ. JH4TB2H26CC000000" type="text" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-auto pl-1">
                                    <button type="button" id="btnBuscarVIN" class="btn btn-secondary text-center pt-1">Buscar</button>
                                </div>
                                <div class="m-0 p-0">
                                    <label class="col-form-label">Digitar manualmente&nbsp;</label>
                                    <input type="checkbox" id="cbDigitarManualmente" switch="info" class="align-bottom mb-1" />
                                    <label for="cbDigitarManualmente" data-on-label="ON" data-off-label="OFF" class="align-bottom mb-1"></label>
                                </div>
                            </div>
                            <div class="row justify-content-between mb-0">
                                <!-- Información del cliente -->
                                <div class="col-lg-6">
                                    <h6 class="mb-1">Características físicas</h6>

                                    <div class="form-group row">
                                        <div class="col-12">
                                            <label class="col-form-label">VIN</label>
                                            <asp:TextBox ID="txtVIN" placeholder="EJ. JH4TB2H26CC000000" CssClass="form-control form-control-sm mascara-vin" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="col-form-label">Tipo de garantía</label>
                                            <asp:DropDownList ID="ddlTipoDeGarantia" runat="server" Enabled="false" CssClass="form-control form-control-sm col-form-label" required="required"></asp:DropDownList>
                                        </div>
                                        <div class="col-6">
                                            <label class="col-form-label">Tipo de vehículo</label>
                                            <asp:TextBox ID="txtTipoDeVehiculo" placeholder="EJ. Turismo" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Marca</label>
                                            <asp:TextBox ID="txtMarca" placeholder="EJ. Honda" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Modelo</label>
                                            <asp:TextBox ID="txtModelo" placeholder="EJ. Civic" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Año</label>
                                            <asp:TextBox ID="txtAnio" CssClass="form-control form-control-sm mascara-enteros" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Color</label>
                                            <asp:TextBox ID="txtColor" CssClass="form-control form-control-sm" type="text" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-12">
                                            <label class="col-form-label">Matrícula</label>
                                            <asp:TextBox ID="txtMatricula" placeholder="EJ. AAA 9999" CssClass="form-control form-control-sm mascara-matricula" type="text" required="required" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                     <h6 class="mb-1 border-top border-gray pt-2">Características mecánicas</h6>
                                   
                                    <div class="form-group row mb-4">
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Cilindraje</label>
                                            <asp:TextBox ID="txtCilindraje" placeholder="EJ. 1.8" CssClass="form-control form-control-sm mascara-cilindraje" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Recorrido</label>
                                            <asp:TextBox ID="txtRecorrido" CssClass="form-control form-control-sm mascara-cantidad" type="text" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Unidad de medida</label>
                                            <asp:DropDownList ID="ddlUnidadDeMedida" runat="server" CssClass="form-control form-control-sm col-form-label" required="required" data-parsley-errors-container="#error-ddlUnidadDeMedida"></asp:DropDownList>
                                            <div id="error-ddlUnidadDeMedida"></div>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Transmisión</label>
                                            <asp:TextBox ID="txtTransmision" placeholder="EJ. Automático" CssClass="form-control form-control-sm" type="text" required="required" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <label class="col-form-label">Tipo de combustible</label>
                                            <asp:TextBox ID="txtTipoDeCombustible" placeholder="EJ. Gasolina" CssClass="form-control form-control-sm" type="text" ReadOnly="true" required="required" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                             <div class="row">
                                <div class="col-12">
                                    <h6 class="mb-0 border-top border-gray pt-2"></h6>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-4">
                                    <label class="col-form-label">Serie 1</label>
                                    <asp:TextBox ID="txtSerieUno" CssClass="form-control form-control-sm" ReadOnly="true" type="text" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-4">
                                    <label class="col-form-label">Serie Motor</label>
                                    <asp:TextBox ID="txtSerieMotor" placeholder="EJ. 0XX-0000000" CssClass="form-control form-control-sm" required="required" type="text" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-4">
                                    <label class="col-form-label">Serie Chasis</label>
                                    <asp:TextBox ID="txtSerieChasis" placeholder="EJ. 0XXXX00X0XX000000" CssClass="form-control form-control-sm" required="required" type="text" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-4">
                                    <label class="col-form-label">Otra serie (opcional)</label>
                                    <asp:TextBox ID="txtSerieDos" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                </div>
                               <div class="col-sm-6 col-md-6 col-lg-4">
                                    <label class="col-form-label">GPS</label>
                                    <asp:TextBox ID="txtGPS" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-6 col-md-6 col-lg-4">
                                    <label class="col-form-label">No. Préstamo (opcional)</label>
                                    <asp:TextBox ID="txtNumeroPrestamo" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                </div>
                                <!-- Préstamos disponibles -->
                                <div class="col-12">
                                    <label class="col-form-label">Comentario</label>
                                    <textarea id="txtComentario" runat="server" class="form-control form-control-sm" data-parsley-maxlength="300" rows="2"></textarea>
                                </div>
                            </div>

                                  
                                </div>
                                <!-- Información del préstamo máximo -->
                               <%-- <div class="col-lg-6 border-left border-gray">
                                   
                                </div>--%>

                                <div class="col-lg-6">

                                     <h6 class="mb-1">Valores de la garantía</h6>
                                    <div class="form-group row mb-0">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Precio mercado</label>
                                            <asp:TextBox ID="txtPrecioMercado" CssClass="form-control form-control-sm mascara-cantidad" type="text" Text="0" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Prima</label>
                                            <asp:TextBox ID="txtValorPrima" CssClass="form-control form-control-sm mascara-cantidad" type="text" Text="0" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Financiado</label>
                                            <asp:TextBox ID="txtValorFinanciado" CssClass="form-control form-control-sm mascara-cantidad" type="text" Text="0" required="required" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Gastos de cierre</label>
                                            <asp:TextBox ID="txtGastosDeCierre" CssClass="form-control form-control-sm mascara-cantidad" type="text" Text="0" required="required" runat="server"></asp:TextBox>
                                        </div>
                                    </div>


                                    <h6 class="mb-1 border-top border-gray pt-2">Propietario de la garantía</h6>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Identidad</label>
                                            <asp:TextBox ID="txtIdentidadPropietario" CssClass="form-control form-control-sm " type="text" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nombre completo</label>
                                            <asp:TextBox ID="txtNombrePropietario" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Estado Civil</label>
                                            <asp:DropDownList ID="ddlEstadoCivilPropietario" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" data-parsley-errors-container="#error-ddlEstadoCivilPropietario"></asp:DropDownList>
                                            <div id="error-ddlEstadoCivilPropietario"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nacionalidad</label>
                                            <asp:DropDownList ID="ddlNacionalidadPropietario" runat="server" CssClass="form-control form-control-sm col-form-label" data-parsley-errors-container="#error-ddlNacionalidadPropietario"></asp:DropDownList>
                                            <div id="error-ddlNacionalidadPropietario"></div>
                                        </div>
                                    </div>


                                     <h6 class="mb-1 border-top border-gray pt-2">Vendedor de la garantía</h6>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Identidad</label>
                                            <asp:TextBox ID="txtIdentidadVendedor" CssClass="form-control form-control-sm " type="text" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nombre completo</label>
                                            <asp:TextBox ID="txtNombreVendedor" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Estado Civil</label>
                                            <asp:DropDownList ID="ddlEstadoCivilVendedor" runat="server" CssClass="form-control form-control-sm col-form-label buscadorddl" data-parsley-errors-container="#error-ddlEstadoCivilVendedor"></asp:DropDownList>
                                            <div id="error-ddlEstadoCivilVendedor"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nacionalidad</label>
                                            <asp:DropDownList ID="ddlNacionalidadVendedor" runat="server" CssClass="form-control form-control-sm col-form-label" data-parsley-errors-container="#error-ddlNacionalidadVendedor"></asp:DropDownList>
                                            <div id="error-ddlNacionalidadVendedor"></div>
                                        </div>
                                    </div>

                                    <div style="display:none;">
                                          <h6 class="mb-1 border-top border-gray pt-2">Datos Para Desembolso</h6>
                                        <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label class="col-form-label">RTN</label>
                                            <asp:TextBox ID="txtRTNVendedor" CssClass="form-control form-control-sm mascara-rtn" type="text" runat="server"></asp:TextBox>
                                        </div>
                                      
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Forma de Pago</label>
                                            <asp:DropDownList ID="ddlFormaDePagoDesembolso" runat="server" CssClass="form-control form-control-sm col-form-label" data-parsley-errors-container="#error-ddlFormaDePagoDesembolso"></asp:DropDownList>
                                            <div id="error-ddlFormaDePagoDesembolso"></div>
                                        </div>
                                    <%--     <div class="col-sm-6">
                                            <label class="col-form-label">Banco a Depositar</label>
                                            <asp:TextBox ID="txtBancoDeposito" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                        </div>--%>
                                         <div class="col-sm-6">
                                            <label class="col-form-label">Banco a Depositar</label>
                                            <asp:DropDownList ID="ddlBancoDesembolso" runat="server" CssClass="form-control form-control-sm col-form-label" data-parsley-errors-container="#error-ddlBancoDesembolso"></asp:DropDownList>
                                            <div id="error-ddlBancoDesembolso"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Tipo Cuenta Bancaria</label>
                                            <asp:DropDownList ID="ddlTipoCuentaBancaria" runat="server" CssClass="form-control form-control-sm col-form-label" data-parsley-errors-container="#error-ddlTipoCuentaBancaria"></asp:DropDownList>
                                            <div id="error-ddlTipoCuentaBancaria"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">No. Cuenta Bancaria</label>
                                            <asp:TextBox ID="txtCuentaBancariaDeposito" CssClass="form-control form-control-sm" type="text" runat="server"></asp:TextBox>
                                        </div>
                                    </div>

                                    </div>
                                  

                                </div>
                            </div>
                         
                        </div>

                        <!-- Documentación de la garantía -->
                        <div id="step-2" class="form-section">
                            <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-12 p-0">
                                    <h6 class="mt-1">Documentación de la garantía <small class="text-info">(Estimado usuario, recuerda subir toda la documentación hasta que ya vayas a guardar la garantía)</small></h6>
                                </div>
                            </div>
                            <!-- Div donde se generan dinamicamente los inputs para la documentación -->
                            <div class="row pr-1 pl-1 text-center" id="DivDocumentacion"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        const user = '<%= pcIDUsuario.ToString() %>';
        esDigitadoManualmente = <%= EsDigitadoManualmente.ToString().ToLower() %>;

        $("#cbDigitarManualmente").prop("checked", esDigitadoManualmente);

        let digitarManualmente = esDigitadoManualmente;

        $("#btnBuscarVIN,#txtBuscarVIN").prop("disabled", digitarManualmente).prop("title", digitarManualmente == true ? 'La búsqueda está desactivada' : '');

        $("#txtVIN,#txtTipoDeVehiculo,#txtMarca,#txtModelo,#txtAnio,#txtCilindraje,#txtTransmision,#txtTipoDeCombustible,#txtSerieUno").prop("readonly", !digitarManualmente);

        $(document).ready(function () {
            $(".mascara-cantidad").inputmask("decimal", {
                alias: 'numeric',
                groupSeparator: ',',
                digits: 2,
                integerDigits: 11,
                digitsOptional: false,
                placeholder: '0',
                radixPoint: ".",
                autoGroup: true,
                min: 0.00
            });
            $(".mascara-enteros").inputmask("decimal", {
                alias: "numeric",
                groupSeparator: ",",
                digits: 0,
                integerDigits: 4,
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
            $(".mascara-cilindraje").inputmask("decimal", {
                alias: "numeric",
                groupSeparator: ",",
                digits: 1,
                integerDigits: 4,
                digitsOptional: false,
                placeholder: "0",
                radixPoint: ".",
                autoGroup: true,
                min: 0.0,
            });
            $(".mascara-identidad").inputmask("9999999999999");
            $(".mascara-rtn").inputmask("99999999999999");
        });
    </script>
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/app/solicitudes/Garantia_Actualizar.js?v=202103090943"></script>
</body>
</html>

