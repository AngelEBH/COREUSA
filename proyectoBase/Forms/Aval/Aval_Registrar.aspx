<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Aval_Registrar.aspx.cs" Inherits="proyectoBase.Forms.Aval.Aval_Registrar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <link rel="shortcut icon" href="/Content/images/favicon.ico" />
    <link href="/Content/css/style.css" rel="stylesheet" />
    <title>Registrar Aval</title>
    <!-- BOOTSTRAP -->
    <link href="../../Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="../../Content/css/metismenu.min.css" rel="stylesheet" />
    <link href="../../Content/css/icons.css" rel="stylesheet" />
    <link href="../../Content/css/style.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/morris/morris.css" rel="stylesheet" />
    <!-- ARCHIVOS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <link href="../../Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="../../Content/css/font/familyRoboto.css" rel="stylesheet" />
    <link href="../../Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="../../Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="../../Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="../../Scripts/plugins/datapicker/datepicker3.css" rel="stylesheet" />
    <link href="../../CSS/Estilos_CSS.css" rel="stylesheet" />
</head>
<body class="EstiloBody">
    <h4 class="text-center">Registrar aval para el cliente:&nbsp;<asp:Label ID="lblNombreCliente" runat="server"></asp:Label>
        | Solicitud: &nbsp;<asp:Label ID="lblIDSolicitud" runat="server"></asp:Label></h4>
    <!-- FORMULARIO POR PASOS -->
    <form id="frmSolicitud" class="form-demo" action="#" data-parsley-excluded="[disabled]" runat="server">
        <div id="smartwizard" class="">
            <ul>
                <li><a href="#step-1">1<br />
                    <small>Información personal</small></a></li>
                <li><a href="#step-2">2<br />
                    <small>Información domiciliar</small></a></li>
                <li><a href="#step-3">3<br />
                    <small>Información laboral</small></a></li>
                <li><a href="#step-4">4<br />
                    <small>Información conyugal</small></a></li>
                <li><a href="#step-5">5<br />
                    <small>Documentación</small></a></li>
            </ul>
            <div>
                <!-- INFORMACION PERSONAL -->
                <div id="step-1" class="form-section">
                    <h5 class="border-bottom border-gray pb-2">Información personal</h5>
                    <!--NOMBRE COMPLETO DEL Aval-->
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Nombre completo</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="primerNombreAval" placeholder="Primer nombre" CssClass="form-control informacionPersonal" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:TextBox ID="SegundoNombreAval" placeholder="Segundo nombre" CssClass="form-control informacionPersonal" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:TextBox ID="primerApellidoAval" placeholder="Primer apellido" CssClass="form-control informacionPersonal" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-sm-2">
                            <asp:TextBox ID="segundoApellidoAval" placeholder="Segundo apellido" CssClass="form-control informacionPersonal" type="text" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <!--PROFESION, NACIONALIDAD, FECHA DE NACIMIENTO-->
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Identidad</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="identidadAval" CssClass="form-control identidad informacionPersonal" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                        </div>
                        <label class="col-sm-2 col-form-label">RTN</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="rtnAval" CssClass="form-control formatoRTN informacionPersonal" type="text" data-parsley-group="informacionPrestamo" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-2 col-form-label">Profesión</label>
                        <div class="col-sm-2">
                            <input id="profesion" class="form-control informacionPersonal" type="text" required="required" data-parsley-group="informacionPersonal" />
                        </div>
                        <label class="col-sm-2 col-form-label">Nacionalidad</label>
                        <div class="col-sm-2">
                            <select name="nacionalidad" id="nacionalidad" class="form-control buscadorddl informacionPersonal" required="required" data-parsley-group="informacionPersonal">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                    </div>
                    <!--EMAIL, TELEFONO,TIPO DE VIVIENDA-->
                    <div class="form-group row justify-content-md-center">
                        <label class="col-sm-2 col-form-label">Email</label>
                        <div class="col-sm-2">
                            <input id="correoElectronico" class="form-control informacionPersonal" type="email" required="required" data-parsley-group="informacionPersonal" />
                        </div>
                        <label class="col-sm-2 col-form-label">Télefono</label>
                        <div class="col-sm-2">
                            <asp:TextBox ID="numeroTelefono" CssClass="form-control Telefono informacionPersonal" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                        </div>
                        <label class="col-sm-2 col-form-label">Fecha de Nacimiento</label>
                        <div class="col-sm-2">
                            <input id="fechaNacimiento" class="form-control datepicker informacionPersonal" type="date" required="required" data-parsley-group="informacionPersonal" />
                        </div>
                    </div>
                    <div class="form-group row justify-content-md-center">
                        <!--SEXO-->
                        <div class="col-sm-10 border border-gray">
                            <label class="col-form-label col-sm-2">Sexo</label>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input informacionPersonal" type="radio" name="sexo" value="M" />
                                <label class="form-check-label" for="inlineRadio1">Masculino</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input informacionPersonal" type="radio" name="sexo" value="F" />
                                <label class="form-check-label" for="inlineRadio2">Femenino</label>
                            </div>
                        </div>
                        <!--SEXO-->
                    </div>
                    <!--INPUTS RADIO DE ESTADO CIVIL-->
                    <div class="form-group row justify-content-md-center">
                        <div class="col-sm-10 border border-gray" id="divEstadoCivil">
                            <label class="col-form-label col-sm-2">Estado civil</label>
                        </div>
                    </div>
                    <div class="form-group row justify-content-md-center">
                        <div class="col-sm-10 border border-gray">
                            <!--VIVIENDA-->
                            <div class="form-group row">
                                <div class="col-sm-12">&nbsp;</div>
                                <label class="col-form-label col-sm-2">Tipo de vivienda</label>
                                <div class="col-sm-3">
                                    <select name="vivivenda" id="vivivenda" class="form-control buscadorddl informacionPersonal" required="required" data-parsley-group="informacionPersonal">
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
                                        <input class="form-check-input informacionPersonal" type="radio" name="tiempoResidir" value="0" />
                                        <label class="form-check-label" for="inlineRadio1">-1 año</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input informacionPersonal" type="radio" name="tiempoResidir" value="1" />
                                        <label class="form-check-label" for="inlineRadio2">1 año</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input informacionPersonal" type="radio" name="tiempoResidir" value="2" />
                                        <label class="form-check-label" for="inlineRadio2">2 años</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input informacionPersonal" type="radio" name="tiempoResidir" value="3" />
                                        <label class="form-check-label" for="inlineRadio2">+2 años</label>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div id="step-2" class="form-section">
                    <h5 class="border-bottom border-gray pb-2">Información domiciliar</h5>

                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Departamento</label>
                        <div class="col-sm-3">
                            <select name="departamento" id="departamento" class="form-control buscadorddl informacionDomiciliar" required="required" data-parsley-errors-container="#error-depto" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-depto"></span>
                        </div>
                        <label class="col-sm-3 col-form-label">Municipio</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="municipio" id="municipio" class="form-control buscadorddl informacionDomiciliar" required="required" data-parsley-errors-container="#error-municipio" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-municipio"></span>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Ciudad</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="ciudad" id="ciudad" class="form-control buscadorddl informacionDomiciliar" required="required" data-parsley-errors-container="#error-ciudad" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-ciudad"></span>
                        </div>
                        <label class="col-sm-3 col-form-label">Barrio o Colonia</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="barrioColonia" id="barrioColonia" class="form-control buscadorddl informacionDomiciliar" required="required" data-parsley-errors-container="#error-colonia" data-parsley-group="informacionDomiciliar">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-colonia"></span>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Telefono casa</label>
                        <div class="col-sm-3">
                            <input id="telefonoCasa" class="form-control Telefono informacionDomiciliar" type="text" data-parsley-group="informacionDomiciliar" />
                        </div>
                        <label class="col-sm-3 col-form-label">Telefono movil</label>
                        <div class="col-sm-3">
                            <asp:TextBox ID="telefonoMovil" CssClass="form-control Telefono informacionDomiciliar" type="text" required="required" data-parsley-group="informacionDomiciliar" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Detalle dirección</label>
                        <div class="col-sm-3">
                            <input placeholder="Calle, avenida, bloque, etc" id="direccionDetallada" class="form-control informacionDomiciliar" type="text" required="required" data-parsley-group="informacionDomiciliar" />
                        </div>
                        <div class="col-sm-3"></div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-12 col-form-label text-center">Referencias del domicilio</label>
                        <div class="col-sm-12">
                            <textarea id="referenciaDireccionDetallada" required="required" class="form-control informacionDomiciliar" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2" data-parsley-group="informacionDomiciliar"></textarea>
                        </div>
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12"></div>
                    </div>
                </div>

                <div id="step-3" class="form-section">
                    <h5 class="border-bottom border-gray pb-2">Información laboral</h5>

                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Nombre del trabajo</label>
                        <div class="col-sm-3">
                            <input id="nombreDelTrabajo" name="nombreDelTrabajo" class="form-control informacionLaboral" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <label class="col-sm-3 col-form-label">Ingresos mensuales</label>
                        <div class="col-sm-3">
                            <input id="ingresosMensuales" class="form-control MascaraCantidad informacionLaboral" type="text" data-parsley-group="informacionLaboral" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Puesto asignado</label>
                        <div class="col-sm-3">
                            <input id="puestoAsignado" name="puestoAsignado" class="form-control informacionLaboral" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <label class="col-sm-3 col-form-label">Fecha de ingreso</label>
                        <div class="col-sm-3">
                            <input id="fechaIngreso" name="fechaIngreso" class="form-control datepicker informacionLaboral" type="date" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Telefono de la empresa</label>
                        <div class="col-sm-3">
                            <input id="telefonoEmpresa" name="telefonoEmpresa" class="form-control Telefono informacionLaboral" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <label class="col-sm-2 col-form-label">Extension RRHH</label>
                        <div class="col-sm-1">
                            <input id="extensionRRHH" name="extensionRRHH" class="form-control Extension informacionLaboral" type="text" data-parsley-group="informacionLaboral" data-parsley-required-message="Requerido" />
                        </div>
                        <label class="col-sm-2 col-form-label">Extension aval</label>
                        <div class="col-sm-1">
                            <input id="extensionAval" name="extensionAval" class="form-control Extension informacionLaboral" type="text" data-parsley-group="informacionLaboral" data-parsley-required-message="Requerido" />
                        </div>
                        <!-- AQUI TEMRMINA INFO GENERAL Y EMPIEZA UBICACION DE LA EMPRESA -->
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <hr />
                            <h5 class="">Ubicación Empresa</h5>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Departamento empresa</label>
                        <div class="col-sm-3">
                            <select name="departamentoEmpresa" id="departamentoEmpresa" class="form-control buscadorddl informacionLaboral" required="required" data-parsley-errors-container="#error-deptoEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-deptoEmpresa"></span>
                        </div>
                        <label class="col-sm-3 col-form-label">Municipio</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="municipioEmpresa" id="municipioEmpresa" class="form-control buscadorddl informacionLaboral" required="required" data-parsley-errors-container="#error-municipioEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-municipioEmpresa"></span>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Ciudad empresa</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="ciudadEmpresa" id="ciudadEmpresa" class="form-control buscadorddl informacionLaboral" required="required" data-parsley-errors-container="#error-ciudadEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-ciudadEmpresa"></span>
                        </div>
                        <label class="col-sm-3 col-form-label">Barrio o Colonia empresa</label>
                        <div class="col-sm-3">
                            <select disabled="disabled" name="barrioColoniaEmpresa" id="barrioColoniaEmpresa" class="form-control buscadorddl informacionLaboral" required="required" data-parsley-errors-container="#error-coloniaEmpresa" data-parsley-group="informacionLaboral">
                                <option value="">Seleccione una opción</option>
                            </select>
                            <span id="error-coloniaEmpresa"></span>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Detalle dirección</label>
                        <div class="col-sm-3">
                            <input placeholder="Calle, avenida, bloque, etc" id="direccionDetalladaEmpresa" name="direccionDetalladaEmpresa informacionLaboral" class="form-control" type="text" required="required" data-parsley-group="informacionLaboral" />
                        </div>
                        <div class="col-sm-6"></div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-12 col-form-label text-center">Referencia ubicación empresa</label>
                        <div class="col-sm-12">
                            <textarea id="referenciaDireccionDetalladaEmpresa" required="required" class="form-control informacionLaboral" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2" data-parsley-group="informacionLaboral"></textarea>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Fuente otros ingresos</label>
                        <div class="col-sm-3">
                            <input id="fuenteOtrosIngresos" name="fuenteOtrosIngresos" class="form-control informacionLaboral" type="text" data-parsley-group="informacionLaboral" />
                        </div>
                        <label class="col-sm-3 col-form-label">Valor otros ingresos</label>
                        <div class="col-sm-3">
                            <input id="valorOtrosIngresos" class="form-control MascaraCantidad informacionLaboral" type="text" data-parsley-group="informacionLaboral" />
                        </div>
                    </div>
                </div>

                <div id="step-4" class="form-section">
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

                <div id="step-5" class="form-section">
                    <h5 class="border-bottom border-gray pb-2">Documentación</h5>

                    <div class="form-group row text-center" id="DivDocumentacion">                        
                    </div>
                </div>
            </div>
        </div>
    </form>

    <script src="../../Scripts/js/jquery.min.js"></script>
    <script src="../../Scripts/js/bootstrap.bundle.min.js"></script>
    <script src="../../Scripts/js/metisMenu.min.js"></script>
    <script src="../../Scripts/js/jquery.slimscroll.js"></script>
    <script src="../../Scripts/js/waves.min.js"></script>
    <script src="../../Scripts/plugins/jquery-sparkline/jquery.sparkline.min.js"></script>
    <script src="../../Scripts/js/app.js"></script>
    <script src="../../Scripts/plugins/mascarasDeEntrada/js/jquery.inputmask.bundle.js"></script>
    <script>
        $(document).ready(function () {

            $("input").not($(":button")).keypress(function (evt) {
                if (evt.keyCode == 13) {
                    iname = $(this).val();
                    if (iname !== 'Submit') {
                        var fields = $(this).parents('form:eq(0),body').find('button, input, textarea, select');
                        var index = fields.index(this);
                        if (index > -1 && (index + 1) < fields.length) {
                            fields.eq(index + 1).focus();
                        }
                        return false;
                    }
                }
            });
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
            $(".Extension").inputmask("9999");
            $(".identidad").inputmask("9999999999999");
            $(".formatoRTN").inputmask("99999999999999");
        });
    </script>
    <!-- SCRIPTS NECESARIOS PARA EL FUNCIONAMIENTO DE LA PAGINA -->
    <script src="../../Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="../../Scripts/plugins/iziToast/js/iziToast.js"></script>
    <script src="../../Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="../../Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="../../Scripts/app/uploader/SolicitudesCredito_RegistrarAval/jquery.fileuploader.min.js"></script>
    <script src="../../Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="../../Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="../../Scripts/app/Aval/Aval_Registrar.js"></script>
</body>
</html>
