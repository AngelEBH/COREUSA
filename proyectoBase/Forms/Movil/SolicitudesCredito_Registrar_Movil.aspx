<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Registrar_Movil.aspx.cs" Inherits="SolicitudesCredito_Registrar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Ingresar solicitud de crédito</title>
    <!-- BOOTSTRAP -->
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard_theme_dots.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard_theme_circles.min.css" rel="stylesheet" />
    <link href="/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <style>
        .sw-theme-circles > ul.step-anchor > li {
            margin-left: 10px;
        }

        .sw-theme-default {
            box-shadow: none;
        }

        .sw-theme-circles > ul.step-anchor {
            margin-bottom: 0px;
        }

            .sw-theme-circles > ul.step-anchor > li > a {
                border: 2px solid #f5f5f5;
                width: 30px;
                height: 30px;
                padding: 6px 0;
                border-radius: 50%;
                box-shadow: inset 0 0 0 3px #fff !important;
                text-decoration: none;
                outline-style: none;
                z-index: 99;
                margin: 0;
            }

        html {
            background-color: white !important;
        }

        .h-100vh {
            height: 100vh !important;
        }

        .container-parent {
            position: relative;
            margin-bottom: 5px;
        }

        .buttons-in-bottom {
            position: absolute;
            bottom: 0;
            width: 100%;
        }
    </style>
