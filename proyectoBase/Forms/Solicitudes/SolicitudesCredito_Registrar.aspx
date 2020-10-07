<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_Registrar.aspx.cs" Inherits="SolicitudesCredito_Registrar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Ingresar solicitud de crédito</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <style>
        html {
            background-color: rgb(255,255,255) !important;
        }

        .card {
            border: none;
            -webkit-box-shadow: none !important;
            box-shadow: none !important;
        }
    </style>
</head>
<body>
    <div class="card mb-0">
        <div class="card-header pb-1 pt-1">
            <h5>Nueva solicitud de crédito</h5>
        </div>
        <div class="card-body">
            <form runat="server" id="frmSolicitud" class="" action="#" data-parsley-excluded="[disabled]">
                <div id="smartwizard" class="h-100">
                    <ul>
                        <li><a href="#step-1" class="pt-3 pb-2 font-12">(1) Información del préstamo</a></li>
                        <li><a href="#step-2" class="pt-3 pb-2 font-12">(2) Información personal</a></li>
                        <li><a href="#step-3" class="pt-3 pb-2 font-12">(3) Información domicilio</a></li>
                        <li><a href="#step-4" class="pt-3 pb-2 font-12">(4) Información laboral</a></li>
                        <li><a href="#step-5" class="pt-3 pb-2 font-12">(5) Información conyugal</a></li>
                        <li><a href="#step-6" class="pt-3 pb-2 font-12">(6) Referencias personales</a></li>
                        <li><a href="#step-7" class="pt-3 pb-2 font-12">(7) Documentación</a></li>
                        <li><a href="#step-7" class="pt-3 pb-2 font-12">(8) Información garantía</a></li>
                    </ul>
                    <div>
                        <!-- Información principal -->
                        <div id="step-1" class="form-section">
                            
                            <!-- loader -->
                            <div class="float-right" id="spinnerCargando" runat="server" visible="false">
                                <div class="spinner-border" role="status">
                                    <span class="sr-only"></span>
                                </div>
                            </div>
                            <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-8 p-0">
                                    <h6 class="mt-1">Información del préstamo</h6>
                                </div>
                                <div class="col-4 align-self-end text-right">
                                    <label>Tipo de solicitud: <span class="btn btn-sm btn-info mb-0" id="lblTipodeSolicitud" runat="server">NUEVO</span></label>
                                    <label>Producto: <span class="btn btn-sm btn-info mb-0" id="lblProducto" runat="server">Prestadito Motos</span></label>
                                </div>
                            </div>

                            <div class="row mb-0">

                                <!-- Información del cliente -->
                                <div class="col-lg-6">
                                    <h6 class="mt-3 mb-1">Datos del cliente</h6>

                                    <div class="form-group row">
                                        <div class="col-6">
                                            <label class="col-form-label">No. Identidad</label>
                                            <asp:TextBox ID="txtIdentidadCliente" CssClass="form-control form-control-sm mascara-identidad" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="col-form-label">RTN numérico</label>
                                            <asp:TextBox ID="txtRtnCliente" CssClass="form-control form-control-sm mascara-rtn" type="text" Enabled="false" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Primer nombre</label>
                                            <asp:TextBox ID="txtPrimerNombre" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Segundo nombre</label>
                                            <asp:TextBox ID="txtSegundoNombre" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Primer apellido</label>
                                            <asp:TextBox ID="txtPrimerApellido" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Segundo apellido</label>
                                            <asp:TextBox ID="txtSegundoApellido" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-12">
                                            <label class="col-form-label">Ingresos precalificado</label>
                                            <asp:TextBox ID="txtIngresosPrecalificados" CssClass="form-control form-control-sm mascara-cantidad" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <!-- Información del préstamo máximo -->
                                <div class="col-lg-6">
                                    <h6 class="mt-3 mb-1">Información préstamo máximo</h6>
                                    <div class="form-group row">
                                        <div class="col-sm-4">
                                            <label class="col-form-label">PMO máximo sugerido</label>
                                            <asp:TextBox ID="txtPrestamoMaximo" CssClass="form-control form-control-sm mascara-cantidad" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:Label CssClass="col-form-label" ID="lblTituloPlazoMaximo" runat="server" Text="Plazo" AssociatedControlID="txtPlazoMaximo" />
                                            <asp:TextBox ID="txtPlazoMaximo" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-4">
                                            <asp:Label CssClass="col-form-label" ID="lblTituloCuotaMaxima" runat="server" Text="Cuota" AssociatedControlID="txtCuotaMaxima" />
                                            <asp:TextBox ID="txtCuotaMaxima" CssClass="form-control form-control-sm mascara-cantidad" type="text" Enabled="false" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <h6 class="mt-1 mb-1">Montos de la solicitud</h6>
                                    <div class="form-group row">
                                        <!-- Monto global -->
                                        <div class="col-sm-3">                                            
                                            <asp:Label CssClass="col-form-label" ID="lblTituloMontoPrestmo" runat="server" Text="Valor global" AssociatedControlID="txtValorGlobal" />
                                            <asp:TextBox ID="txtValorGlobal" CssClass="form-control form-control-sm mascara-cantidad" type="text" required="required" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- Valor de la prima -->
                                        <div class="col-sm-3" id="divPrima" runat="server">
                                            <label class="col-form-label">Valor de la prima</label>
                                            <asp:TextBox ID="txtValorPrima" CssClass="form-control form-control-sm mascara-cantidad" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- Valor a Financiar -->
                                        <div class="col-sm-3" id="divValorFinanciar" runat="server">
                                            <label class="col-form-label">Valor a Financiar</label>
                                            <asp:TextBox ID="txtValorFinanciar" CssClass="form-control form-control-sm mascara-cantidad" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                                        </div>
                                        <!-- Origen -->
                                        <div class="col-sm-3" runat="server" id="divOrigen">
                                            <label class="col-form-label">Origen</label>
                                            <asp:DropDownList ID="ddlOrigen" runat="server" CssClass="form-control form-control-sm" Enabled="false" data-parsley-group="informacionPrestamo" required="required"></asp:DropDownList>
                                        </div>
                                        <!-- Préstamos disponibles -->
                                        <div class="col-12" id="divPrestamosDisponibles" runat="server" visible="true">
                                            <label class="col-form-label">Préstamos disponibles</label>       
                                            <asp:DropDownList ID="ddlPrestamosDisponibles" runat="server" CssClass="form-control form-control-sm" data-parsley-group="informacionPrestamo" required="required"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Información personal -->
                        <div id="step-2" class="form-section">

                            <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-12 p-0">
                                    <h6 class="mt-1">Información personal</h6>
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="col-sm-3">
                                    <label class="col-form-label">Nacionalidad</label>
                                    <asp:DropDownList ID="ddlNacionalidad" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal"></asp:DropDownList>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Profesión</label>
                                    <asp:TextBox ID="txtProfesion" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Email</label>
                                    <asp:TextBox ID="txtCorreoElectronico" CssClass="form-control form-control-sm mascara-telefono" Enabled="false" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Télefono</label>
                                    <asp:TextBox ID="txtNumeroTelefono" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <div class="form-group row">                                
                                <div class="col-3">
                                    <label class="col-form-label">Fecha de nacimiento</label>
                                    <asp:TextBox ID="txtFechaDeNacimiento" CssClass="form-control form-control-sm datepicker" type="date" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-3">
                                    <label class="col-form-label">Edad del cliente</label>
                                    <asp:TextBox ID="txtEdadDelCliente" CssClass="form-control form-control-sm" Enabled="false" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                </div>           
                                
                                <div class="col-3">
                                    <label class="col-form-label">Estado civil</label>
                                    <asp:TextBox ID="TextBox1" CssClass="form-control form-control-sm" Enabled="false" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                    <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal"></asp:DropDownList>
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
                                    <input id="telefonoCasa" class="form-control mascara-telefono" type="text" data-parsley-group="informacionDomiciliar" />
                                </div>
                                <label class="col-sm-3 col-form-label">Telefono movil</label>
                                <div class="col-sm-3">
                                    <asp:TextBox ID="telefonoMovil" CssClass="form-control mascara-telefono" type="text" required="required" data-parsley-group="informacionDomiciliar" runat="server"></asp:TextBox>
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
                                    <input id="ingresosMensuales" class="form-control mascara-cantidad" type="text" required="required" data-parsley-group="informacionLaboral" />
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
                                    <input id="telefonoEmpresa" name="telefonoEmpresa" class="form-control mascara-telefono" type="text" required="required" data-parsley-group="informacionLaboral" />
                                </div>
                                <label class="col-sm-2 col-form-label">Extension RRHH</label>
                                <div class="col-sm-1">
                                    <input id="extensionRRHH" name="extensionRRHH" class="form-control mascara-extension" type="text" data-parsley-group="informacionLaboral" data-parsley-required-message="Requerido" />
                                </div>
                                <label class="col-sm-2 col-form-label">Extension cliente</label>
                                <div class="col-sm-1">
                                    <input id="extensionCliente" name="extensionCliente" class="form-control mascara-extension" type="text" data-parsley-group="informacionLabral" data-parsley-required-message="Requerido" />
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
                                    <input id="valorOtrosIngresos" class="form-control mascara-cantidad" type="text" data-parsley-group="informacionLaboral" />
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
                                    <input id="identidadConyugue" class="form-control infoConyugal mascara-identidad" type="text" required="required" data-parsley-group="informacionConyugal" />
                                </div>
                                <label class="col-sm-3 col-form-label">Fecha nacimiento</label>
                                <div class="col-sm-3">
                                    <input id="fechaNacimientoConyugue" class="form-control infoConyugal datepicker" type="date" required="required" data-parsley-group="informacionConyugal" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Telefono del conyugue</label>
                                <div class="col-sm-3">
                                    <input id="telefonoConyugue" class="form-control infoConyugal mascara-telefono" type="text" required="required" data-parsley-group="informacionConyugal" />
                                </div>
                                <label class="col-sm-3 col-form-label">Lugar de trabajo</label>
                                <div class="col-sm-3">
                                    <input id="lugarTrabajoConyugue" class="form-control infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" />
                                </div>
                            </div>
                            <div class="form-group row">
                                <label class="col-sm-3 col-form-label">Ingreso mensual</label>
                                <div class="col-sm-3">
                                    <input id="ingresoMensualesConyugue" class="form-control infoConyugal mascara-cantidad" type="text" required="required" data-parsley-group="informacionConyugal" />
                                </div>
                                <label class="col-sm-3 col-form-label">Telefono trabajo </label>
                                <div class="col-sm-3">
                                    <input id="telefonoTrabajoConyugue" class="form-control infoConyugal mascara-telefono" type="text" required="required" data-parsley-group="informacionConyugal" />
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
        </div>
    </div>

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
                                <input id="telefonoRef" class="form-control mascara-telefono" type="text" required="required" data-parsley-group="referenciasPersonales" />
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
            $(".mascara-telefono").inputmask("9999-9999");
            $(".mascara-extension").inputmask("9999");
            $(".mascara-identidad").inputmask("9999999999999");
            $(".mascara-rtn").inputmask("99999999999999");
        });
    </script>
    <!-- SCRIPTS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>

    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_Registrar.js?v=20200926121795"></script>
</body>
</html>
