<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Aval_Actualizar.aspx.cs" Inherits="proyectoBase.Forms.Aval.Aval_Actualizar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Actualizar Aval</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/Content/css/icons.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
</head>
<body>
    <form id="frmSolicitud" runat="server" class="form-demo" action="#" data-parsley-excluded="[disabled]">
        <div id="smartwizard" class="">
            <ul>
                <li><a href="#step-1" class="pt-3 pb-2 font-12">Información personal</a></li>
                <li><a href="#step-2" class="pt-3 pb-2 font-12">Información domicilio</a></li>
                <li><a href="#step-3" class="pt-3 pb-2 font-12">Información laboral</a></li>
                <li><a href="#step-4" class="pt-3 pb-2 font-12">Información conyugal</a></li>
                <li><a href="#step-5" class="pt-3 pb-2 font-12">Documentación</a></li>
            </ul>
            <div>
                <div id="step-1" class="form-section">
                    <div class="float-right" id="spinnerCargando">
                        <div class="spinner-border" role="status">
                            <span class="sr-only"></span>
                        </div>
                    </div>
                    <h6 class="border-bottom border-gray pb-2">Actualizar información personal (Aval)</h6>

                    <!--NOMBRE COMPLETO DEL CLIENTE-->
                    <div class="form-group row  justify-content-md-center">
                        <label class="col-sm-1 col-form-label">Nombre</label>
                        <div class="col-sm-2">
                            <input id="primerNombreCliente" placeholder="Primer nombre" class="form-control" type="text" required="required" data-parsley-group="informacionPersonal"/>
                        </div>
                        <div class="col-sm-2">
                            <input id="SegundoNombreCliente" placeholder="Segundo nombre" class="form-control" type="text" required="required" data-parsley-group="informacionPersonal"/>
                        </div>
                        <div class="col-sm-2">
                            <input id="primerApellidoCliente" placeholder="Primer apellido" class="form-control" type="text" required="required" data-parsley-group="informacionPersonal"/>
                        </div>
                        <div class="col-sm-2">
                            <input id="segundoApellidoCliente" placeholder="Segundo apellido" class="form-control" type="text" required="required" data-parsley-group="informacionPersonal"/>
                        </div>
                        <div class="col-sm-1"></div>
                    </div>
                    <!--PROFESION, NACIONALIDAD, FECHA DE NACIMIENTO-->
                    <div class="form-group row justify-content-md-center">
                        <label class="col-sm-1 col-form-label">No. Identidad</label>
                        <div class="col-sm-2">
                            <input id="identidadCliente" class="form-control mascara-identidad" type="text" required="required" data-parsley-group="informacionPersonal"/>
                        </div>
                        <label class="col-sm-1 col-form-label">Profesión</label>
                        <div class="col-sm-2">
                            <input id="profesion" class="form-control" type="text" required="required" data-parsley-group="informacionPersonal"/>
                        </div>
                        <label class="col-sm-2 col-form-label">Nacionalidad</label>
                        <div class="col-sm-2">
                            <select name="nacionalidad" id="nacionalidad" class="form-control buscadorddl" required="required" data-parsley-group="informacionPersonal">
                                <option value="">Seleccione una opción</option>
                            </select>
                        </div>
                    </div>
                    <!--EMAIL, TELEFONO,TIPO DE VIVIENDA-->
                    <div class="form-group row justify-content-md-center">
                        <label class="col-sm-1 col-form-label">Email</label>
                        <div class="col-sm-2">
                            <input id="correoElectronico" class="form-control" type="email" required="required" data-parsley-group="informacionPersonal"/>
                        </div>
                        <label class="col-sm-1 col-form-label">Télefono</label>
                        <div class="col-sm-2">
                            <input id="numeroTelefono" class="form-control mascara-telefono" type="text" required="required" data-parsley-group="informacionPersonal"/>
                        </div>
                        <label class="col-sm-2 col-form-label">Fecha de Nacimiento</label>
                        <div class="col-sm-2">
                            <input id="fechaNacimiento" class="form-control datepicker" type="date" required="required" data-parsley-group="informacionPersonal"/>
                        </div>
                    </div>
                    <!--SEXO-->
                    <div class="form-group row justify-content-md-center">
                        <div class="col-sm-10 border border-gray">
                            <label class="col-form-label col-sm-2">Sexo</label>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="sexo" value="M" required="required" data-parsley-errors-container="#error-sexo" data-parsley-group="informacionPersonal"/>
                                <label class="form-check-label">Masculino</label>
                            </div>
                            <div class="form-check form-check-inline">
                                <input class="form-check-input" type="radio" name="sexo" value="F" required="required" data-parsley-errors-container="#error-sexo" data-parsley-group="informacionPersonal"/>
                                <label class="form-check-label">Femenino</label>
                            </div>
                            <label class="col-form-label col-sm-2" id="error-sexo"></label>
                        </div>
                    </div>
                    <!--INPUTS RADIO DE ESTADO CIVIL-->
                    <div class="form-group row justify-content-md-center">
                        <div class="col-sm-10 border border-gray" id="divEstadoCivil">
                            <label class="col-form-label col-sm-2">Estado civil</label>
                            <label class="col-form-label" id="error-estadoCivil"></label>
                        </div>
                    </div>
                    <!--VIVIENDA, TIEMPO DE RESIDIR-->
                    <div class="form-group row justify-content-md-center">
                        <div class="col-sm-10 border border-gray">
                            <!--VIVIENDA-->
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
                                        <input class="form-check-input" type="radio" name="tiempoResidir" value="0" required="required" data-parsley-errors-container="#error-tiempoResidir" data-parsley-group="informacionPersonal"/>
                                        <label class="form-check-label">-1 año</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input" type="radio" name="tiempoResidir" value="1" required="required" data-parsley-errors-container="#error-tiempoResidir" data-parsley-group="informacionPersonal"/>
                                        <label class="form-check-label">1 año</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input" type="radio" name="tiempoResidir" value="2" required="required" data-parsley-errors-container="#error-tiempoResidir" data-parsley-group="informacionPersonal"/>
                                        <label class="form-check-label">2 años</label>
                                    </div>
                                    <div class="form-check form-check-inline">
                                        <input class="form-check-input" type="radio" name="tiempoResidir" value="3" required="required" data-parsley-errors-container="#error-tiempoResidir" data-parsley-group="informacionPersonal"/>
                                        <label class="form-check-label">+2 años</label>
                                    </div>
                                    <label class="col-form-label col-sm-2" id="error-tiempoResidir"></label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- INFORMACION DOMICILIAR -->
                <div id="step-2" class="form-section">
                    <h6 class="border-bottom border-gray pb-2">Información domiciliar (Aval)</h6>

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
                        <label class="col-sm-3 col-form-label">Ciudad</label>
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
                            <input id="telefonoCasa" class="form-control mascara-telefono" type="text" data-parsley-group="informacionDomiciliar"/>
                        </div>
                        <label class="col-sm-3 col-form-label">Telefono movil</label>
                        <div class="col-sm-3">
                            <input id="telefonoMovil" class="form-control mascara-telefono" type="text" required="required" data-parsley-group="informacionDomiciliar"/>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Detalle dirección</label>
                        <div class="col-sm-3">
                            <input placeholder="Calle, avenida, bloque, etc" id="direccionDetallada" class="form-control" type="text" required="required" data-parsley-group="informacionDomiciliar"/>
                        </div>
                        <div class="col-sm-3"></div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-12 col-form-label text-center">Referencias del domicilio</label>
                        <div class="col-sm-12">
                            <textarea id="referenciaDireccionDetallada" required="required" class="form-control" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2" data-parsley-group="informacionDomiciliar"></textarea>
                        </div>
                    </div>
                </div>

                <!-- INFORMACION LABORAL -->
                <div id="step-3" class="form-section">
                    <h6 class="border-bottom border-gray pb-2">Información laboral (Aval)</h6>

                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Nombre del trabajo</label>
                        <div class="col-sm-3">
                            <input id="nombreDelTrabajo" name="nombreDelTrabajo" class="form-control" type="text" required="required" data-parsley-group="informacionLaboral"/>
                        </div>
                        <label class="col-sm-3 col-form-label">Ingresos mensuales</label>
                        <div class="col-sm-3">
                            <input id="ingresosMensuales" class="form-control mascara-cantidad" type="text" required="required" data-parsley-group="informacionLaboral"/>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Puesto asignado</label>
                        <div class="col-sm-3">
                            <input id="puestoAsignado" name="puestoAsignado" class="form-control" type="text" required="required" data-parsley-group="informacionLaboral"/>
                        </div>

                        <label class="col-sm-3 col-form-label">Fecha de ingreso</label>
                        <div class="col-sm-3">
                            <input id="fechaIngreso" name="fechaIngreso" class="form-control datepicker" type="date" required="required" data-parsley-group="informacionLaboral"/>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Telefono de la empresa</label>
                        <div class="col-sm-3">
                            <input id="telefonoEmpresa" name="telefonoEmpresa" class="form-control mascara-telefono" type="text" required="required" data-parsley-group="informacionLaboral"/>
                        </div>
                        <label class="col-sm-2 col-form-label">Extension RRHH</label>
                        <div class="col-sm-1">
                            <input id="extensionRRHH" name="extensionRRHH" class="form-control mascara-extension" type="text" data-parsley-group="informacionLaboral" data-parsley-required-message="Requerido"/>
                        </div>
                        <label class="col-sm-2 col-form-label">Extension cliente</label>
                        <div class="col-sm-1">
                            <input id="extensionCliente" name="extensionCliente" class="form-control mascara-extension" type="text" data-parsley-group="informacionLabral" data-parsley-required-message="Requerido"/>
                        </div>
                        <!-- AQUI TEMRMINA INFO GENERAL Y EMPIEZA UBICACION DE LA EMPRESA -->
                    </div>
                    <div class="form-group row">
                        <div class="col-sm-12">
                            <hr />
                            <h6 class="">Ubicación Empresa</h6>
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
                        <label class="col-sm-3 col-form-label">Ciudad empresa</label>
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
                            <input placeholder="Calle, avenida, bloque, etc" id="direccionDetalladaEmpresa" name="direccionDetalladaEmpresa" class="form-control" type="text" required="required" data-parsley-group="informacionLaboral"/>
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
                            <input id="fuenteOtrosIngresos" name="fuenteOtrosIngresos" class="form-control" type="text" data-parsley-group="informacionLaboral"/>
                        </div>
                        <label class="col-sm-3 col-form-label">Valor otros ingresos</label>
                        <div class="col-sm-3">
                            <input id="valorOtrosIngresos" class="form-control mascara-cantidad" type="text" data-parsley-group="informacionLaboral"/>
                        </div>
                    </div>
                </div>

                <!-- INFORMACIÓN CONYUGAL -->
                <div id="step-4" class="form-section">
                    <h6 class="border-bottom border-gray pb-2">Información conyugal (Aval)</h6>

                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Nombres del conyugue</label>
                        <div class="col-sm-3">
                            <input id="nombresConyugue" class="form-control infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal"/>
                        </div>
                        <label class="col-sm-3 col-form-label">Apellidos del conyugue</label>
                        <div class="col-sm-3">
                            <input id="apellidosConyugue" class="form-control infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal"/>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Identidad conyugue</label>
                        <div class="col-sm-3">
                            <input id="identidadConyugue" class="form-control infoConyugal mascara-identidad" type="text" required="required" data-parsley-group="informacionConyugal"/>
                        </div>
                        <label class="col-sm-3 col-form-label">Fecha nacimiento</label>
                        <div class="col-sm-3">
                            <input id="fechaNacimientoConyugue" class="form-control infoConyugal datepicker" type="date" required="required" data-parsley-group="informacionConyugal"/>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Telefono del conyugue</label>
                        <div class="col-sm-3">
                            <input id="telefonoConyugue" class="form-control infoConyugal mascara-telefono" type="text" required="required" data-parsley-group="informacionConyugal"/>
                        </div>
                        <label class="col-sm-3 col-form-label">Lugar de trabajo</label>
                        <div class="col-sm-3">
                            <input id="lugarTrabajoConyugue" class="form-control infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal"/>
                        </div>
                    </div>
                    <div class="form-group row">
                        <label class="col-sm-3 col-form-label">Ingreso mensual</label>
                        <div class="col-sm-3">
                            <input id="ingresoMensualesConyugue" class="form-control infoConyugal mascara-cantidad" type="text" required="required" data-parsley-group="informacionConyugal"/>
                        </div>
                        <label class="col-sm-3 col-form-label">Telefono trabajo </label>
                        <div class="col-sm-3">
                            <input id="telefonoTrabajoConyugue" class="form-control infoConyugal mascara-telefono" type="text" required="required" data-parsley-group="informacionConyugal"/>
                        </div>
                    </div>
                </div>

                <!-- DOCUMENTACION -->
                <div id="step-5" class="form-section">
                    <h6 class="border-bottom border-gray pb-2">Documentación (Aval)</h6>

                    <div class="form-group row text-center justify-content-md-center">
                        <div class="col-sm-2">
                            <label class="form-label">Cédula Identidad</label>
                            <form action="#" method="post" enctype="multipart/form-data">
                                <input type="file" name="files" id="fileCedula"/>
                            </form>
                        </div>
                        <div class="col-sm-2">
                            <label class="form-label">Comp. Domicilio</label>
                            <form action="#" method="post" enctype="multipart/form-data">
                                <input type="file" name="files" id="fileCompDomicilio"/>
                            </form>
                        </div>
                        <div class="col-sm-2">
                            <label class="form-label">Comp. Ingresos</label>
                            <form action="#" method="post" enctype="multipart/form-data">
                                <input type="file" name="files" id="compIngresos"/>
                            </form>
                        </div>
                        <div class="col-sm-2">
                            <label class="form-label">Croquis Domicilio</label>
                            <form action="#" method="post" enctype="multipart/form-data">
                                <input type="file" name="files" id="croquisDomicilio"/>
                            </form>
                        </div>
                        <div class="col-sm-2">
                            <label class="form-label">Croquis Empleo</label>
                            <form action="#" method="post" enctype="multipart/form-data">
                                <input type="file" name="files" id="croquisEmpleo"/>
                            </form>
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
        });
    </script>
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/app/uploader/SolicitudesCredito_RegistrarAval/jquery.fileuploader.min.js"></script>
    <script src="/Scripts/app/uploader/SolicitudesCredito_RegistrarAval/custom.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/plugins/datapicker/bootstrap-datepicker.js"></script>
    <script src="/Scripts/app/Aval/Aval_Actualizar.js"></script>
</body>
</html>