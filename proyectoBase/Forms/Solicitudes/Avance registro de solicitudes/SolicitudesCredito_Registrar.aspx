<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SolicitudesCredito_Registrar.aspx.cs" Inherits="SolicitudesCredito_Registrar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Ingresar solicitud de crédito</title>
    <!-- BOOTSTRAP -->
    <link href="/CSS/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/style.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="/Scripts/plugins/datatables/dataTables.bootstrap4.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/CSS/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/CSS/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
</head>
<body class="EstiloBody">
    <!-- FORMULARIO INGRESO DE SOLICITUD DE CREDITO -->
    <asp:Label runat="server" ID="lblAlerta" CssClass="h5 text-danger"></asp:Label>
    <form runat="server" id="frmSolicitud" class="form-demo" action="#" data-parsley-excluded="[disabled]">
        <div id="smartwizard" class="">
            <ul>
                <li><a href="#step-1">1<br />
                    <small>Información del préstamo</small></a></li>
                <li><a href="#step-2">2<br />
                    <small>Información personal</small></a></li>
                <li><a href="#step-3">3<br />
                    <small>Información domicilio</small></a></li>
                <li><a href="#step-4">4<br />
                    <small>Información laboral</small></a></li>
                <li><a href="#step-5">5<br />
                    <small>Información conyugal</small></a></li>
                <li><a href="#step-6">6<br />
                    <small>Referencias personales</small></a></li>
                <li><a href="#step-7">7<br />
                    <small>Documentación</small></a></li>
            </ul>
            <div>
                <!-- INFORMACION DEL PRESTAMO Y LA SOLICITUD -->
                <div id="step-1" class="form-section">
                    <div class="float-right" id="spinnerCargando">
                        <div class="spinner-border" role="status">
                            <span class="sr-only"></span>
                        </div>
                    </div>
                    <h5 class="border-bottom border-gray pb-2">Información del préstamo </h5>

                    <!-- TIPO DE SOLICITUD, TIPO DE PRODUCTO, ORIGEN -->
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Tipo de solicitud</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="tipoSolicitud" Enabled="false" CssClass="form-control" type="text" Text="NUEVO" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <label class="col-sm-1 col-form-label">Producto</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="tipoPrestamo" ReadOnly="true" Enabled="false" CssClass="form-control" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <label class="col-sm-1 col-form-label" id="titleOrigen" style="display: none;">Origen</label>
                        <div class="col-sm-3">
                            <select name="ddlOrigen" id="ddlOrigen" class="form-control form-control-sm" disabled="disabled" style="display: none;" data-parsley-group="informacionPrestamo" required="required">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                    </div>
                    <!-- NOMBRE COMPLETO DEL CLIENTE -->
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Nombre completo</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="primerNombreCliente" placeholder="Primer nombre" CssClass="form-control" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:TextBox ID="SegundoNombreCliente" placeholder="Segundo nombre" CssClass="form-control" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="primerApellidoCliente" placeholder="Primer apellido" CssClass="form-control" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-3">
                            <asp:TextBox ID="segundoApellidoCliente" placeholder="Segundo apellido" CssClass="form-control" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <!-- IDENTIDAD, RTN, INGRESOS -->
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">No. Identidad</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="identidadCliente" CssClass="form-control identidad" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <label class="col-sm-2 col-form-label">RTN numérico</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="rtnCliente" CssClass="form-control formatoRTN" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                        <label class="col-sm-2 col-form-label">Ingresos precalificado</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="ingresosPrecalificado" CssClass="form-control MascaraCantidad" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <!-- PMO SUGERIDO, PLAZO, CUOTA -->
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">PMO sugerido</label>
                        <div class="col-sm-2">
                            <select name="pmoSugeridoSeleccionado" id="pmoSugeridoSeleccionado" disabled="disabled" class="form-control" data-parsley-group="informacionPrestamo" required="required">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                        <label class="col-sm-2 col-form-label" id="lblPlazoPMO">Plazo</label>
                        <div class="col-sm-2">
                            <input id="plazoPmoSeleccionado" readonly="readonly" class="form-control MascaraEnteros" type="text" required="required" data-parsley-group="informacionPrestamo" />
                        </div>
                        <label class="col-sm-2 col-form-label" id="lblCuotaMaxima">Cuota</label>
                        <div class="col-sm-2">
                            <input id="cutoaQuinceal" class="form-control MascaraCantidad" readonly="readonly" type="text" required="required" data-parsley-group="informacionPrestamo" />
                        </div>
                    </div>
                    <!-- PRESTAMO EFECTIVO -->
                    <div class="form-group row divPrestamoEfectivo" style="display: none;">
                        <label id="lblMontoPmo" class="col-sm-2 col-form-label">Monto</label>
                        <div class="col-sm-2">
                            <input id="txtMontoPmoEfectivo" class="form-control MascaraCantidad" disabled="disabled" type="text" required="required" data-parsley-group="informacionPrestamo" />
                        </div>
                    </div>
                    <!-- PRESTAMO CON PRIMA -->
                    <div class="form-group row divPrestamoVehiculo" style="display: none;">
                        <label id="lblValorVehiculo" class="col-sm-2 col-form-label">Valor Vehículo</label>
                        <div class="col-sm-2">
                            <input id="txtValorVehiculo" class="form-control MascaraCantidad" disabled="disabled" type="text" required="required" data-parsley-group="informacionPrestamo" />
                        </div>
                        <label id="lblPrima" class="col-sm-2 col-form-label">Valor Prima</label>
                        <div class="col-sm-2">
                            <input id="txtPrima" class="form-control MascaraCantidad" disabled="disabled" type="text" required="required" data-parsley-group="informacionPrestamo" />
                            <span id="error-prima" style="display: none;"></span>
                        </div>
                        <label id="lblValorFinanciar" class="col-sm-2 col-form-label">Valor financiar</label>
                        <div class="col-sm-2">
                            <input id="txtValorFinanciar" disabled="disabled" required="required" readonly="readonly" class="form-control MascaraCantidad" type="text" data-parsley-group="informacionPrestamo" />
                            <span id="error-valorFinanciar"></span>
                        </div>
                    </div>
                    <!-- PRESTAMOS SUGERIDOS -->
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label" id="titlePrestamosSugeridos" style="display: none;">Prestamos disponibles</label>
                        <div class="col-sm-10" id="divPmosSugeridos" style="display: none;">
                            <select name="pmosSugeridos" id="pmosSugeridos" class="form-control" data-parsley-group="informacionPrestamo" required="required">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                    </div>
                </div>
                <!-- INFORMACION PERSONAL DEL CLIENTE -->
                <div id="step-2" class="form-section">
                    <h5 class="border-bottom border-gray pb-2">Información personal</h5>

                    <!-- PROFESION, NACIONALIDAD, FECHA DE NACIMIENTO -->
                    <div class="form-group row">
                        <label class="col-sm-1 col-form-label">Profesión</label>
                        <div class="col-sm-2">
                            <input id="profesion" class="form-control" type="text" required="required" data-parsley-group="informacionPersonal" />
                        </div>
                        <label class="col-sm-1 col-form-label">Nacionalidad</label>
                        <div class="col-sm-2">
                            <select name="nacionalidad" id="nacionalidad" class="form-control buscadorddl" required="required" data-parsley-group="informacionPersonal">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                        <label class="col-sm-2 col-form-label">Fecha de Nac.</label>
                        <div class="col-sm-2">
                            <input id="fechaNacimiento" class="form-control datepicker" type="date" required="required" data-parsley-group="informacionPersonal" />
                        </div>
                    </div>
                    <!-- EMAIL, TELEFONO,TIPO DE VIVIENDA -->
                    <div class="form-group row">
                        <label class="col-sm-1 col-form-label">Email</label>
                        <div class="col-sm-2">
                            <input id="correoElectronico" class="form-control" type="email" required="required" data-parsley-group="informacionPersonal" />
                        </div>
                        <label class="col-sm-1 col-form-label">Télefono</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="numeroTelefono" CssClass="form-control Telefono" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                        </div>
                        <label class="col-sm-2 col-form-label">Edad cliente</label>
                        <div class="col-sm-2">
                            <input id="edadCliente" class="form-control" readonly=" " type="text" data-parsley-group="informacionPersonal" />
                        </div>
                    </div>
                    <!-- SEXO -->
                    <div class="form-group row">
                        <div class="col-sm-10 border border-gray">
                            <label class="col-form-label col-sm-2">Sexo</label>
                            <div class="form-check form-check-inline">
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
                    <div class="form-group row">
                        <div class="col-sm-10 border border-gray" id="divEstadoCivil">
                            <label class="col-form-label col-sm-2">Estado civil</label>
                        </div>
                    </div>
                    <!-- TIPO DE VIVIENDA -->
                    <div class="form-group row">
                        <div class="col-sm-10 border border-gray">
                            <div class="form-group row">
                                <div class="col-sm-12">&nbsp;</div>
                                <label class="col-form-label col-sm-2">Tipo de vivienda</label>
                                <div class="col-sm-3">
                                    <select name="vivivenda" id="vivivenda" class="form-control buscadorddl" required="required" data-parsley-group="informacionPersonal">
                                        <option value="">Seleccione una opción</option>
                                    </select>
                                </div>
                                <div class="col-sm-5"></div>
                            </div>
                            <!--TIEMPO DE RESIDIR-->
                            <div class="form-group row">
                                <div class="col-sm-10">
                                    <label class="col-form-label">Tiempo residir</label>
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
                    </div>
                </div>
                <!-- INFORMACION DEL DOMICILIO DEL CLIENTE -->
                <div id="step-3" class="form-section">
                    <h5 class="border-bottom border-gray pb-2">Información domicilio</h5>

                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Departamento</label>
                        <div class="col-sm-3">
                            <select name="departamento" id="departamento" class="form-control buscadorddl" required="required" data-parsley-errors-container="#error-depto" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-depto"></span>
                        </div>

                        <label class="col-sm-3 col-form-label">Municipio</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="municipio" id="municipio" class="form-control buscadorddl" required="required" data-parsley-errors-container="#error-municipio" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-municipio"></span>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Ciudad/Poblado</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="ciudad" id="ciudad" class="form-control buscadorddl" required="required" data-parsley-errors-container="#error-ciudad" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-ciudad"></span>
                        </div>
                        <label class="col-sm-3 col-form-label">Barrio o Colonia</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="barrioColonia" id="barrioColonia" class="form-control buscadorddl" required="required" data-parsley-errors-container="#error-colonia" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-colonia"></span>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Telefono casa</label>
                        <div class="col-sm-3">
                            <input id="telefonoCasa" class="form-control Telefono" type="text" data-parsley-group="informacionDomiciliar" />
                        </div>
                        <label class="col-sm-3 col-form-label">Telefono movil</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="telefonoMovil" CssClass="form-control Telefono" type="text" required="required" data-parsley-group="informacionDomiciliar" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Detalle dirección</label>
                        <div class="col-sm-3">
                            <input placeholder="Calle, avenida, bloque, etc" id="direccionDetallada" class="form-control" type="text" required="required" data-parsley-group="informacionDomiciliar" />
                        </div>
                        <div class="col-sm-3"></div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-12 col-form-label text-center">Referencias del domicilio</label>
                        <div class="col-sm-12">
                            <textarea id="referenciaDireccionDetallada" required="required" class="form-control" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2" data-parsley-group="informacionDomiciliar"></textarea>
                        </div>
                        <div class="col-sm-12"></div>
                    </div>
                </div>
                <!-- INFORMACION LABORAL DEL CLIENTE -->
                <div id="step-4" class="form-section">
                    <h5 class="border-bottom border-gray pb-2">Información laboral</h5>

                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Nombre del trabajo</label>
                        <div class="col-sm-3">
                            <input id="nombreDelTrabajo" name="nombreDelTrabajo" class="form-control" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <label class="col-sm-3 col-form-label">Ingresos mensuales</label>
                        <div class="col-sm-3">
                            <input id="ingresosMensuales" class="form-control MascaraCantidad" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Puesto asignado</label>
                        <div class="col-sm-3">
                            <input id="puestoAsignado" name="puestoAsignado" class="form-control" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <label class="col-sm-3 col-form-label">Fecha de ingreso</label>
                        <div class="col-sm-3">
                            <input id="fechaIngreso" name="fechaIngreso" class="form-control datepicker" type="date" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Telefono de la empresa</label>
                        <div class="col-sm-3">
                            <input id="telefonoEmpresa" name="telefonoEmpresa" class="form-control Telefono" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <label class="col-sm-2 col-form-label">Extension RRHH</label>
                        <div class="col-sm-1">
                            <input id="extensionRRHH" name="extensionRRHH" class="form-control Extension" type="text" data-parsley-group="informacionLaboral" data-parsley-required-message="Requerido" />
                        </div>
                        <label class="col-sm-2 col-form-label">Extension cliente</label>
                        <div class="col-sm-1">
                            <input id="extensionCliente" name="extensionCliente" class="form-control Extension" type="text" data-parsley-group="informacionLabral" data-parsley-required-message="Requerido" />
                        </div>
                    </div>
                    <!-- AQUI TEMRMINA INFO GENERAL Y EMPIEZA UBICACION DE LA EMPRESA -->
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <hr />
                            <h5 class="">Dirección Empresa</h5>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Departamento empresa</label>
                        <div class="col-sm-3">
                            <select name="departamentoEmpresa" id="departamentoEmpresa" class="form-control buscadorddl" required="required" data-parsley-errors-container="#error-deptoEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-deptoEmpresa"></span>
                        </div>
                        <label class="col-sm-3 col-form-label">Municipio</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="municipioEmpresa" id="municipioEmpresa" class="form-control buscadorddl" required="required" data-parsley-errors-container="#error-municipioEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-municipioEmpresa"></span>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Ciudad/Poblado empresa</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="ciudadEmpresa" id="ciudadEmpresa" class="form-control buscadorddl" required="required" data-parsley-errors-container="#error-ciudadEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-ciudadEmpresa"></span>
                        </div>
                        <label class="col-sm-3 col-form-label">Barrio o Colonia empresa</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="barrioColoniaEmpresa" id="barrioColoniaEmpresa" class="form-control buscadorddl" required="required" data-parsley-errors-container="#error-coloniaEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-coloniaEmpresa"></span>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Detalle dirección</label>
                        <div class="col-sm-3">
                            <input placeholder="Calle, avenida, bloque, etc" id="direccionDetalladaEmpresa" name="direccionDetalladaEmpresa" class="form-control" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <div class="col-sm-6"></div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-12 col-form-label text-center">Referencia ubicación empresa</label>
                        <div class="col-sm-12">
                            <textarea id="referenciaDireccionDetalladaEmpresa" required="required" class="form-control" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2" data-parsley-group="informacionLaboral"></textarea>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Fuente otros ingresos</label>
                        <div class="col-sm-3">
                            <input id="fuenteOtrosIngresos" name="fuenteOtrosIngresos" class="form-control" type="text" data-parsley-group="informacionLaboral" />
                        </div>
                        <label class="col-sm-3 col-form-label">Valor otros ingresos</label>
                        <div class="col-sm-3">
                            <input id="valorOtrosIngresos" class="form-control MascaraCantidad" type="text" data-parsley-group="informacionLaboral" />
                        </div>
                    </div>
                </div>
                <!-- INFORMACION DEL CONYUGUE -->
                <div id="step-5" class="form-section">

                    <h5 class="border-bottom border-gray pb-2">Información conyugal</h5>

                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Nombres del conyugue</label>
                        <div class="col-sm-3">
                            <input id="nombresConyugue" class="form-control infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                        <label class="col-sm-3 col-form-label">Apellidos del conyugue</label>
                        <div class="col-sm-3">
                            <input id="apellidosConyugue" class="form-control infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Identidad conyugue</label>
                        <div class="col-sm-3">
                            <input id="identidadConyugue" class="form-control infoConyugal identidad" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                        <label class="col-sm-3 col-form-label">Fecha nacimiento</label>
                        <div class="col-sm-3">
                            <input id="fechaNacimientoConyugue" class="form-control infoConyugal datepicker" type="date" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Telefono del conyugue</label>
                        <div class="col-sm-3">
                            <input id="telefonoConyugue" class="form-control infoConyugal Telefono" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                        <label class="col-sm-3 col-form-label">Lugar de trabajo</label>
                        <div class="col-sm-3">
                            <input id="lugarTrabajoConyugue" class="form-control infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Ingreso mensual</label>
                        <div class="col-sm-3">
                            <input id="ingresoMensualesConyugue" class="form-control infoConyugal MascaraCantidad" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                        <label class="col-sm-3 col-form-label">Telefono trabajo </label>
                        <div class="col-sm-3">
                            <input id="telefonoTrabajoConyugue" class="form-control infoConyugal Telefono" type="text" required="required" data-parsley-group="informacionConyugal" />
                        </div>
                    </div>
                </div>
                <!-- REFERENCIAS PERSONALES DEL CLIENTE -->
                <div id="step-6" class="form-section">
                    <h5 class="border-bottom border-gray pb-2">Referencias personales del cliente</h5>

                    <div class="mt-0 header-title">
                        <button id="btnNuevaReferencia" type="button" class="btn btn-success waves-effect waves-light float-right">
                            Nuevo
                        </button>
                    </div>
                    <br />
                    <br />
                    <table class="table table-striped table-bordered dt-responsive nowrap" id="datatable-buttons">
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
                <div id="step-7" class="form-section">
                    <h5 class="border-bottom border-gray pb-2">Documentación</h5>

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

    <!--modal de agregar referencia personal -->
    <%--<div id="modalAddReferencia" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="myModalLabel">Agregar referencia</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
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
    </div>--%>

    <!-- modal reiniciar solicitud -->
    <div id="modalReiniciarSolicitud" class="modal fade bs-example-modal-lg" role="dialog" aria-labelledby="modalReiniciarSolicitudLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title mt-0" id="modalReiniciarSolicitudLabel">Reiniciar solicitud</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                </div>
                <div class="modal-body">
                    ¿Está seguro de que desear reiniciar la solcitud?<br />
                    Se recomienda llevar a cabo esta acción antes de iniciar el ingreso de una nueva solicitud
                </div>
                <div class="modal-footer">
                    <button id="btnReiniciarSolicitud" data-dismiss="modal" class="btn btn-danger waves-effect">
                        Confirmar
                    </button>
                    <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                        Cancelar
                    </button>
                </div>
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
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/plugins/datatables/jquery.dataTables.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/dataTables.buttons.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.bootstrap4.min.js"></script>
    <script src="/Scripts/plugins/datatables/jszip.min.js"></script>
    <script src="/Scripts/plugins/datatables/buttons.html5.min.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>

    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/app/solicitudes/SolicitudesCredito_Registrar.js?v=202009261308"></script>
</body>
</html>
