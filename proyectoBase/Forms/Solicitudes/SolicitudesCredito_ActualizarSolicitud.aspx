<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SolicitudesCredito_ActualizarSolicitud.aspx.cs" Inherits="SolicitudesCredito_ActualizarSolicitud" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml" lang="es">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, user-scalable=0, minimal-ui" />
    <title>Actualizar solicitud de crédito</title>
    <link href="/Content/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/Content/css/style.css?v=202010031033" rel="stylesheet" />
    <link href="/Content/css/icons.css?v=202010031033" rel="stylesheet" />
    <link href="/Scripts/plugins/iziToast/css/iziToast.min.css" rel="stylesheet" />
    <link href="/Scripts/plugins/steps/css/smart_wizard.css" rel="stylesheet" />
    <link href="/Content/css/font/font-fileuploader.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader.min.css" rel="stylesheet" />
    <link href="/Content/css/jquery.fileuploader-theme-dragdrop.css" rel="stylesheet" />
    <link href="/Scripts/plugins/select2/css/select2.min.css" rel="stylesheet" />
    <link href="/CSS/Estilos_CSS.css" rel="stylesheet" />
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
    <form runat="server" id="frmSolicitud" class="" action="#" data-parsley-excluded="[disabled]">
        <div class="card mb-0">
            <div class="card-header pb-1 pt-1">
                <h5>Actualizar solicitud de crédito <small><span runat="server" id="lblMensaje" class="text-danger" visible="false"></span></small></h5>
            </div>
            <div class="card-body">
                <div id="smartwizard" class="h-100">
                    <ul>
                        <li runat="server" id="liInformacionPrestamo"><a href="#step-1" class="pt-3 pb-2 font-12">Condicionamientos</a></li>
                        <li runat="server" visible="true" id="liInformacionPersonal"><a href="#step-2" class="pt-3 pb-2 font-12">Información personal</a></li>
                        <li runat="server" visible="true" id="liInformacionDomicilio"><a href="#step-3" class="pt-3 pb-2 font-12">Información domicilio</a></li>
                        <li runat="server" visible="true" id="liInformacionLaboral"><a href="#step-4" class="pt-3 pb-2 font-12">Información laboral</a></li>
                        <li runat="server" visible="true" id="liInformacionConyugal"><a href="#step-5" class="pt-3 pb-2 font-12">Información conyugal</a></li>
                        <li runat="server" visible="true" id="liReferenciasPersonales"><a href="#step-6" class="pt-3 pb-2 font-12">Referencias personales</a></li>
                        <li runat="server" visible="true" id="liDocumentacion"><a href="#step-7" class="pt-3 pb-2 font-12">Documentación</a></li>
                    </ul>
                    <div>
                        <!-- Listado de condiciones de la solicitud -->
                        <div id="step-1" class="form-section">

                            <!-- loader -->
                            <div class="float-right" id="divLoader" runat="server" visible="false">
                                <div class="spinner-border" role="status">
                                    <span class="sr-only"></span>
                                </div>
                            </div>
                            <div class="form-group row m-0 border-bottom border-gray">
                                <div class="col-12 p-0">
                                    <h6 class="mt-1">Condiciones de la solcitud</h6>
                                </div>
                            </div>

                            <div class="form-group row">
                                <div class="col-12">
                                    <div class="table-responsive">
                                        <table id="tblCondiciones" class="table tabla-compacta table-striped table-bordered" runat="server">
                                            <thead>
                                                <tr>
                                                    <th>Tipo de condición</th>
                                                    <th>Descripción</th>
                                                    <th>Comentario adicional</th>
                                                    <th>Estado</th>
                                                    <%--<th>Acciones</th>--%>
                                                </tr>
                                            </thead>
                                            <tbody>
                                            </tbody>
                                        </table>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-12">
                                            <label class="col-form-label">Otras</label>
                                            <asp:TextBox ID="txtOtrasCondiciones" CssClass="form-control form-control-sm" type="text" ReadOnly="true" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Información personal del cliente -->
                        <div id="step-2" class="form-section">

                            <div class="form-group row m-0 border-bottom border-gray justify-content-between">
                                <div class="col-auto p-0">
                                    <h6 class="mt-1">Actualizar información personal</h6>
                                </div>
                                <div class="col-auto p-0 mb-1">
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_InfoPersonal">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-md-6 border-right border-gray">
                                    <div class="form-group row">
                                        <div class="col-6">
                                            <label class="col-form-label">No. Identidad</label>
                                            <asp:TextBox ID="txtIdentidadCliente" CssClass="form-control form-control-sm mascara-identidad" type="text" Enabled="false" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-6">
                                            <label class="col-form-label">RTN numérico</label>
                                            <asp:TextBox ID="txtRtnCliente" CssClass="form-control form-control-sm mascara-rtn" type="text" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Primer nombre</label>
                                            <asp:TextBox ID="txtPrimerNombre" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Segundo nombre</label>
                                            <asp:TextBox ID="txtSegundoNombre" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Primer apellido</label>
                                            <asp:TextBox ID="txtPrimerApellido" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Segundo apellido</label>
                                            <asp:TextBox ID="txtSegundoApellido" CssClass="form-control form-control-sm" type="text" Enabled="false" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Nacionalidad</label>
                                            <asp:DropDownList ID="ddlNacionalidad" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlNacionalidad"></asp:DropDownList>
                                            <div id="error-ddlNacionalidad"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Tipo de cliente</label>
                                            <asp:DropDownList ID="ddlTipoDeCliente" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlTipoDeCliente"></asp:DropDownList>
                                            <div id="error-ddlTipoDeCliente"></div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-md-4">
                                            <label class="col-form-label">Fecha de nacimiento</label>
                                            <asp:TextBox ID="txtFechaDeNacimiento" CssClass="form-control form-control-sm" type="date" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="col-form-label">Edad del cliente</label>
                                            <asp:TextBox ID="txtEdadDelCliente" CssClass="form-control form-control-sm" Enabled="false" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <label class="col-form-label">Sexo</label>
                                            <div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" type="radio" name="sexoCliente" value="M" runat="server" id="rbSexoMasculino" required="required" data-parsley-errors-container="#error-sexo" data-parsley-group="informacionPersonal" />
                                                    <label class="form-check-label" for="rbSexoMasculino">Masculino</label>
                                                </div>
                                                <div class="form-check form-check-inline">
                                                    <input class="form-check-input" type="radio" name="sexoCliente" value="F" runat="server" id="rbSexoFemenino" required="required" data-parsley-errors-container="#error-sexo" data-parsley-group="informacionPersonal" />
                                                    <label class="form-check-label" for="rbSexoFemenino">Femenino</label>
                                                </div>
                                            </div>
                                            <div id="error-sexo"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-6">

                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Email</label>
                                            <asp:TextBox ID="txtCorreoElectronico" CssClass="form-control form-control-sm" type="email" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Télefono</label>
                                            <asp:TextBox ID="txtNumeroTelefono" CssClass="form-control form-control-sm mascara-telefono" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Tipo de Vivienda</label>
                                            <asp:DropDownList ID="ddlTipoDeVivienda" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlTipoDeVivienda"></asp:DropDownList>
                                            <div id="error-ddlTipoDeVivienda"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Tiempo de residir</label>
                                            <asp:DropDownList ID="ddlTiempoDeResidir" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlTiempoDeResidir"></asp:DropDownList>
                                            <div id="error-ddlTiempoDeResidir"></div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Estado civil</label>
                                            <asp:DropDownList ID="ddlEstadoCivil" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionPersonal" data-parsley-errors-container="#error-ddlEstadoCivil"></asp:DropDownList>
                                            <div id="error-ddlEstadoCivil"></div>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Profesión</label>
                                            <asp:TextBox ID="txtProfesion" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="informacionPersonal" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Información del domicilio del cliente -->
                        <div id="step-3" class="form-section">

                            <div class="form-group row m-0 border-bottom border-gray justify-content-between">
                                <div class="col-auto p-0">
                                    <h6 class="mt-1">Actualizar información del domicilio</h6>
                                </div>
                                <div class="col-auto p-0 mb-1">
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_InfoDomicilio">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-12">
                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Departamento</label>
                                            <asp:DropDownList ID="ddlDepartamentoDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlDepartamentoDomicilio"></asp:DropDownList>
                                            <div id="error-ddlDepartamentoDomicilio"></div>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Municipio</label>
                                            <asp:DropDownList ID="ddlMunicipioDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlMunicipioDomicilio"></asp:DropDownList>
                                            <div id="error-ddlMunicipioDomicilio"></div>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Ciudad/Poblado</label>
                                            <asp:DropDownList ID="ddlCiudadPobladoDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlCiudadPobladoDomicilio"></asp:DropDownList>
                                            <div id="error-ddlCiudadPobladoDomicilio"></div>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Barrio/Colonia</label>
                                            <asp:DropDownList ID="ddlBarrioColoniaDomicilio" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionDomicilio" data-parsley-errors-container="#error-ddlBarrioColoniaDomicilio"></asp:DropDownList>
                                            <div id="error-ddlBarrioColoniaDomicilio"></div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-12">
                                    <h6 class="border-top border-gray"></h6>
                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Teléfono casa</label>
                                            <asp:TextBox ID="txtTelefonoCasa" CssClass="form-control form-control-sm mascara-telefono" type="text" data-parsley-group="informacionDomicilio" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-9">
                                            <label class="col-form-label">Dirección detallada del domicilio</label>
                                            <asp:TextBox ID="txtDireccionDetalladaDomicilio" CssClass="form-control form-control-sm" placeholder="# calle, # avenida, bloque, pasaje, etc" type="text" required="required" data-parsley-group="informacionDomicilio" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-12">
                                            <label class="col-form-label">Referencias del domicilio</label>
                                            <textarea id="txtReferenciasDelDomicilio" runat="server" required="required" class="form-control form-control-sm" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2" data-parsley-group="informacionDomicilio"></textarea>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Información laboral del cliente -->
                        <div id="step-4" class="form-section">
                            <div class="form-group row m-0 border-bottom border-gray justify-content-between">
                                <div class="col-auto p-0">
                                    <h6 class="mt-1">Actualizar información laboral</h6>
                                </div>
                                <div class="col-auto p-0 mb-1">
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_InfoLaboral">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-md-8 border-right border-gray">
                                    <h6 class="mt-3 mb-1">Información general</h6>
                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Nombre del trabajo</label>
                                            <asp:TextBox ID="txtNombreDelTrabajo" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Fecha de ingreso</label>
                                            <asp:TextBox ID="txtFechaDeIngreso" CssClass="form-control form-control-sm" type="date" required="required" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Puesto asignado</label>
                                            <asp:TextBox ID="txtPuestoAsignado" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Ingresos mensuales</label>
                                            <asp:TextBox ID="txtIngresosMensuales" CssClass="form-control form-control-sm mascara-cantidad" ReadOnly="true" type="text" required="required" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Teléfono de la empresa</label>
                                            <asp:TextBox ID="txtTelefonoEmpresa" CssClass="form-control form-control-sm mascara-telefono" type="text" required="required" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Extensión RRHH</label>
                                            <asp:TextBox ID="txtExtensionRecursosHumanos" CssClass="form-control form-control-sm mascara-extension" type="text" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-3">
                                            <label class="col-form-label">Extensión Cliente</label>
                                            <asp:TextBox ID="txtExtensionCliente" CssClass="form-control form-control-sm mascara-extension" type="text" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <h6 class="mt-3 mb-1">Fuente de otros ingresos</h6>
                                    <div class="form-group row">
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Fuente de otros ingresos</label>
                                            <asp:TextBox ID="txtFuenteDeOtrosIngresos" CssClass="form-control form-control-sm" type="text" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="col-sm-6">
                                            <label class="col-form-label">Valor otros ingresos</label>
                                            <asp:TextBox ID="txtValorOtrosIngresos" CssClass="form-control form-control-sm mascara-cantidad" type="text" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Dirección del trabajo -->
                            <h6 class="mb-1 border-top border-gray pt-2">Dirección del trabajo</h6>

                            <div class="form-group row">
                                <div class="col-sm-3">
                                    <label class="col-form-label">Departamento</label>
                                    <asp:DropDownList ID="ddlDepartamentoEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlDepartamentoEmpresa"></asp:DropDownList>
                                    <div id="error-ddlDepartamentoEmpresa"></div>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Municipio</label>
                                    <asp:DropDownList ID="ddlMunicipioEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlMunicipioEmpresa"></asp:DropDownList>
                                    <div id="error-ddlMunicipioEmpresa"></div>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Ciudad/Poblado</label>
                                    <asp:DropDownList ID="ddlCiudadPobladoEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlCiudadPobladoEmpresa"></asp:DropDownList>
                                    <div id="error-ddlCiudadPobladoEmpresa"></div>
                                </div>
                                <div class="col-sm-3">
                                    <label class="col-form-label">Barrio/Colonia</label>
                                    <asp:DropDownList ID="ddlBarrioColoniaEmpresa" runat="server" CssClass="form-control form-control-sm buscadorddl" Enabled="false" required="required" data-parsley-group="informacionLaboral" data-parsley-errors-container="#error-ddlBarrioColoniaEmpresa"></asp:DropDownList>
                                    <div id="error-ddlBarrioColoniaEmpresa"></div>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-12">
                                    <label class="col-form-label">Dirección detallada del trabajo</label>
                                    <asp:TextBox ID="txtDireccionDetalladaEmpresa" CssClass="form-control form-control-sm" placeholder="# calle, # avenida, bloque, pasaje, etc" type="text" required="required" data-parsley-group="informacionLaboral" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-12">
                                    <label class="col-form-label">Referencias de la ubicación del trabajo</label>
                                    <textarea id="txtReferenciasEmpresa" runat="server" required="required" class="form-control form-control-sm" data-parsley-maxlength="255" data-parsley-minlength="15" rows="2" data-parsley-group="informacionLaboral"></textarea>
                                </div>
                            </div>
                        </div>

                        <!-- Información del conyugue -->
                        <div id="step-5" class="form-section">
                            <div class="form-group row m-0 border-bottom border-gray justify-content-between">
                                <div class="col-auto p-0">
                                    <h6 class="mt-1">Información del cónyugue</h6>
                                </div>
                                <div class="col-auto p-0 mb-1">
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_InfoConyugue">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-4">
                                    <label class="col-form-label">Identidad</label>
                                    <asp:TextBox ID="txtIdentidadConyugue" CssClass="form-control form-control-sm mascara-identidad infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-8">
                                    <label class="col-form-label">Nombres del cónyugue</label>
                                    <asp:TextBox ID="txtNombresConyugue" CssClass="form-control form-control-sm infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group row">
                                <div class="col-sm-4">
                                    <label class="col-form-label">Fecha de nacimiento</label>
                                    <asp:TextBox ID="txtFechaNacimientoConyugue" CssClass="form-control form-control-sm infoConyugal" type="date" required="required" data-parsley-group="informacionConyugal" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <label class="col-form-label">Teléfono</label>
                                    <asp:TextBox ID="txtTelefonoConyugue" CssClass="form-control form-control-sm mascara-telefono infoConyugal" type="text" required="required" data-parsley-group="informacionConyugal" runat="server"></asp:TextBox>
                                </div>
                            </div>

                            <!-- Información del trabajo del conyugue -->
                            <h6 class="mb-1 border-top border-gray pt-2">Información del trabajo (opcional)</h6>

                            <div class="form-group row">
                                <div class="col-sm-4">
                                    <label class="col-form-label">Lugar de trabajo</label>
                                    <asp:TextBox ID="txtLugarDeTrabajoConyuge" CssClass="form-control form-control-sm infoConyugal" type="text" data-parsley-group="informacionConyugal" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <label class="col-form-label">Ingreso mensual</label>
                                    <asp:TextBox ID="txtIngresosMensualesConyugue" CssClass="form-control form-control-sm mascara-cantidad infoConyugal" type="text" data-parsley-group="informacionConyugal" runat="server"></asp:TextBox>
                                </div>
                                <div class="col-sm-4">
                                    <label class="col-form-label">Teléfono del trabajo</label>
                                    <asp:TextBox ID="txtTelefonoTrabajoConyugue" CssClass="form-control form-control-sm mascara-telefono infoConyugal" type="text" data-parsley-group="informacionConyugal" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <!-- Referencias personales del cliente -->
                        <div id="step-6" class="form-section">
                            <div class="form-group row m-0 border-bottom border-gray justify-content-between">
                                <div class="col-auto p-0">
                                    <h6 class="mt-1">Referencias personales del cliente</h6>
                                </div>
                                <div class="col-auto p-0 mb-1">
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_Referencias">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>

                            <button type="button" id="btnAgregarReferencia" class="btn btn-info mb-1 mt-1">Nuevo</button>

                            <div class="table-responsive">
                                <table class="table tabla-compacta table-striped table-bordered" id="tblReferenciasPersonales" runat="server">
                                    <thead>
                                        <tr>
                                            <th>Nombre completo</th>
                                            <th>Telefono</th>
                                            <th>Lugar de trabajo</th>
                                            <th>Tiempo de conocer</th>
                                            <th>Parentesco</th>
                                            <th>Acciones</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                    </tbody>
                                </table>
                            </div>
                        </div>

                        <!-- Documentación de la solicitud -->
                        <div id="step-7" class="form-section">
                            <div class="form-group row m-0 border-bottom border-gray justify-content-between">
                                <div class="col-auto p-0">
                                    <h6 class="mt-1">Documentación de la solicitud <small class="text-info">(Estimado usuario, recuerda subir toda la documentación hasta que ya vayas a guardar la solicitud)</small></h6>
                                </div>
                                <div class="col-auto p-0 mb-1">
                                    <button type="button" class="btn btn-warning pt-1 pb-1" id="btnFinalizarCondiciones_Documentacion">
                                        <i class="far fa-check-square"></i>
                                        Finalizar condiciones
                                    </button>
                                </div>
                            </div>
                            <!-- Div donde se generan dinamicamente los inputs para la documentación -->
                            <div class="row pr-1 pl-1 text-center" id="DivDocumentacion">
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalDetallesCondicion" class="modal fade" role="dialog" aria-labelledby="modalDetallesCondicionLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalDetallesCondicionLabel">Detalles de la condición</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtComentarioAdicionalCondicion" runat="server" class="form-control form-control-sm" rows="3"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cerrar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalFinalizarCondicion" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="modalFinalizarCondicionListaLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pt-2 pb-2">
                        <h6 class="modal-title" id="modalFinalizarCondicionListaLabel">Finalizar condicionamientos (<b id="lblSeccion"></b>)</h6>
                    </div>
                    <div class="modal-body">
                        <label>¿Está seguro de finalizar <b>TODAS</b> las condiciones de esta sección?</label>
                    </div>
                    <div class="modal-footer">
                        <button id="btnTerminarCondicionConfirmar" data-dismiss="modal" class="btn btn-primary waves-effect">
                            Confirmar
                        </button>
                        <button data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!--modal de agregar nueva referencia -->
        <div class="modal fade" id="modalAgregarReferenciaPersonal" tabindex="-1" role="dialog" aria-labelledby="modalAgregarReferenciaPersonalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header pb-1 pt-1">
                        <h6 class="modal-title" id="modalAgregarReferenciaPersonalLabel">Agregar referencia personal</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Nombre completo</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtNombreReferencia" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="referenciasPersonales" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Telefono</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtTelefonoReferencia" CssClass="form-control form-control-sm mascara-telefono" type="text" required="required" data-parsley-group="referenciasPersonales" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Tiempo de conocer</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlTiempoDeConocerReferencia" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonales" data-parsley-errors-container="#error-ddlTiempoDeConocerReferencia"></asp:DropDownList>
                                <div id="error-ddlTiempoDeConocerReferencia"></div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Parentesco</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlParentescos" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonales" data-parsley-errors-container="#error-ddlParentescos"></asp:DropDownList>
                                <div id="error-ddlParentescos"></div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Lugar de trabajo</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtLugarTrabajoReferencia" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="referenciasPersonales" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer pt-2 pb-2">
                        <button id="btnAgregarReferenciaTabla" type="button" class="btn btn-primary waves-effect waves-light mr-1">
                            Agregar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div id="modalEliminarReferenciaPersonal" class="modal fade" role="dialog" aria-labelledby="modalEliminarReferenciaPersonalLabel" aria-hidden="true">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h6 class="modal-title mt-0" id="modalEliminarReferenciaPersonalLabel">Eliminar referencia personal</h6>
                        <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesEliminarReferenciaPersonal" runat="server" class="form-control form-control-sm" required="required" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button id="btnEliminarReferenciaPersonalConfirmar" type="button" class="btn btn-danger waves-effect waves-light">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <div class="modal fade" id="modalEditarReferenciaPersonal" tabindex="-1" role="dialog" aria-labelledby="modalEditarReferenciaPersonalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header pb-1 pt-1">
                        <h6 class="modal-title" id="modalEditarReferenciaPersonalLabel">Editar referencia personal</h6>
                    </div>
                    <div class="modal-body">
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Nombre completo</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtNombreReferenciaPersonal_Editar" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="referenciasPersonalesEditar" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Telefono</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtTelefonoReferenciaPersonal_Editar" CssClass="form-control form-control-sm mascara-telefono" type="text" required="required" data-parsley-group="referenciasPersonalesEditar" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Tiempo de conocer</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlTiempoDeConocerReferencia_Editar" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonalesEditar" data-parsley-errors-container="#error-ddlTiempoDeConocerReferencia_Editar"></asp:DropDownList>
                                <div id="error-ddlTiempoDeConocerReferencia_Editar"></div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Parentesco</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlParentesco_Editar" runat="server" CssClass="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonalesEditar" data-parsley-errors-container="#error-ddlParentesco_Editar"></asp:DropDownList>
                                <div id="error-ddlParentesco_Editar"></div>
                            </div>
                        </div>
                        <div class="form-group row">
                            <label class="col-sm-4 col-form-label">Lugar de trabajo</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtLugarDeTrabajoReferencia_Editar" CssClass="form-control form-control-sm" type="text" required="required" data-parsley-group="referenciasPersonalesEditar" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group row">
                            <div class="col-12">
                                <label class="col-form-label">Observaciones</label>
                                <textarea id="txtObservacionesEditarReferenciaPersonal" runat="server" class="form-control form-control-sm" required="required" data-parsley-group="referenciasPersonalesEditar" data-parsley-maxlength="150" data-parsley-minlength="15" rows="2"></textarea>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer pt-2 pb-2">
                        <button id="btnEditarReferenciaConfirmar" type="button" class="btn btn-primary waves-effect waves-light mr-1">
                            Confirmar
                        </button>
                        <button type="reset" data-dismiss="modal" class="btn btn-secondary waves-effect">
                            Cancelar
                        </button>
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
            $(".mascara-extension").inputmask("999999");
            $(".mascara-identidad").inputmask("9999999999999");
            $(".mascara-rtn").inputmask("99999999999999");
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
        });

        const ID_SOLICITUD = <%=this.IdSolicitud%>;
        const ID_CLIENTE = <%=this.IdCliente%>;
    </script>
    <script src="/Scripts/plugins/steps/js/jquery.smartWizard.js"></script>
    <script src="/Scripts/plugins/iziToast/js/iziToast.min.js"></script>
    <script src="/Scripts/plugins/parsleyjs/parsley.js"></script>
    <script src="/Scripts/app/uploader/js/jquery.fileuploader.min.js"></script>
    <script src="/Scripts/plugins/select2/js/select2.full.min.js"></script>
    <script src="/Scripts/app/solicitudes/SolicitudesCredito_ActualizarSolicitud.js?v=202000930092758"></script>
</body>
</html>