</head>
<body class="border-0 h-100vh">
    <!-- FORMULARIO INGRESO DE SOLICITUD DE CREDITO -->
    <asp:Label runat="server" ID="lblAlerta" CssClass="h5 text-danger"></asp:Label>
    <form runat="server" id="frmSolicitud" class="form-demo h-100" action="#" data-parsley-excluded="[disabled]">
        <div id="smartwizard" class="h-100 pb-2 bg-white">
            <ul class="pt-2 justify-content-center">
                <li><a href="#step-1">1</a></li>
                <li><a href="#step-2">2</a></li>
                <li><a href="#step-3">3</a></li>
                <li><a href="#step-4">4</a></li>
                <li><a href="#step-5">5</a></li>
                <li><a href="#step-6">6</a></li>
                <li><a href="#step-7">7</a></li>
            </ul>
            <div>
                <!-- INFORMACION DEL PRESTAMO Y LA SOLICITUD -->
                <div id="step-1" class="form-section p-3">
                    <div class="float-right" id="spinnerCargando">
                        <div class="spinner-border" role="status">
                            <span class="sr-only"></span>
                        </div>
                    </div>
                    <h6 class="border-bottom border-gray pb-2">Información del préstamo </h6>

                    <!-- TIPO DE SOLICITUD, TIPO DE PRODUCTO, ORIGEN -->
                    <div class="form-group form-row mb-0">
                        <div class="col">
                            <label class="col-form-label">Tipo de solicitud</label>
                            <asp:TextBox ID="tipoSolicitud" Enabled="false" CssClass="form-control form-control-sm col-form-label" type="text" Text="NUEVO" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label class="col-form-label">Producto</label>
                            <asp:TextBox ID="tipoPrestamo" ReadOnly="true" Enabled="false" CssClass="form-control form-control-sm col-form-label" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                    </div>


                    <div class="form-group form-row mb-0" id="titleOrigen" style="display: none;">
                        <div class="col">
                            <label class="col-form-label">Origen</label>
                            <select name="ddlOrigen" id="ddlOrigen" class="form-control form-control-sm col-form-label" disabled="disabled" style="display: none;" data-parsley-group="informacionPrestamo" required="required">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                    </div>
                    <!-- NOMBRE COMPLETO DEL CLIENTE -->

                    <div class="form-group form-row mb-0">
                        <div class="col">
                            <label class="col-form-label">Primer nombre</label>
                            <asp:TextBox ID="primerNombreCliente" CssClass="form-control form-control-sm col-form-label" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label class="col-form-label">Segundo nombre</label>
                            <asp:TextBox ID="SegundoNombreCliente" CssClass="form-control form-control-sm col-form-label" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group form-row mb-0">
                        <div class="col">
                            <label class="col-form-label">Primer apellido</label>
                            <asp:TextBox ID="primerApellidoCliente" CssClass="form-control form-control-sm col-form-label" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label class="col-form-label">Segundo apellido</label>
                            <asp:TextBox ID="segundoApellidoCliente" CssClass="form-control form-control-sm col-form-label" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <!-- IDENTIDAD, RTN, INGRESOS -->

                    <div class="form-group form-row mb-0">
                        <div class="col">
                            <label class="col-form-label">No. Identidad</label>
                            <asp:TextBox ID="identidadCliente" CssClass="form-control form-control-sm col-form-label identidad" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label class="col-form-label">RTN numérico</label>
                            <asp:TextBox ID="rtnCliente" CssClass="form-control form-control-sm col-form-label formatoRTN" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="form-group form-row mb-0">
                        <div class="col">
                            <label class="col-form-label">Ingresos precalificado</label>
                            <asp:TextBox ID="ingresosPrecalificado" CssClass="form-control form-control-sm col-form-label MascaraCantidad" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <label class="col-form-label">PMO sugerido</label>
                            <select name="pmoSugeridoSeleccionado" id="pmoSugeridoSeleccionado" disabled="disabled" class="form-control form-control-sm col-form-label" data-parsley-group="informacionPrestamo" required="required">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                    </div>

                    <!-- PMO SUGERIDO, PLAZO, CUOTA -->
                    <div class="form-group form-row mb-0">
                        <div class="col">
                            <label class="col-form-label" id="lblPlazoPMO">Plazo</label>
                            <input id="plazoPmoSeleccionado" readonly="readonly" class="form-control form-control-sm col-form-label MascaraEnteros" type="text" required="required" data-parsley-group="informacionPrestamo" />
                        </div>
                        <div class="col">
                            <label class="col-form-label">Cuota</label>
                            <input id="cutoaQuinceal" class="form-control form-control-sm col-form-label MascaraCantidad" readonly="readonly" type="text" required="required" data-parsley-group="informacionPrestamo" />
                        </div>
                    </div>

                    <!-- PRESTAMO EFECTIVO -->
                    <div class="form-group form-row mb-0 divPrestamoEfectivo" style="display: none;">
                        <div class="col">
                            <label class="col-form-label" id="lblMontoPmo">Monto</label>
                            <input id="txtMontoPmoEfectivo" class="form-control form-control-sm col-form-label MascaraCantidad" disabled="disabled" type="text" required="required" data-parsley-group="informacionPrestamo" />
                        </div>
                    </div>

                    <!-- PRESTAMO CON PRIMA -->
                    <div class="form-group form-row mb-0 divPrestamoVehiculo" style="display: none;">
                        <div class="col">
                            <label id="lblValorVehiculo" class="col-form-label">Valor Vehículo</label>
                            <input id="txtValorVehiculo" class="form-control form-control-sm col-form-label MascaraCantidad" disabled="disabled" type="text" required="required" data-parsley-group="informacionPrestamo" />
                        </div>
                        <div class="col">
                            <label class="col-form-label">Valor Prima</label>
                            <input id="txtPrima" class="form-control form-control-sm col-form-label MascaraCantidad" disabled="disabled" type="text" required="required" data-parsley-group="informacionPrestamo" />
                            <span id="error-prima" style="display: none;"></span>
                        </div>
                        <div class="col-12">
                            <label id="lblValorFinanciar" class="col-form-label">Valor financiar</label>
                            <input id="txtValorFinanciar" disabled="disabled" required="required" readonly="readonly" class="form-control form-control-sm col-form-label MascaraCantidad" type="text" data-parsley-group="informacionPrestamo" />
                            <span id="error-valorFinanciar"></span>
                        </div>
                    </div>

                    <!-- PRESTAMOS SUGERIDOS -->
                    <div class="form-group form-row mb-0" id="divPmosSugeridos">
                        <div class="col-12">
                            <label class="col-form-label" id="titlePrestamosSugeridos" style="display: none;">Prestamos disponibles</label>
                            <select name="pmosSugeridos" id="pmosSugeridos" class="form-control form-control-sm col-form-label" data-parsley-group="informacionPrestamo" required="required">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                    </div>
                </div>
                <!-- INFORMACION PERSONAL DEL CLIENTE -->
                <div id="step-2" class="form-section p-3">
                    <h6 class="border-bottom border-gray pb-2">Información personal</h6>

                    <!-- PROFESION, NACIONALIDAD, FECHA DE NACIMIENTO -->
                    <div class="form-group form-row mb-0">
                        <div class="col-6">
                            <label class="col-form-label">Profesión</label>
                            <input id="profesion" class="form-control form-control-sm col-form-label" readonly="readonly" type="text" required="required" data-parsley-group="informacionPersonal" />
                        </div>
                        <div class="col-6">
                            <label class="col-form-label">Nacionalidad</label>
                            <select name="nacionalidad" id="nacionalidad" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-group="informacionPersonal">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                    </div>
                    <div class="form-group form-row mb-0">
                        <div class="col">
                            <label class="col-form-label">Fecha de Nac.</label>
                            <input id="fechaNacimiento" class="form-control form-control-sm col-form-label datepicker" type="date" required="required" data-parsley-group="informacionPersonal" />
                        </div>
                        <div class="col">
                            <label class="col-form-label">Edad cliente</label>
                            <input id="edadCliente" class="form-control form-control-sm col-form-label" readonly="readonly" type="text" data-parsley-group="informacionPersonal" />
                        </div>
                    </div>
                    <!-- EMAIL, TELEFONO,TIPO DE VIVIENDA -->
                    <div class="form-group form-row mb-0">
                        <div class="col">
                            <label class="col-form-label">Email</label>
                            <input id="correoElectronico" class="form-control form-control-sm col-form-label" type="email" required="required" data-parsley-group="informacionPersonal" />
                        </div>
                        <div class="col">
                            <label class="col-form-label">Télefono</label>
                            <asp:TextBox ID="numeroTelefono" CssClass="form-control form-control-sm col-form-label Telefono" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <!-- TIPO DE VIVIENDA -->
                    <div class="form-group form-row mb-0">
                        <div class="col">
                            <label class="col-form-label">Tipo de vivienda</label>
                            <select name="vivivenda" id="vivivenda" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-group="informacionPersonal">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                    </div>

                    <!-- SEXO -->
                    <div class="form-group form-row">
                        <div class="col-12 text-center">
                            <label class="col-form-label text-center">Sexo</label>
                        </div>
                        <div class="col-12 text-center">
                            <div class="form-check form-check-inline justify-content-center">
                                <input class="form-check-input" type="radio" name="sexo" value="M" />
                                <label class="form-check-label" for="inlineRadio1">Masculino</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="sexo" value="F" />
                                <label class="form-check-label" for="inlineRadio2">Femenino</label>
                            </div>
                        </div>
                    </div>
                    <!-- INPUTS RADIO DE ESTADO CIVIL -->
                    <div class="form-group form-row">
                        <div class="col-12 text-center">
                            <label class="col-form-label text-center">Estado civil</label>
                        </div>
                        <div class="col-12 text-center" id="divEstadoCivil">
                        </div>
                    </div>

                    <div class="form-group row mb-0">
                        <!--TIEMPO DE RESIDIR-->
                        <div class="col-12 text-center">
                            <label class="col-form-label">Tiempo residir</label>
                        </div>
                        <div class="col-12 text-center">
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="tiempoResidir" value="0" />
                                <label class="form-check-label" for="inlineRadio1">-1 año</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="tiempoResidir" value="1" />
                                <label class="form-check-label" for="inlineRadio2">1 año</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="tiempoResidir" value="2" />
                                <label class="form-check-label" for="inlineRadio2">2 años</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="tiempoResidir" value="3" />
                                <label class="form-check-label" for="inlineRadio2">+2 años</label>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- INFORMACION DEL DOMICILIO DEL CLIENTE -->
                <div id="step-3" class="form-section p-3">
                    <h6 class="border-bottom border-gray pb-2">Información domicilio</h6>

                    <div class="form-group form-row mb-0">
                        <div class="col-12">
                            <label class="col-form-label">Departamento</label>
                            <select name="departamento" id="departamento" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-depto" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-depto"></span>
                        </div>
                        <div class="col-12">
                            <label class="col-form-label">Municipio</label>
                            <select disabled="disabled" name="municipio" id="municipio" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-municipio" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-municipio"></span>
                        </div>
                        <div class="col-12">
                            <label class="col-form-label">Ciudad/Poblado</label>
                            <select disabled="disabled" name="ciudad" id="ciudad" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-ciudad" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-ciudad"></span>
                        </div>

                        <div class="col-12">
                            <label class="col-form-label">Barrio/Colonia</label>
                            <select disabled="disabled" name="barrioColonia" id="barrioColonia" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-colonia" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-colonia"></span>
                        </div>

                        <div class="col-6">
                            <label class="col-form-label">Teléfono casa</label>
                            <input id="telefonoCasa" class="form-control form-control-sm col-form-label Telefono" type="text" data-parsley-group="informacionDomiciliar" />
                        </div>
                        <div class="col-6">
                            <label class="col-form-label">Teléfono móvil</label>
                            <asp:TextBox ID="telefonoMovil" CssClass="form-control form-control-sm col-form-label Telefono" type="text" required="required" data-parsley-group="informacionDomiciliar" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group form-row">
                        <div class="col-12">
                            <label class="col-form-label">Detalle dirección</label>
                            <input placeholder="Calle, avenida, bloque, etc" id="direccionDetallada" class="form-control form-control-sm col-form-label" type="text" required="required" data-parsley-group="informacionDomiciliar" />
                        </div>
                        <div class="col">
                            <label class="col-form-label">Referencias del domicilio</label>
                            <textarea id="referenciaDireccionDetallada" required="required" class="form-control form-control-sm col-form-label" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2" data-parsley-group="informacionDomiciliar"></textarea>
                        </div>
                    </div>
                </div>
                <!-- INFORMACION LABORAL DEL CLIENTE -->
                <div id="step-4" class="form-section p-3">
                    <h6 class="border-bottom border-gray pb-2">Información laboral</h6>

                    <div class="form-group form-row">
                        <div class="col-6">
                            <label class="col-form-label">Nombre del trabajo</label>
                            <input id="nombreDelTrabajo" name="nombreDelTrabajo" class="form-control form-control-sm col-form-label" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <div class="col-6">
                            <label class="col-form-label">Ingresos mensuales</label>
                            <input id="ingresosMensuales" class="form-control form-control-sm col-form-label MascaraCantidad" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>

                        <div class="col-6">
                            <label class="col-form-label">Puesto asignado</label>
                            <input id="puestoAsignado" name="puestoAsignado" class="form-control form-control-sm col-form-label" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <div class="col-6">
                            <label class="col-form-label">Fecha de ingreso</label>
                            <input id="fechaIngreso" name="fechaIngreso" class="form-control form-control-sm col-form-label datepicker" type="date" required="required" data-parsley-group="informacionLaboral" />
                        </div>

                        <div class="col-6">
                            <label class="col-form-label">Telefono de la empresa</label>
                            <input id="telefonoEmpresa" name="telefonoEmpresa" class="form-control form-control-sm col-form-label Telefono" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <div class="col-3">
                            <label class="col-form-label">Ext. RRHH</label>
                            <input id="extensionRRHH" name="extensionRRHH" class="form-control form-control-sm col-form-label Extension" type="text" data-parsley-group="informacionLaboral" data-parsley-required-message="Requerido" />
                        </div>
                        <div class="col-3">
                            <label class="col-form-label">Ext. cliente</label>
                            <input id="extensionCliente" name="extensionCliente" class="form-control form-control-sm col-form-label Extension" type="text" data-parsley-group="informacionLabral" data-parsley-required-message="Requerido" />
                        </div>
                    </div>

                    <!-- AQUI TEMRMINA INFO GENERAL Y EMPIEZA UBICACION DE LA EMPRESA -->
                    <div class="form-group form-row">
                        <div class="col-12">
                            <h6 class="border-top border-gray pt-1">Dirección empresa</h6>
                        </div>
                        <div class="col">
                            <label class="col-form-label">Departamento empresa</label>
                            <select name="departamentoEmpresa" id="departamentoEmpresa" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-deptoEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-deptoEmpresa"></span>
                        </div>
                        <div class="col">
                            <label class="col-form-label">Municipio</label>
                            <select disabled="disabled" name="municipioEmpresa" id="municipioEmpresa" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-municipioEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-municipioEmpresa"></span>
                        </div>

                        <div class="col">
                            <label class="col-form-label">Ciudad/Poblado empresa</label>
                            <select disabled="disabled" name="ciudadEmpresa" id="ciudadEmpresa" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-ciudadEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-ciudadEmpresa"></span>
                        </div>

                        <div class="col">
                            <label class="col-form-label">Barrio/Colonia empresa</label>
                            <select disabled="disabled" name="barrioColoniaEmpresa" id="barrioColoniaEmpresa" class="form-control form-control-sm col-form-label buscadorddl" required="required" data-parsley-errors-container="#error-coloniaEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-coloniaEmpresa"></span>
                        </div>

                        <div class="col-12">
                            <label class="col-form-label">Detalle dirección</label>
                            <input placeholder="Calle, avenida, bloque, etc" id="direccionDetalladaEmpresa" name="direccionDetalladaEmpresa" class="form-control form-control-sm col-form-label" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>

                        <div class="col-12">
                            <label class="col-form-label">Referencia ubicación empresa</label>
                            <textarea id="referenciaDireccionDetalladaEmpresa" required="required" class="form-control form-control-sm col-form-label" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2" data-parsley-group="informacionLaboral"></textarea>
                        </div>

                        <div class="col-6">
                            <label class="col-form-label">Fuente otros ingresos</label>
                            <input id="fuenteOtrosIngresos" name="fuenteOtrosIngresos" class="form-control form-control-sm col-form-label" type="text" data-parsley-group="informacionLaboral" />
                        </div>

                        <div class="col-6">
                            <label class="col-form-label">Valor otros ingresos</label>
                            <input id="valorOtrosIngresos" class="form-control form-control-sm col-form-label MascaraCantidad" type="text" data-parsley-group="informacionLaboral" />
                        </div>
                    </div>
                </div>
                <!-- INFORMACION DEL CONYUGUE -->
                <div id="step-5" class="form-section p-3">

                    <h6 class="border-bottom border-gray pb-2">Información conyugal</h6>

                    <div class="form-group form-row">
                        <div class="col-12">
                            <label class="col-form-label">Nombres</label>
                            <input id="nombresConyugue" class="form-control form-control-sm col-form-label infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                        <div class="col-12">
                            <label class="col-form-label">Apellidos</label>
                            <input id="apellidosConyugue" class="form-control form-control-sm col-form-label infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>

                        <div class="col-12">
                            <label class="col-form-label">Identidad</label>
                            <input id="identidadConyugue" class="form-control form-control-sm col-form-label infoConyugal identidad" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                        <div class="col-6">
                            <label class="col-form-label">Fecha nacimiento</label>
                            <input id="fechaNacimientoConyugue" class="form-control form-control-sm col-form-label infoConyugal datepicker" type="date" required="required" data-parsley-group="informacionConyugal" />
                        </div>

                        <div class="col-6">
                            <label class="col-form-label">Telefono</label>
                            <input id="telefonoConyugue" class="form-control form-control-sm col-form-label infoConyugal Telefono" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                        <div class="col-12">
                            <label class="col-form-label">Lugar de trabajo</label>
                            <input id="lugarTrabajoConyugue" class="form-control form-control-sm col-form-label infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>

                        <div class="col-6">
                            <label class="col-form-label">Ingreso mensual</label>
                            <input id="ingresoMensualesConyugue" class="form-control form-control-sm col-form-label MascaraCantidad" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>

                        <div class="col-6">
                            <label class="col-form-label">Telefono trabajo</label>
                            <input id="telefonoTrabajoConyugue" class="form-control form-control-sm col-form-label infoConyugal Telefono" type="text" required="required" data-parsley-group="informacionConyugal" />

                        </div>
                    </div>
                </div>
                <!-- REFERENCIAS PERSONALES DEL CLIENTE -->
                <div id="step-6" class="form-section p-3">
                    <h6 class="border-bottom border-gray pb-2">Referencias personales del cliente</h6>

                    <div class="mt-0 header-title justify-content-end">
                        <button id="btnNuevaReferencia" type="button" class="btn btn-success waves-effect waves-light">
                            Nuevo
                        </button>
                    </div>
                    <table class="table-bordered display compact nowrap table-condensed table-hover dataTable" style="width: 100%" role="grid" id="datatable-buttons">
                        <thead>
                            <tr>
                                <th>Nombre completo</th>
                                <th>Lugar de trabajo ref</th>
                                <th>Tiempo de conocer ref</th>
                                <th>Telefono ref</th>
                                <th>Parentesco ref</th>
                            </tr>
                        </thead>
                        <tbody>
                        </tbody>
                    </table>
                </div>
                <!-- DOCUMENTACION -->
                <div id="step-7" class="form-section p-3">
                    <h6 class="border-bottom border-gray pb-2">Documentación</h6>

                    <div class="form-group row text-center" id="DivDocumentacion">
                    </div>
                </div>
            </div>
        </div>
    </form>

    <div class="modal fade" id="modalAddReferencia" tabindex="-1" role="dialog" aria-labelledby="modalAddReferenciaLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="Agregar referenciaLabel">Agregar referencia</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <form id="addReferencia-form" action="#">
                    <div class="modal-body">
                        <div class="form-group row">
                            <label class="col-sm-5 col-form-label">Nombre completo ref</label>
                            <div class="col-sm-7">
                                <input id="nombreCompletoRef" class="form-control" type="text" required="required" data-parsley-group="referenciasPersonales" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-5 col-form-label">Lugar de trabajo ref</label>
                            <div class="col-sm-7">
                                <input id="lugarTrabajoRef" class="form-control" type="text" required="required" data-parsley-group="referenciasPersonales" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-5 col-form-label">Tiempo de conocer ref</label>
                            <div class="col-sm-7">
                                <select name="tiempoConocerRef" id="tiempoConocerRef" class="form-control" data-parsley-group="informacionPrestamo" required="required">
                                    <option value="">Seleccione una opción</option>
                                </select>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-5 col-form-label">Telefono ref</label>
                            <div class="col-sm-7">
                                <input id="telefonoRef" class="form-control Telefono" type="text" required="required" data-parsley-group="referenciasPersonales" />
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-5 col-form-label">Parentesco</label>
                            <div class="col-sm-7">
                                <select name="parentescoRef" id="parentescoRef" class="form-control" required="required" data-parsley-group="referenciasPersonales">
                                    <option value="">Seleccione una opción</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <input value="Guardar" type="submit" class="btn btn-primary waves-effect waves-light mr-1 send_add" />
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </form>
            </div>
        </div>
    </div>

    <script src="/Scripts/js/jquery.min.js"></script>
    <script src="/Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="/Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(document).ready(function () {
            $(".MascaraCantidad").inputmask("decimal", {
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
            $(".Telefono").inputmask("9999-9999");
            $(".Extension").inputmask("999999");
            $(".identidad").inputmask("9999999999999");
            $(".formatoRTN").inputmask("99999999999999");
        });
    </script>
    <!-- SCRIPTS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.responsive.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>

    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/app/CoreMovil/SolicitudesCredito_Registrar.js?v=55554545454"></script>
</body>
</html>
